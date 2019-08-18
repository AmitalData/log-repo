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
var TextCodeTranslator_1 = require("../../../Infrastructure/Utilities/TextCodeTranslator");
var AdvancedQueryFilterPM_1 = require("../../../Infrastructure/EntityPMs/AdvancedQueryFilterPM");
var SessionInfo_1 = require("../../../Infrastructure/Utilities/SessionInfo");
var AdvancedQueryFiltersPMService_1 = require("../../../Infrastructure/Services/StandardPMs/AdvancedQueryFiltersPMService");
var QueriesPMService_1 = require("../../../Infrastructure/Services/StandardPMs/QueriesPMService");
var TextCodePMService_1 = require("../../../Infrastructure/Services/StandardPMs/TextCodePMService");
var GeneralEntitiesService_1 = require("../../../Infrastructure/Services/StandardPMs/GeneralEntitiesService");
var http_1 = require("@angular/http");
var ServiceArgs_1 = require("../../../Infrastructure/DataContracts/ServiceArgs");
var ObjectFieldPM_1 = require("../../../Infrastructure/EntityPMs/ObjectFieldPM");
var QueryPM_1 = require("../../../Infrastructure/EntityPMs/QueryPM");
var forms_1 = require("@angular/forms");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var QueryColumnsEditComponent_1 = require("../../../Infrastructure/Components/QueryColumnsComponents/QueryColumnsEditComponent");
var QueryColumnPM_1 = require("../../../Infrastructure/EntityPMs/QueryColumnPM");
var FilterField_1 = require("../../../Infrastructure/Components/LogitudeComponents/QueryListComponent/FilterField");
var GeneralEntitiesArgs_1 = require("../../../Infrastructure/DataContracts/GeneralEntitiesArgs");
var Tools_1 = require("../../../Infrastructure/Tools");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var CachedDataManager_1 = require("../../../Infrastructure/Utilities/CachedDataManager");
var ObjectsLocator_1 = require("../../Locators/ObjectsLocator");
var ServiceLocator_1 = require("../../Locators/ServiceLocator");
var CodeNameClass_1 = require("../../DataContracts/CodeNameClass");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var ConfirmWindow_1 = require("../../../Controls/Windows/ConfirmWindow");
var ChooseUserComponent_1 = require("../../../Infrastructure/Components/NewViewComponent/ChooseUserComponent");
var FeatureLocator_1 = require("../../Utilities/FeatureLocator");
var ApiQueryFilters_1 = require("../../DataContracts/ApiQueryFilters");
var UserListService_1 = require("../../../Common/Services/StandardLists/UserListService");
var NewViewComponent = /** @class */ (function () {
    function NewViewComponent(fb, CD) {
        this.CD = CD;
        this.EntityPM = null;
        this.RTL = ObjectsLocator_1.ObjectsLocator.GlobalSetting == undefined ? false : (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == 'rtl' ? true : false);
        this.onDataSourceChangedEvent = new core_1.EventEmitter();
        this.onUnSelectedDataLoadedEvent = new core_1.EventEmitter();
        this.onSelectedDataLoadedEvent = new core_1.EventEmitter();
        this.onUnSelectedDataSourceChangedEvent = new core_1.EventEmitter();
        this.IsNew = true;
        this.SpotlightFeatureEnabled = false;
        this.ValidationErrorsList = [];
        this.CreateBtnText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Create");
        this.BooleanValues = ["True", "False", "No Filter"];
        this.ShareTabIsVisible = false;
        this.IsSharedByMessageVisible = false;
        this.IsSaveButtonEnabled = false;
        this.IsSharedByVisible = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.myUsersList = [];
        this.IsChooseUsersVisible = false;
        this.SharedWithUsersItemsSource = [];
        this.isbtnAddEnabled = true;
        this.isbtnRemoveEnabled = true;
        this.isbtnUpEnabled = true;
        this.isbtnDownEnabled = true;
        this.timeFilterFieldsClass = new FilterField_1.FilterFieldsClass(false, this, this.pubSubAdvanceQueryFiltersService);
        this.serviceArgs = new ServiceArgs_1.ServiceArgs();
        this.serviceArgs.http = ServiceHelper_1.ServiceHelper.Http;
        this._http = ServiceHelper_1.ServiceHelper.Http;
        this.removedQueryFilters = [];
        if (this.GeneralEntitiesArgs == null) {
            this.GeneralEntitiesArgs = new GeneralEntitiesArgs_1.GeneralEntitiesArgs();
            this.GeneralEntitiesArgs.AdvancedQueryFilterPMs = [];
            this.GeneralEntitiesArgs.QueryColumnsPMs = [];
        }
        else {
            if (this.GeneralEntitiesArgs.AdvancedQueryFilterPMs == null) {
                this.GeneralEntitiesArgs.AdvancedQueryFilterPMs = [];
            }
            if (this.GeneralEntitiesArgs.QueryColumnsPMs == null) {
                this.GeneralEntitiesArgs.QueryColumnsPMs = [];
            }
        }
        if (this.CurrentSession == null) {
            this.SearchFieldsId = "SearchFields_-1_-1";
            this.FiltersSearchFieldsId = "FiltersSearchFieldsId_-1_-1";
        }
        else {
            this.SearchFieldsId = "NewViewSearchFields_" + this.CurrentSession.GetNewId("NewViewSearchFields");
            this.FiltersSearchFieldsId = "NewViewFiltersSearchFieldsId_" + this.CurrentSession.GetNewId("NewViewFiltersSearchFieldsId");
        }
        this.SelectedTabCode = "COL";
        this.myForm = fb.group({
        //'ShipperName': ['', Validators.required]
        });
    }
    NewViewComponent.prototype.SetWindowArgs = function (args) {
        var _this = this;
        this.IsNew = args.IsNew;
        this.pubSubAdvanceQueryFiltersService = args.pubSubAdvanceQueryFiltersService;
        this.QueryId = args.queryId;
        this.CurrentObjectTable = args.currentObjectTable;
        this.IsEnabled = false;
        this.ObjectTable = window.ObjectTables.filter(function (d) { return d.Name == args.currentObjectTable; })[0];
        this.ObjectTableId = this.ObjectTable.Id;
        this.ObjectFields = window.ObjectFields.filter(function (d) { return d.ObjectTableId === _this.ObjectTableId && d.CanFilter === true; });
        this.filterFields = new FilterField_1.FilterFieldsClass(true, this, this.pubSubAdvanceQueryFiltersService);
        this.NEWallFilterFieldsClass = new FilterField_1.FilterFieldsClass(true, this, this.pubSubAdvanceQueryFiltersService);
        this.constantFilterFields = new FilterField_1.FilterFieldsClass(true, this, this.pubSubAdvanceQueryFiltersService);
        this.LoadUsers();
        if (!this.IsNew) {
            this.QueryName = args.QueryName;
            this.CreateBtnText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Save");
            //this.LoadQueryPM();
        }
        else {
            this.IsSaveButtonEnabled = true;
            this.CreateBtnText = TextCodeTranslator_1.TextCodeTranslator.Translate("General.B.Create");
            this.EntityPM = new QueryPM_1.QueryPM();
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("User", "User.Feature.ViewsSharing")) {
            this.ShareTabIsVisible = true;
        }
        this.FillShareValuesList();
        this.SetSelectedSharedValue();
        this.Run();
    };
    NewViewComponent.prototype.LoadUsers = function () {
        var _this = this;
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.SortBy = "EnglishName";
        filters.SortDirection = "Ascending";
        filters.PageIndex = 0;
        filters.PageSize = 100;
        filters.Tenant = SessionLocator_1.SessionLocator.Tenant;
        filters.addAdditionalFilter("InActive", false, null, null, "Equals", false, false, false, "boolean");
        var userService = new UserListService_1.UserListService();
        userService.getByFilters(filters).subscribe(function (res) {
            var pmResponse = res;
            if (!pmResponse.HasError) {
                _this.myUsersList = pmResponse.Result;
                if (!_this.IsNew) {
                    _this.LoadQueryPM();
                }
            }
        });
    };
    NewViewComponent.prototype.LoadQueryPM = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        var myService = new QueriesPMService_1.QueriesPMService();
        myService.setServiceArgs(this.serviceArgs);
        myService.get(this.QueryId).subscribe(function (myResult) {
            var myResponse = myResult;
            if (!myResponse.HasError) {
                _this.EntityPM = myResponse.Result;
                if (_this.EntityPM) {
                    _this.ShareWithUsersCount = _this.EntityPM.SharedUserQueries.length;
                    _this.SharedByUserName = _this.EntityPM.SharedByUserName;
                    _this.SharedByUserEmail = _this.EntityPM.SharedByUserEmail;
                    _this.FillSharedWithUsersItemsSource();
                    _this.SetSelectedSharedValue();
                    _this.CheckEditSharedViewsFeature();
                    if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.SharedByUserId) && _this.EntityPM.SharedByUserId != SessionLocator_1.SessionLocator.LoggedUserId) {
                        _this.IsSharedByVisible = true;
                    }
                    if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "CSPV") && (_this.EntityPM.ObjectTableName == "Shipment" || _this.EntityPM.ObjectTableName == "Master")) {
                        _this.SpotlightFeatureEnabled = true;
                    }
                    if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("ARInvoice", "SSPV") && _this.EntityPM.ObjectTableName == "ARInvoice") {
                        _this.SpotlightFeatureEnabled = true;
                    }
                    _this.ShowInSpotLight = _this.EntityPM.SpotlightModeActivated;
                }
                else {
                    _this.ValidationErrorsList.push("This View was deleted");
                }
                _this.CurrentSession.StopBusyIndicator();
            }
            else {
                //show error
                _this.CurrentSession.StopBusyIndicator();
            }
        });
    };
    NewViewComponent.prototype.CheckEditSharedViewsFeature = function (type) {
        if (type === void 0) { type = null; }
        var isEditEnabled = true;
        var isUpDownEnabled = false;
        var isAddEnabled = false;
        var isRemoveEnabled = false;
        if (this.EntityPM.SharedWithAll || this.EntityPM.SharedWithSpecificUsers) {
            if (this.EntityPM.SharedByUserId != SessionLocator_1.SessionLocator.LoggedUserId) {
                if (!FeatureLocator_1.FeatureLocator.HasFeaturePermession("User", "User.Feature.EditSharedViews")) {
                    isEditEnabled = false;
                }
            }
        }
        if (isEditEnabled) {
            if (type == "Selected") {
                isAddEnabled = false;
                isRemoveEnabled = true;
                isUpDownEnabled = true;
            }
            else if (type == "Available") {
                isAddEnabled = true;
                isRemoveEnabled = false;
                isUpDownEnabled = false;
            }
        }
        this.IsSharedByMessageVisible = !isEditEnabled;
        this.IsSaveButtonEnabled = isEditEnabled;
        this.IsbtnUpEnabled = isUpDownEnabled;
        this.IsbtnDownEnabled = isUpDownEnabled;
        this.IsbtnAddEnabled = isAddEnabled;
        this.IsbtnRemoveEnabled = isRemoveEnabled;
    };
    NewViewComponent.prototype.ClearPlaceHolder = function () {
        var temp = document.getElementById(this.SearchFieldsId);
        temp.placeholder = "";
        temp.style.background = "rgba(0, 0, 0, 0)";
    };
    NewViewComponent.prototype.FillPlaceHolder = function () {
        var temp = document.getElementById(this.SearchFieldsId);
        temp.placeholder = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.Search");
        temp.style.background = "url(Images/Search.png) no-repeat scroll";
        temp.style.backgroundPosition = "right center";
        temp.style.paddingRight = "30px";
    };
    NewViewComponent.prototype.ClearFiltersPlaceHolder = function () {
        var temp = document.getElementById(this.FiltersSearchFieldsId);
        temp.placeholder = "";
        temp.style.background = "rgba(0, 0, 0, 0)";
    };
    NewViewComponent.prototype.FillFiltersPlaceHolder = function () {
        var temp = document.getElementById(this.FiltersSearchFieldsId);
        temp.placeholder = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.Search");
        temp.style.background = "url(Images/Search.png) no-repeat scroll";
        temp.style.backgroundPosition = "right center";
        temp.style.paddingRight = "30px";
    };
    NewViewComponent.prototype.Run = function () {
        var _this = this;
        ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("Customization", "QueryDefinition");
        var copy = false;
        var currentQuery = window.Queries.filter(function (d) { return d.Id == _this.QueryId; })[0];
        var currentQuery = window.Queries.filter(function (d) { return d.Id == _this.QueryId; })[0];
        this.addedQueryColumnList = [];
        this.removedQueryColumnList = [];
        if (currentQuery) {
            if (this.IsNew) {
                if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("Shipment", "CSPV") && (currentQuery.ObjectTableName == "Shipment" || currentQuery.ObjectTableName == "Master")) {
                    this.SpotlightFeatureEnabled = true;
                }
                if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("ARInvoice", "SSPV") && currentQuery.ObjectTableName == "ARInvoice") {
                    this.SpotlightFeatureEnabled = true;
                }
                this.ShowInSpotLight = currentQuery.SpotlightModeActivated;
            }
            this._http.get(ServiceHelper_1.ServiceHelper.GetLogitudeURL() + "api/ngMetaData?tenant=" + SessionInfo_1.SessionInfo.LoggedUserTenant + "&queryid=" + this.QueryId + "&objecttableid=" + this.ObjectTable.Id + "&userid=" + SessionInfo_1.SessionInfo.LoggedUserId)
                .subscribe(function (response) {
                _this.queryColumnsList = response.json();
                _this.queryColumnsList = _this.queryColumnsList.sort(function (a, b) { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1; });
                if (_this.queryColumnsList.length == 0) {
                    var zeroColumnsList = [];
                    _this._http.get(ServiceHelper_1.ServiceHelper.GetLogitudeURL() + "api/ngMetaData?tenant=0&queryid=" + _this.QueryId + "&objecttableid=" + _this.ObjectTable.Id + "&userid=null")
                        .subscribe(function (response) {
                        zeroColumnsList = response.json();
                        zeroColumnsList = zeroColumnsList.sort(function (a, b) { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1; });
                        zeroColumnsList.forEach(function (querycolumn, key) {
                            var newcolumn = new QueryColumnPM_1.QueryColumnPM();
                            newcolumn.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                            newcolumn.UserId = SessionInfo_1.SessionInfo.LoggedUserId;
                            newcolumn.DisplayInList = querycolumn.DisplayInList;
                            newcolumn.ObjectFieldName = querycolumn.ObjectFieldName;
                            newcolumn.ColumnWidth = querycolumn.ColumnWidth;
                            newcolumn.ConverterName = querycolumn.ConverterName;
                            newcolumn.DataTemplateName = querycolumn.DataTemplateName;
                            newcolumn.ColumnHeaderTemplateName = querycolumn.ColumnHeaderTemplateName;
                            newcolumn.IndexOrder = querycolumn.IndexOrder;
                            newcolumn.ObjectFieldDataTypeCode = querycolumn.ObjectFieldDataTypeCode;
                            newcolumn.ObjectFieldFieldLableTextCodeDefaultText = querycolumn.ObjectFieldFieldLableTextCodeDefaultText;
                            newcolumn.ObjectFieldId = querycolumn.ObjectFieldId;
                            newcolumn.ObjectFieldListLabelTextCodeCode = querycolumn.ObjectFieldListLabelTextCodeCode;
                            newcolumn.QueryCode = querycolumn.QueryCode;
                            newcolumn.QueryId = querycolumn.QueryId;
                            newcolumn.QueryObjectTableName = querycolumn.QueryObjectTableName;
                            newcolumn.ObjectFieldFieldLableTextCodeCode = querycolumn.ObjectFieldFieldLableTextCodeCode;
                            _this.queryColumnsList.push(newcolumn);
                            _this.addedQueryColumnList.push(newcolumn);
                        });
                    });
                }
                _this.staticColumnsList = _this.queryColumnsList.filter(function (q) { return q.QueryId == _this.QueryId && ((q.UserId == SessionInfo_1.SessionInfo.LoggedUserId && q.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant)); }).sort(function (a, b) { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1; });
                var listColumns = _this.queryColumnsList.filter(function (q) { return q.QueryId == _this.QueryId && ((q.UserId == SessionInfo_1.SessionInfo.LoggedUserId && q.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant)); }).sort(function (a, b) { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1; });
                _this.unselectedObjectFields = window.ObjectFields.filter(function (a) { return a.ObjectTableName == _this.CurrentObjectTable; }).filter(function (d) { return d.DisplayInList == true && (d.Tenant == SessionInfo_1.SessionInfo.LoggedUserTenant || d.Tenant == 0) && ((d.ValidForQuerySection1 == currentQuery.QuerySection || d.ValidForQuerySection2 == currentQuery.QuerySection) || d.IsCustom == true); });
                _this.unselected = [];
                _this.unselectedObjectFields.forEach(function (field, key) {
                    var xx = _this.queryColumnsList.filter(function (q) { return q.QueryId == _this.QueryId && q.ObjectFieldId == field.Id && field.FieldName != "TimeFrameFilter"; });
                    if (xx.length == 0) {
                        _this.unselected.push(field);
                    }
                });
                //this.UnSelectedQueryColumnsList.ItemsSource = unselected.OrderBy(c => c.FieldName);
                _this.OrderedQueryColumnsList = [];
                _this.unSelectedList = _this.unselected.sort(function (a, b) { return (a.FieldName.toLowerCase() === b.FieldName.toLowerCase()) ? 0 : (a.FieldName.toLowerCase() < b.FieldName.toLowerCase()) ? -1 : 1; });
                _this.queryColumnsList.forEach(function (qc, key) {
                    _this.OrderedQueryColumnsList.push(new QueryColumnsEditComponent_1.QueryColumnDetails(qc));
                });
                _this.OrderedQueryColumnsList = _this.OrderedQueryColumnsList.sort(function (a, b) { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1; });
                _this.CD.detectChanges();
                //SelectedQueryColumnsList.ItemsSource = OrderedQueryColumnsList;
                _this.Fixedunselected = _this.unSelectedList;
                _this.IsEnabled = true;
                _this.onUnSelectedDataLoadedEvent.emit(_this.SelectedItem);
                _this.onSelectedDataLoadedEvent.emit(_this.FieldSelectedItem);
            });
            this.QueryFilterChangedAction(this.QueryId);
            var xx = this.NEWallFilterFieldsClass;
        }
    };
    NewViewComponent.prototype.FillShareValuesList = function () {
        this.ShareValuesList = [];
        var obj1 = new CodeNameClass_1.CodeNameClass();
        obj1.Code = "ALL";
        obj1.Name = "All Users";
        var obj2 = new CodeNameClass_1.CodeNameClass();
        obj2.Code = "SPF";
        obj2.Name = "Specific Users";
        var obj3 = new CodeNameClass_1.CodeNameClass();
        obj3.Code = "NON";
        obj3.Name = "None";
        this.ShareValuesList.push(obj1);
        this.ShareValuesList.push(obj2);
        this.ShareValuesList.push(obj3);
    };
    NewViewComponent.prototype.SetSelectedSharedValue = function () {
        if (this.IsNew) {
            this.shareValueSelectedItem = this.ShareValuesList.filter(function (d) { return d.Code == "NON"; })[0];
        }
        else {
            if (this.EntityPM != null) {
                this.ShareWithUsersCount = this.EntityPM.SharedUserQueries.length;
                if (this.EntityPM.SharedWithAll) {
                    this.shareValueSelectedItem = this.ShareValuesList.filter(function (d) { return d.Code == "ALL"; })[0];
                }
                else if (this.EntityPM.SharedWithSpecificUsers) {
                    this.shareValueSelectedItem = this.ShareValuesList.filter(function (d) { return d.Code == "SPF"; })[0];
                    this.IsChooseUsersVisible = true;
                }
                else {
                    this.shareValueSelectedItem = this.ShareValuesList.filter(function (d) { return d.Code == "NON"; })[0];
                }
            }
        }
    };
    Object.defineProperty(NewViewComponent.prototype, "ShareValueSelectedItem", {
        get: function () { return this.shareValueSelectedItem; },
        set: function (value) {
            if (this.shareValueSelectedItem != value) {
                this.shareValueSelectedItem = value;
                if (value.Code == "SPF") {
                    this.IsChooseUsersVisible = true;
                }
                else {
                    this.IsChooseUsersVisible = false;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    NewViewComponent.prototype.ChooseUsers = function () {
        var _this = this;
        var args = new ChooseUserComponent_1.ChooseUserArgs();
        args.MyQuery = this.EntityPM;
        args.AllUsers = this.myUsersList;
        var logWindow = new LogitudeWindow_1.LogitudeWindow();
        logWindow.Title = "Users List";
        logWindow.Width = 725;
        logWindow.Height = 520;
        logWindow.WindowArgs = args;
        logWindow.Show("./Infrastructure/Components/NewViewComponent/ChooseUserComponent");
        logWindow.WindowClosed.subscribe(function ($event) {
            _this.ShareWithUsersCount = _this.EntityPM.SharedUserQueries.length;
            _this.FillSharedWithUsersItemsSource();
        });
    };
    NewViewComponent.prototype.FillSharedWithUsersItemsSource = function () {
        var _this = this;
        this.SharedWithUsersItemsSource = [];
        this.EntityPM.SharedUserQueries.forEach(function (item) {
            var user = _this.myUsersList.filter(function (d) { return d.Id == item.UserId; })[0];
            _this.SharedWithUsersItemsSource.push(new SharedWithUserItem(item, user));
        });
    };
    NewViewComponent.prototype.DeleteUser = function (user) {
        var itemIndex = this.SharedWithUsersItemsSource.indexOf(user);
        if (itemIndex > -1) {
            this.SharedWithUsersItemsSource.splice(itemIndex, 1);
        }
        var index = this.EntityPM.SharedUserQueries.indexOf(user.myEnity);
        if (index > -1) {
            this.EntityPM.RemoveSharedUserQueryPM(user.myEnity);
        }
        this.ShareWithUsersCount = this.EntityPM.SharedUserQueries.length;
    };
    Object.defineProperty(NewViewComponent.prototype, "SearchText", {
        get: function () { return this.searchText; },
        set: function (newValue) {
            this.searchText = newValue;
            if (newValue != null && newValue != "") {
                this.unSelectedList = this.Fixedunselected.filter(function (f) { return f.ListTextCodeCode.toLowerCase().indexOf(newValue.toLowerCase()) > -1 || ((f.FullNameTextCodeLocalDefaultText != null && f.FullNameTextCodeLocalDefaultText != "") && f.FullNameTextCodeLocalDefaultText.toLowerCase().indexOf(newValue.toLowerCase()) > -1); });
            }
            else {
                this.unSelectedList = this.Fixedunselected;
            }
            this.onUnSelectedDataSourceChangedEvent.emit(this.unSelectedList);
            //this.onUnSelectedDataLoadedEvent.emit(this.SelectedItem);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewViewComponent.prototype, "SelectedItem", {
        get: function () { return this.selectedItem; },
        set: function (newValue) {
            this.selectedItem = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewViewComponent.prototype, "QueryName", {
        get: function () { return this.queryName; },
        set: function (newValue) {
            this.queryName = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewViewComponent.prototype, "ShowInSpotLight", {
        get: function () { return this.showInSpotLight; },
        set: function (newValue) {
            if (this.showInSpotLight != newValue) {
                this.showInSpotLight = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewViewComponent.prototype, "FieldSelectedItem", {
        get: function () { return this.fieldSelectedItem; },
        set: function (newValue) {
            this.fieldSelectedItem = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewViewComponent.prototype, "IsbtnAddEnabled", {
        get: function () { return this.isbtnAddEnabled; },
        set: function (newValue) {
            this.isbtnAddEnabled = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewViewComponent.prototype, "IsbtnRemoveEnabled", {
        get: function () { return this.isbtnRemoveEnabled; },
        set: function (newValue) {
            this.isbtnRemoveEnabled = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewViewComponent.prototype, "IsbtnUpEnabled", {
        get: function () { return this.isbtnUpEnabled; },
        set: function (newValue) {
            this.isbtnUpEnabled = newValue;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(NewViewComponent.prototype, "IsbtnDownEnabled", {
        get: function () { return this.isbtnDownEnabled; },
        set: function (newValue) {
            this.isbtnDownEnabled = newValue;
        },
        enumerable: true,
        configurable: true
    });
    NewViewComponent.prototype.onSelectedItemChanged = function (item) {
        this.SelectedItem = item;
        this.onUnSelectedDataLoadedEvent.emit(null);
        this.CheckEditSharedViewsFeature("Selected");
    };
    NewViewComponent.prototype.onFieldSelectedItemChanged = function (item) {
        this.FieldSelectedItem = item;
        this.onSelectedDataLoadedEvent.emit(null);
        this.CheckEditSharedViewsFeature("Available");
    };
    NewViewComponent.prototype.btnUp_Click = function () {
        var item = this.SelectedItem;
        if (item != null) {
            var i = this.OrderedQueryColumnsList.indexOf(item);
            this.ReorderColumnsList();
            //var upColumn = this.OrderedQueryColumnsList.filter(d => d.ObjectFieldId == item.ObjectFieldId && ((d.Tenant == SessionInfo.LoggedUserTenant && d.UserId == SessionInfo.LoggedUserId) || d.Tenant == 0) && d.QueryId == this.QueryId)[0];
            var upColumn = this.OrderedQueryColumnsList.filter(function (d) { return d.ObjectFieldId == item.ObjectFieldId; })[0];
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
    NewViewComponent.prototype.btnDown_Click = function () {
        var item = this.SelectedItem;
        if (item != null) {
            var i = this.OrderedQueryColumnsList.indexOf(item);
            this.ReorderColumnsList();
            //var downColumn = this.OrderedQueryColumnsList.filter(d => d.ObjectFieldId == item.ObjectFieldId && ((d.Tenant == SessionInfo.LoggedUserTenant && d.UserId == SessionInfo.LoggedUserId) || d.Tenant == 0) && d.QueryId == this.QueryId)[0];
            var downColumn = this.OrderedQueryColumnsList.filter(function (d) { return d.ObjectFieldId == item.ObjectFieldId; })[0];
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
    NewViewComponent.prototype.btnAdd_Click = function () {
        var _this = this;
        if (this.FieldSelectedItem != null) {
            var field = this.FieldSelectedItem;
            var queryColumn = this.unSelectedList.filter(function (a) { return a.QueryId == _this.QueryId && a.FieldName == field.FieldName; })[0];
            if (queryColumn) {
                this.removedQueryColumnList = this.removedQueryColumnList.filter(function (a) { return a.FieldName != queryColumn.FieldName; });
            }
            if (queryColumn) {
                if (queryColumn.Id) {
                    var newQueryColumn = new QueryColumnPM_1.QueryColumnPM();
                    newQueryColumn.QueryId = this.QueryId;
                    newQueryColumn.ObjectFieldId = field.Id;
                    newQueryColumn.ObjectFieldName = field.FieldName;
                    newQueryColumn.ObjectFieldFieldLableTextCodeDefaultText = field.FullNameTextCodeDefaultText;
                    newQueryColumn.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                    newQueryColumn.IndexOrder = (this.OrderedQueryColumnsList.length > 0 ? this.OrderedQueryColumnsList[this.OrderedQueryColumnsList.length - 1].IndexOrder + 1 : 0);
                    newQueryColumn.ColumnWidth = 100;
                    newQueryColumn.ConverterName = field.ConverterName;
                    newQueryColumn.DataTemplateName = field.DataTemplateName;
                    newQueryColumn.ObjectFieldListLabelTextCodeCode = field.ListTextCodeCode;
                    newQueryColumn.DisplayInList = true;
                    newQueryColumn.UserId = SessionInfo_1.SessionInfo.LoggedUserId;
                    newQueryColumn.ObjectFieldFieldLableTextCodeCode = field.FullNameTextCodeCode;
                    this.OrderedQueryColumnsList.push(new QueryColumnsEditComponent_1.QueryColumnDetails(newQueryColumn));
                    this.addedQueryColumnList.push(newQueryColumn);
                }
                else {
                    queryColumn.IndexOrder = (this.OrderedQueryColumnsList.length > 0 ? this.OrderedQueryColumnsList[this.OrderedQueryColumnsList.length - 1].IndexOrder + 1 : 0);
                    this.OrderedQueryColumnsList.push(new QueryColumnsEditComponent_1.QueryColumnDetails(queryColumn));
                }
            }
            else {
                var newQueryColumn = new QueryColumnPM_1.QueryColumnPM();
                newQueryColumn.QueryId = this.QueryId;
                newQueryColumn.ObjectFieldId = field.Id;
                newQueryColumn.ObjectFieldName = field.FieldName;
                newQueryColumn.ObjectFieldFieldLableTextCodeDefaultText = field.FullNameTextCodeDefaultText;
                newQueryColumn.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                newQueryColumn.IndexOrder = (this.OrderedQueryColumnsList.length > 0 ? this.OrderedQueryColumnsList[this.OrderedQueryColumnsList.length - 1].IndexOrder + 1 : 0);
                newQueryColumn.ColumnWidth = 100;
                newQueryColumn.ConverterName = field.ConverterName;
                newQueryColumn.DataTemplateName = field.DataTemplateName;
                newQueryColumn.ObjectFieldListLabelTextCodeCode = field.ListTextCodeCode;
                newQueryColumn.DisplayInList = true;
                newQueryColumn.UserId = SessionInfo_1.SessionInfo.LoggedUserId;
                newQueryColumn.ObjectFieldFieldLableTextCodeCode = field.FullNameTextCodeCode;
                this.OrderedQueryColumnsList.push(new QueryColumnsEditComponent_1.QueryColumnDetails(newQueryColumn));
                this.addedQueryColumnList.push(newQueryColumn);
            }
            this.unSelectedList = this.unSelectedList.filter(function (a) { return a.FieldName != field.FieldName; });
            this.IsbtnAddEnabled = false;
            this.CD.detectChanges();
            if (this.SearchText != null && this.SearchText != "") {
                this.unSelectedList = this.unSelectedList.filter(function (f) { return f.FullNameTextCodeDefaultText.toLowerCase().indexOf(_this.SearchText.toLowerCase()) >= 0 || ((f.FullNameTextCodeLocalDefaultText != null && f.FullNameTextCodeLocalDefaultText != "") && f.FullNameTextCodeLocalDefaultText.toLowerCase().indexOf(_this.SearchText.toLowerCase())); });
            }
            this.onUnSelectedDataLoadedEvent.emit(this.SelectedItem);
        }
        this.onSelectedDataLoadedEvent.emit(this.FieldSelectedItem);
        this.FieldSelectedItem = null;
        this.SelectedItem = null;
    };
    NewViewComponent.prototype.btnRemove_Click = function () {
        var _this = this;
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
            this.IsbtnRemoveEnabled = false;
            this.CD.detectChanges();
            this.onUnSelectedDataLoadedEvent.emit(this.SelectedItem);
            this.onSelectedDataLoadedEvent.emit(this.FieldSelectedItem);
            this.FieldSelectedItem = null;
            this.SelectedItem = null;
        }
    };
    NewViewComponent.prototype.ReorderColumnsList = function () {
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
    NewViewComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CurrentWindow.Close(this.QueryId);
    };
    NewViewComponent.prototype.DeleteButtonClicked = function () {
        var _this = this;
        var confirmWindow = new ConfirmWindow_1.ConfirmWindow();
        confirmWindow.Title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.DeletQuery");
        if (this.EntityPM.UserId == SessionInfo_1.SessionInfo.LoggedUserId) {
            confirmWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.WantToDeleteThisQuery"));
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    _this.DoDelete(SessionInfo_1.SessionInfo.LoggedUserId);
                }
            });
        }
        else {
            confirmWindow.Show("This view has been shared with users other than you. Are you sure you want to delete it?");
            confirmWindow.WindowClosed.subscribe(function (event) {
                if (confirmWindow.Yes) {
                    _this.DoDelete(_this.EntityPM.SharedByUserId);
                }
            });
        }
    };
    NewViewComponent.prototype.DoDelete = function (userId) {
        var _this = this;
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
        var query = window.Queries.filter(function (q) { return q.Id == _this.EntityPM.Id; })[0];
        var myService = new QueriesPMService_1.QueriesPMService();
        myService.setServiceArgs(this.serviceArgs);
        var myGeneralService = new GeneralEntitiesService_1.GeneralEntitiesService();
        myService.delete(query, userId).subscribe(function (myResult) {
            _this.CurrentSession.StopBusyIndicator();
            window.Queries = window.Queries.filter(function (a) { return a.Id != query.Id; });
            var ObjectTable = window.ObjectTables.filter(function (x) { return x.Name === _this.CurrentObjectTable; })[0];
            var Query = window.Queries.filter(function (a) { return a.ObjectTableId === ObjectTable.Id && a.IndexOrder == 0; })[0];
            _this.CurrentSession.CloseCurrentWindow();
        });
        //this.GeneralEntitiesArgs = new GeneralEntitiesArgs();
        //this.GeneralEntitiesArgs.RemovedQueryColumnsPMs = [];
        //this.GeneralEntitiesArgs.RemovedQueryFilters = [];
        //var ObjectTable = window.ObjectTables.filter(a => a.Name == this.CurrentObjectTable)[0];
        //this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("General.M.Saving"));
        //this.GeneralEntitiesArgs.Tenant = SessionInfo.LoggedUserTenant;
        //var myQCService: QueryColumnsPMService = new QueryColumnsPMService();
        //myQCService.setServiceArgs(this.serviceArgs);
        //myQCService.GetQueryColumnPMs(SessionInfo.LoggedUserTenant, this.EntityPM.Id, ObjectTable.Id, userId).subscribe(myResult => {
        //    var queryColumns = myResult;
        //    queryColumns.forEach((column, key) => {
        //        this.GeneralEntitiesArgs.RemovedQueryColumnsPMs.push(column);
        //    });
        //    if (this.myAdvancedQueryFiltersPMService == null) {
        //        this.myAdvancedQueryFiltersPMService = new AdvancedQueryFiltersPMService();
        //    }
        //    this.myAdvancedQueryFiltersPMService.setServiceArgs(this.serviceArgs);
        //    this.myAdvancedQueryFiltersPMService.getadvancedqueryfiltersbytenantByQuery(SessionInfo.LoggedUserTenant, userId, this.EntityPM.Id).subscribe(myResult => {
        //        if (myResult == null) {
        //            this.AdvancedQueryFilterPMs = [];
        //        }
        //        else {
        //            this.AdvancedQueryFilterPMs = myResult;
        //            var advanceQueryFilters = this.AdvancedQueryFilterPMs.filter(c => c.QueryId == this.EntityPM.Id);
        //            advanceQueryFilters.forEach((filter, key) => {
        //                this.GeneralEntitiesArgs.RemovedQueryFilters.push(filter);
        //            });
        //        }
        //        var query = window.Queries.filter(q => q.Id == this.EntityPM.Id)[0];
        //        var myService: QueriesPMService = new QueriesPMService();
        //        myService.setServiceArgs(this.serviceArgs);
        //        var myGeneralService: GeneralEntitiesService = new GeneralEntitiesService();
        //        myGeneralService.setServiceArgs(this.serviceArgs);
        //        myGeneralService.update(this.GeneralEntitiesArgs).subscribe(myResult => {
        //            myService.delete(query).subscribe(myResult => {
        //                this.CurrentSession.StopBusyIndicator();
        //                window.Queries = window.Queries.filter(a => a.Id != query.Id);
        //                var ObjectTable = window.ObjectTables.filter(x => x.Name === this.CurrentObjectTable)[0];
        //                var Query = window.Queries.filter(a => a.ObjectTableId === ObjectTable.Id && a.IndexOrder == 0)[0];
        //                this.CurrentSession.CloseCurrentWindow();
        //            });
        //        });
        //    });
        //});
    };
    NewViewComponent.prototype.QueryFilterChangedAction = function (QueryID) {
        var _this = this;
        this.allFilterFields = [];
        this.constantFilterFieldsList = [];
        this.CurrentFilters = [];
        this.advancedQueryFiltersList = [];
        this.fieldsStaticList = [];
        this.timeFrameFields = [];
        this.AdvancedQueryFilterPMs = [];
        if (this.myAdvancedQueryFiltersPMService == null) {
            this.myAdvancedQueryFiltersPMService = new AdvancedQueryFiltersPMService_1.AdvancedQueryFiltersPMService();
            this.myAdvancedQueryFiltersPMService.setServiceArgs(this.serviceArgs);
        }
        this.myAdvancedQueryFiltersPMService.getadvancedqueryfiltersbytenantByQuery(SessionInfo_1.SessionInfo.LoggedUserTenant, SessionInfo_1.SessionInfo.LoggedUserId, QueryID).subscribe(function (myResult) {
            _this.GetFiltersComplete(myResult, QueryID);
        });
    };
    NewViewComponent.prototype.GetFiltersComplete = function (myResult, QueryID) {
        var _this = this;
        if (myResult == null) {
            this.AdvancedQueryFilterPMs = [];
        }
        else {
            this.AdvancedQueryFilterPMs = myResult;
        }
        this.timeFilterFieldsClass = new FilterField_1.FilterFieldsClass(true, this, this.pubSubAdvanceQueryFiltersService);
        this.FieldsValues = new FilterField_1.FieldsValues();
        this.currentQuery = window.Queries.filter(function (q) { return q.Id == QueryID; })[0];
        if (!this.currentQuery) {
        }
        else {
            if (this.currentQuery.DisplayAsCustom) {
                //btnEditView.IsEnabled = true;
            }
            if (this.ObjectFields) {
                this.allFilterFields = window.ObjectFields.filter(function (o) { return o.CanFilter == true && o.DataTypeCode != "Constant" && ((o.ValidForQuerySection1 == _this.currentQuery.QuerySection || o.ValidForQuerySection2 == _this.currentQuery.QuerySection) || (o.ObjectTableId == _this.ObjectTable.Id && o.IsCustom == true)); });
                this.constantFilterFieldsList = window.ObjectFields.filter(function (o) { return o.CanFilter == true && o.DataTypeCode == "Constant" && ((o.ValidForQuerySection1 == _this.currentQuery.QuerySection || o.ValidForQuerySection2 == _this.currentQuery.QuerySection) || (o.ObjectTableId == _this.ObjectTable.Id && o.IsCustom == true)); });
                this.timeFilterFieldsClass.AddFiltersList(window.ObjectFields.filter(function (o) { return o.CanFilter == true && o.FieldName != "TimeFrameFilter" && o.IsTimeFrameFilter == true && (o.ValidForQuerySection1 == _this.currentQuery.QuerySection || o.ValidForQuerySection2 == _this.currentQuery.QuerySection); }), this.currentQuery.Id, myResult);
                this.NEWallFilterFieldsClass.AddFiltersList(window.ObjectFields.filter(function (o) { return o.CanFilter == true && o.DataTypeCode != "Constant" && ((o.ValidForQuerySection1 == _this.currentQuery.QuerySection || o.ValidForQuerySection2 == _this.currentQuery.QuerySection) || (o.ObjectTableId == _this.ObjectTable.Id && o.IsCustom == true)); }), this.currentQuery.Id, myResult);
            }
            else {
                this.allFilterFields = [];
                this.constantFilterFieldsList = [];
                this.NEWallFilterFieldsClass.AddFiltersList([], this.currentQuery.Id, myResult);
            }
            this.filterFields.AddFiltersList(this.allFilterFields, this.currentQuery.Id, myResult);
            this.constantFilterFields.AddFiltersList(this.constantFilterFieldsList, this.currentQuery.Id, myResult);
            this.fieldsStaticList = this.NEWallFilterFieldsClass.FilterFields;
            this.timeFrameFields = this.timeFilterFieldsClass.FilterFields; //fieldsStaticList.Where(d => d.ObjectField.IsTimeFrameFilter == true).ToList();
            var OFPM = new ObjectFieldPM_1.ObjectFieldPM();
            OFPM.FieldName = "NoFilter";
            OFPM.FullNameTextCodeCode = "No Filter";
            this.noFiltersField = new FilterField_1.FilterField(OFPM, this.currentQuery.Id, true, myResult, this, this.pubSubAdvanceQueryFiltersService);
            this.timeFrameFields.push(this.noFiltersField);
            //if (!this.IsNew) {
            this.advancedQueryFiltersList = myResult.filter(function (q) { return q.QueryId == QueryID; });
            //}
            //else {
            //    this.advancedQueryFiltersList = [];
            //}
            this.fillqueryfilters();
        }
    };
    NewViewComponent.prototype.fillqueryfilters = function () {
        var _this = this;
        //if (this.advancedQueryFiltersList == undefined || this.advancedQueryFiltersList == null) {
        this.SelectedObjectFields = [];
        //}
        //else {
        this.advancedQueryFiltersList.forEach(function (item, key) {
            _this.objectField = window.ObjectFields.filter(function (o) { return o.Id === item.ObjectFieldId; })[0];
            var xx = _this.MapJsonToEntityPM(_this.objectField);
            _this.AddFilterField(xx);
        });
        //}
    };
    NewViewComponent.prototype.MapJsonToEntityPM = function (jsonPM, mapParent) {
        if (mapParent === void 0) { mapParent = true; }
        var entityPM;
        entityPM = new ObjectFieldPM_1.ObjectFieldPM();
        var jsonPMKeys = Object.keys(jsonPM);
        for (var key in jsonPMKeys) {
            if (jsonPMKeys[key] === "UIProperties") {
                continue;
            }
            var property = jsonPMKeys[key];
            entityPM[property] = jsonPM[property];
        }
        entityPM.IsDirty = false;
        return entityPM;
    };
    NewViewComponent.prototype.AddFilterField = function (field) {
        if (this.SelectedObjectFields == undefined) {
            this.SelectedObjectFields = [];
        }
        var filters = this.AdvancedQueryFilterPMs.filter(function (d) { return d.IsPredefined == true && d.ObjectFieldId == field.Id; });
        if (filters != null && filters[0] != null) {
            var value = this.AdvancedQueryFilterPMs.filter(function (d) { return d.IsPredefined == true && d.ObjectFieldId == field.Id; })[0].PredefinedValue;
            this.FieldsValues.SetFieldValue(field.Id, value);
        }
        if (!this.IsNew) {
            this.SelectedObjectFields.push(new FilterField_1.FilterField(field, this.QueryId, true, filters, this, this.pubSubAdvanceQueryFiltersService));
        }
        else {
            this.SelectedObjectFields.push(new FilterField_1.FilterField(field, "", true, filters, this, this.pubSubAdvanceQueryFiltersService));
        }
        if (this.SelectedObjectFields.length > 10) {
            this.ValidationErrorsList = [];
            this.ValidationErrorsList.push("The max. number of filters you can use is 10");
            this.NEWallFilterFieldsClass.SetExists(field, false);
            return;
        }
        this.NEWallFilterFieldsClass.SetExists(field, true);
    };
    NewViewComponent.prototype.RemoveFilterField = function (field) {
        if (this.SelectedObjectFields != null) {
            this.SelectedObjectFields = this.SelectedObjectFields.filter(function (a) { return a.ObjectField.Id != field.Id; });
        }
    };
    NewViewComponent.prototype.OnQueryFilterChanged = function (QueryID) {
        this.QueryId = QueryID;
        this.QueryFilterChangedAction(this.QueryId);
        //if (this.IsOpened) {
        //    this.SaveChangesAndRecreate();
        //}
    };
    Object.defineProperty(NewViewComponent.prototype, "FilterssearchText", {
        get: function () { return this.filterssearchText; },
        set: function (newValue) {
            var _this = this;
            this.filterssearchText = newValue;
            if (newValue != null && newValue != "") {
                this.NEWallFilterFieldsClass.FilterFields = this.fieldsStaticList.filter(function (f) { return TextCodeTranslator_1.TextCodeTranslator.Translate(f.ObjectField.FullNameTextCodeCode).toLowerCase().indexOf(newValue.toLowerCase()) > -1; });
                this.SelectedObjectFields.forEach(function (item, key) {
                    _this.NEWallFilterFieldsClass.SetExists(item, true);
                    _this.filterFields.SetExists(item, true);
                });
            }
            else {
                this.NEWallFilterFieldsClass.FilterFields = this.fieldsStaticList;
                this.SelectedObjectFields.forEach(function (item, key) {
                    _this.NEWallFilterFieldsClass.SetExists(item, true);
                    _this.filterFields.SetExists(item, true);
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    NewViewComponent.prototype.DeteteFilter = function (field) {
        //if (this.AdvancedQueryFilterPMs.filter(f => f.ObjectFieldId == field.ObjectField.Id && f.QueryId == this.QueryId)[0] != null) {
        //    var advanceFilter = this.AdvancedQueryFilterPMs.filter(f => f.ObjectFieldId == field.ObjectField.Id && f.QueryId == this.QueryId)[0];
        //var myService: AdvancedQueryFiltersPMService = new AdvancedQueryFiltersPMService();
        //myService.setServiceArgs(this.serviceArgs);
        //myService.delete(advanceFilter).subscribe(myResult => {
        if (this.SelectedObjectFields != null) {
            this.removedQueryFilters.push(this.SelectedObjectFields.filter(function (a) { return a.ObjectField.Id == field.ObjectField.Id; })[0]);
            this.SelectedObjectFields = this.SelectedObjectFields.filter(function (a) { return a.ObjectField.Id != field.ObjectField.Id; });
        }
        if (this.NEWallFilterFieldsClass != null) {
            this.NEWallFilterFieldsClass.SetExists(field, false);
        }
        //});
        //}
    };
    NewViewComponent.prototype.CLearAll = function () {
        var _this = this;
        if (this.NEWallFilterFieldsClass != null) {
            this.SelectedObjectFields.forEach(function (field, key) {
                _this.NEWallFilterFieldsClass.SetExists(field, false);
            });
        }
        if (this.SelectedObjectFields != null) {
            this.SelectedObjectFields = [];
        }
    };
    NewViewComponent.prototype.btnCreateNewQuery_Click = function () {
        if (this.IsNew) {
            this.SavePredifinedQuery(this.QueryName);
        }
        else {
            this.SaveAndClose(this.QueryName);
        }
    };
    NewViewComponent.prototype.SavePredifinedQuery = function (queryName) {
        var _this = this;
        var DoSaving = false;
        var myService = new QueriesPMService_1.QueriesPMService();
        var textCodesService = new TextCodePMService_1.TextCodePMService();
        this.ValidationErrorsList = [];
        if (!Tools_1.AppTool.IsNullOrEmpty(queryName)) {
            if (queryName.length > 30) {
                DoSaving = false;
                this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.QueryNameLength"));
                return;
            }
            if (this.SelectedObjectFields.length == 0) {
                DoSaving = true;
            }
            else {
                this.SelectedObjectFields.forEach(function (item, key) {
                    if (item.TextValue == null || item.TextValue == "") {
                        if (item.ObjectField.DataTypeCode != "Boolean") {
                            DoSaving = false;
                            _this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.SomeFiltersHaveNoValue"));
                            return;
                        }
                        else {
                            DoSaving = true;
                        }
                    }
                    else {
                        DoSaving = true;
                    }
                });
            }
            if (DoSaving) {
                this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
                var theCurrentQuery = (window.Queries.filter(function (q) { return q.Id == _this.QueryId; })[0]);
                var temp = window.Queries.filter(function (q) { return q.ObjectTableId == theCurrentQuery.ObjectTableId && q.UserId == theCurrentQuery.UserId && q.QueryGroupCode == theCurrentQuery.QueryGroupCode; }).sort(function (a, b) { return (a.IndexOrder === b.IndexOrder) ? 0 : (a.IndexOrder < b.IndexOrder) ? -1 : 1; });
                var maxIndex = temp[temp.length - 1];
                this.EntityPM.QuerySection = theCurrentQuery.QuerySection;
                this.EntityPM.ObjectTableId = theCurrentQuery.ObjectTableId;
                this.EntityPM.ObjectTableName = theCurrentQuery.ObjectTableName;
                this.EntityPM.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                this.EntityPM.UserId = SessionInfo_1.SessionInfo.LoggedUserId;
                this.EntityPM.Code = "111";
                this.EntityPM.IndexOrder = maxIndex.IndexOrder + 1;
                this.EntityPM.ObjectTableIsNewWizard = theCurrentQuery.ObjectTableIsNewWizard;
                this.EntityPM.ObjectTableNewWizardControlName = theCurrentQuery.ObjectTableNewWizardControlName;
                this.EntityPM.OriginalQueryId = theCurrentQuery.Id;
                this.EntityPM.QueryGroupCode = theCurrentQuery.QueryGroupCode;
                this.EntityPM.IsAddNewEntityEnabled = theCurrentQuery.IsAddNewEntityEnabled;
                this.EntityPM.NewViewName = queryName;
                this.EntityPM.Perspective = theCurrentQuery.Perspective;
                this.EntityPM.EditWizardName = theCurrentQuery.EditWizardName;
                this.EntityPM.SpotlightModeActivated = this.ShowInSpotLight;
                if (this.ShareValueSelectedItem) {
                    switch (this.ShareValueSelectedItem.Code) {
                        case "ALL": {
                            this.EntityPM.SharedWithAll = true;
                            this.EntityPM.SharedWithSpecificUsers = false;
                            this.EntityPM.SharedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                            if (this.EntityPM.SharedUserQueries != null && this.EntityPM.SharedUserQueries.length > 0) {
                                for (var i = this.EntityPM.SharedUserQueries.length - 1; i >= 0; i--) {
                                    var item = this.EntityPM.SharedUserQueries[i];
                                    this.EntityPM.RemoveSharedUserQueryPM(item);
                                }
                            }
                            break;
                        }
                        case "SPF": {
                            this.EntityPM.SharedWithAll = false;
                            this.EntityPM.SharedWithSpecificUsers = true;
                            this.EntityPM.SharedByUserId = SessionLocator_1.SessionLocator.LoggedUserId;
                            break;
                        }
                        case "NON": {
                            this.EntityPM.SharedWithAll = false;
                            this.EntityPM.SharedWithSpecificUsers = false;
                            this.EntityPM.SharedByUserId = null;
                            if (this.EntityPM.SharedUserQueries != null && this.EntityPM.SharedUserQueries.length > 0) {
                                for (var i = this.EntityPM.SharedUserQueries.length - 1; i >= 0; i--) {
                                    var item = this.EntityPM.SharedUserQueries[i];
                                    this.EntityPM.RemoveSharedUserQueryPM(item);
                                }
                            }
                            break;
                        }
                    }
                }
                var spotlightTemplate = "";
                if (this.EntityPM.SpotlightModeActivated) {
                    if (this.SpotlightFeatureEnabled) {
                        switch (this.EntityPM.ObjectTableName) {
                            case "Shipment": {
                                spotlightTemplate = "ShipmentSpotlightDataTemplate";
                                break;
                            }
                            case "Master": {
                                spotlightTemplate = "MasterSpotlightDataTemplate";
                                break;
                            }
                            case "ARInvoice": {
                                spotlightTemplate = "ARInvoiceSpotlightDataTemplate";
                                break;
                            }
                        }
                        this.EntityPM.SpotlightDataTemplate = spotlightTemplate;
                    }
                }
                else {
                    this.EntityPM.SpotlightDataTemplate = null;
                }
                myService.setServiceArgs(this.serviceArgs);
                textCodesService.setServiceArgs(this.serviceArgs);
                myService.insert(this.EntityPM).subscribe(function (myResult) {
                    textCodesService.get(myResult.Result.NameTextCodeId, myResult.Result.Tenant).subscribe(function (res) {
                        window.TextCodesTranslations.push(res);
                        _this.AddFiltersAndColumns(myResult.Result);
                        window.Queries.push(myResult.Result);
                    });
                });
            }
        }
        else {
            this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.QueryNamecannotBeEmpty"));
        }
    };
    NewViewComponent.prototype.AddFiltersAndColumns = function (newQuery) {
        var _this = this;
        var myGeneralService = new GeneralEntitiesService_1.GeneralEntitiesService();
        var columns = this.OrderedQueryColumnsList;
        columns.forEach(function (column, key) {
            var newColumn = new QueryColumnPM_1.QueryColumnPM();
            newColumn.IndexOrder = column.IndexOrder;
            newColumn.ObjectFieldId = column.ObjectFieldId;
            newColumn.QueryId = _this.IsNew ? newQuery.Id : _this.QueryId;
            newColumn.Tenant = newQuery.Tenant;
            newColumn.ColumnWidth = column.ColumnWidth;
            newColumn.ConverterName = column.ConverterName;
            newColumn.DataTemplateName = column.DataTemplateName;
            newColumn.ObjectFieldFieldLableTextCodeDefaultText = column.ObjectFieldFieldLableTextCodeDefaultText;
            newColumn.ObjectFieldListLabelTextCodeCode = column.ObjectFieldListLabelTextCodeCode;
            newColumn.ObjectFieldName = column.ObjectFieldName;
            newColumn.QueryCode = column.QueryCode;
            newColumn.QueryObjectTableName = column.QueryObjectTableName;
            newColumn.DisplayInList = true;
            if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.SharedByUserId)) {
                newColumn.UserId = _this.EntityPM.SharedByUserId;
            }
            else {
                newColumn.UserId = SessionInfo_1.SessionInfo.LoggedUserId;
            }
            _this.GeneralEntitiesArgs.QueryColumnsPMs.push(newColumn);
        });
        this.SelectedObjectFields.forEach(function (field, key) {
            if (field.TextValue != null) {
                var advanceFilter = new AdvancedQueryFilterPM_1.AdvancedQueryFilterPM();
                advanceFilter.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                advanceFilter.ObjectFieldId = field.ObjectField.Id;
                advanceFilter.QueryId = _this.IsNew ? newQuery.Id : _this.QueryId;
                advanceFilter.DataTypeCode = field.ObjectField.DataTypeCode;
                advanceFilter.DisplayInList = field.ObjectField.DisplayInList;
                advanceFilter.IsCustomFilter = field.ObjectField.IsCustomFilter;
                advanceFilter.ObjectFieldName = field.ObjectField.FieldName;
                advanceFilter.QueryCode = _this.currentQuery.Code;
                advanceFilter.QueryObjectTableName = _this.currentQuery.ObjectTableName;
                advanceFilter.QueryUserId = _this.currentQuery.UserId;
                advanceFilter.ObjectFieldOperator = field.ObjectField.Operator;
                advanceFilter.Operator = field.Operation.Code;
                advanceFilter.PredefinedValue = field.TextValue;
                advanceFilter.IsPredefined = true;
                if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.SharedByUserId)) {
                    advanceFilter.UserId = _this.EntityPM.SharedByUserId;
                }
                else {
                    advanceFilter.UserId = SessionInfo_1.SessionInfo.LoggedUserId;
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(field.MyName)) {
                    advanceFilter.PredefinedValue = field.MyName;
                }
                else if (field.ObjectField.DataTypeCode == "DateTime") {
                    var date = _this.FieldsValues.GetFieldValue(field.ObjectField.Id);
                    if (field.TextValue != null && date != null && field.Operation.Code != "LargerThan" && field.Operation.Code != "LessThan") {
                        advanceFilter.PredefinedValue = date;
                    }
                }
                if (field.ObjectField.DataTypeCode == "DateTime" && field.Operation.Code == "Between") {
                    advanceFilter.PredefinedValue2 = field.TextValue1;
                }
                _this.GeneralEntitiesArgs.AdvancedQueryFilterPMs.push(advanceFilter);
            }
            else {
                _this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.SomeFiltersHaveNoValue"));
            }
        });
        this.GeneralEntitiesArgs.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
        myGeneralService.setServiceArgs(this.serviceArgs);
        if (this.IsNew) {
            myGeneralService.insert(this.GeneralEntitiesArgs).subscribe(function (myResult) {
                myResult.Result.AdvancedQueryFilterPMs.forEach(function (filter, key) {
                    window.PreDefinedFilters.push(filter);
                });
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                _this.CurrentSession.CurrentWindow.Close(newQuery.Id);
            });
        }
        else {
            myGeneralService.update(this.GeneralEntitiesArgs).subscribe(function (myResult) {
                _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                _this.CurrentSession.CurrentWindow.Close(newQuery.Id);
            });
        }
    };
    NewViewComponent.prototype.SaveAndClose = function (queryName) {
        var _this = this;
        var myService = new QueriesPMService_1.QueriesPMService();
        var textCodesService = new TextCodePMService_1.TextCodePMService();
        var DoSaving = false;
        if (!Tools_1.AppTool.IsNullOrEmpty(queryName)) {
            if (queryName.length > 30) {
                DoSaving = false;
                if (this.ValidationErrorsList == null) {
                    this.ValidationErrorsList = [];
                }
                this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.QueryNameLength"));
                return;
            }
            if (this.SelectedObjectFields.length == 0) {
                DoSaving = true;
            }
            else {
                this.SelectedObjectFields.forEach(function (item, key) {
                    if (item.TextValue == null || item.TextValue == "") {
                        if (item.ObjectField.DataTypeCode != "Boolean") {
                            DoSaving = false;
                            if (_this.ValidationErrorsList == null) {
                                _this.ValidationErrorsList = [];
                            }
                            _this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.SomeFiltersHaveNoValue"));
                            return;
                        }
                        else {
                            DoSaving = true;
                        }
                    }
                    else {
                        DoSaving = true;
                    }
                });
            }
        }
        if (DoSaving == true) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator(TextCodeTranslator_1.TextCodeTranslator.Translate("General.M.Saving"));
            this.EntityPM.NewViewName = queryName;
            this.EntityPM.SpotlightModeActivated = this.ShowInSpotLight;
            var spotlightTemplate = "";
            if (this.EntityPM.SpotlightModeActivated) {
                if (this.ShowInSpotLight) {
                    switch (this.EntityPM.ObjectTableName) {
                        case "Shipment": {
                            spotlightTemplate = "ShipmentSpotlightDataTemplate";
                            break;
                        }
                        case "Master": {
                            spotlightTemplate = "MasterSpotlightDataTemplate";
                            break;
                        }
                        case "ARInvoice": {
                            spotlightTemplate = "ARInvoiceSpotlightDataTemplate";
                            break;
                        }
                    }
                    this.EntityPM.SpotlightDataTemplate = spotlightTemplate;
                }
            }
            else {
                this.EntityPM.SpotlightDataTemplate = null;
            }
            if (this.ShareValueSelectedItem) {
                switch (this.ShareValueSelectedItem.Code) {
                    case "ALL": {
                        this.EntityPM.SharedWithAll = true;
                        this.EntityPM.SharedWithSpecificUsers = false;
                        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.SharedByUserId)) {
                            this.EntityPM.SharedByUserId = SessionInfo_1.SessionInfo.LoggedUserId;
                        }
                        if (this.EntityPM.SharedUserQueries != null && this.EntityPM.SharedUserQueries.length > 0) {
                            for (var i = this.EntityPM.SharedUserQueries.length - 1; i >= 0; i--) {
                                var item = this.EntityPM.SharedUserQueries[i];
                                this.EntityPM.RemoveSharedUserQueryPM(item);
                            }
                        }
                        break;
                    }
                    case "SPF": {
                        this.EntityPM.SharedWithAll = false;
                        this.EntityPM.SharedWithSpecificUsers = true;
                        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.SharedByUserId)) {
                            this.EntityPM.SharedByUserId = SessionInfo_1.SessionInfo.LoggedUserId;
                        }
                        break;
                    }
                    case "NON": {
                        this.EntityPM.SharedWithAll = false;
                        this.EntityPM.SharedWithSpecificUsers = false;
                        this.EntityPM.SharedByUserId = null;
                        if (this.EntityPM.SharedUserQueries != null && this.EntityPM.SharedUserQueries.length > 0) {
                            for (var i = this.EntityPM.SharedUserQueries.length - 1; i >= 0; i--) {
                                var item = this.EntityPM.SharedUserQueries[i];
                                this.EntityPM.RemoveSharedUserQueryPM(item);
                            }
                        }
                        break;
                    }
                }
            }
            myService.setServiceArgs(this.serviceArgs);
            textCodesService.setServiceArgs(this.serviceArgs);
            myService.update(this.EntityPM).subscribe(function (myResult) {
                if (!SessionLocator_1.SessionLocator.UseCachedData) {
                    textCodesService.get(myResult.Result.NameTextCodeId, myResult.Result.Tenant).subscribe(function (res) {
                        window.TextCodesTranslations = window.TextCodesTranslations.filter(function (a) { return a.TextCodeId != myResult.Result.NameTextCodeId; });
                        window.TranslationsCache = window.TranslationsCache.filter(function (d) { return d.Code != myResult.Result.NameTextCodeCode; });
                        window.TextCodesTranslations.push(res);
                        window.TranslationsCache.push(res);
                        _this.UpdateColumnsAndFilters();
                        var CurrentQuery = window.Queries.filter(function (x) { return x.Id == _this.QueryId; })[0];
                        CurrentQuery = _this.EntityPM;
                    });
                }
                else {
                    CachedDataManager_1.CachedDataManager.RefreshTenantTextCodes().subscribe(function (response) {
                        _this.UpdateColumnsAndFilters();
                        var CurrentQuery = window.Queries.filter(function (x) { return x.Id == _this.QueryId; })[0];
                        CurrentQuery = _this.EntityPM;
                    });
                }
                var oldItem = window.Queries.filter(function (t) { return t.Id == _this.EntityPM.Id; })[0];
                if (oldItem) {
                    var index = window.Queries.indexOf(oldItem);
                    window.Queries.splice(index, 1);
                    window.Queries.push(_this.EntityPM);
                }
            });
        }
    };
    NewViewComponent.prototype.UpdateColumnsAndFilters = function () {
        var _this = this;
        var myGeneralService = new GeneralEntitiesService_1.GeneralEntitiesService();
        this.GeneralEntitiesArgs.QueryColumnsPMs = [];
        this.GeneralEntitiesArgs.RemovedQueryColumnsPMs = [];
        this.GeneralEntitiesArgs.RemovedQueryFilters = [];
        this.removedQueryColumnList.forEach(function (queryColumn, key) {
            _this.GeneralEntitiesArgs.RemovedQueryColumnsPMs.push(queryColumn);
        });
        this.OrderedQueryColumnsList.forEach(function (queryColumn, key) {
            _this.GeneralEntitiesArgs.QueryColumnsPMs.push(queryColumn);
        });
        this.removedQueryFilters.forEach(function (item, key) {
            if (item != null && item.AdvancedQueryFilterPM != null) {
                _this.GeneralEntitiesArgs.RemovedQueryFilters.push(item.AdvancedQueryFilterPM);
                window.PreDefinedFilters = window.PreDefinedFilters.filter(function (a) { return a.ObjectFieldId != item.AdvancedQueryFilterPM.objectFieldId; });
                _this.AdvancedQueryFilterPMs = _this.AdvancedQueryFilterPMs.filter(function (a) { return a.ObjectFieldId != item.AdvancedQueryFilterPM.objectFieldId; });
            }
        });
        this.SelectedObjectFields.forEach(function (item, key) {
            if (item.AdvancedQueryFilterPM != null) {
                if (item.ObjectField.DataTypeCode != "DateTime" && item.ObjectField.DataTypeCode != "Date") {
                    item.AdvancedQueryFilterPM.PredefinedValue = item.TextValue;
                }
                else if (!Tools_1.AppTool.IsNullOrEmpty(item.MyName)) {
                    item.AdvancedQueryFilterPM.PredefinedValue = item.MyName;
                }
                else {
                    if (item.Operation.Code == "LargerThan" || item.Operation.Code == "LessThan") {
                        var myDate = new Date(item.TextValue.toString());
                        var temp = Tools_1.DateTool.GetDateParts(myDate);
                        var dt = temp.Year + "-" + temp.Month + "-" + temp.Day;
                        item.AdvancedQueryFilterPM.PredefinedValue = dt;
                    }
                    if (item.Operation.Code == "Between") {
                        //var myDate: Date = new Date(item.TextValue1.toString());
                        //var temp = DateTool.GetDateParts(myDate);
                        //var dt = temp.Year + "-" + temp.Month + "-" + temp.Day;
                        //item.AdvancedQueryFilterPM.PredefinedValue2 = dt;
                        item.AdvancedQueryFilterPM.PredefinedValue2 = item.TextValue1;
                    }
                    else {
                        item.AdvancedQueryFilterPM.PredefinedValue2 = null;
                    }
                }
                item.AdvancedQueryFilterPM.Operator = item.Operation.Code;
                _this.GeneralEntitiesArgs.AdvancedQueryFilterPMs.push(item.AdvancedQueryFilterPM);
            }
            else {
                var advanceFilter = new AdvancedQueryFilterPM_1.AdvancedQueryFilterPM();
                advanceFilter.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
                advanceFilter.ObjectFieldId = item.ObjectField.Id;
                advanceFilter.QueryId = _this.QueryId;
                advanceFilter.DataTypeCode = item.ObjectField.DataTypeCode;
                advanceFilter.DisplayInList = item.ObjectField.DisplayInList;
                advanceFilter.IsCustomFilter = item.ObjectField.IsCustomFilter;
                advanceFilter.ObjectFieldName = item.ObjectField.FieldName;
                advanceFilter.QueryCode = _this.currentQuery.Code;
                advanceFilter.QueryObjectTableName = _this.currentQuery.ObjectTableName;
                advanceFilter.QueryUserId = _this.currentQuery.UserId;
                advanceFilter.ObjectFieldOperator = item.ObjectField.Operator;
                advanceFilter.Operator = item.Operation.Code;
                advanceFilter.PredefinedValue = item.TextValue;
                advanceFilter.IsPredefined = true;
                if (!Tools_1.AppTool.IsNullOrEmpty(_this.EntityPM.SharedByUserId)) {
                    advanceFilter.UserId = _this.EntityPM.SharedByUserId;
                }
                else {
                    advanceFilter.UserId = SessionInfo_1.SessionInfo.LoggedUserId;
                }
                if (!Tools_1.AppTool.IsNullOrEmpty(item.MyName)) {
                    advanceFilter.PredefinedValue = item.MyName;
                }
                else if (item.ObjectField.DataTypeCode == "DateTime") {
                    var date = _this.FieldsValues.GetFieldValue(item.ObjectField.Id);
                    if (item.TextValue != null && date != null) {
                        advanceFilter.PredefinedValue = date;
                    }
                    if (item.Operation.Code == "Between") {
                        //var myDate: Date = new Date(item.TextValue1.toString());
                        //var temp = DateTool.GetDateParts(myDate);
                        //var dt = temp.Year + "-" + temp.Month + "-" + temp.Day;//myDate.getDay() + "-" + (myDate.getMonth() + 1) + "-" + myDate.getFullYear();
                        //advanceFilter.PredefinedValue2 = dt;
                        advanceFilter.PredefinedValue2 = item.TextValue1;
                    }
                }
                _this.GeneralEntitiesArgs.AdvancedQueryFilterPMs.push(advanceFilter);
            }
        });
        this.GeneralEntitiesArgs.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant;
        myGeneralService.setServiceArgs(this.serviceArgs);
        myGeneralService.update(this.GeneralEntitiesArgs).subscribe(function (myResult) {
            myResult.Result.AdvancedQueryFilterPMs.forEach(function (filter, key) {
                if (window.PreDefinedFilters.filter(function (o) { return o.Id === filter.Id; }).length == 0) {
                    window.PreDefinedFilters.push(filter);
                }
                else {
                    window.PreDefinedFilters = window.PreDefinedFilters.filter(function (o) { return o.Id != filter.Id; });
                    window.PreDefinedFilters.push(filter);
                }
            });
            _this.removedQueryFilters.forEach(function (item, key) {
                if (item != null && item.AdvancedQueryFilterPM != null) {
                    window.PreDefinedFilters = window.PreDefinedFilters.filter(function (a) { return a.Id != item.AdvancedQueryFilterPM.Id; });
                }
            });
            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            _this.CurrentSession.CurrentWindow.Close(_this.QueryId);
        });
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], NewViewComponent.prototype, "onDataSourceChangedEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], NewViewComponent.prototype, "onUnSelectedDataLoadedEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], NewViewComponent.prototype, "onSelectedDataLoadedEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], NewViewComponent.prototype, "onUnSelectedDataSourceChangedEvent", void 0);
    NewViewComponent = __decorate([
        core_1.Component({
            selector: 'NewViewComponent',
            moduleId: module.id,
            templateUrl: './NewViewComponent.html',
            inputs: ['ObjectTableName', 'event', 'isWindowViewMode', 'isNewViewMode', 'QueryId', 'Filterchangeevent', 'rabaia'],
            providers: [http_1.Http, ServiceArgs_1.ServiceArgs],
        }),
        __metadata("design:paramtypes", [forms_1.FormBuilder, core_1.ChangeDetectorRef])
    ], NewViewComponent);
    return NewViewComponent;
}());
exports.NewViewComponent = NewViewComponent;
var SharedWithUserItem = /** @class */ (function () {
    function SharedWithUserItem(entity, user) {
        this.myEnity = entity;
        this.myUser = user;
    }
    Object.defineProperty(SharedWithUserItem.prototype, "Email", {
        get: function () { return this.myUser.Email; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SharedWithUserItem.prototype, "Name", {
        get: function () { return this.myUser.EnglishName; },
        enumerable: true,
        configurable: true
    });
    return SharedWithUserItem;
}());
exports.SharedWithUserItem = SharedWithUserItem;
//# sourceMappingURL=NewViewComponent.js.map