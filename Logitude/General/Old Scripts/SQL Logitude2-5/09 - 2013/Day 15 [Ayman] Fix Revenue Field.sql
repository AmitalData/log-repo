
-- 1 Run this Script
-- 2 Update Tenants

begin transaction
begin

alter table CustomerProducts add PotentialRevenue decimal null 
alter table CustomerProducts add CommitmentRevenue decimal null 

alter table CustomerProductLocations add PotentialRevenue decimal null 
alter table CustomerProductLocations add CommitmentRevenue decimal null

alter table CustomerProducts drop column Revenue
alter table CustomerProductLocations drop column Revenue

delete from ObjectFields where FieldName = 'Revenue' and ObjectTableId = (select Id from ObjectTables where Name = 'CustomerProduct')
delete from ObjectFields where FieldName = 'Revenue' and ObjectTableId = (select Id from ObjectTables where Name = 'CustomerProductLocation')

delete from TextCodes where Code like '%CustomerProduct.%Revenue%'
delete from TextCodes where Code like '%CustomerProductLocation.%Revenue%'

END
commit transaction

select * from CustomerProductLocations