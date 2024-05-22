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
   public class CustomsHouseTypeAdditionalUpdateClass
   {  		
		public const string HashString = "52a09f80f62a1d708a2a7bcc2ee8dca1";
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "Customs.CustomsHouseTypeAdditional",
			      				    IsNew =  false,
			      				    DBTableName =  "Customs.CustomsHouseTypeAdditionals",
			      				    ObjectTableSingular =  "CustomsHouseTypeAdditional",
			      				    ObjectTablePlural =  "CustomsHouseTypeAdditionals",
			      				    HasCustomFilter =  false,
			      				    HasCustomFields =  false,
			      				    AvailableInCustomization =  true,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  false,
			      				    IsNewWizard =  false,
			      				    KeyPropertyPath =  "Id",
			      				    AutoCompleteSearchWindow =  false,
			      				    IsClosed =  false,
			      				    CacheOnClient =  true,
			      				    EditableFromAutoCompleteWindow =  false,
			      				    HasCounter =  false,
			      				    EnableAddFromLOV =  false,
			      				    IsRestrictable =  false,
			      				    IsMain =  true,
			      				    IsAutoComplete =  false,
			      				    EnableEditFromLOV =  false,
			      				    SortingByObjectField =  "Id",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "BR",
			      				    MaxNumberOfCustomFields =  0,
			      				    LocalDefaultText =  "הגדרות בתי מכס ",
			      				    DefaultText =  "Customs House Type Additional",
			      				    Code =  "CHQG",
			      				    Name =  "Customs.CustomsHouseTypeAdditional",
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
			      				    HashString =  CustomsHouseTypeAdditionalUpdateClass.HashString,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository, List<ObjectField> addedFields, List<TextCode> addedTextCodes)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Id",
					  						ObjectTableName =  "Customs.CustomsHouseTypeAdditional",
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
					  						ObjectTableName =  "Customs.CustomsHouseTypeAdditional",
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
					 
					 						FieldName =  "Code",
					  						ObjectTableName =  "Customs.CustomsHouseTypeAdditional",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.CustomsHouseType",
					  						MinLength =  0,
					  						MaxLength =  17,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  17,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
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
					  						ValidForQuerySection1 =  "CustomsHouseTypeAdditional",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSetting =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFilterInx =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Code",
					  						DefaultText =  "Code",
					  						FullLocalDefaultText =  "טבלת סוג בית מכס",
					  						ListFieldLable =  "CodeListLable",
					  						ListLableDefaultText =  "Code",
					  						ListLocalDefaultText =  "טבלת סוג בית מכס",
					  						IsForeignKey =  true,
					  						ForeignEntity =  "CustomsHouseType",
					  						NavigationPropertyName =  "CustomsHouseType",
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
					 
					 						FieldName =  "TransportModeId",
					  						ObjectTableName =  "Customs.CustomsHouseTypeAdditional",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.CustomsTransportMode",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						Operator =  "Equals",
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
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSetting =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFilterInx =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TransportModeId",
					  						DefaultText =  "Transport Mode",
					  						FullLocalDefaultText =  "מצב תחבורה",
					  						IsForeignKey =  true,
					  						ForeignEntity =  "CustomsTransportMode",
					  						NavigationPropertyName =  "CustomsTransportMode",
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
					 
					 						FieldName =  "UnloadPortCode",
					  						ObjectTableName =  "Customs.CustomsHouseTypeAdditional",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.UnloadingSiteType",
					  						MinLength =  0,
					  						MaxLength =  17,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  17,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						Operator =  "Equals",
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
					  						ValidForQuerySection1 =  "CustomsHouseTypeAdditional",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSetting =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFilterInx =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UnloadPortCode",
					  						DefaultText =  "Unload Port ",
					  						FullLocalDefaultText =  "טבלת סוג אתר פריקה",
					  						ListFieldLable =  "UnloadPortCodeListLable",
					  						ListLableDefaultText =  "Unload Port Code",
					  						ListLocalDefaultText =  "טבלת סוג בית מכס",
					  						IsForeignKey =  true,
					  						ForeignEntity =  "UnloadingSiteType",
					  						NavigationPropertyName =  "UnloadingSiteType",
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
					 
					 						FieldName =  "TransportModeName",
					  						ObjectTableName =  "Customs.CustomsHouseTypeAdditional",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
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
					  						ValidForQuerySection1 =  "CustomsHouseTypeAdditional",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSetting =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFilterInx =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TransportModeName",
					  						DefaultText =  "Transport Mode",
					  						FullLocalDefaultText =  "מצב תחבורה",
					  						ListFieldLable =  "TransportModeNameListLable",
					  						ListLableDefaultText =  "Transport Mode",
					  						ListLocalDefaultText =  "מצב תחבורה",
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
					 
					 						FieldName =  "UnloadPortName",
					  						ObjectTableName =  "Customs.CustomsHouseTypeAdditional",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "UnloadPortName",
					  						ListPropertyPath =  "UnloadPortName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomsHouseTypeAdditional",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSetting =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFilterInx =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UnloadPortName",
					  						DefaultText =  "Unload Port",
					  						FullLocalDefaultText =  "טבלת סוג אתר פריקה",
					  						ListFieldLable =  "UnloadPortNameListLable",
					  						ListLableDefaultText =  "Unload Port",
					  						ListLocalDefaultText =  "טבלת סוג אתר פריקה",
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
					 
					 						FieldName =  "Name",
					  						ObjectTableName =  "Customs.CustomsHouseTypeAdditional",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
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
					  						ValidForQuerySection1 =  "CustomsHouseTypeAdditional",
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
					  						FullLocalDefaultText =  "טבלת סוג בית מכס",
					  						ListFieldLable =  "NameListLable",
					  						ListLableDefaultText =  "Name",
					  						ListLocalDefaultText =  "טבלת סוג בית מכס",
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
					  						ObjectTableName =  "Customs.CustomsHouseTypeAdditional",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  1000,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  1000,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						IsListFilter =  false,
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
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSetting =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFilterInx =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SearchFields",
					  						DefaultText =  "Search",
					  						FullLocalDefaultText =  "חיפוש",
					  						ListFieldLable =  "SearchFieldsListLable",
					  						ListLableDefaultText =  "Seach",
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
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters,Dictionary<string, QueryGroup> tenantQueryGroups )
	    {  
	        //FeatureRepository featureRepository = new FeatureRepository(0); 
            //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
	        QueryGroup CustomsHouseTypeAdditionalQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "CHQG", Name = "Customs.CustomsHouseTypeAdditional" }, queryGroupRepository,tenantQueryGroups);
						QueryGroup CustomsHouseTypeAdditionalQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "eaef", Name = " Query Group" }, queryGroupRepository,tenantQueryGroups);
				        queryGroupRepository.SubmitChanges();
	        ObjectTable CustomsHouseTypeAdditionalObjectTable = objectTables.ContainsKey("Customs.CustomsHouseTypeAdditional") ? objectTables["Customs.CustomsHouseTypeAdditional"] : null;
            if (CustomsHouseTypeAdditionalObjectTable == null)
            {
                IWebFreightContext objectContext = WebFreightContext.GetContext(0);  

                CustomsHouseTypeAdditionalObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Customs.CustomsHouseTypeAdditional" && d.Tenant == 0).FirstOrDefault();
            }

	         
			List<Feature> addedFeatures = new List<Feature>();
			List<TextCode> addedTextCodes = new List<TextCode>();
			List<Query> addedQueries = new List<Query>();
			List<QueryColumn> addedQueryColumns = new List<QueryColumn>();
			List<AdvancedQueryFilter> addedQueryFilters = new List<AdvancedQueryFilter>();
   

			   TextCode CustomsHouseTypeAdditionalTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.HouseTypeAdditional.Q.HouseTypeAdditionalQuery", DefaultText = @"Customs House Type Additional",LocalDefaultText = "סוג בית מכס נוסף", ObjectTableId = CustomsHouseTypeAdditionalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature CustomsHouseTypeAdditionalFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "HOUSETYPEADDITIONAL", ObjectTableId = CustomsHouseTypeAdditionalObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.CustomsHouseTypeAdditional.Features.CustomsSetting", NameTextCodeDefaultText = "Customs Settings", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,CustomsHouseTypeAdditionalObjectTable, addedFeatures, addedTextCodes);

	        //TextCodeRepository.SubmitChanges();
	        //FeaturesRepository.SubmitChanges();    
	      

			  Query CustomsHouseTypeAdditionalQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = CustomsHouseTypeAdditionalTextCode_0.Id, NameTextCodeCode = CustomsHouseTypeAdditionalTextCode_0.Code, ObjectTableName = "Customs.CustomsHouseTypeAdditional", Code = "CustomsHouseTypeAdditional",  QueryGroupCode = "CHQG", IndexOrder = 0, Tenant = 0, ObjectTableId = CustomsHouseTypeAdditionalObjectTable.Id, QuerySection = "CustomsHouseTypeAdditional", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = CustomsHouseTypeAdditionalFeature_0.Id,FeatureUniqeCode= CustomsHouseTypeAdditionalFeature_0.FeatureUniqeCode, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, addedQueries);
	
			 QueryColumn CustomsHouseTypeAdditionalQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomsHouseTypeAdditionalQuery.Id,QueryCode = CustomsHouseTypeAdditionalQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.CustomsHouseTypeAdditional.Code" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn CustomsHouseTypeAdditionalQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomsHouseTypeAdditionalQuery.Id,QueryCode = CustomsHouseTypeAdditionalQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.CustomsHouseTypeAdditional.Name" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn CustomsHouseTypeAdditionalQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomsHouseTypeAdditionalQuery.Id,QueryCode = CustomsHouseTypeAdditionalQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.CustomsHouseTypeAdditional.TransportModeName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn CustomsHouseTypeAdditionalQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomsHouseTypeAdditionalQuery.Id,QueryCode = CustomsHouseTypeAdditionalQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.CustomsHouseTypeAdditional.UnloadPortName" , ColumnWidth = 130 }, addedQueryColumns);
			SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
			SqlBulkInsert.BulkInsert("Features", addedFeatures);
			SqlBulkInsert.BulkInsert("Queries", addedQueries);
			SqlBulkInsert.BulkInsert("QueryColumns", addedQueryColumns);
			SqlBulkInsert.BulkInsert("AdvancedQueryFilters", addedQueryFilters);	 
  
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable CustomsHouseTypeAdditionalObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.CustomsHouseTypeAdditional" && d.Tenant == 0).FirstOrDefault();
		   //List<ObjectField> CustomsHouseTypeAdditionalObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Customs.CustomsHouseTypeAdditional").ToList();
		       
	      

	         Screen CustomsHouseTypeAdditionalHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Customs.HouseTypeAdditional.HeaderScreen", Name = "Header Screen", ObjectTableId = CustomsHouseTypeAdditionalObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      
             ScreenField CustomsCustomsHouseTypeAdditionalCustomsHouseTypeAdditionalHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ScreenId = CustomsHouseTypeAdditionalHeaderScreenScreen0.Id,ScreenCode = CustomsHouseTypeAdditionalHeaderScreenScreen0.Code, ObjectFieldCode = "Customs.CustomsHouseTypeAdditional.Code", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsCustomsHouseTypeAdditionalCustomsHouseTypeAdditionalHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ScreenId = CustomsHouseTypeAdditionalHeaderScreenScreen0.Id,ScreenCode = CustomsHouseTypeAdditionalHeaderScreenScreen0.Code, ObjectFieldCode = "Customs.CustomsHouseTypeAdditional.Name", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          	
		    CustomsHouseTypeAdditionalObjectTable.HeaderScreenId = CustomsHouseTypeAdditionalHeaderScreenScreen0.Id;
		    CustomsHouseTypeAdditionalObjectTable.HeaderScreenCode = CustomsHouseTypeAdditionalHeaderScreenScreen0.Code;

	   		  
	      

	         Screen CustomsHouseTypeAdditionalGeneralTabScreenScreen1 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Customs.CustomsHouseTypeAdditional.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = CustomsHouseTypeAdditionalObjectTable.Id, NumberOfColumns = 1, NumberOfRows = 3, IsReadOnly = false }, screensRepository, tenantScreens);
      
             ScreenField CustomsCustomsHouseTypeAdditionalCustomsCustomsHouseTypeAdditionalGeneralTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ScreenId = CustomsHouseTypeAdditionalGeneralTabScreenScreen1.Id,ScreenCode = CustomsHouseTypeAdditionalGeneralTabScreenScreen1.Code, ObjectFieldCode = "Customs.CustomsHouseTypeAdditional.Code", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsCustomsHouseTypeAdditionalCustomsCustomsHouseTypeAdditionalGeneralTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ScreenId = CustomsHouseTypeAdditionalGeneralTabScreenScreen1.Id,ScreenCode = CustomsHouseTypeAdditionalGeneralTabScreenScreen1.Code, ObjectFieldCode = "Customs.CustomsHouseTypeAdditional.TransportModeId", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsCustomsHouseTypeAdditionalCustomsCustomsHouseTypeAdditionalGeneralTabScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ScreenId = CustomsHouseTypeAdditionalGeneralTabScreenScreen1.Id,ScreenCode = CustomsHouseTypeAdditionalGeneralTabScreenScreen1.Code, ObjectFieldCode = "Customs.CustomsHouseTypeAdditional.UnloadPortCode", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	            

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable CustomsHouseTypeAdditionalObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.CustomsHouseTypeAdditional" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode CustomsHouseTypeAdditionalGeneralTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsHouseTypeAdditional.TH.General", DefaultText = "General",LocalDefaultText = "כללי", ObjectTableId = CustomsHouseTypeAdditionalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CustomsHouseTypeAdditionalGeneralFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GENERAL", ObjectTableId = CustomsHouseTypeAdditionalObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.CustomsHouseTypeAdditional.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,CustomsHouseTypeAdditionalObjectTable);
 
                 
			   TextCode CustomsHouseTypeAdditionalEventsTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsHouseTypeAdditional.TH.Events", DefaultText = "Events",LocalDefaultText = "אירועים", ObjectTableId = CustomsHouseTypeAdditionalObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CustomsHouseTypeAdditionalEventsFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EVENTS", ObjectTableId = CustomsHouseTypeAdditionalObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.CustomsHouseTypeAdditional.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,CustomsHouseTypeAdditionalObjectTable);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "HAGN",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = CustomsHouseTypeAdditionalGeneralFeature_TH0.Id,FeatureUniqeCode = CustomsHouseTypeAdditionalGeneralFeature_TH0.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.GeneralControls.GeneralTabControl", ObjectTableId = CustomsHouseTypeAdditionalObjectTable.Id, TabNameTextCodeId = CustomsHouseTypeAdditionalGeneralTextCode_TH0.Id, TabNameTextCodeCode = CustomsHouseTypeAdditionalGeneralTextCode_TH0.Code, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CHTE",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = CustomsHouseTypeAdditionalEventsFeature_TH1.Id,FeatureUniqeCode = CustomsHouseTypeAdditionalEventsFeature_TH1.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = CustomsHouseTypeAdditionalObjectTable.Id, TabNameTextCodeId = CustomsHouseTypeAdditionalEventsTextCode_TH1.Id, TabNameTextCodeCode = CustomsHouseTypeAdditionalEventsTextCode_TH1.Code, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable CustomsHouseTypeAdditionalObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.CustomsHouseTypeAdditional" && d.Tenant == 0).FirstOrDefault(); 

		   Feature CustomsHouseTypeAdditionalFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = CustomsHouseTypeAdditionalObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomsHouseTypeAdditional.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,CustomsHouseTypeAdditionalObjectTable);
		   Feature CustomsHouseTypeAdditionalFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = CustomsHouseTypeAdditionalObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomsHouseTypeAdditional.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,CustomsHouseTypeAdditionalObjectTable);
		   Feature CustomsHouseTypeAdditionalFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = CustomsHouseTypeAdditionalObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomsHouseTypeAdditional.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,CustomsHouseTypeAdditionalObjectTable);
		   Feature CustomsHouseTypeAdditionalFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = false, ObjectTableId = CustomsHouseTypeAdditionalObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomsHouseTypeAdditional.Features.PackageFeature", NameTextCodeDefaultText = "CustomsHouseTypeAdditional Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,CustomsHouseTypeAdditionalObjectTable);    
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable CustomsHouseTypeAdditionalObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.CustomsHouseTypeAdditional" && d.Tenant == 0).FirstOrDefault(); 
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
                ObjectTableId = CustomsHouseTypeAdditionalObjectTable.Id,
				 
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
                ObjectTableId = CustomsHouseTypeAdditionalObjectTable.Id,
				 
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
	 