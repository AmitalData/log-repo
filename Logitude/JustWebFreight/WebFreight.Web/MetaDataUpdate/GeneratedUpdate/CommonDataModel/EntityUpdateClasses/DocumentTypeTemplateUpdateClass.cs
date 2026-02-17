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
   public class DocumentTypeTemplateUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "DocumentTypeTemplate",
			      				    IsNew =  false,
			      				    DBTableName =  "DocumentTypeTemplates",
			      				    OldDBTableName =  "DocumentTypeTemplates",
			      				    ObjectTableSingular =  "Document Type Template",
			      				    ObjectTablePlural =  "DocumentTypeTemplates",
			      				    HasCustomFilter =  false,
			      				    HasCustomFields =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  true,
			      				    IsNewWizard =  false,
			      				    KeyPropertyPath =  "Id",
			      				    AutoCompleteSearchWindow =  true,
			      				    IsClosed =  false,
			      				    CacheOnClient =  false,
			      				    EditableFromAutoCompleteWindow =  true,
			      				    HasCounter =  false,
			      				    EnableAddFromLOV =  false,
			      				    IsRestrictable =  false,
			      				    IsMain =  true,
			      				    IsAutoComplete =  true,
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
			      				    DefaultText =  "Document Type Template",
			      				    Code =  "cb2c",
			      				    Name =  "DocumentTypeTemplate",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Common",
			      				    NoTS =  false,
			      				    HasMenuButtons =  false,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    SearchFields =  "DocumentTypeTemplate,DocumentTypeTemplates,,Id,",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Description",
					  						OldFieldName =  "Description",
					  						ObjectTableName =  "DocumentTypeTemplate",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  40,
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
					  						Operator =  "StartsWith",
					  						MultiLine =  true,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Description",
					  						ListPropertyPath =  "Description",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Description",
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
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Description",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Language",
					  						OldFieldName =  "Language",
					  						ObjectTableName =  "DocumentTypeTemplate",
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
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  true,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Language",
					  						ListPropertyPath =  "Language",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Language",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "Language",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Language",
					  						DefaultText =  "Language",
					  						ListFieldLable =  "LanguageListLable",
					  						ListLableDefaultText =  "Language",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "Language",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InternalRemarks",
					  						OldFieldName =  "InternalRemarks",
					  						ObjectTableName =  "DocumentTypeTemplate",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  500,
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
					  						Operator =  "StartsWith",
					  						MultiLine =  true,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Language",
					  						ListPropertyPath =  "Language",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Language",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "InternalRemarks",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "InternalRemarks",
					  						DefaultText =  "Remarks",
					  						ListFieldLable =  "InternalRemarksListLable",
					  						ListLableDefaultText =  "Language",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "InternalRemarks",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "CountryCode",
					  						OldFieldName =  "CountryCode",
					  						ObjectTableName =  "DocumentTypeTemplate",
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
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  true,
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
					  						ValidForQuerySection1 =  "CountryCode",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "CountryCode",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "CountryCode",
					  						DefaultText =  "Country",
					  						ListFieldLable =  "CountryCodeListLable",
					  						ListLableDefaultText =  "CountryCode",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "CountryCode",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsEnabledForCustomers",
					  						OldFieldName =  "IsEnabledForCustomers",
					  						ObjectTableName =  "DocumentTypeTemplate",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  true,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsEnabledForCustomers",
					  						ListPropertyPath =  "IsEnabledForCustomers",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "IsEnabledForCustomers",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "IsEnabledForCustomers",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsEnabledForCustomers",
					  						DefaultText =  "Enabled for Customers",
					  						ListFieldLable =  "IsEnabledForCustomersListLable",
					  						ListLableDefaultText =  "IsEnabledForCustomers",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "IsEnabledForCustomers",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "IsCopiedAtSignup",
					  						OldFieldName =  "IsCopiedAtSignup",
					  						ObjectTableName =  "DocumentTypeTemplate",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  true,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsCopiedAtSignup",
					  						ListPropertyPath =  "IsCopiedAtSignup",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "IsCopiedAtSignup",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  true,
					  						Code =  "IsCopiedAtSignup",
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						NumberOfDigits =  0,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsCopiedAtSignup",
					  						DefaultText =  "Copy at Signup",
					  						ListFieldLable =  "IsCopiedAtSignupListLable",
					  						ListLableDefaultText =  "IsCopiedAtSignup",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						HelpTextCode =  "IsCopiedAtSignup",
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BCC",
					  						ObjectTableName =  "DocumentTypeTemplate",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  4000,
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
					  						PMPropertyPath =  "BCC",
					  						ListPropertyPath =  "BCC",
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
					  						FullFieldLable =  "BCC",
					  						DefaultText =  "BCC",
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
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {    

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {      
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable DocumentTypeTemplateObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "DocumentTypeTemplate" && d.Tenant == 0).FirstOrDefault(); 

		   Feature DocumentTypeTemplateFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = true, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, NameTextCodeCode = "DocumentTypeTemplate.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature DocumentTypeTemplateFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = true, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, NameTextCodeCode = "DocumentTypeTemplate.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature DocumentTypeTemplateFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = true, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, NameTextCodeCode = "DocumentTypeTemplate.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature DocumentTypeTemplateFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = true, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, NameTextCodeCode = "DocumentTypeTemplate.Features.PackageFeature", NameTextCodeDefaultText = "DocumentTypeTemplate Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes); 

		   		   //--------------> Additional Features <--------------\\

		   Feature DocumentTypeTemplateFeature_INACTIVE = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "INACTIVE", FeatureTypeCode = "ACT", Packagable = false, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, NameTextCodeCode = "DocumentTypeTemplate.Features.Inactive", NameTextCodeDefaultText = @"Inactive" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature DocumentTypeTemplateFeature_ORGINALTEMPLATE = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ORGINALTEMPLATE", FeatureTypeCode = "ACT", Packagable = false, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, NameTextCodeCode = "DocumentTypeTemplate.Features.OrginalTemplate", NameTextCodeDefaultText = @"Orginal Template" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

   
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable DocumentTypeTemplateObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "DocumentTypeTemplate" && d.Tenant == 0).FirstOrDefault(); 
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
                ObjectTableId = DocumentTypeTemplateObjectTable.Id,
				 
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
                ObjectTableId = DocumentTypeTemplateObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable DocumentTypeTemplateObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "DocumentTypeTemplate" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateOManageTemplates = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.O.ManageTemplates", DefaultText = "Manage Templates",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateOEditDocument = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.O.EditDocument", DefaultText = "Edit Document",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateODocumentCopies = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.O.DocumentCopies", DefaultText = "Document Copies",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateOAdditionalPrintingFields = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.O.AdditionalPrintingFields", DefaultText = "Additional Printing Fields",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateOEditPrintingFields = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.O.EditPrintingFields", DefaultText = "Edit Printing Fields",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateOTemplate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.O.Template", DefaultText = "Template",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateODocumentUpdatedAt = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.O.DocumentUpdatedAt", DefaultText = "Document Last Updated",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateOUpdateNow = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.O.UpdateNow", DefaultText = "Update Document",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateOAddTemplateFromLibrary = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.O.AddTemplateFromLibrary", DefaultText = "Add template from library",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateOFindMoreTemplatesForThisDocument = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.O.FindMoreTemplatesForThisDocument", DefaultText = "find more templates for this document",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateOShowInActive = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.O.ShowInActive", DefaultText = "Show inactive",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateOSetAsDefault = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.O.SetAsDefault", DefaultText = "Set as Default",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateOMarkAsInactive = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.O.MarkAsInactive", DefaultText = "Mark as inactive",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateOMarkAsactive = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.O.MarkAsactive", DefaultText = "Mark as active",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateBSetAsInActive = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.B.SetAsInActive", DefaultText = "Set As Inactive",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateOOriginalTemplate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.O.OriginalTemplate", DefaultText = "Original Template",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateOInActive = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.O.InActive", DefaultText = "Inactive",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateODocumentType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.O.DocumentType", DefaultText = "Document Type",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateOType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.O.Type", DefaultText = "Template Type",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateOCountry = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.O.Country", DefaultText = "Country",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateMSelectDocumentToDublicate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.M.SelectDocumentToDublicate", DefaultText = "Please select a document to dublicate",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateBInsert = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.B.Insert", DefaultText = "Insert",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateODuplicateReport = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.O.DuplicateReport", DefaultText = "Duplicate report",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateOInsertDataField = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.O.InsertDataField", DefaultText = "Insert Data Field",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateONewPrintTemplate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.O.NewPrintTemplate", DefaultText = "New Print Template",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateONewHTMLTemplate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.O.NewHTMLTemplate", DefaultText = "New Template",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateOEditPrintTemplate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.O.EditPrintTemplate", DefaultText = "Edit Print Template",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateOEditHTMLTemplate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.O.EditHTMLTemplate", DefaultText = "Edit Template",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateOBlank = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.O.Blank", DefaultText = "Blank",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateODuplicate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.O.Duplicate", DefaultText = "Duplicate",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateOFromLibrary = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.O.FromLibrary", DefaultText = "From Library",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateOFromFile = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.O.FromFile", DefaultText = "From File",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateOStimulSoft = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.O.StimulSoft", DefaultText = "StimulSoft",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateORichText = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.O.RichText", DefaultText = "RichText",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateBSetAsDefault = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.B.SetAsDefault", DefaultText = "Set As Default",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateBLoadTemplate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.B.LoadTemplate", DefaultText = "Load Template",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateODefault = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.O.Default", DefaultText = "Default",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateODescription = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.O.Description", DefaultText = "Description",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateOLastUpdate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.O.LastUpdate", DefaultText = "Last Update",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocumentTypeTemplateTextCode_DocumentTypeTemplateOUpdatedBy = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocumentTypeTemplate.O.UpdatedBy", DefaultText = "Updated By",LocalDefaultText = null, ObjectTableId = DocumentTypeTemplateObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 