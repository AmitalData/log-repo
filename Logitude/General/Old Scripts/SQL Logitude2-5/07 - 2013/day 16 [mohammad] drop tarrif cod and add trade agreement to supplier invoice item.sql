
alter table Customs.SupplierInvoiceItems drop SupplierInvoiceItem_TariffCodeType
go


alter table Customs.SupplierInvoiceItems drop Column TariffCode
go

alter table Customs.SupplierInvoiceItems add TradeAgreementCode varchar(3) null
go

alter table [Customs].[SupplierInvoiceItems] add constraint [SupplierInvoiceItem_TradeAgreement] foreign key ([TradeAgreementCode]) references [Customs].[TradeAgreements]([Code]);


delete from ObjectFields where FieldName ='TariffCode' and ObjectTableId=(select id from ObjectTables where Name='Customs.SupplierInvoiceItem')
delete from TextCodes where Code like '%TariffCode%' and ObjectTableId=(select id from ObjectTables where Name='Customs.SupplierInvoiceItem')