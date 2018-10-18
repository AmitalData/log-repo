
alter table Tenants add LocalCustomsCode varchar(50) null
go

alter table ShippingAgents add LocalCustomsCode varchar(50) null
go


select * from Queries where objecttableId=(select Id from ObjectTables where name='Customs.PhysicalCheck')
select * from features where id='1-5772'
select * from features where code='BYUPCOMINGCHECK'

select * from packagefeatures where featureid='1-6342'

select * from packagefeatures where packagecode='CUST' and FeatureId in (select id from features where objecttableid=(select Id from ObjectTables where name='Customs.PhysicalCheck'))
update queries set featureid='1-5775' where id ='1-18310'