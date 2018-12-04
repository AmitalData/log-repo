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
   public class CustomerUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "Customer",
			      				    IsNew =  false,
			      				    DBTableName =  "Customers",
			      				    OldDBTableName =  "Customers",
			      				    ObjectTableSingular =  "Customer",
			      				    ObjectTablePlural =  "Customers",
			      				    HasCustomFilter =  false,
			      				    HasCustomFields =  false,
			      				    HasHelper =  true,
			      				    HasShortTitle =  true,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  true,
			      				    IsNewWizard =  true,
			      				    LookUp1 =  "Code",
			      				    LookUp2 =  "EnglishName",
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
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  true,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  10,
			      				    NewWizardControlName =  "Simplog.FreightLib.NewCustomerCommand",
			      				    DefaultText =  "Customer",
			      				    Code =  "CLNT",
			      				    Name =  "Customers",
			      				    GenerateDomainService =  false,
			      				    NoTS =  false,
			      				    HasMenuButtons =  true,
			      				    AllowedForComputingPartners =  true,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  true,
			      				    IsLookUp =  true,
			      				    SearchFields =  "Customer,Customers,Simplog.FreightLib.NewCustomerCommand,Id,",
			      				    CodeField =  "Code",
			      				    NameField =  "EnglishName",
			      				    ClientModuleName =  "Common",
			      				    NewWizardComponentPath =  "./CommonModules/CommonCustomer/Components/NewEntity/NewCustomerComponent",
			      				    AllowedInQueues =  false,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Code",
					  						ObjectTableName =  "Customer",
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
					  						ValidForQuerySection1 =  "Customer",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						SearchFields =  "Code,Customer,Code,Code,Code,Code,0,15",
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  true,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  true,
					  						FullFieldLable =  "Code",
					  						DefaultText =  @"Code",
					  						ListFieldLable =  "CodeListLable",
					  						ListLableDefaultText =  @"Code",
					  						HelpTextCode =  "Code",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "EnglishName",
					  						ObjectTableName =  "Customer",
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
					  						DisplayInLookUpIndex =  2,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  2,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						DataTemplateName =  "CustomerEnglishNameDataTemplate",
					  						HasTemplate =  true,
					  						IsRequired =  true,
					  						FullFieldLable =  "Name",
					  						DefaultText =  @"Name",
					  						ListFieldLable =  "EnglishNameListLable",
					  						ListLableDefaultText =  @"Name",
					  						HelpTextCode =  "Name",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LocalName",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "nText",
					  						Code =  "LocalName",
					  						MaxLength =  100,
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
					  						PMPropertyPath =  "LocalName",
					  						ListPropertyPath =  "LocalName",
					  						DisplayInLookUpIndex =  1,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  1,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						SearchFields =  "LocalName,Customer,Local Name,LocalName,LocalName,Local Name,0,100",
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
					  						DefaultText =  @"Local Name",
					  						ListFieldLable =  "LocalNameListLable",
					  						ListLableDefaultText =  @"Local Name",
					  						HelpTextCode =  "LocalName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ComputedLocalName",
					  						ObjectTableName =  "Customer",
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
					  						ValidForQuerySection1 =  "Customer",
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
					  						DefaultText =  @"Local Name",
					  						HelpTextCode =  "ComputedLocalName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InActive",
					  						ObjectTableName =  "Customer",
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
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "InActive",
					  						DefaultText =  @"Inactive Customer",
					  						ListFieldLable =  "InActiveListLable",
					  						ListLableDefaultText =  @"Inactive",
					  						HelpTextCode =  "InActive",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AccountManagerUserId",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
					  						Code =  "AccountManagerUserId",
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
					  						PMPropertyPath =  "AccountManagerUserId",
					  						ListPropertyPath =  "AccountManagerUserId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "AccountManagerUserId",
					  						DefaultText =  @"Account Manager",
					  						HelpTextCode =  "AccountManagerUserId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SalesmanUserId",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
					  						Code =  "SalesmanUserId",
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
					  						PMPropertyPath =  "SalesmanUserId",
					  						ListPropertyPath =  "SalesmanUserId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						SearchFields =  "SalesmanUserId,Customer,Salesman,SalesmanUserId,SalesmanUserId,,0,15",
					  						DisplayLongName =  false,
					  						NumberOfDigits =  0,
					  						IsMaxLength =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						AllowedInCustomerFieldsSettings =  true,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "SalesmanUserId",
					  						DefaultText =  @"Salesman",
					  						HelpTextCode =  "SalesmanUserId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Website",
					  						ObjectTableName =  "Customer",
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
					  						ValidForQuerySection1 =  "Customer",
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
					  						ListFieldLable =  "WebsiteListLable",
					  						ListLableDefaultText =  @"Website",
					  						HelpTextCode =  "Website",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BillToId",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Card",
					  						Code =  "BillToId",
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
					  						PMPropertyPath =  "BillToId",
					  						ListPropertyPath =  "BillToId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "BillToId",
					  						DefaultText =  @"Bill To",
					  						ListFieldLable =  "BillToIdListLable",
					  						ListLableDefaultText =  @"Bill To",
					  						HelpTextCode =  "BillToId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "VatNumber",
					  						ObjectTableName =  "Customer",
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
					  						ValidForQuerySection1 =  "Customer",
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
					  						ListFieldLable =  "VatNumberListLable",
					  						ListLableDefaultText =  @"VAT Number",
					  						HelpTextCode =  "VatNumber",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PaymentTermId",
					  						ObjectTableName =  "Customer",
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
					  						ValidForQuerySection1 =  "Customer",
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
					  						DefaultText =  @"Payment Term",
					  						HelpTextCode =  "PaymentTermId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PaymentTermEnglishName",
					  						ObjectTableName =  "Customer",
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
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "PaymentTermName",
					  						DefaultText =  @"Payment Term Name",
					  						ListFieldLable =  "PaymentTermListLable",
					  						ListLableDefaultText =  @"Payment Term",
					  						HelpTextCode =  "PaymentTermName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AccountManagerUserEnglishName",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						Code =  "AccountManagerUserEnglishName",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "AccountManagerUserEnglishName",
					  						ListPropertyPath =  "AccountManagerUserEnglishName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "AccountManagerUserEnglishName",
					  						DefaultText =  @"Referent",
					  						ListFieldLable =  "AccountManagerUserNameListLable",
					  						ListLableDefaultText =  @"Referent",
					  						HelpTextCode =  "AccountManagerUserEnglishName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SalesmanUserEnglishName",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						Code =  "SalesmanUserEnglishName",
					  						MaxLength =  40,
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
					  						PMPropertyPath =  "SalesmanUserEnglishName",
					  						ListPropertyPath =  "SalesmanUserEnglishName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "SalesmanUserName",
					  						DefaultText =  @"Salesman",
					  						ListFieldLable =  "SalesmanUserNameListLable",
					  						ListLableDefaultText =  @"Salesman",
					  						HelpTextCode =  "SalesmanUserName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "RankName",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						LookUpTableName =  "User",
					  						Code =  "RankName",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "RankName",
					  						ListPropertyPath =  "RankName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						DataTemplateName =  "RankDataTemplate",
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "RankName",
					  						DefaultText =  @"Rank",
					  						ListFieldLable =  "RankNameListLable",
					  						ListLableDefaultText =  @"Rank",
					  						HelpTextCode =  "RankName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Notes",
					  						ObjectTableName =  "Customer",
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
					  						ValidForQuerySection1 =  "Customer",
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
					  						DefaultText =  @"Notes",
					  						ListFieldLable =  "NotesListLable",
					  						ListLableDefaultText =  @"Notes",
					  						HelpTextCode =  "Notes",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InvoiceCurrencyId",
					  						ObjectTableName =  "Customer",
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
					  						IsCustomFilter =  false,
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
					  						ValidForQuerySection1 =  "Customer",
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
					  						DefaultText =  @"Invoice Currency",
					  						ListFieldLable =  "InvoiceCurrencyIdListLable",
					  						ListLableDefaultText =  @"Invoice Currency",
					  						HelpTextCode =  "InvoiceCurrencyId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CityName",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						Code =  "CityName",
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
					  						PMPropertyPath =  "CityName",
					  						ListPropertyPath =  "CityName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "CityName",
					  						DefaultText =  @"City",
					  						ListFieldLable =  "CityName",
					  						ListLableDefaultText =  @"City",
					  						HelpTextCode =  "CityName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LastShipmentDate",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "DateTime",
					  						Code =  "LastShipmentDate",
					  						MaxLength =  1,
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
					  						PMPropertyPath =  "LastShipmentDate",
					  						ListPropertyPath =  "LastShipmentDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						DataTemplateName =  "DaysFromLastShipmentDataTemplate",
					  						HasTemplate =  true,
					  						IsRequired =  false,
					  						FullFieldLable =  "LastShipmentDate",
					  						DefaultText =  @"Last Shipment",
					  						ListFieldLable =  "LastShipmentDateListLable",
					  						ListLableDefaultText =  @"Last Shipment",
					  						HelpTextCode =  "LastShipmentDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Activity",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						Code =  "Activity",
					  						MaxLength =  25,
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
					  						PMPropertyPath =  "Activity",
					  						ListPropertyPath =  "Activity",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						DataTemplateName =  "ActivityDataTemplate",
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "Activity",
					  						DefaultText =  @"Activity",
					  						ListFieldLable =  "ActivityListLable",
					  						ListLableDefaultText =  @"Activity",
					  						HelpTextCode =  "Activity",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Seniority",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						Code =  "Seniority",
					  						MaxLength =  40,
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
					  						PMPropertyPath =  "Seniority",
					  						ListPropertyPath =  "Seniority",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						DataTemplateName =  "SeniorityDataTemplate",
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "Seniority",
					  						DefaultText =  @"Seniority",
					  						ListFieldLable =  "SeniorityListLable",
					  						ListLableDefaultText =  @"Seniority",
					  						HelpTextCode =  "Seniority",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CustomersByLastActivity",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Boolean",
					  						Code =  "CustomersByLastActivity",
					  						MaxLength =  25,
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
					  						PMPropertyPath =  "CustomersByLastActivity",
					  						ListPropertyPath =  "CustomersByLastActivity",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "CustomersByLastActivity",
					  						DefaultText =  @"Customers By Last Activity",
					  						HelpTextCode =  "CustomersByLastActivity",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "RankId",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Rank",
					  						Code =  "RankId",
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
					  						PMPropertyPath =  "RankId",
					  						ListPropertyPath =  "RankId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "RankId",
					  						DefaultText =  @"Rank",
					  						HelpTextCode =  "RankId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "DaysFromLastShipment",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						Code =  "DaysFromLastShipment",
					  						MaxLength =  25,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  25,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "DaysFromLastShipment",
					  						ListPropertyPath =  "DaysFromLastShipment",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						DataTemplateName =  "DaysFromLastShipmentDataTemplate",
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "DaysFromLastShipment",
					  						DefaultText =  @"Last Shipment",
					  						ListFieldLable =  "DaysFromLastShipmentListLable",
					  						ListLableDefaultText =  @"Last Shipment",
					  						HelpTextCode =  "DaysFromLastShipment",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "StartWorkingManuallySet",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Boolean",
					  						Code =  "StartWorkingManuallySet",
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
					  						PMPropertyPath =  "StartWorkingManuallySet",
					  						ListPropertyPath =  "StartWorkingManuallySet",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  true,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "StartWorkingManuallySet",
					  						DefaultText =  @"Start Working Manually Set",
					  						HelpTextCode =  "StartWorkingManuallySet",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "StartWorkingDate",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "DateTime",
					  						Code =  "StartWorkingDate",
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "StartWorkingDate",
					  						ListPropertyPath =  "StartWorkingDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						DataTemplateName =  "SeniorityDataTemplate",
					  						HasTemplate =  true,
					  						IsRequired =  false,
					  						FullFieldLable =  "StartWorkingDate",
					  						DefaultText =  @"Start Working",
					  						ListFieldLable =  "StartWorkingDateListLable",
					  						ListLableDefaultText =  @"Start Working",
					  						HelpTextCode =  "StartWorkingDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PhoneNumber",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						Code =  "PhoneNumber",
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "PhoneNumber",
					  						ListPropertyPath =  "PhoneNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "PhoneNumber",
					  						DefaultText =  @"Tel",
					  						HelpTextCode =  "PhoneNumber",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FaxNumber",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						Code =  "FaxNumber",
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "FaxNumber",
					  						ListPropertyPath =  "FaxNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "FaxNumber",
					  						DefaultText =  @"Fax",
					  						HelpTextCode =  "FaxNumber",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CityWithCountry",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						Code =  "CityWithCountry",
					  						MaxLength =  145,
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
					  						PMPropertyPath =  "CityWithCountry",
					  						ListPropertyPath =  "CityWithCountry",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						DataTemplateName =  "CityWithCountryTemplate",
					  						HasTemplate =  true,
					  						IsRequired =  false,
					  						FullFieldLable =  "CityWithCountry",
					  						DefaultText =  @"City (Country)",
					  						HelpTextCode =  "CityWithCountry",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "RankCode",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						LookUpTableName =  "User",
					  						Code =  "RankCode",
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
					  						PMPropertyPath =  "RankCode",
					  						ListPropertyPath =  "RankCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						DataTemplateName =  "RankDataTemplate",
					  						HasTemplate =  true,
					  						IsRequired =  false,
					  						FullFieldLable =  "RankCode",
					  						DefaultText =  @"Rank",
					  						ListFieldLable =  "RankCodeListLable",
					  						ListLableDefaultText =  @"Rank",
					  						HelpTextCode =  "RankCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BillToName",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						Code =  "BillToName",
					  						MaxLength =  100,
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
					  						PMPropertyPath =  "BillToName",
					  						ListPropertyPath =  "BillToName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "BillToName",
					  						DefaultText =  @"Bill To",
					  						ListFieldLable =  "BillToNameListLable",
					  						ListLableDefaultText =  @"Bill To",
					  						HelpTextCode =  "BillToName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "VatTypeId",
					  						ObjectTableName =  "Customer",
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
					  						ValidForQuerySection1 =  "Customer",
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
					  						DefaultText =  @"VAT Type",
					  						HelpTextCode =  "VatTypeId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BankName",
					  						ObjectTableName =  "Customer",
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
					  						ValidForQuerySection1 =  "Customer",
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
					  						DefaultText =  @"Bank Name",
					  						HelpTextCode =  "BankName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BankAddress",
					  						ObjectTableName =  "Customer",
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
					  						ValidForQuerySection1 =  "Customer",
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
					  						DefaultText =  @"Bank Address",
					  						HelpTextCode =  "BankAddress",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Swift",
					  						ObjectTableName =  "Customer",
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
					  						ValidForQuerySection1 =  "Customer",
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
					  						DefaultText =  @"Swift",
					  						HelpTextCode =  "Swift",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AccountNumber",
					  						ObjectTableName =  "Customer",
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
					  						ValidForQuerySection1 =  "Customer",
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
					  						DefaultText =  @"Bank Account Number",
					  						HelpTextCode =  "AccountNumber",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IBANNumber",
					  						ObjectTableName =  "Customer",
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
					  						ValidForQuerySection1 =  "Customer",
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
					  						DefaultText =  @"IBAN No.",
					  						HelpTextCode =  "IBANNumber",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SharedLogisticsInvitationStatusName",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						Code =  "SharedLogisticsInvitationStatusName",
					  						MaxLength =  40,
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
					  						PMPropertyPath =  "SharedLogisticsInvitationStatusName",
					  						ListPropertyPath =  "SharedLogisticsInvitationStatusName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						DataTemplateName =  "SharedLogisticsInvitationStatusDataTemplate",
					  						HasTemplate =  true,
					  						IsRequired =  false,
					  						FullFieldLable =  "SharedLogisticsInvitationStatusName",
					  						DefaultText =  @"Invitation Status",
					  						ListFieldLable =  "SharedLogisticsInvitationStatusNameListLable",
					  						ListLableDefaultText =  @"Invitation Status",
					  						HelpTextCode =  "SharedLogisticsInvitationStatusName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LastLoginDate",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "DateTime",
					  						Code =  "LastLoginDate",
					  						MaxLength =  0,
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
					  						PMPropertyPath =  "LastLoginDate",
					  						ListPropertyPath =  "LastLoginDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						HasTemplate =  true,
					  						IsRequired =  false,
					  						FullFieldLable =  "LastLoginDate",
					  						DefaultText =  @"Last Login Date",
					  						ListFieldLable =  "LastLoginDateListLable",
					  						ListLableDefaultText =  @"Last Login Date",
					  						HelpTextCode =  "LastLoginDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InvitationDate",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "DateTime",
					  						Code =  "InvitationDate",
					  						MaxLength =  0,
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
					  						PMPropertyPath =  "InvitationDate",
					  						ListPropertyPath =  "InvitationDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "InvitationDate",
					  						DefaultText =  @"Invitation Date",
					  						ListFieldLable =  "InvitationDateListLable",
					  						ListLableDefaultText =  @"Invitation Date",
					  						HelpTextCode =  "InvitationDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreditLimit",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Double",
					  						Code =  "CreditLimit",
					  						MaxLength =  0,
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
					  						PMPropertyPath =  "CreditLimit",
					  						ListPropertyPath =  "CreditLimit",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "CreditLimit",
					  						DefaultText =  @"Credit Limit",
					  						HelpTextCode =  "CreditLimit",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IndustryId",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Industry",
					  						Code =  "IndustryId",
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
					  						PMPropertyPath =  "IndustryId",
					  						ListPropertyPath =  "IndustryId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "IndustryId",
					  						DefaultText =  @"Industry",
					  						HelpTextCode =  "IndustryId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LeadSourceId",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "LeadSource",
					  						Code =  "LeadSourceId",
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
					  						PMPropertyPath =  "LeadSourceId",
					  						ListPropertyPath =  "LeadSourceId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "LeadSourceId",
					  						DefaultText =  @"Lead Source",
					  						HelpTextCode =  "LeadSourceId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CollectorId",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
					  						Code =  "CollectorId",
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
					  						PMPropertyPath =  "CollectorId",
					  						ListPropertyPath =  "CollectorId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "CollectorId",
					  						DefaultText =  @"Collector",
					  						HelpTextCode =  "CollectorId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ClassifierId",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
					  						Code =  "ClassifierId",
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
					  						PMPropertyPath =  "ClassifierId",
					  						ListPropertyPath =  "ClassifierId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "ClassifierId",
					  						DefaultText =  @"Classifier",
					  						HelpTextCode =  "ClassifierId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IndustryName",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "nText",
					  						Code =  "IndustryName",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "IndustryName",
					  						ListPropertyPath =  "IndustryName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "IndustryName",
					  						DefaultText =  @"Industry",
					  						ListFieldLable =  "IndustryNameListLable",
					  						ListLableDefaultText =  @"Industry",
					  						HelpTextCode =  "IndustryName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ATTN",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "nText",
					  						Code =  "ATTN",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "ATTN",
					  						ListPropertyPath =  "ATTN",
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
					  						FullFieldLable =  "ATTN",
					  						DefaultText =  @"ATTN",
					  						ListFieldLable =  "ATTNListLable",
					  						ListLableDefaultText =  @"ATTN",
					  						HelpTextCode =  "ATTN",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "MyCustomers",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Boolean",
					  						Code =  "MyCustomers",
					  						MaxLength =  0,
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
					  						PMPropertyPath =  "MyCustomers",
					  						ListPropertyPath =  "MyCustomers",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "MyCustomers",
					  						DefaultText =  @"My Customers",
					  						HelpTextCode =  "MyCustomers",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreateDate",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "DateTime",
					  						Code =  "CreateDate",
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
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "CreateDate",
					  						DefaultText =  @"Create Date",
					  						ListFieldLable =  "CreateDateListLable",
					  						ListLableDefaultText =  @"Create Date",
					  						HelpTextCode =  "CreateDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LeadDescription",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						Code =  "LeadDescription",
					  						MaxLength =  100,
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
					  						PMPropertyPath =  "LeadDescription",
					  						ListPropertyPath =  "LeadDescription",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "LeadDescription",
					  						DefaultText =  @"Lead Description",
					  						ListFieldLable =  "LeadDescriptionListLable",
					  						ListLableDefaultText =  @"Lead Description",
					  						HelpTextCode =  "LeadDescription",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "MyCustomersAsAccountManager",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Boolean",
					  						Code =  "MyCustomersAsAccountManager",
					  						MaxLength =  0,
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
					  						PMPropertyPath =  "MyCustomersAsAccountManager",
					  						ListPropertyPath =  "MyCustomersAsAccountManager",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "MyCustomersAsAccountManager",
					  						DefaultText =  @"My Customers (as Account Manager)",
					  						HelpTextCode =  "MyCustomersAsAccountManager",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CollectorName",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						Code =  "CollectorName",
					  						MaxLength =  40,
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
					  						PMPropertyPath =  "CollectorName",
					  						ListPropertyPath =  "CollectorName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "CollectorName",
					  						DefaultText =  @"Collector",
					  						ListFieldLable =  "CollectorNameListLable",
					  						ListLableDefaultText =  @"Collector",
					  						HelpTextCode =  "CollectorName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ClassifierName",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						Code =  "ClassifierName",
					  						MaxLength =  40,
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
					  						PMPropertyPath =  "ClassifierName",
					  						ListPropertyPath =  "ClassifierName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "ClassifierName",
					  						DefaultText =  @"Classifier",
					  						ListFieldLable =  "ClassifierNameListLable",
					  						ListLableDefaultText =  @"Classifier",
					  						HelpTextCode =  "ClassifierName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CustomsAgentId",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Card",
					  						Code =  "CustomsAgentId",
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
					  						PMPropertyPath =  "CustomsAgentId",
					  						ListPropertyPath =  "CustomsAgentId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "CustomsAgentId",
					  						DefaultText =  @"Customs Agent",
					  						HelpTextCode =  "CustomsAgentId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ForwarderId",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Card",
					  						Code =  "ForwarderId",
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
					  						PMPropertyPath =  "ForwarderId",
					  						ListPropertyPath =  "ForwarderId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "ForwarderId",
					  						DefaultText =  @"Forwarder",
					  						HelpTextCode =  "ForwarderId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "MediatorId",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Card",
					  						Code =  "MediatorId",
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
					  						PMPropertyPath =  "MediatorId",
					  						ListPropertyPath =  "MediatorId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "MediatorId",
					  						DefaultText =  @"Mediator",
					  						HelpTextCode =  "MediatorId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "City_Potential",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "nText",
					  						Code =  "City_Potential",
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
					  						PMPropertyPath =  "City_Potential",
					  						ListPropertyPath =  "City_Potential",
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
					  						FullFieldLable =  "City_Potential",
					  						DefaultText =  @"City",
					  						HelpTextCode =  "City_Potential",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Address1_Potential",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "nText",
					  						Code =  "Address1_Potential",
					  						MaxLength =  65,
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
					  						PMPropertyPath =  "Address1_Potential",
					  						ListPropertyPath =  "Address1_Potential",
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
					  						FullFieldLable =  "Address1_Potential",
					  						DefaultText =  @"Address1",
					  						HelpTextCode =  "Address1_Potential",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Address2_Potential",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "nText",
					  						Code =  "Address2_Potential",
					  						MaxLength =  65,
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
					  						PMPropertyPath =  "Address2_Potential",
					  						ListPropertyPath =  "Address2_Potential",
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
					  						FullFieldLable =  "Address2_Potential",
					  						DefaultText =  @"Address2",
					  						HelpTextCode =  "Address2_Potential",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CountryId_Potential",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Country",
					  						Code =  "CountryId_Potential",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "CountryId_Potential",
					  						ListPropertyPath =  "CountryId_Potential",
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
					  						FullFieldLable =  "CountryId_Potential",
					  						DefaultText =  @"Country",
					  						HelpTextCode =  "CountryId_Potential",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "StateId_Potential",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "State",
					  						Code =  "StateId_Potential",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "StateId_Potential",
					  						ListPropertyPath =  "StateId_Potential",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ControlField1 =  "CountryId_Potential",
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
					  						FullFieldLable =  "State_Potential",
					  						DefaultText =  @"State",
					  						HelpTextCode =  "State_Potential",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ZipCode_Potential",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						Code =  "ZipCode_Potential",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "ZipCode_Potential",
					  						ListPropertyPath =  "ZipCode_Potential",
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
					  						FullFieldLable =  "ZipCode_Potential",
					  						DefaultText =  @"Zip Code",
					  						HelpTextCode =  "ZipCode_Potential",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PhoneNumber_Potential",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						Code =  "PhoneNumber_Potential",
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
					  						PMPropertyPath =  "PhoneNumber_Potential",
					  						ListPropertyPath =  "PhoneNumber_Potential",
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
					  						FullFieldLable =  "PhoneNumber_Potential",
					  						DefaultText =  @"Phone",
					  						HelpTextCode =  "PhoneNumber_Potential",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FaxNumber_Potential",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						Code =  "FaxNumber_Potential",
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
					  						PMPropertyPath =  "FaxNumber_Potential",
					  						ListPropertyPath =  "FaxNumber_Potential",
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
					  						FullFieldLable =  "FaxNumber_Potential",
					  						DefaultText =  @"Fax",
					  						HelpTextCode =  "FaxNumber_Potential",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ATTN_Potential",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "nText",
					  						Code =  "ATTN_Potential",
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
					  						PMPropertyPath =  "ATTN_Potential",
					  						ListPropertyPath =  "ATTN_Potential",
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
					  						FullFieldLable =  "ATTN_Potential",
					  						DefaultText =  @"ATTN",
					  						HelpTextCode =  "ATTN_Potential",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LeadSourceName",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "nText",
					  						Code =  "LeadSourceName",
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "LeadSourceName",
					  						ListPropertyPath =  "LeadSourceName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "LeadSourceName",
					  						DefaultText =  @"Lead Source",
					  						HelpTextCode =  "LeadSourceName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CustomerStatusName",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						Code =  "CustomerStatusName",
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
					  						PMPropertyPath =  "CustomerStatusName",
					  						ListPropertyPath =  "CustomerStatusName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						DataTemplateName =  "CustomerStatusTemplate",
					  						HasTemplate =  true,
					  						IsRequired =  false,
					  						FullFieldLable =  "CustomerStatusName",
					  						DefaultText =  @"Status",
					  						ListFieldLable =  "CustomerStatusNameLabel",
					  						ListLableDefaultText =  @"Status",
					  						HelpTextCode =  "CustomerStatusName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PrimaryContactName",
					  						ObjectTableName =  "Customer",
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
					  						ValidForQuerySection1 =  "Customer",
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
					  						DefaultText =  @"Primary Contact",
					  						ListFieldLable =  "PrimaryContactNameLabel",
					  						ListLableDefaultText =  @"Primary Contact",
					  						HelpTextCode =  "PrimaryContactName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PrimaryContactPhone",
					  						ObjectTableName =  "Customer",
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
					  						ValidForQuerySection1 =  "Customer",
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
					  						DefaultText =  @"Tel",
					  						HelpTextCode =  "PrimaryContactPhone",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CodeMyCustomer",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						Code =  "CodeMyCustomer",
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "CodeMyCustomer",
					  						ListPropertyPath =  "CodeMyCustomer",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "CodeMyCustomer",
					  						DefaultText =  @"Code",
					  						HelpTextCode =  "CodeMyCustomer",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ReadyCustomers",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Boolean",
					  						Code =  "ReadyCustomers",
					  						MaxLength =  0,
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
					  						PMPropertyPath =  "ReadyCustomers",
					  						ListPropertyPath =  "ReadyCustomers",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "ReadyCustomers",
					  						DefaultText =  @"Ready Customers",
					  						HelpTextCode =  "ReadyCustomers",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ReadyForActivationDate",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "DateTime",
					  						Code =  "ReadyForActivationDate",
					  						MaxLength =  100,
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
					  						PMPropertyPath =  "ReadyForActivationDate",
					  						ListPropertyPath =  "ReadyForActivationDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "ReadyForActivationDate",
					  						DefaultText =  @"Ready For Activation Date",
					  						ListFieldLable =  "ReadyForActivationDateListLable",
					  						ListLableDefaultText =  @"Ready For Activation Date",
					  						HelpTextCode =  "ReadyForActivationDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsCustomer",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Boolean",
					  						Code =  "IsCustomer",
					  						MaxLength =  1,
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
					  						PMPropertyPath =  "IsCustomer",
					  						ListPropertyPath =  "IsCustomer",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						SearchFields =  "IsCustomer,Customer,Is Customer,IsCustomer,IsCustomer,Is Customer,0,1",
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
					  						FullFieldLable =  "IsCustomer",
					  						DefaultText =  @"Is Customer",
					  						ListFieldLable =  "IsCustomerListLable",
					  						ListLableDefaultText =  @"Is Customer",
					  						HelpTextCode =  "IsCustomer",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ShippersAndConsignees",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Boolean",
					  						Code =  "ShippersAndConsignees",
					  						MaxLength =  0,
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
					  						PMPropertyPath =  "ShippersAndConsignees",
					  						ListPropertyPath =  "ShippersAndConsignees",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "ShippersAndConsignees",
					  						DefaultText =  @"Shippers and Consignees",
					  						HelpTextCode =  "ShippersAndConsignees",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CustomerStatusCode",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "CustomerStatus",
					  						Code =  "CustomerStatusCode",
					  						MaxLength =  3,
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
					  						PMPropertyPath =  "CustomerStatusCode",
					  						ListPropertyPath =  "CustomerStatusCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						HasTemplate =  true,
					  						IsRequired =  false,
					  						FullFieldLable =  "CustomerStatusCode",
					  						DefaultText =  @"Status Code",
					  						HelpTextCode =  "CustomerStatusCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CustomerSizeId",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "CustomerSize",
					  						Code =  "CustomerSizeId",
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
					  						PMPropertyPath =  "CustomerSizeId",
					  						ListPropertyPath =  "CustomerSizeId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "CustomerSizeId",
					  						DefaultText =  @"Customer Size",
					  						HelpTextCode =  "CustomerSizeId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BusinessUnitId",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "BusinessUnit",
					  						Code =  "BusinessUnitId",
					  						MaxLength =  50,
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
					  						PMPropertyPath =  "BusinessUnitId",
					  						ListPropertyPath =  "BusinessUnitId",
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
					  						FullFieldLable =  "BusinessUnitId",
					  						DefaultText =  @"Business Unit",
					  						HelpTextCode =  "BusinessUnitId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CountryName",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						Code =  "CountryName",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						ListPropertyPath =  "CountryName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "CountryName",
					  						DefaultText =  @"Country",
					  						ListFieldLable =  "CountryName",
					  						ListLableDefaultText =  @"Country",
					  						HelpTextCode =  "CountryName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PotentialCustomers",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Boolean",
					  						Code =  "PotentialCustomers",
					  						MaxLength =  0,
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
					  						PMPropertyPath =  "PotentialCustomers",
					  						ListPropertyPath =  "PotentialCustomers",
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
					  						FullFieldLable =  "PotentialCustomers",
					  						DefaultText =  @"Potential Customers",
					  						HelpTextCode =  "PotentialCustomers",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ActiveCustomers",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Boolean",
					  						Code =  "ActiveCustomers",
					  						MaxLength =  0,
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
					  						PMPropertyPath =  "ActiveCustomers",
					  						ListPropertyPath =  "ActiveCustomers",
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
					  						FullFieldLable =  "ActiveCustomers",
					  						DefaultText =  @"Active Customers",
					  						HelpTextCode =  "ActiveCustomers",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InactiveCustomers",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Boolean",
					  						Code =  "InactiveCustomers",
					  						MaxLength =  0,
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
					  						PMPropertyPath =  "InactiveCustomers",
					  						ListPropertyPath =  "InactiveCustomers",
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
					  						FullFieldLable =  "InactiveCustomers",
					  						DefaultText =  @"Inactive Customers",
					  						HelpTextCode =  "InactiveCustomers",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreatedByUserId",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
					  						Code =  "CreatedByUserId",
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
					  						PMPropertyPath =  "CreatedByUserId",
					  						ListPropertyPath =  "CreatedByUserId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "CreatedByUserId",
					  						DefaultText =  @"Created by",
					  						HelpTextCode =  "CreatedByUserId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdatedByUserId",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "User",
					  						Code =  "UpdatedByUserId",
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
					  						PMPropertyPath =  "UpdatedByUserId",
					  						ListPropertyPath =  "UpdatedByUserId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "UpdatedByUserId",
					  						DefaultText =  @"Updated by",
					  						HelpTextCode =  "UpdatedByUserId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreatedByUserName",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						Code =  "CreatedByUserName",
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
					  						PMPropertyPath =  "CreatedByUserName",
					  						ListPropertyPath =  "CreatedByUserName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "CreatedByUserName",
					  						DefaultText =  @"Created by",
					  						ListFieldLable =  "CreatedByUserNameLabel",
					  						ListLableDefaultText =  @"Created by",
					  						HelpTextCode =  "CreatedByUserName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdatedByUserName",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						Code =  "UpdatedByUserName",
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
					  						PMPropertyPath =  "UpdatedByUserName",
					  						ListPropertyPath =  "UpdatedByUserName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "UpdatedByUserName",
					  						DefaultText =  @"Updated by",
					  						ListFieldLable =  "UpdatedByUserNameLabel",
					  						ListLableDefaultText =  @"Updated by",
					  						HelpTextCode =  "UpdatedByUserName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PrimaryContactId",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Contact",
					  						Code =  "PrimaryContactId",
					  						MaxLength =  40,
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
					  						PMPropertyPath =  "PrimaryContactId",
					  						ListPropertyPath =  "PrimaryContactId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "PrimaryContactId",
					  						DefaultText =  @"Primary Contact",
					  						HelpTextCode =  "PrimaryContactId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "RegionId",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Region",
					  						Code =  "RegionId",
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
					  						PMPropertyPath =  "RegionId",
					  						ListPropertyPath =  "RegionId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "RegionId",
					  						DefaultText =  @"Region",
					  						HelpTextCode =  "RegionId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "RegionName",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						Code =  "RegionName",
					  						MaxLength =  100,
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
					  						PMPropertyPath =  "RegionName",
					  						ListPropertyPath =  "RegionName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "RegionName",
					  						DefaultText =  @"Region",
					  						ListFieldLable =  "RegionNameLabel",
					  						ListLableDefaultText =  @"Region",
					  						HelpTextCode =  "RegionName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LastCallDate",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "DateTime",
					  						Code =  "LastCallDate",
					  						MaxLength =  1,
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
					  						PMPropertyPath =  "LastCallDate",
					  						ListPropertyPath =  "LastCallDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "LastCallDate",
					  						DefaultText =  @"Last Call",
					  						ListFieldLable =  "LastCallDateLable",
					  						ListLableDefaultText =  @"Last Call",
					  						HelpTextCode =  "LastCallDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LastMeetingDate",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "DateTime",
					  						Code =  "LastMeetingDate",
					  						MaxLength =  1,
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
					  						PMPropertyPath =  "LastMeetingDate",
					  						ListPropertyPath =  "LastMeetingDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "LastMeetingDate",
					  						DefaultText =  @"Last Meeting",
					  						ListFieldLable =  "LastMeetingDateLable",
					  						ListLableDefaultText =  @"Last Meeting",
					  						HelpTextCode =  "LastMeetingDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LastOpportunityDate",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "DateTime",
					  						Code =  "LastOpportunityDate",
					  						MaxLength =  1,
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
					  						PMPropertyPath =  "LastOpportunityDate",
					  						ListPropertyPath =  "LastOpportunityDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "LastOpportunityDate",
					  						DefaultText =  @"Last Opportunity",
					  						ListFieldLable =  "LastOpportunityDateLable",
					  						ListLableDefaultText =  @"Last Opportunity",
					  						HelpTextCode =  "LastOpportunityDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FirstInvoiceDate",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "DateTime",
					  						Code =  "FirstInvoiceDate",
					  						MaxLength =  1,
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
					  						PMPropertyPath =  "FirstInvoiceDate",
					  						ListPropertyPath =  "FirstInvoiceDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "FirstInvoiceDate",
					  						DefaultText =  @"First Invoice",
					  						ListFieldLable =  "FirstInvoiceDateLable",
					  						ListLableDefaultText =  @"First Invoice",
					  						HelpTextCode =  "FirstInvoiceDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "FirstShipmentDate",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "DateTime",
					  						Code =  "FirstShipmentDate",
					  						MaxLength =  1,
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
					  						PMPropertyPath =  "FirstShipmentDate",
					  						ListPropertyPath =  "FirstShipmentDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "FirstShipmentDate",
					  						DefaultText =  @"First Shipment",
					  						ListFieldLable =  "FirstShipmentDateLable",
					  						ListLableDefaultText =  @"First Shipment",
					  						HelpTextCode =  "FirstShipmentDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LastQuoteDate",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "DateTime",
					  						Code =  "LastQuoteDate",
					  						MaxLength =  1,
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
					  						PMPropertyPath =  "LastQuoteDate",
					  						ListPropertyPath =  "LastQuoteDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "LastQuoteDate",
					  						DefaultText =  @"Last Quote",
					  						ListFieldLable =  "LastQuoteDateLable",
					  						ListLableDefaultText =  @"Last Quote",
					  						HelpTextCode =  "LastQuoteDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LastInteractionDate",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "DateTime",
					  						Code =  "LastInteractionDate",
					  						MaxLength =  1,
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
					  						PMPropertyPath =  "LastInteractionDate",
					  						ListPropertyPath =  "LastInteractionDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeToDateConverter",
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "LastInteractionDate",
					  						DefaultText =  @"Last Interaction",
					  						ListFieldLable =  "LastInteractionDateLable",
					  						ListLableDefaultText =  @"Last Interaction",
					  						HelpTextCode =  "LastInteractionDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CountryId",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Country",
					  						Code =  "CountryId",
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
					  						PMPropertyPath =  "CountryId",
					  						ListPropertyPath =  "CountryId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "CountryId",
					  						DefaultText =  @"Country",
					  						HelpTextCode =  "CountryId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "EnableConsolidationInvoices",
					  						ObjectTableName =  "Customer",
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
					  						ValidForQuerySection1 =  "Customer",
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
					  						DefaultText =  @"Consolidated Inv.",
					  						HelpTextCode =  "EnableConsolidationInvoices",
					  						HelpTextDefaultText =  @"This client will receive one Invoice for multiple Shipments once a period",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ProductsWatch",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Boolean",
					  						Code =  "ProductsWatch",
					  						MaxLength =  1,
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
					  						PMPropertyPath =  "ProductsWatch",
					  						ListPropertyPath =  "ProductsWatch",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						DataTemplateName =  "CustomerProductsWatchDataTemplate",
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "ProductsWatch",
					  						DefaultText =  @"Products Watch",
					  						ListFieldLable =  "ProductsWatchLable",
					  						ListLableDefaultText =  @"Products Watch",
					  						HelpTextCode =  "ProductsWatch",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SalesmanBusinessUnitId",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "BusinessUnit",
					  						Code =  "SalesmanBusinessUnitId",
					  						MaxLength =  50,
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
					  						PMPropertyPath =  "SalesmanBusinessUnitId",
					  						ListPropertyPath =  "SalesmanBusinessUnitId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "SalesmanBusinessUnitId",
					  						DefaultText =  @"Business Unit",
					  						HelpTextCode =  "SalesmanBusinessUnitId",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ActivityWatch",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Boolean",
					  						Code =  "ActivityWatch",
					  						MaxLength =  1,
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
					  						PMPropertyPath =  "ActivityWatch",
					  						ListPropertyPath =  "ActivityWatch",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						DataTemplateName =  "CustomerActivityWatchDataTemplate",
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "ActivityWatch",
					  						DefaultText =  @"Activity Watch",
					  						ListFieldLable =  "ActivityWatchLable",
					  						ListLableDefaultText =  @"Activity Watch",
					  						HelpTextCode =  "ActivityWatch",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PrimaryContactEmail",
					  						ObjectTableName =  "Customer",
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
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "PrimaryContactEmail",
					  						DefaultText =  @"Primary Contact E-mail",
					  						ListFieldLable =  "PrimaryContactEmailLabel",
					  						ListLableDefaultText =  @"Primary Contact E-mail",
					  						HelpTextCode =  "PrimaryContactEmail",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InvoiceCurrencyCode",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						Code =  "InvoiceCurrencyCode",
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "InvoiceCurrencyCode",
					  						ListPropertyPath =  "InvoiceCurrencyCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "InvoiceCurrencyCode",
					  						DefaultText =  @"Invoice Currency",
					  						ListFieldLable =  "InvoiceCurrencyCodeListLable",
					  						ListLableDefaultText =  @"Invoice Currency",
					  						HelpTextCode =  "InvoiceCurrencyCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "KnownConsignor",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						Code =  "KnownConsignor",
					  						MaxLength =  9,
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
					  						PMPropertyPath =  "KnownConsignor",
					  						ListPropertyPath =  "KnownConsignor",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "KnownConsignor",
					  						DefaultText =  @"Known Consignor",
					  						HelpTextCode =  "KnownConsignor",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SharedLogisticsCustomers",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Boolean",
					  						Code =  "SharedLogisticsCustomers",
					  						MaxLength =  0,
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
					  						PMPropertyPath =  "SharedLogisticsCustomers",
					  						ListPropertyPath =  "SharedLogisticsCustomers",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "SharedLogisticsCustomers",
					  						DefaultText =  @"Shared Logistics Customers",
					  						HelpTextCode =  "SharedLogisticsCustomers",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "KCExpirationDate",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "DateTime",
					  						Code =  "KCExpirationDate",
					  						MaxLength =  1,
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
					  						PMPropertyPath =  "KCExpirationDate",
					  						ListPropertyPath =  "KCExpirationDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "KCExpirationDate",
					  						DefaultText =  @"KC expiration date",
					  						HelpTextCode =  "KCExpirationDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdateDate",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "DateTime",
					  						Code =  "UpdateDate",
					  						MaxLength =  1,
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
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						ConverterName =  "Simplog.Infrastructure.Utilities.Converters.DateTimeConverters.DateTimeToTimeAmountConverter",
					  						HasTemplate =  false,
					  						IsRequired =  false,
					  						FullFieldLable =  "UpdateDate",
					  						DefaultText =  @"Update Date",
					  						ListFieldLable =  "UpdateDateListLable",
					  						ListLableDefaultText =  @"Update Date",
					  						HelpTextCode =  "UpdateDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BlockNewInvoiceCreation",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Boolean",
					  						Code =  "BlockNewInvoiceCreation",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "BlockNewInvoiceCreation",
					  						ListPropertyPath =  "BlockNewInvoiceCreation",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "BlockNewInvoiceCreation",
					  						DefaultText =  @"Block New Invoice Creation",
					  						HelpTextCode =  "BlockNewInvoiceCreation",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BlockNewShipmentCreation",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Boolean",
					  						Code =  "BlockNewShipmentCreation",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "BlockNewShipmentCreation",
					  						ListPropertyPath =  "BlockNewShipmentCreation",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "BlockNewShipmentCreation",
					  						DefaultText =  @"Block New Shipment Creation",
					  						HelpTextCode =  "BlockNewShipmentCreation",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ExternalId2",
					  						ObjectTableName =  "Customer",
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
					  						DefaultText =  @"External ID2",
					  						HelpTextCode =  "ExternalId2",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SATForeignRFC",
					  						ObjectTableName =  "Customer",
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
					  						ValidForQuerySection1 =  "Customer",
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
					  						DefaultText =  @"SAT Foreign RFC",
					  						HelpTextCode =  "SATForeignRFC",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "MetodoPagoCode",
					  						ObjectTableName =  "Customer",
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
					  						ValidForQuerySection1 =  "Customer",
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
					  						DefaultText =  @"Metodo Pago",
					  						ListFieldLable =  "MetodoPagoCodeListLable",
					  						ListLableDefaultText =  @"Metodo Pago",
					  						HelpTextCode =  "MetodoPagoCode",
					  						HelpTextDefaultText =  @"Way to Pay:\n-Pago en una sola exhibición (PUE): payment closed at once in one single payment type performed prior to the issuance of the invoice\n-Pago en parcialidades o diferido (PPD): partial payment or deferred performed after the issuance of the invoice",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UsoCFDICode",
					  						ObjectTableName =  "Customer",
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
					  						ValidForQuerySection1 =  "Customer",
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
					  						DefaultText =  @"UsoCFDI",
					  						ListFieldLable =  "UsoCFDIListLable",
					  						ListLableDefaultText =  @"UsoCFDI",
					  						HelpTextCode =  "UsoCFDICode",
					  						HelpTextDefaultText =  @"Use of Digital Fiscal Receipt through Internet",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SupportNotes",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						Code =  "SupportNotes",
					  						MaxLength =  2000,
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
					  						PMPropertyPath =  "SupportNotes",
					  						ListPropertyPath =  "SupportNotes",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "SupportNotes",
					  						DefaultText =  @"Support Notes",
					  						HelpTextCode =  "SupportNotes",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ZipCode",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						Code =  "ZipCode",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						ListPropertyPath =  "ZipCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "ZipCode",
					  						DefaultText =  @"Zip Code",
					  						ListFieldLable =  "ZipCodeListLable",
					  						ListLableDefaultText =  @"Zip Code",
					  						HelpTextCode =  "ZipCode",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Address1",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						Code =  "Address1",
					  						MaxLength =  65,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						ListPropertyPath =  "Address1",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "Address1",
					  						DefaultText =  @"Address1",
					  						ListFieldLable =  "Address1ListLabel",
					  						ListLableDefaultText =  @"Address1",
					  						HelpTextCode =  "Address1",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Address2",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						Code =  "Address2",
					  						MaxLength =  65,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						ListPropertyPath =  "Address2",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "Address2",
					  						DefaultText =  @"Address2",
					  						ListFieldLable =  "Address2ListLabel",
					  						ListLableDefaultText =  @"Address2",
					  						HelpTextCode =  "Address2",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Phone",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						Code =  "Phone",
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
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						ListPropertyPath =  "Phone",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "Phone",
					  						DefaultText =  @"Phone",
					  						ListFieldLable =  "PhoneListLabel",
					  						ListLableDefaultText =  @"Phone",
					  						HelpTextCode =  "Phone",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IRSPlace",
					  						ObjectTableName =  "Customer",
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
					  						ValidForQuerySection1 =  "Customer",
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
					  						DefaultText =  @"IRS Place",
					  						HelpTextCode =  "IRSPlace",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IRSNumber",
					  						ObjectTableName =  "Customer",
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
					  						ValidForQuerySection1 =  "Customer",
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
					  						DefaultText =  @"IRS #",
					  						HelpTextCode =  "IRSNumber",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CustomerSizeName",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "nText",
					  						Code =  "CustomerSizeName",
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
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "CustomerSizeName",
					  						ListPropertyPath =  "CustomerSizeName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "CustomerSizeName",
					  						DefaultText =  @"Customer Size",
					  						ListFieldLable =  "CustomerSizeNameListLabel",
					  						ListLableDefaultText =  @"Customer Size",
					  						HelpTextCode =  "CustomerSizeName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "RequestedAirlines",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						Code =  "RequestedAirlines",
					  						MaxLength =  1000,
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
					  						PMPropertyPath =  "RequestedAirlines",
					  						ListPropertyPath =  "RequestedAirlines",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "RequestedAirlines",
					  						DefaultText =  @"Requested Airlines",
					  						HelpTextCode =  "RequestedAirlines",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "RegisteredAirlines",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						Code =  "RegisteredAirlines",
					  						MaxLength =  1000,
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
					  						PMPropertyPath =  "RegisteredAirlines",
					  						ListPropertyPath =  "RegisteredAirlines",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "RegisteredAirlines",
					  						DefaultText =  @"Registered Airlines",
					  						HelpTextCode =  "RegisteredAirlines",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PendingAirlines",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						Code =  "PendingAirlines",
					  						MaxLength =  1000,
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
					  						PMPropertyPath =  "PendingAirlines",
					  						ListPropertyPath =  "PendingAirlines",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "PendingAirlines",
					  						DefaultText =  @"Pending Airlines",
					  						HelpTextCode =  "PendingAirlines",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ReceivablesAccountingCard",
					  						ObjectTableName =  "Customer",
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
					  						ValidForQuerySection1 =  "Customer",
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
					  						DefaultText =  @"Receivables External ID",
					  						ListFieldLable =  "ReceivablesAccountingCardListLable",
					  						ListLableDefaultText =  @"Receivables External ID",
					  						HelpTextCode =  "ReceivablesAccountingCard",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PayablesAccountingCard",
					  						ObjectTableName =  "Customer",
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
					  						ValidForQuerySection1 =  "Customer",
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
					  						DefaultText =  @"Payables External ID",
					  						ListFieldLable =  "PayablesAccountingCardListLable",
					  						ListLableDefaultText =  @"Payables External ID",
					  						HelpTextCode =  "PayablesAccountingCard",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CompetitorFields",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "Competitor",
					  						Code =  "CompetitorFields",
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
					  						Operator =  "Contains",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "CompetitorFields",
					  						ListPropertyPath =  "CompetitorFields",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "CompetitorFields",
					  						DefaultText =  @"Competitor",
					  						ListFieldLable =  "CompetitorFieldsListLable",
					  						ListLableDefaultText =  @"Competitor",
					  						HelpTextCode =  "CompetitorFields",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsCreditLimitEnabled",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Boolean",
					  						Code =  "IsCreditLimitEnabled",
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
					  						PMPropertyPath =  "IsCreditLimitEnabled",
					  						ListPropertyPath =  "IsCreditLimitEnabled",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "IsCreditLimitEnabled",
					  						DefaultText =  @"Is Credit Limit Enabled",
					  						HelpTextCode =  "IsCreditLimitEnabled",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreditLimitAmount",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Double",
					  						Code =  "CreditLimitAmount",
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
					  						PMPropertyPath =  "CreditLimitAmount",
					  						ListPropertyPath =  "CreditLimitAmount",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "CreditLimitAmount",
					  						DefaultText =  @"Credit Amount",
					  						HelpTextCode =  "CreditLimitAmount",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreditLimitOpenBalance",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Double",
					  						Code =  "CreditLimitOpenBalance",
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
					  						PMPropertyPath =  "CreditLimitOpenBalance",
					  						ListPropertyPath =  "CreditLimitOpenBalance",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "CreditLimitOpenBalance",
					  						DefaultText =  @"Open Balance",
					  						HelpTextCode =  "CreditLimitOpenBalance",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CreditLimitWarningPercentage",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Integer",
					  						Code =  "CreditLimitWarningPercentage",
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
					  						PMPropertyPath =  "CreditLimitWarningPercentage",
					  						ListPropertyPath =  "CreditLimitWarningPercentage",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "CreditLimitWarningPercentage",
					  						DefaultText =  @"Warning Percentage",
					  						HelpTextCode =  "CreditLimitWarningPercentage",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ExternalAccountingBusinessArea",
					  						ObjectTableName =  "Customer",
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
					  						ValidForQuerySection1 =  "Customer",
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
					  						DefaultText =  @"Business Area",
					  						HelpTextCode =  "ExternalAccountingBusinessArea",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "PaymentMethodCode",
					  						ObjectTableName =  "Customer",
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
					  						ValidForQuerySection1 =  "Customer",
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
					  						DefaultText =  @"Forma Pago",
					  						HelpTextCode =  "PaymentMethodCode",
					  						HelpTextDefaultText =  @"Payment Method",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ActivationDate",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "DateTime",
					  						Code =  "ActivationDate",
					  						MaxLength =  1,
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
					  						PMPropertyPath =  "ActivationDate",
					  						ListPropertyPath =  "ActivationDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "ActivationDate",
					  						DefaultText =  @"Activation Date",
					  						ListFieldLable =  "ActivationDateLabel",
					  						ListLableDefaultText =  @"Activation Date",
					  						HelpTextCode =  "ActivationDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InactiveDate",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "DateTime",
					  						Code =  "InactiveDate",
					  						MaxLength =  1,
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
					  						PMPropertyPath =  "InactiveDate",
					  						ListPropertyPath =  "InactiveDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "InactiveDate",
					  						DefaultText =  @"Inactive Date",
					  						ListFieldLable =  "InactiveDateLabel",
					  						ListLableDefaultText =  @"Inactive Date",
					  						HelpTextCode =  "InactiveDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ActivationRequestDate",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "DateTime",
					  						Code =  "ActivationRequestDate",
					  						MaxLength =  1,
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
					  						PMPropertyPath =  "ActivationRequestDate",
					  						ListPropertyPath =  "ActivationRequestDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "ActivationRequestDate",
					  						DefaultText =  @"Activation Request Date",
					  						ListFieldLable =  "ActivationRequestDateLabel",
					  						ListLableDefaultText =  @"Activation Request Date",
					  						HelpTextCode =  "ActivationRequestDate",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ActivatedByUserName",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						Code =  "ActivatedByUserName",
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
					  						PMPropertyPath =  "ActivatedByUserName",
					  						ListPropertyPath =  "ActivatedByUserName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "ActivatedByUserName",
					  						DefaultText =  @"Activated by",
					  						ListFieldLable =  "ActivatedByUserNameLabel",
					  						ListLableDefaultText =  @"Activated by",
					  						HelpTextCode =  "ActivatedByUserName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SetAsInactiveByName",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						Code =  "SetAsInactiveByName",
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
					  						PMPropertyPath =  "SetAsInactiveByName",
					  						ListPropertyPath =  "SetAsInactiveByName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "SetAsInactiveByName",
					  						DefaultText =  @"Set as Inactive by",
					  						ListFieldLable =  "SetAsInactiveByNameLabel",
					  						ListLableDefaultText =  @"Set as Inactive by",
					  						HelpTextCode =  "SetAsInactiveByName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ActivationRequestedByUserName",
					  						ObjectTableName =  "Customer",
					  						FieldsDataType =  "Text",
					  						Code =  "ActivationRequestedByUserName",
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
					  						PMPropertyPath =  "ActivationRequestedByUserName",
					  						ListPropertyPath =  "ActivationRequestedByUserName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customer",
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
					  						FullFieldLable =  "ActivationRequestedByUserName",
					  						DefaultText =  @"Activation Requested by",
					  						ListFieldLable =  "ActivationRequestedByUserNameLabel",
					  						ListLableDefaultText =  @"Activation Requested by",
					  						HelpTextCode =  "ActivationRequestedByUserName",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CustomerProducts",
					  						ObjectTableName =  "Customer",
					  						Code =  "CustomerProducts",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "CustomerProducts",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  true,
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
					  						MultiTableName =  "CustomerProduct",
					  						FullFieldLable =  "CustomerProducts",
					  						DefaultText =  @"Customer Products",
					  						HelpTextCode =  "CustomerProducts",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	    {  
	        FeatureRepository featureRepository = new FeatureRepository(0); 
            List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
            IWebFreightContext objectContext = WebFreightContext.GetContext(0);  
	        QueryGroup CustomerQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "CLNT", Name = "Customers" }, queryGroupRepository);
						QueryGroup CustomerQueryGroup1 = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "9eaa", Name = " Query Group" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable CustomerObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Customer" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> CustomerObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Customer").ToList();   

			   TextCode CustomerTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.Q.CustomersByLastShipment", DefaultText = @"By Last Shipment",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature CustomerFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BYLASTSHIPMETN", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.ByLastShipment", NameTextCodeDefaultText = "By Last Shipment", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode CustomerTextCode_1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "ShippersAndConsignees.Q.ShippersAndConsignees", DefaultText = @"Shippers and Consignees",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature CustomerFeature_1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BYLASTSHIPMETN", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.ByLastShipment", NameTextCodeDefaultText = "By Last Shipment", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode CustomerTextCode_2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.Q.SharedLogisticsCustomers", DefaultText = @"Shared Logistics Customers",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature CustomerFeature_2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SHAREDLOGISTICSCUSTOMERS", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.SharedLogisticsCustomers", NameTextCodeDefaultText = "Shared Logistics Customers", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode CustomerTextCode_3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.Q.Customers", DefaultText = @"Customers",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature CustomerFeature_3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CUSTOMERS", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.Customers", NameTextCodeDefaultText = "Customers", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode CustomerTextCode_4 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.Q.MyCustomersAccMngr", DefaultText = @"My Customers (as Account Manager)",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature CustomerFeature_4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MYCUSTOMRERSASACCMNGR", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.MyCustomersAsAccMngr", NameTextCodeDefaultText = "My customers (as Account Manager)", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode CustomerTextCode_5 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.Q.MyCustomers", DefaultText = @"My Customers (as Salesman)",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature CustomerFeature_5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Customer.Feature.MyCustomers", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.MyCustomers", NameTextCodeDefaultText = "My customers (as Salesman)", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode CustomerTextCode_6 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.Q.ReadyCustomers", DefaultText = @"Waiting for Activation",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature CustomerFeature_6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Customer.Feature.ReadyCustomers", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.ReadyCustomers", NameTextCodeDefaultText = "Waiting for Activation", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode CustomerTextCode_7 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.Q.PotentialCustomers", DefaultText = @"Potential Customers",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature CustomerFeature_7 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Customer.Feature.PotentialCustomers", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.PotentialCustomers", NameTextCodeDefaultText = "Potential Customers", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode CustomerTextCode_8 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.Q.ActiveCustomers", DefaultText = @"Active Customers",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature CustomerFeature_8 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Customer.Feature.ActiveCustomers", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.ActiveCustomers", NameTextCodeDefaultText = "Active Customer", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 

			   TextCode CustomerTextCode_9 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.Q.InactiveCustomers", DefaultText = @"Inactive Customers",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature CustomerFeature_9 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Customer.Feature.InactiveCustomers", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.InactiveCustomers", NameTextCodeDefaultText = "Inactive Customers", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query ByLastShipmentQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = CustomerTextCode_0.Id, Code = "By Last Shipment",  QueryGroupCode = "CLNT", IndexOrder = 0, Tenant = 0, ObjectTableId = CustomerObjectTable.Id, QuerySection = "Customer", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = CustomerFeature_0.Id, DefaultSortName = "LastShipmentDate", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
	
			 QueryColumn ByLastShipmentQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ByLastShipmentQuery.Id, IndexOrder = 0, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "RankCode" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ByLastShipmentQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ByLastShipmentQuery.Id, IndexOrder = 1, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ByLastShipmentQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ByLastShipmentQuery.Id, IndexOrder = 2, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ByLastShipmentQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ByLastShipmentQuery.Id, IndexOrder = 3, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "CityName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ByLastShipmentQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ByLastShipmentQuery.Id, IndexOrder = 4, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "SalesmanUserEnglishName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ByLastShipmentQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ByLastShipmentQuery.Id, IndexOrder = 5, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "StartWorkingDate" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ByLastShipmentQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ByLastShipmentQuery.Id, IndexOrder = 6, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "LastShipmentDate" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter ByLastShipmentQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "IsCustomer" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true",PredefinedValue2 = null, QueryId = ByLastShipmentQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query ShippersAndConsigneesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = CustomerTextCode_1.Id, Code = "ShippersAndConsignees",  QueryGroupCode = "CLNT", IndexOrder = 0, Tenant = 0, ObjectTableId = CustomerObjectTable.Id, QuerySection = "Customer", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = CustomerFeature_1.Id, DefaultSortName = null, DefaultSortDirection = null }, queriesRepository, tenantQueries);
	
			 QueryColumn ShippersAndConsigneesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ShippersAndConsigneesQuery.Id, IndexOrder = 0, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ShippersAndConsigneesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ShippersAndConsigneesQuery.Id, IndexOrder = 1, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ShippersAndConsigneesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ShippersAndConsigneesQuery.Id, IndexOrder = 2, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ShippersAndConsigneesQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ShippersAndConsigneesQuery.Id, IndexOrder = 3, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "VatNumber" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ShippersAndConsigneesQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ShippersAndConsigneesQuery.Id, IndexOrder = 4, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "ReceivablesAccountingCard" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ShippersAndConsigneesQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ShippersAndConsigneesQuery.Id, IndexOrder = 5, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "AccountManagerUserEnglishName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ShippersAndConsigneesQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ShippersAndConsigneesQuery.Id, IndexOrder = 6, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "SalesmanUserEnglishName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ShippersAndConsigneesQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ShippersAndConsigneesQuery.Id, IndexOrder = 7, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "RankName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ShippersAndConsigneesQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ShippersAndConsigneesQuery.Id, IndexOrder = 8, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "Website" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ShippersAndConsigneesQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ShippersAndConsigneesQuery.Id, IndexOrder = 9, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ShippersAndConsigneesQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ShippersAndConsigneesQuery.Id, IndexOrder = 10, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "Notes" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ShippersAndConsigneesQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ShippersAndConsigneesQuery.Id, IndexOrder = 11, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "BillToName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn ShippersAndConsigneesQueryColumn_12 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = ShippersAndConsigneesQuery.Id, IndexOrder = 12, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "IsCustomer" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
  
	      

			  Query SharedLogisticsCustomersQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = CustomerTextCode_2.Id, Code = "Shared Logistics Customers",  EditWizardName = "SharedLogistics.Views.InviteCustomersControl",
			   QueryGroupCode = "CLNT", IndexOrder = 1, Tenant = 0, ObjectTableId = CustomerObjectTable.Id, QuerySection = "Customer", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = CustomerFeature_2.Id, DefaultSortName = "SharedLogisticsInvitationStatusName", DefaultSortDirection = "Descending" }, queriesRepository, tenantQueries);
	
			 QueryColumn SharedLogisticsCustomersQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SharedLogisticsCustomersQuery.Id, IndexOrder = 0, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "SharedLogisticsInvitationStatusName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn SharedLogisticsCustomersQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SharedLogisticsCustomersQuery.Id, IndexOrder = 1, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn SharedLogisticsCustomersQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SharedLogisticsCustomersQuery.Id, IndexOrder = 1, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "RankCode" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn SharedLogisticsCustomersQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SharedLogisticsCustomersQuery.Id, IndexOrder = 2, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn SharedLogisticsCustomersQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SharedLogisticsCustomersQuery.Id, IndexOrder = 3, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn SharedLogisticsCustomersQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SharedLogisticsCustomersQuery.Id, IndexOrder = 4, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "LastShipmentDate" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn SharedLogisticsCustomersQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SharedLogisticsCustomersQuery.Id, IndexOrder = 5, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "StartWorkingDate" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn SharedLogisticsCustomersQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = SharedLogisticsCustomersQuery.Id, IndexOrder = 6, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "LastLoginDate" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter SharedLogisticsCustomersQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "SharedLogisticsCustomers" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true",PredefinedValue2 = null, QueryId = SharedLogisticsCustomersQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query CustomersQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = CustomerTextCode_3.Id, Code = "Customers",  QueryGroupCode = "CLNT", IndexOrder = 2, Tenant = 0, ObjectTableId = CustomerObjectTable.Id, QuerySection = "Customer", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = CustomerFeature_3.Id, DefaultSortName = null, DefaultSortDirection = null }, queriesRepository, tenantQueries);
	
			 QueryColumn CustomersQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomersQuery.Id, IndexOrder = 0, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CustomersQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomersQuery.Id, IndexOrder = 1, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CustomersQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomersQuery.Id, IndexOrder = 2, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CustomersQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomersQuery.Id, IndexOrder = 3, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "VatNumber" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CustomersQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomersQuery.Id, IndexOrder = 4, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "ReceivablesAccountingCard" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CustomersQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomersQuery.Id, IndexOrder = 5, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "AccountManagerUserEnglishName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CustomersQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomersQuery.Id, IndexOrder = 6, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "SalesmanUserEnglishName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CustomersQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomersQuery.Id, IndexOrder = 7, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "RankName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CustomersQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomersQuery.Id, IndexOrder = 8, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "Website" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CustomersQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomersQuery.Id, IndexOrder = 9, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CustomersQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomersQuery.Id, IndexOrder = 10, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "Notes" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn CustomersQueryColumn_11 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = CustomersQuery.Id, IndexOrder = 11, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "BillToName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter CustomersQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "IsCustomer" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true",PredefinedValue2 = null, QueryId = CustomersQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query Customer_Q_MyCustomersAccMngrQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = CustomerTextCode_4.Id, Code = "Customer.Q.MyCustomersAccMngr",  QueryGroupCode = "CLNT", IndexOrder = 3, Tenant = 0, ObjectTableId = CustomerObjectTable.Id, QuerySection = "Customer", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = CustomerFeature_4.Id, DefaultSortName = null, DefaultSortDirection = null }, queriesRepository, tenantQueries);
	
			 QueryColumn Customer_Q_MyCustomersAccMngrQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_Q_MyCustomersAccMngrQuery.Id, IndexOrder = 0, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "RankCode" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_Q_MyCustomersAccMngrQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_Q_MyCustomersAccMngrQuery.Id, IndexOrder = 1, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_Q_MyCustomersAccMngrQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_Q_MyCustomersAccMngrQuery.Id, IndexOrder = 2, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_Q_MyCustomersAccMngrQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_Q_MyCustomersAccMngrQuery.Id, IndexOrder = 3, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_Q_MyCustomersAccMngrQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_Q_MyCustomersAccMngrQuery.Id, IndexOrder = 4, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_Q_MyCustomersAccMngrQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_Q_MyCustomersAccMngrQuery.Id, IndexOrder = 5, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "CityName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_Q_MyCustomersAccMngrQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_Q_MyCustomersAccMngrQuery.Id, IndexOrder = 6, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "StartWorkingDate" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_Q_MyCustomersAccMngrQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_Q_MyCustomersAccMngrQuery.Id, IndexOrder = 7, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "LastShipmentDate" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_Q_MyCustomersAccMngrQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_Q_MyCustomersAccMngrQuery.Id, IndexOrder = 8, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "LastLoginDate" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_Q_MyCustomersAccMngrQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_Q_MyCustomersAccMngrQuery.Id, IndexOrder = 9, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "SharedLogisticsInvitationStatusName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_Q_MyCustomersAccMngrQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_Q_MyCustomersAccMngrQuery.Id, IndexOrder = 10, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "SalesmanUserEnglishName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter Customer_Q_MyCustomersAccMngrQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "MyCustomersAsAccountManager" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true",PredefinedValue2 = null, QueryId = Customer_Q_MyCustomersAccMngrQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);


             AdvancedQueryFilter Customer_Q_MyCustomersAccMngrQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "IsCustomer" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true",PredefinedValue2 = null, QueryId = Customer_Q_MyCustomersAccMngrQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query Customer_MyCustomersQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = CustomerTextCode_5.Id, Code = "Customer.MyCustomers",  QueryGroupCode = "CLNT", IndexOrder = 3, Tenant = 0, ObjectTableId = CustomerObjectTable.Id, QuerySection = "Customer", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = CustomerFeature_5.Id, DefaultSortName = null, DefaultSortDirection = null }, queriesRepository, tenantQueries);
	
			 QueryColumn Customer_MyCustomersQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_MyCustomersQuery.Id, IndexOrder = 0, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "RankCode" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_MyCustomersQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_MyCustomersQuery.Id, IndexOrder = 1, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_MyCustomersQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_MyCustomersQuery.Id, IndexOrder = 2, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_MyCustomersQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_MyCustomersQuery.Id, IndexOrder = 3, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_MyCustomersQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_MyCustomersQuery.Id, IndexOrder = 4, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "CreateDate" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_MyCustomersQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_MyCustomersQuery.Id, IndexOrder = 5, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "CityName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_MyCustomersQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_MyCustomersQuery.Id, IndexOrder = 6, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "StartWorkingDate" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_MyCustomersQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_MyCustomersQuery.Id, IndexOrder = 7, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "LastShipmentDate" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_MyCustomersQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_MyCustomersQuery.Id, IndexOrder = 8, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "LastLoginDate" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_MyCustomersQueryColumn_9 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_MyCustomersQuery.Id, IndexOrder = 9, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "SharedLogisticsInvitationStatusName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_MyCustomersQueryColumn_10 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_MyCustomersQuery.Id, IndexOrder = 10, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "SalesmanUserEnglishName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter Customer_MyCustomersQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "MyCustomers" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true",PredefinedValue2 = null, QueryId = Customer_MyCustomersQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);


             AdvancedQueryFilter Customer_MyCustomersQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "IsCustomer" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true",PredefinedValue2 = null, QueryId = Customer_MyCustomersQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query Customer_ReadyCustomersQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = CustomerTextCode_6.Id, Code = "Customer.ReadyCustomers",  QueryGroupCode = "CLNT", IndexOrder = 5, Tenant = 0, ObjectTableId = CustomerObjectTable.Id, QuerySection = "Customer", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = CustomerFeature_6.Id, DefaultSortName = null, DefaultSortDirection = null }, queriesRepository, tenantQueries);
	
			 QueryColumn Customer_ReadyCustomersQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_ReadyCustomersQuery.Id, IndexOrder = 0, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "RankCode" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_ReadyCustomersQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_ReadyCustomersQuery.Id, IndexOrder = 1, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_ReadyCustomersQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_ReadyCustomersQuery.Id, IndexOrder = 2, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_ReadyCustomersQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_ReadyCustomersQuery.Id, IndexOrder = 3, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_ReadyCustomersQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_ReadyCustomersQuery.Id, IndexOrder = 4, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "ReadyForActivationDate" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_ReadyCustomersQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_ReadyCustomersQuery.Id, IndexOrder = 5, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "CityName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_ReadyCustomersQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_ReadyCustomersQuery.Id, IndexOrder = 6, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "StartWorkingDate" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_ReadyCustomersQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_ReadyCustomersQuery.Id, IndexOrder = 7, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "LastShipmentDate" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_ReadyCustomersQueryColumn_8 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_ReadyCustomersQuery.Id, IndexOrder = 8, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "SalesmanUserEnglishName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter Customer_ReadyCustomersQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "ReadyCustomers" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true",PredefinedValue2 = null, QueryId = Customer_ReadyCustomersQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);


             AdvancedQueryFilter Customer_ReadyCustomersQueryFilter_1 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "IsCustomer" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true",PredefinedValue2 = null, QueryId = Customer_ReadyCustomersQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query Customer_PotentialCustomersQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = CustomerTextCode_7.Id, Code = "Customer.PotentialCustomers",  QueryGroupCode = "CLNT", IndexOrder = 6, Tenant = 0, ObjectTableId = CustomerObjectTable.Id, QuerySection = "Customer", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = CustomerFeature_7.Id, DefaultSortName = null, DefaultSortDirection = null }, queriesRepository, tenantQueries);
	
			 QueryColumn Customer_PotentialCustomersQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_PotentialCustomersQuery.Id, IndexOrder = 0, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "RankCode" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_PotentialCustomersQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_PotentialCustomersQuery.Id, IndexOrder = 1, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_PotentialCustomersQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_PotentialCustomersQuery.Id, IndexOrder = 2, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_PotentialCustomersQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_PotentialCustomersQuery.Id, IndexOrder = 3, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_PotentialCustomersQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_PotentialCustomersQuery.Id, IndexOrder = 4, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "CityName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_PotentialCustomersQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_PotentialCustomersQuery.Id, IndexOrder = 5, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "StartWorkingDate" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_PotentialCustomersQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_PotentialCustomersQuery.Id, IndexOrder = 6, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "LastShipmentDate" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_PotentialCustomersQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_PotentialCustomersQuery.Id, IndexOrder = 7, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "SalesmanUserEnglishName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter Customer_PotentialCustomersQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "PotentialCustomers" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true",PredefinedValue2 = null, QueryId = Customer_PotentialCustomersQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query Customer_ActiveCustomersQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = CustomerTextCode_8.Id, Code = "Customer.ActiveCustomers",  QueryGroupCode = "CLNT", IndexOrder = 7, Tenant = 0, ObjectTableId = CustomerObjectTable.Id, QuerySection = "Customer", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = CustomerFeature_8.Id, DefaultSortName = null, DefaultSortDirection = null }, queriesRepository, tenantQueries);
	
			 QueryColumn Customer_ActiveCustomersQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_ActiveCustomersQuery.Id, IndexOrder = 0, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "RankCode" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_ActiveCustomersQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_ActiveCustomersQuery.Id, IndexOrder = 1, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_ActiveCustomersQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_ActiveCustomersQuery.Id, IndexOrder = 2, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_ActiveCustomersQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_ActiveCustomersQuery.Id, IndexOrder = 3, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_ActiveCustomersQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_ActiveCustomersQuery.Id, IndexOrder = 4, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "CityName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_ActiveCustomersQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_ActiveCustomersQuery.Id, IndexOrder = 5, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "StartWorkingDate" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_ActiveCustomersQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_ActiveCustomersQuery.Id, IndexOrder = 6, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "LastShipmentDate" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_ActiveCustomersQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_ActiveCustomersQuery.Id, IndexOrder = 7, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "SalesmanUserEnglishName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter Customer_ActiveCustomersQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "ActiveCustomers" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true",PredefinedValue2 = null, QueryId = Customer_ActiveCustomersQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

  
	      

			  Query Customer_InactiveCustomersQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = CustomerTextCode_9.Id, Code = "Customer.InactiveCustomers",  QueryGroupCode = "CLNT", IndexOrder = 8, Tenant = 0, ObjectTableId = CustomerObjectTable.Id, QuerySection = "Customer", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = CustomerFeature_9.Id, DefaultSortName = null, DefaultSortDirection = null }, queriesRepository, tenantQueries);
	
			 QueryColumn Customer_InactiveCustomersQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_InactiveCustomersQuery.Id, IndexOrder = 0, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "RankCode" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_InactiveCustomersQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_InactiveCustomersQuery.Id, IndexOrder = 1, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_InactiveCustomersQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_InactiveCustomersQuery.Id, IndexOrder = 2, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_InactiveCustomersQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_InactiveCustomersQuery.Id, IndexOrder = 3, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_InactiveCustomersQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_InactiveCustomersQuery.Id, IndexOrder = 4, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "CityName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_InactiveCustomersQueryColumn_5 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_InactiveCustomersQuery.Id, IndexOrder = 5, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "StartWorkingDate" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_InactiveCustomersQueryColumn_6 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_InactiveCustomersQuery.Id, IndexOrder = 6, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "LastShipmentDate" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn Customer_InactiveCustomersQueryColumn_7 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = Customer_InactiveCustomersQuery.Id, IndexOrder = 7, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "SalesmanUserEnglishName" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

             AdvancedQueryFilter Customer_InactiveCustomersQueryFilter_0 = AddQueries.AddAdvancedQueryFilter(new AdvancedFilterDetails() { IsPredefined = true, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "InactiveCustomers" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, PredefinedValue = "true",PredefinedValue2 = null, QueryId = Customer_InactiveCustomersQuery.Id, Tenant = 0 }, advancedQueryFiltersRepository, tenantAdvancedFilters);

	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable CustomerObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customer" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> CustomerObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Customer").ToList();
		       
	      

	         Screen CustomerHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Customer.HeaderScreen", Name = "Header Screen", ObjectTableId = CustomerObjectTable.Id, NumberOfColumns = 4, NumberOfRows = 2, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField CustomerCustomerHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "CodeMyCustomer").FirstOrDefault().Id, ScreenId = CustomerHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomerCustomerHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "CityWithCountry").FirstOrDefault().Id, ScreenId = CustomerHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomerCustomerHeaderScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "StartWorkingDate").FirstOrDefault().Id, ScreenId = CustomerHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomerCustomerHeaderScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "LeadSourceName").FirstOrDefault().Id, ScreenId = CustomerHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomerCustomerHeaderScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 0, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "PrimaryContactName").FirstOrDefault().Id, ScreenId = CustomerHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomerCustomerHeaderScreenScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 2, Row = 1, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "PrimaryContactPhone").FirstOrDefault().Id, ScreenId = CustomerHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomerCustomerHeaderScreenScreenField6 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 0, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "SalesmanUserEnglishName").FirstOrDefault().Id, ScreenId = CustomerHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomerCustomerHeaderScreenScreenField7 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 3, Row = 1, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "CustomerStatusName").FirstOrDefault().Id, ScreenId = CustomerHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    CustomerObjectTable.HeaderScreenId = CustomerHeaderScreenScreen0.Id;
	   		  
	      

	         Screen CustomerBillingTabScreenScreen1 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Customer.BillingTabScreen", Name = "Billing Tab Screen", ObjectTableId = CustomerObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 7, IsReadOnly = false }, screensRepository, tenantScreens);
      
            ScreenField CustomerCustomerBillingTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "InvoiceCurrencyId").FirstOrDefault().Id, ScreenId = CustomerBillingTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomerCustomerBillingTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "PaymentTermId").FirstOrDefault().Id, ScreenId = CustomerBillingTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomerCustomerBillingTabScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "VatTypeId").FirstOrDefault().Id, ScreenId = CustomerBillingTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomerCustomerBillingTabScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 3, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "EnableConsolidationInvoices").FirstOrDefault().Id, ScreenId = CustomerBillingTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomerCustomerBillingTabScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 4, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "BankName").FirstOrDefault().Id, ScreenId = CustomerBillingTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomerCustomerBillingTabScreenScreenField5 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "BankAddress").FirstOrDefault().Id, ScreenId = CustomerBillingTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomerCustomerBillingTabScreenScreenField6 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 1, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "Swift").FirstOrDefault().Id, ScreenId = CustomerBillingTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomerCustomerBillingTabScreenScreenField7 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 2, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "AccountNumber").FirstOrDefault().Id, ScreenId = CustomerBillingTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomerCustomerBillingTabScreenScreenField8 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 3, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "IBANNumber").FirstOrDefault().Id, ScreenId = CustomerBillingTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
           
	      

	         Screen CustomerAccountingTabScreenScreen2 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Customer.AccountingTabScreen", Name = "Accounting Tab Screen", ObjectTableId = CustomerObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 2, IsReadOnly = false }, screensRepository, tenantScreens);
      
            ScreenField CustomerCustomerAccountingTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "ReceivablesAccountingCard").FirstOrDefault().Id, ScreenId = CustomerAccountingTabScreenScreen2.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomerCustomerAccountingTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = CustomerObjectFields.Where(d => d.FieldName == "PayablesAccountingCard").FirstOrDefault().Id, ScreenId = CustomerAccountingTabScreenScreen2.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
           

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {    
			 ObjectTable CustomerObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customer" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode CustomerSalesTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.TH.Sales", DefaultText = "Sales",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CustomerSalesFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SALES", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.Sales", NameTextCodeDefaultText = "Sales", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode CustomerOverviewTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.TH.Overview", DefaultText = "Overview",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CustomerOverviewFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OVERVIEW", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.Overview", NameTextCodeDefaultText = "Overview", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode CustomerGeneralTextCode_TH2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.TH.General", DefaultText = "General",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CustomerGeneralFeature_TH2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GENERAL", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode CustomerStatisticsTextCode_TH3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.TH.Statistics", DefaultText = "Statistics",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CustomerStatisticsFeature_TH3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "STATISTICS", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.Statistics", NameTextCodeDefaultText = "Statistics", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode CustomerBillingTextCode_TH4 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.TH.Billing", DefaultText = "Billing",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CustomerBillingFeature_TH4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BILLING", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.Billing", NameTextCodeDefaultText = "Billing", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode CustomerAccountingTextCode_TH5 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.TH.Accounting", DefaultText = "Accounting",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CustomerAccountingFeature_TH5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACCOUNTINGTRANSFERE", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.AccountingTransfere", NameTextCodeDefaultText = "Accounting Transfere", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode CustomerAWBStockTextCode_TH6 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.TH.AWBStock", DefaultText = "AWB Stock",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CustomerAWBStockFeature_TH6 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AWBSTOCK", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.AWBSTOCK", NameTextCodeDefaultText = "AWB Stock", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode CustomerAccountingTextCode_TH7 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.TH.Accounting_2", DefaultText = "Accounting",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CustomerAccountingFeature_TH7 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACCOUNTING_2", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.Accounting_2", NameTextCodeDefaultText = "Accounting 2", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode CustomerAddressesTextCode_TH8 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.TH.Addresses", DefaultText = "Addresses",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CustomerAddressesFeature_TH8 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ADDRESSES", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.Addresses", NameTextCodeDefaultText = "Addresses", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode CustomerContactsTextCode_TH9 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.TH.Contacts", DefaultText = "Contacts",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CustomerContactsFeature_TH9 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CONTACTS", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.Contacts", NameTextCodeDefaultText = "Contacts", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode CustomerCommitmentTextCode_TH10 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.TH.Commitments", DefaultText = "Commitment",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CustomerCommitmentFeature_TH10 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "COMMITMENT", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.Commitment", NameTextCodeDefaultText = "Commitment", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode CustomerProductsTextCode_TH11 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.TH.Products", DefaultText = "Products",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CustomerProductsFeature_TH11 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "POTENTIAL", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.Potential", NameTextCodeDefaultText = "Potential", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode CustomerDocsOutTextCode_TH12 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.TH.DocsOut", DefaultText = "Docs Out",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CustomerDocsOutFeature_TH12 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DOCSOUT", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.DocsOut", NameTextCodeDefaultText = "Docs Out", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode CustomerDocsInTextCode_TH13 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.TH.DocsIn", DefaultText = "Docs In",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CustomerDocsInFeature_TH13 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DOCSIN", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.DocsIn", NameTextCodeDefaultText = "Docs In", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode CustomerEventsTextCode_TH14 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.TH.Events", DefaultText = "Events",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CustomerEventsFeature_TH14 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EVENTS", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CLSL",HtmlComponentName = "",HtmlComponentUrl = "./CommonModules/CommonCustomer/Components/EditTabs/CustomerSalesTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "SALES" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.FreightLib.Views.PartnersTabs.CustomerSalesTabControl", ObjectTableId = CustomerObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Customer.TH.Sales" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CLOV",HtmlComponentName = "",HtmlComponentUrl = "./CommonModules/CommonCustomer/Components/EditTabs/CustomerOverviewTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "OVERVIEW" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.FreightLib.Views.PartnersTabs.CustomerOverviewTab.CustomerOverviewControl", ObjectTableId = CustomerObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Customer.TH.Overview" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CLGC",HtmlComponentName = "",HtmlComponentUrl = "./CommonModules/CommonCustomer/Components/EditTabs/CustomerGeneralTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "GENERAL" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.FreightLib.Views.PartnersTabs.CustomerGeneralTabControl", ObjectTableId = CustomerObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Customer.TH.General" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 2 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CLST",HtmlComponentName = "",HtmlComponentUrl = "./CommonModules/CommonCustomer/Components/EditTabs/CustomerStatisticsTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "STATISTICS" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.FreightLib.Views.PartnersTabs.CustomerStatisticsTabControl", ObjectTableId = CustomerObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Customer.TH.Statistics" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 3 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CLBL",HtmlComponentName = "",HtmlComponentUrl = "./CommonModules/CommonCustomer/Components/EditTabs/CustomerBillingTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "BILLING" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.Infrastructure.GeneralControls.BillingTabControl", ObjectTableId = CustomerObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Customer.TH.Billing" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 4 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CLAC",HtmlComponentName = "",HtmlComponentUrl = "./Common/Components/AccountingTab/AccountingTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "ACCOUNTINGTRANSFERE" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.FreightLib.Views.PartnersTabs.PartnersAccountingTab.CustomerAccountingTabControl", ObjectTableId = CustomerObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Customer.TH.Accounting" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 5 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CLAW",HtmlComponentName = "",HtmlComponentUrl = "./CommonModules/CommonCustomer/Components/EditTabs/CustomerAWBStockTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "AWBSTOCK" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.FreightLib.Views.PartnersTabs.CustomerAWBStockTab", ObjectTableId = CustomerObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Customer.TH.AWBStock" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 6 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CSAC",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = tenantFeatures.Where(d => d.Code == "ACCOUNTING_2" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.FreightLib.Views.PartnersTabs.CustomerCRMTabs.CustomerSecondAccountingTabControl", ObjectTableId = CustomerObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Customer.TH.Accounting_2" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 7 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CLAD",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = tenantFeatures.Where(d => d.Code == "ADDRESSES" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.FreightLib.Views.PartnersTabs.PartnerAddressesTab", ObjectTableId = CustomerObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Customer.TH.Addresses" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 8 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CLCO",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = tenantFeatures.Where(d => d.Code == "CONTACTS" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.FreightLib.Views.PartnersTabs.PartnerContactsTab", ObjectTableId = CustomerObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Customer.TH.Contacts" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 9 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CLCT",HtmlComponentName = "",HtmlComponentUrl = "./CommonModules/CommonCustomer/Components/EditTabs/CustomerCommitmentsTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "COMMITMENT" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.FreightLib.Views.PartnersTabs.CustomerCRMTabs.ProductCommitmentTabControl", ObjectTableId = CustomerObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Customer.TH.Commitments" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 10 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CLPO",HtmlComponentName = "",HtmlComponentUrl = "./CommonModules/CommonCustomer/Components/EditTabs/CustomerProductsTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "POTENTIAL" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.FreightLib.Views.PartnersTabs.CustomerCRMTabs.ProductPotentialTabControl", ObjectTableId = CustomerObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Customer.TH.Products" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 11 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CLDO",HtmlComponentName = "",HtmlComponentUrl = "./CommonModules/CommonCustomer/Components/EditTabs/CustomerDocsOutTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "DOCSOUT" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.FreightLib.Views.PartnersTabs.CustomerCRMTabs.CustomerDocsOutTabControl", ObjectTableId = CustomerObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Customer.TH.DocsOut" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 12 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CLDI",HtmlComponentName = "",HtmlComponentUrl = "./CommonModules/CommonCustomer/Components/EditTabs/CustomerDocsInTabComponent", FeatureId = tenantFeatures.Where(d => d.Code == "DOCSIN" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.FreightLib.Views.PartnersTabs.CustomerCRMTabs.CustomerDocsInTabControl", ObjectTableId = CustomerObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Customer.TH.DocsIn" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 13 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CSEV",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = tenantFeatures.Where(d => d.Code == "EVENTS" && d.ObjectTableId == CustomerObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = CustomerObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Customer.TH.Events" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 14 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable CustomerObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customer" && d.Tenant == 0).FirstOrDefault(); 
		   Feature CustomerFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature CustomerFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature CustomerFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature CustomerFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.PackageFeature", NameTextCodeDefaultText = "Customer Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes); 		   
		   Feature  CustomerExcelFeature = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Customer.Feature.EXCEL", Packagable = true, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.EXCEL", NameTextCodeDefaultText = "Download to Excel", FeatureTypeCode = "ACT" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);



		   		   //--------------> Additional Features <--------------\\

		   Feature CustomerFeature_SHAREDLOGISTIC = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SHAREDLOGISTIC", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.SharedLogistic", NameTextCodeDefaultText = @"Shared Logistic" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature CustomerFeature_CUSTOMERSALESMANBYPRODUCT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CUSTOMERSALESMANBYPRODUCT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.CustomerSalesmanByProduct", NameTextCodeDefaultText = @"Split Customer Salesman By Product" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature CustomerFeature_CUSTOMERACCOUNTMANAGERBYPRODUCT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CUSTOMERACCOUNTMANAGERBYPRODUCT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.CustomerAccountManagerByProduct", NameTextCodeDefaultText = @"Split Customer Account Manager By Product" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature CustomerFeature_CUSTOMERFORWARDERBYPRODUCT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CUSTOMERFORWARDERBYPRODUCT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.CustomerForwarderByProduct", NameTextCodeDefaultText = @"Split Customer Forwarder By Product" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature CustomerFeature_CUSTOMERMEDIATORBYPRODUCT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CUSTOMERMEDIATORBYPRODUCT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.CustomerMediatorByProduct", NameTextCodeDefaultText = @"Split Customer Mediator By Product" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature CustomerFeature_CUSTOMERCUSTOMSAGENTBYPRODUCT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CUSTOMERCUSTOMSAGENTBYPRODUCT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.CustomerCustomsAgentByProduct", NameTextCodeDefaultText = @"Split Customer Customs Agent By Product" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature CustomerFeature_QUICKSEARCHALL = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "QUICKSEARCHALL", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.QuickSearchAll", NameTextCodeDefaultText = @"Show all customers in quick search" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature CustomerFeature_MAINCUSTOMERS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MAINCUSTOMERS", FeatureTypeCode = "QUER", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.MaintenanceCustomers", NameTextCodeDefaultText = @"Maintenance Customers" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature CustomerFeature_NEWCUSTOMER = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEWCUSTOMER", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.NewCustomer", NameTextCodeDefaultText = @"New Customer" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature CustomerFeature_CREATETENANT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CREATETENANT", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.CreateTenant", NameTextCodeDefaultText = @"Create Tenant" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature CustomerFeature_OUTLOOKCONNETION = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OUTLOOKCONNETION", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.OutlookConnection", NameTextCodeDefaultText = @"Outlook Connection" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature CustomerFeature_Customer_Feature_EXCEL = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Customer.Feature.EXCEL", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.EXCEL", NameTextCodeDefaultText = @"Download to Excel" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature CustomerFeature_CREATEACTIVECUSTOMER = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CREATEACTIVECUSTOMER", FeatureTypeCode = "ACT", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.CreateActiveCustomer", NameTextCodeDefaultText = @"Allow creating active customers" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

   
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable CustomerObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customer" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "SPOT",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Set as Potential",
                EnglishName =  "Set as Potential",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CustomerObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "UPCU",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Customer Updated",
                EnglishName =  "Customer Updated",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CustomerObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CRCU",
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
                ObjectTableId = CustomerObjectTable.Id,
                EntityStatusId = AllEntityStatuses.Where(d => d.Tenant == 0 && d.Code == "CSCR").FirstOrDefault().Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CSWA",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "To be Activated",
                EnglishName =  "To be Activated",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CustomerObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CSAV",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Customer Activated",
                EnglishName =  "Customer Activated",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CustomerObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CSIN",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Customer Deactivated",
                EnglishName =  "Customer Deactivated",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CustomerObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CSRA",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Customer Reactivated",
                EnglishName =  "Customer Reactivated",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CustomerObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CSMC",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Set as My Customer",
                EnglishName =  "Set as My Customer",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CustomerObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CSNC",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Set as Not My Customer",
                EnglishName =  "Set as Not My Customer",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CustomerObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "POTC",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Created as Potential",
                EnglishName =  "Created as Potential",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CustomerObjectTable.Id,
                EntityStatusId = AllEntityStatuses.Where(d => d.Tenant == 0 && d.Code == "CSCR").FirstOrDefault().Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CEMO",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Email out sent",
                EnglishName =  "Customer email out sent",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CustomerObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "SLCH",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Salesman Changed",
                EnglishName =  "Salesman Changed",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CustomerObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "PRUP",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Products Updated",
                EnglishName =  "Products Updated",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CustomerObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CMUP",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Competitors Updated",
                EnglishName =  "Competitors Updated",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CustomerObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "ADUP",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Additional Services Updated",
                EnglishName =  "Additional Services Updated",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CustomerObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "ARUP",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Addresses Updated",
                EnglishName =  "Addresses Updated",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CustomerObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CNUP",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Contacts Updated",
                EnglishName =  "Contacts Updated",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CustomerObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CACR",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Activity Created",
                EnglishName =  "Activity Created",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CustomerObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CACM",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Activity Completed",
                EnglishName =  "Activity Completed",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CustomerObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CARP",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Activity Reopened",
                EnglishName =  "Activity Reopened",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CustomerObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "PWEN",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Activity Watch Enabled",
                EnglishName =  "Activity Watch Enabled",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CustomerObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "PWDS",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Activity Watch Disabled",
                EnglishName =  "Activity Watch Disabled",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CustomerObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CRDL",
                ShortView =  true,
                IsManualEntry =  false,
                LocalName =  "Credit limit details updated",
                EnglishName =  "Credit limit details updated",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = CustomerObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
		   FeatureRepository featureRepository = new FeatureRepository(0); 
		   List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList(); 
		   ObjectTable CustomerObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customer" && d.Tenant == 0).FirstOrDefault();       
    
			   Feature CustomerFeature_MB0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACTIVATE", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.Activate", NameTextCodeDefaultText = "Activate", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

      
    
			   Feature CustomerFeature_MB1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READYACTIVATION", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.ReadyForActivation", NameTextCodeDefaultText = "Ready For Activation", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

      
    
			   Feature CustomerFeature_MB2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TOTANGO", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.Totango", NameTextCodeDefaultText = "Totango", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

      
    
			   Feature CustomerFeature_MB3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TENANTMANAGEMENT", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.TenantManagement", NameTextCodeDefaultText = "Tenant Management", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

      
    
			   Feature CustomerFeature_MB4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READYACTIVATION", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.ReadyForActivation", NameTextCodeDefaultText = "Ready For Activation", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

      
    
			   Feature CustomerFeature_MB5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Actions", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.More", NameTextCodeDefaultText = "More", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

			   Feature CustomerFeature_MB50 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "INACTIVE", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.InActive", NameTextCodeDefaultText = "InActive", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
             			   Feature CustomerFeature_MB51 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "REACTIVATE", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.ReActivate", NameTextCodeDefaultText = "ReActivate", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
             			   Feature CustomerFeature_MB52 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MYCUSTOMER", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.SetMyCustomer", NameTextCodeDefaultText = "Set as Customer", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
             			   Feature CustomerFeature_MB53 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SETASPOTENTIAL", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.SetAsPotential", NameTextCodeDefaultText = "Set as Potential", FeatureTypeCode = "ACT", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
             			   Feature CustomerFeature_MB54 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NOTMYCUSTOMER", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.SetNotMyCustomer", NameTextCodeDefaultText = "Set as Not My Customer", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
             			   Feature CustomerFeature_MB55 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "QUESTIONNAIREANSWERS", ObjectTableId = CustomerObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customer.Features.ViewQuestionnaireAnswers", NameTextCodeDefaultText = "View Questionnaire Answers", FeatureTypeCode = "ACT", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
              

		   TextCodeRepository.SubmitChanges();
		   FeaturesRepository.SubmitChanges();
		   MenuButtonGroup CustomerMenuButtonGroup = AddMenuButtonGroupAndMenuButtons.AddMenuButtonGroup(new MenuButtonGroupDetails()
				{
					MenuButtonGroupType = "CustomerEdit",
					Name = "CustomerEditButtonsGroup",
					ObjectTableId = CustomerObjectTable.Id,
					Tenant = 0
				}, menuButtonGroupRepository, tenantMenuButtonGroups);        
   
			   MenuButton CustomerMenuButton0 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "Activate",
						Index = 0, 
						IsActive = true,
						LabelTextCodeCode = "Customer.B.Activate",
						LabelTextCodeDefaultText = "Activate",
						Tenant = 0,
						MenuButtonGroupId = CustomerMenuButtonGroup.Id,
						ObjectTableId = CustomerObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = CustomerFeature_MB0.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton CustomerMenuButton1 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "ReadyActivate",
						Index = 1, 
						IsActive = true,
						LabelTextCodeCode = "Customer.B.Ready",
						LabelTextCodeDefaultText = "Ready For Activation",
						Tenant = 0,
						MenuButtonGroupId = CustomerMenuButtonGroup.Id,
						ObjectTableId = CustomerObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = CustomerFeature_MB1.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton CustomerMenuButton2 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "Totango",
						Index = 2, 
						IsActive = true,
						LabelTextCodeCode = "Customer.B.Totango",
						LabelTextCodeDefaultText = "Totango",
						Tenant = 0,
						MenuButtonGroupId = CustomerMenuButtonGroup.Id,
						ObjectTableId = CustomerObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = CustomerFeature_MB2.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton CustomerMenuButton3 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "TenantManagement",
						Index = 3, 
						IsActive = true,
						LabelTextCodeCode = "Customer.B.TenantManagement",
						LabelTextCodeDefaultText = "Manage",
						Tenant = 0,
						MenuButtonGroupId = CustomerMenuButtonGroup.Id,
						ObjectTableId = CustomerObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = CustomerFeature_MB3.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton CustomerMenuButton4 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "CreateTenant",
						Index = 4, 
						IsActive = true,
						LabelTextCodeCode = "Customer.B.CreateTenant",
						LabelTextCodeDefaultText = "Create Tenant",
						Tenant = 0,
						MenuButtonGroupId = CustomerMenuButtonGroup.Id,
						ObjectTableId = CustomerObjectTable.Id,
						MenuButtonType = "button",
						FeatureId = CustomerFeature_MB4.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
       
   
			   MenuButton CustomerMenuButton5 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "Actions",
						Index = 5, 
						IsActive = true,
						LabelTextCodeCode = "Customer.B.More",
						LabelTextCodeDefaultText = "More",
						Tenant = 0,
						MenuButtonGroupId = CustomerMenuButtonGroup.Id,
						ObjectTableId = CustomerObjectTable.Id,
						MenuButtonType = "dropdownbutton",
						FeatureId = CustomerFeature_MB5.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);

			   MenuButton CustomerMenuButton50 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "InActiveCustomer",
						Index = 0, 
						IsActive = true,
						LabelTextCodeCode = "Customer.B.InActive",
						LabelTextCodeDefaultText = "Set as Inactive",
						Tenant = 0,
						MenuButtonGroupId = CustomerMenuButtonGroup.Id,
						ParentMenuButtonId = CustomerMenuButton5.Id,
						ObjectTableId = CustomerObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  CustomerFeature_MB50.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton CustomerMenuButton51 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "ReActivateCustomer",
						Index = 1, 
						IsActive = true,
						LabelTextCodeCode = "Customer.B.ReActivate",
						LabelTextCodeDefaultText = "Reactivate",
						Tenant = 0,
						MenuButtonGroupId = CustomerMenuButtonGroup.Id,
						ParentMenuButtonId = CustomerMenuButton5.Id,
						ObjectTableId = CustomerObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  CustomerFeature_MB51.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton CustomerMenuButton52 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "SetMyCustomer",
						Index = 2, 
						IsActive = true,
						LabelTextCodeCode = "Customer.B.SetMyCustomer",
						LabelTextCodeDefaultText = "Set as Customer",
						Tenant = 0,
						MenuButtonGroupId = CustomerMenuButtonGroup.Id,
						ParentMenuButtonId = CustomerMenuButton5.Id,
						ObjectTableId = CustomerObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  CustomerFeature_MB52.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton CustomerMenuButton53 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "SetAsPotential",
						Index = 3, 
						IsActive = true,
						LabelTextCodeCode = "Customer.B.SetAsPotential",
						LabelTextCodeDefaultText = "Set as Potential",
						Tenant = 0,
						MenuButtonGroupId = CustomerMenuButtonGroup.Id,
						ParentMenuButtonId = CustomerMenuButton5.Id,
						ObjectTableId = CustomerObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  CustomerFeature_MB53.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton CustomerMenuButton54 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "SetNotMyCustomer",
						Index = 4, 
						IsActive = true,
						LabelTextCodeCode = "Customer.B.SetNotMyCustomer",
						LabelTextCodeDefaultText = "Set as Foreign Client",
						Tenant = 0,
						MenuButtonGroupId = CustomerMenuButtonGroup.Id,
						ParentMenuButtonId = CustomerMenuButton5.Id,
						ObjectTableId = CustomerObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  CustomerFeature_MB54.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
			   MenuButton CustomerMenuButton55 = AddMenuButtonGroupAndMenuButtons.AddMenuButton(new MenuButtonDetails()
					{
						EventCode = "ViewQuestionnaireAnswersCustomer",
						Index = 5, 
						IsActive = true,
						LabelTextCodeCode = "Customer.B.ViewQuestionnaireAnswers",
						LabelTextCodeDefaultText = "View Questionnaire Answers",
						Tenant = 0,
						MenuButtonGroupId = CustomerMenuButtonGroup.Id,
						ParentMenuButtonId = CustomerMenuButton5.Id,
						ObjectTableId = CustomerObjectTable.Id,
						MenuButtonType = "menuitem",
						FeatureId=  CustomerFeature_MB55.Id,
						Style = null,
						LocalDefaultText = null,
					}, menuButtonRepository, tenantMenuButtons, TextCodeRepository, textCodes);
	   
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable CustomerObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customer" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode CustomerTextCode_CustomerOGeneralData = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.O.GeneralData", DefaultText = "General Data",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomerTextCode_CustomerOQueryCRMCustomers = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.O.Query.CRMCustomers", DefaultText = "Customers",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomerTextCode_CustomerCHCityNameListLable = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.CH.CityNameListLable", DefaultText = "City",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomerTextCode_CustomerSOverviewCustomerDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.S.Overview.CustomerDetails", DefaultText = "Customer Details",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomerTextCode_CustomerSOverviewContacts = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.S.Overview.Contacts", DefaultText = "Contacts",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomerTextCode_CustomerSOverviewBusinessRecords = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.S.Overview.BusinessRecords", DefaultText = "Business Records",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomerTextCode_CustomerSOverviewQuotes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.S.Overview.Quotes", DefaultText = "Quotes",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomerTextCode_CustomerSOverviewShipments = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.S.Overview.Shipments", DefaultText = "Shipments",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomerTextCode_CustomerSOverviewMoneyInformation = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.S.Overview.MoneyInformation", DefaultText = "Money Information",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "S", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomerTextCode_CustomerQCustomersByLastActivity = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.Q.CustomersByLastActivity", DefaultText = "Customers By Last Activity",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomerTextCode_Customer = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer", DefaultText = "Customer",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "T", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode CustomerTextCode_CustomerOAddContact = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.O.AddContact", DefaultText = "Add Contact",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomerTextCode_CustomerOCompanyName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.O.CompanyName", DefaultText = "Company Name",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomerTextCode_CustomerCHCountryNameListLabel = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.CH.CountryNameListLabel", DefaultText = "Country",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "CH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomerTextCode_CustomerOCompanyLocalName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customer.O.CompanyLocalName", DefaultText = "Local Name",LocalDefaultText = null, ObjectTableId = CustomerObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 