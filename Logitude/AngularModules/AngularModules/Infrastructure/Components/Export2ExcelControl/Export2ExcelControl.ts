
import {Component, Output, EventEmitter} from '@angular/core';
import {TextCodeTranslationPipe} from '../../../Controls/Pipes/TextCodeTranslationPipe';

import {WebFreightDomainService} from '../../../Infrastructure/Services/WebFreightDomainService';
//import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {AmitalGatewayUtil} from '../../../Infrastructure/Utilities/AmitalGatewayUtil';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { HttpClient } from '@angular/common/http';
import {LogboxShipmentExportExcelService} from '../../../Shipment/Services/Others/LogboxShipmentExportExcelService';
import { LogitudeGridExportToExcelExtendedPMService } from 'Common/Services/ExtendedPMs/LogitudeGridExportToExcelExtendedPMService';

@Component({
    

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
  constructor(private http: HttpClient) {

    }
    ObjectTableName: string;
    FileName: string;
    tenant: number;
    queryName: string;
    queryCode: string;
    queryId: string;
    userid: string;
    QueryType: string;
    ExportExcelArgs: any;
    SetWindowArgs(args: any) {

        this.QueryType = args.QueryType ? args.QueryType : "";
        this.queryName = args.QueryName;
        this.ExportExcelArgs = args.ExportExcelArgs;

       if (this.QueryType == "LogBox") {
            var logboxShipmentExportExcelService: LogboxShipmentExportExcelService = new LogboxShipmentExportExcelService();
            logboxShipmentExportExcelService.GetQueryToExcelData(this.ExportExcelArgs).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) this.CompleteExcelData(myResponse.Result);
                else this.CompleteExcelData("Faild");
            });

        }
        else if (this.QueryType == "LogitudeGrid") {
            var logitudeGridExportToExcelExtendedPMService: LogitudeGridExportToExcelExtendedPMService = new LogitudeGridExportToExcelExtendedPMService();
            logitudeGridExportToExcelExtendedPMService.GetQueryToExcelData(this.ExportExcelArgs).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) this.CompleteExcelData(myResponse.Result);
                else this.CompleteExcelData("Faild");
            });

        }

        else {
            var myService: WebFreightDomainService = new WebFreightDomainService();
            this.ObjectTableName = args.currentObjectTable;
            this.tenant = args.tenant;
            this.queryName = TextCodeTranslator.Translate(args.query.NameTextCodeCode);
            this.queryId = args.query.Id;
            this.queryCode = args.query.UniqueCode;

            this.userid = args.userid;
            this.Filters = args.Filters;
            myService.getExcelData(this.Filters, this.queryCode, args.tenant, args.userid, args.currentObjectTable).subscribe((myResult: any) => {
                this.CompleteExcelData(myResult.body);
            });
        }
         
    }

    CompleteExcelData(myResult:any) {
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

        if (this.QueryType != "LogBox") {

            var myService: WebFreightDomainService = new WebFreightDomainService();
            myService.getExcelData(this.Filters, this.queryCode, this.tenant, this.userid, this.ObjectTableName).subscribe((myResult: any) => {
                this.CompleteExcelData(myResult);

            });
        } else if (this.QueryType == "LogBox") {
            var logboxShipmentExportExcelService: LogboxShipmentExportExcelService = new LogboxShipmentExportExcelService();
            logboxShipmentExportExcelService.GetQueryToExcelData(this.ExportExcelArgs).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) this.CompleteExcelData(myResponse.Result);
                else this.CompleteExcelData("Faild");
            });

        }
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
