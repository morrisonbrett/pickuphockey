/* Tune Indexes */
EXEC sp_updatestats;

/* Recent Activities, shows SessionDate column for context */
SELECT TOP 200 FirstName + ' ' + LastName AS Name, DATEADD(HH, -8, ActivityLogs.CreateDateTime) as 'CreateDateTime PST', Activity, DATENAME(WEEKDAY, SessionDate) AS SessionDay, ActivityLogs.SessionId, SessionDate, ActivityLogs.UserId from ActivityLogs
INNER JOIN Sessions ON ActivityLogs.SessionId = sessions.SessionId
INNER JOIN AspNetUsers ON AspNetUsers.Id = UserId
ORDER BY ActivityLogs.CreateDateTime DESC

/* Emergency Info */
SELECT FirstName, LastName, EmergencyName, EmergencyPhone FROM AspNetUsers WHERE EmergencyName IS NOT NULL OR EmergencyPhone IS NOT NULL

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

/* Number of sessions by month and year */
select datename(month, sessiondate) as Month, datename(year, sessiondate) as Year, count(sessionid) as '# of Sessions' from Sessions
where note not like '%cancelled%' -- and datename(month, sessiondate) = 'August'
group by datename(month, sessiondate), month(sessiondate), datename(year, sessiondate), year(sessiondate)
order by year(sessiondate), month(sessiondate)

/* Number of sessions by year */
select year(sessiondate) as Year, count(sessionid) as '# of Sessions' from Sessions
where note not like '%cancelled%'
group by year(sessiondate)
order by year(sessiondate)

/* Top Sellers - All time */
SELECT FirstName, LastName, COUNT(SellerUserId) AS SellerCount, MAX(SessionDate) As LastSessionSold, MIN(SessionDate) As FirstSessionSold from BuySells
INNER JOIN AspNetUsers on SellerUserId = AspNetUsers.id
INNER JOIN Sessions on Sessions.SessionId = BuySells.SessionId
WHERE BuyerUserId != SellerUserId
GROUP BY SellerUserId, FirstName, LastName
ORDER BY COUNT(SellerUserId) DESC

/* Top Sellers - Adjust year and day variables */
DECLARE @SellerYear INT
SET @SellerYear = 2024
DECLARE @SellerWeekday nvarchar(10)
SET @SellerWeekday = 'Wednesday'

SELECT T1.FirstName, T1.LastName, T1.SellerCount, T2.SessionCount, CAST((CAST(T1.SellerCount AS decimal) / CAST(T2.SessionCount AS decimal)) AS decimal(18,4)) * 100 AS SellingPercentage, T2.Year, T2.Weekday FROM
(SELECT SellerUserId, FirstName, LastName, COUNT(SellerUserId) AS SellerCount from BuySells
INNER JOIN AspNetUsers on SellerUserId = AspNetUsers.id
INNER JOIN Sessions on BuySells.SessionId = Sessions.SessionId
LEFT OUTER JOIN SessionsByDate on YEAR(Sessions.SessionDate) = YEAR(SessionsByDate.Year)
WHERE YEAR(Sessions.SessionDate) = @SellerYear AND DATENAME(WeekDay, Sessions.SessionDate) = @SellerWeekday AND Sessions.SessionId != 2771
AND BuyerUserId != SellerUserId
GROUP BY SellerUserId, FirstName, LastName
) AS T1,
(SELECT * from SessionsByDate WHERE YEAR = @SellerYear AND SessionsByDate.Weekday = @SellerWeekday) AS T2
ORDER BY T1.SellerCount DESC

/* Top Sellers - All time */
SELECT 
    T1.FirstName, 
    T1.LastName, 
    T1.SellerCount, 
    T1.FirstSale,
    T1.LastSale,
    T2.SessionCount, 
    CAST((CAST(T1.SellerCount AS decimal) / CAST(T2.SessionCount AS decimal)) AS decimal(18,4)) * 100 AS SellingPercentage
FROM
(SELECT 
    SellerUserId, 
    FirstName, 
    LastName, 
    COUNT(DISTINCT BuySells.SessionId) AS SellerCount,
    MIN(Sessions.SessionDate) AS FirstSale,
    MAX(Sessions.SessionDate) AS LastSale
 FROM BuySells
 INNER JOIN AspNetUsers ON SellerUserId = AspNetUsers.id
 INNER JOIN Sessions ON BuySells.SessionId = Sessions.SessionId
 WHERE Sessions.SessionId != 2771 AND BuyerUserId != SellerUserId
 GROUP BY SellerUserId, FirstName, LastName
) AS T1
CROSS APPLY
(SELECT COUNT(*) AS SessionCount 
 FROM Sessions 
 WHERE SessionId != 2771
 AND SessionDate BETWEEN T1.FirstSale AND T1.LastSale
) AS T2
ORDER BY T1.SellerCount DESC

/* Top Buyers - All time */
SELECT FirstName, LastName, COUNT(BuyerUserId) AS BuyerCount, MAX(SessionDate) As LastSessionBought, MIN(SessionDate) As FirstSessionBought from BuySells
INNER JOIN AspNetUsers on BuyerUserId = AspNetUsers.id
INNER JOIN Sessions on Sessions.SessionId = BuySells.SessionId
WHERE SellerUserId != BuyerUserId
GROUP BY BuyerUserId, FirstName, LastName
ORDER BY COUNT(BuyerUserId) DESC

/* Active users that have never bought */
SELECT Id, FirstName, LastName FROM AspNetUsers
WHERE Id NOT IN (SELECT BuyerUserId from BuySells WHERE BuyerUserId IS NOT NULL)
AND Id NOT IN (SELECT DISTINCT UserId from Regulars)
AND Active = 1
ORDER BY FirstName

/* Subs last bought by date */
SELECT Name, SessionDate, BuySellsSessionId, BuyerUserId FROM BuySellsByBuyer
WHERE SellerUserId IS NOT NULL
  AND SessionDate = (
    SELECT MAX(SessionDate) FROM BuySellsByBuyer bb
    WHERE bb.BuyerUserId = BuySellsByBuyer.BuyerUserId
      AND bb.SellerUserId IS NOT NULL
      AND NOT EXISTS (
        SELECT 1 FROM BuySellsByBuyer bb2
        WHERE bb2.SessionId = bb.SessionId 
          AND bb2.SellerUserId = bb.BuyerUserId
      )
  )
ORDER BY SessionDate DESC

/* Subs first bought by date */
SELECT Name, SessionDate, BuySellsSessionId, BuyerUserId FROM BuySellsByBuyer
WHERE SellerUserId IS NOT NULL
  AND SessionDate = (
    SELECT MIN(SessionDate) FROM BuySellsByBuyer bb
    WHERE bb.BuyerUserId = BuySellsByBuyer.BuyerUserId
      AND bb.SellerUserId IS NOT NULL
      AND NOT EXISTS (
        SELECT 1 FROM BuySellsByBuyer bb2
        WHERE bb2.SessionId = bb.SessionId 
          AND bb2.SellerUserId = bb.BuyerUserId
      )
  )
ORDER BY SessionDate DESC

/* Times from bought to payment sent */
WITH RankedTimes AS (
    SELECT
        b.FirstName,
        b.LastName,
        DATEDIFF(SECOND, b.CreateDateTime, s.CreateDateTime) AS TimeDifference,
        ROW_NUMBER() OVER (PARTITION BY b.UserId ORDER BY DATEDIFF(SECOND, b.CreateDateTime, s.CreateDateTime) ASC) AS FastestRank,
        ROW_NUMBER() OVER (PARTITION BY b.UserId ORDER BY DATEDIFF(SECOND, b.CreateDateTime, s.CreateDateTime) DESC) AS SlowestRank,
        b.SessionId,
        b.UserId
    FROM
        BoughtAndSentView AS b
    JOIN
        BoughtAndSentView AS s ON b.SessionId = s.SessionId AND b.UserId = s.UserId
    JOIN
        Sessions AS ss ON b.SessionId = ss.SessionId
    WHERE
        b.ActivityType = 'BOUGHT' 
        AND s.ActivityType = 'SENT'
        AND ss.SessionDate >= '2023-11-01'
)
SELECT
    FirstName,
    LastName,
    MIN(TimeDifference) AS FastestTimeS,
    dbo.FormatDateTimeFromSeconds(MIN(TimeDifference)) AS FastestTime,
    MAX(TimeDifference) AS SlowestTimeS,
    dbo.FormatDateTimeFromSeconds(MAX(TimeDifference)) AS SlowestTime,
    MIN(SessionId) AS FastestSessionID,
    MAX(SessionId) AS SlowestSessionID,
    AVG(TimeDifference) AS AverageTimeS,
    dbo.FormatDateTimeFromSeconds(AVG(TimeDifference)) AS AverageTime,
    COUNT(DISTINCT SessionId) AS SessionCount,
    UserId
FROM
    RankedTimes
WHERE
    FastestRank > 2  -- Exclude top 2 fastest times
    AND SlowestRank > 2  -- Exclude bottom 2 slowest times
    AND UserId IN (
        SELECT UserId
        FROM BoughtAndSentView
        GROUP BY UserId
    )
GROUP BY
    UserId, FirstName, LastName
ORDER BY
    AverageTimeS;

/* Number of buyers / session totals */
SELECT BuySells.SessionId, Sessions.SessionDate, COUNT(DISTINCT CASE WHEN BuyerUserId IS NOT NULL AND SellerUserId IS NOT NULL THEN BuySellId END) AS [Sold Count]
FROM dbo.BuySells
INNER JOIN Sessions ON BuySells.SessionId = Sessions.SessionId
WHERE Sessions.Note NOT LIKE '%cancelled%' AND SessionDate < GETDATE()
GROUP BY BuySells.SessionId, Sessions.SessionDate
ORDER BY [Sold Count] ASC

SELECT [Sold Count], COUNT(*) AS [Session Count]
FROM (
    SELECT BuySells.SessionId, COUNT(DISTINCT CASE WHEN BuyerUserId IS NOT NULL AND SellerUserId IS NOT NULL THEN BuySellId END) AS [Sold Count]
    FROM dbo.BuySells
	INNER JOIN Sessions ON BuySells.SessionId = Sessions.SessionId
	WHERE Sessions.Note NOT LIKE '%cancelled%' AND SessionDate < GETDATE()
    GROUP BY BuySells.SessionId
) AS Subquery
GROUP BY [Sold Count]
ORDER BY [Sold Count] ASC;

/* Skaters that have never bought or sold */
SELECT TOP (2000) Id, Email, EmailConfirmed, PasswordHash, SecurityStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEndDateUtc, LockoutEnabled, AccessFailedCount, UserName, FirstName, LastName, NotificationPreference, PayPalEmail, Active, Preferred, 
             VenmoAccount, MobileLast4, Rating, PreferredPlus, EmergencyName, EmergencyPhone, LockerRoom13
FROM   dbo.AspNetUsers
WHERE (Id NOT IN
                 (SELECT DISTINCT BuyerUserId
                 FROM    dbo.BuySells
                 WHERE (BuyerUserId IS NOT NULL))) AND (Email NOT LIKE '%brettmorrison%') AND (Id NOT IN
                 (SELECT DISTINCT SellerUserId
                 FROM    dbo.BuySells AS BuySells_1
                 WHERE (SellerUserId IS NOT NULL))) AND (Email NOT LIKE '%brettmorrison%') AND (Id NOT IN
                 (SELECT DISTINCT UserId
                 FROM    dbo.ActivityLogs))