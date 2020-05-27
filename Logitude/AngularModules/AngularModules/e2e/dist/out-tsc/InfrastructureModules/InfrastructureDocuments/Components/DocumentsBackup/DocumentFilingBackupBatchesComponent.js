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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
require("rxjs/add/operator/map");
var core_1 = require("@angular/core");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var DocumentFilingBackupBatchPMExtendedService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentFilingBackupBatchPMExtendedService");
var DocumentFilingBackupBatchesComponent = /** @class */ (function (_super) {
    __extends(DocumentFilingBackupBatchesComponent, _super);
    function DocumentFilingBackupBatchesComponent() {
        var _this = _super.call(this) || this;
        _this.BatchObsList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.documentFilingBackupBatchPMExtendedService = new DocumentFilingBackupBatchPMExtendedService_1.DocumentFilingBackupBatchPMExtendedService();
        return _this;
    }
    DocumentFilingBackupBatchesComponent.prototype.ngOnInit = function () {
        this.LoadData();
    };
    Object.defineProperty(DocumentFilingBackupBatchesComponent.prototype, "SelectedItemBatch", {
        get: function () { return this.selectedItemBatch; },
        set: function (value) { if (this.selectedItemBatch != value)
            this.selectedItemBatch = value; },
        enumerable: true,
        configurable: true
    });
    DocumentFilingBackupBatchesComponent.prototype.LoadData = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.BatchObsList = [];
        this.documentFilingBackupBatchPMExtendedService.GetDocumentFilingBackupBatchPMs().subscribe(function (res) {
            if (!res.HasError) {
                var documentFilingBackupBatchPMs = res.Result;
                documentFilingBackupBatchPMs.forEach(function (item) {
                    _this.BatchObsList.push(new DocumentFilingBackupBatchDataViewModel(item));
                });
                _this.BatchObsList = _this.BatchObsList.sort(function (a, b) { return (a.CreateDateTime > b.CreateDateTime) ? -1 : ((a.CreateDateTime < b.CreateDateTime) ? 1 : 0); });
            }
            else {
                if (res.ErrorsArray && res.ErrorsArray.length > 0) {
                    var messageWindow = new MessageWindow_1.MessageWindow();
                    messageWindow.Show(res.ErrorsArray[0]);
                }
            }
            _this.CurrentSession.StopBusyIndicator();
        });
    };
    DocumentFilingBackupBatchesComponent.prototype.RefreshBatch = function () {
        this.LoadData();
    };
    DocumentFilingBackupBatchesComponent.prototype.AddBatch = function () {
        var _this = this;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.WindowArgs = { Parent: this };
        logitudeWindow.Title = "Add New Batch";
        logitudeWindow.Width = 500;
        logitudeWindow.Height = 300;
        logitudeWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentsBackup/AddDocumentFilingBackupBatchComponent');
        logitudeWindow.WindowClosed.subscribe(function (p) {
            if (p == "OK") {
                _this.LoadData();
            }
        });
    };
    DocumentFilingBackupBatchesComponent.prototype.SettingButtonClicked = function () {
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Documents Backup Setting";
        logWindow.Width = 500;
        logWindow.Height = 300;
        logWindow.Show('./InfrastructureModules/InfrastructureDocuments/Components/DocumentsBackup/DocumentFilingBackupSettingComponent');
    };
    DocumentFilingBackupBatchesComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    DocumentFilingBackupBatchesComponent.prototype.SaveButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    DocumentFilingBackupBatchesComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'DocumentFilingBackupBatchesComponent',
            templateUrl: './DocumentFilingBackupBatchesComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], DocumentFilingBackupBatchesComponent);
    return DocumentFilingBackupBatchesComponent;
}(BaseComponent_1.BaseComponent));
exports.DocumentFilingBackupBatchesComponent = DocumentFilingBackupBatchesComponent;
var DocumentFilingBackupBatchDataViewModel = /** @class */ (function () {
    function DocumentFilingBackupBatchDataViewModel(entityPM) {
        this.EntityPM = entityPM;
    }
    Object.defineProperty(DocumentFilingBackupBatchDataViewModel.prototype, "BatchNumber", {
        get: function () { return this.EntityPM.BatchNumber; },
        set: function (value) { if (this.EntityPM.BatchNumber != value)
            this.EntityPM.BatchNumber = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentFilingBackupBatchDataViewModel.prototype, "DoneDate", {
        get: function () { return this.EntityPM.DoneDate; },
        set: function (value) { if (this.EntityPM.DoneDate != value)
            this.EntityPM.DoneDate = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentFilingBackupBatchDataViewModel.prototype, "CreateDateTime", {
        get: function () { return this.EntityPM.CreateDateTime; },
        set: function (value) { if (this.EntityPM.CreateDateTime != value)
            this.EntityPM.CreateDateTime = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentFilingBackupBatchDataViewModel.prototype, "FromDatetime", {
        get: function () { return this.EntityPM.FromDatetime; },
        set: function (value) { if (this.EntityPM.FromDatetime != value)
            this.EntityPM.FromDatetime = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentFilingBackupBatchDataViewModel.prototype, "ToDatetime", {
        get: function () { return this.EntityPM.ToDatetime; },
        set: function (value) { if (this.EntityPM.ToDatetime != value)
            this.EntityPM.ToDatetime = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentFilingBackupBatchDataViewModel.prototype, "Status", {
        get: function () { return this.EntityPM.Status; },
        set: function (value) { if (this.EntityPM.Status != value)
            this.EntityPM.Status = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentFilingBackupBatchDataViewModel.prototype, "TotalFailed", {
        get: function () { return this.EntityPM.TotalFailed; },
        set: function (value) { if (this.EntityPM.TotalFailed != value)
            this.EntityPM.TotalFailed = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentFilingBackupBatchDataViewModel.prototype, "TotalDocuments", {
        get: function () { return this.EntityPM.TotalDocuments; },
        set: function (value) { if (this.EntityPM.TotalDocuments != value)
            this.EntityPM.TotalDocuments = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DocumentFilingBackupBatchDataViewModel.prototype, "TotalSucceeded", {
        get: function () { return this.EntityPM.TotalSucceeded; },
        set: function (value) { if (this.EntityPM.TotalSucceeded != value)
            this.EntityPM.TotalSucceeded = value; },
        enumerable: true,
        configurable: true
    });
    return DocumentFilingBackupBatchDataViewModel;
}());
exports.DocumentFilingBackupBatchDataViewModel = DocumentFilingBackupBatchDataViewModel;
//# sourceMappingURL=DocumentFilingBackupBatchesComponent.js.map