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
var EntityArgs_1 = require("../DataContracts/EntityArgs");
var SessionLocator_1 = require("../Utilities/SessionLocator");
var EntityPMService_1 = require("../Services/EntityPMService");
var FeatureLocator_1 = require("../Utilities/FeatureLocator");
var Tools_1 = require("../Tools");
var EntityResourceService_1 = require("../Services/EntityResourceService");
var LocationDirective_1 = require("../Utilities/LocationDirective");
var CachedDataManager_1 = require("../Utilities/CachedDataManager");
var CommonDomainService_1 = require("../../Common/Services/CommonDomainService");
var ObjectsLocator_1 = require("../Locators/ObjectsLocator");
var NewEntityComponent = /** @class */ (function () {
    function NewEntityComponent(entityArgs, entityPMService, CD, _entityResourceService) {
        this.entityArgs = entityArgs;
        this.entityPMService = entityPMService;
        this.CD = CD;
        this._entityResourceService = _entityResourceService;
        this.ValidationErrorsList = [];
        this.LayoutDirection = 'ltr';
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.isViewInited = false;
        this.Retries = 0;
        this.TabsItemsSource = [];
        this.LoadedTabsList = [];
        this.LayoutDirection = ObjectsLocator_1.ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection;
    }
    NewEntityComponent.prototype.SetWindowArgs = function (Args) {
        var _this = this;
        this.ScreenCode = Args.ObjectTableName + ".GeneralTabScreen";
        this.EntityPM = Args.EntityPM;
        this.ObjectTableName = Args.ObjectTableName;
        this.ObjectTableId = window.ObjectTables.filter(function (x) { return x.Name === _this.ObjectTableName; })[0].Id;
        this.ObjectTable = window.ObjectTables.filter(function (x) { return x.Name === _this.ObjectTableName; })[0];
        this.entityArgs.EntityPM = this.EntityPM;
        this.entityArgs.ObjectTableName = this.ObjectTableName;
        this.entityArgs.IsNewEntity = true;
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe(function (response) {
            _this.InitEntityPM();
            _this.BuildEditTabs();
            _this.RunComponent();
        });
    };
    NewEntityComponent.prototype.RunComponent = function () {
        if (this.AllLocations) {
            if (this.AllLocations.toArray().length == 0) {
                this.RunComponentTimer();
            }
            else {
                this.isViewInited = true;
                this.InitializeComponent();
            }
        }
        else {
            this.RunComponentTimer();
        }
    };
    NewEntityComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    NewEntityComponent.prototype.InitializeComponent = function () {
        this.SetSelectedTab();
    };
    NewEntityComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    NewEntityComponent.prototype.OkButtonClicked = function () {
        this.SaveEntityChanges();
    };
    NewEntityComponent.prototype.SaveEntityChanges = function () {
        var _this = this;
        //if (this.EntityPM.IsDirty) {
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving ...");
        this.entityPMService.insert(this.ObjectTableName, this.EntityPM).then(function (res) {
            res.subscribe(function (response) {
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                var mm = response;
                if (!mm.HasError) {
                    _this.ValidationErrorsList = [];
                    if (_this.ObjectTable.CacheOnClient === true) {
                        CachedDataManager_1.CachedDataManager.RefreshTableData(_this.ObjectTableName, true);
                    }
                    if (_this.ObjectTableName == 'VatType') {
                        var myCommonDomain = new CommonDomainService_1.CommonDomainService();
                        myCommonDomain.GetAllVatTypesGroups().subscribe(function (myResponse) {
                            if (!myResponse.HasError) {
                                SessionLocator_1.SessionLocator.AllVatTypesGroups = myResponse.Result;
                            }
                            _this.CurrentSession.CloseCurrentWindowEmit(mm.Result["Id"]);
                        });
                    }
                    else {
                        _this.CurrentSession.CloseCurrentWindowEmit(mm.Result["Id"]);
                    }
                }
                else {
                    _this.ValidationErrorsList = mm.ErrorsArray;
                }
            }, function (error) {
                //var allErrors: string[] = [];
                //allErrors.push(error['message']);
                //this.ValidationErrorsList = allErrors;
                console.log("Error===========>", error);
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            });
        });
        // }
    };
    NewEntityComponent.prototype.InitEntityPM = function () {
        switch (this.ObjectTableName) {
            case "Incoterm":
            case "State":
            case "CountryCity":
            case "EventType":
                {
                    this.EntityPM.AddedManually = true;
                    break;
                }
            case "VatType": {
                this.EntityPM.AddedManually = true;
                this.EntityPM.InActive = false;
                this.EntityPM.IsMultiPercentage = false;
                break;
            }
        }
    };
    NewEntityComponent.prototype.BuildEditTabs = function () {
        var _this = this;
        var myTabsSorted = [];
        var allTabs = [];
        this.TabsItemsSource = [];
        allTabs = window.ObjectTableTabs.filter(function (d) { return d.ObjectTableId === _this.ObjectTableId; });
        //allTabs = this.FilterTabs(allTabs);
        allTabs = allTabs.sort(function (a, b) { return a.IndexOrder - b.IndexOrder; });
        for (var i = 0; i < allTabs.length; i++) {
            var tab = allTabs[i];
            if (tab.ControlPath.indexOf("EventsControl") == -1 && tab.HtmlComponentName != "ReportTemplateComponent") {
                if (FeatureLocator_1.FeatureLocator.IsFeatureGranted(tab.FeatureId)) {
                    myTabsSorted.push(tab);
                }
            }
        }
        if (this.ObjectTableName == 'VatType') {
            myTabsSorted = myTabsSorted.filter(function (f) { return f.Code != 'VTPC'; });
        }
        myTabsSorted.forEach(function (item) {
            var itemTab = new TabItem(item);
            _this.TabsItemsSource.push(itemTab);
        });
        this.CD.detectChanges();
    };
    NewEntityComponent.prototype.SetSelectedTab = function () {
        if (this.TabsItemsSource != null) {
            var selected = null;
            if (selected == null) {
                selected = this.TabsItemsSource[0];
            }
            this.SelectionChanged(selected);
        }
    };
    NewEntityComponent.prototype.SelectionChanged = function (mySelectedTab) {
        if (this.SelectedTab != mySelectedTab) {
            this.SelectedTab = mySelectedTab;
            this.CD.detectChanges();
            if (this.LoadedTabsList == null) {
                this.LoadedTabsList = [];
            }
            if (this.LoadedTabsList.filter(function (d) { return d.Code == mySelectedTab.Code; })[0] == null) {
                var myLoadedTabItem = new LoadedTabItem(mySelectedTab.Code);
                this.LoadedTabsList.push(myLoadedTabItem);
                var myComponentName = null;
                var myComponentPath = null;
                switch (mySelectedTab.EntityPM.ControlPath) {
                    case "Simplog.Infrastructure.GeneralControls.GeneralTabControl": {
                        if (!Tools_1.AppTool.IsNullOrEmpty(mySelectedTab.EntityPM.HtmlComponentUrl)) {
                            myComponentPath = mySelectedTab.EntityPM.HtmlComponentUrl;
                            myComponentName = Tools_1.AppTool.GetComponentName(myComponentPath);
                        }
                        else {
                            myComponentName = "GeneralTabComponent";
                            myComponentPath = "./Infrastructure/GenericComponents/GeneratedComponent";
                        }
                        break;
                    }
                    case "Simplog.Infrastructure.GeneralControls.BillingTabControl": {
                        if (!Tools_1.AppTool.IsNullOrEmpty(mySelectedTab.EntityPM.HtmlComponentUrl)) {
                            myComponentPath = mySelectedTab.EntityPM.HtmlComponentUrl;
                            myComponentName = Tools_1.AppTool.GetComponentName(myComponentPath);
                        }
                        else {
                            myComponentName = "BillingTabComponent";
                            myComponentPath = "./CommonModules/CommonPartners/Components/EditTabs/BillingTabComponent";
                        }
                        break;
                    }
                    case "Simplog.Infrastructure.Views.Events.EventsControl": {
                        myComponentName = "EventsTabComponent";
                        myComponentPath = "./Common/Components/Events/EventsTabComponent";
                        break;
                    }
                    case "Simplog.Infrastructure.Views.Communications.CommunicationsControl": {
                        myComponentName = "CommunicationsTabComponent";
                        myComponentPath = "./InfrastructureModules/InfrastructureCommunications/Components/Communications/CommunicationsTabComponent";
                        break;
                    }
                    case "Simplog.FreightLib.Views.PartnersTabs.PartnerContactsTab": {
                        myComponentName = "ContactsTabComponent";
                        myComponentPath = "./CommonModules/CommonPartners/Components/EditTabs/ContactsTabComponent";
                        break;
                    }
                    case "Simplog.FreightLib.Views.PartnersTabs.PartnerAddressesTab": {
                        myComponentName = "AddressesTabComponent";
                        myComponentPath = "./CommonModules/CommonPartners/Components/EditTabs/AddressesTabComponent";
                        break;
                    }
                    // Ayman: no need for this the HtmlComponentUrl is enough (also for the apove cases)
                    //case "Simplog.FreightLib.Views.PartnersTabs.PartnersAccountingTab.PaymentTermAccountingTabControl": {
                    //    myComponentName = "PaymentTermAccountingTabComponent";
                    //    myComponentPath = "./Common/Components/Partners/EditTabs/PartnersAccountingTab/PaymentTermAccountingTabComponent";
                    //    break;
                    //}
                    default: {
                        if (mySelectedTab.EntityPM.HtmlComponentUrl != null) {
                            myComponentPath = mySelectedTab.EntityPM.HtmlComponentUrl;
                            myComponentName = Tools_1.AppTool.GetComponentName(myComponentPath);
                        }
                        break;
                    }
                }
                if (myComponentPath != null) {
                    myLoadedTabItem.ComponentName = myComponentName;
                    myLoadedTabItem.ComponentPath = myComponentPath;
                    this.LoadTabComponent(myLoadedTabItem);
                }
            }
        }
    };
    NewEntityComponent.prototype.LoadTabComponent = function (loadedItem) {
        var _this = this;
        if (loadedItem != null) {
            if (loadedItem.ComponentPath != null) {
                if (!loadedItem.IsLoaded) {
                    var locs = this.AllLocations.toArray().filter(function (f) { return f.Code == 'NewTabLocation'; });
                    var myLocation = locs.filter(function (f) { return f.ItemCode == loadedItem.Code; })[0];
                    if (myLocation != null) {
                        SessionLocator_1.SessionLocator.DynamicLoader.Load(loadedItem.ComponentPath, myLocation.viewContainerRef)
                            .then(function (cmpRef) {
                            loadedItem.IsLoaded = true;
                            if (_this.SelectedTab.EntityPM.ControlPath == "Simplog.Infrastructure.GeneralControls.GeneralTabControl") {
                                if (Tools_1.AppTool.IsNullOrEmpty(_this.SelectedTab.EntityPM.HtmlComponentUrl)) {
                                    cmpRef.instance.Run(_this.EntityPM, _this.entityArgs.ObjectTableName, _this.ScreenCode, true, true);
                                }
                            }
                        });
                    }
                }
            }
        }
    };
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], NewEntityComponent.prototype, "AllLocations", void 0);
    NewEntityComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './NewEntityComponent.html',
            providers: [EntityArgs_1.EntityArgs]
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityPMService_1.EntityPMService, core_1.ChangeDetectorRef, EntityResourceService_1.EntityResourceService])
    ], NewEntityComponent);
    return NewEntityComponent;
}());
exports.NewEntityComponent = NewEntityComponent;
var TabItem = /** @class */ (function () {
    function TabItem(itemPM) {
        this.IsDisabled = false;
        this.Code = itemPM.Code;
        this.EntityPM = itemPM;
        this.TextCode = itemPM.TabNameTextCodeCode;
    }
    return TabItem;
}());
var LoadedTabItem = /** @class */ (function () {
    function LoadedTabItem(myCode) {
        this.IsLoaded = false;
        this.Code = myCode;
    }
    return LoadedTabItem;
}());
//# sourceMappingURL=NewEntityComponent.js.map