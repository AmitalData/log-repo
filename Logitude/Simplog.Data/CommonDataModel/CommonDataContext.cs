using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Transactions;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System.Data.Common;
using System.Data.Entity;
using Simplog.Data.CommonDataModel.Mapping;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Data.InvoiceModel.Mapping;
using Simplog.Data.ShipmentsModel.Mapping;
using Simplog.Data.InfrastructureModel.Mapping;
using Simplog.Data.QuoteModel.Mapping;
using System;
using System.Data;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Configuration;
using Simplog.Global.Data.GlobalModel.Mapping;

namespace Simplog.Data.CommonDataModel
{
    public class CommonDataContext : DbContextBase, ICommonDataContext
    {
        public CommonDataContext()
            //: base("LogitudeStr")
            : this(GetDefaultConn())
        {
            Database.SetInitializer<CommonDataContext>(null);
            Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
        }

        private static DbConnection GetDefaultConn()
        {
            if (LogitudeSettings.WorkEnvironment == "customs" && LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                GlobalDB currentDb;

                //using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                //{                
                currentDb = GlobalDbHelper.GetSingleGlobalDB();
                //}
                string dbConnectionInfo = currentDb.DBConnection;
                string dbSeconderyConnectionInfo = currentDb.SecondaryAzureDBConnection;

                DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo, dbSeconderyConnectionInfo);
                return connection;
            }
            else
            {

                var connectionStringMain = ConfigurationManager.ConnectionStrings["LogitudeStr"].ConnectionString;
                var builder = new SqlConnectionStringBuilder(connectionStringMain);
                DbConnection connection = new SqlConnection(builder.ToString());
                return connection;
            }

        }


        public CommonDataContext(DbConnection conn)
            : base(conn, true)
        {
            Database.SetInitializer<CommonDataContext>(null);
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.AutoDetectChangesEnabled = false;
            Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();

            // this.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL SNAPSHOT;");

        }

        public static ICommonDataContext GetContext(int tenant)
        {
            GlobalDB currentDb;
            //using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            //{
            currentDb = GlobalDbHelper.GetGlobalDB(tenant);
            //}
            string dbConnectionInfo = currentDb.DBConnection;
            string dbSeconderyConnectionInfo = currentDb.SecondaryAzureDBConnection;

            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo, dbSeconderyConnectionInfo);
            CommonDataContext context = new CommonDataContext(connection);

            return context;
        }

        public static CommonDataContext GetContextByDBId(string dbId)
        {
            GlobalDB currentDb;

            //using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            //{                
            currentDb = GlobalDbHelper.GetGlobalDBById(dbId);
            //}
            string dbConnectionInfo = currentDb.DBConnection;
            string dbSeconderyConnectionInfo = currentDb.SecondaryAzureDBConnection;

            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo, dbSeconderyConnectionInfo);
            CommonDataContext context = new CommonDataContext(connection);

            return context;
        }
        public override LogitudeDBSchema LogitudeDBSchema
        {
            get { return Simplog.Server.Infrastructure.LogitudeDBSchema.LOGITUDE_MAIN; }
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                var config = Devart.Data.Oracle.Entity.Configuration.OracleEntityProviderConfig.Instance;
                config.Workarounds.DisableQuoting = true;
                //config.QueryOptions.CaseInsensitiveComparison = true;
                //config.QueryOptions.CaseInsensitiveLike = true;

            }

            Database.SetInitializer<CommonDataContext>(null);
            modelBuilder.Configurations.Add(new SharedUserQueryMap());
            modelBuilder.Configurations.Add(new AccountingSystemMap());
            modelBuilder.Configurations.Add(new AccountingSettingMap());
            modelBuilder.Configurations.Add(new Accounts1Map());
            modelBuilder.Configurations.Add(new AccountTypeMap());
            modelBuilder.Configurations.Add(new AddressMap());
            modelBuilder.Configurations.Add(new AddressTypeMap());
            modelBuilder.Configurations.Add(new AdvancedQueryFilterMap());
            modelBuilder.Configurations.Add(new AgentMap());
            modelBuilder.Configurations.Add(new AirlineMap());
            modelBuilder.Configurations.Add(new CarrierAreaMap());
            modelBuilder.Configurations.Add(new CarrierAreasPortMap());
            modelBuilder.Configurations.Add(new APInvoiceEntityMap());
            modelBuilder.Configurations.Add(new APInvoiceLineMap());
            modelBuilder.Configurations.Add(new APInvoicePaymentMap());
            modelBuilder.Configurations.Add(new APInvoiceMap());
            modelBuilder.Configurations.Add(new APInvoiceStatuMap());
            modelBuilder.Configurations.Add(new APInvoiceTotalVATMap());
            modelBuilder.Configurations.Add(new APInvoiceTypeMap());
            modelBuilder.Configurations.Add(new APPaymentMap());
            modelBuilder.Configurations.Add(new APPaymentStatuMap());
            modelBuilder.Configurations.Add(new ARInvoiceEntityMap());
            modelBuilder.Configurations.Add(new ARInvoiceLineMap());
            modelBuilder.Configurations.Add(new ARInvoicePaymentMap());
            modelBuilder.Configurations.Add(new ARInvoiceMap());
            modelBuilder.Configurations.Add(new ARInvoiceStatuMap());
            modelBuilder.Configurations.Add(new ARInvoiceTotalVATMap());
            modelBuilder.Configurations.Add(new ARInvoiceTypeMap());
            modelBuilder.Configurations.Add(new InvoiceModel.Mapping.AccountingPaymentMethodMap());
            modelBuilder.Configurations.Add(new ARPaymentMap());
            modelBuilder.Configurations.Add(new ARPaymentStatuMap());
            modelBuilder.Configurations.Add(new ARInvoiceStocksStatusMap());
            modelBuilder.Configurations.Add(new AWBChargesCodeMap());
            modelBuilder.Configurations.Add(new AWBSpecialHandlingCodeMap());
            modelBuilder.Configurations.Add(new AWBStatuMap());
            modelBuilder.Configurations.Add(new BranchMap());
            modelBuilder.Configurations.Add(new CardContactMap());
            modelBuilder.Configurations.Add(new CardMap());
            modelBuilder.Configurations.Add(new CategoryTypeMap());
            modelBuilder.Configurations.Add(new ChargesGroupMap());
            modelBuilder.Configurations.Add(new ChargesTypeMap());
            modelBuilder.Configurations.Add(new CommunicationAttachmentMap());
            modelBuilder.Configurations.Add(new CommunicationLogMap());
            modelBuilder.Configurations.Add(new CommunicationLogTypeMap());
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
            modelBuilder.Configurations.Add(new DocumentOutCopyMap());
            modelBuilder.Configurations.Add(new DocumentOutMap());
            modelBuilder.Configurations.Add(new DocumentMap());
            modelBuilder.Configurations.Add(new DocumentTypeCopyMap());
            modelBuilder.Configurations.Add(new DocumentTypeCustomFields1Map());
            modelBuilder.Configurations.Add(new AgentSharedManifestMap());
            modelBuilder.Configurations.Add(new SharedManifestTranslationMap());
            modelBuilder.Configurations.Add(new SharedManifestsStatusMap());
            modelBuilder.Configurations.Add(new DocumentTypeMap());
            modelBuilder.Configurations.Add(new DocumentTypeTemplateMap());
            modelBuilder.Configurations.Add(new DueTypeMap());
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
            modelBuilder.Configurations.Add(new GlobalZoneMap());
            modelBuilder.Configurations.Add(new IATACodeMap());
            modelBuilder.Configurations.Add(new ImageDetailMap());
            modelBuilder.Configurations.Add(new IncotermMap());
            modelBuilder.Configurations.Add(new InsideShipmentPackageMap());
            modelBuilder.Configurations.Add(new BlobFileMap());
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
            modelBuilder.Configurations.Add(new ChargeTypeAccountingMap());
            modelBuilder.Configurations.Add(new ReportMap());
            modelBuilder.Configurations.Add(new ContactLastLoginMap());
            modelBuilder.Configurations.Add(new SharedLogisticsContactLastLoginMap());
            modelBuilder.Configurations.Add(new ContactLoginLogMap());
            modelBuilder.Configurations.Add(new SmallDocumentMap());
            modelBuilder.Configurations.Add(new CommunicationLogStepMap());
            modelBuilder.Configurations.Add(new SharedLogisticsInvitationStatusMap());
            modelBuilder.Configurations.Add(new ReportGroupMap());
            modelBuilder.Configurations.Add(new UserPermittedBranchMap());
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
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Configurations.Add(new VatUniqueTypeMap());
            modelBuilder.Configurations.Add(new VatFormatTypeMap());
            modelBuilder.Configurations.Add(new VatMandatoryTypeMap());
            modelBuilder.Configurations.Add(new CustomerSalesNoteMap());
            modelBuilder.Configurations.Add(new QuoteClosingReasonMap());
            modelBuilder.Configurations.Add(new AuthenticationTokenMap());
            modelBuilder.Configurations.Add(new CustomerStatusMap());
            modelBuilder.Configurations.Add(new ProductTypeModificationMap());
            modelBuilder.Configurations.Add(new CustomerSalesmanByProductMap());
            modelBuilder.Configurations.Add(new CustomerAccountManagerByProductMap());
            modelBuilder.Configurations.Add(new CustomerFreelancerByProductMap());
            modelBuilder.Configurations.Add(new CustomerCustomsAgentByProductMap());
            modelBuilder.Configurations.Add(new CustomerForwarderByProductMap());
            modelBuilder.Configurations.Add(new CustomerMediatorByProductMap());
            modelBuilder.Configurations.Add(new CardExternalCodeByCurrencyMap());
            modelBuilder.Configurations.Add(new BusinessUnitMap());
            modelBuilder.Configurations.Add(new FeatureAccessLevelMap());
            modelBuilder.Configurations.Add(new EmailProviderMap());
            modelBuilder.Configurations.Add(new RegionMap());
            modelBuilder.Configurations.Add(new CustomerSizeMap());
            modelBuilder.Configurations.Add(new ReportModificationMap());
            modelBuilder.Configurations.Add(new ContactsUnseenEntitieMap());
            modelBuilder.Configurations.Add(new SharedFollowedShipmentMap());
            modelBuilder.Configurations.Add(new ColorIndexMap());
            modelBuilder.Configurations.Add(new DataProviderMap());
            modelBuilder.Configurations.Add(new UserPermittedProductMap());
            modelBuilder.Configurations.Add(new DistributorMap());
            modelBuilder.Configurations.Add(new DocumentsFilingMap());
            modelBuilder.Configurations.Add(new DocumentStatusMap());
            modelBuilder.Configurations.Add(new DocumentsFilingMetaDataValueMap());
            modelBuilder.Configurations.Add(new DocumentsMetaDataTypeMap());
            modelBuilder.Configurations.Add(new DocumentTypeMetaDataMap());
            modelBuilder.Configurations.Add(new ComputingPartnerMap());
            modelBuilder.Configurations.Add(new ComputingPartnerCodeMap());
            modelBuilder.Configurations.Add(new ComputingPartnerTableMap());
            modelBuilder.Configurations.Add(new ComputingPartnerTranslationMap());
            modelBuilder.Configurations.Add(new CustomersDataViewMap());
            modelBuilder.Configurations.Add(new DocumentFolderMap());
            modelBuilder.Configurations.Add(new DocumentsDataProviderMap());
            modelBuilder.Configurations.Add(new DocumentTypeCategoryMap());
            modelBuilder.Configurations.Add(new CustomerTenantAccessStatusTypeMap());
            modelBuilder.Configurations.Add(new CustomerTenantAccessMap());
            modelBuilder.Configurations.Add(new CustomerTenantAccessCardMap());
            modelBuilder.Configurations.Add(new CustomerTenantAccessRequestMap());
            modelBuilder.Configurations.Add(new HybridPartnerMap());
            modelBuilder.Configurations.Add(new CustomerTenantAccessCardsBatchMap());
            modelBuilder.Configurations.Add(new HybridTenantStateMap());
            modelBuilder.Configurations.Add(new HybridTenantThresholdMap());
            modelBuilder.Configurations.Add(new CustomMetaDataTypesAddtionalMap());
            modelBuilder.Configurations.Add(new ParticipantMap());
            modelBuilder.Configurations.Add(new AirlineStatisticsMap());
            modelBuilder.Configurations.Add(new AWBDescriptionOfGoodsMap());
            modelBuilder.Configurations.Add(new LogitudeMessagesTransmissionLogMap());
            //modelBuilder.Configurations.Add(new EntityChangesAutomationMap());
            modelBuilder.Configurations.Add(new EntityChangeMap());
            modelBuilder.Configurations.Add(new FeaturePackageTypeMap());
            modelBuilder.Configurations.Add(new PackageConnectedPackageMap());
            modelBuilder.Configurations.Add(new UserLicenseMap());
            modelBuilder.Configurations.Add(new TenantAddOnMap());
            modelBuilder.Configurations.Add(new AutomationResultEmailRecipientMap());
            modelBuilder.Configurations.Add(new AutomationMap());
            modelBuilder.Configurations.Add(new AutomationHistoryMap());
            modelBuilder.Configurations.Add(new AutomationLastUpdateMap());
            modelBuilder.Configurations.Add(new EntityCasualDataMap());
            modelBuilder.Configurations.Add(new AirlineMessagingRuleMap());
            modelBuilder.Configurations.Add(new CustomerFieldsUpdateSettingMap());
            modelBuilder.Configurations.Add(new TenantAdditionalDataMap());
            modelBuilder.Configurations.Add(new VATTypesGroupMap());
            modelBuilder.Configurations.Add(new CustomsInterfaceSettingMap());
            modelBuilder.Configurations.Add(new FTPDetailMap());
            modelBuilder.Configurations.Add(new TwoFactorAuthenticationDeviceMap());
            modelBuilder.Configurations.Add(new TenantLoginPolicyMap());
            modelBuilder.Configurations.Add(new LoginPolicyMap());
            modelBuilder.Configurations.Add(new AgentSharedDocumentMap());
            modelBuilder.Configurations.Add(new MetodoPagoMap());
            modelBuilder.Configurations.Add(new RegistryDateTypeMap());
            modelBuilder.Configurations.Add(new WarehouseTypeMap());
            modelBuilder.Configurations.Add(new ReportsTemplateMap());
            modelBuilder.Configurations.Add(new ReportsTemplatesVersionMap());
            modelBuilder.Configurations.Add(new FeatureChangeMap());
            modelBuilder.Configurations.Add(new FilingInboxMap());
            modelBuilder.Configurations.Add(new FilingInboxAttachmentMap());
            modelBuilder.Configurations.Add(new FilingInboxAttachmentLogMap());
            modelBuilder.Configurations.Add(new ReportExecutionLogMap());
            modelBuilder.Configurations.Add(new INTTRASettingMap());
            modelBuilder.Configurations.Add(new INTTRASettingModeMap());
            modelBuilder.Configurations.Add(new INTTRABranchRegisteredCarrierMap());
            modelBuilder.Configurations.Add(new DocumentFilingBackupBatchMap());
            modelBuilder.Configurations.Add(new DocumentFilingBackupSettingMap());
            modelBuilder.Configurations.Add(new TemperatureUnitMap());
            modelBuilder.Configurations.Add(new HybridPartnersPermissionMap());
            modelBuilder.Configurations.Add(new DWHSettingMap());
            modelBuilder.Configurations.Add(new PaymentGatewayPartnerMap());
            modelBuilder.Configurations.Add(new CustomsShipperMap());
            modelBuilder.Configurations.Add(new CustomerDepositionMap());
            modelBuilder.Configurations.Add(new UsersReleaseNotesDisplayMap());
            modelBuilder.Configurations.Add(new CheckDigitControlAlgorithmMap());
            modelBuilder.Configurations.Add(new CardContactProductMap());
            modelBuilder.Configurations.Add(new DWHBuildStatusMap());
            modelBuilder.Configurations.Add(new DocumentsExecutionLogMap());
            modelBuilder.Configurations.Add(new CardContactAdditionalServiceMap()); 
            modelBuilder.Configurations.Add(new UserLastSettingsMap());
            modelBuilder.Configurations.Add(new CustomerOpenFilesAmountMap());
            modelBuilder.Configurations.Add(new CustomsInterfaceMap());
            modelBuilder.Configurations.Add(new TariffCarrierTranslationMap());
            modelBuilder.Configurations.Add(new VatUniquePartnerTypeMap());
            modelBuilder.Configurations.Add(new WarehouseWeightMeasurementMap());
            modelBuilder.Configurations.Add(new WarehouseWeightRoundingMap());
            modelBuilder.Configurations.Add(new WarehouseStoragePricingMap());
            modelBuilder.Configurations.Add(new CardSearchMap());
            modelBuilder.Configurations.Add(new HorseMap());
            modelBuilder.Configurations.Add(new CargoTenantMilestoneDefinitionMap());
            modelBuilder.Configurations.Add(new DWHEnvironmentSettingMap());
            modelBuilder.Configurations.Add(new PortTimeZoneMap());


            base.OnModelCreating(modelBuilder);
        }

        public IDbSet<TariffCarrierTranslation> TariffCarrierTranslations { get; set; }
        public IDbSet<VatFormatType> VatFormatTypes { get; set; }
        public IDbSet<EmailProvider> EmailProviders { get; set; }
        public IDbSet<AuthenticationToken> AuthenticationTokens { get; set; }
        public IDbSet<VatUniqueType> VatUniqueTypes { get; set; }
        public IDbSet<VatMandatoryType> VatMandatoryTypes { get; set; }
        public IDbSet<CustomerSalesNote> CustomerSalesNotes { get; set; }
        public IDbSet<AddressType> AddressTypes { get; set; }
        public IDbSet<Country> Countries { get; set; }
        public IDbSet<State> States { get; set; }
        public IDbSet<GlobalZone> GlobalZones { get; set; }
        public IDbSet<Address> Addresses { get; set; }
        public IDbSet<Port> Ports { get; set; }
        public IDbSet<Card> Cards { get; set; }
        public IDbSet<Customer> Customers { get; set; }
        public IDbSet<Agent> Agents { get; set; }
        public IDbSet<CustomAgent> CustomAgents { get; set; }
        public IDbSet<ShippingAgent> ShippingAgents { get; set; }
        public IDbSet<Contact> Contacts { get; set; }
        public IDbSet<CardContact> CardContacts { get; set; }
        public IDbSet<PartnerType> PartnerTypes { get; set; }
        public IDbSet<Rank> Ranks { get; set; }
        public IDbSet<User> Users { get; set; }
        public IDbSet<Department> Departments { get; set; }
        public IDbSet<Branch> Branches { get; set; }
        public IDbSet<Airline> Airlines { get; set; }
        public IDbSet<CarrierArea> CarrierAreas { get; set; }
        public IDbSet<CarrierAreasPort> CarrierAreasPorts { get; set; }

        public IDbSet<ShippingLine> ShippingLines { get; set; }
        public IDbSet<Trucker> Truckers { get; set; }
        public IDbSet<Tenant> Tenants { get; set; }
        public IDbSet<Currency> Currencies { get; set; }
        public IDbSet<ContactTenant> ContactTenants { get; set; }
        public IDbSet<ContactTenantRole> ContactTenantRoles { get; set; }
        public IDbSet<Role> Roles { get; set; }
        public IDbSet<PaymentTerm> PaymentTerms { get; set; }
        public IDbSet<VatType> VatTypes { get; set; }
        public IDbSet<Incoterm> Incoterms { get; set; }
        public IDbSet<ChargesType> ChargesTypes { get; set; }
        public IDbSet<Measurement> Measurements { get; set; }
        public IDbSet<Document> Documents { get; set; }
        public IDbSet<DocumentType> DocumentTypes { get; set; }
        public IDbSet<DocumentOut> DocumentOuts { get; set; }
        public IDbSet<PackageType> PackageTypes { get; set; }
        public IDbSet<DocumentTypeCustomField> DocumentTypeCustomFields { get; set; }
        public IDbSet<AgentSharedManifest> AgentSharedManifests { get; set; }
        public IDbSet<TenantAdditionalData> TenantAdditionalDatas { get; set; }
        public IDbSet<FormCustomField> FormCustomFields { get; set; }
        public IDbSet<FieldDataType> FieldDataTypes { get; set; }
        public IDbSet<WeightUnit> WeightUnits { get; set; }
        public IDbSet<DimensionsUnit> DimensionsUnits { get; set; }
        public IDbSet<RateClass> RateClasses { get; set; }
        public IDbSet<CommunicationLog> CommunicationLogs { get; set; }
        public IDbSet<CommunicationAttachment> CommunicationAttachments { get; set; }
        public IDbSet<DueType> DueTypes { get; set; }
        public IDbSet<CommunicationStatusType> CommunicationStatusTypes { get; set; }
        public IDbSet<CommunicationLogType> CommunicationLogTypes { get; set; }
        public IDbSet<WarehouseType> WarehouseTypes { get; set; }
        public IDbSet<NumberFormat> NumberFormats { get; set; }
        public IDbSet<DocumentTypeTemplate> DocumentTypeTemplates { get; set; }
        public IDbSet<TemplateFormat> TemplateFormats { get; set; }
        public IDbSet<DWHSetting> DWHSettings { get; set; }
        public IDbSet<LogBoxTenantSetting> LogBoxTenantSettings { get; set; }
        public IDbSet<DWHBuildStatus> DWHBuildStatus { get; set; }
        public IDbSet<UserLastSettings> UserLastSettings { get; set; }
        public IDbSet<WarehouseWeightMeasurement> WarehouseWeightMeasurements { get; set; }
        public IDbSet<WarehouseWeightRounding> WarehouseWeightRoundings { get; set; }
        public IDbSet<WarehouseStoragePricing> WarehouseStoragePricings { get; set; }
        public IDbSet<Horse> Horses { get; set; }

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
        public IDbSet<CustomerSize> CustomerSizes
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
        public IDbSet<PasswordResetRequest> PasswordResetRequests { get; set; }
        public IDbSet<UserPermittedBranch> UserPermittedBranches { get; set; }
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
        public IDbSet<Competitor> Competitors { get; set; }
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
        public IDbSet<DocumentsDataProvider> DocumentsDataProviders
        {
            get;
            set;
        }
        public IDbSet<CustomerAdditionalService> CustomerAdditionalServices { get; set; }
        public IDbSet<Commodity> Commodities { get; set; }
        public IDbSet<ContactDoneMethod> ContactDoneMethods { get; set; }
        public IDbSet<AdditionalService> AdditionalServices
        {
            get;
            set;
        }
        public IDbSet<CustomerStatus> CustomerStatus { get; set; }
        public IDbSet<ProductTypeModification> ProductTypeModifications { get; set; }
        public IDbSet<CustomerSalesmanByProduct> CustomerSalesmanByProducts { get; set; }
        public IDbSet<CustomerAccountManagerByProduct> CustomerAccountManagerByProducts
        {
            get;
            set;
        }
        public IDbSet<CustomerFreelancerByProduct> CustomerFreelancerByProducts
        {
            get;
            set;
        }
        public IDbSet<CustomerCustomsAgentByProduct> CustomerCustomsAgentByProducts
        {
            get;
            set;
        }
        public IDbSet<CustomerForwarderByProduct> CustomerForwarderByProducts
        {
            get;
            set;
        }
        public IDbSet<CustomerMediatorByProduct> CustomerMediatorByProducts
        {
            get;
            set;
        }
        public IDbSet<CardExternalCodeByCurrency> CardExternalCodeByCurrencies
        {
            get;
            set;
        }
        public IDbSet<BusinessUnit> BusinessUnits { get; set; }
        public IDbSet<FeatureAccessLevel> FeatureAccessLevels { get; set; }
        public IDbSet<Region> Regions
        {
            get;
            set;
        }
        public IDbSet<CountryCity> CountryCities
        {
            get;
            set;
        }
        public IDbSet<ReportModification> ReportModifications
        {
            get;
            set;
        }
        public IDbSet<ContactsUnseenEntitie> ContactsUnseenEntities
        {
            get;
            set;
        }
        public IDbSet<UserPermittedProduct> UserPermittedProducts
        {
            get;
            set;
        }
        public IDbSet<Distributor> Distributors
        {
            get;
            set;
        }
        public IDbSet<SharedFollowedShipment> SharedFollowedShipments
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
        public IDbSet<DocumentsFiling> DocumentsFilings
        {
            get;
            set;
        }
        public IDbSet<DocumentStatus> DocumentStatus
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
        public IDbSet<CustomerTenantAccessCard> CustomerTenantAccessCards
        {
            get;
            set;
        }
        public IDbSet<CustomerTenantAccessRequest> CustomerTenantAccessRequests
        {
            get;
            set;
        }
        public IDbSet<HybridTenantState> HybridTenantStates
        {
            get;
            set;
        }
        public IDbSet<HybridTenantThreshold> HybridTenantThresholds
        {
            get;
            set;
        }
        public IDbSet<CustomMetaDataTypesAddtional> CustomMetaDataTypesAddtionals { get; set; }
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
        public IDbSet<EntityChange> EntityChanges
        {
            get;
            set;
        }
        public IDbSet<Automation> Automations
        {
            get;
            set;
        }
        public IDbSet<AutomationResultEmailRecipient> AutomationResultEmailRecipients
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
        public IDbSet<CargoTenantMilestoneDefinition> CargoTenantMilestoneDefinitions
        {
            get;
            set;
        }
        public IDbSet<ComputingPartner> ComputingPartners { get; set; }
        public IDbSet<ComputingPartnerCode> ComputingPartnerCodes { get; set; }
        public IDbSet<ComputingPartnerTable> ComputingPartnerTables { get; set; }
        public IDbSet<ComputingPartnerTranslation> ComputingPartnerTranslations { get; set; }
        public IDbSet<DocumentFolder> DocumentFolders { get; set; }
        public IDbSet<Participant> Participants { get; set; }
        public IDbSet<AirlineStatistics> AirlineStatistics { get; set; }
        public IDbSet<AWBDescriptionOfGoods> AWBDescriptionOfGoods { get; set; }
        public IDbSet<LogitudeMessagesTransmissionLog> LogitudeMessagesTransmissionLogs { get; set; }
        public IDbSet<FeaturePackageType> FeaturePackageTypes { get; set; }
        public IDbSet<PackageConnectedPackage> PackageConnectedPackages { get; set; }
        public IDbSet<UserLicense> UserLicenses { get; set; }
        public IDbSet<PaymentTermDateType> PaymentTermDateTypes { get; set; }
        public IDbSet<BlobFile> BlobFiles { get; set; }
        public IDbSet<EntityCasualData> EntityCasualDatas { get; set; }
        public IDbSet<AirlineMessagingRule> AirlineMessagingRules { get; set; }
        public IDbSet<CustomerFieldsUpdateSetting> CustomerFieldsUpdateSettings { get; set; }
        public IDbSet<CreditLimitSetting> CreditLimitSettings { get; set; }
        public IDbSet<CardExternalAccountsByProduct> CardExternalAccountsByProducts { get; set; }
        public IDbSet<SharedManifestTranslation> SharedManifestTranslations { get; set; }
        public IDbSet<SharedManifestsStatus> SharedManifestsStatuses { get; set; }
        public IDbSet<VATTypesGroup> VATTypesGroups { get; set; }
        public IDbSet<CustomsInterface> CustomsInterfaces
        {
            get;
            set;
        }
        public IDbSet<CustomsInterfaceSetting> CustomsInterfaceSettings
        {
            get;
            set;
        }
        public IDbSet<TwoFactorAuthenticationDevice> TwoFactorAuthenticationDevices { get; set; }
        public IDbSet<TenantLoginPolicy> TenantLoginPolicies { get; set; }
        public IDbSet<LoginPolicy> LoginPolicies { get; set; }
        public IDbSet<FTPDetail> FTPDetails
        {
            get;
            set;
        }
        public IDbSet<AgentSharedDocument> AgentSharedDocuments { get; set; }
        public IDbSet<MetodoPago> MetodoPagos { get; set; }
        public IDbSet<ChargesExternalAccountsByProduct> ChargesExternalAccountsByProducts { get; set; }
        public IDbSet<RegistryDateType> RegistryDateTypes { get; set; }
        public IDbSet<UsoCFDI> UsoCFDIs { get; set; }
        public IDbSet<ReportsTemplate> ReportsTemplates { get; set; }
        public IDbSet<ReportsTemplatesVersion> ReportsTemplatesVersions { get; set; }
        public IDbSet<FeatureChange> FeatureChanges { get; set; }
        public IDbSet<FilingInbox> FilingInboxes { get; set; }
        public IDbSet<FilingInboxAttachment> FilingInboxAttachments { get; set; }
        public IDbSet<FilingInboxAttachmentLog> FilingInboxAttachmentLogs { get; set; }
        public IDbSet<ReportExecutionLog> ReportExecutionLogs { get; set; }
        public IDbSet<INTTRASetting> INTTRASettings { get; set; }
        public IDbSet<INTTRASettingMode> INTTRASettingModes { get; set; }
        public IDbSet<INTTRABranchRegisteredCarrier> INTTRABranchRegisteredCarriers { get; set; }
        public IDbSet<TemperatureUnit> TemperatureUnits { get; set; }
        public IDbSet<DocumentFilingBackupBatch> DocumentFilingBackupBatches { get; set; }
        public IDbSet<DocumentFilingBackupSetting> DocumentFilingBackupSettings { get; set; }
        public IDbSet<HybridPartnersPermission> HybridPartnersPermissions { get; set; }
        public IDbSet<PaymentGatewayPartner> PaymentGatewayPartners { get; set; }
        public IDbSet<CustomsShipper> CustomsShippers { get; set; }
        public IDbSet<CustomerDeposition> CustomerDepositions { get; set; }
        public IDbSet<UsersReleaseNotesDisplay> UsersReleaseNotesDisplays { get; set; }
        public IDbSet<CheckDigitControlAlgorithm> CheckDigitControlAlgorithms { get; set; }
        public IDbSet<CardContactProduct> CardContactProducts { get; set; }
        public IDbSet<DocumentsExecutionLog> DocumentsExecutionLogs { get; set; } 
        public IDbSet<AccountingPartner> AccountingPartners { get; set; }
        public IDbSet<CardContactAdditionalService> CardContactAdditionalServices { get; set; }

        public IDbSet<SharedLogisticsContactLastLogin> SharedLogisticsContactLastLogins { get; set; }
        public IDbSet<CustomerOpenFilesAmount> CustomerOpenFilesAmounts { get; set; }
        public IDbSet<VatUniquePartnerType> VatUniquePartnerTypes { get; set; }
        public IDbSet<CardSearch> CardSearches { get; set; }
        public IDbSet<ProductItem> ProductItems { get; set; }
        public IDbSet<HTSCode> HTSCodes { get; set; }
        public IDbSet<DWHEnvironmentSetting> DWHEnvironmentSettings { get; set; }

        public IDbSet<PortTimeZone> PortTimeZones { get; set; }


        public DbConnection GetConnection()
        {
            return this.Database.Connection;
        }
        public DbContext GetActiveDbContext()
        {
            return this;
        }
        public void SetAsModified(object entity)
        {
            try
            {
                this.Entry(entity).State = EntityState.Modified;
            }
            catch { }
        }
        public void DetectChanges()
        {
            this.ChangeTracker.DetectChanges();
        }
        public int SaveChanges()
        {
            DetectChanges();
            return base.SaveChanges();
        }


    }
}
