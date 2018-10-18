alter table PackageTypes 
alter column TEU float not null
go

alter table shipments add TEU float 
go

update PackageTypes set TEU=2.25
where code like '45%'

