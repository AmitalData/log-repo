
-- Run this Script then update Tenants
update ObjectTables set HeaderScreenId = null where Name = 'CountryCity'
go

delete from ScreenFields where ScreenId in (select Id from Screens where ObjectTableId = (select Id from ObjectTables where Name = 'CountryCity'))
go

delete from ScreenModifications where ScreenId in (select Id from Screens where ObjectTableId = (select Id from ObjectTables where Name = 'CountryCity'))
go

delete from Screens where ObjectTableId = (select Id from ObjectTables where Name = 'CountryCity')
go

delete from QueryColumns where QueryId in (select Id from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'CountryCity'))
go

delete from ObjectFields where ObjectTableId = (select Id from ObjectTables where Name = 'CountryCity')
go

delete from TextCodes
where
(TextCodeTypeCode = 'F' OR TextCodeTypeCode = 'H' OR TextCodeTypeCode = 'CH')
And ObjectTableId = (select Id from ObjectTables where Name = 'CountryCity')
go
