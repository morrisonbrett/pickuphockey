/* Number of sessions by day of week */
select datename(weekday, sessiondate) as Weekday, count(sessionid) as '# of Sessions' from Sessions
where note not like '%cancelled%'
group by datename(weekday, sessiondate)
order by count(sessionid) desc

/* Number of sessions by month */
select datename(month, sessiondate) as Month, count(sessionid) as '# of Sessions' from Sessions
where note not like '%cancelled%'
group by datename(month, sessiondate), month(sessiondate)
order by count(sessionid) desc

/* Number of sessions by year */
select year(sessiondate) as Year, count(sessionid) as '# of Sessions' from Sessions
where note not like '%cancelled%'
group by year(sessiondate)
order by year(sessiondate)

/* Top Sellers */
SELECT SellerUserId, FirstName, LastName, COUNT(SellerUserId) AS SellerCount from BuySells
INNER JOIN AspNetUsers on SellerUserId = AspNetUsers.id
GROUP BY SellerUserId, FirstName, LastName
ORDER BY COUNT(SellerUserId) DESC

/* Top Buyers */
SELECT BuyerUserId, FirstName, LastName, COUNT(BuyerUserId) AS BuyerCount from BuySells
INNER JOIN AspNetUsers on BuyerUserId = AspNetUsers.id
GROUP BY BuyerUserId, FirstName, LastName
ORDER BY COUNT(BuyerUserId) DESC

/* Recent Activities, shows SessionDate column for context */
SELECT TOP 200 ActivityLogs.*, Sessions.SessionDate from ActivityLogs
INNER JOIN Sessions ON ActivityLogs.SessionId = sessions.SessionId
ORDER BY ActivityLogs.CreateDateTime DESC
