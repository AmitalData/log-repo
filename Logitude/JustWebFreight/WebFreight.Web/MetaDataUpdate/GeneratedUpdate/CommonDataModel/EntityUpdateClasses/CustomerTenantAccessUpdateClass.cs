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

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.CommonDataModel.EntityUpdateClasses
{
   public class CustomerTenantAccessUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "CustomerTenantAccess",
			      				    IsNew =  false,
			      				    DBTableName =  "CustomerTenantAccesses",
			      				    OldDBTableName =  "CustomerTenantAccesses",
			      				    ObjectTableSingular =  "Customer Tenant Access",
			      				    ObjectTablePlural =  "Customer Tenant Accesses",
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
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  false,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    DefaultText =  "Customer Tenant Access",
			      				    Code =  "CTAG",
			      				    Name =  "CustomerTenantAccesses",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Common",
			      				    NoTS =  false,
			      				    HasMenuButtons =  true,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    SearchFields =  "CustomerTenantAccess,CustomerTenantAccesses,,Id,",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CustomerTenant",
					  						OldFieldName =  "CustomerTenant",
					  						ObjectTableName =  "CustomerTenantAccess",
					  						FieldsDataType =  "Integer",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CustomerTenant",
					  						ListPropertyPath =  "CustomerTenant",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomerTenantAccess",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "CustomerTenant",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CustomerTenant",
					  						DefaultText =  "CustomerTenant",
					  						ListFieldLable =  "CustomerTenantListLable",
					  						ListLableDefaultText =  "Tenant",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "CustomerTenant",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CustomerIdInCustomerTenant",
					  						OldFieldName =  "CustomerIdInCustomerTenant",
					  						ObjectTableName =  "CustomerTenantAccess",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CustomerTenant",
					  						ListPropertyPath =  "CustomerTenant",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomerTenantAccess",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "CustomerIdInCustomerTenant",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CustomerIdInCustomerTenant",
					  						DefaultText =  "CustomerIdInCustomerTenant",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "CustomerIdInCustomerTenant",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ContactName",
					  						OldFieldName =  "ContactName",
					  						ObjectTableName =  "CustomerTenantAccess",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  60,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  true,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  true,
					  						PMPropertyPath =  "ContactName",
					  						ListPropertyPath =  "ContactName",
					  						DisplayInLookUpIndex =  1,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomerTenantAccess",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ContactName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ContactName",
					  						DefaultText =  "Contact Name",
					  						ListFieldLable =  "ContactNameListLable",
					  						ListLableDefaultText =  "Contact",
					  						ListLocalDefaultText =  "Contact",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Contact",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CompanyVat",
					  						OldFieldName =  "CompanyVat",
					  						ObjectTableName =  "CustomerTenantAccess",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  20,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  true,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  true,
					  						PMPropertyPath =  "CompanyVat",
					  						ListPropertyPath =  "CompanyVat",
					  						DisplayInLookUpIndex =  1,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomerTenantAccess",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "CompanyVat",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CompanyVat",
					  						DefaultText =  "Company VAT",
					  						ListFieldLable =  "CompanyVatListLable",
					  						ListLableDefaultText =  "VAT #",
					  						ListLocalDefaultText =  "CompanyVAT",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Company VAT",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CompanyName",
					  						OldFieldName =  "CompanyName",
					  						ObjectTableName =  "CustomerTenantAccess",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CompanyName",
					  						ListPropertyPath =  "CompanyName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomerTenantAccess",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "CompanyName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CompanyName",
					  						DefaultText =  "Company Name",
					  						ListFieldLable =  "CompanyNameListLable",
					  						ListLableDefaultText =  "Company",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Company",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CompanyEmail",
					  						OldFieldName =  "CompanyEmail",
					  						ObjectTableName =  "CustomerTenantAccess",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  70,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CompanyEmail",
					  						ListPropertyPath =  "CompanyEmail",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomerTenantAccess",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "CompanyEmail",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CompanyEmail",
					  						DefaultText =  "Company Email",
					  						ListFieldLable =  "CompanyEmailListLable",
					  						ListLableDefaultText =  "Company Email",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Company Email",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ContactMobile",
					  						OldFieldName =  "ContactMobile",
					  						ObjectTableName =  "CustomerTenantAccess",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  25,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ContactMobile",
					  						ListPropertyPath =  "ContactMobile",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomerTenantAccess",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ContactMobile",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ContactMobile",
					  						DefaultText =  "Contact Mobile",
					  						ListFieldLable =  "ContactMobileListLable",
					  						ListLableDefaultText =  "Contact Mobile",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Contact Mobile",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ContactPhone",
					  						OldFieldName =  "ContactPhone",
					  						ObjectTableName =  "CustomerTenantAccess",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  25,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "ContactPhone",
					  						ListPropertyPath =  "ContactPhone",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomerTenantAccess",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "ContactPhone",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "ContactPhone",
					  						DefaultText =  "Contact Phone",
					  						ListFieldLable =  "ContactPhoneListLable",
					  						ListLableDefaultText =  "Contact Phone",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Contact Phone",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "RequestDateTime",
					  						OldFieldName =  "RequestDateTime",
					  						ObjectTableName =  "CustomerTenantAccess",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "RequestDateTime",
					  						ListPropertyPath =  "RequestDateTime",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomerTenantAccess",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "RequestDateTime",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "RequestDateTime",
					  						DefaultText =  "RequestDateTime",
					  						ListFieldLable =  "RequestDateTimeListLable",
					  						ListLableDefaultText =  "Request DateTime",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Request Date",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Status",
					  						OldFieldName =  "Status",
					  						ObjectTableName =  "CustomerTenantAccess",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "CustomerTenantAccessStatusType",
					  						MinLength =  0,
					  						MaxLength =  2,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Status",
					  						ListPropertyPath =  "Status",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomerTenantAccess",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "Status",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Status",
					  						DefaultText =  "Status",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Status",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "StatusName",
					  						OldFieldName =  "StatusName",
					  						ObjectTableName =  "CustomerTenantAccess",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  40,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						DataTemplateName =  "LogBoxStatusDataTemplate",
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
					  						ValidForQuerySection1 =  "CustomerTenantAccess",
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
					  						ColumnHeaderTemplateName =  "LogBoxStatusDataTemplateControl",
					  						DisplayLongName =  false,
					  						FullFieldLable =  "StatusName",
					  						DefaultText =  "Status",
					  						ListFieldLable =  "StatusNameListLable",
					  						ListLableDefaultText =  "Status",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  true,
					  						IsCustom =  false,
					  						HelpTextCode =  "StatusName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdatedByUserId",
					  						OldFieldName =  "UpdatedByUserId",
					  						ObjectTableName =  "CustomerTenantAccess",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
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
					  						ValidForQuerySection1 =  "CustomerTenantAccess",
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
					  						DefaultText =  "UpdatedBy UserId",
					  						ListFieldLable =  "UpdatedByUserIdListLable",
					  						ListLableDefaultText =  "Updated By User",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Updated By User",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LastUpdateDate",
					  						OldFieldName =  "LastUpdateDate",
					  						ObjectTableName =  "CustomerTenantAccess",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "LastUpdateDate",
					  						ListPropertyPath =  "LastUpdateDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomerTenantAccess",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "LastUpdateDate",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LastUpdateDate",
					  						DefaultText =  "Last Update Date",
					  						ListFieldLable =  "LastUpdateDateListLable",
					  						ListLableDefaultText =  "Last Update Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Last Update Date",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SearchFields",
					  						OldFieldName =  "SearchFields",
					  						ObjectTableName =  "CustomerTenantAccess",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  1000,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Contains",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomerTenantAccess",
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
					  						DefaultText =  "Search by Contact , Customer , Company",
					  						HelpTextCode =  "SearchFields",
					  						HelpTextDefaultText =  "Searching by :\n1: code\n2: EnglishName\n3: LocalName",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LastShipmentDate",
					  						OldFieldName =  "LastShipmentDate",
					  						ObjectTableName =  "CustomerTenantAccess",
					  						FieldsDataType =  "DateTime",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "LastShipmentDate",
					  						ListPropertyPath =  "LastShipmentDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomerTenantAccess",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "LastShipmentDate",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LastShipmentDate",
					  						DefaultText =  "Last Shipment Date",
					  						ListFieldLable =  "LastShipmentDateListLable",
					  						ListLableDefaultText =  "Last Shipment Date",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Last Shipment Date",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdatedByUserName",
					  						OldFieldName =  "UpdatedByUserName",
					  						ObjectTableName =  "CustomerTenantAccess",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  60,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "UpdatedByUserName",
					  						ListPropertyPath =  "UpdatedByUserName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomerTenantAccess",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "UpdatedByUserName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "UpdatedByUserName",
					  						DefaultText =  "Updated By",
					  						ListFieldLable =  "UpdatedByUserNameListLable",
					  						ListLableDefaultText =  "Updated By User",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "UpdatedByUser",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "StockTypeCode",
					  						OldFieldName =  "StockTypeCode",
					  						ObjectTableName =  "CustomerTenantAccess",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "StockTypeCode",
					  						ListPropertyPath =  "StockTypeCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomerTenantAccess",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "StockTypeCode",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "StockTypeCode",
					  						DefaultText =  "Stock Type",
					  						FullLocalDefaultText =  "E-mail",
					  						ListFieldLable =  "StockTypeCodeListLable",
					  						ListLableDefaultText =  "Stock Type",
					  						ListLocalDefaultText =  "Contact us",
					  						HelpTextCode =  "Stock Type",
					  						HelpTextDefaultText =  "E-mail",
					  						HelpLocalDefaultText =  "E-mail",
					  						ShortFieldLable =  "StockTypeCode",
					  						ShortFieldLableDefaultText =  "E-mail",
					  						ShortLocalDefaultText =  "E-mail",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsPrivateLabelCustomer",
					  						OldFieldName =  "IsPrivateLabelCustomer",
					  						ObjectTableName =  "CustomerTenantAccess",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsPrivateLabelCustomer",
					  						ListPropertyPath =  "IsPrivateLabelCustomer",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomerTenantAccess",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "IsPrivateLabelCustomer",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsPrivateLabelCustomer",
					  						DefaultText =  "Is Private Label",
					  						ListFieldLable =  "IsPrivateLabelCustomerListLable",
					  						ListLableDefaultText =  "Is Private Label",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "IsPrivateLabelCustomer",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CustomCompanyName",
					  						OldFieldName =  "CustomCompanyName",
					  						ObjectTableName =  "CustomerTenantAccess",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  300,
					  						IsRequired =  false,
					  						CopyToDW =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "CustomCompanyName",
					  						ListPropertyPath =  "CustomCompanyName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "CustomerTenantAccess",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "CustomCompanyName",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CustomCompanyName",
					  						DefaultText =  "CompanyName",
					  						ListFieldLable =  "CustomCompanyNameListLable",
					  						ListLableDefaultText =  "CompanyName",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "CustomCompanyName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	    {  
	        FeatureRepository featureRepository = new FeatureRepository(0); 
            //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
            IWebFreightContext objectContext = WebFreightContext.GetContext(0);  
	        QueryGroup CustomerTenantAccessQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "CTAG", Name = "CustomerTenantAccesses" }, queryGroupRepository);
						QueryGroup CustomerTenantAccessQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "a5dd", Name = " Query Group" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable CustomerTenantAccessObjectTable = objectContext.ObjectTables.Where(d => d.Name == "CustomerTenantAccess" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> CustomerTenantAccessObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "CustomerTenantAccess").ToList();   

			   TextCode CustomerTenantAccessTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CustomerTenantAccess.Q.CustomerTenantAccesses", DefaultText = @"Importers Tenants",LocalDefaultText = null, ObjectTableId = CustomerTenantAccessObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature CustomerTenantAccessFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CUSTOMERTENANTACCESSES", ObjectTableId = CustomerTenantAccessObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomerTenantAccess.Features.CustomerTenantAccesses", NameTextCodeDefaultText = "CustomerTenantAccesses", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query CustomerTenantAccessesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = CustomerTenantAccessTextCode_0.Id, NameTextCodeCode = CustomerTenantAccessTextCode_0.Code, Code = "CustomerTenantAccesses",  QueryGroupCode = "CTAG", IndexOrder = 0, Tenant = 0, ObjectTableId = CustomerTenantAccessObjectTable.Id, QuerySection = "CustomerTenantAccess", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = CustomerTenantAccessFeature_0.Id, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn CustomerTenantAccessesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomerTenantAccessesQuery.Id, IndexOrder = 0, ObjectFieldId = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "CompanyName" && d.ObjectTableId == CustomerTenantAccessObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "CompanyName" && d.ObjectTableId == CustomerTenantAccessObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CustomerTenantAccessesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomerTenantAccessesQuery.Id, IndexOrder = 1, ObjectFieldId = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "CompanyVat" && d.ObjectTableId == CustomerTenantAccessObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "CompanyVat" && d.ObjectTableId == CustomerTenantAccessObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CustomerTenantAccessesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomerTenantAccessesQuery.Id, IndexOrder = 2, ObjectFieldId = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "ContactName" && d.ObjectTableId == CustomerTenantAccessObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "ContactName" && d.ObjectTableId == CustomerTenantAccessObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CustomerTenantAccessesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomerTenantAccessesQuery.Id, IndexOrder = 3, ObjectFieldId = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "ContactMobile" && d.ObjectTableId == CustomerTenantAccessObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "ContactMobile" && d.ObjectTableId == CustomerTenantAccessObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CustomerTenantAccessesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomerTenantAccessesQuery.Id, IndexOrder = 4, ObjectFieldId = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "IsPrivateLabelCustomer" && d.ObjectTableId == CustomerTenantAccessObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "IsPrivateLabelCustomer" && d.ObjectTableId == CustomerTenantAccessObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CustomerTenantAccessesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomerTenantAccessesQuery.Id, IndexOrder = 5, ObjectFieldId = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "StockTypeCode" && d.ObjectTableId == CustomerTenantAccessObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "StockTypeCode" && d.ObjectTableId == CustomerTenantAccessObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CustomerTenantAccessesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomerTenantAccessesQuery.Id, IndexOrder = 6, ObjectFieldId = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "CompanyEmail" && d.ObjectTableId == CustomerTenantAccessObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "CompanyEmail" && d.ObjectTableId == CustomerTenantAccessObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CustomerTenantAccessesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomerTenantAccessesQuery.Id, IndexOrder = 7, ObjectFieldId = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "ContactPhone" && d.ObjectTableId == CustomerTenantAccessObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "ContactPhone" && d.ObjectTableId == CustomerTenantAccessObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CustomerTenantAccessesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomerTenantAccessesQuery.Id, IndexOrder = 8, ObjectFieldId = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == CustomerTenantAccessObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "StatusName" && d.ObjectTableId == CustomerTenantAccessObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CustomerTenantAccessesQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomerTenantAccessesQuery.Id, IndexOrder = 9, ObjectFieldId = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "UpdatedByUserName" && d.ObjectTableId == CustomerTenantAccessObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "UpdatedByUserName" && d.ObjectTableId == CustomerTenantAccessObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CustomerTenantAccessesQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomerTenantAccessesQuery.Id, IndexOrder = 10, ObjectFieldId = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "RequestDateTime" && d.ObjectTableId == CustomerTenantAccessObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "RequestDateTime" && d.ObjectTableId == CustomerTenantAccessObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 120 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CustomerTenantAccessesQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomerTenantAccessesQuery.Id, IndexOrder = 11, ObjectFieldId = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "LastUpdateDate" && d.ObjectTableId == CustomerTenantAccessObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "LastUpdateDate" && d.ObjectTableId == CustomerTenantAccessObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CustomerTenantAccessesQueryColumn_12 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomerTenantAccessesQuery.Id, IndexOrder = 12, ObjectFieldId = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "LastShipmentDate" && d.ObjectTableId == CustomerTenantAccessObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "LastShipmentDate" && d.ObjectTableId == CustomerTenantAccessObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable CustomerTenantAccessObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "CustomerTenantAccess" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> CustomerTenantAccessObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "CustomerTenantAccess").ToList();
		       
	      

	         Screen CustomerTenantAccessHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "CustomerTenantAccess.HeaderScreen", Name = "Header Screen", ObjectTableId = CustomerTenantAccessObjectTable.Id, NumberOfColumns = 5, NumberOfRows = 2, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField CustomerTenantAccessCustomerTenantAccessHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "CustomCompanyName").FirstOrDefault().Id, ScreenId = CustomerTenantAccessHeaderScreenScreen0.Id, ObjectFieldCode = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "CustomCompanyName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomerTenantAccessCustomerTenantAccessHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "CompanyVat").FirstOrDefault().Id, ScreenId = CustomerTenantAccessHeaderScreenScreen0.Id, ObjectFieldCode = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "CompanyVat").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomerTenantAccessCustomerTenantAccessHeaderScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "ContactName").FirstOrDefault().Id, ScreenId = CustomerTenantAccessHeaderScreenScreen0.Id, ObjectFieldCode = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "ContactName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomerTenantAccessCustomerTenantAccessHeaderScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ObjectFieldId = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "CompanyEmail").FirstOrDefault().Id, ScreenId = CustomerTenantAccessHeaderScreenScreen0.Id, ObjectFieldCode = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "CompanyEmail").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomerTenantAccessCustomerTenantAccessHeaderScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "ContactPhone").FirstOrDefault().Id, ScreenId = CustomerTenantAccessHeaderScreenScreen0.Id, ObjectFieldCode = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "ContactPhone").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomerTenantAccessCustomerTenantAccessHeaderScreenScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 1, ObjectFieldId = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "ContactMobile").FirstOrDefault().Id, ScreenId = CustomerTenantAccessHeaderScreenScreen0.Id, ObjectFieldCode = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "ContactMobile").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomerTenantAccessCustomerTenantAccessHeaderScreenScreenField6 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ObjectFieldId = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "StatusName").FirstOrDefault().Id, ScreenId = CustomerTenantAccessHeaderScreenScreen0.Id, ObjectFieldCode = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "StatusName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomerTenantAccessCustomerTenantAccessHeaderScreenScreenField7 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 1, ObjectFieldId = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "RequestDateTime").FirstOrDefault().Id, ScreenId = CustomerTenantAccessHeaderScreenScreen0.Id, ObjectFieldCode = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "RequestDateTime").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomerTenantAccessCustomerTenantAccessHeaderScreenScreenField8 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 4, Row = 0, ObjectFieldId = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "CustomerTenant").FirstOrDefault().Id, ScreenId = CustomerTenantAccessHeaderScreenScreen0.Id, ObjectFieldCode = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "CustomerTenant").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomerTenantAccessCustomerTenantAccessHeaderScreenScreenField9 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 4, Row = 1, ObjectFieldId = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "StockTypeCode").FirstOrDefault().Id, ScreenId = CustomerTenantAccessHeaderScreenScreen0.Id, ObjectFieldCode = CustomerTenantAccessObjectFields.Where(d => d.FieldName == "StockTypeCode").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    CustomerTenantAccessObjectTable.HeaderScreenId = CustomerTenantAccessHeaderScreenScreen0.Id;
	   		  

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable CustomerTenantAccessObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "CustomerTenantAccess" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode CustomerTenantAccessRelatedCustomerTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CustomerTenantAccess.TH.RelatedCustomer", DefaultText = "Related Customer",LocalDefaultText = null, ObjectTableId = CustomerTenantAccessObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CustomerTenantAccessRelatedCustomerFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "RELATEDCUSTOMER", ObjectTableId = CustomerTenantAccessObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomerTenantAccess.Features.RelatedCustomers", NameTextCodeDefaultText = "Related Customer", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CARC",HtmlComponentName = "",HtmlComponentUrl = "./SharedLogistics/Components/RelatedCustomerComponent", FeatureId = CustomerTenantAccessRelatedCustomerFeature_TH0.Id, ControlPath = "Simplog.Infrastructure.Views.CustomerTenantAccessCard.RelatedCustomerControl", ObjectTableId = CustomerTenantAccessObjectTable.Id, TabNameTextCodeId = CustomerTenantAccessRelatedCustomerTextCode_TH0.Id, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable CustomerTenantAccessObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "CustomerTenantAccess" && d.Tenant == 0).FirstOrDefault(); 

		   Feature CustomerTenantAccessFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = true, ObjectTableId = CustomerTenantAccessObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomerTenantAccess.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature CustomerTenantAccessFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = true, ObjectTableId = CustomerTenantAccessObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomerTenantAccess.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature CustomerTenantAccessFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = true, ObjectTableId = CustomerTenantAccessObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomerTenantAccess.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature CustomerTenantAccessFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = true, ObjectTableId = CustomerTenantAccessObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomerTenantAccess.Features.PackageFeature", NameTextCodeDefaultText = "CustomerTenantAccess Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes); 

		   		   //--------------> Additional Features <--------------\\

		   Feature CustomerTenantAccessFeature_CustomerTenantAccess_Feature_EXCEL = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CustomerTenantAccess.Feature.EXCEL", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = CustomerTenantAccessObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomerTenantAccess.Features.EXCEL", NameTextCodeDefaultText = @"Download to Excel" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

   
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable CustomerTenantAccessObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "CustomerTenantAccess" && d.Tenant == 0).FirstOrDefault(); 
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
                ObjectTableId = CustomerTenantAccessObjectTable.Id,
				 
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
                ObjectTableId = CustomerTenantAccessObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
		   FeatureRepository featureRepository = new FeatureRepository(0); 
		   //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList(); 
		   ObjectTable CustomerTenantAccessObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "CustomerTenantAccess" && d.Tenant == 0).FirstOrDefault();       
    
			   Feature CustomerTenantAccessFeature_MB0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Deny", ObjectTableId = CustomerTenantAccessObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomerTenantAccess.Features.Deny", NameTextCodeDefaultText = "Deny", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

 

		   TextCodeRepository.SubmitChanges();
		   FeaturesRepository.SubmitChanges();
		   MenuButtonGroup CustomerTenantAccessMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
				{
					MenuButtonGroupType = "CustomerTenantAccessEdit",
					Name = "CustomerTenantAccessEditButtonsGroup",
					ObjectTableId = CustomerTenantAccessObjectTable.Id,
					Tenant = 0
				}, menuButtonGroupRepository, tenantMenuButtonGroups);        
   
			   MenuButton CustomerTenantAccessMenuButton0 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "Deny",
						Index = 100, 
						IsActive = true,
						LabelTextCodeCode = "CustomerTenantAccess.B.Deny",
						LabelTextCodeDefaultText = "Deny",
						Tenant = 0,
						MenuButtonGroupId = CustomerTenantAccessMenuButtonGroup.Id,
						ObjectTableId = CustomerTenantAccessObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = CustomerTenantAccessFeature_MB0.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);

	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable CustomerTenantAccessObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "CustomerTenantAccess" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode CustomerTenantAccessTextCode_CustomerTenantAccessORelatedCustomersAddRelatedCustomer = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CustomerTenantAccess.O.RelatedCustomers.AddRelatedCustomer", DefaultText = "Add Card",LocalDefaultText = null, ObjectTableId = CustomerTenantAccessObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomerTenantAccessTextCode_CustomerTenantAccessORelatedCustomersEditRelatedCustomer = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CustomerTenantAccess.O.RelatedCustomers.EditRelatedCustomer", DefaultText = "Edit Card",LocalDefaultText = null, ObjectTableId = CustomerTenantAccessObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomerTenantAccessTextCode_CustomerTenantAccessORelatedCustomersCustomerCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CustomerTenantAccess.O.RelatedCustomers.CustomerCode", DefaultText = "Customer Code",LocalDefaultText = null, ObjectTableId = CustomerTenantAccessObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomerTenantAccessTextCode_CustomerTenantAccessORelatedCustomersCustomerName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CustomerTenantAccess.O.RelatedCustomers.CustomerName", DefaultText = "Customer Name",LocalDefaultText = null, ObjectTableId = CustomerTenantAccessObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomerTenantAccessTextCode_CustomerTenantAccessORelatedCustomersCreateDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CustomerTenantAccess.O.RelatedCustomers.CreateDate", DefaultText = "Create date",LocalDefaultText = null, ObjectTableId = CustomerTenantAccessObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomerTenantAccessTextCode_CustomerTenantAccessORelatedCustomersCreatedBy = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CustomerTenantAccess.O.RelatedCustomers.CreatedBy", DefaultText = "Created by",LocalDefaultText = null, ObjectTableId = CustomerTenantAccessObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomerTenantAccessTextCode_CustomerTenantAccessORelatedCustomersLastShipmentDateInQueue = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CustomerTenantAccess.O.RelatedCustomers.LastShipmentDateInQueue", DefaultText = "Last Shipment Date",LocalDefaultText = null, ObjectTableId = CustomerTenantAccessObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomerTenantAccessTextCode_CustomerTenantAccessORelatedCustomersRelatedCustomers = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CustomerTenantAccess.O.RelatedCustomers.RelatedCustomers", DefaultText = "Related Customers",LocalDefaultText = null, ObjectTableId = CustomerTenantAccessObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomerTenantAccessTextCode_CustomerTenantAccessORelatedCustomersUpdateDateTime = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CustomerTenantAccess.O.RelatedCustomers.UpdateDateTime", DefaultText = "Update date",LocalDefaultText = null, ObjectTableId = CustomerTenantAccessObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomerTenantAccessTextCode_CustomerTenantAccessORelatedCustomersStatusType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CustomerTenantAccess.O.RelatedCustomers.StatusType", DefaultText = "Status",LocalDefaultText = null, ObjectTableId = CustomerTenantAccessObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomerTenantAccessTextCode_CustomerTenantAccessORelatedCustomersHybridStartDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CustomerTenantAccess.O.RelatedCustomers.HybridStartDate", DefaultText = "Hybrid Start Date",LocalDefaultText = null, ObjectTableId = CustomerTenantAccessObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomerTenantAccessTextCode_CustomerTenantAccessBRelatedCustomersAddRelatedCustomer = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CustomerTenantAccess.B.RelatedCustomers.AddRelatedCustomer", DefaultText = "Add Related Customer",LocalDefaultText = null, ObjectTableId = CustomerTenantAccessObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 