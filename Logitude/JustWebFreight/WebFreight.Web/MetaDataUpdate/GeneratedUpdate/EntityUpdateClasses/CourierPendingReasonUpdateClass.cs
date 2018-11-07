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
using Logitude.Customs.BL.CloseTables;
using Logitude.CRM.BL.CLoseTable;
using Logitude.BookingLib.BL.CLoseTable;
using Logitude.WarehouseLib.Data.Repositories;
using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.WarehouseLib.BL.CLoseTable;
using Logitude.TimeManagement.Data.Repositories;
using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.BL.CLoseTable;
namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.EntityUpdateClasses
{
   public class CourierPendingReasonUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "Customs.CourierPendingReason",
			      				    IsNew =  false,
			      				    DBTableName =  "Customs.CourierPendingReasons",
			      				    OldDBTableName =  "Customs.CourierPendingReasons",
			      				    ObjectTableSingular =  "CourierPendingReason",
			      				    ObjectTablePlural =  "CourierPendingReasons",
			      				    HasCustomFilter =  false,
			      				    HasCustomFields =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  false,
			      				    IsNewWizard =  true,
			      				    LookUp1 =  "Code",
			      				    LookUp2 =  "LocalName",
			      				    KeyPropertyPath =  "Code",
			      				    AutoCompleteSearchWindow =  false,
			      				    IsClosed =  false,
			      				    CacheOnClient =  false,
			      				    EditableFromAutoCompleteWindow =  false,
			      				    HasCounter =  false,
			      				    EnableAddFromLOV =  true,
			      				    IsRestrictable =  false,
			      				    IsMain =  true,
			      				    IsAutoComplete =  true,
			      				    EnableEditFromLOV =  false,
			      				    SortingByObjectField =  "Code",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "BR",
			      				    MaxNumberOfCustomFields =  0,
			      				    NewWizardControlName =  "AddEditCourierPendingReasonComponent",
			      				    LocalDefaultText =  "סיבת Pending",
			      				    DefaultText =  "Courier Pending Reason",
			      				    Code =  "aef0",
			      				    Name =  "Customs.CourierPendingReason Query Group",
			      				    CloseTableCode =  "Code",
			      				    CloseTableName =  "LocalName",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Customs",
			      				    ServerModuleName =  "Customs",
			      				    NewWizardComponentPath =  "./CustomsModules/CustomsCourier/Components/CourierPendingReason/AddEditCourierPendingReasonComponent",
			      				    NoTS =  false,
			      				    HasMenuButtons =  false,
			      				    AllowedForComputingPartners =  false,
			      				    Code1 =  "bc89",
			      				    Name1 =  " Query Group",
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Code",
					  						OldFieldName =  "Code",
					  						IsNew =  false,
					  						IsChecked =  false,
					  						IsDeleted =  false,
					  						ObjectTableName =  "Customs.CourierPendingReason",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  4,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  true,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  4,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  true,
					  						PMPropertyPath =  "Code",
					  						ListPropertyPath =  "Code",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierPendingReason",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Code",
					  						DefaultText =  "Code",
					  						FullLocalDefaultText =  "קוד",
					  						ListFieldLable =  "CodeListLable",
					  						ListLableDefaultText =  "Code",
					  						ListLocalDefaultText =  "קוד",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						IsSpellCheckedFullFieldLable =  false,
					  						IsSpellCheckedHelpLocalDefaultText =  false,
					  						IsSpellCheckedShortLocalDefaultText =  false,
					  						IsSpellCheckedListLocalDefaultText =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LocalName",
					  						OldFieldName =  "LocalName",
					  						IsNew =  false,
					  						IsChecked =  false,
					  						IsDeleted =  false,
					  						ObjectTableName =  "Customs.CourierPendingReason",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  true,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  100,
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
					  						ValidForQuerySection1 =  "Customs.CourierPendingReason",
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
					  						IsSpellCheckedFullFieldLable =  false,
					  						IsSpellCheckedHelpLocalDefaultText =  false,
					  						IsSpellCheckedShortLocalDefaultText =  false,
					  						IsSpellCheckedListLocalDefaultText =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SearchFields",
					  						OldFieldName =  "SearchFields",
					  						IsNew =  false,
					  						IsChecked =  false,
					  						IsDeleted =  false,
					  						ObjectTableName =  "Customs.CourierPendingReason",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  1000,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  1000,
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
					  						ValidForQuerySection1 =  "Customs.CourierPendingReason",
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
					  						IsSpellCheckedFullFieldLable =  false,
					  						IsSpellCheckedHelpLocalDefaultText =  false,
					  						IsSpellCheckedShortLocalDefaultText =  false,
					  						IsSpellCheckedListLocalDefaultText =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "EnglishName",
					  						OldFieldName =  "EnglishName",
					  						IsNew =  false,
					  						IsChecked =  false,
					  						IsDeleted =  false,
					  						ObjectTableName =  "Customs.CourierPendingReason",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  40,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "EnglishName",
					  						ListPropertyPath =  "EnglishName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierPendingReason",
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
					  						FullLocalDefaultText =  "שם אנגלית",
					  						ListFieldLable =  "EnglishNameListLable",
					  						ListLableDefaultText =  "English Name",
					  						ListLocalDefaultText =  "שם אנגלית",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						IsSpellCheckedFullFieldLable =  false,
					  						IsSpellCheckedHelpLocalDefaultText =  false,
					  						IsSpellCheckedShortLocalDefaultText =  false,
					  						IsSpellCheckedListLocalDefaultText =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Inactive",
					  						OldFieldName =  "Inactive",
					  						IsNew =  false,
					  						IsChecked =  false,
					  						IsDeleted =  false,
					  						ObjectTableName =  "Customs.CourierPendingReason",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
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
					  						ValidForQuerySection1 =  "Customs.CourierPendingReason",
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
					  						FullLocalDefaultText =  "לא פּעיל",
					  						ListFieldLable =  "InactiveListLable",
					  						ListLableDefaultText =  "Inactive",
					  						ListLocalDefaultText =  "לא פּעיל",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						IsSpellCheckedFullFieldLable =  false,
					  						IsSpellCheckedHelpLocalDefaultText =  false,
					  						IsSpellCheckedShortLocalDefaultText =  false,
					  						IsSpellCheckedListLocalDefaultText =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ErrorPlace",
					  						OldFieldName =  "ErrorPlace",
					  						IsNew =  false,
					  						IsChecked =  true,
					  						IsDeleted =  false,
					  						ObjectTableName =  "Customs.CourierPendingReason",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.PendingErrorPlace",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
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
					  						PMPropertyPath =  "ErrorPlace",
					  						ListPropertyPath =  "ErrorPlace",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierPendingReason",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ErrorPlace",
					  						DefaultText =  "Error Place",
					  						FullLocalDefaultText =  "קוד עצירה",
					  						ListFieldLable =  "ErrorPlaceListLable",
					  						ListLableDefaultText =  "Error Place",
					  						ListLocalDefaultText =  "קוד עצירה",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						IsSpellCheckedFullFieldLable =  false,
					  						IsSpellCheckedHelpLocalDefaultText =  false,
					  						IsSpellCheckedShortLocalDefaultText =  false,
					  						IsSpellCheckedListLocalDefaultText =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UnifreightStatusCode",
					  						OldFieldName =  "UnifreightStatusCode",
					  						IsNew =  false,
					  						IsChecked =  false,
					  						IsDeleted =  false,
					  						ObjectTableName =  "Customs.CourierPendingReason",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  3,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  3,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "UnifreightStatusCode",
					  						ListPropertyPath =  "UnifreightStatusCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierPendingReason",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UnifreightStatusCode",
					  						DefaultText =  "Unifreight Status",
					  						FullLocalDefaultText =  "סטטוס יוניפרייט",
					  						ListFieldLable =  "UnifreightStatusCodeListLable",
					  						ListLableDefaultText =  "Unifreight Status",
					  						ListLocalDefaultText =  "סטטוס יוניפרייט",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						IsSpellCheckedFullFieldLable =  false,
					  						IsSpellCheckedHelpLocalDefaultText =  false,
					  						IsSpellCheckedShortLocalDefaultText =  false,
					  						IsSpellCheckedListLocalDefaultText =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ErrorPlaceName",
					  						OldFieldName =  "ErrorPlaceName",
					  						IsNew =  true,
					  						IsChecked =  true,
					  						IsDeleted =  false,
					  						ObjectTableName =  "Customs.CourierPendingReason",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  40,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ErrorPlaceName",
					  						ListPropertyPath =  "ErrorPlaceName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CourierPendingReason",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ErrorPlaceName",
					  						DefaultText =  "Error Place Name",
					  						FullLocalDefaultText =  "קוד עצירה",
					  						ListFieldLable =  "ErrorPlaceNameListLable",
					  						ListLableDefaultText =  "Error Place Name",
					  						ListLocalDefaultText =  "קוד עצירה",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						IsSpellCheckedFullFieldLable =  false,
					  						IsSpellCheckedHelpLocalDefaultText =  false,
					  						IsSpellCheckedShortLocalDefaultText =  false,
					  						IsSpellCheckedListLocalDefaultText =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	    {  
	        FeatureRepository featureRepository = new FeatureRepository(0); 
            List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
            IWebFreightContext objectContext = WebFreightContext.GetContext(0);  
	        QueryGroup CourierPendingReasonQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "aef0", Name = "Customs.CourierPendingReason Query Group" }, queryGroupRepository);
	        queryGroupRepository.SubmitChanges();

	        ObjectTable CourierPendingReasonObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Customs.CourierPendingReason" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> CourierPendingReasonObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Customs.CourierPendingReason").ToList();   

			   TextCode CourierPendingReasonTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CourierPendingReason.Q.AllCouriersPending", DefaultText = "All Couriers Pending",LocalDefaultText = "Pending Reason", ObjectTableId = CourierPendingReasonObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature CourierPendingReasonFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CourierPendingReason.Q.AllCouriersPending", ObjectTableId = CourierPendingReasonObjectTable.Id, Tenant = 0, NameTextCodeCode = "CourierPendingReason.Features.AllCouriersPending", NameTextCodeDefaultText = "AllCouriersPending", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query AllCouriersPendingQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = CourierPendingReasonTextCode_0.Id, Code = "AllCouriersPending",  QueryGroupCode = "aef0", IndexOrder = 0, Tenant = 0, ObjectTableId = CourierPendingReasonObjectTable.Id, QuerySection = "Customs.CourierPendingReason", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = CourierPendingReasonFeature_0.Id, DefaultSortName = "Code", DefaultSortDirection = "Desending" }, queriesRepository, tenantQueries);
	
			 QueryColumn AllCouriersPendingQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCouriersPendingQuery.Id, IndexOrder = 0, ObjectFieldId = CourierPendingReasonObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == CourierPendingReasonObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllCouriersPendingQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCouriersPendingQuery.Id, IndexOrder = 1, ObjectFieldId = CourierPendingReasonObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == CourierPendingReasonObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllCouriersPendingQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCouriersPendingQuery.Id, IndexOrder = 2, ObjectFieldId = CourierPendingReasonObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == CourierPendingReasonObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllCouriersPendingQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCouriersPendingQuery.Id, IndexOrder = 3, ObjectFieldId = CourierPendingReasonObjectFields.Where(d => d.FieldName == "ErrorPlaceName" && d.ObjectTableId == CourierPendingReasonObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllCouriersPendingQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCouriersPendingQuery.Id, IndexOrder = 4, ObjectFieldId = CourierPendingReasonObjectFields.Where(d => d.FieldName == "UnifreightStatusCode" && d.ObjectTableId == CourierPendingReasonObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 167 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllCouriersPendingQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCouriersPendingQuery.Id, IndexOrder = 5, ObjectFieldId = CourierPendingReasonObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == CourierPendingReasonObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 50 }, queryColumnsRepository, tenantQueryColumns);
	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable CourierPendingReasonObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.CourierPendingReason" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> CourierPendingReasonObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Customs.CourierPendingReason").ToList();
		       
	      

	         Screen CourierPendingReasonCustomsCourierPendingReasonHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "CourierPendingReason.Customs.CourierPendingReasonHeaderScreen", Name = "Customs.CourierPendingReasonHeaderScreen", ObjectTableId = CourierPendingReasonObjectTable.Id, NumberOfColumns = 3, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField CourierPendingReasonCustomsCourierPendingReasonHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = CourierPendingReasonObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().Id, ScreenId = CourierPendingReasonCustomsCourierPendingReasonHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CourierPendingReasonCustomsCourierPendingReasonHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = CourierPendingReasonObjectFields.Where(d => d.FieldName == "LocalName").FirstOrDefault().Id, ScreenId = CourierPendingReasonCustomsCourierPendingReasonHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CourierPendingReasonCustomsCourierPendingReasonHeaderScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = CourierPendingReasonObjectFields.Where(d => d.FieldName == "ErrorPlaceName").FirstOrDefault().Id, ScreenId = CourierPendingReasonCustomsCourierPendingReasonHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    CourierPendingReasonObjectTable.HeaderScreenId = CourierPendingReasonCustomsCourierPendingReasonHeaderScreenScreen0.Id;
	   		  

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {    
			 ObjectTable CourierPendingReasonObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.CourierPendingReason" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode CourierPendingReasonGeneralTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierPendingReason.TH.General", DefaultText = "General",LocalDefaultText = "כללי", ObjectTableId = CourierPendingReasonObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CourierPendingReasonGeneralFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CourierPendingReason.Tab.General", ObjectTableId = CourierPendingReasonObjectTable.Id, Tenant = 0, NameTextCodeCode = "CourierPendingReason.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode CourierPendingReasonEventsTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierPendingReason.TH.Events", DefaultText = "Events",LocalDefaultText = "אירועים", ObjectTableId = CourierPendingReasonObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CourierPendingReasonEventsFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CourierPendingReason.Tab.Events", ObjectTableId = CourierPendingReasonObjectTable.Id, Tenant = 0, NameTextCodeCode = "CourierPendingReason.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CPGT",HtmlComponentName = "AddEditCourierPendingReasonComponent",HtmlComponentUrl = "./CustomsModules/CustomsCourier/Components/CourierPendingReason/AddEditCourierPendingReasonComponent", FeatureId = tenantFeatures.Where(d => d.Code == "CourierPendingReason.Tab.General" && d.ObjectTableId == CourierPendingReasonObjectTable.Id).FirstOrDefault().Id, ControlPath = "", ObjectTableId = CourierPendingReasonObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Customs.CourierPendingReason.TH.General" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CPET",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = tenantFeatures.Where(d => d.Code == "CourierPendingReason.Tab.Events" && d.ObjectTableId == CourierPendingReasonObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = CourierPendingReasonObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Customs.CourierPendingReason.TH.Events" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable CourierPendingReasonObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.CourierPendingReason" && d.Tenant == 0).FirstOrDefault(); 
		   Feature CourierPendingReasonFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = CourierPendingReasonObjectTable.Id, Tenant = 0, NameTextCodeCode = "CourierPendingReason.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature CourierPendingReasonFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = CourierPendingReasonObjectTable.Id, Tenant = 0, NameTextCodeCode = "CourierPendingReason.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature CourierPendingReasonFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = CourierPendingReasonObjectTable.Id, Tenant = 0, NameTextCodeCode = "CourierPendingReason.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature CourierPendingReasonFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = CourierPendingReasonObjectTable.Id, Tenant = 0, NameTextCodeCode = "CourierPendingReason.Features.PackageFeature", NameTextCodeDefaultText = "CourierPendingReason Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);    
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext)
	    {   
			ObjectTable CourierPendingReasonObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.CourierPendingReason" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CREV",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
				IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = CourierPendingReasonObjectTable.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPEV",
                EnglishName = "Updated",
                Tenant = 0,
                AddedManually = false,
				IsManualEntry = false,
                LocalName = "Updated",
                ObjectTableId = CourierPendingReasonObjectTable.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
	    }     

   }
    
}
	 