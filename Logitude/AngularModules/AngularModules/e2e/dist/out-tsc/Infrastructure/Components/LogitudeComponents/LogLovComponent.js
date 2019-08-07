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
var EntityListService_1 = require("../../Services/EntityListService");
var ServiceArgs_1 = require("../../DataContracts/ServiceArgs");
var ApiQueryFilters_1 = require("../../DataContracts/ApiQueryFilters");
var ControlsIdCounter_1 = require("../../Utilities/ControlsIdCounter");
var EntityResourceService_1 = require("../../Services/EntityResourceService");
var Tools_1 = require("../../Tools");
var TextCodeTranslator_1 = require("../../Utilities/TextCodeTranslator");
var ServiceResponse_1 = require("../../DataContracts/ServiceResponse");
var FieldValidator_1 = require("../../Validators/FieldValidator");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var forms_1 = require("@angular/forms");
var LogSearchWindowComponent_1 = require("./LogSearchWindowComponent");
var LogLovComponent = /** @class */ (function () {
    //@Input() SelectedValue: any;
    function LogLovComponent(entityListService, _entityResourceService) {
        this.entityListService = entityListService;
        this._entityResourceService = _entityResourceService;
        this.ShowHelp = false;
        this.ObjectFieldName = null;
        this.ObjectFieldHelp = null;
        this.ObjectTableName = null;
        this.LookUpTableName = null;
        this.HideColumns = false;
        this.HideLastColumn = false;
        this.IsReady = false;
        this.ValueChanged = new core_1.EventEmitter();
        this.SelectedItemChanged = new core_1.EventEmitter();
        this.show = false;
        this.isFirstTime = true;
        this.Detach = true;
    }
    Object.defineProperty(LogLovComponent.prototype, "IsDisabled", {
        get: function () {
            return this.isDisabled;
        },
        set: function (newValue) {
            this.isDisabled = newValue;
            if (this.isDisabled) {
                this.imgNgStyle = { 'opacity': .5, 'pointer-events': 'none' };
            }
            else {
                this.imgNgStyle = { 'opacity': 1, 'pointer-events': 'all' };
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogLovComponent.prototype, "SelectedValue", {
        get: function () {
            return this.selectedValue;
        },
        set: function (newValue) {
            var _this = this;
            if (this.selectedValue != newValue) {
                this.selectedValue = newValue;
                if (!this.isSelectedFromList) { //&& !this.isFirstTime) {
                    if (this.uiProperty != null) {
                        this.uiProperty.UIPropertyChanged.emit("valuechanges");
                    }
                    this.ValueChanged.emit(this.selectedValue);
                    //if (!this.isSelectedFromList && !this.isFirstTime) {
                    var lookup = window.ObjectTables.filter(function (d) { return d.Name === _this.LookUpTableName; })[0];
                    //this.DataContext[this.ObjectFieldName] = this.selectedValue;
                    this.GetSingle(lookup);
                    //}
                    this.isFirstTime = false;
                    this.isSelectedFromList = false;
                }
                this.isFirstTime = false;
                this.isSelectedFromList = false;
            }
        },
        enumerable: true,
        configurable: true
    });
    LogLovComponent.prototype.ngAfterViewChecked = function () {
    };
    LogLovComponent.prototype.ngOnInit = function () {
        this.InitializeControl();
    };
    LogLovComponent.prototype.InitializeControl = function () {
        var _this = this;
        this.LogLOVControlClass = "LogLOVControl";
        this.counterId = ControlsIdCounter_1.ControlsIdCounter.GetNextIdCounter();
        this.DivLogLovId = 'LogLov - ' + this.ObjectFieldName + '-' + this.counterId.toString();
        this.ElementId = 'Search - ' + this.ObjectFieldName + '-' + this.counterId.toString();
        this.DropdownId = 'LogLovDropDown-' + this.ObjectFieldName + '-' + this.counterId.toString();
        this.ErrorPopUpId = 'loglovererrorpop_' + this.counterId;
        var objectFieldAvailable = true;
        var lookup = window.ObjectTables.filter(function (d) { return d.Name === _this.LookUpTableName; })[0];
        var table = window.ObjectTables.filter(function (d) { return d.Name === _this.ObjectTableName; })[0];
        if (lookup.AutoCompleteSearchWindow) {
            this.ShowSearchButton = true;
        }
        else {
            this.ShowSearchButton = false;
        }
        this.LookUp1 = lookup.LookUp1;
        this.LookUp2 = lookup.LookUp2;
        this.headerColumns = [];
        this.dataColumns = [];
        if (this.LookUpTableName == 'Port' || this.LookUpTableName == 'Carrier') {
            this.IsAllDataVisible = true;
            this.MyDropDownHeight = { 'height': '100px' };
            this.DisplayHeader = false;
            if (this.LookUpTableName == 'Port') {
                this.AllDataHeaderTitle = 'All Ports';
                this.MyDataHeaderTitle = 'My Ports';
            }
            else {
                this.AllDataHeaderTitle = 'All Carriers';
                this.MyDataHeaderTitle = 'My Carriers';
            }
        }
        else {
            this.IsAllDataVisible = false;
        }
        if (!this.PlaceHolder) {
            this.PlaceHolder = '';
        }
        if (!this.DisplayMemberPath) {
            if (lookup.LookUp2) {
                this.DisplayMemberPath = lookup.LookUp2;
            }
            else {
                this.DisplayMemberPath = lookup.LookUp1;
            }
        }
        if (!this.SelectedValuePath) {
            this.SelectedValuePath = lookup.KeyPropertyPath;
        }
        this.uiProperty = this.DataContext.UIProperties.GetUIProperty(this.ObjectFieldName, this.ObjectTableName, this.DataContext);
        this.IsDisabled = !this.uiProperty.IsEnabled;
        //this.ctrl = new FormControl(this.DataContext[this.ObjectFieldName]);
        //this.LogitudeForm.addControl(this.ObjectFieldName, this.ctrl);
        this._entityResourceService.getEntityResourceByTableName(this.LookUpTableName, 0).subscribe(function (res) {
            var lookupFields = window.ObjectFields.filter(function (d) { return d.DisplayOnLookUp && d.ObjectTableId == lookup.Id; });
            var dropdownWidth = lookupFields.length * 120 + 20;
            _this.DropPopUpStyle = { width: dropdownWidth.toString() + 'px' };
            var additionalCols = lookupFields.filter(function (d) { return d.DisplayInLookUpIndex > 1; });
            if (lookupFields.length == 1) {
                _this.DisplayHeader = false;
            }
            else if (!_this.IsAllDataVisible) {
                _this.DisplayHeader = true;
            }
            _this.IsReady = true;
            for (var i = 0; i < lookupFields.length; i++) {
                var width = 100;
                if (lookupFields[i].DisplayInLookupColumnSize) {
                    width = lookupFields[i].DisplayInLookupColumnSize;
                }
                else {
                    if (lookupFields[i].DisplayInLookUpIndex == 0) {
                        if (!additionalCols || additionalCols.length == 0 || additionalCols.length == 1) {
                            width = 120;
                        }
                        else {
                            width = 70;
                        }
                    }
                    if (lookupFields[i].DisplayInLookUpIndex == 1) {
                        if (!additionalCols || additionalCols.length == 0 || additionalCols.length == 1) {
                            width = 120;
                        }
                        else {
                            width = 100;
                        }
                    }
                    if (lookupFields[i].DisplayInLookUpIndex > 1) {
                        if (additionalCols.length == 1) {
                            width = 95;
                        }
                        if (additionalCols.length > 1) {
                            width = (350 - 170) / additionalCols.length;
                        }
                    }
                }
                _this.headerColumns.push({
                    Display: TextCodeTranslator_1.TextCodeTranslator.Translate(lookupFields[i].ListTextCodeCode),
                    Width: width,
                    Field: lookupFields[i].FieldName,
                    Index: lookupFields[i].DisplayInLookUpIndex,
                });
                _this.dataColumns.push({ Field: lookupFields[i].FieldName });
            }
            if (table) {
                _this.ObjectField = window.ObjectFields.filter(function (d) { return d.ObjectTableId === table.Id && d.FieldName === _this.ObjectFieldName; })[0];
                if (!_this.ObjectField) {
                    objectFieldAvailable = false;
                }
                else {
                    objectFieldAvailable = true;
                    if (_this.ObjectField.HelpTextCodeId != null) {
                        _this.ObjectFieldHelp = TextCodeTranslator_1.TextCodeTranslator.Translate(_this.ObjectField.HelpTextTextCodeCode);
                        if (!Tools_1.AppTool.IsNullOrEmpty(_this.ObjectFieldHelp)) {
                            if (_this.ObjectFieldHelp.length > 1) {
                                _this.ShowHelp = true;
                            }
                        }
                    }
                    if (_this.ObjectField.DependencyFilter1Value) {
                        if (_this.ObjectField.DependencyFilter1Type == "Constant") {
                            _this.DependencyFilter1Value = _this.ObjectField.DependencyFilter1Value;
                            _this.DependencyFilter1IsList = _this.ObjectField.DependencyFilter1IsList;
                        }
                        else {
                            _this.DependencyFilter1Value = _this.DataContext[_this.ObjectField.DependencyFilter1Value];
                        }
                    }
                    if (_this.ObjectField.DependencyFilter2Value) {
                        if (_this.ObjectField.DependencyFilter2Type == "Constant") {
                            _this.DependencyFilter2Value = _this.ObjectField.DependencyFilter2Value;
                            _this.DependencyFilter2IsList = _this.ObjectField.DependencyFilter2IsList;
                        }
                        else {
                            _this.DependencyFilter2Value = _this.DataContext[_this.ObjectField.DependencyFilter2Value];
                        }
                    }
                }
            }
            //
            //if (table) {
            //    var field = window.ObjectFields.filter(d => d.ObjectTableId === table.Id && d.FieldName === this.ObjectFieldName)[0];
            //    if (!field) {
            //        objectFieldAvailable = false;
            //    }
            //}
            //else {
            //    objectFieldAvailable = false;
            //}
            if (_this.DataContext[_this.ObjectFieldName]) {
                _this.GetSingle(lookup);
            }
            //this.ctrl.valueChanges.subscribe(res=> {
            //    this.uiProperty.UIPropertyChanged.emit("valuechanges");
            //    this.ValueChanged.emit(res);
            //    if (!this.isSelectedFromList && !this.isFirstTime) {
            //        this.DataContext[this.ObjectFieldName] = res;
            //        this.GetSingle(lookup);
            //    }
            //    this.isFirstTime = false;
            //    this.isSelectedFromList = false;
            //});
            _this.SearchTextValue = new forms_1.FormControl();
            _this.SearchTextValue.valueChanges
                .debounceTime(400)
                .distinctUntilChanged()
                .subscribe(function (search) {
                if (search != undefined) {
                    _this.Populate(search);
                }
            });
            _this.uiProperty.UIPropertyChanged.subscribe(function (value) {
                if (value != "valuechanges") {
                    var uiProperty = value;
                    _this.IsDisabled = !_this.uiProperty.IsEnabled;
                    if (objectFieldAvailable) {
                        //this.SetControlPropertiesAndValidations(uiProperty, this.ctrl)
                    }
                }
                _this.DetectChanges();
            });
            if (objectFieldAvailable) {
                //this.SetControlPropertiesAndValidations(this.uiProperty, this.ctrl);
            }
        });
    };
    LogLovComponent.prototype.GetSingle = function (lookup) {
        var _this = this;
        if (this.DataContext[this.ObjectFieldName]) {
            var apiFilters = new ApiQueryFilters_1.ApiQueryFilters();
            if (lookup.CacheOnClient) {
                this.entityListService.getSingleFromCache(this.DataContext[this.ObjectFieldName], this.LookUpTableName, apiFilters).then(function (res) {
                    res.subscribe(function (myResponse) {
                        if (myResponse != null) {
                            var list = myResponse;
                            if (myResponse instanceof ServiceResponse_1.ServiceResponse) {
                                list = myResponse.Result;
                            }
                            _this.DisplayValue = list[_this.DisplayMemberPath];
                            _this.SelectedItem = list;
                            _this.SelectedItemObject = _this.SelectedItem;
                            _this.SelectedItemChanged.emit(_this.SelectedItem);
                            _this.DetectChanges();
                        }
                    });
                });
            }
            else {
                this.entityListService.getSingle(this.DataContext[this.ObjectFieldName], this.LookUpTableName).then(function (res) {
                    res.subscribe(function (myResponse) {
                        if (myResponse != null) {
                            var list = myResponse;
                            if (myResponse instanceof ServiceResponse_1.ServiceResponse) {
                                list = myResponse.Result;
                            }
                            _this.DisplayValue = list[_this.DisplayMemberPath];
                            _this.SelectedItem = list;
                            _this.SelectedItemObject = _this.SelectedItem;
                            _this.SelectedItemChanged.emit(_this.SelectedItem);
                            _this.DetectChanges();
                        }
                    });
                });
            }
        }
        else {
            this.DisplayValue = null;
            this.SelectedItem = null;
            this.SelectedItemObject = null;
            this.SelectedItemChanged.emit(this.SelectedItem);
            this.DetectChanges();
        }
    };
    LogLovComponent.prototype.OnWindowClick = function ($event) {
        if (!this.IsOpen) {
            var shownDropDowns = document.getElementsByClassName("show");
            var input = document.getElementById(this.ElementId);
            var dropdown = input.parentElement.parentElement;
            for (var i = 0; i < shownDropDowns.length; i++) {
                if (shownDropDowns[i]) {
                    shownDropDowns[i].classList.add("hide");
                    shownDropDowns[i].classList.remove("show");
                }
            }
        }
    };
    LogLovComponent.prototype.SetControlPropertiesAndValidations = function (uiProperty, ctrl) {
        this.uiProperty.IsRequired = uiProperty.IsRequired;
        var table = window.ObjectTables.filter(function (d) { return d.Name === uiProperty.ObjectTableName; })[0];
        var field = window.ObjectFields.filter(function (d) { return d.ObjectTableId === table.Id && d.FieldName === uiProperty.FieldName; })[0];
        if (field) {
            var minlength = field.MinLength;
            var maxlenght = field.MaxLength;
            var hasminmax;
            hasminmax = false;
            if (field.DataTypeCode.toLowerCase() == "text" || field.DataTypeCode.toLowerCase() == "ntext") {
                if (maxlenght != 0) {
                    hasminmax = true;
                }
            }
        }
        if (this.uiProperty.IsRequired) {
            if (this.DataContext[this.ObjectFieldName] == null || this.DataContext[this.ObjectFieldName] == "") {
                this.ctrl.setErrors({ "required": true });
            }
            if (hasminmax) {
                this.ctrl.validator = forms_1.Validators.compose([forms_1.Validators.required, forms_1.Validators.minLength(minlength), forms_1.Validators.maxLength(maxlenght)]);
            }
            else {
                this.ctrl.validator = forms_1.Validators.required;
            }
        }
        else if (!this.uiProperty.ValidValue) {
            this.ctrl.setErrors({ "error": this.uiProperty.ValidationError });
        }
        else {
            this.ctrl.setErrors(null);
            if (hasminmax) {
                this.ctrl.validator = forms_1.Validators.compose([forms_1.Validators.minLength(minlength), forms_1.Validators.maxLength(maxlenght)]);
            }
            else {
                this.ctrl.validator = null;
            }
        }
    };
    LogLovComponent.prototype.OnLogLovFocus = function () {
        this.ContainerClass = 'chosen-container chosen-container-single chosen-container-active';
    };
    LogLovComponent.prototype.OnLogLovKeyDown = function ($event) {
        var TABKEY = 9;
        var SHIFTKEY = 16;
        var DELETEKEY = 46;
        if ($event.keyCode == TABKEY) {
            if (this.IsOpen) {
                this.ToggleOpenDropDown();
            }
        }
        else if ($event.keyCode == DELETEKEY) {
            this.OnDeleteValue();
            this.ToggleOpenDropDown();
        }
        else if ($event.keyCode != SHIFTKEY) {
            this.ToggleOpenDropDown();
            if (this.IsOpen) {
                this.Populate(null);
            }
        }
    };
    LogLovComponent.prototype.OnSelected = function (item) {
        this.isSelectedFromList = true;
        this.SelectedItem = item;
        this.DataContext[this.ObjectFieldName] = this.SelectedItem[this.SelectedValuePath];
        this.SelectedItemObject = this.SelectedItem;
        this.SelectedItemChanged.emit(this.SelectedItem);
        this.DetectChanges();
        this.DisplayValue = this.SelectedItem[this.DisplayMemberPath];
        this.ValidateField();
        this.ToggleOpenDropDown();
        this.ValueChanged.emit(this.DataContext[this.ObjectFieldName]);
    };
    LogLovComponent.prototype.OnLogLovClicked = function () {
        this.ToggleOpenDropDown();
        if (this.IsOpen) {
            this.Populate(null);
        }
    };
    LogLovComponent.prototype.ToggleOpenDropDown = function () {
        this.IsDropDownVisible = !this.IsDropDownVisible;
        this.IsOpen = !this.IsOpen;
        if (this.IsOpen) {
            this.SearchTextNgModel = "";
        }
        this.DetectChanges();
    };
    LogLovComponent.prototype.OnLogLovBlur = function () {
        this.ContainerClass = 'chosen-container chosen-container-single';
    };
    LogLovComponent.prototype.OnSearchIputKeyDown = function ($event) {
        var TABKEY = 9;
        var ENTERKEY = 13;
        var DOWNKEY = 40;
        var UPKEY = 38;
        var ESC = 27;
        if ($event.keyCode == TABKEY) {
            var active = document.getElementsByClassName("highlighted");
            if (active[0]) {
                var input = document.getElementById(this.ElementId);
                var lis = input.parentElement.parentElement.parentElement.getElementsByTagName("li");
                for (var i = 0; i < lis.length; i++) {
                    if (lis[i].className.search("highlighted") > -1) {
                        lis[i].classList.remove("highlighted");
                        lis[i].click();
                    }
                }
            }
            if (this.IsOpen) {
                this.ToggleOpenDropDown();
            }
        }
        if ($event.keyCode == ENTERKEY) {
            var active = document.getElementsByClassName("highlighted");
            if (active[0]) {
                var input = document.getElementById(this.ElementId);
                var lis = input.parentElement.parentElement.parentElement.getElementsByTagName("li");
                for (var i = 0; i < lis.length; i++) {
                    if (lis[i].className.search("highlighted") > -1) {
                        lis[i].classList.remove("highlighted");
                        lis[i].click();
                    }
                }
            }
        }
        if ($event.keyCode == DOWNKEY) {
            var active = document.getElementsByClassName("highlighted");
            if (!active[0]) {
                var input = document.getElementById(this.ElementId);
                var lis = input.parentElement.parentElement.parentElement.getElementsByTagName("li");
                lis[0].classList.add("highlighted");
            }
            else {
                active[0].nextElementSibling.classList.add("highlighted");
                active[0].classList.remove("highlighted");
                active[0].scrollIntoView(false);
            }
        }
        if ($event.keyCode == UPKEY) {
            var active = document.getElementsByClassName("highlighted");
            if (active[0]) {
                active[0].previousElementSibling.classList.add("highlighted");
                active = document.getElementsByClassName("highlighted");
                active[1].classList.remove("highlighted");
                active[0].scrollIntoView(false);
            }
        }
        if ($event.keyCode == ESC) {
            if (this.IsOpen) {
                this.ToggleOpenDropDown();
            }
        }
    };
    LogLovComponent.prototype.OnLiMouseOver = function ($event) {
        var active = document.getElementsByClassName("highlighted");
        if (active[0]) {
            active[0].classList.remove("highlighted");
        }
        if ($event.target.tagName != "DIV") {
            $event.target.classList.add("highlighted");
        }
        else {
            $event.target.parentElement.parentElement.classList.add("highlighted");
        }
    };
    LogLovComponent.prototype.OnLiMouseLeave = function ($event) {
        $event.target.classList.remove("highlighted");
    };
    LogLovComponent.prototype.Populate = function (searchText) {
        var _this = this;
        var filters = new ApiQueryFilters_1.ApiQueryFilters();
        filters.SortDirection = "Ascending";
        if (this.IsTenantZeroSearch) {
            filters.Tenant = 0;
        }
        if (searchText) {
            filters.addAdditionalFilter("SearchFields", searchText, null, null, "Contains", false, false, false, null);
            //filters.Filter1Name = "SearchFields";
            //filters.Filter1Operator = "Contains";
            //filters.Filter1Value = searchText;
        }
        this.SetDependencyProperties(filters);
        filters.PageIndex = 0;
        filters.PageSize = 50;
        var lookup = window.ObjectTables.filter(function (d) { return d.Name === _this.LookUpTableName; })[0];
        if (lookup.SortingByObjectField) {
            filters.SortBy = lookup.SortingByObjectField;
        }
        var inactiveField = window.ObjectFields.filter(function (d) { return d.FieldName.toLowerCase() === "inactive" && d.ObjectTableId === lookup.Id; })[0];
        if (inactiveField && !this.ShowInActive) {
            filters.addAdditionalFilter(inactiveField.FieldName, false, null, null, "Equals", false, false, false, null);
            //filters.Filter4Name = inactiveField.FieldName;
            //filters.Filter4Operator = "Equals";
            //filters.Filter4Value = false;
        }
        if (!lookup.CacheOnClient || this.IsTenantZeroSearch) {
            this.entityListService.getByFilters(this.LookUpTableName, filters).then(function (res) {
                res.subscribe(function (resp) {
                    if (resp.Result) {
                        _this.ItemsSource = resp.Result;
                    }
                    else {
                        _this.ItemsSource = resp;
                    }
                    _this.ItemsSourceStatic = _this.ItemsSource;
                    _this.DetectChanges();
                    //if (searchText != null) {
                    //this.ItemsSource = this.ItemsSource.filter(d=> d[this.DisplayMemberPath].toLowerCase().startsWith(searchText.toLowerCase()));
                    //}
                });
            });
        }
        else {
            this.entityListService.getAllFromCache(this.LookUpTableName, filters).then(function (res) {
                res.subscribe(function (resp) {
                    if (resp.Result) {
                        _this.ItemsSource = resp.Result;
                    }
                    else {
                        _this.ItemsSource = resp;
                    }
                    _this.ItemsSourceStatic = _this.ItemsSource;
                    _this.DetectChanges();
                    //if (searchText != null) {
                    //    this.ItemsSource = this.ItemsSource.filter(d=> d[this.DisplayMemberPath].toLowerCase().startsWith(searchText.toLowerCase()));
                    //}
                });
            });
        }
        if (this.IsAllDataVisible) {
            var tenantZeroFilters = new ApiQueryFilters_1.ApiQueryFilters();
            tenantZeroFilters.SortDirection = "Ascending";
            if (lookup.SortingByObjectField) {
                tenantZeroFilters.SortBy = lookup.SortingByObjectField;
            }
            tenantZeroFilters.Tenant = 0;
            if (searchText) {
                tenantZeroFilters.addAdditionalFilter("SearchFields", searchText, null, null, "Contains", false, false, false, null);
                //tenantZeroFilters.Filter1Name = "SearchFields";
                //tenantZeroFilters.Filter1Operator = "Contains";
                //tenantZeroFilters.Filter1Value = searchText;
            }
            if (inactiveField && !this.ShowInActive) {
                tenantZeroFilters.addAdditionalFilter(inactiveField.FieldName, false, null, null, "Equals", false, false, false, null);
                //tenantZeroFilters.Filter4Name = inactiveField.FieldName;
                //tenantZeroFilters.Filter4Operator = "Equals";
                //tenantZeroFilters.Filter4Value = false;
            }
            this.SetDependencyProperties(tenantZeroFilters);
            tenantZeroFilters.PageIndex = 0;
            tenantZeroFilters.PageSize = 50;
            this.entityListService.getByFilters(this.LookUpTableName, tenantZeroFilters).then(function (res) {
                res.subscribe(function (resp) {
                    if (resp.Result) {
                        _this.ZeroItemsSource = resp.Result;
                    }
                    else {
                        _this.ZeroItemsSource = resp;
                    }
                    _this.DetectChanges();
                });
            });
        }
    };
    LogLovComponent.prototype.OnSearchInputBlur = function () {
        if (!this.MouseInArea) {
            if (this.IsOpen) {
                this.ToggleOpenDropDown();
            }
        }
    };
    LogLovComponent.prototype.OnMouseOver = function () {
        this.MouseInArea = true;
        if (!this.SelectedItem) {
            this.DeleteButtonNgStyle = { 'visibility': 'hidden' };
        }
        else {
            this.DeleteButtonNgStyle = null;
        }
        this.DetectChanges();
    };
    LogLovComponent.prototype.OnMouseOut = function () {
        this.MouseInArea = false;
        this.DetectChanges();
    };
    LogLovComponent.prototype.SetDependencyProperties = function (apiQueryFilters) {
        var _this = this;
        var table = window.ObjectTables.filter(function (d) { return d.Name === _this.LookUpTableName; })[0];
        if (table.DependencyFilter1 != null && table.DependencyFilter1 != undefined) {
            if (this.DependencyFilter1Value != null && this.DependencyFilter1Value != undefined) {
                var dependencyField = window.ObjectFields.filter(function (d) { return d.ObjectTableId === table.Id && d.FieldName === table.DependencyFilter1; })[0];
                if (dependencyField != null) {
                    var fieldOperator = dependencyField.IsCustomFilter ? "Contains" : "Equals";
                    if (this.DependencyFilter1IsList) {
                        fieldOperator = "InList";
                    }
                    if (this.DependencyFilter1IsListExact) {
                        fieldOperator = "InListExact";
                    }
                    //var depValue1: any = resolver.ResolveValue(dependencyField.DataTypeCode, DependencyProperty1 == null ? null : DependencyProperty1.ToString());
                    //apiQueryFilters.SetFilter(table.DependencyFilter1, depValue1, dependencyField.IsCustomFilter, fieldOperator, null, dependencyField.DisplayInList);
                    apiQueryFilters.addAdditionalFilter(dependencyField.FieldName, this.GetFieldValue(dependencyField.DataTypeCode, this.DependencyFilter1Value), null, null, fieldOperator, dependencyField.IsCustomFilter, dependencyField.DisplayInList, dependencyField.IsCustom, dependencyField.DataTypeCode);
                    //apiQueryFilters.Filter2Name = dependencyField.FieldName;
                    //apiQueryFilters.Filter2Operator = fieldOperator;
                    //apiQueryFilters.Filter2Value = this.GetFieldValue(dependencyField.DataTypeCode, this.DependencyFilter1Value);
                }
            }
        }
        if (table.DependencyFilter2 != null && table.DependencyFilter2 != undefined) {
            if (this.DependencyFilter2Value != null && this.DependencyFilter2Value != undefined) {
                var dependencyField2 = window.ObjectFields.filter(function (d) { return d.ObjectTableId === table.Id && d.FieldName === table.DependencyFilter2; })[0];
                if (dependencyField2 != null) {
                    var fieldOperator = dependencyField2.IsCustomFilter ? "Contains" : "Equals";
                    if (this.DependencyFilter2IsList) {
                        fieldOperator = "InList";
                    }
                    if (this.DependencyFilter2IsListExact) {
                        fieldOperator = "InListExact";
                    }
                    //var depValue1: any = resolver.ResolveValue(dependencyField.DataTypeCode, DependencyProperty1 == null ? null : DependencyProperty1.ToString());
                    //apiQueryFilters.SetFilter(table.DependencyFilter1, depValue1, dependencyField.IsCustomFilter, fieldOperator, null, dependencyField.DisplayInList);
                    apiQueryFilters.addAdditionalFilter(dependencyField2.FieldName, this.GetFieldValue(dependencyField2.DataTypeCode, this.DependencyFilter2Value), null, null, fieldOperator, dependencyField2.IsCustomFilter, dependencyField2.DisplayInList, dependencyField2.IsCustom, dependencyField2.DataTypeCode);
                    //apiQueryFilters.Filter3Name = dependencyField2.FieldName;
                    //apiQueryFilters.Filter3Operator = fieldOperator;
                    //apiQueryFilters.Filter3Value = this.GetFieldValue(dependencyField2.DataTypeCode, this.DependencyFilter2Value);
                }
            }
        }
    };
    LogLovComponent.prototype.GetFieldValue = function (dataTypeCode, value) {
        if (value == null || value == undefined) {
            return null;
        }
        switch (dataTypeCode.toLowerCase()) {
            case "ntext":
            case "text":
                {
                    return value;
                }
            case "datetime":
                {
                    var date;
                    date = value;
                    return date;
                }
            case "boolean":
                {
                    var bb;
                    bb = Boolean(value);
                    return bb;
                }
            case "integer":
            case "double":
            case "decimal":
                {
                    return value;
                }
            default:
                {
                    return value;
                }
        }
    };
    LogLovComponent.prototype.FocusMe = function () {
        var input = document.getElementById(this.ElementId);
        var logLovCtrl = input.parentElement.parentElement.parentElement.getElementsByTagName("a");
        logLovCtrl[0].focus();
    };
    LogLovComponent.prototype.OnAllDataSelect = function (item) {
        var _this = this;
        this.entityListService.getEntityCopyToCurrentTenant(item.Id, this.LookUpTableName).then(function (res) {
            res.subscribe(function (myResponse) {
                if (myResponse != null) {
                    var list = myResponse;
                    if (myResponse instanceof ServiceResponse_1.ServiceResponse) {
                        list = myResponse.Result;
                    }
                    _this.isSelectedFromList = true;
                    _this.SelectedItem = list;
                    _this.DataContext[_this.ObjectFieldName] = _this.SelectedItem[_this.SelectedValuePath];
                    _this.SelectedItemObject = _this.SelectedItem;
                    _this.SelectedItemChanged.emit(_this.SelectedItem);
                    _this.DetectChanges();
                    _this.DisplayValue = _this.SelectedItem[_this.DisplayMemberPath];
                    _this.ToggleOpenDropDown();
                }
            });
        });
    };
    LogLovComponent.prototype.OnDropDownMouseOver = function () {
        this.MouseInArea = true;
        var input = document.getElementById(this.ElementId);
        input.focus();
    };
    LogLovComponent.prototype.OnDropDownMouseOut = function () {
        this.MouseInArea = false;
    };
    LogLovComponent.prototype.OnDeleteValue = function () {
        this.SelectedItem = null;
        this.DataContext[this.ObjectFieldName] = null;
        this.SelectedItemObject = null;
        this.SelectedItemChanged.emit(this.SelectedItem);
        this.DetectChanges();
        this.DisplayValue = null;
        this.ValidateField();
        this.ValueChanged.emit(null);
    };
    LogLovComponent.prototype.SetValidity = function (validValue, errorMessage) {
        this.uiProperty.ValidValue = validValue;
        this.uiProperty.ValidationError = errorMessage;
        if (!validValue) {
            this.InputDivStyle = { 'border': '1px solid #ff0000' };
            if (this.show) {
                this.ShowErrorPopup = true;
            }
        }
        else {
            this.ShowErrorPopup = false;
            if (this.show) {
                this.InputDivStyle = { 'border': '1px solid #3BB3E2' };
            }
            else {
                this.InputDivStyle = null;
            }
        }
    };
    LogLovComponent.prototype.SearchButtonClicked = function () {
        var _this = this;
        var ObjectTable = window.ObjectTables.filter(function (x) { return x.Name === _this.LookUpTableName; })[0];
        var ObjectTableId = window.ObjectTables.filter(function (x) { return x.Name === _this.LookUpTableName; })[0].Id;
        var args = new LogSearchWindowComponent_1.CustomEntityArgs();
        args.ObjectTableId = ObjectTableId;
        args.ObjectTableName = this.LookUpTableName;
        args.IsTenantZeroSearch = this.IsAllDataVisible;
        args.DependencyFilter1Value = this.DependencyFilter1Value;
        args.DependencyFilter1IsList = this.DependencyFilter1IsList;
        args.DependencyFilter1IsListExact = this.DependencyFilter1IsListExact;
        args.DependencyFilter2Value = this.DependencyFilter2Value;
        args.DependencyFilter2IsList = this.DependencyFilter2IsList;
        args.DependencyFilter2IsListExact = this.DependencyFilter2IsListExact;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 800;
        logitudeWindow.Height = 600;
        logitudeWindow.WindowArgs = args;
        logitudeWindow.Title = TextCodeTranslator_1.TextCodeTranslator.TranslateTablePlural(this.LookUpTableName) + " Search";
        logitudeWindow.Show('./Infrastructure/Components/LogitudeComponents/LogSearchWindowComponent');
        logitudeWindow.WindowClosed.subscribe(function ($event) { return _this.OnSearchWindowClosed($event); });
    };
    LogLovComponent.prototype.OnSearchWindowClosed = function (args) {
        if (args && args != 'event') { // No value returned
            var item = args.SelectedItem;
            this.isSelectedFromList = true;
            this.SelectedItem = item;
            this.DataContext[this.ObjectFieldName] = this.SelectedItem[this.SelectedValuePath];
            this.SelectedItemObject = this.SelectedItem;
            this.SelectedItemChanged.emit(this.SelectedItem);
            this.DetectChanges();
            this.DisplayValue = this.SelectedItem[this.DisplayMemberPath];
            this.ValidateField();
            //this.ToggleOpenDropDown();
        }
    };
    LogLovComponent.prototype.ValidateField = function () {
        var _this = this;
        var errors = null;
        var table = window.ObjectTables.filter(function (d) { return d.Name === _this.uiProperty.ObjectTableName; })[0];
        if (table) {
            var field = window.ObjectFields.filter(function (d) { return d.ObjectTableId === table.Id && d.FieldName === _this.uiProperty.FieldName; })[0];
            var fieldValidator = new FieldValidator_1.FieldValidator();
            errors = fieldValidator.Validate(this.ObjectFieldName, this.ObjectTableName, this.DataContext);
        }
        if (errors) {
            if (errors.length > 0) {
                this.SetValidity(false, errors[0]);
            }
            else if (this.uiProperty.IsValidManually == false) {
                this.SetValidity(false, this.uiProperty.ManualValidationError);
            }
            else {
                this.SetValidity(true, null);
            }
        }
        else if (this.uiProperty.IsValidManually == false) {
            this.SetValidity(false, this.uiProperty.ManualValidationError);
        }
        else {
            this.SetValidity(true, null);
        }
        this.uiProperty.UIPropertyChanged.emit(this.uiProperty);
    };
    LogLovComponent.prototype.OnFocus = function () {
        this.Detach = false;
        this.DetectChanges();
        this.show = true;
        if (this.uiProperty.ValidValue) {
            this.InputDivStyle = { 'border': '1px solid #3BB3E2' };
            this.ShowErrorPopup = false;
        }
        else {
            this.InputDivStyle = { 'border': '1px solid #ff0000' };
            this.ShowErrorPopup = true;
        }
    };
    LogLovComponent.prototype.OnBlur = function () {
        this.Detach = true;
        this.DetectChanges();
        this.show = false;
        this.ShowErrorPopup = false;
        if (this.uiProperty.ValidValue) {
            this.InputDivStyle = null;
        }
    };
    LogLovComponent.prototype.DetectChanges = function () {
        return;
    };
    __decorate([
        core_1.Input(),
        __metadata("design:type", forms_1.FormGroup)
    ], LogLovComponent.prototype, "LogitudeForm", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Object)
    ], LogLovComponent.prototype, "SelectedItemObject", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], LogLovComponent.prototype, "ValueChanged", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], LogLovComponent.prototype, "SelectedItemChanged", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Object),
        __metadata("design:paramtypes", [Object])
    ], LogLovComponent.prototype, "SelectedValue", null);
    LogLovComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'LogLov_Old',
            templateUrl: './LogLovComponent.html',
            providers: [EntityListService_1.EntityListService, ServiceArgs_1.ServiceArgs, EntityResourceService_1.EntityResourceService],
            inputs: ['ObjectFieldName', 'ObjectTableName', 'DataContext', 'LookUpTableName', 'DisplayMemberPath', 'SelectedValuePath',
                'PlaceHolder', 'DependencyFilter1Value', 'DependencyFilter2Value', "HideColumns", "HideLastColumn", "DependencyFilter1IsList",
                "DependencyFilter2IsList", "DependencyFilter1IsListExact", "DependencyFilter2IsListExact", "AutoFocus", "IsTenantZeroSearch", "ShowInActive"],
        }),
        __metadata("design:paramtypes", [EntityListService_1.EntityListService, EntityResourceService_1.EntityResourceService])
    ], LogLovComponent);
    return LogLovComponent;
}());
exports.LogLovComponent = LogLovComponent;
//# sourceMappingURL=LogLovComponent.js.map