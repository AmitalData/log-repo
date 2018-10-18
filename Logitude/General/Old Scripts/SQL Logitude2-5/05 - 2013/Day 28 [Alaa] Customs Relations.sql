-- with errors
alter table [Customs].[Consignments] add constraint [Consignment_CargoType] foreign key ([CargoTypeCode]) references [Customs].[CargoIdentifireTypes]([Code]);
go

alter table [Customs].[Consignments] add constraint [Consignment_Declaration] foreign key ([DeclarationId]) references [Customs].[Declarations]([Id]);
go

alter table [Customs].[Consignments] add constraint [Consignment_OriginCountry] foreign key ([OriginCountryCode]) references [Customs].[Countries]([Code]);
go

alter table [Customs].[Consignments] add constraint [Consignment_ReceiverWarehouse] foreign key ([ReceiverWarehouseCode]) references [Customs].[SiteLookups]([Code]);
go

alter table [Customs].[Consignments] add constraint [Consignment_StorageSite] foreign key ([StorageSiteCode]) references [Customs].[SiteLookups]([Code]);
go

alter table [Customs].[Consignments] add constraint [Consignment_UnloadPort] foreign key ([UnloadPortCode]) references [Customs].[SiteLookups]([Code]);
go

alter table [Customs].[ConsignmentPackages] add constraint [ConsignmentPackage_Consigment] foreign key ([DeclarationId], [ConsignmentNumber]) references [Customs].[Consignments]([DeclarationId], [ConsignmentNumber]);
go

alter table [Customs].[ConsignmentPackages] add constraint [ConsignmentPackage_PackageMeasureQualifier] foreign key ([PackageMeasureQualifierCode]) references [Customs].[PackageMeasureQualifiers]([Code]);
go

alter table [Customs].[ConsignmentPackages] add constraint [ConsignmentPackage_PackingType] foreign key ([PackageTypeCode]) references [Customs].[PackingTypes]([Code]);
go

alter table [Customs].[Declarations] add constraint [Declaration_AutonomyRegionType] foreign key ([AutonomyRegionTypeCode]) references [Customs].[AutonomyTypes]([Code]);
go

alter table [Customs].[Declarations] add constraint [Declaration_CustomerCard] foreign key ([CustomerId]) references [dbo].[Cards]([Id]);
go

alter table [Customs].[Declarations] add constraint [Declaration_DeclarationDocumentType] foreign key ([DeclarationDocumentTypeCode]) references [Customs].[LeadDocumentTypes]([Code]);
go

alter table [Customs].[Declarations] add constraint [Declaration_DeclarationOffice] foreign key ([DeclarationOfficeCode]) references [Customs].[SiteLookups]([Code]);
go

alter table [Customs].[Declarations] add constraint [Declaration_EntitleImporter] foreign key ([EntitleImporterId]) references [Customs].[Clients]([Id]);
go

alter table [Customs].[Declarations] add constraint [Declaration_EntitleImporterCountry] foreign key ([EntitleImporterCountryCode]) references [Customs].[Countries]([Code]);
go

alter table [Customs].[Declarations] add constraint [Declaration_Importer] foreign key ([ImporterId]) references [Customs].[Clients]([Id]);
go

alter table [Customs].[Declarations] add constraint [Declaration_ImporterEntitlementType] foreign key ([ImporterEntitlementTypeCode]) references [Customs].[EntitlementTypes]([Code]);
go

alter table [Customs].[Declarations] add constraint [Declaration_ImporterPassCountry] foreign key ([ImporterPassCountryCode]) references [Customs].[Countries]([Code]);
go

alter table [Customs].[Declarations] add constraint [Declaration_ProcedureCurrent] foreign key ([ProcedureCurrentCode]) references [Customs].[GovernmentProcedureTypes]([Code]);
go

alter table [Customs].[Declarations] add constraint [Declaration_TransferImporter] foreign key ([TransferImporterId]) references [Customs].[Clients]([Id]);
go

alter table [Customs].[Declarations] add constraint [Declaration_TransferImporterCountry] foreign key ([TransferImporterCountryCode]) references [Customs].[Countries]([Code]);
go

alter table [Customs].[PhysicalChecks] add constraint [PhysicalCheck_CargoIdentifireType] foreign key ([CargoIdentifierTypeCode]) references [Customs].[CargoIdentifireTypes]([Code]);
go

alter table [Customs].[PhysicalChecks] add constraint [PhysicalCheck_CheckEntityType] foreign key ([CargoTypeCode]) references [Customs].[CheckEntityTypes]([Code]);
go

alter table [Customs].[PhysicalChecks] add constraint [PhysicalCheck_CheckQueueType] foreign key ([QueueTypeCode]) references [Customs].[CheckQueueTypes]([Code]);
go

alter table [Customs].[PhysicalChecks] add constraint [PhysicalCheck_CheckRepresentativeType] foreign key ([InitiatorTypeCode]) references [Customs].[CheckRepresentativeTypes]([Code]);
go

alter table [Customs].[PhysicalChecks] add constraint [PhysicalCheck_CheckSite] foreign key ([CheckSiteCode]) references [Customs].[SiteLookups]([Code]);
go

alter table [Customs].[PhysicalChecks] add constraint [PhysicalCheck_Declaration] foreign key ([DeclarationId]) references [Customs].[Declarations]([Id]);
go

alter table [Customs].[PhysicalChecks] add constraint [PhysicalCheck_Operation] foreign key ([OperationCode]) references [Customs].[PhysicalCheckOperations]([Code]);
go

alter table [Customs].[PhysicalChecks] add constraint [PhysicalCheck_StatusMessage] foreign key ([StatusMessageCode]) references [Customs].[PhysicalCheckStatusMessages]([Code]);
go

alter table [Customs].[PhysicalChecks] add constraint [PhysicalCheck_StorageSite] foreign key ([StorageSiteCode]) references [Customs].[SiteLookups]([Code]);
go

alter table [Customs].[SiteLookups] add constraint [SiteLookup_SiteType] foreign key ([SiteTypeCode]) references [Customs].[SiteTypes]([Code]);
go

alter table [Customs].[Countries] add constraint [Country_CountryGroup] foreign key ([TarriffCode]) references [Customs].[CountryGroups]([Code]);
go

alter table [Customs].[Vendors] add constraint [Vendor_Country] foreign key ([CountryCode]) references [Customs].[Countries]([Code]);
go

alter table [Customs].[VendorCommunications] add constraint [VendorCommunication_CommunicationType] foreign key ([CommunicationTypeCode]) references [Customs].[CommunicationTypes]([Code]);
go

alter table [Customs].[VendorCommunications] add constraint [VendorCommunication_Vendor] foreign key ([VendorId]) references [Customs].[Vendors]([Id]);
go

-- Column 'Customs.VendorTypes.Code' is not the same data type as referencing column 'Vendors.VendorTypeCode'
-- Column 'Customs.SubCountries.Code' is not the same length or scale as referencing column 'Vendors.SubCountryCode'
alter table [Customs].[Vendors] add constraint [Vendor_SubCountry] foreign key ([SubCountryCode]) references [Customs].[SubCountries]([Code]);
go

alter table [Customs].[Vendors] add constraint [Vendor_VendorType] foreign key ([VendorTypeCode]) references [Customs].[VendorTypes]([Code]);
go
