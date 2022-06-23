import { Component, Output, EventEmitter } from '@angular/core';
import { TextCodeTranslationPipe } from '../../../Controls/Pipes/TextCodeTranslationPipe';
import { WebFreightDomainService } from '../../../Infrastructure/Services/WebFreightDomainService';
import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { ServiceHelper } from '../../../Infrastructure/Utilities/ServiceHelper';
import { AmitalGatewayUtil } from '../../../Infrastructure/Utilities/AmitalGatewayUtil';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { BIReportXMLData } from '../../../Infrastructure/Services/InfrastructureDomainService';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { MessageWindow } from '../../../Controls/Windows/MessageWindow';
import { HttpClient } from '@angular/common/http';
import { interval } from 'rxjs';
import { timeInterval } from 'rxjs/operators';

@Component({
    
    templateUrl: './ExportBI2ExcelControl.html',
})

export class ExportBI2ExcelControl {
    btnRetryVisibile = false;
    busyExportingVisibile = true;
    btnSaveToFileVisibile = false;
    Filters: ApiQueryFilters;
    url: string;
    RTL: boolean = ObjectsLocator.GlobalSetting == undefined ? false : (ObjectsLocator.GlobalSetting.LayoutDirection == 'rtl' ? true : false);//true;
    private CurrentSession = SessionLocator.SelectedSession;
    private WebFreightDomainService: WebFreightDomainService;

  constructor(private http: HttpClient) {
        this.WebFreightDomainService = new WebFreightDomainService();        
    }
    ObjectTableName: string;
    FileName: string;
    tenant: number;
    queryName: string;
    queryId: string;
    queryCode: string;
    reportId: string;
    userid: string;
    IncludeTotals: boolean;
    BIReportXMLData: BIReportXMLData = null;
    ExportDataType: string;
    SetWindowArgs(args: any) {
        this.queryId = args.queryId;
        this.queryCode = args.queryCode;
        
        this.reportId = args.reportId;
        this.queryName = args.reportName;
        this.BIReportXMLData = args.BIReportXMLData;
        this.ExportDataType = args.BIReportXMLData ? args.BIReportXMLData.ExportDataType:"";

        this.IncludeTotals = args.IncludeTotals;
        this.BIReportXMLData.UserId = SessionInfo.LoggedUserId;
        this.StartBuildStimulReportViaWorkerRole();
    }

    BIReportsExecutionLogId: string;
    StartBuildStimulReportViaWorkerRole() {

        this.StartBusyIndicator("Generating...");
        this.StartTimerChangeBusyIndicatorMessageAfter50Sec();

        if (this.WebFreightDomainService == null) {
            this.WebFreightDomainService = new WebFreightDomainService();
        }
        this.WebFreightDomainService.GetExportBIReportToExcel(this.BIReportXMLData).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.FileName = myResponse.Result.BIReportKey;
                this.StartCheckBIReportBliudViaWorkerRoleTimer(myResponse.Result.BIReportsExecutionLogId);
            } else {
                this.StopBusyIndicator();
                if (myResponse.HasError && myResponse.ErrorsArray && myResponse.ErrorsArray.length > 0) {
                    this.CancelButtonClicked();
                    var messageWindow = new MessageWindow();
                    messageWindow.Show(myResponse.ErrorsArray[0]);
                }
            }
        });
    }

    //BI Report Timer
    initializeStartCheckBIReportBliudViaWorkerRoleTimer() {
        return interval(2000).pipe(timeInterval());
    }

    IsStartTimerWaitingFirstStimulReportBuildRunning: boolean = false;
    initializeStartTimerWaitingFirstStimulReportBuild() {
        return interval(50000).pipe(timeInterval());
    }
    private StartTimerWaitingFirstStimulReportBuildsub: any = null;
    StartTimerWaitingFirststimulReportBuild() {
        if (this.IsStartTimerWaitingFirstStimulReportBuildRunning) {
            this.StartTimerWaitingFirstStimulReportBuildsub.unsubscribe();
        }
        this.IsStartTimerWaitingFirstStimulReportBuildRunning = true;
        this.StartTimerWaitingFirstStimulReportBuildsub = this.initializeStartTimerWaitingFirstStimulReportBuild().subscribe((res:any) => {
            if (this.CurrentSession && this.CurrentSession.isDestroingSession) {
                this.StartTimerWaitingFirstStimulReportBuildsub.unsubscribe();
                this.IsStartTimerWaitingFirstStimulReportBuildRunning = false;
                return;
            }
            if (this.IsStartTimerWaitingFirstStimulReportBuildRunning) {
                this.StartBuildStimulReportViaWorkerRole();
                this.StartTimerWaitingFirstStimulReportBuildsub.unsubscribe();
                this.IsStartTimerWaitingFirstStimulReportBuildRunning = false;
            }
        });
    }

    private StartCheckBIReportBliudViaWorkerRoleTimersub: any = null;
    IsStartCheckBIReportBliudViaWorkerRoleTimer: boolean = false;
    StartCheckBIReportBliudViaWorkerRoleTimer(bIReportLogId:string) {
        if (this.IsStartCheckBIReportBliudViaWorkerRoleTimer) {
            this.StartCheckBIReportBliudViaWorkerRoleTimersub.unsubscribe();
        }

        this.IsStartCheckBIReportBliudViaWorkerRoleTimer = true;
        this.StartCheckBIReportBliudViaWorkerRoleTimersub = this.initializeStartCheckBIReportBliudViaWorkerRoleTimer().subscribe(respose => {

            if ((this.CurrentSession && this.CurrentSession.isDestroingSession) || !this.IsStartCheckBIReportBliudViaWorkerRoleTimer) {
                this.StartCheckBIReportBliudViaWorkerRoleTimersub.unsubscribe();
                this.IsStartCheckBIReportBliudViaWorkerRoleTimer = false;
                return;
            }

            if (this.IsStartCheckBIReportBliudViaWorkerRoleTimer) {
                if (this.WebFreightDomainService == null) {
                    this.WebFreightDomainService = new WebFreightDomainService();
                }

                this.WebFreightDomainService.GetBIReportLogStatus(bIReportLogId).subscribe((res: ServiceResponse) => {
                    var pmResponse: ServiceResponse = res;
                    if (this.IsStartCheckBIReportBliudViaWorkerRoleTimer) {
                        if (pmResponse.HasError || (pmResponse.Result && pmResponse.Result.ExceptionMessage) || (pmResponse.Result && pmResponse.Result.StatusCode == "D")) {
                            this.StartCheckBIReportBliudViaWorkerRoleTimersub.unsubscribe();
                            this.IsStartCheckBIReportBliudViaWorkerRoleTimer = false;
                            this.StopBusyIndicator();
                            this.btnRetryVisibile = false;
                            this.busyExportingVisibile = false;
                            this.btnSaveToFileVisibile = true;
                        }
                        if (!pmResponse.HasError) {
                            var result = pmResponse.Result;
                            if (result) {
                                if (result.ExceptionMessage) {
                                    this.CancelButtonClicked();

                                    var messageWindow = new MessageWindow();
                                    messageWindow.Show(result.ExceptionMessage);
                                }
                            }
                        }
                        else {
                            if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                                var messageWindow = new MessageWindow();
                                messageWindow.Show(pmResponse.ErrorsArray[0]);
                            }
                        }
                    }
                });
            }
        });
    }

    //Wait Result Stimul Timer
    IsStartTimerChangeBusyIndicatorMessageAfter50SecsRunning: boolean = false;
  initializeStartTimerChangeBusyIndicatorMessageAfter50Sec() {
    return interval(50000).pipe(timeInterval());
    }
    private StartTimerChangeBusyIndicatorMessageAfter50Secsub: any = null;
    StartTimerChangeBusyIndicatorMessageAfter50Sec() {

        if (this.IsStartTimerChangeBusyIndicatorMessageAfter50SecsRunning) {
            this.StartTimerChangeBusyIndicatorMessageAfter50Secsub.unsubscribe();
        }

        this.IsStartTimerChangeBusyIndicatorMessageAfter50SecsRunning = true;
        this.StartTimerChangeBusyIndicatorMessageAfter50Secsub = this.initializeStartTimerChangeBusyIndicatorMessageAfter50Sec().subscribe((res:any) => {
            if (this.CurrentSession && this.CurrentSession.isDestroingSession) {
                this.StartTimerChangeBusyIndicatorMessageAfter50Secsub.unsubscribe();
                this.IsStartTimerChangeBusyIndicatorMessageAfter50SecsRunning = false;
                return;
            }

            if (this.IsStartTimerChangeBusyIndicatorMessageAfter50SecsRunning) {
                this.StartBusyIndicator("Report generating is taking longer than expected. Please wait", 400);
                this.StartTimerChangeBusyIndicatorMessageAfter50Secsub.unsubscribe();
                this.IsStartTimerChangeBusyIndicatorMessageAfter50SecsRunning = false;
            }
        });
    }
    ShowBusyIndicator: boolean = false;
    BusyIndicatorText: string = "";
    WidthBusyIndicator: number;
    StartBusyIndicator(message: string = "Generating...", width: number = 200) {

        this.ShowBusyIndicator = true;
        this.BusyIndicatorText = message;
        this.WidthBusyIndicator = width;

    }

    StopBusyIndicator() {
        if (this.IsStartTimerWaitingFirstStimulReportBuildRunning) {
            this.StartTimerWaitingFirstStimulReportBuildsub.unsubscribe();
            this.IsStartTimerWaitingFirstStimulReportBuildRunning = false;
        }
        if (this.IsStartTimerChangeBusyIndicatorMessageAfter50SecsRunning) {
            this.StartTimerChangeBusyIndicatorMessageAfter50Secsub.unsubscribe();
            this.IsStartTimerChangeBusyIndicatorMessageAfter50SecsRunning = false;
        }

        if (this.IsStartCheckBIReportBliudViaWorkerRoleTimer) {
            this.StartCheckBIReportBliudViaWorkerRoleTimersub.unsubscribe();
            this.IsStartCheckBIReportBliudViaWorkerRoleTimer = false;
        }
        this.ShowBusyIndicator = false;
    }

    SaveExcelFile(tenant: number, FileName: string, OTName: string) {


        var tempDate = new Date();
        var MyDate = tempDate.getDate() + "-" + (tempDate.getMonth() + 1) + "-" + tempDate.getFullYear();
        var url = ServiceHelper.GetLogitudeURL() + "WebPages/DawnloadBIReportPage.aspx?fileName=" + FileName + "&tempId=" + ServiceHelper.GetLDocumentDownloadToken() + "&qname=" + this.queryName + "_" + MyDate + "&fileType=" + this.ExportDataType  ; //+ "&bireport=" + "bireport";
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
        myService.GetExportBIReportToExcel(this.BIReportXMLData).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {

                if (myResponse.Result) {
                    this.FileName = myResponse.Result.BIReportKey;
                    this.btnRetryVisibile = false;
                    this.busyExportingVisibile = false;
                    this.btnSaveToFileVisibile = true;
                }
            } else {
                this.btnRetryVisibile = true;
                this.busyExportingVisibile = false;
                this.btnSaveToFileVisibile = false;
            }
        });
    }
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
