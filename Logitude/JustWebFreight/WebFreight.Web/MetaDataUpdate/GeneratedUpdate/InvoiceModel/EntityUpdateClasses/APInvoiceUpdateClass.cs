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
   public class APInvoiceUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "APInvoice",
			      				    IsNew =  false,
			      				    DBTableName =  "APInvoices",
			      				    OldDBTableName =  "APInvoices",
			      				    ObjectTableSingular =  "A/P Invoice",
			      				    ObjectTablePlural =  "A/P Invoices",
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
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  false,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "BR",
			      				    MaxNumberOfCustomFields =  0,
			      				    DefaultText =  "A/P Invoice",
			      				    Code =  "APIN",
			      				    Name =  "AP Invoices",
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
			      				    PrimaryKeyIsTenant =  false,
			      				    SearchFields =  "APInvoice,APInvoices,,Id,",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsGeneralInvoice",
					  						OldFieldName =  "IsGeneralInvoice",
					  						ObjectTableName =  "APInvoice",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					  						ListFieldLable =  "IsGeneralInvoiceListLable",
					  						ListLableDefaultText =  "IsGeneralInvoice",
					  						ListLocalDefaultText =  "IsGeneralInvoice",
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
					 
					 						FieldName =  "InternalNumber",
					  						OldFieldName =  "InternalNumber",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  20,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  true,
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
					  						PMPropertyPath =  "InternalNumber",
					  						ListPropertyPath =  "InternalNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "InternalNumber",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "InternalNumber",
					  						DefaultText =  "Internal Number",
					  						FullLocalDefaultText =  "מספר פנימי",
					  						ListFieldLable =  "InternalNumberListLable",
					  						ListLableDefaultText =  "Internal #",
					  						ListLocalDefaultText =  "מספר פנימי",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "InternalNumber",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InvoiceNumber",
					  						OldFieldName =  "InvoiceNumber",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  25,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  true,
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					  						FullLocalDefaultText =  "מספר חשבונית",
					  						ListFieldLable =  "InvoiceNumberListLable",
					  						ListLableDefaultText =  "Invoice No.",
					  						ListLocalDefaultText =  "מספר חשבונית",
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
					 
					 						FieldName =  "VendorId",
					  						OldFieldName =  "VendorId",
					  						ObjectTableName =  "APInvoice",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "VendorId",
					  						ListPropertyPath =  "VendorId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "VendorId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "VendorId",
					  						DefaultText =  "Vendor",
					  						FullLocalDefaultText =  "ספק",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "VendorId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "VendorName",
					  						OldFieldName =  "VendorName",
					  						ObjectTableName =  "APInvoice",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "VendorName",
					  						ListPropertyPath =  "VendorName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "VendorName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "VendorName",
					  						DefaultText =  "Vendor",
					  						FullLocalDefaultText =  "ספק",
					  						ListFieldLable =  "VendorNameListLable",
					  						ListLableDefaultText =  "Vendor",
					  						ListLocalDefaultText =  "ספק",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "VendorName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "VATNumber",
					  						OldFieldName =  "VATNumber",
					  						ObjectTableName =  "APInvoice",
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
					  						PMPropertyPath =  "VATNumber",
					  						ListPropertyPath =  "VATNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "VATNumber",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "VATNumber",
					  						DefaultText =  "VAT Number",
					  						FullLocalDefaultText =  "מספר ח.פ",
					  						ListFieldLable =  "VATNumberListLable",
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
					  						HelpTextCode =  "VATNumber",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InvoiceDate",
					  						OldFieldName =  "InvoiceDate",
					  						ObjectTableName =  "APInvoice",
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
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					  						FullLocalDefaultText =  ",תאריך אסמכתא",
					  						ListFieldLable =  "InvoiceDateListLable",
					  						ListLableDefaultText =  "Invoice Date",
					  						ListLocalDefaultText =  ",תאריך אסמכתא",
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
					 
					 						FieldName =  "PaymentTermId",
					  						OldFieldName =  "PaymentTermId",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "PaymentTerm",
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
					  						Operator =  "StartsWith",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					  						DefaultText =  "Payment Terms",
					  						FullLocalDefaultText =  "תנאי תשלום",
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
					 
					 						FieldName =  "PaymentTermName",
					  						OldFieldName =  "PaymentTermName",
					  						ObjectTableName =  "APInvoice",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					  						DefaultText =  "Payment Terms",
					  						FullLocalDefaultText =  "תנאי תשלום",
					  						ListFieldLable =  "PaymentTermNameListLable",
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
					  						HelpTextCode =  "PaymentTermName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DueDate",
					  						OldFieldName =  "DueDate",
					  						ObjectTableName =  "APInvoice",
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
					  						DataTemplateName =  "APInvoiceDueDateTimeDataTemplate",
					  						IsCustomFilter =  false,
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					 
					 						FieldName =  "InvoiceCurrencyExchangeRate",
					  						OldFieldName =  "InvoiceCurrencyExchangeRate",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Double",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  true,
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
					  						PMPropertyPath =  "InvoiceCurrencyExchangeRate",
					  						ListPropertyPath =  "InvoiceCurrencyExchangeRate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APInvoice",
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
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "InvoiceCurrencyExchangeRate",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ExchangeRateDate",
					  						OldFieldName =  "ExchangeRateDate",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  true,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					  						FullLocalDefaultText =  "תאריך שער חליפין",
					  						ListFieldLable =  "ExchangeRateDateListLable",
					  						ListLableDefaultText =  "Exchange Date",
					  						ListLocalDefaultText =  "תאריך שער חליפין",
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
					 
					 						FieldName =  "InvoiceCurrencyId",
					  						OldFieldName =  "InvoiceCurrencyId",
					  						ObjectTableName =  "APInvoice",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					  						FullLocalDefaultText =  "מטבע",
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
					 
					 						FieldName =  "InvoiceCurrencyCode",
					  						OldFieldName =  "InvoiceCurrencyCode",
					  						ObjectTableName =  "APInvoice",
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
					  						Operator =  "Equals",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					  						HelpTextCode =  "InvoiceCurrencyCode",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LocalCurrencyId",
					  						OldFieldName =  "LocalCurrencyId",
					  						ObjectTableName =  "APInvoice",
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "LocalCurrencyId",
					  						ListPropertyPath =  "LocalCurrencyId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APInvoice",
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
					  						HelpTextCode =  "LocalCurrencyId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InternalNotes",
					  						OldFieldName =  "InternalNotes",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  250,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  true,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  true,
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					  						DefaultText =  "Notes",
					  						FullLocalDefaultText =  "הערות",
					  						ListFieldLable =  "InternalNotesListLable",
					  						ListLableDefaultText =  "Notes",
					  						ListLocalDefaultText =  "הערות",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "InternalNotes",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SubTotalInLocalCurrency",
					  						OldFieldName =  "SubTotalInLocalCurrency",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Double",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  true,
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					  						ListLableDefaultText =  "Sub Total (Local Currency)",
					  						ListLocalDefaultText =  "סיכום ביניים בש'ח",
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
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Double",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  true,
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					  						FullLocalDefaultText =  "סיכום ביניים בש'ח",
					  						ListFieldLable =  "SubTotalInInvoiceCurrencyListLable",
					  						ListLableDefaultText =  "Sub Total",
					  						ListLocalDefaultText =  "סיכום ביניים בש'ח",
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
					 
					 						FieldName =  "AmountInInvoiceCurrency",
					  						OldFieldName =  "AmountInInvoiceCurrency",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "SigDouble",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  true,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						DataTemplateName =  "APAmountInInvoiceCurrencyDataTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					 
					 						FieldName =  "AmountInLocalCurrency",
					  						OldFieldName =  "AmountInLocalCurrency",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Double",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  true,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						DataTemplateName =  "APAmountInLocalCurrencyDataTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					 
					 						FieldName =  "StatusCode",
					  						OldFieldName =  "StatusCode",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "APInvoiceStatus",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					  						DefaultText =  "Status",
					  						FullLocalDefaultText =  "סטטוס",
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
					 
					 						FieldName =  "StatusName",
					  						OldFieldName =  "StatusName",
					  						ObjectTableName =  "APInvoice",
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
					  						Operator =  "Equals",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					 
					 						FieldName =  "CreateDate",
					  						OldFieldName =  "CreateDate",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  true,
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					  						FullLocalDefaultText =  "תאריך יצירה",
					  						ListFieldLable =  "CreateDateListLable",
					  						ListLableDefaultText =  "Create Date",
					  						ListLocalDefaultText =  "תאריך יצירה",
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
					 
					 						FieldName =  "CreatedByUserId",
					  						OldFieldName =  "CreatedByUserId",
					  						ObjectTableName =  "APInvoice",
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
					  						Operator =  "StartsWith",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					  						DefaultText =  "Created By",
					  						FullLocalDefaultText =  "יוצר",
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
					  						ObjectTableName =  "APInvoice",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					  						DefaultText =  "Created By",
					  						FullLocalDefaultText =  "יוצר",
					  						ListFieldLable =  "CreatedByUserNameListLable",
					  						ListLableDefaultText =  "Created By",
					  						ListLocalDefaultText =  "יוצר",
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
					 
					 						FieldName =  "IsClosed",
					  						OldFieldName =  "IsClosed",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Boolean",
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
					  						DataTemplateName =  "APInvoiceIsClosedPathTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsClosed",
					  						ListPropertyPath =  "IsClosed",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "IsClosed",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsClosed",
					  						DefaultText =  "Is Closed",
					  						FullLocalDefaultText =  "סגור",
					  						ListFieldLable =  "IsClosedListLable",
					  						ListLableDefaultText =  "Is Closed",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "IsClosed",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ProfitCurrencyId",
					  						OldFieldName =  "ProfitCurrencyId",
					  						ObjectTableName =  "APInvoice",
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ProfitCurrencyId",
					  						ListPropertyPath =  "ProfitCurrencyId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ProfitCurrencyId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ProfitCurrencyId",
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
					  						HelpTextCode =  "ProfitCurrencyId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ProfitCurrencyCode",
					  						OldFieldName =  "ProfitCurrencyCode",
					  						ObjectTableName =  "APInvoice",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					  						ListFieldLable =  "ProfitCurrencyCodeListLable",
					  						ListLableDefaultText =  "Profit Currency",
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
					 
					 						FieldName =  "ProfitCurrencyExchangeRate",
					  						OldFieldName =  "ProfitCurrencyExchangeRate",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Double",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  true,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						DataTemplateName =  "ProfitCurrencyExchangeRateDataTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ProfitCurrencyExchangeRate",
					  						ListPropertyPath =  "ProfitCurrencyExchangeRate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ProfitCurrencyExchangeRate",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ProfitCurrencyExchangeRate",
					  						DefaultText =  "Profit Currency Exchange Rate",
					  						FullLocalDefaultText =  "שער חליפין לרווח",
					  						ListFieldLable =  "ProfitCurrencyExchangeRateListLable",
					  						ListLableDefaultText =  "Profit Currency Exchange Rate",
					  						ListLocalDefaultText =  "שער חליפין לרווח",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "ProfitCurrencyExchangeRate",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AmountInProfitCurrency",
					  						OldFieldName =  "AmountInProfitCurrency",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Double",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  true,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						DataTemplateName =  "AmountInProfitCurrencyDataTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AmountInProfitCurrency",
					  						ListPropertyPath =  "AmountInProfitCurrency",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "AmountInProfitCurrency",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AmountInProfitCurrency",
					  						DefaultText =  "Amount (Profit Currency)",
					  						FullLocalDefaultText =  "סכום (מטבע רווח)",
					  						ListFieldLable =  "AmountInProfitCurrencyListLable",
					  						ListLableDefaultText =  "Amount (Profit Currency)",
					  						ListLocalDefaultText =  "סכום (מטבע רווח)",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						HelpTextCode =  "AmountInProfitCurrency",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdatedByUserId",
					  						OldFieldName =  "UpdatedByUserId",
					  						ObjectTableName =  "APInvoice",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					 
					 						FieldName =  "UpdatedByUserName",
					  						OldFieldName =  "UpdatedByUserName",
					  						ObjectTableName =  "APInvoice",
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
					  						ValidForQuerySection1 =  "APInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "UpdatedByUserName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UpdatedByUserName",
					  						DefaultText =  "Updated By User",
					  						FullLocalDefaultText =  "עודכן ע''י משתמש",
					  						ListFieldLable =  "UpdatedByUserNameListLable",
					  						ListLableDefaultText =  "Updated By User",
					  						ListLocalDefaultText =  "עודכן ע''י משתמש",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "UpdatedByUserName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdateDate",
					  						OldFieldName =  "UpdateDate",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  true,
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					 
					 						FieldName =  "MainEntityId",
					  						OldFieldName =  "MainEntityId",
					  						ObjectTableName =  "APInvoice",
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
					  						PMPropertyPath =  "MainEntityId",
					  						ListPropertyPath =  "MainEntityId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "MainEntityId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "MainEntityId",
					  						DefaultText =  "Main Entity",
					  						FullLocalDefaultText =  "ישות ראשית",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "MainEntityId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "MainEntityReference",
					  						OldFieldName =  "MainEntityReference",
					  						ObjectTableName =  "APInvoice",
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
					  						Operator =  "Equals",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					  						DefaultText =  "Shipment No",
					  						FullLocalDefaultText =  "אסמכתא ישות ראשית ",
					  						ListFieldLable =  "MainEntityReferenceListLable",
					  						ListLableDefaultText =  "Shipment No",
					  						ListLocalDefaultText =  "אסמכתא ישות ראשית ",
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
					 
					 						FieldName =  "SearchFields",
					  						OldFieldName =  "SearchFields",
					  						ObjectTableName =  "APInvoice",
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
					  						ValidForQuerySection1 =  "APInvoice",
					  						ValidForQuerySection2 =  "APInvoiceFollowUp",
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
					  						DefaultText =  "Search Inv. # / Vendor",
					  						FullLocalDefaultText =  "חפש חשבונית\\ספק",
					  						HelpTextCode =  "SearchFields",
					  						HelpTextDefaultText =  "Searching by :\n1:Inv. # \n2:Vendor",
					  						HelpLocalDefaultText =  "חפש לפי חשבונית \\ספק",
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
					 
					 						FieldName =  "UnpaidInvoices",
					  						OldFieldName =  "UnpaidInvoices",
					  						ObjectTableName =  "APInvoice",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					  						DefaultText =  "UnpaidInvoices",
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
					 
					 						FieldName =  "AmountDue",
					  						OldFieldName =  "AmountDue",
					  						ObjectTableName =  "APInvoice",
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
					  						DataTemplateName =  "APInvoiceAmountDueDataTemplate",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					  						FullLocalDefaultText =  "סכום פתוח",
					  						ListFieldLable =  "AmountDueListLable",
					  						ListLableDefaultText =  "Amount Due",
					  						ListLocalDefaultText =  "סכום פתוח",
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
					 
					 						FieldName =  "AmountDueInLocalCurrency",
					  						OldFieldName =  "AmountDueInLocalCurrency",
					  						ObjectTableName =  "APInvoice",
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
					  						DataTemplateName =  "APInvoiceAmountDueInLocalDataTemplate",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					  						ObjectTableName =  "APInvoice",
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
					  						DataTemplateName =  "APInvoiceAmountDueInProfitDataTemplate",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					 
					 						FieldName =  "BranchId",
					  						OldFieldName =  "BranchId",
					  						ObjectTableName =  "APInvoice",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					 
					 						FieldName =  "ConnectedEntityReferences",
					  						OldFieldName =  "ConnectedEntityReferences",
					  						ObjectTableName =  "APInvoice",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					 
					 						FieldName =  "LocalCurrencyCode",
					  						OldFieldName =  "LocalCurrencyCode",
					  						ObjectTableName =  "APInvoice",
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
					  						Operator =  "Equals",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					 
					 						FieldName =  "HouseNumber",
					  						OldFieldName =  "HouseNumber",
					  						ObjectTableName =  "APInvoice",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					  						ObjectTableName =  "APInvoice",
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
					  						DisplayInList =  false,
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					 
					 						FieldName =  "TransferTries",
					  						OldFieldName =  "TransferTries",
					  						ObjectTableName =  "APInvoice",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					 
					 						FieldName =  "TransferError",
					  						OldFieldName =  "TransferError",
					  						ObjectTableName =  "APInvoice",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					 
					 						FieldName =  "IsTransferStarted",
					  						OldFieldName =  "IsTransferStarted",
					  						ObjectTableName =  "APInvoice",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					 
					 						FieldName =  "TransferStatusCode",
					  						OldFieldName =  "TransferStatusCode",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "APInvoiceTransferStatus",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					 
					 						FieldName =  "AccountingExternalCode",
					  						OldFieldName =  "AccountingExternalCode",
					  						ObjectTableName =  "APInvoice",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					 
					 						FieldName =  "NotReadyInvoices",
					  						OldFieldName =  "NotReadyInvoices",
					  						ObjectTableName =  "APInvoice",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					 
					 						FieldName =  "MarkedAsBlockedForTransfer",
					  						OldFieldName =  "MarkedAsBlockedForTransfer",
					  						ObjectTableName =  "APInvoice",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					 
					 						FieldName =  "ErrorInTransferInvoices",
					  						OldFieldName =  "ErrorInTransferInvoices",
					  						ObjectTableName =  "APInvoice",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					 
					 						FieldName =  "ReadyForTransfer",
					  						OldFieldName =  "ReadyForTransfer",
					  						ObjectTableName =  "APInvoice",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					 
					 						FieldName =  "CreditAccount",
					  						OldFieldName =  "CreditAccount",
					  						ObjectTableName =  "APInvoice",
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
					  						PMPropertyPath =  "CreditAccount",
					  						ListPropertyPath =  "CreditAccount",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "CreditAccount",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CreditAccount",
					  						DefaultText =  "Credit Account",
					  						FullLocalDefaultText =  "כרטיס זכות",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "CreditAccount",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TransferStatusName",
					  						OldFieldName =  "TransferStatusName",
					  						ObjectTableName =  "APInvoice",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					 
					 						FieldName =  "IsMultipleEntities",
					  						OldFieldName =  "IsMultipleEntities",
					  						ObjectTableName =  "APInvoice",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsMultipleEntities",
					  						ListPropertyPath =  "IsMultipleEntities",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "IsMultipleEntities",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsMultipleEntities",
					  						DefaultText =  "Is Multiple Entities",
					  						FullLocalDefaultText =  "האם ישויות מרובות",
					  						ListFieldLable =  "IsMultipleEntitiesListLable",
					  						ListLableDefaultText =  "Is Multiple Entities",
					  						ListLocalDefaultText =  "האם ישויות מרובות",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "IsMultipleEntities",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "VendorCode",
					  						OldFieldName =  "VendorCode",
					  						ObjectTableName =  "APInvoice",
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
					  						PMPropertyPath =  "VendorCode",
					  						ListPropertyPath =  "VendorCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "VendorCode",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "VendorCode",
					  						DefaultText =  "Vendor Code",
					  						FullLocalDefaultText =  "קוד ספק",
					  						ListFieldLable =  "VendorCodeListLable",
					  						ListLableDefaultText =  "Vendor Code",
					  						ListLocalDefaultText =  "קוד ספק",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "VendorCode",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ApprovedDate",
					  						OldFieldName =  "ApprovedDate",
					  						ObjectTableName =  "APInvoice",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					  						ObjectTableName =  "APInvoice",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					  						ObjectTableName =  "APInvoice",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					  						ObjectTableName =  "APInvoice",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					 
					 						FieldName =  "VendorGLAccountId",
					  						OldFieldName =  "VendorGLAccountId",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "GLAccount",
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
					  						PMPropertyPath =  "VendorGLAccountId",
					  						ListPropertyPath =  "VendorGLAccountId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "VendorGLAccountId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "VendorGLAccountId",
					  						DefaultText =  "Vendor GL Account",
					  						FullLocalDefaultText =  "כרטסת ספק",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "VendorGLAccountId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AccountingDate",
					  						OldFieldName =  "AccountingDate",
					  						ObjectTableName =  "APInvoice",
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
					  						ValidForQuerySection1 =  "APInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "AccountingDate",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AccountingDate",
					  						DefaultText =  "Accounting Date",
					  						FullLocalDefaultText =  "תאריך חשבונאי",
					  						ListFieldLable =  "AccountingDateListLable",
					  						ListLableDefaultText =  "Accounting Date",
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
					  						HelpTextCode =  "AccountingDate",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsExternalEntity",
					  						OldFieldName =  "IsExternalEntity",
					  						ObjectTableName =  "APInvoice",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					 
					 						FieldName =  "AmountPaid",
					  						OldFieldName =  "AmountPaid",
					  						ObjectTableName =  "APInvoice",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					 
					 						FieldName =  "DraftGeneralAPInvoices",
					  						OldFieldName =  "DraftGeneralAPInvoices",
					  						ObjectTableName =  "APInvoice",
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
					  						PMPropertyPath =  "DraftGeneralAPInvoices",
					  						ListPropertyPath =  "DraftGeneralAPInvoices",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "DraftGeneralAPInvoices",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DraftGeneralAPInvoices",
					  						DefaultText =  "Draft General Invoices",
					  						FullLocalDefaultText =  "חשבוניות בסטטוס טיוטה",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "DraftGeneralAPInvoices",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ApprovalGeneralAPInvoices",
					  						OldFieldName =  "ApprovalGeneralAPInvoices",
					  						ObjectTableName =  "APInvoice",
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
					  						PMPropertyPath =  "ApprovalGeneralAPInvoices",
					  						ListPropertyPath =  "ApprovalGeneralAPInvoices",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ApprovalGeneralAPInvoices",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ApprovalGeneralAPInvoices",
					  						DefaultText =  "Approval General APInvoices",
					  						FullLocalDefaultText =  "חשבוניות מאושרות ",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "ApprovalGeneralAPInvoices",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FirstApproveDate",
					  						OldFieldName =  "FirstApproveDate",
					  						ObjectTableName =  "APInvoice",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "FirstApproveDate",
					  						ListPropertyPath =  "FirstApproveDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "FirstApproveDate",
					  						DefaultText =  "First Approve Date",
					  						ListFieldLable =  "FirstApproveDateListLable",
					  						ListLableDefaultText =  "First Approve Date",
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
					 
					 						FieldName =  "JournalNumber",
					  						OldFieldName =  "JournalNumber",
					  						ObjectTableName =  "APInvoice",
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
					  						ValidForQuerySection1 =  "מספר פקודה",
					  						ValidForQuerySection2 =  "APInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "JournalNumber",
					  						DefaultText =  "Journal No.",
					  						FullLocalDefaultText =  "מספר פקודה",
					  						ListFieldLable =  "JournalNumberListLable",
					  						ListLableDefaultText =  "Journal No.",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BranchName",
					  						OldFieldName =  "BranchName",
					  						ObjectTableName =  "APInvoice",
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
					  						ValidForQuerySection1 =  "APInvoice",
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
					 
					 						FieldName =  "JournalId",
					  						OldFieldName =  "JournalId",
					  						ObjectTableName =  "APInvoice",
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
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "JournalId",
					  						DefaultText =  "JournalId",
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
					 
					 						FieldName =  "VendorLocalName",
					  						OldFieldName =  "VendorLocalName",
					  						ObjectTableName =  "APInvoice",
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
					  						PMPropertyPath =  "VendorLocalName",
					  						ListPropertyPath =  "VendorLocalName",
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
					  						FullFieldLable =  "VendorLocalName",
					  						DefaultText =  "Vendor Name",
					  						FullLocalDefaultText =  "ספק",
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
					 
					 						FieldName =  "VendorCity",
					  						OldFieldName =  "VendorCity",
					  						ObjectTableName =  "APInvoice",
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
					  						PMPropertyPath =  "VendorCity",
					  						ListPropertyPath =  "VendorCity",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "VendorCity",
					  						DefaultText =  "Vendor City",
					  						ListFieldLable =  "VendorCityListLable",
					  						ListLableDefaultText =  "Vendor City",
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
					 
					 						FieldName =  "VendorCountry",
					  						OldFieldName =  "VendorCountry",
					  						ObjectTableName =  "APInvoice",
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
					  						PMPropertyPath =  "VendorCountry",
					  						ListPropertyPath =  "VendorCountry",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "VendorCountry",
					  						DefaultText =  "Vendor Country",
					  						ListFieldLable =  "VendorCountryListLable",
					  						ListLableDefaultText =  "Vendor Country",
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
					  						ObjectTableName =  "APInvoice",
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
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APInvoice",
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
	        QueryGroup APInvoiceQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "APIN", Name = "AP Invoices" }, queryGroupRepository);
						QueryGroup APInvoiceQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "37d0", Name = " Query Group" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable APInvoiceObjectTable = objectContext.ObjectTables.Where(d => d.Name == "APInvoice" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> APInvoiceObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "APInvoice").ToList();   

			   TextCode APInvoiceTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.Q.AllGeneralInvoices", DefaultText = @"All General Invoices",LocalDefaultText = "כל החשבוניות", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature APInvoiceFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ALLGENERALAPINVOICES", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.AllGeneralInvoice", NameTextCodeDefaultText = "All General Invoices", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode APInvoiceTextCode_1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.Q.DraftGeneralInvoices", DefaultText = @"Draft General APInvoices",LocalDefaultText = "חשבוניות בסטטוס טיוטה", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature APInvoiceFeature_1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DRAFTGENERALAPINVOICES", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.DraftGeneralAPInvoice", NameTextCodeDefaultText = "Draft General APInvoices", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode APInvoiceTextCode_2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.Q.ApprovalGeneralInvoices", DefaultText = @"Approval General APInvoices",LocalDefaultText = "חשבוניות מאושרות", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature APInvoiceFeature_2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "APPROVALGENERALAPINVOICES", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.ApprovalGeneralAPInvoice", NameTextCodeDefaultText = "Approval General APInvoices", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode APInvoiceTextCode_3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.Q.AllAPInvoices", DefaultText = @"All Invoices",LocalDefaultText = "כל החשבוניות", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature APInvoiceFeature_3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ALLINVOICES", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.AllInvoices", NameTextCodeDefaultText = "All Invoices", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode APInvoiceTextCode_4 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.Q.WaitingApprovalAPInvoices", DefaultText = @"Waiting for Approval",LocalDefaultText = "חשבוניות ממתינות לאישור", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature APInvoiceFeature_4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "WAITAPPROVAL", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.WaitingForApproval", NameTextCodeDefaultText = "Waiting for Approval", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode APInvoiceTextCode_5 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.Q.UnpaidAPInvoices", DefaultText = @"Unpaid Invoices",LocalDefaultText = "חשבוניות שלא שולמו", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature APInvoiceFeature_5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UNPAIDINVOICES", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.UnpaidInvoices", NameTextCodeDefaultText = "Unpaid Invoices", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode APInvoiceTextCode_6 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.Q.NotReadyInvoices", DefaultText = @"Not Ready Invoices",LocalDefaultText = "חשבוניות לא מוכנות", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature APInvoiceFeature_6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NOTREADYINVOICES", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.NotReadyInvoices", NameTextCodeDefaultText = "Not Ready Invoices", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode APInvoiceTextCode_7 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.Q.MarkedAsBlockedForTransfer", DefaultText = @"Marked as blocked for transfer",LocalDefaultText = "סמן כחסום להעברה", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature APInvoiceFeature_7 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MARKEDASBLOCKEDFORTRANSFER", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.MarkedAsBlockedForTransfer", NameTextCodeDefaultText = "Marked as blocked for transfer", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode APInvoiceTextCode_8 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.Q.ErrorInTransferInvoices", DefaultText = @"Error In Transfer Invoices",LocalDefaultText = "שגיאה בהעברת חשבוניות", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature APInvoiceFeature_8 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ERRORINTRANSFERINVOICES", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.ErrorInTransferInvoices", NameTextCodeDefaultText = "Error In Transfer Invoices", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query AllGeneralAPInvoicesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = APInvoiceTextCode_0.Id, Code = "All General APInvoices",  QueryGroupCode = "APIN", IndexOrder = 0, Tenant = 0, ObjectTableId = APInvoiceObjectTable.Id, QuerySection = "APInvoice", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = APInvoiceFeature_0.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn AllGeneralAPInvoicesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGeneralAPInvoicesQuery.Id, IndexOrder = 0, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGeneralAPInvoicesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGeneralAPInvoicesQuery.Id, IndexOrder = 1, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGeneralAPInvoicesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGeneralAPInvoicesQuery.Id, IndexOrder = 2, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGeneralAPInvoicesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGeneralAPInvoicesQuery.Id, IndexOrder = 3, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGeneralAPInvoicesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGeneralAPInvoicesQuery.Id, IndexOrder = 4, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGeneralAPInvoicesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGeneralAPInvoicesQuery.Id, IndexOrder = 5, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGeneralAPInvoicesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGeneralAPInvoicesQuery.Id, IndexOrder = 6, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGeneralAPInvoicesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGeneralAPInvoicesQuery.Id, IndexOrder = 7, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "UpdateDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "UpdateDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGeneralAPInvoicesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGeneralAPInvoicesQuery.Id, IndexOrder = 8, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "UpdatedByUserName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "UpdatedByUserName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGeneralAPInvoicesQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGeneralAPInvoicesQuery.Id, IndexOrder = 9, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGeneralAPInvoicesQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGeneralAPInvoicesQuery.Id, IndexOrder = 10, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGeneralAPInvoicesQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGeneralAPInvoicesQuery.Id, IndexOrder = 11, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter AllGeneralAPInvoicesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "IsGeneralInvoice" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "IsGeneralInvoice" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "true",PredefinedValue2 = null, QueryId = AllGeneralAPInvoicesQuery.Id, Tenant = 0,Operator = "Equals"}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query DraftGeneralAPInvoicesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = APInvoiceTextCode_1.Id, Code = "Draft General APInvoices",  QueryGroupCode = "APIN", IndexOrder = 1, Tenant = 0, ObjectTableId = APInvoiceObjectTable.Id, QuerySection = "APInvoice", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = APInvoiceFeature_1.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn DraftGeneralAPInvoicesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftGeneralAPInvoicesQuery.Id, IndexOrder = 0, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftGeneralAPInvoicesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftGeneralAPInvoicesQuery.Id, IndexOrder = 1, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftGeneralAPInvoicesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftGeneralAPInvoicesQuery.Id, IndexOrder = 2, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftGeneralAPInvoicesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftGeneralAPInvoicesQuery.Id, IndexOrder = 3, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftGeneralAPInvoicesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftGeneralAPInvoicesQuery.Id, IndexOrder = 4, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftGeneralAPInvoicesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftGeneralAPInvoicesQuery.Id, IndexOrder = 5, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftGeneralAPInvoicesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftGeneralAPInvoicesQuery.Id, IndexOrder = 6, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftGeneralAPInvoicesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftGeneralAPInvoicesQuery.Id, IndexOrder = 7, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "UpdateDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "UpdateDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftGeneralAPInvoicesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftGeneralAPInvoicesQuery.Id, IndexOrder = 8, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "UpdatedByUserName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "UpdatedByUserName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftGeneralAPInvoicesQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftGeneralAPInvoicesQuery.Id, IndexOrder = 9, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftGeneralAPInvoicesQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftGeneralAPInvoicesQuery.Id, IndexOrder = 10, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftGeneralAPInvoicesQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftGeneralAPInvoicesQuery.Id, IndexOrder = 11, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter DraftGeneralAPInvoicesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "DraftGeneralAPInvoices" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "DraftGeneralAPInvoices" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "true",PredefinedValue2 = null, QueryId = DraftGeneralAPInvoicesQuery.Id, Tenant = 0,Operator = "Equals"}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query ApprovalGeneralAPInvoicesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = APInvoiceTextCode_2.Id, Code = "Approval General APInvoices",  QueryGroupCode = "APIN", IndexOrder = 2, Tenant = 0, ObjectTableId = APInvoiceObjectTable.Id, QuerySection = "APInvoice", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = APInvoiceFeature_2.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn ApprovalGeneralAPInvoicesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovalGeneralAPInvoicesQuery.Id, IndexOrder = 0, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApprovalGeneralAPInvoicesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovalGeneralAPInvoicesQuery.Id, IndexOrder = 1, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApprovalGeneralAPInvoicesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovalGeneralAPInvoicesQuery.Id, IndexOrder = 2, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApprovalGeneralAPInvoicesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovalGeneralAPInvoicesQuery.Id, IndexOrder = 3, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApprovalGeneralAPInvoicesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovalGeneralAPInvoicesQuery.Id, IndexOrder = 4, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApprovalGeneralAPInvoicesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovalGeneralAPInvoicesQuery.Id, IndexOrder = 5, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApprovalGeneralAPInvoicesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovalGeneralAPInvoicesQuery.Id, IndexOrder = 6, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApprovalGeneralAPInvoicesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovalGeneralAPInvoicesQuery.Id, IndexOrder = 7, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "UpdateDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "UpdateDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApprovalGeneralAPInvoicesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovalGeneralAPInvoicesQuery.Id, IndexOrder = 8, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "UpdatedByUserName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "UpdatedByUserName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApprovalGeneralAPInvoicesQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovalGeneralAPInvoicesQuery.Id, IndexOrder = 9, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApprovalGeneralAPInvoicesQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovalGeneralAPInvoicesQuery.Id, IndexOrder = 10, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApprovalGeneralAPInvoicesQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovalGeneralAPInvoicesQuery.Id, IndexOrder = 11, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter ApprovalGeneralAPInvoicesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "ApprovalGeneralAPInvoices" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "ApprovalGeneralAPInvoices" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "true",PredefinedValue2 = null, QueryId = ApprovalGeneralAPInvoicesQuery.Id, Tenant = 0,Operator = "Equals"}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query AllInvoicesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = APInvoiceTextCode_3.Id, Code = "All Invoices",  QueryGroupCode = "APIN", IndexOrder = 3, Tenant = 0, ObjectTableId = APInvoiceObjectTable.Id, QuerySection = "APInvoice", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = APInvoiceFeature_3.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn AllInvoicesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllInvoicesQuery.Id, IndexOrder = 0, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllInvoicesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllInvoicesQuery.Id, IndexOrder = 1, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllInvoicesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllInvoicesQuery.Id, IndexOrder = 2, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllInvoicesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllInvoicesQuery.Id, IndexOrder = 3, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllInvoicesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllInvoicesQuery.Id, IndexOrder = 4, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllInvoicesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllInvoicesQuery.Id, IndexOrder = 5, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllInvoicesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllInvoicesQuery.Id, IndexOrder = 6, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllInvoicesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllInvoicesQuery.Id, IndexOrder = 7, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "UpdateDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "UpdateDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllInvoicesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllInvoicesQuery.Id, IndexOrder = 8, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "UpdatedByUserName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "UpdatedByUserName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllInvoicesQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllInvoicesQuery.Id, IndexOrder = 9, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllInvoicesQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllInvoicesQuery.Id, IndexOrder = 10, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllInvoicesQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllInvoicesQuery.Id, IndexOrder = 11, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
  
	      

			  Query WaitingforApprovalQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = APInvoiceTextCode_4.Id, Code = "Waiting for Approval",  QueryGroupCode = "APIN", IndexOrder = 4, Tenant = 0, ObjectTableId = APInvoiceObjectTable.Id, QuerySection = "APInvoice", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = APInvoiceFeature_4.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn WaitingforApprovalQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = WaitingforApprovalQuery.Id, IndexOrder = 0, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn WaitingforApprovalQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = WaitingforApprovalQuery.Id, IndexOrder = 1, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn WaitingforApprovalQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = WaitingforApprovalQuery.Id, IndexOrder = 2, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn WaitingforApprovalQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = WaitingforApprovalQuery.Id, IndexOrder = 3, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn WaitingforApprovalQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = WaitingforApprovalQuery.Id, IndexOrder = 4, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn WaitingforApprovalQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = WaitingforApprovalQuery.Id, IndexOrder = 5, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn WaitingforApprovalQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = WaitingforApprovalQuery.Id, IndexOrder = 6, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn WaitingforApprovalQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = WaitingforApprovalQuery.Id, IndexOrder = 7, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn WaitingforApprovalQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = WaitingforApprovalQuery.Id, IndexOrder = 8, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InternalNumber" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "InternalNumber" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter WaitingforApprovalQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "StatusCode" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "StatusCode" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "WA",PredefinedValue2 = null, QueryId = WaitingforApprovalQuery.Id, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query UnpaidInvoicesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = APInvoiceTextCode_5.Id, Code = "Unpaid Invoices",  QueryGroupCode = "APIN", IndexOrder = 5, Tenant = 0, ObjectTableId = APInvoiceObjectTable.Id, QuerySection = "APInvoice", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = APInvoiceFeature_5.Id, DefaultSortName = "DueDate", DefaultSortDirection = "Ascending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn UnpaidInvoicesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnpaidInvoicesQuery.Id, IndexOrder = 0, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn UnpaidInvoicesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnpaidInvoicesQuery.Id, IndexOrder = 1, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn UnpaidInvoicesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnpaidInvoicesQuery.Id, IndexOrder = 2, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn UnpaidInvoicesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnpaidInvoicesQuery.Id, IndexOrder = 3, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn UnpaidInvoicesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnpaidInvoicesQuery.Id, IndexOrder = 4, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn UnpaidInvoicesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnpaidInvoicesQuery.Id, IndexOrder = 5, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn UnpaidInvoicesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnpaidInvoicesQuery.Id, IndexOrder = 6, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn UnpaidInvoicesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnpaidInvoicesQuery.Id, IndexOrder = 7, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn UnpaidInvoicesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnpaidInvoicesQuery.Id, IndexOrder = 8, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InternalNumber" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "InternalNumber" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter UnpaidInvoicesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "UnpaidInvoices" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "UnpaidInvoices" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "true",PredefinedValue2 = null, QueryId = UnpaidInvoicesQuery.Id, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query NotReadyInvoicesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = APInvoiceTextCode_6.Id, Code = "Not Ready Invoices",  EditWizardName = "Simplog.InvoiceLib.Views.Tabs.APInvoiceTabs.APTransferEditControl",
			   EditWizardComponentPath = "./InvoiceModules/APInvoice/Components/NewEntity/APInvoiceTransferTemplate",
			   QueryGroupCode = "APIN", IndexOrder = 6, Tenant = 0, ObjectTableId = APInvoiceObjectTable.Id, QuerySection = "APInvoice", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = APInvoiceFeature_6.Id, DefaultSortName = "InvoiceDate", DefaultSortDirection = "Ascending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn NotReadyInvoicesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotReadyInvoicesQuery.Id, IndexOrder = 0, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn NotReadyInvoicesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotReadyInvoicesQuery.Id, IndexOrder = 1, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn NotReadyInvoicesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotReadyInvoicesQuery.Id, IndexOrder = 2, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 190 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn NotReadyInvoicesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotReadyInvoicesQuery.Id, IndexOrder = 3, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn NotReadyInvoicesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotReadyInvoicesQuery.Id, IndexOrder = 4, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn NotReadyInvoicesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotReadyInvoicesQuery.Id, IndexOrder = 5, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "ReadyForTransfer" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "ReadyForTransfer" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 50 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn NotReadyInvoicesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotReadyInvoicesQuery.Id, IndexOrder = 6, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "TransferError" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "TransferError" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 500 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter NotReadyInvoicesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "NotReadyInvoices" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "NotReadyInvoices" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "true",PredefinedValue2 = null, QueryId = NotReadyInvoicesQuery.Id, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query MarkedasblockedfortransferQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = APInvoiceTextCode_7.Id, Code = "Marked as blocked for transfer",  EditWizardName = "Simplog.InvoiceLib.Views.Tabs.APInvoiceTabs.APTransferEditControl",
			   EditWizardComponentPath = "./InvoiceModules/APInvoice/Components/NewEntity/APInvoiceTransferTemplate",
			   QueryGroupCode = "APIN", IndexOrder = 7, Tenant = 0, ObjectTableId = APInvoiceObjectTable.Id, QuerySection = "APInvoice", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = APInvoiceFeature_7.Id, DefaultSortName = "InvoiceDate", DefaultSortDirection = "Ascending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn MarkedasblockedfortransferQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MarkedasblockedfortransferQuery.Id, IndexOrder = 0, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MarkedasblockedfortransferQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MarkedasblockedfortransferQuery.Id, IndexOrder = 1, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MarkedasblockedfortransferQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MarkedasblockedfortransferQuery.Id, IndexOrder = 2, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 200 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MarkedasblockedfortransferQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MarkedasblockedfortransferQuery.Id, IndexOrder = 3, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MarkedasblockedfortransferQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MarkedasblockedfortransferQuery.Id, IndexOrder = 4, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter MarkedasblockedfortransferQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "MarkedAsBlockedForTransfer" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "MarkedAsBlockedForTransfer" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "true",PredefinedValue2 = null, QueryId = MarkedasblockedfortransferQuery.Id, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query ErrorInTransferInvoicesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = APInvoiceTextCode_8.Id, Code = "Error In Transfer Invoices",  EditWizardName = "Simplog.InvoiceLib.Views.Tabs.APInvoiceTabs.APTransferEditControl",
			   EditWizardComponentPath = "./InvoiceModules/APInvoice/Components/NewEntity/APInvoiceTransferTemplate",
			   QueryGroupCode = "APIN", IndexOrder = 8, Tenant = 0, ObjectTableId = APInvoiceObjectTable.Id, QuerySection = "APInvoice", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = APInvoiceFeature_8.Id, DefaultSortName = "InvoiceDate", DefaultSortDirection = "Ascending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn ErrorInTransferInvoicesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ErrorInTransferInvoicesQuery.Id, IndexOrder = 0, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ErrorInTransferInvoicesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ErrorInTransferInvoicesQuery.Id, IndexOrder = 1, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ErrorInTransferInvoicesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ErrorInTransferInvoicesQuery.Id, IndexOrder = 2, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 190 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ErrorInTransferInvoicesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ErrorInTransferInvoicesQuery.Id, IndexOrder = 3, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ErrorInTransferInvoicesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ErrorInTransferInvoicesQuery.Id, IndexOrder = 4, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ErrorInTransferInvoicesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ErrorInTransferInvoicesQuery.Id, IndexOrder = 5, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "ReadyForTransfer" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "ReadyForTransfer" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 50 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ErrorInTransferInvoicesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ErrorInTransferInvoicesQuery.Id, IndexOrder = 6, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "TransferError" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "TransferError" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 500 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter ErrorInTransferInvoicesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "ErrorInTransferInvoices" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APInvoiceObjectFields.Where(d => d.FieldName == "ErrorInTransferInvoices" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "true",PredefinedValue2 = null, QueryId = ErrorInTransferInvoicesQuery.Id, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);

	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable APInvoiceObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "APInvoice" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> APInvoiceObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "APInvoice").ToList();
		       
	      

	         Screen APInvoiceGeneralTabScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "APInvoice.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = APInvoiceObjectTable.Id, NumberOfColumns = 1, NumberOfRows = 6, IsReadOnly = false }, screensRepository, tenantScreens);
      
            ScreenField APInvoiceAPInvoiceGeneralTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "UpdatedByUserId").FirstOrDefault().Id, ScreenId = APInvoiceGeneralTabScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField APInvoiceAPInvoiceGeneralTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "UpdateDate").FirstOrDefault().Id, ScreenId = APInvoiceGeneralTabScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField APInvoiceAPInvoiceGeneralTabScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "BranchId").FirstOrDefault().Id, ScreenId = APInvoiceGeneralTabScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField APInvoiceAPInvoiceGeneralTabScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 3, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "MasterNumber").FirstOrDefault().Id, ScreenId = APInvoiceGeneralTabScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField APInvoiceAPInvoiceGeneralTabScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 4, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "HouseNumber").FirstOrDefault().Id, ScreenId = APInvoiceGeneralTabScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField APInvoiceAPInvoiceGeneralTabScreenScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 5, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "AccountingDate").FirstOrDefault().Id, ScreenId = APInvoiceGeneralTabScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
           
	      

	         Screen APInvoiceHeaderScreenScreen1 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "APInvoice.HeaderScreen", Name = "Header Screen", ObjectTableId = APInvoiceObjectTable.Id, NumberOfColumns = 5, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField APInvoiceAPInvoiceHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "ConnectedEntityReferences").FirstOrDefault().Id, ScreenId = APInvoiceHeaderScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField APInvoiceAPInvoiceHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "DueDate").FirstOrDefault().Id, ScreenId = APInvoiceHeaderScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField APInvoiceAPInvoiceHeaderScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "AmountDue").FirstOrDefault().Id, ScreenId = APInvoiceHeaderScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField APInvoiceAPInvoiceHeaderScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "StatusName").FirstOrDefault().Id, ScreenId = APInvoiceHeaderScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField APInvoiceAPInvoiceHeaderScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 4, Row = 0, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "TransferStatusName").FirstOrDefault().Id, ScreenId = APInvoiceHeaderScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    APInvoiceObjectTable.HeaderScreenId = APInvoiceHeaderScreenScreen1.Id;
	   		  
	      

	         Screen APInvoiceFullAccHeaderScreenScreen2 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "APInvoice.FullACCHeaderScreen", Name = "FullAccHeaderScreen", ObjectTableId = APInvoiceObjectTable.Id, NumberOfColumns = 5, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField APInvoiceAPInvoiceFullACCHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "ConnectedEntityReferences").FirstOrDefault().Id, ScreenId = APInvoiceFullAccHeaderScreenScreen2.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField APInvoiceAPInvoiceFullACCHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "DueDate").FirstOrDefault().Id, ScreenId = APInvoiceFullAccHeaderScreenScreen2.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField APInvoiceAPInvoiceFullACCHeaderScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "AmountDue").FirstOrDefault().Id, ScreenId = APInvoiceFullAccHeaderScreenScreen2.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField APInvoiceAPInvoiceFullACCHeaderScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "StatusName").FirstOrDefault().Id, ScreenId = APInvoiceFullAccHeaderScreenScreen2.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField APInvoiceAPInvoiceFullACCHeaderScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 4, Row = 0, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "JournalNumber").FirstOrDefault().Id, ScreenId = APInvoiceFullAccHeaderScreenScreen2.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    APInvoiceObjectTable.HeaderScreenId = APInvoiceFullAccHeaderScreenScreen2.Id;
	   		  

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable APInvoiceObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "APInvoice" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode APInvoiceDetailsTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.TH.Details", DefaultText = "Details",LocalDefaultText = "פרטים", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature APInvoiceDetailsFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DETAILS", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.Details", NameTextCodeDefaultText = "Details", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode APInvoiceGeneralTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.TH.General", DefaultText = "General",LocalDefaultText = "כללי", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature APInvoiceGeneralFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GENERAL", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode APInvoiceDocsOutTextCode_TH2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.TH.DocsOut", DefaultText = "Docs Out",LocalDefaultText = "מסמכים שיצאו", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature APInvoiceDocsOutFeature_TH2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DOCSOUT", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.DocsOut", NameTextCodeDefaultText = "Docs Out", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode APInvoiceDocsInTextCode_TH3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.TH.DocsIn", DefaultText = "Docs In",LocalDefaultText = "מסמכים", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature APInvoiceDocsInFeature_TH3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DOCSIN", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.DocsIn", NameTextCodeDefaultText = "Docs In", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode APInvoiceTransferDetailsTextCode_TH4 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.TH.Transfer", DefaultText = "Transfer Details",LocalDefaultText = "נתוני העברה", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature GeneralTransferDetailsFeature_TH4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACCOUNTINGTRANSFER", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.AccountingTransfer", NameTextCodeDefaultText = "Accounting Transfer", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode APInvoicePaymentsTextCode_TH5 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.TH.APPayments", DefaultText = "Payments",LocalDefaultText = "תשלומים", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature APInvoicePaymentsFeature_TH5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "APPAYMENTS", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.APPayments", NameTextCodeDefaultText = "Payments", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode APInvoiceCommunicationTextCode_TH6 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.TH.Communications", DefaultText = "Communication",LocalDefaultText = "תקשורת", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature APInvoiceCommunicationFeature_TH6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "COMMUNICATION", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.Communication", NameTextCodeDefaultText = "Communication", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode APInvoiceEventsTextCode_TH7 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.TH.Events", DefaultText = "Events",LocalDefaultText = "אירועים", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature APInvoiceEventsFeature_TH7 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EVENTS", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "PIDE",HtmlComponentName = "",HtmlComponentUrl = "./InvoiceModules/APInvoice/Components/EditTabs/APInvoiceDetailsTabComponent", FeatureId = APInvoiceDetailsFeature_TH0.Id, ControlPath = "Simplog.InvoiceLib.Views.Tabs.APInvoiceTabs.APInvoiceDetailsTabControl", ObjectTableId = APInvoiceObjectTable.Id, TabNameTextCodeId = APInvoiceDetailsTextCode_TH0.Id, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "PIGE",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = APInvoiceGeneralFeature_TH1.Id, ControlPath = "Simplog.Infrastructure.GeneralControls.GeneralTabControl", ObjectTableId = APInvoiceObjectTable.Id, TabNameTextCodeId = APInvoiceGeneralTextCode_TH1.Id, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "PIDO",HtmlComponentName = "",HtmlComponentUrl = "./InvoiceModules/APInvoice/Components/EditTabs/APInvoiceDocsOutTabComponent", FeatureId = APInvoiceDocsOutFeature_TH2.Id, ControlPath = "Simplog.InvoiceLib.Views.Tabs.APInvoiceTabs.APInvoiceDocsOutControl", ObjectTableId = APInvoiceObjectTable.Id, TabNameTextCodeId = APInvoiceDocsOutTextCode_TH2.Id, Tenant = 0, IndexOrder = 2 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "PIDI",HtmlComponentName = "",HtmlComponentUrl = "./InvoiceModules/APInvoice/Components/EditTabs/APInvoiceDocsInTabComponent", FeatureId = APInvoiceDocsInFeature_TH3.Id, ControlPath = "Simplog.InvoiceLib.Views.Tabs.APInvoiceTabs.APInvoiceDocsInControl", ObjectTableId = APInvoiceObjectTable.Id, TabNameTextCodeId = APInvoiceDocsInTextCode_TH3.Id, Tenant = 0, IndexOrder = 3 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "PIAC",HtmlComponentName = "",HtmlComponentUrl = "./InvoiceModules/APInvoice/Components/EditTabs/APInvoiceTransferTabComponent", FeatureId = GeneralTransferDetailsFeature_TH4.Id, ControlPath = "Simplog.InvoiceLib.Views.Tabs.APInvoiceTabs.APTransferTabControl", ObjectTableId = APInvoiceObjectTable.Id, TabNameTextCodeId = APInvoiceTransferDetailsTextCode_TH4.Id, Tenant = 0, IndexOrder = 4 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "PIPY",HtmlComponentName = "",HtmlComponentUrl = "./InvoiceModules/APInvoice/Components/EditTabs/APInvoicePaymentsTabComponent", FeatureId = APInvoicePaymentsFeature_TH5.Id, ControlPath = "Simplog.InvoiceLib.Views.Tabs.APInvoiceTabs.APPaymentsTabControl", ObjectTableId = APInvoiceObjectTable.Id, TabNameTextCodeId = APInvoicePaymentsTextCode_TH5.Id, Tenant = 0, IndexOrder = 5 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "APCM",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = APInvoiceCommunicationFeature_TH6.Id, ControlPath = "Simplog.Infrastructure.Views.Communications.CommunicationsControl", ObjectTableId = APInvoiceObjectTable.Id, TabNameTextCodeId = APInvoiceCommunicationTextCode_TH6.Id, Tenant = 0, IndexOrder = 6 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "APIE",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = APInvoiceEventsFeature_TH7.Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = APInvoiceObjectTable.Id, TabNameTextCodeId = APInvoiceEventsTextCode_TH7.Id, Tenant = 0, IndexOrder = 7 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable APInvoiceObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "APInvoice" && d.Tenant == 0).FirstOrDefault(); 

		   Feature APInvoiceFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = true, ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature APInvoiceFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = true, ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature APInvoiceFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = true, ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature APInvoiceFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = true, ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.PackageFeature", NameTextCodeDefaultText = "APInvoice Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes); 

		   		   //--------------> Additional Features <--------------\\

		   Feature APInvoiceFeature_RecalculateExternals = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "RecalculateExternals", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.RecalculateExternals", NameTextCodeDefaultText = @"Recalculate External IDs" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature APInvoiceFeature_EnableMultiRate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EnableMultiRate", FeatureTypeCode = "ACT", Packagable = false, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.EnableMultiRate", NameTextCodeDefaultText = @"Enable multi-rate" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature APInvoiceFeature_APInvoiceEditExchangeRate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "APInvoiceEditExchangeRate", FeatureTypeCode = "ACT", Packagable = false, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.EditExchangeRate", NameTextCodeDefaultText = @"Edit Exchange Rate" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

   
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable APInvoiceObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "APInvoice" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "UPPI",
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
                ObjectTableId = APInvoiceObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CRPI",
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
                ObjectTableId = APInvoiceObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "APIA",
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
                ObjectTableId = APInvoiceObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "APIC",
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
                ObjectTableId = APInvoiceObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "APIV",
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
                ObjectTableId = APInvoiceObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "COIN",
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
                ObjectTableId = APInvoiceObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "APID",
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
                ObjectTableId = APInvoiceObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
		   FeatureRepository featureRepository = new FeatureRepository(0); 
		   //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList(); 
		   ObjectTable APInvoiceObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "APInvoice" && d.Tenant == 0).FirstOrDefault();       
    
			   Feature APInvoiceFeature_MB0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SaveAPInvoice", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.Save", NameTextCodeDefaultText = "Save", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

      
    
			   Feature APInvoiceFeature_MB1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "APPROVE", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.Approve", NameTextCodeDefaultText = "Approve", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

      
    
			   Feature APInvoiceFeature_MB2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PRINT", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.Print", NameTextCodeDefaultText = "Print", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

			   Feature APInvoiceFeature_MB30 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CANCELAPPROVAL", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.CancelApproval", NameTextCodeDefaultText = "Cancel Approval", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
             			   Feature APInvoiceFeature_MB31 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EnableReTransfer", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.EnableReTransfer", NameTextCodeDefaultText = "Enable accounting re-transfer", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
             			   Feature APInvoiceFeature_MB32 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "VOID", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.Void", NameTextCodeDefaultText = "Void", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
             			   Feature APInvoiceFeature_MB33 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SendToQBO", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.SendToQBO", NameTextCodeDefaultText = "Send to QBO", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
              

		   TextCodeRepository.SubmitChanges();
		   FeaturesRepository.SubmitChanges();
		   MenuButtonGroup APInvoiceMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
				{
					MenuButtonGroupType = "APInvoiceEdit",
					Name = "APInvoiceEditButtonsGroup",
					ObjectTableId = APInvoiceObjectTable.Id,
					Tenant = 0
				}, menuButtonGroupRepository, tenantMenuButtonGroups);        
   
			   MenuButton APInvoiceMenuButton0 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "SaveAPInvoice",
						Index = 6, 
						IsActive = true,
						LabelTextCodeCode = "APInvoice.B.Save",
						LabelTextCodeDefaultText = "Save",
						Tenant = 0,
						MenuButtonGroupId = APInvoiceMenuButtonGroup.Id,
						ObjectTableId = APInvoiceObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = APInvoiceFeature_MB0.Id,
						Style = null,
						LocalDefaultText = "שמור",
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton APInvoiceMenuButton1 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "ApproveAPInvoice",
						Index = 7, 
						IsActive = true,
						LabelTextCodeCode = "APInvoice.B.Approve",
						LabelTextCodeDefaultText = "Approve",
						Tenant = 0,
						MenuButtonGroupId = APInvoiceMenuButtonGroup.Id,
						ObjectTableId = APInvoiceObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = APInvoiceFeature_MB1.Id,
						Style = "ApproveButtonStyle",
						LocalDefaultText = "אישור",
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton APInvoiceMenuButton2 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "PrintAPInvoice",
						Index = 8, 
						IsActive = true,
						LabelTextCodeCode = "APInvoice.B.Print",
						LabelTextCodeDefaultText = "Print",
						Tenant = 0,
						MenuButtonGroupId = APInvoiceMenuButtonGroup.Id,
						ObjectTableId = APInvoiceObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = APInvoiceFeature_MB2.Id,
						Style = null,
						LocalDefaultText = "הדפסה",
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton APInvoiceMenuButton3 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "Actions",
						Index = 100, 
						IsActive = false,
						LabelTextCodeCode = "APInvoice.B.Actions",
						LabelTextCodeDefaultText = "More",
						Tenant = 0,
						MenuButtonGroupId = APInvoiceMenuButtonGroup.Id,
						ObjectTableId = APInvoiceObjectTable.Id,
						MenuButtonType = "dropdownbutton",
						FeatureId = null,
						Style = null,
						LocalDefaultText = "נוספים",
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);

			   MenuButton APInvoiceMenuButton30 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "CancelApproval",
						Index = 2, 
						IsActive = true,
						LabelTextCodeCode = "APInvoice.B.CancelApproval",
						LabelTextCodeDefaultText = "Cancel Approval",
						Tenant = 0,
						MenuButtonGroupId = APInvoiceMenuButtonGroup.Id,
						ParentMenuButtonId = APInvoiceMenuButton3.Id,
						ObjectTableId = APInvoiceObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  APInvoiceFeature_MB30.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton APInvoiceMenuButton31 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "ReTransfer",
						Index = 3, 
						IsActive = true,
						LabelTextCodeCode = "APInvoice.B.ReTransfer",
						LabelTextCodeDefaultText = "Enable accounting re-transfer",
						Tenant = 0,
						MenuButtonGroupId = APInvoiceMenuButtonGroup.Id,
						ParentMenuButtonId = APInvoiceMenuButton3.Id,
						ObjectTableId = APInvoiceObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  APInvoiceFeature_MB31.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton APInvoiceMenuButton32 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "VoidAPInvoiceOperationsSeparator",
						Index = 4, 
						IsActive = false,
						LabelTextCodeCode = "APInvoice.B.VoidAPInvoiceOperationsSeparator",
						LabelTextCodeDefaultText = "",
						Tenant = 0,
						MenuButtonGroupId = APInvoiceMenuButtonGroup.Id,
						ParentMenuButtonId = APInvoiceMenuButton3.Id,
						ObjectTableId = APInvoiceObjectTable.Id,
						MenuButtonType = "separator",
						
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton APInvoiceMenuButton33 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "VoidAPInvoice",
						Index = 5, 
						IsActive = true,
						LabelTextCodeCode = "APInvoice.B.Void",
						LabelTextCodeDefaultText = "Void",
						Tenant = 0,
						MenuButtonGroupId = APInvoiceMenuButtonGroup.Id,
						ParentMenuButtonId = APInvoiceMenuButton3.Id,
						ObjectTableId = APInvoiceObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  APInvoiceFeature_MB32.Id,
						Style = null,
						LocalDefaultText = "ביטול",
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton APInvoiceMenuButton34 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "SendToQBO",
						Index = 14, 
						IsActive = true,
						LabelTextCodeCode = "APInvoice.B.SendToQBO",
						LabelTextCodeDefaultText = "Send to QBO",
						Tenant = 0,
						MenuButtonGroupId = APInvoiceMenuButtonGroup.Id,
						ParentMenuButtonId = APInvoiceMenuButton3.Id,
						ObjectTableId = APInvoiceObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  APInvoiceFeature_MB33.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable APInvoiceObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "APInvoice" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode APInvoiceTextCode_APInvoiceSDetailsDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.S.Details.Details", DefaultText = "Details",LocalDefaultText = @"פרטים", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APInvoiceTextCode_APInvoiceSDetailsRate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.S.Details.Rate", DefaultText = "Rate",LocalDefaultText = @"שער", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APInvoiceTextCode_APInvoiceSDetailsCurrencyDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.S.Details.CurrencyDetails", DefaultText = "Currency Details",LocalDefaultText = @"נתוני מטבע", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APInvoiceTextCode_APInvoiceSDetailsVatType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.S.Details.VatType", DefaultText = "Vat Type",LocalDefaultText = @"סוג מע''מ", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APInvoiceTextCode_APInvoiceSDetailsTotals = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.S.Details.Totals", DefaultText = "Totals",LocalDefaultText = @"סה''כ", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APInvoiceTextCode_APInvoiceSDetailsSubtotal = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.S.Details.Subtotal", DefaultText = "Subtotal",LocalDefaultText = @"סיכום ביניים", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APInvoiceTextCode_APInvoiceSPaymentsInvoicePayments = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.S.Payments.InvoicePayments", DefaultText = "Invoice Payments",LocalDefaultText = @"תשלומי חשבונית", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APInvoiceTextCode_APInvoiceSPaymentsTotals = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.S.Payments.Totals", DefaultText = "Totals",LocalDefaultText = @"סה''כ", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APInvoiceTextCode_APInvoiceSPaymentsAmountPaid = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.S.Payments.AmountPaid", DefaultText = "Amount Paid",LocalDefaultText = @"סכום ששולם", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APInvoiceTextCode_APInvoiceSPaymentsConnected = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.S.Payments.Connected", DefaultText = "Connected",LocalDefaultText = @"מקושר", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APInvoiceTextCode_APInvoiceSPaymentsNotConnected = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.S.Payments.NotConnected", DefaultText = "Not Connected",LocalDefaultText = @"לא מקושר", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APInvoiceTextCode_APInvoiceMConnectingMinusAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.M.ConnectingMinusAmount", DefaultText = "Connecting minus amount invoice is only allowed from Payments screen",LocalDefaultText = @"התאמת סכום שלילי לחשבונית אפשרי רק ממסך התשלומים", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APInvoiceTextCode_APInvoiceBAddLine = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.B.AddLine", DefaultText = "Add Line",LocalDefaultText = @"הוסף שורה", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APInvoiceTextCode_APInvoiceBApplyToAll = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.B.ApplyToAll", DefaultText = "Apply to all",LocalDefaultText = @"החל על כל", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APInvoiceTextCode_APInvoiceMCantReceiveFutureDateInvoice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.M.CantReceiveFutureDateInvoice", DefaultText = "Can't receive an invoice with a future date",LocalDefaultText = @"לא ניתן לקלוט חשבונית עם תאריך עתידי", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APInvoiceTextCode_APInvoiceMYouShouldHaveOneLineAtLeast = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.M.YouShouldHaveOneLineAtLeast", DefaultText = "You should have at least 1 invoice line",LocalDefaultText = @"עליך להקליד לפחות שורת חשבונית אחת ", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APInvoiceTextCode_APInvoiceMVatTypePercentageEmpty = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.M.VatTypePercentageEmpty", DefaultText = "Some of invoice lines Vat Type Percentage is empty",LocalDefaultText = @"בחלק מהשורות לא הוגדר אחוז מע''מ", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APInvoiceTextCode_APInvoiceMInvoiceAmountNotMatched = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.M.InvoiceAmountNotMatched", DefaultText = "Invoice Amount field doesnt match the total amount",LocalDefaultText = @"שדה סכום החשבונית אינו תואם לסיכום הסכום בשורות", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APInvoiceTextCode_APInvoiceMInvoiceLineAmountNotZero = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.M.InvoiceLineAmountNotZero", DefaultText = "Invoice line amount field must not be zero",LocalDefaultText = @"שדה השורה של חשבונית אינו יכול להיות אפס", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APInvoiceTextCode_APInvoiceMAccountingSettingsDontAllowVoid = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.M.AccountingSettingsDontAllowVoid", DefaultText = "Accounting Settings doesn't allow void A/P Invoice",LocalDefaultText = @"הגדרות הנהלת החשבונות לא מאפשרות התעלמות מחשבונית ספק", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APInvoiceTextCode_APInvoiceMDisconnectPayments = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.M.DisconnectPayments", DefaultText = "Please disconnect all payments",LocalDefaultText = @"נא לנתק את כל ההתאמות לתשלומים", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APInvoiceTextCode_APInvoiceMConfirmVoid = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.M.ConfirmVoid", DefaultText = "Once you void or delete an invoice, the change is permanent. If you void or delete an invoice and want to restore it later, you'll have to create a new invoice.",LocalDefaultText = @"ברגע שמבטלים או מתעלמים מחשבונית, השינוי הוא בלתי הפיך. במידה וביטלת חשבונית או התעלמת ממנה וברצונך לשחזר אותה, עליך ליצור חשבונית חדשה", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APInvoiceTextCode_APInvoiceMClosedInvoice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.M.ClosedInvoice", DefaultText = "This invoice is closed",LocalDefaultText = @"חשבונית זאת סגורה", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APInvoiceTextCode_APInvoiceMVendorNoGLAccount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.M.VendorNoGLAccount", DefaultText = "The vendor does not have GLAccount",LocalDefaultText = @"לספק לא קושר כרטיס הנהלת חשבונות", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APInvoiceTextCode_APInvoiceMInvoiceCurrNotMatch = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.M.InvoiceCurrNotMatch", DefaultText = "The Invoice Currency does not match the vendor GLAccount Currency ",LocalDefaultText = @"מטבע החשבונית לא תואם את מטבע הכרטיס של הספק", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APInvoiceTextCode_APInvoiceMNoGLAccount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.M.NoGLAccount", DefaultText = "The chosen charge type doesn't have GLAccount connected to it",LocalDefaultText = @"סעיף החיוב הנבחר לא מקושר לכרטיס הנה''ח", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APInvoiceTextCode_APInvoiceSShortTitle = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.S.ShortTitle", DefaultText = "A/P Invoice",LocalDefaultText = @"חשבונית ספק", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APInvoiceTextCode_APInvoiceSShortTitleMultipleShipments = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.S.ShortTitle.MultipleShipments", DefaultText = "Multiple Shipments",LocalDefaultText = @"מספר משלוחים", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APInvoiceTextCode_APInvoiceMSameInvoiceNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.M.SameInvoiceNumber", DefaultText = "Please Note that there is already another invoice with the same number by the same vendor",LocalDefaultText = @"שים לב שקיימת חשבונית לספק זה עם מספר זהה", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 