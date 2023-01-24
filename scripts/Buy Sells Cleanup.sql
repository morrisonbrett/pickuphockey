select * from aspnetusers where lastname = 'minn'
select * from buysells where buyeruserid = '49799561-c216-4a37-b29b-ae6957e4d141' and selleruserid is not null and paymentsent = 0
select * from buysells where selleruserid = '49799561-c216-4a37-b29b-ae6957e4d141' and buyeruserid is not null and paymentreceived = 0

update buysells set paymentreceived = 1 where buysellid in (6241, 17193)

update BuySells set PaymentSent = 1 where buysellid in (
select b.BuySellId
--select u.FirstName, u.LastName, b.*, s.SessionDate
from buysells b
inner join AspNetUsers u on b.buyeruserid = u.id
inner join Sessions s on b.SessionId = s.SessionId
where selleruserid is not null and paymentsent = 0
and YEAR(sessiondate) < 2022
)

update BuySells set PaymentReceived = 1 where buysellid in (
select b.BuySellId
--select u.FirstName, u.LastName, b.*, s.SessionDate
from buysells b
inner join AspNetUsers u on b.SellerUserId = u.id
inner join Sessions s on b.SessionId = s.SessionId
where buyeruserid is not null and PaymentReceived = 0
and YEAR(sessiondate) < 2022
)
