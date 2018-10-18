
delete from ScreenFields where ObjectFieldId=(select id from ObjectFields where FieldName='DeclarationId' and ObjectTableId =(select Id from ObjectTables where Name='PhysicalCheck'))
delete from QueryColumns where ObjectFieldId=(select id from ObjectFields where FieldName='DeclarationId' and ObjectTableId =(select Id from ObjectTables where Name='PhysicalCheck'))
delete from ObjectFields where FieldName='DeclarationId' and ObjectTableId =(select Id from ObjectTables where Name='PhysicalCheck')