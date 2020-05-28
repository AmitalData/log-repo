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
var SessionLocator_1 = require("../../Utilities/SessionLocator");
var LocationDirective_1 = require("../../Utilities/LocationDirective");
var Tools_1 = require("../../Tools");
var ObjectsLocator_1 = require("../../Locators/ObjectsLocator");
var LogTabsComponent = /** @class */ (function () {
    function LogTabsComponent(cd) {
        this.cd = cd;
        this.TabsSource = [];
        this.Disabled = false;
        this.NoBorder = false;
        this.IsFixedTabs = false; // disable new tab button
        this.HideCloseButton = false;
        this.AddTabClicked = new core_1.EventEmitter;
        this.CloseTabClicked = new core_1.EventEmitter;
        this.SelectedTabChanged = new core_1.EventEmitter;
        this.IsRTL = false;
        this.IsOverCloseButton = false;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting != undefined) {
            this.IsRTL = ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl" ? true : false;
        }
    }
    LogTabsComponent.prototype.ngAfterViewInit = function () {
        var _this = this;
        this.timerToken = setTimeout(function () {
            if (_this.TabsSource.length > 0) {
                _this.SelectedTab = _this.TabsSource[0];
                //this.LoadTab(this.TabsSource[0]);
                //this.SelectedTabItem = this.TabsSource[0];
                //this.SelectedTabItem.IsSelected = true;
            }
        }, 10);
        ////this.cd.detectChanges();
    };
    Object.defineProperty(LogTabsComponent.prototype, "SelectedTab", {
        get: function () {
            return this.selectedTab;
        },
        set: function (tab) {
            if (this.selectedTab != tab) {
                var t = tab;
                if (Tools_1.AppTool.IsNullOrEmpty(t))
                    return;
                if (t.IsSelected)
                    return;
                this.TabsSource.forEach(function (item) { item.IsSelected = false; }); // Reset Selection
                tab.IsSelected = true;
                this.LoadTab(t);
                this.selectedTab = t;
            }
        },
        enumerable: true,
        configurable: true
    });
    LogTabsComponent.prototype.AddTab = function () {
        if (this.Disabled)
            return;
        this.resetSelection();
        this.AddTabClicked.emit();
        this.cd.detectChanges();
        this.SelectedTab = this.TabsSource[this.TabsSource.length - 1]; // select last tab
    };
    LogTabsComponent.prototype.CloseTab = function (tab) {
        if (this.TabsSource.length <= 1)
            return;
        this.CloseTabClicked.emit(tab);
        this.cd.detectChanges();
        //this.SelectedTab = this.TabsSource[this.TabsSource.length - 1]; // select last tab
    };
    LogTabsComponent.prototype.SelectionChanged = function (tabItem) {
        if (this.IsOverCloseButton)
            return;
        //console.log(this.TabsSource);
        this.SelectedTab = tabItem;
        this.SelectedTabChanged.emit(tabItem);
    };
    LogTabsComponent.prototype.LoadTab = function (tabItem) {
        var _this = this;
        this.cd.detectChanges();
        var locs = this.AllLocations.toArray();
        var myLocation = locs.filter(function (f) { return f.Code == tabItem.Code; })[0];
        if (Tools_1.AppTool.IsNullOrEmpty(tabItem.ComponentPath)) {
            console.warn("No component path in tab!");
            return;
        }
        if (!tabItem.IsTabLoaded) {
            if (myLocation != null) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load(tabItem.ComponentPath, myLocation.viewContainerRef).then(function (cmpRef) {
                    tabItem.IsTabLoaded = true;
                    cmpRef.instance.ComponentRef = cmpRef;
                    tabItem.ComponentReference = cmpRef.instance;
                    cmpRef.instance.SetTabArgs({
                        EntityPM: tabItem.EntityPM,
                        Tab: tabItem,
                        Disabled: _this.Disabled,
                        Parent: tabItem.Parent,
                        DecErrors: tabItem.DecErrors,
                    });
                });
            }
        }
    };
    LogTabsComponent.prototype.resetSelection = function () {
        //unselect all
        this.TabsSource.forEach(function (item) {
            item.IsSelected = false;
        });
        this.SelectedTab = null;
    };
    LogTabsComponent.prototype.onListMouseDown = function (event, tab) {
        console.log(event);
        if (!Tools_1.AppTool.IsNullOrEmpty(event)) {
            if (event.which == '2') { // mouse wheel click
                //this.CloseTab(tab);
            }
        }
    };
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], LogTabsComponent.prototype, "AllLocations", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Array)
    ], LogTabsComponent.prototype, "TabsSource", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogTabsComponent.prototype, "Disabled", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogTabsComponent.prototype, "NoBorder", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogTabsComponent.prototype, "IsFixedTabs", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogTabsComponent.prototype, "HideCloseButton", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], LogTabsComponent.prototype, "AddTabClicked", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], LogTabsComponent.prototype, "CloseTabClicked", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], LogTabsComponent.prototype, "SelectedTabChanged", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", LogTab),
        __metadata("design:paramtypes", [LogTab])
    ], LogTabsComponent.prototype, "SelectedTab", null);
    LogTabsComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'LogTabs',
            templateUrl: "./LogTabsComponent.html",
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], LogTabsComponent);
    return LogTabsComponent;
}());
exports.LogTabsComponent = LogTabsComponent;
var LogTab = /** @class */ (function () {
    function LogTab() {
        this.IsSelected = false;
        this.IsTabLoaded = false;
        this.Index = SessionLocator_1.SessionLocator.Index;
    }
    LogTab.prototype.SetHeader = function (myHeader) {
        this.Header = myHeader;
    };
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], LogTab.prototype, "AllLocations", void 0);
    return LogTab;
}());
exports.LogTab = LogTab;
//# sourceMappingURL=LogTabsComponent.js.map