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
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var core_1 = require("@angular/core");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var BatchTaskExecutionListService_1 = require("../../../Infrastructure/Services/StandardLists/BatchTaskExecutionListService");
var DocumentsFilingViewsExtService_1 = require("../../../Common/Services/ExtendedLists/DocumentsFilingViewsExtService");
var TaxReportExtendedPMService_1 = require("../../Services/ExtendedPMs/TaxReportExtendedPMService");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var DownloadManager_1 = require("../../../Infrastructure/Utilities/DownloadManager");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var TaxDeductionReportExtendedPMService_1 = require("../../Services/ExtendedPMs/TaxDeductionReportExtendedPMService");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var AccountingFlatFileDownloadComponent = /** @class */ (function (_super) {
    __extends(AccountingFlatFileDownloadComponent, _super);
    function AccountingFlatFileDownloadComponent() {
        var _this = _super.call(this) || this;
        _this.ObjectTableName = "TaxReport";
        _this.DataContext = _this;
        _this.ValidationErrorsList = [];
        _this.timerInterval = 1000;
        _this.Loading = false;
        _this.Success = false;
        _this.Failed = false;
        _this.LabelText = "";
        _this.isRTL = false;
        _this._DocumentsFilingViewsExtService = new DocumentsFilingViewsExtService_1.DocumentsFilingViewsExtService();
        _this._BatchTaskExecutionListService = new BatchTaskExecutionListService_1.BatchTaskExecutionListService();
        _this._TaxReportExtendedPMService = new TaxReportExtendedPMService_1.TaxReportExtendedPMService();
        _this.taxDeductionReportExtendedPMService = new TaxDeductionReportExtendedPMService_1.TaxDeductionReportExtendedPMService();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            _this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        return _this;
    }
    AccountingFlatFileDownloadComponent.prototype.ngOnDestroy = function () {
        if (this.timer) {
            clearInterval(this.timer);
        }
    };
    AccountingFlatFileDownloadComponent.prototype.SetWindowArgs = function (args) {
        if (args != null) {
            this.ChangeStatus();
            this.ObjectTableName = args.ObjectTableName;
            // Tax Report
            if (this.ObjectTableName == "TaxReport") {
                this.reportPM = args.EntityPM;
                // if the file does not need rebuild, show download button
                if (!this.reportPM.NeedsRebulid) {
                    //update status
                    this.ChangeStatus("inprogress");
                    //get documentid
                    this.GetDocument();
                }
            }
            else if (this.ObjectTableName == "TaxDeductionReport") {
                this.taxDeductionPM = args.EntityPM;
            }
            if (args.StartDirectly)
                this.RunService(false);
            if (args.TimerInterval)
                this.timerInterval = args.TimerInterval;
        }
    };
    AccountingFlatFileDownloadComponent.prototype.RunService = function (byButton) {
        var _this = this;
        if (byButton === void 0) { byButton = false; }
        this.ChangeStatus("creating");
        switch (this.ObjectTableName) {
            // Tax Report
            case "TaxReport":
                {
                    if (byButton || this.reportPM.NeedsRebulid) {
                        this._TaxReportExtendedPMService.DownloadPNC874FileInBatch(this.reportPM).subscribe(function (myResult) {
                            var mm = myResult;
                            var entity = mm.Result;
                            _this.btePM = entity;
                            _this.ChangeStatus("inprogress");
                            _this.timer = setInterval(function () {
                                _this.GetBTE();
                            }, _this.timerInterval);
                        });
                    }
                    else {
                        //update status
                        this.ChangeStatus("ready");
                        //get documentid
                        this.GetDocument();
                    }
                    break;
                }
            case "TaxDeductionReport":
                {
                    //if (byButton || this.reportPM.NeedsRebulid) {
                    //this.taxDeductionReportExtendedPMService.DownloadTaxDeduction856FileInBatch(this.taxDeductionPM).subscribe(myResult => {
                    //    var mm: ServiceResponse = myResult;
                    //    var entity = mm.Result;
                    //    this.btePM = entity;
                    //    this.ChangeStatus("inprogress");
                    //    this.timer = setInterval(() => {
                    //        this.GetBTE();
                    //    }, this.timerInterval);
                    //});
                    //} else {
                    //    //update status
                    //    this.ChangeStatus("ready");
                    //    //get documentid
                    //    this.GetDocument();
                    //}
                    break;
                }
            default:
                {
                    // ...
                    break;
                }
        }
    };
    AccountingFlatFileDownloadComponent.prototype.GetBTE = function () {
        var _this = this;
        this._BatchTaskExecutionListService.getSingle(this.btePM.Id).subscribe(function (myResult) {
            console.log("[_BatchTaskExecutionListService.getSingle]", myResult);
            var mm = myResult;
            if (!mm.HasError) {
                _this.bteList = mm.Result;
                if (_this.bteList.StatusCode == "D") // D- Done
                 {
                    // ...
                    //get documentid
                    _this.GetDocument();
                    //stop timer
                    if (_this.timer) {
                        clearInterval(_this.timer);
                    }
                }
                else if (_this.bteList.StatusCode == "F") // F- Failed
                 {
                    //stop timer
                    if (_this.timer) {
                        clearInterval(_this.timer);
                    }
                    //update status
                    _this.ChangeStatus("failed");
                }
            }
            else {
            }
        });
    };
    AccountingFlatFileDownloadComponent.prototype.GetDocument = function () {
        var _this = this;
        var objectTable = window.ObjectTables.filter(function (d) { return d.Name === _this.ObjectTableName; })[0];
        if (this.ObjectTableName == "TaxReport") {
            this._DocumentsFilingViewsExtService.GetLastDocumentsFilingPM(this.reportPM.Id, objectTable.Id).subscribe(function (myResult) {
                console.log("[GetLastDocumentsFilingPM]", myResult);
                var mm = myResult;
                if (!mm.HasError) {
                    _this.docFilingPM = mm.Result;
                    if (!_this.reportPM.NeedsRebulid)
                        _this.ChangeStatus("ready");
                    else
                        _this.ChangeStatus("done");
                }
                else {
                    console.error("GetLastDocumentsFilingPM ERROR", mm);
                }
            });
        }
        else if (this.ObjectTableName == "TaxDeductionReport") {
            this._DocumentsFilingViewsExtService.GetLastDocumentsFilingPM(this.taxDeductionPM.Id, objectTable.Id).subscribe(function (myResult) {
                console.log("[GetLastDocumentsFilingPM]", myResult);
                var mm = myResult;
                if (!mm.HasError) {
                    _this.docFilingPM = mm.Result;
                    _this.ChangeStatus("ready");
                }
                else {
                    console.error("GetLastDocumentsFilingPM ERROR", mm);
                }
            });
        }
    };
    //#region Buttons
    AccountingFlatFileDownloadComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    AccountingFlatFileDownloadComponent.prototype.OkButtonClicked = function () {
    };
    AccountingFlatFileDownloadComponent.prototype.DownloadButtonClicked = function () {
        DownloadManager_1.DownloadManager.DownloadPage(null, this.docFilingPM.SecurityId);
    };
    AccountingFlatFileDownloadComponent.prototype.ShowError = function () {
        var msg = this.bteList.ErrorLog;
        var msgbox = new MessageWindow_1.MessageWindow();
        // msgbox.Width = 500;
        // msgbox.Height = 400;
        msgbox.RTL = this.isRTL;
        msgbox.Show(msg);
    };
    //#endregion
    AccountingFlatFileDownloadComponent.prototype.GetLabelColor = function () {
        if (this.Success)
            return "green";
        else if (this.Failed)
            return "red";
        else if (this.Loading)
            return "blue";
        else
            return "black";
    };
    AccountingFlatFileDownloadComponent.prototype.ChangeStatus = function (status) {
        if (status === void 0) { status = ""; }
        switch (status) {
            case "ready": { // file didn't needs rebuild
                this.Loading = false;
                this.Success = true;
                this.Failed = false;
                this.LabelText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.FileIsReady");
                //this.LabelText = "Creating file ...";
                break;
            }
            case "creating": {
                this.Loading = true;
                this.Success = false;
                this.Failed = false;
                this.LabelText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.CreatingFile");
                //this.LabelText = "Creating file ...";
                break;
            }
            case "inprogress": {
                this.Loading = true;
                this.Success = false;
                this.Failed = false;
                this.LabelText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.PleaseWaitCreatingFile");
                //this.LabelText = "Please wait while creating file ...";
                break;
            }
            case "done": {
                this.Loading = false;
                this.Success = true;
                this.Failed = false;
                this.LabelText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.FileCreated");
                //this.LabelText = "File created";
                break;
            }
            case "failed": {
                this.Loading = false;
                this.Success = false;
                this.Failed = true;
                this.LabelText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.ErrorwhileCreating");
                this.ShowError();
                //this.LabelText = "Error while creating!";
                break;
            }
            default: {
                this.Loading = false;
                this.Success = false;
                this.Failed = false;
                this.LabelText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.clicktoStartCreatingFile");
                //this.LabelText = "Please click create to start creating file";
                break;
            }
        }
    };
    AccountingFlatFileDownloadComponent = __decorate([
        core_1.Component({
            selector: 'AccountingFlatFileDownloadComponent',
            moduleId: module.id,
            templateUrl: './AccountingFlatFileDownloadComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], AccountingFlatFileDownloadComponent);
    return AccountingFlatFileDownloadComponent;
}(BaseComponent_1.BaseComponent));
exports.AccountingFlatFileDownloadComponent = AccountingFlatFileDownloadComponent;
//# sourceMappingURL=AccountingFlatFileDownloadComponent.js.map