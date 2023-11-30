import { Component, OnInit, Input, Output, EventEmitter, AfterViewInit } from '@angular/core';
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
    @Output() ApplyFiltersCountChange = new EventEmitter<number>();

    public FilterItems: GlobalFilterItem[] = [];
    private DashboardGlobalPresetFilterListService: DashboardGlobalPresetFilterListService;
    public ShowFilters: boolean = true;
    private FiltersCount: number = 0;
    private FilterValueExistItems: string[] = [];
    private LastApplied: string[] = [];
    public FilterHasChanges: boolean = false;

    constructor() {
        this.DashboardGlobalPresetFilterListService = new DashboardGlobalPresetFilterListService();
    }

    ngOnInit() {
        this.GetPresetFitlers();
    }

    public get HasKpiChart(): boolean {
        return this.Dashboard?.Widgets?.find(x => x.TypeCode == "kpi") != null;
    }

    GetPresetFitlers() {
        this.DashboardGlobalPresetFilterListService.getAll().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse || myResponse.HasError) return;
            this.AddPresetFitlers(myResponse.Result);
        });
    }

    AddPresetFitlers(filters: DashboardGlobalPresetFilterList[]) {
        filters.forEach(presetFilter => {
            var filter = new GlobalFilterItem(this);
            filter.Id = presetFilter.Code + this.RandomString(5);
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
        this.FilterHasChanges = false;
        this.LastApplied = [];
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
        this.AddFilterItems(this.FilterItems, filterItems);
        this.ApplyFilters.emit(filterItems);
        this.FilterHasChanges = false;
        this.LastApplied = JSON.parse(JSON.stringify(this.FilterValueExistItems));
    }

    AddFilterItems(filters: GlobalFilterItem[], filterItems: any): any {
        if (!filters || filters.length == 0) return filterItems;
        filters.forEach(element => {
            this.SetPresetFilterOperator(element);
            if (this.FilterValueEmpty(element)) return;
            filterItems.push(Object.assign({}, element));
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
        if ((element.Operator == "Previous" || element.Operator == "Next" || element.Operator == "Current") && (!element.DateGroupCode || element.DateGroupCode == "")) return true;
        if (element.Operator == "Between" && (!element.FieldValue2 || element.FieldValue2 == "" || element.FieldValue2 <= element.FieldValue)) return true;
        return false;
    }

    ValueChanged(filterItem: GlobalFilterItem) {
        let valuIsEmpty = this.FilterValueEmpty(filterItem);
        var existItem = this.FilterValueExistItems.find(x => x == filterItem.Id);
        if (!valuIsEmpty) {
            if (!existItem) {
                this.FilterValueExistItems.push(filterItem.Id);
                this.FilterHasChanges = true;
            } else this.FilterHasChanges = true;
        }
        else {
            if (existItem) {
                const index = this.FilterValueExistItems.indexOf(filterItem.Id);
                if (index > -1) this.FilterValueExistItems.splice(index, 1);
                this.CheckHasChanges();
            }
        }

        this.FiltersCount = this.FilterValueExistItems.length;
        this.ApplyFiltersCountChange.emit(this.FiltersCount);
    }

    CheckHasChanges() {
        if (this.LastApplied.toString() != this.FilterValueExistItems.toString()) {
            this.FilterHasChanges = true;
        } else {
            this.FilterHasChanges = false;
        }
    }

    RandomString(length: number) {
        var result = '';
        var characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
        var charactersLength = characters.length;
        for (var i = 0; i < length; i++) {
            result += characters.charAt(Math.floor(Math.random() * charactersLength));
        }
        return result;
    }

}