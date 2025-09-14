
-- run only for amital cloud version


--select * from contacts where email = 'support@amital.co.il' and Tenant = 0
--select Contacts.* from Users,Contacts where Contacts.Id = Users.Id and Contacts.Signature is null and usertype != 'S'
--------------------------------------------------------------------------------------------------
--declare @Tenant as int
--declare @ContactId as varchar(15)
--declare @AdminSignature as varbinary(max)

--set @AdminSignature = (select [signature] from Contacts where email = 'support@amital.co.il' and Tenant = 0)

--begin
--declare ContactsCursor cursor read_only
--for
--select Contacts.Id,Contacts.Tenant
--from Users,Contacts where Contacts.Id = Users.Id and Contacts.Signature is null and usertype != 'S'

--open ContactsCursor fetch next from ContactsCursor into @ContactId,@Tenant
--while @@FETCH_STATUS = 0
--begin

--update contacts set signature  = @AdminSignature where id = @ContactId and Tenant = @Tenant 

--FETCH NEXT FROM ContactsCursor INTO @ContactId,@Tenant
--end

--CLOSE ContactsCursor
--DEALLOCATE ContactsCursor

--end

--select Contacts.* from Users,Contacts where Contacts.Id = Users.Id and Contacts.Signature is null and usertype != 'S'
