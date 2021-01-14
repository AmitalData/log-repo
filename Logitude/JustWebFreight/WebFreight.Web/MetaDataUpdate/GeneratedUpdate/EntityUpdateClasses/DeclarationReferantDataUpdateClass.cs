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

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.EntityUpdateClasses
{
   public class DeclarationReferantDataUpdateClass
   {  		
		public const string HashString = "de609892f3f27744eebfe587f3a59666";
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "Customs.DeclarationReferantData",
			      				    IsNew =  true,
			      				    DBTableName =  "Customs.DeclarationReferantDatas",
			      				    ObjectTableSingular =  "DeclarationReferantData",
			      				    ObjectTablePlural =  "DeclarationReferantDatas",
			      				    HasCustomFilter =  true,
			      				    HasCustomFields =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasFiltersMenu =  true,
			      				    IsEditable =  false,
			      				    IsNewWizard =  false,
			      				    LookUp1 =  "DeclarationId",
			      				    LookUp2 =  "OrderNumber",
			      				    KeyPropertyPath =  "DeclarationId",
			      				    AutoCompleteSearchWindow =  false,
			      				    IsClosed =  false,
			      				    CacheOnClient =  false,
			      				    EditableFromAutoCompleteWindow =  false,
			      				    HasCounter =  true,
			      				    EnableAddFromLOV =  false,
			      				    IsRestrictable =  false,
			      				    IsMain =  true,
			      				    IsAutoComplete =  false,
			      				    EnableEditFromLOV =  false,
			      				    SortingByObjectField =  "DeclarationId",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  false,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    NewWizardControlName =  "Logitude.Customs.NewDeclarationControlCommand",
			      				    LocalDefaultText =  "רפרנט",
			      				    DefaultText =  "Declaration Referant Data",
			      				    Code =  "5277",
			      				    Name =  " Query Group",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Customs",
			      				    ServerModuleName =  "Customs",
			      				    NewWizardComponentPath =  "",
			      				    NoTS =  false,
			      				    HasMenuButtons =  false,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    IsTabsHidden =  false,
			      				    HashString =  DeclarationReferantDataUpdateClass.HashString,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository, List<ObjectField> addedFields, List<TextCode> addedTextCodes)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DeclarationId",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						ValidForQuerySection1 =  "DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DeclarationId",
					  						DefaultText =  "Declaration ID",
					  						FullLocalDefaultText =  "מספר תיק מכס",
					  						ListFieldLable =  "DeclarationIdListLable",
					  						ListLableDefaultText =  "Declaration ID",
					  						ListLocalDefaultText =  "מספר תיק מכס",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  true,
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
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
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
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  true,
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
					 
					 						FieldName =  "OrderNumber",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
					  						FieldsDataType =  "Text",
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
					  						PMPropertyPath =  "OrderNumber",
					  						ListPropertyPath =  "OrderNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "OrderNumber",
					  						DefaultText =  "Order",
					  						FullLocalDefaultText =  "מס' הזמנה",
					  						ListFieldLable =  "OrderNumberListLable",
					  						ListLableDefaultText =  "Order Number",
					  						ListLocalDefaultText =  "מס' הזמנה",
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
					 
					 						FieldName =  "VendorId",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.CustomsVendor",
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
					  						PMPropertyPath =  "VendorId",
					  						ListPropertyPath =  "VendorId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "VendorId",
					  						DefaultText =  "Vendor ID",
					  						FullLocalDefaultText =  "מס' ספק",
					  						ListFieldLable =  "VendorIdListLable",
					  						ListLableDefaultText =  "Vendor ID",
					  						ListLocalDefaultText =  "מס' ספק",
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
					 
					 						FieldName =  "ArrivalDate",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ArrivalDate",
					  						ListPropertyPath =  "ArrivalDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						ColumnHeaderTemplateName =  "ArrivalDateListHeaderTemplate",
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ArrivalDate",
					  						DefaultText =  "Arrival Date",
					  						FullLocalDefaultText =  "ETA/ATA",
					  						ListFieldLable =  "ArrivalDateListLable",
					  						ListLableDefaultText =  "Arrival Date",
					  						ListLocalDefaultText =  "ETA/ATA",
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
					 
					 						FieldName =  "EstimatedArrivalDate",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "EstimatedArrivalDate",
					  						ListPropertyPath =  "EstimatedArrivalDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "EstimatedArrivalDate",
					  						DefaultText =  "Estimated Arrival Date",
					  						FullLocalDefaultText =  "ETA",
					  						ListFieldLable =  "EstimatedArrivalDateListLable",
					  						ListLableDefaultText =  "Estimated Arrival Date",
					  						ListLocalDefaultText =  "ETA",
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
					 
					 						FieldName =  "Weight",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Weight",
					  						ListPropertyPath =  "Weight",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  15,
					  						DigitsAfterPoint =  3,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Weight",
					  						DefaultText =  "Weight",
					  						FullLocalDefaultText =  "משקל",
					  						ListFieldLable =  "WeightListLable",
					  						ListLableDefaultText =  "Weight",
					  						ListLocalDefaultText =  "משקל",
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
					 
					 						FieldName =  "ClassificationStatus",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						PMPropertyPath =  "ClassificationStatus",
					  						ListPropertyPath =  "ClassificationStatus",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ClassificationStatus",
					  						DefaultText =  "Classification Status",
					  						FullLocalDefaultText =  "סיווג",
					  						ListFieldLable =  "ClassificationStatusListLable",
					  						ListLableDefaultText =  "Classification Status",
					  						ListLocalDefaultText =  "סיווג",
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
					 
					 						FieldName =  "ControllerStatus",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						PMPropertyPath =  "ControllerStatus",
					  						ListPropertyPath =  "ControllerStatus",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ControllerStatus",
					  						DefaultText =  "Controller Status",
					  						FullLocalDefaultText =  "מבקר",
					  						ListFieldLable =  "ControllerStatusListLable",
					  						ListLableDefaultText =  "Controller Status",
					  						ListLocalDefaultText =  "מבקר",
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
					 
					 						FieldName =  "CollectionOfMoneyStatus",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						PMPropertyPath =  "CollectionOfMoneyStatus",
					  						ListPropertyPath =  "CollectionOfMoneyStatus",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CollectionOfMoneyStatus",
					  						DefaultText =  "Collection Of Money Status",
					  						FullLocalDefaultText =  "גביה",
					  						ListFieldLable =  "CollectionOfMoneyStatusListLable",
					  						ListLableDefaultText =  "Collection Of Money Status",
					  						ListLocalDefaultText =  "גביה",
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
					 
					 						FieldName =  "FollowUpDate",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "FollowUpDate",
					  						ListPropertyPath =  "FollowUpDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "FollowUpDate",
					  						DefaultText =  "FollowUp Date",
					  						FullLocalDefaultText =  "תאריך מעקב",
					  						ListFieldLable =  "FollowUpDateListLable",
					  						ListLableDefaultText =  "FollowUp Date",
					  						ListLocalDefaultText =  "תאריך מעקב",
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
					 
					 						FieldName =  "WithPaper",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						PMPropertyPath =  "WithPaper",
					  						ListPropertyPath =  "WithPaper",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "WithPaper",
					  						DefaultText =  "With Paper",
					  						FullLocalDefaultText =  "מלווה ניירת",
					  						ListFieldLable =  "WithPaperListLable",
					  						ListLableDefaultText =  "With Paper",
					  						ListLocalDefaultText =  "מלווה ניירת",
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
					 
					 						FieldName =  "IsClosedForFollowUp",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  1,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsClosedForFollowUp",
					  						ListPropertyPath =  "IsClosedForFollowUp",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsClosedForFollowUp",
					  						DefaultText =  "Is Closed For FollowUp",
					  						FullLocalDefaultText =  "סגור /פתוח",
					  						ListFieldLable =  "IsClosedForFollowUpListLable",
					  						ListLableDefaultText =  "Is Closed For FollowUp",
					  						ListLocalDefaultText =  "סגור /פתוח",
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
					 
					 						FieldName =  "IsClassificationRemarks",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						PMPropertyPath =  "IsClassificationRemarks",
					  						ListPropertyPath =  "IsClassificationRemarks",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsClassificationRemarks",
					  						DefaultText =  "Is Classification Remarks",
					  						FullLocalDefaultText =  "הערות מסווג",
					  						ListFieldLable =  "IsClassificationRemarksListLable",
					  						ListLableDefaultText =  "Is Classification Remarks",
					  						ListLocalDefaultText =  "הערות מסווג",
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
					 
					 						FieldName =  "IsControllerRemarks",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						PMPropertyPath =  "IsControllerRemarks",
					  						ListPropertyPath =  "IsControllerRemarks",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsControllerRemarks",
					  						DefaultText =  "Is Controller Remarks",
					  						FullLocalDefaultText =  "הערות מבקר",
					  						ListFieldLable =  "IsControllerRemarksListLable",
					  						ListLableDefaultText =  "Is Controller Remarks",
					  						ListLocalDefaultText =  "הערות מבקר",
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
					 
					 						FieldName =  "PreClassification",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  1,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  1,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "PreClassification",
					  						ListPropertyPath =  "PreClassification",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PreClassification",
					  						DefaultText =  "Pre Classification",
					  						FullLocalDefaultText =  "שירות OCR",
					  						ListFieldLable =  "PreClassificationListLable",
					  						ListLableDefaultText =  "Pre Classification",
					  						ListLocalDefaultText =  "שירות OCR",
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
					 
					 						FieldName =  "CustomFileNo",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						ValidForQuerySection1 =  "Customs.Declaration",
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
					  						ListFieldLable =  "CustomFileNoListLable",
					  						ListLableDefaultText =  "Custom File No",
					  						ListLocalDefaultText =  "תיק עמילות",
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
					  						ObjectTableName =  "Customs.DeclarationReferantData",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  60,
					  						IsRequired =  false,
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
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CustomerName",
					  						DefaultText =  "Customer Name",
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
					 
					 						FieldName =  "TransportModeId",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						ValidForQuerySection1 =  "Customs.Declaration",
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
					  						EnableFullscreenTextBox =  true,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DeclarationOfficeCode",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.CustomsHouseType",
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
					  						PMPropertyPath =  "DeclarationOfficeCode",
					  						ListPropertyPath =  "DeclarationOfficeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DeclarationOfficeCode",
					  						DefaultText =  "Declaration Office",
					  						FullLocalDefaultText =  "בית מכס",
					  						ListFieldLable =  "DeclarationOfficeCodeListLable",
					  						ListLableDefaultText =  "Declaration Office",
					  						ListLocalDefaultText =  "בית מכס",
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
					 
					 						FieldName =  "VendorName",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  55,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  55,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "VendorName",
					  						ListPropertyPath =  "VendorName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "VendorName",
					  						DefaultText =  "Vendor Name",
					  						ListFieldLable =  "VendorNameListLable",
					  						ListLableDefaultText =  "Vendor Name",
					  						ListLocalDefaultText =  "שם ספק",
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
					 
					 						FieldName =  "DeclarationStatusTypeName",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DeclarationStatusTypeName",
					  						ListPropertyPath =  "DeclarationStatusTypeName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DeclarationStatusTypeName",
					  						DefaultText =  "Declaration Status Type",
					  						FullLocalDefaultText =  "סטטוס הצהרה",
					  						ListFieldLable =  "DeclarationStatusTypeNameListLable",
					  						ListLableDefaultText =  "Declaration Status Type Name",
					  						ListLocalDefaultText =  "סטטוס הצהרה",
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
					 
					 						FieldName =  "DeclarationOfficeName",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DeclarationOfficeName",
					  						ListPropertyPath =  "DeclarationOfficeName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DeclarationOfficeName",
					  						DefaultText =  "Declaration Office Name",
					  						ListFieldLable =  "DeclarationOfficeNameListLable",
					  						ListLableDefaultText =  "Declaration Office Name",
					  						ListLocalDefaultText =  "שם תחנת מכס",
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
					 
					 						FieldName =  "DeclarationStatusTypeCode",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.DeclarationStatusType",
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
					  						PMPropertyPath =  "DeclarationStatusTypeCode",
					  						ListPropertyPath =  "DeclarationStatusTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DeclarationStatusTypeCode",
					  						DefaultText =  "Declaration Status Type",
					  						FullLocalDefaultText =  "סטטוס הצהרה",
					  						ListFieldLable =  "DeclarationStatusTypeCodeListLable",
					  						ListLableDefaultText =  "Declaration Status Type",
					  						ListLocalDefaultText =  "סטטוס הצהרה",
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
					 
					 						FieldName =  "ATAOrETA",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						PMPropertyPath =  "ATAOrETA",
					  						ListPropertyPath =  "ATAOrETA",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ATAOrETA",
					  						DefaultText =  "ETA/ATA",
					  						FullLocalDefaultText =  "ETA/ATA",
					  						ListFieldLable =  "ATAOrETAListLable",
					  						ListLableDefaultText =  "ETA/ATA",
					  						ListLocalDefaultText =  "ETA/ATA",
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
					 
					 						FieldName =  "SearchFields",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						SystemMaxLength =  40,
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
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SearchFields",
					  						DefaultText =  "Search",
					  						FullLocalDefaultText =  "תיק/הצהרה/מזהה מטען/לקוח",
					  						ListFieldLable =  "SearchFieldsListLable",
					  						ListLableDefaultText =  "SearchField",
					  						HelpTextCode =  "SearchFields",
					  						HelpTextDefaultText =  "Searching by :",
					  						HelpLocalDefaultText =  "חיפוש על ידי: ",
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
					 
					 						FieldName =  "ExceptionReasonsList",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  1000,
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
					  						PMPropertyPath =  "ExceptionReasonsList",
					  						ListPropertyPath =  "ExceptionReasonsList",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ExceptionReasonsList",
					  						DefaultText =  "Exception Reasons List",
					  						ListFieldLable =  "ExceptionReasonsListListLable",
					  						ListLableDefaultText =  "Exception",
					  						ListLocalDefaultText =  "חריג",
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
					 
					 						FieldName =  "DepartmentId",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Department",
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
					  						PMPropertyPath =  "DepartmentId",
					  						ListPropertyPath =  "DepartmentId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DepartmentId",
					  						DefaultText =  "Department ",
					  						ListFieldLable =  "DepartmentIdListLable",
					  						ListLableDefaultText =  "Department ",
					  						ListLocalDefaultText =  "מחלקה",
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
					 
					 						FieldName =  "ReferentUserId",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ReferentUserId",
					  						ListPropertyPath =  "ReferentUserId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ReferentUserId",
					  						DefaultText =  "Referent User ",
					  						FullLocalDefaultText =  "רפרנט",
					  						ListFieldLable =  "ReferentUserIdListLable",
					  						ListLableDefaultText =  "Referent User ",
					  						ListLocalDefaultText =  "רפרנט",
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
					 
					 						FieldName =  "Actions",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
					  						FieldsDataType =  "Text",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Actions",
					  						ListPropertyPath =  "Actions",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Actions",
					  						DefaultText =  " ",
					  						ListFieldLable =  "ActionsListLable",
					  						ListLableDefaultText =  "Actions",
					  						ListLocalDefaultText =  " פעולות",
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
					 
					 						FieldName =  "AvailabilityDate",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
					  						FieldsDataType =  "DateTime",
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
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AvailabilityDate",
					  						ListPropertyPath =  "AvailabilityDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.Declaration",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AvailabilityDate",
					  						DefaultText =  "Availability Date",
					  						ListFieldLable =  "AvailabilityDateListLable",
					  						ListLableDefaultText =  "Availability Date",
					  						ListLocalDefaultText =  "תאריך זמינות",
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
					 
					 						FieldName =  "ClassifiedUserId",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						PMPropertyPath =  "ClassifiedUserId",
					  						ListPropertyPath =  "ClassifiedUserId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ClassifiedUserId",
					  						DefaultText =  "מסווג",
					  						FullLocalDefaultText =  "מסווג",
					  						ListFieldLable =  "ClassifiedUserIdListLable",
					  						ListLableDefaultText =  "ClassifiedUserId",
					  						ListLocalDefaultText =  "מסווג",
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
					 
					 						FieldName =  "ControllerUserId",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						PMPropertyPath =  "ControllerUserId",
					  						ListPropertyPath =  "ControllerUserId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ControllerUserId",
					  						DefaultText =  "שם מבקר",
					  						FullLocalDefaultText =  "שם מבקר",
					  						ListFieldLable =  "ControllerUserIdListLable",
					  						ListLableDefaultText =  "ControllerUserId",
					  						ListLocalDefaultText =  "שם מבקר",
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
					 
					 						FieldName =  "CollectorUserId",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						PMPropertyPath =  "CollectorUserId",
					  						ListPropertyPath =  "CollectorUserId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CollectorUserId",
					  						DefaultText =  "גובה",
					  						FullLocalDefaultText =  "גובה",
					  						ListFieldLable =  "CollectorUserIdListLable",
					  						ListLableDefaultText =  "CollectorUserId",
					  						ListLocalDefaultText =  "גובה",
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
					 
					 						FieldName =  "NewFile",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						PMPropertyPath =  "NewFile",
					  						ListPropertyPath =  "NewFile",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "NewFile",
					  						DefaultText =  "New File",
					  						FullLocalDefaultText =  "תיק חדש",
					  						ListFieldLable =  "NewFileListLable",
					  						ListLableDefaultText =  "New File",
					  						ListLocalDefaultText =  "תיק חדש",
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
					 
					 						FieldName =  "Favorite",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						PMPropertyPath =  "Favorite",
					  						ListPropertyPath =  "Favorite",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Favorite",
					  						DefaultText =  "Favorite",
					  						FullLocalDefaultText =  "מועדף",
					  						ListFieldLable =  "FavoriteListLable",
					  						ListLableDefaultText =  "Favorite",
					  						ListLocalDefaultText =  "מועדף",
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
					 
					 						FieldName =  "SortedColumns",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						PMPropertyPath =  "SortedColumns",
					  						ListPropertyPath =  "SortedColumns",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SortedColumns",
					  						DefaultText =  "Sorted Columns",
					  						ListFieldLable =  "SortedColumnsListLable",
					  						ListLableDefaultText =  "Sorted Columns",
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
					 
					 						FieldName =  "IsCustomerLogBoxActivated",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						PMPropertyPath =  "IsCustomerLogBoxActivated",
					  						ListPropertyPath =  "IsCustomerLogBoxActivated",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsCustomerLogBoxActivated",
					  						DefaultText =  "Custom Log Box Activated",
					  						ListFieldLable =  "IsCustomerLogBoxActivatedListLable",
					  						ListLableDefaultText =  "Custom Log Box Activated",
					  						ListLocalDefaultText =  "לקוח LogBox",
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
					 
					 						FieldName =  "IsCancelled",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						PMPropertyPath =  "IsCancelled",
					  						ListPropertyPath =  "IsCancelled",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsCancelled",
					  						DefaultText =  "Is Cancelled",
					  						FullLocalDefaultText =  "תיק בוטל",
					  						ListFieldLable =  "IsCancelledListLable",
					  						ListLableDefaultText =  "Is Cancelled",
					  						ListLocalDefaultText =  "תיק בוטל",
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
					 
					 						FieldName =  "ClassifiedUserName",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  60,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  60,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ClassifiedUserName",
					  						ListPropertyPath =  "ClassifiedUserName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ClassifiedUserName",
					  						DefaultText =  "Classified User Name",
					  						FullLocalDefaultText =  "שם מסווג",
					  						ListFieldLable =  "ClassifiedUserNameListLable",
					  						ListLableDefaultText =  "Classified User Name",
					  						ListLocalDefaultText =  "שם מסווג",
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
					 
					 						FieldName =  "ControllerUserName",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  60,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  60,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ControllerUserName",
					  						ListPropertyPath =  "ControllerUserName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ControllerUserName",
					  						DefaultText =  "Controller User Name",
					  						FullLocalDefaultText =  "שם מבקר",
					  						ListFieldLable =  "ControllerUserNameListLable",
					  						ListLableDefaultText =  "Controller User Name",
					  						ListLocalDefaultText =  "שם מבקר",
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
					 
					 						FieldName =  "CollectorUserName",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  60,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  60,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CollectorUserName",
					  						ListPropertyPath =  "CollectorUserName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CollectorUserName",
					  						DefaultText =  "Collector User Name",
					  						FullLocalDefaultText =  "שם גובה",
					  						ListFieldLable =  "CollectorUserNameListLable",
					  						ListLableDefaultText =  "CollectorUserName",
					  						ListLocalDefaultText =  "שם גובה",
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
					 
					 						FieldName =  "LastStatusName",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						PMPropertyPath =  "LastStatusName",
					  						ListPropertyPath =  "LastStatusName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LastStatusName",
					  						DefaultText =  "LastStatusName",
					  						FullLocalDefaultText =  "שם סטטוס אחרון",
					  						ListFieldLable =  "LastStatusNameListLable",
					  						ListLableDefaultText =  "LastStatusName",
					  						ListLocalDefaultText =  " שם סטטוס אחרון",
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
					 
					 						FieldName =  "LastStatusDate",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
					  						FieldsDataType =  "DateTime",
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "LastStatusDate",
					  						ListPropertyPath =  "LastStatusDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LastStatusDate",
					  						DefaultText =  "LastStatusDate",
					  						FullLocalDefaultText =  "תאריך סטטוס אחרון",
					  						ListFieldLable =  "LastStatusDateListLable",
					  						ListLableDefaultText =  "LastStatusDate",
					  						ListLocalDefaultText =  "תאריך סטטוס אחרון",
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
					 
					 						FieldName =  "OrderMoney",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						PMPropertyPath =  "OrderMoney",
					  						ListPropertyPath =  "OrderMoney",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "OrderMoney",
					  						DefaultText =  "Customs Order Money",
					  						FullLocalDefaultText =  "הזמנת כסף",
					  						ListFieldLable =  "OrderMoneyListLable",
					  						ListLableDefaultText =  "Customs Order Money",
					  						ListLocalDefaultText =  "הזמנת כסף",
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
					 
					 						FieldName =  "Team",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
					  						FieldsDataType =  "nText",
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
					  						PMPropertyPath =  "Team",
					  						ListPropertyPath =  "Team",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Team",
					  						DefaultText =  "Team",
					  						FullLocalDefaultText =  "צוות",
					  						ListFieldLable =  "TeamListLable",
					  						ListLableDefaultText =  "Team",
					  						ListLocalDefaultText =  "צוות",
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
					  						ObjectTableName =  "Customs.DeclarationReferantData",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.DeliverySiteType",
					  						MinLength =  0,
					  						MaxLength =  20,
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
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "StorageSiteCode",
					  						DefaultText =  "Storage Site",
					  						FullLocalDefaultText =  "אתר אחסון",
					  						ListFieldLable =  "StorageSiteCodeListLable",
					  						ListLableDefaultText =  "Storage Site Code",
					  						ListLocalDefaultText =  "אתר אחסון",
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
					 
					 						FieldName =  "TaxationDateTime",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "TaxationDateTime",
					  						ListPropertyPath =  "TaxationDateTime",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TaxationDateTime",
					  						DefaultText =  "Taxation Date Time",
					  						FullLocalDefaultText =  "תאריך חישוב מיסים  ",
					  						ListFieldLable =  "TaxationDateTimeListLable",
					  						ListLableDefaultText =  "Taxation Date Time",
					  						ListLocalDefaultText =  "תאריך חישוב מיסים  ",
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
					 
					 						FieldName =  "ProcedureCurrentCode",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.GovernmentProcedureType",
					  						MinLength =  0,
					  						MaxLength =  7,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  7,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ProcedureCurrentCode",
					  						ListPropertyPath =  "ProcedureCurrentCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ProcedureCurrentCode",
					  						DefaultText =  "Government Procedure Type",
					  						FullLocalDefaultText =  "סוג תהליך ",
					  						ListFieldLable =  "ProcedureCurrentCodeListLable",
					  						ListLableDefaultText =  "Procedure Current Name",
					  						ListLocalDefaultText =  "סוג תהליך ",
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
					 
					 						FieldName =  "HatraDate",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						PMPropertyPath =  "HatraDate",
					  						ListPropertyPath =  "HatraDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "HatraDate",
					  						DefaultText =  "Hatra Date",
					  						FullLocalDefaultText =  "תאריך התרה",
					  						ListFieldLable =  "HatraDateListLable",
					  						ListLableDefaultText =  "Hatra Date",
					  						ListLocalDefaultText =  "תאריך התרה",
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
					 
					 						FieldName =  "PaymentDate",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						PMPropertyPath =  "PaymentDate",
					  						ListPropertyPath =  "PaymentDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PaymentDate",
					  						DefaultText =  "Payment Date",
					  						FullLocalDefaultText =  "תאריך תשלום",
					  						ListFieldLable =  "PaymentDateListLable",
					  						ListLableDefaultText =  "Payment Date",
					  						ListLocalDefaultText =  "תאריך תשלום",
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
					 
					 						FieldName =  "ImporterCode",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						PMPropertyPath =  "ImporterCode",
					  						ListPropertyPath =  "ImporterCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ImporterCode",
					  						DefaultText =  "ImporterCode",
					  						FullLocalDefaultText =  "מספר יבואן",
					  						ListFieldLable =  "ImporterCodeListLable",
					  						ListLableDefaultText =  "ImporterCode",
					  						ListLocalDefaultText =  "מספר יבואן",
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
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
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
					  						FullLocalDefaultText =  "מס' לקוח",
					  						ListFieldLable =  "CustomerCodeListLable",
					  						ListLableDefaultText =  "Customer Code",
					  						ListLocalDefaultText =  "מס' לקוח",
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
					 
					 						FieldName =  "ImporterFile",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
					  						FieldsDataType =  "Text",
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
					  						PMPropertyPath =  "ImporterFile",
					  						ListPropertyPath =  "ImporterFile",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ImporterFile",
					  						DefaultText =  "Importer File",
					  						FullLocalDefaultText =  "תיק יבואן",
					  						ListFieldLable =  "ImporterFileListLable",
					  						ListLableDefaultText =  "Importer File",
					  						ListLocalDefaultText =  "תיק יבואן",
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
					 
					 						FieldName =  "AEOImporter",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.Client",
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
					  						PMPropertyPath =  "AEOImporter",
					  						ListPropertyPath =  "AEOImporter",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AEOImporter",
					  						DefaultText =  "לקוח AEO/יבואן מורשה",
					  						ListFieldLable =  "AEOImporterListLable",
					  						ListLableDefaultText =  "AEOImporter",
					  						ListLocalDefaultText =  "לקוח AEO/יבואן מורשה",
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
					 
					 						FieldName =  "FileOpenDate",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "FileOpenDate",
					  						ListPropertyPath =  "FileOpenDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "FileOpenDate",
					  						DefaultText =  "File Open Date",
					  						FullLocalDefaultText =  "תאריך פתיחת תיק",
					  						ListFieldLable =  "FileOpenDateListLable",
					  						ListLableDefaultText =  "File Open Date",
					  						ListLocalDefaultText =  "תאריך פתיחת תיק",
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
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "StorageSiteName",
					  						DefaultText =  "Storage Site",
					  						FullLocalDefaultText =  "שם אתר אחסון",
					  						ListFieldLable =  "StorageSiteNameListLable",
					  						ListLableDefaultText =  "Storage Site Name",
					  						ListLocalDefaultText =  "שם אתר אחסון",
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
					 
					 						FieldName =  "ImporterName",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						PMPropertyPath =  "ImporterName",
					  						ListPropertyPath =  "ImporterName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ImporterName",
					  						DefaultText =  "Importer Name",
					  						FullLocalDefaultText =  "שם יבואן",
					  						ListFieldLable =  "ImporterNameListLable",
					  						ListLableDefaultText =  "Importer Name",
					  						ListLocalDefaultText =  "שם יבואן",
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
					 
					 						FieldName =  "ProcedureCurrentName",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  300,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  300,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ProcedureCurrentName",
					  						ListPropertyPath =  "ProcedureCurrentName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ProcedureCurrentName",
					  						DefaultText =  "Government Procedure Type",
					  						FullLocalDefaultText =  "שם סוג תהליך",
					  						ListFieldLable =  "ProcedureCurrentNameListLable",
					  						ListLableDefaultText =  "Government Procedure Type",
					  						ListLocalDefaultText =  "שם סוג תהליך",
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
					 
					 						FieldName =  "IsPaymentDateNull",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						PMPropertyPath =  "IsPaymentDateNull",
					  						ListPropertyPath =  "IsPaymentDateNull",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsPaymentDateNull",
					  						DefaultText =  "Is PaymentDate Null",
					  						ListFieldLable =  "IsPaymentDateNullListLable",
					  						ListLableDefaultText =  "IsPaymentDateNull",
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
					 
					 						FieldName =  "IsAvailabilityDateNull",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						PMPropertyPath =  "IsAvailabilityDateNull",
					  						ListPropertyPath =  "IsAvailabilityDateNull",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsAvailabilityDateNull",
					  						DefaultText =  "Is AvailabilityDate Null",
					  						ListFieldLable =  "IsAvailabilityDateNullListLable",
					  						ListLableDefaultText =  "Is AvailabilityDate Null",
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
					 
					 						FieldName =  "DeclarationNumber",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						Operator =  "Contains",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DeclarationNumber",
					  						ListPropertyPath =  "DeclarationNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DeclarationNumber",
					  						DefaultText =  "Declaration Number",
					  						FullLocalDefaultText =  "מספר הצהרה",
					  						ListFieldLable =  "DeclarationNumberListLable",
					  						ListLableDefaultText =  "Declaration Number",
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
					 
					 						FieldName =  "IsHatraDateNull",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
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
					  						PMPropertyPath =  "IsHatraDateNull",
					  						ListPropertyPath =  "IsHatraDateNull",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsHatraDateNull",
					  						DefaultText =  "Is PaymentDate Null",
					  						ListFieldLable =  "IsHatraDateNullListLable",
					  						ListLableDefaultText =  "IsHatraDateNull",
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
					 
					 						FieldName =  "RequestedCustomsDocId",
					  						ObjectTableName =  "Customs.DeclarationReferantData",
					  						FieldsDataType =  "Integer",
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
					  						PMPropertyPath =  "RequestedCustomsDocId",
					  						ListPropertyPath =  "RequestedCustomsDocId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.DeclarationReferantData",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "RequestedCustomsDocId",
					  						DefaultText =  "Requested Customs Doc Id",
					  						FullLocalDefaultText =  "מסמך נדרש",
					  						ListFieldLable =  "RequestedCustomsDocIdListLable",
					  						ListLableDefaultText =  "מסמך נדרש",
					  						ListLocalDefaultText =  "Requested Customs DocId",
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
	        QueryGroup DeclarationReferantDataQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "5277", Name = " Query Group" }, queryGroupRepository,tenantQueryGroups);
						QueryGroup DeclarationReferantDataQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "fa77", Name = " Query Group" }, queryGroupRepository,tenantQueryGroups);
				        queryGroupRepository.SubmitChanges();
	        ObjectTable DeclarationReferantDataObjectTable = objectTables.ContainsKey("Customs.DeclarationReferantData") ? objectTables["Customs.DeclarationReferantData"] : null;
            if (DeclarationReferantDataObjectTable == null)
            {
                IWebFreightContext objectContext = WebFreightContext.GetContext(0);  

                DeclarationReferantDataObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Customs.DeclarationReferantData" && d.Tenant == 0).FirstOrDefault();
            }

	         
			List<Feature> addedFeatures = new List<Feature>();
			List<TextCode> addedTextCodes = new List<TextCode>();
			List<Query> addedQueries = new List<Query>();
			List<QueryColumn> addedQueryColumns = new List<QueryColumn>();
			List<AdvancedQueryFilter> addedQueryFilters = new List<AdvancedQueryFilter>();
   

			   TextCode DeclarationReferantDataTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DeclarationReferantData.Q.FilesInProcess", DefaultText = @"Files In Process",LocalDefaultText = "תיקים בטיפול", ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature DeclarationReferantDataFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DeclarationReferantData.Q.FilesInProcess", ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, NameTextCodeCode = "DeclarationReferantDataFeatures.FilesInProcess", NameTextCodeDefaultText = "FilesInProcess", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,DeclarationReferantDataObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode DeclarationReferantDataTextCode_1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DeclarationReferantData.Q.TrackingCases", DefaultText = @"TrackingCases",LocalDefaultText = "תיקים במעקב", ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature DeclarationReferantDataFeature_1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DeclarationReferantData.Q.TrackingCases", ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, NameTextCodeCode = "DeclarationReferantDataFeatures.TrackingCases", NameTextCodeDefaultText = "TrackingCases", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,DeclarationReferantDataObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode DeclarationReferantDataTextCode_2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DeclarationReferantData.Q.AllCases", DefaultText = @"AllCases",LocalDefaultText = "כל התיקים ", ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature DeclarationReferantDataFeature_2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DeclarationReferantData.Q.AllCases", ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, NameTextCodeCode = "DeclarationReferantDataFeatures.AllCases", NameTextCodeDefaultText = "AllCases", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,DeclarationReferantDataObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode DeclarationReferantDataTextCode_3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DeclarationReferantData.Q.FilesInOCR", DefaultText = @"Files In Ocr",LocalDefaultText = "תיקים ב-OCR", ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature DeclarationReferantDataFeature_3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DeclarationReferantData.Q.FilesInOCR", ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, NameTextCodeCode = "DeclarationReferantDataFeatures.FilesInOCR", NameTextCodeDefaultText = "FilesInOCR", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,DeclarationReferantDataObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode DeclarationReferantDataTextCode_4 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DeclarationReferantData.Q.FilesInSivug", DefaultText = @"Files In Sivug",LocalDefaultText = "תיקים בסיווג", ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature DeclarationReferantDataFeature_4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DeclarationReferantData.Q.FilesInSivug", ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, NameTextCodeCode = "DeclarationReferantDataFeatures.FilesInSivug", NameTextCodeDefaultText = "FilesInSivug", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,DeclarationReferantDataObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode DeclarationReferantDataTextCode_5 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DeclarationReferantData.Q.FilesInReview", DefaultText = @"Files In Review",LocalDefaultText = "תיקים בביקורת", ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature DeclarationReferantDataFeature_5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DeclarationReferantData.Q.FilesInReview", ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, NameTextCodeCode = "DeclarationReferantDataFeatures.FilesInReview", NameTextCodeDefaultText = "FilesInReview", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,DeclarationReferantDataObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode DeclarationReferantDataTextCode_6 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DeclarationReferantData.Q.FilesInCreditControl", DefaultText = @"Files In Credit Control",LocalDefaultText = "תיקים בבקרת אשראי", ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature DeclarationReferantDataFeature_6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DeclarationReferantData.Q.FilesInCreditControl", ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, NameTextCodeCode = "DeclarationReferantDataFeatures.FilesInCreditControl", NameTextCodeDefaultText = "FilesInCreditControl", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,DeclarationReferantDataObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode DeclarationReferantDataTextCode_7 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DeclarationReferantData.Q.FilesAvailableFreeOfCharge", DefaultText = @"Files Available free of charge",LocalDefaultText = "תיקים זמינים ללא תשלום", ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature DeclarationReferantDataFeature_7 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DeclarationReferantData.Q.FilesAvailableFreeOfCharge", ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, NameTextCodeCode = "DeclarationReferantDataFeatures.FilesAvailableFreeOfCharge", NameTextCodeDefaultText = "FilesAvailableFreeOfCharge", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,DeclarationReferantDataObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode DeclarationReferantDataTextCode_8 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DeclarationReferantData.Q.FilesInAllInclusive", DefaultText = @"Files All Inclusive",LocalDefaultText = "תיקים בכוללת ", ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature DeclarationReferantDataFeature_8 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DeclarationReferantData.Q.FilesInAllInclusive", ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, NameTextCodeCode = "DeclarationReferantDataFeatures.FilesInAllInclusive", NameTextCodeDefaultText = "FilesInAllInclusive", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,DeclarationReferantDataObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode DeclarationReferantDataTextCode_9 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DeclarationReferantData.Q.FilesRejectedByController", DefaultText = @"Files Rejected By Controller",LocalDefaultText = "תיקים שנדחו עי מבקר", ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature DeclarationReferantDataFeature_9 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DeclarationReferantData.Q.FilesRejectedByController", ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, NameTextCodeCode = "DeclarationReferantDataFeatures.FilesRejectedByController", NameTextCodeDefaultText = "FilesRejectedByController", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,DeclarationReferantDataObjectTable, addedFeatures, addedTextCodes);
 

			   TextCode DeclarationReferantDataTextCode_10 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DeclarationReferantData.Q.FilesRejectedByClassification", DefaultText = @"Files Rejected By Classification",LocalDefaultText = "תיקים שנדחו עי מסווג", ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature DeclarationReferantDataFeature_10 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DeclarationReferantData.Q.FilesRejectedByClassification", ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, NameTextCodeCode = "DeclarationReferantDataFeatures.FilesRejectedByClassification", NameTextCodeDefaultText = "FilesRejectedByClassification", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,DeclarationReferantDataObjectTable, addedFeatures, addedTextCodes);

	        //TextCodeRepository.SubmitChanges();
	        //FeaturesRepository.SubmitChanges();    
	      

			  Query FilesInProcessQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = DeclarationReferantDataTextCode_0.Id, NameTextCodeCode = DeclarationReferantDataTextCode_0.Code, ObjectTableName = "Customs.DeclarationReferantData", Code = "FilesInProcess",  SpotlightDataTemplate = "ReferantSpotlightDataTemplate",  QueryGroupCode = "5277", IndexOrder = 0, Tenant = 0, ObjectTableId = DeclarationReferantDataObjectTable.Id, QuerySection = "Customs.DeclarationReferantData", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = DeclarationReferantDataFeature_0.Id,FeatureUniqeCode= DeclarationReferantDataFeature_0.FeatureUniqeCode, DefaultSortName = "SortedColumns", DefaultSortDirection = "Ascending", Perspective = null }, addedQueries);
	
			 QueryColumn FilesInProcessQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInProcessQuery.Id,QueryCode = FilesInProcessQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.DeclarationReferantData.Favorite" , ColumnWidth = 80 }, addedQueryColumns);

			 QueryColumn FilesInProcessQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInProcessQuery.Id,QueryCode = FilesInProcessQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.DeclarationReferantData.CustomFileNo" , ColumnWidth = 92 }, addedQueryColumns);

			 QueryColumn FilesInProcessQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInProcessQuery.Id,QueryCode = FilesInProcessQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.DeclarationReferantData.CustomerName" , ColumnWidth = 146 }, addedQueryColumns);

			 QueryColumn FilesInProcessQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInProcessQuery.Id,QueryCode = FilesInProcessQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.DeclarationReferantData.OrderNumber" , ColumnWidth = 87 }, addedQueryColumns);

			 QueryColumn FilesInProcessQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInProcessQuery.Id,QueryCode = FilesInProcessQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.DeclarationReferantData.TransportModeId" , ColumnWidth = 58 }, addedQueryColumns);

			 QueryColumn FilesInProcessQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInProcessQuery.Id,QueryCode = FilesInProcessQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Customs.DeclarationReferantData.DeclarationOfficeName" , ColumnWidth = 157 }, addedQueryColumns);

			 QueryColumn FilesInProcessQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInProcessQuery.Id,QueryCode = FilesInProcessQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Customs.DeclarationReferantData.VendorName" , ColumnWidth = 124 }, addedQueryColumns);

			 QueryColumn FilesInProcessQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInProcessQuery.Id,QueryCode = FilesInProcessQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "Customs.DeclarationReferantData.ArrivalDate" , ColumnWidth = 79 }, addedQueryColumns);

			 QueryColumn FilesInProcessQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInProcessQuery.Id,QueryCode = FilesInProcessQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "Customs.DeclarationReferantData.Weight" , ColumnWidth = 75 }, addedQueryColumns);

			 QueryColumn FilesInProcessQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInProcessQuery.Id,QueryCode = FilesInProcessQuery.UniqueCode, IndexOrder = 9, ObjectFieldCode = "Customs.DeclarationReferantData.DeclarationStatusTypeName" , ColumnWidth = 255 }, addedQueryColumns);

			 QueryColumn FilesInProcessQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInProcessQuery.Id,QueryCode = FilesInProcessQuery.UniqueCode, IndexOrder = 10, ObjectFieldCode = "Customs.DeclarationReferantData.PreClassification" , ColumnWidth = 58 }, addedQueryColumns);

			 QueryColumn FilesInProcessQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInProcessQuery.Id,QueryCode = FilesInProcessQuery.UniqueCode, IndexOrder = 11, ObjectFieldCode = "Customs.DeclarationReferantData.ClassificationStatus" , ColumnWidth = 55 }, addedQueryColumns);

			 QueryColumn FilesInProcessQueryColumn_12 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInProcessQuery.Id,QueryCode = FilesInProcessQuery.UniqueCode, IndexOrder = 12, ObjectFieldCode = "Customs.DeclarationReferantData.ControllerStatus" , ColumnWidth = 44 }, addedQueryColumns);

			 QueryColumn FilesInProcessQueryColumn_13 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInProcessQuery.Id,QueryCode = FilesInProcessQuery.UniqueCode, IndexOrder = 13, ObjectFieldCode = "Customs.DeclarationReferantData.CollectionOfMoneyStatus" , ColumnWidth = 50 }, addedQueryColumns);

			 QueryColumn FilesInProcessQueryColumn_14 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInProcessQuery.Id,QueryCode = FilesInProcessQuery.UniqueCode, IndexOrder = 14, ObjectFieldCode = "Customs.DeclarationReferantData.OrderMoney" , ColumnWidth = 50 }, addedQueryColumns);

			 QueryColumn FilesInProcessQueryColumn_15 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInProcessQuery.Id,QueryCode = FilesInProcessQuery.UniqueCode, IndexOrder = 15, ObjectFieldCode = "Customs.DeclarationReferantData.FollowUpDate" , ColumnWidth = 108 }, addedQueryColumns);

			 QueryColumn FilesInProcessQueryColumn_16 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInProcessQuery.Id,QueryCode = FilesInProcessQuery.UniqueCode, IndexOrder = 16, ObjectFieldCode = "Customs.DeclarationReferantData.AvailabilityDate" , ColumnWidth = 108 }, addedQueryColumns);

			 QueryColumn FilesInProcessQueryColumn_17 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInProcessQuery.Id,QueryCode = FilesInProcessQuery.UniqueCode, IndexOrder = 17, ObjectFieldCode = "Customs.DeclarationReferantData.ExceptionReasonsList" , ColumnWidth = 34 }, addedQueryColumns);

			 QueryColumn FilesInProcessQueryColumn_18 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInProcessQuery.Id,QueryCode = FilesInProcessQuery.UniqueCode, IndexOrder = 18, ObjectFieldCode = "Customs.DeclarationReferantData.WithPaper" , ColumnWidth = 69 }, addedQueryColumns);

			 QueryColumn FilesInProcessQueryColumn_19 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInProcessQuery.Id,QueryCode = FilesInProcessQuery.UniqueCode, IndexOrder = 19, ObjectFieldCode = "Customs.DeclarationReferantData.NewFile" , ColumnWidth = 69 }, addedQueryColumns);

			 QueryColumn FilesInProcessQueryColumn_20 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInProcessQuery.Id,QueryCode = FilesInProcessQuery.UniqueCode, IndexOrder = 20, ObjectFieldCode = "Customs.DeclarationReferantData.IsCustomerLogBoxActivated" , ColumnWidth = 68 }, addedQueryColumns);

			 QueryColumn FilesInProcessQueryColumn_21 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInProcessQuery.Id,QueryCode = FilesInProcessQuery.UniqueCode, IndexOrder = 21, ObjectFieldCode = "Customs.DeclarationReferantData.Actions" , ColumnWidth = 68 }, addedQueryColumns);

             AdvancedQueryFilter FilesInProcessQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.DeclarationReferantData.IsCancelled", PredefinedValue = "true",PredefinedValue2 = null, QueryId = FilesInProcessQuery.Id,QueryCode = FilesInProcessQuery.UniqueCode, Tenant = 0,Operator = "NotEqual"}, addedQueryFilters);


             AdvancedQueryFilter FilesInProcessQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.DeclarationReferantData.IsClosedForFollowUp", PredefinedValue = "1",PredefinedValue2 = null, QueryId = FilesInProcessQuery.Id,QueryCode = FilesInProcessQuery.UniqueCode, Tenant = 0,Operator = "NotEqual"}, addedQueryFilters);

  
	      

			  Query TrackingCasesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = DeclarationReferantDataTextCode_1.Id, NameTextCodeCode = DeclarationReferantDataTextCode_1.Code, ObjectTableName = "Customs.DeclarationReferantData", Code = "TrackingCases",  SpotlightDataTemplate = "ReferantSpotlightDataTemplate",  QueryGroupCode = "5277", IndexOrder = 1, Tenant = 0, ObjectTableId = DeclarationReferantDataObjectTable.Id, QuerySection = "Customs.DeclarationReferantData", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = DeclarationReferantDataFeature_1.Id,FeatureUniqeCode= DeclarationReferantDataFeature_1.FeatureUniqeCode, DefaultSortName = "SortedColumns", DefaultSortDirection = "Ascending", Perspective = null }, addedQueries);
	
			 QueryColumn TrackingCasesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TrackingCasesQuery.Id,QueryCode = TrackingCasesQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.DeclarationReferantData.Favorite" , ColumnWidth = 80 }, addedQueryColumns);

			 QueryColumn TrackingCasesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TrackingCasesQuery.Id,QueryCode = TrackingCasesQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.DeclarationReferantData.CustomFileNo" , ColumnWidth = 92 }, addedQueryColumns);

			 QueryColumn TrackingCasesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TrackingCasesQuery.Id,QueryCode = TrackingCasesQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.DeclarationReferantData.CustomerName" , ColumnWidth = 146 }, addedQueryColumns);

			 QueryColumn TrackingCasesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TrackingCasesQuery.Id,QueryCode = TrackingCasesQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.DeclarationReferantData.OrderNumber" , ColumnWidth = 87 }, addedQueryColumns);

			 QueryColumn TrackingCasesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TrackingCasesQuery.Id,QueryCode = TrackingCasesQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.DeclarationReferantData.TransportModeId" , ColumnWidth = 58 }, addedQueryColumns);

			 QueryColumn TrackingCasesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TrackingCasesQuery.Id,QueryCode = TrackingCasesQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Customs.DeclarationReferantData.DeclarationOfficeName" , ColumnWidth = 157 }, addedQueryColumns);

			 QueryColumn TrackingCasesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TrackingCasesQuery.Id,QueryCode = TrackingCasesQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Customs.DeclarationReferantData.VendorName" , ColumnWidth = 124 }, addedQueryColumns);

			 QueryColumn TrackingCasesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TrackingCasesQuery.Id,QueryCode = TrackingCasesQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "Customs.DeclarationReferantData.ArrivalDate" , ColumnWidth = 79 }, addedQueryColumns);

			 QueryColumn TrackingCasesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TrackingCasesQuery.Id,QueryCode = TrackingCasesQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "Customs.DeclarationReferantData.Weight" , ColumnWidth = 75 }, addedQueryColumns);

			 QueryColumn TrackingCasesQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TrackingCasesQuery.Id,QueryCode = TrackingCasesQuery.UniqueCode, IndexOrder = 9, ObjectFieldCode = "Customs.DeclarationReferantData.DeclarationStatusTypeName" , ColumnWidth = 255 }, addedQueryColumns);

			 QueryColumn TrackingCasesQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TrackingCasesQuery.Id,QueryCode = TrackingCasesQuery.UniqueCode, IndexOrder = 10, ObjectFieldCode = "Customs.DeclarationReferantData.PreClassification" , ColumnWidth = 58 }, addedQueryColumns);

			 QueryColumn TrackingCasesQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TrackingCasesQuery.Id,QueryCode = TrackingCasesQuery.UniqueCode, IndexOrder = 11, ObjectFieldCode = "Customs.DeclarationReferantData.ClassificationStatus" , ColumnWidth = 55 }, addedQueryColumns);

			 QueryColumn TrackingCasesQueryColumn_12 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TrackingCasesQuery.Id,QueryCode = TrackingCasesQuery.UniqueCode, IndexOrder = 12, ObjectFieldCode = "Customs.DeclarationReferantData.ControllerStatus" , ColumnWidth = 44 }, addedQueryColumns);

			 QueryColumn TrackingCasesQueryColumn_13 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TrackingCasesQuery.Id,QueryCode = TrackingCasesQuery.UniqueCode, IndexOrder = 13, ObjectFieldCode = "Customs.DeclarationReferantData.CollectionOfMoneyStatus" , ColumnWidth = 50 }, addedQueryColumns);

			 QueryColumn TrackingCasesQueryColumn_14 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TrackingCasesQuery.Id,QueryCode = TrackingCasesQuery.UniqueCode, IndexOrder = 14, ObjectFieldCode = "Customs.DeclarationReferantData.OrderMoney" , ColumnWidth = 50 }, addedQueryColumns);

			 QueryColumn TrackingCasesQueryColumn_15 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TrackingCasesQuery.Id,QueryCode = TrackingCasesQuery.UniqueCode, IndexOrder = 15, ObjectFieldCode = "Customs.DeclarationReferantData.FollowUpDate" , ColumnWidth = 108 }, addedQueryColumns);

			 QueryColumn TrackingCasesQueryColumn_16 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TrackingCasesQuery.Id,QueryCode = TrackingCasesQuery.UniqueCode, IndexOrder = 16, ObjectFieldCode = "Customs.DeclarationReferantData.AvailabilityDate" , ColumnWidth = 108 }, addedQueryColumns);

			 QueryColumn TrackingCasesQueryColumn_17 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TrackingCasesQuery.Id,QueryCode = TrackingCasesQuery.UniqueCode, IndexOrder = 17, ObjectFieldCode = "Customs.DeclarationReferantData.ExceptionReasonsList" , ColumnWidth = 34 }, addedQueryColumns);

			 QueryColumn TrackingCasesQueryColumn_18 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TrackingCasesQuery.Id,QueryCode = TrackingCasesQuery.UniqueCode, IndexOrder = 18, ObjectFieldCode = "Customs.DeclarationReferantData.WithPaper" , ColumnWidth = 69 }, addedQueryColumns);

			 QueryColumn TrackingCasesQueryColumn_19 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TrackingCasesQuery.Id,QueryCode = TrackingCasesQuery.UniqueCode, IndexOrder = 19, ObjectFieldCode = "Customs.DeclarationReferantData.NewFile" , ColumnWidth = 69 }, addedQueryColumns);

			 QueryColumn TrackingCasesQueryColumn_20 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TrackingCasesQuery.Id,QueryCode = TrackingCasesQuery.UniqueCode, IndexOrder = 20, ObjectFieldCode = "Customs.DeclarationReferantData.IsCustomerLogBoxActivated" , ColumnWidth = 68 }, addedQueryColumns);

			 QueryColumn TrackingCasesQueryColumn_21 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = TrackingCasesQuery.Id,QueryCode = TrackingCasesQuery.UniqueCode, IndexOrder = 21, ObjectFieldCode = "Customs.DeclarationReferantData.Actions" , ColumnWidth = 68 }, addedQueryColumns);

             AdvancedQueryFilter TrackingCasesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.DeclarationReferantData.FollowUpDate", PredefinedValue = "Today",PredefinedValue2 = null, QueryId = TrackingCasesQuery.Id,QueryCode = TrackingCasesQuery.UniqueCode, Tenant = 0}, addedQueryFilters);


             AdvancedQueryFilter TrackingCasesQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.DeclarationReferantData.IsCancelled", PredefinedValue = "true",PredefinedValue2 = null, QueryId = TrackingCasesQuery.Id,QueryCode = TrackingCasesQuery.UniqueCode, Tenant = 0,Operator = "NotEqual"}, addedQueryFilters);

  
	      

			  Query AllCasesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = DeclarationReferantDataTextCode_2.Id, NameTextCodeCode = DeclarationReferantDataTextCode_2.Code, ObjectTableName = "Customs.DeclarationReferantData", Code = "AllCases",  SpotlightDataTemplate = "ReferantSpotlightDataTemplate",  QueryGroupCode = "5277", IndexOrder = 2, Tenant = 0, ObjectTableId = DeclarationReferantDataObjectTable.Id, QuerySection = "Customs.DeclarationReferantData", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = DeclarationReferantDataFeature_2.Id,FeatureUniqeCode= DeclarationReferantDataFeature_2.FeatureUniqeCode, DefaultSortName = "SortedColumns", DefaultSortDirection = "Ascending", Perspective = null }, addedQueries);
	
			 QueryColumn AllCasesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCasesQuery.Id,QueryCode = AllCasesQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.DeclarationReferantData.Favorite" , ColumnWidth = 80 }, addedQueryColumns);

			 QueryColumn AllCasesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCasesQuery.Id,QueryCode = AllCasesQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.DeclarationReferantData.CustomFileNo" , ColumnWidth = 92 }, addedQueryColumns);

			 QueryColumn AllCasesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCasesQuery.Id,QueryCode = AllCasesQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.DeclarationReferantData.CustomerName" , ColumnWidth = 146 }, addedQueryColumns);

			 QueryColumn AllCasesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCasesQuery.Id,QueryCode = AllCasesQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.DeclarationReferantData.OrderNumber" , ColumnWidth = 87 }, addedQueryColumns);

			 QueryColumn AllCasesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCasesQuery.Id,QueryCode = AllCasesQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.DeclarationReferantData.TransportModeId" , ColumnWidth = 58 }, addedQueryColumns);

			 QueryColumn AllCasesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCasesQuery.Id,QueryCode = AllCasesQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Customs.DeclarationReferantData.DeclarationOfficeName" , ColumnWidth = 157 }, addedQueryColumns);

			 QueryColumn AllCasesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCasesQuery.Id,QueryCode = AllCasesQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Customs.DeclarationReferantData.VendorName" , ColumnWidth = 124 }, addedQueryColumns);

			 QueryColumn AllCasesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCasesQuery.Id,QueryCode = AllCasesQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "Customs.DeclarationReferantData.ArrivalDate" , ColumnWidth = 79 }, addedQueryColumns);

			 QueryColumn AllCasesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCasesQuery.Id,QueryCode = AllCasesQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "Customs.DeclarationReferantData.Weight" , ColumnWidth = 75 }, addedQueryColumns);

			 QueryColumn AllCasesQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCasesQuery.Id,QueryCode = AllCasesQuery.UniqueCode, IndexOrder = 9, ObjectFieldCode = "Customs.DeclarationReferantData.DeclarationStatusTypeName" , ColumnWidth = 255 }, addedQueryColumns);

			 QueryColumn AllCasesQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCasesQuery.Id,QueryCode = AllCasesQuery.UniqueCode, IndexOrder = 10, ObjectFieldCode = "Customs.DeclarationReferantData.PreClassification" , ColumnWidth = 58 }, addedQueryColumns);

			 QueryColumn AllCasesQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCasesQuery.Id,QueryCode = AllCasesQuery.UniqueCode, IndexOrder = 11, ObjectFieldCode = "Customs.DeclarationReferantData.ClassificationStatus" , ColumnWidth = 55 }, addedQueryColumns);

			 QueryColumn AllCasesQueryColumn_12 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCasesQuery.Id,QueryCode = AllCasesQuery.UniqueCode, IndexOrder = 12, ObjectFieldCode = "Customs.DeclarationReferantData.ControllerStatus" , ColumnWidth = 44 }, addedQueryColumns);

			 QueryColumn AllCasesQueryColumn_13 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCasesQuery.Id,QueryCode = AllCasesQuery.UniqueCode, IndexOrder = 13, ObjectFieldCode = "Customs.DeclarationReferantData.CollectionOfMoneyStatus" , ColumnWidth = 50 }, addedQueryColumns);

			 QueryColumn AllCasesQueryColumn_14 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCasesQuery.Id,QueryCode = AllCasesQuery.UniqueCode, IndexOrder = 14, ObjectFieldCode = "Customs.DeclarationReferantData.OrderMoney" , ColumnWidth = 50 }, addedQueryColumns);

			 QueryColumn AllCasesQueryColumn_15 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCasesQuery.Id,QueryCode = AllCasesQuery.UniqueCode, IndexOrder = 15, ObjectFieldCode = "Customs.DeclarationReferantData.FollowUpDate" , ColumnWidth = 108 }, addedQueryColumns);

			 QueryColumn AllCasesQueryColumn_16 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCasesQuery.Id,QueryCode = AllCasesQuery.UniqueCode, IndexOrder = 16, ObjectFieldCode = "Customs.DeclarationReferantData.AvailabilityDate" , ColumnWidth = 108 }, addedQueryColumns);

			 QueryColumn AllCasesQueryColumn_17 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCasesQuery.Id,QueryCode = AllCasesQuery.UniqueCode, IndexOrder = 17, ObjectFieldCode = "Customs.DeclarationReferantData.ExceptionReasonsList" , ColumnWidth = 34 }, addedQueryColumns);

			 QueryColumn AllCasesQueryColumn_18 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCasesQuery.Id,QueryCode = AllCasesQuery.UniqueCode, IndexOrder = 18, ObjectFieldCode = "Customs.DeclarationReferantData.WithPaper" , ColumnWidth = 69 }, addedQueryColumns);

			 QueryColumn AllCasesQueryColumn_19 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCasesQuery.Id,QueryCode = AllCasesQuery.UniqueCode, IndexOrder = 19, ObjectFieldCode = "Customs.DeclarationReferantData.NewFile" , ColumnWidth = 69 }, addedQueryColumns);

			 QueryColumn AllCasesQueryColumn_20 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCasesQuery.Id,QueryCode = AllCasesQuery.UniqueCode, IndexOrder = 20, ObjectFieldCode = "Customs.DeclarationReferantData.IsCustomerLogBoxActivated" , ColumnWidth = 68 }, addedQueryColumns);

			 QueryColumn AllCasesQueryColumn_21 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCasesQuery.Id,QueryCode = AllCasesQuery.UniqueCode, IndexOrder = 21, ObjectFieldCode = "Customs.DeclarationReferantData.Actions" , ColumnWidth = 68 }, addedQueryColumns);

             AdvancedQueryFilter AllCasesQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.DeclarationReferantData.IsCancelled", PredefinedValue = "true",PredefinedValue2 = null, QueryId = AllCasesQuery.Id,QueryCode = AllCasesQuery.UniqueCode, Tenant = 0,Operator = "NotEqual"}, addedQueryFilters);

  
	      

			  Query FilesInOCRQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = DeclarationReferantDataTextCode_3.Id, NameTextCodeCode = DeclarationReferantDataTextCode_3.Code, ObjectTableName = "Customs.DeclarationReferantData", Code = "FilesInOCR",  QueryGroupCode = "5277", IndexOrder = 3, Tenant = 0, ObjectTableId = DeclarationReferantDataObjectTable.Id, QuerySection = "Customs.DeclarationReferantData", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = DeclarationReferantDataFeature_3.Id,FeatureUniqeCode= DeclarationReferantDataFeature_3.FeatureUniqeCode, DefaultSortName = "SortedColumns", DefaultSortDirection = "Ascending", Perspective = null }, addedQueries);
	
			 QueryColumn FilesInOCRQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInOCRQuery.Id,QueryCode = FilesInOCRQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.DeclarationReferantData.Favorite" , ColumnWidth = 80 }, addedQueryColumns);

			 QueryColumn FilesInOCRQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInOCRQuery.Id,QueryCode = FilesInOCRQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.DeclarationReferantData.CustomFileNo" , ColumnWidth = 92 }, addedQueryColumns);

			 QueryColumn FilesInOCRQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInOCRQuery.Id,QueryCode = FilesInOCRQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.DeclarationReferantData.CustomerName" , ColumnWidth = 146 }, addedQueryColumns);

			 QueryColumn FilesInOCRQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInOCRQuery.Id,QueryCode = FilesInOCRQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.DeclarationReferantData.OrderNumber" , ColumnWidth = 87 }, addedQueryColumns);

			 QueryColumn FilesInOCRQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInOCRQuery.Id,QueryCode = FilesInOCRQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.DeclarationReferantData.TransportModeId" , ColumnWidth = 58 }, addedQueryColumns);

			 QueryColumn FilesInOCRQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInOCRQuery.Id,QueryCode = FilesInOCRQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Customs.DeclarationReferantData.DeclarationOfficeName" , ColumnWidth = 157 }, addedQueryColumns);

			 QueryColumn FilesInOCRQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInOCRQuery.Id,QueryCode = FilesInOCRQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Customs.DeclarationReferantData.VendorName" , ColumnWidth = 124 }, addedQueryColumns);

			 QueryColumn FilesInOCRQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInOCRQuery.Id,QueryCode = FilesInOCRQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "Customs.DeclarationReferantData.ArrivalDate" , ColumnWidth = 79 }, addedQueryColumns);

			 QueryColumn FilesInOCRQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInOCRQuery.Id,QueryCode = FilesInOCRQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "Customs.DeclarationReferantData.Weight" , ColumnWidth = 75 }, addedQueryColumns);

			 QueryColumn FilesInOCRQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInOCRQuery.Id,QueryCode = FilesInOCRQuery.UniqueCode, IndexOrder = 9, ObjectFieldCode = "Customs.DeclarationReferantData.DeclarationStatusTypeName" , ColumnWidth = 255 }, addedQueryColumns);

			 QueryColumn FilesInOCRQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInOCRQuery.Id,QueryCode = FilesInOCRQuery.UniqueCode, IndexOrder = 10, ObjectFieldCode = "Customs.DeclarationReferantData.PreClassification" , ColumnWidth = 58 }, addedQueryColumns);

			 QueryColumn FilesInOCRQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInOCRQuery.Id,QueryCode = FilesInOCRQuery.UniqueCode, IndexOrder = 11, ObjectFieldCode = "Customs.DeclarationReferantData.ClassificationStatus" , ColumnWidth = 55 }, addedQueryColumns);

			 QueryColumn FilesInOCRQueryColumn_12 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInOCRQuery.Id,QueryCode = FilesInOCRQuery.UniqueCode, IndexOrder = 12, ObjectFieldCode = "Customs.DeclarationReferantData.ControllerStatus" , ColumnWidth = 44 }, addedQueryColumns);

			 QueryColumn FilesInOCRQueryColumn_13 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInOCRQuery.Id,QueryCode = FilesInOCRQuery.UniqueCode, IndexOrder = 13, ObjectFieldCode = "Customs.DeclarationReferantData.CollectionOfMoneyStatus" , ColumnWidth = 50 }, addedQueryColumns);

			 QueryColumn FilesInOCRQueryColumn_14 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInOCRQuery.Id,QueryCode = FilesInOCRQuery.UniqueCode, IndexOrder = 14, ObjectFieldCode = "Customs.DeclarationReferantData.OrderMoney" , ColumnWidth = 50 }, addedQueryColumns);

			 QueryColumn FilesInOCRQueryColumn_15 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInOCRQuery.Id,QueryCode = FilesInOCRQuery.UniqueCode, IndexOrder = 15, ObjectFieldCode = "Customs.DeclarationReferantData.FollowUpDate" , ColumnWidth = 108 }, addedQueryColumns);

			 QueryColumn FilesInOCRQueryColumn_16 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInOCRQuery.Id,QueryCode = FilesInOCRQuery.UniqueCode, IndexOrder = 16, ObjectFieldCode = "Customs.DeclarationReferantData.AvailabilityDate" , ColumnWidth = 108 }, addedQueryColumns);

			 QueryColumn FilesInOCRQueryColumn_17 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInOCRQuery.Id,QueryCode = FilesInOCRQuery.UniqueCode, IndexOrder = 17, ObjectFieldCode = "Customs.DeclarationReferantData.ExceptionReasonsList" , ColumnWidth = 34 }, addedQueryColumns);

			 QueryColumn FilesInOCRQueryColumn_18 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInOCRQuery.Id,QueryCode = FilesInOCRQuery.UniqueCode, IndexOrder = 18, ObjectFieldCode = "Customs.DeclarationReferantData.WithPaper" , ColumnWidth = 69 }, addedQueryColumns);

			 QueryColumn FilesInOCRQueryColumn_19 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInOCRQuery.Id,QueryCode = FilesInOCRQuery.UniqueCode, IndexOrder = 19, ObjectFieldCode = "Customs.DeclarationReferantData.NewFile" , ColumnWidth = 69 }, addedQueryColumns);

			 QueryColumn FilesInOCRQueryColumn_20 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInOCRQuery.Id,QueryCode = FilesInOCRQuery.UniqueCode, IndexOrder = 20, ObjectFieldCode = "Customs.DeclarationReferantData.IsCustomerLogBoxActivated" , ColumnWidth = 68 }, addedQueryColumns);

			 QueryColumn FilesInOCRQueryColumn_21 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInOCRQuery.Id,QueryCode = FilesInOCRQuery.UniqueCode, IndexOrder = 21, ObjectFieldCode = "Customs.DeclarationReferantData.Actions" , ColumnWidth = 68 }, addedQueryColumns);

             AdvancedQueryFilter FilesInOCRQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.DeclarationReferantData.PreClassification", PredefinedValue = "P",PredefinedValue2 = null, QueryId = FilesInOCRQuery.Id,QueryCode = FilesInOCRQuery.UniqueCode, Tenant = 0,Operator = "Equal"}, addedQueryFilters);


             AdvancedQueryFilter FilesInOCRQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.DeclarationReferantData.IsCancelled", PredefinedValue = "true",PredefinedValue2 = null, QueryId = FilesInOCRQuery.Id,QueryCode = FilesInOCRQuery.UniqueCode, Tenant = 0,Operator = "NotEqual"}, addedQueryFilters);

  
	      

			  Query FilesInSivugQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = DeclarationReferantDataTextCode_4.Id, NameTextCodeCode = DeclarationReferantDataTextCode_4.Code, ObjectTableName = "Customs.DeclarationReferantData", Code = "FilesInSivug",  QueryGroupCode = "5277", IndexOrder = 4, Tenant = 0, ObjectTableId = DeclarationReferantDataObjectTable.Id, QuerySection = "Customs.DeclarationReferantData", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = DeclarationReferantDataFeature_4.Id,FeatureUniqeCode= DeclarationReferantDataFeature_4.FeatureUniqeCode, DefaultSortName = "SortedColumns", DefaultSortDirection = "Ascending", Perspective = null }, addedQueries);
	
			 QueryColumn FilesInSivugQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInSivugQuery.Id,QueryCode = FilesInSivugQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.DeclarationReferantData.Favorite" , ColumnWidth = 80 }, addedQueryColumns);

			 QueryColumn FilesInSivugQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInSivugQuery.Id,QueryCode = FilesInSivugQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.DeclarationReferantData.CustomFileNo" , ColumnWidth = 92 }, addedQueryColumns);

			 QueryColumn FilesInSivugQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInSivugQuery.Id,QueryCode = FilesInSivugQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.DeclarationReferantData.CustomerName" , ColumnWidth = 146 }, addedQueryColumns);

			 QueryColumn FilesInSivugQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInSivugQuery.Id,QueryCode = FilesInSivugQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.DeclarationReferantData.OrderNumber" , ColumnWidth = 87 }, addedQueryColumns);

			 QueryColumn FilesInSivugQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInSivugQuery.Id,QueryCode = FilesInSivugQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.DeclarationReferantData.TransportModeId" , ColumnWidth = 58 }, addedQueryColumns);

			 QueryColumn FilesInSivugQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInSivugQuery.Id,QueryCode = FilesInSivugQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Customs.DeclarationReferantData.DeclarationOfficeName" , ColumnWidth = 157 }, addedQueryColumns);

			 QueryColumn FilesInSivugQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInSivugQuery.Id,QueryCode = FilesInSivugQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Customs.DeclarationReferantData.VendorName" , ColumnWidth = 124 }, addedQueryColumns);

			 QueryColumn FilesInSivugQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInSivugQuery.Id,QueryCode = FilesInSivugQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "Customs.DeclarationReferantData.ArrivalDate" , ColumnWidth = 79 }, addedQueryColumns);

			 QueryColumn FilesInSivugQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInSivugQuery.Id,QueryCode = FilesInSivugQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "Customs.DeclarationReferantData.Weight" , ColumnWidth = 75 }, addedQueryColumns);

			 QueryColumn FilesInSivugQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInSivugQuery.Id,QueryCode = FilesInSivugQuery.UniqueCode, IndexOrder = 9, ObjectFieldCode = "Customs.DeclarationReferantData.DeclarationStatusTypeName" , ColumnWidth = 255 }, addedQueryColumns);

			 QueryColumn FilesInSivugQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInSivugQuery.Id,QueryCode = FilesInSivugQuery.UniqueCode, IndexOrder = 10, ObjectFieldCode = "Customs.DeclarationReferantData.PreClassification" , ColumnWidth = 58 }, addedQueryColumns);

			 QueryColumn FilesInSivugQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInSivugQuery.Id,QueryCode = FilesInSivugQuery.UniqueCode, IndexOrder = 11, ObjectFieldCode = "Customs.DeclarationReferantData.ClassificationStatus" , ColumnWidth = 55 }, addedQueryColumns);

			 QueryColumn FilesInSivugQueryColumn_12 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInSivugQuery.Id,QueryCode = FilesInSivugQuery.UniqueCode, IndexOrder = 12, ObjectFieldCode = "Customs.DeclarationReferantData.ControllerStatus" , ColumnWidth = 44 }, addedQueryColumns);

			 QueryColumn FilesInSivugQueryColumn_13 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInSivugQuery.Id,QueryCode = FilesInSivugQuery.UniqueCode, IndexOrder = 13, ObjectFieldCode = "Customs.DeclarationReferantData.CollectionOfMoneyStatus" , ColumnWidth = 50 }, addedQueryColumns);

			 QueryColumn FilesInSivugQueryColumn_14 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInSivugQuery.Id,QueryCode = FilesInSivugQuery.UniqueCode, IndexOrder = 14, ObjectFieldCode = "Customs.DeclarationReferantData.OrderMoney" , ColumnWidth = 50 }, addedQueryColumns);

			 QueryColumn FilesInSivugQueryColumn_15 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInSivugQuery.Id,QueryCode = FilesInSivugQuery.UniqueCode, IndexOrder = 15, ObjectFieldCode = "Customs.DeclarationReferantData.FollowUpDate" , ColumnWidth = 108 }, addedQueryColumns);

			 QueryColumn FilesInSivugQueryColumn_16 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInSivugQuery.Id,QueryCode = FilesInSivugQuery.UniqueCode, IndexOrder = 16, ObjectFieldCode = "Customs.DeclarationReferantData.AvailabilityDate" , ColumnWidth = 108 }, addedQueryColumns);

			 QueryColumn FilesInSivugQueryColumn_17 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInSivugQuery.Id,QueryCode = FilesInSivugQuery.UniqueCode, IndexOrder = 17, ObjectFieldCode = "Customs.DeclarationReferantData.ExceptionReasonsList" , ColumnWidth = 34 }, addedQueryColumns);

			 QueryColumn FilesInSivugQueryColumn_18 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInSivugQuery.Id,QueryCode = FilesInSivugQuery.UniqueCode, IndexOrder = 18, ObjectFieldCode = "Customs.DeclarationReferantData.WithPaper" , ColumnWidth = 69 }, addedQueryColumns);

			 QueryColumn FilesInSivugQueryColumn_19 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInSivugQuery.Id,QueryCode = FilesInSivugQuery.UniqueCode, IndexOrder = 19, ObjectFieldCode = "Customs.DeclarationReferantData.NewFile" , ColumnWidth = 69 }, addedQueryColumns);

			 QueryColumn FilesInSivugQueryColumn_20 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInSivugQuery.Id,QueryCode = FilesInSivugQuery.UniqueCode, IndexOrder = 20, ObjectFieldCode = "Customs.DeclarationReferantData.IsCustomerLogBoxActivated" , ColumnWidth = 68 }, addedQueryColumns);

			 QueryColumn FilesInSivugQueryColumn_21 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInSivugQuery.Id,QueryCode = FilesInSivugQuery.UniqueCode, IndexOrder = 21, ObjectFieldCode = "Customs.DeclarationReferantData.Actions" , ColumnWidth = 68 }, addedQueryColumns);

             AdvancedQueryFilter FilesInSivugQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.DeclarationReferantData.ClassificationStatus", PredefinedValue = "P",PredefinedValue2 = null, QueryId = FilesInSivugQuery.Id,QueryCode = FilesInSivugQuery.UniqueCode, Tenant = 0,Operator = "Equal"}, addedQueryFilters);


             AdvancedQueryFilter FilesInSivugQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.DeclarationReferantData.IsCancelled", PredefinedValue = "true",PredefinedValue2 = null, QueryId = FilesInSivugQuery.Id,QueryCode = FilesInSivugQuery.UniqueCode, Tenant = 0,Operator = "NotEqual"}, addedQueryFilters);

  
	      

			  Query FilesInReviewQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = DeclarationReferantDataTextCode_5.Id, NameTextCodeCode = DeclarationReferantDataTextCode_5.Code, ObjectTableName = "Customs.DeclarationReferantData", Code = "FilesInReview",  QueryGroupCode = "5277", IndexOrder = 5, Tenant = 0, ObjectTableId = DeclarationReferantDataObjectTable.Id, QuerySection = "Customs.DeclarationReferantData", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = DeclarationReferantDataFeature_5.Id,FeatureUniqeCode= DeclarationReferantDataFeature_5.FeatureUniqeCode, DefaultSortName = "SortedColumns", DefaultSortDirection = "Ascending", Perspective = null }, addedQueries);
	
			 QueryColumn FilesInReviewQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInReviewQuery.Id,QueryCode = FilesInReviewQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.DeclarationReferantData.Favorite" , ColumnWidth = 80 }, addedQueryColumns);

			 QueryColumn FilesInReviewQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInReviewQuery.Id,QueryCode = FilesInReviewQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.DeclarationReferantData.CustomFileNo" , ColumnWidth = 92 }, addedQueryColumns);

			 QueryColumn FilesInReviewQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInReviewQuery.Id,QueryCode = FilesInReviewQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.DeclarationReferantData.CustomerName" , ColumnWidth = 146 }, addedQueryColumns);

			 QueryColumn FilesInReviewQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInReviewQuery.Id,QueryCode = FilesInReviewQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.DeclarationReferantData.OrderNumber" , ColumnWidth = 87 }, addedQueryColumns);

			 QueryColumn FilesInReviewQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInReviewQuery.Id,QueryCode = FilesInReviewQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.DeclarationReferantData.TransportModeId" , ColumnWidth = 58 }, addedQueryColumns);

			 QueryColumn FilesInReviewQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInReviewQuery.Id,QueryCode = FilesInReviewQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Customs.DeclarationReferantData.DeclarationOfficeName" , ColumnWidth = 157 }, addedQueryColumns);

			 QueryColumn FilesInReviewQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInReviewQuery.Id,QueryCode = FilesInReviewQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Customs.DeclarationReferantData.VendorName" , ColumnWidth = 124 }, addedQueryColumns);

			 QueryColumn FilesInReviewQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInReviewQuery.Id,QueryCode = FilesInReviewQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "Customs.DeclarationReferantData.ArrivalDate" , ColumnWidth = 79 }, addedQueryColumns);

			 QueryColumn FilesInReviewQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInReviewQuery.Id,QueryCode = FilesInReviewQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "Customs.DeclarationReferantData.Weight" , ColumnWidth = 75 }, addedQueryColumns);

			 QueryColumn FilesInReviewQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInReviewQuery.Id,QueryCode = FilesInReviewQuery.UniqueCode, IndexOrder = 9, ObjectFieldCode = "Customs.DeclarationReferantData.DeclarationStatusTypeName" , ColumnWidth = 255 }, addedQueryColumns);

			 QueryColumn FilesInReviewQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInReviewQuery.Id,QueryCode = FilesInReviewQuery.UniqueCode, IndexOrder = 10, ObjectFieldCode = "Customs.DeclarationReferantData.PreClassification" , ColumnWidth = 58 }, addedQueryColumns);

			 QueryColumn FilesInReviewQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInReviewQuery.Id,QueryCode = FilesInReviewQuery.UniqueCode, IndexOrder = 11, ObjectFieldCode = "Customs.DeclarationReferantData.ClassificationStatus" , ColumnWidth = 55 }, addedQueryColumns);

			 QueryColumn FilesInReviewQueryColumn_12 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInReviewQuery.Id,QueryCode = FilesInReviewQuery.UniqueCode, IndexOrder = 12, ObjectFieldCode = "Customs.DeclarationReferantData.ControllerStatus" , ColumnWidth = 44 }, addedQueryColumns);

			 QueryColumn FilesInReviewQueryColumn_13 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInReviewQuery.Id,QueryCode = FilesInReviewQuery.UniqueCode, IndexOrder = 13, ObjectFieldCode = "Customs.DeclarationReferantData.CollectionOfMoneyStatus" , ColumnWidth = 50 }, addedQueryColumns);

			 QueryColumn FilesInReviewQueryColumn_14 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInReviewQuery.Id,QueryCode = FilesInReviewQuery.UniqueCode, IndexOrder = 14, ObjectFieldCode = "Customs.DeclarationReferantData.OrderMoney" , ColumnWidth = 50 }, addedQueryColumns);

			 QueryColumn FilesInReviewQueryColumn_15 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInReviewQuery.Id,QueryCode = FilesInReviewQuery.UniqueCode, IndexOrder = 15, ObjectFieldCode = "Customs.DeclarationReferantData.FollowUpDate" , ColumnWidth = 108 }, addedQueryColumns);

			 QueryColumn FilesInReviewQueryColumn_16 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInReviewQuery.Id,QueryCode = FilesInReviewQuery.UniqueCode, IndexOrder = 16, ObjectFieldCode = "Customs.DeclarationReferantData.AvailabilityDate" , ColumnWidth = 108 }, addedQueryColumns);

			 QueryColumn FilesInReviewQueryColumn_17 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInReviewQuery.Id,QueryCode = FilesInReviewQuery.UniqueCode, IndexOrder = 17, ObjectFieldCode = "Customs.DeclarationReferantData.ExceptionReasonsList" , ColumnWidth = 34 }, addedQueryColumns);

			 QueryColumn FilesInReviewQueryColumn_18 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInReviewQuery.Id,QueryCode = FilesInReviewQuery.UniqueCode, IndexOrder = 18, ObjectFieldCode = "Customs.DeclarationReferantData.WithPaper" , ColumnWidth = 69 }, addedQueryColumns);

			 QueryColumn FilesInReviewQueryColumn_19 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInReviewQuery.Id,QueryCode = FilesInReviewQuery.UniqueCode, IndexOrder = 19, ObjectFieldCode = "Customs.DeclarationReferantData.NewFile" , ColumnWidth = 69 }, addedQueryColumns);

			 QueryColumn FilesInReviewQueryColumn_20 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInReviewQuery.Id,QueryCode = FilesInReviewQuery.UniqueCode, IndexOrder = 20, ObjectFieldCode = "Customs.DeclarationReferantData.IsCustomerLogBoxActivated" , ColumnWidth = 68 }, addedQueryColumns);

			 QueryColumn FilesInReviewQueryColumn_21 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInReviewQuery.Id,QueryCode = FilesInReviewQuery.UniqueCode, IndexOrder = 21, ObjectFieldCode = "Customs.DeclarationReferantData.Actions" , ColumnWidth = 68 }, addedQueryColumns);

             AdvancedQueryFilter FilesInReviewQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.DeclarationReferantData.ControllerStatus", PredefinedValue = "P",PredefinedValue2 = null, QueryId = FilesInReviewQuery.Id,QueryCode = FilesInReviewQuery.UniqueCode, Tenant = 0,Operator = "Equal"}, addedQueryFilters);


             AdvancedQueryFilter FilesInReviewQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.DeclarationReferantData.IsCancelled", PredefinedValue = "true",PredefinedValue2 = null, QueryId = FilesInReviewQuery.Id,QueryCode = FilesInReviewQuery.UniqueCode, Tenant = 0,Operator = "NotEqual"}, addedQueryFilters);

  
	      

			  Query FilesInCreditControlQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = DeclarationReferantDataTextCode_6.Id, NameTextCodeCode = DeclarationReferantDataTextCode_6.Code, ObjectTableName = "Customs.DeclarationReferantData", Code = "FilesInCreditControl",  QueryGroupCode = "5277", IndexOrder = 6, Tenant = 0, ObjectTableId = DeclarationReferantDataObjectTable.Id, QuerySection = "Customs.DeclarationReferantData", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = DeclarationReferantDataFeature_6.Id,FeatureUniqeCode= DeclarationReferantDataFeature_6.FeatureUniqeCode, DefaultSortName = "SortedColumns", DefaultSortDirection = "Ascending", Perspective = null }, addedQueries);
	
			 QueryColumn FilesInCreditControlQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInCreditControlQuery.Id,QueryCode = FilesInCreditControlQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.DeclarationReferantData.Favorite" , ColumnWidth = 80 }, addedQueryColumns);

			 QueryColumn FilesInCreditControlQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInCreditControlQuery.Id,QueryCode = FilesInCreditControlQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.DeclarationReferantData.CustomFileNo" , ColumnWidth = 92 }, addedQueryColumns);

			 QueryColumn FilesInCreditControlQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInCreditControlQuery.Id,QueryCode = FilesInCreditControlQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.DeclarationReferantData.CustomerName" , ColumnWidth = 146 }, addedQueryColumns);

			 QueryColumn FilesInCreditControlQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInCreditControlQuery.Id,QueryCode = FilesInCreditControlQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.DeclarationReferantData.OrderNumber" , ColumnWidth = 87 }, addedQueryColumns);

			 QueryColumn FilesInCreditControlQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInCreditControlQuery.Id,QueryCode = FilesInCreditControlQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.DeclarationReferantData.TransportModeId" , ColumnWidth = 58 }, addedQueryColumns);

			 QueryColumn FilesInCreditControlQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInCreditControlQuery.Id,QueryCode = FilesInCreditControlQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Customs.DeclarationReferantData.DeclarationOfficeName" , ColumnWidth = 157 }, addedQueryColumns);

			 QueryColumn FilesInCreditControlQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInCreditControlQuery.Id,QueryCode = FilesInCreditControlQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Customs.DeclarationReferantData.VendorName" , ColumnWidth = 124 }, addedQueryColumns);

			 QueryColumn FilesInCreditControlQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInCreditControlQuery.Id,QueryCode = FilesInCreditControlQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "Customs.DeclarationReferantData.ArrivalDate" , ColumnWidth = 79 }, addedQueryColumns);

			 QueryColumn FilesInCreditControlQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInCreditControlQuery.Id,QueryCode = FilesInCreditControlQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "Customs.DeclarationReferantData.Weight" , ColumnWidth = 75 }, addedQueryColumns);

			 QueryColumn FilesInCreditControlQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInCreditControlQuery.Id,QueryCode = FilesInCreditControlQuery.UniqueCode, IndexOrder = 9, ObjectFieldCode = "Customs.DeclarationReferantData.DeclarationStatusTypeName" , ColumnWidth = 255 }, addedQueryColumns);

			 QueryColumn FilesInCreditControlQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInCreditControlQuery.Id,QueryCode = FilesInCreditControlQuery.UniqueCode, IndexOrder = 10, ObjectFieldCode = "Customs.DeclarationReferantData.PreClassification" , ColumnWidth = 58 }, addedQueryColumns);

			 QueryColumn FilesInCreditControlQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInCreditControlQuery.Id,QueryCode = FilesInCreditControlQuery.UniqueCode, IndexOrder = 11, ObjectFieldCode = "Customs.DeclarationReferantData.ClassificationStatus" , ColumnWidth = 55 }, addedQueryColumns);

			 QueryColumn FilesInCreditControlQueryColumn_12 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInCreditControlQuery.Id,QueryCode = FilesInCreditControlQuery.UniqueCode, IndexOrder = 12, ObjectFieldCode = "Customs.DeclarationReferantData.ControllerStatus" , ColumnWidth = 44 }, addedQueryColumns);

			 QueryColumn FilesInCreditControlQueryColumn_13 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInCreditControlQuery.Id,QueryCode = FilesInCreditControlQuery.UniqueCode, IndexOrder = 13, ObjectFieldCode = "Customs.DeclarationReferantData.CollectionOfMoneyStatus" , ColumnWidth = 50 }, addedQueryColumns);

			 QueryColumn FilesInCreditControlQueryColumn_14 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInCreditControlQuery.Id,QueryCode = FilesInCreditControlQuery.UniqueCode, IndexOrder = 14, ObjectFieldCode = "Customs.DeclarationReferantData.OrderMoney" , ColumnWidth = 50 }, addedQueryColumns);

			 QueryColumn FilesInCreditControlQueryColumn_15 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInCreditControlQuery.Id,QueryCode = FilesInCreditControlQuery.UniqueCode, IndexOrder = 15, ObjectFieldCode = "Customs.DeclarationReferantData.FollowUpDate" , ColumnWidth = 108 }, addedQueryColumns);

			 QueryColumn FilesInCreditControlQueryColumn_16 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInCreditControlQuery.Id,QueryCode = FilesInCreditControlQuery.UniqueCode, IndexOrder = 16, ObjectFieldCode = "Customs.DeclarationReferantData.AvailabilityDate" , ColumnWidth = 108 }, addedQueryColumns);

			 QueryColumn FilesInCreditControlQueryColumn_17 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInCreditControlQuery.Id,QueryCode = FilesInCreditControlQuery.UniqueCode, IndexOrder = 17, ObjectFieldCode = "Customs.DeclarationReferantData.ExceptionReasonsList" , ColumnWidth = 34 }, addedQueryColumns);

			 QueryColumn FilesInCreditControlQueryColumn_18 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInCreditControlQuery.Id,QueryCode = FilesInCreditControlQuery.UniqueCode, IndexOrder = 18, ObjectFieldCode = "Customs.DeclarationReferantData.WithPaper" , ColumnWidth = 69 }, addedQueryColumns);

			 QueryColumn FilesInCreditControlQueryColumn_19 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInCreditControlQuery.Id,QueryCode = FilesInCreditControlQuery.UniqueCode, IndexOrder = 19, ObjectFieldCode = "Customs.DeclarationReferantData.NewFile" , ColumnWidth = 69 }, addedQueryColumns);

			 QueryColumn FilesInCreditControlQueryColumn_20 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInCreditControlQuery.Id,QueryCode = FilesInCreditControlQuery.UniqueCode, IndexOrder = 20, ObjectFieldCode = "Customs.DeclarationReferantData.IsCustomerLogBoxActivated" , ColumnWidth = 68 }, addedQueryColumns);

			 QueryColumn FilesInCreditControlQueryColumn_21 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInCreditControlQuery.Id,QueryCode = FilesInCreditControlQuery.UniqueCode, IndexOrder = 21, ObjectFieldCode = "Customs.DeclarationReferantData.Actions" , ColumnWidth = 68 }, addedQueryColumns);

             AdvancedQueryFilter FilesInCreditControlQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.DeclarationReferantData.CollectionOfMoneyStatus", PredefinedValue = "P",PredefinedValue2 = null, QueryId = FilesInCreditControlQuery.Id,QueryCode = FilesInCreditControlQuery.UniqueCode, Tenant = 0,Operator = "Equal"}, addedQueryFilters);


             AdvancedQueryFilter FilesInCreditControlQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.DeclarationReferantData.IsCancelled", PredefinedValue = "true",PredefinedValue2 = null, QueryId = FilesInCreditControlQuery.Id,QueryCode = FilesInCreditControlQuery.UniqueCode, Tenant = 0,Operator = "NotEqual"}, addedQueryFilters);

  
	      

			  Query FilesAvailableFreeOfChargeQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = DeclarationReferantDataTextCode_7.Id, NameTextCodeCode = DeclarationReferantDataTextCode_7.Code, ObjectTableName = "Customs.DeclarationReferantData", Code = "FilesAvailableFreeOfCharge",  QueryGroupCode = "5277", IndexOrder = 7, Tenant = 0, ObjectTableId = DeclarationReferantDataObjectTable.Id, QuerySection = "Customs.DeclarationReferantData", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = DeclarationReferantDataFeature_7.Id,FeatureUniqeCode= DeclarationReferantDataFeature_7.FeatureUniqeCode, DefaultSortName = "SortedColumns", DefaultSortDirection = "Ascending", Perspective = null }, addedQueries);
	
			 QueryColumn FilesAvailableFreeOfChargeQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesAvailableFreeOfChargeQuery.Id,QueryCode = FilesAvailableFreeOfChargeQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.DeclarationReferantData.Favorite" , ColumnWidth = 80 }, addedQueryColumns);

			 QueryColumn FilesAvailableFreeOfChargeQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesAvailableFreeOfChargeQuery.Id,QueryCode = FilesAvailableFreeOfChargeQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.DeclarationReferantData.CustomFileNo" , ColumnWidth = 92 }, addedQueryColumns);

			 QueryColumn FilesAvailableFreeOfChargeQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesAvailableFreeOfChargeQuery.Id,QueryCode = FilesAvailableFreeOfChargeQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.DeclarationReferantData.CustomerName" , ColumnWidth = 146 }, addedQueryColumns);

			 QueryColumn FilesAvailableFreeOfChargeQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesAvailableFreeOfChargeQuery.Id,QueryCode = FilesAvailableFreeOfChargeQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.DeclarationReferantData.OrderNumber" , ColumnWidth = 87 }, addedQueryColumns);

			 QueryColumn FilesAvailableFreeOfChargeQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesAvailableFreeOfChargeQuery.Id,QueryCode = FilesAvailableFreeOfChargeQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.DeclarationReferantData.TransportModeId" , ColumnWidth = 58 }, addedQueryColumns);

			 QueryColumn FilesAvailableFreeOfChargeQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesAvailableFreeOfChargeQuery.Id,QueryCode = FilesAvailableFreeOfChargeQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Customs.DeclarationReferantData.DeclarationOfficeName" , ColumnWidth = 157 }, addedQueryColumns);

			 QueryColumn FilesAvailableFreeOfChargeQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesAvailableFreeOfChargeQuery.Id,QueryCode = FilesAvailableFreeOfChargeQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Customs.DeclarationReferantData.VendorName" , ColumnWidth = 124 }, addedQueryColumns);

			 QueryColumn FilesAvailableFreeOfChargeQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesAvailableFreeOfChargeQuery.Id,QueryCode = FilesAvailableFreeOfChargeQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "Customs.DeclarationReferantData.ArrivalDate" , ColumnWidth = 79 }, addedQueryColumns);

			 QueryColumn FilesAvailableFreeOfChargeQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesAvailableFreeOfChargeQuery.Id,QueryCode = FilesAvailableFreeOfChargeQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "Customs.DeclarationReferantData.Weight" , ColumnWidth = 75 }, addedQueryColumns);

			 QueryColumn FilesAvailableFreeOfChargeQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesAvailableFreeOfChargeQuery.Id,QueryCode = FilesAvailableFreeOfChargeQuery.UniqueCode, IndexOrder = 9, ObjectFieldCode = "Customs.DeclarationReferantData.DeclarationStatusTypeName" , ColumnWidth = 255 }, addedQueryColumns);

			 QueryColumn FilesAvailableFreeOfChargeQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesAvailableFreeOfChargeQuery.Id,QueryCode = FilesAvailableFreeOfChargeQuery.UniqueCode, IndexOrder = 10, ObjectFieldCode = "Customs.DeclarationReferantData.PreClassification" , ColumnWidth = 58 }, addedQueryColumns);

			 QueryColumn FilesAvailableFreeOfChargeQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesAvailableFreeOfChargeQuery.Id,QueryCode = FilesAvailableFreeOfChargeQuery.UniqueCode, IndexOrder = 11, ObjectFieldCode = "Customs.DeclarationReferantData.ClassificationStatus" , ColumnWidth = 55 }, addedQueryColumns);

			 QueryColumn FilesAvailableFreeOfChargeQueryColumn_12 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesAvailableFreeOfChargeQuery.Id,QueryCode = FilesAvailableFreeOfChargeQuery.UniqueCode, IndexOrder = 12, ObjectFieldCode = "Customs.DeclarationReferantData.ControllerStatus" , ColumnWidth = 44 }, addedQueryColumns);

			 QueryColumn FilesAvailableFreeOfChargeQueryColumn_13 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesAvailableFreeOfChargeQuery.Id,QueryCode = FilesAvailableFreeOfChargeQuery.UniqueCode, IndexOrder = 13, ObjectFieldCode = "Customs.DeclarationReferantData.CollectionOfMoneyStatus" , ColumnWidth = 50 }, addedQueryColumns);

			 QueryColumn FilesAvailableFreeOfChargeQueryColumn_14 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesAvailableFreeOfChargeQuery.Id,QueryCode = FilesAvailableFreeOfChargeQuery.UniqueCode, IndexOrder = 14, ObjectFieldCode = "Customs.DeclarationReferantData.OrderMoney" , ColumnWidth = 50 }, addedQueryColumns);

			 QueryColumn FilesAvailableFreeOfChargeQueryColumn_15 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesAvailableFreeOfChargeQuery.Id,QueryCode = FilesAvailableFreeOfChargeQuery.UniqueCode, IndexOrder = 15, ObjectFieldCode = "Customs.DeclarationReferantData.FollowUpDate" , ColumnWidth = 108 }, addedQueryColumns);

			 QueryColumn FilesAvailableFreeOfChargeQueryColumn_16 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesAvailableFreeOfChargeQuery.Id,QueryCode = FilesAvailableFreeOfChargeQuery.UniqueCode, IndexOrder = 16, ObjectFieldCode = "Customs.DeclarationReferantData.AvailabilityDate" , ColumnWidth = 108 }, addedQueryColumns);

			 QueryColumn FilesAvailableFreeOfChargeQueryColumn_17 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesAvailableFreeOfChargeQuery.Id,QueryCode = FilesAvailableFreeOfChargeQuery.UniqueCode, IndexOrder = 17, ObjectFieldCode = "Customs.DeclarationReferantData.ExceptionReasonsList" , ColumnWidth = 34 }, addedQueryColumns);

			 QueryColumn FilesAvailableFreeOfChargeQueryColumn_18 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesAvailableFreeOfChargeQuery.Id,QueryCode = FilesAvailableFreeOfChargeQuery.UniqueCode, IndexOrder = 18, ObjectFieldCode = "Customs.DeclarationReferantData.WithPaper" , ColumnWidth = 69 }, addedQueryColumns);

			 QueryColumn FilesAvailableFreeOfChargeQueryColumn_19 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesAvailableFreeOfChargeQuery.Id,QueryCode = FilesAvailableFreeOfChargeQuery.UniqueCode, IndexOrder = 19, ObjectFieldCode = "Customs.DeclarationReferantData.NewFile" , ColumnWidth = 69 }, addedQueryColumns);

			 QueryColumn FilesAvailableFreeOfChargeQueryColumn_20 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesAvailableFreeOfChargeQuery.Id,QueryCode = FilesAvailableFreeOfChargeQuery.UniqueCode, IndexOrder = 20, ObjectFieldCode = "Customs.DeclarationReferantData.IsCustomerLogBoxActivated" , ColumnWidth = 68 }, addedQueryColumns);

			 QueryColumn FilesAvailableFreeOfChargeQueryColumn_21 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesAvailableFreeOfChargeQuery.Id,QueryCode = FilesAvailableFreeOfChargeQuery.UniqueCode, IndexOrder = 21, ObjectFieldCode = "Customs.DeclarationReferantData.Actions" , ColumnWidth = 68 }, addedQueryColumns);

             AdvancedQueryFilter FilesAvailableFreeOfChargeQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.DeclarationReferantData.IsCancelled", PredefinedValue = "true",PredefinedValue2 = null, QueryId = FilesAvailableFreeOfChargeQuery.Id,QueryCode = FilesAvailableFreeOfChargeQuery.UniqueCode, Tenant = 0,Operator = "NotEqual"}, addedQueryFilters);


             AdvancedQueryFilter FilesAvailableFreeOfChargeQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.DeclarationReferantData.IsPaymentDateNull", PredefinedValue = "true",PredefinedValue2 = null, QueryId = FilesAvailableFreeOfChargeQuery.Id,QueryCode = FilesAvailableFreeOfChargeQuery.UniqueCode, Tenant = 0,Operator = "Equal"}, addedQueryFilters);


             AdvancedQueryFilter FilesAvailableFreeOfChargeQueryFilter_2 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.DeclarationReferantData.IsAvailabilityDateNull", PredefinedValue = "false",PredefinedValue2 = null, QueryId = FilesAvailableFreeOfChargeQuery.Id,QueryCode = FilesAvailableFreeOfChargeQuery.UniqueCode, Tenant = 0,Operator = "Equal"}, addedQueryFilters);

  
	      

			  Query FilesInAllInclusiveQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = DeclarationReferantDataTextCode_8.Id, NameTextCodeCode = DeclarationReferantDataTextCode_8.Code, ObjectTableName = "Customs.DeclarationReferantData", Code = "FilesInAllInclusive",  QueryGroupCode = "5277", IndexOrder = 8, Tenant = 0, ObjectTableId = DeclarationReferantDataObjectTable.Id, QuerySection = "Customs.DeclarationReferantData", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = DeclarationReferantDataFeature_8.Id,FeatureUniqeCode= DeclarationReferantDataFeature_8.FeatureUniqeCode, DefaultSortName = "SortedColumns", DefaultSortDirection = "Ascending", Perspective = null }, addedQueries);
	
			 QueryColumn FilesInAllInclusiveQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInAllInclusiveQuery.Id,QueryCode = FilesInAllInclusiveQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.DeclarationReferantData.CustomFileNo" , ColumnWidth = 92 }, addedQueryColumns);

			 QueryColumn FilesInAllInclusiveQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInAllInclusiveQuery.Id,QueryCode = FilesInAllInclusiveQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.DeclarationReferantData.DeclarationNumber" , ColumnWidth = 120 }, addedQueryColumns);

			 QueryColumn FilesInAllInclusiveQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInAllInclusiveQuery.Id,QueryCode = FilesInAllInclusiveQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.DeclarationReferantData.ImporterName" , ColumnWidth = 87 }, addedQueryColumns);

			 QueryColumn FilesInAllInclusiveQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInAllInclusiveQuery.Id,QueryCode = FilesInAllInclusiveQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.DeclarationReferantData.DeclarationOfficeName" , ColumnWidth = 157 }, addedQueryColumns);

			 QueryColumn FilesInAllInclusiveQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesInAllInclusiveQuery.Id,QueryCode = FilesInAllInclusiveQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.DeclarationReferantData.PaymentDate" , ColumnWidth = 124 }, addedQueryColumns);

             AdvancedQueryFilter FilesInAllInclusiveQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.DeclarationReferantData.IsCancelled", PredefinedValue = "true",PredefinedValue2 = null, QueryId = FilesInAllInclusiveQuery.Id,QueryCode = FilesInAllInclusiveQuery.UniqueCode, Tenant = 0,Operator = "NotEqual"}, addedQueryFilters);


             AdvancedQueryFilter FilesInAllInclusiveQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.DeclarationReferantData.RequestedCustomsDocId", PredefinedValue = "1",PredefinedValue2 = null, QueryId = FilesInAllInclusiveQuery.Id,QueryCode = FilesInAllInclusiveQuery.UniqueCode, Tenant = 0,Operator = "Equal"}, addedQueryFilters);


             AdvancedQueryFilter FilesInAllInclusiveQueryFilter_2 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.DeclarationReferantData.IsHatraDateNull", PredefinedValue = "false",PredefinedValue2 = null, QueryId = FilesInAllInclusiveQuery.Id,QueryCode = FilesInAllInclusiveQuery.UniqueCode, Tenant = 0,Operator = "Equal"}, addedQueryFilters);

  
	      

			  Query FilesRejectedByControllerQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = DeclarationReferantDataTextCode_9.Id, NameTextCodeCode = DeclarationReferantDataTextCode_9.Code, ObjectTableName = "Customs.DeclarationReferantData", Code = "FilesRejectedByController",  QueryGroupCode = "5277", IndexOrder = 9, Tenant = 0, ObjectTableId = DeclarationReferantDataObjectTable.Id, QuerySection = "Customs.DeclarationReferantData", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = DeclarationReferantDataFeature_9.Id,FeatureUniqeCode= DeclarationReferantDataFeature_9.FeatureUniqeCode, DefaultSortName = "SortedColumns", DefaultSortDirection = "Ascending", Perspective = null }, addedQueries);
	
			 QueryColumn FilesRejectedByControllerQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByControllerQuery.Id,QueryCode = FilesRejectedByControllerQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.DeclarationReferantData.Favorite" , ColumnWidth = 80 }, addedQueryColumns);

			 QueryColumn FilesRejectedByControllerQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByControllerQuery.Id,QueryCode = FilesRejectedByControllerQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.DeclarationReferantData.CustomFileNo" , ColumnWidth = 92 }, addedQueryColumns);

			 QueryColumn FilesRejectedByControllerQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByControllerQuery.Id,QueryCode = FilesRejectedByControllerQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.DeclarationReferantData.CustomerName" , ColumnWidth = 146 }, addedQueryColumns);

			 QueryColumn FilesRejectedByControllerQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByControllerQuery.Id,QueryCode = FilesRejectedByControllerQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.DeclarationReferantData.OrderNumber" , ColumnWidth = 87 }, addedQueryColumns);

			 QueryColumn FilesRejectedByControllerQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByControllerQuery.Id,QueryCode = FilesRejectedByControllerQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.DeclarationReferantData.TransportModeId" , ColumnWidth = 58 }, addedQueryColumns);

			 QueryColumn FilesRejectedByControllerQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByControllerQuery.Id,QueryCode = FilesRejectedByControllerQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Customs.DeclarationReferantData.DeclarationOfficeName" , ColumnWidth = 157 }, addedQueryColumns);

			 QueryColumn FilesRejectedByControllerQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByControllerQuery.Id,QueryCode = FilesRejectedByControllerQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Customs.DeclarationReferantData.VendorName" , ColumnWidth = 124 }, addedQueryColumns);

			 QueryColumn FilesRejectedByControllerQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByControllerQuery.Id,QueryCode = FilesRejectedByControllerQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "Customs.DeclarationReferantData.ArrivalDate" , ColumnWidth = 79 }, addedQueryColumns);

			 QueryColumn FilesRejectedByControllerQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByControllerQuery.Id,QueryCode = FilesRejectedByControllerQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "Customs.DeclarationReferantData.Weight" , ColumnWidth = 75 }, addedQueryColumns);

			 QueryColumn FilesRejectedByControllerQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByControllerQuery.Id,QueryCode = FilesRejectedByControllerQuery.UniqueCode, IndexOrder = 9, ObjectFieldCode = "Customs.DeclarationReferantData.DeclarationStatusTypeName" , ColumnWidth = 255 }, addedQueryColumns);

			 QueryColumn FilesRejectedByControllerQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByControllerQuery.Id,QueryCode = FilesRejectedByControllerQuery.UniqueCode, IndexOrder = 10, ObjectFieldCode = "Customs.DeclarationReferantData.PreClassification" , ColumnWidth = 58 }, addedQueryColumns);

			 QueryColumn FilesRejectedByControllerQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByControllerQuery.Id,QueryCode = FilesRejectedByControllerQuery.UniqueCode, IndexOrder = 11, ObjectFieldCode = "Customs.DeclarationReferantData.ClassificationStatus" , ColumnWidth = 55 }, addedQueryColumns);

			 QueryColumn FilesRejectedByControllerQueryColumn_12 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByControllerQuery.Id,QueryCode = FilesRejectedByControllerQuery.UniqueCode, IndexOrder = 12, ObjectFieldCode = "Customs.DeclarationReferantData.ControllerStatus" , ColumnWidth = 44 }, addedQueryColumns);

			 QueryColumn FilesRejectedByControllerQueryColumn_13 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByControllerQuery.Id,QueryCode = FilesRejectedByControllerQuery.UniqueCode, IndexOrder = 13, ObjectFieldCode = "Customs.DeclarationReferantData.CollectionOfMoneyStatus" , ColumnWidth = 50 }, addedQueryColumns);

			 QueryColumn FilesRejectedByControllerQueryColumn_14 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByControllerQuery.Id,QueryCode = FilesRejectedByControllerQuery.UniqueCode, IndexOrder = 14, ObjectFieldCode = "Customs.DeclarationReferantData.OrderMoney" , ColumnWidth = 50 }, addedQueryColumns);

			 QueryColumn FilesRejectedByControllerQueryColumn_15 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByControllerQuery.Id,QueryCode = FilesRejectedByControllerQuery.UniqueCode, IndexOrder = 15, ObjectFieldCode = "Customs.DeclarationReferantData.FollowUpDate" , ColumnWidth = 108 }, addedQueryColumns);

			 QueryColumn FilesRejectedByControllerQueryColumn_16 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByControllerQuery.Id,QueryCode = FilesRejectedByControllerQuery.UniqueCode, IndexOrder = 16, ObjectFieldCode = "Customs.DeclarationReferantData.AvailabilityDate" , ColumnWidth = 108 }, addedQueryColumns);

			 QueryColumn FilesRejectedByControllerQueryColumn_17 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByControllerQuery.Id,QueryCode = FilesRejectedByControllerQuery.UniqueCode, IndexOrder = 17, ObjectFieldCode = "Customs.DeclarationReferantData.ExceptionReasonsList" , ColumnWidth = 34 }, addedQueryColumns);

			 QueryColumn FilesRejectedByControllerQueryColumn_18 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByControllerQuery.Id,QueryCode = FilesRejectedByControllerQuery.UniqueCode, IndexOrder = 18, ObjectFieldCode = "Customs.DeclarationReferantData.WithPaper" , ColumnWidth = 69 }, addedQueryColumns);

			 QueryColumn FilesRejectedByControllerQueryColumn_19 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByControllerQuery.Id,QueryCode = FilesRejectedByControllerQuery.UniqueCode, IndexOrder = 19, ObjectFieldCode = "Customs.DeclarationReferantData.NewFile" , ColumnWidth = 69 }, addedQueryColumns);

			 QueryColumn FilesRejectedByControllerQueryColumn_20 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByControllerQuery.Id,QueryCode = FilesRejectedByControllerQuery.UniqueCode, IndexOrder = 20, ObjectFieldCode = "Customs.DeclarationReferantData.IsCustomerLogBoxActivated" , ColumnWidth = 68 }, addedQueryColumns);

			 QueryColumn FilesRejectedByControllerQueryColumn_21 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByControllerQuery.Id,QueryCode = FilesRejectedByControllerQuery.UniqueCode, IndexOrder = 21, ObjectFieldCode = "Customs.DeclarationReferantData.Actions" , ColumnWidth = 68 }, addedQueryColumns);

             AdvancedQueryFilter FilesRejectedByControllerQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.DeclarationReferantData.IsCancelled", PredefinedValue = "true",PredefinedValue2 = null, QueryId = FilesRejectedByControllerQuery.Id,QueryCode = FilesRejectedByControllerQuery.UniqueCode, Tenant = 0,Operator = "NotEqual"}, addedQueryFilters);


             AdvancedQueryFilter FilesRejectedByControllerQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.DeclarationReferantData.ControllerStatus", PredefinedValue = "X",PredefinedValue2 = null, QueryId = FilesRejectedByControllerQuery.Id,QueryCode = FilesRejectedByControllerQuery.UniqueCode, Tenant = 0,Operator = "Equal"}, addedQueryFilters);

  
	      

			  Query FilesRejectedByClassificationQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = DeclarationReferantDataTextCode_10.Id, NameTextCodeCode = DeclarationReferantDataTextCode_10.Code, ObjectTableName = "Customs.DeclarationReferantData", Code = "FilesRejectedByClassification",  QueryGroupCode = "5277", IndexOrder = 10, Tenant = 0, ObjectTableId = DeclarationReferantDataObjectTable.Id, QuerySection = "Customs.DeclarationReferantData", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = DeclarationReferantDataFeature_10.Id,FeatureUniqeCode= DeclarationReferantDataFeature_10.FeatureUniqeCode, DefaultSortName = "SortedColumns", DefaultSortDirection = "Ascending", Perspective = null }, addedQueries);
	
			 QueryColumn FilesRejectedByClassificationQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByClassificationQuery.Id,QueryCode = FilesRejectedByClassificationQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Customs.DeclarationReferantData.Favorite" , ColumnWidth = 80 }, addedQueryColumns);

			 QueryColumn FilesRejectedByClassificationQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByClassificationQuery.Id,QueryCode = FilesRejectedByClassificationQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Customs.DeclarationReferantData.CustomFileNo" , ColumnWidth = 92 }, addedQueryColumns);

			 QueryColumn FilesRejectedByClassificationQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByClassificationQuery.Id,QueryCode = FilesRejectedByClassificationQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Customs.DeclarationReferantData.CustomerName" , ColumnWidth = 146 }, addedQueryColumns);

			 QueryColumn FilesRejectedByClassificationQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByClassificationQuery.Id,QueryCode = FilesRejectedByClassificationQuery.UniqueCode, IndexOrder = 3, ObjectFieldCode = "Customs.DeclarationReferantData.OrderNumber" , ColumnWidth = 87 }, addedQueryColumns);

			 QueryColumn FilesRejectedByClassificationQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByClassificationQuery.Id,QueryCode = FilesRejectedByClassificationQuery.UniqueCode, IndexOrder = 4, ObjectFieldCode = "Customs.DeclarationReferantData.TransportModeId" , ColumnWidth = 58 }, addedQueryColumns);

			 QueryColumn FilesRejectedByClassificationQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByClassificationQuery.Id,QueryCode = FilesRejectedByClassificationQuery.UniqueCode, IndexOrder = 5, ObjectFieldCode = "Customs.DeclarationReferantData.DeclarationOfficeName" , ColumnWidth = 157 }, addedQueryColumns);

			 QueryColumn FilesRejectedByClassificationQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByClassificationQuery.Id,QueryCode = FilesRejectedByClassificationQuery.UniqueCode, IndexOrder = 6, ObjectFieldCode = "Customs.DeclarationReferantData.VendorName" , ColumnWidth = 124 }, addedQueryColumns);

			 QueryColumn FilesRejectedByClassificationQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByClassificationQuery.Id,QueryCode = FilesRejectedByClassificationQuery.UniqueCode, IndexOrder = 7, ObjectFieldCode = "Customs.DeclarationReferantData.ArrivalDate" , ColumnWidth = 79 }, addedQueryColumns);

			 QueryColumn FilesRejectedByClassificationQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByClassificationQuery.Id,QueryCode = FilesRejectedByClassificationQuery.UniqueCode, IndexOrder = 8, ObjectFieldCode = "Customs.DeclarationReferantData.Weight" , ColumnWidth = 75 }, addedQueryColumns);

			 QueryColumn FilesRejectedByClassificationQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByClassificationQuery.Id,QueryCode = FilesRejectedByClassificationQuery.UniqueCode, IndexOrder = 9, ObjectFieldCode = "Customs.DeclarationReferantData.DeclarationStatusTypeName" , ColumnWidth = 255 }, addedQueryColumns);

			 QueryColumn FilesRejectedByClassificationQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByClassificationQuery.Id,QueryCode = FilesRejectedByClassificationQuery.UniqueCode, IndexOrder = 10, ObjectFieldCode = "Customs.DeclarationReferantData.PreClassification" , ColumnWidth = 58 }, addedQueryColumns);

			 QueryColumn FilesRejectedByClassificationQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByClassificationQuery.Id,QueryCode = FilesRejectedByClassificationQuery.UniqueCode, IndexOrder = 11, ObjectFieldCode = "Customs.DeclarationReferantData.ClassificationStatus" , ColumnWidth = 55 }, addedQueryColumns);

			 QueryColumn FilesRejectedByClassificationQueryColumn_12 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByClassificationQuery.Id,QueryCode = FilesRejectedByClassificationQuery.UniqueCode, IndexOrder = 12, ObjectFieldCode = "Customs.DeclarationReferantData.ControllerStatus" , ColumnWidth = 44 }, addedQueryColumns);

			 QueryColumn FilesRejectedByClassificationQueryColumn_13 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByClassificationQuery.Id,QueryCode = FilesRejectedByClassificationQuery.UniqueCode, IndexOrder = 13, ObjectFieldCode = "Customs.DeclarationReferantData.CollectionOfMoneyStatus" , ColumnWidth = 50 }, addedQueryColumns);

			 QueryColumn FilesRejectedByClassificationQueryColumn_14 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByClassificationQuery.Id,QueryCode = FilesRejectedByClassificationQuery.UniqueCode, IndexOrder = 14, ObjectFieldCode = "Customs.DeclarationReferantData.OrderMoney" , ColumnWidth = 50 }, addedQueryColumns);

			 QueryColumn FilesRejectedByClassificationQueryColumn_15 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByClassificationQuery.Id,QueryCode = FilesRejectedByClassificationQuery.UniqueCode, IndexOrder = 15, ObjectFieldCode = "Customs.DeclarationReferantData.FollowUpDate" , ColumnWidth = 108 }, addedQueryColumns);

			 QueryColumn FilesRejectedByClassificationQueryColumn_16 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByClassificationQuery.Id,QueryCode = FilesRejectedByClassificationQuery.UniqueCode, IndexOrder = 16, ObjectFieldCode = "Customs.DeclarationReferantData.AvailabilityDate" , ColumnWidth = 108 }, addedQueryColumns);

			 QueryColumn FilesRejectedByClassificationQueryColumn_17 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByClassificationQuery.Id,QueryCode = FilesRejectedByClassificationQuery.UniqueCode, IndexOrder = 17, ObjectFieldCode = "Customs.DeclarationReferantData.ExceptionReasonsList" , ColumnWidth = 34 }, addedQueryColumns);

			 QueryColumn FilesRejectedByClassificationQueryColumn_18 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByClassificationQuery.Id,QueryCode = FilesRejectedByClassificationQuery.UniqueCode, IndexOrder = 18, ObjectFieldCode = "Customs.DeclarationReferantData.WithPaper" , ColumnWidth = 69 }, addedQueryColumns);

			 QueryColumn FilesRejectedByClassificationQueryColumn_19 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByClassificationQuery.Id,QueryCode = FilesRejectedByClassificationQuery.UniqueCode, IndexOrder = 19, ObjectFieldCode = "Customs.DeclarationReferantData.NewFile" , ColumnWidth = 69 }, addedQueryColumns);

			 QueryColumn FilesRejectedByClassificationQueryColumn_20 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByClassificationQuery.Id,QueryCode = FilesRejectedByClassificationQuery.UniqueCode, IndexOrder = 20, ObjectFieldCode = "Customs.DeclarationReferantData.IsCustomerLogBoxActivated" , ColumnWidth = 68 }, addedQueryColumns);

			 QueryColumn FilesRejectedByClassificationQueryColumn_21 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = FilesRejectedByClassificationQuery.Id,QueryCode = FilesRejectedByClassificationQuery.UniqueCode, IndexOrder = 21, ObjectFieldCode = "Customs.DeclarationReferantData.Actions" , ColumnWidth = 68 }, addedQueryColumns);

             AdvancedQueryFilter FilesRejectedByClassificationQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.DeclarationReferantData.IsCancelled", PredefinedValue = "true",PredefinedValue2 = null, QueryId = FilesRejectedByClassificationQuery.Id,QueryCode = FilesRejectedByClassificationQuery.UniqueCode, Tenant = 0,Operator = "NotEqual"}, addedQueryFilters);


             AdvancedQueryFilter FilesRejectedByClassificationQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldCode = "Customs.DeclarationReferantData.ClassificationStatus", PredefinedValue = "X",PredefinedValue2 = null, QueryId = FilesRejectedByClassificationQuery.Id,QueryCode = FilesRejectedByClassificationQuery.UniqueCode, Tenant = 0,Operator = "Equal"}, addedQueryFilters);

			SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
			SqlBulkInsert.BulkInsert("Features", addedFeatures);
			SqlBulkInsert.BulkInsert("Queries", addedQueries);
			SqlBulkInsert.BulkInsert("QueryColumns", addedQueryColumns);
			SqlBulkInsert.BulkInsert("AdvancedQueryFilters", addedQueryFilters);	 
  
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable DeclarationReferantDataObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.DeclarationReferantData" && d.Tenant == 0).FirstOrDefault();
		   //List<ObjectField> DeclarationReferantDataObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Customs.DeclarationReferantData").ToList();
		       
	      

	         Screen DeclarationReferantDataCustomsDeclarationReferantDataHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "DeclarationReferantData.HeaderScreen", Name = "Customs.DeclarationReferantDataHeaderScreen", ObjectTableId = DeclarationReferantDataObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      	
		    DeclarationReferantDataObjectTable.HeaderScreenId = DeclarationReferantDataCustomsDeclarationReferantDataHeaderScreenScreen0.Id;
		    DeclarationReferantDataObjectTable.HeaderScreenCode = DeclarationReferantDataCustomsDeclarationReferantDataHeaderScreenScreen0.Code;

	   		  

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {      
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable DeclarationReferantDataObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.DeclarationReferantData" && d.Tenant == 0).FirstOrDefault(); 

		   Feature DeclarationReferantDataFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, NameTextCodeCode = "DeclarationReferantData.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,DeclarationReferantDataObjectTable);
		   Feature DeclarationReferantDataFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, NameTextCodeCode = "DeclarationReferantData.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,DeclarationReferantDataObjectTable);
		   Feature DeclarationReferantDataFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, NameTextCodeCode = "DeclarationReferantData.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,DeclarationReferantDataObjectTable);
		   Feature DeclarationReferantDataFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = false, ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, NameTextCodeCode = "DeclarationReferantData.Features.PackageFeature", NameTextCodeDefaultText = "DeclarationReferantData Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,DeclarationReferantDataObjectTable);    
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable DeclarationReferantDataObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.DeclarationReferantData" && d.Tenant == 0).FirstOrDefault(); 
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
                ObjectTableId = DeclarationReferantDataObjectTable.Id,
				 
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
                ObjectTableId = DeclarationReferantDataObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable DeclarationReferantDataObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.DeclarationReferantData" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode DeclarationReferantDataTextCode_CustomsDeclarationReferantDataOFilesInProcess = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationReferantData.O.FilesInProcess", DefaultText = "Files In Process",LocalDefaultText = @"תיקים בטיפול", ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationReferantDataTextCode_CustomsDeclarationReferantDataOTrackingCases = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationReferantData.O.TrackingCases", DefaultText = "TrackingCases",LocalDefaultText = @"תיקים במעקב", ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationReferantDataTextCode_CustomsDeclarationReferantDataOFilesInOCR = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationReferantData.O.FilesInOCR", DefaultText = "FilesInOCR",LocalDefaultText = @"תיקים ב-OCR", ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationReferantDataTextCode_CustomsDeclarationReferantDataOFilesInSivug = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationReferantData.O.FilesInSivug", DefaultText = "FilesInSivug",LocalDefaultText = @"תיקים בסיווג", ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationReferantDataTextCode_CustomsDeclarationReferantDataOFilesInReview = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationReferantData.O.FilesInReview", DefaultText = "FilesInReview",LocalDefaultText = @"תיקים בביקורת", ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationReferantDataTextCode_CustomsDeclarationReferantDataOFilesInCreditControl = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationReferantData.O.FilesInCreditControl", DefaultText = "Files In Credit Control",LocalDefaultText = @"תיקים בבקרת אשראי", ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationReferantDataTextCode_CustomsDeclarationReferantDataOFilesAvailableFreeOfCharge = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationReferantData.O.FilesAvailableFreeOfCharge", DefaultText = "FilesAvailableFreeOfCharge",LocalDefaultText = @"תיקים זמינים ללא תשלום", ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationReferantDataTextCode_CustomsDeclarationReferantDataOAllCases = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationReferantData.O.AllCases", DefaultText = "AllCases",LocalDefaultText = @"כל התיקים", ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationReferantDataTextCode_CustomsDeclarationReferantDataOFilesInAllInclusive = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationReferantData.O.FilesInAllInclusive", DefaultText = "Files All Inclusive",LocalDefaultText = @"תיקים בכוללת", ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationReferantDataTextCode_CustomsDeclarationReferantDataOFilesRejectedByController = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationReferantData.O.FilesRejectedByController", DefaultText = "Files Rejected By Controller",LocalDefaultText = "תיקים שנדחו ע''י מבקר", ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DeclarationReferantDataTextCode_CustomsDeclarationReferantDataOFilesRejectedByClassification = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationReferantData.O.FilesRejectedByClassification", DefaultText = "Files Rejected By Classification",LocalDefaultText = "תיקים שנדחו ע''י מסווג", ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 