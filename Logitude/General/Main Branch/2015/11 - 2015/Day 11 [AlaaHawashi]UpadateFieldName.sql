update ObjectFields 
set FieldName='CompanyVat'
where ObjectTableId in (select id from ObjectTables where Name='CustomerTenantAccess') and FieldName='CompanyVAT'