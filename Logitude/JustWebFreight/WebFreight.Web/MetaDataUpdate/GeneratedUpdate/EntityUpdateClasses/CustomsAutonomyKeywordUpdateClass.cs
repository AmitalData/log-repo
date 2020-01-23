using System;
using System.Collections.Generic;
using System.Linq;
 
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.QuoteModel;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.Repositories;
using WebFreight.Web.CommonDataModel;
using Logitude.BL.CommonDataModel.EntityPMs;
using WebFreight.Web.GlobalModel;
using WebFreight.Web.Helpers;
using WebFreight.Web.InfrastructureModel;
using Logitude.BL.InfrastructureModel.EntityPMs;
using WebFreight.Web.InvoiceModel;
using WebFreight.Web.MetaDataUpdate.AddClasses;
using WebFreight.Web.MetaDataUpdate.DetailClasses;
using WebFreight.Web.QuoteModel;
using WebFreight.Web.ShipmentsModel;
using WebFreight.Web.CommonDataModel.DomainServices;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System.Data.Entity.Core.EntityClient;
using System.Configuration;
using Simplog.Server.Infrastructure;
using System.Data.Common;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.BL;
using Logitude.CRM.Data.Repsitories;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.BL;
using Logitude.Accounting.Data.Repositories;
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.BL;
using Logitude.BookingLib.Data.Repositories;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.BL;
using Logitude.Customs.Data.Repsitories;
using Logitude.Social.Data.EntityPOCOs;
using Logitude.Social.BL;
using Logitude.Social.Data.Repsitories;
using Logitude.Server.Tools.CloseTablesClasses;
using Logitude.Customs.BL.CloseTables;
using Logitude.CRM.BL.CLoseTable;
using Logitude.BookingLib.BL.CLoseTable;
using Logitude.WarehouseLib.Data.Repositories;
using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.WarehouseLib.BL.CLoseTable;
using Logitude.TimeManagement.Data.Repositories;
using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.BL.CLoseTable;
namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.EntityUpdateClasses
{
   public class CustomsAutonomyKeywordUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "Customs.CustomsAutonomyKeyword",
			      				    IsNew =  true,
			      				    DBTableName =  "Customs.CustomsAutonomyKeywords",
			      				    OldDBTableName =  "Customs.CustomsAutonomyKeywords",
			      				    ObjectTableSingular =  "CustomsAutonomyKeyword",
			      				    ObjectTablePlural =  "CustomsAutonomyKeywords",
			      				    HasCustomFilter =  false,
			      				    HasCustomFields =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  false,
			      				    IsNewWizard =  true,
			      				    LookUp1 =  "Id",
			      				    LookUp2 =  "Id",
			      				    KeyPropertyPath =  "Id",
			      				    AutoCompleteSearchWindow =  false,
			      				    IsClosed =  false,
			      				    CacheOnClient =  false,
			      				    EditableFromAutoCompleteWindow =  false,
			      				    HasCounter =  false,
			      				    EnableAddFromLOV =  false,
			      				    IsRestrictable =  false,
			      				    IsMain =  true,
			      				    IsAutoComplete =  false,
			      				    EnableEditFromLOV =  false,
			      				    SortingByObjectField =  "Id",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    NewWizardControlName =  "AutonomyKeywordComponent",
			      				    LocalDefaultText =  "מילות מפתח להצהרת אוטונומיה ",
			      				    DefaultText =  "Customs Autonomy Keyword",
			      				    Code =  "3d96",
			      				    Name =  " Query Group",
			      				    GenerateDomainService =  true,
			      				    ClientModuleName =  "Customs",
			      				    ServerModuleName =  "Customs",
			      				    NewWizardComponentPath =  "./CustomsModules/CustomsCourier/Components/AutonomyKeyword/AutonomyKeywordComponent",
			      				    NoTS =  false,
			      				    ///NoDefaultFeatures =  false,
			      				    HasMenuButtons =  false,
			      				    AllowedForComputingPartners =  false,
			      				    Code1 =  "c5de",
			      				    Name1 =  " Query Group",
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    IsTabsHidden =  false,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "KeywordtypeCode",
					  						OldFieldName =  "KeywordtypeCode",
					  						IsNew =  true,
					  						IsChecked =  true,
					  						IsDeleted =  false,
					  						ObjectTableName =  "Customs.CustomsAutonomyKeyword",
					  						FieldsDataType =  "Text",
					  						MinLength =  1,
					  						MaxLength =  1,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  true,
					  						SystemMaxLength =  1,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "KeywordtypeCode",
					  						ListPropertyPath =  "KeywordtypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CustomsAutonomyKeyword",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "KeywordtypeCode",
					  						DefaultText =  "Keyword Type Code",
					  						FullLocalDefaultText =  "קוד מילת מפתח",
					  						ListFieldLable =  "KeywordtypeCodeListLable",
					  						ListLableDefaultText =  "Keyword Type Code",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						IsSpellCheckedFullFieldLable =  false,
					  						IsSpellCheckedHelpLocalDefaultText =  false,
					  						IsSpellCheckedShortLocalDefaultText =  false,
					  						IsSpellCheckedListLocalDefaultText =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "KeywordsList",
					  						OldFieldName =  "KeywordsList",
					  						IsNew =  true,
					  						IsChecked =  false,
					  						IsDeleted =  false,
					  						ObjectTableName =  "Customs.CustomsAutonomyKeyword",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  2000,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  true,
					  						SystemMaxLength =  2000,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Contains",
					  						MultiLine =  true,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "KeywordsList",
					  						ListPropertyPath =  "KeywordsList",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CustomsAutonomyKeyword",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "KeywordsList",
					  						DefaultText =  "Keywords List",
					  						FullLocalDefaultText =  "רשימת מילות מפתח",
					  						ListFieldLable =  "KeywordsListListLable",
					  						ListLableDefaultText =  "Keywords List",
					  						ListLocalDefaultText =  "רשימת מילות מפתח",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						IsSpellCheckedFullFieldLable =  false,
					  						IsSpellCheckedHelpLocalDefaultText =  false,
					  						IsSpellCheckedShortLocalDefaultText =  false,
					  						IsSpellCheckedListLocalDefaultText =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "KeywordtypeLocalName",
					  						OldFieldName =  "KeywordtypeLocalName",
					  						IsNew =  true,
					  						IsChecked =  true,
					  						IsDeleted =  false,
					  						ObjectTableName =  "Customs.CustomsAutonomyKeyword",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  300,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  300,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "KeywordtypeLocalName",
					  						ListPropertyPath =  "KeywordtypeLocalName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CustomsAutonomyKeyword",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "KeywordtypeLocalName",
					  						DefaultText =  "Keywordtype Name",
					  						FullLocalDefaultText =  "שם קוד מפתח",
					  						ListFieldLable =  "KeywordtypeLocalNameListLable",
					  						ListLableDefaultText =  "Keywordtype Name",
					  						ListLocalDefaultText =  "שם קוד מפתח",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						IsSpellCheckedFullFieldLable =  false,
					  						IsSpellCheckedHelpLocalDefaultText =  false,
					  						IsSpellCheckedShortLocalDefaultText =  false,
					  						IsSpellCheckedListLocalDefaultText =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	    {  
	        FeatureRepository featureRepository = new FeatureRepository(0); 
            List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
            IWebFreightContext objectContext = WebFreightContext.GetContext(0);  
	        QueryGroup CustomsAutonomyKeywordQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "3d96", Name = " Query Group" }, queryGroupRepository);
	        queryGroupRepository.SubmitChanges();

	        ObjectTable CustomsAutonomyKeywordObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Customs.CustomsAutonomyKeyword" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> CustomsAutonomyKeywordObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Customs.CustomsAutonomyKeyword").ToList();   

			   TextCode CustomsAutonomyKeywordTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CustomsAutonomyKeyword.Q.AllCustomsAutonomyKeyword", DefaultText = "All Autonomy Keyword",LocalDefaultText = "Autonomy Keyword", ObjectTableId = CustomsAutonomyKeywordObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature CustomsAutonomyKeywordFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CustomsAutonomyKeyword.Q.AllCustomsAutonomyKeyword", ObjectTableId = CustomsAutonomyKeywordObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomsAutonomyKeyword.Features.AllCustomsAutonomyKeyword", NameTextCodeDefaultText = "AllCustomsAutonomyKeyword", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query AllCustomsAutonomyKeywordQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = CustomsAutonomyKeywordTextCode_0.Id, Code = "AllCustomsAutonomyKeyword",  QueryGroupCode = "3d96", IndexOrder = 0, Tenant = 0, ObjectTableId = CustomsAutonomyKeywordObjectTable.Id, QuerySection = "Customs.CustomsAutonomyKeyword", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = CustomsAutonomyKeywordFeature_0.Id, DefaultSortName = "KeywordtypeCode", DefaultSortDirection = "Desending" }, queriesRepository, tenantQueries);
	
			 QueryColumn AllCustomsAutonomyKeywordQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCustomsAutonomyKeywordQuery.Id, IndexOrder = 0, ObjectFieldId = CustomsAutonomyKeywordObjectFields.Where(d => d.FieldName == "KeywordtypeCode" && d.ObjectTableId == CustomsAutonomyKeywordObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllCustomsAutonomyKeywordQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCustomsAutonomyKeywordQuery.Id, IndexOrder = 1, ObjectFieldId = CustomsAutonomyKeywordObjectFields.Where(d => d.FieldName == "KeywordtypeLocalName" && d.ObjectTableId == CustomsAutonomyKeywordObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllCustomsAutonomyKeywordQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCustomsAutonomyKeywordQuery.Id, IndexOrder = 2, ObjectFieldId = CustomsAutonomyKeywordObjectFields.Where(d => d.FieldName == "KeywordsList" && d.ObjectTableId == CustomsAutonomyKeywordObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 300 }, queryColumnsRepository, tenantQueryColumns);
	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable CustomsAutonomyKeywordObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.CustomsAutonomyKeyword" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> CustomsAutonomyKeywordObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Customs.CustomsAutonomyKeyword").ToList();
		       
	      

	         Screen CustomsAutonomyKeywordCustomsCustomsAutonomyKeywordHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "CustomsAutonomyKeyword.Customs.CustomsAutonomyKeywordHeaderScreen", Name = "Customs.CustomsAutonomyKeywordHeaderScreen", ObjectTableId = CustomsAutonomyKeywordObjectTable.Id, NumberOfColumns = 1, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField CustomsAutonomyKeywordCustomsCustomsAutonomyKeywordHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = CustomsAutonomyKeywordObjectFields.Where(d => d.FieldName == "KeywordtypeLocalName").FirstOrDefault().Id, ScreenId = CustomsAutonomyKeywordCustomsCustomsAutonomyKeywordHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    CustomsAutonomyKeywordObjectTable.HeaderScreenId = CustomsAutonomyKeywordCustomsCustomsAutonomyKeywordHeaderScreenScreen0.Id;
	   		  

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {    
			 ObjectTable CustomsAutonomyKeywordObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.CustomsAutonomyKeyword" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode CustomsAutonomyKeywordGeneralTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsAutonomyKeyword.TH.General", DefaultText = "General",LocalDefaultText = "כללי", ObjectTableId = CustomsAutonomyKeywordObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CustomsAutonomyKeywordGeneralFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CustomsAutonomyKeyword.Tab.General", ObjectTableId = CustomsAutonomyKeywordObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomsAutonomyKeyword.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode CustomsAutonomyKeywordEventsTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsAutonomyKeyword.TH.Events", DefaultText = "Events",LocalDefaultText = "אירועים", ObjectTableId = CustomsAutonomyKeywordObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CustomsAutonomyKeywordEventsFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CustomsAutonomyKeyword.Tab.Events", ObjectTableId = CustomsAutonomyKeywordObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomsAutonomyKeyword.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CPAK",HtmlComponentName = "AutonomyKeywordComponent",HtmlComponentUrl = "./CustomsModules/CustomsCourier/Components/AutonomyKeyword/AutonomyKeywordComponent", FeatureId = tenantFeatures.Where(d => d.Code == "CustomsAutonomyKeyword.Tab.General" && d.ObjectTableId == CustomsAutonomyKeywordObjectTable.Id).FirstOrDefault().Id, ControlPath = " ", ObjectTableId = CustomsAutonomyKeywordObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Customs.CustomsAutonomyKeyword.TH.General" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CPEK",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = tenantFeatures.Where(d => d.Code == "CustomsAutonomyKeyword.Tab.Events" && d.ObjectTableId == CustomsAutonomyKeywordObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = CustomsAutonomyKeywordObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Customs.CustomsAutonomyKeyword.TH.Events" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable CustomsAutonomyKeywordObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.CustomsAutonomyKeyword" && d.Tenant == 0).FirstOrDefault(); 
		   Feature CustomsAutonomyKeywordFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = CustomsAutonomyKeywordObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomsAutonomyKeyword.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature CustomsAutonomyKeywordFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = CustomsAutonomyKeywordObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomsAutonomyKeyword.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature CustomsAutonomyKeywordFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = CustomsAutonomyKeywordObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomsAutonomyKeyword.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature CustomsAutonomyKeywordFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = CustomsAutonomyKeywordObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomsAutonomyKeyword.Features.PackageFeature", NameTextCodeDefaultText = "CustomsAutonomyKeyword Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);    
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext)
	    {   
			ObjectTable CustomsAutonomyKeywordObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.CustomsAutonomyKeyword" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CREV",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
				IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = CustomsAutonomyKeywordObjectTable.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPEV",
                EnglishName = "Updated",
                Tenant = 0,
                AddedManually = false,
				IsManualEntry = false,
                LocalName = "Updated",
                ObjectTableId = CustomsAutonomyKeywordObjectTable.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
	    }     

   }
    
}
	 