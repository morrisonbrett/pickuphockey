-- Create a new regular set
--TRUNCATE TABLE RegularSet
-- Wednesday
INSERT INTO RegularSets (DayOfWeek, CreateDateTime, Description) VALUES (4, GETDATE(), 'Wed Group A 2022')
SELECT @@IDENTITY
SELECT * FROM RegularSets ORDER BY RegularSetId desc

-- Wednesday Light
SELECT Id, FirstName, LastName from AspNetUsers WHERE
FirstName IN ('Darin', 'Dave', 'Greg', 'Russ', 'Chuck', 'David', 'Glen', 'Doron', 'Matt', 'Paul')
AND LastName IN ('St. Ivany', 'Buss', 'Bonanni', 'Belinsky', 'Jarrell', 'Schriger', 'Revivo', 'Azrialy', 'Knox', 'Mellinger')
AND Active = 1

INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('192e34c9-cf6f-4e50-a089-6c537a0dc4b3', 1, 1, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('40b687d1-3d28-4b76-933c-dc779a9ad6ef', 1, 1, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('4a075289-6299-42a3-87ba-512fe4b586b6', 1, 1, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('570e5dbd-766f-4b84-9c18-fc6fa1d3aa93', 1, 1, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('61f8f892-abd5-4a13-9960-b43e6ce0aba0', 1, 1, 2)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('68eb8101-3b60-490a-838b-d7f1b4b11dc9', 1, 1, 2)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('be74dbf8-6c42-4800-8aa4-d6818190077c', 1, 1, 2)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('bfdc1924-33b1-4bb3-aef1-3f86855c0e37', 1, 1, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('ef907d2e-be9d-4e61-b091-735fbb6b2021', 1, 1, 2)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('f7acdb96-a2ae-4159-9bab-c9fa4069d737', 1, 1, 1)

-- Wednesday Dark
SELECT Id, FirstName, LastName from AspNetUsers WHERE
FirstName IN ('David', 'Jeffrey', 'Michael', 'Roger', 'Brandon', 'Brett', 'Brad', 'Matthew', 'Eric', 'John Tyler')
AND LastName IN ('Coons', 'Sabala', 'Ganung', 'Sackaroff', 'Jolley', 'Morrison', 'Hennegan', 'Dugard', 'Johnson', 'Grant')
AND Active = 1

INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('063695a9-7cdd-4478-97ed-6eb3eab078d2', 1, 2, 2)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('080bdd6c-3116-4360-866b-0ab0f7273dcf', 1, 2, 2)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('35addb43-49f9-4da3-b927-29d09e9d60e5', 1, 2, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('3d0a1851-5bcb-4364-a40b-37f758255237', 1, 2, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('3eb98864-8345-4425-850e-85e481b425c6', 1, 2, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('4bceaa3c-653c-4f7e-8bbd-572ef30fd3be', 1, 2, 2)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('607daa7d-5e1e-4d6c-86ff-ae75010d1446', 1, 2, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('80c21632-7eb5-4cf2-b66c-5c9efb6a50b8', 1, 2, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('869707a5-b410-47d3-b2a2-51cf41c366cc', 1, 2, 2)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('fdbfe74a-a5c5-4ff0-8edb-c98a5df9d85a', 1, 2, 1)

-- Friday 
INSERT INTO RegularSets (DayOfWeek, CreateDateTime, Description) VALUES (6, GETDATE(), 'Fri Group A 2022')
SELECT @@IDENTITY
SELECT * FROM RegularSets ORDER BY RegularSetId desc

-- Friday Light
SELECT Id, FirstName, LastName from AspNetUsers WHERE
FirstName IN ('Alex', 'Dave', 'Jeffrey', 'Keith', 'Brad', 'Brandon', 'Brett', 'Damion', 'Dave', 'David')
AND LastName IN ('Lichtenstein', 'Schaaf', 'Sabala', 'Bjelajac', 'Hennegan', 'Jolley', 'Morrison', 'Scheller', 'Buss', 'Schriger')
AND Active = 1

INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('3d0a1851-5bcb-4364-a40b-37f758255237', 2, 1, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('40b687d1-3d28-4b76-933c-dc779a9ad6ef', 2, 1, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('607daa7d-5e1e-4d6c-86ff-ae75010d1446', 2, 1, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('61f8f892-abd5-4a13-9960-b43e6ce0aba0', 2, 1, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('7a06eb4d-d110-421c-9f4d-b3c965acb5a7', 2, 1, 2)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('7f530226-bbe0-4c3c-b6b8-09af7c858576', 2, 1, 2)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('869707a5-b410-47d3-b2a2-51cf41c366cc', 2, 1, 2)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('95037b40-9ca7-48ad-aa2d-0f144ddde8c6', 2, 1, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('fdbfe74a-a5c5-4ff0-8edb-c98a5df9d85a', 2, 1, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('fe4da331-1963-4601-9de7-acf17c71a386', 2, 1, 2)

-- Friday Dark
SELECT Id, FirstName, LastName from AspNetUsers WHERE
FirstName IN ('Michael', 'Oliver', 'Randy', 'Roger', 'David', 'Eric', 'Ian', 'John Tyler', 'Matthew', 'Robert')
AND LastName IN ('Ganung', 'Koechli', 'Riggs', 'Sackaroff', 'Coons', 'Johnson', 'Hirsch', 'Grant', 'Dugard', 'Minn')
AND Active = 1

INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('063695a9-7cdd-4478-97ed-6eb3eab078d2', 2, 2, 2)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('080bdd6c-3116-4360-866b-0ab0f7273dcf', 2, 2, 2)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('216320a7-8d3e-4be1-8a62-e6ec716129a4', 2, 2, 2)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('35addb43-49f9-4da3-b927-29d09e9d60e5', 2, 2, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('370f4789-06de-4c5a-a74f-614c41de513b', 2, 2, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('3eb98864-8345-4425-850e-85e481b425c6', 2, 2, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('49799561-c216-4a37-b29b-ae6957e4d141', 2, 2, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('4bceaa3c-653c-4f7e-8bbd-572ef30fd3be', 2, 2, 2)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('80c21632-7eb5-4cf2-b66c-5c9efb6a50b8', 2, 2, 1)
INSERT INTO Regulars (UserId, RegularSetId, TeamAssignment, PositionPreference) VALUES ('b08e5731-9f68-4ed3-b52a-b93ecb824bcf', 2, 2, 1)
