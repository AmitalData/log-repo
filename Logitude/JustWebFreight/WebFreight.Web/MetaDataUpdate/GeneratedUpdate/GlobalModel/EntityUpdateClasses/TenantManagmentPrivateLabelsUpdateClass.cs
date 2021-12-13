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
using Logitude.CargoTracking.Data.Repositories;
using Logitude.CargoTracking.BL;
using Logitude.CargoTracking.Data.EntityPOCOs;

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.GlobalModel.EntityUpdateClasses
{
   public class TenantManagmentPrivateLabelsUpdateClass
   {  		
		public const string HashString = "5c5535dc3409ef23a967d28773088886";
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "TenantManagmentPrivateLabels",
			      				    IsNew =  false,
			      				    DBTableName =  "TenantManagmentPrivateLabels",
			      				    ObjectTableSingular =  "Tenant Managment Private Labels",
			      				    ObjectTablePlural =  "Tenant Managment Private Labels",
			      				    HasCustomFilter =  false,
			      				    HasCustomFields =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  false,
			      				    IsNewWizard =  true,
			      				    LookUp1 =  "Id",
			      				    LookUp2 =  "PrivateLabelName",
			      				    KeyPropertyPath =  "Id",
			      				    AutoCompleteSearchWindow =  false,
			      				    IsClosed =  false,
			      				    CacheOnClient =  false,
			      				    EditableFromAutoCompleteWindow =  false,
			      				    HasCounter =  false,
			      				    EnableAddFromLOV =  false,
			      				    IsRestrictable =  false,
			      				    IsMain =  true,
			      				    IsAutoComplete =  true,
			      				    EnableEditFromLOV =  false,
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    NewWizardControlName =  "Simplog.Infrastructure.Views.TenantManagmentPrivateLabels.AddEditPrivateLabelsControl",
			      				    DefaultText =  "Tenant Managment Private Labels",
			      				    Code =  "TMPL",
			      				    Name =  "Private Labels",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Infrastructure",
			      				    NewWizardComponentPath =  "./InfrastructureModules/InfrastructureTenantManagement/Components/TenantManagement/AddEditPrivateLabelsComponent",
			      				    NoTS =  false,
			      				    HasMenuButtons =  false,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  true,
			      				    IsTabsHidden =  false,
			      				    SearchFields =  "TenantManagmentPrivateLabels,TenantManagmentPrivateLabels,Simplog.Infrastructure.Views.TenantManagmentPrivateLabels.AddEditPrivateLabelsControl,Id,",
			      				    HashString =  TenantManagmentPrivateLabelsUpdateClass.HashString,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository, List<ObjectField> addedFields, List<TextCode> addedTextCodes)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PrivateLabelShortName",
					  						ObjectTableName =  "TenantManagmentPrivateLabels",
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
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  true,
					  						PMPropertyPath =  "PrivateLabelShortName",
					  						ListPropertyPath =  "PrivateLabelShortName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "TenantManagmentPrivateLabels",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "PrivateLabelShortName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  true,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PrivateLabelShortName",
					  						DefaultText =  "Private Label Short Name",
					  						ListFieldLable =  "PrivateLabelShortNameListLable",
					  						ListLableDefaultText =  "Private Label Short Name",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Private Label Short Name",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PrivateLabelName",
					  						ObjectTableName =  "TenantManagmentPrivateLabels",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  60,
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
					  						PMPropertyPath =  "PrivateLabelName",
					  						ListPropertyPath =  "PrivateLabelName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "TenantManagmentPrivateLabels",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "PrivateLabelName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  true,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PrivateLabelName",
					  						DefaultText =  "Private Label Name",
					  						ListFieldLable =  "PrivateLabelNameListLable",
					  						ListLableDefaultText =  "Private Label Name",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Private Label Name",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PrivateLabelUrl",
					  						ObjectTableName =  "TenantManagmentPrivateLabels",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  100,
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PrivateLabelUrl",
					  						ListPropertyPath =  "PrivateLabelUrl",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "TenantManagmentPrivateLabels",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "PrivateLabelUrl",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PrivateLabelUrl",
					  						DefaultText =  "Private Label Url",
					  						ListFieldLable =  "PrivateLabelUrlListLable",
					  						ListLableDefaultText =  "Private Label Url",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Private Label Url",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ContactUsEmail",
					  						ObjectTableName =  "TenantManagmentPrivateLabels",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  70,
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ContactUsEmail",
					  						ListPropertyPath =  "ContactUsEmaile",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "TenantManagmentPrivateLabels",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ContactUsEmail",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ContactUsEmail",
					  						DefaultText =  "Contact Us Email",
					  						ListFieldLable =  "ContactUsEmailListLable",
					  						ListLableDefaultText =  "Contact Us Email",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Contact Us Email",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ReceiveAllStatuses",
					  						ObjectTableName =  "TenantManagmentPrivateLabels",
					  						FieldsDataType =  "Boolean",
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
					  						PMPropertyPath =  "ReceiveAllStatuses",
					  						ListPropertyPath =  "ReceiveAllStatuses",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "TenantManagmentPrivateLabels",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ReceiveAllStatuses",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ReceiveAllStatuses",
					  						DefaultText =  "Receive All Statuses",
					  						ListFieldLable =  "ReceiveAllStatusesListLable",
					  						ListLableDefaultText =  "Receive All Statuses",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Receive All Statuses",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InActive",
					  						ObjectTableName =  "TenantManagmentPrivateLabels",
					  						FieldsDataType =  "Boolean",
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
					  						ValidForQuerySection1 =  "TenantManagmentPrivateLabels",
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
					  						DefaultText =  "In Active",
					  						ListFieldLable =  "InActiveListLable",
					  						ListLableDefaultText =  "In Active",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "In Active",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SearchFields",
					  						ObjectTableName =  "TenantManagmentPrivateLabels",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  500,
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
					  						ValidForQuerySection1 =  "TenantManagmentPrivateLabels",
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
					  						DefaultText =  "Search..",
					  						HelpTextCode =  "SearchFields",
					  						HelpTextDefaultText =  "Searching by :\n1: Short Name\n2: Full Name",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PrivateLabelDomain",
					  						ObjectTableName =  "TenantManagmentPrivateLabels",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  100,
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
					  						PMPropertyPath =  "PrivateLabelDomain",
					  						ListPropertyPath =  "PrivateLabelDomain",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "TenantManagmentPrivateLabels",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PrivateLabelDomain",
					  						DefaultText =  "Private Label Domain",
					  						ListFieldLable =  "PrivateLabelDomainListLable",
					  						ListLableDefaultText =  "Private Label Domain",
					  						HelpTextCode =  "PrivateLabelDomain",
					  						HelpTextDefaultText =  "The reset password emails will be sent from the domain you filled in this field.",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters,Dictionary<string, QueryGroup> tenantQueryGroups )
	    {  
	        //FeatureRepository featureRepository = new FeatureRepository(0); 
            //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
	        QueryGroup TenantManagmentPrivateLabelsQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "TMPL", Name = "Private Labels" }, queryGroupRepository,tenantQueryGroups);
						QueryGroup TenantManagmentPrivateLabelsQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "387b", Name = " Query Group" }, queryGroupRepository,tenantQueryGroups);
				        queryGroupRepository.SubmitChanges();
	        ObjectTable TenantManagmentPrivateLabelsObjectTable = objectTables.ContainsKey("TenantManagmentPrivateLabels") ? objectTables["TenantManagmentPrivateLabels"] : null;
            if (TenantManagmentPrivateLabelsObjectTable == null)
            {
                IWebFreightContext objectContext = WebFreightContext.GetContext(0);  

                TenantManagmentPrivateLabelsObjectTable = objectContext.ObjectTables.Where(d => d.Name == "TenantManagmentPrivateLabels" && d.Tenant == 0).FirstOrDefault();
            }

	         
			List<Feature> addedFeatures = new List<Feature>();
			List<TextCode> addedTextCodes = new List<TextCode>();
			List<Query> addedQueries = new List<Query>();
			List<QueryColumn> addedQueryColumns = new List<QueryColumn>();
			List<AdvancedQueryFilter> addedQueryFilters = new List<AdvancedQueryFilter>();
   

			   TextCode TenantManagmentPrivateLabelsTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TenantManagmentPrivateLabels.Q.PrivateLabels", DefaultText = @"Private Labels",LocalDefaultText = null, ObjectTableId = TenantManagmentPrivateLabelsObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature TenantManagmentPrivateLabelsFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PrivateLabelsQ", ObjectTableId = TenantManagmentPrivateLabelsObjectTable.Id, Tenant = 0, NameTextCodeCode = "TenantManagmentPrivateLabels.Features.PrivateLabels", NameTextCodeDefaultText = "Private Labels", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,TenantManagmentPrivateLabelsObjectTable, addedFeatures, addedTextCodes);

	        //TextCodeRepository.SubmitChanges();
	        //FeaturesRepository.SubmitChanges();    
	      

			  Query PrivateLabelsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = TenantManagmentPrivateLabelsTextCode_0.Id, NameTextCodeCode = TenantManagmentPrivateLabelsTextCode_0.Code, ObjectTableName = "TenantManagmentPrivateLabels", Code = "PrivateLabels",  EditWizardName = "Simplog.Infrastructure.Views.TenantManagmentPrivateLabels.AddEditPrivateLabelsControl",
			   QueryGroupCode = "TMPL", IndexOrder = 0, Tenant = 0, ObjectTableId = TenantManagmentPrivateLabelsObjectTable.Id, QuerySection = "TenantManagmentPrivateLabels", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = TenantManagmentPrivateLabelsFeature_0.Id,FeatureUniqeCode= TenantManagmentPrivateLabelsFeature_0.FeatureUniqeCode, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, addedQueries);
	
			 QueryColumn PrivateLabelsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PrivateLabelsQuery.Id,QueryCode = PrivateLabelsQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "TenantManagmentPrivateLabels.PrivateLabelName" , ColumnWidth = 200 }, addedQueryColumns);

			 QueryColumn PrivateLabelsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PrivateLabelsQuery.Id,QueryCode = PrivateLabelsQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "TenantManagmentPrivateLabels.PrivateLabelShortName" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn PrivateLabelsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PrivateLabelsQuery.Id,QueryCode = PrivateLabelsQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "TenantManagmentPrivateLabels.PrivateLabelUrl" , ColumnWidth = 200 }, addedQueryColumns);

			 QueryColumn PrivateLabelsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PrivateLabelsQuery.Id,QueryCode = PrivateLabelsQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "TenantManagmentPrivateLabels.ContactUsEmail" , ColumnWidth = 200 }, addedQueryColumns);

			 QueryColumn PrivateLabelsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PrivateLabelsQuery.Id,QueryCode = PrivateLabelsQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "TenantManagmentPrivateLabels.ReceiveAllStatuses" , ColumnWidth = 70 }, addedQueryColumns);

			 QueryColumn PrivateLabelsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = PrivateLabelsQuery.Id,QueryCode = PrivateLabelsQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "TenantManagmentPrivateLabels.InActive" , ColumnWidth = 70 }, addedQueryColumns);
			SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
			SqlBulkInsert.BulkInsert("Features", addedFeatures);
			SqlBulkInsert.BulkInsert("Queries", addedQueries);
			SqlBulkInsert.BulkInsert("QueryColumns", addedQueryColumns);
			SqlBulkInsert.BulkInsert("AdvancedQueryFilters", addedQueryFilters);	 
  
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable TenantManagmentPrivateLabelsObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "TenantManagmentPrivateLabels" && d.Tenant == 0).FirstOrDefault();
		   //List<ObjectField> TenantManagmentPrivateLabelsObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "TenantManagmentPrivateLabels").ToList();
		       
	      

	         Screen TenantManagmentPrivateLabelsTenantManagmentPrivateLabelsHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "TenantManagmentPrivateLabels.HeaderScreen", Name = "TenantManagmentPrivateLabelsHeaderScreen", ObjectTableId = TenantManagmentPrivateLabelsObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      	
		    TenantManagmentPrivateLabelsObjectTable.HeaderScreenId = TenantManagmentPrivateLabelsTenantManagmentPrivateLabelsHeaderScreenScreen0.Id;
		    TenantManagmentPrivateLabelsObjectTable.HeaderScreenCode = TenantManagmentPrivateLabelsTenantManagmentPrivateLabelsHeaderScreenScreen0.Code;

	   		  

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {      
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable TenantManagmentPrivateLabelsObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "TenantManagmentPrivateLabels" && d.Tenant == 0).FirstOrDefault(); 

		   Feature TenantManagmentPrivateLabelsFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = TenantManagmentPrivateLabelsObjectTable.Id, Tenant = 0, NameTextCodeCode = "TenantManagmentPrivateLabels.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,TenantManagmentPrivateLabelsObjectTable);
		   Feature TenantManagmentPrivateLabelsFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = TenantManagmentPrivateLabelsObjectTable.Id, Tenant = 0, NameTextCodeCode = "TenantManagmentPrivateLabels.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,TenantManagmentPrivateLabelsObjectTable);
		   Feature TenantManagmentPrivateLabelsFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = TenantManagmentPrivateLabelsObjectTable.Id, Tenant = 0, NameTextCodeCode = "TenantManagmentPrivateLabels.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,TenantManagmentPrivateLabelsObjectTable);
		   Feature TenantManagmentPrivateLabelsFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = false, ObjectTableId = TenantManagmentPrivateLabelsObjectTable.Id, Tenant = 0, NameTextCodeCode = "TenantManagmentPrivateLabels.Features.PackageFeature", NameTextCodeDefaultText = "TenantManagmentPrivateLabels Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,TenantManagmentPrivateLabelsObjectTable);    
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable TenantManagmentPrivateLabelsObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "TenantManagmentPrivateLabels" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CREV",
                EnglishName =  "Created",
                LocalName =  "Created",
                IsManualEntry =  false,
                ShortView =  true,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = TenantManagmentPrivateLabelsObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "UPEV",
                EnglishName =  "Updated",
                LocalName =  "Updated",
                IsManualEntry =  false,
                ShortView =  false,
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = TenantManagmentPrivateLabelsObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable TenantManagmentPrivateLabelsObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "TenantManagmentPrivateLabels" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode TenantManagmentPrivateLabelsTextCode_TenantManagmentPrivateLabelsSMain = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TenantManagmentPrivateLabels.S.Main", DefaultText = "Main",LocalDefaultText = null, ObjectTableId = TenantManagmentPrivateLabelsObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode TenantManagmentPrivateLabelsTextCode_TenantManagmentPrivateLabelsSBrandingData = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TenantManagmentPrivateLabels.S.BrandingData", DefaultText = "Branding Data",LocalDefaultText = null, ObjectTableId = TenantManagmentPrivateLabelsObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode TenantManagmentPrivateLabelsTextCode_TenantManagmentPrivateLabelsSTermsOfUse = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "TenantManagmentPrivateLabels.S.TermsOfUse", DefaultText = "Terms Of Use",LocalDefaultText = null, ObjectTableId = TenantManagmentPrivateLabelsObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 