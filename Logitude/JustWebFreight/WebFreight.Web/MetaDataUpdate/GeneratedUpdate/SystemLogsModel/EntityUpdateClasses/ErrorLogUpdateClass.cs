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

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.SystemLogsModel.EntityUpdateClasses
{
   public class ErrorLogUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "ErrorLog",
			      				    IsNew =  false,
			      				    DBTableName =  "ErrorLogs",
			      				    OldDBTableName =  "ErrorLogs",
			      				    ObjectTableSingular =  "Error Log",
			      				    ObjectTablePlural =  "Error Log",
			      				    HasCustomFilter =  false,
			      				    HasCustomFields =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  true,
			      				    IsNewWizard =  false,
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
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    DefaultText =  "Error Log",
			      				    Code =  "ERLG",
			      				    Name =  "Error Log",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Infrastructure",
			      				    NoTS =  false,
			      				    HasMenuButtons =  false,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    IsTabsHidden =  false,
			      				    SearchFields =  "ErrorLog,ErrorLogs,,,",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UserName",
					  						OldFieldName =  "UserName",
					  						ObjectTableName =  "ErrorLog",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "UserName",
					  						ListPropertyPath =  "UserName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ErrorLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UserName",
					  						DefaultText =  "User Name",
					  						ListFieldLable =  "UserNameListLable",
					  						ListLableDefaultText =  "UserName",
					  						IsMaxLength =  false,
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
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Tenant",
					  						OldFieldName =  "Tenant",
					  						ObjectTableName =  "ErrorLog",
					  						FieldsDataType =  "Integer",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
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
					  						ValidForQuerySection1 =  "ErrorLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Tenant",
					  						DefaultText =  "Tenant",
					  						ListFieldLable =  "TenantListLable",
					  						ListLableDefaultText =  "Tenant",
					  						IsMaxLength =  false,
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
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Tier",
					  						OldFieldName =  "Tier",
					  						ObjectTableName =  "ErrorLog",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Tier",
					  						ListPropertyPath =  "Tier",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  true,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ErrorLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Tier",
					  						DefaultText =  "Tier",
					  						ListFieldLable =  "TierListLable",
					  						ListLableDefaultText =  "Tier",
					  						IsMaxLength =  false,
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
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Exception",
					  						OldFieldName =  "Exception",
					  						ObjectTableName =  "ErrorLog",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  7000,
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
					  						Operator =  "StartsWith",
					  						MultiLine =  true,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Exception",
					  						ListPropertyPath =  "Exception",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  true,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ErrorLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Exception",
					  						DefaultText =  "Exception",
					  						ListFieldLable =  "ExceptionListLable",
					  						ListLableDefaultText =  "Exception",
					  						IsMaxLength =  false,
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
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Id",
					  						OldFieldName =  "Id",
					  						ObjectTableName =  "ErrorLog",
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
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
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
					  						ValidForQuerySection1 =  "ErrorLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Id",
					  						DefaultText =  "Id",
					  						IsMaxLength =  false,
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
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LogDate",
					  						OldFieldName =  "LogDate",
					  						ObjectTableName =  "ErrorLog",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "LogDate",
					  						ListPropertyPath =  "LogDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ErrorLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LogDate",
					  						DefaultText =  "Log Date",
					  						ListFieldLable =  "LogDateListLable",
					  						ListLableDefaultText =  "Log Date",
					  						IsMaxLength =  false,
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
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ClientDate",
					  						OldFieldName =  "ClientDate",
					  						ObjectTableName =  "ErrorLog",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ClientDate",
					  						ListPropertyPath =  "ClientDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ErrorLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ClientDate",
					  						DefaultText =  "Client Date",
					  						ListFieldLable =  "ClientDateListLable",
					  						ListLableDefaultText =  "Client Date",
					  						IsMaxLength =  false,
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
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "StackTrace",
					  						OldFieldName =  "StackTrace",
					  						ObjectTableName =  "ErrorLog",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  7000,
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
					  						Operator =  "StartsWith",
					  						MultiLine =  true,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "StackTrace",
					  						ListPropertyPath =  "StackTrace",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  true,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ErrorLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "StackTrace",
					  						DefaultText =  "StackTrace",
					  						ListFieldLable =  "StackTraceListLable",
					  						ListLableDefaultText =  "StackTrace",
					  						IsMaxLength =  false,
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
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SearchFields",
					  						OldFieldName =  "SearchFields",
					  						ObjectTableName =  "ErrorLog",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  8000,
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
					  						Operator =  "Contains",
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
					  						ValidForQuerySection1 =  "ErrorLog",
					  						ValidForQuerySection2 =  "ErrorLogFollowUp",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SearchFields",
					  						DefaultText =  "Search Tenant / User Name / Tier / Exception",
					  						HelpTextCode =  "SearchFields",
					  						HelpTextDefaultText =  "Searching by :\n1: Tenant \n2: UserName \n3: Tier \n4: Exception",
					  						IsMaxLength =  false,
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
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IP",
					  						OldFieldName =  "IP",
					  						ObjectTableName =  "ErrorLog",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IP",
					  						ListPropertyPath =  "IP",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ErrorLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IP",
					  						DefaultText =  "IP",
					  						ListFieldLable =  "IPListLable",
					  						ListLableDefaultText =  "IPTrace",
					  						IsMaxLength =  false,
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
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	    {  
	        FeatureRepository featureRepository = new FeatureRepository(0); 
            //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
            IWebFreightContext objectContext = WebFreightContext.GetContext(0);  
	        QueryGroup ErrorLogQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "ERLG", Name = "Error Log" }, queryGroupRepository);
						QueryGroup ErrorLogQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "3c56", Name = " Query Group" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable ErrorLogObjectTable = objectContext.ObjectTables.Where(d => d.Name == "ErrorLog" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> ErrorLogObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "ErrorLog").ToList();   

			   TextCode ErrorLogTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ErrorLog.Q.TodayErrorLog", DefaultText = @"Today",LocalDefaultText = null, ObjectTableId = ErrorLogObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature ErrorLogFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TODAYERRORLOG", ObjectTableId = ErrorLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "ErrorLogObjectTable.Features.TodayErrorLog", NameTextCodeDefaultText = "Today Error Log", FeatureTypeCode = "QUER", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode ErrorLogTextCode_1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ErrorLog.Q.AllErrorLog", DefaultText = @"All Error Log",LocalDefaultText = null, ObjectTableId = ErrorLogObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature ErrorLogFeature_1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ALLERRORLOG", ObjectTableId = ErrorLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "ErrorLogObjectTable.Features.AllErrorLog", NameTextCodeDefaultText = "All Error Log", FeatureTypeCode = "QUER", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query TodayErrorLogsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = ErrorLogTextCode_0.Id, NameTextCodeCode = ErrorLogTextCode_0.Code, Code = "Today Error Logs",  QueryGroupCode = "ERLG", IndexOrder = 0, Tenant = 0, ObjectTableId = ErrorLogObjectTable.Id, QuerySection = "ErrorLog", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = ErrorLogFeature_0.Id,FeatureUniqeCode= ErrorLogFeature_0.FeatureUniqeCode, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn TodayErrorLogsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayErrorLogsQuery.Id, IndexOrder = 0, ObjectFieldId = ErrorLogObjectFields.Where(d => d.FieldName == "UserName" && d.ObjectTableId == ErrorLogObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ErrorLogObjectFields.Where(d => d.FieldName == "UserName" && d.ObjectTableId == ErrorLogObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn TodayErrorLogsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayErrorLogsQuery.Id, IndexOrder = 1, ObjectFieldId = ErrorLogObjectFields.Where(d => d.FieldName == "LogDate" && d.ObjectTableId == ErrorLogObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ErrorLogObjectFields.Where(d => d.FieldName == "LogDate" && d.ObjectTableId == ErrorLogObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn TodayErrorLogsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayErrorLogsQuery.Id, IndexOrder = 2, ObjectFieldId = ErrorLogObjectFields.Where(d => d.FieldName == "ClientDate" && d.ObjectTableId == ErrorLogObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ErrorLogObjectFields.Where(d => d.FieldName == "ClientDate" && d.ObjectTableId == ErrorLogObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn TodayErrorLogsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayErrorLogsQuery.Id, IndexOrder = 3, ObjectFieldId = ErrorLogObjectFields.Where(d => d.FieldName == "Tier" && d.ObjectTableId == ErrorLogObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ErrorLogObjectFields.Where(d => d.FieldName == "Tier" && d.ObjectTableId == ErrorLogObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn TodayErrorLogsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayErrorLogsQuery.Id, IndexOrder = 4, ObjectFieldId = ErrorLogObjectFields.Where(d => d.FieldName == "Exception" && d.ObjectTableId == ErrorLogObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ErrorLogObjectFields.Where(d => d.FieldName == "Exception" && d.ObjectTableId == ErrorLogObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn TodayErrorLogsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayErrorLogsQuery.Id, IndexOrder = 5, ObjectFieldId = ErrorLogObjectFields.Where(d => d.FieldName == "StackTrace" && d.ObjectTableId == ErrorLogObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ErrorLogObjectFields.Where(d => d.FieldName == "StackTrace" && d.ObjectTableId == ErrorLogObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn TodayErrorLogsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayErrorLogsQuery.Id, IndexOrder = 6, ObjectFieldId = ErrorLogObjectFields.Where(d => d.FieldName == "Tenant" && d.ObjectTableId == ErrorLogObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ErrorLogObjectFields.Where(d => d.FieldName == "Tenant" && d.ObjectTableId == ErrorLogObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter TodayErrorLogsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = ErrorLogObjectFields.Where(d => d.FieldName == "LogDate" && d.ObjectTableId == ErrorLogObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ErrorLogObjectFields.Where(d => d.FieldName == "LogDate" && d.ObjectTableId == ErrorLogObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "Today",PredefinedValue2 = null, QueryId = TodayErrorLogsQuery.Id, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query AllErrorLogsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = ErrorLogTextCode_1.Id, NameTextCodeCode = ErrorLogTextCode_1.Code, Code = "All Error Logs",  QueryGroupCode = "ERLG", IndexOrder = 1, Tenant = 0, ObjectTableId = ErrorLogObjectTable.Id, QuerySection = "ErrorLog", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = ErrorLogFeature_1.Id,FeatureUniqeCode= ErrorLogFeature_1.FeatureUniqeCode, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn AllErrorLogsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllErrorLogsQuery.Id, IndexOrder = 0, ObjectFieldId = ErrorLogObjectFields.Where(d => d.FieldName == "UserName" && d.ObjectTableId == ErrorLogObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ErrorLogObjectFields.Where(d => d.FieldName == "UserName" && d.ObjectTableId == ErrorLogObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllErrorLogsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllErrorLogsQuery.Id, IndexOrder = 1, ObjectFieldId = ErrorLogObjectFields.Where(d => d.FieldName == "LogDate" && d.ObjectTableId == ErrorLogObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ErrorLogObjectFields.Where(d => d.FieldName == "LogDate" && d.ObjectTableId == ErrorLogObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllErrorLogsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllErrorLogsQuery.Id, IndexOrder = 2, ObjectFieldId = ErrorLogObjectFields.Where(d => d.FieldName == "ClientDate" && d.ObjectTableId == ErrorLogObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ErrorLogObjectFields.Where(d => d.FieldName == "ClientDate" && d.ObjectTableId == ErrorLogObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllErrorLogsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllErrorLogsQuery.Id, IndexOrder = 3, ObjectFieldId = ErrorLogObjectFields.Where(d => d.FieldName == "Tier" && d.ObjectTableId == ErrorLogObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ErrorLogObjectFields.Where(d => d.FieldName == "Tier" && d.ObjectTableId == ErrorLogObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllErrorLogsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllErrorLogsQuery.Id, IndexOrder = 4, ObjectFieldId = ErrorLogObjectFields.Where(d => d.FieldName == "Exception" && d.ObjectTableId == ErrorLogObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ErrorLogObjectFields.Where(d => d.FieldName == "Exception" && d.ObjectTableId == ErrorLogObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllErrorLogsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllErrorLogsQuery.Id, IndexOrder = 5, ObjectFieldId = ErrorLogObjectFields.Where(d => d.FieldName == "StackTrace" && d.ObjectTableId == ErrorLogObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ErrorLogObjectFields.Where(d => d.FieldName == "StackTrace" && d.ObjectTableId == ErrorLogObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllErrorLogsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllErrorLogsQuery.Id, IndexOrder = 6, ObjectFieldId = ErrorLogObjectFields.Where(d => d.FieldName == "Tenant" && d.ObjectTableId == ErrorLogObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ErrorLogObjectFields.Where(d => d.FieldName == "Tenant" && d.ObjectTableId == ErrorLogObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable ErrorLogObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "ErrorLog" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> ErrorLogObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "ErrorLog").ToList();
		       
	      

	         Screen ErrorLogHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ErrorLog.HeaderScreen", Name = "Header Screen", ObjectTableId = ErrorLogObjectTable.Id, NumberOfColumns = 4, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField ErrorLogErrorLogHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = ErrorLogObjectFields.Where(d => d.FieldName == "UserName").FirstOrDefault().Id, ScreenId = ErrorLogHeaderScreenScreen0.Id,ScreenCode = ErrorLogHeaderScreenScreen0.Code, ObjectFieldCode = ErrorLogObjectFields.Where(d => d.FieldName == "UserName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ErrorLogErrorLogHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = ErrorLogObjectFields.Where(d => d.FieldName == "LogDate").FirstOrDefault().Id, ScreenId = ErrorLogHeaderScreenScreen0.Id,ScreenCode = ErrorLogHeaderScreenScreen0.Code, ObjectFieldCode = ErrorLogObjectFields.Where(d => d.FieldName == "LogDate").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ErrorLogErrorLogHeaderScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = ErrorLogObjectFields.Where(d => d.FieldName == "Tier").FirstOrDefault().Id, ScreenId = ErrorLogHeaderScreenScreen0.Id,ScreenCode = ErrorLogHeaderScreenScreen0.Code, ObjectFieldCode = ErrorLogObjectFields.Where(d => d.FieldName == "Tier").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ErrorLogErrorLogHeaderScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ObjectFieldId = ErrorLogObjectFields.Where(d => d.FieldName == "ClientDate").FirstOrDefault().Id, ScreenId = ErrorLogHeaderScreenScreen0.Id,ScreenCode = ErrorLogHeaderScreenScreen0.Code, ObjectFieldCode = ErrorLogObjectFields.Where(d => d.FieldName == "ClientDate").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    ErrorLogObjectTable.HeaderScreenId = ErrorLogHeaderScreenScreen0.Id;
		    ErrorLogObjectTable.HeaderScreenCode = ErrorLogHeaderScreenScreen0.Code;

	   		  
	      

	         Screen ErrorLogGeneralTabScreenScreen1 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ErrorLog.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = ErrorLogObjectTable.Id, NumberOfColumns = 1, NumberOfRows = 2, IsReadOnly = false }, screensRepository, tenantScreens);
      
            ScreenField ErrorLogErrorLogGeneralTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = ErrorLogObjectFields.Where(d => d.FieldName == "ClientDate").FirstOrDefault().Id, ScreenId = ErrorLogGeneralTabScreenScreen1.Id,ScreenCode = ErrorLogGeneralTabScreenScreen1.Code, ObjectFieldCode = ErrorLogObjectFields.Where(d => d.FieldName == "ClientDate").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ErrorLogErrorLogGeneralTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = ErrorLogObjectFields.Where(d => d.FieldName == "IP").FirstOrDefault().Id, ScreenId = ErrorLogGeneralTabScreenScreen1.Id,ScreenCode = ErrorLogGeneralTabScreenScreen1.Code, ObjectFieldCode = ErrorLogObjectFields.Where(d => d.FieldName == "IP").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
           

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable ErrorLogObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "ErrorLog" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode ErrorLogExceptionTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ErrorLog.TH.Exception", DefaultText = "Exception",LocalDefaultText = null, ObjectTableId = ErrorLogObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature ErrorLogExceptionFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EXCEPTION", ObjectTableId = ErrorLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "ErrorLogObjectTable.Features.Exception", NameTextCodeDefaultText = "Exception", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode ErrorLogGeneralTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ErrorLog.TH.General", DefaultText = "General",LocalDefaultText = null, ObjectTableId = ErrorLogObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature ErrorLogGeneralFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GENERAL", ObjectTableId = ErrorLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "ErrorLogObjectTable.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode ErrorLogEventsTextCode_TH2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ErrorLog.TH.Events", DefaultText = "Events",LocalDefaultText = null, ObjectTableId = ErrorLogObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature ErrorLogEventsFeature_TH2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EVENTS", ObjectTableId = ErrorLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "ErrorLogObjectTable.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "ELEX",HtmlComponentName = "ErrorLogExceptionComponent",HtmlComponentUrl = "./InfrastructureModules/InfrastructureOthers/Components/ErrorLog/ErrorLogExceptionComponent", FeatureId = ErrorLogExceptionFeature_TH0.Id,FeatureUniqeCode = ErrorLogExceptionFeature_TH0.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.MaintenanceControls.ErrorLogException", ObjectTableId = ErrorLogObjectTable.Id, TabNameTextCodeId = ErrorLogExceptionTextCode_TH0.Id, TabNameTextCodeCode = ErrorLogExceptionTextCode_TH0.Code, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "ELGC",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = ErrorLogGeneralFeature_TH1.Id,FeatureUniqeCode = ErrorLogGeneralFeature_TH1.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.GeneralControls.GeneralTabControl", ObjectTableId = ErrorLogObjectTable.Id, TabNameTextCodeId = ErrorLogGeneralTextCode_TH1.Id, TabNameTextCodeCode = ErrorLogGeneralTextCode_TH1.Code, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "ELEV",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = ErrorLogEventsFeature_TH2.Id,FeatureUniqeCode = ErrorLogEventsFeature_TH2.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = ErrorLogObjectTable.Id, TabNameTextCodeId = ErrorLogEventsTextCode_TH2.Id, TabNameTextCodeCode = ErrorLogEventsTextCode_TH2.Code, Tenant = 0, IndexOrder = 2 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable ErrorLogObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "ErrorLog" && d.Tenant == 0).FirstOrDefault(); 

		   Feature ErrorLogFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = true, ObjectTableId = ErrorLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "ErrorLog.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature ErrorLogFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = true, ObjectTableId = ErrorLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "ErrorLog.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature ErrorLogFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = true, ObjectTableId = ErrorLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "ErrorLog.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature ErrorLogFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = true, ObjectTableId = ErrorLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "ErrorLog.Features.PackageFeature", NameTextCodeDefaultText = "ErrorLog Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes); 

		   		   //--------------> Additional Features <--------------\\

		   Feature ErrorLogFeature_ALLERRORLOGS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ALLERRORLOGS", FeatureTypeCode = "QUER", Packagable = true, IsBusinessUnitEnabled = false, IsOld = true, IsCoreFeature = false, ObjectTableId = ErrorLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "ErrorLogObjectTable.Features.AllErrorLog", NameTextCodeDefaultText = @"All Error Log" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

   
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable ErrorLogObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "ErrorLog" && d.Tenant == 0).FirstOrDefault(); 
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
                ObjectTableId = ErrorLogObjectTable.Id,
				 
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
                ObjectTableId = ErrorLogObjectTable.Id,
				 
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
	 