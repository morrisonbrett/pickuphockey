/* Recent Activities, shows SessionDate column for context */
SELECT TOP 200 ActivityLogs.*, Sessions.SessionDate from ActivityLogs
INNER JOIN Sessions ON ActivityLogs.SessionId = sessions.SessionId
ORDER BY ActivityLogs.CreateDateTime DESC

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
WHERE BuyerUserId != SellerUserId
GROUP BY SellerUserId, FirstName, LastName
ORDER BY COUNT(SellerUserId) DESC

/* Top Sellers in 2022 */
SELECT T1.FirstName, T1.LastName, T1.SellerCount, T2.SessionCount, CAST((CAST(T1.SellerCount AS decimal) / CAST(T2.SessionCount AS decimal)) AS decimal(18,4)) * 100 AS SellingPercentage, T2.Year, T2.Weekday FROM
(SELECT SellerUserId, FirstName, LastName, COUNT(SellerUserId) AS SellerCount from BuySells
INNER JOIN AspNetUsers on SellerUserId = AspNetUsers.id
INNER JOIN Sessions on BuySells.SessionId = Sessions.SessionId
LEFT OUTER JOIN SessionsByDate on YEAR(Sessions.SessionDate) = YEAR(SessionsByDate.Year)
WHERE YEAR(Sessions.SessionDate) = 2019 AND DATENAME(WeekDay, Sessions.SessionDate) = 'Friday'
AND BuyerUserId != SellerUserId
GROUP BY SellerUserId, FirstName, LastName
) AS T1,
(SELECT * from SessionsByDate WHERE YEAR = 2019 AND SessionsByDate.Weekday = 'Friday') AS T2
ORDER BY T1.SellerCount DESC

/* Top Buyers */
SELECT BuyerUserId, FirstName, LastName, COUNT(BuyerUserId) AS BuyerCount from BuySells
INNER JOIN AspNetUsers on BuyerUserId = AspNetUsers.id
WHERE SellerUserId != BuyerUserId
GROUP BY BuyerUserId, FirstName, LastName
ORDER BY COUNT(BuyerUserId) DESC
