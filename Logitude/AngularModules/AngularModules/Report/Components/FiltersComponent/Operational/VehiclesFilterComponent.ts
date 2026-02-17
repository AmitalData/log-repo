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
    selector: 'VehiclesFilterComponent',
    templateUrl: './VehiclesFilterComponent.html',
    inputs: ['ReportsPreview']
})

export class VehiclesFilterComponent extends BaseComponent {
    public ReportsPreview: ReportsPreviewComponent;
    reportFliter: ReportFliter;
    public ObjectTableName: string = "Report";
    public DataContext: VehiclesFilterComponent = this;
    public ValidationErrorsList: string[] = [];
    queryFilterItems: QueryFilterItem[];
    queryFilterItem: QueryFilterItem;
    public SupplierValues: string = "CS";
    ToDate: Date;
    FromDate: Date;
    public DateTypeFilterList: DashBoardFilters[];


    constructor() {
        super();
        this.DateTypeFilterList = [];

        this.DateTypeFilterList.push(new DashBoardFilters("Create Date", "CreateDate"));
        this.DateTypeFilterList.push(new DashBoardFilters("Operational Date", "OperationalDate"));
        this.selectedDateTypeItem = this.DateTypeFilterList.filter(d => d.Index == "CreateDate")[0];

        if (SessionLocator.TenantPM.AllowAgentInCustomersLOV) {
            this.SupplierValues = "CS,AG";
        }
        else {
            this.SupplierValues = "CS";
        }
    }
    

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

    
    private packageTypeId: string;
    get PackageTypeId() { return this.packageTypeId; }
    set PackageTypeId(newValue: string) {
        if (this.packageTypeId != newValue) {
            this.packageTypeId = newValue;
        }
    }

    
    private selectedDateTypeItem: DashBoardFilters;
    get SelectedDateTypeItem() { return this.selectedDateTypeItem; }
    set SelectedDateTypeItem(value: DashBoardFilters) {
        if (this.selectedDateTypeItem != value) {
            this.selectedDateTypeItem = value;
        }
    }

    private selectedTransportFilterCountries: string = "All";
    get SelectedTransportFilterCountries() { return this.selectedTransportFilterCountries; }
    set SelectedTransportFilterCountries(newValue: string) {
        if (this.selectedTransportFilterCountries != newValue) {
            this.selectedTransportFilterCountries = newValue;
        }

    }

    private selectedDirectionFilterCountries: string = "All";
    get SelectedDirectionFilterCountries() { return this.selectedDirectionFilterCountries; }
    set SelectedDirectionFilterCountries(newValue: string) {
        if (this.selectedDirectionFilterCountries != newValue) {
            this.selectedDirectionFilterCountries = newValue;
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
            if (this.ToDate != null) {
                if (this.FromDate > this.ToDate) {
                    this.ValidationErrorsList.push("From Date cannot be greater than To Date");
                }
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

                this.queryFilterItems.push(new QueryFilterItem("Packagetype", this.PackageTypeId, "String"));
                this.queryFilterItems.push(new QueryFilterItem("BillToId", this.CustomerId, "String"));
                this.queryFilterItems.push(new QueryFilterItem("TransportMode", this.SelectedTransportFilterCountries, "String"));
                this.queryFilterItems.push(new QueryFilterItem("Direction", this.SelectedDirectionFilterCountries, "String"));

                if (this.SelectedDateTypeItem.Index == "CreateDate") {
                    this.queryFilterItems.push(new QueryFilterItem("IsByCreateDate", true, "boolean"));
                }
                else {
                    this.queryFilterItems.push(new QueryFilterItem("IsByCreateDate", false, "boolean"));
                }

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
