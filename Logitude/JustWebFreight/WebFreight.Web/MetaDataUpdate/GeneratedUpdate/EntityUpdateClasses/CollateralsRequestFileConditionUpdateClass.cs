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
   public class CollateralsRequestFileConditionUpdateClass
   {
   
	 public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTabelRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "Customs.CollateralsRequestFileCondition",
			      				    DBTableName =  "Customs.CollateralsRequestFileConditions",
			      				    ObjectTableSingular =  "CollateralsRequestFileCondition",
			      				    ObjectTablePlural =  "CollateralsRequestFileConditions",
			      				    IsNewWizard =  false,
			      				    KeyPropertyPath =  "CustomsCollateralId",
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
			      				    SortingByObjectField =  "CustomsCollateralId",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  false,
			      				    IsComposition =  true,
			      				    EnableSecurity =  false,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "BR",
			      				    MaxNumberOfCustomFields =  0,
			      				    DefaultText =  "Collaterals Request File Condition",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldsRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	{
	
       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "CustomsCollateralId",
			      				    ObjectTableName =  "Customs.CollateralsRequestFileCondition",
			      				    FieldsDataType =  "Text",
			      				    MinLength =  0,
			      				    MaxLength =  15,
			      				    IsRequired =  true,
			      				    DisplayOnLookUp =  false,
			      				    CanFilter =  false,
			      				    DisplayOnly =  false,
			      				    SystemRequired =  false,
			      				    SystemMaxLength =  15,
			      				    DisplayInList =  false,
			      				    IsCustomFilter =  false,
			      				    MultiLine =  false,
			      				    IsTimeFrameFilter =  false,
			      				    DisplayInSearchWindowList =  false,
			      				    PMPropertyPath =  "CustomsCollateralId",
			      				    ListPropertyPath =  "CustomsCollateralId",
			      				    DisplayInLookUpIndex =  0,
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    DisplayInEntityVariables =  false,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "CustomsCollateralId",
			      				    DefaultText =  "CustomsCollateralId",
			      				    IsMaxLength =  false,
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "ConditionCode",
			      				    ObjectTableName =  "Customs.CollateralsRequestFileCondition",
			      				    FieldsDataType =  "Text",
			      				    MinLength =  0,
			      				    MaxLength =  2,
			      				    IsRequired =  true,
			      				    DisplayOnLookUp =  false,
			      				    CanFilter =  false,
			      				    DisplayOnly =  false,
			      				    SystemRequired =  false,
			      				    SystemMaxLength =  2,
			      				    DisplayInList =  true,
			      				    IsCustomFilter =  false,
			      				    Operator =  "Equals",
			      				    MultiLine =  false,
			      				    IsTimeFrameFilter =  false,
			      				    DisplayInSearchWindowList =  false,
			      				    PMPropertyPath =  "ConditionCode",
			      				    ListPropertyPath =  "ConditionCode",
			      				    DisplayInLookUpIndex =  0,
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    ValidForQuerySection1 =  "Customs.CollateralsRequestFileCondition",
			      				    DisplayInEntityVariables =  false,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "ConditionCode",
			      				    DefaultText =  "Condition Code",
			      				    FullLocalDefaultText =  "תנאי להחזרת בטוחה",
			      				    ListFieldLable =  "ConditionCodeListLable",
			      				    ListLableDefaultText =  "Condition Code",
			      				    ListLocalDefaultText =  "תנאי להחזרת בטוחה",
			      				    IsMaxLength =  false,
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "RequestedAmount",
			      				    ObjectTableName =  "Customs.CollateralsRequestFileCondition",
			      				    FieldsDataType =  "Decimal",
			      				    MinLength =  0,
			      				    MaxLength =  0,
			      				    IsRequired =  false,
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
			      				    PMPropertyPath =  "RequestedAmount",
			      				    ListPropertyPath =  "RequestedAmount",
			      				    DisplayInLookUpIndex =  0,
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    ValidForQuerySection1 =  "Customs.CollateralsRequestFileCondition",
			      				    DisplayInEntityVariables =  false,
			      				    NumberOfDigits =  18,
			      				    DigitsAfterPoint =  2,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "RequestedAmount",
			      				    DefaultText =  "Requested Amount",
			      				    FullLocalDefaultText =  "סכום בטוחה מבוקש",
			      				    ListFieldLable =  "RequestedAmountListLable",
			      				    ListLableDefaultText =  "Requested Amount",
			      				    ListLocalDefaultText =  "סכום בטוחה מבוקש",
			      				    IsMaxLength =  false,
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "ConditionName",
			      				    ObjectTableName =  "Customs.CollateralsRequestFileCondition",
			      				    FieldsDataType =  "nText",
			      				    MinLength =  0,
			      				    MaxLength =  100,
			      				    IsRequired =  false,
			      				    DisplayOnLookUp =  false,
			      				    CanFilter =  false,
			      				    DisplayOnly =  false,
			      				    SystemRequired =  false,
			      				    SystemMaxLength =  100,
			      				    DisplayInList =  true,
			      				    IsCustomFilter =  false,
			      				    MultiLine =  false,
			      				    IsTimeFrameFilter =  false,
			      				    DisplayInSearchWindowList =  false,
			      				    PMPropertyPath =  "ConditionName",
			      				    ListPropertyPath =  "ConditionName",
			      				    DisplayInLookUpIndex =  0,
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    ValidForQuerySection1 =  "Customs.CollateralsRequestFileCondition",
			      				    DisplayInEntityVariables =  false,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "ConditionName",
			      				    DefaultText =  "Condition",
			      				    FullLocalDefaultText =  "תנאי להחזרת בטוחה",
			      				    ListFieldLable =  "ConditionNameListLable",
			      				    ListLableDefaultText =  "Condition",
			      				    ListLocalDefaultText =  "תנאי להחזרת בטוחה",
			      				    IsMaxLength =  false,
			      		
       },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

       AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
       {

	             				    FieldName =  "LineNumber",
			      				    ObjectTableName =  "Customs.CollateralsRequestFileCondition",
			      				    FieldsDataType =  "Integer",
			      				    MinLength =  0,
			      				    MaxLength =  0,
			      				    IsRequired =  true,
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
			      				    PMPropertyPath =  "LineNumber",
			      				    ListPropertyPath =  "LineNumber",
			      				    DisplayInLookUpIndex =  0,
			      				    AutomaticField =  false,
			      				    UniqueField =  false,
			      				    DisplayInSearchWindowListIndex =  0,
			      				    IsMulti =  false,
			      				    DisplayInEntityVariables =  false,
			      				    InActive =  false,
			      				    DisplayLongName =  false,
			      				    FullFieldLable =  "LineNumber",
			      				    DefaultText =  "LineNumber",
			      				    IsMaxLength =  false,
			      		
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
	   ObjectTable CollateralsRequestFileConditionObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.CollateralsRequestFileCondition" && d.Tenant == 0).FirstOrDefault();
	 	   TextCodeRepository.SubmitChanges();
	   FeaturesRepository.SubmitChanges();
	
	} 
	
	public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext)
	{ 
	   ObjectTable CollateralsRequestFileConditionObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.CollateralsRequestFileCondition" && d.Tenant == 0).FirstOrDefault(); 
	   	}    
   }
    
}
	 
