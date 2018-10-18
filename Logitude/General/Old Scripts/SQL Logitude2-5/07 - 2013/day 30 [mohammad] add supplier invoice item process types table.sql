create table [Customs].[SupplierInvoiceItemsProcessTypes] (
    [DeclarationId] [varchar](15) not null,
    [InvoiceCounterKey] [int] not null,
    [InvoiceItemLineNumber] [int] not null,
    [LineNumber] [int] not null,
    [Tenant] [int] not null,
    [ProcessTypeCode] [varchar](7) null,
    primary key ([DeclarationId], [InvoiceCounterKey], [InvoiceItemLineNumber], [LineNumber])
);

alter table [Customs].[SupplierInvoiceItemsProcessTypes] add constraint [SupplierInvoiceItemsProcessType_ProcessType] foreign key ([ProcessTypeCode]) references [Customs].[ItemGovernmentProcedureTypes]([Code]);
alter table [Customs].[SupplierInvoiceItemsProcessTypes] add constraint [SupplierInvoiceItemsProcessType_SupplierInvoiceItem] foreign key ([DeclarationId], [InvoiceCounterKey], [InvoiceItemLineNumber]) references [Customs].[SupplierInvoiceItems]([DeclarationId], [CounterKey], [LineNumber]);
