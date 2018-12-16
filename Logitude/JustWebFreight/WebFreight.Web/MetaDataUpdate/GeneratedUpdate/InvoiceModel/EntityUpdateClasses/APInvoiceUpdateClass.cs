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
namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.InvoiceModel.EntityUpdateClasses
{
   public class APInvoiceUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "APInvoice",
			      				    DBTableName =  "APInvoices",
			      				    ObjectTableSingular =  "A/P Invoice",
			      				    ObjectTablePlural =  "A/P Invoices",
			      				    DefaultText =  "A/P Invoice",
			      				    Name =  "AP Invoices",
			      				    IsNewWizard =  false,
			      				    HasCustomFilter =  true,
			      				    KeyPropertyPath =  "Id",
			      				    AutoCompleteSearchWindow =  false,
			      				    IsClosed =  false,
			      				    CacheOnClient =  false,
			      				    EditableFromAutoCompleteWindow =  false,
			      				    HasCounter =  true,
			      				    EnableEditFromLOV =  false,
			      				    EnableAddFromLOV =  false,
			      				    IsRestrictable =  false,
			      				    IsMain =  true,
			      				    IsAutoComplete =  false,
			      				    CustomFieldsCount =  0,
			      				    HasCustomFields =  false,
			      				    InActive =  false,
			      				    SearchFields =  "APInvoice,APInvoices,,Id,",
			      				    IsSaveButtonVisible =  false,
			      				    EnableSecurity =  true,
			      				    ObjectTableTypeCode =  "BR",
			      				    IsComposition =  false,
			      				    MaxNumberOfCustomFields =  0,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    HasDocuments =  true,
			      				    IsLookUp =  false,
			      				    IsEditable =  true,
			      				    ClientModuleName =  "Invoice",
			      				    HasHelper =  true,
			      				    HasShortTitle =  true,
			      				    HasMenuButtons =  true,
			      				    HasFiltersMenu =  false,
			      				    Code =  "APIN",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsGeneralInvoice",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Boolean",
					  						PMPropertyPath =  "IsGeneralInvoice",
					  						ListPropertyPath =  "IsGeneralInvoice",
					  						FullFieldLable =  "IsGeneralInvoice",
					  						DefaultText =  @"Is General Invoice",
					  						FullLocalDefaultText =  @"חשבונית כללית",
					  						ListFieldLable =  "IsGeneralInvoiceListLable",
					  						ListLableDefaultText =  @"IsGeneralInvoice",
					  						ListLocalDefaultText =  @"IsGeneralInvoice",
					  						ValidForQuerySection1 =  "APInvoice",
					  						IsRequired =  false,
					  						DisplayInList =  false,
					  						Code =  "IsGeneralInvoice",
					  						MaxLength =  1,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
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
					  						HelpTextCode =  "IsGeneralInvoice",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InternalNumber",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
					  						MaxLength =  20,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  true,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "InternalNumber",
					  						ListPropertyPath =  "InternalNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "InternalNumber",
					  						DefaultText =  @"Internal Number",
					  						ListFieldLable =  "InternalNumberListLable",
					  						ListLableDefaultText =  @"Internal #",
					  						HelpTextCode =  "InternalNumber",
					  						Code =  "InternalNumber",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"מספר פנימי",
					  						ListLocalDefaultText =  @"מספר פנימי",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InvoiceNumber",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
					  						MaxLength =  25,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  true,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "InvoiceNumber",
					  						ListPropertyPath =  "InvoiceNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "InvoiceNumber",
					  						DefaultText =  @"Invoice Number",
					  						ListFieldLable =  "InvoiceNumberListLable",
					  						ListLableDefaultText =  @"Invoice No.",
					  						HelpTextCode =  "InvoiceNumber",
					  						Code =  "InvoiceNumber",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  true,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  						FullLocalDefaultText =  @"מספר חשבונית",
					  						ListLocalDefaultText =  @"מספר חשבונית",
					  						HelpLocalDefaultText =  @"מספר חשבונית",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "VendorId",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Card",
					  						DataTypeCode =  "LookUp",
					  						MaxLength =  15,
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
					  						PMPropertyPath =  "VendorId",
					  						ListPropertyPath =  "VendorId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "VendorId",
					  						DefaultText =  @"Vendor",
					  						HelpTextCode =  "VendorId",
					  						Code =  "VendorId",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  						FullLocalDefaultText =  @"ספק",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "VendorName",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
					  						MaxLength =  70,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "VendorName",
					  						ListPropertyPath =  "VendorName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "VendorName",
					  						DefaultText =  @"Vendor",
					  						ListFieldLable =  "VendorNameListLable",
					  						ListLableDefaultText =  @"Vendor",
					  						HelpTextCode =  "VendorName",
					  						Code =  "VendorName",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"ספק",
					  						ListLocalDefaultText =  @"ספק",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "VATNumber",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
					  						MaxLength =  30,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "VATNumber",
					  						ListPropertyPath =  "VATNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "VATNumber",
					  						DefaultText =  @"VAT Number",
					  						ListFieldLable =  "VATNumberListLable",
					  						ListLableDefaultText =  @"VAT Number",
					  						HelpTextCode =  "VATNumber",
					  						Code =  "VATNumber",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"מספר ח.פ",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InvoiceDate",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "DateTime",
					  						DataTypeCode =  "DateTime",
					  						MaxLength =  0,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  true,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "InvoiceDate",
					  						ListPropertyPath =  "InvoiceDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						HasTemplate =  false,
					  						FullFieldLable =  "InvoiceDate",
					  						DefaultText =  @"Invoice Date",
					  						ListFieldLable =  "InvoiceDateListLable",
					  						ListLableDefaultText =  @"Invoice Date",
					  						HelpTextCode =  "InvoiceDate",
					  						Code =  "InvoiceDate",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  						FullLocalDefaultText =  @"תאריך חשבונית",
					  						ListLocalDefaultText =  @"תאריך חשבונית",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PaymentTermId",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "PaymentTerm",
					  						DataTypeCode =  "LookUp",
					  						MaxLength =  15,
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
					  						PMPropertyPath =  "PaymentTermId",
					  						ListPropertyPath =  "PaymentTermId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "PaymentTermId",
					  						DefaultText =  @"Payment Term",
					  						HelpTextCode =  "PaymentTermId",
					  						Code =  "PaymentTermId",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  						FullLocalDefaultText =  @"תנאי תשלום",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PaymentTermName",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
					  						MaxLength =  40,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "PaymentTermName",
					  						ListPropertyPath =  "PaymentTermName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "PaymentTermName",
					  						DefaultText =  @"Payment Term",
					  						ListFieldLable =  "PaymentTermNameListLable",
					  						ListLableDefaultText =  @"Payment Term",
					  						HelpTextCode =  "PaymentTermName",
					  						Code =  "PaymentTermName",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"תנאי תשלום",
					  						ListLocalDefaultText =  @"תנאי תשלום",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DueDate",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "DateTime",
					  						DataTypeCode =  "DateTime",
					  						MaxLength =  0,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  true,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "DueDate",
					  						ListPropertyPath =  "DueDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						DataTemplateName =  "APInvoiceDueDateTimeDataTemplate",
					  						HasTemplate =  true,
					  						FullFieldLable =  "DueDate",
					  						DefaultText =  @"Due Date",
					  						ListFieldLable =  "DueDateListLable",
					  						ListLableDefaultText =  @"Due Date",
					  						HelpTextCode =  "DueDate",
					  						Code =  "DueDate",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  						FullLocalDefaultText =  @"תאריך ערך",
					  						ListLocalDefaultText =  @"תאריך ערך",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InvoiceCurrencyExchangeRate",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Double",
					  						DataTypeCode =  "Double",
					  						MaxLength =  0,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  true,
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
					  						PMPropertyPath =  "InvoiceCurrencyExchangeRate",
					  						ListPropertyPath =  "InvoiceCurrencyExchangeRate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "InvoiceCurrencyExchangeRate",
					  						DefaultText =  @"Exchange Rate",
					  						ListFieldLable =  "InvoiceCurrencyExchangeRateListLable",
					  						ListLableDefaultText =  @"Exchange Rate",
					  						HelpTextCode =  "InvoiceCurrencyExchangeRate",
					  						Code =  "InvoiceCurrencyExchangeRate",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  						FullLocalDefaultText =  @"שער",
					  						ListLocalDefaultText =  @"שער",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ExchangeRateDate",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "DateTime",
					  						DataTypeCode =  "DateTime",
					  						MaxLength =  0,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  true,
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
					  						PMPropertyPath =  "ExchangeRateDate",
					  						ListPropertyPath =  "ExchangeRateDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						HasTemplate =  false,
					  						FullFieldLable =  "ExchangeRateDate",
					  						DefaultText =  @"Exchange Date",
					  						ListFieldLable =  "ExchangeRateDateListLable",
					  						ListLableDefaultText =  @"Exchange Date",
					  						HelpTextCode =  "ExchangeRateDate",
					  						Code =  "ExchangeRateDate",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"תאריך שער חליפין",
					  						ListLocalDefaultText =  @"תאריך שער חליפין",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InvoiceCurrencyId",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Currency",
					  						DataTypeCode =  "LookUp",
					  						MaxLength =  15,
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
					  						PMPropertyPath =  "InvoiceCurrencyId",
					  						ListPropertyPath =  "InvoiceCurrencyId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "InvoiceCurrencyId",
					  						DefaultText =  @"Invoice Currency",
					  						HelpTextCode =  "InvoiceCurrencyId",
					  						Code =  "InvoiceCurrencyId",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  						FullLocalDefaultText =  @"מטבע",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InvoiceCurrencyCode",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
					  						MaxLength =  15,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "InvoiceCurrencyCode",
					  						ListPropertyPath =  "InvoiceCurrencyCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "InvoiceCurrencyCode",
					  						DefaultText =  @"Currency",
					  						ListFieldLable =  "InvoiceCurrencyCodeListLable",
					  						ListLableDefaultText =  @"Currency",
					  						HelpTextCode =  "InvoiceCurrencyCode",
					  						Code =  "InvoiceCurrencyCode",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"מטבע",
					  						ListLocalDefaultText =  @"מטבע",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LocalCurrencyId",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Currency",
					  						DataTypeCode =  "LookUp",
					  						MaxLength =  15,
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
					  						PMPropertyPath =  "LocalCurrencyId",
					  						ListPropertyPath =  "LocalCurrencyId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "LocalCurrencyId",
					  						DefaultText =  @"Local Currency",
					  						HelpTextCode =  "LocalCurrencyId",
					  						Code =  "LocalCurrencyId",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  						FullLocalDefaultText =  @"מטבע מקומי",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InternalNotes",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
					  						MaxLength =  250,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  true,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "InternalNotes",
					  						ListPropertyPath =  "InternalNotes",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "InternalNotes",
					  						DefaultText =  @"Notes",
					  						ListFieldLable =  "InternalNotesListLable",
					  						ListLableDefaultText =  @"Notes",
					  						HelpTextCode =  "InternalNotes",
					  						Code =  "InternalNotes",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"הערות",
					  						ListLocalDefaultText =  @"הערות",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SubTotalInLocalCurrency",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Double",
					  						DataTypeCode =  "Double",
					  						MaxLength =  0,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  true,
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
					  						PMPropertyPath =  "SubTotalInLocalCurrency",
					  						ListPropertyPath =  "SubTotalInLocalCurrency",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "SubTotalInLocalCurrency",
					  						DefaultText =  @"Sub Total (Local Currency)",
					  						ListFieldLable =  "SubTotalInLocalCurrencyListLable",
					  						ListLableDefaultText =  @"Sub Total (Local Currency)",
					  						HelpTextCode =  "SubTotalInLocalCurrency",
					  						Code =  "SubTotalInLocalCurrency",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"סיכום ביניים בש'ח",
					  						ListLocalDefaultText =  @"סיכום ביניים בש'ח",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SubTotalInInvoiceCurrency",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Double",
					  						DataTypeCode =  "Double",
					  						MaxLength =  0,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  true,
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
					  						PMPropertyPath =  "SubTotalInInvoiceCurrency",
					  						ListPropertyPath =  "SubTotalInInvoiceCurrency",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "SubTotalInInvoiceCurrency",
					  						DefaultText =  @"Sub Total",
					  						ListFieldLable =  "SubTotalInInvoiceCurrencyListLable",
					  						ListLableDefaultText =  @"Sub Total",
					  						HelpTextCode =  "SubTotalInInvoiceCurrency",
					  						Code =  "SubTotalInInvoiceCurrency",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"סיכום ביניים בש'ח",
					  						ListLocalDefaultText =  @"סיכום ביניים בש'ח",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AmountInInvoiceCurrency",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "SigDouble",
					  						DataTypeCode =  "SigDouble",
					  						MaxLength =  0,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  true,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "AmountInInvoiceCurrency",
					  						ListPropertyPath =  "AmountInInvoiceCurrency",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						DataTemplateName =  "APAmountInInvoiceCurrencyDataTemplate",
					  						HasTemplate =  true,
					  						FullFieldLable =  "AmountInInvoiceCurrency",
					  						DefaultText =  @"Invoice Amount",
					  						ListFieldLable =  "AmountInInvoiceCurrencyListLable",
					  						ListLableDefaultText =  @"Invoice Amount",
					  						HelpTextCode =  "AmountInInvoiceCurrency",
					  						Code =  "AmountInInvoiceCurrency",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  						FullLocalDefaultText =  @"סכום חשבונית",
					  						ListLocalDefaultText =  @"סכום חשבונית",
					  						HelpLocalDefaultText =  @"סכום חשבונית",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AmountInLocalCurrency",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Double",
					  						DataTypeCode =  "Double",
					  						MaxLength =  0,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  true,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "AmountInLocalCurrency",
					  						ListPropertyPath =  "AmountInLocalCurrency",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						DataTemplateName =  "APAmountInLocalCurrencyDataTemplate",
					  						HasTemplate =  true,
					  						FullFieldLable =  "AmountInLocalCurrency",
					  						DefaultText =  @"Amount (Local Currency)",
					  						ListFieldLable =  "AmountInLocalCurrencyListLable",
					  						ListLableDefaultText =  @"Amount (Local Currency)",
					  						HelpTextCode =  "AmountInLocalCurrency",
					  						Code =  "AmountInLocalCurrency",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  						FullLocalDefaultText =  @"סכום(במטבע מקומי)",
					  						ListLocalDefaultText =  @"סכום(במטבע מקומי)",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "StatusCode",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "APInvoiceStatus",
					  						DataTypeCode =  "LookUp",
					  						MaxLength =  2,
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
					  						PMPropertyPath =  "StatusCode",
					  						ListPropertyPath =  "StatusCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "StatusCode",
					  						DefaultText =  @"Status",
					  						HelpTextCode =  "StatusCode",
					  						Code =  "StatusCode",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"סטטוס",
					  						HelpLocalDefaultText =  @"סטטוס",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "StatusName",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
					  						MaxLength =  40,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "StatusName",
					  						ListPropertyPath =  "StatusName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						DataTemplateName =  "ARInvoiceStatusDataTemplate",
					  						HasTemplate =  true,
					  						FullFieldLable =  "StatusName",
					  						DefaultText =  @"Status",
					  						ListFieldLable =  "StatusNameListLable",
					  						ListLableDefaultText =  @"Status",
					  						HelpTextCode =  "StatusName",
					  						Code =  "StatusName",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"סטטוס",
					  						ListLocalDefaultText =  @"סטטוס",
					  						HelpLocalDefaultText =  @"סטטוס",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreateDate",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "DateTime",
					  						DataTypeCode =  "DateTime",
					  						MaxLength =  0,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  true,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "CreateDate",
					  						ListPropertyPath =  "CreateDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						HasTemplate =  false,
					  						FullFieldLable =  "CreateDate",
					  						DefaultText =  @"Create Date",
					  						ListFieldLable =  "CreateDateListLable",
					  						ListLableDefaultText =  @"Create Date",
					  						HelpTextCode =  "CreateDate",
					  						Code =  "CreateDate",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"תאריך יצירה",
					  						ListLocalDefaultText =  @"תאריך יצירה",
					  						HelpLocalDefaultText =  @"תאריך יצירה",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreatedByUserId",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
					  						DataTypeCode =  "LookUp",
					  						MaxLength =  15,
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
					  						PMPropertyPath =  "CreatedByUserId",
					  						ListPropertyPath =  "CreatedByUserId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "CreatedByUserId",
					  						DefaultText =  @"Created By",
					  						HelpTextCode =  "CreatedByUserId",
					  						Code =  "CreatedByUserId",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  						FullLocalDefaultText =  @"יוצר",
					  						HelpLocalDefaultText =  @"יוצר",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreatedByUserName",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
					  						MaxLength =  60,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "CreatedByUserName",
					  						ListPropertyPath =  "CreatedByUserName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "CreatedByUserName",
					  						DefaultText =  @"Created By",
					  						ListFieldLable =  "CreatedByUserNameListLable",
					  						ListLableDefaultText =  @"Created By",
					  						HelpTextCode =  "CreatedByUserName",
					  						Code =  "CreatedByUserName",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"יוצר",
					  						ListLocalDefaultText =  @"יוצר",
					  						HelpLocalDefaultText =  @"יוצר",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsClosed",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Boolean",
					  						DataTypeCode =  "Boolean",
					  						MaxLength =  15,
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
					  						PMPropertyPath =  "IsClosed",
					  						ListPropertyPath =  "IsClosed",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						DataTemplateName =  "APInvoiceIsClosedPathTemplate",
					  						HasTemplate =  false,
					  						FullFieldLable =  "IsClosed",
					  						DefaultText =  @"Is Closed",
					  						ListFieldLable =  "IsClosedListLable",
					  						ListLableDefaultText =  @"Is Closed",
					  						HelpTextCode =  "IsClosed",
					  						Code =  "IsClosed",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  						FullLocalDefaultText =  @"סגור",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ProfitCurrencyId",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Currency",
					  						DataTypeCode =  "LookUp",
					  						MaxLength =  15,
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
					  						PMPropertyPath =  "ProfitCurrencyId",
					  						ListPropertyPath =  "ProfitCurrencyId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "ProfitCurrencyId",
					  						DefaultText =  @"Profit Currency",
					  						HelpTextCode =  "ProfitCurrencyId",
					  						Code =  "ProfitCurrencyId",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  						FullLocalDefaultText =  @"מטבע רווח",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ProfitCurrencyCode",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
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
					  						PMPropertyPath =  "ProfitCurrencyCode",
					  						ListPropertyPath =  "ProfitCurrencyCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "ProfitCurrencyCode",
					  						DefaultText =  @"Profit Currency",
					  						ListFieldLable =  "ProfitCurrencyCodeListLable",
					  						ListLableDefaultText =  @"Profit Currency",
					  						HelpTextCode =  "ProfitCurrencyCode",
					  						Code =  "ProfitCurrencyCode",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"מטבע רווח",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ProfitCurrencyExchangeRate",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Double",
					  						DataTypeCode =  "Double",
					  						MaxLength =  0,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  true,
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
					  						PMPropertyPath =  "ProfitCurrencyExchangeRate",
					  						ListPropertyPath =  "ProfitCurrencyExchangeRate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						DataTemplateName =  "ProfitCurrencyExchangeRateDataTemplate",
					  						HasTemplate =  false,
					  						FullFieldLable =  "ProfitCurrencyExchangeRate",
					  						DefaultText =  @"Profit Currency Exchange Rate",
					  						ListFieldLable =  "ProfitCurrencyExchangeRateListLable",
					  						ListLableDefaultText =  @"Profit Currency Exchange Rate",
					  						HelpTextCode =  "ProfitCurrencyExchangeRate",
					  						Code =  "ProfitCurrencyExchangeRate",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  						FullLocalDefaultText =  @"שער חליפין לרווח",
					  						ListLocalDefaultText =  @"שער חליפין לרווח",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AmountInProfitCurrency",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Double",
					  						DataTypeCode =  "Double",
					  						MaxLength =  0,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  true,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "AmountInProfitCurrency",
					  						ListPropertyPath =  "AmountInProfitCurrency",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						DataTemplateName =  "AmountInProfitCurrencyDataTemplate",
					  						HasTemplate =  true,
					  						FullFieldLable =  "AmountInProfitCurrency",
					  						DefaultText =  @"Amount (Profit Currency)",
					  						ListFieldLable =  "AmountInProfitCurrencyListLable",
					  						ListLableDefaultText =  @"Amount (Profit Currency)",
					  						HelpTextCode =  "AmountInProfitCurrency",
					  						Code =  "AmountInProfitCurrency",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  						FullLocalDefaultText =  @"סכום (מטבע רווח)",
					  						ListLocalDefaultText =  @"סכום (מטבע רווח)",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdatedByUserId",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
					  						DataTypeCode =  "LookUp",
					  						MaxLength =  15,
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
					  						PMPropertyPath =  "UpdatedByUserId",
					  						ListPropertyPath =  "UpdatedByUserId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "UpdatedByUserId",
					  						DefaultText =  @"Updated By",
					  						HelpTextCode =  "UpdatedByUserId",
					  						Code =  "UpdatedByUserId",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"עודכן ע''י",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdatedByUserName",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
					  						MaxLength =  60,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "UpdatedByUserName",
					  						ListPropertyPath =  "UpdatedByUserName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "UpdatedByUserName",
					  						DefaultText =  @"Updated By User",
					  						ListFieldLable =  "UpdatedByUserNameListLable",
					  						ListLableDefaultText =  @"Updated By User",
					  						HelpTextCode =  "UpdatedByUserName",
					  						Code =  "UpdatedByUserName",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"עודכן ע''י משתמש",
					  						ListLocalDefaultText =  @"עודכן ע''י משתמש",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdateDate",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "DateTime",
					  						DataTypeCode =  "DateTime",
					  						MaxLength =  0,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  true,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "UpdateDate",
					  						ListPropertyPath =  "UpdateDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						HasTemplate =  false,
					  						FullFieldLable =  "UpdateDate",
					  						DefaultText =  @"Update Date",
					  						ListFieldLable =  "UpdateDateListLable",
					  						ListLableDefaultText =  @"Update Date",
					  						HelpTextCode =  "UpdateDate",
					  						Code =  "UpdateDate",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"תאריך עדכון",
					  						ListLocalDefaultText =  @"תאריך עדכון",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "MainEntityId",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
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
					  						PMPropertyPath =  "MainEntityId",
					  						ListPropertyPath =  "MainEntityId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "MainEntityId",
					  						DefaultText =  @"Main Entity",
					  						HelpTextCode =  "MainEntityId",
					  						Code =  "MainEntityId",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"ישות ראשית",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "MainEntityReference",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
					  						MaxLength =  15,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "MainEntityReference",
					  						ListPropertyPath =  "MainEntityReference",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "MainEntityReference",
					  						DefaultText =  @"Main Entity Reference",
					  						ListFieldLable =  "MainEntityReferenceListLable",
					  						ListLableDefaultText =  @"Shipment Number",
					  						HelpTextCode =  "MainEntityReference",
					  						Code =  "MainEntityReference",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  true,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"אסמכתא ישות ראשית ",
					  						ListLocalDefaultText =  @"אסמכתא ישות ראשית ",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SearchFields",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
					  						MaxLength =  4000,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						ValidForQuerySection2 =  "APInvoiceFollowUp",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "SearchFields",
					  						DefaultText =  @"Search Inv. # / Vendor",
					  						HelpTextCode =  "SearchFields",
					  						HelpTextDefaultText =  @"Searching by :\n1:Inv. # \n2:Vendor",
					  						Code =  "SearchFields",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"חפש חשבונית\ספק",
					  						HelpLocalDefaultText =  @"חפש לפי חשבונית \ספק",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UnpaidInvoices",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Boolean",
					  						DataTypeCode =  "Boolean",
					  						MaxLength =  1,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "UnpaidInvoices",
					  						ListPropertyPath =  "UnpaidInvoices",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "UnpaidInvoices",
					  						DefaultText =  @"UnpaidInvoices",
					  						HelpTextCode =  "UnpaidInvoices",
					  						Code =  "UnpaidInvoices",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AmountDue",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Double",
					  						DataTypeCode =  "Double",
					  						MaxLength =  15,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "AmountDue",
					  						ListPropertyPath =  "AmountDue",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						DataTemplateName =  "APInvoiceAmountDueDataTemplate",
					  						HasTemplate =  true,
					  						FullFieldLable =  "AmountDue",
					  						DefaultText =  @"Amount Due",
					  						ListFieldLable =  "AmountDueListLable",
					  						ListLableDefaultText =  @"Amount Due",
					  						HelpTextCode =  "AmountDue",
					  						Code =  "AmountDue",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  						FullLocalDefaultText =  @"סכום פתוח",
					  						ListLocalDefaultText =  @"סכום פתוח",
					  						HelpLocalDefaultText =  @"סכום פתוח",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AmountDueInLocalCurrency",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Double",
					  						DataTypeCode =  "Double",
					  						MaxLength =  15,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "AmountDueInLocalCurrency",
					  						ListPropertyPath =  "AmountDueInLocalCurrency",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						DataTemplateName =  "APInvoiceAmountDueInLocalDataTemplate",
					  						HasTemplate =  true,
					  						FullFieldLable =  "AmountDueInLocalCurrency",
					  						DefaultText =  @"Amount Due (Local Currency)",
					  						ListFieldLable =  "AmountDueInLocalCurrencyListLable",
					  						ListLableDefaultText =  @"Amount Due (Local Currency)",
					  						HelpTextCode =  "AmountDueInLocalCurrency",
					  						Code =  "AmountDueInLocalCurrency",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  						FullLocalDefaultText =  @"סכום לפירעון בש''ח",
					  						ListLocalDefaultText =  @"סכום לפירעון בש''ח",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AmountDueInProfitCurrency",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Double",
					  						DataTypeCode =  "Double",
					  						MaxLength =  15,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "AmountDueInProfitCurrency",
					  						ListPropertyPath =  "AmountDueInProfitCurrency",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						DataTemplateName =  "APInvoiceAmountDueInProfitDataTemplate",
					  						HasTemplate =  true,
					  						FullFieldLable =  "AmountDueInProfitCurrency",
					  						DefaultText =  @"Amount Due (Profit Currency)",
					  						ListFieldLable =  "AmountDueInProfitCurrencyListLable",
					  						ListLableDefaultText =  @"Amount Due (Profit Currency)",
					  						HelpTextCode =  "AmountDueInProfitCurrency",
					  						Code =  "AmountDueInProfitCurrency",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  						FullLocalDefaultText =  @"סכום לתשלום(מטבע רווח)",
					  						ListLocalDefaultText =  @"סכום לתשלום(מטבע רווח)",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BranchId",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Branch",
					  						DataTypeCode =  "LookUp",
					  						MaxLength =  15,
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
					  						PMPropertyPath =  "BranchId",
					  						ListPropertyPath =  "BranchId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "BranchId",
					  						DefaultText =  @"Branch",
					  						HelpTextCode =  "BranchId",
					  						Code =  "BranchId",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  						FullLocalDefaultText =  @"סניף",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ConnectedEntityReferences",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
					  						MaxLength =  120,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "ConnectedEntityReferences",
					  						ListPropertyPath =  "ConnectedEntityReferences",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  true,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "ConnectedEntityReferences",
					  						DefaultText =  @"References",
					  						HelpTextCode =  "ConnectedEntityReferences",
					  						Code =  "ConnectedEntityReferences",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"אסמכתאות",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LocalCurrencyCode",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
					  						MaxLength =  3,
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
					  						PMPropertyPath =  "LocalCurrencyCode",
					  						ListPropertyPath =  "LocalCurrencyCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "LocalCurrencyCode",
					  						DefaultText =  @"Local Currency",
					  						HelpTextCode =  "LocalCurrencyCode",
					  						Code =  "LocalCurrencyCode",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"מטבע מקומי",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "HouseNumber",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
					  						MaxLength =  20,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  20,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "HouseNumber",
					  						ListPropertyPath =  "HouseNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "HouseNumber",
					  						DefaultText =  @"House Number",
					  						HelpTextCode =  "HouseNumber",
					  						Code =  "HouseNumber",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"מספר שטר מטען פנימי",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "MasterNumber",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
					  						MaxLength =  30,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  30,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "MasterNumber",
					  						ListPropertyPath =  "MasterNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "MasterNumber",
					  						DefaultText =  @"Master Number",
					  						HelpTextCode =  "MasterNumber",
					  						Code =  "MasterNumber",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"מספר שטר מטען ראשי",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TransferTries",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Integer",
					  						DataTypeCode =  "Integer",
					  						MaxLength =  1,
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
					  						PMPropertyPath =  "TransferTries",
					  						ListPropertyPath =  "TransferTries",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "TransferTries",
					  						DefaultText =  @"Transfer tries",
					  						HelpTextCode =  "TransferTries",
					  						Code =  "TransferTries",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"נסיונות העברה",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TransferError",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
					  						MaxLength =  250,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "TransferError",
					  						ListPropertyPath =  "TransferError",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						DataTemplateName =  "TransferErrorDataTemplate",
					  						HasTemplate =  true,
					  						FullFieldLable =  "TransferError",
					  						DefaultText =  @"Transfer Error",
					  						ListFieldLable =  "TransferErrorListLable",
					  						ListLableDefaultText =  @"Transfer Error",
					  						HelpTextCode =  "TransferError",
					  						Code =  "TransferError",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"שגיאת העברה",
					  						ListLocalDefaultText =  @"שגיאת העברה",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsTransferStarted",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Boolean",
					  						DataTypeCode =  "Boolean",
					  						MaxLength =  1,
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
					  						PMPropertyPath =  "IsTransferStarted",
					  						ListPropertyPath =  "IsTransferStarted",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "IsTransferStarted",
					  						DefaultText =  @"Is Transfer Started",
					  						HelpTextCode =  "IsTransferStarted",
					  						Code =  "IsTransferStarted",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"החלה העברה",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TransferStatusCode",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "APInvoiceTransferStatus",
					  						DataTypeCode =  "LookUp",
					  						MaxLength =  2,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "TransferStatusCode",
					  						ListPropertyPath =  "TransferStatusCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "TransferStatusCode",
					  						DefaultText =  @"Transfer Status",
					  						HelpTextCode =  "TransferStatusCode",
					  						Code =  "TransferStatusCode",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"סטטוס העברה",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AccountingExternalCode",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "nText",
					  						DataTypeCode =  "nText",
					  						MaxLength =  25,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "AccountingExternalCode",
					  						ListPropertyPath =  "AccountingExternalCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "AccountingExternalCode",
					  						DefaultText =  @"External ID",
					  						ListFieldLable =  "AccountingExternalCodeListLable",
					  						ListLableDefaultText =  @"External ID",
					  						HelpTextCode =  "AccountingExternalCode",
					  						Code =  "AccountingExternalCode",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"חישוב מזהים חיצונים מחדש",
					  						ListLocalDefaultText =  @"חישוב מזהים חיצונים מחדש",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "NotReadyInvoices",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Boolean",
					  						DataTypeCode =  "Boolean",
					  						MaxLength =  1,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "NotReadyInvoices",
					  						ListPropertyPath =  "NotReadyInvoices",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "NotReadyInvoices",
					  						DefaultText =  @"Not Ready Invoices",
					  						HelpTextCode =  "NotReadyInvoices",
					  						Code =  "NotReadyInvoices",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"חשבוניות לא מוכנות",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "MarkedAsBlockedForTransfer",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Boolean",
					  						DataTypeCode =  "Boolean",
					  						MaxLength =  1,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "MarkedAsBlockedForTransfer",
					  						ListPropertyPath =  "MarkedAsBlockedForTransfer",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "MarkedAsBlockedForTransfer",
					  						DefaultText =  @"Marked as blocked for transfer",
					  						HelpTextCode =  "MarkedAsBlockedForTransfer",
					  						Code =  "MarkedAsBlockedForTransfer",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"סמן כחסום להעברה",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ErrorInTransferInvoices",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Boolean",
					  						DataTypeCode =  "Boolean",
					  						MaxLength =  1,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "ErrorInTransferInvoices",
					  						ListPropertyPath =  "ErrorInTransferInvoices",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "ErrorInTransferInvoices",
					  						DefaultText =  @"Error In Transfer Invoices",
					  						HelpTextCode =  "ErrorInTransferInvoices",
					  						Code =  "ErrorInTransferInvoices",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"שגיאה בהעברת חשבוניות",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ReadyForTransfer",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Boolean",
					  						DataTypeCode =  "Boolean",
					  						MaxLength =  1,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "ReadyForTransfer",
					  						ListPropertyPath =  "ReadyForTransfer",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						DataTemplateName =  "ReadyForTransferDataTemplate",
					  						HasTemplate =  true,
					  						FullFieldLable =  "ReadyForTransfer",
					  						DefaultText =  @"Ready For Transfer",
					  						ListFieldLable =  "ReadyForTransfer",
					  						ListLableDefaultText =  @"Ready",
					  						HelpTextCode =  "ReadyForTransfer",
					  						Code =  "ReadyForTransfer",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"מוכן להעברה",
					  						ListLocalDefaultText =  @"מוכן להעברה",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreditAccount",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
					  						MaxLength =  40,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "CreditAccount",
					  						ListPropertyPath =  "CreditAccount",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "CreditAccount",
					  						DefaultText =  @"Credit Account",
					  						HelpTextCode =  "CreditAccount",
					  						Code =  "CreditAccount",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"כרטיס זכות",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TransferStatusName",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
					  						MaxLength =  20,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "TransferStatusName",
					  						ListPropertyPath =  "TransferStatusName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "TransferStatusName",
					  						DefaultText =  @"Transfer Status",
					  						ListFieldLable =  "TransferStatusName",
					  						ListLableDefaultText =  @"Transfer Status",
					  						HelpTextCode =  "TransferStatusName",
					  						Code =  "TransferStatusName",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"סטטוס העברה",
					  						ListLocalDefaultText =  @"סטטוס העברה",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsMultipleEntities",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Boolean",
					  						DataTypeCode =  "Boolean",
					  						MaxLength =  1,
					  						IsCustom =  false,
					  						MinLength =  0,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "IsMultipleEntities",
					  						ListPropertyPath =  "IsMultipleEntities",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "IsMultipleEntities",
					  						DefaultText =  @"Is Multiple Entities",
					  						ListFieldLable =  "IsMultipleEntitiesListLable",
					  						ListLableDefaultText =  @"Is Multiple Entities",
					  						HelpTextCode =  "IsMultipleEntities",
					  						Code =  "IsMultipleEntities",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"האם ישויות מרובות",
					  						ListLocalDefaultText =  @"האם ישויות מרובות",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "VendorCode",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
					  						MaxLength =  15,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "VendorCode",
					  						ListPropertyPath =  "VendorCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "VendorCode",
					  						DefaultText =  @"Vendor Code",
					  						ListFieldLable =  "VendorCodeListLable",
					  						ListLableDefaultText =  @"Vendor Code",
					  						HelpTextCode =  "VendorCode",
					  						Code =  "VendorCode",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  true,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"קוד ספק",
					  						ListLocalDefaultText =  @"קוד ספק",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ApprovedDate",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "DateTime",
					  						DataTypeCode =  "DateTime",
					  						MaxLength =  1,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "ApprovedDate",
					  						ListPropertyPath =  "ApprovedDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						HasTemplate =  false,
					  						FullFieldLable =  "ApprovedDate",
					  						DefaultText =  @"Approved Date",
					  						ListFieldLable =  "ApprovedDateListLable",
					  						ListLableDefaultText =  @"Approved Date",
					  						HelpTextCode =  "ApprovedDate",
					  						Code =  "ApprovedDate",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"תאריך אישור",
					  						ListLocalDefaultText =  @"תאריך אישור",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ApprovedByUserName",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
					  						MaxLength =  60,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "ApprovedByUserName",
					  						ListPropertyPath =  "ApprovedByUserName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "ApprovedByUserName",
					  						DefaultText =  @"Approved By",
					  						ListFieldLable =  "ApprovedByUserNameListLable",
					  						ListLableDefaultText =  @"Approved By",
					  						HelpTextCode =  "ApprovedByUserName",
					  						Code =  "ApprovedByUserName",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"אושר ע''י",
					  						ListLocalDefaultText =  @"אושר ע''י",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ApprovedByUserId",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "LookUp",
					  						DataTypeCode =  "LookUp",
					  						MaxLength =  15,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "ApprovedByUserId",
					  						ListPropertyPath =  "ApprovedByUserId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "ApprovedByUserId",
					  						DefaultText =  @"Approved By",
					  						HelpTextCode =  "ApprovedByUserId",
					  						LookUpTableName =  "User",
					  						Code =  "ApprovedByUserId",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"אושר ע''י",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "OperationalDate",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "DateTime",
					  						DataTypeCode =  "DateTime",
					  						MaxLength =  1,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "OperationalDate",
					  						ListPropertyPath =  "OperationalDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						HasTemplate =  false,
					  						FullFieldLable =  "OperationalDate",
					  						DefaultText =  @"Operational Date",
					  						HelpTextCode =  "OperationalDate",
					  						Code =  "OperationalDate",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"תאריך תפעולי",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "VendorGLAccountId",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "LookUp",
					  						DataTypeCode =  "Text",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "VendorGLAccountId",
					  						ListPropertyPath =  "VendorGLAccountId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "VendorGLAccountId",
					  						DefaultText =  @"Vendor GL Account",
					  						HelpTextCode =  "VendorGLAccountId",
					  						LookUpTableName =  "GLAccount",
					  						Code =  "VendorGLAccountId",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"כרטסת ספק",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AccountingDate",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "DateTime",
					  						DataTypeCode =  "DateTime",
					  						MaxLength =  1,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "AccountingDate",
					  						ListPropertyPath =  "AccountingDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						HasTemplate =  false,
					  						FullFieldLable =  "AccountingDate",
					  						DefaultText =  @"Accounting Date",
					  						HelpTextCode =  "AccountingDate",
					  						Code =  "AccountingDate",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"תאריך רישום ",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsExternalEntity",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Boolean",
					  						DataTypeCode =  "Boolean",
					  						MaxLength =  1,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "IsExternalEntity",
					  						ListPropertyPath =  "IsExternalEntity",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APInvoice",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "IsExternalEntity",
					  						DefaultText =  @"Is external entity",
					  						HelpTextCode =  "IsExternalEntity",
					  						Code =  "IsExternalEntity",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"ישות חיצונית",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AmountPaid",
					  						PMPropertyPath =  "AmountPaid",
					  						ListPropertyPath =  "AmountPaid",
					  						FullFieldLable =  "AmountPaid",
					  						DefaultText =  @"Amount To Pay",
					  						FullLocalDefaultText =  @"סכום לתשלום",
					  						ListFieldLable =  "AmountPaidListLable",
					  						ListLableDefaultText =  @"AmountPaid",
					  						ListLocalDefaultText =  @"AmountPaid",
					  						ObjectTableName =  "APInvoice",
					  						ValidForQuerySection1 =  "APInvoice",
					  						FieldsDataType =  "Double",
					  						Code =  "AmountPaid",
					  						MaxLength =  1,
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
					  						HelpTextCode =  "AmountPaid",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "OpenAmountInInvoiceCurrency",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Double",
					  						Code =  "OpenAmountInInvoiceCurrency",
					  						MaxLength =  0,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  true,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "OpenAmountInInvoiceCurrency",
					  						ListPropertyPath =  "OpenAmountInInvoiceCurrency",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APInvoice",
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
					  						DataTemplateName =  "OpenAmountInInvoiceCurrencyDataTemplate",
					  						HasTemplate =  false,
					  						IsRequired =  true,
					  						FullFieldLable =  "OpenAmountInInvoiceCurrency",
					  						DefaultText =  @"Open Amount",
					  						ListFieldLable =  "OpenAmountInInvoiceCurrencyListLable",
					  						ListLableDefaultText =  @"Open Amount",
					  						HelpTextCode =  "OpenAmountInInvoiceCurrency",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Journal Number",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Text",
					  						Code =  "Journal Number",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "JournalNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APInvoice",
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
					  						FullFieldLable =  "JournalNumber",
					  						DefaultText =  @"Journal Number",
					  						FullLocalDefaultText =  @"מספר פקודה",
					  						HelpTextCode =  "JournalNumber",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DraftGeneralAPInvoices",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Boolean",
					  						Code =  "DraftGeneralAPInvoices",
					  						MaxLength =  1,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "DraftGeneralAPInvoices",
					  						ListPropertyPath =  "DraftGeneralAPInvoices",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APInvoice",
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
					  						FullFieldLable =  "DraftGeneralAPInvoices",
					  						DefaultText =  @"Draft General Invoices",
					  						FullLocalDefaultText =  @"חשבוניות בסטטוס טיוטה",
					  						HelpTextCode =  "DraftGeneralAPInvoices",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ApprovalGeneralAPInvoices",
					  						ObjectTableName =  "APInvoice",
					  						FieldsDataType =  "Boolean",
					  						Code =  "ApprovalGeneralAPInvoices",
					  						MaxLength =  1,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "ApprovalGeneralAPInvoices",
					  						ListPropertyPath =  "ApprovalGeneralAPInvoices",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APInvoice",
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
					  						FullFieldLable =  "ApprovalGeneralAPInvoices",
					  						DefaultText =  @"Approval General APInvoices",
					  						FullLocalDefaultText =  @"חשבוניות מאושרות ",
					  						HelpTextCode =  "ApprovalGeneralAPInvoices",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	    {  
	        FeatureRepository featureRepository = new FeatureRepository(0); 
            List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
            IWebFreightContext objectContext = WebFreightContext.GetContext(0);  
	        QueryGroup APInvoiceQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "APIN", Name = "AP Invoices" }, queryGroupRepository);
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
 

			   TextCode APInvoiceTextCode_4 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.Q.WaitingApprovalAPInvoices", DefaultText = @"Waiting for Approval",LocalDefaultText = "ממתין לאישור", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature APInvoiceFeature_4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "WAITAPPROVAL", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.WaitingForApproval", NameTextCodeDefaultText = "Waiting for Approval", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode APInvoiceTextCode_5 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.Q.UnpaidAPInvoices", DefaultText = @"Unpaid Invoices",LocalDefaultText = "חשבוניות שלא שולמו", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature APInvoiceFeature_5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UNPAIDINVOICES", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.UnpaidInvoices", NameTextCodeDefaultText = "Unpaid Invoices", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode APInvoiceTextCode_6 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.Q.NotReadyInvoices", DefaultText = @"Not Ready Invoices",LocalDefaultText = "חשבוניות לא מוכנות", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature APInvoiceFeature_6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NOTREADYINVOICES", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.NotReadyInvoices", NameTextCodeDefaultText = "Not Ready Invoices", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode APInvoiceTextCode_7 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.Q.MarkedAsBlockedForTransfer", DefaultText = @"Marked as blocked for transfer",LocalDefaultText = "סמן כחסום להעברה", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature APInvoiceFeature_7 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MARKEDASBLOCKEDFORTRANSFER", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.MarkedAsBlockedForTransfer", NameTextCodeDefaultText = "Marked as blocked for transfer", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode APInvoiceTextCode_8 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.Q.ErrorInTransferInvoices", DefaultText = @"Error In Transfer Invoices",LocalDefaultText = "שגיאה בהעברת חשבוניות", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature APInvoiceFeature_8 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ERRORINTRANSFERINVOICES", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.ErrorInTransferInvoices", NameTextCodeDefaultText = "Error In Transfer Invoices", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

            TextCode APInvoiceTextCode_7 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.Q.ErrorInTransfer", DefaultText = @"Error In Transfer", LocalDefaultText = null, ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
            Feature APInvoiceFeature_7 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ErrorInTransfer", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.ErrorInTransfer", NameTextCodeDefaultText = "Error In Transfer", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);



            TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query AllGeneralAPInvoicesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = APInvoiceTextCode_0.Id, Code = "All General APInvoices",  QueryGroupCode = "APIN", IndexOrder = 0, Tenant = 0, ObjectTableId = APInvoiceObjectTable.Id, QuerySection = "APInvoice", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = APInvoiceFeature_0.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
	
			 QueryColumn AllGeneralAPInvoicesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGeneralAPInvoicesQuery.Id, IndexOrder = 0, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGeneralAPInvoicesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGeneralAPInvoicesQuery.Id, IndexOrder = 1, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGeneralAPInvoicesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGeneralAPInvoicesQuery.Id, IndexOrder = 2, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGeneralAPInvoicesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGeneralAPInvoicesQuery.Id, IndexOrder = 3, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGeneralAPInvoicesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGeneralAPInvoicesQuery.Id, IndexOrder = 4, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGeneralAPInvoicesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGeneralAPInvoicesQuery.Id, IndexOrder = 5, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGeneralAPInvoicesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGeneralAPInvoicesQuery.Id, IndexOrder = 6, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGeneralAPInvoicesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGeneralAPInvoicesQuery.Id, IndexOrder = 7, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "UpdateDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGeneralAPInvoicesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGeneralAPInvoicesQuery.Id, IndexOrder = 8, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "UpdatedByUserName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGeneralAPInvoicesQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGeneralAPInvoicesQuery.Id, IndexOrder = 9, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGeneralAPInvoicesQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGeneralAPInvoicesQuery.Id, IndexOrder = 10, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGeneralAPInvoicesQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGeneralAPInvoicesQuery.Id, IndexOrder = 11, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter AllGeneralAPInvoicesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "IsGeneralInvoice" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true",PredefinedValue2 = null, QueryId = AllGeneralAPInvoicesQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query DraftGeneralAPInvoicesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = APInvoiceTextCode_1.Id, Code = "Draft General APInvoices",  QueryGroupCode = "APIN", IndexOrder = 0, Tenant = 0, ObjectTableId = APInvoiceObjectTable.Id, QuerySection = "APInvoice", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = APInvoiceFeature_1.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
	
			 QueryColumn DraftGeneralAPInvoicesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftGeneralAPInvoicesQuery.Id, IndexOrder = 0, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftGeneralAPInvoicesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftGeneralAPInvoicesQuery.Id, IndexOrder = 1, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftGeneralAPInvoicesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftGeneralAPInvoicesQuery.Id, IndexOrder = 2, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftGeneralAPInvoicesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftGeneralAPInvoicesQuery.Id, IndexOrder = 3, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftGeneralAPInvoicesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftGeneralAPInvoicesQuery.Id, IndexOrder = 4, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftGeneralAPInvoicesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftGeneralAPInvoicesQuery.Id, IndexOrder = 5, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftGeneralAPInvoicesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftGeneralAPInvoicesQuery.Id, IndexOrder = 6, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftGeneralAPInvoicesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftGeneralAPInvoicesQuery.Id, IndexOrder = 7, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "UpdateDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftGeneralAPInvoicesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftGeneralAPInvoicesQuery.Id, IndexOrder = 8, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "UpdatedByUserName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftGeneralAPInvoicesQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftGeneralAPInvoicesQuery.Id, IndexOrder = 9, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftGeneralAPInvoicesQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftGeneralAPInvoicesQuery.Id, IndexOrder = 10, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftGeneralAPInvoicesQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftGeneralAPInvoicesQuery.Id, IndexOrder = 11, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter DraftGeneralAPInvoicesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "DraftGeneralAPInvoices" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true",PredefinedValue2 = null, QueryId = DraftGeneralAPInvoicesQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query ApprovalGeneralAPInvoicesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = APInvoiceTextCode_2.Id, Code = "Approval General APInvoices",  QueryGroupCode = "APIN", IndexOrder = 0, Tenant = 0, ObjectTableId = APInvoiceObjectTable.Id, QuerySection = "APInvoice", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = APInvoiceFeature_2.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
	
			 QueryColumn ApprovalGeneralAPInvoicesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovalGeneralAPInvoicesQuery.Id, IndexOrder = 0, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApprovalGeneralAPInvoicesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovalGeneralAPInvoicesQuery.Id, IndexOrder = 1, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApprovalGeneralAPInvoicesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovalGeneralAPInvoicesQuery.Id, IndexOrder = 2, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApprovalGeneralAPInvoicesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovalGeneralAPInvoicesQuery.Id, IndexOrder = 3, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApprovalGeneralAPInvoicesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovalGeneralAPInvoicesQuery.Id, IndexOrder = 4, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApprovalGeneralAPInvoicesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovalGeneralAPInvoicesQuery.Id, IndexOrder = 5, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApprovalGeneralAPInvoicesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovalGeneralAPInvoicesQuery.Id, IndexOrder = 6, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApprovalGeneralAPInvoicesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovalGeneralAPInvoicesQuery.Id, IndexOrder = 7, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "UpdateDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApprovalGeneralAPInvoicesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovalGeneralAPInvoicesQuery.Id, IndexOrder = 8, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "UpdatedByUserName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApprovalGeneralAPInvoicesQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovalGeneralAPInvoicesQuery.Id, IndexOrder = 9, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApprovalGeneralAPInvoicesQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovalGeneralAPInvoicesQuery.Id, IndexOrder = 10, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApprovalGeneralAPInvoicesQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApprovalGeneralAPInvoicesQuery.Id, IndexOrder = 11, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter ApprovalGeneralAPInvoicesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "ApprovalGeneralAPInvoices" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true",PredefinedValue2 = null, QueryId = ApprovalGeneralAPInvoicesQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query AllInvoicesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = APInvoiceTextCode_3.Id, Code = "All Invoices",  QueryGroupCode = "APIN", IndexOrder = 0, Tenant = 0, ObjectTableId = APInvoiceObjectTable.Id, QuerySection = "APInvoice", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = APInvoiceFeature_3.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
	
			 QueryColumn AllInvoicesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllInvoicesQuery.Id, IndexOrder = 0, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllInvoicesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllInvoicesQuery.Id, IndexOrder = 1, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllInvoicesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllInvoicesQuery.Id, IndexOrder = 2, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllInvoicesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllInvoicesQuery.Id, IndexOrder = 3, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllInvoicesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllInvoicesQuery.Id, IndexOrder = 4, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllInvoicesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllInvoicesQuery.Id, IndexOrder = 5, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllInvoicesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllInvoicesQuery.Id, IndexOrder = 6, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllInvoicesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllInvoicesQuery.Id, IndexOrder = 7, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "UpdateDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllInvoicesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllInvoicesQuery.Id, IndexOrder = 8, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "UpdatedByUserName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllInvoicesQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllInvoicesQuery.Id, IndexOrder = 9, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllInvoicesQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllInvoicesQuery.Id, IndexOrder = 10, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllInvoicesQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllInvoicesQuery.Id, IndexOrder = 11, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
  
	      

			  Query WaitingforApprovalQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = APInvoiceTextCode_4.Id, Code = "Waiting for Approval",  QueryGroupCode = "APIN", IndexOrder = 1, Tenant = 0, ObjectTableId = APInvoiceObjectTable.Id, QuerySection = "APInvoice", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = APInvoiceFeature_4.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
	
			 QueryColumn WaitingforApprovalQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = WaitingforApprovalQuery.Id, IndexOrder = 0, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn WaitingforApprovalQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = WaitingforApprovalQuery.Id, IndexOrder = 1, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn WaitingforApprovalQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = WaitingforApprovalQuery.Id, IndexOrder = 2, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn WaitingforApprovalQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = WaitingforApprovalQuery.Id, IndexOrder = 3, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn WaitingforApprovalQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = WaitingforApprovalQuery.Id, IndexOrder = 4, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn WaitingforApprovalQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = WaitingforApprovalQuery.Id, IndexOrder = 5, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn WaitingforApprovalQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = WaitingforApprovalQuery.Id, IndexOrder = 6, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn WaitingforApprovalQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = WaitingforApprovalQuery.Id, IndexOrder = 7, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn WaitingforApprovalQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = WaitingforApprovalQuery.Id, IndexOrder = 8, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InternalNumber" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter WaitingforApprovalQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "StatusCode" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "WA",PredefinedValue2 = null, QueryId = WaitingforApprovalQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query UnpaidInvoicesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = APInvoiceTextCode_5.Id, Code = "Unpaid Invoices",  QueryGroupCode = "APIN", IndexOrder = 2, Tenant = 0, ObjectTableId = APInvoiceObjectTable.Id, QuerySection = "APInvoice", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = APInvoiceFeature_5.Id, DefaultSortName = "DueDate", DefaultSortDirection = "Ascending" }, queriesRepository, tenantQueries);
	
			 QueryColumn UnpaidInvoicesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnpaidInvoicesQuery.Id, IndexOrder = 0, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn UnpaidInvoicesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnpaidInvoicesQuery.Id, IndexOrder = 1, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn UnpaidInvoicesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnpaidInvoicesQuery.Id, IndexOrder = 2, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn UnpaidInvoicesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnpaidInvoicesQuery.Id, IndexOrder = 3, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "DueDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn UnpaidInvoicesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnpaidInvoicesQuery.Id, IndexOrder = 4, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceCurrencyCode" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn UnpaidInvoicesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnpaidInvoicesQuery.Id, IndexOrder = 5, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn UnpaidInvoicesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnpaidInvoicesQuery.Id, IndexOrder = 6, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "AmountDue" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn UnpaidInvoicesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnpaidInvoicesQuery.Id, IndexOrder = 7, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn UnpaidInvoicesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = UnpaidInvoicesQuery.Id, IndexOrder = 8, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InternalNumber" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter UnpaidInvoicesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "UnpaidInvoices" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true",PredefinedValue2 = null, QueryId = UnpaidInvoicesQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query NotReadyInvoicesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = APInvoiceTextCode_6.Id, Code = "Not Ready Invoices",  EditWizardName = "Simplog.InvoiceLib.Views.Tabs.APInvoiceTabs.APTransferEditControl",
			   EditWizardComponentPath = "./InvoiceModules/APInvoice/Components/NewEntity/APInvoiceTransferTemplate",
			   QueryGroupCode = "APIN", IndexOrder = 3, Tenant = 0, ObjectTableId = APInvoiceObjectTable.Id, QuerySection = "APInvoice", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = APInvoiceFeature_6.Id, DefaultSortName = "InvoiceDate", DefaultSortDirection = "Ascending" }, queriesRepository, tenantQueries);
	
			 QueryColumn NotReadyInvoicesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotReadyInvoicesQuery.Id, IndexOrder = 0, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn NotReadyInvoicesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotReadyInvoicesQuery.Id, IndexOrder = 1, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn NotReadyInvoicesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotReadyInvoicesQuery.Id, IndexOrder = 2, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 190 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn NotReadyInvoicesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotReadyInvoicesQuery.Id, IndexOrder = 3, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn NotReadyInvoicesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotReadyInvoicesQuery.Id, IndexOrder = 4, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn NotReadyInvoicesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotReadyInvoicesQuery.Id, IndexOrder = 5, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "ReadyForTransfer" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 50 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn NotReadyInvoicesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotReadyInvoicesQuery.Id, IndexOrder = 6, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "TransferError" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 500 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter NotReadyInvoicesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "NotReadyInvoices" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true",PredefinedValue2 = null, QueryId = NotReadyInvoicesQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query MarkedasblockedfortransferQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = APInvoiceTextCode_7.Id, Code = "Marked as blocked for transfer",  EditWizardName = "Simplog.InvoiceLib.Views.Tabs.APInvoiceTabs.APTransferEditControl",
			   EditWizardComponentPath = "./InvoiceModules/APInvoice/Components/NewEntity/APInvoiceTransferTemplate",
			   QueryGroupCode = "APIN", IndexOrder = 4, Tenant = 0, ObjectTableId = APInvoiceObjectTable.Id, QuerySection = "APInvoice", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = APInvoiceFeature_7.Id, DefaultSortName = "InvoiceDate", DefaultSortDirection = "Ascending" }, queriesRepository, tenantQueries);
	
			 QueryColumn MarkedasblockedfortransferQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MarkedasblockedfortransferQuery.Id, IndexOrder = 0, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MarkedasblockedfortransferQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MarkedasblockedfortransferQuery.Id, IndexOrder = 1, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MarkedasblockedfortransferQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MarkedasblockedfortransferQuery.Id, IndexOrder = 2, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 200 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MarkedasblockedfortransferQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MarkedasblockedfortransferQuery.Id, IndexOrder = 3, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MarkedasblockedfortransferQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MarkedasblockedfortransferQuery.Id, IndexOrder = 4, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter MarkedasblockedfortransferQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "MarkedAsBlockedForTransfer" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true",PredefinedValue2 = null, QueryId = MarkedasblockedfortransferQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query ErrorInTransferInvoicesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = APInvoiceTextCode_8.Id, Code = "Error In Transfer Invoices",  EditWizardName = "Simplog.InvoiceLib.Views.Tabs.APInvoiceTabs.APTransferEditControl",
			   EditWizardComponentPath = "./InvoiceModules/APInvoice/Components/NewEntity/APInvoiceTransferTemplate",
			   QueryGroupCode = "APIN", IndexOrder = 5, Tenant = 0, ObjectTableId = APInvoiceObjectTable.Id, QuerySection = "APInvoice", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = APInvoiceFeature_8.Id, DefaultSortName = "InvoiceDate", DefaultSortDirection = "Ascending" }, queriesRepository, tenantQueries);
	
			 QueryColumn ErrorInTransferInvoicesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ErrorInTransferInvoicesQuery.Id, IndexOrder = 0, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceDate" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ErrorInTransferInvoicesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ErrorInTransferInvoicesQuery.Id, IndexOrder = 1, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "InvoiceNumber" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ErrorInTransferInvoicesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ErrorInTransferInvoicesQuery.Id, IndexOrder = 2, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 190 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ErrorInTransferInvoicesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ErrorInTransferInvoicesQuery.Id, IndexOrder = 3, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ErrorInTransferInvoicesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ErrorInTransferInvoicesQuery.Id, IndexOrder = 4, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "AmountInInvoiceCurrency" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ErrorInTransferInvoicesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ErrorInTransferInvoicesQuery.Id, IndexOrder = 5, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "ReadyForTransfer" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 50 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ErrorInTransferInvoicesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ErrorInTransferInvoicesQuery.Id, IndexOrder = 6, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "TransferError" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 500 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter ErrorInTransferInvoicesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = APInvoiceObjectFields.Where(d => d.FieldName == "ErrorInTransferInvoices" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true",PredefinedValue2 = null, QueryId = ErrorInTransferInvoicesQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

	   
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
	   		  

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {    
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
			   Feature APInvoiceTransferDetailsFeature_TH4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACCOUNTINGTRANSFER", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.AccountingTransfer", NameTextCodeDefaultText = "Accounting Transfer", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode APInvoicePaymentsTextCode_TH5 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.TH.APPayments", DefaultText = "Payments",LocalDefaultText = "תשלומים", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature APInvoicePaymentsFeature_TH5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "APPAYMENTS", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.APPayments", NameTextCodeDefaultText = "Payments", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode APInvoiceCommunicationTextCode_TH6 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.TH.Communications", DefaultText = "Communication",LocalDefaultText = "תקשורת", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature APInvoiceCommunicationFeature_TH6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "COMMUNICATION", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.Communication", NameTextCodeDefaultText = "Communication", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode APInvoiceEventsTextCode_TH7 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.TH.Events", DefaultText = "Events",LocalDefaultText = "אירועים", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature APInvoiceEventsFeature_TH7 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EVENTS", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "PIDE",HtmlComponentName = "",HtmlComponentUrl = "./InvoiceModules/APInvoice/Components/EditTabs/APInvoiceDetailsTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "DETAILS" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.InvoiceLib.Views.Tabs.APInvoiceTabs.APInvoiceDetailsTabControl", ObjectTableId = APInvoiceObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "APInvoice.TH.Details" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "PIGE",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = tenantFeatures.Where(d => d.Code == "GENERAL" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.Infrastructure.GeneralControls.GeneralTabControl", ObjectTableId = APInvoiceObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "APInvoice.TH.General" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "PIDO",HtmlComponentName = "",HtmlComponentUrl = "./InvoiceModules/APInvoice/Components/EditTabs/APInvoiceDocsOutTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "DOCSOUT" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.InvoiceLib.Views.Tabs.APInvoiceTabs.APInvoiceDocsOutControl", ObjectTableId = APInvoiceObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "APInvoice.TH.DocsOut" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 2 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "PIDI",HtmlComponentName = "",HtmlComponentUrl = "./InvoiceModules/APInvoice/Components/EditTabs/APInvoiceDocsInTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "DOCSIN" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.InvoiceLib.Views.Tabs.APInvoiceTabs.APInvoiceDocsInControl", ObjectTableId = APInvoiceObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "APInvoice.TH.DocsIn" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 3 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "PIAC",HtmlComponentName = "",HtmlComponentUrl = "./InvoiceModules/APInvoice/Components/EditTabs/APInvoiceTransferTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "ACCOUNTINGTRANSFER" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.InvoiceLib.Views.Tabs.APInvoiceTabs.APTransferTabControl", ObjectTableId = APInvoiceObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "APInvoice.TH.Transfer" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 4 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "PIPY",HtmlComponentName = "",HtmlComponentUrl = "./InvoiceModules/APInvoice/Components/EditTabs/APInvoicePaymentsTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "APPAYMENTS" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.InvoiceLib.Views.Tabs.APInvoiceTabs.APPaymentsTabControl", ObjectTableId = APInvoiceObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "APInvoice.TH.APPayments" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 5 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "APCM",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = tenantFeatures.Where(d => d.Code == "COMMUNICATION" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.Infrastructure.Views.Communications.CommunicationsControl", ObjectTableId = APInvoiceObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "APInvoice.TH.Communications" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 6 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "APIE",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = tenantFeatures.Where(d => d.Code == "EVENTS" && d.ObjectTableId == APInvoiceObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = APInvoiceObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "APInvoice.TH.Events" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 7 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable APInvoiceObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "APInvoice" && d.Tenant == 0).FirstOrDefault(); 
		   Feature APInvoiceFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature APInvoiceFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature APInvoiceFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature APInvoiceFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.PackageFeature", NameTextCodeDefaultText = "APInvoice Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes); 

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
                ShortView =  false,
                IsManualEntry =  false,
                LocalName =  "Invoice Updated",
                EnglishName =  "Invoice Updated",
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
                ObjectTableId = APInvoiceObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "APIA",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Approved",
                EnglishName =  "Approved",
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
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Canceled",
                EnglishName =  "Canceled",
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
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Voided",
                EnglishName =  "Voided",
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
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Payment Connected",
                EnglishName =  "Payment Connected",
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
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Payment Disconnected",
                EnglishName =  "Payment Disconnected",
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
		   List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList(); 
		   ObjectTable APInvoiceObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "APInvoice" && d.Tenant == 0).FirstOrDefault();       
    
			   Feature APInvoiceFeature_MB0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SaveAPInvoice", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, NameTextCodeCode = "APInvoice.Features.Save", NameTextCodeDefaultText = "Save", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

      
    
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
						LocalDefaultText = "Send to QBO",
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
						LocalDefaultText = null,
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

 		   TextCode APInvoiceTextCode_APInvoice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice", DefaultText = "A/P Invoice",LocalDefaultText = null, ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "T", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APInvoiceTextCode_APInvoiceCHAPInvoiceTypeNameListLable = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.CH.APInvoiceTypeNameListLable", DefaultText = "Type",LocalDefaultText = null, ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APInvoiceTextCode_APInvoiceCHOtherPayments = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.CH.OtherPayments", DefaultText = "Other Payments",LocalDefaultText = @"קבלות אחרות", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APInvoiceTextCode_APInvoiceCHAmountToPay = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APInvoice.CH.AmountToPay", DefaultText = "Amount To Pay",LocalDefaultText = @"סכום לתשלום", ObjectTableId = APInvoiceObjectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

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

   
	    
}

    

   }
    
}
	 