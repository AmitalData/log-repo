update ObjectFields set CanFilter = 1 where ObjectTableId = (select id from ObjectTables where name = 'customer') and
 (FieldName = 'ActiveCustomers' or FieldName = 'PotentialCustomers' or FieldName = 'InactiveCustomers')



 select * from ObjectFields where CanFilter = 0 and id in(select ObjectFieldId from AdvancedQueryFilters)
 update ObjectFields set CanFilter = 1 where  CanFilter = 0 and id in(select ObjectFieldId from AdvancedQueryFilters)