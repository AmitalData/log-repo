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

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.InvoiceModel.EntityUpdateClasses
{
   public class APPaymentMethodUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "APPaymentMethod",
			      				    DBTableName =  "APPaymentMethods",
			      				    ObjectTableSingular =  "A/P Payment Method",
			      				    ObjectTablePlural =  "A/P Payment Methods",
			      				    DefaultText =  "A/P Payment Method",
			      				    Name =  "AP Payment Method",
			      				    IsNewWizard =  false,
			      				    LookUp1 =  "Name",
			      				    HasCustomFilter =  false,
			      				    KeyPropertyPath =  "Id",
			      				    AutoCompleteSearchWindow =  false,
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
			      				    SearchFields =  "APPaymentMethod,APPaymentMethods,,Id,",
			      				    IsSaveButtonVisible =  true,
			      				    EnableSecurity =  true,
			      				    ObjectTableTypeCode =  "MD",
			      				    IsComposition =  false,
			      				    MaxNumberOfCustomFields =  0,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  true,
			      				    IsEditable =  true,
			      				    AllowedForComputingPartners =  false,
			      				    ClientModuleName =  "Invoice",
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasMenuButtons =  false,
			      				    HasFiltersMenu =  false,
			      				    Code =  "PPYM",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Code",
					  						ObjectTableName =  "APPaymentMethod",
					  						FieldsDataType =  "Text",
					  						Code =  "Code",
					  						DataTypeCode =  "Text",
					  						MaxLength =  2,
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
					  						PMPropertyPath =  "Code",
					  						ListPropertyPath =  "Code",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  true,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APPaymentMethod",
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
					  						FullFieldLable =  "Code",
					  						DefaultText =  "Code",
					  						HelpTextCode =  "Code",
					  						DependencyFilter3IsList =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  						ListFieldLable =  "CodeListLable",
					  						ListLableDefaultText =  "Code",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Name",
					  						ObjectTableName =  "APPaymentMethod",
					  						FieldsDataType =  "Text",
					  						Code =  "Name",
					  						DataTypeCode =  "Text",
					  						MaxLength =  20,
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
					  						PMPropertyPath =  "Name",
					  						ListPropertyPath =  "Name",
					  						DisplayInLookUpIndex =  1,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  1,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APPaymentMethod",
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
					  						FullFieldLable =  "Name",
					  						DefaultText =  "Name",
					  						ListFieldLable =  "NameListLable",
					  						ListLableDefaultText =  "Name",
					  						HelpTextCode =  "Name",
					  						DependencyFilter3IsList =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SearchFields",
					  						ObjectTableName =  "APPaymentMethod",
					  						FieldsDataType =  "Text",
					  						Code =  "SearchFields",
					  						DataTypeCode =  "Text",
					  						MaxLength =  1000,
					  						IsCustom =  false,
					  						MinLength =  0,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
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
					  						ValidForQuerySection1 =  "APPaymentMethod",
					  						ValidForQuerySection2 =  "APPaymentMethodFollowUp",
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
					  						FullFieldLable =  "SearchFields",
					  						DefaultText =  "Search codes/ names",
					  						HelpTextCode =  "SearchFields",
					  						HelpTextDefaultText =  "Searching by :\n1: code\n2: name",
					  						DependencyFilter3IsList =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "AddedManually",
					  						ObjectTableName =  "APPaymentMethod",
					  						FieldsDataType =  "Boolean",
					  						Code =  "AddedManually",
					  						DataTypeCode =  "Boolean",
					  						MaxLength =  2,
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
					  						PMPropertyPath =  "AddedManually",
					  						ListPropertyPath =  "AddedManually",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APPaymentMethod",
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
					  						FullFieldLable =  "AddedManually",
					  						DefaultText =  "Added Manually",
					  						HelpTextCode =  "AddedManually",
					  						DependencyFilter3IsList =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						ListFieldLable =  "AddedManuallyListLable",
					  						ListLableDefaultText =  "Added Manually",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Inactive",
					  						ObjectTableName =  "APPaymentMethod",
					  						FieldsDataType =  "Boolean",
					  						Code =  "Inactive",
					  						DataTypeCode =  "Text",
					  						MaxLength =  2,
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
					  						PMPropertyPath =  "Inactive",
					  						ListPropertyPath =  "Inactive",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "APPaymentMethod",
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
					  						FullFieldLable =  "Inactive",
					  						DefaultText =  "Inactive",
					  						HelpTextCode =  "Inactive",
					  						DependencyFilter3IsList =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  						ListFieldLable =  "InactiveListLable",
					  						ListLableDefaultText =  "Inactive",
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	    {  
	        FeatureRepository featureRepository = new FeatureRepository(0); 
            //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
            IWebFreightContext objectContext = WebFreightContext.GetContext(0);  
	        QueryGroup APPaymentMethodQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "PPYM", Name = "AP Payment Method" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable APPaymentMethodObjectTable = objectContext.ObjectTables.Where(d => d.Name == "APPaymentMethod" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> APPaymentMethodObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "APPaymentMethod").ToList();   

			   TextCode APPaymentMethodTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPaymentMethod.Q.AllAPPaymentMethods", DefaultText = @"AP Payment Methods",LocalDefaultText = null, ObjectTableId = APPaymentMethodObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature APPaymentMethodFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ALLAPPAYMENTMETHODS", ObjectTableId = APPaymentMethodObjectTable.Id, Tenant = 0, NameTextCodeCode = "APPaymentMethod.Features.AllAPPaymentMethodts", NameTextCodeDefaultText = "All AP Payment Methods", FeatureTypeCode = "QUER", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query AllAPPaymentMethodsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = APPaymentMethodTextCode_0.Id, Code = "All AP Payment Methods",  QueryGroupCode = "PPYM", IndexOrder = 0, Tenant = 0, ObjectTableId = APPaymentMethodObjectTable.Id, QuerySection = "APPaymentMethod", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = APPaymentMethodFeature_0.Id, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn AllAPPaymentMethodsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllAPPaymentMethodsQuery.Id, IndexOrder = 0, ObjectFieldId = APPaymentMethodObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == APPaymentMethodObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APPaymentMethodObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == APPaymentMethodObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllAPPaymentMethodsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllAPPaymentMethodsQuery.Id, IndexOrder = 1, ObjectFieldId = APPaymentMethodObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTableId == APPaymentMethodObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APPaymentMethodObjectFields.Where(d => d.FieldName == "Name" && d.ObjectTableId == APPaymentMethodObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllAPPaymentMethodsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllAPPaymentMethodsQuery.Id, IndexOrder = 2, ObjectFieldId = APPaymentMethodObjectFields.Where(d => d.FieldName == "AddedManually" && d.ObjectTableId == APPaymentMethodObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APPaymentMethodObjectFields.Where(d => d.FieldName == "AddedManually" && d.ObjectTableId == APPaymentMethodObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllAPPaymentMethodsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllAPPaymentMethodsQuery.Id, IndexOrder = 3, ObjectFieldId = APPaymentMethodObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == APPaymentMethodObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = APPaymentMethodObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == APPaymentMethodObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);
	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable APPaymentMethodObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "APPaymentMethod" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> APPaymentMethodObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "APPaymentMethod").ToList();
		       
	      

	         Screen APPaymentMethodHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "APPaymentMethod.HeaderScreen", Name = "Header Screen", ObjectTableId = APPaymentMethodObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField APPaymentMethodAPPaymentMethodHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = APPaymentMethodObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().Id, ScreenId = APPaymentMethodHeaderScreenScreen0.Id, ObjectFieldCode = APPaymentMethodObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField APPaymentMethodAPPaymentMethodHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = APPaymentMethodObjectFields.Where(d => d.FieldName == "Name").FirstOrDefault().Id, ScreenId = APPaymentMethodHeaderScreenScreen0.Id, ObjectFieldCode = APPaymentMethodObjectFields.Where(d => d.FieldName == "Name").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    APPaymentMethodObjectTable.HeaderScreenId = APPaymentMethodHeaderScreenScreen0.Id;
	   		  
	      

	         Screen APPaymentMethodGeneralTabScreenScreen1 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "APPaymentMethod.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = APPaymentMethodObjectTable.Id, NumberOfColumns = 1, NumberOfRows = 4, IsReadOnly = false }, screensRepository, tenantScreens);
      
            ScreenField APPaymentMethodAPPaymentMethodGeneralTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = APPaymentMethodObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().Id, ScreenId = APPaymentMethodGeneralTabScreenScreen1.Id, ObjectFieldCode = APPaymentMethodObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField APPaymentMethodAPPaymentMethodGeneralTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = APPaymentMethodObjectFields.Where(d => d.FieldName == "Name").FirstOrDefault().Id, ScreenId = APPaymentMethodGeneralTabScreenScreen1.Id, ObjectFieldCode = APPaymentMethodObjectFields.Where(d => d.FieldName == "Name").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField APPaymentMethodAPPaymentMethodGeneralTabScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ObjectFieldId = APPaymentMethodObjectFields.Where(d => d.FieldName == "Inactive").FirstOrDefault().Id, ScreenId = APPaymentMethodGeneralTabScreenScreen1.Id, ObjectFieldCode = APPaymentMethodObjectFields.Where(d => d.FieldName == "Inactive").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
           

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable APPaymentMethodObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "APPaymentMethod" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode APPaymentMethodGeneralTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPaymentMethod.TH.General", DefaultText = "General",LocalDefaultText = null, ObjectTableId = APPaymentMethodObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature APPaymentMethodGeneralFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "APPaymentMethod.Tab.General", ObjectTableId = APPaymentMethodObjectTable.Id, Tenant = 0, NameTextCodeCode = "APPaymentMethod.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode APPaymentMethodAccountingTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPaymentMethod.TH.Accounting", DefaultText = "Accounting",LocalDefaultText = null, ObjectTableId = APPaymentMethodObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature APPaymentMethodAccountingFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "APPaymentMethod.Tab.Accounting", ObjectTableId = APPaymentMethodObjectTable.Id, Tenant = 0, NameTextCodeCode = "APPaymentMethod.Features.Accounting", NameTextCodeDefaultText = "Accounting", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode APPaymentMethodEventsTextCode_TH2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "APPaymentMethod.TH.Events", DefaultText = "Events",LocalDefaultText = null, ObjectTableId = APPaymentMethodObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature APPaymentMethodEventsFeature_TH2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "APPaymentMethod.Tab.Events", ObjectTableId = APPaymentMethodObjectTable.Id, Tenant = 0, NameTextCodeCode = "APPaymentMethod.Features.Events", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "PPGN",HtmlComponentName = "GeneralTabComponent",HtmlComponentUrl = "./Infrastructure/GenericComponents/GeneralTabComponent", FeatureId = APPaymentMethodGeneralFeature_TH0.Id, ControlPath = "Simplog.Infrastructure.GeneralControls.GeneralTabControl", ObjectTableId = APPaymentMethodObjectTable.Id, TabNameTextCodeId = APPaymentMethodGeneralTextCode_TH0.Id, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "PPAC",HtmlComponentName = "",HtmlComponentUrl = "./Common/Components/AccountingTab/AccountingTabComponent", FeatureId = APPaymentMethodAccountingFeature_TH1.Id, ControlPath = "Simplog.Infrastructure.Views.Events.AccountingControl", ObjectTableId = APPaymentMethodObjectTable.Id, TabNameTextCodeId = APPaymentMethodAccountingTextCode_TH1.Id, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "PPEV",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = APPaymentMethodEventsFeature_TH2.Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = APPaymentMethodObjectTable.Id, TabNameTextCodeId = APPaymentMethodEventsTextCode_TH2.Id, Tenant = 0, IndexOrder = 2 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable APPaymentMethodObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "APPaymentMethod" && d.Tenant == 0).FirstOrDefault(); 

		   Feature APPaymentMethodFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = true, ObjectTableId = APPaymentMethodObjectTable.Id, Tenant = 0, NameTextCodeCode = "APPaymentMethod.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature APPaymentMethodFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = true, ObjectTableId = APPaymentMethodObjectTable.Id, Tenant = 0, NameTextCodeCode = "APPaymentMethod.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature APPaymentMethodFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = true, ObjectTableId = APPaymentMethodObjectTable.Id, Tenant = 0, NameTextCodeCode = "APPaymentMethod.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature APPaymentMethodFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = true, ObjectTableId = APPaymentMethodObjectTable.Id, Tenant = 0, NameTextCodeCode = "APPaymentMethod.Features.PackageFeature", NameTextCodeDefaultText = "APPaymentMethod Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);    
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable APPaymentMethodObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "APPaymentMethod" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "UAPM",
                ShortView =  false,
                IsManualEntry =  false,
                LocalName =  "AP Payment Method Updated",
                EnglishName =  "AP Payment Method Updated",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = APPaymentMethodObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CAPM",
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
                ObjectTableId = APPaymentMethodObjectTable.Id,
				 
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
	 