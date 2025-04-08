import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ReportsPreviewComponent } from '../../../Components/ReportsPreviewComponent';
import { SessionInfo } from '../../../../Infrastructure/Utilities/SessionInfo';
import { ReportFliter } from '../../../Components/Filters/ReportFliter';
import { QueryFilterItem } from '../../../Components/Filters/QueryFilterItem';
import { Component } from '@angular/core';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';

@Component({
    
    selector: 'RacingQuotesComponent',
    templateUrl: './RacingQuotesComponent.html',
    inputs: ['ReportsPreview']
})

export class RacingQuotesComponent extends BaseComponent {
    public ReportsPreview: ReportsPreviewComponent;
    reportFliter: ReportFliter;
    ToDate: Date;
    FromDate: Date;
    queryFilterItems: QueryFilterItem[];
    queryFilterItem: QueryFilterItem;

    public ObjectTableName: string = "Report";
    public DataContext: RacingQuotesComponent = this;
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
              
                case "FromDate":
                    this.FromDate = new Date(queryFilterItem.FieldValue);
                    break;
                case "ToDate":
                    this.ToDate = new Date(queryFilterItem.FieldValue);
                    break;
               
            }
   
        }
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
    GetQueryFilterItems(){
        this.queryFilterItems = new Array<QueryFilterItem>();
        if (this.FromDate) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "FromDate";
            this.queryFilterItem.FieldValue = this.FromDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItem.Operator = "GreaterThanOrEqual";
            this.queryFilterItems.push(this.queryFilterItem);
        }
        if (this.ToDate) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "ToDate";
            this.queryFilterItem.FieldValue = this.ToDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItem.Operator = "LessThanOrEqual";
            this.queryFilterItems.push(this.queryFilterItem);
        }
        return this.queryFilterItems;
    }
    ValidateSelectedFilters(){
        this.ValidationErrorsList = [];
        if (this.FromDate > this.ToDate)
            this.ValidationErrorsList.push("From date field must be less than To date field");
        return this.ValidationErrorsList.length == 0;
    }
    RunReport(isloading: boolean) {
        
        if (this.ValidateSelectedFilters()) {
           

            this.reportFliter = new ReportFliter();
            this.reportFliter.Tenant = SessionInfo.LoggedUserTenant;
            this.reportFliter.QueryFilterItemLists = this.GetQueryFilterItems();
            this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;

            this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
            this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
            this.reportFliter.NumberOfPage = 1;
            this.reportFliter.ProcessType = "GenerateReport";

            this.ReportsPreview.CleanPartnersObslist();

            this.ReportsPreview.GenerateReport(this.reportFliter, isloading);
        }
    }
}
