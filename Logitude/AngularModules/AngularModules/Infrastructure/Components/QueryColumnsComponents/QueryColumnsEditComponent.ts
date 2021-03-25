declare var window: any;

import {Component, ViewContainerRef, OnInit, AfterViewInit, ViewChildren, QueryList, Output, EventEmitter, ChangeDetectorRef} from '@angular/core';
import {TextCodeTranslationPipe} from '../../../Controls/Pipes/TextCodeTranslationPipe';
import {LogitudeListBoxComponent} from '../../../Infrastructure/Components/LogitudeComponents/LogitudeListBox/LogitudeListBoxComponent';

//import {ObjectFieldPM} from '../../../Infrastructure/EntityPMs/ObjectFieldPM';
import {QueryColumnPM} from '../../../Infrastructure/EntityPMs/QueryColumnPM';
import {SessionInfo} from '../../../Infrastructure/Utilities/SessionInfo';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {QueryColumnsPMService} from '../../../Infrastructure/Services/StandardPMs/QueryColumnsPMService';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {ServiceHelper} from '../../../Infrastructure/Utilities/ServiceHelper';
import {ObjectsLocator} from '../../Locators/ObjectsLocator';
import { HttpClient } from '@angular/common/http';

@Component({
    

    selector: 'QueryColumnEdit',
    templateUrl: './QueryColumnsEditComponent.html',
    //pipes: [TextCodeTranslationPipe],
    //inputs: ['ObjectTableName', 'event', 'isWindowViewMode', 'isNewViewMode', 'QueryId', 'Filterchangeevent', 'rabaia'],
    providers: [HttpClient, ServiceArgs],
    //directives: [LogitudeListBoxComponent]
})

export class QueryColumnsEditComponent {

    RTL: boolean = ObjectsLocator.GlobalSetting == undefined ? false : (ObjectsLocator.GlobalSetting.LayoutDirection == 'rtl' ? true : false);
    @Output() onSelectedDataLoadedEvent = new EventEmitter();
    @Output() onUnSelectedDataLoadedEvent = new EventEmitter();
    @Output() onDataSourceChangedEvent = new EventEmitter();
    @Output() onUnselectedDataSourceChangedEvent = new EventEmitter();
    DataSource: any[];
    QueryId: string;
    QueryCode: string;
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
    private myQueryColumnsPMService: QueryColumnsPMService;
    private _http: HttpClient;
    public serviceArgs: ServiceArgs;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private CD: ChangeDetectorRef) {
        this.serviceArgs = new ServiceArgs();
        this._http = ServiceHelper.HttpClient;
        this.serviceArgs.http = ServiceHelper.HttpClient;
        if (this.CurrentSession == null) {
            this.SearchFieldsId = "SearchFields_-1_-1";
        }

        else {
            this.SearchFieldsId = "QueryColumnSearchFields_" + this.CurrentSession.GetNewId("QueryColumnSearchFields");
        }
        //this.Run();
    }

    SetWindowArgs(args: any) {
        this.QueryId = args.queryId;
        this.QueryCode = /*this.ObjectTableName + '.' +*/args.queryCode;
        this.isNewQueryMode = args.isNewQueryMode;
        this.CurrentObjectTable = args.currentObjectTable;

        
        this.IsEnabled = false;
        this.ObjectTable = window.ObjectTables.filter(d => d.Name == args.currentObjectTable)[0];
        this.Run();
    }

    ClearPlaceHolder() {
        var temp = document.getElementById(this.SearchFieldsId) as HTMLInputElement;
        temp.placeholder = "";
        temp.style.background = "rgba(0, 0, 0, 0)";
    }

    FillPlaceHolder() {
        var temp = document.getElementById(this.SearchFieldsId) as HTMLInputElement;
        temp.placeholder = TextCodeTranslator.Translate("General.O.Search");
        temp.style.background = "url(Images/Search.png) no-repeat scroll";
        temp.style.backgroundPosition = "right center";
        temp.style.paddingRight = "30px";
    }

    Run() {

        this.HasChanges = false;
        var copy = false;

        var currentQuery = window.Queries.filter(d => d.UniqueCode == this.QueryCode)[0];
        this.addedQueryColumnList = [];
        this.removedQueryColumnList = [];
        //queriesByUser = TenantContext.Current.Queries.Where(d => d.UserId == TenantContext.Current.LoggedContactId).ToList();
        this._http.get(ServiceHelper.GetLogitudeURL() + "api/ngMetaData?tenant=" + SessionInfo.LoggedUserTenant + "&queryCode=" + this.QueryCode + "&objecttableid=" + this.ObjectTable.Id + "&userid=" + SessionInfo.LoggedUserId)
            .subscribe((response: any) => {
                this.queryColumnsList = response;
                // this.queryColumnsList = TenantContext.Current.GeneralContext.QueryColumnPMs.Where(d => d.QueryId == QueryId && ((d.UserId == TenantContext.Current.LoggedContactId && d.Tenant == TenantContext.Current.Id)) && d.DisplayInList).OrderBy(d => d.IndexOrder).ToList();
                this.queryColumnsList = this.queryColumnsList.sort((a, b) => { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1 });

                if (this.queryColumnsList.length == 0) // Copy query columns to my tenant
                {
                    var zeroColumnsList = [];
                    this._http.get(ServiceHelper.GetLogitudeURL() + "api/ngMetaData?tenant=0&queryCode=" + this.QueryCode + "&objecttableid=" + this.ObjectTable.Id + "&userid=null")
                        .subscribe((response: any) => {
                            zeroColumnsList = response;
                            zeroColumnsList = zeroColumnsList.sort((a, b) => { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1 });
                            zeroColumnsList.forEach((querycolumn, key) => {
                                var newcolumn = new QueryColumnPM();

                                    newcolumn.Tenant = SessionInfo.LoggedUserTenant,
                                    newcolumn.UserId = SessionInfo.LoggedUserId,
                                    newcolumn.DisplayInList = querycolumn.DisplayInList,
                                    newcolumn.ObjectFieldName = querycolumn.ObjectFieldName,
                                    newcolumn.ColumnWidth = querycolumn.ColumnWidth,
                                    newcolumn.ConverterName = querycolumn.ConverterName,
                                    newcolumn.DataTemplateName = querycolumn.DataTemplateName,
                                    newcolumn.ColumnHeaderTemplateName = querycolumn.ColumnHeaderTemplateName,
                                    newcolumn.IndexOrder = querycolumn.IndexOrder,
                                    newcolumn.ObjectFieldDataTypeCode = querycolumn.ObjectFieldDataTypeCode,
                                    newcolumn.ObjectFieldFieldLableTextCodeDefaultText = querycolumn.ObjectFieldFieldLableTextCodeDefaultText,
                                    newcolumn.ObjectFieldId = querycolumn.ObjectFieldId,
                                    newcolumn.ObjectFieldListLabelTextCodeCode = querycolumn.ObjectFieldListLabelTextCodeCode,
                                    newcolumn.QueryCode = querycolumn.QueryCode,
                                    newcolumn.QueryId = querycolumn.QueryId,
                                    //newcolumn.QueryCode = querycolumn.QueryCode,
                                    newcolumn.QueryObjectTableName = querycolumn.QueryObjectTableName,
                                    newcolumn.ObjectFieldFieldLableTextCodeCode = querycolumn.ObjectFieldFieldLableTextCodeCode,
                                    newcolumn.ObjectFieldCode = querycolumn.ObjectFieldCode,
                                    // TenantContext.Current.GeneralContext.QueryColumnPMs.Add(newcolumn);
                                    this.queryColumnsList.push(newcolumn);
                                    this.addedQueryColumnList.push(newcolumn);
                                //copy = true;

                            });

                        });

                }


                this.staticColumnsList = this.queryColumnsList.filter(q => q.QueryCode == this.QueryCode && ((q.UserId == SessionInfo.LoggedUserId && q.Tenant == SessionInfo.LoggedUserTenant))).sort((a, b) => { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1 });

                var listColumns = this.queryColumnsList.filter(q => q.QueryCode == this.QueryCode && ((q.UserId == SessionInfo.LoggedUserId && q.Tenant == SessionInfo.LoggedUserTenant))).sort((a, b) => { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1 });


                this.unselectedObjectFields = window.ObjectFields.filter(a => a.ObjectTableName == this.CurrentObjectTable).filter(d => d.DisplayInList == true && (d.Tenant == SessionInfo.LoggedUserTenant || d.Tenant == 0) && ((d.ValidForQuerySection1 == currentQuery.QuerySection || d.ValidForQuerySection2 == currentQuery.QuerySection || (d.AdditionalQuerySections && d.AdditionalQuerySections.split(',').indexOf(currentQuery.QuerySection) > -1)) || d.IsCustom == true));



                this.unselected = [];
                this.unselectedObjectFields.forEach((field, key) => {
                    var xx = this.queryColumnsList.filter(q => q.QueryCode == this.QueryCode && q.ObjectFieldCode == field.FieldCode && field.FieldName != "TimeFrameFilter");
                    var yy = this.unselected.filter(q => q.Id == field.Id);

                    if (xx.length == 0 && yy.length == 0) {
                        this.unselected.push(field);
                    }
                });


                //this.UnSelectedQueryColumnsList.ItemsSource = unselected.OrderBy(c => c.FieldName);
                this.OrderedQueryColumnsList = [];
                this.unSelectedList = this.unselected.sort((a, b) => { return (a.FieldName.toLowerCase() === b.FieldName.toLowerCase()) ? 0 : (a.FieldName.toLowerCase() < b.FieldName.toLowerCase()) ? -1 : 1 });
                this.queryColumnsList.forEach((qc, key) => {
                    var CurColumn = this.OrderedQueryColumnsList.filter(a => a.ObjectFieldName == qc.ObjectFieldName);
                    if (CurColumn == null || CurColumn.length == 0) {
                        this.OrderedQueryColumnsList.push(new QueryColumnDetails(qc));
                    }
                });

                this.OrderedQueryColumnsList = this.OrderedQueryColumnsList.sort((a, b) => { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1 });
                this.CD.detectChanges();
                //SelectedQueryColumnsList.ItemsSource = OrderedQueryColumnsList;
                this.Fixedunselected = this.unSelectedList;

                this.IsEnabled = true;
                this.onUnSelectedDataLoadedEvent.emit(this.SelectedItem);
                this.onSelectedDataLoadedEvent.emit(this.FieldSelectedItem);
            });
    }

    //txtSearch_TextChanged
    private searchText: string;
    public get SearchText() { return this.searchText; }
    public set SearchText(newValue: string) {
        this.searchText = newValue;
        if (newValue != null && newValue != "") {
            this.unSelectedList = this.Fixedunselected.filter(f => TextCodeTranslator.Translate(f.FullNameTextCodeCode).toLowerCase().indexOf(newValue.toLowerCase()) > -1 );
        }
        else {
            this.unSelectedList = this.Fixedunselected;
        }
        this.onUnselectedDataSourceChangedEvent.emit(this.unSelectedList);
        //this.onUnSelectedDataLoadedEvent.emit(this.SelectedItem);

    }
    private selectedItem: any;
    public get SelectedItem() { return this.selectedItem; }
    public set SelectedItem(newValue: any) {
        this.selectedItem = newValue;
    }

    private fieldSelectedItem: any;
    public get FieldSelectedItem() { return this.fieldSelectedItem; }
    public set FieldSelectedItem(newValue: any) {
        this.fieldSelectedItem = newValue;
    }

    private isbtnAddEnabled: boolean = true;
    public get IsbtnAddEnabled() { return this.isbtnAddEnabled; }
    public set IsbtnAddEnabled(newValue: boolean) {
        this.isbtnAddEnabled = newValue;
    }

    private isbtnRemoveEnabled: boolean = true;
    public get IsbtnRemoveEnabled() { return this.isbtnRemoveEnabled; }
    public set IsbtnRemoveEnabled(newValue: boolean) {
        this.isbtnRemoveEnabled = newValue;
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

    onSelectedItemChanged(item) {
        this.SelectedItem = item;
        this.IsbtnAddEnabled = true;
        this.IsbtnRemoveEnabled = false;
        this.IsbtnUpEnabled = false;
        this.IsbtnDownEnabled = false;
        this.onUnSelectedDataLoadedEvent.emit(null);
    }

    onFieldSelectedItemChanged(item) {
        this.FieldSelectedItem = item;
        //if (UnSelectedQueryColumnsList.SelectedItem != null) {
        this.IsbtnAddEnabled = false;
        this.IsbtnRemoveEnabled = true;
        this.IsbtnUpEnabled = true;
        this.IsbtnDownEnabled = true;
        this.onSelectedDataLoadedEvent.emit(null);
        //SelectedQueryColumnsList.SelectedItem = null;
        //}
    }

    btnUp_Click() {

        var item = this.SelectedItem;
        if (item != null) {
            this.HasChanges = true;
            var i = this.OrderedQueryColumnsList.indexOf(item);

            this.ReorderColumnsList();
            var upColumn = this.OrderedQueryColumnsList.filter(d => d.ObjectFieldCode == item.ObjectFieldCode && ((d.Tenant == SessionInfo.LoggedUserTenant && d.UserId == SessionInfo.LoggedUserId) || d.Tenant == 0) && d.QueryCode == this.QueryCode)[0];

            if (i > 0) {
                this.OrderedQueryColumnsList = this.OrderedQueryColumnsList.filter(d => d.ObjectFieldCode != upColumn.ObjectFieldCode);
                this.OrderedQueryColumnsList.filter(o => o.IndexOrder == i - 1)[0].IndexOrder = i;
                upColumn.IndexOrder = i - 1;
                this.OrderedQueryColumnsList.splice(i - 1, 0, upColumn);
                this.onDataSourceChangedEvent.emit(this.OrderedQueryColumnsList);
                //this.ReorderColumnsList();
                this.onSelectedDataLoadedEvent.emit(this.SelectedItem);
            }
        }

    }

    btnDown_Click() {
        var item = this.SelectedItem;
        if (item != null) {
            this.HasChanges = true;

            var i = this.OrderedQueryColumnsList.indexOf(item);

            this.ReorderColumnsList();

            var downColumn = this.OrderedQueryColumnsList.filter(d => d.ObjectFieldCode == item.ObjectFieldCode && ((d.Tenant == SessionInfo.LoggedUserTenant && d.UserId == SessionInfo.LoggedUserId) || d.Tenant == 0) && d.QueryCode == this.QueryCode)[0];

            if (i < this.OrderedQueryColumnsList.length - 1) {
                this.OrderedQueryColumnsList = this.OrderedQueryColumnsList.filter(d => d.ObjectFieldCode != downColumn.ObjectFieldCode);

                this.OrderedQueryColumnsList.filter(o => o.IndexOrder == i + 1)[0].IndexOrder = i;
                downColumn.IndexOrder = i + 1;
                this.OrderedQueryColumnsList.splice(i + 1, 0, downColumn);
                this.onDataSourceChangedEvent.emit(this.OrderedQueryColumnsList);
                //this.ReorderColumnsList();
                this.onSelectedDataLoadedEvent.emit(this.SelectedItem);
            }
        }

    }

    btnAdd_Click() {
        this.HasChanges = true;
        if (this.FieldSelectedItem != null) {

            var field = this.FieldSelectedItem;

            var queryColumn = this.unSelectedList.filter(a => a.QueryCode == this.QueryCode && a.FieldName == field.FieldName)[0];
            if (queryColumn) {
                this.removedQueryColumnList = this.removedQueryColumnList.filter(a => a.FieldName != queryColumn.FieldName);
            }
            if (queryColumn) {
                if (queryColumn.Id) {
                    var newQueryColumn = new QueryColumnPM();

                        newQueryColumn.QueryId = this.QueryId,
                        newQueryColumn.QueryCode = this.QueryCode,
                        newQueryColumn.ObjectFieldCode = field.FieldCode,
                        newQueryColumn.ObjectFieldId = field.Id,
                        //ObjectField = field,
                        newQueryColumn.ObjectFieldName = field.FieldName,
                        newQueryColumn.ObjectFieldFieldLableTextCodeDefaultText = field.FullNameTextCodeDefaultText,
                        newQueryColumn.Tenant = SessionInfo.LoggedUserTenant,
                        newQueryColumn.IndexOrder = (this.OrderedQueryColumnsList.length > 0 ? this.OrderedQueryColumnsList[this.OrderedQueryColumnsList.length - 1].IndexOrder + 1 : 0),
                        newQueryColumn.ColumnWidth = 100,
                        newQueryColumn.ConverterName = field.ConverterName,
                        newQueryColumn.DataTemplateName = field.DataTemplateName,
                        newQueryColumn.ObjectFieldListLabelTextCodeCode = field.ListTextCodeCode,
                        newQueryColumn.DisplayInList = true,
                        newQueryColumn.UserId = SessionInfo.LoggedUserId,
                        newQueryColumn.ObjectFieldFieldLableTextCodeCode = field.FullNameTextCodeCode,


                        this.OrderedQueryColumnsList.push(new QueryColumnDetails(newQueryColumn));

                    this.addedQueryColumnList.push(newQueryColumn);


                }
                else {

                    queryColumn.IndexOrder = (this.OrderedQueryColumnsList.length > 0 ? this.OrderedQueryColumnsList[this.OrderedQueryColumnsList.length - 1].IndexOrder + 1 : 0);
                    this.OrderedQueryColumnsList.push(new QueryColumnDetails(queryColumn));
                }
            }
            else {
                var newQueryColumn = new QueryColumnPM();

                newQueryColumn.QueryId = this.QueryId,
                    newQueryColumn.QueryCode = this.QueryCode,

                    newQueryColumn.ObjectFieldCode = field.FieldCode,
                    newQueryColumn.ObjectFieldId = field.Id,
                    //  ObjectField = field,
                    newQueryColumn.ObjectFieldName = field.FieldName,
                    newQueryColumn.ObjectFieldFieldLableTextCodeDefaultText = field.FullNameTextCodeDefaultText,
                    newQueryColumn.Tenant = SessionInfo.LoggedUserTenant,
                    newQueryColumn.IndexOrder = (this.OrderedQueryColumnsList.length > 0 ? this.OrderedQueryColumnsList[this.OrderedQueryColumnsList.length - 1].IndexOrder + 1 : 0),
                    newQueryColumn.ColumnWidth = 100,
                    newQueryColumn.ConverterName = field.ConverterName,
                    newQueryColumn.DataTemplateName = field.DataTemplateName,

                    newQueryColumn.ObjectFieldListLabelTextCodeCode = field.ListTextCodeCode,
                    newQueryColumn.DisplayInList = true,
                    newQueryColumn.UserId = SessionInfo.LoggedUserId,
                    newQueryColumn.ObjectFieldFieldLableTextCodeCode = field.FullNameTextCodeCode,


                    this.OrderedQueryColumnsList.push(new QueryColumnDetails(newQueryColumn));

                this.addedQueryColumnList.push(newQueryColumn);

            }


            this.unSelectedList = this.unSelectedList.filter(a => a.FieldName != field.FieldName);
            this.Fixedunselected = this.Fixedunselected.filter(a => a.FieldName != field.FieldName);
            this.IsbtnAddEnabled = false; 
            this.CD.detectChanges();


            if (this.SearchText != null && this.SearchText != "") {
                this.unSelectedList = this.unSelectedList.filter(f => f.FullNameTextCodeDefaultText.toLowerCase().indexOf(this.SearchText.toLowerCase()) >= 0 || ((f.FullNameTextCodeLocalDefaultText != null && f.FullNameTextCodeLocalDefaultText != "") && f.FullNameTextCodeLocalDefaultText.toLowerCase().indexOf(this.SearchText.toLowerCase())));
            }
            this.onUnSelectedDataLoadedEvent.emit(this.SelectedItem);


        } 
        //this.ReorderColumnsList();
        this.onSelectedDataLoadedEvent.emit(this.FieldSelectedItem);
        this.FieldSelectedItem = null;
        this.SelectedItem = null;
    }

    btnRemove_Click() {
        this.HasChanges = true;
        if (this.SelectedItem) {
            var queryColumn = this.SelectedItem;

            var objectField = window.ObjectFields.filter(a => a.FieldCode == queryColumn.ObjectFieldCode)[0];

            this.unSelectedList.push(objectField);
            //-----
            this.OrderedQueryColumnsList = this.OrderedQueryColumnsList.filter(a => a.ObjectFieldCode != queryColumn.ObjectFieldCode);


            var pm = this.queryColumnsList.filter(a => a.ObjectFieldCode == queryColumn.ObjectFieldCode && a.QueryCode == this.QueryCode)[0];

            if (pm != null) {
                this.removedQueryColumnList.push(pm);
            }

            var queryColumn2 = this.addedQueryColumnList.filter(a => a.ObjectFieldCode == queryColumn.ObjectFieldCode && a.QueryCode == this.QueryCode)[0];

            if (queryColumn2 != null) {
                this.addedQueryColumnList = this.addedQueryColumnList.filter(a => a.ObjectFieldCode != queryColumn2.ObjectFieldCode);
            }

            this.OrderedQueryColumnsList.forEach((column, key) => {
                if (column.IndexOrder > queryColumn.IndexOrder) {
                    column.IndexOrder--;
                }
            });

            //this.ReorderColumnsList();
            this.IsbtnRemoveEnabled = false;
            this.CD.detectChanges();
            this.onUnSelectedDataLoadedEvent.emit(this.SelectedItem);
            this.onSelectedDataLoadedEvent.emit(this.FieldSelectedItem);
            this.FieldSelectedItem = null;
            this.SelectedItem = null;
        }
    }

    ReorderColumnsList() {
        var queryColumnList = this.OrderedQueryColumnsList.sort((a, b) => { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1 });
        //.filter(d => d.Id != null && d.QueryId == this.QueryId && d.UserId == SessionInfo.LoggedUserId && d.Tenant == SessionInfo.LoggedUserTenant)
        //var TenantZeroqueryColumnList = this.OrderedQueryColumnsList.filter(d => d.QueryId == this.QueryId && d.UserId == null && d.Tenant == 0).sort((a, b) => { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1 });

        var i = 0;
        for (; i < queryColumnList.length; i++) {
            queryColumnList[i].IndexOrder = i;
        }
        //var queryColumnList = this.OrderedQueryColumnsList.filter(d => d.QueryId == this.QueryId && d.UserId == SessionInfo.LoggedUserId && d.Tenant == SessionInfo.LoggedUserTenant).sort((a, b) => { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1 });
        //var i = 0;
        //for (; i < queryColumnList.length; i++) {
        //    queryColumnList[i].IndexOrder = i;
        //}

    }

    SaveChanges() {
        this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving ...");
        //this.needsRebuildList = this.copy;

        //this.addedQueryColumnList.forEach((queryColumn, key) => {

        //    var temp = this.queryColumnsList.filter(a => a.QueryId == this.QueryId && a.ObjectFieldId == queryColumn.ObjectFieldId && a.Tenant == SessionInfo.LoggedUserTenant && a.UserId == SessionInfo.LoggedUserId);
        //    var tempbool = false;
        //    if (temp && (temp.length > 0)) {
        //        tempbool = true;
        //    }
        //    var exists = tempbool;
        //    if (!exists) {
        //        if (this.myQueryColumnsPMService == null) {
        //            this.myQueryColumnsPMService = new QueryColumnsPMService();
        //            this.myQueryColumnsPMService.setServiceArgs(this.serviceArgs);
        //        }
        //        this.myQueryColumnsPMService.insert(queryColumn).subscribe((myResult:any) => {
        //            this.CurrentSession.CloseCurrentWindow();
        //        });
        //    }
        //});



        var Length = 0;
        this.OrderedQueryColumnsList.forEach((queryColumn, key) => {

            var temp = this.queryColumnsList.filter(a => a.QueryCode == this.QueryCode && a.ObjectFieldCode == queryColumn.ObjectFieldCode && a.Tenant == SessionInfo.LoggedUserTenant && a.UserId == SessionInfo.LoggedUserId);

            if (temp.length == 0) {
                temp = this.queryColumnsList.filter(a => a.QueryCode == this.QueryCode && a.ObjectFieldCode == queryColumn.ObjectFieldCode && a.Tenant == 0);
            }
            if (temp.length == 0) {
                temp = this.addedQueryColumnList.filter(a => a.ObjectFieldCode == queryColumn.ObjectFieldCode);
            }
                var qc = temp[0];
                if (qc) {
                    if (this.myQueryColumnsPMService == null) {
                        this.myQueryColumnsPMService = new QueryColumnsPMService();
                        this.myQueryColumnsPMService.setServiceArgs(this.serviceArgs);
                    }
                    qc.Tenant = SessionInfo.LoggedUserTenant;
                    qc.IndexOrder = queryColumn.IndexOrder;
                    qc.UserId = SessionInfo.LoggedUserId;
                    this.myQueryColumnsPMService.update(qc).subscribe((myResult:any) => {
                        Length++;
                        if (Length == this.OrderedQueryColumnsList.length && this.removedQueryColumnList.length == 0) {
                            this.CurrentSession.CurrentWindow.StopBusyIndicator();
                            this.CurrentSession.CloseCurrentWindow();
                        }
                    });
                }
            
        });
        var removedQueryLength = 0;
        this.removedQueryColumnList.forEach((queryColumn, key) => {

            var temp = this.queryColumnsList.filter(a => a.QueryCode == this.QueryCode && a.ObjectFieldCode == queryColumn.ObjectFieldCode && a.Tenant == SessionInfo.LoggedUserTenant && a.UserId == SessionInfo.LoggedUserId);
            var tempbool = false;
            //if (temp && (temp.length > 0)) {
            //    tempbool = true;
            //}
            //var exists = tempbool;
            //if (exists) {
                if (this.myQueryColumnsPMService == null) {
                    this.myQueryColumnsPMService = new QueryColumnsPMService();
                    this.myQueryColumnsPMService.setServiceArgs(this.serviceArgs);
                }
                this.myQueryColumnsPMService.delete(queryColumn).subscribe((myResult:any) => {
                    removedQueryLength = removedQueryLength + 1;
                    if (removedQueryLength == this.removedQueryColumnList.length) {
                        this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        this.CurrentSession.CloseCurrentWindow();
                    }
                });
            //}
        });

        //if (this.addedQueryColumnList.length == 0 && this.removedQueryColumnList.length == 0) {
        //    this.CurrentSession.CloseCurrentWindow();
        //}
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

}

export class QueryColumnDetails {

    constructor(qc: QueryColumnPM) {
        this.Id = qc.Id;
        this.IndexOrder = qc.IndexOrder;
        this.ColumnWidth = qc.ColumnWidth > 0 ? qc.ColumnWidth : 100;
        this.ObjectFieldDataTypeCode = qc.ObjectFieldDataTypeCode;
        this.DisplayInList = qc.DisplayInList;
        this.ObjectFieldId = qc.ObjectFieldId;
        this.ObjectFieldName = qc.ObjectFieldName;
        this.QueryCode = qc.QueryCode;
        this.QueryId = qc.QueryId;
        this.QueryObjectTableName = qc.QueryObjectTableName;
        this.Tenant = qc.Tenant;
        this.UserId = qc.UserId;
        this.UpdatedByTenant = qc.UpdatedByTenant;
        this.DataTemplateName = qc.DataTemplateName;
        this.ColumnHeaderTemplateName = qc.ColumnHeaderTemplateName;
        this.ConverterName = qc.ConverterName;
        this.ObjectFieldListLabelTextCodeCode = qc.ObjectFieldListLabelTextCodeCode;
        this.ObjectFieldFieldLableTextCodeDefaultText = qc.ObjectFieldFieldLableTextCodeDefaultText;
        this.ObjectFieldFieldLableTextCodeCode = qc.ObjectFieldFieldLableTextCodeCode;
        this.ObjectFieldCode = qc.ObjectFieldCode;

        this.QueryColumnPM = qc;
    }

    private id: string;
    public get Id() { return this.id; }
    public set Id(newValue: string) { this.id = newValue; }


    private tenant: number;
    public get Tenant() { return this.tenant; }
    public set Tenant(newValue: number) { this.tenant = newValue; }


    private queryId: string;
    public get QueryId() { return this.queryId; }
    public set QueryId(newValue: string) { this.queryId = newValue; }


    private objectFieldId: string;
    public get ObjectFieldId() { return this.objectFieldId; }
    public set ObjectFieldId(newValue: string) { this.objectFieldId = newValue; }


    private indexOrder: number;
    public get IndexOrder() { return this.indexOrder; }
    public set IndexOrder(newValue: number) { this.indexOrder = newValue; }


    private columnWidth: number;
    public get ColumnWidth() { return this.columnWidth; }
    public set ColumnWidth(newValue: number) { this.columnWidth = newValue; }


    private objectFieldName: string;
    public get ObjectFieldName() { return this.objectFieldName; }
    public set ObjectFieldName(newValue: string) { this.objectFieldName = newValue; }


    private queryCode: string;
    public get QueryCode() { return this.queryCode; }
    public set QueryCode(newValue: string) { this.queryCode = newValue; }


    private displayInList: boolean;
    public get DisplayInList() { return this.displayInList; }
    public set DisplayInList(newValue: boolean) { this.displayInList = newValue; }


    private objectFieldDataTypeCode: string;
    public get ObjectFieldDataTypeCode() { return this.objectFieldDataTypeCode; }
    public set ObjectFieldDataTypeCode(newValue: string) { this.objectFieldDataTypeCode = newValue; }

    private queryObjectTableName: string;
    public get QueryObjectTableName() { return this.queryObjectTableName; }
    public set QueryObjectTableName(newValue: string) { this.queryObjectTableName = newValue; }


    private dataTemplateName: boolean;
    public get DataTemplateName() { return this.dataTemplateName; }
    public set DataTemplateName(newValue: boolean) { this.dataTemplateName = newValue; }


    private columnHeaderTemplateName: string;
    public get ColumnHeaderTemplateName() { return this.columnHeaderTemplateName; }
    public set ColumnHeaderTemplateName(newValue: string) { this.columnHeaderTemplateName = newValue; }


    private converterName: string;
    public get ConverterName() { return this.converterName; }
    public set ConverterName(newValue: string) { this.converterName = newValue; }


    private updatedByTenant: number;
    public get UpdatedByTenant() { return this.updatedByTenant; }
    public set UpdatedByTenant(newValue: number) { this.updatedByTenant = newValue; }


    private objectFieldListLabelTextCodeCode: string;
    public get ObjectFieldListLabelTextCodeCode() { return this.objectFieldListLabelTextCodeCode; }
    public set ObjectFieldListLabelTextCodeCode(newValue: string) { this.objectFieldListLabelTextCodeCode = newValue; }

    private objectFieldFieldLableTextCodeDefaultText: string;
    public get ObjectFieldFieldLableTextCodeDefaultText() { return this.objectFieldFieldLableTextCodeDefaultText; }
    public set ObjectFieldFieldLableTextCodeDefaultText(newValue: string) { this.objectFieldFieldLableTextCodeDefaultText = newValue; }

    private objectFieldFieldLableTextCodeCode: string;
    public get ObjectFieldFieldLableTextCodeCode() { return this.objectFieldFieldLableTextCodeCode; }
    public set ObjectFieldFieldLableTextCodeCode(newValue: string) { this.objectFieldFieldLableTextCodeCode = newValue; }



    private userId: string;
    public get UserId() { return this.userId; }
    public set UserId(newValue: string) { this.userId = newValue; }

    private queryColumnPM: QueryColumnPM;
    public get QueryColumnPM() { return this.queryColumnPM; }
    public set QueryColumnPM(newValue: QueryColumnPM) { this.queryColumnPM = newValue; }

    private objectFieldFullNameTextCodeCode: string;
    public get ObjectFieldFullNameTextCodeCode() { return this.objectFieldFullNameTextCodeCode; }
    public set ObjectFieldFullNameTextCodeCode(newValue: string) { this.objectFieldFullNameTextCodeCode = newValue; }

    private objectFieldCode: string;
    public get ObjectFieldCode() { return this.objectFieldCode; }
    public set ObjectFieldCode(newValue: string) { this.objectFieldCode = newValue; }

}

