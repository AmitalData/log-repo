import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReportsPreviewComponent} from '../../Components/ReportsPreviewComponent';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ReportFliter} from '../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Component, OnInit, Output, ElementRef}  from '@angular/core';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {ParticipantList} from '../../EntityLists/ParticipantList';
import {AppTool} from '../../../Infrastructure/Tools';
import {CodeNameClass} from './CodeNameClass';


@Component({
    moduleId: module.id,
    selector: 'InvoicesFilterComponent',
    templateUrl: './InvoicesFilterComponent.html',
    inputs: ['ReportsPreview']
})

export class InvoicesFilterComponent extends BaseComponent   {
    public ReportsPreview: ReportsPreviewComponent;
    public ValidationErrorsList: string[] = [];

    public IncludeVoidInvoices: boolean = false;
    public IncludeWaiting: boolean = false;
    reportFliter: ReportFliter;
    ToDate: Date;
    FromDate: Date;

    queryFilterItems: QueryFilterItem[];
    public CustomerId = null;
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

    ChangeCurrency(value) {
        this.IsLocalCurrency = value;

    }

    public IsInvoiceDate: boolean = true;
    public IsCreateDate: boolean = false;
    public invoiceType: string;

    public IsLocalCurrency: boolean = true;
    public IsInvoiceCurrency: boolean = false;

    public BranchId: string = null;

    public IncludeStatusString: string = "";
    public IsLocalCurrencyClicked() {

        this.IsLocalCurrency = true;
        this.IsInvoiceCurrency = false;

    }
    public IsInvoiceCurrencyClicked() {


        this.IsLocalCurrency = false;
        this.IsInvoiceCurrency = true;
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



    public DataContext: InvoicesFilterComponent = this;
    public LocalCurrency: string;
    public InvoiceCurrency: string;
    public InvoiceDate: string;
    public CreateDate: string;
    public InvoiceTypeRadio: string;
    public LocalCurencyRadio: string;
    
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.LocalCurrency = "LocalCurrency_" + this.CurrentSession.SessionIndex +this.CurrentSession.GetNewId("LocalCurrency");
        this.InvoiceCurrency = "InvoiceCurrency_" + this.CurrentSession.SessionIndex +this.CurrentSession.GetNewId("InvoiceCurrency");
        this.InvoiceDate = "InvoiceDate_" + this.CurrentSession.SessionIndex+ this.CurrentSession.GetNewId("InvoiceDate");
        this.CreateDate = "CreateDate_" + this.CurrentSession.SessionIndex +this.CurrentSession.GetNewId("CreateDate");
        this.InvoiceTypeRadio = "InvoiceTypeRadio_" + this.CurrentSession.SessionIndex + this.CurrentSession.GetNewId("InvoiceTypeRadio");
        this.LocalCurencyRadio = "LocalCurencyRadio_" + this.CurrentSession.SessionIndex + this.CurrentSession.GetNewId("LocalCurencyRadio");
    }


    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;

        this.IsLocalCurrency = true;
        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = this.daysInMonth(new Date());


        this.FromDate = this.SetDate(Year, month - 1, 1);


        this.ToDate = this.SetDate(Year, month, daysofmonth);


        if (this.ReportsPreview.Report.Code == "RAPI") {
            this.IncludeStatusString = "Include Waiting For Approval Invoices";
        }
        else if (this.ReportsPreview.Report.Code == "RINV") {
            this.IncludeStatusString = "Include Draft Invoices";
        }
    }

    daysInMonth(aDate: Date) {
        return (new Date(aDate.getFullYear(), aDate.getMonth() + 1, 0)).getDate();
    }
    RunReport(isloading: boolean) {

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

        if (this.ValidationErrorsList.length == 0) {
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
            this.queryFilterItem.FieldName = "LocalCurrency";
            this.queryFilterItem.FieldValue = this.IsLocalCurrency;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);


            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "IncludeVoidInvoices";
            this.queryFilterItem.FieldValue = this.IncludeVoidInvoices;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "IncludeDraftInvoices";
            this.queryFilterItem.FieldValue = this.IncludeWaiting;
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
            if (!AppTool.IsNullOrEmpty(this.CustomerId)) this.ReportsPreview.AddPartner("Partner", this.CustomerId);


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
