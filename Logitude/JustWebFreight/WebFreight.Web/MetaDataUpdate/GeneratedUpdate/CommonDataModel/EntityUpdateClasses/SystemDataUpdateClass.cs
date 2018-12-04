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
namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.CommonDataModel.EntityUpdateClasses
{
   public class SystemDataUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "SystemData",
			      				    DBTableName =  "SystemDatas",
			      				    ObjectTableSingular =  "System Data",
			      				    ObjectTablePlural =  "SystemData",
			      				    DefaultText =  "System Data",
			      				    Name =  "SystemData",
			      				    IsNewWizard =  false,
			      				    KeyPropertyPath =  "Id",
			      				    AutoCompleteSearchWindow =  false,
			      				    IsClosed =  false,
			      				    CacheOnClient =  false,
			      				    EditableFromAutoCompleteWindow =  false,
			      				    HasCounter =  false,
			      				    EnableEditFromLOV =  false,
			      				    EnableAddFromLOV =  false,
			      				    IsRestrictable =  false,
			      				    IsMain =  true,
			      				    IsAutoComplete =  false,
			      				    CustomFieldsCount =  0,
			      				    HasCustomFields =  false,
			      				    InActive =  false,
			      				    SearchFields =  "SystemData,,,Id,",
			      				    IsSaveButtonVisible =  true,
			      				    EnableSecurity =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    IsComposition =  false,
			      				    MaxNumberOfCustomFields =  0,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    IsEditable =  true,
			      				    NoViewsController =  true,
			      				    HasCustomFilter =  false,
			      				    AllowedForComputingPartners =  false,
			      				    DisableSearchBox =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasMenuButtons =  false,
			      				    HasFiltersMenu =  false,
			      				    AllowedInQueues =  false,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Date",
					  						ObjectTableName =  "SystemData",
					  						FieldsDataType =  "DateTime",
					  						Code =  "Date",
					  						MaxLength =  250,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "Date",
					  						ListPropertyPath =  "Date",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "Date",
					  						DefaultText =  @"Date",
					  						HelpTextCode =  "Date",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UserId",
					  						ObjectTableName =  "SystemData",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
					  						Code =  "UserId",
					  						MaxLength =  15,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "UserId",
					  						ListPropertyPath =  "UserId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "UserId",
					  						DefaultText =  @"User",
					  						HelpTextCode =  "UserId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Signature",
					  						ObjectTableName =  "SystemData",
					  						FieldsDataType =  "Text",
					  						Code =  "Signature",
					  						MaxLength =  250,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "Signature",
					  						ListPropertyPath =  "Signature",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "Signature",
					  						DefaultText =  @"Signature",
					  						HelpTextCode =  "Signature",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Logo",
					  						ObjectTableName =  "SystemData",
					  						FieldsDataType =  "Text",
					  						Code =  "Logo",
					  						MaxLength =  250,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "Logo",
					  						ListPropertyPath =  "Logo",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "Logo",
					  						DefaultText =  @"Logo",
					  						HelpTextCode =  "Logo",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SmallLogo",
					  						ObjectTableName =  "SystemData",
					  						FieldsDataType =  "Text",
					  						Code =  "SmallLogo",
					  						MaxLength =  250,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "SmallLogo",
					  						ListPropertyPath =  "SmallLogo",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "SmallLogo",
					  						DefaultText =  @"Small Logo",
					  						HelpTextCode =  "SmallLogo",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LocalCurrencyId",
					  						ObjectTableName =  "SystemData",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Currency",
					  						Code =  "LocalCurrencyId",
					  						MaxLength =  15,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "LocalCurrencyId",
					  						ListPropertyPath =  "LocalCurrencyId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "LocalCurrencyId",
					  						DefaultText =  @"Local Currency",
					  						HelpTextCode =  "LocalCurrencyId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Company",
					  						ObjectTableName =  "SystemData",
					  						FieldsDataType =  "Text",
					  						Code =  "Company",
					  						MaxLength =  250,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "Company",
					  						ListPropertyPath =  "Company",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "Company",
					  						DefaultText =  @"Company",
					  						HelpTextCode =  "Company",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Email",
					  						ObjectTableName =  "SystemData",
					  						FieldsDataType =  "Text",
					  						Code =  "Email",
					  						MaxLength =  250,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "Email",
					  						ListPropertyPath =  "Email",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "Email",
					  						DefaultText =  @"Email",
					  						HelpTextCode =  "Email",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Website",
					  						ObjectTableName =  "SystemData",
					  						FieldsDataType =  "Text",
					  						Code =  "Website",
					  						MaxLength =  250,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "Website",
					  						ListPropertyPath =  "Website",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "Website",
					  						DefaultText =  @"Website",
					  						HelpTextCode =  "Website",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IATA",
					  						ObjectTableName =  "SystemData",
					  						FieldsDataType =  "Text",
					  						Code =  "IATA",
					  						MaxLength =  250,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "IATA",
					  						ListPropertyPath =  "IATA",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "IATA",
					  						DefaultText =  @"IATA",
					  						HelpTextCode =  "IATA",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "VatNumber",
					  						ObjectTableName =  "SystemData",
					  						FieldsDataType =  "Text",
					  						Code =  "VatNumber",
					  						MaxLength =  250,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "VatNumber",
					  						ListPropertyPath =  "VatNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "VatNumber",
					  						DefaultText =  @"VAT No.",
					  						HelpTextCode =  "VatNumber",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AddressId",
					  						ObjectTableName =  "SystemData",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Address",
					  						Code =  "AddressId",
					  						MaxLength =  15,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "AddressId",
					  						ListPropertyPath =  "AddressId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "AddressId",
					  						DefaultText =  @"Address ID",
					  						HelpTextCode =  "AddressId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ContactId",
					  						ObjectTableName =  "SystemData",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Contact",
					  						Code =  "ContactId",
					  						MaxLength =  15,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "ContactId",
					  						ListPropertyPath =  "ContactId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "ContactId",
					  						DefaultText =  @"Contact",
					  						HelpTextCode =  "ContactId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Supportemail",
					  						ObjectTableName =  "SystemData",
					  						FieldsDataType =  "Text",
					  						Code =  "Supportemail",
					  						MaxLength =  100,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "Supportemail",
					  						ListPropertyPath =  "Supportemail",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "Supportemail",
					  						DefaultText =  @"Support e-mail",
					  						HelpTextCode =  "Supportemail",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	    {  	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {    

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {      
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable SystemDataObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "SystemData" && d.Tenant == 0).FirstOrDefault(); 
		   Feature SystemDataFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = SystemDataObjectTable.Id, Tenant = 0, NameTextCodeCode = "SystemData.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature SystemDataFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = SystemDataObjectTable.Id, Tenant = 0, NameTextCodeCode = "SystemData.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature SystemDataFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = SystemDataObjectTable.Id, Tenant = 0, NameTextCodeCode = "SystemData.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature SystemDataFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = SystemDataObjectTable.Id, Tenant = 0, NameTextCodeCode = "SystemData.Features.PackageFeature", NameTextCodeDefaultText = "SystemData Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);    
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable SystemDataObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "SystemData" && d.Tenant == 0).FirstOrDefault(); 
	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable SystemDataObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "SystemData" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode SystemDataTextCode_SystemData = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "SystemData", DefaultText = "System Data",LocalDefaultText = null, ObjectTableId = SystemDataObjectTable.Id, Tenant = 0, TextCodeTypeCode = "T", IsSpellChecked = true }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 