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
 
    }

    get FilterItems(): WidgetFilterItem[] {
        return this.DataSource.QueryFilterItems;
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