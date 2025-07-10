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
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data; 
using Logitude.CRM.Data.EntityMapping;
using Devart.Data.Oracle.Entity.Configuration;

namespace Logitude.CRM.Data
{
   public class CRMContext: DbContextBase, ICRMContext
    {
        public CRMContext()
        {
            Database.SetInitializer<CRMContext>(null);    
			Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
			
        }

        public CRMContext(DbConnection conn)
            : base(conn,true)
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.AutoDetectChangesEnabled = false;
            Database.SetInitializer<CRMContext>(null);
			Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
        }

        public static ICRMContext GetContext(int tenant)
        {           
            GlobalDB currentDb;
			currentDb = GlobalDbHelper.GetGlobalDB(tenant);
			string dbConnectionInfo = currentDb.DBConnection;
            DbConnection connection =DatabaseInitializer.GetConnection(dbConnectionInfo);
            CRMContext context = new CRMContext(connection);
            return context;
        }
        public static ICRMContext GetSecContext(int tenant)
		{
			GlobalDB currentDb;
			currentDb = GlobalDbHelper.GetGlobalDB(tenant);
			string dbSeconderyConnectionInfo = currentDb.SecondaryAzureDBConnection;
			DbConnection connection = DatabaseInitializer.GetConnection(dbSeconderyConnectionInfo, null, null);
			CRMContext context = new CRMContext(connection);
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
                var config = OracleEntityProviderConfig.Instance;
                config.Workarounds.DisableQuoting = true;
                
                
            }
            Database.SetInitializer<CRMContext>(null);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
			
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
	
            modelBuilder.Configurations.Add(new CorrespondenceMap());
	
            modelBuilder.Configurations.Add(new CorrespondencesAttachmentMap());
	
            modelBuilder.Configurations.Add(new CRMFilterSettingMap());
	
            modelBuilder.Configurations.Add(new EmployeeGroupMap());
	
            modelBuilder.Configurations.Add(new EmployeeGroupLineMap());
	
            modelBuilder.Configurations.Add(new EscalationActionTimeIndicatorMap());
	
            modelBuilder.Configurations.Add(new EscalationPreDefinitionMap());
	
            modelBuilder.Configurations.Add(new OccasionMap());
	
            modelBuilder.Configurations.Add(new OccasionInviteeMap());
	
            modelBuilder.Configurations.Add(new OccasionStatusMap());
	
            modelBuilder.Configurations.Add(new OccasionTypeMap());
	
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
	
            modelBuilder.Configurations.Add(new SLAEscalationMap());
	
            modelBuilder.Configurations.Add(new SLAEscalationRecepientMap());
	
            modelBuilder.Configurations.Add(new SLAHeaderMap());
	
            modelBuilder.Configurations.Add(new SLALineMap());
	
            modelBuilder.Configurations.Add(new StageMap());
	
            modelBuilder.Configurations.Add(new SupportMailboxMap());
	
            modelBuilder.Configurations.Add(new TicketMap());
	
            modelBuilder.Configurations.Add(new TicketClassificationMap());
	
            modelBuilder.Configurations.Add(new TicketCreatedByTypeMap());
	
            modelBuilder.Configurations.Add(new TicketEscalationMap());
	
            modelBuilder.Configurations.Add(new TicketSeverityMap());
	
            modelBuilder.Configurations.Add(new TicketSourceMap());
	
            modelBuilder.Configurations.Add(new TicketStageMap());
	
            modelBuilder.Configurations.Add(new TicketTypeMap());
	
            modelBuilder.Configurations.Add(new TimeUnitMap());
					
			modelBuilder.Entity<Opportunity>().Property(x => x.ValueField).HasPrecision(18, 2);
				
			modelBuilder.Entity<OpportunityProduct>().Property(x => x.ChargeableWeight).HasPrecision(18, 2);
				
			modelBuilder.Entity<OpportunityProduct>().Property(x => x.TEU).HasPrecision(18, 2);
				
			modelBuilder.Entity<OpportunityProduct>().Property(x => x.Revenue).HasPrecision(18, 2);
				
			modelBuilder.Entity<OpportunityProductLocation>().Property(x => x.TEU).HasPrecision(18, 2);
				
			modelBuilder.Entity<OpportunityProductLocation>().Property(x => x.ChargeableWeight).HasPrecision(18, 2);
				
			modelBuilder.Entity<OpportunityProductLocation>().Property(x => x.Revenue).HasPrecision(18, 2);
						 
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
			modelBuilder.Configurations.Add(new BusinessUnitMap());
			modelBuilder.Configurations.Add(new UserPermittedProductMap());
			modelBuilder.Configurations.Add(new ParticipantMap());
            modelBuilder.Configurations.Add(new AirlineStatisticsMap());
			modelBuilder.Configurations.Add(new AWBDescriptionOfGoodsMap());
            modelBuilder.Configurations.Add(new CustomsShipperMap());
			//modelBuilder.Configurations.Add(new LogitudeMessagesTransmissionLogMap());

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
 

	 public IDbSet<Activity> Activities 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ActivityEmailRecipient> ActivityEmailRecipients 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ActivityInvitee> ActivityInvitees 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ActivityNote> ActivityNotes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ActivityOwnerHistory> ActivityOwnerHistories 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ActivityPriority> ActivityPriorities 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ActivityStatus> ActivityStatus 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ActivityTimeType> ActivityTimeTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ActivityType> ActivityTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CallType> CallTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<Correspondence> Correspondences 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CorrespondencesAttachment> CorrespondencesAttachments 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CRMFilterSetting> CRMFilterSettings 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<EmployeeGroup> EmployeeGroups 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<EmployeeGroupLine> EmployeeGroupLines 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<EscalationActionTimeIndicator> EscalationActionTimeIndicators 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<EscalationPreDefinition> EscalationPreDefinitions 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<Occasion> Occasions 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<OccasionInvitee> OccasionInvitees 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<OccasionStatus> OccasionStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<OccasionType> OccasionTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<Opportunity> Opportunities 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<OpportunityAdditionalService> OpportunityAdditionalServices 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<OpportunityClosingReason> OpportunityClosingReasons 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<OpportunityCompetitor> OpportunityCompetitors 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<OpportunityCompetitorProduct> OpportunityCompetitorProducts 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<OpportunityProduct> OpportunityProducts 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<OpportunityProductLocation> OpportunityProductLocations 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<OpportunityStage> OpportunityStages 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<OpportunityType> OpportunityTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<Questionnaire> Questionnaires 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<QuestionnaireAnswer> QuestionnaireAnswers 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<QuestionnaireAnswerLine> QuestionnaireAnswerLines 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<QuestionnaireQuestion> QuestionnaireQuestions 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<Rating> Ratings 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SLAEscalation> SLAEscalations 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SLAEscalationRecepient> SLAEscalationRecepients 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SLAHeader> SLAHeaders 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SLALine> SLALines 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<Stage> Stages 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<SupportMailbox> SupportMailboxes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<Ticket> Tickets 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<TicketClassification> TicketClassifications 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<TicketCreatedByType> TicketCreatedByTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<TicketEscalation> TicketEscalations 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<TicketSeverity> TicketSeverities 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<TicketSource> TicketSources 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<TicketStage> TicketStages 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<TicketType> TicketTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<TimeUnit> TimeUnits 
	 {
	      get; set;
	 
	 }
	 public IDbSet<OpportunityAnalytic> OpportunityAnalytics
	 {
	      get; set;	 
	 }
 }


}