
delete from ObjectFields where ObjectTableId = (select Id from ObjectTables where Name = 'Priority')
delete from TextCodes where ObjectTableId = (select Id from ObjectTables where Name = 'Priority')
delete from ObjectTableLastUpdates where ObjectTableId = (select Id from ObjectTables where Name = 'Priority')
delete from ObjectTables where Name = 'Priority'