import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { QueryFilterViewItem } from '../../../../Infrastructure/DataContracts/QueryFilterViewItem';


@Component({
    selector: 'QueryFilterTree',
    templateUrl: './QueryFilterTreeComponent.html',
    inputs: ['DataSource', 'IsRoot', 'ObjectTableName', 'ParentObjectTableName']
})

export class QueryFilterTreeComponent extends BaseComponent implements OnInit {
    DataSource: any;
    IsRoot: boolean;
    AllObjectTables: string[] = [];

    constructor() {
        super();
    }

    ngOnInit() {
        this.LoadDefaultAdditionalFilters();
    }

    LoadDefaultAdditionalFilters() {       
        if (this.IsRoot && this.DataSource && this.DataSource.length == 0) {
            this.AddEmptyFilter();
        }
        else if (this.IsRoot) {
            this.SetAllFilters();
        }
    }

    private SetAllFilters() {
        let groupTreeFilter = new QueryFilterViewItem(null, this);
        groupTreeFilter.IsGroup = true;
        groupTreeFilter.setAndOrOperation(this.DataSource.FilterType);
        groupTreeFilter.IndexOrder = this.DataSource.length;
        let MyFilter = this.RestoreFilters(this.DataSource, groupTreeFilter);
        let temp = [];
        temp.push(MyFilter);
        this.DataSource = temp;
    }

    RestoreFilters(BaseFilter: QueryFilterViewItem, MyFilter: QueryFilterViewItem) {
        BaseFilter.AdditionalFilters?.forEach((field) => {
            if (field.AdditionalFilters.length == 0) {
                let groupTreeFilter = new QueryFilterViewItem(field, this);
                MyFilter.AdditionalFilters.push(groupTreeFilter);
            }
            else {
                var DWObjectField = new QueryFilterViewItem(null, this);
                DWObjectField.IsGroup = true;
                DWObjectField.IndexOrder = MyFilter.AdditionalFilters.length;
                DWObjectField.setAndOrOperation(field.AndOr);
                this.RestoreFilters(field, DWObjectField);
                MyFilter.AdditionalFilters.push(DWObjectField);
            }
        });

        return MyFilter;
    }

    public objectTableName: string;
    public get ObjectTableName() { return this.objectTableName; }
    public set ObjectTableName(newValue: string) {
        if (newValue != this.objectTableName) {
            this.objectTableName = newValue;
        }

        this.FillAllObjectTables();
    }

    public parentObjectTableName: string;
    public get ParentObjectTableName() { return this.parentObjectTableName; }
    public set ParentObjectTableName(newValue: string) {
        if (newValue != this.parentObjectTableName) {
            this.parentObjectTableName = newValue;
        }

        this.FillAllObjectTables();
    }

    FillAllObjectTables() {
        this.AllObjectTables = [];
        this.AllObjectTables.push(this.ParentObjectTableName);
        this.AllObjectTables.push(this.ObjectTableName);
    }

    AddFilterToGroup(item: QueryFilterViewItem) {
        var newTreeFilter = new QueryFilterViewItem(null, item.MyParentClass);
        newTreeFilter.IndexOrder = this.DataSource.length;
        var tempData = item.AdditionalFilters;
        tempData.push(newTreeFilter);
        item.AdditionalFilters = tempData;
    }

    AddGroup(item: QueryFilterViewItem) {
        let newGroupTreeFilter = new QueryFilterViewItem(null, item.MyParentClass);
        newGroupTreeFilter.IsGroup = true;
        newGroupTreeFilter.IndexOrder = this.DataSource.length;
        var DWInnerObjectField = new QueryFilterViewItem(null, this.DataSource[0].MyParentClass);
        DWInnerObjectField.IndexOrder = newGroupTreeFilter.AdditionalFilters.length;
        newGroupTreeFilter.AdditionalFilters.push(DWInnerObjectField);
        var tempData = item.AdditionalFilters;
        tempData.push(newGroupTreeFilter);
        item.AdditionalFilters = tempData;
    }

    DeleteFilter(Item: QueryFilterViewItem, ListItems: QueryFilterViewItem[]) {
        ListItems.forEach((Myfilter) => {
            ListItems = this.DeleteSpecificFilter(Myfilter, Item, ListItems);
        });
        return ListItems;
    }

    private DeleteSpecificFilter(Myfilter: QueryFilterViewItem, Item: QueryFilterViewItem, ListItems: QueryFilterViewItem[]) {
        if (Myfilter.AdditionalFilters.length > 0) {
            Myfilter.AdditionalFilters = this.DeleteFilter(Item, Myfilter.AdditionalFilters);
            if (Myfilter.AdditionalFilters.length == 0) {
                ListItems = ListItems.filter(a => a != Myfilter);
            }
        }
        else if (Myfilter == Item) {
            ListItems = ListItems.filter(a => a != Item);
        }
        return ListItems;
    }

    onDeleteFilterClick(item: QueryFilterViewItem) {
        item.MyParentClass.DataSource = this.DeleteFilter(item, item.MyParentClass.DataSource);
        this.DataSource = item.MyParentClass.DataSource;
        if (item.MyParentClass.DataSource.length == 0) this.AddEmptyFilter();
    }

    AddEmptyFilter() {
        let emptyTreeFilter = new QueryFilterViewItem(null, this);
        let newTreeFilter = new QueryFilterViewItem(null, this);
        newTreeFilter.IsGroup = true;
        newTreeFilter.IndexOrder = this.DataSource.length;
        newTreeFilter.AdditionalFilters.push(emptyTreeFilter);
        this.DataSource.push(newTreeFilter);
    }
}
