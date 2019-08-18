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
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var Export2ExcelControl = /** @class */ (function () {
    function Export2ExcelControl(http) {
        this.http = http;
        this.btnRetryVisibile = false;
        this.busyExportingVisibile = true;
        this.btnSaveToFileVisibile = false;
        this.RTL = ObjectsLocator_1.ObjectsLocator.GlobalSetting == undefined ? false : (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == 'rtl' ? true : false); //true;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        ServiceHelper_1.ServiceHelper.Http = http;
        //serviceArgs.http = http;
    }
    Export2ExcelControl.prototype.SetWindowArgs = function (args) {
        var _this = this;
        var myService = new WebFreightDomainService_1.WebFreightDomainService();
        this.ObjectTableName = args.currentObjectTable;
        this.tenant = args.tenant;
        this.queryName = TextCodeTranslator_1.TextCodeTranslator.Translate(args.query.NameTextCodeCode);
        this.queryId = args.query.Id;
        this.userid = args.userid;
        this.Filters = args.Filters;
        myService.getExcelData(this.Filters, this.queryId, args.tenant, args.userid, args.currentObjectTable).subscribe(function (myResult) {
            if (myResult == "Faild") {
                _this.btnRetryVisibile = true;
                _this.busyExportingVisibile = false;
                _this.btnSaveToFileVisibile = false;
            }
            else {
                _this.FileName = myResult;
                //var tempDate = new Date();
                //var MyDate = tempDate.getDate() + "-" + (tempDate.getMonth() + 1) + "-" + tempDate.getFullYear();
                //this.url = logitude_url + "WebPages/DawnLoadExcelPage.aspx?fileName=" + this.FileName + "&tempId=" + ServiceHelper.GetLDocumentDownloadToken() +  "&qname=" + this.queryName + "_" + MyDate;
                _this.btnRetryVisibile = false;
                _this.busyExportingVisibile = false;
                _this.btnSaveToFileVisibile = true;
            }
        });
    };
    Export2ExcelControl.prototype.SaveExcelFile = function (tenant, FileName, OTName) {
        var tempDate = new Date();
        var MyDate = tempDate.getDate() + "-" + (tempDate.getMonth() + 1) + "-" + tempDate.getFullYear();
        var url = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + "WebPages/DawnLoadExcelPage.aspx?fileName=" + FileName + "&tempId=" + ServiceHelper_1.ServiceHelper.GetLDocumentDownloadToken() + "&qname=" + this.queryName + "_" + MyDate;
        //if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
        //    AmitalGatewayUtil.Instance.DeclarationMessaging.RaiseOpenNewBrowser(url);
        //} else
        {
            window.open(url);
        }
        this.CurrentSession.CloseCurrentWindow();
    };
    Export2ExcelControl.prototype.SaveBtnCLicked = function () {
        this.SaveExcelFile(this.tenant, this.FileName, this.ObjectTableName);
    };
    Export2ExcelControl.prototype.RetryBtnClicked = function () {
        var _this = this;
        this.btnRetryVisibile = false;
        this.busyExportingVisibile = true;
        this.btnSaveToFileVisibile = false;
        var myService = new WebFreightDomainService_1.WebFreightDomainService();
        myService.getExcelData(this.Filters, this.queryId, this.tenant, this.userid, this.ObjectTableName).subscribe(function (myResult) {
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
    Export2ExcelControl.prototype.CancelButtonClicked = function () {
        /* I need to abort the process */
        //if (exportExcelService != null) {
        //    exportExcelService.CloseAsync();
        //    exportExcelService.Abort();
        //    exportExcelService.ExportQueryToExcelCompleted -= new EventHandler<ExportQueryToExcelCompletedEventArgs>(exportExcelService_ExportQueryToExcelCompleted);
        //}
        this.CurrentSession.CloseCurrentWindow();
    };
    Export2ExcelControl = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './Export2ExcelControl.html',
        }),
        __metadata("design:paramtypes", [http_1.Http])
    ], Export2ExcelControl);
    return Export2ExcelControl;
}());
exports.Export2ExcelControl = Export2ExcelControl;
//# sourceMappingURL=Export2ExcelControl.js.map