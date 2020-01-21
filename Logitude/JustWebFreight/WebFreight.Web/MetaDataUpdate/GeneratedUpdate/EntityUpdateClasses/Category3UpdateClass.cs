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
using Logitude.Customs.BL.ClosedTable;
using Logitude.CRM.BL.CLoseTable;
using Logitude.BookingLib.BL.CLoseTable;
using Logitude.WarehouseLib.Data.Repositories;
using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.WarehouseLib.BL.CLoseTable;
using Logitude.TimeManagement.Data.Repositories;
using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.BL.CLoseTable;
using Logitude.BL.ShipmentsModel.CloseTables;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Logitude.BL.ShipmentsModel;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.GlobalModel;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.BL;
using Logitude.TariffModule.Data.Repositories;
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.BL.CLoseTable;

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.EntityUpdateClasses
{
   public class Category3UpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "Category3",
			      				    IsNew =  false,
			      				    DBTableName =  "Category3",
			      				    OldDBTableName =  "Category3",
			      				    ObjectTableSingular =  "Category 3",
			      				    ObjectTablePlural =  "Categories 3",
			      				    DescriptionDefaultText =  "This table allow user to categorize GLAccounts",
			      				    HasCustomFilter =  false,
			      				    HasCustomFields =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  false,
			      				    IsNewWizard =  true,
			      				    LookUp1 =  "EnglishName",
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
			      				    SortingByObjectField =  "EnglishName",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    NewWizardControlName =  "NewCategory3Component",
			      				    DefaultText =  "Category 3",
			      				    NewButtonDefaultText =  "New Category 3",
			      				    Code =  "77cf",
			      				    Name =  "Category3 Query Group",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Accounting",
			      				    NewWizardComponentPath =  "./Accounting/Components/Maintenance/NewCategory3Component",
			      				    NoTS =  false,
			      				    HasMenuButtons =  false,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "EnglishName",
					  						OldFieldName =  "EnglishName",
					  						ObjectTableName =  "Category3",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  30,
					  						IsRequired =  true,
					  						DisplayOnLookUp =  true,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  30,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  true,
					  						PMPropertyPath =  "EnglishName",
					  						ListPropertyPath =  "EnglishName",
					  						DisplayInLookUpIndex =  1,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  1,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Category3",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "EnglishName",
					  						DefaultText =  "English Name",
					  						FullLocalDefaultText =  "שם באנגלית",
					  						ListFieldLable =  "EnglishNameListLable",
					  						ListLableDefaultText =  "English Name",
					  						ListLocalDefaultText =  "שם באנגלית",
					  						IsMaxLength =  true,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LocalName",
					  						OldFieldName =  "LocalName",
					  						ObjectTableName =  "Category3",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  30,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  true,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  30,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  true,
					  						PMPropertyPath =  "LocalName",
					  						ListPropertyPath =  "LocalName",
					  						DisplayInLookUpIndex =  2,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Category3",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LocalName",
					  						DefaultText =  "Local Name",
					  						FullLocalDefaultText =  "שם",
					  						ListFieldLable =  "LocalNameListLable",
					  						ListLableDefaultText =  "Local Name",
					  						ListLocalDefaultText =  "שם",
					  						IsMaxLength =  true,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Inactive",
					  						OldFieldName =  "Inactive",
					  						ObjectTableName =  "Category3",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Inactive",
					  						ListPropertyPath =  "Inactive",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Category3",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Inactive",
					  						DefaultText =  "Inactive",
					  						FullLocalDefaultText =  "לא פעיל",
					  						ListFieldLable =  "InactiveListLable",
					  						ListLableDefaultText =  "Inactive",
					  						ListLocalDefaultText =  "לא פעיל",
					  						IsMaxLength =  true,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SearchFields",
					  						OldFieldName =  "SearchFields",
					  						ObjectTableName =  "Category3",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  1000,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  1000,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "SearchFields",
					  						ListPropertyPath =  "SearchFields",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SearchFields",
					  						DefaultText =  "Search Name",
					  						ListFieldLable =  "SearchFieldsListLable",
					  						ListLableDefaultText =  "search",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	    {  
	        FeatureRepository featureRepository = new FeatureRepository(0); 
            //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
            IWebFreightContext objectContext = WebFreightContext.GetContext(0);  
	        QueryGroup Category3QueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "77cf", Name = "Category3 Query Group" }, queryGroupRepository);
						QueryGroup Category3QueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "7d56", Name = " Query Group" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable Category3ObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Category3" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> Category3ObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Category3").ToList();   

			   TextCode Category3TextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Category3.Q.allcategories3", DefaultText = @"Categories 3",LocalDefaultText = null, ObjectTableId = Category3ObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature Category3Feature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Category3.Q.allcategories3", ObjectTableId = Category3ObjectTable.Id, Tenant = 0, NameTextCodeCode = "Category3.Features.allcategories3", NameTextCodeDefaultText = "allcategories3", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query allcategories3Query = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = Category3TextCode_0.Id, NameTextCodeCode = Category3TextCode_0.Code, ObjectTableName = "Category3", Code = "allcategories3",  QueryGroupCode = "77cf", IndexOrder = 0, Tenant = 0, ObjectTableId = Category3ObjectTable.Id, QuerySection = "Category3", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = Category3Feature_0.Id, DefaultSortName = "EnglishName", DefaultSortDirection = "Ascending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn allcategories3QueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allcategories3Query.Id,QueryCode = allcategories3Query.UniqueCode, IndexOrder = 0, ObjectFieldId = Category3ObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == Category3ObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = Category3ObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == Category3ObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn allcategories3QueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allcategories3Query.Id,QueryCode = allcategories3Query.UniqueCode, IndexOrder = 1, ObjectFieldId = Category3ObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == Category3ObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = Category3ObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == Category3ObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn allcategories3QueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = allcategories3Query.Id,QueryCode = allcategories3Query.UniqueCode, IndexOrder = 2, ObjectFieldId = Category3ObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == Category3ObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = Category3ObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == Category3ObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable Category3ObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Category3" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> Category3ObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Category3").ToList();
		       
	      

	         Screen Category3HeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Category3.HeaderScreen", Name = "HeaderScreen", ObjectTableId = Category3ObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 2, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField Category3Category3HeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = Category3ObjectFields.Where(d => d.FieldName == "EnglishName").FirstOrDefault().Id, ScreenId = Category3HeaderScreenScreen0.Id, ObjectFieldCode = Category3ObjectFields.Where(d => d.FieldName == "EnglishName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField Category3Category3HeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = Category3ObjectFields.Where(d => d.FieldName == "LocalName").FirstOrDefault().Id, ScreenId = Category3HeaderScreenScreen0.Id, ObjectFieldCode = Category3ObjectFields.Where(d => d.FieldName == "LocalName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField Category3Category3HeaderScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = Category3ObjectFields.Where(d => d.FieldName == "Inactive").FirstOrDefault().Id, ScreenId = Category3HeaderScreenScreen0.Id, ObjectFieldCode = Category3ObjectFields.Where(d => d.FieldName == "Inactive").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    Category3ObjectTable.HeaderScreenId = Category3HeaderScreenScreen0.Id;
	   		  
	      

	         Screen Category3GeneralTabScreenScreen1 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Category3.GeneralTabScreen", Name = "GeneralTabScreen", ObjectTableId = Category3ObjectTable.Id, NumberOfColumns = 1, NumberOfRows = 3, IsReadOnly = false }, screensRepository, tenantScreens);
      
            ScreenField Category3Category3GeneralTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = Category3ObjectFields.Where(d => d.FieldName == "EnglishName").FirstOrDefault().Id, ScreenId = Category3GeneralTabScreenScreen1.Id, ObjectFieldCode = Category3ObjectFields.Where(d => d.FieldName == "EnglishName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField Category3Category3GeneralTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = Category3ObjectFields.Where(d => d.FieldName == "LocalName").FirstOrDefault().Id, ScreenId = Category3GeneralTabScreenScreen1.Id, ObjectFieldCode = Category3ObjectFields.Where(d => d.FieldName == "LocalName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField Category3Category3GeneralTabScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ObjectFieldId = Category3ObjectFields.Where(d => d.FieldName == "Inactive").FirstOrDefault().Id, ScreenId = Category3GeneralTabScreenScreen1.Id, ObjectFieldCode = Category3ObjectFields.Where(d => d.FieldName == "Inactive").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
           

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable Category3ObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Category3" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode Category3GeneralTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Category3.TH.General", DefaultText = "General",LocalDefaultText = null, ObjectTableId = Category3ObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature Category3GeneralFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Category3.Tab.General", ObjectTableId = Category3ObjectTable.Id, Tenant = 0, NameTextCodeCode = "Category3.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode Category3EventsTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Category3.TH.Events", DefaultText = "Events",LocalDefaultText = null, ObjectTableId = Category3ObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature Category3EventsFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Category3.Tab.Events", ObjectTableId = Category3ObjectTable.Id, Tenant = 0, NameTextCodeCode = "Category3.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "C3GT",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = Category3GeneralFeature_TH0.Id,FeatureUniqeCode = Category3GeneralFeature_TH0.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.GeneralControls.GeneralTabControl", ObjectTableId = Category3ObjectTable.Id, TabNameTextCodeId = Category3GeneralTextCode_TH0.Id, TabNameTextCodeCode = Category3GeneralTextCode_TH0.Code, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "C3ET",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = Category3EventsFeature_TH1.Id,FeatureUniqeCode = Category3EventsFeature_TH1.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = Category3ObjectTable.Id, TabNameTextCodeId = Category3EventsTextCode_TH1.Id, TabNameTextCodeCode = Category3EventsTextCode_TH1.Code, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable Category3ObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Category3" && d.Tenant == 0).FirstOrDefault(); 

		   Feature Category3FeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = true, ObjectTableId = Category3ObjectTable.Id, Tenant = 0, NameTextCodeCode = "Category3.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature Category3FeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = true, ObjectTableId = Category3ObjectTable.Id, Tenant = 0, NameTextCodeCode = "Category3.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature Category3FeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = true, ObjectTableId = Category3ObjectTable.Id, Tenant = 0, NameTextCodeCode = "Category3.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature Category3FeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = true, ObjectTableId = Category3ObjectTable.Id, Tenant = 0, NameTextCodeCode = "Category3.Features.PackageFeature", NameTextCodeDefaultText = "Category3 Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);    
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable Category3ObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Category3" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CREV",
                EnglishName =  "Created",
                LocalName =  "Created",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = Category3ObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "UPEV",
                EnglishName =  "Updated",
                LocalName =  "Updated",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = Category3ObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {     
	    
}

    

   }
    
}
	 