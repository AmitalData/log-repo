-- Jalal :  Run this script, one by one !
-- with errors 
alter table [Customs].[Countries] add 
    [TarriffCode] [varchar](2) null
go

create table [Customs].[SiteTypes] (
    [Code] [varchar](2) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [nvarchar](40) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
go
create table [Customs].[LeadDocumentTypes] (
    [Code] [varchar](3) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [nvarchar](40) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
go

create table [Customs].[PackageMeasureQualifiers] (
    [Code] [varchar](4) not null,
    [EnglishName] [varchar](100) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
go

create table [Customs].[PackingTypes] (
    [Code] [varchar](4) not null,
    [EnglishName] [varchar](100) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
go

create table [Customs].[LeadDocumentTypes] (
    [Code] [varchar](3) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [nvarchar](40) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
go

create table [Customs].[PackageMeasureQualifiers] (
    [Code] [varchar](4) not null,
    [EnglishName] [varchar](100) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
go

create table [Customs].[PackingTypes] (
    [Code] [varchar](4) not null,
    [EnglishName] [varchar](100) null,
    [LocalName] [nvarchar](100) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
go

create table [Customs].[AutonomyTypes] (
    [Code] [varchar](4) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [nvarchar](40) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
go

create table [Customs].[Clients] (
    [Id] [varchar](15) not null,
    [Tenant] [int] not null,
    [Code] [varchar](2) null,
    [EnglishName] [varchar](40) null,
    [LocalName] [nvarchar](40) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Id])
);
go

create table [Customs].[Consignments] (
    [DeclarationId] [varchar](15) not null,
    [ConsignmentNumber] [int] not null,
    [SequenceNumeric] [int] not null,
    [Tenant] [int] not null,
    [CargoTypeCode] [varchar](4) null,
    [ManifestDate] [datetime] null,
    [ManifestNumber] [varchar](35) null,
    [SecondCargoID] [varchar](35) null,
    [ThirdCargoID] [varchar](35) null,
    [UnloadDate] [datetime] null,
    [UnloadPortCode] [varchar](17) null,
    [CargoDescription] [varchar](256) null,
    [IsLastReleaseFromWarehous] [bit] not null,
    [LoadingPortCode] [varchar](17) null,
    [OriginCountryCode] [varchar](2) null,
    [StorageSiteCode] [varchar](17) null,
    [ReceiverWarehouseCode] [varchar](17) null,
    primary key ([DeclarationId], [ConsignmentNumber])
);
create table [Customs].[ConsignmentPackages] (
    [DeclarationId] [varchar](15) not null,
    [ConsignmentNumber] [int] not null,
    [LineNumber] [int] not null,
    [Tenant] [int] not null,
    [PackageMeasureQualifierCode] [varchar](4) null,
    [PackageQuantity] [int] not null,
    [GrossMassMeasure] [decimal](18, 2) null,
    [PackageTypeCode] [varchar](4) null,
    [MarksNumbers] [varchar](512) null,
    primary key ([DeclarationId], [ConsignmentNumber], [LineNumber])
);

create table [Customs].[CountryGroups] (
    [Code] [varchar](2) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [nvarchar](40) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
go

create table [Customs].[EntitlementTypes] (
    [Code] [varchar](3) not null,
    [EnglishName] [varchar](40) null,
    [LocalName] [nvarchar](40) null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
); 
go
 


alter table [Customs].[Declarations] add  
      [VersionId] [varchar](9) null,
    [IssueDateTime] [datetime] null,
    [AgentFileReferenceNumber] [varchar](35) null,
    [ExternalDeclarationNumber] [varchar](35) null,
    [DeclarationOfficeCode] [varchar](17) null,
    [TaxationDateTime] [datetime] null,
    [AgentId] [varchar](4) null,
    [ProcedureCurrentCode] [varchar](7) null,
    [AutonomyRegionTypeCode] [varchar](4) null,
    [ImporterId] [varchar](15) null,
    [ImporterPassCountryCode] [varchar](2) null,
    [TransferImporterId] [varchar](15) null,
    [TransferImporterCountryCode] [varchar](2) null,
    [EntitleImporterId] [varchar](15) null,
    [ImporterEntitlementTypeCode] [varchar](3) null,
    [EntitleImporterCountryCode] [varchar](2) null,
    [DeclarationDocumentId] [varchar](35) null,
    [DeclarationDocumentTypeCode] [varchar](3) null,
    [CreatedByUserId] [varchar](15) null,
    [IsChanged] [bit] not null default 0,
    [PaymentDate] [datetime] null,
    [HatraDate] [datetime] null,
    [DeclarationStatusCode] [varchar](2) null,
    [LoadingFactor] [decimal](18, 2) null,
    [ConstraintCode] [varchar](1) null,
    [DealValue] [decimal](18, 2) null,
    [CIFValue] [decimal](18, 2) null,
    [TotalTax] [decimal](18, 2) null
	go

alter table [Customs].[Declarations] drop column ImporterNumber
go


exec sp_rename 'Customs.Sites', 'SiteLookups'; 
go

alter table customs.sitelookups add SiteTypeCode varchar(2) null
go

alter table [Customs].[PhysicalChecks] drop constraint [PhysicalCheck_StorageSite]
go

alter table [Customs].[PhysicalChecks] drop constraint [PhysicalCheck_CheckSite]
go

alter table customs.Physicalchecks alter column StorageSiteCode varchar(17) null
go
alter table customs.Physicalchecks alter column CheckSiteCode varchar(17) null
go

alter table customs.sitelookups alter column Code varchar(17)not null
go

alter table customs.vendors alter column VendorTypeCode varchar(4) null
go

--drop the key from vendortypes then add the script below
alter table customs.vendortypes drop PK__VendorTy__A25C5AA64356F04A
go

alter table customs.vendortypes alter column Code varchar(4) not null
go

alter table customs.vendortypes add primary key (Code)

--drop the key from sitelookups then add the script below

alter table customs.sitelookups drop PK__Sites__A25C5AA62EE5E349
go


alter table customs.sitelookups alter column Code varchar(17) not null
go

alter table customs.sitelookups add primary key (Code)

alter table customs.subcountries alter column Code varchar(6) not null
go

alter table customs.consignmentpackages alter column packagetypecode varchar(4) not null
go

alter table [Customs].[PhysicalChecks] add constraint [PhysicalCheck_CheckSite] foreign key ([CheckSiteCode]) references [Customs].[SiteLookups]([Code]);
alter table [Customs].[PhysicalChecks] add constraint [PhysicalCheck_StorageSite] foreign key ([StorageSiteCode]) references [Customs].[SiteLookups]([Code]);
alter table [Customs].[Consignments] add constraint [Consignment_CargoType] foreign key ([CargoTypeCode]) references [Customs].[CargoIdentifireTypes]([Code]);
alter table [Customs].[Consignments] add constraint [Consignment_Declaration] foreign key ([DeclarationId]) references [Customs].[Declarations]([Id]);
alter table [Customs].[Consignments] add constraint [Consignment_OriginCountry] foreign key ([OriginCountryCode]) references [Customs].[Countries]([Code]);
alter table [Customs].[Consignments] add constraint [Consignment_ReceiverWarehouse] foreign key ([ReceiverWarehouseCode]) references [Customs].[SiteLookups]([Code]);
alter table [Customs].[Consignments] add constraint [Consignment_StorageSite] foreign key ([StorageSiteCode]) references [Customs].[SiteLookups]([Code]);
alter table [Customs].[Consignments] add constraint [Consignment_UnloadPort] foreign key ([UnloadPortCode]) references [Customs].[SiteLookups]([Code]);
alter table [Customs].[ConsignmentPackages] add constraint [ConsignmentPackage_Consigment] foreign key ([DeclarationId], [ConsignmentNumber]) references [Customs].[Consignments]([DeclarationId], [ConsignmentNumber]);
alter table [Customs].[ConsignmentPackages] add constraint [ConsignmentPackage_PackageMeasureQualifier] foreign key ([PackageMeasureQualifierCode]) references [Customs].[PackageMeasureQualifiers]([Code]);
alter table [Customs].[ConsignmentPackages] add constraint [ConsignmentPackage_PackingType] foreign key ([PackageTypeCode]) references [Customs].[PackingTypes]([Code]);
alter table [Customs].[Countries] add constraint [Country_CountryGroup] foreign key ([TarriffCode]) references [Customs].[CountryGroups]([Code]);
alter table [Customs].[Declarations] add constraint [Declaration_AutonomyRegionType] foreign key ([AutonomyRegionTypeCode]) references [Customs].[AutonomyTypes]([Code]);
alter table [Customs].[Declarations] add constraint [Declaration_CustomerCard] foreign key ([CustomerId]) references [dbo].[Cards]([Id]);
alter table [Customs].[Declarations] add constraint [Declaration_DeclarationDocumentType] foreign key ([DeclarationDocumentTypeCode]) references [Customs].[LeadDocumentTypes]([Code]);
alter table [Customs].[Declarations] add constraint [Declaration_DeclarationOffice] foreign key ([DeclarationOfficeCode]) references [Customs].[SiteLookups]([Code]);
alter table [Customs].[Declarations] add constraint [Declaration_EntitleImporter] foreign key ([EntitleImporterId]) references [Customs].[Clients]([Id]);
alter table [Customs].[Declarations] add constraint [Declaration_EntitleImporterCountry] foreign key ([EntitleImporterCountryCode]) references [Customs].[Countries]([Code]);
alter table [Customs].[Declarations] add constraint [Declaration_Importer] foreign key ([ImporterId]) references [Customs].[Clients]([Id]);
alter table [Customs].[Declarations] add constraint [Declaration_ImporterEntitlementType] foreign key ([ImporterEntitlementTypeCode]) references [Customs].[EntitlementTypes]([Code]);
alter table [Customs].[Declarations] add constraint [Declaration_ImporterPassCountry] foreign key ([ImporterPassCountryCode]) references [Customs].[Countries]([Code]);
alter table [Customs].[Declarations] add constraint [Declaration_ProcedureCurrent] foreign key ([ProcedureCurrentCode]) references [Customs].[GovernmentProcedureTypes]([Code]);
alter table [Customs].[Declarations] add constraint [Declaration_TransferImporter] foreign key ([TransferImporterId]) references [Customs].[Clients]([Id]);
alter table [Customs].[Declarations] add constraint [Declaration_TransferImporterCountry] foreign key ([TransferImporterCountryCode]) references [Customs].[Countries]([Code]);
alter table [Customs].[SiteLookups] add constraint [SiteLookup_SiteType] foreign key ([SiteTypeCode]) references [Customs].[SiteTypes]([Code]);

alter table [Customs].[Vendors] add constraint [Vendor_Country] foreign key ([CountryCode]) references [Customs].[Countries]([Code]);
alter table [Customs].[Vendors] add constraint [Vendor_SubCountry] foreign key ([SubCountryCode]) references [Customs].[SubCountries]([Code]);
alter table [Customs].[Vendors] add constraint [Vendor_VendorType] foreign key ([VendorTypeCode]) references [Customs].[VendorTypes]([Code]);
alter table [Customs].[VendorCommunications] add constraint [VendorCommunication_CommunicationType] foreign key ([CommunicationTypeCode]) references [Customs].[CommunicationTypes]([Code]);
alter table [Customs].[VendorCommunications] add constraint [VendorCommunication_Vendor] foreign key ([VendorId]) references [Customs].[Vendors]([Id]);
