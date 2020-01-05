import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReportsPreviewComponent} from '../../Components/ReportsPreviewComponent';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ReportFliter} from '../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';
import {Component, OnInit, Output, ElementRef}  from '@angular/core';
import {AppTool} from '../../../Infrastructure/Tools';
@Component({
    moduleId: module.id,
    selector: 'StatementFilterComponent',
    templateUrl: './StatementFilterComponent.html',
    inputs: ['ReportsPreview']
})

export class StatementFilterComponent extends BaseComponent implements OnInit {
    public ReportsPreview: ReportsPreviewComponent;    
    reportFliter: ReportFliter;
    
    queryFilterItem: QueryFilterItem;
    public queryFilterItems: QueryFilterItem[];
    
    public ObjectTableName: string = "Report";

    public DataContext: StatementFilterComponent = this;
    constructor() {
        super();
    }

    public CustomerId: string = null;
    public DueDate: Date = null;

    private includeDraftInvoices: boolean = false;
    public get IncludeDraftInvoices() { return this.includeDraftInvoices; }
    public set IncludeDraftInvoices(value: boolean) {
        if (this.includeDraftInvoices != value) {
            this.includeDraftInvoices = value;
        }
    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;
    }

    ngOnInit() {

        //if (!this.ReportsPreview.FilterConrolHeight) {
        //    this.ReportsPreview.SetFilterCotrolHeight(65);
        //}
        //else {
        //}
    }
    
    RunReport(isloading: boolean) {
        this.queryFilterItems = [];

        this.queryFilterItems = new Array<QueryFilterItem>();

        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "BillToId";
        this.queryFilterItem.FieldValue = this.CustomerId;
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);

        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "DueDate";
        this.queryFilterItem.FieldValue = this.DueDate;
        this.queryFilterItem.FieldDataType = "Date";
        this.queryFilterItem.Operator = "LessThanOrEqual";
        this.queryFilterItems.push(this.queryFilterItem);

        this.queryFilterItems.push(new QueryFilterItem("IncludeDraftInvoices", this.IncludeDraftInvoices));

        this.reportFliter = new ReportFliter();
        this.reportFliter.Tenant = SessionInfo.LoggedUserTenant;
        this.reportFliter.QueryFilterItemLists = this.queryFilterItems;
        this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
        this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
        this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
        this.reportFliter.NumberOfPage = 1;
        this.reportFliter.ProcessType = "GenerateReport";


        this.ReportsPreview.CleanPartnersObslist();
        if (!AppTool.IsNullOrEmpty(this.CustomerId)) this.ReportsPreview.AddPartner("Partner", this.CustomerId);

        this.ReportsPreview.GenerateReport(this.reportFliter, isloading);
    }
}


