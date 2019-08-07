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
var ShipmentDomainService_1 = require("../../../../Shipment/Services/ShipmentDomainService");
var ServiceHelper_1 = require("../../../../Infrastructure/Utilities/ServiceHelper");
var DownloadPackagesFileComponent = /** @class */ (function () {
    function DownloadPackagesFileComponent() {
        this.IsResourcesReady = false;
        this.IsDownloadInProgress = true;
        this.IsDownloadingSuccess = false;
        this.IsDownloadingError = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    DownloadPackagesFileComponent.prototype.Download = function (entityId, entityNumber) {
        this.EntityId = entityId;
        this.EntityNumber = entityNumber;
        this.IsResourcesReady = true;
        this.Start();
    };
    DownloadPackagesFileComponent.prototype.Start = function () {
        var _this = this;
        this.IsDownloadInProgress = true;
        this.IsDownloadingSuccess = false;
        this.IsDownloadingError = false;
        var myDomainService = new ShipmentDomainService_1.ShipmentDomainService();
        myDomainService.DownloadShipmentPackages(this.EntityNumber, this.EntityId).subscribe(function (myResponse) {
            _this.IsDownloadInProgress = false;
            if (myResponse.HasError) {
                _this.IsDownloadingError = true;
            }
            else {
                _this.FileName = myResponse.Result;
                _this.IsDownloadingSuccess = true;
            }
        });
    };
    DownloadPackagesFileComponent.prototype.RetryClicked = function () {
        this.Start();
    };
    DownloadPackagesFileComponent.prototype.DownloadClicked = function () {
        var tempDate = new Date();
        var MyDate = tempDate.getDate() + "-" + (tempDate.getMonth() + 1) + "-" + tempDate.getFullYear();
        var url = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + "WebPages/DawnLoadExcelPage.aspx?fileName=" + this.FileName + "&tempId=" + ServiceHelper_1.ServiceHelper.GetLDocumentDownloadToken() + "&qname=" + this.FileName;
        {
            window.open(url);
        }
        this.CurrentSession.CloseCurrentWindow();
    };
    DownloadPackagesFileComponent.prototype.CloseButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    DownloadPackagesFileComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './DownloadPackagesFileComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], DownloadPackagesFileComponent);
    return DownloadPackagesFileComponent;
}());
exports.DownloadPackagesFileComponent = DownloadPackagesFileComponent;
//# sourceMappingURL=DownloadPackagesFileComponent.js.map