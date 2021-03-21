import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ReportsPreviewComponent } from '../../Components/ReportsPreviewComponent';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { ReportFliter } from '../../Components/Filters/ReportFliter';
import { QueryFilterItem } from '../../Components/Filters/QueryFilterItem';
import { Component } from '@angular/core';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { AdvancedDatePickerResolverComponent } from '../../../Infrastructure/Components/LogitudeComponents/AdvancedDatePickerResolverComponent';

@Component({
    selector: 'ShipmentDetailsFilterComponent',
    templateUrl: './ShipmentDetailsFilterComponent.html',
    inputs: ['ReportsPreview'],
})
export class ShipmentDetailsFilterComponent extends BaseComponent {
    public ReportsPreview: ReportsPreviewComponent;
    reportFliter: ReportFliter;
    public ObjectTableName: string = 'Report';
    public DataContext: ShipmentDetailsFilterComponent = this;
    public ValidationErrorsList: string[] = [];
    public IsSchedulerReport: boolean = false;
    queryFilterItems: QueryFilterItem[];
    queryFilterItem: QueryFilterItem;

    ToDate: Date;
    FromDate: Date;

    constructor() {
        super();
    }
    public RunReportTitle: string = 'Run Report';
    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;
        this.FillDefaultDateDetails();
    }

    FillDefaultDateDetails() {
        if (!this.IsSchedulerReport) {
            const month = new Date().getMonth();
            const Year = new Date().getFullYear();
            this.ToDate = AppTool.IsNullOrEmpty(this.ToDate) ? this.SetDate(Year, month) : this.ToDate;
            this.FromDate = AppTool.IsNullOrEmpty(this.FromDate) ? this.SetDate(Year, month - 1) : this.FromDate;
        }
    }
  
    SetDate(year: number, month: number) {
        var date = new Date();
        date.setUTCFullYear(year);
        date.setUTCMonth(month);
        date.setUTCHours(0);
        date.setUTCMinutes(0);
        date.setUTCSeconds(0);
        return date;
    }

    SetRunReportTitle() {
        this.RunReportTitle = 'Preview';
    }
    ValidateSelectedFilters() {
        this.ValidationErrorsList = [];
        let isValid: boolean = true;

        if (this.FromDate == null) {
            this.ValidationErrorsList.push('From Date is required');
            isValid = false;
        }

        if (this.ToDate == null) {
            this.ValidationErrorsList.push('To Date is required');
            isValid = false;
        }
        var advancedDatePickerResolverComponent: AdvancedDatePickerResolverComponent = new AdvancedDatePickerResolverComponent();
        if (!advancedDatePickerResolverComponent.SetValidityBetweenTwoDateOptions(this.FromDate, this.ToDate)) {
            this.ValidationErrorsList.push('From Date cannot be greater than To Date');
            isValid = false;
        }

        return isValid;
    }

    IsPartnersChanged() {
        return false;
    }
    PrepareContactList() {
        //for report scheduler
    }
    SetQueryFilterItems(queryFilterItems: Array<QueryFilterItem>) {
        //For Scheduler Report
        this.IsSchedulerReport = true;

        if (queryFilterItems) {
            queryFilterItems.forEach((queryFilterItem) => {
                this.SetFilterItem(queryFilterItem);
            });
        }
    }

    private SetFilterItem(queryFilterItem: QueryFilterItem) {
        if (queryFilterItem) {
            if (queryFilterItem.FieldName == 'FromDate') {
                this.FromDate = queryFilterItem.FieldValue;
            }
            if (queryFilterItem.FieldName == 'ToDate') {
                this.ToDate = queryFilterItem.FieldValue;
            }
        }
    }

    GetQueryFilterItems() {
        const queryFilterItems = new Array<QueryFilterItem>();

        if (!AppTool.IsNullOrEmpty(this.FromDate)) {
            let queryFilterItem = new QueryFilterItem();
            queryFilterItem.DisplayInList = false;
            queryFilterItem.FieldName = 'FromDate';
            queryFilterItem.FieldValue = this.FromDate;
            queryFilterItem.FieldDataType = 'Date';
            queryFilterItems.push(queryFilterItem);
        }

        if (!AppTool.IsNullOrEmpty(this.ToDate)) {
            let queryFilterItem = new QueryFilterItem();
            queryFilterItem.DisplayInList = false;
            queryFilterItem.FieldName = 'ToDate';
            queryFilterItem.FieldValue = this.ToDate;
            queryFilterItem.FieldDataType = 'Date';
            queryFilterItems.push(queryFilterItem);
        }
        return queryFilterItems;
    }

    RunReport(isloading: boolean) {
        if (isloading) {
            if (this.ValidateSelectedFilters()){
                this.queryFilterItems = this.GetQueryFilterItems();

                this.reportFliter = new ReportFliter();
                this.reportFliter.Tenant = SessionInfo.LoggedUserTenant;
                this.reportFliter.QueryFilterItemLists = this.queryFilterItems;
                this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
                this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
                this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
                this.reportFliter.NumberOfPage = 1;
                this.reportFliter.ProcessType = 'GenerateReport';

                this.ReportsPreview.CleanPartnersObslist();
                this.ReportsPreview.GenerateReport(
                    this.reportFliter,
                    isloading
                );
            }
        }
    }
}
