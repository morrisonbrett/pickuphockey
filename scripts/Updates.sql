/*
  Update script to clear open buy / sell payments and received
*/
declare @userid varchar(256)
set @userid = (select id from AspNetUsers where lastname = 'Mellinger' and firstname = 'Paul')
select @userid

--update BuySells set PaymentSent = 1
--where buysellid in (
--select buysellid from BuySells
select * from BuySells 
inner join sessions on buysells.sessionid = Sessions.sessionid
where buyerUserId = @userid
and selleruserid is not null
and PaymentSent = 0
and year(sessiondate) <= '2022'
--)

--update BuySells set paymentreceived = 1
--where buysellid in (
--select buysellid from BuySells
select * from BuySells 
inner join sessions on buysells.sessionid = Sessions.sessionid
where sellerUserId = @userid
and buyeruserid is not null
and PaymentReceived = 0
and year(sessiondate) <= '2022'
--)

/* Reset a password */
declare @userid varchar(256)
set @userid = (select id from AspNetUsers where lastname = 'Hennegan' and firstname = 'John')
select @userid

select * from AspNetUsers where id = @userid
select * from aspnetuserlogins where userid = @userid
delete from AspNetUserLogins where UserId = @userid
update AspNetUsers set PasswordHash = '123456', SecurityStamp = NEWID(), EmailConfirmed = 1 where id = @userid
select * from AspNetUsers where id = @userid
select * from aspnetuserlogins where userid = @userid
