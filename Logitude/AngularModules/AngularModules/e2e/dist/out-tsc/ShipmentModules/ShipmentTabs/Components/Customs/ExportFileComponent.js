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
var DownloadManager_1 = require("../../../../Infrastructure/Utilities/DownloadManager");
var ShipmentDomainService_1 = require("../../../../Shipment/Services/ShipmentDomainService");
var ExportFileComponent = /** @class */ (function () {
    function ExportFileComponent() {
        this.IsResourcesReady = false;
        this.IsExportingInProgress = true;
        this.IsExportingSuccess = false;
        this.IsExportingError = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    ExportFileComponent.prototype.Export = function (entityId) {
        this.EntityId = entityId;
        this.IsResourcesReady = true;
        this.StartExporting();
    };
    ExportFileComponent.prototype.StartExporting = function () {
        var _this = this;
        this.IsExportingInProgress = true;
        this.IsExportingSuccess = false;
        this.IsExportingError = false;
        var service = new ShipmentDomainService_1.ShipmentDomainService();
        service.SendToCustoms_AES(this.EntityId).subscribe(function (myResponse) {
            _this.IsExportingInProgress = false;
            if (myResponse.HasError) {
                _this.IsExportingError = true;
            }
            else {
                _this.FileName = myResponse.Result;
                _this.IsExportingSuccess = true;
            }
        });
    };
    ExportFileComponent.prototype.RetryClicked = function () {
        this.StartExporting();
    };
    ExportFileComponent.prototype.DownloadClicked = function () {
        DownloadManager_1.DownloadManager.DownloadTransferHeaderFile(this.FileName);
    };
    ExportFileComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    ExportFileComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ExportFileComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], ExportFileComponent);
    return ExportFileComponent;
}());
exports.ExportFileComponent = ExportFileComponent;
//# sourceMappingURL=ExportFileComponent.js.map