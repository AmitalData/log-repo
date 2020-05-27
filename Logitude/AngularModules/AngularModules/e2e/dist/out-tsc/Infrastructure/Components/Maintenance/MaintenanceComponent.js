"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var TextCodeTranslator_1 = require("../../Utilities/TextCodeTranslator");
var SessionLocator_1 = require("../../Utilities/SessionLocator");
var FeatureLocator_1 = require("../../Utilities/FeatureLocator");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var Args_1 = require("../../../Infrastructure/Args");
var Tools_1 = require("../../../Infrastructure/Tools");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
//import {CustomsSettingExtendedListService} from '../../../Customs/Services/ExtendedLists/CustomsSettingExtendedListService';
var ObjectsLocator_1 = require("../../Locators/ObjectsLocator");
//import {RecallClientsForCutoms} from '../../../Customs/Components/CustomsRequests/GeneralRequests/RecallClientsForCutoms';
var MaintenanceComponent = /** @class */ (function () {
    function MaintenanceComponent() {
        this.LayoutDirection = 'ltr';
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.isTransmissionsPageVisible = false;
        this.ItemsSource = [];
        this.BuildPagesMenu();
        this.BuildMaintenanceMenu();
        this.LayoutDirection = ObjectsLocator_1.ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection;
    }
    MaintenanceComponent.prototype.BuildPagesMenu = function () {
        this.PagesMenu = [];
        this.PagesMenu.push(new Menu("PAR", TextCodeTranslator_1.TextCodeTranslator.Translate("General.MC.Partners.Partners")));
        this.PagesMenu.push(new Menu("BIL", TextCodeTranslator_1.TextCodeTranslator.Translate("General.MC.Billings.Billings")));
        this.PagesMenu.push(new Menu("LOC", TextCodeTranslator_1.TextCodeTranslator.Translate("General.MC.Locations.Locations")));
        this.PagesMenu.push(new Menu("OTH", TextCodeTranslator_1.TextCodeTranslator.Translate("General.MC.Others.Others")));
        this.PagesMenu.push(new Menu("PRS", TextCodeTranslator_1.TextCodeTranslator.Translate("General.MC.PersonalSettings.PersonalSettings")));
        this.PagesMenu.push(new Menu("CMS", TextCodeTranslator_1.TextCodeTranslator.Translate("General.MC.SystemSettings.SystemSettings")));
        if (SessionLocator_1.SessionLocator.Tenant == 0) {
            this.PagesMenu.push(new Menu("MNG", TextCodeTranslator_1.TextCodeTranslator.Translate("General.MC.Management.Management")));
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "LEADSOURCES") ||
            FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "STAGES") ||
            FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "ADDITIONALSERVICES") ||
            FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "CLOSINGREASONS") ||
            FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "COMPETITORS") ||
            FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "OPPORTUNITYTYPES") ||
            FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "INDUSTRIES") ||
            FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "PRODUCTTYPES") ||
            FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "EMAILALERTSETTINGS")) {
            this.PagesMenu.push(new Menu("CRM", TextCodeTranslator_1.TextCodeTranslator.Translate("General.MC.CRM.CRM")));
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "TICKET")) {
            this.PagesMenu.push(new Menu("TKT", "Tickets"));
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Occasion.OccasionType")) {
            this.PagesMenu.push(new Menu("OCS", "Occasions"));
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "CUSTOMS")) {
            this.PagesMenu.push(new Menu("CSM", TextCodeTranslator_1.TextCodeTranslator.Translate("General.MC.Custom.Customs")));
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "ACCOUNTINGS")) {
            this.PagesMenu.push(new Menu("ACC", TextCodeTranslator_1.TextCodeTranslator.Translate("General.MC.Accounting.Accounting")));
        }
        this.isTransmissionsPageVisible = false;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.InttraSettings")) {
            this.isTransmissionsPageVisible = true;
        }
        else if (SessionLocator_1.SessionLocator.Tenant == 0) {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.InttraCommunicationSettings")) {
                this.isTransmissionsPageVisible = true;
            }
        }
        if (this.isTransmissionsPageVisible) {
            this.PagesMenu.push(new Menu("TRANS", "Transmissions"));
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.BusinessProcessQueue") || FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.BusinessProcessTeam") ||
            FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.BusinessProcessBusinessRole")) {
            this.PagesMenu.push(new Menu("BUP", TextCodeTranslator_1.TextCodeTranslator.Translate("General.MC.BusinessProcess")));
        }
    };
    MaintenanceComponent.prototype.BuildMaintenanceMenu = function () {
        var _this = this;
        this.AllMaintenanceMenu = [];
        var allMenusTables = window.MenusTables.filter(function (x) { return x.MenuTypeCode === "MTC"; }).sort(function (a, b) { return a.IndexOfOrder - b.IndexOfOrder; });
        allMenusTables.forEach(function (item) {
            if (FeatureLocator_1.FeatureLocator.IsFeatureGranted(item.FeatureId)) {
                if (item.Code == "MTCB") {
                    //CustomsSettingList customsSetting = DataProvider.GetCachedList<CustomsSettingList>("Customs.CustomsSetting").FirstOrDefault();
                    //if (customsSetting != null) {
                    //    if (!customsSetting.IsConnectedToUniFreight) {
                    //        mainList.Add(new MenusTableViewModel(viewInjectionService, eventAggregator, regionManager, container, menu));
                    //    }
                    //}
                }
                else {
                    if (item.Code != "MTHT") {
                        _this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
                    }
                }
            }
            if (item.Code == "MTHT") {
                if (SessionLocator_1.SessionLocator.Tenant == 0) {
                    _this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
                }
            }
        });
        //if (FeatureLocator.HasFeaturePermession("General", "General.Features.Automations")) {
        //    var item = new MenusTablePM();
        //    item.CategoryTypeCode = "BUP";
        //    item.Icon = "Settings"
        //    item.Code = "AUTO";
        //    item.ObjectTableName = "Automations";
        //    this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
        //}
        this.BuildPersonalSettings();
        this.BuildSystemSettings();
        this.BuildCustomsMenus();
        this.BuildAccountingMenus();
        this.BuildOtherMenus();
        this.BuildTransmissionsMenus();
        this.PageChanged(this.PagesMenu[0]);
    };
    MaintenanceComponent.prototype.BuildSystemSettings = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "TERMOFUSERFEATUE")) {
            var item2 = new MenusTablePM();
            item2.CategoryTypeCode = "CMS";
            item2.Icon = "Settings";
            item2.Code = "TOUS";
            item2.ObjectTableName = "Terms of Use";
            this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item2));
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "SYSTEMSETTINGS")) {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.CompanyAddress")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings";
                item.Code = "COAD";
                item.ObjectTableName = "Company Address Settings";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (SessionLocator_1.SessionLocator.Tenant == 0) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings";
                item.Code = "Private Labels";
                item.ObjectTableName = "TenantManagment Private Labels";
                item.ObjectTableId = window.ObjectTables.filter(function (d) { return d.Name == "TenantManagmentPrivateLabels"; })[0].Id;
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.SystemDefaults")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings";
                item.Code = "CODE";
                item.ObjectTableName = "System Defaults";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (SessionLocator_1.SessionLocator.TenantPM.IsDocumentsArchive) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings";
                item.Code = "LBSE";
                item.ObjectTableName = "LogBox Settings";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "COUNTERS")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings";
                item.Code = "COCO";
                item.ObjectTableName = "Counters";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.CompanyLogo")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings";
                item.Code = "COLO";
                item.ObjectTableName = "Company Logo";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings";
                item.Code = "SYCR";
                item.ObjectTableName = "System Currencies";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "HOWTOACCOUNTINGSETTINGS")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings";
                item.Code = "ACSE";
                item.ObjectTableName = "Accounting Settings";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.LocalSettings")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings";
                item.Code = "LOSE";
                item.ObjectTableName = "Local Settings";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.InvoiceSettings")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings";
                item.Code = "INVS";
                item.ObjectTableName = "Invoice Settings";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.AirlineSettings")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings";
                item.Code = "AIRS";
                item.ObjectTableName = "Airline Settings";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.CustomerActivation")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings";
                item.Code = "CUSA";
                item.ObjectTableName = "Customer Settings";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.VATSettings")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings";
                item.Code = "VATS";
                item.ObjectTableName = "VAT Settings";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.SupportManagement")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings";
                item.Code = "SUMN";
                item.ObjectTableName = "Support Management";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.SystemUserPassword")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings";
                item.Code = "SUPW";
                item.ObjectTableName = "System User Password";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.HybridTenantThreshold")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings";
                item.Code = "HYTT";
                item.ObjectTableName = "Hybrid Tenant Threshold";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.Automations")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings";
                item.Code = "AUTO";
                item.ObjectTableName = "Automations";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.TenantManagement")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "List";
                item.Code = "TEMG";
                item.ObjectTableName = "TenantManagement";
                item.ObjectTableId = window.ObjectTables.filter(function (d) { return d.Name == "TenantManagement"; })[0].Id;
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (SessionLocator_1.SessionLocator.Tenant == 0 && FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.BluesnapContract")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "List";
                item.Code = "BCMT";
                item.ObjectTableName = "BluesnapContract";
                item.ObjectTableId = window.ObjectTables.filter(function (d) { return d.Name == "BluesnapContract"; })[0].Id;
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "ApiCredintials")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "List";
                item.Code = "APIC";
                item.ObjectTableName = "ApiCredintials";
                item.ObjectTableId = window.ObjectTables.filter(function (d) { return d.Name == "ApiCredintials"; })[0].Id;
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("WebhookKeys", "WebhookKeys")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "List";
                item.Code = "WHKS";
                item.ObjectTableName = "WebhookKeys";
                item.ObjectTableId = window.ObjectTables.filter(function (d) { return d.Name == "WebhookKeys"; })[0].Id;
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("APILogs", "APILogs")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "List";
                item.Code = "APLG";
                item.ObjectTableName = "APILogs";
                item.ObjectTableId = window.ObjectTables.filter(function (d) { return d.Name == "APILogs"; })[0].Id;
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.CustomerFieldsUpdateSetting")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings";
                item.Code = "CFUS";
                item.ObjectTableName = "CustomerFieldsUpdateSetting";
                item.ObjectTableId = window.ObjectTables.filter(function (d) { return d.Name == "CustomerFieldsUpdateSetting"; })[0].Id;
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.SecurityPolicySettings")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings";
                item.Code = "SECP";
                item.ObjectTableName = "Login Policy";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.QuoteSettings")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings";
                item.Code = "QuoteSettings";
                item.ObjectTableName = "Quote Settings";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            var item1 = new MenusTablePM();
            item1.CategoryTypeCode = "CMS";
            item1.Icon = "Settings";
            item1.Code = "SYIN";
            item1.ObjectTableName = "System Info";
            this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item1));
            var item3 = new MenusTablePM();
            item3.CategoryTypeCode = "CMS";
            item3.Icon = "List";
            item3.Code = "LOGS";
            item3.ObjectTableName = "CommunicationLog";
            item3.ObjectTableId = window.ObjectTables.filter(function (d) { return d.Name == "CommunicationLog"; })[0].Id;
            this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item3));
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("CreditLimitSetting", "Module")) {
                var item4 = new MenusTablePM();
                item4.CategoryTypeCode = "CMS";
                item4.Icon = "Settings";
                item4.Code = "CRLM";
                item4.ObjectTableName = "Credit Limit Settings";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item4));
            }
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "CUSTOMSINTERFACESETTINGS")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings";
                item.Code = "CISE";
                item.ObjectTableName = "Customs Settings";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.DocumentFilingEmailSettings")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CMS";
                item.Icon = "Settings";
                item.Code = "DFES";
                item.ObjectTableName = "Document Filing Email Settings";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "TICKET") && FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "TicketsSetting")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "TKT";
                item.Icon = "Settings";
                item.Code = "MTSE";
                item.ObjectTableName = "Ticket Settings";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
        }
    };
    MaintenanceComponent.prototype.BuildPersonalSettings = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "PERSONALSETTINGS")) {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.Signature")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "PRS";
                item.Icon = "Settings";
                item.Code = "SIGN";
                item.ObjectTableName = "Signature";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.ChangePassword")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "PRS";
                item.Icon = "Settings";
                item.Code = "CHPA";
                item.ObjectTableName = "Change Password";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
        }
    };
    MaintenanceComponent.prototype.BuildCustomsMenus = function () {
        if (window.ObjectTables.filter(function (d) { return d.Name == "Customs.CustomsRequiredField"; })[0] != null) {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Customs.CustomsRequiredField", "CSTMREQFIELDMTC")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "CSM";
                item.Icon = "Settings";
                item.Code = "REFI";
                item.ObjectTableName = "Customs.CustomsRequiredField";
                item.ObjectTableId = window.ObjectTables.filter(function (d) { return d.Name == "Customs.CustomsRequiredField"; })[0].Id;
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
        }
        var yaronWantWithoutLogiUpdate = false; // in "customs" branch do not use it !!
        if (yaronWantWithoutLogiUpdate ||
            (SessionLocator_1.SessionLocator.Tenant == 0 && !SessionLocator_1.SessionLocator.LoggedUserPM.IsDistributor && window.ObjectTables.filter(function (d) { return d.Name == "Customs.CustomsSetting"; })[0] != null)) {
            var item = new MenusTablePM();
            item.CategoryTypeCode = "CSM";
            item.Icon = "Settings";
            item.Code = "CSMN";
            item.ObjectTableName = "Customs.CustomsSetting";
            item.ObjectTableId = window.ObjectTables.filter(function (d) { return d.Name == "Customs.CustomsSetting"; })[0].Id;
            this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
        }
    };
    MaintenanceComponent.prototype.BuildAccountingMenus = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "ACCOUNTINGS")) {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.FullAccountingSetting")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "ACC";
                item.Icon = "Settings";
                item.Code = "FACS";
                item.ObjectTableName = "Full Accounting Setting";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            //if (FeatureLocator.HasFeaturePermession("General", "General.Features.YearTransfer")) {
            //    var item = new MenusTablePM();
            //    item.CategoryTypeCode = "ACC";
            //    item.Icon = "Settings"
            //    item.Code = "ACYT";
            //    item.ObjectTableName = "Year Transfer";
            //    this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            //}
            //if (FeatureLocator.HasFeaturePermession("General", "General.Features.AccountingPeriods")) {
            //    var item = new MenusTablePM();
            //    item.CategoryTypeCode = "ACC";
            //    item.Icon = "Settings"
            //    item.Code = "ACPD";
            //    item.ObjectTableName = "AccountingPeriod";
            //    var ObjectTable = window.ObjectTables.filter(d => d.Name == "AccountingPeriod")[0];
            //    item.ObjectTableId = ObjectTable.Id;
            //    this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            //}
        }
    };
    MaintenanceComponent.prototype.BuildOtherMenus = function () {
        if (SessionLocator_1.SessionLocator.Tenant == 0) { // && FeatureLocator.HasFeaturePermession("General", "HYBRIDPARTNERS")) {
            var item = new MenusTablePM();
            item.CategoryTypeCode = "OTH";
            item.Icon = "List";
            item.Code = "MTHP";
            item.ObjectTableName = "HybridPartner";
            item.ObjectTableId = window.ObjectTables.filter(function (d) { return d.Name == "HybridPartner"; })[0].Id;
            this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "SCHEDULERS")) {
            var item = new MenusTablePM();
            item.CategoryTypeCode = "MNG";
            item.Icon = "List";
            item.Code = "MASC";
            item.ObjectTableName = "TasksScheduler";
            item.TextCode = "General.Features.Schedulers";
            item.ObjectTableId = window.ObjectTables.filter(function (d) { return d.Name == "TasksScheduler"; })[0].Id;
            this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "MAINCUSTOMERS")) {
            var item = new MenusTablePM();
            item.CategoryTypeCode = "Par";
            item.Icon = "Customer";
            item.Code = "CUST";
            item.ObjectTableName = "Customer";
            item.ObjectTableId = window.ObjectTables.filter(function (d) { return d.Name == "Customer"; })[0].Id;
            this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "DROPBOX")) {
            var item = new MenusTablePM();
            item.CategoryTypeCode = "OTH";
            item.Icon = "Settings";
            item.Code = "DRBO";
            item.ObjectTableName = "DropBox Connection";
            //item.ObjectTableId = window.ObjectTables.filter(d => d.Name == "General")[0].Idd
            this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "CacheLogMenu")) {
            var item = new MenusTablePM();
            item.CategoryTypeCode = "OTH";
            item.Icon = "List";
            item.Code = "CCHL";
            item.ObjectTableName = "Cache Log";
            this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
        }
        if (SessionLocator_1.SessionLocator.Tenant == 0) {
            var item = new MenusTablePM();
            item.CategoryTypeCode = "CMS";
            item.Icon = "Settings";
            item.Code = "USPC";
            item.ObjectTableName = "Packages";
            this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            var item = new MenusTablePM();
            item.CategoryTypeCode = "CMS";
            item.Icon = "List";
            item.Code = "ANQU";
            item.ObjectTableName = "AnalyzeQueue";
            item.ObjectTableId = window.ObjectTables.filter(function (d) { return d.Name == "AnalyzeQueue"; })[0].Id;
            this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            var item = new MenusTablePM();
            item.CategoryTypeCode = "CMS";
            item.Icon = "List";
            item.Code = "ACCS";
            item.ObjectTableName = "AccountingSystem";
            item.ObjectTableId = window.ObjectTables.filter(function (d) { return d.Name == "AccountingSystem"; })[0].Id;
            this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            var item = new MenusTablePM();
            item.CategoryTypeCode = "MNG";
            item.Icon = "List";
            item.Code = "ERLG";
            item.ObjectTableName = "ErrorLog";
            item.ObjectTableId = window.ObjectTables.filter(function (d) { return d.Name == "ErrorLog"; })[0].Id;
            this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            var item = new MenusTablePM();
            item.CategoryTypeCode = "MNG";
            item.Icon = "List";
            item.Code = "BSLG";
            item.ObjectTableName = "BatchServicesLog";
            item.ObjectTableId = window.ObjectTables.filter(function (d) { return d.Name == "BatchServicesLog"; })[0].Id;
            this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
        }
        else {
            this.AllMaintenanceMenu = this.AllMaintenanceMenu.filter(function (d) { return d.Code != "MTDS"; });
        }
    };
    MaintenanceComponent.prototype.BuildTransmissionsMenus = function () {
        if (this.isTransmissionsPageVisible) {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.InttraSettings")) {
                var item = new MenusTablePM();
                item.CategoryTypeCode = "TRANS";
                item.Icon = "Settings";
                item.Code = "INTTRA_S";
                item.ObjectTableName = "INTTRA Settings";
                this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
            }
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.InttraCommunicationSettings")) {
                if (SessionLocator_1.SessionLocator.Tenant == 0) {
                    var item = new MenusTablePM();
                    item.CategoryTypeCode = "TRANS";
                    item.Icon = "Settings";
                    item.Code = "INTTRA_CMS";
                    item.ObjectTableName = "INTTRA Communication Settings";
                    this.AllMaintenanceMenu.push(new MaintenanceMenuItem(item));
                }
            }
        }
    };
    // Commands
    MaintenanceComponent.prototype.PageChanged = function (item) {
        var _this = this;
        this.SelectedMenu = item;
        var itemsSource = [];
        if (item != null) {
            itemsSource = this.AllMaintenanceMenu.filter(function (f) { return f.CategoryTypeCode.toUpperCase() == _this.SelectedMenu.Code.toUpperCase(); });
        }
        //this.ItemsSource = itemsSource;
        itemsSource.forEach(function (item) {
            if (Tools_1.AppTool.IsNullOrEmpty(item.TranslatedName)) {
                item.TranslatedName = "";
            }
        });
        this.ItemsSource = itemsSource.sort(function (a, b) { return a.TranslatedName.toLowerCase() !== b.TranslatedName.toLowerCase() ? a.TranslatedName.toLowerCase() < b.TranslatedName.toLowerCase() ? -1 : 1 : 0; });
    };
    MaintenanceComponent.prototype.ItemClicked = function (item) {
        var _this = this;
        if (item) {
            switch (item.Code) {
                case "DFES": {
                    var windowTitle = "Document Filing Email Settings";
                    var logWindow = new LogitudeWindow_1.LogitudeWindow();
                    logWindow.Width = 500;
                    logWindow.Height = 400;
                    logWindow.Title = windowTitle;
                    logWindow.IsShowCloseButton = true;
                    logWindow.Show('./Common/Components/Maintenance/DocumentFilingEmailSettings/DocumentFilingEmailSettingsComponent');
                    break;
                }
                case "CODE": {
                    this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe(function (response) {
                        var windowTitle = "System Defaults ";
                        var logWindow = new LogitudeWindow_1.LogitudeWindow();
                        logWindow.Width = 850;
                        logWindow.Height = 630;
                        logWindow.Title = windowTitle;
                        logWindow.IsShowCloseButton = true;
                        logWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/SystemDefaults/SystemDefaultsComponent');
                    });
                    break;
                }
                case "LBSE": {
                    this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe(function (response) {
                        var windowTitle = "LogBox Settings ";
                        var logWindow = new LogitudeWindow_1.LogitudeWindow();
                        logWindow.Width = 850;
                        logWindow.Height = 630;
                        logWindow.Title = windowTitle;
                        logWindow.IsShowCloseButton = true;
                        logWindow.Show('./Common/Components/Maintenance/LogBoxSettings');
                    });
                    break;
                }
                case "LOSE": {
                    this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe(function (response) {
                        var windowTitle = "Local Settings";
                        var logWindow = new LogitudeWindow_1.LogitudeWindow();
                        logWindow.Width = 750;
                        logWindow.Height = 500;
                        logWindow.Title = windowTitle;
                        logWindow.IsShowCloseButton = true;
                        logWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/LocalSettings/LocalSettingsComponent');
                    });
                    break;
                }
                case "MTCE": {
                    this.CustomsClosedTablesMethod(item);
                    break;
                }
                case "MTIM": {
                    this.InterfaceManageentMethod(item);
                    break;
                }
                case "CSMN": {
                    var test = true;
                    var strict = true;
                    if (test) {
                        if (Tools_1.DateTool.GetCurrentDateAsUtc().valueOf() < new Date(2017, 7, 20).valueOf()) {
                            strict = false;
                        }
                    }
                    var LoggedUserPMCode = SessionLocator_1.SessionLocator.LoggedUserPM.Code || "";
                    LoggedUserPMCode = LoggedUserPMCode.toLowerCase();
                    var allowed = false;
                    allowed = (LoggedUserPMCode == "amital" || LoggedUserPMCode.startsWith("amital."));
                    if (strict && !SessionLocator_1.SessionLocator.LoggedUserPM.IsCustomerCare && allowed) {
                        var messageWindow = new MessageWindow_1.MessageWindow();
                        messageWindow.Show("Logged User Is not Customer Care ");
                        return;
                    }
                    var windowTitle_1 = "הגדרות מכס"; //"Customs Settings";
                    var logWindow_1 = new LogitudeWindow_1.LogitudeWindow();
                    logWindow_1.Width = 750;
                    logWindow_1.Height = 500;
                    logWindow_1.Title = windowTitle_1;
                    logWindow_1.IsShowCloseButton = true;
                    logWindow_1.Show('./CustomsModules/CustomsMaintenance/Components/Maintenance/CustomsSettingsComponent');
                    break;
                }
                case "COAD": {
                    var windowTitle = "Company Address Settings";
                    var logWindow = new LogitudeWindow_1.LogitudeWindow();
                    logWindow.Width = 750;
                    logWindow.Height = 500;
                    logWindow.Title = windowTitle;
                    logWindow.IsShowCloseButton = true;
                    logWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/CompanyAddress/CompanyAddressSettingsComponent');
                    break;
                }
                case "SYCR": {
                    var windowTitle = "System Currencies";
                    var logWindow = new LogitudeWindow_1.LogitudeWindow();
                    logWindow.Title = windowTitle;
                    logWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/SystemCurrencies/SystemCurrenciesComponent');
                    break;
                }
                case "CHPA": {
                    var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                    logitudeWindow.Width = 600;
                    logitudeWindow.Height = 400;
                    logitudeWindow.Title = "Change User Password";
                    this._entityResourceService.getEntityResourceByTableName("User").subscribe(function (response) {
                        logitudeWindow.DataContext = _this;
                        logitudeWindow.Show('./InfrastructureModules/InfrastructureUser/Components/PersonalSettings/ChangePasswordComponent');
                    });
                    break;
                }
                case "COLO": {
                    var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                    logitudeWindow.Width = 740;
                    logitudeWindow.Height = 585;
                    logitudeWindow.DataContext = this;
                    logitudeWindow.Title = "Logo Definition";
                    logitudeWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/UploadImage/UploadLogoComponent');
                    break;
                }
                case "AUTO": {
                    var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                    logitudeWindow.Width = 950;
                    logitudeWindow.Height = 640;
                    logitudeWindow.DataContext = this;
                    logitudeWindow.Title = "Automations";
                    logitudeWindow.Show('./Infrastructure/Components/Maintenance/Automation/MainMenuAutomationComponent');
                    break;
                }
                case "SYIN": {
                    this._entityResourceService.getEntityResourceByTableName("TenantManagement", 0).subscribe(function (response) {
                        _this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe(function (tenantResp) {
                            var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                            logitudeWindow.Width = 800;
                            logitudeWindow.Height = 600;
                            logitudeWindow.DataContext = "SystemInfo";
                            logitudeWindow.Title = "System Info";
                            logitudeWindow.Show('./InfrastructureModules/InfrastructureOthers/Components/CustomizeLogitude/SystemInfoComponent');
                        });
                    });
                    break;
                }
                case "SIGN": {
                    this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe(function (response) {
                        var windowArgs = {};
                        windowArgs.DataViewModel = _this;
                        windowArgs.PageType = "Signature";
                        windowArgs.TemplateId = SessionLocator_1.SessionLocator.LoggedUserId;
                        windowArgs.Tenant = SessionLocator_1.SessionLocator.Tenant;
                        var widthwindow = window.innerWidth;
                        var heighthwindow = window.innerHeight;
                        var logWindow = new LogitudeWindow_1.LogitudeWindow();
                        logWindow.Width = widthwindow - 100;
                        logWindow.Height = heighthwindow - 100;
                        logWindow.Title = "Edit Html Template";
                        logWindow.WindowArgs = windowArgs;
                        logWindow.Show("./InfrastructureModules/InfrastructureDocuments/Components/DocumentComponent/HtmlDocumentPreviewComponent");
                    });
                    break;
                }
                case "TOUS": {
                    this._entityResourceService.getEntityResourceByTableName("TermsofUseSignature", 0).subscribe(function (response) {
                        var widthwindow = window.innerWidth;
                        var heighthwindow = window.innerHeight;
                        var data = SessionLocator_1.SessionLocator.LoggedUserId + "@" + SessionLocator_1.SessionLocator.Tenant + "@Signature";
                        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                        logitudeWindow.Width = 800;
                        logitudeWindow.Height = 500;
                        logitudeWindow.DataContext = data;
                        logitudeWindow.IsShowCloseButton = true;
                        logitudeWindow.Title = "Terms of Use Signature";
                        logitudeWindow.Show('./InfrastructureModules/InfrastructureOthers/Components/TermsOfUse/TermsofUseSignatureComponent');
                    });
                    break;
                }
                case "ACSE": {
                    var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                    logitudeWindow.Width = 820;
                    logitudeWindow.Height = 570;
                    logitudeWindow.Title = "Accounting Settings";
                    logitudeWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/AccountingSettings/AccountingSettingsComponent');
                    break;
                }
                case "FACS": {
                    this._entityResourceService.getEntityResourceByTableName("FullAccountingSetting", 0).subscribe(function (response) {
                        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                        logitudeWindow.Width = 900;
                        logitudeWindow.Height = 500;
                        logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.FullAccountingSettings"); // "Full Accounting Settings";
                        logitudeWindow.Show('./Accounting/Components/Maintenance/FullAccountingSettingsComponent');
                    });
                    break;
                }
                case "ACYT": {
                    this.CurrentSession.StartBusyIndicatorLoading();
                    this._entityResourceService.getEntityResourceByTableName("AccountingPeriod", 0).subscribe(function (response) {
                        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                        logitudeWindow.Width = 500;
                        logitudeWindow.Height = 300;
                        logitudeWindow.Title = "Year Transfer";
                        logitudeWindow.Show('./Accounting/Components/Maintenance/YearTransferComponent');
                    });
                    break;
                }
                case "ACPD": {
                    this._entityResourceService.getEntityResourceByTableName("AccountingPeriod", 0).subscribe(function (response) {
                        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                        logitudeWindow.Width = 750;
                        logitudeWindow.Height = 500;
                        logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Accounting.O.AccountingPeriods");
                        logitudeWindow.Show('./Accounting/Components/Maintenance/AccountingPeriodsComponent');
                    });
                    break;
                }
                case "MTSV": {
                    this._entityResourceService.getEntityResourceByTableName("SLAHeader", 0).subscribe(function (response) {
                        _this._entityResourceService.getEntityResourceByTableName("SLALine", 0).subscribe(function (response) {
                            var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                            logitudeWindow.Width = 950;
                            logitudeWindow.Height = 640;
                            logitudeWindow.ShowCloseButton = true;
                            logitudeWindow.Title = "SLA/Result Setting";
                            logitudeWindow.ShowCloseButton = true;
                            logitudeWindow.Show('./CRMModules/CRMOthers/Components/SLA/SLAMainWindowComponent');
                        });
                    });
                    break;
                }
                case "MTUS": {
                    this._entityResourceService.getEntityResourceByTableName("User", 0).subscribe(function (resp) {
                        SessionLocator_1.SessionLocator.DynamicLoader.Load('./InfrastructureModules/InfrastructureUser/Components/UserWorkspaceComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                            .then(function (cmpRef) {
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run(null);
                            _this.CurrentSession.AddMenuReference(cmpRef);
                        });
                    });
                    break;
                }
                case "CRLM": {
                    var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                    logitudeWindow.Title = "Credit Limit Settings";
                    logitudeWindow.Show('./Common/Components/Maintenance/CreditLimit/CreditLimitSettingsComponent');
                    break;
                }
                case "MTBH": {
                    this._entityResourceService.getEntityResourceByTableName("BusinessHour", 0).subscribe(function (response) {
                        _this._entityResourceService.getEntityResourceByTableName("BusinessHoursHoliday", 0).subscribe(function (response) {
                            var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                            logitudeWindow.Width = 950;
                            logitudeWindow.Height = 550;
                            logitudeWindow.Title = "Business Hours and Holidays";
                            logitudeWindow.ShowCloseButton = true;
                            logitudeWindow.Show('./CRMModules/CRMOthers/Components/BusinessHour/NewBusinessHourAndHolidaysComponent');
                        });
                    });
                    break;
                }
                case "AIRS": {
                    this._entityResourceService.getEntityResourceByTableName("TenantManagement", 0).subscribe(function (response) {
                        var windowTitle = "Airline Settings";
                        var logWindow = new LogitudeWindow_1.LogitudeWindow();
                        logWindow.Width = 650;
                        logWindow.Height = 350;
                        logWindow.Title = windowTitle;
                        logWindow.IsShowCloseButton = true;
                        logWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/AirlineSettings/AirlineSettingsComponent');
                    });
                    break;
                }
                case "DRBO": {
                    var windowTitle = "Dropbox Connection";
                    var logWindow = new LogitudeWindow_1.LogitudeWindow();
                    logWindow.Width = 350;
                    logWindow.Height = 225;
                    logWindow.Title = windowTitle;
                    logWindow.IsShowCloseButton = false;
                    logWindow.Show('./InfrastructureModules/InfrastructureOthers/Components/DropBox/DropBoxConnectionComponent');
                    break;
                }
                case "MTEA": {
                    var windowTitle = "E-mail Notifications Settings";
                    var logWindow = new LogitudeWindow_1.LogitudeWindow();
                    logWindow.Width = 800;
                    logWindow.Height = 550;
                    logWindow.Title = windowTitle;
                    logWindow.IsShowCloseButton = false;
                    logWindow.Show('./InfrastructureModules/InfrastructureOthers/Components/EmailNotifications/EmailNotificationsSettingsComponent');
                    break;
                }
                case "MTIS": {
                    var windowTitle = "Integration Systems Setting";
                    var logWindow = new LogitudeWindow_1.LogitudeWindow();
                    logWindow.Width = 600;
                    logWindow.Height = 350;
                    logWindow.Title = windowTitle;
                    logWindow.IsShowCloseButton = false;
                    logWindow.Show('./InfrastructureModules/InfrastructureOthers/Components/IntegrationSystemsSetting/IntegrationSystemsSetting');
                    break;
                }
                case "USPC": {
                    var logWindow = new LogitudeWindow_1.LogitudeWindow();
                    logWindow.Width = 850;
                    logWindow.Height = 550;
                    logWindow.Title = "User Packages";
                    logWindow.IsShowCloseButton = false;
                    logWindow.Show('./InfrastructureModules/InfrastructureUser/Components/Packages/UserPackagesComponent');
                    break;
                }
                case "CUSA": {
                    this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe(function (response) {
                        var windowTitle = "Customer Settings";
                        var logWindow = new LogitudeWindow_1.LogitudeWindow();
                        logWindow.Width = 800;
                        logWindow.Height = 700;
                        logWindow.Title = windowTitle;
                        logWindow.IsShowCloseButton = true;
                        logWindow.Show('./Common/Components/Maintenance/CustomerActivationSettingsComponent');
                    });
                    break;
                }
                case "VATS": {
                    this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe(function (response) {
                        var windowTitle = "VAT Settings";
                        var logWindow = new LogitudeWindow_1.LogitudeWindow();
                        logWindow.Width = 800;
                        logWindow.Height = 700;
                        logWindow.Title = windowTitle;
                        logWindow.IsShowCloseButton = true;
                        logWindow.Show('./Common/Components/Maintenance/VATSettingsComponent');
                    });
                    break;
                }
                case "CISE": {
                    var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                    logitudeWindow.Title = "Customs Settings";
                    logitudeWindow.Show('./Common/Components/Maintenance/CustomsInterface/CustomsInterfaceSettingsComponent');
                    break;
                }
                case "CFTP": {
                    var LoggedUserPMCode = SessionLocator_1.SessionLocator.LoggedUserPM.Code || "";
                    LoggedUserPMCode = LoggedUserPMCode.toLowerCase();
                    var allowed = false;
                    allowed = (LoggedUserPMCode == "amital" || LoggedUserPMCode.startsWith("amital."));
                    if (!SessionLocator_1.SessionLocator.LoggedUserPM.IsCustomerCare && allowed) {
                        var messageWindow = new MessageWindow_1.MessageWindow();
                        messageWindow.Show("Logged User Is not Customer Care ");
                        return;
                    }
                    var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                    logitudeWindow.Title = "הגדרות תקשורת ";
                    logitudeWindow.Width = 900;
                    logitudeWindow.Height = 530;
                    logitudeWindow.Show('./CustomsModules/CustomsMaintenance/Components/Maintenance/CustomsPartnerFtpListComponent');
                    break;
                }
                case "MTTC": {
                    this._entityResourceService.getEntityResourceByTableName("TicketClassification", 0).subscribe(function (resp) {
                        SessionLocator_1.SessionLocator.DynamicLoader.Load('./CRM/Components/Workspaces/TicketClassificationMaintenanceComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                            .then(function (cmpRef) {
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run();
                            _this.CurrentSession.AddMenuReference(cmpRef);
                        });
                    });
                    break;
                }
                case "INVS": {
                    this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe(function (response) {
                        var windowTitle = "Invoice Settings";
                        var logWindow = new LogitudeWindow_1.LogitudeWindow();
                        logWindow.Title = windowTitle;
                        logWindow.IsShowCloseButton = true;
                        logWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/InvoiceSettings/InvoiceSettingsComponent');
                    });
                    break;
                }
                case "MTFS": {
                    var windowTitle = "FBL Stock";
                    var logWindow = new LogitudeWindow_1.LogitudeWindow();
                    logWindow.Width = 800;
                    logWindow.Height = 550;
                    logWindow.Title = windowTitle;
                    logWindow.IsShowCloseButton = true;
                    logWindow.Show('./ShipmentModules/ShipmentStock/Components/FBLStock/FBLStockMainComponent');
                    break;
                }
                case "MTSE": {
                    this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe(function (response) {
                        var windowTitle = "Ticket Settings";
                        var logWindow = new LogitudeWindow_1.LogitudeWindow();
                        logWindow.Title = windowTitle;
                        logWindow.IsShowCloseButton = true;
                        logWindow.Show('./CRMModules/CRMOthers/Components/TicketSettings/TicketSettingsComponent');
                    });
                    break;
                }
                case "REFI": {
                    var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                    logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.RequiredFields");
                    logitudeWindow.ShowCloseButton = true;
                    logitudeWindow.Height = 525;
                    logitudeWindow.Width = 750;
                    logitudeWindow.Show('./CustomsModules/CustomsMaintenance/Components/Maintenance/RequiredFields/RequiredFieldsComponent');
                    break;
                }
                case "SECP": {
                    this._entityResourceService.getEntityResourceByTableName("TenantLoginPolicy", 0).subscribe(function (response) {
                        var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("TenantLoginPolicy");
                        var logWindow = new LogitudeWindow_1.LogitudeWindow();
                        logWindow.Width = 800;
                        logWindow.Height = 550;
                        logWindow.Title = windowTitle;
                        logWindow.IsShowCloseButton = false;
                        logWindow.Show('./InfrastructureModules/InfrastructureOthers/Components/TenantSecurityPolicy/TenantLoginPolicyComponent');
                    });
                    break;
                }
                case "BSLG": {
                    var logWindow = new LogitudeWindow_1.LogitudeWindow();
                    logWindow.Width = 1100;
                    logWindow.Height = 1000;
                    logWindow.Title = "Batch Services Log";
                    logWindow.IsShowCloseButton = true;
                    logWindow.Show('./InfrastructureModules/InfrastructureBatchService/Components/BatchService/BatchServicesComponent');
                    break;
                }
                case "COCO": {
                    var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                    logitudeWindow.Title = "Counters";
                    logitudeWindow.ShowCloseButton = true;
                    logitudeWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/Counters/CountersComponent');
                    break;
                }
                case "SUMN": {
                    var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                    logitudeWindow.Title = "Support Management";
                    logitudeWindow.Height = 250;
                    logitudeWindow.Width = 300;
                    logitudeWindow.ShowCloseButton = true;
                    logitudeWindow.Show('./InfrastructureModules/InfrastructureUser/Components/SupportManagementComponent');
                    break;
                }
                case "HYTT": {
                    var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                    logitudeWindow.Title = "Hybrid Tenant Threshold";
                    logitudeWindow.Height = 250;
                    logitudeWindow.Width = 300;
                    logitudeWindow.ShowCloseButton = false;
                    logitudeWindow.Show('./InfrastructureModules/InfrastructureOthers/Components/CustomizeLogitude/HybridTenantThresholdComponent');
                    break;
                }
                case "CRTE": {
                    var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                    logitudeWindow.Title = "Create Tenant";
                    logitudeWindow.Height = 500;
                    logitudeWindow.Width = 750;
                    logitudeWindow.ShowCloseButton = false;
                    logitudeWindow.Show('./InfrastructureModules/InfrastructureOthers/Components/CreateTenant/CreateTenantComponent');
                    break;
                }
                case "MASC": {
                    this._entityResourceService.getEntityResourceByTableName("TasksScheduler", 0).subscribe(function (response) {
                        var logWindow = new LogitudeWindow_1.LogitudeWindow();
                        logWindow.Width = 1200;
                        logWindow.Height = 1000;
                        if (!FeatureLocator_1.FeatureLocator.HasFeaturePermession("TasksScheduler", "READ") || (!FeatureLocator_1.FeatureLocator.HasFeaturePermession("TasksScheduler", "TASK") && !FeatureLocator_1.FeatureLocator.HasFeaturePermession("TasksScheduler", "FTP"))) {
                            logWindow.Width = 800;
                            logWindow.Height = 500;
                        }
                        logWindow.Title = "Scheduler";
                        logWindow.IsShowCloseButton = true;
                        logWindow.Show('./InfrastructureModules/InfrastructureBatchService/Components/TaskScheduler/MainSchedulerComponent');
                    });
                    break;
                }
                case "MTHT": {
                    var windowTitle = "Hybrid Tenant State";
                    var logWindow = new LogitudeWindow_1.LogitudeWindow();
                    logWindow.Width = 1100;
                    logWindow.Height = 550;
                    logWindow.Title = windowTitle;
                    logWindow.Show('./InfrastructureModules/InfrastructureHybrid/Components/HybridTenantState/HybridTenantStateComponent');
                    break;
                }
                case "MCSG": {
                    var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                    logitudeWindow.Title = "עמדות חתימה"; //TextCodeTranslator.Translate("Customs.General.O.RequiredFields");
                    logitudeWindow.ShowCloseButton = true;
                    logitudeWindow.Height = 525;
                    logitudeWindow.Width = 750;
                    logitudeWindow.Width = 1000;
                    logitudeWindow.Show('./CustomsModules/CustomsMaintenance/Components/Maintenance/SignStationsComponent');
                    break;
                }
                case "MRSG": {
                    var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                    logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.RecallSuppliersFromFile");
                    logitudeWindow.ShowCloseButton = true;
                    logitudeWindow.Height = 400;
                    logitudeWindow.Width = 500;
                    //logitudeWindow.Show('./Customs/Components/CustomsRequests/GeneralRequests/RecallSuppliersFromFileComponent');
                    logitudeWindow.Show('./CustomsModules/CustomsGeneralRequests/Components/RecallSuppliersFromFileComponent');
                    break;
                }
                case "MTDD": {
                    var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                    logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.MC.Customs.DocumentsDefinition");
                    logitudeWindow.ShowCloseButton = true;
                    logitudeWindow.Height = 650;
                    logitudeWindow.Width = 750;
                    logitudeWindow.Show('./CustomsModules/CustomsMaintenance/Components/Maintenance/CustomsDocumentsDefinitionComponent');
                    break;
                }
                case "MTRC":
                    {
                        var test = true;
                        var strict = true;
                        if (test) {
                            if (Tools_1.DateTool.GetCurrentDateAsUtc().valueOf() < new Date(2017, 7, 20).valueOf()) {
                                strict = false;
                            }
                        }
                        var LoggedUserPMCode = SessionLocator_1.SessionLocator.LoggedUserPM.Code || "";
                        LoggedUserPMCode = LoggedUserPMCode.toLowerCase();
                        var allowed = false;
                        allowed = (LoggedUserPMCode == "amital" || LoggedUserPMCode.startsWith("amital.") || SessionLocator_1.SessionLocator.LoggedUserPM.IsCustomerCare);
                        if (strict && !allowed) {
                            var messageWindow = new MessageWindow_1.MessageWindow();
                            messageWindow.Show("Logged User Is not Customer Care ");
                            return;
                        }
                        var confirmWindow_1 = new ConfirmWindow_1.ConfirmWindow();
                        confirmWindow_1.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.MC.Customs.RecallClientsForCutoms");
                        confirmWindow_1.Width = 300;
                        confirmWindow_1.Height = 200;
                        confirmWindow_1.YesButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.OK");
                        confirmWindow_1.NoButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.B.Cancel");
                        confirmWindow_1.ShowNoButton;
                        confirmWindow_1.Show("לעדכן נתוני יבואנים במערכת?");
                        confirmWindow_1.WindowClosed.subscribe(function (event) {
                            if (confirmWindow_1.Yes) {
                                var servicelink = '../../../CustomsModules/CustomsGeneralRequests/Components/RecallClientsForCutoms';
                                SessionLocator_1.SessionLocator.DynamicLoader.GetInstance(servicelink).then(function (service) {
                                    service.SendRecallMessageToServer();
                                });
                                // this will cause the customs to build every time......mohammad
                                //let _RecallClientsForCutoms: RecallClientsForCutoms = new RecallClientsForCutoms();
                                //_RecallClientsForCutoms.SendRecallMessageToServer();
                            }
                        });
                        break;
                    }
                case "QuoteSettings": {
                    var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                    logitudeWindow.Title = "Quote Settings";
                    logitudeWindow.Show('./QuoteModules/QuoteOthers/Components/Maintenance/QuoteSettingsComponent');
                    break;
                }
                case "INTTRA_S": {
                    var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                    logitudeWindow.Width = 960;
                    logitudeWindow.Height = 600;
                    logitudeWindow.Title = "INTTRA Settings";
                    logitudeWindow.Show('./ShipmentModules/ShipmentINTTRA/Components/Maintenance/INTTRASettingsComponent');
                    break;
                }
                case "INTTRA_CMS": {
                    var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                    logitudeWindow.Title = "INTTRA Communication Settings";
                    logitudeWindow.Show('./ShipmentModules/ShipmentINTTRA/Components/Maintenance/INTTRACommunicationSettingsComponent');
                    break;
                }
                case "CCHL": {
                    var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                    logitudeWindow.Width = 800;
                    logitudeWindow.Height = 600;
                    logitudeWindow.Title = 'Cache Log';
                    logitudeWindow.Show('./Infrastructure/Components/Maintenance/CacheLogComponent');
                    break;
                }
                //  case "TXRP": {
                //    this._entityResourceService.getEntityResourceByTableName("TaxReport", 0).subscribe((resp: any) => {
                //        SessionLocator.DynamicLoader.Load('./InfrastructureModules/InfrastructureUser/Components/UserWorkspaceComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                //            .then(cmpRef => {
                //                cmpRef.instance.ComponentRef = cmpRef;
                //                cmpRef.instance.Run(null);
                //                this.CurrentSession.AddMenuReference(cmpRef);
                //            });
                //    });
                //    break;
                //}
                default: {
                    if (item.ObjectTableId) {
                        var allQueries = window.Queries.filter(function (x) { return x.ObjectTableId === item.ObjectTableId; }).sort(function (a, b) { return a.IndexOrder - b.IndexOrder; });
                        if (allQueries.length == 0) {
                            var myConfirmWindow = new ConfirmWindow_1.ConfirmWindow();
                            myConfirmWindow.Show("No Queries found for " + item.TranslatedName);
                        }
                        else {
                            var listArgs = new Args_1.ListComponentArgs();
                            if (item.Code == "MTCL") {
                                listArgs.Perspective = "ShippersAndConsignees";
                                listArgs.NewButtonLabel = "New Shipper-Consignee";
                            }
                            var SelectedQuery = null;
                            if (listArgs.Perspective != null) {
                                if (item.ObjectTableName == "ErrorLog" || item.ObjectTableName == "AnalyzeQueue" || item.ObjectTableName == "CommunicationLog") {
                                    SelectedQuery = allQueries.filter(function (f) { return ((f.UserId == SessionLocator_1.SessionLocator.LoggedUserId && f.Tenant == SessionLocator_1.SessionLocator.Tenant) || f.Tenant == 0) && f.Perspective == listArgs.Perspective; }).sort(function (a, b) { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1; })[0];
                                }
                                else {
                                    SelectedQuery = allQueries.filter(function (f) { return ((f.UserId == SessionLocator_1.SessionLocator.LoggedUserId && f.Tenant == SessionLocator_1.SessionLocator.Tenant) || f.Tenant == 0) && f.Perspective == listArgs.Perspective; })[0];
                                }
                            }
                            else {
                                if (item.ObjectTableName == "ErrorLog" || item.ObjectTableName == "AnalyzeQueue" || item.ObjectTableName == "CommunicationLog") {
                                    SelectedQuery = allQueries.filter(function (f) { return ((f.UserId == SessionLocator_1.SessionLocator.LoggedUserId && f.Tenant == SessionLocator_1.SessionLocator.Tenant) || f.Tenant == 0); }).sort(function (a, b) { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1; })[0];
                                }
                                else {
                                    SelectedQuery = allQueries.filter(function (f) { return ((f.UserId == SessionLocator_1.SessionLocator.LoggedUserId && f.Tenant == SessionLocator_1.SessionLocator.Tenant) || f.Tenant == 0); })[0];
                                }
                            }
                            //var SelectedQuery = allQueries.filter(f => ((f.UserId == SessionLocator.LoggedUserId && f.Tenant == SessionLocator.Tenant) || f.Tenant == 0) && f.Perspective == listArgs.Perspective)[0];
                            var objectTablePM = window.ObjectTables.filter(function (d) { return d.Id == item.ObjectTableId; })[0];
                            listArgs.QueryCode = SelectedQuery.Code;
                            listArgs.ObjectTableName = objectTablePM.Name;
                            listArgs.BackButtonTitle = "Maintenance";
                            this._entityResourceService.getEntityResourceByTableName(listArgs.ObjectTableName, 0).subscribe(function (response) {
                                listArgs.DisplayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate(SelectedQuery.NameTextCodeCode);
                                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                                    .then(function (cmpRef) {
                                    cmpRef.instance.ComponentRef = cmpRef;
                                    cmpRef.instance.Run(listArgs);
                                    //this.CurrentSession.AddMenuReference(cmpRef);
                                });
                            });
                        }
                    }
                    else {
                        var myConfirmWindow = new ConfirmWindow_1.ConfirmWindow();
                        myConfirmWindow.ShowNoButton = false;
                        myConfirmWindow.YesButtonText = "Ok";
                        myConfirmWindow.Show("Not Implemented");
                    }
                    break;
                }
            }
        }
    };
    MaintenanceComponent.prototype.DoJoker = function (text) {
        switch (text) {
            case "jokeraccfunctionaltest":
                {
                    var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                    logitudeWindow.Width = 400;
                    logitudeWindow.Height = 300;
                    logitudeWindow.Title = "Accounting Load Test";
                    logitudeWindow.Show('./Accounting/Components/Maintenance/AccountingFunctionalTestComponent');
                }
                break;
            case "jokeraccloadtest":
                {
                    var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                    logitudeWindow.Width = 750;
                    logitudeWindow.Height = 500;
                    logitudeWindow.Title = "Accounting Load Test";
                    logitudeWindow.Show('./Accounting/Components/Maintenance/AccountingLoadTestComponent');
                }
                break;
            case "jokersign": {
                var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                logitudeWindow.Title = "עמדות חתימה"; //TextCodeTranslator.Translate("Customs.General.O.RequiredFields");
                logitudeWindow.ShowCloseButton = true;
                logitudeWindow.Height = 525;
                logitudeWindow.Width = 750;
                logitudeWindow.Width = 1000;
                logitudeWindow.Show('./CustomsModules/CustomsMaintenance/Components/Maintenance/SignStationsComponent');
                break;
            }
            case "jokerGetAmitalRestrictOwnerModel": {
                var servicelink = '../../../Customs/Services/ExtendedLists/CustomsSettingExtendedListService';
                SessionLocator_1.SessionLocator.DynamicLoader.GetInstance(servicelink).then(function (service) {
                    service.GetAmitalRestrictOwnerModel(false)
                        .subscribe(function (rsp) {
                        var AmitalRestrictOwnerModel = rsp.Result;
                    });
                });
                // this will cause the customs to build every time....mohammad
                //let customsSettingExtendedListService = new CustomsSettingExtendedListService();
                //customsSettingExtendedListService.GetAmitalRestrictOwnerModel(false)
                //    .subscribe(rsp => {
                //        let AmitalRestrictOwnerModel = rsp.Result;
                //    });
                break;
            }
            case "jokerloadtest": {
                var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
                logitudeWindow.Title = "Load Test"; //TextCodeTranslator.Translate("Customs.General.O.RequiredFields");
                logitudeWindow.ShowCloseButton = true;
                logitudeWindow.Height = 525;
                logitudeWindow.Width = 750;
                logitudeWindow.Show('./CustomsModules/CustomsMaintenance/Components/Maintenance/LoadTestComponent');
                break;
            }
            default: {
                break;
            }
        }
    };
    MaintenanceComponent.prototype.SearchTextChanged = function (text) {
        var _this = this;
        if (text && text.startsWith("joker")) {
            this.DoJoker(text);
            return;
        }
        var itemsSource = this.AllMaintenanceMenu;
        if (Tools_1.AppTool.IsNullOrEmpty(text)) {
            itemsSource = this.AllMaintenanceMenu.filter(function (f) { return f.CategoryTypeCode.toUpperCase() == _this.SelectedMenu.Code.toUpperCase(); });
        }
        else {
            itemsSource = itemsSource.filter(function (f) { return f.TranslatedName != null; });
            itemsSource = itemsSource.filter(function (f) { return f.TranslatedName.toUpperCase().indexOf(text.toUpperCase()) > -1; });
        }
        this.ItemsSource = itemsSource;
    };
    MaintenanceComponent.prototype.CustomsClosedTablesMethod = function (item) {
        var _this = this;
        //ObjectTableName                     :        "Customs.CustomsClosedTable"
        this._entityResourceService.getEntityResourceByTableName(item.ObjectTableName, 0).subscribe(function (response) {
            var listArgs = new Args_1.ListComponentArgs();
            listArgs.DisplayTitle = item.TranslatedName; //TextCodeTranslator.Translate();
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsMaintenance/Components/Maintenance/CustomsClosedTablesComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                //cmpRef.instance.Run(listArgs);
                _this.CurrentSession.AddMenuReference(cmpRef);
            });
        });
    };
    MaintenanceComponent.prototype.InterfaceManageentMethod = function (item) {
        var _this = this;
        //ObjectTableName                     :        "Customs.CustomsClosedTable"
        this._entityResourceService.getEntityResourceByTableName(item.ObjectTableName, 0).subscribe(function (response) {
            var listArgs = new Args_1.ListComponentArgs();
            listArgs.DisplayTitle = item.TranslatedName; //TextCodeTranslator.Translate();
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./CustomsModules/CustomsMaintenance/Components/Maintenance/InterfaceManagementComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                //cmpRef.instance.Run(listArgs);
                _this.CurrentSession.AddMenuReference(cmpRef);
            });
        });
    };
    MaintenanceComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './MaintenanceComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], MaintenanceComponent);
    return MaintenanceComponent;
}());
exports.MaintenanceComponent = MaintenanceComponent;
var MaintenanceMenuItem = /** @class */ (function () {
    function MaintenanceMenuItem(item) {
        this.item = item;
        this.Code = item.Code;
        this.ObjectTableId = item.ObjectTableId;
        this.ObjectTableName = item.ObjectTableName;
        this.CategoryTypeCode = item.CategoryTypeCode;
        this.SetTranslatedName();
        this.SetDescriptionText();
        this.SetImageIconSource();
    }
    MaintenanceMenuItem.prototype.SetTranslatedName = function () {
        var _this = this;
        var myResult = "";
        if (this.Code == "AWMS") {
            var r = "";
        }
        if (this.Code == "MTCL" || this.Code == "MTIS" || this.Code == "MCSG" || this.Code == "MASC" || this.Code == "CRTE") {
            myResult = TextCodeTranslator_1.TextCodeTranslator.TranslateTable(this.item.TextCode);
        }
        else if (this.ObjectTableId == null) {
            myResult = this.item.ObjectTableName;
        }
        else {
            var ObjectTable = window.ObjectTables.filter(function (d) { return d.Id == _this.ObjectTableId; })[0];
            if (ObjectTable.Name.indexOf("Customs.") != -1 || ObjectTable.ClientModuleName == "Accounting") {
                myResult = TextCodeTranslator_1.TextCodeTranslator.TranslateTable(this.item.ObjectTableName);
            }
            else {
                myResult = TextCodeTranslator_1.TextCodeTranslator.TranslateTablePlural(this.item.ObjectTableName);
            }
        }
        this.TranslatedName = myResult;
        if (this.Code == "MASC") {
            this.TranslatedName = "Scheduler";
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.TranslatedName)) {
            this.TranslatedName = this.Code;
        }
    };
    MaintenanceMenuItem.prototype.SetDescriptionText = function () {
        var _this = this;
        var myResult = "";
        if (this.CategoryTypeCode == "PRS" || this.CategoryTypeCode == "CMS" || this.CategoryTypeCode == "MNG" || this.Code == "FACS" || this.Code == "ACYT") {
            switch (this.Code) {
                case "SIGN": {
                    myResult = "Set Signature Settings";
                    break;
                }
                case "CHPA": {
                    myResult = "Change Password";
                    break;
                }
                case "COAD": {
                    myResult = "Company Address Settings";
                    break;
                }
                case "CODE": {
                    myResult = "System Defaults";
                    break;
                }
                case "LBSE": {
                    myResult = "LogBox Settings";
                    break;
                }
                case "COCO": {
                    myResult = "Counters";
                    break;
                }
                case "COLO": {
                    myResult = "Company Logo";
                    break;
                }
                case "ACSE": {
                    myResult = "Accounting Settings";
                    break;
                }
                case "LOSE": {
                    myResult = "Local Settings";
                    break;
                }
                case "USPC": {
                    myResult = "User Packages";
                    break;
                }
                case "TENT": {
                    myResult = "List of Tenants";
                    break;
                }
                case "LOGS": {
                    myResult = "Communication Logs";
                    break;
                }
                case "MNGT": {
                    myResult = "Error Logs";
                    break;
                }
                case "APLG": {
                    myResult = "API Logs";
                    break;
                }
                case "FACS": {
                    myResult = "Define your accounting settings";
                    break;
                }
                //case "ACPD": { myResult = "Define your accounting periods"; break; }
                case "ACYT": {
                    myResult = "Year Transfer";
                    break;
                }
            }
        }
        else {
            var ObjectTable = window.ObjectTables.filter(function (d) { return d.Id == _this.item.ObjectTableId; })[0];
            if (ObjectTable != null) {
                myResult = TextCodeTranslator_1.TextCodeTranslator.Translate(ObjectTable.DescriptionTextCodeCode);
            }
        }
        //if (this.Code == "MCSG") { myResult = "עמדות חתימה"; }
        this.DescriptionText = myResult;
    };
    MaintenanceMenuItem.prototype.SetImageIconSource = function () {
        switch (this.item.Icon) {
            case "Customer":
            case "Customer.png": {
                this.ImageIconSource = "./Images/Maintenance/Customer.png";
                break;
            }
            case "PotentialCustomer.png": {
                this.ImageIconSource = "./Images/Maintenance/ShippersConsignee.png";
                break;
            }
            case "Agent.png": {
                this.ImageIconSource = "./Images/Maintenance/Agent.png";
                break;
            }
            case "CustomAgent.png": {
                this.ImageIconSource = "./Images/Maintenance/CustomAgent.png";
                break;
            }
            case "ShippingAgent.png": {
                this.ImageIconSource = "./Images/Maintenance/ShippingAgent.png";
                break;
            }
            case "Airline.png": {
                this.ImageIconSource = "./Images/Maintenance/Airline.png";
                break;
            }
            case "ShippingLine.png": {
                this.ImageIconSource = "./Images/Maintenance/ShippingLine.png";
                break;
            }
            case "Trucker.png": {
                this.ImageIconSource = "./Images/Maintenance/Trucker.png";
                break;
            }
            case "Settings": {
                this.ImageIconSource = "./Images/Maintenance/Settings.png";
                break;
            }
            case "List":
            case "Table":
            default: {
                this.ImageIconSource = "./Images/Maintenance/Table.png";
                break;
            }
        }
    };
    return MaintenanceMenuItem;
}());
var Menu = /** @class */ (function () {
    function Menu(code, name) {
        this.Code = code;
        this.Name = name;
    }
    return Menu;
}());
var MenusTablePM = /** @class */ (function () {
    function MenusTablePM() {
    }
    return MenusTablePM;
}());
//# sourceMappingURL=MaintenanceComponent.js.map