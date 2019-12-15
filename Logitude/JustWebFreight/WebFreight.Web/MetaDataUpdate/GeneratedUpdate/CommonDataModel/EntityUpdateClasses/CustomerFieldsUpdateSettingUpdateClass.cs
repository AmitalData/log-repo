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
   public class CustomerFieldsUpdateSettingUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "CustomerFieldsUpdateSetting",
			      				    DBTableName =  "CustomerFieldsUpdateSettings",
			      				    ObjectTableSingular =  "Customer Fields Update Setting",
			      				    ObjectTablePlural =  "Customer Fields Update Setting",
			      				    DefaultText =  "Customer Fields Update Setting",
			      				    Name =  "CustomerFieldsUpdateSettings",
			      				    IsNewWizard =  true,
			      				    NewWizardControlName =  "Simplog.Infrastructure.NewCustomerFieldsUpdateSettingCommand",
			      				    HasCustomFilter =  false,
			      				    KeyPropertyPath =  "Id",
			      				    AutoCompleteSearchWindow =  false,
			      				    IsClosed =  false,
			      				    CacheOnClient =  false,
			      				    EditableFromAutoCompleteWindow =  false,
			      				    HasCounter =  true,
			      				    EnableEditFromLOV =  false,
			      				    EnableAddFromLOV =  false,
			      				    IsRestrictable =  false,
			      				    IsMain =  true,
			      				    IsAutoComplete =  false,
			      				    CustomFieldsCount =  0,
			      				    HasCustomFields =  false,
			      				    InActive =  false,
			      				    SearchFields =  "CustomerFieldsUpdateSetting,CustomerFieldsUpdateSettings,Simplog.Infrastructure.NewCustomerFieldsUpdateSettingCommand,Id,",
			      				    IsSaveButtonVisible =  true,
			      				    EnableSecurity =  true,
			      				    ObjectTableTypeCode =  "MD",
			      				    IsComposition =  false,
			      				    MaxNumberOfCustomFields =  0,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    IsEditable =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasMenuButtons =  false,
			      				    HasFiltersMenu =  false,
			      				    ClientModuleName =  "Common",
			      				    NewWizardComponentPath =  "./Common/Components/Maintenance/CustomerFieldsUpdateSetting/AddEditCustomerFieldsUpdateSettingComponent",
			      				    Code =  "CPQU",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ObjectFieldName",
					  						ObjectTableName =  "CustomerFieldsUpdateSetting",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
					  						MaxLength =  200,
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
					  						PMPropertyPath =  "ObjectFieldName",
					  						ListPropertyPath =  "ObjectFieldName",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "CustomerFieldsUpdateSetting",
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
					  						HasTemplate =  false,
					  						FullFieldLable =  "ObjectFieldName",
					  						DefaultText =  "Object Field Name",
					  						ListFieldLable =  "ObjectFieldNameListLable",
					  						ListLableDefaultText =  "Object Field Name",
					  						HelpTextCode =  "ObjectFieldName",
					  						Code =  "ObjectFieldName",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "ObjectFieldId",
					  						ObjectTableName =  "CustomerFieldsUpdateSetting",
					  						FieldsDataType =  "LookUp",
					  						LookUpTableName =  "ObjectTable",
					  						DataTypeCode =  "LookUp",
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
					  						PMPropertyPath =  "ObjectFieldId",
					  						ListPropertyPath =  "ObjectFieldId",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "CustomerFieldsUpdateSetting",
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
					  						HasTemplate =  false,
					  						FullFieldLable =  "ObjectFieldId",
					  						DefaultText =  "Object Field",
					  						ListFieldLable =  "ObjectFieldIdListLable",
					  						ListLableDefaultText =  "Object Field",
					  						HelpTextCode =  "ObjectFieldId",
					  						Code =  "ObjectFieldId",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "UpdateDirection",
					  						ObjectTableName =  "CustomerFieldsUpdateSetting",
					  						FieldsDataType =  "Text",
					  						DataTypeCode =  "Text",
					  						MaxLength =  20,
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
					  						PMPropertyPath =  "UpdateDirection",
					  						ListPropertyPath =  "UpdateDirection",
					  						DisplayInLookUpIndex =  0,
					  						AutomaticField =  false,
					  						UniqueField =  false,
					  						DisplayInSearchWindowListIndex =  0,
					  						DisplayInSearchWindowFiltersIndex =  0,
					  						IsMulti =  false,
					  						ValidForQuerySection1 =  "CustomerFieldsUpdateSetting",
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
					  						HasTemplate =  false,
					  						FullFieldLable =  "UpdateDirection",
					  						DefaultText =  "Update Direction",
					  						ListFieldLable =  "UpdateDirectionListLable",
					  						ListLableDefaultText =  "Update Direction",
					  						HelpTextCode =  "UpdateDirection",
					  						Code =  "UpdateDirection",
					  						DependencyFilter3IsList =  false,
					  						DisplayInDocumentReferences =  false,
					  						CopyToDW =  false,
					  						IsRequired =  true,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	    {  
	        FeatureRepository featureRepository = new FeatureRepository(0); 
            //List<Feature> tenantFeatures = featureRepository.GetFeaturesByTenant(0).ToList();
            IWebFreightContext objectContext = WebFreightContext.GetContext(0);  
	        QueryGroup CustomerFieldsUpdateSettingQueryGroup = AddQueryGroups.AddQueryGroup(new QueryGroupDetails() { Code = "CPQU", Name = "CustomerFieldsUpdateSettings" }, queryGroupRepository);
				        queryGroupRepository.SubmitChanges();

	        ObjectTable CustomerFieldsUpdateSettingObjectTable = objectContext.ObjectTables.Where(d => d.Name == "CustomerFieldsUpdateSetting" && d.Tenant == 0).FirstOrDefault();
	        List<ObjectField> CustomerFieldsUpdateSettingObjectFields = objectContext.ObjectFields.Where(d => d.ObjectTable.Name == "CustomerFieldsUpdateSetting").ToList();   

			   TextCode CustomerFieldsUpdateSettingTextCode_0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CustomerFieldsUpdateSetting.Q.AllCustomerFieldsUpdateSettings", DefaultText = @"All Customer Fields Update Settings",LocalDefaultText = null, ObjectTableId = CustomerFieldsUpdateSettingObjectTable.Id, Tenant = 0, TextCodeTypeCode = "Q", }, TextCodeRepository, textCodes);
			   Feature CustomerFieldsUpdateSettingFeature_0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ALLCUSTOMERFIELDSUPDATESETTING", ObjectTableId = CustomerFieldsUpdateSettingObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomerFieldsUpdateSetting.Features.AllCustomerFieldsUpdateSetting", NameTextCodeDefaultText = "All Customer Fields Update Setting", FeatureTypeCode = "QUER", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);

	        TextCodeRepository.SubmitChanges();
	        FeaturesRepository.SubmitChanges();    
	      

			  Query AllCustomerFieldsUpdateQuery = AddQueries.AddQuery(new QueryDetails() { NameTextCodeId = CustomerFieldsUpdateSettingTextCode_0.Id, Code = "All Customer Fields Update",  EditWizardComponentPath = "./Common/Components/Maintenance/CustomerFieldsUpdateSetting/AddEditCustomerFieldsUpdateSettingComponent",
			   QueryGroupCode = "CPQU", IndexOrder = 1, Tenant = 0, ObjectTableId = CustomerFieldsUpdateSettingObjectTable.Id, QuerySection = "CustomerFieldsUpdateSetting", SystemLevel = true, IsAddNewEntityEnabled = true, FeatureId = CustomerFieldsUpdateSettingFeature_0.Id, DefaultSortName = null, DefaultSortDirection = null, Perspective = null }, queriesRepository, tenantQueries);
	
			 QueryColumn AllCustomerFieldsUpdateQueryColumn_0 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCustomerFieldsUpdateQuery.Id, IndexOrder = 1, ObjectFieldId = CustomerFieldsUpdateSettingObjectFields.Where(d => d.FieldName == "ObjectFieldName" && d.ObjectTableId == CustomerFieldsUpdateSettingObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomerFieldsUpdateSettingObjectFields.Where(d => d.FieldName == "ObjectFieldName" && d.ObjectTableId == CustomerFieldsUpdateSettingObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);

			 QueryColumn AllCustomerFieldsUpdateQueryColumn_1 = AddQueries.AddQueryColumn(new QueryColumnDetails { Tenant = 0, QueryId = AllCustomerFieldsUpdateQuery.Id, IndexOrder = 2, ObjectFieldId = CustomerFieldsUpdateSettingObjectFields.Where(d => d.FieldName == "UpdateDirection" && d.ObjectTableId == CustomerFieldsUpdateSettingObjectTable.Id).FirstOrDefault().Id, ObjectFieldCode = CustomerFieldsUpdateSettingObjectFields.Where(d => d.FieldName == "UpdateDirection" && d.ObjectTableId == CustomerFieldsUpdateSettingObjectTable.Id).FirstOrDefault().FieldCode, ColumnWidth = 130 }, queryColumnsRepository, tenantQueryColumns);
	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable CustomerFieldsUpdateSettingObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "CustomerFieldsUpdateSetting" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> CustomerFieldsUpdateSettingObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "CustomerFieldsUpdateSetting").ToList();
		       
	      

	         Screen CustomerFieldsUpdateSettingHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "CustomerFieldsUpdateSetting.HeaderScreen", Name = "Header Screen", ObjectTableId = CustomerFieldsUpdateSettingObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      
            ScreenField CustomerFieldsUpdateSettingCustomerFieldsUpdateSettingHeaderScreenScreenField0 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 0, Row = 0, ObjectFieldId = CustomerFieldsUpdateSettingObjectFields.Where(d => d.FieldName == "ObjectFieldName").FirstOrDefault().Id, ScreenId = CustomerFieldsUpdateSettingHeaderScreenScreen0.Id, ObjectFieldCode = CustomerFieldsUpdateSettingObjectFields.Where(d => d.FieldName == "ObjectFieldName").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         
            ScreenField CustomerFieldsUpdateSettingCustomerFieldsUpdateSettingHeaderScreenScreenField1 = AddScreensAndScreenFields.AddScreenField(new ScreenFieldDetails() { Column = 1, Row = 0, ObjectFieldId = CustomerFieldsUpdateSettingObjectFields.Where(d => d.FieldName == "UpdateDirection").FirstOrDefault().Id, ScreenId = CustomerFieldsUpdateSettingHeaderScreenScreen0.Id, ObjectFieldCode = CustomerFieldsUpdateSettingObjectFields.Where(d => d.FieldName == "UpdateDirection").FirstOrDefault().FieldCode, Tenant = 0, }, screenFieldsRepository, tenantScreenFields);
         	
		    CustomerFieldsUpdateSettingObjectTable.HeaderScreenId = CustomerFieldsUpdateSettingHeaderScreenScreen0.Id;
	   		  

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {                
			   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault();   
			   ObjectTable CustomerFieldsUpdateSettingObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "CustomerFieldsUpdateSetting" && d.Tenant == 0).FirstOrDefault();  
                 
			   TextCode CustomerFieldsUpdateSettingGeneralTextCode_TH0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CustomerFieldsUpdateSetting.TH.General", DefaultText = "General",LocalDefaultText = null, ObjectTableId = CustomerFieldsUpdateSettingObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", }, TextCodeRepository, textCodes);
			   Feature CustomerFieldsUpdateSettingGeneralFeature_TH0 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GENERAL", ObjectTableId = CustomerFieldsUpdateSettingObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomerFieldsUpdateSetting.Features.General", NameTextCodeDefaultText = "General", FeatureTypeCode = "AREA", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, textCodes);
			 TextCodeRepository.SubmitChanges();
			 FeaturesRepository.SubmitChanges();
			 //List<Feature> tenantFeatures = FeaturesRepository.GetFeaturesByTenant(0).ToList(); 
			 //List<TextCode> tenantTextCodes = TextCodeRepository.GetTextCodesByTenant(0).ToList();
			    
            AddObjectTableTabs.AddObjectTableTab(new ObjectTableTabDetails() { Code = "CFUG",HtmlComponentName = "",HtmlComponentUrl = "", FeatureId = CustomerFieldsUpdateSettingGeneralFeature_TH0.Id, ControlPath = "Simplog.InfrastructureExt.Views.Maintenance.CustomerFieldsUpdateSetting.EditCustomerFieldsUpdateSettingControl", ObjectTableId = CustomerFieldsUpdateSettingObjectTable.Id, TabNameTextCodeId = CustomerFieldsUpdateSettingGeneralTextCode_TH0.Id, Tenant = 0, IndexOrder = 0 }, objectTableTabsRepository, TenantObjectTableTabs);
   
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable CustomerFieldsUpdateSettingObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "CustomerFieldsUpdateSetting" && d.Tenant == 0).FirstOrDefault(); 

		   Feature CustomerFieldsUpdateSettingFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = true, ObjectTableId = CustomerFieldsUpdateSettingObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomerFieldsUpdateSetting.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature CustomerFieldsUpdateSettingFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = true, ObjectTableId = CustomerFieldsUpdateSettingObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomerFieldsUpdateSetting.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature CustomerFieldsUpdateSettingFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = true, ObjectTableId = CustomerFieldsUpdateSettingObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomerFieldsUpdateSetting.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature CustomerFieldsUpdateSettingFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = true, ObjectTableId = CustomerFieldsUpdateSettingObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomerFieldsUpdateSetting.Features.PackageFeature", NameTextCodeDefaultText = "CustomerFieldsUpdateSetting Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);    
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable CustomerFieldsUpdateSettingObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "CustomerFieldsUpdateSetting" && d.Tenant == 0).FirstOrDefault(); 
	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {     
	    
}

    

   }
    
}
	 