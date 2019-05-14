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
   public class TicketStageUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "TicketStage",
			      				    DBTableName =  "TicketStages",
			      				    ObjectTableSingular =  "Ticket Stage",
			      				    ObjectTablePlural =  "Ticket Stages",
			      				    IsEditable =  false,
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
			      				    DefaultText =  "Ticket Stage",
			      				    Code =  "TAQG",
			      				    Name =  "TicketStage",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "CRM",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Code",
					  						ObjectTableName =  "TicketStage",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  2,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  true,
					  						PMPropertyPath =  "Code",
					  						ListPropertyPath =  "Code",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "TicketStage",
					  						DisplayInEntityVariables =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Code",
					  						DefaultText =  "Code",
					  						ListFieldLable =  "CodeListLable",
					  						ListLableDefaultText =  "Code",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Name",
					  						ObjectTableName =  "TicketStage",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  60,
					  						IsRequired =  true,
					  						DisplayOnLookUp =  true,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  60,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  true,
					  						PMPropertyPath =  "Name",
					  						ListPropertyPath =  "Name",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "TicketStage",
					  						DisplayInEntityVariables =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Name",
					  						DefaultText =  "Name",
					  						ListFieldLable =  "NameListLable",
					  						ListLableDefaultText =  "Name",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SearchFields",
					  						ObjectTableName =  "TicketStage",
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
					  						DisplayInEntityVariables =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SearchFields",
					  						DefaultText =  "Search...",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Inactive",
					  						ObjectTableName =  "TicketStage",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  true,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  true,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
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
					  						ValidForQuerySection1 =  "TicketStage",
					  						DisplayInEntityVariables =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Inactive",
					  						DefaultText =  "In Active",
					  						ListFieldLable =  "InactiveListLable",
					  						ListLableDefaultText =  "In Active",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	    {  
	        FeatureRepository featureRepository = new FeatureRepository(0); 
            //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
            IWebFreightContext objectContext = WebFreightContext.GetContext(0);  
	        QueryGroup TicketStageQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "TAQG", Name = "TicketStage" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable TicketStageObjectTable = objectContext.ObjectTables.Where(d => d.Name == "TicketStage" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> TicketStageObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "TicketStage").ToList();   

			   TextCode TicketStageTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TicketStage.Q.AllTicketStages", DefaultText = @"Ticket Stages",LocalDefaultText = null, ObjectTableId = TicketStageObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature TicketStageFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ALLTICKETSTAGES", ObjectTableId = TicketStageObjectTable.Id, Tenant = 0, NameTextCodeCode = "TicketStage.Features.AllTicketStagess", NameTextCodeDefaultText = "All Ticket Stages", FeatureTypeCode = "QUER", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query AllTicketStagesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = TicketStageTextCode_0.Id, Code = "All Ticket Stages",  QueryGroupCode = "TAQG", IndexOrder = 0, Tenant = 0, ObjectTableId = TicketStageObjectTable.Id, QuerySection = "TicketStage", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = TicketStageFeature_0.Id, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn AllTicketStagesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllTicketStagesQuery.Id, IndexOrder = 0, ObjectFieldId = TicketStageObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTableId == TicketStageObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllTicketStagesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllTicketStagesQuery.Id, IndexOrder = 1, ObjectFieldId = TicketStageObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == TicketStageObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable TicketStageObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "TicketStage" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> TicketStageObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "TicketStage").ToList();
		       
	      

	         Screen TicketStageHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "TicketStage.HeaderScreen", Name = "Header Screen", ObjectTableId = TicketStageObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField TicketStageTicketStageHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = TicketStageObjectFields.Where(d => d.FieldName == "Name").FirstOrDefault().Id, ScreenId = TicketStageHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    TicketStageObjectTable.HeaderScreenId = TicketStageHeaderScreenScreen0.Id;
	   		  
	      

	         Screen TicketStageGeneralTabScreenScreen1 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "TicketStage.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = TicketStageObjectTable.Id, NumberOfColumns = 1, NumberOfRows = 4, IsReadOnly = false }, screensRepository, tenantScreens);
      
            ScreenField TicketStageTicketStageGeneralTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = TicketStageObjectFields.Where(d => d.FieldName == "Name").FirstOrDefault().Id, ScreenId = TicketStageGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField TicketStageTicketStageGeneralTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = TicketStageObjectFields.Where(d => d.FieldName == "Inactive").FirstOrDefault().Id, ScreenId = TicketStageGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
           

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable TicketStageObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "TicketStage" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode TicketStageGeneralTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TicketStage.TH.General", DefaultText = "General",LocalDefaultText = null, ObjectTableId = TicketStageObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature TicketStageGeneralFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TicketStage.Tab.General", ObjectTableId = TicketStageObjectTable.Id, Tenant = 0, NameTextCodeCode = "TicketStage.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode TicketStageEventsTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TicketStage.TH.Events", DefaultText = "Events",LocalDefaultText = null, ObjectTableId = TicketStageObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature TicketStageEventsFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TicketStage.Tab.Events", ObjectTableId = TicketStageObjectTable.Id, Tenant = 0, NameTextCodeCode = "TicketStage.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "TAGE",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = TicketStageGeneralFeature_TH0.Id, ControlPath = "Simplog.Infrastructure.GeneralControls.GeneralTabControl", ObjectTableId = TicketStageObjectTable.Id, TabNameTextCodeId = TicketStageGeneralTextCode_TH0.Id, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "TAEV",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = TicketStageEventsFeature_TH1.Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = TicketStageObjectTable.Id, TabNameTextCodeId = TicketStageEventsTextCode_TH1.Id, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable TicketStageObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "TicketStage" && d.Tenant == 0).FirstOrDefault(); 

		   Feature TicketStageFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = TicketStageObjectTable.Id, Tenant = 0, NameTextCodeCode = "TicketStage.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature TicketStageFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = TicketStageObjectTable.Id, Tenant = 0, NameTextCodeCode = "TicketStage.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature TicketStageFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = TicketStageObjectTable.Id, Tenant = 0, NameTextCodeCode = "TicketStage.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature TicketStageFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = TicketStageObjectTable.Id, Tenant = 0, NameTextCodeCode = "TicketStage.Features.PackageFeature", NameTextCodeDefaultText = "TicketStage Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);    
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable TicketStageObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "TicketStage" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "UPTS",
                ShortView =  false,
                IsManualEntry =  false,
                LocalName =  "Ticket Stage Updated",
                EnglishName =  "Ticket Stage Updated",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = TicketStageObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CRTS",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Ticket Stage Created",
                EnglishName =  "Ticket Stage Created",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = TicketStageObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "ACTS",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Ticket Stage Activated",
                EnglishName =  "Ticket Stage Activated",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = TicketStageObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "IATS",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Ticket Stage Inactivated",
                EnglishName =  "Ticket Stage Inactivated",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = TicketStageObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CREV",
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
                ObjectTableId = TicketStageObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "UPEV",
                ShortView =  false,
                IsManualEntry =  false,
                LocalName =  "Updated",
                EnglishName =  "Updated",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = TicketStageObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
		   FeatureRepository featureRepository = new FeatureRepository(0); 
		   //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList(); 
		   ObjectTable TicketStageObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "TicketStage" && d.Tenant == 0).FirstOrDefault();  

		   TextCodeRepository.SubmitChanges();
		   FeaturesRepository.SubmitChanges();
		   MenuButtonGroup TicketStageMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
				{
					MenuButtonGroupType = "TicketStageEdit",
					Name = "TicketStageEditButtonsGroup",
					ObjectTableId = TicketStageObjectTable.Id,
					Tenant = 0
				}, menuButtonGroupRepository, tenantMenuButtonGroups); 
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {     
	    
}

    

   }
    
}
	 