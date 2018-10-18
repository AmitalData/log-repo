delete from QueryColumns 
where ObjectFieldId in (select Id from ObjectFields where (ObjectTableId = (select id from ObjectTables where Name='CustomerTenantAccess') and FieldName='RelatedCustomer'))

delete from ObjectFields 
where ObjectTableId in (select id from ObjectTables where Name='CustomerTenantAccess')and FieldName='RelatedCustomer'




