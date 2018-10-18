delete  from QueryColumns where ObjectFieldId in (select Id from ObjectFields where ObjectTableId = (select Id from ObjectTables where Name = 'InboundEmail') and (FieldName = 'Id' or FieldName = 'Tenant') )
delete  from ObjectFields where ObjectTableId = (select Id from ObjectTables where Name = 'InboundEmail') and (FieldName = 'Id' or FieldName = 'Tenant') 

delete  from QueryColumns where ObjectFieldId in (select Id from ObjectFields where ObjectTableId = (select Id from ObjectTables where Name = 'InboundEmailLine') and (FieldName = 'Id' or FieldName = 'Tenant') )
delete  from ObjectFields where ObjectTableId = (select Id from ObjectTables where Name = 'InboundEmailLine') and (FieldName = 'Id' or FieldName = 'Tenant') 

delete  from QueryColumns where ObjectFieldId in (select Id from ObjectFields where ObjectTableId = (select Id from ObjectTables where Name = 'BusinessHour') and (FieldName = 'Id' or FieldName = 'Tenant') )
delete  from ObjectFields where ObjectTableId = (select Id from ObjectTables where Name = 'BusinessHour') and (FieldName = 'Id' or FieldName = 'Tenant') 

delete  from QueryColumns where ObjectFieldId in (select Id from ObjectFields where ObjectTableId = (select Id from ObjectTables where Name = 'BusinessHoursHoliday') and (FieldName = 'Id' or FieldName = 'Tenant') )
delete  from ObjectFields where ObjectTableId = (select Id from ObjectTables where Name = 'BusinessHoursHoliday') and (FieldName = 'Id' or FieldName = 'Tenant') 

delete  from ObjectFields where ObjectTableId = (select Id from ObjectTables where Name = 'BusinessHoursHoliday') and (FieldName = 'BusinessHourId') 
delete  from ObjectFields where ObjectTableId = (select Id from ObjectTables where Name = 'InboundEmailLine') and (FieldName = 'InboundEmailId') 
