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
var http_1 = require("@angular/http");
var ServiceArgs_1 = require("../../../Infrastructure/DataContracts/ServiceArgs");
var ObjectFieldPM_1 = require("../../../Infrastructure/EntityPMs/ObjectFieldPM");
var forms_1 = require("@angular/forms");
var ApiFiltersEvent_1 = require("../../../Infrastructure/Utilities/events/ApiFiltersEvent");
var FilterField_1 = require("../../../Infrastructure/Components/LogitudeComponents/QueryListComponent/FilterField");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var ServiceHelper_1 = require("../../../Infrastructure/Utilities/ServiceHelper");
var ObjectsLocator_1 = require("../../../Infrastructure/Locators/ObjectsLocator");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var AdvanceSearchComponent = /** @class */ (function () {
    function AdvanceSearchComponent(fb, pubSubService, CD) {
        this.pubSubService = pubSubService;
        this.CD = CD;
        this.IsUserQuery = false;
        this.BooleanValues = ["True", "False", "No Filter"];
        this.LayoutDirection = 'ltr';
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.timeFilterFieldsClass = new FilterField_1.FilterFieldsClass(false, this, this.pubSubService);
        this.serviceArgs = new ServiceArgs_1.ServiceArgs();
        this.serviceArgs.http = ServiceHelper_1.ServiceHelper.Http;
        this.myForm = fb.group({
        //'ShipperName': ['', Validators.required]
        });
        if (this.CurrentSession == null) {
            this.AdvanceQueryDropButtonId = "dvanceQueryDropButton_-1_-1";
            this.AdvanceQuerySearchFieldsId = "AdvanceQuerySearchFields_-1_-1";
        }
        else {
            this.AdvanceQueryDropButtonId = "dvanceQueryDropButton_" + this.CurrentSession.GetNewId("dvanceQueryDropButton");
            this.AdvanceQuerySearchFieldsId = "AdvanceQuerySearchFields_" + this.CurrentSession.GetNewId("AdvanceQuerySearchFields");
        }
        //RTL Layout
        this.LayoutDirection = ObjectsLocator_1.ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection;
    }
    AdvanceSearchComponent.prototype.ShowFieldsList = function () {
        document.getElementById("FieldsDropdown").classList.toggle("showDDButton");
    };
    AdvanceSearchComponent.prototype.ClearPlaceHolder = function () {
        var temp = document.getElementById(this.AdvanceQuerySearchFieldsId);
        temp.placeholder = "";
        temp.style.background = "rgba(0, 0, 0, 0)";
        var ToggleBTN = document.getElementById(this.AdvanceQueryDropButtonId);
        ToggleBTN.className = "ToggleButtonMenuTemp";
        //this.CD.detectChanges();
    };
    AdvanceSearchComponent.prototype.OnDeleteValue = function () {
        var temp = document.getElementById(this.AdvanceQuerySearchFieldsId);
        temp.value = null;
        this.SearchText = null;
        temp.focus();
    };
    AdvanceSearchComponent.prototype.FillPlaceHolder = function () {
        if (!this.SearchText) {
            var temp = document.getElementById(this.AdvanceQuerySearchFieldsId);
            temp.placeholder = "Search";
            temp.style.background = "url(Images/Search.png) no-repeat scroll";
            if (this.LayoutDirection == "rtl") {
                temp.style.backgroundPosition = "left center";
                temp.style.paddingRight = "0px";
                temp.style.paddingLeft = "30px";
            }
            else {
                temp.style.backgroundPosition = "right center";
                temp.style.paddingRight = "30px";
                temp.style.paddingLeft = "0px";
            }
        }
        var ToggleBTN = document.getElementById(this.AdvanceQueryDropButtonId);
        ToggleBTN.className = "ToggleButtonMenu";
    };
    AdvanceSearchComponent.prototype.setToggleButtonMenuTemp = function () {
        var ToggleBTN = document.getElementById(this.AdvanceQueryDropButtonId);
        ToggleBTN.className = "ToggleButtonMenuTemp";
    };
    AdvanceSearchComponent.prototype.setToggleButtonMenu = function () {
        var ToggleBTN = document.getElementById(this.AdvanceQueryDropButtonId);
        ToggleBTN.className = "ToggleButtonMenu";
    };
    AdvanceSearchComponent.prototype.ngOnInit = function () {
        var _this = this;
        //min-height: 163px
        this.Height = {};
        this.QueryChangeEvent.subscribe(function (res) {
            if (_this.QueryId != res.QueryId) {
                _this.SearchText = null;
                _this.OnQueryFilterChanged(res.QueryId);
            }
        });
        //this.textChange.subscribe((res) => {
        //    this.onTextChange(res);
        //});
        this.filterFields = new FilterField_1.FilterFieldsClass(this.isWindowViewMode == undefined ? false : this.isWindowViewMode, this, this.pubSubService);
        this.allFilterFieldsClass = new FilterField_1.FilterFieldsClass(this.isWindowViewMode == undefined ? false : this.isWindowViewMode, this, this.pubSubService);
        this.constantFilterFields = new FilterField_1.FilterFieldsClass(this.isWindowViewMode == undefined ? false : this.isWindowViewMode, this, this.pubSubService);
        //if (this.isWindowViewMode) {
        //    advanceViewBorder.Visibility = Visibility.Collapsed;
        //    windowViewBorder.Visibility = Visibility.Visible;
        //}
        //else {
        //    advanceViewBorder.Visibility = Visibility.Visible;
        //    windowViewBorder.Visibility = Visibility.Collapsed;
        //}
        //// Close the dropdown menu if the user clicks outside of it
        //window.onclick = function (event) {
        //    if (event.target.id != "SearchFields" && event.target.id != "FieldsDropdown" && event.target.id != "AddFilterButton" && event.target.className.indexOf("dropcontent") < 0) {
        //        var dropdowns = document.getElementsByClassName("dropdown-content");
        //        var i;
        //        for (i = 0; i < dropdowns.length; i++) {
        //            var openDropdown = dropdowns[i];
        //            if (openDropdown.classList.contains('showDDButton')) {
        //                openDropdown.classList.remove('showDDButton');
        //            }
        //        }
        //    }
        //}
        this.ObjectTable = window.ObjectTables.filter(function (x) { return x.Name === _this.ObjectTableName; })[0];
        this.ObjectTableId = this.ObjectTable.Id;
        this.ObjectFields = window.ObjectFields.filter(function (d) { return d.ObjectTableId === _this.ObjectTableId && d.CanFilter === true; });
        this.temp = [];
        //this.ObjectFields.forEach((item, key) => {
        //    this.temp.push(new FilterField(item, "", false));
        //});
        this.Run();
    };
    AdvanceSearchComponent.prototype.Run = function () {
        this.HasChanges = false;
        //FilterParametersChangedEvent evt = eventAggregator.GetEvent<FilterParametersChangedEvent>();
        //evt.Subscribe(OnFilterParametersChanged);
        //if (newGrid == null) {
        this.QueryFilterChangedAction(this.QueryId);
        //}
        //if (!loaded) {
        //    loaded = true;
        //}
    };
    AdvanceSearchComponent.prototype.QueryFilterChangedAction = function (QueryID) {
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
            if (myResult == null) {
                _this.AdvancedQueryFilterPMs = [];
            }
            else {
                _this.AdvancedQueryFilterPMs = myResult;
            }
            _this.timeFilterFieldsClass = new FilterField_1.FilterFieldsClass(_this.isNewViewMode, _this, _this.pubSubService);
            _this.FieldsValues = new FilterField_1.FieldsValues();
            _this.currentQuery = window.Queries.filter(function (q) { return q.Id == QueryID; })[0];
            if (!_this.currentQuery) {
                var iMessageWindow = new MessageWindow_1.MessageWindow();
                iMessageWindow.Show("Query not found");
            }
            else {
                //newGrid = new Grid() { Background = new SolidColorBrush(Colors.Transparent) };
                if (_this.currentQuery.UserId != null) {
                    _this.IsUserQuery = true;
                }
                else {
                    _this.IsUserQuery = false;
                }
                if (_this.currentQuery.DisplayAsCustom) {
                }
                if (_this.ObjectFields) {
                    _this.allFilterFields = window.ObjectFields.filter(function (o) { return o.ObjectTableId == _this.ObjectTableId && o.CanFilter == true && o.DataTypeCode != "Constant" && ((o.ValidForQuerySection1 == _this.currentQuery.QuerySection || o.ValidForQuerySection2 == _this.currentQuery.QuerySection) || o.IsCustom == true); });
                    _this.constantFilterFieldsList = window.ObjectFields.filter(function (o) { return o.ObjectTableId == _this.ObjectTableId && o.CanFilter == true && o.DataTypeCode == "Constant" && ((o.ValidForQuerySection1 == _this.currentQuery.QuerySection || o.ValidForQuerySection2 == _this.currentQuery.QuerySection) || o.IsCustom == true); });
                    _this.timeFilterFieldsClass.AddFiltersList(window.ObjectFields.filter(function (o) { return o.ObjectTableId == _this.ObjectTableId && o.CanFilter == true && o.FieldName != "TimeFrameFilter" && o.IsTimeFrameFilter == true && (o.ValidForQuerySection1 == _this.currentQuery.QuerySection || o.ValidForQuerySection2 == _this.currentQuery.QuerySection); }), _this.currentQuery.Id, myResult);
                    var MyFields = window.ObjectFields.filter(function (o) { return o.ObjectTableId == _this.ObjectTableId && o.CanFilter == true && o.DataTypeCode != "Constant" && ((o.ValidForQuerySection1 == _this.currentQuery.QuerySection || o.ValidForQuerySection2 == _this.currentQuery.QuerySection) || o.IsCustom == true); });
                    var MyFilteredFields = [];
                    MyFields.forEach(function (item, key) {
                        var Temp = MyFilteredFields.filter(function (a) { return a.FieldName == item.FieldName; });
                        if (Temp != null && Temp.length == 0) {
                            MyFilteredFields.push(item);
                        }
                    });
                    _this.allFilterFieldsClass.AddFiltersList(MyFilteredFields, _this.currentQuery.Id, myResult);
                }
                else {
                    _this.allFilterFields = [];
                    _this.constantFilterFieldsList = [];
                    _this.allFilterFieldsClass.AddFiltersList([], _this.currentQuery.Id, myResult);
                }
                _this.filterFields.AddFiltersList(_this.allFilterFields, _this.currentQuery.Id, myResult);
                _this.constantFilterFields.AddFiltersList(_this.constantFilterFieldsList, _this.currentQuery.Id, myResult);
                _this.fieldsStaticList = _this.allFilterFieldsClass.FilterFields;
                _this.timeFrameFields = _this.timeFilterFieldsClass.FilterFields; //fieldsStaticList.Where(d => d.ObjectField.IsTimeFrameFilter == true).ToList();
                var OFPM = new ObjectFieldPM_1.ObjectFieldPM();
                OFPM.FieldName = "NoFilter";
                OFPM.FullNameTextCodeCode = "No Filter";
                _this.noFiltersField = new FilterField_1.FilterField(OFPM, _this.currentQuery.Id, _this.isWindowViewMode, myResult);
                _this.timeFrameFields.push(_this.noFiltersField);
                _this.advancedQueryFiltersList = myResult.filter(function (q) { return q.QueryId == QueryID; });
                _this.fillqueryfilters();
            }
        });
    };
    AdvanceSearchComponent.prototype.fillqueryfilters = function () {
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
    AdvanceSearchComponent.prototype.MapJsonToEntityPM = function (jsonPM, mapParent) {
        if (mapParent === void 0) { mapParent = true; }
        if (mapParent === undefined) {
            mapParent = true;
        }
        var entityPM;
        entityPM = new ObjectFieldPM_1.ObjectFieldPM();
        if (jsonPM) {
            var jsonPMKeys = Object.keys(jsonPM);
            for (var key in jsonPMKeys) {
                if (jsonPMKeys[key] === "UIProperties") {
                    continue;
                }
                var property = jsonPMKeys[key];
                entityPM[property] = jsonPM[property];
            }
        }
        entityPM.IsDirty = false;
        return entityPM;
    };
    AdvanceSearchComponent.prototype.AddFilterField = function (field) {
        if (this.SelectedObjectFields == undefined) {
            this.SelectedObjectFields = [];
        }
        var filters = this.AdvancedQueryFilterPMs.filter(function (d) { return d.ObjectFieldId == field.Id; });
        if (filters != null && filters[0] != null && filters[0].IsPredefined == true) {
            var value = this.AdvancedQueryFilterPMs.filter(function (d) { return d.IsPredefined == true && d.ObjectFieldId == field.Id; })[0].PredefinedValue;
            this.FieldsValues.SetFieldValue(field.Id, value);
        }
        if (this.SelectedObjectFields.filter(function (a) { return a.FieldName == field.FieldName; }).length == 0) {
            if (field.DataTypeCode != "Constant") {
                this.SelectedObjectFields.push(new FilterField_1.FilterField(field, this.QueryId, false, filters, this, this.pubSubService));
            }
        }
        if (this.SelectedObjectFields.length % 2 == 0) {
            this.Height = { "min-height": (this.SelectedObjectFields.length / 2 * 27) > 161 ? this.SelectedObjectFields.length / 2 * 27 + "px" : "161px" };
        }
        else {
            this.Height = { "min-height": (this.SelectedObjectFields.length + 1 / 2 * 27) > 161 ? this.SelectedObjectFields.length + 1 / 2 * 27 + "px" : "161px" };
        }
        if (this.SelectedObjectFields.length <= 1) {
            this.Height = { "min-height": "0px" };
        }
        //if (this.AdvancedQueryFilterPMs.filter(d => d.IsPredefined == true && d.ObjectFieldId == field.Id)[0] != null) {
        //    var value = this.AdvancedQueryFilterPMs.filter(d => d.IsPredefined == true && d.ObjectFieldId == field.Id)[0].PredefinedValue;
        //    this.FieldsValues.SetFieldValue(field.Id, value);
        //}
        this.allFilterFieldsClass.SetExists(field, true);
    };
    AdvanceSearchComponent.prototype.RemoveFilterField = function (field) {
        if (this.SelectedObjectFields != null) {
            this.SelectedObjectFields = this.SelectedObjectFields.filter(function (a) { return a.ObjectField.Id != field.Id; });
            if (this.SelectedObjectFields.length % 2 == 0) {
                this.Height = { "min-height": (this.SelectedObjectFields.length / 2 * 27) > 161 ? this.SelectedObjectFields.length / 2 * 27 + "px" : "161px" };
            }
            else {
                this.Height = { "min-height": (this.SelectedObjectFields.length + 1 / 2 * 27) > 161 ? this.SelectedObjectFields.length + 1 / 2 * 27 + "px" : "161px" };
            }
            if (this.SelectedObjectFields.length <= 1) {
                this.Height = { "min-height": "0px" };
            }
        }
    };
    AdvanceSearchComponent.prototype.OnQueryFilterChanged = function (QueryID) {
        this.QueryId = QueryID;
        this.QueryFilterChangedAction(this.QueryId);
        if (this.IsOpened) {
            this.SaveChangesAndRecreate();
        }
    };
    Object.defineProperty(AdvanceSearchComponent.prototype, "SearchText", {
        get: function () { return this.searchText; },
        set: function (newValue) {
            var _this = this;
            this.searchText = newValue;
            if (newValue != null && newValue != "") {
                if (this.isWindowViewMode) {
                    this.allFilterFieldsClass.FilterFields = this.fieldsStaticList.filter(function (f) { return TextCodeTranslator_1.TextCodeTranslator.Translate(f.ObjectField.FullNameTextCodeCode).toLowerCase().indexOf(newValue.toLowerCase()) > -1; });
                    this.SelectedObjectFields.forEach(function (item, key) {
                        _this.allFilterFieldsClass.SetExists(item, true);
                        _this.filterFields.SetExists(item, true);
                    });
                    //foreach(ObjectFieldPM field in CurrentFilters)
                    //{
                    //    filterFields.SetExists(field, true);
                    //}
                }
                else {
                    this.allFilterFieldsClass.FilterFields = this.fieldsStaticList.filter(function (f) { return TextCodeTranslator_1.TextCodeTranslator.Translate(f.ObjectField.FullNameTextCodeCode).toLowerCase().indexOf(newValue.toLowerCase()) > -1; });
                    this.SelectedObjectFields.forEach(function (item, key) {
                        _this.allFilterFieldsClass.SetExists(item, true);
                        _this.filterFields.SetExists(item, true);
                    });
                    //foreach(ObjectFieldPM field in CurrentFilters)
                    //{
                    //    allFilterFieldsClass.SetExists(field, true);
                    //    filterFields.SetExists(field, true);
                    //}
                }
            }
            else {
                this.allFilterFieldsClass.FilterFields = this.fieldsStaticList;
                this.SelectedObjectFields.forEach(function (item, key) {
                    _this.allFilterFieldsClass.SetExists(item, true);
                    _this.filterFields.SetExists(item, true);
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    AdvanceSearchComponent.prototype.SaveChangesAndRecreate = function () {
        //searchControl.SaveChanges();
        //searchControl.SaveChangesCompleted += (ss, ee) => {
        //    DisposeSettingControls();
        //    CreateAdvanceControls();
        //};
    };
    AdvanceSearchComponent.prototype.RunSave = function (field) {
        //isSaving = true;
        //CancelAllOperations();
        var _this = this;
        if (this.isWindowViewMode) { // || isLocalSave
            //this.SelectedObjectFields.forEach((field, key) => {
            if (this.AdvancedQueryFilterPMs.filter(function (f) { return f.ObjectFieldId == field.ObjectField.Id && f.QueryId == _this.QueryId; })[0] != null) {
                var advanceFilter = this.AdvancedQueryFilterPMs.filter(function (f) { return f.ObjectFieldId == field.ObjectField.Id && f.QueryId == _this.QueryId; })[0];
                //if (this.FieldsValues.GetFieldValue(advanceFilter.ObjectFieldId) != null) {
                //    if (field.DataTypeCode == "DateTime" || field.DataTypeCode == "Date") {
                //        string date = this.FieldsValues.GetFieldValue(advanceFilter.ObjectFieldId).ToString();
                //        advanceFilter.PredefinedValue = date;
                //        //string[] datesArr = date.Split(',');
                //        //DateTime date1;
                //        //DateTime date2;
                //        //if (datesArr[0] != null)
                //        //{
                //        //    bool ss = DateTime.TryParse(datesArr[0], out  date1);
                //        //    if (ss)
                //        //    {
                //        //        advanceFilter.PredefinedValue = date1.ToString();
                //        //    }
                //        //    else
                //        //    {
                //        //        advanceFilter.PredefinedValue = null;
                //        //    }
                //        //}
                //        //if (datesArr.Count() > 1)
                //        //{
                //        //    bool ss = DateTime.TryParse(datesArr[1], out  date2);
                //        //    if (ss)
                //        //    {
                //        //        advanceFilter.PredefinedValue2 = date2.ToString();
                //        //    }
                //        //    else
                //        //    {
                //        //        advanceFilter.PredefinedValue2 = null;
                //        //    }
                //        //}
                //    }
                //    else {
                //        string valueString = this.FieldsValues.GetFieldValue(advanceFilter.ObjectFieldId).ToString();
                //        advanceFilter.PredefinedValue = valueString;
                //    }
                //    advanceFilter.IsPredefined = true;
                //}
                //else {
                advanceFilter.PredefinedValue = null;
                advanceFilter.IsPredefined = false;
                //}
            }
            //});
        }
        //this.SelectedObjectFields.forEach((field, key) => {
        if (this.AdvancedQueryFilterPMs.filter(function (f) { return f.ObjectFieldId == field.ObjectField.Id && f.QueryId == _this.QueryId; })[0] == null) {
            var value = this.FieldsValues.GetFieldValue(field.ObjectField.Id);
            var predefinedValue = null;
            var isPredifined = false;
            if (this.isWindowViewMode) { //|| isLocalSave
                if (value != null) {
                    predefinedValue = value.ToString();
                    isPredifined = true;
                }
            }
            var advanceFilter = new AdvancedQueryFilterPM_1.AdvancedQueryFilterPM();
            advanceFilter.Tenant = SessionInfo_1.SessionInfo.LoggedUserTenant,
                advanceFilter.ObjectFieldId = field.ObjectField.Id;
            advanceFilter.QueryId = this.QueryId;
            advanceFilter.DataTypeCode = field.ObjectField.DataTypeCode;
            advanceFilter.DisplayInList = field.ObjectField.DisplayInList;
            advanceFilter.IsCustomFilter = field.ObjectField.IsCustomFilter;
            advanceFilter.ObjectFieldName = field.ObjectField.FieldName;
            advanceFilter.QueryCode = this.currentQuery.Code;
            advanceFilter.QueryObjectTableName = this.currentQuery.ObjectTableName;
            advanceFilter.QueryUserId = this.currentQuery.UserId;
            advanceFilter.ObjectFieldOperator = field.ObjectField.Operator;
            advanceFilter.Operator = field.Operation.Code;
            advanceFilter.PredefinedValue = predefinedValue;
            advanceFilter.IsPredefined = isPredifined;
            advanceFilter.UserId = SessionInfo_1.SessionInfo.LoggedUserId;
            if (field.ObjectField.DataTypeCode == "DateTime" || field.ObjectField.DataTypeCode == "Date") {
                if (this.FieldsValues.GetFieldValue(advanceFilter.ObjectFieldId) != null) {
                    //var date = this.FieldsValues.GetFieldValue(advanceFilter.ObjectFieldId).ToString();
                    //string[] datesArr = date.Split(',');
                    //DateTime date1;
                    //DateTime date2;
                    //if (datesArr[0] != null) {
                    //    bool ss = DateTime.TryParse(datesArr[0], out  date1);
                    //    if (ss) {
                    //        advanceFilter.PredefinedValue = date1.ToString();
                    //    }
                    //    else {
                    //        advanceFilter.PredefinedValue = null;
                    //    }
                    //}
                    //if (datesArr.Count() > 1) {
                    //    bool ss = DateTime.TryParse(datesArr[1], out  date2);
                    //    if (ss) {
                    //        advanceFilter.PredefinedValue2 = date2.ToString();
                    //    }
                    //    else {
                    //        advanceFilter.PredefinedValue2 = null;
                    //    }
                    //}
                }
            }
            var myService = new AdvancedQueryFiltersPMService_1.AdvancedQueryFiltersPMService();
            myService.setServiceArgs(this.serviceArgs);
            myService.insert(advanceFilter).subscribe(function (myResult) {
                _this.AdvancedQueryFilterPMs.push(myResult.Result);
            });
            //generalContext.AdvancedQueryFilterPMs.Add(advanceFilter);
        }
        //});
        //foreach(ObjectFieldPM field in removedFiltersList)
        //{
        //    AdvancedQueryFilterPM filter = (from a in generalContext.AdvancedQueryFilterPMs
        //    where a.QueryId == QueryChangedEventArgs.QueryId && a.ObjectFieldId == field.Id
        //    select a).FirstOrDefault();
        //    if (filter != null) {
        //        generalContext.AdvancedQueryFilterPMs.Remove(filter);
        //    }
        //}
        //if (isLocalSave) {
        //    TenantContext.Current.CurrentSession.StartBusyIndicator("Saving Changes");
        //}
        //TenantContext.Current.CurrentSession.StartBusyIndicator("Saving Changes");
        //HasChanges = false;
        //try {
        //    submitOp = generalContext.SubmitChanges();
        //    submitOp.Completed += new EventHandler(submit_Op_Completed);
        //}
        //catch { TenantContext.Current.CurrentSession.StopBusyIndicator(); }
    };
    AdvanceSearchComponent.prototype.DeteteFilter = function (field) {
        var _this = this;
        //if (this.AdvancedQueryFilterPMs.filter(f => f.ObjectFieldId == field.ObjectField.Id && f.QueryId == this.QueryId)[0] != null) {
        //var advanceFilter = this.AdvancedQueryFilterPMs.filter(f => f.ObjectFieldId == field.ObjectField.Id && f.QueryId == this.QueryId)[0];
        this.myAdvancedQueryFiltersPMService.getuseradvancedqueryfilterbytenantobjecttablequery(SessionInfo_1.SessionInfo.LoggedUserTenant, field.ObjectField.Id, field.QueryId, SessionInfo_1.SessionInfo.LoggedUserId).subscribe(function (filter) {
            if (filter) {
                var myService = new AdvancedQueryFiltersPMService_1.AdvancedQueryFiltersPMService();
                myService.setServiceArgs(_this.serviceArgs);
                myService.delete(filter).subscribe(function (myResult) {
                    if (_this.SelectedObjectFields != null) {
                        _this.SelectedObjectFields = _this.SelectedObjectFields.filter(function (a) { return a.ObjectField.Id != field.ObjectField.Id; });
                    }
                    if (_this.allFilterFieldsClass != null) {
                        //this.allFilterFieldsClass.SetDeleted(field, true);
                        _this.allFilterFieldsClass.SetExists(field, false);
                    }
                    if (_this.SelectedObjectFields.length % 2 == 0) {
                        _this.Height = { "min-height": (_this.SelectedObjectFields.length / 2 * 27) > 161 ? _this.SelectedObjectFields.length / 2 * 27 + "px" : "161px" };
                    }
                    else {
                        _this.Height = { "min-height": (_this.SelectedObjectFields.length + 1 / 2 * 27) > 161 ? _this.SelectedObjectFields.length + 1 / 2 * 27 + "px" : "161px" };
                    }
                    if (_this.SelectedObjectFields.length <= 1) {
                        _this.Height = { "min-height": "0px" };
                    }
                });
            }
            else {
                if (_this.SelectedObjectFields != null) {
                    _this.SelectedObjectFields = _this.SelectedObjectFields.filter(function (a) { return a.ObjectField.Id != field.ObjectField.Id; });
                }
                if (_this.allFilterFieldsClass != null) {
                    //this.allFilterFieldsClass.SetDeleted(field, true);
                    _this.allFilterFieldsClass.SetExists(field, false);
                }
                if (_this.SelectedObjectFields.length % 2 == 0) {
                    _this.Height = { "min-height": (_this.SelectedObjectFields.length / 2 * 27) > 161 ? _this.SelectedObjectFields.length / 2 * 27 + "px" : "161px" };
                }
                else {
                    _this.Height = { "min-height": (_this.SelectedObjectFields.length + 1 / 2 * 27) > 161 ? _this.SelectedObjectFields.length + 1 / 2 * 27 + "px" : "161px" };
                }
                if (_this.SelectedObjectFields.length <= 1) {
                    _this.Height = { "min-height": "0px" };
                }
            }
        });
        //}
    };
    AdvanceSearchComponent.prototype.EditViewClicked = function () {
        var _this = this;
        var windowArgs = {};
        windowArgs.queryId = this.currentQuery.Id;
        windowArgs.currentObjectTable = this.ObjectTableName;
        windowArgs.IsNew = false;
        windowArgs.QueryName = TextCodeTranslator_1.TextCodeTranslator.Translate(this.currentQuery.NameTextCodeCode);
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 960;
        logitudeWindow.Height = 610;
        logitudeWindow.Title = "Edit View";
        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./Infrastructure/Components/NewViewComponent/NewViewComponent');
        logitudeWindow.WindowClosed.subscribe(function ($event) {
            var ObjectTable = window.ObjectTables.filter(function (x) { return x.Name === _this.ObjectTableName; })[0];
            var Query = window.Queries.filter(function (a) { return a.ObjectTableId === ObjectTable.Id && a.Id == $event; })[0];
            // this.QueriesChangedEvent.emit(Query);
        });
    };
    AdvanceSearchComponent.prototype.FiltersBtnClicked = function () {
        var temp = document.getElementById(this.AdvanceQuerySearchFieldsId);
        temp.value = "";
        this.SearchText = "";
        //temp.focus();
        //this.ClearPlaceHolder(); 
        //var ActiveElement = document.activeElement;
        //ActiveElement.blur();
        console.log(temp);
        temp.focus();
        temp.focus();
        temp.select();
        //console.log(document.activeElement); 
    };
    AdvanceSearchComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'AdvSearchComponent',
            templateUrl: './AdvanceSearchComponent.html',
            inputs: ['ObjectTableName', 'QueryChangeEvent', 'isWindowViewMode', 'isNewViewMode', 'QueryId', 'Filterchangeevent', 'rabaia'],
            providers: [http_1.Http],
        }),
        __metadata("design:paramtypes", [forms_1.FormBuilder, ApiFiltersEvent_1.PubSubService, core_1.ChangeDetectorRef])
    ], AdvanceSearchComponent);
    return AdvanceSearchComponent;
}());
exports.AdvanceSearchComponent = AdvanceSearchComponent;
//# sourceMappingURL=AdvanceSearchComponent.js.map