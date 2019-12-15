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
   public class GLAccountUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "GLAccount",
			      				    IsNew =  false,
			      				    DBTableName =  "GLAccounts",
			      				    OldDBTableName =  "GLAccounts",
			      				    ObjectTableSingular =  "GL Account",
			      				    ObjectTablePlural =  "GL Accounts",
			      				    HasCustomFilter =  false,
			      				    HasCustomFields =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  true,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  false,
			      				    IsNewWizard =  true,
			      				    LookUp1 =  "DisplayNumber",
			      				    LookUp2 =  "LocalName",
			      				    DependencyFilter1 =  "IsControlAccount",
			      				    DependencyFilter2 =  "AccountTypeCode",
			      				    KeyPropertyPath =  "Id",
			      				    AutoCompleteSearchWindow =  false,
			      				    IsClosed =  false,
			      				    CacheOnClient =  false,
			      				    EditableFromAutoCompleteWindow =  false,
			      				    HasCounter =  false,
			      				    EnableAddFromLOV =  true,
			      				    IsRestrictable =  false,
			      				    IsMain =  true,
			      				    IsAutoComplete =  false,
			      				    EnableEditFromLOV =  true,
			      				    SortingByObjectField =  "DisplayNumber",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "BR",
			      				    MaxNumberOfCustomFields =  0,
			      				    NewWizardControlName =  "Logitude.Accounting.Views.Tabs.ACC.NewGLAccountControlCommand",
			      				    LocalDefaultText =  "חשבון",
			      				    DefaultText =  "GL Account",
			      				    Code =  "GLAC",
			      				    Name =  "GLAccount",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Accounting",
			      				    NewWizardComponentPath =  "./Accounting/Components/NewEntity/NewGLAccountComponent",
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
					 
					 						FieldName =  "InternalNumber",
					  						OldFieldName =  "InternalNumber",
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  true,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  true,
					  						PMPropertyPath =  "InternalNumber",
					  						ListPropertyPath =  "InternalNumber",
					  						DisplayInLookUpIndex =  2,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "InternalNumber",
					  						DefaultText =  "Internal Counter",
					  						FullLocalDefaultText =  "מספר מערכת",
					  						ListFieldLable =  "InternalNumberListLable",
					  						ListLableDefaultText =  "Internal Counter",
					  						ListLocalDefaultText =  "מספר מערכת",
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
					 
					 						FieldName =  "AccountTypeCode",
					  						OldFieldName =  "AccountTypeCode",
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "GLAccountType",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  1,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AccountTypeCode",
					  						ListPropertyPath =  "AccountTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AccountTypeCode",
					  						DefaultText =  "Account Type Code",
					  						FullLocalDefaultText =  "סוג כרטיס",
					  						ListFieldLable =  "AccountTypeCodeListLable",
					  						ListLableDefaultText =  "Account Type Code",
					  						ListLocalDefaultText =  "סוג כרטיס",
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
					 
					 						FieldName =  "DisplayNumber",
					  						OldFieldName =  "DisplayNumber",
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  true,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  15,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  true,
					  						PMPropertyPath =  "DisplayNumber",
					  						ListPropertyPath =  "DisplayNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DisplayNumber",
					  						DefaultText =  "Account No.",
					  						FullLocalDefaultText =  "מספר כרטיס",
					  						ListFieldLable =  "DisplayNumberListLable",
					  						ListLableDefaultText =  "Account No.",
					  						ListLocalDefaultText =  "מספר כרטיס",
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
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  105,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  true,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  105,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  true,
					  						PMPropertyPath =  "LocalName",
					  						ListPropertyPath =  "LocalName",
					  						DisplayInLookUpIndex =  1,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  1,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
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
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "EnglishName",
					  						OldFieldName =  "EnglishName",
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  75,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  75,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
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
					  						ValidForQuerySection1 =  "GLAccount",
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
					 
					 						FieldName =  "SearchFields",
					  						OldFieldName =  "SearchFields",
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "nText",
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
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SearchFields",
					  						DefaultText =  "Account No. / Name",
					  						FullLocalDefaultText =  "מספר / שם כרטיס",
					  						ListFieldLable =  "SearchFieldsListLable",
					  						ListLableDefaultText =  "Account No. / Name",
					  						ListLocalDefaultText =  "מספר / שם כרטיס",
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
					 
					 						FieldName =  "IsMultiCurrency",
					  						OldFieldName =  "IsMultiCurrency",
					  						ObjectTableName =  "GLAccount",
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
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsMultiCurrency",
					  						DefaultText =  "Multi Currency",
					  						FullLocalDefaultText =  "רב מטבעי",
					  						ListFieldLable =  "IsMultiCurrencyListLable",
					  						ListLableDefaultText =  "Multi Currency",
					  						ListLocalDefaultText =  "רב מטבעי",
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
					  						ObjectTableName =  "GLAccount",
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
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CurrencyId",
					  						DefaultText =  "Currency ",
					  						FullLocalDefaultText =  "מטבע",
					  						ListFieldLable =  "CurrencyIdListLable",
					  						ListLableDefaultText =  "Currency ",
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
					 
					 						FieldName =  "RevenueExpenseType",
					  						OldFieldName =  "RevenueExpenseType",
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "RevenueExpenseType",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "RevenueExpenseType",
					  						ListPropertyPath =  "RevenueExpenseType",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "RevenueExpenseType",
					  						DefaultText =  "Revenue/Expense ",
					  						FullLocalDefaultText =  "סוג חשבון",
					  						ListFieldLable =  "RevenueExpenseTypeListLable",
					  						ListLableDefaultText =  "Revenue/Expense ",
					  						ListLocalDefaultText =  "סוג חשבון",
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
					 
					 						FieldName =  "IsControlAccount",
					  						OldFieldName =  "IsControlAccount",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "IsControlAccount",
					  						ListPropertyPath =  "IsControlAccount",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsControlAccount",
					  						DefaultText =  "Control Account",
					  						FullLocalDefaultText =  "חשבון מרכז",
					  						ListFieldLable =  "IsControlAccountListLable",
					  						ListLableDefaultText =  "Control Account",
					  						ListLocalDefaultText =  "חשבון מרכז",
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
					 
					 						FieldName =  "ChartOfAccountsId",
					  						OldFieldName =  "ChartOfAccountsId",
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "ChartOfAccount",
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
					  						PMPropertyPath =  "ChartOfAccountsId",
					  						ListPropertyPath =  "ChartOfAccountsId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ChartOfAccountsId",
					  						DefaultText =  "Chart Of Accounts",
					  						FullLocalDefaultText =  "קבוצת מאזן",
					  						ListFieldLable =  "ChartOfAccountsIdListLable",
					  						ListLableDefaultText =  "Chart Of Accounts ",
					  						ListLocalDefaultText =  "קבוצת מאזן",
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
					  						ObjectTableName =  "GLAccount",
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
					  						ValidForQuerySection1 =  "GLAccount",
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
					 
					 						FieldName =  "AccountTypeName",
					  						OldFieldName =  "AccountTypeName",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "AccountTypeName",
					  						ListPropertyPath =  "AccountTypeName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AccountTypeName",
					  						DefaultText =  "סוג חשבון",
					  						FullLocalDefaultText =  "סוג חשבון",
					  						ListFieldLable =  "AccountTypeNameListLable",
					  						ListLableDefaultText =  "Account Type",
					  						ListLocalDefaultText =  "סוג חשבון",
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
					  						ObjectTableName =  "GLAccount",
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
					  						ValidForQuerySection1 =  "GLAccount",
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
					  						ListLocalDefaultText =  "שם מטבע ",
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
					 
					 						FieldName =  "RevenueExpenseName",
					  						OldFieldName =  "RevenueExpenseName",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "RevenueExpenseName",
					  						ListPropertyPath =  "RevenueExpenseName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "RevenueExpenseName",
					  						DefaultText =  "Revenue/Expense",
					  						FullLocalDefaultText =  "הכנסות/הוצאות",
					  						ListFieldLable =  "RevenueExpenseNameListLable",
					  						ListLableDefaultText =  "Revenue/Expense",
					  						ListLocalDefaultText =  "הכנסות/הוצאות",
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
					 
					 						FieldName =  "ChartOfAccountsName",
					  						OldFieldName =  "ChartOfAccountsName",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "ChartOfAccountsName",
					  						ListPropertyPath =  "ChartOfAccountsName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ChartOfAccountsName",
					  						DefaultText =  "Chart of Accounts",
					  						FullLocalDefaultText =  "קבוצת מאזן",
					  						ListFieldLable =  "ChartOfAccountsNameListLable",
					  						ListLableDefaultText =  "Chart of Accounts",
					  						ListLocalDefaultText =  "קבוצת מאזן",
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
					 
					 						FieldName =  "ChartOfAccountsTypeCode",
					  						OldFieldName =  "ChartOfAccountsTypeCode",
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "ChartOfAccountsType",
					  						MinLength =  1,
					  						MaxLength =  1,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  1,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ChartOfAccountsTypeCode",
					  						ListPropertyPath =  "ChartOfAccountsTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ChartOfAccountsTypeCode",
					  						DefaultText =  "Chart of Accounts Type ",
					  						FullLocalDefaultText =  "סוג קבוצת מאזן",
					  						ListFieldLable =  "ChartOfAccountsTypeCodeListLable",
					  						ListLableDefaultText =  "Chart of Accounts Type ",
					  						ListLocalDefaultText =  "סוג קבוצת מאזן",
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
					 
					 						FieldName =  "ChartOfAccountsTypeName",
					  						OldFieldName =  "ChartOfAccountsTypeName",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "ChartOfAccountsTypeName",
					  						ListPropertyPath =  "ChartOfAccountsTypeName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ChartOfAccountsTypeName",
					  						DefaultText =  "Chart of Accounts Type",
					  						FullLocalDefaultText =  "סוג קבוצת מאזן",
					  						ListFieldLable =  "ChartOfAccountsTypeNameListLable",
					  						ListLableDefaultText =  "Chart of Accounts Type",
					  						ListLocalDefaultText =  "סוג קבוצת מאזן",
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
					  						ObjectTableName =  "GLAccount",
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
					  						DisplayInLookUpIndex =  3,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  2,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CurrencyCode",
					  						DefaultText =  "Currency Code",
					  						FullLocalDefaultText =  "קוד מטבע",
					  						ListFieldLable =  "CurrencyCodeListLable",
					  						ListLableDefaultText =  "Currency Code",
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
					 
					 						FieldName =  "ReconcileMethodCode",
					  						OldFieldName =  "ReconcileMethodCode",
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "ReconcileMethod",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ReconcileMethodCode",
					  						ListPropertyPath =  "ReconcileMethodCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ReconcileMethodCode",
					  						DefaultText =  "Reconcile Method ",
					  						FullLocalDefaultText =  "שיטת התאמה",
					  						ListFieldLable =  "ReconcileMethodCodeListLable",
					  						ListLableDefaultText =  "Reconcile Method ",
					  						ListLocalDefaultText =  "שיטת התאמה",
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
					 
					 						FieldName =  "ReconcileMethodName",
					  						OldFieldName =  "ReconcileMethodName",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "ReconcileMethodName",
					  						ListPropertyPath =  "ReconcileMethodName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ReconcileMethodName",
					  						DefaultText =  "Reconcile Method",
					  						FullLocalDefaultText =  "שיטת התאמה",
					  						ListFieldLable =  "ReconcileMethodNameListLable",
					  						ListLableDefaultText =  "Reconcile Method",
					  						ListLocalDefaultText =  "שיטת התאמה",
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
					 
					 						FieldName =  "ControlAccountId",
					  						OldFieldName =  "ControlAccountId",
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "GLAccount",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ControlAccountId",
					  						ListPropertyPath =  "ControlAccountId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ControlAccountId",
					  						DefaultText =  "Control Account Id",
					  						FullLocalDefaultText =  "מספר חשבון מרכז",
					  						ListFieldLable =  "ControlAccountIdListLable",
					  						ListLableDefaultText =  "Control Account Id",
					  						ListLocalDefaultText =  "מספר חשבון מרכז",
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
					 
					 						FieldName =  "ControlAccountName",
					  						OldFieldName =  "ControlAccountName",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "ControlAccountName",
					  						ListPropertyPath =  "ControlAccountName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ControlAccountName",
					  						DefaultText =  "Control Account Name",
					  						FullLocalDefaultText =  "שם חשבון מרכז",
					  						ListFieldLable =  "ControlAccountNameListLable",
					  						ListLableDefaultText =  "Control Account Name",
					  						ListLocalDefaultText =  "שם חשבון מרכז",
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
					 
					 						FieldName =  "ControlAccountNumber",
					  						OldFieldName =  "ControlAccountNumber",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "ControlAccountNumber",
					  						ListPropertyPath =  "ControlAccountNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ControlAccountNumber",
					  						DefaultText =  "Control Account Number",
					  						FullLocalDefaultText =  "מספר חשבון מרכז",
					  						ListFieldLable =  "ControlAccountNumberListLable",
					  						ListLableDefaultText =  "Control Account Number",
					  						ListLocalDefaultText =  "מספר חשבון מרכז",
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
					 
					 						FieldName =  "ActiveStatusName",
					  						OldFieldName =  "ActiveStatusName",
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "nText",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ActiveStatusName",
					  						ListPropertyPath =  "ActiveStatusName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ActiveStatusName",
					  						DefaultText =  "Status",
					  						FullLocalDefaultText =  "סטטוס",
					  						ListFieldLable =  "ActiveStatusNameListLable",
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
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "OldCurrencyId",
					  						OldFieldName =  "OldCurrencyId",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "OldCurrencyId",
					  						ListPropertyPath =  "OldCurrencyId",
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
					  						FullFieldLable =  "OldCurrencyId",
					  						DefaultText =  "Old CurrencyId",
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
					 
					 						FieldName =  "OldIsMultiCurrency",
					  						OldFieldName =  "OldIsMultiCurrency",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "OldIsMultiCurrency",
					  						ListPropertyPath =  "OldIsMultiCurrency",
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
					  						FullFieldLable =  "OldIsMultiCurrency",
					  						DefaultText =  "Old IsMultiCurrency",
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
					 
					 						FieldName =  "AutomaticReconcileId",
					  						OldFieldName =  "AutomaticReconcileId",
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "AutomaticReconcileMethod",
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
					  						PMPropertyPath =  "AutomaticReconcileId",
					  						ListPropertyPath =  "AutomaticReconcileId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AutomaticReconcileId",
					  						DefaultText =  "Automatic Reconcile Method ",
					  						FullLocalDefaultText =  "התאמה אוטומטית",
					  						ListFieldLable =  "AutomaticReconcileIdListLable",
					  						ListLableDefaultText =  "Automatic Reconcile Method ",
					  						ListLocalDefaultText =  "התאמה אוטומטית",
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
					 
					 						FieldName =  "AutomaticReconcileName",
					  						OldFieldName =  "AutomaticReconcileName",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "AutomaticReconcileName",
					  						ListPropertyPath =  "AutomaticReconcileName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AutomaticReconcileName",
					  						DefaultText =  "Automatic Reconcile",
					  						FullLocalDefaultText =  "התאמה אוטומטית",
					  						ListFieldLable =  "AutomaticReconcileNameListLable",
					  						ListLableDefaultText =  "Automatic Reconcile",
					  						ListLocalDefaultText =  "התאמה אוטומטית",
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
					 
					 						FieldName =  "PreviousEnglishName",
					  						OldFieldName =  "PreviousEnglishName",
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  75,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  75,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PreviousEnglishName",
					  						ListPropertyPath =  "PreviousEnglishName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PreviousEnglishName",
					  						DefaultText =  "Previous English Name",
					  						FullLocalDefaultText =  "שם קודם באנגלית",
					  						ListFieldLable =  "PreviousEnglishNameListLable",
					  						ListLableDefaultText =  "Previous English Name",
					  						ListLocalDefaultText =  "שם קודם באנגלית",
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
					 
					 						FieldName =  "PreviousEnglishNameChangeDate",
					  						OldFieldName =  "PreviousEnglishNameChangeDate",
					  						ObjectTableName =  "GLAccount",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Between",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PreviousEnglishNameChangeDate",
					  						ListPropertyPath =  "PreviousEnglishNameChangeDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PreviousEnglishNameChangeDate",
					  						DefaultText =  "English Name Change Date",
					  						FullLocalDefaultText =  "תאריך שינוי שם באנגלית",
					  						ListFieldLable =  "PreviousEnglishNameChangeDateListLable",
					  						ListLableDefaultText =  "English Name Change Date",
					  						ListLocalDefaultText =  "תאריך שינוי שם באנגלית",
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
					 
					 						FieldName =  "PreviousLocalName",
					  						OldFieldName =  "PreviousLocalName",
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "nText",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PreviousLocalName",
					  						ListPropertyPath =  "PreviousLocalName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PreviousLocalName",
					  						DefaultText =  "Previous Local Name",
					  						FullLocalDefaultText =  "שם קודם מקומי",
					  						ListFieldLable =  "PreviousLocalNameListLable",
					  						ListLableDefaultText =  "Previous Local Name",
					  						ListLocalDefaultText =  "שם קודם מקומי",
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
					 
					 						FieldName =  "PreviousLocalNameChangeDate",
					  						OldFieldName =  "PreviousLocalNameChangeDate",
					  						ObjectTableName =  "GLAccount",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Between",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PreviousLocalNameChangeDate",
					  						ListPropertyPath =  "PreviousLocalNameChangeDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PreviousLocalNameChangeDate",
					  						DefaultText =  "Local Name Change Date",
					  						FullLocalDefaultText =  "תאריך שינוי שם מקומי",
					  						ListFieldLable =  "PreviousLocalNameChangeDateListLable",
					  						ListLableDefaultText =  "Local Name Change Date",
					  						ListLocalDefaultText =  "תאריך שינוי שם מקומי",
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
					 
					 						FieldName =  "PreviousNumber",
					  						OldFieldName =  "PreviousNumber",
					  						ObjectTableName =  "GLAccount",
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
					  						SystemMaxLength =  15,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PreviousNumber",
					  						ListPropertyPath =  "PreviousNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PreviousNumber",
					  						DefaultText =  "Previous Number",
					  						FullLocalDefaultText =  "מספר קודם",
					  						ListFieldLable =  "PreviousNumberListLable",
					  						ListLableDefaultText =  "Previous Number",
					  						ListLocalDefaultText =  "מספר קודם",
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
					 
					 						FieldName =  "PreviousNumberChangeDate",
					  						OldFieldName =  "PreviousNumberChangeDate",
					  						ObjectTableName =  "GLAccount",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Between",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PreviousNumberChangeDate",
					  						ListPropertyPath =  "PreviousNumberChangeDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PreviousNumberChangeDate",
					  						DefaultText =  "Number Change Date",
					  						FullLocalDefaultText =  "תאריך שינוי מספר",
					  						ListFieldLable =  "PreviousNumberChangeDateListLable",
					  						ListLableDefaultText =  "Number Change Date",
					  						ListLocalDefaultText =  "תאריך שינוי מספר",
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
					 
					 						FieldName =  "PreviousChartOfAccountsId",
					  						OldFieldName =  "PreviousChartOfAccountsId",
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "LookUp",
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
					  						PMPropertyPath =  "PreviousChartOfAccountsId",
					  						ListPropertyPath =  "PreviousChartOfAccountsId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PreviousChartOfAccountsId",
					  						DefaultText =  "Previous Chart of Accounts ",
					  						FullLocalDefaultText =  "קבוצת מאזן קודמת",
					  						ListFieldLable =  "PreviousChartOfAccountsIdListLable",
					  						ListLableDefaultText =  "Previous Chart of Accounts ",
					  						ListLocalDefaultText =  "קבוצת מאזן קודמת",
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
					 
					 						FieldName =  "PreviousChartOfAccountsChangeDate",
					  						OldFieldName =  "PreviousChartOfAccountsChangeDate",
					  						ObjectTableName =  "GLAccount",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Between",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PreviousChartOfAccountsChangeDate",
					  						ListPropertyPath =  "PreviousChartOfAccountsChangeDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PreviousChartOfAccountsChangeDate",
					  						DefaultText =  "Chart of Accounts Change Date",
					  						FullLocalDefaultText =  "תאריך שינוי קבוצת מאזן",
					  						ListFieldLable =  "PreviousChartOfAccountsChangeDateListLable",
					  						ListLableDefaultText =  "Chart of Accounts Change Date",
					  						ListLocalDefaultText =  "תאריך שינוי קבוצת מאזן",
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
					 
					 						FieldName =  "CustomerGLAccountId",
					  						OldFieldName =  "CustomerGLAccountId",
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "GLAccount",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CustomerGLAccountId",
					  						ListPropertyPath =  "CustomerGLAccountId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccountId",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CustomerGLAccountId",
					  						DefaultText =  "Client Account Id",
					  						FullLocalDefaultText =  "מזהה חשבון לקוח",
					  						ListFieldLable =  "CustomerGLAccountIdListLable",
					  						ListLableDefaultText =  "Client Account Id",
					  						ListLocalDefaultText =  "מזהה חשבון לקוח",
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
					 
					 						FieldName =  "CustomerGLAccountName",
					  						OldFieldName =  "CustomerGLAccountName",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "CustomerGLAccountName",
					  						ListPropertyPath =  "CustomerGLAccountName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CustomerGLAccountName",
					  						DefaultText =  "Customer Account Name",
					  						FullLocalDefaultText =  "שם חשבון לקוח",
					  						ListFieldLable =  "CustomerGLAccountNameListLable",
					  						ListLableDefaultText =  "Customer Account Name",
					  						ListLocalDefaultText =  "שם חשבון לקוח",
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
					 
					 						FieldName =  "CustomerGLAccountNumber",
					  						OldFieldName =  "CustomerGLAccountNumber",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "CustomerGLAccountNumber",
					  						ListPropertyPath =  "CustomerGLAccountNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CustomerGLAccountNumber",
					  						DefaultText =  "Customer Account Number",
					  						FullLocalDefaultText =  "מספר חשבון לקוח",
					  						ListFieldLable =  "CustomerGLAccountNumberListLable",
					  						ListLableDefaultText =  "Customer Account Number",
					  						ListLocalDefaultText =  "מספר חשבון לקוח",
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
					 
					 						FieldName =  "BalanceInLocalCurrency",
					  						OldFieldName =  "BalanceInLocalCurrency",
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "Decimal",
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
					  						Operator =  "GreaterThanOrEqual",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "BalanceInLocalCurrency",
					  						ListPropertyPath =  "BalanceInLocalCurrency",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
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
					  						FullFieldLable =  "BalanceInLocalCurrency",
					  						DefaultText =  "Balance in Local Currency",
					  						FullLocalDefaultText =  "יתרה במטבע מקומי",
					  						ListFieldLable =  "BalanceInLocalCurrencyListLable",
					  						ListLableDefaultText =  "Balance in Local Currency",
					  						ListLocalDefaultText =  "יתרה במטבע מקומי",
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
					 
					 						FieldName =  "RevaluationEnabled",
					  						OldFieldName =  "RevaluationEnabled",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "RevaluationEnabled",
					  						ListPropertyPath =  "RevaluationEnabled",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "RevaluationEnabled",
					  						DefaultText =  "Revaluation Enabled",
					  						FullLocalDefaultText =  "שערוך אוטומטי",
					  						ListFieldLable =  "RevaluationEnabledListLable",
					  						ListLableDefaultText =  "Revaluation Enabled",
					  						ListLocalDefaultText =  "שערוך אוטומטי",
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
					 
					 						FieldName =  "ParentAccountId",
					  						OldFieldName =  "ParentAccountId",
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "GLAccount",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ParentAccountId",
					  						ListPropertyPath =  "ParentAccountId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ParentAccountId",
					  						DefaultText =  "Parent Account ",
					  						FullLocalDefaultText =  "מספר חשבון אב",
					  						ListFieldLable =  "ParentAccountIdListLable",
					  						ListLableDefaultText =  "Parent Account ",
					  						ListLocalDefaultText =  "מספר חשבון אב",
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
					 
					 						FieldName =  "ParentAccountName",
					  						OldFieldName =  "ParentAccountName",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "ParentAccountName",
					  						ListPropertyPath =  "ParentAccountName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ParentAccountName",
					  						DefaultText =  "Parent Account Name",
					  						FullLocalDefaultText =  "שם חשבון אב",
					  						ListFieldLable =  "ParentAccountNameListLable",
					  						ListLableDefaultText =  "Parent Account Name",
					  						ListLocalDefaultText =  "שם חשבון אב",
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
					 
					 						FieldName =  "ParentAccountNumber",
					  						OldFieldName =  "ParentAccountNumber",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "ParentAccountNumber",
					  						ListPropertyPath =  "ParentAccountNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ParentAccountNumber",
					  						DefaultText =  "Parent Account Number",
					  						FullLocalDefaultText =  "מספר חשבון אב",
					  						ListFieldLable =  "ParentAccountNumberListLable",
					  						ListLableDefaultText =  "Parent Account Number",
					  						ListLocalDefaultText =  "מספר חשבון אב",
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
					 
					 						FieldName =  "CustomerGLAccountInternalNumber",
					  						OldFieldName =  "CustomerGLAccountInternalNumber",
					  						ObjectTableName =  "GLAccount",
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
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CustomerGLAccountInternalNumber",
					  						ListPropertyPath =  "CustomerGLAccountInternalNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CustomerGLAccountInternalNumber",
					  						DefaultText =  "Customer GL Account Internal Number",
					  						FullLocalDefaultText =  "מספר מערכת של חשבון לקוח",
					  						ListFieldLable =  "CustomerGLAccountInternalNumberListLable",
					  						ListLableDefaultText =  "Customer GL Account Internal Number",
					  						ListLocalDefaultText =  "מספר מערכת של חשבון לקוח",
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
					 
					 						FieldName =  "Category1Id",
					  						OldFieldName =  "Category1Id",
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Category1",
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
					  						PMPropertyPath =  "Category1Id",
					  						ListPropertyPath =  "Category1Id",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Category1Id",
					  						DefaultText =  "Category 1 ",
					  						FullLocalDefaultText =  "מזהה קטגוריה 1",
					  						ListFieldLable =  "Category1IdListLable",
					  						ListLableDefaultText =  "Category 1 ",
					  						ListLocalDefaultText =  "מזהה קטגוריה 1",
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
					 
					 						FieldName =  "Category1Name",
					  						OldFieldName =  "Category1Name",
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "nText",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Category1Name",
					  						ListPropertyPath =  "Category1Name",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Category1Name",
					  						DefaultText =  "Category 1 Name",
					  						FullLocalDefaultText =  "שם קטגוריה 1",
					  						ListFieldLable =  "Category1NameListLable",
					  						ListLableDefaultText =  "Category 1 Name",
					  						ListLocalDefaultText =  "שם קטגוריה 1",
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
					 
					 						FieldName =  "Category2Id",
					  						OldFieldName =  "Category2Id",
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Category2",
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
					  						PMPropertyPath =  "Category2Id",
					  						ListPropertyPath =  "Category2Id",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Category2Id",
					  						DefaultText =  "Category 2 ",
					  						FullLocalDefaultText =  "מזהה קטגוריה 2",
					  						ListFieldLable =  "Category2IdListLable",
					  						ListLableDefaultText =  "Category 2 ",
					  						ListLocalDefaultText =  "מזהה קטגוריה 2",
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
					 
					 						FieldName =  "Category2Name",
					  						OldFieldName =  "Category2Name",
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "nText",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Category2Name",
					  						ListPropertyPath =  "Category2Name",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Category2Name",
					  						DefaultText =  "Category 2 Name",
					  						FullLocalDefaultText =  "שם קטגוריה 2",
					  						ListFieldLable =  "Category2NameListLable",
					  						ListLableDefaultText =  "Category 2 Name",
					  						ListLocalDefaultText =  "שם קטגוריה 2",
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
					 
					 						FieldName =  "Category3Id",
					  						OldFieldName =  "Category3Id",
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Category3",
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
					  						PMPropertyPath =  "Category3Id",
					  						ListPropertyPath =  "Category3Id",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Category3Id",
					  						DefaultText =  "Category 3 ",
					  						FullLocalDefaultText =  "מזהה קטגוריה 3",
					  						ListFieldLable =  "Category3IdListLable",
					  						ListLableDefaultText =  "Category 3 ",
					  						ListLocalDefaultText =  "מזהה קטגוריה 3",
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
					 
					 						FieldName =  "Category3Name",
					  						OldFieldName =  "Category3Name",
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "nText",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Category3Name",
					  						ListPropertyPath =  "Category3Name",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Category3Name",
					  						DefaultText =  "Category 3 Name",
					  						FullLocalDefaultText =  "שם קטגוריה 3",
					  						ListFieldLable =  "Category3NameListLable",
					  						ListLableDefaultText =  "Category 3 Name",
					  						ListLocalDefaultText =  "שם קטגוריה 3",
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
					 
					 						FieldName =  "Category4Id",
					  						OldFieldName =  "Category4Id",
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Category4",
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
					  						PMPropertyPath =  "Category4Id",
					  						ListPropertyPath =  "Category4Id",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Category4Id",
					  						DefaultText =  "Category 4 ",
					  						FullLocalDefaultText =  "מזהה קטגוריה 4",
					  						ListFieldLable =  "Category4IdListLable",
					  						ListLableDefaultText =  "Category 4 ",
					  						ListLocalDefaultText =  "מזהה קטגוריה 4",
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
					 
					 						FieldName =  "Category4Name",
					  						OldFieldName =  "Category4Name",
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "nText",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Category4Name",
					  						ListPropertyPath =  "Category4Name",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Category4Name",
					  						DefaultText =  "Category 4 Name",
					  						FullLocalDefaultText =  "שם קטגוריה 4",
					  						ListFieldLable =  "Category4NameListLable",
					  						ListLableDefaultText =  "Category 4 Name",
					  						ListLocalDefaultText =  "שם קטגוריה 4",
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
					 
					 						FieldName =  "Category5Id",
					  						OldFieldName =  "Category5Id",
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Category5",
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
					  						PMPropertyPath =  "Category5Id",
					  						ListPropertyPath =  "Category5Id",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Category5Id",
					  						DefaultText =  "Category 5 ",
					  						FullLocalDefaultText =  "מזהה קטגוריה 5",
					  						ListFieldLable =  "Category5IdListLable",
					  						ListLableDefaultText =  "Category 5 ",
					  						ListLocalDefaultText =  "מזהה קטגוריה 5",
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
					 
					 						FieldName =  "Category5Name",
					  						OldFieldName =  "Category5Name",
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "nText",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Category5Name",
					  						ListPropertyPath =  "Category5Name",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Category5Name",
					  						DefaultText =  "Categor 5 Name",
					  						FullLocalDefaultText =  "שם קטגוריה ",
					  						ListFieldLable =  "Category5NameListLable",
					  						ListLableDefaultText =  "Categor 5 Name",
					  						ListLocalDefaultText =  "שם קטגוריה ",
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
					 
					 						FieldName =  "IsVATExempt",
					  						OldFieldName =  "IsVATExempt",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "IsVATExempt",
					  						ListPropertyPath =  "IsVATExempt",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsVATExempt",
					  						DefaultText =  "VAT Exempt",
					  						FullLocalDefaultText =  "פטור ממעמ",
					  						ListFieldLable =  "IsVATExemptListLable",
					  						ListLableDefaultText =  "VAT Exempt",
					  						ListLocalDefaultText =  "פטור ממעמ",
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
					 
					 						FieldName =  "ChartOfAccountsCode",
					  						OldFieldName =  "ChartOfAccountsCode",
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  5,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  5,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ChartOfAccountsCode",
					  						ListPropertyPath =  "ChartOfAccountsCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ChartOfAccountsCode",
					  						DefaultText =  "Chart of Accounts Code",
					  						FullLocalDefaultText =  "קוד קבוצת מאזן",
					  						ListFieldLable =  "ChartOfAccountsCodeListLable",
					  						ListLableDefaultText =  "Chart of Accounts Code",
					  						ListLocalDefaultText =  "קוד קבוצת מאזן",
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
					 
					 						FieldName =  "CustomerCode",
					  						OldFieldName =  "CustomerCode",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "CustomerCode",
					  						ListPropertyPath =  "CustomerCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CustomerCode",
					  						DefaultText =  "Customer Code",
					  						FullLocalDefaultText =  "קוד לקוח",
					  						ListFieldLable =  "CustomerCodeListLable",
					  						ListLableDefaultText =  "Customer Code",
					  						ListLocalDefaultText =  "קוד לקוח",
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
					 
					 						FieldName =  "ParentAccountByCurrency",
					  						OldFieldName =  "ParentAccountByCurrency",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "ParentAccountByCurrency",
					  						ListPropertyPath =  "ParentAccountByCurrency",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ParentAccountByCurrency",
					  						DefaultText =  "Parent Account by Currency",
					  						FullLocalDefaultText =  "חשבון אב לפי מטבע",
					  						ListFieldLable =  "ParentAccountByCurrencyListLable",
					  						ListLableDefaultText =  "Parent Account by Currency",
					  						ListLocalDefaultText =  "חשבון אב לפי מטבע",
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
					  						ObjectTableName =  "GLAccount",
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
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LastActivityDate",
					  						DefaultText =  "Last Activity Date",
					  						ListFieldLable =  "LastActivityDateListLable",
					  						ListLableDefaultText =  "Last Activity Date",
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
					  						ObjectTableName =  "GLAccount",
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
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LastActivityTypeName",
					  						DefaultText =  "LastActivityTypeName",
					  						ListFieldLable =  "LastActivityTypeNameListLable",
					  						ListLableDefaultText =  "LastActivityTypeName",
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
					  						ObjectTableName =  "GLAccount",
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
					  						ListFieldLable =  "LastActivityByUserNameListLable",
					  						ListLableDefaultText =  "LastActivityByUserName",
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
					 
					 						FieldName =  "VatNumber",
					  						OldFieldName =  "VatNumber",
					  						ObjectTableName =  "GLAccount",
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
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "VatNumber",
					  						DefaultText =  "VAT No.",
					  						FullLocalDefaultText =  "מספר ח.פ",
					  						ListFieldLable =  "VatNumberListLable",
					  						ListLableDefaultText =  "VAT No.",
					  						ListLocalDefaultText =  "מספר ח.פ",
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
					 
					 						FieldName =  "PaymentTermId",
					  						OldFieldName =  "PaymentTermId",
					  						ObjectTableName =  "GLAccount",
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
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
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
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CollectorId",
					  						OldFieldName =  "CollectorId",
					  						ObjectTableName =  "GLAccount",
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
					  						SystemMaxLength =  15,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CollectorId",
					  						ListPropertyPath =  "CollectorId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Card",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CollectorId",
					  						DefaultText =  "Collector",
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
					 
					 						FieldName =  "SalesmanUserId",
					  						OldFieldName =  "SalesmanUserId",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "SalesmanUserId",
					  						ListPropertyPath =  "SalesmanUserId",
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
					  						FullFieldLable =  "SalesmanUserId",
					  						DefaultText =  "Salesman",
					  						ListFieldLable =  "SalesmanUserIdListLable",
					  						ListLableDefaultText =  "Salesman",
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
					 
					 						FieldName =  "NewGLAccountCardId",
					  						OldFieldName =  "NewGLAccountCardId",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "NewGLAccountCardId",
					  						ListPropertyPath =  "NewGLAccountCardId",
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
					  						FullFieldLable =  "NewGLAccountCardId",
					  						DefaultText =  "New GL Account Card ",
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
					 
					 						FieldName =  "LocalBalanceInDue",
					  						OldFieldName =  "LocalBalanceInDue",
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "Decimal",
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
					  						Operator =  "GreaterThanOrEqual",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "LocalBalanceInDue",
					  						ListPropertyPath =  "LocalBalanceInDue",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
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
					  						FullFieldLable =  "LocalBalanceInDue",
					  						DefaultText =  "Due Balance",
					  						FullLocalDefaultText =  "יתרה לגביה",
					  						ListFieldLable =  "LocalBalanceInDueListLable",
					  						ListLableDefaultText =  "Due Balance",
					  						ListLocalDefaultText =  "יתרה לגביה",
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
					 
					 						FieldName =  "NextDueDate",
					  						OldFieldName =  "NextDueDate",
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "Date",
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
					  						Operator =  "GreaterThanOrEqual",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "NextDueDate",
					  						ListPropertyPath =  "NextDueDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "NextDueDate",
					  						DefaultText =  "Next Due Date",
					  						FullLocalDefaultText =  "תאריך הבא לחישוב יתרה לפירעון",
					  						ListFieldLable =  "NextDueDateListLable",
					  						ListLableDefaultText =  "Next Due Date",
					  						ListLocalDefaultText =  "תאריך הבא לחישוב יתרה לפירעון",
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
					  						ObjectTableName =  "GLAccount",
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
					  						DefaultText =  "Currency ",
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
					 
					 						FieldName =  "ConnectedItems",
					  						OldFieldName =  "ConnectedItems",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "ConnectedItems",
					  						ListPropertyPath =  "ConnectedItems",
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
					  						FullFieldLable =  "ConnectedItems",
					  						DefaultText =  "ConnectedItems",
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
					 
					 						FieldName =  "Type",
					  						OldFieldName =  "Type",
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  10,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  10,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Type",
					  						ListPropertyPath =  "Type",
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
					  						FullFieldLable =  "Type",
					  						DefaultText =  "type",
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
					 
					 						FieldName =  "DeductionFileTypeId",
					  						OldFieldName =  "DeductionFileTypeId",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "DeductionFileTypeId",
					  						ListPropertyPath =  "DeductionFileTypeId",
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
					  						FullFieldLable =  "DeductionFileTypeId",
					  						DefaultText =  "Deduction File Type",
					  						FullLocalDefaultText =  "סוג עיסוק",
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
					 
					 						FieldName =  "DeductionFileNumber",
					  						OldFieldName =  "DeductionFileNumber",
					  						ObjectTableName =  "GLAccount",
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
					  						SystemMaxLength =  30,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DeductionFileNumber",
					  						ListPropertyPath =  "DeductionFileNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DeductionFileNumber",
					  						DefaultText =  "Deduction File No.",
					  						FullLocalDefaultText =  "תיק ניכויים",
					  						ListFieldLable =  "DeductionFileNumberListLable",
					  						ListLableDefaultText =  "Deduction File No.",
					  						ListLocalDefaultText =  "תיק ניכויים",
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
					 
					 						FieldName =  "AssessingOfficeCode",
					  						OldFieldName =  "AssessingOfficeCode",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "AssessingOfficeCode",
					  						ListPropertyPath =  "AssessingOfficeCode",
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
					  						FullFieldLable =  "AssessingOfficeCode",
					  						DefaultText =  "Assessing Office Code",
					  						FullLocalDefaultText =  "פקיד שומה",
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
					 
					 						FieldName =  "Occupation",
					  						OldFieldName =  "Occupation",
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "nText",
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
					  						PMPropertyPath =  "Occupation",
					  						ListPropertyPath =  "Occupation",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Occupation",
					  						DefaultText =  "Occupation",
					  						FullLocalDefaultText =  "עיסוק",
					  						ListFieldLable =  "OccupationListLable",
					  						ListLableDefaultText =  "Occupation",
					  						ListLocalDefaultText =  "עיסוק",
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
					 
					 						FieldName =  "DeductionTypeId",
					  						OldFieldName =  "DeductionTypeId",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "DeductionTypeId",
					  						ListPropertyPath =  "DeductionTypeId",
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
					  						FullFieldLable =  "DeductionTypeId",
					  						DefaultText =  "Deduction Type",
					  						FullLocalDefaultText =  "סוג תיק ניכויים",
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
					 
					 						FieldName =  "ConsolidationVat",
					  						OldFieldName =  "ConsolidationVat",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "ConsolidationVat",
					  						ListPropertyPath =  "ConsolidationVat",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ConsolidationVat",
					  						DefaultText =  "Consolidation Vat",
					  						FullLocalDefaultText =  "איחוד עוסקים",
					  						ListFieldLable =  "ConsolidationVatListLable",
					  						ListLableDefaultText =  "Consolidation Vat",
					  						ListLocalDefaultText =  "איחוד עוסקים",
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
					 
					 						FieldName =  "GLAccountWithholdingTaxes",
					  						OldFieldName =  "GLAccountWithholdingTaxes",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "GLAccountWithholdingTaxes",
					  						ListPropertyPath =  "GLAccountWithholdingTaxes",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  true,
					  						MultiTableName =  "GLAccountWithholdingTax",
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
					  						FullFieldLable =  "GLAccountWithholdingTaxes",
					  						DefaultText =  "GLAccountWithholdingTaxes",
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
					 
					 						FieldName =  "TaxWithholdingLastLine",
					  						OldFieldName =  "TaxWithholdingLastLine",
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "Integer",
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
					  						PMPropertyPath =  "TaxWithholdingLastLine",
					  						ListPropertyPath =  "TaxWithholdingLastLine",
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
					  						FullFieldLable =  "TaxWithholdingLastLine",
					  						DefaultText =  "TaxWithholdingLastLine",
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
					 
					 						FieldName =  "ReconcilationCount",
					  						OldFieldName =  "ReconcilationCount",
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "Integer",
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
					  						PMPropertyPath =  "ReconcilationCount",
					  						ListPropertyPath =  "ReconcilationCount",
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
					  						FullFieldLable =  "ReconcilationCount",
					  						DefaultText =  "ReconcilationCount",
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
					 
					 						FieldName =  "IsEquipmentVendor",
					  						OldFieldName =  "IsEquipmentVendor",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "IsEquipmentVendor",
					  						ListPropertyPath =  "IsEquipmentVendor",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsEquipmentVendor",
					  						DefaultText =  "Is Equipment Supplier",
					  						FullLocalDefaultText =  "ספק ציוד",
					  						ListFieldLable =  "IsEquipmentVendorListLable",
					  						ListLableDefaultText =  "Is Equipment Supplier",
					  						ListLocalDefaultText =  "ספק ציוד",
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
					 
					 						FieldName =  "ExcludeFromDeductionReport",
					  						OldFieldName =  "IsPartOfDeductionReport",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "ExcludeFromDeductionReport",
					  						ListPropertyPath =  "ExcludeFromDeductionReport",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ExcludeFromDeductionReport",
					  						DefaultText =  "Exclude from deduction report",
					  						FullLocalDefaultText =  "לא לכלול בדוח ניכויים",
					  						ListFieldLable =  "ExcludeFromDeductionReportListLable",
					  						ListLableDefaultText =  "Exclude from deduction report",
					  						ListLocalDefaultText =  "לא לכלול בדוח ניכויים",
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
					 
					 						FieldName =  "Parent",
					  						OldFieldName =  "Parent",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "Parent",
					  						ListPropertyPath =  "Parent",
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
					  						FullFieldLable =  "Parent",
					  						DefaultText =  "Parent",
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
					 
					 						FieldName =  "DeductionTypeName",
					  						OldFieldName =  "DeductionTypeName",
					  						ObjectTableName =  "GLAccount",
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
					  						SystemMaxLength =  100,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DeductionTypeName",
					  						ListPropertyPath =  "DeductionTypeName",
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
					  						FullFieldLable =  "DeductionTypeName",
					  						DefaultText =  "DeductionTypeName",
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
					 
					 						FieldName =  "DeductionFileTypeCode",
					  						OldFieldName =  "DeductionFileTypeCode",
					  						ObjectTableName =  "GLAccount",
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
					  						SystemMaxLength =  3,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DeductionFileTypeCode",
					  						ListPropertyPath =  "DeductionFileTypeCode",
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
					  						FullFieldLable =  "DeductionFileTypeCode",
					  						DefaultText =  "DeductionFileTypeCode",
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
					 
					 						FieldName =  "DeductionFileTypeName",
					  						OldFieldName =  "DeductionFileTypeName",
					  						ObjectTableName =  "GLAccount",
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
					  						SystemMaxLength =  100,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DeductionFileTypeName",
					  						ListPropertyPath =  "DeductionFileTypeName",
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
					  						FullFieldLable =  "DeductionFileTypeName",
					  						DefaultText =  "DeductionFileTypeName",
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
					 
					 						FieldName =  "AssessingOfficeName",
					  						OldFieldName =  "AssessingOfficeName",
					  						ObjectTableName =  "GLAccount",
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
					  						SystemMaxLength =  100,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AssessingOfficeName",
					  						ListPropertyPath =  "AssessingOfficeName",
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
					  						FullFieldLable =  "AssessingOfficeName",
					  						DefaultText =  "AssessingOfficeName",
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
					 
					 						FieldName =  "DeductionTypeEnglishName",
					  						OldFieldName =  "DeductionTypeEnglishName",
					  						ObjectTableName =  "GLAccount",
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
					  						SystemMaxLength =  100,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DeductionTypeEnglishName",
					  						ListPropertyPath =  "DeductionTypeEnglishName",
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
					  						FullFieldLable =  "DeductionTypeEnglishName",
					  						DefaultText =  "DeductionTypeEnglishName",
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
					 
					 						FieldName =  "TotalOpenChequesInLocalCur",
					  						OldFieldName =  "TotalOpenChequesInLocalCur",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "TotalOpenChequesInLocalCur",
					  						ListPropertyPath =  "TotalOpenChequesInLocalCur",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
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
					  						FullFieldLable =  "TotalOpenChequesInLocalCur",
					  						DefaultText =  "Unredeemed Cheques",
					  						FullLocalDefaultText =  "המחאות שלא נפרעו",
					  						ListFieldLable =  "TotalOpenChequesInLocalCurListLable",
					  						ListLableDefaultText =  "Unredeemed Cheques",
					  						ListLocalDefaultText =  "המחאות שלא נפרעו",
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
					 
					 						FieldName =  "AutomaticReconcileLocalName",
					  						OldFieldName =  "AutomaticReconcileLocalName",
					  						ObjectTableName =  "GLAccount",
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
					  						SystemMaxLength =  100,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AutomaticReconcileLocalName",
					  						ListPropertyPath =  "AutomaticReconcileLocalName",
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
					  						FullFieldLable =  "AutomaticReconcileLocalName",
					  						DefaultText =  "AutomaticReconcileName",
					  						ListFieldLable =  "AutomaticReconcileLocalNameListLable",
					  						ListLableDefaultText =  "AutomaticReconcileName",
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
					 
					 						FieldName =  "ReconcileMethodLocalName",
					  						OldFieldName =  "ReconcileMethodLocalName",
					  						ObjectTableName =  "GLAccount",
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
					  						SystemMaxLength =  100,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ReconcileMethodLocalName",
					  						ListPropertyPath =  "ReconcileMethodLocalName",
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
					  						FullFieldLable =  "ReconcileMethodLocalName",
					  						DefaultText =  "ReconcileMethodName",
					  						ListFieldLable =  "ReconcileMethodLocalNameListLable",
					  						ListLableDefaultText =  "ReconcileMethodName",
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
					 
					 						FieldName =  "TotFutureOpenChequesInLocalCur",
					  						OldFieldName =  "TotFutureOpenChequesInLocalCur",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "TotFutureOpenChequesInLocalCur",
					  						ListPropertyPath =  "TotFutureOpenChequesInLocalCur",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
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
					  						FullFieldLable =  "TotFutureOpenChequesInLocalCur",
					  						DefaultText =  "Unredeemed Future Cheques",
					  						ListFieldLable =  "TotFutureOpenChequesInLocalCurListLable",
					  						ListLableDefaultText =  "Unredeemed Future Cheques",
					  						ListLocalDefaultText =  "המחאות עתידיות שלא נפרעו",
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
					 
					 						FieldName =  "CardId",
					  						OldFieldName =  "CardId",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "CardId",
					  						ListPropertyPath =  "CardId",
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
					  						FullFieldLable =  "CardId",
					  						DefaultText =  "CardId",
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
					 
					 						FieldName =  "CreatedByUserId",
					  						OldFieldName =  "CreatedByUserId",
					  						ObjectTableName =  "GLAccount",
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
					  						ValidForQuerySection1 =  "GLAccount",
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
					  						FullLocalDefaultText =  "נוצר על ידי",
					  						ListFieldLable =  "CreatedByUserIdListLable",
					  						ListLableDefaultText =  "Created By",
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
					 
					 						FieldName =  "UpdatedByUserId",
					  						OldFieldName =  "UpdatedByUserId",
					  						ObjectTableName =  "GLAccount",
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
					  						ValidForQuerySection1 =  "GLAccount",
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
					  						FullLocalDefaultText =  "עודכן על ידי",
					  						ListFieldLable =  "UpdatedByUserIdListLable",
					  						ListLableDefaultText =  "Updated By",
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
					 
					 						FieldName =  "CreateDate",
					  						OldFieldName =  "CreateDate",
					  						ObjectTableName =  "GLAccount",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "GreaterThanOrEqual",
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
					  						ValidForQuerySection1 =  "GLAccount",
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
					 
					 						FieldName =  "UpdateDate",
					  						OldFieldName =  "UpdateDate",
					  						ObjectTableName =  "GLAccount",
					  						FieldsDataType =  "Date",
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
					  						Operator =  "GreaterThanOrEqual",
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
					  						ValidForQuerySection1 =  "GLAccount",
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
					  						ObjectTableName =  "GLAccount",
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
					  						ValidForQuerySection1 =  "GLAccount",
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
					  						FullLocalDefaultText =  "Created By",
					  						ListFieldLable =  "CreatedByUserNameListLable",
					  						ListLableDefaultText =  "Created By",
					  						ListLocalDefaultText =  "נוצר על ידי",
					  						HelpTextCode =  "CreatedByUserName",
					  						HelpTextDefaultText =  "נוצר על ידי",
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
					 
					 						FieldName =  "UpdatedByUserName",
					  						OldFieldName =  "UpdatedByUserName",
					  						ObjectTableName =  "GLAccount",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
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
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UpdatedByUserName",
					  						DefaultText =  "Updated By",
					  						FullLocalDefaultText =  "עודכן על ידי",
					  						ListFieldLable =  "UpdatedByUserNameListLable",
					  						ListLableDefaultText =  "Updated By",
					  						ListLocalDefaultText =  "עודכן על ידי",
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
					 
					 						FieldName =  "CreatedByLocalName",
					  						OldFieldName =  "CreatedByLocalName",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "CreatedByLocalName",
					  						ListPropertyPath =  "CreatedByLocalName",
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
					  						FullFieldLable =  "CreatedByLocalName",
					  						DefaultText =  "Created By",
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
					 
					 						FieldName =  "UpdatedByLocalName",
					  						OldFieldName =  "UpdatedByLocalName",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "UpdatedByLocalName",
					  						ListPropertyPath =  "UpdatedByLocalName",
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
					  						FullFieldLable =  "UpdatedByLocalName",
					  						DefaultText =  "Updated By",
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
					 
					 						FieldName =  "CardCode",
					  						OldFieldName =  "CardCode",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "CardCode",
					  						ListPropertyPath =  "CardCode",
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
					  						FullFieldLable =  "CardCode",
					  						DefaultText =  "CardCode",
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
					 
					 						FieldName =  "PartnerTypeId",
					  						OldFieldName =  "PartnerTypeId",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "PartnerTypeId",
					  						ListPropertyPath =  "PartnerTypeId",
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
					  						FullFieldLable =  "PartnerTypeId",
					  						DefaultText =  "PartnerTypeId",
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
					 
					 						FieldName =  "AllowEditChequePayToName",
					  						OldFieldName =  "AllowEditChequePayToName",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "AllowEditChequePayToName",
					  						ListPropertyPath =  "AllowEditChequePayToName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "GLAccount",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AllowEditChequePayToName",
					  						DefaultText =  "Allow Editing Cheque Pay To Name",
					  						FullLocalDefaultText =  "אפשר עדכון פרטי משלם בהמחאה",
					  						ListFieldLable =  "AllowEditChequePayToNameListLable",
					  						ListLableDefaultText =  "Allow Editing Cheque Pay To Name",
					  						ListLocalDefaultText =  "אפשר עדכון פרטי משלם בהמחאה",
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
					 
					 						FieldName =  "ActiveForInterest",
					  						OldFieldName =  "ActiveForInterest",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "ActiveForInterest",
					  						ListPropertyPath =  "ActiveForInterest",
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
					  						FullFieldLable =  "ActiveForInterest",
					  						DefaultText =  "Active for Interest",
					  						FullLocalDefaultText =  "פעיל לריבית",
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
					 
					 						FieldName =  "InterestCalculationStartDate",
					  						OldFieldName =  "InterestCalculationStartDate",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "InterestCalculationStartDate",
					  						ListPropertyPath =  "InterestCalculationStartDate",
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
					  						FullFieldLable =  "InterestCalculationStartDate",
					  						DefaultText =  "Interest Calculation Start Date",
					  						FullLocalDefaultText =  "תאריך לחישוב ריבית",
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
					 
					 						FieldName =  "ActiveForInterestCreditInvoice",
					  						OldFieldName =  "ActiveForInterestCreditInvoice",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "ActiveForInterestCreditInvoice",
					  						ListPropertyPath =  "ActiveForInterestCreditInvoice",
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
					  						FullFieldLable =  "ActiveForInterestCreditInvoice",
					  						DefaultText =  "Active for Interest",
					  						FullLocalDefaultText =  "פעיל לחשבונית זיכוי",
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
					 
					 						FieldName =  "MinimumInterestInvoiceBilling",
					  						OldFieldName =  "MinimumInterestInvoiceBilling",
					  						ObjectTableName =  "GLAccount",
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
					  						PMPropertyPath =  "MinimumInterestInvoiceBilling",
					  						ListPropertyPath =  "MinimumInterestInvoiceBilling",
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
					  						NumberOfDigits =  5,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "MinimumInterestInvoiceBilling",
					  						DefaultText =  "Minimum Interest Invoice billing",
					  						FullLocalDefaultText =  "מינימום חיוב בחשבונית ריבית",
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
	        QueryGroup GLAccountQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "GLAC", Name = "GLAccount" }, queryGroupRepository);
						QueryGroup GLAccountQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "VNAC", Name = "VendorGLAccount" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable GLAccountObjectTable = objectContext.ObjectTables.Where(d => d.Name == "GLAccount" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> GLAccountObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "GLAccount").ToList();   

			   TextCode GLAccountTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.GLAccounts", DefaultText = @"General Ledger Accounts",LocalDefaultText = "חשבונות", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature GLAccountFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GLACCOUNTS", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "GLAccount.Features.GLAccounts", NameTextCodeDefaultText = "General Ledger Accounts", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode GLAccountTextCode_1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.Vendors", DefaultText = @"Vendor Accounts",LocalDefaultText = "ספקים", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature GLAccountFeature_1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "VENDORGLACCOUNTS", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "GLAccount.Features.Vendors", NameTextCodeDefaultText = "Vendor Accounts", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode GLAccountTextCode_2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.Clients", DefaultText = @"All Customers",LocalDefaultText = "לקוחות", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature GLAccountFeature_2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CLIENTS", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "GLAccount.Features.Clients", NameTextCodeDefaultText = "All Customers Accounts", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode GLAccountTextCode_3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.Collectors", DefaultText = @"My Customers (As Collectors)",LocalDefaultText = "לקוחות שלי(כגובה)", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature GLAccountFeature_3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "collectorsGLA", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "GLAccount.Features.collectorsGLA", NameTextCodeDefaultText = "My Customers (As Colectors)", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode GLAccountTextCode_4 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.debetors", DefaultText = @"Debtors Customers",LocalDefaultText = "לקוחות חייבים", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature GLAccountFeature_4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "debetorsGLA", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "GLAccount.Features.debetorsGLA", NameTextCodeDefaultText = "Debetors Customers", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode GLAccountTextCode_5 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.ActiveCustomers", DefaultText = @"Active Customers",LocalDefaultText = "לקוחות פעילים", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature GLAccountFeature_5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "activeCustomersGLA", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "GLAccount.Features.activeCustomersGLA", NameTextCodeDefaultText = "Active Customers", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode GLAccountTextCode_6 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.InactiveCustomers", DefaultText = @"Inactive Customers",LocalDefaultText = "לקוחות חסומים", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature GLAccountFeature_6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "inactiveCustomersGla", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "GLAccount.Features.inactiveCustomersGla", NameTextCodeDefaultText = "Inactive Customers", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode GLAccountTextCode_7 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.ActiveGLAccounts", DefaultText = @"Active GL Account",LocalDefaultText = "כרטיסים פעילים", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature GLAccountFeature_7 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACTIVEGLACCOUNTS", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "GLAccount.Features.ACTIVEGLACCOUNTS", NameTextCodeDefaultText = "Active GL Account", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode GLAccountTextCode_8 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.InActiveGLAccounts", DefaultText = @"Inactive GL Account",LocalDefaultText = "כרטיסים לא פעילים", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature GLAccountFeature_8 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "INACTIVEGLACCOUNTS", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "GLAccount.Features.INACTIVEGLACCOUNTS", NameTextCodeDefaultText = "Inactive GL Account", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode GLAccountTextCode_9 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.OpenFiles", DefaultText = @"Open Files",LocalDefaultText = "תקים פתוחים", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature GLAccountFeature_9 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OPENFILESGLACCOUNTS", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "GLAccount.Features.OPENFILESGLACCOUNTS", NameTextCodeDefaultText = "Open Files", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode GLAccountTextCode_10 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.ClosedFiles", DefaultText = @"Closed Files",LocalDefaultText = "תיקים סגורים", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature GLAccountFeature_10 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CLOSEDFILESGLACCOUNTS", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "GLAccount.Features.CLOSEDFILESGLACCOUNTS", NameTextCodeDefaultText = "Closed Files", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode GLAccountTextCode_11 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.AllFiles", DefaultText = @"All Files",LocalDefaultText = "כל התיקים", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature GLAccountFeature_11 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ALLFILESGLACCOUNTS", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "GLAccount.Features.ALLFILESGLACCOUNTS", NameTextCodeDefaultText = "All Files", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode GLAccountTextCode_12 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.AllJobs", DefaultText = @"All Jobs",LocalDefaultText = "כל הג’ובים", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature GLAccountFeature_12 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ALLJOBSGLACCOUNTS", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "GLAccount.Features.ALLJOBSGLACCOUNTS", NameTextCodeDefaultText = "All Jobs", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode GLAccountTextCode_13 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.ActiveVendors", DefaultText = @"Active Vendors",LocalDefaultText = "ספקים פעילים", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature GLAccountFeature_13 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "activeVendorsGLA", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "GLAccount.Features.activeVendorsGLA", NameTextCodeDefaultText = "Active Vendors", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode GLAccountTextCode_14 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.InactiveVendors", DefaultText = @"Inactive Vendors",LocalDefaultText = "ספקים חסומים", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature GLAccountFeature_14 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "inactiveVendorsGla", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "GLAccount.Features.inactiveVendorsGla", NameTextCodeDefaultText = "Inactive Vendors", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode GLAccountTextCode_15 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccount.Q.ClientAccounts", DefaultText = @"All Customers",LocalDefaultText = "לקוחות", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature GLAccountFeature_15 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CLIENTGLACCOUNTS", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "GLAccount.Features.Clients1", NameTextCodeDefaultText = "All Customers Accounts", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode GLAccountTextCode_16 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccounts.Q.AllGLAccounts", DefaultText = @"All GL Accounts",LocalDefaultText = "כל הכרטיסים", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature GLAccountFeature_16 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ALLGLACCOUNTS", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "GLAccount.Features.AllGLAccounts", NameTextCodeDefaultText = "All GL Accounts", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode GLAccountTextCode_17 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccount.Q.OpenMasters", DefaultText = @"Open Masters",LocalDefaultText = "גו’בים פתוחים", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature GLAccountFeature_17 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GLAccount.Q.OpenMasters", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "GLAccountFeatures.OpenMasters", NameTextCodeDefaultText = "OpenMasters", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query GeneralLedgerAccountsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = GLAccountTextCode_0.Id, Code = "General Ledger Accounts",  QueryGroupCode = "GLAC", IndexOrder = 0, Tenant = 0, ObjectTableId = GLAccountObjectTable.Id, QuerySection = "GLAccount", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = GLAccountFeature_0.Id, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn GeneralLedgerAccountsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = GeneralLedgerAccountsQuery.Id, IndexOrder = 0, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 73 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn GeneralLedgerAccountsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = GeneralLedgerAccountsQuery.Id, IndexOrder = 1, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn GeneralLedgerAccountsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = GeneralLedgerAccountsQuery.Id, IndexOrder = 2, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn GeneralLedgerAccountsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = GeneralLedgerAccountsQuery.Id, IndexOrder = 3, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 56 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn GeneralLedgerAccountsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = GeneralLedgerAccountsQuery.Id, IndexOrder = 4, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "RevenueExpenseName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "RevenueExpenseName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 90 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn GeneralLedgerAccountsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = GeneralLedgerAccountsQuery.Id, IndexOrder = 5, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 140 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn GeneralLedgerAccountsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = GeneralLedgerAccountsQuery.Id, IndexOrder = 6, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 56 }, queryColumnsRepository, tenantQueryColumns);
  
	      

			  Query VendorAccountsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = GLAccountTextCode_1.Id, Code = "Vendor Accounts",  QueryGroupCode = "VNAC", IndexOrder = 1, Tenant = 0, ObjectTableId = GLAccountObjectTable.Id, QuerySection = "GLAccount", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = GLAccountFeature_1.Id, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn VendorAccountsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = VendorAccountsQuery.Id, IndexOrder = 0, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn VendorAccountsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = VendorAccountsQuery.Id, IndexOrder = 1, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 220 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn VendorAccountsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = VendorAccountsQuery.Id, IndexOrder = 2, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 220 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn VendorAccountsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = VendorAccountsQuery.Id, IndexOrder = 3, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 56 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn VendorAccountsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = VendorAccountsQuery.Id, IndexOrder = 4, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "VatNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "VatNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 90 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn VendorAccountsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = VendorAccountsQuery.Id, IndexOrder = 5, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "RevenueExpenseName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "RevenueExpenseName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 90 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn VendorAccountsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = VendorAccountsQuery.Id, IndexOrder = 6, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn VendorAccountsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = VendorAccountsQuery.Id, IndexOrder = 7, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 170 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn VendorAccountsQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = VendorAccountsQuery.Id, IndexOrder = 8, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 60 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter VendorAccountsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "3",PredefinedValue2 = null, QueryId = VendorAccountsQuery.Id, Tenant = 0,Operator = "Equals"}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query AllCustomersQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = GLAccountTextCode_2.Id, Code = "All Customers",  QueryGroupCode = "GLAC", IndexOrder = 2, Tenant = 0, ObjectTableId = GLAccountObjectTable.Id, QuerySection = "GLAccount", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = GLAccountFeature_2.Id, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn AllCustomersQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCustomersQuery.Id, IndexOrder = 0, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 73 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllCustomersQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCustomersQuery.Id, IndexOrder = 1, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllCustomersQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCustomersQuery.Id, IndexOrder = 2, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllCustomersQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCustomersQuery.Id, IndexOrder = 3, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 56 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllCustomersQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCustomersQuery.Id, IndexOrder = 4, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "RevenueExpenseName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "RevenueExpenseName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 90 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllCustomersQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCustomersQuery.Id, IndexOrder = 5, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 140 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllCustomersQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCustomersQuery.Id, IndexOrder = 6, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 56 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter AllCustomersQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "2",PredefinedValue2 = null, QueryId = AllCustomersQuery.Id, Tenant = 0,Operator = "Equals"}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query MyCustomersAsCollectorsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = GLAccountTextCode_3.Id, Code = "MyCustomersAsCollectors",  QueryGroupCode = "GLAC", IndexOrder = 3, Tenant = 0, ObjectTableId = GLAccountObjectTable.Id, QuerySection = "GLAccount", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = GLAccountFeature_3.Id, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn MyCustomersAsCollectorsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyCustomersAsCollectorsQuery.Id, IndexOrder = 0, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyCustomersAsCollectorsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyCustomersAsCollectorsQuery.Id, IndexOrder = 1, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 220 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyCustomersAsCollectorsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyCustomersAsCollectorsQuery.Id, IndexOrder = 2, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 220 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyCustomersAsCollectorsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyCustomersAsCollectorsQuery.Id, IndexOrder = 3, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 90 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyCustomersAsCollectorsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyCustomersAsCollectorsQuery.Id, IndexOrder = 4, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "IsMultiCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "IsMultiCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyCustomersAsCollectorsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyCustomersAsCollectorsQuery.Id, IndexOrder = 5, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyCustomersAsCollectorsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyCustomersAsCollectorsQuery.Id, IndexOrder = 6, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 170 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn MyCustomersAsCollectorsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = MyCustomersAsCollectorsQuery.Id, IndexOrder = 7, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 56 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter MyCustomersAsCollectorsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "2",PredefinedValue2 = null, QueryId = MyCustomersAsCollectorsQuery.Id, Tenant = 0,Operator = "Equals"}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query DebetorsCustomersQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = GLAccountTextCode_4.Id, Code = "DebetorsCustomers",  QueryGroupCode = "GLAC", IndexOrder = 4, Tenant = 0, ObjectTableId = GLAccountObjectTable.Id, QuerySection = "GLAccount", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = GLAccountFeature_4.Id, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn DebetorsCustomersQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DebetorsCustomersQuery.Id, IndexOrder = 0, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DebetorsCustomersQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DebetorsCustomersQuery.Id, IndexOrder = 1, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 220 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DebetorsCustomersQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DebetorsCustomersQuery.Id, IndexOrder = 2, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 220 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DebetorsCustomersQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DebetorsCustomersQuery.Id, IndexOrder = 3, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 90 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DebetorsCustomersQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DebetorsCustomersQuery.Id, IndexOrder = 4, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "IsMultiCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "IsMultiCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DebetorsCustomersQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DebetorsCustomersQuery.Id, IndexOrder = 5, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DebetorsCustomersQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DebetorsCustomersQuery.Id, IndexOrder = 6, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 170 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DebetorsCustomersQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DebetorsCustomersQuery.Id, IndexOrder = 7, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 56 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter DebetorsCustomersQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "2",PredefinedValue2 = null, QueryId = DebetorsCustomersQuery.Id, Tenant = 0,Operator = "Equals"}, advancedQueryFiltersRepository, tenantAdvancedFilters);


             AdvancedQueryFilter DebetorsCustomersQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "LocalBalanceInDue" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "LocalBalanceInDue" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "0",PredefinedValue2 = null, QueryId = DebetorsCustomersQuery.Id, Tenant = 0,Operator = "LargerThan"}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query ActiveCustomersGLAccountsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = GLAccountTextCode_5.Id, Code = "ActiveCustomersGLAccounts",  QueryGroupCode = "GLAC", IndexOrder = 5, Tenant = 0, ObjectTableId = GLAccountObjectTable.Id, QuerySection = "GLAccount", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = GLAccountFeature_5.Id, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn ActiveCustomersGLAccountsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ActiveCustomersGLAccountsQuery.Id, IndexOrder = 0, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ActiveCustomersGLAccountsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ActiveCustomersGLAccountsQuery.Id, IndexOrder = 1, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 220 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ActiveCustomersGLAccountsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ActiveCustomersGLAccountsQuery.Id, IndexOrder = 2, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 220 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ActiveCustomersGLAccountsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ActiveCustomersGLAccountsQuery.Id, IndexOrder = 3, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 90 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ActiveCustomersGLAccountsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ActiveCustomersGLAccountsQuery.Id, IndexOrder = 4, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "IsMultiCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "IsMultiCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ActiveCustomersGLAccountsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ActiveCustomersGLAccountsQuery.Id, IndexOrder = 5, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ActiveCustomersGLAccountsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ActiveCustomersGLAccountsQuery.Id, IndexOrder = 6, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 170 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter ActiveCustomersGLAccountsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "2",PredefinedValue2 = null, QueryId = ActiveCustomersGLAccountsQuery.Id, Tenant = 0,Operator = "Equals"}, advancedQueryFiltersRepository, tenantAdvancedFilters);


             AdvancedQueryFilter ActiveCustomersGLAccountsQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "false",PredefinedValue2 = null, QueryId = ActiveCustomersGLAccountsQuery.Id, Tenant = 0,Operator = "Equals"}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query InactiveCustomersGLAccountQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = GLAccountTextCode_6.Id, Code = "InactiveCustomersGLAccount",  QueryGroupCode = "GLAC", IndexOrder = 6, Tenant = 0, ObjectTableId = GLAccountObjectTable.Id, QuerySection = "GLAccount", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = GLAccountFeature_6.Id, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn InactiveCustomersGLAccountQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InactiveCustomersGLAccountQuery.Id, IndexOrder = 0, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InactiveCustomersGLAccountQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InactiveCustomersGLAccountQuery.Id, IndexOrder = 1, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 220 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InactiveCustomersGLAccountQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InactiveCustomersGLAccountQuery.Id, IndexOrder = 2, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 220 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InactiveCustomersGLAccountQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InactiveCustomersGLAccountQuery.Id, IndexOrder = 3, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 90 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InactiveCustomersGLAccountQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InactiveCustomersGLAccountQuery.Id, IndexOrder = 4, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "IsMultiCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "IsMultiCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InactiveCustomersGLAccountQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InactiveCustomersGLAccountQuery.Id, IndexOrder = 5, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InactiveCustomersGLAccountQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InactiveCustomersGLAccountQuery.Id, IndexOrder = 6, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 170 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter InactiveCustomersGLAccountQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "2",PredefinedValue2 = null, QueryId = InactiveCustomersGLAccountQuery.Id, Tenant = 0,Operator = "Equals"}, advancedQueryFiltersRepository, tenantAdvancedFilters);


             AdvancedQueryFilter InactiveCustomersGLAccountQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "true",PredefinedValue2 = null, QueryId = InactiveCustomersGLAccountQuery.Id, Tenant = 0,Operator = "Equals"}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query ActiveGLAccountsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = GLAccountTextCode_7.Id, Code = "ActiveGLAccounts",  QueryGroupCode = "GLAC", IndexOrder = 7, Tenant = 0, ObjectTableId = GLAccountObjectTable.Id, QuerySection = "GLAccount", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = GLAccountFeature_7.Id, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn ActiveGLAccountsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ActiveGLAccountsQuery.Id, IndexOrder = 0, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 85 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ActiveGLAccountsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ActiveGLAccountsQuery.Id, IndexOrder = 1, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ActiveGLAccountsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ActiveGLAccountsQuery.Id, IndexOrder = 2, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ActiveGLAccountsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ActiveGLAccountsQuery.Id, IndexOrder = 3, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 80 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ActiveGLAccountsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ActiveGLAccountsQuery.Id, IndexOrder = 4, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "IsMultiCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "IsMultiCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 110 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ActiveGLAccountsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ActiveGLAccountsQuery.Id, IndexOrder = 5, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "BalanceInLocalCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "BalanceInLocalCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ActiveGLAccountsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ActiveGLAccountsQuery.Id, IndexOrder = 6, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 140 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ActiveGLAccountsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ActiveGLAccountsQuery.Id, IndexOrder = 7, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ActiveGLAccountsQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ActiveGLAccountsQuery.Id, IndexOrder = 8, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 75 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter ActiveGLAccountsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "1",PredefinedValue2 = null, QueryId = ActiveGLAccountsQuery.Id, Tenant = 0,Operator = "Equals"}, advancedQueryFiltersRepository, tenantAdvancedFilters);


             AdvancedQueryFilter ActiveGLAccountsQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "false",PredefinedValue2 = null, QueryId = ActiveGLAccountsQuery.Id, Tenant = 0,Operator = "Equals"}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query InactiveGLAccountsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = GLAccountTextCode_8.Id, Code = "InactiveGLAccounts",  QueryGroupCode = "GLAC", IndexOrder = 8, Tenant = 0, ObjectTableId = GLAccountObjectTable.Id, QuerySection = "GLAccount", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = GLAccountFeature_8.Id, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn InactiveGLAccountsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InactiveGLAccountsQuery.Id, IndexOrder = 0, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 85 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InactiveGLAccountsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InactiveGLAccountsQuery.Id, IndexOrder = 1, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InactiveGLAccountsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InactiveGLAccountsQuery.Id, IndexOrder = 2, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InactiveGLAccountsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InactiveGLAccountsQuery.Id, IndexOrder = 3, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 80 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InactiveGLAccountsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InactiveGLAccountsQuery.Id, IndexOrder = 4, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "IsMultiCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "IsMultiCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 110 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InactiveGLAccountsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InactiveGLAccountsQuery.Id, IndexOrder = 5, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "BalanceInLocalCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "BalanceInLocalCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InactiveGLAccountsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InactiveGLAccountsQuery.Id, IndexOrder = 6, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 140 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InactiveGLAccountsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InactiveGLAccountsQuery.Id, IndexOrder = 7, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InactiveGLAccountsQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InactiveGLAccountsQuery.Id, IndexOrder = 8, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 75 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter InactiveGLAccountsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "1",PredefinedValue2 = null, QueryId = InactiveGLAccountsQuery.Id, Tenant = 0,Operator = "Equals"}, advancedQueryFiltersRepository, tenantAdvancedFilters);


             AdvancedQueryFilter InactiveGLAccountsQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "true",PredefinedValue2 = null, QueryId = InactiveGLAccountsQuery.Id, Tenant = 0,Operator = "Equals"}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query OpenFilesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = GLAccountTextCode_9.Id, Code = "OpenFiles",  QueryGroupCode = "GLAC", IndexOrder = 9, Tenant = 0, ObjectTableId = GLAccountObjectTable.Id, QuerySection = "GLAccount", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = GLAccountFeature_9.Id, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn OpenFilesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenFilesQuery.Id, IndexOrder = 0, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 85 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenFilesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenFilesQuery.Id, IndexOrder = 1, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenFilesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenFilesQuery.Id, IndexOrder = 2, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenFilesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenFilesQuery.Id, IndexOrder = 3, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 80 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenFilesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenFilesQuery.Id, IndexOrder = 4, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "IsMultiCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "IsMultiCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 110 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenFilesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenFilesQuery.Id, IndexOrder = 5, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "BalanceInLocalCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "BalanceInLocalCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenFilesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenFilesQuery.Id, IndexOrder = 6, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 140 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenFilesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenFilesQuery.Id, IndexOrder = 7, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenFilesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenFilesQuery.Id, IndexOrder = 8, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 75 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter OpenFilesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "5",PredefinedValue2 = null, QueryId = OpenFilesQuery.Id, Tenant = 0,Operator = "Equals"}, advancedQueryFiltersRepository, tenantAdvancedFilters);


             AdvancedQueryFilter OpenFilesQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "BalanceInLocalCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "BalanceInLocalCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "0",PredefinedValue2 = null, QueryId = OpenFilesQuery.Id, Tenant = 0,Operator = "NotEqual"}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query ClosedFilesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = GLAccountTextCode_10.Id, Code = "ClosedFiles",  QueryGroupCode = "GLAC", IndexOrder = 10, Tenant = 0, ObjectTableId = GLAccountObjectTable.Id, QuerySection = "GLAccount", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = GLAccountFeature_10.Id, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn ClosedFilesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedFilesQuery.Id, IndexOrder = 0, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 85 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ClosedFilesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedFilesQuery.Id, IndexOrder = 1, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ClosedFilesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedFilesQuery.Id, IndexOrder = 2, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ClosedFilesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedFilesQuery.Id, IndexOrder = 3, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 80 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ClosedFilesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedFilesQuery.Id, IndexOrder = 4, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "IsMultiCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "IsMultiCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 110 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ClosedFilesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedFilesQuery.Id, IndexOrder = 5, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "BalanceInLocalCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "BalanceInLocalCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ClosedFilesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedFilesQuery.Id, IndexOrder = 6, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 140 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ClosedFilesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedFilesQuery.Id, IndexOrder = 7, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ClosedFilesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedFilesQuery.Id, IndexOrder = 8, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 75 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter ClosedFilesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "5",PredefinedValue2 = null, QueryId = ClosedFilesQuery.Id, Tenant = 0,Operator = "Equals"}, advancedQueryFiltersRepository, tenantAdvancedFilters);


             AdvancedQueryFilter ClosedFilesQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "BalanceInLocalCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "BalanceInLocalCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "0",PredefinedValue2 = null, QueryId = ClosedFilesQuery.Id, Tenant = 0,Operator = "Equals"}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query AllFilesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = GLAccountTextCode_11.Id, Code = "AllFiles",  QueryGroupCode = "GLAC", IndexOrder = 11, Tenant = 0, ObjectTableId = GLAccountObjectTable.Id, QuerySection = "GLAccount", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = GLAccountFeature_11.Id, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn AllFilesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllFilesQuery.Id, IndexOrder = 0, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 85 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllFilesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllFilesQuery.Id, IndexOrder = 1, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllFilesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllFilesQuery.Id, IndexOrder = 2, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllFilesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllFilesQuery.Id, IndexOrder = 3, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 80 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllFilesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllFilesQuery.Id, IndexOrder = 4, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "IsMultiCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "IsMultiCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 110 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllFilesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllFilesQuery.Id, IndexOrder = 5, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "BalanceInLocalCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "BalanceInLocalCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllFilesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllFilesQuery.Id, IndexOrder = 6, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 140 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllFilesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllFilesQuery.Id, IndexOrder = 7, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllFilesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllFilesQuery.Id, IndexOrder = 8, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 75 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter AllFilesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "5",PredefinedValue2 = null, QueryId = AllFilesQuery.Id, Tenant = 0,Operator = "Equals"}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query AllJobsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = GLAccountTextCode_12.Id, Code = "AllJobs",  QueryGroupCode = "GLAC", IndexOrder = 12, Tenant = 0, ObjectTableId = GLAccountObjectTable.Id, QuerySection = "GLAccount", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = GLAccountFeature_12.Id, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn AllJobsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllJobsQuery.Id, IndexOrder = 0, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 85 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllJobsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllJobsQuery.Id, IndexOrder = 1, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllJobsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllJobsQuery.Id, IndexOrder = 2, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllJobsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllJobsQuery.Id, IndexOrder = 3, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 80 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllJobsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllJobsQuery.Id, IndexOrder = 4, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "IsMultiCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "IsMultiCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 110 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllJobsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllJobsQuery.Id, IndexOrder = 5, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "BalanceInLocalCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "BalanceInLocalCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllJobsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllJobsQuery.Id, IndexOrder = 6, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 140 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllJobsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllJobsQuery.Id, IndexOrder = 7, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllJobsQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllJobsQuery.Id, IndexOrder = 8, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 75 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter AllJobsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "4",PredefinedValue2 = null, QueryId = AllJobsQuery.Id, Tenant = 0,Operator = "Equals"}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query ActiveVendorsGLAccountsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = GLAccountTextCode_13.Id, Code = "ActiveVendorsGLAccounts",  QueryGroupCode = "GLAC", IndexOrder = 13, Tenant = 0, ObjectTableId = GLAccountObjectTable.Id, QuerySection = "GLAccount", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = GLAccountFeature_13.Id, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn ActiveVendorsGLAccountsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ActiveVendorsGLAccountsQuery.Id, IndexOrder = 0, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ActiveVendorsGLAccountsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ActiveVendorsGLAccountsQuery.Id, IndexOrder = 1, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 220 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ActiveVendorsGLAccountsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ActiveVendorsGLAccountsQuery.Id, IndexOrder = 2, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 220 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ActiveVendorsGLAccountsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ActiveVendorsGLAccountsQuery.Id, IndexOrder = 3, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "VatNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "VatNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 90 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ActiveVendorsGLAccountsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ActiveVendorsGLAccountsQuery.Id, IndexOrder = 4, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ActiveVendorsGLAccountsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ActiveVendorsGLAccountsQuery.Id, IndexOrder = 5, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 170 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ActiveVendorsGLAccountsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ActiveVendorsGLAccountsQuery.Id, IndexOrder = 6, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 60 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter ActiveVendorsGLAccountsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "3",PredefinedValue2 = null, QueryId = ActiveVendorsGLAccountsQuery.Id, Tenant = 0,Operator = "Equals"}, advancedQueryFiltersRepository, tenantAdvancedFilters);


             AdvancedQueryFilter ActiveVendorsGLAccountsQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "false",PredefinedValue2 = null, QueryId = ActiveVendorsGLAccountsQuery.Id, Tenant = 0,Operator = "Equals"}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query InactiveVendorsGLAccountQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = GLAccountTextCode_14.Id, Code = "InactiveVendorsGLAccount",  QueryGroupCode = "GLAC", IndexOrder = 14, Tenant = 0, ObjectTableId = GLAccountObjectTable.Id, QuerySection = "GLAccount", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = GLAccountFeature_14.Id, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn InactiveVendorsGLAccountQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InactiveVendorsGLAccountQuery.Id, IndexOrder = 0, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InactiveVendorsGLAccountQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InactiveVendorsGLAccountQuery.Id, IndexOrder = 1, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 220 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InactiveVendorsGLAccountQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InactiveVendorsGLAccountQuery.Id, IndexOrder = 2, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 220 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InactiveVendorsGLAccountQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InactiveVendorsGLAccountQuery.Id, IndexOrder = 3, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "VatNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "VatNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 90 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InactiveVendorsGLAccountQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InactiveVendorsGLAccountQuery.Id, IndexOrder = 4, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InactiveVendorsGLAccountQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InactiveVendorsGLAccountQuery.Id, IndexOrder = 5, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 170 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn InactiveVendorsGLAccountQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InactiveVendorsGLAccountQuery.Id, IndexOrder = 6, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 60 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter InactiveVendorsGLAccountQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "3",PredefinedValue2 = null, QueryId = InactiveVendorsGLAccountQuery.Id, Tenant = 0,Operator = "Equals"}, advancedQueryFiltersRepository, tenantAdvancedFilters);


             AdvancedQueryFilter InactiveVendorsGLAccountQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "true",PredefinedValue2 = null, QueryId = InactiveVendorsGLAccountQuery.Id, Tenant = 0,Operator = "Equals"}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query ClientAccountsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = GLAccountTextCode_15.Id, Code = "Client Accounts",  QueryGroupCode = "CLAC", IndexOrder = 15, Tenant = 0, ObjectTableId = GLAccountObjectTable.Id, QuerySection = "GLAccount", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = GLAccountFeature_15.Id, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn ClientAccountsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClientAccountsQuery.Id, IndexOrder = 0, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 73 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ClientAccountsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClientAccountsQuery.Id, IndexOrder = 1, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ClientAccountsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClientAccountsQuery.Id, IndexOrder = 2, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ClientAccountsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClientAccountsQuery.Id, IndexOrder = 3, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 56 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ClientAccountsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClientAccountsQuery.Id, IndexOrder = 4, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "RevenueExpenseName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "RevenueExpenseName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 90 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ClientAccountsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClientAccountsQuery.Id, IndexOrder = 5, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 140 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ClientAccountsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClientAccountsQuery.Id, IndexOrder = 6, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 56 }, queryColumnsRepository, tenantQueryColumns);
  
	      

			  Query AllGLAccountsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = GLAccountTextCode_16.Id, Code = "All GLAccounts",  QueryGroupCode = "GLAC", IndexOrder = 16, Tenant = 0, ObjectTableId = GLAccountObjectTable.Id, QuerySection = "GLAccount", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = GLAccountFeature_16.Id, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn AllGLAccountsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGLAccountsQuery.Id, IndexOrder = 0, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 85 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGLAccountsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGLAccountsQuery.Id, IndexOrder = 1, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGLAccountsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGLAccountsQuery.Id, IndexOrder = 2, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGLAccountsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGLAccountsQuery.Id, IndexOrder = 3, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 80 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGLAccountsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGLAccountsQuery.Id, IndexOrder = 4, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "IsMultiCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "IsMultiCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 110 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGLAccountsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGLAccountsQuery.Id, IndexOrder = 5, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "BalanceInLocalCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "BalanceInLocalCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGLAccountsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGLAccountsQuery.Id, IndexOrder = 6, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 140 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGLAccountsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGLAccountsQuery.Id, IndexOrder = 7, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllGLAccountsQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllGLAccountsQuery.Id, IndexOrder = 8, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 75 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter AllGLAccountsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "1",PredefinedValue2 = null, QueryId = AllGLAccountsQuery.Id, Tenant = 0,Operator = "Equals"}, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query OpenMastersQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = GLAccountTextCode_17.Id, Code = "OpenMasters",  QueryGroupCode = "GLAC", IndexOrder = 17, Tenant = 0, ObjectTableId = GLAccountObjectTable.Id, QuerySection = "GLAccount", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = GLAccountFeature_17.Id, DefaultSortName = "CreateDate", DefaultSortDirection = "Desending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn OpenMastersQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenMastersQuery.Id, IndexOrder = 0, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 85 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenMastersQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenMastersQuery.Id, IndexOrder = 1, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenMastersQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenMastersQuery.Id, IndexOrder = 2, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenMastersQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenMastersQuery.Id, IndexOrder = 3, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "CurrencyCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 80 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenMastersQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenMastersQuery.Id, IndexOrder = 4, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "IsMultiCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "IsMultiCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 110 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenMastersQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenMastersQuery.Id, IndexOrder = 5, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "BalanceInLocalCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "BalanceInLocalCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 180 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenMastersQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenMastersQuery.Id, IndexOrder = 6, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 140 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenMastersQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenMastersQuery.Id, IndexOrder = 7, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "ChartOfAccountsTypeName" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 160 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn OpenMastersQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenMastersQuery.Id, IndexOrder = 8, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 75 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter OpenMastersQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "AccountTypeCode" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "4",PredefinedValue2 = null, QueryId = OpenMastersQuery.Id, Tenant = 0,Operator = "Equal"}, advancedQueryFiltersRepository, tenantAdvancedFilters);


             AdvancedQueryFilter OpenMastersQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "BalanceInLocalCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "BalanceInLocalCurrency" && d.ObjectTableId == GLAccountObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "0",PredefinedValue2 = null, QueryId = OpenMastersQuery.Id, Tenant = 0,Operator = "NotEqual"}, advancedQueryFiltersRepository, tenantAdvancedFilters);

	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable GLAccountObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "GLAccount" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> GLAccountObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "GLAccount").ToList();
		       
	      

	         Screen GLAccountHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "GLAccount.HeaderScreen", Name = "Header Screen", ObjectTableId = GLAccountObjectTable.Id, NumberOfColumns = 5, NumberOfRows = 2, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField GLAccountGLAccountHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber").FirstOrDefault().Id, ScreenId = GLAccountHeaderScreenScreen0.Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "DisplayNumber").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField GLAccountGLAccountHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "BalanceInLocalCurrency").FirstOrDefault().Id, ScreenId = GLAccountHeaderScreenScreen0.Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "BalanceInLocalCurrency").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField GLAccountGLAccountHeaderScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "LocalName").FirstOrDefault().Id, ScreenId = GLAccountHeaderScreenScreen0.Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "LocalName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField GLAccountGLAccountHeaderScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "LocalBalanceInDue").FirstOrDefault().Id, ScreenId = GLAccountHeaderScreenScreen0.Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "LocalBalanceInDue").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField GLAccountGLAccountHeaderScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "CurrencyCode").FirstOrDefault().Id, ScreenId = GLAccountHeaderScreenScreen0.Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "CurrencyCode").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField GLAccountGLAccountHeaderScreenScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 1, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "NextDueDate").FirstOrDefault().Id, ScreenId = GLAccountHeaderScreenScreen0.Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "NextDueDate").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField GLAccountGLAccountHeaderScreenScreenField6 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "ReconcileMethodName").FirstOrDefault().Id, ScreenId = GLAccountHeaderScreenScreen0.Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "ReconcileMethodName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField GLAccountGLAccountHeaderScreenScreenField7 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 4, Row = 0, ObjectFieldId = GLAccountObjectFields.Where(d => d.FieldName == "InternalNumber").FirstOrDefault().Id, ScreenId = GLAccountHeaderScreenScreen0.Id, ObjectFieldCode = GLAccountObjectFields.Where(d => d.FieldName == "InternalNumber").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    GLAccountObjectTable.HeaderScreenId = GLAccountHeaderScreenScreen0.Id;
	   		  

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable GLAccountObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "GLAccount" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode GLAccountOverviewTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccount.TH.Overview", DefaultText = "Overview",LocalDefaultText = "מבט על", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature GLAccountOverviewFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GLAccount.Tab.Overview", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "GLAccountFeatures.GAOV", NameTextCodeDefaultText = "Overview", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode GLAccountInterestTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccount.TH.Interest", DefaultText = "Interest",LocalDefaultText = "ריבית", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature GLAccountInterestFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GLAccount.Tab.Interest", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "GLAccountFeatures.GAIT", NameTextCodeDefaultText = "Interest", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode GLAccountTransactionsTextCode_TH2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccount.TH.Transactions", DefaultText = "Transactions",LocalDefaultText = "תנועות", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature GLAccountTransactionsFeature_TH2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TRANSACTIONS", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "GLAccount.Features.Transactions", NameTextCodeDefaultText = "Transactions", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode GLAccountGeneralTextCode_TH3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccount.TH.General", DefaultText = "General",LocalDefaultText = "כללי", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature GLAccountGeneralFeature_TH3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GENERAL", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "GLAccount.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode GLAccountManageReconcTextCode_TH4 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccount.TH.ManageReconciliations", DefaultText = "Manage Reconc.",LocalDefaultText = "ניהול התאמות", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature GLAccountManageReconcFeature_TH4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MANAGERECONCILIATIONS", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "GLAccount.Features.ManageReconciliations", NameTextCodeDefaultText = "Manage Reconc.", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode GLAccountExternalTransactionsTextCode_TH5 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccount.TH.ExternalTransactions", DefaultText = "External Transactions",LocalDefaultText = "תנועות חיצוניות", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature GLAccountExternalTransactionsFeature_TH5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GLAccount.Tab.ExternalTransactions", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "GLAccountFeatures.GLET", NameTextCodeDefaultText = "External Transactions", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode GLAccountManageExternalRecoTextCode_TH6 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccount.TH.ManageExternalReco", DefaultText = "Manage External Reco",LocalDefaultText = "ניהול התאמות חיצוניות", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature GLAccountManageExternalRecoFeature_TH6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GLAccount.Tab.ManageExternalReco", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "GLAccountFeatures.GMER", NameTextCodeDefaultText = "Manage External Reco", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode GLAccountTaxwithholdingTextCode_TH7 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccount.TH.Tax", DefaultText = "Tax withholding",LocalDefaultText = "ניכוי מס במקור", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature GLAccountTaxwithholdingFeature_TH7 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TAX", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "GLAccount.Features.Tax", NameTextCodeDefaultText = "Tax Withholding", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode GLAccountAdditionalDataTextCode_TH8 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccount.TH.Additional", DefaultText = "Additional Data",LocalDefaultText = "נתונים נוספים", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature GLAccountAdditionalDataFeature_TH8 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ADDITIONAL", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "GLAccount.Features.Additional", NameTextCodeDefaultText = "Additional Data", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode GLAccountEventsTextCode_TH9 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccount.TH.Events", DefaultText = "Events",LocalDefaultText = "אירועים", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature GLAccountEventsFeature_TH9 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EVENTS", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "GLAccount.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "GAOV",HtmlComponentName = "GLAccountOverviewComponent",HtmlComponentUrl = "./Accounting/Components/EditTabs/GLAccount/GLAccountOverviewComponent", FeatureId = GLAccountOverviewFeature_TH0.Id, ControlPath = "", ObjectTableId = GLAccountObjectTable.Id, TabNameTextCodeId = GLAccountOverviewTextCode_TH0.Id, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "GAIT",HtmlComponentName = "GLAccountInterestComponent",HtmlComponentUrl = "./Accounting/Components/EditTabs/GLAccount/GLAccountInterestComponent", FeatureId = GLAccountInterestFeature_TH1.Id, ControlPath = "", ObjectTableId = GLAccountObjectTable.Id, TabNameTextCodeId = GLAccountInterestTextCode_TH1.Id, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "GATR",HtmlComponentName = "GLAccountTransactionsTabComponent",HtmlComponentUrl = "./Accounting/Components/EditTabs/GLAccount/GLAccountTransactionsTabComponent", FeatureId = GLAccountTransactionsFeature_TH2.Id, ControlPath = "Logitude.Accounting.Views.Tabs.ACC.GLAccountTransactionsTabControl", ObjectTableId = GLAccountObjectTable.Id, TabNameTextCodeId = GLAccountTransactionsTextCode_TH2.Id, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "GAGC",HtmlComponentName = "GLAccountGeneralTabComponent",HtmlComponentUrl = "./Accounting/Components/EditTabs/GLAccount/GLAccountGeneralTabComponent", FeatureId = GLAccountGeneralFeature_TH3.Id, ControlPath = "Logitude.Accounting.Views.Tabs.ACC.GLAccountGeneralTabControl", ObjectTableId = GLAccountObjectTable.Id, TabNameTextCodeId = GLAccountGeneralTextCode_TH3.Id, Tenant = 0, IndexOrder = 2 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "GAMR",HtmlComponentName = "ManageReconciliationsTabComponent",HtmlComponentUrl = "./Accounting/Components/EditTabs/GLAccount/ManageReconciliationsTabComponent", FeatureId = GLAccountManageReconcFeature_TH4.Id, ControlPath = "Logitude.Accounting.Views.Tabs.RECO.GLAccountManageReconciliationsTabControl", ObjectTableId = GLAccountObjectTable.Id, TabNameTextCodeId = GLAccountManageReconcTextCode_TH4.Id, Tenant = 0, IndexOrder = 3 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "GLET",HtmlComponentName = "ExternalPagesTabComponent",HtmlComponentUrl = "./Accounting/Components/Others/ReconcileExternalPage/ExternalPagesTabComponent", FeatureId = GLAccountExternalTransactionsFeature_TH5.Id, ControlPath = "./Accounting/Components/Others/ReconcileExternalPage/ExternalPagesTabComponent", ObjectTableId = GLAccountObjectTable.Id, TabNameTextCodeId = GLAccountExternalTransactionsTextCode_TH5.Id, Tenant = 0, IndexOrder = 4 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "GMER",HtmlComponentName = "ManageExternalReconciliationTabComponent",HtmlComponentUrl = "./Accounting/Components/Others/ReconcileExternalPage/ManageExternalReconciliationTabComponent", FeatureId = GLAccountManageExternalRecoFeature_TH6.Id, ControlPath = "./Accounting/Components/Others/ReconcileExternalPage/ManageExternalReconciliationTabComponent", ObjectTableId = GLAccountObjectTable.Id, TabNameTextCodeId = GLAccountManageExternalRecoTextCode_TH6.Id, Tenant = 0, IndexOrder = 5 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "GLTX",HtmlComponentName = "GLAccountTaxWithholdingTabComponent",HtmlComponentUrl = "./Accounting/Components/EditTabs/GLAccount/GLAccountTaxWithholdingTabComponent", FeatureId = GLAccountTaxwithholdingFeature_TH7.Id, ControlPath = "", ObjectTableId = GLAccountObjectTable.Id, TabNameTextCodeId = GLAccountTaxwithholdingTextCode_TH7.Id, Tenant = 0, IndexOrder = 6 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "GAAD",HtmlComponentName = "GLAccountAdditionalDataTabComponent",HtmlComponentUrl = "./Accounting/Components/EditTabs/GLAccount/GLAccountAdditionalDataTabComponent", FeatureId = GLAccountAdditionalDataFeature_TH8.Id, ControlPath = "", ObjectTableId = GLAccountObjectTable.Id, TabNameTextCodeId = GLAccountAdditionalDataTextCode_TH8.Id, Tenant = 0, IndexOrder = 7 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "GAEV",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = GLAccountEventsFeature_TH9.Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = GLAccountObjectTable.Id, TabNameTextCodeId = GLAccountEventsTextCode_TH9.Id, Tenant = 0, IndexOrder = 8 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable GLAccountObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "GLAccount" && d.Tenant == 0).FirstOrDefault(); 

		   Feature GLAccountFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = true, ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "GLAccount.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature GLAccountFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = true, ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "GLAccount.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature GLAccountFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = true, ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "GLAccount.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature GLAccountFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = true, ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "GLAccount.Features.PackageFeature", NameTextCodeDefaultText = "GLAccount Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);    
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable GLAccountObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "GLAccount" && d.Tenant == 0).FirstOrDefault(); 
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
                ObjectTableId = GLAccountObjectTable.Id,
				 
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
                ObjectTableId = GLAccountObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "GLPC",
                EnglishName =  "Parent GLAccount updated",
                LocalName =  "כרטיס אב עודכן",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = GLAccountObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CTCH",
                EnglishName =  "Chart Of Account Type Changed",
                LocalName =  "סוג קבוצת מאזן עודכן",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = GLAccountObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "ADD",
                EnglishName =  "Splitted GLAccount added",
                LocalName =  "נוסף כרטיס פיצול לפני מטבע",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = GLAccountObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "INGL",
                EnglishName =  "Splitted GLAccount deactivated",
                LocalName =  "כרטיס הפיצול נחסם",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = GLAccountObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "DISC",
                EnglishName =  "Child GLAccount was disconnected",
                LocalName =  "כרטיס בן נותק",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = GLAccountObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CHID",
                EnglishName =  "Child GLAccount was added",
                LocalName =  "הכרטיס נוסף ככרטיס בן ",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = GLAccountObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "SGAC",
                EnglishName =  "Splitted GLAccount was activated",
                LocalName =  "כרטיס הפיצול מוּפעָל",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = GLAccountObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "LIAC",
                EnglishName =  "Line activated",
                LocalName =  "שורה הופעלה",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = GLAccountObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "DETV",
                EnglishName =  "Line deactivated",
                LocalName =  "שורה מספר נחסמה",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = GLAccountObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CREV",
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
                ObjectTableId = GLAccountObjectTable.Id,
				 
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
                ObjectTableId = GLAccountObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "ACR",
                EnglishName =  "Created",
                LocalName =  "חדש",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = GLAccountObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "ACH",
                EnglishName =  "Account Number Changed",
                LocalName =  "שינוי מספר חשבון",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = GLAccountObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "NCH",
                EnglishName =  "Account Name Changed",
                LocalName =  "שינוי שם חשבון",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = GLAccountObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CHCH",
                EnglishName =  "Chart of Accounts Changed",
                LocalName =  "שינוי קבוצת מאזן",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = GLAccountObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "BLK",
                EnglishName =  "Account Inactivated",
                LocalName =  "הכרטיס נחסם",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = GLAccountObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "UBLK",
                EnglishName =  "Account Activated",
                LocalName =  "הכרטיס הופעל",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = GLAccountObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "GLRC",
                EnglishName =  "Account Reactivated",
                LocalName =  "הכרטיס הופעל מחדש",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = GLAccountObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CAT1",
                EnglishName =  "Category 1 Changed",
                LocalName =  "שינוי  מזהה קטגוריה 1 ",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = GLAccountObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CAT2",
                EnglishName =  "Category 2 Changed",
                LocalName =  "שינוי  מזהה קטגוריה 2 ",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = GLAccountObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CAT3",
                EnglishName =  "Category 3 Changed",
                LocalName =  "שינוי  מזהה קטגוריה 3 ",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = GLAccountObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CAT4",
                EnglishName =  "Category 4 Changed",
                LocalName =  "שינוי  מזהה קטגוריה 4 ",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = GLAccountObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "RVUP",
                EnglishName =  "Revaluation Enabled Changed",
                LocalName =  "שינוי שערוך אוטומטי ",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = GLAccountObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CAT5",
                EnglishName =  "Category 5 Changed",
                LocalName =  "שינוי  מזהה קטגוריה 5 ",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = GLAccountObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "MLUP",
                EnglishName =  "Is Multi Currency Changed",
                LocalName =  "שינוי רב מטבעי ",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = GLAccountObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "RMUP",
                EnglishName =  "Reconcile Method Changed",
                LocalName =  " שינוי שיטת התאמה ",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = GLAccountObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "ARCP",
                EnglishName =  "Automatic Reconcile Method Changed",
                LocalName =  " שינוי התאמה אוטומטית  ",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = GLAccountObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "VAEX",
                EnglishName =  "Is Vat Eexcempt Changed",
                LocalName =  "שדה פטור מע”מ שונה",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = GLAccountObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "DIST",
                EnglishName =  "Card was disconnected",
                LocalName =  "נותק מכרטיס תפעולי",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = GLAccountObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "NTAD",
                EnglishName =  "New Note",
                LocalName =  "הערה חדשה נוספה",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = GLAccountObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "NTUP",
                EnglishName =  "Note Edited",
                LocalName =  "הערה עודכנה",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = GLAccountObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "NTDL",
                EnglishName =  "Note Deleted",
                LocalName =  "הערה נמחקה",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = GLAccountObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
		   FeatureRepository featureRepository = new FeatureRepository(0); 
		   //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList(); 
		   ObjectTable GLAccountObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "GLAccount" && d.Tenant == 0).FirstOrDefault();       
    
			   Feature GLAccountFeature_MB0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "RECOCILE", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "GLAccount.Features.Reconcile", NameTextCodeDefaultText = "Reconcile", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

			   Feature GLAccountFeature_MB10 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "INACITVE", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "GLAccount.Features.Inactive", NameTextCodeDefaultText = "Inactive", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
             			   Feature GLAccountFeature_MB11 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PRINTCARDINDEX", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "GLAccount.Features.PrintCardIndex", NameTextCodeDefaultText = "Print Card Index", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
             			   Feature GLAccountFeature_MB12 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GLAccountReactivate", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, NameTextCodeCode = "GLAccount.Features.Reactivate", NameTextCodeDefaultText = "Reactivate", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
              

		   TextCodeRepository.SubmitChanges();
		   FeaturesRepository.SubmitChanges();
		   MenuButtonGroup GLAccountMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
				{
					MenuButtonGroupType = "GLAEdit",
					Name = "GLAEditButtonsGroup",
					ObjectTableId = GLAccountObjectTable.Id,
					Tenant = 0
				}, menuButtonGroupRepository, tenantMenuButtonGroups);        
   
			   MenuButton GLAccountMenuButton0 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "Reconcile",
						Index = 0, 
						IsActive = true,
						LabelTextCodeCode = "GLAccount.B.Reconcile",
						LabelTextCodeDefaultText = "Reconcile",
						Tenant = 0,
						MenuButtonGroupId = GLAccountMenuButtonGroup.Id,
						ObjectTableId = GLAccountObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = GLAccountFeature_MB0.Id,
						Style = "ApproveButtonStyle",
						LocalDefaultText = "התאם",
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton GLAccountMenuButton1 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "More",
						Index = 100, 
						IsActive = false,
						LabelTextCodeCode = "GLAccount.B.More",
						LabelTextCodeDefaultText = "More",
						Tenant = 0,
						MenuButtonGroupId = GLAccountMenuButtonGroup.Id,
						ObjectTableId = GLAccountObjectTable.Id,
						MenuButtonType = "dropdownbutton",
						FeatureId = null,
						Style = null,
						LocalDefaultText = "נוספים",
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);

			   MenuButton GLAccountMenuButton10 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "GLAccountInactive",
						Index = 0, 
						IsActive = true,
						LabelTextCodeCode = "GLAccount.B.Inactive",
						LabelTextCodeDefaultText = "Inactive",
						Tenant = 0,
						MenuButtonGroupId = GLAccountMenuButtonGroup.Id,
						ParentMenuButtonId = GLAccountMenuButton1.Id,
						ObjectTableId = GLAccountObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  GLAccountFeature_MB10.Id,
						Style = null,
						LocalDefaultText = "חסימה",
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton GLAccountMenuButton11 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "GLAccountPrintCardIndex",
						Index = 1, 
						IsActive = true,
						LabelTextCodeCode = "GLAccount.B.PrintCardIndex",
						LabelTextCodeDefaultText = "Print Card Index",
						Tenant = 0,
						MenuButtonGroupId = GLAccountMenuButtonGroup.Id,
						ParentMenuButtonId = GLAccountMenuButton1.Id,
						ObjectTableId = GLAccountObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  GLAccountFeature_MB11.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton GLAccountMenuButton12 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "GLAccountReactivate",
						Index = 0, 
						IsActive = true,
						LabelTextCodeCode = "GLAccount.B.Reactivate",
						LabelTextCodeDefaultText = "Reactivate",
						Tenant = 0,
						MenuButtonGroupId = GLAccountMenuButtonGroup.Id,
						ParentMenuButtonId = GLAccountMenuButton1.Id,
						ObjectTableId = GLAccountObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  GLAccountFeature_MB12.Id,
						Style = null,
						LocalDefaultText = "הפעל מחדש",
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable GLAccountObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "GLAccount" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode GLAccountTextCode_GLAccountONoCreditDefined = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccount.O.NoCreditDefined", DefaultText = "Credit balance not defined",LocalDefaultText = @"לא הוגדרה מסגרת אשראי ללקוח", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GLAccountTextCode_GLAccountOCreditStatus = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccount.O.CreditStatus ", DefaultText = "Credit Status",LocalDefaultText = @"מצב אשראי", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GLAccountTextCode_GLAccountOQueries = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccount.O.Queries", DefaultText = "GL Accounts Queries",LocalDefaultText = @"שאילתות כרטיסים", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GLAccountTextCode_GLAccountOCounterIsntDefined = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccount.O.CounterIsntDefined", DefaultText = "Counter is not defined, please check accounting settings ",LocalDefaultText = @"מונה כרטיס לא הוגדר , אנא בדוק הגדרות הנהח''ש", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GLAccountTextCode_GLAccountOConnect2ExistingCard = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccount.O.Connect2ExistingCard", DefaultText = "Connect to an existing card",LocalDefaultText = @"חיבור לכרטיס קיים", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GLAccountTextCode_GLAccountOThisGLAccountConnected = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccount.O.ThisGLAccountConnected", DefaultText = "This GL Account is already connected to card (#cards). GL Account cannot be linked to two clients card.",LocalDefaultText = @"שים לב כי הכרטיס כבר מקושר לכרטיס תפעולי (#cards), לא ניתן לקשר כרטיס לשני לקוחות.", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GLAccountTextCode_GLAccountOThisGLAccountConnectedContinue = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccount.O.ThisGLAccountConnectedContinue", DefaultText = "This GL Account is already connected to cards (#cards), Do you want to continue?",LocalDefaultText = @"שים לב כי הכרטסת כבר מקושרת לכרטיס תפעולי (#cards), האם ברצונך להמשיך?", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GLAccountTextCode_GLAccountOExternalAdjust = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccount.O.ExternalAdjust", DefaultText = "External Adjust",LocalDefaultText = @"התאם חיצונית", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GLAccountTextCode_GLAccountONewExternalTransaction = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccount.O.NewExternalTransaction", DefaultText = "New External Transaction",LocalDefaultText = @"דף חיצוני חדש", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GLAccountTextCode_GLAccountOExternalTransaction = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GLAccount.O.ExternalTransaction", DefaultText = "External Transaction",LocalDefaultText = @"תנועות חיצוניות", ObjectTableId = GLAccountObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 