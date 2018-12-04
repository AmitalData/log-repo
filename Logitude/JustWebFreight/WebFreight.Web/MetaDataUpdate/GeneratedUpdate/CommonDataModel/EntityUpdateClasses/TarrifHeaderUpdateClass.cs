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
   public class TarrifHeaderUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "TarrifHeader",
			      				    DBTableName =  "TarrifHeaders",
			      				    ObjectTableSingular =  "Tariff Header",
			      				    ObjectTablePlural =  "TarrifHeaders",
			      				    DefaultText =  "Tariff Header",
			      				    Name =  "TarrifHeader",
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
			      				    SearchFields =  "TarrifHeader,TarrifHeaders,,Id,",
			      				    IsSaveButtonVisible =  true,
			      				    EnableSecurity =  false,
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
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasMenuButtons =  false,
			      				    HasFiltersMenu =  false,
			      				    AllowedInQueues =  false,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CardId",
					  						ObjectTableName =  "TarrifHeader",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Card",
					  						Code =  "CardId",
					  						MaxLength =  15,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
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
					  						PMPropertyPath =  "CardId",
					  						ListPropertyPath =  "CardId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
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
					  						FullFieldLable =  "Card",
					  						DefaultText =  @"Card",
					  						HelpTextCode =  "Card",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TarrifTypeCode",
					  						ObjectTableName =  "TarrifHeader",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "TarrifType",
					  						Code =  "TarrifTypeCode",
					  						MaxLength =  4,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
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
					  						PMPropertyPath =  "TarrifTypeCode",
					  						ListPropertyPath =  "TarrifTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
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
					  						FullFieldLable =  "TarrifTypeCode",
					  						DefaultText =  @"Tariff Type Code",
					  						HelpTextCode =  "TarrifTypeCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FromDate",
					  						ObjectTableName =  "TarrifHeader",
					  						FieldsDataType =  "DateTime",
					  						Code =  "FromDate",
					  						MaxLength =  0,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
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
					  						PMPropertyPath =  "FromDate",
					  						ListPropertyPath =  "FromDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
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
					  						FullFieldLable =  "FromDate",
					  						DefaultText =  @"From Date",
					  						HelpTextCode =  "FromDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ToDate",
					  						ObjectTableName =  "TarrifHeader",
					  						FieldsDataType =  "DateTime",
					  						Code =  "ToDate",
					  						MaxLength =  0,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
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
					  						PMPropertyPath =  "ToDate",
					  						ListPropertyPath =  "ToDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
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
					  						FullFieldLable =  "ToDate",
					  						DefaultText =  @"To Date",
					  						HelpTextCode =  "ToDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreateDate",
					  						ObjectTableName =  "TarrifHeader",
					  						FieldsDataType =  "DateTime",
					  						Code =  "CreateDate",
					  						MaxLength =  0,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
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
					  						PMPropertyPath =  "CreateDate",
					  						ListPropertyPath =  "CreateDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
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
					  						FullFieldLable =  "CreateDate",
					  						DefaultText =  @"Create Date",
					  						HelpTextCode =  "CreateDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InActive",
					  						ObjectTableName =  "TarrifHeader",
					  						FieldsDataType =  "Boolean",
					  						Code =  "InActive",
					  						MaxLength =  0,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
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
					  						HelpTextCode =  "InActive",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Notes",
					  						ObjectTableName =  "TarrifHeader",
					  						FieldsDataType =  "Text",
					  						Code =  "Notes",
					  						MaxLength =  250,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
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
					  						PMPropertyPath =  "Notes",
					  						ListPropertyPath =  "Notes",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
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
					  						FullFieldLable =  "Notes",
					  						DefaultText =  @"Notes",
					  						HelpTextCode =  "Notes",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TransitTimeNotes",
					  						ObjectTableName =  "TarrifHeader",
					  						FieldsDataType =  "Text",
					  						Code =  "TransitTimeNotes",
					  						MaxLength =  250,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
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
					  						PMPropertyPath =  "TransitTimeNotes",
					  						ListPropertyPath =  "TransitTimeNotes",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
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
					  						FullFieldLable =  "TransitTimeNotes",
					  						DefaultText =  @"Transit Time Notes",
					  						HelpTextCode =  "TransitTimeNotes",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	    {  	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {    

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {      
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable TarrifHeaderObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "TarrifHeader" && d.Tenant == 0).FirstOrDefault(); 
		   Feature TarrifHeaderFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = TarrifHeaderObjectTable.Id, Tenant = 0, NameTextCodeCode = "TarrifHeader.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature TarrifHeaderFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = TarrifHeaderObjectTable.Id, Tenant = 0, NameTextCodeCode = "TarrifHeader.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature TarrifHeaderFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = TarrifHeaderObjectTable.Id, Tenant = 0, NameTextCodeCode = "TarrifHeader.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature TarrifHeaderFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = TarrifHeaderObjectTable.Id, Tenant = 0, NameTextCodeCode = "TarrifHeader.Features.PackageFeature", NameTextCodeDefaultText = "TarrifHeader Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);    
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable TarrifHeaderObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "TarrifHeader" && d.Tenant == 0).FirstOrDefault(); 
	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable TarrifHeaderObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "TarrifHeader" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode TarrifHeaderTextCode_TarrifHeader = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TarrifHeader", DefaultText = "Tariff Header",LocalDefaultText = null, ObjectTableId = TarrifHeaderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "T", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode TarrifHeaderTextCode_TarrifHeaderOCarrier = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TarrifHeader.O.Carrier", DefaultText = "Carrier",LocalDefaultText = @"מוביל", ObjectTableId = TarrifHeaderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode TarrifHeaderTextCode_TarrifHeaderBAddSurchargeTarrif = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TarrifHeader.B.AddSurchargeTarrif", DefaultText = "Add Surcharge Tariff",LocalDefaultText = null, ObjectTableId = TarrifHeaderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode TarrifHeaderTextCode_TarrifHeaderONotes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TarrifHeader.O.Notes", DefaultText = "Notes",LocalDefaultText = null, ObjectTableId = TarrifHeaderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode TarrifHeaderTextCode_TarrifHeaderOFrom = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TarrifHeader.O.From", DefaultText = "From",LocalDefaultText = null, ObjectTableId = TarrifHeaderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode TarrifHeaderTextCode_TarrifHeaderOTo = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TarrifHeader.O.To", DefaultText = "To",LocalDefaultText = null, ObjectTableId = TarrifHeaderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode TarrifHeaderTextCode_TarrifHeaderOActiveDates = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TarrifHeader.O.ActiveDates", DefaultText = "Active Dates",LocalDefaultText = null, ObjectTableId = TarrifHeaderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode TarrifHeaderTextCode_TarrifHeaderONewDates = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TarrifHeader.O.NewDates", DefaultText = "New Dates",LocalDefaultText = null, ObjectTableId = TarrifHeaderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode TarrifHeaderTextCode_TarrifHeaderOMakeInactive = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TarrifHeader.O.MakeInactive", DefaultText = "Make Inactive",LocalDefaultText = null, ObjectTableId = TarrifHeaderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode TarrifHeaderTextCode_TarrifHeaderOAnyware = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TarrifHeader.O.Anyware", DefaultText = "Anywhere",LocalDefaultText = null, ObjectTableId = TarrifHeaderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode TarrifHeaderTextCode_TarrifHeaderOPortsList = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TarrifHeader.O.PortsList", DefaultText = "Ports List",LocalDefaultText = null, ObjectTableId = TarrifHeaderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode TarrifHeaderTextCode_TarrifHeaderOCountriesList = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TarrifHeader.O.CountriesList", DefaultText = "Countries List",LocalDefaultText = null, ObjectTableId = TarrifHeaderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode TarrifHeaderTextCode_TarrifHeaderMFromDateLessThanToDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TarrifHeader.M.FromDateLessThanToDate", DefaultText = "From Date should be less than To Date.",LocalDefaultText = null, ObjectTableId = TarrifHeaderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode TarrifHeaderTextCode_TarrifHeaderMAddAtLeastOneCharge = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TarrifHeader.M.AddAtLeastOneCharge", DefaultText = "You must add at least one Charge.",LocalDefaultText = null, ObjectTableId = TarrifHeaderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode TarrifHeaderTextCode_TarrifHeaderOShowActiveTarrifs = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TarrifHeader.O.ShowActiveTarrifs", DefaultText = "Show Active Tariffs",LocalDefaultText = null, ObjectTableId = TarrifHeaderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode TarrifHeaderTextCode_TarrifHeaderOShowAllTarrifs = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TarrifHeader.O.ShowAllTarrifs", DefaultText = "Show All Tariffs",LocalDefaultText = null, ObjectTableId = TarrifHeaderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode TarrifHeaderTextCode_TarrifHeaderOAddSurchargeTarrif = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TarrifHeader.O.AddSurchargeTarrif", DefaultText = "Add Surcharge Tariff",LocalDefaultText = null, ObjectTableId = TarrifHeaderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode TarrifHeaderTextCode_TarrifHeaderOEditSurchargeTarrif = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TarrifHeader.O.EditSurchargeTarrif", DefaultText = "Edit Surcharge Tariff",LocalDefaultText = null, ObjectTableId = TarrifHeaderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode TarrifHeaderTextCode_TarrifHeaderODate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TarrifHeader.O.Date", DefaultText = "Date",LocalDefaultText = null, ObjectTableId = TarrifHeaderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode TarrifHeaderTextCode_TarrifHeaderOCreateDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TarrifHeader.O.CreateDate", DefaultText = "Create Date",LocalDefaultText = null, ObjectTableId = TarrifHeaderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode TarrifHeaderTextCode_TarrifHeaderOFromLocation = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TarrifHeader.O.FromLocation", DefaultText = "From Location",LocalDefaultText = null, ObjectTableId = TarrifHeaderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode TarrifHeaderTextCode_TarrifHeaderOToLocation = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TarrifHeader.O.ToLocation", DefaultText = "To Location",LocalDefaultText = null, ObjectTableId = TarrifHeaderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode TarrifHeaderTextCode_TarrifHeaderBAddFreightTariff = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TarrifHeader.B.AddFreightTariff", DefaultText = "Add Freight Tariff",LocalDefaultText = @"הוסף תעריף הובלה", ObjectTableId = TarrifHeaderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode TarrifHeaderTextCode_TariffHeaderOTariffs = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TariffHeader.O.Tariffs", DefaultText = "Tariffs",LocalDefaultText = @"מחירונים", ObjectTableId = TarrifHeaderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode TarrifHeaderTextCode_TariffHeaderOTariffCharges = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TariffHeader.O.TariffCharges", DefaultText = "Tariffs Charges",LocalDefaultText = @"תעריפי חיובים", ObjectTableId = TarrifHeaderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode TarrifHeaderTextCode_TariffHeaderOShowShipmentCarrierTariffs = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TariffHeader.O.ShowShipmentCarrierTariffs", DefaultText = "Show Shipment Carrier Tariffs",LocalDefaultText = @"הצג מחירוני הובלה ", ObjectTableId = TarrifHeaderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode TarrifHeaderTextCode_TariffHeaderOShowQuoteCarrierTariffs = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TariffHeader.O.ShowQuoteCarrierTariffs", DefaultText = "Show Quote Carrier Tariffs",LocalDefaultText = @"הצג רשומות מחירוני מוביל ", ObjectTableId = TarrifHeaderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode TarrifHeaderTextCode_TariffHeaderOShowAllCarrierTariffs = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TariffHeader.O.ShowAllCarrierTariffs", DefaultText = "Show All Carrier Tariffs",LocalDefaultText = @"הצג את כל מחירוני ההובלה", ObjectTableId = TarrifHeaderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 