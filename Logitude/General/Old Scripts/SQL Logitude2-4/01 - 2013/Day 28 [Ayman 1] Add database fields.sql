
alter table ARInvoices add TransferError nvarchar(250) null
go

alter table AccountingTransferLines add Tenant int not null default 0
go

alter table AccountingTransferLines add EntityReference varchar(20) null
go