import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { AppTool } from '../../../../Infrastructure/Tools';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { QueryFilterItem } from '../../../Components/Filters/QueryFilterItem';
import { ReportsPreviewComponent } from 'Report/Components/ReportsPreviewComponent';
import { TenantPM } from 'Common/EntityPMs/TenantPM';
import { ReportFliter } from 'Report/Components/Filters/ReportFliter';
import { CodeNameClass } from 'Report/Components/FilterReportComponent/CodeNameClass';
import { ReportsDomainService } from 'Report/Services/ReportsDomainService';
import { OpportunityTypePMService } from 'CRM/Services/StandardPMs/OpportunityTypePMService';
import { OpportunityTypeListService } from 'CRM/Services/StandardLists/OpportunityTypeListService';
import { OpportunityTypeList } from 'CRM/EntityLists/OpportunityTypeList';

@Component({

    templateUrl: './LogitudeCRMReportFilterComponent.html',
})

export class LogitudeCRMReportFilterComponent extends BaseComponent {
    public ReportsPreview: ReportsPreviewComponent;
    public ResellerId: string = null;
    public DataContext: LogitudeCRMReportFilterComponent = this;
    public ShowNet: boolean;
    public ExchangeRate: string;
    public ValidationErrorsList: string[] = [];
    public TenantPM: TenantPM;
    public FilterdOpportunityTypeList: any;
    private opportunityTypeListService: OpportunityTypeListService = new OpportunityTypeListService();
    
    queryFilterItems: QueryFilterItem[];
    queryFilterItem: QueryFilterItem;

    constructor() {
        super();
    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;
        this.TenantPM = SessionLocator.TenantPM;
        this.BuildCustomerStatusFilters();
        this.BuildYearsFilters();

        this.opportunityTypeListService.getAllFromCache().subscribe((result: any) => {
            var list: OpportunityTypeList[] = result.Result;
            this.fillOpportunityTypecombo(list);

        });

    }

    

    fillOpportunityTypecombo(arr: any) {
        this.FilterdOpportunityTypeList = [];
        arr.forEach((i) => {
            if (!i.InActive) {
                var item = new CodeNameClass();
                item.Code = i.Id;
                item.Name = i.Name;
                item.Checked = false;
                this.FilterdOpportunityTypeList.push(i);
            }
        });
        this.FilterdOpportunityTypeList.sort((a, b) => { return (a.Name === b.Name) ? 0 : (a.Name < b.Name) ? -1 : 1 });    
    }


    public CustomerStatusList: CodeNameClass[];
    private BuildCustomerStatusFilters() {
        this.CustomerStatusList = [];
        this.CustomerStatusList.push(new CodeNameClass("ALL", "All"));
        this.CustomerStatusList.push(new CodeNameClass("ACT", "Active"));

        this.selectedCustomerStatus = this.CustomerStatusList.filter(d => d.Code == "ALL")[0];
    }

    public Years: number[];
    private BuildYearsFilters() {
        this.Years = [];
        this.BuidYears();
        this.selectedYear = this.Years[0];
    }

    private BuidYears() {
        var currentYear = new Date().getFullYear();
        for (let i = currentYear; i >= 2014; i--) {
            this.Years.push(i);
        }
    }
    
    private selectedCustomerStatus: CodeNameClass;
    get SelectedCustomerStatus() { return this.selectedCustomerStatus; }
    set SelectedCustomerStatus(value: CodeNameClass) {
        if (this.selectedCustomerStatus != value) {
            this.selectedCustomerStatus = value;
        }
    }

    private selectedYear: number;
    get SelectedYear() { return this.selectedYear; }
    set SelectedYear(value: number) {
        if (this.selectedYear != value) {
            this.selectedYear = value;
        }
    }
    
    public SelectedItem: string = "All";
    SelectedOpportunityTypeListChanged(item) {
        this.SelectedItem = item;
    }    


    EditedItemSource(newSource: any) {
        this.FilterdOpportunityTypeList = newSource;
    }

    RunReport(isloading: boolean) {
        this.ValidationErrorsList = [];
        if (this.ExchangeRate == null) {
            this.ValidationErrorsList.push("Exchange Rate is required");
        }

        if (!this.SelectedCustomerStatus) {
            this.ValidationErrorsList.push("Customer field is required");
        }

        if (!this.selectedYear) {
            this.ValidationErrorsList.push("Year field is required");
        }


        if (this.ValidationErrorsList.length != 0) {
            return;
        }

        if (this.SelectedCustomerStatus.Code == "ALL") {
            this.SelectedCustomerStatus.Code = "";
        }

        var myOpportunityTypes: string = "";
        
        if (this.SelectedItem == "All") {
            myOpportunityTypes = "All";
        }

        else {
            this.FilterdOpportunityTypeList.forEach((i) => {
                if (i.Checked) {
                    myOpportunityTypes += i.Id + ",";
                }
            });
        }

        this.queryFilterItems = new Array<QueryFilterItem>();


        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "ExchangeRate";
        this.queryFilterItem.FieldValue = this.ExchangeRate;
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);

        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "OpportunityTypes";
        this.queryFilterItem.FieldValue = myOpportunityTypes;
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);

        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "CustomerStatus";
        this.queryFilterItem.FieldValue = this.SelectedCustomerStatus.Code;
        this.queryFilterItems.push(this.queryFilterItem);

        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "Year";
        this.queryFilterItem.FieldValue = this.SelectedYear;
        this.queryFilterItems.push(this.queryFilterItem);

        if (this.ResellerId) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "ResellerId";
            this.queryFilterItem.FieldValue = this.ResellerId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }

        this.queryFilterItem = new QueryFilterItem();
        this.queryFilterItem.DisplayInList = false;
        this.queryFilterItem.FieldName = "ShowNet";
        this.queryFilterItem.FieldValue = this.ShowNet;
        this.queryFilterItem.Operator = "Equals";
        this.queryFilterItems.push(this.queryFilterItem);


        var reportFliter = new ReportFliter();
        reportFliter.Tenant = SessionLocator.Tenant;
        reportFliter.QueryFilterItemLists = this.queryFilterItems;
        reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
        reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
        reportFliter.ReportCode = this.ReportsPreview.Report.Code;
        reportFliter.NumberOfPage = 1;
        reportFliter.ProcessType = "GenerateReport";

        this.ReportsPreview.GenerateReport(reportFliter, isloading);

    }



}
