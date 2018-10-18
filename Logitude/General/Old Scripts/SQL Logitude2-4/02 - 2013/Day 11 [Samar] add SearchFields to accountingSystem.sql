

alter table accountingSystems add SearchFields nvarchar(1000) NULL
go

alter table accountingSettings add TempCard1 varchar(15) NULL
go

alter table Currencies add AccountingExternalCode varchar (3) NULL
go

alter table ARInvoices add AccountingExternalCode varchar (3) NULL
go

alter table VatTypes add ExternalVATCard varchar (15) NULL
go

alter table ARInvoiceTotalVats add ExternalVATCard varchar (15) NULL
go