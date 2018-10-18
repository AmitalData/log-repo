

delete from AdvancedQueryFilters where QueryId in (select Id from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'ContactPosition'))
go

delete from QueryColumns where QueryId in (select Id from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'ContactPosition'))
go

delete from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'ContactPosition')
go

delete from ObjectTableTabs where ObjectTableId = (select Id from ObjectTables where Name = 'ContactPosition')
go

delete from ScreenModifications where ScreenId in (select Id from Screens where ObjectTableId = (select Id from ObjectTables where Name = 'ContactPosition'))
go

delete from ScreenFields where ScreenId in (select Id from Screens where ObjectTableId = (select Id from ObjectTables where Name = 'ContactPosition'))
go

update ObjectTables set HeaderScreenId = null where Name = 'ContactPosition'
go

delete from Screens where ObjectTableId = (select Id from ObjectTables where Name = 'ContactPosition')
go

delete from ObjectFields where ObjectTableId = (select Id from ObjectTables where Name = 'ContactPosition')
go

delete from MenusTables where ObjectTableId = (select Id from ObjectTables where Name = 'ContactPosition')
go

delete from PackageFeatures where FeatureId in (select Id from Features where ObjectTableId = (select Id from ObjectTables where Name = 'ContactPosition'))
go

delete from RoleFeatures where FeatureId in (select Id from Features where ObjectTableId = (select Id from ObjectTables where Name = 'ContactPosition'))
go

delete from Features where ObjectTableId = (select Id from ObjectTables where Name = 'ContactPosition')
go

delete from Features where Code = 'General.Features.ContactPositions'
go


delete from PackageFeatures where FeatureId in (select Id from Features where Code = 'CONTACTPOSITIONS')
go

delete from RoleFeatures where FeatureId in (select Id from Features where Code = 'CONTACTPOSITIONS')
go

delete from Features where Code = 'CONTACTPOSITIONS'
go

delete from TextCodes where ObjectTableId = (select Id from ObjectTables where Name = 'ContactPosition')
go



delete from EntityLastActivities where ObjectTableId = (select Id from ObjectTables where Name = 'ContactPosition')
go

delete from ObjectFields where FieldName  = 'ContactPositionId'
go

delete from TextCodes where Code = 'Contact.F.ContactPositionId'
go

delete from TextCodes where Code = 'Contact.ContactPositionIdHelpText'
go

delete from TextCodes where Code like '%ContactPosition%'
go

delete from ObjectTables where Name = 'ContactPosition'
go


