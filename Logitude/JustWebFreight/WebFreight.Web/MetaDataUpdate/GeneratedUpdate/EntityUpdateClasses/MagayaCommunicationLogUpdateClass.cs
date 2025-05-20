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
   public class MagayaCommunicationLogUpdateClass
   {  		
		public const string HashString = "b2be41ee0897b24874cd47663aa50e57";
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "MagayaCommunicationLog",
			      				    IsNew =  true,
			      				    DBTableName =  "MagayaCommunicationLogs",
			      				    ObjectTableSingular =  "MagayaCommunicationLog",
			      				    ObjectTablePlural =  "MagayaCommunicationLogs",
			      				    DescriptionDefaultText =  "Magaya Communication Logs",
			      				    HasCustomFilter =  true,
			      				    HasCustomFields =  false,
			      				    AvailableInCustomization =  true,
			      				    SupportSubEntity =  false,
			      				    ApplyGenericCustomFields =  false,
			      				    AvailableInDocumentTypes =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  true,
			      				    IsNewWizard =  false,
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
			      				    SortingByObjectField =  "CreateDate",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    DefaultText =  "MagayaCommunicationLog",
			      				    Code =  "e2e4",
			      				    Name =  " Query Group",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Accounting",
			      				    NoTS =  false,
			      				    HasMenuButtons =  true,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    IsTabsHidden =  false,
			      				    HashString =  MagayaCommunicationLogUpdateClass.HashString,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository, List<ObjectField> addedFields, List<TextCode> addedTextCodes)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Id",
					  						ObjectTableName =  "MagayaCommunicationLog",
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
					  						ValidForQuerySection1 =  "MagayaCommunicationLog",
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
					  						ObjectTableName =  "MagayaCommunicationLog",
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
					  						ValidForQuerySection1 =  "MagayaCommunicationLog",
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
					 
					 						FieldName =  "CreateDate",
					  						ObjectTableName =  "MagayaCommunicationLog",
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
					  						IsListFilter =  true,
					  						Operator =  "LargerThan",
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
					  						ValidForQuerySection1 =  "MagayaCommunicationLog",
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
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
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
					  						ObjectTableName =  "MagayaCommunicationLog",
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
					  						ValidForQuerySection1 =  "MagayaCommunicationLog",
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
					 
					 						FieldName =  "CommunicationId",
					  						ObjectTableName =  "MagayaCommunicationLog",
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
					  						PMPropertyPath =  "CommunicationId",
					  						ListPropertyPath =  "CommunicationId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "MagayaCommunicationLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CommunicationId",
					  						DefaultText =  "CommunicationId",
					  						FullLocalDefaultText =  "CommunicationId",
					  						ListFieldLable =  "CommunicationIdListLable",
					  						ListLableDefaultText =  "CommunicationId",
					  						ListLocalDefaultText =  "CommunicationId",
					  						IsForeignKey =  true,
					  						ForeignEntity =  "CommunicationLog",
					  						NavigationPropertyName =  "CommunicationLog",
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
					 
					 						FieldName =  "Step",
					  						ObjectTableName =  "MagayaCommunicationLog",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  3,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  3,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Step",
					  						ListPropertyPath =  "Step",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "MagayaCommunicationLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Step",
					  						DefaultText =  "Step",
					  						FullLocalDefaultText =  "שלב",
					  						ListFieldLable =  "StepListLable",
					  						ListLableDefaultText =  "Step",
					  						ListLocalDefaultText =  "שלב",
					  						IsForeignKey =  true,
					  						ForeignEntity =  "MagayaStep",
					  						NavigationPropertyName =  "MagayaStep",
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
					 
					 						FieldName =  "StatusCode",
					  						ObjectTableName =  "MagayaCommunicationLog",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  3,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  3,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
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
					  						ValidForQuerySection1 =  "MagayaCommunicationLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "StatusCode",
					  						DefaultText =  "StatusCode",
					  						FullLocalDefaultText =  "סטטוס",
					  						ListFieldLable =  "StatusCodeListLable",
					  						ListLableDefaultText =  "StatusCode",
					  						ListLocalDefaultText =  "סטטוס",
					  						IsForeignKey =  true,
					  						ForeignEntity =  "MagayaStatus",
					  						NavigationPropertyName =  "MagayaStatus",
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
					 
					 						FieldName =  "Exception",
					  						ObjectTableName =  "MagayaCommunicationLog",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Exception",
					  						ListPropertyPath =  "Exception",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "MagayaCommunicationLog",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Exception",
					  						DefaultText =  "Exception",
					  						FullLocalDefaultText =  "שגיאה",
					  						ListFieldLable =  "ExceptionListLable",
					  						ListLableDefaultText =  "Exception",
					  						ListLocalDefaultText =  "שגיאה",
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
	        //FeatureRepository featureRepository = new FeatureRepository(0); 
            //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
	        QueryGroup MagayaCommunicationLogQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "e2e4", Name = " Query Group" }, queryGroupRepository,tenantQueryGroups);
						QueryGroup MagayaCommunicationLogQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "35e0", Name = " Query Group" }, queryGroupRepository,tenantQueryGroups);
				        queryGroupRepository.SubmitChanges();
	        ObjectTable MagayaCommunicationLogObjectTable = objectTables.ContainsKey("MagayaCommunicationLog") ? objectTables["MagayaCommunicationLog"] : null;
            if (MagayaCommunicationLogObjectTable == null)
            {
                IWebFreightContext objectContext = WebFreightContext.GetContext(0);  

                MagayaCommunicationLogObjectTable = objectContext.ObjectTables.Where(d => d.Name == "MagayaCommunicationLog" && d.Tenant == 0).FirstOrDefault();
            }

	         
			List<Feature> addedFeatures = new List<Feature>();
			List<TextCode> addedTextCodes = new List<TextCode>();
			List<Query> addedQueries = new List<Query>();
			List<QueryColumn> addedQueryColumns = new List<QueryColumn>();
			List<AdvancedQueryFilter> addedQueryFilters = new List<AdvancedQueryFilter>();
   

			   TextCode MagayaCommunicationLogTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "MagayaCommunicationLog.Q.TodayMagayaCommunicationLog", DefaultText = @"Today",LocalDefaultText = null, ObjectTableId = MagayaCommunicationLogObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature MagayaCommunicationLogFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MagayaCommunicationLog.Q.TodayMagayaCommunicationLog", ObjectTableId = MagayaCommunicationLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "MagayaCommunicationLogFeatures.TodayMagayaCommunicationLog", NameTextCodeDefaultText = "Today Magaya Communication Log", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,MagayaCommunicationLogObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode MagayaCommunicationLogTextCode_1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "MagayaCommunicationLog.Q.AllMagayaCommunicationLog", DefaultText = @"All Magaya Communication Log",LocalDefaultText = null, ObjectTableId = MagayaCommunicationLogObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature MagayaCommunicationLogFeature_1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MagayaCommunicationLog.Q.AllMagayaCommunicationLog", ObjectTableId = MagayaCommunicationLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "MagayaCommunicationLogFeatures.AllMagayaCommunicationLog", NameTextCodeDefaultText = "All Magaya Communication Log", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,MagayaCommunicationLogObjectTable, addedFeatures, addedTextCodes);

	        //TextCodeRepository.SubmitChanges();
	        //FeaturesRepository.SubmitChanges();    
	      

			  Query TodayMagayaCommunicationLogQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = MagayaCommunicationLogTextCode_0.Id, NameTextCodeCode = MagayaCommunicationLogTextCode_0.Code, ObjectTableName = "MagayaCommunicationLog", Code = "Today Magaya Communication Log",  QueryGroupCode = "e2e4", IndexOrder = 0, Tenant = 0, ObjectTableId = MagayaCommunicationLogObjectTable.Id, QuerySection = "MagayaCommunicationLog", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = MagayaCommunicationLogFeature_0.Id,FeatureUniqeCode= MagayaCommunicationLogFeature_0.FeatureUniqeCode, DefaultSortName = "CreateDate", DefaultSortDirection = "Desending", Perspective = null }, addedQueries);
	
			 QueryColumn TodayMagayaCommunicationLogQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayMagayaCommunicationLogQuery.Id,QueryCode = TodayMagayaCommunicationLogQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "MagayaCommunicationLog.CreateDate" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn TodayMagayaCommunicationLogQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayMagayaCommunicationLogQuery.Id,QueryCode = TodayMagayaCommunicationLogQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "MagayaCommunicationLog.CommunicationId" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn TodayMagayaCommunicationLogQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TodayMagayaCommunicationLogQuery.Id,QueryCode = TodayMagayaCommunicationLogQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "MagayaCommunicationLog.StatusCode" , ColumnWidth = 50 }, addedQueryColumns);

             AdvancedQueryFilter TodayMagayaCommunicationLogQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "MagayaCommunicationLog.CreateDate", PredefinedValue = "Today",PredefinedValue2 = null, CustomPredefined = false, QueryId = TodayMagayaCommunicationLogQuery.Id,QueryCode = TodayMagayaCommunicationLogQuery.UniqueCode, Tenant = 0}, addedQueryFilters);

  
	      

			  Query AllMagayaCommunicationLogQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = MagayaCommunicationLogTextCode_1.Id, NameTextCodeCode = MagayaCommunicationLogTextCode_1.Code, ObjectTableName = "MagayaCommunicationLog", Code = "All Magaya Communication Log",  QueryGroupCode = "e2e4", IndexOrder = 1, Tenant = 0, ObjectTableId = MagayaCommunicationLogObjectTable.Id, QuerySection = "MagayaCommunicationLog", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = MagayaCommunicationLogFeature_1.Id,FeatureUniqeCode= MagayaCommunicationLogFeature_1.FeatureUniqeCode, DefaultSortName = "CreateDate", DefaultSortDirection = "Desending", Perspective = null }, addedQueries);
				SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
			SqlBulkInsert.BulkInsert("Features", addedFeatures);
			SqlBulkInsert.BulkInsert("Queries", addedQueries);
			SqlBulkInsert.BulkInsert("QueryColumns", addedQueryColumns);
			SqlBulkInsert.BulkInsert("AdvancedQueryFilters", addedQueryFilters);	 
  
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable MagayaCommunicationLogObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "MagayaCommunicationLog" && d.Tenant == 0).FirstOrDefault();
		   //List<ObjectField> MagayaCommunicationLogObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "MagayaCommunicationLog").ToList();
		       
	      

	         Screen MagayaCommunicationLogMagayaCommunicationLogHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "MagayaCommunicationLog.HeaderScreen", Name = "MagayaCommunicationLogHeaderScreen", ObjectTableId = MagayaCommunicationLogObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      	
		    MagayaCommunicationLogObjectTable.HeaderScreenId = MagayaCommunicationLogMagayaCommunicationLogHeaderScreenScreen0.Id;
		    MagayaCommunicationLogObjectTable.HeaderScreenCode = MagayaCommunicationLogMagayaCommunicationLogHeaderScreenScreen0.Code;

	   		  

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable MagayaCommunicationLogObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "MagayaCommunicationLog" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode MagayaCommunicationLogDetailsTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "MagayaCommunicationLog.TH.Details", DefaultText = "Details",LocalDefaultText = "פרטים", ObjectTableId = MagayaCommunicationLogObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature MagayaCommunicationLogDetailsFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MagayaCommunicationLog.Tab.Details", ObjectTableId = MagayaCommunicationLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "MagayaCommunicationLogFeatures.MGGT", NameTextCodeDefaultText = "Details", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,MagayaCommunicationLogObjectTable);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = ""MGGT"",HtmlComponentName = "MagayaCommunicationLogDetailsTabComponent",HtmlComponentUrl = "./Accounting/Components/EditTabs/Magaya/MagayaCommunicationLogDetailsTabComponent", FeatureId = MagayaCommunicationLogDetailsFeature_TH0.Id,FeatureUniqeCode = MagayaCommunicationLogDetailsFeature_TH0.FeatureUniqeCode, ControlPath = "./Accounting/Components/EditTabs/Magaya/MagayaCommunicationLogDetailsTabComponent", ObjectTableId = MagayaCommunicationLogObjectTable.Id, TabNameTextCodeId = MagayaCommunicationLogDetailsTextCode_TH0.Id, TabNameTextCodeCode = MagayaCommunicationLogDetailsTextCode_TH0.Code, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable MagayaCommunicationLogObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "MagayaCommunicationLog" && d.Tenant == 0).FirstOrDefault(); 

		   Feature MagayaCommunicationLogFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = MagayaCommunicationLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "MagayaCommunicationLog.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,MagayaCommunicationLogObjectTable);
		   Feature MagayaCommunicationLogFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = MagayaCommunicationLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "MagayaCommunicationLog.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,MagayaCommunicationLogObjectTable);
		   Feature MagayaCommunicationLogFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = MagayaCommunicationLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "MagayaCommunicationLog.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,MagayaCommunicationLogObjectTable);
		   Feature MagayaCommunicationLogFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = false, ObjectTableId = MagayaCommunicationLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "MagayaCommunicationLog.Features.PackageFeature", NameTextCodeDefaultText = "MagayaCommunicationLog Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,MagayaCommunicationLogObjectTable); 

		   		   //--------------> Additional Features <--------------\\

		   Feature MagayaCommunicationLogFeature_MagayaCommunicationLogMenu = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MagayaCommunicationLogMenu", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = MagayaCommunicationLogObjectTable.Id, Tenant = 0, NameTextCodeCode = "MagayaCommunicationLog.Features.MagayaCommunicationLogMenu", NameTextCodeDefaultText = @"Magaya Communication Log Menu" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,MagayaCommunicationLogObjectTable);

   
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable MagayaCommunicationLogObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "MagayaCommunicationLog" && d.Tenant == 0).FirstOrDefault(); 
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
                ObjectTableId = MagayaCommunicationLogObjectTable.Id,
				 
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
                ObjectTableId = MagayaCommunicationLogObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable MagayaCommunicationLogObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "MagayaCommunicationLog" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode MagayaCommunicationLogTextCode_GeneralMCACMagayaCommunicationLog = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.AC.MagayaCommunicationLog", DefaultText = "Magaya Communication Log",LocalDefaultText = @"Magaya Communication Log", ObjectTableId = MagayaCommunicationLogObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 