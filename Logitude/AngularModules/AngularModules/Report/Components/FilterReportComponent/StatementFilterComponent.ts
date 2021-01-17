import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReportsPreviewComponent} from '../../Components/ReportsPreviewComponent';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ReportFliter} from '../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';
import {Component, OnInit}  from '@angular/core';
import { AppTool } from '../../../Infrastructure/Tools';
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

    constructor() {
        super();
        this.IsRegisterDateId = this.IsRegisterDateId + this.CurrentSession.GetNewId(this.IsRegisterDateId);
        this.IsDueDateId = this.IsDueDateId + this.CurrentSession.GetNewId(this.IsDueDateId);
        this.DateRadio = this.DateRadio + this.CurrentSession.GetNewId(this.DateRadio);
    }

    public CustomerId: string = null;
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
    RunReport(isloading: boolean) {
        this.ValidationErrorsList = [];
        if (this.FromDate > this.DueDate)
            this.ValidationErrorsList.push("From date field must be less than To date field");

        if (this.ValidationErrorsList.length == 0) {
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
            this.queryFilterItem.FieldName = "DueDate";
            this.queryFilterItem.FieldValue = this.DueDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItem.Operator = "LessThanOrEqual";
            this.queryFilterItems.push(this.queryFilterItem);

            if (this.FromDate) {
                this.queryFilterItem = new QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "FromDate";
                this.queryFilterItem.FieldValue = this.FromDate;
                this.queryFilterItem.FieldDataType = "Date";
                this.queryFilterItem.Operator = "GreaterThanOrEqual";
                this.queryFilterItems.push(this.queryFilterItem);
            }

            this.queryFilterItems.push(new QueryFilterItem("IncludeDraftInvoices", this.IncludeDraftInvoices));
            this.queryFilterItems.push(new QueryFilterItem("IsByDueDate", this.IsByDueDate));

            if (this.ARAPSelectedValue != "All") {
                this.queryFilterItem = new QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "ARAPFilter";
                this.queryFilterItem.FieldValue = this.ARAPSelectedValue;
                this.queryFilterItem.Operator = "Equals";
                this.queryFilterItems.push(this.queryFilterItem);
            }

            if (this.InvoicePaymentSelectedValue != "All") {
                this.queryFilterItem = new QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "InvoicePaymentFilter";
                this.queryFilterItem.FieldValue = this.InvoicePaymentSelectedValue;
                this.queryFilterItem.Operator = "Equals";
                this.queryFilterItems.push(this.queryFilterItem);
            }


            if (this.SelectedItemComboBox != null && this.SelectedItemComboBox.Code != "All") {
                this.queryFilterItem = new QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "CurrencyCode";
                this.queryFilterItem.FieldValue = this.SelectedItemComboBox.Code;
                this.queryFilterItem.Operator = "CurrencyCode";
                this.queryFilterItems.push(this.queryFilterItem);
            }

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


