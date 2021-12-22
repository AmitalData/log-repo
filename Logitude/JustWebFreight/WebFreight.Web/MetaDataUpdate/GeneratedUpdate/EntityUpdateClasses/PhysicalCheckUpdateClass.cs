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

//using Amital.QuoteOPM.BL.CLoseTable;
using Amital.QuoteOPM.Data.Repsitories;
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.BL;


namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.EntityUpdateClasses
{
   public class PhysicalCheckUpdateClass
   {  		
		public const string HashString = "e47b965f0f2176d4a702b5cb5e3308c4";
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "Customs.PhysicalCheck",
			      				    IsNew =  false,
			      				    DBTableName =  "Customs.PhysicalChecks",
			      				    ObjectTableSingular =  "Customs.PhysicalCheck",
			      				    ObjectTablePlural =  "Customs.PhysicalChecks",
			      				    HasCustomFilter =  false,
			      				    HasCustomFields =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasFiltersMenu =  true,
			      				    IsEditable =  false,
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
			      				    SortingByObjectField =  "Id",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    LocalDefaultText =  "בדיקה פיזית",
			      				    DefaultText =  "Physical Check",
			      				    Code =  "PHCK",
			      				    Name =  "Customs.PhysicalCheck",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Customs",
			      				    ServerModuleName =  "Customs",
			      				    NoTS =  false,
			      				    HasMenuButtons =  true,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    IsTabsHidden =  false,
			      				    HashString =  PhysicalCheckUpdateClass.HashString,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository, List<ObjectField> addedFields, List<TextCode> addedTextCodes)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DeclarationId",
					  						ObjectTableName =  "Customs.PhysicalCheck",
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
					  						ValidForQuerySection1 =  "PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DeclarationId",
					  						DefaultText =  "DeclarationId",
					  						FullLocalDefaultText =  "DeclarationId",
					  						ListFieldLable =  "DeclarationIdListLable",
					  						ListLableDefaultText =  "DeclarationId",
					  						ListLocalDefaultText =  "DeclarationId",
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
					 
					 						FieldName =  "StorageSiteCode",
					  						ObjectTableName =  "Customs.PhysicalCheck",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.SiteLookup",
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "StorageSiteCode",
					  						ListPropertyPath =  "StorageSiteCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "StorageSiteCode",
					  						DefaultText =  "Storage Site",
					  						FullLocalDefaultText =  "םתר םחסון",
					  						ListFieldLable =  "StorageSiteCodeListLable",
					  						ListLableDefaultText =  "Storage Site ",
					  						ListLocalDefaultText =  "םתר םחסון",
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
					 
					 						FieldName =  "StorageSiteName",
					  						ObjectTableName =  "Customs.PhysicalCheck",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "StorageSiteName",
					  						ListPropertyPath =  "StorageSiteName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "StorageSiteName",
					  						DefaultText =  "Storage Site Name",
					  						FullLocalDefaultText =  "םתר םחסון",
					  						ListFieldLable =  "StorageSiteNameListLable",
					  						ListLableDefaultText =  "Storage Site Name",
					  						ListLocalDefaultText =  "םתר םחסון",
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
					 
					 						FieldName =  "CheckSiteCode",
					  						ObjectTableName =  "Customs.PhysicalCheck",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.SiteLookup",
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CheckSiteCode",
					  						ListPropertyPath =  "CheckSiteCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CheckSiteCode",
					  						DefaultText =  "Check Site",
					  						FullLocalDefaultText =  "םתר בדיקה",
					  						ListFieldLable =  "CheckSiteCodeListLable",
					  						ListLableDefaultText =  "Check Site ",
					  						ListLocalDefaultText =  "םתר בדיקה",
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
					 
					 						FieldName =  "CheckSiteName",
					  						ObjectTableName =  "Customs.PhysicalCheck",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CheckSiteName",
					  						ListPropertyPath =  "CheckSiteName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CheckSiteName",
					  						DefaultText =  "Check Site Name",
					  						FullLocalDefaultText =  "םתר בדיקה",
					  						ListFieldLable =  "CheckSiteNameListLable",
					  						ListLableDefaultText =  "Check Site Name",
					  						ListLocalDefaultText =  "םתר בדיקה",
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
					 
					 						FieldName =  "QueueTypeCode",
					  						ObjectTableName =  "Customs.PhysicalCheck",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.CheckQueueType",
					  						MinLength =  0,
					  						MaxLength =  4,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  4,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "QueueTypeCode",
					  						ListPropertyPath =  "QueueTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "QueueTypeCode",
					  						DefaultText =  "Queue Type",
					  						FullLocalDefaultText =  "סוג תור",
					  						ListFieldLable =  "QueueTypeCodeListLable",
					  						ListLableDefaultText =  "Queue Type ",
					  						ListLocalDefaultText =  "סוג תור",
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
					 
					 						FieldName =  "QueueTypeName",
					  						ObjectTableName =  "Customs.PhysicalCheck",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "QueueTypeName",
					  						ListPropertyPath =  "QueueTypeName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "QueueTypeName",
					  						DefaultText =  "Queue Type Name",
					  						FullLocalDefaultText =  "סוג תור",
					  						ListFieldLable =  "QueueTypeNameListLable",
					  						ListLableDefaultText =  "Queue Type Name",
					  						ListLocalDefaultText =  "סוג תור",
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
					 
					 						FieldName =  "OperationCode",
					  						ObjectTableName =  "Customs.PhysicalCheck",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.PhysicalCheckOperation",
					  						MinLength =  0,
					  						MaxLength =  4,
					  						IsRequired =  false,
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
					  						PMPropertyPath =  "OperationCode",
					  						ListPropertyPath =  "OperationCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "OperationCode",
					  						DefaultText =  "Operation",
					  						FullLocalDefaultText =  "קוד פעולה",
					  						ListFieldLable =  "OperationCodeListLable",
					  						ListLableDefaultText =  "Operation ",
					  						ListLocalDefaultText =  "קוד פעולה",
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
					 
					 						FieldName =  "CheckId",
					  						ObjectTableName =  "Customs.PhysicalCheck",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  9,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  9,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CheckId",
					  						ListPropertyPath =  "CheckId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CheckId",
					  						DefaultText =  "Check ",
					  						FullLocalDefaultText =  "מספר בדיקה",
					  						ListFieldLable =  "CheckIdListLable",
					  						ListLableDefaultText =  "Check ",
					  						ListLocalDefaultText =  "מספר בדיקה",
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
					 
					 						FieldName =  "EntityTypeId",
					  						ObjectTableName =  "Customs.PhysicalCheck",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "EntityTypeId",
					  						ListPropertyPath =  "EntityTypeId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "EntityTypeId",
					  						DefaultText =  "Entity Type ",
					  						ListFieldLable =  "EntityTypeIdListLable",
					  						ListLableDefaultText =  "EntityType",
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
					 
					 						FieldName =  "ContainerNubmer",
					  						ObjectTableName =  "Customs.PhysicalCheck",
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
					  						PMPropertyPath =  "ContainerNubmer",
					  						ListPropertyPath =  "ContainerNubmer",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ContainerNubmer",
					  						DefaultText =  "Container Nubmer",
					  						FullLocalDefaultText =  "מספר מכולה",
					  						ListFieldLable =  "ContainerNubmerListLable",
					  						ListLableDefaultText =  "Container Nubmer",
					  						ListLocalDefaultText =  "מספר מכולה",
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
					 
					 						FieldName =  "OpenDate",
					  						ObjectTableName =  "Customs.PhysicalCheck",
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
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToRoutingString",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "OpenDate",
					  						ListPropertyPath =  "OpenDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "OpenDate",
					  						DefaultText =  "Open Date",
					  						FullLocalDefaultText =  "תםריך הפניה לבדיקה",
					  						ListFieldLable =  "OpenDateListLable",
					  						ListLableDefaultText =  "Open Date",
					  						ListLocalDefaultText =  "תםריך הפניה לבדיקה",
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
					 
					 						FieldName =  "LimitDate",
					  						ObjectTableName =  "Customs.PhysicalCheck",
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
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToRoutingString",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "LimitDate",
					  						ListPropertyPath =  "LimitDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LimitDate",
					  						DefaultText =  "Limit Date",
					  						FullLocalDefaultText =  "תםריך הבדיקה",
					  						ListFieldLable =  "LimitDateListLable",
					  						ListLableDefaultText =  "Limit Date",
					  						ListLocalDefaultText =  "תםריך הבדיקה",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CargoIdentifierKey1",
					  						ObjectTableName =  "Customs.PhysicalCheck",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  35,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  35,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CargoIdentifierKey1",
					  						ListPropertyPath =  "CargoIdentifierKey1",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CargoIdentifierKey1",
					  						DefaultText =  "Cargo Identifier Key1",
					  						FullLocalDefaultText =  "מזהה מטען 1",
					  						ListFieldLable =  "CargoIdentifierKey1ListLable",
					  						ListLableDefaultText =  "Cargo Identifier Key1",
					  						ListLocalDefaultText =  "מזהה מטען 1",
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
					 
					 						FieldName =  "CargoIdentifierKey2",
					  						ObjectTableName =  "Customs.PhysicalCheck",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  35,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  35,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CargoIdentifierKey2",
					  						ListPropertyPath =  "CargoIdentifierKey2",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CargoIdentifierKey2",
					  						DefaultText =  "Cargo Identifier Key2",
					  						FullLocalDefaultText =  "מזהה מטען 2",
					  						ListFieldLable =  "CargoIdentifierKey2ListLable",
					  						ListLableDefaultText =  "Cargo Identifier Key2",
					  						ListLocalDefaultText =  "מזהה מטען 2",
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
					 
					 						FieldName =  "CargoIdentifierKey3",
					  						ObjectTableName =  "Customs.PhysicalCheck",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  35,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  35,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CargoIdentifierKey3",
					  						ListPropertyPath =  "CargoIdentifierKey3",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CargoIdentifierKey3",
					  						DefaultText =  "Cargo Identifier Key3",
					  						FullLocalDefaultText =  "מזהה מטען 3",
					  						ListFieldLable =  "CargoIdentifierKey3ListLable",
					  						ListLableDefaultText =  "Cargo Identifier Key3",
					  						ListLocalDefaultText =  "מזהה מטען 3",
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
					 
					 						FieldName =  "RowNumber",
					  						ObjectTableName =  "Customs.PhysicalCheck",
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
					  						SystemMaxLength =  4,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "RowNumber",
					  						ListPropertyPath =  "RowNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "RowNumber",
					  						DefaultText =  "Row Number",
					  						FullLocalDefaultText =  "סידורי במטען",
					  						ListFieldLable =  "RowNumberListLable",
					  						ListLableDefaultText =  "Row Number",
					  						ListLocalDefaultText =  "סידורי במטען",
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
					 
					 						FieldName =  "CheckEssence",
					  						ObjectTableName =  "Customs.PhysicalCheck",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  255,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  255,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CheckEssence",
					  						ListPropertyPath =  "CheckEssence",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CheckEssence",
					  						DefaultText =  "Check Essence",
					  						FullLocalDefaultText =  "מהות הבדיקה",
					  						ListFieldLable =  "CheckEssenceListLable",
					  						ListLableDefaultText =  "Check Essence",
					  						ListLocalDefaultText =  "מהות הבדיקה",
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
					 
					 						FieldName =  "IsClosed",
					  						ObjectTableName =  "Customs.PhysicalCheck",
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
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.BooleanToStringConverter",
					  						DataTemplateName =  "IsClosedDataTemplate",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
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
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsClosed",
					  						DefaultText =  "Is Closed",
					  						FullLocalDefaultText =  "סגורה",
					  						ListFieldLable =  "IsClosedListLable",
					  						ListLableDefaultText =  "Is Closed",
					  						ListLocalDefaultText =  "סגורה",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SearchFields",
					  						ObjectTableName =  "Customs.PhysicalCheck",
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
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						ValidForQuerySection2 =  "Customs.PhysicalCheckFollowUp",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SearchFields",
					  						DefaultText =  "Search ",
					  						FullLocalDefaultText =  "מס' תיק / הצהרה ,  בדיקה , מכולה , מזהי מטען",
					  						ListFieldLable =  "SearchFieldsListLable",
					  						ListLableDefaultText =  "Search Field",
					  						HelpTextCode =  "SearchFields",
					  						HelpTextDefaultText =  "Searching by :\n1:  Declaration No \n2: Customer Name \n3: Container No ",
					  						HelpLocalDefaultText =  "חיפוש על ידי :\n1: הצהרה לם \n2: שם לקוח \n3: המכל לם",
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
					 
					 						FieldName =  "StatusMessageCode",
					  						ObjectTableName =  "Customs.PhysicalCheck",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.PhysicalCheckStatusMessage",
					  						MinLength =  0,
					  						MaxLength =  4,
					  						IsRequired =  false,
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
					  						PMPropertyPath =  "StatusMessageCode",
					  						ListPropertyPath =  "StatusMessageCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "StatusMessageCode",
					  						DefaultText =  "Status Message",
					  						FullLocalDefaultText =  "סוג הודעה",
					  						ListFieldLable =  "StatusMessageCodeListLable",
					  						ListLableDefaultText =  "Status Message ",
					  						ListLocalDefaultText =  "סוג הודעה",
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
					 
					 						FieldName =  "CargoTypeCode",
					  						ObjectTableName =  "Customs.PhysicalCheck",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.CheckEntityType",
					  						MinLength =  0,
					  						MaxLength =  4,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  4,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CargoTypeCode",
					  						ListPropertyPath =  "CargoTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CargoTypeCode",
					  						DefaultText =  "Cargo Type",
					  						FullLocalDefaultText =  "סוג מטען",
					  						ListFieldLable =  "CargoTypeCodeListLable",
					  						ListLableDefaultText =  "Cargo Type ",
					  						ListLocalDefaultText =  "סוג מטען",
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
					 
					 						FieldName =  "InitiatorTypeCode",
					  						ObjectTableName =  "Customs.PhysicalCheck",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.CheckRepresentativeType",
					  						MinLength =  0,
					  						MaxLength =  4,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  4,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "InitiatorTypeCode",
					  						ListPropertyPath =  "InitiatorTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "InitiatorTypeCode",
					  						DefaultText =  "Initiator Type",
					  						FullLocalDefaultText =  "יוזם הבדיקה",
					  						ListFieldLable =  "InitiatorTypeCodeListLable",
					  						ListLableDefaultText =  "Initiator Type ",
					  						ListLocalDefaultText =  "יוזם הבדיקה",
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
					 
					 						FieldName =  "ImporterNumber",
					  						ObjectTableName =  "Customs.PhysicalCheck",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  9,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  9,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ImporterNumber",
					  						ListPropertyPath =  "ImporterNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ImporterNumber",
					  						DefaultText =  "Importer Number",
					  						FullLocalDefaultText =  "מספר יבוםן",
					  						ListFieldLable =  "ImporterNumberListLable",
					  						ListLableDefaultText =  "Importer Number",
					  						ListLocalDefaultText =  "מספר יבוםן",
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
					 
					 						FieldName =  "CargoIdentifierTypeCode",
					  						ObjectTableName =  "Customs.PhysicalCheck",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.CargoIdentifireType",
					  						MinLength =  0,
					  						MaxLength =  4,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  4,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CargoIdentifierTypeCode",
					  						ListPropertyPath =  "CargoIdentifierTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CargoIdentifierTypeCode",
					  						DefaultText =  "Cargo Identifier Type",
					  						FullLocalDefaultText =  "מזהה מטען",
					  						ListFieldLable =  "CargoIdentifierTypeCodeListLable",
					  						ListLableDefaultText =  "Cargo Identifier Type ",
					  						ListLocalDefaultText =  "מזהה מטען",
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
					 
					 						FieldName =  "OperationName",
					  						ObjectTableName =  "Customs.PhysicalCheck",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "OperationName",
					  						ListPropertyPath =  "OperationName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "OperationName",
					  						DefaultText =  "Operation Name",
					  						FullLocalDefaultText =  "קוד פעולה",
					  						ListFieldLable =  "OperationNameListLable",
					  						ListLableDefaultText =  "Operation Name",
					  						ListLocalDefaultText =  "קוד פעולה",
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
					 
					 						FieldName =  "DeclarationNo",
					  						ObjectTableName =  "Customs.PhysicalCheck",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  35,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  35,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DeclarationNo",
					  						ListPropertyPath =  "DeclarationNo",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DeclarationNo",
					  						DefaultText =  "Declaration No",
					  						FullLocalDefaultText =  "מספר הצהרה",
					  						ListFieldLable =  "DeclarationNoListLable",
					  						ListLableDefaultText =  "Declaration No",
					  						ListLocalDefaultText =  "מספר הצהרה",
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
					 
					 						FieldName =  "CargoIdentifierTypeName",
					  						ObjectTableName =  "Customs.PhysicalCheck",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CargoIdentifierTypeName",
					  						ListPropertyPath =  "CargoIdentifierTypeName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CargoIdentifierTypeName",
					  						DefaultText =  "Cargo Identifier Type Name",
					  						FullLocalDefaultText =  "סוג מזהה מטען",
					  						ListFieldLable =  "CargoIdentifierTypeNameListLable",
					  						ListLableDefaultText =  "Cargo Identifier Type Name",
					  						ListLocalDefaultText =  "סוג מזהה מטען",
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
					 
					 						FieldName =  "CheckSiteId",
					  						ObjectTableName =  "Customs.PhysicalCheck",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CheckSiteId",
					  						ListPropertyPath =  "CheckSiteId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CheckSiteId",
					  						DefaultText =  "Check Site",
					  						ListFieldLable =  "CheckSiteIdListLable",
					  						ListLableDefaultText =  "CheckSite",
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
					 
					 						FieldName =  "CustomerCode",
					  						ObjectTableName =  "Customs.PhysicalCheck",
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
					  						SystemMaxLength =  4,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
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
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CustomerCode",
					  						DefaultText =  "Customer ",
					  						FullLocalDefaultText =  "קוד לקוח",
					  						ListFieldLable =  "CustomerCodeListLable",
					  						ListLableDefaultText =  "Customer ",
					  						ListLocalDefaultText =  "קוד לקוח",
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
					 
					 						FieldName =  "CustomerName",
					  						ObjectTableName =  "Customs.PhysicalCheck",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CustomerName",
					  						ListPropertyPath =  "CustomerName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CustomerName",
					  						DefaultText =  "Customer Name",
					  						FullLocalDefaultText =  "שם לקוח",
					  						ListFieldLable =  "CustomerNameListLable",
					  						ListLableDefaultText =  "Customer Name",
					  						ListLocalDefaultText =  "שם לקוח",
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
					 
					 						FieldName =  "CustomFileNo",
					  						ObjectTableName =  "Customs.PhysicalCheck",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  12,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  12,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CustomFileNo",
					  						ListPropertyPath =  "CustomFileNo",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CustomFileNo",
					  						DefaultText =  "Custom File No",
					  						FullLocalDefaultText =  "תיק עמילות/מכס",
					  						ListFieldLable =  "CustomFileNoListLable",
					  						ListLableDefaultText =  "Custom File No",
					  						ListLocalDefaultText =  "תיק עמילות/מכס",
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
					 
					 						FieldName =  "StatusMessageName",
					  						ObjectTableName =  "Customs.PhysicalCheck",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "StatusMessageName",
					  						ListPropertyPath =  "StatusMessageName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "StatusMessageName",
					  						DefaultText =  "Status Message Name",
					  						FullLocalDefaultText =  "סוג הודעה",
					  						ListFieldLable =  "StatusMessageNameListLable",
					  						ListLableDefaultText =  "Status Message Name",
					  						ListLocalDefaultText =  "סוג הודעה",
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
					 
					 						FieldName =  "ConcurrencyGUID",
					  						ObjectTableName =  "Customs.PhysicalCheck",
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
					  						PMPropertyPath =  "ConcurrencyGUID",
					  						ListPropertyPath =  "ConcurrencyGUID",
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
					  						FullFieldLable =  "ConcurrencyGUID",
					  						DefaultText =  "ConcurrencyGUID",
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
					 
					 						FieldName =  "NewConcurrencyGUID",
					  						ObjectTableName =  "Customs.PhysicalCheck",
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
					  						PMPropertyPath =  "NewConcurrencyGUID",
					  						ListPropertyPath =  "NewConcurrencyGUID",
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
					  						FullFieldLable =  "NewConcurrencyGUID",
					  						DefaultText =  "NewConcurrencyGUID",
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
					 
					 						FieldName =  "IsComprehensiveCheck",
					  						ObjectTableName =  "Customs.PhysicalCheck",
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
					  						PMPropertyPath =  "IsComprehensiveCheck",
					  						ListPropertyPath =  "IsComprehensiveCheck",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsComprehensiveCheck",
					  						DefaultText =  "Is Comprehensive Check",
					  						FullLocalDefaultText =  "בדיקה מקיפה",
					  						ListFieldLable =  "IsComprehensiveCheckListLable",
					  						ListLableDefaultText =  "Is Comprehensive Check",
					  						ListLocalDefaultText =  "בדיקה מקיפה",
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
					 
					 						FieldName =  "CheckTypeCode",
					  						ObjectTableName =  "Customs.PhysicalCheck",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.CheckTypeLookup",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  false,
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
					  						PMPropertyPath =  "CheckTypeCode",
					  						ListPropertyPath =  "CheckTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CheckTypeCode",
					  						DefaultText =  "Check Type",
					  						FullLocalDefaultText =  "סוג בדיקה",
					  						ListFieldLable =  "CheckTypeCodeListLable",
					  						ListLableDefaultText =  "Check Type",
					  						ListLocalDefaultText =  "סוג בדיקה",
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
					 
					 						FieldName =  "CheckTypeName",
					  						ObjectTableName =  "Customs.PhysicalCheck",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CheckTypeName",
					  						ListPropertyPath =  "CheckTypeName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CheckTypeName",
					  						DefaultText =  "Check Type Name",
					  						FullLocalDefaultText =  "סוג בדיקה",
					  						ListFieldLable =  "CheckTypeNameListLable",
					  						ListLableDefaultText =  "Check Type Name",
					  						ListLocalDefaultText =  "סוג בדיקה",
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
					 
					 						FieldName =  "CustomerId",
					  						ObjectTableName =  "Customs.PhysicalCheck",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Card",
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
					  						PMPropertyPath =  "CustomerId",
					  						ListPropertyPath =  "CustomerId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CustomerId",
					  						DefaultText =  "Customer ",
					  						FullLocalDefaultText =  "לקוח",
					  						ListFieldLable =  "CustomerIdListLable",
					  						ListLableDefaultText =  "Customer",
					  						ListLocalDefaultText =  "לקוח",
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
					 
					 						FieldName =  "NoEscortRequired",
					  						ObjectTableName =  "Customs.PhysicalCheck",
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
					  						PMPropertyPath =  "NoEscortRequired",
					  						ListPropertyPath =  "NoEscortRequired",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "NoEscortRequired",
					  						DefaultText =  "No Escort Required",
					  						FullLocalDefaultText =  "ללם נוכחות בודק",
					  						ListFieldLable =  "NoEscortRequiredListLable",
					  						ListLableDefaultText =  "No Escort Required",
					  						ListLocalDefaultText =  "ללם נוכחות בודק",
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
					 
					 						FieldName =  "VehicleChassisNumber",
					  						ObjectTableName =  "Customs.PhysicalCheck",
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
					  						SystemMaxLength =  20,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "VehicleChassisNumber",
					  						ListPropertyPath =  "VehicleChassisNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "VehicleChassisNumber",
					  						DefaultText =  "Vehicle Chassis Number",
					  						FullLocalDefaultText =  "מס' שלדה",
					  						ListFieldLable =  "VehicleChassisNumberListLable",
					  						ListLableDefaultText =  "Vehicle Chassis Number",
					  						ListLocalDefaultText =  "מס' שלדה",
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
					 
					 						FieldName =  "EndDate",
					  						ObjectTableName =  "Customs.PhysicalCheck",
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
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToRoutingString",
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "EndDate",
					  						ListPropertyPath =  "EndDate",
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
					  						FullFieldLable =  "EndDate",
					  						DefaultText =  "End Date",
					  						FullLocalDefaultText =  "מועד סיום הבדיקה",
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
					 
					 						FieldName =  "SearchResult",
					  						ObjectTableName =  "Customs.PhysicalCheck",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.PhysicalCheckSearchResultType",
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "SearchResult",
					  						ListPropertyPath =  "SearchResult",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SearchResult",
					  						DefaultText =  "SearchResult",
					  						FullLocalDefaultText =  "תוצםות הבדיקה",
					  						ListFieldLable =  "SearchResultListLable",
					  						ListLableDefaultText =  "SearchResult",
					  						ListLocalDefaultText =  "תוצםות הבדיקה",
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
					 
					 						FieldName =  "SealNumber",
					  						ObjectTableName =  "Customs.PhysicalCheck",
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
					  						SystemMaxLength =  20,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "SealNumber",
					  						ListPropertyPath =  "SealNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SealNumber",
					  						DefaultText =  "Seal Number",
					  						FullLocalDefaultText =  "מס' סגר",
					  						ListFieldLable =  "SealNumberListLable",
					  						ListLableDefaultText =  "Seal Number",
					  						ListLocalDefaultText =  "מס' סגר",
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
					 
					 						FieldName =  "CheckAuthorityAttenderTypeID",
					  						ObjectTableName =  "Customs.PhysicalCheck",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.Authority",
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CheckAuthorityAttenderTypeID",
					  						ListPropertyPath =  "CheckAuthorityAttenderTypeID",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CheckAuthorityAttenderTypeID",
					  						DefaultText =  "Check Authority Attender Type ID",
					  						FullLocalDefaultText =  "רשות מוסמכת",
					  						ListFieldLable =  "CheckAuthorityAttenderTypeIDListLable",
					  						ListLableDefaultText =  "CheckAuthorityAttenderTypeID",
					  						ListLocalDefaultText =  "רשות מוסמכת",
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
					 
					 						FieldName =  "CheckAuthorityAttenderTypeName",
					  						ObjectTableName =  "Customs.PhysicalCheck",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  255,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  255,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CheckAuthorityAttenderTypeName",
					  						ListPropertyPath =  "CheckAuthorityAttenderTypeName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CheckAuthorityAttenderTypeName",
					  						DefaultText =  "CheckAuthorityAttenderTypeName",
					  						FullLocalDefaultText =  "שם נציג רשות מוסמכת",
					  						ListFieldLable =  "CheckAuthorityAttenderTypeNameListLable",
					  						ListLableDefaultText =  "CheckAuthorityAttenderTypeName",
					  						ListLocalDefaultText =  "שם נציג רשות מוסמכת",
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
					 
					 						FieldName =  "CheckAnwserStatus",
					  						ObjectTableName =  "Customs.PhysicalCheck",
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
					  						PMPropertyPath =  "CheckAnwserStatus",
					  						ListPropertyPath =  "CheckAnwserStatus",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CheckAnwserStatus",
					  						DefaultText =  "CheckAnwserStatus",
					  						FullLocalDefaultText =  "סטטוס בקשה",
					  						ListFieldLable =  "CheckAnwserStatusListLable",
					  						ListLableDefaultText =  "CheckAnwserStatus",
					  						ListLocalDefaultText =  "סטטוס בקשה",
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
					 
					 						FieldName =  "MyCloseCheckBox",
					  						ObjectTableName =  "Customs.PhysicalCheck",
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
					  						PMPropertyPath =  "MyCloseCheckBox",
					  						ListPropertyPath =  "MyCloseCheckBox",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "MyCloseCheckBox",
					  						DefaultText =  "סימון לסגירה גורפת",
					  						FullLocalDefaultText =  "סימון לסגירה גורפת",
					  						ListFieldLable =  "MyCloseCheckBoxListLable",
					  						ListLableDefaultText =  "MyCloseCheckBox",
					  						ListLocalDefaultText =  "סימון לסגירה גורפת",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TransportModeId",
					  						ObjectTableName =  "Customs.PhysicalCheck",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.CustomsTransportMode",
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
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TransportModeId",
					  						DefaultText =  "Transport Mode ",
					  						ListFieldLable =  "TransportModeIdListLable",
					  						ListLableDefaultText =  "Transport Mode ",
					  						ListLocalDefaultText =  "סוג הובלה",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Direction",
					  						ObjectTableName =  "Customs.PhysicalCheck",
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Direction",
					  						ListPropertyPath =  "Direction",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.PhysicalCheck",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Direction",
					  						DefaultText =  "Direction",
					  						ListFieldLable =  "DirectionListLable",
					  						ListLableDefaultText =  "Direction",
					  						ListLocalDefaultText =  "כיוון ",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters,Dictionary<string, QueryGroup> tenantQueryGroups )
	    {  
	        //FeatureRepository featureRepository = new FeatureRepository(0); 
            //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
	        QueryGroup PhysicalCheckQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "PHCK", Name = "Customs.PhysicalCheck" }, queryGroupRepository,tenantQueryGroups);
						QueryGroup PhysicalCheckQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "06a2", Name = " Query Group" }, queryGroupRepository,tenantQueryGroups);
				        queryGroupRepository.SubmitChanges();
	        ObjectTable PhysicalCheckObjectTable = objectTables.ContainsKey("Customs.PhysicalCheck") ? objectTables["Customs.PhysicalCheck"] : null;
            if (PhysicalCheckObjectTable == null)
            {
                IWebFreightContext objectContext = WebFreightContext.GetContext(0);  

                PhysicalCheckObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Customs.PhysicalCheck" && d.Tenant == 0).FirstOrDefault();
            }

	         
			List<Feature> addedFeatures = new List<Feature>();
			List<TextCode> addedTextCodes = new List<TextCode>();
			List<Query> addedQueries = new List<Query>();
			List<QueryColumn> addedQueryColumns = new List<QueryColumn>();
			List<AdvancedQueryFilter> addedQueryFilters = new List<AdvancedQueryFilter>();
   

			   TextCode PhysicalCheckTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.Q.OpenChecks", DefaultText = @"Open Checks",LocalDefaultText = "בדיקות פתוחות", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature PhysicalCheckFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PhysicalCheck.Q.OpenChecks", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, NameTextCodeCode = "PhysicalCheckFeatures.OpenChecks", NameTextCodeDefaultText = "OpenChecks", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,PhysicalCheckObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode PhysicalCheckTextCode_1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.Q.ClosedChecks", DefaultText = @"Closed Checks",LocalDefaultText = "בדיקות סגורות", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature PhysicalCheckFeature_1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PhysicalCheck.Q.ClosedChecks", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, NameTextCodeCode = "PhysicalCheckFeatures.ClosedChecks", NameTextCodeDefaultText = "ClosedChecks", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,PhysicalCheckObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode PhysicalCheckTextCode_2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.Q.AllChecks", DefaultText = @"All Checks",LocalDefaultText = "כל הבדיקות", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature PhysicalCheckFeature_2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PhysicalCheck.Q.ByUpcomingChecks", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, NameTextCodeCode = "PhysicalCheckFeatures.ByUpcomingChecks", NameTextCodeDefaultText = "By Upcoming Checks", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,PhysicalCheckObjectTable, addedFeatures, addedTextCodes);

	        //TextCodeRepository.SubmitChanges();
	        //FeaturesRepository.SubmitChanges();    
	      

			  Query OpenChecksQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = PhysicalCheckTextCode_0.Id, NameTextCodeCode = PhysicalCheckTextCode_0.Code, ObjectTableName = "Customs.PhysicalCheck", Code = "OpenChecks",  QueryGroupCode = "PHCK", IndexOrder = 0, Tenant = 0, ObjectTableId = PhysicalCheckObjectTable.Id, QuerySection = "Customs.PhysicalCheck", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = PhysicalCheckFeature_0.Id,FeatureUniqeCode= PhysicalCheckFeature_0.FeatureUniqeCode, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, addedQueries);
	
			 QueryColumn OpenChecksQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenChecksQuery.Id,QueryCode = OpenChecksQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.PhysicalCheck.MyCloseCheckBox" , ColumnWidth = 40 }, addedQueryColumns);

			 QueryColumn OpenChecksQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenChecksQuery.Id,QueryCode = OpenChecksQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.PhysicalCheck.CustomFileNo" , ColumnWidth = 90 }, addedQueryColumns);

			 QueryColumn OpenChecksQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenChecksQuery.Id,QueryCode = OpenChecksQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.PhysicalCheck.DeclarationNo" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn OpenChecksQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenChecksQuery.Id,QueryCode = OpenChecksQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.PhysicalCheck.CustomerName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn OpenChecksQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenChecksQuery.Id,QueryCode = OpenChecksQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.PhysicalCheck.StorageSiteName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn OpenChecksQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenChecksQuery.Id,QueryCode = OpenChecksQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Customs.PhysicalCheck.CheckSiteName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn OpenChecksQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenChecksQuery.Id,QueryCode = OpenChecksQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Customs.PhysicalCheck.QueueTypeName" , ColumnWidth = 70 }, addedQueryColumns);

			 QueryColumn OpenChecksQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenChecksQuery.Id,QueryCode = OpenChecksQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "Customs.PhysicalCheck.CheckId" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn OpenChecksQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenChecksQuery.Id,QueryCode = OpenChecksQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "Customs.PhysicalCheck.LimitDate" , ColumnWidth = 110 }, addedQueryColumns);

			 QueryColumn OpenChecksQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenChecksQuery.Id,QueryCode = OpenChecksQuery.UniqueCode, IndexOrder = 9, ObjectFieldCode = "Customs.PhysicalCheck.Direction" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn OpenChecksQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenChecksQuery.Id,QueryCode = OpenChecksQuery.UniqueCode, IndexOrder = 10, ObjectFieldCode = "Customs.PhysicalCheck.TransportModeId" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn OpenChecksQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenChecksQuery.Id,QueryCode = OpenChecksQuery.UniqueCode, IndexOrder = 11, ObjectFieldCode = "Customs.PhysicalCheck.ContainerNubmer" , ColumnWidth = 110 }, addedQueryColumns);

			 QueryColumn OpenChecksQueryColumn_12 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenChecksQuery.Id,QueryCode = OpenChecksQuery.UniqueCode, IndexOrder = 12, ObjectFieldCode = "Customs.PhysicalCheck.OperationName" , ColumnWidth = 80 }, addedQueryColumns);

			 QueryColumn OpenChecksQueryColumn_13 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenChecksQuery.Id,QueryCode = OpenChecksQuery.UniqueCode, IndexOrder = 13, ObjectFieldCode = "Customs.PhysicalCheck.IsComprehensiveCheck" , ColumnWidth = 80 }, addedQueryColumns);

			 QueryColumn OpenChecksQueryColumn_14 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenChecksQuery.Id,QueryCode = OpenChecksQuery.UniqueCode, IndexOrder = 14, ObjectFieldCode = "Customs.PhysicalCheck.CheckTypeCode" , ColumnWidth = 80 }, addedQueryColumns);

			 QueryColumn OpenChecksQueryColumn_15 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenChecksQuery.Id,QueryCode = OpenChecksQuery.UniqueCode, IndexOrder = 15, ObjectFieldCode = "Customs.PhysicalCheck.CheckTypeName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn OpenChecksQueryColumn_16 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = OpenChecksQuery.Id,QueryCode = OpenChecksQuery.UniqueCode, IndexOrder = 16, ObjectFieldCode = "Customs.PhysicalCheck.VehicleChassisNumber" , ColumnWidth = 130 }, addedQueryColumns);

             AdvancedQueryFilter OpenChecksQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.PhysicalCheck.IsClosed", PredefinedValue = "false",PredefinedValue2 = null, QueryId = OpenChecksQuery.Id,QueryCode = OpenChecksQuery.UniqueCode, Tenant = 0}, addedQueryFilters);

  
	      

			  Query ClosedChecksQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = PhysicalCheckTextCode_1.Id, NameTextCodeCode = PhysicalCheckTextCode_1.Code, ObjectTableName = "Customs.PhysicalCheck", Code = "ClosedChecks",  QueryGroupCode = "PHCK", IndexOrder = 1, Tenant = 0, ObjectTableId = PhysicalCheckObjectTable.Id, QuerySection = "Customs.PhysicalCheck", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = PhysicalCheckFeature_1.Id,FeatureUniqeCode= PhysicalCheckFeature_1.FeatureUniqeCode, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, addedQueries);
	
			 QueryColumn ClosedChecksQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedChecksQuery.Id,QueryCode = ClosedChecksQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.PhysicalCheck.CustomFileNo" , ColumnWidth = 90 }, addedQueryColumns);

			 QueryColumn ClosedChecksQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedChecksQuery.Id,QueryCode = ClosedChecksQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.PhysicalCheck.DeclarationNo" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn ClosedChecksQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedChecksQuery.Id,QueryCode = ClosedChecksQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.PhysicalCheck.CustomerName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn ClosedChecksQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedChecksQuery.Id,QueryCode = ClosedChecksQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.PhysicalCheck.StorageSiteName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn ClosedChecksQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedChecksQuery.Id,QueryCode = ClosedChecksQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.PhysicalCheck.CheckSiteName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn ClosedChecksQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedChecksQuery.Id,QueryCode = ClosedChecksQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Customs.PhysicalCheck.QueueTypeName" , ColumnWidth = 70 }, addedQueryColumns);

			 QueryColumn ClosedChecksQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedChecksQuery.Id,QueryCode = ClosedChecksQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Customs.PhysicalCheck.CheckId" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn ClosedChecksQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedChecksQuery.Id,QueryCode = ClosedChecksQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "Customs.PhysicalCheck.LimitDate" , ColumnWidth = 110 }, addedQueryColumns);

			 QueryColumn ClosedChecksQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedChecksQuery.Id,QueryCode = ClosedChecksQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "Customs.PhysicalCheck.Direction" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn ClosedChecksQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedChecksQuery.Id,QueryCode = ClosedChecksQuery.UniqueCode, IndexOrder = 9, ObjectFieldCode = "Customs.PhysicalCheck.TransportModeId" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn ClosedChecksQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedChecksQuery.Id,QueryCode = ClosedChecksQuery.UniqueCode, IndexOrder = 10, ObjectFieldCode = "Customs.PhysicalCheck.ContainerNubmer" , ColumnWidth = 110 }, addedQueryColumns);

			 QueryColumn ClosedChecksQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedChecksQuery.Id,QueryCode = ClosedChecksQuery.UniqueCode, IndexOrder = 11, ObjectFieldCode = "Customs.PhysicalCheck.OperationName" , ColumnWidth = 80 }, addedQueryColumns);

			 QueryColumn ClosedChecksQueryColumn_12 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedChecksQuery.Id,QueryCode = ClosedChecksQuery.UniqueCode, IndexOrder = 12, ObjectFieldCode = "Customs.PhysicalCheck.IsComprehensiveCheck" , ColumnWidth = 80 }, addedQueryColumns);

			 QueryColumn ClosedChecksQueryColumn_13 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedChecksQuery.Id,QueryCode = ClosedChecksQuery.UniqueCode, IndexOrder = 13, ObjectFieldCode = "Customs.PhysicalCheck.CheckTypeCode" , ColumnWidth = 80 }, addedQueryColumns);

			 QueryColumn ClosedChecksQueryColumn_14 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedChecksQuery.Id,QueryCode = ClosedChecksQuery.UniqueCode, IndexOrder = 14, ObjectFieldCode = "Customs.PhysicalCheck.CheckTypeName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn ClosedChecksQueryColumn_15 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ClosedChecksQuery.Id,QueryCode = ClosedChecksQuery.UniqueCode, IndexOrder = 15, ObjectFieldCode = "Customs.PhysicalCheck.VehicleChassisNumber" , ColumnWidth = 130 }, addedQueryColumns);

             AdvancedQueryFilter ClosedChecksQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.PhysicalCheck.IsClosed", PredefinedValue = "true",PredefinedValue2 = null, QueryId = ClosedChecksQuery.Id,QueryCode = ClosedChecksQuery.UniqueCode, Tenant = 0}, addedQueryFilters);

  
	      

			  Query ByUpcomingChecksQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = PhysicalCheckTextCode_2.Id, NameTextCodeCode = PhysicalCheckTextCode_2.Code, ObjectTableName = "Customs.PhysicalCheck", Code = "By Upcoming Checks",  QueryGroupCode = "PHCK", IndexOrder = 2, Tenant = 0, ObjectTableId = PhysicalCheckObjectTable.Id, QuerySection = "Customs.PhysicalCheck", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = PhysicalCheckFeature_2.Id,FeatureUniqeCode= PhysicalCheckFeature_2.FeatureUniqeCode, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, addedQueries);
	
			 QueryColumn ByUpcomingChecksQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ByUpcomingChecksQuery.Id,QueryCode = ByUpcomingChecksQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.PhysicalCheck.CustomFileNo" , ColumnWidth = 90 }, addedQueryColumns);

			 QueryColumn ByUpcomingChecksQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ByUpcomingChecksQuery.Id,QueryCode = ByUpcomingChecksQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.PhysicalCheck.DeclarationNo" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn ByUpcomingChecksQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ByUpcomingChecksQuery.Id,QueryCode = ByUpcomingChecksQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.PhysicalCheck.CustomerName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn ByUpcomingChecksQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ByUpcomingChecksQuery.Id,QueryCode = ByUpcomingChecksQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.PhysicalCheck.StorageSiteName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn ByUpcomingChecksQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ByUpcomingChecksQuery.Id,QueryCode = ByUpcomingChecksQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.PhysicalCheck.CheckSiteName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn ByUpcomingChecksQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ByUpcomingChecksQuery.Id,QueryCode = ByUpcomingChecksQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Customs.PhysicalCheck.QueueTypeName" , ColumnWidth = 70 }, addedQueryColumns);

			 QueryColumn ByUpcomingChecksQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ByUpcomingChecksQuery.Id,QueryCode = ByUpcomingChecksQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Customs.PhysicalCheck.CheckId" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn ByUpcomingChecksQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ByUpcomingChecksQuery.Id,QueryCode = ByUpcomingChecksQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "Customs.PhysicalCheck.LimitDate" , ColumnWidth = 110 }, addedQueryColumns);

			 QueryColumn ByUpcomingChecksQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ByUpcomingChecksQuery.Id,QueryCode = ByUpcomingChecksQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "Customs.PhysicalCheck.Direction" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn ByUpcomingChecksQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ByUpcomingChecksQuery.Id,QueryCode = ByUpcomingChecksQuery.UniqueCode, IndexOrder = 9, ObjectFieldCode = "Customs.PhysicalCheck.TransportModeId" , ColumnWidth = 100 }, addedQueryColumns);

			 QueryColumn ByUpcomingChecksQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ByUpcomingChecksQuery.Id,QueryCode = ByUpcomingChecksQuery.UniqueCode, IndexOrder = 10, ObjectFieldCode = "Customs.PhysicalCheck.ContainerNubmer" , ColumnWidth = 110 }, addedQueryColumns);

			 QueryColumn ByUpcomingChecksQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ByUpcomingChecksQuery.Id,QueryCode = ByUpcomingChecksQuery.UniqueCode, IndexOrder = 11, ObjectFieldCode = "Customs.PhysicalCheck.OperationName" , ColumnWidth = 80 }, addedQueryColumns);

			 QueryColumn ByUpcomingChecksQueryColumn_12 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ByUpcomingChecksQuery.Id,QueryCode = ByUpcomingChecksQuery.UniqueCode, IndexOrder = 12, ObjectFieldCode = "Customs.PhysicalCheck.IsComprehensiveCheck" , ColumnWidth = 80 }, addedQueryColumns);

			 QueryColumn ByUpcomingChecksQueryColumn_13 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ByUpcomingChecksQuery.Id,QueryCode = ByUpcomingChecksQuery.UniqueCode, IndexOrder = 13, ObjectFieldCode = "Customs.PhysicalCheck.CheckTypeCode" , ColumnWidth = 80 }, addedQueryColumns);

			 QueryColumn ByUpcomingChecksQueryColumn_14 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ByUpcomingChecksQuery.Id,QueryCode = ByUpcomingChecksQuery.UniqueCode, IndexOrder = 14, ObjectFieldCode = "Customs.PhysicalCheck.CheckTypeName" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn ByUpcomingChecksQueryColumn_15 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ByUpcomingChecksQuery.Id,QueryCode = ByUpcomingChecksQuery.UniqueCode, IndexOrder = 15, ObjectFieldCode = "Customs.PhysicalCheck.VehicleChassisNumber" , ColumnWidth = 130 }, addedQueryColumns);
			SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
			SqlBulkInsert.BulkInsert("Features", addedFeatures);
			SqlBulkInsert.BulkInsert("Queries", addedQueries);
			SqlBulkInsert.BulkInsert("QueryColumns", addedQueryColumns);
			SqlBulkInsert.BulkInsert("AdvancedQueryFilters", addedQueryFilters);	 
  
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable PhysicalCheckObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.PhysicalCheck" && d.Tenant == 0).FirstOrDefault();
		   //List<ObjectField> PhysicalCheckObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Customs.PhysicalCheck").ToList();
		       
	      

	         Screen PhysicalCheckHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Customs.PhysicalCheck.HeaderScreen", Name = "Header Screen", ObjectTableId = PhysicalCheckObjectTable.Id, NumberOfColumns = 4, NumberOfRows = 2, IsReadOnly = true }, screensRepository, tenantScreens);
      
             ScreenField CustomsPhysicalCheckCustomsPhysicalCheckHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ScreenId = PhysicalCheckHeaderScreenScreen0.Id,ScreenCode = PhysicalCheckHeaderScreenScreen0.Code, ObjectFieldCode = "Customs.PhysicalCheck.CargoIdentifierTypeName", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsPhysicalCheckCustomsPhysicalCheckHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ScreenId = PhysicalCheckHeaderScreenScreen0.Id,ScreenCode = PhysicalCheckHeaderScreenScreen0.Code, ObjectFieldCode = "Customs.PhysicalCheck.CheckId", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsPhysicalCheckCustomsPhysicalCheckHeaderScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ScreenId = PhysicalCheckHeaderScreenScreen0.Id,ScreenCode = PhysicalCheckHeaderScreenScreen0.Code, ObjectFieldCode = "Customs.PhysicalCheck.CargoIdentifierKey1", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsPhysicalCheckCustomsPhysicalCheckHeaderScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ScreenId = PhysicalCheckHeaderScreenScreen0.Id,ScreenCode = PhysicalCheckHeaderScreenScreen0.Code, ObjectFieldCode = "Customs.PhysicalCheck.LimitDate", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsPhysicalCheckCustomsPhysicalCheckHeaderScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ScreenId = PhysicalCheckHeaderScreenScreen0.Id,ScreenCode = PhysicalCheckHeaderScreenScreen0.Code, ObjectFieldCode = "Customs.PhysicalCheck.CargoIdentifierKey2", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsPhysicalCheckCustomsPhysicalCheckHeaderScreenScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 1, ScreenId = PhysicalCheckHeaderScreenScreen0.Id,ScreenCode = PhysicalCheckHeaderScreenScreen0.Code, ObjectFieldCode = "Customs.PhysicalCheck.IsClosed", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsPhysicalCheckCustomsPhysicalCheckHeaderScreenScreenField6 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ScreenId = PhysicalCheckHeaderScreenScreen0.Id,ScreenCode = PhysicalCheckHeaderScreenScreen0.Code, ObjectFieldCode = "Customs.PhysicalCheck.CargoIdentifierKey3", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          	
		    PhysicalCheckObjectTable.HeaderScreenId = PhysicalCheckHeaderScreenScreen0.Id;
		    PhysicalCheckObjectTable.HeaderScreenCode = PhysicalCheckHeaderScreenScreen0.Code;

	   		  
	      

	         Screen PhysicalCheckGeneralTabScreenScreen1 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Customs.PhysicalCheck.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = PhysicalCheckObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 5, IsReadOnly = false }, screensRepository, tenantScreens);
      
             ScreenField CustomsPhysicalCheckCustomsPhysicalCheckGeneralTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ScreenId = PhysicalCheckGeneralTabScreenScreen1.Id,ScreenCode = PhysicalCheckGeneralTabScreenScreen1.Code, ObjectFieldCode = "Customs.PhysicalCheck.StorageSiteCode", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsPhysicalCheckCustomsPhysicalCheckGeneralTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ScreenId = PhysicalCheckGeneralTabScreenScreen1.Id,ScreenCode = PhysicalCheckGeneralTabScreenScreen1.Code, ObjectFieldCode = "Customs.PhysicalCheck.CheckSiteCode", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsPhysicalCheckCustomsPhysicalCheckGeneralTabScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ScreenId = PhysicalCheckGeneralTabScreenScreen1.Id,ScreenCode = PhysicalCheckGeneralTabScreenScreen1.Code, ObjectFieldCode = "Customs.PhysicalCheck.CheckId", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsPhysicalCheckCustomsPhysicalCheckGeneralTabScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 3, ScreenId = PhysicalCheckGeneralTabScreenScreen1.Id,ScreenCode = PhysicalCheckGeneralTabScreenScreen1.Code, ObjectFieldCode = "Customs.PhysicalCheck.OperationCode", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsPhysicalCheckCustomsPhysicalCheckGeneralTabScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 4, ScreenId = PhysicalCheckGeneralTabScreenScreen1.Id,ScreenCode = PhysicalCheckGeneralTabScreenScreen1.Code, ObjectFieldCode = "Customs.PhysicalCheck.StatusMessageCode", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsPhysicalCheckCustomsPhysicalCheckGeneralTabScreenScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ScreenId = PhysicalCheckGeneralTabScreenScreen1.Id,ScreenCode = PhysicalCheckGeneralTabScreenScreen1.Code, ObjectFieldCode = "Customs.PhysicalCheck.LimitDate", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsPhysicalCheckCustomsPhysicalCheckGeneralTabScreenScreenField6 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ScreenId = PhysicalCheckGeneralTabScreenScreen1.Id,ScreenCode = PhysicalCheckGeneralTabScreenScreen1.Code, ObjectFieldCode = "Customs.PhysicalCheck.ContainerNubmer", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField CustomsPhysicalCheckCustomsPhysicalCheckGeneralTabScreenScreenField7 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 2, ScreenId = PhysicalCheckGeneralTabScreenScreen1.Id,ScreenCode = PhysicalCheckGeneralTabScreenScreen1.Code, ObjectFieldCode = "Customs.PhysicalCheck.QueueTypeCode", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	            

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable PhysicalCheckObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.PhysicalCheck" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode PhysicalCheckGeneralTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.TH.General", DefaultText = "General",LocalDefaultText = "כללי", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature PhysicalCheckGeneralFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GENERAL", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.PhysicalCheck.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,PhysicalCheckObjectTable);
 
                 
			   TextCode PhysicalCheckSearchReasultTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.TH.Answer", DefaultText = "Search Reasult",LocalDefaultText = "תוצםות בדיקה", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature PhysicalCheckSearchReasultFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PhysicalCheck.Tab.SearchReasult", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, NameTextCodeCode = "PhysicalCheckFeatures.ANPC", NameTextCodeDefaultText = "Search Reasult", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,PhysicalCheckObjectTable);
 
                 
			   TextCode PhysicalCheckEventsTextCode_TH2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.TH.Events", DefaultText = "Events",LocalDefaultText = "םירועים", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature PhysicalCheckEventsFeature_TH2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EVENTS", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.PhysicalCheck.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,PhysicalCheckObjectTable);
 
                 
			   TextCode PhysicalCheckCommunicationsTextCode_TH3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.TH.Communications", DefaultText = "Communications",LocalDefaultText = "תקשורת", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature PhysicalCheckCommunicationsFeature_TH3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "COMMUNICATIONS", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.PhysicalCheck.Features.Communication", NameTextCodeDefaultText = "Communication", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,PhysicalCheckObjectTable);
 
                 
			   TextCode PhysicalCheckRequestSheetsTextCode_TH4 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.TH.RequestSheets", DefaultText = "Request Sheets",LocalDefaultText = "גיליון בקשה", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature PhysicalCheckRequestSheetsFeature_TH4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "REQUESTSHEET", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.PhysicalCheck.Features.RequestSheets", NameTextCodeDefaultText = "Request Sheets", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,PhysicalCheckObjectTable);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "PHGC",HtmlComponentName = "PhysicalCheckGeneralTabComponent",HtmlComponentUrl = "./CustomsModules/CustomsPhysicalCheck/Components/EditTabs/General/PhysicalCheckGeneralTabComponent", FeatureId = PhysicalCheckGeneralFeature_TH0.Id,FeatureUniqeCode = PhysicalCheckGeneralFeature_TH0.FeatureUniqeCode, ControlPath = "Logitude.Customs.Views.PhysicalCheckAvailableTimesControl", ObjectTableId = PhysicalCheckObjectTable.Id, TabNameTextCodeId = PhysicalCheckGeneralTextCode_TH0.Id, TabNameTextCodeCode = PhysicalCheckGeneralTextCode_TH0.Code, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "ANPC",HtmlComponentName = "PhysicalCheckSearchReasultTabComponent",HtmlComponentUrl = "./CustomsModules/CustomsPhysicalCheck/Components/EditTabs/SearchReasult/PhysicalCheckSearchReasultTabComponent", FeatureId = PhysicalCheckSearchReasultFeature_TH1.Id,FeatureUniqeCode = PhysicalCheckSearchReasultFeature_TH1.FeatureUniqeCode, ControlPath = "Logitude.Customs.Views.PhysicalCheckAvailableTimesControl", ObjectTableId = PhysicalCheckObjectTable.Id, TabNameTextCodeId = PhysicalCheckSearchReasultTextCode_TH1.Id, TabNameTextCodeCode = PhysicalCheckSearchReasultTextCode_TH1.Code, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "PHEV",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = PhysicalCheckEventsFeature_TH2.Id,FeatureUniqeCode = PhysicalCheckEventsFeature_TH2.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = PhysicalCheckObjectTable.Id, TabNameTextCodeId = PhysicalCheckEventsTextCode_TH2.Id, TabNameTextCodeCode = PhysicalCheckEventsTextCode_TH2.Code, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "PHCM",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = PhysicalCheckCommunicationsFeature_TH3.Id,FeatureUniqeCode = PhysicalCheckCommunicationsFeature_TH3.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.Views.Communications.CommunicationsControl", ObjectTableId = PhysicalCheckObjectTable.Id, TabNameTextCodeId = PhysicalCheckCommunicationsTextCode_TH3.Id, TabNameTextCodeCode = PhysicalCheckCommunicationsTextCode_TH3.Code, Tenant = 0, IndexOrder = 2 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "PHRS",HtmlComponentName = "RequestSheetTabComponent",HtmlComponentUrl = "./CustomsModules/CustomsControls/Components/CustomsRequestsSheetsComponent", FeatureId = PhysicalCheckRequestSheetsFeature_TH4.Id,FeatureUniqeCode = PhysicalCheckRequestSheetsFeature_TH4.FeatureUniqeCode, ControlPath = " ", ObjectTableId = PhysicalCheckObjectTable.Id, TabNameTextCodeId = PhysicalCheckRequestSheetsTextCode_TH4.Id, TabNameTextCodeCode = PhysicalCheckRequestSheetsTextCode_TH4.Code, Tenant = 0, IndexOrder = 3 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable PhysicalCheckObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.PhysicalCheck" && d.Tenant == 0).FirstOrDefault(); 

		   Feature PhysicalCheckFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, NameTextCodeCode = "PhysicalCheck.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,PhysicalCheckObjectTable);
		   Feature PhysicalCheckFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, NameTextCodeCode = "PhysicalCheck.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,PhysicalCheckObjectTable);
		   Feature PhysicalCheckFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, NameTextCodeCode = "PhysicalCheck.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,PhysicalCheckObjectTable);
		   Feature PhysicalCheckFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = false, ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, NameTextCodeCode = "PhysicalCheck.Features.PackageFeature", NameTextCodeDefaultText = "PhysicalCheck Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,PhysicalCheckObjectTable);    
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable PhysicalCheckObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.PhysicalCheck" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "PCI",
                EnglishName =  "Limit date change",
                LocalName =  "התקבלו הנחיות לבדיקה פיזית",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = PhysicalCheckObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "PCE",
                EnglishName =  "Inspection End Notice",
                LocalName =  "הודעה על סיום בדיקה פיזית",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = PhysicalCheckObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "PUI",
                EnglishName =  "Physical Checks Updated",
                LocalName =  "בדיקה פיזית עודכנה",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = PhysicalCheckObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "PUC",
                EnglishName =  "Physical Check Cancelled",
                LocalName =  "בדיקה פיזית בוטלה",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = PhysicalCheckObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "PCF",
                EnglishName =  "Physical Checks Created",
                LocalName =  "בדיקה פיזית נוצרה",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = PhysicalCheckObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "STC",
                EnglishName =  "Physical Checks Updated",
                LocalName =  "שונה מועד זימון למשקף",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = PhysicalCheckObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "SRF",
                EnglishName =  "Inspection End Notice",
                LocalName =  "החזרת מטען מםתר משקף לםחסון",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = PhysicalCheckObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "SPR",
                EnglishName =  "Inspection End Notice",
                LocalName =  "מטען שוחרר מםתר משקף ללקוח",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = PhysicalCheckObjectTable.Id,
				 
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
                ObjectTableId = PhysicalCheckObjectTable.Id,
				 
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
                ObjectTableId = PhysicalCheckObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "SFC",
                EnglishName =  "file summoned for screening site",
                LocalName =  "תיק זומן לבדיקה בםתר משקף",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "LOG",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = PhysicalCheckObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
		   //FeatureRepository featureRepository = new FeatureRepository(0); 
		   //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList(); 
		   ObjectTable PhysicalCheckObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.PhysicalCheck" && d.Tenant == 0).FirstOrDefault();       
    
			   Feature PhysicalCheckFeature_MB0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PHYSICALCHECKACTIONS", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.PhysicalCheck.Features.Actions", NameTextCodeDefaultText = "Actions", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,PhysicalCheckObjectTable);

			   Feature PhysicalCheckFeature_MB00 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CLOSEPHYSICALCHECK", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.PhysicalCheck.Features.ClosePhysicalCheck", NameTextCodeDefaultText = "Close Check", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,PhysicalCheckObjectTable);
              

		   TextCodeRepository.SubmitChanges();
		   FeaturesRepository.SubmitChanges();
		   MenuButtonGroup PhysicalCheckMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
				{
					MenuButtonGroupType = "Customs.PhysicalCheckEdit",
					Name = "Customs.PhysicalCheckEditButtonsGroup",
					ObjectTableId = PhysicalCheckObjectTable.Id,
					Tenant = 0
				}, menuButtonGroupRepository, tenantMenuButtonGroups);        
   
			   MenuButton PhysicalCheckMenuButton0 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "Actions",
						Index = 2, 
						IsActive = false,
						LabelTextCodeCode = "Customs.PhysicalCheck.B.Actions",
						LabelTextCodeDefaultText = "Actions",
						Tenant = 0,
						MenuButtonGroupId = PhysicalCheckMenuButtonGroup.Id,
						ObjectTableId = PhysicalCheckObjectTable.Id,
						MenuButtonType = "dropdownbutton",
						FeatureId = PhysicalCheckFeature_MB0.Id,
						Style = null,
						LocalDefaultText = "פעולות",
						FeatureUniqeCode = PhysicalCheckFeature_MB0.FeatureUniqeCode,
						HtmlComponentPath = null,
						Width = 0,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);

			   MenuButton PhysicalCheckMenuButton00 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "ClosePhysicalCheck",
						Index = 1, 
						IsActive = true,
						LabelTextCodeCode = "Customs.PhysicalCheck.B.ClosePhysicalCheck",
						LabelTextCodeDefaultText = "Close Physical Check",
						Tenant = 0,
						MenuButtonGroupId = PhysicalCheckMenuButtonGroup.Id,
						ParentMenuButtonId = PhysicalCheckMenuButton0.Id,
						ObjectTableId = PhysicalCheckObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  PhysicalCheckFeature_MB00.Id,
						Style = null,
						LocalDefaultText = "סגירת בדיקה",
                        HtmlComponentPath=null,
                        Width=0,
						FeatureUniqeCode=  PhysicalCheckFeature_MB00.FeatureUniqeCode,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable PhysicalCheckObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.PhysicalCheck" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOCustomsQueries = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.CustomsQueries", DefaultText = "Customs Queries",LocalDefaultText = @"שםילתות מכס", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOPhysicalChecks = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.PhysicalChecks", DefaultText = "Physical Checks",LocalDefaultText = @"בדיקות פיסיות", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckODeclarations = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.Declarations", DefaultText = "Declarations",LocalDefaultText = @"הצהרות", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOFromDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.FromDate", DefaultText = "From Date",LocalDefaultText = @" :מתםריך", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOToDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.ToDate", DefaultText = "To Date",LocalDefaultText = @" :עד תםריך", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOGetAutomaticDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.GetAutomaticDate", DefaultText = "Get Automatic Date",LocalDefaultText = @"קבל תםריך םוטומטי", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOGetAvailableTimeList = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.GetAvailableTimeList", DefaultText = "Get Available Time List",LocalDefaultText = @"קבל רשימת תםריכים זמינים", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOXRayAvailableTimes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.XRayAvailableTimes", DefaultText = "Available Times",LocalDefaultText = @"זמינות", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOResponse = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.Response", DefaultText = "Response",LocalDefaultText = @"תשובה", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckORequest = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.Request", DefaultText = "Request",LocalDefaultText = @"בקשה", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOSend = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.Send", DefaultText = "Send",LocalDefaultText = @"שלח", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOChoose = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.Choose", DefaultText = "Choose",LocalDefaultText = @"בחר", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckFAvailableTimesMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.F.AvailableTimesMessage", DefaultText = "Getting Available Times List Completed Successfully",LocalDefaultText = @"קבלת רשימת זמינות הושלמה בהצלחה", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckFAutomaticDateMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.F.AutomaticDateMessage", DefaultText = "Getting Automatic Date Completed Successfully",LocalDefaultText = @"קבלת תםריך םוטומטי הושלמה בהצלחה", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckFChooseDateMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.F.ChooseDateMessage", DefaultText = "New Limit Date Is Chosen Successfully",LocalDefaultText = @"תםריך הגבלה חדש נבחר בהצלחה", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOCheckIdRequierd = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.CheckIdRequierd", DefaultText = "Please Select a check id to request check times",LocalDefaultText = @"םנם בחר id סימון כדי לבקש פעמים סימון", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOCheckSiteRequierd = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.CheckSiteRequierd", DefaultText = "Please Select a check site to request check times",LocalDefaultText = @"םנם בחר םתר המחםה לבקש פעמים סימון", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOQueueTypeRequierd = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.QueueTypeRequierd", DefaultText = "Please Select a queue type to request check times",LocalDefaultText = @"םנם בחר סוג התור לבקש פעמים סימון", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOFromDateLess = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.FromDateLess", DefaultText = "From date must be less than to date",LocalDefaultText = @"ממועד חייב להיות פחות מ עד כה", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOByUpComingChecks = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.ByUpComingChecks", DefaultText = "By UpComing Checks",LocalDefaultText = @"בדיקות פיזיות", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOWaitingResponse = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.WaitingResponse", DefaultText = "Waiting Response...",LocalDefaultText = @"מחכה תגובה ...", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOSelectFromAvailableTimes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.SelectFromAvailableTimes", DefaultText = "Please Select a date from the available times",LocalDefaultText = @"םנם בחר תםריך מתוך פעמים הזמינות", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOAskForAnEarlierDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.AskForAnEarlierDate", DefaultText = "Ask for an earlier date",LocalDefaultText = @"קבל תםריך מוקדם יותר", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOAskForAnLaterDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.AskForAnLaterDate", DefaultText = "Ask for an later date",LocalDefaultText = @"קבל תםריך מםוחר יותר", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOPhysicalCheck = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.PhysicalCheck", DefaultText = "Physical Check",LocalDefaultText = @"בדיקה פיזית", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOCargoIdentifier = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.CargoIdentifier", DefaultText = "Cargo Identifier",LocalDefaultText = @"נתוני מטען", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOcargoIdentifierType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.cargoIdentifierType", DefaultText = "Cargo Identifier Type",LocalDefaultText = @"מזהה מטען", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOCargoIdentifierKey1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.CargoIdentifierKey1", DefaultText = "Cargo Identifier Key1",LocalDefaultText = @"מזהה מטען רםשון", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOCargoIdentifierKey2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.CargoIdentifierKey2", DefaultText = "Cargo Identifier key2",LocalDefaultText = @"מזהה מטען שני", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOContainerNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.ContainerNumber", DefaultText = "Container Number",LocalDefaultText = @"מספר מכולה", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOPhysicalData = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.PhysicalData", DefaultText = "Physical Data",LocalDefaultText = @"נתוני בדיקה פיזית", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOCheckId = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.CheckId", DefaultText = "Check Id ",LocalDefaultText = @"מספר בדיקה", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOOperationCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.OperationCode", DefaultText = "Operation Code",LocalDefaultText = @"קוד פעולה", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOStatusMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.StatusMessage", DefaultText = "Status Message",LocalDefaultText = @"סוג הודעה", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOImporterNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.ImporterNumber", DefaultText = "Importer Number",LocalDefaultText = @"לקוח", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOStorageSiteNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.StorageSiteNumber", DefaultText = "Storage Site Number",LocalDefaultText = @"םתר םחסון", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOCheckSiteNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.CheckSiteNumber", DefaultText = "Check Site Number",LocalDefaultText = @"םתר בדיקה", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOCargoTypeCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.CargoTypeCode", DefaultText = "Cargo Type Code",LocalDefaultText = @"סוג מטען", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOQueueType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.QueueType", DefaultText = "Queue Type",LocalDefaultText = @"סוג תור", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOLimitDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.LimitDate", DefaultText = "Limit Date",LocalDefaultText = @"תםריך הבדיקה", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOOpenData = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.OpenData", DefaultText = "Open Data",LocalDefaultText = @"תםריך זימון הבדיקה", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOCheckType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.CheckType", DefaultText = "Check Type",LocalDefaultText = @"סוג הבדיקה", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOConnectedEntity = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.ConnectedEntity", DefaultText = "Connected Entity",LocalDefaultText = @"ישויות קשורות", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckODeclarationId = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.DeclarationId", DefaultText = "Declaration ID",LocalDefaultText = @"מספר הצהרה", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOCustomFileNo = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.CustomFileNo", DefaultText = "Custom File No.",LocalDefaultText = @"תיק עמילות", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOEndDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.EndDate", DefaultText = "End Date",LocalDefaultText = @"מועד סיום הבדיקה", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode PhysicalCheckTextCode_CustomsPhysicalCheckOGeneralDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PhysicalCheck.O.GeneralDetails", DefaultText = "General Details",LocalDefaultText = @"נתוני בדיקה פיזית", ObjectTableId = PhysicalCheckObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 