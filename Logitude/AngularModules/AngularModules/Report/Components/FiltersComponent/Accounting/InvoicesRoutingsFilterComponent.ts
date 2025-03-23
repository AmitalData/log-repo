import {Component, OnInit, Output, EventEmitter}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ReportFliter} from '../../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../../Components/Filters/QueryFilterItem';
import {CodeNameClass} from '../../../../Infrastructure/DataContracts/CodeNameClass';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
@Component({
    
    selector: 'InvoicesRoutingsFilterComponent',
    templateUrl: './InvoicesRoutingsFilterComponent.html',
})

export class InvoicesRoutingsFilterComponent extends BaseComponent  {
    public IncludeVoidInvoices: boolean = false;
    public IncludeWaiting: boolean = false;
    public InvoiceStatusComboList: CodeNameClass[] = [];
    public InvoiceStatusSelectedItem: CodeNameClass;
    public ValidationErrorsList: string[];
    reportFliter: ReportFliter;
    ToDate: Date;
    FromDate: Date;
    @Output() RunReportEvent: EventEmitter<ReportFliter> = new EventEmitter<ReportFliter>();
    queryFilterItems: QueryFilterItem[];
    queryFilterItem: QueryFilterItem;
    public ObjectTableName: string = "Report";
    public InvoiceDateId: string;
    public CreateDateId: string;
    public ShipmentTypeRadioName: string;
    public LocalCurrencyId: string;
    public LocalCurencyRadio: string;
    public InvoiceCurrencyId: string;
    public shipmentTypeRadio: string;
    public CustomerId: string = null;
    public PartnerId: string = null;

    InitilizeIds() {
        this.InvoiceDateId = "InvoiceDateId_" + this.CurrentSession.GetNewId("InvoiceDateId");        
        this.CreateDateId = "CreateDateId_" + this.CurrentSession.GetNewId("CreateDateId");        
        this.ShipmentTypeRadioName = "ShipmentTypeRadioName_" + this.CurrentSession.GetNewId("ShipmentTypeRadioName");        
        this.LocalCurrencyId = "LocalCurrencyId_" + this.CurrentSession.GetNewId("LocalCurrencyId");        
        this.LocalCurencyRadio = "LocalCurencyRadio_" + this.CurrentSession.GetNewId("LocalCurencyRadio");        
        this.InvoiceCurrencyId = "InvoiceCurrencyId_" + this.CurrentSession.GetNewId("InvoiceCurrencyId");        
        this.ShipmentTypeRadio = "InvoiceDate";
        
    }

    private mySelectedTransportFilter: string = "All";
    public get MySelectedTransportFilter() { return this.mySelectedTransportFilter; }
    public set MySelectedTransportFilter(newValue: string) {
        if (this.mySelectedTransportFilter != newValue) {
            this.mySelectedTransportFilter = newValue;
        }

    }


    private mySelectedDirectionFilter: string = "All";
    public get MySelectedDirectionFilter() { return this.mySelectedDirectionFilter; }
    public set MySelectedDirectionFilter(newValue: string) {
        if (this.mySelectedDirectionFilter != newValue) {
            this.mySelectedDirectionFilter = newValue;
        }
    }

    InvoiceStatusChange(event) {
        this.InvoiceStatusSelectedItem = event;
    }

    settingShipmentTypeCode(code) {
        this.ShipmentTypeRadio = code;
    }

    public set ShipmentTypeRadio(code) {

        this.shipmentTypeRadio = code;

    }
    public get ShipmentTypeRadio() {
        return this.shipmentTypeRadio;
    }

    ChangeCurrency(value) {
        this.IsLocalCurrency = value;
    }


    FillInvoiceStatus(code : string = "") {
        this.InvoiceStatusComboList.push(new CodeNameClass("", "All"));
        this.InvoiceStatusComboList.push(new CodeNameClass("PD", "Paid"));
        this.InvoiceStatusComboList.push(new CodeNameClass("AD", "Unpaid"));
        this.InvoiceStatusSelectedItem = this.InvoiceStatusComboList.filter(d => d.Code == code)[0];
    }

    public IsInvoiceDate: boolean = true;
    public IsCreateDate: boolean = false;
    public invoiceType: string;

    public IsLocalCurrency: boolean = true;
    public IsInvoiceCurrency: boolean = false;


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



    public DataContext: InvoicesRoutingsFilterComponent = this;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.FillInvoiceStatus();
        this.InitilizeIds();
    }


    InitializeComponent() {

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
                case "IsLocalCurrency":
                    this.IsLocalCurrency = queryFilterItem.FieldValue;
                    break;
                case "CustomerId":
                    this.CustomerId = queryFilterItem.FieldValue;
                    break;
                case "PartnerId":
                    this.PartnerId = queryFilterItem.FieldValue;
                    break;
                case "TransportModeCode":
                    this.MySelectedTransportFilter = queryFilterItem.FieldValue;
                    break;
                case "DirectionCode":
                    this.MySelectedDirectionFilter=queryFilterItem.FieldValue;
                    break;
                case "IncludeVoidInvoices":
                    this.IncludeVoidInvoices =queryFilterItem.FieldValue;
                    break;
                case "IncludeDraftInvoices":
                    this.IncludeWaiting = queryFilterItem.FieldValue;
                    break;
                case "InvoiceStatusCode":
                    this.FillInvoiceStatus(queryFilterItem.FieldValue);
                    break;
                case "IsByInvoiceDate":
                    this.ShipmentTypeRadio =queryFilterItem.FieldValue ?? "InvoiceDate";    
                    break;
              
                


            }



        }
    }


    RunReport(isloading: boolean) {
        
        
        if (this.ValidateSelectedFilters()) {
            
            var reportFliter = new ReportFliter();
            reportFliter.NumberOfPage = 1;
            reportFliter.ProcessType = "GenerateReport";
            reportFliter.QueryFilterItemLists = this.GetQueryFilterItems();
            this.RunReportEvent.emit(reportFliter);

        }
    }
    
    ValidateSelectedFilters(){
        this.ValidationErrorsList = [];
        if (this.ToDate < this.FromDate) {
            this.ValidationErrorsList.push("From date must be less than to date");
        }

        if (!this.InvoiceStatusSelectedItem) {
            this.ValidationErrorsList.push("Invoice status field is required");
        }
        return this.ValidationErrorsList.length === 0;
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
        this.queryFilterItem.FieldName = "IsLocalCurrency";
        this.queryFilterItem.FieldValue = this.IsLocalCurrency;
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);

        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "IsByInvoiceDate";
        this.queryFilterItem.FieldValue = this.ShipmentTypeRadio =="InvoiceDate"?true:false;
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
        this.queryFilterItem.FieldName = "PartnerId";
        this.queryFilterItem.FieldValue = this.PartnerId;
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);

        var DirectionFilter: string = null;
        if (this.MySelectedDirectionFilter != "All")
            DirectionFilter = this.MySelectedDirectionFilter;
        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "DirectionCode";
        this.queryFilterItem.FieldValue = DirectionFilter;
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);


        var TransportMode: string = null;
        if (this.MySelectedTransportFilter != "All")
            TransportMode = this.MySelectedTransportFilter;
        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "TransportModeCode";
        this.queryFilterItem.FieldValue = TransportMode;
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

        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "InvoiceStatusCode";
        this.queryFilterItem.FieldValue = this.InvoiceStatusSelectedItem.Code;
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);

        return this.queryFilterItems
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
