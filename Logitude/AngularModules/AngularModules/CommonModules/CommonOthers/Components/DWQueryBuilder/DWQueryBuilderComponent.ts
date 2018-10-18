declare var window: any;

import {Component, ViewContainerRef, OnInit, AfterViewInit, ViewChildren, QueryList, Output, EventEmitter, ChangeDetectorRef} from '@angular/core'; 
import {DWObjectFieldPM} from '../../../../Infrastructure/EntityPMs/DWObjectFieldPM';
import {DWObjectTablePM} from '../../../../Infrastructure/EntityPMs/DWObjectTablePM'; 
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {DWObjectTablePMService} from '../../../../Infrastructure/Services/StandardPMs/DWObjectTablePMService';
import {DWObjectFieldExtendedPMService} from '../../../../Infrastructure/Services/ExtendedPMs/DWObjectFieldExtendedPMService';
import {DWQueryBuilderService} from '../../../../Infrastructure/Services/ExtendedPMs/DWQueryBuilderService';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';  

@Component({
    moduleId: module.id,

    selector: 'DWQueryBuilder',
    templateUrl: './DWQueryBuilderComponent.html', 
})

export class DWQueryBuilderComponent extends BaseComponent{
    RTL: boolean = false;
    public BooleanValues = ["True", "False", "No Filter"];
    public AndOrOps = ["And", "Or"];
    public _DWObjectTablePMService: DWObjectTablePMService;
    public _DWObjectFieldPMService: DWObjectFieldExtendedPMService;
    public _DWQueryBuilderService: DWQueryBuilderService;
    @Output() onSelectedDataLoadedEvent = new EventEmitter();
    @Output() onUnSelectedDataLoadedEvent = new EventEmitter();
    @Output() onDataSourceChangedEvent = new EventEmitter();
    @Output() onUnselectedDataSourceChangedEvent = new EventEmitter();
    DataSource: DWObjectFieldsDetails[];
    SelectedFieldsDataSource: DWObjectFieldsDetails[] = [];
    SelectedFiltersDataSource: DWObjectFieldsDetails[] = [];
    public ObsList: any[] = [];
    public ObsListAll: any[] = [];
    DataContext: any = this;
    /////////////////////////////////////////////////////////
    QueryId: string;
    isNewQueryMode: boolean;
    CurrentObjectTable: string;
    IsEnabled: boolean;
    queryColumnsList: any[];
    unselectedObjectFields: any[];
    staticColumnsList: any[];
    addedQueryColumnList: any[];
    removedQueryColumnList: any[];
    unSelectedList: any[] ;
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
        this._DWQueryBuilderService = new  DWQueryBuilderService();
        this._DWObjectTablePMService.get("Fact_Shipments").subscribe(myResult => {
            if (!myResult.HasError) {
                this._DWObjectFieldPMService.getDWObjectFieldsByDWTableId(myResult.Result.Code).subscribe(Result => {
                    if (!Result.HasError) {
                        Result.Result.forEach((field) => {
                            var view = new DWObjectFieldsDetails(field,this);
                            view.ParentDataTypeCode = field.DataTypeCode;
                            this.ObsList.push(view);
                            this.ObsListAll.push(view);
                        });
                        this.DataSource = this.ObsList;
                    }

                });
            }

        });
    }
    SetWindowArgs(args: any) {
        //this.QueryId = args.queryId;
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
        //if (newValue != null && newValue != "") {
        //    this.unSelectedList = this.Fixedunselected.filter(f => TextCodeTranslator.Translate(f.FullNameTextCodeCode).toLowerCase().indexOf(newValue.toLowerCase()) > -1 );
        //}
        //else {
        //    this.unSelectedList = this.Fixedunselected;
        //}
        this.onUnselectedDataSourceChangedEvent.emit(this.unSelectedList);
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

     

    btnUp_Click() {

        //var item = this.SelectedItem;
        //if (item != null) {
        //    this.HasChanges = true;
        //    var i = this.OrderedQueryColumnsList.indexOf(item);

        //    this.ReorderColumnsList();
        //    var upColumn = this.OrderedQueryColumnsList.filter(d => d.ObjectFieldId == item.ObjectFieldId && ((d.Tenant == SessionInfo.LoggedUserTenant && d.UserId == SessionInfo.LoggedUserId) || d.Tenant == 0) && d.QueryId == this.QueryId)[0];

        //    if (i > 0) {
        //        this.OrderedQueryColumnsList = this.OrderedQueryColumnsList.filter(d => d.ObjectFieldId != upColumn.ObjectFieldId);
        //        this.OrderedQueryColumnsList.filter(o => o.IndexOrder == i - 1)[0].IndexOrder = i;
        //        upColumn.IndexOrder = i - 1;
        //        this.OrderedQueryColumnsList.splice(i - 1, 0, upColumn);
        //        this.onDataSourceChangedEvent.emit(this.OrderedQueryColumnsList);
        //        //this.ReorderColumnsList();
        //        this.onSelectedDataLoadedEvent.emit(this.SelectedItem);
        //    }
        //}

    }

    btnDown_Click() {
        //var item = this.SelectedItem;
        //if (item != null) {
        //    this.HasChanges = true;

        //    var i = this.OrderedQueryColumnsList.indexOf(item);

        //    this.ReorderColumnsList();

        //    var downColumn = this.OrderedQueryColumnsList.filter(d => d.ObjectFieldId == item.ObjectFieldId && ((d.Tenant == SessionInfo.LoggedUserTenant && d.UserId == SessionInfo.LoggedUserId) || d.Tenant == 0) && d.QueryId == this.QueryId)[0];

        //    if (i < this.OrderedQueryColumnsList.length - 1) {
        //        this.OrderedQueryColumnsList = this.OrderedQueryColumnsList.filter(d => d.ObjectFieldId != downColumn.ObjectFieldId);

        //        this.OrderedQueryColumnsList.filter(o => o.IndexOrder == i + 1)[0].IndexOrder = i;
        //        downColumn.IndexOrder = i + 1;
        //        this.OrderedQueryColumnsList.splice(i + 1, 0, downColumn);
        //        this.onDataSourceChangedEvent.emit(this.OrderedQueryColumnsList);
        //        //this.ReorderColumnsList();
        //        this.onSelectedDataLoadedEvent.emit(this.SelectedItem);
        //    }
        //}

    }

    btnAdd_Click(item) {
        this.SelectedItem = item;
        if (this.SelectedItem && this.SelectedItem.DataTypeCode != "LookUp" && this.SelectedItem.DataTypeCode != "Dimension" && this.SelectedFieldsDataSource.indexOf(this.SelectedItem) == -1) {
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
            this.SelectedFieldsDataSource = tempData;
            this.SampleData = [];
            this.SaveChanges();
        }
    }

    btnRemove_Click() {
        if (this.FieldSelectedItem) {
            var index = this.SelectedFieldsDataSource.indexOf(this.FieldSelectedItem);

            if (index !== -1) {
                this.SelectedFieldsDataSource.splice(index, 1);
            }
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

    btnAddFilter_Click(item) {
        this.SelectedItem = item;
        if (this.SelectedItem && this.SelectedItem.DataTypeCode != "LookUp" && this.SelectedItem.DataTypeCode != "Dimension" && this.SelectedFiltersDataSource.indexOf(this.SelectedItem) == -1) {
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
            var tempData = this.SelectedFiltersDataSource;
            tempData.push(this.SelectedItem);
            this.SelectedFiltersDataSource = tempData;
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

    ReorderColumnsList() { 
    }

    SaveChanges() {
       
        this.SampleData = [];
        if (this.SelectedFieldsDataSource.length == 0) { 
            return;
        }
        this.IsPreview = false;
        var SelectStmt = "Select ";
        var GroupByStmt = " group by ";
        var WhereStmt = " Where ";
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
        SelectStmt += " from " + Fact;
        var InnerTblsTpl = this.SelectedFieldsDataSource.filter(a => a.ParentDimTabelName != null);

        var InnerTables = [];
        this.SelectedFieldsDataSource.forEach((field) => {
            if (field.ParentDimTabelName != null && InnerTables.filter(a => a.ParentDimTabelName == field.ParentDimTabelName).length == 0) {
                InnerTables.push(field);
            }
        });

        this.SelectedFiltersDataSource.forEach((filter) => {
            if (filter.ParentDimTabelName != null && InnerTables.filter(a => a.ParentDimTabelName == filter.ParentDimTabelName).length == 0) {
                InnerTables.push(filter);
            }
        });

        //this.Notes = SelectStmt;
        FromTables = FromTables.filter(a => a != Fact);
        WhereStmt = " where "
        this.SelectedFiltersDataSource.filter(a => a.TextValue != null).forEach((filter) => {
            var OperationSimpol = "";
            if (filter.Operation.Code == filter.equalsOp.Code) { 
                OperationSimpol = " = '@@' ";
            }
            else if (filter.Operation.Code == filter.notEqualsOp.Code) {
                OperationSimpol = " <> '@@' ";
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

            WhereStmt += (filter.ParentDimTabelName ? filter.ParentDimTabelName : filter.DWObjectTableCode) + "." + filter.Code + OperationSimpol.replace("@@", filter.TextValue) + " " + filter.AndOr + " ";//" = " + "'" + filter.TextValue + "' and ";
        }); 
        if (WhereStmt == " where ") {
            WhereStmt = "";
        }
        else {
            WhereStmt = WhereStmt.substring(0, WhereStmt.length - 4);
        }
        InnerTables.forEach((mytbl) => {

            this._DWObjectFieldPMService.getDWObjectFieldsByDWTableId(mytbl.ParentDimTabelName).subscribe(Result => {
                if (!Result.HasError) {
                    var Key = Result.Result.filter(a => a.IsPrimaryKey == true)[0];
                    var FactKey = this.DataSource.filter(a => a.DimensionTableCode == mytbl.ParentDimTabelName)[0];
                    SelectStmt += " inner join " + mytbl.ParentDimTabelName + " on " + Fact + "." + FactKey.Code + " = " + mytbl.ParentDimTabelName + "." + Key.Code
                    //.forEach((field) => {
                    //    var view = new DWObjectFieldsDetails(field);
                    //    this.ObsList.push(view);
                    //    this.ObsListAll.push(view);
                    //});
                    //this.DataSource = this.ObsList;
                }
                if (this.SelectedFiltersDataSource.length > 0) {
                    this.Notes = SelectStmt + WhereStmt + (HasMeasurement ? GroupByStmt : "");
                }
                else {
                    this.Notes = SelectStmt + (HasMeasurement ? GroupByStmt : "");
                }
                
            });
            
        });
        if (InnerTables.length == 0) {
            if (this.SelectedFiltersDataSource.length > 0) {
                this.Notes = SelectStmt + WhereStmt + (HasMeasurement ? GroupByStmt : "");
            }
            else {
                this.Notes = SelectStmt + (HasMeasurement ? GroupByStmt : "");
            } 
        }

        //this.SelectedFiltersDataSource.forEach((filter) => {
        //    WhereStmt += filter.ParentDimTabelName + "." + filter.Code + " = " + filter.TextValue;
        //}); 
        
    }

    SampleData: any[] = [];
    CancelButtonClicked() {
        SessionLocator.CurrentSession.CloseCurrentWindow();   
    }
    IsPreview: boolean = true;
    PreviewData() {
        this.IsPreview = true;
        SessionLocator.CurrentSession.CurrentWindow.StartBusyIndicator("Loading ..");
        var tempSQL = this.Notes.replace("Select ", "Select top 100 ").trim();
        this._DWQueryBuilderService.GetDWQueryData(tempSQL,"Fact_Shipments").subscribe(myResult => {
            if (!myResult.HasError) {
                this.SampleData = myResult.Result;
                SessionLocator.CurrentSession.CurrentWindow.StopBusyIndicator();
            }

        });
    }

}

export class DWObjectFieldsDetails extends BaseComponent{
    MyParentClass: DWQueryBuilderComponent;
    constructor(DWObjectField: any, ParentClass: DWQueryBuilderComponent) {
        super();
        this.MyParentClass = ParentClass;
        this.Name = DWObjectField.Name;
        this.Code = DWObjectField.Code;
        this.DWObjectTableCode = DWObjectField.DWObjectTableCode;
        this.DataTypeCode = DWObjectField.DataTypeCode;
        this.DimensionTableCode = DWObjectField.DimensionTableCode;
        this.DisplayName = DWObjectField.Code;
        this.IsPrimaryKey = DWObjectField.IsPrimaryKey;
        this.IsMeasurement = DWObjectField.IsMeasurement;
        this.AggregationTypeCode = DWObjectField.AggregationTypeCode;
        //this.Name = DWObjectField.Name;
    }

    Items: any[] = [];

    
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

    private operation: ObjectFieldOperator;
    public get Operation() {
        if (!this.operation) {
            if ((this.ParentDataTypeCode == "Text" || this.ParentDataTypeCode == "nText") && AppTool.IsNullOrEmpty(this.operation)) {
                this.operation = new ObjectFieldOperator("StartsWith", "Starts With");
                return this.operation;
            }
            else {
                if (AppTool.IsNullOrEmpty(this.operation)) {
                    this.operation = new ObjectFieldOperator("Equals", "Equals to");
                }
                return this.operation;
            } 
        }
        else {
            return this.operation;
        }
    }
    public set Operation(newValue: ObjectFieldOperator) {
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
        this.Operation = operation;
        if (operation.Code == this.IsNullOp.Code || operation.Code == this.IsNotNullOp.Code) {
            this.TextValue = operation.Code;
        } 
        if (operation.Code == this.IsNullOp.Code || operation.Code == this.IsNotNullOp.Code || !AppTool.IsNullOrEmpty(this.TextValue)) {
            this.MyParentClass.SaveChanges();
        } 
    }

    LoadItems(DWObjectField:any) {
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
                            var view = new DWObjectFieldsDetails(field, this.MyParentClass);
                            view.ParentDataTypeCode = DWObjectField.DataTypeCode;
                            view.DisplayName = DWObjectField.Code + view.Code;
                            view.ParentCode = DWObjectField.Code;
                            view.ParentDimTabelName = DWObjectField.DimensionTableCode;
                            
                            ObsList.push(view);
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
    IsNullOp: ObjectFieldOperator = new ObjectFieldOperator("IsNull", "Is Null");
    IsNotNullOp: ObjectFieldOperator = new ObjectFieldOperator("IsNotNull", "Is Not Null");

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

