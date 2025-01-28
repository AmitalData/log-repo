import { EventEmitter, OnDestroy, Output } from "@angular/core";
import { Component } from "@angular/core";
import { ReportExecutionLogPM } from "Common/EntityPMs/ReportExecutionLogPM";
import { ReportPM } from "Common/EntityPMs/ReportPM";
import { ReportService } from "Common/Services/ExtendedLists/ReportService";
import { ReportExecutionLogPMService } from "Common/Services/StandardPMs/ReportExecutionLogPMService";
import { ReportPMService } from "Common/Services/StandardPMs/ReportPMService";
import { AppTool } from "Infrastructure/Tools";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { BehaviorSubject, interval, Subscription } from "rxjs";
import { takeWhile } from "rxjs/operators";
import { QueryFilterItem } from "./Filters/QueryFilterItem";
import { ReportFliter } from "./Filters/ReportFliter";
import { ServiceResponse } from "Infrastructure/DataContracts/ServiceResponse";
import { ReportsTemplateListExtendedService } from "Common/Services/ExtendedLists/ReportsTemplateListExtendedService";
import { ReportsPreviewComponent } from "./ReportsPreviewComponent";
import { parseString } from 'xml2js';
import { ObjectsLocator } from "Infrastructure/Locators/ObjectsLocator";


@Component({
    selector: 'ReportMenuComponent',
    templateUrl: './ReportMenuComponent.html',
    inputs: ['CurrentReportId','IsReportPanelVisible'],
})

export class ReportMenuComponent implements OnDestroy {
    private CurrentSession = SessionLocator.SelectedSession;
    @Output() PinnedChanged = new EventEmitter<boolean>();

    public RelatedReport: ReportExecutionLogPM[];
    SelectedReport: ReportExecutionLogPM;
    private relatedReportSubject = new BehaviorSubject<ReportExecutionLogPM[]>([]);
    RelatedReport$ = this.relatedReportSubject.asObservable();
    showExceptionMessage = false
    isReportPanelVisible: boolean = false;

    currentReportId: string = "";
    private isPinned: boolean = false;
    public LayoutDirection: string = 'ltr';

    constructor() {
        this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;

        this.LoadReports()

    }
    ngOnDestroy(): void {
        this.subscription?.unsubscribe();
       

    }
       

    
    _reportService: ReportService = new ReportService();
    private subscription: Subscription | null = null;
    
    get CurrentReportId() { return this.currentReportId; }
    set CurrentReportId(newValue: string) {
        if(!AppTool.IsNullOrEmpty(newValue)){
            this.LoadReports();
        }
        if (this.currentReportId != newValue) {
            this.currentReportId = newValue;
        }
    }
    get IsReportPanelVisible() { return this.isReportPanelVisible; }
    set IsReportPanelVisible(newValue: boolean) {
        
        if (this.isReportPanelVisible != newValue) {
            this.isReportPanelVisible = newValue;
        }
        if (newValue) 
            this.CurrentReportId = "";
    }
    public LoadReports() {
        this._reportService = new ReportService();
        this._reportService.GetReportByTenantAndUserToMenu(SessionLocator.LoggedUserPM?.Id).subscribe((response: ServiceResponse) => {


            if (!AppTool.IsNullOrEmpty(response)) {

                this.RelatedReport = [];
                var relatedDocs: ReportExecutionLogPM[];
                relatedDocs = response.Result;
                this.RelatedReport = relatedDocs?.sort((a, b) => new Date(b.CreateDate).getTime() - new Date(a.CreateDate).getTime());
                this.relatedReportSubject.next(relatedDocs);
                this.StartCheckingStatus();

            }
        });
    }
    togglePin() {
        this.isPinned = !this.isPinned;
        this.PinnedChanged.emit(this.isPinned);
    }
    StartCheckingStatus() {
        this.subscription = interval(5000)
            .pipe(takeWhile(() => SessionLocator.HomeComponent.IsReportPanelVisible))
            .subscribe(() => this.CheckStatus());
    }
    public CheckStatus() {

        const reportIds = this.RelatedReport?.filter(report => report.StatusCode === 'P' || report.StatusCode === 'W')?.map(report => report.Id).join(',');
        this._reportService.CheckReportsStatus(reportIds).subscribe(statusResponse => {
            if (statusResponse && !statusResponse.HasError) {
                statusResponse?.Result?.forEach((status: any) => {
                    const reportIndex = this.RelatedReport.findIndex(r => r.Id === status.Id);
                    if (reportIndex !== -1) {
                        this.RelatedReport[reportIndex] = status;
                    }
                });
                this.relatedReportSubject.next(this.RelatedReport);
            }
        });

    }
    DeleteReport(relatedRep: ReportExecutionLogPM) {
        this.CurrentSession.StartBusyIndicator("Deleting....");

        this.RelatedReport = this.RelatedReport.filter(report => report.Id !== relatedRep?.Id);
        this.relatedReportSubject.next(this.RelatedReport);
        this._reportService.DeleteFromMenu(relatedRep.Id).subscribe((res: any) => {

            this.CurrentSession.StopBusyIndicator();
        });
    }
    ReportExecutionLogPMService: ReportExecutionLogPMService = new ReportExecutionLogPMService();

    CancelReport(relatedRep: ReportExecutionLogPM) {

        this.CurrentSession.StartBusyIndicator("Canceling...");
        this.ReportExecutionLogPMService.Cancel(relatedRep.Id).subscribe((res: any) => {
            if (!res.HasError) {
                this.LoadReports();
            }
            this.CurrentSession.StopBusyIndicator();
        });
    }
    reportPMService: ReportPMService = new ReportPMService();
    reportsTemplateListExtendedService = new ReportsTemplateListExtendedService();
    ReportTemplates = null;
    ViewReport(relatedRep: ReportExecutionLogPM) {

        this.reportPMService.get(relatedRep.ReportId).subscribe((response: ServiceResponse) => {
            if (response.Result) {
                if (!response.HasError) {
                    this.reportsTemplateListExtendedService.getReportsTemplateListsByReportId(relatedRep.ReportId).subscribe((myResponse: ServiceResponse) => {
                        if (myResponse.HasError) return;

                        this.ReportTemplates = myResponse.Result;
                        this.LoadReportsPreviewComponent(relatedRep, response.Result);

                        this.CurrentSession.StopBusyIndicator();
                    });
                }
            }



        })

    }
    private PageChild_PRREP: any = null;
    ReportsPreviewComponent:ReportsPreviewComponent
    
    LoadReportsPreviewComponent(relatedRep: ReportExecutionLogPM, report: ReportPM) {
        this.CurrentSession.StartBusyIndicator("Preview...");
        SessionLocator.DynamicLoader.Load('./Report/Components/ReportsPreviewComponent', SessionLocator.SelectedSession.SessionLocation.viewContainerRef)
            .then((cmpRef: any) => {
             
                cmpRef.instance.ComponentRef = cmpRef;
                this.PageChild_PRREP = cmpRef.instance;
                this.SetReportDetails(relatedRep, report);
                this.CurrentSession.StopBusyIndicator();
            });
    }

    SetReportDetails(relatedRep: ReportExecutionLogPM, report: ReportPM) {

        let reportFilterItems = this.ConvertXmlToObject(relatedRep.ReportFilterXML)
        this.PageChild_PRREP.SetReportFilterItems(reportFilterItems?.QueryFilterItemLists, false);

        this.PageChild_PRREP.SetReportTemplate(relatedRep.ReportTemplateId);
        this.PageChild_PRREP.ReportsPreview(null, report, this.ReportTemplates);
        this.PageChild_PRREP.GenerateReportViewWorkerRole(reportFilterItems);

    }


    SetReportFilterItems(xml: any): Array<QueryFilterItem> {
        let reportFilterItems: Array<QueryFilterItem> = [];


        let filterItems = xml;
        if (!Array.isArray(filterItems)) {
            filterItems = [filterItems];
        }

        reportFilterItems = filterItems.map((item: any) => ({
            FieldName: item.FieldName,
            FieldValue: this.convertStringToType(item.FieldValue?._),
            FieldValue2: this.convertStringToType(item.FieldValue2?._),
            FieldValue3: this.convertStringToType(item.FieldValue3?._),
            Operator: item.Operator,
            IsCustom: item.IsCustom === 'true',
            DisplayInList: item.DisplayInList === 'true',
            IsCustomField: item.IsCustomField === 'true',
            FieldDataType: item.FieldDataType,
            IsListFilter: item.IsListFilter === 'true',
            IsAnalyticsMetadatas: item.IsAnalyticsMetadatas === 'true'
        }));

        return reportFilterItems;
    }
    convertStringToType(value: string): any {
        if (value === 'true') {
            return true;
        } else if (value === 'false') {
            return false;
        } else {
            return value;
        }
    }
    ConvertXmlToObject(xml: string): ReportFliter {
        let resultObject: any = {};
        parseString(xml, { explicitArray: false }, (err: Error, result: any) => {
            if (err) {
                console.error('Error parsing XML:', err);
                return;
            }

            const reportFilter = result.ReportFliter;
            resultObject.CurrentCurrencyCodeType = reportFilter.CurrentCurrencyCodeType;
            resultObject.DateType = reportFilter.DateType;
            resultObject.IncludeOperationalyClosed = reportFilter.IncludeOperationalyClosed === 'true';
            resultObject.ReportCode = reportFilter.ReportCode;
            resultObject.FilterControlName = reportFilter.FilterControlName;
            resultObject.ReportDocumentId = reportFilter.ReportDocumentId;
            resultObject.CustomerId = reportFilter.CustomerId;
            resultObject.QuoteCustomerTypeCode = reportFilter.QuoteCustomerTypeCode;
            resultObject.FieldDataType = reportFilter.FieldDataType;
            resultObject.Tenant = parseInt(reportFilter.Tenant, 10);
            resultObject.QueryFilterItemLists = this.SetReportFilterItems(reportFilter.QueryFilterItemLists.QueryFilterItem);
            resultObject.NumberOfPage = parseInt(reportFilter.NumberOfPage, 10);
            resultObject.ProcessType = "ReportsRunUsingWR";
            resultObject.ReportKey = reportFilter.ReportKey;
            resultObject.ReportName = reportFilter.ReportName;
            resultObject.InvoiceType = reportFilter.InvoiceType;
            resultObject.DefaultTemplateId = reportFilter.DefaultTemplateId;
            resultObject.DefaultTemplateVsersion = parseInt(reportFilter.DefaultTemplateVsersion, 10);
            resultObject.ReportsRunUsingWR = reportFilter.ReportsRunUsingWR === 'true';
            resultObject.UserId = reportFilter.UserId;
            resultObject.ReportId = reportFilter.ReportId;
            resultObject.NumberOfRequests = parseInt(reportFilter.NumberOfRequests, 10);
            resultObject.Level = reportFilter.Level;
            resultObject.DisablePreview = reportFilter.DisablePreview === 'true';
        });
        return resultObject;
    }


    getPercentage(statusCode: string): number {
        switch (statusCode) {
            case 'P':
            case 'F':
                return 50;
            case 'D':
                return 100;
            case 'W':
                return 0;
            default:
                return 0;
        }
    }


  
}










