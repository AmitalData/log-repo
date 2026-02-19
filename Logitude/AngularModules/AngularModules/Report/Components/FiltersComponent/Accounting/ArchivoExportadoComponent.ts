import {Component, OnInit, Output, EventEmitter}  from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ReportFliter} from '../../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../../Components/Filters/QueryFilterItem';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';

@Component({
    
    templateUrl: './ArchivoExportadoComponent.html',
})

export class ArchivoExportadoComponent extends BaseComponent implements OnInit {
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

    private includeDraftInvoices: boolean = false;
    public get IncludeDraftInvoices() { return this.includeDraftInvoices; }
    public set IncludeDraftInvoices(value: boolean) {
        if (this.includeDraftInvoices != value) {
            this.includeDraftInvoices = value;
        }
    }

    private includeEstimations: boolean = false;
    public get IncludeEstimations() { return this.includeEstimations; }
    public set IncludeEstimations(value: boolean) {
        if (this.includeEstimations != value) {
            this.includeEstimations = value;
        }
    }

    private splitByCharges: boolean = false;
    public get SplitByCharges() { return this.splitByCharges; }
    public set SplitByCharges(value: boolean) {
        if (this.splitByCharges != value) {
            this.splitByCharges = value;
        }
    }
    public IsSchedulerReport: boolean = false;
    SetQueryFilterItems(queryFilterItems: Array<QueryFilterItem>, isSchedulerReport: boolean = true) {
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
                case "IncludeEstimations":
                    this.IncludeEstimations = queryFilterItem.FieldValue;
                    break;
                case "IncludeDraftInvoices":
                    this.IncludeDraftInvoices = queryFilterItem.FieldValue;
                    break;
                case "SelectedCurrencyCode":
                    this.SelectedCurrencyCode=queryFilterItem.FieldValue;
                    break;
               case "IsLocalCurrency":
                   this.SelectedCurrencyCode = queryFilterItem.FieldValue ?? this.LocalCurrencyCode ;
                     break;
            }



        }
    }
    ValidateSelectedFilters(){
        var errors: string[] = [];

        if (AppTool.IsNullOrEmpty(this.FromDate)) {
            errors.push("From Date is required");
        }

        this.ValidationErrorsList = errors;
        return errors.length == 0;
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
        myFilterItems.push(new QueryFilterItem("FromDate", this.FromDate, "Date"));
        myFilterItems.push(new QueryFilterItem("ToDate", this.ToDate, "Date"));
        myFilterItems.push(new QueryFilterItem("IsLocalCurrency", this.SelectedCurrencyCode == this.LocalCurrencyCode ? true : false));
        myFilterItems.push(new QueryFilterItem("SelectedCurrencyCode", this.SelectedCurrencyCode));
        myFilterItems.push(new QueryFilterItem("IncludeDraftInvoices", this.IncludeDraftInvoices));
        myFilterItems.push(new QueryFilterItem("IncludeEstimations", this.IncludeEstimations));
        myFilterItems.push(new QueryFilterItem("SplitByCharges", this.SplitByCharges));
        return myFilterItems;

    }
}
