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

@Component({
    moduleId: module.id,
    selector: 'ParentVsChildTenantsComponent',
    templateUrl: './ParentVsChildTenantsComponent.html',
    inputs: ['ReportsPreview']
})

export class ParentVsChildTenantsComponent extends BaseComponent implements OnInit {
    public ReportsPreview: ReportsPreviewComponent;
    public ValidationErrorsList: string[];
    public ObjectTableName: string = "Report";
    public DataContext: ParentVsChildTenantsComponent = this;
    
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
    RunReport(isloading: boolean) {
        this.ValidationErrorsList = [];

        if (this.ValidationErrorsList.length == 0) {
            this.queryFilterItems = new Array<QueryFilterItem>();

            if (this.SelectedParentTenant != null) {
                this.queryFilterItem = new QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "ParentTenantId";
                this.queryFilterItem.FieldValue = this.SelectedParentTenant.Code;
                this.queryFilterItem.Operator = "Equals";
                this.queryFilterItems.push(this.queryFilterItem);
            }

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
}