import {Component, OnInit, Output, EventEmitter}  from '@angular/core';
import {AppTool} from '../../../../Infrastructure/Tools';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ReportFliter} from '../../Filters/ReportFliter';
import {QueryFilterItem} from '../../Filters/QueryFilterItem';
import {ReportsPreviewComponent} from '../../ReportsPreviewComponent';

@Component({
    
    templateUrl: './ExportDeclarationReportFilterComponent.html',
})

export class ExportDeclarationReportFilterComponent extends BaseComponent {

    public ReportsPreview: ReportsPreviewComponent;
    public ValidationErrorsList: string[];
    public ObjectTableName: string = "Report";
    public DataContext: ExportDeclarationReportFilterComponent = this;

    constructor() {
        super();
    }

    ngOnInit() {

    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;        
    }

    public UserId: string; 

    queryFilterItems: QueryFilterItem[];
    queryFilterItem: QueryFilterItem;
    RunReport(isloading: boolean) {
        this.ValidationErrorsList = [];

        if (this.ValidationErrorsList.length == 0) {
            this.queryFilterItems = new Array<QueryFilterItem>();

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "UserId";
            this.queryFilterItem.FieldValue = this.UserId;
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
}