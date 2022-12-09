-- Create a new regular set
-- Wednesday
INSERT INTO RegularSets (DayOfWeek, CreateDateTime, Description) VALUES (5, GETDATE(), 'Wed Roster 2023.1')
SELECT @@IDENTITY
SELECT * FROM RegularSets ORDER BY RegularSetId desc

-- Wednesday Light
SELECT Id, FirstName, LastName from AspNetUsers WHERE
FirstName IN ('Darin', 'Michael', 'Greg', 'Russ', 'Chuck', 'David', 'Glen', 'Doron', 'Matt', 'Paul')
AND LastName IN ('St. Ivany', 'Ganung', 'Bonanni', 'Belinsky', 'Jarrell', 'Schriger', 'Revivo', 'Azrialy', 'Knox', 'Mellinger')
AND Active = 1 ORDER BY LastName

INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('4a075289-6299-42a3-87ba-512fe4b586b6', 6, 1, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('68eb8101-3b60-490a-838b-d7f1b4b11dc9', 6, 1, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('ef907d2e-be9d-4e61-b091-735fbb6b2021', 6, 1, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('080bdd6c-3116-4360-866b-0ab0f7273dcf', 6, 1, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('bfdc1924-33b1-4bb3-aef1-3f86855c0e37', 6, 1, 2)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('570e5dbd-766f-4b84-9c18-fc6fa1d3aa93', 6, 1, 2)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('192e34c9-cf6f-4e50-a089-6c537a0dc4b3', 6, 1, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('f7acdb96-a2ae-4159-9bab-c9fa4069d737', 6, 1, 2)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('40b687d1-3d28-4b76-933c-dc779a9ad6ef', 6, 1, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('be74dbf8-6c42-4800-8aa4-d6818190077c', 6, 1, 2)

-- Fix positions
UPDATE Regulars SET PositionPreference = 1 WHERE RegularSetId = 6 and UserId in ('bfdc1924-33b1-4bb3-aef1-3f86855c0e37', '570e5dbd-766f-4b84-9c18-fc6fa1d3aa93', 'f7acdb96-a2ae-4159-9bab-c9fa4069d737')
UPDATE Regulars SET PositionPreference = 2 WHERE RegularSetId = 6 and UserId in ('080bdd6c-3116-4360-866b-0ab0f7273dcf', '68eb8101-3b60-490a-838b-d7f1b4b11dc9', 'ef907d2e-be9d-4e61-b091-735fbb6b2021')

-- Wednesday Dark
SELECT Id, FirstName, LastName from AspNetUsers WHERE
FirstName IN ('David', 'Jeffrey', 'Dave', 'Roger', 'Brandon', 'Brett', 'Brad', 'Matthew', 'Eric', 'John Tyler')
AND LastName IN ('Coons', 'Sabala', 'Buss', 'Sackaroff', 'Jolley', 'Morrison', 'Hennegan', 'Dugard', 'Johnson', 'Grant')
AND Active = 1 ORDER BY LastName

INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('61f8f892-abd5-4a13-9960-b43e6ce0aba0', 6, 2, 2)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('4bceaa3c-653c-4f7e-8bbd-572ef30fd3be', 6, 2, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('3eb98864-8345-4425-850e-85e481b425c6', 6, 2, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('80c21632-7eb5-4cf2-b66c-5c9efb6a50b8', 6, 2, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('3d0a1851-5bcb-4364-a40b-37f758255237', 6, 2, 2)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('35addb43-49f9-4da3-b927-29d09e9d60e5', 6, 2, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('607daa7d-5e1e-4d6c-86ff-ae75010d1446', 6, 2, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('fdbfe74a-a5c5-4ff0-8edb-c98a5df9d85a', 6, 2, 2)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('869707a5-b410-47d3-b2a2-51cf41c366cc', 6, 2, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('063695a9-7cdd-4478-97ed-6eb3eab078d2', 6, 2, 2)

-- Fix positions
UPDATE Regulars SET PositionPreference = 1 WHERE RegularSetId = 6 and UserId in ('3d0a1851-5bcb-4364-a40b-37f758255237', 'fdbfe74a-a5c5-4ff0-8edb-c98a5df9d85a')
UPDATE Regulars SET PositionPreference = 2 WHERE RegularSetId = 6 and UserId in ('4bceaa3c-653c-4f7e-8bbd-572ef30fd3be', '869707a5-b410-47d3-b2a2-51cf41c366cc')

-- Swap Players

-- Friday 
INSERT INTO RegularSets (DayOfWeek, CreateDateTime, Description) VALUES (7, GETDATE(), 'Fri Roster 2023.1')
SELECT @@IDENTITY
SELECT * FROM RegularSets ORDER BY RegularSetId desc

-- Friday Light
SELECT Id, FirstName, LastName from AspNetUsers WHERE
FirstName IN ('Alex', 'Darin', 'Jeffrey', 'Keith', 'Brad', 'Brandon', 'Brett', 'Damion', 'Dave', 'David')
AND LastName IN ('Lichtenstein', 'St. Ivany', 'Sabala', 'Bjelajac', 'Hennegan', 'Jolley', 'Morrison', 'Scheller', 'Buss', 'Schriger')
AND Active = 1 ORDER BY LastName

INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('7f530226-bbe0-4c3c-b6b8-09af7c858576', 7, 1, 2)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('61f8f892-abd5-4a13-9960-b43e6ce0aba0', 7, 1, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('3d0a1851-5bcb-4364-a40b-37f758255237', 7, 1, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('607daa7d-5e1e-4d6c-86ff-ae75010d1446', 7, 1, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('7a06eb4d-d110-421c-9f4d-b3c965acb5a7', 7, 1, 2)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('fdbfe74a-a5c5-4ff0-8edb-c98a5df9d85a', 7, 1, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('869707a5-b410-47d3-b2a2-51cf41c366cc', 7, 1, 2)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('95037b40-9ca7-48ad-aa2d-0f144ddde8c6', 7, 1, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('40b687d1-3d28-4b76-933c-dc779a9ad6ef', 7, 1, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('be74dbf8-6c42-4800-8aa4-d6818190077c', 7, 1, 2)

-- Friday Dark
SELECT Id, FirstName, LastName from AspNetUsers WHERE
FirstName IN ('David', 'Michael', 'Oliver', 'Roger', 'Randy', 'Eric', 'Ian', 'John Tyler', 'Matthew', 'Robert')
AND LastName IN ('Coons', 'Ganung', 'Koechli', 'Sackaroff', 'Riggs', 'Johnson', 'Hirsch', 'Grant', 'Dugard', 'Minn')
AND Active = 1 ORDER BY LastName

INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('4bceaa3c-653c-4f7e-8bbd-572ef30fd3be', 7, 2, 2)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('3eb98864-8345-4425-850e-85e481b425c6', 7, 2, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('080bdd6c-3116-4360-866b-0ab0f7273dcf', 7, 2, 2)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('80c21632-7eb5-4cf2-b66c-5c9efb6a50b8', 7, 2, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('b08e5731-9f68-4ed3-b52a-b93ecb824bcf', 7, 2, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('35addb43-49f9-4da3-b927-29d09e9d60e5', 7, 2, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('216320a7-8d3e-4be1-8a62-e6ec716129a4', 7, 2, 2)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('49799561-c216-4a37-b29b-ae6957e4d141', 7, 2, 2)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('370f4789-06de-4c5a-a74f-614c41de513b', 7, 2, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('063695a9-7cdd-4478-97ed-6eb3eab078d2', 7, 2, 2)

-- Fix positions
UPDATE Regulars SET PositionPreference = 1 WHERE RegularSetId = 7 and UserId in ('49799561-c216-4a37-b29b-ae6957e4d141')

-- Swap Players
UPDATE Regulars SET TeamAssignment = 1 WHERE RegularSetId = 7 and UserId in ('063695a9-7cdd-4478-97ed-6eb3eab078d2')
UPDATE Regulars SET TeamAssignment = 2 WHERE RegularSetId = 7 and UserId in ('7a06eb4d-d110-421c-9f4d-b3c965acb5a7')
UPDATE Regulars SET TeamAssignment = 1 WHERE RegularSetId = 7 and UserId in ('216320a7-8d3e-4be1-8a62-e6ec716129a4')
UPDATE Regulars SET TeamAssignment = 2 WHERE RegularSetId = 7 and UserId in ('7f530226-bbe0-4c3c-b6b8-09af7c858576')
UPDATE Regulars SET TeamAssignment = 1 WHERE RegularSetId = 7 and UserId in ('b08e5731-9f68-4ed3-b52a-b93ecb824bcf')
UPDATE Regulars SET TeamAssignment = 2 WHERE RegularSetId = 7 and UserId in ('95037b40-9ca7-48ad-aa2d-0f144ddde8c6')
UPDATE Regulars SET TeamAssignment = 1 WHERE RegularSetId = 7 and UserId in ('4bceaa3c-653c-4f7e-8bbd-572ef30fd3be')
UPDATE Regulars SET TeamAssignment = 2 WHERE RegularSetId = 7 and UserId in ('869707a5-b410-47d3-b2a2-51cf41c366cc')
