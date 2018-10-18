
delete from ObjectFields where ObjectTableId = (select Id from ObjectTables where Name = 'BusinessHoursHoliday') and FieldName = 'Inactive'

delete from TextCodes where ObjectTableId = (select Id from ObjectTables where Name = 'BusinessHoursHoliday') and Code like '%Inactive%' 





