
delete from QueryColumns where ObjectFieldId in ( select id from ObjectFields where FieldName ='IsInlandImport' or FieldName = 'IsInlandExport' )
go
delete from AdvancedQueryFilters where ObjectFieldId in ( select id from ObjectFields where FieldName ='IsInlandImport' or FieldName = 'IsInlandExport'  )
go
delete from ObjectFields where FieldName ='IsInlandImport' or FieldName = 'IsInlandExport' 
go