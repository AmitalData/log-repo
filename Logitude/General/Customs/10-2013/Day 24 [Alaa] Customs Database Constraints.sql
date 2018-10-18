


alter table [Customs].[Consignments] add constraint [Consignment_CargoType] foreign key ([CargoTypeCode]) references [Customs].[CargoIdentifireTypes]([Code]);

alter table [Customs].[CustomsCountries] add constraint [CustomsCountry_CountryGroup] foreign key ([TarriffCode]) references [Customs].[CountryGroups]([Code]);

alter table [Customs].[CustomsVendors] add constraint [CustomsVendor_Country] foreign key ([CountryCode]) references [Customs].[CustomsCountries]([Code]);

alter table [Customs].[CustomsVendors] add constraint [CustomsVendor_SubCountry] foreign key ([SubCountryCode]) references [Customs].[SubCountries]([Code]);



alter table [customs].[CustomsVendors] alter column VendorTypeCode varchar(4) not null;

alter table [Customs].[CustomsVendors] add constraint [CustomsVendor_VendorType] foreign key ([VendorTypeCode]) references [Customs].[VendorTypes]([Code]);

alter table [Customs].[DeclarationConstraints] add constraint [DeclarationConstraint_Declaration] foreign key ([DeclarationID]) references [Customs].[Declarations]([Id]);

alter table [Customs].[DeclarationPayments] add constraint [DeclarationPayment_User] foreign key ([CreatedByUserId]) references [dbo].[Users]([Id]);

alter table [Customs].[DeclarationPaymentMethods] add constraint [DeclarationPaymentMethod_CustomBank] foreign key ([InternalBankId]) references [Customs].[CustomBanks]([Id]);

alter table [Customs].[DeclarationPaymentMethods] add constraint [DeclarationPaymentMethod_DeclarationPayment] foreign key ([DeclarationId]) references [Customs].[DeclarationPayments]([DeclarationId]);

alter table [Customs].[DeclarationPaymentProtests] add constraint [DeclarationPaymentProtest_DeclarationPayment] foreign key ([DeclarationId]) references [Customs].[DeclarationPayments]([DeclarationId]);


alter table [Customs].[DeficitConnectedFileParagraphTypes] alter column ParagraphTypeCode varchar(3) not null;

alter table [Customs].[DeficitConnectedFileParagraphTypes] add constraint [DeficitConnectedFileParagraphType_ParagraphType] foreign key ([ParagraphTypeCode]) references [Customs].[ParagraphTypes]([Code]);

alter table [Customs].[PhysicalChecks] add constraint [PhysicalCheck_CargoIdentifireType] foreign key ([CargoIdentifierTypeCode]) references [Customs].[CargoIdentifireTypes]([Code]);


alter table [Customs].[SupplierInvoices] add constraint [SupplierInvoice_InvoiceType] foreign key ([AccountTypeCode]) references [Customs].[InvoiceTypes]([Code]);


alter table [Customs].[SupplierInvoiceItemsSerialNumbers] add constraint [SupplierInvoiceItemsSerialNumber_CargoIdentifireType] foreign key ([TypeCode]) references [Customs].[CargoIdentifireTypes]([Code]);


alter table [Customs].[SupplierInvoiceItemsTaxesModifications] add constraint [SupplierInvoiceItemsTaxesModification_SupplierInvoiceItemsTax] foreign key ([DeclarationId], [InvoiceCounterKey], [LineNumber], [TaxTypeCode]) references [Customs].[SupplierInvoiceItemsTaxes]([DeclarationId], [InvoiceCounterKey], [LineNumber], [TaxTypeCode]);




