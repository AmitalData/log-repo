
-- Run this SQL
-- Update Tenants & CRM

update ObjectTables set HeaderScreenId = null where Name = 'Stage'
go

update ObjectTables set HeaderScreenId = null where Name = 'LeadSource'
go

update ObjectTables set HeaderScreenId = null where Name = 'AdditionalService'
go


delete from ScreenModifications where ScreenId in (select Id from Screens where ObjectTableId = (select Id from ObjectTables where Name = 'Stage'))
go

delete from ScreenModifications where ScreenId in (select Id from Screens where ObjectTableId = (select Id from ObjectTables where Name = 'LeadSource'))
go

delete from ScreenModifications where ScreenId in (select Id from Screens where ObjectTableId = (select Id from ObjectTables where Name = 'AdditionalService'))
go

delete from ScreenFields where ScreenId in (select Id from Screens where ObjectTableId = (select Id from ObjectTables where Name = 'Stage'))
go

delete from ScreenFields where ScreenId in (select Id from Screens where ObjectTableId = (select Id from ObjectTables where Name = 'LeadSource'))
go

delete from ScreenFields where ScreenId in (select Id from Screens where ObjectTableId = (select Id from ObjectTables where Name = 'AdditionalService'))
go

delete from Screens where ObjectTableId = (select Id from ObjectTables where Name = 'Stage')
go

delete from Screens where ObjectTableId = (select Id from ObjectTables where Name = 'LeadSource')
go

delete from Screens where ObjectTableId = (select Id from ObjectTables where Name = 'AdditionalService')
go

delete from QueryColumns where QueryId in (select Id from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'Stage'))
go

delete from QueryColumns where QueryId in (select Id from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'LeadSource'))
go

delete from QueryColumns where QueryId in (select Id from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'AdditionalService'))
go