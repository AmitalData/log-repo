import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReportsPreviewComponent} from '../../Components/ReportsPreviewComponent';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ReportFliter} from '../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';
import {Component, OnInit, Output, ElementRef}  from '@angular/core';
import {Guid} from '../../../Infrastructure/Utilities/Guid';
import {AppTool} from '../../../Infrastructure/Tools';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
@Component({

    
    selector: 'ParticipantsUsersActivitiesFilterComponent',
    templateUrl: './ParticipantsUsersActivitiesFilterComponent.html',
    inputs: ['ReportsPreview']


})
    

export class ParticipantsUsersActivitiesFilterComponent extends BaseComponent  {

    public ReportsPreview: ReportsPreviewComponent;
    Participant: string = null;
    reportFliter: ReportFliter;
    public ToDate: Date;
    public HeightControl: string;
    public FromDate: Date;
    queryFilterItems: QueryFilterItem[];
    queryFilterItem: QueryFilterItem;
    public ObjectTableName: string = "Report";
    public ValidationErrorsList: string[];

    public DataContext: ParticipantsUsersActivitiesFilterComponent = this;
    constructor() {
        super();
    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;

        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = this.DaysInMonth(new Date());
        this.FromDate = this.SetDate(Year, month - 1, 1);
        this.ToDate = this.SetDate(Year, month - 1, daysofmonth);
    }

    DaysInMonth(aDate: Date) {  
        return (new Date(aDate.getFullYear(), aDate.getMonth() + 1, 0)).getDate();
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
                case "FromDate":
                    this.FromDate = new Date(queryFilterItem.FieldValue);
                    break;
                case "ToDate":
                    this.ToDate = new Date(queryFilterItem.FieldValue);
                     break;
                case "ParticipantId":
                    this.Participant = queryFilterItem.FieldValue;
                    break;
               
               }
            }
    }
    ValidateSelectedFilters() {
        this.ValidationErrorsList = [];
        if (this.FromDate == null) {
            this.ValidationErrorsList.push("From Date is required");

        }

        if (this.ToDate == null) {
            this.ValidationErrorsList.push("To Date is required");

        }

        if (this.FromDate > this.ToDate) {
            this.ValidationErrorsList.push("From Date cannot be greater than To Date");

        }
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
            this.reportFliter.IncludeOperationalyClosed = false;

            this.ReportsPreview.CleanPartnersObslist();
            if (!AppTool.IsNullOrEmpty(this.Participant)) {
                this.ReportsPreview.AddPartner("Participant", this.Participant);
            }

            this.ReportsPreview.GenerateReport(this.reportFliter, isloading);
        }
    }
    GetQueryFilterItems(){
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
        this.queryFilterItem.FieldName = "ParticipantId";
        this.queryFilterItem.FieldValue = this.Participant;
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);
        return this.queryFilterItems;
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