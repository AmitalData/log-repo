import { Component } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ReportFliter } from '../../../Components/Filters/ReportFliter';
import { QueryFilterItem } from '../../../Components/Filters/QueryFilterItem';
import { ReportsPreviewComponent } from '../../../Components/ReportsPreviewComponent';
import { DateTool, AppTool } from '../../../../Infrastructure/Tools';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';

@Component({
    templateUrl: './ShipperReturnsReportFilterComponent.html',
})

export class ShipperReturnsReportFilterComponent extends BaseComponent {

    public ReportsPreview: ReportsPreviewComponent;
    public ValidationErrorsList: string[];
    public ObjectTableName: string = "Report";
    public DataContext: ShipperReturnsReportFilterComponent = this;

    constructor() {
        super();
    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;
        this.FromDate = DateTool.GetCurrentDateAsUtc();
        this.FromDate.setMonth(this.FromDate.getMonth() - 1);
        this.ToDate = DateTool.GetCurrentDateAsUtc();
    }

    public ShipperId: string;
    public FromDate: Date;
    public ToDate: Date;
    public MainCarriageFromPortId: string;
    public MainCarriageFinalDestinationPortId: string;
    public Subshipper: string;
    public CardDependencyProperty1IsList: boolean;
    get CardDependencyProperty1() {
        this.CardDependencyProperty1IsList = false;
        var myResult: string = "CS";
        if (SessionLocator.TenantPM.AllowAgentInCustomersLOV) {
            myResult = "CS,AG";
            this.CardDependencyProperty1IsList = true;
        }
        return myResult;
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
                case "ShipperId":
                    this.ShipperId= queryFilterItem.FieldValue;
                    break;
                case "FromDate":
                    this.FromDate = new Date(queryFilterItem.FieldValue);
                    break;
                case "ToDate":
                    this.ToDate = new Date(queryFilterItem.FieldValue);
                     break;
                case "Subshipper":
                    this.Subshipper = queryFilterItem.FieldValue;
                    break;
                case "MainCarriageFromPortId":
                    this.MainCarriageFromPortId = queryFilterItem.FieldValue;
                    break;
                case "MainCarriageFinalDestinationPortId":
                    this.MainCarriageFinalDestinationPortId = queryFilterItem.FieldValue;
                    break;
               
                           
                                      
            }
                  
        }
    }
    queryFilterItems: QueryFilterItem[];
    queryFilterItem: QueryFilterItem;
    RunReport(isloading: boolean) {
        
        if (this.ValidateSelectedFilters()) {
           
            var reportFliter = new ReportFliter();
            reportFliter.Tenant = SessionLocator.Tenant;
            reportFliter.QueryFilterItemLists = this.GetQueryFilterItems();
            reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
            reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
            reportFliter.ReportCode = this.ReportsPreview.Report.Code;
            reportFliter.NumberOfPage = 1;
            reportFliter.ProcessType = "GenerateReport";

            this.ReportsPreview.GenerateReport(reportFliter, isloading);
        }
    }
    ValidateSelectedFilters(){
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
        return this.ValidationErrorsList.length == 0;
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
        this.queryFilterItem.FieldName = "ShipperId";
        this.queryFilterItem.FieldValue = this.ShipperId;
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);

        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "Subshipper";
        this.queryFilterItem.FieldValue = this.Subshipper;
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);

        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "MainCarriageFromPortId";
        this.queryFilterItem.FieldValue = this.MainCarriageFromPortId;
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);

        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "MainCarriageFinalDestinationPortId";
        this.queryFilterItem.FieldValue = this.MainCarriageFinalDestinationPortId;
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);
        return this.queryFilterItems;
    }
}
