--main database
delete from Users where Id in (select id from Contacts where email='customercare@logitudeworld.com')
delete from ContactTenantRoleSet where ContactTenantId in (select Id from ContactTenants where ContactId in (select id from Contacts where email='customercare@logitudeworld.com'))
delete from ContactTenants where ContactId in (select id from Contacts where email='customercare@logitudeworld.com')
delete from Contacts where Email='customercare@logitudeworld.com'


select * from Users where Id in (select id from Contacts where email='customercare@logitudeworld.com')
select * from ContactTenantRoleSet where ContactTenantId in (select Id from ContactTenants where ContactId in (select id from Contacts where email='customercare@logitudeworld.com'))
select * from ContactTenants where ContactId in (select id from Contacts where email='customercare@logitudeworld.com')
select * from Contacts where Email='customercare@logitudeworld.com'


--global database
delete from ContactPasswords where Email='customercare@logitudeworld.com'
delete from GlobalContacts where Email='customercare@logitudeworld.com'



select * from GlobalContacts where Email='customercare@logitudeworld.com'
select * from ContactPasswords where Email='customercare@logitudeworld.com'