import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReportsPreviewComponent} from '../../Components/ReportsPreviewComponent';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ReportFliter} from '../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';
import {Component, OnInit, Output, ElementRef}  from '@angular/core';
import {FormBuilder, FormGroup, FormsModule} from '@angular/forms';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {DateTool} from '../../../Infrastructure/Tools';
import {LastFilter} from '../../../Infrastructure/Utilities/LastFilter';
import {CodeNameClass} from './CodeNameClass';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {LogitudeListBoxComponent} from '../../../Infrastructure/Components/LogitudeComponents/LogitudeListBox/LogitudeListBoxComponent';

@Component({

    moduleId: module.id,
    selector: 'ActivityStatusDashboardFilterComponent',
    templateUrl: './ActivityStatusDashboardFilterComponent.html',
    inputs: ['ReportsPreview'],
    entryComponents: [LogitudeListBoxComponent]


})

    

export class ActivityStatusDashboardFilterComponent extends BaseComponent implements OnInit {
    public ReportsPreview: ReportsPreviewComponent;

    public timeRangeComboList: Array<LastFilter>;
    public showComboList: Array<CodeNameClass>;
    public SelectedItemShow: any;
    public get TimeRangeComboList() { return this.timeRangeComboList; }
    public set TimeRangeComboList(value: Array<LastFilter>) { this.timeRangeComboList = value; }
    public SelectedItem: LastFilter;
    public FlightNumber: string;
    public myForm: FormGroup;
    public queryFilterItems: QueryFilterItem[];
    public queryFilterItem: QueryFilterItem;
    public DateType: string;
    public reportFliter: ReportFliter;
    public DataContext: ActivityStatusDashboardFilterComponent = this;
    public ValidationErrorsList: string[];
    public ObjectTableName: string = "FlightsSchedulesResponse";

    private mySelectedTransportFilter: string = "All";
    public get MySelectedTransportFilter() { return this.mySelectedTransportFilter; }
    public set MySelectedTransportFilter(newValue: string) {
        if (this.mySelectedTransportFilter != newValue) {
            this.mySelectedTransportFilter = newValue;
        }

    }


    private mySelectedDirectionFilter: string = "All";
    public get MySelectedDirectionFilter() { return this.mySelectedDirectionFilter; }
    public set MySelectedDirectionFilter(newValue: string) {
        if (this.mySelectedDirectionFilter != newValue) {
            this.mySelectedDirectionFilter = newValue;
        }
    }

    constructor(fb: FormBuilder) {
        super();
        this.myForm = fb.group({});
    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;

        this.timeRangeComboList = LastFilter.myList();
        var lastFilter: LastFilter = new LastFilter();
        lastFilter.lastTitle = "Custom";
        lastFilter.LastDays = 0;
        lastFilter.Lastmonths = 0;

        this.timeRangeComboList.push(lastFilter);
        this.SelectedItem = this.timeRangeComboList[0];
        this.ComputeDays();


        this.BuildShowTypesFilters();
    }

    daysInMonth(aDate: Date) {
        return (new Date(aDate.getFullYear(), aDate.getMonth() + 1, aDate.getDate() + 1)).getDate();
    }


    ComputeDays() {
        var days;
        var Todate: Date = DateTool.GetDateParts(DateTool.GetCurrentDateAsUtc()).DateObject;
        this.activityFromDate = DateTool.GetDateParts(DateTool.GetCurrentDateAsUtc()).DateObject;
        if (this.SelectedItem.LastDays == -7) {
            days = -7;
            Todate.setDate(Todate.getDate() - 6);
            this.activityToDate = Todate;
        }

        else if (this.SelectedItem.LastDays == -30) {
            days = -30;
            Todate.setMonth(Todate.getMonth() - 1);
            this.activityToDate = Todate;
        }

        else if (this.SelectedItem.LastDays == -90) {
            days = -90;
            Todate.setMonth(Todate.getMonth() - 3);
            this.activityToDate = Todate;
        }

        else if (this.SelectedItem.LastDays == -365) {
            days = -365;
            Todate.setMonth(Todate.getMonth() - 12);
            this.activityToDate = Todate;
        }

        return days;

    }


    ngOnInit() {

        //if (!this.ReportsPreview.FilterConrolHeight) {
        //    this.ReportsPreview.SetFilterCotrolHeight(80);
        //}
        //this.timeRangeComboList = LastFilter.myList();
        //this.SelectedItem = this.timeRangeComboList[0];
        //this.BuildShowTypesFilters();
        
    }


    private activityToDate: Date;
    public get ActivityToDate() { return this.activityToDate; }
    public set ActivityToDate(value: Date) {
        if (value != this.activityToDate) {
            this.activityToDate = value;
            this.SelectedItem = this.TimeRangeComboList[4];

        }
    }

    private activityFromDate: Date;
    public get ActivityFromDate() { return this.activityFromDate; }
    public set ActivityFromDate(value: Date) {
        if (value != this.activityFromDate) {
            this.activityFromDate = value;
            this.SelectedItem = this.TimeRangeComboList[4];
        }
    }



    onSelectedItemChanged(item) {
        this.SelectedItem = item;
        this.ComputeDays();
    }


    onSelectedItemShowChanged(item) {
        this.SelectedItemShow = item;
    }

    public BuildShowTypesFilters() {

        this.showComboList = [];
        var item1: CodeNameClass = new CodeNameClass();
        item1.Code = "SHI";
        item1.Name = "Shipments";
        this.showComboList.push(item1);
        item1 = new CodeNameClass();
        item1.Code = "CHW";
        item1.Name = "ChargeWeight";
        this.showComboList.push(item1);

        item1 = new CodeNameClass();

        item1.Code = "GSW";
        item1.Name = "GrossWeight";
        this.showComboList.push(item1);

        item1 = new CodeNameClass();

        item1.Code = "PAC";
        item1.Name = "Profit " + "(" + SessionLocator.TenantPM.AccountingCurrencyCode + ")";
        this.showComboList.push(item1);

        item1 = new CodeNameClass();

        item1.Code = "PPT";
        item1.Name = "Profit " + "(" + SessionLocator.TenantPM.ProfitCurrencyCode + ")";
        this.showComboList.push(item1);

        item1 = new CodeNameClass();

        item1.Code = "RAC";
        //Receivables
        item1.Name = "Receivables " + "(" + SessionLocator.TenantPM.AccountingCurrencyCode + ")";
        this.showComboList.push(item1);
        item1 = new CodeNameClass();

        item1.Code = "RPT";
        item1.Name = "Receivables " + "(" + SessionLocator.TenantPM.ProfitCurrencyCode + ")";
        this.showComboList.push(item1);
        this.SelectedItemShow = this.showComboList[0];

    }

    RunReport() {


        this.ValidationErrorsList = [];

        if (!this.SelectedItem) {
            this.ValidationErrorsList.push("Time Range field is required");
        }

        if (!this.SelectedItemShow) {
            this.ValidationErrorsList.push("Show field is required");
        }

        if (this.SelectedItem.lastTitle == "Custom") {
            if (this.ActivityFromDate == null)
                this.ValidationErrorsList.push("From Date field is required");
            if (this.ActivityToDate == null)
                this.ValidationErrorsList.push("To Date field is required");

        }


        if (this.ValidationErrorsList.length == 0) {


            var showIndex;


            switch (this.SelectedItemShow.Code) {
                case "SHI":
                    {
                        showIndex = 0;
                        break;
                    }

                case "CHW":
                    {
                        showIndex = 1;
                        break;
                    }

                case "GSW":
                    {
                        showIndex = 2;
                        break;
                    }

                case "PAC":
                    {
                        showIndex = 3;
                        break;
                    }

                case "PPT":
                    {
                        showIndex = 4;
                        break;
                    }

                case "RAC":
                    {
                        showIndex = 5;
                        break;
                    }

                case "RPT":
                    {
                        showIndex = 6;
                        break;
                    }
            }

            if (this.MySelectedDirectionFilter == "All")
                this.MySelectedDirectionFilter = "";
            if (this.MySelectedTransportFilter == "All")
                this.MySelectedTransportFilter = "";

            this.queryFilterItems = [];
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "LastDays";
            this.queryFilterItem.FieldValue = this.SelectedItem.LastDays;

            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);


            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "LastMonths";
            this.queryFilterItem.FieldValue = 0 + "";
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);


            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "ShowIndex";
            this.queryFilterItem.FieldValue = showIndex;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);


            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "DirectionId";
            this.queryFilterItem.FieldValue = this.MySelectedDirectionFilter;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);


            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "TransportModeId";
            this.queryFilterItem.FieldValue = this.MySelectedTransportFilter;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);

                this.queryFilterItem = new QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "FromDate";
                this.queryFilterItem.FieldValue = this.ActivityToDate;
                this.queryFilterItem.FieldDataType = "Date";
                this.queryFilterItems.push(this.queryFilterItem);

                this.queryFilterItem = new QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "ToDate";
                this.queryFilterItem.FieldValue = this.ActivityFromDate;
                this.queryFilterItem.FieldDataType = "Date";
                this.queryFilterItems.push(this.queryFilterItem);
            


            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "TimeRange";
            this.queryFilterItem.FieldValue = this.SelectedItem.lastTitle;
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

