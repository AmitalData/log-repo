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
var UIProperties_1 = require("./UIProperties");
var SessionLocator_1 = require("../../Utilities/SessionLocator");
var Tools_1 = require("../../Tools");
var TextCodeTranslator_1 = require("../../Utilities/TextCodeTranslator");
var ObjectTablePM_1 = require("../../EntityPMs/ObjectTablePM");
var ApiQueryFilters_1 = require("../../DataContracts/ApiQueryFilters");
var EntityResourceService_1 = require("../../Services/EntityResourceService");
var EntityListService_1 = require("../../Services/EntityListService");
var ServiceResponse_1 = require("../../DataContracts/ServiceResponse");
var Observable_1 = require("rxjs/Observable");
require("rxjs/add/operator/debounceTime");
require("rxjs/add/operator/throttleTime");
require("rxjs/add/observable/fromEvent");
var FieldValidator_1 = require("../../Validators/FieldValidator");
var ControlsIdCounter_1 = require("../../Utilities/ControlsIdCounter");
var PickListComponent = /** @class */ (function () {
    function PickListComponent(_entityResourceService, entityListService) {
        this._entityResourceService = _entityResourceService;
        this.entityListService = entityListService;
        this.IsMultipleChoice = false;
        this.FromNewView = false;
        this.IsFreeText = false;
        this.IgnoreCustomFieldCheck = false;
        //-------------------------------------------------------
        this.OnBlurEvent = new core_1.EventEmitter();
        this.ValueChanged = new core_1.EventEmitter();
        this.SelectedItemChanged = new core_1.EventEmitter();
        this.LostFocus = new core_1.EventEmitter();
        //-------------------------------------------------------
        this.DropDownWidth = 300;
        this.DropDownHeight = 257;
        this.ItemsSourceCount = -1;
        this.AfterViewInitialized = false;
        this.IsCTRLDown = false;
        this.searchTextChanged = false;
        this.FocusOnMe = false;
        this.showPopup = false;
        this.ShowErrorPopup = false;
        this.show = false;
        this.tabkeyDown = false;
        this.onhover = false;
        this.ObjectFieldHelp = null;
        this.ShowHelp = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
    }
    Object.defineProperty(PickListComponent.prototype, "IsVisible", {
        get: function () {
            if (!this.uiProperty) {
                this.uiProperty = this.DataContext.UIProperties.GetUIProperty(this.ObjectFieldName, this.ObjectTableName, this.DataContext);
            }
            return this.uiProperty.IsVisible;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PickListComponent.prototype, "IsDisabled", {
        get: function () {
            return this.isDisabled;
        },
        set: function (newValue) {
            this.isDisabled = newValue;
            if (this.isDisabled) {
                this.imgNgStyle = { 'opacity': .5, 'pointer-events': 'none' };
                this.InputDivStyle = { 'opacity': .5, };
            }
            else {
                this.imgNgStyle = { 'opacity': 1, 'pointer-events': 'all' };
                if (this.uiProperty != null) {
                    if (this.uiProperty.ValidValue) {
                        this.InputDivStyle = { 'opacity': 1, };
                    }
                    else {
                        this.InputDivStyle = { 'opacity': 1, /*'pointer-events': 'all',*/ 'border': '1px solid #ff0000' };
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PickListComponent.prototype, "SelectedValue", {
        get: function () {
            return this.selectedValue;
        },
        set: function (newValue) {
            if (this.selectedValue != newValue) {
                this.selectedValue = newValue;
                if (!this.LookUpTable || this.IsFreeText) {
                    this.LookUpTable = new ObjectTablePM_1.ObjectTablePM();
                    this.LookUpTable.Name = 'CustomPickList';
                    this.LookUpTableName = 'CustomPickList';
                    this.LookUpTable.Tenant = SessionLocator_1.SessionLocator.Tenant;
                    this.LookUpTable.LookUp1 = "Value";
                    this.LookUpTable.CacheOnClient = true;
                }
                if (this.LookUpTable) {
                    if (!this.isSelectedFromList || this.IsFreeText) {
                        this.ValueChanged.emit(this.selectedValue);
                        this.GetSingle(this.LookUpTable);
                    }
                    this.isSelectedFromList = false;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    PickListComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.counterId = null;
        var baseIdCombination = null;
        if (this.ObjectTableName) {
            baseIdCombination = this.ObjectTableName + "_" + this.ObjectFieldName;
        }
        else {
            baseIdCombination = this.ObjectFieldName;
        }
        if (this.CheckIfExists(baseIdCombination)) {
            this.counterId = ControlsIdCounter_1.ControlsIdCounter.GetNextControlIdCounter(baseIdCombination);
        }
        if (this.counterId != null) {
            baseIdCombination = baseIdCombination + '_' + this.counterId.toString();
        }
        this.SetControlIds(baseIdCombination);
        this.ObjectTable = window.ObjectTables.filter(function (d) { return d.Name === _this.ObjectTableName; })[0];
        this.ObjectField = window.ObjectFields.filter(function (d) { return d.ObjectTableId === _this.ObjectTable.Id && d.FieldName === _this.ObjectFieldName; })[0];
        this.LookUpTable = new ObjectTablePM_1.ObjectTablePM();
        this.LookUpTable.Name = 'CustomPickList';
        this.LookUpTableName = 'CustomPickList';
        this.LookUpTable.Tenant = SessionLocator_1.SessionLocator.Tenant;
        this.LookUpTable.LookUp1 = "Value";
        this.LookUpTable.CacheOnClient = true;
        this.DisplayMemberPath = 'Value';
        this.SelectedValuePath = 'Id';
        this.KeyPropertyPath = 'Id';
        this.uiProperty = this.DataContext.UIProperties.GetUIProperty(this.ObjectFieldName, this.ObjectTableName, this.DataContext);
        if (this.IgnoreCustomFieldCheck == true) {
            this.DataContext[this.ObjectFieldName] = this.DataContext["TextValue"];
        }
        this.IsDisabled = !this.uiProperty.IsEnabled;
        this._entityResourceService.getEntityResourceByTableName('CustomPickList', 0).subscribe(function (res) {
            var objectFieldAvailable = true;
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
            }
            if (_this.DataContext[_this.ObjectFieldName]) {
                _this.GetSingle(_this.LookUpTable);
            }
            else if (_this.FromNewView) {
                var lookup = _this.LookUpTable;
                var apiFilters;
                if (!_this.QueryFilterItems) {
                    apiFilters = new ApiQueryFilters_1.ApiQueryFilters();
                }
                else {
                    apiFilters = new ApiQueryFilters_1.ApiQueryFilters();
                    for (var i = 0; i < _this.QueryFilterItems.AdditionalFilters.length; i++) {
                        var filter = _this.QueryFilterItems.AdditionalFilters[i];
                        apiFilters.addAdditionalFilter(filter.FieldName, filter.FieldValue, filter.FieldValue2, filter.FieldValue3, filter.Operator, filter.IsCustom, filter.DisplayInList, filter.IsCustomField, filter.FieldDataType, filter.IgnoreFilter, _this.LookUpTable.CacheOnClient);
                    }
                    //apiFilters = this.QueryFilterItems;
                }
                if (lookup.CacheOnClient) {
                    _this.entityListService.getSingleFromCache(_this.DataContext['TextValue'], _this.LookUpTableName, apiFilters).then(function (res) {
                        res.subscribe(function (myResponse) {
                            if (myResponse != null) {
                                var list = myResponse;
                                if (myResponse instanceof ServiceResponse_1.ServiceResponse) {
                                    list = myResponse.Result;
                                }
                                if (list) {
                                    _this.SearchTextNgModel = list[_this.DisplayMemberPath];
                                    _this.OldSearchInput = _this.SearchTextNgModel;
                                    _this.DisplayValue = list[_this.DisplayMemberPath];
                                    _this.SelectedItem = list;
                                    _this.SelectedItemChanged.emit(_this.SelectedItem);
                                }
                            }
                        });
                    });
                }
                else {
                    _this.entityListService.getSingle(_this.DataContext['TextValue'], _this.LookUpTableName).then(function (res) {
                        res.subscribe(function (myResponse) {
                            if (myResponse != null) {
                                var list = myResponse;
                                if (myResponse instanceof ServiceResponse_1.ServiceResponse) {
                                    list = myResponse.Result;
                                }
                                if (list) {
                                    _this.SearchTextNgModel = list[_this.DisplayMemberPath];
                                    _this.OldSearchInput = _this.SearchTextNgModel;
                                    _this.DisplayValue = list[_this.DisplayMemberPath];
                                    _this.SelectedItem = list;
                                    _this.SelectedItemChanged.emit(_this.SelectedItem);
                                }
                            }
                        });
                    });
                }
                if (objectFieldAvailable == true) {
                    _this.ValidateField(true);
                }
            }
            if (objectFieldAvailable == true) {
                _this.uiProperty.UIPropertyChanged.subscribe(function (value) {
                    if (value instanceof UIProperties_1.UIPropertyArgs) {
                        var uiPropertyArgs = value;
                        var uiProperty = uiPropertyArgs.uiProperty;
                        if (uiProperty.FieldName == _this.ObjectFieldName && uiProperty.ObjectTableName == _this.ObjectTableName) {
                            if (uiPropertyArgs.property == "IsEnabled") {
                                var isEnabled = uiPropertyArgs.newValue;
                                _this.IsDisabled = !isEnabled;
                                _this.uiProperty.IsEnabled = isEnabled;
                            }
                            else if (uiPropertyArgs.property == "IsRequired") {
                                if (_this.searchTextChanged) {
                                    _this.ValidateField(false);
                                }
                            }
                            else if (uiPropertyArgs.property == "IsVisible") {
                                if (uiPropertyArgs.newValue == true && _this.AfterViewInitialized == false) {
                                    _this.InitializeAfterViewInit();
                                }
                            }
                            else if (uiPropertyArgs.property == "IsValid") {
                                _this.ValidateField(false);
                            }
                        }
                    }
                });
            }
        });
    };
    PickListComponent.prototype.ngAfterViewInit = function () {
        this.InitializeAfterViewInit();
    };
    PickListComponent.prototype.ngOnDestroy = function () {
    };
    PickListComponent.prototype.InitializeAfterViewInit = function () {
        var _this = this;
        var input = document.getElementById(this.ElementId);
        if (input != null && input != undefined) {
            this.AfterViewInitialized = true;
        }
        Observable_1.Observable.fromEvent(input, 'keydown')
            .debounceTime(400)
            .subscribe(function (keyboardEvent) {
            var TABKEY = 9;
            var ENTERKEY = 13;
            var DOWNKEY = 40;
            var UPKEY = 38;
            var ESC = 27;
            var END = 35;
            var HOME = 36;
            var CTRL = 17;
            var BACKSPACE = 8;
            var which = logLoveReturnWhich(keyboardEvent);
            if (which == TABKEY || which == ENTERKEY || which == DOWNKEY || which == UPKEY
                || which == ESC || which == END || which == HOME || which == 220 || which == CTRL || _this.IsCTRLDown) {
                return;
            }
            if (_this.SearchTextNgModel != undefined) {
                _this.OldSearchInput = _this.SearchTextNgModel;
                _this.IsDropDownVisible = true;
                _this.IsOpen = true;
                _this.Populate(_this.SearchTextNgModel);
                _this.searchTextChanged = true;
            }
            if (!_this.SearchTextNgModel) {
                _this.OnDeleteValue();
            }
        });
        if (this.FocusOnMe) {
            var element = document.getElementById(this.ElementId);
            element.focus();
            this.CurrentSession.SessionEvent.emit({ IsCell: true, Id: element.id, OnBlurEvent: this.OnBlurEvent });
            this.timerToken = setTimeout(function () {
                Selection(element);
            }, 1);
        }
    };
    PickListComponent.prototype.CheckIfExists = function (IdCom) {
        var element = document.getElementById(IdCom);
        if (element != null && element != undefined) {
            return true;
        }
        return false;
    };
    PickListComponent.prototype.SetControlIds = function (baseIdCombination) {
        this.DivPickListId = 'picklist_' + baseIdCombination;
        this.ElementId = baseIdCombination;
        this.DropdownId = 'picklistdropdown-' + baseIdCombination;
        this.ErrorPopUpId = 'picklistererrorpop_' + baseIdCombination;
        this.MyDataListId = 'mydatapicklist_' + baseIdCombination;
    };
    PickListComponent.prototype.Populate = function (searchText, setFirstAsSelected) {
        if (setFirstAsSelected === void 0) { setFirstAsSelected = false; }
        this.LovMessage = null;
        //reset counters
        this.ItemsSourceCount = -1;
        this.ItemsSource = [];
        this.ItemsSourceStatic = [];
        var filters;
        if (!this.QueryFilterItems) {
            filters = new ApiQueryFilters_1.ApiQueryFilters();
        }
        else {
            // filters = this.QueryFilterItems;
            filters = new ApiQueryFilters_1.ApiQueryFilters();
            for (var i = 0; i < this.QueryFilterItems.AdditionalFilters.length; i++) {
                var filter = this.QueryFilterItems.AdditionalFilters[i];
                filters.addAdditionalFilter(filter.FieldName, filter.FieldValue, filter.FieldValue2, filter.FieldValue3, filter.Operator, filter.IsCustom, filter.DisplayInList, filter.IsCustomField, filter.FieldDataType, filter.IgnoreFilter, this.LookUpTable.CacheOnClient);
            }
        }
        filters.SortDirection = "Ascending";
        filters.PageIndex = 0;
        filters.SortBy = 'Value';
        if (this.ObjectField != null) {
            filters.addAdditionalFilter("Code", this.ObjectField.CustomPickListCode, null, null, "Equals", false, false, false, null, false, this.LookUpTable.CacheOnClient);
        }
        else {
            filters.addAdditionalFilter("Code", this.PickListCode, null, null, "Equals", false, false, false, null, false, this.LookUpTable.CacheOnClient);
        }
        filters.addAdditionalFilter("IsMultipleChoice", this.IsMultipleChoice, null, null, "Equals", false, false, false, null, false, this.LookUpTable.CacheOnClient);
        this.CallDataFromCache(searchText, filters, setFirstAsSelected);
    };
    PickListComponent.prototype.CallDataFromCache = function (searchText, filters, setFirstAsSelected) {
        var _this = this;
        if (setFirstAsSelected === void 0) { setFirstAsSelected = false; }
        if (!this.LookUpTable.AutoCompleteSearchWindow) {
            filters.PageSize = 1000;
        }
        else {
            filters.PageSize = 10;
        }
        if (searchText) {
            filters.addAdditionalFilter('Value', searchText, null, null, "StartsWith", false, false, false, null, false, this.LookUpTable.CacheOnClient);
        }
        //turn loading flag on
        this.isLoading = true;
        this.entityListService.getByFilters(this.LookUpTableName, filters).then(function (res) {
            res.subscribe(function (resp) {
                if (resp.Result) {
                    _this.ItemsSource = resp.Result;
                    _this.ItemsSourceCount = resp.Result.length;
                }
                else {
                    _this.ItemsSource = resp;
                    _this.ItemsSourceCount = resp.length;
                }
                if (_this.ItemsSourceCount == 0) {
                    _this.LovMessage = "No more results founds";
                }
                else {
                    _this.LovMessage = null;
                }
                _this.ItemsSourceStatic = _this.ItemsSource;
                _this.HighlightSelectedValue(_this.ItemsSource);
                _this.isLoading = false;
            });
        });
    };
    PickListComponent.prototype.HighlightSelectedValue = function (items) {
        var _this = this;
        this.SelectedItemKey = null;
        if (this.SearchTextNgModel != null && this.SearchTextNgModel != undefined && this.SearchTextNgModel != "") {
            var item = items.filter(function (d) { return d[_this.DisplayMemberPath].toLowerCase() === _this.SearchTextNgModel.toLowerCase(); })[0];
            if (item) {
                var index = items.indexOf(item);
                var temp = items[0];
                items[0] = item;
                items[index] = temp;
                this.SelectedItemKey = item[this.KeyPropertyPath];
            }
            else if (items.length > 0) {
                this.SelectedItemKey = items[0][this.KeyPropertyPath];
            }
        }
    };
    PickListComponent.prototype.GetSingle = function (lookup) {
        var _this = this;
        if (this.DataContext[this.ObjectFieldName] || this.IsFreeText) {
            var value = this.IsFreeText ? this.selectedValue : this.DataContext[this.ObjectFieldName];
            if (this.ObjectField) {
                if (this.ObjectField.IsCustom && this.IgnoreCustomFieldCheck == false) {
                    var customFieldClass = this.DataContext[this.ObjectFieldName];
                    if (customFieldClass != null && customFieldClass != undefined) {
                        value = customFieldClass.Value;
                    }
                    else {
                        console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
                    }
                }
            }
            var apiFilters;
            if (!this.QueryFilterItems) {
                apiFilters = new ApiQueryFilters_1.ApiQueryFilters();
            }
            else {
                apiFilters = new ApiQueryFilters_1.ApiQueryFilters();
                for (var i = 0; i < this.QueryFilterItems.AdditionalFilters.length; i++) {
                    var filter = this.QueryFilterItems.AdditionalFilters[i];
                    apiFilters.addAdditionalFilter(filter.FieldName, filter.FieldValue, filter.FieldValue2, filter.FieldValue3, filter.Operator, filter.IsCustom, filter.DisplayInList, filter.IsCustomField, filter.FieldDataType, filter.IgnoreFilter, this.LookUpTable.CacheOnClient);
                }
                //apiFilters = this.QueryFilterItems;
            }
            if (lookup.CacheOnClient) {
                this.entityListService.getSingleFromCache(value, this.LookUpTableName, apiFilters).then(function (res) {
                    res.subscribe(function (myResponse) {
                        if (myResponse != null) {
                            var list = myResponse;
                            if (myResponse instanceof ServiceResponse_1.ServiceResponse) {
                                list = myResponse.Result;
                            }
                            if (list) {
                                _this.SearchTextNgModel = list[_this.DisplayMemberPath];
                                _this.OldSearchInput = _this.SearchTextNgModel;
                                _this.DisplayValue = list[_this.DisplayMemberPath];
                                _this.SelectedItem = list;
                                _this.SelectedItemChanged.emit(_this.SelectedItem);
                            }
                        }
                    });
                });
            }
            else {
                this.entityListService.getSingle(value, this.LookUpTableName).then(function (res) {
                    res.subscribe(function (myResponse) {
                        if (myResponse != null) {
                            var list = myResponse;
                            if (myResponse instanceof ServiceResponse_1.ServiceResponse) {
                                list = myResponse.Result;
                            }
                            if (list) {
                                _this.SearchTextNgModel = list[_this.DisplayMemberPath];
                                _this.OldSearchInput = _this.SearchTextNgModel;
                                _this.DisplayValue = list[_this.DisplayMemberPath];
                                _this.SelectedItem = list;
                                _this.SelectedItemChanged.emit(_this.SelectedItem);
                            }
                        }
                    });
                });
            }
            this.ValidateField(true);
        }
        else {
            if (this.deleteSearchText != false) {
                this.SearchTextNgModel = null;
                this.OldSearchInput = this.SearchTextNgModel;
            }
            this.DisplayValue = null;
            this.SelectedItem = null;
            this.SelectedItemChanged.emit(this.SelectedItem);
        }
    };
    PickListComponent.prototype.OnDeleteValue = function (deleteSearch) {
        if (deleteSearch === void 0) { deleteSearch = true; }
        this.showPopup = false;
        this.deleteSearchText = deleteSearch;
        this.SelectedItem = null;
        if (this.ObjectField) {
            if (this.ObjectField.IsCustom && this.IgnoreCustomFieldCheck == false) {
                var customFieldClass = this.DataContext[this.ObjectFieldName];
                if (customFieldClass != null && customFieldClass != undefined) {
                    customFieldClass.Value = null;
                    this.DataContext[this.ObjectFieldName] = customFieldClass;
                }
                else {
                    console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
                }
            }
            else {
                this.DataContext[this.ObjectFieldName] = null;
            }
        }
        else {
            this.DataContext[this.ObjectFieldName] = null;
        }
        this.SelectedItemChanged.emit(this.SelectedItem);
        this.DisplayValue = null;
        this.ValueChanged.emit(this.DataContext[this.ObjectFieldName]);
        if (deleteSearch) {
            this.SearchTextNgModel = null;
            this.OldSearchInput = this.SearchTextNgModel;
            this.ValidateField();
        }
    };
    PickListComponent.prototype.ValidateField = function (emitPropertyChanged) {
        var _this = this;
        if (emitPropertyChanged === void 0) { emitPropertyChanged = true; }
        if (!this.NoValidation && this.uiProperty) {
            var errors = null;
            //var table = window.ObjectTables.filter(d => d.Name === this.uiProperty.ObjectTableName)[0];
            if (this.ObjectTable) {
                var field = window.ObjectFields.filter(function (d) { return d.ObjectTableId === _this.ObjectTable.Id && d.FieldName === _this.uiProperty.FieldName; })[0];
                var fieldValidator = new FieldValidator_1.FieldValidator();
                errors = fieldValidator.Validate(this.ObjectFieldName, this.ObjectTableName, this.DataContext);
            }
            if (this.uiProperty.IsValidManually == false) {
                this.SetValidity(false, this.uiProperty.ManualValidationError);
            }
            else if (errors) {
                if (errors.length > 0) {
                    this.SetValidity(false, errors[0]);
                }
                //else if (this.uiProperty.IsValidManually == false) {
                //    this.SetValidity(false, this.uiProperty.ManualValidationError);
                //}
                else {
                    this.SetValidity(true, null);
                }
            }
            //else if (this.uiProperty.IsValidManually == false) {
            //    this.SetValidity(false, this.uiProperty.ManualValidationError);
            //}
            else {
                this.SetValidity(true, null);
            }
            if (emitPropertyChanged) {
                this.uiProperty.UIPropertyChanged.emit(this.uiProperty);
            }
        }
    };
    PickListComponent.prototype.SetValidity = function (validValue, errorMessage) {
        this.uiProperty.ValidValue = validValue;
        this.uiProperty.ValidationError = errorMessage;
        if (!validValue) {
            if (this.IsDisabled) {
                this.ShowErrorPopup = false;
                this.show = false;
                this.InputDivStyle = { 'border': '1px solid #AAAAAA', 'opacity': .5, };
            }
            else {
                this.InputDivStyle = { 'border': '1px solid #ff0000', };
                if (this.show) {
                    this.ShowErrorPopup = true;
                }
            }
        }
        else {
            if (this.IsDisabled) {
                this.InputDivStyle = { 'border': '1px solid #AAAAAA', 'opacity': .5, };
            }
            else {
                this.ShowErrorPopup = false;
                if (this.show) {
                    this.InputDivStyle = { 'border': '1px solid #3BB3E2', };
                }
                else {
                    this.InputDivStyle = null;
                }
            }
        }
    };
    PickListComponent.prototype.OnMouseOver = function () {
        this.MouseInArea = true;
        if (!this.SelectedItem || this.IsDisabled) {
            this.DeleteButtonNgStyle = { 'visibility': 'hidden' };
        }
        else {
            this.DeleteButtonNgStyle = null;
        }
        if (this.IsDisabled) {
            this.InputDivStyle = { 'border': '1px solid #AAAAAA', 'opacity': .5, };
        }
    };
    PickListComponent.prototype.OnMouseOut = function () {
        this.MouseInArea = false;
    };
    PickListComponent.prototype.OnMouseHover = function () {
        var _this = this;
        this.onhover = true;
        this.timerToken = setTimeout(function () {
            if (_this.onhover) {
                //this.show = true;
                _this.ShowMaintenanceBtn = true;
                _this.showPopup = false;
            }
        }, 700);
    };
    PickListComponent.prototype.OnMouseLeave = function () {
        var _this = this;
        this.onhover = false;
        this.timerToken = setTimeout(function () {
            if (!_this.showPopup && !_this.MouseInArea) {
                _this.ShowMaintenanceBtn = false;
                _this.showPopup = false;
            }
        }, 700);
    };
    PickListComponent.prototype.OnToggleClicked = function () {
        this.ToggleOpenDropDown();
        if (this.IsOpen) {
            this.Populate(null);
        }
    };
    PickListComponent.prototype.ToggleOpenDropDown = function () {
        this.IsDropDownVisible = !this.IsDropDownVisible;
        this.IsOpen = !this.IsOpen;
        if (this.IsOpen) {
            var input = document.getElementById(this.ElementId);
            input.focus();
            this.showPopup = false;
        }
    };
    PickListComponent.prototype.OnMaintenanceClick = function () {
        this.CheckEditAddEnable();
        var input = document.getElementById(this.ElementId);
        input.focus();
        this.TogglePopup();
        this.IsOpen = false;
        this.IsDropDownVisible = false;
    };
    PickListComponent.prototype.CheckEditAddEnable = function () {
        if (this.SelectedItem == null) {
            this.isDeleteDisabled = true;
        }
        else {
            this.isDeleteDisabled = false;
        }
    };
    PickListComponent.prototype.TogglePopup = function () {
        this.showPopup = !this.showPopup;
    };
    PickListComponent.prototype.OnSearchInputBlur = function () {
        if (!this.MouseInArea) {
            if (this.IsOpen) {
                this.ToggleOpenDropDown();
            }
            this.show = false;
            this.ShowErrorPopup = false;
            this.ShowMaintenanceBtn = false;
            if (this.uiProperty.ValidValue) {
                this.InputDivStyle = null;
            }
            this.OnBlurEvent.emit({ Id: this.ElementId });
            if (this.ObjectField) {
                if (this.ObjectField.IsCustom && this.IgnoreCustomFieldCheck == false) {
                    var customFieldClass = this.DataContext[this.ObjectFieldName];
                    if (customFieldClass != null && customFieldClass != undefined) {
                        this.LostFocus.emit(customFieldClass.Value);
                    }
                    else {
                        console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
                    }
                }
                else {
                    this.LostFocus.emit(this.DataContext[this.ObjectFieldName]);
                }
            }
            else {
                this.LostFocus.emit(this.DataContext[this.ObjectFieldName]);
            }
        }
        if (!this.SelectedItem && this.SearchTextNgModel != null && this.SearchTextNgModel != undefined) {
            if (this.LookUpTable.CacheOnClient && this.tabkeyDown == true && !this.IsOpen) {
                this.Populate(this.SearchTextNgModel, true);
                this.tabkeyDown = false;
            }
            else {
                this.SearchTextNgModel = null;
                this.OldSearchInput = this.SearchTextNgModel;
                this.ValidateField();
            }
        }
    };
    PickListComponent.prototype.OnSelected = function (item) {
        this.isSelectedFromList = true;
        this.SelectedItem = item;
        if (this.ObjectField) {
            if (this.ObjectField.IsCustom && this.IgnoreCustomFieldCheck == false) {
                var customFieldClass = this.DataContext[this.ObjectFieldName];
                if (customFieldClass != null && customFieldClass != undefined) {
                    customFieldClass.Value = this.SelectedItem[this.SelectedValuePath];
                    this.DataContext[this.ObjectFieldName] = customFieldClass;
                }
                else {
                    console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
                }
            }
            else {
                this.DataContext[this.ObjectFieldName] = this.SelectedItem[this.SelectedValuePath];
            }
        }
        else {
            this.DataContext[this.ObjectFieldName] = this.SelectedItem[this.SelectedValuePath];
        }
        this.SelectedItemChanged.emit(this.SelectedItem);
        this.DisplayValue = this.SelectedItem[this.DisplayMemberPath];
        this.SearchTextNgModel = this.SelectedItem[this.DisplayMemberPath];
        this.OldSearchInput = this.SearchTextNgModel;
        this.ValueChanged.emit(this.DataContext[this.ObjectFieldName]);
        this.ValidateField();
        if (this.IsOpen) {
            this.ToggleOpenDropDown();
        }
        this.show = false;
        this.ShowErrorPopup = false;
        if (this.uiProperty.ValidValue) {
            this.InputDivStyle = null;
        }
        var elem = document.getElementById(this.ElementId);
        elem.focus();
        this.MouseInArea = false;
    };
    PickListComponent.prototype.OnDropDownMouseOver = function () {
        this.MouseInArea = true;
        //this.CanClose = false;
        var input = document.getElementById(this.ElementId);
        input.focus();
    };
    PickListComponent.prototype.OnDropDownMouseOut = function () {
        //this.CanClose = true;
        this.MouseInArea = false;
    };
    PickListComponent.prototype.OnLiMouseOver = function ($event) {
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
    PickListComponent.prototype.OnLiMouseLeave = function ($event) {
        $event.target.classList.remove("highlighted");
    };
    PickListComponent.prototype.OnFocus = function () {
        this.show = true;
        this.showPopup = false;
        if (this.uiProperty.ValidValue) {
            this.InputDivStyle = { 'border': '1px solid #3BB3E2' };
            this.ShowErrorPopup = false;
        }
        else {
            this.InputDivStyle = { 'border': '1px solid #ff0000' };
            this.ShowErrorPopup = true;
        }
        if (!this.IsOpen) {
            var input = document.getElementById(this.ElementId);
            Selection(input);
            //input.select();
        }
    };
    PickListComponent.prototype.OnBlur = function () {
        this.show = false;
        this.showPopup = false;
        this.ShowErrorPopup = false;
        if (this.uiProperty.ValidValue) {
            this.InputDivStyle = null;
        }
    };
    PickListComponent.prototype.OnSearchIputKeyDown = function ($event) {
        var TABKEY = 9;
        var ENTERKEY = 13;
        var DOWNKEY = 40;
        var UPKEY = 38;
        var ESC = 27;
        var CTRL = 17;
        var SHIFT = 16;
        if ($event.keyCode == CTRL) {
            this.IsCTRLDown = true;
        }
        if ($event.keyCode == 220) {
            return false;
        }
        if ($event.keyCode == TABKEY) {
            this.MouseInArea = false;
            this.tabkeyDown = true;
            if (this.IsOpen) {
                var active = document.getElementsByClassName("highlighted");
                if (active[0]) {
                    var input = document.getElementById(this.MyDataListId);
                    var lis = input.getElementsByTagName("li");
                    for (var i = 0; i < lis.length; i++) {
                        if (lis[i].className.search("highlighted") > -1) {
                            lis[i].classList.remove("highlighted");
                            lis[i].click();
                        }
                    }
                }
                else {
                    var selected = document.getElementsByClassName("liItemSelected");
                    if (selected[0]) {
                        var input = document.getElementById(this.MyDataListId);
                        var lis = input.getElementsByTagName("li");
                        for (var i = 0; i < lis.length; i++) {
                            if (lis[i].className.search("liItemSelected") > -1) {
                                lis[i].classList.remove("liItemSelected");
                                lis[i].click();
                            }
                        }
                    }
                }
            }
            if (this.IsOpen) {
                this.ToggleOpenDropDown();
            }
            //this.OnBlurEvent.emit("");
        }
        if ($event.keyCode == ENTERKEY) {
            if (this.IsOpen) {
                var active = document.getElementsByClassName("highlighted");
                if (active[0]) {
                    var input = document.getElementById(this.MyDataListId);
                    var lis = input.getElementsByTagName("li");
                    for (var i = 0; i < lis.length; i++) {
                        if (lis[i].className.search("highlighted") > -1) {
                            lis[i].classList.remove("highlighted");
                            lis[i].click();
                        }
                    }
                }
                else {
                    var selected = document.getElementsByClassName("liItemSelected");
                    if (selected[0]) {
                        var input = document.getElementById(this.MyDataListId);
                        var lis = input.getElementsByTagName("li");
                        for (var i = 0; i < lis.length; i++) {
                            if (lis[i].className.search("liItemSelected") > -1) {
                                lis[i].classList.remove("liItemSelected");
                                lis[i].click();
                            }
                        }
                    }
                }
            }
        }
        if ($event.keyCode == DOWNKEY) {
            if (!this.IsOpen) {
                this.ToggleOpenDropDown();
                if (this.IsOpen) {
                    this.Populate(null);
                }
            }
            else {
                var selected = document.getElementsByClassName("liItemSelected");
                if (selected[0]) {
                    var input = document.getElementById(this.MyDataListId);
                    var lis = input.getElementsByTagName("li");
                    for (var i = 0; i < lis.length; i++) {
                        if (lis[i].className.search("liItemSelected") > -1) {
                            lis[i].classList.remove("liItemSelected");
                            lis[i].classList.add("highlighted");
                        }
                    }
                }
                this.NavigateListItems(true);
            }
            return false;
        }
        if ($event.keyCode == UPKEY) {
            this.NavigateListItems(false);
            return false;
        }
        if ($event.keyCode == ESC) {
            if (this.IsOpen) {
                this.ToggleOpenDropDown();
            }
        }
        var key = $event.keyCode;
        if (key != 17 && key != 18 && key != 19 && key != 20
            && key != 37 && key != 38 && key != 39 && key != 40
            && key != 35 && key != 45 && key != 33 && key != 34
            && key != 145 && key != 13 && key != 27 && key != 9
            && key != 16 && key != 36 && key != 144
            && !(key >= 112 && key <= 123) && !this.IsCTRLDown) {
            this.OnDeleteValue(false);
        }
    };
    PickListComponent.prototype.NavigateListItems = function (isDown) {
        if (isDown) {
            var isSelected = false;
            var active = document.getElementsByClassName("highlighted");
            if (!active[0]) {
                if (this.ItemsSource.length > 0) {
                    var input = document.getElementById(this.MyDataListId);
                    var lis = input.getElementsByTagName("li");
                    lis[0].classList.add("highlighted");
                }
            }
            else {
                if (active[0].nextElementSibling) {
                    active[0].nextElementSibling.classList.add("highlighted");
                    active[0].classList.remove("highlighted");
                    active[0].scrollIntoView(false);
                }
            }
        }
        else {
            var active = document.getElementsByClassName("highlighted");
            if (active[0]) {
                if (this.ItemsSource.length > 0) {
                    if (active[0].previousElementSibling) {
                        active[0].previousElementSibling.classList.add("highlighted");
                        active = document.getElementsByClassName("highlighted");
                        active[1].classList.remove("highlighted");
                        active[0].scrollIntoView(false);
                    }
                }
            }
        }
    };
    PickListComponent.prototype.OnSearchInputKeyUP = function ($event) {
        var CTRL = 17;
        if ($event.keyCode == CTRL) {
            this.IsCTRLDown = false;
        }
    };
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], PickListComponent.prototype, "ObjectFieldName", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], PickListComponent.prototype, "ObjectTableName", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Object)
    ], PickListComponent.prototype, "DataContext", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], PickListComponent.prototype, "HideColumns", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], PickListComponent.prototype, "HideLastColumn", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], PickListComponent.prototype, "NoValidation", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], PickListComponent.prototype, "PlaceHolder", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], PickListComponent.prototype, "IsMultipleChoice", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], PickListComponent.prototype, "PickListCode", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], PickListComponent.prototype, "FromNewView", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], PickListComponent.prototype, "IsFreeText", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], PickListComponent.prototype, "IgnoreCustomFieldCheck", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], PickListComponent.prototype, "OnBlurEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], PickListComponent.prototype, "ValueChanged", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], PickListComponent.prototype, "SelectedItemChanged", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], PickListComponent.prototype, "LostFocus", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Object),
        __metadata("design:paramtypes", [Object])
    ], PickListComponent.prototype, "SelectedValue", null);
    PickListComponent = __decorate([
        core_1.Component({
            selector: 'PickList',
            moduleId: './Infrastructure/Components/LogitudeComponents/',
            templateUrl: 'PickListComponent.html',
        }),
        __metadata("design:paramtypes", [EntityResourceService_1.EntityResourceService, EntityListService_1.EntityListService])
    ], PickListComponent);
    return PickListComponent;
}());
exports.PickListComponent = PickListComponent;
//# sourceMappingURL=PickListComponent.js.map