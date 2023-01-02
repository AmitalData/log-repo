import { Component, OnInit, Input } from '@angular/core';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { WidgetFilterItem } from './WidgetFilterItem';
import { MixPanelLocator } from 'Common/MixPanel/MixPanelLocator';


@Component({
    selector: 'WidgetFilter',
    templateUrl: './WidgetFilterComponent.html'
})
//QueryFilterViewItem
export class WidgetFilterComponent extends BaseComponent implements OnInit {
    @Input() public IsRoot: boolean;
    @Input() public DataSource: WidgetFilterItem;
    @Input() public EntityId: string;
    @Input() public DashboardId: string;


    public DateGroupCodes = ['Day', 'Week', 'Month', 'Quarter', 'Year'];
    public Quarters = ['Q1', 'Q2', 'Q3', 'Q4'];
    public Years: number[];

    ngOnInit() {
        this.Years = [];
        this.BuidYears();
    }

    private BuidYears() {
        for (let i = new Date().getFullYear() + 3; i > new Date().getFullYear(); i--) {
            this.Years.push(i);
        }
        for (let i = new Date().getFullYear(); i > new Date().getFullYear() - 30; i--) {
            this.Years.push(i);
        }
    }

    get FilterItems(): WidgetFilterItem[] {
        return this.DataSource?.QueryFilterItems;
    }

    AddEmptyFilter() {
        var groupFilter = new WidgetFilterItem(null, false, this.DashboardId);
        groupFilter.IsGroup = true;
        groupFilter.QueryFilterItems.push(new WidgetFilterItem(null, false, this.DashboardId));
        this.DataSource.QueryFilterItems.push(groupFilter);
        MixPanelLocator.PostDashboardAction({ ActionName: "Widget Add Empty Filter Click", DashboardId: this.DashboardId });
    }

    onDeleteFilterClick(filterItem: WidgetFilterItem) {
        this.DataSource.QueryFilterItems = this.FilterItems.filter(item => item !== filterItem);
        MixPanelLocator.PostDashboardAction({ ActionName: "Widget Delete Filter Click", DashboardId: this.DashboardId });
    }

    AddFilterToGroup(item: WidgetFilterItem) {
        item.QueryFilterItems.push(new WidgetFilterItem(null, false, this.DashboardId));
        MixPanelLocator.PostDashboardAction({ ActionName: "Widget Add Filter Click", DashboardId: this.DashboardId });
    }

    AddGroup(item: WidgetFilterItem) {
        let newGroupTreeFilter = new WidgetFilterItem(null, false, this.DashboardId);
        newGroupTreeFilter.IsGroup = true;

        var newGroupField = new WidgetFilterItem(null, false, this.DashboardId);
        newGroupTreeFilter.QueryFilterItems.push(newGroupField);

        item.QueryFilterItems.push(newGroupTreeFilter);
        MixPanelLocator.PostDashboardAction({ ActionName: "Widget Group Filter Click", DashboardId: this.DashboardId });
    }

}