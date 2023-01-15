import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { DashboardGlobalPresetFilterList } from 'DashboardModule/EntityLists/DashboardGlobalPresetFilterList';
import { DashboardPM } from 'DashboardModule/EntityPMs/DashboardPM';
import { DashboardGlobalPresetFilterListService } from 'DashboardModule/Services/StandardLists/DashboardGlobalPresetFilterListService';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';
import { GlobalFilterItem } from './GlobalFilterItem';

@Component({
    selector: 'GlobalFilter',
    templateUrl: './GlobalFilterComponent.html',
    styleUrls: ['./GlobalFilter.scss']
})

export class GlobalFilterComponent implements OnInit {
    @Input() public Dashboard: DashboardPM;
    @Output() ApplyFilters = new EventEmitter<any[]>();
    public FilterItems: GlobalFilterItem[] = [];

    private DashboardGlobalPresetFilterListService: DashboardGlobalPresetFilterListService;
    public ShowFilters: boolean = true;

    constructor() {
        this.DashboardGlobalPresetFilterListService = new DashboardGlobalPresetFilterListService();
    }

    ngOnInit() {
        this.GetPresetFitlers();
    }

    GetPresetFitlers() {
        this.DashboardGlobalPresetFilterListService.getAll().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse || myResponse.HasError) return;
            this.AddPresetFitlers(myResponse.Result);
        });
    }

    AddPresetFitlers(filters: DashboardGlobalPresetFilterList[]) {
        filters.forEach(presetFilter => {
            var filter = new GlobalFilterItem();
            filter.FieldId = presetFilter.Code;
            filter.DataTypeCode = presetFilter.DataTypeCode;
            filter.DisplayName = presetFilter.DisplayName;
            filter.Operator = "";
            filter.IsPreset = true;
            filter.JoinedTableName = presetFilter.JoinedTableName;
            filter.JoinedTableDisplayField = presetFilter.JoinedTableDisplayField;
            filter.CanSearch = presetFilter.CanSearch;
            filter.IsMultiSelect = presetFilter.IsMultiSelect;
            this.FilterItems.push(filter);
        })

    }


    FilterExist(): boolean {
        if (this.FilterItems && this.FilterItems.length > 0) return true;
        return false;
    }

    ClearFiltersClick() {
        if (!this.FilterExist()) return;
        if (this.FilterItems) this.FilterItems.forEach(element => { this.ClearFilter(element); });
        this.ShowFilters = false;
        setTimeout(() => {
            this.ShowFilters = true
        }, 10);
        this.ApplyFilters.emit(null);
    }

    private ClearFilter(element: GlobalFilterItem) {
        element.FieldValue = null;
        element.FieldValue2 = null;
        element.FieldValue3 = null;
        element.DateGroupCode = null;
        element.Operator = null;
        element.CompareWithPrevious = false;
    }

    ApplyFiltersClick() {
        if (!this.FilterExist()) return;
        var filterItems = [];
        this.AddFilterItems(this.FilterItems, filterItems, true);
        this.ApplyFilters.emit(filterItems);
    }

    AddFilterItems(filters: GlobalFilterItem[], filterItems: any, isCommon: boolean = false): any {
        if (!filters || filters.length == 0) return filterItems;
        filters.forEach(element => {
            this.SetPresetFilterOperator(element);
            if (this.FilterValueEmpty(element)) return;
            filterItems.push(element);
        });
        return filterItems;
    }

    SetPresetFilterOperator(filterItem: GlobalFilterItem) {
        if (!filterItem.IsPreset) return;
        if (filterItem.DataTypeCode == "LookUp") {
            if (filterItem.IsMultiSelect) filterItem.Operator = "InListExact";
            else filterItem.Operator = "Equal";
        }
    }

    FilterValueEmpty(element: GlobalFilterItem): boolean {
        if (element.Operator == "IsEmpty" || element.Operator == "IsNotEmpty") return false;
        if (element.Operator != "Previous" && element.Operator != "Next" && element.Operator != "Current" && (!element.FieldValue || element.FieldValue == "")) return true;
        if ((element.Operator == "Previous" || element.Operator == "Next") && (!element.FieldValue3 || element.FieldValue3 == "")) return true;
        if (element.Operator == "Between" && (!element.FieldValue2 || element.FieldValue2 == "" || element.FieldValue2 <= element.FieldValue)) return true;
        return false;
    }
}