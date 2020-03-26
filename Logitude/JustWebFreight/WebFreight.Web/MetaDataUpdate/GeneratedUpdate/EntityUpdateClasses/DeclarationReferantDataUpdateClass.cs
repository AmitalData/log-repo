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
   public class DeclarationReferantDataUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "Customs.DeclarationReferantData",
			      				    IsNew =  true,
			      				    DBTableName =  "Customs.DeclarationReferantDatas",
			      				    OldDBTableName =  "Customs.DeclarationReferantDatas",
			      				    ObjectTableSingular =  "DeclarationReferantData",
			      				    ObjectTablePlural =  "DeclarationReferantDatas",
			      				    HasCustomFilter =  false,
			      				    HasCustomFields =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  false,
			      				    IsNewWizard =  false,
			      				    LookUp1 =  "DeclarationId",
			      				    LookUp2 =  "OrderNumber",
			      				    KeyPropertyPath =  "DeclarationId",
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
			      				    SortingByObjectField =  "DeclarationId",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  false,
			      				    IsComposition =  false,
			      				    EnableSecurity =  false,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    LocalDefaultText =  "רפרנט",
			      				    DefaultText =  "Declaration Referant Data",
			      				    Code =  "5277",
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
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DeclarationId",
					  						OldFieldName =  "DeclarationId",
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
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Tenant",
					  						OldFieldName =  "Tenant",
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
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "OrderNumber",
					  						OldFieldName =  "OrderNumber",
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
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "VendorId",
					  						OldFieldName =  "VendorId",
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
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ArrivalDate",
					  						OldFieldName =  "ArrivalDate",
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
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ArrivalDate",
					  						DefaultText =  "Arrival Date",
					  						FullLocalDefaultText =  "ATA",
					  						ListFieldLable =  "ArrivalDateListLable",
					  						ListLableDefaultText =  "Arrival Date",
					  						ListLocalDefaultText =  "ATA",
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
					 
					 						FieldName =  "EstimatedArrivalDate",
					  						OldFieldName =  "EstimatedArrivalDate",
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
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Weight",
					  						OldFieldName =  "Weight",
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
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ClassificationStatus",
					  						OldFieldName =  "ClassificationStatus",
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
					  						DisplayInList =  false,
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
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ControllerStatus",
					  						OldFieldName =  "ControllerStatus",
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
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CollectionOfMoneyStatus",
					  						OldFieldName =  "CollectionOfMoneyStatus",
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
					  						DisplayInList =  false,
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
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FollowUpDate",
					  						OldFieldName =  "FollowUpDate",
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
					  						DisplayInEntityVariables =  false,
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
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsExceptional",
					  						OldFieldName =  "IsExceptional",
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
					  						PMPropertyPath =  "IsExceptional",
					  						ListPropertyPath =  "IsExceptional",
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
					  						FullFieldLable =  "IsExceptional",
					  						DefaultText =  "Is Exceptional",
					  						FullLocalDefaultText =  "חריג",
					  						ListFieldLable =  "IsExceptionalListLable",
					  						ListLableDefaultText =  "Is Exceptional",
					  						ListLocalDefaultText =  "חריג",
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
					 
					 						FieldName =  "WithPaper",
					  						OldFieldName =  "IsManualProcess",
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
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsClosedForFollowUp",
					  						OldFieldName =  "IsClosedForFollowUp",
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
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsClassificationRemarks",
					  						OldFieldName =  "IsClassificationRemarks",
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
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsControllerRemarks",
					  						OldFieldName =  "IsControllerRemarks",
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
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PreClassification",
					  						OldFieldName =  "PreClassification",
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
					  						FullLocalDefaultText =  "טרום סיווג",
					  						ListFieldLable =  "PreClassificationListLable",
					  						ListLableDefaultText =  "Pre Classification",
					  						ListLocalDefaultText =  "טרום סיווג",
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
	        QueryGroup DeclarationReferantDataQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "5277", Name = " Query Group" }, queryGroupRepository);
						QueryGroup DeclarationReferantDataQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "fa77", Name = " Query Group" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable DeclarationReferantDataObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Customs.DeclarationReferantData" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> DeclarationReferantDataObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Customs.DeclarationReferantData").ToList();   

			   TextCode DeclarationReferantDataTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DeclarationReferantData.Q.DERE", DefaultText = @"Files In Process",LocalDefaultText = "תיקים בטיפול", ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature DeclarationReferantDataFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DeclarationReferantData.Q.DERE", ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, NameTextCodeCode = "DeclarationReferantData.Features.DERE", NameTextCodeDefaultText = "DERE", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query DEREQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = DeclarationReferantDataTextCode_0.Id, NameTextCodeCode = DeclarationReferantDataTextCode_0.Code, ObjectTableName = "Customs.DeclarationReferantData", Code = "DERE",  SpotlightDataTemplate = "ReferantSpotlightDataTemplate",  QueryGroupCode = "5277", IndexOrder = 0, Tenant = 0, ObjectTableId = DeclarationReferantDataObjectTable.Id, QuerySection = "Customs.DeclarationReferantData", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = DeclarationReferantDataFeature_0.Id,FeatureUniqeCode= DeclarationReferantDataFeature_0.FeatureUniqeCode, DefaultSortName = "ArrivalDate", DefaultSortDirection = "Desending", Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn DEREQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DEREQuery.Id,QueryCode = DEREQuery.UniqueCode, IndexOrder = 0, ObjectFieldId = DeclarationReferantDataObjectFields.Where(d => d.FieldName == "CustomFileNo" && d.ObjectTableId == DeclarationReferantDataObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = DeclarationReferantDataObjectFields.Where(d => d.FieldName == "CustomFileNo" && d.ObjectTableId == DeclarationReferantDataObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 94 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DEREQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DEREQuery.Id,QueryCode = DEREQuery.UniqueCode, IndexOrder = 1, ObjectFieldId = DeclarationReferantDataObjectFields.Where(d => d.FieldName == "CustomerName" && d.ObjectTableId == DeclarationReferantDataObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = DeclarationReferantDataObjectFields.Where(d => d.FieldName == "CustomerName" && d.ObjectTableId == DeclarationReferantDataObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 172 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DEREQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DEREQuery.Id,QueryCode = DEREQuery.UniqueCode, IndexOrder = 2, ObjectFieldId = DeclarationReferantDataObjectFields.Where(d => d.FieldName == "OrderNumber" && d.ObjectTableId == DeclarationReferantDataObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = DeclarationReferantDataObjectFields.Where(d => d.FieldName == "OrderNumber" && d.ObjectTableId == DeclarationReferantDataObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 89 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DEREQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DEREQuery.Id,QueryCode = DEREQuery.UniqueCode, IndexOrder = 3, ObjectFieldId = DeclarationReferantDataObjectFields.Where(d => d.FieldName == "TransportModeId" && d.ObjectTableId == DeclarationReferantDataObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = DeclarationReferantDataObjectFields.Where(d => d.FieldName == "TransportModeId" && d.ObjectTableId == DeclarationReferantDataObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 60 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DEREQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DEREQuery.Id,QueryCode = DEREQuery.UniqueCode, IndexOrder = 4, ObjectFieldId = DeclarationReferantDataObjectFields.Where(d => d.FieldName == "DeclarationOfficeName" && d.ObjectTableId == DeclarationReferantDataObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = DeclarationReferantDataObjectFields.Where(d => d.FieldName == "DeclarationOfficeName" && d.ObjectTableId == DeclarationReferantDataObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 159 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DEREQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DEREQuery.Id,QueryCode = DEREQuery.UniqueCode, IndexOrder = 5, ObjectFieldId = DeclarationReferantDataObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == DeclarationReferantDataObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = DeclarationReferantDataObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == DeclarationReferantDataObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 243 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DEREQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DEREQuery.Id,QueryCode = DEREQuery.UniqueCode, IndexOrder = 6, ObjectFieldId = DeclarationReferantDataObjectFields.Where(d => d.FieldName == "ArrivalDate" && d.ObjectTableId == DeclarationReferantDataObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = DeclarationReferantDataObjectFields.Where(d => d.FieldName == "ArrivalDate" && d.ObjectTableId == DeclarationReferantDataObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 81 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DEREQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DEREQuery.Id,QueryCode = DEREQuery.UniqueCode, IndexOrder = 7, ObjectFieldId = DeclarationReferantDataObjectFields.Where(d => d.FieldName == "Weight" && d.ObjectTableId == DeclarationReferantDataObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = DeclarationReferantDataObjectFields.Where(d => d.FieldName == "Weight" && d.ObjectTableId == DeclarationReferantDataObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 77 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DEREQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DEREQuery.Id,QueryCode = DEREQuery.UniqueCode, IndexOrder = 8, ObjectFieldId = DeclarationReferantDataObjectFields.Where(d => d.FieldName == "DeclarationStatusTypeName" && d.ObjectTableId == DeclarationReferantDataObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = DeclarationReferantDataObjectFields.Where(d => d.FieldName == "DeclarationStatusTypeName" && d.ObjectTableId == DeclarationReferantDataObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 257 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DEREQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DEREQuery.Id,QueryCode = DEREQuery.UniqueCode, IndexOrder = 9, ObjectFieldId = DeclarationReferantDataObjectFields.Where(d => d.FieldName == "PreClassification" && d.ObjectTableId == DeclarationReferantDataObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = DeclarationReferantDataObjectFields.Where(d => d.FieldName == "PreClassification" && d.ObjectTableId == DeclarationReferantDataObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 60 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DEREQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DEREQuery.Id,QueryCode = DEREQuery.UniqueCode, IndexOrder = 10, ObjectFieldId = DeclarationReferantDataObjectFields.Where(d => d.FieldName == "ClassificationStatus" && d.ObjectTableId == DeclarationReferantDataObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = DeclarationReferantDataObjectFields.Where(d => d.FieldName == "ClassificationStatus" && d.ObjectTableId == DeclarationReferantDataObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 96 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DEREQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DEREQuery.Id,QueryCode = DEREQuery.UniqueCode, IndexOrder = 11, ObjectFieldId = DeclarationReferantDataObjectFields.Where(d => d.FieldName == "ControllerStatus" && d.ObjectTableId == DeclarationReferantDataObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = DeclarationReferantDataObjectFields.Where(d => d.FieldName == "ControllerStatus" && d.ObjectTableId == DeclarationReferantDataObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 46 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DEREQueryColumn_12 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DEREQuery.Id,QueryCode = DEREQuery.UniqueCode, IndexOrder = 12, ObjectFieldId = DeclarationReferantDataObjectFields.Where(d => d.FieldName == "CollectionOfMoneyStatus" && d.ObjectTableId == DeclarationReferantDataObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = DeclarationReferantDataObjectFields.Where(d => d.FieldName == "CollectionOfMoneyStatus" && d.ObjectTableId == DeclarationReferantDataObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 96 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DEREQueryColumn_13 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DEREQuery.Id,QueryCode = DEREQuery.UniqueCode, IndexOrder = 13, ObjectFieldId = DeclarationReferantDataObjectFields.Where(d => d.FieldName == "FollowUpDate" && d.ObjectTableId == DeclarationReferantDataObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = DeclarationReferantDataObjectFields.Where(d => d.FieldName == "FollowUpDate" && d.ObjectTableId == DeclarationReferantDataObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 110 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DEREQueryColumn_14 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DEREQuery.Id,QueryCode = DEREQuery.UniqueCode, IndexOrder = 14, ObjectFieldId = DeclarationReferantDataObjectFields.Where(d => d.FieldName == "ExceptionReasonsList" && d.ObjectTableId == DeclarationReferantDataObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = DeclarationReferantDataObjectFields.Where(d => d.FieldName == "ExceptionReasonsList" && d.ObjectTableId == DeclarationReferantDataObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 71 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DEREQueryColumn_15 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DEREQuery.Id,QueryCode = DEREQuery.UniqueCode, IndexOrder = 15, ObjectFieldId = DeclarationReferantDataObjectFields.Where(d => d.FieldName == "WithPaper" && d.ObjectTableId == DeclarationReferantDataObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = DeclarationReferantDataObjectFields.Where(d => d.FieldName == "WithPaper" && d.ObjectTableId == DeclarationReferantDataObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 71 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn DEREQueryColumn_16 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = DEREQuery.Id,QueryCode = DEREQuery.UniqueCode, IndexOrder = 16, ObjectFieldId = DeclarationReferantDataObjectFields.Where(d => d.FieldName == "Actions" && d.ObjectTableId == DeclarationReferantDataObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = DeclarationReferantDataObjectFields.Where(d => d.FieldName == "Actions" && d.ObjectTableId == DeclarationReferantDataObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 70 }, queryColumnsRepository, tenantQueryColumns);
	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {    

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {      
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable DeclarationReferantDataObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.DeclarationReferantData" && d.Tenant == 0).FirstOrDefault(); 

		   Feature DeclarationReferantDataFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = true, ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, NameTextCodeCode = "DeclarationReferantData.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature DeclarationReferantDataFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = true, ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, NameTextCodeCode = "DeclarationReferantData.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature DeclarationReferantDataFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = true, ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, NameTextCodeCode = "DeclarationReferantData.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature DeclarationReferantDataFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = true, ObjectTableId = DeclarationReferantDataObjectTable.Id, Tenant = 0, NameTextCodeCode = "DeclarationReferantData.Features.PackageFeature", NameTextCodeDefaultText = "DeclarationReferantData Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);    
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable DeclarationReferantDataObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.DeclarationReferantData" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CREV",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Created",
                EnglishName =  "Created",
                EventTypeCategoryCode =  "OPE",
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
                ShortView =  false,
                IsManualEntry =  false,
                LocalName =  "Updated",
                EnglishName =  "Updated",
                EventTypeCategoryCode =  "OPE",
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
	    
}

    

   }
    
}
	 