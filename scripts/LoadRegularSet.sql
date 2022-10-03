-- Create a new regular set
--TRUNCATE TABLE RegularSet
-- Wednesday
INSERT INTO RegularSet (DayOfWeek, CreateDateTime, Description) VALUES (4, GETDATE(), 'Wed Group A 2022')
SELECT @@IDENTITY
SELECT * FROM RegularSet ORDER BY RegularSetId desc

-- Wednesday Light
SELECT Id, FirstName, LastName from AspNetUsers WHERE
FirstName IN ('Darin', 'Dave', 'Greg', 'Russ', 'Chuck', 'David', 'Glen', 'Doron', 'Matt', 'Paul')
AND LastName IN ('St. Ivany', 'Buss', 'Bonanni', 'Belinsky', 'Jarrell', 'Schriger', 'Revivo', 'Azrialy', 'Knox', 'Mellinger')
AND Active = 1

-- Wednesday Dark
SELECT Id, FirstName, LastName from AspNetUsers WHERE
FirstName IN ('David', 'Jeffrey', 'Michael', 'Roger', 'Brandon', 'Brett', 'Brad', 'Matthew', 'Eric', 'John Tyler')
AND LastName IN ('Coons', 'Sabala', 'Ganung', 'Sackaroff', 'Jolley', 'Morrison', 'Hennegan', 'Dugard', 'Johnson', 'Grant')
AND Active = 1

-- Build Insert statement for each ID
-- INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES

-- Friday 
INSERT INTO RegularSet (DayOfWeek, CreateDateTime, Description) VALUES (6, GETDATE(), 'Fri Group A 2022')
SELECT @@IDENTITY
SELECT * FROM RegularSet ORDER BY RegularSetId desc

-- Friday Light
SELECT Id, FirstName, LastName from AspNetUsers WHERE
FirstName IN ('Alex', 'Dave', 'Jeffrey', 'Keith', 'Brad', 'Brandon', 'Brett', 'Damion', 'Dave', 'David')
AND LastName IN ('Lichtenstein', 'Schaaf', 'Sabala', 'Bjelajac', 'Hennegan', 'Jolley', 'Morrison', 'Scheller', 'Buss', 'Schriger')
AND Active = 1

-- Friday Dark
SELECT Id, FirstName, LastName from AspNetUsers WHERE
FirstName IN ('Michael', 'Oliver', 'Randy', 'Roger', 'David', 'Eric', 'Ian', 'John Tyler', 'Matthew', 'Robert')
AND LastName IN ('Ganung', 'Koechli', 'Riggs', 'Sackaroff', 'Coons', 'Johnson', 'Hirsch', 'Grant', 'Dugard', 'Minn')
AND Active = 1
