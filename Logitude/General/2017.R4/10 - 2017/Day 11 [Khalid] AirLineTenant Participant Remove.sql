
update ObjectFields set IsRequiered='false' 
where FieldName = 'AirlineTenant'
and ObjectTableId = (select Id from ObjectTables where Name ='Participant')


