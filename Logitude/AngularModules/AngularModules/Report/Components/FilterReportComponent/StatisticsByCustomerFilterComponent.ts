import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReportsPreviewComponent} from '../../Components/ReportsPreviewComponent';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ReportFliter} from '../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Component, OnInit, Output, ElementRef}  from '@angular/core';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {AppTool} from '../../../Infrastructure/Tools';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
@Component({
    
    selector: 'StatisticsByCustomerFilterComponent',
    templateUrl: './StatisticsByCustomerFilterComponent.html',
    inputs: ['ReportsPreview']
})

export class StatisticsByCustomerFilterComponent extends BaseComponent {
    public ReportsPreview: ReportsPreviewComponent;
    reportFliter: ReportFliter;
    public ObjectTableName: string = "Report";
    public DataContext: StatisticsByCustomerFilterComponent = this;
    public ValidationErrorsList: string[] = [];
    queryFilterItems: QueryFilterItem[];
    queryFilterItem: QueryFilterItem;

    ToDate: Date;
    FromDate: Date;

    public ProfitCurrencyCode: string = SessionLocator.TenantPM.ProfitCurrencyCode;
    public LocalCurrencyCode: string = SessionLocator.TenantPM.AccountingCurrencyCode;

    public IncludeClosed: boolean = false;
    public CustomerId = null;
    public IsCreateDateId: string = "IsCreateDateId";
    public IsOperationalDateId: string = "IsOperationalDateId";
    public DateRadio: string = "DateRadio_";
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

        this.IsOperationalDateId = this.IsOperationalDateId + this.CurrentSession.GetNewId(this.IsOperationalDateId);
        this.IsCreateDateId = this.IsCreateDateId + this.CurrentSession.GetNewId(this.IsCreateDateId);
        this.DateRadio = this.DateRadio + this.CurrentSession.GetNewId(this.DateRadio);
    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;

        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = this.daysInMonth(new Date());

        this.FromDate = this.SetDate(Year, month - 1, 1);
        this.ToDate = this.SetDate(Year, month, daysofmonth);

        //this.RunReport(false);
    }

    public IsByCreateDate: boolean = true;
    public IsOperationalDate: boolean = false;
    public IsCreateDate: boolean = true;
    IsCreateDateClicked() {
        this.IsByCreateDate = true;
    }
    IsOperationalDateClicked() {
        this.IsByCreateDate = false;
    }

    public selectedCurrency: string = this.LocalCurrencyCode;
    public get SelectedCurrency() {
        return this.selectedCurrency;
    }
    public set SelectedCurrency(value: string) {
        this.selectedCurrency = value;
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

    public LevelCodeSelectedValue: string = "All";
    LevelCodeitemClicked(itemValue: string) {
        if (this.LevelCodeSelectedValue != itemValue) {
            this.LevelCodeSelectedValue = itemValue;
        }
    }
    LevelCodeMouseOver(itemValue: string) {
        if (this.LevelCodeSelectedValue != itemValue) {

        }
    }
    LevelCodeMouseLeave(itemValue: string) {
        if (this.LevelCodeSelectedValue != itemValue) {

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
                case "FromDate":
                    this.FromDate = new Date(queryFilterItem.FieldValue);                                      
                    break;
                case "ToDate":
                    this.ToDate=new Date(queryFilterItem.FieldValue);
                    break;
                case "IsByCreateDate":{
                    this.IsByCreateDate = queryFilterItem.FieldValue;
                    this.IsOperationalDate = !queryFilterItem.FieldValue;

                    break;                          

                }
                case "CustomerId":
                        this.CustomerId = queryFilterItem.FieldValue;
                        break; 
                case "Direction":
                        this.SelectedDirectionFilter = queryFilterItem.FieldValue;
                        break;
                case "TransportMode":
                        this.SelectedTransportFilter = queryFilterItem.FieldValue;
                        break;
                        
                case "ShipmentLevel":
                        this.LevelCodeSelectedValue = queryFilterItem.FieldValue;
                        break;
                case "Currency":
                        var currency = queryFilterItem.FieldValue.split(",");
                        if (currency[1] == "profit") {
                            this.SelectedCurrency = this.ProfitCurrencyCode;
                        }
                        else {
                            this.SelectedCurrency = this.LocalCurrencyCode;
                        }
                        break;
                case "IncludeOperationalClosed":
                        this.IncludeClosed = queryFilterItem.FieldValue;
                        break;
            }
    
        }
    }
    ValidateSelectedFilters() {
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
        return this.ValidationErrorsList.length == 0;
    }

    RunReport(isloading: boolean) {
      

        if (this.ValidateSelectedFilters()) {
           

            this.reportFliter = new ReportFliter();
            this.reportFliter.Tenant = SessionInfo.LoggedUserTenant;
            this.reportFliter.QueryFilterItemLists = this.GetQueryFilterItems();
            this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
            this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
            this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
            this.reportFliter.NumberOfPage = 1;
            this.reportFliter.ProcessType = "GenerateReport";
            //this.reportFliter.IncludeOperationalyClosed = this.IncludeClosed;

            this.ReportsPreview.CleanPartnersObslist();

            if (!AppTool.IsNullOrEmpty(this.CustomerId)) {
                this.ReportsPreview.AddPartner("Customer", this.CustomerId);
            }

            this.ReportsPreview.GenerateReport(this.reportFliter, isloading);
        }
    }
    GetQueryFilterItems() {
        this.queryFilterItems = new Array<QueryFilterItem>();

        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "CustomerId";
        this.queryFilterItem.FieldValue = this.CustomerId;
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);

        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "IsByCreateDate";
        this.queryFilterItem.FieldValue = this.IsByCreateDate;
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);

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

        if (this.SelectedDirectionFilter != "All") {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "Direction";
            this.queryFilterItem.FieldValue = this.SelectedDirectionFilter;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }

        if (this.SelectedTransportFilter != "All") {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "TransportMode";
            this.queryFilterItem.FieldValue = this.SelectedTransportFilter;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }

        if (this.LevelCodeSelectedValue != "All") {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "ShipmentLevel";
            this.queryFilterItem.FieldValue = this.LevelCodeSelectedValue;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }

        var currency: string;
        if (this.SelectedCurrency == this.LocalCurrencyCode) {
            currency = this.LocalCurrencyCode + ",local";
        }

        else {
            currency = this.ProfitCurrencyCode + ",profit";
        }

        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "Currency";
        this.queryFilterItem.FieldValue = currency;
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);

        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "IncludeOperationalClosed";
        this.queryFilterItem.FieldValue = this.IncludeClosed;
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);
        return  this.queryFilterItems;
    }
    daysInMonth(aDate: Date) {
        return (new Date(aDate.getFullYear(), aDate.getMonth() + 1, 0)).getDate();
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
