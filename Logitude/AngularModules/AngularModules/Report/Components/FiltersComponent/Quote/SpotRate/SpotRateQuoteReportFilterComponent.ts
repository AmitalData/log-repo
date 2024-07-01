import { Component } from '@angular/core';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { QueryFilterItem } from '../../../../Components/Filters/QueryFilterItem';
import { ReportsPreviewComponent } from 'Report/Components/ReportsPreviewComponent';
import { TenantPM } from 'Common/EntityPMs/TenantPM';
import { ReportFliter } from 'Report/Components/Filters/ReportFliter';
import { AppTool, DateTool } from 'Infrastructure/Tools';
import { TextCodeTranslator } from 'Infrastructure/Utilities/TextCodeTranslator';
import { EntityResourceService } from 'Infrastructure/Services/EntityResourceService';
import { CardExtendedPMService } from 'Common/Services/ExtendedPMs/CardExtendedPMService';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';

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
    entityResourceService: EntityResourceService = new EntityResourceService();

    isReady: boolean = false;
    public RunReportTitle: string;
    public OpenDate: Date;
    public ExpirationDate: Date;
    public CustomerId: string = null;
    public SalesmanId: string = null;
    public IsSchedulerReport: boolean;
    GLAccountChanged: boolean;

    constructor() {
        super();
        this.entityResourceService.getEntityResourceByTableName("GLAccount").subscribe(response => { this.isReady = true; this.SetRunReportTitle(); });

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
        reportFliter.QueryFilterItemLists = this.GetQueryFilterItems ();

        this.ReportsPreview.GenerateReport(reportFliter, isloading);

    }


    private GetQueryFilterItems () {
        var myFilterItems: QueryFilterItem[] = [];
        myFilterItems.push(new QueryFilterItem("CustomerId", this.CustomerId));
        myFilterItems.push(new QueryFilterItem("SalesmanId", this.SalesmanId));
        myFilterItems.push(new QueryFilterItem("OpenDateGraterThan", this.OpenDate,null, 'Date'));
        myFilterItems.push(new QueryFilterItem("ExpirationDateLessThan", this.ExpirationDate,null, 'Date'));
        return myFilterItems;
    }

    SetCustomerIdFilter(queryFilterItem: QueryFilterItem) {
        if (queryFilterItem.FieldName == "CustomerId") {
            this.CustomerId = queryFilterItem.FieldValue;
            this.GLAccountChanged = true;
        }
    }
    SetSalesmanIdFilter(queryFilterItem: QueryFilterItem) {
        if (queryFilterItem.FieldName == "SalesmanId") {
            this.SalesmanId = queryFilterItem.FieldValue;
        }
    }
    SetOpenDateFilter(queryFilterItem: QueryFilterItem) {
        if (queryFilterItem.FieldName == "OpenDateGraterThan") {
            this.OpenDate = queryFilterItem.FieldValue;
        }
    }
    SetExpirationDateLessThanFilter(queryFilterItem: QueryFilterItem) {
        if (queryFilterItem.FieldName == "ExpirationDateLessThan") {
            this.ExpirationDate = queryFilterItem.FieldValue;
        }
    }

    SetQueryFilterItems(queryFilterItems: Array<QueryFilterItem>) { //For Scheduler Report
        this.IsSchedulerReport = true;
        if (queryFilterItems) {
            queryFilterItems.forEach(queryFilterItem => {
                this.SetFilterItem(queryFilterItem);
            });
        }
    }
    private SetFilterItem(queryFilterItem: QueryFilterItem) {
        if (queryFilterItem) {
            this.SetExpirationDateLessThanFilter(queryFilterItem);
            this.SetOpenDateFilter(queryFilterItem);
            this.SetSalesmanIdFilter(queryFilterItem);
            this.SetCustomerIdFilter(queryFilterItem);

        }

    }
    SetRunReportTitle() {
        if (this.isReady) {
            if (this.IsSchedulerReport) {
                this.RunReportTitle = TextCodeTranslator.Translate("AgingReport.O.PreviewReport");
            }
            else {
                this.RunReportTitle = TextCodeTranslator.Translate("AgingReport.O.RunReport");
            }

        }


    }
    ValidateSelectedFilters (){
        return true;
    }
    IsPartnersChanged(SelectedTab) {
        if (SelectedTab == '2')
            this.GLAccountChanged = false;
        return this.GLAccountChanged;
    }

    PrepareContactList() {

        //var glAccountId = this.GetLookUpFieldValue(this.Customer);
        if (this.CustomerId != null) {
            this.GLAccountCardContacts(this.CustomerId);

        }
    }
    GetLookUpFieldValue(field) {
        if (field) {
            if (field[0]["@nil"] != "true")
                return field;
        }
        return null
    }
    GLAccountCardContacts(glAccountId:string) {
        var cardExtendedPMService = new CardExtendedPMService();
        cardExtendedPMService.GetAllConnectedPartnersByGLAccountId(glAccountId).subscribe((response: ServiceResponse) => {
            if (!response.HasError) {
                var allContacts = response.Result;
                if (allContacts != null && allContacts.length > 0) {
                    allContacts.forEach(contact => {
                        if (!AppTool.IsNullOrEmpty(contact)) this.ReportsPreview.AddPartner(contact.PartnerName, contact.PartnerId);
                    });
                    this.ReportsPreview.PartnersObslist.reverse();
                }
            }
        });
    }
    GetMainCustomerFieldName() {
        return 'CustomerId';
    }
}
