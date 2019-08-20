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
var ConfirmWindow_1 = require("../../Controls/Windows/ConfirmWindow");
var SessionLocator_1 = require("../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../Infrastructure/Services/EntityResourceService");
var ReportsTemplatePMService_1 = require("../../Common/Services/StandardPMs/ReportsTemplatePMService");
var ReportsTemplatesVersionListExtendedService_1 = require("../../Common/Services/ExtendedLists/ReportsTemplatesVersionListExtendedService");
var ReportsTemplatesVersionPMExtendedService_1 = require("../../Common/Services/ExtendedPMs/ReportsTemplatesVersionPMExtendedService");
var LogitudeWindow_1 = require("../../Controls/Windows/LogitudeWindow");
var ReportsTemplateRestoreComponent = /** @class */ (function () {
    function ReportsTemplateRestoreComponent() {
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this.ReportsTemplatesVersionLists = [];
        this.RestoreButtonLable = "Restore";
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsVisibile = false;
        this.reportsTemplatePMService = new ReportsTemplatePMService_1.ReportsTemplatePMService();
        this.reportsTemplatesVersionListExtendedService = new ReportsTemplatesVersionListExtendedService_1.ReportsTemplatesVersionListExtendedService();
        this.reportsTemplatesVersionPMExtendedService = new ReportsTemplatesVersionPMExtendedService_1.ReportsTemplatesVersionPMExtendedService();
    }
    ReportsTemplateRestoreComponent.prototype.ngOnInit = function () {
    };
    ReportsTemplateRestoreComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("ReportsTemplatesVersion", 0).subscribe(function (response) {
            _this.IsVisibile = true;
            if (args) {
                _this.DataViewModel = args.DataViewModel;
                _this.ReportsTemplatePM = args.ReportsTemplatePM;
                if (_this.ReportsTemplatePM) {
                    _this.LoadReportsTemplatesVersionLists();
                }
            }
        });
    };
    ReportsTemplateRestoreComponent.prototype.RestoreButtonClicked = function (item) {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Show("Please notice that this will restore this version and set as the current one");
        confirmWindow.WindowClosed.subscribe(function (event) {
            if (confirmWindow.Yes) {
                _this.Restore(item);
            }
        });
    };
    ReportsTemplateRestoreComponent.prototype.Restore = function (item) {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        this.reportsTemplatesVersionPMExtendedService.GetRestoreReportsTemplatesVersion(item.Id, SessionLocator_1.SessionLocator.LoggedUserId).subscribe(function (res) {
            var pmResponse = res;
            _this.CurrentSession.StopBusyIndicator();
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    if (_this.DataViewModel) {
                        _this.DataViewModel.IsChange = true;
                        _this.DataViewModel.Refresh();
                    }
                    _this.CurrentSession.CloseCurrentWindow();
                }
            }
        });
    };
    ReportsTemplateRestoreComponent.prototype.PreviewButtonClicked = function (item) {
        if (item) {
            var windowArgs = {};
            windowArgs.DataViewModel = this;
            windowArgs.ProcessType = "ReportPreview";
            windowArgs.ReportTemplateId = item.ReportDocumentId;
            windowArgs.Tenant = item.Tenant;
            var widthwindow = window.innerWidth;
            var heighthwindow = window.innerHeight;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = widthwindow - 100;
            logWindow.Height = heighthwindow - 100;
            logWindow.Title = "Preview Report Template Version";
            logWindow.IsShowCloseButton = true;
            logWindow.WindowArgs = windowArgs;
            window.designerClosed = false;
            logWindow.Show("./Infrastructure/Components/StimulsoftDesigner/StimulsoftDesigner");
            logWindow.WindowClosed.subscribe(function ($event) {
            });
        }
    };
    ReportsTemplateRestoreComponent.prototype.LoadReportsTemplatesVersionLists = function () {
        var _this = this;
        this.ReportsTemplatesVersionLists = [];
        this.CurrentSession.StartBusyIndicatorLoading();
        this.reportsTemplatesVersionListExtendedService.getReportsTemplatesVersionListsByReportTemplateId(this.ReportsTemplatePM.Id).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var result = pmResponse.Result;
                if (result) {
                    result.sort(function (a, b) { return (a.Version === b.Version) ? 0 : (a.Version < b.Version) ? 1 : -1; }).forEach(function (item) {
                        _this.ReportsTemplatesVersionLists.push(new ReportsTemplateRestoreItem(item));
                    });
                    if (_this.ReportsTemplatesVersionLists[0]) {
                        _this.ReportsTemplatesVersionLists[0].IsCurrentVersion = true;
                        _this.ReportsTemplatesVersionLists[0].VisibleRestoredViewButton = true;
                    }
                }
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    ReportsTemplateRestoreComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    //MouseEvent
    ReportsTemplateRestoreComponent.prototype.OnmMouseOver = function (reportsTemplatesVersionItem) {
        this.ReportsTemplatesVersionLists.forEach(function (item) {
            if (!item.IsCurrentVersion) {
                item.VisibleRestoredViewButton = false;
            }
        });
        reportsTemplatesVersionItem.VisibleRestoredViewButton = true;
    };
    ReportsTemplateRestoreComponent.prototype.OnmMouseleave = function (item) {
        this.ReportsTemplatesVersionLists.forEach(function (item) {
            item.VisibleRestoredViewButton = false;
            if (item.IsCurrentVersion) {
                item.VisibleRestoredViewButton = true;
            }
        });
    };
    ReportsTemplateRestoreComponent = __decorate([
        core_1.Component({
            moduleId: './Report/Components/',
            selector: 'ReportsTemplateRestoreComponent',
            templateUrl: 'ReportsTemplateRestoreComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ReportsTemplateRestoreComponent);
    return ReportsTemplateRestoreComponent;
}());
exports.ReportsTemplateRestoreComponent = ReportsTemplateRestoreComponent;
var ReportsTemplateRestoreItem = /** @class */ (function () {
    function ReportsTemplateRestoreItem(reportsTemplatesVersionList) {
        this.VisibleRestoredViewButton = false;
        this.IsCurrentVersion = false;
        this.EntityList = reportsTemplatesVersionList;
        this.Id = reportsTemplatesVersionList.Id;
        this.Version = reportsTemplatesVersionList.Version;
        this.IsRestored = reportsTemplatesVersionList.IsRestored;
        this.UpdateDate = reportsTemplatesVersionList.UpdateDate;
        this.UpdateByUserName = reportsTemplatesVersionList.UpdateByUserName;
        this.ReportDocumentId = reportsTemplatesVersionList.ReportDocumentId;
        this.Tenant = reportsTemplatesVersionList.Tenant;
    }
    return ReportsTemplateRestoreItem;
}());
exports.ReportsTemplateRestoreItem = ReportsTemplateRestoreItem;
//# sourceMappingURL=ReportsTemplateRestoreComponent.js.map