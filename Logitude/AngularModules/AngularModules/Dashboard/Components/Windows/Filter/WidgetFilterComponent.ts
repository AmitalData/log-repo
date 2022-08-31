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
        if (this.IsRoot && this.DataSource && this.DataSource.QueryFilterItems && this.DataSource.QueryFilterItems.length != 0)
            this.DataSource = this.GetAllFilters(this.DataSource);
    }

    get FilterItems(): WidgetFilterItem[] {
        return this.DataSource.QueryFilterItems;
    }

    private GetAllFilters(oldValue: WidgetFilterItem) {
        let groupTreeFilter = new WidgetFilterItem();
        groupTreeFilter.IsGroup = true;
        groupTreeFilter.setAndOrOperation(oldValue.FilterType);

        let myFilter = this.RestoreFilters(oldValue, groupTreeFilter);        
        let parentItem = new WidgetFilterItem();

        parentItem.QueryFilterItems.push(myFilter);
        return parentItem;
    }

    RestoreFilters(oldFilter: WidgetFilterItem, newFilter: WidgetFilterItem) {
        oldFilter.QueryFilterItems?.forEach((oldField) => {
            this.BuildFilter(oldField, newFilter);
        });
        return newFilter;
    }

    private BuildFilter(oldField: WidgetFilterItem, myFilter: WidgetFilterItem) {
        if (oldField.QueryFilterItems.length == 0) {
            let groupTreeFilter = new WidgetFilterItem();
            myFilter.QueryFilterItems.push(groupTreeFilter);
            return;
        }
        var field = new WidgetFilterItem();
        field.IsGroup = true;
        field.setAndOrOperation(oldField.FilterType);
        this.RestoreFilters(oldField, field);
        myFilter.QueryFilterItems.push(field);
    }

    AddEmptyFilter() {
        var groupFilter = new WidgetFilterItem();
        groupFilter.IsGroup = true;
        groupFilter.QueryFilterItems.push(new WidgetFilterItem());
        this.DataSource.QueryFilterItems.push(groupFilter);
    }

    onDeleteFilterClick(filterItem: WidgetFilterItem) {
        this.DataSource.QueryFilterItems = this.FilterItems.filter(item => item !== filterItem);
    }

    AddFilterToGroup(item: WidgetFilterItem) {
        item.QueryFilterItems.push(new WidgetFilterItem());
    }

    AddGroup(item: WidgetFilterItem) {
        let newGroupTreeFilter = new WidgetFilterItem();
        newGroupTreeFilter.IsGroup = true;

        var newGroupField = new WidgetFilterItem();
        newGroupTreeFilter.QueryFilterItems.push(newGroupField);

        item.QueryFilterItems.push(newGroupTreeFilter);
    }

}