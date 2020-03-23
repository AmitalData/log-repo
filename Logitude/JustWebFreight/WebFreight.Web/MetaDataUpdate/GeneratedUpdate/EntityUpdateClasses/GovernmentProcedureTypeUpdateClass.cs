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

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.EntityUpdateClasses
{
   public class GovernmentProcedureTypeUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "Customs.GovernmentProcedureType",
			      				    DBTableName =  "Customs.GovernmentProcedureTypes",
			      				    ObjectTableSingular =  "Customs.GovernmentProcedureType",
			      				    ObjectTablePlural =  "Customs.GovernmentProcedureTypes",
			      				    HasCustomFilter =  false,
			      				    HasCustomFields =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    IsEditable =  false,
			      				    IsNewWizard =  false,
			      				    LookUp1 =  "Code",
			      				    LookUp2 =  "LocalName",
			      				    KeyPropertyPath =  "Code",
			      				    AutoCompleteSearchWindow =  false,
			      				    IsClosed =  false,
			      				    CacheOnClient =  true,
			      				    EditableFromAutoCompleteWindow =  false,
			      				    HasCounter =  false,
			      				    EnableAddFromLOV =  false,
			      				    IsRestrictable =  false,
			      				    IsMain =  true,
			      				    IsAutoComplete =  true,
			      				    EnableEditFromLOV =  false,
			      				    SortingByObjectField =  "Code",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    DefaultText =  "Government Procedure Type",
			      				    Code =  "GOPT",
			      				    Name =  "Customs.GovernmentProcedureType",
			      				    CloseTableCode =  "Code",
			      				    CloseTableName =  "LocalName",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Customs",
			      				    ServerModuleName =  "Customs",
			      				    NoTS =  false,
			      				    HasMenuButtons =  false,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Code",
					  						ObjectTableName =  "Customs.GovernmentProcedureType",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  7,
					  						IsRequired =  true,
					  						DisplayOnLookUp =  true,
					  						CanFilter =  true,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  7,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Code",
					  						ListPropertyPath =  "Code",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  true,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "Customs.GovernmentProcedureType",
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Code",
					  						DefaultText =  "Code",
					  						FullLocalDefaultText =  "קוד",
					  						ListFieldLable =  "CodeListLable",
					  						ListLableDefaultText =  "Code",
					  						ListLocalDefaultText =  "קוד",
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
					 
					 						FieldName =  "LocalName",
					  						ObjectTableName =  "Customs.GovernmentProcedureType",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  60,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  true,
					  						CanFilter =  true,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  60,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  true,
					  						PMPropertyPath =  "LocalName",
					  						ListPropertyPath =  "LocalName",
					  						DisplayInLookUpIndex =  1,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  1,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "Customs.GovernmentProcedureType",
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "LocalName",
					  						DefaultText =  "Local Name",
					  						FullLocalDefaultText =  "שם מקומי",
					  						ListFieldLable =  "LocalNameListLable",
					  						ListLableDefaultText =  "Local Name",
					  						ListLocalDefaultText =  "שם מקומי",
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
					 
					 						FieldName =  "EnglishName",
					  						ObjectTableName =  "Customs.GovernmentProcedureType",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  60,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  false,
					  						SystemRequired =  false,
					  						SystemMaxLength =  60,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "StartsWith",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  true,
					  						PMPropertyPath =  "EnglishName",
					  						ListPropertyPath =  "EnglishName",
					  						DisplayInLookUpIndex =  1,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  1,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "Customs.GovernmentProcedureType",
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "EnglishName",
					  						DefaultText =  "English Name",
					  						FullLocalDefaultText =  "שם אנגלית",
					  						ListFieldLable =  "EnglishNameListLable",
					  						ListLableDefaultText =  "English Name",
					  						ListLocalDefaultText =  "שם אנגלית",
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
					 
					 						FieldName =  "SearchFields",
					  						ObjectTableName =  "Customs.GovernmentProcedureType",
					  						FieldsDataType =  "nText",
					  						MinLength =  0,
					  						MaxLength =  1000,
					  						IsRequired =  false,
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
					  						PMPropertyPath =  "SearchFields",
					  						ListPropertyPath =  "SearchFields",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "Customs.GovernmentProcedureType",
					  						DisplayInEntityVariables =  true,
					  						DigitsAfterPoint =  0,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "SearchFields",
					  						DefaultText =  "Search codes/ names",
					  						ListFieldLable =  "SearchFieldsListLable",
					  						ListLableDefaultText =  "SearchFields",
					  						HelpTextCode =  "SearchFields",
					  						HelpTextDefaultText =  "Searching by :\n1: code\n2: name",
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
					 
					 						FieldName =  "Inactive",
					  						ObjectTableName =  "Customs.GovernmentProcedureType",
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
					  						PMPropertyPath =  "Inactive",
					  						ListPropertyPath =  "Inactive",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "Customs.GovernmentProcedureType",
					  						DisplayInEntityVariables =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "Inactive",
					  						DefaultText =  "Inactive ",
					  						ListFieldLable =  "InactiveListLable",
					  						ListLableDefaultText =  "Inactive",
					  						ListLocalDefaultText =  "לא פּעיל",
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
					 
					 						FieldName =  "IsImport",
					  						ObjectTableName =  "Customs.GovernmentProcedureType",
					  						FieldsDataType =  "Boolean",
					  						MinLength =  0,
					  						MaxLength =  0,
					  						IsRequired =  false,
					  						DisplayOnLookUp =  false,
					  						CanFilter =  true,
					  						DisplayOnly =  true,
					  						SystemRequired =  false,
					  						SystemMaxLength =  0,
					  						DisplayInList =  true,
					  						IsCustomFilter =  false,
					  						Operator =  "Equals",
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "IsImport",
					  						ListPropertyPath =  "IsImport",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "Customs.GovernmentProcedureType",
					  						DisplayInEntityVariables =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IsImport",
					  						DefaultText =  "Is Import",
					  						FullLocalDefaultText =  "האם יבוא",
					  						ListFieldLable =  "IsImportListLable",
					  						ListLableDefaultText =  "Is Import",
					  						ListLocalDefaultText =  "האם יבוא",
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
					 
					 						FieldName =  "IndexOrder",
					  						ObjectTableName =  "Customs.GovernmentProcedureType",
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
					  						PMPropertyPath =  "IndexOrder",
					  						ListPropertyPath =  "IndexOrder",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						IsMulti =  false,
					  						DependencyFilter1IsList =  false,
					  						DependencyFilter2IsList =  false,
					  						ValidForQuerySection1 =  "Customs.GovernmentProcedureType",
					  						DisplayInEntityVariables =  false,
					  						InActive =  false,
					  						DisplayLongName =  false,
					  						FullFieldLable =  "IndexOrder",
					  						DefaultText =  "Order",
					  						ListFieldLable =  "IndexOrderListLable",
					  						ListLableDefaultText =  "Order",
					  						ListLocalDefaultText =  "להזמין",
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
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	    {  
	        FeatureRepository featureRepository = new FeatureRepository(0); 
            //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
            IWebFreightContext objectContext = WebFreightContext.GetContext(0);  
	        QueryGroup GovernmentProcedureTypeQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "GOPT", Name = "Customs.GovernmentProcedureType" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable GovernmentProcedureTypeObjectTable = objectContext.ObjectTables.Where(d => d.Name == "Customs.GovernmentProcedureType" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> GovernmentProcedureTypeObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Customs.GovernmentProcedureType").ToList();   

			   TextCode GovernmentProcedureTypeTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "GovernmentProcedureType.Q.GovernmentProcedureType", DefaultText = @"GovernmentProcedureType",LocalDefaultText = "GovernmentProcedureType", ObjectTableId = GovernmentProcedureTypeObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature GovernmentProcedureTypeFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GovernmentProcedureType.Q.GovernmentProcedureType", ObjectTableId = GovernmentProcedureTypeObjectTable.Id, Tenant = 0, NameTextCodeCode = "GovernmentProcedureTypeFeatures.GovernmentProcedureType", NameTextCodeDefaultText = "GovernmentProcedureType", FeatureTypeCode = "QUER", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query GovernmentProcedureTypeQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = GovernmentProcedureTypeTextCode_0.Id, NameTextCodeCode = GovernmentProcedureTypeTextCode_0.Code, ObjectTableName = "Customs.GovernmentProcedureType", Code = "GovernmentProcedureType",  QueryGroupCode = "GOPT", IndexOrder = 0, Tenant = 0, ObjectTableId = GovernmentProcedureTypeObjectTable.Id, QuerySection = "Customs.GovernmentProcedureType", SystemLevel = true, IsAddNewEntityEnabled = false, FeatureId = GovernmentProcedureTypeFeature_0.Id,FeatureUniqeCode= GovernmentProcedureTypeFeature_0.FeatureUniqeCode, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn GovernmentProcedureTypeQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = GovernmentProcedureTypeQuery.Id,QueryCode = GovernmentProcedureTypeQuery.UniqueCode, IndexOrder = 0, ObjectFieldId = GovernmentProcedureTypeObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == GovernmentProcedureTypeObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GovernmentProcedureTypeObjectFields.Where(d => d.FieldName == "Code" && d.ObjectTableId == GovernmentProcedureTypeObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn GovernmentProcedureTypeQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = GovernmentProcedureTypeQuery.Id,QueryCode = GovernmentProcedureTypeQuery.UniqueCode, IndexOrder = 1, ObjectFieldId = GovernmentProcedureTypeObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GovernmentProcedureTypeObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GovernmentProcedureTypeObjectFields.Where(d => d.FieldName == "EnglishName" && d.ObjectTableId == GovernmentProcedureTypeObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn GovernmentProcedureTypeQueryColumn_2 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = GovernmentProcedureTypeQuery.Id,QueryCode = GovernmentProcedureTypeQuery.UniqueCode, IndexOrder = 2, ObjectFieldId = GovernmentProcedureTypeObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GovernmentProcedureTypeObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GovernmentProcedureTypeObjectFields.Where(d => d.FieldName == "LocalName" && d.ObjectTableId == GovernmentProcedureTypeObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn GovernmentProcedureTypeQueryColumn_3 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = GovernmentProcedureTypeQuery.Id,QueryCode = GovernmentProcedureTypeQuery.UniqueCode, IndexOrder = 3, ObjectFieldId = GovernmentProcedureTypeObjectFields.Where(d => d.FieldName == "IsImport" && d.ObjectTableId == GovernmentProcedureTypeObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GovernmentProcedureTypeObjectFields.Where(d => d.FieldName == "IsImport" && d.ObjectTableId == GovernmentProcedureTypeObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn GovernmentProcedureTypeQueryColumn_4 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = GovernmentProcedureTypeQuery.Id,QueryCode = GovernmentProcedureTypeQuery.UniqueCode, IndexOrder = 4, ObjectFieldId = GovernmentProcedureTypeObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GovernmentProcedureTypeObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = GovernmentProcedureTypeObjectFields.Where(d => d.FieldName == "Inactive" && d.ObjectTableId == GovernmentProcedureTypeObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable GovernmentProcedureTypeObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.GovernmentProcedureType" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> GovernmentProcedureTypeObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "Customs.GovernmentProcedureType").ToList();
		       
	      

	         Screen GovernmentProcedureTypeGeneralTabScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "GovernmentProcedureType.GeneralTabScreen", Name = "GeneralTabScreen", ObjectTableId = GovernmentProcedureTypeObjectTable.Id, NumberOfColumns = 1, NumberOfRows = 4, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField GovernmentProcedureTypeGovernmentProcedureTypeGeneralTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = GovernmentProcedureTypeObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().Id, ScreenId = GovernmentProcedureTypeGeneralTabScreenScreen0.Id,ScreenCode = GovernmentProcedureTypeGeneralTabScreenScreen0.Code, ObjectFieldCode = GovernmentProcedureTypeObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField GovernmentProcedureTypeGovernmentProcedureTypeGeneralTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = GovernmentProcedureTypeObjectFields.Where(d => d.FieldName == "LocalName").FirstOrDefault().Id, ScreenId = GovernmentProcedureTypeGeneralTabScreenScreen0.Id,ScreenCode = GovernmentProcedureTypeGeneralTabScreenScreen0.Code, ObjectFieldCode = GovernmentProcedureTypeObjectFields.Where(d => d.FieldName == "LocalName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField GovernmentProcedureTypeGovernmentProcedureTypeGeneralTabScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ObjectFieldId = GovernmentProcedureTypeObjectFields.Where(d => d.FieldName == "IsImport").FirstOrDefault().Id, ScreenId = GovernmentProcedureTypeGeneralTabScreenScreen0.Id,ScreenCode = GovernmentProcedureTypeGeneralTabScreenScreen0.Code, ObjectFieldCode = GovernmentProcedureTypeObjectFields.Where(d => d.FieldName == "IsImport").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField GovernmentProcedureTypeGovernmentProcedureTypeGeneralTabScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 3, ObjectFieldId = GovernmentProcedureTypeObjectFields.Where(d => d.FieldName == "IndexOrder").FirstOrDefault().Id, ScreenId = GovernmentProcedureTypeGeneralTabScreenScreen0.Id,ScreenCode = GovernmentProcedureTypeGeneralTabScreenScreen0.Code, ObjectFieldCode = GovernmentProcedureTypeObjectFields.Where(d => d.FieldName == "IndexOrder").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
           
	      

	         Screen GovernmentProcedureTypeHeaderScreenScreen1 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Customs.GovernmentProcedureType.HeaderScreen", Name = "Header Screen", ObjectTableId = GovernmentProcedureTypeObjectTable.Id, NumberOfColumns = 3, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField GovernmentProcedureTypeCustomsGovernmentProcedureTypeHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = GovernmentProcedureTypeObjectFields.Where(d => d.FieldName == "EnglishName").FirstOrDefault().Id, ScreenId = GovernmentProcedureTypeHeaderScreenScreen1.Id,ScreenCode = GovernmentProcedureTypeHeaderScreenScreen1.Code, ObjectFieldCode = GovernmentProcedureTypeObjectFields.Where(d => d.FieldName == "EnglishName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField GovernmentProcedureTypeCustomsGovernmentProcedureTypeHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = GovernmentProcedureTypeObjectFields.Where(d => d.FieldName == "LocalName").FirstOrDefault().Id, ScreenId = GovernmentProcedureTypeHeaderScreenScreen1.Id,ScreenCode = GovernmentProcedureTypeHeaderScreenScreen1.Code, ObjectFieldCode = GovernmentProcedureTypeObjectFields.Where(d => d.FieldName == "LocalName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    GovernmentProcedureTypeObjectTable.HeaderScreenId = GovernmentProcedureTypeHeaderScreenScreen1.Id;
		    GovernmentProcedureTypeObjectTable.HeaderScreenCode = GovernmentProcedureTypeHeaderScreenScreen1.Code;

	   		  
	      

	         Screen GovernmentProcedureTypeGeneralTabScreenScreen2 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "Customs.GovernmentProcedureType.GeneralTabScreen", Name = "General Tab Screen", ObjectTableId = GovernmentProcedureTypeObjectTable.Id, NumberOfColumns = 1, NumberOfRows = 5, IsReadOnly = false }, screensRepository, tenantScreens);
      
            ScreenField GovernmentProcedureTypeCustomsGovernmentProcedureTypeGeneralTabScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = GovernmentProcedureTypeObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().Id, ScreenId = GovernmentProcedureTypeGeneralTabScreenScreen2.Id,ScreenCode = GovernmentProcedureTypeGeneralTabScreenScreen2.Code, ObjectFieldCode = GovernmentProcedureTypeObjectFields.Where(d => d.FieldName == "Code").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField GovernmentProcedureTypeCustomsGovernmentProcedureTypeGeneralTabScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 1, ObjectFieldId = GovernmentProcedureTypeObjectFields.Where(d => d.FieldName == "EnglishName").FirstOrDefault().Id, ScreenId = GovernmentProcedureTypeGeneralTabScreenScreen2.Id,ScreenCode = GovernmentProcedureTypeGeneralTabScreenScreen2.Code, ObjectFieldCode = GovernmentProcedureTypeObjectFields.Where(d => d.FieldName == "EnglishName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField GovernmentProcedureTypeCustomsGovernmentProcedureTypeGeneralTabScreenScreenField2 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 2, ObjectFieldId = GovernmentProcedureTypeObjectFields.Where(d => d.FieldName == "LocalName").FirstOrDefault().Id, ScreenId = GovernmentProcedureTypeGeneralTabScreenScreen2.Id,ScreenCode = GovernmentProcedureTypeGeneralTabScreenScreen2.Code, ObjectFieldCode = GovernmentProcedureTypeObjectFields.Where(d => d.FieldName == "LocalName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField GovernmentProcedureTypeCustomsGovernmentProcedureTypeGeneralTabScreenScreenField3 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 3, ObjectFieldId = GovernmentProcedureTypeObjectFields.Where(d => d.FieldName == "IsImport").FirstOrDefault().Id, ScreenId = GovernmentProcedureTypeGeneralTabScreenScreen2.Id,ScreenCode = GovernmentProcedureTypeGeneralTabScreenScreen2.Code, ObjectFieldCode = GovernmentProcedureTypeObjectFields.Where(d => d.FieldName == "IsImport").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField GovernmentProcedureTypeCustomsGovernmentProcedureTypeGeneralTabScreenScreenField4 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 4, ObjectFieldId = GovernmentProcedureTypeObjectFields.Where(d => d.FieldName == "IndexOrder").FirstOrDefault().Id, ScreenId = GovernmentProcedureTypeGeneralTabScreenScreen2.Id,ScreenCode = GovernmentProcedureTypeGeneralTabScreenScreen2.Code, ObjectFieldCode = GovernmentProcedureTypeObjectFields.Where(d => d.FieldName == "IndexOrder").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
           

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable GovernmentProcedureTypeObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.GovernmentProcedureType" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode GovernmentProcedureTypeGeneralTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GovernmentProcedureType.TH.General", DefaultText = "General",LocalDefaultText = "כללי", ObjectTableId = GovernmentProcedureTypeObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature GovernmentProcedureTypeGeneralFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GovernmentProcedureType.Tab.General", ObjectTableId = GovernmentProcedureTypeObjectTable.Id, Tenant = 0, NameTextCodeCode = "GovernmentProcedureTypeFeatures.GPGN", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
 
                 
			   TextCode GovernmentProcedureTypeEventsTextCode_TH1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GovernmentProcedureType.TH.Events", DefaultText = "Events",LocalDefaultText = "אירועים", ObjectTableId = GovernmentProcedureTypeObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature GovernmentProcedureTypeEventsFeature_TH1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GovernmentProcedureType.Tab.Events", ObjectTableId = GovernmentProcedureTypeObjectTable.Id, Tenant = 0, NameTextCodeCode = "GovernmentProcedureTypeFeatures.GPEV", NameTextCodeDefaultText = "Events", FeatureTypeCode = "AREA", Packagable = false }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "GPGN",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = GovernmentProcedureTypeGeneralFeature_TH0.Id,FeatureUniqeCode = GovernmentProcedureTypeGeneralFeature_TH0.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.GeneralControls.GeneralTabControl", ObjectTableId = GovernmentProcedureTypeObjectTable.Id, TabNameTextCodeId = GovernmentProcedureTypeGeneralTextCode_TH0.Id, TabNameTextCodeCode = GovernmentProcedureTypeGeneralTextCode_TH0.Code, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "GPEV",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = GovernmentProcedureTypeEventsFeature_TH1.Id,FeatureUniqeCode = GovernmentProcedureTypeEventsFeature_TH1.FeatureUniqeCode, ControlPath = "Simplog.Infrastructure.Views.Events.EventsControl", ObjectTableId = GovernmentProcedureTypeObjectTable.Id, TabNameTextCodeId = GovernmentProcedureTypeEventsTextCode_TH1.Id, TabNameTextCodeCode = GovernmentProcedureTypeEventsTextCode_TH1.Code, Tenant = 0, IndexOrder = 1 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable GovernmentProcedureTypeObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.GovernmentProcedureType" && d.Tenant == 0).FirstOrDefault(); 

		   Feature GovernmentProcedureTypeFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = true, ObjectTableId = GovernmentProcedureTypeObjectTable.Id, Tenant = 0, NameTextCodeCode = "GovernmentProcedureType.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature GovernmentProcedureTypeFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = true, ObjectTableId = GovernmentProcedureTypeObjectTable.Id, Tenant = 0, NameTextCodeCode = "GovernmentProcedureType.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature GovernmentProcedureTypeFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = true, ObjectTableId = GovernmentProcedureTypeObjectTable.Id, Tenant = 0, NameTextCodeCode = "GovernmentProcedureType.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature GovernmentProcedureTypeFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = true, ObjectTableId = GovernmentProcedureTypeObjectTable.Id, Tenant = 0, NameTextCodeCode = "GovernmentProcedureType.Features.PackageFeature", NameTextCodeDefaultText = "GovernmentProcedureType Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes); 

		   		   //--------------> Additional Features <--------------\\

		   Feature GovernmentProcedureTypeFeature_GENERAL = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GENERAL", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GovernmentProcedureTypeObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.GovernmentProcedureType.Features.General", NameTextCodeDefaultText = @"General" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GovernmentProcedureTypeFeature_EVENTS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EVENTS", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GovernmentProcedureTypeObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.GovernmentProcedureType.Features.Events", NameTextCodeDefaultText = @"Events" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GovernmentProcedureTypeFeature_GOVERNMENTPROCEDURETYPE = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GOVERNMENTPROCEDURETYPE", FeatureTypeCode = "QUER", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GovernmentProcedureTypeObjectTable.Id, Tenant = 0, NameTextCodeCode = "Customs.GovernmentProcedureType.Features.GovernmentProcedureTypes", NameTextCodeDefaultText = @"Government Procedure Types" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

   
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable GovernmentProcedureTypeObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "Customs.GovernmentProcedureType" && d.Tenant == 0).FirstOrDefault(); 
            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "CREV",
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
                ObjectTableId = GovernmentProcedureTypeObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


            AddEventTypes.AddEventType(new EventTypeDetails()
            {
                Code =  "UPEV",
                ShortView =  false,
                IsManualEntry =  false,
                LocalName =  "Updated",
                EnglishName =  "Updated",
                EventTypeCategoryCode =  "OPE",
                IsAgentView =  false,
                IsCustomerView =  false,
                IsSharedLogisticsEnabled =  false,
                AllowedInAutomation =  false,
                ManualActivatedFollowUp =  false,
                IsFollowUp =  false,
                ObjectTableId = GovernmentProcedureTypeObjectTable.Id,
				 
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
	 