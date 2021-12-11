using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel.DomainServices;
using WebFreight.Web.Security;
using Simplog.Global.Data.GlobalModel;
using Logitude.Server.Tools.Counters;
using Logitude.CRM.Data.Repsitories;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL.EntityQueryServices;
using Simplog.Data.CommonDataModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.GlobalModel.Tools.TraceEvents;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Data.Helpers;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.Data.Repositories;
using Logitude.TariffModule.Data;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.EntityOtherServices;
using Logitude.BL.QuoteModel.EntityQueries;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace WebFreight.Web.InfrastructureModel
{
    public class SignUpClass
    {
        static int tenant;

        #region Repository and Query Definitions
        static TariffSettingRepository tariffSettingRepository;
        static AccountingSettingRepository accountingSettingsRepository;
        static SATInterfaceSettingRepository sATInterfaceSettingRepository;
        static CustomsInterfaceSettingRepository customsInterfaceSettingRepository;
        static SharedLogisticsSettingRepository sharedLogisticsSettingRepository;
        static TenantRepository tenantRepository;
        static GlobalTenantRepository globalTenantRepository;
        static BranchRepository branchRepository;
        static DepartmentRepository departmentRepository;
        static ObjectTableRepository objectTableRepository;
        static TextCodeRepository textCodeRepository;
        static ObjectFieldRepository objectFieldsRepository;
        static ObjectFieldValidationRepository objectFieldValidationRepository;
        static RoleRepository roleRepository;
        static FeatureRepository featureRepository;
        static RoleFeatureRepository roleFeatureRepository;
        static UserRepository userRepository;
        static CounterRepository counterRepository;
        static CounterDefinitionRepository counterDefinitionRepository;
        static TenantSettingRepository tenantSettingRepository;
        static ObjectTableTabRepository objectTableTabRepository;
        static ObjectTableHelperControlRepository objectTableHelperControlRepository;
        static ScreensRepository screensRepository;
        static ScreenFieldsRepository screenFieldsRepository;
        static ObjectTableRuleRepository objectTableRuleRepository;
        static ObjectTableRuleFieldRepository objectTableRuleFieldRepository;
        static QueryRepository queryRepository;
        static QueryColumnRepository queryColumnRepository;
        static AdvancedQueryFilterRepository advancedQueryFilterRepository;
        static TranslationHeaderRepository translationHeaderRepository;
        static PaymentTermRepository paymentTermRepository;
        static DocumentTypeRepository documentTypeRepository;
        static DocumentTypeCopyRepository documentTypeCopyRepository;
        static DocumentTypeTemplateRepository documentTypeTemplateRepository;
        static MenusTableRepository menusTableRepository;
        static EntityStatusRepository entityStatusRepository;
        static EventTypeRepository eventTypeRepository;
        static MeasurementRepository measurementRepository;
        static VatTypeRepository vatTypeRepository;
        static ChargesTypeRepository chargesTypeRepository;
        static ChargesGroupRepository chargesGroupRepository;
        static RankRepository rankRepository;
        static PackageTypeRepository packageTypeRepository;
        static DocumentTypeCustomFieldRepository documentTypeCustomFieldRepository;
        static AccountRepository accountRepository;
        static VatTypePercentageRepository vatTypePercentageRepository;
        static CreditCardTypeRepository creditCardTypeRepository;
        static MoveTypeRepository moveTypeRepository;
        static LeadSourceRepository leadSourceRepository;
        static StageRepository stageRepository;
        static IndustryRepository industryRepository;
        static AdditionalServiceRepository additionalServiceRepository;
        static OpportunityClosingReasonRepository closingReasonRepository;
        static QuoteStageRepository quoteStageRepository;
        static OpportunityTypeRepository opportunityTypeRepository;
        static DocumentsMetaDataTypeRepository documentsMetaDataTypeRepository;
        static AccountingPaymentMethodRepository PaymentMethodRepository;
        static QuoteClosingReasonRepository quoteClosingReasonRepository;
        static ShipmentSubTypeRepository shipmentSubTypeRepository;

        // Tariff 
        static PriceStepRepository priceStepRepository;
        static TariffProductRepository tariffProductRepository;

        //Tickets 
        static TicketTypeRepository ticketTypeRepository;
        static TicketStageRepository ticketStageRepository;
        static TicketSeverityRepository ticketSeverityRepository;
        static TicketClassificationRepository ticketClassificationRepository;

        //SLA 
        static BusinessHourRepository businessHourRepository;
        static SLAHeaderRepository slaHeaderRepository;
        static SLALineRepository slaLineRepository;
        static WithholdingTaxDeductionTypeRepository withholdingTaxDeductionTypeRepository;
        private static AdvancedQueryFilterQuery advancedQueryFilterQuery;
        private static ChargesTypeQuery chargesTypeQuery;
        private static ChargesGroupQuery chargesGroupQuery;
        private static EntityStatusQuery entityStatusQuery;
        private static EventTypeQuery eventTypeQuery;
        private static MenusTableQuery menusTableQuery;
        private static ObjectFieldQuery objectFieldsQuery;
        private static DocumentTypeQuery documentTypeQuery;
        private static MeasurementQuery measurementQuery;
        private static PackageTypeQuery packageTypeQuery;
        private static ObjectTableHelperControlQuery objectTableHelperControlQuery;
        private static ObjectTableRuleFieldQuery objectTableRuleFieldQuery;
        private static ObjectTableTabQuery objectTableTabQuery;
        private static QueryColumnQuery queryColumnQuery;
        private static QueryQuery queryQuery;
        private static ScreenFieldsQuery screenFieldsQuery;
        private static ScreensQuery screensQuery;
        private static TenantSettingQuery tenantSettingQuery;
        private static CreditCardTypeQuery creditCardTypeQuery;
        private static MoveTypeQuery moveTypeQuery;
        private static LeadSourceQuery leadSourceQuery;
        private static StageQueryService stageQuery;
        private static IndustryQuery industryQuery;
        private static AdditionalServiceQuery additionalServiceQuery;
        private static OpportunityClosingReasonQueryService closingReasonQuery;
        private static CustomsRequiredFieldRepository customsRequiredFieldRepository;
        private static DocumentTypeCustomsDataRepository documentTypeCustomsDataRepository;
        private static QuoteClosingReasonQuery quoteClosingReasonQuery;
        private static ShipmentSubTypeQuery shipmentSubTypeQuery;

        public static ScreenFieldsRepository ScreenFieldsRepository
        {
            get { return screenFieldsRepository; }
            set { screenFieldsRepository = value; }
        }

        public static ObjectFieldValidationRepository ObjectFieldValidationRepository
        {
            get { return objectFieldValidationRepository; }
            set { objectFieldValidationRepository = value; }
        }

        public static RoleFeatureRepository RoleFeatureRepository
        {
            get { return roleFeatureRepository; }
            set { roleFeatureRepository = value; }
        }

        public static ObjectTableTabRepository ObjectTableTabRepository
        {
            get { return objectTableTabRepository; }
            set { objectTableTabRepository = value; }
        }

        public static ObjectTableHelperControlRepository ObjectTableHelperControlRepository
        {
            get { return objectTableHelperControlRepository; }
            set { objectTableHelperControlRepository = value; }
        }

        public static ObjectTableRuleFieldRepository ObjectTableRuleFieldRepository
        {
            get { return objectTableRuleFieldRepository; }
            set { objectTableRuleFieldRepository = value; }
        }
        #endregion

        public static CustomsRequiredFieldRepository CustomsRequiredFieldRepository
        {
            get { return customsRequiredFieldRepository; }
            set { customsRequiredFieldRepository = value; }
        }
   
        public static DocumentTypeCustomsDataRepository DocumentTypeCustomsDataRepository
        {
            get { return documentTypeCustomsDataRepository; }
            set { documentTypeCustomsDataRepository = value; }
        }


        static FullAccountingSettingRepository fullAccountingSettingsRepository;
        static BankCodeRepository bankCodeRepository;
        public static TaxWithholdingAssessOfficeRepository taxWithholdingAssessOfficeRepository;
        public static void InitializeRepositories(int theTenant)
        {
            #region Repositories and Queries
            tariffSettingRepository = new TariffSettingRepository(theTenant);
            sATInterfaceSettingRepository = new SATInterfaceSettingRepository(theTenant);
            accountingSettingsRepository = new AccountingSettingRepository(theTenant);
            customsInterfaceSettingRepository = new CustomsInterfaceSettingRepository(theTenant);
            sharedLogisticsSettingRepository = new SharedLogisticsSettingRepository(theTenant);
            branchRepository = new BranchRepository(theTenant);
            departmentRepository = new DepartmentRepository(theTenant);
            objectTableRepository = new ObjectTableRepository(theTenant);
            textCodeRepository = new TextCodeRepository(theTenant);
            objectFieldsRepository = new ObjectFieldRepository(theTenant);
            ObjectFieldValidationRepository = new ObjectFieldValidationRepository(theTenant);
            roleRepository = new RoleRepository(theTenant);
            featureRepository = new FeatureRepository(theTenant);
            RoleFeatureRepository = new RoleFeatureRepository(theTenant);
            userRepository = new UserRepository(theTenant);
            counterRepository = new CounterRepository(theTenant);
            counterDefinitionRepository = new CounterDefinitionRepository(theTenant);
            tenantSettingRepository = new TenantSettingRepository(theTenant);
            ObjectTableTabRepository = new ObjectTableTabRepository(theTenant);
            ObjectTableHelperControlRepository = new ObjectTableHelperControlRepository(theTenant);
            screensRepository = new ScreensRepository(theTenant);
            ScreenFieldsRepository = new ScreenFieldsRepository(theTenant);
            objectTableRuleRepository = new ObjectTableRuleRepository(theTenant);
            ObjectTableRuleFieldRepository = new ObjectTableRuleFieldRepository(theTenant);
            queryRepository = new QueryRepository(theTenant);
            queryColumnRepository = new QueryColumnRepository(theTenant);
            advancedQueryFilterRepository = new AdvancedQueryFilterRepository(theTenant);
            translationHeaderRepository = new TranslationHeaderRepository(theTenant);
            paymentTermRepository = new PaymentTermRepository(theTenant);
            documentTypeRepository = new DocumentTypeRepository(theTenant);
            documentTypeCopyRepository = new DocumentTypeCopyRepository(theTenant);
            documentTypeTemplateRepository = new DocumentTypeTemplateRepository(theTenant);
            menusTableRepository = new MenusTableRepository(theTenant);
            entityStatusRepository = new EntityStatusRepository(theTenant);
            eventTypeRepository = new EventTypeRepository(theTenant);
            measurementRepository = new MeasurementRepository(theTenant);
            vatTypeRepository = new VatTypeRepository(theTenant);
            chargesTypeRepository = new ChargesTypeRepository(theTenant);
            chargesGroupRepository = new ChargesGroupRepository(theTenant);
            rankRepository = new RankRepository(theTenant);
            packageTypeRepository = new PackageTypeRepository(theTenant);
            documentTypeCustomFieldRepository = new DocumentTypeCustomFieldRepository(theTenant);
            accountRepository = new AccountRepository(theTenant);
            vatTypePercentageRepository = new VatTypePercentageRepository(theTenant);
            creditCardTypeRepository = new CreditCardTypeRepository(theTenant);
            moveTypeRepository = new MoveTypeRepository(theTenant);
            leadSourceRepository = new LeadSourceRepository(theTenant);
            stageRepository = new StageRepository(theTenant);
            industryRepository = new IndustryRepository(theTenant);
            additionalServiceRepository = new AdditionalServiceRepository(theTenant);
            closingReasonRepository = new OpportunityClosingReasonRepository(theTenant);
            quoteStageRepository = new QuoteStageRepository(theTenant);
            opportunityTypeRepository = new OpportunityTypeRepository(theTenant);
            customsRequiredFieldRepository = new Logitude.Customs.Data.Repsitories.CustomsRequiredFieldRepository(theTenant);
            documentTypeCustomsDataRepository = new Logitude.Customs.Data.Repsitories.DocumentTypeCustomsDataRepository(theTenant);
            documentsMetaDataTypeRepository = new DocumentsMetaDataTypeRepository(theTenant);
            PaymentMethodRepository = new Simplog.Data.InvoiceModel.Repositories.AccountingPaymentMethodRepository(theTenant);
            quoteClosingReasonRepository = new QuoteClosingReasonRepository(theTenant);
            shipmentSubTypeRepository = new ShipmentSubTypeRepository(theTenant);

            //Tariff 
            priceStepRepository = new PriceStepRepository(theTenant);
            tariffProductRepository = new TariffProductRepository(theTenant);

            //Tickets 
            ticketTypeRepository = new TicketTypeRepository(theTenant);
            ticketStageRepository = new TicketStageRepository(theTenant);
            ticketSeverityRepository = new TicketSeverityRepository(theTenant);
            ticketClassificationRepository = new TicketClassificationRepository(theTenant);

            // SLA 
            businessHourRepository = new BusinessHourRepository(theTenant);
            slaHeaderRepository = new SLAHeaderRepository(theTenant);
            slaLineRepository = new SLALineRepository(theTenant);
            withholdingTaxDeductionTypeRepository = new WithholdingTaxDeductionTypeRepository(theTenant);
            objectFieldsQuery = new ObjectFieldQuery(objectFieldsRepository);
            screensQuery = new ScreensQuery(screensRepository);
            queryQuery = new QueryQuery(queryRepository);
            queryColumnQuery = new QueryColumnQuery(queryColumnRepository);
            menusTableQuery = new MenusTableQuery(menusTableRepository);
            entityStatusQuery = new EntityStatusQuery(entityStatusRepository);
            eventTypeQuery = new EventTypeQuery(eventTypeRepository);
            advancedQueryFilterQuery = new AdvancedQueryFilterQuery(theTenant);
            chargesTypeQuery = new ChargesTypeQuery(theTenant);
            chargesGroupQuery = new ChargesGroupQuery(theTenant);
            documentTypeQuery = new DocumentTypeQuery(theTenant);
            measurementQuery = new MeasurementQuery(theTenant);
            packageTypeQuery = new PackageTypeQuery(theTenant);
            creditCardTypeQuery = new CreditCardTypeQuery(theTenant);
            moveTypeQuery = new MoveTypeQuery(moveTypeRepository);
            leadSourceQuery = new LeadSourceQuery(leadSourceRepository);
            stageQuery = new StageQueryService(stageRepository);
            industryQuery = new IndustryQuery(industryRepository);
            additionalServiceQuery = new AdditionalServiceQuery(additionalServiceRepository);
            closingReasonQuery = new OpportunityClosingReasonQueryService(closingReasonRepository);
            quoteClosingReasonQuery = new QuoteClosingReasonQuery(quoteClosingReasonRepository);
            shipmentSubTypeQuery = new ShipmentSubTypeQuery(shipmentSubTypeRepository);

            fullAccountingSettingsRepository = new FullAccountingSettingRepository(theTenant);
            bankCodeRepository = new BankCodeRepository(theTenant);
            taxWithholdingAssessOfficeRepository = new TaxWithholdingAssessOfficeRepository(theTenant);
            #endregion
        }
        private static Setting setting;
        public static string StartSignUp(SignUpInfoClass signUpInfo)
        {
            //signUpInfo.Company
            //signUpInfo.Name
            using (TransactionScope setScope = TransactionFactory.GetNewTransaction(new TimeSpan(2, 0, 0)))//new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(2, 0, 0)))
            {
                SettingRepository settingRepository = new SettingRepository();
                setting = settingRepository.GetSingleSetting("1");
                setScope.Complete();
            }
            string password = null;
            using (TransactionScope scop = TransactionFactory.GetNewTransaction(new TimeSpan(2, 5, 0)))//new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(2, 5, 0)))//Required, new TimeSpan(2, 5, 0)))
            {
                #region SignUp methods
                //using (TransactionScope scope = TransactionFactory.GetTransaction())
                //{
                // globalTenantRepository = new GlobalTenantRepository();
                TariffSetting zeroTariffSetting=null;
                AccountingSetting zeroAccountingSettings;
                SATInterfaceSetting tenantZeroSATInterfaceSetting;
                CustomsInterfaceSetting zeroCustomsInterfaceSetting;
                SharedLogisticsSetting zeroSharedLogisticsSetting;
                List<ObjectTable> tenantZeroObjectTables;
                List<Counter> tenantZeroCounters;
                List<CounterDefinition> tenantZeroCounterDefinitions;
                List<PaymentTerm> tenantZeroPaymentTerms;
                List<DocumentTypePM> tenantZeroDocumentTypes;
                List<EntityStatusPM> tenantZeroEntityStatus;
                List<EventTypePM> tenantZeroEventTypes;
                List<MeasurementPM> tenantZeroMeasurements;
                List<VatType> tenantZeroVatTypes;
                List<ChargesTypePM> tenantZeroChargesTypes;
                List<ChargesGroupPM> tenantZeroChargesGroups;
                List<PackageTypePM> tenantZeroPackageTypes;
                List<DocumentTypeCustomField> tenantZeroCustomFields;
                List<CreditCardTypePM> tenantZeroCreditCardTypes;
                List<MoveTypePM> tenantZeroMoveTypes;
                List<LeadSource> tenantZeroLeadSources;
                List<Stage> tenantZeroStages;
                List<IndustryPM> tenantZeroIndustries;
                List<AdditionalService> tenantZeroAdditionalServices;
                List<OpportunityClosingReason> tenantZeroClosingReasons;
                List<QuoteStage> tenantZeroQuoteStages;
                List<OpportunityType> tenantZeroOpportunityTypes;
                List<CustomsRequiredField> tenantZeroCustomsRequiredFields = null;
                List<DocumentTypeCustomsData> tenantZeroDocumentTypeCustomsDatas = null;
                List<DocumentsMetaDataType> tenantZeroDocumentsMetaDataType = null;
                List<Simplog.Data.InvoiceModel.EntityPOCOs.AccountingPaymentMethod> tenantZeroPaymentMethods=null;
                List<BankCode> tenantZeroBankCodes = null;
                List<TaxWithholdingAssessOffice> tenantZeroTaxWithholdingAssessOffices = null;
                List<QuoteClosingReason> tenantZeroQuoteClosingReasons= null;
                List<ShipmentSubType> tenantZeroShipmentSubTypes= null;

                //Tickets
                List<TicketType> tenantZeroTicketTypes = null;
                List<TicketStage> tenantZeroTicketStages = null;
                List<TicketSeverity> tenantZeroTicketSeverities = null;

                // SLA
                List<BusinessHour> tenantZeroBusinessHours = null;
                List<SLAHeader> tenantZeroSLAHeaders = null;
                List<SLALine> tenantZeroSLALines = null;
                List<WithholdingTaxDeductionType> tenantZeroWithholdingTaxDeductionType = null;
                using (TransactionScope scope = TransactionFactory.GetNewTransaction(new TimeSpan(2, 30, 0)))//new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(2, 30, 0)))
                {
                    InitializeRepositories(0);

                    #region lists
                    tenantZeroObjectTables = objectTableRepository.GetObjectsByTenant(0).Where(d => d.InActive == false).ToList();
                    tenantZeroCounters = counterRepository.GetCounters(0).ToList();
                    tenantZeroCounterDefinitions = counterDefinitionRepository.GetCounterDefinitions(0).ToList();
                    tenantZeroPaymentTerms = paymentTermRepository.GetPaymenTermsByTenant(0).ToList();
                    tenantZeroDocumentTypes = documentTypeQuery.GetDocumentTypePMsByTenant(0).ToList();
                    tenantZeroEntityStatus = entityStatusQuery.GetEntityStatusPMsByTenant(0).ToList();
                    tenantZeroEventTypes = eventTypeQuery.GetEventTypePMsByTenant(0).ToList();
                    tenantZeroMeasurements = measurementQuery.GetUnitOfMeasurementPMs(0).ToList();
                    tenantZeroVatTypes = vatTypeRepository.GetVatTypes(0).ToList();
                    tenantZeroChargesTypes = chargesTypeQuery.GetChargesTypePMsByTenant(0).ToList();
                    tenantZeroChargesGroups = chargesGroupQuery.GetChargesGroupPMsByTenant(0).ToList();
                    tenantZeroPackageTypes = packageTypeQuery.GetPackageTypePMsByTenant(0).ToList();
                    tenantZeroCustomFields = documentTypeCustomFieldRepository.GetDocumentTypeCustomFields(0).ToList();

                    ITariffModuleContext iTariffContext= TariffModuleContext.GetContext(0);
                    if (setting.WorkEnvironment != "customs") zeroTariffSetting = (from d in iTariffContext.TariffSettings where d.Tenant == 0 select d).FirstOrDefault();

                    zeroAccountingSettings = accountingSettingsRepository.GetSingleAccountSetting(0);
                    zeroCustomsInterfaceSetting = customsInterfaceSettingRepository.GetSingleCustomsInterfaceSetting(0, 0);
                    zeroSharedLogisticsSetting = sharedLogisticsSettingRepository.GetSingle("0", 0);
                    tenantZeroCreditCardTypes = creditCardTypeQuery.GetCreditCardTypePMs(0).ToList();
                    tenantZeroMoveTypes = moveTypeQuery.GetMoveTypePMs(0).ToList();
                    tenantZeroLeadSources = leadSourceRepository.GetLeadSources(0).ToList();
                    tenantZeroStages = stageRepository.GetAll(0).ToList();
                    tenantZeroIndustries = industryQuery.GetIndustryPMsByTenant(0).ToList();
                    tenantZeroAdditionalServices = additionalServiceRepository.GetAdditionalServices(0).ToList();
                    tenantZeroClosingReasons = closingReasonRepository.GetAll(0).ToList();
                    tenantZeroQuoteStages = quoteStageRepository.GetQuoteStagesForSignup(0).ToList();
                    tenantZeroOpportunityTypes = opportunityTypeRepository.GetAll(0).ToList();
                    tenantZeroDocumentsMetaDataType = documentsMetaDataTypeRepository.GetDocumentsMetaDataTypes(0).ToList();
                    if (setting.WorkEnvironment != "customs") tenantZeroPaymentMethods = PaymentMethodRepository.GetAccountingPaymentMethods(0).ToList();
                    tenantZeroBankCodes = bankCodeRepository.GetAll(0).ToList();
                    if (setting.WorkEnvironment != "customs") tenantZeroQuoteClosingReasons = quoteClosingReasonRepository.GetQuoteClosingReasons(0).ToList();
                    if (setting.WorkEnvironment != "customs") tenantZeroShipmentSubTypes = shipmentSubTypeRepository.GetShipmentSubTypes(0).ToList();

                    //Tickets 
                    tenantZeroTicketTypes = ticketTypeRepository.GetAll(0).ToList();
                    tenantZeroTicketStages = ticketStageRepository.GetAll(0).ToList();
                    tenantZeroTicketSeverities = ticketSeverityRepository.GetAll(0).ToList();

                    //SLA 
                    tenantZeroBusinessHours = businessHourRepository.GetBusinessHours(0).ToList();
                    if (setting.WorkEnvironment != "customs") tenantZeroSLAHeaders = slaHeaderRepository.GetAll(0).ToList();
                    tenantZeroSLALines = slaLineRepository.GetAll(0).ToList();
                    tenantZeroWithholdingTaxDeductionType = withholdingTaxDeductionTypeRepository.GetAll(0).ToList();
                    tenantZeroSATInterfaceSetting = sATInterfaceSettingRepository.GetSingleSATInterfaceSetting(0);
                    tenantZeroTaxWithholdingAssessOffices = taxWithholdingAssessOfficeRepository.GetAll(0).ToList();
                    if (setting.WorkEnvironment == "customs")
                    {
                        tenantZeroCustomsRequiredFields = CustomsRequiredFieldRepository.GetAll(0).ToList();
                        tenantZeroDocumentTypeCustomsDatas = DocumentTypeCustomsDataRepository.GetAll(0).ToList();
                    }

                    #endregion
                    scope.Complete();
                }

                tenant = CreateTenant(signUpInfo);
                InitializeRepositories(tenant);
                AddDefaultSATInterfaceSettings(tenant, sATInterfaceSettingRepository, tenantZeroSATInterfaceSetting);// Temporerly Commented By Rabaia So Create Tenant Continue until Islam Check it            
                if (setting.WorkEnvironment != "customs") AddDefaultTariffSettings(tenant, tariffSettingRepository, zeroTariffSetting);
                if (setting.WorkEnvironment != "customs") AddDefaultTariffProducts(tenant);
                AddDefaultAccountingSettings(tenant, accountingSettingsRepository, zeroAccountingSettings);
                AddDefaultCustomsInterfaceSettings(tenant, customsInterfaceSettingRepository, zeroCustomsInterfaceSetting);
                AddDefaultSharedLogisticsSettings(tenant, sharedLogisticsSettingRepository, zeroSharedLogisticsSetting);
                AddBranchesAndDepartments(tenant, branchRepository, departmentRepository);

                UserShortDetails userShortDetails = new UserShortDetails
                {
                    Tenant = tenant,
                    Name = signUpInfo.Name,
                    Email = signUpInfo.Email,
                    PhoneNumber = signUpInfo.Phone
                };
                password = AddUser(userShortDetails, userRepository, branchRepository, departmentRepository, roleRepository);
                UpdateLogBoxTenant(tenant, signUpInfo);
                AddCounters(tenant, counterRepository, tenantZeroObjectTables, tenantZeroCounters);
                List<Counter> currentTenantCounters = counterRepository.GetCounters(tenant).ToList();

                AddCounterDefinitions(tenant, counterDefinitionRepository, tenantZeroCounters, currentTenantCounters, tenantZeroCounterDefinitions);
                AddTenantSettings(tenant, tenantSettingRepository, tenantZeroObjectTables);
                AddPaymentTerms(tenant, paymentTermRepository, tenantZeroPaymentTerms);

                AddDocumentTypes(tenant, documentTypeRepository, documentTypeCopyRepository, tenantZeroDocumentTypes, tenantZeroObjectTables, /*CurrentTenantObjectTables*/null, documentTypeCustomFieldRepository, tenantZeroCustomFields, signUpInfo.CountryCode);
                List<DocumentType> currentTenantDocumentTypes = documentTypeRepository.GetDocumentTypes(tenant).ToList();

                AddDocumentTypeTemplates(tenant, documentTypeTemplateRepository, tenantZeroDocumentTypes, currentTenantDocumentTypes, documentTypeRepository, signUpInfo.CountryCode);

                AddEntityStatus(tenant, entityStatusRepository, tenantZeroEntityStatus, tenantZeroObjectTables);
                List<EntityStatus> currentTenantEntityStatus = entityStatusRepository.GetEntityStatusByTenant(tenant).ToList();

                AddEventTypes(tenant, eventTypeRepository, tenantZeroEventTypes, tenantZeroObjectTables, currentTenantEntityStatus, tenantZeroEntityStatus);

                AddMeasurements(tenant, measurementRepository, tenantZeroMeasurements);
                List<Measurement> currentTenantMeasurement = measurementRepository.GetMeasurementsByTenant(tenant).ToList();

                AddCreditCardTypes(tenant, creditCardTypeRepository, tenantZeroCreditCardTypes);
                List<CreditCardType> currentTenantCreditCardTypes = creditCardTypeRepository.GetCreditCardTypes(tenant).ToList();

                AddMoveTypes(tenant, moveTypeRepository, tenantZeroMoveTypes);
                List<MoveType> currentTenantMoveTypes = moveTypeRepository.GetMoveTypesByTenant(tenant).ToList();

                AddVatTypes(tenant, vatTypeRepository, tenantZeroVatTypes, vatTypePercentageRepository);
                List<VatType> currentTenantVatTypes = vatTypeRepository.GetVatTypes(tenant).ToList();

                AddLeadSources(tenant, leadSourceRepository, tenantZeroLeadSources);
                AddStages(tenant, stageRepository, tenantZeroStages);
                AddIndustries(tenant, industryRepository, tenantZeroIndustries);
                AddAdditionalServices(tenant, additionalServiceRepository, tenantZeroAdditionalServices);
                AddClosingReasons(tenant, closingReasonRepository, tenantZeroClosingReasons);
                AddQuoteStages(tenant, quoteStageRepository, tenantZeroQuoteStages);
                AddChargesTypes(tenant, chargesTypeRepository, tenantZeroChargesTypes, currentTenantMeasurement, tenantZeroVatTypes, currentTenantVatTypes);
                AddChargesGroups(tenant, chargesGroupRepository, tenantZeroChargesGroups);
                AddRanks(tenant, rankRepository);
                AddPackageTypes(tenant, packageTypeRepository, measurementRepository, tenantZeroPackageTypes);
                AddOpportunityTypes(tenant, opportunityTypeRepository, tenantZeroOpportunityTypes);
                AddDocumentsMetaDataTypes(tenant, documentsMetaDataTypeRepository, tenantZeroDocumentsMetaDataType);
                if (setting.WorkEnvironment != "customs")  AddPaymentMethods(tenant, PaymentMethodRepository, tenantZeroPaymentMethods);
                AddTicketTypes(tenant, ticketTypeRepository, tenantZeroTicketTypes);
                AddTicketStages(tenant, ticketStageRepository, tenantZeroTicketStages);
                AddTicketSeverities(tenant, ticketSeverityRepository, tenantZeroTicketSeverities);
                if (setting.WorkEnvironment != "customs")  AddQuoteClosingReasons(tenant, quoteClosingReasonRepository, tenantZeroQuoteClosingReasons);
                if (setting.WorkEnvironment != "customs") AddShipmentSubTypes(tenant, shipmentSubTypeRepository, tenantZeroShipmentSubTypes);
                AddBusinessHours(tenant, businessHourRepository, tenantZeroBusinessHours);
                if (setting.WorkEnvironment != "customs") AddSLAHeaders(tenant, slaHeaderRepository, tenantZeroSLAHeaders);
                AddWithholdingTaxDeductionTypes(tenant, withholdingTaxDeductionTypeRepository, tenantZeroWithholdingTaxDeductionType);
                //AddSLALines(tenant, slaLineRepository, tenantZeroSLALines);
                AddBankCodes(tenant, bankCodeRepository, tenantZeroBankCodes);
                AddTaxWithholdingAssessOffice(tenant, taxWithholdingAssessOfficeRepository, tenantZeroTaxWithholdingAssessOffices);
                //AddJournalActionTypes(tenant);

                if (setting.WorkEnvironment == "customs")
                {
                    AddCustomsRequiredFields(tenant, customsRequiredFieldRepository, tenantZeroCustomsRequiredFields);
                    AddDocumentType(tenant, documentTypeRepository, tenantZeroDocumentTypes);
                    AddDocumentTypeCustomsData(tenant, documentTypeCustomsDataRepository, tenantZeroDocumentTypeCustomsDatas);
                }
                else
                {
                    AddSLALines(tenant, slaLineRepository, tenantZeroSLALines);

                    AddJournalActionTypes(tenant);
                }

                UserShortDetails systemUserShortDetails = new UserShortDetails
                {
                    Tenant = tenant,
                    Name = "System",
                    Email = "system@tenant" + tenant + ".com",
                    PhoneNumber = "99999999"
                };
                string systemPassword = AddUser(systemUserShortDetails, userRepository, branchRepository, departmentRepository, roleRepository);

                if (setting.WorkEnvironment != "customs")  AddDefaultFullAccountingSettings(tenant, fullAccountingSettingsRepository);

                AddReportFromTenantZero(tenant);

                if (setting.WorkEnvironment != "customs") AddGeneralBIReportFolder(tenant);

                AddTenantLoginPolicy(tenant);

                AddAutomationFromTenantZero(tenant , tenantZeroDocumentTypes);

                #endregion
                scop.Complete();
            }



            
            TenantManagement tenantManagement = null;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction(new TimeSpan(2, 0, 0)))
            {
                TenantManagementRepository tenantrep = new TenantManagementRepository();
                tenantManagement = tenantrep.GetSingleTenantManagement(tenant);
                scope.Complete();
            }

            if (tenantManagement.Id != 0) //Islam- new database signup
            {
                TenantManagementTracing.Trace(null, tenantManagement, true);
            }

            InitializeEmployeeGroup(signUpInfo.Email);
            InitializeTicketClassification();


            AddQuoteTemplate(tenant);
            if (signUpInfo.IsCrmTenant)
            {
                // IsCrmTenant

                // Update Customer 
                if (!string.IsNullOrEmpty(signUpInfo.CustomerId))
                {
                    CardRepository cardRepository = new CardRepository(signUpInfo.Tenant);
                    StageRepository stageRepository = new StageRepository(signUpInfo.Tenant);
                    OpportunityStageRepository opportunityStageRepository = new OpportunityStageRepository(signUpInfo.Tenant);
                    Card card = cardRepository.GetSingleCardWithoutInclude(signUpInfo.CustomerId, signUpInfo.Tenant);

                    if (card != null && string.IsNullOrEmpty(card.ReceivablesAccountingCard))
                    {
                        card.ReceivablesAccountingCard = tenant.ToString();

                        #region Address Company 
                        TenantRepository tenantRepository = new TenantRepository(tenant);
                        Tenant tenantPoco = tenantRepository.GetSingleByTenant(tenant);
                        AddressRepository addressRepository = new AddressRepository(tenant);

                        if (tenantPoco != null  )
                        {
                            Address cardAddress = addressRepository.GetSingleAddressByCardIdAndTypeId(card.Id, "M", card.Tenant);

                            if (cardAddress != null)
                            {
                                #region Country
                                string countryId = null;
                                string countryCode = null;
                                if (!string.IsNullOrEmpty(cardAddress.CountryId))
                                {
                                    CountryRepository countryRepository = new CountryRepository(tenant);
                                    countryCode = cardAddress.Country!=null ? cardAddress.Country.Code : null;

                                    if (!string.IsNullOrEmpty(countryCode))
                                    {
                                        countryId = countryRepository.GetCountryIdByCode(countryCode, tenant);                                        
                                    }
                                }
                                #endregion

                                #region State
                                string stateId = null;
                                string stateCode = null;
                                if (!string.IsNullOrEmpty(cardAddress.StateId))
                                {
                                    StateRepository stateRepository = new StateRepository(tenant);
                                    stateCode = cardAddress.State != null ? cardAddress.State.Code : null;

                                    if (!string.IsNullOrEmpty(stateCode)) { 
                                        stateId = stateRepository.GetStateIdByCode(stateCode, tenant);
                                    }
                                }
                                #endregion

                                Address newAddress = new Address()
                                {
                                    Id = IdCounter.GetNumber("Address", 0).ToString(),
                                    Address1 = cardAddress.Address1,
                                    Address2 = cardAddress.Address2,
                                    City = cardAddress.City,
                                    CountryId = countryId,
                                    StateId = stateId,
                                    ZipCode = cardAddress.ZipCode,
                                    PhoneNumber = cardAddress.PhoneNumber,
                                    FaxNumber = cardAddress.FaxNumber,
                                    AddressTypeId = "M",
                                    Name = cardAddress.Name,
                                    Tenant = tenant,
                                    Description = cardAddress.Description,
                                    ATTN = cardAddress.ATTN,
                                };

                                addressRepository.Add(newAddress);
                                addressRepository.SubmitChanges();

                                ComputeInvoiceSectionFields(tenantPoco, newAddress, countryCode, stateCode);

                                tenantPoco.AddressId = newAddress.Id;
                                tenantRepository.Update(tenantPoco);
                                tenantRepository.SubmitChanges();

                                if (countryCode == "IL")
                                {
                                    AccountingSetting iAccountingSetting = accountingSettingsRepository.GetSingleAccountSetting(tenant);
                                    if (iAccountingSetting != null)
                                    {
                                        iAccountingSetting.AllowVoidAPI = false;
                                        iAccountingSetting.AllowVoidAPP = false;
                                        iAccountingSetting.AllowVoidARI = false;
                                        iAccountingSetting.AllowVoidARP = false;
                                        iAccountingSetting.AllowManualInvoiceNumber = false;
                                        iAccountingSetting.IsARInvoiceChronologicalDates = true;
                                        iAccountingSetting.IsARPaymentChronologicalDates = true;
                                        iAccountingSetting.IsVatNumberMandatoryInAP = true;
                                        iAccountingSetting.IsVatNumberMandatoryInAR = true;
                                        accountingSettingsRepository.Update(iAccountingSetting);
                                        accountingSettingsRepository.SubmitChanges();
                                    }

                                    List<string> iCodes = new List<string>();
                                    iCodes.Add("999S");
                                    iCodes.Add("999C");
                                    iCodes.Add("999M");
                                    iCodes.Add("999CI");
                                    iCodes.Add("999MP");
                                    iCodes.Add("999P");
                                    List<DocumentType> iDocumentTypes = documentTypeRepository.GetDocumentTypesByCodeLists(iCodes, tenant);
                                    if (iDocumentTypes.Count > 0)
                                    {
                                        foreach (DocumentType iDocumentType in iDocumentTypes)
                                        {
                                            List<DocumentTypeCopy> iDocumentTypeCopies = (from d in documentTypeRepository.context.DocumentTypeCopies
                                                                                          where d.Tenant == tenant && d.DocumentTypeId == iDocumentType.Id
                                                                                          select d).ToList();

                                            if (iDocumentTypeCopies.Count > 0)
                                            {
                                                DocumentTypeCopy iDocumentTypeCopy = iDocumentTypeCopies.Where(d => d.Name.ToLower() == "original").FirstOrDefault();
                                                if (iDocumentTypeCopy == null)
                                                {
                                                    iDocumentTypeCopy = iDocumentTypeCopies.Where(d => d.Code.ToLower() == iDocumentType.Code.ToLower()).FirstOrDefault();
                                                }

                                                if (iDocumentTypeCopy != null)
                                                {
                                                    iDocumentType.LimitedPrintCopyId = iDocumentTypeCopy.Id;
                                                    iDocumentType.IsDocumentOneTimePrintLimited = true;
                                                    documentTypeRepository.Update(iDocumentType);
                                                }
                                            }
                                        }

                                        documentTypeRepository.SubmitChanges();
                                    }
                                }

                                if(countryCode == "MX")
                                {
                                    MexicanCountryCities mexicanCountryCities = new MexicanCountryCities();
                                    mexicanCountryCities.AddMexicanCountryCities(tenant, countryId);     
                                }
                            }                                                      
                        }
                        #endregion

                        OpportunityRepository opportunityRepository = new OpportunityRepository(signUpInfo.Tenant);
                        IQueryable<Opportunity> opportunityList = opportunityRepository.GetOppListByCustomerIdAndTenant(signUpInfo.CustomerId, signUpInfo.Tenant);

                        foreach (Opportunity opportunity in opportunityList)
                        {
                            Stage stage = stageRepository.GetStageByCode("TEN", signUpInfo.Tenant);
                            if (stage == null)
                            {
                                stage = new Stage() { Id = IdCounter.GetNumber("Stage", signUpInfo.Tenant), Name = "Tenant Opened", Tenant = signUpInfo.Tenant, InActive = false, IsSelectable = true, MaxDays = null, Probability = 0, SearchFields = "TEN,Tenant Open,0", Code = "TEN" };
                                stageRepository.Add(stage);
                                stageRepository.SubmitChanges();
                            }

                            OpportunityStage opportunityStage = new OpportunityStage() { Id = IdCounter.GetNumber("OpportunityStage", signUpInfo.Tenant), FromStageId = opportunity.StageId, ToStageId = stage.Id, StartDate = opportunity.LastStageDate, EndDate = TenantServerConfigration.GetCurrentDateTime(0), Tenant = signUpInfo.Tenant, OpportunityId = opportunity.Id };

                            opportunityStageRepository.Add(opportunityStage);
                            opportunity.StageId = stage.Id;
                            opportunity.Field3 = tenant.ToString();
                            opportunityRepository.Update(opportunity);
                        }


                        opportunityStageRepository.SubmitChanges();
                        opportunityRepository.SubmitChanges();

                        cardRepository.Update(card);
                        cardRepository.SubmitChanges();
                    }
                }
            }
            signUpInfo.Tenant = tenant;
            return password;
        }

        private static void AddShipmentSubTypes(int tenant, ShipmentSubTypeRepository shipmentSubTypeRepository, List<ShipmentSubType> tenantZeroShipmentSubTypes)
        {
            foreach (ShipmentSubType subType in tenantZeroShipmentSubTypes)
            {
                ShipmentSubType newSubType = new ShipmentSubType()
                {
                    Tenant = tenant,
                    Name = subType.Name,
                    Code = subType.Code,
                    ShipmentTypeCode = subType.ShipmentTypeCode,
                    Inactive = subType.Inactive,
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    CreatedByUserId = subType.CreatedByUserId,
                    UpdatedByUserId = subType.UpdatedByUserId,
                    SearchFields = subType.SearchFields,
                    Id = IdCounter.GetNumber("ShipmentSubType", tenant).ToString(),
                };
                shipmentSubTypeRepository.Add(newSubType);
            }
            shipmentSubTypeRepository.SubmitChanges();
        }

        private static void AddQuoteClosingReasons(int tenant, QuoteClosingReasonRepository quoteClosingReasonRepository, List<QuoteClosingReason> tenantZeroQuoteClosingReasons)
        {
            //User myUser = userRepository.GetSingleUserByEmail("system@tenant" + tenant + ".com", tenant, false);

            List<QuoteClosingReason> closingReasonsList = tenantZeroQuoteClosingReasons.Where(d => d.Tenant == 0).ToList();
            foreach (QuoteClosingReason closingReason in closingReasonsList)
            {
                QuoteClosingReason newClosingReason = new QuoteClosingReason()
                {
                    Tenant = tenant,
                    Name = closingReason.Name,
                    Code = closingReason.Code,
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                    CreatedByUserId = closingReason.CreatedByUserId,
                    UpdatedByUserId = closingReason.UpdatedByUserId,
                    SearchFields = closingReason.SearchFields,                    
                    Id = IdCounter.GetNumber("QuoteClosingReason", tenant).ToString(),
                };
                quoteClosingReasonRepository.Add(newClosingReason);
            }
            quoteClosingReasonRepository.SubmitChanges();
        }

        private static void AddPriceSteps(TariffSetting setting)
        {
            List<PriceStep>  tenantZeroPriceSteps = priceStepRepository.GetAll(0).ToList();
            List<PriceStep> priceSteps = tenantZeroPriceSteps.Where(d => d.Tenant == 0).ToList();
            TariffSettingRepository tariffSettingRepository = new TariffSettingRepository(tenant);

            foreach (PriceStep item in priceSteps)
            {
                PriceStep newPriceStep = new PriceStep()
                {
                    Id = IdCounter.GetNumber("PriceStep", tenant).ToString(),
                    Tenant = tenant,
                    CreateDate = item.CreateDate,
                    CreatedByUserId = item.CreatedByUserId,
                    UpdateDate = item.UpdateDate,
                    UpdatedByUserId = item.UpdatedByUserId,
                    Name = item.Name,
                    Steps = item.Steps,
                    Inactive = item.Inactive,
                    SearchFields = item.SearchFields,
                };

                priceStepRepository.Add(newPriceStep);
                priceStepRepository.SubmitChanges();
                if (setting != null)
                {
                    setting.AirDefaultStepsId = newPriceStep.Id;
                    setting.LCLDefaultStepsId = newPriceStep.Id;
                    tariffSettingRepository.Update(setting);
                    tariffSettingRepository.SubmitChanges();
                }
            }
        }

        private static void AddAutomationFromTenantZero(int tenant, List<DocumentTypePM> tenantZeroDocumentTypes)
        {
            AutomationHelper automationHelper = new AutomationHelper();
            automationHelper.CopyAutomationFromTenantZeroToMyTenant(tenant, tenantZeroDocumentTypes);
        }

        private static void ComputeInvoiceSectionFields(Tenant iTenant, Address iAddress, string iCountryCode, string iStateCode)
        {
            if (iAddress != null)
            {
                var value = iTenant.Company + Environment.NewLine;

                if (iAddress.Address1 != null && iAddress.Address2 != null)
                {
                    value = value + iAddress.Address1 + "," + iAddress.Address2 + Environment.NewLine;
                }

                else if (iAddress.Address1 != null && iAddress.Address2 == null)
                {
                    value = value + iAddress.Address1 + Environment.NewLine;
                }

                else if (iAddress.Address1 == null && iAddress.Address2 != null)
                {
                    value = value + iAddress.Address2 + Environment.NewLine;
                }

                if (iAddress.City != null || iStateCode != null || iCountryCode != null || iAddress.ZipCode != null)
                {
                    if (iAddress.City != null && iCountryCode == null)
                    {
                        value = value + iAddress.City;
                    }

                    else if (iCountryCode != null && iAddress.City == null)
                    {
                        value = value + iCountryCode;
                    }

                    else if (iAddress.City != null && iCountryCode != null)
                    {
                        value = value + iAddress.City + "-" + iCountryCode;
                    }

                    else if (iStateCode != null)
                    {
                        value = value + "(" + iStateCode + ")";
                    }

                    else if (iAddress.ZipCode != null)
                    {
                        value = value + iAddress.ZipCode;
                    }

                    value = value + Environment.NewLine;
                }

                if (iAddress.PhoneNumber != null && iAddress.FaxNumber != null)
                {
                    value = value + "Tel:" + " " + iAddress.PhoneNumber + " " + "Fax:" + " " + iAddress.FaxNumber + Environment.NewLine;
                }

                else if (iAddress.PhoneNumber != null && iAddress.FaxNumber == null)
                {
                    value = value + "Tel:" + " " + iAddress.PhoneNumber + " " + Environment.NewLine;
                }

                else if (iAddress.PhoneNumber == null && iAddress.FaxNumber != null)
                {
                    value = value + "Fax:" + " " + iAddress.FaxNumber + Environment.NewLine;
                }

                iTenant.InvoiceSection1 = value;
                iTenant.InvoiceSection2 = iTenant.Company;
            }
        }

        private static void InitializeEmployeeGroup(string email)
        {
            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant);

            UserRepository userRepository = new UserRepository(tenant);
            User systemUser = userRepository.GetSingleUserByEmail(email, tenant, false);

            EmployeeGroupRepository employeeGroupRepository = new EmployeeGroupRepository(tenant);
            bool isEmployeeGroupExists = employeeGroupRepository.IsEmployeeGroupExists(tenant);

            EmployeeGroup employeeGroup = null;
            if (!isEmployeeGroupExists)
            {
                employeeGroup = new EmployeeGroup()
                {
                    Id = IdCounter.GetNumber("EmployeeGroup", tenant).ToString(),
                    Tenant = tenant,
                    CreateDate = todayDate,
                    CreatedByUserId = systemUser.Id,
                    UpdateDate = todayDate,
                    UpdatedByUserId = systemUser.Id,
                    SearchFields = "Unassigned Tickets",
                    Name = "Unassigned Tickets",
                    Description = null,
                    Inactive = false,
                    ManagerUserId = null,
                };

                employeeGroupRepository.Add(employeeGroup);
                employeeGroupRepository.SubmitChanges();
            }

            TicketClassificationRepository classifiationRepository = new TicketClassificationRepository(tenant);
            TicketClassification ticketClassification = classifiationRepository.GetTicketClassificationByName("General", tenant);

            if (ticketClassification != null)
            {
                ticketClassification.EmployeeGroupId = employeeGroup.Id;

                classifiationRepository.Update(ticketClassification);
                classifiationRepository.SubmitChanges();
            }
        }

        private static void InitializeTicketClassification()
        {
            TicketClassificationRepository classifiationRepository = new TicketClassificationRepository(tenant);
            TicketSeverityRepository severityRepository = new TicketSeverityRepository(tenant);
            EmployeeGroupRepository employeeGroupRepository = new EmployeeGroupRepository(tenant);
            TicketSeverity ticketSeverity = severityRepository.GetTicketSeverityByCode("MD", tenant);
            if (ticketSeverity != null)// added because it fails when from customs.
            {
                string defaultSeverityId = severityRepository.GetTicketSeverityByCode("MD", tenant).Id;
                EmployeeGroup employeeGroup = employeeGroupRepository.GetEmployeeGroupByName("Unassigned Tickets", tenant);
                bool isTicketClassificationExists = classifiationRepository.IsTicketClassificationExists(tenant);

                if (!isTicketClassificationExists)
                {
                    TicketClassification classification = new TicketClassification()
                    {
                        Id = Convert.ToString(tenant),
                        Tenant = tenant,
                        Name = "General",
                        ParentId = null,
                        SearchFields = "General",
                        Inactive = false,
                        DefaultSeverityId = defaultSeverityId,
                        EmployeeGroupId = employeeGroup.Id,
                    };

                    classifiationRepository.Add(classification);
                    classifiationRepository.SubmitChanges();
                }
            }
        }

        private static void AddJournalActionTypes(int tenant)
        {
            JournalActionTypeRepository repository = new JournalActionTypeRepository(tenant);

            JournalActionType type1 = new JournalActionType()
            {
                Id = IdCounter.GetNumber("JournalActionType", tenant).ToString(),
                Tenant = tenant,
                Code = "1",
                EnglishName = "Credit",
                LocalName = "זכות",
                SearchFields = "1זכותCredit",
                Inactive = false,
            };
            repository.Add(type1);

            JournalActionType type2 = new JournalActionType()
            {
                Id = IdCounter.GetNumber("JournalActionType", tenant).ToString(),
                Tenant = tenant,
                Code = "2",
                EnglishName = "Debit",
                LocalName = "חובה",
                SearchFields = "2חובהDebit",
                Inactive = false,
            };
            repository.Add(type2);

            JournalActionType type3 = new JournalActionType()
            {
                Id = IdCounter.GetNumber("JournalActionType", tenant).ToString(),
                Tenant = tenant,
                Code = "3",
                EnglishName = "Debit+Credit",
                LocalName = "חובה+זכות",
                SearchFields = "3חובה+זכותDebit+Credit",
                Inactive = false,
            };
            repository.Add(type3);

            JournalActionType type4 = new JournalActionType()
            {
                Id = IdCounter.GetNumber("JournalActionType", tenant).ToString(),
                Tenant = tenant,
                Code = "4",
                EnglishName = "Debit+Credit+VAT Extract",
                LocalName = "חובה+זכות+חילוץ מע\"מ",
                SearchFields = "4חובה+זכות+חילוץ מע\"מDebit+Credit+VAT Extract",
                Inactive = false,
            };
            repository.Add(type4);

            repository.SubmitChanges();
        }

        private static void AddCustomsRequiredFields(int tenant, Logitude.Customs.Data.Repsitories.CustomsRequiredFieldRepository customsRequiredFieldRepository, List<CustomsRequiredField> tenantZeroCustomsRequiredFields)
        {
            foreach (CustomsRequiredField field in tenantZeroCustomsRequiredFields)
            {
                CustomsRequiredField newField = new CustomsRequiredField()
                {
                    Id = IdCounter.GetNumber("CustomsRequiredField", tenant).ToString(),
                    ObjectfieldId = field.ObjectfieldId,
                    ObjectfieldCode = field.ObjectfieldCode,
                    ObjectTableId = field.ObjectTableId,
                    IsImport=field.IsImport,
                    IsExport=field.IsExport,
                    Tenant = tenant,
                };
                customsRequiredFieldRepository.Add(newField);
            }

            customsRequiredFieldRepository.SubmitChanges();
        }

        private static void AddDocumentType(int tenant, Simplog.Data.CommonDataModel.Repositories.DocumentTypeRepository documentTypeRepository, List<DocumentTypePM> tenantZeroDocumentTypes)
        {
            foreach (DocumentTypePM field in tenantZeroDocumentTypes)
            {

                DocumentType newField = new DocumentType()
                {
                    Id = IdCounter.GetNumber("DocumentType", tenant).ToString(),
                    Code = field.Code,
                    Tenant = tenant,
                    IsAir = field.IsAir,
                    IsDocIn = field.IsDocIn,
                    IsDocOut = field.IsDocOut,
                    IsInland = field.IsInland,
                    IsOcean = field.IsOcean,
                    Name = field.Name,
                    Notes = field.Notes,
                    InActive = field.InActive,
                    SearchFields = field.SearchFields,
                    ObjectTableId = field.ObjectTableId,
                    Subject = field.Subject,
                    DocumentTypeDefaultHTMLTemplateId = field.DocumentTypeDefaultHTMLTemplateId,
                    DocumentTypeDefaultReportTemplateId = field.DocumentTypeDefaultReportTemplateId,
                    TemplateFormatCode = field.TemplateFormatCode,
                    DocumentTypeDefaultEditorTool = field.DocumentTypeDefaultEditorTool,
                    IsMaster = field.IsMaster,
                    IsDirect = field.IsDirect,
                    IsHouse = field.IsHouse,
                    CustomControl = field.CustomControl,
                    AgentRoleId = field.AgentRoleId,
                    CustomerRoleId = field.CustomerRoleId,
                    IsAgentView = field.IsAgentView,
                    IsCustomerView = field.IsCustomerView,
                    IsReadOnly = field.IsReadOnly,
                    LimitedPrintCopyId = field.LimitedPrintCopyId,
                    IsDocumentOneTimePrintLimited = field.IsDocumentOneTimePrintLimited,
                    IsCopiedAtSignup = field.IsCopiedAtSignup,
                    IsEnabledForCustomers = field.IsEnabledForCustomers,
                    CountryCode = field.CountryCode,
                    DocumentTypeCategoryCode = field.DocumentTypeCategoryCode,
                    OrderBy = field.OrderBy,
                    FileName = field.FileName,
                    IsAgentSharedInDirect = field.IsAgentSharedInDirect,
                    IsAgentSharedInHouse = field.IsAgentSharedInHouse,
                    IsAgentSharedInMaster = field.IsAgentSharedInMaster,
                    SharedDocumentTypeCopyId = field.SharedDocumentTypeCopyId,
                    IsAirDigitalSignRequired = field.IsAirDigitalSignRequired,
                    IsOceanDigitalSignRequired = field.IsOceanDigitalSignRequired,
                    IsInlandDigitalSignRequired = field.IsInlandDigitalSignRequired,
                    IsSystemAdditionalPrintingFields = field.IsSystemAdditionalPrintingFields,
                    PrintingFieldsScreenCode = field.PrintingFieldsScreenCode,
                    AddedManually = field.AddedManually,
                    OnPrintPopulateDateFieldName = field.OnPrintPopulateDateFieldName,
                    OnSendPopulateDateFieldName = field.OnSendPopulateDateFieldName,
                    OnUploadPopulateDateFieldName = field.OnUploadPopulateDateFieldName,
                };
                documentTypeRepository.Add(newField);
            }

            documentTypeRepository.SubmitChanges();
        }

        private static void AddDocumentTypeCustomsData(int tenant, Logitude.Customs.Data.Repsitories.DocumentTypeCustomsDataRepository documentTypeCustomsDataRepository, List<DocumentTypeCustomsData> tenantZeroDocumentTypeCustomsDatas)
        {
            foreach (DocumentTypeCustomsData field in tenantZeroDocumentTypeCustomsDatas)
            {
                DocumentTypeCustomsData newField = new DocumentTypeCustomsData()
                {
                    Tenant = tenant,
                    DocumentTypeId=field.DocumentTypeId,
                    CustomsDoucumentTypeCode = field.CustomsDoucumentTypeCode,
                };
                documentTypeCustomsDataRepository.Add(newField);
            }

            documentTypeCustomsDataRepository.SubmitChanges();
        }
        private static void AddBankCodes(int tenant, BankCodeRepository bankCodeRepository, List<BankCode> tenantZeroBankCode)
        {
            foreach (BankCode field in tenantZeroBankCode)
            {
                BankCode newField = new BankCode()
                {
                    Id = IdCounter.GetNumber("BankCode", tenant).ToString(),
                    Code = field.Code,
                    EnglishName = field.EnglishName,
                    Tenant = tenant,
                    Inactive = field.Inactive,
                    LocalName = field.LocalName,
                    LogoId = field.LogoId,
                 SearchFields = field.Code +","+field.EnglishName +"," + field.LocalName,

                };
                bankCodeRepository.Add(newField);
            }

            bankCodeRepository.SubmitChanges();
        }

        private static void AddDefaultTariffSettings(int theTenant, TariffSettingRepository iRepository, TariffSetting zeroEntity)
        {
            if (zeroEntity != null)
            {
                TariffSetting settings = new TariffSetting()
                {
                    Id = IdCounter.GetNumber("TariffSetting", theTenant).ToString(),
                    Tenant = theTenant,
                    DefaultPriceSteps = zeroEntity.DefaultPriceSteps,
                    ContainerDefaults = "20GP, 40GP, 20HC",
                };

                iRepository.Add(settings);
                iRepository.SubmitChanges();
                AddPriceSteps(settings);
            }   
        }

        private static void AddDefaultTariffProducts(int tenant)
        {
            TariffProduct GENTariffProduct = new TariffProduct()
            {
                Id = IdCounter.GetNumber("TariffProduct", tenant).ToString(),
                Tenant = tenant,
                Code = "GEN",
                Name = "General",
                LocalName = "General",
                Inactive = false,
                SearchFields = "GEN, General"
            };
            
            TariffProduct DNGTariffProduct = new TariffProduct()
            {
                Id = IdCounter.GetNumber("TariffProduct", tenant).ToString(),
                Tenant = tenant,
                Code = "DNG",
                Name = "Dangerous Goods",
                LocalName = "Dangerous Goods",
                Inactive = false,
                SearchFields = "DNG, Dangerous Goods"
            };

            tariffProductRepository.Add(GENTariffProduct);
            tariffProductRepository.Add(DNGTariffProduct);
            tariffProductRepository.SubmitChanges();
        }

        private static void AddDefaultAccountingSettings(int theTenant, AccountingSettingRepository theAccountingSettingsRepository, AccountingSetting tenantZeroAccoutingSettings)
        {
            if (tenantZeroAccoutingSettings != null)
            {
                AccountingSetting settings = new AccountingSetting()
                {
                    Id = theTenant,
                    AccountingSystemCode = tenantZeroAccoutingSettings.AccountingSystemCode != null ? tenantZeroAccoutingSettings.AccountingSystemCode : "NO",
                    AllowManualInvoiceNumber = tenantZeroAccoutingSettings.AllowManualInvoiceNumber,
                    AllowVoidAPI = tenantZeroAccoutingSettings.AllowVoidAPI,
                    AllowVoidAPP = tenantZeroAccoutingSettings.AllowVoidAPP,
                    AllowVoidARI = tenantZeroAccoutingSettings.AllowVoidARI,
                    AllowVoidARP = tenantZeroAccoutingSettings.AllowVoidARP,
                    IsARInvoiceChronologicalDates = tenantZeroAccoutingSettings.IsARInvoiceChronologicalDates,
                    IsVatNumberMandatoryInAP = tenantZeroAccoutingSettings.IsVatNumberMandatoryInAP,
                    IsVatNumberMandatoryInAR = tenantZeroAccoutingSettings.IsVatNumberMandatoryInAR,
                };

                theAccountingSettingsRepository.Add(settings);
                theAccountingSettingsRepository.SubmitChanges();
            }
        }

        private static void AddTaxWithholdingAssessOffice(int theTenant, TaxWithholdingAssessOfficeRepository theTaxWithholdingAssessOfficeRepository, List<TaxWithholdingAssessOffice>  tenantZeroTaxWithholdingAssessOffices)
        {

            foreach (TaxWithholdingAssessOffice field in tenantZeroTaxWithholdingAssessOffices)
            {


                if (tenantZeroTaxWithholdingAssessOffices != null)
                {
                    TaxWithholdingAssessOffice poco = new TaxWithholdingAssessOffice()
                    {
                        Id = IdCounter.GetNumber("TaxWithholdingAssessOffice", tenant).ToString(),

                        Code = field.Code,
                        LocalName = field.LocalName,
                        Name = field.Name,
                        Inactive = field.Inactive,
                        Tenant = theTenant,
                        SearchFields = field.Code + "," + field.Name + "," + field.LocalName,
                        

                    };

                    theTaxWithholdingAssessOfficeRepository.Add(poco);
                    theTaxWithholdingAssessOfficeRepository.SubmitChanges();
                }
            }
        }

        private static void AddDefaultSATInterfaceSettings(int theTenant, SATInterfaceSettingRepository theSATInterfaceSettingRepository, SATInterfaceSetting tenantZeroSATInterfaceSetting)
        {
            if (tenantZeroSATInterfaceSetting != null)
            {
                SATInterfaceSetting settings = new SATInterfaceSetting()
                {
                    Tenant = theTenant,
                    SATInterfaceCode = tenantZeroSATInterfaceSetting.SATInterfaceCode,


                };

                theSATInterfaceSettingRepository.Add(settings);
                theSATInterfaceSettingRepository.SubmitChanges();
            }
        }

        private static void AddDefaultCustomsInterfaceSettings(int theTenant, CustomsInterfaceSettingRepository theCustomsInterfaceSettingRepository, CustomsInterfaceSetting tenantZeroCustomsInterfaceSetting)
        {
            if (tenantZeroCustomsInterfaceSetting != null)
            {
                CustomsInterfaceSetting settings = new CustomsInterfaceSetting()
                {
                    Tenant = theTenant,
                    LocalCustomsInterfaceCode = tenantZeroCustomsInterfaceSetting.LocalCustomsInterfaceCode != null ? tenantZeroCustomsInterfaceSetting.LocalCustomsInterfaceCode : "NO",
                    ImportToUSAInterfaceCode = tenantZeroCustomsInterfaceSetting.ImportToUSAInterfaceCode != null ? tenantZeroCustomsInterfaceSetting.ImportToUSAInterfaceCode : "NO",
                    ExportFromUSAInterfaceCode = tenantZeroCustomsInterfaceSetting.ExportFromUSAInterfaceCode != null ? tenantZeroCustomsInterfaceSetting.ExportFromUSAInterfaceCode : "NO",
                    LocalCompanyId = tenantZeroCustomsInterfaceSetting.LocalCompanyId,
                    LocalUserId = tenantZeroCustomsInterfaceSetting.LocalUserId,
                    LocalPassword = tenantZeroCustomsInterfaceSetting.LocalPassword,
                };
                
                theCustomsInterfaceSettingRepository.Add(settings);
                theCustomsInterfaceSettingRepository.SubmitChanges();
            }
        }

        private static void AddDefaultSharedLogisticsSettings(int theTenant, SharedLogisticsSettingRepository theSharedLogisticsSettingRepository, SharedLogisticsSetting tenantZeroSharedLogisticsSetting)
        {
            if (tenantZeroSharedLogisticsSetting != null)
            {
                SharedLogisticsSetting settings = new SharedLogisticsSetting()
                {
                    Tenant = theTenant,
                    Id = theTenant.ToString(),
                    IsShowAmountLocalCurrency = true,
                };

                theSharedLogisticsSettingRepository.Add(settings);
                theSharedLogisticsSettingRepository.SubmitChanges();
            }
        }

        public static int CreateTenant(SignUpInfoClass signUpInfoClass)
        {       
            LogitudeLead lead = null;

            if (signUpInfoClass.IsCrmTenant)
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(2, 0, 0)))
                {
                    LogitudeLeadRepository logitudeLeadRepository = new LogitudeLeadRepository();
                    lead = logitudeLeadRepository.GetSingleLogitudeLeadByCustomerId(signUpInfoClass.CustomerId);
                    scope.Complete();
                }
            }

            TenantPM newTenant = GetNewTenantPM(signUpInfoClass, lead);

            using (TransactionScope scope = TransactionFactory.GetNewTransaction(new TimeSpan(2, 0, 0)))//new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(2, 0, 0)))
            {
                TenantRepository tenantZeroRepository = new TenantRepository(0);
                Tenant tenantZero = tenantZeroRepository.GetSingleTenant(0);

                if (tenantZero != null)
                {
                    newTenant = MapTenantZeroDetailsToNewTenant(newTenant, tenantZero);
                }
                scope.Complete();
            }

            newTenant.PasswordPolicyCode = GetPasswordPolicyCode(newTenant);

            ICommonDataContext commonContext = CommonDataContext.GetContext(newTenant.Id);
            TenantService service = new TenantService(commonContext, newTenant.Id);
            service.Create(newTenant);

            return newTenant.Id;
        }

        private static TenantPM GetNewTenantPM(SignUpInfoClass signUpInfoClass, LogitudeLead lead)
        {
            TenantPM newTenant = new TenantPM()
            {
                Company = signUpInfoClass.Company,
                Language = "EN",
                VolumeUnitCode = "CBM",
                GrossWeightUnitCode = "KG",
                DimensionsUnitCode = "Cm",
                ChargeableWeightUnitCode = "KG",
                WeightMeasurementUnitCode = "MT",
                ExportFreightPrepaidCollectId = "P",
                ExportOtherPrepaidCollectId = "P",
                ImportFreightPrepaidCollectId = "P",
                ImportOtherPrepaidCollectId = "P",
                DateTimeFormat = @"dd\/MM\/yyyy", //"dd/mm/yyyy hh:mm:ss tt",
                CASSCode = lead != null ? lead.CASSCode : "",
                IATA = lead != null ? lead.IATACode : "",
                PackageCode = signUpInfoClass.PackageCode,
                CreateTenantFromSignUp = true,
                TimeZoneOffset = null,
                CheckDigitControlAlgorithmCode = "NONE",
                TransferQuotationsToUnifreightTrigger = "OnSend",
            };

            return newTenant;
        }

        private static TenantPM MapTenantZeroDetailsToNewTenant(TenantPM newTenant, Tenant tenantZero)
        {
            newTenant.MasterExportFreightPrepaidCollectId = tenantZero.MasterExportFreightPrepaidCollectId;
            newTenant.MasterExportOtherPrepaidCollectId = tenantZero.MasterExportOtherPrepaidCollectId;
            newTenant.MasterImportFreightPrepaidCollectId = tenantZero.MasterImportFreightPrepaidCollectId;
            newTenant.MasterImportOtherPrepaidCollectId = tenantZero.MasterImportOtherPrepaidCollectId;

            newTenant.AirRatio = tenantZero.AirRatio;
            newTenant.FCLRatio = tenantZero.FCLRatio;
            newTenant.LCLRatio = tenantZero.LCLRatio;
            newTenant.FTLRatio = tenantZero.FTLRatio;
            newTenant.LTLRatio = tenantZero.LTLRatio;

            if (CheckIsDayLightSettingsRequiredForEnvironment())
            {
                newTenant.DayLightStartDate = tenantZero.DayLightStartDate;
                newTenant.DayLightEndDate = tenantZero.DayLightEndDate;
                newTenant.DayLightOffset = tenantZero.DayLightOffset;
            }

            return newTenant;
        }

        private static string GetPasswordPolicyCode(TenantPM newTenant)
        {
            PasswordPolicyRepository passwordPoliciesRepository = new PasswordPolicyRepository(newTenant.Id);
            PasswordPolicy policy = passwordPoliciesRepository.GetSinglePasswordPolicy("MEDU");

            return policy.Code;
        }

        private static bool CheckIsDayLightSettingsRequiredForEnvironment()
        {
            if (!string.IsNullOrEmpty(LogitudeSettings.DeploymentStage) && (LogitudeSettings.DeploymentStage.ToLower() == "logboxwe1" || LogitudeSettings.DeploymentStage.ToLower() == "test2" || LogitudeSettings.DeploymentStage.ToLower() == "amitalstorage"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public static void UpdateLogBoxTenant(int tenant, SignUpInfoClass signUpInfoClass)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            TenantService service = new TenantService(commonContext, tenant);
            TenantQuery query = new TenantQuery(tenant);
            var newTenant = query.GetSinglePM(tenant);
            if (signUpInfoClass.PackageCode == "IMPO")
            {
                AddressRepository addressRepository = new AddressRepository(signUpInfoClass.Tenant);
                var CustomerAddress = addressRepository.GetMainAddressByCardId(signUpInfoClass.CustomerId, signUpInfoClass.Tenant);
                AddressService addressService = new AddressService(commonContext, tenant);
                CountryRepository CountryRepository = new CountryRepository(tenant);
                ContactRepository ContactRepository = new ContactRepository(signUpInfoClass.Tenant);
                ContactService ContactService = new ContactService(commonContext, tenant);
                CardQuery cardQuery = new CardQuery(signUpInfoClass.Tenant);
                CustomerQuery CustomerQuery = new CustomerQuery(signUpInfoClass.Tenant);
                var currentCard = cardQuery.GetSinglePM(signUpInfoClass.CustomerId, signUpInfoClass.Tenant);
                var currentCustomer = CustomerQuery.GetSinglePMForLogBox(signUpInfoClass.CustomerId, signUpInfoClass.Tenant);
                ContactQuery contactQuery = new ContactQuery(ContactRepository);
                var CurrentContact = contactQuery.GetSinglePM(currentCard.PrimaryContactId, signUpInfoClass.Tenant);
                CustomerQuery = new CustomerQuery(tenant);
                ContactPM contactPM = contactQuery.GetContactByEmailOnly(signUpInfoClass.Email, tenant);//commonContext.Contacts.Where(d => d.Tenant == tenant && d.Email == signUpInfoClass.Email).FirstOrDefault();
                                                                                                        //{
                                                                                                        //    Tenant = tenant,
                                                                                                        //    EnglishName = CurrentContact.EnglishName,
                                                                                                        //    InActive = false,
                                                                                                        //    DisplayGettingStarted = false,
                                                                                                        //    IsUser = true,
                                                                                                        //    Email = CurrentContact.Email,
                                                                                                        //    LocalName = CurrentContact.LocalName,
                                                                                                        //    BusinessPhone = CurrentContact.BusinessPhone,
                                                                                                        //    Mobile = CurrentContact.Mobile

                //};
                contactPM.SetAsPrimaryForCard = true;
                contactPM.IsCreatedWithPartner = true;
                //ContactService.Update(contactPM);
                CustomerPM customerPM = new CustomerPM()
                {
                    Tenant = tenant,
                    PrimaryContactId = contactPM.Id,
                    IsFirstContactToAdd = true,
                    EnglishName = currentCard.EnglishName,
                    LocalName = currentCard.LocalName,
                    PartnerTypeId = "CS",
                    Code = CodeCounter.GetNumber("Card", tenant).ToString(),
                    IsHybrid = true,
                    IsCustomer = true,
                    ExistedContactId = contactPM.Id,
                    Contacts = new List<ContactPM>(),
                };
                customerPM.Contacts.Add(contactPM);
                //var Contact = ContactRepository.GetSingleContactByEmailAndTenant("system@tenant" + tenant + ".com", tenant);
                CustomerService CustomerService = new CustomerService(commonContext, customerPM, contactPM.Id);
                CustomerService.Create();


                var NewCountry = CountryRepository.GetSingleCountryByCode(CustomerAddress.Country.Code, tenant);
                AddressPM TenantAddress = new AddressPM()
                {
                    Address1 = CustomerAddress.Address1,
                    Address2 = CustomerAddress.Address2,
                    AddressTypeId = CustomerAddress.AddressTypeId,
                    Name = CustomerAddress.Name,
                    City = CustomerAddress.City,
                    CountryId = NewCountry.Id,
                    IsLocalLanguage = true,
                    InActive = false,
                    Description = CustomerAddress.Description,
                    Tenant = tenant,
                    IsHybrid = true,
                };
                addressService.Create(TenantAddress);
                CurrencyRepository CurrencyRepository = new CurrencyRepository(0);
                CurrencyQuery currencyQuery = new CurrencyQuery(CurrencyRepository);
                var currencies = currencyQuery.GetCurrenciesByTenantPM(0).Where(d => d.Code == "USD" || d.Code == "NIS").ToList();
                CurrencyRepository = new CurrencyRepository(tenant);
                //foreach (CurrencyPM currency in currencies)
                //{
                //    Currency newCurrency = new Currency()
                //    {
                //        Code = currency.Code,
                //        EnglishName = currency.EnglishName,
                //        Id = IdCounter.GetNumber("Currency", tenant).ToString(),
                //        InActive = currency.InActive,
                //        LocalName = currency.LocalName,
                //        Notes = currency.Notes,
                //        Tenant = tenant,
                //        SearchFields = currency.SearchFields,
                //    };
                //    CurrencyRepository.Add(newCurrency);
                //}
                //CurrencyRepository.SubmitChanges();
                var Cur = CurrencyRepository.GetSingleCurrencyByCode("NIS", 0);
                var ProfCur = CurrencyRepository.GetSingleCurrencyByCode("USD", 0);
                Currency newCurrency = new Currency()
                {
                    Code = Cur.Code,
                    EnglishName = Cur.EnglishName,
                    Id = IdCounter.GetNumber("Currency", tenant).ToString(),
                    InActive = Cur.InActive,
                    LocalName = Cur.LocalName,
                    Notes = Cur.Notes,
                    Tenant = tenant,
                    SearchFields = Cur.SearchFields,
                };
                CurrencyRepository.Add(newCurrency);
                newCurrency = new Currency()
                {
                    Code = ProfCur.Code,
                    EnglishName = ProfCur.EnglishName,
                    Id = IdCounter.GetNumber("Currency", tenant).ToString(),
                    InActive = ProfCur.InActive,
                    LocalName = ProfCur.LocalName,
                    Notes = ProfCur.Notes,
                    Tenant = tenant,
                    SearchFields = ProfCur.SearchFields,
                };
                CurrencyRepository.Add(newCurrency);
                CurrencyRepository.SubmitChanges();
                CardRepository CardRepository = new CardRepository(signUpInfoClass.Tenant);
                var CrmCustomer = CardRepository.GetSingleCard(signUpInfoClass.CustomerId, signUpInfoClass.Tenant, false);
                newTenant.CurrencyId = Cur.Id;
                newTenant.ProfitCurrencyId = ProfCur.Id;
                newTenant.ProfitCurrencyRate = 4;
                newTenant.VatNumber = CrmCustomer.VatNumber;
                newTenant.AddressId = TenantAddress.Id;
                newTenant.IsDocumentsArchive = true;
                newTenant.CustomerId = customerPM.Id;
                newTenant.CustomerTenantShareImportFile = true;
                if (!string.IsNullOrEmpty(newTenant.PrivateLabelId))
                {
                    newTenant.AutoArchiveOnInvoice = true;
                    newTenant.DocumentShareAsDefault = true;
                }
                service.Update(newTenant);
            }
        }
        
        public static void AddBranchesAndDepartments(int theTenant, BranchRepository theBranchRepository, DepartmentRepository theDepartmentRepository)
        {
            Department deb;
            Branch branch;
            Department debartment2;
            Department debartment3;
            deb = new Department()
            {
                Id = IdCounter.GetNumber("Department", theTenant).ToString(),
                Tenant = theTenant,
                EnglishName = "Management",
                LocalName = "Management",
                InActive = false,
                SearchFields = "Management",
            };

            debartment2 = new Department()
            {
                Id = IdCounter.GetNumber("Department", theTenant).ToString(),
                Tenant = theTenant,
                EnglishName = "Accounting",
                LocalName = "Accounting",
                InActive = false,
                SearchFields = "Accounting",
            };
            debartment3 = new Department()
            {
                Id = IdCounter.GetNumber("Department", theTenant).ToString(),
                Tenant = theTenant,
                EnglishName = "Operational",
                LocalName = "Operational",
                InActive = false,
                SearchFields = "Operational",
            };
            theDepartmentRepository.Add(deb);
            theDepartmentRepository.Add(debartment2);
            theDepartmentRepository.Add(debartment3);
            theDepartmentRepository.SubmitChanges();

            branch = new Branch()
            {
                Id = IdCounter.GetNumber("Branch", theTenant).ToString(),
                Tenant = theTenant,
                EnglishName = "Main Office",
                LocalName = "Main Office",
                InActive = false,
                SearchFields = "Main Office",
            };
            theBranchRepository.Add(branch);
            theBranchRepository.SubmitChanges();
        }

        public static string AddUser(UserShortDetails userShortDetails, UserRepository theUserRepository, BranchRepository theBranchRepository, DepartmentRepository theDepartmentRepository, RoleRepository theRoleRepository)
        {
            ContactRepository contactRepository = new ContactRepository(tenant);
            ContactTenantRepository contactTenantRepository = new ContactTenantRepository(tenant);
            ContactTenantRoleRepository contactTenantRoleRepository = new ContactTenantRoleRepository(tenant);
            UserPM user = GetNewUserPM(userShortDetails, theBranchRepository, theDepartmentRepository);

            InsertUser(user, theUserRepository, contactRepository, theRoleRepository, contactTenantRoleRepository, contactTenantRepository);

            if (!user.Email.Contains("system@tenant") && !user.Email.Contains("customercare@logitudeworld"))
            {
                SetUserData(userShortDetails.Tenant);
            }

            return user.Password;
        }

        private static UserPM GetNewUserPM(UserShortDetails userShortDetails, BranchRepository theBranchRepository, DepartmentRepository theDepartmentRepository)
        {
            BranchQuery branchQuery = new BranchQuery(theBranchRepository);
            BranchPM branch = branchQuery.GetBranchByName("Main Office", tenant);
            DepartmentQuery departmentQuery = new DepartmentQuery(theDepartmentRepository);
            DepartmentPM department = departmentQuery.GetDepartmentByName("Management", tenant);
            UserPM user = new UserPM
            {
                Tenant = userShortDetails.Tenant,
                InternetAccess = true,
                Email = userShortDetails.Email,
                BusinessUnitId = userShortDetails.Tenant.ToString(),
                DontShowLocal = setting.WorkEnvironment == "customs" ? false : true,
                EnglishName = userShortDetails.Name,
                Birthday = DateTime.Now,
                Fax = "",
                Mobile = "",
                LocalName = userShortDetails.Name,
                Anniversary = DateTime.Now,
                DepartmentId = department.Id,
                BranchId = branch.Id,
                BusinessPhone = userShortDetails.PhoneNumber,
                SignupRole = true,
                SearchFields = userShortDetails.Email + "," + userShortDetails.Name + "," + userShortDetails.PhoneNumber
            };

            using (TransactionScope scope = TransactionFactory.GetNewTransaction(new TimeSpan(2, 0, 0)))//new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(2, 0, 0)))
            {
                IGlobalContext globalContext = GlobalContext.GetContext();
                ContactPassword contactPassword = globalContext.ContactPasswords.Where(c => c.Email == user.Email).FirstOrDefault();
                user.Password = contactPassword == null ? PasswordGenerator.Generate(8) : null;

                scope.Complete();
            }

            user.DontShowLocal = setting.WorkEnvironment == "customs" ? false : true;
            user.UserType = user.Email.Contains("system@tenant") ? "S" : "R";
            return user;
        }

        public static void MapUserToContact(UserPM user, Contact contact)
        {
            contact.Anniversary = user.Anniversary;
            contact.Birthday = user.Birthday;
            contact.BusinessPhone = user.BusinessPhone;
            contact.Email = user.Email;
            contact.EnglishName = user.EnglishName;
            contact.FacebookId = user.FacebookId;
            contact.Fax = user.Fax;
            contact.InActive = user.InActive;
            contact.LocalName = user.LocalName;
            contact.Mobile = user.Mobile;
            //contact.Password = user.Password;
            contact.Notes = user.Notes;
            contact.Tenant = user.Tenant;
            contact.SearchFields = user.SearchFields;
            contact.DisplayGettingStarted = true;

            if (setting.WorkEnvironment == "customs")
            {
                contact.DontShowLocalLabels = false;
            }
            else
            {
                contact.DontShowLocalLabels = true;
            }
            // contact.IsUser = true;
        }

        public static void MapUserUserPM(UserPM userPM, User user)
        {
            user.BranchId = userPM.BranchId;
            user.DepartmentId = userPM.DepartmentId;
            user.Notes = userPM.Notes;
            user.Tenant = userPM.Tenant;
            user.SearchFields = userPM.SearchFields;
            user.BusinessUnitId = userPM.BusinessUnitId;
        }

        public static void InsertUser(UserPM user, UserRepository usersRepository, ContactRepository contactsRepository, RoleRepository rolesRepository, ContactTenantRoleRepository contactTenantRolesRepository, ContactTenantRepository contactTenantsRepository)
        {
            SecurityUtility.AuthenticationOnTenant(user.Tenant);
            // this.ChangeConnectionString(user.Tenant);
            //if (!ContactsRepository.CheckEmailAvailabilityForTenant(user.Email, user.Tenant))
            //{

            user.Email = user.Email.ToLower();
            Contact newContact = new Contact();
            MapUserToContact(user, newContact);
            Contact adminContact = contactsRepository.GetSingleContactByEmail("admin@fnarsoft.com", 0);
            if (adminContact != null)
            {
                newContact.Signature = adminContact.Signature;
                newContact.SignatureHtml = adminContact.SignatureHtml;
            }

            #region insertContact
            newContact.Id = IdCounter.GetNumber("Contact", user.Tenant).ToString();
            newContact.ComputedKey = (!string.IsNullOrEmpty(newContact.Email) ? newContact.Email : newContact.Id);
            //newContact.MustChangePassword = true;
            newContact.UserType = user.UserType;

            //if (string.IsNullOrEmpty(user.Password))
            //{
            //    user.Password = "123";
            //}

            //newContact.Password = PasswordGenerator.GetHashedPassword(user.Email,user.Password);

            ContactTenant newContactTenant = new ContactTenant()
            {
                Id = IdCounter.GetNumber("ContactTenant", user.Tenant).ToString(),
                TenantId = newContact.Tenant,
                ContactId = newContact.Id,
            };
            contactsRepository.Add(newContact);
            RoleQuery roleQuery = new RoleQuery(rolesRepository);

            #region admin role for signup
            if (user.SignupRole)
            {
                RolePM adimnrole = roleQuery.GetSinglePMByName("Administrator", 0);

                ContactTenantRole admincontactTenantRole = new ContactTenantRole()
                {
                    ContactTenantId = newContactTenant.Id,
                    Id = IdCounter.GetNumber("ContactTenantRole", newContact.Tenant).ToString(),
                    RoleId = adimnrole.Id,
                    Tenant = newContact.Tenant,
                };
                contactTenantRolesRepository.Add(admincontactTenantRole);



                RolePM BillingRole = roleQuery.GetSinglePMByName("Billing", 0);

                ContactTenantRole billcontactTenantRole = new ContactTenantRole()
                {
                    ContactTenantId = newContactTenant.Id,
                    Id = IdCounter.GetNumber("ContactTenantRole", newContact.Tenant).ToString(),
                    RoleId = BillingRole.Id,
                    Tenant = newContact.Tenant,
                };
                contactTenantRolesRepository.Add(billcontactTenantRole);
            }
            #endregion

            if (!string.IsNullOrEmpty(newContact.Email))
            {
                using (TransactionScope scope = TransactionFactory.GetNewTransaction(new TimeSpan(2, 0, 0)))//new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(2, 0, 0)))
                {
                    IGlobalContext globalContext = GlobalContext.GetContext();
                    ContactPassword contactPassword = globalContext.ContactPasswords.Where(c => c.Email == user.Email).FirstOrDefault();
                    if (contactPassword == null)
                    {
                        string newPassword = PasswordGenerator.GetBCryptHashedPassword(user.Email, user.Password);

                        contactPassword = new ContactPassword()
                        {
                            Email = user.Email,
                            Password = newPassword,
                            IsLocked = false,
                            NumberOfRetries = 0,
                            MustChangePassword = true,
                            IsBCrypt = true,

                        };

                        globalContext.ContactPasswords.Add(contactPassword);
                    }



                    GlobalContactRepository globalContactRep = new GlobalContactRepository(globalContext);

                    bool globalContactExists = (from a in globalContactRep.GetGlobalContactByTenant(newContact.Tenant)
                                                where a.Email == newContact.Email
                                                select a).Any();

                    //bool globalContactIdExists = (from a in globalContactRep.GetGlobalContactByTenant(newContact.Tenant)
                    //                            where a.Id==newContact.Id
                    //                            select a).Any();
                    //while (globalContactIdExists)
                    //{
                    //    newContact.Id = IdCounter.GetNumber("Contact", newContact.Tenant);
                    //    globalContactIdExists = (from a in globalContactRep.GetGlobalContactByTenant(newContact.Tenant)
                    //                             where a.Id == newContact.Id
                    //                             select a).Any();
                    //}

                    if (!globalContactExists)
                    {

                        GlobalContact gcontact = new GlobalContact() { Email = newContact.Email, Id = newContact.Id, GlobalTenantId = newContact.Tenant, IsUser = true, };

                        globalContactRep.Add(gcontact);
                        globalContactRep.SubmitChanges();
                        scope.Complete();
                    }
                    //throw new Exception("hahahahahaha");

                }
                contactTenantsRepository.Add(newContactTenant);
            }
            #endregion

            User newUser = new User();
            newUser.Id = newContact.Id;
            user.Id = newUser.Id;
            MapUserUserPM(user, newUser);
            usersRepository.Add(newUser);
            contactsRepository.SubmitChanges();
            usersRepository.SubmitChanges();
            contactTenantsRepository.SubmitChanges();
            contactTenantRolesRepository.SubmitChanges();
            // }
            //else
            //{
            //    throw new ApplicationException("This user already exists in this tenant.");
            //}
        }

        public static void AddCounters(int theTenant, CounterRepository theCounterRepository, List<ObjectTable> tenantZeroObjectTables, List<Counter> tenantZeroCounters)
        {
            foreach (Counter counter in tenantZeroCounters)
            {
                ObjectTable zeroObjectTable = tenantZeroObjectTables.Where(d => d.Id == counter.ObjectTableId).FirstOrDefault();
                if (zeroObjectTable != null)
                {
                    //ObjectTable currentObjectTable = CurrentTenantObjectTables.Where(d => d.Name == zeroObjectTable.Name).FirstOrDefault();
                    Counter newCounterPM = new Counter()
                    {
                        Tenant = theTenant,
                        ObjectTableId = zeroObjectTable.Id,
                        Code = counter.Code,
                        Name = counter.Name,
                        Id = IdCounter.GetNumber("Counter", theTenant).ToString(),
                    };
                    theCounterRepository.Add(newCounterPM);
                }
            }
            theCounterRepository.SubmitChanges();
        }

        public static void AddCounterDefinitions(int theTenant, CounterDefinitionRepository theCounterDefinitionRepository, List<Counter> tenantZeroCounters, List<Counter> currentTenantCounters, List<CounterDefinition> tenantZeroCounterDefinition)
        {
            foreach (CounterDefinition counterDef in tenantZeroCounterDefinition)
            {
                Counter zeroCounter = tenantZeroCounters.Where(d => d.Id == counterDef.CounterId).FirstOrDefault();
                Counter counter = currentTenantCounters.Where(d => d.Code == zeroCounter.Code).FirstOrDefault();
                CounterDefinition newCounterDefinition = new CounterDefinition()
                {
                    Id = IdCounter.GetNumber("CounterDefinition", theTenant).ToString(),
                    Tenant = theTenant,
                    CounterId = counter.Id,
                    Parameter1 = counterDef.Parameter1,
                    Parameter2 = counterDef.Parameter2,
                    Prefix = counterDef.Prefix,
                    UniquePerPrefix = counterDef.UniquePerPrefix,
                    StartNumber = counterDef.StartNumber,
                };
                theCounterDefinitionRepository.Add(newCounterDefinition);
            }
            theCounterDefinitionRepository.SubmitChanges();
        }

        public static void AddTenantSettings(int theTenant, TenantSettingRepository theTenantSettingRepository, List<ObjectTable> tenantZeroObjectTables)
        {
            tenantSettingQuery = new TenantSettingQuery(theTenantSettingRepository);
            List<TenantSettingPM> settings = tenantSettingQuery.GetTenantSettingsByTenant(0).ToList();
            foreach (TenantSettingPM setting in settings)
            {
                ObjectTable zeroObjectTable = tenantZeroObjectTables.Where(d => d.Id == setting.ObjectTableId).FirstOrDefault();
                if (zeroObjectTable != null)
                {
                    TenantSetting newTenantSetting = new TenantSetting()
                    {
                        Tenant = theTenant,
                        ObjectTableId = zeroObjectTable.Id,
                        SettingCode = setting.SettingCode,
                        SettingValue = setting.SettingValue,
                        Id = IdCounter.GetNumber("TenantSetting", theTenant).ToString(),
                        Size = setting.Size,
                        Prefix = setting.Prefix,
                    };
                    theTenantSettingRepository.Add(newTenantSetting);
                }
            }
            theTenantSettingRepository.SubmitChanges();
        }

        public static void AddPaymentTerms(int theTenant, PaymentTermRepository thePaymentTermRepository, List<PaymentTerm> tenantZeroPaymentTerms)
        {
            foreach (PaymentTerm a in tenantZeroPaymentTerms)
            {
                PaymentTerm newPaymentTerm = new PaymentTerm()
                {
                    AddedManually = a.AddedManually,
                    Description = a.Description,
                    Days = a.Days,
                    InActive = a.InActive,
                    Id = IdCounter.GetNumber("PaymentTerm", theTenant).ToString(),
                    EnglishName = a.EnglishName,
                    LocalDescription = a.LocalDescription,
                    LocalName = a.LocalName,
                    Tenant = theTenant,
                    SearchFields = a.SearchFields,
                    IsManuallySet = a.IsManuallySet,
                    DisplayInLOV = a.DisplayInLOV,
                    FromDateTypeCode = "INV",
                    Code = a.Code,
                };

                thePaymentTermRepository.Add(newPaymentTerm);
            }

            thePaymentTermRepository.SubmitChanges();
        }

        public static void AddDocumentTypes(int theTenant, DocumentTypeRepository theDocumentTypeRepository, DocumentTypeCopyRepository theDocumentTypeCopyRepository, List<DocumentTypePM> tenantZeroDocumentTypes, List<ObjectTable> tenantZeroObjectTables, List<ObjectTable> currentTenantObjectTables, DocumentTypeCustomFieldRepository theDocumentTypeCustomFieldRepository, List<DocumentTypeCustomField> tenantZeroCustomFields, string countryCode=null)
        {
            //string countryCode = GetCurrentTenantCountryCode(tenant);

            foreach (DocumentTypePM docType in tenantZeroDocumentTypes)
            {
                AutomationHelper automationHelper = new AutomationHelper();
                List<string> automationDocumentTypeIds = automationHelper.GetAutomationDocumentTypeIds(tenant);

                if (((!docType.InActive && docType.IsCopiedAtSignup && docType.IsEnabledForCustomers) || automationDocumentTypeIds.Contains(docType.Id)) && (string.IsNullOrEmpty(docType.CountryCode?.Trim()) || (docType.CountryCode == countryCode)))
                {
                    ObjectTable tenantZeroObject = tenantZeroObjectTables.Where(d => d.Id == docType.ObjectTableId).FirstOrDefault();
                    if (tenantZeroObject != null)
                    {
                        List<DocumentTypeCustomField> zeroCustomFields = tenantZeroCustomFields.Where(d => d.DocumentTypeId == docType.Id).ToList();
                        DocumentType newDocType = new DocumentType()
                        {
                            Id = IdCounter.GetNumber("DocumentType", theTenant).ToString(),
                            Code = docType.Code,
                            Name = docType.Name,
                            IsOcean = docType.IsOcean,
                            IsAir = docType.IsAir,
                            IsInland = docType.IsInland,
                            IsDocIn = docType.IsDocIn,
                            IsDocOut = false,
                            FollowUpTypeId = docType.FollowUpTypeId,
                            Tenant = theTenant,
                            ObjectTableId = tenantZeroObject != null ? tenantZeroObject.Id : "",
                            SearchFields = docType.SearchFields,
                            IsMaster = docType.IsMaster,
                            IsDirect = docType.IsDirect,
                            IsHouse = docType.IsHouse,
                            TemplateFormatCode = docType.TemplateFormatCode,
                            IsEnabledForCustomers = true,
                            IsCopiedAtSignup = true,
                            CountryCode = docType.CountryCode,
                            Subject = docType.Subject,
                            Notes = docType.Notes,
                            OrderBy = docType.OrderBy,
                            DocumentTypeCategoryCode = docType.DocumentTypeCategoryCode,
                            //IsAgentView = docType.IsAgentView,
                            //IsCustomerView = docType.IsCustomerView,
                            IsSystemAdditionalPrintingFields = docType.IsSystemAdditionalPrintingFields,
                            PrintingFieldsScreenCode = docType.PrintingFieldsScreenCode,
                            OnPrintPopulateDateFieldName = docType.OnPrintPopulateDateFieldName,
                            OnSendPopulateDateFieldName = docType.OnSendPopulateDateFieldName,
                            OnUploadPopulateDateFieldName = docType.OnUploadPopulateDateFieldName,
                        };
                        foreach (DocumentTypeCopyPM copy in docType.DocumentTypeCopies)
                        {
                            if (!copy.InActive)
                            {
                                DocumentTypeCopy newCopy = new DocumentTypeCopy()
                                {
                                    Id = IdCounter.GetNumber("DocumentTypeCopy", theTenant).ToString(),
                                    Code = copy.Code,
                                    Name = copy.Name,
                                    Tenant = theTenant,
                                    IndexOrder = copy.IndexOrder,
                                    IsSelectedByDefault = copy.IsSelectedByDefault,
                                    DocumentTypeId = newDocType.Id,
                                };
                                theDocumentTypeCopyRepository.Add(newCopy);
                            }
                        }
                        foreach (DocumentTypeCustomField customField in zeroCustomFields)
                        {
                            DocumentTypeCustomField newCustomField = new DocumentTypeCustomField()
                            {
                                DocumentTypeId = newDocType.Id,
                                DefaultValue = customField.DefaultValue,
                                FieldCode = customField.FieldCode,
                                FieldDataTypeCode = customField.FieldDataTypeCode,
                                Id = IdCounter.GetNumber("DocumentTypeCustomField", theTenant).ToString(),
                                InActive = customField.InActive,
                                IndexOrder = customField.IndexOrder,
                                IsRequired = customField.IsRequired,
                                MultiLine = customField.MultiLine,
                                Name = customField.Name,
                                Tenant = theTenant,
                            };
                            theDocumentTypeCustomFieldRepository.Add(newCustomField);
                        }

                        theDocumentTypeRepository.Add(newDocType);
                    }
                }
            }
            theDocumentTypeRepository.SubmitChanges();
            theDocumentTypeCopyRepository.SubmitChanges();
            theDocumentTypeCustomFieldRepository.SubmitChanges();
        }

        public static void AddDocumentTypeTemplates(int theTenant, DocumentTypeTemplateRepository theDocumentTypeTemplateRepository, List<DocumentTypePM> tenantZeroDocumentType, List<DocumentType> currentTenantDocumentType, DocumentTypeRepository theDocumentTypeRepository, string coutryCode = null)
        {
            DocumentTypeTemplateQuery theDocumentTypeTemplateQuery = new DocumentTypeTemplateQuery(theDocumentTypeTemplateRepository);
            List<DocumentTypeTemplatePM> documentTypeTemplateList = theDocumentTypeTemplateQuery.GetDocumentTypeTemplatePMsByTenant(0).ToList();
            bool sameCountry = false;
            AutomationHelper automationHelper = new AutomationHelper();
            List<string> automationDocumentTypeTemplateIds = automationHelper.GetAutomationDocumentTypeTemplateIds(tenant);

            documentTypeTemplateList = documentTypeTemplateList.Where(d => (d.IsCopiedAtSignup && d.IsEnabledForCustomers) || automationDocumentTypeTemplateIds.Contains(d.Id)).ToList();
            
            foreach (DocumentTypePM documenttype in tenantZeroDocumentType)
            {
                DocumentType usedDocumenttype = currentTenantDocumentType.Where(d => d.Code == documenttype.Code && d.Tenant == theTenant && !d.InActive && d.IsCopiedAtSignup).FirstOrDefault();
                sameCountry = documenttype.CountryCode == coutryCode;
                if (usedDocumenttype != null && (string.IsNullOrEmpty(documenttype.CountryCode?.Trim()) || sameCountry))
                {
                    List<DocumentTypeTemplate> documentTypeTemplates = new List<DocumentTypeTemplate>();
                    foreach (DocumentTypeTemplatePM documentTypeTemplatePM in documentTypeTemplateList.Where(d => d.DocumentTypeId == documenttype.Id))
                    {
                        if (sameCountry || (string.IsNullOrEmpty(documentTypeTemplatePM.CountryCode?.Trim()) || documentTypeTemplatePM.CountryCode == coutryCode)) {
                            DocumentTypeTemplate newDocumentTypeTemplate = GetInstanceFromDocumentTypeTemplate(theTenant, usedDocumenttype, documentTypeTemplatePM);
                            theDocumentTypeTemplateRepository.Add(newDocumentTypeTemplate);
                            documentTypeTemplates.Add(newDocumentTypeTemplate); 
                        }
                    }
                    usedDocumenttype.DocumentTypeDefaultReportTemplateId = GetDocumentTypeDefaultReportTemplateId(documentTypeTemplates, documenttype, coutryCode);
                    usedDocumenttype.DocumentTypeDefaultHTMLTemplateId = GetDocumentTypeDefaultHTMLTemplateId(documentTypeTemplates, documenttype, coutryCode);

                    usedDocumenttype.IsDocOut = documenttype.IsDocOut;
                    theDocumentTypeRepository.Update(usedDocumenttype);
                }
            }

            theDocumentTypeTemplateRepository.SubmitChanges();
            theDocumentTypeRepository.SubmitChanges();
        }

        private static string GetDocumentTypeDefaultHTMLTemplateId(List<DocumentTypeTemplate> documentTypeTemplates, DocumentTypePM documenttype, string coutryCode)
        {
            DocumentTypeTemplate documentTypeDefaultHTMLTemplate = documentTypeTemplates.Where(d => d.CountryCode == coutryCode && d.TemplateType == "M" && d.OriginalTemplateId == documenttype.DocumentTypeDefaultHTMLTemplateId).FirstOrDefault();

            if (documentTypeDefaultHTMLTemplate == null)
            {
                documentTypeDefaultHTMLTemplate = documentTypeTemplates.Where(d => d.TemplateType == "M" && d.OriginalTemplateId == documenttype.DocumentTypeDefaultHTMLTemplateId).FirstOrDefault();
            }

            if (documentTypeDefaultHTMLTemplate == null)
            {
                documentTypeDefaultHTMLTemplate = documentTypeTemplates.Where(d => d.TemplateType == "M").FirstOrDefault();
            }

            return documentTypeDefaultHTMLTemplate != null ? documentTypeDefaultHTMLTemplate.Id : null;
        }

        private static string GetDocumentTypeDefaultReportTemplateId(List<DocumentTypeTemplate> documentTypeTemplates, DocumentTypePM documenttype, string coutryCode)
        {
            DocumentTypeTemplate documentTypeDefaultReportTemplate = documentTypeTemplates.Where(d => d.CountryCode == coutryCode && d.TemplateType == "P" && d.OriginalTemplateId == documenttype.DocumentTypeDefaultReportTemplateId).FirstOrDefault();

            if (documentTypeDefaultReportTemplate == null)
            {
                documentTypeDefaultReportTemplate = documentTypeTemplates.Where(d => d.TemplateType == "P" && d.OriginalTemplateId == documenttype.DocumentTypeDefaultReportTemplateId).FirstOrDefault();
            }

            if (documentTypeDefaultReportTemplate == null)
            {
                documentTypeDefaultReportTemplate = documentTypeTemplates.Where(d => d.TemplateType == "P").FirstOrDefault();
            }

            return documentTypeDefaultReportTemplate!=null? documentTypeDefaultReportTemplate.Id:null;
        }

        private static  DocumentTypeTemplate GetInstanceFromDocumentTypeTemplate(int theTenant, DocumentType documenttype, DocumentTypeTemplatePM documentTypeTemplatePM)
        {
            return  new DocumentTypeTemplate()
            {
                Id = IdCounter.GetNumber("DocumentTypeTemplate", theTenant).ToString(),
                Tenant = theTenant,
                TemplateBody = documentTypeTemplatePM.TemplateBody,
                TemplateBodyHtml = documentTypeTemplatePM.TemplateBodyHtml,
                TemplateFooterHeight = documentTypeTemplatePM.TemplateFooterHeight,
                TemplateHeaderHeight = documentTypeTemplatePM.TemplateHeaderHeight,
                TemplateFooterHtml = documentTypeTemplatePM.TemplateFooterHtml,
                TemplateHeaderHtml = documentTypeTemplatePM.TemplateHeaderHtml,
                TemplateType = documentTypeTemplatePM.TemplateType,
                HorizontalShift = documentTypeTemplatePM.HorizontalShift,
                InActive = documentTypeTemplatePM.InActive,
                Description = documentTypeTemplatePM.Description,
                DocumentTypeId = documenttype.Id,
                EditorTool = documentTypeTemplatePM.EditorTool,
                VerticalShift = documentTypeTemplatePM.VerticalShift,
                IsEnabledForCustomers = true,
                IsCopiedAtSignup = true,
                CountryCode = documentTypeTemplatePM.CountryCode,
                Language = documentTypeTemplatePM.Language,
                OriginalTemplateId = documentTypeTemplatePM.Id,
                InternalRemarks = documentTypeTemplatePM.InternalRemarks,
                Subject = documentTypeTemplatePM.Subject,
                From = documentTypeTemplatePM.From,
                CC = documentTypeTemplatePM.CC,
                ReplyTo = documentTypeTemplatePM.ReplyTo,
                To = documentTypeTemplatePM.To,
            };
        }

        public static void AddEntityStatus(int theTenant, EntityStatusRepository theEntityStatusRepository, List<EntityStatusPM> tenantZeroEntityStatus, List<ObjectTable> tenantZeroObjectTables)
        {
            foreach (EntityStatusPM entityStatus in tenantZeroEntityStatus)
            {
                EntityStatus newEntityStatus = new EntityStatus()
                {
                    Id = IdCounter.GetNumber("EntityStatus", theTenant).ToString(),
                    Tenant = theTenant,
                    Name = entityStatus.Name,
                    Code = entityStatus.Code,
                    ObjectTableId = tenantZeroObjectTables.Where(d => d.Name == entityStatus.ObjectTableName).FirstOrDefault().Id,
                    StatusWeight = entityStatus.StatusWeight,
                    SearchFields = entityStatus.SearchFields,
                    DisplayName = !string.IsNullOrEmpty(entityStatus.DisplayName)? entityStatus.DisplayName: entityStatus.Name,
                };
                theEntityStatusRepository.Add(newEntityStatus);
            }
            theEntityStatusRepository.SubmitChanges();
        }

        public static void AddEventTypes(int theTenant, EventTypeRepository theEventTypeRepository, List<EventTypePM> tenantZeroEventTypes, List<ObjectTable> tenantZeroObjectTables, List<EntityStatus> currentTenantEntityStatus, List<EntityStatusPM> tenantZeroEntityStatus)
        {
            foreach (EventTypePM eventType in tenantZeroEventTypes)
            {
                ObjectTable tenantZeroObject = tenantZeroObjectTables.Where(d => d.Id == eventType.ObjectTableId).FirstOrDefault();

                if (tenantZeroObject != null)
                {
                    EventType newEventType = new EventType()
                    {
                        Id = IdCounter.GetNumber("EventType", theTenant).ToString(),
                        Tenant = theTenant,
                        AddedManually = eventType.AddedManually,
                        Code = eventType.Code,
                        EnglishName = eventType.EnglishName,
                        FollowUpEnglishName = eventType.FollowUpEnglishName,
                        FollowUpLocalName = eventType.FollowUpLocalName,
                        ManualActivatedFollowUp = eventType.ManualActivatedFollowUp,
                        IsFollowUp = eventType.IsFollowUp,
                        IsManualEntry = eventType.IsManualEntry,
                        ObjectTableId = tenantZeroObject.Id,
                        EventTypeCategoryCode = eventType.EventTypeCategoryCode,
                        LocalName = eventType.LocalName,
                        ShortView = eventType.ShortView,
                        SearchFields = eventType.SearchFields,
                        IsCustomerView = eventType.IsCustomerView,                         
                    };

                    if (eventType.EntityStatusId != null)
                    {
                        EntityStatusPM tenantZeroStatus = tenantZeroEntityStatus.Where(d => d.Id == eventType.EntityStatusId).FirstOrDefault();
                        if (tenantZeroStatus != null)
                        {
                            EntityStatus tenantStatus = currentTenantEntityStatus.Where(d => d.Code == tenantZeroStatus.Code && d.ObjectTableId == tenantZeroStatus.ObjectTableId).FirstOrDefault();
                            if (tenantStatus != null)
                            {
                                newEventType.EntityStatusId = tenantStatus.Id;
                            }
                        }
                    }

                    theEventTypeRepository.Add(newEventType);
                }
            }

            theEventTypeRepository.SubmitChanges();
        }

        public static void AddMeasurements(int theTenant, MeasurementRepository theMeasurementRepository, List<MeasurementPM> tenantZeroMeasurements)
        {
            List<MeasurementPM> measurementsList = tenantZeroMeasurements.Where(d => d.Tenant == 0 && d.IsContainer == false).ToList();
            foreach (MeasurementPM measurement in measurementsList)
            {
                Measurement newMeasurement = new Measurement()
                {
                    InActive = measurement.InActive,
                    Tenant = theTenant,
                    Code = measurement.Code,
                    IsContainer = measurement.IsContainer,
                    IsContainerMeasurement = measurement.IsContainerMeasurement,
                    Name = measurement.Name,
                    ShortName = measurement.ShortName,
                    SearchFields = measurement.SearchFields,
                    Id = IdCounter.GetNumber("Measurement", theTenant).ToString(),
                };
                theMeasurementRepository.Add(newMeasurement);
            }
            theMeasurementRepository.SubmitChanges();
        }

        public static void AddCreditCardTypes(int theTenant, CreditCardTypeRepository creditCardTypeRepository, List<CreditCardTypePM> tenantZeroCreditCardTypes)
        {
            List<CreditCardTypePM> creditCardTypesList = tenantZeroCreditCardTypes.Where(d => d.Tenant == 0).ToList();
            foreach (CreditCardTypePM creditCardType in creditCardTypesList)
            {
                CreditCardType newCreditCardType = new CreditCardType()
                {
                    Tenant = theTenant,
                    Code = creditCardType.Code,
                    Name = creditCardType.Name,
                    SearchFields = creditCardType.SearchFields,
                    Id = IdCounter.GetNumber("CreditCardType", theTenant).ToString(),
                };
                creditCardTypeRepository.Add(newCreditCardType);
            }
            creditCardTypeRepository.SubmitChanges();
        }

        public static void AddMoveTypes(int theTenant, MoveTypeRepository moveTypeRepository, List<MoveTypePM> tenantZeroMoveTypes)
        {
            List<MoveTypePM> moveTypesList = tenantZeroMoveTypes.Where(d => d.Tenant == 0).ToList();
            foreach (MoveTypePM moveType in moveTypesList)
            {
                MoveType newMoveType = new MoveType()
                {
                    Tenant = theTenant,
                    AddedManually = moveType.AddedManually,
                    InActive = moveType.InActive,
                    MoveTypeLocalName = moveType.MoveTypeLocalName,
                    TransportModeId = moveType.TransportModeId,
                    Code = moveType.Code,
                    MoveTypeEnglishName = moveType.MoveTypeEnglishName,
                    SearchFields = moveType.SearchFields,
                    Id = IdCounter.GetNumber("MoveType", theTenant).ToString(),
                };
                moveTypeRepository.Add(newMoveType);
            }
            moveTypeRepository.SubmitChanges();
        }

        public static void AddVatTypes(int theTenant, VatTypeRepository theVatTypeRepository, List<VatType> tenantZeroVatTypes, VatTypePercentageRepository theVatTypePercentageRepository)
        {
            foreach (VatType vattype in tenantZeroVatTypes)
            {
                VatType newVatType = new VatType()
                {
                    Id = IdCounter.GetNumber("VatType", theTenant).ToString(),
                    InActive = vattype.InActive,
                    AddedManually = vattype.AddedManually,
                    Code = vattype.Code,
                    EnglishName = vattype.EnglishName,
                    LocalName = vattype.LocalName,
                    Tenant = theTenant,
                    Description = vattype.Description,
                    LocalDescription = vattype.LocalDescription,
                    SearchFields = vattype.SearchFields,
                };
                theVatTypeRepository.Add(newVatType);
                if (newVatType.Code == "ZERO" || newVatType.Code == "EXMPT")
                {
                    VatTypePercentage vattypePercentage = new VatTypePercentage()
                    {
                        FromDate = new DateTime(2012, 1, 1),
                        Id = IdCounter.GetNumber("VatTypePercentage", theTenant).ToString(),
                        Percentage = 0,
                        Tenant = theTenant,
                        VatTypeId = newVatType.Id,
                    };
                    theVatTypePercentageRepository.Add(vattypePercentage);
                }
            }
            theVatTypeRepository.SubmitChanges();
            theVatTypePercentageRepository.SubmitChanges();
        }

        public static void AddChargesTypes(int theTenant, ChargesTypeRepository theChargesTypeRepository, List<ChargesTypePM> tenantZeroChargesTypes, List<Measurement> currentTenantMeasurement, List<VatType> tenantZeroVatTypes, List<VatType> currentTenantVatTypes)
        {
            foreach (ChargesTypePM a in tenantZeroChargesTypes)
            {
                ChargesType charge = new ChargesType()
                {
                    Id = IdCounter.GetNumber("ChargesType", theTenant).ToString(),
                    AddedManually = false,
                    Tenant = theTenant,
                    MeasurementId = currentTenantMeasurement.Where(d => d.Code == a.MeasurementCode && d.Tenant == theTenant).FirstOrDefault().Id,
                    Code = a.Code,
                    InActive = false,
                    LocalName = a.LocalName,
                    ContainerMeasurementId = currentTenantMeasurement.Where(d => d.Code == a.ContainerMeasurementCode && d.Tenant == theTenant).FirstOrDefault() != null ? currentTenantMeasurement.Where(d => d.Code == a.ContainerMeasurementCode && d.Tenant == theTenant).FirstOrDefault().Id : null,
                    IsAutoDisplayInQuote = a.IsAutoDisplayInQuote,
                    EnglishName = a.EnglishName,
                    AWBPrintDescription = a.AWBPrintDescription,
                    ChargesGroupCode = a.ChargesGroupCode,
                    ChargesGroupId = a.ChargesGroupId,
                    IATACodeId = a.IATACodeId,
                    Description = a.Description,
                    IsAir = a.IsAir,
                    IsOcean = a.IsOcean,
                    IsInland = a.IsInland,
                    IsAutoDisplayInConsolidation = a.IsAutoDisplayInConsolidation,
                    IsAutoDisplayInShipment = a.IsAutoDisplayInShipment,
                    IsPayable = a.IsPayable,
                    IsReceivable = a.IsReceivable,
                    DueTypeCode = a.DueTypeCode,
                    ViewOrder = a.ViewOrder,
                    SearchFields = a.SearchFields,
                    AccountingVATSplit = a.AccountingVATSplit,
                    ReceivableCreditAccount = a.ReceivableCreditAccount,
                    PayableDebitAccount = a.PayableDebitAccount,
                    IsAutoDisplayInCustoms = a.IsAutoDisplayInCustoms,
                    IsExport = a.IsExport,
                    IsImport = a.IsImport,
                    IsDomestic = a.IsDomestic,
                    IsDrop = a.IsDrop,
                };

                VatType vattype = tenantZeroVatTypes.Where(d => d.Id == a.VatTypeId).FirstOrDefault();
                if (vattype != null)
                {
                    VatType newVat = currentTenantVatTypes.Where(d => d.Code == vattype.Code && d.Tenant == theTenant).FirstOrDefault();
                    charge.VatTypeId = newVat.Id;
                }

                theChargesTypeRepository.Add(charge);
            }

            theChargesTypeRepository.SubmitChanges();
        }
        
        public static void AddChargesGroups(int theTenant, ChargesGroupRepository theChargesGroupRepository, List<ChargesGroupPM> tenantZeroChargesGroups)
        {
            foreach (ChargesGroupPM a in tenantZeroChargesGroups)
            {
                string localName = !string.IsNullOrEmpty(a.LocalName) ? a.LocalName : a.Name;
                ChargesGroup chargesGroup = new ChargesGroup()
                {
                    Id = IdCounter.GetNumber("ChargesGroup", theTenant).ToString(),
                    Tenant = theTenant,
                    Code = a.Code,
                    Name = a.Name,
                    LocalName = localName,
                    SearchFields = a.SearchFields,
                    ViewOrder = a.ViewOrder,

                };
                theChargesGroupRepository.Add(chargesGroup);

            }
            theChargesGroupRepository.SubmitChanges();
        }
        
        public static void AddRanks(int theTenant, RankRepository theRankRepository)
        {
            List<Rank> ranks = theRankRepository.GetRanks(0).ToList();
            foreach (Rank r in ranks)
            {
                Rank newRank = new Rank()
                {
                    Code = r.Code,
                    Id = IdCounter.GetNumber("Rank", theTenant).ToString(),
                    Name = r.Name,
                    Tenant = theTenant,
                    SearchFields = r.SearchFields,
                };
                theRankRepository.Add(newRank);
            }
            theRankRepository.SubmitChanges();
        }

        public static void AddPackageTypes(int theTenant, PackageTypeRepository thePackageTypeRepository, MeasurementRepository theMeasurementRepository, List<PackageTypePM> tenantZeroPackageTypes)
        {
            foreach (PackageTypePM packageType in tenantZeroPackageTypes)
            {
                PackageType newPackageType = new PackageType()
                {
                    Id = IdCounter.GetNumber("PackageType", theTenant).ToString(),
                    Tenant = theTenant,
                    Code = packageType.Code,
                    IsAir = packageType.IsAir,
                    IsInland = packageType.IsInland,
                    IsOcean = packageType.IsOcean,
                    IsContainer = packageType.IsContainer,
                    EnglishName = packageType.EnglishName,
                    LocalName = packageType.LocalName,
                    AddedManually = packageType.AddedManually,
                    Notes = packageType.Notes,
                    ContainerSize = packageType.ContainerSize,
                    TEU = packageType.TEU,
                    Volume = packageType.Volume,
                    PrintAs = packageType.PrintAs,
                    SearchFields = packageType.SearchFields,
                };

                if (packageType.IsContainer)
                {
                    Measurement newMeasurement = new Measurement();
                    newMeasurement.Id = IdCounter.GetNumber("Measurement", theTenant).ToString();
                    newPackageType.MeasurementId = newMeasurement.Id;
                    newMeasurement.Tenant = theTenant;
                    newMeasurement.IsContainer = true;
                    newMeasurement.Name = packageType.EnglishName;
                    newMeasurement.Code = packageType.Code;
                    newMeasurement.ShortName = packageType.EnglishName;
                    newMeasurement.SearchFields = packageType.SearchFields;
                    theMeasurementRepository.Add(newMeasurement);
                }
                thePackageTypeRepository.Add(newPackageType);
            }
            theMeasurementRepository.SubmitChanges();
            thePackageTypeRepository.SubmitChanges();
        }

        public static void AddLeadSources(int theTenant, LeadSourceRepository leadSourceRepository, List<LeadSource> tenantZeroLeadSources)
        {
            List<LeadSource> leadSourcesList = tenantZeroLeadSources.Where(d => d.Tenant == 0).ToList();
            foreach (LeadSource leadSource in leadSourcesList)
            {
                LeadSource newLeadSource = new LeadSource()
                {
                    Tenant = theTenant,
                    Name = leadSource.Name,
                    Code = leadSource.Code,
                    SearchFields = leadSource.SearchFields,
                    Id = IdCounter.GetNumber("LeadSource", theTenant).ToString(),
                };
                leadSourceRepository.Add(newLeadSource);
            }
            leadSourceRepository.SubmitChanges();
        }

        public static void AddStages(int theTenant, StageRepository stageRepository, List<Stage> tenantZeroStages)
        {
            List<Stage> stagesList = tenantZeroStages.Where(d => d.Tenant == 0).ToList();
            foreach (Stage stage in stagesList)
            {
                Stage newStage = new Stage()
                {
                    Tenant = theTenant,
                    Name = stage.Name,
                    Code = stage.Code,
                    Probability = stage.Probability,
                    SearchFields = stage.SearchFields,
                    IsSelectable = stage.IsSelectable,
                    Id = IdCounter.GetNumber("Stage", theTenant).ToString(),
                };
                stageRepository.Add(newStage);
            }
            stageRepository.SubmitChanges();
        }

        public static void AddIndustries(int theTenant, IndustryRepository industryRepository, List<IndustryPM> tenantZeroIndustries)
        {
            List<IndustryPM> industriesList = tenantZeroIndustries.Where(d => d.Tenant == 0).ToList();
            foreach (IndustryPM industry in industriesList)
            {
                Industry newIndustry = new Industry()
                {
                    Tenant = theTenant,
                    Name = industry.Name,
                    Code = industry.Code,
                    SearchFields = industry.SearchFields,
                    Id = IdCounter.GetNumber("Industry", theTenant).ToString(),
                };
                industryRepository.Add(newIndustry);
            }
            industryRepository.SubmitChanges();
        }

        public static void AddAdditionalServices(int theTenant, AdditionalServiceRepository additionalServiceRepository, List<AdditionalService> tenantZeroAdditionalServices)
        {
            List<AdditionalService> additionalServicesList = tenantZeroAdditionalServices.Where(d => d.Tenant == 0).ToList();
            foreach (AdditionalService additionalService in additionalServicesList)
            {
                AdditionalService newAdditionalService = new AdditionalService()
                {
                    Tenant = theTenant,
                    Name = additionalService.Name,
                    SearchFields = additionalService.SearchFields,
                    InActive = additionalService.InActive,
                    Id = IdCounter.GetNumber("AdditionalService", theTenant).ToString(),
                };
                additionalServiceRepository.Add(newAdditionalService);
            }
            additionalServiceRepository.SubmitChanges();
        }

        public static void AddClosingReasons(int theTenant, OpportunityClosingReasonRepository closingReasonRepository, List<OpportunityClosingReason> tenantZeroClosingReasons)
        {
            List<OpportunityClosingReason> closingReasonsList = tenantZeroClosingReasons.Where(d => d.Tenant == 0).ToList();
            foreach (OpportunityClosingReason closingReason in closingReasonsList)
            {
                OpportunityClosingReason newClosingReason = new OpportunityClosingReason()
                {
                    Tenant = theTenant,
                    Name = closingReason.Name,
                    Code = closingReason.Code,
                    LocalName = closingReason.LocalName,
                    SearchFields = closingReason.SearchFields,
                    IsClosedLost = closingReason.IsClosedLost,
                    AddedManually = closingReason.AddedManually,
                    Id = IdCounter.GetNumber("OpportunityClosingReason", theTenant).ToString(),
                };
                closingReasonRepository.Add(newClosingReason);
            }
            closingReasonRepository.SubmitChanges();
        }

        public static void AddQuoteStages(int myTenant, QuoteStageRepository entityRepository, List<QuoteStage> tenantZeroCollection)
        {
            List<QuoteStage> entityCollection = tenantZeroCollection.Where(d => d.Tenant == 0).ToList();

            foreach (QuoteStage item in entityCollection)
            {
                QuoteStage newItem = new QuoteStage()
                {
                    Id = IdCounter.GetNumber("QuoteStage", myTenant).ToString(),
                    Tenant = myTenant,
                    Name = item.Name,
                    Code = item.Code,
                    MaxDays = item.MaxDays,
                    SearchFields = item.SearchFields,
                    Rank = item.Rank,
                };

                entityRepository.Add(newItem);
            }

            entityRepository.SubmitChanges();
        }

        public static void AddOpportunityTypes(int theTenant, OpportunityTypeRepository TypeRepository, List<OpportunityType> tenantZeroTypes)
        {
            List<OpportunityType> TypesList = tenantZeroTypes.Where(d => d.Tenant == 0).ToList();
            foreach (OpportunityType Type in TypesList)
            {
                OpportunityType newType = new OpportunityType()
                {
                    Tenant = theTenant,
                    Name = Type.Name,
                    Code = Type.Code,
                    SearchFields = Type.SearchFields,
                    InActive = Type.InActive,
                    Id = IdCounter.GetNumber("OpportunityType", theTenant).ToString(),
                };
                TypeRepository.Add(newType);
            }
            TypeRepository.SubmitChanges();
        }

        public static void AddDocumentsMetaDataTypes(int theTenant, DocumentsMetaDataTypeRepository TypeRepository, List<DocumentsMetaDataType> tenantZeroTypes)
        {
            List<DocumentsMetaDataType> TypesList = tenantZeroTypes.Where(d => d.Tenant == 0).ToList();
            foreach (DocumentsMetaDataType Type in TypesList)
            {
                DocumentsMetaDataType newType = new DocumentsMetaDataType()
                {
                    Tenant = theTenant,
                    Code = Type.Code,
                    InActive = Type.InActive,
                    Id = IdCounter.GetNumber("DocumentsMetaDataType", theTenant).ToString(),
                    CustomsMetaDataCode = Type.CustomsMetaDataCode,
                    EnglishName = Type.EnglishName,
                    Format = Type.Format,
                    LocalName = Type.LocalName
                };
                TypeRepository.Add(newType);
            }
            TypeRepository.SubmitChanges();
        }

        public static void AddPaymentMethods(int theTenant, Simplog.Data.InvoiceModel.Repositories.AccountingPaymentMethodRepository TypeRepository, List<Simplog.Data.InvoiceModel.EntityPOCOs.AccountingPaymentMethod> tenantZeroTypes)
        {
            List<Simplog.Data.InvoiceModel.EntityPOCOs.AccountingPaymentMethod> TypesList = tenantZeroTypes.Where(d => d.Tenant == 0).ToList();
            foreach (Simplog.Data.InvoiceModel.EntityPOCOs.AccountingPaymentMethod Type in TypesList)
            {
                Simplog.Data.InvoiceModel.EntityPOCOs.AccountingPaymentMethod newType = new Simplog.Data.InvoiceModel.EntityPOCOs.AccountingPaymentMethod()
                {
                    Tenant = theTenant,
                    Name = Type.Name,
                    Code = Type.Code,
                    SearchFields = Type.SearchFields,
                    Inactive = Type.Inactive,
                    AddedManually = Type.AddedManually,
                    IsAP = Type.IsAP,
                    IsAR= Type.IsAR,                
                    Id = IdCounter.GetNumber("AccountingPaymentMethod", theTenant).ToString(),
                };
                TypeRepository.Add(newType);
            }
            TypeRepository.SubmitChanges();
        }
              
        public static void SetUserData(int theTenant)
        {
            GlobalZoneRepository globalZoneRepository;
            CountryRepository countryRepository;
            StateRepository stateRepository;
            CurrencyRepository currencyRep;
            CurrencyQuery currencyQuery;
            IncotermRepository incotermsRepository;
            List<GlobalZone> zones;
            List<Country> countries;
            List<State> states;
            List<CurrencyPM> currencies;
            List<Incoterm> incoterms;

            using (TransactionScope scope = TransactionFactory.GetNewTransaction(new TimeSpan(2, 0, 0)))//new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(2, 0, 0)))
            {
                globalZoneRepository = new GlobalZoneRepository(0);
                countryRepository = new CountryRepository(0);
                stateRepository = new StateRepository(0);
                currencyRep = new CurrencyRepository(0);
                currencyQuery = new CurrencyQuery(currencyRep);
                incotermsRepository = new IncotermRepository(0);
                zones = globalZoneRepository.GetGlobalZones(0).ToList<GlobalZone>();
                countries = countryRepository.GetCountries(0).ToList<Country>();
                states = stateRepository.GetStates(0).ToList();
                currencies = currencyQuery.GetCurrenciesByTenantPM(0).Where(d => d.Code == "USD" || d.Code == "EUR").ToList();
                incoterms = incotermsRepository.GetIncoterms(0).ToList<Incoterm>();
                scope.Complete();
            }

            globalZoneRepository = new GlobalZoneRepository(theTenant);
            countryRepository = new CountryRepository(theTenant);
            stateRepository = new StateRepository(theTenant);
            currencyRep = new CurrencyRepository(theTenant);
            incotermsRepository = new IncotermRepository(theTenant);

            foreach (GlobalZone z in zones)
            {
                GlobalZone zone = new GlobalZone()
                {
                    Code = z.Code,
                    EnglishName = z.EnglishName,
                    LocalName = z.LocalName,
                    Tenant = theTenant,
                    Id = IdCounter.GetNumber("GlobalZone", theTenant).ToString(),
                    InActive = z.Code == "--" ? z.InActive : false,
                    SearchFields = z.SearchFields,
                };

                globalZoneRepository.Add(zone);
            }

            globalZoneRepository.SubmitChanges();

            Dictionary<string, GlobalZone> globalzones = globalZoneRepository.GetGlobalZones(theTenant).ToDictionary(d => d.Code, t => t);

            foreach (Country c in countries)
            {
                string globalZoneId;
                string globalZoneCode = "";

                using (TransactionScope scope = TransactionFactory.GetNewTransaction(new TimeSpan(2, 0, 0)))//new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(2, 0, 0)))
                {
                    globalZoneCode = zones.Where(d => d.Id == c.GlobalZoneId).FirstOrDefault().Code;//c.GlobalZone.Code;
                    scope.Complete();
                }

                globalZoneId = globalzones[globalZoneCode].Id;//globalZoneRepository.GetGlobalZones(theTenant).Where(d => d.Code == globalZoneCode).FirstOrDefault().Id;
                Country country = new Country()
                {
                    Code = c.Code,
                    EnglishName = c.EnglishName,
                    LocalName = c.LocalName,
                    Tenant = theTenant,
                    AddedManually = c.Code == "--" ? c.AddedManually : false,
                    InActive = c.Code == "--" ? c.InActive : false,
                    Id = IdCounter.GetNumber("Country", theTenant).ToString(),
                    GlobalZoneId = globalZoneId,//globalZoneRepository.GetGlobalZones(tenant).Where(d => d.Code == c.GlobalZone.Code).FirstOrDefault().Id,//referenceService.GetGlobalZonesByTenant(tenant).Where(g => g.Code == c.GlobalZone.Code).FirstOrDefault().Id,
                    EC = c.EC,
                    Notes = c.Notes,
                    SearchFields = c.SearchFields,
                    HasStates = c.HasStates,
                    IsStateRequired = c.IsStateRequired,
                    IsNorthAmerica = c.IsNorthAmerica,
                };
                countryRepository.Add(country);
            }
            countryRepository.SubmitChanges();

            Dictionary<string, Country> countriesDictionary = countryRepository.GetCountries(theTenant).ToDictionary(d => d.Code, t => t);
            foreach (State s in states)
            {
                string countryId;
                string countryCode = "";
                using (TransactionScope scope = TransactionFactory.GetNewTransaction(new TimeSpan(2, 0, 0)))//new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(2, 0, 0)))
                {
                    countryCode = countries.Where(d => d.Id == s.CountryId).FirstOrDefault().Code;//s.Country.Code;
                    scope.Complete();
                }
                countryId = countriesDictionary[countryCode].Id;//countryRepository.GetCountries(theTenant).Where(d => d.Code == countryCode).FirstOrDefault().Id;
                State state = new State()
                {
                    Code = s.Code,
                    EnglishName = s.EnglishName,
                    LocalName = s.LocalName,
                    Tenant = theTenant,
                    AddedManually = false,
                    InActive = false,
                    Id = IdCounter.GetNumber("State", theTenant).ToString(),
                    CountryId = countryId,//countryRepository.GetCountries(tenant).Where(d => d.Code == s.Country.Code).FirstOrDefault().Id,//referenceService.GetCountriesByTenant(tenant).Where(g => g.Code == s.Country.Code).FirstOrDefault().Id,
                    SearchFields = s.SearchFields,

               
                };

                stateRepository.Add(state);
            }

            stateRepository.SubmitChanges();

            PortRepository portRepository = new PortRepository(0);
            Port unsignedPort = portRepository.GetUnsignedPort(0);
            if (unsignedPort != null)
            {
                Port newPort = new Port()
                {
                    AddedManually = false,
                    Code = unsignedPort.Code,
                    EnglishName = unsignedPort.EnglishName,
                    Field1 = unsignedPort.Field1,
                    Field10 = unsignedPort.Field10,
                    Field2 = unsignedPort.Field2,
                    Field3 = unsignedPort.Field3,
                    Field4 = unsignedPort.Field4,
                    Field5 = unsignedPort.Field5,
                    Field6 = unsignedPort.Field6,
                    Field7 = unsignedPort.Field7,
                    Field8 = unsignedPort.Field8,
                    Field9 = unsignedPort.Field9,
                    Tenant = theTenant,
                    InActive = unsignedPort.InActive,
                    IsAir = unsignedPort.IsAir,
                    IsInland = unsignedPort.IsInland,
                    IsOcean = unsignedPort.IsOcean,
                    Latitude = unsignedPort.Latitude,
                    LocalName = unsignedPort.LocalName,
                    Longtitude = unsignedPort.Longtitude,
                    SearchFields = unsignedPort.SearchFields,
                    Id = IdCounter.GetNumber("Port", theTenant).ToString(),
                    StateId = null,
                    Notes = null,
                    CountryId = ""
                };

                Country unsignedCountry = countryRepository.GetUnsignedCountry(theTenant);
                if (unsignedCountry != null)
                {
                    newPort.CountryId = unsignedCountry.Id;
                }

                portRepository.Add(newPort);
                portRepository.SubmitChanges();
            }

            #region Carriers

            #endregion

            #region currencies

            //foreach (CurrencyPM currency in currencies)
            //{
            //    Currency newCurrency = new Currency()
            //    {
            //        Code = currency.Code,
            //        EnglishName = currency.EnglishName,
            //        Id = IdCounter.GetNumber("Currency", theTenant).ToString(),
            //        InActive = currency.InActive,
            //        LocalName = currency.LocalName,
            //        Notes = currency.Notes,
            //        Tenant = theTenant,
            //        SearchFields = currency.SearchFields,
            //    };
            //    currencyRep.Add(newCurrency);
            //}
            //currencyRep.SubmitChanges();

            #endregion

            foreach (Incoterm i in incoterms)
            {
                Incoterm incoterm = new Incoterm()
                {
                    Id = IdCounter.GetNumber("Incoterm", theTenant).ToString(),
                    Name = i.Name,
                    LocalName = i.LocalName,
                    Tenant = theTenant,
                    Code = i.Code,
                    Freight = i.Freight,
                    OtherCharges = i.OtherCharges,
                    AddedManually = i.AddedManually,
                    InActive = i.InActive,
                    Notes = i.Notes,
                    SearchFields = i.SearchFields,
                };

                incotermsRepository.Add(incoterm);
            }

            incotermsRepository.SubmitChanges();
        }

        public static void AddReportFromTenantZero(int theTenant)
        {
            ReportHelper reportHelper = new ReportHelper();
            reportHelper.UpdateReports(theTenant);
        }

        public static void AddGeneralBIReportFolder(int tenant)
        {
            User systemUser = GetTenantSystemUser(tenant);
            BIReportFolderRepository bIReportFolderRepository = new BIReportFolderRepository(tenant);
            BIReportFolder bIReportFolder = new BIReportFolder()
            {
                Id = IdCounter.GetNumber("BIReportFolder", tenant),
                Tenant = tenant,
                CreateDate = DateTime.Now,
                CreatedByUserId = systemUser?.Id,
                UpdateDate = DateTime.Now,
                UpdatedByUserId = systemUser?.Id,
                SearchFields = "General",
                Name = "General",
                Description = null,
                Index = 0,
                PermissionForAll = true,
                PermittedByUserId = null,
            };

            bIReportFolderRepository.Add(bIReportFolder);
            bIReportFolderRepository.SubmitChanges();
        }

        private static User GetTenantSystemUser(int tenant)
        {
            string systemUserEmail = "system@tenant" + tenant + ".com";
            UserRepository userRepository = new UserRepository(tenant);
            User systemUser = userRepository.GetSingleUserByEmail(systemUserEmail, tenant, false);
            return systemUser;
        }

        public static void AddTenantLoginPolicy(int theTenant)
        {
            TenantLoginPolicyRepository tenantLoginPolicyRepository = new TenantLoginPolicyRepository(theTenant);
            TenantLoginPolicy tenantLoginPolicy = new TenantLoginPolicy()
            {
                Tenant = theTenant,
                LoginPolicyCode = "NOREST",
                SessionTimeout = 8
            };

            tenantLoginPolicyRepository.Add(tenantLoginPolicy);
            tenantLoginPolicyRepository.SubmitChanges();
        }

        public static void AddQuoteTemplate(int theTenant)
        {
            QuoteTemplateHelper quoteTemplateHelper = new QuoteTemplateHelper();
            QuoteTemplateCopyDetails quoteTemplateCopyDetails = new QuoteTemplateCopyDetails
            {
                Tenant = theTenant
            };
            quoteTemplateHelper.CopyQuoteTemplateFromTenantZero(quoteTemplateCopyDetails);
        }

        #region Tickets 
        public static void AddTicketTypes(int theTenant, TicketTypeRepository TypeRepository, List<TicketType> tenantZeroTypes)
        {
            List<TicketType> TypesList = tenantZeroTypes.Where(d => d.Tenant == 0).ToList();
            foreach (TicketType type in TypesList)
            {
                TicketType newType = new TicketType()
                {
                    Id = IdCounter.GetNumber("TicketType", theTenant).ToString(),
                    Tenant = theTenant,
                    Name = type.Name,
                    Code = type.Code,
                    SearchFields = type.SearchFields,
                    Inactive = type.Inactive,

                };
                TypeRepository.Add(newType);
            }
            TypeRepository.SubmitChanges();
        }

        public static void AddTicketStages(int theTenant, TicketStageRepository TypeRepository, List<TicketStage> tenantZeroTypes)
        {
            List<TicketStage> StagesList = tenantZeroTypes.Where(d => d.Tenant == 0).ToList();
            foreach (TicketStage stage in StagesList)
            {
                TicketStage newStage = new TicketStage()
                {
                    Id = IdCounter.GetNumber("TicketStage", theTenant).ToString(),
                    Tenant = theTenant,
                    Name = stage.Name,
                    Code = stage.Code,
                    SearchFields = stage.SearchFields,
                    Inactive = stage.Inactive,

                };
                TypeRepository.Add(newStage);
            }
            TypeRepository.SubmitChanges();
        }

        public static void AddTicketSeverities(int theTenant, TicketSeverityRepository TypeRepository, List<TicketSeverity> tenantZeroTypes)
        {
            List<TicketSeverity> SeveritiesList = tenantZeroTypes.Where(d => d.Tenant == 0).ToList();
            foreach (TicketSeverity severity in SeveritiesList)
            {
                TicketSeverity newSeverity = new TicketSeverity()
                {
                    Id = IdCounter.GetNumber("TicketSeverity", theTenant).ToString(),
                    Tenant = theTenant,
                    Name = severity.Name,
                    Code = severity.Code,
                    Severity = severity.Severity,
                    SearchFields = severity.SearchFields,
                    Inactive = severity.Inactive,

                };
                TypeRepository.Add(newSeverity);
            }
            TypeRepository.SubmitChanges();
        }
        #endregion 

        #region SLA 
        public static void AddBusinessHours(int theTenant, BusinessHourRepository TypeRepository, List<BusinessHour> tenantZeroTypes)
        {
            List<BusinessHour> BusinessHoursList = tenantZeroTypes.Where(d => d.Tenant == 0).ToList();
            foreach (BusinessHour item in BusinessHoursList)
            {
                BusinessHour newBusinessHour = new BusinessHour()
                {
                    Id = IdCounter.GetNumber("BusinessHour", theTenant).ToString(),
                    Tenant = theTenant,
                    Name = item.Name,
                    Description = item.Description,

                    Is247 = item.Is247,

                    CreatedByUserId = item.CreatedByUserId,

                    UpdatedByUserId = item.UpdatedByUserId,

                    CreateDate = item.CreateDate,
                    UpdateDate = item.UpdateDate,

                    IsMondayEnabeled = item.IsMondayEnabeled,
                    IsTuesdayEnabeled = item.IsTuesdayEnabeled,
                    IsWednesdayEnabeled = item.IsWednesdayEnabeled,
                    IsThursdayEnabeled = item.IsThursdayEnabeled,
                    IsFridayEnabeled = item.IsFridayEnabeled,
                    IsSaturdayEnabeled = item.IsSaturdayEnabeled,
                    IsSundayEnabeled = item.IsSundayEnabeled,

                    MondayFromHour = item.MondayFromHour,
                    TuesdayFromHour = item.TuesdayFromHour,
                    WednesdayFromHour = item.WednesdayFromHour,
                    ThursdayFromHour = item.ThursdayFromHour,
                    FridayFromHour = item.FridayFromHour,
                    SaturdayFromHour = item.SaturdayFromHour,
                    SundayFromHour = item.SundayFromHour,

                    MondayToHour = item.MondayToHour,
                    TuesdayToHour = item.TuesdayToHour,
                    WednesdayToHour = item.WednesdayToHour,
                    ThursdayToHour = item.ThursdayToHour,
                    FridayToHour = item.FridayToHour,
                    SaturdayToHour = item.SaturdayToHour,
                    SundayToHour = item.SundayToHour,
                    Code = item.Code,
                };

                TypeRepository.Add(newBusinessHour);
            }

            TypeRepository.SubmitChanges();
        }

        public static void AddSLAHeaders(int theTenant, SLAHeaderRepository repository, List<SLAHeader> tenantZeroTypes)
        {
            List<SLAHeader> SLAHeadersList = tenantZeroTypes.Where(d => d.Tenant == 0).ToList();
            TenantRepository tenantRepository;
            Tenant tenantPoco;
            foreach (SLAHeader item in SLAHeadersList)
            {
                SLAHeader newSLAHeader = new SLAHeader()
                {
                    Id = IdCounter.GetNumber("SLAHeader", theTenant).ToString(),
                    Tenant = theTenant,
                    CreateDate = item.CreateDate,
                    CreatedByUserId = item.CreatedByUserId,
                    UpdateDate = item.UpdateDate,
                    UpdatedByUserId = item.UpdatedByUserId,
                    Name = item.Name,
                    Description = item.Description,
                };

                repository.Add(newSLAHeader);
                repository.SubmitChanges();

                tenantRepository = new TenantRepository(theTenant);
                tenantPoco = tenantRepository.GetSingleByTenant(theTenant);
                if (tenantPoco != null)
                {
                    tenantPoco.DefaultSLAId = newSLAHeader.Id;
                    tenantRepository.Update(tenantPoco);
                    tenantRepository.SubmitChanges();
                }
            }
        }

        public static void AddSLALines(int theTenant, SLALineRepository TypeRepository, List<SLALine> tenantZeroTypes)
        {
            SLAHeaderRepository myRep = new SLAHeaderRepository(0);
            string SLAHeaderId = myRep.GetSingleHeaderByIdAndName(theTenant, "SLA").Id.ToString();

            BusinessHourRepository businessHourRepository = new BusinessHourRepository(0);
            string BusinessHoursId = businessHourRepository.GetBusinessHourByCode("BUS", theTenant).Id.ToString();

            TicketSeverityRepository ticketSeverityRepository = new TicketSeverityRepository(0);
            TicketSeverity[] ticketSeverites = ticketSeverityRepository.GetAll(theTenant).ToArray();

            // Low Severity 
            SLALine newSLALine = new SLALine()
            {
                Id = IdCounter.GetNumber("SLALine", theTenant).ToString(),
                Tenant = theTenant,
                SeverityId = ticketSeverites.Where(a => a.Code == "LW").FirstOrDefault().Id,
                SLAHeaderId = SLAHeaderId,
                BusinessHoursId = BusinessHoursId,
                FirstResponseTime = 6,
                FirstResponseTimeUnit = "OO",
                FirstResponseTimeInMinute = 360,
                ResolveWithinTime = 12,
                ResolveWithinTimeUnit = "OO",
                ResolveWithinTimeInMinute = 720,
                ResolveWithinEscalate = true,
                FirstResponseEscalate = true,
            };

            TypeRepository.Add(newSLALine);

            // Medium
            newSLALine = new SLALine()
            {
                Id = IdCounter.GetNumber("SLALine", theTenant).ToString(),
                Tenant = theTenant,
                SeverityId = ticketSeverites.Where(a => a.Code == "MD").FirstOrDefault().Id,
                SLAHeaderId = SLAHeaderId,
                BusinessHoursId = BusinessHoursId,
                FirstResponseTime = 6,
                FirstResponseTimeUnit = "OO",
                FirstResponseTimeInMinute = 180,
                ResolveWithinTime = 12,
                ResolveWithinTimeUnit = "OO",
                ResolveWithinTimeInMinute = 360,
                ResolveWithinEscalate = true,
                FirstResponseEscalate = true,
            };

            TypeRepository.Add(newSLALine);

            // High
            newSLALine = new SLALine()
            {
                Id = IdCounter.GetNumber("SLALine", theTenant).ToString(),
                Tenant = theTenant,
                SeverityId = ticketSeverites.Where(a => a.Code == "HI").FirstOrDefault().Id,
                SLAHeaderId = SLAHeaderId,
                BusinessHoursId = BusinessHoursId,
                FirstResponseTime = 2,
                FirstResponseTimeUnit = "OO",
                FirstResponseTimeInMinute = 120,
                ResolveWithinTime = 4,
                ResolveWithinTimeUnit = "OO",
                ResolveWithinTimeInMinute = 240,
                ResolveWithinEscalate = true,
                FirstResponseEscalate = true,
            };

            TypeRepository.Add(newSLALine);

            //Urgent
            newSLALine = new SLALine()
            {
                Id = IdCounter.GetNumber("SLALine", theTenant).ToString(),
                Tenant = theTenant,
                SeverityId = ticketSeverites.Where(a => a.Code == "UR").FirstOrDefault().Id,
                SLAHeaderId = SLAHeaderId,
                BusinessHoursId = BusinessHoursId,
                FirstResponseTime = 1,
                FirstResponseTimeUnit = "OO",
                FirstResponseTimeInMinute = 60,
                ResolveWithinTime = 2,
                ResolveWithinTimeUnit = "OO",
                ResolveWithinTimeInMinute = 120,
                ResolveWithinEscalate = true,
                FirstResponseEscalate = true,
            };

            TypeRepository.Add(newSLALine);
            TypeRepository.SubmitChanges();
        }
        #endregion 

        private static void AddDefaultFullAccountingSettings(int theTenant, FullAccountingSettingRepository theFullAccountingSettingsRepository)
        {
            FullAccountingSetting settings = new FullAccountingSetting()
            {
                Id = theTenant.ToString(),
                Tenant = theTenant,
            };

            theFullAccountingSettingsRepository.Add(settings);
            theFullAccountingSettingsRepository.SubmitChanges();
        }

        public static void AddWithholdingTaxDeductionTypes(int theTenant, WithholdingTaxDeductionTypeRepository withholdingTaxDeductionTypeRepository, List<WithholdingTaxDeductionType> tenantZeroTypes)
        {
            List<WithholdingTaxDeductionType> typesList = tenantZeroTypes.Where(d => d.Tenant == 0).ToList();
            foreach (WithholdingTaxDeductionType type in typesList)
            {
                WithholdingTaxDeductionType newtype = new WithholdingTaxDeductionType()
                {
                    Id = IdCounter.GetNumber("WithholdingTaxDeductionType", theTenant).ToString(),
                    Tenant = theTenant,
                    EnglishName = type.EnglishName,
                    Code = type.Code,
                    LocalName = type.LocalName,
                    SearchFields = type.SearchFields,
                    Inactive = type.Inactive,
                    
                };
                withholdingTaxDeductionTypeRepository.Add(newtype);
            }
            withholdingTaxDeductionTypeRepository.SubmitChanges();
        }
    }

    public class UserShortDetails
    {
        public int Tenant;
        public string Name;
        public string Email;
        public string PhoneNumber;
    }
}
