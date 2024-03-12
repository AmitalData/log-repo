using System.Data.Common;
using System.Data.Entity;
using Simplog.Data.CommonDataModel.Mapping;
using Simplog.Data.InfrastructureModel.Mapping;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Mapping;
using Simplog.Data.QuoteModel.Mapping;
using Simplog.Data.ShipmentsModel.Mapping;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Helpers;
using Simplog.Server.Infrastructure;


namespace Simplog.Data.InvoiceModel
{
    public class InvoiceContext : DbContextBase, IInvoiceContext
    {
        public InvoiceContext()
            : base("LogitudeStr")
        {
            Database.SetInitializer<InvoiceContext>(null);
            Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
        }

        public InvoiceContext(DbConnection conn)
            : base(conn, true)
        {
            Configuration.LazyLoadingEnabled = false;
            Configuration.AutoDetectChangesEnabled = false;
            Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
            Database.SetInitializer<InvoiceContext>(null);
        }

        public void SetAsModified(object entity)
        {
            Entry(entity).State = EntityState.Modified;
        }

        public static IInvoiceContext GetSecContext(int tenant)
        {
            GlobalDB currentDb;
            currentDb = GlobalDbHelper.GetGlobalDB(tenant);
            string dbSeconderyConnectionInfo = currentDb.SecondaryAzureDBConnection;
            DbConnection connection = DatabaseInitializer.GetConnection(dbSeconderyConnectionInfo, null, null);
            InvoiceContext context = new InvoiceContext(connection);
            return context;
        }

        public static IInvoiceContext GetContext(int tenant)
        {
            GlobalDB currentDb;
            currentDb = GlobalDbHelper.GetGlobalDB(tenant);
            string dbConnectionInfo = currentDb.DBConnection;
            string dbSeconderyConnectionInfo = currentDb.SecondaryAzureDBConnection;
            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo, dbSeconderyConnectionInfo);
            InvoiceContext context = new InvoiceContext(connection);
            return context;
        }

        public override LogitudeDBSchema LogitudeDBSchema
        {
            get { return LogitudeDBSchema.LOGITUDE_MAIN; }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (LogitudeSettings.DatabaseManagementSystem == "oracle")
            {
                var config = Devart.Data.Oracle.Entity.Configuration.OracleEntityProviderConfig.Instance;
                config.Workarounds.DisableQuoting = true;
            }

            Database.SetInitializer<InvoiceContext>(null);
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
            modelBuilder.Configurations.Add(new ARInvoiceLineActionMap());
            modelBuilder.Configurations.Add(new ARPaymentTransferStatusMap());
            modelBuilder.Configurations.Add(new APPaymentTransferStatusMap());
            modelBuilder.Configurations.Add(new ARInvoiceStockMap());
            modelBuilder.Configurations.Add(new ARInvoiceStockLineMap());
            modelBuilder.Configurations.Add(new ARInvoicesSignedStatusMap());
            modelBuilder.Configurations.Add(new ARPaymentBankTranferMap());
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
            modelBuilder.Configurations.Add(new AccountingTransferHeaderMap());
            modelBuilder.Configurations.Add(new AccountingTransferLineMap());
            modelBuilder.Configurations.Add(new AccountingTransferTypeMap());
            modelBuilder.Configurations.Add(new EventTypeCategoryMap());
            modelBuilder.Configurations.Add(new ContactLastLoginMap());
            modelBuilder.Configurations.Add(new SharedLogisticsContactLastLoginMap());
            modelBuilder.Configurations.Add(new SmallDocumentMap());
            modelBuilder.Configurations.Add(new CommunicationLogStepMap());
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
            modelBuilder.Configurations.Add(new ContactDoneMethodMap());
            modelBuilder.Configurations.Add(new AdditionalServiceMap());
            modelBuilder.Configurations.Add(new QuotePackageMap());
            modelBuilder.Configurations.Add(new ExternalSystemsTablesCodeMap());
            modelBuilder.Configurations.Add(new QuoteClosingReasonMap());
            modelBuilder.Configurations.Add(new ExternalSystemsSyncStatusMap());
            modelBuilder.Configurations.Add(new ExternalSystemsMissingTranslationMap());
            modelBuilder.Configurations.Add(new AccountingSystemsSettingMap());
            modelBuilder.Configurations.Add(new AccountingSystemsSyncStatusMap());
            modelBuilder.Configurations.Add(new QuickbooksSyncRequestTicketMap());
            modelBuilder.Configurations.Add(new CustomerStatusMap());
            modelBuilder.Configurations.Add(new ParticipantMap());
            modelBuilder.Configurations.Add(new AirlineStatisticsMap());
            modelBuilder.Configurations.Add(new AWBDescriptionOfGoodsMap());
            modelBuilder.Configurations.Add(new LogitudeMessagesTransmissionLogMap());
            modelBuilder.Configurations.Add(new SATInterfaceSettingMap());
            modelBuilder.Configurations.Add(new SATPaymentMethodMap());
            modelBuilder.Configurations.Add(new BankAccountLiteMap());
            modelBuilder.Configurations.Add(new SATTransferStatusMap());
            modelBuilder.Configurations.Add(new QBOGlobalTaxCalculationMap());
            modelBuilder.Configurations.Add(new SATInvoiceStatusMap());
            modelBuilder.Configurations.Add(new CustomsShipperMap());
            modelBuilder.Configurations.Add(new ARPaymentChequeReplicaMap());
            modelBuilder.Configurations.Add(new ARPaymentChequeStatusReplicaMap());
            modelBuilder.Configurations.Add(new DigitalInvoicesCounterDataViewMap());
            modelBuilder.Configurations.Add(new ControlForInvoiceLinesDataViewMap());

            base.OnModelCreating(modelBuilder);
        }

        public IDbSet<ARInvoice> ARInvoices
        {
            get; set;
        }

        public IDbSet<ARInvoiceLine> ARInvoiceLines
        {
            get; set;
        }

        public IDbSet<ARInvoiceType> ARInvoiceTypes
        {
            get; set;
        }
        public IDbSet<ARInvoiceStatus> ARInvoiceStatuses
        {
            get; set;
        }

        public IDbSet<ARInvoiceTotalVAT> ARInvoiceTotalVATs
        {
            get; set;
        }

        public IDbSet<ARInvoiceEntity> ARInvoiceEntities
        {
            get; set;
        }

        public IDbSet<Account> Accounts
        {
            get; set;
        }

        public IDbSet<AccountType> AccountTypes
        {
            get; set;
        }

        public IDbSet<ARPayment> ARPayments
        {
            get; set;
        }

        public IDbSet<EntityPOCOs.AccountingPaymentMethod> AccountingPaymentMethods
        {
            get; set;
        }

        public IDbSet<ARPaymentStatus> ARPaymentStatus
        {
            get; set;
        }

        public IDbSet<ARInvoicePayment> ARInvoicePayments
        {
            get; set;
        }

        public IDbSet<ARInvoicesSignedStatus> ARInvoicesSignedStatuses
        {
            get; set;
        }

        public IDbSet<APInvoice> APInvoices
        {
            get; set;
        }

        public IDbSet<APInvoiceLine> APInvoiceLines
        {
            get; set;
        }

        public IDbSet<APInvoiceStatus> APInvoiceStatus
        {
            get; set;
        }

        public IDbSet<APInvoiceTotalVAT> APInvoiceTotalVATs
        {
            get; set;
        }

        public IDbSet<APInvoiceType> APInvoiceTypes
        {
            get; set;
        }

        public IDbSet<APInvoiceEntity> APInvoiceEntities
        {
            get; set;
        }

        public IDbSet<APPayment> APPayments
        {
            get; set;
        }

        public IDbSet<APPaymentStatus> APPaymentStatus
        {
            get; set;
        }

        public IDbSet<APInvoicePayment> APInvoicePayments
        {
            get; set;
        }

        public IDbSet<CreditCardType> CreditCardTypes
        {
            get; set;
        }

        public IDbSet<AccountingTransferHeader> AccountingTransferHeaders { get; set; }
        public IDbSet<AccountingTransferLine> AccountingTransferLines { get; set; }
        public IDbSet<AccountingTransferType> AccountingTransferTypes { get; set; }
        public IDbSet<ARInvoiceTransferStatus> ARInvoiceTransferStatuses { get; set; }
        public IDbSet<APInvoiceTransferStatus> APInvoiceTransferStatuses { get; set; }
        public IDbSet<ExternalSystemsTablesCode> ExternalSystemsTablesCodes { get; set; }
        public IDbSet<ExternalSystemsMissingTranslation> ExternalSystemsMissingTranslations { get; set; }
        public IDbSet<ExternalSystemsSyncStatus> ExternalSystemsSyncStatuses { get; set; }
        public IDbSet<AccountingSystemsSetting> AccountingSystemsSettings { get; set; }
        public IDbSet<AccountingSystemsSyncStatus> AccountingSystemsSyncStatuses { get; set; }
        public IDbSet<QuickbooksSyncRequestTicket> QuickbooksSyncRequestTickets { get; set; }
        public IDbSet<ARInvoiceLineAction> ARInvoiceLineActions { get; set; }
        public IDbSet<ARPaymentTransferStatus> ARPaymentTransferStatuses { get; set; }
        public IDbSet<APPaymentTransferStatus> APPaymentTransferStatuses { get; set; }
        public IDbSet<SATInterface> SATInterfaces { get; set; }
        public IDbSet<SATInterfaceSetting> SATInterfaceSettings { get; set; }
        public IDbSet<SATPaymentMethod> SATPaymentMethods { get; set; }
        public IDbSet<BankAccountLite> BankAccountLites { get; set; }
        public IDbSet<SATTransferStatus> SATTransferStatus { get; set; }
        public IDbSet<SATInvoiceStatus> SATInvoiceStatus { get; set; }
        public IDbSet<ARInvoiceStocksStatus> ARInvoiceStocksStatus { get; set; }
        public IDbSet<ARInvoiceStock> ARInvoiceStocks { get; set; }
        public IDbSet<ARInvoiceStockLine> ARInvoiceStockLines { get; set; }
        public IDbSet<ARPaymentChequeReplica> ARPaymentChequeReplicas { get; set; }
        public IDbSet<ARPaymentChequeStatusReplica> ARPaymentChequeStatusReplicas { get; set; }
        public IDbSet<ARInvoiceChargesConstraint> ARInvoiceChargesConstraints { get; set; }
        public IDbSet<QBOGlobalTaxCalculation> QBOGlobalTaxCalculations { get; set; }
        public IDbSet<ARPaymentBankTranfer> ARPaymentBankTranfers { get; set; }

        public IDbSet<ARInvoiceAnalytic> ARInvoiceAnalytics { get; set; }

        public IDbSet<APInvoiceAnalytic> APInvoiceAnalytics { get; set; }
        public IDbSet<DigitalInvoicesCounterDataView> DigitalInvoicesCounterDataView { get; set; }
        public IDbSet<ControlForInvoiceLinesDataView> ControlForInvoiceLinesDataView { get; set; }
        public IDbSet<ConfirmationNumberStatus> ConfirmationNumberStatuses { get; set; }

        public void DetectChanges()
        {
            ChangeTracker.DetectChanges();
        }

        public int SaveChanges()
        {
            DetectChanges();
            return base.SaveChanges();
        }

        public DbConnection GetConnection()
        {
            return this.Database.Connection;
        }

        public DbContext GetActiveDbContext()
        {
            return this;
        }
    }
}
