"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var http_1 = require("@angular/http");
//import {ObjectFieldPM} from '../../../Infrastructure/EntityPMs/ObjectFieldPM';
var QueryColumnPM_1 = require("../../../Infrastructure/EntityPMs/QueryColumnPM");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var QueryColumnsPMService_1 = require("../../../Infrastructure/Services/StandardPMs/QueryColumnsPMService");
var ServiceArgs_1 = require("../../../Infrastructure/DataContracts/ServiceArgs");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var ObjectsLocator_1 = require("../../Locators/ObjectsLocator");
var QueryColumnsEditComponent = /** @class */ (function () {
    function QueryColumnsEditComponent(CD) {
        this.CD = CD;
        this.RTL = ObjectsLocator_1.ObjectsLocator.GlobalSetting == undefined ? false : (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == 'rtl' ? true : false);
        this.onSelectedDataLoadedEvent = new core_1.EventEmitter();
        this.onUnSelectedDataLoadedEvent = new core_1.EventEmitter();
        this.onDataSourceChangedEvent = new core_1.EventEmitter();
        this.onUnselectedDataSourceChangedEvent = new core_1.EventEmitter();
        this.HasChanges = false;
        this.needsRebuildList = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.isbtnAddEnabled = true;
        this.isbtnRemoveEnabled = true;
        this.isbtnUpEnabled = true;
        this.isbtnDownEnabled = true;
        this.serviceArgs = new ServiceArgs_1.ServiceArgs();
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this.serviceArgs.http = ServiceHelper_1.ServiceHelper.Http;
        ;
        if (this.CurrentSession == null) {
            this.SearchFieldsId = "SearchFields_-1_-1";
        }
        else {
            this.SearchFieldsId = "QueryColumnSearchFields_" + this.CurrentSession.GetNewId("QueryColumnSearchFields");
        }
        //this.Run();
    }
    QueryColumnsEditComponent.prototype.SetWindowArgs = function (args) {
        this.QueryId = args.queryId;
        this.isNewQueryMode = args.isNewQueryMode;
        this.CurrentObjectTable = args.currentObjectTable;
        this.IsEnabled = false;
        this.ObjectTable = window.ObjectTables.filter(function (d) { return d.Name == args.currentObjectTable; })[0];
        this.Run();
    };
    QueryColumnsEditComponent.prototype.ClearPlaceHolder = function () {
        var temp = document.getElementById(this.SearchFieldsId);
        temp.placeholder = "";
        temp.style.background = "rgba(0, 0, 0, 0)";
    };
    QueryColumnsEditComponent.prototype.FillPlaceHolder = function () {
        var temp = document.getElementById(this.SearchFieldsId);
        temp.placeholder = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.Search");
        temp.style.background = "url(Images/Search.png) no-repeat scroll";
        temp.style.backgroundPosition = "right center";
        temp.style.paddingRight = "30px";
    };
    QueryColumnsEditComponent.prototype.Run = function () {
        var _this = this;
        this.HasChanges = false;
        var copy = false;
        var currentQuery = window.Queries.filter(function (d) { return d.Id == _this.QueryId; })[0];
        this.addedQueryColumnList = [];
        this.removedQueryColumnList = [];
        //queriesByUser = TenantContext.Current.Queries.Where(d => d.UserId == TenantContext.Current.LoggedContactId).ToList();
        this._http.get(ServiceHelper_1.ServiceHelper.GetLogitudeURL() + "api/ngMetaData?tenant=" + SessionInfo_1.SessionInfo.LoggedUserTenant + "&queryid=" + this.QueryId + "&objecttableid=" + this.ObjectTable.Id + "&userid=" + SessionInfo_1.SessionInfo.LoggedUserId)
            .subscribe(function (response) {
            _this.queryColumnsList = response.json();
            // this.queryColumnsList = TenantContext.Current.GeneralContext.QueryColumnPMs.Where(d => d.QueryId == QueryId && ((d.UserId == TenantContext.Current.LoggedContactId && d.Tenant == TenantContext.Current.Id)) && d.DisplayInList).OrderBy(d => d.IndexOrder).ToList();
            _this.queryColumnsList = _this.queryColumnsList.sort(function (a, b) { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1; });
            if (_this.queryColumnsList.length == 0) // Copy query columns to my tenant
             {
                var zeroColumnsList = [];
                _this._http.get(ServiceHelper_1.ServiceHelper.GetLogitudeURL() + "api/ngMetaData?tenant=0&queryid=" + _this.QueryId + "&objecttableid=" + _this.ObjectTable.Id + "&userid=null")
                    .subscribe(function (response) {
                    zeroColumnsList = response.json();
                    zeroColumnsList = zeroColumnsList.sort(function (a, b) { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1; });
                    zeroColumnsList.forEach(function (querycolumn, key) {
                        var newcolumn = new QueryColumnPM_1.QueryColumnPM();
                        newcolumn.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant,
                            newcolumn.UserId = SessionInfo_1.SessionInfo.LoggedUserId,
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
                            newcolumn.QueryObjectTableName = querycolumn.QueryObjectTableName,
                            newcolumn.ObjectFieldFieldLableTextCodeCode = querycolumn.ObjectFieldFieldLableTextCodeCode,
                            // TenantContext.Current.GeneralContext.QueryColumnPMs.Add(newcolumn);
                            _this.queryColumnsList.push(newcolumn);
                        _this.addedQueryColumnList.push(newcolumn);
                        //copy = true;
                    });
                });
            }
            _this.staticColumnsList = _this.queryColumnsList.filter(function (q) { return q.QueryId == _this.QueryId && ((q.UserId == SessionInfo_1.SessionInfo.LoggedUserId && q.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant)); }).sort(function (a, b) { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1; });
            var listColumns = _this.queryColumnsList.filter(function (q) { return q.QueryId == _this.QueryId && ((q.UserId == SessionInfo_1.SessionInfo.LoggedUserId && q.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant)); }).sort(function (a, b) { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1; });
            _this.unselectedObjectFields = window.ObjectFields.filter(function (a) { return a.ObjectTableName == _this.CurrentObjectTable; }).filter(function (d) { return d.DisplayInList == true && (d.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant || d.Tenant == 0) && ((d.ValidForQuerySection1 == currentQuery.QuerySection || d.ValidForQuerySection2 == currentQuery.QuerySection) || d.IsCustom == true); });
            _this.unselected = [];
            _this.unselectedObjectFields.forEach(function (field, key) {
                var xx = _this.queryColumnsList.filter(function (q) { return q.QueryId == _this.QueryId && q.ObjectFieldId == field.Id && field.FieldName != "TimeFrameFilter"; });
                var yy = _this.unselected.filter(function (q) { return q.Id == field.Id; });
                if (xx.length == 0 && yy.length == 0) {
                    _this.unselected.push(field);
                }
            });
            //this.UnSelectedQueryColumnsList.ItemsSource = unselected.OrderBy(c => c.FieldName);
            _this.OrderedQueryColumnsList = [];
            _this.unSelectedList = _this.unselected.sort(function (a, b) { return (a.FieldName.toLowerCase() === b.FieldName.toLowerCase()) ? 0 : (a.FieldName.toLowerCase() < b.FieldName.toLowerCase()) ? -1 : 1; });
            _this.queryColumnsList.forEach(function (qc, key) {
                var CurColumn = _this.OrderedQueryColumnsList.filter(function (a) { return a.ObjectFieldName == qc.ObjectFieldName; });
                if (CurColumn == null || CurColumn.length == 0) {
                    _this.OrderedQueryColumnsList.push(new QueryColumnDetails(qc));
                }
            });
            _this.OrderedQueryColumnsList = _this.OrderedQueryColumnsList.sort(function (a, b) { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1; });
            _this.CD.detectChanges();
            //SelectedQueryColumnsList.ItemsSource = OrderedQueryColumnsList;
            _this.Fixedunselected = _this.unSelectedList;
            _this.IsEnabled = true;
            _this.onUnSelectedDataLoadedEvent.emit(_this.SelectedItem);
            _this.onSelectedDataLoadedEvent.emit(_this.FieldSelectedItem);
        });
    };
    Object.defineProperty(QueryColumnsEditComponent.prototype, "SearchText", {
        get: function () { return this.searchText; },
        set: function (newValue) {
            this.searchText = newValue;
            if (newValue != null && newValue != "") {
                this.unSelectedList = this.Fixedunselected.filter(function (f) { return TextCodeTranslator_1.TextCodeTranslator.Translate(f.FullNameTextCodeCode).toLowerCase().indexOf(newValue.toLowerCase()) > -1; });
            }
            else {
                this.unSelectedList = this.Fixedunselected;
            }
            this.onUnselectedDataSourceChangedEvent.emit(this.unSelectedList);
            //this.onUnSelectedDataLoadedEvent.emit(this.SelectedItem);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryColumnsEditComponent.prototype, "SelectedItem", {
        get: function () { return this.selectedItem; },
        set: function (newValue) {
            this.selectedItem = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryColumnsEditComponent.prototype, "FieldSelectedItem", {
        get: function () { return this.fieldSelectedItem; },
        set: function (newValue) {
            this.fieldSelectedItem = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryColumnsEditComponent.prototype, "IsbtnAddEnabled", {
        get: function () { return this.isbtnAddEnabled; },
        set: function (newValue) {
            this.isbtnAddEnabled = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryColumnsEditComponent.prototype, "IsbtnRemoveEnabled", {
        get: function () { return this.isbtnRemoveEnabled; },
        set: function (newValue) {
            this.isbtnRemoveEnabled = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryColumnsEditComponent.prototype, "IsbtnUpEnabled", {
        get: function () { return this.isbtnUpEnabled; },
        set: function (newValue) {
            this.isbtnUpEnabled = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryColumnsEditComponent.prototype, "IsbtnDownEnabled", {
        get: function () { return this.isbtnDownEnabled; },
        set: function (newValue) {
            this.isbtnDownEnabled = newValue;
        },
        enumerable: true,
        configurable: true
    });
    QueryColumnsEditComponent.prototype.onSelectedItemChanged = function (item) {
        this.SelectedItem = item;
        this.IsbtnAddEnabled = true;
        this.IsbtnRemoveEnabled = false;
        this.IsbtnUpEnabled = false;
        this.IsbtnDownEnabled = false;
        this.onUnSelectedDataLoadedEvent.emit(null);
    };
    QueryColumnsEditComponent.prototype.onFieldSelectedItemChanged = function (item) {
        this.FieldSelectedItem = item;
        //if (UnSelectedQueryColumnsList.SelectedItem != null) {
        this.IsbtnAddEnabled = false;
        this.IsbtnRemoveEnabled = true;
        this.IsbtnUpEnabled = true;
        this.IsbtnDownEnabled = true;
        this.onSelectedDataLoadedEvent.emit(null);
        //SelectedQueryColumnsList.SelectedItem = null;
        //}
    };
    QueryColumnsEditComponent.prototype.btnUp_Click = function () {
        var _this = this;
        var item = this.SelectedItem;
        if (item != null) {
            this.HasChanges = true;
            var i = this.OrderedQueryColumnsList.indexOf(item);
            this.ReorderColumnsList();
            var upColumn = this.OrderedQueryColumnsList.filter(function (d) { return d.ObjectFieldId == item.ObjectFieldId && ((d.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant && d.UserId == SessionInfo_1.SessionInfo.LoggedUserId) || d.Tenant == 0) && d.QueryId == _this.QueryId; })[0];
            if (i > 0) {
                this.OrderedQueryColumnsList = this.OrderedQueryColumnsList.filter(function (d) { return d.ObjectFieldId != upColumn.ObjectFieldId; });
                this.OrderedQueryColumnsList.filter(function (o) { return o.IndexOrder == i - 1; })[0].IndexOrder = i;
                upColumn.IndexOrder = i - 1;
                this.OrderedQueryColumnsList.splice(i - 1, 0, upColumn);
                this.onDataSourceChangedEvent.emit(this.OrderedQueryColumnsList);
                //this.ReorderColumnsList();
                this.onSelectedDataLoadedEvent.emit(this.SelectedItem);
            }
        }
    };
    QueryColumnsEditComponent.prototype.btnDown_Click = function () {
        var _this = this;
        var item = this.SelectedItem;
        if (item != null) {
            this.HasChanges = true;
            var i = this.OrderedQueryColumnsList.indexOf(item);
            this.ReorderColumnsList();
            var downColumn = this.OrderedQueryColumnsList.filter(function (d) { return d.ObjectFieldId == item.ObjectFieldId && ((d.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant && d.UserId == SessionInfo_1.SessionInfo.LoggedUserId) || d.Tenant == 0) && d.QueryId == _this.QueryId; })[0];
            if (i < this.OrderedQueryColumnsList.length - 1) {
                this.OrderedQueryColumnsList = this.OrderedQueryColumnsList.filter(function (d) { return d.ObjectFieldId != downColumn.ObjectFieldId; });
                this.OrderedQueryColumnsList.filter(function (o) { return o.IndexOrder == i + 1; })[0].IndexOrder = i;
                downColumn.IndexOrder = i + 1;
                this.OrderedQueryColumnsList.splice(i + 1, 0, downColumn);
                this.onDataSourceChangedEvent.emit(this.OrderedQueryColumnsList);
                //this.ReorderColumnsList();
                this.onSelectedDataLoadedEvent.emit(this.SelectedItem);
            }
        }
    };
    QueryColumnsEditComponent.prototype.btnAdd_Click = function () {
        var _this = this;
        this.HasChanges = true;
        if (this.FieldSelectedItem != null) {
            var field = this.FieldSelectedItem;
            var queryColumn = this.unSelectedList.filter(function (a) { return a.QueryId == _this.QueryId && a.FieldName == field.FieldName; })[0];
            if (queryColumn) {
                this.removedQueryColumnList = this.removedQueryColumnList.filter(function (a) { return a.FieldName != queryColumn.FieldName; });
            }
            if (queryColumn) {
                if (queryColumn.Id) {
                    var newQueryColumn = new QueryColumnPM_1.QueryColumnPM();
                    newQueryColumn.QueryId = this.QueryId,
                        newQueryColumn.ObjectFieldId = field.Id,
                        //ObjectField = field,
                        newQueryColumn.ObjectFieldName = field.FieldName,
                        newQueryColumn.ObjectFieldFieldLableTextCodeDefaultText = field.FullNameTextCodeDefaultText,
                        newQueryColumn.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant,
                        newQueryColumn.IndexOrder = (this.OrderedQueryColumnsList.length > 0 ? this.OrderedQueryColumnsList[this.OrderedQueryColumnsList.length - 1].IndexOrder + 1 : 0),
                        newQueryColumn.ColumnWidth = 100,
                        newQueryColumn.ConverterName = field.ConverterName,
                        newQueryColumn.DataTemplateName = field.DataTemplateName,
                        newQueryColumn.ObjectFieldListLabelTextCodeCode = field.ListTextCodeCode,
                        newQueryColumn.DisplayInList = true,
                        newQueryColumn.UserId = SessionInfo_1.SessionInfo.LoggedUserId,
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
                var newQueryColumn = new QueryColumnPM_1.QueryColumnPM();
                newQueryColumn.QueryId = this.QueryId,
                    newQueryColumn.ObjectFieldId = field.Id,
                    //  ObjectField = field,
                    newQueryColumn.ObjectFieldName = field.FieldName,
                    newQueryColumn.ObjectFieldFieldLableTextCodeDefaultText = field.FullNameTextCodeDefaultText,
                    newQueryColumn.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant,
                    newQueryColumn.IndexOrder = (this.OrderedQueryColumnsList.length > 0 ? this.OrderedQueryColumnsList[this.OrderedQueryColumnsList.length - 1].IndexOrder + 1 : 0),
                    newQueryColumn.ColumnWidth = 100,
                    newQueryColumn.ConverterName = field.ConverterName,
                    newQueryColumn.DataTemplateName = field.DataTemplateName,
                    newQueryColumn.ObjectFieldListLabelTextCodeCode = field.ListTextCodeCode,
                    newQueryColumn.DisplayInList = true,
                    newQueryColumn.UserId = SessionInfo_1.SessionInfo.LoggedUserId,
                    newQueryColumn.ObjectFieldFieldLableTextCodeCode = field.FullNameTextCodeCode,
                    this.OrderedQueryColumnsList.push(new QueryColumnDetails(newQueryColumn));
                this.addedQueryColumnList.push(newQueryColumn);
            }
            this.unSelectedList = this.unSelectedList.filter(function (a) { return a.FieldName != field.FieldName; });
            this.Fixedunselected = this.Fixedunselected.filter(function (a) { return a.FieldName != field.FieldName; });
            this.IsbtnAddEnabled = false;
            this.CD.detectChanges();
            if (this.SearchText != null && this.SearchText != "") {
                this.unSelectedList = this.unSelectedList.filter(function (f) { return f.FullNameTextCodeDefaultText.toLowerCase().indexOf(_this.SearchText.toLowerCase()) >= 0 || ((f.FullNameTextCodeLocalDefaultText != null && f.FullNameTextCodeLocalDefaultText != "") && f.FullNameTextCodeLocalDefaultText.toLowerCase().indexOf(_this.SearchText.toLowerCase())); });
            }
            this.onUnSelectedDataLoadedEvent.emit(this.SelectedItem);
        }
        //this.ReorderColumnsList();
        this.onSelectedDataLoadedEvent.emit(this.FieldSelectedItem);
        this.FieldSelectedItem = null;
        this.SelectedItem = null;
    };
    QueryColumnsEditComponent.prototype.btnRemove_Click = function () {
        var _this = this;
        this.HasChanges = true;
        if (this.SelectedItem) {
            var queryColumn = this.SelectedItem;
            var objectField = window.ObjectFields.filter(function (a) { return a.Id == queryColumn.ObjectFieldId; })[0];
            this.unSelectedList.push(objectField);
            //-----
            this.OrderedQueryColumnsList = this.OrderedQueryColumnsList.filter(function (a) { return a.ObjectFieldId != queryColumn.ObjectFieldId; });
            var pm = this.queryColumnsList.filter(function (a) { return a.ObjectFieldId == queryColumn.ObjectFieldId && a.QueryId == _this.QueryId; })[0];
            if (pm != null) {
                this.removedQueryColumnList.push(pm);
            }
            var queryColumn2 = this.addedQueryColumnList.filter(function (a) { return a.ObjectFieldId == queryColumn.ObjectFieldId && a.QueryId == _this.QueryId; })[0];
            if (queryColumn2 != null) {
                this.addedQueryColumnList = this.addedQueryColumnList.filter(function (a) { return a.ObjectFieldId != queryColumn2.ObjectFieldId; });
            }
            this.OrderedQueryColumnsList.forEach(function (column, key) {
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
    };
    QueryColumnsEditComponent.prototype.ReorderColumnsList = function () {
        var queryColumnList = this.OrderedQueryColumnsList.sort(function (a, b) { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1; });
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
    };
    QueryColumnsEditComponent.prototype.SaveChanges = function () {
        var _this = this;
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
        //        this.myQueryColumnsPMService.insert(queryColumn).subscribe(myResult => {
        //            this.CurrentSession.CloseCurrentWindow();
        //        });
        //    }
        //});
        var Length = 0;
        this.OrderedQueryColumnsList.forEach(function (queryColumn, key) {
            var temp = _this.queryColumnsList.filter(function (a) { return a.QueryId == _this.QueryId && a.ObjectFieldId == queryColumn.ObjectFieldId && a.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant && a.UserId == SessionInfo_1.SessionInfo.LoggedUserId; });
            if (temp.length == 0) {
                temp = _this.queryColumnsList.filter(function (a) { return a.QueryId == _this.QueryId && a.ObjectFieldId == queryColumn.ObjectFieldId && a.Tenant == 0; });
            }
            if (temp.length == 0) {
                temp = _this.addedQueryColumnList.filter(function (a) { return a.ObjectFieldId == queryColumn.ObjectFieldId; });
            }
            var qc = temp[0];
            if (qc) {
                if (_this.myQueryColumnsPMService == null) {
                    _this.myQueryColumnsPMService = new QueryColumnsPMService_1.QueryColumnsPMService();
                    _this.myQueryColumnsPMService.setServiceArgs(_this.serviceArgs);
                }
                qc.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                qc.IndexOrder = queryColumn.IndexOrder;
                qc.UserId = SessionInfo_1.SessionInfo.LoggedUserId;
                _this.myQueryColumnsPMService.update(qc).subscribe(function (myResult) {
                    Length++;
                    if (Length == _this.OrderedQueryColumnsList.length && _this.removedQueryColumnList.length == 0) {
                        _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                        _this.CurrentSession.CloseCurrentWindow();
                    }
                });
            }
        });
        var removedQueryLength = 0;
        this.removedQueryColumnList.forEach(function (queryColumn, key) {
            var temp = _this.queryColumnsList.filter(function (a) { return a.QueryId == _this.QueryId && a.ObjectFieldId == queryColumn.ObjectFieldId && a.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant && a.UserId == SessionInfo_1.SessionInfo.LoggedUserId; });
            var tempbool = false;
            //if (temp && (temp.length > 0)) {
            //    tempbool = true;
            //}
            //var exists = tempbool;
            //if (exists) {
            if (_this.myQueryColumnsPMService == null) {
                _this.myQueryColumnsPMService = new QueryColumnsPMService_1.QueryColumnsPMService();
                _this.myQueryColumnsPMService.setServiceArgs(_this.serviceArgs);
            }
            _this.myQueryColumnsPMService.delete(queryColumn).subscribe(function (myResult) {
                removedQueryLength = removedQueryLength + 1;
                if (removedQueryLength == _this.removedQueryColumnList.length) {
                    _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    _this.CurrentSession.CloseCurrentWindow();
                }
            });
            //}
        });
        //if (this.addedQueryColumnList.length == 0 && this.removedQueryColumnList.length == 0) {
        //    this.CurrentSession.CloseCurrentWindow();
        //}
    };
    QueryColumnsEditComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], QueryColumnsEditComponent.prototype, "onSelectedDataLoadedEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], QueryColumnsEditComponent.prototype, "onUnSelectedDataLoadedEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], QueryColumnsEditComponent.prototype, "onDataSourceChangedEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], QueryColumnsEditComponent.prototype, "onUnselectedDataSourceChangedEvent", void 0);
    QueryColumnsEditComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'QueryColumnEdit',
            templateUrl: './QueryColumnsEditComponent.html',
            //pipes: [TextCodeTranslationPipe],
            //inputs: ['ObjectTableName', 'event', 'isWindowViewMode', 'isNewViewMode', 'QueryId', 'Filterchangeevent', 'rabaia'],
            providers: [http_1.Http, ServiceArgs_1.ServiceArgs],
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], QueryColumnsEditComponent);
    return QueryColumnsEditComponent;
}());
exports.QueryColumnsEditComponent = QueryColumnsEditComponent;
var QueryColumnDetails = /** @class */ (function () {
    function QueryColumnDetails(qc) {
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
        this.QueryColumnPM = qc;
    }
    Object.defineProperty(QueryColumnDetails.prototype, "Id", {
        get: function () { return this.id; },
        set: function (newValue) { this.id = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryColumnDetails.prototype, "Tenant", {
        get: function () { return this.tenant; },
        set: function (newValue) { this.tenant = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryColumnDetails.prototype, "QueryId", {
        get: function () { return this.queryId; },
        set: function (newValue) { this.queryId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryColumnDetails.prototype, "ObjectFieldId", {
        get: function () { return this.objectFieldId; },
        set: function (newValue) { this.objectFieldId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryColumnDetails.prototype, "IndexOrder", {
        get: function () { return this.indexOrder; },
        set: function (newValue) { this.indexOrder = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryColumnDetails.prototype, "ColumnWidth", {
        get: function () { return this.columnWidth; },
        set: function (newValue) { this.columnWidth = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryColumnDetails.prototype, "ObjectFieldName", {
        get: function () { return this.objectFieldName; },
        set: function (newValue) { this.objectFieldName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryColumnDetails.prototype, "QueryCode", {
        get: function () { return this.queryCode; },
        set: function (newValue) { this.queryCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryColumnDetails.prototype, "DisplayInList", {
        get: function () { return this.displayInList; },
        set: function (newValue) { this.displayInList = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryColumnDetails.prototype, "ObjectFieldDataTypeCode", {
        get: function () { return this.objectFieldDataTypeCode; },
        set: function (newValue) { this.objectFieldDataTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryColumnDetails.prototype, "QueryObjectTableName", {
        get: function () { return this.queryObjectTableName; },
        set: function (newValue) { this.queryObjectTableName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryColumnDetails.prototype, "DataTemplateName", {
        get: function () { return this.dataTemplateName; },
        set: function (newValue) { this.dataTemplateName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryColumnDetails.prototype, "ColumnHeaderTemplateName", {
        get: function () { return this.columnHeaderTemplateName; },
        set: function (newValue) { this.columnHeaderTemplateName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryColumnDetails.prototype, "ConverterName", {
        get: function () { return this.converterName; },
        set: function (newValue) { this.converterName = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryColumnDetails.prototype, "UpdatedByTenant", {
        get: function () { return this.updatedByTenant; },
        set: function (newValue) { this.updatedByTenant = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryColumnDetails.prototype, "ObjectFieldListLabelTextCodeCode", {
        get: function () { return this.objectFieldListLabelTextCodeCode; },
        set: function (newValue) { this.objectFieldListLabelTextCodeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryColumnDetails.prototype, "ObjectFieldFieldLableTextCodeDefaultText", {
        get: function () { return this.objectFieldFieldLableTextCodeDefaultText; },
        set: function (newValue) { this.objectFieldFieldLableTextCodeDefaultText = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryColumnDetails.prototype, "ObjectFieldFieldLableTextCodeCode", {
        get: function () { return this.objectFieldFieldLableTextCodeCode; },
        set: function (newValue) { this.objectFieldFieldLableTextCodeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryColumnDetails.prototype, "UserId", {
        get: function () { return this.userId; },
        set: function (newValue) { this.userId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryColumnDetails.prototype, "QueryColumnPM", {
        get: function () { return this.queryColumnPM; },
        set: function (newValue) { this.queryColumnPM = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(QueryColumnDetails.prototype, "ObjectFieldFullNameTextCodeCode", {
        get: function () { return this.objectFieldFullNameTextCodeCode; },
        set: function (newValue) { this.objectFieldFullNameTextCodeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    return QueryColumnDetails;
}());
exports.QueryColumnDetails = QueryColumnDetails;
//# sourceMappingURL=QueryColumnsEditComponent.js.map