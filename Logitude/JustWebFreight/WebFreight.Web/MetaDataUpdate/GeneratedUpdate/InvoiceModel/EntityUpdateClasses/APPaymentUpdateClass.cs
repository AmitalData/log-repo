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
   public class APPaymentUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "APPayment",
			      				    DBTableName =  "APPayments",
			      				    ObjectTableSingular =  "A/P Payment",
			      				    ObjectTablePlural =  "A/P Payments",
			      				    HasCustomFilter =  true,
			      				    HasCustomFields =  false,
			      				    HasHelper =  true,
			      				    HasShortTitle =  true,
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
			      				    SortingByObjectField =  "PaymentNo",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "BR",
			      				    MaxNumberOfCustomFields =  0,
			      				    DefaultText =  "A/P Payment",
			      				    Code =  "APPY",
			      				    Name =  "AP Payments",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Invoice",
			      				    NoTS =  false,
			      				    HasMenuButtons =  true,
			      				    CustomFieldsCount =  0,
			      				    SearchFields =  "APPayment,APPayments,,Id,",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PaymentNo",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  20,
					  						IsRequired =  false,
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
					  						PMPropertyPath =  "PaymentNo",
					  						ListPropertyPath =  "PaymentNo",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PaymentNo",
					  						DefaultText =  @"Payment No.",
					  						ListFieldLable =  "PaymentNoListLable",
					  						ListLableDefaultText =  @"#",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "PaymentNo",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  true,
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"מספר קבלה",
					  						ListLocalDefaultText =  @"מספר קבלה",
					  						HelpTextCode =  "PaymentNo",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreateDate",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  true,
					  						DisplayOnLookUp =  true,
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
					  						ValidForQuerySection1 =  "APPayment",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CreateDate",
					  						DefaultText =  @"Create Date",
					  						ListFieldLable =  "CreateDateListLable",
					  						ListLableDefaultText =  @"Create Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "CreateDate",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"תאריך יצירה",
					  						ListLocalDefaultText =  @"תאריך יצירה",
					  						HelpTextCode =  "CreateDate",
					  						HelpLocalDefaultText =  @"תאריך יצירה",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreatedByUserId",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
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
					  						PMPropertyPath =  "CreatedByUserId",
					  						ListPropertyPath =  "CreatedByUserId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CreatedByUserId",
					  						DefaultText =  @"Created By",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "CreatedByUserId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"נוצר ע''י",
					  						HelpTextCode =  "CreatedByUserId",
					  						HelpLocalDefaultText =  @"נוצר ע''י",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreatedByUserName",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  60,
					  						IsRequired =  false,
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
					  						PMPropertyPath =  "CreatedByUserName",
					  						ListPropertyPath =  "CreatedByUserName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CreatedByUserName",
					  						DefaultText =  @"Created By",
					  						ListFieldLable =  "CreatedByUserNameListLable",
					  						ListLableDefaultText =  @"Created By",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "CreatedByUserName",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"נוצר ע''י",
					  						ListLocalDefaultText =  @"נוצר ע''י",
					  						HelpTextCode =  "CreatedByUserName",
					  						HelpLocalDefaultText =  @"נוצר ע''י",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PrintDate",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  true,
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
					  						PMPropertyPath =  "PrintDate",
					  						ListPropertyPath =  "PrintDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PrintDate",
					  						DefaultText =  @"Print Date",
					  						ListFieldLable =  "PrintDateListLable",
					  						ListLableDefaultText =  @"Print Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "PrintDate",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"תאריך הדפסה",
					  						ListLocalDefaultText =  @"תאריך הדפסה",
					  						HelpTextCode =  "PrintDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PrintByUserId",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
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
					  						PMPropertyPath =  "PrintByUserId",
					  						ListPropertyPath =  "PrintByUserId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PrintByUserId",
					  						DefaultText =  @"Print By User",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "PrintByUserId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"הודפס ע''י",
					  						HelpTextCode =  "PrintByUserId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LocalCurrencyId",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Currency",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
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
					  						PMPropertyPath =  "LocalCurrencyId",
					  						ListPropertyPath =  "LocalCurrencyId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LocalCurrencyId",
					  						DefaultText =  @"Local Currency",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "LocalCurrencyId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"מטבע מקומי",
					  						HelpTextCode =  "LocalCurrencyId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AmountInLocalCurrency",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "Double",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  true,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						DataTemplateName =  "APPaymentAmountInLocalCurrencyDataTemplate",
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
					  						ValidForQuerySection1 =  "APPayment",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AmountInLocalCurrency",
					  						DefaultText =  @"Amount (Local Currency)",
					  						ListFieldLable =  "AmountInLocalCurrencyListLable",
					  						ListLableDefaultText =  @"Amount (Local Currency)",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						Code =  "AmountInLocalCurrency",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"סכום(במטבע מקומי)",
					  						ListLocalDefaultText =  @"סכום(במטבע מקומי)",
					  						HelpTextCode =  "AmountInLocalCurrency",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "VendorId",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Card",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
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
					  						PMPropertyPath =  "VendorId",
					  						ListPropertyPath =  "VendorId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "VendorId",
					  						DefaultText =  @"Vendor",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "VendorId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"ספק",
					  						HelpTextCode =  "VendorId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "VendorName",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  70,
					  						IsRequired =  false,
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
					  						PMPropertyPath =  "VendorName",
					  						ListPropertyPath =  "VendorName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "VendorName",
					  						DefaultText =  @"Vendor",
					  						ListFieldLable =  "VendorNameListLable",
					  						ListLableDefaultText =  @"Vendor",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "VendorName",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"ספק",
					  						ListLocalDefaultText =  @"ספק",
					  						HelpTextCode =  "VendorName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "StatusCode",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "APPaymentStatus",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  false,
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
					  						PMPropertyPath =  "StatusCode",
					  						ListPropertyPath =  "StatusCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "StatusCode",
					  						DefaultText =  @"Status",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "StatusCode",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"סטטוס",
					  						HelpTextCode =  "StatusCode",
					  						HelpLocalDefaultText =  @"סטטוס",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "StatusName",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
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
					  						PMPropertyPath =  "StatusName",
					  						ListPropertyPath =  "StatusName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "StatusName",
					  						DefaultText =  @"Status",
					  						ListFieldLable =  "StatusNameListLable",
					  						ListLableDefaultText =  @"Status",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "StatusName",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"סטטוס",
					  						ListLocalDefaultText =  @"סטטוס",
					  						HelpTextCode =  "StatusName",
					  						HelpLocalDefaultText =  @"סטטוס",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsClosed",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						DataTemplateName =  "APPaymentIsClosedPathTemplate",
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
					  						ValidForQuerySection1 =  "APPayment",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsClosed",
					  						DefaultText =  @"Is Closed",
					  						ListFieldLable =  "IsClosedListLable",
					  						ListLableDefaultText =  @"Is Closed",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "IsClosed",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"סגור",
					  						ListLocalDefaultText =  @"סגור",
					  						HelpTextCode =  "IsClosed",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PaymentCurrencyId",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Currency",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
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
					  						PMPropertyPath =  "PaymentCurrencyId",
					  						ListPropertyPath =  "PaymentCurrencyId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PaymentCurrencyId",
					  						DefaultText =  @"Payment Currency",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "PaymentCurrencyId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"מטבע התשלום",
					  						HelpTextCode =  "PaymentCurrencyId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PaymentCurrencyCode",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
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
					  						PMPropertyPath =  "PaymentCurrencyCode",
					  						ListPropertyPath =  "PaymentCurrencyCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PaymentCurrencyCode",
					  						DefaultText =  @"Currency",
					  						ListFieldLable =  "PaymentCurrencyCodeListLable",
					  						ListLableDefaultText =  @"Currency",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "PaymentCurrencyCode",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"מטבע",
					  						ListLocalDefaultText =  @"מטבע",
					  						HelpTextCode =  "PaymentCurrencyCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AmountInPaymentCurrency",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "Double",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  true,
					  						DisplayOnLookUp =  true,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						DataTemplateName =  "AmountInPaymentCurrencyDataTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AmountInPaymentCurrency",
					  						ListPropertyPath =  "AmountInPaymentCurrency",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AmountInPaymentCurrency",
					  						DefaultText =  @"Payment Amount",
					  						ListFieldLable =  "AmountInPaymentCurrencyListLable",
					  						ListLableDefaultText =  @"Payment Amount",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						Code =  "AmountInPaymentCurrency",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"סכום במטבע התשלום",
					  						ListLocalDefaultText =  @"סכום במטבע התשלום",
					  						HelpTextCode =  "AmountInPaymentCurrency",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AccountingPaymentMethodCode",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "Text",
					  						LookUpTableName =  "AccountingPaymentMethod",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  false,
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
					  						PMPropertyPath =  "AccountingPaymentMethodCode",
					  						ListPropertyPath =  "AccountingPaymentMethodCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AccountingPaymentMethodCode",
					  						DefaultText =  @"Payment Method",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "AccountingPaymentMethodCode",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"אמצעי תשלום",
					  						HelpTextCode =  "AccountingPaymentMethodCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PaymentMethodName",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
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
					  						PMPropertyPath =  "PaymentMethodName",
					  						ListPropertyPath =  "PaymentMethodName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PaymentMethodName",
					  						DefaultText =  @"Payment Method",
					  						ListFieldLable =  "PaymentMethodNameListLable",
					  						ListLableDefaultText =  @"Payment Method",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "PaymentMethodName",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"אמצעי תשלום",
					  						ListLocalDefaultText =  @"אמצעי תשלום",
					  						HelpTextCode =  "PaymentMethodName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PrintNotes",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  500,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  true,
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
					  						PMPropertyPath =  "PrintNotes",
					  						ListPropertyPath =  "PrintNotes",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PrintNotes",
					  						DefaultText =  @"Print Notes",
					  						ListFieldLable =  "PrintNotesListLable",
					  						ListLableDefaultText =  @"Print Notes",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "PrintNotes",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"הערות הדפסה",
					  						ListLocalDefaultText =  @"הערות הדפסה",
					  						HelpTextCode =  "PrintNotes",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InternalNotes",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  250,
					  						IsRequired =  false,
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
					  						PMPropertyPath =  "InternalNotes",
					  						ListPropertyPath =  "InternalNotes",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "InternalNotes",
					  						DefaultText =  @"Notes",
					  						ListFieldLable =  "InternalNotesListLable",
					  						ListLableDefaultText =  @"Notes",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "InternalNotes",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"אין סכומים שליליים פתוחים",
					  						ListLocalDefaultText =  @"אין סכומים שליליים פתוחים",
					  						HelpTextCode =  "InternalNotes",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PaymentCurrencyExchangeRate",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "Double",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  true,
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
					  						PMPropertyPath =  "PaymentCurrencyExchangeRate",
					  						ListPropertyPath =  "PaymentCurrencyExchangeRate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PaymentCurrencyExchangeRate",
					  						DefaultText =  @"Exchange Rate",
					  						ListFieldLable =  "PaymentCurrencyExchangeRateListLable",
					  						ListLableDefaultText =  @"Exchange Rate",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "PaymentCurrencyExchangeRate",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"שער חליפין",
					  						ListLocalDefaultText =  @"שער חליפין",
					  						HelpTextCode =  "PaymentCurrencyExchangeRate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PaymentCurrencyExchangeRateDate",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  true,
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
					  						PMPropertyPath =  "PaymentCurrencyExchangeRateDate",
					  						ListPropertyPath =  "PaymentCurrencyExchangeRateDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PaymentCurrencyExchangeRateDate",
					  						DefaultText =  @"Exchange Date",
					  						ListFieldLable =  "PaymentCurrencyExchangeRateDateListLable",
					  						ListLableDefaultText =  @"Exchange Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "PaymentCurrencyExchangeRateDate",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"תאריך שער חליפין",
					  						ListLocalDefaultText =  @"תאריך שער חליפין",
					  						HelpTextCode =  "PaymentCurrencyExchangeRateDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "VendorAddressId",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Address",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
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
					  						PMPropertyPath =  "VendorAddressId",
					  						ListPropertyPath =  "VendorAddressId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "VendorAddressId",
					  						DefaultText =  @"Vendor Address",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "VendorAddressId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"כתובת ספק",
					  						HelpTextCode =  "VendorAddressId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "OpenAmount",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "Double",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  true,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						DataTemplateName =  "PaymentOpenAmountDataTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "OpenAmount",
					  						ListPropertyPath =  "OpenAmount",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "OpenAmount",
					  						DefaultText =  @"Open Amount",
					  						ListFieldLable =  "OpenAmountListLable",
					  						ListLableDefaultText =  @"Open Amount",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						Code =  "OpenAmount",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"סה''כ סכום שטרם הותאם",
					  						ListLocalDefaultText =  @"סה''כ סכום שטרם הותאם",
					  						HelpTextCode =  "OpenAmount",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ValueDate",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  true,
					  						DisplayOnLookUp =  true,
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
					  						PMPropertyPath =  "ValueDate",
					  						ListPropertyPath =  "ValueDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ValueDate",
					  						DefaultText =  @"Value Date",
					  						ListFieldLable =  "ValueDateListLable",
					  						ListLableDefaultText =  @"Value Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "ValueDate",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"תאריך ערך",
					  						ListLocalDefaultText =  @"תאריך ערך",
					  						HelpTextCode =  "ValueDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Bank",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  10,
					  						IsRequired =  false,
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
					  						PMPropertyPath =  "Bank",
					  						ListPropertyPath =  "Bank",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Bank",
					  						DefaultText =  @"Bank",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "Bank",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"בנק",
					  						HelpTextCode =  "Bank",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BankBranch",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  10,
					  						IsRequired =  false,
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
					  						PMPropertyPath =  "BankBranch",
					  						ListPropertyPath =  "BankBranch",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "BankBranch",
					  						DefaultText =  @"Bank Branch",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "BankBranch",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"סניף",
					  						HelpTextCode =  "BankBranch",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SearchFields",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  20000,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  20000,
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
					  						ValidForQuerySection1 =  "APPayment",
					  						ValidForQuerySection2 =  "APPaymentFollowUp",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SearchFields",
					  						DefaultText =  @"Search Payment # / Vendor / Reference",
					  						HelpTextCode =  "SearchFields",
					  						HelpTextDefaultText =  @"Searching by :\n1:Payment # \n2:Vendor \n3:Reference",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "SearchFields",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"חיפוש לפי מספר תשלום\ספק",
					  						HelpLocalDefaultText =  @"חיפוש לפי מספר תשלום\ספק",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "OpenPayments",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
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
					  						PMPropertyPath =  "OpenPayments",
					  						ListPropertyPath =  "OpenPayments",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "OpenPayments",
					  						DefaultText =  @"OpenPayments",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "OpenPayments",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"קבלות פתוחות",
					  						HelpTextCode =  "OpenPayments",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DraftPayments",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
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
					  						PMPropertyPath =  "DraftPayments",
					  						ListPropertyPath =  "DraftPayments",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DraftPayments",
					  						DefaultText =  @"DraftPayments",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "DraftPayments",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"טיוטת חשבונית ספק",
					  						HelpTextCode =  "DraftPayments",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BranchId",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Branch",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  true,
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
					  						PMPropertyPath =  "BranchId",
					  						ListPropertyPath =  "BranchId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "BranchId",
					  						DefaultText =  @"Branch",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "BranchId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"סניף",
					  						HelpTextCode =  "BranchId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ChequeOrPaymentRef",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  30,
					  						IsRequired =  false,
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
					  						PMPropertyPath =  "ChequeOrPaymentRef",
					  						ListPropertyPath =  "ChequeOrPaymentRef",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ChequeOrPaymentRef",
					  						DefaultText =  @"Cheque Or Payment Ref",
					  						ListFieldLable =  "ChequeOrPaymentRefListLable",
					  						ListLableDefaultText =  @"Cheque Or Payment Ref",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "ChequeOrPaymentRef",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"מספר המחאה",
					  						ListLocalDefaultText =  @"מספר המחאה",
					  						HelpTextCode =  "ChequeOrPaymentRef",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "RegisterDate",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  true,
					  						DisplayOnLookUp =  true,
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
					  						PMPropertyPath =  "RegisterDate",
					  						ListPropertyPath =  "RegisterDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "RegisterDate",
					  						DefaultText =  @"Register Date",
					  						ListFieldLable =  "RegisterDateListLable",
					  						ListLableDefaultText =  @"Register Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "RegisterDate",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"תאריך רישום",
					  						ListLocalDefaultText =  @"תאריך רישום",
					  						HelpTextCode =  "RegisterDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreditCardTypeId",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "CreditCardType",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
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
					  						PMPropertyPath =  "CreditCardTypeId",
					  						ListPropertyPath =  "CreditCardTypeId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CreditCardTypeId",
					  						DefaultText =  @"Credit Card Type",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "CreditCardTypeId",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"סוג כרטיס אשראי",
					  						HelpTextCode =  "CreditCardTypeId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LocalCurrencyCode",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  3,
					  						IsRequired =  false,
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
					  						PMPropertyPath =  "LocalCurrencyCode",
					  						ListPropertyPath =  "LocalCurrencyCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LocalCurrencyCode",
					  						DefaultText =  @"Local Currency",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "LocalCurrencyCode",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"מטבע מקומי",
					  						HelpTextCode =  "LocalCurrencyCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Account",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  20,
					  						IsRequired =  false,
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
					  						PMPropertyPath =  "Account",
					  						ListPropertyPath =  "Account",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
					  						DisplayInEntityVariables =  true,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Account",
					  						DefaultText =  @"Account",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "Account",
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"חשבון",
					  						ListFieldLable =  "ARAccountNameListLable",
					  						ListLableDefaultText =  @"Account",
					  						HelpTextCode =  "Account",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SATApprovalDate",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
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
					  						PMPropertyPath =  "SATApprovalDate",
					  						ListPropertyPath =  "SATApprovalDate",
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SATApprovalDate",
					  						DefaultText =  @"SAT Approval Date",
					  						ListFieldLable =  "SATApprovalDateListLable",
					  						ListLableDefaultText =  @"SAT Approval Date",
					  						ListLocalDefaultText =  @"SAT Approval Date",
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
					 
					 						FieldName =  "PaymentMethodCode",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "Text",
					  						Code =  "PaymentMethodCode",
					  						MaxLength =  2,
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
					  						PMPropertyPath =  "PaymentMethodCode",
					  						ListPropertyPath =  "PaymentMethodCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
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
					  						FullFieldLable =  "PaymentMethodCode",
					  						DefaultText =  @"Payment Method",
					  						FullLocalDefaultText =  @"אמצעי תשלום",
					  						HelpTextCode =  "PaymentMethodCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ARAccountName",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "Text",
					  						Code =  "ARAccountName",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "CheckOrPaymentRef",
					  						ListPropertyPath =  "CheckOrPaymentRef",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
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
					  						FullFieldLable =  "CheckOrPaymentRef",
					  						DefaultText =  @"Check Or Payment Ref",
					  						ListFieldLable =  "CheckOrPaymentRefListLable",
					  						ListLableDefaultText =  @"Check Or Payment Ref",
					  						HelpTextCode =  "CheckOrPaymentRef",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BankBankBankBranch",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "Text",
					  						Code =  "BankBankBankBranch",
					  						MaxLength =  10,
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
					  						PMPropertyPath =  "Branch",
					  						ListPropertyPath =  "Branch",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
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
					  						FullFieldLable =  "BankBankBankBranch",
					  						DefaultText =  @"BankBankBankBranch",
					  						HelpTextCode =  "BankBankBankBranch",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TransferError",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "Text",
					  						Code =  "TransferError",
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
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
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
					  						DataTemplateName =  "TransferErrorDataTemplate",
					  						HasTemplate =  true,
					  						IsRequired =  false,
					  						FullFieldLable =  "TransferError",
					  						DefaultText =  @"Transfer Error",
					  						FullLocalDefaultText =  @"שגיאה בהעברה",
					  						ListFieldLable =  "TransferErrorListLable",
					  						ListLableDefaultText =  @"Transfer Error",
					  						ListLocalDefaultText =  @"שגיאה בהעברה",
					  						HelpTextCode =  "TransferError",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TransferStatusCode",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "APPaymentTransferStatus",
					  						Code =  "TransferStatusCode",
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
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
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
					  						FullFieldLable =  "TransferStatusCode",
					  						DefaultText =  @"Transfer Status",
					  						FullLocalDefaultText =  @"סטאטוס העברה",
					  						HelpTextCode =  "TransferStatusCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TransferStatusName",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "Text",
					  						Code =  "TransferStatusName",
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
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
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
					  						FullFieldLable =  "TransferStatusName",
					  						DefaultText =  @"Transfer Status",
					  						FullLocalDefaultText =  @"סטאטוס העברה",
					  						ListFieldLable =  "TransferStatusName",
					  						ListLableDefaultText =  @"Transfer Status",
					  						ListLocalDefaultText =  @"סטאטוס העברה",
					  						HelpTextCode =  "TransferStatusName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ReadyForTransfer",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "Boolean",
					  						Code =  "ReadyForTransfer",
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
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
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
					  						DataTemplateName =  "ReadyForTransferDataTemplate",
					  						HasTemplate =  true,
					  						IsRequired =  false,
					  						FullFieldLable =  "ReadyForTransfer",
					  						DefaultText =  @"Ready For Transfer",
					  						FullLocalDefaultText =  @"סכום במטבע רווח",
					  						ListFieldLable =  "ReadyForTransfer",
					  						ListLableDefaultText =  @"Ready",
					  						ListLocalDefaultText =  @"סכום במטבע רווח",
					  						HelpTextCode =  "ReadyForTransfer",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "NotReadyPayments",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "Boolean",
					  						Code =  "NotReadyPayments",
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
					  						PMPropertyPath =  "NotReadyPayments",
					  						ListPropertyPath =  "NotReadyPayments",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
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
					  						FullFieldLable =  "NotReadyPayments",
					  						DefaultText =  @"Not Ready Payments",
					  						FullLocalDefaultText =  @"קבלות לא מוכנות",
					  						HelpTextCode =  "NotReadyPayments",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "MarkedAsBlockedForTransfer",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "Boolean",
					  						Code =  "MarkedAsBlockedForTransfer",
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
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
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
					  						FullFieldLable =  "MarkedAsBlockedForTransfer",
					  						DefaultText =  @"Marked as blocked for transfer",
					  						FullLocalDefaultText =  @"סמן כחסום להעברה",
					  						HelpTextCode =  "MarkedAsBlockedForTransfer",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ApprovedByUserId",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
					  						Code =  "ApprovedByUserId",
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
					  						Operator =  "StartsWith",
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
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
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
					  						FullFieldLable =  "ApprovedByUserId",
					  						DefaultText =  @"Approved By User",
					  						FullLocalDefaultText =  @"משתמש מאשר",
					  						HelpTextCode =  "ApprovedByUserId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ApprovedDateTime",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "DateTime",
					  						Code =  "ApprovedDateTime",
					  						MaxLength =  0,
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
					  						PMPropertyPath =  "ApprovedDateTime",
					  						ListPropertyPath =  "ApprovedDateTime",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
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
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "ApprovedDateTime",
					  						DefaultText =  @"Approved Date",
					  						FullLocalDefaultText =  @"תאריך אישור",
					  						HelpTextCode =  "ApprovedDateTime",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TaxDeductionPercentage",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "Integer",
					  						Code =  "TaxDeductionPercentage",
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
					  						PMPropertyPath =  "TaxDeductionPercentage",
					  						ListPropertyPath =  "TaxDeductionPercentage",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
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
					  						FullFieldLable =  "TaxDeductionPercentage",
					  						DefaultText =  @"Tax D. Percentage",
					  						FullLocalDefaultText =  @"אחוז ניכוי מס במקור מחושב",
					  						HelpTextCode =  "TaxDeductionPercentage",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TaxDeductionLocalAmount",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "Decimal",
					  						Code =  "TaxDeductionLocalAmount",
					  						MaxLength =  0,
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
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  2,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  16,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "TaxDeductionLocalAmount",
					  						DefaultText =  @"Tax Deduction",
					  						FullLocalDefaultText =  @"סכום ניכוי מס במקור",
					  						HelpTextCode =  "TaxDeductionLocalAmount",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AccountingPaymentMethodId",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "AccountingPaymentMethod",
					  						Code =  "AccountingPaymentMethodId",
					  						MaxLength =  20,
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
					  						PMPropertyPath =  "AccountingPaymentMethodId",
					  						ListPropertyPath =  "AccountingPaymentMethodId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
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
					  						IsRequired =  true,
					  						FullFieldLable =  "AccountingPaymentMethodId",
					  						DefaultText =  @"Payment Method",
					  						FullLocalDefaultText =  @"אמצעי תשלום",
					  						HelpTextCode =  "AccountingPaymentMethodId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BankAccountId",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "nText",
					  						Code =  "BankAccountId",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "BankAccountId",
					  						ListPropertyPath =  "BankAccountId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
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
					  						FullFieldLable =  "BankAccountId",
					  						DefaultText =  @"BankAccount",
					  						FullLocalDefaultText =  @"חשבון בנק",
					  						HelpTextCode =  "BankAccountId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BranchName",
					  						ObjectTableName =  "APPayment",
					  						FieldsDataType =  "Text",
					  						Code =  "BranchName",
					  						MaxLength =  100,
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
					  						PMPropertyPath =  "BranchName",
					  						ListPropertyPath =  "BranchName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "APPayment",
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
					  						FullFieldLable =  "BranchName",
					  						DefaultText =  @"Branch",
					  						FullLocalDefaultText =  @"סניף",
					  						ListFieldLable =  "BranchNameListLable",
					  						ListLableDefaultText =  @"Branch",
					  						ListLocalDefaultText =  @"סניף",
					  						HelpTextCode =  "BranchName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	    {  
	        FeatureRepository featureRepository = new FeatureRepository(0); 
            List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
            IWebFreightContext objectContext = WebFreightContext.GetContext(0);  
	        QueryGroup APPaymentQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "APPY", Name = "AP Payments" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable APPaymentObjectTable = objectContext.ObjectTables.Where(d => d.Name == "APPayment" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> APPaymentObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "APPayment").ToList();   

			   TextCode APPaymentTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.Q.AllAPPayments", DefaultText = @"All Payments",LocalDefaultText = "כל התשלומים", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature APPaymentFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ALLPAYMENTS", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "APPayment.Features.AllPayments", NameTextCodeDefaultText = "All Payments", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode APPaymentTextCode_1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.Q.OpenAPPayments", DefaultText = @"Open Payments",LocalDefaultText = "תשלומים פתוחים", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature APPaymentFeature_1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OPENPAYMENTS", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "APPayment.Features.OpenPayments", NameTextCodeDefaultText = "Open Payments", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode APPaymentTextCode_2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.Q.DraftAPPayments", DefaultText = @"Draft Payments",LocalDefaultText = "תשלומים בסטטוס טיוטה", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature APPaymentFeature_2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DRAFTPAYMENTS", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "APPayment.Features.DraftPayments", NameTextCodeDefaultText = "Draft Payments", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode APPaymentTextCode_3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.Q.NotReadyPayments", DefaultText = @"Not Ready Payments",LocalDefaultText = "לא מוכן תשלומים", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature APPaymentFeature_3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NOTREADYPAYMENTS", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "APPayment.Features.NotReadyPayments", NameTextCodeDefaultText = "Not Ready Payments", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode APPaymentTextCode_4 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.Q.MarkedAsBlockedForTransfer", DefaultText = @"Marked as blocked for transfer",LocalDefaultText = "מסומן כחסום לצורך העברה", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature APPaymentFeature_4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MARKEDASBLOCKEDFORTRANSFER", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "APPayment.Features.MarkedAsBlockedForTransfer", NameTextCodeDefaultText = "Marked as blocked for transfer", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query AllPaymentsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = APPaymentTextCode_0.Id, Code = "All Payments",  QueryGroupCode = "APPY", IndexOrder = 0, Tenant = 0, ObjectTableId = APPaymentObjectTable.Id, QuerySection = "APPayment", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = APPaymentFeature_0.Id, DefaultSortName = "PaymentNo", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
	
			 QueryColumn AllPaymentsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllPaymentsQuery.Id, IndexOrder = 0, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "PaymentNo" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllPaymentsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllPaymentsQuery.Id, IndexOrder = 1, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllPaymentsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllPaymentsQuery.Id, IndexOrder = 2, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "RegisterDate" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllPaymentsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllPaymentsQuery.Id, IndexOrder = 3, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "PaymentCurrencyCode" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllPaymentsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllPaymentsQuery.Id, IndexOrder = 4, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllPaymentsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllPaymentsQuery.Id, IndexOrder = 5, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllPaymentsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllPaymentsQuery.Id, IndexOrder = 6, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllPaymentsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllPaymentsQuery.Id, IndexOrder = 7, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "PaymentMethodName" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllPaymentsQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllPaymentsQuery.Id, IndexOrder = 8, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "AmountInPaymentCurrency" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllPaymentsQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllPaymentsQuery.Id, IndexOrder = 9, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "OpenAmount" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllPaymentsQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllPaymentsQuery.Id, IndexOrder = 10, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "InternalNotes" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
  
	      

			  Query OpenPaymentsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = APPaymentTextCode_1.Id, Code = "Open Payments",  QueryGroupCode = "APPY", IndexOrder = 1, Tenant = 0, ObjectTableId = APPaymentObjectTable.Id, QuerySection = "APPayment", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = APPaymentFeature_1.Id, DefaultSortName = "PaymentNo", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
	
			 QueryColumn OpenPaymentsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenPaymentsQuery.Id, IndexOrder = 0, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "PaymentNo" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenPaymentsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenPaymentsQuery.Id, IndexOrder = 1, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenPaymentsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenPaymentsQuery.Id, IndexOrder = 2, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "RegisterDate" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenPaymentsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenPaymentsQuery.Id, IndexOrder = 3, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "PaymentCurrencyCode" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenPaymentsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenPaymentsQuery.Id, IndexOrder = 4, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenPaymentsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenPaymentsQuery.Id, IndexOrder = 5, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenPaymentsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenPaymentsQuery.Id, IndexOrder = 6, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenPaymentsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenPaymentsQuery.Id, IndexOrder = 7, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "PaymentMethodName" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenPaymentsQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenPaymentsQuery.Id, IndexOrder = 8, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "AmountInPaymentCurrency" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenPaymentsQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenPaymentsQuery.Id, IndexOrder = 9, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "OpenAmount" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenPaymentsQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenPaymentsQuery.Id, IndexOrder = 10, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "InternalNotes" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter OpenPaymentsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "OpenPayments" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true",PredefinedValue2 = null, QueryId = OpenPaymentsQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query DraftPaymentsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = APPaymentTextCode_2.Id, Code = "Draft Payments",  QueryGroupCode = "APPY", IndexOrder = 1, Tenant = 0, ObjectTableId = APPaymentObjectTable.Id, QuerySection = "APPayment", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = APPaymentFeature_2.Id, DefaultSortName = "PaymentNo", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
	
			 QueryColumn DraftPaymentsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftPaymentsQuery.Id, IndexOrder = 0, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "PaymentNo" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftPaymentsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftPaymentsQuery.Id, IndexOrder = 1, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftPaymentsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftPaymentsQuery.Id, IndexOrder = 2, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "RegisterDate" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftPaymentsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftPaymentsQuery.Id, IndexOrder = 3, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "PaymentCurrencyCode" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftPaymentsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftPaymentsQuery.Id, IndexOrder = 4, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftPaymentsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftPaymentsQuery.Id, IndexOrder = 5, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftPaymentsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftPaymentsQuery.Id, IndexOrder = 6, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftPaymentsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftPaymentsQuery.Id, IndexOrder = 7, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "PaymentMethodName" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftPaymentsQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftPaymentsQuery.Id, IndexOrder = 8, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "AmountInPaymentCurrency" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftPaymentsQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftPaymentsQuery.Id, IndexOrder = 9, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "OpenAmount" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftPaymentsQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftPaymentsQuery.Id, IndexOrder = 10, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "InternalNotes" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter DraftPaymentsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "DraftPayments" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true",PredefinedValue2 = null, QueryId = DraftPaymentsQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query NotReadyPaymentsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = APPaymentTextCode_3.Id, Code = "Not Ready Payments",  EditWizardComponentPath = "./InvoiceModules/APPayment/Components/NewEntity/APPaymentTransferTemplate",
			   QueryGroupCode = "APPY", IndexOrder = 3, Tenant = 0, ObjectTableId = APPaymentObjectTable.Id, QuerySection = "APPayment", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = APPaymentFeature_3.Id, DefaultSortName = "PaymentNo", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
	
			 QueryColumn NotReadyPaymentsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotReadyPaymentsQuery.Id, IndexOrder = 0, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "RegisterDate" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn NotReadyPaymentsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotReadyPaymentsQuery.Id, IndexOrder = 1, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "PaymentNo" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn NotReadyPaymentsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotReadyPaymentsQuery.Id, IndexOrder = 2, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 190 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn NotReadyPaymentsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotReadyPaymentsQuery.Id, IndexOrder = 3, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn NotReadyPaymentsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotReadyPaymentsQuery.Id, IndexOrder = 4, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "AmountInPaymentCurrency" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn NotReadyPaymentsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotReadyPaymentsQuery.Id, IndexOrder = 5, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "ReadyForTransfer" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 50 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn NotReadyPaymentsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotReadyPaymentsQuery.Id, IndexOrder = 6, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "TransferError" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 500 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter NotReadyPaymentsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "NotReadyPayments" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true",PredefinedValue2 = null, QueryId = NotReadyPaymentsQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query MarkedasblockedfortransferQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = APPaymentTextCode_4.Id, Code = "Marked as blocked for transfer",  EditWizardComponentPath = "./InvoiceModules/APPayment/Components/NewEntity/APPaymentTransferTemplate",
			   QueryGroupCode = "APPY", IndexOrder = 4, Tenant = 0, ObjectTableId = APPaymentObjectTable.Id, QuerySection = "APPayment", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = APPaymentFeature_4.Id, DefaultSortName = "PaymentNo", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
	
			 QueryColumn MarkedasblockedfortransferQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MarkedasblockedfortransferQuery.Id, IndexOrder = 0, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "RegisterDate" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MarkedasblockedfortransferQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MarkedasblockedfortransferQuery.Id, IndexOrder = 1, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "PaymentNo" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MarkedasblockedfortransferQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MarkedasblockedfortransferQuery.Id, IndexOrder = 2, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 190 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MarkedasblockedfortransferQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MarkedasblockedfortransferQuery.Id, IndexOrder = 3, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MarkedasblockedfortransferQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MarkedasblockedfortransferQuery.Id, IndexOrder = 4, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "AmountInPaymentCurrency" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MarkedasblockedfortransferQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MarkedasblockedfortransferQuery.Id, IndexOrder = 5, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "ReadyForTransfer" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 50 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MarkedasblockedfortransferQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MarkedasblockedfortransferQuery.Id, IndexOrder = 6, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "TransferError" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 500 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter MarkedasblockedfortransferQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "MarkedAsBlockedForTransfer" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true",PredefinedValue2 = null, QueryId = MarkedasblockedfortransferQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable APPaymentObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "APPayment" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> APPaymentObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "APPayment").ToList();
		       
	      

	         Screen APPaymentHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "APPayment.HeaderScreen", Name = "Header Screen", ObjectTableId = APPaymentObjectTable.Id, NumberOfColumns = 4, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField APPaymentAPPaymentHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "PaymentNo").FirstOrDefault().Id, ScreenId = APPaymentHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField APPaymentAPPaymentHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "OpenAmount").FirstOrDefault().Id, ScreenId = APPaymentHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField APPaymentAPPaymentHeaderScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "StatusName").FirstOrDefault().Id, ScreenId = APPaymentHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField APPaymentAPPaymentHeaderScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "TransferStatusName").FirstOrDefault().Id, ScreenId = APPaymentHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    APPaymentObjectTable.HeaderScreenId = APPaymentHeaderScreenScreen0.Id;
	   		  
	      

	         Screen APPaymentGeneralTabScreenScreen1 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "APPayment.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = APPaymentObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 6, IsReadOnly = false }, screensRepository, tenantScreens);
      
            ScreenField APPaymentAPPaymentGeneralTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "PaymentNo").FirstOrDefault().Id, ScreenId = APPaymentGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField APPaymentAPPaymentGeneralTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "AccountingPaymentMethodCode").FirstOrDefault().Id, ScreenId = APPaymentGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField APPaymentAPPaymentGeneralTabScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "VendorId").FirstOrDefault().Id, ScreenId = APPaymentGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField APPaymentAPPaymentGeneralTabScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 3, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "StatusCode").FirstOrDefault().Id, ScreenId = APPaymentGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField APPaymentAPPaymentGeneralTabScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "LocalCurrencyId").FirstOrDefault().Id, ScreenId = APPaymentGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField APPaymentAPPaymentGeneralTabScreenScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "PaymentCurrencyId").FirstOrDefault().Id, ScreenId = APPaymentGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField APPaymentAPPaymentGeneralTabScreenScreenField6 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 2, ObjectFieldId = APPaymentObjectFields.Where(d => d.FieldName == "InternalNotes").FirstOrDefault().Id, ScreenId = APPaymentGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
           

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {    
			 ObjectTable APPaymentObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "APPayment" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode APPaymentDetailsTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.TH.Details", DefaultText = "Details",LocalDefaultText = "פרטים", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature APPaymentDetailsFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DETAILS", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "APPayment.Features.Details", NameTextCodeDefaultText = "Details", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode APPaymentDocsOutTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.TH.Docs", DefaultText = "Docs Out",LocalDefaultText = "מסמכים שיצאו", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
 
                 
			   TextCode APPaymentDocsInTextCode_TH2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.TH.DocsIn", DefaultText = "Docs In",LocalDefaultText = "מסמכים", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
 
                 
			   TextCode APPaymentTransferDetailsTextCode_TH3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.TH.TransferDetails", DefaultText = "Transfer Details",LocalDefaultText = "נתוני העברה", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature APPaymentTransferDetailsFeature_TH3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACCOUNTINGTRANSFER", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.AccountingTransfer", NameTextCodeDefaultText = "Accounting Transfer", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode APPaymentEventsTextCode_TH4 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.TH.Events", DefaultText = "Events",LocalDefaultText = "אירועים", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "APPD",HtmlComponentName = "",HtmlComponentUrl = "./InvoiceModules/APPayment/Components/EditTabs/APPaymentDetailsTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "DETAILS" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.InvoiceLib.Views.Tabs.APPaymentsTabs.APPaymentsDetailsTabControl", ObjectTableId = APPaymentObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "APPayment.TH.Details" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "APDO",HtmlComponentName = "",HtmlComponentUrl = "./InvoiceModules/APPayment/Components/EditTabs/APPaymentDocsOutTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "DETAILS" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.InvoiceLib.Views.Tabs.APPaymentsTabs.APPaymentDocsOutTabControl", ObjectTableId = APPaymentObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "APPayment.TH.Docs" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "APDI",HtmlComponentName = "",HtmlComponentUrl = "./InvoiceModules/APPayment/Components/EditTabs/APPaymentDocsInTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "DETAILS" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.InvoiceLib.Views.Tabs.APPaymentsTabs.APPaymentsDocsInTabControl", ObjectTableId = APPaymentObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "APPayment.TH.DocsIn" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 2 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "APPT",HtmlComponentName = "",HtmlComponentUrl = "./InvoiceModules/APPayment/Components/EditTabs/APPaymentTransferTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "ACCOUNTINGTRANSFER" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.InvoiceLib.Views.Tabs.APPaymentsTabs.APPaymentTransferTabControl", ObjectTableId = APPaymentObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "APPayment.TH.TransferDetails" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 3 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "APPE",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = tenantFeatures.Where(d => d.Code == "DETAILS" && d.ObjectTableId == APPaymentObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = APPaymentObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "APPayment.TH.Events" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 4 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable APPaymentObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "APPayment" && d.Tenant == 0).FirstOrDefault(); 
		   Feature APPaymentFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "APPayment.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature APPaymentFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "APPayment.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature APPaymentFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "APPayment.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature APPaymentFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "APPayment.Features.PackageFeature", NameTextCodeDefaultText = "APPayment Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes); 

		   		   //--------------> Additional Features <--------------\\

		   Feature APPaymentFeature_DOCSOUT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DOCSOUT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "APPayment.Features.DocsOut", NameTextCodeDefaultText = @"Docs Out" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature APPaymentFeature_DOCSIN = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DOCSIN", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "APPayment.Features.DocsIn", NameTextCodeDefaultText = @"Docs In" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature APPaymentFeature_EVENTS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EVENTS", FeatureTypeCode = "AREA", Packagable = false, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "APPayment.Features.Events", NameTextCodeDefaultText = @"Events" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature APPaymentFeature_APPaymentEditExchangeRate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "APPaymentEditExchangeRate", FeatureTypeCode = "ACT", Packagable = false, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "APPayment.Features.EditExchangeRate", NameTextCodeDefaultText = @"Edit Exchange Rate" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature APPaymentFeature_RecalculateExternals = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "RecalculateExternals", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "APPayment.Features.RecalculateExternals", NameTextCodeDefaultText = @"Recalculate External IDs" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature APPaymentFeature_EnableMultiCurrency = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EnableMultiCurrency", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "APPayment.Features.EnableMultiCurrency", NameTextCodeDefaultText = @"Enable multi-currency" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

   
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable APPaymentObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "APPayment" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "UPAP",
                ShortView =  false,
                IsManualEntry =  false,
                LocalName =  "Payment Updated",
                EnglishName =  "Payment Updated",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = APPaymentObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CRAP",
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
                ObjectTableId = APPaymentObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "APPA",
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
                ObjectTableId = APPaymentObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "APPC",
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
                ObjectTableId = APPaymentObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "APPV",
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
                ObjectTableId = APPaymentObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CNPY",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Connected",
                EnglishName =  "Connected",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = APPaymentObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
		   FeatureRepository featureRepository = new FeatureRepository(0); 
		   List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList(); 
		   ObjectTable APPaymentObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "APPayment" && d.Tenant == 0).FirstOrDefault();       
    
			   Feature APPaymentFeature_MB0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "APPROVE", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "APPayment.Features.Approve", NameTextCodeDefaultText = "Approve", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

      
    
			   Feature APPaymentFeature_MB1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PRINT", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "APPayment.Features.Print", NameTextCodeDefaultText = "Print", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

      
    
			   Feature APPaymentFeature_MB2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Actions", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "APPayment.Features.More", NameTextCodeDefaultText = "More", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

			   Feature APPaymentFeature_MB20 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CANCELAPPROVAL", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "APPayment.Features.CancelApproval", NameTextCodeDefaultText = "Cancel Approval", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
             			   Feature APPaymentFeature_MB21 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "VOID", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "APPayment.Features.Void", NameTextCodeDefaultText = "Void", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
              

		   TextCodeRepository.SubmitChanges();
		   FeaturesRepository.SubmitChanges();
		   MenuButtonGroup APPaymentMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
				{
					MenuButtonGroupType = "APPaymentEdit",
					Name = "APPaymentEditButtonsGroup",
					ObjectTableId = APPaymentObjectTable.Id,
					Tenant = 0
				}, menuButtonGroupRepository, tenantMenuButtonGroups);        
   
			   MenuButton APPaymentMenuButton0 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "ApproveAPPayment",
						Index = 0, 
						IsActive = true,
						LabelTextCodeCode = "APPayment.B.Approve",
						LabelTextCodeDefaultText = "Approve",
						Tenant = 0,
						MenuButtonGroupId = APPaymentMenuButtonGroup.Id,
						ObjectTableId = APPaymentObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = APPaymentFeature_MB0.Id,
						Style = "ApproveButtonStyle",
						LocalDefaultText = "אישור",
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton APPaymentMenuButton1 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "PrintAPPayment",
						Index = 1, 
						IsActive = true,
						LabelTextCodeCode = "APPayment.B.Print",
						LabelTextCodeDefaultText = "Print",
						Tenant = 0,
						MenuButtonGroupId = APPaymentMenuButtonGroup.Id,
						ObjectTableId = APPaymentObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = APPaymentFeature_MB1.Id,
						Style = null,
						LocalDefaultText = "הדפסה",
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton APPaymentMenuButton2 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "Actions",
						Index = 2, 
						IsActive = true,
						LabelTextCodeCode = "APPayment.B.Actions",
						LabelTextCodeDefaultText = "More",
						Tenant = 0,
						MenuButtonGroupId = APPaymentMenuButtonGroup.Id,
						ObjectTableId = APPaymentObjectTable.Id,
						MenuButtonType = "dropdownbutton",
						FeatureId = APPaymentFeature_MB2.Id,
						Style = null,
						LocalDefaultText = "ביטול",
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);

			   MenuButton APPaymentMenuButton20 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "CancelApproval",
						Index = 0, 
						IsActive = true,
						LabelTextCodeCode = "APPayment.B.CancelApproval",
						LabelTextCodeDefaultText = "Cancel Approval",
						Tenant = 0,
						MenuButtonGroupId = APPaymentMenuButtonGroup.Id,
						ParentMenuButtonId = APPaymentMenuButton2.Id,
						ObjectTableId = APPaymentObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  APPaymentFeature_MB20.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton APPaymentMenuButton21 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "VoidAPPaymentOperationsSeparator",
						Index = 1, 
						IsActive = true,
						LabelTextCodeCode = "APPayment.B.VoidAPPaymentOperationsSeparator",
						LabelTextCodeDefaultText = "",
						Tenant = 0,
						MenuButtonGroupId = APPaymentMenuButtonGroup.Id,
						ParentMenuButtonId = APPaymentMenuButton2.Id,
						ObjectTableId = APPaymentObjectTable.Id,
						MenuButtonType = "separator",
						
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton APPaymentMenuButton22 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "VoidAPPayment",
						Index = 2, 
						IsActive = true,
						LabelTextCodeCode = "APPayment.B.Void",
						LabelTextCodeDefaultText = "Void",
						Tenant = 0,
						MenuButtonGroupId = APPaymentMenuButtonGroup.Id,
						ParentMenuButtonId = APPaymentMenuButton2.Id,
						ObjectTableId = APPaymentObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  APPaymentFeature_MB21.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable APPaymentObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "APPayment" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode APPaymentTextCode_APPaymentSShortTitle = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.S.ShortTitle", DefaultText = "A/P Payment",LocalDefaultText = @"תשלום לספק", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APPaymentTextCode_APPaymentMFullAccountingCashBookCheck = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.M.FullAccountingCashBookCheck", DefaultText = "Cashbook amount is lower than payment amount",LocalDefaultText = @"היתרה בקופה הינה קטנה מסכום הוראת התשלום", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APPaymentTextCode_APPaymentMNoVendorTaxWithholdingPercentage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.M.NoVendorTaxWithholdingPercentage", DefaultText = "There is no tax withholding definitions for this vendor, the default tax withholding percentage will be taken from system defaults",LocalDefaultText = @"לא מוגדר ללקוח אחוז ניכוי מס במקור, אחוז ניכוי ברירת מחדל ילקח מהגדרות מערכת", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APPaymentTextCode_APPaymentMSearchByMsg = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.M.SearchByMsg", DefaultText = "Search by Invoice #/ Vendor",LocalDefaultText = @"חפש לפי חשבונית\ספק", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APPaymentTextCode_APPaymentCHOtherPayments = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.CH.OtherPayments", DefaultText = "Other Payments",LocalDefaultText = @"קבלות אחרות", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APPaymentTextCode_APPaymentCHAmountToPay = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.CH.AmountToPay", DefaultText = "Amount To Pay",LocalDefaultText = @"סכום שהותאם", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APPaymentTextCode_APPaymentCHShipmentNo = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.CH.ShipmentNo", DefaultText = "Shipment #",LocalDefaultText = @"משלוח", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APPaymentTextCode_APPaymentMValueDateCantBeFutureDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.M.ValueDateCantBeFutureDate", DefaultText = "Value date can't be future date",LocalDefaultText = @"תאריך ערך לא יכול להיות תאריך עתידי", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APPaymentTextCode_APPaymentMPaymentInvoicesHaveErrors = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.M.PaymentInvoicesHaveErrors", DefaultText = "Payment Invoices have errors",LocalDefaultText = @"חשבוניות ספק עם שגיאות", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APPaymentTextCode_APPaymentSDetailsCreditCard = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.S.Details.CreditCard", DefaultText = "Credit Card",LocalDefaultText = @"כרטיס אשראי", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APPaymentTextCode_APPaymentSDetailsDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.S.Details.Details", DefaultText = "Details",LocalDefaultText = @"פרטים", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APPaymentTextCode_APPaymentSDetailsRate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.S.Details.Rate", DefaultText = "Rate",LocalDefaultText = @"שער", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APPaymentTextCode_APPaymentSDetailsCurrencyDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.S.Details.CurrencyDetails", DefaultText = "Currency Details",LocalDefaultText = @"נתוני מטבע", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APPaymentTextCode_APPaymentSDetailsNotMatched = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.S.Details.NotMatched", DefaultText = "Not Matched",LocalDefaultText = @"לא מותאם", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APPaymentTextCode_APPaymentSDetailsTotals = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.S.Details.Totals", DefaultText = "Totals",LocalDefaultText = @"סיכומים", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APPaymentTextCode_APPaymentSDetailsAPInvoices = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.S.Details.APInvoices", DefaultText = "A/P Invoices",LocalDefaultText = @"חשבוניות ספק", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APPaymentTextCode_APPaymentSDetailsPaymentRef = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.S.Details.PaymentRef", DefaultText = "Payment Ref",LocalDefaultText = @"אסמכתא תשלום ", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APPaymentTextCode_APPaymentSDetailsChequeRef = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.S.Details.ChequeRef", DefaultText = "Cheque Ref",LocalDefaultText = @"מספק שיק", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APPaymentTextCode_APPaymentSDetailsAmountToPay = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.S.Details.AmountToPay", DefaultText = "Amount To Pay",LocalDefaultText = @"סה''כ סכום שהותאם", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APPaymentTextCode_APPaymentSDetailsCheque = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.S.Details.Cheque", DefaultText = "Cheque",LocalDefaultText = @"שיק", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APPaymentTextCode_APPaymentSDetailsBankTransfer = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.S.Details.BankTransfer", DefaultText = "Bank Transfer",LocalDefaultText = @"העברה בנקאית", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APPaymentTextCode_APPaymentMCantSetZeroAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.M.CantSetZeroAmount", DefaultText = "Can't set Payment Amount to Zero",LocalDefaultText = @"לא ניתן להגדיר סכום לתשלום בערך אפס", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APPaymentTextCode_APPaymentMCantSetMinusAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.M.CantSetMinusAmount", DefaultText = "Can't set Payment Amount to minus amount",LocalDefaultText = @"לא ניתן להגדיר סכום לתשלום בערך שלילי", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APPaymentTextCode_APPaymentMCantSetFutureDatePayment = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.M.CantSetFutureDatePayment", DefaultText = "Can't create payment with future date",LocalDefaultText = @"לא ניתן להכין תשלום עם תאריך עתידי", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APPaymentTextCode_APPaymentMPaymentInvoicesHasErrors = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.M.PaymentInvoicesHasErrors", DefaultText = "Payment Invoices has errors",LocalDefaultText = null, ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APPaymentTextCode_APPaymentMPaymentAmountPaidCantBeMinus = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.M.PaymentAmountPaidCantBeMinus", DefaultText = "Payment Amount Paid can't be minus",LocalDefaultText = @"לא ניתן להגדיר סכום לתשלום בערך שלילי", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APPaymentTextCode_APPaymentMPaymentAmountPaidCantBeBigger = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.M.PaymentAmountPaidCantBeBigger", DefaultText = "Payment Amount Paid can't be bigger than Payment Amount",LocalDefaultText = @"הסכום ששולם לא יכול להיות גדול מהסכום לתשלום", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APPaymentTextCode_APPaymentMAccountingSettingsDontAllowVoid = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.M.AccountingSettingsDontAllowVoid", DefaultText = "Accounting Settings doesn't allow void A/P Payment",LocalDefaultText = @"הגדרות המערכת לא מאפשרות התעלמות מתשלום לספק", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APPaymentTextCode_APPaymentMDisconnectInvoices = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.M.DisconnectInvoices", DefaultText = "Please disconnect all invoices",LocalDefaultText = @"אנא נתק את כל החשבוניות", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APPaymentTextCode_APPaymentMConfirmVoid = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.M.ConfirmVoid", DefaultText = "Once you void or delete an payment, the change is permanent. If you void or delete an payment and want to restore it later, you'll have to create a new payment.",LocalDefaultText = @"לאחר ביטול או מחיקה של תשלום, השינוי הוא קבוע. אם תבטל או תמחק תשלום וברצונך לשחזר אותו במועד מאוחר יותר, יהיה עליך ליצור תשלום חדש.", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APPaymentTextCode_APPaymentMNoTemplate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.M.NoTemplate", DefaultText = "A/P Payment document has no template!",LocalDefaultText = @"אין תבנית הדפסה למסמך תשלום לספק", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APPaymentTextCode_APPaymentMOnlyMinusValue = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.M.OnlyMinusValue", DefaultText = "Only minus value allowed",LocalDefaultText = @"מותר רק ערכים שליליים", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APPaymentTextCode_APPaymentMAmountPaidNotLess = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.M.AmountPaidNotLess", DefaultText = "Amount Paid can't be less than",LocalDefaultText = @"סכום לתשלום לא יכול להיות קטן מ-", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APPaymentTextCode_APPaymentMCantPayMinusValue = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.M.CantPayMinusValue", DefaultText = "Can't pay minus value",LocalDefaultText = @"לא ניתן לשלם סכום במינוס", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APPaymentTextCode_APPaymentMAmountPaidLessOrEqual = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment.M.AmountPaidLessOrEqual", DefaultText = "Amount Paid must be less than or equals to",LocalDefaultText = @"סכום לתשלום חייב להיות שווה או פחות מ-", ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode APPaymentTextCode_APPayment = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPayment", DefaultText = "A/P Payment",LocalDefaultText = null, ObjectTableId = APPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "T", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 