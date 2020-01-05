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

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.InvoiceModel.EntityUpdateClasses
{
   public class AccountingTransferHeaderUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "AccountingTransferHeader",
			      				    IsNew =  false,
			      				    DBTableName =  "AccountingTransferHeaders",
			      				    OldDBTableName =  "AccountingTransferHeaders",
			      				    ObjectTableSingular =  "Accounting Transfer",
			      				    ObjectTablePlural =  "Accounting Transfers",
			      				    HasCustomFilter =  true,
			      				    HasCustomFields =  false,
			      				    HasHelper =  true,
			      				    HasShortTitle =  false,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  true,
			      				    IsNewWizard =  true,
			      				    KeyPropertyPath =  "Id",
			      				    AutoCompleteSearchWindow =  false,
			      				    IsClosed =  false,
			      				    CacheOnClient =  false,
			      				    EditableFromAutoCompleteWindow =  false,
			      				    HasCounter =  true,
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
			      				    ObjectTableTypeCode =  "BR",
			      				    MaxNumberOfCustomFields =  0,
			      				    NewWizardControlName =  "Simplog.InvoiceLib.NewAccountingTransferCommand",
			      				    DefaultText =  "Accounting Transfer",
			      				    Code =  "ACTH",
			      				    Name =  "Accounting Transfer Headers",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Invoice",
			      				    NoTS =  false,
			      				    HasMenuButtons =  false,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    IsTabsHidden =  false,
			      				    SearchFields =  "AccountingTransferHeader,AccountingTransferHeaders,Simplog.InvoiceLib.NewAccountingTransferCommand,Id,",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TransferNumber",
					  						OldFieldName =  "TransferNumber",
					  						ObjectTableName =  "AccountingTransferHeader",
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
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TransferNumber",
					  						ListPropertyPath =  "TransferNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "TransferNumber",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TransferNumber",
					  						DefaultText =  "Transfer Number",
					  						ListFieldLable =  "TransferNumberListLable",
					  						ListLableDefaultText =  "Transfer No",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "TransferNumber",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TransferDate",
					  						OldFieldName =  "TransferDate",
					  						ObjectTableName =  "AccountingTransferHeader",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						DataTemplateName =  "TransferDateDataTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TransferDate",
					  						ListPropertyPath =  "TransferDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "TransferDate",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TransferDate",
					  						DefaultText =  "Transfer Date",
					  						ListFieldLable =  "TransferDateListLable",
					  						ListLableDefaultText =  "Transfer Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "TransferDate",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FileName",
					  						OldFieldName =  "FileName",
					  						ObjectTableName =  "AccountingTransferHeader",
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
					  						PMPropertyPath =  "FileName",
					  						ListPropertyPath =  "FileName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "FileName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "FileName",
					  						DefaultText =  "File",
					  						ListFieldLable =  "FileNameListLable",
					  						ListLableDefaultText =  "File Name",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "FileName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UserId",
					  						OldFieldName =  "UserId",
					  						ObjectTableName =  "AccountingTransferHeader",
					  						FieldsDataType =  "LookUp",
					  						MinLength =  0,
					  						MaxLength =  15,
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "UserId",
					  						ListPropertyPath =  "UserId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "UserId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UserId",
					  						DefaultText =  "User",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "UserId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AccountingTransferTypeCode",
					  						OldFieldName =  "AccountingTransferTypeCode",
					  						ObjectTableName =  "AccountingTransferHeader",
					  						FieldsDataType =  "LookUp",
					  						MinLength =  0,
					  						MaxLength =  4,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
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
					  						PMPropertyPath =  "AccountingTransferTypeCode",
					  						ListPropertyPath =  "AccountingTransferTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "AccountingTransferTypeCode",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AccountingTransferTypeCode",
					  						DefaultText =  "Transfer Type",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "AccountingTransferTypeCode",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SearchFields",
					  						OldFieldName =  "SearchFields",
					  						ObjectTableName =  "AccountingTransferHeader",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  1000,
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
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "SearchFields",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SearchFields",
					  						DefaultText =  "Search..",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "SearchFields",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UserName",
					  						OldFieldName =  "UserName",
					  						ObjectTableName =  "AccountingTransferHeader",
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
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "UserName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UserName",
					  						DefaultText =  "User",
					  						ListFieldLable =  "UserNameListLable",
					  						ListLableDefaultText =  "User",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "UserName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AccountingTransferTypeName",
					  						OldFieldName =  "AccountingTransferTypeName",
					  						ObjectTableName =  "AccountingTransferHeader",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AccountingTransferTypeName",
					  						ListPropertyPath =  "AccountingTransferTypeName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "AccountingTransferTypeName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AccountingTransferTypeName",
					  						DefaultText =  "Transfer Type",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "AccountingTransferTypeName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ARInvoiceTransferHistory",
					  						OldFieldName =  "ARInvoiceTransferHistory",
					  						ObjectTableName =  "AccountingTransferHeader",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ARInvoiceTransferHistory",
					  						ListPropertyPath =  "ARInvoiceTransferHistory",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ARInvoiceTransferHistory",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ARInvoiceTransferHistory",
					  						DefaultText =  "A/R Invoice Transfer History",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "ARInvoiceTransferHistory",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "APInvoiceTransferHistory",
					  						OldFieldName =  "APInvoiceTransferHistory",
					  						ObjectTableName =  "AccountingTransferHeader",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "APInvoiceTransferHistory",
					  						ListPropertyPath =  "APInvoiceTransferHistory",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "APInvoiceTransferHistory",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "APInvoiceTransferHistory",
					  						DefaultText =  "A/P Invoice Transfer History",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "APInvoiceTransferHistory",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Notes",
					  						OldFieldName =  "Notes",
					  						ObjectTableName =  "AccountingTransferHeader",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  250,
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
					  						PMPropertyPath =  "Notes",
					  						ListPropertyPath =  "Notes",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AccountingTransferHeader",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "Notes",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Notes",
					  						DefaultText =  "Notes",
					  						ListFieldLable =  "NotesListLable",
					  						ListLableDefaultText =  "Notes",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Notes",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "APPaymentTransferHistory",
					  						OldFieldName =  "APPaymentTransferHistory",
					  						ObjectTableName =  "AccountingTransferHeader",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "APPaymentTransferHistory",
					  						ListPropertyPath =  "APPaymentTransferHistory",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "APPaymentTransferHistory",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "APPaymentTransferHistory",
					  						DefaultText =  "A/P Payment Transfer History",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "APPaymentTransferHistory",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ARPaymentTransferHistory",
					  						OldFieldName =  "ARPaymentTransferHistory",
					  						ObjectTableName =  "AccountingTransferHeader",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ARPaymentTransferHistory",
					  						ListPropertyPath =  "ARPaymentTransferHistory",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ARPaymentTransferHistory",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ARPaymentTransferHistory",
					  						DefaultText =  "A/R Payment Transfer History",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "ARPaymentTransferHistory",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	    {  
	        FeatureRepository featureRepository = new FeatureRepository(0); 
            //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
            IWebFreightContext objectContext = WebFreightContext.GetContext(0);  
	        QueryGroup AccountingTransferHeaderQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "ACTH", Name = "Accounting Transfer Headers" }, queryGroupRepository);
						QueryGroup AccountingTransferHeaderQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "fb08", Name = " Query Group" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable AccountingTransferHeaderObjectTable = objectContext.ObjectTables.Where(d => d.Name == "AccountingTransferHeader" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> AccountingTransferHeaderObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "AccountingTransferHeader").ToList();   

			   TextCode AccountingTransferHeaderTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AccountingTransferHeader.Q.ARInvoiceTransferHistory", DefaultText = @"Transfer history",LocalDefaultText = null, ObjectTableId = AccountingTransferHeaderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature AccountingTransferHeaderFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ARInvoiceTransferHistory", ObjectTableId = AccountingTransferHeaderObjectTable.Id, Tenant = 0, NameTextCodeCode = "AccountingTransferHeader.Features.ARInvoiceTransferHistory", NameTextCodeDefaultText = "A/R Invoice Transfer History", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode AccountingTransferHeaderTextCode_1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AccountingTransferHeader.Q.APInvoiceTransferHistory", DefaultText = @"Transfer history",LocalDefaultText = null, ObjectTableId = AccountingTransferHeaderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature AccountingTransferHeaderFeature_1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "APInvoiceTransferHistory", ObjectTableId = AccountingTransferHeaderObjectTable.Id, Tenant = 0, NameTextCodeCode = "AccountingTransferHeader.Features.APInvoiceTransferHistory", NameTextCodeDefaultText = "A/P Invoice Transfer History", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode AccountingTransferHeaderTextCode_2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AccountingTransferHeader.Q.ARPaymentTransferHistory", DefaultText = @"Transfer history",LocalDefaultText = null, ObjectTableId = AccountingTransferHeaderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature AccountingTransferHeaderFeature_2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ARPaymentTransferHistory", ObjectTableId = AccountingTransferHeaderObjectTable.Id, Tenant = 0, NameTextCodeCode = "AccountingTransferHeader.Features.ARPaymentTransferHistory", NameTextCodeDefaultText = "A/R Payment Transfer History", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode AccountingTransferHeaderTextCode_3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AccountingTransferHeader.Q.APPaymentTransferHistory", DefaultText = @"Transfer history",LocalDefaultText = null, ObjectTableId = AccountingTransferHeaderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature AccountingTransferHeaderFeature_3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "APPaymentTransferHistory", ObjectTableId = AccountingTransferHeaderObjectTable.Id, Tenant = 0, NameTextCodeCode = "AccountingTransferHeader.Features.APPaymentTransferHistory", NameTextCodeDefaultText = "A/P Payment Transfer History", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query ARInvoiceTransferHistoryQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = AccountingTransferHeaderTextCode_0.Id, NameTextCodeCode = AccountingTransferHeaderTextCode_0.Code, Code = "ARInvoiceTransferHistory",  QueryGroupCode = "ACTH", IndexOrder = 0, Tenant = 0, ObjectTableId = AccountingTransferHeaderObjectTable.Id, QuerySection = "AccountingTransferHeader", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = AccountingTransferHeaderFeature_0.Id, DefaultSortName = "TransferDate", DefaultSortDirection = "Descending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn ARInvoiceTransferHistoryQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ARInvoiceTransferHistoryQuery.Id, IndexOrder = 0, ObjectFieldId = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "TransferNumber" && d.ObjectTableId == AccountingTransferHeaderObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "TransferNumber" && d.ObjectTableId == AccountingTransferHeaderObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 110 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ARInvoiceTransferHistoryQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ARInvoiceTransferHistoryQuery.Id, IndexOrder = 1, ObjectFieldId = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "TransferDate" && d.ObjectTableId == AccountingTransferHeaderObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "TransferDate" && d.ObjectTableId == AccountingTransferHeaderObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 140 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ARInvoiceTransferHistoryQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ARInvoiceTransferHistoryQuery.Id, IndexOrder = 2, ObjectFieldId = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "FileName" && d.ObjectTableId == AccountingTransferHeaderObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "FileName" && d.ObjectTableId == AccountingTransferHeaderObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter ARInvoiceTransferHistoryQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "ARInvoiceTransferHistory" && d.ObjectTableId == AccountingTransferHeaderObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "ARInvoiceTransferHistory" && d.ObjectTableId == AccountingTransferHeaderObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "true",PredefinedValue2 = null, QueryId = ARInvoiceTransferHistoryQuery.Id, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query APInvoiceTransferHistoryQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = AccountingTransferHeaderTextCode_1.Id, NameTextCodeCode = AccountingTransferHeaderTextCode_1.Code, Code = "APInvoiceTransferHistory",  QueryGroupCode = "ACTH", IndexOrder = 1, Tenant = 0, ObjectTableId = AccountingTransferHeaderObjectTable.Id, QuerySection = "AccountingTransferHeader", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = AccountingTransferHeaderFeature_1.Id, DefaultSortName = "TransferDate", DefaultSortDirection = "Descending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn APInvoiceTransferHistoryQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = APInvoiceTransferHistoryQuery.Id, IndexOrder = 0, ObjectFieldId = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "TransferNumber" && d.ObjectTableId == AccountingTransferHeaderObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "TransferNumber" && d.ObjectTableId == AccountingTransferHeaderObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 110 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn APInvoiceTransferHistoryQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = APInvoiceTransferHistoryQuery.Id, IndexOrder = 1, ObjectFieldId = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "TransferDate" && d.ObjectTableId == AccountingTransferHeaderObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "TransferDate" && d.ObjectTableId == AccountingTransferHeaderObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 140 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn APInvoiceTransferHistoryQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = APInvoiceTransferHistoryQuery.Id, IndexOrder = 2, ObjectFieldId = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "FileName" && d.ObjectTableId == AccountingTransferHeaderObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "FileName" && d.ObjectTableId == AccountingTransferHeaderObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter APInvoiceTransferHistoryQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "APInvoiceTransferHistory" && d.ObjectTableId == AccountingTransferHeaderObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "APInvoiceTransferHistory" && d.ObjectTableId == AccountingTransferHeaderObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "true",PredefinedValue2 = null, QueryId = APInvoiceTransferHistoryQuery.Id, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query ARPaymentTransferHistoryQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = AccountingTransferHeaderTextCode_2.Id, NameTextCodeCode = AccountingTransferHeaderTextCode_2.Code, Code = "ARPaymentTransferHistory",  QueryGroupCode = "ACTH", IndexOrder = 2, Tenant = 0, ObjectTableId = AccountingTransferHeaderObjectTable.Id, QuerySection = "AccountingTransferHeader", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = AccountingTransferHeaderFeature_2.Id, DefaultSortName = "TransferDate", DefaultSortDirection = "Descending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn ARPaymentTransferHistoryQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ARPaymentTransferHistoryQuery.Id, IndexOrder = 0, ObjectFieldId = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "TransferNumber" && d.ObjectTableId == AccountingTransferHeaderObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "TransferNumber" && d.ObjectTableId == AccountingTransferHeaderObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 110 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ARPaymentTransferHistoryQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ARPaymentTransferHistoryQuery.Id, IndexOrder = 1, ObjectFieldId = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "TransferDate" && d.ObjectTableId == AccountingTransferHeaderObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "TransferDate" && d.ObjectTableId == AccountingTransferHeaderObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 140 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ARPaymentTransferHistoryQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ARPaymentTransferHistoryQuery.Id, IndexOrder = 2, ObjectFieldId = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "FileName" && d.ObjectTableId == AccountingTransferHeaderObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "FileName" && d.ObjectTableId == AccountingTransferHeaderObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter ARPaymentTransferHistoryQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "ARPaymentTransferHistory" && d.ObjectTableId == AccountingTransferHeaderObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "ARPaymentTransferHistory" && d.ObjectTableId == AccountingTransferHeaderObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "true",PredefinedValue2 = null, QueryId = ARPaymentTransferHistoryQuery.Id, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query APPaymentTransferHistoryQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = AccountingTransferHeaderTextCode_3.Id, NameTextCodeCode = AccountingTransferHeaderTextCode_3.Code, Code = "APPaymentTransferHistory",  QueryGroupCode = "ACTH", IndexOrder = 3, Tenant = 0, ObjectTableId = AccountingTransferHeaderObjectTable.Id, QuerySection = "AccountingTransferHeader", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = AccountingTransferHeaderFeature_3.Id, DefaultSortName = "TransferDate", DefaultSortDirection = "Descending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn APPaymentTransferHistoryQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = APPaymentTransferHistoryQuery.Id, IndexOrder = 0, ObjectFieldId = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "TransferNumber" && d.ObjectTableId == AccountingTransferHeaderObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "TransferNumber" && d.ObjectTableId == AccountingTransferHeaderObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 110 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn APPaymentTransferHistoryQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = APPaymentTransferHistoryQuery.Id, IndexOrder = 1, ObjectFieldId = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "TransferDate" && d.ObjectTableId == AccountingTransferHeaderObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "TransferDate" && d.ObjectTableId == AccountingTransferHeaderObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 140 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn APPaymentTransferHistoryQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = APPaymentTransferHistoryQuery.Id, IndexOrder = 2, ObjectFieldId = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "FileName" && d.ObjectTableId == AccountingTransferHeaderObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "FileName" && d.ObjectTableId == AccountingTransferHeaderObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter APPaymentTransferHistoryQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "APPaymentTransferHistory" && d.ObjectTableId == AccountingTransferHeaderObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "APPaymentTransferHistory" && d.ObjectTableId == AccountingTransferHeaderObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "true",PredefinedValue2 = null, QueryId = APPaymentTransferHistoryQuery.Id, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);

	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable AccountingTransferHeaderObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "AccountingTransferHeader" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> AccountingTransferHeaderObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "AccountingTransferHeader").ToList();
		       
	      

	         Screen AccountingTransferHeaderHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "AccountingTransfer.HeaderScreen", Name = "Header Screen", ObjectTableId = AccountingTransferHeaderObjectTable.Id, NumberOfColumns = 3, NumberOfRows = 2, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField AccountingTransferHeaderAccountingTransferHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "TransferNumber").FirstOrDefault().Id, ScreenId = AccountingTransferHeaderHeaderScreenScreen0.Id, ObjectFieldCode = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "TransferNumber").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField AccountingTransferHeaderAccountingTransferHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "TransferDate").FirstOrDefault().Id, ScreenId = AccountingTransferHeaderHeaderScreenScreen0.Id, ObjectFieldCode = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "TransferDate").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField AccountingTransferHeaderAccountingTransferHeaderScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "FileName").FirstOrDefault().Id, ScreenId = AccountingTransferHeaderHeaderScreenScreen0.Id, ObjectFieldCode = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "FileName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField AccountingTransferHeaderAccountingTransferHeaderScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ObjectFieldId = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "UserName").FirstOrDefault().Id, ScreenId = AccountingTransferHeaderHeaderScreenScreen0.Id, ObjectFieldCode = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "UserName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField AccountingTransferHeaderAccountingTransferHeaderScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "AccountingTransferTypeName").FirstOrDefault().Id, ScreenId = AccountingTransferHeaderHeaderScreenScreen0.Id, ObjectFieldCode = AccountingTransferHeaderObjectFields.Where(d => d.FieldName == "AccountingTransferTypeName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    AccountingTransferHeaderObjectTable.HeaderScreenId = AccountingTransferHeaderHeaderScreenScreen0.Id;
	   		  

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable AccountingTransferHeaderObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "AccountingTransferHeader" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode AccountingTransferHeaderDetailsTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AccountingTransferHeader.TH.Details", DefaultText = "Details",LocalDefaultText = null, ObjectTableId = AccountingTransferHeaderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature AccountingTransferHeaderDetailsFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DETAILS", ObjectTableId = AccountingTransferHeaderObjectTable.Id, Tenant = 0, NameTextCodeCode = "AccountingTransferHeader.Features.Details", NameTextCodeDefaultText = "Details", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode AccountingTransferHeaderEventsTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AccountingTransferHeader.TH.Events", DefaultText = "Events",LocalDefaultText = null, ObjectTableId = AccountingTransferHeaderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature AccountingTransferHeaderEventsFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EVENTS", ObjectTableId = AccountingTransferHeaderObjectTable.Id, Tenant = 0, NameTextCodeCode = "AccountingTransferHeader.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "ATDT",HtmlComponentName = "",HtmlComponentUrl = "./Invoice/Components/EditTabs/TransferHeader/TransferHeaderDetailsTabComponent", FeatureId = AccountingTransferHeaderDetailsFeature_TH0.Id, ControlPath = "Simplog.InvoiceLib.Views.Tabs.AccountingTransferTabs.AccountingTransferDetailsTab", ObjectTableId = AccountingTransferHeaderObjectTable.Id, TabNameTextCodeId = AccountingTransferHeaderDetailsTextCode_TH0.Id, TabNameTextCodeCode = AccountingTransferHeaderDetailsTextCode_TH0.Code, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "ATET",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = AccountingTransferHeaderEventsFeature_TH1.Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = AccountingTransferHeaderObjectTable.Id, TabNameTextCodeId = AccountingTransferHeaderEventsTextCode_TH1.Id, TabNameTextCodeCode = AccountingTransferHeaderEventsTextCode_TH1.Code, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable AccountingTransferHeaderObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "AccountingTransferHeader" && d.Tenant == 0).FirstOrDefault(); 

		   Feature AccountingTransferHeaderFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = true, ObjectTableId = AccountingTransferHeaderObjectTable.Id, Tenant = 0, NameTextCodeCode = "AccountingTransferHeader.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature AccountingTransferHeaderFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = true, ObjectTableId = AccountingTransferHeaderObjectTable.Id, Tenant = 0, NameTextCodeCode = "AccountingTransferHeader.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature AccountingTransferHeaderFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = true, ObjectTableId = AccountingTransferHeaderObjectTable.Id, Tenant = 0, NameTextCodeCode = "AccountingTransferHeader.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature AccountingTransferHeaderFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = true, ObjectTableId = AccountingTransferHeaderObjectTable.Id, Tenant = 0, NameTextCodeCode = "AccountingTransferHeader.Features.PackageFeature", NameTextCodeDefaultText = "AccountingTransferHeader Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes); 

		   		   //--------------> Additional Features <--------------\\

		   Feature AccountingTransferHeaderFeature_REBUILD = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "REBUILD", FeatureTypeCode = "ACT", Packagable = false, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = AccountingTransferHeaderObjectTable.Id, Tenant = 0, NameTextCodeCode = "AccountingTransferHeader.Features.Rebuild", NameTextCodeDefaultText = @"Rebuild" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature AccountingTransferHeaderFeature_QuickbooksConnectAuth1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "QuickbooksConnectAuth1", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = AccountingTransferHeaderObjectTable.Id, Tenant = 0, NameTextCodeCode = "AccountingTransferHeader.Features.QuickbooksConnectAuth1", NameTextCodeDefaultText = @"Quickbooks Connect Auth1" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

   
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable AccountingTransferHeaderObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "AccountingTransferHeader" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CAAH",
                EnglishName =  "Created",
                LocalName =  "Created",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = AccountingTransferHeaderObjectTable.Id,
				 
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
	 