import { Component } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ReportsPreviewComponent } from '../../Components/ReportsPreviewComponent';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { ReportFliter } from '../../Components/Filters/ReportFliter';
import { QueryFilterItem } from '../../Components/Filters/QueryFilterItem';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';

@Component({
    moduleId: module.id,
    selector: 'TasksOfNoProjectsFilterComponent',
    templateUrl: './TasksOfNoProjectsFilterComponent.html',
    inputs: ['ReportsPreview']
})

export class TasksOfNoProjectsFilterComponent extends BaseComponent {
    public ReportsPreview: ReportsPreviewComponent;
    public DataContext: TasksOfNoProjectsFilterComponent = this;
    reportFliter: ReportFliter;
    ToDate: Date;
    FromDate: Date;
    EmployeeUserId: string;
    queryFilterItems: QueryFilterItem[];
    public AgentId = null;
    queryFilterItem: QueryFilterItem;
    public ValidationErrorsList: string[];
    public ObjectTableName: string = "TMEmployeeTime";

    constructor() {
        super();

    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.EmployeeUserId = SessionLocator.LoggedUserId;

        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = this.daysInMonth(new Date());

        this.FromDate = this.SetDate(Year, month, 1);
        this.ToDate = this.SetDate(Year, month, daysofmonth);

        this.ReportsPreview = myReportsPreview;
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

    RunReport() {
        this.ValidationErrorsList = [];
        if (this.FromDate == null) {
            this.ValidationErrorsList.push("From Date is required");
        }
        if (this.ToDate == null) {
            this.ValidationErrorsList.push("To Date is required");
        }
   
        if (this.ValidationErrorsList.length == 0) {
            this.queryFilterItems = new Array<QueryFilterItem>();
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "FromDate";
            this.queryFilterItem.FieldValue = this.FromDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "ToDate";
            this.queryFilterItem.FieldValue = this.ToDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "EmployeeUserId";
            this.queryFilterItem.FieldValue = this.EmployeeUserId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);

            this.reportFliter = new ReportFliter();
            this.reportFliter.Tenant = SessionInfo.LoggedUserTenant;
            this.reportFliter.QueryFilterItemLists = this.queryFilterItems;
            this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
            this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
            this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
            this.reportFliter.NumberOfPage = 1;
            this.reportFliter.ProcessType = "GenerateReport";
            this.ReportsPreview.GenerateReport(this.reportFliter, true);
        }
    }
}
