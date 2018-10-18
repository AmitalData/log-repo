
-- apply on main db

delete from ObjectFields where FieldName='id' and ObjectTableId =(select Id from ObjectTables where Name='Declaration')
delete from ObjectFields where FieldName='tenant' and ObjectTableId =(select Id from ObjectTables where Name='Declaration')

delete from ObjectFields where FieldName='id' and ObjectTableId =(select Id from ObjectTables where Name='PhysicalCheck')
delete from ObjectFields where FieldName='tenant' and ObjectTableId =(select Id from ObjectTables where Name='PhysicalCheck')