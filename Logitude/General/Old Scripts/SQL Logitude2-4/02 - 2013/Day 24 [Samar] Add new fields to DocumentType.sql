-- excute on Logitude2-4_Main

alter table DocumentTypes add IsCustomerView bit not null default 0
go

alter table DocumentTypes add IsAgentView bit not null default 0
go