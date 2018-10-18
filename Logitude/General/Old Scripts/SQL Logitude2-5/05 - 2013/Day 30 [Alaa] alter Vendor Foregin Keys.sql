-- with errors

alter table [Customs].[Vendors] drop constraint [Vendor_VendorType]
go

alter table [Customs].[VendorTypes] drop PK__VendorTy__A25C5AA668536ACF 
go

alter table [Customs].[VendorTypes] alter column Code varchar (2) not null
go


alter table [Customs].[Vendors] alter column VendorTypeCode varchar(2) null 
go

ALTER TABLE [Customs].[VendorTypes]
ADD CONSTRAINT [PK__VendorTy]
    PRIMARY KEY CLUSTERED ([Code] ASC);
GO

alter table [Customs].[Vendors] add constraint [Vendor_VendorType] foreign key ([VendorTypeCode]) references [Customs].[VendorTypes]([Code]);
go

alter table [Customs].[Vendors] drop constraint [Vendor_SubCountry]
go

alter table [Customs].[SubCountries] drop PK__SubCount__A25C5AA64262CC11
go

alter table [Customs].[SubCountries] alter column Code varchar (6) not null
go

ALTER TABLE [Customs].[SubCountries]
ADD CONSTRAINT [PK__SubCount]
    PRIMARY KEY CLUSTERED ([Code] ASC);
GO

alter table [Customs].[Vendors] add constraint [Vendor_SubCountry] foreign key ([SubCountryCode]) references [Customs].[SubCountries]([Code]);
go
