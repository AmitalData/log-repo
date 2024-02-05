
import { Component, Output, EventEmitter } from '@angular/core';
import { TextCodeTranslationPipe } from '../../../Controls/Pipes/TextCodeTranslationPipe';

import { WebFreightDomainService } from '../../../Infrastructure/Services/WebFreightDomainService';
//import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { AmitalGatewayUtil } from '../../../Infrastructure/Utilities/AmitalGatewayUtil';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';
import { ServiceResponse } from '../../DataContracts/ServiceResponse';
import { HttpClient } from '@angular/common/http';
import { LogboxShipmentExportExcelService } from '../../../Shipment/Services/Others/LogboxShipmentExportExcelService';
import { LogitudeGridExportToExcelExtendedPMService } from 'Common/Services/ExtendedPMs/LogitudeGridExportToExcelExtendedPMService';
import { interval, Observable, TimeInterval, timer } from 'rxjs';
import { takeUntil, timeInterval } from 'rxjs/operators';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { ReconcileExcelDataArgs, ReconciliationExtendedPMService } from 'Accounting/Services/ExtendedPMs/ReconciliationExtendedPMService';


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
    Type: string
    ExportExcelArgs: any;
    WebFreightDomainService: WebFreightDomainService;
    ReconcileExcelDataArgs: ReconcileExcelDataArgs;
    SetWindowArgs(args: any) {
        this.QueryType = args.QueryType ? args.QueryType : "";
        this.queryName = args.QueryName;
        this.ExportExcelArgs = args.ExportExcelArgs;
        this.ReconcileExcelDataArgs = args.ReconcileExcelDataArgs;
        this.Type = args.Type ? args.Type : "";

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
        else if (this.QueryType == "DraftReconciliation") {
            var reconciliationExtendedPMService: ReconciliationExtendedPMService = new ReconciliationExtendedPMService();
            reconciliationExtendedPMService.PostReconcileExcelData(this.ReconcileExcelDataArgs).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) this.CompleteExcelData(myResponse.Result);
                else this.CompleteExcelData("Faild");
            });

        }
        else if (this.QueryType == "DraftReconciliationExt") {
            var reconciliationExtendedPMService: ReconciliationExtendedPMService = new ReconciliationExtendedPMService();
            reconciliationExtendedPMService.PostReconcileExtExcelData(this.ReconcileExcelDataArgs).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) this.CompleteExcelData(myResponse.Result);
                else this.CompleteExcelData("Faild");
            });

        }
        else {
            this.WebFreightDomainService = new WebFreightDomainService();
            this.ObjectTableName = args.currentObjectTable;
            this.tenant = args.tenant;
            this.queryName = TextCodeTranslator.Translate(args.query.NameTextCodeCode);
            this.queryId = args.query.Id;
            this.queryCode = args.query.UniqueCode;

            this.userid = args.userid;
            this.Filters = args.Filters;
            this.WebFreightDomainService.getExcelData(this.Filters, this.queryCode, args.tenant, args.userid, args.currentObjectTable).subscribe((myResult: ExportResult) => {
                this.HandleExportResult(myResult);
            }, error => { this.OnError(error) });
        }

    }

    OnError(error) {

        this.btnRetryVisibile = true;
        this.busyExportingVisibile = false;
        this.btnSaveToFileVisibile = false;

        console.error(error);
    }

    HandleExportResult(myResult: ExportResult) {
        this.FileName = myResult.FileName;
        if (!myResult.IsWorkerRole) {
            this.btnRetryVisibile = false;
            this.busyExportingVisibile = false;
            this.btnSaveToFileVisibile = true;
        }
        else {
            this.StartExecutionLogCheckTimer(myResult.ExecutionLogId);
        }
    }

    initializeStartExecutionLogCheckTimer() {
        const source = interval(2000);
        const timer$ = timer(1200000); //complete after
        //return interval(2000).pipe(takeUntil(timer$));

        return source.pipe(takeUntil(timer$));

    }
    private StartExecutionLogCheckTimerSub: any = null;
    IsStartExecutionLogCheckTimer = false;
    IsSucceeded = false;
    StartExecutionLogCheckTimer(logId: string) {
        if (this.IsStartExecutionLogCheckTimer) {
            this.StartExecutionLogCheckTimerSub.unsubscribe();
        }

        this.IsStartExecutionLogCheckTimer = true;
        this.StartExecutionLogCheckTimerSub = this.initializeStartExecutionLogCheckTimer().subscribe(() => {

            if ((this.CurrentSession && this.CurrentSession.isDestroingSession)
                || !this.IsStartExecutionLogCheckTimer) {
                this.StopQueryLogCheckTimer();
                return;
            }

            if (this.IsStartExecutionLogCheckTimer) {

                this.WebFreightDomainService ?? new WebFreightDomainService();


                this.WebFreightDomainService.GetQueryExportExecutionLogStatus(logId).subscribe(
                    (res: ServiceResponse) => {
                        const pmResponse: ServiceResponse = res;
                        if (this.IsStartExecutionLogCheckTimer) {
                            if (pmResponse.HasError
                                || (pmResponse.Result && pmResponse.Result.ExceptionMessage)
                                || (pmResponse.Result && pmResponse.Result.StatusCode === "D")) {

                                this.IsSucceeded = true;
                                this.StopQueryLogCheckTimer();

                                this.CompleteExcelData(this.FileName);
                            }
                            if (!pmResponse.HasError) {
                                const result = pmResponse.Result;
                                if (result) {
                                    if (result.ExceptionMessage) {
                                        this.ShowRetryOption();
                                    }
                                }
                            }
                            else {
                                if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                                    this.ShowRetryOption();
                                }
                            }
                        }
                    },
                    error => { console.log(error) }
                );
            }
        },
            error => { console.log(error) },
            () => {
                if (!this.IsSucceeded)
                    this.LogCheckTimerCompletedUnsuccessfully();
            }
        );
    }

    private LogCheckTimerCompletedUnsuccessfully() {
        console.log("ExecutionLogCheckTimer Completed");
        this.StopQueryLogCheckTimer();
        this.ShowRetryOption();
    }

    private StopQueryLogCheckTimer() {
        this.StartExecutionLogCheckTimerSub?.unsubscribe();
        this.IsStartExecutionLogCheckTimer = false;

    }

    private ShowRetryOption() {
        this.btnRetryVisibile = true;
        this.busyExportingVisibile = false;
        this.btnSaveToFileVisibile = false;
    }

    CompleteExcelData(myResult: string) {
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


    SaveExcelFile(tenant: number, FileName: string, OTName: string, Type: string) {
        var tempDate = new Date();
        var MyDate = tempDate.getDate() + "-" + (tempDate.getMonth() + 1) + "-" + tempDate.getFullYear();
        var url = ServiceHelper.GetLogitudeURL() + "WebPages/DawnLoadExcelPage.aspx?fileName=" + FileName + "&tempId=" + ServiceHelper.GetLDocumentDownloadToken() + "&qname=" + this.queryName + "_" + MyDate + "&type=" + this.Type;
       // url += "&Type=SaveToMicrosoftExcel2007"; 
        //if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
        //    AmitalGatewayUtil.Instance.DeclarationMessaging.RaiseOpenNewBrowser(url);
        //} else
        {
            window.open(url);
        }

        this.CurrentSession.CloseCurrentWindow();
    }

    SaveBtnCLicked() {
        this.SaveExcelFile(this.tenant, this.FileName, this.ObjectTableName, this.Type);
    }
    RetryBtnClicked() {

        this.btnRetryVisibile = false;
        this.busyExportingVisibile = true;
        this.btnSaveToFileVisibile = false;

        if (this.QueryType != "LogBox") {

            this.WebFreightDomainService ?? new WebFreightDomainService();
            this.WebFreightDomainService.getExcelData(this.Filters, this.queryCode, this.tenant, this.userid, this.ObjectTableName).subscribe((myResult: ExportResult) => {
                this.HandleExportResult(myResult);

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

        this.StopQueryLogCheckTimer();
        /* I need to abort the process */
        //if (exportExcelService != null) {
        //    exportExcelService.CloseAsync();
        //    exportExcelService.Abort();
        //    exportExcelService.ExportQueryToExcelCompleted -= new EventHandler<ExportQueryToExcelCompletedEventArgs>(exportExcelService_ExportQueryToExcelCompleted);
        //}
        this.CurrentSession.CloseCurrentWindow();
    }



}

interface ExportResult {

    ExecutionLogId: string;
    FileName: string;
    IsWorkerRole: boolean;
}
