import {Component, OnInit, Output, ElementRef, EventEmitter}  from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReportsPreviewComponent} from '../../Components/ReportsPreviewComponent';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ReportFliter} from '../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';
import {AppTool} from '../../../Infrastructure/Tools';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
@Component({
    
    selector: 'StatementByInvoiceDateFilterComponent',
    templateUrl: './StatementByInvoiceDateFilterComponent.html',
    inputs: ['ReportsPreview']
})

export class StatementByInvoiceDateFilterComponent extends BaseComponent   {
    public ReportsPreview: ReportsPreviewComponent;
    public CustomerId: string = null;
    public PartnerId: string = null;    
    public ObjectTableName: string = "Report";
    public RunReportTitle: string = "Run Report";
    public ValidationErrorsList: string[];
    @Output() RunReportEvent: EventEmitter<ReportFliter> = new EventEmitter<ReportFliter>();

    public DataContext: StatementByInvoiceDateFilterComponent = this;

    constructor() {
        super();
        this.ValidationErrorsList = [];
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

        if (!AppTool.IsNullOrEmpty(this.CustomerId)) {
            queryFilterItem = new QueryFilterItem();
            queryFilterItem.DisplayInList = false;
            queryFilterItem.FieldName = "CustomerId";
            queryFilterItem.FieldValue = this.CustomerId;
            queryFilterItem.Operator = "Equals";
            queryFilterItems.push(queryFilterItem);
        }

        if (!AppTool.IsNullOrEmpty(this.PartnerId)) {
            queryFilterItem = new QueryFilterItem();
            queryFilterItem.DisplayInList = false;
            queryFilterItem.FieldName = "PartnerId";
            queryFilterItem.FieldValue = this.PartnerId;
            queryFilterItem.Operator = "Equals";
            queryFilterItems.push(queryFilterItem);
        }
        
        return queryFilterItems;
    }

    SetQueryFilterItems(queryFilterItems: Array<QueryFilterItem>,isSchedulerReport:boolean=true) { //For Scheduler Report
        this.RunReportTitle = "Preview";
        if (queryFilterItems) {
            queryFilterItems.forEach(queryFilterItem => {
                this.SetFilterItem(queryFilterItem);
            });
        }
    }

    private SetFilterItem(queryFilterItem: QueryFilterItem) {
        if (queryFilterItem) {
            if (queryFilterItem.FieldName == "CustomerId") {
                this.CustomerId = queryFilterItem.FieldValue;
            }
            if (queryFilterItem.FieldName == "PartnerId") {
                this.PartnerId = queryFilterItem.FieldValue;
            }
        }
    }

    ValidateSelectedFilters() {
        this.ValidationErrorsList = [];
        if (AppTool.IsNullOrEmpty(this.PartnerId)) {
            this.ValidationErrorsList.push("Please select a Partner");
            return false;
        }
        return true;
    }

    PrepareContactList() {
        this.ReportsPreview.CleanPartnersObslist();
        if (!AppTool.IsNullOrEmpty(this.CustomerId)) this.ReportsPreview.AddPartner("Partner", this.CustomerId);
    }

    RunReport() {
        if (this.ValidateSelectedFilters()) {
            var queryFilterItems: Array<QueryFilterItem> = this.GetQueryFilterItems();
            var reportFliter: ReportFliter;

            reportFliter = new ReportFliter();
            reportFliter.Tenant = SessionInfo.LoggedUserTenant;
            reportFliter.QueryFilterItemLists = queryFilterItems;
            reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
            reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
            reportFliter.ReportCode = this.ReportsPreview.Report.Code;
            reportFliter.NumberOfPage = 1;
            reportFliter.ProcessType = "GenerateReport";

            this.PrepareContactList();
            this.RunReportEvent.emit(reportFliter);
            //this.ReportsPreview.GenerateReport(this.reportFliter, isloading);
        }
    }
}
