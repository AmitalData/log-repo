-- execute on system Logs DB

alter table ContactActivityLogs add [CardId] varchar(15) null
go

alter table ContactActivityLogs add [PartnerTypeId] varchar(2) null
go