using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel
{
    public interface ICommonDataContext : IContext
    {
        IDbSet<AddressType> AddressTypes { get; }
        IDbSet<Country> Countries { get; }
        IDbSet<State> States { get; }
        IDbSet<GlobalZone> GlobalZones { get; }
        IDbSet<Address> Addresses { get; }
        IDbSet<Port> Ports { get; }
        IDbSet<EntityDate> EntityDates { get; }
        IDbSet<Card> Cards { get; }
        IDbSet<Customer> Customers { get; }
        IDbSet<Agent> Agents { get; }
        IDbSet<CustomAgent> CustomAgents { get; }
        IDbSet<ShippingAgent> ShippingAgents { get; }
        IDbSet<Contact> Contacts { get; }
        IDbSet<CardContact> CardContacts { get; }
        IDbSet<PartnerType> PartnerTypes { get; }
        IDbSet<Rank> Ranks { get; }
        IDbSet<User> Users { get; }
        IDbSet<Department> Departments { get; }
        IDbSet<Branch> Branches { get; }
        IDbSet<Airline> Airlines { get; }
        IDbSet<CarrierArea> CarrierAreas { get; }
        IDbSet<CarrierAreasPort> CarrierAreasPorts { get; }

        IDbSet<ShippingLine> ShippingLines { get; }
        IDbSet<Trucker> Truckers { get; }
        IDbSet<Tenant> Tenants { get; }
        IDbSet<Currency> Currencies { get; }
        IDbSet<ContactTenant> ContactTenants { get; }
        IDbSet<ContactTenantRole> ContactTenantRoles { get; }
        IDbSet<Role> Roles { get; }
        IDbSet<PaymentTerm> PaymentTerms { get; }
        IDbSet<VatType> VatTypes { get; }
        IDbSet<Incoterm> Incoterms { get; }
        IDbSet<ChargesType> ChargesTypes { get; }
        IDbSet<Measurement> Measurements { get; }
        IDbSet<Document> Documents { get; }
        IDbSet<DocumentType> DocumentTypes { get; }
        IDbSet<DocumentOut> DocumentOuts { get; }
        IDbSet<PackageType> PackageTypes { get; }
        IDbSet<DocumentTypeCustomField> DocumentTypeCustomFields { get; }
        IDbSet<FormCustomField> FormCustomFields { get; }
        IDbSet<WeightUnit> WeightUnits { get; }
        IDbSet<DimensionsUnit> DimensionsUnits { get; }
        IDbSet<RateClass> RateClasses { get; }
        IDbSet<CommunicationLog> CommunicationLogs { get; }
        IDbSet<CommunicationAttachment> CommunicationAttachments { get; }
        IDbSet<DueType> DueTypes { get; }
        IDbSet<CommunicationStatusType> CommunicationStatusTypes { get; }
        IDbSet<CommunicationLogType> CommunicationLogTypes { get; }
        IDbSet<DocumentTypeTemplate> DocumentTypeTemplates { get; }
        IDbSet<TemplateFormat> TemplateFormats { get; }
        IDbSet<Warehouse> Warehouses { get; }
        IDbSet<Vessel> Vessels { get; }
        IDbSet<MAWBStack> MAWBStacks { get; }
        IDbSet<DocumentTypeCopy> DocumentTypeCopies { get; }
        IDbSet<DocumentOutCopy> DocumentOutCopies { get; }
        IDbSet<Feature> Features { get; }
        IDbSet<RoleFeature> RoleFeatures { get; }
        IDbSet<Restriction> Restrictions { get; }
        IDbSet<VatTypePercentage> VatTypePercentages { get; }
        IDbSet<TarrifHeader> TarrifHeaders { get; }
        IDbSet<TarrifType> TarrifTypes { get; }
        IDbSet<TarrifCharge> TarrifCharges { get; }
        IDbSet<TarrifFromTo> TarrifFromToes { get; }
        IDbSet<TarrifFromToType> TarrifFromToTypes { get; }
        IDbSet<TarrifStep> TarrifSteps { get; }
        IDbSet<UserLoginLog> UserLoginLogs { get; }
        IDbSet<PasswordPolicy> PasswordPolicies { get; }
        IDbSet<Vendor> Vendors { get; }
        IDbSet<AccountingSetting> AccountingSettings { get; }
        IDbSet<AccountingSystem> AccountingSystems { get; }
        IDbSet<FeatureType> FeatureTypes { get; }
        IDbSet<RoleType> RoleTypes { get; }
        IDbSet<Package> Packages { get; }
        IDbSet<PackageFeature> PackageFeatures { get; }
        IDbSet<UserLastLogin> UserLastLogins { get; }
        IDbSet<TermsofUse> TermsofUses { get; }
        IDbSet<TermsofUseSignature> TermsofUseSignatures { get; }
        IDbSet<ChargeTypeAccounting> ChargeTypeAccountings { get; }
        IDbSet<ContactLastLogin> ContactLastLogins { get; }
        IDbSet<SharedLogisticsContactLastLogin> SharedLogisticsContactLastLogins { get; }
        IDbSet<ContactLoginLog> ContactLoginLogs { get; }
        IDbSet<Report> Reports { get; }
        IDbSet<UserPermittedBranch> UserPermittedBranches { get; }
        IDbSet<SmallDocument> SmallDocuments { get; }
        IDbSet<CommunicationLogStep> CommunicationLogSteps { get; }
        IDbSet<ReportGroup> ReportGroups { get; }
        IDbSet<LeadSource> LeadSources { get; }
        IDbSet<Industry> Industries { get; }
        IDbSet<ProductType> ProductTypes { get; set; }
        IDbSet<ProductPeriod> ProductPeriods { get; set; }
        IDbSet<CustomerProduct> CustomerProducts { get; set; }
        IDbSet<CustomerProductActualData> CustomerProductActualDatas { get; set; }
        IDbSet<CustomerProductLocation> CustomerProductLocations { get; set; }
        IDbSet<CustomerProductLocationActualData> CustomerProductLocationActualDatas { get; set; }
        IDbSet<Competitor> Competitors { get; set; }
        IDbSet<CustomerCompetitor> CustomerCompetitors { get; set; }
        IDbSet<CustomerCompetitorProduct> CustomerCompetitorProducts { get; set; }
        IDbSet<CustomerAdditionalService> CustomerAdditionalServices { get; set; }
        IDbSet<Commodity> Commodities { get; }
        IDbSet<ContactDoneMethod> ContactDoneMethods { get; }
        IDbSet<AdditionalService> AdditionalServices { get; set; }
        IDbSet<VatMandatoryType> VatMandatoryTypes { get; set; }
        IDbSet<VatUniqueType> VatUniqueTypes { get; set; }
        IDbSet<CustomerSalesNote> CustomerSalesNotes { get; set; }
        IDbSet<AuthenticationToken> AuthenticationTokens { get; set; }
        IDbSet<CustomerStatus> CustomerStatus { get; set; }
        IDbSet<ProductTypeModification> ProductTypeModifications { get; set; }
        IDbSet<CustomerSalesmanByProduct> CustomerSalesmanByProducts { get; set; }
        IDbSet<CustomerAccountManagerByProduct> CustomerAccountManagerByProducts { get; set; }
        IDbSet<CustomerFreelancerByProduct> CustomerFreelancerByProducts { get; set; }
        IDbSet<CustomerCustomsAgentByProduct> CustomerCustomsAgentByProducts { get; set; }
        IDbSet<CustomerForwarderByProduct> CustomerForwarderByProducts { get; set; }
        IDbSet<CustomerMediatorByProduct> CustomerMediatorByProducts { get; set; }
        IDbSet<CardExternalCodeByCurrency> CardExternalCodeByCurrencies { get; set; }
        IDbSet<BusinessUnit> BusinessUnits { get; set; }
        IDbSet<FeatureAccessLevel> FeatureAccessLevels { get; set; }
        IDbSet<EmailProvider> EmailProviders { get; }
        IDbSet<Region> Regions { get; set; }
        IDbSet<CustomerSize> CustomerSizes { get; set; }
        IDbSet<CountryCity> CountryCities { get; set; }
        IDbSet<ReportModification> ReportModifications { get; set; }
        IDbSet<ContactsUnseenEntitie> ContactsUnseenEntities { get; set; }
        IDbSet<HybridTenantState> HybridTenantStates { get; set; }
        IDbSet<HybridTenantThreshold> HybridTenantThresholds { get; set; }
        IDbSet<CustomMetaDataTypesAddtional> CustomMetaDataTypesAddtionals { get; set; }
        IDbSet<SharedFollowedShipment> SharedFollowedShipments { get; set; }
        IDbSet<ColorIndex> ColorIndexs { get; set; }
        IDbSet<DataProvider> DataProviders { get; set; }
        IDbSet<UserPermittedProduct> UserPermittedProducts { get; set; }
        IDbSet<Distributor> Distributors { get; set; }
        IDbSet<DocumentsFiling> DocumentsFilings { get; set; }
        IDbSet<DocumentStatus> DocumentStatus { get; set; }
        IDbSet<DocumentsDataProvider> DocumentsDataProviders { get; set; }
        IDbSet<DocumentsFilingMetaDataValue> DocumentsFilingMetaDataValues { get; set; }
        IDbSet<DocumentTypeMetaData> DocumentTypeMetaDatas { get; set; }
        IDbSet<DocumentsMetaDataType> DocumentsMetaDataTypes { get; set; }
        IDbSet<ComputingPartner> ComputingPartners { get; set; }
        IDbSet<ComputingPartnerCode> ComputingPartnerCodes { get; set; }
        IDbSet<ComputingPartnerTable> ComputingPartnerTables { get; set; }
        IDbSet<ComputingPartnerTranslation> ComputingPartnerTranslations { get; set; }
        IDbSet<DocumentFolder> DocumentFolders { get; set; }
        IDbSet<DocumentTypeCategory> DocumentTypeCategories { get; set; }
        IDbSet<CustomerTenantAccessStatusType> CustomerTenantAccessStatusTypes { get; set; }
        IDbSet<CustomerTenantAccess> CustomerTenantAccesses { get; set; }
        IDbSet<CustomerTenantAccessCard> CustomerTenantAccessCards { get; set; }
        IDbSet<CustomerTenantAccessRequest> CustomerTenantAccessRequests { get; set; }
        IDbSet<HybridPartner> HybridPartners { get; set; }
        IDbSet<CustomerTenantAccessCardsBatch> CustomerTenantAccessCardsBatches { get; set; }
        IDbSet<Participant> Participants { get; }
        IDbSet<AirlineStatistics> AirlineStatistics { get; }
        IDbSet<AWBDescriptionOfGoods> AWBDescriptionOfGoods { get; }
        IDbSet<VatFormatType> VatFormatTypes { get; set; }
        IDbSet<LogitudeMessagesTransmissionLog> LogitudeMessagesTransmissionLogs { get; set; }
        IDbSet<FeaturePackageType> FeaturePackageTypes { get; }
        IDbSet<PackageConnectedPackage> PackageConnectedPackages { get; }
        IDbSet<UserLicense> UserLicenses { get; }
        IDbSet<EntityChange> EntityChanges { get; set; }
        IDbSet<Automation> Automations { get; set; }
        IDbSet<AutomationResultEmailRecipient> AutomationResultEmailRecipients { get; set; }
        IDbSet<AutomationHistory> AutomationHistorys { get; set; }
        IDbSet<AutomationLastUpdate> AutomationLastUpdates { get; set; }
        IDbSet<EntityCasualData> EntityCasualDatas { get; set; }
        IDbSet<AirlineMessagingRule> AirlineMessagingRules { get; set; }
        IDbSet<PaymentTermDateType> PaymentTermDateTypes { get; set; }
        IDbSet<CustomerFieldsUpdateSetting> CustomerFieldsUpdateSettings { get; set; }
        IDbSet<BlobFile> BlobFiles { get; set; }
        IDbSet<CreditLimitSetting> CreditLimitSettings { get; set; }
        IDbSet<AgentSharedManifest> AgentSharedManifests { get; set; }
        IDbSet<SharedManifestTranslation> SharedManifestTranslations { get; set; }
        IDbSet<SharedManifestsStatus> SharedManifestsStatuses { get; set; }
        IDbSet<TenantAdditionalData> TenantAdditionalDatas { get; set; }
        IDbSet<CardExternalAccountsByProduct> CardExternalAccountsByProducts { get; set; }
        IDbSet<CustomsInterface> CustomsInterfaces { get; set; }
        IDbSet<CustomsInterfaceSetting> CustomsInterfaceSettings { get; set; }
        IDbSet<FTPDetail> FTPDetails { get; set; }
        IDbSet<VATTypesGroup> VATTypesGroups { get; set; }
        IDbSet<TwoFactorAuthenticationDevice> TwoFactorAuthenticationDevices { get; set; }
        IDbSet<TenantLoginPolicy> TenantLoginPolicies { get; set; }
        IDbSet<LoginPolicy> LoginPolicies { get; set; }
        IDbSet<AgentSharedDocument> AgentSharedDocuments { get; set; }
        IDbSet<MetodoPago> MetodoPagos { get; }
        IDbSet<ChargesExternalAccountsByProduct> ChargesExternalAccountsByProducts { get; set; }
        IDbSet<RegistryDateType> RegistryDateTypes { get; set; }
        IDbSet<WarehouseType> WarehouseTypes { get; }
        IDbSet<NumberFormat> NumberFormats { get; }
        IDbSet<UsoCFDI> UsoCFDIs { get; }
        IDbSet<ReportsTemplate> ReportsTemplates { get; set; }
        IDbSet<ReportsTemplatesVersion> ReportsTemplatesVersions { get; set; }
        IDbSet<FeatureChange> FeatureChanges { get; set; }
        IDbSet<FilingInbox> FilingInboxes { get; set; }
        IDbSet<FilingInboxAttachment> FilingInboxAttachments { get; set; }
        IDbSet<FilingInboxAttachmentLog> FilingInboxAttachmentLogs { get; set; }
        IDbSet<ReportExecutionLog> ReportExecutionLogs { get; set; }
        IDbSet<INTTRASetting> INTTRASettings { get; set; }
        IDbSet<INTTRASettingMode> INTTRASettingModes { get; set; }
        IDbSet<INTTRABranchRegisteredCarrier> INTTRABranchRegisteredCarriers { get; set; }
        IDbSet<DocumentFilingBackupBatch> DocumentFilingBackupBatches { get; set; }
        IDbSet<DocumentFilingBackupSetting> DocumentFilingBackupSettings { get; set; }
        IDbSet<HybridPartnersPermission> HybridPartnersPermissions { get; set; }
        IDbSet<DWHSetting> DWHSettings { get; set; }
        IDbSet<PaymentGatewayPartner> PaymentGatewayPartners { get; set; }
        IDbSet<TemperatureUnit> TemperatureUnits { get; set; }
        IDbSet<CustomerDeposition> CustomerDepositions { get; set; }
        IDbSet<CustomsShipper> CustomsShippers { get; }
        IDbSet<UsersReleaseNotesDisplay> UsersReleaseNotesDisplays { get; set;}
        IDbSet<CheckDigitControlAlgorithm> CheckDigitControlAlgorithms { get; set; }
        IDbSet<CardContactProduct> CardContactProducts { get; set; }
        IDbSet<LogBoxTenantSetting> LogBoxTenantSettings { get; set; }
        IDbSet<DWHBuildStatus> DWHBuildStatus { get; set; }
        IDbSet<AccountingPartner> AccountingPartners { get; set; }
        IDbSet<DocumentsExecutionLog> DocumentsExecutionLogs { get; set; }
        IDbSet<CardContactAdditionalService> CardContactAdditionalServices { get; set; }
        IDbSet<UserLastSettings> UserLastSettings { get; set; }

        void SetAsModified(object entity);
        void DetectChanges();
        int SaveChanges();
    }
}