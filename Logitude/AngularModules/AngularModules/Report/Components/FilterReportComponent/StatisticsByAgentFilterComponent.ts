import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReportsPreviewComponent} from '../../Components/ReportsPreviewComponent';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ReportFliter} from '../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Component }  from '@angular/core';
import { AppTool } from '../../../Infrastructure/Tools';

@Component({    
    selector: 'StatisticsByAgentFilterComponent',
    templateUrl: './StatisticsByAgentFilterComponent.html',
    inputs: ['ReportsPreview']
})

export class StatisticsByAgentFilterComponent extends BaseComponent  {
    public ReportsPreview: ReportsPreviewComponent;    
    reportFliter: ReportFliter;   
    public ObjectTableName: string = "Report";
    public DataContext: StatisticsByAgentFilterComponent = this;     
    public ValidationErrorsList: string[] = [];
    queryFilterItems: QueryFilterItem[];
    queryFilterItem: QueryFilterItem;
    
    ToDate: Date;
    FromDate: Date;

    public ProfitCurrencyCode: string = SessionLocator.TenantPM.ProfitCurrencyCode;
    public LocalCurrencyCode: string = SessionLocator.TenantPM.AccountingCurrencyCode;

    public IncludeClosed: boolean = false;
    public AgentId = null;
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
            this.queryFilterItem.FieldName = "AgentId";
            this.queryFilterItem.FieldValue = this.AgentId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "CurrencyCodeType";
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);

            if (this.SelectedCurrency == this.LocalCurrencyCode)
                this.queryFilterItem.FieldValue = this.LocalCurrencyCode + ",local";
            else
                this.queryFilterItem.FieldValue = this.ProfitCurrencyCode + ",profit";

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "IncludeOperationalyClosed";
            this.queryFilterItem.FieldValue = this.IncludeClosed;
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

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "ShipmentLevel";
            this.queryFilterItem.FieldValue = this.LevelCodeSelectedValue;
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
            if (!AppTool.IsNullOrEmpty(this.AgentId)) {
                this.ReportsPreview.AddPartner("Agent", this.AgentId);
            }

            this.ReportsPreview.GenerateReport(this.reportFliter, isloading);
        }
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

    private mySelectedDirectionFilter: string = "All";
    get SelectedDirectionFilter() { return this.mySelectedDirectionFilter; }
    set SelectedDirectionFilter(value: string) {
        if (this.mySelectedDirectionFilter != value) {
            this.mySelectedDirectionFilter = value;           
        }
    }

    private mySelectedTransportFilter: string = "All";
    get SelectedTransportFilter() { return this.mySelectedTransportFilter; }
    set SelectedTransportFilter(value: string) {
        if (this.mySelectedTransportFilter != value) {
            this.mySelectedTransportFilter = value;
        }
    }

    public LevelCodeSelectedValue: string = "Shipments";
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
}
