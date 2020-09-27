declare var System: any;
declare var window: any;

import {Component, OnInit, EventEmitter, Output, AfterViewInit, ChangeDetectorRef, Query} from '@angular/core';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
//import {TextCodeTranslationPipe} from '../../../../Controls/Pipes/TextCodeTranslationPipe';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {GeneralEntitiesArgs} from '../../../../Infrastructure/DataContracts/GeneralEntitiesArgs';
import {GeneralEntitiesService} from '../../../../Infrastructure/Services/StandardPMs/GeneralEntitiesService';
import {AdvancedQueryFiltersPMService} from '../../../../Infrastructure/Services/StandardPMs/AdvancedQueryFiltersPMService';

import {ServiceArgs} from '../../../../Infrastructure/DataContracts/ServiceArgs';
import {SessionInfo} from '../../../../Infrastructure/Utilities/SessionInfo';
import {AdvancedQueryFilterPM} from '../../../../Infrastructure/EntityPMs/AdvancedQueryFilterPM';
import {QueriesPMService} from '../../../../Infrastructure/Services/StandardPMs/QueriesPMService';
import {QueryColumnsPMService} from '../../../../Infrastructure/Services/StandardPMs/QueryColumnsPMService'; 
import {ServiceHelper} from '../../../../Infrastructure/Utilities/ServiceHelper';
import {CachedDataManager} from '../../../../Infrastructure/Utilities/CachedDataManager';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import { SharedUserQueryPM } from '../../../EntityPMs/SharedUserQueryPM';
import { QueryPM } from '../../../EntityPMs/QueryPM';
import { ServiceResponse } from '../../../DataContracts/ServiceResponse';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { HttpClient } from '@angular/common/http';


@Component({
    

    selector: 'QueryList',
    templateUrl: './QueryListComponent.html',
    inputs: ['ItemsSource', 'SelectedItem', 'Binding', 'UserItemSource', 'ObjectTableName', 'onSelectedQueryChangeEvent', 'LoadResourceCompleted', 'pubSubAdvanceQueryFiltersService', 'QueryListSourceChanged'],
    providers: [HttpClient],
})

export class QueryListComponent implements OnInit, AfterViewInit {
    public Text: string = null;    
    public UserItemSource: any[];
    public HandledUserItemSource: any[];
    public Binding: string = null;
    public SelectedItem: any = null;
    public ControlId: string = null;
    public DropdownId: string = null;
    public NewViewId: string = null;
    public ListControlId: string = null;
    public MinHeight: number = 30;
    public MaxHeight: number = 1000;
    @Output() itemSelectedEvent = new EventEmitter();
    @Output() QueriesChangedEvent = new EventEmitter();
    @Output() NewViewClosedEvent = new EventEmitter();
    LoadResourceCompleted: EventEmitter<any>;
    QueryListSourceChanged: EventEmitter<any>;
    ignoreMouseDown: boolean = false;
    ignoreItemClicked: boolean = false;
    ignorePublicClicked: boolean = false;
    newViewClicked: boolean = false;
    ShowButtons: boolean = false;
    public ObjectTableName: string;
    onSelectedQueryChangeEvent: EventEmitter<any>;
    GeneralEntitiesArgs: GeneralEntitiesArgs;
    private myAdvancedQueryFiltersPMService: AdvancedQueryFiltersPMService;
    public AdvancedQueryFilterPMs: AdvancedQueryFilterPM[];
    pubSubAdvanceQueryFiltersService: any;
    public serviceArgs: ServiceArgs;
    LayoutDirection: string = 'ltr';

    private itemsSource: any;
    public get ItemsSource() { return this.itemsSource; }
    public set ItemsSource(newValue: any) {
        this.itemsSource = newValue;
    }

    public NotSharedUserItemSource: any[];
    public SharedUserItemSource: any[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private CD: ChangeDetectorRef) {
        this.serviceArgs = new ServiceArgs();
        this.serviceArgs.http = ServiceHelper.HttpClient;
        this.ItemsSource = [];
        this.UserItemSource = [];
        this.HandledUserItemSource = [];
        this.NotSharedUserItemSource = [];
        this.SharedUserItemSource = [];

        if (this.CurrentSession == null) {
            this.ControlId = "QueryList_-1_-1";
            this.DropdownId = "QueryListDropdown_-1_-1";
            this.ListControlId = "QueryListList_-1_-1";
            this.NewViewId = "NewViewId_-1_-1";
        }

        else {
            var idIndex = this.CurrentSession.GetNewId("ComboBox");
            this.ControlId = "QueryList_" + idIndex;
            this.DropdownId = "QueryListDropdown_" + idIndex;
            this.ListControlId = "QueryListList_" + idIndex;
            this.NewViewId = "NewViewId_" + idIndex;
        }

        this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
    }
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    ngOnInit() {
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName, 0).subscribe((response:any) => {
            if (this.SelectedItem != null) {
                this.SetDisplayText();
            }

            var xx = this.DropdownId;
            var temp = this.ignorePublicClicked;

            this.onSelectedQueryChangeEvent.subscribe((res) => {
                this.ItemClicked(res, true);
                this.ComputeListHeight(this.ItemsSource.length + this.UserItemSource.length);
            });

            this.FillUserItemSource_Share();

            this.QueryListSourceChanged.subscribe((res) => {
                this.UserItemSource = res;
            });        
        }); 
    }

    private FillUserItemSource_Share() {
        this.NotSharedUserItemSource = [];
        this.SharedUserItemSource = [];

        for (let i = 0; i < this.UserItemSource.length; i++) {
            if (!AppTool.IsNullOrEmpty(this.UserItemSource[i])) {
                if (AppTool.IsNullOrEmpty(this.UserItemSource[i].SharedByUserId)) {
                    this.NotSharedUserItemSource.push(this.UserItemSource[i]);
                }
                else {
                    this.SharedUserItemSource.push(this.UserItemSource[i]);
                }
            }
        }

        //this.NotSharedUserItemSource = this.UserItemSource.filter(d => AppTool.IsNullOrEmpty(d.SharedByUserId));
        //this.SharedUserItemSource = this.UserItemSource.filter(d => !AppTool.IsNullOrEmpty(d.SharedByUserId));
    }

    ngAfterViewInit() {
        this.ComputeListHeight(this.ItemsSource.length + this.UserItemSource.length);
    }

    ComputeListHeight(ItemsCount: number) {
        //if (ItemsCount == 0) {
        //    document.getElementById(this.ListControlId).style.height = this.MinHeight + "px";
        //}
        //else {
        //    var itemsHeight = ((ItemsCount * 23) + 3);
        //    if (itemsHeight > this.MaxHeight) {
        //        document.getElementById(this.ListControlId).style.height = this.MaxHeight + "px";
        //    }

        //    else {
        //        document.getElementById(this.ListControlId).style.height = itemsHeight + 36 + "px";
        //    }
        //}
    }

    mousedown(event) {
        if (this.ItemsSource.length == 0 && this.UserItemSource.length == 0) {
            document.getElementById(this.DropdownId).style.height = this.MinHeight + "px";
        }

        else {
            var itemsHeight = ((this.ItemsSource.length * 23) + (this.UserItemSource.length * 23) + 3);
            if (itemsHeight > this.MaxHeight) {
                document.getElementById(this.DropdownId).style.height = this.MaxHeight + "px";
                document.getElementById(this.ListControlId).style.height = itemsHeight + "px";
            }

            else {
                document.getElementById(this.DropdownId).style.height = itemsHeight + 36 + "px";
                document.getElementById(this.ListControlId).style.height = "100%";
            }
        }

        document.getElementById(this.DropdownId).style.visibility = "visible";
    }

    OnFocus() {

    }

    OnLostFocus() {
        document.getElementById(this.DropdownId).style.height = "0px";
        document.getElementById(this.DropdownId).style.visibility = "hidden";
    }

    private IsOpened: boolean = false;
    ComboBoxClicked() {
        var item = document.getElementById(this.ControlId);
        if (item != null) {
            this.IsOpened = !this.IsOpened;

            if (!this.IsOpened) {
                item.blur();
            }
        }
    }

    ItemClicked(clickedItem: any, IgnoreChange: boolean = false) {
        if (clickedItem != null && this.ignoreItemClicked == false) {
            this.ignoreMouseDown = true;
            if ((this.SelectedItem != clickedItem) || IgnoreChange) {
                this.SelectedItem = clickedItem;

                var myComboBox = document.getElementById(this.ControlId);
                if (myComboBox != null) {
                    myComboBox.blur();
                }

                this.SetDisplayText();
                var filters = new ApiQueryFilters();
                
                if (window.PreDefinedFilters.filter(d => d.QueryCode == clickedItem.UniqueCode) != null) {
                    var predefinedFilters = window.PreDefinedFilters.filter(d => d.QueryCode == clickedItem.UniqueCode);
                    predefinedFilters.forEach((filter, key) => {
                        var filterOperator = (!AppTool.IsNullOrEmpty(filter.Operator)) ? filter.Operator : filter.ObjectFieldOperator;
                        var value1 = filter.PredefinedValue;
                        var value2 = filter.PredefinedValue2;

                        if (value2 != null) {
                            filterOperator = "Between";
                        }

                        if (filter.DataTypeCode == "DateTime") {
                            var TodayDate = new Date();
                            if (value1 == '#today') value1 = new Date(TodayDate.getFullYear(), TodayDate.getMonth(), TodayDate.getDate(), 0, 0, 0);
                            if (value2 == '#today') value2 = new Date(TodayDate.getFullYear(), TodayDate.getMonth(), TodayDate.getDate(), 23, 59, 59);

                            var YesterdayDate = DateTool.AddDays((new Date()), -1);
                            var LastSevenDaysDate = DateTool.AddDays((new Date()), -7)
                            var LastThirtyDaysDate = DateTool.AddDays((new Date()), -30);
                            var CurrentYearFromDate = new Date(new Date().getFullYear(), 0, 1);
                            var CurrentYearToDate = new Date();
                            var LastYearFromDate = DateTool.AddDays((new Date()), -365);
                            var LastYearToDate = new Date();
                            
                            if (value1 == "Today") {
                                value1 = TodayDate;
                                filterOperator = "Equals";
                            }

                            else if (value1 == "Yesterday") {
                                value1 = YesterdayDate;
                                filterOperator = "GreaterThanOrEqual";
                            }

                            else if (value1 == "Last 7 Days") {
                                value1 = LastSevenDaysDate;
                                filterOperator = "GreaterThanOrEqual";
                            }

                            else if (value1 == "Last 30 Days") {
                                value1 = LastThirtyDaysDate;
                                filterOperator = "GreaterThanOrEqual";
                            }

                            else if (value1 == "Current Year") {
                                value1 = CurrentYearFromDate;
                                value2 = CurrentYearToDate;
                                filterOperator = "Between";
                            }

                            else if (value1 == "Last Year") {
                                value1 = LastYearFromDate;
                                value2 = LastYearToDate;
                                filterOperator = "Between";
                            }
                        }
                       
                        var ObjectField = window.ObjectFields.filter(d => d.FieldCode == filter.ObjectFieldCode);
                        filters.addAdditionalFilter(
                            filter.ObjectFieldName, value1, value2, null, filterOperator, ObjectField.IsCustomFilter, filter.DisplayInList, ObjectField.IsCustom, filter.DataTypeCode);
                    });
                }
                
                if (!AppTool.IsNullOrEmpty(clickedItem.DefaultSortColumn)) {
                    filters.SortBy = clickedItem.DefaultSortColumn;
                }

                if (!AppTool.IsNullOrEmpty(clickedItem.DefaultSortDirection)) {
                    filters.SortDirection = clickedItem.DefaultSortDirection;
                }
                
                this.itemSelectedEvent.emit({ QueryCode: clickedItem.UniqueCode, Filters: filters, Title: TextCodeTranslator.Translate(clickedItem.NameTextCodeCode) });
            }
        }

        else if (this.ignoreItemClicked == true) {
            this.ignoreItemClicked = false;
        }        
    }

    private SetDisplayText() {
        var myDisplayText: string = null;

        if (this.SelectedItem != null) {
            if (this.Binding == null) {
                myDisplayText = this.SelectedItem;
            }

            else {
                myDisplayText = this.SelectedItem[this.Binding];
            }
        }

        this.Text = TextCodeTranslator.Translate(myDisplayText);
    }

    NewViewClicked() {
        this.newViewClicked = true;
        this.ignoreMouseDown = false;

        var windowArgs: any = {};
        windowArgs.queryCode = this.SelectedItem.UniqueCode;
        windowArgs.queryId = this.SelectedItem.Id;
        windowArgs.currentObjectTable = this.ObjectTableName;
        windowArgs.IsNew = true;
        windowArgs.pubSubAdvanceQueryFiltersService = this.pubSubAdvanceQueryFiltersService;

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 960;
        logitudeWindow.Height = 610;
        logitudeWindow.Title = TextCodeTranslator.Translate("General.O.CreateNewView");
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./Infrastructure/Components/NewViewComponent/NewViewComponent');
        logitudeWindow.WindowClosed.subscribe(($event: any) => {
            var ObjectTable = window.ObjectTables.filter(x => x.Name === this.ObjectTableName)[0];
            if ($event != this.SelectedItem.UniqueCode) {
                CachedDataManager.RefreshTenantTextCodes().subscribe((response:any) => {
                    var Query = window.Queries.filter(a => a.ObjectTableId === ObjectTable.Id && a.UniqueCode == $event)[0];
                    this.UserItemSource.push(Query);
                    this.ComputeListHeight(this.ItemsSource.length + this.UserItemSource.length);
                    this.SelectedItem = Query;
                    this.SetDisplayText();
                    this.FillUserItemSource_Share();
                    this.QueriesChangedEvent.emit(Query);
                    this.NewViewClosedEvent.emit("");
                });
            }
        });
    }
   



    IgnoreMouseDown() {
        if (this.newViewClicked == true) {
            this.ignoreMouseDown = false;
        }
        else {
            this.ignoreMouseDown = true;
        }
    }

    DeleteButtonClicked(Item) {
        var confirmWindow = new ConfirmWindow();
        confirmWindow.Title = TextCodeTranslator.Translate("General.O.DeletQuery");
        confirmWindow.Show(TextCodeTranslator.Translate("General.M.WantToDeleteThisQuery"));
        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes) {
                this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));

                var query = window.Queries.filter(q => q.UniqueCode == Item.UniqueCode)[0];
                var myService: QueriesPMService = new QueriesPMService();
                myService.setServiceArgs(this.serviceArgs);

                myService.delete(query, SessionInfo.LoggedUserId).subscribe((myResult:any) => {
                    this.CurrentSession.StopBusyIndicator();
                    window.Queries = window.Queries.filter(a => a.UniqueCode != query.UniqueCode);
                    var ObjectTable = window.ObjectTables.filter(x => x.Name === this.ObjectTableName)[0];
                    var Query = window.Queries.filter(a => a.ObjectTableId === ObjectTable.Id && a.IndexOrder == 0)[0];
                    this.UserItemSource = this.UserItemSource.filter(a => a.UniqueCode != query.UniqueCode);
                    this.ComputeListHeight(this.ItemsSource.length + this.UserItemSource.length);
                    this.FillUserItemSource_Share();
                    this.QueriesChangedEvent.emit(Query);
                });
            }
        });


        



        //this.GeneralEntitiesArgs = new GeneralEntitiesArgs();
        //this.GeneralEntitiesArgs.RemovedQueryColumnsPMs = [];
        //this.GeneralEntitiesArgs.RemovedQueryFilters = [];
        //this.ignoreItemClicked = true;
        //var ObjectTable = window.ObjectTables.filter(a => a.Name == this.ObjectTableName)[0];

        //var confirmWindow = new ConfirmWindow();
        //confirmWindow.Title = TextCodeTranslator.Translate("General.O.DeletQuery");
        //confirmWindow.Show(TextCodeTranslator.Translate("General.M.WantToDeleteThisQuery"));
        //confirmWindow.WindowClosed.subscribe((event: any) => {
        //    if (confirmWindow.Yes) {
        //        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
        //        this.GeneralEntitiesArgs.Tenant = SessionInfo.LoggedUserTenant;
        //        var myQCService: QueryColumnsPMService = new QueryColumnsPMService();
        //        myQCService.setServiceArgs(this.serviceArgs);
        //        myQCService.GetQueryColumnPMs(SessionInfo.LoggedUserTenant, Item.Id, ObjectTable.Id, SessionInfo.LoggedUserId).subscribe((myResult:any) => {
        //            var queryColumns = myResult;

        //            queryColumns.forEach((column, key) => {
        //                this.GeneralEntitiesArgs.RemovedQueryColumnsPMs.push(column);
        //            });

        //            if (this.myAdvancedQueryFiltersPMService == null) {
        //                this.myAdvancedQueryFiltersPMService = new AdvancedQueryFiltersPMService();
        //            }

        //            this.myAdvancedQueryFiltersPMService.setServiceArgs(this.serviceArgs);
        //            this.myAdvancedQueryFiltersPMService.getadvancedqueryfiltersbytenantByQuery(SessionInfo.LoggedUserTenant, SessionInfo.LoggedUserId, Item.Id).subscribe((myResult:any) => {
        //                if (myResult == null) {
        //                    this.AdvancedQueryFilterPMs = [];
        //                }

        //                else {
        //                    this.AdvancedQueryFilterPMs = myResult;
        //                    var advanceQueryFilters = this.AdvancedQueryFilterPMs.filter(c => c.QueryId == Item.Id);
        //                    advanceQueryFilters.forEach((filter, key) => {
        //                        this.GeneralEntitiesArgs.RemovedQueryFilters.push(filter);
        //                    });
        //                }

        //                var query = window.Queries.filter(q => q.Id == Item.Id)[0];
        //                var myService: QueriesPMService = new QueriesPMService();
        //                myService.setServiceArgs(this.serviceArgs);
        //                var myGeneralService: GeneralEntitiesService = new GeneralEntitiesService();
        //                myGeneralService.setServiceArgs(this.serviceArgs);
        //                myGeneralService.update(this.GeneralEntitiesArgs).subscribe((myResult:any) => {
        //                    myService.delete(query).subscribe((myResult:any) => {
        //                        this.CurrentSession.StopBusyIndicator();
        //                        window.Queries = window.Queries.filter(a => a.Id != query.Id);
        //                        var ObjectTable = window.ObjectTables.filter(x => x.Name === this.ObjectTableName)[0];
        //                        var Query = window.Queries.filter(a => a.ObjectTableId === ObjectTable.Id && a.IndexOrder == 0)[0];
        //                        this.UserItemSource = this.UserItemSource.filter(a => a.Id != query.Id);
        //                        this.ComputeListHeight(this.ItemsSource.length + this.UserItemSource.length);
        //                        this.FillUserItemSource_Share();
        //                        this.QueriesChangedEvent.emit(Query);
        //                    });
        //                });
        //            });
        //        });
        //    }
        //});
    }

    EditButtonClicked(Item) {
        this.ignoreItemClicked = true;
        this.ignoreMouseDown = false;
        var windowArgs: any = {};
        windowArgs.queryId = Item.Id;
        windowArgs.queryCode = Item.UniqueCode;
        windowArgs.pubSubAdvanceQueryFiltersService = this.pubSubAdvanceQueryFiltersService;
        windowArgs.currentObjectTable = this.ObjectTableName;
        windowArgs.IsNew = false;
        windowArgs.QueryName = TextCodeTranslator.Translate(Item[this.Binding]);
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 960;
        logitudeWindow.Height = 610;
        logitudeWindow.Title = TextCodeTranslator.Translate("General.B.EditView");
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./Infrastructure/Components/NewViewComponent/NewViewComponent');
        logitudeWindow.WindowClosed.subscribe(($event: any) => {
            var ObjectTable = window.ObjectTables.filter(x => x.Name === this.ObjectTableName)[0];
            var Query = window.Queries.filter(a => a.ObjectTableId === ObjectTable.Id && a.UniqueCode == $event)[0];

            if (!Query) {
                Query = window.Queries.filter(a => a.ObjectTableId === ObjectTable.Id && a.IndexOrder == 0 && a.UniqueCode != $event)[0];
            }

            this.SetDisplayText();
            this.QueriesChangedEvent.emit(Query);
            this.FillUserItemSource_Share();
            this.NewViewClosedEvent.emit("");            
            this.CD.detectChanges();            
        });
    }
}
