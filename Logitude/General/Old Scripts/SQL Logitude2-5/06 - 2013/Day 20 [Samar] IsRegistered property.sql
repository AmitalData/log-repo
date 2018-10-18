-- global db

alter table TenantManagements drop constraint DF__TenantMan__IsReg__286302EC
go

alter table TenantManagements drop column IsRegistered
go


use [Logitude2-5_Main]
go

delete from ObjectFields where FieldName = 'IsRegistered' and ObjectTableId = (select Id from ObjectTables where name = 'TenantManagement')
go

delete from TextCodes where code like '%IsRegistered%' and ObjectTableId = (select Id from ObjectTables where name = 'TenantManagement')
go

alter table Airlines add IsRegistered bit not null default 0
go
