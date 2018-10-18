

-- Please Run this then update Tenants

delete from AdvancedQueryFilters where QueryId in (select Id from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'AccountingTransferHeader'))
go

delete from QueryColumns where QueryId in (select Id from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'AccountingTransferHeader'))
go

delete from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'AccountingTransferHeader')
go

delete from PackageFeatures where FeatureId in (select Id from Features where ObjectTableId = (select Id from ObjectTables where Name = 'AccountingTransferHeader'))
go

delete from RoleFeatures where FeatureId in (select Id from Features where ObjectTableId = (select Id from ObjectTables where Name = 'AccountingTransferHeader'))
go

delete from Features where FeatureTypeCode = 'QUER' and ObjectTableId = (select Id from ObjectTables where Name = 'AccountingTransferHeader')
go

delete from ObjectFields where FieldName like '%TransferHistory%' and ObjectTableId = (select Id from ObjectTables where Name = 'AccountingTransferHeader')
go

delete from TextCodes where Code like '%AccountingTransferHeader%TransferHistory%'
go