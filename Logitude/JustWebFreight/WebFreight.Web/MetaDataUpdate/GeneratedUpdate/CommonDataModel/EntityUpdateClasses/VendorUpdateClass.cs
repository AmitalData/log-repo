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
namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.CommonDataModel.EntityUpdateClasses
{
   public class VendorUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "Vendor",
			      				    DBTableName =  "Vendors",
			      				    ObjectTableSingular =  "Vendor",
			      				    ObjectTablePlural =  "Vendors",
			      				    DefaultText =  "Vendor",
			      				    Name =  "Vendors",
			      				    IsNewWizard =  true,
			      				    NewWizardControlName =  "Simplog.FreightLib.NewVendorCommand",
			      				    HasCustomFilter =  false,
			      				    KeyPropertyPath =  "Id",
			      				    AutoCompleteSearchWindow =  false,
			      				    IsClosed =  false,
			      				    CacheOnClient =  false,
			      				    EditableFromAutoCompleteWindow =  true,
			      				    HasCounter =  false,
			      				    EnableEditFromLOV =  false,
			      				    EnableAddFromLOV =  false,
			      				    IsRestrictable =  false,
			      				    IsMain =  true,
			      				    IsAutoComplete =  false,
			      				    CustomFieldsCount =  0,
			      				    HasCustomFields =  false,
			      				    InActive =  false,
			      				    SearchFields =  "Vendor,Vendors,Simplog.FreightLib.NewVendorCommand,Id,",
			      				    IsSaveButtonVisible =  true,
			      				    EnableSecurity =  true,
			      				    ObjectTableTypeCode =  "MD",
			      				    IsComposition =  false,
			      				    MaxNumberOfCustomFields =  0,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    IsEditable =  true,
			      				    AllowedForComputingPartners =  false,
			      				    DisableSearchBox =  false,
			      				    ClientModuleName =  "Common",
			      				    NewWizardComponentPath =  "./CommonModules/CommonPartners/Components/NewEntity/NewVendorComponent",
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasMenuButtons =  false,
			      				    HasFiltersMenu =  false,
			      				    AllowedInQueues =  false,
			      				    Code =  "VNDR",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Website",
					  						ObjectTableName =  "Vendor",
					  						FieldsDataType =  "Text",
					  						Code =  "Website",
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
					  						ValidForQuerySection1 =  "Vendor",
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
					  						DefaultText =  "Website",
					  						ListFieldLable =  "WebsiteListLable",
					  						ListLableDefaultText =  "Website",
					  						HelpTextCode =  "Website",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Code",
					  						ObjectTableName =  "Vendor",
					  						FieldsDataType =  "Text",
					  						Code =  "Code",
					  						MaxLength =  15,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  true,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  true,
					  						DisplayInSearchWindowFilters =  true,
					  						PMPropertyPath =  "Code",
					  						ListPropertyPath =  "Code",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  true,
					  						UniqueField =  true,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  1,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Vendor",
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
					  						IsRequired =  true,
					  						FullFieldLable =  "Code",
					  						DefaultText =  "Code",
					  						ListFieldLable =  "CodeListLable",
					  						ListLableDefaultText =  "Code",
					  						HelpTextCode =  "Code",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "EnglishName",
					  						ObjectTableName =  "Vendor",
					  						FieldsDataType =  "Text",
					  						Code =  "EnglishName",
					  						MaxLength =  70,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  true,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  true,
					  						DisplayInSearchWindowFilters =  true,
					  						PMPropertyPath =  "EnglishName",
					  						ListPropertyPath =  "EnglishName",
					  						DisplayInLookUpIndex =  1,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  1,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Vendor",
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
					  						IsRequired =  true,
					  						FullFieldLable =  "English Name",
					  						DefaultText =  "Name",
					  						ListFieldLable =  "EnglishNameListLable",
					  						ListLableDefaultText =  "Name",
					  						HelpTextCode =  "English Name",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LocalName",
					  						ObjectTableName =  "Vendor",
					  						FieldsDataType =  "nText",
					  						Code =  "LocalName",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "LocalName",
					  						ListPropertyPath =  "LocalName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Vendor",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
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
					  						FullFieldLable =  "LocalName",
					  						DefaultText =  "Local Name",
					  						ListFieldLable =  "LocalNameListLable",
					  						ListLableDefaultText =  "Local Name",
					  						HelpTextCode =  "LocalName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ComputedLocalName",
					  						ObjectTableName =  "Vendor",
					  						FieldsDataType =  "nText",
					  						Code =  "ComputedLocalName",
					  						MaxLength =  100,
					  						IsCustom =  false,
					  						MinLength =  0,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "ComputedLocalName",
					  						ListPropertyPath =  "ComputedLocalName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Vendor",
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
					  						FullFieldLable =  "ComputedLocalName",
					  						DefaultText =  "Local Name",
					  						HelpTextCode =  "ComputedLocalName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InActive",
					  						ObjectTableName =  "Vendor",
					  						FieldsDataType =  "Boolean",
					  						Code =  "InActive",
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
					  						PMPropertyPath =  "InActive",
					  						ListPropertyPath =  "InActive",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Vendor",
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
					  						FullFieldLable =  "InActive",
					  						DefaultText =  "Inactive Vendor",
					  						ListFieldLable =  "InActiveListLable",
					  						ListLableDefaultText =  "Inactive",
					  						HelpTextCode =  "InActive",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "VatNumber",
					  						ObjectTableName =  "Vendor",
					  						FieldsDataType =  "Text",
					  						Code =  "VatNumber",
					  						MaxLength =  20,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						ValidForQuerySection1 =  "Vendor",
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
					  						DefaultText =  "VAT No.",
					  						ListFieldLable =  "VatNumberListLable",
					  						ListLableDefaultText =  "VAT Number",
					  						HelpTextCode =  "VatNumber",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PaymentTermId",
					  						ObjectTableName =  "Vendor",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "PaymentTerm",
					  						Code =  "PaymentTermId",
					  						MaxLength =  15,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "PaymentTermId",
					  						ListPropertyPath =  "PaymentTermId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Vendor",
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
					  						FullFieldLable =  "PaymentTermId",
					  						DefaultText =  "Payment Term",
					  						HelpTextCode =  "PaymentTermId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PaymentTermEnglishName",
					  						ObjectTableName =  "Vendor",
					  						FieldsDataType =  "Text",
					  						LookUpTableName =  "PaymentTerm",
					  						Code =  "PaymentTermEnglishName",
					  						MaxLength =  15,
					  						IsCustom =  false,
					  						MinLength =  0,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "Card.PaymentTerm.EnglishName",
					  						ListPropertyPath =  "PaymentTermEnglishName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Vendor",
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
					  						FullFieldLable =  "PaymentTermEnglishName",
					  						DefaultText =  "Payment Term Name",
					  						ListFieldLable =  "PaymentTermListLable",
					  						ListLableDefaultText =  "Payment Term",
					  						HelpTextCode =  "PaymentTermEnglishName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Notes",
					  						ObjectTableName =  "Vendor",
					  						FieldsDataType =  "Text",
					  						Code =  "Notes",
					  						MaxLength =  2000,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  true,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "Notes",
					  						ListPropertyPath =  "Notes",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Vendor",
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
					  						DataTemplateName =  "NotesDataTemplate",
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "Notes",
					  						DefaultText =  "Notes",
					  						ListFieldLable =  "NotesListLable",
					  						ListLableDefaultText =  "Notes",
					  						HelpTextCode =  "Notes",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InvoiceCurrencyId",
					  						ObjectTableName =  "Vendor",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Currency",
					  						Code =  "InvoiceCurrencyId",
					  						MaxLength =  15,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "InvoiceCurrencyId",
					  						ListPropertyPath =  "InvoiceCurrencyId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Vendor",
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
					  						FullFieldLable =  "InvoiceCurrencyId",
					  						DefaultText =  "Invoice Currency",
					  						HelpTextCode =  "InvoiceCurrencyId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "VatTypeId",
					  						ObjectTableName =  "Vendor",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "VatType",
					  						Code =  "VatTypeId",
					  						MaxLength =  15,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  true,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "VatTypeId",
					  						ListPropertyPath =  "VatTypeId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Vendor",
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
					  						FullFieldLable =  "VatTypeId",
					  						DefaultText =  "VAT Type",
					  						HelpTextCode =  "VatTypeId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BankName",
					  						ObjectTableName =  "Vendor",
					  						FieldsDataType =  "Text",
					  						Code =  "BankName",
					  						MaxLength =  40,
					  						IsCustom =  false,
					  						MinLength =  0,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "BankName",
					  						ListPropertyPath =  "BankName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Vendor",
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
					  						FullFieldLable =  "BankName",
					  						DefaultText =  "Bank Name",
					  						HelpTextCode =  "BankName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BankAddress",
					  						ObjectTableName =  "Vendor",
					  						FieldsDataType =  "Text",
					  						Code =  "BankAddress",
					  						MaxLength =  100,
					  						IsCustom =  false,
					  						MinLength =  0,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "BankAddress",
					  						ListPropertyPath =  "BankAddress",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Vendor",
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
					  						FullFieldLable =  "BankAddress",
					  						DefaultText =  "Bank Address",
					  						HelpTextCode =  "BankAddress",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Swift",
					  						ObjectTableName =  "Vendor",
					  						FieldsDataType =  "Text",
					  						Code =  "Swift",
					  						MaxLength =  20,
					  						IsCustom =  false,
					  						MinLength =  0,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "Swift",
					  						ListPropertyPath =  "Swift",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Vendor",
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
					  						FullFieldLable =  "Swift",
					  						DefaultText =  "Swift",
					  						HelpTextCode =  "Swift",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AccountNumber",
					  						ObjectTableName =  "Vendor",
					  						FieldsDataType =  "Text",
					  						Code =  "AccountNumber",
					  						MaxLength =  25,
					  						IsCustom =  false,
					  						MinLength =  0,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "AccountNumber",
					  						ListPropertyPath =  "AccountNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Vendor",
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
					  						FullFieldLable =  "AccountNumber",
					  						DefaultText =  "Bank Account Number",
					  						HelpTextCode =  "AccountNumber",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IBANNumber",
					  						ObjectTableName =  "Vendor",
					  						FieldsDataType =  "Text",
					  						Code =  "IBANNumber",
					  						MaxLength =  30,
					  						IsCustom =  false,
					  						MinLength =  0,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "IBANNumber",
					  						ListPropertyPath =  "IBANNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Vendor",
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
					  						FullFieldLable =  "IBANNumber",
					  						DefaultText =  "IBAN No.",
					  						HelpTextCode =  "IBANNumber",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "EnableConsolidationInvoices",
					  						ObjectTableName =  "Vendor",
					  						FieldsDataType =  "Boolean",
					  						Code =  "EnableConsolidationInvoices",
					  						MaxLength =  1,
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
					  						PMPropertyPath =  "EnableConsolidationInvoices",
					  						ListPropertyPath =  "EnableConsolidationInvoices",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Vendor",
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
					  						FullFieldLable =  "EnableConsolidationInvoices",
					  						DefaultText =  "Consolidated Inv.",
					  						HelpTextCode =  "EnableConsolidationInvoices",
					  						HelpTextDefaultText =  "This client will receive one Invoice for multiple Shipments once a period",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IRSPlace",
					  						ObjectTableName =  "Vendor",
					  						FieldsDataType =  "Text",
					  						Code =  "IRSPlace",
					  						MaxLength =  60,
					  						IsCustom =  false,
					  						MinLength =  0,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "IRSPlace",
					  						ListPropertyPath =  "IRSPlace",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Vendor",
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
					  						FullFieldLable =  "IRSPlace",
					  						DefaultText =  "IRS Place",
					  						HelpTextCode =  "IRSPlace",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IRSNumber",
					  						ObjectTableName =  "Vendor",
					  						FieldsDataType =  "Text",
					  						Code =  "IRSNumber",
					  						MaxLength =  60,
					  						IsCustom =  false,
					  						MinLength =  0,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "IRSNumber",
					  						ListPropertyPath =  "IRSNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Vendor",
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
					  						FullFieldLable =  "IRSNumber",
					  						DefaultText =  "IRS Number",
					  						HelpTextCode =  "IRSNumber",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SATForeignRFC",
					  						ObjectTableName =  "Vendor",
					  						FieldsDataType =  "Text",
					  						Code =  "SATForeignRFC",
					  						MaxLength =  20,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "SATForeignRFC",
					  						ListPropertyPath =  "SATForeignRFC",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Vendor",
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
					  						FullFieldLable =  "SATForeignRFC",
					  						DefaultText =  "SAT Foreign RFC",
					  						HelpTextCode =  "SATForeignRFC",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ExternalId2",
					  						ObjectTableName =  "Vendor",
					  						FieldsDataType =  "Text",
					  						Code =  "ExternalId2",
					  						MaxLength =  25,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "ExternalId2",
					  						ListPropertyPath =  "ExternalId2",
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
					  						FullFieldLable =  "ExternalId2",
					  						DefaultText =  "External ID2",
					  						HelpTextCode =  "ExternalId2",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "MetodoPagoCode",
					  						ObjectTableName =  "Vendor",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "MetodoPago",
					  						Code =  "MetodoPagoCode",
					  						MaxLength =  3,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "MetodoPagoCode",
					  						ListPropertyPath =  "MetodoPagoCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Vendor",
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
					  						FullFieldLable =  "MetodoPagoCode",
					  						DefaultText =  "Metodo Pago",
					  						ListFieldLable =  "MetodoPagoCodeListLable",
					  						ListLableDefaultText =  "Metodo Pago",
					  						HelpTextCode =  "MetodoPagoCode",
					  						HelpTextDefaultText =  "Way to Pay:\n-Pago en una sola exhibición (PUE): payment closed at once in one single payment type performed prior to the issuance of the invoice\n-Pago en parcialidades o diferido (PPD): partial payment or deferred performed after the issuance of the invoice",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UsoCFDICode",
					  						ObjectTableName =  "Vendor",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "UsoCFDI",
					  						Code =  "UsoCFDICode",
					  						MaxLength =  3,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "UsoCFDICode",
					  						ListPropertyPath =  "UsoCFDICode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Vendor",
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
					  						FullFieldLable =  "UsoCFDICode",
					  						DefaultText =  "UsoCFDI",
					  						ListFieldLable =  "UsoCFDIListLable",
					  						ListLableDefaultText =  "UsoCFDI",
					  						HelpTextCode =  "UsoCFDICode",
					  						HelpTextDefaultText =  "Use of Digital Fiscal Receipt through Internet",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ReceivablesAccountingCard",
					  						ObjectTableName =  "Vendor",
					  						FieldsDataType =  "Text",
					  						Code =  "ReceivablesAccountingCard",
					  						MaxLength =  25,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "ReceivablesAccountingCard",
					  						ListPropertyPath =  "ReceivablesAccountingCard",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Vendor",
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
					  						FullFieldLable =  "ReceivablesAccountingCard",
					  						DefaultText =  "Receivables External ID",
					  						ListFieldLable =  "ReceivablesAccountingCardListLable",
					  						ListLableDefaultText =  "Receivables External ID",
					  						HelpTextCode =  "ReceivablesAccountingCard",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PayablesAccountingCard",
					  						ObjectTableName =  "Vendor",
					  						FieldsDataType =  "Text",
					  						Code =  "PayablesAccountingCard",
					  						MaxLength =  25,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "PayablesAccountingCard",
					  						ListPropertyPath =  "PayablesAccountingCard",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Vendor",
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
					  						FullFieldLable =  "PayablesAccountingCard",
					  						DefaultText =  "Payables External ID",
					  						ListFieldLable =  "PayablesAccountingCardListLable",
					  						ListLableDefaultText =  "Payables External ID",
					  						HelpTextCode =  "PayablesAccountingCard",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ExternalAccountingBusinessArea",
					  						ObjectTableName =  "Vendor",
					  						FieldsDataType =  "Text",
					  						Code =  "ExternalAccountingBusinessArea",
					  						MaxLength =  25,
					  						IsCustom =  false,
					  						MinLength =  0,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "ExternalAccountingBusinessArea",
					  						ListPropertyPath =  "ExternalAccountingBusinessArea",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Vendor",
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
					  						FullFieldLable =  "ExternalAccountingBusinessArea",
					  						DefaultText =  "Business Area",
					  						HelpTextCode =  "ExternalAccountingBusinessArea",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PaymentMethodCode",
					  						ObjectTableName =  "Vendor",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "SATPaymentMethod",
					  						Code =  "PaymentMethodCode",
					  						MaxLength =  2,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "PaymentMethodCode",
					  						ListPropertyPath =  "PaymentMethodCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Vendor",
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
					  						FullFieldLable =  "PaymentMethodCode",
					  						DefaultText =  "Forma Pago",
					  						HelpTextCode =  "PaymentMethodCode",
					  						HelpTextDefaultText =  "Payment Method",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PrimaryContactName",
					  						ObjectTableName =  "Vendor",
					  						FieldsDataType =  "Text",
					  						Code =  "PrimaryContactName",
					  						MaxLength =  60,
					  						IsCustom =  false,
					  						MinLength =  0,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "PrimaryContactName",
					  						ListPropertyPath =  "PrimaryContactName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Vendor",
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
					  						FullFieldLable =  "PrimaryContactName",
					  						DefaultText =  "Primary Contact Name",
					  						ListFieldLable =  "PrimaryContactNameLabel",
					  						ListLableDefaultText =  "Primary Contact",
					  						HelpTextCode =  "PrimaryContactName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PrimaryContactEmail",
					  						ObjectTableName =  "Vendor",
					  						FieldsDataType =  "Text",
					  						Code =  "PrimaryContactEmail",
					  						MaxLength =  70,
					  						IsCustom =  false,
					  						MinLength =  0,
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
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "PrimaryContactEmail",
					  						ListPropertyPath =  "PrimaryContactEmail",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Vendor",
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
					  						FullFieldLable =  "PrimaryContactEmail",
					  						DefaultText =  "Primary Contact E-mail",
					  						ListFieldLable =  "PrimaryContactEmailLabel",
					  						ListLableDefaultText =  "Primary Contact E-mail",
					  						HelpTextCode =  "PrimaryContactEmail",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PrimaryContactPhone",
					  						ObjectTableName =  "Vendor",
					  						FieldsDataType =  "Text",
					  						Code =  "PrimaryContactPhone",
					  						MaxLength =  25,
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
					  						PMPropertyPath =  "PrimaryContactPhone",
					  						ListPropertyPath =  "PrimaryContactPhone",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Vendor",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
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
					  						FullFieldLable =  "PrimaryContactPhone",
					  						DefaultText =  "Primary Contact Phone",
					  						HelpTextCode =  "PrimaryContactPhone",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	    {  
	        FeatureRepository featureRepository = new FeatureRepository(0); 
            List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
            IWebFreightContext objectContext = WebFreightContext.GetContext(0);  
	        QueryGroup VendorQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "VNDR", Name = "Vendors" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable VendorObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Vendor" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> VendorObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Vendor").ToList();   

			   TextCode VendorTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Vendor.Q.Vendors", DefaultText = @"Vendors",LocalDefaultText = null, ObjectTableId = VendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature VendorFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "VENDORS", ObjectTableId = VendorObjectTable.Id, Tenant = 0, NameTextCodeCode = "Vendor.Features.Vendors", NameTextCodeDefaultText = "Vendors", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query VendorsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = VendorTextCode_0.Id, Code = "Vendors",  QueryGroupCode = "VNDR", IndexOrder = 0, Tenant = 0, ObjectTableId = VendorObjectTable.Id, QuerySection = "Vendor", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = VendorFeature_0.Id, DefaultSortName = "Code", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
	
			 QueryColumn VendorsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = VendorsQuery.Id, IndexOrder = 0, ObjectFieldId = VendorObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == VendorObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn VendorsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = VendorsQuery.Id, IndexOrder = 1, ObjectFieldId = VendorObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == VendorObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn VendorsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = VendorsQuery.Id, IndexOrder = 2, ObjectFieldId = VendorObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == VendorObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn VendorsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = VendorsQuery.Id, IndexOrder = 3, ObjectFieldId = VendorObjectFields.Where(d => d.FieldName == "VatNumber" && d.ObjectTableId == VendorObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn VendorsQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = VendorsQuery.Id, IndexOrder = 4, ObjectFieldId = VendorObjectFields.Where(d => d.FieldName == "ReceivablesAccountingCard" && d.ObjectTableId == VendorObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn VendorsQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = VendorsQuery.Id, IndexOrder = 5, ObjectFieldId = VendorObjectFields.Where(d => d.FieldName == "PaymentTermEnglishName" && d.ObjectTableId == VendorObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn VendorsQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = VendorsQuery.Id, IndexOrder = 6, ObjectFieldId = VendorObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTableId == VendorObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn VendorsQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = VendorsQuery.Id, IndexOrder = 7, ObjectFieldId = VendorObjectFields.Where(d => d.FieldName == "Notes" && d.ObjectTableId == VendorObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable VendorObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Vendor" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> VendorObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Vendor").ToList();
		       
	      

	         Screen VendorHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Customs.Vendor.HeaderScreen", Name = "Header Screen", ObjectTableId = VendorObjectTable.Id, NumberOfColumns = 1, NumberOfRows = 2, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField VendorCustomsVendorHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = VendorObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().Id, ScreenId = VendorHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField VendorCustomsVendorHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = VendorObjectFields.Where(d => d.FieldName == "EnglishName").FirstOrDefault().Id, ScreenId = VendorHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    VendorObjectTable.HeaderScreenId = VendorHeaderScreenScreen0.Id;
	   		  
	      

	         Screen VendorGeneralTabScreenScreen1 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Customs.Vendor.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = VendorObjectTable.Id, NumberOfColumns = 1, NumberOfRows = 6, IsReadOnly = false }, screensRepository, tenantScreens);
      
            ScreenField VendorCustomsVendorGeneralTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = VendorObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().Id, ScreenId = VendorGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField VendorCustomsVendorGeneralTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = VendorObjectFields.Where(d => d.FieldName == "EnglishName").FirstOrDefault().Id, ScreenId = VendorGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField VendorCustomsVendorGeneralTabScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ObjectFieldId = VendorObjectFields.Where(d => d.FieldName == "LocalName").FirstOrDefault().Id, ScreenId = VendorGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField VendorCustomsVendorGeneralTabScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 3, ObjectFieldId = VendorObjectFields.Where(d => d.FieldName == "Website").FirstOrDefault().Id, ScreenId = VendorGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField VendorCustomsVendorGeneralTabScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 4, ObjectFieldId = VendorObjectFields.Where(d => d.FieldName == "InActive").FirstOrDefault().Id, ScreenId = VendorGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField VendorCustomsVendorGeneralTabScreenScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 5, ObjectFieldId = VendorObjectFields.Where(d => d.FieldName == "Notes").FirstOrDefault().Id, ScreenId = VendorGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
           
	      

	         Screen VendorBillingTabScreenScreen2 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Vendor.BillingTabScreen", Name = "Billing Tab Screen", ObjectTableId = VendorObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 6, IsReadOnly = false }, screensRepository, tenantScreens);
      
            ScreenField VendorVendorBillingTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = VendorObjectFields.Where(d => d.FieldName == "VatNumber").FirstOrDefault().Id, ScreenId = VendorBillingTabScreenScreen2.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField VendorVendorBillingTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = VendorObjectFields.Where(d => d.FieldName == "PaymentTermId").FirstOrDefault().Id, ScreenId = VendorBillingTabScreenScreen2.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField VendorVendorBillingTabScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ObjectFieldId = VendorObjectFields.Where(d => d.FieldName == "InvoiceCurrencyId").FirstOrDefault().Id, ScreenId = VendorBillingTabScreenScreen2.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField VendorVendorBillingTabScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 3, ObjectFieldId = VendorObjectFields.Where(d => d.FieldName == "VatTypeId").FirstOrDefault().Id, ScreenId = VendorBillingTabScreenScreen2.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField VendorVendorBillingTabScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 5, ObjectFieldId = VendorObjectFields.Where(d => d.FieldName == "EnableConsolidationInvoices").FirstOrDefault().Id, ScreenId = VendorBillingTabScreenScreen2.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField VendorVendorBillingTabScreenScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = VendorObjectFields.Where(d => d.FieldName == "BankName").FirstOrDefault().Id, ScreenId = VendorBillingTabScreenScreen2.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField VendorVendorBillingTabScreenScreenField6 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ObjectFieldId = VendorObjectFields.Where(d => d.FieldName == "BankAddress").FirstOrDefault().Id, ScreenId = VendorBillingTabScreenScreen2.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField VendorVendorBillingTabScreenScreenField7 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 2, ObjectFieldId = VendorObjectFields.Where(d => d.FieldName == "Swift").FirstOrDefault().Id, ScreenId = VendorBillingTabScreenScreen2.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField VendorVendorBillingTabScreenScreenField8 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 3, ObjectFieldId = VendorObjectFields.Where(d => d.FieldName == "AccountNumber").FirstOrDefault().Id, ScreenId = VendorBillingTabScreenScreen2.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField VendorVendorBillingTabScreenScreenField9 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 4, ObjectFieldId = VendorObjectFields.Where(d => d.FieldName == "IBANNumber").FirstOrDefault().Id, ScreenId = VendorBillingTabScreenScreen2.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
           
	      

	         Screen VendorAccountingTabScreenScreen3 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Vendor.AccountingTabScreen", Name = "Accounting Tab Screen", ObjectTableId = VendorObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 2, IsReadOnly = false }, screensRepository, tenantScreens);
      
            ScreenField VendorVendorAccountingTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = VendorObjectFields.Where(d => d.FieldName == "ReceivablesAccountingCard").FirstOrDefault().Id, ScreenId = VendorAccountingTabScreenScreen3.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField VendorVendorAccountingTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = VendorObjectFields.Where(d => d.FieldName == "PayablesAccountingCard").FirstOrDefault().Id, ScreenId = VendorAccountingTabScreenScreen3.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
           
	      

	         Screen VendorHeaderScreenScreen4 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Vendor.HeaderScreen", Name = "Header Screen", ObjectTableId = VendorObjectTable.Id, NumberOfColumns = 1, NumberOfRows = 2, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField VendorVendorHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = VendorObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().Id, ScreenId = VendorHeaderScreenScreen4.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField VendorVendorHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = VendorObjectFields.Where(d => d.FieldName == "EnglishName").FirstOrDefault().Id, ScreenId = VendorHeaderScreenScreen4.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    VendorObjectTable.HeaderScreenId = VendorHeaderScreenScreen4.Id;
	   		  
	      

	         Screen VendorGeneralTabScreenScreen5 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Vendor.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = VendorObjectTable.Id, NumberOfColumns = 1, NumberOfRows = 6, IsReadOnly = false }, screensRepository, tenantScreens);
      
            ScreenField VendorVendorGeneralTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = VendorObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().Id, ScreenId = VendorGeneralTabScreenScreen5.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField VendorVendorGeneralTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = VendorObjectFields.Where(d => d.FieldName == "EnglishName").FirstOrDefault().Id, ScreenId = VendorGeneralTabScreenScreen5.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField VendorVendorGeneralTabScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ObjectFieldId = VendorObjectFields.Where(d => d.FieldName == "LocalName").FirstOrDefault().Id, ScreenId = VendorGeneralTabScreenScreen5.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField VendorVendorGeneralTabScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 3, ObjectFieldId = VendorObjectFields.Where(d => d.FieldName == "Website").FirstOrDefault().Id, ScreenId = VendorGeneralTabScreenScreen5.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField VendorVendorGeneralTabScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 4, ObjectFieldId = VendorObjectFields.Where(d => d.FieldName == "InActive").FirstOrDefault().Id, ScreenId = VendorGeneralTabScreenScreen5.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField VendorVendorGeneralTabScreenScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 5, ObjectFieldId = VendorObjectFields.Where(d => d.FieldName == "Notes").FirstOrDefault().Id, ScreenId = VendorGeneralTabScreenScreen5.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
           

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {    
			 ObjectTable VendorObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Vendor" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode VendorGeneralTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Vendor.TH.General", DefaultText = "General",LocalDefaultText = null, ObjectTableId = VendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature VendorGeneralFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GENERAL", ObjectTableId = VendorObjectTable.Id, Tenant = 0, NameTextCodeCode = "Vendor.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode VendorBillingTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Vendor.TH.Billing", DefaultText = "Billing",LocalDefaultText = null, ObjectTableId = VendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature VendorBillingFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BILLING", ObjectTableId = VendorObjectTable.Id, Tenant = 0, NameTextCodeCode = "Vendor.Features.Billing", NameTextCodeDefaultText = "Billing", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode VendorAccountingTextCode_TH2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Vendor.TH.Accounting", DefaultText = "Accounting",LocalDefaultText = null, ObjectTableId = VendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature VendorAccountingFeature_TH2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACCOUNTINGTRANSFER", ObjectTableId = VendorObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.AccountingTransfer", NameTextCodeDefaultText = "Accounting Transfer", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode VendorAddressesTextCode_TH3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Vendor.TH.Addresses", DefaultText = "Addresses",LocalDefaultText = null, ObjectTableId = VendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature VendorAddressesFeature_TH3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ADDRESSES", ObjectTableId = VendorObjectTable.Id, Tenant = 0, NameTextCodeCode = "Vendor.Features.Addresses", NameTextCodeDefaultText = "Addresses", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode VendorContactsTextCode_TH4 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Vendor.TH.Contacts", DefaultText = "Contacts",LocalDefaultText = null, ObjectTableId = VendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature VendorContactsFeature_TH4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CONTACTS", ObjectTableId = VendorObjectTable.Id, Tenant = 0, NameTextCodeCode = "Vendor.Features.Contacts", NameTextCodeDefaultText = "Contacts", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode VendorEventsTextCode_TH5 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Vendor.TH.Events", DefaultText = "Events",LocalDefaultText = null, ObjectTableId = VendorObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature VendorEventsFeature_TH5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EVENTS", ObjectTableId = VendorObjectTable.Id, Tenant = 0, NameTextCodeCode = "Vendor.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "VDGC",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = tenantFeatures.Where(d => d.Code == "GENERAL" && d.ObjectTableId == VendorObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.Infrastructure.GeneralControls.GeneralTabControl", ObjectTableId = VendorObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Vendor.TH.General" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "VDBL",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = tenantFeatures.Where(d => d.Code == "BILLING" && d.ObjectTableId == VendorObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.Infrastructure.GeneralControls.BillingTabControl", ObjectTableId = VendorObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Vendor.TH.Billing" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "VDAC",HtmlComponentName = "",HtmlComponentUrl = "./Common/Components/AccountingTab/AccountingTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "ACCOUNTINGTRANSFER" && d.ObjectTableId == VendorObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.FreightLib.Views.PartnersTabs.PartnersAccountingTab.VendorAccountingTabControl", ObjectTableId = VendorObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Vendor.TH.Accounting" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 2 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "VDAD",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = tenantFeatures.Where(d => d.Code == "ADDRESSES" && d.ObjectTableId == VendorObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.FreightLib.Views.PartnersTabs.PartnerAddressesTab", ObjectTableId = VendorObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Vendor.TH.Addresses" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 3 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "VDCO",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = tenantFeatures.Where(d => d.Code == "CONTACTS" && d.ObjectTableId == VendorObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.FreightLib.Views.PartnersTabs.PartnerContactsTab", ObjectTableId = VendorObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Vendor.TH.Contacts" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 4 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "VDEV",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = tenantFeatures.Where(d => d.Code == "EVENTS" && d.ObjectTableId == VendorObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = VendorObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Vendor.TH.Events" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 5 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable VendorObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Vendor" && d.Tenant == 0).FirstOrDefault(); 
		   Feature VendorFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = VendorObjectTable.Id, Tenant = 0, NameTextCodeCode = "Vendor.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature VendorFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = VendorObjectTable.Id, Tenant = 0, NameTextCodeCode = "Vendor.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature VendorFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = VendorObjectTable.Id, Tenant = 0, NameTextCodeCode = "Vendor.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature VendorFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = VendorObjectTable.Id, Tenant = 0, NameTextCodeCode = "Vendor.Features.PackageFeature", NameTextCodeDefaultText = "Vendor Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);    
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable VendorObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Vendor" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "UPVD",
                ShortView =  false,
                IsManualEntry =  false,
                LocalName =  "Vendor Updated",
                EnglishName =  "Vendor Updated",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = VendorObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CRVD",
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
                ObjectTableId = VendorObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
	    }     

   }
    
}
	 