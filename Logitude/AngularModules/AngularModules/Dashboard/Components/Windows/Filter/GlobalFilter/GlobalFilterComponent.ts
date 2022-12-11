import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { DashboardPM } from 'DashboardModule/EntityPMs/DashboardPM';
import { CodeNameClass } from 'Infrastructure/DataContracts/CodeNameClass';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { DashboardGlobalFilterPM } from 'DashboardModule/EntityPMs/DashboardGlobalFilterPM';

@Component({
    selector: 'GlobalFilter',
    templateUrl: './GlobalFilterComponent.html'
})

export class GlobalFilterComponent implements OnInit {
    @Input() public Dashboard: DashboardPM;
    @Output() ApplyFilters = new EventEmitter<string>();

    public CommonFilters: GlobalFilterItem[];
    public DatasetFilters: GlobalFilterItem[];
    public FilterTypes: CodeNameClass[] = [];
    public CommonFilterFields: CodeNameClass[] = [];

    constructor() {
        this.CommonFilters = [];
        this.DatasetFilters = [];
    }

    ngOnInit() {
        this.BuildFilterTypesList();
        this.BuildCommonFilterFieldsList();
        this.BuildGlobalFilters();
    }

    private BuildFilterTypesList() {
        this.FilterTypes = [];

        this.FilterTypes.push(new CodeNameClass("COMN", "Common Filter"));
        this.FilterTypes.push(new CodeNameClass("DATA", "Dataset Filter"));
    }
    private BuildCommonFilterFieldsList() {
        this.CommonFilterFields = [];

        this.CommonFilterFields.push(new CodeNameClass("CreateDate", "Create Date", "Date"));
        this.CommonFilterFields.push(new CodeNameClass("Number", "Number", "String"));
    }
    private BuildGlobalFilters() {
        this.Dashboard.DashboardGlobalFilters.sort((a, b) => { return a.LineNumber - b.LineNumber }).forEach(item => {
            if (item.IsCommonFilter)
                this.CommonFilters.push(new GlobalFilterItem(item, this));
            else {
                this.DatasetFilters.push(new GlobalFilterItem(item, this));
            }
        });
    }

    ClearFiltersClick() {
        if (!this.FilterExist()) return;
        if (this.CommonFilters) this.CommonFilters.forEach(element => { this.ClearFilter(element); });
        if (this.DatasetFilters) this.DatasetFilters.forEach(element => { this.ClearFilter(element); });
        this.ApplyFilters.emit(null);
    }

    private ClearFilter(element: GlobalFilterItem) {
        element.FieldValue = null;
        element.FieldValue2 = null;
        element.FieldValue3 = null;
    }

    FilterExist(): boolean {
        if (this.CommonFilters && this.CommonFilters.length > 0) return true;
        if (this.DatasetFilters && this.DatasetFilters.length > 0) return true;
        return false;
    }

    ApplyFiltersClick() {
        if (!this.FilterExist()) return;
        var filterItems = [];
        this.AddFilterItems(this.CommonFilters, filterItems, true);
        this.AddFilterItems(this.DatasetFilters, filterItems, false);
        this.ApplyFilters.emit(JSON.stringify(filterItems));
    }

    AddFilterItems(filters: GlobalFilterItem[], filterItems: any, isCommon: boolean = false): any {
        if (!filters || filters.length == 0) return filterItems;
        filters.forEach(element => {
            if (this.FilterValueEmpty(element)) return;
            filterItems.push(this.MapFilterToDashboardFilter(element, isCommon));
        });
        return filterItems;
    }

    FilterValueEmpty(element: GlobalFilterItem): boolean {
        if (element.FilterOperator == "IsEmpty" || element.FilterOperator == "IsNotEmpty") return false;
        if (element.FilterOperator != "Previous" && element.FilterOperator != "Next" && element.FilterOperator != "Current" && (!element.FieldValue || element.FieldValue == "")) return true;
        if ((element.FilterOperator == "Previous" || element.FilterOperator == "Next") && (!element.FieldValue3 || element.FieldValue3 == "")) return true;
        if (element.FilterOperator == "Between" && (!element.FieldValue2 || element.FieldValue2 == "" || element.FieldValue2 <= element.FieldValue)) return true;
        return false;
    }

    MapFilterToDashboardFilter(element: GlobalFilterItem, isCommon: boolean): any {
        return {
            FieldId: element.DataSetFieldId,
            DataSetId: element.DataSetId,
            FieldName: isCommon? element.CommonFilterField : element.EntityPM.FieldCode,
            IsCommon: isCommon,
            Operator: element.FilterOperator,
            DateGroupCode : element.DateGroupCode,
            FieldDataType: element.DataTypeCode,
            FieldValue: element.FieldValue,
            FieldValue2: element.FieldValue2,
            FieldValue3: element.FieldValue3,
        }
    }

}

export class GlobalFilterItem extends BaseComponent {
    public EntityPM: DashboardGlobalFilterPM;
    public Operators: CodeNameClass[] = [];
    public ObjectTableName: string = "DashboardGlobalFilter";
    public DataContext = this;
    public FieldValue: any;
    public FieldValue2: any;
    public FieldValue3: any;
    public DateGroupCode: string;

    constructor(filter: DashboardGlobalFilterPM, public fatherComponent: GlobalFilterComponent) {
        super();
        this.EntityPM = filter;

        this.SetUIProperties();
        this.FillOperators(this.EntityPM.DataTypeCode);
        this.SetFilterType();
        this.SetFilterField();
        this.SetOperator();
    }

    private SetUIProperties() {
        this.UIProperties.SetEnabled("DataSetId", this.ObjectTableName, false);
        this.UIProperties.SetEnabled("DataSetFieldId", this.ObjectTableName, false);
    }
    private SetFilterType() {
        this.selectedFilterType = new CodeNameClass();
        this.selectedFilterType = this.fatherComponent.FilterTypes.filter(d => d.Code == (this.EntityPM.IsCommonFilter ? "COMN" : "DATA"))[0];
    }
    private SetFilterField() {
        if (this.EntityPM.IsCommonFilter) {
            this.selectedCommonFilterField = new CodeNameClass();
            this.selectedCommonFilterField = this.fatherComponent.CommonFilterFields.filter(d => d.Code == this.EntityPM.CommonFilterField)[0];
        }
    }
    private SetOperator() {
        this.selectedOperator = new CodeNameClass();
        this.selectedOperator = this.Operators.filter(d => d.Code == this.EntityPM.FilterOperator)[0];
    }

    private FillOperators(dataTypeCode: string) {
        this.Operators = [];
        switch (dataTypeCode) {
            case "DateTime":
            case "Date":
                this.Operators.push(new CodeNameClass("GreaterThan", "After"));
                this.Operators.push(new CodeNameClass("LessThan", "Before"));
                this.Operators.push(new CodeNameClass("Previous", "Previous"));
                this.Operators.push(new CodeNameClass("Current", "Current"));
                this.Operators.push(new CodeNameClass("Next", "Next"));
                this.Operators.push(new CodeNameClass("Between", "Between"));
                break;

            case "Integer":
            case "Decimal":
            case "Double":
                this.Operators.push(new CodeNameClass("Equal", "Equal"));
                this.Operators.push(new CodeNameClass("NotEqual", "Does Not Equal"));
                this.Operators.push(new CodeNameClass("GreaterThan", "Greater Than"));
                this.Operators.push(new CodeNameClass("LessThan", "Less Than"));
                this.Operators.push(new CodeNameClass("GreaterThanOrEqual", "Greater Than Or Equal"));
                this.Operators.push(new CodeNameClass("LessThanOrEqual", "Less Than Or Equal"));
                this.Operators.push(new CodeNameClass("IsEmpty", "Is Empty"));
                this.Operators.push(new CodeNameClass("IsNotEmpty", "Is not Empty"));
                break;

            case "Boolean":
                this.Operators.push(new CodeNameClass("Equal", "Equal"));
                this.Operators.push(new CodeNameClass("IsEmpty", "Is Empty"));
                this.Operators.push(new CodeNameClass("IsNotEmpty", "Is not Empty"));
                break;

            case "LookUp":
                this.Operators.push(new CodeNameClass("Equal", "Equal"));
                this.Operators.push(new CodeNameClass("NotEqual", "Does Not Equal"));
                this.Operators.push(new CodeNameClass("IsEmpty", "Is Empty"));
                this.Operators.push(new CodeNameClass("IsNotEmpty", "Is not Empty"));
                break;

            default:
                this.Operators.push(new CodeNameClass("Equal", "Equal"));
                this.Operators.push(new CodeNameClass("NotEqual", "Does Not Equal"));
                this.Operators.push(new CodeNameClass("Contains", "Contains"));
                this.Operators.push(new CodeNameClass("NotContains", "Does Not Contain"));
                this.Operators.push(new CodeNameClass("IsEmpty", "Is Empty"));
                this.Operators.push(new CodeNameClass("IsNotEmpty", "Is not Empty"));
                break;
        }
    }

    private selectedFilterType: CodeNameClass;
    get SelectedFilterType() { return this.selectedFilterType; }
    set SelectedFilterType(value: CodeNameClass) {
        if (this.selectedFilterType != value) {
            this.selectedFilterType = value;
        }
    }

    private selectedCommonFilterField: CodeNameClass;
    get SelectedCommonFilterField() { return this.selectedCommonFilterField; }
    set SelectedCommonFilterField(value: CodeNameClass) {
        if (this.selectedCommonFilterField != value) {
            this.selectedCommonFilterField = value;
        }
    }

    private selectedOperator: CodeNameClass;
    get SelectedOperator() { return this.selectedOperator; }
    set SelectedOperator(value: CodeNameClass) {
        if (this.selectedOperator != value) {
            this.selectedOperator = value;
        }
    }

    get IsCommonFilter() { return this.EntityPM.IsCommonFilter; }
    set IsCommonFilter(value: boolean) {
        if (this.EntityPM.IsCommonFilter != value) {
            this.EntityPM.IsCommonFilter = value;
        }
    }

    get CommonFilterField() { return this.EntityPM.CommonFilterField; }
    set CommonFilterField(value: string) {
        if (this.EntityPM.CommonFilterField != value) {
            this.EntityPM.CommonFilterField = value;
        }
    }

    get DataSetId() { return this.EntityPM.DataSetId; }
    set DataSetId(value: string) {
        if (this.EntityPM.DataSetId != value) {
            this.EntityPM.DataSetId = value;
        }
    }

    get DataSetFieldId() { return this.EntityPM.DataSetFieldId; }
    set DataSetFieldId(value: string) {
        if (this.EntityPM.DataSetFieldId != value) {
            this.EntityPM.DataSetFieldId = value;
        }
    }

    get FilterOperator() { return this.EntityPM.FilterOperator; }
    set FilterOperator(value: string) {
        if (this.EntityPM.FilterOperator != value) {
            this.EntityPM.FilterOperator = value;
        }
    }

    get DataTypeCode() { return this.EntityPM.DataTypeCode; }
    set DataTypeCode(value: string) {
        if (this.EntityPM.DataTypeCode != value) {
            this.EntityPM.DataTypeCode = value;
        }
    }

    get LineNumber() { return this.EntityPM.LineNumber; }
    set LineNumber(value: number) {
        if (this.EntityPM.LineNumber != value) {
            this.EntityPM.LineNumber = value;
        }
    }

}
