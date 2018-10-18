

update QueryColumns 
set ColumnWidth = 350
where 
QueryId = (select Id from Queries where Code = 'Country Cities')
and ObjectFieldId = (select Id from ObjectFields where FieldName = 'CountryEnglishName' and ObjectTableId = (select Id from ObjectTables where Name = 'CountryCity'))
go