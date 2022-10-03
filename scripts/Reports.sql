select datename(weekday, sessiondate) as Weekday, count(sessionid) as '# of Sessions' from Sessions
where not (note is not null and note like '%cancelled%')
group by datename(weekday, sessiondate)
order by count(sessionid) desc

select datename(month, sessiondate) as Month, count(sessionid) as '# of Sessions' from Sessions
where not (note is not null and note like '%cancelled%')
group by datename(month, sessiondate), month(sessiondate)
order by count(sessionid) desc

select year(sessiondate) as Year, count(sessionid) as '# of Sessions' from Sessions
where not (note is not null and note like '%cancelled%')
group by year(sessiondate)
order by year(sessiondate)

