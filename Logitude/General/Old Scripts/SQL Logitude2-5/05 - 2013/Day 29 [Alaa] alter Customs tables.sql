-- with errors

alter table [customs].[SupplierInvoices] alter column IssueDate datetime null

alter table [Customs].[SupplierInvoices] alter column InvoiceAmount decimal(18, 2) null

alter table [Customs].[SupplierInvoices] alter column ActualPayedAmount decimal(18 , 2) null

alter table [Customs].[SupplierInvoices] alter column TotalFreightInInvoiceCurrency decimal(18, 2 ) null

alter table [Customs].[SupplierInvoices] alter column TotalFreightInNIS decimal(18, 2) null

alter table [Customs].[SupplierInvoices] alter column TotalInsuranceInInvoiceCurrency decimal(18 , 2) null

alter table [Customs].[SupplierInvoices] alter column TotalInsuranceInNIS decimal(18,2) null

alter table [Customs].[SupplierInvoices] alter column ExchangeRate decimal (18, 2) null


alter table [Customs].[SupplierInvoiceItems] alter column ItemPrice decimal (18, 2)  null
alter table [Customs].[SupplierInvoiceItems] alter column NonCustomsItemPrice decimal (18, 2)  null
alter table [Customs].[SupplierInvoiceItems] alter column WholeSaleItemPrice decimal (18, 2)  null
alter table [Customs].[SupplierInvoiceItems] alter column OptionalTamaPercentage decimal (18, 2)  null

alter table [Customs].[SupplierInvoiceItemsQuantities] alter column Quantity decimal (18,2) null

alter table [Customs].[PhysicalChecks] alter column CargoIdentifierKey1 varchar(35) null
alter table [Customs].[PhysicalChecks] alter column CargoIdentifierKey2 varchar(35) null
alter table [Customs].[PhysicalChecks] alter column CargoIdentifierKey3 varchar(35) null
alter table [Customs].[PhysicalChecks] alter column CheckEssence nvarchar(255) null
alter table [Customs].[PhysicalChecks] alter column RowNumber varchar (4) null

alter table [Customs].[Consignments] drop constraint DF__Consignme__Seque__3C0AD43D
alter table [Customs].[Consignments] drop column SequenceNumeric 

alter table [Customs].[ConsignmentPackages] alter column PackageQuantity int null

alter table [Customs].[VendorCommunications] alter column CommunicationTypeCode varchar(2) null

alter table [Customs].[InvoiceTypes] alter column  EnglishName varchar(40) null







