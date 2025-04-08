import { EventEmitter, HostListener, OnDestroy, Output } from "@angular/core";
import { Component } from "@angular/core";
import { ReportPM } from "Common/EntityPMs/ReportPM";
import { ReportService } from "Common/Services/ExtendedLists/ReportService";
import { ReportExecutionLogPMService } from "Common/Services/StandardPMs/ReportExecutionLogPMService";
import { ReportPMService } from "Common/Services/StandardPMs/ReportPMService";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";
import { BehaviorSubject, interval, Subscription } from "rxjs";
import { QueryFilterItem } from "./Filters/QueryFilterItem";
import { ReportFliter } from "./Filters/ReportFliter";
import { ServiceResponse } from "Infrastructure/DataContracts/ServiceResponse";
import { ReportsTemplateListExtendedService } from "Common/Services/ExtendedLists/ReportsTemplateListExtendedService";
import { ReportsPreviewComponent } from "./ReportsPreviewComponent";
import { parseString } from 'xml2js';
import { ObjectsLocator } from "Infrastructure/Locators/ObjectsLocator";
import { MessageWindow } from "Controls/Windows/MessageWindow";
import { TextCodeTranslator } from "Infrastructure/Utilities/TextCodeTranslator";
import { AppTool } from "Infrastructure/Tools";


@Component({
    selector: 'ReportMenuComponent',
    templateUrl: './ReportMenuComponent.html',
    inputs: ['CurrentReportId', 'IsReportPanelVisible', 'IsPinned','CurrentSelectedTab'],
})

export class ReportMenuComponent implements OnDestroy {
    private CurrentSession = SessionLocator.SelectedSession;
    @Output() PinnedChanged = new EventEmitter<boolean>();
    @Output() NumberDoneReports = new EventEmitter<number>();
    private numberDoneReports = 0;

    public RelatedReport: ReportMenuClass[];
    SelectedReport: ReportMenuClass;
    private relatedReportSubject = new BehaviorSubject<ReportMenuClass[]>([]);
    RelatedReport$ = this.relatedReportSubject.asObservable();
    showExceptionMessage = false
    isReportPanelVisible: boolean = false;
    public selectedTab: string = ''; 
    public currentSelectedTab: number = 0; 

    currentReportId: string = "";
    public isPinned: boolean = false;
    public LayoutDirection: string = 'ltr';
    public groupedReports: { [key: string]: any[] } = {}; 
    public MenuTypeNames: { [key in MenuTypes]: string } = {
        [MenuTypes.ReportExecutionLog]: TextCodeTranslator.Translate("General.MH.Reports"),
        [MenuTypes.BatchTaskExecution]: TextCodeTranslator.Translate("Accounting.General.O.TaxReport"),
    };
    constructor(private reportService: ReportService) {
        this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
      
        this.groupReportsByType();


    }
    groupReportsByType() {
        this.reportService.relatedReportSubject.subscribe(reportList => {
            this.groupedReports = reportList.reduce((groups: { [key: string]: ReportMenuClass[] }, report: ReportMenuClass) => {
                const type = report.ItemType;
                if (!groups[type]) {
                    groups[type] = [];
                }
                groups[type].push(report);
                return groups; 
            }, {});
            this.selectedTab =  (!AppTool.IsNullOrEmpty(this.selectedTab) && AppTool.IsNullOrEmpty(this.CurrentReportId)) ? this.selectedTab: Object.keys(this.groupedReports)[this.CurrentSelectedTab];

        });
    }
    selectTab(tab: string) {
        this.currentSelectedTab = null;
        this.selectedTab = tab;
    }
    ngOnDestroy(): void {
        this.subscription?.unsubscribe();

    }


    _reportService: ReportService = new ReportService();
    private subscription: Subscription | null = null;

    get CurrentReportId() { return this.currentReportId; }
    set CurrentReportId(newValue: string) {

        if (this.currentReportId != newValue) {
            this.currentReportId = newValue;
        }
    }
    get CurrentSelectedTab() { return this.currentSelectedTab; }
    set CurrentSelectedTab(newValue: number) {

        if (this.currentSelectedTab != newValue) {
            this.currentSelectedTab = newValue;
        }
    }
    get IsPinned() { return this.isPinned; }
    set IsPinned(newValue: boolean) {
     

        if (this.isPinned != newValue) {
            this.isPinned = newValue;
        }
    }
    get IsReportPanelVisible() { return this.isReportPanelVisible; }
    set IsReportPanelVisible(newValue: boolean) {

        if (this.isReportPanelVisible != newValue) {
            this.isReportPanelVisible = newValue;
        }
       
    }

    togglePin() {
        this.isPinned = !this.isPinned;
        this.PinnedChanged.emit(this.isPinned);
    }

    DeleteReport(relatedRep: ReportMenuClass) {
        this.CurrentSession.StartBusyIndicator("Deleting....");


        this._reportService.DeleteFromMenu(relatedRep.Id,relatedRep.ItemType).subscribe((res: any) => {
            if (!res.HasError) {
                this.reportService.LoadReports();
                SessionLocator.HomeComponent.IsReportPanelVisible = true;

            }
            this.CurrentSession.StopBusyIndicator();
        });
    }
    ReportExecutionLogPMService: ReportExecutionLogPMService = new ReportExecutionLogPMService();

    CancelReport(relatedRep: ReportMenuClass) {

        this.CurrentSession.StartBusyIndicator("Canceling...");
        this.ReportExecutionLogPMService.Cancel(relatedRep.Id).subscribe((res: any) => {
            if (!res.HasError) {
                this.reportService.LoadReports();
                SessionLocator.HomeComponent.IsReportPanelVisible = true;
            }
            this.CurrentSession.StopBusyIndicator();
        });
    }
    reportPMService: ReportPMService = new ReportPMService();
    reportsTemplateListExtendedService = new ReportsTemplateListExtendedService();
    ReportTemplates = null;
    ViewReport(relatedRep: ReportMenuClass) {
        switch (relatedRep.ItemType) {
            case MenuTypes.ReportExecutionLog:
                this.ViewReportExecutionLog(relatedRep);
                break;
            case MenuTypes.BatchTaskExecution:
                this.ViewBatchTaskExecution(relatedRep);
                break;
            default:
                this.ShowNoSupportWindow();
                break;
        }
        

    }
    ShowNoSupportWindow() {
        var msg = new MessageWindow();
        msg.Width = 450;
        msg.Show(" This item is not supported yet");
    }
    ViewReportExecutionLog(relatedRep: ReportMenuClass) {
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

    ViewBatchTaskExecution(relatedRep: ReportMenuClass) {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent',
            this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                const parser = new DOMParser();
                const xmlDoc = parser.parseFromString(relatedRep.ReportFilterXML, "text/xml");
                const reportId = xmlDoc.getElementsByTagName("ReportId")[0]?.textContent;

                cmpRef.instance.Run({ EntityId: reportId, ObjectTableName: "TaxReport" });
                
            });
    }
    private PageChild_PRREP: any = null;
    ReportsPreviewComponent: ReportsPreviewComponent

    LoadReportsPreviewComponent(relatedRep: ReportMenuClass, report: ReportPM) {
        this.CurrentSession.StartBusyIndicator("Preview...");
        SessionLocator.DynamicLoader.Load('./Report/Components/ReportsPreviewComponent', SessionLocator.SelectedSession.SessionLocation.viewContainerRef)
            .then((cmpRef: any) => {

                cmpRef.instance.ComponentRef = cmpRef;
                this.PageChild_PRREP = cmpRef.instance;
                this.SetReportDetails(relatedRep, report);
                this.CurrentSession.StopBusyIndicator();
            });
    }

    SetReportDetails(relatedRep: ReportMenuClass, report: ReportPM) {

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

}


export class ReportMenuClass {
    public StatusCode: string;
    public ReportLocalName: string;
    public ReportName: string;
    public CreateDate: Date = new Date();
    public ExceptionMessage: string = '';
    public ReportId: string;
    public ReportFilterXML: string;
    public ReportTemplateId: string;
    public NotDisplayInMenu: boolean = false;
    public ItemType: number = 0
    public Id: string;
}

 export enum MenuTypes {
    ReportExecutionLog = 0,
    BatchTaskExecution = 1,
}














