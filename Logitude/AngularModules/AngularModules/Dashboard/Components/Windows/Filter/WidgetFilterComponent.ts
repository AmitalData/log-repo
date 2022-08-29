


import { Component, OnInit, Input } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { AppTool } from '../../../../Infrastructure/Tools';
import { WidgetFilterItem } from './WidgetFilterItem';


@Component({
    selector: 'WidgetFilter',
    templateUrl: './WidgetFilterComponent.html',
    inputs: ['DataSource', 'IsRoot', 'ObjectTableName', 'ParentObjectTableName']
})


export class WidgetFilterComponent extends BaseComponent implements OnInit {
    @Input() IsRoot: boolean;
    @Input() ObjectTableName: string;
    @Input() DataSource: any;


    ngOnInit(): void {


    }
    
    onDeleteFilterClick(item: WidgetFilterItem) {
        item.MyParentClass.DataSource = this.DeleteFilter(item, item.MyParentClass.DataSource);
        this.DataSource = item.MyParentClass.DataSource;
        if (item.MyParentClass.DataSource.length == 0 && item.MyParentClass.IsRoot) this.AddEmptyFilter(item);
    }

    AddEmptyFilter(item: WidgetFilterItem = null) {
        let emptyTreeFilter = new WidgetFilterItem(null, item == null ? this : item.MyParentClass);
        let newTreeFilter = new WidgetFilterItem(null, item == null ? this : item.MyParentClass);
        newTreeFilter.IsGroup = true;
        newTreeFilter.IndexOrder = this.DataSource.length;
        newTreeFilter.QueryFilterItems.push(emptyTreeFilter);
        this.DataSource.push(newTreeFilter);
    }
    
    DeleteFilter(Item: WidgetFilterItem, ListItems: WidgetFilterItem[]) {
        ListItems.forEach((Myfilter) => {
            ListItems = this.DeleteSpecificFilter(Myfilter, Item, ListItems);
        });
        return ListItems;
    }

    private DeleteSpecificFilter(Myfilter: WidgetFilterItem, Item: WidgetFilterItem, ListItems: WidgetFilterItem[]) {
        if (Myfilter.QueryFilterItems.length > 0) {
            Myfilter.QueryFilterItems = this.DeleteFilter(Item, Myfilter.QueryFilterItems);
            if (Myfilter.QueryFilterItems.length == 0) {
                ListItems = ListItems.filter(a => a != Myfilter);
            }
        }
        else if (Myfilter == Item) {
            ListItems = ListItems.filter(a => a != Item);
        }
        return ListItems;
    }

}