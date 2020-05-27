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
var http_1 = require("@angular/http");
var WebFreightDomainService_1 = require("../../../../Infrastructure/Services/WebFreightDomainService");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var ServiceHelper_1 = require("../../../../Infrastructure/Utilities/ServiceHelper");
var DownloadAllFilesComponent = /** @class */ (function () {
    function DownloadAllFilesComponent(http) {
        this.http = http;
        this.btnRetryVisibile = false;
        this.busyExportingVisibile = true;
        this.btnSaveToFileVisibile = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        ServiceHelper_1.ServiceHelper.Http = http;
        //serviceArgs.http = http;
    }
    DownloadAllFilesComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        var myService = new WebFreightDomainService_1.WebFreightDomainService();
        this.ObjectTableId = args.ObjectTableId;
        this.tenant = SessionLocator_1.SessionLocator.Tenant;
        this.ShipmentId = args.ShipmentId;
        myService.DownLoadAllFilesForShipments(this.ShipmentId, this.ObjectTableId, this.tenant).subscribe(function (myResult) {
            if (myResult == "Faild") {
                _this.btnRetryVisibile = true;
                _this.busyExportingVisibile = false;
                _this.btnSaveToFileVisibile = false;
            }
            else {
                _this.FileName = myResult;
                _this.btnRetryVisibile = false;
                _this.busyExportingVisibile = false;
                _this.btnSaveToFileVisibile = true;
            }
        });
    };
    DownloadAllFilesComponent.prototype.SaveExcelFile = function (tenant, fileName) {
        var token = ServiceHelper_1.ServiceHelper.GetLDocumentDownloadToken();
        var url = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + "WebPages/DownloadFileName.aspx?id=" + fileName + "&tempId=" + token;
        window.open(url);
        this.CurrentSession.CloseCurrentWindow();
    };
    DownloadAllFilesComponent.prototype.SaveBtnCLicked = function () {
        this.SaveExcelFile(this.tenant, this.FileName);
    };
    DownloadAllFilesComponent.prototype.RetryBtnClicked = function () {
        var _this = this;
        this.btnRetryVisibile = false;
        this.busyExportingVisibile = true;
        this.btnSaveToFileVisibile = false;
        var myService = new WebFreightDomainService_1.WebFreightDomainService();
        myService.DownLoadAllFilesForShipments(this.ShipmentId, this.ObjectTableId, this.tenant).subscribe(function (myResult) {
            if (myResult == "Faild") {
                _this.btnRetryVisibile = true;
                _this.busyExportingVisibile = false;
                _this.btnSaveToFileVisibile = false;
            }
            else {
                _this.FileName = myResult;
                _this.btnRetryVisibile = false;
                _this.busyExportingVisibile = false;
                _this.btnSaveToFileVisibile = true;
            }
        });
    };
    DownloadAllFilesComponent.prototype.CancelButtonClicked = function () {
        /* I need to abort the process */
        //if (exportExcelService != null) {
        //    exportExcelService.CloseAsync();
        //    exportExcelService.Abort();
        //    exportExcelService.ExportQueryToExcelCompleted -= new EventHandler<ExportQueryToExcelCompletedEventArgs>(exportExcelService_ExportQueryToExcelCompleted);
        //}
        this.CurrentSession.CloseCurrentWindow();
    };
    DownloadAllFilesComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './DownloadAllFilesComponent.html',
        }),
        __metadata("design:paramtypes", [http_1.Http])
    ], DownloadAllFilesComponent);
    return DownloadAllFilesComponent;
}());
exports.DownloadAllFilesComponent = DownloadAllFilesComponent;
//# sourceMappingURL=DownloadAllFilesComponent.js.map