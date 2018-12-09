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
   public class ARPaymentUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "ARPayment",
			      				    IsNew =  false,
			      				    DBTableName =  "ARPayments",
			      				    OldDBTableName =  "ARPayments",
			      				    ObjectTableSingular =  "A/R Payment",
			      				    ObjectTablePlural =  "A/R Payments",
			      				    HasCustomFilter =  true,
			      				    HasCustomFields =  false,
			      				    HasHelper =  true,
			      				    HasShortTitle =  true,
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
			      				    SortingByObjectField =  "PaymentNo",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "BR",
			      				    MaxNumberOfCustomFields =  0,
			      				    NewWizardControlName =  "Simplog.InvoiceLib.NewARPaymentCommand",
			      				    DefaultText =  "A/R Payment",
			      				    Code =  "ARPT",
			      				    Name =  "AR Payments",
			      				    GenerateDomainService =  false,
			      				    NoTS =  false,
			      				    HasMenuButtons =  true,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  true,
			      				    IsLookUp =  false,
			      				    SearchFields =  "ARPayment,ARPayments,Simplog.InvoiceLib.NewARPaymentCommand,Id,",
			      				    ClientModuleName =  "Invoice",
			      				    NewWizardComponentPath =  "./InvoiceModules/ARPayment/Components/NewEntity/NewARPaymentComponent",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdatedByUserId",
					  						OldFieldName =  "UpdatedByUserId",
					  						ObjectTableName =  "ARPayment",
					  						FieldsDataType =  "LookUp",
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
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UpdatedByUserId",
					  						DefaultText =  @"Updated By",
					  						FullLocalDefaultText =  @"עודכן ע''י",
					  						ListFieldLable =  "UpdatedByUserIdListLable",
					  						ListLableDefaultText =  @"UpdatedByUserId",
					  						ListLocalDefaultText =  @"UpdatedByUserId",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						LookUpTableName =  "User",
					  						Code =  "UpdatedByUserId",
					  						Operator =  "Equals",
					  						DigitsAfterPoint =  0,
					  						NumberOfDigits =  0,
					  						CopyToDW =  false,
					  						HelpTextCode =  "UpdatedByUserId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdateDate",
					  						OldFieldName =  "UpdateDate",
					  						ObjectTableName =  "ARPayment",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
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
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UpdateDate",
					  						DefaultText =  @"Update Date",
					  						FullLocalDefaultText =  @"תאריך עדכון",
					  						ListFieldLable =  "UpdateDateListLable",
					  						ListLableDefaultText =  @"Update Date",
					  						ListLocalDefaultText =  @"תאריך עדכון",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "UpdateDate",
					  						Operator =  "Equals",
					  						DigitsAfterPoint =  0,
					  						NumberOfDigits =  0,
					  						CopyToDW =  false,
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						HelpTextCode =  "UpdateDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PaymentNo",
					  						OldFieldName =  "PaymentNo",
					  						ObjectTableName =  "ARPayment",
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
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  true,
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
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"מספר קבלה",
					  						ListLocalDefaultText =  @"מספר קבלה",
					  						HelpTextCode =  "PaymentNo",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreateDate",
					  						OldFieldName =  "CreateDate",
					  						ObjectTableName =  "ARPayment",
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
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"תאריך יצירה",
					  						ListLocalDefaultText =  @"תאריך יצירה",
					  						HelpTextCode =  "CreateDate",
					  						HelpLocalDefaultText =  @"תאריך יצירה",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreatedByUserId",
					  						OldFieldName =  "CreatedByUserId",
					  						ObjectTableName =  "ARPayment",
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
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"יוצר",
					  						HelpTextCode =  "CreatedByUserId",
					  						HelpLocalDefaultText =  @"יוצר",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreatedByUserName",
					  						OldFieldName =  "CreatedByUserName",
					  						ObjectTableName =  "ARPayment",
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
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"יוצר",
					  						ListLocalDefaultText =  @"יוצר",
					  						HelpTextCode =  "CreatedByUserName",
					  						HelpLocalDefaultText =  @"יוצר",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PrintDate",
					  						OldFieldName =  "PrintDate",
					  						ObjectTableName =  "ARPayment",
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
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"תאריך הדפסה",
					  						ListLocalDefaultText =  @"תאריך הדפסה",
					  						HelpTextCode =  "PrintDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PrintByUserId",
					  						OldFieldName =  "PrintByUserId",
					  						ObjectTableName =  "ARPayment",
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
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"הודפס ע''י",
					  						HelpTextCode =  "PrintByUserId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LocalCurrencyId",
					  						OldFieldName =  "LocalCurrencyId",
					  						ObjectTableName =  "ARPayment",
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
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"מטבע מקומי",
					  						HelpTextCode =  "LocalCurrencyId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AmountInLocalCurrency",
					  						OldFieldName =  "AmountInLocalCurrency",
					  						ObjectTableName =  "ARPayment",
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
					  						DataTemplateName =  "ARPaymentAmountInLocalCurrencyDataTemplate",
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
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "AmountInLocalCurrency",
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"סכום(במטבע מקומי)",
					  						ListLocalDefaultText =  @"סכום(במטבע מקומי)",
					  						HelpTextCode =  "AmountInLocalCurrency",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BranchId",
					  						OldFieldName =  "BranchId",
					  						ObjectTableName =  "ARPayment",
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
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"סניף",
					  						HelpTextCode =  "BranchId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BillToId",
					  						OldFieldName =  "BillToId",
					  						ObjectTableName =  "ARPayment",
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
					  						PMPropertyPath =  "BillToId",
					  						ListPropertyPath =  "BillToId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						FullFieldLable =  "BillToId",
					  						DefaultText =  @"Bill To",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "BillToId",
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"לקוח",
					  						HelpTextCode =  "BillToId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BillToName",
					  						OldFieldName =  "BillToName",
					  						ObjectTableName =  "ARPayment",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  100,
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
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						FullFieldLable =  "BillToName",
					  						DefaultText =  @"Bill To",
					  						ListFieldLable =  "BillToNameListLable",
					  						ListLableDefaultText =  @"Bill To",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "BillToName",
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"לקוח",
					  						ListLocalDefaultText =  @"לקוח",
					  						HelpTextCode =  "BillToName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ARAccountId",
					  						OldFieldName =  "ARAccountId",
					  						ObjectTableName =  "ARPayment",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Account",
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ARAccountId",
					  						ListPropertyPath =  "ARAccountId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						FullFieldLable =  "ARAccountId",
					  						DefaultText =  @"Account",
					  						ListFieldLable =  "ARAccountIdListLable",
					  						ListLableDefaultText =  @"Account Id",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "ARAccountId",
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"חשבון",
					  						HelpTextCode =  "ARAccountId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "StatusCode",
					  						OldFieldName =  "StatusCode",
					  						ObjectTableName =  "ARPayment",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "ARPaymentStatus",
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
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"סטטוס",
					  						HelpTextCode =  "StatusCode",
					  						HelpLocalDefaultText =  @"סטטוס",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "StatusName",
					  						OldFieldName =  "StatusName",
					  						ObjectTableName =  "ARPayment",
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
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"סטטוס",
					  						ListLocalDefaultText =  @"סטטוס",
					  						HelpTextCode =  "StatusName",
					  						HelpLocalDefaultText =  @"סטטוס",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsClosed",
					  						OldFieldName =  "IsClosed",
					  						ObjectTableName =  "ARPayment",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						DataTemplateName =  "ARPaymentIsClosedPathTemplate",
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
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"סגור",
					  						HelpTextCode =  "IsClosed",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PaymentCurrencyId",
					  						OldFieldName =  "PaymentCurrencyId",
					  						ObjectTableName =  "ARPayment",
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
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						FullFieldLable =  "PaymentCurrencyId",
					  						DefaultText =  @"Payment Currency",
					  						ListFieldLable =  "PaymentCurrencyIdListLable",
					  						ListLableDefaultText =  @"Payment Currency Id",
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
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"מטבע קבלה",
					  						HelpTextCode =  "PaymentCurrencyId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PaymentCurrencyCode",
					  						OldFieldName =  "PaymentCurrencyCode",
					  						ObjectTableName =  "ARPayment",
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
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"מטבע",
					  						ListLocalDefaultText =  @"מטבע",
					  						HelpTextCode =  "PaymentCurrencyCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AmountInPaymentCurrency",
					  						OldFieldName =  "AmountInPaymentCurrency",
					  						ObjectTableName =  "ARPayment",
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
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"סכום קבלה",
					  						ListLocalDefaultText =  @"סכום קבלה",
					  						HelpTextCode =  "AmountInPaymentCurrency",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PaidBy",
					  						OldFieldName =  "PaidBy",
					  						ObjectTableName =  "ARPayment",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  50,
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
					  						PMPropertyPath =  "PaidBy",
					  						ListPropertyPath =  "PaidBy",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						FullFieldLable =  "PaidBy",
					  						DefaultText =  @"Paid By",
					  						ListFieldLable =  "PaidByListLable",
					  						ListLableDefaultText =  @"Paid By",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "PaidBy",
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"שולם ע''י",
					  						ListLocalDefaultText =  @"שולם ע''י",
					  						HelpTextCode =  "PaidBy",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AccountingPaymentMethodCode",
					  						OldFieldName =  "AccountingPaymentMethodCode",
					  						ObjectTableName =  "ARPayment",
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
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"אמצעי תשלום",
					  						HelpTextCode =  "AccountingPaymentMethodCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PaymentMethodName",
					  						OldFieldName =  "PaymentMethodName",
					  						ObjectTableName =  "ARPayment",
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
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"אמצעי תשלום",
					  						ListLocalDefaultText =  @"אמצעי תשלום",
					  						HelpTextCode =  "PaymentMethodName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreditAccountId",
					  						OldFieldName =  "CreditAccountId",
					  						ObjectTableName =  "ARPayment",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Account",
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CreditAccountId",
					  						ListPropertyPath =  "CreditAccountId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						FullFieldLable =  "CreditAccountId",
					  						DefaultText =  @"Credit Account",
					  						ListFieldLable =  "CreditAccountIdListLable",
					  						ListLableDefaultText =  @"Credit Account Id",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "CreditAccountId",
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"כרטיס זכות",
					  						HelpTextCode =  "CreditAccountId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PrintNotes",
					  						OldFieldName =  "PrintNotes",
					  						ObjectTableName =  "ARPayment",
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
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"הערות הדפסה",
					  						ListLocalDefaultText =  @"הערות הדפסה",
					  						HelpTextCode =  "PrintNotes",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InternalNotes",
					  						OldFieldName =  "InternalNotes",
					  						ObjectTableName =  "ARPayment",
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
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"הערות",
					  						ListLocalDefaultText =  @"הערות",
					  						HelpTextCode =  "InternalNotes",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PaymentCurrencyExchangeRate",
					  						OldFieldName =  "PaymentCurrencyExchangeRate",
					  						ObjectTableName =  "ARPayment",
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
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"שער",
					  						ListLocalDefaultText =  @"שער",
					  						HelpTextCode =  "PaymentCurrencyExchangeRate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ExchangeRateDate",
					  						OldFieldName =  "ExchangeRateDate",
					  						ObjectTableName =  "ARPayment",
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
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						FullFieldLable =  "ExchangeRateDate",
					  						DefaultText =  @"Exchange Date",
					  						ListFieldLable =  "ExchangeRateDateListLable",
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
					  						Code =  "ExchangeRateDate",
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"תאריך שער חליפין",
					  						ListLocalDefaultText =  @"תאריך שער חליפין",
					  						HelpTextCode =  "ExchangeRateDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BillToAddressId",
					  						OldFieldName =  "BillToAddressId",
					  						ObjectTableName =  "ARPayment",
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
					  						PMPropertyPath =  "BillToAddressId",
					  						ListPropertyPath =  "BillToAddressId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						FullFieldLable =  "BillToAddressId",
					  						DefaultText =  @"Bill To Address",
					  						ListFieldLable =  "BillToAddressIdListLable",
					  						ListLableDefaultText =  @"Bill To Address Id",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "BillToAddressId",
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"כתובת לחיוב",
					  						HelpTextCode =  "BillToAddressId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SearchFields",
					  						OldFieldName =  "SearchFields",
					  						ObjectTableName =  "ARPayment",
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
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
					  						ValidForQuerySection2 =  "ARPaymentFollowUp",
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
					  						DefaultText =  @"Search Payment # / Bill to / Reference",
					  						HelpTextCode =  "SearchFields",
					  						HelpTextDefaultText =  @"Searching by :\n1:Payment # \n2:Bill to \n3:Reference",
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
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"חיפוש לפי מספר קבלה\לקוח",
					  						HelpLocalDefaultText =  @"חיפוש לפי מספר קבלה\לקוח",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreditAccountName",
					  						OldFieldName =  "CreditAccountName",
					  						ObjectTableName =  "ARPayment",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  40,
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
					  						PMPropertyPath =  "CreditAccountName",
					  						ListPropertyPath =  "CreditAccountName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						FullFieldLable =  "CreditAccountName",
					  						DefaultText =  @"Credit Account",
					  						ListFieldLable =  "CreditAccountNameListLable",
					  						ListLableDefaultText =  @"Credit Account",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "CreditAccountName",
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"כרטיס זכות",
					  						HelpTextCode =  "CreditAccountName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "OpenAmount",
					  						OldFieldName =  "OpenAmount",
					  						ObjectTableName =  "ARPayment",
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
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"סכום פתוח",
					  						ListLocalDefaultText =  @"סכום פתוח",
					  						HelpTextCode =  "OpenAmount",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ValueDate",
					  						OldFieldName =  "ValueDate",
					  						ObjectTableName =  "ARPayment",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  true,
					  						DisplayOnLookUp =  false,
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
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
					  						ValidForQuerySection2 =  "ARPaymentFollowUp",
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
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"תאריך ערך",
					  						ListLocalDefaultText =  @"תאריך ערך",
					  						HelpTextCode =  "ValueDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "OpenPayments",
					  						OldFieldName =  "OpenPayments",
					  						ObjectTableName =  "ARPayment",
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
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"קבלות פתוחות",
					  						HelpTextCode =  "OpenPayments",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DraftPayments",
					  						OldFieldName =  "DraftPayments",
					  						ObjectTableName =  "ARPayment",
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
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"טיוטת חשבונית ספק",
					  						HelpTextCode =  "DraftPayments",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ChequeOrPaymentRef",
					  						OldFieldName =  "ChequeOrPaymentRef",
					  						ObjectTableName =  "ARPayment",
					  						FieldsDataType =  "nText",
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
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"אסמכתא שיק או חשבונית ספק",
					  						ListLocalDefaultText =  @"אסמכתא שיק או חשבונית ספק",
					  						HelpTextCode =  "ChequeOrPaymentRef",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Bank",
					  						OldFieldName =  "Bank",
					  						ObjectTableName =  "ARPayment",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  30,
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
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"בנק",
					  						HelpTextCode =  "Bank",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BankBranch",
					  						OldFieldName =  "BankBranch",
					  						ObjectTableName =  "ARPayment",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  30,
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
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"סניף בנק",
					  						HelpTextCode =  "BankBranch",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "RegisterDate",
					  						OldFieldName =  "RegisterDate",
					  						ObjectTableName =  "ARPayment",
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
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"תאריך רישום",
					  						ListLocalDefaultText =  @"תאריך רישום",
					  						HelpTextCode =  "RegisterDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreditCardTypeId",
					  						OldFieldName =  "CreditCardTypeId",
					  						ObjectTableName =  "ARPayment",
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
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"סוג כרטיס אשראי",
					  						HelpTextCode =  "CreditCardTypeId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LocalCurrencyCode",
					  						OldFieldName =  "LocalCurrencyCode",
					  						ObjectTableName =  "ARPayment",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  3,
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
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"מטבע מקומי",
					  						HelpTextCode =  "LocalCurrencyCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Account",
					  						OldFieldName =  "Account",
					  						ObjectTableName =  "ARPayment",
					  						FieldsDataType =  "nText",
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
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"חשבון",
					  						HelpTextCode =  "Account",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ARAccountName",
					  						OldFieldName =  "ARAccountName",
					  						ObjectTableName =  "ARPayment",
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
					  						PMPropertyPath =  "ARAccountName",
					  						ListPropertyPath =  "ARAccountName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						FullFieldLable =  "ARAccountName",
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
					  						Code =  "ARAccountName",
					  						CopyToDW =  false,
					  						FullLocalDefaultText =  @"חשבון",
					  						ListFieldLable =  "ARAccountNameListLable",
					  						ListLableDefaultText =  @"Account",
					  						HelpTextCode =  "ARAccountName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SATPaymentMethodCode",
					  						OldFieldName =  "SATPaymentMethodCode",
					  						ObjectTableName =  "ARPayment",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "SATPaymentMethod",
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
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						FullFieldLable =  "SATPaymentMethodCode",
					  						DefaultText =  @"Forma Pago",
					  						FullLocalDefaultText =  @"Forma Pago",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "SATPaymentMethodCode",
					  						CopyToDW =  false,
					  						HelpTextCode =  "SATPaymentMethodCode",
					  						HelpTextDefaultText =  @"Payment Method",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SATTransferStatusCode",
					  						OldFieldName =  "SATTransferStatusCode",
					  						ObjectTableName =  "ARPayment",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "SATTransferStatus",
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
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SATTransferStatusCode",
					  						DefaultText =  @"SAT Transfer Status",
					  						ListFieldLable =  "SATTransferStatusCodeListLable",
					  						ListLableDefaultText =  @"SAT TransferStatus Code",
					  						ListLocalDefaultText =  @"UsoCFDI",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "SATTransferStatusCode",
					  						Operator =  "StartsWith",
					  						ValidForQuerySection1 =  "ARPayment",
					  						DigitsAfterPoint =  0,
					  						NumberOfDigits =  0,
					  						CopyToDW =  false,
					  						HelpTextCode =  "SATTransferStatusCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TransmissionError",
					  						OldFieldName =  "TransmissionError",
					  						ObjectTableName =  "ARPayment",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  8000,
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
					  						ValidForQuerySection1 =  "ARPayment",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TransmissionError",
					  						DefaultText =  @"Transmission Error",
					  						FullLocalDefaultText =  @"שגיאת העברה",
					  						ListFieldLable =  "TransmissionErrorListLable",
					  						ListLableDefaultText =  @"Transmission Error",
					  						ListLocalDefaultText =  @"שגיאת העברה",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						Code =  "TransmissionError",
					  						Operator =  "StartsWith",
					  						DigitsAfterPoint =  0,
					  						NumberOfDigits =  0,
					  						CopyToDW =  false,
					  						DataTemplateName =  "TransferErrorDataTemplate",
					  						HelpTextCode =  "TransmissionError",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "MetodoPagoCode",
					  						OldFieldName =  "MetodoPagoCode",
					  						ObjectTableName =  "ARPayment",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "MetodoPago",
					  						MinLength =  0,
					  						MaxLength =  3,
					  						IsRequired =  false,
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
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "MetodoPagoCode",
					  						DefaultText =  @"Metodo Pago",
					  						ListFieldLable =  "MetodoPagoCodeListLable",
					  						ListLableDefaultText =  @"Metodo Pago",
					  						ListLocalDefaultText =  @"Metodo Pago",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "MetodoPagoCode",
					  						Operator =  "StartsWith",
					  						ValidForQuerySection1 =  "ARPayment",
					  						DigitsAfterPoint =  0,
					  						NumberOfDigits =  0,
					  						CopyToDW =  false,
					  						HelpTextCode =  "MetodoPagoCode",
					  						HelpTextDefaultText =  @"Way to Pay:\n-Pago en una sola exhibición (PUE): payment closed at once in one single payment type performed prior to the issuance of the invoice\n-Pago en parcialidades o diferido (PPD): partial payment or deferred performed after the issuance of the invoice",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TipoCadenaPago",
					  						OldFieldName =  "TipoCadenaPago",
					  						ObjectTableName =  "ARPayment",
					  						FieldsDataType =  "Text",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TipoCadenaPago",
					  						ListPropertyPath =  "TipoCadenaPago",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TipoCadenaPago",
					  						DefaultText =  @"Tipo Cadena Pago",
					  						ListFieldLable =  "TipoCadenaPagoListLable",
					  						ListLableDefaultText =  @"Tipo Cadena Pago",
					  						ListLocalDefaultText =  @"Tipo Cadena Pago",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "TipoCadenaPago",
					  						Operator =  "StartsWith",
					  						DigitsAfterPoint =  0,
					  						NumberOfDigits =  0,
					  						CopyToDW =  false,
					  						HelpTextCode =  "TipoCadenaPago",
					  						HelpTextDefaultText =  @"Payment Transfer Way",
					  						HelpLocalDefaultText =  @"Clave del tipo de cadena de pago que genera la entidad receptora de pago",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CertPago",
					  						OldFieldName =  "CertPago",
					  						ObjectTableName =  "ARPayment",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  true,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CertPago",
					  						ListPropertyPath =  "CertPago",
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
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CertPago",
					  						DefaultText =  @"Cert Pago",
					  						IsMaxLength =  true,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "CertPago",
					  						Operator =  "StartsWith",
					  						ValidForQuerySection1 =  "ARPayment",
					  						DigitsAfterPoint =  0,
					  						NumberOfDigits =  0,
					  						CopyToDW =  false,
					  						HelpTextCode =  "CertPago",
					  						HelpTextDefaultText =  @"The certificate that corresponds to the payment. It is a text chain of 64 base format",
					  						HelpLocalDefaultText =  @"Certificado que corresponde al pago",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CadPago",
					  						OldFieldName =  "CadPago",
					  						ObjectTableName =  "ARPayment",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  200,
					  						IsRequired =  false,
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
					  						PMPropertyPath =  "CadPago",
					  						ListPropertyPath =  "CadPago",
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
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CadPago",
					  						DefaultText =  @"Cad Pago",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "CadPago",
					  						Operator =  "StartsWith",
					  						ValidForQuerySection1 =  "ARPayment",
					  						DigitsAfterPoint =  0,
					  						NumberOfDigits =  0,
					  						CopyToDW =  false,
					  						HelpTextCode =  "CadPago",
					  						HelpTextDefaultText =  @"Payment original chain sent by the beneficiary's bank institution",
					  						HelpLocalDefaultText =  @"Cadena Original del Comprobante de Pago generado por la entidad emisora de la cuenta beneficiaria",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SelloPago",
					  						OldFieldName =  "SelloPago",
					  						ObjectTableName =  "ARPayment",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  true,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "SelloPago",
					  						ListPropertyPath =  "SelloPago",
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
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SelloPago",
					  						DefaultText =  @"Sello Pago",
					  						IsMaxLength =  true,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						Code =  "SelloPago",
					  						Operator =  "StartsWith",
					  						ValidForQuerySection1 =  "ARPayment",
					  						DigitsAfterPoint =  0,
					  						NumberOfDigits =  0,
					  						CopyToDW =  false,
					  						HelpTextCode =  "SelloPago",
					  						HelpTextDefaultText =  @"The digital seal associates the payment. It is a text chain of 64 base format",
					  						HelpLocalDefaultText =  @"Sello digital que se asocie el pago",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ARPaymentMethodCode",
					  						ObjectTableName =  "ARPayment",
					  						FieldsDataType =  "Text",
					  						Code =  "ARPaymentMethodCode",
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
					  						PMPropertyPath =  "ARPaymentMethodCode",
					  						ListPropertyPath =  "ARPaymentMethodCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						FullFieldLable =  "ARPaymentMethodCode",
					  						DefaultText =  @"Payment Method",
					  						FullLocalDefaultText =  @"אמצעי תשלום",
					  						HelpTextCode =  "ARPaymentMethodCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PaymentDate",
					  						ObjectTableName =  "ARPayment",
					  						FieldsDataType =  "DateTime",
					  						Code =  "PaymentDate",
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
					  						PMPropertyPath =  "PaymentDate",
					  						ListPropertyPath =  "PaymentDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						FullFieldLable =  "PaymentDate",
					  						DefaultText =  @"Payment Date",
					  						ListFieldLable =  "PaymentDateListLable",
					  						ListLableDefaultText =  @"Payment Date",
					  						HelpTextCode =  "PaymentDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InvoiceNumber",
					  						ObjectTableName =  "ARPayment",
					  						FieldsDataType =  "Text",
					  						Code =  "InvoiceNumber",
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
					  						PMPropertyPath =  "InvoiceNumber",
					  						ListPropertyPath =  "InvoiceNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						FullFieldLable =  "InvoiceNumber",
					  						DefaultText =  @"Invoice No.",
					  						FullLocalDefaultText =  @"מספר חשבונית",
					  						ListFieldLable =  "InvoiceNumberListLable",
					  						ListLableDefaultText =  @"Invoice No.",
					  						ListLocalDefaultText =  @"מספר חשבונית",
					  						HelpTextCode =  "InvoiceNumber",
					  						HelpLocalDefaultText =  @"מספר חשבונית",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ShipmentNumber",
					  						ObjectTableName =  "ARPayment",
					  						FieldsDataType =  "Text",
					  						Code =  "ShipmentNumber",
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
					  						PMPropertyPath =  "ShipmentNumber",
					  						ListPropertyPath =  "ShipmentNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						FullFieldLable =  "ShipmentNumber",
					  						DefaultText =  @"Shipment No.",
					  						FullLocalDefaultText =  @"מספר משלוח",
					  						ListFieldLable =  "ShipmentNumberListLable",
					  						ListLableDefaultText =  @"Shipment No.",
					  						ListLocalDefaultText =  @"מספר משלוח",
					  						HelpTextCode =  "ShipmentNumber",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SATTransferStatusName",
					  						ObjectTableName =  "ARPayment",
					  						FieldsDataType =  "Text",
					  						Code =  "SATTransferStatusName",
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
					  						PMPropertyPath =  "SATTransferStatusName",
					  						ListPropertyPath =  "SATTransferStatusName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
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
					  						HasTemplate =  true,
					  						IsRequired =  false,
					  						FullFieldLable =  "SATTransferStatusName",
					  						DefaultText =  @"SAT Transfer Status",
					  						ListFieldLable =  "SATTransferStatusNameListLable",
					  						ListLableDefaultText =  @"SAT Transfer Status",
					  						HelpTextCode =  "SATTransferStatusName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BankAccountLiteId",
					  						ObjectTableName =  "ARPayment",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "BankAccountLite",
					  						Code =  "BankAccountLiteId",
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
					  						PMPropertyPath =  "BankAccountLiteId",
					  						ListPropertyPath =  "BankAccountLiteId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						FullFieldLable =  "BankAccountLiteId",
					  						DefaultText =  @"Bank Account",
					  						FullLocalDefaultText =  @"חשבון בנק",
					  						ListFieldLable =  "BankAccountLiteIdListLable",
					  						ListLableDefaultText =  @"Bank Account",
					  						ListLocalDefaultText =  @"חשבון בנק",
					  						HelpTextCode =  "BankAccountLiteId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BankAccountId",
					  						ObjectTableName =  "ARPayment",
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
					  						ValidForQuerySection1 =  "ARPayment",
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
					 
					 						FieldName =  "InvoiceNumbers",
					  						ObjectTableName =  "ARPayment",
					  						FieldsDataType =  "Text",
					  						Code =  "InvoiceNumbers",
					  						MaxLength =  4000,
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
					  						PMPropertyPath =  "InvoiceNumbers",
					  						ListPropertyPath =  "InvoiceNumbers",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						FullFieldLable =  "InvoiceNumbers",
					  						DefaultText =  @"Invoice Numbers",
					  						FullLocalDefaultText =  @"מספרי חשבוניות לקוח",
					  						HelpTextCode =  "InvoiceNumbers",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TransferError",
					  						ObjectTableName =  "ARPayment",
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
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						FullLocalDefaultText =  @"שגיאת העברה",
					  						ListFieldLable =  "TransferErrorListLable",
					  						ListLableDefaultText =  @"Transfer Error",
					  						ListLocalDefaultText =  @"שגיאת העברה",
					  						HelpTextCode =  "TransferError",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TransferTries",
					  						ObjectTableName =  "ARPayment",
					  						FieldsDataType =  "Integer",
					  						Code =  "TransferTries",
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
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						FullFieldLable =  "TransferTries",
					  						DefaultText =  @"Transfer tries",
					  						FullLocalDefaultText =  @"נסיונות העברה",
					  						HelpTextCode =  "TransferTries",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsTransferStarted",
					  						ObjectTableName =  "ARPayment",
					  						FieldsDataType =  "Boolean",
					  						Code =  "IsTransferStarted",
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
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						FullFieldLable =  "IsTransferStarted",
					  						DefaultText =  @"Is Transfer Started",
					  						FullLocalDefaultText =  @"החלה העברה",
					  						HelpTextCode =  "IsTransferStarted",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TransferStatusCode",
					  						ObjectTableName =  "ARPayment",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "ARPaymentTransferStatus",
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
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						FullLocalDefaultText =  @"סטטוס העברה",
					  						HelpTextCode =  "TransferStatusCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TransferStatusName",
					  						ObjectTableName =  "ARPayment",
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
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						FullLocalDefaultText =  @"סטטוס העברה",
					  						ListFieldLable =  "TransferStatusName",
					  						ListLableDefaultText =  @"Transfer Status",
					  						ListLocalDefaultText =  @"סטטוס העברה",
					  						HelpTextCode =  "TransferStatusName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ReadyForTransfer",
					  						ObjectTableName =  "ARPayment",
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
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						FullLocalDefaultText =  @"מוכן להעברה",
					  						ListFieldLable =  "ReadyForTransfer",
					  						ListLableDefaultText =  @"Ready",
					  						ListLocalDefaultText =  @"מוכן להעברה",
					  						HelpTextCode =  "ReadyForTransfer",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "NotReadyPayments",
					  						ObjectTableName =  "ARPayment",
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
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						ObjectTableName =  "ARPayment",
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
					  						ValidForQuerySection1 =  "ARPayment",
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
					 
					 						FieldName =  "AccountingPaymentMethodId",
					  						ObjectTableName =  "ARPayment",
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
					  						ValidForQuerySection1 =  "ARPayment",
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
					 
					 						FieldName =  "BranchName",
					  						ObjectTableName =  "ARPayment",
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
					  						ValidForQuerySection1 =  "ARPayment",
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
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BankAccountName",
					  						ObjectTableName =  "ARPayment",
					  						FieldsDataType =  "Text",
					  						Code =  "BankAccountName",
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
					  						PMPropertyPath =  "BankAccountName",
					  						ListPropertyPath =  "BankAccountName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "ARPayment",
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
					  						FullFieldLable =  "BankAccountName",
					  						DefaultText =  @"Bank Account",
					  						FullLocalDefaultText =  @"חשבון בנק",
					  						ListFieldLable =  "BankAccountNameListLable",
					  						ListLableDefaultText =  @"Bank Account",
					  						ListLocalDefaultText =  @"חשבון בנק",
					  						HelpTextCode =  "BankAccountName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	    {  
	        FeatureRepository featureRepository = new FeatureRepository(0); 
            List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
            IWebFreightContext objectContext = WebFreightContext.GetContext(0);  
	        QueryGroup ARPaymentQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "ARPT", Name = "AR Payments" }, queryGroupRepository);
						QueryGroup ARPaymentQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "3f2e", Name = " Query Group" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable ARPaymentObjectTable = objectContext.ObjectTables.Where(d => d.Name == "ARPayment" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> ARPaymentObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "ARPayment").ToList();   

			   TextCode ARPaymentTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.Q.AllPayments", DefaultText = @"All Payments",LocalDefaultText = "כל הקבלות", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature ARPaymentFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ALLPAYMENTS", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARPayment.Features.AllPayments", NameTextCodeDefaultText = "All Payments", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode ARPaymentTextCode_1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.Q.OpenPayments", DefaultText = @"Open Payments",LocalDefaultText = "קבלות בסטטוס מאושר", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature ARPaymentFeature_1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OPENPAYMENTS", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARPayment.Features.OpenPayments", NameTextCodeDefaultText = "Open Payments", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode ARPaymentTextCode_2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.Q.DraftPayments", DefaultText = @"Draft Payments",LocalDefaultText = "קבלות בסטטוס טיוטה", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature ARPaymentFeature_2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DRAFTPAYMENTS", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARPayment.Features.DraftPayments", NameTextCodeDefaultText = "Draft Payments", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode ARPaymentTextCode_3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.Q.NotReadyPayments", DefaultText = @"Not Ready Payments",LocalDefaultText = "לא מוכן תשלומים", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature ARPaymentFeature_3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NOTREADYPAYMENTS", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARPayment.Features.NotReadyPayments", NameTextCodeDefaultText = "Not Ready Payments", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode ARPaymentTextCode_4 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.Q.MarkedAsBlockedForTransfer", DefaultText = @"Marked as blocked for transfer",LocalDefaultText = "מסומן כחסום לצורך העברה", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature ARPaymentFeature_4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MARKEDASBLOCKEDFORTRANSFER", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARPayment.Features.MarkedAsBlockedForTransfer", NameTextCodeDefaultText = "Marked as blocked for transfer", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode ARPaymentTextCode_5 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.Q.FailedSAT", DefaultText = @"SAT Failed Payments",LocalDefaultText = null, ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature ARPaymentFeature_5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SATFAILEDPAYMENTS", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARPayment.Features.SATFailedPayments", NameTextCodeDefaultText = "Payments Failed to Open in SAT", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode ARPaymentTextCode_6 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.Q.FailedSAT", DefaultText = @"SAT Failed Payments",LocalDefaultText = null, ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature ARPaymentFeature_6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SATFAILEDPAYMENTS", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARPayment.Features.SATFailedPayments", NameTextCodeDefaultText = "Payments Failed to Open in SAT", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query AllPaymentsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = ARPaymentTextCode_0.Id, Code = "All Payments",  QueryGroupCode = "ARPT", IndexOrder = 0, Tenant = 0, ObjectTableId = ARPaymentObjectTable.Id, QuerySection = "ARPayment", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = ARPaymentFeature_0.Id, DefaultSortName = "PaymentNo", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
	
			 QueryColumn AllPaymentsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllPaymentsQuery.Id, IndexOrder = 0, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "PaymentNo" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllPaymentsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllPaymentsQuery.Id, IndexOrder = 1, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "BillToName" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllPaymentsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllPaymentsQuery.Id, IndexOrder = 2, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "RegisterDate" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllPaymentsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllPaymentsQuery.Id, IndexOrder = 3, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "PaymentCurrencyCode" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllPaymentsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllPaymentsQuery.Id, IndexOrder = 4, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllPaymentsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllPaymentsQuery.Id, IndexOrder = 5, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllPaymentsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllPaymentsQuery.Id, IndexOrder = 6, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllPaymentsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllPaymentsQuery.Id, IndexOrder = 7, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "PaymentMethodName" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllPaymentsQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllPaymentsQuery.Id, IndexOrder = 8, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "AmountInPaymentCurrency" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllPaymentsQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllPaymentsQuery.Id, IndexOrder = 9, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "OpenAmount" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllPaymentsQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllPaymentsQuery.Id, IndexOrder = 10, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "InternalNotes" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
  
	      

			  Query OpenPaymentsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = ARPaymentTextCode_1.Id, Code = "Open Payments",  QueryGroupCode = "ARPT", IndexOrder = 1, Tenant = 0, ObjectTableId = ARPaymentObjectTable.Id, QuerySection = "ARPayment", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = ARPaymentFeature_1.Id, DefaultSortName = "PaymentNo", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
	
			 QueryColumn OpenPaymentsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenPaymentsQuery.Id, IndexOrder = 0, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "PaymentNo" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenPaymentsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenPaymentsQuery.Id, IndexOrder = 1, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "BillToName" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenPaymentsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenPaymentsQuery.Id, IndexOrder = 2, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "RegisterDate" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenPaymentsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenPaymentsQuery.Id, IndexOrder = 3, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "PaymentCurrencyCode" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenPaymentsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenPaymentsQuery.Id, IndexOrder = 4, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenPaymentsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenPaymentsQuery.Id, IndexOrder = 5, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenPaymentsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenPaymentsQuery.Id, IndexOrder = 6, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenPaymentsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenPaymentsQuery.Id, IndexOrder = 7, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "PaymentMethodName" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenPaymentsQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenPaymentsQuery.Id, IndexOrder = 8, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "AmountInPaymentCurrency" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenPaymentsQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenPaymentsQuery.Id, IndexOrder = 9, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "OpenAmount" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenPaymentsQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenPaymentsQuery.Id, IndexOrder = 10, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "InternalNotes" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter OpenPaymentsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "OpenPayments" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true",PredefinedValue2 = null, QueryId = OpenPaymentsQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query DraftPaymentsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = ARPaymentTextCode_2.Id, Code = "Draft Payments",  QueryGroupCode = "ARPT", IndexOrder = 2, Tenant = 0, ObjectTableId = ARPaymentObjectTable.Id, QuerySection = "ARPayment", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = ARPaymentFeature_2.Id, DefaultSortName = "PaymentNo", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
	
			 QueryColumn DraftPaymentsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftPaymentsQuery.Id, IndexOrder = 0, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "PaymentNo" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftPaymentsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftPaymentsQuery.Id, IndexOrder = 1, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "BillToName" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftPaymentsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftPaymentsQuery.Id, IndexOrder = 2, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "RegisterDate" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftPaymentsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftPaymentsQuery.Id, IndexOrder = 3, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "PaymentCurrencyCode" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftPaymentsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftPaymentsQuery.Id, IndexOrder = 4, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftPaymentsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftPaymentsQuery.Id, IndexOrder = 5, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftPaymentsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftPaymentsQuery.Id, IndexOrder = 6, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftPaymentsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftPaymentsQuery.Id, IndexOrder = 7, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "PaymentMethodName" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftPaymentsQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftPaymentsQuery.Id, IndexOrder = 8, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "AmountInPaymentCurrency" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftPaymentsQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftPaymentsQuery.Id, IndexOrder = 9, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "OpenAmount" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DraftPaymentsQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DraftPaymentsQuery.Id, IndexOrder = 10, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "InternalNotes" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter DraftPaymentsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "DraftPayments" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true",PredefinedValue2 = null, QueryId = DraftPaymentsQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query NotReadyPaymentsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = ARPaymentTextCode_3.Id, Code = "Not Ready Payments",  EditWizardComponentPath = "./InvoiceModules/ARPayment/Components/NewEntity/ARPaymentTransferTemplate",
			   QueryGroupCode = "ARPT", IndexOrder = 3, Tenant = 0, ObjectTableId = ARPaymentObjectTable.Id, QuerySection = "ARPayment", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = ARPaymentFeature_3.Id, DefaultSortName = "PaymentNo", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
	
			 QueryColumn NotReadyPaymentsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotReadyPaymentsQuery.Id, IndexOrder = 0, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "RegisterDate" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn NotReadyPaymentsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotReadyPaymentsQuery.Id, IndexOrder = 1, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "PaymentNo" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn NotReadyPaymentsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotReadyPaymentsQuery.Id, IndexOrder = 2, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "BillToName" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 190 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn NotReadyPaymentsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotReadyPaymentsQuery.Id, IndexOrder = 3, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn NotReadyPaymentsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotReadyPaymentsQuery.Id, IndexOrder = 4, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "AmountInPaymentCurrency" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn NotReadyPaymentsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotReadyPaymentsQuery.Id, IndexOrder = 5, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "ReadyForTransfer" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 50 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn NotReadyPaymentsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = NotReadyPaymentsQuery.Id, IndexOrder = 6, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "TransferError" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 500 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter NotReadyPaymentsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "NotReadyPayments" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true",PredefinedValue2 = null, QueryId = NotReadyPaymentsQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query MarkedasblockedfortransferQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = ARPaymentTextCode_4.Id, Code = "Marked as blocked for transfer",  EditWizardComponentPath = "./InvoiceModules/ARPayment/Components/NewEntity/ARPaymentTransferTemplate",
			   QueryGroupCode = "ARPT", IndexOrder = 4, Tenant = 0, ObjectTableId = ARPaymentObjectTable.Id, QuerySection = "ARPayment", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = ARPaymentFeature_4.Id, DefaultSortName = "PaymentNo", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
	
			 QueryColumn MarkedasblockedfortransferQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MarkedasblockedfortransferQuery.Id, IndexOrder = 0, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "RegisterDate" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MarkedasblockedfortransferQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MarkedasblockedfortransferQuery.Id, IndexOrder = 1, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "PaymentNo" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MarkedasblockedfortransferQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MarkedasblockedfortransferQuery.Id, IndexOrder = 2, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "BillToName" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 190 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MarkedasblockedfortransferQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MarkedasblockedfortransferQuery.Id, IndexOrder = 3, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MarkedasblockedfortransferQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MarkedasblockedfortransferQuery.Id, IndexOrder = 4, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "AmountInPaymentCurrency" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MarkedasblockedfortransferQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MarkedasblockedfortransferQuery.Id, IndexOrder = 5, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "ReadyForTransfer" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 50 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MarkedasblockedfortransferQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MarkedasblockedfortransferQuery.Id, IndexOrder = 6, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "TransferError" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 500 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter MarkedasblockedfortransferQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "MarkedAsBlockedForTransfer" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true",PredefinedValue2 = null, QueryId = MarkedasblockedfortransferQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);


             AdvancedQueryFilter MarkedasblockedfortransferQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "SATTransferStatusCode" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "TE",PredefinedValue2 = null, QueryId = MarkedasblockedfortransferQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query PaymentsFailedtoOpeninSATQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = ARPaymentTextCode_5.Id, Code = "Payments Failed to Open in SAT",  QueryGroupCode = "ARPT", IndexOrder = 5, Tenant = 0, ObjectTableId = ARPaymentObjectTable.Id, QuerySection = "ARPayment", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = ARPaymentFeature_5.Id, DefaultSortName = "PaymentNo", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
	
			 QueryColumn PaymentsFailedtoOpeninSATQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PaymentsFailedtoOpeninSATQuery.Id, IndexOrder = 0, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "PaymentNo" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn PaymentsFailedtoOpeninSATQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PaymentsFailedtoOpeninSATQuery.Id, IndexOrder = 1, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "BillToName" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn PaymentsFailedtoOpeninSATQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PaymentsFailedtoOpeninSATQuery.Id, IndexOrder = 2, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "RegisterDate" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn PaymentsFailedtoOpeninSATQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PaymentsFailedtoOpeninSATQuery.Id, IndexOrder = 3, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "PaymentCurrencyCode" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn PaymentsFailedtoOpeninSATQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PaymentsFailedtoOpeninSATQuery.Id, IndexOrder = 4, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn PaymentsFailedtoOpeninSATQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PaymentsFailedtoOpeninSATQuery.Id, IndexOrder = 5, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "ReadyForTransfer" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 50 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn PaymentsFailedtoOpeninSATQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PaymentsFailedtoOpeninSATQuery.Id, IndexOrder = 5, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn PaymentsFailedtoOpeninSATQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PaymentsFailedtoOpeninSATQuery.Id, IndexOrder = 6, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn PaymentsFailedtoOpeninSATQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PaymentsFailedtoOpeninSATQuery.Id, IndexOrder = 6, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "TransferError" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 500 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn PaymentsFailedtoOpeninSATQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PaymentsFailedtoOpeninSATQuery.Id, IndexOrder = 7, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "PaymentMethodName" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn PaymentsFailedtoOpeninSATQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PaymentsFailedtoOpeninSATQuery.Id, IndexOrder = 8, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "AmountInPaymentCurrency" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn PaymentsFailedtoOpeninSATQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PaymentsFailedtoOpeninSATQuery.Id, IndexOrder = 9, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "OpenAmount" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn PaymentsFailedtoOpeninSATQueryColumn_12 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PaymentsFailedtoOpeninSATQuery.Id, IndexOrder = 10, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "InternalNotes" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter PaymentsFailedtoOpeninSATQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "SATTransferStatusCode" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "TE",PredefinedValue2 = null, QueryId = PaymentsFailedtoOpeninSATQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query SATFailedPaymentsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = ARPaymentTextCode_6.Id, Code = "SAT Failed Payments",  QueryGroupCode = "ARPT", IndexOrder = 5, Tenant = 0, ObjectTableId = ARPaymentObjectTable.Id, QuerySection = "ARPayment", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = ARPaymentFeature_6.Id, DefaultSortName = "PaymentNo", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
	
			 QueryColumn SATFailedPaymentsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SATFailedPaymentsQuery.Id, IndexOrder = 0, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "RegisterDate" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn SATFailedPaymentsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SATFailedPaymentsQuery.Id, IndexOrder = 1, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "PaymentNo" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn SATFailedPaymentsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SATFailedPaymentsQuery.Id, IndexOrder = 2, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "BillToName" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 190 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn SATFailedPaymentsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SATFailedPaymentsQuery.Id, IndexOrder = 3, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn SATFailedPaymentsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SATFailedPaymentsQuery.Id, IndexOrder = 4, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "AmountInPaymentCurrency" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn SATFailedPaymentsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SATFailedPaymentsQuery.Id, IndexOrder = 5, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "ReadyForTransfer" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 50 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn SATFailedPaymentsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SATFailedPaymentsQuery.Id, IndexOrder = 6, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "TransferError" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 500 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter SATFailedPaymentsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "SATTransferStatusCode" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "TE",PredefinedValue2 = null, QueryId = SATFailedPaymentsQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable ARPaymentObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "ARPayment" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> ARPaymentObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "ARPayment").ToList();
		       
	      

	         Screen ARPaymentHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ARPayment.HeaderScreen", Name = "Header Screen", ObjectTableId = ARPaymentObjectTable.Id, NumberOfColumns = 5, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField ARPaymentARPaymentHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "PaymentNo").FirstOrDefault().Id, ScreenId = ARPaymentHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ARPaymentARPaymentHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "OpenAmount").FirstOrDefault().Id, ScreenId = ARPaymentHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ARPaymentARPaymentHeaderScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "StatusName").FirstOrDefault().Id, ScreenId = ARPaymentHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ARPaymentARPaymentHeaderScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "TransferStatusName").FirstOrDefault().Id, ScreenId = ARPaymentHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ARPaymentARPaymentHeaderScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 4, Row = 0, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "SATTransferStatusName").FirstOrDefault().Id, ScreenId = ARPaymentHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    ARPaymentObjectTable.HeaderScreenId = ARPaymentHeaderScreenScreen0.Id;
	   		  
	      

	         Screen ARPaymentGeneralTabScreenScreen1 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ARPayment.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = ARPaymentObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 6, IsReadOnly = false }, screensRepository, tenantScreens);
      
            ScreenField ARPaymentARPaymentGeneralTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "UpdatedByUserId").FirstOrDefault().Id, ScreenId = ARPaymentGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ARPaymentARPaymentGeneralTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "UpdateDate").FirstOrDefault().Id, ScreenId = ARPaymentGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField ARPaymentARPaymentGeneralTabScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ObjectFieldId = ARPaymentObjectFields.Where(d => d.FieldName == "BankAccountLiteId").FirstOrDefault().Id, ScreenId = ARPaymentGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
           

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {    
			 ObjectTable ARPaymentObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "ARPayment" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode ARPaymentDetailsTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.TH.Details", DefaultText = "Details",LocalDefaultText = "פרטים", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature ARPaymentDetailsFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DETAILS", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARPayment.Features.Details", NameTextCodeDefaultText = "Details", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode ARPaymentGeneralTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.TH.General", DefaultText = "General",LocalDefaultText = "כללי", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature ARPaymentGeneralFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GENERAL", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARPayment.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode ARPaymentDocsOutTextCode_TH2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.TH.Docs", DefaultText = "Docs Out",LocalDefaultText = "מסמכים שיצאו", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature ARPaymentDocsOutFeature_TH2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DOCSOUT", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARPayment.Features.DocsOut", NameTextCodeDefaultText = "Docs Out", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode ARPaymentDocsInTextCode_TH3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.TH.DocsIn", DefaultText = "Docs In",LocalDefaultText = "מסמכים", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature ARPaymentDocsInFeature_TH3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DOCSIN", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARPayment.Features.DocsIn", NameTextCodeDefaultText = "Docs In", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode ARPaymentTransferDetailsTextCode_TH4 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.TH.TransferDetails", DefaultText = "Transfer Details",LocalDefaultText = "נתוני העברה", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature ARPaymentTransferDetailsFeature_TH4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACCOUNTINGTRANSFER", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.AccountingTransfer", NameTextCodeDefaultText = "Accounting Transfer", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode ARPaymentEventsTextCode_TH5 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.TH.Events", DefaultText = "Events",LocalDefaultText = "אירועים", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature ARPaymentEventsFeature_TH5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EVENTS", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARPayment.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode ARPaymentCommunicationTextCode_TH6 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.TH.Communications", DefaultText = "Communication",LocalDefaultText = "תקשורת", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature ARPaymentCommunicationFeature_TH6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "COMMUNICATION", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARPayment.Features.Communication", NameTextCodeDefaultText = "Communication", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "ARPD",HtmlComponentName = "",HtmlComponentUrl = "./InvoiceModules/ARPayment/Components/EditTabs/ARPaymentDetailsTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "DETAILS" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.InvoiceLib.Views.Tabs.ARPaymentsTabs.ARPaymentsDetailsTabControl", ObjectTableId = ARPaymentObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "ARPayment.TH.Details" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "ARPG",HtmlComponentName = "",HtmlComponentUrl = "./InvoiceModules/ARPayment/Components/EditTabs/ARPaymentGeneralTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "GENERAL" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.Infrastructure.GeneralControls.GeneralTabControl", ObjectTableId = ARPaymentObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "ARPayment.TH.General" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "ARDO",HtmlComponentName = "",HtmlComponentUrl = "./InvoiceModules/ARPayment/Components/EditTabs/ARPaymentDocsOutTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "DOCSOUT" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.InvoiceLib.Views.Tabs.ARPaymentsTabs.ARPaymentDocsOutTabControl", ObjectTableId = ARPaymentObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "ARPayment.TH.Docs" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 2 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "ARDI",HtmlComponentName = "",HtmlComponentUrl = "./InvoiceModules/ARPayment/Components/EditTabs/ARPaymentDocsInTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "DOCSIN" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.InvoiceLib.Views.Tabs.ARPaymentsTabs.ARPaymentsDocsInTabControl", ObjectTableId = ARPaymentObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "ARPayment.TH.DocsIn" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 3 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "ARPT",HtmlComponentName = "",HtmlComponentUrl = "./InvoiceModules/ARPayment/Components/EditTabs/ARPaymentTransferTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "ACCOUNTINGTRANSFER" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.InvoiceLib.Views.Tabs.ARPaymentsTabs.ARPaymentTransferTabControl", ObjectTableId = ARPaymentObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "ARPayment.TH.TransferDetails" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 4 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "ARPE",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = tenantFeatures.Where(d => d.Code == "EVENTS" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = ARPaymentObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "ARPayment.TH.Events" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 5 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "ARPM",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = tenantFeatures.Where(d => d.Code == "COMMUNICATION" && d.ObjectTableId == ARPaymentObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.Infrastructure.Views.Communications.CommunicationsControl", ObjectTableId = ARPaymentObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "ARPayment.TH.Communications" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 5 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable ARPaymentObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "ARPayment" && d.Tenant == 0).FirstOrDefault(); 
		   Feature ARPaymentFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARPayment.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature ARPaymentFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARPayment.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature ARPaymentFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARPayment.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature ARPaymentFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARPayment.Features.PackageFeature", NameTextCodeDefaultText = "ARPayment Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes); 

		   		   //--------------> Additional Features <--------------\\

		   Feature ARPaymentFeature_RecalculateExternals = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "RecalculateExternals", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARPayment.Features.RecalculateExternals", NameTextCodeDefaultText = @"Recalculate External IDs" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature ARPaymentFeature_EnableMultiCurrency = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EnableMultiCurrency", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARPayment.Features.EnableMultiCurrency", NameTextCodeDefaultText = @"Enable multi-currency" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature ARPaymentFeature_ARPaymentEditExchangeRate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ARPaymentEditExchangeRate", FeatureTypeCode = "ACT", Packagable = false, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARPayment.Features.EditExchangeRate", NameTextCodeDefaultText = @"Edit Exchange Rate" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

   
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable ARPaymentObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "ARPayment" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "R2CB",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "המחאה הוצאה מהפקדה",
                EnglishName =  "Returned to Cashbook",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = ARPaymentObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "R2CS",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "המחאה הוצאה מהפקדה והוחזרה ללקוח",
                EnglishName =  "Returned to Customer",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = ARPaymentObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "UPPY",
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
                ObjectTableId = ARPaymentObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CRPY",
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
                ObjectTableId = ARPaymentObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "ARPA",
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
                ObjectTableId = ARPaymentObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "ARPC",
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
                ObjectTableId = ARPaymentObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "ARPV",
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
                ObjectTableId = ARPaymentObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CNAR",
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
                ObjectTableId = ARPaymentObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
		   FeatureRepository featureRepository = new FeatureRepository(0); 
		   List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList(); 
		   ObjectTable ARPaymentObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "ARPayment" && d.Tenant == 0).FirstOrDefault();       
    
			   Feature ARPaymentFeature_MB0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "APPROVE", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARPayment.Features.Approve", NameTextCodeDefaultText = "Approve", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

      
    
			   Feature ARPaymentFeature_MB1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PRINT", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARPayment.Features.Print", NameTextCodeDefaultText = "Print", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

      
    
			   Feature ARPaymentFeature_MB2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CHECKSATSTATUS", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARPayment.Features.CheckSATStatus", NameTextCodeDefaultText = "Check SAT Status", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

      
    
			   Feature ARPaymentFeature_MB3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Actions", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARPayment.Features.More", NameTextCodeDefaultText = "More", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

			   Feature ARPaymentFeature_MB30 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CANCELAPPROVAL", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARPayment.Features.CancelApproval", NameTextCodeDefaultText = "Cancel Approval", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
             			   Feature ARPaymentFeature_MB31 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EnableReTransfer", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARPayment.Features.EnableReTransfer", NameTextCodeDefaultText = "Enable accounting re-transfer", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
             			   Feature ARPaymentFeature_MB32 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "VOID", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARPayment.Features.Void", NameTextCodeDefaultText = "Void", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
             			   Feature ARPaymentFeature_MB33 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SENDToSAT", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARPayment.Features.SendToSAT", NameTextCodeDefaultText = "Send to SAT", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
             			   Feature ARPaymentFeature_MB34 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SendToQBO", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, NameTextCodeCode = "ARPayment.Features.SendToQBO", NameTextCodeDefaultText = "Send To QBO", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
              

		   TextCodeRepository.SubmitChanges();
		   FeaturesRepository.SubmitChanges();
		   MenuButtonGroup ARPaymentMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
				{
					MenuButtonGroupType = "ARPaymentEdit",
					Name = "ARPaymentEditButtonsGroup",
					ObjectTableId = ARPaymentObjectTable.Id,
					Tenant = 0
				}, menuButtonGroupRepository, tenantMenuButtonGroups);        
   
			   MenuButton ARPaymentMenuButton0 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "ApproveARPayment",
						Index = 0, 
						IsActive = true,
						LabelTextCodeCode = "ARPayment.B.Approve",
						LabelTextCodeDefaultText = "Approve",
						Tenant = 0,
						MenuButtonGroupId = ARPaymentMenuButtonGroup.Id,
						ObjectTableId = ARPaymentObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = ARPaymentFeature_MB0.Id,
						Style = "ApproveButtonStyle",
						LocalDefaultText = "אישור",
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton ARPaymentMenuButton1 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "PrintARPayment",
						Index = 1, 
						IsActive = true,
						LabelTextCodeCode = "ARPayment.B.Print",
						LabelTextCodeDefaultText = "Print",
						Tenant = 0,
						MenuButtonGroupId = ARPaymentMenuButtonGroup.Id,
						ObjectTableId = ARPaymentObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = ARPaymentFeature_MB1.Id,
						Style = null,
						LocalDefaultText = "הדפסה",
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton ARPaymentMenuButton2 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "CheckSATStatus",
						Index = 2, 
						IsActive = true,
						LabelTextCodeCode = "ARPayment.B.CheckSATStatus",
						LabelTextCodeDefaultText = "Check SAT Status",
						Tenant = 0,
						MenuButtonGroupId = ARPaymentMenuButtonGroup.Id,
						ObjectTableId = ARPaymentObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = ARPaymentFeature_MB2.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton ARPaymentMenuButton3 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "Actions",
						Index = 3, 
						IsActive = true,
						LabelTextCodeCode = "ARPayment.B.Actions",
						LabelTextCodeDefaultText = "More",
						Tenant = 0,
						MenuButtonGroupId = ARPaymentMenuButtonGroup.Id,
						ObjectTableId = ARPaymentObjectTable.Id,
						MenuButtonType = "dropdownbutton",
						FeatureId = ARPaymentFeature_MB3.Id,
						Style = null,
						LocalDefaultText = "Send To QBO",
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);

			   MenuButton ARPaymentMenuButton30 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "CancelApproval",
						Index = 0, 
						IsActive = true,
						LabelTextCodeCode = "ARPayment.B.CancelApproval",
						LabelTextCodeDefaultText = "Cancel Approval",
						Tenant = 0,
						MenuButtonGroupId = ARPaymentMenuButtonGroup.Id,
						ParentMenuButtonId = ARPaymentMenuButton3.Id,
						ObjectTableId = ARPaymentObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  ARPaymentFeature_MB30.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton ARPaymentMenuButton31 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "ReTransfer",
						Index = 1, 
						IsActive = true,
						LabelTextCodeCode = "ARPayment.B.ReTransfer",
						LabelTextCodeDefaultText = "Enable accounting re-transfer",
						Tenant = 0,
						MenuButtonGroupId = ARPaymentMenuButtonGroup.Id,
						ParentMenuButtonId = ARPaymentMenuButton3.Id,
						ObjectTableId = ARPaymentObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  ARPaymentFeature_MB31.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton ARPaymentMenuButton32 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "VoidARPaymentOperationsSeparator",
						Index = 2, 
						IsActive = true,
						LabelTextCodeCode = "ARPayment.B.VoidARPaymentOperationsSeparator",
						LabelTextCodeDefaultText = "",
						Tenant = 0,
						MenuButtonGroupId = ARPaymentMenuButtonGroup.Id,
						ParentMenuButtonId = ARPaymentMenuButton3.Id,
						ObjectTableId = ARPaymentObjectTable.Id,
						MenuButtonType = "separator",
						
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton ARPaymentMenuButton33 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "VoidARPayemnt",
						Index = 3, 
						IsActive = true,
						LabelTextCodeCode = "ARPayment.B.Void",
						LabelTextCodeDefaultText = "Void",
						Tenant = 0,
						MenuButtonGroupId = ARPaymentMenuButtonGroup.Id,
						ParentMenuButtonId = ARPaymentMenuButton3.Id,
						ObjectTableId = ARPaymentObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  ARPaymentFeature_MB32.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton ARPaymentMenuButton34 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "SENDToSAT",
						Index = 4, 
						IsActive = true,
						LabelTextCodeCode = "ARPayment.B.SENDToSAT",
						LabelTextCodeDefaultText = "Send to SAT",
						Tenant = 0,
						MenuButtonGroupId = ARPaymentMenuButtonGroup.Id,
						ParentMenuButtonId = ARPaymentMenuButton3.Id,
						ObjectTableId = ARPaymentObjectTable.Id,
						MenuButtonType = "button",
						FeatureId=  ARPaymentFeature_MB33.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton ARPaymentMenuButton35 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "SendToQBO",
						Index = 5, 
						IsActive = true,
						LabelTextCodeCode = "ARPayment.B.SendToQBO",
						LabelTextCodeDefaultText = "Send To QBO",
						Tenant = 0,
						MenuButtonGroupId = ARPaymentMenuButtonGroup.Id,
						ParentMenuButtonId = ARPaymentMenuButton3.Id,
						ObjectTableId = ARPaymentObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  ARPaymentFeature_MB34.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable ARPaymentObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "ARPayment" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode ARPaymentTextCode_ARPaymentMNoChequeCashBookCurr = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.M.NoChequeCashBookCurr", DefaultText = "There is no cheque cash book in",LocalDefaultText = @"אין שיק מזומן במאגר", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentMPaymentInvoicesHaveErrors = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.M.PaymentInvoicesHaveErrors", DefaultText = "Payment Invoices have errors",LocalDefaultText = @" יש שגיאה בתשלומי חשבוניות", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPayment = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment", DefaultText = "A/R Payment",LocalDefaultText = @"קבלת לקוח", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "T", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentMCantSetZeroAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.M.CantSetZeroAmount", DefaultText = "Can't set Payment Amount to Zero",LocalDefaultText = @"לא ניתן להגדיר סכום לתשלום בערך אפס", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentMCantSetMinusAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.M.CantSetMinusAmount", DefaultText = "Can't set Payment Amount to minus amount",LocalDefaultText = @"לא ניתן להגדיר סכום לתשלום בערך שלילי", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentMCantSetFutureDatePayment = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.M.CantSetFutureDatePayment", DefaultText = "Can't create payment with future date",LocalDefaultText = @"לא ניתן ליצור תשלום עם תאריך עתידי", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentMPaymentInvoicesHasErrors = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.M.PaymentInvoicesHasErrors", DefaultText = "Payment Invoices has errors",LocalDefaultText = null, ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentMPaymentAmountPaidCantBeMinus = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.M.PaymentAmountPaidCantBeMinus", DefaultText = "Payment Amount Paid can't be negative",LocalDefaultText = @"לא ניתן להגדיר סכום לתשלום בערך שלילי", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentMPaymentAmountPaidCantBeBigger = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.M.PaymentAmountPaidCantBeBigger", DefaultText = "Payment Amount Paid can't be bigger than Payment Amount",LocalDefaultText = @"סכום התשלום ששולם לא יכול להיות גדול מסכום התשלום ", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentMAccountingSettingsDontAllowVoid = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.M.AccountingSettingsDontAllowVoid", DefaultText = "Accounting Settings doesn't allow void A/R Payment",LocalDefaultText = @"ע''פ הגדרות מערכת הנהח''ש לא ניתן להתעלם מקבלה", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentMDisconnectInvoices = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.M.DisconnectInvoices", DefaultText = "Please disconnect all invoices",LocalDefaultText = @"אנא נתק את כל החשבוניות המקושרות", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentMConfirmVoid = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.M.ConfirmVoid", DefaultText = "Once you void or delete a payment, the change is permanent. If you void or delete a payment and want to restore it later, you'll have to create a new payment.",LocalDefaultText = @"האם אתה בטוח שברצונך לבטל את הקבלה ?", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentMNoTemplate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.M.NoTemplate", DefaultText = "A/R Payment document has no template!",LocalDefaultText = @"אין תבנית הדפסה למסמך קבלה", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentMOnlyMinusValue = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.M.OnlyMinusValue", DefaultText = "Only negative value allowed",LocalDefaultText = @"מותר רק ערך שלילי", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentMAmountPaidNotLess = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.M.AmountPaidNotLess", DefaultText = "Amount Paid can't be less than",LocalDefaultText = @"הסכום ששולם אינו יכול להיות נמוך מ", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentMCantPayMinusValue = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.M.CantPayMinusValue", DefaultText = "Can't pay minus value",LocalDefaultText = @"''לא ניתן לשלם ערך שלילי או לא ניתן לשלם סכום שלילי (לא תרגום מדוייק אבל יותר נכון)''", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentMAmountPaidLessOrEqual = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.M.AmountPaidLessOrEqual", DefaultText = "Amount Paid must be less than or equal to",LocalDefaultText = @"הסכום ששולם חייב להיות קטן או שווה ל", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentSDetailsDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.S.Details.Details", DefaultText = "Details",LocalDefaultText = @"פרטים", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentSDetailsRate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.S.Details.Rate", DefaultText = "Rate",LocalDefaultText = @"שער", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentSDetailsCurrencyDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.S.Details.CurrencyDetails", DefaultText = "Currency Details",LocalDefaultText = @"נתוני מטבע", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentSDetailsNotMatched = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.S.Details.NotMatched", DefaultText = "Not Matched",LocalDefaultText = @"לא מותאם", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentSDetailsTotals = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.S.Details.Totals", DefaultText = "Totals",LocalDefaultText = @"סה''כ", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentSDetailsARInvoices = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.S.Details.ARInvoices", DefaultText = "A/R Invoices",LocalDefaultText = @"חשבונית לקוח", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentSDetailsPaymentRef = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.S.Details.PaymentRef", DefaultText = "Payment Ref",LocalDefaultText = @"אסמכתא קבלה", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentSDetailsChequeRef = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.S.Details.ChequeRef", DefaultText = "Cheque Ref",LocalDefaultText = @"אסמכתא המחאה", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentSDetailsAmountToPay = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.S.Details.AmountToPay", DefaultText = "Amount To Pay",LocalDefaultText = @"סכום לתשלום", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentSDetailsCheque = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.S.Details.Cheque", DefaultText = "Cheque",LocalDefaultText = @"המחאה", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentSDetailsBankTransfer = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.S.Details.BankTransfer", DefaultText = "Bank Transfer",LocalDefaultText = @"העברה בנקאית", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentSDetailsCreditCard = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.S.Details.CreditCard", DefaultText = "Credit Card",LocalDefaultText = @"כרטיס זכות", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentSNoInvoicesFound = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.S.NoInvoicesFound", DefaultText = "No Invoices Found",LocalDefaultText = @"לא נמצאו חשבוניות", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentMARPaymentCashbook = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.M.ARPaymentCashbook", DefaultText = "There is no appropriate Cashbook for this payment, You have to create one",LocalDefaultText = @"לא קיימת קופה המתאימה למטבע הקבלה , יש לקים קופה למטבע זה", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentMBillToGLAccount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.M.BillToGLAccount", DefaultText = "The bill to doesn’t have GLAccount connected to it",LocalDefaultText = @"הלקוח לא מקושר לכרטיס הנה''ח", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentMBillToGLAccountCurrency = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.M.BillToGLAccountCurrency", DefaultText = "The ARPayment Currency does not match the bill to GLAccount Currency ",LocalDefaultText = @"מטבע הקבלה לא תואם את מטבע כרטיס הנה''ח של הלקוח", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentMBanckAccountGLAccount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.M.BanckAccountGLAccount", DefaultText = "The currency of the bank account GLAccount (%) is different from ARPayment curreny",LocalDefaultText = @"המטבע של כרטיס הבנק (%) לא תואם את מטבע הקבלה", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentMClosedMonth = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.M.ClosedMonth", DefaultText = "Closed Month",LocalDefaultText = @"חודש סגור", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentMValueDateCantBeFutureDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.M.ValueDateCantBeFutureDate", DefaultText = "Value date can't be future date",LocalDefaultText = @"תאריך ערך לא יכול להיות תאריך עתידי", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentMSearchByMsg = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.M.SearchByMsg", DefaultText = "Search by Invoice #/ Bill To",LocalDefaultText = @"חפש לפי חשבונית", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentCHShipmentNo = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.CH.ShipmentNo", DefaultText = "Shipment #",LocalDefaultText = @"משלוח", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentSShortTitle = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.S.ShortTitle", DefaultText = "A/R Payment",LocalDefaultText = @"קבלה", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentMARpaymentValueHigherThanCashbookValue = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.M.ARpaymentValueHigherThanCashbookValue", DefaultText = "ARpayment Value is higher than Cashbook Value",LocalDefaultText = @"לא ניתן לבטל קבלה - סכום הקבלה גדול מהיתרה בקופה", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode ARPaymentTextCode_ARPaymentMCANTCancelARPayment = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ARPayment.M.CANTCancelARPayment", DefaultText = "Can’t Cancel this ARPayment because at least one of the cheques is deposited or redeemed, return these cheques to the cashbook and cancel the external reconciliation ",LocalDefaultText = @"לא ניתן לבטל את הקבלה, חלק מההמחאות הופקדו/נפרעו כבר, יש להחזיר את ההמחאות לקופה ולבטל את ההתאמה החיצונית ", ObjectTableId = ARPaymentObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 