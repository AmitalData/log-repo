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
namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.CommonDataModel.EntityUpdateClasses
{
   public class CommodityUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "Commodity",
			      				    DBTableName =  "Commodities",
			      				    ObjectTableSingular =  "Commodity",
			      				    ObjectTablePlural =  "Commodities",
			      				    DefaultText =  "Commodity",
			      				    Name =  "Commodity",
			      				    IsNewWizard =  false,
			      				    HasCustomFilter =  false,
			      				    KeyPropertyPath =  "Id",
			      				    AutoCompleteSearchWindow =  false,
			      				    IsClosed =  false,
			      				    CacheOnClient =  false,
			      				    EditableFromAutoCompleteWindow =  false,
			      				    HasCounter =  false,
			      				    EnableEditFromLOV =  false,
			      				    EnableAddFromLOV =  false,
			      				    IsRestrictable =  false,
			      				    IsMain =  true,
			      				    IsAutoComplete =  false,
			      				    CustomFieldsCount =  0,
			      				    HasCustomFields =  false,
			      				    InActive =  false,
			      				    SearchFields =  "Commodity,Commodities,,Id,",
			      				    IsSaveButtonVisible =  true,
			      				    EnableSecurity =  true,
			      				    ObjectTableTypeCode =  "MD",
			      				    IsComposition =  false,
			      				    MaxNumberOfCustomFields =  0,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    IsEditable =  true,
			      				    AllowedForComputingPartners =  false,
			      				    DisableSearchBox =  false,
			      				    ClientModuleName =  "Common",
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasMenuButtons =  false,
			      				    HasFiltersMenu =  false,
			      				    AllowedInQueues =  false,
			      				    Code =  "CMMD",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Code",
					  						ObjectTableName =  "Commodity",
					  						FieldsDataType =  "Text",
					  						Code =  "Code",
					  						MaxLength =  15,
					  						IsCustom =  false,
					  						MinLength =  4,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "Code",
					  						ListPropertyPath =  "Code",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Commodity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  true,
					  						FullFieldLable =  "Code",
					  						DefaultText =  @"Code",
					  						ListFieldLable =  "CodeListLable",
					  						ListLableDefaultText =  @"Code",
					  						HelpTextCode =  "Code",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Name",
					  						ObjectTableName =  "Commodity",
					  						FieldsDataType =  "nText",
					  						Code =  "Name",
					  						MaxLength =  250,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "Name",
					  						ListPropertyPath =  "Name",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Commodity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  true,
					  						FullFieldLable =  "Name",
					  						DefaultText =  @"Name",
					  						ListFieldLable =  "NameListLable",
					  						ListLableDefaultText =  @"Name",
					  						HelpTextCode =  "Name",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SearchFields",
					  						ObjectTableName =  "Commodity",
					  						FieldsDataType =  "nText",
					  						Code =  "SearchFields",
					  						MaxLength =  1000,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Contains",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Commodity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "SearchFields",
					  						DefaultText =  @"Search codes/ names",
					  						HelpTextCode =  "SearchFields",
					  						HelpTextDefaultText =  @"Searching by :\n1: codes \n2: names",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InActive",
					  						ObjectTableName =  "Commodity",
					  						FieldsDataType =  "Boolean",
					  						Code =  "InActive",
					  						MaxLength =  15,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "InActive",
					  						ListPropertyPath =  "InActive",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Commodity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  true,
					  						FullFieldLable =  "InActive",
					  						DefaultText =  @"Inactive",
					  						ListFieldLable =  "InActiveListLable",
					  						ListLableDefaultText =  @"Inactive",
					  						HelpTextCode =  "InActive",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AirlineId",
					  						ObjectTableName =  "Commodity",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Card",
					  						Code =  "AirlineId",
					  						MaxLength =  15,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "AirlineId",
					  						ListPropertyPath =  "AirlineId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Commodity",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "AirlineId",
					  						DefaultText =  @"Airline",
					  						HelpTextCode =  "AirlineId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	    {  
	        FeatureRepository featureRepository = new FeatureRepository(0); 
            List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
            IWebFreightContext objectContext = WebFreightContext.GetContext(0);  
	        QueryGroup CommodityQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "CMMD", Name = "Commodity" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable CommodityObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Commodity" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> CommodityObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Commodity").ToList();   

			   TextCode CommodityTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Commodity.Q.AllCommodities", DefaultText = @"All Commodities",LocalDefaultText = null, ObjectTableId = CommodityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature CommodityFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Commodity.Q.AllQuery", ObjectTableId = CommodityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Commodity.Features.AllCommodities", NameTextCodeDefaultText = "All Commodities", FeatureTypeCode = "QUER", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query AllCommoditiesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = CommodityTextCode_0.Id, Code = "All Commodities",  QueryGroupCode = "CMMD", IndexOrder = 0, Tenant = 0, ObjectTableId = CommodityObjectTable.Id, QuerySection = "Commodity", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = CommodityFeature_0.Id, DefaultSortName = null, DefaultSortDirection = null }, queriesRepository, tenantQueries);
	
			 QueryColumn AllCommoditiesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCommoditiesQuery.Id, IndexOrder = 1, ObjectFieldId = CommodityObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == CommodityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 90 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllCommoditiesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCommoditiesQuery.Id, IndexOrder = 2, ObjectFieldId = CommodityObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTableId == CommodityObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);
	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable CommodityObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Commodity" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> CommodityObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Commodity").ToList();
		       
	      

	         Screen CommodityHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Commodity.HeaderScreen", Name = "Header Screen", ObjectTableId = CommodityObjectTable.Id, NumberOfColumns = 1, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField CommodityCommodityHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = CommodityObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().Id, ScreenId = CommodityHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    CommodityObjectTable.HeaderScreenId = CommodityHeaderScreenScreen0.Id;
	   		  
	      

	         Screen CommodityGeneralTabScreenScreen1 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Commodity.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = CommodityObjectTable.Id, NumberOfColumns = 1, NumberOfRows = 3, IsReadOnly = false }, screensRepository, tenantScreens);
      
            ScreenField CommodityCommodityGeneralTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = CommodityObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().Id, ScreenId = CommodityGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CommodityCommodityGeneralTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = CommodityObjectFields.Where(d => d.FieldName == "Name").FirstOrDefault().Id, ScreenId = CommodityGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CommodityCommodityGeneralTabScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ObjectFieldId = CommodityObjectFields.Where(d => d.FieldName == "InActive").FirstOrDefault().Id, ScreenId = CommodityGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
           

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {    
			 ObjectTable CommodityObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Commodity" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode CommodityGeneralTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Commodity.TH.General", DefaultText = "General",LocalDefaultText = null, ObjectTableId = CommodityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CommodityGeneralFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Commodity.Tab.General", ObjectTableId = CommodityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Commodity.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode CommodityEventsTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Commodity.TH.Events", DefaultText = "Events",LocalDefaultText = null, ObjectTableId = CommodityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CommodityEventsFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Commodity.Tab.Events", ObjectTableId = CommodityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Commodity.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CMGN",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = tenantFeatures.Where(d => d.Code == "Commodity.Tab.General" && d.ObjectTableId == CommodityObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.Infrastructure.GeneralControls.GeneralTabControl", ObjectTableId = CommodityObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Commodity.TH.General" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CMEV",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = tenantFeatures.Where(d => d.Code == "Commodity.Tab.Events" && d.ObjectTableId == CommodityObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = CommodityObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Commodity.TH.Events" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable CommodityObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Commodity" && d.Tenant == 0).FirstOrDefault(); 
		   Feature CommodityFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = CommodityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Commodity.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature CommodityFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = CommodityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Commodity.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature CommodityFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = CommodityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Commodity.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature CommodityFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = CommodityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Commodity.Features.PackageFeature", NameTextCodeDefaultText = "Commodity Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes); 

		   		   //--------------> Additional Features <--------------\\

		   Feature CommodityFeature_Commodity_M_Commodities = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Commodity.M.Commodities", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = true, IsCoreFeature = false, ObjectTableId = CommodityObjectTable.Id, Tenant = 0, NameTextCodeCode = "Commodity.Features.Commodities", NameTextCodeDefaultText = @"Commodities" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

   
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable CommodityObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Commodity" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "UPCM",
                ShortView =  false,
                IsManualEntry =  false,
                LocalName =  "Commodity Updated",
                EnglishName =  "Commodity Updated",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CommodityObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CRCM",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Created",
                EnglishName =  "Created",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CommodityObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable CommodityObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Commodity" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode CommodityTextCode_Commodity = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Commodity", DefaultText = "Commodity",LocalDefaultText = null, ObjectTableId = CommodityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "T", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 