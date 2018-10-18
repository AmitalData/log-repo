alter table [Customs].[Consignments] drop constraint [Consignment_CargoType]
GO 
alter table [Customs].[PhysicalChecks] drop constraint [PhysicalCheck_CargoIdentifireType] 
GO
alter table [Customs].[SupplierInvoiceItemsSerialNumbers] drop constraint [SupplierInvoiceItemsSerialNumber_CargoIdentifireType] 
GO
alter table [Customs].[CargoIdentifireTypes] drop  constraint PK__CargoIde__A25C5AA610615C29
GO
alter table [Customs].[CargoIdentifireTypes] alter column code varchar(3) not null
GO
alter table [Customs].[Consignments] alter column cargoTypeCode varchar (3)
GO
alter table [Customs].[PhysicalChecks] alter column CargoIdentifierTypeCode varchar(3)
GO
alter table [Customs].[SupplierInvoiceItemsSerialNumbers] alter column TypeCode varchar(3)
GO

alter table [Customs].[CargoIdentifireTypes] add constraint pk_CargoId_Code primary key (code)
GO

alter table [Customs].[Consignments] add constraint [Consignment_CargoType] foreign key ([CargoTypeCode]) references [Customs].[CargoIdentifireTypes]([Code]);
go
alter table [Customs].[PhysicalChecks] add constraint [PhysicalCheck_CargoIdentifireType] foreign key ([CargoIdentifierTypeCode]) references [Customs].[CargoIdentifireTypes]([Code]);
go
alter table [Customs].[SupplierInvoiceItemsSerialNumbers] add constraint [SupplierInvoiceItemsSerialNumber_CargoIdentifireType] foreign key ([TypeCode]) references [Customs].[CargoIdentifireTypes]([Code]);
go





