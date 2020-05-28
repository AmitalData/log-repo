"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
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
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var TenantImportComponent_1 = require("../../../../Common/Components/Maintenance/TenantImportComponent");
var Tools_1 = require("../../../../Infrastructure/Tools");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var Args_1 = require("../../../../Infrastructure/Args");
var CommonDomainService_1 = require("../../../../Common/Services/CommonDomainService");
var GlobalDomainService_1 = require("../../../../Common/Services/GlobalDomainService");
var ServiceHelper_1 = require("../../../../Infrastructure/Utilities/ServiceHelper");
var SessionInfo_1 = require("../../../../Infrastructure/Utilities/SessionInfo");
var Args_2 = require("../../../../Infrastructure/Args");
var ServiceLocator_1 = require("../../../../Infrastructure/Locators/ServiceLocator");
var GettingStartedComponent = /** @class */ (function (_super) {
    __extends(GettingStartedComponent, _super);
    function GettingStartedComponent() {
        var _this = _super.call(this) || this;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this.DataContext = _this;
        _this.VideosObslist = [];
        _this.HowToObslist = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        //Help Resources
        _this.HasVedios = false;
        _this.HasHowTos = false;
        // CheckFeatures 1
        _this.CustomerInfoVisibility = false;
        _this.AgentInfoVisibility = false;
        _this.UserInfoVisibility = false;
        _this.PortInfoVisibility = false;
        _this.AirlineInfoVisibility = false;
        _this.ShippingLineInfoVisibility = false;
        _this.AddCustomerVisibility = false;
        _this.AddAgentVisibility = false;
        _this.AddUserVisibility = false;
        _this.AddPortVisibility = false;
        _this.AddAirlineVisibility = false;
        _this.AddShippingLineVisibility = false;
        _this.SystemSettingsVisibility = false;
        _this.CompanyAddressVisibility = false;
        _this.SystemDefaultsVisibility = false;
        _this.CountersVisibility = false;
        _this.CompanyLogoVisibility = false;
        _this.SystemCurrenciesVisibility = false;
        _this.AccountingSettingsVisibility = false;
        _this.LocalSettingsVisibility = false;
        _this.InvoiceSettingsVisibility = false;
        _this.AirlineSettingsVisibility = false;
        _this.PersonalSettingsVisibility = false;
        _this.SignatureVisibility = false;
        _this.ChangePasswordVisibility = false;
        _this.PortsCount = 0;
        _this.AgentsCount = 0;
        _this.CustomersCount = 0;
        _this.UsersCount = 0;
        _this.QuotesCount = 0;
        _this.ShipmentsCount = 0;
        _this.MastersCount = 0;
        _this.AirlinesCount = 0;
        _this.ShippinglinesCount = 0;
        // Check Box 
        _this.checkBoxIsEnabled = true;
        _this.LoadData();
        _this.CheckFeatures1();
        _this.CheckFeatures2();
        return _this;
    }
    GettingStartedComponent.prototype.LoadData = function () {
        this.LoadHelpResources();
        this.LoadGetStartedData();
    };
    GettingStartedComponent.prototype.LoadHelpResources = function () {
        var _this = this;
        var service = new GlobalDomainService_1.GlobalDomainService();
        service.GetAllHelpResources().subscribe(function (response) {
            if (!response.HasError) {
                var allItems = response.Result;
                var videoList = allItems.filter(function (d) { return d.Type == "VID"; });
                var nonVideoList = allItems.filter(function (d) { return d.Type != "VID"; });
                _this.BuildVideoist(videoList);
                _this.BuildHowToList(nonVideoList);
            }
        });
    };
    GettingStartedComponent.prototype.BuildVideoist = function (myList) {
        var _this = this;
        this.VideosObslist = [];
        myList.sort(function (a, b) { return (a.UpdateDate === b.UpdateDate) ? 0 : (a.UpdateDate > b.UpdateDate) ? -1 : 1; }).forEach(function (item) {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("HelpResource", item.FeatureCode)) {
                _this.VideosObslist.push(new HelpResourceArgs(item));
            }
        });
        this.HasVedios = this.VideosObslist.length > 0 ? true : false;
    };
    GettingStartedComponent.prototype.BuildHowToList = function (myList) {
        var _this = this;
        this.HowToObslist = [];
        var releaseNotes = myList.filter(function (d) { return d.Type == "REL"; });
        var nonReleaseNotes = myList.filter(function (d) { return d.Type != "REL"; });
        if (releaseNotes.length > 0) {
            var releaseNote = releaseNotes.filter(function (d) { return d.Type == "REL"; }).sort(function (a, b) {
                return (a.UpdateDate === b.UpdateDate) ? 0 : (a.UpdateDate > b.UpdateDate) ? -1 : 1;
            })[0];
            this.HowToObslist.push(new HelpResourceArgs(releaseNote));
        }
        nonReleaseNotes.sort(function (a, b) { return (a.UpdateDate === b.UpdateDate) ? 0 : (a.UpdateDate > b.UpdateDate) ? -1 : 1; }).forEach(function (item) {
            if (_this.HowToObslist.length < 9) {
                _this.HowToObslist.push(new HelpResourceArgs(item));
            }
        });
        this.HasHowTos = this.HowToObslist.length > 0 ? true : false;
    };
    GettingStartedComponent.prototype.CheckFeatures1 = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Customer", "Module")) {
            this.CustomerInfoVisibility = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Agent", "Module")) {
            this.AgentInfoVisibility = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("User", "Module")) {
            this.UserInfoVisibility = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Port", "Module")) {
            this.PortInfoVisibility = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Airline", "Module")) {
            this.AirlineInfoVisibility = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("ShippingLine", "Module")) {
            this.ShippingLineInfoVisibility = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Customer", "Module")) {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Customer", "NEW") && FeatureLocator_1.FeatureLocator.HasFeaturePermession("Customer", "NEWCUSTOMER")) {
                this.AddCustomerVisibility = true;
            }
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Agent", "Module")) {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Agent", "NEW") && FeatureLocator_1.FeatureLocator.HasFeaturePermession("Agent", "NEWAGENT")) {
                this.AddAgentVisibility = true;
            }
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("User", "Module")) {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("User", "NEW") && FeatureLocator_1.FeatureLocator.HasFeaturePermession("User", "NEWUSER")) {
                this.AddUserVisibility = true;
            }
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Port", "Module")) {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Port", "NEW") && FeatureLocator_1.FeatureLocator.HasFeaturePermession("Port", "NEWPORT")) {
                this.AddPortVisibility = true;
            }
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Airline", "Module")) {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Airline", "NEW") && FeatureLocator_1.FeatureLocator.HasFeaturePermession("Airline", "NEWAIRLINE")) {
                this.AddAirlineVisibility = true;
            }
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("ShippingLine", "Module")) {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("ShippingLine", "NEW") && FeatureLocator_1.FeatureLocator.HasFeaturePermession("ShippingLine", "NEWSHIPPINGLINE")) {
                this.AddShippingLineVisibility = true;
            }
        }
    };
    GettingStartedComponent.prototype.CheckFeatures2 = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "SYSTEMSETTINGS")) {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.CompanyAddress")) {
                this.SystemSettingsVisibility = true;
            }
            else if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.SystemDefaults")) {
                this.SystemSettingsVisibility = true;
            }
            else if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "COUNTERS")) {
                this.SystemSettingsVisibility = true;
            }
            else if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.CompanyLogo")) {
                this.SystemSettingsVisibility = true;
            }
            else if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
                this.SystemSettingsVisibility = true;
            }
            else if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "HOWTOACCOUNTINGSETTINGS")) {
                this.SystemSettingsVisibility = true;
            }
            else if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.LocalSettings")) {
                this.SystemSettingsVisibility = true;
            }
            else if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.InvoiceSettings")) {
                this.SystemSettingsVisibility = true;
            }
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.CompanyAddress")) {
            this.CompanyAddressVisibility = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.SystemDefaults")) {
            this.SystemDefaultsVisibility = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "COUNTERS")) {
            this.CountersVisibility = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.CompanyLogo")) {
            this.CompanyLogoVisibility = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            this.SystemCurrenciesVisibility = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "HOWTOACCOUNTINGSETTINGS")) {
            this.AccountingSettingsVisibility = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.LocalSettings")) {
            this.LocalSettingsVisibility = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.InvoiceSettings")) {
            this.InvoiceSettingsVisibility = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.AirlineSettings")) {
            this.AirlineSettingsVisibility = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "PERSONALSETTINGS")) {
            if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.Signature")) {
                this.PersonalSettingsVisibility = true;
            }
            else if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.ChangePassword")) {
                this.PersonalSettingsVisibility = true;
            }
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.Signature")) {
            this.SignatureVisibility = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("General", "General.Features.ChangePassword")) {
            this.ChangePasswordVisibility = true;
        }
    };
    // Data Management 
    GettingStartedComponent.prototype.New = function (entity) {
        switch (entity) {
            case "User": {
                this.RunNewEntity(entity, "./InfrastructureModules/InfrastructureUser/Components/NewUserComponent");
                break;
            }
            case "Agent": {
                this.RunNewEntity(entity, "./CommonModules/CommonAgent/Components/NewEntity/NewAgentComponent");
                break;
            }
            case "Customer": {
                this.RunNewEntity(entity, "./CommonModules/CommonCustomer/Components/NewEntity/NewCustomerComponent");
                break;
            }
            case "Port": {
                this.RunImportEntity(entity);
                break;
            }
            case "Airline": {
                this.RunImportEntity(entity);
                break;
            }
            case "ShippingLine": {
                this.RunImportEntity(entity);
                break;
            }
            default: {
                break;
            }
        }
    };
    GettingStartedComponent.prototype.RunNewEntity = function (objectTableName, path) {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName(objectTableName).subscribe(function (response) {
            var str = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NewEntity");
            str = str.replace("%Entity", TextCodeTranslator_1.TextCodeTranslator.TranslateTable(objectTableName));
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            logWindow.Title = str;
            logWindow.Show(path);
            logWindow.WindowClosed.subscribe(function ($event) {
                _this.LoadGetStartedData();
            });
        });
    };
    GettingStartedComponent.prototype.RunImportEntity = function (objectTableName) {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName(objectTableName, 0).subscribe(function (response) {
            var windowTitle = "Add " + objectTableName;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            var args = new TenantImportComponent_1.ImportEntityArgs();
            var objectTable = window.ObjectTables.filter(function (x) { return x.Name === objectTableName; })[0];
            if (objectTable) {
                args.ObjectTableId = objectTable.Id;
            }
            args.ObjectTableName = objectTableName;
            logWindow.WindowArgs = args;
            logWindow.Width = 1000;
            logWindow.Height = 600;
            logWindow.Title = windowTitle;
            logWindow.Show('./Common/Components/Maintenance/TenantImportComponent');
            logWindow.WindowClosed.subscribe(function ($event) {
                _this.LoadGetStartedData();
            });
        });
    };
    GettingStartedComponent.prototype.ViewList = function (entity) {
        var _this = this;
        if (entity == "User") {
            this._entityResourceService.getEntityResourceByTableName("User", 0).subscribe(function (resp) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./InfrastructureModules/InfrastructureUser/Components/UserWorkspaceComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.ComponentRef = cmpRef;
                    var args = new Args_2.UserArgs();
                    args.BackButtonText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.MH.GettingStarted");
                    cmpRef.instance.Run(args);
                    _this.CurrentSession.AddMenuReference(cmpRef);
                });
            });
        }
        else {
            var objectTabelId = "";
            var objectTablePM = window.ObjectTables.filter(function (x) { return x.Name === entity; })[0];
            if (objectTablePM != null) {
                objectTabelId = objectTablePM.Id;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(objectTabelId)) {
                this._entityResourceService.getEntityResourceByTableName(entity).subscribe(function (response) {
                    _this.ViewQuery(objectTabelId, entity);
                });
            }
        }
    };
    GettingStartedComponent.prototype.ViewQuery = function (objectTableId, objectTableName) {
        var _this = this;
        var filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
        var queryCode = "";
        switch (objectTableName.toLocaleLowerCase()) {
            case "customer":
                {
                    queryCode = "Customers";
                    break;
                }
            case "agent":
                {
                    queryCode = "Agents";
                    break;
                }
            case "user":
                {
                    queryCode = "AllUsers";
                    break;
                }
            case "port":
                {
                    queryCode = "Ports";
                    break;
                }
            case "airline":
                {
                    queryCode = "Airlines";
                    break;
                }
            case "shippingline":
                {
                    queryCode = "Shipping lines";
                    break;
                }
        }
        var listArgs = new Args_1.ListComponentArgs();
        listArgs.Filters = filterAgrs;
        listArgs.QueryCode = queryCode;
        listArgs.ObjectTableName = objectTableName;
        listArgs.BackButtonTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("General.MH.GettingStarted");
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run(listArgs);
            _this.CurrentSession.AddMenuReference(cmpRef);
        });
    };
    // LoadGetStartedData
    GettingStartedComponent.prototype.LoadGetStartedData = function () {
        var _this = this;
        var service = new CommonDomainService_1.CommonDomainService();
        service.GetGettingStartedData().subscribe(function (response) {
            if (!response.HasError) {
                var result = response.Result;
                if (result != null) {
                    _this.PortsCount = result.PortsCount;
                    _this.AgentsCount = result.AgentsCount;
                    _this.CustomersCount = result.CustomersCount;
                    _this.UsersCount = result.UsersCount;
                    _this.QuotesCount = result.QuotesCount;
                    _this.ShipmentsCount = result.ShipmentsCount;
                    _this.MastersCount = result.MastersCount;
                    _this.AirlinesCount = result.AirlinesCount;
                    _this.ShippinglinesCount = result.ShippinglinesCount;
                }
            }
        });
    };
    // System Info
    GettingStartedComponent.prototype.SystemInfoClicked = function () {
        var _this = this;
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
    };
    GettingStartedComponent.prototype.CompanyAddressClick = function () {
        var windowTitle = "Company Address Settings";
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 750;
        logWindow.Height = 500;
        logWindow.Title = windowTitle;
        logWindow.IsShowCloseButton = true;
        logWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/CompanyAddress/CompanyAddressSettingsComponent');
    };
    GettingStartedComponent.prototype.SystemDefaultsClick = function () {
        this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe(function (response) {
            var windowTitle = "System Defaults ";
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 850;
            logWindow.Height = 630;
            logWindow.Title = windowTitle;
            logWindow.IsShowCloseButton = true;
            logWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/SystemDefaults/SystemDefaultsComponent');
        });
    };
    GettingStartedComponent.prototype.CompanyCountersClick = function () {
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Title = "Counters";
        logitudeWindow.ShowCloseButton = true;
        logitudeWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/Counters/CountersComponent');
    };
    GettingStartedComponent.prototype.CompanyLogoClick = function () {
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 740;
        logitudeWindow.Height = 585;
        logitudeWindow.DataContext = this;
        logitudeWindow.Title = "Logo Definition";
        logitudeWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/UploadImage/UploadLogoComponent');
    };
    GettingStartedComponent.prototype.SystemCurrenciesClick = function () {
        var windowTitle = "System Currencies";
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = windowTitle;
        logWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/SystemCurrencies/SystemCurrenciesComponent');
    };
    GettingStartedComponent.prototype.AccountingSettingsClick = function () {
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 820;
        logitudeWindow.Height = 570;
        logitudeWindow.Title = "Accounting Settings";
        logitudeWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/AccountingSettings/AccountingSettingsComponent');
    };
    GettingStartedComponent.prototype.LocalSettingsClick = function () {
        this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe(function (response) {
            var windowTitle = "Local Settings";
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 750;
            logWindow.Height = 500;
            logWindow.Title = windowTitle;
            logWindow.IsShowCloseButton = true;
            logWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/LocalSettings/LocalSettingsComponent');
        });
    };
    GettingStartedComponent.prototype.InvoiceSettingsClick = function () {
        this._entityResourceService.getEntityResourceByTableName("Tenant", 0).subscribe(function (response) {
            var windowTitle = "Invoice Settings";
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 750;
            logWindow.Height = 500;
            logWindow.Title = windowTitle;
            logWindow.IsShowCloseButton = true;
            logWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/InvoiceSettings/InvoiceSettingsComponent');
        });
    };
    GettingStartedComponent.prototype.AirlineSettingsClick = function () {
        this._entityResourceService.getEntityResourceByTableName("TenantManagement", 0).subscribe(function (response) {
            var windowTitle = "Airline Settings";
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 650;
            logWindow.Height = 350;
            logWindow.Title = windowTitle;
            logWindow.IsShowCloseButton = true;
            logWindow.Show('./InfrastructureModules/InfrastructureGettingStarted/Components/AirlineSettings/AirlineSettingsComponent');
        });
    };
    GettingStartedComponent.prototype.SignatureClick = function () {
        var _this = this;
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
    };
    GettingStartedComponent.prototype.ChangePasswordClick = function () {
        var _this = this;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 600;
        logitudeWindow.Height = 400;
        logitudeWindow.Title = "Change User Password";
        this._entityResourceService.getEntityResourceByTableName("User").subscribe(function (response) {
            logitudeWindow.DataContext = _this;
            logitudeWindow.Show('./InfrastructureModules/InfrastructureUser/Components/PersonalSettings/ChangePasswordComponent');
        });
    };
    Object.defineProperty(GettingStartedComponent.prototype, "CheckBoxIsEnabled", {
        get: function () { return this.checkBoxIsEnabled; },
        set: function (value) {
            this.checkBoxIsEnabled = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GettingStartedComponent.prototype, "IsDisplayGetStartedChecked", {
        get: function () {
            if (SessionLocator_1.SessionLocator.LoggedUserPM != null) {
                return SessionLocator_1.SessionLocator.LoggedUserPM.DisplayGettingStarted;
            }
            return null;
        },
        set: function (value) {
            if (SessionLocator_1.SessionLocator.LoggedUserPM.DisplayGettingStarted != value) {
                SessionLocator_1.SessionLocator.LoggedUserPM.DisplayGettingStarted = value;
                this.CheckBoxIsEnabled = false;
                this.SubmitOnCheckBox();
            }
        },
        enumerable: true,
        configurable: true
    });
    GettingStartedComponent.prototype.SubmitOnCheckBox = function () {
        var _this = this;
        var service = new CommonDomainService_1.CommonDomainService();
        service.UpdateUserData(this.IsDisplayGetStartedChecked).subscribe(function (response) {
            if (!response.HasError) {
                var result = response.Result;
                _this.CheckBoxIsEnabled = true;
            }
        });
    };
    GettingStartedComponent.prototype.ViewAllResources = function () {
        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Help Center", "View All");
        var uri = 'TrainingResourcesHTML/TrainingResourcesMainPage.aspx?tempId=' + SessionInfo_1.SessionInfo.DocumentDownloadToken;
        var navigate = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + uri;
        window.open(navigate);
    };
    GettingStartedComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './GettingStartedComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], GettingStartedComponent);
    return GettingStartedComponent;
}(BaseComponent_1.BaseComponent));
exports.GettingStartedComponent = GettingStartedComponent;
var HelpResourceArgs = /** @class */ (function () {
    function HelpResourceArgs(help) {
        this.entity = help;
    }
    Object.defineProperty(HelpResourceArgs.prototype, "VideoContent", {
        get: function () { return this.entity.Name + " (" + this.entity.Duration + ")"; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HelpResourceArgs.prototype, "Uri", {
        get: function () { return this.entity.VideoURL; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HelpResourceArgs.prototype, "HowToContent", {
        get: function () { return this.entity.Name; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HelpResourceArgs.prototype, "Code", {
        get: function () { return this.entity.Code; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HelpResourceArgs.prototype, "IsNew", {
        get: function () { return this.entity.IsNew; },
        enumerable: true,
        configurable: true
    });
    HelpResourceArgs.prototype.HowToMethod = function () {
        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Help Center", "How-To");
        var uri = "/WebPages/HowToDownloadPage.aspx?id=" + this.Code;
        window.open(ServiceHelper_1.ServiceHelper.GetLogitudeURL() + uri);
    };
    HelpResourceArgs.prototype.NafigateToURL = function () {
        window.open(this.Uri);
    };
    return HelpResourceArgs;
}());
exports.HelpResourceArgs = HelpResourceArgs;
//# sourceMappingURL=GettingStartedComponent.js.map