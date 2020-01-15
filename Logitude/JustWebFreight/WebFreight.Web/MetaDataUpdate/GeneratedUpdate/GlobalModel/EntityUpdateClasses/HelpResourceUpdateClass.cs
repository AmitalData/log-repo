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

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.GlobalModel.EntityUpdateClasses
{
   public class HelpResourceUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "HelpResource",
			      				    IsNew =  false,
			      				    DBTableName =  "HelpResources",
			      				    OldDBTableName =  "HelpResources",
			      				    ObjectTableSingular =  "Help Center",
			      				    ObjectTablePlural =  "Help Center",
			      				    HasCustomFilter =  false,
			      				    HasCustomFields =  false,
			      				    HasHelper =  false,
			      				    HasShortTitle =  false,
			      				    HasFiltersMenu =  false,
			      				    IsEditable =  false,
			      				    IsNewWizard =  false,
			      				    KeyPropertyPath =  "Code",
			      				    AutoCompleteSearchWindow =  false,
			      				    IsClosed =  false,
			      				    CacheOnClient =  false,
			      				    EditableFromAutoCompleteWindow =  false,
			      				    HasCounter =  true,
			      				    EnableAddFromLOV =  false,
			      				    IsRestrictable =  true,
			      				    IsMain =  true,
			      				    IsAutoComplete =  false,
			      				    EnableEditFromLOV =  false,
			      				    InActive =  false,
			      				    IsSaveButtonVisible =  true,
			      				    IsComposition =  false,
			      				    EnableSecurity =  true,
			      				    AllowCustomFields =  true,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    DefaultText =  "Help Center",
			      				    Code =  "50c4",
			      				    Name =  "HelpResource",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Global",
			      				    NoTS =  false,
			      				    HasMenuButtons =  false,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    IsTabsHidden =  false,
			      				    SearchFields =  "HelpResource,HelpResources,,Code,",
			                    
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

		   ObjectTable HelpResourceObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "HelpResource" && d.Tenant == 0).FirstOrDefault();
		   List<ObjectField> HelpResourceObjectFields = ObjectContext.ObjectFields.Where(d => d.ObjectTable.Name == "HelpResource").ToList();
		       
	      

	         Screen HelpResourceHelpResourceHeaderScreenScreen0 = AddScreensAndScreenFields.AddScreen(new ScreenDetails() { Code = "HelpResource.HeaderScreen", Name = "HelpResourceHeaderScreen", ObjectTableId = HelpResourceObjectTable.Id, NumberOfColumns = 2, NumberOfRows = 1, IsReadOnly = true }, screensRepository, tenantScreens);
      	
		    HelpResourceObjectTable.HeaderScreenId = HelpResourceHelpResourceHeaderScreenScreen0.Id;
	   		  

	    }

	    public void AddTableTabs(Dictionary<string, ObjectTableTab> TenantObjectTableTabs, Dictionary<string, TextCode> textCodes,ObjectTableTabRepository objectTableTabsRepository,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures ,IWebFreightContext ObjectContext)
	    {      
	    } 
	
	    public void AddTableFeatures(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  
		   ObjectTable HelpResourceObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "HelpResource" && d.Tenant == 0).FirstOrDefault(); 


		   		   //--------------> Additional Features <--------------\\

		   Feature HelpResourceFeature_CREATESIGNATURE = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CREATESIGNATURE", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.CreateSignature", NameTextCodeDefaultText = @"Create Your Own Signature" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_BUILDCONSOLIDATION = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BUILDCONSOLIDATION", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.BuildConsolidation", NameTextCodeDefaultText = @"Build a Consolidation Shipment" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_GENERICINTERFACE = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GENERICINTERFACE", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.GenericInterface", NameTextCodeDefaultText = @"Generic Invoice Interface" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_MANAGEUSERS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MANAGEUSERS", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ManageUsers", NameTextCodeDefaultText = @"Manage Users" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_ADVANCEDWORKBOOK = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ADVANCEDWORKBOOK", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.AdvancedWorkbook", NameTextCodeDefaultText = @"Advanced Features Workbook" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_AWBQUICKTOUR = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AWBQUICKTOUR", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.AWBQuickTour", NameTextCodeDefaultText = @"e-AWB Quick Tour" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_MAILTEMPLATES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MAILTEMPLATES", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.MailTemplates", NameTextCodeDefaultText = @"Managing Mail Templates" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_MANAGECURRENCY = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MANAGECURRENCY", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.CurrencyManagement", NameTextCodeDefaultText = @"Currency Management" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_ANALYZINGCRM = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ANALYZINGCRM", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.AnalyzeCRMData", NameTextCodeDefaultText = @"Analyzing CRM Data" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_MANAGEOPPORTUNITY = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MANAGEOPPORTUNITY", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ManagingOpportunities", NameTextCodeDefaultText = @"Managing Opportunities" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_ACTIVITYWORK = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACTIVITYWORK", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.WorkingActivities", NameTextCodeDefaultText = @"Working With Activities" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_MEASUREMENT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MEASUREMENT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ChangeMeasurement", NameTextCodeDefaultText = @"Change the Units of Measurement" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_AWBTUTORIALSP = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AWBTUTORIALSP", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.AWBTutorialSpanish", NameTextCodeDefaultText = @"e-AWB Tutorial (Spanish)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_GETTINGAROUNDSP = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GETTINGAROUNDSP", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.GettingAroundSpanish", NameTextCodeDefaultText = @"Getting Around in Logitude (Spanish)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_MANAGECUSTOMERS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MANAGECUSTOMERS", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ManagingCustomers", NameTextCodeDefaultText = @"Managing Customers" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_OUTLOOKCONNETION = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OUTLOOKCONNETION", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.OutlookConnection", NameTextCodeDefaultText = @"Logitude Outlook Connection" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_CUSTOMROLES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CUSTOMROLES", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.CustomRoles", NameTextCodeDefaultText = @"Custom Roles" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_AIRLINEACCOUNT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AIRLINEACCOUNT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.AirlineAccountNumber", NameTextCodeDefaultText = @"Airline Account Number" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_MANAGEAWBSTOCK = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MANAGEAWBSTOCK", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ManageAWBStock", NameTextCodeDefaultText = @"Manage AWB Stock by Airlines" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_CANCELINVOICE = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CANCELINVOICE", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.CancelInvoice", NameTextCodeDefaultText = @"Cancel an Invoice" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_SHAREDLOGISTICS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SHAREDLOGISTICS", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.SharedLogistics", NameTextCodeDefaultText = @"Activate Shared Logistics" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_GETTINGAROUND = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GETTINGAROUND", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.GettingAround", NameTextCodeDefaultText = @"Getting Around in Logitude" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_AWBTUTORIAL = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AWBTUTORIAL", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.AWBTutorial", NameTextCodeDefaultText = @"e-AWB Tutorial" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_CONSOLINVOICE = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CONSOLINVOICE", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ConsolidatedInvoice", NameTextCodeDefaultText = @"Issue a Consolidated Invoice" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_AWBTUTORIALFR = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AWBTUTORIALFR", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.AWBTutorialFrench", NameTextCodeDefaultText = @"e-AWB Tutorial (French)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_RELEASEDEC15 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "RELEASEDEC15", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ReleaseDecember2015", NameTextCodeDefaultText = @"December 2015 - Version R5.15" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_RELEASEFEB16 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "RELEASEFEB16", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ReleaseFebruary2016", NameTextCodeDefaultText = @"February 2016 - Version R1.16" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_LOGITUDEINTRO = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "LOGITUDEINTRO", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.LogitudeIntroduction", NameTextCodeDefaultText = @"Introduction to Logitude" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_BUILDSHIPMENT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BUILDSHIPMENT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.BuildingShipment", NameTextCodeDefaultText = @"Building a New Shipment" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_ISSUEINVOICE = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ISSUEINVOICE", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.IssuingInvoice", NameTextCodeDefaultText = @"Issuing an Invoice" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_BUSINESSTOOLS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BUSINESSTOOLS", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.BusinessTools", NameTextCodeDefaultText = @"Business Tools" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_BILLINGTOOLS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BILLINGTOOLS", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.BillingTools", NameTextCodeDefaultText = @"Billing & Accounting Tools" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_AWBWORLD = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AWBWORLD", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.LogitudeWorldAWB", NameTextCodeDefaultText = @"Logitude World e-AWB" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_MANAGECUSTOMERSHE = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MANAGECUSTOMERSHE", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ManagingCustomersHebrew", NameTextCodeDefaultText = @"Managing Customers (Hebrew)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_RELEASEMAY16 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "RELEASEMAY16", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ReleaseMay2016", NameTextCodeDefaultText = @"May 2016 - Version R2.16" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_EBOOKWORK = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EBOOKWORK", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.EBookWork", NameTextCodeDefaultText = @"Working with eBooking" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_CHANGEPASSWORD = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CHANGEPASSWORD", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ChangePassword", NameTextCodeDefaultText = @"How to change Password" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_BLUESNAP = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BLUESNAP", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.BlueSnapSubscribep", NameTextCodeDefaultText = @"Subscribe to Logitude BlueSnap" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_UNIFREIGHTGUIDE = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UNIFREIGHTGUIDE", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.UnifreightGuide", NameTextCodeDefaultText = @"Unifreight Mobile - User Guide (Hebrew)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_UNIFREIGHTSHARING = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UNIFREIGHTSHARING", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.UnifreightSharing", NameTextCodeDefaultText = @"Unifreight Mobile - Invitation and Sharing Data (Hebrew)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_OUTLOOKCONNECTIONHEBREW = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OUTLOOKCONNECTIONHEBREW", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.OutlookConnectionHebrew", NameTextCodeDefaultText = @"Logitude Outlook Connection (Hebrew)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_ANALYZINGCRMHEBREW = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ANALYZINGCRMHEBREW", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.AnalyzeCRMDataHebrew", NameTextCodeDefaultText = @"Analyzing CRM Data (Hebrew)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_OUTLOOKINSTALLATIONHEBREW = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OUTLOOKINSTALLATIONHEBREW", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.OutlookInstallationHebrew", NameTextCodeDefaultText = @"Outlook Installation (Hebrew)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_MANAGEOPPORTUNITYHEBREW = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MANAGEOPPORTUNITYHEBREW", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ManagingOpportunitiesHebrew", NameTextCodeDefaultText = @"Managing Opportunities (Hebrew)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_UNIFREIGHTCRMR52015 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UNIFREIGHTCRMR52015", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.UnifreightCRMR52015", NameTextCodeDefaultText = @"Unifreight CRM R5-2015 (Hebrew)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_UNIFREIGHTCRMR12016 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UNIFREIGHTCRMR12016", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.UnifreightCRMR12016", NameTextCodeDefaultText = @"Unifreight CRM R1-2016 (Hebrew)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_UNIFREIGHTCRMR22016 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UNIFREIGHTCRMR22016", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.UnifreightCRMR22016", NameTextCodeDefaultText = @"Unifreight CRM R2-2016 (Hebrew)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_OUTLOOKCONNECTIONSETUP = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OUTLOOKCONNECTIONSETUP", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.OutlookInstallationSetup", NameTextCodeDefaultText = @"Logitude Outlook Connection Setup Guide" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_RELEASEJUL16 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "RELEASEJUL16", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ReleaseJul2016", NameTextCodeDefaultText = @"July 2016 - Version R3.16" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_RELEASEOCT16 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "RELEASEOCT16", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ReleaseOct2016", NameTextCodeDefaultText = @"October 2016 - Version R4.16" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_LOGITUDEMOBILE = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "LOGITUDEMOBILE", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.LogitudeMobile", NameTextCodeDefaultText = @"Logitude Mobile" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_SHAREDLOGISTICSANDMOBILESETUP = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SHAREDLOGISTICSANDMOBILESETUP", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.SharedLogisticsMobileSetup", NameTextCodeDefaultText = @"Shared Logistics & Mobile Setup" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_ANALYZINGCRMFRENCH = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ANALYZINGCRMFRENCH", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.AnalyzeCRMDataFrench", NameTextCodeDefaultText = @"Analyzing CRM Data (French)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_MANAGEOPPORTUNITYFRENCH = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MANAGEOPPORTUNITYFRENCH", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ManagingOpportunitiesFrench", NameTextCodeDefaultText = @"Managing Opportunities (French)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_ACTIVITYWORKFRENCH = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACTIVITYWORKFRENCH", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.WorkingActivitiesFrench", NameTextCodeDefaultText = @"Working With Activities (French)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_QUOTESTUTORIAL = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "QUOTESTUTORIAL", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.QuotesTutorial", NameTextCodeDefaultText = @"Quotes Tutorial" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_RELEASEDEC16 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "RELEASEDEC16", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ReleaseDec2016", NameTextCodeDefaultText = @"December 2016 - Version R5.16" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_RELEASEFEB17 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "RELEASEFEB17", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ReleaseFeb2017", NameTextCodeDefaultText = @"Feb 2017 - Version R1.17" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_MANAGEUSERFRENCHTUTORIAL = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MANAGEUSERFRENCHTUTORIAL", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ManageUserFrenchTutorial", NameTextCodeDefaultText = @"How to manage users (French)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_QUICKBOOKSCONNECTION = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "QUICKBOOKSCONNECTION", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.QuickBooksConnection", NameTextCodeDefaultText = @"QuickBooks Online Connection Setup and Activation" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_VATTYPEMANAGEMENT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "VATTYPEMANAGEMENT", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.VATTypeManagement", NameTextCodeDefaultText = @"VAT Type Management)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_BUILDCONSOLIDATIONSPANISH = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BUILDCONSOLIDATIONSPANISH", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.BuildConsolidationSpanish", NameTextCodeDefaultText = @"How to Build a Consolidation Shipment (Spanish)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_SHAREDLOGISTICSANDMOBILESPANISH = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SHAREDLOGISTICSANDMOBILESPANISH", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.SharedLogisticsMobileSetupSpanish", NameTextCodeDefaultText = @"Shared Logistics & Mobile (Spanish)" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_RELEASEMAY17 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "RELEASEMAY17", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ReleaseMay2017", NameTextCodeDefaultText = @"May 2017 - Version R2.17" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_RELEASEJUL17 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "RELEASEJUL17", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.ReleaseJuly2017", NameTextCodeDefaultText = @"July 2017 - Version R3.17" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature HelpResourceFeature_FOLLOWUPSTORIAL = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "FOLLOWUPSTORIAL", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.FollowUps", NameTextCodeDefaultText = @"Follow Ups" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   //Feature HelpResourceFeature_Module = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.Module", NameTextCodeDefaultText = @"HelpResource Package Feature" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   //Feature HelpResourceFeature_READ = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", Packagable = false, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = HelpResourceObjectTable.Id, Tenant = 0, NameTextCodeCode = "HelpResource.Features.READ", NameTextCodeDefaultText = @"Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

   
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable HelpResourceObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "HelpResource" && d.Tenant == 0).FirstOrDefault(); 
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
                ObjectTableId = HelpResourceObjectTable.Id,
				 
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
                ObjectTableId = HelpResourceObjectTable.Id,
				 
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
	 