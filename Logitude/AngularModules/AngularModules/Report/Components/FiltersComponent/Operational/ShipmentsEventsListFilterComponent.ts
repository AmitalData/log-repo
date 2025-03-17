import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ReportsPreviewComponent } from '../../../Components/ReportsPreviewComponent';
import { SessionInfo } from '../../../../Infrastructure/Utilities/SessionInfo';
import { ReportFliter } from '../../../Components/Filters/ReportFliter';
import { QueryFilterItem } from '../../../Components/Filters/QueryFilterItem';
import { Component } from '@angular/core';
import { DateTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DashBoardFilters } from '../../../../Infrastructure/DataContracts/Dashboard/DashboardFilters';
import { AppTool } from '../../../../Infrastructure/Tools';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
@Component({

    selector: 'ShipmentsEventsListFilterComponent',
    templateUrl: './ShipmentsEventsListFilterComponent.html',
    inputs: ['ReportsPreview']
})

export class ShipmentsEventsListFilterComponent extends BaseComponent {
    public ReportsPreview: ReportsPreviewComponent;
    reportFliter: ReportFliter;
    public ObjectTableName: string = "Report";
    ToDate: Date;
    FromDate: Date;
    ManuallyAddedEventsOnly: boolean = true;
    queryFilterItems: QueryFilterItem[];
    queryFilterItem: QueryFilterItem;
    public DataContext: ShipmentsEventsListFilterComponent = this;
    constructor() {
        super();
        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = this.daysInMonth(new Date());
        this.FromDate = this.SetDate(Year, month, 1);
        this.ToDate = this.SetDate(Year, month, daysofmonth);

    }


    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;

    }


    public IsSchedulerReport: boolean = false;
    SetQueryFilterItems(queryFilterItems: Array<QueryFilterItem>, isSchedulerReport: boolean = true) {
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

    ValidationErrorsList: string[];
    ValidateSelectedFilters() {

        this.ValidationErrorsList = [];
        if (this.FromDate == null) {
            this.ValidationErrorsList.push("From date is required");

        }

        if (this.ToDate == null) {
            this.ValidationErrorsList.push("To date is required");
        }



        if (this.ToDate && this.FromDate) {

            if (this.ToDate < this.FromDate) {
                this.ValidationErrorsList.push("From date must be less than to date");
            }

            if (((this.ToDate.valueOf() - this.FromDate.valueOf()) / (1000 * 60 * 60 * 24)) > 31) {
                this.ValidationErrorsList.push("Dates should be within one month");
            }
        }

        return this.ValidationErrorsList.length == 0;
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
    private SetFilterItem(queryFilterItem: QueryFilterItem) {
        if (queryFilterItem) {
            switch (queryFilterItem.FieldName) {
                case "FromDate":
                    this.FromDate = new Date(queryFilterItem.FieldValue);
                    break;
                case "ToDate":
                    this.ToDate = new Date(queryFilterItem.FieldValue);
                    break;
                case "UserId":
                    this.UserId = queryFilterItem.FieldValue;
                    break;
                case "EventTypeId":
                    this.EventTypeId = queryFilterItem.FieldValue;
                    break;
                case "ManuallyAddedEventsOnly":
                    this.ManuallyAddedEventsOnly = queryFilterItem.FieldValue;
                    break;
            }
        }
    }
    UserId: string;
    EventTypeId: string;
    RunReport(isloading: boolean) {


        //this.SetUIProperties();
        if (this.ValidateSelectedFilters()) {

            this.reportFliter = new ReportFliter();
            this.reportFliter.Tenant = SessionInfo.LoggedUserTenant;
            this.reportFliter.QueryFilterItemLists = this.GetQueryFilterItems();
            this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
            this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
            this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
            this.reportFliter.NumberOfPage = 1;
            this.reportFliter.ProcessType = "GenerateReport";


            this.reportFliter.IncludeOperationalyClosed = false;

            this.ReportsPreview.GenerateReport(this.reportFliter, isloading);
        }





    }
    GetQueryFilterItems() {
        //FromDate
        this.queryFilterItems = new Array<QueryFilterItem>();
        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "FromDate";
        this.queryFilterItem.FieldValue = this.FromDate;
        this.queryFilterItem.FieldDataType = "Date";
        this.queryFilterItem.Operator = "GreaterThanOrEqual";
        this.queryFilterItems.push(this.queryFilterItem);


        //ToDate
        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "ToDate";
        this.queryFilterItem.FieldValue = this.ToDate;
        this.queryFilterItem.FieldDataType = "Date";
        this.queryFilterItem.Operator = "LessThanOrEqual";
        this.queryFilterItems.push(this.queryFilterItem);


        //UserId
        if (this.UserId) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "UserId";
            this.queryFilterItem.FieldValue = this.UserId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }

        if (this.EventTypeId) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "EventTypeId";
            this.queryFilterItem.FieldValue = this.EventTypeId;
            this.queryFilterItem.FieldDataType = "string";
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }



        //ManuallyAddedEventsOnly
        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "ManuallyAddedEventsOnly";
        this.queryFilterItem.FieldValue = this.ManuallyAddedEventsOnly;
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);

        return this.queryFilterItems;
    }
}
