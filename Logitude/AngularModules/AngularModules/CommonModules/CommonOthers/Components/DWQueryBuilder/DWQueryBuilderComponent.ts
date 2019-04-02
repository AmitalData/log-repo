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
import { DWQueryData } from '../../../../Common/DataContracts/DWQueryData';
import { DWSubQueryPMService } from '../../../../Infrastructure/Services/StandardPMs/DWSubQueryPMService';
import { DWSubQueryPM } from '../../../../Infrastructure/EntityPMs/DWSubQueryPM';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { DWObjectTableListService } from '../../../../Infrastructure/Services/StandardLists/DWObjectTableListService';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { DWQueryPMService } from '../../../../Infrastructure/Services/StandardPMs/DWQueryPMService';
import { DWQueryBuilderHelper } from '../../../../Infrastructure/Helpers/DWQueryBuilderHelper';


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
    public _DWQueryPMService: DWQueryPMService;
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

    ObjectTable: any;
    SearchFieldsId: string;
    HasChanges: boolean = false;
    IsBIReportWorkspace: boolean = false;
    IsBIReportEditScreen: boolean = false;
    FolderId: string;
    public SelectedFiltersDataSourceChanged: any;
    public _DWQueryBuilderHelper: DWQueryBuilderHelper;
    /////////
    //public TooltipId: string = null;
    //public TooltipContentId: string = null;
    public IconPath: string = "./Images/Help.png";
    public IconBackground: string = null;
    public Width: number = 256;
    public Height: number = 125;
    public IconSize: number = 17;
    mouseover(MyItem) {
        if (MyItem.HelpText) {
            var item = document.getElementById(MyItem.TooltipId);
            var itemRect = item.getBoundingClientRect();

            var isToRight = true;
            var ApplicationSession = document.getElementById("ApplicationSession");
            if (ApplicationSession) {
                var appWidth = ApplicationSession.clientWidth;
                var appHeight = ApplicationSession.clientHeight;

                if ((itemRect.left + this.Width) > appWidth) {
                    isToRight = false;
                }
            }

            document.getElementById(MyItem.TooltipContentId).style.position = "fixed";
            document.getElementById(MyItem.TooltipContentId).style.top = (itemRect.top - this.Height + 5) + 'px';

            if (isToRight) {
                document.getElementById(MyItem.TooltipContentId).style.backgroundImage = "url('./_Resources/Images/Icons/Tooltips/Tootip.png')";
                document.getElementById(MyItem.TooltipContentId).style.left = (itemRect.left + 5) + 'px';
            }

            else {
                document.getElementById(MyItem.TooltipContentId).style.backgroundImage = "url('./_Resources/Images/Icons/Tooltips/TootipFlipped.png')";
                document.getElementById(MyItem.TooltipContentId).style.left = (itemRect.left - this.Width) + 'px';
            }
        } 
    }
    constructor(private CD: ChangeDetectorRef) {
        super();
        this._DWObjectTablePMService = new DWObjectTablePMService();
        this._DWQueryPMService = new DWQueryPMService();
        this._DWObjectFieldPMService = new DWObjectFieldExtendedPMService();
        this._DWQueryBuilderService = new DWQueryBuilderService();
        this._DWSubQueryPMService = new DWSubQueryPMService();
        this._DWObjectTableListService = new DWObjectTableListService();
        this._DWQueryBuilderHelper = new DWQueryBuilderHelper();
        if (SessionLocator.CurrentSession == null) {
            this.SearchFieldsId = "SearchFields_-1_-1";
        }

        else {
            this.SearchFieldsId = "DWQueryBuilderSearchFields_" + SessionLocator.CurrentSession.GetNewId("DWQueryBuilderSearchFields");
        }
     

        //this._DWQueryBuilderHelper.FillAllFactFields("Fact_Shipments");
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
                    //this.StartFiltersBusyIndicator("Restoring filters ..");
                    this._DWObjectFieldPMService.getDWObjectFieldsWithChildrenByDWTableId(myResult.Result.Code).subscribe(Result => {
                        this.ObsList = [];
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
                            //this.StopFiltersBusyIndicator();
                        }

                    });
                }
            });
        });


    }
    SetWindowArgs(args: any) {
        this.QID = args.DWQueryId;
        this.IsBIReportWorkspace = args.IsBIReportWorkspace;
        this.IsBIReportEditScreen = args.IsBIReportEditScreen;
        this.FolderId = args.FolderId;

        if (this.QID) {
            this._DWSubQueryPMService.getByQueryId(this.QID).subscribe(myResult => {
                if (!myResult.HasError) {
                    this.ID = myResult.Result.SubQueryData.Id;

                    this.EditButtonClicked();

                }
            });
        }
    }

    ClearPlaceHolder() {
        var temp = document.getElementById(this.SearchFieldsId) as HTMLInputElement;
        temp.placeholder = "";
        temp.style.background = "rgba(0, 0, 0, 0)";
        temp.select();
    }

    FillPlaceHolder() {
        if (!this.SearchText) {
            var temp = document.getElementById(this.SearchFieldsId) as HTMLInputElement;
            temp.placeholder = TextCodeTranslator.Translate("General.O.Search");
            temp.style.background = "url(Images/Search.png) no-repeat scroll";
            temp.style.backgroundPosition = "right center";
            temp.style.paddingRight = "30px";
        }
    }

    OnDeleteValue() {
        var temp = document.getElementById(this.SearchFieldsId) as HTMLInputElement;
        temp.value = null;
        this.SearchText = null;
        temp.focus();
    }

    Run() {

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
    btnUp_Click(selectedItem) {

        var item = selectedItem;//this.FieldSelectedItem;
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
            }
            this.SelectedFieldsDataSource = this.ResetIndexes(this.SelectedFieldsDataSource);
        }

    }
    //ShowArrows: boolean = false;
    ShowOrderArrows(item) {
        this.FieldSelectedItem = item;
    }
    //ShowOrderArrows(item) {
    //}
    btnDown_Click(selectedItem) {
        var item = selectedItem;//this.FieldSelectedItem;
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

        this.SelectedItem = item;
        var myCurrentItem = this.SelectedFieldsDataSource.filter(a => a.DisplayName == this.SelectedItem.DisplayName);
        if (this.SelectedItem && myCurrentItem && myCurrentItem.length == 0) {
            if (this.SelectedItem.Code == '[Full Date]') {
                this.SelectedItem.ParentDataTypeCode = "LookUp";
                this.SelectedItem.DataTypeCode = "DateTime";
                this.SelectedItem.HasTree = true;
            }
            if (this.SelectedItem.Name == 'Full Date') {
                this.SelectedItem.HasTree = false;
            }
            var tempData = this.SelectedFieldsDataSource;
            tempData.push(this.SelectedItem);
            this.SelectedFieldsDataSource = this.ResetIndexes(tempData);
            //this.SampleData = [];
            //this.Notes = "";
            //this.SaveChanges();
            this.ClearData();
        }
    }

    ClearData() {
        this.SampleData = [];
        this.Notes = "";
        this.IsPreview = true;
        this.IsDataReturened = true;
    }

    btnRemove_Click(item) {
        this.FieldSelectedItem = item;
        this.FieldSelectedItem = item;
        if (this.FieldSelectedItem) {
            var index = this.SelectedFieldsDataSource.indexOf(this.FieldSelectedItem);

            if (index !== -1) {
                this.SelectedFieldsDataSource.splice(index, 1);
            }
            this.SelectedFieldsDataSource = this.ResetIndexes(this.SelectedFieldsDataSource);
            //this.SampleData = [];
            //this.SaveChanges();
            this.ClearData();

        }
    }
    RootGroups: DWObjectFieldsDetails[] = [];
    btnAddFilter_Click(item: DWObjectFieldsDetails) {
        if (item.CannotFilter == true) {
            this.messageWindow.Width = 300;
            this.messageWindow.Height = 150;
            this.messageWindow.Title = "Not available for filtering";
            this.messageWindow.Message = "This field is not available for filtering, you can use the code";
            this.messageWindow.Show(this.messageWindow.Message);
            return;
        }
        var view = new DWObjectFieldsDetails(item.BaseDWObjectField, this);
        if (view.DWObjectTableCode.indexOf("DIM_") != -1) {
            //view.ParentDataTypeCode = "LookUp";
            if (view.Code == '[Full Date]') {
                view.ParentDataTypeCode = "DateTime";
                view.DataTypeCode = "DateTime";
                view.HasTree = true;
            }
            else {
                view.ParentDataTypeCode = "LookUp";
            }
            if (view.Name == 'Full Date') {
                view.HasTree = false;
            }
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
        view.DisplayName = item.DisplayName;
        view.DimensionTableDisplayName = item.DimensionTableDisplayName;
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

                this.SelectedFiltersDataSource = tempDataNew;
            }

            this.RunDetectChanges();

            if (this.SelectedItem.DataTypeCode == "Boolean") {
                //this.SaveChanges();
                this.ClearData();
            }
        }
    }

    btnRemoveFilter_Click() {
        if (this.FilterSelectedItem) {
            var index = this.SelectedFiltersDataSource.indexOf(this.FilterSelectedItem);

            if (index !== -1) {
                this.SelectedFiltersDataSource.splice(index, 1);
            }
            //this.SaveChanges();
            this.ClearData();

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
            var values: string[] = textValue.toString().split(';');
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
    public DWQueryData: DWQueryData;
    SaveChanges(StopPreview: boolean = false) {
        this.DWQueryData = new DWQueryData();
        this.DWQueryData.Filters = this.SelectedFiltersDataSource[0];
        this.DWQueryData.Columns = this.SelectedFieldsDataSource;
        this.DWQueryData.PageIndex = 0;
        this.DWQueryData.PageSize = 100;

        //if (this.DWQueryData.Filters) {
        if (this.SelectedFiltersDataSource.length > 0 && this.ValidFiltersValues(this.SelectedFiltersDataSource[0]) != true) {
            this.messageWindow.Width = 300;
            this.messageWindow.Height = 150;
            this.messageWindow.Title = "Invalid Filters";
            this.messageWindow.Message = "There is an invalid input in one of the filters";
            this.messageWindow.Show(this.messageWindow.Message);
            return;
        }
        if (this.SelectedFieldsDataSource.length > 0) {


            this.StartBusyIndicator("Loading ..");
            this.IsPreview = !StopPreview;
            this._DWQueryBuilderService.GetNewDWQueryData(this.DWQueryData).subscribe(myResult => {
                if (!myResult.HasError) {
                    //this.SampleData = myResult.Result.SQLDataResult;
                    this.StopBusyIndicator();
                    if (StopPreview == true) {
                        this.SampleData = [];
                        var MySql = myResult.Result.SQLString.split("ORDER BY")[0];
                        this.Notes = MySql;//myResult.Result.SQLString;

                    }
                    else {
                        this.PreviewData(StopPreview, myResult.Result.SQLDataResult);
                    }

                }
            });

        }
        //else {
        //    this.Notes = SelectStmt + (HasMeasurement && GroupByStmt != " group by" ? GroupByStmt : "");
        //    if (GroupByStmt != " group by" || this.Notes.indexOf(" group by") == -1) {
        //        this.PreviewData(StopPreview);
        //    }
        //}

        //} 
        //this.InnerTables = [];
        //this.SampleData = [];
        //if (this.SelectedFieldsDataSource.length == 0) {
        //    return;
        //}
        //this.IsPreview = false;
        //var SelectStmt = "Select ";
        //var GroupByStmt = " group by ";
        ////var Wheremt = " Where ";
        //this.WhereStmt = " where ";
        //var HasMeasurement: boolean = false;
        //var FromTables = [];
        //this.SelectedFieldsDataSource.forEach((field) => {
        //    if (field.IsMeasurement) {
        //        HasMeasurement = true;
        //        SelectStmt += field.AggregationTypeCode + "(" + field.DWObjectTableCode + "." + field.Code + ")" + (field.DisplayName ? " as " + field.DisplayName + "," : ",");
        //    }
        //    else {
        //        SelectStmt += field.DWObjectTableCode + "." + field.Code + (field.DisplayName ? " as " + field.DisplayName + "," : ",");
        //        GroupByStmt += field.DWObjectTableCode + "." + field.Code + ",";
        //    }

        //    if (FromTables.filter(a => a == field.DWObjectTableCode).length == 0) {
        //        FromTables.push(field.DWObjectTableCode);
        //    }
        //});

        //SelectStmt = SelectStmt.substring(0, SelectStmt.length - 1);
        //GroupByStmt = GroupByStmt.substring(0, GroupByStmt.length - 1);
        //var Fact = FromTables.filter(a => a.indexOf("Fact") != -1)[0];
        //if (AppTool.IsNullOrEmpty(Fact)) {
        //    Fact = "Fact_Shipments";
        //}
        //SelectStmt += " from " + Fact;


        //this.SelectedFieldsDataSource.forEach((field) => {
        //    if (field.ParentDimTabelName != null && this.InnerTables.filter(a => a.ParentDimTabelName == field.ParentDimTabelName).length == 0) {
        //        this.InnerTables.push(field);
        //    }
        //});



        //if (this.SelectedFiltersDataSource.length > 0) {
        //    this.GetWhereJoined(this.SelectedFiltersDataSource);
        //    this.GetWhereStmtForFiltersList(this.SelectedFiltersDataSource, this.SelectedFiltersDataSource[0].AndOr);
        //}


        //FromTables = FromTables.filter(a => a != Fact);

        //this.InnerTables.forEach((mytbl) => {
        //    var Key = this.AllFieldsObsList.filter(a => a.DWObjectTableCode == mytbl.ParentDimTabelName && a.IsPrimaryKey == true)[0];
        //    var FactKey = this.AllFieldsDataSource.filter(a => a.DimensionTableCode == mytbl.ParentDimTabelName)[0];
        //    SelectStmt += " inner join " + mytbl.ParentDimTabelName + " on " + Fact + "." + ((FactKey.DataTypeCode.toLowerCase() == "lookup" || FactKey.DataTypeCode.toLowerCase() == "dimension") ? FactKey.DisplayName : FactKey.Code) + " = " + mytbl.ParentDimTabelName + "." + Key.Code



        //});

        //if (this.SelectedFiltersDataSource.length > 0) {
        //    this.Notes = SelectStmt + this.WhereStmt + (HasMeasurement && GroupByStmt != " group by" ? GroupByStmt : "");
        //    if (GroupByStmt != " group by" || this.Notes.indexOf(" group by") == -1) {
        //        this.PreviewData(StopPreview);
        //    }
        //}
        //else {
        //    this.Notes = SelectStmt + (HasMeasurement && GroupByStmt != " group by" ? GroupByStmt : "");
        //    if (GroupByStmt != " group by" || this.Notes.indexOf(" group by") == -1) {
        //        this.PreviewData(StopPreview);
        //    }
        //}


    }

    ValidFiltersValues(MyFilter: DWObjectFieldsDetails) {
        if (MyFilter.FilterItems.length == 0) {
            return true;
        }
        var Valid = true;
        MyFilter.FilterItems.forEach((field) => {

            if (field.FilterItems.length == 0) {
                if (field.DataTypeCode && field.TextValue) {
                    switch (field.DataTypeCode.toLowerCase()) {
                        case 'integer':
                        case 'double':
                        case 'decimal':
                            {
                                var val: number;

                                if ((field.TextValue + "").indexOf(',') == -1) {
                                    val = Number(field.TextValue);
                                }

                                if (isNaN(Number(val))) {
                                    Valid = false;
                                }
                            } 
                        default: { 
                            break;
                        }

                    }

                }

            }
            else {
                this.ValidFiltersValues(field);
            }

        });

        return Valid;


    }

    SampleData: any[] = [];
    CancelButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindowEmit("cancel");
    }
    IsPreview: boolean = true;
    IsDataReturened: boolean = true;
    PreviewData(StopPreview: boolean = false, Data: any[]) {
        //if (this.Notes) {
        if (StopPreview == true) {
            this.SampleData = [];
            this.IsPreview = false;
            return;
        }
        if (Data.length > 0) {
            this.IsDataReturened = true;
        }
        else {
            this.IsDataReturened = false;
        }
        this.IsPreview = true;
        this.SampleData = Data;
        //var tempSQL = this.Notes.replace("Select ", "Select top 100 ").trim();
        //this._DWQueryBuilderService.GetDWQueryData(tempSQL, "Fact_Shipments").subscribe(myResult => {
        //    if (!myResult.HasError) {
        //        this.SampleData = myResult.Result;

        //    }

        //});
        //}

    }
    ShowSQL() {
        this.SaveChanges(true);
    }

    AddFilterToGroup() {
        var DWObjectField = new DWObjectFieldsDetails(null, this);
        DWObjectField.IndexOrder = this.SelectedFiltersDataSource.length;

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

    }

    public BusyIndicatorText: string = null;
    public ShowBusyIndicator: boolean = false;
    public StartBusyIndicator(myText: string) {
        this.BusyIndicatorText = myText;
        this.ShowBusyIndicator = true;
        this.RunDetectChanges();
    }

    public StopBusyIndicator() {
        this.BusyIndicatorText = null;
        this.ShowBusyIndicator = false;
    }

    public FiltersBusyIndicatorText: string = null;
    public FiltersShowBusyIndicator: boolean = false;
    public StartFiltersBusyIndicator(myText: string) {
        this.FiltersBusyIndicatorText = myText;
        this.FiltersShowBusyIndicator = true;
        this.RunDetectChanges();
    }

    public StopFiltersBusyIndicator() {
        this.FiltersBusyIndicatorText = null;
        this.FiltersShowBusyIndicator = false;
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
    public QueryData: DWQueryData;
    SaveButtonClicked() {

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
                this.QueryData = new DWQueryData();
                this.QueryData.SubQueryData = MySubQuery;
                this.QueryData.Columns = this.SelectedFieldsDataSource;
                this.QueryData.Filters = this.SelectedFiltersDataSource[0];
                if (AppTool.IsNullOrEmpty(this.ID)) {
                    this._DWSubQueryPMService.insertDWQueryData(this.QueryData).subscribe(myResult => {
                        //if (!myResult.HasError) {

                        //}
                        this.ID = myResult.Result.Id;
                        this.QID = myResult.Result.DWQueryId
                        this.EditButtonClicked();

                        SessionLocator.CurrentSession.CurrentWindow.StopBusyIndicator();
                        if (this.IsBIReportWorkspace) {
                            SessionLocator.CurrentSession.CloseCurrentWindowEmit("ok");
                        }
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
                    this._DWSubQueryPMService.UpdateDWQueryData(this.QueryData).subscribe(myResult => {

                        SessionLocator.CurrentSession.CurrentWindow.StopBusyIndicator();
                        if (this.IsBIReportWorkspace || this.IsBIReportEditScreen) {
                            SessionLocator.CurrentSession.CloseCurrentWindowEmit("ok");
                        }
                    });
                }

            }
        });
    }
    NotExist: boolean = true;
    EditButtonClicked() {
        this.SelectedFieldsDataSource = [];
        this.SelectedFiltersDataSource = [];
        SessionLocator.CurrentSession.CurrentWindow.StartBusyIndicator("Loading ..");
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
                        view.DimensionTableDisplayName = field.DimensionTableDisplayName;

                        view.ParentCode = field.ParentCode;
                        view.ParentDimTabelName = field.ParentDimTabelName;

                        tempColumns.push(view);

                    });

                    this.SelectedFieldsDataSource = this.ResetIndexes(tempColumns);

                    //////////////////////////////////////////

                    ////////////////////////////////////////////////
                    if (QueryData.Filters) {
                        var DWObjectField = new DWObjectFieldsDetails(null, this);
                        DWObjectField.IsGroup = true;
                        DWObjectField.setAndOrOperation(QueryData.Filters.AndOr, false);
                        DWObjectField.IndexOrder = this.SelectedFiltersDataSource.length;
                        var MyFilter = this.RestoreFilters(QueryData.Filters, DWObjectField);
                        var temp = [];
                        temp.push(MyFilter);
                        this.SelectedFiltersDataSource = temp;
                    }
                    else {
                        this.SelectedFiltersDataSource = [];
                    }
                    if (SessionLocator.CurrentSession.CurrentWindow) {
                        SessionLocator.CurrentSession.CurrentWindow.StopBusyIndicator();
                    }
                    this.PreviewData(true, []);
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

                    if (view.Code == '[Full Date]') {
                        view.ParentDataTypeCode = "DateTime";
                        view.DataTypeCode = "DateTime";
                    }
                    else {
                        view.ParentDataTypeCode = "LookUp";
                    }
                    view.ParentDimTabelName = field.DWObjectTableCode;
                }
                else {
                    view.ParentDataTypeCode = field.DataTypeCode;
                }
            }

            if (field.FilterItems.length == 0) {
                //view.TextValue = field.TextValue;
                view.setTextValue(field.TextValue, false);
                view.MultiSelectedValueLists = this.MapMultiSelectedValueLists(field.MultiSelectedValueLists);
                view.Operation = new ObjectFieldOperator(field.OperationCode, field.OperationName);
                MyFilter.FilterItems.push(view);
            }
            else {
                var DWObjectField = new DWObjectFieldsDetails(null, this);
                DWObjectField.IsGroup = true;
                DWObjectField.IndexOrder = MyFilter.FilterItems.length;
                //DWObjectField.AndOr = field.AndOr;
                DWObjectField.setAndOrOperation(field.AndOr, false);
                this.RestoreFilters(field, DWObjectField);
                MyFilter.FilterItems.push(DWObjectField);

            }


        });

        return MyFilter;


    }


    MapMultiSelectedValueLists(lists: MultiSelectedValue[]) {
        var result: MultiSelectedValue[] = [];
        if (lists) {
            lists.forEach((field) => {
                var item: MultiSelectedValue = new MultiSelectedValue();
                var i = "";
                var j = 0;
                while (field["Value" + i]) {
                    var valueDetails: ValueDetails = new ValueDetails();
                    valueDetails.Header = field["Value" + i].Header;
                    valueDetails.Row = field["Value" + i].Row;
                    item["Value" + i] = valueDetails;
                    j += 1;
                    i = j.toString();

                }
                result.push(item);

            });

            return result;
        }
    }

    RunDetectChanges() {
        if (this.CD) {
            var isDestroyed: boolean = this.CD['destroyed'];
            if (!isDestroyed) {
                this.CD.detectChanges();
            }
        }
    }
}

export class DWObjectFieldsDetails extends BaseComponent {
    public MyParentClass: DWQueryBuilderComponent;
    public BaseDWObjectField: any;
    public TooltipId: string = null;
    public TooltipContentId: string = null;
    constructor(DWObjectField: any = null, ParentClass: DWQueryBuilderComponent = null) {
        super();
        var idIndex = SessionLocator.CurrentSession.GetNewId("Tooltip");
        this.TooltipId = "Tooltip_" + idIndex;
        this.TooltipContentId = "TooltipContent_" + idIndex;
        this.BaseDWObjectField = DWObjectField;
        if (ParentClass != null) {
            this.MyParentClass = ParentClass;
            this.IndexOrder = ParentClass.SelectedFieldsDataSource.length;
        }
        if (DWObjectField != null) {
            this.HideTree = DWObjectField.HideTree;
            this.LOVAdditionalColumns = DWObjectField.LOVAdditionalColumns;
            if (!this.LOVAdditionalColumns && ParentClass && ParentClass.AllFieldsDataSource) {
                var field = ParentClass.AllFieldsDataSource.filter(a => a.DWObjectTableCode == DWObjectField.DWObjectTableCode && a.Code == DWObjectField.Code && a.Name == DWObjectField.Name)[0];
                if (field) {
                    this.LOVAdditionalColumns = DWObjectField.LOVAdditionalColumns = field.LOVAdditionalColumns;
                }

            }

            if (!AppTool.IsNullOrEmpty(DWObjectField.DimensionTableDisplayName)) {
                this.DimensionTableDisplayName = DWObjectField.DimensionTableDisplayName;
            }
            else {
                this.DimensionTableDisplayName = DWObjectField.ParentCode;
            }

            this.Name = DWObjectField.Name;
            this.Code = DWObjectField.Code;
            this.DWObjectTableCode = DWObjectField.DWObjectTableCode;
            this.DimensionTableCode = DWObjectField.DimensionTableCode;
            this.DataTypeCode = DWObjectField.DataTypeCode;
            //if (DWObjectField.FilterItems && DWObjectField.FilterItems.length == 0) {
            this.DisplayName = this.ComputeDisplayName(DWObjectField);
            //}
            this.CannotFilter = DWObjectField.CannotFilter;
            this.HelpText = DWObjectField.HelpText;
            this.IsPrimaryKey = DWObjectField.IsPrimaryKey;
            this.IsMeasurement = DWObjectField.IsMeasurement;
            this.AggregationTypeCode = DWObjectField.AggregationTypeCode;

            if (DWObjectField.FilterType) {
                this.FilterType = DWObjectField.FilterType;
            }
            if (DWObjectField.IsSetDefaults) {
                this.IsSetDefaults = DWObjectField.IsSetDefaults;
            }
            if (DWObjectField.IsMandatoryFilter) {
                this.IsMandatoryFilter = DWObjectField.IsMandatoryFilter;
            }


            //this.Name = DWObjectField.Name;
        }

    }

   

    public ComputeDisplayName(DWObjectField: any) {
        //(AppTool.IsNullOrEmpty(DWObjectField.DisplayName)) ? (DWObjectField.DWObjectTableCode + ' ' + DWObjectField.Code) : (DWObjectField.DisplayName);
        var Displayname = DWObjectField.DisplayName;
        if (AppTool.IsNullOrEmpty(DWObjectField.DisplayName)) {
            if (DWObjectField.DWObjectTableCode && DWObjectField.DWObjectTableCode.indexOf("DIM_") != -1) {
                Displayname = DWObjectField.ParentCode + ' ' + DWObjectField.Name;
            }
            else {
                Displayname = DWObjectField.Name;
            }
        }
        return Displayname;
    }



    private hideTree: boolean;
    public get HideTree() { return this.hideTree; }
    public set HideTree(newValue: boolean) { this.hideTree = newValue; }

    private cannotFilter: boolean = false;
    public get CannotFilter() { return this.cannotFilter; }
    public set CannotFilter(newValue: boolean) { this.cannotFilter = newValue; }

    private helpText: string;
    public get HelpText() { return this.helpText; }
    public set HelpText(newValue: string) { this.helpText = newValue; }

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

    private displayname: string;
    public get DisplayName() { return this.displayname; }
    public set DisplayName(newValue: string) { this.displayname = newValue; }

    private dimensionTableDisplayName: string;
    public get DimensionTableDisplayName() { return this.dimensionTableDisplayName; }
    public set DimensionTableDisplayName(newValue: string) { this.dimensionTableDisplayName = newValue; }

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
            this.HasTree = !this.HideTree ? true : false;
            var MyTable = this.MyParentClass.AllTables.filter(a => a.Code == this.DimensionTableCode);
            if (MyTable && MyTable.length > 0) {
                this.Code = MyTable[0].DefaultFilterBy;
                this.DWObjectTableCode = MyTable[0].Code;
                this.DisplayName = this.ComputeDisplayName(this);//(AppTool.IsNullOrEmpty(this.DisplayName)) ? (this.DWObjectTableCode + ' ' + this.Code) : (this.DisplayName);
                this.ParentDimTabelName = this.DimensionTableCode;
                if (!AppTool.IsNullOrEmpty(this.DimensionTableDisplayName)) {
                    this.DimensionTableDisplayName = this.DimensionTableDisplayName;
                }
                else {
                    this.DimensionTableDisplayName = this.ParentCode;
                }
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
        if (this.textValue != newValue) {
            this.textValue = newValue;
            this.MyParentClass.ClearData();
            //if ((newValue == true || newValue == false) && this.textValue) {
            //    this.textValue = newValue;

            //    //if (!this.DontSaveChanges) {
            //    //    //this.MyParentClass.SaveChanges();
            //    //    this.MyParentClass.ClearData();
            //    //}


            //}
            //else if (newValue != true || newValue != false) {
            //    this.textValue = newValue;
            //    if (!this.DontSaveChanges) {
            //        //this.MyParentClass.SaveChanges();
            //        this.MyParentClass.ClearData();
            //    }
            //}

            this.DontSaveChanges = false;
        }
    }

    public setTextValue(Newvalue: any, ClearData: boolean = true) {
        this.textValue = Newvalue;
        if (ClearData == true) {
            this.MyParentClass.ClearData();
        }
    }

    public setAndOrOperation(Newvalue: string, ClearData: boolean = true) {
        this.AndOr = Newvalue;
        if (ClearData == true) {
            this.MyParentClass.ClearData();
        }
    }





    private multiSelectedValueLists: MultiSelectedValue[];
    public get MultiSelectedValueLists() {
        return this.multiSelectedValueLists;
    }
    public set MultiSelectedValueLists(newValue: MultiSelectedValue[]) {
        this.multiSelectedValueLists = newValue;

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
            else if ((this.ParentDataTypeCode == "Date" || this.ParentDataTypeCode == "DateTime") && AppTool.IsNullOrEmpty(this.operation)) {
                this.operation = new ObjectFieldOperator("Before", "Before");
                this.OperationCode = "Before";
                this.OperationName = "Before";
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
        //this.MyParentClass.SaveChanges();
        //this.MyParentClass.ClearData();
    }

    private filterType: string = "Fixed Filter";
    public get FilterType() { return this.filterType; }
    public set FilterType(newValue: string) { this.filterType = newValue; }

    private isSetDefaults: boolean = false;
    public get IsSetDefaults() { return this.isSetDefaults; }
    public set IsSetDefaults(newValue: boolean) {
        if (this.isSetDefaults != newValue) {
            this.isSetDefaults = newValue;
            if (newValue == false) {
                this.TextValue = null;
            }
        }
    }

    private isMandatoryFilter: boolean = false;
    public get IsMandatoryFilter() { return this.isMandatoryFilter; }
    public set IsMandatoryFilter(newValue: boolean) { if (this.isMandatoryFilter != newValue) { this.isMandatoryFilter = newValue; } }


    FilterTypeChanged(Value) {
        this.FilterType = Value;
    }

    OpenFilterSettings() {
        var windowArgs: any = {};
        windowArgs.IsMandatoryFilter = this.IsMandatoryFilter;
        windowArgs.IsSetDefaults = this.IsSetDefaults;



        var logWindow = new LogitudeWindow();
        logWindow.WindowArgs = windowArgs;
        logWindow.Width = 500;
        logWindow.Height = 260;
        logWindow.Title = "Ask User Settings";
        logWindow.Show('./CommonModules/CommonOthers/Components/DWQueryBuilder/DWFilterSettings');
        logWindow.WindowClosed.subscribe(($event: string) => {
            if ($event) {
                var MySettings = $event.split(',');
                if (MySettings[0] == "true") {
                    this.IsMandatoryFilter = true;
                }
                else {
                    this.IsMandatoryFilter = false;
                }
                if (MySettings[1] == "true") {
                    this.IsSetDefaults = true;
                }
                else {
                    this.IsSetDefaults = false;
                }
            }
        });
    }


    DontSaveChanges: boolean = false;
    OperationValueChanged(operation) {



        if (operation.Code == this.currentOp.Code || operation.Code == this.beforeOp.Code || operation.Code == this.afterOp.Code || operation.Code == this.previousOp.Code || operation.Code == this.nextOp.Code || operation.Code == this.currentOp.Code) {
            this.DontSaveChanges = true;
            //this.TextValue = "";
            this.Operation = operation;
        } else {

            if (this.Operation.Code == this.IsNullOp.Code || this.Operation.Code == this.IsNotNullOp.Code) {
                this.TextValue = "";
                this.MultiSelectedValueLists = [];
            }

            this.Operation = operation;
            if (operation.Code == this.IsNullOp.Code || operation.Code == this.IsNotNullOp.Code) {
                this.TextValue = operation.Code;
            }
            if (operation.Code == this.IsNullOp.Code || operation.Code == this.IsNotNullOp.Code || !AppTool.IsNullOrEmpty(this.TextValue)) {
                //this.MyParentClass.SaveChanges();
                this.MyParentClass.ClearData();
            }
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
                                if (field.Code == '[Full Date]') {
                                    view.ParentDataTypeCode = field.DataTypeCode;
                                    view.DataTypeCode = "DateTime";
                                }
                                else {
                                    view.ParentDataTypeCode = DWObjectField.DataTypeCode;
                                }

                                if (!AppTool.IsNullOrEmpty(DWObjectField.Code)) {
                                    view.DisplayName = '[' + (DWObjectField.Name.replace('[', '').replace(']', '') + ' ' + view.Name.replace('[', '').replace(']', '')) + ']';//.replace('[', '').replace('[', '').replace(']', '').replace(']', '');
                                    view.DimensionTableDisplayName = DWObjectField.Name.replace('[', '').replace(']', '');
                                }
                                else if (!AppTool.IsNullOrEmpty(DWObjectField.DisplayName)) {
                                    view.DisplayName = DWObjectField.Name;
                                    view.DimensionTableDisplayName = DWObjectField.Name;
                                }
                                else {
                                    view.DisplayName = '[' + (DWObjectField.Name + ' ' + view.Name.replace('[', '').replace(']', '')) + ']';//.replace('[', '').replace('[', '').replace(']', '').replace(']', '');
                                    view.DimensionTableDisplayName = DWObjectField.Name;

                                }
                                view.ParentCode = DWObjectField.Code;
                                view.ParentDimTabelName = DWObjectField.DimensionTableCode;

                                ObsList.push(view);
                            }

                        });
                        this.Items = ObsList;
                        this.IsViewTree = true;
                    }

                });
            }

        });
    }
    @Output() ShowSampleDateCommand = new EventEmitter();

    onTextChange(value) {
        this.TextValue = value;
        this.ShowSampleDateCommand.emit(this);
    }

    AndOrOpsChanged(value) {
        this.AndOr = value;
    }

    OnMouseOver(event) {
        var e = event.toElement;
        if (e && e.className == "LinkBtn") {
            return;
        }
        if (this.ShowBtns == false) {
            this.ShowBtns = true;
        }
    }

    OnMouseOut(event) {

        var e = event.toElement;
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

        this.TextValue = "";
        this.MultiSelectedValueLists = [];
        this.Name = DWObjectField.Name;
        this.Code = DWObjectField.Code;
        this.DWObjectTableCode = DWObjectField.DWObjectTableCode;
        this.DataTypeCode = DWObjectField.DataTypeCode;
        this.DimensionTableCode = DWObjectField.DimensionTableCode;
        this.DisplayName = this.ComputeDisplayName(DWObjectField);//(AppTool.IsNullOrEmpty(DWObjectField.DisplayName)) ? (DWObjectField.DWObjectTableCode + ' ' + DWObjectField.Code) : (DWObjectField.DisplayName);

        if (!AppTool.IsNullOrEmpty(DWObjectField.DimensionTableDisplayName)) {
            this.DimensionTableDisplayName = DWObjectField.DimensionTableDisplayName;
        }
        else {
            this.DimensionTableDisplayName = DWObjectField.ParentCode;
        }

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


        this.Operators = this.GetFieldOperators(this);

        if ((this.ParentDataTypeCode == "Text" || this.ParentDataTypeCode == "nText")) {
            this.Operation = new ObjectFieldOperator("StartsWith", "Starts With");
        }
        else if ((this.ParentDataTypeCode == "Date" || this.ParentDataTypeCode == "DateTime")) {
            this.Operation = new ObjectFieldOperator("Before", "Before");
        }
        else {
            this.Operation = new ObjectFieldOperator("Equals", "Equals to");
        }

    }

    onDeleteFilterClick() {
        this.MyParentClass.DeleteField(this, this.MyParentClass.SelectedFiltersDataSource);

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
        ) {
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

        if (field.ParentDataTypeCode == "DateTime" || field.ParentDataTypeCode == "Date") {
            this.list.push(this.beforeOp);
            this.list.push(this.afterOp);
            this.list.push(this.previousOp);
            this.list.push(this.currentOp);
            this.list.push(this.nextOp);

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


    beforeOp: ObjectFieldOperator = new ObjectFieldOperator("Before", "Before");
    afterOp: ObjectFieldOperator = new ObjectFieldOperator("After", "After");
    previousOp: ObjectFieldOperator = new ObjectFieldOperator("Previous", "Previous");
    currentOp: ObjectFieldOperator = new ObjectFieldOperator("Current", "Current");
    nextOp: ObjectFieldOperator = new ObjectFieldOperator("Next", "Next");
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


export class MultiSelectedValue {

    public Value: ValueDetails;
    public Value1: ValueDetails;
    public Value2: ValueDetails;
    public Value3: ValueDetails;
    public Value4: ValueDetails;
    public Value5: ValueDetails;
    public Value6: ValueDetails;
    public Value7: ValueDetails;
    public Value8: ValueDetails;
    public Value9: ValueDetails;
    public Value10: ValueDetails;

}

export class ValueDetails {
    public Header: string;
    public Row: string;
}


