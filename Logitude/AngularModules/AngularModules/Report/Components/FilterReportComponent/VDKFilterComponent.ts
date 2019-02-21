import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ReportsPreviewComponent } from '../../Components/ReportsPreviewComponent';
import { SessionInfo } from '../../../Infrastructure/Utilities/SessionInfo';
import { ReportFliter } from '../../Components/Filters/ReportFliter';
import { QueryFilterItem } from '../../Components/Filters/QueryFilterItem';
import { Component } from '@angular/core';
import { DateTool } from '../../../Infrastructure/Tools';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';

@Component({
    moduleId: module.id,
    selector: 'VDKFilterComponent',
    templateUrl: './VDKFilterComponent.html',
    inputs: ['ReportsPreview']
})

export class VDKFilterComponent extends BaseComponent {
    public ReportsPreview: ReportsPreviewComponent;
    reportFliter: ReportFliter;
    public ObjectTableName: string = "Report";
    public DataContext: VDKFilterComponent = this;
    public ValidationErrorsList: string[] = [];
    queryFilterItems: QueryFilterItem[];
    queryFilterItem: QueryFilterItem;
    public SupplierValues: string = "CS";
    ToDate: Date;
    FromDate: Date;
    private shipmentCustomerTypeCode: string;
    get ShipmentCustomerTypeCode() { return this.shipmentCustomerTypeCode; }
    set ShipmentCustomerTypeCode(newValue: string) {
        if (this.shipmentCustomerTypeCode != newValue) {
            this.shipmentCustomerTypeCode = newValue;
        }
    }

    private customerId: string;
    get CustomerId() { return this.customerId; }
    set CustomerId(newValue: string) {
        if (this.customerId != newValue) {
            this.customerId = newValue;
        }
    }

    private supplierId: string;
    get SupplierId() { return this.supplierId; }
    set SupplierId(newValue: string) {
        if (this.supplierId != newValue) {
            this.supplierId = newValue;
        }
    }


    private branchId: string;
    get BranchId() { return this.branchId; }
    set BranchId(newValue: string) {
        if (this.branchId != newValue) {
            this.branchId = newValue;
        }
    }


    private entityStatus: string;
    get EntityStatus() { return this.entityStatus; }
    set EntityStatus(newValue: string) {
        if (this.entityStatus != newValue) {
            this.entityStatus = newValue;
        }
    }


    
    

    constructor() {
        super();
        if (SessionLocator.TenantPM.AllowAgentInCustomersLOV) {
            this.SupplierValues = "CS,AG";
        }
        else {
            this.SupplierValues = "CS";
        }
    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;
        this.FromDate = DateTool.GetCurrentDateAsUtc();
        this.FromDate.setMonth(this.FromDate.getMonth() - 1);
        this.ToDate = DateTool.GetCurrentDateAsUtc();
        this.RunReport(false);
    }


    RunReport(isloading: boolean) {
        if (isloading) {
        this.ValidationErrorsList = [];
        if (this.FromDate == null) {
            this.ValidationErrorsList.push("From Date is required");
        }

       

            if (this.ValidationErrorsList.length == 0) {
                this.queryFilterItems = new Array<QueryFilterItem>();

                this.queryFilterItem = new QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "FromDate";
                this.queryFilterItem.FieldValue = this.FromDate;
                this.queryFilterItem.FieldDataType = "Date";
                this.queryFilterItems.push(this.queryFilterItem);

                this.queryFilterItem = new QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "ToDate";
                this.queryFilterItem.FieldValue = this.ToDate;
                this.queryFilterItem.FieldDataType = "Date";
                this.queryFilterItems.push(this.queryFilterItem);

                this.queryFilterItems.push(new QueryFilterItem("BranchId", this.BranchId, "String"));
                this.queryFilterItems.push(new QueryFilterItem("CustomerId", this.CustomerId, "String"));
                this.queryFilterItems.push(new QueryFilterItem("EntityStatus", this.EntityStatus, "String"));
                this.queryFilterItems.push(new QueryFilterItem("SupplierId", this.supplierId, "String"));
                
                this.reportFliter = new ReportFliter();
                this.reportFliter.Tenant = SessionInfo.LoggedUserTenant;
                this.reportFliter.QueryFilterItemLists = this.queryFilterItems;
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
}
