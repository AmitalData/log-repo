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
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data; 
using Logitude.Accounting.Data.EntityMapping;

namespace Logitude.Accounting.Data
{
   public  partial  class AccountingContext: DbContextBase, IAccountingContext
    {
        public AccountingContext()
        {
            Database.SetInitializer<AccountingContext>(null);
			Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();
        }

        public AccountingContext(DbConnection conn)
            : base(conn,true)
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.AutoDetectChangesEnabled = false;
            Database.SetInitializer<AccountingContext>(null);
			Database.CommandTimeout = ApplicationAppInfo.GetDataBaseTimeOut();

        }

        public static IAccountingContext GetContext(int tenant)
        {           
            GlobalDB currentDb;
			currentDb = GlobalDbHelper.GetGlobalDB(tenant);
            string dbConnectionInfo = currentDb.DBConnection;
            DbConnection connection =DatabaseInitializer.GetConnection(dbConnectionInfo);
            AccountingContext context = new AccountingContext(connection);
			 
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
            Database.SetInitializer<AccountingContext>(null);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
			
            modelBuilder.Configurations.Add(new AccountingCompanyTypeMap());
	
            modelBuilder.Configurations.Add(new AccountingEntityMap());
	
            modelBuilder.Configurations.Add(new AccountingIntegrityCheckMap());
	
            modelBuilder.Configurations.Add(new AccountingNoteMap());
	
            modelBuilder.Configurations.Add(new AccountingPeriodMap());
	
            modelBuilder.Configurations.Add(new ARPaymentChequeMap());
	
            modelBuilder.Configurations.Add(new ARPaymentChequeStatusMap());
	
            modelBuilder.Configurations.Add(new AutomaticExternalRconcilMthodMap());
	
            modelBuilder.Configurations.Add(new AutomaticReconcileMap());
	
            modelBuilder.Configurations.Add(new AutomaticReconcileMethodMap());
	
            modelBuilder.Configurations.Add(new BankAccountMap());
	
            modelBuilder.Configurations.Add(new BankCodeMap());
	
            modelBuilder.Configurations.Add(new BankDepositMap());
	
            modelBuilder.Configurations.Add(new BankDepositLineMap());
	
            modelBuilder.Configurations.Add(new BankPageEntryTypeMap());
	
            modelBuilder.Configurations.Add(new CashBookMap());
	
            modelBuilder.Configurations.Add(new CashBookLineMap());
	
            modelBuilder.Configurations.Add(new CashBookTypeMap());
	
            modelBuilder.Configurations.Add(new Category1Map());
	
            modelBuilder.Configurations.Add(new Category2Map());
	
            modelBuilder.Configurations.Add(new Category3Map());
	
            modelBuilder.Configurations.Add(new Category4Map());
	
            modelBuilder.Configurations.Add(new Category5Map());
	
            modelBuilder.Configurations.Add(new ChartOfAccountMap());
	
            modelBuilder.Configurations.Add(new ChartOfAccountsTypeMap());
	
            modelBuilder.Configurations.Add(new ExternalPageAdditionalDataMap());
	
            modelBuilder.Configurations.Add(new ExternalReconciliationMap());
	
            modelBuilder.Configurations.Add(new ExternalReconciliationLineMap());
	
            modelBuilder.Configurations.Add(new FullAccountingSettingMap());
	
            modelBuilder.Configurations.Add(new GLAccountMap());
	
            modelBuilder.Configurations.Add(new GLAccountCounterMap());
	
            modelBuilder.Configurations.Add(new GLAccountCurrencyMap());
	
            modelBuilder.Configurations.Add(new GLAccountInterestPeriodMap());
	
            modelBuilder.Configurations.Add(new GLAccountMoreDataMap());
	
            modelBuilder.Configurations.Add(new GLAccountTotalByMonthMap());
	
            modelBuilder.Configurations.Add(new GLAccountTotalDateTypeMap());
	
            modelBuilder.Configurations.Add(new GLAccountTypeMap());
	
            modelBuilder.Configurations.Add(new GLAccountWithholdingTaxMap());
	
            modelBuilder.Configurations.Add(new IntegrityCheckStatusMap());
	
            modelBuilder.Configurations.Add(new InterestBasesPeriodMap());
	
            modelBuilder.Configurations.Add(new InterestBasesTypeMap());
	
            modelBuilder.Configurations.Add(new InterestEntityTypeMap());
	
            modelBuilder.Configurations.Add(new InterestReportMap());
	
            modelBuilder.Configurations.Add(new InterestReportLineMap());
	
            modelBuilder.Configurations.Add(new InterestReportLinesByDateMap());
	
            modelBuilder.Configurations.Add(new InterestReportStatuseMap());
	
            modelBuilder.Configurations.Add(new InterestTransactionMap());
	
            modelBuilder.Configurations.Add(new JournalMap());
	
            modelBuilder.Configurations.Add(new JournalActionTypeMap());
	
            modelBuilder.Configurations.Add(new JournalAdditionalDataMap());
	
            modelBuilder.Configurations.Add(new JournalExternalReconcileMap());
	
            modelBuilder.Configurations.Add(new JournalLineMap());
	
            modelBuilder.Configurations.Add(new JournalMoreDataMap());
	
            modelBuilder.Configurations.Add(new JournalReconcileMap());
	
            modelBuilder.Configurations.Add(new JournalStatusTypeMap());
	
            modelBuilder.Configurations.Add(new JournalTypeMap());
	
            modelBuilder.Configurations.Add(new LedgerTransactionMap());
	
            modelBuilder.Configurations.Add(new OpenFormatReportMap());
	
            modelBuilder.Configurations.Add(new OpenFormatReportStatusMap());
	
            modelBuilder.Configurations.Add(new PaymentChequeMap());
	
            modelBuilder.Configurations.Add(new PaymentChequeLineMap());
	
            modelBuilder.Configurations.Add(new PaymentChequeStatusMap());
	
            modelBuilder.Configurations.Add(new PeriodTypeMap());
	
            modelBuilder.Configurations.Add(new ReconcileExternalPageMap());
	
            modelBuilder.Configurations.Add(new ReconcileExternalPageLineMap());
	
            modelBuilder.Configurations.Add(new ReconcileExternalPageStatusMap());
	
            modelBuilder.Configurations.Add(new ReconcileMethodMap());
	
            modelBuilder.Configurations.Add(new ReconciliationMap());
	
            modelBuilder.Configurations.Add(new ReconciliationLineMap());
	
            modelBuilder.Configurations.Add(new RevaluationMap());
	
            modelBuilder.Configurations.Add(new RevaluationStatusMap());
	
            modelBuilder.Configurations.Add(new RevenueExpenseTypeMap());
	
            modelBuilder.Configurations.Add(new TaxDeductionReportMap());
	
            modelBuilder.Configurations.Add(new TaxDeductionReportStatusMap());
	
            modelBuilder.Configurations.Add(new TaxReportMap());
	
            modelBuilder.Configurations.Add(new TaxReportLineMap());
	
            modelBuilder.Configurations.Add(new TaxReportLineStatusMap());
	
            modelBuilder.Configurations.Add(new TaxReportLineTransmitStatusMap());
	
            modelBuilder.Configurations.Add(new TaxReportLineTypeMap());
	
            modelBuilder.Configurations.Add(new TaxReportStatusMap());
	
            modelBuilder.Configurations.Add(new TaxWithholdingAssessOfficeMap());
	
            modelBuilder.Configurations.Add(new TestEntityMap());
	
            modelBuilder.Configurations.Add(new VatReportStatusMap());
	
            modelBuilder.Configurations.Add(new WithholdingTaxDeductionTypeMap());
				
				
			modelBuilder.Entity<ARPaymentCheque>().Property(x => x.LocalAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<ARPaymentCheque>().Property(x => x.ForeignAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<ARPaymentCheque>().Property(x => x.ExchangeRate).HasPrecision(5, 3);
				
			modelBuilder.Entity<BankAccount>().Property(x => x.LastPageCloseBalance).HasPrecision(16, 2);
				
			modelBuilder.Entity<BankDeposit>().Property(x => x.LocalDepositAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<BankDeposit>().Property(x => x.ForeignAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<CashBook>().Property(x => x.TotalAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<ExternalPageAdditionalData>().Property(x => x.LastPageCloseBalance).HasPrecision(16, 2);
				
			modelBuilder.Entity<FullAccountingSetting>().Property(x => x.DefaultTaxWithholdPercentage).HasPrecision(16, 2);
				
			modelBuilder.Entity<GLAccount>().Property(x => x.InterestCreditLimit).HasPrecision(18, 2);
				
			modelBuilder.Entity<GLAccountInterestPeriod>().Property(x => x.StandardAddInterestPercent).HasPrecision(4, 2);
				
			modelBuilder.Entity<GLAccountInterestPeriod>().Property(x => x.ExceptionalAddInterestPercent).HasPrecision(4, 2);
				
			modelBuilder.Entity<GLAccountInterestPeriod>().Property(x => x.CreditAddInterestPercent).HasPrecision(4, 2);
				
			modelBuilder.Entity<GLAccountMoreData>().Property(x => x.BalanceInLocalCurrency).HasPrecision(16, 2);
				
			modelBuilder.Entity<GLAccountMoreData>().Property(x => x.LocalBalanceInDue).HasPrecision(16, 2);
				
			modelBuilder.Entity<GLAccountMoreData>().Property(x => x.TotalOpenChequesInLocalCur).HasPrecision(16, 2);
				
			modelBuilder.Entity<GLAccountMoreData>().Property(x => x.TotFutureOpenChequesInLocalCur).HasPrecision(16, 2);
				
			modelBuilder.Entity<GLAccountTotalByMonth>().Property(x => x.LocalAmountDebit).HasPrecision(16, 2);
				
			modelBuilder.Entity<GLAccountTotalByMonth>().Property(x => x.LocalAmountCredit).HasPrecision(16, 2);
				
			modelBuilder.Entity<GLAccountTotalByMonth>().Property(x => x.ForeignAmountDebit).HasPrecision(16, 2);
				
			modelBuilder.Entity<GLAccountTotalByMonth>().Property(x => x.ForeignAmountCredit).HasPrecision(16, 2);
				
			modelBuilder.Entity<InterestBasesPeriod>().Property(x => x.InterestRate).HasPrecision(4, 2);
				
			modelBuilder.Entity<InterestReport>().Property(x => x.TotalAmount).HasPrecision(18, 2);
				
			modelBuilder.Entity<InterestReport>().Property(x => x.OpenBalance).HasPrecision(18, 2);
				
			modelBuilder.Entity<InterestReport>().Property(x => x.CloseBalance).HasPrecision(18, 2);
				
			modelBuilder.Entity<InterestReport>().Property(x => x.InvoiceAmount).HasPrecision(18, 2);
				
			modelBuilder.Entity<InterestReport>().Property(x => x.GLAccountInterestCreditLimit).HasPrecision(18, 2);
				
			modelBuilder.Entity<InterestReportLinesByDate>().Property(x => x.TotalAmount).HasPrecision(18, 2);
				
			modelBuilder.Entity<InterestReportLinesByDate>().Property(x => x.AccumulatedAmount).HasPrecision(18, 2);
				
			modelBuilder.Entity<InterestReportLinesByDate>().Property(x => x.StandardInterestPercentage).HasPrecision(4, 2);
				
			modelBuilder.Entity<InterestReportLinesByDate>().Property(x => x.ExceptionalInterestPercentage).HasPrecision(4, 2);
				
			modelBuilder.Entity<InterestReportLinesByDate>().Property(x => x.CreditInterestPercentage).HasPrecision(4, 2);
				
			modelBuilder.Entity<InterestReportLinesByDate>().Property(x => x.StandardInterestAmount).HasPrecision(18, 2);
				
			modelBuilder.Entity<InterestReportLinesByDate>().Property(x => x.ExceptionalInterestAmount).HasPrecision(18, 2);
				
			modelBuilder.Entity<InterestReportLinesByDate>().Property(x => x.CreditInterestAmount).HasPrecision(18, 2);
				
			modelBuilder.Entity<InterestReportLinesByDate>().Property(x => x.CalculatedStandInterestAmount).HasPrecision(20, 4);
				
			modelBuilder.Entity<InterestReportLinesByDate>().Property(x => x.CalculatedExcepInterestAmount).HasPrecision(20, 4);
				
			modelBuilder.Entity<InterestReportLinesByDate>().Property(x => x.CalculatedCreditInterestAmount).HasPrecision(20, 4);
				
			modelBuilder.Entity<InterestTransaction>().Property(x => x.LocalAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<InterestTransaction>().Property(x => x.ForeignAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<JournalLine>().Property(x => x.LocalAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<JournalLine>().Property(x => x.ForeignAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<JournalLine>().Property(x => x.ExchangeRate).HasPrecision(16, 5);
				
			modelBuilder.Entity<JournalLine>().Property(x => x.ExternalOpenAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<JournalReconcile>().Property(x => x.ReconciliationAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<LedgerTransaction>().Property(x => x.LocalAmountDebit).HasPrecision(16, 2);
				
			modelBuilder.Entity<LedgerTransaction>().Property(x => x.LocalAmountCredit).HasPrecision(16, 2);
				
			modelBuilder.Entity<LedgerTransaction>().Property(x => x.ForeignAmountDebit).HasPrecision(16, 2);
				
			modelBuilder.Entity<LedgerTransaction>().Property(x => x.ForeignAmountCredit).HasPrecision(16, 2);
				
			modelBuilder.Entity<LedgerTransaction>().Property(x => x.ExchangeRate).HasPrecision(16, 5);
				
			modelBuilder.Entity<LedgerTransaction>().Property(x => x.OpenAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<LedgerTransaction>().Property(x => x.AmountToReconcile).HasPrecision(16, 2);
				
			modelBuilder.Entity<PaymentCheque>().Property(x => x.LocalAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<PaymentCheque>().Property(x => x.ForeignAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<PaymentCheque>().Property(x => x.ExchangeRate).HasPrecision(5, 3);
				
			modelBuilder.Entity<ReconcileExternalPageLine>().Property(x => x.DebitAmount).HasPrecision(15, 2);
				
			modelBuilder.Entity<ReconcileExternalPageLine>().Property(x => x.CreditAmount).HasPrecision(15, 2);
				
			modelBuilder.Entity<ReconciliationLine>().Property(x => x.ReconciliationAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<TaxReport>().Property(x => x.TaxableOutputAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<TaxReport>().Property(x => x.OutputTaxAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<TaxReport>().Property(x => x.TaxableOutputsWithDiffPercent).HasPrecision(16, 2);
				
			modelBuilder.Entity<TaxReport>().Property(x => x.OutputTaxAmountWithDiffPercent).HasPrecision(16, 2);
				
			modelBuilder.Entity<TaxReport>().Property(x => x.ExemptTaxableOutput).HasPrecision(16, 2);
				
			modelBuilder.Entity<TaxReport>().Property(x => x.OtherInputsTaxAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<TaxReport>().Property(x => x.EquipmentInputsTaxAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<TaxReport>().Property(x => x.AmountForPayRefund).HasPrecision(16, 2);
				
			modelBuilder.Entity<TaxReportLine>().Property(x => x.VatAmount).HasPrecision(16, 2);
				
			modelBuilder.Entity<TaxReportLine>().Property(x => x.VatableInvoiceAmount).HasPrecision(16, 2);
						 
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
            modelBuilder.Configurations.Add(new APPaymentMethodMap());
            modelBuilder.Configurations.Add(new APPaymentMap());
            modelBuilder.Configurations.Add(new APPaymentStatuMap());
            modelBuilder.Configurations.Add(new ARInvoiceEntityMap());
            modelBuilder.Configurations.Add(new ARInvoiceLineMap());
            modelBuilder.Configurations.Add(new ARInvoicePaymentMap());
            modelBuilder.Configurations.Add(new ARInvoiceMap());
            modelBuilder.Configurations.Add(new ARInvoiceStatuMap());
            modelBuilder.Configurations.Add(new ARInvoiceTotalVATMap());
            modelBuilder.Configurations.Add(new ARInvoiceTypeMap());
             
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
			modelBuilder.Configurations.Add(new ShipmentCommodityMap());
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
 

	 public IDbSet<AccountingCompanyType> AccountingCompanyTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<AccountingEntity> AccountingEntities 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<AccountingIntegrityCheck> AccountingIntegrityChecks 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<AccountingNote> AccountingNotes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<AccountingPeriod> AccountingPeriods 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ARPaymentCheque> ARPaymentCheques 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ARPaymentChequeStatus> ARPaymentChequeStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<AutomaticExternalRconcilMthod> AutomaticExternalRconcilMthods 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<AutomaticReconcile> AutomaticReconciles 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<AutomaticReconcileMethod> AutomaticReconcileMethods 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<BankAccount> BankAccounts 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<BankCode> BankCodes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<BankDeposit> BankDeposits 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<BankDepositLine> BankDepositLines 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<BankPageEntryType> BankPageEntryTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CashBook> CashBooks 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CashBookLine> CashBookLines 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<CashBookType> CashBookTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<Category1> Category1 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<Category2> Category2 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<Category3> Category3 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<Category4> Category4 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<Category5> Category5 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ChartOfAccount> ChartOfAccounts 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ChartOfAccountsType> ChartOfAccountsTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ExternalPageAdditionalData> ExternalPageAdditionalDatas 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ExternalReconciliation> ExternalReconciliations 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ExternalReconciliationLine> ExternalReconciliationLines 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<FullAccountingSetting> FullAccountingSettings 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<GLAccount> GLAccounts 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<GLAccountCounter> GLAccountCounters 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<GLAccountCurrency> GLAccountCurrencies 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<GLAccountInterestPeriod> GLAccountInterestPeriods 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<GLAccountMoreData> GLAccountMoreDatas 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<GLAccountTotalByMonth> GLAccountTotalByMonths 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<GLAccountTotalDateType> GLAccountTotalDateTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<GLAccountType> GLAccountTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<GLAccountWithholdingTax> GLAccountWithholdingTax 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<IntegrityCheckStatus> IntegrityCheckStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<InterestBasesPeriod> InterestBasesPeriods 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<InterestBasesType> InterestBasesTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<InterestEntityType> InterestEntityTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<InterestReport> InterestReports 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<InterestReportLine> InterestReportLines 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<InterestReportLinesByDate> InterestReportLinesByDates 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<InterestReportStatuse> InterestReportStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<InterestTransaction> InterestTransactions 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<Journal> Journals 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<JournalActionType> JournalActionTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<JournalAdditionalData> JournalAdditionalDatas 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<JournalExternalReconcile> JournalExternalReconciles 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<JournalLine> JournalLines 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<JournalMoreData> JournalMoreDatas 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<JournalReconcile> JournalReconciles 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<JournalStatusType> JournalStatusTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<JournalType> JournalTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<LedgerTransaction> LedgerTransactions 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<OpenFormatReport> OpenFormatReports 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<OpenFormatReportStatus> OpenFormatReportStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<PaymentCheque> PaymentCheques 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<PaymentChequeLine> PaymentChequeLines 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<PaymentChequeStatus> PaymentChequeStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<PeriodType> PeriodTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ReconcileExternalPage> ReconcileExternalPages 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ReconcileExternalPageLine> ReconcileExternalPageLines 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ReconcileExternalPageStatus> ReconcileExternalPageStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ReconcileMethod> ReconcileMethods 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<Reconciliation> Reconciliations 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<ReconciliationLine> ReconciliationLines 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<Revaluation> Revaluations 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<RevaluationStatus> RevaluationStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<RevenueExpenseType> RevenueExpenseTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<TaxDeductionReport> TaxDeductionReports 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<TaxDeductionReportStatus> TaxDeductionReportStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<TaxReport> TaxReports 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<TaxReportLine> TaxReportLines 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<TaxReportLineStatus> TaxReportLineStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<TaxReportLineTransmitStatus> TaxReportLineTransmitStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<TaxReportLineType> TaxReportLineTypes 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<TaxReportStatus> TaxReportStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<TaxWithholdingAssessOffice> TaxWithholdingAssessOffices 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<TestEntity> TestEntities 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<VatReportStatus> VatReportStatuses 
	 {
	      get; set;
	 
	 }
	
	 public IDbSet<WithholdingTaxDeductionType> WithholdingTaxDeductionTypes 
	 {
	      get; set;
	 
	 }
	  
 }


}