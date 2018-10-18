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

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.EntityUpdateClasses
{
   public class SupplierInvoiceItemsTaxesModificationUpdateClass
   {
   
	 public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTabelRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "Customs.SupplierInvoiceItemsTaxesModification",
			      				    DBTableName =  "Customs.SupplierInvoiceItemsTaxesModifications",
			      				    ObjectTableSingular =  "Supplier Invoice Items Taxes Modification",
			      				    ObjectTablePlural =  "Supplier Invoice Items Taxes Modifications",
			      				    IsEditable =  false,
			      				    IsNewWizard =  false,
			      				    KeyPropertyPath =  "DeclarationId",
			      				    AutoCompleteSearchWindow =  false,
			      				    IsClosed =  false,
			      				    CacheOnClient =  false,
			      				    EditableFromAutoCompleteWindow =  false,
			      				    HasCounter =  false,
			      				    EnableAddFromLOV =  false,
			      				    IsRestrictable =  false,
			      				    IsMain =  false,
			      				    IsAutoComplete =  false,
			      				    EnableEditFromLOV =  false,
			      				    SortingByObjectField =  "LineNumber",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  false,
			      				    IsComposition =  true,
			      				    EnableSecurity =  false,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    DefaultText =  "Supplier Invoice Items Taxes Modification",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldsRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	{
	
       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "DeclarationId",
			      				    ObjectTableName =  "Customs.SupplierInvoiceItemsTaxesModification",
			      				    FieldsDataType =  "Text",
			      				    MinLength =  0,
			      				    MaxLength =  15,
			      				    IsRequired =  true,
			      				    DisplayOnLookUp =  false,
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
			      				    ValidForQuerySection1 =  "Customs.SupplierInvoiceItemsTaxesModification",
			      				    DisplayInEntityVariables =  false,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "DeclarationId",
			      				    DefaultText =  "Declaration Id",
			      				    FullLocalDefaultText =  "מספר תיק מכס",
			      				    ListFieldLable =  "DeclarationIdListLable",
			      				    ListLableDefaultText =  "Declaration Id",
			      				    ListLocalDefaultText =  "מספר תיק מכס",
			      				    IsMaxLength =  false,
			      				    IsFixedLength =  false,
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "InvoiceCounterKey",
			      				    ObjectTableName =  "Customs.SupplierInvoiceItemsTaxesModification",
			      				    FieldsDataType =  "Integer",
			      				    MinLength =  0,
			      				    MaxLength =  0,
			      				    IsRequired =  false,
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
			      				    PMPropertyPath =  "InvoiceCounterKey",
			      				    ListPropertyPath =  "InvoiceCounterKey",
			      				    DisplayInLookUpIndex =  0,
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    DependencyFilter1IsList =  false,
			      				    DependencyFilter2IsList =  false,
			      				    ValidForQuerySection1 =  "Customs.SupplierInvoiceItemsTaxesModification",
			      				    DisplayInEntityVariables =  false,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "InvoiceCounterKey",
			      				    DefaultText =  "Invoice Counter Key",
			      				    ListFieldLable =  "InvoiceCounterKeyListLable",
			      				    ListLableDefaultText =  "Invoice Counter Key",
			      				    IsMaxLength =  false,
			      				    IsFixedLength =  false,
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "LineNumber",
			      				    ObjectTableName =  "Customs.SupplierInvoiceItemsTaxesModification",
			      				    FieldsDataType =  "Integer",
			      				    MinLength =  0,
			      				    MaxLength =  0,
			      				    IsRequired =  false,
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
			      				    PMPropertyPath =  "LineNumber",
			      				    ListPropertyPath =  "LineNumber",
			      				    DisplayInLookUpIndex =  0,
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    DependencyFilter1IsList =  false,
			      				    DependencyFilter2IsList =  false,
			      				    ValidForQuerySection1 =  "Customs.SupplierInvoiceItemsTaxesModification",
			      				    DisplayInEntityVariables =  false,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "LineNumber",
			      				    DefaultText =  "LineNumber",
			      				    ListFieldLable =  "LineNumberListLable",
			      				    ListLableDefaultText =  "LineNumber",
			      				    IsMaxLength =  false,
			      				    IsFixedLength =  false,
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "TaxTypeCode",
			      				    ObjectTableName =  "Customs.SupplierInvoiceItemsTaxesModification",
			      				    FieldsDataType =  "LookUp",
			      				    LookUpTableName =  "Customs.ParagraphType",
			      				    MinLength =  0,
			      				    MaxLength =  3,
			      				    IsRequired =  true,
			      				    DisplayOnLookUp =  false,
			      				    CanFilter =  true,
			      				    DisplayOnly =  false,
			      				    SystemRequired =  false,
			      				    SystemMaxLength =  3,
			      				    DisplayInList =  true,
			      				    IsCustomFilter =  false,
			      				    Operator =  "Equals",
			      				    MultiLine =  false,
			      				    IsTimeFrameFilter =  false,
			      				    DisplayInSearchWindowList =  false,
			      				    PMPropertyPath =  "TaxTypeCode",
			      				    ListPropertyPath =  "TaxTypeCode",
			      				    DisplayInLookUpIndex =  0,
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    DependencyFilter1IsList =  false,
			      				    DependencyFilter2IsList =  false,
			      				    ValidForQuerySection1 =  "Customs.SupplierInvoiceItemsTaxesModification",
			      				    DisplayInEntityVariables =  false,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "TaxTypeCode",
			      				    DefaultText =  "Tax Type Code",
			      				    FullLocalDefaultText =  "סעיף מס לתשלום",
			      				    ListFieldLable =  "TaxTypeCodeListLable",
			      				    ListLableDefaultText =  "Tax Type Code",
			      				    ListLocalDefaultText =  "סעיף מס לתשלום",
			      				    IsMaxLength =  false,
			      				    IsFixedLength =  false,
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "TypeCode",
			      				    ObjectTableName =  "Customs.SupplierInvoiceItemsTaxesModification",
			      				    FieldsDataType =  "LookUp",
			      				    LookUpTableName =  "Customs.ModificationAndDiscountType",
			      				    MinLength =  0,
			      				    MaxLength =  3,
			      				    IsRequired =  true,
			      				    DisplayOnLookUp =  false,
			      				    CanFilter =  true,
			      				    DisplayOnly =  false,
			      				    SystemRequired =  false,
			      				    SystemMaxLength =  3,
			      				    DisplayInList =  true,
			      				    IsCustomFilter =  false,
			      				    Operator =  "Equals",
			      				    MultiLine =  false,
			      				    IsTimeFrameFilter =  false,
			      				    DisplayInSearchWindowList =  false,
			      				    PMPropertyPath =  "TypeCode",
			      				    ListPropertyPath =  "TypeCode",
			      				    DisplayInLookUpIndex =  0,
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    DependencyFilter1IsList =  false,
			      				    DependencyFilter2IsList =  false,
			      				    ValidForQuerySection1 =  "Customs.SupplierInvoiceItemsTaxesModification",
			      				    DisplayInEntityVariables =  false,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "TypeCode",
			      				    DefaultText =  "Type Code",
			      				    FullLocalDefaultText =  "סוג מס",
			      				    ListFieldLable =  "TypeCodeListLable",
			      				    ListLableDefaultText =  "Type Code",
			      				    ListLocalDefaultText =  "סוג מס",
			      				    IsMaxLength =  false,
			      				    IsFixedLength =  false,
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "CurrencyTypeCode",
			      				    ObjectTableName =  "Customs.SupplierInvoiceItemsTaxesModification",
			      				    FieldsDataType =  "LookUp",
			      				    LookUpTableName =  "Customs.CurrencyType",
			      				    MinLength =  0,
			      				    MaxLength =  3,
			      				    IsRequired =  false,
			      				    DisplayOnLookUp =  false,
			      				    CanFilter =  true,
			      				    DisplayOnly =  false,
			      				    SystemRequired =  false,
			      				    SystemMaxLength =  3,
			      				    DisplayInList =  true,
			      				    IsCustomFilter =  false,
			      				    Operator =  "Equals",
			      				    MultiLine =  false,
			      				    IsTimeFrameFilter =  false,
			      				    DisplayInSearchWindowList =  false,
			      				    PMPropertyPath =  "CurrencyTypeCode",
			      				    ListPropertyPath =  "CurrencyTypeCode",
			      				    DisplayInLookUpIndex =  0,
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    DependencyFilter1IsList =  false,
			      				    DependencyFilter2IsList =  false,
			      				    ValidForQuerySection1 =  "Customs.SupplierInvoiceItemsTaxesModification",
			      				    DisplayInEntityVariables =  false,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "CurrencyTypeCode",
			      				    DefaultText =  "Currency Type Code",
			      				    FullLocalDefaultText =  "מטבע",
			      				    ListFieldLable =  "CurrencyTypeCodeListLable",
			      				    ListLableDefaultText =  "Currency Type Code",
			      				    ListLocalDefaultText =  "מטבע",
			      				    IsMaxLength =  false,
			      				    IsFixedLength =  false,
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "Amount",
			      				    ObjectTableName =  "Customs.SupplierInvoiceItemsTaxesModification",
			      				    FieldsDataType =  "Decimal",
			      				    MinLength =  0,
			      				    MaxLength =  0,
			      				    IsRequired =  false,
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
			      				    PMPropertyPath =  "Amount",
			      				    ListPropertyPath =  "Amount",
			      				    DisplayInLookUpIndex =  0,
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    DependencyFilter1IsList =  false,
			      				    DependencyFilter2IsList =  false,
			      				    ValidForQuerySection1 =  "Customs.SupplierInvoiceItemsTaxesModification",
			      				    DisplayInEntityVariables =  false,
			      				    NumberOfDigits =  16,
			      				    DigitsAfterPoint =  2,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "Amount",
			      				    DefaultText =  "Amount",
			      				    FullLocalDefaultText =  "סכום",
			      				    ListFieldLable =  "AmountListLable",
			      				    ListLableDefaultText =  "Amount",
			      				    ListLocalDefaultText =  "סכום",
			      				    IsMaxLength =  false,
			      				    IsFixedLength =  false,
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	}

	public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	{  
	   FeatureRepository featureRepository = new FeatureRepository(0); 
       List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
       IWebFreightContext objectContext = WebFreightContext.GetContext(0);  
	  	   
	}

	public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	{   
	    
	   
	
	}

	public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	{
	   FeatureRepository featureRepository = new FeatureRepository(0); 
       List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList(); 
	   ObjectTable SupplierInvoiceItemsTaxesModificationObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.SupplierInvoiceItemsTaxesModification" && d.Tenant == 0).FirstOrDefault();
	 	   TextCodeRepository.SubmitChanges();
	   FeaturesRepository.SubmitChanges();
	
	} 
	
	public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext)
	{ 
	   ObjectTable SupplierInvoiceItemsTaxesModificationObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.SupplierInvoiceItemsTaxesModification" && d.Tenant == 0).FirstOrDefault(); 
	   	}    
   }
    
}
	 