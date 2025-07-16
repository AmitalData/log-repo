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
using Logitude.Workflow.Data.Repositories;
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.BL.CLoseTable;
using Logitude.DashboardModule.Data.Repositories;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.BL;




namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.EntityUpdateClasses
{
   public class SIIRequestUpdateClass
   {  		
		public const string HashString = "dde7fe999c206f4e725b55baef425e97";
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "Customs.SIIRequest",
			      				    IsNew =  true,
			      				    DBTableName =  "Customs.SIIRequests",
			      				    ObjectTableSingular =  "SIIRequest",
			      				    ObjectTablePlural =  "SIIRequests",
			      				    HasCustomFilter =  false,
			      				    HasCustomFields =  false,
			      				    AvailableInCustomization =  false,
			      				    SupportSubEntity =  false,
			      				    ApplyGenericCustomFields =  false,
			      				    AvailableInDocumentTypes =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  false,
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
			      				    IsAutoComplete =  true,
			      				    EnableEditFromLOV =  false,
			      				    SortingByObjectField =  "Id",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  false,
			      				    IsComposition =  false,
			      				    EnableSecurity =  false,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    LocalDefaultText =  "בקשות למכון תקנים",
			      				    DefaultText =  "SIIRequest",
			      				    Code =  "7b9b",
			      				    Name =  " Query Group",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Customs",
			      				    ServerModuleName =  "Customs",
			      				    NoTS =  false,
			      				    HasMenuButtons =  false,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    IsTabsHidden =  false,
			      				    HashString =  SIIRequestUpdateClass.HashString,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository, List<ObjectField> addedFields, List<TextCode> addedTextCodes)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Id",
					  						ObjectTableName =  "Customs.SIIRequest",
					  						FieldsDataType =  "Text",
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
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Id",
					  						ListPropertyPath =  "Id",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "SIIRequest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Id",
					  						DefaultText =  "Id",
					  						ListFieldLable =  "IdListLable",
					  						ListLableDefaultText =  "Id",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  true,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  true,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						DisplayInRequiredFields =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Tenant",
					  						ObjectTableName =  "Customs.SIIRequest",
					  						FieldsDataType =  "Integer",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Tenant",
					  						ListPropertyPath =  "Tenant",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "SIIRequest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Tenant",
					  						DefaultText =  "Tenant",
					  						ListFieldLable =  "TenantListLable",
					  						ListLableDefaultText =  "Tenant",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  true,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  true,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						DisplayInRequiredFields =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SearchFields",
					  						ObjectTableName =  "Customs.SIIRequest",
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
					  						IsListFilter =  false,
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
					  						ValidForQuerySection1 =  "SIIRequest",
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
					  						IsForeignKey =  false,
					  						IsMaxLength =  true,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						DisplayInRequiredFields =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "RequestNo",
					  						ObjectTableName =  "Customs.SIIRequest",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  9,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  9,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "RequestNo",
					  						ListPropertyPath =  "RequestNo",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.SIIRequest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "RequestNo",
					  						DefaultText =  "RequestNo",
					  						FullLocalDefaultText =  "מספר בקשה ",
					  						ListFieldLable =  "RequestNoListLable",
					  						ListLableDefaultText =  "RequestNo",
					  						ListLocalDefaultText =  "מספר בקשה ",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						DisplayInRequiredFields =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DeclarationId",
					  						ObjectTableName =  "Customs.SIIRequest",
					  						FieldsDataType =  "Text",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DeclarationId",
					  						ListPropertyPath =  "DeclarationId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.SIIRequest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DeclarationId",
					  						DefaultText =  "Declaration Id",
					  						ListFieldLable =  "DeclarationIdListLable",
					  						ListLableDefaultText =  "DeclarationId",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						DisplayInRequiredFields =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Status",
					  						ObjectTableName =  "Customs.SIIRequest",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.SIIRequestStatus",
					  						MinLength =  0,
					  						MaxLength =  3,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  3,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Status",
					  						ListPropertyPath =  "Status",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.SIIRequest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Status",
					  						DefaultText =  "Status",
					  						FullLocalDefaultText =  "סטטוס בקשה ",
					  						ListFieldLable =  "StatusListLable",
					  						ListLableDefaultText =  "Status",
					  						ListLocalDefaultText =  "סטטוס בקשה ",
					  						IsForeignKey =  true,
					  						ForeignEntity =  "SIIRequestStatus",
					  						NavigationPropertyName =  "SIIRequestStatus",
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						DisplayInRequiredFields =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "WareHouseAddress",
					  						ObjectTableName =  "Customs.SIIRequest",
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
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "WareHouseAddress",
					  						ListPropertyPath =  "WareHouseAddress",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.SIIRequest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "WareHouseAddress",
					  						DefaultText =  "WareHouse Address ",
					  						FullLocalDefaultText =  "כתובת מחסן ",
					  						ListFieldLable =  "WareHouseAddressListLable",
					  						ListLableDefaultText =  "WareHouse Address ",
					  						ListLocalDefaultText =  "כתובת מחסן ",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						DisplayInRequiredFields =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "WareHouseCity",
					  						ObjectTableName =  "Customs.SIIRequest",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.City",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "WareHouseCity",
					  						ListPropertyPath =  "WareHouseCity",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.SIIRequest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "WareHouseCity",
					  						DefaultText =  "WareHouse City ",
					  						FullLocalDefaultText =  "ישוב מחסן ",
					  						ListFieldLable =  "WareHouseCityListLable",
					  						ListLableDefaultText =  "WareHouse City ",
					  						ListLocalDefaultText =  "ישוב מחסן ",
					  						IsForeignKey =  true,
					  						ForeignEntity =  "City",
					  						NavigationPropertyName =  "City",
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						DisplayInRequiredFields =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsClosed",
					  						ObjectTableName =  "Customs.SIIRequest",
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
					  						IsListFilter =  false,
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
					  						ValidForQuerySection1 =  "Customs.SIIRequest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsClosed",
					  						DefaultText =  "IsClosed",
					  						FullLocalDefaultText =  "סגור/פתוח ",
					  						ListFieldLable =  "IsClosedListLable",
					  						ListLableDefaultText =  "IsClosed",
					  						ListLocalDefaultText =  "סגור/פתוח ",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						DisplayInRequiredFields =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "WareHouseCityName",
					  						ObjectTableName =  "Customs.SIIRequest",
					  						FieldsDataType =  "nText",
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
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "WareHouseCityName",
					  						ListPropertyPath =  "WareHouseCityName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.SIIRequest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "WareHouseCityName",
					  						DefaultText =  "WareHouse City Name",
					  						FullLocalDefaultText =  "ישוב מחסן ",
					  						ListFieldLable =  "WareHouseCityNameListLable",
					  						ListLableDefaultText =  "WareHouseCityName",
					  						ListLocalDefaultText =  "ישוב מחסן ",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						DisplayInRequiredFields =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Remarks",
					  						ObjectTableName =  "Customs.SIIRequest",
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
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Remarks",
					  						ListPropertyPath =  "Remarks",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.SIIRequest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Remarks",
					  						DefaultText =  "Remarks ",
					  						FullLocalDefaultText =  "הערות",
					  						ListFieldLable =  "RemarksListLable",
					  						ListLableDefaultText =  "Remarks ",
					  						ListLocalDefaultText =  "הערות",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						DisplayInRequiredFields =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ListCounter",
					  						ObjectTableName =  "Customs.SIIRequest",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ListCounter",
					  						ListPropertyPath =  "ListCounter",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.SIIRequest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ListCounter",
					  						DefaultText =  "ListCounter",
					  						FullLocalDefaultText =  "#",
					  						ListFieldLable =  "ListCounterListLable",
					  						ListLableDefaultText =  "ListCounter",
					  						ListLocalDefaultText =  "#",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						DisplayInRequiredFields =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ImporterId",
					  						ObjectTableName =  "Customs.SIIRequest",
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
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ImporterId",
					  						ListPropertyPath =  "ImporterId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.SIIRequest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ImporterId",
					  						DefaultText =  "ImporterId",
					  						FullLocalDefaultText =  "מס' יבואן ",
					  						ListFieldLable =  "ImporterIdListLable",
					  						ListLableDefaultText =  "Importer Id",
					  						ListLocalDefaultText =  "מס' יבואן ",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						DisplayInRequiredFields =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ContactName",
					  						ObjectTableName =  "Customs.SIIRequest",
					  						FieldsDataType =  "nText",
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
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ContactName",
					  						ListPropertyPath =  "ContactName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.SIIRequest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ContactName",
					  						DefaultText =  "Contact Name",
					  						FullLocalDefaultText =  "איש קשר",
					  						ListFieldLable =  "ContactNameListLable",
					  						ListLableDefaultText =  "Contact Name",
					  						ListLocalDefaultText =  "איש קשר ",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						DisplayInRequiredFields =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UnloadDate",
					  						ObjectTableName =  "Customs.SIIRequest",
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
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "UnloadDate",
					  						ListPropertyPath =  "UnloadDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.SIIRequest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UnloadDate",
					  						DefaultText =  "Unload Date",
					  						FullLocalDefaultText =  "תאריך הגעה ",
					  						ListFieldLable =  "UnloadDateListLable",
					  						ListLableDefaultText =  "Unload Date",
					  						ListLocalDefaultText =  "תאריך הגעה ",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						DisplayInRequiredFields =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ManifestNumber",
					  						ObjectTableName =  "Customs.SIIRequest",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  35,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  35,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ManifestNumber",
					  						ListPropertyPath =  "ManifestNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.SIIRequest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ManifestNumber",
					  						DefaultText =  "Manifest Number",
					  						FullLocalDefaultText =  "שטר מטען ",
					  						ListFieldLable =  "ManifestNumberListLable",
					  						ListLableDefaultText =  "Manifest Number",
					  						ListLocalDefaultText =  "שטר מטען ",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						DisplayInRequiredFields =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "VesselName",
					  						ObjectTableName =  "Customs.SIIRequest",
					  						FieldsDataType =  "nText",
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
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "VesselName",
					  						ListPropertyPath =  "VesselName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.SIIRequest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "VesselName",
					  						DefaultText =  "Vessel Name",
					  						FullLocalDefaultText =  "שם אוניה ",
					  						ListFieldLable =  "VesselNameListLable",
					  						ListLableDefaultText =  "Vessel Name",
					  						ListLocalDefaultText =  "שם אוניה ",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						DisplayInRequiredFields =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ContactEmail",
					  						ObjectTableName =  "Customs.SIIRequest",
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
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ContactEmail",
					  						ListPropertyPath =  "ContactEmail",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.SIIRequest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ContactEmail",
					  						DefaultText =  "Email",
					  						FullLocalDefaultText =  "אימייל",
					  						ListFieldLable =  "ContactEmailListLable",
					  						ListLableDefaultText =  "Email",
					  						ListLocalDefaultText =  "אימייל",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						DisplayInRequiredFields =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ContactTel",
					  						ObjectTableName =  "Customs.SIIRequest",
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
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ContactTel",
					  						ListPropertyPath =  "ContactTel",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.SIIRequest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ContactTel",
					  						DefaultText =  "ContactTel",
					  						FullLocalDefaultText =  "טלפון ",
					  						ListFieldLable =  "ContactTelListLable",
					  						ListLableDefaultText =  "ContactTel",
					  						ListLocalDefaultText =  "טלפון ",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						DisplayInRequiredFields =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ContactCellPhone",
					  						ObjectTableName =  "Customs.SIIRequest",
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
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ContactCellPhone",
					  						ListPropertyPath =  "ContactCellPhone",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.SIIRequest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ContactCellPhone",
					  						DefaultText =  "Contact CellPhone",
					  						FullLocalDefaultText =  "טלפון נייד ",
					  						ListFieldLable =  "ContactCellPhoneListLable",
					  						ListLableDefaultText =  "Contact CellPhone",
					  						ListLocalDefaultText =  "טלפון נייד ",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						DisplayInRequiredFields =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ContactFax",
					  						ObjectTableName =  "Customs.SIIRequest",
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
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ContactFax",
					  						ListPropertyPath =  "ContactFax",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.SIIRequest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ContactFax",
					  						DefaultText =  "Contact Fax",
					  						FullLocalDefaultText =  "פקס ",
					  						ListFieldLable =  "ContactFaxListLable",
					  						ListLableDefaultText =  "Contact Fax",
					  						ListLocalDefaultText =  "פקס ",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						DisplayInRequiredFields =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ContactId",
					  						ObjectTableName =  "Customs.SIIRequest",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Contact",
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
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ContactId",
					  						ListPropertyPath =  "ContactId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.SIIRequest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ContactId",
					  						DefaultText =  "Contact Id",
					  						ListFieldLable =  "ContactIdListLable",
					  						ListLableDefaultText =  "Contact",
					  						ListLocalDefaultText =  "איש קשר",
					  						IsForeignKey =  true,
					  						ForeignEntity =  "Contact",
					  						NavigationPropertyName =  "Contact",
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						DisplayInRequiredFields =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FromApplicationId",
					  						ObjectTableName =  "Customs.SIIRequest",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "FromApplicationId",
					  						ListPropertyPath =  "FromApplicationId",
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
					  						FullFieldLable =  "FromApplicationId",
					  						DefaultText =  "From ApplicationI d",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						DisplayInRequiredFields =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "OriginCountryCode",
					  						ObjectTableName =  "Customs.SIIRequest",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.CustomsCountry",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  2,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "OriginCountryCode",
					  						ListPropertyPath =  "OriginCountryCode",
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
					  						FullFieldLable =  "OriginCountryCode",
					  						DefaultText =  "Origin Country",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						DisplayInRequiredFields =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UnloadPortCode",
					  						ObjectTableName =  "Customs.SIIRequest",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.UnloadingSiteType",
					  						MinLength =  0,
					  						MaxLength =  17,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  17,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "UnloadPortCode",
					  						ListPropertyPath =  "UnloadPortCode",
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
					  						FullFieldLable =  "UnloadPortCode",
					  						DefaultText =  "Unload Port",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						DisplayInRequiredFields =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "RequestDate",
					  						ObjectTableName =  "Customs.SIIRequest",
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
					  						IsListFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "RequestDate",
					  						ListPropertyPath =  "RequestDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.SIIRequest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "RequestDate",
					  						DefaultText =  "RequestDate ",
					  						FullLocalDefaultText =  "תאריך בקשה",
					  						ListFieldLable =  "RequestDateListLable",
					  						ListLableDefaultText =  "RequestDate ",
					  						ListLocalDefaultText =  "תאריך בקשה",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						DisplayInRequiredFields =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters,Dictionary<string, QueryGroup> tenantQueryGroups )
	    {    
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable SIIRequestObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.SIIRequest" && d.Tenant == 0).FirstOrDefault();
		   //List<ObjectField> SIIRequestObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Customs.SIIRequest").ToList();
		       
	      

	         Screen SIIRequestCustomsSIIRequestHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "SIIRequest.HeaderScreen", Name = "Customs.SIIRequestHeaderScreen", ObjectTableId = SIIRequestObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      	
		    SIIRequestObjectTable.HeaderScreenId = SIIRequestCustomsSIIRequestHeaderScreenScreen0.Id;
		    SIIRequestObjectTable.HeaderScreenCode = SIIRequestCustomsSIIRequestHeaderScreenScreen0.Code;

	   		  

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {      
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable SIIRequestObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.SIIRequest" && d.Tenant == 0).FirstOrDefault(); 

		   Feature SIIRequestFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, NameTextCodeCode = "SIIRequest.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,SIIRequestObjectTable);
		   Feature SIIRequestFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, NameTextCodeCode = "SIIRequest.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,SIIRequestObjectTable);
		   Feature SIIRequestFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, NameTextCodeCode = "SIIRequest.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,SIIRequestObjectTable);
		   Feature SIIRequestFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = false, ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, NameTextCodeCode = "SIIRequest.Features.PackageFeature", NameTextCodeDefaultText = "SIIRequest Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,SIIRequestObjectTable);    
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable SIIRequestObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.SIIRequest" && d.Tenant == 0).FirstOrDefault(); 
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
                ObjectTableId = SIIRequestObjectTable.Id,
				 
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
                ObjectTableId = SIIRequestObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable SIIRequestObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.SIIRequest" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode SIIRequestTextCode_CustomsSIIRequestONewRequest = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.NewRequest", DefaultText = "New Request",LocalDefaultText = @"פתיחת בקשה", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOCompletData = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.CompletData", DefaultText = "Completing application data",LocalDefaultText = @"השלמת נתוני בקשה", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOOpenRequest = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.OpenRequest", DefaultText = "Open SIIRequests",LocalDefaultText = @"רשימת בקשות פתוחות", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOClosedRequest = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.ClosedRequest", DefaultText = "Closed SIIRequests",LocalDefaultText = @"רשימת בקשות סגורות", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOAllRequest = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.AllRequest", DefaultText = "All SIIRequests",LocalDefaultText = @"הכל ", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOGeneralInfo = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.GeneralInfo", DefaultText = "General Info",LocalDefaultText = @"נתונים כללים", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOSivug = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.Sivug", DefaultText = "Sivug",LocalDefaultText = @"סיווגים", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestODocuments = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.Documents", DefaultText = "Documents",LocalDefaultText = @"צרופות", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestORequiredCertificates = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.RequiredCertificates", DefaultText = "Required Certificates",LocalDefaultText = @"אישורים נדרשים", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestORequiredStatus = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.RequiredStatus", DefaultText = "Required Status",LocalDefaultText = @"סטטוס דרישה", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOLevel = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.Level", DefaultText = "Level",LocalDefaultText = @"רמה", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestODeclaration = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.Declaration", DefaultText = "Declaration",LocalDefaultText = @"הצהרה", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOSupplierInvoice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.SupplierInvoice", DefaultText = "SupplierInvoice",LocalDefaultText = @"חשבון", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOSivugSearchFields = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.SivugSearchFields", DefaultText = "SearchFields",LocalDefaultText = @"פרט מכס/מספר פריט", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOInvoiceNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.InvoiceNumber", DefaultText = "Invoice Number",LocalDefaultText = @"מספר חשבון", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOLineNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.LineNumber", DefaultText = "LineNumber",LocalDefaultText = @"שורה", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOItemCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.ItemCode", DefaultText = "ItemCode ",LocalDefaultText = @"קוד פריט", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOItemDescription = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.ItemDescription", DefaultText = "Item Description",LocalDefaultText = @"תאור", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOClassificationCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.ClassificationCode", DefaultText = "Classification Code",LocalDefaultText = @"פרט מכס", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOTradeAgreementCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.TradeAgreementCode", DefaultText = "Trade Agreement Code",LocalDefaultText = @"קוד הסכם", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOInvoiceQuantityType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.InvoiceQuantityType", DefaultText = "Quantity Type",LocalDefaultText = @"יח' מידה", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOInvoiceQuantity = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.InvoiceQuantity", DefaultText = "Quantity",LocalDefaultText = @"כמות", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOItemPrice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.ItemPrice", DefaultText = "Item Price",LocalDefaultText = @"ערך סחורה", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOItemPriceCurrencyCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.ItemPriceCurrencyCode", DefaultText = "Item Price Currency Code",LocalDefaultText = @"מטבע", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOOriginCountryCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.OriginCountryCode", DefaultText = "Origin Country",LocalDefaultText = @"ארץ מקור", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOConfirm = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.Confirm", DefaultText = "Confirm",LocalDefaultText = @"אישור", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOCancel = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.Cancel", DefaultText = "Cancel",LocalDefaultText = @"ביטול", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRquestOErrorNoItems = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRquest.O.ErrorNoItems", DefaultText = "Error in getting supplier invoice items for SII request",LocalDefaultText = @"שגיאה בקבלת פריטי חשבונית ספק עבור בקשת SII", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOMissingCommDef = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.MissingCommDef", DefaultText = "Message (%name) – Communication definition not found",LocalDefaultText = @"מסר (%name) - לא נמצא הגדרת תקשורת", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOMissingServiceUrl = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.MissingServiceUrl", DefaultText = "Required field message (%name) – Service URL",LocalDefaultText = @"הינו שדה חובה מסר (%name) - כתובת השירות", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOMissingUsername = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.MissingUsername", DefaultText = "Required field (%name) – Username",LocalDefaultText = @"הינו שדה חובה (%name) - שם משתמש", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOMissingPassword = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.MissingPassword", DefaultText = "Required field (%name) – Password",LocalDefaultText = @"הינו שדה חובה (%name) - סיסמא", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOUnSavedChanges = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.UnSavedChanges", DefaultText = "there is unsaved changes",LocalDefaultText = @"בנתוני הבקשה הנוכחית בוצעו שינויים שלא נשמרו, האם ברצונך לשמור אותם?", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOSendingRequest = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.SendingRequest", DefaultText = "Sending Request",LocalDefaultText = @"שולח בקשה למכון תקנים", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestONoRowSelected = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.NoRowSelected", DefaultText = "Please select at least one row to send the request.",LocalDefaultText = @"יש לבחור לפחות שורה אחת לשליחה.", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestORequest = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.Request", DefaultText = "Request",LocalDefaultText = @"בקשה", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestORequiredFields = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.RequiredFields", DefaultText = "Required Fields",LocalDefaultText = @"שדות חובה במסר שליחה למכון תקנים", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOformApplicationId = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.formApplicationId", DefaultText = "Form Application Id",LocalDefaultText = @"מספר פנייה", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOimporterNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.importerNumber", DefaultText = "Importer Number",LocalDefaultText = @"מספר יבואן", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOimporterEmail = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.importerEmail", DefaultText = "Importer Email",LocalDefaultText = @"אימייל יבואן", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOapplicantIdNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.applicantIdNumber", DefaultText = "Applicant Id Number",LocalDefaultText = @"ת.ז של שולח הבקשה", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOapplicantFullName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.applicantFullName", DefaultText = "Applicant Full Name",LocalDefaultText = @"שם מלא של שולח הבקשה", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOcustomsAgentRegisteredNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.customsAgentRegisteredNumber", DefaultText = "Customs Agent Registered Number",LocalDefaultText = @"ת.ז/ח.פ של סוכן מכס", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOcustomsAgentName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.customsAgentName", DefaultText = "Customs Agent Name",LocalDefaultText = @"שם עמילות המכס", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOdeliveryArrivalDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.deliveryArrivalDate", DefaultText = "Delivery Arrival Date",LocalDefaultText = @"תאריך הגעה", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestObillOfLadingId = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.billOfLadingId", DefaultText = "Bill Of Lading Id",LocalDefaultText = @"מספר שטר מטען", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOformAttachmentIndex = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.formAttachmentIndex", DefaultText = "Form Attachment Index",LocalDefaultText = @"מזהה צרופת שטר מטען", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOimportCountry = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.importCountry", DefaultText = "Import Country",LocalDefaultText = @"ארץ יבוא", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOdestinationPort = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.destinationPort", DefaultText = "Destination Port",LocalDefaultText = @"נמל יעד", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOwarehouseLocationName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.warehouseLocationName", DefaultText = "Warehouse Location Name",LocalDefaultText = @"כתובת אחסון הטובין לאחר השחרור - רחוב/מיקום", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOwarehouseSettlement = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.warehouseSettlement", DefaultText = "Warehouse Settlement",LocalDefaultText = @"ישוב מיקום אחסון", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOcontactPersonFirstName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.contactPersonFirstName", DefaultText = "Contact Person First Name",LocalDefaultText = @"שם פרטי איש קשר", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOcontactPersonLastName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.contactPersonLastName", DefaultText = "Contact Person Last Name",LocalDefaultText = @"שם משפחה איש קשר", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOcontactPersonPhone = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.contactPersonPhone", DefaultText = "Contact Person Phone",LocalDefaultText = @"טלפון איש קשר", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOcontactPersonEmail = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.contactPersonEmail", DefaultText = "Contact Person Email",LocalDefaultText = @"אימייל איש קשר", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOisNumericCountryCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.isNumericCountryCode", DefaultText = "Is Numeric Country Code",LocalDefaultText = @"האם קוד נומרי לארצות", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOlineSerialNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.lineSerialNumber", DefaultText = "Line Serial Number",LocalDefaultText = @"מספר סידורי של השורה", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOcustomsItem = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.customsItem", DefaultText = "Customs Item",LocalDefaultText = @"פרט מכס", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOoriginCountry = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.originCountry", DefaultText = "Origin Country",LocalDefaultText = @"ארץ יצור", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOmodelCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.modelCode", DefaultText = "Model Code",LocalDefaultText = @"קוד דגם", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOmodelDescription = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.modelDescription", DefaultText = "Model Description",LocalDefaultText = @"תאור הדגם", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOsupplierInvoiceNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.supplierInvoiceNumber", DefaultText = "Supplier Invoice Number",LocalDefaultText = @"מספר חשבון ספק", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOsupplierInvoiceDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.supplierInvoiceDate", DefaultText = "Supplier Invoice Date",LocalDefaultText = @"תאריך חשבון ספק", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOformAttachmentIndexes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.formAttachmentIndexes", DefaultText = "Form Attachment Indexes",LocalDefaultText = @"מזההי צרופות המקושרים לשורה", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOisDutchGroup1Requested = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.isDutchGroup1Requested", DefaultText = "Is Dutch Group1 Requested",LocalDefaultText = @"האם מבוקש החמרה לקבוצה 1", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOproductFileNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.productFileNumber", DefaultText = "Product File Number",LocalDefaultText = "מספר תיק מוצר במת''י", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOquantityToRelease = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.quantityToRelease", DefaultText = "Quantity To Release",LocalDefaultText = @"כמות במשלוח", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOsiiUnitCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.siiUnitCode", DefaultText = "SII Unit Code",LocalDefaultText = @"יחידת מידה מת''י", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOproductCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.productCode", DefaultText = "Product Code",LocalDefaultText = @"קוד מוצר", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOLine = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.Line", DefaultText = "Line",LocalDefaultText = @"שורה", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOAttachment = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.Attachment", DefaultText = "Attachment",LocalDefaultText = @"צרופה", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestODataNotCompleted = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.DataNotCompleted", DefaultText = "Request data not completed",LocalDefaultText = @"לא הושלמו נתוני בקשה", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOquantityByDeclaredUnit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.quantityByDeclaredUnit", DefaultText = "quantity By Declared Unit",LocalDefaultText = @"כמות במשלוח לפי יחידה מוצהרת", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOdeclaredUnitCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.declaredUnitCode", DefaultText = "declared Unit Code",LocalDefaultText = @"יחידת מידה מוצהרת", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode SIIRequestTextCode_CustomsSIIRequestOFileTypeNotAllowed = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SIIRequest.O.FileTypeNotAllowed", DefaultText = "File type {0} cannot be sent to SII. Allowed: {1}.",LocalDefaultText = @"קובץ מסוג {0} אינו מותר לשליחה למכון התקנים. סוגים מותרים: {1}.", ObjectTableId = SIIRequestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 