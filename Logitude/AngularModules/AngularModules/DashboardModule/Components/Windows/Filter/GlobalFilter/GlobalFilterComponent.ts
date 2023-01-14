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
        // if (this.CommonFilters && this.CommonFilters.length > 0) return true;
        // if (this.DatasetFilters && this.DatasetFilters.length > 0) return true;
        return false;
    }

    ApplyFiltersClick() {
        if (!this.FilterExist()) return;
        var filterItems = [];
        // this.AddFilterItems(this.CommonFilters, filterItems, true);
        // this.AddFilterItems(this.DatasetFilters, filterItems, false);
        this.ApplyFilters.emit(filterItems);
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
        // if (element.FilterOperator == "IsEmpty" || element.FilterOperator == "IsNotEmpty") return false;
        // if (element.FilterOperator != "Previous" && element.FilterOperator != "Next" && element.FilterOperator != "Current" && (!element.FieldValue || element.FieldValue == "")) return true;
        // if ((element.FilterOperator == "Previous" || element.FilterOperator == "Next") && (!element.FieldValue3 || element.FieldValue3 == "")) return true;
        // if (element.FilterOperator == "Between" && (!element.FieldValue2 || element.FieldValue2 == "" || element.FieldValue2 <= element.FieldValue)) return true;
        return false;
    }

    MapFilterToDashboardFilter(element: GlobalFilterItem, isCommon: boolean): any {
        return {
            // FieldId: element.DataSetFieldId,
            // DataSetId: element.DataSetId,
            // FieldName: isCommon ? element.CommonFilterField : element.EntityPM.FieldCode,
            // IsCommon: isCommon,
            // Operator: element.FilterOperator,
            DateGroupCode: element.DateGroupCode,
            FieldDataType: element.DataTypeCode,
            FieldValue: element.FieldValue,
            FieldValue2: element.FieldValue2,
            FieldValue3: element.FieldValue3,
        }
    }
}