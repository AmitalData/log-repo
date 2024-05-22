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

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.ShipmentsModel.EntityUpdateClasses
{
   public class ContainerTrackingProviderUpdateClass
   {  		
		public const string HashString = "96c2ac5c19d2c3b52bcca3c2185eba6b";
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "ContainerTrackingProvider",
			      				    IsNew =  true,
			      				    DBTableName =  "ContainerTrackingProviders",
			      				    ObjectTableSingular =  "Container Tracking Setting",
			      				    ObjectTablePlural =  "Container Tracking Settings",
			      				    HasCustomFilter =  false,
			      				    HasCustomFields =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  true,
			      				    IsNewWizard =  false,
			      				    LookUp1 =  "Name",
			      				    LookUp2 =  "Name",
			      				    KeyPropertyPath =  "SourceCode",
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
			      				    SortingByObjectField =  "SourceCode",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    DefaultText =  "Container Tracking Setting",
			      				    Code =  "70a6",
			      				    Name =  " Query Group",
			      				    CloseTableCode =  "Code",
			      				    CloseTableName =  "Name",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Shipment",
			      				    NoTS =  false,
			      				    HasMenuButtons =  true,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    IsTabsHidden =  false,
			      				    HashString =  ContainerTrackingProviderUpdateClass.HashString,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository, List<ObjectField> addedFields, List<TextCode> addedTextCodes)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SourceCode",
					  						ObjectTableName =  "ContainerTrackingProvider",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "ContainerStatusSource",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "SourceCode",
					  						ListPropertyPath =  "SourceCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  true,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Container Tracking Provider",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSetting =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFilterInx =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SourceCode",
					  						DefaultText =  "Source Code",
					  						ListFieldLable =  "SourceCodeListLable",
					  						ListLableDefaultText =  "Source Code",
					  						IsForeignKey =  true,
					  						ForeignEntity =  "ContainerStatusSource",
					  						NavigationPropertyName =  "ContainerStatusSource",
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
					 
					 						FieldName =  "Name",
					  						ObjectTableName =  "ContainerTrackingProvider",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  200,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  true,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  200,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  true,
					  						PMPropertyPath =  "Name",
					  						ListPropertyPath =  "Name",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Container Tracking Provider",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSetting =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFilterInx =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Name",
					  						DefaultText =  "Name",
					  						ListFieldLable =  "NameListLable",
					  						ListLableDefaultText =  "Name",
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
					 
					 						FieldName =  "SearchFields",
					  						ObjectTableName =  "ContainerTrackingProvider",
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
					  						ValidForQuerySection1 =  "Container Tracking Provider",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSetting =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFilterInx =  0,
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
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CallbackURL",
					  						ObjectTableName =  "ContainerTrackingProvider",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  2000,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  2000,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CallbackURL",
					  						ListPropertyPath =  "CallbackURL",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Container Tracking Provider",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSetting =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFilterInx =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CallbackURL",
					  						DefaultText =  "Callback URL",
					  						ListFieldLable =  "CallbackURLListLable",
					  						ListLableDefaultText =  "Callback URL",
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
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "APIKey",
					  						ObjectTableName =  "ContainerTrackingProvider",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  2000,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  2000,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "APIKey",
					  						ListPropertyPath =  "APIKey",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Container Tracking Provider",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSetting =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFilterInx =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "APIKey",
					  						DefaultText =  "API Key",
					  						ListFieldLable =  "APIKeyListLable",
					  						ListLableDefaultText =  "API Key",
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
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ProviderURL",
					  						ObjectTableName =  "ContainerTrackingProvider",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  2000,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  2000,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ProviderURL",
					  						ListPropertyPath =  "ProviderURL",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Container Tracking Provider",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSetting =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFilterInx =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ProviderURL",
					  						DefaultText =  "Provider URL",
					  						ListFieldLable =  "ProviderURLListLable",
					  						ListLableDefaultText =  "Provider URL",
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
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LogitudeToken",
					  						ObjectTableName =  "ContainerTrackingProvider",
					  						FieldsDataType =  "Text",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "LogitudeToken",
					  						ListPropertyPath =  "LogitudeToken",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Container Tracking Provider",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSetting =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFilterInx =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LogitudeToken",
					  						DefaultText =  "Logitude Token",
					  						ListFieldLable =  "LogitudeTokenListLable",
					  						ListLableDefaultText =  "Logitude Token",
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
					 
					 						FieldName =  "Id",
					  						ObjectTableName =  "ContainerTrackingProvider",
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
					  						ValidForQuerySection1 =  "Id",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSetting =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFilterInx =  0,
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
					  						ObjectTableName =  "ContainerTrackingProvider",
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
					  						ValidForQuerySection1 =  "Container Tracking Provider",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSetting =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFilterInx =  0,
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
	        QueryGroup ContainerTrackingProviderQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "70a6", Name = " Query Group" }, queryGroupRepository,tenantQueryGroups);
						QueryGroup ContainerTrackingProviderQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "5d49", Name = " Query Group" }, queryGroupRepository,tenantQueryGroups);
				        queryGroupRepository.SubmitChanges();
	        ObjectTable ContainerTrackingProviderObjectTable = objectTables.ContainsKey("ContainerTrackingProvider") ? objectTables["ContainerTrackingProvider"] : null;
            if (ContainerTrackingProviderObjectTable == null)
            {
                IWebFreightContext objectContext = WebFreightContext.GetContext(0);  

                ContainerTrackingProviderObjectTable = objectContext.ObjectTables.Where(d => d.Name == "ContainerTrackingProvider" && d.Tenant == 0).FirstOrDefault();
            }

	         
			List<Feature> addedFeatures = new List<Feature>();
			List<TextCode> addedTextCodes = new List<TextCode>();
			List<Query> addedQueries = new List<Query>();
			List<QueryColumn> addedQueryColumns = new List<QueryColumn>();
			List<AdvancedQueryFilter> addedQueryFilters = new List<AdvancedQueryFilter>();
   

			   TextCode ContainerTrackingProviderTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ContainerTrackingProvider.Q.ContainerTrackingSetting", DefaultText = @"Container Tracking Setting",LocalDefaultText = "", ObjectTableId = ContainerTrackingProviderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature ContainerTrackingProviderFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ContainerTrackingProvider.Q.ContainerTrackingSetting", ObjectTableId = ContainerTrackingProviderObjectTable.Id, Tenant = 0, NameTextCodeCode = "ContainerTrackingProviderFeatures.ContainerTrackingSetting", NameTextCodeDefaultText = "Container Tracking Setting", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,ContainerTrackingProviderObjectTable, addedFeatures, addedTextCodes);

	        //TextCodeRepository.SubmitChanges();
	        //FeaturesRepository.SubmitChanges();    
	      

			  Query ContainerTrackingSettingQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = ContainerTrackingProviderTextCode_0.Id, NameTextCodeCode = ContainerTrackingProviderTextCode_0.Code, ObjectTableName = "ContainerTrackingProvider", Code = "Container Tracking Setting",  QueryGroupCode = "70a6", IndexOrder = 0, Tenant = 0, ObjectTableId = ContainerTrackingProviderObjectTable.Id, QuerySection = "ContainerTrackingProvider", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = ContainerTrackingProviderFeature_0.Id,FeatureUniqeCode= ContainerTrackingProviderFeature_0.FeatureUniqeCode, DefaultSortName = "SourceCode", DefaultSortDirection = "Ascending", Perspective = null }, addedQueries);
	
			 QueryColumn ContainerTrackingSettingQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ContainerTrackingSettingQuery.Id,QueryCode = ContainerTrackingSettingQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "ContainerTrackingProvider.SourceCode" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn ContainerTrackingSettingQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ContainerTrackingSettingQuery.Id,QueryCode = ContainerTrackingSettingQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "ContainerTrackingProvider.Name" , ColumnWidth = 155 }, addedQueryColumns);

			 QueryColumn ContainerTrackingSettingQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ContainerTrackingSettingQuery.Id,QueryCode = ContainerTrackingSettingQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "ContainerTrackingProvider.CallbackURL" , ColumnWidth = 256 }, addedQueryColumns);

			 QueryColumn ContainerTrackingSettingQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ContainerTrackingSettingQuery.Id,QueryCode = ContainerTrackingSettingQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "ContainerTrackingProvider.APIKey" , ColumnWidth = 145 }, addedQueryColumns);

			 QueryColumn ContainerTrackingSettingQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ContainerTrackingSettingQuery.Id,QueryCode = ContainerTrackingSettingQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "ContainerTrackingProvider.ProviderURL" , ColumnWidth = 138 }, addedQueryColumns);

			 QueryColumn ContainerTrackingSettingQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ContainerTrackingSettingQuery.Id,QueryCode = ContainerTrackingSettingQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "ContainerTrackingProvider.LogitudeToken" , ColumnWidth = 274 }, addedQueryColumns);
			SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
			SqlBulkInsert.BulkInsert("Features", addedFeatures);
			SqlBulkInsert.BulkInsert("Queries", addedQueries);
			SqlBulkInsert.BulkInsert("QueryColumns", addedQueryColumns);
			SqlBulkInsert.BulkInsert("AdvancedQueryFilters", addedQueryFilters);	 
  
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable ContainerTrackingProviderObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "ContainerTrackingProvider" && d.Tenant == 0).FirstOrDefault();
		   //List<ObjectField> ContainerTrackingProviderObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "ContainerTrackingProvider").ToList();
		       
	      

	         Screen ContainerTrackingProviderContainerTrackingProviderHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ContainerTrackingProvider.HeaderScreen", Name = "ContainerTrackingProviderHeaderScreen", ObjectTableId = ContainerTrackingProviderObjectTable.Id, NumberOfColumns = 1, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      
             ScreenField ContainerTrackingProviderContainerTrackingProviderHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ScreenId = ContainerTrackingProviderContainerTrackingProviderHeaderScreenScreen0.Id,ScreenCode = ContainerTrackingProviderContainerTrackingProviderHeaderScreenScreen0.Code, ObjectFieldCode = "ContainerTrackingProvider.SourceCode", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          	
		    ContainerTrackingProviderObjectTable.HeaderScreenId = ContainerTrackingProviderContainerTrackingProviderHeaderScreenScreen0.Id;
		    ContainerTrackingProviderObjectTable.HeaderScreenCode = ContainerTrackingProviderContainerTrackingProviderHeaderScreenScreen0.Code;

	   		  
	      

	         Screen ContainerTrackingProviderGeneralTabScreenScreen1 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "ContainerTrackingProvider.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = ContainerTrackingProviderObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 3, IsReadOnly = false }, screensRepository, tenantScreens);
      
             ScreenField ContainerTrackingProviderContainerTrackingProviderGeneralTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ScreenId = ContainerTrackingProviderGeneralTabScreenScreen1.Id,ScreenCode = ContainerTrackingProviderGeneralTabScreenScreen1.Code, ObjectFieldCode = "ContainerTrackingProvider.SourceCode", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField ContainerTrackingProviderContainerTrackingProviderGeneralTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ScreenId = ContainerTrackingProviderGeneralTabScreenScreen1.Id,ScreenCode = ContainerTrackingProviderGeneralTabScreenScreen1.Code, ObjectFieldCode = "ContainerTrackingProvider.ProviderURL", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField ContainerTrackingProviderContainerTrackingProviderGeneralTabScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ScreenId = ContainerTrackingProviderGeneralTabScreenScreen1.Id,ScreenCode = ContainerTrackingProviderGeneralTabScreenScreen1.Code, ObjectFieldCode = "ContainerTrackingProvider.CallbackURL", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField ContainerTrackingProviderContainerTrackingProviderGeneralTabScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ScreenId = ContainerTrackingProviderGeneralTabScreenScreen1.Id,ScreenCode = ContainerTrackingProviderGeneralTabScreenScreen1.Code, ObjectFieldCode = "ContainerTrackingProvider.Name", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField ContainerTrackingProviderContainerTrackingProviderGeneralTabScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ScreenId = ContainerTrackingProviderGeneralTabScreenScreen1.Id,ScreenCode = ContainerTrackingProviderGeneralTabScreenScreen1.Code, ObjectFieldCode = "ContainerTrackingProvider.APIKey", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField ContainerTrackingProviderContainerTrackingProviderGeneralTabScreenScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 2, ScreenId = ContainerTrackingProviderGeneralTabScreenScreen1.Id,ScreenCode = ContainerTrackingProviderGeneralTabScreenScreen1.Code, ObjectFieldCode = "ContainerTrackingProvider.LogitudeToken", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	            

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable ContainerTrackingProviderObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "ContainerTrackingProvider" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode ContainerTrackingProviderGeneralTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ContainerTrackingProvider.TH.General", DefaultText = "General",LocalDefaultText = null, ObjectTableId = ContainerTrackingProviderObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature ContainerTrackingProviderGeneralFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ContainerTrackingProvider.Tab.General", ObjectTableId = ContainerTrackingProviderObjectTable.Id, Tenant = 0, NameTextCodeCode = "ContainerTrackingProviderFeatures.CTSG", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,ContainerTrackingProviderObjectTable);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CTSG",HtmlComponentName = "GeneralTabComponent",HtmlComponentUrl = "./Infrastructure/GenericComponents/GeneralTabComponent", FeatureId = ContainerTrackingProviderGeneralFeature_TH0.Id,FeatureUniqeCode = ContainerTrackingProviderGeneralFeature_TH0.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.GeneralControls.GeneralTabControl", ObjectTableId = ContainerTrackingProviderObjectTable.Id, TabNameTextCodeId = ContainerTrackingProviderGeneralTextCode_TH0.Id, TabNameTextCodeCode = ContainerTrackingProviderGeneralTextCode_TH0.Code, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable ContainerTrackingProviderObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "ContainerTrackingProvider" && d.Tenant == 0).FirstOrDefault(); 

		   Feature ContainerTrackingProviderFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = ContainerTrackingProviderObjectTable.Id, Tenant = 0, NameTextCodeCode = "ContainerTrackingProvider.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ContainerTrackingProviderObjectTable);
		   Feature ContainerTrackingProviderFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = ContainerTrackingProviderObjectTable.Id, Tenant = 0, NameTextCodeCode = "ContainerTrackingProvider.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ContainerTrackingProviderObjectTable);
		   Feature ContainerTrackingProviderFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = ContainerTrackingProviderObjectTable.Id, Tenant = 0, NameTextCodeCode = "ContainerTrackingProvider.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ContainerTrackingProviderObjectTable);
		   Feature ContainerTrackingProviderFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = false, ObjectTableId = ContainerTrackingProviderObjectTable.Id, Tenant = 0, NameTextCodeCode = "ContainerTrackingProvider.Features.PackageFeature", NameTextCodeDefaultText = "ContainerTrackingProvider Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ContainerTrackingProviderObjectTable);    
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable ContainerTrackingProviderObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "ContainerTrackingProvider" && d.Tenant == 0).FirstOrDefault(); 
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
                ObjectTableId = ContainerTrackingProviderObjectTable.Id,
				 
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
                ObjectTableId = ContainerTrackingProviderObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
		   //FeatureRepository featureRepository = new FeatureRepository(0); 
		   //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList(); 
		   ObjectTable ContainerTrackingProviderObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "ContainerTrackingProvider" && d.Tenant == 0).FirstOrDefault(); 			   Feature ContainerTrackingProviderFeature_MB00 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GetCarrier", ObjectTableId = ContainerTrackingProviderObjectTable.Id, Tenant = 0, NameTextCodeCode = "ContainerTrackingProvider.Features.GetCarrier", NameTextCodeDefaultText = "Get Carrier", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,ContainerTrackingProviderObjectTable);
             			   Feature ContainerTrackingProviderFeature_MB01 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACTVRef", ObjectTableId = ContainerTrackingProviderObjectTable.Id, Tenant = 0, NameTextCodeCode = "ContainerTrackingProvider.Features.GetActiveRequests", NameTextCodeDefaultText = "Get Active Requests", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,ContainerTrackingProviderObjectTable);
              

		   TextCodeRepository.SubmitChanges();
		   FeaturesRepository.SubmitChanges();
		   MenuButtonGroup ContainerTrackingProviderMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
				{
					MenuButtonGroupType = "ContainerTrackingProviderEdit",
					Name = "CTPEditButtonsGroup",
					ObjectTableId = ContainerTrackingProviderObjectTable.Id,
					Tenant = 0
				}, menuButtonGroupRepository, tenantMenuButtonGroups);        
   
			   MenuButton ContainerTrackingProviderMenuButton0 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "Actions",
						Index = 0, 
						IsActive = false,
						LabelTextCodeCode = "ContainerTrackingProvider.B.More",
						LabelTextCodeDefaultText = "More",
						Tenant = 0,
						MenuButtonGroupId = ContainerTrackingProviderMenuButtonGroup.Id,
						ObjectTableId = ContainerTrackingProviderObjectTable.Id,
						MenuButtonType = "dropdownbutton",
						FeatureId = null,
						Style = null,
						LocalDefaultText = null,
						FeatureUniqeCode = null,
						HtmlComponentPath = null,
						Width = 0,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);

			   MenuButton ContainerTrackingProviderMenuButton00 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "GetCarrier",
						Index = 0, 
						IsActive = true,
						LabelTextCodeCode = "ContainerTrackingProvider.B.GetCarrier",
						LabelTextCodeDefaultText = "Get Carrier",
						Tenant = 0,
						MenuButtonGroupId = ContainerTrackingProviderMenuButtonGroup.Id,
						ParentMenuButtonId = ContainerTrackingProviderMenuButton0.Id,
						ObjectTableId = ContainerTrackingProviderObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  ContainerTrackingProviderFeature_MB00.Id,
						Style = null,
						LocalDefaultText = "Get Carrier",
                        HtmlComponentPath=null,
                        Width=0,
						FeatureUniqeCode=  ContainerTrackingProviderFeature_MB00.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton ContainerTrackingProviderMenuButton01 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "ACTVRef",
						Index = 1, 
						IsActive = true,
						LabelTextCodeCode = "ContainerTrackingProvider.B.GetActiveRequests",
						LabelTextCodeDefaultText = "Get Active Requests",
						Tenant = 0,
						MenuButtonGroupId = ContainerTrackingProviderMenuButtonGroup.Id,
						ParentMenuButtonId = ContainerTrackingProviderMenuButton0.Id,
						ObjectTableId = ContainerTrackingProviderObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  ContainerTrackingProviderFeature_MB01.Id,
						Style = null,
						LocalDefaultText = "Get Active Requests",
                        HtmlComponentPath=null,
                        Width=0,
						FeatureUniqeCode=  ContainerTrackingProviderFeature_MB01.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {     
	    
}

    

   }
    
}
	 