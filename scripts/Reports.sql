/* Number of sessions by day of week */
select datename(weekday, sessiondate) as Weekday, count(sessionid) as '# of Sessions' from Sessions
where not (note is not null and note like '%cancelled%')
group by datename(weekday, sessiondate)
order by count(sessionid) desc

/* Number of sessions by month */
select datename(month, sessiondate) as Month, count(sessionid) as '# of Sessions' from Sessions
where not (note is not null and note like '%cancelled%')
group by datename(month, sessiondate), month(sessiondate)
order by count(sessionid) desc

/* Number of sessions by year */
select year(sessiondate) as Year, count(sessionid) as '# of Sessions' from Sessions
where not (note is not null and note like '%cancelled%')
group by year(sessiondate)
order by year(sessiondate)

/* Recent Activities, shows SessionDate column for context */
SELECT TOP 100 ActivityLogs.*, Sessions.SessionDate from ActivityLogs
INNER JOIN Sessions ON ActivityLogs.SessionId = sessions.SessionId
ORDER BY ActivityLogs.CreateDateTime DESC
