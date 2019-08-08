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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var FeatureLocator_1 = require("../../../Infrastructure/Utilities/FeatureLocator");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var CRMDomainService_1 = require("../../Services/CRMDomainService");
var Args_1 = require("../../../Infrastructure/Args");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var OccasionWorkspaceComponent = /** @class */ (function () {
    function OccasionWorkspaceComponent(_entityResourceService) {
        this._entityResourceService = _entityResourceService;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsNewOccasionVisible = false;
        this.AllOccasionsQueryVisibility = false;
        this.AllOccasionsCount = 0;
        this.myDomainService = new CRMDomainService_1.CRMDomainService();
        this.SetQueriesVisibility();
        this.LoadQueriesCounts();
    }
    OccasionWorkspaceComponent.prototype.SetQueriesVisibility = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Occasion", "New")) {
            this.IsNewOccasionVisible = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Occasion", "Occasion.Q.AllOccasions")) {
            this.AllOccasionsQueryVisibility = true;
        }
    };
    OccasionWorkspaceComponent.prototype.NewOccasionClicked = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("OccasionType", 0).subscribe(function (response) {
            var windowTitle = "New Occasion";
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 850;
            logWindow.Height = 700;
            logWindow.Title = windowTitle;
            logWindow.WindowClosed.subscribe(function (s) {
                if (s) {
                    _this.LoadQueriesCounts();
                }
            });
            logWindow.Show('./CRMModules/CRMOccasion/Components/NewEntity/NewOccasionComponent');
        });
    };
    OccasionWorkspaceComponent.prototype.LoadQueriesCounts = function () {
        var _this = this;
        this.myDomainService.GetOccasionsSummary().subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                var myData = myResponse.Result;
                if (myData != null) {
                    _this.AllOccasionsCount = myData.AllOccasionsCount;
                }
            }
        });
    };
    OccasionWorkspaceComponent.prototype.ViewQuery = function (code) {
        var _this = this;
        if (code) {
            var objectTableName = "Occasion";
            var queryCode = code;
            var displayTitle = "";
            var backButtonTitle = "CRM";
            var filterAgrs = new ApiQueryFilters_1.ApiQueryFilters();
            displayTitle = queryCode;
            var listArgs = new Args_1.ListComponentArgs();
            listArgs.Filters = filterAgrs;
            listArgs.QueryCode = queryCode;
            listArgs.ObjectTableName = objectTableName;
            listArgs.DisplayTitle = displayTitle;
            listArgs.BackButtonTitle = backButtonTitle;
            SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/ListComponent/ListComponent', this.CurrentSession.SessionMenuLocation.viewContainerRef)
                .then(function (cmpRef) {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run(listArgs);
                cmpRef.instance.BackCompleted.subscribe(function ($event) { return _this.LoadQueriesCounts(); });
                _this.CurrentSession.AddMenuReference(cmpRef);
            });
            ;
        }
    };
    OccasionWorkspaceComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './OccasionWorkspaceComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], OccasionWorkspaceComponent);
    return OccasionWorkspaceComponent;
}());
exports.OccasionWorkspaceComponent = OccasionWorkspaceComponent;
//# sourceMappingURL=OccasionWorkspaceComponent.js.map