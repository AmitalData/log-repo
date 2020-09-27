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
using Logitude.CargoTracking.Data.Repositories;
using Logitude.CargoTracking.BL;
using Logitude.CargoTracking.Data.EntityPOCOs;

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.EntityUpdateClasses
{
   public class CustomsGeneralUpdateClass
   {  		
		public const string HashString = "08a100488da5537225454277c4466bfe";
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "CustomsGeneral",
			      				    IsNew =  false,
			      				    DBTableName =  "CustomsGenerals",
			      				    ObjectTableSingular =  "CustomsGeneral",
			      				    ObjectTablePlural =  "CustomsGenerals",
			      				    HasCustomFilter =  false,
			      				    HasCustomFields =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  true,
			      				    IsNewWizard =  false,
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
			      				    SortingByObjectField =  "Id",
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  false,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    DefaultText =  "CustomsGeneral",
			      				    Code =  "e4fa",
			      				    Name =  "CustomsGeneral",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Customs",
			      				    ServerModuleName =  "Customs",
			      				    NoTS =  true,
			      				    HasMenuButtons =  false,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    IsTabsHidden =  false,
			      				    SearchFields =  "CustomsGeneral,CustomsGenerals,,Id,",
			      				    HashString =  CustomsGeneralUpdateClass.HashString,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository, List<ObjectField> addedFields, List<TextCode> addedTextCodes)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Id",
					  						ObjectTableName =  "CustomsGeneral",
					  						FieldsDataType =  "Text",
					  						MinLength =  0,
					  						MaxLength =  15,
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
					  						PMPropertyPath =  "Id",
					  						ListPropertyPath =  "Id",
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
					  						FullFieldLable =  "Id",
					  						DefaultText =  "id",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Tenant",
					  						ObjectTableName =  "CustomsGeneral",
					  						FieldsDataType =  "Integer",
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
					  						DisplayInList =  false,
					  						IsCustomFilter =  false,
					  						MultiLine =  false,
					  						IsTimeFrameFilter =  false,
					  						DisplayInSearchWindowList =  false,
					  						PMPropertyPath =  "Tenant",
					  						ListPropertyPath =  "Tenant",
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
					  						FullFieldLable =  "Tenant",
					  						DefaultText =  "Tenant",
					  						IsMaxLength =  false,
					  						IsFixedLength =  false,
					  						EnableAutoFill =  false,
					  						IncludeInSearchField =  false,
					  						AllowedinAutomationConditions =  false,
					  						AutomationEmailRecipient =  false,
					  						CanAutomateSetValue =  false,
					  						DisplayInAutomationAsEnitity =  false,
					  						HasTemplate =  false,
					  						IsCustom =  false,
					  						EnableFullscreenTextBox =  false,
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes,objectTables,addedFields,addedTextCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters,Dictionary<string, QueryGroup> tenantQueryGroups )
	    {    
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable CustomsGeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "CustomsGeneral" && d.Tenant == 0).FirstOrDefault();
		   //List<ObjectField> CustomsGeneralObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "CustomsGeneral").ToList();
		       
	      

	         Screen CustomsGeneralCustomsGeneralHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "CustomsGeneral.HeaderScreen", Name = "CustomsGeneralHeaderScreen", ObjectTableId = CustomsGeneralObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      	
		    CustomsGeneralObjectTable.HeaderScreenId = CustomsGeneralCustomsGeneralHeaderScreenScreen0.Id;
		    CustomsGeneralObjectTable.HeaderScreenCode = CustomsGeneralCustomsGeneralHeaderScreenScreen0.Code;

	   		  

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {      
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable CustomsGeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "CustomsGeneral" && d.Tenant == 0).FirstOrDefault(); 


		   		   //--------------> Additional Features <--------------\\

		   Feature CustomsGeneralFeature_CUSTOMS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CUSTOMS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = true, IsCoreFeature = false, ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Customs", NameTextCodeDefaultText = @"Customs" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,CustomsGeneralObjectTable);

		   Feature CustomsGeneralFeature_CUSTOMSINTERFACESETTINGS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CUSTOMSINTERFACESETTINGS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.CUSTOMSINTERFACESETTINGS", NameTextCodeDefaultText = @"Customs Interface Settings" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,CustomsGeneralObjectTable);

		   Feature CustomsGeneralFeature_CUSTOMSETTING = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CUSTOMSETTING", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.CustomsSetting", NameTextCodeDefaultText = @"Customs Setting" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,CustomsGeneralObjectTable);

		   Feature CustomsGeneralFeature_CUSTOMSCOLLATERAL = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CUSTOMSCOLLATERAL", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.CustomsCollateral", NameTextCodeDefaultText = @"Customs Collateral" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,CustomsGeneralObjectTable);

		   Feature CustomsGeneralFeature_CUSTOMSDECLARATION = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CUSTOMSDECLARATION", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.CustomsDeclaration", NameTextCodeDefaultText = @"Customs Declarations" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,CustomsGeneralObjectTable);

		   Feature CustomsGeneralFeature_CUSTOMSIGN = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CUSTOMSIGN", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.CustomsSign", NameTextCodeDefaultText = @"Sign Stations" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,CustomsGeneralObjectTable);

		   Feature CustomsGeneralFeature_CustomsAirlineMTC = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CustomsAirlineMTC", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.CustomsAirline", NameTextCodeDefaultText = @"Customs Airline" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,CustomsGeneralObjectTable);

		   Feature CustomsGeneralFeature_DocumentsDefinition = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DocumentsDefinition", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "CustomsGeneral.Features.DocumentsDefinition", NameTextCodeDefaultText = @"Documents Definition" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes,CustomsGeneralObjectTable);

   
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable CustomsGeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "CustomsGeneral" && d.Tenant == 0).FirstOrDefault(); 
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
                ObjectTableId = CustomsGeneralObjectTable.Id,
				 
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
                ObjectTableId = CustomsGeneralObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable CustomsGeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "CustomsGeneral" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode CustomsGeneralTextCode_CustomsGeneralBAdd = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.B.Add", DefaultText = "Add",LocalDefaultText = @"הוסף", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralBOK = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.B.OK", DefaultText = "OK",LocalDefaultText = @"אישור", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralBCancel = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.B.Cancel", DefaultText = "Cancel",LocalDefaultText = @"ביטול", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralONewEntity = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.NewEntity", DefaultText = "New %Entity",LocalDefaultText = @"%Entity חדש", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOSendSample = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.SendSample", DefaultText = "Send Sample",LocalDefaultText = @"שלח לדוגמה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCustoms = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.Customs", DefaultText = "Customs",LocalDefaultText = @"מכס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralONewPaymentOrder = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.NewPaymentOrder", DefaultText = "New Payment Order",LocalDefaultText = @"שליפת הוראת תשלום", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOClose = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.Close", DefaultText = "Close",LocalDefaultText = @"סגור", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOLoading = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.Loading", DefaultText = "Loading ....",LocalDefaultText = @"טוען ....", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOSending = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.Sending", DefaultText = "Sending ....",LocalDefaultText = @"שולח ....", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOAddRemoveColumns = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.AddRemoveColumns", DefaultText = "Add/Remove columns",LocalDefaultText = @"הוסף/מחק עמודות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralONoFiltersHaveBeenSet = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.NoFiltersHaveBeenSet", DefaultText = "No filters have been set",LocalDefaultText = @"לא הוגדרו חיתוכים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOExportToExcel = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.ExportToExcel", DefaultText = "Export to excel ",LocalDefaultText = @"Excel הורד לאקסל", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOSaving = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.Saving", DefaultText = "Saving ....",LocalDefaultText = @"שמירה ....", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOEditMetaData = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.EditMetaData", DefaultText = "Edit Meta Data",LocalDefaultText = @"עריכת מטה דאטה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOEditCustomDocument = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.EditCustomDocument", DefaultText = "Edit Custom Document",LocalDefaultText = @"עריכת מסמך מכס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOIsRequired = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.IsRequired", DefaultText = "Is Required",LocalDefaultText = @"הוא נדרש", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOFieldForTableIsRequired = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.FieldForTableIsRequired", DefaultText = "%FieldName in %TableName %EntityReference is Required",LocalDefaultText = @"%FieldName ב- %TableName %EntityReference הוא חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOObjectTables = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.ObjectTables", DefaultText = "Object Tables",LocalDefaultText = @"אוביקטים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralORequiredFields = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.RequiredFields", DefaultText = "Required Fields",LocalDefaultText = @"שדות חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOWrongEntityName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.WrongEntityName", DefaultText = "Entity name is wrong",LocalDefaultText = @"שם הישות שגוי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOAddRemoveRequiredFields = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.AddRemoveRequiredFields", DefaultText = "Add / Remove Required Fields",LocalDefaultText = @"הוספה / הסרה של שדות חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOSelectObjectTable = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.SelectObjectTable", DefaultText = "You must select an ObjectTable",LocalDefaultText = @"עליך לבחור אובייקט", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOViewCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.ViewCode", DefaultText = "View Code",LocalDefaultText = @"צג קוד", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOExisted = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.Existed", DefaultText = "Existed",LocalDefaultText = @"קיים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCustomsExchangeRate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CustomsExchangeRate", DefaultText = "Customs Exchange Rate",LocalDefaultText = @"שערי מטבע מכס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCurrencyRate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CurrencyRate", DefaultText = "Currency Rate",LocalDefaultText = @"טבלת שערים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCurrencyExchangeRateQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CurrencyExchangeRateQuery", DefaultText = "Exchange Rate Query",LocalDefaultText = @"שאילתא לשערים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCustomsExchangeRateFDateFrom = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsExchangeRate.F.DateFrom", DefaultText = "Date from",LocalDefaultText = @"מתאריך", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCustomsExchangeRateFDateTo = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsExchangeRate.F.DateTo", DefaultText = "Date to",LocalDefaultText = @"עד תאריך", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsExchangeRateOToDateMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ExchangeRate.O.ToDateMandatory", DefaultText = "Date to field is mandatory",LocalDefaultText = @"תאריך לשדה הוא חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsExchangeRateOFromDateMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ExchangeRate.O.FromDateMandatory", DefaultText = "Date from field is mandatory",LocalDefaultText = @"תאריך מהשדה הוא חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCustomsBlockListInWarehouse = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CustomsBlockListInWarehouse", DefaultText = "Customs Block List In Warehouse",LocalDefaultText = @"גושים במחסן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOBlockListInWarehouseQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.BlockListInWarehouseQuery", DefaultText = "Block List in Warehouse Query",LocalDefaultText = @"שאילתת גושים במחסן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCustomsMorningMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CustomsMorningMessage", DefaultText = "Customs Morning Message",LocalDefaultText = @"הודעות בוקר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOMorningMessageQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.MorningMessageQuery", DefaultText = "Morning Message Query",LocalDefaultText = @"שאילתא להודעות בוקר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCustomsBlockListInWarehouseOFromDateMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsBlockListInWarehouse.O.FromDateMandatory", DefaultText = "Date to field is mandatory",LocalDefaultText = @"מתאריך פתיחת גוש הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCustomsBlockListInWarehouseOToDateMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsBlockListInWarehouse.O.ToDateMandatory", DefaultText = "Date from field is mandatory",LocalDefaultText = @"עד תאריך פתיחת גוש הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCustomsBlockListInWarehouseOStorageSiteNumberMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsBlockListInWarehouse.O.StorageSiteNumberMandatory", DefaultText = "Date to field is mandatory",LocalDefaultText = @"אתר אחסון הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCustomsBlockListInWarehouseOShowResetBlocksMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsBlockListInWarehouse.O.ShowResetBlocksMandatory", DefaultText = "Date from field is mandatory",LocalDefaultText = @"כולל גושים מאופסים הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOWarning = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.Warning", DefaultText = "Warning",LocalDefaultText = @"התראה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOInvoiceRelatedPoiner = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.InvoiceRelatedPoiner", DefaultText = "There are document tickets related to this supplier invoice",LocalDefaultText = @"קיימות צרופות שמקושרות לחשבון הספק", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOInvoiceItemRelatedPoiner = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.InvoiceItemRelatedPoiner", DefaultText = "There are document tickets related to this supplier invoice item",LocalDefaultText = @"קיימות צרופות שמקושרות לפרט המכס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeOGuarantee = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Guarantee.O.Guarantee", DefaultText = "Guarantee",LocalDefaultText = @"אחריות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeORequiredGuaranteeTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Guarantee.O.RequiredGuaranteeTypes", DefaultText = "Required Guarantee Types",LocalDefaultText = @"הרכב ערבות נדרש", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeOGuaranteeConditions = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Guarantee.O.GuaranteeConditions", DefaultText = "Guarantee Conditions",LocalDefaultText = @"תנאי ערבות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeOGuaranteeReturnRequest = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Guarantee.O.GuaranteeReturnRequest", DefaultText = "Guarantee Return Request",LocalDefaultText = @"בקשה להחזרת ערבות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCargoQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CargoQuery", DefaultText = "Manifest Status Query",LocalDefaultText = @"שאילתא למצהר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCargoQueryHeader = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CargoQueryHeader", DefaultText = "Manifest Status Query",LocalDefaultText = @"שאילתא למצהר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCargoDataMissing = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CargoDataMissing", DefaultText = "Cargo data are missing",LocalDefaultText = @"חסרים נתוני מזהה מטען", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOWarehouseBlockMissing = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.WarehouseBlockMissing", DefaultText = "Cargo data are missing",LocalDefaultText = @"חסר מספר גוש", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOStorageSiteMissing = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.StorageSiteMissing", DefaultText = "Warehouse Block is missing",LocalDefaultText = @"חסר אתר אחסון", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOWarehouseBlockBalance = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.WarehouseBlockBalance", DefaultText = "Warehouse Block Balance",LocalDefaultText = @"יתרות מלאי בגוש", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOWarehouseBlockBalanceQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.WarehouseBlockBalanceQuery", DefaultText = "Warehouse Block Balance",LocalDefaultText = @"שאילתא ליתרות מלאי בגוש", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOTaxationDateTimeCheck = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.TaxationDateTimeCheck", DefaultText = "Taxes date",LocalDefaultText = @"בדיקת תאריך חישוב מיסים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralODocumetsUploaded = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.DocumetsUploaded", DefaultText = "Not all documets were uploaded , continue ?",LocalDefaultText = @"לא כל המסמכים הועלו למכס האם להמשיך ?", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralODocumetsUploadedCheck = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.DocumetsUploadedCheck", DefaultText = "Documents upload check",LocalDefaultText = @"בדיקת מסמכים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralONoPaymentDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.NoPaymentDate", DefaultText = "Declaration was already paid , can’t send",LocalDefaultText = @"הצהרה כבר שולמה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralONoImporterId = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.NoImporterId", DefaultText = "Importer Is Mandatory",LocalDefaultText = @"מספר יבואן הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOConstraintsInProgress = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.ConstraintsInProgress", DefaultText = "Declaration Paid , waiting for constraint approval",LocalDefaultText = @"טיוטה ממתינה לאישור אילוץ הגשה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOFuturePaymentDone = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.FuturePaymentDone", DefaultText = "Future payment was done",LocalDefaultText = @"בוצעה הגשה עתידית", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOTaxationDateTimeNotToday = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.TaxationDateTimeNotToday", DefaultText = "Taxes date is different from today , continue ?",LocalDefaultText = @"תאריך חישוב מיסים שונה מהיום , האם לעדכן לתאריך של היום?", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralONoSupplierInvoiceForDeclaration = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.NoSupplierInvoiceForDeclaration", DefaultText = "Need at least one Supplier Invoice",LocalDefaultText = @"יש להקליד לפחות חשבון ספק אחד", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationOCargoDataMissing = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.CargoDataMissing", DefaultText = "Cargo data are missing",LocalDefaultText = @"חסרים נתוני מזהה מטען", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOGuaranteeReturnQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.GuaranteeReturnQuery", DefaultText = "Guarantee Return Query",LocalDefaultText = @"שאילתא לבקשת החזרת ערבות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOGuaranteeReturnMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.GuaranteeReturnMessage", DefaultText = "Guarantee Return Message",LocalDefaultText = @"בקשה להחזרת ערבות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeReturnOGuaranteeExternalMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeReturn.O.GuaranteeExternalMandatory", DefaultText = "Guarantee External field is mandatory",LocalDefaultText = @"מספר כתב ערבות חיצוני הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeReturnOGuaranteeAmountToReturnMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeReturn.O.GuaranteeAmountToReturnMandatory", DefaultText = "Guarantee Amount To Return field is mandatory",LocalDefaultText = @"סכום להחזרה הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeReturnOGuaranteeCertificateType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeReturn.O.GuaranteeCertificateType", DefaultText = "Guarantee Certificate Type field is mandatory",LocalDefaultText = @"סוג כתב הערבות הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOBankAccountToRefundMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.BankAccountToRefundMessage", DefaultText = "Bank Account To Refund",LocalDefaultText = @"בקשה להחזר פיקדון", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOBankAccountToRefundQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.BankAccountToRefundQuery", DefaultText = "Bank Account To Refund Query",LocalDefaultText = @"שאילתא לבקשת החזר פיקדון", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralBAddDocumentVersion = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.B.AddDocumentVersion", DefaultText = "Add Version",LocalDefaultText = @"גרסה חדשה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOConstraintsInProgress2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.ConstraintsInProgress2", DefaultText = "There are constraints in progress",LocalDefaultText = @"קיימים אילוצים בתהליך", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOImporterCodeNoId = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.ImporterCodeNoId", DefaultText = "Need to retrieve client before sending",LocalDefaultText = @"יש לשלוף לקוח מהמכס לפני שליחה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralONoConsignmentPackages = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.NoConsignmentPackages", DefaultText = "Must enter at least one Package record",LocalDefaultText = @"יש להזין פרטי אריזה למשגור", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralONoSupplierInvoiceItemForInvoice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.NoSupplierInvoiceItemForInvoice", DefaultText = "Must enter at least one item for invoice",LocalDefaultText = @"יש להזין לכל חשבון לפחות שורת פרט מכס אחת", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralODeclarationRestoreQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.DeclarationRestoreQuery", DefaultText = "Declaration restore Query",LocalDefaultText = @"שאילתא לשחזור נתוני הצהרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralODeclarationRestoreHeader = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.DeclarationRestoreHeader", DefaultText = "Declaration restore Query",LocalDefaultText = @"שחזור נתוני הצהרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralODeclarationRestoreDataMissing = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.DeclarationRestoreDataMissing", DefaultText = "Declaration number is missing",LocalDefaultText = @"חסר מספר הצהרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOFaultQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.FaultQuery", DefaultText = "Faults Query",LocalDefaultText = @"שאילתא לליקויים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOFaultQueryCustomFileDeclaration = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.FaultQueryCustomFileDeclaration", DefaultText = "Custom File / Declaration",LocalDefaultText = @"תיק עמילות / הצהרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOFaultQueryClient = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.FaultQueryClient", DefaultText = "Client / Importer",LocalDefaultText = @"מס' לקוח / מס' יבואן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOFaultQueryDates = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.FaultQueryDates", DefaultText = "From date / To date",LocalDefaultText = @"מתאריך / עד תאריך", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOFaultQueryFaultCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.FaultQueryFaultCode", DefaultText = "Faults Code",LocalDefaultText = @"קוד ליקוי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOFaultQueryAgent = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.FaultQueryAgent", DefaultText = "Agent Code",LocalDefaultText = @"קוד סוכן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsBankAccountToRefundQueryOFileNumberMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.BankAccountToRefundQuery.O.FileNumberMandatory", DefaultText = "File Number field is mandatory",LocalDefaultText = @"מספר תיק תפג הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsBankAccountToRefundQueryOFileTypeMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.BankAccountToRefundQuery.O.FileTypeMandatory", DefaultText = "File Type field is mandatory",LocalDefaultText = @"סוג תיק הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsBankAccountToRefundQueryOIdentifierMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.BankAccountToRefundQuery.O.IdentifierMandatory", DefaultText = "Identifier Type field is mandatory",LocalDefaultText = @"סוג מוטב להחזר הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsBankAccountToRefundQueryOIdentifierCodeMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.BankAccountToRefundQuery.O.IdentifierCodeMandatory", DefaultText = "Identifier Code field is mandatory",LocalDefaultText = @"מספר מוטב להחזר הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsBankAccountToRefundQueryOCountryCodeMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.BankAccountToRefundQuery.O.CountryCodeMandatory", DefaultText = "Country Code field is mandatory",LocalDefaultText = @"ארץ חשבון בנק הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsBankAccountToRefundQueryOBankCodeMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.BankAccountToRefundQuery.O.BankCodeMandatory", DefaultText = "Bank Code field is mandatory",LocalDefaultText = @"קוד בנק הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsBankAccountToRefundQueryOBankBranchMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.BankAccountToRefundQuery.O.BankBranchMandatory", DefaultText = "Bank Branch field is mandatory",LocalDefaultText = @"מספר סניף הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsBankAccountToRefundQueryOAccountNumberMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.BankAccountToRefundQuery.O.AccountNumberMandatory", DefaultText = "Account Number field is mandatory",LocalDefaultText = @"מספר חשבון הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralONoDataFound = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.NoDataFound", DefaultText = "No Data Found",LocalDefaultText = @"לא נמצאו תוצאות מתאימות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationNoDeclarationNoForCustomfile = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.NoDeclarationNoForCustomfile", DefaultText = "There is no declaration number for custom file",LocalDefaultText = @"לתיק זה אין עדיין מספר הצהרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationOStartDateIsMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.StartDateIsMandatory", DefaultText = "From date is mandatory",LocalDefaultText = @"תאריך התחלה הוא חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationOEndDateIsMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.EndDateIsMandatory", DefaultText = "Dates is mandatory",LocalDefaultText = @"תאריך סיום הוא חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationOAgentExternalIDIsMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.AgentExternalIDIsMandatory", DefaultText = "Agent Code is mandatory",LocalDefaultText = @"קוד סוכן הוא חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeReturnOReasonMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeReturn.O.ReasonMandatory", DefaultText = "Reason field is mandatory",LocalDefaultText = @"נימוק סוכן הוא חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeReturnOFileNumberMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeReturn.O.FileNumberMandatory", DefaultText = "File Number field is mandatory",LocalDefaultText = @"מספר תיק תפג הוא חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOImporterDeclarationQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.ImporterDeclarationQuery", DefaultText = "Importer Declaration Query",LocalDefaultText = @"שאילתא לתצהיר יבואן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOImporterDeclarationMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.ImporterDeclarationMessage", DefaultText = "Importer Declaration Message",LocalDefaultText = @"תצהיר יבואן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsImporterDeclarationQueryOImporterNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ImporterDeclarationQuery.O.ImporterNumber", DefaultText = "Importer Number",LocalDefaultText = @"מספר יבואן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsImporterDeclarationQueryONumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ImporterDeclarationQuery.O.Number", DefaultText = "Number",LocalDefaultText = @"מספר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsImporterDeclarationQueryOSearchByDeclarationExpire = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ImporterDeclarationQuery.O.SearchByDeclarationExpire", DefaultText = "Search By Declaration ",LocalDefaultText = @"תצהירי יבואן לפי תוקף", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsImporterDeclarationQueryOSearchByDeclarationsupplier = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ImporterDeclarationQuery.O.SearchByDeclarationsupplier", DefaultText = "Search By Declaration Supplier",LocalDefaultText = @"תצהירים לפי סוג", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsImporterDeclarationQueryODeclarationExpire = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ImporterDeclarationQuery.O.DeclarationExpire", DefaultText = "Declaration Expire",LocalDefaultText = @"תצהירים בתוקף עד", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsImporterDeclarationQueryODeclarationConect = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ImporterDeclarationQuery.O.DeclarationConect", DefaultText = "Declaration Conect",LocalDefaultText = @"תצהירים שקשורים ל", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsImporterDeclarationQueryOImporterNumberMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ImporterDeclarationQuery.O.ImporterNumberMandatory", DefaultText = "Importer Number field is mandatory",LocalDefaultText = @"מספר יבואן הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsImporterDeclarationQueryODeclarationExpireMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ImporterDeclarationQuery.O.DeclarationExpireMandatory", DefaultText = "Declaration Expire field is mandatory",LocalDefaultText = @"תצהירים בתוקף עד הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsImporterDeclarationQueryODeclarationConectMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ImporterDeclarationQuery.O.DeclarationConectMandatory", DefaultText = "Declaration Conect field is mandatory",LocalDefaultText = @"תצהירים שקשורים ל הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsImporterDeclarationQueryODeclarationNumberMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ImporterDeclarationQuery.O.DeclarationNumberMandatory", DefaultText = "Declaration Number field is mandatory",LocalDefaultText = @"מספר הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsImporterDeclarationQueryOFromDateMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ImporterDeclarationQuery.O.FromDateMandatory", DefaultText = "From Date field is mandatory",LocalDefaultText = @"מתאריך הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsImporterDeclarationQueryOToDateMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ImporterDeclarationQuery.O.ToDateMandatory", DefaultText = "To Date field is mandatory",LocalDefaultText = @"עד תאריך הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsMAlreadySendReSend = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.M.AlreadySendReSend", DefaultText = "There is a request in progress , To continue ?",LocalDefaultText = @"יש בקשה זהה בתהליך, האם להמשיך ?", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDepositODisplayOnly = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Deposit.O.DisplayOnly", DefaultText = "Values are for Display Only",LocalDefaultText = @"הנתונים הינם לתצוגה בלבד", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralBClose = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.B.Close", DefaultText = "Close",LocalDefaultText = @"סגור", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralORecallSuppliersFromFile = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.RecallSuppliersFromFile", DefaultText = "Recall Suppliers From File",LocalDefaultText = @"איחזור ספקים מקובץ ", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralORecallsuppliersButtonLabel = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.RecallsuppliersButtonLabel", DefaultText = "Suppliers sequence retrieval",LocalDefaultText = @"שליפת ספקים ברצף", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralODeclarationPrintQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.DeclarationPrintQuery", DefaultText = "Declaration Print Query",LocalDefaultText = @"שאילתא להדפסת הצהרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralODeclarationPrintMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.DeclarationPrintMessage", DefaultText = "Declaration Print Message",LocalDefaultText = @"הדפסת הצהרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationPrintQueryOSearchByDeclaration = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationPrintQuery.O.SearchByDeclaration", DefaultText = "Search By Declaration",LocalDefaultText = @"חפש לפי הצהרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationPrintQueryOSearchByCargo = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationPrintQuery.O.SearchByCargo", DefaultText = "Search By Cargo",LocalDefaultText = @"חפש לפי מטענים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationPrintQueryODeclarationNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationPrintQuery.O.DeclarationNumber", DefaultText = "Declaration Number",LocalDefaultText = @"מס' תיק/הצהרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationPrintQueryODeclarationNumberMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationPrintQuery.O.DeclarationNumberMandatory", DefaultText = "Custom File/Declaration Number fields are mandatory",LocalDefaultText = @"מס' תיק/הצהרה הם שדות חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsBlockListInWarehouseOBlockListInWarehouseResults = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.BlockListInWarehouse.O.BlockListInWarehouseResults", DefaultText = "Block List in Warehouse Results",LocalDefaultText = @"תוצאות שאילתת גושים במחסן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsBlockListInWarehouseODeclarationNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.BlockListInWarehouse.O.DeclarationNumber", DefaultText = "Declaration Number",LocalDefaultText = @"מספר הצהרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsBlockListInWarehouseOWarehouseBlockNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.BlockListInWarehouse.O.WarehouseBlockNumber", DefaultText = "Warehouse Block Number",LocalDefaultText = @"מספר גוש", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsBlockListInWarehouseOImporterNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.BlockListInWarehouse.O.ImporterNumber", DefaultText = "Importer Number",LocalDefaultText = @"מספר יבואן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsBlockListInWarehouseOImporterTitle = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.BlockListInWarehouse.O.ImporterTitle", DefaultText = "Importer Title",LocalDefaultText = @"שם יבואן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsBlockListInWarehouseOOpeningDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.BlockListInWarehouse.O.OpeningDate", DefaultText = "Opening Date",LocalDefaultText = @"תאריך פתיחה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsBlockListInWarehouseOOriginalOpeningDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.BlockListInWarehouse.O.OriginalOpeningDate", DefaultText = "Original Opening Date",LocalDefaultText = @"תאריך אחסנה מקורי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsBlockListInWarehouseOLogicalPackagesQuantityBalance = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.BlockListInWarehouse.O.LogicalPackagesQuantityBalance", DefaultText = "Logical Balance",LocalDefaultText = @"יתרת אריזות לוגית", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsBlockListInWarehouseOPhysicalPackagesQuantityBalance = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.BlockListInWarehouse.O.PhysicalPackagesQuantityBalance", DefaultText = "Physical Balance",LocalDefaultText = @"יתרת אריזות פיזית", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsBlockListInWarehouseOValue = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.BlockListInWarehouse.O.Value", DefaultText = "Value",LocalDefaultText = @"יתרה בשח", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsBlockListInWarehouseOSpecialActivityTypeName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.BlockListInWarehouse.O.SpecialActivityTypeName", DefaultText = "Special Activity Type",LocalDefaultText = @"פעולה מיוחדת", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsMorningMessageOMorningMessageResults = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.MorningMessage.O.MorningMessageResults", DefaultText = "Morning Message Results",LocalDefaultText = @"תוצאות שאילתא להודעות בוקר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsMorningMessageOMessageID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.MorningMessage.O.MessageID", DefaultText = "Message ID",LocalDefaultText = @"מזהה הודעה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsMorningMessageOCategory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.MorningMessage.O.Category", DefaultText = "Category",LocalDefaultText = @"קטגוריה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsMorningMessageOSubject = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.MorningMessage.O.Subject", DefaultText = "Subject",LocalDefaultText = @"נושא", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsMorningMessageOContent = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.MorningMessage.O.Content", DefaultText = "Content",LocalDefaultText = @"תוכן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsMorningMessageOMessageDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.MorningMessage.O.MessageDate", DefaultText = "Message Date",LocalDefaultText = @"תאריך", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOImporterDeclarationResults = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.ImporterDeclarationResults", DefaultText = "Importer Declaration Results",LocalDefaultText = @"תוצאות שאילתא תצהיר יבואן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsImporterDeclarationQueryOPeriodDeclaration = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ImporterDeclarationQuery.O.PeriodDeclaration", DefaultText = "Period Declaration List",LocalDefaultText = @"רשימת תצהירים תקופתיים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsImporterDeclarationQueryOLoiDeclaration = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ImporterDeclarationQuery.O.LoiDeclaration", DefaultText = "Loi Declaration List",LocalDefaultText = @"רשימת תצהירים להצהרת יבוא", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsImporterDeclarationQueryOSecurityDeclaration = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ImporterDeclarationQuery.O.SecurityDeclaration", DefaultText = "Security Declaration List",LocalDefaultText = @"רשימת תצהירים בטחוניים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsImporterDeclarationQueryOPeriodDeclarationID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ImporterDeclarationQuery.O.PeriodDeclarationID", DefaultText = "Period Declaration Id",LocalDefaultText = @"מספר תצהיר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsImporterDeclarationQueryOVendorName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ImporterDeclarationQuery.O.VendorName", DefaultText = "Vendor Name",LocalDefaultText = @"ספק", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsImporterDeclarationQueryOCreateDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ImporterDeclarationQuery.O.CreateDate", DefaultText = "Create Date",LocalDefaultText = @"תאריך יצירה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsImporterDeclarationQueryOValidityFrom = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ImporterDeclarationQuery.O.ValidityFrom", DefaultText = "validity From",LocalDefaultText = @"תקף מתאריך", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsImporterDeclarationQueryOExpirationDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ImporterDeclarationQuery.O.ExpirationDate", DefaultText = "Expiration Date",LocalDefaultText = @"תאריך תוקף", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsImporterDeclarationQueryOStatusName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ImporterDeclarationQuery.O.StatusName", DefaultText = "Status",LocalDefaultText = @"סטטוס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsImporterDeclarationQueryODocumentID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ImporterDeclarationQuery.O.DocumentID", DefaultText = "Document ID",LocalDefaultText = @"מזהה מסמך", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsImporterDeclarationQueryODeclarationID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ImporterDeclarationQuery.O.DeclarationID", DefaultText = "Declaration Id",LocalDefaultText = @"מספר הצהרת יבוא", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsImporterDeclarationQueryOSecurityDeclarationName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ImporterDeclarationQuery.O.SecurityDeclarationName", DefaultText = "Security Declaration Type",LocalDefaultText = @"סוג תצהיר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOWarehouseBlockBalanceResults = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.WarehouseBlockBalanceResults", DefaultText = "Warehouse Block Balance Results",LocalDefaultText = @"תוצאות שאילתא ליתרות מלאי בגוש", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsWarehouseBlockBalanceOActionDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.WarehouseBlockBalance.O.ActionDate", DefaultText = "Warehouse Block Balance Results",LocalDefaultText = @"תאריך תנועה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsWarehouseBlockBalanceOActionPackagesQuantity = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.WarehouseBlockBalance.O.ActionPackagesQuantity", DefaultText = "Packages Quantity",LocalDefaultText = @"סך אריזות לפני תנועה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsWarehouseBlockBalanceOPackagesQuantityAfterAction = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.WarehouseBlockBalance.O.PackagesQuantityAfterAction", DefaultText = "Packages Quantity After Action",LocalDefaultText = @"סך אריזות לאחר תנועה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsWarehouseBlockBalanceOActionValue = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.WarehouseBlockBalance.O.ActionValue", DefaultText = "Action Value",LocalDefaultText = @"ערך לפני תנועה בש''ח", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsWarehouseBlockBalanceOValueAfterAction = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.WarehouseBlockBalance.O.ValueAfterAction", DefaultText = "Value After Action",LocalDefaultText = @"ערך בש''ח לאחר תנועה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsWarehouseBlockBalanceOGovernmentProcedureType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.WarehouseBlockBalance.O.GovernmentProcedureType", DefaultText = "Government Procedure Type",LocalDefaultText = @"תהליך מכסי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsWarehouseBlockBalanceODeclarationNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.WarehouseBlockBalance.O.DeclarationNumber", DefaultText = "Declaration Number",LocalDefaultText = @"מספר הצהרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsWarehouseBlockBalanceOStorageActionDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.WarehouseBlockBalance.O.StorageActionDate", DefaultText = "Storage Action Date",LocalDefaultText = @"תאריך תנועה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsWarehouseBlockBalanceOStorageActionType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.WarehouseBlockBalance.O.StorageActionType", DefaultText = "Storage Action Type",LocalDefaultText = @"סוג תנועה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsWarehouseBlockBalanceOStorageActionPackagesQuantity = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.WarehouseBlockBalance.O.StorageActionPackagesQuantity", DefaultText = "Storage Action Packages Quantity",LocalDefaultText = @"סך אריזות לפני תנועה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsWarehouseBlockBalanceOPackagesQuantityAfterStorageAction = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.WarehouseBlockBalance.O.PackagesQuantityAfterStorageAction", DefaultText = "Packages Quantity After Storage Action",LocalDefaultText = @"אריזות לאחר תנועה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsWarehouseBlockBalanceOStorageReferenceType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.WarehouseBlockBalance.O.StorageReferenceType", DefaultText = "Storage Reference Type",LocalDefaultText = @"סוג אסמכתא", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCustomsMasterBOLQueryMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CustomsMasterBOLQueryMessage", DefaultText = "Customs Master BOL Query Message",LocalDefaultText = @"שטרי מטען", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOMasterBOLQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.MasterBOLQuery", DefaultText = "Master BOL Query",LocalDefaultText = @"שאילתא לשטרי מטען", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsMasterBOLQueryOMasterBOLQueryResults = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.MasterBOLQuery.O.MasterBOLQueryResults", DefaultText = "Morning Message Results",LocalDefaultText = @"תוצאות שאילתא לשטרי מטען", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsMasterBOLQueryODate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.MasterBOLQuery.O.Date", DefaultText = "Date (Year)",LocalDefaultText = @"שנת טיסה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsMasterBOLQueryOMaster = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.MasterBOLQuery.O.Master", DefaultText = "Master Number",LocalDefaultText = @"מאסטר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsMasterBOLQueryOCargoIdentifierKey3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.MasterBOLQuery.O.CargoIdentifierKey3", DefaultText = "CargoIdentifierKey3",LocalDefaultText = @"שט'מ פנימי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsMasterBOLQueryOPacakgesQuantity = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.MasterBOLQuery.O.PacakgesQuantity", DefaultText = "Pacakges Quantity",LocalDefaultText = @"כמות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsMasterBOLQueryOTotalWeight = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.MasterBOLQuery.O.TotalWeight", DefaultText = "Total Weight",LocalDefaultText = @"משקל", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsMasterBOLQueryOYearDateIsMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.MasterBOLQuery.O.YearDateIsMandatory", DefaultText = "Date field is mandatory",LocalDefaultText = @"שנת טיסה הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsMasterBOLQueryOMasterBillOfLadingIsMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.MasterBOLQuery.O.MasterBillOfLadingIsMandatory", DefaultText = "Master field is mandatory",LocalDefaultText = @"מאסטר הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOSubmitDeclarationAgain = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.SubmitDeclarationAgain", DefaultText = "Need to Submit Declaration Again",LocalDefaultText = @"סטטוס הצהרה מחייב שליחה מחדש למכס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralODeficitFileFilterQueryMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.DeficitFileFilterQueryMessage", DefaultText = "Deficit File Filter Query Message",LocalDefaultText = @"גרעונות נתוני קלט", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralODeficitFileFilterQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.DeficitFileFilterQuery", DefaultText = "Deficit File Filter Query",LocalDefaultText = @"שאילתא לגרעונות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeficitFileFilterQueryODeficitFileFilterQueryResults = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeficitFileFilterQuery.O.DeficitFileFilterQueryResults", DefaultText = "Deficit File Filter Results",LocalDefaultText = @"תוצאות שאילתא לגרעונות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeficitFileFilterQueryODeficitFileFilterGeneralResults = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeficitFileFilterQuery.O.DeficitFileFilterGeneralResults", DefaultText = "General Details",LocalDefaultText = @"נתונים כלליים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeficitFileFilterQueryODeficitFileFilterOpenFileResults = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeficitFileFilterQuery.O.DeficitFileFilterOpenFileResults", DefaultText = "Open Files",LocalDefaultText = @"תיקים פתוחים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeficitFileFilterQueryODeficitFileFilterCloseFileResults = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeficitFileFilterQuery.O.DeficitFileFilterCloseFileResults", DefaultText = "Close Files",LocalDefaultText = @"תיקים סגורים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeficitFileFilterQueryODeficitFileFilterPaymentResults = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeficitFileFilterQuery.O.DeficitFileFilterPaymentResults", DefaultText = "Payments Details",LocalDefaultText = @"הוראות תשלום", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeficitFileFilterQueryODeficitFileFilterDocumentsResults = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeficitFileFilterQuery.O.DeficitFileFilterDocumentsResults", DefaultText = "Documents Details",LocalDefaultText = @"מסמכים נדרשים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeficitFileFilterQueryFFileNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeficitFileFilterQuery.F.FileNumber", DefaultText = "FileNumber",LocalDefaultText = @"מספר תיק תפג", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeficitFileFilterQueryFNumeral = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeficitFileFilterQuery.F.Numeral", DefaultText = "Numeral",LocalDefaultText = @"מספר רץ", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeficitFileFilterQueryFStatusName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeficitFileFilterQuery.F.StatusName", DefaultText = "Status",LocalDefaultText = @"סטטוס התיק", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeficitFileFilterQueryFCustomOfficeName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeficitFileFilterQuery.F.CustomOfficeName", DefaultText = "CustomOffice Name",LocalDefaultText = @"בית המכס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeficitFileFilterQueryFExternalName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeficitFileFilterQuery.F.ExternalName", DefaultText = "External Name",LocalDefaultText = @"יבואן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeficitFileFilterQueryFAgentName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeficitFileFilterQuery.F.AgentName", DefaultText = "Agent Name",LocalDefaultText = @"הסוכן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeficitFileFilterQueryOFileNumberMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeficitFileFilterQuery.O.FileNumberMandatory", DefaultText = "File Number field is mandatory",LocalDefaultText = @"מספר תיק תפג הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeficitFileFilterQueryONumeralMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeficitFileFilterQuery.O.NumeralMandatory", DefaultText = "Numeral field is mandatory",LocalDefaultText = @"מספר רץ הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOMasavPaymentsToAgentQueryMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.MasavPaymentsToAgentQueryMessage", DefaultText = "Masav Payments To Agent Query Message",LocalDefaultText = @"בקשת דוח קופה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOMasavPaymentsToAgentQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.MasavPaymentsToAgentQuery", DefaultText = "Masav Payments To Agent Query",LocalDefaultText = @"שאילתא לבקשת דוח קופה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOMasavPaymentsToAgentQueryResults = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.MasavPaymentsToAgentQueryResults", DefaultText = "Deficit File Filter Results",LocalDefaultText = @"תוצאות שאילתא לבקשת דוח קופה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsMasavPaymentsToAgentQueryOPaymentDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.MasavPaymentsToAgentQuery.O.PaymentDate", DefaultText = "Payment Date",LocalDefaultText = @"תאריך תשלום", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsMasavPaymentsToAgentQueryOPaymentDateMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.MasavPaymentsToAgentQuery.O.PaymentDateMandatory", DefaultText = "File Number field is mandatory",LocalDefaultText = @"תאריך תשלום הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCreditQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CreditQuery", DefaultText = "Credit Query",LocalDefaultText = @"שאילתא לתקרת אשראי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCreditQueryHeader = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CreditQueryHeader", DefaultText = "Credit Query",LocalDefaultText = @"שאילתא לתקרת אשראי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCreditQueryDataMissing = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CreditQueryDataMissing", DefaultText = "Importer or Agent VAT is mandatory",LocalDefaultText = @"חובה להזין מספר יבואן או סוכן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCreditQueryResults = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CreditQueryResults", DefaultText = "Results",LocalDefaultText = @"פירוט יתרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCreditQueryOImporterVat = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CreditQuery.O.ImporterVat", DefaultText = "Importer VAT",LocalDefaultText = @"ח.פ יבואן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCreditQueryOAgentVat = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CreditQuery.O.AgentVat", DefaultText = "Agent VAT",LocalDefaultText = @"ח.פ סוכן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCreditQueryResultsODailyCieling = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CreditQueryResults.O.DailyCieling", DefaultText = "Daily Cieling",LocalDefaultText = @"תקרה יומית", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCreditQueryResultsOUsedBalance = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CreditQueryResults.O.UsedBalance", DefaultText = "Used Balance",LocalDefaultText = @"ניצול יומי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCreditQueryResultsOFreeBalance = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CreditQueryResults.O.FreeBalance", DefaultText = "Free Balance",LocalDefaultText = @"יתרה יומית פנויה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeficitFileFilterQueryFDisplayFileNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeficitFileFilterQuery.F.DisplayFileNumber", DefaultText = "Display File Number",LocalDefaultText = @"מספר תיק", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeficitFileFilterQueryFEntityTypeName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeficitFileFilterQuery.F.EntityTypeName", DefaultText = "Entity Type",LocalDefaultText = @"ישות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeficitFileFilterQueryFDeficitEntityID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeficitFileFilterQuery.F.DeficitEntityID", DefaultText = "Deficit Entity",LocalDefaultText = @"מספר ישות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeficitFileFilterQueryFProductionDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeficitFileFilterQuery.F.ProductionDate", DefaultText = "Production Date",LocalDefaultText = @"ת. הודעת החיוב", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeficitFileFilterQueryFUnpaidBalance = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeficitFileFilterQuery.F.UnpaidBalance", DefaultText = "Unpaid Balance",LocalDefaultText = @"קרן החוב", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeficitFileFilterQueryFEstimatedBalance = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeficitFileFilterQuery.F.EstimatedBalance", DefaultText = "Estimated Balance",LocalDefaultText = @"חוב משוערך", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeficitFileFilterQueryFSecondaryStatus = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeficitFileFilterQuery.F.SecondaryStatus", DefaultText = "Secondary Status",LocalDefaultText = @"סיבת סגירה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeficitFileFilterQueryFCloseDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeficitFileFilterQuery.F.CloseDate", DefaultText = "Close Date",LocalDefaultText = @"תאריך סגירה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeficitFileFilterQueryFPaymentId = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeficitFileFilterQuery.F.PaymentId", DefaultText = "Payment Id",LocalDefaultText = @"מספר הוראה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeficitFileFilterQueryFPaymentProcess = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeficitFileFilterQuery.F.PaymentProcess", DefaultText = "Payment Process",LocalDefaultText = @"תהליך הוראה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeficitFileFilterQueryFAmountSum = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeficitFileFilterQuery.F.AmountSum", DefaultText = "Amount Sum",LocalDefaultText = @"סכום הוראה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeficitFileFilterQueryFCreateDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeficitFileFilterQuery.F.CreateDate", DefaultText = "Create Date",LocalDefaultText = @"תאריך יצירה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeficitFileFilterQueryFPaymentOrderPayDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeficitFileFilterQuery.F.PaymentOrderPayDate", DefaultText = "Payment Pay Date",LocalDefaultText = @"תאריך תשלום", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeficitFileFilterQueryFValidityDateTo = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeficitFileFilterQuery.F.ValidityDateTo", DefaultText = "Validity Date",LocalDefaultText = @"תאריך תוקף", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeficitFileFilterQueryFDocumentTypeName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeficitFileFilterQuery.F.DocumentTypeName", DefaultText = "Document Type",LocalDefaultText = @"סוג מסמך", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeficitFileFilterQueryFDocumentID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeficitFileFilterQuery.F.DocumentID", DefaultText = "Document ID",LocalDefaultText = @"סימוכין מכס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsMasavPaymentsToAgentQueryOPaymentProcess = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.MasavPaymentsToAgentQuery.O.PaymentProcess", DefaultText = "Payment Process",LocalDefaultText = @"תהליך יוצר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsMasavPaymentsToAgentQueryOPaymentType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.MasavPaymentsToAgentQuery.O.PaymentType", DefaultText = "Payment Type",LocalDefaultText = @"סוג הוראה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsMasavPaymentsToAgentQueryOPaymentID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.MasavPaymentsToAgentQuery.O.PaymentID", DefaultText = "Payment ID",LocalDefaultText = @"הוראת תשלום", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsMasavPaymentsToAgentQueryOEntityIdExternalReferenceID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.MasavPaymentsToAgentQuery.O.EntityIdExternalReferenceID", DefaultText = "Entity External Reference ID",LocalDefaultText = @"תיק סוכן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsMasavPaymentsToAgentQueryOPaymentMethodAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.MasavPaymentsToAgentQuery.O.PaymentMethodAmount", DefaultText = "Payment Method Amount",LocalDefaultText = @"סכום ששולם במסב", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsMasavPaymentsToAgentQueryOBank = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.MasavPaymentsToAgentQuery.O.Bank", DefaultText = "Bank",LocalDefaultText = @"בנק", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsMasavPaymentsToAgentQueryOBranch = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.MasavPaymentsToAgentQuery.O.Branch", DefaultText = "Branch",LocalDefaultText = @"סניף", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsMasavPaymentsToAgentQueryOAccountNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.MasavPaymentsToAgentQuery.O.AccountNumber", DefaultText = "Account Number",LocalDefaultText = @"חשבון", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsMasavPaymentsToAgentQueryOExternalID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.MasavPaymentsToAgentQuery.O.ExternalID", DefaultText = "External ID",LocalDefaultText = @"יבואן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsMasavPaymentsToAgentQueryOAgentAccountPosession = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.MasavPaymentsToAgentQuery.O.AgentAccountPosession", DefaultText = "Agent Account Posession",LocalDefaultText = @"מסב סוכן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCreditQueryOnlyOneVat = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CreditQueryOnlyOneVat", DefaultText = "Can not send Importer and Agent VAT. Need to send only one VAT number",LocalDefaultText = @"לא ניתן לשלוח את מספר היבואן וגם את מספר הסוכן. ניתן לשלוח רק את אחד מהם", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOSpecialActivityRequestHeader = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.SpecialActivityRequestHeader", DefaultText = "Special Activity Request",LocalDefaultText = @"בקשה לפעולות מיוחדות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestOGeneral = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.O.General", DefaultText = "General Section",LocalDefaultText = @"כללי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestOGoodsDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.O.GoodsDetails", DefaultText = "Goods Details",LocalDefaultText = @"בקשה לפירוט טובין", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestORepresentitive = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.O.Representitive", DefaultText = "Representitive",LocalDefaultText = @"נציגים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestOSample = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.O.Sample", DefaultText = "Sample Request",LocalDefaultText = @"בקשה להוצאת דוגמא", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestORepacking = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.O.Repacking", DefaultText = "Repacking",LocalDefaultText = @"בקשה לאריזה מחדש", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestOCurrentPacking = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.O.CurrentPacking", DefaultText = "Current Packing",LocalDefaultText = @"מצב קיים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestODesiredPacking = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.O.DesiredPacking", DefaultText = "Desired Packing",LocalDefaultText = @"מצב רצוי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestOSpecialActivityRequestNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.O.SpecialActivityRequestNumber", DefaultText = "Special Activity Request Number",LocalDefaultText = @"מספר פעולה באתר אחסון", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestOActivityRequestStartDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.O.ActivityRequestStartDate", DefaultText = "Activity Request Start Date",LocalDefaultText = @"תאריך תחילת הפעולה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestOActivityRequestEndDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.O.ActivityRequestEndDate", DefaultText = "Activity Request End Date",LocalDefaultText = @"תאריך סיום הפעולה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestOCustomFileNo = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.O.CustomFileNo", DefaultText = "Custom File No",LocalDefaultText = @"מספר תיק עמילות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestOSiteNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.O.SiteNumber", DefaultText = "Site Number",LocalDefaultText = @"מספר אתר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestOWarehouseBlockNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.O.WarehouseBlockNumber", DefaultText = "Warehouse Block Number",LocalDefaultText = @"מספר גוש", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestOCargoIdentifierType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.O.CargoIdentifierType", DefaultText = "Cargo Identifier Type",LocalDefaultText = @"סוג מזהה מטען", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestOCargoIdentifierKey1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.O.CargoIdentifierKey1", DefaultText = "Cargo Identifier Key1",LocalDefaultText = @"מזהה מטען ראשון", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestOCargoIdentifierKey2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.O.CargoIdentifierKey2", DefaultText = "Cargo Identifier Key2",LocalDefaultText = @"מזהה מטען שני", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestOCargoIdentifierKey3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.O.CargoIdentifierKey3", DefaultText = "Cargo Identifier Key3",LocalDefaultText = @"מזהה מטען שלישי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestOCargoRowNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.O.CargoRowNumber", DefaultText = "Cargo Row Number",LocalDefaultText = @"מספר סידורי במטען", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestOAuthorityCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.O.AuthorityCode", DefaultText = "Authority Code",LocalDefaultText = @"רשות מוסמכת עבור דוגמא", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestOImporterNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.O.ImporterNumber", DefaultText = "Importer Number",LocalDefaultText = @"מספר יבואן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestOSpecialActivityTypeEssence = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.O.SpecialActivityTypeEssence", DefaultText = "Essence",LocalDefaultText = @"מהות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestOIdemanderType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.O.IdemanderType", DefaultText = "Idemander Type",LocalDefaultText = @"סוג גורם מבקש פירוט", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestOSpecialActionsCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.O.SpecialActionsCode", DefaultText = "Special Actions Code",LocalDefaultText = @"קוד פעולות מיוחדות לביצוע", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestOOtherDescription = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.O.OtherDescription", DefaultText = "Other Description",LocalDefaultText = @"הסבר נוסף", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestOGoodsDescription = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.O.GoodsDescription", DefaultText = "Goods Description",LocalDefaultText = @"תיאור טובין", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestOApprovalDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.O.ApprovalDate", DefaultText = "Approval Date",LocalDefaultText = @"תאריך אישור", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestOApprovalName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.O.ApprovalName", DefaultText = "Approval Name",LocalDefaultText = @"שם מאשר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFSampleRowNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.SampleRowNumber", DefaultText = "Row",LocalDefaultText = @"שורה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFSampleReturnDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.SampleReturnDate", DefaultText = "SampleReturnDate",LocalDefaultText = @"מועד החזרת דוגמא", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFCustomsItem = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.CustomsItem", DefaultText = "CustomsItem",LocalDefaultText = @"פרט מכס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFCustomsItemQuantity = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.CustomsItemQuantity", DefaultText = "CustomsItemQuantity",LocalDefaultText = @"כמות להוצאה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFSampleValue = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.SampleValue", DefaultText = "SampleValue",LocalDefaultText = @"ערך הדוגמא", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFSampleDescription = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.SampleDescription", DefaultText = "SampleDescription",LocalDefaultText = @"תיאור דוגמא", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFPackageType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.PackageType", DefaultText = "PackageType",LocalDefaultText = @"זהוי אריזה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFQuantity = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.Quantity", DefaultText = "Quantity",LocalDefaultText = @"כמות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFWeight = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.Weight", DefaultText = "Weight",LocalDefaultText = @"משקל", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsMasavPaymentsToAgentQueryOEntityType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.MasavPaymentsToAgentQuery.O.EntityType", DefaultText = "Entity Type",LocalDefaultText = @"סוג ישות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsMasavPaymentsToAgentQueryOEntityIdKey1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.MasavPaymentsToAgentQuery.O.EntityIdKey1", DefaultText = "EntityIdKey1",LocalDefaultText = @"מזהה חיצוני 1", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsMasavPaymentsToAgentQueryOEntityIdKey2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.MasavPaymentsToAgentQuery.O.EntityIdKey2", DefaultText = "EntityIdKey2",LocalDefaultText = @"מזהה חיצוני 2", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsMasavPaymentsToAgentQueryOEntityIdKey3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.MasavPaymentsToAgentQuery.O.EntityIdKey3", DefaultText = "EntityIdKey3",LocalDefaultText = @"מזהה חיצוני 3", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsImporterDeclarationQueryODeclarationDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ImporterDeclarationQuery.O.DeclarationDate", DefaultText = "Declaration Date",LocalDefaultText = @"תאריך תצהיר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeficitFileFilterQueryFTotalRefundAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeficitFileFilterQuery.F.TotalRefundAmount", DefaultText = "Total Refund Amount",LocalDefaultText = @"סכום ששולם", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeficitFileFilterQueryFTotalComponentAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeficitFileFilterQuery.F.TotalComponentAmount", DefaultText = "Total Component Amount",LocalDefaultText = @"סכום חוב", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestORepresentative = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.O.Representative", DefaultText = "Representative",LocalDefaultText = @"נציגים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFRepresentativeNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.RepresentativeNumber", DefaultText = "Row",LocalDefaultText = @"שורה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFRepresentativeName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.RepresentativeName", DefaultText = "Representative Name",LocalDefaultText = @"שם נציג", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFRepresentativeID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.RepresentativeID", DefaultText = "Representative ID",LocalDefaultText = @"תעודת זהות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFCurrentRePackingOldLineNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.CurrentRePackingOldLineNumber", DefaultText = "Serial",LocalDefaultText = @"סידורי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFCurrentPresentPackingStateContent = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.CurrentPresentPackingStateContent", DefaultText = "Comments",LocalDefaultText = @"הערות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFRepackingCurrentPackageType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.RepackingCurrentPackageType", DefaultText = "Package Type",LocalDefaultText = @"סוג אריזה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFRepackingCurrentPackageId = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.RepackingCurrentPackageId", DefaultText = "Package Id",LocalDefaultText = @"מזהה אריזה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFRepackingCurrentQuantity = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.RepackingCurrentQuantity", DefaultText = "Quantity",LocalDefaultText = @"כמות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFRepackingCurrentWeight = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.RepackingCurrentWeight", DefaultText = "Weight",LocalDefaultText = @"משקל", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFDesiredRePackingNewLineNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.DesiredRePackingNewLineNumber", DefaultText = "Serial",LocalDefaultText = @"סידורי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFDesiredRePackingOldLineNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.DesiredRePackingOldLineNumber", DefaultText = "New serial",LocalDefaultText = @"מספר סידורי חדש", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFRepackingDesiredPackageType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.RepackingDesiredPackageType", DefaultText = "Package Type",LocalDefaultText = @"סוג אריזה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFRepackingDesiredPackageId = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.RepackingDesiredPackageId", DefaultText = "Package Id",LocalDefaultText = @"מזהה אריזה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFRepackingDesiredQuantity = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.RepackingDesiredQuantity", DefaultText = "Quantity",LocalDefaultText = @"כמות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFRepackingDesiredWeight = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.RepackingDesiredWeight", DefaultText = "Weight",LocalDefaultText = @"משקל", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOPaymentQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.PaymentQuery", DefaultText = "Payment Query",LocalDefaultText = @"שאילתא להוראות תשלום", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOPaymentQueryHeader = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.PaymentQueryHeader", DefaultText = "Payment Query",LocalDefaultText = @"שאילתא להוראות תשלום", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOPaymentQueryDataMissing = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.PaymentQueryDataMissing", DefaultText = "Importer or Agent VAT is mandatory",LocalDefaultText = @"חובה להזין מספר יבואן או סוכן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOPaymentQueryResults = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.PaymentQueryResults", DefaultText = "Results",LocalDefaultText = @"תוצאות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsPaymentQueryOImporterVat = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PaymentQuery.O.ImporterVat", DefaultText = "Importer VAT",LocalDefaultText = @"ח.פ יבואן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsPaymentQueryOAgentVat = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PaymentQuery.O.AgentVat", DefaultText = "Agent VAT",LocalDefaultText = @"ח.פ סוכן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsPaymentQueryOpaymentDateFrom = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PaymentQuery.O.paymentDateFrom", DefaultText = "Payment Date From",LocalDefaultText = @"תאריך תשלום מ", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsPaymentQueryOpaymentDateTo = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PaymentQuery.O.paymentDateTo", DefaultText = "Payment Date To",LocalDefaultText = @"תאריך תשלום עד", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsPaymentQueryOInternalBankId = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PaymentQuery.O.InternalBankId", DefaultText = "Agent Bank",LocalDefaultText = @"בנק סוכן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsPaymentQueryOEffectiveDateFrom = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PaymentQuery.O.EffectiveDateFrom", DefaultText = "Effective Date From",LocalDefaultText = @"תאריך תוקף מ", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsPaymentQueryOEffectiveDateTo = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PaymentQuery.O.EffectiveDateTo", DefaultText = "Effective Date To",LocalDefaultText = @"תאריך תוקף עד", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsPaymentQueryOBankId = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PaymentQuery.O.BankId", DefaultText = "Bank No.",LocalDefaultText = @"מספר בנק", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsPaymentQueryOBranchId = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PaymentQuery.O.BranchId", DefaultText = "Branch",LocalDefaultText = @"סניף", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsPaymentQueryOPaymentOrderStatus = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PaymentQuery.O.PaymentOrderStatus", DefaultText = "Payment Order Status",LocalDefaultText = @"סטטוס הוראה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsPaymentQueryOPaymentOrderType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PaymentQuery.O.PaymentOrderType", DefaultText = "Payment Order Type",LocalDefaultText = @"סוג הוראת תשלום", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsPaymentQueryOPaymentProcess = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PaymentQuery.O.PaymentProcess", DefaultText = "Payment Process",LocalDefaultText = @"התהליך היוצר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsPaymentQueryOBankAccount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PaymentQuery.O.BankAccount", DefaultText = "Bank Account",LocalDefaultText = @"מספר חשבון", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsPaymentQueryOEntityType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PaymentQuery.O.EntityType", DefaultText = "Entity Type",LocalDefaultText = @"סוג ישות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsPaymentQueryOEntityExternalID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PaymentQuery.O.EntityExternalID", DefaultText = "Entity Type",LocalDefaultText = @"מספר ישות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsPaymentQueryOPaymentID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PaymentQuery.O.PaymentID", DefaultText = "Payment ID",LocalDefaultText = @"מספר הוראת תשלום", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsPaymentQueryOPaymentAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PaymentQuery.O.PaymentAmount", DefaultText = "Payment Amount",LocalDefaultText = @"סכום הוראה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsPaymentQueryOImporter = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PaymentQuery.O.Importer", DefaultText = "Importer",LocalDefaultText = @"יבואן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsPaymentQueryOAgent = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PaymentQuery.O.Agent", DefaultText = "Agent",LocalDefaultText = @"סוכן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsPaymentQueryOPaymentMethodType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PaymentQuery.O.PaymentMethodType", DefaultText = "Payment Method Type",LocalDefaultText = @"אמצעי תשלום", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsPaymentQueryOPaymentType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.PaymentQuery.O.PaymentType", DefaultText = "Payment Type",LocalDefaultText = @"סוג הוראה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOGuaranteeFileFilterQueryMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.GuaranteeFileFilterQueryMessage", DefaultText = "Guarantee File Filter Query Message",LocalDefaultText = @"שאילתא לערבויות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOGuaranteeFileFilterQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.GuaranteeFileFilterQuery", DefaultText = "Guarantee File Filter Query",LocalDefaultText = @"שאילתא לערבויות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryOGuaranteeFileFilterQueryResults = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.O.GuaranteeFileFilterQueryResults", DefaultText = "Guarantee File Filter Results",LocalDefaultText = @"תוצאות שאילתא לערבויות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryOGuaranteeFileFilterGeneralResults = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.O.GuaranteeFileFilterGeneralResults", DefaultText = "General Details",LocalDefaultText = @"נתונים כלליים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryOGuaranteeFileFilterGuaranteeLettersResults = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.O.GuaranteeFileFilterGuaranteeLettersResults", DefaultText = "Guarantee Letters",LocalDefaultText = @"רשימת כתבי ערבות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryOGuaranteeFileFilterCreditTransactionsResults = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.O.GuaranteeFileFilterCreditTransactionsResults", DefaultText = "Credit Transactions",LocalDefaultText = @"תנועות אשראי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryOGuaranteeFileFilterDocumentsResults = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.O.GuaranteeFileFilterDocumentsResults", DefaultText = "Documents Details",LocalDefaultText = @"מסמכים נדרשים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryFFileNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.F.FileNumber", DefaultText = "File Number",LocalDefaultText = @"מספר תיק תפג", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryFGuranteeType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.F.GuranteeType", DefaultText = "Gurantee File",LocalDefaultText = @"סוג ערבות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryFGuranteeTypeFile = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.F.GuranteeType.File", DefaultText = "Gurantee Type",LocalDefaultText = @"תיק ערבות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryFGuranteeTypeReq = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.F.GuranteeType.Req", DefaultText = "Gurantee Request",LocalDefaultText = @"בקשה לערבות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryFNumeral = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.F.Numeral", DefaultText = "Numeral",LocalDefaultText = @"מספר רץ", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryFCustomOfficeName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.F.CustomOfficeName", DefaultText = "CustomOffice Name",LocalDefaultText = @"בית המכס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryFExternalName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.F.ExternalName", DefaultText = "External Name",LocalDefaultText = @"יבואן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryFAgentName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.F.AgentName", DefaultText = "Agent Name",LocalDefaultText = @"הסוכן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryFDisplayFileNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.F.DisplayFileNumber", DefaultText = "Display File Number",LocalDefaultText = @"מספר תיק", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryFEntityTypeName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.F.EntityTypeName", DefaultText = "Entity Type",LocalDefaultText = @"ישות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryFGuaranteeEntityID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.F.GuaranteeEntityID", DefaultText = "Guarantee Entity",LocalDefaultText = @"מספר ישות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryFStatusName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.F.StatusName", DefaultText = "Status",LocalDefaultText = @"סטטוס התיק", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryFStatus = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.F.Status", DefaultText = "Status",LocalDefaultText = @"סטטוס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryFCreditLimit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.F.CreditLimit", DefaultText = "Credit Limit",LocalDefaultText = @"תקרת אשראי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryFCreditBalance = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.F.CreditBalance", DefaultText = "Credit Balance",LocalDefaultText = @"יתרת אשראי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryFGuaranteedName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.F.GuaranteedName", DefaultText = "Guaranteed Name",LocalDefaultText = @"שם הנערב", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryFGuaranteeExecutedAmountAdjusted = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.F.GuaranteeExecutedAmountAdjusted", DefaultText = "Guarantee Executed Amount Adjusted",LocalDefaultText = @"סכום שמומש", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryFGuaranteeAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.F.GuaranteeAmount", DefaultText = "Guarantee Amount",LocalDefaultText = @"סכום ערבות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryFGuaranteeTypeName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.F.GuaranteeTypeName", DefaultText = "Guarantee Type Name",LocalDefaultText = @"סוג כתב ערבות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryFCertificateID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.F.CertificateID", DefaultText = "Certificate ID",LocalDefaultText = @"מספר ערבות פנימי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryFGuaranteeExternalCertificateNumebr = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.F.GuaranteeExternalCertificateNumebr", DefaultText = "Guarantee External Certificate Numebr",LocalDefaultText = @"מספר ערבות חיצוני", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryFGuaranatorName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.F.GuaranatorName", DefaultText = "Guaranator Name",LocalDefaultText = @"שם הערב", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryFGuaranteeValidityDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.F.GuaranteeValidityDate", DefaultText = "Guarantee Validity Date",LocalDefaultText = @"תוקף הערבות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryFCertificateAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.F.CertificateAmount", DefaultText = "Certificate Amount",LocalDefaultText = @"סכום ערבות כולל", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryFCertificateAllocation = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.F.CertificateAllocation", DefaultText = "Certificate Allocation",LocalDefaultText = @"סכום מוקצה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryFAvaliableCertificateAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.F.AvaliableCertificateAmount", DefaultText = "Avaliable Certificate Amount",LocalDefaultText = @"סכום פנוי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryFCreditTransactionDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.F.CreditTransactionDate", DefaultText = "Credit Transaction Date",LocalDefaultText = @"מועד תנועה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryFCreditTransactionName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.F.CreditTransactionName", DefaultText = "Credit Transaction Name",LocalDefaultText = @"סוג תנועה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryFAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.F.Amount", DefaultText = "Amount",LocalDefaultText = @"סכום", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryFValidity = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.F.Validity", DefaultText = "Validity",LocalDefaultText = @"תוקף", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryFDocumentTypeName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.F.DocumentTypeName", DefaultText = "Document Type",LocalDefaultText = @"סוג מסמך", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryFDocumentID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.F.DocumentID", DefaultText = "Document ID",LocalDefaultText = @"סימוכין מכס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryOFileNumberMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.O.FileNumberMandatory", DefaultText = "File Number field is mandatory",LocalDefaultText = @"מספר תיק תפג הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryONumeralMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.O.NumeralMandatory", DefaultText = "Numeral field is mandatory",LocalDefaultText = @"מספר רץ הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeFileFilterQueryOGuaranteeType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeFileFilterQuery.O.GuaranteeType", DefaultText = "Guarantee Type field is mandatory",LocalDefaultText = @"סוג ערבות הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationStatusQueryODeclarationID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationStatusQuery.O.DeclarationID", DefaultText = "Declaration ID",LocalDefaultText = @"מס' הצהרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationStatusQueryODeclarationStatusText = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationStatusQuery.O.DeclarationStatusText", DefaultText = "Declaration Status",LocalDefaultText = @"סטטוס הצהרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationStatusQueryOLogisticStatusText = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationStatusQuery.O.LogisticStatusText", DefaultText = "Logistic Status",LocalDefaultText = @"סטטוס לוגיסטי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationStatusQueryOTaxationDateTime = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationStatusQuery.O.TaxationDateTime", DefaultText = "Taxation Date",LocalDefaultText = @"תאריך חישוב מיסים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationStatusQueryOReleaseDateTime = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationStatusQuery.O.ReleaseDateTime", DefaultText = "Release Date",LocalDefaultText = @"תאריך התרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationStatusQueryODeclarationVersion = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationStatusQuery.O.DeclarationVersion", DefaultText = "Declaration Version",LocalDefaultText = @"מס' גרסה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationStatusQueryODeclarationOfficeText = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationStatusQuery.O.DeclarationOfficeText", DefaultText = "Declaration Office",LocalDefaultText = @"תחנת מכס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationStatusQueryOFinancialStatusText = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationStatusQuery.O.FinancialStatusText", DefaultText = "Financial Status",LocalDefaultText = @"סטטוס כספי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationStatusQueryOSubmitDateTime = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationStatusQuery.O.SubmitDateTime", DefaultText = "Submit Date",LocalDefaultText = @"תאריך הגשה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsMasterBOLQueryOInternalIdentifier = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.MasterBOLQuery.O.InternalIdentifier", DefaultText = "Internal Identifier",LocalDefaultText = @"מזהה פנימי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsMasterBOLQueryOInternalIdentifierIsMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.MasterBOLQuery.O.InternalIdentifierIsMandatory", DefaultText = "Internal field is mandatory",LocalDefaultText = @"מזהה פנימי הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFActivityRequestStartEndDateMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.ActivityRequestStartEndDateMandatory", DefaultText = "Activity Request Start or End Date is mandatory",LocalDefaultText = @"תאריך תחילת או סיום הפעולה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFAuthorityCodeMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.AuthorityCodeMandatory", DefaultText = "Authority Code field is mandatory",LocalDefaultText = @"רשות מוסמכת עבור דוגמא הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFCargoRowNumbeMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.CargoRowNumbeMandatory", DefaultText = "Cargo Row Number field is mandatory",LocalDefaultText = @"מספר סידורי במטען הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFSiteNumberMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.SiteNumberMandatory", DefaultText = "Site Number field is mandatory",LocalDefaultText = @"מספר אתר הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFCargoIdentifierTypMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.CargoIdentifierTypMandatory", DefaultText = "Cargo Identifier Type field is mandatory",LocalDefaultText = @"סוג מזהה מטען הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFCargoIdentifierKey1Mandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.CargoIdentifierKey1Mandatory", DefaultText = "Cargo IdentifierKey 1 field is mandatory",LocalDefaultText = @"מזהה מטען ראשון הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFCargoIdentifierKey2Mandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.CargoIdentifierKey2Mandatory", DefaultText = "Cargo IdentifierKey 2 field is mandatory",LocalDefaultText = @"מזהה מטען שני הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFIdemanderTypeMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.IdemanderTypeMandatory", DefaultText = "Idemander Type field is mandatory",LocalDefaultText = @"סוג גורם מבקש פירוט הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFSpecialActionsCodeMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.SpecialActionsCodeMandatory", DefaultText = "Special Actions Code field is mandatory",LocalDefaultText = @"קוד פעולות מיוחדות לביצוע הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFGoodsDescriptionMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.GoodsDescriptionMandatory", DefaultText = "Goods Description field is mandatory",LocalDefaultText = @"תיאור טובין הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFSampleItemsMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.SampleItemsMandatory", DefaultText = "Sample is mandatory",LocalDefaultText = @"חובה להזין דוגמאות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFRepresentativeItemsItemsMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.RepresentativeItemsItemsMandatory", DefaultText = "Representative is mandatory",LocalDefaultText = @"חובה להזין נציגים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFRepackingDesiredItemsItemsMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.RepackingDesiredItemsItemsMandatory", DefaultText = "Desired Repacking is mandatory",LocalDefaultText = @"חובה להזין מצב רצוי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFRepackingCurrentItemsItemsMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.RepackingCurrentItemsItemsMandatory", DefaultText = "Current Repacking is mandatory",LocalDefaultText = @"חובה להזין מצב קיים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFApprovalDateMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.ApprovalDateMandatory", DefaultText = "Approval Date field is mandatory",LocalDefaultText = @"תאריך אישור הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFApprovalNameMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.ApprovalNameMandatory", DefaultText = "Approval Name field is mandatory",LocalDefaultText = @"שם מאשר הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOBlockLogicalActivities = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.BlockLogicalActivities", DefaultText = "Block Logical Activities",LocalDefaultText = @"תנועות לוגיות לגוש", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOBlockStorageActivities = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.BlockStorageActivities", DefaultText = "Block Storage Activities",LocalDefaultText = @"תנועות פיזיות בגוש", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOFaultQueryResault = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.FaultQueryResault", DefaultText = "Fault Query Resault",LocalDefaultText = @"תוצאות שאילתא לליקויים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsFaultQueryOProceduralFaultID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.FaultQuery.O.ProceduralFaultID", DefaultText = "Fault Id",LocalDefaultText = @"מספר ליקוי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsFaultQueryOProceduralFaultInputProcess = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.FaultQuery.O.ProceduralFaultInputProcess", DefaultText = "Fault Input Process",LocalDefaultText = @"תהליך הזנת ליקוי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsFaultQueryOCreateDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.FaultQuery.O.CreateDate", DefaultText = "Create Date",LocalDefaultText = @"תאריך יצירה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsFaultQueryOProceduralFaultStatus = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.FaultQuery.O.ProceduralFaultStatus", DefaultText = "FaultS tatus",LocalDefaultText = @"סטטוס ליקוי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsFaultQueryODeclarationId = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.FaultQuery.O.DeclarationId", DefaultText = "Declaration Id",LocalDefaultText = @"מספר הצהרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsFaultQueryOResponsibilityID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.FaultQuery.O.ResponsibilityID", DefaultText = "Responsibility ID",LocalDefaultText = @"יבואן אחראי ליקוי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsFaultQueryOCustomsHouse = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.FaultQuery.O.CustomsHouse", DefaultText = "Customs House",LocalDefaultText = @"תחנת מכס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsFaultQueryOAgentInDeclaration = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.FaultQuery.O.AgentInDeclaration", DefaultText = "Agent In Declaration",LocalDefaultText = @"סוכן בהצהרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsFaultQueryOExporterImporterInDeclaration = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.FaultQuery.O.ExporterImporterInDeclaration", DefaultText = "Exporter/Importer In Declaration",LocalDefaultText = @"יבואן בהצהרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsFaultQueryOSeverity = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.FaultQuery.O.Severity", DefaultText = "Severity",LocalDefaultText = @"חומרת ליקוי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsFaultQueryOFelonyType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.FaultQuery.O.FelonyType", DefaultText = "Felony Type",LocalDefaultText = @"סוג עבירה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsFaultQueryORansomViolationType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.FaultQuery.O.RansomViolationType", DefaultText = "Ransom Violation Type",LocalDefaultText = @"סוג הפרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsFaultQueryORansomViolationSum = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.FaultQuery.O.RansomViolationSum", DefaultText = "Ransom Violation Sum",LocalDefaultText = @"ערך טובין הפרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsFaultQueryOScoring = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.FaultQuery.O.Scoring", DefaultText = "Scoring",LocalDefaultText = @"ניקוד ליקוי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestOApplicantAgentNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.O.ApplicantAgentNumber", DefaultText = "Agent Number",LocalDefaultText = @"מספר סוכן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsBlockListInWarehouseONumberOfBlocksInList = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.BlockListInWarehouse.O.NumberOfBlocksInList", DefaultText = "Number Of Blocks",LocalDefaultText = @"סה''כ גושים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOSpecialActivities = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.SpecialActivities", DefaultText = "Special Activities",LocalDefaultText = @"פעולות מיוחדות שבוצעו", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOGoodsItemByInvoice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.GoodsItemByInvoice", DefaultText = "Goods Item By Invoice",LocalDefaultText = @"פירוט סחורות לפי חשבון", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsWarehouseBlockBalanceOStorageUnloadingExceptionType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.WarehouseBlockBalance.O.StorageUnloadingExceptionType", DefaultText = "Storage Unloading Exception Type",LocalDefaultText = @"תיאור חריגה בקליטה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsWarehouseBlockBalanceOPackingType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.WarehouseBlockBalance.O.PackingType", DefaultText = "Packing Type",LocalDefaultText = @"סוג אריזה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsWarehouseBlockBalanceOStorageActionPackagesWeight = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.WarehouseBlockBalance.O.StorageActionPackagesWeight", DefaultText = "Storage Action Packages Weight",LocalDefaultText = @"משקל", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsWarehouseBlockBalanceOPackagesWeightAfterStorageAction = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.WarehouseBlockBalance.O.PackagesWeightAfterStorageAction", DefaultText = "Packages Weight After Storage Action",LocalDefaultText = @"משקל לאחר תנועה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsWarehouseBlockBalanceOSpecialActivityType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.WarehouseBlockBalance.O.SpecialActivityType", DefaultText = "Special Activity Type",LocalDefaultText = @"פעולה מיוחדת", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsWarehouseBlockBalanceOInvoiceSequenceNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.WarehouseBlockBalance.O.InvoiceSequenceNumber", DefaultText = "Invoice Sequence Number",LocalDefaultText = @"סידורי חשבון", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsWarehouseBlockBalanceOGoodsItemSequenceNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.WarehouseBlockBalance.O.GoodsItemSequenceNumber", DefaultText = "GoodsItem Sequence Number",LocalDefaultText = @"סידורי סחורה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsWarehouseBlockBalanceORemainingQuantity = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.WarehouseBlockBalance.O.RemainingQuantity", DefaultText = "Remaining Quantity",LocalDefaultText = @"כמות סטט' נותרת", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsWarehouseBlockBalanceOGoodsPriceBase = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.WarehouseBlockBalance.O.GoodsPriceBase", DefaultText = "Goods Price Base",LocalDefaultText = @"שווי בסיס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsWarehouseBlockBalanceOGoodsPriceMAD = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.WarehouseBlockBalance.O.GoodsPriceMAD", DefaultText = "Goods Price MAD",LocalDefaultText = @"שווי לאחר הנחות והתאמות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsWarehouseBlockBalanceOGoodsPriceMADAEF = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.WarehouseBlockBalance.O.GoodsPriceMADAEF", DefaultText = "GoodsPrice MAD AEF",LocalDefaultText = @"שווי סופי למכס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsWarehouseBlockBalanceOCurrencyType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.WarehouseBlockBalance.O.CurrencyType", DefaultText = "Currency Type",LocalDefaultText = @"סוג מטבע", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsWarehouseBlockBalanceOExchangeRate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.WarehouseBlockBalance.O.ExchangeRate", DefaultText = "Exchange Rate",LocalDefaultText = @"שער המרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralODeclarationFilterQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.DeclarationFilterQuery", DefaultText = "Declaration filter Query",LocalDefaultText = "שאילתא לתיקי תפ''ג עבור הצהרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralODeclarationFilterHeader = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.DeclarationFilterHeader", DefaultText = "Declaration filter Query",LocalDefaultText = "שאילתא לתיקי תפ''ג עבור הצהרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralODeclarationFilterDataMissing = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.DeclarationFilterDataMissing", DefaultText = "Declaration number is missing",LocalDefaultText = @"חסר מספר הצהרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationFilterFNumeral = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationFilter.F.Numeral", DefaultText = "file number / Numeral",LocalDefaultText = @"מספר תיק / רץ", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralODeclarationFilterGeneralDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.DeclarationFilterGeneralDetails", DefaultText = "General Details",LocalDefaultText = @"נתונים כלליים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationFilterFGeneralDetailsExternalID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationFilter.F.GeneralDetailsExternalID", DefaultText = "External ID",LocalDefaultText = @"מספר יבואן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationFilterFGeneralDetailsName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationFilter.F.GeneralDetailsName", DefaultText = "Name",LocalDefaultText = @"יבואן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationFilterFGeneralDetailsCustomOfficeName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationFilter.F.GeneralDetailsCustomOfficeName", DefaultText = "Custom Office Name",LocalDefaultText = @"בית מכס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationFilterFGeneralDetailsStatusName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationFilter.F.GeneralDetailsStatusName", DefaultText = "Status Name",LocalDefaultText = @"סטטוס הצהרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralODeclarationFilterClaim = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.DeclarationFilterClaim", DefaultText = "Claim",LocalDefaultText = @"נתוני תביעות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationFilterFClaimDisplayFileNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationFilter.F.ClaimDisplayFileNumber", DefaultText = "Display File Number",LocalDefaultText = @"תיק פרטני", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationFilterFClaimAgentName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationFilter.F.ClaimAgentName", DefaultText = "Agent Name",LocalDefaultText = @"סוכן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationFilterFClaimCreateDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationFilter.F.ClaimCreateDate", DefaultText = "Create Date",LocalDefaultText = @"תאריך יצירה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationFilterFClaimClaimAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationFilter.F.ClaimClaimAmount", DefaultText = "Claim Amount",LocalDefaultText = @"סכום תביעה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationFilterFClaimTotalRefundAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationFilter.F.ClaimTotalRefundAmount", DefaultText = "Total Refund Amount",LocalDefaultText = @"סכום החזר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationFilterFClaimCloseDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationFilter.F.ClaimCloseDate", DefaultText = "Close Date",LocalDefaultText = @"תאריך סגירה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationFilterFClaimStatusName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationFilter.F.ClaimStatusName", DefaultText = "Status Name",LocalDefaultText = @"סטטוס תיק", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralODeclarationFilterDeficit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.DeclarationFilterDeficit", DefaultText = "Deficit",LocalDefaultText = @"נתוני גרעונות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationFilterFDeficitDisplayFileNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationFilter.F.DeficitDisplayFileNumber", DefaultText = "Display File Number",LocalDefaultText = @"תיק פרטני", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationFilterFDeficitAgentName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationFilter.F.DeficitAgentName", DefaultText = "Agent Name",LocalDefaultText = @"סוכן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationFilterFDeficitProductionDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationFilter.F.DeficitProductionDate", DefaultText = "Production Date",LocalDefaultText = @"ת. הודעת חיוב", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationFilterFDeficitEstimatedBalance = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationFilter.F.DeficitEstimatedBalance", DefaultText = "Estimated Balance",LocalDefaultText = @"חוב משוערך", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationFilterFDeficitTotalRefundAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationFilter.F.DeficitTotalRefundAmount", DefaultText = "Total Refund Amount",LocalDefaultText = @"סכום ששולם", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationFilterFDeficitCloseDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationFilter.F.DeficitCloseDate", DefaultText = "Close Date",LocalDefaultText = @"תאריך סגירה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationFilterFDeficitStatusName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationFilter.F.DeficitStatusName", DefaultText = "Status Name",LocalDefaultText = @"סטטוס תיק", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralODeclarationFilterGuarantee = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.DeclarationFilterGuarantee", DefaultText = "Guarantees",LocalDefaultText = @"נתוני ערבויות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationFilterFGuaranteeDisplayFileNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationFilter.F.GuaranteeDisplayFileNumber", DefaultText = "Display File Number",LocalDefaultText = @"תיק פרטני", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationFilterFGuaranteeAgentName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationFilter.F.GuaranteeAgentName", DefaultText = "Agent Name",LocalDefaultText = @"סוכן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationFilterFGuaranteeFileTypeName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationFilter.F.GuaranteeFileTypeName", DefaultText = "File Type Name",LocalDefaultText = @"סוג תיק", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationFilterFGuaranteeValidity = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationFilter.F.GuaranteeValidity", DefaultText = "Validity",LocalDefaultText = @"תוקף ערבות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationFilterFGuaranteeAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationFilter.F.GuaranteeAmount", DefaultText = "Amount",LocalDefaultText = @"סכום מוקצה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationFilterFGuaranteeGuaranteeStatus = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationFilter.F.GuaranteeGuaranteeStatus", DefaultText = "Guarantee Status",LocalDefaultText = @"סטטוס תיק", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOGuaranteeCertificateFilterQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.GuaranteeCertificateFilterQuery", DefaultText = "Guarantee Certificate Filter Query",LocalDefaultText = @"שאילתא לנתוני כתב ערבות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOGuaranteeCertificateHeader = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.GuaranteeCertificateHeader", DefaultText = "Guarantee Certificate Filter Query",LocalDefaultText = @"שאילתא לנתוני כתב ערבות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeCertificateFNumeral = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeCertificate.F.Numeral", DefaultText = "file number / Numeral",LocalDefaultText = @"מספר תיק / רץ", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeCertificateFGuaranteeCertificateType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeCertificate.F.GuaranteeCertificateType", DefaultText = "Guarantee Certificate Type",LocalDefaultText = @"סוג כתב ערבות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeCertificateFCertificateID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeCertificate.F.CertificateID", DefaultText = "Certificate ID",LocalDefaultText = @"מזהה כתב ערבות פנימי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeCertificateFGuaranteeExternalCertificateNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeCertificate.F.GuaranteeExternalCertificateNumber", DefaultText = "Guarantee External Certificate Number",LocalDefaultText = @"מזהה כתב ערבות חיצוני", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeCertificateFGuarantorID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeCertificate.F.GuarantorID", DefaultText = "Guarantor ID",LocalDefaultText = @"סוג הערב", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOGuaranteeCertificateGeneralDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.GuaranteeCertificateGeneralDetails", DefaultText = "General Details",LocalDefaultText = @"נתונים כלליים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeCertificateFGeneralDetailsGuaranteeTypeName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeCertificate.F.GeneralDetailsGuaranteeTypeName", DefaultText = "Guarantee Type Name",LocalDefaultText = @"סוג כתב ערבות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeCertificateFGeneralDetailsGuaranteeExternalCertificateNumebr = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeCertificate.F.GeneralDetailsGuaranteeExternalCertificateNumebr", DefaultText = "Guarantee External Certificate Numebr",LocalDefaultText = @"מזהה ערבות חיצוני", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeCertificateFGeneralDetailsGuaranteeValidityDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeCertificate.F.GeneralDetailsGuaranteeValidityDate", DefaultText = "Guarantee Validity Date",LocalDefaultText = @"תוקף כתב העערבות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeCertificateFGeneralDetailsCertificateAvailableAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeCertificate.F.GeneralDetailsCertificateAvailableAmount", DefaultText = "Certificate Available Amount",LocalDefaultText = @"סכום פנוי להקצאה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeCertificateFGeneralDetailsGuaranteeCertificateStatus = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeCertificate.F.GeneralDetailsGuaranteeCertificateStatus", DefaultText = "Guarantee Certificate Status",LocalDefaultText = @"סטטוס ערבות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeCertificateFGeneralDetailsGuaranteedName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeCertificate.F.GeneralDetailsGuaranteedName", DefaultText = "Guaranteed Name",LocalDefaultText = @"שם הנערב", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeCertificateFGeneralDetailsCertificateID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeCertificate.F.GeneralDetailsCertificateID", DefaultText = "Certificate ID",LocalDefaultText = @"מזהה ערבות פנימי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeCertificateFGeneralDetailsGuaranteeAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeCertificate.F.GeneralDetailsGuaranteeAmount", DefaultText = "Guarantee Amount",LocalDefaultText = @"סכום ערבות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeCertificateFGeneralDetailsTotalCertificateAllocation = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeCertificate.F.GeneralDetailsTotalCertificateAllocation", DefaultText = "Total Certificate Allocation",LocalDefaultText = @"סכום שהקוצה מהערבות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOGuaranteeCertificateAllocation = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.GuaranteeCertificateAllocation", DefaultText = "Allocation List",LocalDefaultText = @"הקצאות על כתב הערבות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeCertificateFAllocationFileTypeName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeCertificate.F.AllocationFileTypeName", DefaultText = "File Type Name",LocalDefaultText = @"סוג תיק", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeCertificateFAllocationDisplayFileNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeCertificate.F.AllocationDisplayFileNumber", DefaultText = "Display File Number",LocalDefaultText = @"מספר תיק פרטני", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeCertificateFAllocationCertificateAllocationAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeCertificate.F.AllocationCertificateAllocationAmount", DefaultText = "Certificate Allocation Amount",LocalDefaultText = @"סכום מוקצה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeCertificateFAllocationValidity = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeCertificate.F.AllocationValidity", DefaultText = "Validity",LocalDefaultText = @"תאריך תוקף", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeCertificateFAllocationUpdateDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeCertificate.F.AllocationUpdateDate", DefaultText = "Update Date",LocalDefaultText = @"תאריך עדכון", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOGuaranteeCertificateRequest = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.GuaranteeCertificateRequest", DefaultText = "Request List",LocalDefaultText = @"בקשות על כתב הערבות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeCertificateFRequestGuarenteeRequestNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeCertificate.F.RequestGuarenteeRequestNumber", DefaultText = "Guarentee Request Number",LocalDefaultText = @"מספר בקשה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeCertificateFRequestDisplayFileNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeCertificate.F.RequestDisplayFileNumber", DefaultText = "Display File Number",LocalDefaultText = @"מספר תיק פרטני", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeCertificateFRequestRequestDescription = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeCertificate.F.RequestRequestDescription", DefaultText = "Request Description",LocalDefaultText = @"תיאור הבקשה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeCertificateFRequestRequestedExecutionValue = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeCertificate.F.RequestRequestedExecutionValue", DefaultText = "Requested Execution Value",LocalDefaultText = @"הסכום המבוקש", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeCertificateFRequestRequestedValidityDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeCertificate.F.RequestRequestedValidityDate", DefaultText = "Requested Validity Date",LocalDefaultText = @"התוקף המבוקש", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeCertificateFRequestCreateTime = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeCertificate.F.RequestCreateTime", DefaultText = "Create Time",LocalDefaultText = @"תאריך יצירה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGuaranteeCertificateFRequestGuarenteeRequestStatusName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.GuaranteeCertificate.F.RequestGuarenteeRequestStatusName", DefaultText = "Guarentee Request Status Name",LocalDefaultText = @"סטטוס בקשה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFSampleReturnDateMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.SampleReturnDateMandatory", DefaultText = "Sample Return Date is mandatory",LocalDefaultText = @"מועד החזרת דוגמא הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFSampleValueMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.SampleValueMandatory", DefaultText = "Sample Value is mandatory",LocalDefaultText = @"ערך הדוגמא הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFSampleDescriptionMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.SampleDescriptionMandatory", DefaultText = "Sample Description is mandatory",LocalDefaultText = @"תיאור דוגמא הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFCurrencyTypeCodeMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.CurrencyTypeCodeMandatory", DefaultText = "Currency Type is mandatory",LocalDefaultText = @"סוג מטבע הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestFCurrencyTypeCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.F.CurrencyTypeCode", DefaultText = "Currency Type",LocalDefaultText = @"סוג מטבע", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCourierBOLQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CourierBOLQuery", DefaultText = "Courier BOL Query",LocalDefaultText = @"שאילתא לשטרי מטען בלדר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCourierBOLQueryOQueryDataMissing = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierBOLQuery.O.QueryDataMissing", DefaultText = "CourierBillOfLadingNumber and Courier VAT are mandatory",LocalDefaultText = "חובה להזין ח.פ בלדר ומספר שט''מ בלדר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCourierBOLQueryOCourierBOLQueryDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierBOLQuery.O.CourierBOLQueryDetails", DefaultText = "Courier BOL Query Details",LocalDefaultText = "נתוני שט''מ ראשי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCourierBOLQueryOCourierBOLQueryResult = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierBOLQuery.O.CourierBOLQueryResult", DefaultText = "Courier BOL Query Result",LocalDefaultText = "נתוני שט''מ בלדר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCourierBOLQueryFCourierBOL = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierBOLQuery.F.CourierBOL", DefaultText = "Courier BOL",LocalDefaultText = "מספר שט''מ בלדר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCourierBOLQueryFCourierVAT = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierBOLQuery.F.CourierVAT", DefaultText = "CourierVAT",LocalDefaultText = @"ח.פ בלדר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCourierBOLQueryFcargoIdentiferTypeId = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierBOLQuery.F.cargoIdentiferTypeId", DefaultText = "Cargo Identifer TypeId",LocalDefaultText = @"מזהה מטען", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCourierBOLQueryFcargoIdentifierKey1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierBOLQuery.F.cargoIdentifierKey1", DefaultText = "Cargo Identifier Key1",LocalDefaultText = "מס' שט''מ בלדר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCourierBOLQueryFcargoIdentifierKey3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CourierBOLQuery.F.cargoIdentifierKey3", DefaultText = "Cargo Identifier Key3",LocalDefaultText = "תאריך שט''מ בלדר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCargoDetailsResults = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CargoDetailsResults", DefaultText = "Cargo Details",LocalDefaultText = @"פרטי עסקה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCargoQueryManifestResults = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CargoQueryManifestResults", DefaultText = "Manifest Details",LocalDefaultText = @"נתוני מצהר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryManifestResultsOManifestType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryManifestResults.O.ManifestType", DefaultText = "Type",LocalDefaultText = @"סוג", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryManifestResultsOManifestNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryManifestResults.O.ManifestNumber", DefaultText = "Number",LocalDefaultText = @"מספר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryManifestResultsOManifestStatus = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryManifestResults.O.ManifestStatus", DefaultText = "Status",LocalDefaultText = @"סטטוס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCargoQueryCargoResults = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CargoQueryCargoResults", DefaultText = "Cargo Details",LocalDefaultText = @"נתוני עסקה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargoResultsOCargoIdentifierKey1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargoResults.O.CargoIdentifierKey1", DefaultText = "Cargo",LocalDefaultText = @"עסקה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargoResultsOParentCargoID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargoResults.O.ParentCargoID", DefaultText = "Parent Cargo",LocalDefaultText = @"עסקת אב", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargoResultsOUnloadingLocationName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargoResults.O.UnloadingLocationName", DefaultText = "Unloading Location",LocalDefaultText = @"אתר פריקה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargoResultsOTotalNumberOfPackeges = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargoResults.O.TotalNumberOfPackeges", DefaultText = "Total Of Packeges",LocalDefaultText = @"כמות מוצהרת", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargoResultsOTotalRecordNumberOfPackeges = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargoResults.O.TotalRecordNumberOfPackeges", DefaultText = "Total Record Of Packeges",LocalDefaultText = @"כמות קליטה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargoResultsOGovernmentProcedureTypeName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargoResults.O.GovernmentProcedureTypeName", DefaultText = "Government Procedure",LocalDefaultText = @"תהליך מכסי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargoResultsOTreatmentWayName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargoResults.O.TreatmentWayName", DefaultText = "Treatment Way",LocalDefaultText = @"צורת טיפול", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargoResultsOGoodsReceiptPlaceSiteName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargoResults.O.GoodsReceiptPlaceSiteName", DefaultText = "Goods Receipt Site",LocalDefaultText = @"אתר מסירה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargoResultsOTotalWeight = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargoResults.O.TotalWeight", DefaultText = "Total Weight",LocalDefaultText = @"משקל מוצהר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargoResultsOTotalRecordWeight = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargoResults.O.TotalRecordWeight", DefaultText = "Total Record Weight",LocalDefaultText = @"משקל קליטה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargoResultsOMasterBolNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargoResults.O.MasterBolNumber", DefaultText = "Master Bol",LocalDefaultText = @"שטר מטען", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargoResultsOBillOfLadingNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargoResults.O.BillOfLadingNumber", DefaultText = "Bill Of Lading Number",LocalDefaultText = @"שטר מטען פנימי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargoResultsOTransitDestinationLocationName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargoResults.O.TransitDestinationLocationName", DefaultText = "Transit Destination Location",LocalDefaultText = @"אתר יציאה מהארץ", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCargoQueryDeliveryOrderResults = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CargoQueryDeliveryOrderResults", DefaultText = "Delivery Order",LocalDefaultText = @"פקודת מסירה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryDeliveryOrderResultsODeliveryOrderNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryDeliveryOrderResults.O.DeliveryOrderNumber", DefaultText = "Delivery Order Number",LocalDefaultText = @"מספר פקודה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryDeliveryOrderResultsOProducerName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryDeliveryOrderResults.O.ProducerName", DefaultText = "Producer Name",LocalDefaultText = @"גורם מוסר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryDeliveryOrderResultsOReceiverCustomerActivityTypeName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryDeliveryOrderResults.O.ReceiverCustomerActivityTypeName", DefaultText = "Receiver Activity Type",LocalDefaultText = @"סוג מקבל", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryDeliveryOrderResultsOReceiverName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryDeliveryOrderResults.O.ReceiverName", DefaultText = "Receiver Name",LocalDefaultText = @"גורם מקבל", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryDeliveryOrderResultsODeliveryOrderDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryDeliveryOrderResults.O.DeliveryOrderDate", DefaultText = "Delivery Order Date",LocalDefaultText = @"זמן קליטה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryDeliveryOrderResultsODeliveryOrderStatusName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryDeliveryOrderResults.O.DeliveryOrderStatusName", DefaultText = "Status",LocalDefaultText = @"סטטוס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryDeliveryOrderResultsODeliverySiteName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryDeliveryOrderResults.O.DeliverySiteName", DefaultText = "Delivery Site",LocalDefaultText = @"נשלח אל", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCargoQueryCargosVersionResults = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CargoQueryCargosVersionResults", DefaultText = "Cargos Version",LocalDefaultText = @"גרסאות עסקה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargosVersionResultsOVersion = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargosVersionResults.O.Version", DefaultText = "Version",LocalDefaultText = @"מספר גרסה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargosVersionResultsOSubmiterName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargosVersionResults.O.SubmiterName", DefaultText = "Submiter Name",LocalDefaultText = @"גורם מדווח", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargosVersionResultsOCreateDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargosVersionResults.O.CreateDate", DefaultText = "Create Date",LocalDefaultText = @"זמן קליטה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargosVersionResultsOActionDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargosVersionResults.O.ActionDate", DefaultText = "Action Date",LocalDefaultText = @"זמן אישור", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargosVersionResultsOCargoStausName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargosVersionResults.O.CargoStausName", DefaultText = "Status",LocalDefaultText = @"סטטוס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCargoQueryCargoItemResults = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CargoQueryCargoItemResults", DefaultText = "Cargos Items",LocalDefaultText = @"סידוריים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargoItemResultsORowNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargoItemResults.O.RowNumber", DefaultText = "Row Number",LocalDefaultText = @"סידורי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargoItemResultsOParentCargoRowDetailsID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargoItemResults.O.ParentCargoRowDetailsID", DefaultText = "Parent Row Number",LocalDefaultText = @"סידורי בעסקת אב", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargoItemResultsOContainerNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargoItemResults.O.ContainerNumber", DefaultText = "Container Number",LocalDefaultText = @"מספר מכולה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargoItemResultsOCharacteristicCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargoItemResults.O.CharacteristicCode", DefaultText = "Characteristic Code",LocalDefaultText = @"מכולה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargoItemResultsOContainerType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargoItemResults.O.ContainerType", DefaultText = "Container Type",LocalDefaultText = @"סוג מכולה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargoItemResultsOLength = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargoItemResults.O.Length", DefaultText = "Length",LocalDefaultText = @"אורך", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargoItemResultsOQuantity = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargoItemResults.O.Quantity", DefaultText = "Quantity",LocalDefaultText = @"כמות מוצהרת", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargoItemResultsOGrossMassMeasureWeight = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargoItemResults.O.GrossMassMeasureWeight", DefaultText = "Measure Weight",LocalDefaultText = @"משקל מוצהר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargoItemResultsORecordNumberOfPackeges = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargoItemResults.O.RecordNumberOfPackeges", DefaultText = "Record Number Of Packeges",LocalDefaultText = @"כמות קליטה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargoItemResultsOTotalRecordWeight = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargoItemResults.O.TotalRecordWeight", DefaultText = "Total Record Weight",LocalDefaultText = @"משקל קליטה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargoItemResultsODangerousGoodsIndication = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargoItemResults.O.DangerousGoodsIndication", DefaultText = "Dangerous Indication",LocalDefaultText = @"חומר מסוכן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCargoQuerySealDetailsResults = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CargoQuerySealDetailsResults", DefaultText = "Seal Details",LocalDefaultText = @"נתוני סגר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQuerySealDetailsResultsORowNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQuerySealDetailsResults.O.RowNumber", DefaultText = "Row Number",LocalDefaultText = @"סידורי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQuerySealDetailsResultsOSealTypeName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQuerySealDetailsResults.O.SealTypeName", DefaultText = "Seal Type",LocalDefaultText = @"סוג סגר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQuerySealDetailsResultsOSealNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQuerySealDetailsResults.O.SealNumber", DefaultText = "Seal Number",LocalDefaultText = @"מספר סגר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCargoQueryCargoMovmentResults = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CargoQueryCargoMovmentResults", DefaultText = "Movment Details",LocalDefaultText = @"תנועות מטען", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargoMovmentResultsOStatusName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargoMovmentResults.O.StatusName", DefaultText = "Movment Status",LocalDefaultText = @"סטטוס תנועה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargoMovmentResultsORowNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargoMovmentResults.O.RowNumber", DefaultText = "Row Number",LocalDefaultText = @"סידורי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargoMovmentResultsOExitReasonName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargoMovmentResults.O.ExitReasonName", DefaultText = "Exit Reason",LocalDefaultText = @"סיבת יציאה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargoMovmentResultsODocumentNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargoMovmentResults.O.DocumentNumber", DefaultText = "Document Number",LocalDefaultText = @"מס. תעודה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargoMovmentResultsOReferenceTypeName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargoMovmentResults.O.ReferenceTypeName", DefaultText = "Reference Type",LocalDefaultText = @"סוג אסמכתא", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargoMovmentResultsOReferenceNum = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargoMovmentResults.O.ReferenceNum", DefaultText = "Reference Num",LocalDefaultText = @"מס. אסמכתא", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargoMovmentResultsOExitSiteName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargoMovmentResults.O.ExitSiteName", DefaultText = "Exit Site",LocalDefaultText = @"אתר יציאה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargoMovmentResultsOExitDateTime = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargoMovmentResults.O.ExitDateTime", DefaultText = "Exit Date",LocalDefaultText = @"ת. יציאה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargoMovmentResultsOEntrySiteName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargoMovmentResults.O.EntrySiteName", DefaultText = "Entry Site",LocalDefaultText = @"אתר כניסה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCargoQueryCargoMovmentResultsOEntryDateTime = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CargoQueryCargoMovmentResults.O.EntryDateTime", DefaultText = "Entry Date",LocalDefaultText = @"ת. כניסה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralODeclarationReshimonConversion = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.DeclarationReshimonConversion", DefaultText = "Special Activity Request",LocalDefaultText = @"המרות בין מספר רשימון ומספר הצהרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralODeclarationReshimonConversionHeader = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.DeclarationReshimonConversionHeader", DefaultText = "Special Activity Request",LocalDefaultText = @"המרות בין מספר רשימון ומספר הצהרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationReshimonConversionODeclarationType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationReshimonConversion.O.DeclarationType", DefaultText = "Declaration Type",LocalDefaultText = @"סוג הצהרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationReshimonConversionODeclarationNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationReshimonConversion.O.DeclarationNumber", DefaultText = "Converted Declaration Number",LocalDefaultText = @"מספר הצהרה מוסבת", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationReshimonConversionOReshimonNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationReshimonConversion.O.ReshimonNumber", DefaultText = "Reshimon Number",LocalDefaultText = @"מספר רשימון", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationReshimonConversionOReshimonNumberLengthError = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationReshimonConversion.O.ReshimonNumberLengthError", DefaultText = "Reshimon Number Length Error",LocalDefaultText = @"מספר רשימון חייב להיות עם 9 ספרות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationReshimonConversionOReshimonNumberControlDigitError = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationReshimonConversion.O.ReshimonNumberControlDigitError", DefaultText = "Reshimon Number Control Digit Error",LocalDefaultText = @"שגיאה בסיפרה האחרונה של מספר הרשומון", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationReshimonConversionODeclarationNumberLengthError = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationReshimonConversion.O.DeclarationNumberLengthError", DefaultText = "Declaration Number Length Error",LocalDefaultText = @"מספר הצהרה חייבת להיות עם 14 ספרות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationReshimonConversionODeclarationNumberConvertionDigitsError = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationReshimonConversion.O.DeclarationNumberConvertionDigitsError", DefaultText = "Declaration Number Convertion Digits Error",LocalDefaultText = @"ספרות 3+4 צריכות להיות 98 ביבוא או 99 ביצוא", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationOIsSignedVersionError = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.IsSignedVersionError", DefaultText = "Last version was not signed - Can not send payment",LocalDefaultText = @"גרסת הצהרה אחרונה אינה חתומה - לא ניתן להגיש תשלום", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationOIsSignedVersionErrorForCustomerCare = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.IsSignedVersionErrorForCustomerCare", DefaultText = "Last version was not signed , send anyway ? ",LocalDefaultText = @"גרסה אחרונה לא נחתמה לשלוח בכל מקרה ?", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsExchangeRateOCurrencyTypeName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ExchangeRate.O.CurrencyTypeName", DefaultText = "Currency Type",LocalDefaultText = @"שם מטבע", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOIsConvertedDeclaration = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.IsConvertedDeclaration", DefaultText = "Converted Declaration",LocalDefaultText = @"הצהרה מוסבת", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsImporterDeclarationQueryODeclarationConectAll = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ImporterDeclarationQuery.O.DeclarationConect.All", DefaultText = "All",LocalDefaultText = @"הכל", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsImporterDeclarationQueryODeclarationConectImportDeclaration = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ImporterDeclarationQuery.O.DeclarationConect.ImportDeclaration", DefaultText = "Import Declaration",LocalDefaultText = @"הצהרת יבוא", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsImporterDeclarationQueryODeclarationConectVendor = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ImporterDeclarationQuery.O.DeclarationConect.Vendor", DefaultText = "Vendor",LocalDefaultText = @"ספק", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsImporterDeclarationQueryOVendorID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ImporterDeclarationQuery.O.VendorID", DefaultText = "Vendor ID",LocalDefaultText = @"מספר ספק", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationPrintQueryOSuccessMessgae = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationPrintQuery.O.SuccessMessgae", DefaultText = "Message was sent Successfully. You can see the form in declaration",LocalDefaultText = @"מסר הדפסת הצהרה בוצע בהצלחה. ניתן לראות את הטופס בהצהרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSpecialActivityRequestOCheckSite = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SpecialActivityRequest.O.CheckSite", DefaultText = "Check Site",LocalDefaultText = @"אתר בדיקה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOClaimFileFilterMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.ClaimFileFilterMessage", DefaultText = "Customs Claim Message",LocalDefaultText = @"תביעות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOClaimFileFilterQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.ClaimFileFilterQuery", DefaultText = "Claim Query",LocalDefaultText = @"שאילתא לתביעות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimFileFilterOClaimFileFilterResults = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClaimFileFilter.O.ClaimFileFilterResults", DefaultText = "Claim File Filter Results",LocalDefaultText = @"תוצאות שאילתא לתביעות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimFileFilterOFileNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClaimFileFilter.O.FileNumber", DefaultText = "File Number",LocalDefaultText = @"מס' תיק תפג", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimFileFilterONumeral = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClaimFileFilter.O.Numeral", DefaultText = "Numeral",LocalDefaultText = @"מס' רץ", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimFileFilterOClaimFileFilterGeneralData = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClaimFileFilter.O.ClaimFileFilterGeneralData", DefaultText = "General Data",LocalDefaultText = @"נתונים כלליים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimFileFilterOExternalId = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClaimFileFilter.O.ExternalId", DefaultText = "External",LocalDefaultText = @"יבואן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimFileFilterOExternalName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClaimFileFilter.O.ExternalName", DefaultText = "External Name",LocalDefaultText = @"שם היבואן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimFileFilterOCustomOfficeName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClaimFileFilter.O.CustomOfficeName", DefaultText = "Custom Office",LocalDefaultText = @"בית  מכס מטפל", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimFileFilterOOpenFileCounter = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClaimFileFilter.O.OpenFileCounter", DefaultText = "Open File Counter",LocalDefaultText = @"מונה תיקים פתוחים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimFileFilterOAgentName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClaimFileFilter.O.AgentName", DefaultText = "Agent Name",LocalDefaultText = @"שם הסוכן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimFileFilterOCloseFileCounter = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClaimFileFilter.O.CloseFileCounter", DefaultText = "Close File Counter",LocalDefaultText = @"מונה תיקים סגורים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimFileFilterOClaimFileFilterOpenFiles = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClaimFileFilter.O.ClaimFileFilterOpenFiles", DefaultText = "Open Files",LocalDefaultText = @"תיקים פתוחים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimFileFilterOClaimFileFilterClosedFiles = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClaimFileFilter.O.ClaimFileFilterClosedFiles", DefaultText = "Closed Files",LocalDefaultText = @"תיקים סגורים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimFileFilterOEntityTypeName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClaimFileFilter.O.EntityTypeName", DefaultText = "Entity Type",LocalDefaultText = @"ישות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimFileFilterOClaimEntityID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClaimFileFilter.O.ClaimEntityID", DefaultText = "Claim Entity",LocalDefaultText = @"מזהה ישות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimFileFilterOCreateDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClaimFileFilter.O.CreateDate", DefaultText = "Create Date",LocalDefaultText = @"תאריך פתיחה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimFileFilterOClaimAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClaimFileFilter.O.ClaimAmount", DefaultText = "Claim Amount",LocalDefaultText = @"סכום התביעה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimFileFilterOStatusName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClaimFileFilter.O.StatusName", DefaultText = "File Status",LocalDefaultText = @"סטטוס תיק", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimFileFilterOTotalComponentAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClaimFileFilter.O.TotalComponentAmount", DefaultText = "Total Component Amount",LocalDefaultText = @"סך החזר (קרן)", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimFileFilterOTotalRefundAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClaimFileFilter.O.TotalRefundAmount", DefaultText = "Total Refund Amount",LocalDefaultText = @"סך החזר (משוערך)", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimFileFilterOCloseDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClaimFileFilter.O.CloseDate", DefaultText = "Close Date",LocalDefaultText = @"תאריך סגירה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimFileFilterOSecondaryStatus = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClaimFileFilter.O.SecondaryStatus", DefaultText = "Secondary Status",LocalDefaultText = @"סיבת סגירה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimFileFilterOClaimFileFilterRefundList = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClaimFileFilter.O.ClaimFileFilterRefundList", DefaultText = "Refund List",LocalDefaultText = @"רשימת הוראות החזר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimFileFilterOPaymentOrderID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClaimFileFilter.O.PaymentOrderID", DefaultText = "Payment Order",LocalDefaultText = @"שם מוטב", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimFileFilterOPaymentProcessName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClaimFileFilter.O.PaymentProcessName", DefaultText = "Payment Process",LocalDefaultText = @"מספר ההוראה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimFileFilterOAmountSum = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClaimFileFilter.O.AmountSum", DefaultText = "Amount Sum",LocalDefaultText = @"סכום ההוראה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimFileFilterOValidityDateTo = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClaimFileFilter.O.ValidityDateTo", DefaultText = "Validity Date",LocalDefaultText = @"תאריך תוקף", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimFileFilterOPaymentOrderStatusName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClaimFileFilter.O.PaymentOrderStatusName", DefaultText = "Payment Order Status",LocalDefaultText = @"סטטוס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimFileFilterOClaimFileFilterRequiredDocuments = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClaimFileFilter.O.ClaimFileFilterRequiredDocuments", DefaultText = "Required Documents",LocalDefaultText = @"רשימת מסמכים נדרשים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimFileFilterODocumentCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClaimFileFilter.O.DocumentCode", DefaultText = "Document Code",LocalDefaultText = @"מזהה סוג מסמך", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimFileFilterOTypeName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClaimFileFilter.O.TypeName", DefaultText = "Type Name",LocalDefaultText = @"שם המסמך", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimFileFilterODisplayFileNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClaimFileFilter.O.DisplayFileNumber", DefaultText = "Display File",LocalDefaultText = @"תיק פרטני", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimFileFilterODocumentID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClaimFileFilter.O.DocumentID", DefaultText = "Document ID",LocalDefaultText = @"מזהה צרופה במכס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsAddAttachmentResponseOSendDocument = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.AddAttachmentResponse.O.SendDocument", DefaultText = "Send Document",LocalDefaultText = @"שליחת צרופה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsAddAttachmentResponseOSuccessMessgae = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.AddAttachmentResponse.O.SuccessMessgae", DefaultText = "Document was sent Successfully",LocalDefaultText = @"המסמך נשלח למכס בהצלחה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsAddAttachmentResponseODocumentNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.AddAttachmentResponse.O.DocumentNumber", DefaultText = "Document Number",LocalDefaultText = @"מספר מסמך", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsAddAttachmentResponseOCustomDocument = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.AddAttachmentResponse.O.CustomDocument", DefaultText = "Custom Document Number",LocalDefaultText = @"סימוכין מכס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsAddAttachmentResponseOCustomRecievedDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.AddAttachmentResponse.O.CustomRecievedDate", DefaultText = "Custom Recieved Date",LocalDefaultText = @"תאריך קליטה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsAddAttachmentResponseORemarks = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.AddAttachmentResponse.O.Remarks", DefaultText = "Remarks",LocalDefaultText = @"הערות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsAddAttachmentResponseOErrorMessgae = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.AddAttachmentResponse.O.ErrorMessgae", DefaultText = "Failed to send document",LocalDefaultText = @"שליחת המסמך למכס נכשלה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsAddAttachmentResponseOErrorCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.AddAttachmentResponse.O.ErrorCode", DefaultText = "Error code",LocalDefaultText = @"קוד השגיאה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsAddAttachmentResponseOErrorRemarks = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.AddAttachmentResponse.O.ErrorRemarks", DefaultText = "Error Remarks",LocalDefaultText = @"תיאור השגיאה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationStatusONoUpdate1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationStatus.O.NoUpdate1", DefaultText = "Declaration Status has not updated, Please send a request for Declaration reconstruction",LocalDefaultText = @"סטטוס ההצהרה לא עודכן , יש לבצע בקשה לשחזור נתוני הצהרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationOCorrections = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Corrections", DefaultText = "Corrections have been made for the Declaration, Declaration details have not been updated, Please send a request for Declaration reconstruction",LocalDefaultText = @"בוצעו תיקונים בהצהרה (תיקון הצהרה) נתוני ההצהרה לא עודכנו", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCustomsBookQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CustomsBookQuery", DefaultText = "Customs Book Update",LocalDefaultText = @"עדכון ספר סיווג", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralODocumentSizeLimit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.DocumentSizeLimit", DefaultText = "Document Size is above limit (200MB) , you must split it to smaller files",LocalDefaultText = "לא ניתן לקשר את המסמך מכיוון שהוא חורג מהגודל המותר (200 מ''ב) , יש לפצל תחילה את המסמך", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralONewClaim = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.NewClaim", DefaultText = "New Claim",LocalDefaultText = @"תביעה חדשה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCustomItemLegalDemandsQueryMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CustomItemLegalDemandsQueryMessage", DefaultText = "Custom Item Legal Demands Query Message",LocalDefaultText = @"דרישת חוקיות לפרט מכס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCustomItemLegalDemandsQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CustomItemLegalDemandsQuery", DefaultText = "Custom Item Legal Demands Query",LocalDefaultText = @"שאילתא לדרישת חוקיות לפרט מכס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCustomItemLegalDemandsQueryResults = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CustomItemLegalDemandsQueryResults", DefaultText = "Custom Item Legal Demands Results",LocalDefaultText = @"תוצאות שאילתא לדרישת חוקיות לפרט מכס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCustomItemLegalDemandsQueryOMAWBDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomItemLegalDemandsQuery.O.MAWBDetails", DefaultText = "MAWB Details",LocalDefaultText = @"נתוני שט''מ ראשי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCustomItemLegalDemandsQueryOIllegalImportData = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomItemLegalDemandsQuery.O.IllegalImportData", DefaultText = "Illegal Import Data",LocalDefaultText = @"נתוני חוקיות יבוא", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCustomItemLegalDemandsQueryOExclusionOfCountries = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomItemLegalDemandsQuery.O.ExclusionOfCountries", DefaultText = "Exclusion Of Countries",LocalDefaultText = @"החרגה של מדינות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCustomItemLegalDemandsQueryOCustomsBookType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomItemLegalDemandsQuery.O.CustomsBookType", DefaultText = "Customs Book Type",LocalDefaultText = @"סוג ספר מכס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCustomItemLegalDemandsQueryOCustomsBookTypeMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomItemLegalDemandsQuery.O.CustomsBookTypeMandatory", DefaultText = "Customs Book Type field is mandatory",LocalDefaultText = @"סוג ספר מכס הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCustomItemLegalDemandsQueryOClassificationCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomItemLegalDemandsQuery.O.ClassificationCode", DefaultText = "Classification",LocalDefaultText = @"פרט מכס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCustomItemLegalDemandsQueryOClassificationCodeMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomItemLegalDemandsQuery.O.ClassificationCodeMandatory", DefaultText = "Classification field is mandatory",LocalDefaultText = @"פרט מכס הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCustomItemLegalDemandsQueryOValidToDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomItemLegalDemandsQuery.O.ValidToDate", DefaultText = "Valid To Date",LocalDefaultText = @"נכון לתאריך", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCustomItemLegalDemandsQueryOValidToDateMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomItemLegalDemandsQuery.O.ValidToDateMandatory", DefaultText = "Valid To Date field is mandatory",LocalDefaultText = @"נכון לתאריך הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCustomItemLegalDemandsQueryORequirementSource = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomItemLegalDemandsQuery.O.RequirementSource", DefaultText = "RequirementSource",LocalDefaultText = @"מקור חוקי לדרישה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCustomItemLegalDemandsQueryOFullClassification = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomItemLegalDemandsQuery.O.FullClassification", DefaultText = "FullClassification",LocalDefaultText = @"נובע מפרט/ פרט", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCustomItemLegalDemandsQueryORequirementGoodsDescription = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomItemLegalDemandsQuery.O.RequirementGoodsDescription", DefaultText = "Requirement Goods Description",LocalDefaultText = @"תאור טובין בדרישה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCustomItemLegalDemandsQueryOAuthoritiyName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomItemLegalDemandsQuery.O.AuthoritiyName", DefaultText = "Authoritiy",LocalDefaultText = @"גורם מאשר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCustomItemLegalDemandsQueryOCertificateTypeName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomItemLegalDemandsQuery.O.CertificateTypeName", DefaultText = "Certificate Type",LocalDefaultText = @"סוג אישור", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCustomItemLegalDemandsQueryOTextualCondition = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomItemLegalDemandsQuery.O.TextualCondition", DefaultText = "Textual Condition",LocalDefaultText = @"תיאור תנאים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCustomItemLegalDemandsQueryOInterConditionsRelationshipName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomItemLegalDemandsQuery.O.InterConditionsRelationshipName", DefaultText = "Conditions Relationship",LocalDefaultText = @"יחס תנאים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCustomItemLegalDemandsQueryOIsPersonalImportIncluded = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomItemLegalDemandsQuery.O.IsPersonalImportIncluded", DefaultText = "Is Personal Import Included",LocalDefaultText = @"חל ביבוא אישי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCustomItemLegalDemandsQueryOIsCarnetIncluded = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomItemLegalDemandsQuery.O.IsCarnetIncluded", DefaultText = "Is Carnet Included",LocalDefaultText = @"חל בקרנה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCustomItemLegalDemandsQueryOCountryName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomItemLegalDemandsQuery.O.CountryName", DefaultText = "Country",LocalDefaultText = @"מדינה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsMasterBOLQueryOReturnAllInernalCargos = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.MasterBOLQuery.O.ReturnAllInernalCargos", DefaultText = "Return All Inernal Cargos",LocalDefaultText = @"הצג את כל הפנימיים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsMasterBOLQueryOSubmitter = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.MasterBOLQuery.O.Submitter", DefaultText = "Submitter",LocalDefaultText = @"גורם מדווח", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsMasterBOLQueryOExactMatch = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.MasterBOLQuery.O.ExactMatch", DefaultText = "Exact Match",LocalDefaultText = @"התאמה מדוייקת", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationOCancelled = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Cancelled", DefaultText = "Display Only - Declaration was cancelled",LocalDefaultText = @"הנתונים לתצוגה בלבד – ההצהרה מבוטלת", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsRequiredDocumentResponseOConnectedEntities = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.RequiredDocumentResponse.O.ConnectedEntities", DefaultText = "Connected Entities",LocalDefaultText = @"ישויות קשורות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsRequiredDocumentResponseODocumentTypeName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.RequiredDocumentResponse.O.DocumentTypeName", DefaultText = "Required Document Type",LocalDefaultText = @"סוג מסמך נדרש", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsRequiredDocumentResponseODocumentWorkerName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.RequiredDocumentResponse.O.DocumentWorkerName", DefaultText = "Worker Name",LocalDefaultText = @"שם עובד המכס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsRequiredDocumentResponseOCustomRemarks = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.RequiredDocumentResponse.O.CustomRemarks", DefaultText = "Custom Remarks",LocalDefaultText = @"הערות המכס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCreditQueryResultsBankAccount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CreditQueryResultsBankAccount", DefaultText = "Bank Account Results",LocalDefaultText = @"פירוט חשבונות בנק", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCreditQueryResultsOBankAccount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CreditQueryResults.O.BankAccount", DefaultText = "Bank Account",LocalDefaultText = @"מספר חשבון", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCreditQueryResultsOUsedBalanceForBankAccount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CreditQueryResults.O.UsedBalanceForBankAccount", DefaultText = "Used Balance For Bank Account",LocalDefaultText = @"ניצול יומי מחשבון", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsWarehouseBlockBalanceOSiteText = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.WarehouseBlockBalance.O.SiteText", DefaultText = "Warehouse",LocalDefaultText = @"מחסן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsWarehouseBlockBalanceOMaxStorageDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.WarehouseBlockBalance.O.MaxStorageDate", DefaultText = "MaxStorage Date",LocalDefaultText = @"תאריך אחרון לאחסון", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsWarehouseBlockBalanceOBlockClosureDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.WarehouseBlockBalance.O.BlockClosureDate", DefaultText = "BlockClosure Date",LocalDefaultText = @"תאריך סגירת גוש במחסן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsNewPaymentOrderOPaymentNumberMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.NewPaymentOrder.O.PaymentNumberMandatory", DefaultText = "Payment Number field is mandatory",LocalDefaultText = @"מספר הוראת תשלום הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsExportDeclarationDataQueryFExportDeclarationData = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ExportDeclarationDataQuery.F.ExportDeclarationData", DefaultText = "Export Declaration Data",LocalDefaultText = @"נתוני הצהרת יצוא", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsExportDeclarationDataQueryFReshimonNubmer = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ExportDeclarationDataQuery.F.ReshimonNubmer", DefaultText = "Reshimon Nubmer",LocalDefaultText = @"מספר רשימון יצוא", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsExportDeclarationDataQueryFDeclarationData = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ExportDeclarationDataQuery.F.DeclarationData", DefaultText = "Declaration Data",LocalDefaultText = @"פרטי הצהרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsExportDeclarationDataQueryFDeclarationNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ExportDeclarationDataQuery.F.DeclarationNumber", DefaultText = "Declaration Number",LocalDefaultText = @"מספר הצהרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsExportDeclarationDataQueryFCalculationDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ExportDeclarationDataQuery.F.CalculationDate", DefaultText = "Calculation Date",LocalDefaultText = @"תאריך חישוב", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsExportDeclarationDataQueryFFOBNetoNISAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ExportDeclarationDataQuery.F.FOBNetoNISAmount ", DefaultText = "FOB Neto NIS Amount ",LocalDefaultText = @"ערך נטו בש''ח", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsExportDeclarationDataQueryFAgentCustomerExternalID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ExportDeclarationDataQuery.F.AgentCustomerExternalID", DefaultText = "Agent External ID",LocalDefaultText = @"מזהה חיצוני של סוכן המכס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsExportDeclarationDataQueryFLoadingDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ExportDeclarationDataQuery.F.LoadingDate", DefaultText = "Loading Date",LocalDefaultText = @"תאריך טעינה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsExportDeclarationDataQueryFFOBNISAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ExportDeclarationDataQuery.F.FOBNISAmount ", DefaultText = "FOB NIS Amount ",LocalDefaultText = @"ערך FOB בש''ח", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsExportDeclarationDataQueryFInvoiceData = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ExportDeclarationDataQuery.F.InvoiceData", DefaultText = "Invoice Data",LocalDefaultText = @"נתוני חשבון", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsExportDeclarationDataQueryFSequenceNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ExportDeclarationDataQuery.F.SequenceNumber", DefaultText = "Sequence Number",LocalDefaultText = @"סידורי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsExportDeclarationDataQueryFExternalID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ExportDeclarationDataQuery.F.ExternalID", DefaultText = "External ID",LocalDefaultText = @"מספר חשבון", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsExportDeclarationDataQueryFInvoiceAmountCurrency = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ExportDeclarationDataQuery.F.InvoiceAmountCurrency", DefaultText = "Invoice Amount+Currency",LocalDefaultText = @"ערך חשבון (מטבע)", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsExportDeclarationDataQueryFGoodsItemData = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ExportDeclarationDataQuery.F.GoodsItemData", DefaultText = "Goods Item Data",LocalDefaultText = @"נתוני סחורות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsExportDeclarationDataQueryFCustomsItem = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ExportDeclarationDataQuery.F.CustomsItem", DefaultText = "Customs Item",LocalDefaultText = @"פרטי מכס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsExportDeclarationDataQueryFValueQuantity = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ExportDeclarationDataQuery.F.ValueQuantity", DefaultText = "Value Quantity",LocalDefaultText = @"כמות סטטיסטית", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsExportDeclarationDataQueryFForeignCurrencyAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ExportDeclarationDataQuery.F.ForeignCurrencyAmount", DefaultText = "Foreign Amount+Currency",LocalDefaultText = @"מחיר בסיס (מטבע)", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsExportDeclarationDataQueryFOriginCountry = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ExportDeclarationDataQuery.F.OriginCountry", DefaultText = "Origin Country",LocalDefaultText = @"ארץ מקור", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsExportDeclarationDataQueryFGovernmentProcedure = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ExportDeclarationDataQuery.F.GovernmentProcedure", DefaultText = "Government Procedure",LocalDefaultText = @"תהליך ברמת הסחורה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsExportDeclarationDataQueryFVehicleExternalID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ExportDeclarationDataQuery.F.VehicleExternalID", DefaultText = "Vehicle External",LocalDefaultText = @"מספר זיהוי רכב", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsExportDeclarationDataQueryFCargoIdentityQualifierID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ExportDeclarationDataQuery.F.CargoIdentityQualifierID", DefaultText = "Cargo Identity Qualifier",LocalDefaultText = @"סוג זיהוי רכב", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsExportDeclarationDataQueryFRichbitNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ExportDeclarationDataQuery.F.RichbitNumber", DefaultText = "Richbit Number",LocalDefaultText = @"מספר שילדה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsExportDeclarationDataQueryOReshimonNubmerMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ExportDeclarationDataQuery.O.ReshimonNubmerMandatory", DefaultText = "Reshimon Nubmer field is mandatory",LocalDefaultText = @"מספר רשימון יצוא הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOExportDeclarationDataQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.ExportDeclarationDataQuery", DefaultText = "Export Declaration Data Query",LocalDefaultText = @"שאילתא להצהרה יצוא", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralODeposition = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.Deposition", DefaultText = "Deposition",LocalDefaultText = @"תצהיר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationODuplicateInvoiceNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.DuplicateInvoiceNumber", DefaultText = "Invoice number already exists",LocalDefaultText = @"מספר החשבון כבר קיים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCreditGoldQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CreditGoldQuery", DefaultText = "Credit Gold Query",LocalDefaultText = @"שאילתא לתקרת זהב", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCreditGoldQueryOCreationDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CreditGoldQuery.O.CreationDate", DefaultText = "Creation Date",LocalDefaultText = @"תאריך הקמה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCreditGoldQueryORTGSDeposits = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CreditGoldQuery.O.RTGSDeposits", DefaultText = "RTGS Deposits",LocalDefaultText = @"הפקדות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCreditGoldQueryORTGSUsed = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CreditGoldQuery.O.RTGSUsed", DefaultText = "RTGS Used",LocalDefaultText = @"ניצולים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCreditGoldQueryORTGSCurrentBalance = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CreditGoldQuery.O.RTGSCurrentBalance", DefaultText = "RTGS Current Balance",LocalDefaultText = @"יתרה נוכחית", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCreditGoldQueryOActiveInd = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CreditGoldQuery.O.ActiveInd", DefaultText = "ActiveInd",LocalDefaultText = @"האם פעיל", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCreditGoldQueryOTransactionType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CreditGoldQuery.O.TransactionType", DefaultText = "Transaction Type",LocalDefaultText = @"תנועה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCreditGoldQueryOPaymentID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CreditGoldQuery.O.PaymentID", DefaultText = "Payment",LocalDefaultText = @"הוראת תשלום", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCreditGoldQueryOPaymentStatus = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CreditGoldQuery.O.PaymentStatus", DefaultText = "Payment Status",LocalDefaultText = @"סטטוס הוראה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCreditGoldQueryOPaymentDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CreditGoldQuery.O.PaymentDate", DefaultText = "Payment Date",LocalDefaultText = @"תאריך תשלום", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCreditGoldQueryOEntityType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CreditGoldQuery.O.EntityType", DefaultText = "Entity Type",LocalDefaultText = @"סוג ישות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCreditGoldQueryOEntityID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CreditGoldQuery.O.EntityID", DefaultText = "Entity",LocalDefaultText = @"מספר ישות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCreditGoldQueryOTransactionAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CreditGoldQuery.O.TransactionAmount", DefaultText = "Transaction Amount",LocalDefaultText = @"סכום", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCreditGoldQueryORTGSBalance = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CreditGoldQuery.O.RTGSBalance", DefaultText = "RTGS Balance",LocalDefaultText = @"יתרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCreditGoldQueryORTGSRefund = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CreditGoldQuery.O.RTGSRefund", DefaultText = "RTGS Refund",LocalDefaultText = @"החזרים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCreditGoldQueryOUpdateUser = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CreditGoldQuery.O.UpdateUser", DefaultText = "Update User",LocalDefaultText = @"משתמש מעדכן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCreditGoldQueryOTransactionResult = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CreditGoldQuery.O.TransactionResult", DefaultText = "Transaction Result",LocalDefaultText = @"פירוט תנועות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsExportDeclarationDataQueryFInvoiceAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ExportDeclarationDataQuery.F.InvoiceAmount", DefaultText = "Invoice Amount",LocalDefaultText = @"ערך חשבון", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsExportDeclarationDataQueryFForeignAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ExportDeclarationDataQuery.F.ForeignAmount", DefaultText = "Foreign Amount",LocalDefaultText = @"מחיר בסיס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationStatusQueryOHandeledWroker = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationStatusQuery.O.HandeledWroker", DefaultText = "Handeled Wroker",LocalDefaultText = @"מעריך מטפל", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOClientSearchByIDQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.ClientSearchByIDQuery", DefaultText = "Client Search Query",LocalDefaultText = @"נתונים נוספים ליבואן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClientSearchByIDQueryOAgentAuthorization = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClientSearchByIDQuery.O.AgentAuthorization", DefaultText = "Agent Authorization List",LocalDefaultText = @"רשימת כתבי הרשאה שניתנו לסוכן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClientSearchByIDQueryOClientAuthorization = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClientSearchByIDQuery.O.ClientAuthorization", DefaultText = "Client Authorization List",LocalDefaultText = @"רשימת כתבי הרשאה שהתקבלו מהלקוח", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClientSearchByIDQueryOIndicationPerClassification = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClientSearchByIDQuery.O.IndicationPerClassification", DefaultText = "Indication Per Classification",LocalDefaultText = @"אינדיקציות לפרט מכס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClientSearchByIDQueryOExportRequest = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClientSearchByIDQuery.O.ExportRequest", DefaultText = "Export Request",LocalDefaultText = @"בקשות יצואן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClientSearchByIDQueryOPoaID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClientSearchByIDQuery.O.PoaID", DefaultText = "POA ID",LocalDefaultText = @"מס' כתב הרשאה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClientSearchByIDQueryOAuthorizedName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClientSearchByIDQuery.O.AuthorizedName", DefaultText = "Authorized",LocalDefaultText = @"שם המורשה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClientSearchByIDQueryOCustomerActivityTypeName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClientSearchByIDQuery.O.CustomerActivityTypeName", DefaultText = "Customer Activity Type",LocalDefaultText = @"סוג הרשאה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClientSearchByIDQueryOPoaStatusName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClientSearchByIDQuery.O.PoaStatusName", DefaultText = "Poa Status",LocalDefaultText = @"סטטוס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClientSearchByIDQueryOPoaAuthorizationTypeName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClientSearchByIDQuery.O.PoaAuthorizationTypeName", DefaultText = "Poa Authorization Type",LocalDefaultText = @"סוג הרשאה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClientSearchByIDQueryOStartDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClientSearchByIDQuery.O.StartDate", DefaultText = "Start Date",LocalDefaultText = @"תחילת תוקף", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClientSearchByIDQueryOEndDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClientSearchByIDQuery.O.EndDate", DefaultText = "End Date",LocalDefaultText = @"סיום תוקף", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClientSearchByIDQueryOIndicationPerClassificationTypeName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClientSearchByIDQuery.O.IndicationPerClassificationTypeName", DefaultText = "IndicationPer Classification Type",LocalDefaultText = @"סוג פטור", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClientSearchByIDQueryOClassificationID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClientSearchByIDQuery.O.ClassificationID", DefaultText = "Classification ID",LocalDefaultText = @"פרט מכס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClientSearchByIDQueryOGoodsItemDescription = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClientSearchByIDQuery.O.GoodsItemDescription", DefaultText = "Goods Item Description",LocalDefaultText = @"תיאור טובין", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClientSearchByIDQueryORequestID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClientSearchByIDQuery.O.RequestID", DefaultText = "Request ID",LocalDefaultText = @"מספר בקשה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClientSearchByIDQueryORequestTypeName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClientSearchByIDQuery.O.RequestTypeName", DefaultText = "Request Type",LocalDefaultText = @"סוג בקשה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClientSearchByIDQueryOCreateDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClientSearchByIDQuery.O.CreateDate", DefaultText = "Create Date",LocalDefaultText = @"תאריך יצירה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClientSearchByIDQueryOStationName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClientSearchByIDQuery.O.StationName", DefaultText = "Station",LocalDefaultText = @"תחנת מכס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClientSearchByIDQueryOApprovementStartDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClientSearchByIDQuery.O.ApprovementStartDate", DefaultText = "Approvement Start Date",LocalDefaultText = @"תאריך תחילת אישור", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClientSearchByIDQueryOApprovementEndDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ClientSearchByIDQuery.O.ApprovementEndDate", DefaultText = "Approvement End Date",LocalDefaultText = @"תאריך סיום אישור", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationOpaymentDateSmallerThanFuture = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.paymentDateSmallerThanFuture", DefaultText = "The payment date is smaller than the future payment date, continue?",LocalDefaultText = @"תאריך הגשה קטן מתאריך הגשה עתידית האם להמשיך ?", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCourierAlreadyExist = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CourierAlreadyExist", DefaultText = "There is already master courier with the same values",LocalDefaultText = @"קיים בלדר ראשי עם נתונים זהים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOWeightValue = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.WeightValue", DefaultText = "Payment Terms",LocalDefaultText = @"תנאי תשלום", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralODocumetsMandatoryTicket = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.DocumetsMandatoryTicket", DefaultText = "Some ticket are marked as mandatory for send , continue ?",LocalDefaultText = @"קיימים טיקטים שלא נשלחו ומוגדרים כחובה לשליחה, האם להמשיך ?", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOFreightIncotermMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.FreightIncotermMandatory", DefaultText = "Freight amount was entered and it not matches to incoterm code , continue ?",LocalDefaultText = @"קיימים נתוני ערך הובלה אך תנאי המכר בתיק אינם דורשים זאת , להמשיך ?", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsReleaseGoodsOGeneralData = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ReleaseGoods.O.GeneralData", DefaultText = "General Data",LocalDefaultText = @"מידע כללי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsReleaseGoodsOProcessType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ReleaseGoods.O.ProcessType", DefaultText = "Process Type",LocalDefaultText = @"סוג תהליך", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsReleaseGoodsOTotalDealValue = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ReleaseGoods.O.TotalDealValue", DefaultText = "Total Deal Value",LocalDefaultText = @"סה”כ ערך עסקה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsReleaseGoodsOTotalCIFValue = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ReleaseGoods.O.TotalCIFValue", DefaultText = "Total CIF Value",LocalDefaultText = @"סה”כ ערך סיף", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsReleaseGoodsOCurrencyTypeCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ReleaseGoods.O.CurrencyTypeCode", DefaultText = "Currency Type Code",LocalDefaultText = @"קוד מטבע", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsReleaseGoodsOExchangeRate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ReleaseGoods.O.ExchangeRate", DefaultText = "Exchange Rate",LocalDefaultText = @"שער מטבע", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsReleaseGoodsOClient = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ReleaseGoods.O.Client", DefaultText = "Client",LocalDefaultText = @"לקוח", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsReleaseGoodsOLoadingPort = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ReleaseGoods.O.LoadingPort", DefaultText = "Loading Port",LocalDefaultText = @"נמל טעינה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsReleaseGoodsOGoodsItemPath = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ReleaseGoods.O.GoodsItemPath", DefaultText = "Goods Item Path",LocalDefaultText = @"שורת פרט המכס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsReleaseGoodsOGoodsItem = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ReleaseGoods.O.GoodsItem", DefaultText = "Goods Item",LocalDefaultText = @"פרט המכס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsReleaseGoodsOGoodsItems = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ReleaseGoods.O.GoodsItems", DefaultText = "Goods Items",LocalDefaultText = @"פרטי המכס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsReleaseGoodsOGoodsDescription = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ReleaseGoods.O.GoodsDescription", DefaultText = "Goods Description",LocalDefaultText = @"תאור טובין", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsReleaseGoodsOUnloadingPort = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ReleaseGoods.O.UnloadingPort", DefaultText = "Unloading Port",LocalDefaultText = @"נמל פריקה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsReleaseGoodsOStorageSite = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.ReleaseGoods.O.StorageSite", DefaultText = "Storage Site",LocalDefaultText = @"אתר אחסון", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsBankAccountToRefundQueryOFileNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.BankAccountToRefundQuery.O.FileNumber", DefaultText = "File Number",LocalDefaultText = @"מספר תיק תפ''ג", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsBankAccountToRefundQueryONumeral = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.BankAccountToRefundQuery.O.Numeral", DefaultText = "Numeral",LocalDefaultText = @"מספר רץ תפ''ג", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsBankAccountToRefundQueryOIdentifierType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.BankAccountToRefundQuery.O.IdentifierType", DefaultText = "Identifier Type",LocalDefaultText = @"סוג מוטב להחזר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsBankAccountToRefundQueryOIdentifierCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.BankAccountToRefundQuery.O.IdentifierCode", DefaultText = "Identifier Code",LocalDefaultText = @"מספר מוטב להחזר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsBankAccountToRefundQueryOCountryCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.BankAccountToRefundQuery.O.CountryCode", DefaultText = "Country Code",LocalDefaultText = @"ארץ חשבון בנק", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsBankAccountToRefundQueryOBankCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.BankAccountToRefundQuery.O.BankCode", DefaultText = "Bank Code",LocalDefaultText = @"מספר בנק", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsBankAccountToRefundQueryOBankBranch = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.BankAccountToRefundQuery.O.BankBranch", DefaultText = "Branch Code",LocalDefaultText = @"מספר סניף", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsBankAccountToRefundQueryOAccountNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.BankAccountToRefundQuery.O.AccountNumber", DefaultText = "Account Number",LocalDefaultText = @"מספר חשבון", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsBankAccountToRefundQueryOAccountCurrency = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.BankAccountToRefundQuery.O.AccountCurrency", DefaultText = "Account Currency",LocalDefaultText = @"מטבע לניהול החשבון", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsBankAccountToRefundQueryOAccountCurrencyMandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.BankAccountToRefundQuery.O.AccountCurrencyMandatory", DefaultText = "Account Currency field is mandatory",LocalDefaultText = @"מטבע לניהול החשבון הוא שדה חובה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationOClosed = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.Closed", DefaultText = "Display Only - Declaration was closed",LocalDefaultText = @"הנתונים לתצוגה בלבד – ההצהרה סגורה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsWarehouseBlockBalanceOStorageEntryPortChargeBalance = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.WarehouseBlockBalance.O.StorageEntryPortChargeBalance", DefaultText = "Port Charge Balance",LocalDefaultText = @"יתרת אגרת נמל", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsWarehouseBlockBalanceOStorageEntryTransportBalance = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.WarehouseBlockBalance.O.StorageEntryTransportBalance", DefaultText = "Transport Balance",LocalDefaultText = @"יתרת הובלה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsWarehouseBlockBalanceOStorageEntryInsuranceBalance = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.WarehouseBlockBalance.O.StorageEntryInsuranceBalance", DefaultText = "Insurance Balance",LocalDefaultText = @"יתרת ביטוח", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsWarehouseBlockBalanceOCargoMovementReference = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.WarehouseBlockBalance.O.CargoMovementReference", DefaultText = "Cargo Movement Reference",LocalDefaultText = @"מס' אסמכתא", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralBSaveAndNew = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.B.SaveAndNew", DefaultText = "Save And New",LocalDefaultText = @"שמירה וחדש", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOAll = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.All", DefaultText = "All",LocalDefaultText = @"הכל", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralRequestInProgress = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.RequestInProgress", DefaultText = "There is a request ({0}) in progress , can't continue  until its finished ",LocalDefaultText = @"קיימת בקשה בתהליך ({0}) יש לבטל את הבקשה או להמתין לסיום הטיפול בה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOPreSendValidations = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.PreSendValidations", DefaultText = "Validations before sending request",LocalDefaultText = @"בדיקות לפני שליחת מסר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CommunicationLogStepsOCommunicationLogSteps = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CommunicationLogSteps.O.CommunicationLogSteps", DefaultText = "Communication Log Steps",LocalDefaultText = @"תקשורת התחבר צעדים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationTHDocuments = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.TH.Documents", DefaultText = "Documents",LocalDefaultText = @"מסמכים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsRequestSheetORequestCreateDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.RequestSheet.O.RequestCreateDate", DefaultText = "To Date shouldn't be smaller than From Date",LocalDefaultText = "הערך בשדה ''עד תאריך'' צריך להיות קטן או שווה לערך בשדה ''מ תאריך", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCustomsVendorTHEvents = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsVendor.TH.Events", DefaultText = "Events",LocalDefaultText = @"אירועים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCustomsVendorTHCommunications = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsVendor.TH.Communications", DefaultText = "Communications",LocalDefaultText = @"תקשורת", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSupplierInvioceItemsCertificate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SupplierInvioceItemsCertificate", DefaultText = "Supplier Invioce Items Certificate",LocalDefaultText = null, ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationTHPayments = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.TH.Payments", DefaultText = "Declaration Payment",LocalDefaultText = @"הגשת תשלום", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CommunicationLogStepsOStepNumber = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CommunicationLogSteps.O.StepNumber", DefaultText = "Step Number",LocalDefaultText = @"שלב מספר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_GeneralMCCustomsCustomsSignStation = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Customs.CustomsSignStation", DefaultText = "Sign Stations",LocalDefaultText = @"עמדות חתימה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCouriersVatTHEvents = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CouriersVat.TH.Events", DefaultText = "Events",LocalDefaultText = @"אירועים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOInterfaceManagement = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.InterfaceManagement", DefaultText = "Interface Managements",LocalDefaultText = @"ממשק ניהול", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsInterfaceManagementOSystemDefinitions = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.InterfaceManagement.O.SystemDefinitions", DefaultText = "System Definitions",LocalDefaultText = @"הגדרות מערכת", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsSupplierInvoiceItemsQuantity = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.SupplierInvoiceItemsQuantity", DefaultText = "Supplier Invoice Items Quantity",LocalDefaultText = null, ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsCustomsItemsFSearchFields = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.CustomsItems.F.SearchFields", DefaultText = "SearchFields",LocalDefaultText = null, ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_tableFSearchFields = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "table.F.SearchFields", DefaultText = "SearchFields",LocalDefaultText = null, ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_GeneralMCCustomsDocumentsDefinition = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Customs.DocumentsDefinition", DefaultText = "Documents Definition",LocalDefaultText = @"הגדרת סוגי מסמך למסך צרופות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimTHRefundIsraelBankDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Claim.TH.RefundIsraelBankDetails", DefaultText = "Local Bank Details",LocalDefaultText = @"פרטי בנק ישראלי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimTHRefundForeignBankDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Claim.TH.RefundForeignBankDetails", DefaultText = "Foreign Bank Details",LocalDefaultText = @"פרטי בנק זר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimTHCancelOrObjection = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Claim.TH.CancelOrObjection", DefaultText = "Cancel Or Objection",LocalDefaultText = @"ביטול תביעה/ערר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimORelatedEntites = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Claim.O.RelatedEntites", DefaultText = "Related Entites",LocalDefaultText = @"ישויות תביעה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralBNo = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.B.No", DefaultText = "No",LocalDefaultText = @"לא", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOSpecialActivityRequestSample = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.SpecialActivityRequestSample", DefaultText = "Special Activity Request Sample",LocalDefaultText = @"מדגם לבקשת פעילות מיוחדת", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_GeneralMHCustomsCollateral = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.CustomsCollateral", DefaultText = "Customs Collateral",LocalDefaultText = @"בטוחות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_GeneralMHReferantScreen = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.ReferantScreen", DefaultText = "Referant Screen",LocalDefaultText = @"מסך רפרנט", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsVehicleOVehicleWasUsed = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Vehicle.O.VehicleWasUsed", DefaultText = "This vehicle was already used in Custom File",LocalDefaultText = @"רכב זה נמצא בתיק עמילות מספר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CommunicationLogStepsOName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CommunicationLogSteps.O.Name", DefaultText = "Name",LocalDefaultText = @"שם", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CommunicationLogStepsODuration = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CommunicationLogSteps.O.Duration", DefaultText = "Duration",LocalDefaultText = @"משך", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CommunicationLogStepsOStatus = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CommunicationLogSteps.O.Status", DefaultText = "Status",LocalDefaultText = @"סטטוס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CommunicationLogStepsORetries = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CommunicationLogSteps.O.Retries", DefaultText = "Retries",LocalDefaultText = @"ניסיונות חוזרים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CommunicationLogStepsOStartDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CommunicationLogSteps.O.StartDate", DefaultText = "Start Date",LocalDefaultText = @"תאריך ההתחלה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CommunicationLogStepsOEndDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "CommunicationLogSteps.O.EndDate", DefaultText = "End Date",LocalDefaultText = @"תאריך סיום", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimTHReasonsAndExplanitaions = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Claim.TH.ReasonsAndExplanitaions", DefaultText = "Reasons And Explanitaions",LocalDefaultText = @"סיבות ונימוקים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsClaimTHClaimDecision = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Claim.TH.ClaimDecision", DefaultText = "Claim Decision",LocalDefaultText = @"החלטת המכס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsVehicleOVehicaleOwner = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Vehicle.O.VehicaleOwner", DefaultText = "Vehicle Owners",LocalDefaultText = @"בעלים רכב", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_GeneralOConnectedDeclarations = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.ConnectedDeclarations", DefaultText = "Connected Declarations",LocalDefaultText = @"הצהרות מקושרות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationOConsignmentPackagesDanger = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.Declaration.O.ConsignmentPackagesDanger", DefaultText = "Consignment Packages Danger",LocalDefaultText = @"חבילות משלוחים סכנה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_GeneralOAddresses = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Addresses", DefaultText = "Addresses",LocalDefaultText = @"כתובות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_GeneralODrivingLicense = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DrivingLicense", DefaultText = "Driving License",LocalDefaultText = @"רשיון נהיגה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_GeneralOEvents = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Events", DefaultText = "Events",LocalDefaultText = @"אירועים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_GeneralORequestSheets = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.RequestSheets", DefaultText = "Request Sheets",LocalDefaultText = @"גיליון בקשה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralONewCustomsFile = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.NewCustomsFile", DefaultText = "New Customs File",LocalDefaultText = @"פתיחת תיק חדש", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationReferantDataOFollowUpDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationReferantData.O.FollowUpDate", DefaultText = "FollowUpDate",LocalDefaultText = @"תאריך מעקב", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationReferantDataOExceptionReasonsCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationReferantData.O.ExceptionReasonsCode ", DefaultText = "ExceptionReasonsCode ",LocalDefaultText = @"קוד חריג", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationReferantDataOExceptionRemarks = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationReferantData.O.ExceptionRemarks ", DefaultText = "ExceptionRemarks ",LocalDefaultText = @"הערות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationReferantDataOStatus = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationReferantData.O.Status ", DefaultText = "Status ",LocalDefaultText = @"סטטוס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_GeneralMCCustomsReAnalysis = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Customs.ReAnalysis", DefaultText = "Re-Analysis",LocalDefaultText = @"ניתוח בקשות גורף", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_GeneralMCCustomsRecallClientsForCutoms = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Customs.RecallClientsForCutoms", DefaultText = "Update Importers Data",LocalDefaultText = @"עדכון נתוני יבואנים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsInterfaceManagementOTenantDefinitions = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.InterfaceManagement.O.TenantDefinitions", DefaultText = "Tenant Definitions",LocalDefaultText = @"הגדרות סביבה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "TH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_GeneralONotConnectedDeclarations = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.NotConnectedDeclarations", DefaultText = "Not Connected Declarations",LocalDefaultText = @"הצהרות לא מקושרות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOInAutomaticPayment = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.InAutomaticPayment", DefaultText = "Declaration in automatic payment process.",LocalDefaultText = @"הצהרה בתהליך תשלום אוטומטי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 