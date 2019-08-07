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
var WebFreightDomainService_1 = require("../../../Infrastructure/Services/WebFreightDomainService");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var ExportBI2ExcelControl = /** @class */ (function () {
    function ExportBI2ExcelControl(http) {
        this.http = http;
        this.btnRetryVisibile = false;
        this.busyExportingVisibile = true;
        this.btnSaveToFileVisibile = false;
        this.RTL = ObjectsLocator_1.ObjectsLocator.GlobalSetting == undefined ? false : (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == 'rtl' ? true : false); //true;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.BIReportXMLData = null;
        ServiceHelper_1.ServiceHelper.Http = http;
    }
    ExportBI2ExcelControl.prototype.SetWindowArgs = function (args) {
        var _this = this;
        var myService = new WebFreightDomainService_1.WebFreightDomainService();
        this.queryId = args.queryId;
        this.reportId = args.reportId;
        this.queryName = args.reportName;
        this.BIReportXMLData = args.BIReportXMLData;
        myService.GetExportBIReportToExcel(this.BIReportXMLData).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                if (myResponse.Result == "Faild") {
                    _this.btnRetryVisibile = true;
                    _this.busyExportingVisibile = false;
                    _this.btnSaveToFileVisibile = false;
                }
                else {
                    _this.FileName = myResponse.Result;
                    _this.btnRetryVisibile = false;
                    _this.busyExportingVisibile = false;
                    _this.btnSaveToFileVisibile = true;
                }
            }
        });
    };
    ExportBI2ExcelControl.prototype.SaveExcelFile = function (tenant, FileName, OTName) {
        var tempDate = new Date();
        var MyDate = tempDate.getDate() + "-" + (tempDate.getMonth() + 1) + "-" + tempDate.getFullYear();
        var url = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + "WebPages/DawnLoadExcelPage.aspx?fileName=" + FileName + "&tempId=" + ServiceHelper_1.ServiceHelper.GetLDocumentDownloadToken() + "&qname=" + this.queryName + "_" + MyDate + "&Type=SaveToMicrosoftExcel2007"; //+ "&bireport=" + "bireport";
        {
            window.open(url);
        }
        this.CurrentSession.CloseCurrentWindow();
    };
    ExportBI2ExcelControl.prototype.SaveBtnCLicked = function () {
        this.SaveExcelFile(this.tenant, this.FileName, this.ObjectTableName);
    };
    ExportBI2ExcelControl.prototype.RetryBtnClicked = function () {
        var _this = this;
        this.btnRetryVisibile = false;
        this.busyExportingVisibile = true;
        this.btnSaveToFileVisibile = false;
        var myService = new WebFreightDomainService_1.WebFreightDomainService();
        myService.GetExportBIReportToExcel(this.BIReportXMLData).subscribe(function (myResponse) {
            if (!myResponse.HasError) {
                if (myResponse.Result == "Faild") {
                    _this.btnRetryVisibile = true;
                    _this.busyExportingVisibile = false;
                    _this.btnSaveToFileVisibile = false;
                }
                else {
                    _this.FileName = myResponse.Result;
                    _this.btnRetryVisibile = false;
                    _this.busyExportingVisibile = false;
                    _this.btnSaveToFileVisibile = true;
                }
            }
        });
    };
    ExportBI2ExcelControl.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    ExportBI2ExcelControl = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ExportBI2ExcelControl.html',
        }),
        __metadata("design:paramtypes", [http_1.Http])
    ], ExportBI2ExcelControl);
    return ExportBI2ExcelControl;
}());
exports.ExportBI2ExcelControl = ExportBI2ExcelControl;
//# sourceMappingURL=ExportBI2ExcelControl.js.map