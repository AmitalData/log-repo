import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ReportsPreviewComponent } from '../../../Components/ReportsPreviewComponent';
import { SessionInfo } from '../../../../Infrastructure/Utilities/SessionInfo';
import { ReportFliter } from '../../../Components/Filters/ReportFliter';
import { QueryFilterItem } from '../../../Components/Filters/QueryFilterItem';
import { Component } from '@angular/core';
import { DateTool } from '../../../../Infrastructure/Tools';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DashBoardFilters } from '../../../../Infrastructure/DataContracts/Dashboard/DashboardFilters';

@Component({
    moduleId: module.id,
    selector: 'UnicargoExportReportFilterComponent',
    templateUrl: './UnicargoExportReportFilterComponent.html',
    inputs: ['ReportsPreview']
})

export class UnicargoExportReportFilterComponent extends BaseComponent {
    public ReportsPreview: ReportsPreviewComponent;
    reportFliter: ReportFliter;
    public ObjectTableName: string = "Report";
    public DataContext: UnicargoExportReportFilterComponent = this;
    constructor() {
        super();
    }




    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;
        this.RunReport(false);
    }


    RunReport(isloading: boolean) {
        if (isloading) {
            this.reportFliter = new ReportFliter();
            this.reportFliter.Tenant = SessionInfo.LoggedUserTenant;
            this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
            this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
            this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
            this.reportFliter.NumberOfPage = 1;
            this.reportFliter.ProcessType = "GenerateReport";
            this.ReportsPreview.CleanPartnersObslist();
            this.ReportsPreview.GenerateReport(this.reportFliter, isloading);
        }
    }
}
