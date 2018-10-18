

alter table AccountingSettings add AllowMinusInvoicelines bit not null default 0
go

alter table Currencies alter column AccountingExternalCode nvarchar(3) null
go

alter table ARInvoices alter column AccountingExternalCode nvarchar(3) null
go

alter table Shipments alter column Notes nvarchar(1000) null
go

alter table Cards alter column Notes nvarchar(1000) null
go