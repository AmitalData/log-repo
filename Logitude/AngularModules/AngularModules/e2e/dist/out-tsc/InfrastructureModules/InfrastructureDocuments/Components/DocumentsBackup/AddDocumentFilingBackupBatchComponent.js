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
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var DocumentFilingBackupBatchPM_1 = require("../../../../Common/EntityPMs/DocumentFilingBackupBatchPM");
var DocumentFilingBackupBatchPMExtendedService_1 = require("../../../../Common/Services/ExtendedPMs/DocumentFilingBackupBatchPMExtendedService");
var DocumentFilingBackupBatchPMService_1 = require("../../../../Common/Services/StandardPMs/DocumentFilingBackupBatchPMService");
var Tools_1 = require("../../../../Infrastructure/Tools");
var AddDocumentFilingBackupBatchComponent = /** @class */ (function (_super) {
    __extends(AddDocumentFilingBackupBatchComponent, _super);
    function AddDocumentFilingBackupBatchComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.IsNewDocumentFilingBackupSetting = false;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.documentFilingBackupBatchPMExtendedService = new DocumentFilingBackupBatchPMExtendedService_1.DocumentFilingBackupBatchPMExtendedService();
        _this.SetRequired();
        return _this;
    }
    AddDocumentFilingBackupBatchComponent.prototype.ngOnInit = function () {
    };
    AddDocumentFilingBackupBatchComponent.prototype.SetRequired = function () {
        var isRequiredFromDate = !this.FromDatetime ? true : false;
        this.UIProperties.SetRequired("FromDatetime", "DocumentFilingBackupBatch", isRequiredFromDate);
        var isRequiredToDate = !this.ToDatetime ? true : false;
        this.UIProperties.SetRequired("ToDatetime", "DocumentFilingBackupBatch", isRequiredToDate);
    };
    Object.defineProperty(AddDocumentFilingBackupBatchComponent.prototype, "IncludeBackedUp", {
        get: function () {
            return this.includeBackedUp;
        },
        set: function (value) {
            if (this.includeBackedUp != value)
                this.includeBackedUp = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddDocumentFilingBackupBatchComponent.prototype, "FromDatetime", {
        get: function () {
            return this.fromDatetime;
        },
        set: function (value) {
            if (this.fromDatetime != value)
                this.fromDatetime = value;
            this.SetRequired();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(AddDocumentFilingBackupBatchComponent.prototype, "ToDatetime", {
        get: function () {
            return this.toDatetime;
        },
        set: function (value) {
            if (this.toDatetime != value)
                this.toDatetime = value;
            this.SetRequired();
        },
        enumerable: true,
        configurable: true
    });
    AddDocumentFilingBackupBatchComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AddDocumentFilingBackupBatchComponent.prototype.OkButtonClicked = function () {
        this.ValidationErrorsList = [];
        if (!this.FromDatetime || !this.ToDatetime) {
            if (!this.FromDatetime)
                this.ValidationErrorsList.push("From Date field is required");
            if (!this.ToDatetime)
                this.ValidationErrorsList.push("To Date field is required");
        }
        else if (this.FromDatetime > this.ToDatetime)
            this.ValidationErrorsList.push("From date must be smaller\equal to To date");
        else {
            if (Tools_1.DateTool.GetDaysBetweenDates(this.FromDatetime, this.ToDatetime) > 360) {
                this.ValidationErrorsList.push("The backup period should be less or equal to one year");
            }
        }
        if (this.ValidationErrorsList.length == 0) {
            var todayDateTime = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
            //this.FromDatetime = DateTool.GetDate(this.FromDatetime.getUTCFullYear(), this.FromDatetime.getUTCMonth(), this.FromDatetime.getUTCDate() , todayDateTime.getUTCHours(), todayDateTime.getUTCMinutes(), todayDateTime.getUTCSeconds());
            //this.ToDatetime = DateTool.GetDate(this.ToDatetime.getUTCFullYear(), this.ToDatetime.getUTCMonth(), this.ToDatetime.getUTCDate(), todayDateTime.getUTCHours(), todayDateTime.getUTCMinutes(), todayDateTime.getUTCSeconds());
            this.InsertDocumentFilingBackupBatchPMService();
        }
    };
    AddDocumentFilingBackupBatchComponent.prototype.InsertDocumentFilingBackupBatchPMService = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorSaving();
        var service = new DocumentFilingBackupBatchPMService_1.DocumentFilingBackupBatchPMService();
        var documentFilingBackupBatchPM = new DocumentFilingBackupBatchPM_1.DocumentFilingBackupBatchPM();
        documentFilingBackupBatchPM.ToDatetime = this.ToDatetime;
        documentFilingBackupBatchPM.FromDatetime = this.FromDatetime;
        documentFilingBackupBatchPM.IncludeBackedUp = this.IncludeBackedUp;
        documentFilingBackupBatchPM.Status = "Created";
        documentFilingBackupBatchPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
        documentFilingBackupBatchPM.TotalFailed = 0;
        documentFilingBackupBatchPM.TotalDocuments = 0;
        documentFilingBackupBatchPM.TotalSucceeded = 0;
        service.insert(documentFilingBackupBatchPM).subscribe(function (res) {
            _this.CurrentSession.StopBusyIndicator();
            if (!res.HasError) {
                _this.CurrentSession.CloseCurrentWindowEmit("OK");
            }
            else {
                if (res.ErrorsArray && res.ErrorsArray.length > 0) {
                    var messageWindow = new MessageWindow_1.MessageWindow();
                    messageWindow.Show(res.ErrorsArray[0]);
                }
            }
        });
    };
    AddDocumentFilingBackupBatchComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'AddDocumentFilingBackupBatchComponent',
            templateUrl: './AddDocumentFilingBackupBatchComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AddDocumentFilingBackupBatchComponent);
    return AddDocumentFilingBackupBatchComponent;
}(BaseComponent_1.BaseComponent));
exports.AddDocumentFilingBackupBatchComponent = AddDocumentFilingBackupBatchComponent;
//# sourceMappingURL=AddDocumentFilingBackupBatchComponent.js.map