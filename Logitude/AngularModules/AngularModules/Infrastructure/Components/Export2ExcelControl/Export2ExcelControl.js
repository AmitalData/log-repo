import { Component } from '@angular/core';
import { Http } from '@angular/http';
import { WebFreightDomainService } from '../../../Infrastructure/Services/WebFreightDomainService';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
export var Export2ExcelControl = (function () {
    function Export2ExcelControl(http) {
        this.http = http;
        this.btnRetryVisibile = false;
        this.busyExportingVisibile = true;
        this.btnSaveToFileVisibile = false;
        this.RTL = SessionLocator.GlobalSetting == undefined ? false : (SessionLocator.GlobalSetting.LayoutDirection == 'rtl' ? true : false); //true;
        ServiceHelper.Http = http;
        //serviceArgs.http = http;
    }
    Export2ExcelControl.prototype.SetWindowArgs = function (args) {
        var _this = this;
        var myService = new WebFreightDomainService();
        this.ObjectTableName = args.currentObjectTable;
        this.tenant = args.tenant;
        this.queryName = TextCodeTranslator.Translate(args.query.NameTextCodeCode);
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
                //this.url = logitude_url + "WebPages/DawnLoadExcelPage.aspx?fileName=" + this.FileName + "&token=" + ServiceHelper.GetLoggedUserToken() + "&tenant=" + this.tenant + "&qname=" + this.queryName + "_" + MyDate;
                _this.btnRetryVisibile = false;
                _this.busyExportingVisibile = false;
                _this.btnSaveToFileVisibile = true;
            }
        });
    };
    Export2ExcelControl.prototype.SaveExcelFile = function (tenant, FileName, OTName) {
        var tempDate = new Date();
        var MyDate = tempDate.getDate() + "-" + (tempDate.getMonth() + 1) + "-" + tempDate.getFullYear();
        var url = ServiceHelper.GetLogitudeURL() + "WebPages/DawnLoadExcelPage.aspx?fileName=" + FileName + "&token=" + ServiceHelper.GetLoggedUserToken() + "&tenant=" + tenant + "&qname=" + this.queryName + "_" + MyDate;
        //if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
        //    AmitalGatewayUtil.Instance.DeclarationMessaging.RaiseOpenNewBrowser(url);
        //} else
        {
            window.open(url);
        }
        SessionLocator.SelectedSession.CloseCurrentWindow();
    };
    Export2ExcelControl.prototype.SaveBtnCLicked = function () {
        this.SaveExcelFile(this.tenant, this.FileName, this.ObjectTableName);
    };
    Export2ExcelControl.prototype.RetryBtnClicked = function () {
        var _this = this;
        this.btnRetryVisibile = false;
        this.busyExportingVisibile = true;
        this.btnSaveToFileVisibile = false;
        var myService = new WebFreightDomainService();
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
        SessionLocator.SelectedSession.CloseCurrentWindow();
    };
    Export2ExcelControl.decorators = [
        { type: Component, args: [{
                    moduleId: module.id,
                    templateUrl: './Export2ExcelControl.html',
                },] },
    ];
    /** @nocollapse */
    Export2ExcelControl.ctorParameters = [
        { type: Http, },
    ];
    return Export2ExcelControl;
}());
//# sourceMappingURL=Export2ExcelControl.js.map