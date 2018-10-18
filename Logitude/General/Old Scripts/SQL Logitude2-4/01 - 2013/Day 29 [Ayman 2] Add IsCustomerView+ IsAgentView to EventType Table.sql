

alter table EventTypes add IsCustomerView bit not null default 0
go

alter table EventTypes add IsAgentView bit not null default 0
go