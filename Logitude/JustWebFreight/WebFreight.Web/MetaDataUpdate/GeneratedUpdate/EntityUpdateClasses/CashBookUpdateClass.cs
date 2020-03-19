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
   public class CashBookUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "CashBook",
			      				    IsNew =  false,
			      				    DBTableName =  "CashBooks",
			      				    OldDBTableName =  "CashBooks",
			      				    ObjectTableSingular =  "CashBook",
			      				    ObjectTablePlural =  "CashBooks",
			      				    DescriptionDefaultText =  "Cashbook",
			      				    HasCustomFilter =  false,
			      				    HasCustomFields =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  false,
			      				    IsNewWizard =  true,
			      				    LookUp1 =  "EnglishName",
			      				    LookUp2 =  "AccountNumber",
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
			      				    SortingByObjectField =  "EnglishName",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "BR",
			      				    MaxNumberOfCustomFields =  0,
			      				    NewWizardControlName =  "NewCashBookComponent",
			      				    LocalDefaultText =  "קופה",
			      				    DefaultText =  "Cashbook",
			      				    Code =  "e691",
			      				    Name =  "CashBook Query Group",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Accounting",
			      				    NewWizardComponentPath =  "./Accounting/Components/NewEntity/NewCashBookComponent",
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
					  						ObjectTableName =  "CashBook",
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
					  						IsCustomFilter =  false,
					  						Operator =  "Between",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CreateDate",
					  						ListPropertyPath =  "CreateDate",
					  						DisplayInLookUpIndex =  1,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  1,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CashBook",
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
					  						FullLocalDefaultText =  "תאריך פתיחה",
					  						ListFieldLable =  "CreateDateListLable",
					  						ListLableDefaultText =  "Create Date",
					  						ListLocalDefaultText =  "תאריך פתיחה",
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
					  						ObjectTableName =  "CashBook",
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
					  						ValidForQuerySection1 =  "CashBook",
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
					  						FullLocalDefaultText =  "נוצר על ידי משתמש",
					  						ListFieldLable =  "CreatedByUserIdListLable",
					  						ListLableDefaultText =  "Created By",
					  						ListLocalDefaultText =  "נוצר על ידי משתמש",
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
					  						ObjectTableName =  "CashBook",
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
					  						IsCustomFilter =  false,
					  						Operator =  "Between",
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
					  						ValidForQuerySection1 =  "CashBook",
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
					  						ObjectTableName =  "CashBook",
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
					  						ValidForQuerySection1 =  "CashBook",
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
					  						FullLocalDefaultText =  "עודכן על ידי משתמש",
					  						ListFieldLable =  "UpdatedByUserIdListLable",
					  						ListLableDefaultText =  "Updated By",
					  						ListLocalDefaultText =  "עודכן על ידי משתמש",
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
					  						ObjectTableName =  "CashBook",
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
					  						ValidForQuerySection1 =  "CashBook",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SearchFields",
					  						DefaultText =  "Search ...",
					  						ListFieldLable =  "SearchFieldsListLable",
					  						ListLableDefaultText =  "Search ...",
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
					  						ObjectTableName =  "CashBook",
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
					  						ValidForQuerySection1 =  "CashBook",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CreatedByUserName",
					  						DefaultText =  "Created by",
					  						FullLocalDefaultText =  "נוצר על ידי",
					  						ListFieldLable =  "CreatedByUserNameListLable",
					  						ListLableDefaultText =  "Created by",
					  						ListLocalDefaultText =  "נוצר על ידי",
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
					 
					 						FieldName =  "UpdatedByUserName",
					  						OldFieldName =  "UpdatedByUserName",
					  						ObjectTableName =  "CashBook",
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
					  						ValidForQuerySection1 =  "CashBook",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UpdatedByUserName",
					  						DefaultText =  "Updated by",
					  						FullLocalDefaultText =  "עודכן על ידי",
					  						ListFieldLable =  "UpdatedByUserNameListLable",
					  						ListLableDefaultText =  "Updated by",
					  						ListLocalDefaultText =  "עודכן על ידי",
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
					 
					 						FieldName =  "EnglishName",
					  						OldFieldName =  "EnglishName",
					  						ObjectTableName =  "CashBook",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  60,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  true,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  60,
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
					  						ValidForQuerySection1 =  "CashBook",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "EnglishName",
					  						DefaultText =  "English Name",
					  						FullLocalDefaultText =  "שם באנגלית",
					  						ListFieldLable =  "EnglishNameListLable",
					  						ListLableDefaultText =  "English Name",
					  						ListLocalDefaultText =  "שם באנגלית",
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
					 
					 						FieldName =  "LocalName",
					  						OldFieldName =  "LocalName",
					  						ObjectTableName =  "CashBook",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  60,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  60,
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
					  						ValidForQuerySection1 =  "CashBook",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LocalName",
					  						DefaultText =  "Local Name",
					  						FullLocalDefaultText =  "שם מקומי",
					  						ListFieldLable =  "LocalNameListLable",
					  						ListLableDefaultText =  "Local Name",
					  						ListLocalDefaultText =  "שם מקומי",
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
					 
					 						FieldName =  "Inactive",
					  						OldFieldName =  "Inactive",
					  						ObjectTableName =  "CashBook",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Inactive",
					  						ListPropertyPath =  "Inactive",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CashBook",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Inactive",
					  						DefaultText =  "Inactive",
					  						FullLocalDefaultText =  "חסום",
					  						ListFieldLable =  "InactiveListLable",
					  						ListLableDefaultText =  "Inactive",
					  						ListLocalDefaultText =  "חסום",
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
					 
					 						FieldName =  "CurrencyId",
					  						OldFieldName =  "CurrencyId",
					  						ObjectTableName =  "CashBook",
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
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
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
					  						ValidForQuerySection1 =  "CashBook",
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
					  						FullLocalDefaultText =  "מטבע",
					  						ListFieldLable =  "CurrencyIdListLable",
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
					 
					 						FieldName =  "CurrencyCode",
					  						OldFieldName =  "CurrencyCode",
					  						ObjectTableName =  "CashBook",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  5,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  true,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  5,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  true,
					  						PMPropertyPath =  "CurrencyCode",
					  						ListPropertyPath =  "CurrencyCode",
					  						DisplayInLookUpIndex =  2,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  2,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CashBook",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CurrencyCode",
					  						DefaultText =  "Currency",
					  						FullLocalDefaultText =  "קוד מטבע",
					  						ListFieldLable =  "CurrencyCodeListLable",
					  						ListLableDefaultText =  "Currency",
					  						ListLocalDefaultText =  "קוד מטבע",
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
					 
					 						FieldName =  "CurrencyName",
					  						OldFieldName =  "CurrencyName",
					  						ObjectTableName =  "CashBook",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CurrencyName",
					  						ListPropertyPath =  "CurrencyName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CashBook",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CurrencyName",
					  						DefaultText =  "Currency",
					  						FullLocalDefaultText =  "שם מטבע",
					  						ListFieldLable =  "CurrencyNameListLable",
					  						ListLableDefaultText =  "Currency",
					  						ListLocalDefaultText =  "שם מטבע",
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
					 
					 						FieldName =  "CashBookTypeCode",
					  						OldFieldName =  "CashBookTypeCode",
					  						ObjectTableName =  "CashBook",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "CashBookType",
					  						MinLength =  0,
					  						MaxLength =  3,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  3,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CashBookTypeCode",
					  						ListPropertyPath =  "CashBookTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CashBook",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CashBookTypeCode",
					  						DefaultText =  "Cash Book Type",
					  						FullLocalDefaultText =  "סוג קופה",
					  						ListFieldLable =  "CashBookTypeCodeListLable",
					  						ListLableDefaultText =  "Cash Book Type",
					  						ListLocalDefaultText =  "סוג קופה",
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
					 
					 						FieldName =  "CashBookTypeName",
					  						OldFieldName =  "CashBookTypeName",
					  						ObjectTableName =  "CashBook",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  45,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  true,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  45,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  true,
					  						PMPropertyPath =  "CashBookTypeName",
					  						ListPropertyPath =  "CashBookTypeName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CashBook",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CashBookTypeName",
					  						DefaultText =  "Cash Book Type",
					  						FullLocalDefaultText =  "שם סוג קופה",
					  						ListFieldLable =  "CashBookTypeNameListLable",
					  						ListLableDefaultText =  "Cash Book Type",
					  						ListLocalDefaultText =  "שם סוג קופה",
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
					 
					 						FieldName =  "TotalAmount",
					  						OldFieldName =  "TotalAmount",
					  						ObjectTableName =  "CashBook",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TotalAmount",
					  						ListPropertyPath =  "TotalAmount",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CashBook",
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
					  						FullFieldLable =  "TotalAmount",
					  						DefaultText =  "Total Amount",
					  						FullLocalDefaultText =  "סך כולל",
					  						ListFieldLable =  "TotalAmountListLable",
					  						ListLableDefaultText =  "Total Amount",
					  						ListLocalDefaultText =  "סך כולל",
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
					 
					 						FieldName =  "AccountId",
					  						OldFieldName =  "AccountId",
					  						ObjectTableName =  "CashBook",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "GLAccount",
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AccountId",
					  						ListPropertyPath =  "AccountId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CashBook",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AccountId",
					  						DefaultText =  "GL Account",
					  						FullLocalDefaultText =  "חשבון",
					  						ListFieldLable =  "AccountIdListLable",
					  						ListLableDefaultText =  "GL Account",
					  						ListLocalDefaultText =  "חשבון",
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
					 
					 						FieldName =  "AccountNumber",
					  						OldFieldName =  "AccountNumber",
					  						ObjectTableName =  "CashBook",
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
					  						ValidForQuerySection1 =  "CashBook",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AccountNumber",
					  						DefaultText =  "Account No.",
					  						FullLocalDefaultText =  "מספר חשבון",
					  						ListFieldLable =  "AccountNumberListLable",
					  						ListLableDefaultText =  "Account No.",
					  						ListLocalDefaultText =  "מספר חשבון",
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
					 
					 						FieldName =  "AccountName",
					  						OldFieldName =  "AccountName",
					  						ObjectTableName =  "CashBook",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AccountName",
					  						ListPropertyPath =  "AccountName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CashBook",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AccountName",
					  						DefaultText =  "Account Name",
					  						FullLocalDefaultText =  "שם חשבון",
					  						ListFieldLable =  "AccountNameListLable",
					  						ListLableDefaultText =  "Account Name",
					  						ListLocalDefaultText =  "שם חשבון",
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
					 
					 						FieldName =  "CashBookLines",
					  						OldFieldName =  "CashBookLines",
					  						ObjectTableName =  "CashBook",
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
					  						PMPropertyPath =  "CashBookLines",
					  						ListPropertyPath =  "CashBookLines",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  true,
					  						MultiTableName =  "CashBookLine",
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
					  						FullFieldLable =  "CashBookLines",
					  						DefaultText =  "Cash Book Lines",
					  						ListFieldLable =  "CashBookLinesListLable",
					  						ListLableDefaultText =  "Cash Book Lines",
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
					 
					 						FieldName =  "BranchId",
					  						OldFieldName =  "BranchId",
					  						ObjectTableName =  "CashBook",
					  						FieldsDataType =  "Text",
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
					  						ValidForQuerySection1 =  "CashBook",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "BranchId",
					  						DefaultText =  "Branch",
					  						FullLocalDefaultText =  "סניף",
					  						ListFieldLable =  "BranchIdListLable",
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
					 
					 						FieldName =  "CurrencySign",
					  						OldFieldName =  "CurrencySign",
					  						ObjectTableName =  "CashBook",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  3,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  3,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CurrencySign",
					  						ListPropertyPath =  "CurrencySign",
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
					  						FullFieldLable =  "CurrencySign",
					  						DefaultText =  "Currency",
					  						ListFieldLable =  "CurrencySignListLable",
					  						ListLableDefaultText =  "Currency",
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
					  						ObjectTableName =  "CashBook",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  300,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  300,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
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
					  						ValidForQuerySection1 =  "CashBook",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "BranchName",
					  						DefaultText =  "Branch",
					  						FullLocalDefaultText =  "סניף",
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
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	    {  
	        FeatureRepository featureRepository = new FeatureRepository(0); 
            //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
            IWebFreightContext objectContext = WebFreightContext.GetContext(0);  
	        QueryGroup CashBookQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "e691", Name = "CashBook Query Group" }, queryGroupRepository);
						QueryGroup CashBookQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "165d", Name = " Query Group" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable CashBookObjectTable = objectContext.ObjectTables.Where(d => d.Name == "CashBook" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> CashBookObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "CashBook").ToList();   

			   TextCode CashBookTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CashBook.Q.CashBooks", DefaultText = @"All Cashbooks",LocalDefaultText = "כל הקופות", ObjectTableId = CashBookObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature CashBookFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CashBook.Q.CashBooks", ObjectTableId = CashBookObjectTable.Id, Tenant = 0, NameTextCodeCode = "CashBook.Features.CashBooks", NameTextCodeDefaultText = "CashBooks", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode CashBookTextCode_1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CashBook.Q.CashbookCash", DefaultText = @"Cash Cashbooks",LocalDefaultText = "מזומן הקופות", ObjectTableId = CashBookObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature CashBookFeature_1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CashBook.Q.CashbookCash", ObjectTableId = CashBookObjectTable.Id, Tenant = 0, NameTextCodeCode = "CashBook.Features.CashbookCash", NameTextCodeDefaultText = "CashbookCash", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode CashBookTextCode_2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CashBook.Q.CashbookCheque", DefaultText = @"Cheque Cashbooks",LocalDefaultText = "המחאה הקופות", ObjectTableId = CashBookObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature CashBookFeature_2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CashBook.Q.CashbookCheque", ObjectTableId = CashBookObjectTable.Id, Tenant = 0, NameTextCodeCode = "CashBook.Features.CashbookCheque", NameTextCodeDefaultText = "CashbookCheque", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query CashBooksQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = CashBookTextCode_0.Id, NameTextCodeCode = CashBookTextCode_0.Code, ObjectTableName = "CashBook", Code = "CashBooks",  QueryGroupCode = "e691", IndexOrder = 0, Tenant = 0, ObjectTableId = CashBookObjectTable.Id, QuerySection = "CashBook", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = CashBookFeature_0.Id,FeatureUniqeCode= CashBookFeature_0.FeatureUniqeCode, DefaultSortName = "EnglishName", DefaultSortDirection = "Ascending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn CashBooksQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CashBooksQuery.Id,QueryCode = CashBooksQuery.UniqueCode, IndexOrder = 0, ObjectFieldId = CashBookObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CashBookObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 270 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CashBooksQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CashBooksQuery.Id,QueryCode = CashBooksQuery.UniqueCode, IndexOrder = 1, ObjectFieldId = CashBookObjectFields.Where(d => d.FieldName == "BranchName" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CashBookObjectFields.Where(d => d.FieldName == "BranchName" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 141 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CashBooksQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CashBooksQuery.Id,QueryCode = CashBooksQuery.UniqueCode, IndexOrder = 2, ObjectFieldId = CashBookObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CashBookObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 270 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CashBooksQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CashBooksQuery.Id,QueryCode = CashBooksQuery.UniqueCode, IndexOrder = 3, ObjectFieldId = CashBookObjectFields.Where(d => d.FieldName == "CashBookTypeName" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CashBookObjectFields.Where(d => d.FieldName == "CashBookTypeName" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 170 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CashBooksQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CashBooksQuery.Id,QueryCode = CashBooksQuery.UniqueCode, IndexOrder = 4, ObjectFieldId = CashBookObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CashBookObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 157 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CashBooksQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CashBooksQuery.Id,QueryCode = CashBooksQuery.UniqueCode, IndexOrder = 5, ObjectFieldId = CashBookObjectFields.Where(d => d.FieldName == "TotalAmount" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CashBookObjectFields.Where(d => d.FieldName == "TotalAmount" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 140 }, queryColumnsRepository, tenantQueryColumns);
  
	      

			  Query CashbookCashQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = CashBookTextCode_1.Id, NameTextCodeCode = CashBookTextCode_1.Code, ObjectTableName = "CashBook", Code = "CashbookCash",  QueryGroupCode = "e691", IndexOrder = 1, Tenant = 0, ObjectTableId = CashBookObjectTable.Id, QuerySection = "CashBook", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = CashBookFeature_1.Id,FeatureUniqeCode= CashBookFeature_1.FeatureUniqeCode, DefaultSortName = "CreateDate", DefaultSortDirection = "Desending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn CashbookCashQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CashbookCashQuery.Id,QueryCode = CashbookCashQuery.UniqueCode, IndexOrder = 0, ObjectFieldId = CashBookObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CashBookObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 261 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CashbookCashQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CashbookCashQuery.Id,QueryCode = CashbookCashQuery.UniqueCode, IndexOrder = 1, ObjectFieldId = CashBookObjectFields.Where(d => d.FieldName == "BranchName" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CashBookObjectFields.Where(d => d.FieldName == "BranchName" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 140 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CashbookCashQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CashbookCashQuery.Id,QueryCode = CashbookCashQuery.UniqueCode, IndexOrder = 2, ObjectFieldId = CashBookObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CashBookObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 293 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CashbookCashQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CashbookCashQuery.Id,QueryCode = CashbookCashQuery.UniqueCode, IndexOrder = 3, ObjectFieldId = CashBookObjectFields.Where(d => d.FieldName == "CashBookTypeName" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CashBookObjectFields.Where(d => d.FieldName == "CashBookTypeName" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 183 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CashbookCashQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CashbookCashQuery.Id,QueryCode = CashbookCashQuery.UniqueCode, IndexOrder = 4, ObjectFieldId = CashBookObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CashBookObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 153 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CashbookCashQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CashbookCashQuery.Id,QueryCode = CashbookCashQuery.UniqueCode, IndexOrder = 5, ObjectFieldId = CashBookObjectFields.Where(d => d.FieldName == "TotalAmount" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CashBookObjectFields.Where(d => d.FieldName == "TotalAmount" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 156 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter CashbookCashQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = CashBookObjectFields.Where(d => d.FieldName == "CashBookTypeCode" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CashBookObjectFields.Where(d => d.FieldName == "CashBookTypeCode" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "1",PredefinedValue2 = null, QueryId = CashbookCashQuery.Id,QueryCode = CashbookCashQuery.UniqueCode, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query CashbookChequeQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = CashBookTextCode_2.Id, NameTextCodeCode = CashBookTextCode_2.Code, ObjectTableName = "CashBook", Code = "CashbookCheque",  QueryGroupCode = "e691", IndexOrder = 2, Tenant = 0, ObjectTableId = CashBookObjectTable.Id, QuerySection = "CashBook", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = CashBookFeature_2.Id,FeatureUniqeCode= CashBookFeature_2.FeatureUniqeCode, DefaultSortName = "CreateDate", DefaultSortDirection = "Desending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn CashbookChequeQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CashbookChequeQuery.Id,QueryCode = CashbookChequeQuery.UniqueCode, IndexOrder = 0, ObjectFieldId = CashBookObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CashBookObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 291 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CashbookChequeQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CashbookChequeQuery.Id,QueryCode = CashbookChequeQuery.UniqueCode, IndexOrder = 1, ObjectFieldId = CashBookObjectFields.Where(d => d.FieldName == "BranchName" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CashBookObjectFields.Where(d => d.FieldName == "BranchName" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 140 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CashbookChequeQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CashbookChequeQuery.Id,QueryCode = CashbookChequeQuery.UniqueCode, IndexOrder = 2, ObjectFieldId = CashBookObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CashBookObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 276 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CashbookChequeQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CashbookChequeQuery.Id,QueryCode = CashbookChequeQuery.UniqueCode, IndexOrder = 3, ObjectFieldId = CashBookObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CashBookObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 155 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CashbookChequeQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CashbookChequeQuery.Id,QueryCode = CashbookChequeQuery.UniqueCode, IndexOrder = 4, ObjectFieldId = CashBookObjectFields.Where(d => d.FieldName == "CashBookTypeCode" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CashBookObjectFields.Where(d => d.FieldName == "CashBookTypeCode" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 165 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CashbookChequeQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CashbookChequeQuery.Id,QueryCode = CashbookChequeQuery.UniqueCode, IndexOrder = 5, ObjectFieldId = CashBookObjectFields.Where(d => d.FieldName == "TotalAmount" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CashBookObjectFields.Where(d => d.FieldName == "TotalAmount" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 134 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter CashbookChequeQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = CashBookObjectFields.Where(d => d.FieldName == "CashBookTypeCode" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CashBookObjectFields.Where(d => d.FieldName == "CashBookTypeCode" && d.ObjectTableId == CashBookObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "2",PredefinedValue2 = null, QueryId = CashbookChequeQuery.Id,QueryCode = CashbookChequeQuery.UniqueCode, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);

	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable CashBookObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "CashBook" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> CashBookObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "CashBook").ToList();
		       
	      

	         Screen CashBookHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "CashBook.HeaderScreen", Name = "HeaderScreen", ObjectTableId = CashBookObjectTable.Id, NumberOfColumns = 4, NumberOfRows = 2, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField CashBookCashBookHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = CashBookObjectFields.Where(d => d.FieldName == "EnglishName").FirstOrDefault().Id, ScreenId = CashBookHeaderScreenScreen0.Id,ScreenCode = CashBookHeaderScreenScreen0.Code, ObjectFieldCode = CashBookObjectFields.Where(d => d.FieldName == "EnglishName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CashBookCashBookHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = CashBookObjectFields.Where(d => d.FieldName == "AccountName").FirstOrDefault().Id, ScreenId = CashBookHeaderScreenScreen0.Id,ScreenCode = CashBookHeaderScreenScreen0.Code, ObjectFieldCode = CashBookObjectFields.Where(d => d.FieldName == "AccountName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CashBookCashBookHeaderScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = CashBookObjectFields.Where(d => d.FieldName == "LocalName").FirstOrDefault().Id, ScreenId = CashBookHeaderScreenScreen0.Id,ScreenCode = CashBookHeaderScreenScreen0.Code, ObjectFieldCode = CashBookObjectFields.Where(d => d.FieldName == "LocalName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CashBookCashBookHeaderScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ObjectFieldId = CashBookObjectFields.Where(d => d.FieldName == "CurrencyCode").FirstOrDefault().Id, ScreenId = CashBookHeaderScreenScreen0.Id,ScreenCode = CashBookHeaderScreenScreen0.Code, ObjectFieldCode = CashBookObjectFields.Where(d => d.FieldName == "CurrencyCode").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CashBookCashBookHeaderScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = CashBookObjectFields.Where(d => d.FieldName == "TotalAmount").FirstOrDefault().Id, ScreenId = CashBookHeaderScreenScreen0.Id,ScreenCode = CashBookHeaderScreenScreen0.Code, ObjectFieldCode = CashBookObjectFields.Where(d => d.FieldName == "TotalAmount").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CashBookCashBookHeaderScreenScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 1, ObjectFieldId = CashBookObjectFields.Where(d => d.FieldName == "BranchName").FirstOrDefault().Id, ScreenId = CashBookHeaderScreenScreen0.Id,ScreenCode = CashBookHeaderScreenScreen0.Code, ObjectFieldCode = CashBookObjectFields.Where(d => d.FieldName == "BranchName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CashBookCashBookHeaderScreenScreenField6 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ObjectFieldId = CashBookObjectFields.Where(d => d.FieldName == "CashBookTypeName").FirstOrDefault().Id, ScreenId = CashBookHeaderScreenScreen0.Id,ScreenCode = CashBookHeaderScreenScreen0.Code, ObjectFieldCode = CashBookObjectFields.Where(d => d.FieldName == "CashBookTypeName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    CashBookObjectTable.HeaderScreenId = CashBookHeaderScreenScreen0.Id;
		    CashBookObjectTable.HeaderScreenCode = CashBookHeaderScreenScreen0.Code;

	   		  
	      

	         Screen CashBookGeneralTabScreenScreen1 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "CashBook.GeneralTabScreen", Name = "GeneralTabScreen", ObjectTableId = CashBookObjectTable.Id, NumberOfColumns = 1, NumberOfRows = 5, IsReadOnly = false }, screensRepository, tenantScreens);
      
            ScreenField CashBookCashBookGeneralTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = CashBookObjectFields.Where(d => d.FieldName == "CashBookTypeCode").FirstOrDefault().Id, ScreenId = CashBookGeneralTabScreenScreen1.Id,ScreenCode = CashBookGeneralTabScreenScreen1.Code, ObjectFieldCode = CashBookObjectFields.Where(d => d.FieldName == "CashBookTypeCode").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CashBookCashBookGeneralTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = CashBookObjectFields.Where(d => d.FieldName == "EnglishName").FirstOrDefault().Id, ScreenId = CashBookGeneralTabScreenScreen1.Id,ScreenCode = CashBookGeneralTabScreenScreen1.Code, ObjectFieldCode = CashBookObjectFields.Where(d => d.FieldName == "EnglishName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CashBookCashBookGeneralTabScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ObjectFieldId = CashBookObjectFields.Where(d => d.FieldName == "LocalName").FirstOrDefault().Id, ScreenId = CashBookGeneralTabScreenScreen1.Id,ScreenCode = CashBookGeneralTabScreenScreen1.Code, ObjectFieldCode = CashBookObjectFields.Where(d => d.FieldName == "LocalName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CashBookCashBookGeneralTabScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 3, ObjectFieldId = CashBookObjectFields.Where(d => d.FieldName == "CurrencyCode").FirstOrDefault().Id, ScreenId = CashBookGeneralTabScreenScreen1.Id,ScreenCode = CashBookGeneralTabScreenScreen1.Code, ObjectFieldCode = CashBookObjectFields.Where(d => d.FieldName == "CurrencyCode").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CashBookCashBookGeneralTabScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 4, ObjectFieldId = CashBookObjectFields.Where(d => d.FieldName == "AccountNumber").FirstOrDefault().Id, ScreenId = CashBookGeneralTabScreenScreen1.Id,ScreenCode = CashBookGeneralTabScreenScreen1.Code, ObjectFieldCode = CashBookObjectFields.Where(d => d.FieldName == "AccountNumber").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
           

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable CashBookObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "CashBook" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode CashBookDetailsTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CashBook.TH.Details", DefaultText = "Details",LocalDefaultText = "פרטים", ObjectTableId = CashBookObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CashBookDetailsFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DETAILS", ObjectTableId = CashBookObjectTable.Id, Tenant = 0, NameTextCodeCode = "CashBook.Features.Details", NameTextCodeDefaultText = "Details", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode CashBookGeneralTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CashBook.TH.General", DefaultText = "General",LocalDefaultText = "כללי", ObjectTableId = CashBookObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CashBookGeneralFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CashBook.Tab.General", ObjectTableId = CashBookObjectTable.Id, Tenant = 0, NameTextCodeCode = "CashBook.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode CashBookManageDepoTextCode_TH2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CashBook.TH.ManageDepo", DefaultText = "Manage Depo.",LocalDefaultText = "ניהול הפקדות", ObjectTableId = CashBookObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CashBookManageDepoFeature_TH2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MNGDEPO", ObjectTableId = CashBookObjectTable.Id, Tenant = 0, NameTextCodeCode = "CashBook.Features.MNGDEPO", NameTextCodeDefaultText = "Manage Depo.", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode CashBookEventsTextCode_TH3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CashBook.TH.Events", DefaultText = "Events",LocalDefaultText = "אירועים", ObjectTableId = CashBookObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CashBookEventsFeature_TH3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CashBook.Tab.Events", ObjectTableId = CashBookObjectTable.Id, Tenant = 0, NameTextCodeCode = "CashBook.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CBDT",HtmlComponentName = "CashBookDetailsTabComponent",HtmlComponentUrl = "./Accounting/Components/EditTabs/CashBook/CashBookDetailsTabComponent", FeatureId = CashBookDetailsFeature_TH0.Id,FeatureUniqeCode = CashBookDetailsFeature_TH0.FeatureUniqeCode, ControlPath = "Logitude.Accounting.Views.Tabs.CSHB.CashBookDetailsTabComponent", ObjectTableId = CashBookObjectTable.Id, TabNameTextCodeId = CashBookDetailsTextCode_TH0.Id, TabNameTextCodeCode = CashBookDetailsTextCode_TH0.Code, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CSHG",HtmlComponentName = "CashBookGeneralTabComponent",HtmlComponentUrl = "./Accounting/Components/EditTabs/CashBook/CashBookGeneralTabComponent", FeatureId = CashBookGeneralFeature_TH1.Id,FeatureUniqeCode = CashBookGeneralFeature_TH1.FeatureUniqeCode, ControlPath = "./Accounting/Components/EditTabs/CashBook/CashBookGeneralTabComponent", ObjectTableId = CashBookObjectTable.Id, TabNameTextCodeId = CashBookGeneralTextCode_TH1.Id, TabNameTextCodeCode = CashBookGeneralTextCode_TH1.Code, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CBMD",HtmlComponentName = "CashBookManageDepoTabComponent",HtmlComponentUrl = "./Accounting/Components/EditTabs/CashBook/CashBookManageDepoTabComponent", FeatureId = CashBookManageDepoFeature_TH2.Id,FeatureUniqeCode = CashBookManageDepoFeature_TH2.FeatureUniqeCode, ControlPath = "Logitude.Accounting.Views.Tabs.CSHB.CashBookManageDepoTabComponent", ObjectTableId = CashBookObjectTable.Id, TabNameTextCodeId = CashBookManageDepoTextCode_TH2.Id, TabNameTextCodeCode = CashBookManageDepoTextCode_TH2.Code, Tenant = 0, IndexOrder = 2 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CSHE",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = CashBookEventsFeature_TH3.Id,FeatureUniqeCode = CashBookEventsFeature_TH3.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = CashBookObjectTable.Id, TabNameTextCodeId = CashBookEventsTextCode_TH3.Id, TabNameTextCodeCode = CashBookEventsTextCode_TH3.Code, Tenant = 0, IndexOrder = 3 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable CashBookObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "CashBook" && d.Tenant == 0).FirstOrDefault(); 

		   Feature CashBookFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = true, ObjectTableId = CashBookObjectTable.Id, Tenant = 0, NameTextCodeCode = "CashBook.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature CashBookFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = true, ObjectTableId = CashBookObjectTable.Id, Tenant = 0, NameTextCodeCode = "CashBook.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature CashBookFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = true, ObjectTableId = CashBookObjectTable.Id, Tenant = 0, NameTextCodeCode = "CashBook.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature CashBookFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = true, ObjectTableId = CashBookObjectTable.Id, Tenant = 0, NameTextCodeCode = "CashBook.Features.PackageFeature", NameTextCodeDefaultText = "CashBook Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);    
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable CashBookObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "CashBook" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CBDP",
                EnglishName =  "Deposit Created",
                LocalName =  "הפקדה נוצר",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CashBookObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CR2C",
                EnglishName =  "Cheque Returned to Cashbook",
                LocalName =  "בדוק את החזרות במזומן הספר",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CashBookObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "ENCH",
                EnglishName =  "English Name Changed",
                LocalName =  "English Name Changed",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CashBookObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "LCCH",
                EnglishName =  "Local Name Changed",
                LocalName =  "Local Name Changed",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CashBookObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CREV",
                EnglishName =  "Created",
                LocalName =  "שנוצר",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CashBookObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "UPEV",
                EnglishName =  "Updated",
                LocalName =  "מעודכן",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CashBookObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
		   FeatureRepository featureRepository = new FeatureRepository(0); 
		   //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList(); 
		   ObjectTable CashBookObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "CashBook" && d.Tenant == 0).FirstOrDefault();       
    
			   Feature CashBookFeature_MB0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DPSTCASHBOOK", ObjectTableId = CashBookObjectTable.Id, Tenant = 0, NameTextCodeCode = "CashBook.Features.Deposite", NameTextCodeDefaultText = "Deposite", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

			   Feature CashBookFeature_MB10 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "INACITVE", ObjectTableId = CashBookObjectTable.Id, Tenant = 0, NameTextCodeCode = "CashBook.Features.Inactive", NameTextCodeDefaultText = "Inactive", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
              

		   TextCodeRepository.SubmitChanges();
		   FeaturesRepository.SubmitChanges();
		   MenuButtonGroup CashBookMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
				{
					MenuButtonGroupType = "CasBookEdit",
					Name = "CasBookEditButtonsGroup",
					ObjectTableId = CashBookObjectTable.Id,
					Tenant = 0
				}, menuButtonGroupRepository, tenantMenuButtonGroups);        
   
			   MenuButton CashBookMenuButton0 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "CashBookDeposite",
						Index = 0, 
						IsActive = true,
						LabelTextCodeCode = "CashBook.B.Deposite",
						LabelTextCodeDefaultText = "Deposit",
						Tenant = 0,
						MenuButtonGroupId = CashBookMenuButtonGroup.Id,
						ObjectTableId = CashBookObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = CashBookFeature_MB0.Id,
						Style = "ApproveButtonStyle",
						LocalDefaultText = "הפקדה",
						FeatureUniqeCode = CashBookFeature_MB0.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton CashBookMenuButton1 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "More",
						Index = 100, 
						IsActive = false,
						LabelTextCodeCode = "CashBook.B.More",
						LabelTextCodeDefaultText = "More",
						Tenant = 0,
						MenuButtonGroupId = CashBookMenuButtonGroup.Id,
						ObjectTableId = CashBookObjectTable.Id,
						MenuButtonType = "dropdownbutton",
						FeatureId = null,
						Style = null,
						LocalDefaultText = "נוספים",
						FeatureUniqeCode = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);

			   MenuButton CashBookMenuButton10 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "CashBookInactive",
						Index = 0, 
						IsActive = true,
						LabelTextCodeCode = "CashBook.B.Inactive",
						LabelTextCodeDefaultText = "Inactive",
						Tenant = 0,
						MenuButtonGroupId = CashBookMenuButtonGroup.Id,
						ParentMenuButtonId = CashBookMenuButton1.Id,
						ObjectTableId = CashBookObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  CashBookFeature_MB10.Id,
						Style = null,
						LocalDefaultText = "חסימה",
						FeatureUniqeCode=  CashBookFeature_MB10.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable CashBookObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "CashBook" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode CashBookTextCode_CashbookOName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Cashbook.O.Name", DefaultText = "Cashbook",LocalDefaultText = @"שם הקופה", ObjectTableId = CashBookObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 