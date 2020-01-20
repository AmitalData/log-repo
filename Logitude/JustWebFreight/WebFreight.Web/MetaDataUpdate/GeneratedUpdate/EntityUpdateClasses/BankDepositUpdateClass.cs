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

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.EntityUpdateClasses
{
   public class BankDepositUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "BankDeposit",
			      				    IsNew =  false,
			      				    DBTableName =  "BankDeposits",
			      				    OldDBTableName =  "BankDeposits",
			      				    ObjectTableSingular =  "BankDeposit",
			      				    ObjectTablePlural =  "BankDeposits",
			      				    HasCustomFilter =  false,
			      				    HasCustomFields =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  true,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  false,
			      				    IsNewWizard =  true,
			      				    LookUp1 =  "DepositNumber",
			      				    LookUp2 =  "DepositDate",
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
			      				    SortingByObjectField =  "DepositNumber",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  false,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    NewWizardControlName =  "NewBankDepositComponent",
			      				    LocalDefaultText =  "הפקדה",
			      				    DefaultText =  "Bank Deposit",
			      				    Code =  "BNKD",
			      				    Name =  "BankDeposit Query Group",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Accounting",
			      				    NewWizardComponentPath =  "./Accounting/Components/NewEntity/NewBankDepositComponent",
			      				    NoTS =  false,
			      				    HasMenuButtons =  true,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    IsTabsHidden =  false,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreateDate",
					  						OldFieldName =  "CreateDate",
					  						ObjectTableName =  "BankDeposit",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  true,
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
					  						ValidForQuerySection1 =  "BankDeposit",
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
					  						FullLocalDefaultText =  "תאריך יצירה",
					  						ListFieldLable =  "CreateDateListLable",
					  						ListLableDefaultText =  "Create Date",
					  						ListLocalDefaultText =  "תאריך יצירה",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  true,
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
					 
					 						FieldName =  "CreatedByUserId",
					  						OldFieldName =  "CreatedByUserId",
					  						ObjectTableName =  "BankDeposit",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
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
					  						ValidForQuerySection1 =  "BankDeposit",
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
					  						FullLocalDefaultText =  "יוצר",
					  						ListFieldLable =  "CreatedByUserIdListLable",
					  						ListLableDefaultText =  "Created By",
					  						ListLocalDefaultText =  "יוצר ההפקדה",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  true,
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
					 
					 						FieldName =  "UpdateDate",
					  						OldFieldName =  "UpdateDate",
					  						ObjectTableName =  "BankDeposit",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  true,
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
					  						ValidForQuerySection1 =  "BankDeposit",
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
					  						FullLocalDefaultText =  "תאריך עדכון",
					  						ListFieldLable =  "UpdateDateListLable",
					  						ListLableDefaultText =  "Update Date",
					  						ListLocalDefaultText =  "תאריך עדכון",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  true,
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
					 
					 						FieldName =  "UpdatedByUserId",
					  						OldFieldName =  "UpdatedByUserId",
					  						ObjectTableName =  "BankDeposit",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
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
					  						FullLocalDefaultText =  "עודכן ע''י משתמש",
					  						ListFieldLable =  "UpdatedByUserIdListLable",
					  						ListLableDefaultText =  "Updated By",
					  						ListLocalDefaultText =  "עודכן ע''י משתמש",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  true,
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
					  						ObjectTableName =  "BankDeposit",
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
					  						ValidForQuerySection1 =  "BankDeposit",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SearchFields",
					  						DefaultText =  "Deposit No. / Cheque No.",
					  						FullLocalDefaultText =  "מספר הפקדה / המחאה",
					  						ListFieldLable =  "SearchFieldsListLable",
					  						ListLableDefaultText =  "Deposit No. / Cheque No.",
					  						ListLocalDefaultText =  "מספר הפקדה / המחאה",
					  						IsMaxLength =  true,
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
					 
					 						FieldName =  "DepositNumber",
					  						OldFieldName =  "DepositNumber",
					  						ObjectTableName =  "BankDeposit",
					  						FieldsDataType =  "Integer",
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  true,
					  						PMPropertyPath =  "DepositNumber",
					  						ListPropertyPath =  "DepositNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "BankDeposit",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DepositNumber",
					  						DefaultText =  "Deposit No.",
					  						FullLocalDefaultText =  "מספר הפקדה",
					  						ListFieldLable =  "DepositNumberListLable",
					  						ListLableDefaultText =  "Deposit No.",
					  						ListLocalDefaultText =  "מספר הפקדה",
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
					 
					 						FieldName =  "DepositDate",
					  						OldFieldName =  "DepositDate",
					  						ObjectTableName =  "BankDeposit",
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  true,
					  						PMPropertyPath =  "DepositDate",
					  						ListPropertyPath =  "DepositDate",
					  						DisplayInLookUpIndex =  1,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  1,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "BankDeposit",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DepositDate",
					  						DefaultText =  "Deposit Date",
					  						FullLocalDefaultText =  "תאריך חשבונאי",
					  						ListFieldLable =  "DepositDateListLable",
					  						ListLableDefaultText =  "Deposit Date",
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
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DepositCurrencyId",
					  						OldFieldName =  "DepositCurrencyId",
					  						ObjectTableName =  "BankDeposit",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Currency",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  true,
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
					  						PMPropertyPath =  "DepositCurrencyId",
					  						ListPropertyPath =  "DepositCurrencyId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "BankDeposit",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DepositCurrencyId",
					  						DefaultText =  "Deposit Currency",
					  						FullLocalDefaultText =  "מטבע הפקדה",
					  						ListFieldLable =  "DepositCurrencyIdListLable",
					  						ListLableDefaultText =  "Deposit Currency",
					  						ListLocalDefaultText =  "מטבע הפקדה",
					  						IsMaxLength =  false,
					  						IsFixedLength =  true,
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
					 
					 						FieldName =  "LocalDepositAmount",
					  						OldFieldName =  "LocalDepositAmount",
					  						ObjectTableName =  "BankDeposit",
					  						FieldsDataType =  "Decimal",
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
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "LocalDepositAmount",
					  						ListPropertyPath =  "LocalDepositAmount",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "BankDeposit",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  16,
					  						DigitsAfterPoint =  2,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LocalDepositAmount",
					  						DefaultText =  "Local  Amount",
					  						FullLocalDefaultText =  "סכום הפקדה במטבע מקומי",
					  						ListFieldLable =  "LocalDepositAmountListLable",
					  						ListLableDefaultText =  "Local  Amount",
					  						ListLocalDefaultText =  "סכום הפקדה במטבע מקומי",
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
					 
					 						FieldName =  "ForeignAmount",
					  						OldFieldName =  "ForeignAmount",
					  						ObjectTableName =  "BankDeposit",
					  						FieldsDataType =  "Decimal",
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
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ForeignAmount",
					  						ListPropertyPath =  "ForeignAmount",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "BankDeposit",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  16,
					  						DigitsAfterPoint =  2,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ForeignAmount",
					  						DefaultText =  "Foreign Amount",
					  						FullLocalDefaultText =  "סכום הפקדה במטבע זר",
					  						ListFieldLable =  "ForeignAmountListLable",
					  						ListLableDefaultText =  "Foreign Amount",
					  						ListLocalDefaultText =  "סכום הפקדה במטבע זר",
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
					 
					 						FieldName =  "DepositBankAccountId",
					  						OldFieldName =  "DepositBankAccountId",
					  						ObjectTableName =  "BankDeposit",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "BankAccount",
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DepositBankAccountId",
					  						ListPropertyPath =  "DepositBankAccountId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "BankDeposit",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DepositBankAccountId",
					  						DefaultText =  "Bank Account",
					  						FullLocalDefaultText =  "בנק להפקדה",
					  						ListFieldLable =  "DepositBankAccountIdListLable",
					  						ListLableDefaultText =  "Bank Account",
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
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CashBookId",
					  						OldFieldName =  "CashBookId",
					  						ObjectTableName =  "BankDeposit",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "CashBook",
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CashBookId",
					  						ListPropertyPath =  "CashBookId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "BankDeposit",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CashBookId",
					  						DefaultText =  "Cash Book",
					  						FullLocalDefaultText =  "קופה",
					  						ListFieldLable =  "CashBookIdListLable",
					  						ListLableDefaultText =  "Cash Book",
					  						ListLocalDefaultText =  "קופה",
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
					 
					 						FieldName =  "AccountingDate",
					  						OldFieldName =  "AccountingDate",
					  						ObjectTableName =  "BankDeposit",
					  						FieldsDataType =  "Date",
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
					  						IsCustomFilter =  false,
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
					  						ValidForQuerySection1 =  "BankDeposit",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
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
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BankDepositLines",
					  						OldFieldName =  "BankDepositLines",
					  						ObjectTableName =  "BankDeposit",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "BankDepositLines",
					  						ListPropertyPath =  "BankDepositLines",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  true,
					  						MultiTableName =  "BankDepositLine",
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
					  						FullFieldLable =  "BankDepositLines",
					  						DefaultText =  "Deposit Lines",
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
					 
					 						FieldName =  "CashBookGLAccountId",
					  						OldFieldName =  "CashBookGLAccountId",
					  						ObjectTableName =  "BankDeposit",
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
					  						PMPropertyPath =  "CashBookGLAccountId",
					  						ListPropertyPath =  "CashBookGLAccountId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "BankDeposit",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CashBookGLAccountId",
					  						DefaultText =  "Cash Book GL Account",
					  						FullLocalDefaultText =  "כרטיס קופה",
					  						ListFieldLable =  "CashBookGLAccountIdListLable",
					  						ListLableDefaultText =  "Cash Book GL Account ",
					  						ListLocalDefaultText =  "כרטיס קופה",
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
					 
					 						FieldName =  "IsCashDeposit",
					  						OldFieldName =  "IsCashDeposit",
					  						ObjectTableName =  "BankDeposit",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsCashDeposit",
					  						ListPropertyPath =  "IsCashDeposit",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "BankDeposit",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsCashDeposit",
					  						DefaultText =  "CashDeposit",
					  						FullLocalDefaultText =  " הפקדת מזומן",
					  						ListFieldLable =  "IsCashDepositListLable",
					  						ListLableDefaultText =  "CashDeposit",
					  						ListLocalDefaultText =  " הפקדת מזומן",
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
					 
					 						FieldName =  "DeferredGLAccountId",
					  						OldFieldName =  "DeferredGLAccountId",
					  						ObjectTableName =  "BankDeposit",
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
					  						PMPropertyPath =  "DeferredGLAccountId",
					  						ListPropertyPath =  "DeferredGLAccountId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "BankDeposit",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DeferredGLAccountId",
					  						DefaultText =  "Deferred GL Account",
					  						ListFieldLable =  "DeferredGLAccountIdListLable",
					  						ListLableDefaultText =  "Deferred GL Account",
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
					 
					 						FieldName =  "CashGLAccountId",
					  						OldFieldName =  "CashGLAccountId",
					  						ObjectTableName =  "BankDeposit",
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
					  						PMPropertyPath =  "CashGLAccountId",
					  						ListPropertyPath =  "CashGLAccountId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "BankDeposit",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CashGLAccountId",
					  						DefaultText =  "Cash GL Account ",
					  						ListFieldLable =  "CashGLAccountIdListLable",
					  						ListLableDefaultText =  "Cash GL Account",
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
					 
					 						FieldName =  "IsCanceled",
					  						OldFieldName =  "IsCanceled",
					  						ObjectTableName =  "BankDeposit",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsCanceled",
					  						ListPropertyPath =  "IsCanceled",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "BankDeposit",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsCanceled",
					  						DefaultText =  "Canceled",
					  						FullLocalDefaultText =  "מבוטלת",
					  						ListFieldLable =  "IsCanceledListLable",
					  						ListLableDefaultText =  "Canceled",
					  						ListLocalDefaultText =  "מבוטלת",
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
					 
					 						FieldName =  "DepositCurrencyCode",
					  						OldFieldName =  "DepositCurrencyCode",
					  						ObjectTableName =  "BankDeposit",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  50,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  50,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DepositCurrencyCode",
					  						ListPropertyPath =  "DepositCurrencyCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "BankDeposit",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DepositCurrencyCode",
					  						DefaultText =  "Currency",
					  						FullLocalDefaultText =  "מטבע",
					  						ListFieldLable =  "DepositCurrencyCodeListLable",
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
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "JournalNumber",
					  						OldFieldName =  "JournalNumber",
					  						ObjectTableName =  "BankDeposit",
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
					  						ValidForQuerySection1 =  "BankDeposit",
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
					  						FullLocalDefaultText =  "מספר פקודת יומן",
					  						ListFieldLable =  "JournalNumberListLable",
					  						ListLableDefaultText =  "Journal No.",
					  						ListLocalDefaultText =  "מספר פקודת יומן",
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
					  						ObjectTableName =  "BankDeposit",
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
					  						DefaultText =  "Journal ",
					  						FullLocalDefaultText =  "פקודת יומן",
					  						ListFieldLable =  "JournalIdListLable",
					  						ListLableDefaultText =  "Journal ",
					  						ListLocalDefaultText =  "פקודת יומן",
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
					 
					 						FieldName =  "CashBookName",
					  						OldFieldName =  "CashBookName",
					  						ObjectTableName =  "BankDeposit",
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
					  						SystemMaxLength =  60,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CashBookName",
					  						ListPropertyPath =  "CashBookName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "BankDeposit",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CashBookName",
					  						DefaultText =  "Cashbook",
					  						FullLocalDefaultText =  "קופה",
					  						ListFieldLable =  "CashBookNameListLable",
					  						ListLableDefaultText =  "Cashbook",
					  						ListLocalDefaultText =  "קופה",
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
					 
					 						FieldName =  "LastActivityDate",
					  						OldFieldName =  "LastActivityDate",
					  						ObjectTableName =  "BankDeposit",
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
					  						ValidForQuerySection1 =  "BankDeposit",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LastActivityDate",
					  						DefaultText =  "Last Date",
					  						FullLocalDefaultText =  "תאריך אחרון",
					  						ListFieldLable =  "LastActivityDateListLable",
					  						ListLableDefaultText =  "Last Date",
					  						ListLocalDefaultText =  "תאריך אחרון",
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
					 
					 						FieldName =  "LastActivityTypeName",
					  						OldFieldName =  "LastActivityTypeName",
					  						ObjectTableName =  "BankDeposit",
					  						FieldsDataType =  "Text",
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
					  						ValidForQuerySection1 =  "BankDeposit",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LastActivityTypeName",
					  						DefaultText =  "Last Activity Type Name",
					  						FullLocalDefaultText =  "סוג ישות",
					  						ListFieldLable =  "LastActivityTypeNameListLable",
					  						ListLableDefaultText =  "Last Activity Type Name",
					  						ListLocalDefaultText =  "סוג ישות",
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
					 
					 						FieldName =  "LastActivityByUserName",
					  						OldFieldName =  "LastActivityByUserName",
					  						ObjectTableName =  "BankDeposit",
					  						FieldsDataType =  "Text",
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
					  						ValidForQuerySection1 =  "BankDeposit",
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
					  						FullLocalDefaultText =  "מעדכן אחרון",
					  						ListFieldLable =  "LastActivityByUserNameListLable",
					  						ListLableDefaultText =  "LastActivityByUserName",
					  						ListLocalDefaultText =  "מעדכן אחרון",
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
					 
					 						FieldName =  "CreatedByUserName",
					  						OldFieldName =  "CreatedByUserName",
					  						ObjectTableName =  "BankDeposit",
					  						FieldsDataType =  "Text",
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
					  						ValidForQuerySection1 =  "BankDeposit",
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
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BankAccountNumber",
					  						OldFieldName =  "BankAccountNumber",
					  						ObjectTableName =  "BankDeposit",
					  						FieldsDataType =  "Text",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "BankAccountNumber",
					  						ListPropertyPath =  "BankAccountNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "BankAccount",
					  						ValidForQuerySection2 =  "BankDeposit",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "BankAccountNumber",
					  						DefaultText =  "Bank Account",
					  						FullLocalDefaultText =  "מספר חשבון בנק",
					  						ListFieldLable =  "BankAccountNumberListLable",
					  						ListLableDefaultText =  "Bank Account",
					  						ListLocalDefaultText =  "מספר חשבון בנק",
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
					 
					 						FieldName =  "JournalQueueId",
					  						OldFieldName =  "JournalQueueId",
					  						ObjectTableName =  "BankDeposit",
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
					  						PMPropertyPath =  "JournalQueueId",
					  						ListPropertyPath =  "JournalQueueId",
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
					  						FullFieldLable =  "JournalQueueId",
					  						DefaultText =  "Journal Queue Id",
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
	        QueryGroup BankDepositQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "BNKD", Name = "BankDeposit Query Group" }, queryGroupRepository);
						QueryGroup BankDepositQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "BNKD", Name = "BankDeposit" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable BankDepositObjectTable = objectContext.ObjectTables.Where(d => d.Name == "BankDeposit" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> BankDepositObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "BankDeposit").ToList();   

			   TextCode BankDepositTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BankDeposit.Q.AllBankDeposits", DefaultText = @"All Bank Deposits",LocalDefaultText = "כל פיקדונות הבנק", ObjectTableId = BankDepositObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature BankDepositFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BankDeposit.Q.AllBankDeposits", ObjectTableId = BankDepositObjectTable.Id, Tenant = 0, NameTextCodeCode = "BankDeposit.Features.AllBankDeposits", NameTextCodeDefaultText = "AllBankDeposits", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode BankDepositTextCode_1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BankDeposit.Q.Today", DefaultText = @"Today Deposits",LocalDefaultText = "הפקדות מהיום", ObjectTableId = BankDepositObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature BankDepositFeature_1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TodayBankDeposit", ObjectTableId = BankDepositObjectTable.Id, Tenant = 0, NameTextCodeCode = "BankDeposit.Features.todaydepo", NameTextCodeDefaultText = "Today Deposit", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode BankDepositTextCode_2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BankDeposit.Q.cash", DefaultText = @"Cash Deposit",LocalDefaultText = "הפקדות מזומן", ObjectTableId = BankDepositObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature BankDepositFeature_2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CashBankDeposit", ObjectTableId = BankDepositObjectTable.Id, Tenant = 0, NameTextCodeCode = "BankDeposit.Features.cashdepo", NameTextCodeDefaultText = "Cash Deposit", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode BankDepositTextCode_3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BankDeposit.Q.chequeDeposit", DefaultText = @"Cheque Deposit",LocalDefaultText = "הפקדות המחאות", ObjectTableId = BankDepositObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature BankDepositFeature_3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ChequeBankDeposit", ObjectTableId = BankDepositObjectTable.Id, Tenant = 0, NameTextCodeCode = "BankDeposit.Features.chequedepo", NameTextCodeDefaultText = "Cheque Deposit", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query AllBankDepositsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = BankDepositTextCode_0.Id, NameTextCodeCode = BankDepositTextCode_0.Code, ObjectTableName = "BankDeposit", Code = "AllBankDeposits",  QueryGroupCode = "8a96", IndexOrder = 0, Tenant = 0, ObjectTableId = BankDepositObjectTable.Id, QuerySection = "BankDeposit", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = BankDepositFeature_0.Id, DefaultSortName = "DepositNumber", DefaultSortDirection = "Desending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn AllBankDepositsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllBankDepositsQuery.Id,QueryCode = AllBankDepositsQuery.Code, IndexOrder = 0, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "DepositNumber" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "DepositNumber" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 137 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllBankDepositsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllBankDepositsQuery.Id,QueryCode = AllBankDepositsQuery.Code, IndexOrder = 1, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "CreatedByUserId" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "CreatedByUserId" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllBankDepositsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllBankDepositsQuery.Id,QueryCode = AllBankDepositsQuery.Code, IndexOrder = 2, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllBankDepositsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllBankDepositsQuery.Id,QueryCode = AllBankDepositsQuery.Code, IndexOrder = 3, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "DepositDate" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "DepositDate" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 116 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllBankDepositsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllBankDepositsQuery.Id,QueryCode = AllBankDepositsQuery.Code, IndexOrder = 4, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "AccountingDate" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "AccountingDate" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllBankDepositsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllBankDepositsQuery.Id,QueryCode = AllBankDepositsQuery.Code, IndexOrder = 5, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "LocalDepositAmount" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "LocalDepositAmount" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 135 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllBankDepositsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllBankDepositsQuery.Id,QueryCode = AllBankDepositsQuery.Code, IndexOrder = 6, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "DepositCurrencyCode" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "DepositCurrencyCode" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 142 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllBankDepositsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllBankDepositsQuery.Id,QueryCode = AllBankDepositsQuery.Code, IndexOrder = 7, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "DepositCurrencyId" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "DepositCurrencyId" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 124 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllBankDepositsQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllBankDepositsQuery.Id,QueryCode = AllBankDepositsQuery.Code, IndexOrder = 8, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "ForeignAmount" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "ForeignAmount" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 118 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllBankDepositsQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllBankDepositsQuery.Id,QueryCode = AllBankDepositsQuery.Code, IndexOrder = 9, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "DepositBankAccountId" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "DepositBankAccountId" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 147 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllBankDepositsQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllBankDepositsQuery.Id,QueryCode = AllBankDepositsQuery.Code, IndexOrder = 10, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 220 }, queryColumnsRepository, tenantQueryColumns);
  
	      

			  Query TodayDepositsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = BankDepositTextCode_1.Id, NameTextCodeCode = BankDepositTextCode_1.Code, ObjectTableName = "BankDeposit", Code = "TodayDeposits",  QueryGroupCode = "BNKD", IndexOrder = 1, Tenant = 0, ObjectTableId = BankDepositObjectTable.Id, QuerySection = "BankDeposit", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = BankDepositFeature_1.Id, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn TodayDepositsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayDepositsQuery.Id,QueryCode = TodayDepositsQuery.Code, IndexOrder = 0, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "DepositNumber" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "DepositNumber" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn TodayDepositsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayDepositsQuery.Id,QueryCode = TodayDepositsQuery.Code, IndexOrder = 1, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "DepositDate" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "DepositDate" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn TodayDepositsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayDepositsQuery.Id,QueryCode = TodayDepositsQuery.Code, IndexOrder = 2, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "LocalDepositAmount" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "LocalDepositAmount" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn TodayDepositsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayDepositsQuery.Id,QueryCode = TodayDepositsQuery.Code, IndexOrder = 3, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "DepositCurrencyCode" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "DepositCurrencyCode" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn TodayDepositsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayDepositsQuery.Id,QueryCode = TodayDepositsQuery.Code, IndexOrder = 4, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "ForeignAmount" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "ForeignAmount" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn TodayDepositsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayDepositsQuery.Id,QueryCode = TodayDepositsQuery.Code, IndexOrder = 5, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn TodayDepositsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayDepositsQuery.Id,QueryCode = TodayDepositsQuery.Code, IndexOrder = 6, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter TodayDepositsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "DepositDate" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "DepositDate" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "#today",PredefinedValue2 = "#today", QueryId = TodayDepositsQuery.Id,QueryCode = TodayDepositsQuery.Code, Tenant = 0,Operator = "Between"}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query cashDepositsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = BankDepositTextCode_2.Id, NameTextCodeCode = BankDepositTextCode_2.Code, ObjectTableName = "BankDeposit", Code = "cashDeposits",  QueryGroupCode = "BNKD", IndexOrder = 2, Tenant = 0, ObjectTableId = BankDepositObjectTable.Id, QuerySection = "BankDeposit", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = BankDepositFeature_2.Id, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn cashDepositsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = cashDepositsQuery.Id,QueryCode = cashDepositsQuery.Code, IndexOrder = 0, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "DepositNumber" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "DepositNumber" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn cashDepositsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = cashDepositsQuery.Id,QueryCode = cashDepositsQuery.Code, IndexOrder = 1, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "DepositDate" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "DepositDate" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn cashDepositsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = cashDepositsQuery.Id,QueryCode = cashDepositsQuery.Code, IndexOrder = 2, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "LocalDepositAmount" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "LocalDepositAmount" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn cashDepositsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = cashDepositsQuery.Id,QueryCode = cashDepositsQuery.Code, IndexOrder = 3, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "DepositCurrencyCode" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "DepositCurrencyCode" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn cashDepositsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = cashDepositsQuery.Id,QueryCode = cashDepositsQuery.Code, IndexOrder = 4, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "ForeignAmount" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "ForeignAmount" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn cashDepositsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = cashDepositsQuery.Id,QueryCode = cashDepositsQuery.Code, IndexOrder = 5, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn cashDepositsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = cashDepositsQuery.Id,QueryCode = cashDepositsQuery.Code, IndexOrder = 6, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter cashDepositsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "IsCashDeposit" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "IsCashDeposit" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "true",PredefinedValue2 = null, QueryId = cashDepositsQuery.Id,QueryCode = cashDepositsQuery.Code, Tenant = 0,Operator = "Equals"}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query chequeDepositQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = BankDepositTextCode_3.Id, NameTextCodeCode = BankDepositTextCode_3.Code, ObjectTableName = "BankDeposit", Code = "chequeDeposit",  QueryGroupCode = "BNKD", IndexOrder = 3, Tenant = 0, ObjectTableId = BankDepositObjectTable.Id, QuerySection = "BankDeposit", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = BankDepositFeature_3.Id, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn chequeDepositQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = chequeDepositQuery.Id,QueryCode = chequeDepositQuery.Code, IndexOrder = 0, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "DepositNumber" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "DepositNumber" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn chequeDepositQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = chequeDepositQuery.Id,QueryCode = chequeDepositQuery.Code, IndexOrder = 1, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "DepositDate" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "DepositDate" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn chequeDepositQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = chequeDepositQuery.Id,QueryCode = chequeDepositQuery.Code, IndexOrder = 2, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "LocalDepositAmount" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "LocalDepositAmount" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn chequeDepositQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = chequeDepositQuery.Id,QueryCode = chequeDepositQuery.Code, IndexOrder = 3, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "DepositCurrencyCode" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "DepositCurrencyCode" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn chequeDepositQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = chequeDepositQuery.Id,QueryCode = chequeDepositQuery.Code, IndexOrder = 4, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "ForeignAmount" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "ForeignAmount" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn chequeDepositQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = chequeDepositQuery.Id,QueryCode = chequeDepositQuery.Code, IndexOrder = 5, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "CreatedByUserName" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn chequeDepositQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = chequeDepositQuery.Id,QueryCode = chequeDepositQuery.Code, IndexOrder = 6, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter chequeDepositQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "IsCashDeposit" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "IsCashDeposit" && d.ObjectTableId == BankDepositObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "false",PredefinedValue2 = null, QueryId = chequeDepositQuery.Id,QueryCode = chequeDepositQuery.Code, Tenant = 0,Operator = "Equals"}, advancedQueryFiltersRepository, tenantAdvancedFilters);

	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable BankDepositObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "BankDeposit" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> BankDepositObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "BankDeposit").ToList();
		       
	      

	         Screen BankDepositBankDepositDetailsScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "BankDeposit.BankDepositDetailsScreen", Name = "BankDepositDetailsScreen", ObjectTableId = BankDepositObjectTable.Id, NumberOfColumns = 1, NumberOfRows = 1, IsReadOnly = false }, screensRepository, tenantScreens);
      
            ScreenField BankDepositBankDepositBankDepositDetailsScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "DepositNumber").FirstOrDefault().Id, ScreenId = BankDepositBankDepositDetailsScreenScreen0.Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "DepositNumber").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
           
	      

	         Screen BankDepositHeaderScreenScreen1 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "BankDeposit.HeaderScreen", Name = "HeaderScreen", ObjectTableId = BankDepositObjectTable.Id, NumberOfColumns = 4, NumberOfRows = 2, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField BankDepositHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "DepositNumber").FirstOrDefault().Id, ScreenId = BankDepositHeaderScreenScreen1.Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "DepositNumber").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField BankDepositHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "LocalDepositAmount").FirstOrDefault().Id, ScreenId = BankDepositHeaderScreenScreen1.Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "LocalDepositAmount").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField BankDepositHeaderScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "DepositDate").FirstOrDefault().Id, ScreenId = BankDepositHeaderScreenScreen1.Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "DepositDate").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField BankDepositHeaderScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "CashBookId").FirstOrDefault().Id, ScreenId = BankDepositHeaderScreenScreen1.Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "CashBookId").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField BankDepositHeaderScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "JournalId").FirstOrDefault().Id, ScreenId = BankDepositHeaderScreenScreen1.Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "JournalId").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField BankDepositHeaderScreenScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 1, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "IsCanceled").FirstOrDefault().Id, ScreenId = BankDepositHeaderScreenScreen1.Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "IsCanceled").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField BankDepositHeaderScreenScreenField6 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ObjectFieldId = BankDepositObjectFields.Where(d => d.FieldName == "BankAccountNumber").FirstOrDefault().Id, ScreenId = BankDepositHeaderScreenScreen1.Id, ObjectFieldCode = BankDepositObjectFields.Where(d => d.FieldName == "BankAccountNumber").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    BankDepositObjectTable.HeaderScreenId = BankDepositHeaderScreenScreen1.Id;
	   		  

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable BankDepositObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "BankDeposit" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode BankDepositDetailsTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BankDeposit.TH.Details", DefaultText = "Details",LocalDefaultText = "פרטים", ObjectTableId = BankDepositObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature BankDepositDetailsFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BankDeposit.Tab.Details", ObjectTableId = BankDepositObjectTable.Id, Tenant = 0, NameTextCodeCode = "BankDeposit.Features.Details", NameTextCodeDefaultText = "Details Tab", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode BankDepositEventsTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "BankDeposit.TH.Events", DefaultText = "Events",LocalDefaultText = "אירועים", ObjectTableId = BankDepositObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature BankDepositEventsFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BankDeposit.Tab.Events", ObjectTableId = BankDepositObjectTable.Id, Tenant = 0, NameTextCodeCode = "BankDepositFeatures.DPEV", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "BDDL",HtmlComponentName = "BankDepositDetailsTabComponent",HtmlComponentUrl = "./Accounting/Components/EditTabs/BankDeposit/BankDepositDetailsTabComponent", FeatureId = BankDepositDetailsFeature_TH0.Id, ControlPath = "Logitude.Accounting.ViewModels.Tabs.BNK.BankDepositDetailsTabControl", ObjectTableId = BankDepositObjectTable.Id, TabNameTextCodeId = BankDepositDetailsTextCode_TH0.Id, TabNameTextCodeCode = BankDepositDetailsTextCode_TH0.Code, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "DPEV",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = BankDepositEventsFeature_TH1.Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = BankDepositObjectTable.Id, TabNameTextCodeId = BankDepositEventsTextCode_TH1.Id, TabNameTextCodeCode = BankDepositEventsTextCode_TH1.Code, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable BankDepositObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "BankDeposit" && d.Tenant == 0).FirstOrDefault(); 

		   Feature BankDepositFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = true, ObjectTableId = BankDepositObjectTable.Id, Tenant = 0, NameTextCodeCode = "BankDeposit.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature BankDepositFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = true, ObjectTableId = BankDepositObjectTable.Id, Tenant = 0, NameTextCodeCode = "BankDeposit.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature BankDepositFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = true, ObjectTableId = BankDepositObjectTable.Id, Tenant = 0, NameTextCodeCode = "BankDeposit.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature BankDepositFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = true, ObjectTableId = BankDepositObjectTable.Id, Tenant = 0, NameTextCodeCode = "BankDeposit.Features.PackageFeature", NameTextCodeDefaultText = "BankDeposit Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);    
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable BankDepositObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "BankDeposit" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "BDRC",
                EnglishName =  "Cheque returned to cashbook",
                LocalName =  "המחאה הוצאה מהפקדה",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = BankDepositObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CREV",
                EnglishName =  "Created",
                LocalName =  "יצירה",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = BankDepositObjectTable.Id,
				 
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
                ObjectTableId = BankDepositObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CNEV",
                EnglishName =  "Cancelled",
                LocalName =  "ביטול",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = BankDepositObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
		   FeatureRepository featureRepository = new FeatureRepository(0); 
		   //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList(); 
		   ObjectTable BankDepositObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "BankDeposit" && d.Tenant == 0).FirstOrDefault();       
    
			   Feature BankDepositFeature_MB0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BANKDEPOSITAPRV", ObjectTableId = BankDepositObjectTable.Id, Tenant = 0, NameTextCodeCode = "BankDeposit.Features.BankDepositApprove", NameTextCodeDefaultText = "Approve Deposit", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

			   Feature BankDepositFeature_MB10 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BANKDEPOSITPRINT", ObjectTableId = BankDepositObjectTable.Id, Tenant = 0, NameTextCodeCode = "BankDeposit.Features.BankDepositPrint", NameTextCodeDefaultText = "Print Bank Deposit", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
             			   Feature BankDepositFeature_MB11 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CancelDeposit", ObjectTableId = BankDepositObjectTable.Id, Tenant = 0, NameTextCodeCode = "BankDeposit.Features.CancelDeposit", NameTextCodeDefaultText = "Cancel Deposit", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
              

		   TextCodeRepository.SubmitChanges();
		   FeaturesRepository.SubmitChanges();
		   MenuButtonGroup BankDepositMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
				{
					MenuButtonGroupType = "BankDepositEdit",
					Name = "BankDepositEditButtonsGroup",
					ObjectTableId = BankDepositObjectTable.Id,
					Tenant = 0
				}, menuButtonGroupRepository, tenantMenuButtonGroups);        
   
			   MenuButton BankDepositMenuButton0 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "BankDepositApprove",
						Index = 0, 
						IsActive = true,
						LabelTextCodeCode = "BankDeposit.B.Approve",
						LabelTextCodeDefaultText = "Approve",
						Tenant = 0,
						MenuButtonGroupId = BankDepositMenuButtonGroup.Id,
						ObjectTableId = BankDepositObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = BankDepositFeature_MB0.Id,
						Style = "ApproveButtonStyle",
						LocalDefaultText = "אישור",
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton BankDepositMenuButton1 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "More",
						Index = 100, 
						IsActive = false,
						LabelTextCodeCode = "BankDeposit.B.More",
						LabelTextCodeDefaultText = "More",
						Tenant = 0,
						MenuButtonGroupId = BankDepositMenuButtonGroup.Id,
						ObjectTableId = BankDepositObjectTable.Id,
						MenuButtonType = "dropdownbutton",
						FeatureId = null,
						Style = null,
						LocalDefaultText = "נוספים",
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);

			   MenuButton BankDepositMenuButton10 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "BankDepositPrint",
						Index = 0, 
						IsActive = true,
						LabelTextCodeCode = "BankDeposit.B.BankDepositPrint",
						LabelTextCodeDefaultText = "Print",
						Tenant = 0,
						MenuButtonGroupId = BankDepositMenuButtonGroup.Id,
						ParentMenuButtonId = BankDepositMenuButton1.Id,
						ObjectTableId = BankDepositObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  BankDepositFeature_MB10.Id,
						Style = "Ordinary",
						LocalDefaultText = "הדפס",
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton BankDepositMenuButton11 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "CancelDeposit",
						Index = 1, 
						IsActive = true,
						LabelTextCodeCode = "BankDeposit.B.Cancel",
						LabelTextCodeDefaultText = "Cancel deposit",
						Tenant = 0,
						MenuButtonGroupId = BankDepositMenuButtonGroup.Id,
						ParentMenuButtonId = BankDepositMenuButton1.Id,
						ObjectTableId = BankDepositObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  BankDepositFeature_MB11.Id,
						Style = null,
						LocalDefaultText = "ביטול הפקדה",
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {     
	    
}

    

   }
    
}
	 