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
namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.CommonDataModel.EntityUpdateClasses
{
   public class DocsInUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "DocsIn",
			      				    IsNew =  false,
			      				    DBTableName =  "DocumentIns",
			      				    OldDBTableName =  "DocumentIns",
			      				    ObjectTableSingular =  "Docs In",
			      				    ObjectTablePlural =  "Docs Ins",
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
			      				    DefaultText =  "Docs In",
			      				    Code =  "6a6d",
			      				    Name =  "DocsIn",
			      				    GenerateDomainService =  false,
			      				    NoTS =  false,
			      				    HasMenuButtons =  false,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    SearchFields =  "DocsIn,DocumentIns,,Id,",
			                    
            }, ObjectTableRepository, TextCodeRepository, objectTables, textCodes);
		}
	
	    public void AddObjectFields(Dictionary<string, ObjectField> objectFields, Dictionary<string, TextCode> textCodes,ObjectFieldRepository ObjectFieldsRepository,TextCodeRepository TextCodeRepository)
	    {
	         	    }

	    public void AddTableQueries(Dictionary<string, Query> tenantQueries,Dictionary<string, QueryColumn> tenantQueryColumns, Dictionary<string, TextCode> textCodes, QueryGroupRepository queryGroupRepository, QueryRepository queriesRepository, QueryColumnRepository queryColumnsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, Dictionary<string, Feature> TenantFeatures,AdvancedQueryFilterRepository advancedQueryFiltersRepository,Dictionary<string, AdvancedQueryFilter> tenantAdvancedFilters)
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
			ObjectTable DocsInObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "DocsIn" && d.Tenant == 0).FirstOrDefault(); 
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
                ObjectTableId = DocsInObjectTable.Id,
				 
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
                ObjectTableId = DocsInObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable DocsInObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "DocsIn" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode DocsInTextCode_DocsInBDeleteAttachment = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsIn.B.DeleteAttachment", DefaultText = "Delete Attachment",LocalDefaultText = null, ObjectTableId = DocsInObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsInTextCode_DocsInBSearch = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsIn.B.Search", DefaultText = "Search",LocalDefaultText = null, ObjectTableId = DocsInObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsInTextCode_DocsInOAttach = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsIn.O.Attach", DefaultText = "Attach",LocalDefaultText = null, ObjectTableId = DocsInObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsInTextCode_DocsInOActions = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsIn.O.Actions", DefaultText = "Actions",LocalDefaultText = null, ObjectTableId = DocsInObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsInTextCode_DocsInOName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsIn.O.Name", DefaultText = "Name",LocalDefaultText = null, ObjectTableId = DocsInObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsInTextCode_DocsInOReferenceNo = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsIn.O.ReferenceNo", DefaultText = "Reference No.",LocalDefaultText = null, ObjectTableId = DocsInObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsInTextCode_DocsInOReceivedBy = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsIn.O.ReceivedBy", DefaultText = "Received By",LocalDefaultText = null, ObjectTableId = DocsInObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsInTextCode_DocsInOReceivedDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsIn.O.ReceivedDate", DefaultText = "Received Date",LocalDefaultText = null, ObjectTableId = DocsInObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsInTextCode_DocsInOFollowUp = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsIn.O.FollowUp", DefaultText = "Follow Up",LocalDefaultText = null, ObjectTableId = DocsInObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsInTextCode_DocsInONotes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsIn.O.Notes", DefaultText = "Notes",LocalDefaultText = null, ObjectTableId = DocsInObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsInTextCode_DocsInBUndoReceived = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsIn.B.UndoReceived", DefaultText = "Undo Received",LocalDefaultText = null, ObjectTableId = DocsInObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsInTextCode_DocsInBSetReceived = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsIn.B.SetReceived", DefaultText = "Set Received",LocalDefaultText = null, ObjectTableId = DocsInObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsInTextCode_DocsInBUpload = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsIn.B.Upload", DefaultText = "Upload",LocalDefaultText = null, ObjectTableId = DocsInObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsInTextCode_DocsInBView = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsIn.B.View", DefaultText = "View",LocalDefaultText = null, ObjectTableId = DocsInObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsInTextCode_DocsInBAdditional = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsIn.B.Additional", DefaultText = "Additional",LocalDefaultText = null, ObjectTableId = DocsInObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsInTextCode_DocsInBSelectFiles = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsIn.B.SelectFiles", DefaultText = "Select Files",LocalDefaultText = null, ObjectTableId = DocsInObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsInTextCode_DocsInBClose = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsIn.B.Close", DefaultText = "Close",LocalDefaultText = null, ObjectTableId = DocsInObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsInTextCode_DocsInODocsIn = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsIn.O.DocsIn", DefaultText = "Docs In",LocalDefaultText = null, ObjectTableId = DocsInObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsInTextCode_DocsInOFileSize = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsIn.O.FileSize", DefaultText = "File Size",LocalDefaultText = null, ObjectTableId = DocsInObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsInTextCode_DocsInOFileName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsIn.O.FileName", DefaultText = "File Name",LocalDefaultText = null, ObjectTableId = DocsInObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsInTextCode_DocsInOContactsList = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsIn.O.ContactsList", DefaultText = "Contacts List",LocalDefaultText = null, ObjectTableId = DocsInObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsInTextCode_DocsInMFileOpenByAnotherProgram = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsIn.M.FileOpenByAnotherProgram", DefaultText = "File is already open by another program!",LocalDefaultText = null, ObjectTableId = DocsInObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsInTextCode_DocsInMCantUploadFile = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsIn.M.CantUploadFile", DefaultText = "Can't upload file ( empty or corrupted)",LocalDefaultText = null, ObjectTableId = DocsInObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsInTextCode_DocsInMFileUploadedSuccessfully = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsIn.M.FileUploadedSuccessfully", DefaultText = "File Uploaded Successfully",LocalDefaultText = null, ObjectTableId = DocsInObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode DocsInTextCode_DocsInMNoDocsInFound = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "DocsIn.M.NoDocsInFound", DefaultText = "No Docs In found",LocalDefaultText = null, ObjectTableId = DocsInObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 