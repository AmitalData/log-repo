import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ReportFliter } from '../../../Components/Filters/ReportFliter';
import { QueryFilterItem } from '../../../Components/Filters/QueryFilterItem';
import { ReportsPreviewComponent } from '../../../Components/ReportsPreviewComponent';
import { DateTool, AppTool } from '../../../../Infrastructure/Tools';

@Component({
    moduleId: module.id,
    templateUrl: './BluesnapPaymentsReportFilterComponent.html',
})

export class BluesnapPaymentsReportFilterComponent extends BaseComponent {

    public ReportsPreview: ReportsPreviewComponent;
    public ValidationErrorsList: string[];
    public ObjectTableName: string = "Report";
    public DataContext: BluesnapPaymentsReportFilterComponent = this;

    constructor() {
        super();
    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;
        this.FromDate = DateTool.GetCurrentDateAsUtc();
        this.FromDate.setMonth(this.FromDate.getMonth() - 1);
        this.ToDate = DateTool.GetCurrentDateAsUtc();
    }

    public FromDate: Date;
    public ToDate: Date;
    public ShowAllRecurringTenants: boolean;
    queryFilterItems: QueryFilterItem[];
    queryFilterItem: QueryFilterItem;

    RunReport(isloading: boolean) {
        this.ValidationErrorsList = [];

        if (this.FromDate != null && this.ToDate != null) {
            if (this.ToDate < this.FromDate) {
                this.ValidationErrorsList.push("From date must be less than to date");
            }
        }

        if (AppTool.IsNullOrEmpty(this.ToDate)) {
            this.ValidationErrorsList.push("To Date is required");
        }

        if (AppTool.IsNullOrEmpty(this.FromDate)) {
            this.ValidationErrorsList.push("From Date is required");
        }

        if (this.ValidationErrorsList.length == 0) {
            this.queryFilterItems = new Array<QueryFilterItem>();

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
            this.queryFilterItem.FieldName = "ShowAllRecurringTenants";
            this.queryFilterItem.FieldValue = this.ShowAllRecurringTenants;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);

            var reportFliter = new ReportFliter();
            reportFliter.Tenant = SessionLocator.Tenant;
            reportFliter.QueryFilterItemLists = this.queryFilterItems;
            reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
            reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
            reportFliter.ReportCode = this.ReportsPreview.Report.Code;
            reportFliter.NumberOfPage = 1;
            reportFliter.ProcessType = "GenerateReport";

            this.ReportsPreview.GenerateReport(reportFliter, isloading);
        }
    }
}
