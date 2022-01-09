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

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.CommonDataModel.EntityUpdateClasses
{
   public class ReportUpdateClass
   {  		
		public const string HashString = "e2542d8f1f7ee3fa7ec065a816b0f422";
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "Report",
			      				    IsNew =  false,
			      				    DBTableName =  "Reports",
			      				    ObjectTableSingular =  "Report",
			      				    ObjectTablePlural =  "Reports",
			      				    DescriptionDefaultText =  "Maintain all the reports you can use in the system.",
			      				    HasCustomFilter =  false,
			      				    HasCustomFields =  false,
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
			      				    HasCounter =  true,
			      				    EnableAddFromLOV =  false,
			      				    IsRestrictable =  true,
			      				    IsMain =  true,
			      				    IsAutoComplete =  false,
			      				    EnableEditFromLOV =  false,
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  true,
			      				    HasDynamicHeader =  true,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  10,
			      				    DefaultText =  "Report",
			      				    Code =  "RPRT",
			      				    Name =  "Report",
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
			      				    SearchFields =  "Report,Reports,,Id,",
			      				    HashString =  ReportUpdateClass.HashString,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository, List<ObjectField> addedFields, List<TextCode> addedTextCodes)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Id",
					  						ObjectTableName =  "Report",
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
					  						DisplayInList =  false,
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
					  						ValidForQuerySection1 =  "Report",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "Id",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Id",
					  						DefaultText =  "Id",
					  						ListFieldLable =  "IdListLable",
					  						ListLableDefaultText =  "Id",
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
					  						HelpTextCode =  "Id",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Tenant",
					  						ObjectTableName =  "Report",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
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
					  						ValidForQuerySection1 =  "Report",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "Tenant",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Tenant",
					  						DefaultText =  "Tenant",
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
					  						HelpTextCode =  "Tenant",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Name",
					  						ObjectTableName =  "Report",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  40,
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
					  						PMPropertyPath =  "Name",
					  						ListPropertyPath =  "Name",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  true,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Report",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "Name",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Name",
					  						DefaultText =  "Name",
					  						FullLocalDefaultText =  "שם",
					  						ListFieldLable =  "NameListLable",
					  						ListLableDefaultText =  "Name",
					  						ListLocalDefaultText =  "שם",
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
					  						HelpTextCode =  "Name",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FilterControlName",
					  						ObjectTableName =  "Report",
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
					  						PMPropertyPath =  "FilterControlName",
					  						ListPropertyPath =  "FilterControlName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  true,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Report",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "FilterControlName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "FilterControlName",
					  						DefaultText =  "FilterControlName",
					  						ListFieldLable =  "FilterControlNameListLable",
					  						ListLableDefaultText =  "FilterControlName",
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
					  						HelpTextCode =  "FilterControlName",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Description",
					  						ObjectTableName =  "Report",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  250,
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
					  						PMPropertyPath =  "Description",
					  						ListPropertyPath =  "Description",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  true,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Report",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "Description",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Description",
					  						DefaultText =  "Description",
					  						ListFieldLable =  "DescriptionListLable",
					  						ListLableDefaultText =  "Description",
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
					  						HelpTextCode =  "Description",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SearchFields",
					  						ObjectTableName =  "Report",
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
					  						ValidForQuerySection1 =  "Report",
					  						ValidForQuerySection2 =  "ReportFollowUp",
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
					  						DefaultText =  "Search Tenant / Name / Description",
					  						HelpTextCode =  "SearchFields",
					  						HelpTextDefaultText =  "Searching by :\n1: Tenant \n2: Name \n3: Description",
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
					 
					 						FieldName =  "Code",
					  						ObjectTableName =  "Report",
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
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
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
					  						ValidForQuerySection1 =  "Report",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "Code",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Code",
					  						DefaultText =  "Code",
					  						ListFieldLable =  "CodeListLable",
					  						ListLableDefaultText =  "Code",
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
					  						HelpTextCode =  "Code",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InActive",
					  						ObjectTableName =  "Report",
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
					  						ValidForQuerySection1 =  "Report",
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
					  						DefaultText =  "InActive",
					  						ListFieldLable =  "InActiveListLable",
					  						ListLableDefaultText =  "InActive",
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
					  						HelpTextCode =  "InActive",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FilterHtmlComponentUrl",
					  						ObjectTableName =  "Report",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  256,
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
					  						PMPropertyPath =  "FilterHtmlComponentUrl",
					  						ListPropertyPath =  "FilterHtmlComponentUrl",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Report",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "FilterHtmlComponentUrl",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "FilterHtmlComponentUrl",
					  						DefaultText =  "FilterHtmlComponentUrl",
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
					  						HelpTextCode =  "FilterHtmlComponentUrl",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AvailableForScheduling",
					  						ObjectTableName =  "Report",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "AvailableForScheduling",
					  						ListPropertyPath =  "AvailableForScheduling",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Report",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "AvailableForScheduling",
					  						DefaultText =  "Available For Scheduling",
					  						ListFieldLable =  "AvailableForSchedulingListLable",
					  						ListLableDefaultText =  "Available For Scheduling",
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
					 
					 						FieldName =  "DisablePreview",
					  						ObjectTableName =  "Report",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "DisablePreview",
					  						ListPropertyPath =  "DisablePreview",
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
					  						FullFieldLable =  "DisablePreview",
					  						DefaultText =  "Disable Preview",
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
					 
					 						FieldName =  "IsExcelReportAllowed",
					  						ObjectTableName =  "Report",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsExcelReportAllowed",
					  						ListPropertyPath =  "IsExcelReportAllowed",
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Report",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsExcelReportAllowed",
					  						DefaultText =  "Excel Report Allowed",
					  						ListFieldLable =  "IsExcelReportAllowedListLable",
					  						ListLableDefaultText =  "Excel Report Allowed",
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
	        QueryGroup ReportQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "RPRT", Name = "Report" }, queryGroupRepository,tenantQueryGroups);
						QueryGroup ReportQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "d689", Name = " Query Group" }, queryGroupRepository,tenantQueryGroups);
				        queryGroupRepository.SubmitChanges();
	        ObjectTable ReportObjectTable = objectTables.ContainsKey("Report") ? objectTables["Report"] : null;
            if (ReportObjectTable == null)
            {
                IWebFreightContext objectContext = WebFreightContext.GetContext(0);  

                ReportObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Report" && d.Tenant == 0).FirstOrDefault();
            }

	         
			List<Feature> addedFeatures = new List<Feature>();
			List<TextCode> addedTextCodes = new List<TextCode>();
			List<Query> addedQueries = new List<Query>();
			List<QueryColumn> addedQueryColumns = new List<QueryColumn>();
			List<AdvancedQueryFilter> addedQueryFilters = new List<AdvancedQueryFilter>();
   

			   TextCode ReportTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Report.Q.AllReports", DefaultText = @"All Reports",LocalDefaultText = null, ObjectTableId = ReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, textCodes, addedTextCodes);
			   Feature ReportFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ALLREPORTS", ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.AllReports", NameTextCodeDefaultText = "All Reports", FeatureTypeCode = "QUER", Packagable = true }, TenantFeatures, textCodes,ReportObjectTable, addedFeatures, addedTextCodes);

	        //TextCodeRepository.SubmitChanges();
	        //FeaturesRepository.SubmitChanges();    
	      

			  Query AllReportsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = ReportTextCode_0.Id, NameTextCodeCode = ReportTextCode_0.Code, ObjectTableName = "Report", Code = "All Reports",  QueryGroupCode = "RPRT", IndexOrder = 0, Tenant = 0, ObjectTableId = ReportObjectTable.Id, QuerySection = "Report", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = ReportFeature_0.Id,FeatureUniqeCode= ReportFeature_0.FeatureUniqeCode, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, addedQueries);
	
			 QueryColumn AllReportsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllReportsQuery.Id,QueryCode = AllReportsQuery.UniqueCode, IndexOrder = 0, ObjectFieldCode = "Report.Name" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn AllReportsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllReportsQuery.Id,QueryCode = AllReportsQuery.UniqueCode, IndexOrder = 1, ObjectFieldCode = "Report.Description" , ColumnWidth = 130 }, addedQueryColumns);

			 QueryColumn AllReportsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllReportsQuery.Id,QueryCode = AllReportsQuery.UniqueCode, IndexOrder = 2, ObjectFieldCode = "Report.FilterControlName" , ColumnWidth = 130 }, addedQueryColumns);
			SqlBulkInsert.BulkInsert("TextCodes", addedTextCodes);
			SqlBulkInsert.BulkInsert("Features", addedFeatures);
			SqlBulkInsert.BulkInsert("Queries", addedQueries);
			SqlBulkInsert.BulkInsert("QueryColumns", addedQueryColumns);
			SqlBulkInsert.BulkInsert("AdvancedQueryFilters", addedQueryFilters);	 
  
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable ReportObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Report" && d.Tenant == 0).FirstOrDefault();
		   //List<ObjectField> ReportObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Report").ToList();
		       
	      

	         Screen ReportHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Report.HeaderScreen", Name = "Header Screen", ObjectTableId = ReportObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      
             ScreenField ReportReportHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ScreenId = ReportHeaderScreenScreen0.Id,ScreenCode = ReportHeaderScreenScreen0.Code, ObjectFieldCode = "Report.Name", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField ReportReportHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ScreenId = ReportHeaderScreenScreen0.Id,ScreenCode = ReportHeaderScreenScreen0.Code, ObjectFieldCode = "Report.FilterControlName", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          	
		    ReportObjectTable.HeaderScreenId = ReportHeaderScreenScreen0.Id;
		    ReportObjectTable.HeaderScreenCode = ReportHeaderScreenScreen0.Code;

	   		  
	      

	         Screen ReportGeneralTabScreenScreen1 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Report.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = ReportObjectTable.Id, NumberOfColumns = 1, NumberOfRows = 5, IsReadOnly = false }, screensRepository, tenantScreens);
      
             ScreenField ReportReportGeneralTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ScreenId = ReportGeneralTabScreenScreen1.Id,ScreenCode = ReportGeneralTabScreenScreen1.Code, ObjectFieldCode = "Report.Code", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField ReportReportGeneralTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ScreenId = ReportGeneralTabScreenScreen1.Id,ScreenCode = ReportGeneralTabScreenScreen1.Code, ObjectFieldCode = "Report.Name", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField ReportReportGeneralTabScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ScreenId = ReportGeneralTabScreenScreen1.Id,ScreenCode = ReportGeneralTabScreenScreen1.Code, ObjectFieldCode = "Report.Description", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField ReportReportGeneralTabScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 3, ScreenId = ReportGeneralTabScreenScreen1.Id,ScreenCode = ReportGeneralTabScreenScreen1.Code, ObjectFieldCode = "Report.FilterControlName", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	          
             ScreenField ReportReportGeneralTabScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 4, ScreenId = ReportGeneralTabScreenScreen1.Id,ScreenCode = ReportGeneralTabScreenScreen1.Code, ObjectFieldCode = "Report.InActive", Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
	            

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable ReportObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Report" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode ReportGeneralTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Report.TH.General", DefaultText = "General",LocalDefaultText = null, ObjectTableId = ReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature ReportGeneralFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GENERAL", ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,ReportObjectTable);
 
                 
			   TextCode ReportReportTemplateTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Report.TH.Template", DefaultText = "Report Template",LocalDefaultText = null, ObjectTableId = ReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature ReportReportTemplateFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TEMPLATE", ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.Template", NameTextCodeDefaultText = "Report Template", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,ReportObjectTable);
 
                 
			   TextCode ReportEventsTextCode_TH2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Report.TH.Events", DefaultText = "Events",LocalDefaultText = null, ObjectTableId = ReportObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature ReportEventsFeature_TH2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EVENTS", ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes,ReportObjectTable);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "RPGC",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = ReportGeneralFeature_TH0.Id,FeatureUniqeCode = ReportGeneralFeature_TH0.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.GeneralControls.GeneralTabControl", ObjectTableId = ReportObjectTable.Id, TabNameTextCodeId = ReportGeneralTextCode_TH0.Id, TabNameTextCodeCode = ReportGeneralTextCode_TH0.Code, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "RPTP",HtmlComponentName = "ReportTemplateComponent",HtmlComponentUrl = "./Report/Components/ReportTemplateComponent", FeatureId = ReportReportTemplateFeature_TH1.Id,FeatureUniqeCode = ReportReportTemplateFeature_TH1.FeatureUniqeCode, ControlPath = "Logitude.Reports.ReportTemplateControl", ObjectTableId = ReportObjectTable.Id, TabNameTextCodeId = ReportReportTemplateTextCode_TH1.Id, TabNameTextCodeCode = ReportReportTemplateTextCode_TH1.Code, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "RPEV",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = ReportEventsFeature_TH2.Id,FeatureUniqeCode = ReportEventsFeature_TH2.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = ReportObjectTable.Id, TabNameTextCodeId = ReportEventsTextCode_TH2.Id, TabNameTextCodeCode = ReportEventsTextCode_TH2.Code, Tenant = 0, IndexOrder = 2 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable ReportObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Report" && d.Tenant == 0).FirstOrDefault(); 

		   Feature ReportFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "Report.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);
		   Feature ReportFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "Report.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);
		   Feature ReportFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "Report.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);
		   Feature ReportFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "Report.Features.PackageFeature", NameTextCodeDefaultText = "Report Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable); 

		   		   //--------------> Additional Features <--------------\\

		   Feature ReportFeature_STATISTICSBYAIRLINE = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "STATISTICSBYAIRLINE", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.StatisticsbyAirline", NameTextCodeDefaultText = @"Statistics by Airline" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_STATISTICSBYSHIPPINGLINES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "STATISTICSBYSHIPPINGLINES", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.StatisticsbyShippingLines", NameTextCodeDefaultText = @"Statistics by Shipping Lines" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_PROFITBYSHIPMENTSTATISTICS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PROFITBYSHIPMENTSTATISTICS", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.ProfitByShipmentStatistics", NameTextCodeDefaultText = @"Profit By Shipment Statistics" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_STATISTICSBYCUSTOMER = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "STATISTICSBYCUSTOMER", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.StatisticsByCustomer", NameTextCodeDefaultText = @"Statistics By Customer" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_STATEMENT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "STATEMENT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.Statement", NameTextCodeDefaultText = @"Statement" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_INVOICEBYPARTNER = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "INVOICEBYPARTNER", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.InvoicesByPartner", NameTextCodeDefaultText = @"Invoices By Partner" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_QUOTESSTATISTICS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "QUOTESSTATISTICS", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.QuotesStatistics", NameTextCodeDefaultText = @"Quotes Statistics" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_ARINVOICEINCLUDEVAT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ARINVOICEINCLUDEVAT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.ARInvoiceIncludeVAT", NameTextCodeDefaultText = @"AR Invoice include VAT" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_AGEDACCOUNTSRECIEVABLES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AGEDACCOUNTSRECIEVABLES", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.AgedAccountsReceivable", NameTextCodeDefaultText = @"Aging report - Statement summarized" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_OPENFORMATREPORT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OPENFORMATREPORT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.OpenFormatReport", NameTextCodeDefaultText = @"Open Format Report" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_IATASTATISTICS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "IATASTATISTICS", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.IATAStatistics", NameTextCodeDefaultText = @"IATA Statistics" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_ACCOUNTINGLEDGER = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACCOUNTINGLEDGER", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.AccountingLedger", NameTextCodeDefaultText = @"Accounting Ledger" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_APINVOICEINCLUDEVAT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "APINVOICEINCLUDEVAT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.APInvoiceIncludeVAT", NameTextCodeDefaultText = @"AP Invoice Include VAT" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_OCEANSHIPMENTREPORT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OCEANSHIPMENTREPORT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.OceanShipmentReport", NameTextCodeDefaultText = @"Ocean Shipment Report to Client" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_STATEMENTBYINVOICEDATE = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "STATEMENTBYINVOICEDATE", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.StatementByInvoiceDate", NameTextCodeDefaultText = @"Statement By Invoice Date" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_STAGECHANGINGREPORT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "STAGECHANGINGREPORT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.StageChangingReport", NameTextCodeDefaultText = @"Stage Changing Report" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_MONTHLYCONVERSION = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MONTHLYCONVERSION", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.MonthlyConversionReport", NameTextCodeDefaultText = @"Monthly Conversion Report" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_EXPECTEDINCOMEREPORT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EXPECTEDINCOMEREPORT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.ExpectedIncomeReport", NameTextCodeDefaultText = @"Expected monthly income report" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_CONTAINERTRUCKINGREPORT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CONTAINERTRUCKINGREPORT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.ContainerTruckingReport", NameTextCodeDefaultText = @"Container Trucking" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_CONTAINERDETAILSBYVOYAGE = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CONTAINERDETAILSBYVOYAGE", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.ContainerDetailsByVoyage", NameTextCodeDefaultText = @"Container Details by Voyage" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_APPROVEDOPPORTUNITIES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "APPROVEDOPPORTUNITIES", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.ApprovedOpportunities", NameTextCodeDefaultText = @"Approved Opportunities: Potential vs. Actual" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_CUSTOMERADDITIONALSERVICES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CUSTOMERADDITIONALSERVICES", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.CustomerAdditionalServices", NameTextCodeDefaultText = @"Customer Additional Services" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_CUSTOMERPOTENTIALACTUAL = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CUSTOMERPOTENTIALACTUAL", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.CustomerPotentialActual", NameTextCodeDefaultText = @"Customer Potential vs. Actual" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_OPPORTUNITIESSERVICES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OPPORTUNITIESSERVICES", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.OpportunitiesByAdditionalServices", NameTextCodeDefaultText = @"Opportunities by Additional Services" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_STATISTICSBYAGENT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "STATISTICSBYAGENT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.StatisticsByAgent", NameTextCodeDefaultText = @"Statistics By Agent" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_ACTIVITYSTATUSDASHBOARD = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACTIVITYSTATUSDASHBOARD", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.ActivityStatusDashBoard", NameTextCodeDefaultText = @"Activity Status Dashboard" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_BOOKINGREPORT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BOOKINGREPORT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.BookingReport", NameTextCodeDefaultText = @"e-Booking Report" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_EAWBREPORT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EAWBREPORT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.EAWBReport", NameTextCodeDefaultText = @"e-AWBs Report" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_FLIGHTREPORT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "FLIGHTREPORT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.FlightBookingList", NameTextCodeDefaultText = @"Flight Booking List" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_SHIPMENTANALYSIS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SHIPMENTANALYSIS", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.ShipmentChargesAnalysis", NameTextCodeDefaultText = @"Shipment Charges Analysis" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_ACTIVEPARTICIPANTS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACTIVEPARTICIPANTS", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.ParticipantsUsersActivities", NameTextCodeDefaultText = @"Participants Users Activities" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_ARINVOICEINCLUDEVATROUTINGS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ARINVOICEINCLUDEVATROUTINGS", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.ARInvoiceIncludeVATRoutings", NameTextCodeDefaultText = @"AR Invoice include VAT and Routings" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_OCEANINSIGHTREQUESTSREPORT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OCEANINSIGHTREQUESTSREPORT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.OceanInsightRequests", NameTextCodeDefaultText = @"Ocean Insight Requests" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_ARINVOICEBANKDEPOSIT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ARINVOICEBANKDEPOSIT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.ARInvoiceBankDeposit", NameTextCodeDefaultText = @"Deposit report" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_CASSREPORT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CASSREPORT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.CASSReport", NameTextCodeDefaultText = @"CASSReport" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_Report_Features_ShipmentProfitVSQuoteEstimate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Report.Features.ShipmentProfitVSQuoteEstimate", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "Report.Features.ShipmentProfitVSQuoteEstimate", NameTextCodeDefaultText = @"Shipment Profit vs. Quote Estimate" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_Report_Features_ArchivoExportado = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Report.Features.ArchivoExportado", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "Report.Features.ArchivoExportado", NameTextCodeDefaultText = @"Archivo Exportado" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_INVENTORYREPORT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "INVENTORYREPORT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "Report.Features.InventoryReport", NameTextCodeDefaultText = @"Inventory Report" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_EMPLOYEETIMESHEETREPORT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EMPLOYEETIMESHEETREPORT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.EmployeeTimeSheet", NameTextCodeDefaultText = @"Employee TimeSheet" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_WORKHOURSPERPROJECTREPORT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "WORKHOURSPERPROJECTREPORT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.WorkHoursPerProject", NameTextCodeDefaultText = @"Work Hours Per Project" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_Report_Features_AccAgingReport = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Report.Features.AccAgingReport", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "Report.Features.AccAgingReport", NameTextCodeDefaultText = @"Accounting aging report" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_SHIPMENTANALYSISTOTALS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SHIPMENTANALYSISTOTALS", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.ShipmentChargesAnalysisIncludeTotals", NameTextCodeDefaultText = @"Shipment Charges Analysis include Totals" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_OPENSHIPMENTBYCUSTOMER = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OPENSHIPMENTBYCUSTOMER", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.OpenShipmentsByCustomer", NameTextCodeDefaultText = @"Open Shipments by Customer" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_PARENTTENANT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PARENTTENANT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.ParentVsChildTenants", NameTextCodeDefaultText = @"Parent vs. Child Tenants" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_REVEXP = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "REVEXP", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.RevenueExpense", NameTextCodeDefaultText = @"Revenue\Expense" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_TRAIL = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TRAIL", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.TrailBalance", NameTextCodeDefaultText = @"Trail Balance" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_WORKDAYSPERPROJECTREPORT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "WORKDAYSPERPROJECTREPORT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.WorkDaysPerProject", NameTextCodeDefaultText = @"Work Days Per Project" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_USERSBYTENANTREPORT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "USERSBYTENANTREPORT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.UsersbyTenantReport", NameTextCodeDefaultText = @"Users by Tenant Report" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_LICENSEMANAGEMENTREPORT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "LICENSEMANAGEMENTREPORT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.LicenseManagement", NameTextCodeDefaultText = @"License Management" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_SHIPMENTSSTOCKS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SHIPMENTSSTOCKS", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.ShipmentsStocks", NameTextCodeDefaultText = @"Shipments Stocks" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_TASKSWITHNORPROJECTSREPORT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TASKSWITHNORPROJECTSREPORT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.TasksWithoutProjects", NameTextCodeDefaultText = @"Tasks not Connected to Projects" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_SHIPMENTDETAILS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SHIPMENTDETAILS", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.ShipmentDetails", NameTextCodeDefaultText = @"Shipment Details" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_Report_Features_DetailedShipmentCharges = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Report.Features.DetailedShipmentCharges", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "Report.Features.Report.Features.DetailedShipmentCharges", NameTextCodeDefaultText = @"Detailed Shipment Charges Analysis" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_UNER = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UNER", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "Report.Features.UNER", NameTextCodeDefaultText = @"Unicargo Export Report" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_FlightBookingManifest = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "FlightBookingManifest", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "Report.Features.FlightBookingManifest", NameTextCodeDefaultText = @"Flight Bookings Manifest" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_BLUESNAPPAYMENTREPORT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BLUESNAPPAYMENTREPORT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "Report.Features.BLUESNAPPAYMENTREPORT", NameTextCodeDefaultText = @"Bluesnap Payments Report" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_INTERESTREPORT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "INTERESTREPORT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "Report.Features.INTERESTREPORT", NameTextCodeDefaultText = @"Interest Report" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_REPORTSSCHEDULER = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "REPORTSSCHEDULER", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "Report.Features.REPORTSSCHEDULER", NameTextCodeDefaultText = @"Reports Scheduler" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_Report_Features_CSStatusReport = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Report.Features.CSStatusReport", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "Report.Features.Report.Features.CSStatusReport", NameTextCodeDefaultText = @"Customer Status Report" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_Report_Features_VendorCharges = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Report.Features.VendorCharges", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.VendorChargesAnalysis", NameTextCodeDefaultText = @"Vendor Charges Analysis" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_VDK = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "VDK", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.VDK", NameTextCodeDefaultText = @"VDK Report Templates" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_Report_Features_LTReport = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Report.Features.LTReport", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "Report.Features.LTReport", NameTextCodeDefaultText = @"Ledger Transaction Report" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_Vehicles = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Vehicles", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.Vehicles", NameTextCodeDefaultText = @"Vehicles Shipping" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_SHEL = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SHEL", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.ShipmentsEventsList", NameTextCodeDefaultText = @"Shipments Events List" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_ATRE = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ATRE", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.AutomationTestReport", NameTextCodeDefaultText = @"Automation Test Report" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_WORKDAYSPERGATEGORY = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "WORKDAYSPERGATEGORY", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.WorkDaysPerCategory", NameTextCodeDefaultText = @"Work Days Per Category" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_SHRR = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SHRR", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.ShipperReturnsReport", NameTextCodeDefaultText = @"Shipper Returns Report" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_RCRF = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "RCRF", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "ReportObjectTable.Features.RacingReportFeature", NameTextCodeDefaultText = @"Racing - Quotes" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_ExternalReconciliationLinesReport = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ExternalReconciliationLinesReport", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "Report.Features.ExternalReconciliationLinesReport", NameTextCodeDefaultText = @"External Reconciliation Lines Report" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_UserDefinedReport = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UserDefinedReport", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "Report.Features.UserDefinedReport", NameTextCodeDefaultText = @"User Defined Report" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_LOCR = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "LOCR", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "Report.Features.LOCR", NameTextCodeDefaultText = @"Logitude CRM Report" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

		   Feature ReportFeature_PerVendorReport = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PerVendorReport", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = ReportObjectTable.Id, Tenant = 0, NameTextCodeCode = "Report.Features.PerVendorReport", NameTextCodeDefaultText = @"856 Per Vendor Report" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,ReportObjectTable);

   
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable ReportObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Report" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "UPRP",
                EnglishName =  "Report Updated",
                LocalName =  "Report Updated",
                IsManualEntry =  false,
                ShortView =  false,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = ReportObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CRRP",
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
                ObjectTableId = ReportObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "TEPU",
                EnglishName =  "New Template Uploaded",
                LocalName =  "New Template Uploaded",
                IsManualEntry =  false,
                ShortView =  true,
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = ReportObjectTable.Id,
				 
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
	 