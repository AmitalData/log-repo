using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data.Entity.Validation;
using System.Data.Entity.ModelConfiguration.Conventions;
using Simplog.Data.CommonDataModel.Mapping;
using Simplog.Data.InvoiceModel.Mapping;
using Simplog.Data.InfrastructureModel.Mapping;
using Simplog.Data.ShipmentsModel.Mapping;
using Simplog.Data.QuoteModel.Mapping;
using Amital.QuoteOPM.Data.EntityMapping; 
using Amital.QuoteOPM.Data; 
using Amital.QuoteOPM.Data.EntityPOCOs;

namespace Amital.QuoteOPM.Data
{
   public partial class QuoteOPMContext: DbContextBase, IQuoteOPMContext
    {
        public QuoteOPMContext()
        {
            Database.SetInitializer<QuoteOPMContext>(null);            
        }

        public QuoteOPMContext(DbConnection conn)
            : base(conn,true)
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.AutoDetectChangesEnabled = false;
            Database.SetInitializer<QuoteOPMContext>(null);
        }

        public static IQuoteOPMContext GetContext(int tenant)
        {           
            GlobalDB currentDb;
			currentDb = GlobalDbHelper.GetGlobalDB(tenant);
			string dbConnectionInfo = currentDb.DBConnection;
            DbConnection connection =DatabaseInitializer.GetConnection(dbConnectionInfo);
            QuoteOPMContext context = new QuoteOPMContext(connection);
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
                config.Workarounds.IgnoreSchemaName = true;
                
            }

            Database.SetInitializer<QuoteOPMContext>(null);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
			
            modelBuilder.Configurations.Add(new BorderOPTypeMap());
	
            modelBuilder.Configurations.Add(new MarkUpOPTypeMap());
	
            modelBuilder.Configurations.Add(new QuoteOPMap());
	
            modelBuilder.Configurations.Add(new QuoteOPChargeMap());
	
            modelBuilder.Configurations.Add(new QuoteOPClosingReasonMap());
	
            modelBuilder.Configurations.Add(new QuoteOPComputedFieldMap());
	
            modelBuilder.Configurations.Add(new QuoteOPCostChargeMap());
	
            modelBuilder.Configurations.Add(new QuoteOPCustomerTypeMap());
	
            modelBuilder.Configurations.Add(new QuoteopPackageMap());
	
            modelBuilder.Configurations.Add(new QuoteOPPriceStepsMap());
	
            modelBuilder.Configurations.Add(new QuoteOPRatingMap());
	
            modelBuilder.Configurations.Add(new QuoteOPSaleChargeMap());
	
            modelBuilder.Configurations.Add(new QuoteOPSalesTotalMap());
	
            modelBuilder.Configurations.Add(new QuoteOPSettingMap());
	
            modelBuilder.Configurations.Add(new QuoteOPStageMap());
	
            modelBuilder.Configurations.Add(new QuoteOPTemplateMap());
	
            modelBuilder.Configurations.Add(new QuoteOPTemplateSectionMap());
	
            modelBuilder.Configurations.Add(new QuoteOPTemplateSectionTypeMap());
	
            modelBuilder.Configurations.Add(new QuoteOPTemplateSettingMap());
	
            modelBuilder.Configurations.Add(new QuoteOPTemplateTableDesignMap());
	
            modelBuilder.Configurations.Add(new QuoteOPTemplateTextCodeMap());
	
            modelBuilder.Configurations.Add(new QuoteOPTemplateTextDesignMap());
	
            modelBuilder.Configurations.Add(new QuoteOPTotalVATMap());
	
            modelBuilder.Configurations.Add(new QuoteOPTypeMap());
	
            modelBuilder.Configurations.Add(new QuoteOPVATsTotalMap());
	
						  #region
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
            modelBuilder.Configurations.Add(new AWBChargesCodeMap());
            modelBuilder.Configurations.Add(new AWBSpecialHandlingCodeMap());
            modelBuilder.Configurations.Add(new AWBStatuMap());
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
            modelBuilder.Configurations.Add(new LeadSourceMap());
            modelBuilder.Configurations.Add(new IndustryMap());
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
			modelBuilder.Configurations.Add(new UserPermittedProductMap());
			modelBuilder.Configurations.Add(new ParticipantMap());
            modelBuilder.Configurations.Add(new AirlineStatisticsMap());
			modelBuilder.Configurations.Add(new AWBDescriptionOfGoodsMap());
			modelBuilder.Configurations.Add(new LogitudeMessagesTransmissionLogMap());
			            modelBuilder.Configurations.Add(new CustomsShipperMap());
            #endregion 
            base.OnModelCreating(modelBuilder);
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

 
	 public IDbSet<BorderOPType> BorderOPTypes 
	 {
	      get; set;
	 
	 }
		 public IDbSet<MarkUpOPType> MarkUpOPTypes 
	 {
	      get; set;
	 
	 }
		 public IDbSet<QuoteOP> QuoteOPs 
	 {
	      get; set;
	 
	 }
		 public IDbSet<QuoteOPCharge> QuoteOPCharges 
	 {
	      get; set;
	 
	 }
		 public IDbSet<QuoteOPClosingReason> QuoteOPClosingReasons 
	 {
	      get; set;
	 
	 }
		 public IDbSet<QuoteOPComputedField> QuoteOPComputedFields 
	 {
	      get; set;
	 
	 }
		 public IDbSet<QuoteOPCustomerType> QuoteOPCustomerTypes 
	 {
	      get; set;
	 
	 }
		 public IDbSet<QuoteopPackage> QuoteopPackages 
	 {
	      get; set;
	 
	 }
		 public IDbSet<QuoteOPPriceSteps> QuoteOPPriceSteps 
	 {
	      get; set;
	 
	 }
		 public IDbSet<QuoteOPRating> QuoteOPRatings 
	 {
	      get; set;
	 
	 }
		 public IDbSet<QuoteOPSetting> QuoteOPSettings 
	 {
	      get; set;
	 
	 }
		 public IDbSet<QuoteOPStage> QuoteOPStages 
	 {
	      get; set;
	 
	 }
		 public IDbSet<QuoteOPTemplate> QuoteOPTemplates 
	 {
	      get; set;
	 
	 }
		 public IDbSet<QuoteOPTemplateSection> QuoteOPTemplateSections 
	 {
	      get; set;
	 
	 }
		 public IDbSet<QuoteOPTemplateSectionType> QuoteOPTemplateSectionTypes 
	 {
	      get; set;
	 
	 }
		 public IDbSet<QuoteOPTemplateSetting> QuoteOPTemplateSettings 
	 {
	      get; set;
	 
	 }
		 public IDbSet<QuoteOPTemplateTableDesign> QuoteOPTemplateTableDesigns 
	 {
	      get; set;
	 
	 }
		 public IDbSet<QuoteOPTemplateTextCode> QuoteOPTemplateTextCodes 
	 {
	      get; set;
	 
	 }
		 public IDbSet<QuoteOPTemplateTextDesign> QuoteOPTemplateTextDesigns 
	 {
	      get; set;
	 
	 }
		 public IDbSet<QuoteOPTotalVAT> QuoteOPTotalVATs 
	 {
	      get; set;
	 
	 }
		 public IDbSet<QuoteOPType> QuoteOPTypes 
	 {
	      get; set;
	 
	 }
	  
 }


}