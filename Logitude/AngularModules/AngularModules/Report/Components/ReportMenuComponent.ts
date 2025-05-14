import { EventEmitter, HostListener, OnDestroy, Output } from "@angular/core";
import { Component } from "@angular/core";
import { ReportExecutionLogPM } from "Common/EntityPMs/ReportExecutionLogPM";
import { ReportPM } from "Common/EntityPMs/ReportPM";
import { ReportService } from "Common/Services/ExtendedLists/ReportService";
import { ReportExecutionLogPMService } from "Common/Services/StandardPMs/ReportExecutionLogPMService";
import { ReportPMService } from "Common/Services/StandardPMs/ReportPMService";
import { AppTool, DateTool } from "Infrastructure/Tools";
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
import { ReportList } from "Report/EntityLists/ReportList";
import { TextCodeTranslator } from "Infrastructure/Utilities/TextCodeTranslator";
import { HomeComponent } from "Infrastructure/Components/HomeComponent/HomeComponent";


@Component({
    selector: 'ReportMenuComponent',
    templateUrl: './ReportMenuComponent.html',
    inputs: ['CurrentReportId','IsReportPanelVisible','IsPinned'],
})

export class ReportMenuComponent implements OnDestroy {
    private CurrentSession = SessionLocator.SelectedSession;
    @Output() PinnedChanged = new EventEmitter<boolean>();
    @Output() NumberDoneReports = new EventEmitter<number>();
    private numberDoneReports = 0;

    public RelatedReport: ReportFilterItem[];
    SelectedReport: ReportFilterItem;
    private relatedReportSubject = new BehaviorSubject<ReportFilterItem[]>([]);
    RelatedReport$ = this.relatedReportSubject.asObservable();
    showExceptionMessage = false
    isReportPanelVisible: boolean = false;

    currentReportId: string = "";
    public isPinned: boolean = false;
    public LayoutDirection: string = 'ltr';

    constructor(private reportService: ReportService) {
        this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;

        
        this.reportService.relatedReportSubject.subscribe(reportList => {
            var updatedReportList: ReportFilterItem[] = reportList.map(report => {
               // const filterValues: ReportFliter = this.ConvertXmlToObject(report.ReportFilterXML);
                return {
                    reportExecutionLogPM: report,
                    filters:null
                    //  filterValues.QueryFilterItemLists
                    //     .filter((filter: QueryFilterItem) => filter.FieldValue !== null && filter.FieldValue !== undefined && filter.FieldValue !== '')
                    //     .map((filter: QueryFilterItem) => {
                    //         let fieldValue = filter.FieldValue;
                    //         if (filter.FieldDataType === 'Date' && fieldValue) {
                    //             fieldValue = DateTool.GetDateFormats(fieldValue).DateString.replace(/\//g, '-');
                    //         }
                    //         return `${filter.FieldName} :${fieldValue}`;
                    //     })
                    //     .join(', ')
                    //     .toString()
                };
            });
            this.relatedReportSubject.next(updatedReportList);
        });
        
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
    get IsPinned() { return this.isPinned; }
    set IsPinned(newValue: boolean) {``
        
        if (this.isPinned != newValue) {
            this.isPinned = newValue;
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
   
    togglePin() {
        this.isPinned = !this.isPinned;
        this.PinnedChanged.emit(this.isPinned);
    }
    
    DeleteReport(relatedRep: ReportFilterItem) {
        this.CurrentSession.StartBusyIndicator("Deleting....");

      
        this._reportService.DeleteFromMenu(relatedRep.reportExecutionLogPM.Id).subscribe((res: any) => {
            if (!res.HasError) {
                this.reportService.LoadReports();
                SessionLocator.HomeComponent.IsReportPanelVisible = true;

            }    
            this.CurrentSession.StopBusyIndicator();
        });
    }
    ReportExecutionLogPMService: ReportExecutionLogPMService = new ReportExecutionLogPMService();

    CancelReport(relatedRep: ReportFilterItem) {

        this.CurrentSession.StartBusyIndicator("Canceling...");
        this.ReportExecutionLogPMService.Cancel(relatedRep.reportExecutionLogPM.Id).subscribe((res: any) => {
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
    ViewReport(relatedRep: ReportFilterItem) {

        this.reportPMService.get(relatedRep.reportExecutionLogPM.ReportId).subscribe((response: ServiceResponse) => {
            if (response.Result) {
                if (!response.HasError) {
                    this.reportsTemplateListExtendedService.getReportsTemplateListsByReportId(relatedRep.reportExecutionLogPM.ReportId).subscribe((myResponse: ServiceResponse) => {
                        if (myResponse.HasError) return;

                        this.ReportTemplates = myResponse.Result;
                        this.LoadReportsPreviewComponent(relatedRep.reportExecutionLogPM, response.Result);

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

}

export class ReportFilterItem {
    reportExecutionLogPM: ReportExecutionLogPM;
    filters:string;
}










