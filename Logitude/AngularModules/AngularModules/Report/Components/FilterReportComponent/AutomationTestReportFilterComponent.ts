import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ReportsPreviewComponent } from '../../Components/ReportsPreviewComponent';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { ReportFliter } from '../../Components/Filters/ReportFliter';
import { QueryFilterItem } from '../../Components/Filters/QueryFilterItem';
import { Component, Output, EventEmitter } from '@angular/core';

@Component({
    
    selector: 'AutomationTestReportFilterComponent',
    templateUrl: './AutomationTestReportFilterComponent.html',
    inputs: ['ReportsPreview']
})

export class AutomationTestReportFilterComponent extends BaseComponent {
    public ReportsPreview: ReportsPreviewComponent;
    public DataContext: AutomationTestReportFilterComponent = this;
    public ObjectTableName: string = "Report";
    public RunReportTitle: string = "Run Report";
    public IsReportScheduler: boolean = false;
    IsException: boolean = true;
    @Output() RunReportEvent: EventEmitter<ReportFliter> = new EventEmitter<ReportFliter>();

    constructor() {
        super();
    }
    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;
    }

    SetRunReportTitle() {
        this.RunReportTitle = "Preview";
    }

    GetMainCustomerFieldName() {
        return null;
    }

    IsPartnersChanged() {
        return false;
    }

    GetQueryFilterItems() {
        var queryFilterItems = new Array<QueryFilterItem>();
        var queryFilterItem: QueryFilterItem;

        //IsException
        queryFilterItem = new QueryFilterItem();
        queryFilterItem.DisplayInList = false;
        queryFilterItem.FieldName = "IsException";
        queryFilterItem.FieldValue = this.IsException ? true : false;
        queryFilterItem.Operator = "Equals";
        queryFilterItems.push(queryFilterItem);

        return queryFilterItems;
    }


    SetQueryFilterItems(queryFilterItems: Array<QueryFilterItem>) { //For Scheduler Report
        this.RunReportTitle = "Preview";
        this.IsReportScheduler = true;
        if (queryFilterItems) {
            queryFilterItems.forEach(queryFilterItem => {
                this.SetFilterItem(queryFilterItem);
            });
        }
    }

    private SetFilterItem(queryFilterItem: QueryFilterItem) {
        if (queryFilterItem) {
            if (queryFilterItem.FieldName == "IsException") {
                this.IsException = queryFilterItem.FieldValue;
            }
        }
    }

    ValidateSelectedFilters() {
        var isValid: boolean = true;

        //nothing to validate

        return isValid;
    }

    PrepareContactList() {
        //for report scheduler
    }

    RunReport() {
        var reportFliter: ReportFliter = new ReportFliter();

        reportFliter.Tenant = SessionInfo.LoggedUserTenant;
        reportFliter.QueryFilterItemLists = this.GetQueryFilterItems();
        reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
        reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
        reportFliter.ReportCode = this.ReportsPreview.Report.Code;
        reportFliter.NumberOfPage = 1;
        reportFliter.ProcessType = "GenerateReport";

        reportFliter.IncludeOperationalyClosed = false;

        this.RunReportEvent.emit(reportFliter);
        //this.ReportsPreview.GenerateReport(this.reportFliter, isloading);
    }
}
