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
   public class AdditionalServiceUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "AdditionalService",
			      				    DBTableName =  "AdditionalServices",
			      				    ObjectTableSingular =  "Additional Service",
			      				    ObjectTablePlural =  "Additional Services",
			      				    HasCustomFilter =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    IsEditable =  true,
			      				    IsNewWizard =  false,
			      				    LookUp1 =  "Name",
			      				    KeyPropertyPath =  "Id",
			      				    AutoCompleteSearchWindow =  false,
			      				    IsClosed =  false,
			      				    CacheOnClient =  true,
			      				    EditableFromAutoCompleteWindow =  false,
			      				    HasCounter =  false,
			      				    EnableAddFromLOV =  false,
			      				    IsRestrictable =  false,
			      				    IsMain =  true,
			      				    IsAutoComplete =  true,
			      				    EnableEditFromLOV =  false,
			      				    SortingByObjectField =  "Name",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    DefaultText =  "Additional Service",
			      				    Code =  "ADSV",
			      				    Name =  "AdditionalService",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Common",
			      				    HasMenuButtons =  false,
			      				    CustomFieldsCount =  0,
			      				    HasCustomFields =  false,
			      				    SearchFields =  "AdditionalService,AdditionalServices,,Id,Name",
			      				    HasDocuments =  false,
			      				    IsLookUp =  true,
			      				    AllowedForComputingPartners =  false,
			      				    DisableSearchBox =  false,
			      				    HasFiltersMenu =  false,
			      				    AllowedInQueues =  false,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Name",
					  						ObjectTableName =  "AdditionalService",
					  						FieldsDataType =  "nText",
					  						Code =  "Name",
					  						MaxLength =  60,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  true,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  true,
					  						SystemMaxLength =  60,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  true,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "Name",
					  						ListPropertyPath =  "Name",
					  						DisplayInLookUpIndex =  1,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  1,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AdditionalService",
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
					  						FullFieldLable =  "Name",
					  						DefaultText =  @"Name",
					  						ListFieldLable =  "NameListLable",
					  						ListLableDefaultText =  @"Name",
					  						HelpTextCode =  "Name",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SearchFields",
					  						ObjectTableName =  "AdditionalService",
					  						FieldsDataType =  "nText",
					  						Code =  "SearchFields",
					  						MaxLength =  1000,
					  						IsCustom =  false,
					  						MinLength =  0,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "SearchFields",
					  						ListPropertyPath =  "SearchFields",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AdditionalService",
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
					  						IsRequired =  false,
					  						FullFieldLable =  "SearchFields",
					  						DefaultText =  @"Search...",
					  						ListFieldLable =  "SearchFieldsListLable",
					  						ListLableDefaultText =  @"SearchFields",
					  						HelpTextCode =  "SearchFields",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InActive",
					  						ObjectTableName =  "AdditionalService",
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
					  						ValidForQuerySection1 =  "AdditionalService",
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
					  						FullFieldLable =  "InActive",
					  						DefaultText =  @"Inactive",
					  						ListFieldLable =  "InActiveListLable",
					  						ListLableDefaultText =  @"Inactive",
					  						HelpTextCode =  "InActive",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	    {  
	        FeatureRepository featureRepository = new FeatureRepository(0); 
            List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
            IWebFreightContext objectContext = WebFreightContext.GetContext(0);  
	        QueryGroup AdditionalServiceQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "ADSV", Name = "AdditionalService" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable AdditionalServiceObjectTable = objectContext.ObjectTables.Where(d => d.Name == "AdditionalService" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> AdditionalServiceObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "AdditionalService").ToList();   

			   TextCode AdditionalServiceTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AdditionalService.Q.AllAdditionalServices", DefaultText = @"All Additional Services",LocalDefaultText = null, ObjectTableId = AdditionalServiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature AdditionalServiceFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ALLADDITIONALSERVICES", ObjectTableId = AdditionalServiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "AdditionalService.Features.AllAdditionalServices", NameTextCodeDefaultText = "All Additional Services", FeatureTypeCode = "QUER", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query AllAdditionalServicesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = AdditionalServiceTextCode_0.Id, Code = "All Additional Services",  QueryGroupCode = "ADSV", IndexOrder = 0, Tenant = 0, ObjectTableId = AdditionalServiceObjectTable.Id, QuerySection = "AdditionalService", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = AdditionalServiceFeature_0.Id, DefaultSortName = null, DefaultSortDirection = null }, queriesRepository, tenantQueries);
	
			 QueryColumn AllAdditionalServicesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllAdditionalServicesQuery.Id, IndexOrder = 0, ObjectFieldId = AdditionalServiceObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTableId == AdditionalServiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllAdditionalServicesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllAdditionalServicesQuery.Id, IndexOrder = 1, ObjectFieldId = AdditionalServiceObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTableId == AdditionalServiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable AdditionalServiceObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "AdditionalService" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> AdditionalServiceObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "AdditionalService").ToList();
		       
	      

	         Screen AdditionalServiceHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "AdditionalService.HeaderScreen", Name = "Header Screen", ObjectTableId = AdditionalServiceObjectTable.Id, NumberOfColumns = 1, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField AdditionalServiceAdditionalServiceHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = AdditionalServiceObjectFields.Where(d => d.FieldName == "Name").FirstOrDefault().Id, ScreenId = AdditionalServiceHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    AdditionalServiceObjectTable.HeaderScreenId = AdditionalServiceHeaderScreenScreen0.Id;
	   		  
	      

	         Screen AdditionalServiceGeneralTabScreenScreen1 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "AdditionalService.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = AdditionalServiceObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 2, IsReadOnly = false }, screensRepository, tenantScreens);
      
            ScreenField AdditionalServiceAdditionalServiceGeneralTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = AdditionalServiceObjectFields.Where(d => d.FieldName == "Name").FirstOrDefault().Id, ScreenId = AdditionalServiceGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField AdditionalServiceAdditionalServiceGeneralTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = AdditionalServiceObjectFields.Where(d => d.FieldName == "InActive").FirstOrDefault().Id, ScreenId = AdditionalServiceGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
           

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {    
			 ObjectTable AdditionalServiceObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "AdditionalService" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode AdditionalServiceGeneralTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AdditionalService.TH.General", DefaultText = "General",LocalDefaultText = null, ObjectTableId = AdditionalServiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature AdditionalServiceGeneralFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AdditionalService.Tab.General", ObjectTableId = AdditionalServiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "AdditionalService.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode AdditionalServiceEventsTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AdditionalService.TH.Events", DefaultText = "Events",LocalDefaultText = null, ObjectTableId = AdditionalServiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature AdditionalServiceEventsFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AdditionalService.Tab.Events", ObjectTableId = AdditionalServiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "AdditionalService.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "AVGN",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = tenantFeatures.Where(d => d.Code == "AdditionalService.Tab.General" && d.ObjectTableId == AdditionalServiceObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.Infrastructure.GeneralControls.GeneralTabControl", ObjectTableId = AdditionalServiceObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "AdditionalService.TH.General" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "AVEV",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = tenantFeatures.Where(d => d.Code == "AdditionalService.Tab.Events" && d.ObjectTableId == AdditionalServiceObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = AdditionalServiceObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "AdditionalService.TH.Events" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable AdditionalServiceObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "AdditionalService" && d.Tenant == 0).FirstOrDefault(); 
		   Feature AdditionalServiceFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = AdditionalServiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "AdditionalService.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature AdditionalServiceFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = AdditionalServiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "AdditionalService.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature AdditionalServiceFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = AdditionalServiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "AdditionalService.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature AdditionalServiceFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = AdditionalServiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "AdditionalService.Features.PackageFeature", NameTextCodeDefaultText = "AdditionalService Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);    
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable AdditionalServiceObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "AdditionalService" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "UPAS",
                ShortView =  false,
                IsManualEntry =  false,
                LocalName =  "Additional Service Updated",
                EnglishName =  "Additional Service Updated",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = AdditionalServiceObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CRAS",
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
                ObjectTableId = AdditionalServiceObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable AdditionalServiceObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "AdditionalService" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode AdditionalServiceTextCode_AdditionalService = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AdditionalService", DefaultText = "Additional Service",LocalDefaultText = null, ObjectTableId = AdditionalServiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "T", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 