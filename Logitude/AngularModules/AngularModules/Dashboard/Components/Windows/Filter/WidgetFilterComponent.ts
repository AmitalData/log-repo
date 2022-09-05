import { Component, OnInit, Input } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { AppTool } from '../../../../Infrastructure/Tools';
import { WidgetFilterItem } from './WidgetFilterItem';


@Component({
    selector: 'WidgetFilter',
    templateUrl: './WidgetFilterComponent.html'
})
//QueryFilterViewItem
export class WidgetFilterComponent extends BaseComponent implements OnInit {
    @Input() public IsRoot: boolean;
    @Input() public DataSource: WidgetFilterItem;
    @Input() public EntityId: string;

    ngOnInit() {
        this.LoadDefaultAdditionalFilters();
    }

    LoadDefaultAdditionalFilters() {
        // if (this.IsRoot) this.DataSource = this.GetAllFilters(this.DataSource[0]);
    }

    get FilterItems(): WidgetFilterItem[] {
        return this.DataSource.QueryFilterItems;
    }

    private GetAllFilters(oldValue: WidgetFilterItem) {
        let groupTreeFilter = new WidgetFilterItem();
        groupTreeFilter.IsGroup = true;
        groupTreeFilter.setAndOrOperation(oldValue.FilterType);
        groupTreeFilter.IndexOrder = 1;
        let myFilter = this.RestoreFilters(oldValue, groupTreeFilter);
        let myFilterList = [];
        myFilterList.push(myFilter);
        return myFilterList;
    }

    RestoreFilters(baseFilter: WidgetFilterItem, myFilter: WidgetFilterItem) {
        baseFilter.QueryFilterItems?.forEach((field) => {
            this.BuildFilter(field, myFilter);
        });
        return myFilter;
    }

    private BuildFilter(field: WidgetFilterItem, myFilter: WidgetFilterItem) {
        if (field.QueryFilterItems.length == 0) {
            let groupTreeFilter = new WidgetFilterItem();
            myFilter.QueryFilterItems.push(groupTreeFilter);
            return;
        }
        var DWObjectField = new WidgetFilterItem();
        DWObjectField.IsGroup = true;
        DWObjectField.IndexOrder = myFilter.QueryFilterItems.length;
        DWObjectField.setAndOrOperation(field.FilterType);
        this.RestoreFilters(field, DWObjectField);
        myFilter.QueryFilterItems.push(DWObjectField);
    }

    AddEmptyFilter() {
        var groupFilter = new WidgetFilterItem();
        groupFilter.IsGroup = true;
        groupFilter.IndexOrder = this.FilterItems.length;
        groupFilter.QueryFilterItems.push(new WidgetFilterItem());
        this.DataSource.QueryFilterItems.push(groupFilter);
    }

    onDeleteFilterClick(filterItem: WidgetFilterItem) {
        this.DataSource.QueryFilterItems = this.FilterItems.filter(item => item !== filterItem);
    }

    AddFilterToGroup(item: WidgetFilterItem) {
        var newTreeFilter = new WidgetFilterItem();
        newTreeFilter.IndexOrder = this.FilterItems.length;
        item.QueryFilterItems.push(newTreeFilter);
    }

    AddGroup(item: WidgetFilterItem) {
        let newGroupTreeFilter = new WidgetFilterItem();
        newGroupTreeFilter.IsGroup = true;
        newGroupTreeFilter.IndexOrder = this.FilterItems.length;

        var newGroupField = new WidgetFilterItem();
        newGroupField.IndexOrder = newGroupTreeFilter.QueryFilterItems.length;
        newGroupTreeFilter.QueryFilterItems.push(newGroupField);

        item.QueryFilterItems.push(newGroupTreeFilter);
    }

}