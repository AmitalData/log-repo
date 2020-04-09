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
   public class CustomsGeneralUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "CustomsGeneral",
			      				    IsNew =  false,
			      				    DBTableName =  "CustomsGenerals",
			      				    OldDBTableName =  "CustomsGenerals",
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
			      				    Code =  "1173",
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
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         
			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Id",
					  						OldFieldName =  "Id",
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
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 

			   AddObjectsAndObjectFields.AddObjectField(new ObjectFieldsDetails() 
			   {
					 
					 						FieldName =  "Tenant",
					  						OldFieldName =  "Tenant",
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
					  		
			   },TextCodeRepository,ObjectFieldsRepository,objectFields,textCodes);
 
	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
	    {  	   
	    }

	    public void AddTableScreens(Dictionary<string, Screen> tenantScreens,Dictionary<string, ScreenField> tenantScreenFields, ScreensRepository screensRepository, ScreenFieldsRepository screenFieldsRepository,IWebFreightContext ObjectContext)
	    {   

		   ObjectTable CustomsGeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "CustomsGeneral" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> CustomsGeneralObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "CustomsGeneral").ToList();
		       
	      

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

 		   TextCode CustomsGeneralTextCode_CustomsGeneralODeclarationRestoreQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.DeclarationRestoreQuery", DefaultText = "Declaration restore Query",LocalDefaultText = @"שאילתא לשחזור נתוני הצהרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOMorningMessageQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.MorningMessageQuery", DefaultText = "Morning Message Query",LocalDefaultText = @"שאילתא להודעות בוקר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCourierBOLQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CourierBOLQuery", DefaultText = "Courier BOL Query",LocalDefaultText = @"שאילתא לשטרי מטען בלדר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOGuaranteeCertificateFilterQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.GuaranteeCertificateFilterQuery", DefaultText = "Guarantee Certificate Filter Query",LocalDefaultText = @"שאילתא לנתוני כתב ערבות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOFaultQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.FaultQuery", DefaultText = "Faults Query",LocalDefaultText = @"שאילתא לליקויים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOWarehouseBlockBalance = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.WarehouseBlockBalance", DefaultText = "Warehouse Block Balance",LocalDefaultText = @"יתרות מלאי בגוש", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOMasterBOLQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.MasterBOLQuery", DefaultText = "Master BOL Query",LocalDefaultText = @"שאילתא לשטרי מטען", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCurrencyExchangeRateQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CurrencyExchangeRateQuery", DefaultText = "Exchange Rate Query",LocalDefaultText = @"שאילתא לשערים", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCustomItemLegalDemandsQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CustomItemLegalDemandsQuery", DefaultText = "Custom Item Legal Demands Query",LocalDefaultText = @"שאילתא לדרישת חוקיות לפרט מכס", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralODeclarationPrintQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.DeclarationPrintQuery", DefaultText = "Declaration Print Query",LocalDefaultText = @"שאילתא להדפסת הצהרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOClientSearchByIDQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.ClientSearchByIDQuery", DefaultText = "Client Search Query",LocalDefaultText = @"נתונים נוספים ליבואן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOBlockListInWarehouseQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.BlockListInWarehouseQuery", DefaultText = "Block List in Warehouse Query",LocalDefaultText = @"שאילתת גושים במחסן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCargoQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CargoQuery", DefaultText = "Manifest Status Query",LocalDefaultText = @"שאילתא למצהר", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOImporterDeclarationQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.ImporterDeclarationQuery", DefaultText = "Importer Declaration Query",LocalDefaultText = @"שאילתא לתצהיר יבואן", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOBankAccountToRefundQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.BankAccountToRefundQuery", DefaultText = "Bank Account To Refund Query",LocalDefaultText = @"שאילתא לבקשת החזר פיקדון", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralODeficitFileFilterQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.DeficitFileFilterQuery", DefaultText = "Deficit File Filter Query",LocalDefaultText = @"שאילתא לגרעונות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOMasavPaymentsToAgentQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.MasavPaymentsToAgentQuery", DefaultText = "Masav Payments To Agent Query",LocalDefaultText = @"שאילתא לבקשת דוח קופה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOGuaranteeFileFilterQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.GuaranteeFileFilterQuery", DefaultText = "Guarantee File Filter Query",LocalDefaultText = @"שאילתא לערבויות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralODeclarationFilterQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.DeclarationFilterQuery", DefaultText = "Declaration filter Query",LocalDefaultText = "שאילתא לתיקי תפ''ג עבור הצהרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralODeclarationReshimonConversion = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.DeclarationReshimonConversion", DefaultText = "Special Activity Request",LocalDefaultText = @"המרות בין מספר רשימון ומספר הצהרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCreditQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CreditQuery", DefaultText = "Credit Query",LocalDefaultText = @"שאילתא לתקרת אשראי", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOPaymentQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.PaymentQuery", DefaultText = "Payment Query",LocalDefaultText = @"שאילתא להוראות תשלום", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOClaimFileFilterQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.ClaimFileFilterQuery", DefaultText = "Claim Query",LocalDefaultText = @"שאילתא לתביעות", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsGeneralOCustomsBookQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.CustomsBookQuery", DefaultText = "Customs Book Update",LocalDefaultText = @"עדכון ספר סיווג", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode CustomsGeneralTextCode_CustomsDeclarationStatusQueryODeclarationID = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.DeclarationStatusQuery.O.DeclarationID", DefaultText = "Declaration ID",LocalDefaultText = @"מס' הצהרה", ObjectTableId = CustomsGeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 