"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
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
var core_2 = require("@angular/core");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var Tools_1 = require("../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var CustomsRequestsSheetStatusListService_1 = require("../../../Customs/Services/StandardLists/CustomsRequestsSheetStatusListService");
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var ApiQueryFilters_1 = require("../../../Infrastructure/DataContracts/ApiQueryFilters");
var EntityListService_1 = require("../../../Infrastructure/Services/EntityListService");
var EntityArgs_1 = require("../../../Infrastructure/DataContracts/EntityArgs");
var Guid_1 = require("../../../Infrastructure/Utilities/Guid");
//////////////////////////////////////////////////////////////////
var CustomsRequestsSheetsComponent = /** @class */ (function (_super) {
    __extends(CustomsRequestsSheetsComponent, _super);
    function CustomsRequestsSheetsComponent(entityArgs, _CD) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this._CD = _CD;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this._Id = Guid_1.Guid.newGuid();
        //IsDCA?: boolean;
        _this._SelectedDCAValue = 'ALL'; //'ALL';//DCA//!DCA
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.CustomsRequestsSheet";
        _this.columns = null;
        _this._MySearchText = "Search";
        _this._TranslationLoaded = false;
        _this.FiltersSectionVisibility = true;
        _this.MenuHeaderchangeevent = new core_1.EventEmitter();
        _this.onQueryChangeEvent = new core_1.EventEmitter();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.DataSource = {
            pageSize: 30,
            rowCount: null,
            //SortData("RequestCreateDate", "Descending", false, false);
            sortingCol: "RequestCreateDate",
            sortingDir: "Descending",
            getRows: function (skip, take, sortingCol, sortingDir, getCount, searchFields, filters) {
                if (filters === void 0) { filters = null; }
                var tempo = _this.getRows(skip, take, sortingCol, sortingDir, getCount, searchFields, filters);
                return tempo;
            },
        };
        _this.ErrorMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.RequestSheet.O.RequestCreateDate");
        _this.IsSearchButtonEnabled = true;
        _this._CustomsRequestsSheetStatusListService = new CustomsRequestsSheetStatusListService_1.CustomsRequestsSheetStatusListService();
        _this._AllCustomsRequestsSheetStatusListVM = [];
        console.log("12....");
        return _this;
    }
    Object.defineProperty(CustomsRequestsSheetsComponent.prototype, "FromRequestCreateDate", {
        get: function () { return this._FromRequestCreateDate; },
        set: function (val) {
            this._FromRequestCreateDate = val;
            this.DatesValidate("FromDate");
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsRequestsSheetsComponent.prototype, "FromRequestTime", {
        get: function () { return this._FromRequestTime; },
        set: function (val) {
            this._FromRequestTime = val;
            this.DatesValidate("FromDate");
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsRequestsSheetsComponent.prototype, "ToRequestCreateDate", {
        get: function () { return this._ToRequestCreateDate; },
        set: function (val) {
            this._ToRequestCreateDate = val;
            this.DatesValidate("ToDate");
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsRequestsSheetsComponent.prototype, "ToRequestTime", {
        get: function () { return this._ToRequestTime; },
        set: function (val) {
            this._ToRequestTime = val;
            this.DatesValidate("ToDate");
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomsRequestsSheetsComponent.prototype, "AllCRSSChecked", {
        //_stratSearch: boolean = true;
        get: function () { return this._AllCRSSChecked; },
        set: function (value) {
            this._AllCRSSChecked = value;
            if (this._AllCRSSChecked) {
                this._AllCustomsRequestsSheetStatusListVM.forEach(function (VM) {
                    VM.IsChecked = false;
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    ;
    CustomsRequestsSheetsComponent.prototype.SetEntityArgs = function (entityArgs) {
        this.entityArgs = entityArgs;
    };
    CustomsRequestsSheetsComponent.prototype.ngOnInit = function () {
        this.MyRequestOnly = true;
        if (this.entityArgs.ObjectTableName) {
            if (this.entityArgs.ObjectTableName == "Customs.Declaration") {
                this.RefreshButtonVisibility = true; //Visibility.Visible;
                this.CustomFileNo = this.entityArgs.EntityPM.CustomFileNo;
                this.MyRequestOnly = false;
                this.UIProperties.SetEnabled("CustomFileNo", this.ObjectTableName, false);
            }
            else if (this.entityArgs.ObjectTableName == "Customs.Notification") {
                this.CloseButtonVisibility = true;
                ; //Visibility.Visible; ///??????
            }
            else {
                this.FiltersSectionVisibility = false; //Visibility.Collapsed;
                this.RefreshButtonVisibility = true; //Visibility.Visible;
            }
        }
        else {
            this.FromRequestCreateDate = Tools_1.DateTool.AddDays(Tools_1.DateTool.GetCurrentDateAsUtc(), 0);
            this.RefreshButtonVisibility = true; //Visibility.Visible;
        }
        this.InitScreen();
    };
    CustomsRequestsSheetsComponent.prototype.ngOnDestroy = function () {
        console.log("CustomsRequestsSheetsComponent:ngOnDestroy");
        this.entityArgs = null;
        this._CD = null;
    };
    CustomsRequestsSheetsComponent.prototype.InitScreen = function () {
        var _this = this;
        //this.CurrentSession.StartBusyIndicator("");
        this._entityResourceService.getEntityResourceByTableName("Customs.CustomsRequestsSheet", 0).subscribe(function (response) {
            _this._entityResourceService.getEntityResourceByTableName("CommunicationLog", 0).subscribe(function (response) {
                _this._entityResourceService.getEntityResourceByTableName("Customs.Declaration", 0).subscribe(function (response) {
                    //this._entityResourceService.getEntityResourceByTableName("CustomsRequestsSheetStatus", 0).subscribe(response => {
                    if (Tools_1.AppTool.IsNullOrEmpty(_this.Title)) {
                        _this.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.TH.RequestSheet");
                    }
                    _this._entityListService = new EntityListService_1.EntityListService();
                    _this.BuildColumns();
                    //alert(TextCodeTranslator.Translate("Customs.CustomsRequestsSheet.F.RequestDescription"));
                    _this._MySearchText = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Notification.O.Search");
                    _this._CustomsRequestsSheetStatusListService.getAll() /*getAllFromCache()*/.subscribe(function (resCRSSttsList) {
                        //this.CurrentSession.StopBusyIndicator();
                        var listCustomsRequestsSheetStatusList = resCRSSttsList.Result;
                        listCustomsRequestsSheetStatusList.sort(function (a, b) {
                            return (a.LocalName === b.LocalName) ? 0 : (a.LocalName < b.LocalName) ? -1 : 1;
                        }).forEach(function (item) {
                            _this._AllCustomsRequestsSheetStatusListVM.push(new CustomsRequestsSheetStatusListVM(item, _this.entityArgs.ObjectTableName == "Customs.Declaration"));
                        });
                        if (_this.entityArgs.ObjectTableName == "Customs.Declaration") {
                            _this.AllCRSSChecked = true;
                        }
                        _this._TranslationLoaded = true;
                        var isDestroyed = _this._CD['destroyed'];
                        if (!isDestroyed) {
                            _this._CD.detectChanges();
                        }
                        _this.CRSSearch();
                        //this.CurrentSession.StopBusyIndicator();
                    });
                });
            });
        });
    };
    CustomsRequestsSheetsComponent.prototype.ngAfterViewInit = function () {
        //this._CD.detectChanges();
        //this.CRSSearch();
    };
    CustomsRequestsSheetsComponent.prototype.CRSSearch = function () {
        var _this = this;
        this.IsSearchButtonEnabled = false;
        //this.CurrentSession.StartBusyIndicator("");
        setTimeout(function () {
            _this.MenuHeaderchangeevent.emit({ Filters: _this.filterAgrs, IgnoreFilter: false });
        }, 10);
        //this.onQueryChangeEvent.emit({ Filters: new ApiQueryFilters() }); 
    };
    CustomsRequestsSheetsComponent.prototype.BuildColumns = function () {
        this.columns = [];
        this.columns.push({
            FieldName: 'RequestDescription',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsRequestsSheet.F.RequestDescription"),
            Styles: { width: '220px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'RequestDescription'
        });
        this.columns.push({
            FieldName: 'CustomFileNo',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsRequestsSheet.F.CustomFileNo"),
            Styles: { width: '100px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'CustomFileNo'
        });
        this.columns.push({
            FieldName: 'RequestStatusName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsRequestsSheet.F.RequestStatusName"),
            Styles: { width: '100px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'RequestStatusName'
        });
        this.columns.push({
            FieldName: 'RequestCreateDate',
            DataTypeCode: 'Date',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsRequestsSheet.F.RequestCreateDate"),
            Styles: { width: '140px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CustomsRequestsSheetsListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsRequestsSheetsListTemplate',
            ServerSideSortable: true,
            SortByName: 'RequestCreateDate'
        });
        this.columns.push({
            FieldName: 'RequestOwnerName',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsRequestsSheet.F.RequestOwnerName"),
            Styles: { width: '140px' },
            IsCustomTemplate: true,
            ServerSideSortable: true,
            SortByName: 'RequestOwnerName'
        });
        this.columns.push({
            FieldName: 'IsDCA',
            DataTypeCode: 'boolean',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsRequestsSheet.F.IsDCA"),
            Styles: { width: '50px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CustomsRequestsSheetsListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsRequestsSheetsListTemplate',
            ServerSideSortable: true,
            SortByName: 'IsDCA'
        });
        this.columns.push({
            FieldName: 'IsRestored',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsRequestsSheet.F.IsRestored"),
            Styles: { width: '60px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CustomsRequestsSheetsListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsRequestsSheetsListTemplate',
            ServerSideSortable: true,
            SortByName: 'IsRestored'
        });
        this.columns.push({
            FieldName: 'CancleRequest',
            DataTypeCode: 'String',
            Display: TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.RequestSheet.O.Cancelled"),
            Styles: { width: '80px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CustomsRequestsSheetsListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsRequestsSheetsListTemplate',
        });
        this.columns.push({
            FieldName: 'ShowFormatedResponse',
            DataTypeCode: 'String',
            Display: '',
            Styles: { width: '80px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CustomsRequestsSheetsListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsRequestsSheetsListTemplate',
        });
        this.columns.push({
            FieldName: 'ShowLog',
            DataTypeCode: 'String',
            Display: '',
            Styles: { width: '80px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CustomsRequestsSheetsListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsRequestsSheetsListTemplate',
        });
        this.columns.push({
            FieldName: 'ReAnalyze',
            DataTypeCode: 'String',
            Display: '',
            Styles: { width: '80px' },
            IsCustomTemplate: true,
            HtmlListComponentName: 'CustomsRequestsSheetsListTemplate',
            HtmlListComponentUrl: './CustomsModules/CustomsListTemplates/Components/CustomsRequestsSheetsListTemplate',
        });
    };
    CustomsRequestsSheetsComponent.prototype.OnFirstRowSelected = function ($event) {
        // alert("OnFirstRowSelected()" + $event);
    };
    CustomsRequestsSheetsComponent.prototype.OnrowSelectedEvent = function ($event) {
        // alert("OnrowSelectedEvent()" + $event);
    };
    CustomsRequestsSheetsComponent.prototype.OnColumnResisedevent = function ($event) {
        //  alert("ViewInitCompleted()" + $event);
    };
    CustomsRequestsSheetsComponent.prototype.ViewInitCompleted = function ($event) {
        //alert("ViewInitCompleted()" + $event);
        //this.SelectedRow = this.ItemsSource.Collection[0];
        //this.OnRowSelected(this.SelectedRow);
    };
    CustomsRequestsSheetsComponent.prototype.getRows = function (skip, take, sortingCol, sortingDir, getCount, searchfields, filters) {
        var _this = this;
        if (filters === void 0) { filters = null; }
        if (filters == null) {
            filters = new ApiQueryFilters_1.ApiQueryFilters();
        }
        filters.PageSize = take;
        filters.PageIndex = skip;
        filters.GetAll = false;
        filters.GetCount = true;
        filters.SortBy = sortingCol; //"RequestCreateDate";
        filters.SortDirection = sortingDir; //"Descending";
        if (Tools_1.AppTool.IsNullOrEmpty(filters.SortBy)) {
            filters.SortBy = "RequestCreateDate";
        }
        if (Tools_1.AppTool.IsNullOrEmpty(filters.SortDirection)) {
            filters.SortDirection = "Descending";
        }
        filters.addAdditionalFilter("Tenant", SessionLocator_1.SessionLocator.Tenant, null, null, "Equals", false, false, false, "number");
        var objectTableName = "";
        var objectTableId1 = "";
        if (this.entityArgs) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.entityArgs.ObjectTableName)) {
                objectTableName = this.entityArgs.ObjectTableName;
                var objectTablePM = //window.ObjectTables.filter(d => d.Id == ObjectTableId)[0];
                 window.ObjectTables.filter(function (t) { return t.Name == objectTableName; })[0];
                objectTableId1 = objectTablePM.Id;
            }
        }
        //if (!AppTool.IsNullOrEmpty(objectTableName)) {
        if (objectTableName === "Customs.Notification") {
            //////never tested !!!!!!!- copy from silverlight
            filters.addAdditionalFilter("Id", this.entityArgs.EntityPM.Id, null, null, "Equals", false, false, false, "string");
        }
        else {
            if (!Tools_1.AppTool.IsNullOrEmpty(objectTableName) && objectTableName != "Customs.Declaration") {
                //////never tested !!!!!!!- copy from silverlight
                //filters.addAdditionalFilter("ObjectTableId1", objectTableId, null, null, "Equals", false, false, false, "string");
                filters.addAdditionalFilter("ObjectTableId1", objectTableId1, null, null, "Equals", false, false, false, "string");
                var EntityId1 = this.entityArgs.EntityPM.Id;
                if (Tools_1.AppTool.IsNullOrEmpty(EntityId1)) {
                    EntityId1 = "new Entity do not get any rows !!!!";
                }
                //filters.addAdditionalFilter("EntityId1", this.entityArgs.EntityPM.Id, null, null, "Equals", false, false, false, "string");
                filters.addAdditionalFilter("EntityId1", EntityId1, null, null, "Equals", false, false, false, "string");
            }
            else {
                if (this.FromDateTime != null || this.ToDateTime != null) { // for Region
                    filters.addAdditionalFilter("RequestCreateDate", this.FromDateTime, this.ToDateTime, null, "Between", false, false, false, "DateTime");
                }
                if (this.MyRequestOnly) {
                    filters.addAdditionalFilter("RequestOwnerId", SessionLocator_1.SessionLocator.LoggedUserId, null, null, "Equals", false, false, false, "string");
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(this.CorrelationId)) {
                    filters.addAdditionalFilter("CorrelationId", this.CorrelationId, null, null, "Equals", false, false, false, "string");
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(this.CustomFileNo)) {
                    filters.addAdditionalFilter("CustomFileNo", this.CustomFileNo, null, null, "Equals", false, false, false, "string");
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(this.InterfaceTypeCode)) {
                    filters.addAdditionalFilter("InterfaceTypeCode", this.InterfaceTypeCode, null, null, "Equals", false, false, false, "string");
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(this.SearchFields)) {
                    filters.addAdditionalFilter("SearchFields", this.SearchFields, null, null, "Contains", false, false, false, "string");
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(this.EntityReference)) {
                    filters.addAdditionalFilter("EntityReference", this.EntityReference, null, null, "Equals", false, false, false, "string");
                }
                if (Tools_1.AppTool.IsNullOrEmpty(objectTableName) || objectTableName == "Customs.Declaration") {
                    this.GetRequestStatusString(filters);
                }
                if (this.IsRestored) {
                    filters.addAdditionalFilter("IsRestored", this.IsRestored, null, null, "Equals", false, false, false, "boolean");
                }
                //_SelectedDCAValue: string = 'ALL';//'ALL';//DCA//!DCA
                if (this._SelectedDCAValue != "ALL") {
                    filters.addAdditionalFilter("IsDCA", this._SelectedDCAValue === "DCA", null, null, "Equals", false, false, false, "boolean");
                }
            }
        }
        var myout = this._entityListService
            .getExtendedByFilters("Customs.CustomsRequestsSheet", filters);
        myout.then(function (res) {
            _this.IsSearchButtonEnabled = true;
            //this.CurrentSession.StopBusyIndicator();
        });
        return myout;
    };
    CustomsRequestsSheetsComponent.prototype.GetRequestStatusString = function (filters) {
        var RequestStatusString = "";
        if (this.AllCRSSChecked)
            return;
        this._AllCustomsRequestsSheetStatusListVM.forEach(function (requestStatus) {
            if (requestStatus.IsChecked) {
                if (!Tools_1.AppTool.IsNullOrEmpty(RequestStatusString)) {
                    RequestStatusString = RequestStatusString + ",";
                }
                RequestStatusString = RequestStatusString + requestStatus.MyItem.Code;
            }
        });
        if (!Tools_1.AppTool.IsNullOrEmpty(RequestStatusString)) {
            //.SetFilter("RequestStatusCode", RequestStatusString, false, "InListExact", null, true);
            filters.addAdditionalFilter("RequestStatusCode", RequestStatusString, null, null, "InListExact", false, false, false, "string");
        }
    };
    CustomsRequestsSheetsComponent.prototype.DatesValidate = function (mydate) {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.FromRequestCreateDate)) {
            this.FromDateTime = new Date(this.FromRequestCreateDate.toString());
            this.FromDateTime = new Date(this.FromDateTime.getUTCFullYear(), this.FromDateTime.getUTCMonth(), this.FromDateTime.getUTCDate(), 0, 0, 0);
        }
        if (this.FromRequestTime) {
            //this.FromDateTime = DateTool.AddHour(this.FromDateTime, this.FromRequestTime.getHours())
            //this.FromDateTime = DateTool.AddMinute(this.FromDateTime, this.FromRequestTime.getMinutes())
            //this.FromDateTime = DateTool.AddSecond(this.FromDateTime, this.FromRequestTime.getSeconds())
            this.FromDateTime = new Date(this.FromDateTime.getUTCFullYear(), this.FromDateTime.getUTCMonth(), this.FromDateTime.getUTCDate() + 1, this.FromRequestTime.getUTCHours(), this.FromRequestTime.getUTCMinutes(), this.FromRequestTime.getUTCSeconds());
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ToRequestCreateDate)) {
            this.ToDateTime = new Date(this.ToRequestCreateDate.toString());
            this.ToDateTime = new Date(this.ToDateTime.getUTCFullYear(), this.ToDateTime.getUTCMonth(), this.ToDateTime.getUTCDate(), 23, 59, 59);
        }
        if (this.ToRequestTime) {
            //this.ToDateTime = DateTool.AddHour(this.ToDateTime, this.ToRequestTime.getHours())
            //this.ToDateTime = DateTool.AddMinute(this.ToDateTime, this.ToRequestTime.getMinutes())
            //this.ToDateTime = DateTool.AddSecond(this.ToDateTime, this.ToRequestTime.getSeconds())
            this.ToDateTime = new Date(this.ToDateTime.getUTCFullYear(), this.ToDateTime.getUTCMonth(), this.ToDateTime.getUTCDate(), this.ToRequestTime.getUTCHours(), this.ToRequestTime.getUTCMinutes(), this.ToRequestTime.getUTCSeconds());
        }
        if (Tools_1.DateTool.IsNullOrMinDateTime(this.FromDateTime) && Tools_1.DateTool.IsNullOrMinDateTime(this.ToDateTime)) {
            this.DateOk();
            return;
        }
        if (!Tools_1.DateTool.IsNullOrMinDateTime(this.FromDateTime) && Tools_1.DateTool.IsNullOrMinDateTime(this.ToDateTime)) {
            this.DateOk();
            return;
        }
        if (Tools_1.DateTool.IsNullOrMinDateTime(this.FromDateTime) && !Tools_1.DateTool.IsNullOrMinDateTime(this.ToDateTime)) {
            this.DateOk();
            return;
        }
        if (this.ToDateTime.valueOf() <= this.FromDateTime.valueOf()) {
            if (mydate == "FromDate") {
                //this.UIProperties.SetValidity("CustomFileNo", this.ObjectTableName, true, "");
                this.UIProperties.SetValidity("FromRequestCreateDate", "Customs.CustomsRequestsSheet", false, this.ErrorMessage);
                this.IsSearchButtonEnabled = false;
            }
            else if (mydate == "ToDate") {
                this.UIProperties.SetValidity("ToRequestCreateDate", "Customs.CustomsRequestsSheet", false, this.ErrorMessage);
                this.IsSearchButtonEnabled = false;
            }
        }
        else {
            this.DateOk();
        }
    };
    CustomsRequestsSheetsComponent.prototype.DateOk = function () {
        this.UIProperties.SetValidity("FromRequestCreateDate", "Customs.CustomsRequestsSheet", true, this.ErrorMessage);
        this.UIProperties.SetValidity("ToRequestCreateDate", "Customs.CustomsRequestsSheet", true, this.ErrorMessage);
        this.IsSearchButtonEnabled = true;
    };
    CustomsRequestsSheetsComponent.prototype.RefreshButtonClicked = function () {
        this.CRSSearch();
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], CustomsRequestsSheetsComponent.prototype, "MenuHeaderchangeevent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], CustomsRequestsSheetsComponent.prototype, "onQueryChangeEvent", void 0);
    CustomsRequestsSheetsComponent = __decorate([
        core_1.Component({
            selector: 'CustomsRequestsSheetsComponent',
            moduleId: module.id,
            templateUrl: './CustomsRequestsSheetsComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, core_2.ChangeDetectorRef])
    ], CustomsRequestsSheetsComponent);
    return CustomsRequestsSheetsComponent;
}(BaseComponent_1.BaseComponent));
exports.CustomsRequestsSheetsComponent = CustomsRequestsSheetsComponent;
////////////////////////////////////////
var CustomsRequestsSheetStatusListVM = /** @class */ (function () {
    function CustomsRequestsSheetStatusListVM(MyItem, isdeclaration) {
        this.MyItem = MyItem;
        var Code = MyItem.Code;
        if (Code == "1" || Code == "2" || Code == "3" || Code == "4" || Code == "5" || Code == "21" || Code == "99") {
            this.IsChecked = true;
        }
        if ( //declarationPM != null
        isdeclaration
            && Code != "99") {
            this.IsChecked = true;
        }
    }
    return CustomsRequestsSheetStatusListVM;
}());
exports.CustomsRequestsSheetStatusListVM = CustomsRequestsSheetStatusListVM;
//////////////////////////////////////////////////
//# sourceMappingURL=CustomsRequestsSheetsComponent.js.map