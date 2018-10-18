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
    constructor() {
        super();

        this.IsOperationalDateId = this.IsOperationalDateId + SessionLocator.CurrentSession.GetNewId(this.IsOperationalDateId);
        this.IsCreateDateId = this.IsCreateDateId + SessionLocator.CurrentSession.GetNewId(this.IsCreateDateId); 
        this.DateRadio = this.DateRadio + SessionLocator.CurrentSession.GetNewId(this.DateRadio);      
    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;

        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = this.daysInMonth(new Date());

        this.FromDate = this.SetDate(Year, month - 1, 1);
        this.ToDate = this.SetDate(Year, month, daysofmonth);

        this.RunReport(false);
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
}