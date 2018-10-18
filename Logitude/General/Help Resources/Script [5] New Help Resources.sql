update HelpResources set IsNew = 0, CreateDate = '2015-12-13', UpdateDate = '2015-12-13'
go


insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('22', 'Managing Customers', GETDATE(), GETDATE(), 'EN', 'TUT', 'CRM', null, null, 'managing_customers.pdf', 'Managing Customers', 1)

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('23', 'Logitude Outlook Connection', GETDATE(), GETDATE(), 'EN', 'TUT', 'CRM', null, null, 'outlook_connection.pdf', 'Logitude Outlook Connection', 1)

insert into HelpResources(Code, Name, CreateDate, UpdateDate, [Language], [Type], Category, VideoURL, Duration, [FileName], SearchFields, IsNew)
values('24', 'Custom Roles', GETDATE(), GETDATE(), 'EN', 'TUT', 'OPE', null, null, 'custom_roles.pdf', 'Custom Roles', 1)

update HelpResources 
set IsNew = 1, UpdateDate = GETDATE()
where Code = '12' or Code = '18' or Code = '19'

--release date
update HelpResources set CreateDate = '2016-02-28', UpdateDate = '2016-02-28' 
where Code = '22' or Code = '23' or Code = '24' or Code = '12' or Code = '18' or Code = '19'

