import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReportsPreviewComponent} from '../../Components/ReportsPreviewComponent';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ReportFliter} from '../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';
import {Component, OnInit, Output, ElementRef}  from '@angular/core';
import {FormBuilder, FormGroup, FormsModule} from '@angular/forms';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
@Component({

    moduleId: module.id,
    selector: 'FlightBookingFilterComponent',
    templateUrl: './FlightBookingFilterComponent.html',
    inputs: ['ReportsPreview']

    
})

   





export class FlightBookingFilterComponent extends BaseComponent implements OnInit {
    public ReportsPreview: ReportsPreviewComponent;

    public FlightNumber: string;
    private flightDate: Date;
    public get FlightDate() { return this.flightDate; }
    public set FlightDate(value: Date) { if (this.flightDate != value) this.flightDate = value; }
    public queryFilterItems: QueryFilterItem[];
    public queryFilterItem: QueryFilterItem;
    public DateType: string;
    public reportFliter: ReportFliter;
    public DataContext: FlightBookingFilterComponent = this;
    public ValidationErrorsList: string[];
    public ObjectTableName: string = "FlightsSchedulesResponse";
    constructor() {
        super();
        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = this.daysInMonth(new Date());
        this.FlightDate = this.SetDate(Year, month, 1);
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

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;      
    }

    daysInMonth(aDate: Date) {
        return (new Date(aDate.getFullYear(), aDate.getMonth() + 1, aDate.getDate()+1 )).getDate();
    }

    ngOnInit() {

        //if (!this.ReportsPreview.FilterConrolHeight) {
        //    this.ReportsPreview.SetFilterCotrolHeight(85);
        //}
        //else {

        //    var month = new Date().getMonth();
        //    var Year = new Date().getFullYear();
        //    var daysofmonth = this.daysInMonth(new Date());


        //    this.date = new Date(Year, month, 2);

        //}
    }

    RunReport() {

        this.ValidationErrorsList = [];
        if (this.FlightNumber == null || this.FlightNumber.trim() == '') {

            this.ValidationErrorsList.push("Flight Number is required");

        }

        if (this.FlightDate == null) {

            this.ValidationErrorsList.push("Flight Date is required");

        }
        if (this.ValidationErrorsList.length==0) {
            this.queryFilterItems = new Array<QueryFilterItem>();



            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "FlightDate";
            this.queryFilterItem.FieldValue = this.FlightDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "FlightNumber";
            this.queryFilterItem.FieldValue = this.FlightNumber;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);


            if (!this.DateType) {
                this.DateType = "CreateDate";
            }




            this.reportFliter = new ReportFliter();
            this.reportFliter.DateType = this.DateType;
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
