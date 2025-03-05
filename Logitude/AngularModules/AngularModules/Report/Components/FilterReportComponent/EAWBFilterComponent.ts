import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReportsPreviewComponent} from '../../Components/ReportsPreviewComponent';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ReportFliter} from '../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Component, OnInit, Output, ElementRef}  from '@angular/core';
import {FormBuilder, FormGroup, FormsModule} from '@angular/forms';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {ParticipantList} from '../../EntityLists/ParticipantList';
import {ReportsDomainService} from '../../Services/ReportsDomainService';
import {CodeNameClass} from './CodeNameClass';

import {AppTool} from '../../../Infrastructure/Tools';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';

@Component({
    
    selector: 'EAWBFilterComponent',
    templateUrl: './EAWBFilterComponent.html',
    inputs: ['ReportsPreview']
})

export class EAWBFilterComponent extends BaseComponent implements OnInit {
    public ReportsPreview: ReportsPreviewComponent;
    public SelectedItemComboBox: string;
    public TenantPM: TenantPM;
    public TenantsComboList: Array<CodeNameClass>;
    MainCarriageFromPortId: string;
    MainCarriageFinalDestinationPortId: string;
    reportFliter: ReportFliter;
    ToDate: Date;
    public HeightControl: string;
    FromDate: Date;
    queryFilterItems: QueryFilterItem[];

    queryFilterItem: QueryFilterItem;
    public ObjectTableName: string = "Report";
    public DataContext: EAWBFilterComponent = this;
    constructor() {
        super();
    }
   
    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;
        this.TenantPM = SessionLocator.TenantPM;

        var month = new Date().getMonth();
        var Year = new Date().getFullYear();
        var daysofmonth = this.DaysInMonth(new Date());
        this.FromDate = this.SetDate(Year, month - 1, 1);
        this.ToDate = this.SetDate(Year, month - 1, daysofmonth);

        this.GetTenants();
    }

    ngOnInit() {
        
    }

    GetTenants() {
        var mySerivce: ReportsDomainService = new ReportsDomainService();
        mySerivce.GetActivityStatus(this.TenantPM.Id).subscribe((result: any)=>{
                this.UpdateTenantsList(result);
        });
    }

    UpdateTenantsList(result: Array<ParticipantList>) {
        this.TenantsComboList = [];
        result.sort((a, b) => { return (a.ForwarderTenantName === b.ForwarderTenantName) ? 0 : (a.ForwarderTenantName > b.ForwarderTenantName) ? -1 : 1 });
        result.forEach(item => {

            var obj: CodeNameClass = new CodeNameClass();
            obj.Code = item.ForwarderTenant.toString();
            obj.Name = item.ForwarderTenantName;
            this.TenantsComboList.push(obj);
        });
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
              
                case "MainCarriageFromPortId":
                    this.MainCarriageFromPortId = queryFilterItem.FieldValue;
                    break;
                case "MainCarriageFinalDestinationPortId":
                    this.MainCarriageFinalDestinationPortId = queryFilterItem.FieldValue;
                    break;
                case "CustomerId":
                    this.SelectedItemComboBox = queryFilterItem.FieldValue;
                    break;   
               
            }
    
           
    
        }
    }
    ValidateSelectedFilters() {
      return true;
    }

    RunReport(isloading: boolean) {
        
        
        this.reportFliter = new ReportFliter();
        this.reportFliter.Tenant = SessionInfo.LoggedUserTenant;
        this.reportFliter.QueryFilterItemLists = this.GetQueryFilterItems();
        this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
        this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
        this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
        this.reportFliter.NumberOfPage = 1;
        this.reportFliter.ProcessType = "GenerateReport";
        this.reportFliter.IncludeOperationalyClosed = false;
        this.ReportsPreview.CleanPartnersObslist();

        if (!AppTool.IsNullOrEmpty(this.SelectedItemComboBox)) {
            this.ReportsPreview.AddPartner("Participant", this.SelectedItemComboBox);
        }

        this.ReportsPreview.GenerateReport(this.reportFliter, isloading);
    }
    GetQueryFilterItems(){
        this.queryFilterItems = new Array<QueryFilterItem>();

        if (this.FromDate != null) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "FromDate";
            this.queryFilterItem.FieldValue = this.FromDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItems.push(this.queryFilterItem);
        }

        if (this.ToDate != null) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "ToDate";
            this.queryFilterItem.FieldValue = this.ToDate;
            this.queryFilterItem.FieldDataType = "Date";
            this.queryFilterItems.push(this.queryFilterItem);
        }

        if (this.MainCarriageFromPortId) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "MainCarriageFromPortId";
            this.queryFilterItem.FieldValue = this.MainCarriageFromPortId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }

        if (this.MainCarriageFinalDestinationPortId) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "MainCarriageFinalDestinationPortId";
            this.queryFilterItem.FieldValue = this.MainCarriageFinalDestinationPortId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }

        if (this.SelectedItemComboBox) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "CustomerId";
            this.queryFilterItem.FieldValue = this.SelectedItemComboBox;
            this.queryFilterItem.Operator = "CustomerId";
            this.queryFilterItems.push(this.queryFilterItem);
        }
        return this.queryFilterItems;
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

    private DaysInMonth(aDate: Date) {
        return (new Date(aDate.getFullYear(), aDate.getMonth(), 0)).getDate();
    }
}
