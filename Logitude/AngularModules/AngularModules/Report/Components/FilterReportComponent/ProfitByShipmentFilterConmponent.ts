import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReportsPreviewComponent} from '../../Components/ReportsPreviewComponent';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ReportFliter} from '../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Component, OnInit}  from '@angular/core';
import {AppTool} from '../../../Infrastructure/Tools';

@Component({    
    selector: 'ProfitByShipmentFilterConmponent',
    templateUrl: './ProfitByShipmentFilterConmponent.html',
    inputs: ['ReportsPreview']
})

export class ProfitByShipmentFilterConmponent extends BaseComponent implements OnInit {
    public ReportsPreview: ReportsPreviewComponent;    
    public IncludeClosed: boolean = false;
    public IncludeAccounting: boolean = false;
    public IncludeAccountedOnly: boolean = false;
    public ProfitCurrencyCode: string = SessionLocator.TenantPM.ProfitCurrencyCode;
    public LocalCurrencyCode: string = SessionLocator.TenantPM.AccountingCurrencyCode;

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
        
    public SalesmanUserId: string;
    public AgentId: string;
    public DepartmentId: string;
    public CarrierId: string;

    private mySelectedDirectionFilter: string = "All";
    public get MySelectedDirectionFilter() { return this.mySelectedDirectionFilter; }
    public set MySelectedDirectionFilter(newValue: string) {
        if (this.mySelectedDirectionFilter != newValue) {
            this.mySelectedDirectionFilter = newValue;
        }
    }
    
    queryFilterItems: QueryFilterItem[];
    public CustomerId = null;
    queryFilterItem: QueryFilterItem;
    public ObjectTableName: string = "Report";
    public DataContext: ProfitByShipmentFilterConmponent = this;
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

    ngOnInit() {
        
    }

    private mySelectedTransportFilter: string = "All";
    get SelectedTransportFilter() { return this.mySelectedTransportFilter; }
    set SelectedTransportFilter(value: string) {
        if (this.mySelectedTransportFilter != value) {
            this.mySelectedTransportFilter = value;
        }
    }

    daysInMonth(aDate: Date) {
        return (new Date(aDate.getFullYear(), aDate.getMonth() + 1, 0)).getDate();
    }

    RunReport(isloading: boolean) {
        this.queryFilterItems = new Array<QueryFilterItem>();

        if (this.FromDate) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "FromDate";
            this.queryFilterItem.FieldValue = this.FromDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItem.Operator = "GreaterThanOrEqual";
            this.queryFilterItems.push(this.queryFilterItem);
        }

        if (this.ToDate) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "ToDate";
            this.queryFilterItem.FieldValue = this.ToDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItem.Operator = "LessThanOrEqual";
            this.queryFilterItems.push(this.queryFilterItem);
        }

        if (this.AgentId) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "AgentId";
            this.queryFilterItem.FieldValue = this.AgentId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }

        if (this.MySelectedDirectionFilter) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "DirectionId";
            this.queryFilterItem.FieldValue = this.MySelectedDirectionFilter;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }

        if (this.IncludeAccounting) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "AccountingClosed";
            this.queryFilterItem.FieldValue = this.IncludeAccounting;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }

        if (this.IncludeAccountedOnly) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "IncludeAccountedOnly";
            this.queryFilterItem.FieldValue = this.IncludeAccountedOnly;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }

        if (this.CustomerId) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "CustomerId";
            this.queryFilterItem.FieldValue = this.CustomerId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }      
        
        if (this.SalesmanUserId) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "SalesmanUserId";
            this.queryFilterItem.FieldValue = this.SalesmanUserId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }

        if (this.DepartmentId) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "DepartmentId";
            this.queryFilterItem.FieldValue = this.DepartmentId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }

        if (this.CarrierId) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "CarrierId";
            this.queryFilterItem.FieldValue = this.CarrierId;
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
        this.queryFilterItem.FieldName = "IsByCreateDate";
        this.queryFilterItem.FieldValue = this.IsByCreateDate;
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);

        this.reportFliter = new ReportFliter();
        this.reportFliter.Tenant = SessionInfo.LoggedUserTenant;
        this.reportFliter.QueryFilterItemLists = this.queryFilterItems;
        this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;

        if (this.SelectedCurrency == this.LocalCurrencyCode)
            this.reportFliter.CurrentCurrencyCodeType = this.LocalCurrencyCode + ",local";
        else
            this.reportFliter.CurrentCurrencyCodeType = this.ProfitCurrencyCode + ",profit";

        this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
        this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
        this.reportFliter.NumberOfPage = 1;
        this.reportFliter.ProcessType = "GenerateReport";

        this.reportFliter.IncludeOperationalyClosed = this.IncludeClosed;

        this.ReportsPreview.CleanPartnersObslist();
        if (!AppTool.IsNullOrEmpty(this.CustomerId)) this.ReportsPreview.AddPartner("Partner", this.CustomerId);
        if (!AppTool.IsNullOrEmpty(this.AgentId)) this.ReportsPreview.AddPartner("Agent", this.AgentId);

        this.ReportsPreview.GenerateReport(this.reportFliter, isloading);
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

    public IsByCreateDate: boolean = false;
    public IsCreateDate: boolean = false;
    public IsOperationalDate: boolean = true;
    public IsCreateDateId: string = "IsCreateDateId_";
    public IsOperationalDateId: string = "IsOperationalDateId";
    public DateRadio: string = "DateRadio_";

    IsOperationalDateClicked() {
        this.IsByCreateDate = false;
    }

    IsCreateDateClicked() {
        this.IsByCreateDate = true;
    }
}
