import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReportsPreviewComponent} from '../../Components/ReportsPreviewComponent';
import {Component, OnInit}  from '@angular/core';
import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';
import {ReportFliter} from '../../Components/Filters/ReportFliter';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {AppTool} from '../../../Infrastructure/Tools';
@Component({
    moduleId: module.id,
    selector: 'OpenShipmentsByCustomerFilterComponent',
    templateUrl: './OpenShipmentsByCustomerFilterComponent.html',
    inputs: ['ReportsPreview']
})

export class OpenShipmentsByCustomerFilterComponent extends BaseComponent implements OnInit {
    public ReportsPreview: ReportsPreviewComponent;
    public ValidationErrorsList: string[];
    public ObjectTableName: string = "Report";
    public DataContext: OpenShipmentsByCustomerFilterComponent = this;

    constructor() {
        super();
    }

    ngOnInit() {

    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;
    }

    public CustomerId: string; 
    queryFilterItems: QueryFilterItem[];
    queryFilterItem: QueryFilterItem;
    RunReport(isloading: boolean) {
        this.ValidationErrorsList = [];        

        if (this.ValidationErrorsList.length == 0) {
            this.queryFilterItems = new Array<QueryFilterItem>();

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "CustomerId";
            this.queryFilterItem.FieldValue = this.CustomerId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);

            var reportFliter = new ReportFliter();
            reportFliter.Tenant = SessionInfo.LoggedUserTenant;
            reportFliter.QueryFilterItemLists = this.queryFilterItems;
            reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
            reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
            reportFliter.ReportCode = this.ReportsPreview.Report.Code;
            reportFliter.NumberOfPage = 1;
            reportFliter.ProcessType = "GenerateReport";
            
            this.ReportsPreview.GenerateReport(reportFliter, isloading);

            this.ReportsPreview.CleanPartnersObslist();
            if (!AppTool.IsNullOrEmpty(this.CustomerId)) this.ReportsPreview.AddPartner("Partner", this.CustomerId);
        }
    }
}