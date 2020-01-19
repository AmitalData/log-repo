

using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Mapping;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Mapping;
using Simplog.Data.InvoiceModel.Mapping;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Mapping;
using Simplog.Data.ShipmentModel.Mapping;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using Simplog.Data.ShipmentsModel.Mapping;
using Logitude.OracleDatabaseMigration.Migrations;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.Social.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityMapping;
using Logitude.Social.Data.EntityMapping;
using Logitude.CRM.Data.EntityMapping;
using Logitude.BookingLib.Data.EntityMapping;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityMapping;
using Devart.Data.Oracle.Entity.Migrations;
using Devart.Data.Oracle;
using System.Diagnostics;
using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.WarehouseLib.Data.EntityMapping;
using Logitude.TimeManagement.Data.EntityMapping;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityMapping;
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.Data.EntityMapping;

namespace Logitude.OracleDatabaseMigration.LogitudeModel
{
    public class LogitudeMigrationContext : DbContext
    {

        public LogitudeMigrationContext()
            : base("OracleMainMigration")
        //:this( GetConn())
        {
            Debug.WriteLine(this.Database.Connection.ConnectionString);
            Database.SetInitializer<LogitudeMigrationContext>(new MigrateDatabaseToLatestVersion<LogitudeMigrationContext, Configuration>());

        }

        private static DbConnection GetConn()
        {
            return new OracleConnection(System.Configuration.ConfigurationManager.ConnectionStrings["OracleMainMigration"].ConnectionString);
        }

        public LogitudeMigrationContext(DbConnection connection)
            : base(connection, true)
        {
            //Database.SetInitializer<LogitudeMainContext>(new MigrateDatabaseToLatestVersion<LogitudeMainContext, Configuration>());
            try
            {
                bool exists = Database.CreateIfNotExists();
            }

            catch (Exception ex) { }
            try
            {
                //var configuration = new Configuration();
                //configuration.AutomaticMigrationDataLossAllowed = true;
                //configuration.TargetDatabase = new DbConnectionInfo(connection.ConnectionString, "System.Data.SqlClient");
                //var migrator = new DbMigrator(configuration);
                //migrator.Update();
            }
            catch (Exception eee) { }
        }

        #region Common Context
        public IDbSet<CustomsShipper> CustomsShippers { get; set; }
        public IDbSet<SharedUserQuery> SharedUserQueries { get; set; }
        public IDbSet<DocumentFilingBackupSetting> DocumentFilingBackupSettings { get; set; }
        public IDbSet<MetodoPago> MetodoPagos { get; set; }
        public IDbSet<BlobFile> BlobFiles
        {
            get;
            set;
        }
        public IDbSet<CustomerFieldsUpdateSetting> CustomerFieldsUpdateSettings
        {
            get;
            set;
        }
        public IDbSet<DocumentFolder> DocumentFolders
        {
            get;
            set;
        }
        public IDbSet<DocumentsFiling> DocumentsFilings
        {
            get;
            set;
        }
        public IDbSet<Distributor> Distributors
        {
            get;
            set;
        }
        public IDbSet<EmailProvider> EmailProviders { get; set; }
        public IDbSet<AuthenticationToken> AuthenticationTokens { get; set; }
        public IDbSet<VatUniqueType> VatUniqueTypes { get; set; }
        public IDbSet<VatMandatoryType> VatMandatoryTypes { get; set; }
        public IDbSet<CustomerSalesNote> CustomerSalesNotes { get; set; }
        public IDbSet<BusinessUnit> BusinessUnits { get; set; }
        public IDbSet<FeatureAccessLevel> FeatureAccessLevels { get; set; }
        public IDbSet<AddressType> AddressTypes
        {
            get;
            set;
        }
        public IDbSet<ColorIndex> ColorIndexs
        {
            get;
            set;
        }
        public IDbSet<DataProvider> DataProviders
        {
            get;
            set;
        }
        public IDbSet<Country> Countries
        {
            get;
            set;
        }
        public IDbSet<State> States
        {
            get;
            set;
        }
        public IDbSet<GlobalZone> GlobalZones
        {
            get;
            set;
        }
        public IDbSet<Address> Addresses
        {
            get;
            set;
        }
        public IDbSet<Port> Ports
        {
            get;
            set;
        }
        public IDbSet<Card> Cards
        {
            get;
            set;
        }
        public IDbSet<Customer> Customers
        {
            get;
            set;
        }
        public IDbSet<Agent> Agents
        {
            get;
            set;
        }
        public IDbSet<CustomAgent> CustomAgents
        {
            get;
            set;
        }
        public IDbSet<ShippingAgent> ShippingAgents
        {
            get;
            set;
        }
        public IDbSet<Contact> Contacts
        {
            get;
            set;
        }
        public IDbSet<CardContact> CardContacts
        {
            get;
            set;
        }
        public IDbSet<PartnerType> PartnerTypes
        {
            get;
            set;
        }
        public IDbSet<Rank> Ranks
        {
            get;
            set;
        }
        public IDbSet<User> Users
        {
            get;
            set;
        }
        public IDbSet<Department> Departments
        {
            get;
            set;
        }
        public IDbSet<Branch> Branches
        {
            get;
            set;
        }
        public IDbSet<Airline> Airlines
        {
            get;
            set;
        }
        public IDbSet<ShippingLine> ShippingLines
        {
            get;
            set;
        }
        public IDbSet<Trucker> Truckers
        {
            get;
            set;
        }
        public IDbSet<Tenant> Tenants
        {
            get;
            set;
        }
        public IDbSet<TenantAdditionalData> TenantAdditionalDatas
        {
            get;
            set;
        }
        public IDbSet<Currency> Currencies
        {
            get;
            set;
        }
        public IDbSet<ContactTenant> ContactTenants
        {
            get;
            set;
        }
        public IDbSet<ContactTenantRole> ContactTenantRoles
        {
            get;
            set;
        }
        public IDbSet<Role> Roles
        {
            get;
            set;
        }
        public IDbSet<PaymentTerm> PaymentTerms
        {
            get;
            set;
        }
        public IDbSet<VatType> VatTypes
        {
            get;
            set;
        }
        public IDbSet<Incoterm> Incoterms
        {
            get;
            set;
        }
        public IDbSet<ChargesType> ChargesTypes
        {
            get;
            set;
        }
        public IDbSet<Measurement> Measurements
        {
            get;
            set;
        }
        public IDbSet<Document> Documents
        {
            get;
            set;
        }
        public IDbSet<DocumentType> DocumentTypes
        {
            get;
            set;
        }
        public IDbSet<DocumentsDataProvider> DocumentsDataProviders
        {
            get;
            set;
        }
        public IDbSet<DocumentOut> DocumentOuts
        {
            get;
            set;
        }
        public IDbSet<PackageType> PackageTypes
        {
            get;
            set;
        }
        public IDbSet<DocumentTypeCustomField> DocumentTypeCustomFields
        {
            get;
            set;
        }
        public IDbSet<FormCustomField> FormCustomFields
        {
            get;
            set;
        }
        public IDbSet<FieldDataType> FieldDataTypes
        {
            get;
            set;
        }
        public IDbSet<WeightUnit> WeightUnits
        {
            get;
            set;
        }
        public IDbSet<DimensionsUnit> DimensionsUnits
        {
            get;
            set;
        }
        public IDbSet<RateClass> RateClasses
        {
            get;
            set;
        }
        public IDbSet<CommunicationLog> CommunicationLogs
        {
            get;
            set;
        }
        public IDbSet<CommunicationAttachment> CommunicationAttachments
        {
            get;
            set;
        }
        public IDbSet<DueType> DueTypes
        {
            get;
            set;
        }
        public IDbSet<CommunicationStatusType> CommunicationStatusTypes
        {
            get;
            set;
        }
        public IDbSet<CommunicationLogType> CommunicationLogTypes
        {
            get;
            set;
        }
        public IDbSet<WarehouseType> WarehouseTypes
        {
            get;
            set;
        }

        public IDbSet<NumberFormat> NumberFormats
        {
            get;
            set;
        }
        public IDbSet<DocumentTypeTemplate> DocumentTypeTemplates
        {
            get;
            set;
        }
        public IDbSet<TemplateFormat> TemplateFormats
        {
            get;
            set;
        }
        //public IDbSet<DWHSetting> DWHSettings { get; set; }
        public IDbSet<Warehouse> Warehouses
        {
            get;
            set;
        }
        public IDbSet<Vessel> Vessels
        {
            get;
            set;
        }
        public IDbSet<MAWBStack> MAWBStacks
        {
            get;
            set;
        }
        public IDbSet<DocumentTypeCopy> DocumentTypeCopies
        {
            get;
            set;
        }
        public IDbSet<DocumentOutCopy> DocumentOutCopies
        {
            get;
            set;
        }
        public IDbSet<Feature> Features
        {
            get;
            set;
        }
        public IDbSet<RoleFeature> RoleFeatures
        {
            get;
            set;
        }
        public IDbSet<Restriction> Restrictions
        {
            get;
            set;
        }
        public IDbSet<VatTypePercentage> VatTypePercentages
        {
            get;
            set;
        }
        public IDbSet<TarrifHeader> TarrifHeaders
        {
            get;
            set;
        }
        public IDbSet<TarrifType> TarrifTypes
        {
            get;
            set;
        }
        public IDbSet<TariffSetting> TariffSettings
        {
            get; set;

        }

        public IDbSet<TarrifCharge> TarrifCharges
        {
            get;
            set;
        }
        public IDbSet<TarrifFromTo> TarrifFromToes
        {
            get;
            set;
        }
        public IDbSet<TarrifFromToType> TarrifFromToTypes
        {
            get;
            set;
        }
        public IDbSet<TarrifStep> TarrifSteps
        {
            get;
            set;
        }
        public IDbSet<UserLoginLog> UserLoginLogs
        {
            get;
            set;
        }
        public IDbSet<PasswordPolicy> PasswordPolicies
        {
            get;
            set;
        }
        public IDbSet<Vendor> Vendors
        {
            get;
            set;
        }
        public IDbSet<AccountingSetting> AccountingSettings
        {
            get;
            set;
        }
        public IDbSet<AccountingSystem> AccountingSystems
        {
            get;
            set;
        }
        public IDbSet<FeatureType> FeatureTypes
        {
            get;
            set;
        }
        public IDbSet<RoleType> RoleTypes
        {
            get;
            set;
        }
        public IDbSet<Package> Packages
        {
            get;
            set;
        }
        public IDbSet<PackageFeature> PackageFeatures
        {
            get;
            set;
        }
        public IDbSet<UserLastLogin> UserLastLogins
        {
            get;
            set;
        }
        public IDbSet<TermsofUse> TermsofUses
        {
            get;
            set;
        }
        public IDbSet<TermsofUseSignature> TermsofUseSignatures
        {
            get;
            set;
        }
        public IDbSet<ChargeTypeAccounting> ChargeTypeAccountings { get; set; }
        public IDbSet<Report> Reports { get; set; }
        public IDbSet<LeadSource> LeadSources
        {
            get;
            set;
        }
        public IDbSet<Industry> Industries
        {
            get;
            set;
        }
        public IDbSet<EntityDate> EntityDates
        {
            get;
            set;
        }
        public IDbSet<ContactLastLogin> ContactLastLogins
        {
            get;
            set;
        }
        public IDbSet<ContactLoginLog> ContactLoginLogs
        {
            get;
            set;
        }
        public IDbSet<SmallDocument> SmallDocuments
        {
            get;
            set;
        }
        public IDbSet<CommunicationLogStep> CommunicationLogSteps
        {
            get;
            set;
        }
        public IDbSet<UserPermittedBranch> UserPermittedBranches
        {
            get;
            set;
        }
        public IDbSet<ReportGroup> ReportGroups
        {
            get;
            set;
        }
        public IDbSet<ProductPeriod> ProductPeriods
        {
            get;
            set;
        }
        public IDbSet<ProductType> ProductTypes
        {
            get;
            set;
        }
        public IDbSet<CustomerProduct> CustomerProducts
        {
            get;
            set;
        }
        public IDbSet<CustomerProductActualData> CustomerProductActualDatas
        {
            get;
            set;
        }
        public IDbSet<CustomerProductLocation> CustomerProductLocations
        {
            get;
            set;
        }
        public IDbSet<CustomerProductLocationActualData> CustomerProductLocationActualDatas
        {
            get;
            set;
        }
        public IDbSet<Competitor> Competitors
        {
            get;
            set;
        }
        public IDbSet<CustomerCompetitor> CustomerCompetitors
        {
            get;
            set;
        }
        public IDbSet<CustomerCompetitorProduct> CustomerCompetitorProducts
        {
            get;
            set;
        }
        public IDbSet<HybridTenantState> HybridTenantStates { get; set; }
        public IDbSet<HybridTenantThreshold> HybridTenantThresholds { get; set; }
        public IDbSet<CustomMetaDataTypesAddtional> CustomMetaDataTypesAddtionals { get; set; }
        public IDbSet<CustomerAdditionalService> CustomerAdditionalServices { get; set; }
        public IDbSet<Commodity> Commodities { get; set; }
        public IDbSet<ContactDoneMethod> ContactDoneMethods { get; set; }
        public IDbSet<AdditionalService> AdditionalServices { get; set; }
        public IDbSet<CustomerStatus> CustomerStatus { get; set; }
        public IDbSet<ProductTypeModification> ProductTypeModifications { get; set; }
        public IDbSet<CustomerSalesmanByProduct> CustomerSalesmanByProducts { get; set; }
        public IDbSet<CustomerAccountManagerByProduct> CustomerAccountManagerByProducts { get; set; }
        public IDbSet<CustomerFreelancerByProduct> CustomerFreelancerByProducts { get; set; }
        public IDbSet<CustomerCustomsAgentByProduct> CustomerCustomsAgentByProducts
        {
            get;
            set;
        }
        public IDbSet<CustomerForwarderByProduct> CustomerForwarderByProducts { get; set; }
        public IDbSet<CustomerMediatorByProduct> CustomerMediatorByProducts { get; set; }
        public IDbSet<SpecialServicesType> SpecialServicesTypes { get; set; }
        public IDbSet<Region> Regions { get; set; }
        public IDbSet<CountryCity> CountryCities
        {
            get;
            set;
        }
        public IDbSet<ContactsUnseenEntitie> ContactsUnseenEntities
        {
            get;
            set;
        }
        public IDbSet<SharedFollowedShipment> SharedFollowedShipments
        {
            get;
            set;
        }
        public IDbSet<ReportModification> ReportModifications
        {
            get;
            set;
        }
        public IDbSet<DocumentsFilingMetaDataValue> DocumentsFilingMetaDataValues
        {
            get;
            set;
        }
        public IDbSet<DocumentTypeMetaData> DocumentTypeMetaDatas
        {
            get;
            set;
        }
        public IDbSet<DocumentsMetaDataType> DocumentsMetaDataTypes
        {
            get;
            set;
        }
        public IDbSet<ComputingPartner> ComputingPartners { get; set; }
        public IDbSet<ComputingPartnerCode> ComputingPartnerCodes { get; set; }
        public IDbSet<ComputingPartnerTable> ComputingPartnerTables { get; set; }
        public IDbSet<ComputingPartnerTranslation> ComputingPartnerTranslations { get; set; }
        public IDbSet<DocumentTypeCategory> DocumentTypeCategories
        {
            get;
            set;
        }
        public IDbSet<CustomerTenantAccessStatusType> CustomerTenantAccessStatusTypes
        {
            get;
            set;
        }
        public IDbSet<CustomerTenantAccess> CustomerTenantAccesses
        {
            get;
            set;
        }
        public IDbSet<CustomerTenantAccessRequest> CustomerTenantAccessRequests
        {
            get;
            set;
        }
        public IDbSet<CustomerTenantAccessCard> CustomerTenantAccessds
        {
            get;
            set;
        }
        public IDbSet<HybridPartner> HybridPartners
        {
            get;
            set;
        }
        public IDbSet<CustomerTenantAccessCardsBatch> CustomerTenantAccessCardsBatches
        {
            get;
            set;
        }
        public IDbSet<Participant> Participants
        {
            get;
            set;
        }
        public IDbSet<AirlineStatistics> AirlineStatistics
        {
            get;
            set;
        }
        public IDbSet<LogitudeMessagesTransmissionLog> LogitudeMessagesTransmissionLogs
        {
            get;
            set;
        }
        public IDbSet<ContinuousMessagesTypeCode> ContinuousMessagesTypeCodes
        {
            get;
            set;
        }

        public IDbSet<FreightPaymentMethod> FreightPaymentMethods
        {
            get;
            set;
        }

        //public IDbSet<CourierStatus> CourierStatuses
        //{
        //    get;
        //    set;
        //}

        public IDbSet<DeclarationCourierStatus> DeclarationCourierStatuses
        {
            get;
            set;
        }

        public IDbSet<CourierPendingReason> CourierPendingReasons
        {
            get;
            set;
        }

        public IDbSet<ImporterDeclarationType> ImporterDeclarationTypes
        {
            get;
            set;
        }

        public IDbSet<CommercialSale> CommercialSales
        {
            get;
            set;
        }
        public IDbSet<ClaimExplanationCode> ClaimExplanationCodes
        {
            get;
            set;
        }
        public IDbSet<ClaimImporterDeclarsPage3> ClaimImporterDeclarsPage3s
        {
            get;
            set;
        }
        public IDbSet<ClaimImporterDeclarsPage3A> ClaimImporterDeclarsPage3As
        {
            get;
            set;
        }
        public IDbSet<EntityChange> EntityChanges
        {
            get;
            set;
        }
        public IDbSet<AgentSharedManifest> AgentSharedManifests
        {
            get;
            set;
        }
        public IDbSet<SharedManifestsStatus> SharedManifestsStatuses { get; set; }
        public IDbSet<AutomationResultEmailRecipient> AutomationResultEmailRecipients
        {
            get;
            set;
        }
        public IDbSet<Automation> Automations
        {
            get;
            set;
        }
        public IDbSet<AutomationHistory> AutomationHistorys
        {
            get;
            set;
        }
        public IDbSet<AutomationLastUpdate> AutomationLastUpdates
        {
            get;
            set;
        }
        public IDbSet<ClaimImporterDeclarsPage3B> ClaimImporterDeclarsPage3Bs
        {
            get;
            set;

        }
        public IDbSet<Claim> Claims
        {
            get;
            set;
        }
        public IDbSet<ClaimEntity> ClaimEntities
        {
            get;
            set;
        }
        public IDbSet<ClaimsRelatedEntity> ClaimsRelatedEntities
        {
            get;
            set;
        }
        public IDbSet<CourtInstance> CourtInstances
        {
            get;
            set;
        }
        public IDbSet<ClaimsRelatedEntitiesAmount> ClaimsRelatedEntitiesAmounts
        {
            get;
            set;
        }
        public IDbSet<ClaimsRelatedEntitiesReason> ClaimsRelatedEntitiesReasons
        {
            get;
            set;
        }
        public IDbSet<ClaimsRelatedEntsReasonsExp> ClaimsRelatedEntsReasonsExps
        {
            get;
            set;
        }
        public IDbSet<ClaimsRelatedEntsExpDeclar> ClaimsRelatedEntsExpDeclars
        {
            get;
            set;
        }
        public IDbSet<ClaimImporterDeclarsP3Loi> ClaimImporterDeclarsP3LoiS
        {
            get;
            set;
        }

        public IDbSet<FeaturePackageType> FeaturePackageTypes { get; set; }
        public IDbSet<PackageConnectedPackage> PackageConnectedPackages { get; set; }
        public IDbSet<UserLicense> UserLicenses { get; set; }
        public IDbSet<EntityCasualData> EntityCasualDatas { get; set; }
        public IDbSet<AirlineMessagingRule> AirlineMessagingRules { get; set; }
        public IDbSet<PaymentTermDateType> PaymentTermDateTypes
        {
            get;
            set;
        }
        public IDbSet<CreditLimitSetting> CreditLimitSetting { get; set; }
        public IDbSet<CardExternalAccountsByProduct> CardExternalAccountsByProducts { get; set; }
        public IDbSet<CustomsInterface> CustomsInterfaces { get; set; }
        public IDbSet<CustomsInterfaceSetting> CustomsInterfaceSettings { get; set; }
        public IDbSet<VATTypesGroup> VATTypesGroups { get; set; }
        public IDbSet<TwoFactorAuthenticationDevice> TwoFactorAuthenticationDevices { get; set; }
        public IDbSet<TenantLoginPolicy> TenantLoginPolicies { get; set; }
        public IDbSet<LoginPolicy> LoginPolicies { get; set; }
        public IDbSet<AgentSharedDocument> AgentSharedDocuments
        {
            get;
            set;
        }
        public IDbSet<ChargesExternalAccountsByProduct> ChargesExternalAccountsByProducts { get; set; }
        public IDbSet<RegistryDateType> RegistryDateTypes { get; set; }
        public IDbSet<ReportsTemplate> ReportsTemplates { get; set; }
        public IDbSet<ReportsTemplatesVersion> ReportsTemplatesVersions { get; set; }
        public IDbSet<FeatureChange> FeatureChanges { get; set; }
        public IDbSet<FilingInbox> FilingInboxes { get; set; }
        public IDbSet<FilingInboxAttachment> FilingInboxAttachments { get; set; }
        public IDbSet<FilingInboxAttachmentLog> FilingInboxAttachmentLogs { get; set; }
        public IDbSet<ReportExecutionLog> ReportExecutionLogs { get; set; }
        public IDbSet<DocumentTypeCustomsData> DocumentTypeCustomsData { get; set; }
        public IDbSet<CustomsDocumentsDefinition> CustomsDocumentsDefinition { get; set; }
        public IDbSet<UIMessage> UIMessage { get; set; }
        public IDbSet<UIMessageAdditional> UIMessageAdditional { get; set; }
        public IDbSet<PointerLevel> PointerLevel { get; set; }
        public IDbSet<ClientDrivingLicense> ClientDrivingLicense { get; set; }
        public IDbSet<ClientDrivingLicenseType> ClientDrivingLicenseType { get; set; }
        public IDbSet<PendingErrorPlace> PendingErrorPlaces { get; set; }
        public IDbSet<DecisionType> DecisionType { get; set; }
        public IDbSet<SeizureMethodType> SeizureMethodType { get; set; }
        public IDbSet<ClaimsRelatedEntitiesSeizure> ClaimsRelatedEntitiesSeizure { get; set; }
        public IDbSet<ClaimsRelatedEntitiesRefund> ClaimsRelatedEntitiesRefund { get; set; }
        public IDbSet<SeizureFactorType> SeizureFactorType { get; set; }
        public IDbSet<MamanSpecialAction> MamanSpecialAction { get; set; }
        public IDbSet<MamanSpecialActionStatus> MamanSpecialActionStatus { get; set; }
        public IDbSet<DeclarationMamanSpecialAction> PendingDeclarationMamanSpecialActionErrorPlace { get; set; }
        public IDbSet<RefundCustomerActivityType> RefundCustomerActivityType { get; set; }
        public IDbSet<TransferCargoMethodType> TransferCargoMethodType { get; set; }
        public IDbSet<GatepassReturnCode> GatepassReturnCode { get; set; }
        public IDbSet<UpdateCode> UpdateCode { get; set; }
        public IDbSet<GatepassRequest> GatepassRequest { get; set; }
        public IDbSet<PendingByKeyword> PendingByKeywords
        {
            get;
            set;
        }
        public IDbSet<ContinuousRequestType> ContinuousRequestType { get; set; }
        public IDbSet<RequestType> RequestType { get; set; }
        public IDbSet<ApprovedProfession> ApprovedProfession { get; set; }
        public IDbSet<DeficitDecision> DeficitDecision { get; set; }

        public IDbSet<SealCompletenes> SealCompletenes { get; set; }

        public IDbSet<SealType> SealType { get; set; }
        #endregion

        #region Webfreight Context


        public IDbSet<QueueMessage> QueueMessages
        {
            get;
            set;
        }

        public IDbSet<QueueMessageMoreDetails> QueueMessageMoreDetails
        {
            get;
            set;
        }

        public IDbSet<QueueDefinition> QueueDefinitions
        {
            get;
            set;
        }


        public IDbSet<ObjectTableLastUpdate> ObjectTableLastUpdates
        {
            get;
            set;
        }

        public IDbSet<PrepaidCollect> PrepaidCollects
        {
            get;
            set;
        }

        public IDbSet<CounterLastNumber> CounterLastNumbers
        {
            get;
            set;
        }

        public IDbSet<TransportMode> TransportModes
        {
            get;
            set;
        }

        public IDbSet<Direction> Directions
        {
            get;
            set;
        }

        public IDbSet<MoveType> MoveTypes
        {
            get;
            set;
        }

        public IDbSet<SpecialService> SpecialServices
        {
            get;
            set;
        }

        public IDbSet<FollowUp> FollowUps
        {
            get;
            set;
        }

        public IDbSet<ValidationType> ValidationTypes
        {
            get;
            set;
        }

        public IDbSet<Translation> Translations
        {
            get;
            set;
        }

        public IDbSet<TranslationHeader> TranslationHeaders
        {
            get;
            set;
        }


        public IDbSet<ActivityOwnerHistory> ActivityOwnerHistories
        {
            get;
            set;
        }


        public IDbSet<QuestionnaireAnswerLine> QuestionnaireAnswerLines
        {
            get;
            set;
        }

        public IDbSet<QuestionnaireAnswer> QuestionnaireAnswers
        {
            get;
            set;
        }

        public IDbSet<QuestionnaireQuestion> QuestionnaireQuestions
        {
            get;
            set;
        }

        public IDbSet<Questionnaire> Questionnaires
        {
            get;
            set;
        }

        public IDbSet<TextCode> TextCodes
        {
            get;
            set;
        }

        public IDbSet<ObjectTable> ObjectTables
        {
            get;
            set;
        }

        public IDbSet<ObjectField> ObjectFields
        {
            get;
            set;
        }

        public IDbSet<Screen> Screens
        {
            get;
            set;
        }

        public IDbSet<ScreenField> ScreenFields
        {
            get;
            set;
        }

        public IDbSet<TextCodeType> TextCodeTypes
        {
            get;
            set;
        }

        public IDbSet<RatesTable> RatesTable
        {
            get;
            set;
        }

        public IDbSet<EventType> EventType
        {
            get;
            set;
        }

        public IDbSet<TraceEvent> TraceEvent
        {
            get;
            set;
        }

        public IDbSet<Query> Queries
        {
            get;
            set;
        }

        public IDbSet<AdvancedQueryFilter> AdvancedQueryFilters
        {
            get;
            set;
        }

        public IDbSet<QueryColumn> QueryColumns
        {
            get;
            set;
        }

        public IDbSet<DBIdCounter> DBIdCounters
        {
            get;
            set;
        }

        public IDbSet<MenusTable> MenusTables
        {
            get;
            set;
        }

        public IDbSet<MenuType> MenusTypes
        {
            get;
            set;
        }

        public IDbSet<CategoryType> CategoryTypes
        {
            get;
            set;
        }

        public IDbSet<CustomTable> CustomTables
        {
            get;
            set;
        }

        public IDbSet<ObjectTableTab> ObjectTableTabs
        {
            get;
            set;
        }

        public IDbSet<ObjectTableHelperControl> ObjectTableHelperControls
        {
            get;
            set;
        }
        public IDbSet<GeneralLock> GeneralLocks
        {
            get;
            set;
        }
        public IDbSet<IATACode> IATACodes
        {
            get;
            set;
        }

        public IDbSet<ChargesGroup> ChargesGroups
        {
            get;
            set;
        }

        public IDbSet<VolumeUnit> VolumeUnits
        {
            get;
            set;
        }

        public IDbSet<EntityStatus> EntityStatus
        {
            get;
            set;
        }

        public IDbSet<DescriptionOfGoods> DescriptionOfGoods
        {
            get;
            set;
        }

        public IDbSet<MenuButton> MenuButtons
        {
            get;
            set;
        }

        public IDbSet<MenuButtonGroup> MenuButtonGroups
        {
            get;
            set;
        }

        public IDbSet<EntityLastActivity> EntityLastActivities
        {
            get;
            set;
        }

        public IDbSet<CounterDefinition> CounterDefinitions
        {
            get;
            set;
        }

        public IDbSet<CounterStat> CounterStats
        {
            get;
            set;
        }

        public IDbSet<ObjectFieldValidation> ObjectFieldValidations
        {
            get;
            set;
        }

        public IDbSet<RuleType> RuleTypes
        {
            get;
            set;
        }

        public IDbSet<ObjectTableRule> ObjectTableRules
        {
            get;
            set;
        }

        public IDbSet<ObjectTableRuleField> ObjectTableRuleFields
        {
            get;
            set;
        }

        public IDbSet<QueryGroup> QueryGroups
        {
            get;
            set;
        }

        public IDbSet<TriggerType> TriggerTypes
        {
            get;
            set;
        }

        public IDbSet<RuleNotificationType> RuleNotificationTypes
        {
            get;
            set;
        }

        public IDbSet<Counter> Counters
        {
            get;
            set;
        }

        public IDbSet<TenantSetting> TenantSettings
        {
            get;
            set;
        }

        public IDbSet<Tip> Tips
        {
            get;
            set;
        }

        public IDbSet<TipsVisibility> TipsVisibilities
        {
            get;
            set;
        }

        public IDbSet<ObjectFieldModification> ObjectFieldModifications
        {
            get;
            set;
        }

        public IDbSet<ScreenModification> ScreenModifications
        {
            get;
            set;
        }

        public IDbSet<ImageDetail> ImageDetails
        {
            get;
            set;
        }


        public IDbSet<ImageLibrary> ImageLibrarys
        {
            get;
            set;
        }



        public IDbSet<PermissionType> PermissionTypes
        {
            get;
            set;
        }

        public IDbSet<ObjectTableType> ObjectTableTypes
        {
            get;
            set;
        }

        public IDbSet<RuleConditionField> RuleConditionFields
        {
            get;
            set;
        }

        public IDbSet<CustomPickList> CustomPickLists
        {
            get;
            set;
        }

        public IDbSet<SharedLogisticsUpdate> SharedLogisticsUpdates
        {
            get;
            set;
        }

        public IDbSet<SharedLogisticsUpdateStatus> SharedLogisticsUpdateStatus
        {
            get;
            set;
        }

        public IDbSet<EntityLastUpdate> EntityLastUpdates
        {
            get;
            set;
        }

        public IDbSet<EntityLastActivityType> EntityLastActivityTypes
        {
            get;
            set;
        }

        public IDbSet<SharedLogisticsInvitationStatus> SharedLogisticsInvitationStatus
        {
            get;
            set;
        }

        public IDbSet<EventTypeCategory> EventTypeCategories
        {
            get;
            set;
        }

        public IDbSet<InboundEmail> InboundEmails
        {
            get;
            set;
        }

        public IDbSet<InboundEmailLine> InboundEmailLines
        {
            get;
            set;
        }

        public IDbSet<BusinessHour> BusinessHours
        {
            get;
            set;
        }

        public IDbSet<BusinessHoursHoliday> BusinessHoursHolidays
        {
            get;
            set;
        }
        #endregion

        #region Shipment Context
        public IDbSet<Shipment> Shipments
        {
            get;
            set;
        }
        public IDbSet<ShipmentType> ShipmentTypes
        {
            get;
            set;
        }
        public IDbSet<ShipmentMasterData> ShipmentMasterDatas
        {
            get;
            set;
        }
        public IDbSet<EmailAlertSetting> EmailAlertSettings
        {
            get;
            set;
        }
        public IDbSet<ShipmentReceivable> ShipmentReceivables
        {
            get;
            set;
        }
        public IDbSet<ShipmentPickUpDelivery> ShipmentPickUpDeliveries
        {
            get;
            set;
        }
        public IDbSet<ShipmentPackage> ShipmentPackages
        {
            get;
            set;
        }
        public IDbSet<InsideShipmentPackage> InsideShipmentPackages
        {
            get;
            set;
        }
        public IDbSet<ShipmentPickUpDeliveryPackage> ShipmentPickUpDeliveryPackages
        {
            get;
            set;
        }
        public IDbSet<ShipmentOrderPackage> ShipmentOrderPackages
        {
            get;
            set;
        }
        public IDbSet<ShipmentPayableLineStatus> ShipmentPayableLineStatus
        {
            get;
            set;
        }
        public IDbSet<ShipmentPayable> ShipmentPayables
        {
            get;
            set;
        }
        public IDbSet<ShipmentReceivableLineStatus> ShipmentReceivableLineStatus
        {
            get;
            set;
        }
        public IDbSet<ShipmentReceivableStatus> ShipmentReceivableStatus
        {
            get;
            set;
        }
        public IDbSet<ShipmentPayableStatus> ShipmentPayableStatus
        {
            get;
            set;
        }
        public IDbSet<ShipmentCustomerType> ShipmentCustomerTypes
        {
            get;
            set;
        }
        public IDbSet<PickUpDeliveryType> PickUpDeliveryTypes
        {
            get;
            set;
        }
        public IDbSet<PickUpDeliveryFromToType> PickUpDeliveryFromToTypes
        {
            get;
            set;
        }
        public IDbSet<ShipmentAWBPrintOnly> ShipmentAWBPrintOnlies
        {
            get;
            set;
        }
        public IDbSet<NextLeg> NextLegs
        {
            get;
            set;
        }
        public IDbSet<ShipmentPayableAmountType> ShipmentPayableAmountTypes
        {
            get;
            set;
        }
        public IDbSet<ShipmentLevel> ShipmentLevels
        {
            get;
            set;
        }
        public IDbSet<AWBChargesCode> AWBChargeCodes
        {
            get;
            set;
        }
        public IDbSet<AWBSpecialHandlingCode> AWBHandlingCodes
        {
            get;
            set;
        }
        public IDbSet<FWBStatus> FWBStatus
        {
            get;
            set;
        }

        public IDbSet<CustomsTransmissionsStatus> CustomsTransmissionsStatus
        {
            get;
            set;
        }

        public IDbSet<FHLStatus> FHLStatus
        {
            get;
            set;
        }
        public IDbSet<AWBStatus> AWBStatus
        {
            get;
            set;
        }
        public IDbSet<ShipmentCarrierStatus> ShipmentCarrierStatuses
        {
            get;
            set;
        }
        public IDbSet<APILogs> APILogs
        {
            get;
            set;
        }
        public IDbSet<APILogsData> APILogsData
        {
            get;
            set;
        }
        public IDbSet<OceanInsightsRequest> OceanInsightsRequests
        {
            get;
            set;
        }
        public IDbSet<OceanInsightsRequestsCount> OceanInsightsRequestsCounts
        {
            get;
            set;
        }
        public IDbSet<OceanInsightsStatuses> OceanInsightsStatuses
        {
            get;
            set;
        }
        public IDbSet<TaskSchedulerHistory> TaskSchedulerHistories
        {
            get;
            set;
        }
        public IDbSet<TasksScheduler> TasksSchedulers
        {
            get;
            set;
        }
        public IDbSet<SchedulerLogs> SchedulerLogs
        {
            get;
            set;
        }
        public IDbSet<SchedulerProcedure> SchedulerProcedures
        {
            get;
            set;
        }

        public IDbSet<AWBOCI> AWBOCIs { get; set; }
        public IDbSet<AWBCustomsInformation> AWBCustomsInformations { get; set; }
        public IDbSet<AWBInformation> AWBInformations { get; set; }
        public IDbSet<ShipmentPackageItem> ShipmentPackageItems { get; set; }
        public IDbSet<ShipmentCommodity> ShipmentCommodities { get; set; }
        public IDbSet<MessagingStock> MessagingStocks { get; set; }
        public IDbSet<MessagingStockUsageHistory> MessagingStockUsageHistories { get; set; }
        public IDbSet<UserPermittedProduct> UserPermittedProducts { get; set; }
        public IDbSet<AccountingInformationIdentifier> AccountingInformationIdentifiers { get; set; }
        public IDbSet<ManifestStatus> ManifestStatus { get; set; }
        public IDbSet<AWBAdditionalHandlingInfo> AWBAdditionalHandlingInfos { get; set; }
        public IDbSet<OtherParticipantId> OtherParticipantIds { get; set; }
        public IDbSet<FBLStock> FBLStocks { get; set; }
        public IDbSet<OBLType> OBLTypes { get; set; }
        public IDbSet<ShipmentCustomsMessageType> ShipmentCustomsMessageTypes { get; set; }
        public IDbSet<ShipmentCustomsTransmission> ShipmentCustomsTransmissions { get; set; }


        #endregion

        #region Invoice Context
        public IDbSet<ARInvoice> ARInvoices
        {
            get;
            set;
        }

        public IDbSet<ARInvoiceLine> ARInvoiceLines
        {
            get;
            set;
        }

        public IDbSet<ARInvoiceType> ARInvoiceTypes
        {
            get;
            set;
        }

        public IDbSet<ARInvoiceStatus> ARInvoiceStatuses
        {
            get;
            set;
        }

        public IDbSet<ARInvoiceTotalVAT> ARInvoiceTotalVATs
        {
            get;
            set;
        }

        public IDbSet<ARInvoiceEntity> ARInvoiceEntities
        {
            get;
            set;
        }

        public IDbSet<Account> Accounts
        {
            get;
            set;
        }

        public IDbSet<AccountType> AccountTypes
        {
            get;
            set;
        }

        public IDbSet<ARPayment> ARPayments
        {
            get;
            set;
        }

        public IDbSet<AccountingPaymentMethod> PaymentMethods
        {
            get;
            set;
        }

        public IDbSet<ARPaymentStatus> ARPaymentStatus
        {
            get;
            set;
        }

        public IDbSet<ARInvoicePayment> ARInvoicePayments
        {
            get;
            set;
        }


        public IDbSet<APInvoice> APInvoices
        {
            get;
            set;
        }

        public IDbSet<APInvoiceLine> APInvoiceLines
        {
            get;
            set;
        }

        public IDbSet<APInvoiceStatus> APInvoiceStatus
        {
            get;
            set;
        }

        public IDbSet<APInvoiceTotalVAT> APInvoiceTotalVATs
        {
            get;
            set;
        }

        public IDbSet<APInvoiceType> APInvoiceTypes
        {
            get;
            set;
        }

        public IDbSet<APInvoiceEntity> APInvoiceEntities
        {
            get;
            set;
        }

        public IDbSet<APPayment> APPayments
        {
            get;
            set;
        }

        public IDbSet<APPaymentMethod> APPaymentMethods
        {
            get;
            set;
        }

        public IDbSet<APPaymentStatus> APPaymentStatus
        {
            get;
            set;
        }

        public IDbSet<APInvoicePayment> APInvoicePayments
        {
            get;
            set;
        }

        public IDbSet<CreditCardType> CreditCardTypes
        {
            get;
            set;
        }
        public IDbSet<SATPaymentMethod> SATPaymentMethods
        {
            get;
            set;
        }

        public IDbSet<BankAccountLite> BankAccountLites
        {
            get;
            set;
        }
        public IDbSet<AccountingTransferHeader> AccountingTransferHeaders { get; set; }
        public IDbSet<AccountingTransferLine> AccountingTransferLines { get; set; }
        public IDbSet<AccountingTransferType> AccountingTransferTypes { get; set; }
        public IDbSet<ARInvoiceTransferStatus> ARInvoiceTransferStatuses { get; set; }
        public IDbSet<APInvoiceTransferStatus> APInvoiceTransferStatuses { get; set; }
        public IDbSet<ARPaymentTransferStatus> ARPaymentTransferStatuses { get; set; }
        public IDbSet<ExternalSystemsTablesCode> ExternalSystemsTablesCodes { get; set; }
        public IDbSet<ExternalSystemsMissingTranslation> ExternalSystemsMissingTranslations { get; set; }
        public IDbSet<ExternalSystemsSyncStatus> ExternalSystemsSyncStatuses { get; set; }
        public IDbSet<AccountingSystemsSetting> AccountingSystemsSettings { get; set; }
        public IDbSet<AccountingSystemsSyncStatus> AccountingSystemsSyncStatuses { get; set; }
        public IDbSet<QuickbooksSyncRequestTicket> QuickbooksSyncRequestTickets { get; set; }
        public IDbSet<CardExternalCodeByCurrency> CardExternalCodeByCurrencies { get; set; }
        public IDbSet<ARInvoiceLineAction> ARInvoiceLineActions { get; set; }


        public IDbSet<SATInterface> SATInterfaces { get; set; }
        public IDbSet<SATInterfaceSetting> SATInterfaceSettings { get; set; }
        public IDbSet<SATTransferStatus> SATTransferStatus { get; set; }
        public IDbSet<SATInvoiceStatus> SATInvoiceStatus { get; set; }


        #endregion

        #region Quotes Context
        public IDbSet<Quote> Quotes { get; set; }
        public IDbSet<QuoteCharge> QuoteCharges { get; set; }
        public IDbSet<QuotePriceSteps> QuotePriceSteps { get; set; }
        public IDbSet<QuoteType> QuoteTypes { get; set; }
        public IDbSet<QuoteTemplateDetailsField> QuoteTemplateDetailsFields { get; set; }
        public IDbSet<QuoteTemplateHeaderField> QuoteTemplateHeaderFields { get; set; }
        public IDbSet<QuoteTemplateTextDesign> QuoteTemplateTextDesigns { get; set; }
        public IDbSet<QuoteTemplateTableDesign> QuoteTemplateTableDesigns { get; set; }
        public IDbSet<QuoteDocumentVersion> QuoteDocumentVersions { get; set; }
        public IDbSet<BorderType> BorderTypes { get; set; }
        public IDbSet<MarkUpType> MarkUpTypes { get; set; }
        public IDbSet<QuoteCustomerType> QuoteCustomerTypes { get; set; }
        public IDbSet<QuotePackage> QuotePackages { get; set; }
        public IDbSet<QuoteTemplate> QuoteTemplates { get; set; }
        public IDbSet<QuoteTemplateSetting> QuoteTemplateSettings { get; set; }
        public IDbSet<QuoteTemplateSectionType> QuoteTemplateSectionTypes { get; set; }
        public IDbSet<QuoteTemplateSection> QuoteTemplateSections { get; set; }
        public IDbSet<QuoteTemplateSectionModification> QuoteTemplateSectionModifications { get; set; }
        public IDbSet<QuoteTemplateTextCode> QuoteTemplateTextCodes { get; set; }
        public IDbSet<QuoteStage> QuoteStages { get; set; }
        public IDbSet<QuoteRating> QuoteRatings { get; set; }
        public IDbSet<QuoteTemplateExcludedSection> QuoteTemplateExcludedSections { get; set; }
        public IDbSet<QuoteTotalVAT> QuoteTotalVATs { get; set; }
        #endregion

        #region Custom Context

        public IDbSet<MamanStatus> MamanStatuses
        {
            get; set;

        }
	

        public IDbSet<AcceptanceStatus> AcceptanceStatuses
        {
            get; set;

        }

        public IDbSet<CustomerIdentifyType> CustomerIdentifyTypes
        {
            get;
            set;
        }


        public IDbSet<CustomsVerificationStatusType> CustomsVerificationStatusTypes
        {
            get;
            set;
        }

        public IDbSet<CustomsDocumentsTicket> CustomsDocumentsTickets
        {
            get;
            set;
        }


        public IDbSet<CustomsSetting> CustomsSettings
        {
            get;
            set;

        }


        public IDbSet<LeadDocumentExceptionType> LeadDocumentExceptionTypes
        {
            get;
            set;

        }

        public IDbSet<ClosedTableStatus> ClosedTableStatus
        {
            get;
            set;

        }

        public IDbSet<CustomsClosedTable> CustomsClosedTables
        {
            get;
            set;

        }

        public IDbSet<CollateralAnswerStatus> CollateralAnswerStatus
        {
            get;
            set;

        }

        public IDbSet<CollateralAnswerType> CollateralAnswerTypes
        {
            get;
            set;

        }

        public IDbSet<CollateralRequestStatus> CollateralRequestStatus
        {
            get;
            set;

        }

        public IDbSet<CollateralType> CollateralTypes
        {
            get;
            set;

        }

        public IDbSet<CustomsCollateral> CustomsCollaterals
        {
            get;
            set;

        }

        public IDbSet<CustomsCollateralsAnswer> CustomsCollateralsAnswers
        {
            get;
            set;

        }
        public IDbSet<CustomsCollateralsCondition> CustomsCollateralsConditions
        {
            get;
            set;

        }

        public IDbSet<CustomsHouseType> CustomsHouseTypes
        {
            get;
            set;

        }


        public IDbSet<ReturnCondition> ReturnConditions
        {
            get;
            set;

        }

        public IDbSet<AddressContactState> AddressContactStates
        {
            get;
            set;

        }

        public IDbSet<AddressPurpose> AddressPurposes
        {
            get;
            set;

        }

        public IDbSet<AttachmentType> AttachmentTypes
        {
            get;
            set;

        }

        public IDbSet<AuthorizedSignerPermit> AuthorizedSignerPermits
        {
            get;
            set;

        }

        public IDbSet<AutonomyType> AutonomyTypes
        {
            get;
            set;

        }

        public IDbSet<Bank> Banks
        {
            get;
            set;

        }

        public IDbSet<CargoIdentifireType> CargoIdentifireTypes
        {
            get;
            set;

        }

        public IDbSet<CertificateExemptionType> CertificateExemptionTypes
        {
            get;
            set;

        }

        public IDbSet<CheckEntityType> CheckEntityTypes
        {
            get;
            set;

        }

        public IDbSet<CheckQueueType> CheckQueueTypes
        {
            get;
            set;

        }

        public IDbSet<CheckRepresentativeType> CheckRepresentativeTypes
        {
            get;
            set;

        }

        public IDbSet<City> Cities
        {
            get;
            set;
        }

        public IDbSet<Client> Clients
        {
            get;
            set;

        }

        public IDbSet<ClientAddress> ClientAddresses
        {
            get;
            set;

        }

        public IDbSet<ClientsAddressCommType> ClientsAddressCommTypes
        {
            get;
            set;

        }

        public IDbSet<CommunicationType> CommunicationTypes
        {
            get;
            set;

        }

        public IDbSet<ConfirmationType> ConfirmationTypes
        {
            get;
            set;

        }

        public IDbSet<Consignment> Consignments
        {
            get;
            set;

        }

        public IDbSet<ConsignmentInternalTransition> ConsignmentInternalTransitions
        {
            get;
            set;

        }

        public IDbSet<ConsignmentPackage> ConsignmentPackages
        {
            get;
            set;

        }

        public IDbSet<ConsignmentPackDanger> ConsignmentPackDangers
        {
            get; set;

        }

        public IDbSet<ConstraintStatus> ConstraintStatuses
        {
            get;
            set;

        }

        public IDbSet<ConstraintType> ConstraintTypes
        {
            get;
            set;

        }

        public IDbSet<ContactRoleType> ContactRoleTypes
        {
            get;
            set;

        }

        public IDbSet<CountryGroup> CountryGroups
        {
            get;
            set;

        }

        public IDbSet<CurrencyType> CurrencyTypes
        {
            get;
            set;

        }

        public IDbSet<CustomBank> CustomBanks
        {
            get;
            set;

        }

        public IDbSet<CustomDocumentType> CustomDocumentTypes
        {
            get;
            set;

        }

        public IDbSet<CustomDocumentTypeMetaData> CustomDocumentTypeMetaData
        {
            get;
            set;

        }

        public IDbSet<CustomerActivityType> CustomerActivityTypes
        {
            get;
            set;

        }

        public IDbSet<CustomerRoleType> CustomerRoleTypes
        {
            get;
            set;

        }

        public IDbSet<CustomerTypeGeneral> CustomerTypeGenerals
        {
            get;
            set;

        }

        public IDbSet<CustomMetaDataType> CustomMetaDataTypes
        {
            get;
            set;

        }

        public IDbSet<CustomsAddressType> CustomsAddressTypes
        {
            get;
            set;

        }

        public IDbSet<CustomsAirline> CustomsAirlines
        {
            get; set;

        }

        public IDbSet<CustomsAutonomyKeyword> CustomsAutonomyKeywords
        {
            get; set;

        }
        public IDbSet<CustomsBook> CustomsBooks
        {
            get;
            set;

        }

        public IDbSet<CustomsBookType> CustomsBookTypes
        {
            get;
            set;

        }

        public IDbSet<CustomsBranch> CustomsBranches
        {
            get;
            set;

        }

        public IDbSet<CustomsCountry> CustomsCountries
        {
            get;
            set;

        }

        public IDbSet<CustomsDocument> CustomsDocuments
        {
            get;
            set;

        }

        public IDbSet<CustomsDocumentMetaDataValue> CustomsDocumentMetaDataValues
        {
            get;
            set;

        }

        public IDbSet<CustomsDocumentPointer> CustomsDocumentPointers
        {
            get;
            set;

        }

        public IDbSet<CustomsDocumentStatusType> CustomsDocumentStatusTypes
        {
            get;
            set;

        }

        public IDbSet<CustomsPaymentTerm> CustomsPaymentTerms
        {
            get;
            set;

        }

        public IDbSet<CustomsVendor> CustomsVendors
        {
            get;
            set;

        }

        public IDbSet<DangerousGoodsPackingReq> DangerousGoodsPackingReqs
        {
            get;
            set;

        }

        public IDbSet<Declaration> Declarations
        {
            get;
            set;

        }

        public IDbSet<DeclarationConstraint> DeclarationConstraints
        {
            get;
            set;

        }

        public IDbSet<DeclarationErrorMapping> DeclarationErrorMappings
        {
            get;
            set;

        }



        public IDbSet<DeclarationPayment> DeclarationPayments
        {
            get;
            set;

        }

        public IDbSet<DeclarationPaymentMethod> DeclarationPaymentMethods
        {
            get;
            set;

        }

        public IDbSet<DeclarationPaymentProtest> DeclarationPaymentProtests
        {
            get;
            set;

        }

        public IDbSet<DeclarationPending> DeclarationPendings
        {
            get; set;

        }

        public IDbSet<DeclarationStatusType> DeclarationStatusTypes
        {
            get;
            set;

        }

        public IDbSet<DeclarationTax> DeclarationTaxes
        {
            get;
            set;

        }

        public IDbSet<DeficitConnFileParagraphType> DeficitConnFileParagraphTypes
        {
            get;
            set;

        }

        public IDbSet<Deficit> Deficits
        {
            get;
            set;

        }

        public IDbSet<DeliverySiteType> DeliverySiteTypes
        {
            get;
            set;

        }

        public IDbSet<EntitlementType> EntitlementTypes
        {
            get;
            set;

        }

        public IDbSet<EntityTypeLookup> EntityTypeLookups
        {
            get;
            set;

        }

        public IDbSet<Gender> Genders
        {
            get;
            set;

        }

        public IDbSet<GovernmentProcedureType> GovernmentProcedureTypes
        {
            get;
            set;

        }

        public IDbSet<InternalBorderSiteType> InternalBorderSiteTypes
        {
            get;
            set;

        }

        public IDbSet<InternationalSite> InternationalSites
        {
            get;
            set;

        }

        public IDbSet<InvoiceType> InvoiceTypes
        {
            get;
            set;

        }

        public IDbSet<ItemGovernmentProcedureType> ItemGovernmentProcedureTypes
        {
            get;
            set;

        }

        public IDbSet<LeadDocumentType> LeadDocumentTypes
        {
            get;
            set;

        }

        public IDbSet<MeasureQualifier> MeasureQualifier
        {
            get;
            set;

        }

        public IDbSet<MeasurmentUnit> MeasurmentUnits
        {
            get;
            set;

        }

        public IDbSet<ModificationAndDiscountType> ModificationAndDiscountTypes
        {
            get;
            set;

        }

        public IDbSet<NotificationType> NotificationTypes
        {
            get;
            set;

        }

        public IDbSet<OrganizationUnitType> OrganizationUnitTypes
        {
            get;
            set;

        }

        public IDbSet<PackageMeasureQualifier> PackageMeasureQualifiers
        {
            get;
            set;

        }

        public IDbSet<PackingType> PackingTypes
        {
            get;
            set;

        }

        public IDbSet<ParagraphType> ParagraphTypes
        {
            get;
            set;

        }

        public IDbSet<PassportType> PassportTypes
        {
            get;
            set;

        }

        public IDbSet<PayerActivityType> PayerActivityTypes
        {
            get;
            set;

        }

        public IDbSet<PayerType> PayerTypes
        {
            get;
            set;

        }

        public IDbSet<PaymentMethodStatus> PaymentMethodStatus
        {
            get;
            set;

        }

        public IDbSet<PaymentMethodType> PaymentMethodTypes
        {
            get;
            set;

        }

        public IDbSet<PaymentOrder> PaymentOrders
        {
            get;
            set;

        }

        public IDbSet<PaymentOrderConnectionTable> PaymentOrderConnectionTables
        {
            get;
            set;

        }

        public IDbSet<PaymentOrderLine> PaymentOrderLines
        {
            get;
            set;

        }

        public IDbSet<PaymentOrderMethod> PaymentOrderMethods
        {
            get;
            set;

        }



        public IDbSet<PaymentOrderProtestReason> PaymentOrderProtestReasons
        {
            get;
            set;

        }

        public IDbSet<PaymentOrderStatus> PaymentOrderStatus
        {
            get;
            set;

        }

        public IDbSet<PaymentOrderType> PaymentOrderTypes
        {
            get;
            set;

        }

        public IDbSet<PaymentProcess> PaymentProcesses
        {
            get;
            set;

        }

        public IDbSet<PaymentProtestType> PaymentProtestTypes
        {
            get;
            set;

        }

        public IDbSet<PaymentType> PaymentTypes
        {
            get;
            set;

        }

        public IDbSet<PhysicalCheck> PhysicalChecks
        {
            get;
            set;

        }

        public IDbSet<PhysicalCheckOperation> PhysicalCheckOperations
        {
            get;
            set;

        }

        public IDbSet<PhysicalCheckStatusMessage> PhysicalCheckStatusMessages
        {
            get;
            set;

        }



        public IDbSet<ProductIdentificationType> ProductIdentificationTypes
        {
            get;
            set;

        }

        public IDbSet<ProductNameType> ProductNameTypes
        {
            get;
            set;

        }

        public IDbSet<PropertiesDetailsHistory> PropertiesDetailsHistorys
        {
            get;
            set;

        }

        public IDbSet<RegisteredWarehouseSiteType> RegisteredWarehouseSiteTypes
        {
            get;
            set;

        }

        public IDbSet<SalesTaxExemptionType> SalesTaxExemptionTypes
        {
            get;
            set;

        }

        public IDbSet<SiteLookup> SiteLookups
        {
            get;
            set;

        }

        public IDbSet<SiteType> SiteTypes
        {
            get;
            set;

        }

        public IDbSet<SpecializationType> SpecializationTypes
        {
            get;
            set;

        }

        public IDbSet<SubCountry> SubCountries
        {
            get;
            set;

        }

        public IDbSet<SupplierInvioceItemCertificat> SupplierInvioceItemCertificats
        {
            get;
            set;

        }

        public IDbSet<SupplierInvoice> SupplierInvoices
        {
            get;
            set;

        }

        public IDbSet<SupplierInvoiceFreightAmount> SupplierInvoiceFreightAmounts
        {
            get;
            set;

        }

        public IDbSet<SupplierInvoiceItem> SupplierInvoiceItems
        {
            get;
            set;

        }

        public IDbSet<SupplierInvoiceItemsConDeclar> SupplierInvoiceItemsConDeclars
        {
            get;
            set;

        }

        public IDbSet<SupplierInvoiceItemsDescript> SupplierInvoiceItemsDescripts
        {
            get;
            set;

        }

        public IDbSet<SupplierInvoiceItemsMod> SupplierInvoiceItemsMods
        {
            get;
            set;

        }

        public IDbSet<SupplierInvoiceItemProcesType> SupplierInvoiceItemProcesTypes
        {
            get;
            set;

        }

        public IDbSet<SupplierInvoiceItemsProdIdent> SupplierInvoiceItemsProdIdents
        {
            get;
            set;

        }


        public IDbSet<SupplierInvoiceItemsSerialNum> SupplierInvoiceItemsSerialNums
        {
            get;
            set;

        }

        public IDbSet<SupplierInvoiceItemsTax> SupplierInvoiceItemsTaxes
        {
            get;
            set;

        }

        //public IDbSet<SupplierInvoiceItemsTaxesMod> SupplierInvoiceItemsTaxesMods
        //{
        //    get;
        //    set;

        //}

        public IDbSet<SupplierInvoiceModification> SupplierInvoiceModifications
        {
            get;
            set;

        }

        public IDbSet<Tapag> Tapags
        {
            get;
            set;

        }

        public IDbSet<TapagConnectionTable> TapagConnectionTables
        {
            get;
            set;

        }

        public IDbSet<TapagType> TapagTypes
        {
            get;
            set;

        }

        public IDbSet<TermsOfSaleType> TermsOfSaleTypes
        {
            get;
            set;

        }

        public IDbSet<TradeAgreement> TradeAgreements
        {
            get;
            set;

        }

        public IDbSet<VendorCommunication> VendorCommunications
        {
            get;
            set;

        }

        public IDbSet<VendorType> VendorTypes
        {
            get;
            set;

        }

        public IDbSet<CustomsRequiredField> CustomsRequiredFields { get; set; }

        public IDbSet<CustomsExchangeRate> CustomsExchangeRates { get; set; }

        public IDbSet<CustomsRequestsSheet> CustomsRequestsSheets { get; set; }
        public IDbSet<CustomsRequestsSheetStatus> CustomsRequestsSheetStatuses { get; set; }
        public IDbSet<InterfaceSendOption> InterfaceSendOptions { get; set; }
        public IDbSet<ConstraintProcessType> ConstraintProcessTypes { get; set; }
        public IDbSet<ConstraintApprovalDecision> ConstraintApprovalDecisions { get; set; }
        // public IDbSet<Item> Items { get; set; }
        public IDbSet<CustomsTransportMode> CustomsTransportModes { get; set; }
        // public IDbSet<TenantCustomsBranch> TenantCustomsBranches { get; set; }
        public IDbSet<CustomsHouseTypeAdditional> CustomsHouseTypeAdditionals { get; set; }
        public IDbSet<CustomsItem> CustomsItems { get; set; }
        public IDbSet<CustomsItemDetailsHistory> CustomsItemDetailsHistorys { get; set; }
        public IDbSet<ImporterTypeForClaim> ImporterTypeForClaims
        {
            get;
            set;

        }
        public IDbSet<InterfaceManagement> InterfaceManagements { get; set; }
        public IDbSet<InterfaceTenantDefinition> InterfaceTenantDefinitions { get; set; }
        public IDbSet<NotificationDefinition> NotificationDefinitions { get; set; }
        public IDbSet<AssigneeNotificationType> AssigneeNotificationTypes { get; set; }
        public IDbSet<Notification> Notifications { get; set; }
        public IDbSet<NotificationTenantDefinition> NotificationTenantDefinition { get; set; }
        public IDbSet<Deposit> Deposits { get; set; }
        public IDbSet<DepositEssenceType> DepositEssenceTypes { get; set; }
        public IDbSet<DepositCondition> DepositConditions { get; set; }
        public IDbSet<UnloadingSiteType> UnloadingSiteTypes { get; set; }
        public IDbSet<MorningMessageType> MorningMessageTypes { get; set; }
        public IDbSet<VendorStatus> VendorStatuses { get; set; }
        public IDbSet<VendorTransactionType> VendorTransactionTypes { get; set; }
        public IDbSet<ValidCustomsItem> ValidCustomsItems { get; set; }
        public IDbSet<CustomBanksCard> CustomBanksCards { get; set; }
        public IDbSet<TradeLevyExamptType> TradeLevyExamptTypes { get; set; }
        public IDbSet<SupplierInvoiceItemsLevy> SupplierInvoiceItemsLevies { get; set; }
        public IDbSet<ImporterDesposition> ImporterDespositions { get; set; }
        public IDbSet<Guarantee> Guarantees { get; set; }
        public IDbSet<RequiredGuaranteeType> RequiredGuaranteeTypes { get; set; }
        public IDbSet<GuaranteeCertificateType> GuaranteeCertificateTypes { get; set; }
        public IDbSet<GuaranteeCondition> GuaranteeConditions { get; set; }

        public IDbSet<DBMigration> DBMigrations
        {
            get; set;

        }

        public IDbSet<DBMigrationLine> DBMigrationLines
        {
            get; set;

        }
        public IDbSet<DebtNotificationType> DebtNotificationTypes { get; set; }
        public IDbSet<DemanderType> DemanderTypes { get; set; }
        public IDbSet<DepositCustomerActivity> DepositCustomerActivities { get; set; }
        public IDbSet<RequestStatus> RequestStatuses { get; set; }
        public IDbSet<CollateralsRequestFileCond> CollateralsRequestFileConds { get; set; }
        public IDbSet<StorageMessageType> StorageMessageTypes { get; set; }
        public IDbSet<SpecialActionDescriptionType> SpecialActionDescriptionTypes { get; set; }
        public IDbSet<ProceduralFault> ProceduralFaults { get; set; }
        public IDbSet<ProceduralFaultStatus> ProceduralFaultStatuses { get; set; }
        public IDbSet<ProceduralFaultInSourceType> ProceduralFaultInSourceTypes { get; set; }
        public IDbSet<FaultInspectionType> FaultInspectionTypes { get; set; }
        public IDbSet<ProceduralFaultType> ProceduralFaultTypes { get; set; }
        public IDbSet<ProcessingReason> ProcessingReasons
        {
            get;
            set;

        }

        public IDbSet<ProceduralFaultInProcessType> ProceduralFaultInProcessTypes { get; set; }
        public IDbSet<RansomViolationType> RansomViolationTypes { get; set; }
        public IDbSet<ProceduralFaultsConnEntity> ProceduralFaultsConnEntities { get; set; }
        public IDbSet<CargoIdentityQualifier> CargoIdentityQualifiers { get; set; }
        public IDbSet<DepositFileType> DepositFileTypes { get; set; }

        public IDbSet<Vehicle> Vehicles { get; set; }
        public IDbSet<VehiclePoolType> VehiclePoolTypes { get; set; }
        public IDbSet<VehiclePriceListType> VehiclePriceListTypes { get; set; }
        public IDbSet<VehicleManufacturer> VehicleManufacturers { get; set; }
        public IDbSet<ConverterType> ConverterType { get; set; }
        public IDbSet<VehicleTecnologyType> VehicleTecnologyTypes { get; set; }
        public IDbSet<FuelType> FuelTypes { get; set; }
        public IDbSet<VehicleStatus> VehicleStatuses { get; set; }
        public IDbSet<VehicleType> VehicleTypes { get; set; }
        public IDbSet<VehicleSafetyAccessory> VehicleSafetyAccessories { get; set; }
        public IDbSet<VehicleOwner> VehicleOwners { get; set; }
        public IDbSet<VehicleSafeAccessoryInstlType> VehicleSafeAccessoryInstlTypes { get; set; }
        public IDbSet<SupplierInvoiceItemVehicle> SupplierInvoiceItemVehicles { get; set; }
        public IDbSet<CustomsPartnersItem> CustomsPartnersItems { get; set; }
        public IDbSet<CustomsPartnerFtp> CustomsPartnerFtps { get; set; }
        public IDbSet<SignatureType> SignatureTypes { get; set; }
        public IDbSet<CheckEssenceLookup> CheckEssenceLookups { get; set; }
        public IDbSet<Authority> Authorities { get; set; }
        public IDbSet<VehicleReductionType> VehicleReductionTypes { get; set; }
        public IDbSet<SupplierInvoiceItemVehicleMod> SupplierInvoiceItemVehicleMods { get; set; }
        public IDbSet<VehicleSafetyAccessoryType> VehicleSafetyAccessoryTypes { get; set; }
        public IDbSet<GuaranteeCustomerActivity> GuaranteeCustomerActivities { get; set; }
        public IDbSet<HazardousSubstance> HazardousSubstances { get; set;}
        public IDbSet<CheckTypeLookup> CheckTypeLookups { get; set; }
        public IDbSet<SupplierInvoiceItemModVehicle> SupplierInvoiceItemModVehicles { get; set; }
        public IDbSet<CertificatesStatus> CertificatesStatuses { get; set; }
        public IDbSet<AmendmentRequestStatus> AmendmentRequestStatuses { get; set; }
        public IDbSet<AmendmentStatus> AmendmentStatuses {get; set;}
        public IDbSet<AmendmentType> AmendmentTypes  { get; set; }
        public IDbSet<DeclarationStatementType> DeclarationStatementTypes { get; set; }
        public IDbSet<AmendmentFieldReasonType> AmendmentFieldReasonTypes { get; set; }
        public IDbSet<VendorCommission> VendorCommissions { get; set; }

        public IDbSet<SupplierInvoiceItemVehicleAdd> SupplierInvoiceItemVehicleAdds { get; set; }
        public IDbSet<NotificationReply> NotificationReplies { get; set; }
        public IDbSet<FacilitationType> FacilitationTypes { get; set; }
        public IDbSet<CustomsInsuranceCompany> CustomsInsuranceCompanies { get; set; }
        public IDbSet<AccumalationState> AccumalationStates { get; set; }
        public IDbSet<StorageStatus> StorageStatuses { get; set; }
        public IDbSet<CourierMaster> CourierMasters { get; set; }
        public IDbSet<CourierDeclaration> CourierDeclarations { get; set; }
        public IDbSet<CargoStatus> CargoStatuses { get; set; }
        public IDbSet<MAWBType> MAWBTypes { get; set; }
        public IDbSet<DeclarationConsAcceptance> DeclarationConsAcceptances { get; set; }
        public IDbSet<CourierCustomStatus> CourierCustomStatuses { get; set; }
        public IDbSet<AgentTalkBackType> AgentTalkBackTypes { get; set; }
        public IDbSet<ManifestCargoStatus> ManifestCargoStatuses { get; set; }
        public IDbSet<CourierManifestStatus> CourierManifestStatuses { get; set; }
        public IDbSet<CourierDeclarationStatus> CourierDeclarationStatuses { get; set; }
        public IDbSet<CourierPaymentStatus> CourierPaymentStatuses { get; set; }
        public IDbSet<ActionCode> ActionCodes { get; set; }
        public IDbSet<SplitOrMergeReason> SplitOrMergeReasons { get; set; }
        public IDbSet<CargoSplitRequestStatus> CargoSplitRequestStatuses { get; set; }
        public IDbSet<DeclarationCargoSplit> DeclarationCargoSplits { get; set; }
        public IDbSet<DecCargoSplitCon> DecCargoSplitCons { get; set; }
        public IDbSet<TreatmentWay> TreatmentWays { get; set; }
        public IDbSet<DecCargoSplitConsItem> DecCargoSplitConsItems { get; set; }
        public IDbSet<DecCargoSplitConsPackDet> DecCargoSplitConsPackDets { get; set; }
        public IDbSet<DecDangersContact> DecDangersContacts
        {
            get; set;

        }
        public IDbSet<TPGFileType> TPGFileTypes { get; set; }
        public IDbSet<DecCargoSplitCargoIdentifier> DecCargoSplitCargoIdentifiers { get; set; }


        #endregion

        #region CRM Context
        public IDbSet<Activity> Activities { get; set; }
        public IDbSet<ActivityStatus> ActivityStatus { get; set; }
        public IDbSet<ActivityTimeType> ActivityTimeTypes { get; set; }
        public IDbSet<ActivityType> ActivityTypes { get; set; }
        public IDbSet<CallType> CallTypes { get; set; }
        public IDbSet<Opportunity> Opportunities { get; set; }
        public IDbSet<OpportunityCompetitor> OpportunityCompetitors { get; set; }
        public IDbSet<OpportunityCompetitorProduct> OpportunityCompetitorProducts { get; set; }
        public IDbSet<OpportunityProduct> OpportunityProducts { get; set; }
        public IDbSet<OpportunityProductLocation> OpportunityProductLocations { get; set; }
        public IDbSet<OpportunityType> OpportunityTypes { get; set; }
        public IDbSet<ActivityPriority> ActivityPriorities { get; set; }
        public IDbSet<Rating> Ratings { get; set; }
        public IDbSet<Stage> Stages { get; set; }
        public IDbSet<OpportunityStage> OpportunityStages { get; set; }
        public IDbSet<ActivityInvitee> ActivityInvitees { get; set; }
        public IDbSet<ActivityEmailRecipient> ActivityEmailRecipients { get; set; }
        public IDbSet<OpportunityAdditionalService> OpportunityAdditionalServices { get; set; }
        public IDbSet<ActivityNote> ActivityNotes { get; set; }
        public IDbSet<CRMFilterSetting> CRMFilterSettings { get; set; }

        public IDbSet<Ticket> Tickets { get; set; }
        public IDbSet<Correspondence> Correspondences { get; set; }
        public IDbSet<TicketClassification> TicketClassifications { get; set; }
        public IDbSet<TicketStage> TicketStages { get; set; }
        public IDbSet<TicketSeverity> TicketSeveritys { get; set; }
        public IDbSet<TicketType> TicketTypes { get; set; }
        public IDbSet<EmployeeGroup> EmployeeGroups { get; set; }
        public IDbSet<EmployeeGroupLine> EmployeeGroupLines { get; set; }

        public IDbSet<SLAHeader> SLAHeaders { get; set; }
        public IDbSet<SLALine> SLALines { get; set; }
        public IDbSet<TimeUnit> TimeUnits { get; set; }

        public IDbSet<EscalationActionTimeIndicator> EscalationActionTimeIndicators { get; set; }
        public IDbSet<EscalationPreDefinition> EscalationPreDefinitions { get; set; }

        public IDbSet<SLAEscalation> SLAEscalations { get; set; }
        public IDbSet<SLAEscalationRecepient> SLAEscalationRecepients { get; set; }

        public IDbSet<TicketEscalation> TicketEscalations { get; set; }

        public IDbSet<CorrespondencesAttachment> CorrespondencesAttachments { get; set; }

        public IDbSet<TicketCreatedByType> TicketCreatedByTypes { get; set; }
        public IDbSet<TicketSource> TicketSources { get; set; }
        #endregion

        #region Social Context



        public IDbSet<ConversationHeader> ConversationHeaders
        {
            get;
            set;

        }

        public IDbSet<ConversationHeaderMessage> ConversationHeaderMessages
        {
            get;
            set;

        }


        public IDbSet<ConversationHeaderParticipant> ConversationHeaderParticipants
        {
            get;
            set;

        }

        public IDbSet<Feed> Feeds
        {
            get;
            set;

        }

        public IDbSet<FollowEntity> FollowEntities
        {
            get;
            set;

        }

        public IDbSet<Follower> Followers
        {
            get;
            set;

        }

        public IDbSet<Group> Groups
        {
            get;
            set;

        }

        public IDbSet<GroupMember> GroupMembers
        {
            get;
            set;

        }

        public IDbSet<Post> Posts
        {
            get;
            set;

        }

        public IDbSet<PostLike> PostLikes
        {
            get;
            set;

        }
        #endregion

        #region Booking Context
        public IDbSet<Booking> Bookings { get; set; }
        public IDbSet<BookingAnswer> BookingAnswers { get; set; }
        public IDbSet<BookingAnswerStatus> BookingAnswerStatus { get; set; }
        public IDbSet<BookingPackage> BookingPackages { get; set; }
        public IDbSet<BookingSpaceAllocation> BookingSpaceAllocations { get; set; }
        public IDbSet<FFRStatus> FFRStatus { get; set; }
        public IDbSet<FlightsSchedulesRequest> FlightsSchedulesRequests { get; set; }
        public IDbSet<FlightsSchedulesResponse> FlightsSchedulesResponses { get; set; }
        public IDbSet<FlightsSchedulesRequestStatus> FlightsSchedulesRequestStatus { get; set; }
        public IDbSet<BookingLevel> BookingLevels { get; set; }
        public IDbSet<BookingProduct> BookingProducts { get; set; }
        #endregion

        #region Accouting Region
        //public IDbSet<JournalActionType> JournalActionTypes { get; set; }
        //public IDbSet<JournalStatusType> JournalStatusTypes { get; set; }
        //public IDbSet<JournalType> JournalTypes { get; set; }
        //public IDbSet<TestEntity> TestEntities { get; set; }
        //public IDbSet<AccountingEntity> AccountingEntities { get; set; }
        //public IDbSet<ChartOfAccount> ChartOfAccounts { get; set; }
        //public IDbSet<ChartOfAccountsType> ChartOfAccountsTypes { get; set; }
        //public IDbSet<GLAccountType> GLAccountTypes { get; set; }
        //public IDbSet<RevenueExpenseType> RevenueExpenseTypes { get; set; }
        //public IDbSet<GLAccount> GLAccounts { get; set; }

        public IDbSet<PaymentChequeStatus> PaymentChequeStatuses
        {
            get;
            set;
        }
        public IDbSet<AccountingEntity> AccountingEntities
        {
            get;
            set;
        }




        public IDbSet<BankCode> BankCodes
        {
            get;
            set;

        }

        public IDbSet<BankAccount> BankAccounts
        {
            get;
            set;

        }

        public IDbSet<ChartOfAccount> ChartOfAccounts
        {
            get;
            set;
        }


        public IDbSet<ChartOfAccountsType> ChartOfAccountsTypes
        {
            get;
            set;
        }


        public IDbSet<BankDeposit> BankDeposits
        {
            get;
            set;
        }

        public IDbSet<BankDepositLine> BankDepositLines
        {
            get;
            set;
        }

        public IDbSet<FullAccountingSetting> FullAccountingSettings
        {
            get;
            set;

        }


        public IDbSet<GLAccount> GLAccounts
        {
            get;
            set;
        }


        public IDbSet<GLAccountType> GLAccountTypes
        {
            get;
            set;
        }


        public IDbSet<Journal> Journals
        {
            get;
            set;
        }



        public IDbSet<JournalLine> JournalLines
        {
            get;
            set;
        }




        public IDbSet<LedgerTransaction> LedgerTransactions
        {
            get;
            set;
        }


        public IDbSet<GLAccountTotalByMonth> GLAccountTotalByMonths
        {
            get;
            set;
        }

        public IDbSet<GLAccountTotalDateType> GLAccountTotalDateTypes
        {
            get;
            set;

        }

        public IDbSet<ReconcileMethod> ReconcileMethods
        {
            get;
            set;
        }

        public IDbSet<PeriodType> PeriodTypes
        {
            get;
            set;
        }

        public IDbSet<AccountingPeriod> AccountingPeriods
        {
            get;
            set;
        }

        public IDbSet<AutomaticReconcile> AutomaticReconciles
        {
            get;
            set;
        }

        public IDbSet<AutomaticReconcileMethod> AutomaticReconcileMethods
        {
            get;
            set;
        }

        public IDbSet<Reconciliation> Reconciliations
        {
            get;
            set;
        }

        public IDbSet<ReconciliationLine> ReconciliationLines
        {
            get;
            set;
        }

        public IDbSet<CashBookType> CashBookTypes
        {
            get;
            set;
        }

        public IDbSet<CashBook> CashBooks
        {
            get;
            set;
        }

        public IDbSet<Accounting.Data.EntityPOCOs.ARPaymentCheque> ARPaymentCheques
        {
            get;
            set;
        }

        public IDbSet<CashBookLine> CashBookLines
        {
            get;
            set;
        }

        public IDbSet<Revaluation> Revaluations
        {
            get;
            set;
        }

        public IDbSet<GLAccountCurrency> GLAccountCurrencies
        {
            get;
            set;
        }

        public IDbSet<ARPaymentChequeStatus> ARPaymentChequeStatues
        {
            get;
            set;

        }

        public IDbSet<ReconcileExternalPage> ReconcileExternalPages { get; set; }
        public IDbSet<ReconcileExternalPageLine> ReconcileExternalPageLines { get; set; }
        public IDbSet<ReconcileExternalPageStatus> ReconcileExternalPageStatuses { get; set; }

        #endregion

        #region Warehouse Context



        public IDbSet<WarehouseEntry> WarehouseEntries
        {
            get;
            set;

        }

        public IDbSet<WarehouseRelease> WarehouseReleases
        {
            get;
            set;

        }


        public IDbSet<WarehouseEntryPackage> WarehouseEntryPackages
        {
            get;
            set;

        }

        public IDbSet<WarehouseReleasePackage> WarehouseReleasePackages
        {
            get;
            set;

        }

        public IDbSet<WarehouseEntryPackagesRelease> WarehouseEntryPackagesReleases
        {
            get;
            set;

        }

        public IDbSet<WarehouseEntryStatus> WarehouseEntryStatuses
        {
            get;
            set;

        }

        public IDbSet<WarehouseReleaseStatus> WarehouseReleaseStatuses
        {
            get;
            set;

        }

        #endregion

        #region time Management

        public IDbSet<TMEmployeeTime> TMEmployeeTimes
        {
            get;
            set;

        }

        public IDbSet<TMLocation> TMLocations
        {
            get;
            set;

        }

        public IDbSet<TMProject> TMProjects
        {
            get;
            set;

        }
        public IDbSet<TMOfficeHour> TMOfficeHours
        {
            get;
            set;

        }
        #endregion


       


        #region Infrastructure Generated
        public IDbSet<Toggle> Toggles
        {
            get;
            set;

        }

        public IDbSet<FeatureToggle> FeatureToggle
        {
            get;
            set;

        }

        public IDbSet<BIReport> BIReports
        {
            get;
            set;

        }
        public IDbSet<BIReportsType> BIReportsTypes
        {
            get;
            set;

        }

        public IDbSet<BusinessRole> BusinessRoles
        {
            get;
            set;

        }

        public IDbSet<BusinessProcessQueue> BusinessProcessQueues
        {
            get;
            set;

        }

        public IDbSet<Team> Teams
        {
            get;
            set;

        }

        public IDbSet<LBPTeamMember> LBPTeamMembers
        {
            get;
            set;

        }

        public IDbSet<TeamMemberBusinessRole> TeamMemberBusinessRoles
        {
            get;
            set;

        }

        public IDbSet<BatchTaskExecution> BatchTaskExecutions
        {
            get;
            set;

        }

        public IDbSet<BatchTaskExecutionStatus> BatchTaskExecutionStatus
        {
            get;
            set;

        }

        public IDbSet<SharedLogisticsSetting> SharedLogisticsSettings
        {
            get;
            set;

        }
        #endregion
        public IDbSet<UsersReleaseNotesDisplay> UsersReleaseNotesDisplays { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UsersReleaseNotesDisplayMap());
            #region Social
            modelBuilder.Configurations.Add(new ConversationHeaderMap());
            modelBuilder.Configurations.Add(new ConversationHeaderMessageMap());
            modelBuilder.Configurations.Add(new ConversationHeaderParticipantMap());
            modelBuilder.Configurations.Add(new FeedMap());
            modelBuilder.Configurations.Add(new FollowEntityMap());
            modelBuilder.Configurations.Add(new FollowerMap());
            modelBuilder.Configurations.Add(new GroupMap());
            modelBuilder.Configurations.Add(new GroupMemberMap());
            modelBuilder.Configurations.Add(new PostMap());
            modelBuilder.Configurations.Add(new PostLikeMap());
            #endregion

            #region CRM
            modelBuilder.Configurations.Add(new ActivityMap());
            modelBuilder.Configurations.Add(new ActivityEmailRecipientMap());
            modelBuilder.Configurations.Add(new ActivityInviteeMap());
            modelBuilder.Configurations.Add(new ActivityNoteMap());
            modelBuilder.Configurations.Add(new ActivityOwnerHistoryMap());
            modelBuilder.Configurations.Add(new ActivityPriorityMap());
            modelBuilder.Configurations.Add(new ActivityStatusMap());
            modelBuilder.Configurations.Add(new ActivityTimeTypeMap());
            modelBuilder.Configurations.Add(new ActivityTypeMap());
            modelBuilder.Configurations.Add(new CallTypeMap());
            modelBuilder.Configurations.Add(new CRMFilterSettingMap());
            modelBuilder.Configurations.Add(new OpportunityMap());
            modelBuilder.Configurations.Add(new OpportunityAdditionalServiceMap());
            modelBuilder.Configurations.Add(new OpportunityClosingReasonMap());
            modelBuilder.Configurations.Add(new OpportunityCompetitorMap());
            modelBuilder.Configurations.Add(new OpportunityCompetitorProductMap());
            modelBuilder.Configurations.Add(new OpportunityProductMap());
            modelBuilder.Configurations.Add(new OpportunityProductLocationMap());
            modelBuilder.Configurations.Add(new OpportunityStageMap());
            modelBuilder.Configurations.Add(new OpportunityTypeMap());
            modelBuilder.Configurations.Add(new QuestionnaireMap());
            modelBuilder.Configurations.Add(new QuestionnaireAnswerMap());
            modelBuilder.Configurations.Add(new QuestionnaireAnswerLineMap());
            modelBuilder.Configurations.Add(new QuestionnaireQuestionMap());
            modelBuilder.Configurations.Add(new RatingMap());
            modelBuilder.Configurations.Add(new StageMap());

            modelBuilder.Configurations.Add(new TicketMap());
            modelBuilder.Configurations.Add(new CorrespondenceMap());
            modelBuilder.Configurations.Add(new TicketClassificationMap());
            modelBuilder.Configurations.Add(new TicketStageMap());
            modelBuilder.Configurations.Add(new TicketSeverityMap());
            modelBuilder.Configurations.Add(new TicketTypeMap());
            modelBuilder.Configurations.Add(new EmployeeGroupMap());
            modelBuilder.Configurations.Add(new EmployeeGroupLineMap());

            modelBuilder.Configurations.Add(new SLAHeaderMap());
            modelBuilder.Configurations.Add(new SLALineMap());
            modelBuilder.Configurations.Add(new TimeUnitMap());

            modelBuilder.Configurations.Add(new EscalationActionTimeIndicatorMap());
            modelBuilder.Configurations.Add(new EscalationPreDefinitionMap());
            modelBuilder.Configurations.Add(new SLAEscalationMap());
            modelBuilder.Configurations.Add(new SLAEscalationRecepientMap());

            modelBuilder.Configurations.Add(new TicketEscalationMap());

            modelBuilder.Configurations.Add(new CorrespondencesAttachmentMap());
            modelBuilder.Configurations.Add(new TicketSourceMap());
            modelBuilder.Configurations.Add(new TicketCreatedByTypeMap());
            #endregion

            #region Booking
            modelBuilder.Configurations.Add(new BookingMap());
            modelBuilder.Configurations.Add(new BookingAnswerMap());
            modelBuilder.Configurations.Add(new BookingAnswerStatusMap());
            modelBuilder.Configurations.Add(new BookingPackageMap());
            modelBuilder.Configurations.Add(new BookingSpaceAllocationMap());
            modelBuilder.Configurations.Add(new BookingStatusMap());
            modelBuilder.Configurations.Add(new FFRStatusMap());
            modelBuilder.Configurations.Add(new BookingLastRequestMap());
            modelBuilder.Configurations.Add(new FlightsSchedulesRequestMap());
            modelBuilder.Configurations.Add(new FlightsSchedulesResponseMap());
            modelBuilder.Configurations.Add(new FlightsSchedulesRequestStatusMap());
            modelBuilder.Configurations.Add(new BookingLevelMap());
            modelBuilder.Configurations.Add(new BookingProductMap());
            #endregion

            #region customs
            modelBuilder.Configurations.Add(new MamanStatusMap());
            modelBuilder.Configurations.Add(new AcceptanceStatusMap());
            modelBuilder.Configurations.Add(new CustomerIdentifyTypeMap());

            modelBuilder.Configurations.Add(new CustomsVerificationStatusTypeMap());

            modelBuilder.Configurations.Add(new AddressContactStateMap());

            modelBuilder.Configurations.Add(new AddressPurposeMap());

            modelBuilder.Configurations.Add(new AssigneeNotificationTypeMap());

            modelBuilder.Configurations.Add(new AttachmentTypeMap());

            modelBuilder.Configurations.Add(new AuthorizedSignerPermitMap());

            modelBuilder.Configurations.Add(new AutonomyTypeMap());

            modelBuilder.Configurations.Add(new BankMap());

            modelBuilder.Configurations.Add(new CargoIdentifireTypeMap());

            modelBuilder.Configurations.Add(new CargoIdentityQualifierMap());

            modelBuilder.Configurations.Add(new CertificateExemptionTypeMap());

            modelBuilder.Configurations.Add(new CheckEntityTypeMap());

            modelBuilder.Configurations.Add(new CheckQueueTypeMap());

            modelBuilder.Configurations.Add(new CheckRepresentativeTypeMap());

            modelBuilder.Configurations.Add(new CityMap());

            modelBuilder.Configurations.Add(new ClaimMap());

            modelBuilder.Configurations.Add(new ClaimEntityMap());

            modelBuilder.Configurations.Add(new ClaimsRelatedEntityMap());

            modelBuilder.Configurations.Add(new ClaimsRelatedEntitiesAmountMap());

            modelBuilder.Configurations.Add(new ClaimsRelatedEntitiesReasonMap());

            modelBuilder.Configurations.Add(new ClaimsRelatedEntsReasonsExpMap());

            modelBuilder.Configurations.Add(new ClaimsRelatedEntsExpDeclarMap());

            modelBuilder.Configurations.Add(new ClaimExplanationCodeMap());

            modelBuilder.Configurations.Add(new ClaimImporterDeclarsPage3Map());

            modelBuilder.Configurations.Add(new ClaimImporterDeclarsPage3AMap());

            modelBuilder.Configurations.Add(new ClaimImporterDeclarsPage3BMap());

            modelBuilder.Configurations.Add(new ClaimImporterDeclarsP3LoiMap());

            modelBuilder.Configurations.Add(new CommercialSaleMap());

            modelBuilder.Configurations.Add(new ContinuousMessagesTypeCodeMap());

            modelBuilder.Configurations.Add(new FreightPaymentMethodMap());

            modelBuilder.Configurations.Add(new ImporterDeclarationTypeMap());

            modelBuilder.Configurations.Add(new CourtInstanceMap());

            modelBuilder.Configurations.Add(new ClientMap());

            modelBuilder.Configurations.Add(new ClientAddressMap());

            modelBuilder.Configurations.Add(new ClientsAddressCommTypeMap());

            modelBuilder.Configurations.Add(new ClosedTableStatusMap());

            modelBuilder.Configurations.Add(new CollateralAnswerStatusMap());

            modelBuilder.Configurations.Add(new CollateralAnswerTypeMap());

            modelBuilder.Configurations.Add(new CollateralRequestStatusMap());

            modelBuilder.Configurations.Add(new CollateralsRequestFileCondMap());

            modelBuilder.Configurations.Add(new CollateralTypeMap());

            modelBuilder.Configurations.Add(new CommunicationTypeMap());

            modelBuilder.Configurations.Add(new ConfirmationTypeMap());

            modelBuilder.Configurations.Add(new ConsignmentMap());

            modelBuilder.Configurations.Add(new ConsignmentInternalTransitionMap());

            modelBuilder.Configurations.Add(new ConsignmentPackageMap());

            modelBuilder.Configurations.Add(new ConsignmentPackDangerMap());

            modelBuilder.Configurations.Add(new ConstraintApprovalDecisionMap());

            modelBuilder.Configurations.Add(new ConstraintProcessTypeMap());

            modelBuilder.Configurations.Add(new ConstraintStatusMap());

            modelBuilder.Configurations.Add(new ConstraintTypeMap());

            modelBuilder.Configurations.Add(new ContactRoleTypeMap());

            modelBuilder.Configurations.Add(new ConverterTypeMap());

            modelBuilder.Configurations.Add(new CountryGroupMap());

            modelBuilder.Configurations.Add(new CurrencyTypeMap());

            modelBuilder.Configurations.Add(new CustomBankMap());

            modelBuilder.Configurations.Add(new CustomBanksCardMap());

            modelBuilder.Configurations.Add(new CustomDocumentTypeMap());

            modelBuilder.Configurations.Add(new CustomDocumentTypeMetaDataMap());

            modelBuilder.Configurations.Add(new CustomerActivityTypeMap());

            modelBuilder.Configurations.Add(new CustomerRoleTypeMap());

            modelBuilder.Configurations.Add(new CustomerTypeGeneralMap());

            modelBuilder.Configurations.Add(new CustomMetaDataTypeMap());

            modelBuilder.Configurations.Add(new CustomsAddressTypeMap());

            modelBuilder.Configurations.Add(new CustomsBookMap());

            modelBuilder.Configurations.Add(new CustomsBookTypeMap());

            modelBuilder.Configurations.Add(new CustomsBranchMap());

            modelBuilder.Configurations.Add(new CustomsClosedTableMap());

            modelBuilder.Configurations.Add(new CustomsCollateralMap());

            modelBuilder.Configurations.Add(new CustomsCollateralsAnswerMap());

            modelBuilder.Configurations.Add(new CustomsCollateralsConditionMap());

            modelBuilder.Configurations.Add(new CustomsCountryMap());

            modelBuilder.Configurations.Add(new CustomsDocumentMap());

            modelBuilder.Configurations.Add(new CustomsDocumentMetaDataValueMap());

            modelBuilder.Configurations.Add(new CustomsDocumentPointerMap());

            modelBuilder.Configurations.Add(new CustomsDocumentStatusTypeMap());

            modelBuilder.Configurations.Add(new CustomsDocumentsTicketMap());

            modelBuilder.Configurations.Add(new CustomsEnvoirmentTypeMap());

            modelBuilder.Configurations.Add(new CustomsExchangeRateMap());

            modelBuilder.Configurations.Add(new CustomsHouseTypeMap());

            modelBuilder.Configurations.Add(new CustomsHouseTypeAdditionalMap());

            modelBuilder.Configurations.Add(new CustomsItemMap());

            modelBuilder.Configurations.Add(new CustomsItemDetailsHistoryMap());

            modelBuilder.Configurations.Add(new CustomsPartnerFtpMap());

            modelBuilder.Configurations.Add(new CustomsPaymentTermMap());

            modelBuilder.Configurations.Add(new CustomsRequestsSheetMap());

            modelBuilder.Configurations.Add(new CustomsRequestsSheetStatusMap());

            modelBuilder.Configurations.Add(new CustomsRequiredFieldMap());

            modelBuilder.Configurations.Add(new CustomsSettingMap());

            modelBuilder.Configurations.Add(new CustomsTransportModeMap());

            modelBuilder.Configurations.Add(new CustomsVendorMap());

            modelBuilder.Configurations.Add(new DangerousGoodsPackingReqMap());

            modelBuilder.Configurations.Add(new DBMigrationMap());

            modelBuilder.Configurations.Add(new DBMigrationLineMap());

            modelBuilder.Configurations.Add(new DebtNotificationTypeMap());

            modelBuilder.Configurations.Add(new DeclarationMap());

            modelBuilder.Configurations.Add(new DeclarationConstraintMap());

            modelBuilder.Configurations.Add(new DeclarationErrorMappingMap());

            modelBuilder.Configurations.Add(new DeclarationPaymentMap());

            modelBuilder.Configurations.Add(new DeclarationPaymentMethodMap());

            modelBuilder.Configurations.Add(new DeclarationPaymentProtestMap());

            modelBuilder.Configurations.Add(new DeclarationPendingMap());

            modelBuilder.Configurations.Add(new DeclarationStatusTypeMap());

            modelBuilder.Configurations.Add(new DeclarationTaxMap());

            modelBuilder.Configurations.Add(new DeficitConnFileParagraphTypeMap());

            modelBuilder.Configurations.Add(new DeficitMap());

            modelBuilder.Configurations.Add(new DeliverySiteTypeMap());

            modelBuilder.Configurations.Add(new DemanderTypeMap());

            modelBuilder.Configurations.Add(new DepositMap());

            modelBuilder.Configurations.Add(new DepositConditionMap());

            modelBuilder.Configurations.Add(new DepositCustomerActivityMap());

            modelBuilder.Configurations.Add(new DepositEssenceTypeMap());

            modelBuilder.Configurations.Add(new DepositFileTypeMap());

            modelBuilder.Configurations.Add(new EntitlementTypeMap());

            modelBuilder.Configurations.Add(new EntityTypeLookupMap());

            modelBuilder.Configurations.Add(new FaultInspectionTypeMap());

            modelBuilder.Configurations.Add(new FuelTypeMap());

            modelBuilder.Configurations.Add(new GenderMap());

            modelBuilder.Configurations.Add(new GovernmentProcedureTypeMap());

            modelBuilder.Configurations.Add(new GuaranteeMap());

            modelBuilder.Configurations.Add(new GuaranteeCertificateTypeMap());

            modelBuilder.Configurations.Add(new GuaranteeConditionMap());

            modelBuilder.Configurations.Add(new ImporterDespositionMap());

            modelBuilder.Configurations.Add(new ImporterPeriodicDeclarStatusMap());

            modelBuilder.Configurations.Add(new ImporterTypeForClaimMap());

            modelBuilder.Configurations.Add(new InterfaceManagementMap());

            modelBuilder.Configurations.Add(new InterfaceSendOptionMap());

            modelBuilder.Configurations.Add(new InterfaceTenantDefinitionMap());

            modelBuilder.Configurations.Add(new InternalBorderSiteTypeMap());

            modelBuilder.Configurations.Add(new InternationalSiteMap());

            modelBuilder.Configurations.Add(new InvoiceTypeMap());

            modelBuilder.Configurations.Add(new CustomsPartnersItemMap());

            modelBuilder.Configurations.Add(new ItemGovernmentProcedureTypeMap());

            modelBuilder.Configurations.Add(new LastReleaseFromWarehouseMap());

            modelBuilder.Configurations.Add(new LeadDocumentExceptionTypeMap());

            modelBuilder.Configurations.Add(new LeadDocumentTypeMap());

            modelBuilder.Configurations.Add(new MeasureQualifierMap());

            modelBuilder.Configurations.Add(new MeasurmentUnitMap());

            modelBuilder.Configurations.Add(new ModificationAndDiscountTypeMap());

            modelBuilder.Configurations.Add(new MorningMessageTypeMap());

            modelBuilder.Configurations.Add(new NotificationMap());

            modelBuilder.Configurations.Add(new NotificationDefinitionMap());

            modelBuilder.Configurations.Add(new NotificationTenantDefinitionMap());

            modelBuilder.Configurations.Add(new NotificationTypeMap());

            modelBuilder.Configurations.Add(new OrganizationUnitTypeMap());

            modelBuilder.Configurations.Add(new PackageMeasureQualifierMap());

            modelBuilder.Configurations.Add(new PackingTypeMap());

            modelBuilder.Configurations.Add(new ParagraphTypeMap());

            modelBuilder.Configurations.Add(new PassportTypeMap());

            modelBuilder.Configurations.Add(new PayerActivityTypeMap());

            modelBuilder.Configurations.Add(new PayerTypeMap());

            modelBuilder.Configurations.Add(new PaymentMethodStatusMap());

            modelBuilder.Configurations.Add(new PaymentMethodTypeMap());

            modelBuilder.Configurations.Add(new PaymentOrderMap());

            modelBuilder.Configurations.Add(new PaymentOrderConnectionTableMap());

            modelBuilder.Configurations.Add(new PaymentOrderLineMap());

            modelBuilder.Configurations.Add(new PaymentOrderMethodMap());

            modelBuilder.Configurations.Add(new PaymentOrderProtestReasonMap());

            modelBuilder.Configurations.Add(new PaymentOrderStatusMap());

            modelBuilder.Configurations.Add(new PaymentOrderTypeMap());

            modelBuilder.Configurations.Add(new PaymentProcessMap());

            modelBuilder.Configurations.Add(new PaymentProtestTypeMap());

            modelBuilder.Configurations.Add(new PaymentTypeMap());

            modelBuilder.Configurations.Add(new PhysicalCheckMap());

            modelBuilder.Configurations.Add(new PhysicalCheckOperationMap());

            modelBuilder.Configurations.Add(new PhysicalCheckStatusMessageMap());

            modelBuilder.Configurations.Add(new ProceduralFaultInProcessTypeMap());

            modelBuilder.Configurations.Add(new ProceduralFaultInSourceTypeMap());

            modelBuilder.Configurations.Add(new ProceduralFaultMap());

            modelBuilder.Configurations.Add(new ProceduralFaultsConnEntityMap());

            modelBuilder.Configurations.Add(new ProceduralFaultStatusMap());

            modelBuilder.Configurations.Add(new ProceduralFaultTypeMap());

            modelBuilder.Configurations.Add(new ProcessingReasonMap());

            modelBuilder.Configurations.Add(new ProductIdentificationTypeMap());

            modelBuilder.Configurations.Add(new ProductNameTypeMap());

            modelBuilder.Configurations.Add(new PropertiesDetailsHistoryMap());

            modelBuilder.Configurations.Add(new RansomViolationTypeMap());

            modelBuilder.Configurations.Add(new RegisteredWarehouseSiteTypeMap());

            modelBuilder.Configurations.Add(new RequestStatusMap());

            modelBuilder.Configurations.Add(new RequiredGuaranteeTypeMap());

            modelBuilder.Configurations.Add(new ReturnConditionMap());

            modelBuilder.Configurations.Add(new SalesTaxExemptionTypeMap());

            modelBuilder.Configurations.Add(new SiteLookupMap());

            modelBuilder.Configurations.Add(new SiteTypeMap());

            modelBuilder.Configurations.Add(new SpecialActionDescriptionTypeMap());

            modelBuilder.Configurations.Add(new SpecializationTypeMap());

            modelBuilder.Configurations.Add(new StorageMessageTypeMap());

            modelBuilder.Configurations.Add(new SubCountryMap());

            modelBuilder.Configurations.Add(new SupplierInvioceItemCertificatMap());

            modelBuilder.Configurations.Add(new SupplierInvoiceMap());

            modelBuilder.Configurations.Add(new SupplierInvoiceFreightAmountMap());

            modelBuilder.Configurations.Add(new SupplierInvoiceItemMap());

            modelBuilder.Configurations.Add(new SupplierInvoiceItemsConDeclarMap());

            modelBuilder.Configurations.Add(new SupplierInvoiceItemsDescriptMap());

            modelBuilder.Configurations.Add(new SupplierInvoiceItemsLevyMap());

            modelBuilder.Configurations.Add(new SupplierInvoiceItemsModMap());

            modelBuilder.Configurations.Add(new SupplierInvoiceItemProcesTypeMap());

            modelBuilder.Configurations.Add(new SupplierInvoiceItemsProdIdentMap());

            modelBuilder.Configurations.Add(new SupplierInvoiceItemsSerialNumMap());

            modelBuilder.Configurations.Add(new SupplierInvoiceItemsTaxMap());

            //modelBuilder.Configurations.Add(new SupplierInvoiceItemsTaxesModMap());

            modelBuilder.Configurations.Add(new SupplierInvoiceModificationMap());

            modelBuilder.Configurations.Add(new TapagMap());

            modelBuilder.Configurations.Add(new TapagConnectionTableMap());

            modelBuilder.Configurations.Add(new TapagTypeMap());

            modelBuilder.Configurations.Add(new TermsOfSaleTypeMap());

            modelBuilder.Configurations.Add(new TradeAgreementMap());

            modelBuilder.Configurations.Add(new TradeLevyExamptTypeMap());

            modelBuilder.Configurations.Add(new UnloadingSiteTypeMap());

            modelBuilder.Configurations.Add(new ValidCustomsItemMap());

            modelBuilder.Configurations.Add(new VehicleMap());

            modelBuilder.Configurations.Add(new VehicleManufacturerMap());

            modelBuilder.Configurations.Add(new VehicleOwnerMap());

            modelBuilder.Configurations.Add(new VehiclePoolTypeMap());

            modelBuilder.Configurations.Add(new VehiclePriceListTypeMap());

            modelBuilder.Configurations.Add(new VehicleSafetyAccessoryMap());

            modelBuilder.Configurations.Add(new VehicleSafeAccessoryInstlTypeMap());

            modelBuilder.Configurations.Add(new VehicleStatusMap());

            modelBuilder.Configurations.Add(new VehicleTecnologyTypeMap());

            modelBuilder.Configurations.Add(new VehicleTypeMap());

            modelBuilder.Configurations.Add(new VendorCommunicationMap());

            modelBuilder.Configurations.Add(new VendorStatusMap());

            modelBuilder.Configurations.Add(new VendorTransactionTypeMap());

            modelBuilder.Configurations.Add(new VendorTypeMap());

            modelBuilder.Configurations.Add(new SupplierInvoiceItemVehicleMap());

            modelBuilder.Configurations.Add(new SignatureTypeMap());
            modelBuilder.Configurations.Add(new CheckEssenceLookupMap());
            modelBuilder.Configurations.Add(new AuthorityMap());
            modelBuilder.Configurations.Add(new VehicleReductionTypeMap());
            modelBuilder.Configurations.Add(new SupplierInvoiceItemVehicleModMap());
            modelBuilder.Configurations.Add(new VehicleSafetyAccessoryTypeMap());
            modelBuilder.Configurations.Add(new GuaranteeCustomerActivityMap());
            modelBuilder.Configurations.Add(new HazardousSubstanceMap());
            modelBuilder.Configurations.Add(new CheckTypeLookupMap());
            modelBuilder.Configurations.Add(new SupplierInvoiceItemModVehicleMap());
            modelBuilder.Configurations.Add(new CertificatesStatusMap());
            modelBuilder.Configurations.Add(new AmendmentRequestStatusMap());
            modelBuilder.Configurations.Add(new AmendmentStatusMap());
            modelBuilder.Configurations.Add(new AmendmentTypeMap());
            modelBuilder.Configurations.Add(new AmendmentFieldReasonTypeMap());
            modelBuilder.Configurations.Add(new DeclarationStatementTypeMap());
            modelBuilder.Configurations.Add(new SupplierInvoiceItemVehicleAddMap());
            modelBuilder.Configurations.Add(new VendorCommissionMap());
            modelBuilder.Configurations.Add(new NotificationReplyMap());
            modelBuilder.Configurations.Add(new AccumalationStateMap());
            modelBuilder.Configurations.Add(new StorageStatusMap());
            modelBuilder.Configurations.Add(new CourierStatusMap());
            modelBuilder.Configurations.Add(new DeclarationCourierStatusMap());
            modelBuilder.Configurations.Add(new CourierPendingReasonMap());
            modelBuilder.Configurations.Add(new CargoStatusMap());
            modelBuilder.Configurations.Add(new MAWBTypeMap());
            modelBuilder.Configurations.Add(new DeclarationConsAcceptanceMap());
            modelBuilder.Configurations.Add(new ManifestCargoStatusMap());
            modelBuilder.Configurations.Add(new CourierManifestStatusMap());
            modelBuilder.Configurations.Add(new CourierDeclarationStatusMap());
            modelBuilder.Configurations.Add(new CourierPaymentStatusMap());
            modelBuilder.Configurations.Add(new ActionCodeMap());
            modelBuilder.Configurations.Add(new SplitOrMergeReasonMap());
            modelBuilder.Configurations.Add(new CargoSplitRequestStatusMap());
            modelBuilder.Configurations.Add(new DeclarationCargoSplitMap());
            modelBuilder.Configurations.Add(new DecCargoSplitConMap());
            modelBuilder.Configurations.Add(new TreatmentWayMap());
            modelBuilder.Configurations.Add(new DecCargoSplitConsItemMap());
            modelBuilder.Configurations.Add(new DecCargoSplitConsPackDetMap());
            modelBuilder.Configurations.Add(new DecDangersContactMap());
            modelBuilder.Configurations.Add(new TPGFileTypeMap());
            modelBuilder.Configurations.Add(new DecCargoSplitCargoIdentifierMap());
            modelBuilder.Configurations.Add(new PendingErrorPlaceMap());
            modelBuilder.Configurations.Add(new DecisionTypeMap());
            modelBuilder.Configurations.Add(new SeizureMethodTypeMap());
            modelBuilder.Configurations.Add(new ClaimsRelatedEntitiesSeizureMap());
            modelBuilder.Configurations.Add(new ClaimsRelatedEntitiesRefundMap());
            modelBuilder.Configurations.Add(new SeizureFactorTypeMap());
            modelBuilder.Configurations.Add(new MamanSpecialActionMap());
            modelBuilder.Configurations.Add(new MamanSpecialActionStatusMap());
            modelBuilder.Configurations.Add(new DeclarationMamanSpecialActionMap());
            modelBuilder.Configurations.Add(new RefundCustomerActivityTypeMap());
            modelBuilder.Configurations.Add(new TransferCargoMethodTypeMap());
            modelBuilder.Configurations.Add(new GatepassReturnCodeMap());
            modelBuilder.Configurations.Add(new UpdateCodeMap());
            modelBuilder.Configurations.Add(new GatepassRequestMap());
            modelBuilder.Configurations.Add(new PendingByKeywordMap());
            modelBuilder.Configurations.Add(new ContinuousRequestTypeMap());
            modelBuilder.Configurations.Add(new RequestTypeMap());
            modelBuilder.Configurations.Add(new DeficitDecisionMap());
            modelBuilder.Configurations.Add(new SealCompletenesMap());
            modelBuilder.Configurations.Add(new SealTypeMap());

            #endregion

            #region Accounting
            modelBuilder.Configurations.Add(new JournalActionTypeMap());
            modelBuilder.Configurations.Add(new AccountingEntityMap());
            modelBuilder.Configurations.Add(new AccountingPeriodMap());
            modelBuilder.Configurations.Add(new AutomaticReconcileMap());
            modelBuilder.Configurations.Add(new AutomaticReconcileMethodMap());
            modelBuilder.Configurations.Add(new BankCodeMap());
            modelBuilder.Configurations.Add(new BankAccountMap());

            modelBuilder.Entity<ClaimImporterDeclarsPage3B>().Property(x => x.SaleAmountAfter).HasPrecision(16, 2);

            modelBuilder.Entity<ClaimImporterDeclarsPage3B>().Property(x => x.SaleAmountClaim).HasPrecision(16, 2);

            modelBuilder.Entity<ClaimImporterDeclarsPage3B>().Property(x => x.SaleAmountBefore).HasPrecision(16, 2);
            modelBuilder.Configurations.Add(new ChartOfAccountMap());
            modelBuilder.Configurations.Add(new ChartOfAccountsTypeMap());
            modelBuilder.Configurations.Add(new FullAccountingSettingMap());
            modelBuilder.Configurations.Add(new GLAccountMap());
            //    modelBuilder.Entity<GLAccount>().HasOptional(x => x.ParentAccount).WithMany().HasForeignKey(x => x.ParentAccountId);
            //   modelBuilder.Entity<GLAccount>().HasOptional(x => x.CustomerGLAccount).WithMany().HasForeignKey(x => x.CustomerGLAccountId);

            modelBuilder.Configurations.Add(new GLAccountTotalByMonthMap());
            modelBuilder.Configurations.Add(new GLAccountTotalDateTypeMap());
            modelBuilder.Configurations.Add(new GLAccountCurrencyMap());

            modelBuilder.Configurations.Add(new GLAccountTypeMap());
            modelBuilder.Configurations.Add(new JournalMap());
            modelBuilder.Configurations.Add(new JournalTypeMap());
            modelBuilder.Configurations.Add(new JournalStatusTypeMap());
            modelBuilder.Configurations.Add(new JournalLineMap());

            modelBuilder.Configurations.Add(new LedgerTransactionMap());
            modelBuilder.Configurations.Add(new PeriodTypeMap());
            modelBuilder.Configurations.Add(new ReconcileMethodMap());
            modelBuilder.Configurations.Add(new RevenueExpenseTypeMap());
            modelBuilder.Configurations.Add(new TestEntityMap());

            modelBuilder.Configurations.Add(new ReconciliationMap());
            modelBuilder.Configurations.Add(new ReconciliationLineMap());
            modelBuilder.Configurations.Add(new CashBookTypeMap());
            modelBuilder.Configurations.Add(new CashBookMap());
            modelBuilder.Configurations.Add(new ARPaymentChequeMap());
            modelBuilder.Configurations.Add(new CashBookLineMap());
            modelBuilder.Configurations.Add(new BankDepositMap());
            modelBuilder.Configurations.Add(new BankDepositLineMap());
            modelBuilder.Configurations.Add(new RevaluationMap());



            modelBuilder.Configurations.Add(new Category1Map());
            modelBuilder.Configurations.Add(new Category2Map());
            modelBuilder.Configurations.Add(new Category3Map());
            modelBuilder.Configurations.Add(new Category4Map());
            modelBuilder.Configurations.Add(new Category5Map());

            modelBuilder.Configurations.Add(new ARPaymentChequeStatusMap());
            modelBuilder.Configurations.Add(new ReconcileExternalPageMap());
            modelBuilder.Configurations.Add(new ReconcileExternalPageLineMap());
            modelBuilder.Configurations.Add(new ReconcileExternalPageStatusMap());

            #endregion

            #region WareHouse		
            modelBuilder.Configurations.Add(new WarehouseEntryMap());
            modelBuilder.Configurations.Add(new WarehouseReleaseMap());
            modelBuilder.Configurations.Add(new WarehouseEntryPackageMap());
            modelBuilder.Configurations.Add(new WarehouseReleasePackageMap());
            modelBuilder.Configurations.Add(new WarehouseEntryPackagesReleaseMap());
            modelBuilder.Configurations.Add(new WarehouseEntryStatusMap());
            modelBuilder.Configurations.Add(new WarehouseReleaseStatusMap());
            #endregion

            #region Time Management		
            modelBuilder.Configurations.Add(new TMEmployeeTimeMap());
            modelBuilder.Configurations.Add(new TMLocationMap());
            modelBuilder.Configurations.Add(new TMProjectMap());
            #endregion
            modelBuilder.Configurations.Add(new SharedUserQueryMap());
            //modelBuilder.Configurations.Add(new DWHSettingMap());
            modelBuilder.Configurations.Add(new CustomsShipperMap());
            modelBuilder.Configurations.Add(new DocumentFilingBackupSettingMap());
            modelBuilder.Configurations.Add(new CustomerFieldsUpdateSettingMap());
            modelBuilder.Configurations.Add(new BlobFileMap());

            modelBuilder.Entity<ClientAddress>().Property(x => x.LocalApartment).HasPrecision(4, 0);

            modelBuilder.Entity<DeclarationPaymentMethod>().Property(x => x.Amount).HasPrecision(16, 2);

            modelBuilder.Entity<DeclarationPaymentProtest>().Property(x => x.GoodsItemLineNumber).HasPrecision(16, 5);

            modelBuilder.Entity<DeclarationPaymentProtest>().Property(x => x.AmountInDispute).HasPrecision(16, 2);

            modelBuilder.Entity<DeficitConnFileParagraphType>().Property(x => x.Amount).HasPrecision(16, 2);

            modelBuilder.Entity<PaymentOrderProtestReason>().Property(x => x.GoodsItemLineNumber).HasPrecision(5, 0);

            modelBuilder.Entity<PaymentOrderProtestReason>().Property(x => x.AmountInDispute).HasPrecision(16, 2);

            modelBuilder.Entity<SupplierInvoice>().Property(x => x.ExchangeRate).HasPrecision(12, 10);

            modelBuilder.Entity<SupplierInvoice>().Property(x => x.InsuranceAmount).HasPrecision(16, 2);

            modelBuilder.Entity<SupplierInvoiceFreightAmount>().Property(x => x.Amount).HasPrecision(16, 2);

            modelBuilder.Entity<SupplierInvoiceItem>().Property(x => x.InvoiceQuantity).HasPrecision(14, 3);

            modelBuilder.Entity<SupplierInvoiceItem>().Property(x => x.StatisticQuantity).HasPrecision(14, 3);

            modelBuilder.Entity<SupplierInvoiceItem>().Property(x => x.AdditionalQuantity).HasPrecision(14, 3);

            modelBuilder.Entity<Declaration>().Property(x => x.DealValue).HasPrecision(16, 2);

            modelBuilder.Entity<Declaration>().Property(x => x.DealValueWithoutFactor).HasPrecision(16, 2);

            modelBuilder.Entity<Declaration>().Property(x => x.CIFValue).HasPrecision(16, 2);

            modelBuilder.Entity<Declaration>().Property(x => x.TotalTax).HasPrecision(16, 2);

            modelBuilder.Entity<SupplierInvoice>().Property(x => x.InvoiceAmount).HasPrecision(16, 2);

            modelBuilder.Entity<SupplierInvoice>().Property(x => x.ActualPayedAmount).HasPrecision(16, 2);

            modelBuilder.Entity<SupplierInvoice>().Property(x => x.TotalFreightInFreightCurrency).HasPrecision(16, 2);

            modelBuilder.Entity<SupplierInvoice>().Property(x => x.TotalFreightInNIS).HasPrecision(16, 2);

            modelBuilder.Entity<SupplierInvoice>().Property(x => x.InsuranceAmount).HasPrecision(16, 2);

            modelBuilder.Entity<SupplierInvoiceModification>().Property(x => x.Amount).HasPrecision(16, 2);

            modelBuilder.Entity<SupplierInvoiceFreightAmount>().Property(x => x.Amount).HasPrecision(16, 2);

            modelBuilder.Entity<SupplierInvoiceItem>().Property(x => x.ItemPrice).HasPrecision(16, 2);

            modelBuilder.Entity<SupplierInvoiceItem>().Property(x => x.NonCustomsItemPrice).HasPrecision(16, 2);

            modelBuilder.Entity<SupplierInvoiceItem>().Property(x => x.WholeSaleItemPrice).HasPrecision(16, 2);

            modelBuilder.Entity<SupplierInvoiceItemsMod>().Property(x => x.Amount).HasPrecision(16, 2);

            modelBuilder.Entity<DeclarationTax>().Property(x => x.TotalAmount).HasPrecision(16, 2);

            modelBuilder.Entity<DeclarationTax>().Property(x => x.DeferredTaxAmount).HasPrecision(16, 2);
            modelBuilder.Entity<DeclarationTax>().Property(x => x.TaxBaseAmount).HasPrecision(16, 2);

            modelBuilder.Entity<SupplierInvoiceItemsTax>().Property(x => x.TaxBaseAmount).HasPrecision(16, 2);

            modelBuilder.Entity<SupplierInvoiceItemsTax>().Property(x => x.TaxAmount).HasPrecision(16, 2);

            modelBuilder.Entity<SupplierInvoiceItemsTax>().Property(x => x.DeferedTaxAmount).HasPrecision(16, 2);
            modelBuilder.Entity<SupplierInvoiceItemsTax>().Property(x => x.TaxRate).HasPrecision(17, 2);

            //modelBuilder.Entity<SupplierInvoiceItemsTaxesMod>().Property(x => x.Amount).HasPrecision(16, 2);

            //modelBuilder.Entity<CustomsExchangeRate>().Property(x => x.ExchangeRate).HasPrecision(12, 10);

            modelBuilder.Entity<Deposit>().Property(x => x.DepositAmount).HasPrecision(16, 2);

            modelBuilder.Entity<DepositCondition>().Property(x => x.DepositAmount).HasPrecision(16, 2);

            Configuration.AutoDetectChangesEnabled = true;
            modelBuilder.Entity<Opportunity>().Property(x => x.ValueField).HasPrecision(18, 2);

            modelBuilder.Entity<OpportunityProduct>().Property(x => x.ChargeableWeight).HasPrecision(18, 2);
            modelBuilder.Entity<OpportunityProduct>().Property(x => x.TEU).HasPrecision(18, 2);
            modelBuilder.Entity<OpportunityProduct>().Property(x => x.Revenue).HasPrecision(18, 2);

            modelBuilder.Entity<OpportunityProductLocation>().Property(x => x.ChargeableWeight).HasPrecision(18, 2);
            modelBuilder.Entity<OpportunityProductLocation>().Property(x => x.TEU).HasPrecision(18, 2);
            modelBuilder.Entity<OpportunityProductLocation>().Property(x => x.Revenue).HasPrecision(18, 2);


            modelBuilder.Entity<RequiredGuaranteeType>().Property(x => x.GuaranteeAmount).HasPrecision(16, 2);
            modelBuilder.Entity<GuaranteeCondition>().Property(x => x.GuaranteeAmount).HasPrecision(16, 2);

            modelBuilder.Entity<SupplierInvoice>().Property(x => x.InsruancePercentage).HasPrecision(7, 4);
            modelBuilder.Entity<DBMigration>().Property(x => x.MajorVersion).HasPrecision(5, 2);

            modelBuilder.Entity<Declaration>().Property(x => x.LoadingFactor).HasPrecision(18, 10);

            modelBuilder.Entity<ProceduralFault>().Property(x => x.RansomViolationSum).HasPrecision(16, 2);
            modelBuilder.Entity<PaymentOrderMethod>().Property(x => x.Amount).HasPrecision(16, 2);

            modelBuilder.Entity<Vehicle>().Property(x => x.GreenIndex).HasPrecision(9, 3);
            modelBuilder.Entity<Vehicle>().Property(x => x.VehicleSafetyAccessoryPoints).HasPrecision(4, 2);



            modelBuilder.Entity<ProceduralFault>().Property(x => x.RansomViolationSum).HasPrecision(16, 2);
            modelBuilder.Entity<PaymentOrderMethod>().Property(x => x.Amount).HasPrecision(16, 2);
            modelBuilder.Entity<SupplierInvoiceItemVehicleMod>().Property(x => x.DeductAmount).HasPrecision(16, 2);
            modelBuilder.Entity<SupplierInvoiceItemModVehicle>().Property(x => x.DeductAmount).HasPrecision(16, 2);
            modelBuilder.Entity<SupplierInvoiceItemVehicleAdd>().Property(x => x.VehicleValue).HasPrecision(16, 2);
            modelBuilder.Entity<SupplierInvoiceItemVehicleAdd>().Property(x => x.ChassisTax).HasPrecision(16, 2);
            modelBuilder.Entity<SupplierInvoiceItemVehicleAdd>().Property(x => x.ChassisPurchaseTax).HasPrecision(16, 2);
            modelBuilder.Entity<SupplierInvoiceItemVehicleAdd>().Property(x => x.ChassisVat).HasPrecision(16, 2);


            //modelBuilder.Entity<SupplierInvoiceItemsConDeclar>().Property(x => x.Quantity).HasPrecision(14, 3);

            //Commesioin percentage
            modelBuilder.Entity<SupplierInvoice>().Property(x => x.VendorComissionPercentage).HasPrecision(7, 4);
            modelBuilder.Entity<VendorCommission>().Property(x => x.CommisionPercentage).HasPrecision(7, 4);

            modelBuilder.Configurations.Add(new AccountingSystemMap());
            modelBuilder.Configurations.Add(new AccountingSettingMap());
            modelBuilder.Configurations.Add(new Accounts1Map());
            modelBuilder.Configurations.Add(new AccountTypeMap());
            modelBuilder.Configurations.Add(new AddressMap());
            modelBuilder.Configurations.Add(new QuoteTemplateTextCodeMap());
            modelBuilder.Configurations.Add(new AddressTypeMap());
            modelBuilder.Configurations.Add(new AdvancedQueryFilterMap());
            modelBuilder.Configurations.Add(new AgentMap());
            modelBuilder.Configurations.Add(new AirlineMap());
            modelBuilder.Configurations.Add(new APInvoiceEntityMap());
            modelBuilder.Configurations.Add(new APInvoiceLineMap());
            modelBuilder.Configurations.Add(new APInvoicePaymentMap());
            modelBuilder.Configurations.Add(new APInvoiceMap());
            modelBuilder.Configurations.Add(new APInvoiceStatuMap());
            modelBuilder.Configurations.Add(new APInvoiceTotalVATMap());
            modelBuilder.Configurations.Add(new APInvoiceTypeMap());
            modelBuilder.Configurations.Add(new APPaymentMethodMap());
            modelBuilder.Configurations.Add(new APPaymentMap());
            modelBuilder.Configurations.Add(new APPaymentStatuMap());
            modelBuilder.Configurations.Add(new ARInvoiceEntityMap());
            modelBuilder.Configurations.Add(new ARInvoiceLineMap());
            modelBuilder.Configurations.Add(new ARInvoicePaymentMap());
            modelBuilder.Configurations.Add(new ARInvoiceMap());
            modelBuilder.Configurations.Add(new ARInvoiceTransferStatusMap());
            modelBuilder.Configurations.Add(new APInvoiceTransferStatusMap());
            modelBuilder.Configurations.Add(new ARPaymentTransferStatusMap());
            modelBuilder.Configurations.Add(new APPaymentTransferStatusMap());
            modelBuilder.Configurations.Add(new QuoteDocumentVersionMap());
            modelBuilder.Configurations.Add(new ARInvoiceStatuMap());
            modelBuilder.Configurations.Add(new ARInvoiceTotalVATMap());
            modelBuilder.Configurations.Add(new ARInvoiceTypeMap());
            modelBuilder.Configurations.Add(new AccountingPaymentMethodMap());
            modelBuilder.Configurations.Add(new ARPaymentMap());
            modelBuilder.Configurations.Add(new ARPaymentStatuMap());
            modelBuilder.Configurations.Add(new ARInvoiceStocksStatusMap());
            modelBuilder.Configurations.Add(new ARInvoiceLineActionMap());
            modelBuilder.Configurations.Add(new AWBChargesCodeMap());
            modelBuilder.Configurations.Add(new AWBSpecialHandlingCodeMap());
            modelBuilder.Configurations.Add(new EmailAlertSettingMap());
            modelBuilder.Configurations.Add(new AWBStatuMap());
            modelBuilder.Configurations.Add(new ExternalSystemsTablesCodeMap());
            modelBuilder.Configurations.Add(new ExternalSystemsMissingTranslationMap());
            modelBuilder.Configurations.Add(new ExternalSystemsSyncStatusMap());
            modelBuilder.Configurations.Add(new AccountingSystemsSettingMap());
            modelBuilder.Configurations.Add(new AccountingSystemsSyncStatusMap());
            modelBuilder.Configurations.Add(new QuickbooksSyncRequestTicketMap());
            modelBuilder.Configurations.Add(new CardExternalCodeByCurrencyMap());
            modelBuilder.Configurations.Add(new BranchMap());
            modelBuilder.Configurations.Add(new CardContactMap());
            modelBuilder.Configurations.Add(new CardMap());
            modelBuilder.Configurations.Add(new CategoryTypeMap());
            modelBuilder.Configurations.Add(new ChargesGroupMap());
            modelBuilder.Configurations.Add(new ChargesTypeMap());
            modelBuilder.Configurations.Add(new CommunicationAttachmentMap());
            modelBuilder.Configurations.Add(new CommunicationLogMap());
            modelBuilder.Configurations.Add(new CommunicationLogTypeMap());
            modelBuilder.Configurations.Add(new WarehouseTypeMap());
            modelBuilder.Configurations.Add(new CommunicationStatusTypeMap());
            modelBuilder.Configurations.Add(new ContactMap());
            modelBuilder.Configurations.Add(new ContactTenantRoleSetMap());
            modelBuilder.Configurations.Add(new ContactTenantMap());
            modelBuilder.Configurations.Add(new CounterDefinitionMap());
            modelBuilder.Configurations.Add(new CounterLastNumberMap());
            modelBuilder.Configurations.Add(new CounterMap());
            modelBuilder.Configurations.Add(new CounterStatMap());
            modelBuilder.Configurations.Add(new CountryMap());
            modelBuilder.Configurations.Add(new CreditCardTypeMap());
            modelBuilder.Configurations.Add(new CurrencyMap());
            modelBuilder.Configurations.Add(new CustomAgentMap());
            modelBuilder.Configurations.Add(new CustomerMap());
            
            modelBuilder.Configurations.Add(new CustomPickListMap());
            modelBuilder.Configurations.Add(new CustomTableMap());
            modelBuilder.Configurations.Add(new DataBasePropertyMap());
            modelBuilder.Configurations.Add(new DBIdCounterMap());
            modelBuilder.Configurations.Add(new DepartmentMap());
            modelBuilder.Configurations.Add(new DescriptionOfGoodMap());
            modelBuilder.Configurations.Add(new DimensionsUnitMap());
            modelBuilder.Configurations.Add(new DirectionMap());
            modelBuilder.Configurations.Add(new ReportMap());
            modelBuilder.Configurations.Add(new CustomerSalesmanByProductMap());
            modelBuilder.Configurations.Add(new CustomerAccountManagerByProductMap());
            modelBuilder.Configurations.Add(new CustomerFreelancerByProductMap());
            modelBuilder.Configurations.Add(new CustomerCustomsAgentByProductMap());
            modelBuilder.Configurations.Add(new CustomerForwarderByProductMap());
            modelBuilder.Configurations.Add(new CustomerMediatorByProductMap());
            modelBuilder.Configurations.Add(new DocumentOutCopyMap());
            modelBuilder.Configurations.Add(new DocumentOutMap());
            modelBuilder.Configurations.Add(new DocumentMap());
            modelBuilder.Configurations.Add(new DocumentTypeCopyMap());
            modelBuilder.Configurations.Add(new DocumentTypeCustomFields1Map());
            modelBuilder.Configurations.Add(new DocumentTypeMap());
            modelBuilder.Configurations.Add(new DocumentTypeTemplateMap());
            modelBuilder.Configurations.Add(new DueTypeMap());
            modelBuilder.Configurations.Add(new DocumentsDataProviderMap());
            modelBuilder.Configurations.Add(new EntityDateMap());
            modelBuilder.Configurations.Add(new EntityLastActivityMap());
            modelBuilder.Configurations.Add(new EntityLastActivityTypeMap());
            modelBuilder.Configurations.Add(new EntityLastUpdateMap());
            modelBuilder.Configurations.Add(new EntityStatuMap());
            modelBuilder.Configurations.Add(new EventTypeMap());
            modelBuilder.Configurations.Add(new FeatureMap());
            modelBuilder.Configurations.Add(new FeatureTypeMap());
            modelBuilder.Configurations.Add(new FHLStatuMap());
            modelBuilder.Configurations.Add(new FieldDataTypeMap());
            modelBuilder.Configurations.Add(new FollowUpMap());
            modelBuilder.Configurations.Add(new FormCustomFields1Map());
            modelBuilder.Configurations.Add(new FWBStatuMap());
            modelBuilder.Configurations.Add(new CustomsTransmissionsStatusMap());
            modelBuilder.Configurations.Add(new GlobalZoneMap());
            modelBuilder.Configurations.Add(new IATACodeMap());
            modelBuilder.Configurations.Add(new ImageDetailMap());
            modelBuilder.Configurations.Add(new ImageLibraryMap());
            modelBuilder.Configurations.Add(new IncotermMap());
            modelBuilder.Configurations.Add(new InsideShipmentPackageMap());
            modelBuilder.Configurations.Add(new MarkUpTypeMap());
            modelBuilder.Configurations.Add(new MAWBStackMap());
            modelBuilder.Configurations.Add(new MeasurementMap());
            modelBuilder.Configurations.Add(new MenuButtonGroupMap());
            modelBuilder.Configurations.Add(new MenuButtonMap());
            modelBuilder.Configurations.Add(new MenusTableMap());
            modelBuilder.Configurations.Add(new MenuTypeMap());
            modelBuilder.Configurations.Add(new MoveTypeMap());
            modelBuilder.Configurations.Add(new NextLegMap());
            modelBuilder.Configurations.Add(new ObjectFieldModificationMap());
            modelBuilder.Configurations.Add(new ObjectFieldMap());
            modelBuilder.Configurations.Add(new ObjectFieldValidationMap());
            modelBuilder.Configurations.Add(new ObjectTableHelperControlMap());
            modelBuilder.Configurations.Add(new ObjectTableRuleFieldMap());
            modelBuilder.Configurations.Add(new ObjectTableRuleMap());
            modelBuilder.Configurations.Add(new ObjectTableMap());
            modelBuilder.Configurations.Add(new ObjectTableTabMap());
            modelBuilder.Configurations.Add(new ObjectTableTypeMap());
            modelBuilder.Configurations.Add(new PackageFeatureMap());
            modelBuilder.Configurations.Add(new PackageMap());
            modelBuilder.Configurations.Add(new PackageTypeMap());
            modelBuilder.Configurations.Add(new PartnerTypeMap());
            modelBuilder.Configurations.Add(new PasswordPolicyMap());
            modelBuilder.Configurations.Add(new ProductTypeModificationMap());
            modelBuilder.Configurations.Add(new PaymentTermMap());
            modelBuilder.Configurations.Add(new PermissionTypeMap());
            modelBuilder.Configurations.Add(new PickUpDeliveryFromToTypeMap());
            modelBuilder.Configurations.Add(new PickUpDeliveryTypeMap());
            modelBuilder.Configurations.Add(new PortMap());
            modelBuilder.Configurations.Add(new PrepaidCollectMap());
            modelBuilder.Configurations.Add(new QueryMap());
            modelBuilder.Configurations.Add(new QueryColumnMap());
            modelBuilder.Configurations.Add(new QueryGroupMap());
            modelBuilder.Configurations.Add(new QuoteChargeMap());
            modelBuilder.Configurations.Add(new QuoteCustomerTypeMap());
            modelBuilder.Configurations.Add(new QuotePriceStepMap());
            modelBuilder.Configurations.Add(new QuoteMap());
            modelBuilder.Configurations.Add(new QuoteTypeMap());
            modelBuilder.Configurations.Add(new RankMap());
            modelBuilder.Configurations.Add(new RateClassMap());
            modelBuilder.Configurations.Add(new RatesTableMap());
            modelBuilder.Configurations.Add(new RestrictionMap());
            modelBuilder.Configurations.Add(new RoleFeatureMap());
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new RoleTypeMap());
            modelBuilder.Configurations.Add(new RuleConditionFieldMap());
            modelBuilder.Configurations.Add(new RuleNotificationTypeMap());
            modelBuilder.Configurations.Add(new RuleTypeMap());
            modelBuilder.Configurations.Add(new ScreenFieldMap());
            modelBuilder.Configurations.Add(new ScreenModificationMap());
            modelBuilder.Configurations.Add(new ScreenMap());
            modelBuilder.Configurations.Add(new SharedLogisticsUpdateMap());
            modelBuilder.Configurations.Add(new SharedLogisticsUpdateStatuMap());
            modelBuilder.Configurations.Add(new ShipmentAWBPrintOnlyMap());
            modelBuilder.Configurations.Add(new ShipmentCarrierStatusMap());
            modelBuilder.Configurations.Add(new ShipmentCustomerTypeMap());
            modelBuilder.Configurations.Add(new ShipmentLevelMap());
            modelBuilder.Configurations.Add(new ShipmentMasterDataMap());
            modelBuilder.Configurations.Add(new ShipmentOrderPackageMap());
            modelBuilder.Configurations.Add(new ShipmentPackageMap());
            modelBuilder.Configurations.Add(new ShipmentPayableAmountTypeMap());
            modelBuilder.Configurations.Add(new ShipmentPayableLineStatuMap());
            modelBuilder.Configurations.Add(new ShipmentPayableMap());
            modelBuilder.Configurations.Add(new ShipmentAssemblyMap());
            modelBuilder.Configurations.Add(new ShipmentPayableStatuMap());
            modelBuilder.Configurations.Add(new ShipmentPickUpDeliveryMap());
            modelBuilder.Configurations.Add(new ShipmentPickUpDeliveryPackageMap());
            modelBuilder.Configurations.Add(new ShipmentReceivableLineStatuMap());
            modelBuilder.Configurations.Add(new ShipmentReceivableMap());
            modelBuilder.Configurations.Add(new ShipmentReceivableStatuMap());
            modelBuilder.Configurations.Add(new ShipmentMap());
            modelBuilder.Configurations.Add(new ShipmentTypeMap());
            modelBuilder.Configurations.Add(new ShippingAgentMap());
            modelBuilder.Configurations.Add(new ShippingLineMap());
            modelBuilder.Configurations.Add(new SpecialServiceMap());
            modelBuilder.Configurations.Add(new StateMap());
            modelBuilder.Configurations.Add(new TarrifChargeMap());
            modelBuilder.Configurations.Add(new TarrifFromToMap());
            modelBuilder.Configurations.Add(new TarrifFromToTypeMap());
            modelBuilder.Configurations.Add(new TarrifHeaderMap());
            modelBuilder.Configurations.Add(new TarrifStepMap());
            modelBuilder.Configurations.Add(new TarrifTypeMap());
            modelBuilder.Configurations.Add(new TariffSettingMap());
            modelBuilder.Configurations.Add(new TemplateFormatMap());
            modelBuilder.Configurations.Add(new TenantMap());
            modelBuilder.Configurations.Add(new TenantSettingMap());
            modelBuilder.Configurations.Add(new TermsofUsMap());
            modelBuilder.Configurations.Add(new TermsofUseSignatureMap());
            modelBuilder.Configurations.Add(new TextCodeMap());
            modelBuilder.Configurations.Add(new TextCodeTypeMap());
            modelBuilder.Configurations.Add(new TipMap());
            modelBuilder.Configurations.Add(new TipsVisibilityMap());
            modelBuilder.Configurations.Add(new TraceEventMap());
            modelBuilder.Configurations.Add(new TranslationHeaderMap());
            modelBuilder.Configurations.Add(new TranslationMap());
            modelBuilder.Configurations.Add(new TransportModeMap());
            modelBuilder.Configurations.Add(new TriggerTypeMap());
            modelBuilder.Configurations.Add(new TruckerMap());
            modelBuilder.Configurations.Add(new UserLastLoginMap());
            modelBuilder.Configurations.Add(new UserLoginLogMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new ValidationTypeMap());
            modelBuilder.Configurations.Add(new VatTypePercentageMap());
            modelBuilder.Configurations.Add(new VatTypeMap());
            modelBuilder.Configurations.Add(new VendorMap());
            modelBuilder.Configurations.Add(new VesselMap());
            modelBuilder.Configurations.Add(new VolumeUnitMap());
            modelBuilder.Configurations.Add(new WarehouseMap());
            modelBuilder.Configurations.Add(new WeightUnitMap());
            modelBuilder.Configurations.Add(new ContactLastLoginMap());
            modelBuilder.Configurations.Add(new SmallDocumentMap());
            modelBuilder.Configurations.Add(new CommunicationLogStepMap());
            modelBuilder.Configurations.Add(new ShipmentPackageItemMap());
            modelBuilder.Configurations.Add(new SharedLogisticsInvitationStatusMap());
            modelBuilder.Configurations.Add(new IndustryMap());
            modelBuilder.Configurations.Add(new LeadSourceMap());
            modelBuilder.Configurations.Add(new ProductPeriodMap());
            modelBuilder.Configurations.Add(new ProductTypeMap());
            modelBuilder.Configurations.Add(new CustomerProductMap());
            modelBuilder.Configurations.Add(new CustomerProductActualDataMap());
            modelBuilder.Configurations.Add(new CustomerProductLocationMap());
            modelBuilder.Configurations.Add(new CustomerProductLocationActualDataMap());
            modelBuilder.Configurations.Add(new CompetitorMap());
            modelBuilder.Configurations.Add(new CustomerCompetitorMap());
            modelBuilder.Configurations.Add(new CustomerCompetitorProductMap());
            modelBuilder.Configurations.Add(new CustomerAdditionalServiceMap());
            modelBuilder.Configurations.Add(new CommodityMap());
            modelBuilder.Configurations.Add(new ContactDoneMethodMap());
            modelBuilder.Configurations.Add(new AdditionalServiceMap());
            modelBuilder.Configurations.Add(new QuotePackageMap());
            modelBuilder.Configurations.Add(new QuoteTemplateSettingMap());
            modelBuilder.Configurations.Add(new QuoteTemplateMap());
            modelBuilder.Configurations.Add(new QuoteTemplateSectionTypeMap());
            modelBuilder.Configurations.Add(new QuoteTemplateSectionModificationMap());
            modelBuilder.Configurations.Add(new QuoteTemplateExcludedSectionMap());
            modelBuilder.Configurations.Add(new QuoteTemplateSectionMap());
            modelBuilder.Configurations.Add(new VatUniqueTypeMap());
            modelBuilder.Configurations.Add(new VatFormatTypeMap());
            modelBuilder.Configurations.Add(new VatMandatoryTypeMap());
            modelBuilder.Configurations.Add(new CustomerSalesNoteMap());
            modelBuilder.Configurations.Add(new QuoteTemplateDetailsFieldMap());
            modelBuilder.Configurations.Add(new QuoteTemplateHeaderFieldMap());
            modelBuilder.Configurations.Add(new ShipmentComputedFieldsMap());
            modelBuilder.Configurations.Add(new AuthenticationTokenMap());
            modelBuilder.Configurations.Add(new CustomerStatusMap());
            modelBuilder.Configurations.Add(new ShipmentCommodityMap());
            modelBuilder.Configurations.Add(new QuoteTemplateTableDesignMap());
            modelBuilder.Configurations.Add(new QuoteTemplateTextDesignMap());
            modelBuilder.Configurations.Add(new BorderTypeMap());
            modelBuilder.Configurations.Add(new BusinessUnitMap());
            modelBuilder.Configurations.Add(new FeatureAccessLevelMap());
            modelBuilder.Configurations.Add(new SpecialServicesTypeMap());
            modelBuilder.Configurations.Add(new EmailProviderMap());
            modelBuilder.Configurations.Add(new RegionMap());
            modelBuilder.Configurations.Add(new MessagingStockMap());
            modelBuilder.Configurations.Add(new MessagingStockUsageHistoryMap());
            modelBuilder.Configurations.Add(new QuoteStageMap());
            modelBuilder.Configurations.Add(new QuoteRatingMap());
            modelBuilder.Configurations.Add(new CountryCityMap());
            modelBuilder.Configurations.Add(new ReportModificationMap());
            modelBuilder.Configurations.Add(new ContactsUnseenEntitieMap());
            modelBuilder.Configurations.Add(new DistributorMap());
            modelBuilder.Configurations.Add(new AccountingInformationIdentifierMap());
            modelBuilder.Configurations.Add(new SharedFollowedShipmentMap());
            modelBuilder.Configurations.Add(new DocumentsFilingMap());
            modelBuilder.Configurations.Add(new DocumentsFilingMetaDataValueMap());
            modelBuilder.Configurations.Add(new CustomMetaDataTypesAddtionalMap());
            modelBuilder.Configurations.Add(new DocumentsMetaDataTypeMap());
            modelBuilder.Configurations.Add(new DocumentTypeMetaDataMap());
            modelBuilder.Configurations.Add(new ComputingPartnerMap());
            modelBuilder.Configurations.Add(new ComputingPartnerCodeMap());
            modelBuilder.Configurations.Add(new ComputingPartnerTableMap());
            modelBuilder.Configurations.Add(new ComputingPartnerTranslationMap());
            modelBuilder.Configurations.Add(new DataProviderMap());
            modelBuilder.Configurations.Add(new DocumentFolderMap());
            modelBuilder.Configurations.Add(new ColorIndexMap());
            modelBuilder.Configurations.Add(new ManifestStatusMap());
            modelBuilder.Configurations.Add(new AWBAdditionalHandlingInfoMap());
            modelBuilder.Configurations.Add(new DocumentTypeCategoryMap());
            modelBuilder.Configurations.Add(new CustomerTenantAccessStatusTypeMap());
            modelBuilder.Configurations.Add(new CustomerTenantAccessMap());
            modelBuilder.Configurations.Add(new CustomerTenantAccessCardMap());
            modelBuilder.Configurations.Add(new CustomerTenantAccessRequestMap());
            modelBuilder.Configurations.Add(new HybridPartnerMap());
            modelBuilder.Configurations.Add(new CustomerTenantAccessCardsBatchMap());
            modelBuilder.Configurations.Add(new ObjectTableLastUpdateMap());


            modelBuilder.Configurations.Add(new InboundEmailsMap());
            modelBuilder.Configurations.Add(new InboundEmailLinesMap());
            modelBuilder.Configurations.Add(new QueueDefinitionMap());
            modelBuilder.Configurations.Add(new QueueMessageMap());
            modelBuilder.Configurations.Add(new BusinessHoursHolidayMap());
            modelBuilder.Configurations.Add(new BusinessHourMap());
            modelBuilder.Configurations.Add(new APILogsDataMap());
            modelBuilder.Configurations.Add(new APILogsMap());
            modelBuilder.Configurations.Add(new ParticipantMap());
            modelBuilder.Configurations.Add(new AirlineStatisticsMap());
            modelBuilder.Configurations.Add(new AWBDescriptionOfGoodsMap());
            modelBuilder.Configurations.Add(new OceanInsightsStatusesMap());
            modelBuilder.Configurations.Add(new OceanInsightsRequestMap());
            modelBuilder.Configurations.Add(new OceanInsightsRequestsCountMap());
            modelBuilder.Configurations.Add(new LogitudeMessagesTransmissionLogMap());
            // modelBuilder.Configurations.Add(new EntityChangesAutomationMap());
            modelBuilder.Configurations.Add(new EntityChangeMap());
            modelBuilder.Configurations.Add(new AgentSharedManifestMap());
            modelBuilder.Configurations.Add(new SharedManifestTranslationMap());
            modelBuilder.Configurations.Add(new SharedManifestsStatusMap());
            modelBuilder.Configurations.Add(new AutomationMap());
            modelBuilder.Configurations.Add(new AutomationResultEmailRecipientMap());
            modelBuilder.Configurations.Add(new AutomationHistoryMap());
            modelBuilder.Configurations.Add(new AutomationLastUpdateMap());
            modelBuilder.Configurations.Add(new FeaturePackageTypeMap());
            modelBuilder.Configurations.Add(new PackageConnectedPackageMap());
            modelBuilder.Configurations.Add(new UserLicenseMap());
            modelBuilder.Configurations.Add(new EntityCasualDataMap());
            modelBuilder.Configurations.Add(new AirlineMessagingRuleMap());
            modelBuilder.Configurations.Add(new TasksSchedulerMap());
            modelBuilder.Configurations.Add(new TaskSchedulerHistoryMap());
            modelBuilder.Configurations.Add(new PaymentTermDateTypeMap());
            modelBuilder.Configurations.Add(new OtherParticipantIdMap());
            modelBuilder.Configurations.Add(new FacilitationTypeMap());
            modelBuilder.Configurations.Add(new CreditLimitSettingMap());
            modelBuilder.Configurations.Add(new CardExternalAccountsByProductMap());
            modelBuilder.Configurations.Add(new QuoteTotalVATMap());
            modelBuilder.Configurations.Add(new CustomsInterfaceMap());
            modelBuilder.Configurations.Add(new CustomsInterfaceSettingMap());
            modelBuilder.Configurations.Add(new FTPDetailMap());
            modelBuilder.Configurations.Add(new VATTypesGroupMap());
            modelBuilder.Configurations.Add(new SATInterfaceMap());
            
            modelBuilder.Configurations.Add(new SATInterfaceSettingMap());

            modelBuilder.Configurations.Add(new FBLStockMap());
            modelBuilder.Configurations.Add(new OBLTypeMap());
            modelBuilder.Configurations.Add(new SATPaymentMethodMap());
            modelBuilder.Configurations.Add(new TwoFactorAuthenticationDeviceMap());
            modelBuilder.Configurations.Add(new TenantLoginPolicyMap());
            modelBuilder.Configurations.Add(new LoginPolicyMap());

            modelBuilder.Configurations.Add(new SchedulerLogsMap());
            modelBuilder.Configurations.Add(new SchedulerProcedureMap());








            ////






            modelBuilder.Configurations.Add(new EventTypeCategoryMap());
            modelBuilder.Configurations.Add(new QuoteClosingReasonMap());
            modelBuilder.Configurations.Add(new CustomsInsuranceCompanyMap());
            modelBuilder.Configurations.Add(new CouriersVatMap());
            modelBuilder.Configurations.Add(new AgentSharedDocumentMap());
            modelBuilder.Configurations.Add(new ChargesExternalAccountsByProductMap());
            modelBuilder.Configurations.Add(new RegistryDateTypeMap());
            modelBuilder.Configurations.Add(new ReportsTemplateMap());
            modelBuilder.Configurations.Add(new ReportsTemplatesVersionMap());
            modelBuilder.Configurations.Add(new ShipmentCustomsMessageTypeMap());
            modelBuilder.Configurations.Add(new ShipmentCustomsTransmissionMap());
            modelBuilder.Configurations.Add(new BankAccountLiteMap());
            modelBuilder.Configurations.Add(new SATTransferStatusMap());
            modelBuilder.Configurations.Add(new SATInvoiceStatusMap());
            modelBuilder.Configurations.Add(new FeatureChangeMap());
            modelBuilder.Configurations.Add(new FilingInboxMap());
            modelBuilder.Configurations.Add(new FilingInboxAttachmentMap());
            modelBuilder.Configurations.Add(new FilingInboxAttachmentLogMap());
            modelBuilder.Configurations.Add(new ReportExecutionLogMap());
            modelBuilder.Configurations.Add(new CustomsAirlineMap());
            modelBuilder.Configurations.Add(new CustomsAutonomyKeywordMap());

            modelBuilder.Entity<ObjectTable>().HasOptional(p => p.MainTip).WithMany();
            modelBuilder.Entity<Tip>().HasRequired(p => p.ObjectTable).WithMany();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Configurations.Add(new CourierMasterMap());
            modelBuilder.Configurations.Add(new CourierDeclarationMap());
            
            
            
            modelBuilder.Configurations.Add(new CourierCustomStatusMap());
            modelBuilder.Configurations.Add(new AgentTalkBackTypeMap());

            modelBuilder.Configurations.Add(new DocumentTypeCustomsDataMap());
            modelBuilder.Configurations.Add(new CustomsDocumentsDefinitionMap());
            modelBuilder.Configurations.Add(new UIMessageMap());
            modelBuilder.Configurations.Add(new UIMessageAdditionalMap());
            modelBuilder.Configurations.Add(new PointerLevelMap());
            modelBuilder.Configurations.Add(new ClientDrivingLicenseMap());
            modelBuilder.Configurations.Add(new ClientDrivingLicenseTypeMap());
            modelBuilder.Configurations.Add(new PaymentChequeStatusMap());
            modelBuilder.Entity<ObjectTable>().HasOptional(p => p.MainTip).WithMany();
            modelBuilder.Entity<Tip>().HasRequired(p => p.ObjectTable).WithMany();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Configurations.Add(new MetodoPagoMap());

            #region Infrastructure Generated
            modelBuilder.Configurations.Add(new ToggleMap());
            modelBuilder.Configurations.Add(new FeatureToggleMap());
            modelBuilder.Configurations.Add(new BIReportMap());
            modelBuilder.Configurations.Add(new BIReportsTypeMap());
            modelBuilder.Configurations.Add(new BusinessRoleMap());
            modelBuilder.Configurations.Add(new BusinessProcessQueueMap());
            modelBuilder.Configurations.Add(new TeamMap());
            modelBuilder.Configurations.Add(new LBPTeamMemberMap());
            modelBuilder.Configurations.Add(new TeamMemberBusinessRoleMap());
            modelBuilder.Configurations.Add(new BatchTaskExecutionMap());
            modelBuilder.Configurations.Add(new BatchTaskExecutionStatusMap());
            modelBuilder.Configurations.Add(new SharedLogisticsSettingMap());
            #endregion

            base.OnModelCreating(modelBuilder);
        }
    }
}
