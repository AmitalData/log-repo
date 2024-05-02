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

//using Amital.QuoteOPM.BL.CLoseTable;





namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.CommonDataModel.EntityUpdateClasses
{
   public class AgentSharedManifestUpdateClass
   {  		
		public const string HashString = "d8fb36249d5f7ed003a429edf29ca9dd";
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "AgentSharedManifest",
			      				    IsNew =  false,
			      				    DBTableName =  "AgentSharedManifests",
			      				    ObjectTableSingular =  "Agent Shared Manifest",
			      				    ObjectTablePlural =  "Agent Shared Manifests",
			      				    HasCustomFilter =  false,
			      				    HasCustomFields =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  false,
			      				    IsNewWizard =  false,
			      				    KeyPropertyPath =  "Id",
			      				    AutoCompleteSearchWindow =  true,
			      				    IsClosed =  false,
			      				    CacheOnClient =  false,
			      				    EditableFromAutoCompleteWindow =  true,
			      				    HasCounter =  false,
			      				    EnableAddFromLOV =  false,
			      				    IsRestrictable =  false,
			      				    IsMain =  true,
			      				    IsAutoComplete =  true,
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
			      				    DefaultText =  "Agent Shared Manifest",
			      				    Code =  "CASM",
			      				    Name =  "Agent Shared Manifests",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Common",
			      				    NoTS =  false,
			      				    HasMenuButtons =  false,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    IsTabsHidden =  false,
			      				    SearchFields =  "AgentSharedManifest,AgentSharedManifests,,Id,CreateDate",
			      				    HashString =  AgentSharedManifestUpdateClass.HashString,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository, List<ObjectField> addedFields, List<TextCode> addedTextCodes)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Id",
					  						ObjectTableName =  "AgentSharedManifest",
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
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
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
					  						ValidForQuerySection1 =  "AgentSharedManifest",
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
					  						FullLocalDefaultText =  "Id",
					  						ListFieldLable =  "IdListLable",
					  						ListLableDefaultText =  "Id",
					  						ListLocalDefaultText =  "Id",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  true,
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
					 
					 						FieldName =  "Tenant",
					  						ObjectTableName =  "AgentSharedManifest",
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
					  						ValidForQuerySection1 =  "AgentSharedManifest",
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
					  						FullLocalDefaultText =  "Tenant",
					  						ListFieldLable =  "TenantListLable",
					  						ListLableDefaultText =  "Tenant",
					  						ListLocalDefaultText =  "Tenant",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  true,
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
					 
					 						FieldName =  "ManifestXML",
					  						ObjectTableName =  "AgentSharedManifest",
					  						FieldsDataType =  "nText",
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
					  						PMPropertyPath =  "ManifestXML",
					  						ListPropertyPath =  "ManifestXML",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ManifestXML",
					  						DefaultText =  "ManifestXML",
					  						FullLocalDefaultText =  "ManifestXML",
					  						ListFieldLable =  "ManifestXMLListLable",
					  						ListLableDefaultText =  "ManifestXML",
					  						ListLocalDefaultText =  "ManifestXML",
					  						IsForeignKey =  false,
					  						IsMaxLength =  true,
					  						NoMetaDataField =  true,
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
					 
					 						FieldName =  "SearchFields",
					  						ObjectTableName =  "AgentSharedManifest",
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
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
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
					  						ValidForQuerySection1 =  "AgentSharedManifest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SearchFields",
					  						DefaultText =  "SearchFields",
					  						FullLocalDefaultText =  "SearchFields",
					  						ListFieldLable =  "SearchFieldsListLable",
					  						ListLableDefaultText =  "SearchFields",
					  						ListLocalDefaultText =  "SearchFields",
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
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ShipmentTypeId",
					  						ObjectTableName =  "AgentSharedManifest",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  4,
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
					  						PMPropertyPath =  "ShipmentTypeId",
					  						ListPropertyPath =  "ShipmentTypeId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ShipmentTypeId",
					  						DefaultText =  "ShipmentTypeId",
					  						FullLocalDefaultText =  "ShipmentTypeId",
					  						ListFieldLable =  "ShipmentTypeIdListLable",
					  						ListLableDefaultText =  "ShipmentTypeId",
					  						ListLocalDefaultText =  "ShipmentTypeId",
					  						IsForeignKey =  true,
					  						ForeignEntity =  "ShipmentType",
					  						NavigationPropertyName =  "ShipmentType",
					  						IsMaxLength =  false,
					  						NoMetaDataField =  true,
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
					 
					 						FieldName =  "ChargeableWeight",
					  						ObjectTableName =  "AgentSharedManifest",
					  						FieldsDataType =  "Double",
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
					  						PMPropertyPath =  "ChargeableWeight",
					  						ListPropertyPath =  "ChargeableWeight",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ChargeableWeight",
					  						DefaultText =  "ChargeableWeight",
					  						FullLocalDefaultText =  "ChargeableWeight",
					  						ListFieldLable =  "ChargeableWeightListLable",
					  						ListLableDefaultText =  "ChargeableWeight",
					  						ListLocalDefaultText =  "ChargeableWeight",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  true,
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
					 
					 						FieldName =  "AgentId",
					  						ObjectTableName =  "AgentSharedManifest",
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
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AgentId",
					  						ListPropertyPath =  "AgentId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AgentId",
					  						DefaultText =  "AgentId",
					  						FullLocalDefaultText =  "AgentId",
					  						ListFieldLable =  "AgentIdListLable",
					  						ListLableDefaultText =  "AgentId",
					  						ListLocalDefaultText =  "AgentId",
					  						IsForeignKey =  true,
					  						ForeignEntity =  "Agent",
					  						NavigationPropertyName =  "Agent",
					  						IsMaxLength =  false,
					  						NoMetaDataField =  true,
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
					 
					 						FieldName =  "FromPortId",
					  						ObjectTableName =  "AgentSharedManifest",
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
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "FromPortId",
					  						ListPropertyPath =  "FromPortId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "FromPortId",
					  						DefaultText =  "FromPortId",
					  						FullLocalDefaultText =  "FromPortId",
					  						ListFieldLable =  "FromPortIdListLable",
					  						ListLableDefaultText =  "FromPortId",
					  						ListLocalDefaultText =  "FromPortId",
					  						IsForeignKey =  true,
					  						ForeignEntity =  "Port",
					  						NavigationPropertyName =  "FromPort",
					  						IsMaxLength =  false,
					  						NoMetaDataField =  true,
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
					 
					 						FieldName =  "ToPortId",
					  						ObjectTableName =  "AgentSharedManifest",
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
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ToPortId",
					  						ListPropertyPath =  "ToPortId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ToPortId",
					  						DefaultText =  "ToPortId",
					  						FullLocalDefaultText =  "ToPortId",
					  						ListFieldLable =  "ToPortIdListLable",
					  						ListLableDefaultText =  "ToPortId",
					  						ListLocalDefaultText =  "ToPortId",
					  						IsForeignKey =  true,
					  						ForeignEntity =  "Port",
					  						NavigationPropertyName =  "ToPort",
					  						IsMaxLength =  false,
					  						NoMetaDataField =  true,
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
					 
					 						FieldName =  "CancelledBySenderAgent",
					  						ObjectTableName =  "AgentSharedManifest",
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
					  						PMPropertyPath =  "CancelledBySenderAgent",
					  						ListPropertyPath =  "CancelledBySenderAgent",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CancelledBySenderAgent",
					  						DefaultText =  "CancelledBySenderAgent",
					  						FullLocalDefaultText =  "CancelledBySenderAgent",
					  						ListFieldLable =  "CancelledBySenderAgentListLable",
					  						ListLableDefaultText =  "CancelledBySenderAgent",
					  						ListLocalDefaultText =  "CancelledBySenderAgent",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  true,
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
					 
					 						FieldName =  "Master",
					  						ObjectTableName =  "AgentSharedManifest",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  20,
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Master",
					  						ListPropertyPath =  "Master",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "Master",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Master",
					  						DefaultText =  "Master",
					  						ListFieldLable =  "MasterListLable",
					  						ListLableDefaultText =  "Master",
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
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Master",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AgentReference",
					  						ObjectTableName =  "AgentSharedManifest",
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
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AgentReference",
					  						ListPropertyPath =  "AgentReference",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "AgentReference",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AgentReference",
					  						DefaultText =  "Agent Ref.",
					  						ListFieldLable =  "AgentReferenceListLable",
					  						ListLableDefaultText =  "Agent Ref.",
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
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "AgentReference",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreateDate",
					  						ObjectTableName =  "AgentSharedManifest",
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
					  						ValidForQuerySection1 =  "AgentSharedManifest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "CreateDate",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CreateDate",
					  						DefaultText =  "Create Date",
					  						ListFieldLable =  "CreateDateListLable",
					  						ListLableDefaultText =  "Create Date",
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
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						HelpTextCode =  "CreateDate",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdateDate",
					  						ObjectTableName =  "AgentSharedManifest",
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
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
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
					  						ValidForQuerySection1 =  "AgentSharedManifest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "UpdateDate",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UpdateDate",
					  						DefaultText =  "Update Date",
					  						ListFieldLable =  "UpdateDateListLable",
					  						ListLableDefaultText =  "Update Date",
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
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						HelpTextCode =  "UpdateDate",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdatedByUserId",
					  						ObjectTableName =  "AgentSharedManifest",
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
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
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
					  						ValidForQuerySection1 =  "AgentSharedManifest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "UpdatedByUserId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UpdatedByUserId",
					  						DefaultText =  "Updated By",
					  						IsForeignKey =  true,
					  						ForeignEntity =  "User",
					  						NavigationPropertyName =  "UpdatedByUser",
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "UpdatedByUserId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DirectionId",
					  						ObjectTableName =  "AgentSharedManifest",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  1,
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DirectionId",
					  						ListPropertyPath =  "DirectionId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "DirectionId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						ColumnHeaderTemplateName =  "DirectionHeaderTemplate",
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DirectionId",
					  						DefaultText =  "Direction",
					  						ListFieldLable =  "DirectionIdListLable",
					  						ListLableDefaultText =  "Direction",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  true,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HtmlListComponentName =  "DirectionCellDisplayListTemplate",
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						HelpTextCode =  "DirectionId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TransportModeId",
					  						ObjectTableName =  "AgentSharedManifest",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  1,
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TransportModeId",
					  						ListPropertyPath =  "TransportModeId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "TransportModeId",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						ColumnHeaderTemplateName =  "TransportModeHeaderTemplate",
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TransportModeId",
					  						DefaultText =  "Transport Mode",
					  						ListFieldLable =  "TransportModeIdListLable",
					  						ListLableDefaultText =  "Transport Mode",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  true,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						HelpTextCode =  "TransportModeId",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PackagesQuantity",
					  						ObjectTableName =  "AgentSharedManifest",
					  						FieldsDataType =  "Integer",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PackagesQuantity",
					  						ListPropertyPath =  "PackagesQuantity",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "PackagesQuantity",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PackagesQuantity",
					  						DefaultText =  "QTY",
					  						ListFieldLable =  "PackagesQuantityListLable",
					  						ListLableDefaultText =  "QTY",
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
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "PackagesQuantity",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "StatusName",
					  						ObjectTableName =  "AgentSharedManifest",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  100,
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
					  						ValidForQuerySection1 =  "AgentSharedManifest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "StatusName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "StatusName",
					  						DefaultText =  "Status",
					  						ListFieldLable =  "StatusNameListLable",
					  						ListLableDefaultText =  "Status",
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
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						HelpTextCode =  "StatusName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ShipmentlevelCode",
					  						ObjectTableName =  "AgentSharedManifest",
					  						FieldsDataType =  "Text",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ShipmentlevelCode",
					  						ListPropertyPath =  "ShipmentlevelCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ShipmentlevelCode",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ShipmentlevelCode",
					  						DefaultText =  "Shipment level",
					  						ListFieldLable =  "ShipmentlevelCodeListLable",
					  						ListLableDefaultText =  "Shipment level",
					  						IsForeignKey =  true,
					  						ForeignEntity =  "ShipmentLevel",
					  						NavigationPropertyName =  "ShipmentLevel",
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "ShipmentlevelCode",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AgentName",
					  						ObjectTableName =  "AgentSharedManifest",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  100,
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AgentName",
					  						ListPropertyPath =  "AgentName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "AgentName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AgentName",
					  						DefaultText =  "Agent",
					  						ListFieldLable =  "AgentNameListLable",
					  						ListLableDefaultText =  "Agent",
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
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "AgentName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Routing",
					  						ObjectTableName =  "AgentSharedManifest",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  100,
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Routing",
					  						ListPropertyPath =  "Routing",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "Routing",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Routing",
					  						DefaultText =  "Routing",
					  						ListFieldLable =  "RoutingListLable",
					  						ListLableDefaultText =  "Routing",
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
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Routing",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "StatusCode",
					  						ObjectTableName =  "AgentSharedManifest",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  4,
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
					  						Operator =  "StartsWith",
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
					  						ValidForQuerySection1 =  "AgentSharedManifest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "StatusCode",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "StatusCode",
					  						DefaultText =  "Status Code",
					  						ListFieldLable =  "StatusCodeListLable",
					  						ListLableDefaultText =  "Status Code",
					  						IsForeignKey =  true,
					  						ForeignEntity =  "SharedManifestsStatus",
					  						NavigationPropertyName =  "SharedManifestsStatus",
					  						IsMaxLength =  false,
					  						NoMetaDataField =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "StatusCode",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "GrossWeight",
					  						ObjectTableName =  "AgentSharedManifest",
					  						FieldsDataType =  "SigDouble",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "GrossWeight",
					  						ListPropertyPath =  "GrossWeight",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "GrossWeight",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "GrossWeight",
					  						DefaultText =  "Gross Weight",
					  						ListFieldLable =  "GrossWeightListLable",
					  						ListLableDefaultText =  "Gross Weight",
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
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "GrossWeight",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TEU",
					  						ObjectTableName =  "AgentSharedManifest",
					  						FieldsDataType =  "Double",
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
					  						PMPropertyPath =  "TEU",
					  						ListPropertyPath =  "TEU",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "TEU",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  2,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TEU",
					  						DefaultText =  "TEU",
					  						ListFieldLable =  "TEUListLable",
					  						ListLableDefaultText =  "TEU",
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
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "TEU",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ShipmentLevelName",
					  						ObjectTableName =  "AgentSharedManifest",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  100,
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ShipmentLevelName",
					  						ListPropertyPath =  "ShipmentLevelName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ShipmentLevelName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ShipmentLevelName",
					  						DefaultText =  "Type",
					  						ListFieldLable =  "ShipmentLevelNameListLable",
					  						ListLableDefaultText =  "Type",
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
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "ShipmentLevelName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ManifestSL",
					  						ObjectTableName =  "AgentSharedManifest",
					  						FieldsDataType =  "Text",
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
					  						PMPropertyPath =  "ManifestSL",
					  						ListPropertyPath =  "ManifestSL",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ManifestSL",
					  						DefaultText =  "ManifestSL",
					  						FullLocalDefaultText =  "ManifestSL",
					  						ListFieldLable =  "ManifestSLListLable",
					  						ListLableDefaultText =  "ManifestSL",
					  						ListLocalDefaultText =  "ManifestSL",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  true,
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
					 
					 						FieldName =  "TransportModeName",
					  						ObjectTableName =  "AgentSharedManifest",
					  						FieldsDataType =  "Text",
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
					  						PMPropertyPath =  "TransportModeName",
					  						ListPropertyPath =  "TransportModeName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TransportModeName",
					  						DefaultText =  "TransportModeName",
					  						FullLocalDefaultText =  "TransportModeName",
					  						ListFieldLable =  "TransportModeNameListLable",
					  						ListLableDefaultText =  "TransportModeName",
					  						ListLocalDefaultText =  "TransportModeName",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  true,
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
					 
					 						FieldName =  "SharedManifestTranslations",
					  						ObjectTableName =  "AgentSharedManifest",
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
					  						PMPropertyPath =  "SharedManifestTranslations",
					  						ListPropertyPath =  "SharedManifestTranslations",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						MultiTableName =  "SharedManifestTranslation",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SharedManifestTranslations",
					  						DefaultText =  "SharedManifestTranslations",
					  						FullLocalDefaultText =  "SharedManifestTranslations",
					  						ListFieldLable =  "SharedManifestTranslationsListLable",
					  						ListLableDefaultText =  "SharedManifestTranslations",
					  						ListLocalDefaultText =  "SharedManifestTranslations",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  true,
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
					 
					 						FieldName =  "DirectionName",
					  						ObjectTableName =  "AgentSharedManifest",
					  						FieldsDataType =  "Text",
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
					  						PMPropertyPath =  "DirectionName",
					  						ListPropertyPath =  "DirectionName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "AgentSharedManifest",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DirectionName",
					  						DefaultText =  "DirectionName",
					  						FullLocalDefaultText =  "DirectionName",
					  						ListFieldLable =  "DirectionNameListLable",
					  						ListLableDefaultText =  "DirectionName",
					  						ListLocalDefaultText =  "DirectionName",
					  						IsForeignKey =  false,
					  						IsMaxLength =  false,
					  						NoMetaDataField =  true,
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
	        QueryGroup AgentSharedManifestQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "CASM", Name = "Agent Shared Manifests" }, queryGroupRepository,tenantQueryGroups);
						QueryGroup AgentSharedManifestQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "AASM", Name = "Air Agent Shared Manifests" }, queryGroupRepository,tenantQueryGroups);
				        queryGroupRepository.SubmitChanges();
	        ObjectTable AgentSharedManifestObjectTable = objectTables.ContainsKey("AgentSharedManifest") ? objectTables["AgentSharedManifest"] : null;
            if (AgentSharedManifestObjectTable == null)
            {
                IWebFreightContext objectContext = WebFreightContext.GetContext(0);  

                AgentSharedManifestObjectTable = objectContext.ObjectTables.Where(d => d.Name == "AgentSharedManifest" && d.Tenant == 0).FirstOrDefault();
            }

	         
			List<Feature> addedFeatures = new List<Feature>();
			List<TextCode> addedTextCodes = new List<TextCode>();
			List<Query> addedQueries = new List<Query>();
			List<QueryColumn> addedQueryColumns = new List<QueryColumn>();
			List<AdvancedQueryFilter> addedQueryFilters = new List<AdvancedQueryFilter>();
   

			   TextCode AgentSharedManifestTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AgentSharedManifest.Q.AgentSharedManifests", DefaultText = @"All Agent Shared Manifests",LocalDefaultText = null, ObjectTableId = AgentSharedManifestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature AgentSharedManifestFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AgentSharedManifestQ", ObjectTableId = AgentSharedManifestObjectTable.Id, Tenant = 0, NameTextCodeCode = "AgentSharedManifest.Features.AgentSharedManifestS", NameTextCodeDefaultText = "All Agent Shared Manifests", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,AgentSharedManifestObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode AgentSharedManifestTextCode_1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AgentSharedManifest.Q.AirAgentSharedManifests", DefaultText = @"Air Agent Shared Manifests",LocalDefaultText = null, ObjectTableId = AgentSharedManifestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature AgentSharedManifestFeature_1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AirAgentSharedManifestsQ", ObjectTableId = AgentSharedManifestObjectTable.Id, Tenant = 0, NameTextCodeCode = "AgentSharedManifest.Features.AirAgentSharedManifests", NameTextCodeDefaultText = "Air Agent Shared Manifests", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,AgentSharedManifestObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode AgentSharedManifestTextCode_2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AgentSharedManifest.Q.OceanAgentSharedManifests", DefaultText = @"Ocean Agent Shared Manifests",LocalDefaultText = null, ObjectTableId = AgentSharedManifestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature AgentSharedManifestFeature_2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OceanAgentSharedManifestsQ", ObjectTableId = AgentSharedManifestObjectTable.Id, Tenant = 0, NameTextCodeCode = "AgentSharedManifest.Features.OceanAgentSharedManifests", NameTextCodeDefaultText = "Ocean Agent Shared Manifests", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,AgentSharedManifestObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode AgentSharedManifestTextCode_3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AgentSharedManifest.Q.InlandAgentSharedManifests", DefaultText = @"Inland Agent Shared Manifests",LocalDefaultText = null, ObjectTableId = AgentSharedManifestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature AgentSharedManifestFeature_3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "InlandAgentSharedManifestsQ", ObjectTableId = AgentSharedManifestObjectTable.Id, Tenant = 0, NameTextCodeCode = "AgentSharedManifest.Features.InlandAgentSharedManifests", NameTextCodeDefaultText = "Inland Agent Shared Manifests", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,AgentSharedManifestObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode AgentSharedManifestTextCode_4 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "AgentSharedManifest.Q.CancelledAgentSharedManifests", DefaultText = @"Cancelled Agent Shared Manifests",LocalDefaultText = null, ObjectTableId = AgentSharedManifestObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature AgentSharedManifestFeature_4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CancelledAgentSharedManifestsQ", ObjectTableId = AgentSharedManifestObjectTable.Id, Tenant = 0, NameTextCodeCode = "AgentSharedManifest.Features.CancelledAgentSharedManifests", NameTextCodeDefaultText = "Cancelled Agent Shared Manifests", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,AgentSharedManifestObjectTable, addedFeatures, addedTextCodes);

	        //TextCodeRepository.SubmitChanges();
	        //FeaturesRepository.SubmitChanges();    
	      

			  Query AgentSharedManifestsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = AgentSharedManifestTextCode_0.Id, NameTextCodeCode = AgentSharedManifestTextCode_0.Code, ObjectTableName = "AgentSharedManifest", Code = "Agent Shared Manifests",  EditWizardName = "./Common/Components/SharedManifest/SharedManifestComponent",
			   QueryGroupCode = "ASMN", IndexOrder = 0, Tenant = 0, ObjectTableId = AgentSharedManifestObjectTable.Id, QuerySection = "AgentSharedManifest", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = AgentSharedManifestFeature_0.Id,FeatureUniqeCode= AgentSharedManifestFeature_0.FeatureUniqeCode, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", Perspective = null }, addedQueries);
	
			 QueryColumn AgentSharedManifestsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AgentSharedManifestsQuery.Id,QueryCode = AgentSharedManifestsQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "AgentSharedManifest.DirectionId" , ColumnWidth = 25 }, addedQueryColumns);

			 QueryColumn AgentSharedManifestsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AgentSharedManifestsQuery.Id,QueryCode = AgentSharedManifestsQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "AgentSharedManifest.TransportModeId" , ColumnWidth = 25 }, addedQueryColumns);

			 QueryColumn AgentSharedManifestsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AgentSharedManifestsQuery.Id,QueryCode = AgentSharedManifestsQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "AgentSharedManifest.Master" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn AgentSharedManifestsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AgentSharedManifestsQuery.Id,QueryCode = AgentSharedManifestsQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "AgentSharedManifest.AgentName" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn AgentSharedManifestsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AgentSharedManifestsQuery.Id,QueryCode = AgentSharedManifestsQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "AgentSharedManifest.Routing" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn AgentSharedManifestsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AgentSharedManifestsQuery.Id,QueryCode = AgentSharedManifestsQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "AgentSharedManifest.AgentReference" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn AgentSharedManifestsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AgentSharedManifestsQuery.Id,QueryCode = AgentSharedManifestsQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "AgentSharedManifest.ShipmentLevelName" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn AgentSharedManifestsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AgentSharedManifestsQuery.Id,QueryCode = AgentSharedManifestsQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "AgentSharedManifest.GrossWeight" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn AgentSharedManifestsQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AgentSharedManifestsQuery.Id,QueryCode = AgentSharedManifestsQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "AgentSharedManifest.ShipmentlevelCode" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn AgentSharedManifestsQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AgentSharedManifestsQuery.Id,QueryCode = AgentSharedManifestsQuery.UniqueCode, IndexOrder = 9, ObjectFieldCode = "AgentSharedManifest.PackagesQuantity" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn AgentSharedManifestsQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AgentSharedManifestsQuery.Id,QueryCode = AgentSharedManifestsQuery.UniqueCode, IndexOrder = 10, ObjectFieldCode = "AgentSharedManifest.StatusName" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn AgentSharedManifestsQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AgentSharedManifestsQuery.Id,QueryCode = AgentSharedManifestsQuery.UniqueCode, IndexOrder = 11, ObjectFieldCode = "AgentSharedManifest.TEU" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn AgentSharedManifestsQueryColumn_12 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AgentSharedManifestsQuery.Id,QueryCode = AgentSharedManifestsQuery.UniqueCode, IndexOrder = 12, ObjectFieldCode = "AgentSharedManifest.CreateDate" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn AgentSharedManifestsQueryColumn_13 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AgentSharedManifestsQuery.Id,QueryCode = AgentSharedManifestsQuery.UniqueCode, IndexOrder = 13, ObjectFieldCode = "AgentSharedManifest.UpdateDate" , ColumnWidth = 150 }, addedQueryColumns);
  
	      

			  Query AirAgentSharedManifestsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = AgentSharedManifestTextCode_1.Id, NameTextCodeCode = AgentSharedManifestTextCode_1.Code, ObjectTableName = "AgentSharedManifest", Code = "AirAgentSharedManifests",  EditWizardName = "./Common/Components/SharedManifest/SharedManifestComponent",
			   QueryGroupCode = "AASM", IndexOrder = 1, Tenant = 0, ObjectTableId = AgentSharedManifestObjectTable.Id, QuerySection = "AgentSharedManifest", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = AgentSharedManifestFeature_1.Id,FeatureUniqeCode= AgentSharedManifestFeature_1.FeatureUniqeCode, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", Perspective = null }, addedQueries);
	
			 QueryColumn AirAgentSharedManifestsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirAgentSharedManifestsQuery.Id,QueryCode = AirAgentSharedManifestsQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "AgentSharedManifest.DirectionId" , ColumnWidth = 25 }, addedQueryColumns);

			 QueryColumn AirAgentSharedManifestsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirAgentSharedManifestsQuery.Id,QueryCode = AirAgentSharedManifestsQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "AgentSharedManifest.TransportModeId" , ColumnWidth = 25 }, addedQueryColumns);

			 QueryColumn AirAgentSharedManifestsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirAgentSharedManifestsQuery.Id,QueryCode = AirAgentSharedManifestsQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "AgentSharedManifest.Master" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn AirAgentSharedManifestsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirAgentSharedManifestsQuery.Id,QueryCode = AirAgentSharedManifestsQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "AgentSharedManifest.AgentName" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn AirAgentSharedManifestsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirAgentSharedManifestsQuery.Id,QueryCode = AirAgentSharedManifestsQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "AgentSharedManifest.Routing" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn AirAgentSharedManifestsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirAgentSharedManifestsQuery.Id,QueryCode = AirAgentSharedManifestsQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "AgentSharedManifest.AgentReference" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn AirAgentSharedManifestsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirAgentSharedManifestsQuery.Id,QueryCode = AirAgentSharedManifestsQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "AgentSharedManifest.ShipmentLevelName" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn AirAgentSharedManifestsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirAgentSharedManifestsQuery.Id,QueryCode = AirAgentSharedManifestsQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "AgentSharedManifest.GrossWeight" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn AirAgentSharedManifestsQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirAgentSharedManifestsQuery.Id,QueryCode = AirAgentSharedManifestsQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "AgentSharedManifest.ShipmentlevelCode" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn AirAgentSharedManifestsQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirAgentSharedManifestsQuery.Id,QueryCode = AirAgentSharedManifestsQuery.UniqueCode, IndexOrder = 9, ObjectFieldCode = "AgentSharedManifest.PackagesQuantity" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn AirAgentSharedManifestsQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirAgentSharedManifestsQuery.Id,QueryCode = AirAgentSharedManifestsQuery.UniqueCode, IndexOrder = 10, ObjectFieldCode = "AgentSharedManifest.TEU" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn AirAgentSharedManifestsQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirAgentSharedManifestsQuery.Id,QueryCode = AirAgentSharedManifestsQuery.UniqueCode, IndexOrder = 11, ObjectFieldCode = "AgentSharedManifest.CreateDate" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn AirAgentSharedManifestsQueryColumn_12 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AirAgentSharedManifestsQuery.Id,QueryCode = AirAgentSharedManifestsQuery.UniqueCode, IndexOrder = 12, ObjectFieldCode = "AgentSharedManifest.UpdateDate" , ColumnWidth = 150 }, addedQueryColumns);

             AdvancedQueryFilter AirAgentSharedManifestsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "AgentSharedManifest.TransportModeId", PredefinedValue = "A",PredefinedValue2 = null, CustomPredefined = false, QueryId = AirAgentSharedManifestsQuery.Id,QueryCode = AirAgentSharedManifestsQuery.UniqueCode, Tenant = 0}, addedQueryFilters);


             AdvancedQueryFilter AirAgentSharedManifestsQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "AgentSharedManifest.StatusCode", PredefinedValue = "WAIT",PredefinedValue2 = null, CustomPredefined = false, QueryId = AirAgentSharedManifestsQuery.Id,QueryCode = AirAgentSharedManifestsQuery.UniqueCode, Tenant = 0}, addedQueryFilters);

  
	      

			  Query OceanAgentSharedManifestsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = AgentSharedManifestTextCode_2.Id, NameTextCodeCode = AgentSharedManifestTextCode_2.Code, ObjectTableName = "AgentSharedManifest", Code = "OceanAgentSharedManifests",  EditWizardName = "./Common/Components/SharedManifest/SharedManifestComponent",
			   QueryGroupCode = "OASM", IndexOrder = 2, Tenant = 0, ObjectTableId = AgentSharedManifestObjectTable.Id, QuerySection = "AgentSharedManifest", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = AgentSharedManifestFeature_2.Id,FeatureUniqeCode= AgentSharedManifestFeature_2.FeatureUniqeCode, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", Perspective = null }, addedQueries);
	
			 QueryColumn OceanAgentSharedManifestsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OceanAgentSharedManifestsQuery.Id,QueryCode = OceanAgentSharedManifestsQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "AgentSharedManifest.DirectionId" , ColumnWidth = 25 }, addedQueryColumns);

			 QueryColumn OceanAgentSharedManifestsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OceanAgentSharedManifestsQuery.Id,QueryCode = OceanAgentSharedManifestsQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "AgentSharedManifest.TransportModeId" , ColumnWidth = 25 }, addedQueryColumns);

			 QueryColumn OceanAgentSharedManifestsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OceanAgentSharedManifestsQuery.Id,QueryCode = OceanAgentSharedManifestsQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "AgentSharedManifest.Master" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn OceanAgentSharedManifestsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OceanAgentSharedManifestsQuery.Id,QueryCode = OceanAgentSharedManifestsQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "AgentSharedManifest.AgentName" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn OceanAgentSharedManifestsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OceanAgentSharedManifestsQuery.Id,QueryCode = OceanAgentSharedManifestsQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "AgentSharedManifest.Routing" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn OceanAgentSharedManifestsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OceanAgentSharedManifestsQuery.Id,QueryCode = OceanAgentSharedManifestsQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "AgentSharedManifest.AgentReference" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn OceanAgentSharedManifestsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OceanAgentSharedManifestsQuery.Id,QueryCode = OceanAgentSharedManifestsQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "AgentSharedManifest.ShipmentLevelName" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn OceanAgentSharedManifestsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OceanAgentSharedManifestsQuery.Id,QueryCode = OceanAgentSharedManifestsQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "AgentSharedManifest.GrossWeight" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn OceanAgentSharedManifestsQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OceanAgentSharedManifestsQuery.Id,QueryCode = OceanAgentSharedManifestsQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "AgentSharedManifest.ShipmentlevelCode" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn OceanAgentSharedManifestsQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OceanAgentSharedManifestsQuery.Id,QueryCode = OceanAgentSharedManifestsQuery.UniqueCode, IndexOrder = 9, ObjectFieldCode = "AgentSharedManifest.PackagesQuantity" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn OceanAgentSharedManifestsQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OceanAgentSharedManifestsQuery.Id,QueryCode = OceanAgentSharedManifestsQuery.UniqueCode, IndexOrder = 10, ObjectFieldCode = "AgentSharedManifest.TEU" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn OceanAgentSharedManifestsQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OceanAgentSharedManifestsQuery.Id,QueryCode = OceanAgentSharedManifestsQuery.UniqueCode, IndexOrder = 11, ObjectFieldCode = "AgentSharedManifest.CreateDate" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn OceanAgentSharedManifestsQueryColumn_12 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OceanAgentSharedManifestsQuery.Id,QueryCode = OceanAgentSharedManifestsQuery.UniqueCode, IndexOrder = 12, ObjectFieldCode = "AgentSharedManifest.UpdateDate" , ColumnWidth = 150 }, addedQueryColumns);

             AdvancedQueryFilter OceanAgentSharedManifestsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "AgentSharedManifest.TransportModeId", PredefinedValue = "O",PredefinedValue2 = null, CustomPredefined = false, QueryId = OceanAgentSharedManifestsQuery.Id,QueryCode = OceanAgentSharedManifestsQuery.UniqueCode, Tenant = 0}, addedQueryFilters);


             AdvancedQueryFilter OceanAgentSharedManifestsQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "AgentSharedManifest.StatusCode", PredefinedValue = "WAIT",PredefinedValue2 = null, CustomPredefined = false, QueryId = OceanAgentSharedManifestsQuery.Id,QueryCode = OceanAgentSharedManifestsQuery.UniqueCode, Tenant = 0}, addedQueryFilters);

  
	      

			  Query InlandAgentSharedManifestsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = AgentSharedManifestTextCode_3.Id, NameTextCodeCode = AgentSharedManifestTextCode_3.Code, ObjectTableName = "AgentSharedManifest", Code = "InlandAgentSharedManifests",  EditWizardName = "./Common/Components/SharedManifest/SharedManifestComponent",
			   QueryGroupCode = "IASM", IndexOrder = 3, Tenant = 0, ObjectTableId = AgentSharedManifestObjectTable.Id, QuerySection = "AgentSharedManifest", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = AgentSharedManifestFeature_3.Id,FeatureUniqeCode= AgentSharedManifestFeature_3.FeatureUniqeCode, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", Perspective = null }, addedQueries);
	
			 QueryColumn InlandAgentSharedManifestsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InlandAgentSharedManifestsQuery.Id,QueryCode = InlandAgentSharedManifestsQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "AgentSharedManifest.DirectionId" , ColumnWidth = 25 }, addedQueryColumns);

			 QueryColumn InlandAgentSharedManifestsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InlandAgentSharedManifestsQuery.Id,QueryCode = InlandAgentSharedManifestsQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "AgentSharedManifest.TransportModeId" , ColumnWidth = 25 }, addedQueryColumns);

			 QueryColumn InlandAgentSharedManifestsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InlandAgentSharedManifestsQuery.Id,QueryCode = InlandAgentSharedManifestsQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "AgentSharedManifest.Master" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn InlandAgentSharedManifestsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InlandAgentSharedManifestsQuery.Id,QueryCode = InlandAgentSharedManifestsQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "AgentSharedManifest.AgentName" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn InlandAgentSharedManifestsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InlandAgentSharedManifestsQuery.Id,QueryCode = InlandAgentSharedManifestsQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "AgentSharedManifest.Routing" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn InlandAgentSharedManifestsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InlandAgentSharedManifestsQuery.Id,QueryCode = InlandAgentSharedManifestsQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "AgentSharedManifest.AgentReference" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn InlandAgentSharedManifestsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InlandAgentSharedManifestsQuery.Id,QueryCode = InlandAgentSharedManifestsQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "AgentSharedManifest.ShipmentLevelName" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn InlandAgentSharedManifestsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InlandAgentSharedManifestsQuery.Id,QueryCode = InlandAgentSharedManifestsQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "AgentSharedManifest.GrossWeight" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn InlandAgentSharedManifestsQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InlandAgentSharedManifestsQuery.Id,QueryCode = InlandAgentSharedManifestsQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "AgentSharedManifest.ShipmentlevelCode" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn InlandAgentSharedManifestsQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InlandAgentSharedManifestsQuery.Id,QueryCode = InlandAgentSharedManifestsQuery.UniqueCode, IndexOrder = 9, ObjectFieldCode = "AgentSharedManifest.PackagesQuantity" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn InlandAgentSharedManifestsQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InlandAgentSharedManifestsQuery.Id,QueryCode = InlandAgentSharedManifestsQuery.UniqueCode, IndexOrder = 10, ObjectFieldCode = "AgentSharedManifest.TEU" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn InlandAgentSharedManifestsQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InlandAgentSharedManifestsQuery.Id,QueryCode = InlandAgentSharedManifestsQuery.UniqueCode, IndexOrder = 11, ObjectFieldCode = "AgentSharedManifest.CreateDate" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn InlandAgentSharedManifestsQueryColumn_12 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = InlandAgentSharedManifestsQuery.Id,QueryCode = InlandAgentSharedManifestsQuery.UniqueCode, IndexOrder = 12, ObjectFieldCode = "AgentSharedManifest.UpdateDate" , ColumnWidth = 150 }, addedQueryColumns);

             AdvancedQueryFilter InlandAgentSharedManifestsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "AgentSharedManifest.TransportModeId", PredefinedValue = "I",PredefinedValue2 = null, CustomPredefined = false, QueryId = InlandAgentSharedManifestsQuery.Id,QueryCode = InlandAgentSharedManifestsQuery.UniqueCode, Tenant = 0}, addedQueryFilters);


             AdvancedQueryFilter InlandAgentSharedManifestsQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "AgentSharedManifest.StatusCode", PredefinedValue = "WAIT",PredefinedValue2 = null, CustomPredefined = false, QueryId = InlandAgentSharedManifestsQuery.Id,QueryCode = InlandAgentSharedManifestsQuery.UniqueCode, Tenant = 0,Operator = "Equal"}, addedQueryFilters);

  
	      

			  Query CancelledAgentSharedManifestsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = AgentSharedManifestTextCode_4.Id, NameTextCodeCode = AgentSharedManifestTextCode_4.Code, ObjectTableName = "AgentSharedManifest", Code = "CancelledAgentSharedManifests",  EditWizardName = "./Common/Components/SharedManifest/SharedManifestComponent",
			   QueryGroupCode = "CASM", IndexOrder = 4, Tenant = 0, ObjectTableId = AgentSharedManifestObjectTable.Id, QuerySection = "AgentSharedManifest", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = AgentSharedManifestFeature_4.Id,FeatureUniqeCode= AgentSharedManifestFeature_4.FeatureUniqeCode, DefaultSortName = "CreateDate", DefaultSortDirection = "Descending", Perspective = null }, addedQueries);
	
			 QueryColumn CancelledAgentSharedManifestsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledAgentSharedManifestsQuery.Id,QueryCode = CancelledAgentSharedManifestsQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "AgentSharedManifest.DirectionId" , ColumnWidth = 25 }, addedQueryColumns);

			 QueryColumn CancelledAgentSharedManifestsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledAgentSharedManifestsQuery.Id,QueryCode = CancelledAgentSharedManifestsQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "AgentSharedManifest.TransportModeId" , ColumnWidth = 25 }, addedQueryColumns);

			 QueryColumn CancelledAgentSharedManifestsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledAgentSharedManifestsQuery.Id,QueryCode = CancelledAgentSharedManifestsQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "AgentSharedManifest.Master" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn CancelledAgentSharedManifestsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledAgentSharedManifestsQuery.Id,QueryCode = CancelledAgentSharedManifestsQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "AgentSharedManifest.AgentName" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn CancelledAgentSharedManifestsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledAgentSharedManifestsQuery.Id,QueryCode = CancelledAgentSharedManifestsQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "AgentSharedManifest.Routing" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn CancelledAgentSharedManifestsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledAgentSharedManifestsQuery.Id,QueryCode = CancelledAgentSharedManifestsQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "AgentSharedManifest.AgentReference" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn CancelledAgentSharedManifestsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledAgentSharedManifestsQuery.Id,QueryCode = CancelledAgentSharedManifestsQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "AgentSharedManifest.ShipmentLevelName" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn CancelledAgentSharedManifestsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledAgentSharedManifestsQuery.Id,QueryCode = CancelledAgentSharedManifestsQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "AgentSharedManifest.GrossWeight" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn CancelledAgentSharedManifestsQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledAgentSharedManifestsQuery.Id,QueryCode = CancelledAgentSharedManifestsQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "AgentSharedManifest.ShipmentlevelCode" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn CancelledAgentSharedManifestsQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledAgentSharedManifestsQuery.Id,QueryCode = CancelledAgentSharedManifestsQuery.UniqueCode, IndexOrder = 9, ObjectFieldCode = "AgentSharedManifest.PackagesQuantity" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn CancelledAgentSharedManifestsQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledAgentSharedManifestsQuery.Id,QueryCode = CancelledAgentSharedManifestsQuery.UniqueCode, IndexOrder = 10, ObjectFieldCode = "AgentSharedManifest.TEU" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn CancelledAgentSharedManifestsQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledAgentSharedManifestsQuery.Id,QueryCode = CancelledAgentSharedManifestsQuery.UniqueCode, IndexOrder = 11, ObjectFieldCode = "AgentSharedManifest.CreateDate" , ColumnWidth = 150 }, addedQueryColumns);

			 QueryColumn CancelledAgentSharedManifestsQueryColumn_12 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CancelledAgentSharedManifestsQuery.Id,QueryCode = CancelledAgentSharedManifestsQuery.UniqueCode, IndexOrder = 12, ObjectFieldCode = "AgentSharedManifest.UpdateDate" , ColumnWidth = 150 }, addedQueryColumns);

             AdvancedQueryFilter CancelledAgentSharedManifestsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "AgentSharedManifest.StatusCode", PredefinedValue = "CANC",PredefinedValue2 = null, CustomPredefined = false, QueryId = CancelledAgentSharedManifestsQuery.Id,QueryCode = CancelledAgentSharedManifestsQuery.UniqueCode, Tenant = 0}, addedQueryFilters);

			SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
			SqlBulkInsert.BulkInsert("Features", addedFeatures);
			SqlBulkInsert.BulkInsert("Queries", addedQueries);
			SqlBulkInsert.BulkInsert("QueryColumns", addedQueryColumns);
			SqlBulkInsert.BulkInsert("AdvancedQueryFilters", addedQueryFilters);	 
  
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable AgentSharedManifestObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "AgentSharedManifest" && d.Tenant == 0).FirstOrDefault();
		   //List<ObjectField> AgentSharedManifestObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "AgentSharedManifest").ToList();
		       
	      

	         Screen AgentSharedManifestAgentSharedManifestHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "AgentSharedManifest.HeaderScreen", Name = "AgentSharedManifestHeaderScreen", ObjectTableId = AgentSharedManifestObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      	
		    AgentSharedManifestObjectTable.HeaderScreenId = AgentSharedManifestAgentSharedManifestHeaderScreenScreen0.Id;
		    AgentSharedManifestObjectTable.HeaderScreenCode = AgentSharedManifestAgentSharedManifestHeaderScreenScreen0.Code;

	   		  

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {      
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable AgentSharedManifestObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "AgentSharedManifest" && d.Tenant == 0).FirstOrDefault(); 

		   Feature AgentSharedManifestFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = AgentSharedManifestObjectTable.Id, Tenant = 0, NameTextCodeCode = "AgentSharedManifest.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,AgentSharedManifestObjectTable);
		   Feature AgentSharedManifestFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = AgentSharedManifestObjectTable.Id, Tenant = 0, NameTextCodeCode = "AgentSharedManifest.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,AgentSharedManifestObjectTable);
		   Feature AgentSharedManifestFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = AgentSharedManifestObjectTable.Id, Tenant = 0, NameTextCodeCode = "AgentSharedManifest.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,AgentSharedManifestObjectTable);
		   Feature AgentSharedManifestFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = false, ObjectTableId = AgentSharedManifestObjectTable.Id, Tenant = 0, NameTextCodeCode = "AgentSharedManifest.Features.PackageFeature", NameTextCodeDefaultText = "AgentSharedManifest Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,AgentSharedManifestObjectTable); 

		   		   //--------------> Additional Features <--------------\\

		   Feature AgentSharedManifestFeature_UPDATESHAREDAGENT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATESHAREDAGENT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = AgentSharedManifestObjectTable.Id, Tenant = 0, NameTextCodeCode = "AgentSharedManifest.Features.UpdateSharedAgent", NameTextCodeDefaultText = @"Update Shared Agent" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,AgentSharedManifestObjectTable);

   
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable AgentSharedManifestObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "AgentSharedManifest" && d.Tenant == 0).FirstOrDefault(); 
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
                ObjectTableId = AgentSharedManifestObjectTable.Id,
				 
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
                ObjectTableId = AgentSharedManifestObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {     
	    
}

    

   }
    
}
	 