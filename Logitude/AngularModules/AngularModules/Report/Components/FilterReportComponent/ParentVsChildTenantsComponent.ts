import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReportsPreviewComponent} from '../../Components/ReportsPreviewComponent';
import {Component, OnInit}  from '@angular/core';
import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';
import {ReportFliter} from '../../Components/Filters/ReportFliter';
import {CodeNameClass} from '../../../Infrastructure/DataContracts/CodeNameClass';
import {GlobalDomainService} from '../../../Common/Services/GlobalDomainService';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TenantManagementList} from '../../../Infrastructure/EntityLists/TenantManagementList';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';

@Component({
    
    selector: 'ParentVsChildTenantsComponent',
    templateUrl: './ParentVsChildTenantsComponent.html',
    inputs: ['ReportsPreview']
})

export class ParentVsChildTenantsComponent extends BaseComponent implements OnInit {
    public ReportsPreview: ReportsPreviewComponent;
    public ValidationErrorsList: string[];
    public ObjectTableName: string = "Report";
    public DataContext: ParentVsChildTenantsComponent = this;
    public SelectedParentTenantFilter: string = null;
    constructor() {
        super();
    }

    ngOnInit() {

    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;

        this.GetTenants();
    }

    private GetTenants() {
        var service: GlobalDomainService = new GlobalDomainService();
        service.GetParentTenants().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                var myList: TenantManagementList[] = myResponse.Result

                this.BuildParentTenantList(myList);
            }
        });
    }

    public ParentTenantList: CodeNameClass[];
    private BuildParentTenantList(myList: TenantManagementList[]) {
        this.ParentTenantList = [];

        if (myList) {
            if (myList.length > 0) {
                
                myList.forEach(item => {
                    var newItem: CodeNameClass = new CodeNameClass();
                    newItem.Code = item.Id.toString();
                    newItem.Name = item.Name;
                    newItem.DisplyText = item.Name + " (" + item.Id + ")";

                    this.ParentTenantList.push(newItem);
                });  
                if(this.SelectedParentTenantFilter!=null)  {
                    this.SelectedParentTenant = this.ParentTenantList.filter(d => d.Code === this.SelectedParentTenantFilter)[0];
                }            
            }
        }
    }

    private selectedParentTenant: CodeNameClass;
    get SelectedParentTenant() { return this.selectedParentTenant; }
    set SelectedParentTenant(value: CodeNameClass) {
        if (this.selectedParentTenant != value) {
            this.selectedParentTenant = value;
        }
    }

    queryFilterItems: QueryFilterItem[];
    queryFilterItem: QueryFilterItem;
    public RunReportTitle: string = 'Run Report';
    SetRunReportTitle() {
        
            if (this.IsSchedulerReport) {
                this.RunReportTitle = TextCodeTranslator.Translate("AgingReport.O.PreviewReport");
            }
            else {
                this.RunReportTitle = TextCodeTranslator.Translate("AgingReport.O.RunReport");
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
    private SetFilterItem(queryFilterItem: QueryFilterItem) {
   
        if (queryFilterItem) {
            switch (queryFilterItem.FieldName) {
                case "ParentTenantId":{
                    this.SelectedParentTenantFilter= queryFilterItem.FieldValue;
                    this.GetTenants();
                    break;
                        
                }
                    
            }
                 
        }
    }
    ValidateSelectedFilters(){
      return true;  
    }
    RunReport(isloading: boolean) {
        
            var reportFilter = new ReportFliter();
            reportFilter.Tenant = SessionLocator.Tenant;
            reportFilter.QueryFilterItemLists = this.GetQueryFilterItems();
            reportFilter.FilterControlName = this.ReportsPreview.FilterControlName;
            reportFilter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
            reportFilter.ReportCode = this.ReportsPreview.Report.Code;
            reportFilter.NumberOfPage = 1;
            reportFilter.ProcessType = "GenerateReport";

            this.ReportsPreview.GenerateReport(reportFilter, isloading);
      
    }

    GetQueryFilterItems(){
        this.queryFilterItems = new Array<QueryFilterItem>();

            if (this.SelectedParentTenant != null) {
                this.queryFilterItem = new QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "ParentTenantId";
                this.queryFilterItem.FieldValue = this.SelectedParentTenant.Code;
                this.queryFilterItem.Operator = "Equals";
                this.queryFilterItems.push(this.queryFilterItem);
            }
            return this.queryFilterItems;   
    }
}