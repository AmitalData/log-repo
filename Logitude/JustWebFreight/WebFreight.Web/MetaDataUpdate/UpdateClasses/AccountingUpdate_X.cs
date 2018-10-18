using Logitude.Accounting.BL.EntityPMs;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
//using Logitude.AccountingMessaging.MessagingServices;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.QuoteModel;
using Simplog.Data.ShipmentsModel;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using WebFreight.Web.CommonDataModel.DomainServices;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using WebFreight.Web.MetaDataUpdate.AddClasses;
using WebFreight.Web.MetaDataUpdate.DetailClasses;
using Microsoft.Practices.Unity;
//using Logitude.Accounting.BL.Messaging.Accounting;
using Logitude.BL.InfrastructureModel.EntityQueries;

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate
{
    public partial class AccountingUpdate
    {
        public IWebFreightContext ObjectContext { get; set; }
        public ObjectTabelRepository ObjectTableRepository { get; set; }
        public ObjectFieldsRepository ObjectFieldsRepository { get; set; }
        public TextCodeRepository TextCodeRepository { get; set; }
        public TranslationRepository TranslationRepository { get; set; }
        public TranslationHeaderRepository TranslationHeaderRepository { get; set; }
        public DataTypeRepository DataTypeRepository { get; set; }
        public ScreensRepository ScreensRepository { get; set; }
        public ScreenFieldsRepository ScreenFieldsRepository { get; set; }
        public TextCodeTypesRepository TextCodeTypesRepository { get; set; }
        public FieldDataTypesRepository FieldDataTypesRepository { get; set; }
        public QueryRepository QueriesRepository { get; set; }
        public QueryColumnRepository QueryColumnsRepository { get; set; }
        public CustomTableRepository CustomTablesRepository { get; set; }
        public MenusTableRepository MenusTablesRepository { get; set; }
        public MenuTypeRepository MenuTypesRepository { get; set; }
        public CategoryTypeRepository CategoryTypesRepository { get; set; }
        public AdvancedQueryFilterRepository AdvancedQueryFiltersRepository { get; set; }
        public ObjectTableTabRepository ObjectTableTabsRepository { get; set; }
        public ObjectTableHelperControlRepository ObjectTableHelperControlsRepository { get; set; }
        public MenuButtonGroupRepository MenuButtonGroupRepository { get; set; }
        public MenuButtonRepository MenuButtonRepository { get; set; }
        public ObjectFieldValidationRepository ObjectFieldValidationRepository { get; set; }
        public TipRepository TipRepository { get; set; }
        public TenantSettingRepository TenantSettingRepository { get; set; }
        public QueryGroupRepository QueryGroupRepository { get; set; }
        public ObjectTableRuleRepository ObjectTableRuleRepository { get; set; }
        public ObjectTableRuleFieldRepository ObjectTableRuleFieldRepository { get; set; }
        public RuleConditionFieldRepository RuleConditionFieldRepository { get; set; }
        public CounterDefinitionRepository CounterDefinitionRepository;
        public CounterRepository CounterRepository { get; set; }


        private MenuButtonGroupQuery menuButtonGroupQuery;
        private ObjectFieldsQuery objectFieldsQuery;
        private ObjectTabelQuery objectTabelQuery;
        private QueryQuery queryQuery;

        #region update tenant zero
        public void LoadUpdateTenantZero(IWebFreightContext context)
        {
            isUpdate = true;
            LoadObjectsTenantZero(context);


        }
        #endregion

        #region objectTables

        //ObjectTable CustomsDeclarationObject;
      
        #endregion

        #region LoadObjectsTenantZero()
       // bool isUpdate = false;
        public void LoadObjectsTenantZero(IWebFreightContext context)
        {
            CommonDataDomainService commonDomain = new CommonDataDomainService();
            ObjectContext = context;
            TextCodeRepository = new TextCodeRepository(ObjectContext);
            ObjectTableRepository = new ObjectTabelRepository(ObjectContext);
            ObjectFieldsRepository = new ObjectFieldsRepository(ObjectContext);
            TipRepository = new TipRepository(ObjectContext);
            CounterDefinitionRepository = new CounterDefinitionRepository(ObjectContext);
            MenuButtonRepository = new MenuButtonRepository(ObjectContext);
            MenuButtonGroupRepository = new MenuButtonGroupRepository(ObjectContext);
            TenantSettingRepository = new TenantSettingRepository(ObjectContext);
            Dictionary<string, TextCode> textcodes = TextCodeRepository.GetTextCodesByTenant(0).ToDictionary(d => d.Code + d.Tenant.ToString() + d.ObjectTableId, a => a);
            Dictionary<string, ObjectTable> objectTables = ObjectTableRepository.GetObjectsByTenant(0).ToDictionary(d => d.Name, a => a);
            Dictionary<string, ObjectField> objectfields = ObjectFieldsRepository.GetObjectFieldsByTenant(0).ToDictionary(d => d.FieldName + d.ObjectTableId, a => a);
            Dictionary<string, Tip> tips = TipRepository.GetTips(0).ToDictionary(d => d.Code, a => a);

            CreateAllTablesTips(tips, textcodes);
    
           // LoadRolesAndFeatures(0);
            CreateMenuButtonsForTenant(0);
            //CreateTableCounters(0);
                          
            this.ObjectContext.SaveChanges();
        }

        void CreateAllTablesTips(Dictionary<string, Tip> tips, Dictionary<string, TextCode> textCodes)
        {

        }


        #endregion

        #region Create All Object Tables
        ObjectTable TestObjectTable;
        ObjectTable TestObjectTable2;
        
        //private void CreateAllObjectsTables(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes)
        //{
        //    #region Declaration
        //    CustomsDeclarationObject = AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
        //    {
        //        DefaultText = "Declaration",
        //        ObjectTableName = "Customs.Declaration",
        //        ObjectTablePlural = "Customs.Declarations",
        //        ObjectTableSingular = "Customs.Declaration",
        //        DBTableName = "Customs.Declarations",
        //        Tenant = 0,
        //        IsMain = true,
        //        ObjectTableTypeCode = "MD",
        //        IsNewWizard = true,
        //        NewWizardControlName = "Logitude.Customs.NewDeclarationControlCommand",
        //        LocalDefaultText = "הכרזה חדשה",

        //    }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
        //    #endregion

        //    this.ObjectContext.SaveChanges();
        //}
        #endregion

        #region create menubuttons
        public List<MenuButtonGroupPM> CreateMenuButtonsForTenant(int tenant)
        {
            menuButtonGroupQuery = new MenuButtonGroupQuery(MenuButtonGroupRepository);

            FeatureQuery featureQuery = new FeatureQuery(tenant);
            List<FeaturePM> features = featureQuery.GetFeaturePMsByTenant(tenant).ToList();

            Dictionary<string, TextCode> TextCodes = TextCodeRepository.GetTextCodesByTenant(tenant).Where(d => d.TextCodeTypeCode == "B").ToDictionary(s => s.Code, a => a);
            Dictionary<string, MenuButton> TenantMenuButtons = MenuButtonRepository.GetMenuButtonsByTenant(tenant).ToDictionary(d => d.EventCode + d.MenuButtonGroupId, a => a);
            Dictionary<string, MenuButtonGroup> TenantMenuButtonGroups = MenuButtonGroupRepository.GetMenuButtonGroupsByTenant(tenant).ToDictionary(d => d.Name, a => a);

            //string declarationTableId = ObjectContext.ObjectTables.Where(f => f.Name == "Customs.Declaration" && f.Tenant == tenant).FirstOrDefault().Id;
          
            //FeaturePM declarationFeature_SendDeclaration = features.Where(d => d.Code == "SENDDECLARATION" && d.ObjectTableId == declarationTableId).FirstOrDefault();
            //FeaturePM declarationFeature_DeclarationPayment = features.Where(d => d.Code == "DECLARATIONPAYMENT" && d.ObjectTableId == declarationTableId).FirstOrDefault();

            //string paymentOrderTableId = ObjectContext.ObjectTables.Where(f => f.Name == "Customs.PaymentOrder" && f.Tenant == tenant).FirstOrDefault().Id;

            //FeaturePM paymentOrderFeature_Sendpayment = features.Where(d => d.Code == "SENDPAYMENTORDER" && d.ObjectTableId == paymentOrderTableId).FirstOrDefault();
            //FeaturePM paymentOrderFeature_Closepayment = features.Where(d => d.Code == "CLOSEPAYMENTORDER" && d.ObjectTableId == paymentOrderTableId).FirstOrDefault();
            //FeaturePM paymentOrderFeature_Morepayment = features.Where(d => d.Code == "MOREPAYMENTORDER" && d.ObjectTableId == paymentOrderTableId).FirstOrDefault();
            //FeaturePM paymentOrderFeature_UnClosepayment = features.Where(d => d.Code == "UNCLOSEPAYMENTORDER" && d.ObjectTableId == paymentOrderTableId).FirstOrDefault();

            
            //string vendorTableId = ObjectContext.ObjectTables.Where(f => f.Name == "Customs.CustomsVendor" && f.Tenant == tenant).FirstOrDefault().Id;

            //FeaturePM vendorFeature_SaveVendor = features.Where(d => d.Code == "SAVEVENDOR" && d.ObjectTableId == vendorTableId).FirstOrDefault();

            #region Declaration Buttons
            //MenuButtonGroup declarationMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
            //{
            //    MenuButtonGroupType = "Customs.DeclarationEdit",
            //    Name = "Customs.DeclarationEditButtonsGroup",
            //    ObjectTableId = declarationTableId,
            //    Tenant = tenant,
            //}, MenuButtonGroupRepository, TenantMenuButtonGroups);

            #region actions button
            //MenuButton actionButton = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
            //{
            //    EventCode = "More",
            //    Index = 1,
            //    IsActive = false,
            //    LabelTextCodeCode = "Customs.Declaration.B.More",
            //    LabelTextCodeDefaultText = "More",
            //    LocalDefaultText = "נוספים",
            //    Tenant = tenant,
            //    MenuButtonGroupId = declarationMenuButtonGroup.Id,
            //    ObjectTableId = declarationTableId,
            //    MenuButtonType = "dropdownbutton",
            //}, MenuButtonRepository, TenantMenuButtons, TextCodeRepository, TextCodes);
            #endregion

  
 
            #endregion



            return menuButtonGroupQuery.GetMenuButtonGroupPMsByTenant(tenant);
        }
        #endregion

        #region load object fields functions

        //#region CreateCustomsDeclarationObjectFields()
        //private void CreateCustomsDeclarationObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes)
        //{
        //    ObjectTabelRepository objecttableRep = new ObjectTabelRepository(0);
        //    objectTabelQuery = new ObjectTabelQuery(objecttableRep);

        //    List<ObjectTablePM> objectTables = objectTabelQuery.GetObjectPMsByTenant(0).ToList();

        //    ObjectTablePM CardObject = objectTables.Where(d => d.Name == "Card").FirstOrDefault();

  

        //    AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails()
        //    {
        //        DefaultText = "Search codes/ names",
        //        DisplayOnLookUp = false,
        //        FullFieldLable = "SearchFields",
        //        FieldName = "SearchFields",
        //        FieldsDataType = "Text",
        //        IsRequired = false,
        //        MaxLength = 1000,
        //        MinLength = 0,
        //        ObjectTableId = CustomsSiteTypeObject.Id,
        //        ObjectTableName = "Customs.SiteType",
        //        ObjectTablePlural = "Customs.SiteTypes",
        //        ObjectTableSingular = "Customs.SiteType",
        //        Tenant = 0,
        //        TextCodeType = "F",
        //        DisplayOnly = false,
        //        SystemMaxLength = 40,
        //        SystemRequired = false,
        //        CanFilter = true,
        //        ValidForQuerySection1 = "Customs.SiteType",
        //        ValidForQuerySection2 = "Customs.SiteTypeFollowUp",
        //        DisplayInList = false,
        //        IsCustomFilter = false,
        //        Operator = "Contains",
        //        HelpTextDefaultText = "Searching by :\n1: code\n2: name",
        //        HelpTextCode = "SearchFields",
        //    }, TextCodeRepository, ObjectFieldsRepository, objectFields, textCodes);

        //    this.ObjectContext.SaveChanges();

        //}
        //#endregion






        #endregion

        #region load Queries
        public void loadQueries()
        {
            ObjectContext = WebFreightContext.GetContext(0);

            QueriesRepository = new QueryRepository(ObjectContext);
            QueryColumnsRepository = new QueryColumnRepository(ObjectContext);
            AdvancedQueryFiltersRepository = new AdvancedQueryFilterRepository(ObjectContext);
            QueryGroupRepository = new QueryGroupRepository(ObjectContext);

            Dictionary<string, Query> tenantQueries = QueriesRepository.GetQueriesByTenantSystemLevel(0).ToDictionary(d => d.Code + d.ObjectTableId, a => a);
            Dictionary<string, QueryColumn> tenantQueryColumns = QueryColumnsRepository.GetQueryColumnsByTenant(0).ToDictionary(d => d.QueryId + d.ObjectFieldId, a => a);

            Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters = AdvancedQueryFiltersRepository.GetAdvancedQueryFiltersByTenant(0).ToDictionary(d => d.QueryId + d.ObjectFieldId, a => a);

            FeatureRepository featureRepository = new FeatureRepository(0);
            List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();


            #region ObjectFieldsLists
            //List<ObjectField> objectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Shipment").ToList();
            //List<ObjectField> MasterObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Master").ToList();
            //List<ObjectField> PortsObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Port").ToList();
        
            

            #region closed tables
            //List<ObjectField> checkEntityTypeFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Customs.CheckEntityType").ToList();
            //List<ObjectField> checkRepresentativeTypeFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Customs.CheckRepresentativeType").ToList();
         

            #endregion
            #endregion

            #region ObjectTables

            //ObjectTable CustomsPhysicalCheckObject = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.PhysicalCheck" && d.Tenant == 0).FirstOrDefault();
            //ObjectTable CustomsDeclarationObject = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.Declaration" && d.Tenant == 0).FirstOrDefault();
                  
            #region closed Tables
            //ObjectTable checkEntityTypeObject = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.CheckEntityType" && d.Tenant == 0).FirstOrDefault();
            //ObjectTable checkRepresentativeTypeObject = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.CheckRepresentativeType" && d.Tenant == 0).FirstOrDefault();
 
            
            

            #endregion
      
            #endregion

            #region QueryGroups

            //QueryGroup CustomsPhysicalCheckGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "PHCK", Name = "Customs.PhysicalCheck" }, QueryGroupRepository);
            //QueryGroup CustomsDeclarationGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "DECL", Name = "Customs.Declaration" }, QueryGroupRepository);
                    
           
           
            #region closed tables
            //QueryGroup checkRepresentativeTypeGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "CHRT", Name = "Customs.CheckRepresentativeType" }, QueryGroupRepository);
            //QueryGroup tradeAgreementGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "TRAG", Name = "Customs.TradeAgreement" }, QueryGroupRepository);
            QueryGroupRepository.SubmitChanges();
 
            
            
            #endregion

            #endregion

            QueryGroupRepository.SubmitChanges();

            #region Features



            #region CustomsPhysicalCheck features
       //     Feature CustomsPhysicalCheckFeature = tenantFeatures.Where(d => d.Code == "BYUPCOMINGCHECK" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            #endregion

            #region closed tables

            //Feature CheckEntityTypeFeature = tenantFeatures.Where(d => d.Code == "CHECKENTITYTYPE" && d.FeatureTypeCode == "QUER").FirstOrDefault();
            //Feature CheckRepresentativeTypeFeature = tenantFeatures.Where(d => d.Code == "CHECKREPRESENTATIVETYPE" && d.FeatureTypeCode == "QUER").FirstOrDefault();
           
            
            
            

            #endregion

            #endregion

            #region CustomsPhysicalCheck queries
          //  Query ByUpcomingChecks = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = ObjectContext.TextCodes.Where(d => d.Code == "Customs.PhysicalCheck.Q.ByUpcomingCheck" && d.ObjectTableId == CustomsPhysicalCheckObject.Id ).FirstOrDefault().Id, Code = "By Upcoming Checks", QueryGroupCode = CustomsPhysicalCheckGroup.Code, IndexOrder = 0, Tenant = 0, ObjectTableId = CustomsPhysicalCheckObject.Id, QuerySection = "Customs.PhysicalCheck", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = CustomsPhysicalCheckFeature.Id, SpotlightDataTemplate = "CheckSpotLightDataTemplate" }, QueriesRepository, tenantQueries);
           // QueryColumn ByUpcomingChecks1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ByUpcomingChecks.Id, IndexOrder = 0, ObjectFieldId = CustomsPhysicalCheckFields.Where(d => d.FieldName == "DeclarationNo" && d.ObjectTableId == CustomsPhysicalCheckObject.Id).FirstOrDefault().Id, ColumnWidth = 73 }, QueryColumnsRepository, tenantQueryColumns);
          //  QueryColumn ByUpcomingChecks3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ByUpcomingChecks.Id, IndexOrder = 1, ObjectFieldId = CustomsPhysicalCheckFields.Where(d => d.FieldName == "CustomerName" && d.ObjectTableId == CustomsPhysicalCheckObject.Id).FirstOrDefault().Id, ColumnWidth = 56 }, QueryColumnsRepository, tenantQueryColumns);
      
            
            #endregion

      
            #region Customs Closed Tables

            #region BankQuery



            //Query bankQuery = AddQueries.AddQuery(new QueryDetails()
            //{
            //    NameTextCodeId = ObjectContext.TextCodes.Where(d => d.Code == "Customs.Bank.Q.BankQuery" && d.ObjectTableId == bankObjectTable.Id).FirstOrDefault().Id,
            //    Code = "Bank",
            //    QueryGroupCode = bankQueryGroup.Code,
            //    IndexOrder = 0,
            //    Tenant = 0,
            //    ObjectTableId = bankObjectTable.Id,
            //    QuerySection = "Banks",
            //    SystemLevel = true,
            //    IsAddNewEntityEnabled = false,
            //    FeatureId = bankQueryFeature.Id,
               
            //}, QueriesRepository, tenantQueries);
            //QueryColumn bankQueryColumn01 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = bankQuery.Id, IndexOrder = 0, ObjectFieldId = bankFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == bankObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 70 }, QueryColumnsRepository, tenantQueryColumns);
            //QueryColumn bankQueryColumn02 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = bankQuery.Id, IndexOrder = 1, ObjectFieldId = bankFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == bankObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, QueryColumnsRepository, tenantQueryColumns);
            //QueryColumn bankQueryColumn03 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = bankQuery.Id, IndexOrder = 2, ObjectFieldId = bankFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == bankObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, QueryColumnsRepository, tenantQueryColumns);

            #endregion



            #endregion



            ObjectContext.SaveChanges();
        }

        //private void BuildCloseTable(string tableName, string pluralName, string featureCode, string queryGroupCode, string textCodeCode)
        //{
        //    ObjectTable bankObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.Bank" && d.Tenant == 0).FirstOrDefault();

        //    QueryGroup bankQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "QGBN", Name = "Customs.Bank" }, QueryGroupRepository);

        //}



        #endregion
      
     
        #region load screens

        public void loadScreens()
        {
            ObjectContext = WebFreightContext.GetContext(0);
            ScreenFieldsRepository = new ScreenFieldsRepository(ObjectContext);
            ScreensRepository = new ScreensRepository(ObjectContext);
            Dictionary<string, Screen> tenantScreens = ScreensRepository.GetScreensByTenant(0).ToDictionary(d => d.Code + d.ObjectTableId, a => a);
            Dictionary<string, ScreenField> tenantScreenField = ScreenFieldsRepository.GetScreenFieldsByTenant(0).ToDictionary(d => d.ScreenId + d.ObjectFieldId);

      //      BuildCustomsPhysicalCheckScreens(tenantScreens, tenantScreenField);
            
        //    LoadObjectTableRulesANDFieldsValidations();
        }


        #region BuildCustomsPhysicalCheckScreens
        //private void BuildCustomsPhysicalCheckScreens(Dictionary<string, Screen> tenantScreens, Dictionary<string, ScreenField> tenantScreenFields)
        //{
        //    ObjectTable CustomsPhysicalCheckObject = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.PhysicalCheck" && d.Tenant == 0).FirstOrDefault();

        //    ObjectField DeclarationNoField = ObjectContext.ObjectFields.Where(d => d.FieldName == "DeclarationNo" && d.ObjectTableId == CustomsPhysicalCheckObject.Id && d.Tenant == 0).FirstOrDefault();
        //    ObjectField StorageSiteCodeField = ObjectContext.ObjectFields.Where(d => d.FieldName == "StorageSiteCode" && d.ObjectTableId == CustomsPhysicalCheckObject.Id && d.Tenant == 0).FirstOrDefault();
        //    ObjectField CheckSiteCodeField = ObjectContext.ObjectFields.Where(d => d.FieldName == "CheckSiteCode" && d.ObjectTableId == CustomsPhysicalCheckObject.Id && d.Tenant == 0).FirstOrDefault();
        //    ObjectField CheckIdField = ObjectContext.ObjectFields.Where(d => d.FieldName == "CheckId" && d.ObjectTableId == CustomsPhysicalCheckObject.Id && d.Tenant == 0).FirstOrDefault();
        //    ObjectField LimitDateField = ObjectContext.ObjectFields.Where(d => d.FieldName == "LimitDate" && d.ObjectTableId == CustomsPhysicalCheckObject.Id && d.Tenant == 0).FirstOrDefault();
        //    ObjectField ContainerNubmerField = ObjectContext.ObjectFields.Where(d => d.FieldName == "ContainerNubmer" && d.ObjectTableId == CustomsPhysicalCheckObject.Id && d.Tenant == 0).FirstOrDefault();
        //    ObjectField QueueTypeCodeField = ObjectContext.ObjectFields.Where(d => d.FieldName == "QueueTypeCode" && d.ObjectTableId == CustomsPhysicalCheckObject.Id && d.Tenant == 0).FirstOrDefault();
        //    ObjectField OperationCodeField = ObjectContext.ObjectFields.Where(d => d.FieldName == "OperationCode" && d.ObjectTableId == CustomsPhysicalCheckObject.Id && d.Tenant == 0).FirstOrDefault();
        //    ObjectField StatusMessageCodeField = ObjectContext.ObjectFields.Where(d => d.FieldName == "StatusMessageCode" && d.ObjectTableId == CustomsPhysicalCheckObject.Id && d.Tenant == 0).FirstOrDefault();

        //    ObjectField CargoIdentifierTypeNameField = ObjectContext.ObjectFields.Where(d => d.FieldName == "CargoIdentifierTypeName" && d.ObjectTableId == CustomsPhysicalCheckObject.Id && d.Tenant == 0).FirstOrDefault();
        //    ObjectField CargoIdentifierKey1Field = ObjectContext.ObjectFields.Where(d => d.FieldName == "CargoIdentifierKey1" && d.ObjectTableId == CustomsPhysicalCheckObject.Id && d.Tenant == 0).FirstOrDefault();
        //    ObjectField CargoIdentifierKey2Field = ObjectContext.ObjectFields.Where(d => d.FieldName == "CargoIdentifierKey2" && d.ObjectTableId == CustomsPhysicalCheckObject.Id && d.Tenant == 0).FirstOrDefault();
        //    ObjectField CargoIdentifierKey3Field = ObjectContext.ObjectFields.Where(d => d.FieldName == "CargoIdentifierKey3" && d.ObjectTableId == CustomsPhysicalCheckObject.Id && d.Tenant == 0).FirstOrDefault();
            


        //    #region Header Screen
        //    Screen HeaderScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Customs.PhysicalCheck.HeaderScreen", Name = "Header Screen", ObjectTableId = CustomsPhysicalCheckObject.Id, NumberOfColumns = 4, NumberOfRows = 1, IsReadOnly = true }, ScreensRepository, tenantScreens);

        //    ScreenField ScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = CargoIdentifierTypeNameField.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
        //    ScreenField ScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = CargoIdentifierKey1Field.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
        //    ScreenField ScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = CargoIdentifierKey2Field.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);
        //    ScreenField ScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ObjectFieldId = CargoIdentifierKey3Field.Id, ScreenId = HeaderScreen.Id, Tenant = 0 }, ScreenFieldsRepository, tenantScreenFields);

        //    CustomsPhysicalCheckObject.HeaderScreenId = HeaderScreen.Id;
        //    #endregion

        //    #region General Screen
        //    Screen generalTabScreen = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Customs.PhysicalCheck.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = CustomsPhysicalCheckObject.Id, NumberOfColumns = 2, NumberOfRows = 6, }, ScreensRepository, tenantScreens);

        //    //ScreenField GScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 0, Column = 0, ObjectFieldId = DeclarationNoField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
        //    ScreenField GScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 0, Column = 0, ObjectFieldId = StorageSiteCodeField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
        //    ScreenField GScreenField7 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 1, Column = 0, ObjectFieldId = CheckSiteCodeField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
        //    ScreenField GScreenField9 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 2, Column = 0, ObjectFieldId = CheckIdField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
        //    ScreenField GScreenField13 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 3, Column = 0, ObjectFieldId = OperationCodeField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
        //    ScreenField GScreenField14 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 4, Column = 0, ObjectFieldId = StatusMessageCodeField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
        //    ScreenField GScreenField10 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 0, Column = 1, ObjectFieldId = LimitDateField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
        //    ScreenField GScreenField11 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 1, Column = 1, ObjectFieldId = ContainerNubmerField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
        //    ScreenField GScreenField12 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Row = 2, Column = 1, ObjectFieldId = QueueTypeCodeField.Id, ScreenId = generalTabScreen.Id, Tenant = 0, }, ScreenFieldsRepository, tenantScreenFields);
        //    #endregion

        //    ObjectContext.SaveChanges();
        //}
        #endregion

      


        #endregion

        #region Load Rules
        public void LoadObjectTableRulesANDFieldsValidations()
        {
            ObjectContext = WebFreightContext.GetContext(0);
            ObjectTableRuleRepository = new ObjectTableRuleRepository(ObjectContext);
            ObjectTableRuleFieldRepository = new ObjectTableRuleFieldRepository(ObjectContext);
            ObjectFieldValidationRepository = new ObjectFieldValidationRepository(ObjectContext);
            RuleConditionFieldRepository = new RuleConditionFieldRepository(ObjectContext);
            Dictionary<string, ObjectTableRule> TenantObjectTableRule = ObjectTableRuleRepository.GetObjectTableRules(0).ToDictionary(d => d.RuleCode, a => a);
            Dictionary<string, ObjectTableRuleField> TenantObjectTableRuleFields = ObjectTableRuleFieldRepository.GetObjectTableRuleFields(0).ToDictionary(d => d.ObjectTableRuleId + d.ObjectFieldId, a => a);
            Dictionary<string, RuleConditionField> TenantRuleConditionFields = RuleConditionFieldRepository.GetRuleConditionFieldsByTenant(0).ToDictionary(d => d.ObjectTableRuleId + d.ObjectFieldId, a => a);
            List<ObjectFieldValidation> TenantObjectFieldValidations = ObjectFieldValidationRepository.GetObjectFieldValidations(0).ToList();

            CreateClientRules(TenantObjectTableRule, TenantObjectTableRuleFields, TenantObjectFieldValidations);
        
        }

        #region ClientRules
        private void CreateClientRules(Dictionary<string, ObjectTableRule> TenantObjectTableRule, Dictionary<string, ObjectTableRuleField> TenantObjectTableRuleFields, List<ObjectFieldValidation> TenantObjectFieldValidations)
        {
          
            ObjectTable ClientTable = ObjectContext.ObjectTables.Where(f => f.Name == "Accounting.Client" && f.Tenant == 0).FirstOrDefault();
            ObjectField ClientCode = ObjectContext.ObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == ClientTable.Id).FirstOrDefault();

            //ObjectTableRule ClientDuplicationRule = AddObjectTableRules.AddObjectTableRule(new ObjectTableRuleDetails()
            //{
            //    RuleCode = "CLND",
            //    Name = "Code Duplication",
            //    ObjectTableId = ClientTable.Id,
            //    Tenant = 0,
            //    RuleTypeCode = "DUPL",
            //    SystemLevel = true,
            //    OutputMessage = "This Client already exists",
            //    ActiveForNew = true,
            //    ActiveForUpdate = false,
            //    TriggerTypeCode = "ALLW",
            //    RuleNotificationTypeCode = "ERR",
            //}, ObjectTableRuleRepository, TenantObjectTableRule);

            //ObjectTableRuleField ClientCodeField = AddObjectTableRules.AddObjectTableRuleField(new ObjectTableRuleFieldDetails() { ObjectFieldId = ClientCode.Id, ObjectTableRuleId = ClientDuplicationRule.Id, SystemLevel = true, Tenant = 0 }, ObjectTableRuleFieldRepository, TenantObjectTableRuleFields);

           
            ObjectContext.SaveChanges();
           
        }

            #endregion

  
        
        private void CreateObjectFieldValidations(List<ObjectFieldValidation> TenantObjectFieldValidations)
        {

        }
        #endregion

        #region Load Error messages

        public void LoadErrorMessages()
        {


        }
        #endregion

        #region load objecttable tab
        public void LoadObjectTableTabs()
        {
            ObjectContext = WebFreightContext.GetContext(0);
            ObjectTableTabsRepository = new ObjectTableTabRepository(ObjectContext);

            FeatureRepository featureRepository = new FeatureRepository(0);
            List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();

            #region ObjectTables

           // ObjectTable CustomsPhysicalCheckTable = ObjectContext.ObjectTables.Where(f => f.Name == "Customs.PhysicalCheck" && f.Tenant == 0).FirstOrDefault();
         
            #endregion

            Dictionary<string, ObjectTableTab> TenantObjectTableTabs = ObjectTableTabsRepository.GetObjectTableTabsByTenant(0).ToDictionary(d => d.Code, a => a);

            #region CustomsPhysicalCheck
          //  Feature CustomsphysicalCheckGENERALFeature = tenantFeatures.Where(d => d.Code == "GENERAL" && d.ObjectTableId == CustomsPhysicalCheckTable.Id).FirstOrDefault();
           // Feature CustomsphysicalCheckEVENTSFeature = tenantFeatures.Where(d => d.Code == "EVENTS" && d.ObjectTableId == CustomsPhysicalCheckTable.Id).FirstOrDefault();
    
            //tabs
          //  AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { FeatureId = CustomsphysicalCheckGENERALFeature.Id, ControlPath = "Simplog.Infrastructure.GeneralControls.GeneralTabControl", ObjectTableId = CustomsPhysicalCheckTable.Id, TabNameTextCodeId = ObjectContext.TextCodes.Where(d => d.Code == "Customs.PhysicalCheck.TH.General" && d.Tenant == 0).FirstOrDefault().Id, Code = "PHGC", Tenant = 0, IndexOrder = 0 }, ObjectTableTabsRepository, TenantObjectTableTabs);
           // AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { FeatureId = CustomsphysicalCheckEVENTSFeature.Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = CustomsPhysicalCheckTable.Id, TabNameTextCodeId = ObjectContext.TextCodes.Where(d => d.Code == "Customs.PhysicalCheck.TH.Events" && d.Tenant == 0).FirstOrDefault().Id, Code = "PHEV", Tenant = 0, IndexOrder = 1 }, ObjectTableTabsRepository, TenantObjectTableTabs);
      
          
            #endregion



            ObjectContext.SaveChanges();

        }
        #endregion

        #region load helper controls
        public void LoadObjectTableHelperControls()
        {
            ObjectContext = WebFreightContext.GetContext(0);
            ObjectTableHelperControlsRepository = new ObjectTableHelperControlRepository(ObjectContext);






        }
        #endregion

       
        #region LoadTextCodes

        #region load other fields textCodes
        public void LoadOtherFields(IWebFreightContext context)
        {
            ObjectContext = context;//WebFreightContext.GetContext(0);
            TextCodeRepository = new TextCodeRepository(ObjectContext);

            Dictionary<string, TextCode> textcodes = TextCodeRepository.GetTextCodesByTenant(0).ToDictionary(d => d.Code + d.Tenant.ToString() + d.ObjectTableId, a => a);
            LoadTextCodes_General(textcodes);

         //   LoadTextCodes_CustomsPhysicalCheck(textcodes);
         //   LoadTextCodes_CustomsVendors(textcodes);
           

            #region ObjectTable

         //   ObjectTable CustomsPyhysicalCheckTable = ObjectContext.ObjectTables.Where(f => f.Name == "Customs.PhysicalCheck" && f.Tenant == 0).FirstOrDefault();
         //   ObjectTable CustomsDeclarationTable = ObjectContext.ObjectTables.Where(f => f.Name == "Customs.Declaration" && f.Tenant == 0).FirstOrDefault();
        
            #region closed Tables
         //   ObjectTable checkEntityTypeObject = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.CheckEntityType" && d.Tenant == 0).FirstOrDefault();
         //   ObjectTable checkRepresentativeTypeObject = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.CheckRepresentativeType" && d.Tenant == 0).FirstOrDefault();
    
            
            #endregion

            #endregion


            #region TableDescription


            #endregion

            #region Tabs Headers

            #region CustomsPhysicalCheck
       //     AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.TH.General", DefaultText = "General", LocalDefaultText = "כללי", ObjectTableId = CustomsPyhysicalCheckTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textcodes);
        //    AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.TH.Events", DefaultText = "Events", LocalDefaultText = "אירועים", ObjectTableId = CustomsPyhysicalCheckTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textcodes);
        
         
            #endregion

    
            #endregion

            #region QueryTextCodes


            #region CustomsPhysicalCheck Queries
       //     AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.Q.ByUpcomingCheck", DefaultText = "By Upcoming Check", LocalDefaultText = "בדיקות פיזיות", ObjectTableId = CustomsPyhysicalCheckTable.Id, Tenant = 0, TextCodeTypeCode = "Q" }, TextCodeRepository, textcodes);

            #endregion

     

            #region closed Tables


         //   AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CheckEntityTypes.Q.CheckEntityTypesQuery", DefaultText = "Check Entity Types", LocalDefaultText = "בדקו סוגי ישות", ObjectTableId = checkEntityTypeObject.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textcodes);
         //   AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CheckRepresentativeType.Q.CheckRepresentativeTypeQuery", DefaultText = "Check Representative Types", LocalDefaultText = "בדקו סוג הנציג", ObjectTableId = checkRepresentativeTypeObject.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textcodes);
  
            
              
            #endregion



            #endregion

       //     AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PaymentOrder.Q.PaymentOrderQuery", DefaultText = "Payment Orders", LocalDefaultText = "כל הוראות התשלום", ObjectTableId = paymentOrderTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textcodes);
        //    AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PaymentOrder.TH.General", DefaultText = "General", LocalDefaultText = "כללי", ObjectTableId = paymentOrderTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textcodes);
            
            
            ObjectContext.SaveChanges();
               }
        #endregion

        #region LoadTextCodes_General()
        private void LoadTextCodes_General(Dictionary<string, TextCode> textcodes)
        {
            ObjectTable objectTable = ObjectContext.ObjectTables.Where(f => f.Name == "General" && f.Tenant == 0).FirstOrDefault();

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.SaveAndNew", DefaultText = "Save And New", LocalDefaultText = "שמירה וחדש", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, TextCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.Add", DefaultText = "Add", LocalDefaultText = "הוסף", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, TextCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.OK", DefaultText = "OK", LocalDefaultText = "אישור", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, TextCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.Cancel", DefaultText = "Cancel", LocalDefaultText = "ביטול", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, TextCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.Close", DefaultText = "Close", LocalDefaultText = "סגור", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, TextCodeRepository, textcodes);

            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.AddDocumentVersion", DefaultText = "Add Version", LocalDefaultText = "גרסה חדשה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, TextCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NewEntity", DefaultText = "New %Entity", LocalDefaultText = "%Entity חדש", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, TextCodeRepository, textcodes);
            //AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Others.CustomBank", DefaultText = "Custom Banks", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", }, TextCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.SendSample", DefaultText = "Send Sample", LocalDefaultText = "שלח לדוגמה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, TextCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Customs", DefaultText = "Customs", LocalDefaultText = "מכס", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, TextCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NewPaymentOrder", DefaultText = "New Payment Order", LocalDefaultText = "שליפת הוראת תשלום", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, TextCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Close", DefaultText = "Close", LocalDefaultText = "סגור", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, TextCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Loading", DefaultText = "Loading ....", LocalDefaultText = "טוען ....", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, TextCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Sending", DefaultText = "Sending ....", LocalDefaultText = "שולח ....", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, TextCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AddRemoveColumns", DefaultText = "Add/Remove columns", LocalDefaultText = "הוסף/מחק עמודות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, TextCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NoFiltersHaveBeenSet", DefaultText = "No filters have been set", LocalDefaultText = "לא הוגדרו חיתוכים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, TextCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ExportToExcel", DefaultText = "Export to excel ", LocalDefaultText = "Excel הורד לאקסל", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, TextCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Saving", DefaultText = "Saving ....", LocalDefaultText = "שמירה ....", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, TextCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.EditMetaData", DefaultText = "Edit Meta Data", LocalDefaultText = "עריכת מטה דאטה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, TextCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.EditCustomDocument", DefaultText = "Edit Custom Document", LocalDefaultText = "עריכת מסמך מכס", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, TextCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.IsRequired", DefaultText = "Is Required", LocalDefaultText = "הוא נדרש", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, TextCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.FieldForTableIsRequired", DefaultText = "%FieldName in %TableName %EntityReference is Required", LocalDefaultText = "%FieldName ב- %TableName %EntityReference הוא חובה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, TextCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ObjectTables", DefaultText = "Object Tables", LocalDefaultText = "שולחנות אובייקט", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, TextCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.RequiredFields", DefaultText = "Required Fields", LocalDefaultText = "שדות חובה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, TextCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.WrongEntityName", DefaultText = "Entity name is wrong", LocalDefaultText = "שם הישות שגוי", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, TextCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AddRemoveRequiredFields", DefaultText = "Add / Remove Required Fields", LocalDefaultText = "הוספה / הסרה של שדות חובה", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, TextCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.SelectObjectTable", DefaultText = "You must select an ObjectTable", LocalDefaultText = "עליך לבחור בלוח אובייקט", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, TextCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ViewCode", DefaultText = "View Code", LocalDefaultText = "צג קוד", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, TextCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Existed", DefaultText = "Existed", LocalDefaultText = "קיים", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, TextCodeRepository, textcodes);
            AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.All", DefaultText = "All", LocalDefaultText = "הכל", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, TextCodeRepository, textcodes);
        

            #region MainMenu
         

            #endregion

        }
        #endregion

        #region LoadTextCodes_CustomsPhysicalCheck

        private void LoadTextCodes_CustomsPhysicalCheck(Dictionary<string, TextCode> textcodes)
        {
            ObjectTable objectTable = ObjectContext.ObjectTables.Where(f => f.Name == "Customs.PhysicalCheck" && f.Tenant == 0).FirstOrDefault();

        //    AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.CustomsQueries", DefaultText = "Customs Queries", LocalDefaultText = "שאילתות מכס", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, TextCodeRepository, textcodes);
         //   AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.PhysicalChecks", DefaultText = "Physical Checks", LocalDefaultText = "בדיקות פיסיות", ObjectTableId = objectTable.Id, Tenant = 0, TextCodeTypeCode = "O", }, TextCodeRepository, textcodes);
   
        
          
            ObjectContext.SaveChanges();
        }

        #endregion

        #endregion




        #region Load menusTables
        public void LoadMenustables()
        {
            ObjectContext = WebFreightContext.GetContext(0);
            MenusTablesRepository = new MenusTableRepository(ObjectContext);
            FeatureRepository featureRepository = new FeatureRepository(0);
            Dictionary<string, MenusTable> tenantMenusTables = MenusTablesRepository.GetMenusTablesByTenant(0).ToDictionary(d => d.Code, a => a);

            List<ObjectTable> tenantObjectTables = ObjectTableRepository.GetObjectsByTenant(0).ToList();
            List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();

          //  Feature customBankFeature = tenantFeatures.Where(d => d.Code == "CUSTOMBANK" && d.FeatureTypeCode == "MENU").FirstOrDefault();
          //  Feature itemFeature = tenantFeatures.Where(d => d.Code == "ITEM" && d.FeatureTypeCode == "MENU").FirstOrDefault();
               
        
            #region Menus

         //   AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "CSDC", Tenant = 0, MenuTypeCode = "Main", IndexOfOrder = 10, CategoryTypeCode = null, TextCode = "General.MH.Declarations", Icon = "CustomersPath", FeatureId = customFeature.Id, ObjectTableId = tenantObjectTables.Where(o => o.Name == "Customs.Declaration").FirstOrDefault().Id }, MenusTablesRepository, tenantMenusTables);
          //  AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "CSRS", Tenant = 0, MenuTypeCode = "Main", IndexOfOrder = 11, CategoryTypeCode = null, TextCode = "General.MH.CustomsRequestsSheets", Icon = "ReportsPath", FeatureId = customFeature.Id, }, MenusTablesRepository, tenantMenusTables);
             

            #endregion

            #region Maintanance
            /*Others*/
        //    AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "MTCE", Tenant = 0, MenuTypeCode = "MTC", IndexOfOrder = 0, CategoryTypeCode = "CSM", TextCode = "General.MC.CSM.CustomsTables", Icon = "DocumentTypes.png", ObjectTableId = tenantObjectTables.Where(o => o.Name == "Customs.CustomsClosedTable").FirstOrDefault().Id, FeatureId = customFeature.Id }, MenusTablesRepository, tenantMenusTables);
         
          //  AddMenusTables.AddMenusTable(new MenusTableDetails() { Code = "MTCB", Tenant = 0, MenuTypeCode = "MTC", IndexOfOrder = 2, CategoryTypeCode = "CSM", TextCode = "General.MC.Customs.CustomBank", Icon = "DocumentTypes.png", ObjectTableId = tenantObjectTables.Where(o => o.Name == "Customs.CustomBank").FirstOrDefault().Id, FeatureId = customBankFeature.Id }, MenusTablesRepository, tenantMenusTables);
   
            // General.Features.EmailAlertSetting
            MenusTablesRepository.SubmitChanges();

      
            
            #endregion

             
            MenusTablesRepository.SubmitChanges();

        }
        #endregion

        #region Roles and features
        public void LoadRolesAndFeatures(int tenant)
        {
            ICommonDataContext ObjectContext = CommonDataContext.GetContext(tenant);
            FeatureRepository FeaturesRepository = new FeatureRepository(ObjectContext);
            RoleFeatureRepository RoleFeaturesRepository = new RoleFeatureRepository(ObjectContext);
            TextCodeRepository textCodeRep = new TextCodeRepository(tenant);
            ObjectTabelRepository objecttableRep = new ObjectTabelRepository(tenant);
            objectTabelQuery = new ObjectTabelQuery(objecttableRep);

            List<ObjectTablePM> objectTables = objectTabelQuery.GetObjectPMsByTenant(tenant).ToList();
            ObjectTablePM GeneralObjectTable = objectTables.Where(d => d.Name == "General").FirstOrDefault();

      //      ObjectTablePM CustomsPhysicalCheckObjectTable = objectTables.Where(d => d.Name == "Customs.PhysicalCheck").FirstOrDefault();
       //     ObjectTablePM CustomsDeclarationObjectTable = objectTables.Where(d => d.Name == "Customs.Declaration").FirstOrDefault();
    

            #region closed Tables
       //     ObjectTablePM checkEntityTypeObject = objectTables.Where(d => d.Name == "Customs.CheckEntityType" && d.Tenant == 0).FirstOrDefault();
        //    ObjectTablePM checkRepresentativeTypeObject = objectTables.Where(d => d.Name == "Customs.CheckRepresentativeType" && d.Tenant == 0).FirstOrDefault();
   
            
          
            #endregion
           
            RoleRepository RolesRepository = new RoleRepository(ObjectContext);
            Dictionary<string, Role> TenantRoles = RolesRepository.GetRoles(tenant).ToDictionary(d => d.Code, a => a);
            Dictionary<string, Feature> TenantFeatures = FeaturesRepository.GetFeaturesByTenant(tenant).ToDictionary(d => d.Code + d.ObjectTableId, a => a);
            Dictionary<string, TextCode> TextCodes = textCodeRep.GetTextCodesByTenant(tenant).ToDictionary(d => d.Code + d.Tenant + d.ObjectTableId, a => a);
            List<RoleFeature> TenantRoleFeatures = RoleFeaturesRepository.GetRoleFeaturesByTenant(tenant).ToList();

            #region Roles
            Role AdimistratorRole = AddRolesAndFeaturesClass.AddRole(new RoleDetails() { Code = "ADMN", Tenant = tenant, RoleTypeCode = "IN", Name = "Administrator", Description = "All system feauters" }, RolesRepository, TenantRoles);
            Role ManagerRole = AddRolesAndFeaturesClass.AddRole(new RoleDetails() { Code = "MANG", Tenant = tenant, RoleTypeCode = "IN", Name = "Manager", Description = "All feauters except system setup" }, RolesRepository, TenantRoles);
            Role FreightOperationRole = AddRolesAndFeaturesClass.AddRole(new RoleDetails() { Code = "FROP", Tenant = tenant, RoleTypeCode = "IN", Name = "Freight Operation", Description = "Full Operation include A/R Invoicing .\nNo Accounting, system setup and Manager Dashboards ." }, RolesRepository, TenantRoles);
            Role role_04 = AddRolesAndFeaturesClass.AddRole(new RoleDetails() { Code = "SALE", Tenant = tenant, RoleTypeCode = "SA", Name = "Sales", Description = "Quotes Management and Operational view" }, RolesRepository, TenantRoles);
            Role role_05 = AddRolesAndFeaturesClass.AddRole(new RoleDetails() { Code = "ACCT", Tenant = tenant, RoleTypeCode = "AC", Name = "Accounting", Description = "Full accounting include Payables and Payments .\nNo operational features, system setup and Manager Dashboards ." }, RolesRepository, TenantRoles);

            #endregion

            #region Module Features
    //        Feature F_57 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = exchangeRateObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Customs.ExchangeRate.Features.PackageFeature", NameTextCodeDefaultText = "Exchange Rate Package Feature", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
     //       Feature F_58 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = interfaceManagementObject.Id, Tenant = tenant, NameTextCodeCode = "Customs.InterfaceManagement.Features.PackageFeature", NameTextCodeDefaultText = "Interface Management Package Feature", Packagable = true }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
    
            #endregion

            #region features

            #region CustomsPhysicalCheck
        //    Feature CustomsPhysicalCheckFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", ObjectTableId = CustomsPhysicalCheckObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Customs.PhysicalCheck.Features.Read", NameTextCodeDefaultText = "Read", FeatureTypeCode = "READ" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
        //    Feature CustomsPhysicalCheckFeature2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GENERAL", Packagable = true, ObjectTableId = CustomsPhysicalCheckObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Customs.PhysicalCheck.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
       

            #endregion

            textCodeRep.SubmitChanges();
            FeaturesRepository.SubmitChanges();
   
     
            #region closed tables
            #region Read features
       //     Feature ConstraintTypeFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", ObjectTableId = constraintTypeObject.Id, Tenant = tenant, NameTextCodeCode = "Customs.ConstraintType.Features.Read", NameTextCodeDefaultText = "Read", FeatureTypeCode = "READ" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
        //    Feature ConstraintStatusFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", ObjectTableId = constraintStatusObject.Id, Tenant = tenant, NameTextCodeCode = "Customs.ConstraintStatus.Features.Read", NameTextCodeDefaultText = "Read", FeatureTypeCode = "READ" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
          
            
            #endregion
            textCodeRep.SubmitChanges();
            FeaturesRepository.SubmitChanges();
            #region Update Features
      //      Feature CustomsHouseTypeFeature2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", ObjectTableId = customsHouseTypeObject.Id, Tenant = tenant, NameTextCodeCode = "Customs.CustomsHouseType.Features.Update", NameTextCodeDefaultText = "Edit Customs House Type", FeatureTypeCode = "UPDT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
        
            #endregion

      

       //     Feature checkEntityTypeFeature = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CHECKENTITYTYPE", ObjectTableId = checkEntityTypeObject.Id, Tenant = tenant, NameTextCodeCode = "Customs.CheckEntityType.Features.Read", NameTextCodeDefaultText = "Check Entity Type", FeatureTypeCode = "QUER" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
       //     Feature checkRepresentativeTypeFeature = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CHECKREPRESENTATIVETYPE", ObjectTableId = checkRepresentativeTypeObject.Id, Tenant = tenant, NameTextCodeCode = "Customs.checkRepresentativeType.Features.Read", NameTextCodeDefaultText = "Check Representative Type", FeatureTypeCode = "QUER" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
             
            
            textCodeRep.SubmitChanges();
            FeaturesRepository.SubmitChanges();

            #region AccountingRequiredField

    //        Feature AccountingRequiredFieldFeature1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", ObjectTableId = AccountingRequiredFieldObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Accounting.AccountingRequiredField.Features.Read", NameTextCodeDefaultText = "Read", FeatureTypeCode = "READ" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
     //       Feature AccountingRequiredFieldFeature2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", ObjectTableId = AccountingRequiredFieldObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Accounting.AccountingRequiredField.Features.New", NameTextCodeDefaultText = "New Accounting Required Field", FeatureTypeCode = "NEW" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
     //       Feature AccountingRequiredFieldFeature3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", ObjectTableId = AccountingRequiredFieldObjectTable.Id, Tenant = tenant, NameTextCodeCode = "Accounting.AccountingRequiredField.Features.Edit", NameTextCodeDefaultText = "Edit Accounting Required Field", FeatureTypeCode = "UPDT" }, FeaturesRepository, textCodeRep, TenantFeatures, TextCodes);
        

            #endregion

            textCodeRep.SubmitChanges();
            FeaturesRepository.SubmitChanges();
            #endregion
            #endregion

       
            #region Administrator

            #region CustomsPhysicalCheck
         
            #endregion

    
            RolesRepository.SubmitChanges();
            #region closed tables
        
            #endregion

            #region CustomsRequiredField

            #endregion 


      
     
            #endregion
        

            #endregion


            #region Freight Operation


            #endregion


           






            RolesRepository.SubmitChanges();
        }
        

        #region Closed Tables

        public void LoadBaseTablesForDataBases()
        {
            GlobalDBRepository globalDbRep = new GlobalDBRepository();
            List<GlobalDB> dbList = globalDbRep.GetGlobalDBs().ToList();

            foreach (GlobalDB db in dbList)
            {
                LoadBaseTablesForConnection(db.DBConnection);
            }
        }

        public void UpgradeClosedTablesForTenantZero()
        {
            isUpdate = true;
            List<GlobalDB> dbList = null;
            using (TransactionScope scop = new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(0, 5, 0)))
            {
                GlobalDBRepository globalDbRep = new GlobalDBRepository();
                dbList = globalDbRep.GetGlobalDBs().ToList();
            }

            foreach (GlobalDB db in dbList)
            {
                LoadBaseTablesForConnection(db.DBConnection);
            }
        }

        private void LoadBaseTablesForConnection(string connectionStr)
        {
            AccountingContext customContext = new AccountingContext(DatabaseInitializer.GetConnection(connectionStr));


            //-------------Password Policy---------------

         //   PhysicalCheckOperationRepository physicalCheckOperationRepository = new PhysicalCheckOperationRepository(customContext);
         //   AddClosedTables.AddPhysicalCheckOperation(new PhysicalCheckOperationDetails() { Code = "1", EnglishName = "Invite", LocalName = "זימון" }, physicalCheckOperationRepository);
          //  AddClosedTables.AddPhysicalCheckOperation(new PhysicalCheckOperationDetails() { Code = "2", EnglishName = "Update", LocalName = "עדכון" }, physicalCheckOperationRepository);
       
        //    physicalCheckOperationRepository.SubmitChanges();


       //     PhysicalCheckStatusMessageRepository physicalCheckStatusMessageRepository = new PhysicalCheckStatusMessageRepository(customContext);
         //   AddClosedTables.AddPhysicalCheckStatusMessage(new PhysicalCheckStatusMessageDetails() { Code = "1", EnglishName = "First Transmition", LocalName = "שליחה ראשונית" }, physicalCheckStatusMessageRepository);
         //   AddClosedTables.AddPhysicalCheckStatusMessage(new PhysicalCheckStatusMessageDetails() { Code = "2", EnglishName = "Last Transmition", LocalName = "שליחה סופית" }, physicalCheckStatusMessageRepository);

         //   physicalCheckStatusMessageRepository.SubmitChanges();


         //   ClosedTableStatusRepository closedTableStatusRepository = new ClosedTableStatusRepository(customContext);
         //   AddClosedTables.AddClosedTableStatus(new ClosedTableStatusDetails() { Code = "1", EnglishName = "New", LocalName="חדש", }, closedTableStatusRepository);
          //  AddClosedTables.AddClosedTableStatus(new ClosedTableStatusDetails() { Code = "2", EnglishName = "Updating", LocalName = "מעדכן", }, closedTableStatusRepository);
          //  AddClosedTables.AddClosedTableStatus(new ClosedTableStatusDetails() { Code = "3", EnglishName = "Up ToDate", LocalName = "מעודכן", }, closedTableStatusRepository);

         //   closedTableStatusRepository.SubmitChanges();

         //   CustomsEnvoirmentTypeRepository customsEnvoirmentTypeRepository = new CustomsEnvoirmentTypeRepository(customContext);
          //  AddClosedTables.AddCustomsEnvoirmentType(new CustomsEnvoirmentTypeDetails() { Code = "1", EnglishName = "PrePilot", LocalName = "פיילוט רשות", }, customsEnvoirmentTypeRepository);
          //  AddClosedTables.AddCustomsEnvoirmentType(new CustomsEnvoirmentTypeDetails() { Code = "2", EnglishName = "Pilot", LocalName = "פיילוט חובה", }, customsEnvoirmentTypeRepository);
          //  AddClosedTables.AddCustomsEnvoirmentType(new CustomsEnvoirmentTypeDetails() { Code = "3", EnglishName = "Production", LocalName = "ייצור", }, customsEnvoirmentTypeRepository);
          //  customsEnvoirmentTypeRepository.SubmitChanges();
          //  AddNewDeclarationErrorMappingSample();

        }
          #endregion

        #region EventTypes
        public void LoadEventTypes()
        {
        }

        #endregion

        #region counters
        public void CreateTableCounters(int tenant)
        {
        }
        #endregion

   
      

        
        
        
    


    }
    
}