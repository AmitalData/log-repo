delete from AdvancedQueryFilters where ObjectFieldId =(select id from ObjectFields where FieldName ='IsAirImport')
go
delete from AdvancedQueryFilters where ObjectFieldId =(select id from ObjectFields where FieldName ='IsAirExport')
go
delete from AdvancedQueryFilters where ObjectFieldId =(select id from ObjectFields where FieldName ='IsCustomsImport')
go
delete from AdvancedQueryFilters where ObjectFieldId =(select id from ObjectFields where FieldName ='IsOceanImport')
go
delete from AdvancedQueryFilters where ObjectFieldId =(select id from ObjectFields where FieldName ='IsOceanExport')
go
delete from AdvancedQueryFilters where ObjectFieldId =(select id from ObjectFields where FieldName ='IsInlandDomestic')
go
delete from AdvancedQueryFilters where ObjectFieldId =(select id from ObjectFields where FieldName ='IsAll')
go