import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { QueryFilterViewItem } from '../../../../Infrastructure/DataContracts/QueryFilterViewItem';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { AppTool } from '../../../../Infrastructure/Tools';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';

declare var window: any;

@Component({
    selector: 'QueryFilterTree',
    templateUrl: './QueryFilterTreeComponent.html',
    inputs: ['DataSource', 'IsRoot', 'ObjectTableName', 'ParentObjectTableName']
})

export class QueryFilterTreeComponent extends BaseComponent implements OnInit {
    public CurrentSession = SessionLocator.SelectedSession;
    public EntityResourceService: EntityResourceService = new EntityResourceService();
    AllObjectTables: ShortObjectTableDetails[] = [];
    ObjectTable: any;
    ParentObjectTable: any;

    constructor() {
        super();
    }

    ngOnInit() {
        this.LoadDefaultAdditionalFilters();
    }

    LoadDefaultAdditionalFilters() {
        if (this.MustAddEmptyFilter()) {
            this.AddEmptyFilter();
        }
        else if (this.IsRoot && AppTool.IsNullOrEmpty(this.ObjectTableName)) {
            this.AddEmptyFilter();
        }
        else if (this.IsRoot) {
            this.LoadLookUpTableResources();
        }
    }

    LoadLookUpTableResources() {
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Loading ...");
        this.LoadObjectTableEntityResources();
    }
    LoadObjectTableEntityResources() {
        if (this.ObjectTable && !this.ObjectTable.IsCustom) {
            this.EntityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((response: any) => {
                this.LoadParentObjectTableEntityResources();
            });
        }
        else {
            this.LoadParentObjectTableEntityResources();
        }
    }

    LoadParentObjectTableEntityResources() {
        let parentObjectTable = window.ObjectTables.filter(ob => ob.Name == this.ParentObjectTableName)[0];
        if (parentObjectTable && !parentObjectTable.IsCustom) {
            this.EntityResourceService.getEntityResourceByTableName(this.ParentObjectTableName).subscribe((response: any) => {
                this.LoadEntityResourcesFinished();
            });
        }
        else {
            this.LoadEntityResourcesFinished();
        }
    }

    LoadEntityResourcesFinished() {
        this.CurrentSession.StopBusyIndicator();
        let dataSourceItem = this.DataSource[0];
        if (dataSourceItem && dataSourceItem.length != 1) {
            this.DataSource = this.GetAllFilters(dataSourceItem);
        }
    }

    MustAddEmptyFilter() {

        if (!this.IsRoot) return false;
        if (!this.DataSource) return false;
        if (this.DataSource.length == 0) return true;
        if (this.DataSource.length == 1 && (this.DataSource[0].QueryFilterItems == null || this.DataSource[0].QueryFilterItems.length == 0)) return true;

        return false;
    }

    private GetAllFilters(oldValue) {
        let groupTreeFilter = new QueryFilterViewItem(null, this);
        groupTreeFilter.IsGroup = true;
        groupTreeFilter.setAndOrOperation(oldValue.FilterType);
        groupTreeFilter.IndexOrder = 1;
        let myFilter = this.RestoreFilters(oldValue, groupTreeFilter);
        let myFilterList = [];
        myFilterList.push(myFilter);
        return myFilterList;
    }

    RestoreFilters(BaseFilter: QueryFilterViewItem, MyFilter: QueryFilterViewItem) {
        BaseFilter.QueryFilterItems?.forEach((field) => {
            this.RestoreQueryFilterViewItem(field, MyFilter);
        });

        return MyFilter;
    }

    public dataSource: any;
    private RestoreQueryFilterViewItem(field: QueryFilterViewItem, MyFilter: QueryFilterViewItem) {
        if (field.QueryFilterItems.length == 0) {
            MyFilter.QueryFilterItems.push(new QueryFilterViewItem(field, this));
            return;
        }

        var DWObjectField = new QueryFilterViewItem(null, this);
        DWObjectField.IsGroup = true;
        DWObjectField.IndexOrder = MyFilter.QueryFilterItems.length;
        DWObjectField.setAndOrOperation(field.FilterType);
        this.RestoreFilters(field, DWObjectField);
        MyFilter.QueryFilterItems.push(DWObjectField);
    }

    public get DataSource() { return this.dataSource; }
    public set DataSource(newValue: any) {
        this.dataSource = newValue;
        if (this.IsRoot) {
            this.CurrentSession.SessionEvent.emit({ Name: "RefreshAdditionalFiltersData", Value: this.DataSource });
        }
    }

    public objectTableName: string;
    public get ObjectTableName() { return this.objectTableName; }
    public set ObjectTableName(newValue: string) {
        if (this.IsRoot && !AppTool.IsNullOrEmpty(newValue) && !AppTool.IsNullOrEmpty(this.objectTableName) && this.objectTableName != newValue) {
            this.DataSource = [];
        }
        if (newValue != this.objectTableName) {
            this.objectTableName = newValue;
            this.ObjectTable = window.ObjectTables.filter(ob => ob.Name == this.ObjectTableName)[0];
        }

        this.FillAllObjectTables();
    }

    public parentObjectTableName: string;
    public get ParentObjectTableName() { return this.parentObjectTableName; }
    public set ParentObjectTableName(newValue: string) {
        if (newValue != this.parentObjectTableName) {
            this.parentObjectTableName = newValue;
            this.ParentObjectTable = window.ObjectTables.filter(ob => ob.Name == this.ParentObjectTableName)[0];
        }

        this.FillAllObjectTables();
    }

    public isRoot: boolean;
    public get IsRoot() { return this.isRoot; }
    public set IsRoot(newValue: boolean) {
        if (newValue != this.isRoot) {
            this.isRoot = newValue;
        }
    }

    FillAllObjectTables() {
        this.AllObjectTables = [];
        this.PushParentObjectTable();
        this.PushObjectTable();
    }
    
    PushParentObjectTable() {
        if (!this.ParentObjectTable) return;
        this.AllObjectTables.push(new ShortObjectTableDetails(this.ParentObjectTableName, this.ParentObjectTable.FullNameTextCodeDefaultText));
    }

    PushObjectTable() {
        if (!this.ObjectTable) return;
        if (this.ParentObjectTableName == this.ObjectTableName) return;
        this.AllObjectTables.push(new ShortObjectTableDetails(this.ObjectTableName, this.ObjectTable.FullNameTextCodeDefaultText));
    }

    SelectedMainEntityName(item) {
        return this.AllObjectTables.filter(ob => ob.Code == item.MainEntityName)[0];
    }

    SelectedSecondaryEntityName(item) {
        return this.AllObjectTables.filter(ob => ob.Code == item.SecondaryEntityName)[0];
    }

    MainEntityChanged(item, selectedShortObjectTable) {
        item.MainEntityChanged(selectedShortObjectTable.Code);
    }

    SecondaryEntityChanged(item, selectedShortObjectTable) {
        item.SecondaryEntityChanged(selectedShortObjectTable.Code);
    }

    AddFilterToGroup(item: QueryFilterViewItem) {
        var newTreeFilter = new QueryFilterViewItem(null, item.MyParentClass);
        newTreeFilter.IndexOrder = this.DataSource.length;
        var tempData = item.QueryFilterItems;
        tempData.push(newTreeFilter);
        item.QueryFilterItems = tempData;
    }

    AddGroup(item: QueryFilterViewItem) {
        let newGroupTreeFilter = new QueryFilterViewItem(null, item.MyParentClass);
        newGroupTreeFilter.IsGroup = true;
        newGroupTreeFilter.IndexOrder = this.DataSource.length;
        var DWInnerObjectField = new QueryFilterViewItem(null, this.DataSource[0].MyParentClass);
        DWInnerObjectField.IndexOrder = newGroupTreeFilter.QueryFilterItems.length;
        newGroupTreeFilter.QueryFilterItems.push(DWInnerObjectField);
        var tempData = item.QueryFilterItems;
        tempData.push(newGroupTreeFilter);
        item.QueryFilterItems = tempData;
    }

    DeleteFilter(Item: QueryFilterViewItem, ListItems: QueryFilterViewItem[]) {
        ListItems.forEach((Myfilter) => {
            ListItems = this.DeleteSpecificFilter(Myfilter, Item, ListItems);
        });
        return ListItems;
    }

    private DeleteSpecificFilter(Myfilter: QueryFilterViewItem, Item: QueryFilterViewItem, ListItems: QueryFilterViewItem[]) {

        if (Myfilter.QueryFilterItems.length > 0) {
            ListItems = this.DeleteChildFilter(Myfilter, Item, ListItems);
        }
        else if (Myfilter == Item) {
            ListItems = ListItems.filter(a => a != Item);
        }
        return ListItems;
    }

    private DeleteChildFilter(Myfilter: QueryFilterViewItem, Item: QueryFilterViewItem, ListItems: QueryFilterViewItem[]) {
        Myfilter.QueryFilterItems = this.DeleteFilter(Item, Myfilter.QueryFilterItems);
        if (Myfilter.QueryFilterItems.length == 0) {
            ListItems = ListItems.filter(a => a != Myfilter);
        }
        return ListItems;
    }

    onDeleteFilterClick(item: QueryFilterViewItem) {
        item.MyParentClass.DataSource = this.DeleteFilter(item, item.MyParentClass.DataSource);
        this.DataSource = item.MyParentClass.DataSource;
    }

    AddEmptyFilter(item: QueryFilterViewItem = null) {
        this.DataSource = [];
        let emptyTreeFilter = new QueryFilterViewItem(null, item == null ? this : item.MyParentClass);
        let newTreeFilter = new QueryFilterViewItem(null, item == null ? this : item.MyParentClass);
        newTreeFilter.IsGroup = true;
        newTreeFilter.IndexOrder = this.DataSource.length;
        newTreeFilter.QueryFilterItems.push(emptyTreeFilter);
        this.DataSource.push(newTreeFilter);
    }
}

class ShortObjectTableDetails {
    constructor(code: string, name: string) {
        this.Code = code;
        this.Name = name;
    }
    Code: string;
    Name: string;
}
