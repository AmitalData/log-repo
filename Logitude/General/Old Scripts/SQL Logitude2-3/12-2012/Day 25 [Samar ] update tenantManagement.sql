alter table TenantManagements drop constraint DF__TenantMan__IsBlo__20C1E124
go
alter table TenantManagements drop column IsBlocked
go

alter table TenantManagements drop constraint DF__TenantMan__IsRec__1FCDBCEB
go
alter table TenantManagements drop column IsRecurring
go


alter table TenantManagements add [IsRecurring] bit NOT NULL default 0 
go

alter table TenantManagements add [IsBlocked] bit NOT NULL default 0 
go