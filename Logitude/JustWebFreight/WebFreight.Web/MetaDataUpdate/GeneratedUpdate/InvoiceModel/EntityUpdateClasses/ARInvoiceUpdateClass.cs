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
   public class ARInvoiceUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "ARInvoice",
			      				    IsNew =  false,
			      				    DBTableName =  "ARInvoices",
			      				    OldDBTableName =  "ARInvoices",
			      				    ObjectTableSingular =  "Invoice",
			      				    ObjectTablePlural =  "Invoices",
			      				    HasCustomFilter =  true,
			      				    HasCustomFields =  false,
			      				    HasHelper =  true,
			      				    HasShortTitle =  true,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  true,
			      				    IsNewWizard =  false,
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
			      				    SortingByObjectField =  "CreateDate",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  false,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  true,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "BR",
			      				    MaxNumberOfCustomFields =  10,
			      				    DefaultText =  "Invoice",
			      				    Code =  "INVC",
			      				    Name =  "Invoices",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Invoice",
			      				    NoTS =  false,
			      				    HasMenuButtons =  true,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  true,
			      				    IsLookUp =  false,
			      				    IsTabsHidden =  false,
			      				    SearchFields =  "ARInvoice,ARInvoices,,Id,",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IssuedByUserId",
					  						OldFieldName =  "IssuedByUserId",
					  						ObjectTableName =  "ARInvoice",
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
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IssuedByUserId",
					  						ListPropertyPath =  "IssuedByUserId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "IssuedByUserId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IssuedByUserId",
					  						DefaultText =  "Issued By User",
					  						FullLocalDefaultText =  "IssuedByUserId",
					  						ListFieldLable =  "IssuedByUserIdListLable",
					  						ListLableDefaultText =  "IssuedByUserId",
					  						ListLocalDefaultText =  "IssuedByUserId",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "IssuedByUserId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ExchangeRateDate",
					  						OldFieldName =  "ExchangeRateDate",
					  						ObjectTableName =  "ARInvoice",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ExchangeRateDate",
					  						ListPropertyPath =  "ExchangeRateDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ExchangeRateDate",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ExchangeRateDate",
					  						DefaultText =  "Exchange Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "ExchangeRateDate",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DraftNumber",
					  						OldFieldName =  "DraftNumber",
					  						ObjectTableName =  "ARInvoice",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DraftNumber",
					  						ListPropertyPath =  "DraftNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "DraftNumber",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DraftNumber",
					  						DefaultText =  "Draft Number",
					  						ListFieldLable =  "DraftNumberListLable",
					  						ListLableDefaultText =  "Draft #",
					  						ListLocalDefaultText =  "מספר טיוטה",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "DraftNumber",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Reference",
					  						OldFieldName =  "Reference",
					  						ObjectTableName =  "ARInvoice",
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
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Reference",
					  						ListPropertyPath =  "Reference",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "Reference",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Reference",
					  						DefaultText =  "Reference",
					  						HelpTextCode =  "Reference",
					  						HelpTextDefaultText =  "Searching by:\n1. Invoice Number\n2. Entity Reference ",
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
					 
					 						FieldName =  "SearchFields",
					  						OldFieldName =  "SearchFields",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "Text",
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
					  						Operator =  "Contains",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						ValidForQuerySection2 =  "InvoiceFollowUp",
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
					  						DefaultText =  "Search Inv. # / Bill to",
					  						FullLocalDefaultText =  "חיפוש לפי מספר חשבונית, לקוח",
					  						HelpTextCode =  "SearchFields",
					  						HelpTextDefaultText =  "Searching by :\n1:Inv. # \n2:Bill to",
					  						HelpLocalDefaultText =  "חיפוש לפי מספר חשבונית, לקוח",
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
					 
					 						FieldName =  "MainEntityReference",
					  						OldFieldName =  "MainEntityReference",
					  						ObjectTableName =  "ARInvoice",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "MainEntityReference",
					  						ListPropertyPath =  "MainEntityReference",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "MainEntityReference",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "MainEntityReference",
					  						DefaultText =  "Shipment No.",
					  						FullLocalDefaultText =  "מספר אסמכתא",
					  						ListFieldLable =  "MainEntityReferenceListLable",
					  						ListLableDefaultText =  "Shipment No.",
					  						ListLocalDefaultText =  "מספר אסמכתא",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "MainEntityReference",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AmountDue",
					  						OldFieldName =  "AmountDue",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "Double",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						DataTemplateName =  "InvoiceAmountDueDataTemplate",
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AmountDue",
					  						ListPropertyPath =  "AmountDue",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "AmountDue",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AmountDue",
					  						DefaultText =  "Amount Due",
					  						FullLocalDefaultText =  "סכום חובה",
					  						ListFieldLable =  "AmountDueListLable",
					  						ListLableDefaultText =  "Amount Due",
					  						ListLocalDefaultText =  "סכום חובה",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						HelpTextCode =  "AmountDue",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ExpectedPaymentDate",
					  						OldFieldName =  "ExpectedPaymentDate",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "DateTime",
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
					  						DataTemplateName =  "InvoiceExpectedPaymentDateDataTemplate",
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ExpectedPaymentDate",
					  						ListPropertyPath =  "ExpectedPaymentDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ExpectedPaymentDate",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ExpectedPaymentDate",
					  						DefaultText =  "Expected Payment Date",
					  						FullLocalDefaultText =  "תאריך צפוי לתשלום",
					  						ListFieldLable =  "ExpectedPaymentDateListLable",
					  						ListLableDefaultText =  "Exp. Payment Date",
					  						ListLocalDefaultText =  "תאריך צפוי לתשלום",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						HelpTextCode =  "ExpectedPaymentDate",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UnpaidInvoices",
					  						OldFieldName =  "UnpaidInvoices",
					  						ObjectTableName =  "ARInvoice",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "UnpaidInvoices",
					  						ListPropertyPath =  "UnpaidInvoices",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "UnpaidInvoices",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UnpaidInvoices",
					  						DefaultText =  "Unpaid Invoices",
					  						FullLocalDefaultText =  "חשבוניות שלא שולמו",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "UnpaidInvoices",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InvoiceNumber",
					  						OldFieldName =  "InvoiceNumber",
					  						ObjectTableName =  "ARInvoice",
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
					  						PMPropertyPath =  "InvoiceNumber",
					  						ListPropertyPath =  "InvoiceNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "InvoiceNumber",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "InvoiceNumber",
					  						DefaultText =  "Invoice Number",
					  						ListFieldLable =  "InvoiceNumberListLable",
					  						ListLableDefaultText =  "Invoice No.",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "InvoiceNumber",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ARInvoiceTypeCode",
					  						OldFieldName =  "ARInvoiceTypeCode",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "ARInvoiceType",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ARInvoiceTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ARInvoiceTypeCode",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ARInvoiceTypeCode",
					  						DefaultText =  "Invoice Type",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "ARInvoiceTypeCode",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ARInvoiceTypeName",
					  						OldFieldName =  "ARInvoiceTypeName",
					  						ObjectTableName =  "ARInvoice",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ARInvoiceTypeName",
					  						ListPropertyPath =  "ARInvoiceTypeName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ARInvoiceTypeName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ARInvoiceTypeName",
					  						DefaultText =  "Invoice Type",
					  						ListFieldLable =  "ARInvoiceTypeNameListLable",
					  						ListLableDefaultText =  "Invoice Type",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "ARInvoiceTypeName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PaymentTermId",
					  						OldFieldName =  "PaymentTermId",
					  						ObjectTableName =  "ARInvoice",
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
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PaymentTermId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
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
					  						FullLocalDefaultText =  "תנאי תשלום",
					  						ListFieldLable =  "PaymentTermIdListLable",
					  						ListLableDefaultText =  "Payment Terms",
					  						ListLocalDefaultText =  "תנאי תשלום",
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
					 
					 						FieldName =  "StatusName",
					  						OldFieldName =  "StatusName",
					  						ObjectTableName =  "ARInvoice",
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
					  						DataTemplateName =  "ARInvoiceStatusDataTemplate",
					  						IsCustomFilter =  false,
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
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "StatusName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "StatusName",
					  						DefaultText =  "Status",
					  						FullLocalDefaultText =  "סטטוס",
					  						ListFieldLable =  "StatusNameListLable",
					  						ListLableDefaultText =  "Status",
					  						ListLocalDefaultText =  "סטטוס",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						HelpTextCode =  "StatusName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AmountDueInLocalCurrency",
					  						OldFieldName =  "AmountDueInLocalCurrency",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "Double",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						DataTemplateName =  "AmountDueInLocalCurrencyDataTemplate",
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AmountDueInLocalCurrency",
					  						ListPropertyPath =  "AmountDueInLocalCurrency",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "AmountDueInLocalCurrency",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AmountDueInLocalCurrency",
					  						DefaultText =  "Amount Due (Local Currency)",
					  						FullLocalDefaultText =  "סכום לפירעון בש''ח",
					  						ListFieldLable =  "AmountDueInLocalCurrencyListLable",
					  						ListLableDefaultText =  "Amount Due (Local Currency)",
					  						ListLocalDefaultText =  "סכום לפירעון בש''ח",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						HelpTextCode =  "AmountDueInLocalCurrency",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AmountDueInProfitCurrency",
					  						OldFieldName =  "AmountDueInProfitCurrency",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "Double",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						DataTemplateName =  "AmountDueInProfitCurrencyDataTemplate",
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AmountDueInProfitCurrency",
					  						ListPropertyPath =  "AmountDueInProfitCurrency",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "AmountDueInProfitCurrency",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AmountDueInProfitCurrency",
					  						DefaultText =  "Amount Due (Profit Currency)",
					  						FullLocalDefaultText =  "סכום לתשלום(מטבע רווח)",
					  						ListFieldLable =  "AmountDueInProfitCurrencyListLable",
					  						ListLableDefaultText =  "Amount Due (Profit Currency)",
					  						ListLocalDefaultText =  "סכום לתשלום(מטבע רווח)",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						HelpTextCode =  "AmountDueInProfitCurrency",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PaymentTermName",
					  						OldFieldName =  "PaymentTermName",
					  						ObjectTableName =  "ARInvoice",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PaymentTermName",
					  						ListPropertyPath =  "PaymentTermName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "PaymentTermName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PaymentTermName",
					  						DefaultText =  "Payment Term",
					  						FullLocalDefaultText =  "תנאי תשלום",
					  						ListFieldLable =  "PaymentTermNameListLable",
					  						ListLableDefaultText =  "Payment Term",
					  						ListLocalDefaultText =  "תנאי תשלום",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "PaymentTermName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BillToId",
					  						OldFieldName =  "BillToId",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Card",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "BillToId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "BillToId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "BillToId",
					  						DefaultText =  "Bill To",
					  						FullLocalDefaultText =  "כרטיס לחיוב",
					  						ListFieldLable =  "BillToIdListLable",
					  						ListLableDefaultText =  "Bill To",
					  						ListLocalDefaultText =  "כרטיס לחיוב",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "BillToId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BillToPartnerId",
					  						OldFieldName =  "BillToPartnerId",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "PartnerType",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "BillToPartnerId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "BillToPartnerId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "BillToPartnerId",
					  						DefaultText =  "Bill To Partner",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "BillToPartnerId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BillToPartnerName",
					  						OldFieldName =  "BillToPartnerName",
					  						ObjectTableName =  "ARInvoice",
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
					  						PMPropertyPath =  "BillToPartnerName",
					  						ListPropertyPath =  "BillToPartnerName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "BillToPartnerName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "BillToPartnerName",
					  						DefaultText =  "Bill To Partner",
					  						ListFieldLable =  "BillToPartnerNameListLable",
					  						ListLableDefaultText =  "Bill To Partner",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "BillToPartnerName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BillToName",
					  						OldFieldName =  "BillToName",
					  						ObjectTableName =  "ARInvoice",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "BillToName",
					  						ListPropertyPath =  "BillToName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "BillToName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "BillToName",
					  						DefaultText =  "Bill To",
					  						ListFieldLable =  "BillToNameListLable",
					  						ListLableDefaultText =  "Bill To",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "BillToName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BillToAddressId",
					  						OldFieldName =  "BillToAddressId",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Address",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "BillToAddressId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "BillToAddressId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "BillToAddressId",
					  						DefaultText =  "Address",
					  						FullLocalDefaultText =  "כתובת",
					  						ListFieldLable =  "BillToAddressIdListLable",
					  						ListLableDefaultText =  "Address",
					  						ListLocalDefaultText =  "כתובת",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "BillToAddressId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "VatNumber",
					  						OldFieldName =  "VatNumber",
					  						ObjectTableName =  "ARInvoice",
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
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
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
					  						ValidForQuerySection1 =  "ARInvoice",
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
					  						FullLocalDefaultText =  "מספר ח.פ",
					  						ListFieldLable =  "VatNumberListLable",
					  						ListLableDefaultText =  "VAT Number",
					  						ListLocalDefaultText =  "מספר ח.פ",
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
					 
					 						FieldName =  "CreateDate",
					  						OldFieldName =  "CreateDate",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  15,
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
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "CreateDate",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CreateDate",
					  						DefaultText =  "Create Date",
					  						ListFieldLable =  "CreateDateListLable",
					  						ListLableDefaultText =  "Create Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "CreateDate",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InvoiceDate",
					  						OldFieldName =  "InvoiceDate",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  15,
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  true,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "InvoiceDate",
					  						ListPropertyPath =  "InvoiceDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "InvoiceDate",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "InvoiceDate",
					  						DefaultText =  "Invoice Date",
					  						FullLocalDefaultText =  "תאריך חשבונאי",
					  						ListFieldLable =  "InvoiceDateListLable",
					  						ListLableDefaultText =  "Invoice Date",
					  						ListLocalDefaultText =  "תאריך חשבונאי",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "InvoiceDate",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreatedByUserId",
					  						OldFieldName =  "CreatedByUserId",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
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
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "CreatedByUserId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CreatedByUserId",
					  						DefaultText =  "Created By User",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "CreatedByUserId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreatedByUserName",
					  						OldFieldName =  "CreatedByUserName",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  60,
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
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "CreatedByUserName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CreatedByUserName",
					  						DefaultText =  "Created By User",
					  						ListFieldLable =  "CreatedByUserNameListLable",
					  						ListLableDefaultText =  "Created By",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "CreatedByUserName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PrintByUserId",
					  						OldFieldName =  "PrintByUserId",
					  						ObjectTableName =  "ARInvoice",
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
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PrintByUserId",
					  						ListPropertyPath =  "PrintByUserId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "PrintByUserId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PrintByUserId",
					  						DefaultText =  "Print By User",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "PrintByUserId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PrintByUserName",
					  						OldFieldName =  "PrintByUserName",
					  						ObjectTableName =  "ARInvoice",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PrintByUserName",
					  						ListPropertyPath =  "PrintByUserName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "PrintByUserName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PrintByUserName",
					  						DefaultText =  "Print By User",
					  						ListFieldLable =  "PrintByUserNameListLable",
					  						ListLableDefaultText =  "Print By",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "PrintByUserName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InvoiceCurrencyId",
					  						OldFieldName =  "InvoiceCurrencyId",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Currency",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "InvoiceCurrencyId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
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
					  						DefaultText =  "Currency",
					  						FullLocalDefaultText =  "מטבע",
					  						ListFieldLable =  "InvoiceCurrencyIdListLable",
					  						ListLableDefaultText =  "Currency",
					  						ListLocalDefaultText =  "מטבע",
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
					 
					 						FieldName =  "IsPrinted",
					  						OldFieldName =  "IsPrinted",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						DataTemplateName =  "ARInvoicePrintedDataTemplate",
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsPrinted",
					  						ListPropertyPath =  "IsPrinted",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "IsPrinted",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						ColumnHeaderTemplateName =  "ARInvoiceIsPrintedHeaderTemplate",
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsPrinted",
					  						DefaultText =  "Printed",
					  						FullLocalDefaultText =  "הודפס",
					  						ListFieldLable =  "IsPrintedListLable",
					  						ListLableDefaultText =  "IsPrinted",
					  						ListLocalDefaultText =  "הודפס",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						HelpTextCode =  "IsPrinted",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ConnectedEntityReferences",
					  						OldFieldName =  "ConnectedEntityReferences",
					  						ObjectTableName =  "ARInvoice",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ConnectedEntityReferences",
					  						ListPropertyPath =  "ConnectedEntityReferences",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ConnectedEntityReferences",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ConnectedEntityReferences",
					  						DefaultText =  "References",
					  						FullLocalDefaultText =  "אסמכתאות",
					  						IsMaxLength =  true,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "ConnectedEntityReferences",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdateDate",
					  						OldFieldName =  "UpdateDate",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "DateTime",
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
					  						DisplayInList =  true,
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						IsCustomFilter =  false,
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
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "UpdateDate",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UpdateDate",
					  						DefaultText =  "Update Date",
					  						FullLocalDefaultText =  "תאריך עדכון",
					  						ListFieldLable =  "UpdateDateListLable",
					  						ListLableDefaultText =  "Update Date",
					  						ListLocalDefaultText =  "תאריך עדכון",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "UpdateDate",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdatedByUserId",
					  						OldFieldName =  "UpdatedByUserId",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
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
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "UpdatedByUserId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UpdatedByUserId",
					  						DefaultText =  "Updated By",
					  						FullLocalDefaultText =  "עודכן ע''י",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "UpdatedByUserId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InvoiceCurrencyCode",
					  						OldFieldName =  "InvoiceCurrencyCode",
					  						ObjectTableName =  "ARInvoice",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "InvoiceCurrencyCode",
					  						ListPropertyPath =  "InvoiceCurrencyCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "InvoiceCurrencyCode",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "InvoiceCurrencyCode",
					  						DefaultText =  "Currency",
					  						FullLocalDefaultText =  "מטבע",
					  						ListFieldLable =  "InvoiceCurrencyCodeListLable",
					  						ListLableDefaultText =  "Invoice Currency",
					  						ListLocalDefaultText =  "מטבע",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "InvoiceCurrencyCode",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LocalCurrencyId",
					  						OldFieldName =  "LocalCurrencyId",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Currency",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "LocalCurrencyId",
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
					  						Code =  "LocalCurrencyId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LocalCurrencyId",
					  						DefaultText =  "Local Currency",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "LocalCurrencyId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InvoiceCurrencyExchangeRate",
					  						OldFieldName =  "InvoiceCurrencyExchangeRate",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "Double",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "InvoiceExchangeRate",
					  						ListPropertyPath =  "InvoiceExchangeRate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "InvoiceCurrencyExchangeRate",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "InvoiceCurrencyExchangeRate",
					  						DefaultText =  "Exchange Rate",
					  						FullLocalDefaultText =  "שער",
					  						ListFieldLable =  "InvoiceCurrencyExchangeRateListLable",
					  						ListLableDefaultText =  "Exchange Rate",
					  						ListLocalDefaultText =  "שער",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						HelpTextCode =  "InvoiceCurrencyExchangeRate",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SubTotalInLocalCurrency",
					  						OldFieldName =  "SubTotalInLocalCurrency",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "Double",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "SubTotalInLocalCurrency",
					  						ListPropertyPath =  "SubTotalInLocalCurrency",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "SubTotalInLocalCurrency",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SubTotalInLocalCurrency",
					  						DefaultText =  "Sub Total (Local Currency)",
					  						FullLocalDefaultText =  "סיכום ביניים בש'ח",
					  						ListFieldLable =  "SubTotalInLocalCurrencyListLable",
					  						ListLableDefaultText =  "Subtotal Local",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "SubTotalInLocalCurrency",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SubTotalInInvoiceCurrency",
					  						OldFieldName =  "SubTotalInInvoiceCurrency",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "Double",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "SubTotalInInvoiceCurrency",
					  						ListPropertyPath =  "SubTotalInInvoiceCurrency",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "SubTotalInInvoiceCurrency",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SubTotalInInvoiceCurrency",
					  						DefaultText =  "Sub Total",
					  						FullLocalDefaultText =  "סיכום ביניים",
					  						ListFieldLable =  "SubTotalInInvoiceCurrencyListLable",
					  						ListLableDefaultText =  "Subtotal Invoice",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "SubTotalInInvoiceCurrency",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AmountInLocalCurrency",
					  						OldFieldName =  "AmountInLocalCurrency",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "Double",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						DataTemplateName =  "AmountInLocalCurrencyDataTemplate",
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AmountInLocalCurrency",
					  						ListPropertyPath =  "AmountInLocalCurrency",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "AmountInLocalCurrency",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AmountInLocalCurrency",
					  						DefaultText =  "Amount (Local Currency)",
					  						FullLocalDefaultText =  "סכום(במטבע מקומי)",
					  						ListFieldLable =  "AmountInLocalCurrencyListLable",
					  						ListLableDefaultText =  "Amount (Local Currency)",
					  						ListLocalDefaultText =  "סכום(במטבע מקומי)",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						HelpTextCode =  "AmountInLocalCurrency",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AmountInInvoiceCurrency",
					  						OldFieldName =  "AmountInInvoiceCurrency",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "Double",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						DataTemplateName =  "AmountInInvoiceCurrencyDataTemplate",
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AmountInInvoiceCurrency",
					  						ListPropertyPath =  "AmountInInvoiceCurrency",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "AmountInInvoiceCurrency",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AmountInInvoiceCurrency",
					  						DefaultText =  "Invoice Amount",
					  						FullLocalDefaultText =  "סכום חשבונית",
					  						ListFieldLable =  "AmountInInvoiceCurrencyListLable",
					  						ListLableDefaultText =  "Invoice Amount",
					  						ListLocalDefaultText =  "סכום חשבונית",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						HelpTextCode =  "AmountInInvoiceCurrency",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "StatusCode",
					  						OldFieldName =  "StatusCode",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "ARInvoiceStatus",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "StatusCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "StatusCode",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "StatusCode",
					  						DefaultText =  "Invoice Status",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "StatusCode",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ARInvoiceStatusName",
					  						OldFieldName =  "ARInvoiceStatusName",
					  						ObjectTableName =  "ARInvoice",
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
					  						DataTemplateName =  "ARInvoiceStatusDataTemplate",
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ARInvoiceStatusName",
					  						ListPropertyPath =  "ARInvoiceStatusName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ARInvoiceStatusName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ARInvoiceStatusName",
					  						DefaultText =  "Invoice Status",
					  						ListFieldLable =  "ARInvoiceStatusNameListLable",
					  						ListLableDefaultText =  "Invoice Status",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "ARInvoiceStatusName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsAutoCredit",
					  						OldFieldName =  "IsAutoCredit",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsAutoCredit",
					  						ListPropertyPath =  "IsAutoCredit",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "IsAutoCredit",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsAutoCredit",
					  						DefaultText =  "Is Auto Credit",
					  						ListFieldLable =  "IsAutoCreditListLable",
					  						ListLableDefaultText =  "Is Auto Credit",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "IsAutoCredit",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsCancelled",
					  						OldFieldName =  "IsCancelled",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
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
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "IsCancelled",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsCancelled",
					  						DefaultText =  "Is Canceled",
					  						ListFieldLable =  "IsCancelledListLable",
					  						ListLableDefaultText =  "Is Canceled",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "IsCancelled",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CancelledByARInvoiceId",
					  						OldFieldName =  "CancelledByARInvoiceId",
					  						ObjectTableName =  "ARInvoice",
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
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CancelledByARInvoiceId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "CancelledByARInvoiceId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CancelledByARInvoiceId",
					  						DefaultText =  "Canceled by Invoice",
					  						FullLocalDefaultText =  "בוטל של חשבונית",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "CancelledByARInvoiceId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InternalNotes",
					  						OldFieldName =  "InternalNotes",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "Text",
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
					  						DataTemplateName =  "InternalNotesDataTemplate",
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "InternalNotes",
					  						ListPropertyPath =  "InternalNotes",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "InternalNotes",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "InternalNotes",
					  						DefaultText =  "Internal Notes",
					  						ListFieldLable =  "InternalNotesListLable",
					  						ListLableDefaultText =  "Internal Notes",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						HelpTextCode =  "InternalNotes",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PrintNotes",
					  						OldFieldName =  "PrintNotes",
					  						ObjectTableName =  "ARInvoice",
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
					  						DisplayInList =  false,
					  						DataTemplateName =  "PrintNotesDataTemplate",
					  						IsCustomFilter =  false,
					  						MultiLine =  true,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PrintNotes",
					  						ListPropertyPath =  "PrintNotes",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "PrintNotes",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PrintNotes",
					  						DefaultText =  "Print Notes",
					  						ListFieldLable =  "PrintNotesListLable",
					  						ListLableDefaultText =  "Notes",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						HelpTextCode =  "PrintNotes",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DueDate",
					  						OldFieldName =  "DueDate",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						DataTemplateName =  "InvoiceDueDateTimeDataTemplate",
					  						IsCustomFilter =  false,
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
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "DueDate",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DueDate",
					  						DefaultText =  "Due Date",
					  						FullLocalDefaultText =  "תאריך פרעון",
					  						ListFieldLable =  "DueDateListLable",
					  						ListLableDefaultText =  "Due Date",
					  						ListLocalDefaultText =  "תאריך פרעון",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						HelpTextCode =  "DueDate",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PrintDate",
					  						OldFieldName =  "PrintDate",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "DateTime",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PrintDate",
					  						ListPropertyPath =  "PrintDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "PrintDate",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PrintDate",
					  						DefaultText =  "Print Date",
					  						ListFieldLable =  "PrintDateListLable",
					  						ListLableDefaultText =  "Print Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "PrintDate",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsInvoiceNumberManuallySet",
					  						OldFieldName =  "IsInvoiceNumberManuallySet",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsInvoiceNumberManuallySet",
					  						ListPropertyPath =  "IsInvoiceNumberManuallySet",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "IsInvoiceNumberManuallySet",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsInvoiceNumberManuallySet",
					  						DefaultText =  "Manually Set",
					  						ListFieldLable =  "IsInvoiceNumberManuallySetListLable",
					  						ListLableDefaultText =  "Number Manually Set",
					  						HelpTextCode =  "IsInvoiceNumberManuallySet",
					  						HelpTextDefaultText =  "To enter the invoice number manually, select this option and type in the number in Invoice Number.",
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
					 
					 						FieldName =  "Sent",
					  						OldFieldName =  "Sent",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						DataTemplateName =  "ARInvoiceSentDataTemplate",
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Sent",
					  						ListPropertyPath =  "Sent",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "Sent",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						ColumnHeaderTemplateName =  "ARInvoiceSentHeaderTemplate",
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Sent",
					  						DefaultText =  "Sent",
					  						ListFieldLable =  "SentListLable",
					  						ListLableDefaultText =  "Sent",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						HelpTextCode =  "Sent",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ReadyForTransfer",
					  						OldFieldName =  "ReadyForTransfer",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  10,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						DataTemplateName =  "ReadyForTransferDataTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ReadyForTransfer",
					  						ListPropertyPath =  "ReadyForTransfer",
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
					  						Code =  "ReadyForTransfer",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ReadyForTransfer",
					  						DefaultText =  "Ready For Transfer",
					  						FullLocalDefaultText =  "מוכן להעברה",
					  						ListFieldLable =  "ReadyForTransferListLable",
					  						ListLableDefaultText =  "Ready",
					  						ListLocalDefaultText =  "מוכן להעברה",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						HelpTextCode =  "ReadyForTransfer",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DebitAccount",
					  						OldFieldName =  "DebitAccount",
					  						ObjectTableName =  "ARInvoice",
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
					  						PMPropertyPath =  "DebitAccount",
					  						ListPropertyPath =  "DebitAccount",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "DebitAccount",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DebitAccount",
					  						DefaultText =  "Debit Account",
					  						FullLocalDefaultText =  "כרטיס חובה",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "DebitAccount",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LocalCurrencyCode",
					  						OldFieldName =  "LocalCurrencyCode",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  3,
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
					  						PMPropertyPath =  "LocalCurrencyCode",
					  						ListPropertyPath =  "LocalCurrencyCode",
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
					  						Code =  "LocalCurrencyCode",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LocalCurrencyCode",
					  						DefaultText =  "Local Currency",
					  						FullLocalDefaultText =  "מטבע מקומי",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "LocalCurrencyCode",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ProfitCurrencyCode",
					  						OldFieldName =  "ProfitCurrencyCode",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  3,
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
					  						PMPropertyPath =  "ProfitCurrencyCode",
					  						ListPropertyPath =  "ProfitCurrencyCode",
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
					  						Code =  "ProfitCurrencyCode",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ProfitCurrencyCode",
					  						DefaultText =  "Profit Currency",
					  						FullLocalDefaultText =  "מטבע רווח",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "ProfitCurrencyCode",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TransferError",
					  						OldFieldName =  "TransferError",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "Text",
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
					  						DataTemplateName =  "TransferErrorDataTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TransferError",
					  						ListPropertyPath =  "TransferError",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "TransferError",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TransferError",
					  						DefaultText =  "Transfer Error",
					  						FullLocalDefaultText =  "שגיאת העברה",
					  						ListFieldLable =  "TransferErrorListLable",
					  						ListLableDefaultText =  "Transfer Error",
					  						ListLocalDefaultText =  "שגיאת העברה",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						HelpTextCode =  "TransferError",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "NotReadyInvoices",
					  						OldFieldName =  "NotReadyInvoices",
					  						ObjectTableName =  "ARInvoice",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "NotReadyInvoices",
					  						ListPropertyPath =  "NotReadyInvoices",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "NotReadyInvoices",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "NotReadyInvoices",
					  						DefaultText =  "Not Ready Invoices",
					  						FullLocalDefaultText =  "חשבוניות לא מוכנות",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "NotReadyInvoices",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AccountingExternalCode",
					  						OldFieldName =  "AccountingExternalCode",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "nText",
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
					  						PMPropertyPath =  "AccountingExternalCode",
					  						ListPropertyPath =  "AccountingExternalCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "AccountingExternalCode",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AccountingExternalCode",
					  						DefaultText =  "External ID",
					  						FullLocalDefaultText =  "חישוב מזהים חיצונים מחדש",
					  						ListFieldLable =  "AccountingExternalCodeListLable",
					  						ListLableDefaultText =  "External ID",
					  						ListLocalDefaultText =  "חישוב מזהים חיצונים מחדש",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "AccountingExternalCode",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "HouseNumber",
					  						OldFieldName =  "HouseNumber",
					  						ObjectTableName =  "ARInvoice",
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
					  						SystemMaxLength =  20,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "HouseNumber",
					  						ListPropertyPath =  "HouseNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						ValidForQuerySection2 =  "InvoiceFollowUp",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "HouseNumber",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "HouseNumber",
					  						DefaultText =  "House Number",
					  						FullLocalDefaultText =  "מספר שטר מטען פנימי",
					  						ListFieldLable =  "HouseNumberListLable",
					  						ListLableDefaultText =  "House Number",
					  						ListLocalDefaultText =  "מספר שטר מטען פנימי",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "HouseNumber",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "MasterNumber",
					  						OldFieldName =  "MasterNumber",
					  						ObjectTableName =  "ARInvoice",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "MasterNumber",
					  						ListPropertyPath =  "MasterNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						ValidForQuerySection2 =  "InvoiceFollowUp",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "MasterNumber",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "MasterNumber",
					  						DefaultText =  "Master Number",
					  						FullLocalDefaultText =  "מספר שטר מטען ראשי",
					  						ListFieldLable =  "MasterNumberListLable",
					  						ListLableDefaultText =  "Master Number",
					  						ListLocalDefaultText =  "מספר שטר מטען ראשי",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "MasterNumber",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TransferStatusCode",
					  						OldFieldName =  "TransferStatusCode",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "ARInvoiceTransferStatus",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TransferStatusCode",
					  						ListPropertyPath =  "TransferStatusCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "TransferStatusCode",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TransferStatusCode",
					  						DefaultText =  "Transfer Status",
					  						FullLocalDefaultText =  "סטטוס העברה",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "TransferStatusCode",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "MarkedAsBlockedForTransfer",
					  						OldFieldName =  "MarkedAsBlockedForTransfer",
					  						ObjectTableName =  "ARInvoice",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "MarkedAsBlockedForTransfer",
					  						ListPropertyPath =  "MarkedAsBlockedForTransfer",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "MarkedAsBlockedForTransfer",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "MarkedAsBlockedForTransfer",
					  						DefaultText =  "Marked as blocked for transfer",
					  						FullLocalDefaultText =  "סמן כחסום להעברה",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "MarkedAsBlockedForTransfer",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BranchId",
					  						OldFieldName =  "BranchId",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Branch",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
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
					  						PMPropertyPath =  "BranchId",
					  						ListPropertyPath =  "BranchId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "BranchId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "BranchId",
					  						DefaultText =  "Branch",
					  						FullLocalDefaultText =  "סניף",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "BranchId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ErrorInTransferInvoices",
					  						OldFieldName =  "ErrorInTransferInvoices",
					  						ObjectTableName =  "ARInvoice",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ErrorInTransferInvoices",
					  						ListPropertyPath =  "ErrorInTransferInvoices",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ErrorInTransferInvoices",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ErrorInTransferInvoices",
					  						DefaultText =  "Error In Transfer Invoices",
					  						FullLocalDefaultText =  "שגיאה בהעברת חשבוניות",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "ErrorInTransferInvoices",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CustomerRef",
					  						OldFieldName =  "CustomerRef",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  105,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  105,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						ValidForQuerySection2 =  "InvoiceFollowUp",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "CustomerRef",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CustomerRef",
					  						DefaultText =  "Customer Ref",
					  						FullLocalDefaultText =  "אסמכתא",
					  						ListFieldLable =  "CustomerRefListLable",
					  						ListLableDefaultText =  "Customer Ref",
					  						ListLocalDefaultText =  "אסמכתא",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "CustomerRef",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsConstituentInvoice",
					  						OldFieldName =  "IsConstituentInvoice",
					  						ObjectTableName =  "ARInvoice",
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
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsConstituentInvoice",
					  						ListPropertyPath =  "IsConstituentInvoice",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "IsConstituentInvoice",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsConstituentInvoice",
					  						DefaultText =  "Is Constituent",
					  						FullLocalDefaultText =  "תעודת אשראי",
					  						HelpTextCode =  "IsConstituentInvoice",
					  						HelpTextDefaultText =  "This feature can be enabled from the card level / billing tab",
					  						HelpLocalDefaultText =  "ניתן להוסיף יכולת זו ברמת הכרטיס/ לשונית התחשבנות",
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
					 
					 						FieldName =  "ConsolidationInvoiceId",
					  						OldFieldName =  "ConsolidationInvoiceId",
					  						ObjectTableName =  "ARInvoice",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ConsolidationInvoiceId",
					  						ListPropertyPath =  "ConsolidationInvoiceId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ConsolidationInvoiceId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ConsolidationInvoiceId",
					  						DefaultText =  "Consolidation Invoice",
					  						FullLocalDefaultText =  "חשבונית איחוד",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "ConsolidationInvoiceId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsConsolidationInvoice",
					  						OldFieldName =  "IsConsolidationInvoice",
					  						ObjectTableName =  "ARInvoice",
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsConsolidationInvoice",
					  						ListPropertyPath =  "IsConsolidationInvoice",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "IsConsolidationInvoice",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsConsolidationInvoice",
					  						DefaultText =  "Is Consolidation",
					  						FullLocalDefaultText =  "איחוד",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "IsConsolidationInvoice",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "OpenConstituentInvoices",
					  						OldFieldName =  "OpenConstituentInvoices",
					  						ObjectTableName =  "ARInvoice",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "OpenConstituentInvoices",
					  						ListPropertyPath =  "OpenConstituentInvoices",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "OpenConstituentInvoices",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "OpenConstituentInvoices",
					  						DefaultText =  "Open Constituent Invoices",
					  						FullLocalDefaultText =  "פתיחת חשבונית איחוד",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "OpenConstituentInvoices",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TransferTries",
					  						OldFieldName =  "TransferTries",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "Integer",
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
					  						PMPropertyPath =  "TransferTries",
					  						ListPropertyPath =  "TransferTries",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "TransferTries",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TransferTries",
					  						DefaultText =  "Transfer tries",
					  						FullLocalDefaultText =  "נסיונות העברה",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "TransferTries",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsTransferStarted",
					  						OldFieldName =  "IsTransferStarted",
					  						ObjectTableName =  "ARInvoice",
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
					  						PMPropertyPath =  "IsTransferStarted",
					  						ListPropertyPath =  "IsTransferStarted",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "IsTransferStarted",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsTransferStarted",
					  						DefaultText =  "Is Transfer Started",
					  						FullLocalDefaultText =  "החלה העברה",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "IsTransferStarted",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TransferStatusName",
					  						OldFieldName =  "TransferStatusName",
					  						ObjectTableName =  "ARInvoice",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TransferStatusName",
					  						ListPropertyPath =  "TransferStatusName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "TransferStatusName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TransferStatusName",
					  						DefaultText =  "Transfer Status",
					  						FullLocalDefaultText =  "סטטוס העברה",
					  						ListFieldLable =  "TransferStatusNameListLable",
					  						ListLableDefaultText =  "Transfer Status",
					  						ListLocalDefaultText =  "סטטוס העברה",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "TransferStatusName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BillToCode",
					  						OldFieldName =  "BillToCode",
					  						ObjectTableName =  "ARInvoice",
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
					  						PMPropertyPath =  "BillToCode",
					  						ListPropertyPath =  "BillToCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "BillToCode",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "BillToCode",
					  						DefaultText =  "Bill to Code",
					  						FullLocalDefaultText =  "מספר לקוח",
					  						ListFieldLable =  "BillToCodeListLable",
					  						ListLableDefaultText =  "Bill to Code",
					  						ListLocalDefaultText =  "מספר לקוח",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "BillToCode",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ApprovedDate",
					  						OldFieldName =  "ApprovedDate",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "DateTime",
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
					  						DisplayInList =  true,
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ApprovedDate",
					  						ListPropertyPath =  "ApprovedDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ApprovedDate",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ApprovedDate",
					  						DefaultText =  "Approved Date",
					  						FullLocalDefaultText =  "תאריך אישור",
					  						ListFieldLable =  "ApprovedDateListLable",
					  						ListLableDefaultText =  "Approved Date",
					  						ListLocalDefaultText =  "תאריך אישור",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "ApprovedDate",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ApprovedByUserName",
					  						OldFieldName =  "ApprovedByUserName",
					  						ObjectTableName =  "ARInvoice",
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
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ApprovedByUserName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ApprovedByUserName",
					  						DefaultText =  "Approved By",
					  						FullLocalDefaultText =  "אושר ע''י",
					  						ListFieldLable =  "ApprovedByUserNameListLable",
					  						ListLableDefaultText =  "Approved By",
					  						ListLocalDefaultText =  "אושר ע''י",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "ApprovedByUserName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ApprovedByUserId",
					  						OldFieldName =  "ApprovedByUserId",
					  						ObjectTableName =  "ARInvoice",
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
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
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
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ApprovedByUserId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ApprovedByUserId",
					  						DefaultText =  "Approved By",
					  						FullLocalDefaultText =  "אושר ע''י",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "ApprovedByUserId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "OperationalDate",
					  						OldFieldName =  "OperationalDate",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "DateTime",
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
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "OperationalDate",
					  						ListPropertyPath =  "OperationalDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "OperationalDate",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "OperationalDate",
					  						DefaultText =  "Operational Date",
					  						FullLocalDefaultText =  "תאריך תפעולי",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "OperationalDate",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DateForVATInterest",
					  						OldFieldName =  "DateForVATInterest",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "DateTime",
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
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DateForVATInterest",
					  						ListPropertyPath =  "DateForVATInterest",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "DateForVATInterest",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DateForVATInterest",
					  						DefaultText =  "Date for VAT interest",
					  						FullLocalDefaultText =  "תאריך לריבית",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "DateForVATInterest",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SplitJournalByCurrency",
					  						OldFieldName =  "SplitJournalByCurrency",
					  						ObjectTableName =  "ARInvoice",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "SplitJournalByCurrency",
					  						ListPropertyPath =  "SplitJournalByCurrency",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "SplitJournalByCurrency",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SplitJournalByCurrency",
					  						DefaultText =  "Split journal by currency",
					  						FullLocalDefaultText =  "פיצול פקודת יומן לפי מטבע",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "SplitJournalByCurrency",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsExternalEntity",
					  						OldFieldName =  "IsExternalEntity",
					  						ObjectTableName =  "ARInvoice",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsExternalEntity",
					  						ListPropertyPath =  "IsExternalEntity",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "IsExternalEntity",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsExternalEntity",
					  						DefaultText =  "Is external entity",
					  						FullLocalDefaultText =  "ישות חיצונית",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "IsExternalEntity",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsGeneralInvoice",
					  						OldFieldName =  "IsGeneralInvoice",
					  						ObjectTableName =  "ARInvoice",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsGeneralInvoice",
					  						ListPropertyPath =  "IsGeneralInvoice",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "IsGeneralInvoice",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsGeneralInvoice",
					  						DefaultText =  "Is General Invoice",
					  						FullLocalDefaultText =  "חשבונית כללית",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "IsGeneralInvoice",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DraftGeneralInvoices",
					  						OldFieldName =  "DraftGeneralInvoices",
					  						ObjectTableName =  "ARInvoice",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DraftGeneralInvoices",
					  						ListPropertyPath =  "DraftGeneralInvoices",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "DraftGeneralInvoices",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DraftGeneralInvoices",
					  						DefaultText =  "Draft General Invoices",
					  						FullLocalDefaultText =  "טיוטות חשבוניות כלליות",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "DraftGeneralInvoices",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ApprovalGeneralInvoices",
					  						OldFieldName =  "ApprovalGeneralInvoices",
					  						ObjectTableName =  "ARInvoice",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ApprovalGeneralInvoices",
					  						ListPropertyPath =  "ApprovalGeneralInvoices",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ApprovalGeneralInvoices",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ApprovalGeneralInvoices",
					  						DefaultText =  "Approval General Invoices",
					  						FullLocalDefaultText =  "חשבניות כלליות מאושרות",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "ApprovalGeneralInvoices",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "RequestedPaymentMethodID",
					  						OldFieldName =  "RequestedPaymentMethodID",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  2,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "RequestedPaymentMethodID",
					  						ListPropertyPath =  "RequestedPaymentMethodID",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
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
					  						FullFieldLable =  "RequestedPaymentMethodID",
					  						DefaultText =  "Requested Payment Method",
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
					 
					 						FieldName =  "SATPaymentMethodCode",
					  						OldFieldName =  "SATPaymentMethodCode",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "SATPaymentMethod",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
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
					  						PMPropertyPath =  "SATPaymentMethodCode",
					  						ListPropertyPath =  "SATPaymentMethodCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "SATPaymentMethodCode",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SATPaymentMethodCode",
					  						DefaultText =  "Forma Pago",
					  						FullLocalDefaultText =  "SAT Payment Method",
					  						HelpTextCode =  "SATPaymentMethodCode",
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
					 
					 						FieldName =  "JournalId",
					  						OldFieldName =  "JournalId",
					  						ObjectTableName =  "ARInvoice",
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
					  						PMPropertyPath =  "JournalId",
					  						ListPropertyPath =  "JournalId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						Code =  "JournalId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "JournalId",
					  						DefaultText =  "Journal ",
					  						FullLocalDefaultText =  "פקודת יומן",
					  						ListFieldLable =  "JournalIdListLable",
					  						ListLableDefaultText =  "JournalId",
					  						ListLocalDefaultText =  "JournalId",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						HelpTextCode =  "JournalId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "JournalNumber",
					  						OldFieldName =  "JournalNumber",
					  						ObjectTableName =  "ARInvoice",
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
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						Code =  "JournalNumber",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "JournalNumber",
					  						DefaultText =  "Journal No.",
					  						FullLocalDefaultText =  "מספר פקודה",
					  						ListFieldLable =  "JournalNumberListLable",
					  						ListLableDefaultText =  "Journal No.",
					  						ListLocalDefaultText =  "מספר פקודה",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						HelpTextCode =  "JournalNumber",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IssuedByUserName",
					  						OldFieldName =  "IssuedByUserName",
					  						ObjectTableName =  "ARInvoice",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IssuedByUserName",
					  						ListPropertyPath =  "IssuedByUserName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "IssuedByUserName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IssuedByUserName",
					  						DefaultText =  "Issued By User",
					  						FullLocalDefaultText =  "IssuedByUserName",
					  						ListFieldLable =  "IssuedByUserNameListLable",
					  						ListLableDefaultText =  "Issued By User",
					  						ListLocalDefaultText =  "IssuedByUserName",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "IssuedByUserName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AmountPaid",
					  						OldFieldName =  "AmountPaid",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "Double",
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
					  						PMPropertyPath =  "AmountPaid",
					  						ListPropertyPath =  "AmountPaid",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "AmountPaid",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AmountPaid",
					  						DefaultText =  "Amount To Pay",
					  						FullLocalDefaultText =  "סכום לתשלום",
					  						ListFieldLable =  "AmountPaidListLable",
					  						ListLableDefaultText =  "AmountPaid",
					  						ListLocalDefaultText =  "AmountPaid",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "AmountPaid",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TransmissionError",
					  						OldFieldName =  "TransmissionError",
					  						ObjectTableName =  "ARInvoice",
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
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						DataTemplateName =  "TransferErrorDataTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TransmissionError",
					  						ListPropertyPath =  "TransmissionError",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "TransmissionError",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TransmissionError",
					  						DefaultText =  "Transmission Error",
					  						FullLocalDefaultText =  "שגיאת העברה",
					  						ListFieldLable =  "TransmissionErrorListLable",
					  						ListLableDefaultText =  "Transmission Error",
					  						ListLocalDefaultText =  "שגיאת העברה",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						HelpTextCode =  "TransmissionError",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "RelatedInvoice",
					  						OldFieldName =  "RelatedInvoice",
					  						ObjectTableName =  "ARInvoice",
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
					  						SystemMaxLength =  20,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "RelatedInvoice",
					  						ListPropertyPath =  "RelatedInvoice",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "RelatedInvoice",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "RelatedInvoice",
					  						DefaultText =  "Related Invoice",
					  						FullLocalDefaultText =  "חשבוניות מקושרות",
					  						ListFieldLable =  "RelatedInvoiceListLable",
					  						ListLableDefaultText =  "RelatedInvoice",
					  						ListLocalDefaultText =  "TransmissionError",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "RelatedInvoice",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "MetodoPagoCode",
					  						OldFieldName =  "MetodoPagoCode",
					  						ObjectTableName =  "ARInvoice",
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
					  						ValidForQuerySection1 =  "ARInvoice",
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
					  						ObjectTableName =  "ARInvoice",
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
					  						ValidForQuerySection1 =  "ARInvoice",
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
					 
					 						FieldName =  "SATTransferStatusCode",
					  						OldFieldName =  "SATTransferStatusCode",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "SATTransferStatus",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
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
					  						PMPropertyPath =  "SATTransferStatusCode",
					  						ListPropertyPath =  "SATTransferStatusCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "SATTransferStatusCode",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SATTransferStatusCode",
					  						DefaultText =  "SAT Transfer Status",
					  						ListFieldLable =  "SATTransferStatusCodeListLable",
					  						ListLableDefaultText =  "SAT TransferStatus Code",
					  						ListLocalDefaultText =  "UsoCFDI",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "SATTransferStatusCode",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SATInvoiceStatusCode",
					  						OldFieldName =  "SATInvoiceStatusCode",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "SATInvoiceStatus",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
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
					  						PMPropertyPath =  "SATInvoiceStatusCode",
					  						ListPropertyPath =  "SATInvoiceStatusCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "SATInvoiceStatusCode",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SATInvoiceStatusCode",
					  						DefaultText =  "SAT Invoice Status",
					  						ListFieldLable =  "SATInvoiceStatusCodeListLable",
					  						ListLableDefaultText =  "SAT TransferStatus Code",
					  						ListLocalDefaultText =  "UsoCFDI",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "SATInvoiceStatusCode",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsMultiCurrency",
					  						OldFieldName =  "IsMultiCurrency",
					  						ObjectTableName =  "ARInvoice",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsMultiCurrency",
					  						ListPropertyPath =  "IsMultiCurrency",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "IsMultiCurrency",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsMultiCurrency",
					  						DefaultText =  "Multi Currency",
					  						FullLocalDefaultText =  "חשבונית רב מטבעית",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "IsMultiCurrency",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SalesmanUserId",
					  						OldFieldName =  "SalesmanUserId",
					  						ObjectTableName =  "ARInvoice",
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
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "SalesmanUserId",
					  						ListPropertyPath =  "SalesmanUserId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter3Value =  "True",
					  						DependencyFilter3Type =  "Constant",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "SalesmanUserId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SalesmanUserId",
					  						DefaultText =  "Salesman",
					  						FullLocalDefaultText =  "איש מכירות",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "SalesmanUserId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SalesmanUserName",
					  						OldFieldName =  "SalesmanUserName",
					  						ObjectTableName =  "ARInvoice",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "SalesmanUserName",
					  						ListPropertyPath =  "SalesmanUserName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						Code =  "SalesmanUserName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SalesmanUserName",
					  						DefaultText =  "Salesman",
					  						FullLocalDefaultText =  "איש מכירות",
					  						ListFieldLable =  "SalesmanUserNameListLable",
					  						ListLableDefaultText =  "Salesman",
					  						ListLocalDefaultText =  "איש מכירות",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "SalesmanUserName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsCustomsChargesOnly",
					  						OldFieldName =  "IsCustomsChargesOnly",
					  						ObjectTableName =  "ARInvoice",
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
					  						PMPropertyPath =  "IsCustomsChargesOnly",
					  						ListPropertyPath =  "IsCustomsChargesOnly",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "IsCustomsChargesOnly",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsCustomsChargesOnly",
					  						DefaultText =  "Is Customs Only",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "IsCustomsChargesOnly",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsCustomsInvoice",
					  						OldFieldName =  "IsCustomsInvoice",
					  						ObjectTableName =  "ARInvoice",
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsCustomsInvoice",
					  						ListPropertyPath =  "IsCustomsInvoice",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "IsCustomsInvoice",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsCustomsInvoice",
					  						DefaultText =  "Is Customs",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "IsCustomsInvoice",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BillToAccountManagerName",
					  						OldFieldName =  "BillToAccountManagerName",
					  						ObjectTableName =  "ARInvoice",
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
					  						PMPropertyPath =  "BillToAccountManagerName",
					  						ListPropertyPath =  "BillToAccountManagerName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "BillToAccountManagerName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "BillToAccountManagerName",
					  						DefaultText =  "Bill to account manager",
					  						FullLocalDefaultText =  "מנהל לקוח",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "BillToAccountManagerName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SATTransferStatusName",
					  						OldFieldName =  "SATTransferStatusName",
					  						ObjectTableName =  "ARInvoice",
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
					  						PMPropertyPath =  "SATTransferStatusName",
					  						ListPropertyPath =  "SATTransferStatusName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						Code =  "SATTransferStatusName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SATTransferStatusName",
					  						DefaultText =  "SAT Transfer Status",
					  						ListFieldLable =  "SATTransferStatusNameListLable",
					  						ListLableDefaultText =  "SAT Transfer Status",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						HelpTextCode =  "SATTransferStatusName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SATInvoiceStatusName",
					  						OldFieldName =  "SATInvoiceStatusName",
					  						ObjectTableName =  "ARInvoice",
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
					  						PMPropertyPath =  "SATInvoiceStatusName",
					  						ListPropertyPath =  "SATInvoiceStatusName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						Code =  "SATInvoiceStatusName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SATInvoiceStatusName",
					  						DefaultText =  "SAT Invoice Status",
					  						ListFieldLable =  "SATInvoiceStatusNameListLable",
					  						ListLableDefaultText =  "SAT Invoice Status",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "SATInvoiceStatusName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Intercompany",
					  						OldFieldName =  "Intercompany",
					  						ObjectTableName =  "ARInvoice",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Intercompany",
					  						ListPropertyPath =  "Intercompany",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "Intercompany",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Intercompany",
					  						DefaultText =  "Intercompany",
					  						ListFieldLable =  "IntercompanyListLable",
					  						ListLableDefaultText =  "Intercompany",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Intercompany",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BankAccountLiteId",
					  						OldFieldName =  "BankAccountLiteId",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "BankAccountLite",
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
					  						PMPropertyPath =  "BankAccountLiteId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "BankAccountLiteId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "BankAccountLiteId",
					  						DefaultText =  "Deposit Bank",
					  						FullLocalDefaultText =  "בנק להפקדה",
					  						ListFieldLable =  "BankAccountLiteIdListLable",
					  						ListLableDefaultText =  "Deposit Bank",
					  						ListLocalDefaultText =  "בנק להפקדה",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "BankAccountLiteId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TotalAmountForTaxReport",
					  						OldFieldName =  "TotalAmountForTaxReport",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "Decimal",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "TotalAmountForTaxReport",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  16,
					  						DigitsAfterPoint =  2,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TotalAmountForTaxReport",
					  						DefaultText =  "Total Amount For Tax Report",
					  						FullLocalDefaultText =  "סה''כ סכום חשבונית לדוח מע''מ",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "TotalAmountForTaxReport",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TotaVatableAmountForTaxReport",
					  						OldFieldName =  "TotaVatableAmountForTaxReport",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "Decimal",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "TotaVatableAmountForTaxReport",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  16,
					  						DigitsAfterPoint =  2,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TotaVatableAmountForTaxReport",
					  						DefaultText =  "Total  Vatable Amount For Tax Report",
					  						FullLocalDefaultText =  "סה''כ סכום חייב במע''מ לדוח מע''מ",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "TotaVatableAmountForTaxReport",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TotalVAT",
					  						OldFieldName =  "TotalVAT",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "Decimal",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "TotalVAT",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  16,
					  						DigitsAfterPoint =  2,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TotalVAT",
					  						DefaultText =  "Total VAT Amount",
					  						FullLocalDefaultText =  "סה''כ מע''מ לדוח מע''מ",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "TotalVAT",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsFullAccounting",
					  						OldFieldName =  "IsFullAccounting",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "Boolean",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsFullAccounting",
					  						ListPropertyPath =  "IsFullAccounting",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsFullAccounting",
					  						DefaultText =  "Full Accounting",
					  						FullLocalDefaultText =  "הנה\"ח מלאה",
					  						ListFieldLable =  "IsFullAccountingListLable",
					  						ListLableDefaultText =  "Full Accounting",
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
					 
					 						FieldName =  "CreditARInvoice",
					  						OldFieldName =  "CreditARInvoice",
					  						ObjectTableName =  "ARInvoice",
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
					  						PMPropertyPath =  "CreditARInvoice",
					  						ListPropertyPath =  "CreditARInvoice",
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
					  						FullFieldLable =  "CreditARInvoice",
					  						DefaultText =  "CreditARInvoice",
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
					 
					 						FieldName =  "ConcurrencyGUID",
					  						OldFieldName =  "ConcurrencyGUID",
					  						ObjectTableName =  "ARInvoice",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ConcurrencyGUID",
					  						ListPropertyPath =  "ConcurrencyGUID",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ConcurrencyGUID",
					  						DefaultText =  "ConcurrencyGUID",
					  						ListFieldLable =  "ConcurrencyGUIDListLable",
					  						ListLableDefaultText =  "ConcurrencyGUID",
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
					 
					 						FieldName =  "NewConcurrencyGUID",
					  						OldFieldName =  "NewConcurrencyGUID",
					  						ObjectTableName =  "ARInvoice",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "NewConcurrencyGUID",
					  						ListPropertyPath =  "NewConcurrencyGUID",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "NewConcurrencyGUID",
					  						DefaultText =  "NewConcurrencyGUID",
					  						ListFieldLable =  "NewConcurrencyGUIDListLable",
					  						ListLableDefaultText =  "NewConcurrencyGUID",
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
					 
					 						FieldName =  "ARInvoiceStockId",
					  						OldFieldName =  "ARInvoiceStockId",
					  						ObjectTableName =  "ARInvoice",
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
					  						PMPropertyPath =  "ARInvoiceStockId",
					  						ListPropertyPath =  "ARInvoiceStockId",
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
					  						FullFieldLable =  "ARInvoiceStockId",
					  						DefaultText =  "AR Invoice Stock",
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
					 
					 						FieldName =  "IsInvoiceNumberFromStock",
					  						OldFieldName =  "IsInvoiceNumberFromStock",
					  						ObjectTableName =  "ARInvoice",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsInvoiceNumberFromStock",
					  						ListPropertyPath =  "IsInvoiceNumberFromStock",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsInvoiceNumberFromStock",
					  						DefaultText =  "Number From Stock",
					  						ListFieldLable =  "IsInvoiceNumberFromStockListLable",
					  						ListLableDefaultText =  "Number From Stock",
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
					 
					 						FieldName =  "BillToLocalName",
					  						OldFieldName =  "BillToLocalName",
					  						ObjectTableName =  "ARInvoice",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "BillToLocalName",
					  						ListPropertyPath =  "BillToLocalName",
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
					  						FullFieldLable =  "BillToLocalName",
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
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DocumentFilingId",
					  						OldFieldName =  "DocumentFilingId",
					  						ObjectTableName =  "ARInvoice",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DocumentFilingId",
					  						ListPropertyPath =  "DocumentFilingId",
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
					  						FullFieldLable =  "DocumentFilingId",
					  						DefaultText =  "Document Filing",
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
					 
					 						FieldName =  "BranchName",
					  						OldFieldName =  "BranchName",
					  						ObjectTableName =  "ARInvoice",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "BranchName",
					  						ListPropertyPath =  "BranchName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "BranchName",
					  						DefaultText =  "Branch",
					  						ListFieldLable =  "BranchNameListLable",
					  						ListLableDefaultText =  "Branch",
					  						ListLocalDefaultText =  "סניף",
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
					 
					 						FieldName =  "OpenInvoices",
					  						OldFieldName =  "OpenInvoices",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "Boolean",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "OpenInvoices",
					  						ListPropertyPath =  "OpenInvoices",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "OpenInvoices",
					  						DefaultText =  "Open Invoices",
					  						FullLocalDefaultText =  "חשבוניות שלא שולמו",
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
					 
					 						FieldName =  "BillToCity",
					  						OldFieldName =  "BillToCity",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  25,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  25,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "BillToCity",
					  						ListPropertyPath =  "BillToCity",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "BillToCity",
					  						DefaultText =  "Bill To City",
					  						ListFieldLable =  "BillToCityListLable",
					  						ListLableDefaultText =  "Bill To City",
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
					 
					 						FieldName =  "BillToCountry",
					  						OldFieldName =  "BillToCountry",
					  						ObjectTableName =  "ARInvoice",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  120,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  120,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "BillToCountry",
					  						ListPropertyPath =  "BillToCountry",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "BillToCountry",
					  						DefaultText =  "Bill To Country",
					  						ListFieldLable =  "BillToCountryListLable",
					  						ListLableDefaultText =  "Bill To Country",
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
					 
					 						FieldName =  "CreatedByPartner",
					  						OldFieldName =  "CreatedByPartner",
					  						ObjectTableName =  "ARInvoice",
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
					  						SystemMaxLength =  25,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CreatedByPartner",
					  						ListPropertyPath =  "CreatedByPartner",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CreatedByPartner",
					  						DefaultText =  "Created By Partner",
					  						ListFieldLable =  "CreatedByPartnerListLable",
					  						ListLableDefaultText =  "Created By Partner",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  true,
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
	        QueryGroup ARInvoiceQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "INVC", Name = "Invoices" }, queryGroupRepository);
						QueryGroup ARInvoiceQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "9a70", Name = " Query Group" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable ARInvoiceObjectTable = objectContext.ObjectTables.Where(d => d.Name == "ARInvoice" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> ARInvoiceObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "ARInvoice").ToList();   

			   TextCode ARInvoiceTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.Q.AllInvoices", DefaultText = @"All Invoices",LocalDefaultText = "כל החשבוניות", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature ARInvoiceFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ALLINVOICES", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.AllInvoices", NameTextCodeDefaultText = "All Invoices", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode ARInvoiceTextCode_1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.Q.DraftGeneralInvoices", DefaultText = @"Draft Invoices",LocalDefaultText = "חשבוניות בסטטוס טיוטה", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature ARInvoiceFeature_1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DRAFTGENERALINVOICES", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.DraftGeneralInvoice", NameTextCodeDefaultText = "Draft General Invoice", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode ARInvoiceTextCode_2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.Q.ApprovalGeneralInvoices", DefaultText = @"Approved Invoices",LocalDefaultText = "חשבוניות מאושרות", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature ARInvoiceFeature_2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "APPROVALGENERALINVOICES", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.ApprovalGeneralInvoice", NameTextCodeDefaultText = "Aproval General Invoice", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode ARInvoiceTextCode_3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.Q.AllGeneralInvoices", DefaultText = @"All Invoices",LocalDefaultText = "כל החשבוניות", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature ARInvoiceFeature_3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ALLGENERALINVOICES", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.AllGeneralInvoice", NameTextCodeDefaultText = "All General Invoice", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode ARInvoiceTextCode_4 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.Q.DraftInvoices", DefaultText = @"Draft Invoices",LocalDefaultText = "חשבוניות בסטטוס טיוטה", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature ARInvoiceFeature_4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DRAFTINVOICES", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.DraftInvoices", NameTextCodeDefaultText = "Draft Invoices", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode ARInvoiceTextCode_5 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.Q.UnpaidInvoices", DefaultText = @"Unpaid Invoices",LocalDefaultText = "חשבוניות שלא שולמו", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature ARInvoiceFeature_5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UNPAIDINVOICES", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.UnpaidInvoices", NameTextCodeDefaultText = "Unpaid Invoices", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode ARInvoiceTextCode_6 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.Q.NotReadyInvoices", DefaultText = @"Not Ready Invoices",LocalDefaultText = "לא מוכן חשבוניות", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature ARInvoiceFeature_6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NOTREADYINVOICES", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.NotReadyInvoices", NameTextCodeDefaultText = "Not Ready Invoices", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode ARInvoiceTextCode_7 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.Q.MarkedAsBlockedForTransfer", DefaultText = @"Marked as blocked for transfer",LocalDefaultText = "מסומן כחסום לצורך העברה", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature ARInvoiceFeature_7 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MARKEDASBLOCKEDFORTRANSFER", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.MarkedAsBlockedForTransfer", NameTextCodeDefaultText = "Marked as blocked for transfer", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode ARInvoiceTextCode_8 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.Q.ErrorInTransferInvoices", DefaultText = @"Error In Transfer Invoices",LocalDefaultText = "שגיאה בהעברת חשבוניות", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature ARInvoiceFeature_8 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ERRORINTRANSFERINVOICES", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.ErrorInTransferInvoices", NameTextCodeDefaultText = "Error In Transfer Invoices", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode ARInvoiceTextCode_9 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.Q.OpenConstituentInvoices", DefaultText = @"Open Constituent",LocalDefaultText = "פתח מכונן", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature ARInvoiceFeature_9 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OPENCONSTUTUENT", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.OpenConstituentInvoices", NameTextCodeDefaultText = "Open Constituent Invoices", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode ARInvoiceTextCode_10 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.Q.FailedSAT", DefaultText = @"SAT Failed Invoices",LocalDefaultText = null, ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature ARInvoiceFeature_10 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SATFAILEDINVOICES", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.SATFailedInvoices", NameTextCodeDefaultText = "Invoices Failed to Open in SAT", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode ARInvoiceTextCode_11 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.Q.FailedSAT", DefaultText = @"SAT Failed Invoices",LocalDefaultText = null, ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature ARInvoiceFeature_11 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SATFAILEDINVOICES", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.SATFailedInvoices", NameTextCodeDefaultText = "Invoices Failed to Open in SAT", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode ARInvoiceTextCode_12 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.Q.OpenInvoices", DefaultText = @"Open Invoices",LocalDefaultText = "חשבוניות שלא שולמו", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature ARInvoiceFeature_12 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ARInvoice.Q.OpenInvoices", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoiceFeatures.OpenInvoices", NameTextCodeDefaultText = "OpenInvoices", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query AllInvoicesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = ARInvoiceTextCode_0.Id, NameTextCodeCode = ARInvoiceTextCode_0.Code, ObjectTableName = "ARInvoice", Code = "All Invoices",  QueryGroupCode = "INVC", IndexOrder = 0, Tenant = 0, ObjectTableId = ARInvoiceObjectTable.Id, QuerySection = "ARInvoice", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = ARInvoiceFeature_0.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn AllInvoicesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllInvoicesQuery.Id,QueryCode = AllInvoicesQuery.Code, IndexOrder = 0, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "IsPrinted" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "IsPrinted" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 30 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllInvoicesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllInvoicesQuery.Id,QueryCode = AllInvoicesQuery.Code, IndexOrder = 1, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "Sent" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "Sent" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 30 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllInvoicesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllInvoicesQuery.Id,QueryCode = AllInvoicesQuery.Code, IndexOrder = 2, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllInvoicesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllInvoicesQuery.Id,QueryCode = AllInvoicesQuery.Code, IndexOrder = 3, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "ARInvoiceTypeName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "ARInvoiceTypeName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllInvoicesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllInvoicesQuery.Id,QueryCode = AllInvoicesQuery.Code, IndexOrder = 4, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "BillToName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "BillToName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllInvoicesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllInvoicesQuery.Id,QueryCode = AllInvoicesQuery.Code, IndexOrder = 5, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllInvoicesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllInvoicesQuery.Id,QueryCode = AllInvoicesQuery.Code, IndexOrder = 6, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllInvoicesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllInvoicesQuery.Id,QueryCode = AllInvoicesQuery.Code, IndexOrder = 7, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllInvoicesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllInvoicesQuery.Id,QueryCode = AllInvoicesQuery.Code, IndexOrder = 8, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllInvoicesQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllInvoicesQuery.Id,QueryCode = AllInvoicesQuery.Code, IndexOrder = 9, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllInvoicesQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllInvoicesQuery.Id,QueryCode = AllInvoicesQuery.Code, IndexOrder = 10, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllInvoicesQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllInvoicesQuery.Id,QueryCode = AllInvoicesQuery.Code, IndexOrder = 11, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
  
	      

			  Query DraftGeneralInvoicesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = ARInvoiceTextCode_1.Id, NameTextCodeCode = ARInvoiceTextCode_1.Code, ObjectTableName = "ARInvoice", Code = "Draft General Invoices",  QueryGroupCode = "INVC", IndexOrder = 1, Tenant = 0, ObjectTableId = ARInvoiceObjectTable.Id, QuerySection = "ARInvoice", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = ARInvoiceFeature_1.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn DraftGeneralInvoicesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftGeneralInvoicesQuery.Id,QueryCode = DraftGeneralInvoicesQuery.Code, IndexOrder = 0, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "IsPrinted" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "IsPrinted" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 30 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftGeneralInvoicesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftGeneralInvoicesQuery.Id,QueryCode = DraftGeneralInvoicesQuery.Code, IndexOrder = 1, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "Sent" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "Sent" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 30 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftGeneralInvoicesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftGeneralInvoicesQuery.Id,QueryCode = DraftGeneralInvoicesQuery.Code, IndexOrder = 2, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftGeneralInvoicesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftGeneralInvoicesQuery.Id,QueryCode = DraftGeneralInvoicesQuery.Code, IndexOrder = 3, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "ARInvoiceTypeName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "ARInvoiceTypeName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftGeneralInvoicesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftGeneralInvoicesQuery.Id,QueryCode = DraftGeneralInvoicesQuery.Code, IndexOrder = 4, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "BillToName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "BillToName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftGeneralInvoicesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftGeneralInvoicesQuery.Id,QueryCode = DraftGeneralInvoicesQuery.Code, IndexOrder = 5, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftGeneralInvoicesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftGeneralInvoicesQuery.Id,QueryCode = DraftGeneralInvoicesQuery.Code, IndexOrder = 6, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftGeneralInvoicesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftGeneralInvoicesQuery.Id,QueryCode = DraftGeneralInvoicesQuery.Code, IndexOrder = 7, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftGeneralInvoicesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftGeneralInvoicesQuery.Id,QueryCode = DraftGeneralInvoicesQuery.Code, IndexOrder = 8, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftGeneralInvoicesQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftGeneralInvoicesQuery.Id,QueryCode = DraftGeneralInvoicesQuery.Code, IndexOrder = 9, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftGeneralInvoicesQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftGeneralInvoicesQuery.Id,QueryCode = DraftGeneralInvoicesQuery.Code, IndexOrder = 10, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftGeneralInvoicesQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftGeneralInvoicesQuery.Id,QueryCode = DraftGeneralInvoicesQuery.Code, IndexOrder = 11, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter DraftGeneralInvoicesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "DraftGeneralInvoices" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "DraftGeneralInvoices" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "true",PredefinedValue2 = null, QueryId = DraftGeneralInvoicesQuery.Id,QueryCode = DraftGeneralInvoicesQuery.Code, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query ApprovalGeneralInvoicesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = ARInvoiceTextCode_2.Id, NameTextCodeCode = ARInvoiceTextCode_2.Code, ObjectTableName = "ARInvoice", Code = "Approval General Invoices",  QueryGroupCode = "INVC", IndexOrder = 2, Tenant = 0, ObjectTableId = ARInvoiceObjectTable.Id, QuerySection = "ARInvoice", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = ARInvoiceFeature_2.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn ApprovalGeneralInvoicesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovalGeneralInvoicesQuery.Id,QueryCode = ApprovalGeneralInvoicesQuery.Code, IndexOrder = 0, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "IsPrinted" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "IsPrinted" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 30 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApprovalGeneralInvoicesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovalGeneralInvoicesQuery.Id,QueryCode = ApprovalGeneralInvoicesQuery.Code, IndexOrder = 1, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "Sent" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "Sent" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 30 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApprovalGeneralInvoicesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovalGeneralInvoicesQuery.Id,QueryCode = ApprovalGeneralInvoicesQuery.Code, IndexOrder = 2, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApprovalGeneralInvoicesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovalGeneralInvoicesQuery.Id,QueryCode = ApprovalGeneralInvoicesQuery.Code, IndexOrder = 3, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "ARInvoiceTypeName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "ARInvoiceTypeName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApprovalGeneralInvoicesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovalGeneralInvoicesQuery.Id,QueryCode = ApprovalGeneralInvoicesQuery.Code, IndexOrder = 4, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "BillToName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "BillToName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApprovalGeneralInvoicesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovalGeneralInvoicesQuery.Id,QueryCode = ApprovalGeneralInvoicesQuery.Code, IndexOrder = 5, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApprovalGeneralInvoicesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovalGeneralInvoicesQuery.Id,QueryCode = ApprovalGeneralInvoicesQuery.Code, IndexOrder = 6, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApprovalGeneralInvoicesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovalGeneralInvoicesQuery.Id,QueryCode = ApprovalGeneralInvoicesQuery.Code, IndexOrder = 7, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApprovalGeneralInvoicesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovalGeneralInvoicesQuery.Id,QueryCode = ApprovalGeneralInvoicesQuery.Code, IndexOrder = 8, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApprovalGeneralInvoicesQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovalGeneralInvoicesQuery.Id,QueryCode = ApprovalGeneralInvoicesQuery.Code, IndexOrder = 9, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApprovalGeneralInvoicesQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovalGeneralInvoicesQuery.Id,QueryCode = ApprovalGeneralInvoicesQuery.Code, IndexOrder = 10, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApprovalGeneralInvoicesQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovalGeneralInvoicesQuery.Id,QueryCode = ApprovalGeneralInvoicesQuery.Code, IndexOrder = 11, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter ApprovalGeneralInvoicesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "ApprovalGeneralInvoices" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "ApprovalGeneralInvoices" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "true",PredefinedValue2 = null, QueryId = ApprovalGeneralInvoicesQuery.Id,QueryCode = ApprovalGeneralInvoicesQuery.Code, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query AllGeneralInvoicesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = ARInvoiceTextCode_3.Id, NameTextCodeCode = ARInvoiceTextCode_3.Code, ObjectTableName = "ARInvoice", Code = "All General Invoices",  QueryGroupCode = "INVC", IndexOrder = 3, Tenant = 0, ObjectTableId = ARInvoiceObjectTable.Id, QuerySection = "ARInvoice", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = ARInvoiceFeature_3.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn AllGeneralInvoicesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGeneralInvoicesQuery.Id,QueryCode = AllGeneralInvoicesQuery.Code, IndexOrder = 0, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "IsPrinted" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "IsPrinted" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 30 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGeneralInvoicesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGeneralInvoicesQuery.Id,QueryCode = AllGeneralInvoicesQuery.Code, IndexOrder = 1, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "Sent" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "Sent" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 30 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGeneralInvoicesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGeneralInvoicesQuery.Id,QueryCode = AllGeneralInvoicesQuery.Code, IndexOrder = 2, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGeneralInvoicesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGeneralInvoicesQuery.Id,QueryCode = AllGeneralInvoicesQuery.Code, IndexOrder = 3, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "ARInvoiceTypeName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "ARInvoiceTypeName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGeneralInvoicesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGeneralInvoicesQuery.Id,QueryCode = AllGeneralInvoicesQuery.Code, IndexOrder = 4, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "BillToName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "BillToName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGeneralInvoicesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGeneralInvoicesQuery.Id,QueryCode = AllGeneralInvoicesQuery.Code, IndexOrder = 5, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGeneralInvoicesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGeneralInvoicesQuery.Id,QueryCode = AllGeneralInvoicesQuery.Code, IndexOrder = 6, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGeneralInvoicesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGeneralInvoicesQuery.Id,QueryCode = AllGeneralInvoicesQuery.Code, IndexOrder = 7, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGeneralInvoicesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGeneralInvoicesQuery.Id,QueryCode = AllGeneralInvoicesQuery.Code, IndexOrder = 8, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGeneralInvoicesQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGeneralInvoicesQuery.Id,QueryCode = AllGeneralInvoicesQuery.Code, IndexOrder = 9, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGeneralInvoicesQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGeneralInvoicesQuery.Id,QueryCode = AllGeneralInvoicesQuery.Code, IndexOrder = 10, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGeneralInvoicesQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGeneralInvoicesQuery.Id,QueryCode = AllGeneralInvoicesQuery.Code, IndexOrder = 11, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter AllGeneralInvoicesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "IsGeneralInvoice" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "IsGeneralInvoice" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "true",PredefinedValue2 = null, QueryId = AllGeneralInvoicesQuery.Id,QueryCode = AllGeneralInvoicesQuery.Code, Tenant = 0,Operator = "Equals"}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query DraftInvoicesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = ARInvoiceTextCode_4.Id, NameTextCodeCode = ARInvoiceTextCode_4.Code, ObjectTableName = "ARInvoice", Code = "Draft Invoices",  SpotlightDataTemplate = "ARInvoiceSpotlightDataTemplate",  QueryGroupCode = "INVC", IndexOrder = 4, Tenant = 0, ObjectTableId = ARInvoiceObjectTable.Id, QuerySection = "ARInvoice", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = ARInvoiceFeature_4.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn DraftInvoicesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftInvoicesQuery.Id,QueryCode = DraftInvoicesQuery.Code, IndexOrder = 0, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "IsPrinted" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "IsPrinted" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 30 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftInvoicesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftInvoicesQuery.Id,QueryCode = DraftInvoicesQuery.Code, IndexOrder = 1, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "Sent" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "Sent" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 30 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftInvoicesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftInvoicesQuery.Id,QueryCode = DraftInvoicesQuery.Code, IndexOrder = 2, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "DraftNumber" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "DraftNumber" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftInvoicesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftInvoicesQuery.Id,QueryCode = DraftInvoicesQuery.Code, IndexOrder = 3, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "ARInvoiceTypeName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "ARInvoiceTypeName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftInvoicesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftInvoicesQuery.Id,QueryCode = DraftInvoicesQuery.Code, IndexOrder = 4, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "BillToName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "BillToName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftInvoicesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftInvoicesQuery.Id,QueryCode = DraftInvoicesQuery.Code, IndexOrder = 5, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftInvoicesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftInvoicesQuery.Id,QueryCode = DraftInvoicesQuery.Code, IndexOrder = 6, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftInvoicesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftInvoicesQuery.Id,QueryCode = DraftInvoicesQuery.Code, IndexOrder = 7, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftInvoicesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftInvoicesQuery.Id,QueryCode = DraftInvoicesQuery.Code, IndexOrder = 8, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftInvoicesQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftInvoicesQuery.Id,QueryCode = DraftInvoicesQuery.Code, IndexOrder = 9, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftInvoicesQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftInvoicesQuery.Id,QueryCode = DraftInvoicesQuery.Code, IndexOrder = 10, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter DraftInvoicesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "StatusCode" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "StatusCode" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "DR",PredefinedValue2 = null, QueryId = DraftInvoicesQuery.Id,QueryCode = DraftInvoicesQuery.Code, Tenant = 0,Operator = "Equals"}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query UnpaidInvoicesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = ARInvoiceTextCode_5.Id, NameTextCodeCode = ARInvoiceTextCode_5.Code, ObjectTableName = "ARInvoice", Code = "Unpaid Invoices",  SpotlightDataTemplate = "ARInvoiceSpotlightDataTemplate",  QueryGroupCode = "INVC", IndexOrder = 5, Tenant = 0, ObjectTableId = ARInvoiceObjectTable.Id, QuerySection = "ARInvoice", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = ARInvoiceFeature_5.Id, DefaultSortName = "DueDate", DefaultSortDirection = "Ascending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn UnpaidInvoicesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnpaidInvoicesQuery.Id,QueryCode = UnpaidInvoicesQuery.Code, IndexOrder = 0, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "IsPrinted" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "IsPrinted" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 30 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn UnpaidInvoicesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnpaidInvoicesQuery.Id,QueryCode = UnpaidInvoicesQuery.Code, IndexOrder = 1, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "Sent" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "Sent" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 30 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn UnpaidInvoicesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnpaidInvoicesQuery.Id,QueryCode = UnpaidInvoicesQuery.Code, IndexOrder = 2, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn UnpaidInvoicesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnpaidInvoicesQuery.Id,QueryCode = UnpaidInvoicesQuery.Code, IndexOrder = 3, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "ARInvoiceTypeName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "ARInvoiceTypeName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn UnpaidInvoicesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnpaidInvoicesQuery.Id,QueryCode = UnpaidInvoicesQuery.Code, IndexOrder = 4, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "BillToName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "BillToName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn UnpaidInvoicesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnpaidInvoicesQuery.Id,QueryCode = UnpaidInvoicesQuery.Code, IndexOrder = 5, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn UnpaidInvoicesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnpaidInvoicesQuery.Id,QueryCode = UnpaidInvoicesQuery.Code, IndexOrder = 6, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn UnpaidInvoicesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnpaidInvoicesQuery.Id,QueryCode = UnpaidInvoicesQuery.Code, IndexOrder = 7, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn UnpaidInvoicesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnpaidInvoicesQuery.Id,QueryCode = UnpaidInvoicesQuery.Code, IndexOrder = 8, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn UnpaidInvoicesQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnpaidInvoicesQuery.Id,QueryCode = UnpaidInvoicesQuery.Code, IndexOrder = 9, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn UnpaidInvoicesQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnpaidInvoicesQuery.Id,QueryCode = UnpaidInvoicesQuery.Code, IndexOrder = 10, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "ExpectedPaymentDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "ExpectedPaymentDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn UnpaidInvoicesQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnpaidInvoicesQuery.Id,QueryCode = UnpaidInvoicesQuery.Code, IndexOrder = 11, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "InternalNotes" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "InternalNotes" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter UnpaidInvoicesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "UnpaidInvoices" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "UnpaidInvoices" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "true",PredefinedValue2 = null, QueryId = UnpaidInvoicesQuery.Id,QueryCode = UnpaidInvoicesQuery.Code, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query NotReadyInvoicesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = ARInvoiceTextCode_6.Id, NameTextCodeCode = ARInvoiceTextCode_6.Code, ObjectTableName = "ARInvoice", Code = "Not Ready Invoices",  EditWizardName = "Simplog.InvoiceLib.Views.Tabs.ARInvoiceTabs.ARTransferEditControl",
			   EditWizardComponentPath = "./InvoiceModules/ARInvoice/Components/NewEntity/ARInvoiceTransferTemplate",
			   QueryGroupCode = "INVC", IndexOrder = 6, Tenant = 0, ObjectTableId = ARInvoiceObjectTable.Id, QuerySection = "ARInvoice", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = ARInvoiceFeature_6.Id, DefaultSortName = "InvoiceDate", DefaultSortDirection = "Ascending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn NotReadyInvoicesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotReadyInvoicesQuery.Id,QueryCode = NotReadyInvoicesQuery.Code, IndexOrder = 0, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn NotReadyInvoicesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotReadyInvoicesQuery.Id,QueryCode = NotReadyInvoicesQuery.Code, IndexOrder = 1, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn NotReadyInvoicesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotReadyInvoicesQuery.Id,QueryCode = NotReadyInvoicesQuery.Code, IndexOrder = 2, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "BillToName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "BillToName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 190 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn NotReadyInvoicesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotReadyInvoicesQuery.Id,QueryCode = NotReadyInvoicesQuery.Code, IndexOrder = 3, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn NotReadyInvoicesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotReadyInvoicesQuery.Id,QueryCode = NotReadyInvoicesQuery.Code, IndexOrder = 4, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn NotReadyInvoicesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotReadyInvoicesQuery.Id,QueryCode = NotReadyInvoicesQuery.Code, IndexOrder = 5, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "ReadyForTransfer" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "ReadyForTransfer" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 50 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn NotReadyInvoicesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotReadyInvoicesQuery.Id,QueryCode = NotReadyInvoicesQuery.Code, IndexOrder = 6, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "TransferError" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "TransferError" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 500 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter NotReadyInvoicesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "NotReadyInvoices" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "NotReadyInvoices" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "true",PredefinedValue2 = null, QueryId = NotReadyInvoicesQuery.Id,QueryCode = NotReadyInvoicesQuery.Code, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query MarkedasblockedfortransferQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = ARInvoiceTextCode_7.Id, NameTextCodeCode = ARInvoiceTextCode_7.Code, ObjectTableName = "ARInvoice", Code = "Marked as blocked for transfer",  EditWizardName = "Simplog.InvoiceLib.Views.Tabs.ARInvoiceTabs.ARTransferEditControl",
			   EditWizardComponentPath = "./InvoiceModules/ARInvoice/Components/NewEntity/ARInvoiceTransferTemplate",
			   QueryGroupCode = "INVC", IndexOrder = 7, Tenant = 0, ObjectTableId = ARInvoiceObjectTable.Id, QuerySection = "ARInvoice", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = ARInvoiceFeature_7.Id, DefaultSortName = "InvoiceDate", DefaultSortDirection = "Ascending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn MarkedasblockedfortransferQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MarkedasblockedfortransferQuery.Id,QueryCode = MarkedasblockedfortransferQuery.Code, IndexOrder = 0, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MarkedasblockedfortransferQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MarkedasblockedfortransferQuery.Id,QueryCode = MarkedasblockedfortransferQuery.Code, IndexOrder = 1, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MarkedasblockedfortransferQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MarkedasblockedfortransferQuery.Id,QueryCode = MarkedasblockedfortransferQuery.Code, IndexOrder = 2, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "BillToName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "BillToName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 200 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MarkedasblockedfortransferQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MarkedasblockedfortransferQuery.Id,QueryCode = MarkedasblockedfortransferQuery.Code, IndexOrder = 3, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MarkedasblockedfortransferQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MarkedasblockedfortransferQuery.Id,QueryCode = MarkedasblockedfortransferQuery.Code, IndexOrder = 4, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter MarkedasblockedfortransferQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "MarkedAsBlockedForTransfer" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "MarkedAsBlockedForTransfer" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "true",PredefinedValue2 = null, QueryId = MarkedasblockedfortransferQuery.Id,QueryCode = MarkedasblockedfortransferQuery.Code, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query ErrorInTransferInvoicesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = ARInvoiceTextCode_8.Id, NameTextCodeCode = ARInvoiceTextCode_8.Code, ObjectTableName = "ARInvoice", Code = "Error In Transfer Invoices",  EditWizardName = "Simplog.InvoiceLib.Views.Tabs.ARInvoiceTabs.ARTransferEditControl",
			   EditWizardComponentPath = "./InvoiceModules/ARInvoice/Components/NewEntity/ARInvoiceTransferTemplate",
			   QueryGroupCode = "INVC", IndexOrder = 8, Tenant = 0, ObjectTableId = ARInvoiceObjectTable.Id, QuerySection = "ARInvoice", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = ARInvoiceFeature_8.Id, DefaultSortName = "InvoiceDate", DefaultSortDirection = "Ascending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn ErrorInTransferInvoicesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ErrorInTransferInvoicesQuery.Id,QueryCode = ErrorInTransferInvoicesQuery.Code, IndexOrder = 0, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ErrorInTransferInvoicesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ErrorInTransferInvoicesQuery.Id,QueryCode = ErrorInTransferInvoicesQuery.Code, IndexOrder = 1, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ErrorInTransferInvoicesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ErrorInTransferInvoicesQuery.Id,QueryCode = ErrorInTransferInvoicesQuery.Code, IndexOrder = 2, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "BillToName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "BillToName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 190 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ErrorInTransferInvoicesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ErrorInTransferInvoicesQuery.Id,QueryCode = ErrorInTransferInvoicesQuery.Code, IndexOrder = 3, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ErrorInTransferInvoicesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ErrorInTransferInvoicesQuery.Id,QueryCode = ErrorInTransferInvoicesQuery.Code, IndexOrder = 4, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ErrorInTransferInvoicesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ErrorInTransferInvoicesQuery.Id,QueryCode = ErrorInTransferInvoicesQuery.Code, IndexOrder = 5, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "ReadyForTransfer" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "ReadyForTransfer" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 50 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ErrorInTransferInvoicesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ErrorInTransferInvoicesQuery.Id,QueryCode = ErrorInTransferInvoicesQuery.Code, IndexOrder = 6, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "TransferError" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "TransferError" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 500 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter ErrorInTransferInvoicesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "ErrorInTransferInvoices" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "ErrorInTransferInvoices" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "true",PredefinedValue2 = null, QueryId = ErrorInTransferInvoicesQuery.Id,QueryCode = ErrorInTransferInvoicesQuery.Code, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query OpenConstituentQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = ARInvoiceTextCode_9.Id, NameTextCodeCode = ARInvoiceTextCode_9.Code, ObjectTableName = "ARInvoice", Code = "OpenConstituent",  QueryGroupCode = "INVC", IndexOrder = 9, Tenant = 0, ObjectTableId = ARInvoiceObjectTable.Id, QuerySection = "ARInvoice", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = ARInvoiceFeature_9.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn OpenConstituentQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenConstituentQuery.Id,QueryCode = OpenConstituentQuery.Code, IndexOrder = 0, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "IsPrinted" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "IsPrinted" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 30 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenConstituentQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenConstituentQuery.Id,QueryCode = OpenConstituentQuery.Code, IndexOrder = 1, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "Sent" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "Sent" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 30 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenConstituentQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenConstituentQuery.Id,QueryCode = OpenConstituentQuery.Code, IndexOrder = 2, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenConstituentQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenConstituentQuery.Id,QueryCode = OpenConstituentQuery.Code, IndexOrder = 3, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "ARInvoiceTypeName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "ARInvoiceTypeName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenConstituentQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenConstituentQuery.Id,QueryCode = OpenConstituentQuery.Code, IndexOrder = 4, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "BillToName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "BillToName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenConstituentQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenConstituentQuery.Id,QueryCode = OpenConstituentQuery.Code, IndexOrder = 5, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenConstituentQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenConstituentQuery.Id,QueryCode = OpenConstituentQuery.Code, IndexOrder = 6, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenConstituentQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenConstituentQuery.Id,QueryCode = OpenConstituentQuery.Code, IndexOrder = 7, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenConstituentQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenConstituentQuery.Id,QueryCode = OpenConstituentQuery.Code, IndexOrder = 8, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenConstituentQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenConstituentQuery.Id,QueryCode = OpenConstituentQuery.Code, IndexOrder = 9, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenConstituentQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenConstituentQuery.Id,QueryCode = OpenConstituentQuery.Code, IndexOrder = 10, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenConstituentQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenConstituentQuery.Id,QueryCode = OpenConstituentQuery.Code, IndexOrder = 11, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter OpenConstituentQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "OpenConstituentInvoices" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "OpenConstituentInvoices" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "true",PredefinedValue2 = null, QueryId = OpenConstituentQuery.Id,QueryCode = OpenConstituentQuery.Code, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query InvoicesFailedtoOpeninSATQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = ARInvoiceTextCode_10.Id, NameTextCodeCode = ARInvoiceTextCode_10.Code, ObjectTableName = "ARInvoice", Code = "Invoices Failed to Open in SAT",  QueryGroupCode = "INVC", IndexOrder = 10, Tenant = 0, ObjectTableId = ARInvoiceObjectTable.Id, QuerySection = "ARInvoice", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = ARInvoiceFeature_10.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn InvoicesFailedtoOpeninSATQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InvoicesFailedtoOpeninSATQuery.Id,QueryCode = InvoicesFailedtoOpeninSATQuery.Code, IndexOrder = 0, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "IsPrinted" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "IsPrinted" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 30 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InvoicesFailedtoOpeninSATQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InvoicesFailedtoOpeninSATQuery.Id,QueryCode = InvoicesFailedtoOpeninSATQuery.Code, IndexOrder = 1, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "Sent" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "Sent" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 30 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InvoicesFailedtoOpeninSATQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InvoicesFailedtoOpeninSATQuery.Id,QueryCode = InvoicesFailedtoOpeninSATQuery.Code, IndexOrder = 2, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InvoicesFailedtoOpeninSATQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InvoicesFailedtoOpeninSATQuery.Id,QueryCode = InvoicesFailedtoOpeninSATQuery.Code, IndexOrder = 3, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "ARInvoiceTypeName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "ARInvoiceTypeName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InvoicesFailedtoOpeninSATQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InvoicesFailedtoOpeninSATQuery.Id,QueryCode = InvoicesFailedtoOpeninSATQuery.Code, IndexOrder = 4, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "BillToName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "BillToName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InvoicesFailedtoOpeninSATQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InvoicesFailedtoOpeninSATQuery.Id,QueryCode = InvoicesFailedtoOpeninSATQuery.Code, IndexOrder = 5, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InvoicesFailedtoOpeninSATQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InvoicesFailedtoOpeninSATQuery.Id,QueryCode = InvoicesFailedtoOpeninSATQuery.Code, IndexOrder = 6, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InvoicesFailedtoOpeninSATQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InvoicesFailedtoOpeninSATQuery.Id,QueryCode = InvoicesFailedtoOpeninSATQuery.Code, IndexOrder = 7, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InvoicesFailedtoOpeninSATQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InvoicesFailedtoOpeninSATQuery.Id,QueryCode = InvoicesFailedtoOpeninSATQuery.Code, IndexOrder = 8, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InvoicesFailedtoOpeninSATQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InvoicesFailedtoOpeninSATQuery.Id,QueryCode = InvoicesFailedtoOpeninSATQuery.Code, IndexOrder = 9, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InvoicesFailedtoOpeninSATQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InvoicesFailedtoOpeninSATQuery.Id,QueryCode = InvoicesFailedtoOpeninSATQuery.Code, IndexOrder = 10, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InvoicesFailedtoOpeninSATQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InvoicesFailedtoOpeninSATQuery.Id,QueryCode = InvoicesFailedtoOpeninSATQuery.Code, IndexOrder = 11, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter InvoicesFailedtoOpeninSATQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "SATTransferStatusCode" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "SATTransferStatusCode" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "TE",PredefinedValue2 = null, QueryId = InvoicesFailedtoOpeninSATQuery.Id,QueryCode = InvoicesFailedtoOpeninSATQuery.Code, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query SATFailedInvoicesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = ARInvoiceTextCode_11.Id, NameTextCodeCode = ARInvoiceTextCode_11.Code, ObjectTableName = "ARInvoice", Code = "SAT Failed Invoices",  QueryGroupCode = "INVC", IndexOrder = 11, Tenant = 0, ObjectTableId = ARInvoiceObjectTable.Id, QuerySection = "ARInvoice", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = ARInvoiceFeature_11.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn SATFailedInvoicesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SATFailedInvoicesQuery.Id,QueryCode = SATFailedInvoicesQuery.Code, IndexOrder = 0, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "IsPrinted" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "IsPrinted" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 30 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn SATFailedInvoicesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SATFailedInvoicesQuery.Id,QueryCode = SATFailedInvoicesQuery.Code, IndexOrder = 1, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "Sent" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "Sent" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 30 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn SATFailedInvoicesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SATFailedInvoicesQuery.Id,QueryCode = SATFailedInvoicesQuery.Code, IndexOrder = 2, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn SATFailedInvoicesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SATFailedInvoicesQuery.Id,QueryCode = SATFailedInvoicesQuery.Code, IndexOrder = 3, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "ARInvoiceTypeName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "ARInvoiceTypeName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn SATFailedInvoicesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SATFailedInvoicesQuery.Id,QueryCode = SATFailedInvoicesQuery.Code, IndexOrder = 4, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "BillToName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "BillToName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn SATFailedInvoicesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SATFailedInvoicesQuery.Id,QueryCode = SATFailedInvoicesQuery.Code, IndexOrder = 5, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn SATFailedInvoicesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SATFailedInvoicesQuery.Id,QueryCode = SATFailedInvoicesQuery.Code, IndexOrder = 6, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn SATFailedInvoicesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SATFailedInvoicesQuery.Id,QueryCode = SATFailedInvoicesQuery.Code, IndexOrder = 7, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn SATFailedInvoicesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SATFailedInvoicesQuery.Id,QueryCode = SATFailedInvoicesQuery.Code, IndexOrder = 8, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn SATFailedInvoicesQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SATFailedInvoicesQuery.Id,QueryCode = SATFailedInvoicesQuery.Code, IndexOrder = 9, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn SATFailedInvoicesQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SATFailedInvoicesQuery.Id,QueryCode = SATFailedInvoicesQuery.Code, IndexOrder = 10, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn SATFailedInvoicesQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SATFailedInvoicesQuery.Id,QueryCode = SATFailedInvoicesQuery.Code, IndexOrder = 11, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter SATFailedInvoicesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "SATTransferStatusCode" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "SATTransferStatusCode" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "TE",PredefinedValue2 = null, QueryId = SATFailedInvoicesQuery.Id,QueryCode = SATFailedInvoicesQuery.Code, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query OpenInvoicesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = ARInvoiceTextCode_12.Id, NameTextCodeCode = ARInvoiceTextCode_12.Code, ObjectTableName = "ARInvoice", Code = "OpenInvoices",  QueryGroupCode = "INVC", IndexOrder = 12, Tenant = 0, ObjectTableId = ARInvoiceObjectTable.Id, QuerySection = "ARInvoice", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = ARInvoiceFeature_12.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Desending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn OpenInvoicesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenInvoicesQuery.Id,QueryCode = OpenInvoicesQuery.Code, IndexOrder = 0, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "IsPrinted" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "IsPrinted" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 30 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenInvoicesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenInvoicesQuery.Id,QueryCode = OpenInvoicesQuery.Code, IndexOrder = 1, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "Sent" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "Sent" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 30 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenInvoicesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenInvoicesQuery.Id,QueryCode = OpenInvoicesQuery.Code, IndexOrder = 2, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenInvoicesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenInvoicesQuery.Id,QueryCode = OpenInvoicesQuery.Code, IndexOrder = 3, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "ARInvoiceTypeName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "ARInvoiceTypeName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenInvoicesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenInvoicesQuery.Id,QueryCode = OpenInvoicesQuery.Code, IndexOrder = 4, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "BillToName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "BillToName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenInvoicesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenInvoicesQuery.Id,QueryCode = OpenInvoicesQuery.Code, IndexOrder = 5, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenInvoicesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenInvoicesQuery.Id,QueryCode = OpenInvoicesQuery.Code, IndexOrder = 6, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenInvoicesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenInvoicesQuery.Id,QueryCode = OpenInvoicesQuery.Code, IndexOrder = 7, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenInvoicesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenInvoicesQuery.Id,QueryCode = OpenInvoicesQuery.Code, IndexOrder = 8, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenInvoicesQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenInvoicesQuery.Id,QueryCode = OpenInvoicesQuery.Code, IndexOrder = 9, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenInvoicesQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenInvoicesQuery.Id,QueryCode = OpenInvoicesQuery.Code, IndexOrder = 10, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "ExpectedPaymentDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "ExpectedPaymentDate" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenInvoicesQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenInvoicesQuery.Id,QueryCode = OpenInvoicesQuery.Code, IndexOrder = 11, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "InternalNotes" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "InternalNotes" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter OpenInvoicesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "OpenInvoices" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "OpenInvoices" && d.ObjectTableId == ARInvoiceObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "true",PredefinedValue2 = null, QueryId = OpenInvoicesQuery.Id,QueryCode = OpenInvoicesQuery.Code, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);

	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable ARInvoiceObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "ARInvoice" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> ARInvoiceObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "ARInvoice").ToList();
		       
	      

	         Screen ARInvoiceHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ARInvoice.FullAccHeaderScreen", Name = "Header Screen", ObjectTableId = ARInvoiceObjectTable.Id, NumberOfColumns = 5, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField ARInvoiceARInvoiceFullAccHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "MainEntityReference").FirstOrDefault().Id, ScreenId = ARInvoiceHeaderScreenScreen0.Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "MainEntityReference").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ARInvoiceARInvoiceFullAccHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "DueDate").FirstOrDefault().Id, ScreenId = ARInvoiceHeaderScreenScreen0.Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "DueDate").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ARInvoiceARInvoiceFullAccHeaderScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountDue").FirstOrDefault().Id, ScreenId = ARInvoiceHeaderScreenScreen0.Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountDue").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ARInvoiceARInvoiceFullAccHeaderScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "StatusName").FirstOrDefault().Id, ScreenId = ARInvoiceHeaderScreenScreen0.Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "StatusName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ARInvoiceARInvoiceFullAccHeaderScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 4, Row = 0, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "JournalNumber").FirstOrDefault().Id, ScreenId = ARInvoiceHeaderScreenScreen0.Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "JournalNumber").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    ARInvoiceObjectTable.HeaderScreenId = ARInvoiceHeaderScreenScreen0.Id;
	   		  
	      

	         Screen ARInvoiceHeaderScreenScreen1 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ARInvoice.HeaderScreen", Name = "Header Screen", ObjectTableId = ARInvoiceObjectTable.Id, NumberOfColumns = 5, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField ARInvoiceARInvoiceHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "MainEntityReference").FirstOrDefault().Id, ScreenId = ARInvoiceHeaderScreenScreen1.Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "MainEntityReference").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ARInvoiceARInvoiceHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "DueDate").FirstOrDefault().Id, ScreenId = ARInvoiceHeaderScreenScreen1.Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "DueDate").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ARInvoiceARInvoiceHeaderScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountDue").FirstOrDefault().Id, ScreenId = ARInvoiceHeaderScreenScreen1.Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "AmountDue").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ARInvoiceARInvoiceHeaderScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "StatusName").FirstOrDefault().Id, ScreenId = ARInvoiceHeaderScreenScreen1.Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "StatusName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ARInvoiceARInvoiceHeaderScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 4, Row = 0, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "TransferStatusName").FirstOrDefault().Id, ScreenId = ARInvoiceHeaderScreenScreen1.Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "TransferStatusName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    ARInvoiceObjectTable.HeaderScreenId = ARInvoiceHeaderScreenScreen1.Id;
	   		  
	      

	         Screen ARInvoiceGeneralTabScreenScreen2 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ARInvoice.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = ARInvoiceObjectTable.Id, NumberOfColumns = 1, NumberOfRows = 10, IsReadOnly = false }, screensRepository, tenantScreens);
      
            ScreenField ARInvoiceARInvoiceGeneralTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "UpdatedByUserId").FirstOrDefault().Id, ScreenId = ARInvoiceGeneralTabScreenScreen2.Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "UpdatedByUserId").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ARInvoiceARInvoiceGeneralTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "UpdateDate").FirstOrDefault().Id, ScreenId = ARInvoiceGeneralTabScreenScreen2.Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "UpdateDate").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ARInvoiceARInvoiceGeneralTabScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "Sent").FirstOrDefault().Id, ScreenId = ARInvoiceGeneralTabScreenScreen2.Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "Sent").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ARInvoiceARInvoiceGeneralTabScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 3, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "HouseNumber").FirstOrDefault().Id, ScreenId = ARInvoiceGeneralTabScreenScreen2.Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "HouseNumber").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ARInvoiceARInvoiceGeneralTabScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 4, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "MasterNumber").FirstOrDefault().Id, ScreenId = ARInvoiceGeneralTabScreenScreen2.Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "MasterNumber").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ARInvoiceARInvoiceGeneralTabScreenScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 5, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "BranchId").FirstOrDefault().Id, ScreenId = ARInvoiceGeneralTabScreenScreen2.Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "BranchId").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ARInvoiceARInvoiceGeneralTabScreenScreenField6 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 6, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "CustomerRef").FirstOrDefault().Id, ScreenId = ARInvoiceGeneralTabScreenScreen2.Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "CustomerRef").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ARInvoiceARInvoiceGeneralTabScreenScreenField7 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 7, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "SalesmanUserId").FirstOrDefault().Id, ScreenId = ARInvoiceGeneralTabScreenScreen2.Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "SalesmanUserId").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ARInvoiceARInvoiceGeneralTabScreenScreenField8 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 8, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "BankAccountLiteId").FirstOrDefault().Id, ScreenId = ARInvoiceGeneralTabScreenScreen2.Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "BankAccountLiteId").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ARInvoiceARInvoiceGeneralTabScreenScreenField9 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 9, ObjectFieldId = ARInvoiceObjectFields.Where(d => d.FieldName == "Intercompany").FirstOrDefault().Id, ScreenId = ARInvoiceGeneralTabScreenScreen2.Id, ObjectFieldCode = ARInvoiceObjectFields.Where(d => d.FieldName == "Intercompany").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
           

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable ARInvoiceObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "ARInvoice" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode ARInvoiceDetailsTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.TH.Details", DefaultText = "Details",LocalDefaultText = "פרטים", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature ARInvoiceDetailsFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DETAILS", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.Details", NameTextCodeDefaultText = "Details", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode ARInvoiceGeneralTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.TH.General", DefaultText = "General",LocalDefaultText = "כללי", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature ARInvoiceGeneralFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GENERAL", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode ARInvoiceDocsOutTextCode_TH2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.TH.DocsOut", DefaultText = "Docs Out",LocalDefaultText = "מסמכים שיצאו", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature ARInvoiceDocsOutFeature_TH2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DOCSOUT", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.DocsOut", NameTextCodeDefaultText = "Docs Out", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode ARInvoiceDocsInTextCode_TH3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.TH.DocsIn", DefaultText = "Docs In",LocalDefaultText = "מסמכים", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature ARInvoiceDocsInFeature_TH3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DOCSIN", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.DocsIn", NameTextCodeDefaultText = "Docs In", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode ARInvoiceTransferDetailsTextCode_TH4 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.TH.Accounting", DefaultText = "Transfer Details",LocalDefaultText = "נתוני העברה", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature GeneralTransferDetailsFeature_TH4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACCOUNTINGTRANSFER", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.AccountingTransfer", NameTextCodeDefaultText = "Accounting Transfer", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode ARInvoicePaymentsTextCode_TH5 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.TH.ARPayments", DefaultText = "Payments",LocalDefaultText = "תשלומים", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature ARInvoicePaymentsFeature_TH5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ARPAYMENTS", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.ARPayments", NameTextCodeDefaultText = "Payments", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode ARInvoiceCommunicationTextCode_TH6 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.TH.Communications", DefaultText = "Communication",LocalDefaultText = "תקשורת", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature ARInvoiceCommunicationFeature_TH6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "COMMUNICATION", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.Communication", NameTextCodeDefaultText = "Communication", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode ARInvoiceEventsTextCode_TH7 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.TH.Events", DefaultText = "Events",LocalDefaultText = "אירועים", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature ARInvoiceEventsFeature_TH7 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EVENTS", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "INIL",HtmlComponentName = "",HtmlComponentUrl = "./InvoiceModules/ARInvoice/Components/EditTabs/ARInvoiceDetailsTabComponent", FeatureId = ARInvoiceDetailsFeature_TH0.Id, ControlPath = "Simplog.InvoiceLib.Views.Tabs.ARInvoiceTabs.ARInvoiceDetailsTabControl", ObjectTableId = ARInvoiceObjectTable.Id, TabNameTextCodeId = ARInvoiceDetailsTextCode_TH0.Id, TabNameTextCodeCode = ARInvoiceDetailsTextCode_TH0.Code, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "INGC",HtmlComponentName = "",HtmlComponentUrl = "./InvoiceModules/ARInvoice/Components/EditTabs/ARInvoiceGeneralTabComponent", FeatureId = ARInvoiceGeneralFeature_TH1.Id, ControlPath = "Simplog.Infrastructure.GeneralControls.GeneralTabControl", ObjectTableId = ARInvoiceObjectTable.Id, TabNameTextCodeId = ARInvoiceGeneralTextCode_TH1.Id, TabNameTextCodeCode = ARInvoiceGeneralTextCode_TH1.Code, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "INDO",HtmlComponentName = "",HtmlComponentUrl = "./InvoiceModules/ARInvoice/Components/EditTabs/ARInvoiceDocsOutTabComponent", FeatureId = ARInvoiceDocsOutFeature_TH2.Id, ControlPath = "Simplog.InvoiceLib.Views.Tabs.ARInvoiceTabs.ARInvoiceDocsOutControl", ObjectTableId = ARInvoiceObjectTable.Id, TabNameTextCodeId = ARInvoiceDocsOutTextCode_TH2.Id, TabNameTextCodeCode = ARInvoiceDocsOutTextCode_TH2.Code, Tenant = 0, IndexOrder = 2 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "INDI",HtmlComponentName = "",HtmlComponentUrl = "./InvoiceModules/ARInvoice/Components/EditTabs/ARInvoiceDocsInTabComponent", FeatureId = ARInvoiceDocsInFeature_TH3.Id, ControlPath = "Simplog.InvoiceLib.Views.Tabs.ARInvoiceTabs.ARInvoiceDocsInControl", ObjectTableId = ARInvoiceObjectTable.Id, TabNameTextCodeId = ARInvoiceDocsInTextCode_TH3.Id, TabNameTextCodeCode = ARInvoiceDocsInTextCode_TH3.Code, Tenant = 0, IndexOrder = 3 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "INAC",HtmlComponentName = "",HtmlComponentUrl = "./InvoiceModules/ARInvoice/Components/EditTabs/ARInvoiceTransferTabComponent", FeatureId = GeneralTransferDetailsFeature_TH4.Id, ControlPath = "Simplog.InvoiceLib.Views.Tabs.ARInvoiceTabs.ARTransferTabControl", ObjectTableId = ARInvoiceObjectTable.Id, TabNameTextCodeId = ARInvoiceTransferDetailsTextCode_TH4.Id, TabNameTextCodeCode = ARInvoiceTransferDetailsTextCode_TH4.Code, Tenant = 0, IndexOrder = 4 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "INRC",HtmlComponentName = "",HtmlComponentUrl = "./InvoiceModules/ARInvoice/Components/EditTabs/ARInvoicePaymentsTabComponent", FeatureId = ARInvoicePaymentsFeature_TH5.Id, ControlPath = "Simplog.InvoiceLib.Views.Tabs.ARInvoiceTabs.ARPaymentsTabControl", ObjectTableId = ARInvoiceObjectTable.Id, TabNameTextCodeId = ARInvoicePaymentsTextCode_TH5.Id, TabNameTextCodeCode = ARInvoicePaymentsTextCode_TH5.Code, Tenant = 0, IndexOrder = 5 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "ARCM",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = ARInvoiceCommunicationFeature_TH6.Id, ControlPath = "Simplog.Infrastructure.Views.Communications.CommunicationsControl", ObjectTableId = ARInvoiceObjectTable.Id, TabNameTextCodeId = ARInvoiceCommunicationTextCode_TH6.Id, TabNameTextCodeCode = ARInvoiceCommunicationTextCode_TH6.Code, Tenant = 0, IndexOrder = 6 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "ARIE",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = ARInvoiceEventsFeature_TH7.Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = ARInvoiceObjectTable.Id, TabNameTextCodeId = ARInvoiceEventsTextCode_TH7.Id, TabNameTextCodeCode = ARInvoiceEventsTextCode_TH7.Code, Tenant = 0, IndexOrder = 7 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable ARInvoiceObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "ARInvoice" && d.Tenant == 0).FirstOrDefault(); 

		   Feature ARInvoiceFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = true, ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature ARInvoiceFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = true, ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature ARInvoiceFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = true, ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature ARInvoiceFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = true, ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.PackageFeature", NameTextCodeDefaultText = "ARInvoice Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes); 

		   		   //--------------> Additional Features <--------------\\

		   Feature ARInvoiceFeature_TRANSFER = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TRANSFER", FeatureTypeCode = "ACT", Packagable = false, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.Transfer", NameTextCodeDefaultText = @"Transfer" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature ARInvoiceFeature_Consolidation_Constituent = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Consolidation.Constituent", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.ConsolidationAndConstituent", NameTextCodeDefaultText = @"Consolidation & Constituent" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature ARInvoiceFeature_RecalculateExternals = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "RecalculateExternals", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.RecalculateExternals", NameTextCodeDefaultText = @"Recalculate External IDs" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature ARInvoiceFeature_Intercompany = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Intercompany", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.Intercompany", NameTextCodeDefaultText = @"Intercompany" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature ARInvoiceFeature_ARInvoiceEditExchangeRate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ARInvoiceEditExchangeRate", FeatureTypeCode = "ACT", Packagable = false, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.EditExchangeRate", NameTextCodeDefaultText = @"Edit Exchange Rate" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature ARInvoiceFeature_ManageStocks = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ManageStocks", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.ManageStocks", NameTextCodeDefaultText = @"Manage Stocks" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature ARInvoiceFeature_SSPV = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SSPV", FeatureTypeCode = "ACT", Packagable = false, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.SSPV", NameTextCodeDefaultText = @"Show in spotlight mode" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature ARInvoiceFeature_NEWCREDITNOTE = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEWCREDITNOTE", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.NEWCREDITNOTE", NameTextCodeDefaultText = @"New Credit Note" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

   
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable ARInvoiceObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "ARInvoice" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "ISLC",
                EnglishName =  "Salesman Changed",
                LocalName =  "Salesman Changed",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = ARInvoiceObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "INTS",
                EnglishName =  "Transferred to SAT",
                LocalName =  "Transferred to SAT",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = ARInvoiceObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "INAS",
                EnglishName =  "Approved by SAT",
                LocalName =  "Approved by SAT",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = ARInvoiceObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "PATS",
                EnglishName =  "Transferred to SAT",
                LocalName =  "Transferred to SAT",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = ARInvoiceObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "PAAS",
                EnglishName =  "Approved by SAT",
                LocalName =  "Approved by SAT",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = ARInvoiceObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "INVO",
                EnglishName =  "Voided",
                LocalName =  "Voided",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = ARInvoiceObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "COAR",
                EnglishName =  "Payment Connected",
                LocalName =  "Payment Connected",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = ARInvoiceObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "INSE",
                EnglishName =  "Invoice Sent",
                LocalName =  "Invoice Sent",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = ARInvoiceObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "INNS",
                EnglishName =  "Return Invoice to Not Sent",
                LocalName =  "Return Invoice to Not Sent",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = ARInvoiceObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "INPD",
                EnglishName =  "Invoice Paid",
                LocalName =  "Invoice Paid",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = ARInvoiceObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "INNP",
                EnglishName =  "Return Invoice to Not Paid",
                LocalName =  "Return Invoice to Not Paid",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = ARInvoiceObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "UPIN",
                EnglishName =  "Invoice Updated",
                LocalName =  "Invoice Updated",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = ARInvoiceObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CRIN",
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
                ObjectTableId = ARInvoiceObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "ARID",
                EnglishName =  "Payment Disconnected",
                LocalName =  "Payment Disconnected",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = ARInvoiceObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "INCN",
                EnglishName =  "Constituent Connected",
                LocalName =  "Constituent Connected",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = ARInvoiceObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "INDS",
                EnglishName =  "Constituent Disconnected",
                LocalName =  "Constituent Disconnected",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = ARInvoiceObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "INAP",
                EnglishName =  "Approved",
                LocalName =  "Approved",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = ARInvoiceObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "INCA",
                EnglishName =  "Canceled",
                LocalName =  "Canceled",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = ARInvoiceObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "REMF",
                EnglishName =  "Reminder",
                LocalName =  "Reminder",
                IsManualEntry =  true,
                ShortView =  true,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  true,
                IsFollowUp =  true,
                FollowUpEnglishName =  "Reminder",
                FollowUpLocalName =  "Reminder",
                ObjectTableId = ARInvoiceObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "NTFS",
                EnglishName =  "Number Taken From Stock ",
                LocalName =  "Number Taken From Stock ",
                IsManualEntry =  false,
                ShortView =  true,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = ARInvoiceObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "NRTS",
                EnglishName =  "Number Returned To Stock ",
                LocalName =  "Number Returned To Stock ",
                IsManualEntry =  false,
                ShortView =  true,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = ARInvoiceObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
		   FeatureRepository featureRepository = new FeatureRepository(0); 
		   //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList(); 
		   ObjectTable ARInvoiceObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "ARInvoice" && d.Tenant == 0).FirstOrDefault();       
    
			   Feature ARInvoiceFeature_MB0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SETASDRAFT", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.SetAsDraft", NameTextCodeDefaultText = "Set As Draft", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

      
    
			   Feature ARInvoiceFeature_MB1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SAVEANDAPPROVE", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.SaveAndApprove", NameTextCodeDefaultText = "Save And Approve", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

      
    
			   Feature ARInvoiceFeature_MB2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PRINT", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.Print", NameTextCodeDefaultText = "Print", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

      
    
			   Feature ARInvoiceFeature_MB3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CHECKSATSTATUS", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.CheckSATStatus", NameTextCodeDefaultText = "Check SAT Status", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

			   Feature ARInvoiceFeature_MB40 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CancelDraft", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.CancelDraft", NameTextCodeDefaultText = "Cancel Draft", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
             			   Feature ARInvoiceFeature_MB41 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AUTOCREDIT", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.AutoCredit", NameTextCodeDefaultText = "Auto Credit", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
             			   Feature ARInvoiceFeature_MB42 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SETASSENT", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.SetAsSent", NameTextCodeDefaultText = "Set As Sent", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
             			   Feature ARInvoiceFeature_MB43 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EnableReTransfer", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.EnableReTransfer", NameTextCodeDefaultText = "Enable accounting re-transfer", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
             			   Feature ARInvoiceFeature_MB44 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "VOID", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.Void", NameTextCodeDefaultText = "Void", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
             			   Feature ARInvoiceFeature_MB45 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SendToQBO", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARInvoice.Features.SendToQBO", NameTextCodeDefaultText = "Send to QBO", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
              

		   TextCodeRepository.SubmitChanges();
		   FeaturesRepository.SubmitChanges();
		   MenuButtonGroup ARInvoiceMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
				{
					MenuButtonGroupType = "InvoiceEdit",
					Name = "InvoiceEditButtonsGroup",
					ObjectTableId = ARInvoiceObjectTable.Id,
					Tenant = 0
				}, menuButtonGroupRepository, tenantMenuButtonGroups);        
   
			   MenuButton ARInvoiceMenuButton0 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "SaveAsDraft",
						Index = 10, 
						IsActive = true,
						LabelTextCodeCode = "ARInvoice.B.SaveAsDraft",
						LabelTextCodeDefaultText = "Save as Draft",
						Tenant = 0,
						MenuButtonGroupId = ARInvoiceMenuButtonGroup.Id,
						ObjectTableId = ARInvoiceObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = ARInvoiceFeature_MB0.Id,
						Style = null,
						LocalDefaultText = "שמור כטיוטה",
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton ARInvoiceMenuButton1 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "SaveAndApprove",
						Index = 11, 
						IsActive = true,
						LabelTextCodeCode = "ARInvoice.B.Approve",
						LabelTextCodeDefaultText = "Approve",
						Tenant = 0,
						MenuButtonGroupId = ARInvoiceMenuButtonGroup.Id,
						ObjectTableId = ARInvoiceObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = ARInvoiceFeature_MB1.Id,
						Style = "ApproveButtonStyle",
						LocalDefaultText = "אישור",
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton ARInvoiceMenuButton2 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "PrintInvoice",
						Index = 12, 
						IsActive = true,
						LabelTextCodeCode = "ARInvoice.B.Print",
						LabelTextCodeDefaultText = "Print",
						Tenant = 0,
						MenuButtonGroupId = ARInvoiceMenuButtonGroup.Id,
						ObjectTableId = ARInvoiceObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = ARInvoiceFeature_MB2.Id,
						Style = null,
						LocalDefaultText = "הדפסה",
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton ARInvoiceMenuButton3 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "CheckSATStatus",
						Index = 13, 
						IsActive = true,
						LabelTextCodeCode = "ARInvoice.B.CheckSATStatus",
						LabelTextCodeDefaultText = "Check SAT Status",
						Tenant = 0,
						MenuButtonGroupId = ARInvoiceMenuButtonGroup.Id,
						ObjectTableId = ARInvoiceObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = ARInvoiceFeature_MB3.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton ARInvoiceMenuButton4 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "Actions",
						Index = 100, 
						IsActive = false,
						LabelTextCodeCode = "ARInvoice.B.Actions",
						LabelTextCodeDefaultText = "More",
						Tenant = 0,
						MenuButtonGroupId = ARInvoiceMenuButtonGroup.Id,
						ObjectTableId = ARInvoiceObjectTable.Id,
						MenuButtonType = "dropdownbutton",
						FeatureId = null,
						Style = null,
						LocalDefaultText = "נוספים",
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);

			   MenuButton ARInvoiceMenuButton40 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "CancelDraft",
						Index = 2, 
						IsActive = true,
						LabelTextCodeCode = "ARInvoice.B.CancelDraft",
						LabelTextCodeDefaultText = "Cancel Draft",
						Tenant = 0,
						MenuButtonGroupId = ARInvoiceMenuButtonGroup.Id,
						ParentMenuButtonId = ARInvoiceMenuButton4.Id,
						ObjectTableId = ARInvoiceObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  ARInvoiceFeature_MB40.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton ARInvoiceMenuButton41 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "AutoCredit",
						Index = 3, 
						IsActive = true,
						LabelTextCodeCode = "ARInvoice.B.AutoCredit",
						LabelTextCodeDefaultText = "Auto Credit",
						Tenant = 0,
						MenuButtonGroupId = ARInvoiceMenuButtonGroup.Id,
						ParentMenuButtonId = ARInvoiceMenuButton4.Id,
						ObjectTableId = ARInvoiceObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  ARInvoiceFeature_MB41.Id,
						Style = null,
						LocalDefaultText = "ביטול",
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton ARInvoiceMenuButton42 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "InvoiceOperationsSeparator",
						Index = 5, 
						IsActive = false,
						LabelTextCodeCode = "ARInvoice.B.InvoiceOperationsSeparator",
						LabelTextCodeDefaultText = "",
						Tenant = 0,
						MenuButtonGroupId = ARInvoiceMenuButtonGroup.Id,
						ParentMenuButtonId = ARInvoiceMenuButton4.Id,
						ObjectTableId = ARInvoiceObjectTable.Id,
						MenuButtonType = "separator",
						
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton ARInvoiceMenuButton43 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "SetAsSent",
						Index = 6, 
						IsActive = true,
						LabelTextCodeCode = "ARInvoice.B.SetAsSent",
						LabelTextCodeDefaultText = "Set as Sent",
						Tenant = 0,
						MenuButtonGroupId = ARInvoiceMenuButtonGroup.Id,
						ParentMenuButtonId = ARInvoiceMenuButton4.Id,
						ObjectTableId = ARInvoiceObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  ARInvoiceFeature_MB42.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton ARInvoiceMenuButton44 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "ReTransfer",
						Index = 7, 
						IsActive = true,
						LabelTextCodeCode = "ARInvoice.B.ReTransfer",
						LabelTextCodeDefaultText = "Enable accounting re-transfer",
						Tenant = 0,
						MenuButtonGroupId = ARInvoiceMenuButtonGroup.Id,
						ParentMenuButtonId = ARInvoiceMenuButton4.Id,
						ObjectTableId = ARInvoiceObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  ARInvoiceFeature_MB43.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton ARInvoiceMenuButton45 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "VoidARInvoiceOperationsSeparator",
						Index = 8, 
						IsActive = false,
						LabelTextCodeCode = "ARInvoice.B.VoidARInvoiceOperationsSeparator",
						LabelTextCodeDefaultText = "",
						Tenant = 0,
						MenuButtonGroupId = ARInvoiceMenuButtonGroup.Id,
						ParentMenuButtonId = ARInvoiceMenuButton4.Id,
						ObjectTableId = ARInvoiceObjectTable.Id,
						MenuButtonType = "separator",
						
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton ARInvoiceMenuButton46 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "VoidARInvoice",
						Index = 9, 
						IsActive = true,
						LabelTextCodeCode = "ARInvoice.B.Void",
						LabelTextCodeDefaultText = "Void",
						Tenant = 0,
						MenuButtonGroupId = ARInvoiceMenuButtonGroup.Id,
						ParentMenuButtonId = ARInvoiceMenuButton4.Id,
						ObjectTableId = ARInvoiceObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  ARInvoiceFeature_MB44.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton ARInvoiceMenuButton47 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "SendToQBO",
						Index = 14, 
						IsActive = true,
						LabelTextCodeCode = "ARInvoice.B.SendToQBO",
						LabelTextCodeDefaultText = "Send to QBO",
						Tenant = 0,
						MenuButtonGroupId = ARInvoiceMenuButtonGroup.Id,
						ParentMenuButtonId = ARInvoiceMenuButton4.Id,
						ObjectTableId = ARInvoiceObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  ARInvoiceFeature_MB45.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable ARInvoiceObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "ARInvoice" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode ARInvoiceTextCode_ARInvoiceBClose = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.B.Close", DefaultText = "Close",LocalDefaultText = @"", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMInvoiceNumberAlreadyAdded = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.InvoiceNumberAlreadyAdded", DefaultText = "Invoice Number already been added to another Invoice",LocalDefaultText = @"קיימת חשבונית עם מספר זהה", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMCantIssueInvoiceWithFutureDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.CantIssueInvoiceWithFutureDate", DefaultText = "Cant issue Invoice with Future Invoice Date",LocalDefaultText = @"לא ניתן לאשר חשבונית עם תאריך עתידי", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMInvoiceLinesHaveDifferentExchangeRates = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.InvoiceLinesHaveDifferentExchangeRates", DefaultText = "(%CurrencyCode) Invoice Lines have different Exchange Rate values",LocalDefaultText = @"ישנם שערי חליפין שונים בשורות החשבונית", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSDetailsNoVat = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.Details.NoVat", DefaultText = "No VAT for this date",LocalDefaultText = @"אין מ''ע לתאריך זה", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSPaymentsInvoicePayments = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.Payments.InvoicePayments", DefaultText = "Invoice Payments",LocalDefaultText = @"תשלומי חשבונית", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSPaymentsAmountPaid = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.Payments.AmountPaid", DefaultText = "Amount Paid",LocalDefaultText = @"סכום ששולם", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSPaymentsConnected = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.Payments.Connected", DefaultText = "Connected",LocalDefaultText = @"מקושר", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSPaymentsNotConnected = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.Payments.NotConnected", DefaultText = "Not Connected",LocalDefaultText = @"לא מקושר", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSPaymentsTotals = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.Payments.Totals", DefaultText = "Totals",LocalDefaultText = @"סה''כ", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceBPaymentsNewPayment = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.B.Payments.NewPayment", DefaultText = "New Payment",LocalDefaultText = @"תשלום חדש", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMYouShouldHaveOneLineAtLeast = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.YouShouldHaveOneLineAtLeast", DefaultText = "You should have 1 Invoice line at least",LocalDefaultText = @"חובה ליצור לפחות שורת חשבונית אחת", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMManualInvoiceNumberNotAllowed = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.ManualInvoiceNumberNotAllowed", DefaultText = "Accounting Settings dont allowe manual invoice number",LocalDefaultText = @"ע''פ הגדרות מערכת הנה''ח לא ניתן להקליד מספר חשבונית באופן ידני", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMChronologicalDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.ChronologicalDate", DefaultText = "Invoice Date should be bigger or equals to the Last Chronological Date %Date",LocalDefaultText = @"תאריך החשבונית חייב להיות גדול או שווה לתאריך %Date, שהוא תאריך החשבונית האחרונה במערכת", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMCantAddPaymentForDraftInvoice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.CantAddPaymentForDraftInvoice", DefaultText = "Cant add payment for Draft invoice",LocalDefaultText = @"לא ניתן להוסיף תשלום עבור טיוטת חשבונית", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMAmountPaidBiggerThanInvoiceAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.AmountPaidBiggerThanInvoiceAmount", DefaultText = "Amount paid equals or bigger than invoice amount",LocalDefaultText = @"הסכום ששולם שווה או גדול לסכום החשבונית", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMAccountingSettingsDontAllowVoid = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.AccountingSettingsDontAllowVoid", DefaultText = "Accounting Settings doesn't allow void A/R Invoice",LocalDefaultText = @"הגדרות הנה''ח לא מאפשרות ביטול חשבוניות", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMDisconnectPayments = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.DisconnectPayments", DefaultText = "Please disconnect all payments",LocalDefaultText = @"נא לנתק את כל ההתאמות לתשלומים", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMConfirmVoid = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.ConfirmVoid", DefaultText = "Once you void or delete an invoice, the change is permanent. If you void or delete an invoice and want to restore it later, you'll have to create a new invoice.",LocalDefaultText = @"ברגע שמבטלים או מתעלמים מחשבונית, השינוי הוא בלתי הפיך. במידה וביטלת חשבונית או התעלמת ממנה וברצונך לשחזר אותה, עליך ליצור חשבונית חדשה", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMDeletingDraft = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.DeletingDraft", DefaultText = "Deleting draft invoice...",LocalDefaultText = @"מוחק טיוטת חשבונית...", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMDeletedSuccessfully = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.DeletedSuccessfully", DefaultText = "Invoice Deleted Successfully",LocalDefaultText = @"החשבונית נמחקה בהצלחה", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMConfirmAutoCredit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.ConfirmAutoCredit", DefaultText = "This action will create a credit note, it will credit the client with the full amount of the invoice and cancel it",LocalDefaultText = @"פעולה זו תיצור חשבונית זיכוי, תזכה את הלקוח בסכום החשבונית המלא ותסמן את החשבונית כמבטלת אותה", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMCreatingAutoCredit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.CreatingAutoCredit", DefaultText = "Creating Auto Credit Invoice ...",LocalDefaultText = @"יוצר חשבונית זיכוי חדשה....", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMNoTemplate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.NoTemplate", DefaultText = "A/R Invoice document has no template!",LocalDefaultText = @"לא הוגדרה תבנית הדפסה למסמך חשבונית", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMNoPositivePrice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.NoPositivePrice", DefaultText = "Can't set a positive unit price on credit note",LocalDefaultText = @"מחיר יחידה לא יכול להיות גדול מאפס בזיכוי", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMNoMinusPrice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.NoMinusPrice", DefaultText = "Unit price should be bigger than zero",LocalDefaultText = @"מחיר יחידה חייב להיות גדול מאפס", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSDetailsAll = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.Details.All", DefaultText = "All",LocalDefaultText = @"הכל", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSDetailsPrepaid = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.Details.Prepaid", DefaultText = "Prepaid",LocalDefaultText = @"תשלום מראש", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSDetailsCollect = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.Details.Collect", DefaultText = "Collect",LocalDefaultText = @"גוביינא", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSDetailsTotals = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.Details.Totals", DefaultText = "Totals",LocalDefaultText = @"סה''כ", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSDetailsSubtotal = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.Details.Subtotal", DefaultText = "Subtotal",LocalDefaultText = @"סיכום ביניים", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSDetailsVatType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.Details.VatType", DefaultText = "Vat Type",LocalDefaultText = @"סוג מע''מ", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSDetailsDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.Details.Details", DefaultText = "Details",LocalDefaultText = @"פרטים", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSDetailsRate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.Details.Rate", DefaultText = "Rate",LocalDefaultText = @"שער", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSDetailsCurrencyDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.Details.CurrencyDetails", DefaultText = "Currency Details",LocalDefaultText = @"נתוני מטבע", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSDetailsFilterBy = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.Details.FilterBy", DefaultText = "Filter By",LocalDefaultText = @"סנן לפי", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceBDetailsApplyToAll = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.B.Details.ApplyToAll", DefaultText = "Apply to all",LocalDefaultText = @"החל על כל", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceBDetailsExportToAccount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.B.Details.ExportToAccount", DefaultText = "Export to Accounting",LocalDefaultText = @"יצוא להנה''ח", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMConnectCredit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.ConnectCredit", DefaultText = "Connecting Credit invoice is only allowed from Payments screen",LocalDefaultText = @"ניתן לקשר חשבונית זיכוי ממסך התשלומים בלבד", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceBTransfer = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.B.Transfer", DefaultText = "Transfer",LocalDefaultText = null, ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMUpdateInvoiceDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.UpdateInvoiceDate", DefaultText = "Please note that the current invoice date is %Date, adjust to Today's date ?",LocalDefaultText = @"שים לב, תאריך החשבונית הוא DATE, האם להתאים לתאריך של היום?", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMNoGLAccount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.NoGLAccount", DefaultText = "The chosen charge type doesn't have GLAccount connected to it",LocalDefaultText = @"סעיף החיוב הנבחר לא מקושר לכרטיס הנה''ח", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceBOk = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.B.Ok", DefaultText = "OK",LocalDefaultText = @"אישור", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceBCancel = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.B.Cancel", DefaultText = "Cancel",LocalDefaultText = @"ביטול", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMVatTypePercentageEmpty = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.VatTypePercentageEmpty", DefaultText = "Some of invoice lines Vat Type Percentage is empty",LocalDefaultText = @"בחלק מהשורות לא הוגדר אחוז מע''מ", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceONewInvoice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.O.NewInvoice", DefaultText = "Create New Invoice",LocalDefaultText = @"יצירת חשבונית חדשה", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceONewCreditNote = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.O.NewCreditNote", DefaultText = "Create New Credit Note",LocalDefaultText = @"צור זיכוי חדש", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceBEdit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.B.Edit", DefaultText = "Edit",LocalDefaultText = @"עריכה", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMYouShouldHaveOneLineAtLease = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.YouShouldHaveOneLineAtLease", DefaultText = "You should have at least 1 invoice line.",LocalDefaultText = null, ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMYouShouldSetInvoiceNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.YouShouldSetInvoiceNumber", DefaultText = "You Should Set Invoice Number",LocalDefaultText = @"עליך לקבוע מספר חשבונית", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMBillToGLAccount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.BillToGLAccount", DefaultText = "The bill to does not have GLAccount",LocalDefaultText = @"ללקוח אין כרטיס הנה''ח", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMChargeTypeGLAccount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.ChargeTypeGLAccount", DefaultText = "The Receivable GLAccount of the Charge Type Duties is NULL",LocalDefaultText = @"סעיף חיוב AAAAA לא מקושר לכרטיס הנה''ח", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMClosedMonth = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.ClosedMonth", DefaultText = "Closed Month",LocalDefaultText = @"חודש סגור", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMInvoiceCurrencyGLAccount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.InvoiceCurrencyGLAccount", DefaultText = "The Invoice Currency does not match to the bill to GLAccount Currency",LocalDefaultText = @"מטבע החשבונית לא זהה למטבע כרטיס הנה''ח של הלקוח", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMMultiPercentageVATs = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.MultiPercentageVATs", DefaultText = "Your accounting settings doesn't enable Multi-percentage VATs",LocalDefaultText = @"", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMConstituentInvoiceCantBeConnected = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.ConstituentInvoiceCantBeConnected", DefaultText = "Constituent invoice can't be connected",LocalDefaultText = @"לא ניתן לקשר תעודת  אשראי", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMNoMatchingInvoices = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.NoMatchingInvoices", DefaultText = "No matching invoices",LocalDefaultText = @"אין חשבוניות מתאימות", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMCancelARInvoice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.CancelARInvoice", DefaultText = "Are you sure you want to cancel this invoice?",LocalDefaultText = @"האם אתה בטוח שברצונך לבטל את החשבונית?", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMAlreadyTransferredInvoicesMsg = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.AlreadyTransferredInvoicesMsg", DefaultText = "Already transferred invoices can't be voided, \nrather you can use the 'Auto Credit' option.",LocalDefaultText = @"", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMInvoiceManualNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.InvoiceManualNumber", DefaultText = "Invoice Manual Number",LocalDefaultText = @"", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMNewConsolidationInvoiceErrorMsg1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.NewConsolidationInvoiceErrorMsg1", DefaultText = "Bill to is not allowed for consolidation invoices",LocalDefaultText = @"לא ניתן לשנות את הלקוח בחשבונית מרכזת", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMNewConsolidationInvoiceErrorMsg2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.NewConsolidationInvoiceErrorMsg2", DefaultText = "The customer exceeded the credit limit available.",LocalDefaultText = @"הלקוח חרג ממסגרת האשראי הזמינה לו", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMNewConsolidationInvoiceErrorMsg3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.NewConsolidationInvoiceErrorMsg3", DefaultText = "Bill To exceeded its credit limit of",LocalDefaultText = @"הלקוח חרג ממסגרת האשראי של", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMNewConsolidationInvoiceErrorMsg4 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.NewConsolidationInvoiceErrorMsg4", DefaultText = "The current balance stands on",LocalDefaultText = @"היתרה הנוכחית עומדת על", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMNewConsolidationInvoiceErrorMsg5 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.NewConsolidationInvoiceErrorMsg5", DefaultText = "The remaining credit limit for this customer is",LocalDefaultText = @"יתרת האשראי שנותרה עבור לקוח זה היא", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMNewConsolidationInvoiceErrorMsg6 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.NewConsolidationInvoiceErrorMsg6", DefaultText = "Credit limit setting is blocking invoice for bill to",LocalDefaultText = @"הגדרת מסגרת האשראי חוסמת את הפקת החשבונית ללקוח זה", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSSetInvoiceAsSent = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.SetInvoiceAsSent", DefaultText = "Set Invoice as Sent",LocalDefaultText = @"סמן חשבונית כנשלחה", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSSendInvoiceNotes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.SendInvoiceNotes", DefaultText = "Send Invoice Notes",LocalDefaultText = @"שלח הערות חשבונית", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSAutoCreditingMsg1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.AutoCreditingMsg1", DefaultText = "Please disconnect payments before auto crediting",LocalDefaultText = @"לפני ביצוע סטורנו יש לבטל את ההתאמה ", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSAutoCreditingMsg2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.AutoCreditingMsg2", DefaultText = "Please choose the auto credit date",LocalDefaultText = @"אנא בחר את תאריך הזיכוי", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSAutoCreditingMsg3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.AutoCreditingMsg3", DefaultText = "Enter auto credit invoice manual number",LocalDefaultText = @"", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSAutoCreditingMsg4 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.AutoCreditingMsg4", DefaultText = "Only Unpaid invoice can be auto crediting",LocalDefaultText = @"לא ניתן לבטל חשבונית ששולמה", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSAutoCreditingMsg5 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.AutoCreditingMsg5", DefaultText = "This number is already exists",LocalDefaultText = @"מספר זה כבר קיים", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSNewConsolidation = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.NewConsolidation", DefaultText = "New Consolidation",LocalDefaultText = @"מרכזת חדשה", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSCreditLimit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.CreditLimit", DefaultText = "Credit limit",LocalDefaultText = @"", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMNewGeneralInvoiceErrorMsg1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.NewGeneralInvoiceErrorMsg1", DefaultText = "The chosen card doesn’t have GLAccount connected to it",LocalDefaultText = @"לכרטיס הנבחר לא קושר כרטיס הנהלת חשבונות", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMNewGeneralInvoiceErrorMsg2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.NewGeneralInvoiceErrorMsg2", DefaultText = "The invoice currency does not match to the bill to GLAccount",LocalDefaultText = @"מטבע החשבונית אינו תואם את מטבע הכרטיס לחיוב", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMNewGeneralInvoiceErrorMsg3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.NewGeneralInvoiceErrorMsg3", DefaultText = "The chosen card doesn’t have GLAccount connected to it",LocalDefaultText = @"לכרטיס הנבחר לא קושר כרטיס הנהלת חשבונות", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMDueDateLowerThanInvoiceDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.DueDateLowerThanInvoiceDate", DefaultText = "The Due date shouldn't be lower than the Accounting date",LocalDefaultText = @"לא ניתן להזין תאריך פירעון נמוך מהתאריך החשבונאי", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSShortTitle = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.ShortTitle", DefaultText = "A/R Invoice",LocalDefaultText = @"חשבונית לקוח", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSShortTitleCustomsInvoice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.ShortTitle.CustomsInvoice", DefaultText = "Customs Invoice",LocalDefaultText = @"חשבונית מכס", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSShortTitleCustomsCreditNote = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.ShortTitle.CustomsCreditNote", DefaultText = "Customs Credit Note",LocalDefaultText = @"הערה על אשראי מכס", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSShortTitleConsolidation = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.ShortTitle.Consolidation", DefaultText = "Consolidation",LocalDefaultText = @"חשבונית מרכזת", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSShortTitleAutoCredited = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.ShortTitle.AutoCredited", DefaultText = "Auto Credited",LocalDefaultText = @"בוטלה", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSShortTitleCancelled = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.ShortTitle.Cancelled", DefaultText = "Cancelled",LocalDefaultText = @"מבוטלת", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSShortTitleByInvoice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.ShortTitle.ByInvoice", DefaultText = "By Invoice",LocalDefaultText = @"של חשבונית", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSShortTitleConstituent = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.ShortTitle.Constituent", DefaultText = "Constituent",LocalDefaultText = @"תעודת אשראי", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSShortTitleConnectedToConsolidation = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.ShortTitle.ConnectedToConsolidation", DefaultText = "Connected to consolidation",LocalDefaultText = @"מקושר לחשבונית מרכזת", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSShortTitleCreditNote = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.ShortTitle.CreditNote", DefaultText = "Credit Note",LocalDefaultText = @"זיכוי", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSShortTitleAutoCredit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.ShortTitle.AutoCredit", DefaultText = "Auto Credit",LocalDefaultText = @"ביטול", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceMMultiCurrencyMustInLocalCurrency = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.M.MultiCurrencyMustInLocalCurrency", DefaultText = "Multi currency ARInvoice should be only in Local currency",LocalDefaultText = @"חשבונית רב מטבעית חייבת להיות במטבע מקומי", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceSShortTitleByInvoiceAutoCredited = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.S.ShortTitle.ByInvoiceAutoCredited", DefaultText = "By Invoice",LocalDefaultText = " ע''י חשבונית", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceOCanNotCreditExempt = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.O.CanNotCreditExempt", DefaultText = "Can not credit card exempt VAT if the amount is not exempt",LocalDefaultText = @"לא ניתן לזכות כרטיס פטור בסכום חייב במע''מ", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceOCanNotCreditCardIsNotExempt = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.O.CanNotCreditCardIsNotExempt", DefaultText = "Can not credit card that is not exempt VAT if the amount is exempt.",LocalDefaultText = @"לא ניתן לזכות כרטיס חייב בסכום פטור", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARInvoiceTextCode_ARInvoiceOMissingDocument = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARInvoice.O.MissingDocument", DefaultText = "Invoice Document is missing",LocalDefaultText = @"המסמך שקשור לחשבונית לא נמצא", ObjectTableId = ARInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 