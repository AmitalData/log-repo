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

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.InfrastructureModel.EntityUpdateClasses
{
   public class FollowUpUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "FollowUp",
			      				    DBTableName =  "FollowUps",
			      				    ObjectTableSingular =  "Follow-Up",
			      				    ObjectTablePlural =  "FollowUps",
			      				    DefaultText =  "Follow-Up",
			      				    Name =  "FollowUp",
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
			      				    IsMain =  false,
			      				    ClientModuleName =  "Infrastructure",
			      				    IsAutoComplete =  false,
			      				    CustomFieldsCount =  0,
			      				    HasCustomFields =  false,
			      				    InActive =  false,
			      				    SearchFields =  "FollowUp,FollowUps,,Id,",
			      				    IsSaveButtonVisible =  true,
			      				    EnableSecurity =  true,
			      				    ObjectTableTypeCode =  "MD",
			      				    IsComposition =  true,
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
			      				    IsTabsHidden =  false,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Notes",
					  						ObjectTableName =  "FollowUp",
					  						FieldsDataType =  "nText",
					  						FieldCode =  "FollowUp.Notes",
					  						Code =  "Notes",
					  						MaxLength =  1000,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
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
					  						DisplayOnLookUpLocal =  false,
					  						FullNameTextCodeCode =  "FollowUp.F.Notes",
					  						HelpTextCodeCode =  "FollowUp.NotesHelpText",
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						EnableFullscreenTextBox =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "Notes",
					  						DefaultText =  "Notes",
					  						HelpTextCode =  "Notes",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "EventTypeId",
					  						ObjectTableName =  "FollowUp",
					  						FieldsDataType =  "Text",
					  						FieldCode =  "FollowUp.EventTypeId",
					  						Code =  "EventTypeId",
					  						MaxLength =  15,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  true,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "EventTypeId",
					  						ListPropertyPath =  "EventTypeId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  true,
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
					  						DisplayOnLookUpLocal =  false,
					  						FullNameTextCodeCode =  "FollowUp.F.EventTypeId",
					  						HelpTextCodeCode =  "FollowUp.EventTypeIdHelpText",
					  						ListTextCodeCode =  "FollowUp.CH.EventTypeIdListLable",
					  						ShortNameTextCodeCode =  "FollowUp.F.EventTypeId.Short",
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						EnableFullscreenTextBox =  false,
					  						IsRequired =  true,
					  						FullFieldLable =  "EventTypeId",
					  						DefaultText =  "Event Type ID",
					  						ListFieldLable =  "EventTypeIdListLable",
					  						ListLableDefaultText =  "Event Type ID",
					  						HelpTextCode =  "EventTypeId",
					  						ShortFieldLable =  "EventTypeId",
					  						ShortFieldLableDefaultText =  "Type",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "OwnerUserId",
					  						ObjectTableName =  "FollowUp",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
					  						FieldCode =  "FollowUp.OwnerUserId",
					  						Code =  "OwnerUserId",
					  						MaxLength =  15,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  true,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "OwnerUserId",
					  						ListPropertyPath =  "OwnerUserId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  true,
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
					  						DisplayOnLookUpLocal =  false,
					  						FullNameTextCodeCode =  "FollowUp.F.OwnerUserId",
					  						HelpTextCodeCode =  "FollowUp.OwnerUserIdHelpText",
					  						ListTextCodeCode =  "FollowUp.CH.OwnerUserIdListLable",
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						EnableFullscreenTextBox =  false,
					  						IsRequired =  true,
					  						FullFieldLable =  "OwnerUserId",
					  						DefaultText =  "Owner",
					  						ListFieldLable =  "OwnerUserIdListLable",
					  						ListLableDefaultText =  "Owner User ID",
					  						HelpTextCode =  "OwnerUserId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Date",
					  						ObjectTableName =  "FollowUp",
					  						FieldsDataType =  "DateTime",
					  						FieldCode =  "FollowUp.Date",
					  						Code =  "Date",
					  						MaxLength =  0,
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
					  						PMPropertyPath =  "Date",
					  						ListPropertyPath =  "Date",
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
					  						DisplayOnLookUpLocal =  false,
					  						FullNameTextCodeCode =  "FollowUp.F.Date",
					  						HelpTextCodeCode =  "FollowUp.DateHelpText",
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						EnableFullscreenTextBox =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "Date",
					  						DefaultText =  "Date",
					  						HelpTextCode =  "Date",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DoneNote",
					  						ObjectTableName =  "FollowUp",
					  						FieldsDataType =  "nText",
					  						FieldCode =  "FollowUp.DoneNote",
					  						Code =  "DoneNote",
					  						MaxLength =  250,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "DoneNote",
					  						ListPropertyPath =  "DoneNote",
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
					  						DisplayOnLookUpLocal =  false,
					  						FullNameTextCodeCode =  "FollowUp.F.DoneNote",
					  						HelpTextCodeCode =  "FollowUp.DoneNoteHelpText",
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						EnableFullscreenTextBox =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "DoneNote",
					  						DefaultText =  "Notes",
					  						HelpTextCode =  "DoneNote",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DoneDateTime",
					  						ObjectTableName =  "FollowUp",
					  						FieldsDataType =  "DateTime",
					  						FieldCode =  "FollowUp.DoneDateTime",
					  						Code =  "DoneDateTime",
					  						MaxLength =  0,
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
					  						PMPropertyPath =  "DoneDateTime",
					  						ListPropertyPath =  "DoneDateTime",
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
					  						DisplayOnLookUpLocal =  false,
					  						FullNameTextCodeCode =  "FollowUp.F.DoneDateTime",
					  						HelpTextCodeCode =  "FollowUp.DoneDateTimeHelpText",
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						EnableFullscreenTextBox =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "DoneDateTime",
					  						DefaultText =  "Date",
					  						HelpTextCode =  "DoneDateTime",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Done",
					  						ObjectTableName =  "FollowUp",
					  						FieldsDataType =  "Boolean",
					  						FieldCode =  "FollowUp.Done",
					  						Code =  "Done",
					  						MaxLength =  0,
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
					  						PMPropertyPath =  "Done",
					  						ListPropertyPath =  "Done",
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
					  						DisplayOnLookUpLocal =  false,
					  						FullNameTextCodeCode =  "FollowUp.F.Done",
					  						HelpTextCodeCode =  "FollowUp.DoneHelpText",
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						EnableFullscreenTextBox =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "Done",
					  						DefaultText =  "Mark As Done",
					  						HelpTextCode =  "Done",
					  		
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
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable FollowUpObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "FollowUp" && d.Tenant == 0).FirstOrDefault(); 
	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable FollowUpObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "FollowUp" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode FollowUpTextCode_FollowUpMDateMustBeWithinYears = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "FollowUp.M.DateMustBeWithinYears", DefaultText = "Selected date must be within the last 100 years or the next 100 years",LocalDefaultText = null, ObjectTableId = FollowUpObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode FollowUpTextCode_FollowUpMMarkAsUnDone = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "FollowUp.M.MarkAsUnDone", DefaultText = "Mark As Undone",LocalDefaultText = null, ObjectTableId = FollowUpObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode FollowUpTextCode_FollowUpONewFollowUps = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "FollowUp.O.NewFollowUps", DefaultText = "New Follow Up",LocalDefaultText = null, ObjectTableId = FollowUpObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode FollowUpTextCode_FollowUpMAddFollowUp = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "FollowUp.M.AddFollowUp", DefaultText = "Add Follow Up",LocalDefaultText = null, ObjectTableId = FollowUpObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode FollowUpTextCode_FollowUpMEditFollowUp = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "FollowUp.M.EditFollowUp", DefaultText = "Edit Follow Up",LocalDefaultText = null, ObjectTableId = FollowUpObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode FollowUpTextCode_FollowUpMDeleteFollowUp = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "FollowUp.M.DeleteFollowUp", DefaultText = "Delete Follow Up",LocalDefaultText = null, ObjectTableId = FollowUpObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode FollowUpTextCode_FollowUpMNewFollowUp = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "FollowUp.M.NewFollowUp", DefaultText = "New Follow Up",LocalDefaultText = null, ObjectTableId = FollowUpObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode FollowUpTextCode_FollowUpMMarkAsDone = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "FollowUp.M.MarkAsDone", DefaultText = "Mark As Done",LocalDefaultText = null, ObjectTableId = FollowUpObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode FollowUpTextCode_FollowUpOAddFollowUp = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "FollowUp.O.AddFollowUp", DefaultText = "Add Follow Up",LocalDefaultText = null, ObjectTableId = FollowUpObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode FollowUpTextCode_FollowUpOEditFollowUp = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "FollowUp.O.EditFollowUp", DefaultText = "Edit Follow Up",LocalDefaultText = null, ObjectTableId = FollowUpObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode FollowUpTextCode_FollowUpOSelectFollowUpType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "FollowUp.O.SelectFollowUpType", DefaultText = "Select Follow Up Type",LocalDefaultText = null, ObjectTableId = FollowUpObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode FollowUpTextCode_FollowUpOFollowUpDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "FollowUp.O.FollowUpDetails", DefaultText = "Follow Up Details",LocalDefaultText = null, ObjectTableId = FollowUpObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode FollowUpTextCode_FollowUpODoneDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "FollowUp.O.DoneDetails", DefaultText = "Done Details",LocalDefaultText = null, ObjectTableId = FollowUpObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 