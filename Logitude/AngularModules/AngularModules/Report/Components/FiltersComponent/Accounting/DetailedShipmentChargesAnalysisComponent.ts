import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AppTool } from '../../../../Infrastructure/Tools';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ReportFliter } from '../../../Components/Filters/ReportFliter';
import { QueryFilterItem } from '../../../Components/Filters/QueryFilterItem';
import { CodeNameClass } from '../../../../Infrastructure/DataContracts/CodeNameClass';

@Component({
    moduleId: module.id,
    templateUrl: './DetailedShipmentChargesAnalysisComponent.html',
})

export class DetailedShipmentChargesAnalysisComponent extends BaseComponent implements OnInit {
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
        this.BuildDateFilter();
    }

    SetUIProperties() {
        this.UIProperties.SetRequired("FromDate", null, AppTool.IsNullOrEmpty(this.FromDate) ? true : false);
    }

    public DateFilterList: CodeNameClass[];
    private BuildDateFilter() {
        this.DateFilterList = [];

        this.DateFilterList.push(new CodeNameClass("OPE", "Operational Date"));
        this.DateFilterList.push(new CodeNameClass("CRT", "Create Date"));
        this.DateFilterList.push(new CodeNameClass("REG", "Registry Date"));

        this.selectedDateFilter = this.DateFilterList.filter(d => d.Code == "OPE")[0];
    }

    private selectedDateFilter: CodeNameClass;
    get SelectedDateFilter() { return this.selectedDateFilter; }
    set SelectedDateFilter(value: CodeNameClass) {
        if (this.selectedDateFilter != value) {
            this.selectedDateFilter = value;
        }
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

    private includeCancelledShipments: boolean = false;
    public get IncludeCancelledShipments() { return this.includeCancelledShipments; }
    public set IncludeCancelledShipments(value: boolean) {
        if (this.includeCancelledShipments != value) {
            this.includeCancelledShipments = value;
        }
    }

    private splitByCharges: boolean = false;
    public get SplitByCharges() { return this.splitByCharges; }
    public set SplitByCharges(value: boolean) {
        if (this.splitByCharges != value) {
            this.splitByCharges = value;
        }
    }

    RunButtonClicked() {
        this.SetUIProperties();

        var errors: string[] = [];

        if (AppTool.IsNullOrEmpty(this.FromDate)) {
            errors.push("From Date is required");
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            var myFilterItems: QueryFilterItem[] = [];
            myFilterItems.push(new QueryFilterItem("SelectedDateType", this.SelectedDateFilter.Code));
            myFilterItems.push(new QueryFilterItem("FromDate", this.FromDate, "Date"));
            myFilterItems.push(new QueryFilterItem("ToDate", this.ToDate, "Date"));
            myFilterItems.push(new QueryFilterItem("IsLocalCurrency", this.SelectedCurrencyCode == this.LocalCurrencyCode ? true : false));
            myFilterItems.push(new QueryFilterItem("SelectedCurrencyCode", this.SelectedCurrencyCode));
            myFilterItems.push(new QueryFilterItem("IncludeDraftInvoices", this.IncludeDraftInvoices));
            myFilterItems.push(new QueryFilterItem("IncludeEstimations", this.IncludeEstimations));
            myFilterItems.push(new QueryFilterItem("SplitByCharges", this.SplitByCharges));
            myFilterItems.push(new QueryFilterItem("IncludeCancelledShipments", this.IncludeCancelledShipments));

            var myReportFliter: ReportFliter = new ReportFliter();
            myReportFliter.NumberOfPage = 1;
            myReportFliter.ProcessType = "GenerateReport";
            myReportFliter.QueryFilterItemLists = myFilterItems;
            this.RunReportEvent.emit(myReportFliter);
        }
    }
}
