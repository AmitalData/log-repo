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
namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.GlobalModel.EntityUpdateClasses
{
   public class ApiCredintialsUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "ApiCredintials",
			      				    DBTableName =  "ApiCredintials",
			      				    ObjectTableSingular =  "API Credentials",
			      				    ObjectTablePlural =  "API Credentials",
			      				    DefaultText =  "API Credentials",
			      				    Name =  "ApiCredintials",
			      				    IsNewWizard =  true,
			      				    NewWizardControlName =  "Simplog.Infrastructure.Views.ApiCredintials.AddEditApiCredintialsControl",
			      				    HasCustomFilter =  false,
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
			      				    SearchFields =  "ApiCredintials,ApiCredintials,Simplog.Infrastructure.Views.ApiCredintials.AddEditApiCredintialsControl,,",
			      				    IsSaveButtonVisible =  true,
			      				    EnableSecurity =  true,
			      				    ObjectTableTypeCode =  "MD",
			      				    IsComposition =  false,
			      				    MaxNumberOfCustomFields =  0,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    IsEditable =  false,
			      				    AllowedForComputingPartners =  false,
			      				    ClientModuleName =  "Infrastructure",
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasMenuButtons =  false,
			      				    HasFiltersMenu =  false,
			      				    NewWizardComponentPath =  "./InfrastructureModules/InfrastructureOthers/Components/ApiCredintials/ApiCredintialsComponent",
			      				    Code =  "APIC",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "HashedPrimaryAccessKey",
					  						ObjectTableName =  "ApiCredintials",
					  						FieldsDataType =  "Text",
					  						PMPropertyPath =  "HashedPrimaryAccessKey",
					  						ListPropertyPath =  "HashedPrimaryAccessKey",
					  						FullFieldLable =  "HashedPrimaryAccessKey",
					  						DefaultText =  @"Primary Access Key",
					  						FullLocalDefaultText =  @"HashedPrimaryAccessKey",
					  						ListFieldLable =  "HashedPrimaryAccessKeyListLable",
					  						ListLableDefaultText =  @"HashedPrimaryAccessKey",
					  						ListLocalDefaultText =  @"HashedPrimaryAccessKey",
					  						ValidForQuerySection1 =  "ApiCredintials",
					  						IsRequired =  true,
					  						DisplayInList =  false,
					  						Code =  "HashedPrimaryAccessKey",
					  						MaxLength =  40,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
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
					  						HelpTextCode =  "HashedPrimaryAccessKey",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "HashedSeconderyAccessKey",
					  						ObjectTableName =  "ApiCredintials",
					  						FieldsDataType =  "Text",
					  						PMPropertyPath =  "HashedSeconderyAccessKey",
					  						ListPropertyPath =  "HashedSeconderyAccessKey",
					  						FullFieldLable =  "HashedSeconderyAccessKey",
					  						DefaultText =  @"Secondery Access Key",
					  						FullLocalDefaultText =  @"HashedSeconderyAccessKey",
					  						ListFieldLable =  "HashedSeconderyAccessKeyListLable",
					  						ListLableDefaultText =  @"HashedSeconderyAccessKey",
					  						ListLocalDefaultText =  @"HashedSeconderyAccessKey",
					  						ValidForQuerySection1 =  "ApiCredintials",
					  						IsRequired =  true,
					  						DisplayInList =  false,
					  						Code =  "HashedSeconderyAccessKey",
					  						MaxLength =  40,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
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
					  						HelpTextCode =  "HashedSeconderyAccessKey",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UsedFor",
					  						ObjectTableName =  "ApiCredintials",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
					  						MaxLength =  250,
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
					  						PMPropertyPath =  "UsedFor",
					  						ListPropertyPath =  "UsedFor",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "ApiCredintials",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						DisplayInDocumentReferences =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "UsedFor",
					  						DefaultText =  @"Used For",
					  						ListFieldLable =  "UsedForListLable",
					  						ListLableDefaultText =  @"Used For",
					  						HelpTextCode =  "UsedFor",
					  						Code =  "UsedFor",
					  						DependencyFilter3IsList =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreateDate",
					  						ObjectTableName =  "ApiCredintials",
					  						FieldsDataType =  "DateTime",
					  						DataTypeCode =  "DateTime",
					  						MaxLength =  0,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "CreateDate",
					  						ListPropertyPath =  "CreateDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						DisplayInDocumentReferences =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "CreateDate",
					  						DefaultText =  @"Create Date",
					  						ListFieldLable =  "CreateDateListLable",
					  						ListLableDefaultText =  @"Create Date",
					  						HelpTextCode =  "CreateDate",
					  						Code =  "CreateDate",
					  						DependencyFilter3IsList =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdateDate",
					  						ObjectTableName =  "ApiCredintials",
					  						FieldsDataType =  "DateTime",
					  						DataTypeCode =  "DateTime",
					  						MaxLength =  0,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "UpdateDate",
					  						ListPropertyPath =  "UpdateDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						DisplayInDocumentReferences =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "UpdateDate",
					  						DefaultText =  @"Update Date",
					  						ListFieldLable =  "UpdateDateListLable",
					  						ListLableDefaultText =  @"Update Date",
					  						HelpTextCode =  "UpdateDate",
					  						Code =  "UpdateDate",
					  						DependencyFilter3IsList =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AllowedIPs",
					  						ObjectTableName =  "ApiCredintials",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
					  						MaxLength =  1000,
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
					  						PMPropertyPath =  "AllowedIPs",
					  						ListPropertyPath =  "AllowedIPs",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "ApiCredintials",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						DisplayInDocumentReferences =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "AllowedIPs",
					  						DefaultText =  @"Allowed IPs",
					  						ListFieldLable =  "AllowedIPsListLable",
					  						ListLableDefaultText =  @"Allowed IPs",
					  						HelpTextCode =  "AllowedIPs",
					  						Code =  "AllowedIPs",
					  						DependencyFilter3IsList =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreatedBy",
					  						ObjectTableName =  "ApiCredintials",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
					  						MaxLength =  100,
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
					  						PMPropertyPath =  "CreatedBy",
					  						ListPropertyPath =  "CreatedBy",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "ApiCredintials",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						DisplayInDocumentReferences =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "CreatedBy",
					  						DefaultText =  @"Created By",
					  						ListFieldLable =  "CreatedByListLable",
					  						ListLableDefaultText =  @"Created By",
					  						HelpTextCode =  "CreatedBy",
					  						Code =  "CreatedBy",
					  						DependencyFilter3IsList =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdatedBy",
					  						ObjectTableName =  "ApiCredintials",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
					  						MaxLength =  250,
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
					  						PMPropertyPath =  "UpdatedBy",
					  						ListPropertyPath =  "UpdatedBy",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "ApiCredintials",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						DisplayInDocumentReferences =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "UpdatedBy",
					  						DefaultText =  @"Updated By",
					  						ListFieldLable =  "UpdatedByListLable",
					  						ListLableDefaultText =  @"Updated By",
					  						HelpTextCode =  "UpdatedBy",
					  						Code =  "UpdatedBy",
					  						DependencyFilter3IsList =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "maskedPrimaryAccessKey",
					  						ObjectTableName =  "ApiCredintials",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
					  						MaxLength =  40,
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
					  						PMPropertyPath =  "maskedPrimaryAccessKey",
					  						ListPropertyPath =  "maskedPrimaryAccessKey",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "ApiCredintials",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						DisplayInDocumentReferences =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "maskedPrimaryAccessKey",
					  						DefaultText =  @"masked Primary Access Key",
					  						ListFieldLable =  "maskedPrimaryAccessKeyListLable",
					  						ListLableDefaultText =  @"masked Primary Access Key",
					  						HelpTextCode =  "maskedPrimaryAccessKey",
					  						Code =  "maskedPrimaryAccessKey",
					  						DependencyFilter3IsList =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "maskedSeconderyAccessKey",
					  						ObjectTableName =  "ApiCredintials",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
					  						MaxLength =  40,
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
					  						PMPropertyPath =  "maskedSeconderyAccessKey",
					  						ListPropertyPath =  "maskedSeconderyAccessKey",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "ApiCredintials",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						DisplayInDocumentReferences =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "masked Secondery Access Key",
					  						DefaultText =  @"masked Secondery Access Key",
					  						ListFieldLable =  "maskedSeconderyAccessKeyListLable",
					  						ListLableDefaultText =  @"masked Secondery Access Key",
					  						HelpTextCode =  "masked Secondery Access Key",
					  						Code =  "maskedSeconderyAccessKey",
					  						DependencyFilter3IsList =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ComputingPartnerId",
					  						ObjectTableName =  "ApiCredintials",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
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
					  						PMPropertyPath =  "ComputingPartnerId",
					  						ListPropertyPath =  "ComputingPartnerId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "ApiCredintials",
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
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
					  						AllowedInAirlineMessaging =  false,
					  						DisplayInDocumentReferences =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "Computing Partner",
					  						DefaultText =  @"Computing Partner",
					  						ListFieldLable =  "ComputingPartnerIdListLable",
					  						ListLableDefaultText =  @"Computing Partner",
					  						HelpTextCode =  "Computing Partner",
					  						Code =  "ComputingPartnerId",
					  						DependencyFilter3IsList =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	    {  
	        FeatureRepository featureRepository = new FeatureRepository(0); 
            List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
            IWebFreightContext objectContext = WebFreightContext.GetContext(0);  
	        QueryGroup ApiCredintialsQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "APIC", Name = "ApiCredintials" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable ApiCredintialsObjectTable = objectContext.ObjectTables.Where(d => d.Name == "ApiCredintials" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> ApiCredintialsObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "ApiCredintials").ToList();   

			   TextCode ApiCredintialsTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ApiCredintials.Q.ApiCredintials", DefaultText = @"All API Credentials",LocalDefaultText = null, ObjectTableId = ApiCredintialsObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature ApiCredintialsFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ApiCredintialsQ", ObjectTableId = ApiCredintialsObjectTable.Id, Tenant = 0, NameTextCodeCode = "ApiCredintials.Features.ApiCredintials", NameTextCodeDefaultText = "API Credintials", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query ApiCredintialsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = ApiCredintialsTextCode_0.Id, Code = "ApiCredintials",  EditWizardName = "Simplog.Infrastructure.Views.ApiCredintials.AddEditApiCredintialsControl",
			   EditWizardComponentPath = "./InfrastructureModules/InfrastructureOthers/Components/ApiCredintials/ApiCredintialsComponent",
			   QueryGroupCode = "APIC", IndexOrder = 0, Tenant = 0, ObjectTableId = ApiCredintialsObjectTable.Id, QuerySection = "ApiCredintials", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = ApiCredintialsFeature_0.Id, DefaultSortName = null, DefaultSortDirection = null }, queriesRepository, tenantQueries);
	
			 QueryColumn ApiCredintialsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApiCredintialsQuery.Id, IndexOrder = 0, ObjectFieldId = ApiCredintialsObjectFields.Where(d => d.FieldName == "UsedFor" && d.ObjectTableId == ApiCredintialsObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 200 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApiCredintialsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApiCredintialsQuery.Id, IndexOrder = 1, ObjectFieldId = ApiCredintialsObjectFields.Where(d => d.FieldName == "maskedPrimaryAccessKey" && d.ObjectTableId == ApiCredintialsObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApiCredintialsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApiCredintialsQuery.Id, IndexOrder = 2, ObjectFieldId = ApiCredintialsObjectFields.Where(d => d.FieldName == "maskedSeconderyAccessKey" && d.ObjectTableId == ApiCredintialsObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApiCredintialsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApiCredintialsQuery.Id, IndexOrder = 3, ObjectFieldId = ApiCredintialsObjectFields.Where(d => d.FieldName == "AllowedIPs" && d.ObjectTableId == ApiCredintialsObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 250 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApiCredintialsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApiCredintialsQuery.Id, IndexOrder = 4, ObjectFieldId = ApiCredintialsObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == ApiCredintialsObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 200 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApiCredintialsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApiCredintialsQuery.Id, IndexOrder = 5, ObjectFieldId = ApiCredintialsObjectFields.Where(d => d.FieldName == "UpdateDate" && d.ObjectTableId == ApiCredintialsObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 200 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApiCredintialsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApiCredintialsQuery.Id, IndexOrder = 6, ObjectFieldId = ApiCredintialsObjectFields.Where(d => d.FieldName == "CreatedBy" && d.ObjectTableId == ApiCredintialsObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 200 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ApiCredintialsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ApiCredintialsQuery.Id, IndexOrder = 7, ObjectFieldId = ApiCredintialsObjectFields.Where(d => d.FieldName == "UpdatedBy" && d.ObjectTableId == ApiCredintialsObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 200 }, queryColumnsRepository, tenantQueryColumns);
	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {    

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {      
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable ApiCredintialsObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "ApiCredintials" && d.Tenant == 0).FirstOrDefault(); 
		   Feature ApiCredintialsFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = ApiCredintialsObjectTable.Id, Tenant = 0, NameTextCodeCode = "ApiCredintials.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature ApiCredintialsFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = ApiCredintialsObjectTable.Id, Tenant = 0, NameTextCodeCode = "ApiCredintials.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature ApiCredintialsFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = ApiCredintialsObjectTable.Id, Tenant = 0, NameTextCodeCode = "ApiCredintials.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature ApiCredintialsFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = ApiCredintialsObjectTable.Id, Tenant = 0, NameTextCodeCode = "ApiCredintials.Features.PackageFeature", NameTextCodeDefaultText = "ApiCredintials Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);    
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable ApiCredintialsObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "ApiCredintials" && d.Tenant == 0).FirstOrDefault(); 
	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {     
	    
}

    

   }
    
}
	 