import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ReportsPreviewComponent } from '../../../Components/ReportsPreviewComponent';
import { SessionInfo } from '../../../../Infrastructure/Utilities/SessionInfo';
import { ReportFliter } from '../../../Components/Filters/ReportFliter';
import { QueryFilterItem } from '../../../Components/Filters/QueryFilterItem';
import { AppTool } from '../../../../Infrastructure/Tools';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';

@Component({
    
    selector: 'FlightBookingsManifestFilterComponent',
    templateUrl: './FlightBookingsManifestFilterComponent.html',
    inputs: ['ReportsPreview']
})

export class FlightBookingsManifestFilterComponent extends BaseComponent {
    public ReportsPreview: ReportsPreviewComponent;
    
    public queryFilterItems: QueryFilterItem[];
    public queryFilterItem: QueryFilterItem;
    public DateType: string;
    public reportFliter: ReportFliter;
    public DataContext: FlightBookingsManifestFilterComponent = this;
    public ValidationErrorsList: string[];
    public ObjectTableName: string = "FlightsSchedulesResponse";
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

    private DaysInMonth(aDate: Date) {
        return (new Date(aDate.getFullYear(), aDate.getMonth() + 1, aDate.getDate() + 1)).getDate();
    }
    private SetDate(year: number, month: number, day: number) {
        var date = new Date();
        date.setUTCFullYear(year);
        date.setUTCMonth(month);
        date.setUTCDate(day);
        date.setUTCHours(0);
        date.setUTCMinutes(0);
        date.setUTCSeconds(0);
        return date;
    }

    public FromDate: Date;
    public ToDate: Date;
    public FlightNumber: string;
    public MainCarriageFromPortId: string;
    public MainCarriageFinalDestinationPortId: string;
    public ClearingAgentId: string;
    public ConsigneeId: string;
    public ShipperId: string;
    public CutOffDate: Date;

    public RunReportTitle: string = 'Run Report';
    SetRunReportTitle() {
        
            if (this.IsSchedulerReport) {
                this.RunReportTitle = TextCodeTranslator.Translate("AgingReport.O.PreviewReport");
            }
            else {
                this.RunReportTitle = TextCodeTranslator.Translate("AgingReport.O.RunReport");
            }
        
    }
    public IsSchedulerReport: boolean = false;
    SetQueryFilterItems(queryFilterItems: Array<QueryFilterItem>,isSchedulerReport:boolean=true) { 
        this.IsSchedulerReport = isSchedulerReport;
        if (queryFilterItems) {
            queryFilterItems.forEach(queryFilterItem => {
                this.SetFilterItem(queryFilterItem);
            });
        }
    }
    private SetFilterItem(queryFilterItem: QueryFilterItem) {
   
      
   
        if (queryFilterItem) {
            switch (queryFilterItem.FieldName) {
                case "FlightNumber":
                    this.FlightNumber= queryFilterItem.FieldValue;
                    break;
                case "FromDate":
                    this.FromDate = new Date(queryFilterItem.FieldValue);
                    break;
                case "ToDate":
                    this.ToDate = new Date(queryFilterItem.FieldValue);
                     break;
                case "MainCarriageFromPortId":
                    this.MainCarriageFromPortId = queryFilterItem.FieldValue;
                    break;
                case "ClearingAgentId":
                    this.ClearingAgentId = queryFilterItem.FieldValue;
                    break;
                case "MainCarriageFinalDestinationPortId":
                    this.MainCarriageFinalDestinationPortId = queryFilterItem.FieldValue;
                    break;
                case "ConsigneeId":
                    this.ConsigneeId = queryFilterItem.FieldValue;
                    break; 
                 case "ShipperId":
                        this.ShipperId = queryFilterItem.FieldValue;
                        break;    
                case "CutOffDate":
                        this.CutOffDate = new Date(queryFilterItem.FieldValue);
                        break; 
               
                           
                                      
            }
                  
        }
    }
    RunReport() {
       

        if (this.ValidateSelectedFilters()) {
           
            this.reportFliter = new ReportFliter();
            this.reportFliter.DateType = this.DateType;
            this.reportFliter.Tenant = SessionInfo.LoggedUserTenant;
            this.reportFliter.QueryFilterItemLists = this.GetQueryFilterItems();
            this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
            this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
            this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
            this.reportFliter.NumberOfPage = 1;
            this.reportFliter.ProcessType = "GenerateReport";

            this.ReportsPreview.GenerateReport(this.reportFliter, true);
        }
    }
    ValidateSelectedFilters(){
        this.ValidationErrorsList = [];
        
        if (this.FromDate == null) {
            this.ValidationErrorsList.push("From date is required");
        }

        if (this.ToDate == null) {
            this.ValidationErrorsList.push("To date is required");
        }
        return this.ValidationErrorsList.length == 0;
    }
    GetQueryFilterItems(){
        this.queryFilterItems = [];

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

        if (!AppTool.IsNullOrEmpty(this.FlightNumber)) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "FlightNumber";
            this.queryFilterItem.FieldValue = this.FlightNumber;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }

        if (this.MainCarriageFromPortId) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "MainCarriageFromPortId";
            this.queryFilterItem.FieldValue = this.MainCarriageFromPortId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }

        if (this.MainCarriageFinalDestinationPortId) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "MainCarriageFinalDestinationPortId";
            this.queryFilterItem.FieldValue = this.MainCarriageFinalDestinationPortId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }

        if (!AppTool.IsNullOrEmpty(this.ClearingAgentId)) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "ClearingAgentId";
            this.queryFilterItem.FieldValue = this.ClearingAgentId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }

        if (!AppTool.IsNullOrEmpty(this.ConsigneeId)) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "ConsigneeId";
            this.queryFilterItem.FieldValue = this.ConsigneeId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }

        if (!AppTool.IsNullOrEmpty(this.ShipperId)) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "ShipperId";
            this.queryFilterItem.FieldValue = this.ShipperId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }

        if (this.CutOffDate) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "CutOffDate";
            this.queryFilterItem.FieldValue = this.CutOffDate;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }
        return this.queryFilterItems;
    }
}
