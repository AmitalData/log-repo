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
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var AccountingTransferHeaderPMService_1 = require("../../../Services/StandardPMs/AccountingTransferHeaderPMService");
var DownloadManager_1 = require("../../../../Infrastructure/Utilities/DownloadManager");
var ExportTransferComponent = /** @class */ (function () {
    function ExportTransferComponent() {
        this.TransferredCount = 0;
        this.TransferredEntityName = null;
        this.IsResourcesReady = false;
        this.IsExportingInProgress = true;
        this.IsExportingSuccess = false;
        this.IsExportingError = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.entityPMService = new AccountingTransferHeaderPMService_1.AccountingTransferHeaderPMService();
    }
    ExportTransferComponent.prototype.Export = function (entityPM) {
        this.EntityPM = entityPM;
        this.TransferredCount = entityPM.TransferLines.length;
        switch (this.EntityPM.AccountingTransferTypeCode) {
            case "ARIN": {
                this.TransferredEntityName = "A/R Invoice";
                break;
            }
            case "APIN": {
                this.TransferredEntityName = "A/P Invoice";
                break;
            }
            case "ARPA": {
                this.TransferredEntityName = "A/R Payment";
                break;
            }
            case "APPA": {
                this.TransferredEntityName = "A/P Payment";
                break;
            }
        }
        this.IsResourcesReady = true;
        this.StartExporting();
    };
    ExportTransferComponent.prototype.StartExporting = function () {
        var _this = this;
        this.IsExportingInProgress = true;
        this.IsExportingSuccess = false;
        this.IsExportingError = false;
        this.entityPMService.insert(this.EntityPM).subscribe(function (myResponse) {
            _this.IsExportingInProgress = false;
            if (myResponse.HasError) {
                _this.IsExportingError = true;
            }
            else {
                _this.IsExportingSuccess = true;
                _this.CurrentSession.FireEvent("TransferExportFirstTime");
            }
        });
    };
    ExportTransferComponent.prototype.RetryClicked = function () {
        this.StartExporting();
    };
    ExportTransferComponent.prototype.DownloadClicked = function () {
        DownloadManager_1.DownloadManager.DownloadTransferHeaderFile(this.EntityPM.FileName);
    };
    ExportTransferComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    ExportTransferComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ExportTransferComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ExportTransferComponent);
    return ExportTransferComponent;
}());
exports.ExportTransferComponent = ExportTransferComponent;
//# sourceMappingURL=ExportTransferComponent.js.map