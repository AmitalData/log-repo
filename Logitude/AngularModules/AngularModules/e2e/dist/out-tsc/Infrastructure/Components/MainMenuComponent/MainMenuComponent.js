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
var LocationDirective_1 = require("../../Utilities/LocationDirective");
var SessionLocator_1 = require("../../Utilities/SessionLocator");
var FeatureLocator_1 = require("../../Utilities/FeatureLocator");
var LastFilterClass_1 = require("../../Utilities/LastFilterClass");
var Tools_1 = require("../../Tools");
var Args_1 = require("../../../Infrastructure/Args");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var ObjectsLocator_1 = require("../../Locators/ObjectsLocator");
var ServiceLocator_1 = require("../../Locators/ServiceLocator");
var MainMenuComponent = /** @class */ (function () {
    function MainMenuComponent() {
        this.MainMenuWidth = 145;
        this.MainMenuWidthCollapsed = 45;
        this.MainMenuWidthOpened = 145;
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.LayoutDirection = 'ltr';
        this.SelectionChanging = new core_1.EventEmitter();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.isLoaderReady = false;
        this.Retries = 0;
        this.isChangingSelected = false;
        this.ClickedMenuItem = null;
        this.ShowFollowUps = false;
        this.FollowUpsTableId = null;
        this.OldObjectTable = null;
        this.pointerEvents = 'all';
        this.isMainSidebarCollapsed = false;
        this.MainMenuItems = new Array();
        this.MainMenuItems = this.GetMainMenuItemsFromWindow();
        // Layout Direction
        this.LayoutDirection = ObjectsLocator_1.ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection;
        var defaultStatus = LastFilterClass_1.LastFilterClass.GetFilterValue("Simplog.Infrastructure.Views.MenuView", "Sidebar");
        if (!Tools_1.AppTool.IsNullOrEmpty(defaultStatus)) {
            this.IsMainSidebarCollapsed = defaultStatus == "true" ? true : false;
        }
        else {
            this.IsMainSidebarCollapsed = SessionLocator_1.SessionLocator.IsMainSidebarCollapsed;
        }
        this.MainMenuWidth = this.IsMainSidebarCollapsed == true ? this.MainMenuWidthCollapsed : this.MainMenuWidthOpened;
    }
    MainMenuComponent.prototype.GetMainMenuItemsFromWindow = function () {
        var myResult = [];
        window.MenusTables.filter(function (f) { return f.MenuTypeCode.toUpperCase() == "MAIN"; }).forEach(function (item) {
            var isAddingItem = false;
            if (item.FeatureId == null) {
                isAddingItem = true;
            }
            else {
                if (FeatureLocator_1.FeatureLocator.IsFeatureGranted(item.FeatureId)) {
                    isAddingItem = true;
                }
            }
            if (isAddingItem) {
                var menuItem = new MainMenuItem(item.TextCode, Tools_1.AppTool.GetMainMenuIconCode(item.TextCode));
                menuItem.IndexOfOrder = item.IndexOfOrder;
                menuItem.ObjectTableId = item.ObjectTableId;
                menuItem.HtmlView = item.HtmlView;
                menuItem.ObjectTableName = item.ObjectTableName;
                myResult.push(menuItem);
            }
        });
        myResult = myResult.sort(function (a, b) { return a.IndexOfOrder - b.IndexOfOrder; });
        return myResult;
    };
    MainMenuComponent.prototype.RunComponent = function () {
        if (this.AllLocations) {
            if (this.AllLocations.toArray().length == 0) {
                this.RunComponentTimer();
            }
            else {
                this.isLoaderReady = true;
                var locs = this.AllLocations.toArray().filter(function (f) { return f.Code == 'MainMenuContainer'; });
                var myLocation = locs[0];
                this.CurrentSession.SessionMenuLocation = myLocation;
                this.InitSelectedMenu();
            }
        }
        else {
            this.RunComponentTimer();
        }
    };
    MainMenuComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    MainMenuComponent.prototype.InitSelectedMenu = function () {
        var mySelectedMenu = this.MainMenuItems[0];
        var selectedMenuTextCode = null;
        if (SessionLocator_1.SessionLocator.IsExternalParams) {
            if (SessionLocator_1.SessionLocator.ExternalParams != null) {
                switch (SessionLocator_1.SessionLocator.ExternalParams.Menu) {
                    case "Tickets": {
                        selectedMenuTextCode = "General.MH.Ticket";
                        break;
                    }
                    case "LogBox": {
                        selectedMenuTextCode = "General.MH.Importers";
                        break;
                    }
                    case "DAPP": {
                        selectedMenuTextCode = "General.MH.Importers";
                        break;
                    }
                    case "protractor": {
                        selectedMenuTextCode = "General.MH.Maintenance";
                        break;
                    }
                }
            }
        }
        else {
            if (SessionLocator_1.SessionLocator.LoggedUserPM.DisplayGettingStarted && this.CurrentSession.SessionIndex == 0) {
                selectedMenuTextCode = "General.MH.GettingStarted";
            }
            else {
                selectedMenuTextCode = LastFilterClass_1.LastFilterClass.GetFilterValue("Simplog.Infrastructure.Views.MenuView", "MainMenu");
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(selectedMenuTextCode)) {
            var mySelectedItem = this.MainMenuItems.filter(function (m) { return m.TextCode == selectedMenuTextCode; })[0];
            if (mySelectedItem != null) {
                mySelectedMenu = mySelectedItem;
            }
        }
        this.SelectionChanged(mySelectedMenu);
    };
    MainMenuComponent.prototype.BlockScreenLoad = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.BlockType)) {
            this.CurrentSession.DestroyMenuReferences();
            this.CurrentSession.DestroyListComponentReferences();
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/LoginComponent/BlockScreenComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
            });
        }
    };
    MainMenuComponent.prototype.SelectionChanged = function (item) {
        if (this.ClickedMenuItem != item) {
            this.ClickedMenuItem = item;
            // Code
            if (!Tools_1.AppTool.IsNullOrEmpty(SessionLocator_1.SessionLocator.BlockType)) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/LoginComponent/BlockScreenComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.ComponentRef = cmpRef;
                });
            }
            else {
                this.SelectionChanging.emit(true);
                var isSubscribed = false;
                if (this.SelectionChanging) {
                    if (this.SelectionChanging.observers) {
                        if (this.SelectionChanging.observers.length > 0) {
                            isSubscribed = true;
                        }
                    }
                }
                if (isSubscribed == false) {
                    this.ChangeMenu();
                }
            }
        }
        else {
            this.SelectionChanging.emit(true);
        }
    };
    MainMenuComponent.prototype.ChangeMenu = function () {
        if (!this.isChangingSelected) {
            this.isChangingSelected = true;
            if (this.SelectedMenu != this.ClickedMenuItem) {
                this.SelectedMenu = this.ClickedMenuItem;
                this.ChangeScreen();
            }
            else {
                this.isChangingSelected = false;
            }
        }
    };
    // count: number = 0;
    MainMenuComponent.prototype.ChangeScreen = function () {
        var _this = this;
        if (this.isLoaderReady) {
            this.ShowFollowUps = false;
            var myComponentPath = null;
            var isListComponent = false;
            //if (this.count % 2 == 0) {
            this.CurrentSession.DestroyMenuReferences();
            this.CurrentSession.DestroyListComponentReferences();
            // this.count++;
            // }
            if (!SessionLocator_1.SessionLocator.IsExternalParams) {
                var defaultFilterCode = LastFilterClass_1.LastFilterClass.GetFilterValue("Simplog.Infrastructure.Views.MenuView", "MainMenu");
                if (defaultFilterCode != this.SelectedMenu.TextCode) {
                    LastFilterClass_1.LastFilterClass.UpdateFilter("Simplog.Infrastructure.Views.MenuView", "MainMenu", this.SelectedMenu.TextCode);
                }
            }
            if (this.SelectedMenu != null) {
                switch (this.SelectedMenu.TextCode) {
                    case "General.MH.Operations": {
                        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Operations", "Main View");
                        myComponentPath = "./Shipment/Components/Workspaces/OperationsComponent";
                        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "Shipment.Followups")) {
                            this.FollowUpsTableId = this.SelectedMenu.ObjectTableId;
                            this.ShowFollowUps = true;
                        }
                        break;
                    }
                    case "General.MH.ContainersFU": {
                        myComponentPath = "./Shipment/Components/Workspaces/ContainersFUsComponent";
                        break;
                    }
                    case "General.MH.TimeManagement": {
                        myComponentPath = "./TimeManagement/Components/Workspaces/TimeManagementWorkspaceComponent";
                        break;
                    }
                    case "General.MH.TariffModule": {
                        myComponentPath = "./TariffModule/Components/Workspaces/TariffModuleWorkspaceComponent";
                        break;
                    }
                    case "General.MH.FilingInbox": {
                        myComponentPath = "./CommonModules/CommonFilingInbox/Components/FilingInboxWorkspaceComponent";
                        break;
                    }
                    case "General.MH.Quotes": {
                        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Quote", "List View");
                        myComponentPath = "./Quote/Components/Workspaces/QuotesComponent";
                        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Quote", "Quote.Followups")) {
                            this.FollowUpsTableId = this.SelectedMenu.ObjectTableId;
                            this.ShowFollowUps = true;
                        }
                        break;
                    }
                    case "General.MH.Dashboard": {
                        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "Main View");
                        myComponentPath = "./Dashboard/Components/Workspace/DashboardComponent";
                        break;
                    }
                    case "General.MH.AirlineDashboard": {
                        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "Main View");
                        myComponentPath = "./Dashboard/Components/Workspace/AirLineDashboardComponent";
                        break;
                    }
                    case "General.MH.CRM": {
                        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("CRM", "Main View");
                        myComponentPath = "./CRM/Components/Workspaces/CRMWorkspaceComponent";
                        break;
                    }
                    case "General.MH.Ticket": {
                        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Ticket", "List View");
                        myComponentPath = "./CRM/Components/Workspaces/TicketsWorkspaceComponent";
                        break;
                    }
                    case "General.MH.Importers": {
                        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Importers", "Main View");
                        myComponentPath = "./ShipmentModules/ShipmentLogBox/Components/Logbox/LogBoxMainComponent";
                        break;
                    }
                    case "General.MH.Accounting": {
                        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Accounting", "Main View");
                        myComponentPath = "./Invoice/Components/Workspaces/InvoiceComponent";
                        break;
                    }
                    case "General.MH.Reports": {
                        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Reports", "Main View");
                        //myComponentPath = "./Report/Components/Workspaces/ReportComponent";
                        myComponentPath = "./Report/Components/Workspaces/MainReportsWorkspace";
                        break;
                    }
                    case "General.MH.Maintenance": {
                        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Maintenance", "Main View");
                        myComponentPath = "./Infrastructure/Components/Maintenance/MaintenanceComponent";
                        break;
                    }
                    case "General.MH.GettingStarted": {
                        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("GettingStarted", "Main View");
                        myComponentPath = "./InfrastructureModules/InfrastructureGettingStarted/Components/Workspaces/GettingStartedComponent";
                        break;
                    }
                    case "General.MH.FullAccounting": {
                        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("FullAccounting", "Main View");
                        myComponentPath = "./Accounting/Components/Workspaces/AccountingWorkspaceComponent";
                        break;
                    }
                    case "General.MH.SharedLogistics": {
                        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("SharedLogistics", "Main View");
                        myComponentPath = "./SharedLogistics/Components/SharedLogisticMainMenuComponent";
                        break;
                    }
                    case "General.MH.Shipments": {
                        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("SharedLogistics", "Main View");
                        myComponentPath = "./SharedLogistics/Components/Workspaces/SharedShipmentsWorkspaceComponent";
                        break;
                    }
                    case "General.MH.Invoices": {
                        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("SharedLogistics", "Main View");
                        myComponentPath = "./SharedLogistics/Components/Workspaces/SharedInvoicesWorkspaceComponent";
                        break;
                    }
                    case "General.MH.ActivationWizard": {
                        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("ActivationWizard", "Main View");
                        myComponentPath = "./InfrastructureModules/InfrastructureOthers/Components/ActivationWizard/ActivationWizardComponent";
                        break;
                    }
                    case "General.MH.Customers": {
                        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Customers", "List View");
                        var listArgs = new Args_1.ListComponentArgs();
                        listArgs.Perspective = "customers";
                        listArgs.QueryCode = "Customers";
                        listArgs.ObjectTableName = "Customer";
                        listArgs.DisplayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate(this.SelectedMenu.TextCode);
                        listArgs.HideBackButton = true;
                        this._entityResourceService.getEntityResourceByTableName("Customer", 0).subscribe(function (response) {
                            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                                .then(function (cmpRef) {
                                cmpRef.instance.ComponentRef = cmpRef;
                                cmpRef.instance.Run(listArgs);
                                _this.CurrentSession.AddMenuReference(cmpRef);
                                _this.ChangeSessionHeader(_this.SelectedMenu);
                                _this.isChangingSelected = false;
                                //this.pointerEvents = 'all';
                            });
                        });
                        break;
                    }
                    case "General.MH.Contacts": {
                        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Contacts", "List View");
                        var listArgs = new Args_1.ListComponentArgs();
                        listArgs.QueryCode = "Contacts";
                        listArgs.ObjectTableName = "Contact";
                        listArgs.DisplayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate(this.SelectedMenu.TextCode);
                        listArgs.HideBackButton = true;
                        this._entityResourceService.getEntityResourceByTableName("Contact", 0).subscribe(function (response) {
                            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                                .then(function (cmpRef) {
                                cmpRef.instance.ComponentRef = cmpRef;
                                cmpRef.instance.Run(listArgs);
                                _this.CurrentSession.AddMenuReference(cmpRef);
                                _this.ChangeSessionHeader(_this.SelectedMenu);
                                _this.isChangingSelected = false;
                                // this.pointerEvents = 'all';
                            });
                        });
                        break;
                    }
                    case "General.MH.CustomsCollateral": {
                        var listArgs = new Args_1.ListComponentArgs();
                        listArgs.QueryCode = "OpenCollaterals";
                        listArgs.ObjectTableName = "Customs.CustomsCollateral";
                        //  listArgs.DisplayTitle = TextCodeTranslator.Translate(this.SelectedMenu.TextCode);
                        listArgs.HideBackButton = true;
                        this._entityResourceService.getEntityResourceByTableName("Customs.CustomsCollateral", 0).subscribe(function (response) {
                            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                                .then(function (cmpRef) {
                                cmpRef.instance.ComponentRef = cmpRef;
                                cmpRef.instance.Run(listArgs);
                                _this.CurrentSession.AddMenuReference(cmpRef);
                                _this.ChangeSessionHeader(_this.SelectedMenu);
                                _this.isChangingSelected = false;
                                //this.pointerEvents = 'all';
                            });
                        });
                        break;
                    }
                    case "General.MH.Social": { // Abed Code
                        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Social", "Main View");
                        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Social/Components/SocialMainComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                            .then(function (cmpRef) {
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.InitializeSocialMainComponent(null);
                            _this.CurrentSession.AddMenuReference(cmpRef);
                            _this.ChangeSessionHeader(_this.SelectedMenu);
                            _this.isChangingSelected = false;
                            //this.pointerEvents = 'all';
                        });
                        break;
                    }
                    case "General.MH.PaymentOrders": {
                        var listArgs = new Args_1.ListComponentArgs();
                        listArgs.ObjectTableName = "Customs.PaymentOrder";
                        listArgs.NewButtonLabel = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.General.O.NewPaymentOrder");
                        listArgs.HideBackButton = true;
                        this._entityResourceService.getEntityResourceByTableName("Customs.PaymentOrder", 0).subscribe(function (response) {
                            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                                .then(function (cmpRef) {
                                cmpRef.instance.ComponentRef = cmpRef;
                                cmpRef.instance.Run(listArgs);
                                _this.CurrentSession.AddMenuReference(cmpRef);
                                _this.ChangeSessionHeader(_this.SelectedMenu);
                                _this.isChangingSelected = false;
                                //this.pointerEvents = 'all';
                            });
                        });
                        break;
                    }
                    case "General.MH.AirlineDashboard": {
                        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Dashboard", "Main View");
                        myComponentPath = "./Dashboard/Components/Workspace/AirLineDashboardComponent";
                        break;
                    }
                    case "General.MH.CrossDocks": { // Abed Code
                        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("CrossDocks", "Main View");
                        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Warehouse/Components/Workspaces/WarehouseWorkspaceComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                            .then(function (cmpRef) {
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.InitComponent();
                            _this.CurrentSession.AddMenuReference(cmpRef);
                            _this.ChangeSessionHeader(_this.SelectedMenu);
                            _this.isChangingSelected = false;
                            //this.pointerEvents = 'all';
                        });
                        break;
                    }
                    case "General.MH.DeclarationCargoSplits": {
                        var listArgs = new Args_1.ListComponentArgs();
                        listArgs.QueryCode = "OpenCargoSplits";
                        listArgs.ObjectTableName = "Customs.DeclarationCargoSplit";
                        //  listArgs.DisplayTitle = TextCodeTranslator.Translate(this.SelectedMenu.TextCode);
                        listArgs.HideBackButton = true;
                        this._entityResourceService.getEntityResourceByTableName("Customs.DeclarationCargoSplit", 0).subscribe(function (response) {
                            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                                .then(function (cmpRef) {
                                cmpRef.instance.ComponentRef = cmpRef;
                                cmpRef.instance.Run(listArgs);
                                _this.CurrentSession.AddMenuReference(cmpRef);
                                _this.ChangeSessionHeader(_this.SelectedMenu);
                                _this.isChangingSelected = false;
                                //this.pointerEvents = 'all';
                            });
                        });
                        break;
                    }
                    case "General.MH.Tasks": {
                        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Tasks", "Main View");
                        myComponentPath = "./InfrastructureModules/InfrastructureBusinessProcess/Components/Workspaces/TasksWorkspaceComponent";
                        break;
                    }
                    case "General.MH.Depositions": {
                        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Customs Shipper", "List View");
                        var listArgs = new Args_1.ListComponentArgs();
                        listArgs.QueryCode = "AllDepositionsQuery";
                        listArgs.ObjectTableName = "CustomsShipper";
                        listArgs.DisplayTitle = TextCodeTranslator_1.TextCodeTranslator.Translate(this.SelectedMenu.TextCode);
                        listArgs.HideBackButton = true;
                        this._entityResourceService.getEntityResourceByTableName("CustomsShipper", 0).subscribe(function (response) {
                            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                                .then(function (cmpRef) {
                                cmpRef.instance.ComponentRef = cmpRef;
                                cmpRef.instance.Run(listArgs);
                                _this.CurrentSession.AddMenuReference(cmpRef);
                                _this.ChangeSessionHeader(_this.SelectedMenu);
                                _this.isChangingSelected = false;
                                // this.pointerEvents = 'all';
                            });
                        });
                        break;
                    }
                    default: {
                        if (this.SelectedMenu.ObjectTableName) {
                            ServiceLocator_1.ServiceLocator.SendTotangoUserActivity(this.SelectedMenu.ObjectTableName, "List View");
                        }
                        if (this.SelectedMenu.HtmlView == null || this.SelectedMenu.HtmlView == undefined || this.SelectedMenu.HtmlView.indexOf('/ListComponent/ListComponent') > -1) {
                            if (this.SelectedMenu.ObjectTableId != null && this.SelectedMenu.ObjectTableId != undefined) {
                                isListComponent = true;
                                var listArgs = new Args_1.ListComponentArgs();
                                var objectTable = window.ObjectTables.filter(function (x) { return x.Id === _this.SelectedMenu.ObjectTableId; })[0];
                                if (objectTable != null && objectTable != undefined) {
                                    listArgs.ObjectTableName = objectTable.Name;
                                    //listArgs.DisplayTitle = TextCodeTranslator.Translate(this.SelectedMenu.TextCode);
                                    listArgs.HideBackButton = true;
                                    if (Tools_1.AppTool.IsNullOrEmpty(listArgs.NewButtonLabel)) {
                                        var tempText = TextCodeTranslator_1.TextCodeTranslator.Translate(listArgs.ObjectTableName + ".NewButton");
                                        if (!Tools_1.AppTool.IsNullOrEmpty(tempText)) {
                                            listArgs.NewButtonLabel = tempText;
                                        }
                                    }
                                    this._entityResourceService.getEntityResourceByTableName(objectTable.Name, 0).subscribe(function (response) {
                                        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', _this.CurrentSession.SessionMenuLocation.viewContainerRef)
                                            .then(function (cmpRef) {
                                            cmpRef.instance.ComponentRef = cmpRef;
                                            cmpRef.instance.Run(listArgs);
                                            _this.CurrentSession.AddMenuReference(cmpRef);
                                            _this.ChangeSessionHeader(_this.SelectedMenu);
                                            _this.isChangingSelected = false;
                                            //this.pointerEvents = 'all';
                                        });
                                    });
                                }
                            }
                            else {
                                this.isChangingSelected = false;
                            }
                        }
                        else {
                            myComponentPath = this.SelectedMenu.HtmlView;
                        }
                    }
                }
            }
            if (myComponentPath != null) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load(myComponentPath, this.CurrentSession.SessionMenuLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    if (_this.SelectedMenu.TextCode == 'General.MH.CustomsRequestsSheets') {
                        if (cmpRef.entityArgs) {
                            cmpRef.entityArgs.ObjectTableName = null;
                            cmpRef.entityArgs.EntityPM = null;
                        }
                    }
                    _this.CurrentSession.DestroyMenuReferences();
                    _this.CurrentSession.DestroyListComponentReferences();
                    _this.CurrentSession.AddMenuReference(cmpRef);
                    _this.ChangeSessionHeader(_this.SelectedMenu);
                    _this.isChangingSelected = false;
                    //this.pointerEvents = 'all';
                });
            }
            else if (!isListComponent) {
                this.isChangingSelected = false;
                //this.pointerEvents = 'all';
            }
        }
        else {
            this.isChangingSelected = false;
            // this.pointerEvents = 'all';
        }
    };
    MainMenuComponent.prototype.ChangeSessionHeader = function (menu) {
        this.CurrentSession.ChangeSessionHeader({ MenuTextCode: menu.TextCode });
    };
    Object.defineProperty(MainMenuComponent.prototype, "IsMainSidebarCollapsed", {
        get: function () { return this.isMainSidebarCollapsed; },
        set: function (value) {
            if (this.isMainSidebarCollapsed != value) {
                this.isMainSidebarCollapsed = value;
                SessionLocator_1.SessionLocator.IsMainSidebarCollapsed = value;
                LastFilterClass_1.LastFilterClass.UpdateFilter("Simplog.Infrastructure.Views.MenuView", "Sidebar", value + "");
                this.MainMenuWidth = value == true ? this.MainMenuWidthCollapsed : this.MainMenuWidthOpened;
            }
        },
        enumerable: true,
        configurable: true
    });
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], MainMenuComponent.prototype, "AllLocations", void 0);
    __decorate([
        core_1.ViewChild("MainMenuContainer", { read: core_1.ViewContainerRef }),
        __metadata("design:type", core_1.ViewContainerRef)
    ], MainMenuComponent.prototype, "viewContainerRef", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], MainMenuComponent.prototype, "SelectionChanging", void 0);
    MainMenuComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './MainMenuComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], MainMenuComponent);
    return MainMenuComponent;
}());
exports.MainMenuComponent = MainMenuComponent;
var MainMenuItem = /** @class */ (function () {
    function MainMenuItem(textCode, myIcon) {
        this.TextCode = textCode;
        this.IconCode = myIcon;
        this.IconSource = "./Images/Menu/" + myIcon + ".png";
        this.IconSelectedSource = "./Images/Menu/" + myIcon + ".Selected.png";
    }
    return MainMenuItem;
}());
exports.MainMenuItem = MainMenuItem;
//# sourceMappingURL=MainMenuComponent.js.map