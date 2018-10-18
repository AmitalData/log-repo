

update ObjectFields set FieldName = 'StateId' where FieldName = 'State' and ObjectTableId = (Select Id from ObjectTables where Name = 'Address') 