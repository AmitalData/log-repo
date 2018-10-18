
-- Run this SQL
-- update Tenants

delete from QueryColumns
where
QueryId in (select Id from Queries where ObjectTableId = (select Id from ObjectTables where Name = 'Customer'))
AND
ObjectFieldId = (select Id from ObjectFields where FieldName = 'IsCustomer' and ObjectTableId = (select Id from ObjectTables where Name = 'Customer'))
