using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Transactions;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Mapping;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Mapping;
using Simplog.Data.InvoiceModel.Mapping;
using Simplog.Data.QuoteModel.Mapping;
using Simplog.Data.ShipmentModel.Mapping;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Mapping;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Linq;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel
{
    public class ShipmentsContext : DbContextBase, IShipmentsContext
    {
        public ShipmentsContext()
            : base("LogitudeStr")
        {
            Database.SetInitializer<ShipmentsContext>(null);
            Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
     
        }

        public ShipmentsContext(DbConnection conn)
            : base(conn,true)
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.AutoDetectChangesEnabled = false;
            Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
            Database.SetInitializer<ShipmentsContext>(null);
            
        }

        public static IShipmentsContext GetContext(int tenant)
        {
            GlobalDB currentDb;
            //using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            //{
                currentDb = GlobalDbHelper.GetGlobalDB(tenant);
            //}
            string dbConnectionInfo = currentDb.DBConnection;
            string dbSeconderyConnectionInfo = currentDb.SecondaryAzureDBConnection;

            DbConnection connection =DatabaseInitializer.GetConnection(dbConnectionInfo,dbSeconderyConnectionInfo);
            ShipmentsContext context = new ShipmentsContext(connection);
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
                ////config.QueryOptions.CaseInsensitiveComparison = true;
                ////config.QueryOptions.CaseInsensitiveLike = true;
                //modelBuilder.SetDefaultSchema("LOGITUDE_MAIN");
            }

            Database.SetInitializer<ShipmentsContext>(null);
            //string databasename = DatabaseInitializer.GetDatabaseName();
            //Database.DefaultConnectionFactory.CreateConnection(databasename);
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
            modelBuilder.Configurations.Add(new ContactLastLoginMap());
            modelBuilder.Configurations.Add(new SharedLogisticsContactLastLoginMap());
            modelBuilder.Configurations.Add(new SmallDocumentMap());
            modelBuilder.Configurations.Add(new CommunicationLogStepMap());
            modelBuilder.Configurations.Add(new SharedLogisticsInvitationStatusMap());
            modelBuilder.Configurations.Add(new ShipmentPackageItemMap());
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
            modelBuilder.Configurations.Add(new ShipmentCommodityMap());
            modelBuilder.Configurations.Add(new SpecialServicesTypeMap());
            modelBuilder.Configurations.Add(new MessagingStockMap());
            modelBuilder.Configurations.Add(new MessagingStockUsageHistoryMap());
            modelBuilder.Configurations.Add(new AccountingInformationIdentifierMap());
            modelBuilder.Configurations.Add(new ManifestStatusMap());
            modelBuilder.Configurations.Add(new AWBAdditionalHandlingInfoMap());
            modelBuilder.Configurations.Add(new ShipmentComputedFieldsMap());
            modelBuilder.Configurations.Add(new ParticipantMap());
            modelBuilder.Configurations.Add(new AirlineStatisticsMap());
            modelBuilder.Configurations.Add(new AWBDescriptionOfGoodsMap());
            modelBuilder.Configurations.Add(new OceanInsightsRequestMap());
            modelBuilder.Configurations.Add(new OceanInsightsRequestsCountMap());
            modelBuilder.Configurations.Add(new OceanInsightsStatusesMap());
            modelBuilder.Configurations.Add(new LogitudeMessagesTransmissionLogMap());

            modelBuilder.Configurations.Add(new OtherParticipantIdMap());
            modelBuilder.Configurations.Add(new ShipmentAdditionalCloudDataMap());

            modelBuilder.Configurations.Add(new FBLStockMap());

            modelBuilder.Configurations.Add(new CustomsTransmissionsStatusMap());
            modelBuilder.Configurations.Add(new OBLTypeMap());
            modelBuilder.Configurations.Add(new ShipmentCustomsMessageTypeMap());

            modelBuilder.Configurations.Add(new INTTRAStatusMap());
            modelBuilder.Configurations.Add(new INTTRABookingTransStatusMap());
            modelBuilder.Configurations.Add(new INTTRABookingStatusMap());
            modelBuilder.Configurations.Add(new INTTRASIStatusMap());
            modelBuilder.Configurations.Add(new ShipmentContainerStatusMap());
            modelBuilder.Configurations.Add(new PickUpDeliveryTransportModeMap());
            modelBuilder.Configurations.Add(new INTTRADocumentTypeMap());
            modelBuilder.Configurations.Add(new ShipmentPackageHarmonizeMap());
            modelBuilder.Configurations.Add(new PickUpDeliveryPackageHarmonizeMap());
            modelBuilder.Configurations.Add(new CustomsShipperMap());
            modelBuilder.Configurations.Add(new HarmonizeCodeMap());
            modelBuilder.Configurations.Add(new CustomsTransferHeaderMap());
            modelBuilder.Configurations.Add(new CustomsTransferLineMap());
            modelBuilder.Configurations.Add(new CustomsTransferTypeMap());
            modelBuilder.Configurations.Add(new ShipmentSubTypeMap());
            modelBuilder.Configurations.Add(new ShipmentStoragePricingMap());
            modelBuilder.Configurations.Add(new ShipmentProductItemMap());
            modelBuilder.Configurations.Add(new ContainerStatusMap());
            modelBuilder.Configurations.Add(new ContainerStatusSourceMap());
            modelBuilder.Configurations.Add(new ShipmentUnassignedFieldMap());

            base.OnModelCreating(modelBuilder);
        }

        public IDbSet<Shipment> Shipments { get; set; }
        public IDbSet<ShipmentType> ShipmentTypes { get; set; }
        public IDbSet<ShipmentMasterData> ShipmentMasterDatas { get; set; }
        public IDbSet<ShipmentReceivable> ShipmentReceivables { get; set; }
        public IDbSet<ShipmentPickUpDelivery> ShipmentPickUpDeliveries { get; set; }
        public IDbSet<PackageType> PackageTypes { get; set; }
        public IDbSet<ShipmentPackage> ShipmentPackages { get; set; }
        public IDbSet<InsideShipmentPackage> InsideShipmentPackages { get; set; }
        public IDbSet<ShipmentPickUpDeliveryPackage> ShipmentPickUpDeliveryPackages { get; set; }
        public IDbSet<ShipmentOrderPackage> ShipmentOrderPackages { get; set; }
        public IDbSet<ShipmentPayableLineStatus> ShipmentPayableLineStatus { get; set; }
        public IDbSet<ShipmentPayable> ShipmentPayables { get; set; }
        public IDbSet<ShipmentReceivableLineStatus> ShipmentReceivableLineStatus { get; set; }
        public IDbSet<ShipmentReceivableStatus> ShipmentReceivableStatus { get; set; }
        public IDbSet<ShipmentPayableStatus> ShipmentPayableStatus { get; set; }
        public IDbSet<ShipmentCustomerType> ShipmentCustomerTypes { get; set; }
        public IDbSet<PickUpDeliveryType> PickUpDeliveryTypes { get; set; }
        public IDbSet<PickUpDeliveryFromToType> PickUpDeliveryFromToTypes { get; set; }
        public IDbSet<ShipmentAWBPrintOnly> ShipmentAWBPrintOnlies { get; set; }
        public IDbSet<NextLeg> NextLegs { get; set; }
        public IDbSet<ShipmentPayableAmountType> ShipmentPayableAmountTypes { get; set; }
        public IDbSet<ShipmentLevel> ShipmentLevels { get; set; }
        public IDbSet<AWBChargesCode> AWBChargeCodes { get; set; }
        public IDbSet<AWBSpecialHandlingCode> AWBHandlingCodes { get; set; }
        public IDbSet<FWBStatus> FWBStatus { get; set; }
        public IDbSet<FHLStatus> FHLStatus { get; set; }
        public IDbSet<AWBStatus> AWBStatus { get; set; }
        public IDbSet<ShipmentCarrierStatus> ShipmentCarrierStatuses { get; set; }
        public IDbSet<AWBOCI> AWBOCIs { get; set; }
        public IDbSet<AWBCustomsInformation> AWBCustomsInformations { get; set; }
        public IDbSet<AWBInformation> AWBInformations { get; set; }
        public IDbSet<ShipmentPackageItem> ShipmentPackageItems { get; set; }
        public IDbSet<ShipmentCommodity> ShipmentCommodities { get; set; }
        public IDbSet<MessagingStock> MessagingStocks { get; set; }
        public IDbSet<MessagingStockUsageHistory> MessagingStockUsageHistories { get; set; }
        public IDbSet<AccountingInformationIdentifier> AccountingInformationIdentifiers { get; set; }
        public IDbSet<SpecialServicesType> SpecialServicesTypes { get; set; }
        public IDbSet<ManifestStatus> ManifestStatus { get; set; }
        public IDbSet<AWBAdditionalHandlingInfo> AWBAdditionalHandlingInfos { get; set; }
        public IDbSet<ShipmentComputedFields> ShipmentComputedFields { get; set; }
        public IDbSet<ShipmentAdditionalCloudData> ShipmentAdditionalCloudDatas { get; set; }
        public IDbSet<OceanInsightsRequestsCount> OceanInsightsRequestsCounts { get; set; }
        public IDbSet<OceanInsightsRequest> OceanInsightsRequests { get; set; }
        public IDbSet<LogitudeOceanInsightsRequest> LogitudeOceanInsightsRequests { get; set; }
        public IDbSet<LogitudeOceanInsightsResponse> LogitudeOceanInsightsResponses { get; set; }
        public IDbSet<OceanInsightsStatuses> OceanInsightsStatuses { get; set; }
        public IDbSet<OtherParticipantId> OtherParticipantIds { get; set; }
        public IDbSet<CustomsTransmissionsStatus> CustomsTransmissionsStatus { get; set; }
        public IDbSet<OBLType> OBLTypes { get; set; }
        public IDbSet<ShipmentCustomsMessageType> ShipmentCustomsMessageTypes { get; set; }
        public IDbSet<ShipmentCustomsTransmission> ShipmentCustomsTransmissions { get; set; }
        public IDbSet<INTTRAStatus> INTTRAStatuses { get; set; }
        public IDbSet<INTTRASIStatus> INTTRASIStatus { get; set; }
        public IDbSet<INTTRABookingStatus> INTTRABookingStatuses { get; set; }
        public IDbSet<INTTRABookingTransStatus> INTTRABookingTransStatuses { get; set; }
        public IDbSet<ShipmentContainerStatus> ShipmentContainerStatuses { get; set; }
        public IDbSet<PickUpDeliveryTransportMode> PickUpDeliveryTransportModes { get; set; }
        public IDbSet<INTTRADocumentType> INTTRADocumentTypes { get; set; }

        public IDbSet<ShipmentPackageHarmonize> ShipmentPackageHarmonizes { get; set; }
        public IDbSet<PickUpDeliveryPackageHarmonize> PickUpDeliveryPackageHarmonizes { get; set; }
        public IDbSet<HarmonizeCode> HarmonizeCodes { get; set; }
        public IDbSet<ShipmentSubType> ShipmentSubTypes { get; set; }
        public IDbSet<ShipmentStoragePricing> ShipmentStoragePricings { get; set; }
        public IDbSet<ShipmentProductItem> ShipmentProductItems { get; set; }
        public IDbSet<ShipmentUnassignedField> ShipmentUnassignedFields { get; set; }
        public IDbSet<Container> Containers { get; set; }
        public IDbSet<ContainerStatus> ContainerStatuses { get; set; }
        public IDbSet<ContainerStatusSource> ContainerStatusSources { get; set; }
        public IDbSet<PayableProratedAmount> PayableProratedAmounts { get; set; }


        [DbFunction("ShipmentsContext", "udf_ShipmentSearch")]
        public IQueryable<ShipmentDataView> ShipmentSearch(string SearchFields)
        {
            var result = Database.SqlQuery<ShipmentDataView>("Select * from [dbo].[udf_ShipmentSearch](" + SearchFields + ")").AsQueryable(); 
            return result; 
        }
        public IQueryable<TOutput> FunctionTableValue<TOutput>(string functionName, SqlParameter[] parameters)
        {
            parameters = parameters ?? new SqlParameter[] { };

            string commandText = String.Format("SELECT * FROM dbo.{0}", String.Format("{0}({1})", functionName, String.Join(",", parameters.Select(x => x.ParameterName))));

            return ObjectContext.ExecuteStoreQuery<TOutput>(commandText, parameters).AsEnumerable().AsQueryable();
        }
        private ObjectContext ObjectContext
        {
            get { return (this as IObjectContextAdapter).ObjectContext; }
        }

        public IDbSet<FBLStock> FBLStocks
        {
            get;
            set;
        }

        public IDbSet<ShipmentAssembly> ShipmentAssemblies
        {
            get;
            set;
        }

        public IDbSet<CustomsTransferType> CustomsTransferTypes
        {
            get;
            set;
        }

        public IDbSet<CustomsTransferLine> CustomsTransferLines
        {
            get;
            set;
        }

        public IDbSet<CustomsTransferHeader> CustomsTransferHeaders
        {
            get;
            set;
        }
        public IDbSet<ARInvoice> ARInvoicesForReports
        {
            get; set;
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
