import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReportsPreviewComponent} from '../../Components/ReportsPreviewComponent';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ReportFliter} from '../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';
import {Component, OnInit}  from '@angular/core';
import { AppTool } from '../../../Infrastructure/Tools';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
@Component({

    
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
        
    }
    public IsSchedulerReport : boolean = false;
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
                case "FlightDate":
                    this.FlightDate = new Date(queryFilterItem.FieldValue);
                    break;
                case "CustomAgentId":
                    this.CustomAgentId = queryFilterItem.FieldValue;
                    break;
                case "FlightNumber":
                    this.FlightNumber = queryFilterItem.FieldValue;
                    break;   
               }
            }
    }
    public CustomAgentId: string; 
    RunReport() {
       
        if (this.ValidateSelectedFilters()) {
            
            if (!this.DateType) {
                this.DateType = "CreateDate";
            }
            
            this.reportFliter = new ReportFliter();
            this.reportFliter.DateType = this.DateType;
            this.reportFliter.Tenant = SessionInfo.LoggedUserTenant;
            this.reportFliter.QueryFilterItemLists = this.GetQueryFilterItems();
            this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
            this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
            this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
            this.reportFliter.NumberOfPage = 1;
            this.reportFliter.ProcessType = "GenerateReport";

            this.ReportsPreview.CleanPartnersObslist();
            if (!AppTool.IsNullOrEmpty(this.CustomAgentId)) {
                this.ReportsPreview.AddPartner("Custom Agent", this.CustomAgentId);
            }
            this.ReportsPreview.GenerateReport(this.reportFliter, true);
        }
    } 
    GetQueryFilterItems(){
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

        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "CustomAgentId";
        this.queryFilterItem.FieldValue = this.CustomAgentId;
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);
        return this.queryFilterItems;
    }
    ValidateSelectedFilters() {
        this.ValidationErrorsList = [];
        if (this.FlightNumber == null || this.FlightNumber.trim() == '') {
            this.ValidationErrorsList.push("Flight Number is required");
        }

        if (this.FlightDate == null) {
            this.ValidationErrorsList.push("Flight Date is required");
        }
        return this.ValidationErrorsList.length == 0;
    }  
}

