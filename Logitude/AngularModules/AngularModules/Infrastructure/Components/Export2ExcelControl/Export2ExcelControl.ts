import {Component, Output, EventEmitter} from '@angular/core';
import {TextCodeTranslationPipe} from '../../../Controls/Pipes/TextCodeTranslationPipe';
import {Http} from '@angular/http';
import {WebFreightDomainService} from '../../../Infrastructure/Services/WebFreightDomainService';
//import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {AmitalGatewayUtil} from '../../../Infrastructure/Utilities/AmitalGatewayUtil';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    moduleId: module.id,

    templateUrl: './Export2ExcelControl.html',
    //pipes: [TextCodeTranslationPipe],
    //providers: [ServiceArgs]
})

export class Export2ExcelControl {
    btnRetryVisibile = false;
    busyExportingVisibile = true;
    btnSaveToFileVisibile = false;
    Filters: ApiQueryFilters;
    url: string;
    RTL: boolean = ObjectsLocator.GlobalSetting == undefined ? false : (ObjectsLocator.GlobalSetting.LayoutDirection == 'rtl' ? true : false);//true;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private http: Http) {
        ServiceHelper.Http = http;
        //serviceArgs.http = http;
    }
    ObjectTableName: string;
    FileName: string;
    tenant: number;
    queryName: string;
    queryId: string;
    userid: string;
    SetWindowArgs(args: any) {
        var myService: WebFreightDomainService = new WebFreightDomainService();
        this.ObjectTableName = args.currentObjectTable;
        this.tenant = args.tenant;
        this.queryName = TextCodeTranslator.Translate(args.query.NameTextCodeCode);
        this.queryId = args.query.Id;
        this.userid = args.userid;
        this.Filters = args.Filters;
        myService.getExcelData(this.Filters, this.queryId, args.tenant, args.userid, args.currentObjectTable).subscribe(myResult => {
            if (myResult == "Faild") {
                this.btnRetryVisibile = true;
                this.busyExportingVisibile = false;
                this.btnSaveToFileVisibile = false;
            }
            else {
                this.FileName = myResult;
                //var tempDate = new Date();
                //var MyDate = tempDate.getDate() + "-" + (tempDate.getMonth() + 1) + "-" + tempDate.getFullYear();
                //this.url = logitude_url + "WebPages/DawnLoadExcelPage.aspx?fileName=" + this.FileName + "&tempId=" + ServiceHelper.GetLDocumentDownloadToken() +  "&qname=" + this.queryName + "_" + MyDate;

                this.btnRetryVisibile = false;
                this.busyExportingVisibile = false;
                this.btnSaveToFileVisibile = true;
            }

        });
    }

    SaveExcelFile(tenant: number, FileName: string, OTName: string) {
        var tempDate = new Date();
        var MyDate = tempDate.getDate() + "-" + (tempDate.getMonth() + 1) + "-" + tempDate.getFullYear();
        var url = ServiceHelper.GetLogitudeURL() + "WebPages/DawnLoadExcelPage.aspx?fileName=" + FileName + "&tempId=" + ServiceHelper.GetLDocumentDownloadToken() +  "&qname=" + this.queryName + "_" + MyDate;
        //if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
        //    AmitalGatewayUtil.Instance.DeclarationMessaging.RaiseOpenNewBrowser(url);
        //} else
        {
            window.open(url);
        }
        
        this.CurrentSession.CloseCurrentWindow();
    }

    SaveBtnCLicked() {
        this.SaveExcelFile(this.tenant, this.FileName, this.ObjectTableName);
    }
    RetryBtnClicked() {
        this.btnRetryVisibile = false;
        this.busyExportingVisibile = true;
        this.btnSaveToFileVisibile = false;
        var myService: WebFreightDomainService = new WebFreightDomainService();
        myService.getExcelData(this.Filters, this.queryId, this.tenant, this.userid, this.ObjectTableName).subscribe(myResult => {
            if (myResult == "Faild") {
                this.btnRetryVisibile = true;
                this.busyExportingVisibile = false;
                this.btnSaveToFileVisibile = false;
            }
            else {
                this.FileName = myResult;
                this.btnRetryVisibile = false;
                this.busyExportingVisibile = false;
                this.btnSaveToFileVisibile = true;
            }

        });
    }

    CancelButtonClicked() {
        /* I need to abort the process */
        //if (exportExcelService != null) {
        //    exportExcelService.CloseAsync();
        //    exportExcelService.Abort();
        //    exportExcelService.ExportQueryToExcelCompleted -= new EventHandler<ExportQueryToExcelCompletedEventArgs>(exportExcelService_ExportQueryToExcelCompleted);
        //}
        this.CurrentSession.CloseCurrentWindow();
    }

}
