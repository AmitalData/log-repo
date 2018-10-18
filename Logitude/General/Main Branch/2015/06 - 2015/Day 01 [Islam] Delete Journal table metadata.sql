
-- deleteing journal tables metadata
-----------------------------------------------------------------------------------------------------------------------------------------
delete from screenfields where ScreenId in (select id from Screens where  ObjectTableId = (select id from ObjectTables where name = 'journal'))
go
update ObjectTables set HeaderScreenId=  null   where name = 'journal'
go
delete Screens where  ObjectTableId = (select id from ObjectTables where name = 'journal')
go

delete from QueryColumns where QueryId in (select id from Queries where  ObjectTableId = (select id from ObjectTables where name = 'journal'))
go
delete from queries where ObjectTableId = (select id from ObjectTables where name = 'journal')
go

delete from ObjectFields where ObjectTableId = (select id from ObjectTables where name = 'journal')
go
delete from ObjectTableTabs where ObjectTableId = (select id from ObjectTables where name = 'journal')
go
delete from EventTypes where ObjectTableId = (select id from ObjectTables where name = 'journal')
go
delete from PackageFeatures where FeatureId in (select id from Features where ObjectTableId = (select id from ObjectTables where name = 'journal'))
go
delete from RoleFeatures where FeatureId in (select id from Features where ObjectTableId = (select id from ObjectTables where name = 'journal'))
go
delete from Features where ObjectTableId = (select id from ObjectTables where name = 'journal')
go
delete from TextCodes where ObjectTableId = (select id from ObjectTables where name = 'journal')
go
delete from ObjectTables  where name = 'journal'
go
-----------------------------------------------------------------------------------------------------------------------------------------
delete from screenfields where ScreenId in (select id from Screens where  ObjectTableId = (select id from ObjectTables where name = 'journalline'))
go
update ObjectTables set HeaderScreenId=  null   where name = 'journalline'
go
delete Screens where  ObjectTableId = (select id from ObjectTables where name = 'journalline')
go
delete from QueryColumns where QueryId in (select id from Queries where  ObjectTableId = (select id from ObjectTables where name = 'journalline'))
go
delete from queries where ObjectTableId = (select id from ObjectTables where name = 'journalline')
go
delete from ObjectFields where ObjectTableId = (select id from ObjectTables where name = 'journalline')
go
delete from ObjectTableTabs where ObjectTableId = (select id from ObjectTables where name = 'journalline')
go
delete from EventTypes where ObjectTableId = (select id from ObjectTables where name = 'journalline')
go
delete from Features where ObjectTableId = (select id from ObjectTables where name = 'journalline')
go
delete from TextCodes where ObjectTableId = (select id from ObjectTables where name = 'journalline')
go
delete from ObjectTables  where name = 'journalline'
go
-----------------------------------------------------------------------------------------------------------------------------------------
delete from screenfields where ScreenId in (select id from Screens where  ObjectTableId = (select id from ObjectTables where name = 'journaltype'))
go
update ObjectTables set HeaderScreenId=  null   where name = 'journaltype'
go
delete Screens where  ObjectTableId = (select id from ObjectTables where name = 'journaltype')
go
delete from QueryColumns where QueryId in (select id from Queries where  ObjectTableId = (select id from ObjectTables where name = 'journaltype'))
go
delete from queries where ObjectTableId = (select id from ObjectTables where name = 'journaltype')
go
delete from ObjectFields where ObjectTableId = (select id from ObjectTables where name = 'journaltype')
go
delete from ObjectTableTabs where ObjectTableId = (select id from ObjectTables where name = 'journaltype')
go
delete from EventTypes where ObjectTableId = (select id from ObjectTables where name = 'journaltype')
go
delete from Features where ObjectTableId = (select id from ObjectTables where name = 'journaltype')
go
delete from TextCodes where ObjectTableId = (select id from ObjectTables where name = 'journaltype')
go
delete from ObjectTables  where name = 'journaltype'



 

