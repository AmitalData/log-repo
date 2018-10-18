update ObjectFields 
set IsRequiered = '0'
where FieldName = 'CountryEnglishName' and ObjectTableId= (select id from ObjectTables where name ='state') 