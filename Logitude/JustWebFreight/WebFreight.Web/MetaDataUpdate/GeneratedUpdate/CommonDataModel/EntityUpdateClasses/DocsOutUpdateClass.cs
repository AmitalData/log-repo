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
using Logitude.Workflow.Data.Repositories;
using Logitude.Workflow.Data.EntityPOCOs;
using Logitude.Workflow.BL.CLoseTable;
using Logitude.DashboardModule.Data.Repositories;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.BL;







namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.CommonDataModel.EntityUpdateClasses
{
   public class DocsOutUpdateClass
   {  		
		public const string HashString = "efdf354c0964a559b7dc61460dcd84f0";
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "DocsOut",
			      				    IsNew =  false,
			      				    DBTableName =  "DocumentOuts",
			      				    OldDBTableName =  "DocumentOuts",
			      				    ObjectTableSingular =  "Docs Out",
			      				    ObjectTablePlural =  "Docs Outs",
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
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  true,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    DefaultText =  "Docs Out",
			      				    Code =  "b709",
			      				    Name =  "DocsOut",
			      				    GenerateDomainService =  false,
			      				    NoTS =  false,
			      				    HasMenuButtons =  false,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    SearchFields =  "DocsOut,DocumentOuts,,Id,",
			      				    HashString =  DocsOutUpdateClass.HashString,
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository, List<ObjectField> addedFields, List<TextCode> addedTextCodes)
	    {
	         	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters,Dictionary<string, QueryGroup> tenantQueryGroups )
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
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable DocsOutObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "DocsOut" && d.Tenant == 0).FirstOrDefault(); 
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
                ObjectTableId = DocsOutObjectTable.Id,
				 
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
                ObjectTableId = DocsOutObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable DocsOutObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "DocsOut" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode DocsOutTextCode_DocsOutMSelectCopyThenRebuild = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.M.SelectCopyThenRebuild", DefaultText = "Please select a copy then rebuild!",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutMNoTemplatesFound = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.M.NoTemplatesFound", DefaultText = "No templates found for this document!",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutMRebuildThenPrintAgain = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.M.RebuildThenPrintAgain", DefaultText = "Please Rebuild Then Print Again!",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutMNoDocumentFound = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.M.NoDocumentFound", DefaultText = "No Document Found!",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutMExportDocumentFailed = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.M.ExportDocumentFailed", DefaultText = "Export Document Failed",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutMSpecifyRecepient = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.M.SpecifyRecepient", DefaultText = "Please specify at least one recepient",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutBGet = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.B.Get", DefaultText = "Get",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutBSet = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.B.Set", DefaultText = "Set",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutODocumentEditor = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.O.DocumentEditor", DefaultText = "Document Editor",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutOEditedFields = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.O.EditedFields", DefaultText = "edited fields",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutONumberOfEditedFields = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.O.NumberOfEditedFields", DefaultText = "Number of Edited fields",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutOTemplatesError = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.O.TemplatesError", DefaultText = "Templates Error",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutOSendMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.O.SendMessage", DefaultText = "Send Message",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutOAvailableDocuments = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.O.AvailableDocuments", DefaultText = "Available Documents",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutBEdit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.B.Edit", DefaultText = "Edit",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutBView = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.B.View", DefaultText = "View",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutBSend = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.B.Send", DefaultText = "Send",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutBPrintSend = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.B.PrintSend", DefaultText = "Print/Send",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutBPrint = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.B.Print", DefaultText = "Print",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutBStartEdit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.B.StartEdit", DefaultText = "Start Edit",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutBEndEdit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.B.EndEdit", DefaultText = "End Edit",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutBReset = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.B.Reset", DefaultText = "Reset",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutBApply = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.B.Apply", DefaultText = "Apply",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutBClose = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.B.Close", DefaultText = "Close",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutBSave = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.B.Save", DefaultText = "Save",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutBRebuild = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.B.Rebuild", DefaultText = "Rebuild",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutBSaveSelectedAsDefault = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.B.SaveSelectedAsDefault", DefaultText = "Save Selected As Default",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutBPrintAllCopies = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.B.PrintAllCopies", DefaultText = "Print All Copies",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutODocsOut = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.O.DocsOut", DefaultText = "Docs Out",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutOVerticalShift = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.O.VerticalShift", DefaultText = "Vertical Shift",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutOMM = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.O.MM", DefaultText = "mm",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutOHorizontalShift = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.O.HorizontalShift", DefaultText = "Horizontal Shift",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutOTemplates = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.O.Templates", DefaultText = "Templates",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutOAdditionalFields = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.O.AdditionalFields", DefaultText = "Additional Fields",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutOLastBuildDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.O.LastBuildDate", DefaultText = "Last Build Date",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutBSearch = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.B.Search", DefaultText = "Search",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutOActions = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.O.Actions", DefaultText = "Actions",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutOName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.O.Name", DefaultText = "Name",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutOReferenceNo = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.O.ReferenceNo", DefaultText = "Reference No.",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutOIssuedBy = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.O.IssuedBy", DefaultText = "Issued By",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutOIssuedDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.O.IssuedDate", DefaultText = "Issued Date",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutOFollowUp = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.O.FollowUp", DefaultText = "Follow Up",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutAddTemplates = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.AddTemplates", DefaultText = "Adding Templates",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "H", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsOutTextCode_DocsOutAddYourOwnTemplates = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsOut.AddYourOwnTemplates", DefaultText = "You can add your own templates for sending e-mails in addition to the pre-installed templates",LocalDefaultText = null, ObjectTableId = DocsOutObjectTable.Id, Tenant = 0, TextCodeTypeCode = "H", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 