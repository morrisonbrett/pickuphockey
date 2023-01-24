declare @id nvarchar(256)
declare @oldid nvarchar(256)
declare @oldrating decimal

select * from AspNetUsers where lastname like '%Quan%'
select @id = '4b3b4360-f17a-465c-9c9e-17f086699296'
select @oldid = '254c67ad-17e5-4f55-8f01-c2a2bc4d709f'
select @oldrating = (select top 1 rating from AspNetUsers where id = @oldid)

select @id, @oldid, @oldrating

select * from ActivityLogs where Userid = @oldid
select * from BuySells where BuyerUserId = @oldid
select * from BuySells where SellerUserId = @oldid

update ActivityLogs set UserId = @id where Userid = @oldid
update BuySells set BuyerUserId = @id where BuyerUserId = @oldid
update BuySells set SellerUserId = @id where SellerUserId = @oldid
update AspNetUsers set Rating = @oldrating where Id = @id

delete from AspNetUsers where id = @oldid
