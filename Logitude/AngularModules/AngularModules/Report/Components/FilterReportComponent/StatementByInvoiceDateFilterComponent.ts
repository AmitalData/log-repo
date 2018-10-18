import {Component, OnInit, Output, ElementRef}  from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReportsPreviewComponent} from '../../Components/ReportsPreviewComponent';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ReportFliter} from '../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';
import {AppTool} from '../../../Infrastructure/Tools';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
@Component({
    moduleId: module.id,
    selector: 'StatementByInvoiceDateFilterComponent',
    templateUrl: './StatementByInvoiceDateFilterComponent.html',
    inputs: ['ReportsPreview']
})

export class StatementByInvoiceDateFilterComponent extends BaseComponent   {
    public ReportsPreview: ReportsPreviewComponent;
    public CustomerId: string = null;    
    public queryFilterItems: QueryFilterItem[];
    public ObjectTableName: string = "Report";
    reportFliter: ReportFliter;
    queryFilterItem: QueryFilterItem;
    public DataContext: StatementByInvoiceDateFilterComponent = this;

    constructor() {
        super();
    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;
    }

    
    
    RunReport(isloading: boolean) {
        this.queryFilterItems = [];
        this.queryFilterItems = new Array<QueryFilterItem>();

        if (AppTool.IsNullOrEmpty(this.CustomerId)) {
            var window: MessageWindow = new MessageWindow();
            window.Title = "Message";
            window.Show("Please select a partner then press Run Report");
        }
        else {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "CustomerId";
            this.queryFilterItem.FieldValue = this.CustomerId;
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
            if (!AppTool.IsNullOrEmpty(this.CustomerId)) this.ReportsPreview.AddPartner("Partner", this.CustomerId);


            this.ReportsPreview.GenerateReport(this.reportFliter, isloading);
        }
    }
}