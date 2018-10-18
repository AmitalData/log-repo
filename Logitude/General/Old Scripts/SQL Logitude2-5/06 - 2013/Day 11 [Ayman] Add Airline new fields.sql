
alter table Airlines add NeedsRegistration bit not null default 0
go

alter table Airlines add IsRegistered bit not null default 0
go