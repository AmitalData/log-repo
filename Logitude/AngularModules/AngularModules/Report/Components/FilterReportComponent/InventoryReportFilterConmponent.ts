

declare var System: any;
declare var window: any;
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ReportsPreviewComponent} from '../../Components/ReportsPreviewComponent';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {ReportFliter} from '../../Components/Filters/ReportFliter';
import {QueryFilterItem} from '../../Components/Filters/QueryFilterItem';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {Component, OnInit, Output, ElementRef, EventEmitter}  from '@angular/core';
import {FormBuilder, FormGroup, FormsModule} from '@angular/forms';
import {CodeNameClass} from './CodeNameClass';
import {AppTool} from '../../../Infrastructure/Tools';
import { isNullOrUndefined } from 'util';
@Component({
    
    selector: 'InventoryReportFilterConmponent',
    templateUrl: './InventoryReportFilterConmponent.html',
    inputs: ['ReportsPreview']
})

export class InventoryReportFilterConmponent extends BaseComponent implements OnInit {
    @Output() RunReportEvent: EventEmitter<ReportFliter> = new EventEmitter<ReportFliter>();

    public ReportsPreview: ReportsPreviewComponent;
    WarehouseId: string;
    CustomerId: string;
    public myForm: FormGroup;
    reportFliter: ReportFliter;
    queryFilterItem: QueryFilterItem;
    public ObjectTableName: string = "Report";
    ShipperConsigneeId: string;
    public DataContext: InventoryReportFilterConmponent = this;
    public DaysinWarehouseFilterItemSource: Array<CodeNameClass>;
    SelectedItemDaysinWarehouseFilter: CodeNameClass;
    public DaysInWarehouse: number;
    public ValidationErrorsList: string[];
    public IsDaysInWarehouseRequired: boolean = false;

    constructor(fb: FormBuilder) {
        super();
        this.myForm = fb.group({});

    }

    InitializeComponent(myReportsPreview: ReportsPreviewComponent) {
        this.ReportsPreview = myReportsPreview;
        //this.RunReport(false);
    }

    ngOnInit() {
        this.FillDaysinWarehouseFilterItemSource();
        this.ValidationErrorsList = [];
    }

    FillDaysinWarehouseFilterItemSource() {
        this.DaysinWarehouseFilterItemSource = [];
        this.DaysinWarehouseFilterItemSource.push(new CodeNameClass("Empty", ""));
        this.DaysinWarehouseFilterItemSource.push(new CodeNameClass("Equals", "Equal to"));
        this.DaysinWarehouseFilterItemSource.push(new CodeNameClass("NotEquals", "Not Equal to"));
        this.DaysinWarehouseFilterItemSource.push(new CodeNameClass("GreaterThan", "Greater than"));
        this.DaysinWarehouseFilterItemSource.push(new CodeNameClass("Lessthan", "Less than"));
        this.DaysinWarehouseFilterItemSource.push(new CodeNameClass("GreaterThanOREqualTo", "Greater than or equal to"));
        this.DaysinWarehouseFilterItemSource.push(new CodeNameClass("LessThanOrEqualTo", "Less than or equal to"));
    }

    onSelectedItemDaysinWarehouseFilterChange(item) {
        this.SelectedItemDaysinWarehouseFilter = item;
        if (item.Code == "Empty") {
            this.IsDaysInWarehouseRequired = false;
            this.ValidationErrorsList = [];
        }
        else {
            this.IsDaysInWarehouseRequired = true;
        }
    }

    onDaysInWarehouseChange(item) {
        if (isNullOrUndefined(item))
            this.ValidationErrorsList.push("Days In Warehouse Field Required");
        else
            this.ValidationErrorsList = [];
    }


    GetQueryFilterItems() {

        var queryFilterItems = new Array<QueryFilterItem>();

        if (!AppTool.IsNullOrEmpty(this.CustomerId)) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "CustomerId";
            this.queryFilterItem.FieldValue = this.CustomerId;
            this.queryFilterItem.Operator = "Equals";
            queryFilterItems.push(this.queryFilterItem);
        }

        if (!AppTool.IsNullOrEmpty(this.WarehouseId)) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "WarehouseId";
            this.queryFilterItem.FieldValue = this.WarehouseId;
            this.queryFilterItem.Operator = "Equals";
            queryFilterItems.push(this.queryFilterItem);
        }

        if (!AppTool.IsNullOrEmpty(this.ShipperConsigneeId)) {
            this.queryFilterItem = new QueryFilterItem();
            this.queryFilterItem.DisplayInList = false;
            this.queryFilterItem.FieldName = "ShipperConsigneeId";
            this.queryFilterItem.FieldValue = this.ShipperConsigneeId;
            this.queryFilterItem.Operator = "Equals";
            queryFilterItems.push(this.queryFilterItem);
        } 

        if (this.SelectedItemDaysinWarehouseFilter) {
            if (this.SelectedItemDaysinWarehouseFilter.Code != "Empty") {
                this.queryFilterItem = new QueryFilterItem();
                this.queryFilterItem.DisplayInList = false;
                this.queryFilterItem.FieldName = "DaysInWarehouse";
                this.queryFilterItem.FieldValue = this.DaysInWarehouse;
                this.queryFilterItem.Operator = this.SelectedItemDaysinWarehouseFilter.Code;
                queryFilterItems.push(this.queryFilterItem);
            }
        }

        return queryFilterItems;
    }
    SetQueryFilterItems(queryFilterItems: Array<QueryFilterItem>,isSchedulerReport:boolean=true) {
        if (queryFilterItems) {
            queryFilterItems.forEach(queryFilterItem => {
                this.SetFilterItem(queryFilterItem);
            });
        }
    }

    private SetFilterItem(queryFilterItem: QueryFilterItem) {
        this.FillDaysinWarehouseFilterItemSource();
        if (queryFilterItem) {
            if (queryFilterItem.FieldName == "CustomerId") {
                this.CustomerId = queryFilterItem.FieldValue;
            }
            else if (queryFilterItem.FieldName == "WarehouseId") {
                this.WarehouseId = queryFilterItem.FieldValue;
            }
            else if (queryFilterItem.FieldName == "ShipperConsigneeId") {
                this.ShipperConsigneeId = queryFilterItem.FieldValue;
            }
            else if (queryFilterItem.FieldName == "DaysInWarehouse") {
                this.DaysInWarehouse = queryFilterItem.FieldValue;
                this.SelectedItemDaysinWarehouseFilter = this.DaysinWarehouseFilterItemSource.filter(d => d.Code == queryFilterItem.Operator)[0];
                if (this.SelectedItemDaysinWarehouseFilter.Code != "Empty") {
                    this.IsDaysInWarehouseRequired = true;
                }
            }
        }

    }


    RunReport(isloading: boolean) {
        if (this.IsDaysInWarehouseRequired && isNullOrUndefined(this.DaysInWarehouse)) {
            this.ValidationErrorsList.push("Days In Warehouse Field Required");
        }
        if (this.ValidationErrorsList.length > 0) {
            return;
        }


        this.reportFliter = new ReportFliter();
        this.reportFliter.Tenant = SessionInfo.LoggedUserTenant;
        this.reportFliter.QueryFilterItemLists = this.GetQueryFilterItems();
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


        this.RunReportEvent.emit(this.reportFliter);
      //  this.ReportsPreview.GenerateReport(this.reportFliter, isloading);


    }

 



}
