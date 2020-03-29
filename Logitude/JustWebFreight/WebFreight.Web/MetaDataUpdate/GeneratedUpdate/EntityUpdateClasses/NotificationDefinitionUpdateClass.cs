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
   public class NotificationDefinitionUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "Customs.NotificationDefinition",
			      				    DBTableName =  "Customs.NotificationDefinitions",
			      				    ObjectTableSingular =  "NotificationDefinition",
			      				    ObjectTablePlural =  "NotificationDefinitions",
			      				    HasCustomFilter =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    IsEditable =  false,
			      				    IsNewWizard =  false,
			      				    LookUp1 =  "Code",
			      				    LookUp2 =  "LocalName",
			      				    KeyPropertyPath =  "Code",
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
			      				    SortingByObjectField =  "Code",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "BR",
			      				    MaxNumberOfCustomFields =  0,
			      				    LocalDefaultText =  "הגדרת הודעה",
			      				    DefaultText =  "Notification Definition",
			      				    Code =  "NDQG",
			      				    Name =  "Customs.NotificationDefinition",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Customs",
			      				    ServerModuleName =  "Customs",
			      				    HasMenuButtons =  false,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Code",
					  						ObjectTableName =  "Customs.NotificationDefinition",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  5,
					  						IsRequired =  true,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  5,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Code",
					  						ListPropertyPath =  "Code",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "Customs.NotificationDefinition",
					  						DisplayInEntityVariables =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Code",
					  						DefaultText =  "Code",
					  						FullLocalDefaultText =  "קוד התראה",
					  						ListFieldLable =  "CodeListLable",
					  						ListLableDefaultText =  "Code",
					  						ListLocalDefaultText =  "קוד התראה",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "EnglishName",
					  						ObjectTableName =  "Customs.NotificationDefinition",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  40,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "EnglishName",
					  						ListPropertyPath =  "EnglishName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "Customs.NotificationDefinition",
					  						DisplayInEntityVariables =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "EnglishName",
					  						DefaultText =  "English Name",
					  						FullLocalDefaultText =  "שם באנגלית",
					  						ListFieldLable =  "EnglishNameListLable",
					  						ListLableDefaultText =  "English Name",
					  						ListLocalDefaultText =  "שם באנגלית",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LocalName",
					  						ObjectTableName =  "Customs.NotificationDefinition",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  40,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "LocalName",
					  						ListPropertyPath =  "LocalName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "Customs.NotificationDefinition",
					  						DisplayInEntityVariables =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LocalName",
					  						DefaultText =  "Local Name",
					  						FullLocalDefaultText =  "שם עברית",
					  						ListFieldLable =  "LocalNameListLable",
					  						ListLableDefaultText =  "Local Name",
					  						ListLocalDefaultText =  "שם עברית",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AssigneeNotificationTypeCode",
					  						ObjectTableName =  "Customs.NotificationDefinition",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.AssigneeNotificationType",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  1,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AssigneeNotificationTypeCode",
					  						ListPropertyPath =  "AssigneeNotificationTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "Customs.NotificationDefinition",
					  						DisplayInEntityVariables =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AssigneeNotificationTypeCode",
					  						DefaultText =  "Assignee Notification Type",
					  						FullLocalDefaultText =  "סוג התראה",
					  						ListFieldLable =  "AssigneeNotificationTypeCodeListLable",
					  						ListLableDefaultText =  "Assignee Notification Type",
					  						ListLocalDefaultText =  "סוג התראה",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AssigneeNotificationTypeName",
					  						ObjectTableName =  "Customs.NotificationDefinition",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  40,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AssigneeNotificationTypeName",
					  						ListPropertyPath =  "AssigneeNotificationTypeName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "Customs.NotificationDefinition",
					  						DisplayInEntityVariables =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AssigneeNotificationTypeName",
					  						DefaultText =  "Assignee Notification Type",
					  						FullLocalDefaultText =  "סוג הודעה",
					  						ListFieldLable =  "AssigneeNotificationTypeNameListLable",
					  						ListLableDefaultText =  "Assignee Notification Type",
					  						ListLocalDefaultText =  "סוג הודעה",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DefaultAssigneeName",
					  						ObjectTableName =  "Customs.NotificationDefinition",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  40,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DefaultAssigneeName",
					  						ListPropertyPath =  "DefaultAssigneeName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "Customs.NotificationDefinition",
					  						DisplayInEntityVariables =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DefaultAssigneeName",
					  						DefaultText =  "Default Assignee Name",
					  						FullLocalDefaultText =  "אחראי ברירת מחדל",
					  						ListFieldLable =  "DefaultAssigneeNameListLable",
					  						ListLableDefaultText =  "Default Assignee Name",
					  						ListLocalDefaultText =  "אחראי ברירת מחדל",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DefaultAssigneeId",
					  						ObjectTableName =  "Customs.NotificationDefinition",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DefaultAssigneeId",
					  						ListPropertyPath =  "DefaultAssigneeId",
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
					  						FullFieldLable =  "DefaultAssigneeId",
					  						DefaultText =  "Default Assignee",
					  						FullLocalDefaultText =  "אחראי ברירת מחדל",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SearchFields",
					  						ObjectTableName =  "Customs.NotificationDefinition",
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
					  						ValidForQuerySection1 =  "Customs.NotificationDefinition",
					  						DisplayInEntityVariables =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SearchFields",
					  						DefaultText =  "Search ...",
					  						FullLocalDefaultText =  "חפש לפי",
					  						ListFieldLable =  "SearchFieldsListLable",
					  						ListLableDefaultText =  "Search by ...",
					  						ListLocalDefaultText =  "חפש לפי",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Inactive",
					  						ObjectTableName =  "Customs.NotificationDefinition",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
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
					  						PMPropertyPath =  "Inactive",
					  						ListPropertyPath =  "Inactive",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "Customs.NotificationDefinition",
					  						DisplayInEntityVariables =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Inactive",
					  						DefaultText =  "Inactive ",
					  						FullLocalDefaultText =  "לא פּעיל",
					  						ListFieldLable =  "InactiveListLable",
					  						ListLableDefaultText =  "Inactive",
					  						ListLocalDefaultText =  "לא פּעיל",
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
	        QueryGroup NotificationDefinitionQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "NDQG", Name = "Customs.NotificationDefinition" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable NotificationDefinitionObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Customs.NotificationDefinition" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> NotificationDefinitionObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Customs.NotificationDefinition").ToList();   

			   TextCode NotificationDefinitionTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.NotificationDefinition.Q.NotificationDefinitionQuery", DefaultText = @"Notification Definition",LocalDefaultText = "הגדרת הודעה", ObjectTableId = NotificationDefinitionObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature NotificationDefinitionFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NotificationDefinition.Q.NotificationDefinition", ObjectTableId = NotificationDefinitionObjectTable.Id, Tenant = 0, NameTextCodeCode = "NotificationDefinitionFeatures.NotificationDefinition", NameTextCodeDefaultText = "NotificationDefinition", FeatureTypeCode = "QUER", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query NotificationDefinitionQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = NotificationDefinitionTextCode_0.Id, NameTextCodeCode = NotificationDefinitionTextCode_0.Code, ObjectTableName = "Customs.NotificationDefinition", Code = "NotificationDefinition",  QueryGroupCode = "NDQG", IndexOrder = 0, Tenant = 0, ObjectTableId = NotificationDefinitionObjectTable.Id, QuerySection = "Customs.NotificationDefinition", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = NotificationDefinitionFeature_0.Id,FeatureUniqeCode= NotificationDefinitionFeature_0.FeatureUniqeCode, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn NotificationDefinitionQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotificationDefinitionQuery.Id,QueryCode = NotificationDefinitionQuery.UniqueCode, IndexOrder = 0, ObjectFieldId = NotificationDefinitionObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == NotificationDefinitionObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = NotificationDefinitionObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == NotificationDefinitionObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn NotificationDefinitionQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotificationDefinitionQuery.Id,QueryCode = NotificationDefinitionQuery.UniqueCode, IndexOrder = 1, ObjectFieldId = NotificationDefinitionObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == NotificationDefinitionObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = NotificationDefinitionObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == NotificationDefinitionObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn NotificationDefinitionQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotificationDefinitionQuery.Id,QueryCode = NotificationDefinitionQuery.UniqueCode, IndexOrder = 2, ObjectFieldId = NotificationDefinitionObjectFields.Where(d => d.FieldName == "AssigneeNotificationTypeName" && d.ObjectTableId == NotificationDefinitionObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = NotificationDefinitionObjectFields.Where(d => d.FieldName == "AssigneeNotificationTypeName" && d.ObjectTableId == NotificationDefinitionObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn NotificationDefinitionQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotificationDefinitionQuery.Id,QueryCode = NotificationDefinitionQuery.UniqueCode, IndexOrder = 3, ObjectFieldId = NotificationDefinitionObjectFields.Where(d => d.FieldName == "DefaultAssigneeName" && d.ObjectTableId == NotificationDefinitionObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = NotificationDefinitionObjectFields.Where(d => d.FieldName == "DefaultAssigneeName" && d.ObjectTableId == NotificationDefinitionObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable NotificationDefinitionObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.NotificationDefinition" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> NotificationDefinitionObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Customs.NotificationDefinition").ToList();
		       
	      

	         Screen NotificationDefinitionGeneralTabScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "NotificationDefinition.GeneralTabScreen", Name = "GeneralTabScreen", ObjectTableId = NotificationDefinitionObjectTable.Id, NumberOfColumns = 1, NumberOfRows = 4, IsReadOnly = false }, screensRepository, tenantScreens);
      
            ScreenField NotificationDefinitionNotificationDefinitionGeneralTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = NotificationDefinitionObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().Id, ScreenId = NotificationDefinitionGeneralTabScreenScreen0.Id,ScreenCode = NotificationDefinitionGeneralTabScreenScreen0.Code, ObjectFieldCode = NotificationDefinitionObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField NotificationDefinitionNotificationDefinitionGeneralTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = NotificationDefinitionObjectFields.Where(d => d.FieldName == "LocalName").FirstOrDefault().Id, ScreenId = NotificationDefinitionGeneralTabScreenScreen0.Id,ScreenCode = NotificationDefinitionGeneralTabScreenScreen0.Code, ObjectFieldCode = NotificationDefinitionObjectFields.Where(d => d.FieldName == "LocalName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField NotificationDefinitionNotificationDefinitionGeneralTabScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ObjectFieldId = NotificationDefinitionObjectFields.Where(d => d.FieldName == "AssigneeNotificationTypeCode").FirstOrDefault().Id, ScreenId = NotificationDefinitionGeneralTabScreenScreen0.Id,ScreenCode = NotificationDefinitionGeneralTabScreenScreen0.Code, ObjectFieldCode = NotificationDefinitionObjectFields.Where(d => d.FieldName == "AssigneeNotificationTypeCode").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField NotificationDefinitionNotificationDefinitionGeneralTabScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 3, ObjectFieldId = NotificationDefinitionObjectFields.Where(d => d.FieldName == "DefaultAssigneeId").FirstOrDefault().Id, ScreenId = NotificationDefinitionGeneralTabScreenScreen0.Id,ScreenCode = NotificationDefinitionGeneralTabScreenScreen0.Code, ObjectFieldCode = NotificationDefinitionObjectFields.Where(d => d.FieldName == "DefaultAssigneeId").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
           
	      

	         Screen NotificationDefinitionHeaderScreenScreen1 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Customs.NotificationDefinition.HeaderScreen", Name = "Header Screen", ObjectTableId = NotificationDefinitionObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField NotificationDefinitionCustomsNotificationDefinitionHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = NotificationDefinitionObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().Id, ScreenId = NotificationDefinitionHeaderScreenScreen1.Id,ScreenCode = NotificationDefinitionHeaderScreenScreen1.Code, ObjectFieldCode = NotificationDefinitionObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField NotificationDefinitionCustomsNotificationDefinitionHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = NotificationDefinitionObjectFields.Where(d => d.FieldName == "LocalName").FirstOrDefault().Id, ScreenId = NotificationDefinitionHeaderScreenScreen1.Id,ScreenCode = NotificationDefinitionHeaderScreenScreen1.Code, ObjectFieldCode = NotificationDefinitionObjectFields.Where(d => d.FieldName == "LocalName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    NotificationDefinitionObjectTable.HeaderScreenId = NotificationDefinitionHeaderScreenScreen1.Id;
		    NotificationDefinitionObjectTable.HeaderScreenCode = NotificationDefinitionHeaderScreenScreen1.Code;

	   		  
	      

	         Screen NotificationDefinitionGeneralTabScreenScreen2 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Customs.NotificationDefinition.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = NotificationDefinitionObjectTable.Id, NumberOfColumns = 1, NumberOfRows = 4, IsReadOnly = false }, screensRepository, tenantScreens);
      
            ScreenField NotificationDefinitionCustomsNotificationDefinitionGeneralTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = NotificationDefinitionObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().Id, ScreenId = NotificationDefinitionGeneralTabScreenScreen2.Id,ScreenCode = NotificationDefinitionGeneralTabScreenScreen2.Code, ObjectFieldCode = NotificationDefinitionObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField NotificationDefinitionCustomsNotificationDefinitionGeneralTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = NotificationDefinitionObjectFields.Where(d => d.FieldName == "LocalName").FirstOrDefault().Id, ScreenId = NotificationDefinitionGeneralTabScreenScreen2.Id,ScreenCode = NotificationDefinitionGeneralTabScreenScreen2.Code, ObjectFieldCode = NotificationDefinitionObjectFields.Where(d => d.FieldName == "LocalName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField NotificationDefinitionCustomsNotificationDefinitionGeneralTabScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ObjectFieldId = NotificationDefinitionObjectFields.Where(d => d.FieldName == "AssigneeNotificationTypeCode").FirstOrDefault().Id, ScreenId = NotificationDefinitionGeneralTabScreenScreen2.Id,ScreenCode = NotificationDefinitionGeneralTabScreenScreen2.Code, ObjectFieldCode = NotificationDefinitionObjectFields.Where(d => d.FieldName == "AssigneeNotificationTypeCode").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField NotificationDefinitionCustomsNotificationDefinitionGeneralTabScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 3, ObjectFieldId = NotificationDefinitionObjectFields.Where(d => d.FieldName == "DefaultAssigneeId").FirstOrDefault().Id, ScreenId = NotificationDefinitionGeneralTabScreenScreen2.Id,ScreenCode = NotificationDefinitionGeneralTabScreenScreen2.Code, ObjectFieldCode = NotificationDefinitionObjectFields.Where(d => d.FieldName == "DefaultAssigneeId").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
           

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable NotificationDefinitionObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.NotificationDefinition" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode NotificationDefinitionGeneralTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.NotificationDefinition.TH.General", DefaultText = "General",LocalDefaultText = "כללי", ObjectTableId = NotificationDefinitionObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature NotificationDefinitionGeneralFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NotificationDefinition.Tab.General", ObjectTableId = NotificationDefinitionObjectTable.Id, Tenant = 0, NameTextCodeCode = "NotificationDefinition.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   Feature NotificationDefinitionGeneralFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GENERAL", ObjectTableId = NotificationDefinitionObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.NotificationDefinition.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode NotificationDefinitionEventsTextCode_TH2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.NotificationDefinition.TH.Events", DefaultText = "Events",LocalDefaultText = "אירועים", ObjectTableId = NotificationDefinitionObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature NotificationDefinitionEventsFeature_TH2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EVENTS", ObjectTableId = NotificationDefinitionObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.NotificationDefinition.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "NDGT",HtmlComponentName = "GeneralTabComponent",HtmlComponentUrl = "./Infrastructure/GenericComponents/GeneralTabComponent", FeatureId = NotificationDefinitionGeneralFeature_TH0.Id,FeatureUniqeCode = NotificationDefinitionGeneralFeature_TH0.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.GeneralControls.GeneralTabControl", ObjectTableId = NotificationDefinitionObjectTable.Id, TabNameTextCodeId = NotificationDefinitionGeneralTextCode_TH0.Id, TabNameTextCodeCode = NotificationDefinitionGeneralTextCode_TH0.Code, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "NDGN",HtmlComponentName = "GeneralTabComponent",HtmlComponentUrl = "./Infrastructure/GenericComponents/GeneralTabComponent", FeatureId = NotificationDefinitionGeneralFeature_TH1.Id,FeatureUniqeCode = NotificationDefinitionGeneralFeature_TH1.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.GeneralControls.GeneralTabControl", ObjectTableId = NotificationDefinitionObjectTable.Id, TabNameTextCodeId = NotificationDefinitionGeneralTextCode_TH0.Id, TabNameTextCodeCode = NotificationDefinitionGeneralTextCode_TH0.Code, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "NDEV",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = NotificationDefinitionEventsFeature_TH2.Id,FeatureUniqeCode = NotificationDefinitionEventsFeature_TH2.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = NotificationDefinitionObjectTable.Id, TabNameTextCodeId = NotificationDefinitionEventsTextCode_TH2.Id, TabNameTextCodeCode = NotificationDefinitionEventsTextCode_TH2.Code, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable NotificationDefinitionObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.NotificationDefinition" && d.Tenant == 0).FirstOrDefault(); 

		   Feature NotificationDefinitionFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = true, ObjectTableId = NotificationDefinitionObjectTable.Id, Tenant = 0, NameTextCodeCode = "NotificationDefinition.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature NotificationDefinitionFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = true, ObjectTableId = NotificationDefinitionObjectTable.Id, Tenant = 0, NameTextCodeCode = "NotificationDefinition.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature NotificationDefinitionFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = true, ObjectTableId = NotificationDefinitionObjectTable.Id, Tenant = 0, NameTextCodeCode = "NotificationDefinition.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature NotificationDefinitionFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = true, ObjectTableId = NotificationDefinitionObjectTable.Id, Tenant = 0, NameTextCodeCode = "NotificationDefinition.Features.PackageFeature", NameTextCodeDefaultText = "NotificationDefinition Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);    
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable NotificationDefinitionObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.NotificationDefinition" && d.Tenant == 0).FirstOrDefault(); 
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
                ObjectTableId = NotificationDefinitionObjectTable.Id,
				 
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
                ObjectTableId = NotificationDefinitionObjectTable.Id,
				 
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
	 