
-- After Running this SQL, Update Tenants
delete from ScreenFields where ObjectFieldId = (select Id from ObjectFields where FieldName = 'EntityStatusId' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))
go

delete from ScreenFields where ObjectFieldId = (select Id from ObjectFields where FieldName = 'EntityStatusName' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))
go

delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'EntityStatusId' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))
go

delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'EntityStatusName' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))
go

delete from AdvancedQueryFilters where objectfieldid  = (select id from ObjectFields where FieldName = 'EntityStatusId' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote'))

delete from ObjectFields where FieldName = 'EntityStatusId' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote')
go

delete from ObjectFields where FieldName = 'EntityStatusName' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote')
go

delete from TextCodes where Code like '%EntityStatus%' and ObjectTableId = (select Id from ObjectTables where Name = 'Quote')
go