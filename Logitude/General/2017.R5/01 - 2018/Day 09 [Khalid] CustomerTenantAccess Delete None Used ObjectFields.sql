--> Please run this script at Main db 
delete  from QueryColumns where ObjectFieldId=(select Id from ObjectFields where ObjectTableId=(Select Id from ObjectTables Where Name='CustomertenantAccess')and FieldName='Status')
delete from ObjectFields where ObjectTableId=(Select Id from ObjectTables Where Name='CustomertenantAccess')and FieldName='Status'
delete from ObjectFields where ObjectTableId=(Select Id from ObjectTables Where Name='CustomertenantAccess')and FieldName='CustomerIdInCustomerTenant'