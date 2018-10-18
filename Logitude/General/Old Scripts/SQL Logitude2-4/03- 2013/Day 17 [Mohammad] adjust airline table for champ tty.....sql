alter table Airlines add TTY varchar(7) null
go

alter table Airlines add FSU bit not null default 0
go

alter table Airlines add FSRFSA bit not null default 0
go

alter table Airlines add FWB bit not null default 0
go

alter table Airlines add FHL bit not null default 0
go

alter table Airlines add FVRFVA bit not null default 0
go
