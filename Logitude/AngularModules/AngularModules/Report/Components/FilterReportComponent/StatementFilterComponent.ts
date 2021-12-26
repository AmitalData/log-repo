import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ReportsPreviewComponent } from '../../Components/ReportsPreviewComponent';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { ReportFliter } from '../../Components/Filters/ReportFliter';
import { QueryFilterItem } from '../../Components/Filters/QueryFilterItem';
import { Component, OnInit } from '@angular/core';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { CodeNameClass } from './CodeNameClass';
import { CurrencyListService } from '../../../Common/Services/StandardLists/CurrencyListService';
import { CurrencyList } from '../../../Common/EntityLists/CurrencyList';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';

@Component({
    selector: 'StatementFilterComponent',
    templateUrl: './StatementFilterComponent.html',
    inputs: ['ReportsPreview']
})

export class StatementFilterComponent extends BaseComponent implements OnInit {
    public ReportsPreview: ReportsPreviewComponent;
    reportFliter: ReportFliter;
    queryFilterItem: QueryFilterItem;
    public queryFilterItems: QueryFilterItem[];
    public ObjectTableName: string = "Report";
    public DataContext: StatementFilterComponent = this;
    public CurrenciesComboList: Array<CodeNameClass>;
    public IsRegisterDateId: string = "IsRegisterDateId_";
    public IsDueDateId: string = "IsDueDateId_";
    private CurrentSession = SessionLocator.SelectedSession;
    public ValidationErrorsList: Array<string> = [];
    public IsSchedulerReport: boolean = false;
    public RunReportTitle: string;
    public CustomerChanged: boolean = false;
    SchedulerCurrencyCode: any;

    constructor() {
        super();
        this.IsRegisterDateId = this.IsRegisterDateId + this.CurrentSession.GetNewId(this.IsRegisterDateId);
        this.IsDueDateId = this.IsDueDateId + this.CurrentSession.GetNewId(this.IsDueDateId);
        this.DateRadio = this.DateRadio + this.CurrentSession.GetNewId(this.DateRadio);
        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = this.daysInMonth(new Date());

        this.FromDate = this.SetDate(Year, month - 1, 1);
        this.DueDate = this.SetDate(Year, month, daysofmonth);
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
    daysInMonth(aDate: Date) {
        return (new Date(aDate.getFullYear(), aDate.getMonth() + 1, 0)).getDate();
    }

    public CustomerId: string = null;
    public PartnerId: string = null;
    public DueDate: Date = null;
    public FromDate: Date = null;

    private selectedItemComboBox: CodeNameClass;
    get SelectedItemComboBox() { return this.selectedItemComboBox; }
    set SelectedItemComboBox(value: CodeNameClass) {
        if (this.selectedItemComboBox != value) {
            this.selectedItemComboBox = value;
        }
    }
    private includeDraftInvoices: boolean = false;
    public get IncludeDraftInvoices() { return this.includeDraftInvoices; }
    public set IncludeDraftInvoices(value: boolean) {
        if (this.includeDraftInvoices != value) {
            this.includeDraftInvoices = value;
        }
    }

    private customer: string;
    public get Customer() { return this.customer; }
    public set Customer(value: string) {
        if (this.customer != value) {
            this.SetCustomerChanged(this.customer);
            this.customer = value;
        }
    }


    private SetCustomerChanged(value: string) {
        if (value != undefined)
            this.CustomerChanged = true;
    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;
        this.FillCurrenciesComboBoxList();
    }

    FillCurrenciesComboBoxList() {
        var service: CurrencyListService = new CurrencyListService();
        service.getAll().subscribe((result: ServiceResponse) => {
            var list: CurrencyList[] = result.Result.filter(d => !d.InActive);
            if (list != null) {
                this.CurrenciesComboList = [];
                var all: CodeNameClass = new CodeNameClass();
                all.Code = "All";
                all.Name = "All";
                this.CurrenciesComboList.push(all);
                list.forEach(item => {
                    var currency: CodeNameClass = new CodeNameClass();
                    currency.Code = item.Code.toString();
                    currency.Name = item.Code;
                    this.CurrenciesComboList.push(currency);
                });

                this.SelectedItemComboBox = this.CurrenciesComboList.filter(a => a.Code == "All")[0];

                if (this.IsSchedulerReport && this.SchedulerCurrencyCode)
                    this.SelectedItemComboBox = this.CurrenciesComboList.filter(a => a.Code == this.SchedulerCurrencyCode)[0];

            }
        });
    }
    ngOnInit() {

    }

    public IsByDueDate: boolean = true;
    public IsRegisterDate: boolean = false;
    public IsDueDate: boolean = true;
    public DateRadio: string = "DateRadio_";
    IsDueDateClicked() {
        this.IsByDueDate = true;
    }
    IsRegisterDateClicked() {
        this.IsByDueDate = false;
    }

    SetQueryFilterItems(queryFilterItems: Array<QueryFilterItem>) { //For Report Scheduler
        this.IsSchedulerReport = true;
        if (!queryFilterItems) {
            return;
        }
        queryFilterItems.forEach(queryFilterItem => {
            this.SetFilterItem(queryFilterItem);
        });
    }


    SetRunReportTitle() {
        if (this.IsSchedulerReport) {
            this.RunReportTitle = 'Preview';
            return;
        }
        this.RunReportTitle = 'Run Report';
    }

    SetFilterItem(queryFilterItem: QueryFilterItem) {
        if (!queryFilterItem)
            return;

        this.SetCustomerIdFilter(queryFilterItem);
        this.SetCurrencyFilter(queryFilterItem);
        this.SetByDueDateFilter(queryFilterItem);
        this.SetFromDateFilter(queryFilterItem);
        this.SetDueDateFilter(queryFilterItem);
        this.SetIncludeDraftInvoicesFilter(queryFilterItem);
        this.SetPartnerIdFilter(queryFilterItem);
        this.SetARAPFilter(queryFilterItem);
        this.SetInvoicePaymentFilter(queryFilterItem);
    }

    GetLookUpFieldValue(field) {
        if (field && field[0]["@nil"] != "true")
            return field;
        return null
    }

    SetInvoicePaymentFilter(queryFilterItem: QueryFilterItem) {
        if (queryFilterItem.FieldName != "InvoicePaymentFilter") {
            return;
        }
        this.InvoicePaymentSelectedValue = queryFilterItem.FieldValue;
    }

    SetARAPFilter(queryFilterItem: QueryFilterItem) {
        if (queryFilterItem.FieldName != "ARAPFilter") {
            return;
        }
        this.ARAPSelectedValue = queryFilterItem.FieldValue;
    }

    SetPartnerIdFilter(queryFilterItem: QueryFilterItem) {
        if (queryFilterItem.FieldName != "PartnerId") {
            return;
        }
        this.PartnerId = this.GetLookUpFieldValue(queryFilterItem.FieldValue);
    }
    SetIncludeDraftInvoicesFilter(queryFilterItem: QueryFilterItem) {
        if (queryFilterItem.FieldName != "IncludeDraftInvoices") {
            return;
        }
        this.IncludeDraftInvoices = queryFilterItem.FieldValue;
    }

    SetDueDateFilter(queryFilterItem: QueryFilterItem) {
        if (queryFilterItem.FieldName != "DueDate") {
            return;
        }
        this.DueDate = this.GetLookUpFieldValue(queryFilterItem.FieldValue);
    }

    SetFromDateFilter(queryFilterItem: QueryFilterItem) {
        if (queryFilterItem.FieldName != "FromDate") {
            return;
        }
        this.FromDate = queryFilterItem.FieldValue;
    }

    SetByDueDateFilter(queryFilterItem: QueryFilterItem) {
        if (queryFilterItem.FieldName != "IsByDueDate") {
            return;
        }
        this.IsByDueDate = queryFilterItem.FieldValue;
        this.IsRegisterDate = false;
        this.IsDueDate = false;

        if (this.IsByDueDate) {
            this.IsDueDate = true;
        }
        if (!this.IsByDueDate) {
            this.IsRegisterDate = true;
        }
    }

    SetCurrencyFilter(queryFilterItem: QueryFilterItem) {
        if (queryFilterItem.FieldName != "CurrencyCode") {
            return;
        }
        this.SchedulerCurrencyCode = queryFilterItem.FieldValue;
    }

    SetCustomerIdFilter(queryFilterItem: QueryFilterItem) {
        if (queryFilterItem.FieldName != "BillToId") {
            return;
        }
        this.CustomerId = this.GetLookUpFieldValue(queryFilterItem.FieldValue);
    }

    RunReport(isloading: boolean) {
        if (!this.ValidateSelectedFilters())
            return;

        this.GetQueryFilterItems();

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

    GetQueryFilterItems() {
        this.queryFilterItems = [];
        this.queryFilterItems = new Array<QueryFilterItem>();
        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "BillToId";
        this.queryFilterItem.FieldValue = this.CustomerId;
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);
        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "PartnerId";
        this.queryFilterItem.FieldValue = this.PartnerId;
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);
        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "DueDate";
        this.queryFilterItem.FieldValue = this.DueDate;
        this.queryFilterItem.FieldDataType = "Date";
        this.queryFilterItem.Operator = "LessThanOrEqual";
        this.queryFilterItems.push(this.queryFilterItem);
        if (this.FromDate) {
            this.AddDateQueryFilter();
        }
        this.queryFilterItems.push(new QueryFilterItem("IncludeDraftInvoices", this.IncludeDraftInvoices));
        this.queryFilterItems.push(new QueryFilterItem("IsByDueDate", this.IsByDueDate));
        if (this.ARAPSelectedValue != "All") {
            this.AddARAPAllFilter();
        }
        if (this.InvoicePaymentSelectedValue != "All") {
            this.AddInvoicePaymentFilter();
        }
        if (this.SelectedItemComboBox != null && this.SelectedItemComboBox.Code != "All") {
            this.AddCurrencyCodeFilter();
        }

        return this.queryFilterItems;
    }

    private AddCurrencyCodeFilter() {
        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "CurrencyCode";
        this.queryFilterItem.FieldValue = this.SelectedItemComboBox.Code;
        this.queryFilterItem.Operator = "CurrencyCode";
        this.queryFilterItems.push(this.queryFilterItem);
    }

    private AddInvoicePaymentFilter() {
        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "InvoicePaymentFilter";
        this.queryFilterItem.FieldValue = this.InvoicePaymentSelectedValue;
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);
    }

    private AddARAPAllFilter() {
        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "ARAPFilter";
        this.queryFilterItem.FieldValue = this.ARAPSelectedValue;
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);
    }

    private AddDateQueryFilter() {
        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "FromDate";
        this.queryFilterItem.FieldValue = this.FromDate;
        this.queryFilterItem.FieldDataType = "Date";
        this.queryFilterItem.Operator = "GreaterThanOrEqual";
        this.queryFilterItems.push(this.queryFilterItem);
    }

    ValidateSelectedFilters() {
        this.ValidationErrorsList = [];
        var isValid: boolean = true;
        isValid = !isValid ? false : this.ValidateDates();
        return isValid;
    }

    ValidateDates(): boolean {
        if (this.FromDate != null && this.DueDate != null && (this.FromDate > this.DueDate)) {
            this.ValidationErrorsList.push("From date field must be less than To date field");
            return false
        }
        return true;
    }

    IsPartnersChanged(SelectedTab) { //For Report Scheduler
        if (SelectedTab == '2')
            this.CustomerChanged = false;
        return this.CustomerChanged;
    }

    GetMainCustomerFieldName() { //For Report Scheduler
        return null;
    }

    PrepareContactList() {//For Report Scheduler
        this.ReportsPreview.CleanPartnersObslist();
        if (!AppTool.IsNullOrEmpty(this.CustomerId)) this.ReportsPreview.AddPartner("Bill To/ Vendor", this.CustomerId);
        if (!AppTool.IsNullOrEmpty(this.PartnerId)) this.ReportsPreview.AddPartner("Partner", this.PartnerId);
    }


    public ARAPSelectedValue: string = "All";
    ARAPItemClicked(itemValue: string) {
        if (this.ARAPSelectedValue != itemValue) {
            this.ARAPSelectedValue = itemValue;
        }
    }
    ARAPMouseOver(itemValue: string) {
        if (this.ARAPSelectedValue != itemValue) {

        }
    }
    ARAPMouseLeave(itemValue: string) {
        if (this.ARAPSelectedValue != itemValue) {

        }
    }


    public InvoicePaymentSelectedValue: string = "All";
    InvoicePaymentItemClicked(itemValue: string) {
        if (this.InvoicePaymentSelectedValue != itemValue) {
            this.InvoicePaymentSelectedValue = itemValue;
        }
    }
    InvoicePaymentMouseOver(itemValue: string) {
        if (this.InvoicePaymentSelectedValue != itemValue) {

        }
    }
    InvoicePaymentMouseLeave(itemValue: string) {
        if (this.InvoicePaymentSelectedValue != itemValue) {

        }
    }
}


