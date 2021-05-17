import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReportsPreviewComponent} from '../../Components/ReportsPreviewComponent';
import {Component, OnInit}  from '@angular/core';
import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';
import {ReportFliter} from '../../Components/Filters/ReportFliter';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {AppTool} from '../../../Infrastructure/Tools';
@Component({
    
    selector: 'OpenShipmentsByCustomerFilterComponent',
    templateUrl: './OpenShipmentsByCustomerFilterComponent.html',
    inputs: ['ReportsPreview']
})

export class OpenShipmentsByCustomerFilterComponent extends BaseComponent implements OnInit {
    public ReportsPreview: ReportsPreviewComponent;
    public ValidationErrorsList: string[] = [];
    public ObjectTableName: string = "Report";
    public DataContext: OpenShipmentsByCustomerFilterComponent = this;
    public RunReportTitle: string = 'Run Report';
    public IsSchedulerReport: boolean = false;
    public CustomerChanged: boolean = false;

    constructor() {
        super();
    }

    ngOnInit() {

    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;
    }

    SetRunReportTitle() {
        this.RunReportTitle = 'Preview';
    }

    GetQueryFilterItems() {
        var queryFilterItems = new Array<QueryFilterItem>();
        var queryFilterItem: QueryFilterItem;

        queryFilterItem = new QueryFilterItem();
        queryFilterItem.DisplayInList = false;
        queryFilterItem.FieldName = "CustomerId";
        queryFilterItem.FieldValue = this.CustomerId;
        queryFilterItem.Operator = "Equals";
        queryFilterItems.push(queryFilterItem);

        return queryFilterItems;
    }

    SetQueryFilterItems(queryFilterItems: Array<QueryFilterItem>) { //For Report Scheduler
        this.IsSchedulerReport = true;
        if (queryFilterItems) {
            queryFilterItems.forEach(queryFilterItem => {
                this.SetFilterItem(queryFilterItem);
            });
        }
    }

    private SetFilterItem(queryFilterItem: QueryFilterItem) {
        if (queryFilterItem) {
            switch (queryFilterItem.FieldName) {
                case "CustomerId":
                    this.CustomerId = queryFilterItem.FieldValue;
                    break;
            }
        }
    }

    ValidateSelectedFilters() {
        this.ValidationErrorsList = [];
        var isValid: boolean = true;

        return isValid;
    }

    PrepareContactList() { //For Report Scheduler
        if (!AppTool.IsNullOrEmpty(this.CustomerId)) this.ReportsPreview.AddPartner("Customer", this.CustomerId);
    }

    IsPartnersChanged(SelectedTab) { //For Report Scheduler
        if (SelectedTab == '2')
            this.CustomerChanged = false;
        return this.CustomerChanged;
    }

    GetMainCustomerFieldName() { //For Report Scheduler
        return null;
    }

    public customerId: string;
    get CustomerId() { return this.customerId; }
    set CustomerId(value: string) {
        if (this.customerId != value) {
            this.SetCustomerChanged(this.customerId);
            this.customerId = value;
        }
    }

    private SetCustomerChanged(value: string) {
        if (value != undefined)
            this.CustomerChanged = true;
    }

    queryFilterItems: QueryFilterItem[];
    queryFilterItem: QueryFilterItem;
    RunReport(isloading: boolean) {      

        if (this.ValidateSelectedFilters()) {

            var reportFliter = new ReportFliter();
            reportFliter.Tenant = SessionInfo.LoggedUserTenant;
            reportFliter.QueryFilterItemLists = this.GetQueryFilterItems();
            reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
            reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
            reportFliter.ReportCode = this.ReportsPreview.Report.Code;
            reportFliter.NumberOfPage = 1;
            reportFliter.ProcessType = "GenerateReport";
            
            this.ReportsPreview.GenerateReport(reportFliter, isloading);

            this.ReportsPreview.CleanPartnersObslist();
            this.PrepareContactList();
        }
    }
}
