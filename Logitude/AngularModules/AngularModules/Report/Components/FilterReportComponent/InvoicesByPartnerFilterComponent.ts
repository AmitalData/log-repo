/// <reference path="codenameclass.ts" />


declare var System: any;
declare var window: any;
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
    selector: 'InvoicesByPartnerFilterComponent',
    templateUrl: './InvoicesByPartnerFilterComponent.html',
    inputs: ['ReportsPreview']
})

export class InvoicesByPartnerFilterComponent extends BaseComponent   {
    public ReportsPreview: ReportsPreviewComponent;

    public ProfitCurrencyCode: string = SessionLocator.TenantPM.ProfitCurrencyCode;
    public LocalCurrencyCode: string = SessionLocator.TenantPM.AccountingCurrencyCode;
    public ValidationErrorsList: string[] = [];
    public IncludeClosed: boolean = false;
    reportFliter: ReportFliter;
    ToDate: Date;
    FromDate: Date;
    public selectedCurrency: string = this.LocalCurrencyCode;
    public get SelectedCurrency() {

        return this.selectedCurrency;
    }

    public set SelectedCurrency(value: string) {
        this.selectedCurrency = value;

    }
    queryFilterItems: QueryFilterItem[];
    public CustomerId = null;
    queryFilterItem: QueryFilterItem;
    public ObjectTableName: string = "Report";


    public IsInvoiceDate: boolean = true;
    public IsCreateDate: boolean = false;
    public invoiceType: string;

    public get InvoiceType() {

        return this.ShipmentTypeRadio;


    }


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

    IsInvoiceDateClicked() {

        this.IsInvoiceDate = true;
        this.IsCreateDate = false;

    }
    IsCreateDateClicked() {
        this.IsInvoiceDate = false;
        this.IsCreateDate = true;
    }



    public DataContext: InvoicesByPartnerFilterComponent = this;
    constructor() {
        super();
    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;

        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = this.daysInMonth(new Date());


        this.FromDate = this.SetDate(Year, month - 1, 1);


        this.ToDate = this.SetDate(Year, month - 1, daysofmonth);
    }
 
    daysInMonth(aDate: Date) {
        return (new Date(aDate.getFullYear(), aDate.getMonth() , 0)).getDate();
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
            this.queryFilterItem.FieldName = "CreateDate";
            this.queryFilterItem.FieldValue = this.FromDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItem.Operator = "GreaterThanOrEqual";
            this.queryFilterItems.push(this.queryFilterItem);

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "CreateDate";
            this.queryFilterItem.FieldValue = this.ToDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItem.Operator = "LessThanOrEqual";
            this.queryFilterItems.push(this.queryFilterItem);



            if (this.CustomerId) {
                this.queryFilterItem = new QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "BillToId";
                this.queryFilterItem.FieldValue = this.CustomerId;
                this.queryFilterItem.Operator = "Equals";
                this.queryFilterItems.push(this.queryFilterItem);
            }






            this.reportFliter = new ReportFliter();
            this.reportFliter.Tenant = SessionInfo.LoggedUserTenant;
            this.reportFliter.QueryFilterItemLists = this.queryFilterItems;
            this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
            if (this.SelectedCurrency == this.LocalCurrencyCode)
                this.reportFliter.CurrentCurrencyCodeType = this.LocalCurrencyCode + ",local";
            else
                this.reportFliter.CurrentCurrencyCodeType = this.ProfitCurrencyCode + ",profit";

            this.reportFliter.DateType = this.InvoiceType;
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