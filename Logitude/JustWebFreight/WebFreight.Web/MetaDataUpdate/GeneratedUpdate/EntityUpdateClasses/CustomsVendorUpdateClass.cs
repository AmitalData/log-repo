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
   public class CustomsVendorUpdateClass
   {  		
		public const string HashString = "03f04817fb1952418edeac8f515070cd";
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "Customs.CustomsVendor",
			      				    IsNew =  false,
			      				    DBTableName =  "Customs.CustomsVendors",
			      				    OldDBTableName =  "Customs.CustomsVendors",
			      				    ObjectTableSingular =  "Customs Vendor",
			      				    ObjectTablePlural =  "Customs Vendors",
			      				    HasCustomFilter =  false,
			      				    HasCustomFields =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  false,
			      				    IsNewWizard =  true,
			      				    LookUp1 =  "VendorNumber",
			      				    LookUp2 =  "VendorName",
			      				    DependencyFilter1 =  "StatusCode",
			      				    KeyPropertyPath =  "Id",
			      				    AutoCompleteSearchWindow =  false,
			      				    IsClosed =  false,
			      				    CacheOnClient =  false,
			      				    EditableFromAutoCompleteWindow =  false,
			      				    HasCounter =  false,
			      				    EnableAddFromLOV =  true,
			      				    IsRestrictable =  false,
			      				    IsMain =  true,
			      				    IsAutoComplete =  false,
			      				    EnableEditFromLOV =  false,
			      				    SortingByObjectField =  "VendorName",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  false,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    NewWizardControlName =  "Logitude.Customs.Views.NewVendorControlCommand",
			      				    LocalDefaultText =  "מוכר",
			      				    DefaultText =  "Vendor",
			      				    Code =  "VNDR",
			      				    Name =  "Customs.CustomsVendor",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Customs",
			      				    ServerModuleName =  "Customs",
			      				    NewWizardComponentPath =  "./CustomsModules/CustomsVendor/Components/NewEntity/NewVendorComponent",
			      				    NoTS =  false,
			      				    HasMenuButtons =  false,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    HashString =  CustomsVendorUpdateClass.HashString,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SearchFields",
					  						OldFieldName =  "SearchFields",
					  						ObjectTableName =  "Customs.CustomsVendor",
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
					  						ValidForQuerySection1 =  "Customs.CustomsVendor",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SearchFields",
					  						DefaultText =  "Search...",
					  						FullLocalDefaultText =  "מספר ספק , שם ספק , DUNS , VAT",
					  						ListFieldLable =  "SearchFieldsListLable",
					  						ListLableDefaultText =  "SearchFields",
					  						ListLocalDefaultText =  "מספר ספק , שם ספק , DUNS , VAT",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "VendorNumber",
					  						OldFieldName =  "VendorNumber",
					  						ObjectTableName =  "Customs.CustomsVendor",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  9,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  true,
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
					  						DisplayInSearchWindowList =  true,
					  						PMPropertyPath =  "VendorNumber",
					  						ListPropertyPath =  "VendorNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomsVendors",
					  						ValidForQuerySection2 =  "ActiveCustomsVendors",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "VendorNumber",
					  						DefaultText =  "Number",
					  						FullLocalDefaultText =  "קוד ספק",
					  						ListFieldLable =  "VendorNumberListLable",
					  						ListLableDefaultText =  "Number",
					  						ListLocalDefaultText =  "קוד ספק",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "VendorTypeCode",
					  						OldFieldName =  "VendorTypeCode",
					  						ObjectTableName =  "Customs.CustomsVendor",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.VendorType",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  true,
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
					  						PMPropertyPath =  "VendorTypeCode",
					  						ListPropertyPath =  "VendorTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomsVendors",
					  						ValidForQuerySection2 =  "ActiveCustomsVendors",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "VendorTypeCode",
					  						DefaultText =  "Vendor Type",
					  						FullLocalDefaultText =  "סוג ספק",
					  						ListFieldLable =  "VendorTypeCodeListLable",
					  						ListLableDefaultText =  "Vendor Type ",
					  						ListLocalDefaultText =  "סוג ספק",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "VendorName",
					  						OldFieldName =  "VendorName",
					  						ObjectTableName =  "Customs.CustomsVendor",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  55,
					  						IsRequired =  true,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  true,
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
					  						DisplayInSearchWindowList =  true,
					  						PMPropertyPath =  "VendorName",
					  						ListPropertyPath =  "VendorName",
					  						DisplayInLookUpIndex =  1,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  1,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomsVendors",
					  						ValidForQuerySection2 =  "ActiveCustomsVendors",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "VendorName",
					  						DefaultText =  "Name",
					  						FullLocalDefaultText =  "שם ספק",
					  						ListFieldLable =  "VendorNameListLable",
					  						ListLableDefaultText =  "Name",
					  						ListLocalDefaultText =  "שם ספק",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CountryCode",
					  						OldFieldName =  "CountryCode",
					  						ObjectTableName =  "Customs.CustomsVendor",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.CustomsCountry",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  true,
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
					  						PMPropertyPath =  "CountryCode",
					  						ListPropertyPath =  "CountryCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomsVendors",
					  						ValidForQuerySection2 =  "ActiveCustomsVendors",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CountryCode",
					  						DefaultText =  "Country",
					  						FullLocalDefaultText =  "מדינה",
					  						ListFieldLable =  "CountryCodeListLable",
					  						ListLableDefaultText =  "Country",
					  						ListLocalDefaultText =  "מדינה",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SubCountryCode",
					  						OldFieldName =  "SubCountryCode",
					  						ObjectTableName =  "Customs.CustomsVendor",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.SubCountry",
					  						MinLength =  0,
					  						MaxLength =  6,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  6,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "SubCountryCode",
					  						ListPropertyPath =  "SubCountryCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomsVendors",
					  						ValidForQuerySection2 =  "ActiveCustomsVendors",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						ControlField1 =  "CountryCode",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SubCountryCode",
					  						DefaultText =  "Sub Country",
					  						FullLocalDefaultText =  "תת מדינה",
					  						ListFieldLable =  "SubCountryCodeListLable",
					  						ListLableDefaultText =  "Sub Country ",
					  						ListLocalDefaultText =  "תת מדינה",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SubCountryName",
					  						OldFieldName =  "SubCountryName",
					  						ObjectTableName =  "Customs.CustomsVendor",
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
					  						PMPropertyPath =  "SubCountryName",
					  						ListPropertyPath =  "SubCountryName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomsVendors",
					  						ValidForQuerySection2 =  "ActiveCustomsVendors",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SubCountryName",
					  						DefaultText =  "Sub Country",
					  						FullLocalDefaultText =  "מדינה שם תת",
					  						ListFieldLable =  "SubCountryNameListLable",
					  						ListLableDefaultText =  "Sub Country Name",
					  						ListLocalDefaultText =  "מדינה תת",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CityName",
					  						OldFieldName =  "CityName",
					  						ObjectTableName =  "Customs.CustomsVendor",
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
					  						PMPropertyPath =  "CityName",
					  						ListPropertyPath =  "CityName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomsVendors",
					  						ValidForQuerySection2 =  "ActiveCustomsVendors",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CityName",
					  						DefaultText =  "City",
					  						FullLocalDefaultText =  "עיר",
					  						ListFieldLable =  "CityNameListLable",
					  						ListLableDefaultText =  "City",
					  						ListLocalDefaultText =  "עיר",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "MainAddressLine",
					  						OldFieldName =  "MainAddressLine",
					  						ObjectTableName =  "Customs.CustomsVendor",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  70,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  70,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "MainAddressLine",
					  						ListPropertyPath =  "MainAddressLine",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomsVendors",
					  						ValidForQuerySection2 =  "ActiveCustomsVendors",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "MainAddressLine",
					  						DefaultText =  "Main Address",
					  						FullLocalDefaultText =  "כתובת ראשית",
					  						ListFieldLable =  "MainAddressLineListLable",
					  						ListLableDefaultText =  "Main Address",
					  						ListLocalDefaultText =  "כתובת ראשית",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PostalCode",
					  						OldFieldName =  "PostalCode",
					  						ObjectTableName =  "Customs.CustomsVendor",
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
					  						PMPropertyPath =  "PostalCode",
					  						ListPropertyPath =  "PostalCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomsVendors",
					  						ValidForQuerySection2 =  "ActiveCustomsVendors",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "PostalCode",
					  						DefaultText =  "Postal Code",
					  						FullLocalDefaultText =  "מיקוד",
					  						ListFieldLable =  "PostalCodeListLable",
					  						ListLableDefaultText =  "Postal Code",
					  						ListLocalDefaultText =  "מיקוד",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DunsNumber",
					  						OldFieldName =  "DunsNumber",
					  						ObjectTableName =  "Customs.CustomsVendor",
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
					  						PMPropertyPath =  "DunsNumber",
					  						ListPropertyPath =  "DunsNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomsVendors",
					  						ValidForQuerySection2 =  "ActiveCustomsVendors",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "DunsNumber",
					  						DefaultText =  "Duns",
					  						ListFieldLable =  "DunsNumberListLable",
					  						ListLableDefaultText =  "Duns",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "VATNumber",
					  						OldFieldName =  "VATNumber",
					  						ObjectTableName =  "Customs.CustomsVendor",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  25,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						DisplayOnLookUpLocal =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  25,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "VATNumber",
					  						ListPropertyPath =  "VATNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomsVendors",
					  						ValidForQuerySection2 =  "ActiveCustomsVendors",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "VATNumber",
					  						DefaultText =  "VAT Number",
					  						FullLocalDefaultText =  "מספר עוסק מורשה",
					  						ListFieldLable =  "VATNumberListLable",
					  						ListLableDefaultText =  "VAT Number",
					  						ListLocalDefaultText =  "מספר עוסק מורשה",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "StatusCode",
					  						OldFieldName =  "StatusCode",
					  						ObjectTableName =  "Customs.CustomsVendor",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.VendorStatus",
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
					  						ValidForQuerySection1 =  "CustomsVendors",
					  						ValidForQuerySection2 =  "ActiveCustomsVendors",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "StatusCode",
					  						DefaultText =  "Status ",
					  						FullLocalDefaultText =  "סטטוס ספק",
					  						ListFieldLable =  "StatusCodeListLable",
					  						ListLableDefaultText =  "Status ",
					  						ListLocalDefaultText =  "סטטוס ספק",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "TransactionTypeID",
					  						OldFieldName =  "TransactionTypeID",
					  						ObjectTableName =  "Customs.CustomsVendor",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Customs.VendorTransactionType",
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
					  						PMPropertyPath =  "TransactionTypeID",
					  						ListPropertyPath =  "TransactionTypeID",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomsVendors",
					  						ValidForQuerySection2 =  "ActiveCustomsVendors",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "TransactionTypeID",
					  						DefaultText =  "Transaction Type ",
					  						FullLocalDefaultText =  "סוג תנועה על ספק",
					  						ListFieldLable =  "TransactionTypeIDListLable",
					  						ListLableDefaultText =  "Transaction Type ",
					  						ListLocalDefaultText =  "סוג תנועה על ספק",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsPalestinian",
					  						OldFieldName =  "IsPalestinian",
					  						ObjectTableName =  "Customs.CustomsVendor",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  1,
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsPalestinian",
					  						ListPropertyPath =  "IsPalestinian",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  true,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsPalestinian",
					  						DefaultText =  "Is Palestinian",
					  						FullLocalDefaultText =  "רש\"פ",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "VendorTypeName",
					  						OldFieldName =  "VendorTypeName",
					  						ObjectTableName =  "Customs.CustomsVendor",
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
					  						PMPropertyPath =  "VendorTypeName",
					  						ListPropertyPath =  "VendorTypeName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomsVendors",
					  						ValidForQuerySection2 =  "ActiveCustomsVendors",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "VendorTypeName",
					  						DefaultText =  "Vendor Type Name",
					  						FullLocalDefaultText =  "סוג שם ספק",
					  						ListFieldLable =  "VendorTypeNameListLable",
					  						ListLableDefaultText =  "Vendor Type Name",
					  						ListLocalDefaultText =  "סוג שם ספק",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CountryName",
					  						OldFieldName =  "CountryName",
					  						ObjectTableName =  "Customs.CustomsVendor",
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
					  						DisplayInSearchWindowList =  true,
					  						PMPropertyPath =  "CountryName",
					  						ListPropertyPath =  "CountryName",
					  						DisplayInLookUpIndex =  2,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  2,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomsVendors",
					  						ValidForQuerySection2 =  "ActiveCustomsVendors",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CountryName",
					  						DefaultText =  "Country Name",
					  						FullLocalDefaultText =  "שם מדינה",
					  						ListFieldLable =  "CountryNameListLable",
					  						ListLableDefaultText =  "Country Name",
					  						ListLocalDefaultText =  "שם מדינה",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "VendorCommunications",
					  						OldFieldName =  "VendorCommunications",
					  						ObjectTableName =  "Customs.CustomsVendor",
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
					  						PMPropertyPath =  "VendorCommunications",
					  						ListPropertyPath =  "VendorCommunications",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  true,
					  						MultiTableName =  "Customs.VendorCommunication",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomsVendors",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "VendorCommunications",
					  						DefaultText =  "VendorCommunications",
					  						FullLocalDefaultText =  "VendorCommunications",
					  						ListFieldLable =  "VendorCommunicationsListLable",
					  						ListLableDefaultText =  "VendorCommunications",
					  						ListLocalDefaultText =  "VendorCommunications",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InActive",
					  						OldFieldName =  "InActive",
					  						ObjectTableName =  "Customs.CustomsVendor",
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
					  						ValidForQuerySection1 =  "CustomsVendors",
					  						ValidForQuerySection2 =  "ActiveCustomsVendors",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "InActive",
					  						DefaultText =  "InActive",
					  						FullLocalDefaultText =  "לא פעיל",
					  						ListFieldLable =  "InActiveListLable",
					  						ListLableDefaultText =  "InActive",
					  						ListLocalDefaultText =  "לא פעיל",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ExternalId",
					  						OldFieldName =  "ExternalId",
					  						ObjectTableName =  "Customs.CustomsVendor",
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
					  						PMPropertyPath =  "ExternalId",
					  						ListPropertyPath =  "ExternalId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomsVendors",
					  						ValidForQuerySection2 =  "ActiveCustomsVendors",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ExternalId",
					  						DefaultText =  "External ID",
					  						FullLocalDefaultText =  "קוד ספק ביוניפרייט",
					  						ListFieldLable =  "ExternalIdListLable",
					  						ListLableDefaultText =  "External ID",
					  						ListLocalDefaultText =  "קוד ספק ביוניפרייט",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "StatusName",
					  						OldFieldName =  "StatusName",
					  						ObjectTableName =  "Customs.CustomsVendor",
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
					  						ValidForQuerySection1 =  "CustomsVendors",
					  						ValidForQuerySection2 =  "ActiveCustomsVendors",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "StatusName",
					  						DefaultText =  "Status Name",
					  						FullLocalDefaultText =  "סטטוס ספק",
					  						ListFieldLable =  "StatusNameListLable",
					  						ListLableDefaultText =  "Status Name",
					  						ListLocalDefaultText =  "סטטוס ספק",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ConcurrencyGUID",
					  						OldFieldName =  "ConcurrencyGUID",
					  						ObjectTableName =  "Customs.CustomsVendor",
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
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "NewConcurrencyGUID",
					  						OldFieldName =  "NewConcurrencyGUID",
					  						ObjectTableName =  "Customs.CustomsVendor",
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
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	    {  
	        FeatureRepository featureRepository = new FeatureRepository(0); 
            //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
            IWebFreightContext objectContext = WebFreightContext.GetContext(0);  
	        QueryGroup CustomsVendorQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "VNDR", Name = "Customs.CustomsVendor" }, queryGroupRepository);
						QueryGroup CustomsVendorQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "6b33", Name = " Query Group" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();
	        ObjectTable CustomsVendorObjectTable = objectTables.ContainsKey("Customs.CustomsVendor") ? objectTables["Customs.CustomsVendor"] : null;
            if (CustomsVendorObjectTable == null)
            {
                CustomsVendorObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Customs.CustomsVendor" && d.Tenant == 0).FirstOrDefault();
            }

	         
	        List<ObjectField> CustomsVendorObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Customs.CustomsVendor").ToList();   

			   TextCode CustomsVendorTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Vendor.Q.AllVendorsQuery", DefaultText = @"All Vendors",LocalDefaultText = "ספקים", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature CustomsVendorFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ALLVENDORS", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.CustomsVendor.Features.AllVendors", NameTextCodeDefaultText = "All Vendors", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode CustomsVendorTextCode_1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Vendor.Q.VendorQuery", DefaultText = @"Active Vendors",LocalDefaultText = "ספקים פעילים", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature CustomsVendorFeature_1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACTIVEVENDORS", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.CustomsVendor.Features.ActiveVendors", NameTextCodeDefaultText = "Active Vendors", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query AllVendorsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = CustomsVendorTextCode_0.Id, NameTextCodeCode = CustomsVendorTextCode_0.Code, ObjectTableName = "Customs.CustomsVendor", Code = "AllVendors",  EditWizardName = "Logitude.Customs.Views.AddEditVendorControl",
			   QueryGroupCode = "VNDR", IndexOrder = 0, Tenant = 0, ObjectTableId = CustomsVendorObjectTable.Id, QuerySection = "Customs.CustomsVendor", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = CustomsVendorFeature_0.Id,FeatureUniqeCode= CustomsVendorFeature_0.FeatureUniqeCode, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn AllVendorsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllVendorsQuery.Id,QueryCode = AllVendorsQuery.UniqueCode, IndexOrder = 0, ObjectFieldId = CustomsVendorObjectFields.Where(d => d.FieldName == "VendorNumber" && d.ObjectTableId == CustomsVendorObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomsVendorObjectFields.Where(d => d.FieldName == "VendorNumber" && d.ObjectTableId == CustomsVendorObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllVendorsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllVendorsQuery.Id,QueryCode = AllVendorsQuery.UniqueCode, IndexOrder = 1, ObjectFieldId = CustomsVendorObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == CustomsVendorObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomsVendorObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == CustomsVendorObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllVendorsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllVendorsQuery.Id,QueryCode = AllVendorsQuery.UniqueCode, IndexOrder = 2, ObjectFieldId = CustomsVendorObjectFields.Where(d => d.FieldName == "CountryCode" && d.ObjectTableId == CustomsVendorObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomsVendorObjectFields.Where(d => d.FieldName == "CountryCode" && d.ObjectTableId == CustomsVendorObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllVendorsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllVendorsQuery.Id,QueryCode = AllVendorsQuery.UniqueCode, IndexOrder = 3, ObjectFieldId = CustomsVendorObjectFields.Where(d => d.FieldName == "CityName" && d.ObjectTableId == CustomsVendorObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomsVendorObjectFields.Where(d => d.FieldName == "CityName" && d.ObjectTableId == CustomsVendorObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllVendorsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllVendorsQuery.Id,QueryCode = AllVendorsQuery.UniqueCode, IndexOrder = 4, ObjectFieldId = CustomsVendorObjectFields.Where(d => d.FieldName == "MainAddressLine" && d.ObjectTableId == CustomsVendorObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomsVendorObjectFields.Where(d => d.FieldName == "MainAddressLine" && d.ObjectTableId == CustomsVendorObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllVendorsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllVendorsQuery.Id,QueryCode = AllVendorsQuery.UniqueCode, IndexOrder = 5, ObjectFieldId = CustomsVendorObjectFields.Where(d => d.FieldName == "VATNumber" && d.ObjectTableId == CustomsVendorObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomsVendorObjectFields.Where(d => d.FieldName == "VATNumber" && d.ObjectTableId == CustomsVendorObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllVendorsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllVendorsQuery.Id,QueryCode = AllVendorsQuery.UniqueCode, IndexOrder = 6, ObjectFieldId = CustomsVendorObjectFields.Where(d => d.FieldName == "DunsNumber" && d.ObjectTableId == CustomsVendorObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomsVendorObjectFields.Where(d => d.FieldName == "DunsNumber" && d.ObjectTableId == CustomsVendorObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
  
	      

			  Query ActiveVendorsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = CustomsVendorTextCode_1.Id, NameTextCodeCode = CustomsVendorTextCode_1.Code, ObjectTableName = "Customs.CustomsVendor", Code = "ActiveVendors",  EditWizardName = "Logitude.Customs.Views.AddEditVendorControl",
			   QueryGroupCode = "VNDR", IndexOrder = 1, Tenant = 0, ObjectTableId = CustomsVendorObjectTable.Id, QuerySection = "Customs.CustomsVendor", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = CustomsVendorFeature_1.Id,FeatureUniqeCode= CustomsVendorFeature_1.FeatureUniqeCode, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn ActiveVendorsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ActiveVendorsQuery.Id,QueryCode = ActiveVendorsQuery.UniqueCode, IndexOrder = 0, ObjectFieldId = CustomsVendorObjectFields.Where(d => d.FieldName == "VendorNumber" && d.ObjectTableId == CustomsVendorObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomsVendorObjectFields.Where(d => d.FieldName == "VendorNumber" && d.ObjectTableId == CustomsVendorObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ActiveVendorsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ActiveVendorsQuery.Id,QueryCode = ActiveVendorsQuery.UniqueCode, IndexOrder = 1, ObjectFieldId = CustomsVendorObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == CustomsVendorObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomsVendorObjectFields.Where(d => d.FieldName == "VendorName" && d.ObjectTableId == CustomsVendorObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ActiveVendorsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ActiveVendorsQuery.Id,QueryCode = ActiveVendorsQuery.UniqueCode, IndexOrder = 2, ObjectFieldId = CustomsVendorObjectFields.Where(d => d.FieldName == "CountryCode" && d.ObjectTableId == CustomsVendorObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomsVendorObjectFields.Where(d => d.FieldName == "CountryCode" && d.ObjectTableId == CustomsVendorObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ActiveVendorsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ActiveVendorsQuery.Id,QueryCode = ActiveVendorsQuery.UniqueCode, IndexOrder = 3, ObjectFieldId = CustomsVendorObjectFields.Where(d => d.FieldName == "CityName" && d.ObjectTableId == CustomsVendorObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomsVendorObjectFields.Where(d => d.FieldName == "CityName" && d.ObjectTableId == CustomsVendorObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ActiveVendorsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ActiveVendorsQuery.Id,QueryCode = ActiveVendorsQuery.UniqueCode, IndexOrder = 4, ObjectFieldId = CustomsVendorObjectFields.Where(d => d.FieldName == "MainAddressLine" && d.ObjectTableId == CustomsVendorObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomsVendorObjectFields.Where(d => d.FieldName == "MainAddressLine" && d.ObjectTableId == CustomsVendorObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ActiveVendorsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ActiveVendorsQuery.Id,QueryCode = ActiveVendorsQuery.UniqueCode, IndexOrder = 5, ObjectFieldId = CustomsVendorObjectFields.Where(d => d.FieldName == "VATNumber" && d.ObjectTableId == CustomsVendorObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomsVendorObjectFields.Where(d => d.FieldName == "VATNumber" && d.ObjectTableId == CustomsVendorObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ActiveVendorsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ActiveVendorsQuery.Id,QueryCode = ActiveVendorsQuery.UniqueCode, IndexOrder = 6, ObjectFieldId = CustomsVendorObjectFields.Where(d => d.FieldName == "DunsNumber" && d.ObjectTableId == CustomsVendorObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomsVendorObjectFields.Where(d => d.FieldName == "DunsNumber" && d.ObjectTableId == CustomsVendorObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter ActiveVendorsQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = CustomsVendorObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTableId == CustomsVendorObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomsVendorObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTableId == CustomsVendorObjectTable.Id).FirstOrDefault().FieldCode, PredefinedValue = "false",PredefinedValue2 = null, QueryId = ActiveVendorsQuery.Id,QueryCode = ActiveVendorsQuery.UniqueCode, Tenant = 0}, advancedQueryFiltersRepository, tenantAdvancedFilters);

	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable CustomsVendorObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.CustomsVendor" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> CustomsVendorObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Customs.CustomsVendor").ToList();
		       
	      

	         Screen CustomsVendorHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Customs.Vendor.HeaderScreen", Name = "Header Screen", ObjectTableId = CustomsVendorObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField CustomsVendorCustomsVendorHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = CustomsVendorObjectFields.Where(d => d.FieldName == "VendorNumber").FirstOrDefault().Id, ScreenId = CustomsVendorHeaderScreenScreen0.Id,ScreenCode = CustomsVendorHeaderScreenScreen0.Code, ObjectFieldCode = CustomsVendorObjectFields.Where(d => d.FieldName == "VendorNumber").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomsVendorCustomsVendorHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = CustomsVendorObjectFields.Where(d => d.FieldName == "VendorName").FirstOrDefault().Id, ScreenId = CustomsVendorHeaderScreenScreen0.Id,ScreenCode = CustomsVendorHeaderScreenScreen0.Code, ObjectFieldCode = CustomsVendorObjectFields.Where(d => d.FieldName == "VendorName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    CustomsVendorObjectTable.HeaderScreenId = CustomsVendorHeaderScreenScreen0.Id;
		    CustomsVendorObjectTable.HeaderScreenCode = CustomsVendorHeaderScreenScreen0.Code;

	   		  
	      

	         Screen CustomsVendorGeneralTabScreenScreen1 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Customs.Vendor.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = CustomsVendorObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 5, IsReadOnly = false }, screensRepository, tenantScreens);
      
            ScreenField CustomsVendorCustomsVendorGeneralTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = CustomsVendorObjectFields.Where(d => d.FieldName == "VendorTypeCode").FirstOrDefault().Id, ScreenId = CustomsVendorGeneralTabScreenScreen1.Id,ScreenCode = CustomsVendorGeneralTabScreenScreen1.Code, ObjectFieldCode = CustomsVendorObjectFields.Where(d => d.FieldName == "VendorTypeCode").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomsVendorCustomsVendorGeneralTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = CustomsVendorObjectFields.Where(d => d.FieldName == "CountryCode").FirstOrDefault().Id, ScreenId = CustomsVendorGeneralTabScreenScreen1.Id,ScreenCode = CustomsVendorGeneralTabScreenScreen1.Code, ObjectFieldCode = CustomsVendorObjectFields.Where(d => d.FieldName == "CountryCode").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomsVendorCustomsVendorGeneralTabScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ObjectFieldId = CustomsVendorObjectFields.Where(d => d.FieldName == "CityName").FirstOrDefault().Id, ScreenId = CustomsVendorGeneralTabScreenScreen1.Id,ScreenCode = CustomsVendorGeneralTabScreenScreen1.Code, ObjectFieldCode = CustomsVendorObjectFields.Where(d => d.FieldName == "CityName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomsVendorCustomsVendorGeneralTabScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 3, ObjectFieldId = CustomsVendorObjectFields.Where(d => d.FieldName == "PostalCode").FirstOrDefault().Id, ScreenId = CustomsVendorGeneralTabScreenScreen1.Id,ScreenCode = CustomsVendorGeneralTabScreenScreen1.Code, ObjectFieldCode = CustomsVendorObjectFields.Where(d => d.FieldName == "PostalCode").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomsVendorCustomsVendorGeneralTabScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 4, ObjectFieldId = CustomsVendorObjectFields.Where(d => d.FieldName == "VATNumber").FirstOrDefault().Id, ScreenId = CustomsVendorGeneralTabScreenScreen1.Id,ScreenCode = CustomsVendorGeneralTabScreenScreen1.Code, ObjectFieldCode = CustomsVendorObjectFields.Where(d => d.FieldName == "VATNumber").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomsVendorCustomsVendorGeneralTabScreenScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = CustomsVendorObjectFields.Where(d => d.FieldName == "VendorName").FirstOrDefault().Id, ScreenId = CustomsVendorGeneralTabScreenScreen1.Id,ScreenCode = CustomsVendorGeneralTabScreenScreen1.Code, ObjectFieldCode = CustomsVendorObjectFields.Where(d => d.FieldName == "VendorName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomsVendorCustomsVendorGeneralTabScreenScreenField6 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ObjectFieldId = CustomsVendorObjectFields.Where(d => d.FieldName == "SubCountryCode").FirstOrDefault().Id, ScreenId = CustomsVendorGeneralTabScreenScreen1.Id,ScreenCode = CustomsVendorGeneralTabScreenScreen1.Code, ObjectFieldCode = CustomsVendorObjectFields.Where(d => d.FieldName == "SubCountryCode").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomsVendorCustomsVendorGeneralTabScreenScreenField7 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 2, ObjectFieldId = CustomsVendorObjectFields.Where(d => d.FieldName == "MainAddressLine").FirstOrDefault().Id, ScreenId = CustomsVendorGeneralTabScreenScreen1.Id,ScreenCode = CustomsVendorGeneralTabScreenScreen1.Code, ObjectFieldCode = CustomsVendorObjectFields.Where(d => d.FieldName == "MainAddressLine").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomsVendorCustomsVendorGeneralTabScreenScreenField8 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 3, ObjectFieldId = CustomsVendorObjectFields.Where(d => d.FieldName == "DunsNumber").FirstOrDefault().Id, ScreenId = CustomsVendorGeneralTabScreenScreen1.Id,ScreenCode = CustomsVendorGeneralTabScreenScreen1.Code, ObjectFieldCode = CustomsVendorObjectFields.Where(d => d.FieldName == "DunsNumber").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
           

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable CustomsVendorObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.CustomsVendor" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode CustomsVendorGeneralTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Vendor.TH.General", DefaultText = "General",LocalDefaultText = "כללי", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CustomsVendorGeneralFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GENERAL", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.CustomsVendor.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode CustomsVendorEventsTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Vendor.TH.Events", DefaultText = "Events",LocalDefaultText = "אירועים", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CustomsVendorEventsFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EVENTS", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.CustomsVendor.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode CustomsVendorCommunicationsTextCode_TH2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Vendor.TH.Communications", DefaultText = "Communications",LocalDefaultText = "תקשורת", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CustomsVendorCommunicationsFeature_TH2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "COMMUNICATIONS", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.CustomsVendor.Features.Communication", NameTextCodeDefaultText = "Communication", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode CustomsVendorRequestSheetTextCode_TH3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Vendor.TH.RequestSheet", DefaultText = "Request Sheet",LocalDefaultText = "גיליון בקשה", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CustomsVendorRequestSheetFeature_TH3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "REQUESTSHEETS", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.CustomsVendor.Features.RequestSheets", NameTextCodeDefaultText = "Request Sheets", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CVGN",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = CustomsVendorGeneralFeature_TH0.Id,FeatureUniqeCode = CustomsVendorGeneralFeature_TH0.FeatureUniqeCode, ControlPath = "Logitude.Customs.Views.AddVendorControl", ObjectTableId = CustomsVendorObjectTable.Id, TabNameTextCodeId = CustomsVendorGeneralTextCode_TH0.Id, TabNameTextCodeCode = CustomsVendorGeneralTextCode_TH0.Code, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CVEV",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = CustomsVendorEventsFeature_TH1.Id,FeatureUniqeCode = CustomsVendorEventsFeature_TH1.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = CustomsVendorObjectTable.Id, TabNameTextCodeId = CustomsVendorEventsTextCode_TH1.Id, TabNameTextCodeCode = CustomsVendorEventsTextCode_TH1.Code, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CVCM",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = CustomsVendorCommunicationsFeature_TH2.Id,FeatureUniqeCode = CustomsVendorCommunicationsFeature_TH2.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.Views.Communications.CommunicationsControl", ObjectTableId = CustomsVendorObjectTable.Id, TabNameTextCodeId = CustomsVendorCommunicationsTextCode_TH2.Id, TabNameTextCodeCode = CustomsVendorCommunicationsTextCode_TH2.Code, Tenant = 0, IndexOrder = 2 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CVRS",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = CustomsVendorRequestSheetFeature_TH3.Id,FeatureUniqeCode = CustomsVendorRequestSheetFeature_TH3.FeatureUniqeCode, ControlPath = " ", ObjectTableId = CustomsVendorObjectTable.Id, TabNameTextCodeId = CustomsVendorRequestSheetTextCode_TH3.Id, TabNameTextCodeCode = CustomsVendorRequestSheetTextCode_TH3.Code, Tenant = 0, IndexOrder = 3 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable CustomsVendorObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.CustomsVendor" && d.Tenant == 0).FirstOrDefault(); 

		   Feature CustomsVendorFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = true, ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomsVendor.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature CustomsVendorFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = true, ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomsVendor.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature CustomsVendorFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = true, ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomsVendor.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature CustomsVendorFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = true, ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomsVendor.Features.PackageFeature", NameTextCodeDefaultText = "CustomsVendor Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);    
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable CustomsVendorObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.CustomsVendor" && d.Tenant == 0).FirstOrDefault(); 
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
                ObjectTableId = CustomsVendorObjectTable.Id,
				 
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
                ObjectTableId = CustomsVendorObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
		   FeatureRepository featureRepository = new FeatureRepository(0); 
		   //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList(); 
		   ObjectTable CustomsVendorObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.CustomsVendor" && d.Tenant == 0).FirstOrDefault();       
    
			   Feature CustomsVendorFeature_MB0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SAVEVENDOR", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.CustomsVendor.Features.SaveVendor", NameTextCodeDefaultText = "Save", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

 

		   TextCodeRepository.SubmitChanges();
		   FeaturesRepository.SubmitChanges();
		   MenuButtonGroup CustomsVendorMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
				{
					MenuButtonGroupType = "Customs.VendorEdit",
					Name = "Customs.VendorEditButtonsGroup",
					ObjectTableId = CustomsVendorObjectTable.Id,
					Tenant = 0
				}, menuButtonGroupRepository, tenantMenuButtonGroups);        
   
			   MenuButton CustomsVendorMenuButton0 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "SaveVendor",
						Index = 1, 
						IsActive = true,
						LabelTextCodeCode = "Customs.CustomsVendor.B.SaveVendor",
						LabelTextCodeDefaultText = "Save",
						Tenant = 0,
						MenuButtonGroupId = CustomsVendorMenuButtonGroup.Id,
						ObjectTableId = CustomsVendorObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = CustomsVendorFeature_MB0.Id,
						Style = null,
						LocalDefaultText = "מִלְבַד",
						FeatureUniqeCode = CustomsVendorFeature_MB0.FeatureUniqeCode,
						HtmlComponentPath = null,
						Width = 0,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);

	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable CustomsVendorObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.CustomsVendor" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode CustomsVendorTextCode_CustomsVendorOSearchBy = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Vendor.O.SearchBy", DefaultText = "Search By",LocalDefaultText = @"חיפוש לפי", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsVendorTextCode_CustomsVendorONew = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Vendor.O.New", DefaultText = "Create Supplier",LocalDefaultText = @"הקמת ספק", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsVendorTextCode_CustomsVendorOSearch = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Vendor.O.Search", DefaultText = "Search",LocalDefaultText = @"חיפוש", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsVendorTextCode_CustomsVendorOMore = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Vendor.O.More", DefaultText = "More",LocalDefaultText = @"חיפוש מתקדם", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsVendorTextCode_CustomsVendorONewVendor = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Vendor.O.NewVendor", DefaultText = "New Vendor",LocalDefaultText = @"הקמת ספק", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsVendorTextCode_CustomsVendorOCommunications = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Vendor.O.Communications", DefaultText = "Communications",LocalDefaultText = @"תקשורות", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsVendorTextCode_CustomsVendorOLess = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Vendor.O.Less", DefaultText = "Less",LocalDefaultText = @"חיפוש רגיל", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsVendorTextCode_CustomsVendorOVendors = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Vendor.O.Vendors", DefaultText = "Vendors",LocalDefaultText = @"ספקים", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsVendorTextCode_CustomsVendorODeleteCommunication = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Vendor.O.DeleteCommunication", DefaultText = "Delete this Communication?",LocalDefaultText = @" האם למחק תקשורת זו?", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsVendorTextCode_CustomsCustomsVendorOSearchRequieredFieldsError = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsVendor.O.SearchRequieredFieldsError", DefaultText = "To search you must fill vendor name and country code",LocalDefaultText = @"עליך למלא את שדות החיפוש ", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsVendorTextCode_CustomsVendorORequierdCommunication = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Vendor.O.RequierdCommunication", DefaultText = "At least one Communication is Required",LocalDefaultText = @"תקשורת לפחות אחד נדרשת", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsVendorTextCode_CustomsVendorOExternalId = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Vendor.O.ExternalId", DefaultText = "External ID",LocalDefaultText = "ח''פ / ת''ז", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsVendorTextCode_CustomsVendorOPassportNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Vendor.O.PassportNumber", DefaultText = "Passport Number",LocalDefaultText = @"מספר דרכון", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsVendorTextCode_CustomsVendorOCityNameIsRequired = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Vendor.O.CityNameIsRequired", DefaultText = "City Name Is Required",LocalDefaultText = @"העיר שם נדרש", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsVendorTextCode_CustomsVendorOEditVendor = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Vendor.O.EditVendor", DefaultText = "Edit Vendor",LocalDefaultText = @"עריכת ספק", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsVendorTextCode_CustomsGeneralBDeleteVendor = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.B.DeleteVendor", DefaultText = "Delete Vendor",LocalDefaultText = @"מחיקת ספק", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsVendorTextCode_CustomsGeneralBOpenCommunication = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.B.OpenCommunication", DefaultText = "Open Communication",LocalDefaultText = @"פתיחת תקשורת", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsVendorTextCode_CustomsCustomsVendorODifferenceVendorCountMessagePre = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsVendor.O.DifferenceVendorCountMessagePre", DefaultText = "There are ",LocalDefaultText = @"קיימות ", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsVendorTextCode_CustomsCustomsVendorODifferenceVendorCountMessagePost = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsVendor.O.DifferenceVendorCountMessagePost", DefaultText = " search results. Please narrow your search.",LocalDefaultText = @" תוצאות. יש להוסיף קריטריונים בחיפוש, על מנת לצמצם תוצאות.", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsVendorTextCode_CustomsVendorOAddingVendorSuccessfully = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Vendor.O.AddingVendorSuccessfully", DefaultText = "Adding vendor completed successfully.",LocalDefaultText = @"הוספת ספק הושלם בהצלחה.", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsVendorTextCode_CustomsVendorONewClient = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Vendor.O.NewClient", DefaultText = "New Client",LocalDefaultText = @"שליפת יבואן", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsVendorTextCode_CustomsVendorOSearchVendors = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Vendor.O.SearchVendors", DefaultText = "Search Vendors",LocalDefaultText = @"חיפוש ספקים", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsVendorTextCode_CustomsVendorODeletevendor = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Vendor.O.Deletevendor", DefaultText = "Are you sure you want to delete this vendor?",LocalDefaultText = @"האם אתה בטוח שברצונך למחוק את הספק הזה?", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsVendorTextCode_CustomsVendorODelete = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Vendor.O.Delete", DefaultText = "Delete",LocalDefaultText = @"מחק", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsVendorTextCode_CustomsCustomsVendorOCountry = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsVendor.O.Country", DefaultText = "Country:",LocalDefaultText = @":מדינה", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsVendorTextCode_CustomsCustomsVendorOExist = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsVendor.O.Exist", DefaultText = "Exist",LocalDefaultText = @"קיים במערכת ", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsVendorTextCode_CustomsCustomsVendorOIsPalestinian = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsVendor.O.IsPalestinian", DefaultText = "Is Palestinian",LocalDefaultText = @"קיים ספק פלסטיני ", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsVendorTextCode_CustomsCustomsVendorOUpdateVendor = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsVendor.O.UpdateVendor", DefaultText = "Update Vendor Data ?",LocalDefaultText = @"עדכן נתוני ספק ?", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsVendorTextCode_CustomsCustomsVendorOAddVendorCommunication = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsVendor.O.AddVendorCommunication", DefaultText = "Add Vendor Communication",LocalDefaultText = @"הוסף תקשורת חדשה", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsVendorTextCode_CustomsCustomsVendorOmustEnterAtLeast2Chars = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsVendor.O.mustEnterAtLeast2Chars", DefaultText = "Must enter at least two chars in supplier name",LocalDefaultText = @"יש להזין לפחות 2 תווים בשם הספק", ObjectTableId = CustomsVendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 