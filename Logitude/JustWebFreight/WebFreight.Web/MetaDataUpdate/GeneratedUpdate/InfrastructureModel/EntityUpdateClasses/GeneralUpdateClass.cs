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

namespace WebFreight.Web.MetaDataUpdate.GeneratedUpdate.InfrastructureModel.EntityUpdateClasses
{
   public class GeneralUpdateClass
   {  
	    public void AddObjectTable(Dictionary<string, ObjectTable> objectTables, Dictionary<string, TextCode> textCodes,ObjectTableRepository ObjectTableRepository,TextCodeRepository TextCodeRepository)
        {                     
            
            AddObjectsAndObjectFields.AddObjectTable(new ObjectTableDetails()
            {
			
	             				    ObjectTableName =  "General",
			      				    IsNew =  false,
			      				    DBTableName =  "Generals",
			      				    OldDBTableName =  "Generals",
			      				    ObjectTableSingular =  "General",
			      				    ObjectTablePlural =  "Generals",
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
			      				    IsComposition =  false,
			      				    EnableSecurity =  false,
			      				    AllowCustomFields =  false,
			      				    HasDynamicHeader =  false,
			      				    ObjectTableTypeCode =  "MD",
			      				    MaxNumberOfCustomFields =  0,
			      				    DefaultText =  "General",

			      				    Code =  "919a",

			      				    Name =  "General",
			      				    GenerateDomainService =  false,
			      				    ClientModuleName =  "Generals",
			      				    ServerModuleName =  "Generals",
			      				    NoTS =  false,
			      				    HasMenuButtons =  false,
			      				    AllowedForComputingPartners =  false,
			      				    CustomFieldsCount =  0,
			      				    DisableSearchBox =  false,
			      				    HasDocuments =  false,
			      				    IsLookUp =  false,
			      				    IsTabsHidden =  false,
			      				    SearchFields =  "General,Generals,,Id,",
			                    
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
		   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault(); 

		   Feature GeneralFeatureNew = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NEW", FeatureTypeCode = "NEW", IsBusinessUnitEnabled = true, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.New", NameTextCodeDefaultText = "New" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature GeneralFeatureRead = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "READ", FeatureTypeCode = "READ", IsBusinessUnitEnabled = true, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Read", NameTextCodeDefaultText = "Read" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature GeneralFeatureUpdate = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "UPDATE", FeatureTypeCode = "UPDT", IsBusinessUnitEnabled = true, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Edit", NameTextCodeDefaultText = "Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);
		   Feature GeneralFeatureModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Module", FeatureTypeCode = "MODL", IsBusinessUnitEnabled = true, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.PackageFeature", NameTextCodeDefaultText = "General Package Feature", Packagable = true }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes); 

		   		   //--------------> Additional Features <--------------\\

		   Feature GeneralFeature_DASHBOARD = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DASHBOARD", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Dashboard", NameTextCodeDefaultText = @"Dashboard" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_USERS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "USERS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Users", NameTextCodeDefaultText = @"Users" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_QUOTES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "QUOTES", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Quotes", NameTextCodeDefaultText = @"Quotes" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_ARINVOICES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ARINVOICES", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.ARInvoices", NameTextCodeDefaultText = @"Accounting" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_SHIPMENTS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SHIPMENTS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Shipments", NameTextCodeDefaultText = @"Shipments" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_MASTERS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MASTERS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Masters", NameTextCodeDefaultText = @"Masters" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_MAINTENANCE = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MAINTENANCE", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Maintenance", NameTextCodeDefaultText = @"Maintenance" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_CONTACTS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CONTACTS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Contacts", NameTextCodeDefaultText = @"Contacts" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_CUSTOMERS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CUSTOMERS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Customers", NameTextCodeDefaultText = @"Customers" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_AGENTS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AGENTS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Agents", NameTextCodeDefaultText = @"Agents" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_CUSTOMAGENTS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CUSTOMAGENTS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.CustomAgents", NameTextCodeDefaultText = @"CustomAgents" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_SHIPPINGAGENTS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SHIPPINGAGENTS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.ShippingAgents", NameTextCodeDefaultText = @"Shipping Agents" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_AIRLINES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AIRLINES", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Airlines", NameTextCodeDefaultText = @"Airlines" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_SHIPPINGLINES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SHIPPINGLINES", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.ShippingLines", NameTextCodeDefaultText = @"Shipping Lines" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_TRUCKERS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TRUCKERS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Truckers", NameTextCodeDefaultText = @"Truckers" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_INCOTERMS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "INCOTERMS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Incoterms", NameTextCodeDefaultText = @"Incoterms" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_PAYMENTTERMS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PAYMENTTERMS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.PaymentTerms", NameTextCodeDefaultText = @"Payment Terms" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_CURRENCIES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CURRENCIES", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Currencies", NameTextCodeDefaultText = @"Currencies" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_VATTYPES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "VATTYPES", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.VatTypes", NameTextCodeDefaultText = @"Vat Types" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_CHARGESTYPES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CHARGESTYPES", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.ChargesTypes", NameTextCodeDefaultText = @"Charges Types" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_PORTS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PORTS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Ports", NameTextCodeDefaultText = @"Ports" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_COUNTRIES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "COUNTRIES", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Countries", NameTextCodeDefaultText = @"Countries" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_GLOBALZONES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GLOBALZONES", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.GlobalZones", NameTextCodeDefaultText = @"Global Zones" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_BRANCHES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BRANCHES", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Branches", NameTextCodeDefaultText = @"Branches" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_DEPARTMENTS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DEPARTMENTS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Departments", NameTextCodeDefaultText = @"Departments" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_STATES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "STATES", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.States", NameTextCodeDefaultText = @"States" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_DOCUMENTTYPES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DOCUMENTTYPES", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.DocumentTypes", NameTextCodeDefaultText = @"Document Types" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_EVENTTYPES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EVENTTYPES", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.EventTypes", NameTextCodeDefaultText = @"Event Types" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_PACKAGETYPES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PACKAGETYPES", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.PackageTypes", NameTextCodeDefaultText = @"Package Types" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_PACKAGETYSPES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PACKAGETYSPES", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = true, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.PackageTypes", NameTextCodeDefaultText = @"Package Types" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_VESSELS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "VESSELS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Vessels", NameTextCodeDefaultText = @"Vessels" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_WAREHOUSES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "WAREHOUSES", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Warehouses", NameTextCodeDefaultText = @"Warehouses" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_RATESTABLES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "RATESTABLES", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.RatesTables", NameTextCodeDefaultText = @"Rates Tables" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_POTENTIALCUSTOMERS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "POTENTIALCUSTOMERS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = true, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.PotentialCustomers", NameTextCodeDefaultText = @"PotentialCustomers" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_ACCOUNTS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACCOUNTS", FeatureTypeCode = "OTH", Packagable = false, IsBusinessUnitEnabled = false, IsOld = true, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Accounts", NameTextCodeDefaultText = @"Accounts" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_APINVOICES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "APINVOICES", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.APInvoices", NameTextCodeDefaultText = @"A/P Invoices" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_VENDORS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "VENDORS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Vendors", NameTextCodeDefaultText = @"Vendors" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_FOLLOWUPS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "FOLLOWUPS", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = true, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.FollowUps", NameTextCodeDefaultText = @"Follow Ups" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_GETTINGSTARTED = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GETTINGSTARTED", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = true, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.GettingStarted", NameTextCodeDefaultText = @"Getting Started" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_EVENTS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EVENTS", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Events", NameTextCodeDefaultText = @"Events" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_DOCSIN = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DOCSIN", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.DocsIn", NameTextCodeDefaultText = @"DocsIn" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_BUILDQUERIES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BUILDQUERIES", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.BuildQueries", NameTextCodeDefaultText = @"Build Queries" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_Customization = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.Customization", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Customization", NameTextCodeDefaultText = @"Customization" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_PERSONALSETTINGS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PERSONALSETTINGS", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.PersonalSettings", NameTextCodeDefaultText = @"Personal Settings" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_SYSTEMSETTINGS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SYSTEMSETTINGS", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.SystemSettings", NameTextCodeDefaultText = @"System Settings" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_SHAREDLOGISTICS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SHAREDLOGISTICS", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.SharedLogisticsUpdates", NameTextCodeDefaultText = @"Web Access" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_CANCELINVOICE = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CANCELINVOICE", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.CANCELINVOICE", NameTextCodeDefaultText = @"CANCEL INVOICE" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_HOWTOACCOUNTINGSETTINGS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "HOWTOACCOUNTINGSETTINGS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.HOWTOACCOUNTINGSETTINGS", NameTextCodeDefaultText = @"Accounting Settings" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_CREDITCARDTYPES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CREDITCARDTYPES", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.CreditCardTypes", NameTextCodeDefaultText = @"Credit Card Types" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_SHAREDLOGISTICSMENU = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SHAREDLOGISTICSMENU", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.SharedLogistics", NameTextCodeDefaultText = @"Shared Logistics" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_MOVETYPES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MOVETYPES", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.MoveTypes", NameTextCodeDefaultText = @"Move Types" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_REPORTS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "REPORTS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Report", NameTextCodeDefaultText = @"Report" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_ERRORLOG = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ERRORLOG", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.ErrorLog", NameTextCodeDefaultText = @"Error Log" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_CUSTOMS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CUSTOMS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Customs", NameTextCodeDefaultText = @"Customs" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_CHAMP = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CHAMP", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Champ", NameTextCodeDefaultText = @"Champ Settings" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_COMPETITORS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "COMPETITORS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Competitors", NameTextCodeDefaultText = @"Competitors" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_STAGES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "STAGES", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = true, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Stages", NameTextCodeDefaultText = @"Stages" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_LEADSOURCES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "LEADSOURCES", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.LeadSources", NameTextCodeDefaultText = @"Lead Sources" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_CRM = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CRM", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.CRM", NameTextCodeDefaultText = @"CRM" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_OPERATIONS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OPERATIONS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Operations", NameTextCodeDefaultText = @"Operations" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_INDUSTRIES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "INDUSTRIES", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Industries", NameTextCodeDefaultText = @"Industries" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_CUSTOMBANK = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CUSTOMBANK", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = true, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.CustomBank", NameTextCodeDefaultText = @"Custom Bank" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_SOCIAL = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SOCIAL", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Social", NameTextCodeDefaultText = @"Social" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_QUOTETMPLATES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "QUOTETMPLATES", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.QuoteTemplate", NameTextCodeDefaultText = @"Quote Template" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_COUNTERS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "COUNTERS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Counters", NameTextCodeDefaultText = @"Counters" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_CompanyAddress = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.CompanyAddress", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.CompanyAddress", NameTextCodeDefaultText = @"Company Address Settings" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_SystemDefaults = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.SystemDefaults", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.SystemDefaults", NameTextCodeDefaultText = @"System Defaults" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_CompanyLogo = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.CompanyLogo", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.CompanyLogo", NameTextCodeDefaultText = @"Company Logo" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_SystemCurrencies = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.SystemCurrencies", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.SystemCurrencies", NameTextCodeDefaultText = @"System Currencies" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_LocalSettings = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.LocalSettings", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.LocalSettings", NameTextCodeDefaultText = @"Local Settings" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_InvoiceSettings = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.InvoiceSettings", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.InvoiceSettings", NameTextCodeDefaultText = @"Invoice Settings" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_Signature = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.Signature", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Signature", NameTextCodeDefaultText = @"Signature" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_SIGNATURESETTING = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SIGNATURESETTING", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Signature", NameTextCodeDefaultText = @"Signature" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_CHANGEPASSWORDSETTING = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CHANGEPASSWORDSETTING", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.ChangePassword", NameTextCodeDefaultText = @"Change Password" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_ChangePassword = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.ChangePassword", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.ChangePassword", NameTextCodeDefaultText = @"Change Password" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_ADDITIONALSERVICES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ADDITIONALSERVICES", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.AdditionalServices", NameTextCodeDefaultText = @"Additional Services" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_SOCIALGENERAL = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SOCIALGENERAL", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.SocialGeneral", NameTextCodeDefaultText = @"Social" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_PRODUCTTYPES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PRODUCTTYPES", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.ProductTypes", NameTextCodeDefaultText = @"Product Types" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_IMPORTSHIPMETNS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "IMPORTSHIPMETNS", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = true, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.ImportShipments", NameTextCodeDefaultText = @"Import Shipments" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_CustomerActivation = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.CustomerActivation", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.CustomerActivation", NameTextCodeDefaultText = @"Customer Activation Settings" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_EMAILALERTSETTINGS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EMAILALERTSETTINGS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.EmailAlertSettings", NameTextCodeDefaultText = @"Email Alert Settings" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_SHIPPERSANDCONSIGNEES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SHIPPERSANDCONSIGNEES", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.ShippersAndConsignees", NameTextCodeDefaultText = @"Shippers and Consignees" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_SPECIALSERVICESTYPES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SPECIALSERVICESTYPES", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.SpecialServicesType", NameTextCodeDefaultText = @"Special Service Types" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_REGIONS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "REGIONS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Region", NameTextCodeDefaultText = @"Regions" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_LOGITUDELEADS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "LOGITUDELEADS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.LogitudeLead", NameTextCodeDefaultText = @"Logitude Lead" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_CUSTOMERSIZES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CUSTOMERSIZES", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.CustomerSizes", NameTextCodeDefaultText = @"Customer Sizes" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_CLOSINGREASONS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CLOSINGREASONS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = true, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.ClosingReasons", NameTextCodeDefaultText = @"Closing Reasons" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_QUESTIONNAIRE = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "QUESTIONNAIRE", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = true, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Questionnaire", NameTextCodeDefaultText = @"Questionnaire" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_QUESTIONNAIREGENERAL = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "QUESTIONNAIREGENERAL", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = true, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.QuestionnaireGeneral", NameTextCodeDefaultText = @"Questionnaire" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_TenantManagement = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.TenantManagement", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.TenantManagement", NameTextCodeDefaultText = @"Tenant Management" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_CountryCity_M_Cities = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CountryCity.M.Cities", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Cities", NameTextCodeDefaultText = @"Cities" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_SupportManagement = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.SupportManagement", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.SupportManagement", NameTextCodeDefaultText = @"Support Management" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_CustomFieldsEdit = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.CustomFieldsEdit", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.CustomFieldsEdit", NameTextCodeDefaultText = @"Custom Fields Edit" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_DOCUMENTS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DOCUMENTS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Documents", NameTextCodeDefaultText = @"Documents" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_MAINCUSTOMERS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MAINCUSTOMERS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.MaintenanceCustomers", NameTextCodeDefaultText = @"Maintenance Customers" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_CUSTOMIZATION = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CUSTOMIZATION", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Customization1", NameTextCodeDefaultText = @"Customization" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_BluesnapContract = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.BluesnapContract", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.BluesnapContract", NameTextCodeDefaultText = @"Bluesnap Contract" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_BLUESNAPS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BLUESNAPS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.BluesnapContracts", NameTextCodeDefaultText = @"Bluesnap Contracts" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_DOCUMENTFOLDERS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DOCUMENTFOLDERS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.DocumentFolder", NameTextCodeDefaultText = @"Document Folder" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_MOBILE = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MOBILE", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Mobile", NameTextCodeDefaultText = @"Mobile" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_TENANTTOTANGO = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TENANTTOTANGO", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Totango", NameTextCodeDefaultText = @"Totango" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_ACCOUNTINGSYSTEMS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACCOUNTINGSYSTEMS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.AccountingSystems", NameTextCodeDefaultText = @"Accounting Systems" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Menu_AWBAdditionalHandlingInfo = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Menu.AWBAdditionalHandlingInfo", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Menu.AWBAdditionalHandlingInfo", NameTextCodeDefaultText = @"AWB Additional Handling Info" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_IMPORTER = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "IMPORTER", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Importers", NameTextCodeDefaultText = @"Importers" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_CUSTOMERTENANTACCESSES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CUSTOMERTENANTACCESSES", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.CustomerTenantAccesses", NameTextCodeDefaultText = @"Customer Tenant Accesses" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_CUSTOMERTENANTACCESSREQUESTS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CUSTOMERTENANTACCESSREQUESTS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.CustomerTenantAccessRequests", NameTextCodeDefaultText = @"Customer Tenant Access Requests" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_HYBRIDPARTNERS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "HYBRIDPARTNERS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.HybridPartners", NameTextCodeDefaultText = @"Hybrid Partners" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_BusinessHour = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.BusinessHour", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.BusinessHour", NameTextCodeDefaultText = @"Business Hours" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_BUSINESSHOUR = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BUSINESSHOUR", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.BusinessHour", NameTextCodeDefaultText = @"Business Hours" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_HybridTenantThreshold = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.HybridTenantThreshold", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.HybridTenantThreshold", NameTextCodeDefaultText = @"Hybrid Tenant Threshold" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_SystemUserPassword = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.SystemUserPassword", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.SystemUserPassword", NameTextCodeDefaultText = @"System User Password" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_DATACHECK = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DATACHECK", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.DataCheck", NameTextCodeDefaultText = @"Data Security Check" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_INBOUNDEMAIL = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "INBOUNDEMAIL", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.InboundEmail", NameTextCodeDefaultText = @"Inbound Email" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_FULLACCOUNTING = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "FULLACCOUNTING", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.FULLACCOUNTING", NameTextCodeDefaultText = @"FULL ACCOUNTING" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_ACTIVATIONWIZARD = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACTIVATIONWIZARD", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.ActivationWizards", NameTextCodeDefaultText = @"Activation Wizard" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_PARTICIPANTS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PARTICIPANTS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Participants", NameTextCodeDefaultText = @"Participants" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_AIRLINESTATISTICS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AIRLINESTATISTICS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.AirlineStatistics", NameTextCodeDefaultText = @"Airline Statistics" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_ApiCredintials = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ApiCredintials", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.ApiCredintials", NameTextCodeDefaultText = @"API Credintials" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_OPPORTUNITYTYPES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OPPORTUNITYTYPES", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.OpportunityTypes", NameTextCodeDefaultText = @"Opportunity Types" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_TICKETTYPES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TICKETTYPES", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.TicketTypes", NameTextCodeDefaultText = @"Ticket Types" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_TICKETSEVERITIES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TICKETSEVERITIES", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.TicketSeverities", NameTextCodeDefaultText = @"Ticket Severities" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_TICKETSTAGES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TICKETSTAGES", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.TicketStages", NameTextCodeDefaultText = @"Ticket Stages" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_TICKETCLASSIFICATION = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TICKETCLASSIFICATION", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.TicketClassifications", NameTextCodeDefaultText = @"Ticket Classifications" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_EMPLOYEEGROUPS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EMPLOYEEGROUPS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.EmployeeGroups", NameTextCodeDefaultText = @"Employee Groups" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_SLAHEADER = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SLAHEADER", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.SLAHeader", NameTextCodeDefaultText = @"SLA" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_HYBRIDTENANTSTATE = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "HYBRIDTENANTSTATE", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.HybridTenantStates", NameTextCodeDefaultText = @"Hybrid Tenant State" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_AirlineSettings = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.AirlineSettings", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.AirlineSettings", NameTextCodeDefaultText = @"Airline Settings" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_FlightsSchedules = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "FlightsSchedules", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.FlightsSchedules", NameTextCodeDefaultText = @"Flights Schedules" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_Automations = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.Automations", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Automations", NameTextCodeDefaultText = @"Automations" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_TICKET = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TICKET", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Ticket", NameTextCodeDefaultText = @"Tickets" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_TRANSMISSIONLOG = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TRANSMISSIONLOG", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.LogitudeMessagesTransmissionLogs", NameTextCodeDefaultText = @"Logitude Messages Transmission Logs" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_AIRLINEDASHBOARD = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AIRLINEDASHBOARD", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.AirlineDashboard", NameTextCodeDefaultText = @"Airline Dashboard" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_OUTLOOKCONNETION = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "OUTLOOKCONNETION", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.OutLookConnection", NameTextCodeDefaultText = @"OutLook Connection" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_ADDPARTNERTOACTIVATIONWIZARD = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ADDPARTNERTOACTIVATIONWIZARD", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.AddPartnerToActivationWizards", NameTextCodeDefaultText = @"Add Partner To Activation Wizard" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_ACCOUNTINGS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACCOUNTINGS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Accounting", NameTextCodeDefaultText = @"Accounting" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_Category1 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.Category1", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Category1", NameTextCodeDefaultText = @"Category 1" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_Category2 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.Category2", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Category2", NameTextCodeDefaultText = @"Category 2" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_Category3 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.Category3", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Category3", NameTextCodeDefaultText = @"Category 3" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_Category4 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.Category4", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Category4", NameTextCodeDefaultText = @"Category 4" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_Category5 = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.Category5", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Category5", NameTextCodeDefaultText = @"Category 5" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_FullAccountingSetting = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.FullAccountingSetting", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.FullAccountingSetting", NameTextCodeDefaultText = @"Full Accounting Setting" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_CustomerFieldsUpdateSetting = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.CustomerFieldsUpdateSetting", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.CustomerFieldsUpdateSetting", NameTextCodeDefaultText = @"Customer Fields Update Settings" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_AccountingPeriods = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.AccountingPeriods", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.AccountingPeriods", NameTextCodeDefaultText = @"Accounting Periods Menu" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_CHARGESGROUP = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CHARGESGROUP", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.ChargesGroups", NameTextCodeDefaultText = @"Charges Groups" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_NotesRightToLeftEnabled = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "NotesRightToLeftEnabled", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.NotesRightToLeftEnabled", NameTextCodeDefaultText = @"Notes Right To Left" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_PrivateLabels = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PrivateLabels", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.PrivateLabels", NameTextCodeDefaultText = @"Private Labels" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_WAREHOUSEMANAGEMENT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "WAREHOUSEMANAGEMENT", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.WarehouseManagement", NameTextCodeDefaultText = @"Warehouse Management" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_EXPORTEXCEL = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EXPORTEXCEL", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.EXCEL", NameTextCodeDefaultText = @"Download to Excel" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_DROPBOX = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DROPBOX", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.DropBox", NameTextCodeDefaultText = @"DropBox" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_DROPBOXTESTFILE = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "DROPBOXTESTFILE", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.DropBoxTestFile", NameTextCodeDefaultText = @"DropBoxTestFile" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_CUSTOMSINTERFACESETTINGS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CUSTOMSINTERFACESETTINGS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.CUSTOMSINTERFACESETTINGS", NameTextCodeDefaultText = @"Customs Interface Settings" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_SATINTERFACE = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SATINTERFACE", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.SATInterface", NameTextCodeDefaultText = @"SAT Interface" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_LBDS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "LBDS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.LBDS", NameTextCodeDefaultText = @"LB Digital signature" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_FBLSTOCKS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "FBLSTOCKS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.FBLStocks", NameTextCodeDefaultText = @"FBL Stock" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_MOBILELOGO = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "MOBILELOGO", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.MobileLogo", NameTextCodeDefaultText = @"Mobile Logo" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_SHAREDLOGISTICSLOGO = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SHAREDLOGISTICSLOGO", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.sharedLogisticsLogo", NameTextCodeDefaultText = @"Shared Logistics Logo" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_TimeManagement = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TimeManagement", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.TimeManagement", NameTextCodeDefaultText = @"TimeManagement" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_SecurityPolicySettings = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.SecurityPolicySettings", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.SecurityPolicySettings", NameTextCodeDefaultText = @"Login Policy Settings" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_INTEGRATIONSYSTEMSETTINGS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "INTEGRATIONSYSTEMSETTINGS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.IntegrationSystemsSetting", NameTextCodeDefaultText = @"Integration Systems Setting" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_TERMOFUSERFEATUE = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TERMOFUSERFEATUE", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.TermsofUse", NameTextCodeDefaultText = @"Terms of Use" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_FeaturesChanges = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "FeaturesChanges", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.FeaturesChanges", NameTextCodeDefaultText = @"Features Changes" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_DocumentFilingEmailSettings = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.DocumentFilingEmailSettings", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.DocumentFilingEmailSettings", NameTextCodeDefaultText = @"Document Filing Email Settings" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_YearTransfer = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.YearTransfer", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.YearTransfer", NameTextCodeDefaultText = @"Year Transfer" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_AppSettingsBtn = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AppSettingsBtn", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.AppSettingsBtn", NameTextCodeDefaultText = @"Application Settings Button" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_CROSSDOCKS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CROSSDOCKS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.CrossDocks", NameTextCodeDefaultText = @"Cross Docks" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_QuoteSettings = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.QuoteSettings", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.QuoteSettings", NameTextCodeDefaultText = @"Quote Settings" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_InttraSettings = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.InttraSettings", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.InttraSettings", NameTextCodeDefaultText = @"INTTRA Settings" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_InttraCommunicationSettings = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.InttraCommunicationSettings", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.InttraCommunicationSettings", NameTextCodeDefaultText = @"INTTRA Communication Settings" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_ARPAYMENTMETHODS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ARPAYMENTMETHODS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.ARPaymentMethods", NameTextCodeDefaultText = @"AR payment Methods" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_APPAYMENTMETHODS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "APPAYMENTMETHODS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.APPaymentMethods", NameTextCodeDefaultText = @"AP payment Methods" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_DocumentsBackup = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.DocumentsBackup", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.DocumentsBackup", NameTextCodeDefaultText = @"Documents Backup" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_AllowAgentInCustomersLOV = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AllowAgentInCustomersLOV", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.AllowAgentInCustomersLOV", NameTextCodeDefaultText = @"Allow Agent In Shippers/Consignees LOV" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_DataBackup = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.DataBackup", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.DataBackup", NameTextCodeDefaultText = @"Data Backup" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_TicketsSetting = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TicketsSetting", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.TicketsSetting", NameTextCodeDefaultText = @"Ticket Settings" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_ACCOUNTINGPAYMENTMETHODS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACCOUNTINGPAYMENTMETHODS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.AccountingPaymentMethods", NameTextCodeDefaultText = @"Accounting payment Methods" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_BusinessProcessQueue = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.BusinessProcessQueue", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.BusinessProcessQueue", NameTextCodeDefaultText = @"Business Process Queues" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_BusinessProcessBusinessRole = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.BusinessProcessBusinessRole", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.BusinessProcessBusinessRole", NameTextCodeDefaultText = @"Business Process Business Roles" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_BusinessProcessTeam = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.BusinessProcessTeam", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.BusinessProcessTeam", NameTextCodeDefaultText = @"Business Process Teams" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_TASKS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TASKS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Tasks", NameTextCodeDefaultText = @"Tasks" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_BatchTaskExecutionMNU = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "BatchTaskExecutionMNU", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.BatchTaskExecutions", NameTextCodeDefaultText = @"Batch Task Executions" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_SHAREDSHIPMENTS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SHAREDSHIPMENTS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.SharedShipments", NameTextCodeDefaultText = @"Shared Logistics Shipments" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_SHAREDINVOICES = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SHAREDINVOICES", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.SharedInvoices", NameTextCodeDefaultText = @"Shared Logistics Invoices" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_EXTERNALAPIS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "EXTERNALAPIS", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.ExternalAPIs", NameTextCodeDefaultText = @"External API" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_SCHEDULERS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SCHEDULERS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Schedulers", NameTextCodeDefaultText = @"Schedulers" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_TariffModule = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "TariffModule", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.TariffModule", NameTextCodeDefaultText = @"Tariffs" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_CREATETENANT = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CREATETENANT", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.CREATETENANT", NameTextCodeDefaultText = @"Create Tenant" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_ACCJORN = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACCJORN", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.ACCJORN", NameTextCodeDefaultText = @"Journal Tab" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_CacheLogMenu = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CacheLogMenu", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.CacheLogMenu", NameTextCodeDefaultText = @"Cache Log" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Occasion_OccasionType = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Occasion.OccasionType", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.General.Occasion.OccasionType", NameTextCodeDefaultText = @"Occasion Types" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_VATSettings = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.VATSettings", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.General.Features.VATSettings", NameTextCodeDefaultText = @"VAT Settings" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_General_Features_TenantAdditionalData = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "General.Features.TenantAdditionalData", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.General.Features.TenantAdditionalData", NameTextCodeDefaultText = @"Payment Gateway Definition" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_Menu_Occasions = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "Menu.Occasions", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Menu.Occasions", NameTextCodeDefaultText = @"Occasions" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_AllowCustomersInAgentsLOV = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "AllowCustomersInAgentsLOV", FeatureTypeCode = "OTH", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.AllowCustomersInAgentsLOV", NameTextCodeDefaultText = @"Allow Shippers/Consignees in Agents LOV" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_PRICESTEPS = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "PRICESTEPS", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.Menu.PriceSteps", NameTextCodeDefaultText = @"Prices Steps" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_SupportMailBoxMenu = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "SupportMailBoxMenu", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.SupportMailBoxMenu", NameTextCodeDefaultText = @"Support Mail Box" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_ACCOUNTINGTRANSFER = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "ACCOUNTINGTRANSFER", FeatureTypeCode = "AREA", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.ACCOUNTINGTRANSFER", NameTextCodeDefaultText = @"Accounting Transfer" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_JOURNALMENU = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "JOURNALMENU", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = true, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.JOURNALMENU", NameTextCodeDefaultText = @"Journal" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_VENDORGLACCOUNTSMENU = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "VENDORGLACCOUNTSMENU", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = true, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.VENDORGLACCOUNTSMENU", NameTextCodeDefaultText = @"Vendor Accounts" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_CLIENTGLACCOUNTSMENU = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "CLIENTGLACCOUNTSMENU", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = true, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.CLIENTGLACCOUNTSMENU", NameTextCodeDefaultText = @"Client Accounts" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_GLACCOUNTSMENU = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "GLACCOUNTSMENU", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = true, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.GLACCOUNTSMENU", NameTextCodeDefaultText = @"General Ledger Accounts" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_JOURNALACTIONTYPESMENU = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "JOURNALACTIONTYPESMENU", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = true, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.JOURNALACTIONTYPESMENU", NameTextCodeDefaultText = @"Journal Action Types" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

		   Feature GeneralFeature_QPDB = AddRolesAndFeaturesClass.AddFeature(new FeatureDetails() { Code = "QPDB", FeatureTypeCode = "MENU", Packagable = true, IsBusinessUnitEnabled = false, IsOld = false, IsCoreFeature = false, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, NameTextCodeCode = "General.Features.QPDB", NameTextCodeDefaultText = @"Quotes Performance Dashboard" }, FeaturesRepository, TextCodeRepository, TenantFeatures, TextCodes);

   
	    
		}

	    public void AddTableEventTypes(Dictionary<string, EventType> tenantEventTypes,EventTypeRepository EventTypeRepository,IWebFreightContext ObjectContext,List<EntityStatus> AllEntityStatuses)
	    {   
			ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault(); 
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
                ObjectTableId = GeneralObjectTable.Id,
				 
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
                ObjectTableId = GeneralObjectTable.Id,
				 
            }, EventTypeRepository, tenantEventTypes);


	    }
	
	    public void AddTableMenuButtons(Dictionary<string, MenuButton> tenantMenuButtons,Dictionary<string, MenuButtonGroup> tenantMenuButtonGroups, Dictionary<string, TextCode> textCodes,TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository, MenuButtonRepository menuButtonRepository,Dictionary<string, Feature> TenantFeatures,MenuButtonGroupRepository menuButtonGroupRepository ,IWebFreightContext ObjectContext)
	    {  
	    }

	    public void AddTableTextCodes(TextCodeRepository TextCodeRepository,FeatureRepository FeaturesRepository,Dictionary<string, Feature> TenantFeatures,Dictionary<string, TextCode> TextCodes,IWebFreightContext ObjectContext)
	    {  

		   		   //--------------> Additional TextCodes <--------------\\

 		   ObjectTable GeneralObjectTable = ObjectContext.ObjectTables.Where(d => d.Name == "General" && d.Tenant == 0).FirstOrDefault(); 

 		   TextCode GeneralTextCode_GeneralMFieldWarning = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.FieldWarning", DefaultText = "%FieldName Field is not filled",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMSubmitFailed = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.SubmitFailed", DefaultText = "Submit failed",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHQuotes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.Quotes", DefaultText = "Quotes",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHContacts = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.Contacts", DefaultText = "Contacts",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHAccounting = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.Accounting", DefaultText = "Accounting",LocalDefaultText = @"הנה''ח", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHDashboard = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.Dashboard", DefaultText = "Dashboard",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHMaintenance = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.Maintenance", DefaultText = "Maintenance",LocalDefaultText = @"תחזוקת מערכת", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCPartnersPartners = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Partners.Partners", DefaultText = "Partners",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCPartnersCustomers = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Partners.Customers", DefaultText = "Customers",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCPartnersPotentialCustomers = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Partners.PotentialCustomers", DefaultText = "Potential Customers",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCPartnersAgents = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Partners.Agents", DefaultText = "Agents",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCPartnersCustomsAgents = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Partners.CustomsAgents", DefaultText = "Customs Agents",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCPartnersShippingAgents = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Partners.ShippingAgents", DefaultText = "Shipping Agents",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCPartnersAirlines = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Partners.Airlines", DefaultText = "Airlines",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCPartnersShippingLines = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Partners.ShippingLines", DefaultText = "Shipping Lines",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCPartnersTruckers = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Partners.Truckers", DefaultText = "Truckers",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCBillingsBillings = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Billings.Billings", DefaultText = "Billings",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCBillingsIncoterms = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Billings.Incoterms", DefaultText = "Incoterms",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCBillingsPaymentTerm = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Billings.PaymentTerm", DefaultText = "Payment Term",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCBillingsCurrency = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Billings.Currency", DefaultText = "Currency",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCBillingsCurrencyRates = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Billings.CurrencyRates", DefaultText = "Currency Rates",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCBillingsVatTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Billings.VatTypes", DefaultText = "Vat Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCBillingsChargesTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Billings.ChargesTypes", DefaultText = "Charges Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCOthersOthers = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Others.Others", DefaultText = "Others",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCOthersPorts = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Others.Ports", DefaultText = "Ports",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCOthersCountries = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Others.Countries", DefaultText = "Countries",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCOthersGlobalZones = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Others.GlobalZones", DefaultText = "Global Zones",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCOthersBranches = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Others.Branches", DefaultText = "Branches",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCOthersDepartments = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Others.Departments", DefaultText = "Departments",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCOthersContacts = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Others.Contacts", DefaultText = "Contacts",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCOthersUsers = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Others.Users", DefaultText = "Users",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCOthersStates = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Others.States", DefaultText = "States",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCOthersFollowUpTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Others.FollowUpTypes", DefaultText = "FollowUp Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCOthersDocumentTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Others.DocumentTypes", DefaultText = "Document Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCOthersEventTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Others.EventTypes", DefaultText = "Event Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCOthersPackageTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Others.PackageTypes", DefaultText = "Package Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCOthersVessels = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Others.Vessels", DefaultText = "Vessels",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCSetupSetup = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Setup.Setup", DefaultText = "Setup",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBOk = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.Ok", DefaultText = "OK",LocalDefaultText = @"אישור", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBCancel = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.Cancel", DefaultText = "Cancel",LocalDefaultText = @"ביטול", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBAdd = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.Add", DefaultText = "Add",LocalDefaultText = @"הוסף", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBEdit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.Edit", DefaultText = "Edit",LocalDefaultText = @"ערוך", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBDelete = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.Delete", DefaultText = "Delete",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBClose = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.Close", DefaultText = "Close",LocalDefaultText = @"סגור", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBSave = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.Save", DefaultText = "Save",LocalDefaultText = @"שמור", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBSaveAndClose = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.SaveAndClose", DefaultText = "Save & Close",LocalDefaultText = @"שמור וסגור", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBDontSave = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.DontSave", DefaultText = "Don't Save",LocalDefaultText = @"אל תשמור", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBYes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.Yes", DefaultText = "Yes",LocalDefaultText = @"כן", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBNo = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.No", DefaultText = "No",LocalDefaultText = @"לא", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBNext = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.Next", DefaultText = "Next",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBFinish = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.Finish", DefaultText = "Finish",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBPrevious = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.Previous", DefaultText = "Previous",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMThisEntityhasunsavedchanges = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.ThisEntityhasunsavedchanges", DefaultText = "This %Entity has unsaved changes do you want to save it?",LocalDefaultText = @"%Entity זה שינויים שלא נשמרו האם ברצונך לשמור אותו?", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMContainerNumberFormatisInvalid = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.ContainerNumberFormatisInvalid", DefaultText = "Container Number Format is Invalid <Must be 4 Letters and 7 Digits>",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMContainerNumberCheckDigitiswrong = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.ContainerNumberCheckDigitiswrong", DefaultText = "Container Number Check Digit is wrong <Must be %CheckDigit>",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMAddingIsNotAvailable = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.AddingIsNotAvailable", DefaultText = "Adding new %Entity is not available at this moment please send the requested %Entity by email to: support@logitudeworld.com",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMSaving = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.Saving", DefaultText = "Saving...",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMLoading = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.Loading", DefaultText = "Loading...",LocalDefaultText = @"טוען", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCantUpdateRecord = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.CantUpdateRecord", DefaultText = "Sorry you can't update this record right now it's being updated by another user",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMFieldIsRequired = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.FieldIsRequired", DefaultText = "%FieldName Field is Required",LocalDefaultText = @"%FieldName שדה חובה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMMinMax = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.MinMax", DefaultText = "%FieldName Field must be less than %Maxlength and more than %Minlength",LocalDefaultText = @"%FieldName השדה חייב להיות קטן מ- %Maxlength וגדול מ- %Minlength", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMSubmitFaild = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.SubmitFaild", DefaultText = "Submit failed",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = true }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMEntityAlreadyExists = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.EntityAlreadyExists", DefaultText = "This %Entity already exists",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMErrorLoadingTranslations = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.ErrorLoadingTranslations", DefaultText = "Error Loading Translations",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMAccountingCurrencyIsNotSet = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.AccountingCurrencyIsNotSet", DefaultText = "Accounting Currency is not set",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralONewEntity = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.NewEntity", DefaultText = "New %Entity",LocalDefaultText = @"חדשה %Entity", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOEditEntity = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.EditEntity", DefaultText = "Edit %Entity",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOSignOut = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.SignOut", DefaultText = "Sign Out",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOUnSavedChanges = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.UnSavedChanges", DefaultText = "Unsaved Changes",LocalDefaultText = @"שינויים שלא נשמרו", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOFindMore = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.FindMore", DefaultText = "Find More",LocalDefaultText = @"מצא עוד", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOEnterDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.EnterDate", DefaultText = "Enter Date",LocalDefaultText = @"הזן תאריך", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOEnterTime = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.EnterTime", DefaultText = "Enter Time",LocalDefaultText = @"הזן זמן", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOGeneral = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.General", DefaultText = "General",LocalDefaultText = @"כללי", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOConfirm = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Confirm", DefaultText = "Confirm",LocalDefaultText = @"אישור", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOToday = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Today", DefaultText = "Today",LocalDefaultText = @"היום", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOYesterday = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Yesterday", DefaultText = "Yesterday",LocalDefaultText = @"אתמול", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOTomorrow = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Tomorrow", DefaultText = "Tomorrow",LocalDefaultText = @"מחר", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOPresent = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Present", DefaultText = "Present",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOFuture = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Future", DefaultText = "Future",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOPast = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Past", DefaultText = "Past",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOAddSession = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.AddSession", DefaultText = "Add Session",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCBillingsAccounts = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Billings.Accounts", DefaultText = "Accounts",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCPartnersVendors = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Partners.Vendors", DefaultText = "Vendors",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCPartnersWarehouses = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Partners.Warehouses", DefaultText = "Warehouses",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHGettingStarted = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.GettingStarted", DefaultText = "Getting Started",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCLocationsLocations = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Locations.Locations", DefaultText = "Locations",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBBack = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.Back", DefaultText = "Back",LocalDefaultText = @"חזרה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBAddFilter = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.AddFilter", DefaultText = "Add Filter",LocalDefaultText = @"הוסף מסנן", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBEditView = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.EditView", DefaultText = "Edit View",LocalDefaultText = @"עריכת תצוגה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBRemove = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.Remove", DefaultText = "Remove",LocalDefaultText = @"הסר", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBImport = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.Import", DefaultText = "Import",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBRetry = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.Retry", DefaultText = "Retry",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBDownloadFile = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.DownloadFile", DefaultText = "Download file",LocalDefaultText = @"הורד קובץ", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBDashBoardMoreDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.DashBoard.MoreDetails", DefaultText = "More Details",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMLoadingFromServer = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.LoadingFromServer", DefaultText = "Loading data list from server...",LocalDefaultText = @"טוען נתונים מהשרת ...", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOBilling = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Billing", DefaultText = "Billing",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOMenu = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Menu", DefaultText = "Menu",LocalDefaultText = @"תפריט", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOView = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.View", DefaultText = "View",LocalDefaultText = @"תצוגה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOAdvancedFilters = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.AdvancedFilters", DefaultText = "Advanced Filters",LocalDefaultText = @"מסננים מתקדמים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOAvailableColumns = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.AvailableColumns", DefaultText = "Available Columns",LocalDefaultText = @"עמודות זמינות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOSelectedColumns = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.SelectedColumns", DefaultText = "Selected Columns",LocalDefaultText = @"עמודות נבחרות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOAll = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.All", DefaultText = "All",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOImport = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Import", DefaultText = "Import",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOExport = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Export", DefaultText = "Export",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOAir = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Air", DefaultText = "Air",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOOcean = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Ocean", DefaultText = "Ocean",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOInland = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Inland", DefaultText = "Inland",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOAccountingReceivable = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.AccountingReceivable", DefaultText = "Accounting Receivable",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOAccountingPayables = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.AccountingPayables", DefaultText = "Accounting Payables",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOTips = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Tips", DefaultText = "Tips...",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODontShowAgain = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DontShowAgain", DefaultText = "Dont show again",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOSearch = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Search", DefaultText = "Search",LocalDefaultText = @"חפש", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOMessage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Message", DefaultText = "Message",LocalDefaultText = @"הודעה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOFilterBy = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.FilterBy", DefaultText = "Filter By",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOAccountReceivable = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.AccountReceivable", DefaultText = "Accounts Receivable",LocalDefaultText = @"לקוחות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOARAgingReport = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.ARAgingReport", DefaultText = "A/R Aging Report",LocalDefaultText = @"דוח גיול לקוחות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOTop5Debtors = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Top5Debtors", DefaultText = "Top 5 Debtors",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOAccountPayable = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.AccountPayable", DefaultText = "Accounts Payable",LocalDefaultText = @"ספקים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOAPAgingReport = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.APAgingReport", DefaultText = "A/P Aging Report",LocalDefaultText = @"דוח גיול ספקים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOTop5Creditors = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Top5Creditors", DefaultText = "Top 5 Creditors",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOTopName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.TopName", DefaultText = "Name",LocalDefaultText = @"שם", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOOutstanding = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Outstanding", DefaultText = "Outstanding",LocalDefaultText = @"מובחר", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOOverdue = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Overdue", DefaultText = "Overdue",LocalDefaultText = @"חוב בפיגור", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOInvoices = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Invoices", DefaultText = "Invoices",LocalDefaultText = @"החשבוניות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOPayments = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Payments", DefaultText = "Payments",LocalDefaultText = @"תשלומים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardActivityStatus = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.ActivityStatus", DefaultText = "Activity Status",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardShipmentsQuantity = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.ShipmentsQuantity", DefaultText = "Shipments quantity by time ",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardTimeRange = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.TimeRange", DefaultText = "Time Range",LocalDefaultText = @"טווח זמן", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardShow = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.Show", DefaultText = "Show",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardNoShipments = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.NoShipments", DefaultText = "No Shipments",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardTop5Debtors = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.Top5Debtors", DefaultText = "Top 5 Debtors Exposure",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardNoDebtors = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.NoDebtors", DefaultText = "No Debtors",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardFollowUp = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.FollowUp", DefaultText = "Follow Up",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardDueToday = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.DueToday", DefaultText = "Due Today",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardOverDue = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.OverDue", DefaultText = "Over Due",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardTotalForToday = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.TotalForToday", DefaultText = "Total For Today",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardNoFollowUps = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.NoFollowUps", DefaultText = "No Follow Ups",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardMoneyIn = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.MoneyIn", DefaultText = "Money In",LocalDefaultText = @"", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardProfitCurrency = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.ProfitCurrency", DefaultText = "Profit Currency",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardLocalCurrency = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.LocalCurrency", DefaultText = "Local Currency",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardInvoices = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.Invoices", DefaultText = "Invoices",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardPayments = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.Payments", DefaultText = "Payments",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardNoInvoices = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.NoInvoices", DefaultText = "No Invoices",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardDailySpotlight = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.DailySpotlight", DefaultText = "Daily Spotlight",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardShipments = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.Shipments", DefaultText = "Shipments",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardQuotes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.Quotes", DefaultText = "Quotes",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardNewCustomers = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.NewCustomers", DefaultText = "New Customers",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardToday = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.Today", DefaultText = "Today",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardYesterday = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.Yesterday", DefaultText = "Yesterday",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardLastWeek = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.LastWeek", DefaultText = "Last Week",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardCountries = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.Countries", DefaultText = "Countries",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardTop = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.Top", DefaultText = "Top",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardIncludeOthers = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.IncludeOthers", DefaultText = "Include Others",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardNoCountries = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.NoCountries", DefaultText = "No Countries",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardCustomers = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.Customers", DefaultText = "Customers",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardNoCustomers = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.NoCustomers", DefaultText = "No Customers",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardDirection = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.Direction", DefaultText = "Direction and Transport Mode",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardChargeWeight = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.ChargeWeight", DefaultText = "Charge. Weight",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardGrossWeight = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.GrossWeight", DefaultText = "Gross Weight",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardProfit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.Profit", DefaultText = "Profit",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardLastMonth = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.LastMonth", DefaultText = "Last Month",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardLast6Months = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.Last6Months", DefaultText = "Last 6 Months",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardLastYear = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.LastYear", DefaultText = "Last Year",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardLast2Years = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.Last2Years", DefaultText = "Last 2 Years",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardLast3Years = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.Last3Years", DefaultText = "Last 3 Years",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCPersonalSettingsPersonalSettings = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.PersonalSettings.PersonalSettings", DefaultText = "Personal Settings",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCSystemSettingsSystemSettings = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.SystemSettings.SystemSettings", DefaultText = "System Settings",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOFollowUps = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.FollowUps", DefaultText = "Follow Ups",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralONewPayment = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.NewPayment", DefaultText = "New Payment",LocalDefaultText = @"תשלום חדש", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBConfirm = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.Confirm", DefaultText = "Confirm",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBCreate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.Create", DefaultText = "Create",LocalDefaultText = @"חדש", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBLogin = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.Login", DefaultText = "Login",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBBuildBackup = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.BuildBackup", DefaultText = "Build Backup",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBDownloadBackup = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.DownloadBackup", DefaultText = "Download Backup",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMWantToDeleteThisQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.WantToDeleteThisQuery", DefaultText = "Are you sure you want to delete this query?",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMThisQueryCantBeDeleted = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.ThisQueryCantBeDeleted", DefaultText = "This query can't be deleted  (System Level Query)",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMInvalidQueryName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.InvalidQueryName", DefaultText = "Invalid query name",LocalDefaultText = @"שם לא חוקית של השאילתה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMFiltersHaveNoValues = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.FiltersHaveNoValues", DefaultText = "Filters have no values",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMQueryNameLength = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.QueryNameLength", DefaultText = "Query name length must be less than 30 charachters!",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMQueryNamecannotBeEmpty = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.QueryNamecannotBeEmpty", DefaultText = "Query name cannot be empty!",LocalDefaultText = @"שם השאילתה לא יכול להיות ריק!", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMInvalidFilterValues = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.InvalidFilterValues", DefaultText = "Invalid filter values",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMSomeFiltersHaveNoValue = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.SomeFiltersHaveNoValue", DefaultText = "Some filters have no value!",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMFillEmailAndPassword = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.FillEmailAndPassword", DefaultText = "Please fill the email and password to login!",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMNotSignedUp = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.NotSignedUp", DefaultText = "Sorry but you are not signed up in the application!",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMPressBuildBackupButton = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.PressBuildBackupButton", DefaultText = "Please press Build Backup button in order to start backup operation",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMYourDataIsReady = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.YourDataIsReady", DefaultText = "Your data is ready. Please press Download button",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMPreparingYourData = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.PreparingYourData", DefaultText = "Preparing your data, Please wait...",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMLoadingShipment = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.LoadingShipment", DefaultText = "Loading Shipment...",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMLoadingQuote = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.LoadingQuote", DefaultText = "Loading Quote...",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMLoadingInvoice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.LoadingInvoice", DefaultText = "Loading Invoice...",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMBuildingDocument = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.BuildingDocument", DefaultText = "Building document...",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMSavingDefaultCopies = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.SavingDefaultCopies", DefaultText = "Saving default Copies...",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMRefreshingDocument = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.RefreshingDocument", DefaultText = "Refreshing Document...",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMSendingMail = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.SendingMail", DefaultText = "Sending mail....",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralONotes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Notes", DefaultText = "Notes",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODeletQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DeletQuery", DefaultText = "Delete Query",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralORenameQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.RenameQuery", DefaultText = "Rename Query",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOSaveAsNewView = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.SaveAsNewView", DefaultText = "Save as New View",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOViewName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.ViewName", DefaultText = "View Name",LocalDefaultText = @"הצג את השם", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOColumns = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Columns", DefaultText = "Columns",LocalDefaultText = @"עמודות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOFilters = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Filters", DefaultText = "Filters",LocalDefaultText = @"מסננים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOAdvanceSettings = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.AdvanceSettings", DefaultText = "Advance Settings",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralONoDataFound = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.NoDataFound", DefaultText = "No Data Found",LocalDefaultText = @"לא נמצאו תוצאות מתאימות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODataFields = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DataFields", DefaultText = "Data Fields",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOSystemData = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.SystemData", DefaultText = "System Data",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralONoDebts = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.NoDebts", DefaultText = "No Debts",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOAmounts = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Amounts", DefaultText = "Amounts",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralONoCredits = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.NoCredits", DefaultText = "No Credits",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardReceivables = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.Receivables", DefaultText = "Receivables",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardDebtors = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.Debtors", DefaultText = "Debtors",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardIncome = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.Income", DefaultText = "Income",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHCustomers = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.Customers", DefaultText = "Customers",LocalDefaultText = @"Customers", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBSendRequest = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.SendRequest", DefaultText = "Send",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOCommunications = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Communications", DefaultText = "Communications",LocalDefaultText = @"תקשורת", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOImportEntities = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.ImportEntities", DefaultText = "Add %Entity to your company",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOTop10Debtors = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Top10Debtors", DefaultText = "Top 10 Debtors",LocalDefaultText = @"עשרת החייבים ביותר", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOTop10Creditors = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Top10Creditors", DefaultText = "Top 10 Creditors",LocalDefaultText = @"עשרת הזכאים ביותר", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHOperations = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.Operations", DefaultText = "Operations",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCBillingsCreditCardTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Billings.CreditCardTypes", DefaultText = "Credit Card Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCManagementManagement = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Management.Management", DefaultText = "Management",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCManagementErrorLogs = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Management.ErrorLogs", DefaultText = "ErrorLogs",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHReports = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.Reports", DefaultText = "Reports",LocalDefaultText = @"דוחות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHSharedLogistics = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.SharedLogistics", DefaultText = "Shared Logistics",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCOthersMoveTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Others.MoveTypes", DefaultText = "Move Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCOthersReports = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Others.Reports", DefaultText = "Reports",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCManagementErrorLog = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Management.ErrorLog", DefaultText = "ErrorLogs",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOAccounting = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Accounting", DefaultText = "Accounting",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHCustoms = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.Customs", DefaultText = "Customs Request",LocalDefaultText = @"בקשות מכס", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardMoneyOut = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.MoneyOut", DefaultText = "Money Out",LocalDefaultText = @"כסף יוצא", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_CustomsGeneralBAdd = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.B.Add", DefaultText = "Add",LocalDefaultText = @"הוסף", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_CustomsGeneralBOK = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.B.OK", DefaultText = "OK",LocalDefaultText = @"אישור", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_CustomsGeneralBCancel = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.B.Cancel", DefaultText = "Cancel",LocalDefaultText = @"ביטול", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_CustomsGeneralONewEntity = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.NewEntity", DefaultText = "New %Entity",LocalDefaultText = @"%Entity חדש", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHPhysicalChecks = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.PhysicalChecks", DefaultText = "Physical Checks",LocalDefaultText = @"בדיקות פיזיות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHDeclarations = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.Declarations", DefaultText = "Declarations",LocalDefaultText = @"הצהרות יבוא", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHPaymentOrders = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.PaymentOrders", DefaultText = "Payment Orders",LocalDefaultText = @"הוראות תשלום", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHCustomsMaintenance = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.CustomsMaintenance", DefaultText = "Customs Tables",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHCRM = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.CRM", DefaultText = "CRM",LocalDefaultText = @"CRM", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCOthersCustomBank = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Others.CustomBank", DefaultText = "Custom Banks",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCCRMCRM = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.CRM.CRM", DefaultText = "CRM",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOQueryColumnsEdit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.QueryColumnsEdit", DefaultText = "Query Columns Edit",LocalDefaultText = @"עריכת עמודות השאילתא", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOSaveAs = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.SaveAs", DefaultText = "Save As..",LocalDefaultText = @"..שמור כ", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOFiltersList = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.FiltersList", DefaultText = "Filters List",LocalDefaultText = @"רשימת מסננים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOSelectedFilters = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.SelectedFilters", DefaultText = "Selected Filters",LocalDefaultText = @"קריטריונים שנבחרו", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOClearAll = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.ClearAll", DefaultText = "Clear all",LocalDefaultText = @"נקה הכל", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOAddfilterstothesavedview = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Addfilterstothesavedview", DefaultText = "Add filters to the saved view",LocalDefaultText = @"הוסף למסנני התצוגה השמורה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOCreateNewView = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.CreateNewView", DefaultText = "Create New View",LocalDefaultText = @"צור פרופיל חדש", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesTables = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.Tables", DefaultText = "Tables",LocalDefaultText = @"טבלאות מכס", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCPartnersClients = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Partners.Clients", DefaultText = "Clients",LocalDefaultText = @"לקוחות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesCheckEntityTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.CheckEntityTypes", DefaultText = "Check Entity Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesCheckRepresentativeTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.CheckRepresentativeTypes", DefaultText = "Check Representative Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesSiteLookups = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.SiteLookups", DefaultText = "Site Lookups",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesCheckQueueTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.CheckQueueTypes", DefaultText = "Check Queue Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesCargoIdentifireTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.CargoIdentifireTypes", DefaultText = "Cargo Identifire Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesPhysicalCheckOperations = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.PhysicalCheckOperations", DefaultText = "Physical Check Operations",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesPhysicalCheckStatusMessages = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.PhysicalCheckStatusMessages", DefaultText = "Physical Check Status Messages",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesCountries = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.Countries", DefaultText = "Countries",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesSubCountries = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.SubCountries", DefaultText = "Sub Countries",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesCommunicationTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.CommunicationTypes", DefaultText = "Communication Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesVendorTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.VendorTypes", DefaultText = "Vendor Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesAutonomyTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.AutonomyTypes", DefaultText = "Autonomy Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesCountryGroups = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.CountryGroups", DefaultText = "Country Groups",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesEntitlementTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.EntitlementTypes", DefaultText = "Entitlement Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesGovernmentProcedureTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.GovernmentProcedureTypes", DefaultText = "Government Procedure Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesLeadDocumentTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.LeadDocumentTypes", DefaultText = "Lead Document Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesPackageMeasureQualifiers = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.PackageMeasureQualifiers", DefaultText = "Package Measure Qualifiers",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesPackingTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.PackingTypes", DefaultText = "Packing Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesSiteTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.SiteTypes", DefaultText = "Site Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesCurrencyTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.CurrencyTypes", DefaultText = "Currency Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesCustomsBookTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.CustomsBookTypes", DefaultText = "Customs Book Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesInvoiceTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.InvoiceTypes", DefaultText = "Invoice Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesItemGovernmentProcedureTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.ItemGovernmentProcedureTypes", DefaultText = "Item Government Procedure Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesPaymentTerms = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.PaymentTerms", DefaultText = "Payment Terms",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesPaymentTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.PaymentTypes", DefaultText = "Payment Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesPreferenceDocumentTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.PreferenceDocumentTypes", DefaultText = "Preference Document Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesProductNameTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.ProductNameTypes", DefaultText = "Product Name Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesSalesTaxExemptionTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.SalesTaxExemptionTypes", DefaultText = "Sales Tax Exemption Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesTariffCodeTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.TariffCodeTypes", DefaultText = "Tariff Code Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesTermsOfSaleTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.TermsOfSaleTypes", DefaultText = "Terms Of Sale Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesAttachmentTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.AttachmentTypes", DefaultText = "Attachment Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesCertificateExemptionTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.CertificateExemptionTypes", DefaultText = "Certificate Exemption Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesConfirmationTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.ConfirmationTypes", DefaultText = "Confirmation Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesMeasurmentUnits = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.MeasurmentUnits", DefaultText = "Measurment Units",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesModificationAndDiscountTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.ModificationAndDiscountTypes", DefaultText = "Modification And Discount Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesParagraphTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.ParagraphTypes", DefaultText = "Paragraph Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesTradeAgreements = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.TradeAgreements", DefaultText = "Trade Agreements",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesCustomerActivityTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.CustomerActivityTypes", DefaultText = "Customer Activity Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesOrganizationUnitTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.OrganizationUnitTypes", DefaultText = "Organization Unit Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesPaymentOrderOperationalStatuses = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.PaymentOrderOperationalStatuses", DefaultText = "Payment Order Operational Statuses",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesPaymentOrderStatuses = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.PaymentOrderStatuses", DefaultText = "Payment Order Statuses",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesPaymentOrderTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.PaymentOrderTypes", DefaultText = "Payment Order Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesPaymentProcesses = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.PaymentProcesses", DefaultText = "Payment Processes",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesBanks = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.Banks", DefaultText = "Banks",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesBranchs = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.Branchs", DefaultText = "Branchs",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesPaymentMethodStatuses = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.PaymentMethodStatuses", DefaultText = "Payment Method Statuses",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesPaymentMethodTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.PaymentMethodTypes", DefaultText = "Payment Method Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesPaymentProtestTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.PaymentProtestTypes", DefaultText = "Payment Protest Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesAddressContactStates = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.AddressContactStates", DefaultText = "Address Contact States",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesAddressPurposes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.AddressPurposes", DefaultText = "Address Purposes",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesAddressTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.AddressTypes", DefaultText = "Address Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesAuthorizedSignerPermits = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.AuthorizedSignerPermits", DefaultText = "Authorized Signer Permits",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesCities = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.Cities", DefaultText = "Cities",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesContactRoleTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.ContactRoleTypes", DefaultText = "Contact Role Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesGenders = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.Genders", DefaultText = "Genders",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesPassportTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.PassportTypes", DefaultText = "Passport Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesDeclarationStatusTypes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.DeclarationStatusTypes", DefaultText = "Declaration Status Types",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_CustomsGeneralOSendSample = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.SendSample", DefaultText = "Send Sample",LocalDefaultText = @"שלח לדוגמה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_CustomsGeneralOCustoms = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.Customs", DefaultText = "Customs",LocalDefaultText = @"מכס", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_CustomsGeneralONewPaymentOrder = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.NewPaymentOrder", DefaultText = "New Payment Order",LocalDefaultText = @"שליפת הוראת תשלום", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_CustomsGeneralOClose = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.Close", DefaultText = "Close",LocalDefaultText = @"סגור", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_CustomsGeneralOLoading = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.Loading", DefaultText = "Loading ....",LocalDefaultText = @"טוען ....", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_CustomsGeneralOSending = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.Sending", DefaultText = "Sending ....",LocalDefaultText = @"שולח ....", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_CustomsGeneralOAddRemoveColumns = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.AddRemoveColumns", DefaultText = "Add/Remove columns",LocalDefaultText = @"הוסף/מחק עמודות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_CustomsGeneralONoFiltersHaveBeenSet = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.NoFiltersHaveBeenSet", DefaultText = "No filters have been set",LocalDefaultText = @"לא הוגדרו חיתוכים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_CustomsGeneralOExportToExcel = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.ExportToExcel", DefaultText = "Export to excel ",LocalDefaultText = @"Excel הורד לאקסל", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralONewView = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.NewView", DefaultText = "New View",LocalDefaultText = @"צפייה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHSocial = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.Social", DefaultText = "Social",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_CustomsGeneralOSaving = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.Saving", DefaultText = "Saving ....",LocalDefaultText = @"שמירה ....", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_CustomsGeneralOEditMetaData = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.EditMetaData", DefaultText = "Edit Meta Data",LocalDefaultText = @"עריכת מטה דאטה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_CustomsGeneralOEditCustomDocument = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.EditCustomDocument", DefaultText = "Edit Custom Document",LocalDefaultText = @"עריכת מסמך מכס", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCOthersQuoteTemplates = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Others.QuoteTemplates", DefaultText = "Quote Templates",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCOthersCommodities = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Others.Commodities", DefaultText = "Commodities",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_CustomsGeneralOIsRequired = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Customs.General.O.IsRequired", DefaultText = "Is Required",LocalDefaultText = @"הוא נדרש", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCOthersExternalSystemTablesCodes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Others.ExternalSystemTablesCodes", DefaultText = "External Tables",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOCustomsImport = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.CustomsImport", DefaultText = "Customs Import",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODashBoardCountriesNoDrop = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DashBoard.CountriesNoDrop", DefaultText = "The data viewed don't include drop and domestic/inland shipments",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCPartnersShippersAndConsignees = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Partners.ShippersAndConsignees", DefaultText = "Shippers and Consignees",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHCustomsRequestsSheets = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.CustomsRequestsSheets", DefaultText = "Requests Sheets",LocalDefaultText = @"גיליון בקשות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHDocuments = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.Documents", DefaultText = "Documents",LocalDefaultText = @"מסמכים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCCustomCustoms = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Custom.Customs", DefaultText = "Customs",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCCSMCustomsTables = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.CSM.CustomsTables", DefaultText = "Customs Tables",LocalDefaultText = @"מכס סגור לוח", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOViewCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.ViewCode", DefaultText = "View Code",LocalDefaultText = @"צג קוד", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBAddDocumentType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.AddDocumentType", DefaultText = "Documents",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOImportDocumentType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.ImportDocumentType", DefaultText = "Add Document To List",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCOthersDocumentFolders = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Others.DocumentFolders", DefaultText = "Document Folders",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBSaveAs = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.SaveAs", DefaultText = "Save as",LocalDefaultText = @"שמירה כ", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHImporters = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.Importers", DefaultText = "LogBox",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralONewPotentialCustomer = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.NewPotentialCustomer", DefaultText = "New Potential Customer",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCCRMInboundEmail = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.CRM.InboundEmail", DefaultText = "InboundEmails",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCOthersCustomerTenantAccesses = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Others.CustomerTenantAccesses", DefaultText = "Importers Tenants",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCOthersCustomerTenantAccessRequests = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Others.CustomerTenantAccessRequests", DefaultText = "Customer Tenant Access Requests",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHFullAccounting = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.FullAccounting", DefaultText = "Full Accounting",LocalDefaultText = @"הנהלת חשבונות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHActivationWizard = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.ActivationWizard", DefaultText = "Activation Wizard",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCPartnersParticipants = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Partners.Participants", DefaultText = "Participants",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCOthersAirlineStatistics = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Others.AirlineStatistics", DefaultText = "Airline Statistics",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHParticipants = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.Participants", DefaultText = "Participants",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHAirlineStatistics = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.AirlineStatistics", DefaultText = "Airline Statistics",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCOthersHybridTenantStates = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Others.HybridTenantStates", DefaultText = "Hybrid Tenant States",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHTicket = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.Ticket", DefaultText = "Tickets",LocalDefaultText = @"Tickets", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCOthersLogitudeMessagesTransmissionLog = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Others.LogitudeMessagesTransmissionLog", DefaultText = "Logitude Messages Transmission Log",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHAirlineDashboard = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.AirlineDashboard", DefaultText = "Dashboard",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHLogitudeMessagesTransmissionLog = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.LogitudeMessagesTransmissionLog", DefaultText = "Messages Log",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCAccountingAccounting = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Accounting.Accounting", DefaultText = "Accounting",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralBAddDocumentVersion = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.AddDocumentVersion", DefaultText = "Add Version",LocalDefaultText = @"גרסה חדשה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralBReconcile = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.Reconcile", DefaultText = "Reconcile",LocalDefaultText = @"התאמה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralBSaveAsDraft = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.SaveAsDraft", DefaultText = "Save as Draft",LocalDefaultText = @"שמור טיוטה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralBAdjust = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.Adjust", DefaultText = "Adjust",LocalDefaultText = @"אישור הפרשים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralBAutomaticReconcile = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.AutomaticReconcile", DefaultText = "AutomaticReconcile",LocalDefaultText = @"התאמה אוטומטית", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralBView = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.View", DefaultText = "View",LocalDefaultText = @"צפייה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralBCancelReconciliation = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.CancelReconciliation", DefaultText = "Cancel Reconciliation",LocalDefaultText = @"בטל התאמה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralORefDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.RefDate", DefaultText = "Ref. Date",LocalDefaultText = @"תאריך אסמכתא", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOJournalNo = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.JournalNo", DefaultText = "Journal No.",LocalDefaultText = @"מספר פקודה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOTransactionNo = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.TransactionNo", DefaultText = "Transaction No.",LocalDefaultText = @"מספר תנועה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOAccountingDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AccountingDate", DefaultText = "Accounting Date",LocalDefaultText = @"תאריך חשבונאי", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralODueDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.DueDate", DefaultText = "Due Date",LocalDefaultText = @"תאריך לתשלום", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOOriginalAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.OriginalAmount", DefaultText = "Original Amount",LocalDefaultText = @"מקורף סכום מקורי", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOOpenAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.OpenAmount", DefaultText = "Open Amount",LocalDefaultText = @"יתרת פתיחה לתקופה המוצגת", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOAmountToReconcile = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AmountToReconcile", DefaultText = "Amount to Reconcile",LocalDefaultText = @"סכום להתאמה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOReconciliationAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ReconciliationAmount", DefaultText = "Reconciliation Amount",LocalDefaultText = @"סכום התאמה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOReconciliationNo = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ReconciliationNo", DefaultText = "Reconciliation No.",LocalDefaultText = @"התאמה מס'", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralORef1 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Ref1", DefaultText = "Ref. 1",LocalDefaultText = @"אסמכתא 1", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralORef2 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Ref2", DefaultText = "Ref. 2",LocalDefaultText = @"אסמכתא 2", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralORef3 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Ref3", DefaultText = "Ref. 3",LocalDefaultText = @"אסמכתא 3", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralONotes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Notes", DefaultText = "Notes",LocalDefaultText = @"הערות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOTotals = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Totals", DefaultText = "Totals",LocalDefaultText = @"סך הכל", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralODetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Details", DefaultText = "Details",LocalDefaultText = @"פרטים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOCreateDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.CreateDate", DefaultText = "Create Date",LocalDefaultText = @"תאריך יצירה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOCurrency = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Currency", DefaultText = "Currency",LocalDefaultText = @"מטבע", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOSource = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Source", DefaultText = "Source",LocalDefaultText = @"מקור", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralONewEntity = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NewEntity", DefaultText = "New %Entity",LocalDefaultText = @"%Entity חדש", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOClose = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Close", DefaultText = "Close",LocalDefaultText = @"סגור", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOLoading = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Loading", DefaultText = "Loading ....",LocalDefaultText = @"טוען ....", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOSending = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Sending", DefaultText = "Sending ....",LocalDefaultText = @"שולח ....", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOAddRemoveColumns = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AddRemoveColumns", DefaultText = "Add/Remove columns",LocalDefaultText = @"הוסף/מחק עמודות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralONoFiltersHaveBeenSet = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NoFiltersHaveBeenSet", DefaultText = "No filters have been set",LocalDefaultText = @"לא הוגדרו חיתוכים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOExportToExcel = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ExportToExcel", DefaultText = "Export to excel ",LocalDefaultText = @"Excel הורד לאקסל", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOSaving = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Saving", DefaultText = "Saving ....",LocalDefaultText = @"שמירה ....", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOPlsWait = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.PlsWait", DefaultText = "Please wait a moment ....",LocalDefaultText = @"נא להמתין רגע ....", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOEditMetaData = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.EditMetaData", DefaultText = "Edit Meta Data",LocalDefaultText = @"עריכת מטה דאטה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOIsRequired = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.IsRequired", DefaultText = "Is Required",LocalDefaultText = @"הוא נדרש", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOFieldForTableIsRequired = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.FieldForTableIsRequired", DefaultText = "%FieldName in %TableName %EntityReference is Required",LocalDefaultText = @"%FieldName ב- %TableName %EntityReference הוא חובה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOObjectTables = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ObjectTables", DefaultText = "Object Tables",LocalDefaultText = @"טבלאות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralORequiredFields = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.RequiredFields", DefaultText = "Required Fields",LocalDefaultText = @"שדות חובה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOWrongEntityName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.WrongEntityName", DefaultText = "Entity name is wrong",LocalDefaultText = @"שם הישות שגוי", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOAddRemoveRequiredFields = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AddRemoveRequiredFields", DefaultText = "Add / Remove Required Fields",LocalDefaultText = @"הוספה / הסרה של שדות חובה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOSelectObjectTable = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.SelectObjectTable", DefaultText = "You must select an ObjectTable",LocalDefaultText = @"עליך לבחור בלוח אובייקט", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOViewCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ViewCode", DefaultText = "View Code",LocalDefaultText = @"תצוגת קוד", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOExisted = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Existed", DefaultText = "Existed",LocalDefaultText = @"קיים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOAll = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.All", DefaultText = "All",LocalDefaultText = @"הכל", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOFullAccounting = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.FullAccounting", DefaultText = "Full Accounting",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOOldValue = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.OldValue", DefaultText = "Old value: ",LocalDefaultText = @", ערך קודם: ", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralONewValue = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NewValue", DefaultText = ", New value: ",LocalDefaultText = @", ערך חדש: ", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOValueLong = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ValueLong", DefaultText = "Value too long",LocalDefaultText = @"ערך ארוך מדי", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOValueShort = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ValueShort", DefaultText = "Value too short",LocalDefaultText = @"ערך קצר מדי", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOValueWrong = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ValueWrong", DefaultText = "Wrong value",LocalDefaultText = @"ערך שגוי", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOUncompletedRecoQuestion = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.UncompletedRecoQuestion", DefaultText = "There is an uncompleted reconciliation, do you want to complete it?",LocalDefaultText = @"קיימת טיוטת התאמה, האם ברצונך להשלים אותה?", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOUncompletedReconciliation = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.UncompletedReconciliation", DefaultText = "Uncompleted Reconciliation",LocalDefaultText = @"טיוטת התאמה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOSavingAsDraft = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.SavingAsDraft", DefaultText = "Saving as a draft",LocalDefaultText = @"שמירת טיוטה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralONoLinesChosen = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NoLinesChosen", DefaultText = "No lines were chosen",LocalDefaultText = @"לא סומנה אף שורה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOTotalMustZero = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.TotalMustZero", DefaultText = "Total amount must be zero",LocalDefaultText = @"סך הכל צריל להיות אפס", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOReconcileError = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ReconcileError", DefaultText = "Reconcile Error",LocalDefaultText = @"שגיאת התאמה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOPermissionError = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.PermissionError", DefaultText = "Permission Error",LocalDefaultText = @"שגיאת הרשאה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralONoPermission = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NoPermission", DefaultText = "You don't have permission to perform this action",LocalDefaultText = @"אין לך הרשאה לבצע את הפעולה הזאת", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOAutomaticRecoQuestion = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AutomaticRecoQuestion", DefaultText = "Any draft reconciliation will be deleted, do you want to proceed?",LocalDefaultText = @"טיוטת התאמה תימחק, האם ברצונך להמשיך?", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOAutomaticReconciliation = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AutomaticReconciliation", DefaultText = "Automatic Reconciliation",LocalDefaultText = @"התאמה אוטומטית", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOCancelRecoQuestion = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.CancelRecoQuestion", DefaultText = "The reconciliation will be cancelled and deleted, do you want to proceed?",LocalDefaultText = @"ההתאמה תבוטל ותימחק, האם ברצונך להמשיך?", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOCancelReconciliation = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.CancelReconciliation", DefaultText = "Cancel Reconciliation",LocalDefaultText = @"ביטול התאמה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOReconcileSearchHint = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ReconcileSearchHint", DefaultText = "Search Journal No./References",LocalDefaultText = @"חפש לפי מס' פקודה/אסמכתאות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOEnterYear = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.EnterYear", DefaultText = "Please enter a year",LocalDefaultText = @"נא להקליד שנה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralBSaveAndNew = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.SaveAndNew", DefaultText = "Save And New",LocalDefaultText = @"שמירה וחדש", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralBAdd = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.Add", DefaultText = "Add",LocalDefaultText = @"הוסף", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralBOK = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.OK", DefaultText = "OK",LocalDefaultText = @"אישור", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralBCancel = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.Cancel", DefaultText = "Cancel",LocalDefaultText = @"ביטול", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralBClose = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.Close", DefaultText = "Close",LocalDefaultText = @"סגור", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralBBrowse = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.Browse", DefaultText = "Browse",LocalDefaultText = @"אישור", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralBSearch = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.Search", DefaultText = "Search",LocalDefaultText = @"חפש", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralBEdit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.Edit", DefaultText = "Edit",LocalDefaultText = @"ערוכה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralBEvents = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.B.Events", DefaultText = "Events",LocalDefaultText = @"אירועים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHClaims = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.Claims", DefaultText = "Claim",LocalDefaultText = @"תביעות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCBillingsChargesGroups = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Billings.ChargesGroups", DefaultText = "Charges Groups",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBContinue = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.Continue", DefaultText = "Continue",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBApprove = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.Approve", DefaultText = "Approve",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOAccountingTransfer = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.AccountingTransfer", DefaultText = "Accounting Transfer",LocalDefaultText = @"העברה בהנה''ח", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOSettings = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Settings", DefaultText = "Settings",LocalDefaultText = @"הגדרות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOARInvoices = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.ARInvoices", DefaultText = "AR/ Invoices",LocalDefaultText = @"חשבונית לקוח", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOAPInvoices = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.APInvoices", DefaultText = "AP/ Invoices",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOARPayments = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.ARPayments", DefaultText = "AR/ Payments",LocalDefaultText = @"קבלת לקוח", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOViews = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Views", DefaultText = "Views",LocalDefaultText = @"תצוגות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralONew = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.New", DefaultText = "New",LocalDefaultText = @"חדש", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralONewConsolidation = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.NewConsolidation", DefaultText = "New consolidation invoice",LocalDefaultText = @"חשבונית מרכזת חדשה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralONewConsolidationCredit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.NewConsolidationCredit", DefaultText = "New consolidation credit note",LocalDefaultText = @"זיכוי מרכז חדש", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOMyViews = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.MyViews", DefaultText = "My Views",LocalDefaultText = @"תצוגה שלי", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOAccountingSettings = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.AccountingSettings", DefaultText = "Accounting Settings",LocalDefaultText = @"הגדרות הנה''ח", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOAccountingSystem = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.AccountingSystem", DefaultText = "Accounting System",LocalDefaultText = @"הגדרות הנה''ח", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOSATInterfaceSettings = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.SATInterfaceSettings", DefaultText = "SAT (Mexico) Interface Settings",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralONewTransfer = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.NewTransfer", DefaultText = "New Transfer",LocalDefaultText = @"העברה חדשה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralORecalculateExternalIDs = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.RecalculateExternalIDs", DefaultText = "Recalculate External IDs",LocalDefaultText = @"חישוב מזהים חיצונים מחדש", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOAccountReceivableInvoices = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.AccountReceivableInvoices", DefaultText = "Account Receivable Invoices",LocalDefaultText = @"קבלות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOManageReceivableInvoices = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.ManageReceivableInvoices", DefaultText = "Allows to issue invoices to your clients",LocalDefaultText = @"אפשר להנפיק חשבוניות ללקוחות שלך", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOAccountReceivablePayments = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.AccountReceivablePayments", DefaultText = "Account Receivable Payments",LocalDefaultText = @"חשבוניות לקוח", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOAccountPayableInvoices = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.AccountPayableInvoices", DefaultText = "Account Payable Invoices",LocalDefaultText = @"", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOManagePayableInvoices = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.ManagePayableInvoices", DefaultText = "Manage the invoices that you receive from vendors.",LocalDefaultText = @"נהל את החשבוניות שקיבלת מהספקים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOAccountPayablePayments = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.AccountPayablePayments", DefaultText = "Account Payable Payments",LocalDefaultText = @"", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOManagePayablePayments = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.ManagePayablePayments", DefaultText = "Manage your clients' payments, and match invoices to payments and payments to invoices.",LocalDefaultText = @"נהל את הקבלות של הלקוחות שלך, והתאם חשבוניות לקבלות וקבלות לחשבוניות.", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOManageReceivablePayments = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.ManageReceivablePayments", DefaultText = "Manage your clients' payments, and match invoices to payments and payments to invoices. You can now open a pre-payment which you can close when you issue an invoice for that amount. The invoice status is updated to 'Paid' by closing the amount against a payment",LocalDefaultText = @"נהל את הקבלות של הלקוחות שלך, והתאם חשבוניות לקבלות ולהיפך. כעת תוכל לקבלה מראש ששתיסגר עם הפקת חשבונית עבור סכום זה. סטטוס החשבונית מתעדכן ל- 'שולם' על ידי סגירת הסכום כנגד קבלה.", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCOthersFBLStock = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Others.FBLStock", DefaultText = "FBL Stock",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHTimeManagement = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.TimeManagement", DefaultText = "Time Management",LocalDefaultText = @"Time Management", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBLoadingcompleted = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.Loadingcompleted", DefaultText = "Loading data list completed successfully.",LocalDefaultText = @"טעינת נתונים בוצעה בהצלחה.", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBErroroccured = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.Erroroccured", DefaultText = "Error occured while loading data!",LocalDefaultText = @"שגיאה בטעינת נתונים !", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBExportingDataToExcel = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.ExportingDataToExcel", DefaultText = "Exporting View Data List To Excel File",LocalDefaultText = @"מייצא נתונים לקובץ אקסל", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOMy = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.My", DefaultText = "My %entity",LocalDefaultText = @"%entity שלי", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOErrorsFound = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.ErrorsFound", DefaultText = "Errors Found",LocalDefaultText = @"שגיאות שנמצאו", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralONoMoreResult = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.NoMoreResult", DefaultText = "No More Results Found",LocalDefaultText = @"לא נמצאו תוצאות נוספות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOPckgNotIncluded = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.PckgNotIncluded", DefaultText = "Your package doesn't include this module..",LocalDefaultText = @"החבילה שלך לא כוללת את המודול הזה..", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralONoData = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.NoData", DefaultText = "No Data",LocalDefaultText = @"לא נמצאו נתונים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOViewAll = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.ViewAll", DefaultText = "View All",LocalDefaultText = @"הצג הכול", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOInvalidInput = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.InvalidInput", DefaultText = "Invalid Input",LocalDefaultText = @"הערך שהוזן שגוי", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesCouriersVat = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.CouriersVat", DefaultText = "Couriers Vat",LocalDefaultText = @"רשימת בלדרים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHDocumentsFiling = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.DocumentsFiling", DefaultText = "Documents",LocalDefaultText = @"מסמכים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCCRMIntegrationSystemsSetting = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.CRM.IntegrationSystemsSetting", DefaultText = "Integration Systems Setting",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBClickToAddText = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.ClickToAddText", DefaultText = "Click to add text",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHFilingInbox = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.FilingInbox", DefaultText = "Filing Inbox",LocalDefaultText = @"Filing Inbox", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHCrossDocks = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.CrossDocks", DefaultText = "Cross Docks",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBClear = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.Clear", DefaultText = "Clear",LocalDefaultText = @"נקה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMMax = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.M.Max", DefaultText = "%FieldName Field must be less than %Maxlength",LocalDefaultText = @"%FieldName השדה חייב להיות קטן מ- %Maxlength", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOAPPayments = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.APPayments", DefaultText = "AP/ Payments",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHDeclarationCargoSplits = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.DeclarationCargoSplits", DefaultText = "Cargo Splits",LocalDefaultText = @"בקשות פיצול מטען", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOFalse = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.False", DefaultText = "False",LocalDefaultText = @"שגוי", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOConnectedToGLA = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ConnectedToGLA", DefaultText = "Can’t change the chart of account, there are GL Accounts connected to it.",LocalDefaultText = @"לא ניתן לשנות סוג קבוצת מאזן, ישנם כרטיסים המחוברים לקבוצה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralORevaluationDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.RevaluationDate", DefaultText = "Revaluation Date",LocalDefaultText = @"תאריך שערוך", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOGLAccountForRevaluation = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.GLAccountForRevaluation", DefaultText = "Revaluation's GL Account:",LocalDefaultText = @"כרטיס הפרשי שער", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOGLAccountsforrevaluation = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.GLAccountsforrevaluation", DefaultText = "GLAccounts for revaluation:",LocalDefaultText = @"כרטיסים לשערוך", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralODefaultGLAccount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.DefaultGLAccount", DefaultText = "Default GL Account",LocalDefaultText = @"כרטיסי ברירת מחדל", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOChartOfAccount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ChartOfAccount", DefaultText = "Chart Of Account:",LocalDefaultText = @"קבוצת מאז", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOAccount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Account", DefaultText = "Account:",LocalDefaultText = @"חשבון", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOJournals = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Journals", DefaultText = "Journals",LocalDefaultText = @"פקודת יומן", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOJournalLines = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.JournalLines", DefaultText = "Journal Lines",LocalDefaultText = @"שורות פקודת יומן", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralORevaluationAccountReq = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.RevaluationAccountReq", DefaultText = "The field Revaluation’s GL Account is reqired",LocalDefaultText = @" שדה כרטיסים לשערוך הינו שדה חובה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOChooseGLAccount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ChooseGLAccount", DefaultText = "You must choose GL Accounts for revaluation",LocalDefaultText = @"חובה לבחור כרטיסים לשערוך", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOMain = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Main", DefaultText = "Main",LocalDefaultText = @"ראשי", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOReceivables = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Receivables", DefaultText = "Receivables",LocalDefaultText = @"לקוחות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOPayables = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Payables", DefaultText = "Payables",LocalDefaultText = @"ספקים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOBanks = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Banks", DefaultText = "Banks",LocalDefaultText = @"בנקים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOMisc = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Misc", DefaultText = "Misc",LocalDefaultText = @"שונות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOMainQueries = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.MainQueries", DefaultText = "Main Queries",LocalDefaultText = @"שאילתות ראשיות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralORecentGLAccounts = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.RecentGLAccounts", DefaultText = "Recent GL Accounts",LocalDefaultText = @"כרטיסים אחרונים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOGLAccount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.GLAccount", DefaultText = "GL Account",LocalDefaultText = @"כרטיסים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOUser = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.User", DefaultText = "User",LocalDefaultText = @"משתמש", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOAccountManager = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AccountManager", DefaultText = "Account Manager",LocalDefaultText = @"מנהל חשבון", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOSearch = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Search", DefaultText = "Search",LocalDefaultText = @"חיפוש", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralONew = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.New", DefaultText = "New",LocalDefaultText = @"חדש", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOAccountNoName = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AccountNoName", DefaultText = "Account No. / Name",LocalDefaultText = @"מספר חשבון / שם חשבון", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOCustomersQueries = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.CustomersQueries", DefaultText = "Customers Queries",LocalDefaultText = @"שאילתות לקוחות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralORecentCustomers = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.RecentCustomers", DefaultText = "Recent Customers",LocalDefaultText = @"לקוחות אחרונים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOAgingGraph = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AgingGraph", DefaultText = "Aging Graph",LocalDefaultText = @"גרף גיול", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOTop10Debtors = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Top10Debtors", DefaultText = "Top 10 Debtors",LocalDefaultText = @"10 החייבים ביותר", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOTimeRange = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.TimeRange", DefaultText = "Time Range",LocalDefaultText = @"טווח זמן", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOLastXMonth = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.LastXMonth", DefaultText = "Last #number Month",LocalDefaultText = @"#number חודשים אחרונים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOBalanceDue = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.BalanceDue", DefaultText = "Balance Due",LocalDefaultText = @"יתרה חייבת", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOAccountingBalance = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AccountingBalance", DefaultText = "Accounting Balance",LocalDefaultText = @"יתרה חשבונאית", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralONewInvoice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NewInvoice", DefaultText = "New General Invoice",LocalDefaultText = @"חשבונית כללית חדשה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralONewCreditNote = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NewCreditNote", DefaultText = "New Credit Note",LocalDefaultText = @"זיכוי חדש", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOCollector = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Collector", DefaultText = "Collector",LocalDefaultText = @"גובה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOSalesman = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Salesman", DefaultText = "Salesman",LocalDefaultText = @"איש מכירות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOBalanceinlocalcurrency = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Balanceinlocalcurrency", DefaultText = "Balance in local currency",LocalDefaultText = @"יתרה ב", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOdueBalance = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.dueBalance", DefaultText = "Due balance",LocalDefaultText = @" יתרה חייבת", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralONewAccount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NewAccount", DefaultText = "New Account",LocalDefaultText = @"חשבון חדש", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOCustomers = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Customers", DefaultText = "Customers",LocalDefaultText = @"לקוחות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOGeneralInvoice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.GeneralInvoice", DefaultText = "Invoices",LocalDefaultText = @" החשבוניות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O ", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOPayments = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Payments", DefaultText = "Payments",LocalDefaultText = @"תשלומים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOVendorsQueries = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.VendorsQueries", DefaultText = "Vendors Queries",LocalDefaultText = @"שאילתות ספקים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralORecentVendors = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.RecentVendors", DefaultText = "Recent Vendors",LocalDefaultText = @"ספקים אחרונים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOVendors = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Vendors", DefaultText = "Vendors",LocalDefaultText = @"ספקים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOTransactions = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Transactions", DefaultText = "Transactions",LocalDefaultText = @"תנועות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOReconcile = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Reconcile", DefaultText = "Reconcile",LocalDefaultText = @"התאם", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOCreateDatefrom = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.CreateDatefrom", DefaultText = "Create date from",LocalDefaultText = @"תאריך יצירה מ", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralODatefrom = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Datefrom", DefaultText = "Date from",LocalDefaultText = @"מתאריך", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOTo = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.To", DefaultText = "To",LocalDefaultText = @"עד", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOJournalNoRef = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.JournalNoRef", DefaultText = "Journal No. / Ref.",LocalDefaultText = @"מספר פקודת יומן/ אסמכתא", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOWithAttachedAccounts = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.WithAttachedAccounts", DefaultText = "With Attached Accounts",LocalDefaultText = @"לכלול כרטיסים מקושרים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOTotalInLocalCurrency = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.TotalInLocalCurrency", DefaultText = "Total In Local Currency",LocalDefaultText = @"סה”כ במטבע מקומי", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOTotalInCurrencies = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.TotalInCurrencies", DefaultText = "Total In Currencies",LocalDefaultText = @"סך הכל במטבעות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOWithSplittedbycurrency = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.WithSplittedbycurrency", DefaultText = "With Splitted by currency GL Accounts",LocalDefaultText = @"לכלול כרטיסי פיצול לפי מטבע", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOOpenBalanceByCurrency = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.OpenBalanceByCurrency", DefaultText = "Open Balance By Currency",LocalDefaultText = @"מאזן פתוח לפי מטבע", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOBankQueries = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.BankQueries", DefaultText = "Bank Queries",LocalDefaultText = @"שאילתות בנקים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralORecentDeposits = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.RecentDeposits", DefaultText = "Recent Deposits",LocalDefaultText = @"הפקדות אחרונות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOCashbookStatus = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.CashbookStatus", DefaultText = "Cashbook Status",LocalDefaultText = @"סטטוס קופה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralODeposits = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Deposits", DefaultText = "Deposits",LocalDefaultText = @"הפקדות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralQAllDeposits = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.Q.AllDeposits", DefaultText = "All Deposits",LocalDefaultText = @"כל ההפקדות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOCashbooks = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Cashbooks", DefaultText = "Cashbooks",LocalDefaultText = @"קופות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOCash = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Cash", DefaultText = "Cash",LocalDefaultText = @"מזומן", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOCheque = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Cheque", DefaultText = "Cheque",LocalDefaultText = @"המחאה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOpostdated = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.postdated", DefaultText = "Postdated",LocalDefaultText = @"דחוי", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOAllCashbook = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AllCashbook", DefaultText = "All Cashbook",LocalDefaultText = @"כל הקופות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOBankAccounts = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.BankAccounts", DefaultText = "Bank Accounts",LocalDefaultText = @"חשבונות בנק", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOAllBankAccounts = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AllBankAccounts", DefaultText = "All Bank Accounts",LocalDefaultText = @"כל חשבונות הבנק", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralODate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Date", DefaultText = "Date",LocalDefaultText = @"תאריך", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOBanksQuery = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.BanksQuery", DefaultText = "Banks Query",LocalDefaultText = @"שאילתות בנק", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralODepositNoChequeNo = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.DepositNoChequeNo", DefaultText = "Deposit No. / Cheque No.",LocalDefaultText = @"מספר הפקדה / מספר המחאה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralODepositDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.DepositDetails", DefaultText = "Deposit Details",LocalDefaultText = @"פרטי הפקדה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralONewDeposit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NewDeposit", DefaultText = "New Deposit",LocalDefaultText = @"הפקדה חדשה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOCashCheques = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.CashCheques", DefaultText = "Cash Cheques",LocalDefaultText = @"המחאות מזומן", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOpostdatedCheques = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.postdatedCheques", DefaultText = "Postdated Cheques",LocalDefaultText = @"המחאות דחויות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOCashbookTotal = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.CashbookTotal", DefaultText = "Cashbook Total",LocalDefaultText = @"סך הכל", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralODepositAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.DepositAmount", DefaultText = "Deposit Amount",LocalDefaultText = @"סכום הפקדה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOnoChequestodeposit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.noChequestodeposit", DefaultText = "There is no Cheques to deposit",LocalDefaultText = @"אין פיקדון להפקדה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOInCashbook = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.InCashbook", DefaultText = "In Cashbook",LocalDefaultText = @"בתוך הספר במזומן", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOInBank = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.InBank", DefaultText = "In Bank",LocalDefaultText = @"בבנק", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOChequeNoBankAccount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ChequeNoBankAccount", DefaultText = "Cheque No. / Bank Account",LocalDefaultText = @"בדוק מספר / חשבון בנק", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOCancelApproval = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.CancelApproval", DefaultText = "Cancel Approval",LocalDefaultText = @"ביטול אישור", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOSaveasdraft = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Saveasdraft", DefaultText = "Save as draft",LocalDefaultText = @"שמור כטיוטא", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOApprove = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Approve", DefaultText = "Approve",LocalDefaultText = @"לאשר", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralONewCashbook = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NewCashbook", DefaultText = "New Cashbook",LocalDefaultText = @"קופה חדשה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralONewBankAccounts = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NewBankAccounts", DefaultText = "New Bank Account",LocalDefaultText = @"חשבון בנק חדש", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralONewJournal = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NewJournal", DefaultText = "New Journal",LocalDefaultText = @"פקודת יומן חדשה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOCredit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Credit", DefaultText = "Credit",LocalDefaultText = @"זכות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralODebit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Debit", DefaultText = "Debit",LocalDefaultText = @"חובה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOExchangeRatesetbysystem = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ExchangeRatesetbysystem", DefaultText = "The Exchange Rate set by system",LocalDefaultText = @"שער החליפין נקבע על ידי המערכת", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOExchangeRatesetbyuser = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ExchangeRatesetbyuser", DefaultText = "The Exchange Rate set by user",LocalDefaultText = @"שער החליפין שנקבע על ידי המשתמש", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOReference = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Reference", DefaultText = "Reference",LocalDefaultText = @"אסמכתא", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOAutomaticReconcile = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AutomaticReconcile", DefaultText = "Automatic Reconcile",LocalDefaultText = @"התאם אוטומטית", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOAccordingto = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Accordingto", DefaultText = "According to",LocalDefaultText = @"לפי", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralODifference = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Difference", DefaultText = "Difference",LocalDefaultText = @"הפרש", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOAdjust = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Adjust", DefaultText = "Adjust",LocalDefaultText = @"התאם ", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOEquals = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Equals", DefaultText = "Equals",LocalDefaultText = @"שווים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralONotEqual = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NotEqual", DefaultText = "Not Equal",LocalDefaultText = @"לא שווה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOLargerThan = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.LargerThan", DefaultText = "Larger Than",LocalDefaultText = @"גדול מ", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOLessThan = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.LessThan", DefaultText = "Less Than",LocalDefaultText = @"פחות מ", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOLessThanOrEqual = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.LessThanOrEqual", DefaultText = "Less Than Or Equal",LocalDefaultText = @"פחות מ או שווה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOGreaterThanOrEqual = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.GreaterThanOrEqual", DefaultText = "Greater Than Or Equal",LocalDefaultText = @"גדול או שווה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOReconciled = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Reconciled", DefaultText = "Reconciled",LocalDefaultText = @"התואם", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOReconciliation = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Reconciliation", DefaultText = "Reconciliation",LocalDefaultText = @"התאמה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOwascreatedsuccessfully = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.wascreatedsuccessfully", DefaultText = "was created successfully",LocalDefaultText = @"נוצרה בהצלחה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOcashbookDetails = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.cashbookDetails", DefaultText = "Cashbook details",LocalDefaultText = @"נוצרה בהצלחה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Amount", DefaultText = "Amount",LocalDefaultText = @"סכום", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralONoChequesintheCashbook = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NoChequesintheCashbook", DefaultText = "No Cheques in the Cashbook",LocalDefaultText = @"אין צ'קים בקופות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralONewPage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NewPage", DefaultText = "New Bank Page",LocalDefaultText = @"דף בנק חדש", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralODifferentCurrencies = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.DifferentCurrencies", DefaultText = "The payment currency does not match to the pay to GL Account Currency ",LocalDefaultText = @"המטבע של התשלום לא תואם למטבע כרטיס הנה”ח", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOPaymentChequeExist = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.PaymentChequeExist", DefaultText = "There is a payment cheque with the same number",LocalDefaultText = @"קיימת המחאה עם מספר זהה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralONoChequeCounter = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NoChequeCounter", DefaultText = "The cheque counter did not defined for the chosen bank ",LocalDefaultText = @"מונה המחאות לא הוגדר עבור חשבון הבנק הנבחר", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOLine = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Line", DefaultText = "in line",LocalDefaultText = @"שורה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOManageReconciliation = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ManageReconciliation", DefaultText = "Manage reconciliations",LocalDefaultText = @"ניהול התאמות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOZeroDeposit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ZeroDeposit", DefaultText = "The deposit amount must be bigger than zero",LocalDefaultText = @"הסכום להפקדה חייב להיות גדול מאפס", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOAllFieldsRequired = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AllFieldsRequired", DefaultText = "All Fields Required",LocalDefaultText = @"כל השדות נדרשים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOCurrencydifferent = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Currencydifferent", DefaultText = "The currency of the bank account and the cashbook is different",LocalDefaultText = @"המטבע של חשבון הבנק והקופה שונים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOnoCashICashbook = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.noCashICashbook", DefaultText = "There are no cash in the cashbook",LocalDefaultText = @"אין מזומנים בקופה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOnoChequesinCashbook = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.noChequesinCashbook", DefaultText = "There are no cheques in the cashbook",LocalDefaultText = @"אין המחאות בקופה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOJournalNotValid = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.JournalNotValid", DefaultText = "Journal is not valid",LocalDefaultText = @"פקודת יומן לא תקפה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOCurrentCurrency = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.CurrentCurrency", DefaultText = "Current currency is",LocalDefaultText = @"נבחר מטבע", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOButAccountCurrencyDifferent = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ButAccountCurrencyDifferent", DefaultText = "But account currency is different",LocalDefaultText = @"ומטבע הכרטיס הוא", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOGLAccountIs = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.GLAccountIs", DefaultText = "Account is ",LocalDefaultText = @"כרטיס ", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOGLAccountcurnotmatchcashbookcur = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.GLAccountcurnotmatchcashbookcur", DefaultText = "The GL Account currency does not match the cashbook currency",LocalDefaultText = @"המטבע של הכרטיס הנבחר לא תואם למטבע הקופה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOThereOpenTransaction = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ThereOpenTransaction", DefaultText = "There are open transactions for the GL Account. can’t make it single currency GL Account",LocalDefaultText = @"לכרטיס ישנם תנועות פתוחת. אי אפשר להפוך אותו לכרטיס חד מטבעי", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOThereTransactions4GLAwithexistingCurrency = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ThereTransactions4GLAwithexistingCurrency", DefaultText = "Can’t change currency. There are transactions for this GL Account with the existing currency",LocalDefaultText = @"אי אפשר לשנות את המטבע. לכרטיס יש תנועות במטבע הנוכחי", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOselectAtLeast1Linetodeposit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.selectAtLeast1Linetodeposit", DefaultText = "Please select at least one cashbook line to deposit it",LocalDefaultText = @"יש לבחור לפחות המחאה אחת להפקדה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOSelected = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Selected", DefaultText = "Selected",LocalDefaultText = @"סה''כ סכום נבחר", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOCancellationReason = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.CancellationReason", DefaultText = "Cancellation Reason",LocalDefaultText = @"סיבת ביטול", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOGLABalanceNotEqual0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.GLABalanceNotEqual0", DefaultText = "Can’t inactivate the GL Account. GL Account’s balance is not equal to zero",LocalDefaultText = @"לא ניתן לחסום את הכרטיס . יתרת כרטיס שונה מאפס", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOchooseActionCode = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.chooseActionCode", DefaultText = "You must choose action code for line",LocalDefaultText = @"יש לבחור קוד פעולה עבור שורה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOchooseCreditAccount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.chooseCreditAccount", DefaultText = "You must choose credit account for line",LocalDefaultText = @"יש לבחור כרטיס זכות עבור שורה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOchooseDebitAccount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.chooseDebitAccount", DefaultText = "You must choose debit account for line",LocalDefaultText = @"יש לבחור כרטיס חובה עבור שורה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOchooseCurrency = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.chooseCurrency", DefaultText = "You must choose currency for line",LocalDefaultText = @"יש לבחור מטבע עבור שורה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOAmountIsMissing = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AmountIsMissing", DefaultText = "Amount is missing for line",LocalDefaultText = @"סכום חסר בשורה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOchooseRefDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.chooseRefDate", DefaultText = "You should choose ref. date for line",LocalDefaultText = @"יש לבחור תאריך אסמכתא עבור שורה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOchooseDueDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.chooseDueDate", DefaultText = "You should choose due date for line",LocalDefaultText = @"יש לבחור תאריך ערך עבור שורה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOCreditDebitAccountMustSameCurrency = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.CreditDebitAccountMustSameCurrency", DefaultText = "Credit and debit account must be the same currency",LocalDefaultText = @"כרטיס זכות וכרטיס חובה חייבים להיות באותו מטבע", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOTotalDebitMustEqualTotalCredit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.TotalDebitMustEqualTotalCredit", DefaultText = "Total debit amount must be equal to total credit amount. There is a difference of",LocalDefaultText = @"“סה“כ חובה צריך להיות שווה לסה“כ זכות, קיים הפרש של", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralONoAutoRecoMethodDefined4GLAccount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NoAutoRecoMethodDefined4GLAccount", DefaultText = "No automatic reconcile method defined for this GL Account",LocalDefaultText = @"לא הוגדר לכרטיס שיטת התאמה אוטומטית", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralONotransactionsSelected = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NotransactionsSelected", DefaultText = "No transactions selected",LocalDefaultText = @"לא נבחרו תנועות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralODifferenceMustEqual0 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.DifferenceMustEqual0", DefaultText = "The difference must be equal to zero",LocalDefaultText = @"ההפרש חייב להיות שווה לאפס", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOPrepareTransactions = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.PrepareTransactions", DefaultText = "Preparing Transactions",LocalDefaultText = @"מכין תנועות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralONewConnectedGLAccount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NewConnectedGLAccount", DefaultText = "New Connected GLAccount",LocalDefaultText = @"כרטיס מקושר חדש", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOPaymentAmount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.PaymentAmount", DefaultText = "Payment Amount:",LocalDefaultText = @"סכום לתשלום:", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralONewGeneralInvoice = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NewGeneralInvoice", DefaultText = "New General Invoice",LocalDefaultText = @"חשבונית כללית חדשה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOFalse = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.False", DefaultText = "False",LocalDefaultText = @"שגוי", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOBankAccountDifferentCurrencies = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.BankAccountDifferentCurrencies", DefaultText = "The payment currency should be similar to bank gl account currency",LocalDefaultText = @"מטבע התשלום חייב להיות זהה למטבע הכרטיס של הבנק", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralODisconnect = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Disconnect", DefaultText = "Disconnect",LocalDefaultText = @"ניתוק", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralODifferentAmounts = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.DifferentAmounts", DefaultText = "Total amount of lines must be equal to foreign amount",LocalDefaultText = @"סהכ סכום השורות חייב להיות שווה לסכום ההמחאה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralONewPaymentCheque = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NewPaymentCheque", DefaultText = "New Payment Cheque",LocalDefaultText = @"המחאה חדשה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOAdded = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Added", DefaultText = "Splitted by currency GLAccount has been added-",LocalDefaultText = @"נוסף כרטיס פיצול לפני מטבע-", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralODeactivated = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Deactivated", DefaultText = "The Splitted GLAccount- was deactivated",LocalDefaultText = @"כרטיס הפיצול- נחסם", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOChildAdded = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ChildAdded", DefaultText = "The GLAccount- was added as a child ",LocalDefaultText = @"הכרטיס - נוסף ככרטיס בן", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralODisconnected = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Disconnected", DefaultText = "The Child GLAccount- was disconnected ",LocalDefaultText = @"כרטיס בן - נותק", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOThereischashbookwithcurrencytypebranch = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Thereischashbookwithcurrencytypebranch", DefaultText = "There is chashbook with the chosen currency. type & branch",LocalDefaultText = @"קיימת קופה עם המטבע והסוג והסניף הנבחרים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOToDateMustGreaterFromDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ToDateMustGreaterFromDate", DefaultText = "To date must be greater than from date ",LocalDefaultText = @"עד תאריך חייב להיות גדול מתאריך", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOFromDateMustSmallerToDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.FromDateMustSmallerToDate", DefaultText = "From date must be smaller than to date ",LocalDefaultText = @"מתאריך חייב להיות קטן מ- עד תאריך", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOAreyousuredeleteline = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Areyousuredeleteline", DefaultText = "Are you sure to delete this line",LocalDefaultText = @"האם אתה בטוח שאתה רוצה למחוק את השורה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOSourceJournal = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.SourceJournal", DefaultText = "Source Journal",LocalDefaultText = @"פקודת מקור", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOGLAccountAlreadyConnectedToBankAccount = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.GLAccountAlreadyConnectedToBankAccount", DefaultText = "The GL Account is already connected to a Bank Account",LocalDefaultText = @"הכרטיס הנבחר מקושר לחשבון בנק אחר", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOActivated = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Activated", DefaultText = "The Splitted GLAccount- was activated",LocalDefaultText = @"כרטיס הפיצול- מוּפעָל", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralODifferentBankCurrency = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.DifferentBankCurrency", DefaultText = "Bank GLAccount currency differs from Local currency. Can’t create new payment cheque with currency that differs from local currency",LocalDefaultText = @"מטבע הכרטיס של הבנק שונה ממטבע מקומי. לא ניתן ליצור המחאה חדשה עם מטבע שונה ממטבע מקומי", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOToDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ToDate", DefaultText = "To Date",LocalDefaultText = @"לתאריך:", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOLevel = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Level", DefaultText = "Level:",LocalDefaultText = @"רמה:", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOChartOfaccountType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ChartOfaccountType", DefaultText = "Chart of account type",LocalDefaultText = @"סוג קבוצת מאזן", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOChartOfaccountFilter = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ChartOfaccountFilter", DefaultText = "Chart of account",LocalDefaultText = @"קבוצת מאזן", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOWithZeroBalance = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.WithZeroBalance", DefaultText = "GLAccount with balance equal to zero",LocalDefaultText = @"כלול כרטיסים ללא תנועות עם יתרה 0", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralORun = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Run", DefaultText = "Run",LocalDefaultText = @"הרץ", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOFutureDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.FutureDate", DefaultText = "Future Date",LocalDefaultText = @"תאריך עתיד", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOWithTransaction = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.WithTransaction", DefaultText = "With Transaction",LocalDefaultText = @"עם תנועות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOWithoutTransaction = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.WithoutTransaction", DefaultText = "Without Transaction",LocalDefaultText = @"בלי תנועות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOFilterBy = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.FilterBy", DefaultText = "Filter by:",LocalDefaultText = @"סנן לפי:", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOBuildingDocument = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.BuildingDocument", DefaultText = "Building document....",LocalDefaultText = @"....טוען מסמך", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOExternalReconcile = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ExternalReconcile", DefaultText = "External Reconcile",LocalDefaultText = @"התאמות חיצוניות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOCancelReconciltiation = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.CancelReconciltiation", DefaultText = "Cancel Reconciltiation",LocalDefaultText = @"ביטול התאמה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOIsCancelled = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.IsCancelled", DefaultText = "Cancelled",LocalDefaultText = @"מבוטל", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOWantToCancelCurrentReconciliation = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.WantToCancelCurrentReconciliation", DefaultText = "Are you sure you want to cancel the current reconciliation?",LocalDefaultText = @"האם אתה בטוח שאתה רוצה לבטל את ההתאמה הנוכחית?", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOSplittedAccountMsgCredit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.SplittedAccountMsgCredit", DefaultText = "There is a splitted GL Accounts for the chosen multi currency Credit Account. The transactions will be registered in the splitted by currency GL Account",LocalDefaultText = @"לכרטיס זה מוגדר פיצול מטבעות . התנועה תרשם על הכרטיס שמתאים למטבע", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOSplittedAccountMsgDebit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.SplittedAccountMsgDebit", DefaultText = "There is a splitted GL Accounts for the chosen multi currency Debit Account. The transactions will be registered in the splitted by currency GL Account",LocalDefaultText = @"לכרטיס זה מוגדר פיצול מטבעות . התנועה תרשם על הכרטיס שמתאים למטבע", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOCantVoidJouranlItDidntTurnedToTransactions = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.CantVoidJouranlItDidntTurnedToTransactions", DefaultText = "Can't void the Jouranl. It did not turned to transactions",LocalDefaultText = @"לא ניתן לבטל את פקודת היומן משום שהיא לא הפכה לתנועות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingORefDateAndAmountAreRequired = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.RefDateAndAmountAreRequired", DefaultText = "Ref. Date and Amount are required",LocalDefaultText = @"תאריך אסמכתא וסכום הינם שדות חובה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingONoAutomaticReconcileFoundByThisMethod = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.NoAutomaticReconcileFoundByThisMethod", DefaultText = "No automatic reconcile found by this method. Please change method or select lines manually",LocalDefaultText = @"לא נמצאה התאמה עבור שיטת ההתאמה שנבחרה. יש לשנות שיטת התאמה או להתאים ידנית", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralONoReconcile = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NoReconcile", DefaultText = "No Reconcile",LocalDefaultText = @"לא נמצא תוצאות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOAutomaticReconcileWillClearAllSelectedLines = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.AutomaticReconcileWillClearAllSelectedLines", DefaultText = "Automatic Reconcile will clear all selected lines. continue?",LocalDefaultText = @"השורות הנבחרות ימחקו. האם להמשיך?", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOSelectTwoTransactionAtLeast = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.SelectTwoTransactionAtLeast", DefaultText = "Please select at least two transactions in order to create a new external reconciliation",LocalDefaultText = @"חובה לבחור לפחות שתי תנועות ע''מ ליצור התאמה חדשה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOTaxLineTitle = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.TaxLineTitle", DefaultText = "New tax deduction period",LocalDefaultText = @"תקופת ניכוי חדשה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOMustBeLess = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.MustBeLess", DefaultText = "From date must be less than to date",LocalDefaultText = @"מ-תאריך חייב להיות קטן מ-עד תאריך", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOMustBeLarger = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.MustBeLarger", DefaultText = "To date must be larger than from date",LocalDefaultText = @"עד תאריך חייב להיות גדול מ-תאריך", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOTaxDeduction = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.TaxDeduction", DefaultText = "Tax deduction details",LocalDefaultText = @"פירוט ניכויים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOReferenceDateIsRequired = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.ReferenceDateIsRequired", DefaultText = "Reference date is required",LocalDefaultText = @"שדה תאריך הוא שדה חובה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOAmountIsRequired = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.AmountIsRequired", DefaultText = "Amount is required",LocalDefaultText = @"שדה סכום הוא חובה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOEditTaxPeriod = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.EditTaxPeriod", DefaultText = "Edit tax deduction period",LocalDefaultText = @"עריכת תקופת ניכוי", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOBalanceOfCashbookUnequalZeroCantBlocked = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.BalanceOfCashbookUnequalZeroCantBlocked", DefaultText = "The balance of the cashbook is unequal to zero, can’t be blocked",LocalDefaultText = @"יתרת הקופה שונה מאפס, לא ניתן לחסום", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingONoCashInCashbook = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.NoCashInCashbook", DefaultText = "There are no cash in the cashbook",LocalDefaultText = @"אין מזומנים בקופה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingONoChequesInCashbook = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.NoChequesInCashbook", DefaultText = "There are no cheques in the cashbook",LocalDefaultText = @"אין המחאות בקופה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOFutureYear = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.FutureYear", DefaultText = "Future Year!",LocalDefaultText = @"!שנה עתידית", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingONoExchangeRateForLocalCurrency = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.NoExchangeRateForLocalCurrency", DefaultText = "There is no exchange rate definition for local currency",LocalDefaultText = @"חסרה הגדרת שער המרה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOPeriodLinesIsNotCreated = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.PeriodLinesIsNotCreated", DefaultText = "The period lines for %Year is not created yet, do you want it to be created?",LocalDefaultText = @"לא נוצרו עדיין רשומות לשנת %Year , האם לצור؟", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOCreate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.Create", DefaultText = "Create",LocalDefaultText = @"יצירה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOMonthNotEndedCantClosed = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.MonthNotEndedCantClosed", DefaultText = "The month is not ended, can’t be closed",LocalDefaultText = @"החודש לא הסתיים, לא ניתן לסגור אותו", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOLineActivated = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.LineActivated", DefaultText = "Line Activated",LocalDefaultText = @"שורה הופעלה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOLineDeactivated = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.LineDeactivated", DefaultText = "Line - deactivated",LocalDefaultText = @"שורה מספר - נחסמה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOReconcileAccordingTo = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.ReconcileAccordingTo", DefaultText = "Reconcile according to",LocalDefaultText = @"התאמה לפי", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOFiltersSelected = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.FiltersSelected", DefaultText = "Filters selected",LocalDefaultText = @"מסננים נבחרו", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOLast7days = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.Last7days", DefaultText = "Last 7 days",LocalDefaultText = @"7 ימים אחרונים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOLastmonth = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.Lastmonth", DefaultText = "Last month",LocalDefaultText = @"חודש אחרון", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOLast3months = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.Last3months", DefaultText = "Last 3 months",LocalDefaultText = @"3 חודשים אחרונים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOLastyear = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.Lastyear", DefaultText = "Last year",LocalDefaultText = @"שנה אחרונה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOFromDate = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.FromDate", DefaultText = "From Date",LocalDefaultText = @"מתאריך", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingONoReconciliationFound = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.NoReconciliationFound", DefaultText = "There is no reconciliation found",LocalDefaultText = @"לא נמצאו התאמות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOThereRTransactions4GLAccountCantUpdated = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.ThereRTransactions4GLAccountCantUpdated", DefaultText = "There are transactions for the current GL Account, therefore , it can’t be updated",LocalDefaultText = @"ישנם תנועות בכרטיס הנה”ח המקושר, ולכן לא ניתן לעדכן את הכרטיס", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingONewReconcileWithAdjusment = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.NewReconcileWithAdjusment", DefaultText = "The difference must be equal to zero, In this case, new reconcile with adjusment will be created",LocalDefaultText = @"ההפרש חייב להיות שווה לאפס, במקרה כזה תיווצר התאמה עם תיקון שיוצר פקודת יומן חדשה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOOutOfDeposit = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.OutOfDeposit", DefaultText = "Out of deposit",LocalDefaultText = @"הוצאה מהפקדה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOOutOfDepositMSG = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.OutOfDepositMSG", DefaultText = "The selected cheque will be out of deposite, Choose if you want to return the cheque to cashbook or to customer",LocalDefaultText = @"ההמחאה שנבחרה תוצא מהפקדה, בחר אם תרצה להחזיר את ההמחאה לקופה או ללקוח", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralODemoEnvironmentMsg = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.DemoEnvironmentMsg", DefaultText = " Please note that you can't Edit currency rates in the demo environment!",LocalDefaultText = @"שים לב שאינך יכול לערוך שערי מטבע בסביבת ההדגמה!", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOMissingDefaultPercentage = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.MissingDefaultPercentage", DefaultText = "Missing default tax withholding percentage in accounting settings",LocalDefaultText = @"חסרה הגדרת מערכת לאחוז ניכוי מס במקור", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOFullAccountingSettings = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.FullAccountingSettings", DefaultText = "Full Accounting Settings",LocalDefaultText = @"הגדרות הנהלת חשבונות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOControlGLAccounts = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.ControlGLAccounts", DefaultText = "Control GL Accounts",LocalDefaultText = @"כרטיסים מרכזים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOCustom = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.Custom", DefaultText = "Custom",LocalDefaultText = @"מותאם אישית", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOCashbook = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.Cashbook", DefaultText = "Cashbook",LocalDefaultText = @"קופה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOCustomer = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.Customer", DefaultText = "Customer",LocalDefaultText = @"לקוח", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOAccountingPeriods = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.AccountingPeriods", DefaultText = "Accounting Periods",LocalDefaultText = @"תקופות חשבונאיות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOBackFromAutoRecoMSG = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.BackFromAutoRecoMSG", DefaultText = "#number transactions to reconcile was found",LocalDefaultText = @"נמצאו #number תנועות להתאמה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOBackFromAutoRecoBACK = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.BackFromAutoRecoBACK", DefaultText = "Back",LocalDefaultText = @"חזור להתאמה ידנית", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralODraftPayments = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.DraftPayments", DefaultText = "Draft Payments",LocalDefaultText = @"קבלות בסטטוס טיוטה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOOpenPayments = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.OpenPayments", DefaultText = "Open Payments",LocalDefaultText = @"קבלות בסטטוס מאושר", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOAllPayments = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AllPayments", DefaultText = "All Payments",LocalDefaultText = @"כל הקבלות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOAreconcileOfXTransactionCreated = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.AreconcileOfXTransactionCreated", DefaultText = "A reconcile of #Number transaction was created",LocalDefaultText = @"בוצעה התאמה עבור #Number תנועו", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingONolineswereenteredonbank = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.Nolineswereenteredonbank", DefaultText = "No lines were entered on the bank page",LocalDefaultText = @"לא הוזנו שורות בדף הבנק", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingODetailedControlAccountForCustomer = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.DetailedControlAccountForCustomer", DefaultText = "Detailed for customer",LocalDefaultText = @"פירוט לקוחות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingODetailedControlAccountForVendor = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.DetailedControlAccountForVendor", DefaultText = "Detailed for vendor",LocalDefaultText = @"פירוט ספקים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOVoid = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Void", DefaultText = "Void",LocalDefaultText = @"ביטול", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOCurrencyDetailed = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.CurrencyDetailed", DefaultText = "Currency detailed",LocalDefaultText = @"פירוט לפי מטבעות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCBusinessProcess = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.BusinessProcess", DefaultText = "Business Process",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOTaxableTransactions = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.TaxableTransactions", DefaultText = "Taxable Transactions",LocalDefaultText = @"עסקאות חייבות במע”מ", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOExemptTransactions = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.ExemptTransactions", DefaultText = "Exempt Transactions",LocalDefaultText = @"עסקאות פטורות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOAllTransactions = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.AllTransactions", DefaultText = "All Transactions",LocalDefaultText = @"כל העסקאות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOInputsEquipments = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.InputsEquipments", DefaultText = "Inputs - Equipments",LocalDefaultText = @"תשומות - ציוד", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOInputsOther = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.InputsOther", DefaultText = "Inputs - Other",LocalDefaultText = @"תשומות - אחר", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOIncluded = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.Included", DefaultText = "Included",LocalDefaultText = @"לכלול", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOLine = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.Line", DefaultText = "Line",LocalDefaultText = @"שורה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOReportExist = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.ReportExist", DefaultText = "There is already report for the chosen month",LocalDefaultText = @"ישנו דוח לחודש הנבחר", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOHigherMonthReport = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.HigherMonthReport", DefaultText = "There’s a report with a higher month",LocalDefaultText = @"קיים כבר דוח עם חודש גבוה יותר", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOReportWithClosedMonth = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.ReportWithClosedMonth", DefaultText = "Report month Month is not closed , please check Accounting Periods",LocalDefaultText = "לא ניתן להפיק דוח מע'' על חודש שעדיין לא נסגר", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOWantToCancelTaxReport = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.WantToCancelTaxReport", DefaultText = "Are you sure you want to cancel this report?",LocalDefaultText = @"האם אתה בטוח שאתה רוצה לבטל דוח זה؟", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBNew = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.New", DefaultText = "New",LocalDefaultText = @"אישור", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBSelect = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.Select", DefaultText = "Select",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCTablesCustomsAirline = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Tables.CustomsAirline", DefaultText = "Airlines",LocalDefaultText = @"חברות תעופה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHTasks = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.Tasks", DefaultText = "Tasks",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCOthersBTEX = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Others.BTEX", DefaultText = "Batch Task Executions",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOShowError = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.ShowError", DefaultText = "Show Error",LocalDefaultText = @"הצג שגיאה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOCreateFile = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.CreateFile", DefaultText = "Create File",LocalDefaultText = @"צור מסמך", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOCreatingFile = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.CreatingFile", DefaultText = "Creating File ...",LocalDefaultText = @"יוצר מסמך...", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOPleaseWaitCreatingFile = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.PleaseWaitCreatingFile", DefaultText = "Please wait while creating file ...",LocalDefaultText = @"נא לחכות עד סיום הפקת המסמך...", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOFileCreated = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.FileCreated", DefaultText = "File Created",LocalDefaultText = @"מסמך נוצר", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOErrorwhileCreating = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.ErrorwhileCreating", DefaultText = "Error while creating!",LocalDefaultText = @"שגיאה ביצירת המסמך!", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOclicktoStartCreatingFile = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.clicktoStartCreatingFile", DefaultText = "Please click create to start creating file",LocalDefaultText = @"נא לללחוץ ע''מ לצור את הקובץ", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOFileIsReady = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.FileIsReady", DefaultText = "File is ready to download",LocalDefaultText = @"יוצר מסמך...", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOReferences = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.References", DefaultText = "References",LocalDefaultText = @"אסמכתאות", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingONoautorecofoundbymethodchangemethod = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.Noautorecofoundbymethodchangemethod", DefaultText = "No automatic reconcile found by this method, Please change method or select lines manually",LocalDefaultText = @"לא נמצאו תנועות מתאימות לשיטת ההתאמה שנבחרה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOEditLine = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.EditLine", DefaultText = "Edit Line Number",LocalDefaultText = @"ערוך שורה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOTaxReportErrorMsg = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.TaxReportErrorMsg", DefaultText = "Cannot approve report, there are #Number errors",LocalDefaultText = @"קיימות #Number שגיאות - יש לתקנם לפני שידור הדוח", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOInactive = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.Inactive", DefaultText = "Inactive",LocalDefaultText = @"חסום", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOPostdatedChequeRedemption = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.PostdatedChequeRedemption", DefaultText = "Postdated Cheque Redemption",LocalDefaultText = @"פרעון שיק דחוי", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHShipments = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.Shipments", DefaultText = "Shipments",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHInvoices = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.Invoices", DefaultText = "Invoices",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOPaymentBankAccountCurrencyDifferent = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.PaymentBankAccountCurrencyDifferent", DefaultText = "Payment currency is different from bank account currency",LocalDefaultText = @"מטבע התשלום שונה ממטבע חשבון הבנק הנבחר", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "M", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHDepositions = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.Depositions", DefaultText = "Depositions",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOYes = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Yes", DefaultText = "Yes",LocalDefaultText = @"כן", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralONo = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.No", DefaultText = "No",LocalDefaultText = @"לא", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOShowNewRelease = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.ShowNewRelease", DefaultText = "New Version is online, for more details",LocalDefaultText = @"גרסה חדשה עלתה לאוויר - לפירוט השינויים", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralBPressHere = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.B.PressHere", DefaultText = "Press Here",LocalDefaultText = @"לחץ כאן", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "B", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHTariffModule = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.TariffModule", DefaultText = "Tariffs",LocalDefaultText = @"Tariffs", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCOthersCreateTenant = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Others.CreateTenant", DefaultText = "Create Tenant",LocalDefaultText = @"Create Tenant", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingOJournals = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.O.Journals", DefaultText = "Journals",LocalDefaultText = @"פק יומן", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCOthersCacheLog = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Others.CacheLog", DefaultText = "Cache Log",LocalDefaultText = @"Cache Log", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOWarning = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.Warning", DefaultText = "Warning",LocalDefaultText = @"אזהרה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHOccasions = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.Occasions", DefaultText = "Occasions",LocalDefaultText = @"Occasions", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOInterest = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Interest", DefaultText = "Interest",LocalDefaultText = @"ריבית", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOInterestBases = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.InterestBases", DefaultText = "Interest Bases",LocalDefaultText = @"בסיסי ריבית", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralONewInterestBaseType = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.NewInterestBaseType", DefaultText = "New Interest Base Type",LocalDefaultText = @"בסיס ריבית חדש", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOAbaseperiodwiththesamestartdateexists = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Abaseperiodwiththesamestartdateexists", DefaultText = "A base period with the same start date exists  ",LocalDefaultText = @"קיימת תקופה עם תאריך התחלה זהה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOAbasetypewiththesamecodeexists = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Abasetypewiththesamecodeexists", DefaultText = "A base type with the same code exists",LocalDefaultText = @"קיים בסיס עם קוד זהה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOTheratepercentageshouldbeformattedas0000 = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.Theratepercentageshouldbeformattedas00.00", DefaultText = "The rate percentage should be formatted as 00.00",LocalDefaultText = @"האחוז צריך להיות מוגדר בפורמט 00.00", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOAbaseperiodwiththesamestartdateisalreadyAdded = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AbaseperiodwiththesamestartdateisalreadyAdded", DefaultText = "A base period with the same start date is already Added",LocalDefaultText = @"כבר קיימת תקופה עם תאריך התחלה זהה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralORecordwassettoInactive = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.RecordwassettoInactive", DefaultText = "Record was set to Inactive",LocalDefaultText = @"הרשומה הוגדרה כלא פעילה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMCOthersPriceSteps = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MC.Others.PriceSteps", DefaultText = "Prices Steps",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MC", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralODeleteExistInterestperiods = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.DeleteExistInterestperiods", DefaultText = "You must delete the Interest periods record before Deactivating the customer",LocalDefaultText = @"יש למחוק הגדרת תקופות ריבית לפני חסימת לקוח", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOLineDateExist = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.LineDateExist", DefaultText = "Line with the same date already exist",LocalDefaultText = @"קיימת כבר שורה עם תאריך זהה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOFieldInterestCalculationStartDateismandatory = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.FieldInterestCalculationStartDateismandatory", DefaultText = "Field Interest Calculation Start Date is mandatory",LocalDefaultText = @"שדה תאריך הוא חובה", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOAtleastoneGLAccountInterestPeriodsrecordisrequired = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.AtleastoneGLAccountInterestPeriodsrecordisrequired", DefaultText = "At least one GLAccount Interest Periods record is required",LocalDefaultText = @"חובה להזין לפחות רשומה אחת של תקופת ריבית", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralOUsedSpace = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.O.UsedSpace", DefaultText = "Used Space",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_GeneralMHContainersFU = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "General.MH.ContainersFU", DefaultText = "ContainersFU",LocalDefaultText = null, ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "MH", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralFromDateMustBeLTT = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.FromDateMustBeLTT", DefaultText = " ''From date'' field must be less than or equal to ''To date'' field  ",LocalDefaultText = "מ-תאריך חייב להיות קטן או שווה לשדה עד תאריך", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

 		   TextCode GeneralTextCode_AccountingGeneralOToDateMustBeGTF = AddTextCodes.AddTextCode(new TextCodeDetails() { Code = "Accounting.General.O.ToDateMustBeGTF", DefaultText = " ''To date'' field must be greater than or equal to ''From date'' field ", LocalDefaultText = @"עד תאריך חייב להיות גדול או שווה לשדה מ-תאריך", ObjectTableId = GeneralObjectTable.Id, Tenant = 0, TextCodeTypeCode = "O", IsSpellChecked = false }, TextCodeRepository, TextCodes);

   
	    
}

    

   }
    
}
	 