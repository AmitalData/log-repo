alter table [Customs].[SupplierInvoiceItems] add ItemPriceCurrencyCode varchar(3) null
alter table [Customs].[SupplierInvoiceItems] add NonCustomsItemPriceCurrencyCode varchar(3) null
alter table [Customs].[SupplierInvoiceItems] add WholeSaleItemPriceCurrencyCode varchar(3) null


alter table [Customs].[SupplierInvoiceItems] add constraint [SupplierInvoiceItem_ItemPriceCurrency] foreign key ([ItemPriceCurrencyCode]) references [Customs].[CurrencyTypes]([Code]);
alter table [Customs].[SupplierInvoiceItems] add constraint [SupplierInvoiceItem_NonCustomsItemPriceCurrency] foreign key ([NonCustomsItemPriceCurrencyCode]) references [Customs].[CurrencyTypes]([Code]);
alter table [Customs].[SupplierInvoiceItems] add constraint [SupplierInvoiceItem_WholeSaleItemPriceCurrency] foreign key ([WholeSaleItemPriceCurrencyCode]) references [Customs].[CurrencyTypes]([Code]);

