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
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var LocationDirective_1 = require("../../../Infrastructure/Utilities/LocationDirective");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var MainReportsWorkspace = /** @class */ (function () {
    function MainReportsWorkspace(_entityResourceService) {
        this._entityResourceService = _entityResourceService;
        this.IsMenuVisible = false;
        this.IsBIItemVisible = false;
        this.IsReportItemVisible = false;
        this.IsResourcesReady = false;
        this.isLoaderReady = false;
        this.Retries = 0;
        this.Page_BI = null;
        this.Page_Report = null;
    }
    MainReportsWorkspace.prototype.ngOnInit = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("BIReportFolder", 0).subscribe(function (response) {
            _this._entityResourceService.getEntityResourceByTableName("BIReport", 0).subscribe(function (response) {
                _this._entityResourceService.getEntityResourceByTableName("Report", 0).subscribe(function (response) {
                    _this.IsResourcesReady = true;
                    if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("BIReport", "BIReport.Menu")) {
                        _this.IsBIItemVisible = true;
                        _this.IsMenuVisible = true;
                    }
                    if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Report", "Module")) {
                        _this.IsReportItemVisible = true;
                    }
                    _this.RunComponent();
                });
            });
        });
    };
    MainReportsWorkspace.prototype.RunComponent = function () {
        if (this.AllLocations) {
            if (this.AllLocations.length == 0) {
                this.RunComponentTimer();
            }
            else {
                this.isLoaderReady = true;
                this.SetSelectedItem();
            }
        }
        else {
            this.RunComponentTimer();
        }
    };
    MainReportsWorkspace.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
        if (this.Retries < 3) {
            this.timerToken = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    MainReportsWorkspace.prototype.SetSelectedItem = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Report", "Module")) {
            this.SelectedItem = "Report";
        }
        else {
            this.SelectedItem = "BI";
        }
    };
    Object.defineProperty(MainReportsWorkspace.prototype, "SelectedItem", {
        get: function () { return this.selectedItem; },
        set: function (newValue) {
            if (this.selectedItem != newValue) {
                this.selectedItem = newValue;
                this.SelectionChanged();
            }
        },
        enumerable: true,
        configurable: true
    });
    MainReportsWorkspace.prototype.SelectionChanged = function () {
        var _this = this;
        if (this.isLoaderReady) {
            if (this.SelectedItem != null) {
                var myLocation_1 = this.AllLocations.toArray().filter(function (d) { return d.Code == _this.SelectedItem; })[0];
                if (myLocation_1 != null) {
                    switch (this.SelectedItem) {
                        case "Report": {
                            if (this.Page_Report == null) {
                                this._entityResourceService.getEntityResourceByTableName("Booking", 0).subscribe(function (response) {
                                    SessionLocator_1.SessionLocator.DynamicLoader.Load('./Report/Components/Workspaces/ReportComponent', myLocation_1.viewContainerRef)
                                        .then(function (cmpRef) {
                                        _this.Page_Report = cmpRef.instance;
                                        _this.Page_Report.InitComponent();
                                    });
                                });
                            }
                            break;
                        }
                        case "BI": {
                            if (this.Page_BI == null) {
                                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Report/Components/Workspaces/BIFolderReportComponent', myLocation_1.viewContainerRef)
                                    .then(function (cmpRef) {
                                    _this.Page_BI = cmpRef.instance;
                                    _this.Page_BI.InitComponent();
                                });
                            }
                            break;
                        }
                    }
                }
            }
        }
    };
    __decorate([
        core_1.ViewChildren(LocationDirective_1.LocationDirective),
        __metadata("design:type", core_1.QueryList)
    ], MainReportsWorkspace.prototype, "AllLocations", void 0);
    MainReportsWorkspace = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'MainReportsWorkspace',
            templateUrl: './MainReportsWorkspace.html',
            providers: [EntityResourceService_1.EntityResourceService],
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], MainReportsWorkspace);
    return MainReportsWorkspace;
}());
exports.MainReportsWorkspace = MainReportsWorkspace;
//# sourceMappingURL=MainReportsWorkspace.js.map