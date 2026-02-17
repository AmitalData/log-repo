import { Component, Output, EventEmitter } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ReportFliter } from '../../../Components/Filters/ReportFliter';
import { QueryFilterItem } from '../../../Components/Filters/QueryFilterItem';
import { ReportsPreviewComponent } from '../../../Components/ReportsPreviewComponent';
import { CodeNameClass } from '../../../../Infrastructure/DataContracts/CodeNameClass';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';

@Component({

    templateUrl: './VendorChargesAnalysisFilterComponent.html',
})

export class VendorChargesAnalysisFilterComponent extends BaseComponent {
    public DataContext = this;
    public ValidationErrorsList: string[] = [];
    @Output() RunReportEvent: EventEmitter<ReportFliter> = new EventEmitter<ReportFliter>();
    public ReportsPreview: ReportsPreviewComponent;
    constructor() {
        super();
    }

    public VendorId: string;
    public ChargesTypeId: string;
    public FromDate: Date;
    public ToDate: Date;
    public ProfitCurrencyCode: string;
    public LocalCurrencyCode: string;
    public IncludeAccountedOnly: boolean = false;
    public ShipmentNumber: string;

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;

        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = this.DaysInMonth(new Date());
        this.FromDate = this.SetDate(Year, month - 1, 1);
        this.ToDate = this.SetDate(Year, month - 1, daysofmonth);

        this.ProfitCurrencyCode = SessionLocator.TenantPM.ProfitCurrencyCode;
        this.LocalCurrencyCode = SessionLocator.TenantPM.AccountingCurrencyCode;
        this.SelectedCurrencyCode = this.LocalCurrencyCode;

        this.SelectedOperationalCode = "All";
        this.SelectedAccountingCode = "All";

        this.BuildDateFilter();
    }

    private DaysInMonth(aDate: Date) {
        return (new Date(aDate.getFullYear(), aDate.getMonth(), 0)).getDate();
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

    public DateFilterList: CodeNameClass[];
    private BuildDateFilter(code: string = "CRT") {
        this.DateFilterList = [];
        this.DateFilterList.push(new CodeNameClass("CRT", "Create Date"));
        this.DateFilterList.push(new CodeNameClass("OPE", "Operational Date"));

        this.selectedDateFilter = this.DateFilterList.filter(d => d.Code == code)[0];
    }

    private selectedDateFilter: CodeNameClass;
    get SelectedDateFilter() { return this.selectedDateFilter; }
    set SelectedDateFilter(value: CodeNameClass) {
        if (this.selectedDateFilter != value) {
            this.selectedDateFilter = value;
        }
    }

    private mySelectedTransportFilter: string = "All";
    get SelectedTransportFilter() { return this.mySelectedTransportFilter; }
    set SelectedTransportFilter(value: string) {
        if (this.mySelectedTransportFilter != value) {
            this.mySelectedTransportFilter = value;
        }
    }

    private mySelectedDirectionFilter: string = "All";
    get SelectedDirectionFilter() { return this.mySelectedDirectionFilter; }
    set SelectedDirectionFilter(value: string) {
        if (this.mySelectedDirectionFilter != value) {
            this.mySelectedDirectionFilter = value;
        }
    }

    public SelectedCurrencyCode: string = null;
    OnSelectCurrency(myCurrencyCode: string) {
        this.SelectedCurrencyCode = myCurrencyCode;
    }

    private selectedOperationalCode: string = "All";
    public get SelectedOperationalCode() { return this.selectedOperationalCode; }
    public set SelectedOperationalCode(value: string) {
        if (this.selectedOperationalCode != value) {
            this.selectedOperationalCode = value;
            this.ApplySelectedStyle("OPE");
        }
    }

    OnOperationalClicked(myCode: string) {
        this.SelectedOperationalCode = myCode;
    }

    private selectedAccountingCode: string = "All";
    public get SelectedAccountingCode() { return this.selectedAccountingCode; }
    public set SelectedAccountingCode(value: string) {
        if (this.selectedAccountingCode != value) {
            this.selectedAccountingCode = value;
            this.ApplySelectedStyle("ACC");
        }
    }

    OnAccountingClicked(myCode: string) {
        this.SelectedAccountingCode = myCode;
    }

    public FilterId_OO: string = "operational_open";
    public FilterId_OC: string = "operational_close";
    OperationalMouseOver(itemValue: string) {
        if (this.SelectedOperationalCode != itemValue) {
            var img_O = document.getElementById(this.FilterId_OO);
            var img_C = document.getElementById(this.FilterId_OC);

            switch (itemValue) {
                case "All": {

                    break;
                }

                case "Open": {
                    img_O.setAttribute("src", "./Images/Icons/opened_h.png");
                    break;
                }

                case "Close": {
                    img_C.setAttribute("src", "./Images/Icons/closed_h.png");
                    break;
                }
            }
        }
    }
    OperationalMouseLeave(itemValue: string) {
        if (this.SelectedOperationalCode != itemValue) {
            var img_O = document.getElementById(this.FilterId_OO);
            var img_C = document.getElementById(this.FilterId_OC);

            switch (itemValue) {
                case "All": {

                    break;
                }

                case "Open": {
                    img_O.setAttribute("src", "./Images/Icons/opened_n.png");
                    break;
                }

                case "Close": {
                    img_C.setAttribute("src", "./Images/Icons/closed_n.png");
                    break;
                }
            }
        }
    }

    public FilterId_AO: string = "accounting_open";
    public FilterId_AC: string = "accounting_close";
    AccountingMouseOver(itemValue: string) {
        if (this.SelectedAccountingCode != itemValue) {
            var img_O = document.getElementById(this.FilterId_AO);
            var img_C = document.getElementById(this.FilterId_AC);

            switch (itemValue) {
                case "All": {

                    break;
                }

                case "Open": {
                    img_O.setAttribute("src", "./Images/Icons/opened_h.png");
                    break;
                }

                case "Close": {
                    img_C.setAttribute("src", "./Images/Icons/closed_h.png");
                    break;
                }
            }
        }
    }
    AccountingMouseLeave(itemValue: string) {
        if (this.SelectedAccountingCode != itemValue) {
            var img_O = document.getElementById(this.FilterId_AO);
            var img_C = document.getElementById(this.FilterId_AC);

            switch (itemValue) {
                case "All": {

                    break;
                }

                case "Open": {
                    img_O.setAttribute("src", "./Images/Icons/opened_n.png");
                    break;
                }

                case "Close": {
                    img_C.setAttribute("src", "./Images/Icons/closed_n.png");
                    break;
                }
            }
        }
    }

    ApplySelectedStyle(mode: string) {
        var img_O;
        var img_C;

        switch (mode) {
            case "OPE": {
                img_O = document.getElementById(this.FilterId_OO);
                img_C = document.getElementById(this.FilterId_OC);

                img_O.setAttribute("src", "./Images/Icons/opened_n.png");
                img_C.setAttribute("src", "./Images/Icons/closed_n.png");

                switch (this.SelectedOperationalCode) {
                    case "All": {

                        break;
                    }

                    case "Open": {
                        img_O.setAttribute("src", "./Images/Icons/opened_s.png");
                        break;
                    }

                    case "Close": {
                        img_C.setAttribute("src", "./Images/Icons/closed_s.png");
                        break;
                    }
                }

                break;
            }

            case "ACC": {
                img_O = document.getElementById(this.FilterId_AO);
                img_C = document.getElementById(this.FilterId_AC);

                img_O.setAttribute("src", "./Images/Icons/opened_n.png");
                img_C.setAttribute("src", "./Images/Icons/closed_n.png");

                switch (this.SelectedAccountingCode) {
                    case "All": {

                        break;
                    }

                    case "Open": {
                        img_O.setAttribute("src", "./Images/Icons/opened_s.png");
                        break;
                    }

                    case "Close": {
                        img_C.setAttribute("src", "./Images/Icons/closed_s.png");
                        break;
                    }
                }
                break;
            }
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
                case "VendorId":
                    this.VendorId = queryFilterItem.FieldValue;
                    break;
                case "ChargesTypeId":
                    this.ChargesTypeId = queryFilterItem.FieldValue;
                    break;
                case "AccountingType":
                    this.SelectedAccountingCode = queryFilterItem.FieldValue;
                    break;
                case "Direction":
                    this.SelectedDirectionFilter = queryFilterItem.FieldValue;
                    break;
                case "TransportMode":
                    this.SelectedTransportFilter = queryFilterItem.FieldValue;
                    break;
                case "ShipmentNumber":
                    this.ShipmentNumber = queryFilterItem.FieldValue;
                    break;
                case "DateType":
                    this.BuildDateFilter(queryFilterItem.FieldValue);
                    break;
                case "OperationalType":
                    this.SelectedOperationalCode = queryFilterItem.FieldValue ?? this.LocalCurrencyCode;
                    break;
                case "SelectedCurrencyCode":
                    this.SelectedCurrencyCode=queryFilterItem.FieldValue;
                    break;
                


            }



        }
    }
    RunButtonClicked() {


        if (this.ValidateSelectedFilters()) {

            var myReportFliter: ReportFliter = new ReportFliter();
            myReportFliter.NumberOfPage = 1;
            myReportFliter.ProcessType = "GenerateReport";
            myReportFliter.QueryFilterItemLists = this.GetQueryFilterItems();
            this.RunReportEvent.emit(myReportFliter);
        }
    }
    GetQueryFilterItems() {
        var myFilterItems: QueryFilterItem[] = [];
        myFilterItems.push(new QueryFilterItem("VendorId", this.VendorId));
        myFilterItems.push(new QueryFilterItem("DateType", this.SelectedDateFilter.Code));
        myFilterItems.push(new QueryFilterItem("FromDate", this.FromDate));
        myFilterItems.push(new QueryFilterItem("ToDate", this.ToDate));
        myFilterItems.push(new QueryFilterItem("ChargesTypeId", this.ChargesTypeId));
        myFilterItems.push(new QueryFilterItem("IncludeAccountedOnly", this.IncludeAccountedOnly));
        myFilterItems.push(new QueryFilterItem("IsLocalCurrency", this.SelectedCurrencyCode == this.LocalCurrencyCode ? true : false));
        myFilterItems.push(new QueryFilterItem("SelectedCurrencyCode", this.SelectedCurrencyCode));
        myFilterItems.push(new QueryFilterItem("OperationalType", this.SelectedOperationalCode));
        myFilterItems.push(new QueryFilterItem("AccountingType", this.SelectedAccountingCode));
        myFilterItems.push(new QueryFilterItem("Direction", this.SelectedDirectionFilter));
        myFilterItems.push(new QueryFilterItem("TransportMode", this.SelectedTransportFilter));
        myFilterItems.push(new QueryFilterItem("ShipmentNumber", this.ShipmentNumber));
        return myFilterItems;
    }
    ValidateSelectedFilters() {
        var errors: string[] = [];

        if (!this.SelectedDateFilter) {
            errors.push("Date field is required");
        }

        if (this.FromDate != null && this.ToDate != null) {
            if (this.ToDate < this.FromDate) {
                errors.push("From date must be less than to date");
            }
        }

        else {
            if (this.FromDate == null) {
                errors.push("From Date is required");
            }

            if (this.ToDate == null) {
                errors.push("To Date is required");
            }
        }
        return errors.length === 0;
    }
}
