select * from aspnetusers where lastname like '% -%'
--update aspnetusers set active = 0 where lastname like '% -%'
--update aspnetusers set active = 1 where lastname like '% -%'

declare @venmo nvarchar(100)
declare @paypal nvarchar(100)
declare @last4 nvarchar(100)
declare @user nvarchar(100)
declare @first nvarchar(100)
declare @last nvarchar(100)

set @venmo = 'john-bryan-17'
set @paypal = 'mail@johnbryan.tv'
set @last4 = '1672'
set @first = 'John'
set @last = 'Bryan - 24'
set @user = 'johnbryan24@brettmorrison.com'
insert into aspnetusers (id, email, emailconfirmed, phonenumberconfirmed, twofactorenabled, lockoutenabled, accessfailedcount, username, passwordhash, securitystamp, firstname, lastname, notificationpreference, paypalemail, venmoaccount, mobilelast4, active)
values (newid(), @user, 1, 0, 0, 1, 0, @user, newid(), newid(), @first, @last, 0, @paypal, @venmo, @last4, 0)
