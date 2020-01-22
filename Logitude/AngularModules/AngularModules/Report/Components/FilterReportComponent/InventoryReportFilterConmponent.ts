

declare var System: any;
declare var window: any;
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReportsPreviewComponent} from '../../Components/ReportsPreviewComponent';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ReportFliter} from '../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Component, OnInit, Output, ElementRef}  from '@angular/core';
import {FormBuilder, FormGroup, FormsModule} from '@angular/forms';
import {CodeNameClass} from './CodeNameClass';
import {AppTool} from '../../../Infrastructure/Tools';
@Component({
    moduleId: module.id,
    selector: 'InventoryReportFilterConmponent',
    templateUrl: './InventoryReportFilterConmponent.html',
    inputs: ['ReportsPreview']
})

export class InventoryReportFilterConmponent extends BaseComponent implements OnInit {
    public ReportsPreview: ReportsPreviewComponent;
    WarehouseId: string;
    CustomerId: string;
    public myForm: FormGroup;
    queryFilterItems: QueryFilterItem[];
    reportFliter: ReportFliter;
    queryFilterItem: QueryFilterItem;
    public ObjectTableName: string = "Report";
    ShipperConsigneeId: string;
    public DataContext: InventoryReportFilterConmponent = this;
    public DaysinWarehouseFilterItemSource: Array<CodeNameClass>;
    SelectedItemDaysinWarehouseFilter: CodeNameClass;
    public DaysInWarehouse: number = 0;

    constructor(fb: FormBuilder) {
        super();
        this.myForm = fb.group({});

    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;
        //this.RunReport(false);
    }

    ngOnInit() {

        this.DaysinWarehouseFilterItemSource = [];
        this.DaysinWarehouseFilterItemSource.push(new CodeNameClass("Equals","Equal to"));
        this.DaysinWarehouseFilterItemSource.push(new CodeNameClass("NotEquals", "Not Equal to"));
        this.DaysinWarehouseFilterItemSource.push(new CodeNameClass("GreaterThan", "Greater than"));
        this.DaysinWarehouseFilterItemSource.push(new CodeNameClass("Lessthan", "Less than"));
        this.DaysinWarehouseFilterItemSource.push(new CodeNameClass("GreaterThanOREqualTo", "Greater than or equal to"));
        this.DaysinWarehouseFilterItemSource.push(new CodeNameClass("LessThanOrEqualTo", "Less than or equal to"));
        this.SelectedItemDaysinWarehouseFilter = this.DaysinWarehouseFilterItemSource.filter(d => d.Code == "GreaterThan")[0];
    }




    RunReport(isloading: boolean) {

        this.queryFilterItems = new Array<QueryFilterItem>();

        if (!AppTool.IsNullOrEmpty(this.CustomerId)) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "CustomerId";
            this.queryFilterItem.FieldValue = this.CustomerId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }

        if (!AppTool.IsNullOrEmpty(this.WarehouseId)) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "WarehouseId";
            this.queryFilterItem.FieldValue = this.WarehouseId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }

        if (!AppTool.IsNullOrEmpty(this.ShipperConsigneeId)) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "ShipperConsigneeId";
            this.queryFilterItem.FieldValue = this.ShipperConsigneeId;
            this.queryFilterItem.Operator = "Equals";
            this.queryFilterItems.push(this.queryFilterItem);
        }
        if (this.SelectedItemDaysinWarehouseFilter) {

            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "DaysInWarehouse";
            this.queryFilterItem.FieldValue = this.DaysInWarehouse;
            this.queryFilterItem.Operator = this.SelectedItemDaysinWarehouseFilter.Code;
            this.queryFilterItems.push(this.queryFilterItem);
        }


       




        this.reportFliter = new ReportFliter();
        this.reportFliter.Tenant = SessionInfo.LoggedUserTenant;
        this.reportFliter.QueryFilterItemLists = this.queryFilterItems;
        this.reportFliter.FilterControlName = this.ReportsPreview.FilterControlName;
        this.reportFliter.ReportDocumentId = this.ReportsPreview.Report.ReportDocumentId;
        this.reportFliter.ReportCode = this.ReportsPreview.Report.Code;
        this.reportFliter.NumberOfPage = 1;
        this.reportFliter.ProcessType = "GenerateReport";

        // EntityPartner

        this.ReportsPreview.CleanPartnersObslist();

        if (!AppTool.IsNullOrEmpty(this.CustomerId)) {
            this.ReportsPreview.AddPartner("Customer", this.CustomerId);
        }

        this.ReportsPreview.GenerateReport(this.reportFliter, isloading);


    }

 



}