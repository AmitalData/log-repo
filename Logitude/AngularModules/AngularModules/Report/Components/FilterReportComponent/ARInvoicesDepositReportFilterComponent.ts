import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReportsPreviewComponent} from '../../Components/ReportsPreviewComponent';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ReportFliter} from '../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { Component, OnInit } from '@angular/core';
import {FormBuilder, FormGroup, FormsModule} from '@angular/forms';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {ParticipantList} from '../../EntityLists/ParticipantList';
import {AppTool} from '../../../Infrastructure/Tools';
import {CodeNameClass} from './CodeNameClass';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';


@Component({
    
    selector: 'ARInvoicesDepositReportFilterComponent',
    templateUrl: './ARInvoicesDepositReportFilterComponent.html',
    inputs: ['ReportsPreview']
})

export class ARInvoicesDepositReportFilterComponent extends BaseComponent implements OnInit {
    public ReportsPreview: ReportsPreviewComponent;

    public ValidationErrorsList: string[];
    public LocalCurrency: string = null;
    public InvoiceCurrency: string;
    public IncludeVoidInvoices: boolean = false;
    public IncludeWaiting: boolean = false;
    reportFliter: ReportFliter;
    ToDate: Date;
    FromDate: Date;

    public myForm: FormGroup;
    queryFilterItems: QueryFilterItem[];
    queryFilterItem: QueryFilterItem;
    public ObjectTableName: string = "Report";


    settingShipmentTypeCode(code) {
        this.ShipmentTypeRadio = code;

    }

    public shipmentTypeRadio: string = "InvoiceDate";
    public set ShipmentTypeRadio(code) {

        this.shipmentTypeRadio = code;

    }
    public get ShipmentTypeRadio() {
        return this.shipmentTypeRadio;
    }


    public IsInvoiceDate: boolean = true;
    public IsCreateDate: boolean = false;
    public invoiceType: string;

    public IsLocalCurrency: boolean = true;

    public BranchId: string = null;
    public CustomerId: string = null;
    setLocalCurrency(code) {
        if (code == "true")
            this.IsLocalCurrency = true;
        else if (code == "false")
            this.IsLocalCurrency = false;

        this.CurrencyId = null;
        this.SetUIProperties();
    }

    public Currency() {


    }


    public get InvoiceType() {

        return this.ShipmentTypeRadio;


    }

    IsInvoiceDateClicked() {

        this.IsInvoiceDate = true;
        this.IsCreateDate = false;

    }
    IsCreateDateClicked() {
        this.IsInvoiceDate = false;
        this.IsCreateDate = true;
    }



    public DataContext: ARInvoicesDepositReportFilterComponent = this;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

        if (this.CurrentSession == null) {
            this.LocalCurrency = "Local_-1_-1";
            this.InvoiceCurrency = "Invoice_-1_-1";
        }

        else {
            var idIndex = this.CurrentSession.GetNewId("RadioButton");

            this.LocalCurrency = "Local_" + idIndex;
            this.InvoiceCurrency = "Invoice_" + idIndex;

        }


    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;

        this.IsLocalCurrency = true;
        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = this.daysInMonth(new Date());

        this.FromDate = this.SetDate(Year, month - 1, 1);
        this.ToDate = this.SetDate(Year, month, daysofmonth);
    }










    daysInMonth(aDate: Date) {
        return (new Date(aDate.getFullYear(), aDate.getMonth() + 1, 0)).getDate();
    }

    ngOnInit() {
        this.SetUIProperties();
    }

    SetUIProperties() {
        this.UIProperties.SetEnabled("CurrencyId", null, !this.IsLocalCurrency);
    }

    private currencyId: string;
    public get CurrencyId() { return this.currencyId; }
    public set CurrencyId(value: string) {
        if (this.currencyId != value) {
            this.currencyId = value;
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
                case "BranchId":
                    this.BranchId = queryFilterItem.FieldValue;
                    break;
                case "CustomerId":
                    this.CustomerId = queryFilterItem.FieldValue;
                    break;
                case "LocalCurrency":
                    this.IsLocalCurrency = queryFilterItem.FieldValue;
                    break;
                case "CurrencyId":
                    this.CurrencyId=queryFilterItem.FieldValue;
                    break;
               
            }



        }
    }

    RunReport(isloading: boolean) {


       

        if (this.ValidateSelectedFilters()){


         
            this.reportFliter = new ReportFliter();
            this.reportFliter.Tenant = SessionInfo.LoggedUserTenant;
            this.reportFliter.QueryFilterItemLists = this.GetQueryFilterItems();
            this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;

            this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
            this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
            this.reportFliter.NumberOfPage = 1;
            this.reportFliter.ProcessType = "GenerateReport";

            this.ReportsPreview.CleanPartnersObslist();
            if (!AppTool.IsNullOrEmpty(this.CustomerId)) this.ReportsPreview.AddPartner("Customer", this.CustomerId);


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
        this.queryFilterItem.FieldName = "BranchId";
        this.queryFilterItem.FieldValue = this.BranchId;
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);

        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "CustomerId";
        this.queryFilterItem.FieldValue = this.CustomerId;
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);


        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "LocalCurrency";
        this.queryFilterItem.FieldValue = this.IsLocalCurrency;
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);

        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "CurrencyId";
        this.queryFilterItem.FieldValue = this.CurrencyId;
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);
        return this.queryFilterItems;
    }
    ValidateSelectedFilters(){
        this.ValidationErrorsList = [];
        if (this.FromDate == null ) {

            this.ValidationErrorsList.push("From Date is Required");

        }

        if (this.ToDate == null) {

            this.ValidationErrorsList.push("To Date is Required");

        }
        return this.ValidationErrorsList.length == 0;
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
