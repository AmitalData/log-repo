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
using Logitude.CargoTracking.Data.Repositories;
using Logitude.CargoTracking.BL;
using Logitude.CargoTracking.Data.EntityPOCOs;
using Logitude.Workflow.Data.Repositories;
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.BL.CLoseTable;
using Logitude.DashboardModule.Data.Repositories;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.BL;




namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.EntityUpdateClasses
{
   public class ExternalReconciliationUpdateClass
   {  		
		public const string HashString = "2d656be76c27ee44b0dad66826bea176";
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "ExternalReconciliation",
			      				    IsNew =  false,
			      				    DBTableName =  "ExternalReconciliations",
			      				    ObjectTableSingular =  "External Reconciliation",
			      				    ObjectTablePlural =  "External Reconciliations",
			      				    HasCustomFilter =  false,
			      				    HasCustomFields =  false,
			      				    AvailableInCustomization =  true,
			      				    SupportSubEntity =  false,
			      				    ApplyGenericCustomFields =  false,
			      				    AvailableInDocumentTypes =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  true,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  false,
			      				    IsNewWizard =  false,
			      				    LookUp1 =  "ReconciliationNumber",
			      				    LookUp2 =  "CreateDate",
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
			      				    SortingByObjectField =  "ReconciliationNumber",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "BR",
			      				    MaxNumberOfCustomFields =  0,
			      				    LocalDefaultText =  "התאמה",
			      				    DefaultText =  "External Reconciliation",
			      				    Code =  "8f4b",
			      				    Name =  "ExternalReconciliation Query Group",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Accounting",
			      				    NoTS =  false,
			      				    HasMenuButtons =  true,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    IsTabsHidden =  false,
			      				    HashString =  ExternalReconciliationUpdateClass.HashString,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository, List<ObjectField> addedFields, List<TextCode> addedTextCodes)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Id",
					  						ObjectTableName =  "ExternalReconciliation",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Id",
					  						ListPropertyPath =  "Id",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ExternalReconciliation",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Id",
					  						DefaultText =  "Id",
					  						FullLocalDefaultText =  "מזהה",
					  						ListFieldLable =  "IdListLable",
					  						ListLableDefaultText =  "Id",
					  						ListLocalDefaultText =  "מזהה",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  true,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Tenant",
					  						ObjectTableName =  "ExternalReconciliation",
					  						FieldsDataType =  "Integer",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Tenant",
					  						ListPropertyPath =  "Tenant",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ExternalReconciliation",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Tenant",
					  						DefaultText =  "Tenant",
					  						FullLocalDefaultText =  "tenant",
					  						ListFieldLable =  "TenantListLable",
					  						ListLableDefaultText =  "Tenant",
					  						ListLocalDefaultText =  "tenant",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  true,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "GLAccountId",
					  						ObjectTableName =  "ExternalReconciliation",
					  						FieldsDataType =  "LookUp",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "GLAccountId",
					  						ListPropertyPath =  "GLAccountId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ExternalReconciliation",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "GLAccountId",
					  						DefaultText =  "Account",
					  						FullLocalDefaultText =  "מספר חשבון",
					  						ListFieldLable =  "GLAccountIdListLable",
					  						ListLableDefaultText =  "Account",
					  						ListLocalDefaultText =  "מספר חשבון",
					  						IsForeignKey =  true,
					  						ForeignEntity =  "GLAccount",
					  						NavigationPropertyName =  "Account",
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ReconciliationNumber",
					  						ObjectTableName =  "ExternalReconciliation",
					  						FieldsDataType =  "Integer",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  true,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  true,
					  						PMPropertyPath =  "ReconciliationNumber",
					  						ListPropertyPath =  "ReconciliationNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ExternalReconciliation",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ReconciliationNumber",
					  						DefaultText =  "Reconciliation No.",
					  						FullLocalDefaultText =  "מספר התאמה",
					  						ListFieldLable =  "ReconciliationNumberListLable",
					  						ListLableDefaultText =  "Reconciliation No.",
					  						ListLocalDefaultText =  "מספר התאמה",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreateDate",
					  						ObjectTableName =  "ExternalReconciliation",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  true,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						Operator =  "Between",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  true,
					  						PMPropertyPath =  "CreateDate",
					  						ListPropertyPath =  "CreateDate",
					  						DisplayInLookUpIndex =  1,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  1,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ExternalReconciliation",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CreateDate",
					  						DefaultText =  "Create Date",
					  						FullLocalDefaultText =  "תאריך פתיחה",
					  						ListFieldLable =  "CreateDateListLable",
					  						ListLableDefaultText =  "Create Date",
					  						ListLocalDefaultText =  "תאריך פתיחה",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreatedByUserId",
					  						ObjectTableName =  "ExternalReconciliation",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CreatedByUserId",
					  						ListPropertyPath =  "CreatedByUserId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ExternalReconciliation",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CreatedByUserId",
					  						DefaultText =  "Created by User",
					  						FullLocalDefaultText =  "נוצר על ידי משתמש",
					  						ListFieldLable =  "CreatedByUserIdListLable",
					  						ListLableDefaultText =  "Created by User",
					  						ListLocalDefaultText =  "נוצר על ידי משתמש",
					  						IsForeignKey =  true,
					  						ForeignEntity =  "User",
					  						NavigationPropertyName =  "CreatedByUser",
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SearchFields",
					  						ObjectTableName =  "ExternalReconciliation",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  1000,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  1000,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
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
					  						ValidForQuerySection1 =  "ExternalReconciliation",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SearchFields",
					  						DefaultText =  "Reconciliation No.",
					  						FullLocalDefaultText =  "שדות חיפוש",
					  						ListFieldLable =  "SearchFieldsListLable",
					  						ListLableDefaultText =  "Reconciliation No.",
					  						ListLocalDefaultText =  "שדות חיפוש",
					  						IsForeignKey =  false,
					  						IsMaxLength =  true,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ExternalReconciliationLines",
					  						ObjectTableName =  "ExternalReconciliation",
					  						FieldsDataType =  "List",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ExternalReconciliationLines",
					  						ListPropertyPath =  "ExternalReconciliationLines",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  true,
					  						MultiTableName =  "ExternalReconciliationLine",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ExternalReconciliationLines",
					  						DefaultText =  "Reconciliation Lines",
					  						ListFieldLable =  "ExternalReconciliationLinesListLable",
					  						ListLableDefaultText =  "Reconciliation Lines",
					  						IsForeignKey =  false,
					  						ThisKey =  "Id",
					  						OtherKey =  "ReconciliationId",
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AccountNumber",
					  						ObjectTableName =  "ExternalReconciliation",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AccountNumber",
					  						ListPropertyPath =  "AccountNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ExternalReconciliation",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AccountNumber",
					  						DefaultText =  "Account Number",
					  						FullLocalDefaultText =  "מספר חשבון",
					  						ListFieldLable =  "AccountNumberListLable",
					  						ListLableDefaultText =  "Account Number",
					  						ListLocalDefaultText =  "מספר חשבון",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AccountName",
					  						ObjectTableName =  "ExternalReconciliation",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  100,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AccountName",
					  						ListPropertyPath =  "AccountName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ExternalReconciliation",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AccountName",
					  						DefaultText =  "Account",
					  						FullLocalDefaultText =  "חשבון",
					  						ListFieldLable =  "AccountNameListLable",
					  						ListLableDefaultText =  "Account",
					  						ListLocalDefaultText =  "חשבון",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreatedByUserName",
					  						ObjectTableName =  "ExternalReconciliation",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  200,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  200,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CreatedByUserName",
					  						ListPropertyPath =  "CreatedByUserName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ExternalReconciliation",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CreatedByUserName",
					  						DefaultText =  "Created By",
					  						FullLocalDefaultText =  "נוצר על ידי",
					  						ListFieldLable =  "CreatedByUserNameListLable",
					  						ListLableDefaultText =  "Created By",
					  						ListLocalDefaultText =  "נוצר על ידי",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AccountCurrencyId",
					  						ObjectTableName =  "ExternalReconciliation",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AccountCurrencyId",
					  						ListPropertyPath =  "AccountCurrencyId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AccountCurrencyId",
					  						DefaultText =  "GL Account Currency",
					  						ListFieldLable =  "AccountCurrencyIdListLable",
					  						ListLableDefaultText =  "GL Account Currency",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsCancelled",
					  						ObjectTableName =  "ExternalReconciliation",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsCancelled",
					  						ListPropertyPath =  "IsCancelled",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ExternalReconciliation",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsCancelled",
					  						DefaultText =  "Is cancelled",
					  						FullLocalDefaultText =  "מבוטלת",
					  						ListFieldLable =  "IsCancelledListLable",
					  						ListLableDefaultText =  "Is cancelled",
					  						ListLocalDefaultText =  "מבוטלת",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BankAccountId",
					  						ObjectTableName =  "ExternalReconciliation",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "BankAccountId",
					  						ListPropertyPath =  "BankAccountId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "BankAccountId",
					  						DefaultText =  "BankAccountId",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AccountLocalName",
					  						ObjectTableName =  "ExternalReconciliation",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  200,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  200,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AccountLocalName",
					  						ListPropertyPath =  "AccountLocalName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AccountLocalName",
					  						DefaultText =  "Account",
					  						FullLocalDefaultText =  "חשבון",
					  						ListFieldLable =  "AccountLocalNameListLable",
					  						ListLableDefaultText =  "Account",
					  						ListLocalDefaultText =  "חשבון",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CrossYearReconcile",
					  						ObjectTableName =  "ExternalReconciliation",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CrossYearReconcile",
					  						ListPropertyPath =  "CrossYearReconcile",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ExternalReconciliation",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CrossYearReconcile",
					  						DefaultText =  "Cross Year Reconcile",
					  						ListFieldLable =  "CrossYearReconcileListLable",
					  						ListLableDefaultText =  "Cross Year Reconcile",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters,Dictionary<string, QueryGroup> tenantQueryGroups )
	    {    
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable ExternalReconciliationObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "ExternalReconciliation" && d.Tenant == 0).FirstOrDefault();
		   //List<ObjectField> ExternalReconciliationObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "ExternalReconciliation").ToList();
		       
	      

	         Screen ExternalReconciliationExternalReconciliationHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ExternalReconciliation.ExternalReconciliationHeaderScreen", Name = "ExternalReconciliationHeaderScreen", ObjectTableId = ExternalReconciliationObjectTable.Id, NumberOfColumns = 4, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      
             ScreenField ExternalReconciliationExternalReconciliationExternalReconciliationHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ScreenId = ExternalReconciliationExternalReconciliationHeaderScreenScreen0.Id,ScreenCode = ExternalReconciliationExternalReconciliationHeaderScreenScreen0.Code, ObjectFieldCode = "ExternalReconciliation.ReconciliationNumber", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField ExternalReconciliationExternalReconciliationExternalReconciliationHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ScreenId = ExternalReconciliationExternalReconciliationHeaderScreenScreen0.Id,ScreenCode = ExternalReconciliationExternalReconciliationHeaderScreenScreen0.Code, ObjectFieldCode = "ExternalReconciliation.AccountNumber", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField ExternalReconciliationExternalReconciliationExternalReconciliationHeaderScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ScreenId = ExternalReconciliationExternalReconciliationHeaderScreenScreen0.Id,ScreenCode = ExternalReconciliationExternalReconciliationHeaderScreenScreen0.Code, ObjectFieldCode = "ExternalReconciliation.CreateDate", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField ExternalReconciliationExternalReconciliationExternalReconciliationHeaderScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ScreenId = ExternalReconciliationExternalReconciliationHeaderScreenScreen0.Id,ScreenCode = ExternalReconciliationExternalReconciliationHeaderScreenScreen0.Code, ObjectFieldCode = "ExternalReconciliation.IsCancelled", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          	
		    ExternalReconciliationObjectTable.HeaderScreenId = ExternalReconciliationExternalReconciliationHeaderScreenScreen0.Id;
		    ExternalReconciliationObjectTable.HeaderScreenCode = ExternalReconciliationExternalReconciliationHeaderScreenScreen0.Code;

	   		  

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable ExternalReconciliationObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "ExternalReconciliation" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode ExternalReconciliationDetailsTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ExternalReconciliation.TH.Details", DefaultText = "Details",LocalDefaultText = "פרטים", ObjectTableId = ExternalReconciliationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature ExternalReconciliationDetailsFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ExternalReconciliation.Tab.Details", ObjectTableId = ExternalReconciliationObjectTable.Id, Tenant = 0, NameTextCodeCode = "ExternalReconciliation.Features.Details", NameTextCodeDefaultText = "Details", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,ExternalReconciliationObjectTable);
 
                 
			   TextCode ExternalReconciliationEventsTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ExternalReconciliation.TH.Events", DefaultText = "Events",LocalDefaultText = "אירועים", ObjectTableId = ExternalReconciliationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature ExternalReconciliationEventsFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ExternalReconciliation.Tab.Events", ObjectTableId = ExternalReconciliationObjectTable.Id, Tenant = 0, NameTextCodeCode = "ExternalReconciliationFeatures.EREV", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,ExternalReconciliationObjectTable);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "ERDT",HtmlComponentName = "ExternalRecoDetailsTabComponent",HtmlComponentUrl = "./Accounting/Components/EditTabs/ExternalReconciliation/ExternalRecoDetailsTabComponent", FeatureId = ExternalReconciliationDetailsFeature_TH0.Id,FeatureUniqeCode = ExternalReconciliationDetailsFeature_TH0.FeatureUniqeCode, ControlPath = "ExternalRecoDetailsTabComponent", ObjectTableId = ExternalReconciliationObjectTable.Id, TabNameTextCodeId = ExternalReconciliationDetailsTextCode_TH0.Id, TabNameTextCodeCode = ExternalReconciliationDetailsTextCode_TH0.Code, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "EREV",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = ExternalReconciliationEventsFeature_TH1.Id,FeatureUniqeCode = ExternalReconciliationEventsFeature_TH1.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = ExternalReconciliationObjectTable.Id, TabNameTextCodeId = ExternalReconciliationEventsTextCode_TH1.Id, TabNameTextCodeCode = ExternalReconciliationEventsTextCode_TH1.Code, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable ExternalReconciliationObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "ExternalReconciliation" && d.Tenant == 0).FirstOrDefault(); 

		   Feature ExternalReconciliationFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = ExternalReconciliationObjectTable.Id, Tenant = 0, NameTextCodeCode = "ExternalReconciliation.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ExternalReconciliationObjectTable);
		   Feature ExternalReconciliationFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = ExternalReconciliationObjectTable.Id, Tenant = 0, NameTextCodeCode = "ExternalReconciliation.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ExternalReconciliationObjectTable);
		   Feature ExternalReconciliationFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = ExternalReconciliationObjectTable.Id, Tenant = 0, NameTextCodeCode = "ExternalReconciliation.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ExternalReconciliationObjectTable);
		   Feature ExternalReconciliationFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = false, ObjectTableId = ExternalReconciliationObjectTable.Id, Tenant = 0, NameTextCodeCode = "ExternalReconciliation.Features.PackageFeature", NameTextCodeDefaultText = "ExternalReconciliation Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ExternalReconciliationObjectTable); 

		   		   //--------------> Additional Features <--------------\\

		   Feature ExternalReconciliationFeature_ExtRecoGenerateTestRecords = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ExtRecoGenerateTestRecords", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ExternalReconciliationObjectTable.Id, Tenant = 0, NameTextCodeCode = "ExternalReconciliation.Features.ExtRecoGenerateTestRecords", NameTextCodeDefaultText = @"Generate Test Records" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ExternalReconciliationObjectTable);

		   Feature ExternalReconciliationFeature_ExtRecoCreateBankTransferPY = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ExtRecoCreateBankTransferPY", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ExternalReconciliationObjectTable.Id, Tenant = 0, NameTextCodeCode = "ExternalReconciliation.Features.ExtRecoCreateBankTransferPY", NameTextCodeDefaultText = @"Create Bank Transfer Payment Button" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ExternalReconciliationObjectTable);

   
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable ExternalReconciliationObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "ExternalReconciliation" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CREV",
                EnglishName =  "Reconciliation Created",
                LocalName =  "Reconciliation Created",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = ExternalReconciliationObjectTable.Id,
				 
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
                ObjectTableId = ExternalReconciliationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "ERCN",
                EnglishName =  "Reconciliation Cancelled",
                LocalName =  "Reconciliation Cancelled",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = ExternalReconciliationObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
		   //FeatureRepository featureRepository = new FeatureRepository(0); 
		   //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList(); 
		   ObjectTable ExternalReconciliationObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "ExternalReconciliation" && d.Tenant == 0).FirstOrDefault();       
    
			   Feature ExternalReconciliationFeature_MB0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ExtReconciliationCancelMenuButton", ObjectTableId = ExternalReconciliationObjectTable.Id, Tenant = 0, NameTextCodeCode = "ExternalReconciliation.Features.CancelReconcileMenuButton", NameTextCodeDefaultText = "Cancel Reconciltiation", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,ExternalReconciliationObjectTable);

 

		   TextCodeRepository.SubmitChanges();
		   FeaturesRepository.SubmitChanges();
		   MenuButtonGroup ExternalReconciliationMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
				{
					MenuButtonGroupType = "ExternalReconciliationEdit",
					Name = "ExtReconciliationEditButtonsGroup",
					ObjectTableId = ExternalReconciliationObjectTable.Id,
					Tenant = 0
				}, menuButtonGroupRepository, tenantMenuButtonGroups);        
   
			   MenuButton ExternalReconciliationMenuButton0 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "CancelExtReco",
						Index = 0, 
						IsActive = true,
						LabelTextCodeCode = "ExternalReconciliation.B.CancelExtReco",
						LabelTextCodeDefaultText = "Cancel Rec.",
						Tenant = 0,
						MenuButtonGroupId = ExternalReconciliationMenuButtonGroup.Id,
						ObjectTableId = ExternalReconciliationObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = ExternalReconciliationFeature_MB0.Id,
						Style = null,
						LocalDefaultText = "ביטול התאמה",
						FeatureUniqeCode = ExternalReconciliationFeature_MB0.FeatureUniqeCode,
						HtmlComponentPath = null,
						Width = 0,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);

	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable ExternalReconciliationObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "ExternalReconciliation" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode ExternalReconciliationTextCode_ExternalReconciliationOCantReconcileTwoTransfer = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ExternalReconciliation.O.CantReconcileTwoTransfer", DefaultText = "One line at a time must be marked when making a deferred check payment reconcile.",LocalDefaultText = @"יש לסמן שורה אחת בכל פעם כאשר מבצעים התאמה של פירעון שיק דחוי.", ObjectTableId = ExternalReconciliationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ExternalReconciliationTextCode_ExternalReconciliationOblockOneTransferTwoPageLines = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ExternalReconciliation.O.blockOneTransferTwoPageLines", DefaultText = "When transfer transaction selected, only one line should be marked on the external page with the deferred check amount.",LocalDefaultText = @"כאשר פורעים שיק דחוי יש לסמן רק שורה אחת בצד הבנק", ObjectTableId = ExternalReconciliationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ExternalReconciliationTextCode_ExternalReconciliationOblockTwoTransferOnePageLine = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ExternalReconciliation.O.blockTwoTransferOnePageLine", DefaultText = "There is two transfer transactions selected, you can select page line only if one transfer ledger is selected",LocalDefaultText = null, ObjectTableId = ExternalReconciliationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ExternalReconciliationTextCode_ExternalReconciliationOOnlyOneTransferTransactionCanReconciledWithOnePageLine = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ExternalReconciliation.O.OnlyOneTransferTransactionCanReconciledWithOnePageLine", DefaultText = "Only one transfer transaction can be reconciled with only one page line in one time",LocalDefaultText = @"ניתן לפרוע  שיק אחד בכל פעולה , יש לסמן שורה אחת  מכרטיס שקים לפרעון ", ObjectTableId = ExternalReconciliationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ExternalReconciliationTextCode_ExternalReconciliationOSelectOneAutoRecoMethod = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ExternalReconciliation.O.SelectOneAutoRecoMethod", DefaultText = "At least one Automatic Reconcile method should be selected",LocalDefaultText = @"יש לסמן לפחות שיטת התאמה אחת", ObjectTableId = ExternalReconciliationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ExternalReconciliationTextCode_ExternalReconciliationOCantAutoRecoByRefDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ExternalReconciliation.O.CantAutoRecoByRefDate", DefaultText = "You cannot automatically reconcile only by reference date. It must be paired with another option",LocalDefaultText = @"לא ניתן לבצע התאמה אוטומטית לפי תאריך אסמכתא ו/או תאריך חשבונאי בלבד", ObjectTableId = ExternalReconciliationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ExternalReconciliationTextCode_ExternalReconciliationOCantAdjustLedgersOnly = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ExternalReconciliation.O.CantAdjustLedgersOnly", DefaultText = "Cant adjust only transaction lines",LocalDefaultText = @"לא ניתן לבצע התאמה עם שורות מהכרטיס בלבד", ObjectTableId = ExternalReconciliationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ExternalReconciliationTextCode_ExternalReconciliationOCantReconcileTransferTransactionsWithMultipleBankPages = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ExternalReconciliation.O.CantReconcileTransferTransactionsWithMultipleBankPages", DefaultText = "You can't Reconcile Transfer Transactions with multiple bank pages. Please select only one bank page",LocalDefaultText = @"לא ניתן להתאים תנועה מכרטיס דחויים עם יותר משורה אחת מצד הבנק, נא לבחור שורה אחת בכל פעם", ObjectTableId = ExternalReconciliationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ExternalReconciliationTextCode_ExternalReconciliationOCreateBankTransferInfo = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ExternalReconciliation.O.CreateBankTransferInfo", DefaultText = "Once you select one or more bank Page lines, this option allows you to create and reconcile a new AR Payment with the selected lines",LocalDefaultText = @"אופציה זו מאפשרת להפיק קבלה משורת דף בנק אחת או יותר", ObjectTableId = ExternalReconciliationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ExternalReconciliationTextCode_ExternalReconciliationOBankTransferDifferenceMsg = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ExternalReconciliation.O.BankTransferDifferenceMsg", DefaultText = "Please check the selected lines. The difference should be less than zero in order to create the payment",LocalDefaultText = @"נא לבחור תנועות פתוחות בסכום חובה גדול מאפס", ObjectTableId = ExternalReconciliationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ExternalReconciliationTextCode_ExternalReconciliationOBTOnlyBankPages = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ExternalReconciliation.O.BTOnlyBankPages", DefaultText = "Please select only bank pages in order to continue",LocalDefaultText = @"על מנת להפיק קבלה יש לבחור דפי בנק בלבד", ObjectTableId = ExternalReconciliationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ExternalReconciliationTextCode_ExternalReconciliationOBTbackgroundCreationMSG = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ExternalReconciliation.O.BTbackgroundCreationMSG", DefaultText = "AR Payment #number has been created successfully. The external reconciliation is being created in the background",LocalDefaultText = @"נוצרה קבלה #number , ההתאמה תבוצע ברקע", ObjectTableId = ExternalReconciliationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ExternalReconciliationTextCode_ExternalReconciliationOBTUnsavedChanges = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ExternalReconciliation.O.BTUnsavedChanges", DefaultText = "This A/R Payment has unsaved changes. By continuing, you will lose the payment and the changes done to the reconciliation screen",LocalDefaultText = @"בקבלה זאת בוצעו שינויים שטרם נשמרו , האם ברצונך לשמור זאת בהתחשב בכך שהתאמה חיצונית לא תתבצע אם הקבלה לא תאושר", ObjectTableId = ExternalReconciliationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ExternalReconciliationTextCode_ExternalReconciliationOFutureValueDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ExternalReconciliation.O.FutureValueDate", DefaultText = "Value date must be less than or equals to today's date",LocalDefaultText = @"תאריך ערך צריך להיות קטן מתאריך נוכחי", ObjectTableId = ExternalReconciliationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ExternalReconciliationTextCode_ExternalReconciliationOBTCreditLinesOnly = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ExternalReconciliation.O.BTCreditLinesOnly", DefaultText = "Please check the selected lines. AR Payment reconciliation is allowed only with positive credit bank pages lines",LocalDefaultText = @"ניתן ליצור קבלה רק על שורת זכות חיובית בצד הבנק", ObjectTableId = ExternalReconciliationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ExternalReconciliationTextCode_ExternalReconciliationOBTCreateARPayment = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ExternalReconciliation.O.BTCreateARPayment", DefaultText = "Create AR Payment",LocalDefaultText = @"יצירת קבלה", ObjectTableId = ExternalReconciliationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ExternalReconciliationTextCode_ExternalReconciliationOCrossYearConfirmMsg = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ExternalReconciliation.O.CrossYearConfirmMsg", DefaultText = "There are some reconciliation lines from different years. Are you sure you want to continue?",LocalDefaultText = @"ישנם שורות להתאמה משנים שונות! האם אתה בטוח שברצונך להמשיך", ObjectTableId = ExternalReconciliationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ExternalReconciliationTextCode_ExternalReconciliationOShowCrossYearLabel = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ExternalReconciliation.O.ShowCrossYearLabel", DefaultText = "Show Cross Years Reconciliations",LocalDefaultText = @"הצג התאמות עם שורות משנים שונות", ObjectTableId = ExternalReconciliationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ExternalReconciliationTextCode_ExternalReconciliationOAccoountFilterAll = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ExternalReconciliation.O.AccoountFilter.All", DefaultText = "All",LocalDefaultText = @"הכל", ObjectTableId = ExternalReconciliationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ExternalReconciliationTextCode_ExternalReconciliationOAccoountFilterBank = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ExternalReconciliation.O.AccoountFilter.Bank", DefaultText = "Bank GL Account",LocalDefaultText = @"כרטיס חשבון", ObjectTableId = ExternalReconciliationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ExternalReconciliationTextCode_ExternalReconciliationOAccoountFilterTransfer = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ExternalReconciliation.O.AccoountFilter.Transfer", DefaultText = "Transfer GL Account",LocalDefaultText = @"כרטיס דחויים לשלם", ObjectTableId = ExternalReconciliationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ExternalReconciliationTextCode_ExternalReconciliationOAccoountFilterlabel = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ExternalReconciliation.O.AccoountFilter.label", DefaultText = "Display transactions from",LocalDefaultText = @"הצג תנועות מ", ObjectTableId = ExternalReconciliationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ExternalReconciliationTextCode_ExternalReconciliationOAccoountFilterTransferTransaction = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ExternalReconciliation.O.AccoountFilter.TransferTransaction", DefaultText = "Transfer Transaction",LocalDefaultText = @"תנועה מחשבון דחויים", ObjectTableId = ExternalReconciliationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ExternalReconciliationTextCode_ExternalReconciliationOSelectOnePage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ExternalReconciliation.O.SelectOnePage", DefaultText = "to adjust bank fees with Transaction select only one page line",LocalDefaultText = @"לא ניתן לפרוע מספר קבלות בו זמנית , יש לבצע פרעון של כל שורת קבלה בנפרד.", ObjectTableId = ExternalReconciliationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ExternalReconciliationTextCode_ExternalReconciliationOSelectOneOrMore = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ExternalReconciliation.O.SelectOneOrMore", DefaultText = "to adjust bank fees, select one or more row External page line",LocalDefaultText = @"כדי ליצור פקודה בגין עמלות יש לבחור בשורת בנק אחת בכל פעם", ObjectTableId = ExternalReconciliationObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 