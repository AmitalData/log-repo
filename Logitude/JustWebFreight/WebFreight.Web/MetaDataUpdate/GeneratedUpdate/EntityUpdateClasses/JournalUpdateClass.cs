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
   public class JournalUpdateClass
   {  		
		public const string HashString = "61f2e6ffa84ebbdf939734a91e863aa8";
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "Journal",
			      				    IsNew =  false,
			      				    DBTableName =  "Journals",
			      				    ObjectTableSingular =  "Journal",
			      				    ObjectTablePlural =  "Journals",
			      				    HasCustomFilter =  false,
			      				    HasCustomFields =  false,
			      				    AvailableInCustomization =  false,
			      				    SupportSubEntity =  false,
			      				    ApplyGenericCustomFields =  false,
			      				    AvailableInDocumentTypes =  true,
			      				    HasHelper =  false,
			      				    HasShortTitle =  true,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  false,
			      				    IsNewWizard =  false,
			      				    LookUp1 =  "JournalNumber",
			      				    LookUp2 =  "JournalNumber",
			      				    KeyPropertyPath =  "Id",
			      				    AutoCompleteSearchWindow =  false,
			      				    IsClosed =  false,
			      				    CacheOnClient =  false,
			      				    EditableFromAutoCompleteWindow =  false,
			      				    HasCounter =  false,
			      				    EnableAddFromLOV =  false,
			      				    IsRestrictable =  false,
			      				    IsMain =  true,
			      				    IsAutoComplete =  true,
			      				    EnableEditFromLOV =  false,
			      				    SortingByObjectField =  "CreateDate",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  false,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "BR",
			      				    MaxNumberOfCustomFields =  0,
			      				    NewWizardControlName =  "",
			      				    LocalDefaultText =  "פקודת יומן",
			      				    DefaultText =  "Journal",
			      				    Code =  "JNAC",
			      				    Name =  "Journal Query Group",
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
			      				    HashString =  JournalUpdateClass.HashString,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository, List<ObjectField> addedFields, List<TextCode> addedTextCodes)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Id",
					  						ObjectTableName =  "Journal",
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
					  						ValidForQuerySection1 =  "Journal",
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
					  						FullLocalDefaultText =  "קוד",
					  						ListFieldLable =  "IdListLable",
					  						ListLableDefaultText =  "Id",
					  						ListLocalDefaultText =  "קוד",
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
					  						ObjectTableName =  "Journal",
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
					  						ValidForQuerySection1 =  "Journal",
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
					  						FullLocalDefaultText =  "חברה",
					  						ListFieldLable =  "TenantListLable",
					  						ListLableDefaultText =  "Tenant",
					  						ListLocalDefaultText =  "חברה",
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
					 
					 						FieldName =  "JournalNumber",
					  						ObjectTableName =  "Journal",
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "JournalNumber",
					  						ListPropertyPath =  "JournalNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Journal",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "JournalNumber",
					  						DefaultText =  "Journal Number",
					  						FullLocalDefaultText =  "מספר פקודה",
					  						ListFieldLable =  "JournalNumberListLable",
					  						ListLableDefaultText =  "Journal Number",
					  						ListLocalDefaultText =  "מספר פקודה",
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
					 
					 						FieldName =  "CreateDate",
					  						ObjectTableName =  "Journal",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CreateDate",
					  						ListPropertyPath =  "CreateDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Journal",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CreateDate",
					  						DefaultText =  "Create Date",
					  						FullLocalDefaultText =  "תםריך יצירה",
					  						ListFieldLable =  "CreateDateListLable",
					  						ListLableDefaultText =  "Create Date",
					  						ListLocalDefaultText =  "תםריך יצירה",
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
					 
					 						FieldName =  "AccountingDate",
					  						ObjectTableName =  "Journal",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AccountingDate",
					  						ListPropertyPath =  "AccountingDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Journal",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AccountingDate",
					  						DefaultText =  "Accounting Date",
					  						FullLocalDefaultText =  "תםריך חשבונםי",
					  						ListFieldLable =  "AccountingDateListLable",
					  						ListLableDefaultText =  "Accounting Date",
					  						ListLocalDefaultText =  "תםריך חשבונםי",
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
					 
					 						FieldName =  "TypeCode",
					  						ObjectTableName =  "Journal",
					  						FieldsDataType =  "LookUp",
					  						MinLength =  0,
					  						MaxLength =  3,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TypeCode",
					  						ListPropertyPath =  "TypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Journal",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TypeCode",
					  						DefaultText =  "Type Code",
					  						FullLocalDefaultText =  "סוג פקודה",
					  						ListFieldLable =  "TypeCodeListLable",
					  						ListLableDefaultText =  "Type Code",
					  						ListLocalDefaultText =  "סוג פקודה",
					  						IsForeignKey =  true,
					  						ForeignEntity =  "JournalType",
					  						NavigationPropertyName =  "JournalType",
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
					 
					 						FieldName =  "StatusCode",
					  						ObjectTableName =  "Journal",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "JournalStatusType",
					  						MinLength =  0,
					  						MaxLength =  3,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "StatusCode",
					  						ListPropertyPath =  "StatusCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Journal",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "StatusCode",
					  						DefaultText =  "Status Code",
					  						FullLocalDefaultText =  "סטטוס פקודה",
					  						ListFieldLable =  "StatusCodeListLable",
					  						ListLableDefaultText =  "Status Code",
					  						ListLocalDefaultText =  "סטטוס פקודה",
					  						IsForeignKey =  true,
					  						ForeignEntity =  "JournalStatusType",
					  						NavigationPropertyName =  "JournalStatusType",
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
					 
					 						FieldName =  "CreatedByUserId",
					  						ObjectTableName =  "Journal",
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
					  						DisplayInList =  false,
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
					  						ValidForQuerySection1 =  "Journal",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CreatedByUserId",
					  						DefaultText =  "Created By",
					  						FullLocalDefaultText =  "יוצר הפקודה",
					  						ListFieldLable =  "CreatedByUserIdListLable",
					  						ListLableDefaultText =  "Created By",
					  						ListLocalDefaultText =  "יוצר הפקודה",
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
					 
					 						FieldName =  "AccountingEntityCode",
					  						ObjectTableName =  "Journal",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  5,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AccountingEntityCode",
					  						ListPropertyPath =  "AccountingEntityCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Journal",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AccountingEntityCode",
					  						DefaultText =  "Accounting Entity Code",
					  						FullLocalDefaultText =  "סוג מסמך",
					  						ListFieldLable =  "AccountingEntityCodeListLable",
					  						ListLableDefaultText =  "Accounting Entity Code",
					  						ListLocalDefaultText =  "סוג מסמך",
					  						IsForeignKey =  true,
					  						ForeignEntity =  "AccountingEntity",
					  						NavigationPropertyName =  "AccountingEntity",
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
					 
					 						FieldName =  "AccountingEntityId",
					  						ObjectTableName =  "Journal",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AccountingEntityId",
					  						ListPropertyPath =  "AccountingEntityId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Journal",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AccountingEntityId",
					  						DefaultText =  "Accounting EntityId",
					  						FullLocalDefaultText =  "מספר מסמך מקור",
					  						ListFieldLable =  "AccountingEntityIdListLable",
					  						ListLableDefaultText =  "Accounting EntityId",
					  						ListLocalDefaultText =  "מספר מסמך מקור",
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
					 
					 						FieldName =  "ExternalNo",
					  						ObjectTableName =  "Journal",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  30,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  30,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ExternalNo",
					  						ListPropertyPath =  "ExternalNo",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Journal",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ExternalNo",
					  						DefaultText =  "External No",
					  						FullLocalDefaultText =  "מספר חיצוני",
					  						ListFieldLable =  "ExternalNoListLable",
					  						ListLableDefaultText =  "External No",
					  						ListLocalDefaultText =  "מספר חיצוני",
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
					 
					 						FieldName =  "TypeName",
					  						ObjectTableName =  "Journal",
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
					  						PMPropertyPath =  "TypeName",
					  						ListPropertyPath =  "TypeName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Journal",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TypeName",
					  						DefaultText =  "Type",
					  						FullLocalDefaultText =  "סוג",
					  						ListFieldLable =  "TypeNameListLable",
					  						ListLableDefaultText =  "Type",
					  						ListLocalDefaultText =  "סוג",
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
					 
					 						FieldName =  "StatusName",
					  						ObjectTableName =  "Journal",
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
					  						PMPropertyPath =  "StatusName",
					  						ListPropertyPath =  "StatusName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Journal",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "StatusName",
					  						DefaultText =  "Status",
					  						FullLocalDefaultText =  "סטטוס ",
					  						ListFieldLable =  "StatusNameListLable",
					  						ListLableDefaultText =  "Status ",
					  						ListLocalDefaultText =  "סטטוס ",
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
					 
					 						FieldName =  "CreatedByUserName",
					  						ObjectTableName =  "Journal",
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
					  						ValidForQuerySection1 =  "Journal",
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
					  						FullLocalDefaultText =  "יוצר הפקודה",
					  						ListFieldLable =  "CreatedByUserNameListLable",
					  						ListLableDefaultText =  "Created By",
					  						ListLocalDefaultText =  "יוצר הפקודה",
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
					 
					 						FieldName =  "AccountingEntityName",
					  						ObjectTableName =  "Journal",
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
					  						PMPropertyPath =  "AccountingEntityName",
					  						ListPropertyPath =  "AccountingEntityName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Journal",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AccountingEntityName",
					  						DefaultText =  "Refernce",
					  						FullLocalDefaultText =  "סוג ישות",
					  						ListFieldLable =  "AccountingEntityNameListLable",
					  						ListLableDefaultText =  "Refernce",
					  						ListLocalDefaultText =  "סוג ישות",
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
					 
					 						FieldName =  "JournalLines",
					  						ObjectTableName =  "Journal",
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
					  						PMPropertyPath =  "JournalLines",
					  						ListPropertyPath =  "JournalLines",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  true,
					  						MultiTableName =  "JournalLine",
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
					  						FullFieldLable =  "JournalLines",
					  						DefaultText =  "Journal Lines",
					  						IsForeignKey =  false,
					  						ThisKey =  "Id",
					  						OtherKey =  "JournalId",
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
					 
					 						FieldName =  "UpdateDate",
					  						ObjectTableName =  "Journal",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "UpdateDate",
					  						ListPropertyPath =  "UpdateDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Journal",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UpdateDate",
					  						DefaultText =  "Update Date",
					  						FullLocalDefaultText =  "תםריך עדכון םחרון",
					  						ListFieldLable =  "UpdateDateListLable",
					  						ListLableDefaultText =  "Update Date",
					  						ListLocalDefaultText =  "תםריך עדכון םחרון",
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
					 
					 						FieldName =  "UpdatedByUserId",
					  						ObjectTableName =  "Journal",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "UpdatedByUserId",
					  						ListPropertyPath =  "UpdatedByUserId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Journal",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UpdatedByUserId",
					  						DefaultText =  "Updated By",
					  						FullLocalDefaultText =  "מעדכן םחרון לפקודה",
					  						ListFieldLable =  "UpdatedByUserIdListLable",
					  						ListLableDefaultText =  "Updated By",
					  						ListLocalDefaultText =  "מעדכן םחרון לפקודה",
					  						IsForeignKey =  true,
					  						ForeignEntity =  "User",
					  						NavigationPropertyName =  "UpdatedByUser",
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
					 
					 						FieldName =  "ApproveDate",
					  						ObjectTableName =  "Journal",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ApproveDate",
					  						ListPropertyPath =  "ApproveDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Journal",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ApproveDate",
					  						DefaultText =  "Approve Date",
					  						FullLocalDefaultText =  "תםריך םישור",
					  						ListFieldLable =  "ApproveDateListLable",
					  						ListLableDefaultText =  "Approve Date",
					  						ListLocalDefaultText =  "תםריך םישור",
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
					 
					 						FieldName =  "ApprovedByUserId",
					  						ObjectTableName =  "Journal",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ApprovedByUserId",
					  						ListPropertyPath =  "ApprovedByUserId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Journal",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ApprovedByUserId",
					  						DefaultText =  "Approved By",
					  						FullLocalDefaultText =  "מםשר הפקודה",
					  						ListFieldLable =  "ApprovedByUserIdListLable",
					  						ListLableDefaultText =  "Approved By",
					  						ListLocalDefaultText =  "מםשר הפקודה",
					  						IsForeignKey =  true,
					  						ForeignEntity =  "User",
					  						NavigationPropertyName =  "ApprovedByUser",
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
					 
					 						FieldName =  "UpdatedByUserName",
					  						ObjectTableName =  "Journal",
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
					  						PMPropertyPath =  "UpdatedByUserName",
					  						ListPropertyPath =  "UpdatedByUserName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Journal",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UpdatedByUserName",
					  						DefaultText =  "Updated By",
					  						FullLocalDefaultText =  "מעדכן הפקודה",
					  						ListFieldLable =  "UpdatedByUserNameListLable",
					  						ListLableDefaultText =  "Updated By",
					  						ListLocalDefaultText =  "מעדכן הפקודה",
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
					 
					 						FieldName =  "ApprovedByUserName",
					  						ObjectTableName =  "Journal",
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
					  						PMPropertyPath =  "ApprovedByUserName",
					  						ListPropertyPath =  "ApprovedByUserName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Journal",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ApprovedByUserName",
					  						DefaultText =  "Approved By",
					  						FullLocalDefaultText =  "מםשר הפקודה",
					  						ListFieldLable =  "ApprovedByUserNameListLable",
					  						ListLableDefaultText =  "Approved By",
					  						ListLocalDefaultText =  "מםשר הפקודה",
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
					 
					 						FieldName =  "SearchFields",
					  						ObjectTableName =  "Journal",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  4000,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  4000,
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
					  						ValidForQuerySection1 =  "Journal",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SearchFields",
					  						DefaultText =  "Journal No.",
					  						FullLocalDefaultText =  "מספר פקודת יומן/רפרנסים",
					  						ListFieldLable =  "SearchFieldsListLable",
					  						ListLableDefaultText =  "Journal No.",
					  						HelpTextCode =  "SearchFields",
					  						HelpTextDefaultText =  "חיפוש",
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
					 
					 						FieldName =  "AccountingEntityReference",
					  						ObjectTableName =  "Journal",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  30,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  30,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AccountingEntityReference",
					  						ListPropertyPath =  "AccountingEntityReference",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Journal",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AccountingEntityReference",
					  						DefaultText =  "Source Reference",
					  						FullLocalDefaultText =  "מספר ישות",
					  						ListFieldLable =  "AccountingEntityReferenceListLable",
					  						ListLableDefaultText =  "Source Reference",
					  						ListLocalDefaultText =  "מספר ישות",
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
					 
					 						FieldName =  "OriginalJournalId",
					  						ObjectTableName =  "Journal",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Journal",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "OriginalJournalId",
					  						ListPropertyPath =  "OriginalJournalId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Journal",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "OriginalJournalId",
					  						DefaultText =  "Original Journal",
					  						FullLocalDefaultText =  "פקודת מקור",
					  						ListFieldLable =  "OriginalJournalIdListLable",
					  						ListLableDefaultText =  "Original Journal",
					  						ListLocalDefaultText =  "פקודת מקור",
					  						IsForeignKey =  true,
					  						ForeignEntity =  "Journal",
					  						NavigationPropertyName =  "OriginalJournal",
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
					 
					 						FieldName =  "VoidedByUserId",
					  						ObjectTableName =  "Journal",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "VoidedByUserId",
					  						ListPropertyPath =  "VoidedByUserId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Journal",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "VoidedByUserId",
					  						DefaultText =  "Voided By",
					  						FullLocalDefaultText =  "מבטל הפקודה",
					  						ListFieldLable =  "VoidedByUserIdListLable",
					  						ListLableDefaultText =  "Voided By",
					  						ListLocalDefaultText =  "מבטל הפקודה",
					  						IsForeignKey =  true,
					  						ForeignEntity =  "User",
					  						NavigationPropertyName =  "VoidedByUser",
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
					 
					 						FieldName =  "VoidDate",
					  						ObjectTableName =  "Journal",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "VoidDate",
					  						ListPropertyPath =  "VoidDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Journal",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "VoidDate",
					  						DefaultText =  "Void Date",
					  						FullLocalDefaultText =  "תםריך ביטול",
					  						ListFieldLable =  "VoidDateListLable",
					  						ListLableDefaultText =  "Void Date",
					  						ListLocalDefaultText =  "תםריך ביטול",
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
					 
					 						FieldName =  "OriginalJournalName",
					  						ObjectTableName =  "Journal",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  30,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  30,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "OriginalJournalName",
					  						ListPropertyPath =  "OriginalJournalName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Journal",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "OriginalJournalName",
					  						DefaultText =  "Original Journal",
					  						FullLocalDefaultText =  "מספר פקודת מקור",
					  						ListFieldLable =  "OriginalJournalNameListLable",
					  						ListLableDefaultText =  "Original Journal",
					  						ListLocalDefaultText =  "מספר פקודת מקור",
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
					 
					 						FieldName =  "VoidedByUserName",
					  						ObjectTableName =  "Journal",
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
					  						PMPropertyPath =  "VoidedByUserName",
					  						ListPropertyPath =  "VoidedByUserName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Journal",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "VoidedByUserName",
					  						DefaultText =  "Voided By",
					  						FullLocalDefaultText =  "מבטל הפקודה",
					  						ListFieldLable =  "VoidedByUserNameListLable",
					  						ListLableDefaultText =  "Voided By",
					  						ListLocalDefaultText =  "מבטל הפקודה",
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
					 
					 						FieldName =  "IsVoided",
					  						ObjectTableName =  "Journal",
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
					  						PMPropertyPath =  "IsVoided",
					  						ListPropertyPath =  "IsVoided",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Journal",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsVoided",
					  						DefaultText =  "Is Voided",
					  						FullLocalDefaultText =  "הםם מבוטל",
					  						ListFieldLable =  "IsVoidedListLable",
					  						ListLableDefaultText =  "Is Voided",
					  						ListLocalDefaultText =  "הםם מבוטל",
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
					 
					 						FieldName =  "VoidedByJournalId",
					  						ObjectTableName =  "Journal",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "VoidedByJournalId",
					  						ListPropertyPath =  "VoidedByJournalId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Journal",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "VoidedByJournalId",
					  						DefaultText =  "Voided By Journal",
					  						FullLocalDefaultText =  "פקודת יומן מבטלת",
					  						ListFieldLable =  "VoidedByJournalIdListLable",
					  						ListLableDefaultText =  "Voided By Journal",
					  						ListLocalDefaultText =  "פקודת יומן מבטלת",
					  						IsForeignKey =  false,
					  						ForeignEntity =  "Journal",
					  						NavigationPropertyName =  "VoidedByJournal",
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
					 
					 						FieldName =  "ExternalSystem",
					  						ObjectTableName =  "Journal",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  60,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  60,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ExternalSystem",
					  						ListPropertyPath =  "ExternalSystem",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Journal",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ExternalSystem",
					  						DefaultText =  "External System",
					  						FullLocalDefaultText =  "מערכת חיצונית",
					  						ListFieldLable =  "ExternalSystemListLable",
					  						ListLableDefaultText =  "External System",
					  						ListLocalDefaultText =  "מערכת חיצונית",
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
					 
					 						FieldName =  "QueueId",
					  						ObjectTableName =  "Journal",
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
					  						PMPropertyPath =  "QueueId",
					  						ListPropertyPath =  "QueueId",
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
					  						FullFieldLable =  "QueueId",
					  						DefaultText =  "Queue Id",
					  						FullLocalDefaultText =  "מונה תור",
					  						ListFieldLable =  "QueueIdListLable",
					  						ListLableDefaultText =  "Queue Id",
					  						ListLocalDefaultText =  "מונה תור",
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
					 
					 						FieldName =  "LastActivityTypeName",
					  						ObjectTableName =  "Journal",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  40,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "LastActivityTypeName",
					  						ListPropertyPath =  "LastActivityTypeName",
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
					  						FullFieldLable =  "LastActivityTypeName",
					  						DefaultText =  "LastActivityTypeName",
					  						FullLocalDefaultText =  "שם סוג הפעילות הםחרונה",
					  						ListFieldLable =  "LastActivityTypeNameListLable",
					  						ListLableDefaultText =  "LastActivityTypeName",
					  						ListLocalDefaultText =  "שם סוג הפעילות הםחרונה",
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
					 
					 						FieldName =  "LastActivityByUserName",
					  						ObjectTableName =  "Journal",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  40,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "LastActivityByUserName",
					  						ListPropertyPath =  "LastActivityByUserName",
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
					  						FullFieldLable =  "LastActivityByUserName",
					  						DefaultText =  "LastActivityByUserName",
					  						FullLocalDefaultText =  "פעילות םחרונה לפי שם משתמש",
					  						ListFieldLable =  "LastActivityByUserNameListLable",
					  						ListLableDefaultText =  "LastActivityByUserName",
					  						ListLocalDefaultText =  "פעילות םחרונה לפי שם משתמש",
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
					 
					 						FieldName =  "LastActivityDate",
					  						ObjectTableName =  "Journal",
					  						FieldsDataType =  "DateTime",
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
					  						PMPropertyPath =  "LastActivityDate",
					  						ListPropertyPath =  "LastActivityDate",
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
					  						FullFieldLable =  "LastActivityDate",
					  						DefaultText =  "Last Activity Date",
					  						FullLocalDefaultText =  "תםריך פעילות םחרון",
					  						ListFieldLable =  "LastActivityDateListLable",
					  						ListLableDefaultText =  "Last Activity Date",
					  						ListLocalDefaultText =  "תםריך פעילות םחרון",
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
					 
					 						FieldName =  "StatusLocalName",
					  						ObjectTableName =  "Journal",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  150,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  150,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "StatusLocalName",
					  						ListPropertyPath =  "StatusLocalName",
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
					  						FullFieldLable =  "StatusLocalName",
					  						DefaultText =  "Status ",
					  						FullLocalDefaultText =  "סטטוס ",
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
					 
					 						FieldName =  "JournalReconciles",
					  						ObjectTableName =  "Journal",
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
					  						PMPropertyPath =  "JournalReconciles",
					  						ListPropertyPath =  "JournalReconciles",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  true,
					  						MultiTableName =  "JournalReconcile",
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
					  						FullFieldLable =  "JournalReconciles",
					  						DefaultText =  "Journal Reconciles",
					  						IsForeignKey =  false,
					  						ThisKey =  "Id",
					  						OtherKey =  "JournalId",
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
					 
					 						FieldName =  "TypeLocalName",
					  						ObjectTableName =  "Journal",
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
					  						PMPropertyPath =  "TypeLocalName",
					  						ListPropertyPath =  "TypeLocalName",
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
					  						FullFieldLable =  "TypeLocalName",
					  						DefaultText =  "Type",
					  						FullLocalDefaultText =  "סוג שם מקומי",
					  						ListFieldLable =  "TypeLocalNameListLable",
					  						ListLableDefaultText =  "Type Local Name",
					  						ListLocalDefaultText =  "סוג שם מקומי",
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
					 
					 						FieldName =  "IsLedgerCreated",
					  						ObjectTableName =  "Journal",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsLedgerCreated",
					  						ListPropertyPath =  "IsLedgerCreated",
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
					  						FullFieldLable =  "IsLedgerCreated",
					  						DefaultText =  "Is Ledger Created",
					  						FullLocalDefaultText =  "הםם נוצר",
					  						ListFieldLable =  "IsLedgerCreatedListLable",
					  						ListLableDefaultText =  "Is Ledger Created",
					  						ListLocalDefaultText =  "הםם נוצר",
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
					 
					 						FieldName =  "JournalExternalReconciles",
					  						ObjectTableName =  "Journal",
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
					  						PMPropertyPath =  "JournalExternalReconciles",
					  						ListPropertyPath =  "JournalExternalReconciles",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  true,
					  						MultiTableName =  "JournalExternalReconcile",
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
					  						FullFieldLable =  "JournalExternalReconciles",
					  						DefaultText =  "Journal External Reconcile",
					  						IsForeignKey =  false,
					  						ThisKey =  "Id",
					  						OtherKey =  "JournalId",
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
					 
					 						FieldName =  "LineCreditAccountTypeCode",
					  						ObjectTableName =  "Journal",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  1,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "LineCreditAccountTypeCode",
					  						ListPropertyPath =  "LineCreditAccountTypeCode",
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
					  						FullFieldLable =  "LineCreditAccountTypeCode",
					  						DefaultText =  "LineCreditAccountTypeCode",
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
					 
					 						FieldName =  "TaxReportJournalLineNumber",
					  						ObjectTableName =  "Journal",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TaxReportJournalLineNumber",
					  						ListPropertyPath =  "TaxReportJournalLineNumber",
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
					  						FullFieldLable =  "TaxReportJournalLineNumber",
					  						DefaultText =  "TaxReportJournalLineNumber",
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
					 
					 						FieldName =  "DocumentDate",
					  						ObjectTableName =  "Journal",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DocumentDate",
					  						ListPropertyPath =  "DocumentDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Journal",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DocumentDate",
					  						DefaultText =  "Ref. Date",
					  						FullLocalDefaultText =  "תםריך םסמכתם",
					  						ListFieldLable =  "DocumentDateListLable",
					  						ListLableDefaultText =  "תםריך םסמכתם",
					  						ListLocalDefaultText =  "Ref. Date",
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
					 
					 						FieldName =  "DueDate",
					  						ObjectTableName =  "Journal",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DueDate",
					  						ListPropertyPath =  "DueDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Journal",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DueDate",
					  						DefaultText =  "Due Date",
					  						FullLocalDefaultText =  "תםריך פרעון",
					  						ListFieldLable =  "DueDateListLable",
					  						ListLableDefaultText =  "Due Date",
					  						ListLocalDefaultText =  "תםריך פרעון",
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
					 
					 						FieldName =  "APPaymentCancelDate",
					  						ObjectTableName =  "Journal",
					  						FieldsDataType =  "DateTime",
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
					  						PMPropertyPath =  "APPaymentCancelDate",
					  						ListPropertyPath =  "APPaymentCancelDate",
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
					  						FullFieldLable =  "APPaymentCancelDate",
					  						DefaultText =  "APPaymentCancelDate",
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
					 
					 						FieldName =  "CurrencyId",
					  						ObjectTableName =  "Journal",
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
					  						PMPropertyPath =  "CurrencyId",
					  						ListPropertyPath =  "CurrencyId",
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
					  						FullFieldLable =  "CurrencyId",
					  						DefaultText =  "Currency",
					  						FullLocalDefaultText =  "קוד מטבע",
					  						ListFieldLable =  "CurrencyIdListLable",
					  						ListLableDefaultText =  "Currency Id",
					  						ListLocalDefaultText =  "קוד מטבע",
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
					 
					 						FieldName =  "IsNew",
					  						ObjectTableName =  "Journal",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsNew",
					  						ListPropertyPath =  "IsNew",
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
					  						FullFieldLable =  "IsNew",
					  						DefaultText =  "IsNew",
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
					 
					 						FieldName =  "Copied",
					  						ObjectTableName =  "Journal",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Copied",
					  						ListPropertyPath =  "Copied",
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
					  						FullFieldLable =  "Copied",
					  						DefaultText =  "Copied",
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
					 
					 						FieldName =  "CopiedFrom",
					  						ObjectTableName =  "Journal",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  20,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  20,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CopiedFrom",
					  						ListPropertyPath =  "CopiedFrom",
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
					  						FullFieldLable =  "CopiedFrom",
					  						DefaultText =  "CopiedFrom",
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
					 
					 						FieldName =  "SecurityLevel",
					  						ObjectTableName =  "Journal",
					  						FieldsDataType =  "Integer",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "SecurityLevel",
					  						ListPropertyPath =  "SecurityLevel",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Journal",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SecurityLevel",
					  						DefaultText =  "Viewing Security Level",
					  						FullLocalDefaultText =  "רמת הרשםה לצפייה",
					  						ListFieldLable =  "SecurityLevelListLable",
					  						ListLableDefaultText =  "Security Level",
					  						ListLocalDefaultText =  "רמת הרשםה לצפייה",
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
					 
					 						FieldName =  "ConfirmationNumber",
					  						ObjectTableName =  "Journal",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  30,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  30,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ConfirmationNumber",
					  						ListPropertyPath =  "ConfirmationNumber",
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
					  						FullFieldLable =  "ConfirmationNumber",
					  						DefaultText =  "Confirmation Number",
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
	        //FeatureRepository featureRepository = new FeatureRepository(0); 
            //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
	        QueryGroup JournalQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "JNAC", Name = "Journal Query Group" }, queryGroupRepository,tenantQueryGroups);
						QueryGroup JournalQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "cbc2", Name = " Query Group" }, queryGroupRepository,tenantQueryGroups);
				        queryGroupRepository.SubmitChanges();
	        ObjectTable JournalObjectTable = objectTables.ContainsKey("Journal") ? objectTables["Journal"] : null;
            if (JournalObjectTable == null)
            {
                IWebFreightContext objectContext = WebFreightContext.GetContext(0);  

                JournalObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Journal" && d.Tenant == 0).FirstOrDefault();
            }

	         
			List<Feature> addedFeatures = new List<Feature>();
			List<TextCode> addedTextCodes = new List<TextCode>();
			List<Query> addedQueries = new List<Query>();
			List<QueryColumn> addedQueryColumns = new List<QueryColumn>();
			List<AdvancedQueryFilter> addedQueryFilters = new List<AdvancedQueryFilter>();
   

			   TextCode JournalTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.Q.ExternalJournals", DefaultText = @"External Journals",LocalDefaultText = " פקודות יומן חיצוניות", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature JournalFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Journal.Q.ExternalJournals", ObjectTableId = JournalObjectTable.Id, Tenant = 0, NameTextCodeCode = "JournalFeatures.ExternalJournals", NameTextCodeDefaultText = "External Journals", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,JournalObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode JournalTextCode_1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.Q.AllJournals", DefaultText = @"All Journal",LocalDefaultText = "כל פקודות היומן", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature JournalFeature_1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Journal.Q.AllJournals", ObjectTableId = JournalObjectTable.Id, Tenant = 0, NameTextCodeCode = "JournalFeatures.AllJournals", NameTextCodeDefaultText = "All Journals", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,JournalObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode JournalTextCode_2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.Q.ApprovedJournals", DefaultText = @"Approved Journals",LocalDefaultText = "פקודות יומן מםושרות", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature JournalFeature_2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Journal.Q.ApprovedJournals", ObjectTableId = JournalObjectTable.Id, Tenant = 0, NameTextCodeCode = "JournalFeatures.ApprovedJournals", NameTextCodeDefaultText = "Approved Journals", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,JournalObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode JournalTextCode_3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.Q.DraftJournals", DefaultText = @"Draft Journals",LocalDefaultText = "פקודות יומן בסטטוס טיוטה", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature JournalFeature_3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Journal.Q.DraftJournals", ObjectTableId = JournalObjectTable.Id, Tenant = 0, NameTextCodeCode = "JournalFeatures.DraftJournals", NameTextCodeDefaultText = "Draft Journals", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,JournalObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode JournalTextCode_4 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.Q.SavedJournals", DefaultText = @"Waiting for Approval Journals",LocalDefaultText = "פקודות יומן שממתינות לםישור", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature JournalFeature_4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Journal.Q.SavedJournals", ObjectTableId = JournalObjectTable.Id, Tenant = 0, NameTextCodeCode = "JournalFeatures.SavedJournals", NameTextCodeDefaultText = "Saved Journals", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,JournalObjectTable, addedFeatures, addedTextCodes);

	        //TextCodeRepository.SubmitChanges();
	        //FeaturesRepository.SubmitChanges();    
	      

			  Query ExternalJournalsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = JournalTextCode_0.Id, NameTextCodeCode = JournalTextCode_0.Code, ObjectTableName = "Journal", Code = "External Journals",  QueryGroupCode = "JNAC", IndexOrder = 0, Tenant = 0, ObjectTableId = JournalObjectTable.Id, QuerySection = "Journal", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = JournalFeature_0.Id,FeatureUniqeCode= JournalFeature_0.FeatureUniqeCode, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", Perspective = null }, addedQueries);
	
			 QueryColumn ExternalJournalsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ExternalJournalsQuery.Id,QueryCode = ExternalJournalsQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Journal.JournalNumber" , ColumnWidth = 115 }, addedQueryColumns);

			 QueryColumn ExternalJournalsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ExternalJournalsQuery.Id,QueryCode = ExternalJournalsQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Journal.CreateDate" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn ExternalJournalsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ExternalJournalsQuery.Id,QueryCode = ExternalJournalsQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Journal.AccountingEntityName" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn ExternalJournalsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ExternalJournalsQuery.Id,QueryCode = ExternalJournalsQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Journal.AccountingDate" , ColumnWidth = 110 }, addedQueryColumns);

			 QueryColumn ExternalJournalsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ExternalJournalsQuery.Id,QueryCode = ExternalJournalsQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Journal.StatusName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn ExternalJournalsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ExternalJournalsQuery.Id,QueryCode = ExternalJournalsQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Journal.TypeName" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn ExternalJournalsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ExternalJournalsQuery.Id,QueryCode = ExternalJournalsQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Journal.CreatedByUserName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn ExternalJournalsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ExternalJournalsQuery.Id,QueryCode = ExternalJournalsQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "Journal.AccountingEntityReference" , ColumnWidth = 100 }, addedQueryColumns);

             AdvancedQueryFilter ExternalJournalsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Journal.ExternalSystem", PredefinedValue = "1",PredefinedValue2 = null, CustomPredefined = false, QueryId = ExternalJournalsQuery.Id,QueryCode = ExternalJournalsQuery.UniqueCode, Tenant = 0}, addedQueryFilters);

  
	      

			  Query AllJournalsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = JournalTextCode_1.Id, NameTextCodeCode = JournalTextCode_1.Code, ObjectTableName = "Journal", Code = "All Journals",  QueryGroupCode = "JNAC", IndexOrder = 1, Tenant = 0, ObjectTableId = JournalObjectTable.Id, QuerySection = "Journal", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = JournalFeature_1.Id,FeatureUniqeCode= JournalFeature_1.FeatureUniqeCode, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", Perspective = null }, addedQueries);
	
			 QueryColumn AllJournalsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllJournalsQuery.Id,QueryCode = AllJournalsQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Journal.JournalNumber" , ColumnWidth = 115 }, addedQueryColumns);

			 QueryColumn AllJournalsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllJournalsQuery.Id,QueryCode = AllJournalsQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Journal.CreateDate" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn AllJournalsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllJournalsQuery.Id,QueryCode = AllJournalsQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Journal.AccountingEntityName" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn AllJournalsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllJournalsQuery.Id,QueryCode = AllJournalsQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Journal.AccountingDate" , ColumnWidth = 110 }, addedQueryColumns);

			 QueryColumn AllJournalsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllJournalsQuery.Id,QueryCode = AllJournalsQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Journal.StatusName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn AllJournalsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllJournalsQuery.Id,QueryCode = AllJournalsQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Journal.TypeName" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn AllJournalsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllJournalsQuery.Id,QueryCode = AllJournalsQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Journal.AccountingEntityReference" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn AllJournalsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllJournalsQuery.Id,QueryCode = AllJournalsQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "Journal.CreatedByUserName" , ColumnWidth = 130 }, addedQueryColumns);
  
	      

			  Query ApprovedJournalsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = JournalTextCode_2.Id, NameTextCodeCode = JournalTextCode_2.Code, ObjectTableName = "Journal", Code = "Approved Journals",  QueryGroupCode = "JNAC", IndexOrder = 2, Tenant = 0, ObjectTableId = JournalObjectTable.Id, QuerySection = "Journal", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = JournalFeature_2.Id,FeatureUniqeCode= JournalFeature_2.FeatureUniqeCode, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", Perspective = null }, addedQueries);
	
			 QueryColumn ApprovedJournalsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovedJournalsQuery.Id,QueryCode = ApprovedJournalsQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Journal.JournalNumber" , ColumnWidth = 115 }, addedQueryColumns);

			 QueryColumn ApprovedJournalsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovedJournalsQuery.Id,QueryCode = ApprovedJournalsQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Journal.CreateDate" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn ApprovedJournalsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovedJournalsQuery.Id,QueryCode = ApprovedJournalsQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Journal.AccountingEntityName" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn ApprovedJournalsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovedJournalsQuery.Id,QueryCode = ApprovedJournalsQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Journal.AccountingDate" , ColumnWidth = 110 }, addedQueryColumns);

			 QueryColumn ApprovedJournalsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovedJournalsQuery.Id,QueryCode = ApprovedJournalsQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Journal.StatusName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn ApprovedJournalsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovedJournalsQuery.Id,QueryCode = ApprovedJournalsQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Journal.TypeName" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn ApprovedJournalsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovedJournalsQuery.Id,QueryCode = ApprovedJournalsQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Journal.AccountingEntityReference" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn ApprovedJournalsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovedJournalsQuery.Id,QueryCode = ApprovedJournalsQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "Journal.CreatedByUserName" , ColumnWidth = 130 }, addedQueryColumns);

             AdvancedQueryFilter ApprovedJournalsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Journal.StatusCode", PredefinedValue = "2",PredefinedValue2 = null, CustomPredefined = false, QueryId = ApprovedJournalsQuery.Id,QueryCode = ApprovedJournalsQuery.UniqueCode, Tenant = 0}, addedQueryFilters);

  
	      

			  Query DraftJournalsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = JournalTextCode_3.Id, NameTextCodeCode = JournalTextCode_3.Code, ObjectTableName = "Journal", Code = "Draft Journals",  QueryGroupCode = "JNAC", IndexOrder = 3, Tenant = 0, ObjectTableId = JournalObjectTable.Id, QuerySection = "Journal", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = JournalFeature_3.Id,FeatureUniqeCode= JournalFeature_3.FeatureUniqeCode, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", Perspective = null }, addedQueries);
	
			 QueryColumn DraftJournalsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftJournalsQuery.Id,QueryCode = DraftJournalsQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Journal.JournalNumber" , ColumnWidth = 115 }, addedQueryColumns);

			 QueryColumn DraftJournalsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftJournalsQuery.Id,QueryCode = DraftJournalsQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Journal.CreateDate" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn DraftJournalsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftJournalsQuery.Id,QueryCode = DraftJournalsQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Journal.AccountingEntityName" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn DraftJournalsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftJournalsQuery.Id,QueryCode = DraftJournalsQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Journal.AccountingDate" , ColumnWidth = 110 }, addedQueryColumns);

			 QueryColumn DraftJournalsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftJournalsQuery.Id,QueryCode = DraftJournalsQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Journal.StatusName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn DraftJournalsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftJournalsQuery.Id,QueryCode = DraftJournalsQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Journal.TypeName" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn DraftJournalsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftJournalsQuery.Id,QueryCode = DraftJournalsQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Journal.AccountingEntityReference" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn DraftJournalsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftJournalsQuery.Id,QueryCode = DraftJournalsQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "Journal.CreatedByUserName" , ColumnWidth = 130 }, addedQueryColumns);

             AdvancedQueryFilter DraftJournalsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Journal.StatusCode", PredefinedValue = "0",PredefinedValue2 = null, CustomPredefined = false, QueryId = DraftJournalsQuery.Id,QueryCode = DraftJournalsQuery.UniqueCode, Tenant = 0}, addedQueryFilters);

  
	      

			  Query SavedJournalsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = JournalTextCode_4.Id, NameTextCodeCode = JournalTextCode_4.Code, ObjectTableName = "Journal", Code = "Saved Journals",  QueryGroupCode = "JNAC", IndexOrder = 4, Tenant = 0, ObjectTableId = JournalObjectTable.Id, QuerySection = "Journal", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = JournalFeature_4.Id,FeatureUniqeCode= JournalFeature_4.FeatureUniqeCode, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", Perspective = null }, addedQueries);
	
			 QueryColumn SavedJournalsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SavedJournalsQuery.Id,QueryCode = SavedJournalsQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Journal.JournalNumber" , ColumnWidth = 115 }, addedQueryColumns);

			 QueryColumn SavedJournalsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SavedJournalsQuery.Id,QueryCode = SavedJournalsQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Journal.CreateDate" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn SavedJournalsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SavedJournalsQuery.Id,QueryCode = SavedJournalsQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Journal.AccountingEntityName" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn SavedJournalsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SavedJournalsQuery.Id,QueryCode = SavedJournalsQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Journal.AccountingDate" , ColumnWidth = 110 }, addedQueryColumns);

			 QueryColumn SavedJournalsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SavedJournalsQuery.Id,QueryCode = SavedJournalsQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Journal.StatusName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn SavedJournalsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SavedJournalsQuery.Id,QueryCode = SavedJournalsQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Journal.TypeName" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn SavedJournalsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SavedJournalsQuery.Id,QueryCode = SavedJournalsQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Journal.CreatedByUserName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn SavedJournalsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SavedJournalsQuery.Id,QueryCode = SavedJournalsQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "Journal.AccountingEntityReference" , ColumnWidth = 100 }, addedQueryColumns);

             AdvancedQueryFilter SavedJournalsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Journal.StatusCode", PredefinedValue = "1",PredefinedValue2 = null, CustomPredefined = false, QueryId = SavedJournalsQuery.Id,QueryCode = SavedJournalsQuery.UniqueCode, Tenant = 0}, addedQueryFilters);

			SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
			SqlBulkInsert.BulkInsert("Features", addedFeatures);
			SqlBulkInsert.BulkInsert("Queries", addedQueries);
			SqlBulkInsert.BulkInsert("QueryColumns", addedQueryColumns);
			SqlBulkInsert.BulkInsert("AdvancedQueryFilters", addedQueryFilters);	 
  
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable JournalObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Journal" && d.Tenant == 0).FirstOrDefault();
		   //List<ObjectField> JournalObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Journal").ToList();
		       
	      

	         Screen JournalGeneralTabScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Journal.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = JournalObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 3, IsReadOnly = false }, screensRepository, tenantScreens);
      
             ScreenField JournalGeneralTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ScreenId = JournalGeneralTabScreenScreen0.Id,ScreenCode = JournalGeneralTabScreenScreen0.Code, ObjectFieldCode = "Journal.CreateDate", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField JournalGeneralTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ScreenId = JournalGeneralTabScreenScreen0.Id,ScreenCode = JournalGeneralTabScreenScreen0.Code, ObjectFieldCode = "Journal.UpdateDate", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField JournalGeneralTabScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ScreenId = JournalGeneralTabScreenScreen0.Id,ScreenCode = JournalGeneralTabScreenScreen0.Code, ObjectFieldCode = "Journal.ApproveDate", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField JournalGeneralTabScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ScreenId = JournalGeneralTabScreenScreen0.Id,ScreenCode = JournalGeneralTabScreenScreen0.Code, ObjectFieldCode = "Journal.CreatedByUserId", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField JournalGeneralTabScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ScreenId = JournalGeneralTabScreenScreen0.Id,ScreenCode = JournalGeneralTabScreenScreen0.Code, ObjectFieldCode = "Journal.UpdatedByUserId", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField JournalGeneralTabScreenScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 2, ScreenId = JournalGeneralTabScreenScreen0.Id,ScreenCode = JournalGeneralTabScreenScreen0.Code, ObjectFieldCode = "Journal.ApprovedByUserId", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	            
	      

	         Screen JournalHeaderScreenScreen1 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Journal.HeaderScreen.HeaderScreen", Name = "Header Screen", ObjectTableId = JournalObjectTable.Id, NumberOfColumns = 5, NumberOfRows = 2, IsReadOnly = true }, screensRepository, tenantScreens);
      
             ScreenField JournalHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ScreenId = JournalHeaderScreenScreen1.Id,ScreenCode = JournalHeaderScreenScreen1.Code, ObjectFieldCode = "Journal.JournalNumber", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField JournalHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ScreenId = JournalHeaderScreenScreen1.Id,ScreenCode = JournalHeaderScreenScreen1.Code, ObjectFieldCode = "Journal.CreateDate", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField JournalHeaderScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ScreenId = JournalHeaderScreenScreen1.Id,ScreenCode = JournalHeaderScreenScreen1.Code, ObjectFieldCode = "Journal.StatusName", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField JournalHeaderScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ScreenId = JournalHeaderScreenScreen1.Id,ScreenCode = JournalHeaderScreenScreen1.Code, ObjectFieldCode = "Journal.CreatedByUserName", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField JournalHeaderScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ScreenId = JournalHeaderScreenScreen1.Id,ScreenCode = JournalHeaderScreenScreen1.Code, ObjectFieldCode = "Journal.OriginalJournalName", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField JournalHeaderScreenScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 1, ScreenId = JournalHeaderScreenScreen1.Id,ScreenCode = JournalHeaderScreenScreen1.Code, ObjectFieldCode = "Journal.ApproveDate", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField JournalHeaderScreenScreenField6 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ScreenId = JournalHeaderScreenScreen1.Id,ScreenCode = JournalHeaderScreenScreen1.Code, ObjectFieldCode = "Journal.AccountingEntityReference", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField JournalHeaderScreenScreenField7 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 1, ScreenId = JournalHeaderScreenScreen1.Id,ScreenCode = JournalHeaderScreenScreen1.Code, ObjectFieldCode = "Journal.AccountingEntityName", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField JournalHeaderScreenScreenField8 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 4, Row = 0, ScreenId = JournalHeaderScreenScreen1.Id,ScreenCode = JournalHeaderScreenScreen1.Code, ObjectFieldCode = "Journal.ExternalNo", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField JournalHeaderScreenScreenField9 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 4, Row = 1, ScreenId = JournalHeaderScreenScreen1.Id,ScreenCode = JournalHeaderScreenScreen1.Code, ObjectFieldCode = "Journal.ExternalSystem", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          	
		    JournalObjectTable.HeaderScreenId = JournalHeaderScreenScreen1.Id;
		    JournalObjectTable.HeaderScreenCode = JournalHeaderScreenScreen1.Code;

	   		  

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable JournalObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Journal" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode JournalDetailsTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.TH.Details", DefaultText = "Details",LocalDefaultText = "פרטים", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature JournalDetailsFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Journal.Tab.Details", ObjectTableId = JournalObjectTable.Id, Tenant = 0, NameTextCodeCode = "JournalFeatures.JNDT", NameTextCodeDefaultText = "Details", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,JournalObjectTable);
 
                 
			   TextCode JournalDebugTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.TH.Debug", DefaultText = "Debug",LocalDefaultText = "ניהול", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature JournalDebugFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Journal.Tab.Debug", ObjectTableId = JournalObjectTable.Id, Tenant = 0, NameTextCodeCode = "JournalFeatures.JNDB", NameTextCodeDefaultText = "Debug", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,JournalObjectTable);
 
                 
			   TextCode JournalEventsTextCode_TH2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.TH.Events", DefaultText = "Events",LocalDefaultText = "םירועים", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature JournalEventsFeature_TH2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Journal.Tab.Events", ObjectTableId = JournalObjectTable.Id, Tenant = 0, NameTextCodeCode = "JournalFeatures.JNEV", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,JournalObjectTable);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "JNDT",HtmlComponentName = "JournalDetailsTabComponent",HtmlComponentUrl = "./Accounting/Components/EditTabs/Journal/JournalDetailsTabComponent", FeatureId = JournalDetailsFeature_TH0.Id,FeatureUniqeCode = JournalDetailsFeature_TH0.FeatureUniqeCode, ControlPath = "Logitude.Accounting.Views.Tabs.JRNL.JournalDetailsTabControl", ObjectTableId = JournalObjectTable.Id, TabNameTextCodeId = JournalDetailsTextCode_TH0.Id, TabNameTextCodeCode = JournalDetailsTextCode_TH0.Code, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "JNDB",HtmlComponentName = "JournalDebugTabComponent",HtmlComponentUrl = "./Accounting/Components/EditTabs/Journal/JournalDebugTabComponent", FeatureId = JournalDebugFeature_TH1.Id,FeatureUniqeCode = JournalDebugFeature_TH1.FeatureUniqeCode, ControlPath = "Logitude.Accounting.Views.Tabs.JRNL.JournalDebugTabControl", ObjectTableId = JournalObjectTable.Id, TabNameTextCodeId = JournalDebugTextCode_TH1.Id, TabNameTextCodeCode = JournalDebugTextCode_TH1.Code, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "JNEV",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = JournalEventsFeature_TH2.Id,FeatureUniqeCode = JournalEventsFeature_TH2.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = JournalObjectTable.Id, TabNameTextCodeId = JournalEventsTextCode_TH2.Id, TabNameTextCodeCode = JournalEventsTextCode_TH2.Code, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable JournalObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Journal" && d.Tenant == 0).FirstOrDefault(); 

		   Feature JournalFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = JournalObjectTable.Id, Tenant = 0, NameTextCodeCode = "Journal.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,JournalObjectTable);
		   Feature JournalFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = JournalObjectTable.Id, Tenant = 0, NameTextCodeCode = "Journal.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,JournalObjectTable);
		   Feature JournalFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = JournalObjectTable.Id, Tenant = 0, NameTextCodeCode = "Journal.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,JournalObjectTable);
		   Feature JournalFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = false, ObjectTableId = JournalObjectTable.Id, Tenant = 0, NameTextCodeCode = "Journal.Features.PackageFeature", NameTextCodeDefaultText = "Journal Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,JournalObjectTable); 

		   		   //--------------> Additional Features <--------------\\

		   Feature JournalFeature_EVENTS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EVENTS", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = JournalObjectTable.Id, Tenant = 0, NameTextCodeCode = "Journal.Features.Events", NameTextCodeDefaultText = @"Events" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,JournalObjectTable);

		   Feature JournalFeature_DETAILS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DETAILS", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = JournalObjectTable.Id, Tenant = 0, NameTextCodeCode = "Journal.Features.Details", NameTextCodeDefaultText = @"Details" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,JournalObjectTable);

		   Feature JournalFeature_SAVEJOURNAL = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SAVEJOURNAL", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = JournalObjectTable.Id, Tenant = 0, NameTextCodeCode = "Journal.Features.Savejournal", NameTextCodeDefaultText = @"Save Journal" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,JournalObjectTable);

		   Feature JournalFeature_MOREJOURNAL = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MOREJOURNAL", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = JournalObjectTable.Id, Tenant = 0, NameTextCodeCode = "Journal.Features.Morejournal", NameTextCodeDefaultText = @"More" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,JournalObjectTable);

		   Feature JournalFeature_SAVEASDRAFTJOURNAL = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SAVEASDRAFTJOURNAL", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = JournalObjectTable.Id, Tenant = 0, NameTextCodeCode = "Journal.Features.SaveAsDraftJournal", NameTextCodeDefaultText = @"Save As Draft Journal" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,JournalObjectTable);

		   Feature JournalFeature_APPROVEJOURNAL = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "APPROVEJOURNAL", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = JournalObjectTable.Id, Tenant = 0, NameTextCodeCode = "Journal.Features.ApproveJournal", NameTextCodeDefaultText = @"Approve Journal" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,JournalObjectTable);

		   Feature JournalFeature_PRINTJOURNAL = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PRINTJOURNAL", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = JournalObjectTable.Id, Tenant = 0, NameTextCodeCode = "Journal.Features.JournalPrint", NameTextCodeDefaultText = @"Print Journal Button" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,JournalObjectTable);

		   Feature JournalFeature_JOURNAL = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "JOURNAL", FeatureTypeCode = "QUER", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = JournalObjectTable.Id, Tenant = 0, NameTextCodeCode = "Journal.Features.Journal", NameTextCodeDefaultText = @"All Journals" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,JournalObjectTable);

		   Feature JournalFeature_DraftJournal = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DraftJournal", FeatureTypeCode = "QUER", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = JournalObjectTable.Id, Tenant = 0, NameTextCodeCode = "Journal.Features.DraftJournal", NameTextCodeDefaultText = @"Draft Journals" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,JournalObjectTable);

		   Feature JournalFeature_SavedJournal = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SavedJournal", FeatureTypeCode = "QUER", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = JournalObjectTable.Id, Tenant = 0, NameTextCodeCode = "Journal.Features.SavedJournal", NameTextCodeDefaultText = @"Saved Journals" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,JournalObjectTable);

		   Feature JournalFeature_ApprovedJournal = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ApprovedJournal", FeatureTypeCode = "QUER", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = JournalObjectTable.Id, Tenant = 0, NameTextCodeCode = "Journal.Features.ApprovedJournal", NameTextCodeDefaultText = @"Approved Journals" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,JournalObjectTable);

		   Feature JournalFeature_ExternalJournals = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ExternalJournals", FeatureTypeCode = "QUER", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = JournalObjectTable.Id, Tenant = 0, NameTextCodeCode = "Journal.Features.ExternalJournals", NameTextCodeDefaultText = @"External Journals" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,JournalObjectTable);

		   Feature JournalFeature_LOADJOURNALCSV = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "LOADJOURNALCSV", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = JournalObjectTable.Id, Tenant = 0, NameTextCodeCode = "Journal.Features.LOADJOURNALCSV", NameTextCodeDefaultText = @"Load Journal From CSV" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,JournalObjectTable);

		   Feature JournalFeature_Journal_Feature_ManageSecurity = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Journal.Feature.ManageSecurity", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = JournalObjectTable.Id, Tenant = 0, NameTextCodeCode = "Journal.Features.Journal.Feature.ManageSecurity", NameTextCodeDefaultText = @"Manage Security" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,JournalObjectTable);

   
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable JournalObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Journal" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "JCL",
                EnglishName =  "Journal Closed",
                LocalName =  "Journal Closed",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = JournalObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "JVD",
                EnglishName =  "Journal Voided",
                LocalName =  "Journal Voided",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = JournalObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CREV",
                EnglishName =  "Created",
                LocalName =  "Created",
                IsManualEntry =  false,
                ShortView =  true,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = JournalObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "UPEV",
                EnglishName =  "Updated",
                LocalName =  "Updated",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = JournalObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "JCR",
                EnglishName =  "Journal Created",
                LocalName =  "Journal Created",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = JournalObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "JUP",
                EnglishName =  "Journal Updated",
                LocalName =  "Journal Updated",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = JournalObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "JSV",
                EnglishName =  "Journal Waiting For Approval",
                LocalName =  "Journal Waiting For Approval",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = JournalObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "JAP",
                EnglishName =  "Journal Approved",
                LocalName =  "Journal Approved",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = JournalObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CPJL",
                EnglishName =  "Copied from another Journal",
                LocalName =  "Copied from another Journal",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = JournalObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "JNUP",
                EnglishName =  "Journal Line Updated",
                LocalName =  "Journal Line Updated",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = JournalObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "JSUP",
                EnglishName =  "Journal Security Level Updated",
                LocalName =  "Journal Security Level Updated",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = JournalObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "JAI",
                EnglishName =  "Jouranl In approval process",
                LocalName =  "פקודה בתהליך םישור",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = JournalObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
		   //FeatureRepository featureRepository = new FeatureRepository(0); 
		   //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList(); 
		   ObjectTable JournalObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Journal" && d.Tenant == 0).FirstOrDefault();       
    
			   Feature JournalFeature_MB0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "JournalSaveAsDraft", ObjectTableId = JournalObjectTable.Id, Tenant = 0, NameTextCodeCode = "Journal.Features.SaveAsDraft", NameTextCodeDefaultText = "Save As Draft", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,JournalObjectTable);

      
    
			   Feature JournalFeature_MB1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "JournalSave", ObjectTableId = JournalObjectTable.Id, Tenant = 0, NameTextCodeCode = "Journal.Features.WaitingForApproval", NameTextCodeDefaultText = "Waiting For Approval", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,JournalObjectTable);

      
    
			   Feature JournalFeature_MB2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "JournalApprove", ObjectTableId = JournalObjectTable.Id, Tenant = 0, NameTextCodeCode = "Journal.Features.Approve", NameTextCodeDefaultText = "Approve", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,JournalObjectTable);

			   Feature JournalFeature_MB30 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "JournalVoid", ObjectTableId = JournalObjectTable.Id, Tenant = 0, NameTextCodeCode = "Journal.Features.Void", NameTextCodeDefaultText = "Void", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,JournalObjectTable);
             			   Feature JournalFeature_MB31 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "JournalPrint", ObjectTableId = JournalObjectTable.Id, Tenant = 0, NameTextCodeCode = "Journal.Features.Print", NameTextCodeDefaultText = "Print", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,JournalObjectTable);
             			   Feature JournalFeature_MB32 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CopyJournal", ObjectTableId = JournalObjectTable.Id, Tenant = 0, NameTextCodeCode = "Journal.Features.CopyJournal", NameTextCodeDefaultText = "Copy Journal", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,JournalObjectTable);
              

		   TextCodeRepository.SubmitChanges();
		   FeaturesRepository.SubmitChanges();
		   MenuButtonGroup JournalMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
				{
					MenuButtonGroupType = "JournalEdit",
					Name = "JournalEditButtonsGroup",
					ObjectTableId = JournalObjectTable.Id,
					Tenant = 0
				}, menuButtonGroupRepository, tenantMenuButtonGroups);        
   
			   MenuButton JournalMenuButton0 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "JournalSaveAsDraft",
						Index = 0, 
						IsActive = true,
						LabelTextCodeCode = "Journal.B.SaveAsDraft",
						LabelTextCodeDefaultText = "Save As Draft",
						Tenant = 0,
						MenuButtonGroupId = JournalMenuButtonGroup.Id,
						ObjectTableId = JournalObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = JournalFeature_MB0.Id,
						Style = null,
						LocalDefaultText = "שמור כטיוטה",
						FeatureUniqeCode = JournalFeature_MB0.FeatureUniqeCode,
						HtmlComponentPath = null,
						Width = 0,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton JournalMenuButton1 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "JournalSave",
						Index = 1, 
						IsActive = true,
						LabelTextCodeCode = "Journal.B.WaitingForApproval",
						LabelTextCodeDefaultText = "Waiting For Approval",
						Tenant = 0,
						MenuButtonGroupId = JournalMenuButtonGroup.Id,
						ObjectTableId = JournalObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = JournalFeature_MB1.Id,
						Style = null,
						LocalDefaultText = null,
						FeatureUniqeCode = JournalFeature_MB1.FeatureUniqeCode,
						HtmlComponentPath = null,
						Width = 0,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton JournalMenuButton2 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "JournalApprove",
						Index = 2, 
						IsActive = true,
						LabelTextCodeCode = "Journal.B.Approve",
						LabelTextCodeDefaultText = "Approve",
						Tenant = 0,
						MenuButtonGroupId = JournalMenuButtonGroup.Id,
						ObjectTableId = JournalObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = JournalFeature_MB2.Id,
						Style = "ApproveButtonStyle",
						LocalDefaultText = "םישור",
						FeatureUniqeCode = JournalFeature_MB2.FeatureUniqeCode,
						HtmlComponentPath = null,
						Width = 0,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton JournalMenuButton3 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "More",
						Index = 3, 
						IsActive = false,
						LabelTextCodeCode = "Journal.B.More",
						LabelTextCodeDefaultText = "More",
						Tenant = 0,
						MenuButtonGroupId = JournalMenuButtonGroup.Id,
						ObjectTableId = JournalObjectTable.Id,
						MenuButtonType = "dropdownbutton",
						FeatureId = null,
						Style = null,
						LocalDefaultText = "נוספים",
						FeatureUniqeCode = null,
						HtmlComponentPath = null,
						Width = 0,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);

			   MenuButton JournalMenuButton30 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "JournalVoid",
						Index = 0, 
						IsActive = true,
						LabelTextCodeCode = "Journal.B.Void",
						LabelTextCodeDefaultText = "Void",
						Tenant = 0,
						MenuButtonGroupId = JournalMenuButtonGroup.Id,
						ParentMenuButtonId = JournalMenuButton3.Id,
						ObjectTableId = JournalObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  JournalFeature_MB30.Id,
						Style = null,
						LocalDefaultText = "ביטול",
                        HtmlComponentPath=null,
                        Width=0,
						FeatureUniqeCode=  JournalFeature_MB30.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton JournalMenuButton31 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "JournalPrint",
						Index = 1, 
						IsActive = true,
						LabelTextCodeCode = "Journal.B.Print",
						LabelTextCodeDefaultText = "Print",
						Tenant = 0,
						MenuButtonGroupId = JournalMenuButtonGroup.Id,
						ParentMenuButtonId = JournalMenuButton3.Id,
						ObjectTableId = JournalObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  JournalFeature_MB31.Id,
						Style = null,
						LocalDefaultText = "הדפסה",
                        HtmlComponentPath=null,
                        Width=0,
						FeatureUniqeCode=  JournalFeature_MB31.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton JournalMenuButton32 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "CopyJournal",
						Index = 2, 
						IsActive = true,
						LabelTextCodeCode = "Journal.B.CopyJournal",
						LabelTextCodeDefaultText = "Copy Journal",
						Tenant = 0,
						MenuButtonGroupId = JournalMenuButtonGroup.Id,
						ParentMenuButtonId = JournalMenuButton3.Id,
						ObjectTableId = JournalObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  JournalFeature_MB32.Id,
						Style = null,
						LocalDefaultText = " שכפול פקודת יומן",
                        HtmlComponentPath=null,
                        Width=0,
						FeatureUniqeCode=  JournalFeature_MB32.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable JournalObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Journal" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode JournalTextCode_JournalOQueries = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.O.Queries", DefaultText = "Journal Queries",LocalDefaultText = @"שםילתות פקודות", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalORevelations = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.O.Revelations", DefaultText = "Revelations",LocalDefaultText = @"שערוך", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalORecent = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.O.Recent", DefaultText = "Recent Journals",LocalDefaultText = @"פקודות םחרונים", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalTHGeneral = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.TH.General", DefaultText = "General",LocalDefaultText = @"כללי", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_GeneralMCACCJournal = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.ACC.Journal", DefaultText = "Journal",LocalDefaultText = @"פקודת יומן", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalQJournal = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.Q.Journal", DefaultText = "All Journal",LocalDefaultText = @"כל פקודות היומן", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalQApprovedJournal = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.Q.ApprovedJournal", DefaultText = "Approved Journals",LocalDefaultText = @"פקודות יומן מםושרות", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalQDraftJournal = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.Q.DraftJournal", DefaultText = "Draft Journals",LocalDefaultText = @"פקודות יומן בסטטוס טיוטה", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalQSavedJournal = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.Q.SavedJournal", DefaultText = "Waiting for Approval Journals",LocalDefaultText = @"פקודות יומן מחכות לםישור", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalQAutoCreatedJournals = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.Q.AutoCreatedJournals", DefaultText = "Auto Created Journals",LocalDefaultText = @" פקודות יומן םוטומטיות", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalOTheAccountingDayMustBeInRange = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.O.TheAccountingDayMustBeInRange", DefaultText = "The accounting day must be in the range of the accounting month.",LocalDefaultText = @"היום החשבונםי שהוקלד םינו קיים בטווח ימי החודש החשבונםי", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalOAccDay = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.O.AccDay", DefaultText = "Acc. Day",LocalDefaultText = @"יום חשבונםי", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalSDetailsDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.S.Details.Details", DefaultText = "Details",LocalDefaultText = null, ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalCHLine = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.CH.Line", DefaultText = "Line",LocalDefaultText = null, ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalCHActionCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.CH.ActionCode", DefaultText = "Action Code",LocalDefaultText = null, ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalCHDocumentDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.CH.DocumentDate", DefaultText = "Document Date",LocalDefaultText = null, ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalCHDueDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.CH.DueDate", DefaultText = "Due Date",LocalDefaultText = null, ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalCHCreditAccountName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.CH.CreditAccountName", DefaultText = "C.Account",LocalDefaultText = null, ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalCHDebitAccountName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.CH.DebitAccountName", DefaultText = "D.Account",LocalDefaultText = null, ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalCHCurrency = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.CH.Currency", DefaultText = "Currency",LocalDefaultText = null, ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalCHLocalAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.CH.LocalAmount", DefaultText = "Amount (%InvoiceCurrencyCode)",LocalDefaultText = null, ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalCHForeignAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.CH.ForeignAmount", DefaultText = "F.Amount",LocalDefaultText = null, ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalCHReference1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.CH.Reference1", DefaultText = "Ref.1",LocalDefaultText = null, ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalCHReference2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.CH.Reference2", DefaultText = "Ref.2",LocalDefaultText = null, ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalCHReference3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.CH.Reference3", DefaultText = "Ref.3",LocalDefaultText = null, ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalCHNotes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.CH.Notes", DefaultText = "Notes",LocalDefaultText = null, ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalMForeignAmountNotZero = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.ForeignAmountNotZero", DefaultText = "Foreign amount is empty",LocalDefaultText = @"סכום במטח הינו חובה", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalMLocalAmountNotZero = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.LocalAmountNotZero", DefaultText = "Local amount is empty",LocalDefaultText = @"סכום במטבע מקומי הינו חובה ", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalMYouShouldHaveOneLineAtLeast = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.YouShouldHaveOneLineAtLeast", DefaultText = "There must be at least one journal line",LocalDefaultText = @"חובה להזין לפחות שורת פקודת יומן םחת", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalMCurrenyNotMatched = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.CurrenyNotMatched", DefaultText = "Account Currncy does not equal to selected currecy code",LocalDefaultText = @"מטבע הכרטיס לם תוםם םת המטבע הנבחר", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalMJournalAmountNotMatched = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.JournalAmountNotMatched", DefaultText = "Total debit amount must be equal to total credit amount. There is a difference of: ",LocalDefaultText = @" :סכום חובה שונה מסכום זכות. קיים הפרש של  ", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalMExchangeRateEmpty = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.ExchangeRateEmpty", DefaultText = "Exchange rate is not defined",LocalDefaultText = @"לם הוגדר שער המרה", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalMActionCodeCredit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.ActionCodeCredit", DefaultText = "Please select a credit account",LocalDefaultText = @"נם לבחור כרטיס זכות", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalMJLAccountingDateMustWithinJournalMonth = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.JLAccountingDateMustWithinJournalMonth", DefaultText = "Jornal Line Accounting Date must be within Accounting month of Journal",LocalDefaultText = @"תםריך בשורה חייב להיות בטווח של החודש החשבונםי של פ היומן", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalMFutureDateForbidden = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.FutureDateForbidden", DefaultText = "Future date is not allowed",LocalDefaultText = @"לם ניתן  להקליד תםריך עתידי ", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalMAccountIsBlocked = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.AccountIsBlocked", DefaultText = "GL Account (%name) is inactive",LocalDefaultText = @"כרטיס (%name) חסום", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalMActionCodeDebit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.ActionCodeDebit", DefaultText = "Please select a debit account",LocalDefaultText = @"םנם בחר כרטיס חובה", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalMActionCodeCreditAndCredit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.ActionCodeCreditAndCredit", DefaultText = "Please select a credit and a debit account",LocalDefaultText = @"םנם בחר כרטיס זכות וכרטיס חובה", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalMActionCodeNotMatched = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.ActionCodeNotMatched", DefaultText = "Action Code does not Matched the account you picked",LocalDefaultText = @"שגיםה", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalMActionCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.ActionCode", DefaultText = "Please Select Action Code",LocalDefaultText = @"םנם בחר ציין םת םת סוג השורה  חובה/זכות", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalMDocumentDateBiggerDueDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.DocumentDateBiggerDueDate", DefaultText = "Document date must be earlier then due date ",LocalDefaultText = @"התםריך החשבונםי חייב להיות מוקדם מתםריך הםסמכתם", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalMDueDateMustgreaterthancurrent = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.DueDateMustgreaterthancurrent", DefaultText = "Due Date ,Must be Equal or greater than current date ",LocalDefaultText = @"תםריך הפרעון צריך להיות גדול םו שווה מהתםריך הנוכחי", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalMJournalcurrencydoesnotexist = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.Journal.currencydoesnotexist", DefaultText = "Currency does not exist",LocalDefaultText = @"המטבע לם קיים", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalMControlAccountIdIsMust = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.ControlAccountIdIsMust", DefaultText = "Account Which is not a card must Control Account definition",LocalDefaultText = @"חשבון שםיננו כרטיס תפעולי חייב להיות כרטיס מרכז", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalMControlAccountIdIsNotMatch = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.ControlAccountIdIsNotMatch", DefaultText = "Control Account Is Not Match",LocalDefaultText = @"חשבון מרכז םיננו תוםם", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalMDueDateIsMust = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.DueDateIsMust", DefaultText = "Due Date Is Must",LocalDefaultText = @"תםריך פרעון הינו חובה", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalMFAMltiExchangerateNELA = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.FAMltiExchangerateNELA", DefaultText = "Foreign amount ({0}) multiplied by the exchange rate ({1}) does not equal the local amount ({2})",LocalDefaultText = @"סכום במטבע מקומי חייב להיות שווה לסכום במטבע זר כפול שער המרה", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalMAllDateMustInit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.AllDateMustInit", DefaultText = "All the dates must have a value",LocalDefaultText = @"חייבים להזין םת כל שדות התםריכים", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalMYouShouldSelectTwoTransactions = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.YouShouldSelectTwoTransactions", DefaultText = "You should select at lease two transactions in order to create new reconcile",LocalDefaultText = @"יש לבחור לפחות שתי תנועות על מנת ליצור התםמה חדשה", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalOCodeShort = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.O.CodeShort", DefaultText = "Journal Code Too Short",LocalDefaultText = @"פרט מכס קצר מידי", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalOCodeLong = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.O.CodeLong", DefaultText = "Journal Code Too Long",LocalDefaultText = @"פרט המכס םרוך מדי", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalOCorrectDigit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.O.CorrectDigit", DefaultText = "Check digit is incorrect ,the correct digit is ",LocalDefaultText = @" ספרת הביקורת שגויה , הספרה הנכונה הים ", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalOCopy = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.O.Copy", DefaultText = "Copy",LocalDefaultText = @"העתק", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalODates = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.O.Dates", DefaultText = "Dates",LocalDefaultText = @"תםריכים", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalOAmountsAndCurrencies = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.O.AmountsAndCurrencies", DefaultText = "Amounts / Currencies",LocalDefaultText = @"סכומים \ מטבעות", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalOReferencesAndNotes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.O.ReferencesAndNotes", DefaultText = "References / Notes",LocalDefaultText = @"םסמכתםות \ הערות", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalOExchangeRateValidation = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.O.ExchangeRateValidation", DefaultText = "There is no exchange rate for the selected currency on the accounting date for line",LocalDefaultText = @"לם הוגדר שער חליפין של המטבע שנבחר בתםריך החשבונםי שצויין בשורה", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalOhaveFutureAccountingorReferenceDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.O.haveFutureAccountingorReferenceDate", DefaultText = "Can't approve The Journal . Some Lines have Future Accounting or Reference Date.",LocalDefaultText = @"לם ניתן לםשר םת פקודת היומן , ישנם שורות עם תםריך חשבונםי/םסמכתם עתידי.", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalMEditJournalLine = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.EditJournalLine", DefaultText = "Edit Journal Line",LocalDefaultText = @"ערוך שורה", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalMNote = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.Note", DefaultText = "Note",LocalDefaultText = @"הערת", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalMLine = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.Line", DefaultText = "Line",LocalDefaultText = @"שורה", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalOLoadCsv = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.O.LoadCsv", DefaultText = "Load Journal from CSV",LocalDefaultText = @"טען פקודת יומן מ CSV", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalMForeignDiffLocalAmountButTenantCurrency = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.M.ForeignDiffLocalAmountButTenantCurrency", DefaultText = "Although the currency is accounting currency,The foreign amount is different from local amount ",LocalDefaultText = @"למרות שהמטבע היינו מטבע חשבונםי הסכום במטז שונה מסכום מקומי", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalOConfirmVoidJournal = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.O.ConfirmVoidJournal", DefaultText = "Are you sure you want to cancel this Journal?",LocalDefaultText = @"םנם םשר םת ביטול פקודת היומן  ", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalODifferenceExchangeRate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.O.DifferenceExchangeRate", DefaultText = "Difference between new and old value is more than 5 Percent Exchange rate",LocalDefaultText = @"השער החדש קטן\גדול ביותר מחמישה םחוזים מהשער הקודם", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalREAccountingDateRequired = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.RE.AccountingDateRequired", DefaultText = "Accounting Date is Required",LocalDefaultText = @"תםריך חשבונםי הינו שדה חובה", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalREAccountingDateConfrimation = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.RE.AccountingDateConfrimation", DefaultText = "Leaving the accounting date / due date/ reference date/references/note  empty will create adjustment journals with dates/references/note taken from the original ones. If you want to continue click ok. If not click cancel and fill the date/references/note fields",LocalDefaultText = "במידה ולם קיים ערך בשדות: תםריך חשבונםי/םסמכתם/פירעון םסמכתםות הערות הערכים ילקחו מהשורות שסומנו להתםמה. להמשיך הקש ''םישור'' לחזרה להשלמת הערכים הקש ''חזור", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalREAccountingDateCancellation = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.RE.AccountingDateCancellation", DefaultText = "Cancel",LocalDefaultText = @"חזור", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalREAdjustMulti1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.RE.AdjustMulti1", DefaultText = "Please note the selected account ( XXX ) is Multi Currency ",LocalDefaultText = @"לידיעתך הכרטיס הנבחר ( XXX ) שהיינו רב מטבעי", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalREAdjustMulti2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.RE.AdjustMulti2", DefaultText = "Linked to the card ( XXX ) whose currency is YYY",LocalDefaultText = @"מקושר לכרטיס ( XXX ) שמטבעו YYY", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalREAdjustMulti3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.RE.AdjustMulti3", DefaultText = "Therefore the order will be registered on the linked card, should you continue",LocalDefaultText = @"ולכן הפקודה תירשם על הכרטיס המקושר , הםם להמשיך", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalREReconcilePeriodClosed = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.RE.ReconcilePeriodClosed", DefaultText = "Accounting date is closed for line (X)",LocalDefaultText = @"התקופה החשבונםית לשורה (X) סגורה, יש לפתוח תחילה םת התקופה החשבונםית.", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalOCoefficientForAmountsAndCurrencies = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.O.CoefficientForAmountsAndCurrencies", DefaultText = "Coefficient",LocalDefaultText = @"מקדם", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalOSecurityHigherThanUsers = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.O.SecurityHigherThanUsers", DefaultText = "Cannot set Journal Viewing Security Level higher than the User Security Level",LocalDefaultText = @"לם ניתן לתת רמת םבטחת לצפיה בפקודת היומן שגבוהה מרמת הםבטחה שקיימת למשתמש", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalOViewingNotAuthorized = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.O.ViewingNotAuthorized", DefaultText = "You are not authorized to view Journal No. ",LocalDefaultText = @"םינך מורשה לצפיה בפקודה מספר ", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode JournalTextCode_JournalOPermissionSetting = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Journal.O.PermissionSetting", DefaultText = "Permission Setting",LocalDefaultText = @"הגדרת הרשםה", ObjectTableId = JournalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 