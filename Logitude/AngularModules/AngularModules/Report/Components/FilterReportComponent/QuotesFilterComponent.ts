import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReportsPreviewComponent} from '../../Components/ReportsPreviewComponent';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ReportFliter} from '../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';
import {Component}  from '@angular/core';
import {AppTool} from '../../../Infrastructure/Tools';

@Component({
    moduleId: module.id,
    selector: 'QuotesFilterComponent',
    templateUrl: './QuotesFilterComponent.html',
    inputs: ['ReportsPreview']
})

export class QuotesFilterComponent extends BaseComponent   {
    public ReportsPreview: ReportsPreviewComponent;
    reportFliter: ReportFliter;
    ToDate: Date;
    FromDate: Date;
    public SalesmanUserId: string;
    queryFilterItems: QueryFilterItem[];
    public customerId = null;
    public get CustomerId() { return this.customerId; }
    public set CustomerId(value: string) { this.customerId = value; }
    queryFilterItem: QueryFilterItem;
    public ObjectTableName: string = "Report";
    public DataContext: QuotesFilterComponent = this;
    public objectTableName = "Quote";
    public ValidationErrorsList: Array<string> = [];
    constructor() {
        super();
    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;

        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = this.daysInMonth(new Date());
        
        this.FromDate = this.SetDate(Year, month - 1, 1);
    }

    daysInMonth(aDate: Date) {
        return (new Date(aDate.getFullYear(), aDate.getMonth() + 1, 0)).getDate();
    }

    SetDate(year: number, month: number, day: number) {
        var date = new Date();
        date.setUTCFullYear(year);
        date.setUTCMonth(month);
        date.setUTCDate(day);
        date.setUTCHours(0);
        date.setUTCMinutes(0);
        date.setUTCSeconds(0);
        return date;
    }

    RunReport(isloading: boolean) {
        this.ValidationErrorsList = [];
        if (this.FromDate > this.ToDate)
            this.ValidationErrorsList.push("From date field must be less than To date field");

        if (this.ValidationErrorsList.length == 0) {
            this.queryFilterItems = new Array<QueryFilterItem>();
            if (this.FromDate) {
                this.queryFilterItem = new QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "OpenDate";
                this.queryFilterItem.FieldValue = this.FromDate;
                this.queryFilterItem.FieldDataType = "Date";
                this.queryFilterItem.Operator = "GreaterThanOrEqual";
                this.queryFilterItems.push(this.queryFilterItem);
            }
            if (this.ToDate) {
                this.queryFilterItem = new QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "ExpirationDate";
                this.queryFilterItem.FieldValue = this.ToDate;
                this.queryFilterItem.FieldDataType = "Date";
                this.queryFilterItem.Operator = "LessThanOrEqual";
                this.queryFilterItems.push(this.queryFilterItem);
            }

            if (this.CustomerId) {
                this.queryFilterItem = new QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "CustomerId";
                this.queryFilterItem.FieldValue = this.CustomerId;
                this.queryFilterItem.Operator = "Equals";
                this.queryFilterItems.push(this.queryFilterItem);                
            }

            if (this.SalesmanUserId) {
                this.queryFilterItem = new QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "SalesmanUserId";
                this.queryFilterItem.FieldValue = this.SalesmanUserId;
                this.queryFilterItem.Operator = "Equals";
                this.queryFilterItems.push(this.queryFilterItem);                
            }   
                     
            this.reportFliter = new ReportFliter();
            this.reportFliter.Tenant = SessionInfo.LoggedUserTenant;
            this.reportFliter.QueryFilterItemLists = this.queryFilterItems;
            this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;

            this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
            this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
            this.reportFliter.NumberOfPage = 1;
            this.reportFliter.ProcessType = "GenerateReport";

            this.ReportsPreview.CleanPartnersObslist();
            if (!AppTool.IsNullOrEmpty(this.CustomerId)) this.ReportsPreview.AddPartner("Customer", this.CustomerId);

            this.ReportsPreview.GenerateReport(this.reportFliter, isloading);
        }
    }
}