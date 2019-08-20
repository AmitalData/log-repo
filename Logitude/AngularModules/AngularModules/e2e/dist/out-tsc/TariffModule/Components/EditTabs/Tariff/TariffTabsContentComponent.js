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
var common_1 = require("@angular/common");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var LocationDirective_1 = require("../../../../Infrastructure/Utilities/LocationDirective");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var TariffTabsContentComponent = /** @class */ (function () {
    function TariffTabsContentComponent(entityArgs) {
        this.entityArgs = entityArgs;
        this.Tabs = [];
        this.EditTabTariffType = "VR";
        this.CurrentSessionSelectedEvent = null;
        this.Retries = 0;
        this.Listen();
    }
    TariffTabsContentComponent.prototype.Listen = function () {
        var _this = this;
        this.CurrentSessionSelectedEvent = this.entityArgs.EditComponent.CurrentSession.SessionSeleced.subscribe(function (isSessionSeleced) {
            if (isSessionSeleced) {
                if (!_this.AllLocations) {
                    if (_this.timerToken) {
                        clearTimeout(_this.timerToken);
                    }
                    _this.Retries = 0;
                    _this.RunComponent();
                }
            }
        });
    };
    TariffTabsContentComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.CurrentSessionSelectedEvent);
    };
    TariffTabsContentComponent.prototype.Run = function (args) {
        this.EntityPM = args['EntityPM'];
        if (this.EntityPM.TypeCode == "ASC") {
            this.EditTabTariffType = "SVR";
        }
        this.BuildTabs();
        this.RunComponent();
    };
    TariffTabsContentComponent.prototype.BuildTabs = function () {
        var _this = this;
        this.Tabs = [];
        var datePipe = new common_1.DatePipe("en-US");
        var from = "";
        var to = "";
        var header = "";
        var index = 0;
        var todayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
        var draftVersion = this.EntityPM.TariffVersions.filter(function (d) { return d.IsDraft; })[0];
        if (draftVersion != null) {
            if (this.EntityPM.TypeCode == "ASC") {
                header = "Version " + draftVersion.Version;
            }
            else {
                from = datePipe.transform(draftVersion.StartDate, 'dd/MMM/yy');
                to = datePipe.transform(draftVersion.ExpirationDate, 'dd/MMM/yy');
                header = from + " - " + to;
            }
            this.Tabs.push(new TariffDetailsTab(index, this.EditTabTariffType, header, draftVersion));
            index++;
        }
        this.EntityPM.ActiveVersions.sort(function (a, b) { return (a.Version === b.Version) ? 0 : (a.Version > b.Version) ? -1 : 1; }).forEach(function (item) {
            if (_this.EntityPM.TypeCode == "ASC") {
                header = "Version " + item.Version;
            }
            else {
                from = datePipe.transform(item.StartDate, 'dd/MMM/yy');
                to = datePipe.transform(item.ExpirationDate, 'dd/MMM/yy');
                header = from + " - " + to;
            }
            _this.Tabs.push(new TariffDetailsTab(index, _this.EditTabTariffType, header, item));
            index++;
        });
        this.Tabs.push(new TariffDetailsTab(index + 1, "GN", "General"));
        this.Tabs.push(new TariffDetailsTab(index + 2, "VH", "Version History"));
        this.Tabs.push(new TariffDetailsTab(index + 3, "EV", "Events"));
    };
    TariffTabsContentComponent.prototype.RunComponent = function () {
        var _this = this;
        if (this.AllLocations) {
            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }
            else {
                if (!Tools_1.AppTool.IsNullOrEmpty(this.entityArgs.EditComponent.PreSelectedTabCode)) {
                    var SelectedTab = this.Tabs.filter(function (p) { return p.VersionPM != null ? (p.VersionPM.Version == +_this.entityArgs.EditComponent.PreSelectedTabCode) : 0; })[0];
                    if (SelectedTab) {
                        this.SelectionChanged(SelectedTab);
                    }
                    else {
                        this.SelectionChanged(this.Tabs[0]);
                    }
                    this.entityArgs.EditComponent.PreSelectedTabCode = null;
                }
                else {
                    this.SelectionChanged(this.Tabs[0]);
                }
            }
        }
        else {
            this.RunComponentTimer();
        }
    };
    TariffTabsContentComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    TariffTabsContentComponent.prototype.SelectionChanged = function (clickdTab) {
        var _this = this;
        if (clickdTab != null) {
            if (this.SelectedTabItem != clickdTab) {
                this.SelectedTabItem = clickdTab;
                this.Tabs.forEach(function (item) {
                    item.IsSelected = false;
                });
                this.SelectedTabItem.IsSelected = true;
            }
            if (this.SelectedTabItem.IsTabLoaded) {
            }
            else {
                var locs = this.AllLocations.toArray().filter(function (f) { return f.Code == 'TariffTabLocation'; });
                var location_1 = locs.filter(function (f) { return f.Index == _this.SelectedTabItem.Index; })[0];
                if (location_1) {
                    if (this.SelectedTabItem.IsTabLoaded) {
                    }
                    else if (this.SelectedTabItem.ComponentPath) {
                        SessionLocator_1.SessionLocator.DynamicLoader.Load(this.SelectedTabItem.ComponentPath, location_1.viewContainerRef).then(function (cmpRef) {
                            _this.SelectedTabItem.IsTabLoaded = true;
                            if (_this.SelectedTabItem.VersionPM) {
                                cmpRef.instance.Intialize({ CurrentVersion: _this.SelectedTabItem.VersionPM });
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
    ], TariffTabsContentComponent.prototype, "AllLocations", void 0);
    TariffTabsContentComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './TariffTabsContentComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], TariffTabsContentComponent);
    return TariffTabsContentComponent;
}());
exports.TariffTabsContentComponent = TariffTabsContentComponent;
var TariffDetailsTab = /** @class */ (function () {
    function TariffDetailsTab(index, code, header, version) {
        if (version === void 0) { version = null; }
        this.Header = null;
        this.IsSelected = false;
        this.IsTabLoaded = false;
        this.IsDraft = false;
        this.Index = index;
        this.Code = code;
        this.Header = header;
        switch (this.Code) {
            case "VR": {
                this.IsDraft = version.IsDraft;
                this.VersionPM = version;
                this.ComponentPath = "./TariffModule/Components/EditTabs/Tariff/VersionTabComponent";
                break;
            }
            case "SVR": {
                this.IsDraft = version.IsDraft;
                this.VersionPM = version;
                this.ComponentPath = "./TariffModule/Components/EditTabs/Tariff/SurchargeVersionTabComponent";
                break;
            }
            case "GN": {
                this.ComponentPath = "./TariffModule/Components/EditTabs/Tariff/TariffGeneralTabComponent";
                break;
            }
            case "VH": {
                this.ComponentPath = "./TariffModule/Components/EditTabs/Tariff/VersionHistoryTabComponent";
                break;
            }
            case "EV": {
                this.ComponentPath = "./Common/Components/Events/EventsTabComponent";
                break;
            }
        }
    }
    return TariffDetailsTab;
}());
//# sourceMappingURL=TariffTabsContentComponent.js.map