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
namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.InfrastructureModel.EntityUpdateClasses
{
   public class RatesTableUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "RatesTable",
			      				    DBTableName =  "RatesTables",
			      				    ObjectTableSingular =  "Rates Table",
			      				    ObjectTablePlural =  "RatesTables",
			      				    DefaultText =  "Rates Table",
			      				    Name =  "Rates",
			      				    IsNewWizard =  false,
			      				    HasCustomFilter =  false,
			      				    KeyPropertyPath =  "Id",
			      				    AutoCompleteSearchWindow =  true,
			      				    IsClosed =  false,
			      				    CacheOnClient =  true,
			      				    EditableFromAutoCompleteWindow =  false,
			      				    HasCounter =  false,
			      				    EnableEditFromLOV =  false,
			      				    EnableAddFromLOV =  false,
			      				    IsRestrictable =  false,
			      				    IsMain =  true,
			      				    IsAutoComplete =  true,
			      				    CustomFieldsCount =  0,
			      				    HasCustomFields =  false,
			      				    InActive =  false,
			      				    SearchFields =  "RatesTable,RatesTables,,Id,",
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
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasMenuButtons =  false,
			      				    HasFiltersMenu =  false,
			      				    ClientModuleName =  "Infrastructure",
			      				    Code =  "RATE",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ForeignCurrencyId",
					  						ObjectTableName =  "RatesTable",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "ForeignCurrencyId",
					  						ListPropertyPath =  "ForeignCurrencyId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  true,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "RatesTable",
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "ForeignCurrencyId",
					  						DefaultText =  @"Foreign Currency",
					  						ListFieldLable =  "ForeignCurrencyIdListLable",
					  						ListLableDefaultText =  @"Foreign Currency",
					  						HelpTextCode =  "ForeignCurrencyId",
					  						ShortFieldLable =  "ForeignCurrencyId",
					  						ShortFieldLableDefaultText =  @"Currency",
					  						Code =  "ForeignCurrencyId",
					  						DependencyFilter3IsList =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  						ShortLocalDefaultText =  @"מטבע זר",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ForeignCurrencyCode",
					  						ObjectTableName =  "RatesTable",
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
					  						IsCustomFilter =  true,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "ForeignCurrencyCode",
					  						ListPropertyPath =  "ForeignCurrencyCode",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  true,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "RatesTable",
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "ForeignCurrencyCode",
					  						DefaultText =  @"Foreign Currency Code",
					  						ListFieldLable =  "ForeignCurrencyCodeListLable",
					  						ListLableDefaultText =  @"Foreign Currency Code",
					  						HelpTextCode =  "ForeignCurrencyCode",
					  						ShortFieldLable =  "CurrencyCode",
					  						ShortFieldLableDefaultText =  @"Code",
					  						Code =  "ForeignCurrencyCode",
					  						DependencyFilter3IsList =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						ShortLocalDefaultText =  @"קוד מטבע זר",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "BaseCurrencyId",
					  						ObjectTableName =  "RatesTable",
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
					  						IsCustomFilter =  true,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "BaseCurrencyId",
					  						ListPropertyPath =  "BaseCurrencyId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  true,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "RatesTable",
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "BaseCurrencyId",
					  						DefaultText =  @"Base Currency",
					  						ListFieldLable =  "BaseCurrencyIdListLable",
					  						ListLableDefaultText =  @"Base Currency",
					  						HelpTextCode =  "BaseCurrencyId",
					  						Code =  "BaseCurrencyId",
					  						DependencyFilter3IsList =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Rate",
					  						ObjectTableName =  "RatesTable",
					  						FieldsDataType =  "Double",
					  						DataTypeCode =  "Double",
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
					  						PMPropertyPath =  "Rate",
					  						ListPropertyPath =  "Rate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "RatesTable",
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "Rate",
					  						DefaultText =  @"Rate",
					  						ListFieldLable =  "RateListLable",
					  						ListLableDefaultText =  @"Rate",
					  						HelpTextCode =  "Rate",
					  						ShortFieldLable =  "Rate",
					  						ShortFieldLableDefaultText =  @"Rate",
					  						Code =  "Rate",
					  						DependencyFilter3IsList =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  						ShortLocalDefaultText =  @"שער",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ValueDate",
					  						ObjectTableName =  "RatesTable",
					  						FieldsDataType =  "DateTime",
					  						DataTypeCode =  "DateTime",
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
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						PMPropertyPath =  "ValueDate",
					  						ListPropertyPath =  "ValueDate",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "RatesTable",
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "ValueDate",
					  						DefaultText =  @"Value Date",
					  						ListFieldLable =  "ValueDateLable",
					  						ListLableDefaultText =  @"Value Date",
					  						HelpTextCode =  "ValueDate",
					  						ShortFieldLable =  "Date",
					  						ShortFieldLableDefaultText =  @"Date",
					  						Code =  "ValueDate",
					  						DependencyFilter3IsList =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  						ShortLocalDefaultText =  @"תאריך ערך",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SearchFields",
					  						ObjectTableName =  "RatesTable",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
					  						MaxLength =  1000,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
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
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "RatesTable",
					  						ValidForQuerySection2 =  "RatesTableFollowUp",
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
					  						AllowedInAirlineMessaging =  false,
					  						HasTemplate =  false,
					  						FullFieldLable =  "SearchFields",
					  						DefaultText =  @"Search codes/ names",
					  						HelpTextCode =  "SearchFields",
					  						HelpTextDefaultText =  @"Searching by :\n1: code\n2: name",
					  						Code =  "SearchFields",
					  						DependencyFilter3IsList =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						FullLocalDefaultText =  @"תאריך ערך",
					  						HelpLocalDefaultText =  @"חיפוש לפי קוד\שם",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	    {  
	        FeatureRepository featureRepository = new FeatureRepository(0); 
            //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
            IWebFreightContext objectContext = WebFreightContext.GetContext(0);  
	        QueryGroup RatesTableQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "RATE", Name = "Rates" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable RatesTableObjectTable = objectContext.ObjectTables.Where(d => d.Name == "RatesTable" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> RatesTableObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "RatesTable").ToList();   

			   TextCode RatesTableTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "RatesTable.Q.RatesTables", DefaultText = @"Rates",LocalDefaultText = null, ObjectTableId = RatesTableObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature RatesTableFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "RATESTABLE", ObjectTableId = RatesTableObjectTable.Id, Tenant = 0, NameTextCodeCode = "RatesTable.Features.Rates", NameTextCodeDefaultText = "Rates", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query RatesQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = RatesTableTextCode_0.Id, Code = "Rates",  QueryGroupCode = "RATE", IndexOrder = 0, Tenant = 0, ObjectTableId = RatesTableObjectTable.Id, QuerySection = "RatesTable", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = RatesTableFeature_0.Id, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn RatesQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = RatesQuery.Id, IndexOrder = 0, ObjectFieldId = RatesTableObjectFields.Where(d => d.FieldName == "ForeignCurrencyCode" && d.ObjectTableId == RatesTableObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn RatesQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = RatesQuery.Id, IndexOrder = 1, ObjectFieldId = RatesTableObjectFields.Where(d => d.FieldName == "ValueDate" && d.ObjectTableId == RatesTableObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn RatesQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = RatesQuery.Id, IndexOrder = 2, ObjectFieldId = RatesTableObjectFields.Where(d => d.FieldName == "Rate" && d.ObjectTableId == RatesTableObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {    

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable RatesTableObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "RatesTable" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode RatesTableGeneralTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "RatesTable.TH.Main", DefaultText = "General",LocalDefaultText = null, ObjectTableId = RatesTableObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature RatesTableGeneralFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "RatesTable.Tab.General", ObjectTableId = RatesTableObjectTable.Id, Tenant = 0, NameTextCodeCode = "RatesTableFeatures.RTMA", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode RatesTableEventsTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "RatesTable.TH.Events", DefaultText = "Events",LocalDefaultText = null, ObjectTableId = RatesTableObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature RatesTableEventsFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "RatesTable.Tab.Events", ObjectTableId = RatesTableObjectTable.Id, Tenant = 0, NameTextCodeCode = "RatesTableFeatures.RTEV", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "RTMA",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = RatesTableGeneralFeature_TH0.Id, ControlPath = "Simplog.FreightLib.Views.CurrencyRates.RatesTableMainTabControl", ObjectTableId = RatesTableObjectTable.Id, TabNameTextCodeId = RatesTableGeneralTextCode_TH0.Id, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "RTEV",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = RatesTableEventsFeature_TH1.Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = RatesTableObjectTable.Id, TabNameTextCodeId = RatesTableEventsTextCode_TH1.Id, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable RatesTableObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "RatesTable" && d.Tenant == 0).FirstOrDefault(); 
		   Feature RatesTableFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = RatesTableObjectTable.Id, Tenant = 0, NameTextCodeCode = "RatesTable.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature RatesTableFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = RatesTableObjectTable.Id, Tenant = 0, NameTextCodeCode = "RatesTable.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature RatesTableFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = RatesTableObjectTable.Id, Tenant = 0, NameTextCodeCode = "RatesTable.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature RatesTableFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = RatesTableObjectTable.Id, Tenant = 0, NameTextCodeCode = "RatesTable.Features.PackageFeature", NameTextCodeDefaultText = "RatesTable Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);    
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable RatesTableObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "RatesTable" && d.Tenant == 0).FirstOrDefault(); 
	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable RatesTableObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "RatesTable" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode RatesTableTextCode_RatesTableOEditCurrencyRate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "RatesTable.O.EditCurrencyRate", DefaultText = "Edit Currency Rate",LocalDefaultText = null, ObjectTableId = RatesTableObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode RatesTableTextCode_RatesTableOCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "RatesTable.O.Code", DefaultText = "Code",LocalDefaultText = null, ObjectTableId = RatesTableObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode RatesTableTextCode_RatesTableOName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "RatesTable.O.Name", DefaultText = "Name",LocalDefaultText = null, ObjectTableId = RatesTableObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode RatesTableTextCode_RatesTableOOldValues = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "RatesTable.O.OldValues", DefaultText = "Old Values",LocalDefaultText = null, ObjectTableId = RatesTableObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode RatesTableTextCode_RatesTableOCurrencyHistory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "RatesTable.O.CurrencyHistory", DefaultText = "Currency History",LocalDefaultText = null, ObjectTableId = RatesTableObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode RatesTableTextCode_RatesTableMDefirenceIsMoreThan = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "RatesTable.M.DefirenceIsMoreThan", DefaultText = "Difference is more than 5 %",LocalDefaultText = @"השינוי גדול מ- 5%", ObjectTableId = RatesTableObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode RatesTableTextCode_RatesTableBViewHistory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "RatesTable.B.ViewHistory", DefaultText = "View History",LocalDefaultText = null, ObjectTableId = RatesTableObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode RatesTableTextCode_RatesTableBSetAsToday = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "RatesTable.B.SetAsToday", DefaultText = "Set As Today",LocalDefaultText = null, ObjectTableId = RatesTableObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode RatesTableTextCode_RatesTableOExchangeDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "RatesTable.O.ExchangeDate", DefaultText = "Exchange Date",LocalDefaultText = null, ObjectTableId = RatesTableObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode RatesTableTextCode_RatesTableMChangingTheExchangeRate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "RatesTable.M.ChangingTheExchangeRate", DefaultText = "Please confirm changing the exchange rate to ",LocalDefaultText = @"אנא אשר שינוי שער החליפין ל", ObjectTableId = RatesTableObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode RatesTableTextCode_RatesTableOUpdateCurrencyRate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "RatesTable.O.UpdateCurrencyRate", DefaultText = "Update Currency Rate",LocalDefaultText = @"עדכן שער חליפין למטבע", ObjectTableId = RatesTableObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 