

delete from ObjectFields where FieldName = 'ProductTypeCode' and ObjectTableId = (select Id from ObjectTables where Name = 'CustomerAdditionalService')
delete from TextCodes where Code like '%ProductTypeCode%' and ObjectTableId = (select Id from ObjectTables where Name = 'CustomerAdditionalService')