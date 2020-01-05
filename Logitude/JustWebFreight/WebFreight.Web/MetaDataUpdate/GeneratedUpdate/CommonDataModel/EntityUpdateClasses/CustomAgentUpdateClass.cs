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

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.CommonDataModel.EntityUpdateClasses
{
   public class CustomAgentUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "CustomAgent",
			      				    IsNew =  false,
			      				    DBTableName =  "CustomAgents",
			      				    OldDBTableName =  "CustomAgents",
			      				    ObjectTableSingular =  "Custom Agent",
			      				    ObjectTablePlural =  "Custom Agents",
			      				    DescriptionDefaultText =  "Add and manage information about customs agents you work with. Specify communication, address and billing details, and create list of contacts.",
			      				    HasCustomFilter =  true,
			      				    HasCustomFields =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  true,
			      				    IsNewWizard =  true,
			      				    KeyPropertyPath =  "Id",
			      				    AutoCompleteSearchWindow =  false,
			      				    IsClosed =  false,
			      				    CacheOnClient =  false,
			      				    EditableFromAutoCompleteWindow =  true,
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
			      				    NewWizardControlName =  "Simplog.FreightLib.NewCustomAgentCommand",
			      				    DefaultText =  "Custom Agent",
			      				    Code =  "CAGT",
			      				    Name =  "Custom Agents",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Common",
			      				    NewWizardComponentPath =  "./CommonModules/CommonPartners/Components/NewEntity/NewCustomAgentComponent",
			      				    NoTS =  false,
			      				    HasMenuButtons =  true,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    IsTabsHidden =  false,
			      				    SearchFields =  "CustomAgent,CustomAgents,Simplog.FreightLib.NewCustomAgentCommand,Id,",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InvoiceCurrencyId",
					  						OldFieldName =  "InvoiceCurrencyId",
					  						ObjectTableName =  "CustomAgent",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Currency",
					  						MinLength =  0,
					  						MaxLength =  15,
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "InvoiceCurrencyId",
					  						ListPropertyPath =  "InvoiceCurrencyId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomAgent",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "InvoiceCurrencyId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "InvoiceCurrencyId",
					  						DefaultText =  "Invoice Currency",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "InvoiceCurrencyId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "VatTypeId",
					  						OldFieldName =  "VatTypeId",
					  						ObjectTableName =  "CustomAgent",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "VatType",
					  						MinLength =  0,
					  						MaxLength =  15,
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "VatTypeId",
					  						ListPropertyPath =  "VatTypeId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomAgent",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "VatTypeId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "VatTypeId",
					  						DefaultText =  "VAT Type",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "VatTypeId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BankName",
					  						OldFieldName =  "BankName",
					  						ObjectTableName =  "CustomAgent",
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
					  						PMPropertyPath =  "BankName",
					  						ListPropertyPath =  "BankName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomAgent",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "BankName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "BankName",
					  						DefaultText =  "Bank Name",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "BankName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BankAddress",
					  						OldFieldName =  "BankAddress",
					  						ObjectTableName =  "CustomAgent",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  100,
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
					  						PMPropertyPath =  "BankAddress",
					  						ListPropertyPath =  "BankAddress",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomAgent",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "BankAddress",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "BankAddress",
					  						DefaultText =  "Bank Address",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "BankAddress",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Swift",
					  						OldFieldName =  "Swift",
					  						ObjectTableName =  "CustomAgent",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Swift",
					  						ListPropertyPath =  "Swift",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomAgent",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "Swift",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Swift",
					  						DefaultText =  "Swift",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Swift",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AccountNumber",
					  						OldFieldName =  "AccountNumber",
					  						ObjectTableName =  "CustomAgent",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  25,
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
					  						ValidForQuerySection1 =  "CustomAgent",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "AccountNumber",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AccountNumber",
					  						DefaultText =  "Bank Account Number",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "AccountNumber",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IBANNumber",
					  						OldFieldName =  "IBANNumber",
					  						ObjectTableName =  "CustomAgent",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  30,
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
					  						PMPropertyPath =  "IBANNumber",
					  						ListPropertyPath =  "IBANNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomAgents",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "IBANNumber",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IBANNumber",
					  						DefaultText =  "IBAN No.",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "IBANNumber",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "EnableConsolidationInvoices",
					  						OldFieldName =  "EnableConsolidationInvoices",
					  						ObjectTableName =  "CustomAgent",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  1,
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
					  						PMPropertyPath =  "EnableConsolidationInvoices",
					  						ListPropertyPath =  "EnableConsolidationInvoices",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomAgent",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "EnableConsolidationInvoices",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "EnableConsolidationInvoices",
					  						DefaultText =  "Consolidated Inv.",
					  						HelpTextCode =  "EnableConsolidationInvoices",
					  						HelpTextDefaultText =  "This client will receive one Invoice for multiple Shipments once a period",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Website",
					  						OldFieldName =  "Website",
					  						ObjectTableName =  "CustomAgent",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  100,
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Website",
					  						ListPropertyPath =  "Website",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomAgent",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "Website",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Website",
					  						DefaultText =  "Website",
					  						ListFieldLable =  "WebsiteListLable",
					  						ListLableDefaultText =  "Website",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Website",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Code",
					  						OldFieldName =  "Code",
					  						ObjectTableName =  "CustomAgent",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  true,
					  						PMPropertyPath =  "Code",
					  						ListPropertyPath =  "Code",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  true,
					  						UniqueField =  true,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomAgent",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "Code",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  true,
					  						DisplayInSearchWindowFiltersIndex =  1,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
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
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Code",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "EnglishName",
					  						OldFieldName =  "EnglishName",
					  						ObjectTableName =  "CustomAgent",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  70,
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  true,
					  						PMPropertyPath =  "EnglishName",
					  						ListPropertyPath =  "EnglishName",
					  						DisplayInLookUpIndex =  1,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  1,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomAgent",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "EnglishName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  true,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "EnglishName",
					  						DefaultText =  "Name",
					  						ListFieldLable =  "EnglishNameListLable",
					  						ListLableDefaultText =  "Name",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Name",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LocalName",
					  						OldFieldName =  "LocalName",
					  						ObjectTableName =  "CustomAgent",
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
					  						SystemMaxLength =  0,
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
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomAgent",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						Code =  "LocalName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LocalName",
					  						DefaultText =  "Local Name",
					  						ListFieldLable =  "LocalNameListLable",
					  						ListLableDefaultText =  "Local Name",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "LocalName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ComputedLocalName",
					  						OldFieldName =  "ComputedLocalName",
					  						ObjectTableName =  "CustomAgent",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  100,
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
					  						PMPropertyPath =  "ComputedLocalName",
					  						ListPropertyPath =  "ComputedLocalName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomAgent",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ComputedLocalName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ComputedLocalName",
					  						DefaultText =  "Local Name",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "ComputedLocalName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InActive",
					  						OldFieldName =  "InActive",
					  						ObjectTableName =  "CustomAgent",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  15,
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "InActive",
					  						ListPropertyPath =  "InActive",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomAgent",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "InActive",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "InActive",
					  						DefaultText =  "Inactive Customs Agent",
					  						ListFieldLable =  "InActiveListLable",
					  						ListLableDefaultText =  "Inactive",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "InActive",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "VatNumber",
					  						OldFieldName =  "VatNumber",
					  						ObjectTableName =  "CustomAgent",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  20,
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "VatNumber",
					  						ListPropertyPath =  "VatNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomAgent",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "VatNumber",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "VatNumber",
					  						DefaultText =  "VAT No.",
					  						ListFieldLable =  "VatNumberListLable",
					  						ListLableDefaultText =  "VAT Number",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "VatNumber",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AccountingCard",
					  						OldFieldName =  "AccountingCard",
					  						ObjectTableName =  "CustomAgent",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  25,
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AccountingCard",
					  						ListPropertyPath =  "AccountingCard",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomAgent",
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
					  						FullFieldLable =  "AccountingCard",
					  						DefaultText =  "External ID",
					  						ListFieldLable =  "AccountingCardListLable",
					  						ListLableDefaultText =  "External ID",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "AccountingCard",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PaymentTermId",
					  						OldFieldName =  "PaymentTermId",
					  						ObjectTableName =  "CustomAgent",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "PaymentTerm",
					  						MinLength =  0,
					  						MaxLength =  15,
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PaymentTermId",
					  						ListPropertyPath =  "PaymentTermId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomAgent",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "PaymentTermId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PaymentTermId",
					  						DefaultText =  "Payment Term",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "PaymentTermId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PaymentTermEnglishName",
					  						OldFieldName =  "PaymentTermEnglishName",
					  						ObjectTableName =  "CustomAgent",
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
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Card.PaymentTerm.EnglishName",
					  						ListPropertyPath =  "PaymentTermEnglishName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomAgent",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "PaymentTermEnglishName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PaymentTermEnglishName",
					  						DefaultText =  "Payment Term",
					  						ListFieldLable =  "PaymentTermEnglishNameListLable",
					  						ListLableDefaultText =  "Payment Term",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "PaymentTermEnglishName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Notes",
					  						OldFieldName =  "Notes",
					  						ObjectTableName =  "CustomAgent",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  2000,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						DataTemplateName =  "NotesDataTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  true,
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
					  						ValidForQuerySection1 =  "CustomAgent",
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
					 
					 						FieldName =  "CityName",
					  						OldFieldName =  "CityName",
					  						ObjectTableName =  "CustomAgent",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  25,
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CityName",
					  						ListPropertyPath =  "CityName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomAgent",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "CityName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CityName",
					  						DefaultText =  "City",
					  						FullLocalDefaultText =  "CityName",
					  						ListFieldLable =  "CityNameListLable",
					  						ListLableDefaultText =  "City",
					  						ListLocalDefaultText =  "CityName",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "CityName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CountryId",
					  						OldFieldName =  "CountryId",
					  						ObjectTableName =  "CustomAgent",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Country",
					  						MinLength =  0,
					  						MaxLength =  15,
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CountryId",
					  						ListPropertyPath =  "CountryId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomAgent",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "CountryId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CountryId",
					  						DefaultText =  "Country",
					  						FullLocalDefaultText =  "CountryId",
					  						ListFieldLable =  "CountryIdListLable",
					  						ListLableDefaultText =  "CountryId",
					  						ListLocalDefaultText =  "CountryId",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "CountryId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CountryName",
					  						OldFieldName =  "CountryName",
					  						ObjectTableName =  "CustomAgent",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  120,
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CountryName",
					  						ListPropertyPath =  "CountryName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomAgent",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						Code =  "CountryName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CountryName",
					  						DefaultText =  "Country",
					  						FullLocalDefaultText =  "CountryName",
					  						ListFieldLable =  "CountryNameListLable",
					  						ListLableDefaultText =  "Country",
					  						ListLocalDefaultText =  "CountryName",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "CountryName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "MetodoPagoCode",
					  						OldFieldName =  "MetodoPagoCode",
					  						ObjectTableName =  "CustomAgent",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "MetodoPago",
					  						MinLength =  0,
					  						MaxLength =  3,
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "MetodoPagoCode",
					  						ListPropertyPath =  "MetodoPagoCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomAgent",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "MetodoPagoCode",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "MetodoPagoCode",
					  						DefaultText =  "Metodo Pago",
					  						ListFieldLable =  "MetodoPagoCodeListLable",
					  						ListLableDefaultText =  "Metodo Pago",
					  						ListLocalDefaultText =  "Metodo Pago",
					  						HelpTextCode =  "MetodoPagoCode",
					  						HelpTextDefaultText =  "Way to Pay:\n-Pago en una sola exhibición (PUE): payment closed at once in one single payment type performed prior to the issuance of the invoice\n-Pago en parcialidades o diferido (PPD): partial payment or deferred performed after the issuance of the invoice",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UsoCFDICode",
					  						OldFieldName =  "UsoCFDICode",
					  						ObjectTableName =  "CustomAgent",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "UsoCFDI",
					  						MinLength =  0,
					  						MaxLength =  3,
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "UsoCFDICode",
					  						ListPropertyPath =  "UsoCFDICode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomAgent",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "UsoCFDICode",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UsoCFDICode",
					  						DefaultText =  "UsoCFDI",
					  						ListFieldLable =  "UsoCFDICodeListLable",
					  						ListLableDefaultText =  "UsoCFDI",
					  						ListLocalDefaultText =  "UsoCFDI",
					  						HelpTextCode =  "UsoCFDICode",
					  						HelpTextDefaultText =  "Use of Digital Fiscal Receipt through Internet",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SATForeignRFC",
					  						OldFieldName =  "SATForeignRFC",
					  						ObjectTableName =  "CustomAgent",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "SATForeignRFC",
					  						ListPropertyPath =  "SATForeignRFC",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomAgent",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "SATForeignRFC",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SATForeignRFC",
					  						DefaultText =  "SAT Foreign RFC",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "SATForeignRFC",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ExternalId2",
					  						OldFieldName =  "ExternalId2",
					  						ObjectTableName =  "CustomAgent",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  25,
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ExternalId2",
					  						ListPropertyPath =  "ExternalId2",
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
					  						Code =  "ExternalId2",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ExternalId2",
					  						DefaultText =  "External ID2",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "ExternalId2",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IRSPlace",
					  						OldFieldName =  "IRSPlace",
					  						ObjectTableName =  "CustomAgent",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  60,
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
					  						PMPropertyPath =  "IRSPlace",
					  						ListPropertyPath =  "IRSPlace",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomAgent",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "IRSPlace",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IRSPlace",
					  						DefaultText =  "IRS Place",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "IRSPlace",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IRSNumber",
					  						OldFieldName =  "IRSNumber",
					  						ObjectTableName =  "CustomAgent",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  60,
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
					  						PMPropertyPath =  "IRSNumber",
					  						ListPropertyPath =  "IRSNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomAgent",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "IRSNumber",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IRSNumber",
					  						DefaultText =  "IRS #",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "IRSNumber",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ReceivablesAccountingCard",
					  						OldFieldName =  "ReceivablesAccountingCard",
					  						ObjectTableName =  "CustomAgent",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  25,
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ReceivablesAccountingCard",
					  						ListPropertyPath =  "ReceivablesAccountingCard",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomAgent",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ReceivablesAccountingCard",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ReceivablesAccountingCard",
					  						DefaultText =  "Receivables External ID",
					  						ListFieldLable =  "ReceivablesAccountingCardListLable",
					  						ListLableDefaultText =  "Receivables External ID",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "ReceivablesAccountingCard",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PayablesAccountingCard",
					  						OldFieldName =  "PayablesAccountingCard",
					  						ObjectTableName =  "CustomAgent",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  25,
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PayablesAccountingCard",
					  						ListPropertyPath =  "PayablesAccountingCard",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomAgent",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "PayablesAccountingCard",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PayablesAccountingCard",
					  						DefaultText =  "Payables External ID",
					  						ListFieldLable =  "PayablesAccountingCardListLable",
					  						ListLableDefaultText =  "Payables External ID",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "PayablesAccountingCard",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ExternalAccountingBusinessArea",
					  						OldFieldName =  "ExternalAccountingBusinessArea",
					  						ObjectTableName =  "CustomAgent",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  25,
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
					  						PMPropertyPath =  "ExternalAccountingBusinessArea",
					  						ListPropertyPath =  "ExternalAccountingBusinessArea",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomAgent",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ExternalAccountingBusinessArea",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ExternalAccountingBusinessArea",
					  						DefaultText =  "Business Area",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "ExternalAccountingBusinessArea",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PaymentMethodCode",
					  						OldFieldName =  "PaymentMethodCode",
					  						ObjectTableName =  "CustomAgent",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "SATPaymentMethod",
					  						MinLength =  0,
					  						MaxLength =  2,
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PaymentMethodCode",
					  						ListPropertyPath =  "PaymentMethodCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomAgent",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "PaymentMethodCode",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PaymentMethodCode",
					  						DefaultText =  "Forma Pago",
					  						HelpTextCode =  "PaymentMethodCode",
					  						HelpTextDefaultText =  "Payment Method",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PrimaryContactName",
					  						OldFieldName =  "PrimaryContactName",
					  						ObjectTableName =  "CustomAgent",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  60,
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PrimaryContactName",
					  						ListPropertyPath =  "PrimaryContactName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomAgent",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "PrimaryContactName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PrimaryContactName",
					  						DefaultText =  "Primary Contact Name",
					  						ListFieldLable =  "PrimaryContactNameListLable",
					  						ListLableDefaultText =  "Primary Contact",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "PrimaryContactName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PrimaryContactEmail",
					  						OldFieldName =  "PrimaryContactEmail",
					  						ObjectTableName =  "CustomAgent",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  70,
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PrimaryContactEmail",
					  						ListPropertyPath =  "PrimaryContactEmail",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomAgent",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "PrimaryContactEmail",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PrimaryContactEmail",
					  						DefaultText =  "Primary Contact E-mail",
					  						ListFieldLable =  "PrimaryContactEmailListLable",
					  						ListLableDefaultText =  "Primary Contact E-mail",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "PrimaryContactEmail",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PrimaryContactPhone",
					  						OldFieldName =  "PrimaryContactPhone",
					  						ObjectTableName =  "CustomAgent",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  25,
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
					  						PMPropertyPath =  "PrimaryContactPhone",
					  						ListPropertyPath =  "PrimaryContactPhone",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomAgent",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						Code =  "PrimaryContactPhone",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PrimaryContactPhone",
					  						DefaultText =  "Primary Contact Phone",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "PrimaryContactPhone",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "GLAccountId",
					  						OldFieldName =  "GLAccountId",
					  						ObjectTableName =  "CustomAgent",
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
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "GLAccountId",
					  						DefaultText =  "GLAccountId",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
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
	        QueryGroup CustomAgentQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "CAGT", Name = "Custom Agents" }, queryGroupRepository);
						QueryGroup CustomAgentQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "cf6e", Name = " Query Group" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable CustomAgentObjectTable = objectContext.ObjectTables.Where(d => d.Name == "CustomAgent" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> CustomAgentObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "CustomAgent").ToList();   

			   TextCode CustomAgentTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CustomAgent.Q.CustomAgents", DefaultText = @"Custom Agents",LocalDefaultText = null, ObjectTableId = CustomAgentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature CustomAgentFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CUSTOMAGENT", ObjectTableId = CustomAgentObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomAgent.Features.CustomAgents", NameTextCodeDefaultText = "Custom Agents", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query CustomagentsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = CustomAgentTextCode_0.Id, NameTextCodeCode = CustomAgentTextCode_0.Code, Code = "Custom agents",  QueryGroupCode = "CAGT", IndexOrder = 0, Tenant = 0, ObjectTableId = CustomAgentObjectTable.Id, QuerySection = "CustomAgent", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = CustomAgentFeature_0.Id,FeatureUniqeCode= CustomAgentFeature_0.FeatureUniqeCode, DefaultSortName = "Code", DefaultSortDirection = "Descending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn CustomagentsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomagentsQuery.Id, IndexOrder = 0, ObjectFieldId = CustomAgentObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == CustomAgentObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomAgentObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == CustomAgentObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CustomagentsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomagentsQuery.Id, IndexOrder = 1, ObjectFieldId = CustomAgentObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == CustomAgentObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomAgentObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == CustomAgentObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CustomagentsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomagentsQuery.Id, IndexOrder = 2, ObjectFieldId = CustomAgentObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == CustomAgentObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomAgentObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == CustomAgentObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CustomagentsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomagentsQuery.Id, IndexOrder = 3, ObjectFieldId = CustomAgentObjectFields.Where(d => d.FieldName == "VatNumber" && d.ObjectTableId == CustomAgentObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomAgentObjectFields.Where(d => d.FieldName == "VatNumber" && d.ObjectTableId == CustomAgentObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CustomagentsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomagentsQuery.Id, IndexOrder = 4, ObjectFieldId = CustomAgentObjectFields.Where(d => d.FieldName == "ReceivablesAccountingCard" && d.ObjectTableId == CustomAgentObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomAgentObjectFields.Where(d => d.FieldName == "ReceivablesAccountingCard" && d.ObjectTableId == CustomAgentObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CustomagentsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomagentsQuery.Id, IndexOrder = 5, ObjectFieldId = CustomAgentObjectFields.Where(d => d.FieldName == "PaymentTermEnglishName" && d.ObjectTableId == CustomAgentObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomAgentObjectFields.Where(d => d.FieldName == "PaymentTermEnglishName" && d.ObjectTableId == CustomAgentObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CustomagentsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomagentsQuery.Id, IndexOrder = 6, ObjectFieldId = CustomAgentObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTableId == CustomAgentObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomAgentObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTableId == CustomAgentObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CustomagentsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomagentsQuery.Id, IndexOrder = 7, ObjectFieldId = CustomAgentObjectFields.Where(d => d.FieldName == "Notes" && d.ObjectTableId == CustomAgentObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomAgentObjectFields.Where(d => d.FieldName == "Notes" && d.ObjectTableId == CustomAgentObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable CustomAgentObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "CustomAgent" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> CustomAgentObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "CustomAgent").ToList();
		       
	      

	         Screen CustomAgentGeneralTabScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "CustomAgent.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = CustomAgentObjectTable.Id, NumberOfColumns = 1, NumberOfRows = 6, IsReadOnly = false }, screensRepository, tenantScreens);
      
            ScreenField CustomAgentCustomAgentGeneralTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = CustomAgentObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().Id, ScreenId = CustomAgentGeneralTabScreenScreen0.Id, ObjectFieldCode = CustomAgentObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomAgentCustomAgentGeneralTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = CustomAgentObjectFields.Where(d => d.FieldName == "EnglishName").FirstOrDefault().Id, ScreenId = CustomAgentGeneralTabScreenScreen0.Id, ObjectFieldCode = CustomAgentObjectFields.Where(d => d.FieldName == "EnglishName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomAgentCustomAgentGeneralTabScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ObjectFieldId = CustomAgentObjectFields.Where(d => d.FieldName == "LocalName").FirstOrDefault().Id, ScreenId = CustomAgentGeneralTabScreenScreen0.Id, ObjectFieldCode = CustomAgentObjectFields.Where(d => d.FieldName == "LocalName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomAgentCustomAgentGeneralTabScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 3, ObjectFieldId = CustomAgentObjectFields.Where(d => d.FieldName == "Website").FirstOrDefault().Id, ScreenId = CustomAgentGeneralTabScreenScreen0.Id, ObjectFieldCode = CustomAgentObjectFields.Where(d => d.FieldName == "Website").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomAgentCustomAgentGeneralTabScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 4, ObjectFieldId = CustomAgentObjectFields.Where(d => d.FieldName == "InActive").FirstOrDefault().Id, ScreenId = CustomAgentGeneralTabScreenScreen0.Id, ObjectFieldCode = CustomAgentObjectFields.Where(d => d.FieldName == "InActive").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomAgentCustomAgentGeneralTabScreenScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 5, ObjectFieldId = CustomAgentObjectFields.Where(d => d.FieldName == "Notes").FirstOrDefault().Id, ScreenId = CustomAgentGeneralTabScreenScreen0.Id, ObjectFieldCode = CustomAgentObjectFields.Where(d => d.FieldName == "Notes").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
           
	      

	         Screen CustomAgentHeaderScreenScreen1 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "CustomAgent.HeaderScreen", Name = "Header Screen", ObjectTableId = CustomAgentObjectTable.Id, NumberOfColumns = 1, NumberOfRows = 2, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField CustomAgentCustomAgentHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = CustomAgentObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().Id, ScreenId = CustomAgentHeaderScreenScreen1.Id, ObjectFieldCode = CustomAgentObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomAgentCustomAgentHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = CustomAgentObjectFields.Where(d => d.FieldName == "EnglishName").FirstOrDefault().Id, ScreenId = CustomAgentHeaderScreenScreen1.Id, ObjectFieldCode = CustomAgentObjectFields.Where(d => d.FieldName == "EnglishName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    CustomAgentObjectTable.HeaderScreenId = CustomAgentHeaderScreenScreen1.Id;
	   		  
	      

	         Screen CustomAgentBillingTabScreenScreen2 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "CustomAgent.BillingTabScreen", Name = "Billing Tab Screen", ObjectTableId = CustomAgentObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 5, IsReadOnly = false }, screensRepository, tenantScreens);
      
            ScreenField CustomAgentCustomAgentBillingTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = CustomAgentObjectFields.Where(d => d.FieldName == "VatNumber").FirstOrDefault().Id, ScreenId = CustomAgentBillingTabScreenScreen2.Id, ObjectFieldCode = CustomAgentObjectFields.Where(d => d.FieldName == "VatNumber").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomAgentCustomAgentBillingTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = CustomAgentObjectFields.Where(d => d.FieldName == "PaymentTermId").FirstOrDefault().Id, ScreenId = CustomAgentBillingTabScreenScreen2.Id, ObjectFieldCode = CustomAgentObjectFields.Where(d => d.FieldName == "PaymentTermId").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomAgentCustomAgentBillingTabScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ObjectFieldId = CustomAgentObjectFields.Where(d => d.FieldName == "InvoiceCurrencyId").FirstOrDefault().Id, ScreenId = CustomAgentBillingTabScreenScreen2.Id, ObjectFieldCode = CustomAgentObjectFields.Where(d => d.FieldName == "InvoiceCurrencyId").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomAgentCustomAgentBillingTabScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 3, ObjectFieldId = CustomAgentObjectFields.Where(d => d.FieldName == "VatTypeId").FirstOrDefault().Id, ScreenId = CustomAgentBillingTabScreenScreen2.Id, ObjectFieldCode = CustomAgentObjectFields.Where(d => d.FieldName == "VatTypeId").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomAgentCustomAgentBillingTabScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 4, ObjectFieldId = CustomAgentObjectFields.Where(d => d.FieldName == "EnableConsolidationInvoices").FirstOrDefault().Id, ScreenId = CustomAgentBillingTabScreenScreen2.Id, ObjectFieldCode = CustomAgentObjectFields.Where(d => d.FieldName == "EnableConsolidationInvoices").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomAgentCustomAgentBillingTabScreenScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = CustomAgentObjectFields.Where(d => d.FieldName == "BankName").FirstOrDefault().Id, ScreenId = CustomAgentBillingTabScreenScreen2.Id, ObjectFieldCode = CustomAgentObjectFields.Where(d => d.FieldName == "BankName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomAgentCustomAgentBillingTabScreenScreenField6 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ObjectFieldId = CustomAgentObjectFields.Where(d => d.FieldName == "BankAddress").FirstOrDefault().Id, ScreenId = CustomAgentBillingTabScreenScreen2.Id, ObjectFieldCode = CustomAgentObjectFields.Where(d => d.FieldName == "BankAddress").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomAgentCustomAgentBillingTabScreenScreenField7 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 2, ObjectFieldId = CustomAgentObjectFields.Where(d => d.FieldName == "Swift").FirstOrDefault().Id, ScreenId = CustomAgentBillingTabScreenScreen2.Id, ObjectFieldCode = CustomAgentObjectFields.Where(d => d.FieldName == "Swift").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomAgentCustomAgentBillingTabScreenScreenField8 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 3, ObjectFieldId = CustomAgentObjectFields.Where(d => d.FieldName == "AccountNumber").FirstOrDefault().Id, ScreenId = CustomAgentBillingTabScreenScreen2.Id, ObjectFieldCode = CustomAgentObjectFields.Where(d => d.FieldName == "AccountNumber").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomAgentCustomAgentBillingTabScreenScreenField9 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 4, ObjectFieldId = CustomAgentObjectFields.Where(d => d.FieldName == "IBANNumber").FirstOrDefault().Id, ScreenId = CustomAgentBillingTabScreenScreen2.Id, ObjectFieldCode = CustomAgentObjectFields.Where(d => d.FieldName == "IBANNumber").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
           
	      

	         Screen CustomAgentAccountingTabScreenScreen3 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "CustomAgent.AccountingTabScreen", Name = "Accounting Tab Screen", ObjectTableId = CustomAgentObjectTable.Id, NumberOfColumns = 1, NumberOfRows = 2, IsReadOnly = false }, screensRepository, tenantScreens);
      
            ScreenField CustomAgentCustomAgentAccountingTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = CustomAgentObjectFields.Where(d => d.FieldName == "ReceivablesAccountingCard").FirstOrDefault().Id, ScreenId = CustomAgentAccountingTabScreenScreen3.Id, ObjectFieldCode = CustomAgentObjectFields.Where(d => d.FieldName == "ReceivablesAccountingCard").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomAgentCustomAgentAccountingTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = CustomAgentObjectFields.Where(d => d.FieldName == "PayablesAccountingCard").FirstOrDefault().Id, ScreenId = CustomAgentAccountingTabScreenScreen3.Id, ObjectFieldCode = CustomAgentObjectFields.Where(d => d.FieldName == "PayablesAccountingCard").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
           

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable CustomAgentObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "CustomAgent" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode CustomAgentGeneralTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CustomAgent.TH.General", DefaultText = "General",LocalDefaultText = null, ObjectTableId = CustomAgentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CustomAgentGeneralFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GENERAL", ObjectTableId = CustomAgentObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomAgent.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode CustomAgentBillingTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CustomAgent.TH.Billing", DefaultText = "Billing",LocalDefaultText = null, ObjectTableId = CustomAgentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CustomAgentBillingFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BILLING", ObjectTableId = CustomAgentObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomAgent.Features.Billing", NameTextCodeDefaultText = "Edit Custom Billing", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode CustomAgentAccountingTextCode_TH2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CustomAgent.TH.Accounting", DefaultText = "Accounting",LocalDefaultText = null, ObjectTableId = CustomAgentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature GeneralAccountingFeature_TH2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACCOUNTINGTRANSFER", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.AccountingTransfer", NameTextCodeDefaultText = "Accounting Transfer", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode CustomAgentAddressesTextCode_TH3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CustomAgent.TH.Addresses", DefaultText = "Addresses",LocalDefaultText = null, ObjectTableId = CustomAgentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CustomAgentAddressesFeature_TH3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ADDRESSES", ObjectTableId = CustomAgentObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomAgent.Features.Addresses", NameTextCodeDefaultText = "Edit Custom Addresses", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode CustomAgentContactsTextCode_TH4 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CustomAgent.TH.Contacts", DefaultText = "Contacts",LocalDefaultText = null, ObjectTableId = CustomAgentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CustomAgentContactsFeature_TH4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CONTACTS", ObjectTableId = CustomAgentObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomAgent.Features.Contacts", NameTextCodeDefaultText = "Edit Custom Contacts", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode CustomAgentDocsInTextCode_TH5 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CustomAgent.TH.DocsIn", DefaultText = "Docs In",LocalDefaultText = null, ObjectTableId = CustomAgentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CustomAgentDocsInFeature_TH5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DOCSIN", ObjectTableId = CustomAgentObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomAgent.Features.DocsIn", NameTextCodeDefaultText = "Docs In", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode CustomAgentEventsTextCode_TH6 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CustomAgent.TH.Events", DefaultText = "Events",LocalDefaultText = null, ObjectTableId = CustomAgentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CustomAgentEventsFeature_TH6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EVENTS", ObjectTableId = CustomAgentObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomAgent.Features.Events", NameTextCodeDefaultText = "Edit Custom Events", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CUGC",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = CustomAgentGeneralFeature_TH0.Id, ControlPath = "Simplog.Infrastructure.GeneralControls.GeneralTabControl", ObjectTableId = CustomAgentObjectTable.Id, TabNameTextCodeId = CustomAgentGeneralTextCode_TH0.Id, TabNameTextCodeCode = CustomAgentGeneralTextCode_TH0.Code, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CUBL",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = CustomAgentBillingFeature_TH1.Id, ControlPath = "Simplog.Infrastructure.GeneralControls.BillingTabControl", ObjectTableId = CustomAgentObjectTable.Id, TabNameTextCodeId = CustomAgentBillingTextCode_TH1.Id, TabNameTextCodeCode = CustomAgentBillingTextCode_TH1.Code, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CUAC",HtmlComponentName = "",HtmlComponentUrl = "./Common/Components/AccountingTab/AccountingTabComponent", FeatureId = GeneralAccountingFeature_TH2.Id, ControlPath = "Simplog.FreightLib.Views.PartnersTabs.PartnersAccountingTab.CustomAgentAccountingTabControl", ObjectTableId = CustomAgentObjectTable.Id, TabNameTextCodeId = CustomAgentAccountingTextCode_TH2.Id, TabNameTextCodeCode = CustomAgentAccountingTextCode_TH2.Code, Tenant = 0, IndexOrder = 2 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CUAD",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = CustomAgentAddressesFeature_TH3.Id, ControlPath = "Simplog.FreightLib.Views.PartnersTabs.PartnerAddressesTab", ObjectTableId = CustomAgentObjectTable.Id, TabNameTextCodeId = CustomAgentAddressesTextCode_TH3.Id, TabNameTextCodeCode = CustomAgentAddressesTextCode_TH3.Code, Tenant = 0, IndexOrder = 3 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CUCO",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = CustomAgentContactsFeature_TH4.Id, ControlPath = "Simplog.FreightLib.Views.PartnersTabs.PartnerContactsTab", ObjectTableId = CustomAgentObjectTable.Id, TabNameTextCodeId = CustomAgentContactsTextCode_TH4.Id, TabNameTextCodeCode = CustomAgentContactsTextCode_TH4.Code, Tenant = 0, IndexOrder = 4 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CGDI",HtmlComponentName = "",HtmlComponentUrl = "./CommonModules/CommonPartners/Components/EditTabs/CustomAgent/CustomAgentDocsInTabComponent", FeatureId = CustomAgentDocsInFeature_TH5.Id, ControlPath = "", ObjectTableId = CustomAgentObjectTable.Id, TabNameTextCodeId = CustomAgentDocsInTextCode_TH5.Id, TabNameTextCodeCode = CustomAgentDocsInTextCode_TH5.Code, Tenant = 0, IndexOrder = 5 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CUEV",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = CustomAgentEventsFeature_TH6.Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = CustomAgentObjectTable.Id, TabNameTextCodeId = CustomAgentEventsTextCode_TH6.Id, TabNameTextCodeCode = CustomAgentEventsTextCode_TH6.Code, Tenant = 0, IndexOrder = 6 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable CustomAgentObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "CustomAgent" && d.Tenant == 0).FirstOrDefault(); 

		   Feature CustomAgentFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = true, ObjectTableId = CustomAgentObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomAgent.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature CustomAgentFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = true, ObjectTableId = CustomAgentObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomAgent.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature CustomAgentFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = true, ObjectTableId = CustomAgentObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomAgent.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature CustomAgentFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = true, ObjectTableId = CustomAgentObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomAgent.Features.PackageFeature", NameTextCodeDefaultText = "CustomAgent Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);    
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable CustomAgentObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "CustomAgent" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "UPCA",
                EnglishName =  "Custom Agent Updated",
                LocalName =  "Custom Agent Updated",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CustomAgentObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CRCA",
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
                ObjectTableId = CustomAgentObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CGDS",
                EnglishName =  "GLAccount was disconnected",
                LocalName =  "נותק כרטיס הנהח\"ש",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CustomAgentObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
		   FeatureRepository featureRepository = new FeatureRepository(0); 
		   //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList(); 
		   ObjectTable CustomAgentObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "CustomAgent" && d.Tenant == 0).FirstOrDefault(); 			   Feature CustomAgentFeature_MB00 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Disconnect", ObjectTableId = CustomAgentObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomAgent.Features.DisconnectGLAccount", NameTextCodeDefaultText = "Disconnect GLAccount", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
              

		   TextCodeRepository.SubmitChanges();
		   FeaturesRepository.SubmitChanges();
		   MenuButtonGroup CustomAgentMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
				{
					MenuButtonGroupType = "CustomAgentEdit",
					Name = "CustomAgentEditButtonsGroup",
					ObjectTableId = CustomAgentObjectTable.Id,
					Tenant = 0
				}, menuButtonGroupRepository, tenantMenuButtonGroups);        
   
			   MenuButton CustomAgentMenuButton0 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "More",
						Index = 100, 
						IsActive = false,
						LabelTextCodeCode = "CustomAgent.B.More",
						LabelTextCodeDefaultText = "More",
						Tenant = 0,
						MenuButtonGroupId = CustomAgentMenuButtonGroup.Id,
						ObjectTableId = CustomAgentObjectTable.Id,
						MenuButtonType = "dropdownbutton",
						FeatureId = null,
						Style = null,
						LocalDefaultText = "More",
						FeatureUniqeCode = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);

			   MenuButton CustomAgentMenuButton00 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "Disconnect",
						Index = 0, 
						IsActive = true,
						LabelTextCodeCode = "CustomAgent.B.DisconnectGLAccount",
						LabelTextCodeDefaultText = "Disconnect GLAccount",
						Tenant = 0,
						MenuButtonGroupId = CustomAgentMenuButtonGroup.Id,
						ParentMenuButtonId = CustomAgentMenuButton0.Id,
						ObjectTableId = CustomAgentObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  CustomAgentFeature_MB00.Id,
						Style = null,
						LocalDefaultText = "נתק כרטיס",
						FeatureUniqeCode=  CustomAgentFeature_MB00.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {     
	    
}

    

   }
    
}
	 