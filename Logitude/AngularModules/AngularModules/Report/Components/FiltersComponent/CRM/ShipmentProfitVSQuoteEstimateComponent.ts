import {Component, OnInit, Output, EventEmitter}  from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ReportFliter} from '../../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../../Components/Filters/QueryFilterItem';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';

@Component({
    
    templateUrl: './ShipmentProfitVSQuoteEstimateComponent.html',
})

export class ShipmentProfitVSQuoteEstimateComponent extends BaseComponent implements OnInit {
    public DataContext = this;
    public LocalCurrencyCode: string;
    public ProfitCurrencyCode: string;
    public ValidationErrorsList: string[] = [];
    @Output() RunReportEvent: EventEmitter<ReportFliter> = new EventEmitter<ReportFliter>();
    constructor() {
        super();
        this.LocalCurrencyCode = SessionLocator.LocalCurrencyCode;
        this.ProfitCurrencyCode = SessionLocator.TenantPM.ProfitCurrencyCode;
        this.SelectedCurrencyCode = this.LocalCurrencyCode;
    }

    ngOnInit() {
        this.SetUIProperties();
    }

    SetUIProperties() {
        this.UIProperties.SetRequired("FromDate", null, AppTool.IsNullOrEmpty(this.FromDate) ? true : false);
    }

    // Filters
    private customerId: string = null;
    public get CustomerId() { return this.customerId; }
    public set CustomerId(value: string) {
        if (this.customerId != value) {
            this.customerId = value;
        }
    }

    private salesmanUserId: string = null;
    public get SalesmanUserId() { return this.salesmanUserId; }
    public set SalesmanUserId(value: string) {
        if (this.salesmanUserId != value) {
            this.salesmanUserId = value;
        }
    }

    private fromDate: Date = null;
    public get FromDate() { return this.fromDate; }
    public set FromDate(value: Date) {
        if (this.fromDate != value) {
            this.fromDate = value;
            this.SetUIProperties();
        }
    }

    private toDate: Date = null;
    public get ToDate() { return this.toDate; }
    public set ToDate(value: Date) {
        if (this.toDate != value) {
            this.toDate = value;
        }
    }

    private selectedCurrencyCode: string = null;
    public get SelectedCurrencyCode() { return this.selectedCurrencyCode; }
    public set SelectedCurrencyCode(value: string) {
        if (this.selectedCurrencyCode != value) {
            this.selectedCurrencyCode = value;
        }
    }

    private shipmentsTypeCode: string = "C";
    public get ShipmentsTypeCode() { return this.shipmentsTypeCode; }
    public set ShipmentsTypeCode(value: string) {
        if (this.shipmentsTypeCode != value) {
            this.shipmentsTypeCode = value;
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
                case "CustomerId":
                    this.CustomerId = queryFilterItem.FieldValue;
                    break;
                case "SalesmanUserId":
                    this.SalesmanUserId =queryFilterItem.FieldValue;
                    break;
                case "FromDate":
                    this.FromDate = new Date(queryFilterItem.FieldValue);
                    break;
                case "ToDate":
                    this.ToDate = new Date(queryFilterItem.FieldValue);
                    break;
                case "IsLocalCurrency":
                    this.SelectedCurrencyCode = queryFilterItem.FieldValue ?? this.LocalCurrencyCode;
                    break;
                case "ShipmentsTypeCode":
                    this.ShipmentsTypeCode = queryFilterItem.FieldValue;
                    break;
            }
   
        }
    }

    RunButtonClicked() {
        this.SetUIProperties();

       

        if (this.ValidateSelectedFilters()) {
           
            var myReportFliter: ReportFliter = new ReportFliter();
            myReportFliter.NumberOfPage = 1;
            myReportFliter.ProcessType = "GenerateReport";
            myReportFliter.QueryFilterItemLists = this.GetQueryFilterItems();
            this.RunReportEvent.emit(myReportFliter);
        }
    }
    GetQueryFilterItems(){
        var myFilterItems: QueryFilterItem[] = [];
        myFilterItems.push(new QueryFilterItem("CustomerId", this.CustomerId));
        myFilterItems.push(new QueryFilterItem("SalesmanUserId", this.SalesmanUserId));
        myFilterItems.push(new QueryFilterItem("FromDate", this.FromDate, "Date"));

        if (this.ToDate != null) {
            myFilterItems.push(new QueryFilterItem("ToDate", this.ToDate, "Date"));
        }

        myFilterItems.push(new QueryFilterItem("IsLocalCurrency", this.SelectedCurrencyCode == this.LocalCurrencyCode ? true : false));
        myFilterItems.push(new QueryFilterItem("ShipmentsTypeCode", this.ShipmentsTypeCode));
        return myFilterItems;
    }

    ValidateSelectedFilters(){
        var errors: string[] = [];

        if (AppTool.IsNullOrEmpty(this.FromDate)) {
            errors.push("From Date is required");
        }

        this.ValidationErrorsList = errors;
        return errors.length == 0;
    }
}
