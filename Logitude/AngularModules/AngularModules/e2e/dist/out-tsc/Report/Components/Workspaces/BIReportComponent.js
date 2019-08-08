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
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var Tools_1 = require("../../../Infrastructure/Tools");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var BIReportListService_1 = require("../../../Infrastructure/Services/StandardLists/BIReportListService");
var InfrastructureDomainService_1 = require("../../../Infrastructure/Services/InfrastructureDomainService");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var BIReportComponent = /** @class */ (function () {
    function BIReportComponent(entityResourceService) {
        this.entityResourceService = entityResourceService;
        this.ItemsSource = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.mySearchText = null;
        this.InfrastructureDomainService = new InfrastructureDomainService_1.InfrastructureDomainService();
        this.LoadData();
        this.Listen();
    }
    BIReportComponent.prototype.Listen = function () {
        var _this = this;
        this.CurrentSession.SessionEvent.subscribe(function (s) {
            if (s == "BIRefresh") {
                _this.LoadData();
            }
        });
    };
    BIReportComponent.prototype.InitComponent = function () {
    };
    BIReportComponent.prototype.LoadData = function () {
        var _this = this;
        this.ItemsSource = [];
        this.BIReportListService = new BIReportListService_1.BIReportListService();
        this.BIReportListService.getAll().subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                var myResult = myResponse.Result;
                if (Tools_1.AppTool.IsNullOrEmpty(_this.mySearchText)) {
                    _this.ItemsSource = myResult;
                }
                else {
                    myResult.forEach(function (item) {
                        if (!Tools_1.AppTool.IsNullOrEmpty(item.Name) && item.Name.toUpperCase().indexOf(_this.mySearchText.toUpperCase()) > -1
                            ||
                                !Tools_1.AppTool.IsNullOrEmpty(item.Name) && item.Name.toUpperCase().indexOf(_this.mySearchText.toUpperCase()) > -1) {
                            _this.ItemsSource.push(item);
                        }
                    });
                }
            }
        });
    };
    BIReportComponent.prototype.NewBIReportButtonClicked = function () {
        var _this = this;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Width = 1200;
        logWindow.Height = 820;
        var windowArgs = {};
        windowArgs.IsBIReportWorkspace = true;
        logWindow.WindowArgs = windowArgs;
        logWindow.Title = "Query Builder";
        logWindow.Show('./CommonModules/CommonOthers/Components/LoadSampleData/DWQueryBuilderComponent');
        logWindow.ComponentLoaded.subscribe(function (s) {
            logWindow.WindowClosed.subscribe(function (d) {
                if (s != null && d != "cancel") {
                    SessionLocator_1.SessionLocator.DynamicLoader.Load("./InfrastructureModules/InfrastructureBIReport/Components/Workspaces/BIReportPreviewComponent", _this.CurrentSession.SessionLocation.viewContainerRef)
                        .then(function (cmpRef) {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ DWQueryId: s.QID, ObjectTableName: 'BIReport', EntityId: null });
                    });
                }
            });
        });
    };
    BIReportComponent.prototype.OnNewBIReportWindowClosed = function (arg) {
        if (arg != 'cancel') {
            this.LoadData();
        }
    };
    BIReportComponent.prototype.EditBIReportClicked = function (report) {
        var _this = this;
        this.entityResourceService.getEntityResourceByTableName("BIReport", 0).subscribe(function (response) {
            if (!Tools_1.AppTool.IsNullOrEmpty(report.Id)) {
                SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', _this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(function (cmpRef) {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: report.Id, ObjectTableName: 'BIReport' });
                    cmpRef.instance.BackCompleted.subscribe(function (bk) {
                    });
                });
            }
        });
    };
    BIReportComponent.prototype.ViewBIReportClicked = function (report) {
        SessionLocator_1.SessionLocator.DynamicLoader.Load("./InfrastructureModules/InfrastructureBIReport/Components/Workspaces/BIReportPreviewComponent", this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ DWQueryId: report.DWQueryId, ObjectTableName: 'BIReport', EntityList: report, EntityId: report.Id });
        });
    };
    BIReportComponent.prototype.ExportToExcelClicked = function (report) {
        var windowArgs = {};
        windowArgs.queryId = report.DWQueryId;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 500;
        logitudeWindow.Height = 200;
        logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.ExportingDataToExcel");
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./Infrastructure/Components/ExportBI2ExcelControl/ExportBI2ExcelControl');
    };
    BIReportComponent.prototype.SearchTextChanged = function (text) {
        this.mySearchText = text;
        this.LoadData();
    };
    BIReportComponent = __decorate([
        core_1.Component({
            moduleId: './Report/Components/Workspaces/',
            templateUrl: 'BIReportComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService])
    ], BIReportComponent);
    return BIReportComponent;
}());
exports.BIReportComponent = BIReportComponent;
//# sourceMappingURL=BIReportComponent.js.map