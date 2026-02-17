using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Transactions;
using Simplog.Data.CommonDataModel.EntityPOCOs; 
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Mapping;
using Simplog.Data.InfrastructureModel.EntityPOCOs; 
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Mapping;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Mapping;
using Simplog.Data.QuoteModel.Mapping;
using Simplog.Data.ShipmentsModel.Mapping;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Helpers;
using Simplog.Server.Infrastructure;
using System.Linq;
using Simplog.Global.Data.GlobalModel.Mapping;
//using WebFreight.Web.QuoteModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel
{
    public class WebFreightContext : DbContextBase, IWebFreightContext
    {
        public WebFreightContext()
            : base("LogitudeStr")
        {
            Database.SetInitializer<WebFreightContext>(null);
            Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
        }

        public WebFreightContext(DbConnection connection)
            : base(connection, true)
        {
            InitializeContext();
            Database.SetInitializer<WebFreightContext>(null);
            Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
        }

        private void InitializeContext()
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.AutoDetectChangesEnabled = false;
            Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
        }
        public static IWebFreightContext GetSecondaryContext(int tenant)
        {
            GlobalDB currentDb;
            currentDb = GlobalDbHelper.GetGlobalDB(tenant);
            string dbSeconderyConnectionInfo = currentDb.SecondaryAzureDBConnection;
            DbConnection connection = DatabaseInitializer.GetConnection(dbSeconderyConnectionInfo, null, null);
            WebFreightContext context = new WebFreightContext(connection);
            return context;
        }
        public static IWebFreightContext GetContext(int tenant)
        {
            GlobalDB currentDb;
            //using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            //{
            currentDb = GlobalDbHelper.GetGlobalDB(tenant);

            // }
            string dbConnectionInfo = currentDb.DBConnection;
            string dbSeconderyConnectionInfo = currentDb.SecondaryAzureDBConnection;

            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo, dbSeconderyConnectionInfo);
            WebFreightContext context = new WebFreightContext(connection);

            return context;
        }
        public  DbContextTransaction GetSnapshotTransaction()
        {
            return Database.BeginTransaction(System.Data.IsolationLevel.Snapshot);

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
                ///modelBuilder.SetDefaultSchema("LOGITUDE_MAIN");
            }
            Database.SetInitializer<WebFreightContext>(null);
            //string databasename = DatabaseInitializer.GetDatabaseName();
            //Database.DefaultConnectionFactory.CreateConnection(databasename);

            modelBuilder.Configurations.Add(new AccountingSystemMap());
            modelBuilder.Configurations.Add(new AccountingSettingMap());
            modelBuilder.Configurations.Add(new Accounts1Map());
            modelBuilder.Configurations.Add(new AccountTypeMap());
            modelBuilder.Configurations.Add(new AddressMap());
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
            modelBuilder.Configurations.Add(new APPaymentMap());
            modelBuilder.Configurations.Add(new APPaymentStatuMap());
            modelBuilder.Configurations.Add(new ARInvoiceEntityMap());
            modelBuilder.Configurations.Add(new ARInvoiceLineMap());
            modelBuilder.Configurations.Add(new ARInvoicePaymentMap());
            modelBuilder.Configurations.Add(new ARInvoiceMap());
            modelBuilder.Configurations.Add(new ARInvoiceStatuMap());
            modelBuilder.Configurations.Add(new ARInvoiceTotalVATMap());
            modelBuilder.Configurations.Add(new ARInvoiceTypeMap());
            modelBuilder.Configurations.Add(new AccountingPaymentMethodMap());
            modelBuilder.Configurations.Add(new ARPaymentMap());
            modelBuilder.Configurations.Add(new ARPaymentStatuMap());
            modelBuilder.Configurations.Add(new ARInvoiceStocksStatusMap());
            modelBuilder.Configurations.Add(new AWBChargesCodeMap());
            modelBuilder.Configurations.Add(new AWBSpecialHandlingCodeMap());
            modelBuilder.Configurations.Add(new AWBStatuMap());
            modelBuilder.Configurations.Add(new EmailAlertSettingMap());

            //modelBuilder.Configurations.Add(new BlobMap());
            //modelBuilder.Configurations.Add(new BlobTypeMap());
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
            //modelBuilder.Configurations.Add(new DocPrintCopyMap());
            //modelBuilder.Configurations.Add(new DocPrintMap());
            //modelBuilder.Configurations.Add(new DocTypeMap());
            //modelBuilder.Configurations.Add(new DocumentInMap());
            modelBuilder.Configurations.Add(new DocumentsFilingMap());
            modelBuilder.Configurations.Add(new DocumentOutCopyMap());
            modelBuilder.Configurations.Add(new DocumentOutMap());
            modelBuilder.Configurations.Add(new DocumentMap());
            modelBuilder.Configurations.Add(new DocumentTypeCopyMap());
            modelBuilder.Configurations.Add(new DocumentTypeCustomFields1Map());
            modelBuilder.Configurations.Add(new DocumentTypeMap());
            modelBuilder.Configurations.Add(new DocumentTypeTemplateMap());
            modelBuilder.Configurations.Add(new DueTypeMap());
            modelBuilder.Configurations.Add(new EntityDateMap());
            modelBuilder.Configurations.Add(new EntityLastActivityMap());
            modelBuilder.Configurations.Add(new EntityLastActivityTypeMap());
            modelBuilder.Configurations.Add(new EntityLastUpdateMap());
            modelBuilder.Configurations.Add(new EntityStatuMap());
            modelBuilder.Configurations.Add(new EventTypeMap());
            modelBuilder.Configurations.Add(new EventRemarkMap());
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

            modelBuilder.Configurations.Add(new PaymentTermMap());
            modelBuilder.Configurations.Add(new PermissionTypeMap());
            modelBuilder.Configurations.Add(new PickUpDeliveryFromToTypeMap());
            modelBuilder.Configurations.Add(new PickUpDeliveryTypeMap());
            modelBuilder.Configurations.Add(new PortMap());
            modelBuilder.Configurations.Add(new PrepaidCollectMap());
            //modelBuilder.Configurations.Add(new PrintTemplateCopyMap());
            //modelBuilder.Configurations.Add(new PrintTemplateCustomFieldMap());
            //modelBuilder.Configurations.Add(new PrintTemplateFormCustomFieldMap());
            //modelBuilder.Configurations.Add(new PrintTemplateOptionMap());
            //modelBuilder.Configurations.Add(new PrintTemplateMap());
            //modelBuilder.Configurations.Add(new PrintTemplateVersionMap());
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
            modelBuilder.Configurations.Add(new AdditionalCurrencyRateMap());
            modelBuilder.Configurations.Add(new CurrencyRateMap());
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
            modelBuilder.Configurations.Add(new AccountingTransferHeaderMap());
            modelBuilder.Configurations.Add(new AccountingTransferLineMap());
            modelBuilder.Configurations.Add(new AccountingTransferTypeMap());
            modelBuilder.Configurations.Add(new EventTypeCategoryMap());
            modelBuilder.Configurations.Add(new ContactLastLoginMap());
            modelBuilder.Configurations.Add(new SharedLogisticsContactLastLoginMap());
            modelBuilder.Configurations.Add(new SmallDocumentMap());
            modelBuilder.Configurations.Add(new CommunicationLogStepMap());
            //modelBuilder.Configurations.Add(new AgingReportInvoiceDataViewMap());
            //modelBuilder.Configurations.Add(new APAgingReportDataViewMap());
            //modelBuilder.Configurations.Add(new ShipmentDataViewMap());
            //modelBuilder.Configurations.Add(new ShipmentFollowUpDataViewMap());
            modelBuilder.Configurations.Add(new SharedLogisticsInvitationStatusMap());
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
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
            modelBuilder.Configurations.Add(new ContactDoneMethodMap());
            modelBuilder.Configurations.Add(new AdditionalServiceMap());
            modelBuilder.Configurations.Add(new QuotePackageMap());
            modelBuilder.Configurations.Add(new QuoteClosingReasonMap());
            modelBuilder.Configurations.Add(new CustomerStatusMap());
            modelBuilder.Configurations.Add(new CustomerSizeMap());
            modelBuilder.Configurations.Add(new ObjectTableLastUpdateMap());
            modelBuilder.Configurations.Add(new EntityStatusTypeMap());

            // InboundEmails
            modelBuilder.Configurations.Add(new InboundEmailsMap());
            modelBuilder.Configurations.Add(new InboundEmailLinesMap());

            modelBuilder.Configurations.Add(new QueueDefinitionMap());
            modelBuilder.Configurations.Add(new QueueMessageMap());

            // Business Hour 
            modelBuilder.Configurations.Add(new BusinessHourMap());
            modelBuilder.Configurations.Add(new BusinessHoursHolidayMap());

            modelBuilder.Configurations.Add(new APILogsMap());
            modelBuilder.Configurations.Add(new APILogsDataMap());

            modelBuilder.Configurations.Add(new QueueMessageMoreDetailsMap());
            modelBuilder.Configurations.Add(new ParticipantMap());
            modelBuilder.Configurations.Add(new AirlineStatisticsMap());
            modelBuilder.Configurations.Add(new AWBDescriptionOfGoodsMap());
            modelBuilder.Configurations.Add(new LogitudeMessagesTransmissionLogMap());
            modelBuilder.Configurations.Add(new TasksSchedulerMap());
            modelBuilder.Configurations.Add(new TaskSchedulerHistoryMap());
            modelBuilder.Configurations.Add(new DWObjectTableMap());
            modelBuilder.Configurations.Add(new DWObjectFieldMap());


            modelBuilder.Configurations.Add(new DWQueryMap());
            modelBuilder.Configurations.Add(new DWSubQueryMap());
            modelBuilder.Configurations.Add(new DWQueryColumnMap());
            modelBuilder.Configurations.Add(new DWQueryFilterMap());
            modelBuilder.Configurations.Add(new CustomsShipperMap());
            modelBuilder.Configurations.Add(new SharedUserQueryMap());
            modelBuilder.Configurations.Add(new DWCategoriesMap());
            modelBuilder.Configurations.Add(new DWObjectFieldCategoriesMap());
            //modelBuilder.Configurations.Add(new SchedulerLogsMap());
            modelBuilder.Configurations.Add(new SchedulerProcedureMap());
            modelBuilder.Configurations.Add(new WorkerRoleNameMap());
            modelBuilder.Configurations.Add(new QueryExportExecutionLogMap());
            modelBuilder.Configurations.Add(new ChildEntitiesCustomFieldMap());
            modelBuilder.Configurations.Add(new ScreenSectionMap());
            modelBuilder.Configurations.Add(new TabModificationMap());
            modelBuilder.Configurations.Add(new CustomChildObjectMap());
            modelBuilder.Configurations.Add(new DataCustomObjectMap());
            modelBuilder.Configurations.Add(new ReferenceCustomObjectMap());
            modelBuilder.Configurations.Add(new DeploymentPackageMap());
            modelBuilder.Configurations.Add(new DeploymentPackagesVersionMap());
            modelBuilder.Configurations.Add(new CustomFieldsMainObjectMap());
            modelBuilder.Configurations.Add(new DeploymentPackageExecutionLogMap());
            modelBuilder.Configurations.Add(new SearchIndexMap());
            modelBuilder.Configurations.Add(new SearchIndexTenantHistoryMap());


            modelBuilder.Entity<ObjectTable>().HasOptional(p => p.MainTip).WithMany();
            modelBuilder.Entity<Tip>().HasRequired(p => p.ObjectTable).WithMany();
            base.OnModelCreating(modelBuilder);
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

        public IDbSet<PartnerType> PartnerTypes
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

        public IDbSet<FieldDataType> FieldDataTypes
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

        public DbSet<TextCode> TextCodes
        {
            get;
            set;
        }

        public IDbSet<ObjectTable> ObjectTables
        {
            get;
            set;
        }

        public IQueryable<ObjectField> ObjectFields { get { return ObjectFieldsDbSet.Where(x => !x.ForMetaDataOnly); } }
        public DbSet<ObjectField> ObjectFieldsDbSet
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

        public IDbSet<AdditionalCurrencyRate> AdditionalCurrencyRates
        {
            get;
            set;
        }

        public IDbSet<CurrencyRate> CurrencyRates
        {
            get;
            set;
        }

        public IDbSet<EventType> EventType
        {
            get;
            set;
        }
       /* public IDbSet<EventRemark> EventRemark
        {
            get;
            set;
        }*/
        public IDbSet<TraceEvent> TraceEvent
        {
            get;
            set;
        }

        public IDbSet<Rank> Ranks
        {
            get;
            set;
        }

        public IDbSet<Document> Documents
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
        public IDbSet<QuoteChargesGroup> QuoteChargesGroups
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

        public IDbSet<EmailAlertSetting> EmailAlertSettings
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
        public IDbSet<ImageLibrary> ImageLibraries
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

        public IDbSet<TasksScheduler> TasksSchedulers
        {
            get;
            set;
        }

        public IDbSet<TaskSchedulerHistory> TaskSchedulerHistories
        {
            get;
            set;
        }

        public IDbSet<SharedUserQuery> SharedUserQueries
        {
            get;
            set;
        }

        //public IDbSet<SchedulerLogs> SchedulerLogs
        //{
        //    get;
        //    set;
        //}

        public IDbSet<SchedulerProcedure> SchedulerProcedures
        {
            get;
            set;
        }

        public IDbSet<ChildEntitiesCustomField> ChildEntitiesCustomFields
        {
            get;
            set;
        }


        public IDbSet<ScreenSection> ScreenSections
        {
            get;
            set;
        }

        public IDbSet<CustomChildObject> CustomChildObjects
        {
            get;
            set;
        }

        public void SetAsModified(object entity)
        {
            this.Entry(entity).State = EntityState.Modified;
        }

        public void DetectChanges()
        {
            this.ChangeTracker.DetectChanges();
        }

        public int SaveChanges()
        {
            //try
            //{
            DetectChanges();

            return base.SaveChanges();
            //}
            //catch (DbEntityValidationException ex) //itzik
            //{

            //    //var FormatedException = ExceptionFormatUtil.GetFormated(ex);
            //    var errMess = "";
            //    foreach (var eve in ex.EntityValidationErrors)
            //    {
            //        errMess += string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
            //            eve.Entry.Entity.GetType().Name, eve.Entry.State);
            //        foreach (var ve in eve.ValidationErrors)
            //        {
            //            errMess += string.Format("- Property: \"{0}\", Error: \"{1}\"",
            //                ve.PropertyName, ve.ErrorMessage);
            //        }
            //    }
            //    throw new Exception(errMess);
            //}
            //catch (Exception e)
            //{
            //    throw e;
            //}







        }

        public IDbSet<EventTypeCategory> EventTypeCategories
        {
            get;
            set;
        }

        public DbConnection GetConnection()
        {
            return this.Database.Connection;
        }
        public DbContext GetActiveDbContext()
        {
            return this;
        }


        public IDbSet<ObjectTableLastUpdate> ObjectTableLastUpdates
        {
            get;
            set;
        }

        // Inbound Emails 
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


        public IDbSet<QueueDefinition> QueueDefinitions
        {
            get;
            set;
        }

        public IDbSet<QueueMessage> QueueMessages
        {
            get;
            set;
        }

        // Business Hours
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


        public IDbSet<QueueMessageMoreDetails> QueueMessageMoreDetails
        {
            get;
            set;
        }


        public IDbSet<DWObjectTable> DWObjectTables
        {
            get;
            set;
        }

        public IDbSet<DWObjectField> DWObjectFields
        {
            get;
            set;
        }


        public IDbSet<DWQuery> DWQueries
        {
            get;
            set;
        }

        public IDbSet<DWSubQuery> DWSubQueries
        {
            get;
            set;
        }

        public IDbSet<DWQueryColumn> DWQueryColumns
        {
            get;
            set;
        }

        public IDbSet<DWQueryFilter> DWQueryFilters
        {
            get;
            set;
        }

        public IDbSet<DWCategories> DWCategories
        {
            get;
            set;
        }

        public IDbSet<DWObjectFieldCategories> DWObjectFieldCategories
        {
            get;
            set;
        }

        public IDbSet<RuleUpdateHistory> RuleUpdateHistories
        {
            get;
            set;
        }

        public IDbSet<WorkerRoleName> WorkerRoleNames
        {
            get;
            set;
        }

        public IDbSet<QueryExportExecutionLog> QueryExportExecutionLogs
        {
            get;
            set;
        }

        public IDbSet<EntityStatusType> EntityStatusTypes
        {
            get;
            set;
        }

        public IDbSet<MultiEntityUpdateLog> MultiEntityUpdateLogs
        {
            get;
            set;
        }

        public IDbSet<TabModification> TabsModifications
        {
            get;
            set;
        }

        public IDbSet<DataCustomObject> DataCustomObjects
        {
            get;
            set;
        }
        public IDbSet<ReferenceCustomObject> ReferenceCustomObjects
        {
            get;
            set;
        }
        public IDbSet<DeploymentPackage> DeploymentPackages
        {
            get;
            set;
        }
        public IDbSet<DeploymentPackagesVersion> DeploymentPackagesVersions
        {
            get;
            set;
        }
        public IDbSet<CustomFieldsMainObject> CustomFieldsMainObjects
        {
            get;
            set;
        }
        public IDbSet<DeploymentPackageExecutionLog> DeploymentPackageExecutionLogs
        {
            get;
            set;
        }

        public IDbSet<EventRemark> EventRemarks
        {
            get;
            set;
        }

        public IDbSet<DefaultAndConfiguration> DefaultAndConfigurations
        {
            get;
            set;
        }

        public IDbSet<DefaultAndConfigurationKey> DefaultAndConfigurationKey
        {
            get;
            set;
        }

        public IDbSet<SearchIndex> SearchIndexes
        {
            get;
            set;
        }

        public IDbSet<SearchIndexTenantHistory> SearchIndexTenantHistories
        {
            get;
            set;
        }
    }
}