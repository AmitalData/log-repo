import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReportsPreviewComponent} from '../../Components/ReportsPreviewComponent';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ReportFliter} from '../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Component, OnInit, Output, ElementRef}  from '@angular/core';
import {FormBuilder, FormGroup, FormsModule} from '@angular/forms';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {ParticipantList} from '../../EntityLists/ParticipantList';
import {AppTool} from '../../../Infrastructure/Tools';
import {CodeNameClass} from './CodeNameClass';


@Component({
    moduleId: module.id,
    selector: 'CASSReportFilterComponent',
    templateUrl: './CASSReportFilterComponent.html',
    inputs: ['ReportsPreview']
})

export class CASSReportFilterComponent extends BaseComponent implements OnInit {
    public ReportsPreview: ReportsPreviewComponent;


    reportFliter: ReportFliter;
    ToDate: Date;
    public HeightControl: string;
    FromDate: Date;
    public ValidationErrorsList: string[];
    private mainCarriageCarrierId: string;
    public get MainCarriageCarrierId() { return this.mainCarriageCarrierId; }
    public set MainCarriageCarrierId(value: string) { this.mainCarriageCarrierId = value; }

    public myForm: FormGroup;
    queryFilterItems: QueryFilterItem[];

    queryFilterItem: QueryFilterItem;
    public DataContext: CASSReportFilterComponent = this;
    constructor() {
        super();
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
        //if (!this.ReportsPreview.FilterConrolHeight) {
        //    this.ReportsPreview.SetFilterCotrolHeight(80);
        //}
        //else {


        //    var month = new Date().getMonth();
        //    var Year = new Date().getFullYear();
        //    var daysofmonth = this.daysInMonth(new Date());
        //    this.FromDate = this.SetDate(Year, month - 1, 1);            
        //    this.ToDate = this.SetDate(Year, month, daysofmonth);
        //}
    }    
    daysInMonth(aDate: Date) {
        return (new Date(aDate.getFullYear(), aDate.getMonth() + 1, 0)).getDate();
    }


    Validate() {

        this.ValidationErrorsList = [];
        if (AppTool.IsNullOrEmpty(this.MainCarriageCarrierId)) {
            this.ValidationErrorsList.push("Airline is required");
        }

        if (this.FromDate == null) {
            this.ValidationErrorsList.push("From date is required");
        }

        if (this.ToDate == null) {
            this.ValidationErrorsList.push("To date is required");
        }

        if (this.ToDate < this.FromDate) {
            this.ValidationErrorsList.push("From date must be less than to date");
        }


    }

    RunReport(isloading: boolean) {




        this.Validate();

        if (this.ValidationErrorsList.length == 0) {
            this.queryFilterItems = new Array<QueryFilterItem>();
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "FromDate";
            this.queryFilterItem.FieldValue = this.FromDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItem.Operator = "GreaterThanOrEqual";
            this.queryFilterItems.push(this.queryFilterItem);



            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "ToDate";
            this.queryFilterItem.FieldValue = this.ToDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItem.Operator = "LessThanOrEqual";
            this.queryFilterItems.push(this.queryFilterItem);

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "AirlineId";
            this.queryFilterItem.FieldValue = this.MainCarriageCarrierId;
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


            this.ReportsPreview.CleanPartnersObslist();
            if (!AppTool.IsNullOrEmpty(this.MainCarriageCarrierId)) {
                this.ReportsPreview.AddPartner("Airline", this.MainCarriageCarrierId);
            }



            this.ReportsPreview.GenerateReport(this.reportFliter, isloading);
        }

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



}