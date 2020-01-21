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
using Logitude.Customs.BL.CloseTables;
using Logitude.CRM.BL.CLoseTable;
using Logitude.BookingLib.BL.CLoseTable;
using Logitude.WarehouseLib.Data.Repositories;
using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.WarehouseLib.BL.CLoseTable;
using Logitude.TimeManagement.Data.Repositories;
using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.BL.CLoseTable;
namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.EntityUpdateClasses
{
   public class CouriersVatUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "Customs.CouriersVat",
			      				    IsNew =  false,
			      				    DBTableName =  "Customs.CouriersVats",
			      				    OldDBTableName =  "Customs.CouriersVats",
			      				    ObjectTableSingular =  "CouriersVat",
			      				    ObjectTablePlural =  "CouriersVats",
			      				    HasCustomFilter =  false,
			      				    HasCustomFields =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  false,
			      				    IsNewWizard =  true,
			      				    LookUp1 =  "VatNumber",
			      				    LookUp2 =  "LocalName",
			      				    KeyPropertyPath =  "Id",
			      				    AutoCompleteSearchWindow =  false,
			      				    IsClosed =  false,
			      				    CacheOnClient =  false,
			      				    EditableFromAutoCompleteWindow =  false,
			      				    HasCounter =  false,
			      				    EnableAddFromLOV =  true,
			      				    IsRestrictable =  false,
			      				    IsMain =  true,
			      				    IsAutoComplete =  true,
			      				    EnableEditFromLOV =  false,
			      				    SortingByObjectField =  "EnglishName",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "BR",
			      				    MaxNumberOfCustomFields =  0,
			      				    NewWizardControlName =  "AddEditCouriersVatComponent",
			      				    LocalDefaultText =  "רשימת בלדרים",
			      				    DefaultText =  "Couriers Vat",
			      				    Code =  "77f9",
			      				    Name =  "Customs.CouriersVat Query Group",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Customs",
			      				    ServerModuleName =  "Customs",
			      				    NewWizardComponentPath =  "./CustomsModules/CustomsCourier/Components/CourierVat/AddEditCouriersVatComponent",
			      				    NoTS =  false,
			      				    HasMenuButtons =  false,
			      				    AllowedForComputingPartners =  false,
			      				    Code1 =  "c1c6",
			      				    Name1 =  " Query Group",
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "VatNumber",
					  						OldFieldName =  "VatNumber",
					  						IsNew =  false,
					  						IsChecked =  false,
					  						IsDeleted =  false,
					  						ObjectTableName =  "Customs.CouriersVat",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  20,
					  						IsRequired =  true,
					  						DisplayOnLookUp =  true,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  20,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  true,
					  						PMPropertyPath =  "VatNumber",
					  						ListPropertyPath =  "VatNumber",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CouriersVat",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "VatNumber",
					  						DefaultText =  "Vat Number",
					  						FullLocalDefaultText =  "ח''פ",
					  						ListFieldLable =  "VatNumberListLable",
					  						ListLableDefaultText =  "Vat Number",
					  						ListLocalDefaultText =  "ח''פ",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						IsSpellCheckedFullFieldLable =  false,
					  						IsSpellCheckedHelpLocalDefaultText =  false,
					  						IsSpellCheckedShortLocalDefaultText =  false,
					  						IsSpellCheckedListLocalDefaultText =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "LocalName",
					  						OldFieldName =  "LocalName",
					  						IsNew =  false,
					  						IsChecked =  false,
					  						IsDeleted =  false,
					  						ObjectTableName =  "Customs.CouriersVat",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  100,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  true,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  100,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Contains",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  true,
					  						PMPropertyPath =  "LocalName",
					  						ListPropertyPath =  "LocalName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CouriersVat",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LocalName",
					  						DefaultText =  "Local Name",
					  						FullLocalDefaultText =  "שם עברית",
					  						ListFieldLable =  "LocalNameListLable",
					  						ListLableDefaultText =  "Local Name",
					  						ListLocalDefaultText =  "שם עברית",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						IsSpellCheckedFullFieldLable =  false,
					  						IsSpellCheckedHelpLocalDefaultText =  false,
					  						IsSpellCheckedShortLocalDefaultText =  false,
					  						IsSpellCheckedListLocalDefaultText =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "EnglishName",
					  						OldFieldName =  "EnglishName",
					  						IsNew =  false,
					  						IsChecked =  false,
					  						IsDeleted =  false,
					  						ObjectTableName =  "Customs.CouriersVat",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  70,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  70,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Contains",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "EnglishName",
					  						ListPropertyPath =  "EnglishName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CouriersVat",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "EnglishName",
					  						DefaultText =  "English Name",
					  						FullLocalDefaultText =  "שם באנגלית",
					  						ListFieldLable =  "EnglishNameListLable",
					  						ListLableDefaultText =  "English Name",
					  						ListLocalDefaultText =  "שם באנגלית",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						IsSpellCheckedFullFieldLable =  false,
					  						IsSpellCheckedHelpLocalDefaultText =  false,
					  						IsSpellCheckedShortLocalDefaultText =  false,
					  						IsSpellCheckedListLocalDefaultText =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "InActive",
					  						OldFieldName =  "InActive",
					  						IsNew =  false,
					  						IsChecked =  false,
					  						IsDeleted =  false,
					  						ObjectTableName =  "Customs.CouriersVat",
					  						FieldsDataType =  "Boolean",
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
					  						PMPropertyPath =  "InActive",
					  						ListPropertyPath =  "InActive",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						DependencyFilter3IsList =  false,
					  						ValidForQuerySection1 =  "Customs.CouriersVat",
					  						IsRestrictable =  false,
					  						DisplayInEntityVariables =  false,
					  						AllowedInCustomerFieldsSettings =  false,
					  						DisplayInSearchWindowFilters =  false,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						DisplayInDocumentReferences =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "InActive",
					  						DefaultText =  "Inactive",
					  						FullLocalDefaultText =  "לא פעיל",
					  						ListFieldLable =  "InActiveListLable",
					  						ListLableDefaultText =  "Inactive",
					  						ListLocalDefaultText =  "לא פעיל",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						IsSpellCheckedFullFieldLable =  false,
					  						IsSpellCheckedHelpLocalDefaultText =  false,
					  						IsSpellCheckedShortLocalDefaultText =  false,
					  						IsSpellCheckedListLocalDefaultText =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "SearchFields",
					  						OldFieldName =  "SearchFields",
					  						IsNew =  false,
					  						IsChecked =  false,
					  						IsDeleted =  false,
					  						ObjectTableName =  "Customs.CouriersVat",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  500,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  false,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  500,
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						Operator =  "Contains",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "SearchFields",
					  						ListPropertyPath =  "SearchFields",
					  						DisplayInLookUpIndex =  0,
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
					  						FullFieldLable =  "SearchFields",
					  						DefaultText =  "Search...",
					  						FullLocalDefaultText =  "חפש...",
					  						ListFieldLable =  "SearchFieldsListLable",
					  						ListLableDefaultText =  "Search...",
					  						ListLocalDefaultText =  "חפש...",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						IsSpellCheckedFullFieldLable =  false,
					  						IsSpellCheckedHelpLocalDefaultText =  false,
					  						IsSpellCheckedShortLocalDefaultText =  false,
					  						IsSpellCheckedListLocalDefaultText =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	    {  
	        FeatureRepository featureRepository = new FeatureRepository(0); 
            List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
            IWebFreightContext objectContext = WebFreightContext.GetContext(0);  
	        QueryGroup CouriersVatQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "77f9", Name = "Customs.CouriersVat Query Group" }, queryGroupRepository);
	        queryGroupRepository.SubmitChanges();

	        ObjectTable CouriersVatObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Customs.CouriersVat" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> CouriersVatObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Customs.CouriersVat").ToList();   

			   TextCode CouriersVatTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CouriersVat.Q.AllCouriersVats", DefaultText = "All Couriers Vat",LocalDefaultText = "רשימת בלדרים", ObjectTableId = CouriersVatObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature CouriersVatFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CouriersVat.Q.AllCouriersVats", ObjectTableId = CouriersVatObjectTable.Id, Tenant = 0, NameTextCodeCode = "CouriersVat.Features.AllCouriersVats", NameTextCodeDefaultText = "AllCouriersVats", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query AllCouriersVatsQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = CouriersVatTextCode_0.Id, Code = "AllCouriersVats",  QueryGroupCode = "77f9", IndexOrder = 0, Tenant = 0, ObjectTableId = CouriersVatObjectTable.Id, QuerySection = "Customs.CouriersVat", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = CouriersVatFeature_0.Id,FeatureUniqeCode= CouriersVatFeature_0.FeatureUniqeCode, DefaultSortName = "EnglishName", DefaultSortDirection = "Ascending" }, queriesRepository, tenantQueries);
	
			 QueryColumn AllCouriersVatsQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCouriersVatsQuery.Id, IndexOrder = 0, ObjectFieldId = CouriersVatObjectFields.Where(d => d.FieldName == "VatNumber" && d.ObjectTableId == CouriersVatObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 150 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllCouriersVatsQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCouriersVatsQuery.Id, IndexOrder = 1, ObjectFieldId = CouriersVatObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == CouriersVatObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 240 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllCouriersVatsQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCouriersVatsQuery.Id, IndexOrder = 2, ObjectFieldId = CouriersVatObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == CouriersVatObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 220 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllCouriersVatsQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCouriersVatsQuery.Id, IndexOrder = 3, ObjectFieldId = CouriersVatObjectFields.Where(d => d.FieldName == "InActive" && d.ObjectTableId == CouriersVatObjectTable.Id).FirstOrDefault().Id, ColumnWidth = 100 }, queryColumnsRepository, tenantQueryColumns);
	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable CouriersVatObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.CouriersVat" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> CouriersVatObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Customs.CouriersVat").ToList();
		       
	      

	         Screen CouriersVatCustomsCouriersVatHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "CouriersVat.Customs.CouriersVatHeaderScreen", Name = "Customs.CouriersVatHeaderScreen", ObjectTableId = CouriersVatObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField CouriersVatCustomsCouriersVatHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = CouriersVatObjectFields.Where(d => d.FieldName == "VatNumber").FirstOrDefault().Id, ScreenId = CouriersVatCustomsCouriersVatHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CouriersVatCustomsCouriersVatHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = CouriersVatObjectFields.Where(d => d.FieldName == "LocalName").FirstOrDefault().Id, ScreenId = CouriersVatCustomsCouriersVatHeaderScreenScreen0.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    CouriersVatObjectTable.HeaderScreenId = CouriersVatCustomsCouriersVatHeaderScreenScreen0.Id;
	   		  
	      

	         Screen CouriersVatGeneralTabScreenScreen1 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "CouriersVat.GeneralTabScreen", Name = "GeneralTabScreen", ObjectTableId = CouriersVatObjectTable.Id, NumberOfColumns = 1, NumberOfRows = 4, IsReadOnly = false }, screensRepository, tenantScreens);
      
            ScreenField CouriersVatGeneralTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = CouriersVatObjectFields.Where(d => d.FieldName == "VatNumber").FirstOrDefault().Id, ScreenId = CouriersVatGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CouriersVatGeneralTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = CouriersVatObjectFields.Where(d => d.FieldName == "LocalName").FirstOrDefault().Id, ScreenId = CouriersVatGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CouriersVatGeneralTabScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ObjectFieldId = CouriersVatObjectFields.Where(d => d.FieldName == "EnglishName").FirstOrDefault().Id, ScreenId = CouriersVatGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CouriersVatGeneralTabScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 3, ObjectFieldId = CouriersVatObjectFields.Where(d => d.FieldName == "InActive").FirstOrDefault().Id, ScreenId = CouriersVatGeneralTabScreenScreen1.Id, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
           

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {    
			 ObjectTable CouriersVatObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.CouriersVat" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode CouriersVatGeneralTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CouriersVat.TH.General", DefaultText = "General",LocalDefaultText = "כללי", ObjectTableId = CouriersVatObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CouriersVatGeneralFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CouriersVat.Tab.General", ObjectTableId = CouriersVatObjectTable.Id, Tenant = 0, NameTextCodeCode = "CouriersVat.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode CouriersVatEventTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CouriersVat.TH.Event", DefaultText = "Event",LocalDefaultText = "אירועים", ObjectTableId = CouriersVatObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CouriersVatEventFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CouriersVat.Tab.Event", ObjectTableId = CouriersVatObjectTable.Id, Tenant = 0, NameTextCodeCode = "CouriersVat.Features.Event", NameTextCodeDefaultText = "Event", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "VCGT",HtmlComponentName = "AddEditCouriersVatComponent",HtmlComponentUrl = "./CustomsModules/CustomsCourier/Components/CourierVat/AddEditCouriersVatComponent", FeatureId = tenantFeatures.Where(d => d.Code == "CouriersVat.Tab.General" && d.ObjectTableId == CouriersVatObjectTable.Id).FirstOrDefault().Id, ControlPath = "", ObjectTableId = CouriersVatObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Customs.CouriersVat.TH.General" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "VCVT",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = tenantFeatures.Where(d => d.Code == "CouriersVat.Tab.Event" && d.ObjectTableId == CouriersVatObjectTable.Id).FirstOrDefault().Id, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = CouriersVatObjectTable.Id, TabNameTextCodeId = tenantTextCodes.Where(d => d.Code == "Customs.CouriersVat.TH.Event" && d.Tenant == 0).FirstOrDefault().Id, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable CouriersVatObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.CouriersVat" && d.Tenant == 0).FirstOrDefault(); 
		   Feature CouriersVatFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = false, ObjectTableId = CouriersVatObjectTable.Id, Tenant = 0, NameTextCodeCode = "CouriersVat.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature CouriersVatFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = false, ObjectTableId = CouriersVatObjectTable.Id, Tenant = 0, NameTextCodeCode = "CouriersVat.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature CouriersVatFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = false, ObjectTableId = CouriersVatObjectTable.Id, Tenant = 0, NameTextCodeCode = "CouriersVat.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature CouriersVatFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", ObjectTableId = CouriersVatObjectTable.Id, Tenant = 0, NameTextCodeCode = "CouriersVat.Features.PackageFeature", NameTextCodeDefaultText = "CouriersVat Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);    
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext)
	    {   
			ObjectTable CouriersVatObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.CouriersVat" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "CREV",
                EnglishName = "Created",
                Tenant = 0,
                AddedManually = false,
				IsManualEntry = false,
                LocalName = "Created",
                ObjectTableId = CouriersVatObjectTable.Id,
                ShortView = true,
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code = "UPEV",
                EnglishName = "Updated",
                Tenant = 0,
                AddedManually = false,
				IsManualEntry = false,
                LocalName = "Updated",
                ObjectTableId = CouriersVatObjectTable.Id,
                ShortView = false,
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
	    }     

   }
    
}
	 