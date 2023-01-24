declare @userid nvarchar(256)
declare @first nvarchar(256)
declare @last nvarchar(256)

set @first = 'Ben'
set @last = 'Dixon'

select * from aspnetusers where firstname = @first and lastname = @last order by email
select @userid = id from aspnetusers where firstname = @first and lastname = @last order by email
select @userid
--update aspnetusers set emailconfirmed = 1, passwordhash = '333', securitystamp = newid() where id = @userid
select * from aspnetuserlogins where userid = @userid
--delete from aspnetuserlogins where userid = @userid
