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
   public class TicketSeverityUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "TicketSeverity",
			      				    DBTableName =  "TicketSeverities",
			      				    ObjectTableSingular =  "Ticket Severity",
			      				    ObjectTablePlural =  "Ticket Severities",
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
			      				    SortingByObjectField =  "Severity",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    DefaultText =  "Ticket Severity",
			      				    Code =  "TSQG",
			      				    Name =  "TicketSeverity",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "CRM",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Code",
					  						ObjectTableName =  "TicketSeverity",
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
					  						ValidForQuerySection1 =  "TicketSeverity",
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
					  						ObjectTableName =  "TicketSeverity",
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
					  						ValidForQuerySection1 =  "TicketSeverity",
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
					  						ObjectTableName =  "TicketSeverity",
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
					 
					 						FieldName =  "Severity",
					  						ObjectTableName =  "TicketSeverity",
					  						FieldsDataType =  "Integer",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  true,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Severity",
					  						ListPropertyPath =  "Severity",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  true,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DisplayInEntityVariables =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Severity",
					  						DefaultText =  "Severity ",
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
					  						ObjectTableName =  "TicketSeverity",
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
					  						ValidForQuerySection1 =  "TicketSeverity",
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
	        QueryGroup TicketSeverityQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "TSQG", Name = "TicketSeverity" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable TicketSeverityObjectTable = objectContext.ObjectTables.Where(d => d.Name == "TicketSeverity" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> TicketSeverityObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "TicketSeverity").ToList();   

			   TextCode TicketSeverityTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TicketSeverity.Q.AllTicketSeverities", DefaultText = @"Ticket Severities",LocalDefaultText = null, ObjectTableId = TicketSeverityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature TicketSeverityFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ALLTICKETSEVERITIES", ObjectTableId = TicketSeverityObjectTable.Id, Tenant = 0, NameTextCodeCode = "TicketSeverity.Features.AllTicketSeverities", NameTextCodeDefaultText = "All Ticket Severities", FeatureTypeCode = "QUER", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query AllTicketSeveritiesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = TicketSeverityTextCode_0.Id, NameTextCodeCode = TicketSeverityTextCode_0.Code, ObjectTableName = "TicketSeverity", Code = "All Ticket Severities",  QueryGroupCode = "TSQG", IndexOrder = 0, Tenant = 0, ObjectTableId = TicketSeverityObjectTable.Id, QuerySection = "TicketSeverity", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = TicketSeverityFeature_0.Id, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn AllTicketSeveritiesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllTicketSeveritiesQuery.Id,QueryCode = AllTicketSeveritiesQuery.UniqueCode, IndexOrder = 0, ObjectFieldId = TicketSeverityObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTableId == TicketSeverityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = TicketSeverityObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTableId == TicketSeverityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllTicketSeveritiesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllTicketSeveritiesQuery.Id,QueryCode = AllTicketSeveritiesQuery.UniqueCode, IndexOrder = 1, ObjectFieldId = TicketSeverityObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == TicketSeverityObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = TicketSeverityObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == TicketSeverityObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable TicketSeverityObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "TicketSeverity" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> TicketSeverityObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "TicketSeverity").ToList();
		       
	      

	         Screen TicketSeverityHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "TicketSeverity.HeaderScreen", Name = "Header Screen", ObjectTableId = TicketSeverityObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField TicketSeverityTicketSeverityHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = TicketSeverityObjectFields.Where(d => d.FieldName == "Name").FirstOrDefault().Id, ScreenId = TicketSeverityHeaderScreenScreen0.Id, ObjectFieldCode = TicketSeverityObjectFields.Where(d => d.FieldName == "Name").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    TicketSeverityObjectTable.HeaderScreenId = TicketSeverityHeaderScreenScreen0.Id;
	   		  
	      

	         Screen TicketSeverityGeneralTabScreenScreen1 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "TicketSeverity.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = TicketSeverityObjectTable.Id, NumberOfColumns = 1, NumberOfRows = 4, IsReadOnly = false }, screensRepository, tenantScreens);
      
            ScreenField TicketSeverityTicketSeverityGeneralTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = TicketSeverityObjectFields.Where(d => d.FieldName == "Name").FirstOrDefault().Id, ScreenId = TicketSeverityGeneralTabScreenScreen1.Id, ObjectFieldCode = TicketSeverityObjectFields.Where(d => d.FieldName == "Name").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField TicketSeverityTicketSeverityGeneralTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = TicketSeverityObjectFields.Where(d => d.FieldName == "Inactive").FirstOrDefault().Id, ScreenId = TicketSeverityGeneralTabScreenScreen1.Id, ObjectFieldCode = TicketSeverityObjectFields.Where(d => d.FieldName == "Inactive").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
           

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable TicketSeverityObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "TicketSeverity" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode TicketSeverityGeneralTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TicketSeverity.TH.General", DefaultText = "General",LocalDefaultText = null, ObjectTableId = TicketSeverityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature TicketSeverityGeneralFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TicketSeverity.Tab.General", ObjectTableId = TicketSeverityObjectTable.Id, Tenant = 0, NameTextCodeCode = "TicketSeverity.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode TicketSeverityEventsTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TicketSeverity.TH.Events", DefaultText = "Events",LocalDefaultText = null, ObjectTableId = TicketSeverityObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature TicketSeverityEventsFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TicketSeverity.Tab.Events", ObjectTableId = TicketSeverityObjectTable.Id, Tenant = 0, NameTextCodeCode = "TicketSeverity.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "TSGE",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = TicketSeverityGeneralFeature_TH0.Id,FeatureUniqeCode = TicketSeverityGeneralFeature_TH0.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.GeneralControls.GeneralTabControl", ObjectTableId = TicketSeverityObjectTable.Id, TabNameTextCodeId = TicketSeverityGeneralTextCode_TH0.Id, TabNameTextCodeCode = TicketSeverityGeneralTextCode_TH0.Code, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "TSEV",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = TicketSeverityEventsFeature_TH1.Id,FeatureUniqeCode = TicketSeverityEventsFeature_TH1.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = TicketSeverityObjectTable.Id, TabNameTextCodeId = TicketSeverityEventsTextCode_TH1.Id, TabNameTextCodeCode = TicketSeverityEventsTextCode_TH1.Code, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable TicketSeverityObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "TicketSeverity" && d.Tenant == 0).FirstOrDefault(); 

		   Feature TicketSeverityFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = true, ObjectTableId = TicketSeverityObjectTable.Id, Tenant = 0, NameTextCodeCode = "TicketSeverity.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature TicketSeverityFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = true, ObjectTableId = TicketSeverityObjectTable.Id, Tenant = 0, NameTextCodeCode = "TicketSeverity.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature TicketSeverityFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = true, ObjectTableId = TicketSeverityObjectTable.Id, Tenant = 0, NameTextCodeCode = "TicketSeverity.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature TicketSeverityFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = true, ObjectTableId = TicketSeverityObjectTable.Id, Tenant = 0, NameTextCodeCode = "TicketSeverity.Features.PackageFeature", NameTextCodeDefaultText = "TicketSeverity Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);    
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable TicketSeverityObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "TicketSeverity" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "UPTE",
                ShortView =  false,
                IsManualEntry =  false,
                LocalName =  "Ticket Severity Updated",
                EnglishName =  "Ticket Severity Updated",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = TicketSeverityObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CRTE",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Ticket Severity Created",
                EnglishName =  "Ticket Severity Created",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = TicketSeverityObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "ACTE",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Ticket Severity Activated",
                EnglishName =  "Ticket Severity Activated",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = TicketSeverityObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "IATE",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Ticket Severity Inactivated",
                EnglishName =  "Ticket Severity Inactivated",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = TicketSeverityObjectTable.Id,
				 
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
                ObjectTableId = TicketSeverityObjectTable.Id,
				 
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
                ObjectTableId = TicketSeverityObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
		   FeatureRepository featureRepository = new FeatureRepository(0); 
		   //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList(); 
		   ObjectTable TicketSeverityObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "TicketSeverity" && d.Tenant == 0).FirstOrDefault();  

		   TextCodeRepository.SubmitChanges();
		   FeaturesRepository.SubmitChanges();
		   MenuButtonGroup TicketSeverityMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
				{
					MenuButtonGroupType = "TicketSeverityEdit",
					Name = "TicketSeverityEditButtonsGroup",
					ObjectTableId = TicketSeverityObjectTable.Id,
					Tenant = 0
				}, menuButtonGroupRepository, tenantMenuButtonGroups); 
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {     
	    
}

    

   }
    
}
	 