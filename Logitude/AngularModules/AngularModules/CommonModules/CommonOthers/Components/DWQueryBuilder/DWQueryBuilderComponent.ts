declare var window: any;

import { Component, ViewContainerRef, OnInit, AfterViewInit, ViewChildren, QueryList, Output, EventEmitter, ChangeDetectorRef } from '@angular/core';
import { DWObjectFieldPM } from '../../../../Infrastructure/EntityPMs/DWObjectFieldPM';
import { DWObjectTablePM } from '../../../../Infrastructure/EntityPMs/DWObjectTablePM';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { DWObjectTablePMService } from '../../../../Infrastructure/Services/StandardPMs/DWObjectTablePMService';
import { DWObjectFieldExtendedPMService } from '../../../../Infrastructure/Services/ExtendedPMs/DWObjectFieldExtendedPMService';
import { DWQueryBuilderService } from '../../../../Infrastructure/Services/ExtendedPMs/DWQueryBuilderService';
import { AppTool, DateTool } from '../../../../Infrastructure/Tools';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { GroupByPipe } from '../../../../Infrastructure/Pipes/GroupByPipe';
import { DWQueryData } from '../../../../Common/DataContracts/DWQueryData';
import { DWSubQueryPMService } from '../../../../Infrastructure/Services/StandardPMs/DWSubQueryPMService';
import { DWSubQueryPM } from '../../../../Infrastructure/EntityPMs/DWSubQueryPM';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { DWObjectTableListService } from '../../../../Infrastructure/Services/StandardLists/DWObjectTableListService';


@Component({
    moduleId: module.id,
    selector: 'DWQueryBuilder',
    templateUrl: './DWQueryBuilderComponent.html',
})

export class DWQueryBuilderComponent extends BaseComponent {
    RTL: boolean = false;
    public BooleanValues = ["True", "False", "No Filter"];
    public AndOrOps = ["And", "Or"];
    public _DWObjectTablePMService: DWObjectTablePMService;
    public _DWObjectFieldPMService: DWObjectFieldExtendedPMService;
    public _DWQueryBuilderService: DWQueryBuilderService;
    public _DWSubQueryPMService: DWSubQueryPMService;
    public _DWObjectTableListService: DWObjectTableListService;
    @Output() SelectedFiltersDataSourceChanged = new EventEmitter();
    @Output() onSelectedDataLoadedEvent = new EventEmitter();
    @Output() onUnSelectedDataLoadedEvent = new EventEmitter();
    @Output() onDataSourceChangedEvent = new EventEmitter();
    @Output() onUnselectedDataSourceChangedEvent = new EventEmitter();
    DataSource: any[];
    AllFieldsDataSource: DWObjectFieldsDetails[];
    AllGroupsDataSource: DWFieldsGroup[];
    SelectedFieldsDataSource: DWObjectFieldsDetails[] = [];
    SelectedFiltersDataSource: DWObjectFieldsDetails[] = [];
    AllFieldsWithChildrenDataSource: DWObjectFieldsDetails[];
    public ObsList: any[] = [];
    public ObsListAll: any[] = [];
    DataContext: any = this;
    public AllFieldsObsList: any[] = [];
    /////////////////////////////////////////////////////////
    QueryId: string;
    AllTables: any[] = [];
    isNewQueryMode: boolean;
    CurrentObjectTable: string;
    IsEnabled: boolean;
    queryColumnsList: any[];
    unselectedObjectFields: any[];
    staticColumnsList: any[];
    addedQueryColumnList: any[];
    removedQueryColumnList: any[];
    unSelectedList: any[];
    OrderedQueryColumnsList: any[];
    unselected: any[];
    Fixedunselected: any[];
    ObjectTable: any;
    SearchFieldsId: string;
    HasChanges: boolean = false;
    needsRebuildList: boolean = false;
    constructor(private CD: ChangeDetectorRef) {
        super();
        this._DWObjectTablePMService = new DWObjectTablePMService();
        this._DWObjectFieldPMService = new DWObjectFieldExtendedPMService();
        this._DWQueryBuilderService = new DWQueryBuilderService();
        this._DWSubQueryPMService = new DWSubQueryPMService();
        this._DWObjectTableListService = new DWObjectTableListService();

        this._DWObjectTableListService.getAll().subscribe(myResult => {
            this.AllTables = myResult.Result;
            this._DWObjectTablePMService.get("Fact_Shipments").subscribe(myResult => {
                if (!myResult.HasError) {
                    this._DWObjectFieldPMService.GetDWObjectFieldsByDWTableIdGroupedByCategory(myResult.Result.Code).subscribe(Result => {//getDWObjectFieldsByDWTableId
                        if (!Result.HasError) {
                            var MyGroups = [];
                            var MyAllGroups = [];
                            Result.Result.forEach((Group) => {
                                if (Group.FieldsList.filter(a => a.DisplayInQueryBuilder == true).length > 0) {
                                    var view = new DWFieldsGroup(Group.Key, Group.FieldsList);
                                    if (MyGroups.length == 0) {
                                        view.IsDetailesOpened = true;
                                        view.DetailsIcon = "./Images/CellIcons/Arrowup.png";
                                    }
                                    else {
                                        view.IsDetailesOpened = false;
                                        view.DetailsIcon = "./Images/CellIcons/Arrowdown.png";
                                    }
                                    var MyInnerList = [];
                                    view.FieldsList.forEach((field) => {
                                        if (field.DisplayInQueryBuilder == true) {
                                            var MyItem = new DWObjectFieldsDetails(field, this);
                                            MyItem.ParentDataTypeCode = field.DataTypeCode;
                                            MyItem.Category1 = field.Category1;
                                            MyItem.Category2 = field.Category2;
                                            MyInnerList.push(MyItem);
                                            this.ObsList.push(MyItem);
                                            this.ObsListAll.push(MyItem);
                                        }
                                    });

                                    var view1 = new DWFieldsGroup(Group.Key, Group.FieldsList);
                                    if (MyAllGroups.length == 0) {
                                        view1.IsDetailesOpened = true;
                                        view1.DetailsIcon = "./Images/CellIcons/Arrowup.png";
                                    }
                                    else {
                                        view1.IsDetailesOpened = false;
                                        view1.DetailsIcon = "./Images/CellIcons/Arrowdown.png";
                                    }
                                    var MyInnerList1 = [];
                                    view1.FieldsList.forEach((field) => {
                                        if (field.DisplayInQueryBuilder == true) {
                                            var MyItem = new DWObjectFieldsDetails(field, this);
                                            MyItem.ParentDataTypeCode = field.DataTypeCode;
                                            MyItem.Category1 = field.Category1;
                                            MyItem.Category2 = field.Category2;
                                            MyInnerList1.push(MyItem);
                                            this.ObsList.push(MyItem);
                                            this.ObsListAll.push(MyItem);
                                        }
                                    });

                                    view.FieldsList = MyInnerList;
                                    view1.FieldsList = MyInnerList1;
                                    MyGroups.push(view);
                                    MyAllGroups.push(view1);
                                }
                            });
                            //Result.Result.forEach((field) => {
                            //    if (field.DisplayInQueryBuilder == true) {
                            //        var view = new DWObjectFieldsDetails(field, this);
                            //        view.ParentDataTypeCode = field.DataTypeCode;
                            //        view.Category1 = field.Category1;
                            //        view.Category2 = field.Category2;
                            //        this.ObsList.push(view);
                            //        this.ObsListAll.push(view);
                            //    }
                            //});
                            this.DataSource = MyGroups;//this.ObsList;
                            this.AllGroupsDataSource = MyAllGroups;
                            this.AllFieldsDataSource = this.ObsList;
                        }

                    });
                    this._DWObjectFieldPMService.getDWObjectFieldsWithChildrenByDWTableId(myResult.Result.Code).subscribe(Result => {
                        if (!Result.HasError) {
                            Result.Result.forEach((field) => {
                                if (field.DisplayInQueryBuilder == true || field.IsPrimaryKey == true) {
                                    var view = new DWObjectFieldsDetails(field, this);
                                    view.ParentDataTypeCode = field.DataTypeCode;
                                    this.AllFieldsObsList.push(field);
                                    this.ObsList.push(view);
                                    this.ObsListAll.push(view);
                                }
                            });
                            //this.DataSource = this.ObsList;
                            this.AllFieldsWithChildrenDataSource = this.ObsList;
                        }

                    });
                }
            });
        });


    }
    SetWindowArgs(args: any) {
        //this.ID = args.queryId;
        //this.isNewQueryMode = args.isNewQueryMode;
        //this.CurrentObjectTable = args.currentObjectTable;
        //this.IsEnabled = false;
        //this.ObjectTable = window.ObjectTables.filter(d => d.Name == args.currentObjectTable)[0];
        //this.Run();
    }

    ClearPlaceHolder() {
        var temp = document.getElementById(this.SearchFieldsId) as HTMLInputElement;
        temp.placeholder = "";
        temp.style.background = "rgba(0, 0, 0, 0)";
    }

    FillPlaceHolder() {
        //var temp = document.getElementById(this.SearchFieldsId) as HTMLInputElement;
        //temp.placeholder = TextCodeTranslator.Translate("General.O.Search");
        //temp.style.background = "url(Images/Search.png) no-repeat scroll";
        //temp.style.backgroundPosition = "right center";
        //temp.style.paddingRight = "30px";
    }

    Run() {

        //this.HasChanges = false;
        //var copy = false;

        //var currentQuery = window.Queries.filter(d => d.Id == this.QueryId)[0];
        //this.addedQueryColumnList = [];
        //this.removedQueryColumnList = [];
        ////queriesByUser = TenantContext.Current.Queries.Where(d => d.UserId == TenantContext.Current.LoggedContactId).ToList();
        //this._http.get(ServiceHelper.GetLogitudeURL() + "api/ngMetaData?tenant=" + SessionInfo.LoggedUserTenant + "&queryid=" + this.QueryId + "&objecttableid=" + this.ObjectTable.Id + "&userid=" + SessionInfo.LoggedUserId)
        //    .subscribe((response) => {
        //        this.queryColumnsList = response.json();
        //        // this.queryColumnsList = TenantContext.Current.GeneralContext.QueryColumnPMs.Where(d => d.QueryId == QueryId && ((d.UserId == TenantContext.Current.LoggedContactId && d.Tenant == TenantContext.Current.Id)) && d.DisplayInList).OrderBy(d => d.IndexOrder).ToList();
        //        this.queryColumnsList = this.queryColumnsList.sort((a, b) => { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1 });

        //        if (this.queryColumnsList.length == 0) // Copy query columns to my tenant
        //        {
        //            var zeroColumnsList = [];
        //            this._http.get(ServiceHelper.GetLogitudeURL() + "api/ngMetaData?tenant=0&queryid=" + this.QueryId + "&objecttableid=" + this.ObjectTable.Id + "&userid=null")
        //                .subscribe((response) => {
        //                    zeroColumnsList = response.json();
        //                    zeroColumnsList = zeroColumnsList.sort((a, b) => { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1 });
        //                    zeroColumnsList.forEach((querycolumn, key) => {
        //                        var newcolumn = new QueryColumnPM();

        //                            newcolumn.Tenant = SessionInfo.LoggedUserTenant,
        //                            newcolumn.UserId = SessionInfo.LoggedUserId,
        //                            newcolumn.DisplayInList = querycolumn.DisplayInList,
        //                            newcolumn.ObjectFieldName = querycolumn.ObjectFieldName,
        //                            newcolumn.ColumnWidth = querycolumn.ColumnWidth,
        //                            newcolumn.ConverterName = querycolumn.ConverterName,
        //                            newcolumn.DataTemplateName = querycolumn.DataTemplateName,
        //                            newcolumn.ColumnHeaderTemplateName = querycolumn.ColumnHeaderTemplateName,
        //                            newcolumn.IndexOrder = querycolumn.IndexOrder,
        //                            newcolumn.ObjectFieldDataTypeCode = querycolumn.ObjectFieldDataTypeCode,
        //                            newcolumn.ObjectFieldFieldLableTextCodeDefaultText = querycolumn.ObjectFieldFieldLableTextCodeDefaultText,
        //                            newcolumn.ObjectFieldId = querycolumn.ObjectFieldId,
        //                            newcolumn.ObjectFieldListLabelTextCodeCode = querycolumn.ObjectFieldListLabelTextCodeCode,
        //                            newcolumn.QueryCode = querycolumn.QueryCode,
        //                            newcolumn.QueryId = querycolumn.QueryId,
        //                            newcolumn.QueryObjectTableName = querycolumn.QueryObjectTableName,
        //                            newcolumn.ObjectFieldFieldLableTextCodeCode = querycolumn.ObjectFieldFieldLableTextCodeCode,
        //                            // TenantContext.Current.GeneralContext.QueryColumnPMs.Add(newcolumn);
        //                            this.queryColumnsList.push(newcolumn);
        //                            this.addedQueryColumnList.push(newcolumn);
        //                        //copy = true;

        //                    });

        //                });

        //        }


        //        this.staticColumnsList = this.queryColumnsList.filter(q => q.QueryId == this.QueryId && ((q.UserId == SessionInfo.LoggedUserId && q.Tenant == SessionInfo.LoggedUserTenant))).sort((a, b) => { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1 });

        //        var listColumns = this.queryColumnsList.filter(q => q.QueryId == this.QueryId && ((q.UserId == SessionInfo.LoggedUserId && q.Tenant == SessionInfo.LoggedUserTenant))).sort((a, b) => { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1 });


        //        this.unselectedObjectFields = window.ObjectFields.filter(a => a.ObjectTableName == this.CurrentObjectTable).filter(d => d.DisplayInList == true && (d.Tenant == SessionInfo.LoggedUserTenant || d.Tenant == 0) && ((d.ValidForQuerySection1 == currentQuery.QuerySection || d.ValidForQuerySection2 == currentQuery.QuerySection) || d.IsCustom == true));


        //        this.unselected = [];
        //        this.unselectedObjectFields.forEach((field, key) => {
        //            var xx = this.queryColumnsList.filter(q => q.QueryId == this.QueryId && q.ObjectFieldId == field.Id && field.FieldName != "TimeFrameFilter");
        //            var yy = this.unselected.filter(q => q.Id == field.Id);

        //            if (xx.length == 0 && yy.length == 0) {
        //                this.unselected.push(field);
        //            }
        //        });


        //        //this.UnSelectedQueryColumnsList.ItemsSource = unselected.OrderBy(c => c.FieldName);
        //        this.OrderedQueryColumnsList = [];
        //        this.unSelectedList = this.unselected.sort((a, b) => { return (a.FieldName.toLowerCase() === b.FieldName.toLowerCase()) ? 0 : (a.FieldName.toLowerCase() < b.FieldName.toLowerCase()) ? -1 : 1 });
        //        this.queryColumnsList.forEach((qc, key) => {
        //            var CurColumn = this.OrderedQueryColumnsList.filter(a => a.ObjectFieldName == qc.ObjectFieldName);
        //            if (CurColumn == null || CurColumn.length == 0) {
        //                this.OrderedQueryColumnsList.push(new QueryColumnDetails(qc));
        //            }
        //        });

        //        this.OrderedQueryColumnsList = this.OrderedQueryColumnsList.sort((a, b) => { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1 });
        //        this.CD.detectChanges();
        //        //SelectedQueryColumnsList.ItemsSource = OrderedQueryColumnsList;
        //        this.Fixedunselected = this.unSelectedList;

        //        this.IsEnabled = true;
        //        this.onUnSelectedDataLoadedEvent.emit(this.SelectedItem);
        //        this.onSelectedDataLoadedEvent.emit(this.FieldSelectedItem);
        //    });
    }

    SetSelectedItem(item) {
        this.SelectedItem = item;
        this.FieldSelectedItem = null;
        this.FilterSelectedItem = null;
        this.IsbtnAddEnabled = true;
        this.IsbtnRemoveEnabled = false;
        this.IsbtnAddFilterEnabled = true;
        this.IsbtnRemoveFilterEnabled = false;
        this.IsbtnUpEnabled = false;
        this.IsbtnDownEnabled = false;
    }

    SetFieldSelectedItem(item) {
        this.FieldSelectedItem = item;
        this.SelectedItem = null;
        this.FilterSelectedItem = null;
        this.IsbtnAddEnabled = false;
        this.IsbtnRemoveEnabled = true;
        this.IsbtnUpEnabled = true;
        this.IsbtnDownEnabled = true;
        this.IsbtnAddFilterEnabled = false;
        this.IsbtnRemoveFilterEnabled = false;
    }

    SetFilterSelectedItem(item) {
        this.FilterSelectedItem = item;
        this.SelectedItem = null;
        this.FieldSelectedItem = null;
        this.IsbtnAddFilterEnabled = false;
        this.IsbtnRemoveFilterEnabled = true;
        this.IsbtnAddEnabled = false;
        this.IsbtnRemoveEnabled = false;
    }

    private notes: string;
    public get Notes() { return this.notes; }
    public set Notes(newValue: string) {
        this.notes = newValue;
    }

    private searchText: string;
    public get SearchText() { return this.searchText; }
    public set SearchText(newValue: string) {
        this.searchText = newValue;
        if (newValue != null && newValue != "") {
            //var myAll = this.AllGroupsDataSource;
            this.AllGroupsDataSource.forEach((Group) => {
                var temp = Group.FieldsList.filter(a => a.Name.toLowerCase().indexOf(newValue.toLowerCase()) > -1);
                this.DataSource.filter(a => a.Key == Group.Key)[0].FieldsList = temp;
               
                if (temp.length == 0) {
                    this.DataSource.filter(a => a.Key == Group.Key)[0].IsDetailesOpened = false;
                    this.DataSource.filter(a => a.Key == Group.Key)[0].DetailsIcon = "./Images/CellIcons/Arrowdown.png";
                }
                else {
                    this.DataSource.filter(a => a.Key == Group.Key)[0].IsDetailesOpened = true;
                    this.DataSource.filter(a => a.Key == Group.Key)[0].DetailsIcon = "./Images/CellIcons/Arrowup.png";
                }

            });
            //this.DataSource = this.AllFieldsDataSource.filter(a => a.Name.toLowerCase().indexOf(newValue.toLowerCase()) > -1);
        }
        else {
            //this.DataSource = this.AllFieldsDataSource;
            var index = 0;
            this.AllGroupsDataSource.forEach((Group) => {
                if (index == 0) {
                    this.DataSource.filter(a => a.Key == Group.Key)[0].IsDetailesOpened = true;
                    this.DataSource.filter(a => a.Key == Group.Key)[0].DetailsIcon = "./Images/CellIcons/Arrowup.png";
                }
                else {
                    this.DataSource.filter(a => a.Key == Group.Key)[0].IsDetailesOpened = false;
                    this.DataSource.filter(a => a.Key == Group.Key)[0].DetailsIcon = "./Images/CellIcons/Arrowdown.png";
                }
                this.DataSource.filter(a => a.Key == Group.Key)[0].FieldsList = Group.FieldsList;
                index++;
            });
        }
        //this.AllGroupsDataSource = myAll;
        //this.onUnselectedDataSourceChangedEvent.emit(this.unSelectedList);
        //this.onUnSelectedDataLoadedEvent.emit(this.SelectedItem);

    }

    private selectedItem: DWObjectFieldsDetails;
    public get SelectedItem() { return this.selectedItem; }
    public set SelectedItem(newValue: DWObjectFieldsDetails) {
        this.selectedItem = newValue;
    }

    private fieldSelectedItem: DWObjectFieldsDetails;
    public get FieldSelectedItem() { return this.fieldSelectedItem; }
    public set FieldSelectedItem(newValue: DWObjectFieldsDetails) {
        this.fieldSelectedItem = newValue;
    }

    private filterSelectedItem: DWObjectFieldsDetails;
    public get FilterSelectedItem() { return this.filterSelectedItem; }
    public set FilterSelectedItem(newValue: DWObjectFieldsDetails) {
        this.filterSelectedItem = newValue;
    }

    private isbtnAddEnabled: boolean = false;
    public get IsbtnAddEnabled() { return this.isbtnAddEnabled; }
    public set IsbtnAddEnabled(newValue: boolean) {
        this.isbtnAddEnabled = newValue;
    }

    private isbtnRemoveEnabled: boolean = false;
    public get IsbtnRemoveEnabled() { return this.isbtnRemoveEnabled; }
    public set IsbtnRemoveEnabled(newValue: boolean) {
        this.isbtnRemoveEnabled = newValue;
    }

    private isbtnAddFilterEnabled: boolean = false;
    public get IsbtnAddFilterEnabled() { return this.isbtnAddFilterEnabled; }
    public set IsbtnAddFilterEnabled(newValue: boolean) {
        this.isbtnAddFilterEnabled = newValue;
    }

    private isbtnRemoveFilterEnabled: boolean = false;
    public get IsbtnRemoveFilterEnabled() { return this.isbtnRemoveFilterEnabled; }
    public set IsbtnRemoveFilterEnabled(newValue: boolean) {
        this.isbtnRemoveFilterEnabled = newValue;
    }

    private isbtnUpEnabled: boolean = true;
    public get IsbtnUpEnabled() { return this.isbtnUpEnabled; }
    public set IsbtnUpEnabled(newValue: boolean) {
        this.isbtnUpEnabled = newValue;
    }

    private isbtnDownEnabled: boolean = true;
    public get IsbtnDownEnabled() { return this.isbtnDownEnabled; }
    public set IsbtnDownEnabled(newValue: boolean) {
        this.isbtnDownEnabled = newValue;
    }


    ReorderColumnsList() {
        var queryColumnList = this.SelectedFieldsDataSource.sort((a, b) => { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1 });

        var i = 0;
        for (; i < queryColumnList.length; i++) {
            queryColumnList[i].IndexOrder = i;
        }
    }
    btnUp_Click() {

        var item = this.FieldSelectedItem;
        if (item != null) {
            //this.HasChanges = true;
            var i = this.SelectedFieldsDataSource.indexOf(item);

            this.ReorderColumnsList();
            var upColumn = this.SelectedFieldsDataSource.filter(d => d.DisplayName == item.DisplayName)[0];

            if (i > 0) {
                this.SelectedFieldsDataSource = this.SelectedFieldsDataSource.filter(d => d.DisplayName != upColumn.DisplayName);
                this.SelectedFieldsDataSource.filter(o => o.IndexOrder == i - 1)[0].IndexOrder = i;
                upColumn.IndexOrder = i - 1;
                this.SelectedFieldsDataSource.splice(i - 1, 0, upColumn);
                //this.onDataSourceChangedEvent.emit(this.SelectedFieldsDataSource);
                //this.ReorderColumnsList();
                //this.onSelectedDataLoadedEvent.emit(this.SelectedItem);
            }
            this.SelectedFieldsDataSource = this.ResetIndexes(this.SelectedFieldsDataSource);
        }

    }

    btnDown_Click() {
        var item = this.FieldSelectedItem;
        if (item != null) {
            //this.HasChanges = true;

            var i = this.SelectedFieldsDataSource.indexOf(item);

            this.ReorderColumnsList();

            var downColumn = this.SelectedFieldsDataSource.filter(d => d.DisplayName == item.DisplayName)[0];

            if (i < this.SelectedFieldsDataSource.length - 1) {
                this.SelectedFieldsDataSource = this.SelectedFieldsDataSource.filter(d => d.DisplayName != item.DisplayName);

                this.SelectedFieldsDataSource.filter(o => o.IndexOrder == i + 1)[0].IndexOrder = i;
                downColumn.IndexOrder = i + 1;
                this.SelectedFieldsDataSource.splice(i + 1, 0, downColumn);

            }
            this.SelectedFieldsDataSource = this.ResetIndexes(this.SelectedFieldsDataSource);
        }

    }

    ResetIndexes(TempArray: any[]) {
        var index = 0;
        TempArray.forEach((field) => {
            field.IndexOrder = index;
            index++;
        });
        return TempArray;
    }

    btnAdd_Click(item) {
        //var view = new DWObjectFieldsDetails(item.BaseDWObjectField, this);
        //if (view.DWObjectTableCode.indexOf("DIM_") != -1) {
        //    view.ParentDataTypeCode = "LookUp";
        //    view.ParentDimTabelName = item.BaseDWObjectField.DWObjectTableCode;
        //}
        //else {
        //    view.ParentDataTypeCode = item.BaseDWObjectField.DataTypeCode; 
        //}
        this.SelectedItem = item;
        if (this.SelectedItem && this.SelectedFieldsDataSource.indexOf(this.SelectedItem) == -1) {//&& this.SelectedItem.DataTypeCode != "LookUp" && this.SelectedItem.DataTypeCode != "Dimension"
            //var index = this.DataSource.indexOf(this.SelectedItem);

            //if (index !== -1) {
            //    this.DataSource.splice(index, 1);
            //}
            //else {
            //    var temp = this.DataSource.filter(a => a.Items.indexOf(this.SelectedItem) !== -1)[0];
            //    if (temp) {
            //        index = this.DataSource.filter(a => a.Items.indexOf(this.SelectedItem) !== -1)[0].Items.indexOf(this.SelectedItem);
            //        if (index !== -1) {
            //            this.DataSource.filter(a => a.Items.indexOf(this.SelectedItem) !== -1)[0].Items.splice(index, 1);
            //        }
            //    }
            //}
            var tempData = this.SelectedFieldsDataSource;
            tempData.push(this.SelectedItem);
            this.SelectedFieldsDataSource = this.ResetIndexes(tempData);
            this.SampleData = [];
            this.SaveChanges();
        }
    }

    btnRemove_Click(item) {
        this.FieldSelectedItem = item;
        if (this.FieldSelectedItem) {
            var index = this.SelectedFieldsDataSource.indexOf(this.FieldSelectedItem);

            if (index !== -1) {
                this.SelectedFieldsDataSource.splice(index, 1);
            }
            this.SelectedFieldsDataSource = this.ResetIndexes(this.SelectedFieldsDataSource);
            this.SampleData = [];
            this.SaveChanges();
            //var temp = this.DataSource.filter(a => a.Code == parentCode)[0];
            //if (this.FieldSelectedItem.parentCode) {
            //    //index = this.DataSource.filter(a => a.Items.indexOf(this.FieldSelectedItem) !== -1)[0].Items.indexOf(this.FieldSelectedItem);
            //    var tempData = this.DataSource.filter(a => a.Code == this.FieldSelectedItem.parentCode)[0].Items;
            //    tempData.push(this.FieldSelectedItem);
            //    this.DataSource.filter(a => a.Code == this.FieldSelectedItem.parentCode)[0].Items = tempData;

            //}
            //else {

            //    var tempData1 = this.DataSource;
            //    tempData1.push(this.FieldSelectedItem);
            //    this.DataSource = tempData1;
            //}
        }
    }
    RootGroups: DWObjectFieldsDetails[] = [];
    btnAddFilter_Click(item) {
        //this.SelectedItem = item;
        var view = new DWObjectFieldsDetails(item.BaseDWObjectField, this);
        if (view.DWObjectTableCode.indexOf("DIM_") != -1) {
            view.ParentDataTypeCode = "LookUp";
            if (item.BaseDWObjectField.DataTypeCode == "LookUp" || item.BaseDWObjectField.DataTypeCode == "Dimension") {
                view.ParentDimTabelName = item.BaseDWObjectField.DimensionTableCode;
            }
            else {
                view.ParentDimTabelName = item.BaseDWObjectField.DWObjectTableCode;
            }
        }
        else {
            view.ParentDataTypeCode = item.BaseDWObjectField.DataTypeCode;
        }
        this.SelectedItem = view;
        if (this.SelectedItem && this.SelectedFiltersDataSource.indexOf(this.SelectedItem) == -1) {//&& this.SelectedItem.DataTypeCode != "LookUp" && this.SelectedItem.DataTypeCode != "Dimension"
            if (this.SelectedFiltersDataSource.length == 0) {
                var DWObjectField = new DWObjectFieldsDetails(null, this);
                DWObjectField.IsGroup = true;
                DWObjectField.IndexOrder = this.SelectedFiltersDataSource.length;
                DWObjectField.FilterItems.push(this.SelectedItem);
                this.SelectedFiltersDataSource.push(DWObjectField);
            }
            else {
                var tempData = this.SelectedFiltersDataSource[0].FilterItems;
                tempData.push(this.SelectedItem);
                this.SelectedFiltersDataSource[0].FilterItems = tempData;
                var tempDataNew = this.SelectedFiltersDataSource;
                this.SelectedFiltersDataSource = [];
                //tempDataNew.push(this.SelectedItem);
                this.SelectedFiltersDataSource = tempDataNew;
            }
            //this.SelectedFiltersDataSourceChanged.emit(this.SelectedFiltersDataSource);
            if (this.CD) {
                this.CD.detectChanges();
            }
            //var tempData = this.SelectedFiltersDataSource;
            //tempData.push(this.SelectedItem);
            //this.SelectedFiltersDataSource = tempData;
            //this.RootGroups[0].FilterItems = tempData;
            if (this.SelectedItem.DataTypeCode == "Boolean") {
                this.SaveChanges();
            }
        }
    }

    btnRemoveFilter_Click() {
        if (this.FilterSelectedItem) {
            var index = this.SelectedFiltersDataSource.indexOf(this.FilterSelectedItem);

            if (index !== -1) {
                this.SelectedFiltersDataSource.splice(index, 1);
            }
            this.SaveChanges();
            //var temp = this.DataSource.filter(a => a.Code == parentCode)[0];
            //if (this.FilterSelectedItem.parentCode) {
            //    //index = this.DataSource.filter(a => a.Items.indexOf(this.FilterSelectedItem) !== -1)[0].Items.indexOf(this.FilterSelectedItem);
            //    var tempData = this.DataSource.filter(a => a.Code == this.FilterSelectedItem.parentCode)[0].Items;
            //    tempData.push(this.FilterSelectedItem);
            //    this.DataSource.filter(a => a.Code == this.FilterSelectedItem.parentCode)[0].Items = tempData;

            //}
            //else {

            //    var tempData1 = this.DataSource;
            //    tempData1.push(this.FilterSelectedItem);
            //    this.DataSource = tempData1;
            //}
        }
    }
    WhereStmt: string = " where ";
    GetWhereStmtForFiltersList(FiltersList: DWObjectFieldsDetails[], AndOr: string) {


        FiltersList.forEach((Myfilter) => {

            var isHaveMultiSelect = false;
            if (Myfilter.FilterItems.length > 0) {
                if (this.GetIfFiltersHaveValues(Myfilter.FilterItems) == true) {
                    this.WhereStmt = this.WhereStmt + " ( ";
                }
                this.GetWhereStmtForFiltersList(Myfilter.FilterItems, Myfilter.AndOr);
                if (this.WhereStmt == " where ") {
                    this.WhereStmt = "";
                }
                else if (this.WhereStmt.substring(this.WhereStmt.length - 4).indexOf("And") != -1 || this.WhereStmt.substring(this.WhereStmt.length - 4).indexOf("Or") != -1) {
                    this.WhereStmt = this.WhereStmt.substring(0, this.WhereStmt.length - 4);
                }
                if (this.WhereStmt != "" && this.GetIfFiltersHaveValues(Myfilter.FilterItems) == true) {
                    this.WhereStmt = this.WhereStmt + " ) ";
                }

            }
            else {
                //Myfilter.FilterItems.filter(a => a.TextValue != null).forEach((filter) => {
                if (!AppTool.IsNullOrEmpty(Myfilter.TextValue)) {
                    var filter = Myfilter;
                    var OperationSimpol = "";
                    if (filter.Operation.Code == filter.equalsOp.Code) {
                        if (filter.DataTypeCode == 'Integer' || filter.DataTypeCode == 'Double' || filter.DataTypeCode == 'Decimal') {
                            OperationSimpol = " = @@ ";
                        }
                        else {

                            OperationSimpol = " IN ( '";
                            OperationSimpol = this.BuildMultiValueSql(filter.TextValue, OperationSimpol);
                            isHaveMultiSelect = true;


                        }
                    }
                    else if (filter.Operation.Code == filter.notEqualsOp.Code) {
                        if (filter.DataTypeCode == 'Integer' || filter.DataTypeCode == 'Double' || filter.DataTypeCode == 'Decimal') {
                            OperationSimpol = " <> @@ ";
                        }
                        else {

                            OperationSimpol = " not IN ( '";
                            OperationSimpol = this.BuildMultiValueSql(filter.TextValue, OperationSimpol);
                            isHaveMultiSelect = true;

                            //abed
                        }                       
                    }
                    else if (filter.Operation.Code == filter.startsWithOp.Code) {
                        OperationSimpol = " like '@@%' ";
                    }
                    //else if (filter.Operation.Code == filter.IsNullOp.Code) {
                    //    OperationSimpol = " like '%@@' ";
                    //}
                    else if (filter.Operation.Code == filter.IsNullOp.Code) {
                        OperationSimpol = " is null ";
                    }
                    else if (filter.Operation.Code == filter.IsNotNullOp.Code) {
                        OperationSimpol = " is not null ";
                    }
                    else if (filter.Operation.Code == filter.greaterThanOrEqualOp.Code) {
                        OperationSimpol = " >= @@ ";
                    }
                    else if (filter.Operation.Code == filter.largerThanOp.Code) {
                        OperationSimpol = " > @@ ";
                    }
                    else if (filter.Operation.Code == filter.lessThanOp.Code) {
                        OperationSimpol = " < @@ ";
                    }
                    else if (filter.Operation.Code == filter.lessThanOrEqualOp.Code) {
                        OperationSimpol = " <= @@ ";
                    }


       
                    if (filter.Operation.Code == filter.IsNullOp.Code) {
                        this.WhereStmt += (filter.ParentDimTabelName ? filter.ParentDimTabelName : filter.DWObjectTableCode) + "." + filter.Code + " is null or " + (filter.ParentDimTabelName ? filter.ParentDimTabelName : filter.DWObjectTableCode) + "." + filter.Code + " = '' " + " " + AndOr + " ";
                    }
                    else if (filter.Operation.Code == filter.IsNotNullOp.Code) {
                        this.WhereStmt += (filter.ParentDimTabelName ? filter.ParentDimTabelName : filter.DWObjectTableCode) + "." + filter.Code + " is not null and " + (filter.ParentDimTabelName ? filter.ParentDimTabelName : filter.DWObjectTableCode) + "." + filter.Code + " <> '' " + " " + AndOr + " ";
                    }
                    else {
                        var operation = !isHaveMultiSelect ? OperationSimpol.replace("@@", filter.TextValue) : OperationSimpol;
                        this.WhereStmt += (filter.ParentDimTabelName ? filter.ParentDimTabelName : filter.DWObjectTableCode) + "." + filter.Code + operation + " " + AndOr + " ";//" = " + "'" + filter.TextValue + "' and ";
                    }
                }
                else {
                    //if (this.WhereStmt == " where  ( ") {
                    //    this.WhereStmt = "";
                    //}
                }
                //});
            }
            //if (Myfilter.IsGroup == true) {
            //    this.WhereStmt = this.WhereStmt + " ) ";
            //}
        });



        //return WhereStmt;
    }


    BuildMultiValueSql(textValue: any, operationSimpol: string) {

        var result = operationSimpol;
        if (textValue) {
            var values: string[] = textValue.toString().split(',');
            if (values.length > 0) {
                values.forEach((item) => {
                    if (item) {
                        result += (item + "','");
                    }
                });

                result += ")";
                result = result.replace(",')", ")");

            } else result += " ')";

        } else result += " ')";

        return result;
    }

    GetIfFiltersHaveValues(FiltersList: DWObjectFieldsDetails[]) {
        return FiltersList.filter(a => !AppTool.IsNullOrEmpty(a.TextValue)).length > 0;
    }

    GetWhereJoined(FiltersList: DWObjectFieldsDetails[]) {


        FiltersList.forEach((Myfilter) => {
            if (Myfilter.FilterItems.length > 0) {
                this.GetWhereJoined(Myfilter.FilterItems);
            }
            else {
                if (Myfilter.ParentDimTabelName != null && this.InnerTables.filter(a => a.ParentDimTabelName == Myfilter.ParentDimTabelName).length == 0) {
                    this.InnerTables.push(Myfilter);
                }
            }
        });

    }
    TempFilters: any[] = [];
    DeleteField(Item: DWObjectFieldsDetails, ListItems: DWObjectFieldsDetails[]) {


        ListItems.forEach((Myfilter) => {
            if (Myfilter.FilterItems.length > 0) {// Myfilter.FilterItems.indexOf(Item) > 
                this.DeleteField(Item, Myfilter.FilterItems);
            }
            else {
                if (Myfilter == Item) {
                    ListItems = ListItems.filter(a => a != Item);
                }
            }
        });

    }

    InnerTables: any[] = [];
    SaveChanges(StopPreview: boolean = false) {
        this.InnerTables = [];
        this.SampleData = [];
        if (this.SelectedFieldsDataSource.length == 0) {
            return;
        }
        this.IsPreview = false;
        var SelectStmt = "Select ";
        var GroupByStmt = " group by ";
        //var Wheremt = " Where ";
        this.WhereStmt = " where ";
        var HasMeasurement: boolean = false;
        var FromTables = [];
        this.SelectedFieldsDataSource.forEach((field) => {
            if (field.IsMeasurement) {
                HasMeasurement = true;
                SelectStmt += field.AggregationTypeCode + "(" + field.DWObjectTableCode + "." + field.Code + ")" + (field.DisplayName ? " as " + field.DisplayName + "," : ",");
            }
            else {
                SelectStmt += field.DWObjectTableCode + "." + field.Code + (field.DisplayName ? " as " + field.DisplayName + "," : ",");
                GroupByStmt += field.DWObjectTableCode + "." + field.Code + ",";
            }

            if (FromTables.filter(a => a == field.DWObjectTableCode).length == 0) {
                FromTables.push(field.DWObjectTableCode);
            }
        });

        SelectStmt = SelectStmt.substring(0, SelectStmt.length - 1);
        GroupByStmt = GroupByStmt.substring(0, GroupByStmt.length - 1);
        var Fact = FromTables.filter(a => a.indexOf("Fact") != -1)[0];
        if (AppTool.IsNullOrEmpty(Fact)) {
            Fact = "Fact_Shipments";
        }
        SelectStmt += " from " + Fact;
        //var InnerTblsTpl = this.SelectedFieldsDataSource.filter(a => a.ParentDimTabelName != null);


        this.SelectedFieldsDataSource.forEach((field) => {
            if (field.ParentDimTabelName != null && this.InnerTables.filter(a => a.ParentDimTabelName == field.ParentDimTabelName).length == 0) {
                this.InnerTables.push(field);
            }
        });

        //this.SelectedFiltersDataSource.forEach((Myfilter) => {
        //    Myfilter.FilterItems.forEach((filter) => {
        //        if (filter.ParentDimTabelName != null && this.InnerTables.filter(a => a.ParentDimTabelName == filter.ParentDimTabelName).length == 0) {
        //            this.InnerTables.push(filter);
        //        }
        //    }); 
        //});
        if (this.SelectedFiltersDataSource.length > 0) {
            this.GetWhereJoined(this.SelectedFiltersDataSource);
            this.GetWhereStmtForFiltersList(this.SelectedFiltersDataSource, this.SelectedFiltersDataSource[0].AndOr);
        }

        //this.Notes = SelectStmt;
        FromTables = FromTables.filter(a => a != Fact);

        this.InnerTables.forEach((mytbl) => {
            var Key = this.AllFieldsObsList.filter(a => a.DWObjectTableCode == mytbl.ParentDimTabelName && a.IsPrimaryKey == true)[0];
            var FactKey = this.AllFieldsDataSource.filter(a => a.DimensionTableCode == mytbl.ParentDimTabelName)[0];
            SelectStmt += " inner join " + mytbl.ParentDimTabelName + " on " + Fact + "." + ((FactKey.DataTypeCode.toLowerCase() == "lookup" || FactKey.DataTypeCode.toLowerCase() == "dimension") ? FactKey.DisplayName : FactKey.Code) + " = " + mytbl.ParentDimTabelName + "." + Key.Code

            //this._DWObjectFieldPMService.getDWObjectFieldsByDWTableId(mytbl.ParentDimTabelName).subscribe(Result => {
            //    if (!Result.HasError) {
            //        var Key = Result.Result.filter(a => a.IsPrimaryKey == true)[0];
            //        var FactKey = this.AllFieldsDataSource.filter(a => a.DimensionTableCode == mytbl.ParentDimTabelName)[0];
            //        SelectStmt += " inner join " + mytbl.ParentDimTabelName + " on " + Fact + "." + FactKey.Code + " = " + mytbl.ParentDimTabelName + "." + Key.Code
            //        //.forEach((field) => {
            //        //    var view = new DWObjectFieldsDetails(field);
            //        //    this.ObsList.push(view);
            //        //    this.ObsListAll.push(view);
            //        //});
            //        //this.DataSource = this.ObsList;
            //    }
            //    if (this.SelectedFiltersDataSource.length > 0) {
            //        this.Notes = SelectStmt + this.WhereStmt + (HasMeasurement ? GroupByStmt : "");
            //        this.PreviewData(StopPreview);
            //    }
            //    else {
            //        this.Notes = SelectStmt + (HasMeasurement ? GroupByStmt : "");
            //        this.PreviewData(StopPreview);
            //    }

            //});

        });
        //if (this.InnerTables.length == 0) {
        if (this.SelectedFiltersDataSource.length > 0) {
            this.Notes = SelectStmt + this.WhereStmt + (HasMeasurement && GroupByStmt != " group by" ? GroupByStmt : "");
            if (GroupByStmt != " group by" || this.Notes.indexOf(" group by") == -1) {
                this.PreviewData(StopPreview);
            }
        }
        else {
            this.Notes = SelectStmt + (HasMeasurement && GroupByStmt != " group by" ? GroupByStmt : "");
            if (GroupByStmt != " group by" || this.Notes.indexOf(" group by") == -1) {
                this.PreviewData(StopPreview);
            }
        }
        //}

        //this.SelectedFiltersDataSource.forEach((filter) => {
        //    WhereStmt += filter.ParentDimTabelName + "." + filter.Code + " = " + filter.TextValue;
        //}); 

    }

    SampleData: any[] = [];
    CancelButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindow();
    }
    IsPreview: boolean = true;
    PreviewData(StopPreview: boolean = false) {
        if (this.Notes) {
            if (StopPreview == true) {
                return;
            }
            this.IsPreview = true;
            this.StartBusyIndicator("Loading ..");
            var tempSQL = this.Notes.replace("Select ", "Select top 100 ").trim();
            this._DWQueryBuilderService.GetDWQueryData(tempSQL, "Fact_Shipments").subscribe(myResult => {
                if (!myResult.HasError) {
                    this.SampleData = myResult.Result;
                    this.StopBusyIndicator();
                }

            });
        }

    }
    ShowSQL() {
        this.SaveChanges(true);
    }

    AddFilterToGroup() {
        var DWObjectField = new DWObjectFieldsDetails(null, this);
        DWObjectField.IndexOrder = this.SelectedFiltersDataSource.length;
        //this.Name = DWObjectField.Name;
        //this.Code = DWObjectField.Code;
        //this.DWObjectTableCode = DWObjectField.DWObjectTableCode;
        //this.DataTypeCode = DWObjectField.DataTypeCode;
        //this.DimensionTableCode = DWObjectField.DimensionTableCode;
        //this.DisplayName = DWObjectField.Code;
        //this.IsPrimaryKey = DWObjectField.IsPrimaryKey;
        //this.IsMeasurement = DWObjectField.IsMeasurement;
        //this.AggregationTypeCode = DWObjectField.AggregationTypeCode;
        //this.IndexOrder = ParentClass.SelectedFieldsDataSource.length;
        //this.SelectedFiltersDataSource.push(DWObjectField);
        var tempData = this.SelectedFiltersDataSource[0].FilterItems;
        tempData.push(this.SelectedItem);
        this.SelectedFiltersDataSource[0].FilterItems = tempData;
    }
    AddGroup() {
        var DWObjectField = new DWObjectFieldsDetails(null, this);
        DWObjectField.IsGroup = true;
        DWObjectField.IndexOrder = this.SelectedFiltersDataSource.length;
        var DWInnerObjectField = new DWObjectFieldsDetails(null, this);
        DWInnerObjectField.IndexOrder = DWObjectField.FilterItems.length;
        DWObjectField.FilterItems.push(DWInnerObjectField);
        var tempData = this.SelectedFiltersDataSource[0].FilterItems;
        tempData.push(this.SelectedItem);
        this.SelectedFiltersDataSource[0].FilterItems = tempData;
        //this.SelectedFiltersDataSource.push(DWObjectField);
    }

    public BusyIndicatorText: string = null;
    public ShowBusyIndicator: boolean = false;
    public StartBusyIndicator(myText: string) {
        this.BusyIndicatorText = myText;
        this.ShowBusyIndicator = true;
    }

    public StopBusyIndicator() {
        this.BusyIndicatorText = null;
        this.ShowBusyIndicator = false;
    }

    private qID: string;
    public get QID() { return this.qID; }
    public set QID(newValue: string) {
        this.qID = newValue;
    }

    private iD: string;
    public get ID() { return this.iD; }
    public set ID(newValue: string) {
        this.iD = newValue;
    }
    private messageWindow: MessageWindow = new MessageWindow();
    SaveButtonClicked() {
        //this.messageWindow.Width = 300;
        //this.messageWindow.Height = 150;
        //this.messageWindow.Title = "";
        //this.messageWindow.Message = "Shipment (" + myResult.Result.ForwarderShipmentNumber + ") with the same order number is alreay exist in the query";
        //this.messageWindow.Show(this.messageWindow.Message);
        //this.messageWindow.WindowClosed.subscribe(($event) => {

        //});
        if (this.SelectedFieldsDataSource.length == 0) {
            this.messageWindow.Width = 300;
            this.messageWindow.Height = 150;
            this.messageWindow.Title = "Invalid Query";
            this.messageWindow.Message = "The query should contain at least one column.";
            this.messageWindow.Show(this.messageWindow.Message);
            return;
        }
        SessionLocator.CurrentSession.CurrentWindow.StartBusyIndicator("Saving ..");
        this._DWObjectTablePMService.get("Fact_Shipments").subscribe(myResult => {
            if (!myResult.HasError) {
                var MySubQuery = new DWSubQueryPM();
                MySubQuery.Tenant = SessionLocator.Tenant;
                MySubQuery.DWFactTableCode = myResult.Result.Code;
                //MySubQuery.DWQueryId = '1-1';
                MySubQuery.SQLString = this.Notes;
                var QueryData = new DWQueryData();
                QueryData.SubQueryData = MySubQuery;
                QueryData.Columns = this.SelectedFieldsDataSource;
                QueryData.Filters = this.SelectedFiltersDataSource[0];
                if (AppTool.IsNullOrEmpty(this.ID)) {
                    this._DWSubQueryPMService.insertDWQueryData(QueryData).subscribe(myResult => {
                        //if (!myResult.HasError) {

                        //}
                        this.ID = myResult.Result.Id;
                        this.QID = myResult.Result.DWQueryId
                        this.EditButtonClicked();
                        SessionLocator.CurrentSession.CurrentWindow.StopBusyIndicator(); 
                    });
                }
                else {
                    MySubQuery.Id = this.ID;
                    if (this.NotExist == true) {
                        this.messageWindow.Width = 300;
                        this.messageWindow.Height = 150;
                        this.messageWindow.Title = "Query Doesn't Exist";
                        this.messageWindow.Message = "Query With the Id " + this.ID + " does not exist";
                        this.messageWindow.Show(this.messageWindow.Message);
                        this.NotExist = true;
                    }
                    this._DWSubQueryPMService.UpdateDWQueryData(QueryData).subscribe(myResult => {
                        //if (!myResult.HasError) {

                        //}
                        SessionLocator.CurrentSession.CurrentWindow.StopBusyIndicator();
                    });
                }
            }

        });
    }
    NotExist: boolean = true;
    EditButtonClicked() {
        this.SelectedFieldsDataSource = [];
        this.SelectedFiltersDataSource = [];
        if (!AppTool.IsNullOrEmpty(this.ID)) {
            var QueryData = new DWQueryData();
            this._DWSubQueryPMService.get(this.ID).subscribe(myResult => {
                if (myResult.Result == null) {
                    this.messageWindow.Width = 300;
                    this.messageWindow.Height = 150;
                    this.messageWindow.Title = "Query Doesn't Exist";
                    this.messageWindow.Message = "Query With the Id " + this.ID + " does not exist";
                    this.messageWindow.Show(this.messageWindow.Message);
                    this.NotExist = true;
                    return;
                }
                else {
                    this.NotExist = false;
                }
                if (!myResult.HasError) {
                    QueryData = myResult.Result;

                    ////////////////////////////////////////////////

                    var tempColumns = this.SelectedFieldsDataSource;
                    QueryData.Columns.forEach((field) => {
                        var view = new DWObjectFieldsDetails(field, this);
                        view.ParentDataTypeCode = field.ParentDataTypeCode;
                        view.DisplayName = field.DisplayName;
                        view.ParentCode = field.ParentCode;
                        view.ParentDimTabelName = field.ParentDimTabelName;

                        tempColumns.push(view);

                    });

                    this.SelectedFieldsDataSource = this.ResetIndexes(tempColumns);

                    //////////////////////////////////////////

                    ////////////////////////////////////////////////
                    var DWObjectField = new DWObjectFieldsDetails(null, this);
                    DWObjectField.IsGroup = true;
                    DWObjectField.AndOr = QueryData.Filters.AndOr;
                    DWObjectField.IndexOrder = this.SelectedFiltersDataSource.length;
                    var MyFilter = this.RestoreFilters(QueryData.Filters, DWObjectField);
                    var temp = [];
                    temp.push(MyFilter);
                    this.SelectedFiltersDataSource = temp;
                    //////////////////////////////////////////
                }
            });
        }
    }

    RestoreFilters(BaseFilter: DWObjectFieldsDetails, MyFilter: DWObjectFieldsDetails) {

        BaseFilter.FilterItems.forEach((field) => {
            var view = new DWObjectFieldsDetails(field, this);
            if (field.FilterItems.length == 0) {
                if (field.DWObjectTableCode.indexOf("DIM_") != -1) {
                    view.ParentDataTypeCode = "LookUp";
                    view.ParentDimTabelName = field.DWObjectTableCode;
                }
                else {
                    view.ParentDataTypeCode = field.DataTypeCode;
                }
            }
            //field = view;
            //else {
            //    var DWObjectField = new DWObjectFieldsDetails(null, this);
            //    DWObjectField.IsGroup = true;
            //    DWObjectField.IndexOrder = this.SelectedFiltersDataSource.length; 
            //    //field.FilterItems.forEach((innerfield) => {
            //    //    //var myview = new DWObjectFieldsDetails(field, this);
            //    //    //if (field.FilterItems.length == 0) {
            //    //    //    if (field.DWObjectTableCode.indexOf("DIM_") != -1) {
            //    //    //        view.ParentDataTypeCode = "LookUp";
            //    //    //        view.ParentDimTabelName = field.DWObjectTableCode;
            //    //    //    }
            //    //    //    else {
            //    //    //        view.ParentDataTypeCode = field.DataTypeCode;
            //    //    //    }
            //    //    //}
            //    //    //DWObjectField.FilterItems.push(view);
            //    //});
            //    //DWObjectField.IsGroup = true;
            //    //DWObjectField.IndexOrder = this.SelectedFiltersDataSource.length;
            //    //DWObjectField.FilterItems.push(view);
            //    this.RestoreFilters(field); 
            //    this.SelectedFiltersDataSource.push(DWObjectField);
            //    return;
            //}
            if (field.FilterItems.length == 0) {
                view.TextValue = field.TextValue;
                view.MultSelectValueLists = this.MapMultSelectValueLists(field.MultSelectValueLists);
                view.Operation = new ObjectFieldOperator(field.OperationCode, field.OperationName);
                MyFilter.FilterItems.push(view);
            }
            else {
                var DWObjectField = new DWObjectFieldsDetails(null, this);
                DWObjectField.IsGroup = true;
                DWObjectField.IndexOrder = MyFilter.FilterItems.length;
                DWObjectField.AndOr = field.AndOr;
                this.RestoreFilters(field, DWObjectField);
                MyFilter.FilterItems.push(DWObjectField);
                //var tempData = this.SelectedFiltersDataSource[0].FilterItems;
                //tempData.push(view);
                //this.SelectedFiltersDataSource[0].FilterItems = tempData;
                //var tempDataNew = this.SelectedFiltersDataSource;
                //this.SelectedFiltersDataSource = [];
                ////tempDataNew.push(this.SelectedItem);
                //this.SelectedFiltersDataSource = tempDataNew;
            }
            //this.RestoreFilters(field); 

        });

        return MyFilter;

        //this.SelectedFieldsDataSource = this.ResetIndexes(tempFilters);
    }


    MapMultSelectValueLists(lists: MultSelectValue[]) {
        var result: MultSelectValue[] = [];
        if (lists) {
            lists.forEach((field) => {
                var item: MultSelectValue = new MultSelectValue();
                var i = "";
                var j = 0;
                while (field["Value" + i]) {
                    var fieldDetails: FieldDetails = new FieldDetails();
                    fieldDetails.Column = field["Value" + i].Column;
                    fieldDetails.Row = field["Value" + i].Row;
                    item["Value" + i] = fieldDetails;
                    j += 1;
                    i = j.toString();

                }
                result.push(item);

            });

            return result;
        }
    }
}

export class DWObjectFieldsDetails extends BaseComponent {
    public MyParentClass: DWQueryBuilderComponent;
    public BaseDWObjectField: any;
    constructor(DWObjectField: any = null, ParentClass: DWQueryBuilderComponent = null) {
        super();
        this.BaseDWObjectField = DWObjectField;
        if (ParentClass != null) {
            this.MyParentClass = ParentClass;
            this.IndexOrder = ParentClass.SelectedFieldsDataSource.length;
        }
        if (DWObjectField != null) {
            this.LOVAdditionalColumns = DWObjectField.LOVAdditionalColumns;
            if (!this.LOVAdditionalColumns && ParentClass && ParentClass.AllFieldsDataSource ) {
                var field = ParentClass.AllFieldsDataSource.filter(a => a.DWObjectTableCode == DWObjectField.DWObjectTableCode && a.Code == DWObjectField.Code && a.Name == DWObjectField.Name)[0];
                if (field) {
                    this.LOVAdditionalColumns = DWObjectField.LOVAdditionalColumns = field.LOVAdditionalColumns;
                }

            }



            this.Name = DWObjectField.Name;
            this.Code = DWObjectField.Code;
            this.DWObjectTableCode = DWObjectField.DWObjectTableCode;
            this.DimensionTableCode = DWObjectField.DimensionTableCode;
            this.DataTypeCode = DWObjectField.DataTypeCode;
            this.DisplayName = DWObjectField.Code;
            this.IsPrimaryKey = DWObjectField.IsPrimaryKey;
            this.IsMeasurement = DWObjectField.IsMeasurement;
            this.AggregationTypeCode = DWObjectField.AggregationTypeCode;
            //this.Name = DWObjectField.Name;
        }

    }


    Items: any[] = [];
    FilterItems: DWObjectFieldsDetails[] = [];
    private lOVAdditionalColumns: string;
    public get LOVAdditionalColumns() { return this.lOVAdditionalColumns; }
    public set LOVAdditionalColumns(newValue: string) { this.lOVAdditionalColumns = newValue; }

    private category1: string;
    public get Category1() { return this.category1; }
    public set Category1(newValue: string) { this.category1 = newValue; }


    private category2: string;
    public get Category2() { return this.category2; }
    public set Category2(newValue: string) { this.category2 = newValue; }

    private indexOrder: number;
    public get IndexOrder() { return this.indexOrder; }
    public set IndexOrder(newValue: number) { this.indexOrder = newValue; }

    private isGroup: boolean = false;
    public get IsGroup() { return this.isGroup; }
    public set IsGroup(newValue: boolean) { this.isGroup = newValue; }

    private isPrimaryKey: boolean;
    public get IsPrimaryKey() { return this.isPrimaryKey; }
    public set IsPrimaryKey(newValue: boolean) { this.isPrimaryKey = newValue; }

    private showBtns: boolean = false;
    public get ShowBtns() { return this.showBtns; }
    public set ShowBtns(newValue: boolean) { this.showBtns = newValue; }

    private isViewTree: boolean;
    public get IsViewTree() { return this.isViewTree; }
    public set IsViewTree(newValue: boolean) { this.isViewTree = newValue; }


    private hasTree: boolean;
    public get HasTree() { return this.hasTree; }
    public set HasTree(newValue: boolean) { this.hasTree = newValue; }


    private name: string;
    public get Name() { return this.name; }
    public set Name(newValue: string) { this.name = newValue; }

    private Displayname: string;
    public get DisplayName() { return this.Displayname; }
    public set DisplayName(newValue: string) { this.Displayname = newValue; }


    private code: string;
    public get Code() { return this.code; }
    public set Code(newValue: string) { this.code = newValue; }

    private parentcode: string;
    public get ParentCode() { return this.parentcode; }
    public set ParentCode(newValue: string) { this.parentcode = newValue; }

    private parentDimTabelName: string;
    public get ParentDimTabelName() { return this.parentDimTabelName; }
    public set ParentDimTabelName(newValue: string) { this.parentDimTabelName = newValue; }


    private dWObjectTableCode: string;
    public get DWObjectTableCode() { return this.dWObjectTableCode; }
    public set DWObjectTableCode(newValue: string) { if (this.dWObjectTableCode != newValue) { this.dWObjectTableCode = newValue; } }

    private isMeasurement: boolean;
    public get IsMeasurement() { return this.isMeasurement; }
    public set IsMeasurement(newValue: boolean) { if (this.isMeasurement != newValue) { this.isMeasurement = newValue; } }

    private aggregationTypeCode: string;
    public get AggregationTypeCode() { return this.aggregationTypeCode; }
    public set AggregationTypeCode(newValue: string) { if (this.aggregationTypeCode != newValue) { this.aggregationTypeCode = newValue; } }

    private dataTypeCode: string;
    public get DataTypeCode() { return this.dataTypeCode; }
    public set DataTypeCode(newValue: string) {
        //if (this.dataTypeCode != newValue) {
        this.dataTypeCode = newValue;
        if (this.dataTypeCode == "Boolean") {
            this.TextValue = false;
        }
        if (this.dataTypeCode == "LookUp" || this.dataTypeCode == "Dimension") {
            this.HasTree = true;
            var MyTable = this.MyParentClass.AllTables.filter(a => a.Code == this.DimensionTableCode);
            if (MyTable && MyTable.length > 0) {
                this.Code = MyTable[0].DefaultFilterBy;
                this.DWObjectTableCode = MyTable[0].Code;
                this.DisplayName = this.DWObjectTableCode + this.Code;
                this.ParentDimTabelName = this.DimensionTableCode;
            }
        }
        else {
            this.HasTree = false;
        }
        //}
    }

    private parentDataTypeCode: string;
    public get ParentDataTypeCode() { return this.parentDataTypeCode; }
    public set ParentDataTypeCode(newValue: string) {
        if (this.parentDataTypeCode != newValue) {
            this.parentDataTypeCode = newValue;
        }
    }


    private dimensionTableCode: string;
    public get DimensionTableCode() { return this.dimensionTableCode; }
    public set DimensionTableCode(newValue: string) { if (this.dimensionTableCode != newValue) { this.dimensionTableCode = newValue; } }

    private operators: ObjectFieldOperator[];
    public get Operators() { return this.GetFieldOperators(this); }
    public set Operators(newValue: ObjectFieldOperator[]) {
        this.operators = newValue;
    }

    private textValue: any;
    public get TextValue() {
        return this.textValue;
    }
    public set TextValue(newValue: any) {
        this.textValue = newValue;
        this.MyParentClass.SaveChanges();
    }

    private multSelectValueLists: MultSelectValue[];
    public get MultSelectValueLists() {
        return this.multSelectValueLists;
    }
    public set MultSelectValueLists(newValue: MultSelectValue[]) {
        this.multSelectValueLists = newValue;
        this.MyParentClass.SaveChanges();
    }



    private operationName: string;
    public get OperationName() { return this.operationName; }
    public set OperationName(newValue: string) {
        if (this.operationName != newValue) {
            this.operationName = newValue;
        }
    }

    private operationCode: string;
    public get OperationCode() { return this.operationCode; }
    public set OperationCode(newValue: string) {
        if (this.operationCode != newValue) {
            this.operationCode = newValue;
        }
    }

    private operation: ObjectFieldOperator;
    public get Operation() {
        if (!this.operation) {
            if ((this.ParentDataTypeCode == "Text" || this.ParentDataTypeCode == "nText") && AppTool.IsNullOrEmpty(this.operation)) {
                this.operation = new ObjectFieldOperator("StartsWith", "Starts With");
                this.OperationCode = "StartsWith";
                this.OperationName = "Starts With";
                return this.operation;
            }
            else {
                if (AppTool.IsNullOrEmpty(this.operation)) {
                    this.operation = new ObjectFieldOperator("Equals", "Equals to");
                }
                this.OperationCode = "Equals";
                this.OperationName = "Equals to";
                return this.operation;
            }
        }
        else {
            return this.operation;
        }
    }
    public set Operation(newValue: ObjectFieldOperator) {
        this.OperationCode = newValue.Code;
        this.OperationName = newValue.Name;
        this.operation = newValue;
    }

    private andOr: string;
    public get AndOr() {
        if (AppTool.IsNullOrEmpty(this.andOr)) {
            return "And";
        }
        return this.andOr;
    }
    public set AndOr(newValue: string) {
        this.andOr = newValue;
        this.MyParentClass.SaveChanges();
    }

    OperationValueChanged(operation) {

        if (this.Operation.Code == this.IsNullOp.Code || this.Operation.Code == this.IsNotNullOp.Code) {
            this.TextValue = "";
            this.MultSelectValueLists = [];
        }

        this.Operation = operation;
        if (operation.Code == this.IsNullOp.Code || operation.Code == this.IsNotNullOp.Code) {
            this.TextValue = operation.Code;
        }
        if (operation.Code == this.IsNullOp.Code || operation.Code == this.IsNotNullOp.Code || !AppTool.IsNullOrEmpty(this.TextValue)) {
            this.MyParentClass.SaveChanges();
        }
    }

    LoadItems(DWObjectField: any) {
        if (this.IsViewTree) {
            this.IsViewTree = false;
        }
        else {
            if (this.Items.length == 0) {
                this.Load(DWObjectField);
            }
            else this.IsViewTree = true;
        }
    }

    Load(DWObjectField: any) {
        var _DWObjectTablePMService = new DWObjectTablePMService();
        var _DWObjectFieldPMService = new DWObjectFieldExtendedPMService();
        var ObsList = [];
        _DWObjectTablePMService.get(DWObjectField.DimensionTableCode).subscribe(myResult => {
            if (!myResult.HasError) {
                _DWObjectFieldPMService.getDWObjectFieldsByDWTableId(myResult.Result.Code).subscribe(Result => {
                    if (!Result.HasError) {
                        Result.Result.forEach((field) => {
                            if (field.DisplayInQueryBuilder == true) {
                                var view = new DWObjectFieldsDetails(field, this.MyParentClass);
                                view.ParentDataTypeCode = DWObjectField.DataTypeCode;
                                if (!AppTool.IsNullOrEmpty(DWObjectField.Code)) {
                                    view.DisplayName = '[' + (DWObjectField.Code.replace('[', '').replace(']', '') + view.Code.replace('[', '').replace(']', '')) + ']';//.replace('[', '').replace('[', '').replace(']', '').replace(']', '');
                                }
                                else if (!AppTool.IsNullOrEmpty(DWObjectField.DisplayName)) {
                                    view.DisplayName = DWObjectField.DisplayName;
                                }
                                else {
                                    view.DisplayName = '[' + (DWObjectField.Name + view.Code.replace('[', '').replace(']', '')) + ']';//.replace('[', '').replace('[', '').replace(']', '').replace(']', '');

                                }
                                view.ParentCode = DWObjectField.Code;
                                view.ParentDimTabelName = DWObjectField.DimensionTableCode;

                                ObsList.push(view);
                            }
                            //this.ObsListAll.push(view);
                        });
                        this.Items = ObsList;
                        this.IsViewTree = true;
                    }

                });
            }

        });
    }

    onTextChange(value) {
        this.TextValue = value;
    }

    AndOrOpsChanged(value) {
        this.AndOr = value;
    }

    OnMouseOver(event) {
        //console.log("Over");
        var e = event.toElement;// || event.relatedTarget;
        if (e && e.className == "LinkBtn") {
            return;
        }
        if (this.ShowBtns == false) {
            this.ShowBtns = true;
        }
    }

    OnMouseOut(event) {
        //console.log("Out");
        var e = event.toElement;// || event.relatedTarget;
        if (e && e.className == "LinkBtn") {
            return;
        }
        if (this.ShowBtns == true) {
            this.ShowBtns = false;
        }
    }

    preventInnerHover(event) {
        event.stopPropagation();
    }

    AddFilterToGroup() {
        var DWObjectField = new DWObjectFieldsDetails();
        DWObjectField.IndexOrder = this.FilterItems.length;
        //this.Name = DWObjectField.Name;
        //this.Code = DWObjectField.Code;
        //this.DWObjectTableCode = DWObjectField.DWObjectTableCode;
        //this.DataTypeCode = DWObjectField.DataTypeCode;
        //this.DimensionTableCode = DWObjectField.DimensionTableCode;
        //this.DisplayName = DWObjectField.Code;
        //this.IsPrimaryKey = DWObjectField.IsPrimaryKey;
        //this.IsMeasurement = DWObjectField.IsMeasurement;
        //this.AggregationTypeCode = DWObjectField.AggregationTypeCode;
        //this.IndexOrder = ParentClass.SelectedFieldsDataSource.length;
        this.FilterItems.push(DWObjectField);
    }

    AddGroup(Father: DWObjectFieldsDetails) {
        var DWObjectField = new DWObjectFieldsDetails(null, Father.MyParentClass);
        DWObjectField.IsGroup = true;
        DWObjectField.IndexOrder = Father.MyParentClass.SelectedFiltersDataSource.length;
        var DWInnerObjectField = new DWObjectFieldsDetails(null, Father.MyParentClass);
        DWInnerObjectField.IndexOrder = DWObjectField.FilterItems.length;
        DWObjectField.FilterItems.push(DWInnerObjectField);
        Father.MyParentClass.SelectedFiltersDataSource.push(DWObjectField);
    }

    FieldValueChanged(DWObjectField: DWObjectFieldsDetails) {
        //this.MyParentClass = ParentClass;
        this.TextValue = "";
        this.MultSelectValueLists = [];
        this.Name = DWObjectField.Name;
        this.Code = DWObjectField.Code;
        this.DWObjectTableCode = DWObjectField.DWObjectTableCode;
        this.DataTypeCode = DWObjectField.DataTypeCode;
        this.DimensionTableCode = DWObjectField.DimensionTableCode;
        this.DisplayName = DWObjectField.Code;
        this.IsPrimaryKey = DWObjectField.IsPrimaryKey;
        this.IsMeasurement = DWObjectField.IsMeasurement;
        this.AggregationTypeCode = DWObjectField.AggregationTypeCode;
        this.LOVAdditionalColumns = DWObjectField.LOVAdditionalColumns;
        if (this.DWObjectTableCode.indexOf("DIM_") != -1) {
            this.ParentDataTypeCode = "LookUp";
            this.ParentDimTabelName = DWObjectField.DWObjectTableCode;
        }
        else {
            this.ParentDataTypeCode = DWObjectField.DataTypeCode;
            this.ParentDimTabelName = DWObjectField.ParentDimTabelName;
        }
        //this.ParentDataTypeCode = DWObjectField.DataTypeCode;

        this.Operators = this.GetFieldOperators(this);

        if ((this.ParentDataTypeCode == "Text" || this.ParentDataTypeCode == "nText")) {
            this.Operation = new ObjectFieldOperator("StartsWith", "Starts With");
        }
        else {
            this.Operation = new ObjectFieldOperator("Equals", "Equals to");
        }
        //this.IndexOrder = ParentClass.SelectedFieldsDataSource.length;
        //var Filters = DWObjectField.MyParentClass.SelectedFiltersDataSource;
        //DWObjectField.MyParentClass.SelectedFiltersDataSource = [];
        //DWObjectField.MyParentClass.SelectedFiltersDataSource = Filters;


        //this.MyParentClass.SelectedFiltersDataSource.where
    }

    onDeleteFilterClick() {
        this.MyParentClass.DeleteField(this, this.MyParentClass.SelectedFiltersDataSource);
        //var temp = this.MyParentClass.SelectedFieldsDataSource;
    }

    list: ObjectFieldOperator[];
    private GetFieldOperators(field: DWObjectFieldsDetails) {


        this.list = [];

        if (field.ParentDataTypeCode == "Text" || field.ParentDataTypeCode == "nText") {


            this.list.push(this.equalsOp);
            this.list.push(this.startsWithOp);
            this.list.push(this.IsNullOp);
            this.list.push(this.IsNotNullOp);

        }

        if (field.ParentDataTypeCode == "Integer" || field.ParentDataTypeCode == "UnsInteger"
            || field.ParentDataTypeCode == "Double" || field.ParentDataTypeCode == "SigDouble"
            || field.ParentDataTypeCode == "Decimal" || field.ParentDataTypeCode == "UnsDecimal"
            || field.ParentDataTypeCode == "DateTime" || field.ParentDataTypeCode == "Date") {
            this.list.push(this.largerThanOp);
            this.list.push(this.lessThanOp);
            this.list.push(this.equalsOp);
            this.list.push(this.greaterThanOrEqualOp);
            this.list.push(this.lessThanOrEqualOp);

        }


        if (field.ParentDataTypeCode == "LookUp" || field.ParentDataTypeCode == "Dimension" || field.ParentDataTypeCode == "PickList") {
            this.list.push(this.equalsOp);
            this.list.push(this.notEqualsOp);
            this.list.push(this.IsNullOp);
            this.list.push(this.IsNotNullOp);


        }
        if (field.ParentDataTypeCode == "Boolean") {
            this.list.push(this.equalsOp);
            this.list.push(this.notEqualsOp);
        }
        return this.list;
    }




    startsWithOp: ObjectFieldOperator = new ObjectFieldOperator("StartsWith", "Starts With");
    equalsOp: ObjectFieldOperator = new ObjectFieldOperator("Equals", "Equals to");
    notEqualsOp: ObjectFieldOperator = new ObjectFieldOperator("NotEqual", "Not Equal to");
    largerThanOp: ObjectFieldOperator = new ObjectFieldOperator("LargerThan", "Greater Than");
    lessThanOp: ObjectFieldOperator = new ObjectFieldOperator("LessThan", "Less Than");
    greaterThanOrEqualOp: ObjectFieldOperator = new ObjectFieldOperator("GreaterThanOrEqual", "Greater Than Or Equal");
    lessThanOrEqualOp: ObjectFieldOperator = new ObjectFieldOperator("LessThanOrEqual", "Less Than Or Equal");
    BetweenOp: ObjectFieldOperator = new ObjectFieldOperator("Between", "Between");
    IsNullOp: ObjectFieldOperator = new ObjectFieldOperator("IsNull", "Is Empty");
    IsNotNullOp: ObjectFieldOperator = new ObjectFieldOperator("IsNotNull", "Has Value");

}

export class ObjectFieldOperator {

    constructor(code: string, name: string) {
        this.Code = code;
        this.Name = name;
    }

    private code: string;
    public get Code() { return this.code; }
    public set Code(newValue: string) { this.code = newValue; }

    private name: string;
    public get Name() { return this.name; }
    public set Name(newValue: string) { this.name = newValue; }

}

export class DWFieldsGroup {

    constructor(Key: string, FieldsList: any[]) {
        this.Key = Key;
        this.FieldsList = FieldsList;
    }

    private key: string;
    public get Key() { return this.key; }
    public set Key(newValue: string) { this.key = newValue; }

    private fieldsList: any[];
    public get FieldsList() { return this.fieldsList; }
    public set FieldsList(newValue: any[]) { this.fieldsList = newValue; }

    private detailsIcon: string = "./Images/CellIcons/Arrowup.png";
    public get DetailsIcon() { return this.detailsIcon; }
    public set DetailsIcon(newValue: string) { this.detailsIcon = newValue; }

    private isDetailesOpened: boolean = false;
    public get IsDetailesOpened() { return this.isDetailesOpened; }
    public set IsDetailesOpened(newValue: boolean) { this.isDetailesOpened = newValue; }

    GroupClicked() {
        this.IsDetailesOpened = !this.IsDetailesOpened;
        if (!this.IsDetailesOpened) {
            this.DetailsIcon = "./Images/CellIcons/Arrowdown.png";
        }
        else {
            this.DetailsIcon = "./Images/CellIcons/Arrowup.png";
        }
    }
}


export class MultSelectValue {

    public  Value: FieldDetails;
    public  Value1: FieldDetails;
    public   Value2: FieldDetails;
    public   Value3: FieldDetails;
    public   Value4: FieldDetails;
    public  Value5: FieldDetails;
    public  Value6: FieldDetails;
    public  Value7: FieldDetails;
    public Value8: FieldDetails;
    public   Value9: FieldDetails;
    public  Value10: FieldDetails;

}

export class FieldDetails {
   public Column: string;
   public  Row: string;
}
//export class GroupItem {

//    FilterItems: DWObjectFieldsDetails[] = [];
//    GroupItems: GroupItem[] = [];

//    private andOr: string;
//    public get AndOr() {
//        if (AppTool.IsNullOrEmpty(this.andOr)) {
//            return "And";
//        }
//        return this.andOr;
//    }
//    public set AndOr(newValue: string) {
//        this.andOr = newValue; 
//    }

//}

