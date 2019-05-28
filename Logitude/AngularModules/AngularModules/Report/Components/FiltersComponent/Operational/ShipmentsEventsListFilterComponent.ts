import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ReportsPreviewComponent } from '../../../Components/ReportsPreviewComponent';
import { SessionInfo } from '../../../../Infrastructure/Utilities/SessionInfo';
import { ReportFliter } from '../../../Components/Filters/ReportFliter';
import { QueryFilterItem } from '../../../Components/Filters/QueryFilterItem';
import { Component } from '@angular/core';
import { DateTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DashBoardFilters } from '../../../../Infrastructure/DataContracts/Dashboard/DashboardFilters';
import {AppTool} from '../../../../Infrastructure/Tools';
@Component({
    moduleId: module.id,
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


    //SetUIProperties() {
    //    this.UIProperties.SetRequired("FromDate", null, AppTool.IsNullOrEmpty(this.FromDate) ? true : false);
    //    this.UIProperties.SetRequired("ToDate", null, AppTool.IsNullOrEmpty(this.ToDate) ? true : false);
    //}


    ValidationErrorsList: string[];
    Validate() {

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

    UserId: string;
    RunReport(isloading: boolean) {

        this.Validate();
        //this.SetUIProperties();
        if (this.ValidationErrorsList.length == 0) {
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



            //ManuallyAddedEventsOnly
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "ManuallyAddedEventsOnly";
            this.queryFilterItem.FieldValue = this.ManuallyAddedEventsOnly;
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


            this.reportFliter.IncludeOperationalyClosed = false;

            this.ReportsPreview.GenerateReport(this.reportFliter, isloading);
        }

      



    }
}
