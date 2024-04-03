import { Component } from '@angular/core';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { QueryFilterItem } from '../../../../Components/Filters/QueryFilterItem';
import { ReportsPreviewComponent } from 'Report/Components/ReportsPreviewComponent';
import { TenantPM } from 'Common/EntityPMs/TenantPM';
import { ReportFliter } from 'Report/Components/Filters/ReportFliter';
import { DateTool } from 'Infrastructure/Tools';

@Component({

    templateUrl: './SpotRateQuoteReportFilterComponent.html',
})

export class SpotRateQuoteReportFilterComponent extends BaseComponent {
    public ReportsPreview: ReportsPreviewComponent;
    public ValidationErrorsList: string[] = [];
    public TenantPM: TenantPM;

    queryFilterItems: QueryFilterItem[];
    queryFilterItem: QueryFilterItem;
    public DataContext: SpotRateQuoteReportFilterComponent = this;

    public OpenDate: Date;
    public ExpirationDate: Date;
    public CustomerId: string = null;
    public SalesmanId: string = null;
    
    constructor() {
        super();
    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;
        this.TenantPM = SessionLocator.TenantPM;
        this.SetDates();
    }


    private SetDates() {
        this.OpenDate = DateTool.GetCurrentDateAsUtc();
        this.OpenDate.setMonth(this.OpenDate.getMonth() - 1);
    }

    RunReport(isloading: boolean) {
        var reportFliter = new ReportFliter();
        reportFliter.Tenant = SessionLocator.Tenant;
        reportFliter.QueryFilterItemLists = this.queryFilterItems;
        reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
        reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
        reportFliter.ReportCode = this.ReportsPreview.Report.Code;
        reportFliter.NumberOfPage = 1;
        reportFliter.ProcessType = "GenerateReport";
        reportFliter.QueryFilterItemLists = this.BuildQueryFilterItems();

        this.ReportsPreview.GenerateReport(reportFliter, isloading);

    }


    private BuildQueryFilterItems() {
        var myFilterItems: QueryFilterItem[] = [];
        myFilterItems.push(new QueryFilterItem("CustomerId", this.CustomerId));
        myFilterItems.push(new QueryFilterItem("SalesmanId", this.SalesmanId));
        myFilterItems.push(new QueryFilterItem("OpenDateGraterThan", this.OpenDate));
        myFilterItems.push(new QueryFilterItem("ExpirationDateLessThan", this.ExpirationDate));
        return myFilterItems;
    }
}
