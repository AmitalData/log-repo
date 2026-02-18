import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReportsPreviewComponent} from '../../Components/ReportsPreviewComponent';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow'
import {ReportFliter} from '../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';
import {Component, OnInit, Output, ElementRef}  from '@angular/core';
import {AppTool} from '../../../Infrastructure/Tools';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';

@Component({
    
    selector: 'AccountingLedgerFilterComponent',
    templateUrl: './AccountingLedgerFilterComponent.html',
    inputs: ['ReportsPreview']
})

export class AccountingLedgerFilterComponent extends BaseComponent implements OnInit {
    public ReportsPreview: ReportsPreviewComponent;
    reportFliter: ReportFliter;
    queryFilterItems: QueryFilterItem[];
    queryFilterItem: QueryFilterItem;
    public ValidationErrorsList: string[] = [];
    public ObjectTableName: string = "Report";
    public DataContext: AccountingLedgerFilterComponent = this;

    CustomerId: string;
    PartnerId: string;
    FromDate: Date;
    ToDate: Date;

    public IsCreateDateId: string = "IsCreateDateId";
    public IsValueDateId: string = "IsValueDateId";
    public DateRadio: string = "DateRadio_";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

        this.IsValueDateId = this.IsValueDateId + this.CurrentSession.GetNewId(this.IsValueDateId);
        this.IsCreateDateId = this.IsCreateDateId + this.CurrentSession.GetNewId(this.IsCreateDateId);
        this.DateRadio = this.DateRadio + this.CurrentSession.GetNewId(this.DateRadio);
    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;

        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = this.daysInMonth(new Date());

        this.FromDate = this.SetDate(Year, month - 1, 1);
        this.ToDate = this.SetDate(Year, month, daysofmonth);
    }

    ngOnInit() {

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

    public IsByCreateDate: boolean = true;
    public IsValueDate: boolean = false;
    public IsCreateDate: boolean = true;
    IsCreateDateClicked() {
        this.IsByCreateDate = true;
    }
    IsValueDateClicked() {
        this.IsByCreateDate = false;
    }
    public IsSchedulerReport: boolean = false;
    SetQueryFilterItems(queryFilterItems: Array<QueryFilterItem>,isSchedulerReport:boolean=true) { 
        this.IsSchedulerReport = isSchedulerReport;
        if (queryFilterItems) {
            queryFilterItems.forEach(queryFilterItem => {
                this.SetFilterItem(queryFilterItem);
            });
        }
    }
    public RunReportTitle: string = 'Run Report';
        SetRunReportTitle() {
      
            if (this.IsSchedulerReport) {
                this.RunReportTitle = TextCodeTranslator.Translate("AgingReport.O.PreviewReport");
            }
            else {
                this.RunReportTitle = TextCodeTranslator.Translate("AgingReport.O.RunReport");
            }
       
    }
    private SetFilterItem(queryFilterItem: QueryFilterItem) {
        
        if (queryFilterItem) {
            switch (queryFilterItem.FieldName) {
                case "ToDate":
                    this.ToDate = new Date(queryFilterItem.FieldValue);
                    break;
                case "FromDate":
                    this.FromDate = new Date(queryFilterItem.FieldValue);
                break;
                case "PartnerId":
                    this.PartnerId = queryFilterItem.FieldValue;
                     break;
                 case "BillToId":
                    this.CustomerId = queryFilterItem.FieldValue;
                    break;
                case "IsByCreateDate":{
                    this.IsByCreateDate = queryFilterItem.FieldValue;
                    this.IsCreateDate = queryFilterItem.FieldValue;
                    this.IsValueDate = !queryFilterItem.FieldValue;
                     break;
                 }
                 case "CustomerId":
                    this.CustomerId = queryFilterItem.FieldValue;
                     break;
                                 
            }
   
    
        }
    }
    ValidateSelectedFilters() {
        this.ValidationErrorsList = [];

        if (this.FromDate == null) {
            this.ValidationErrorsList.push("From Date is required");
        }

        if (this.ToDate == null) {
            this.ValidationErrorsList.push("To Date is required");
        }

        if (this.FromDate > this.ToDate) {
            this.ValidationErrorsList.push("From Date cannot be greater than To Date");
        }

        return this.ValidationErrorsList.length == 0;
    }
    RunReport() {
        this.ValidationErrorsList = [];
       

        if (this.ValidateSelectedFilters()) {

            
            this.reportFliter = new ReportFliter();
            this.reportFliter.Tenant = SessionInfo.LoggedUserTenant;
            this.reportFliter.QueryFilterItemLists = this.GetQueryFilterItems();
            this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
            this.reportFliter.CustomerId = this.CustomerId;
            this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
            this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
            this.reportFliter.NumberOfPage = 1;
            this.reportFliter.ProcessType = "GenerateReport";

            this.ReportsPreview.CleanPartnersObslist();

            if (!AppTool.IsNullOrEmpty(this.CustomerId)) {
                this.ReportsPreview.AddPartner("Partner", this.CustomerId);
            }

            this.ReportsPreview.GenerateReport(this.reportFliter, true);
        }
    }
    GetQueryFilterItems(){
        this.queryFilterItems = new Array<QueryFilterItem>();

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "IsByCreateDate";
            this.queryFilterItem.FieldValue = this.IsByCreateDate;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "FromDate";
            this.queryFilterItem.FieldValue = this.FromDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItems.push(this.queryFilterItem);

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "ToDate";
            this.queryFilterItem.FieldValue = this.ToDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItems.push(this.queryFilterItem);

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "BillToId";
            this.queryFilterItem.FieldValue = this.CustomerId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "PartnerId";
            this.queryFilterItem.FieldValue = this.PartnerId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);

        return this.queryFilterItems;
    }
}
