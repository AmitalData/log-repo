-- global db

alter table TenantManagements add IsRegistered bit not null default 0
go