delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'BillToName' and ObjectTableId = (select Id from ObjectTables where Name = 'Customer'))
delete from QueryColumns where ObjectFieldId = (select Id from ObjectFields where FieldName = 'BillToId' and ObjectTableId = (select Id from ObjectTables where Name = 'Customer'))

delete from ObjectFields where FieldName = 'BillToId' and ObjectTableId = (select Id from ObjectTables where Name = 'Customer')
delete from ObjectFields where FieldName = 'BillToName' and ObjectTableId = (select Id from ObjectTables where Name = 'Customer')

delete from TextCodes where Code like '%BillTo%' and ObjectTableId = (select Id from ObjectTables where Name = 'Customer')