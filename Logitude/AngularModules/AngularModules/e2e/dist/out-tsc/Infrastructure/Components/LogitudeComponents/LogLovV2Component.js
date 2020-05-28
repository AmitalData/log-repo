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
var SessionLocator_1 = require("../../Utilities/SessionLocator");
var Tools_1 = require("../../Tools");
var TextCodeTranslator_1 = require("../../Utilities/TextCodeTranslator");
var ServiceResponse_1 = require("../../DataContracts/ServiceResponse");
var FieldValidator_1 = require("../../Validators/FieldValidator");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var forms_1 = require("@angular/forms");
var LogSearchWindowComponent_1 = require("./LogSearchWindowComponent");
var Observable_1 = require("rxjs/Observable");
require("rxjs/add/operator/debounceTime");
require("rxjs/add/operator/throttleTime");
require("rxjs/add/observable/fromEvent");
var UIProperties_1 = require("./UIProperties");
var FeatureLocator_1 = require("../../Utilities/FeatureLocator");
var InfraSettings_1 = require("../../Utilities/InfraSettings");
var MessageWindow_1 = require("../../../Controls/Windows/MessageWindow");
var EntityPMService_1 = require("../../Services/EntityPMService");
var TenantImportComponent_1 = require("../../../Common/Components/Maintenance/TenantImportComponent");
var CachedDataManager_1 = require("../../Utilities/CachedDataManager");
var ObjectsLocator_1 = require("../../Locators/ObjectsLocator");
var SessionInfo_1 = require("../../Utilities/SessionInfo");
var LogLovV2Component = /** @class */ (function () {
    function LogLovV2Component(entityListService, entityPMService, _entityResourceService) {
        this.entityListService = entityListService;
        this.entityPMService = entityPMService;
        this._entityResourceService = _entityResourceService;
        this.ForceShowValidation = false;
        this.ShowHelp = false;
        this.ObjectFieldName = null;
        this.ObjectFieldHelp = null;
        this.ObjectTableName = null;
        this.LookUpTableName = null;
        this.HideColumns = false;
        this.HideLastColumn = false;
        this.DisplayMemberPathManuallySet = false;
        this.IsFreeText = false;
        this.AlwaysEnabled = false;
        this.IgnoreCustomFieldCheck = false;
        this.LayoutDirection = 'ltr';
        this.isFirstTime = true;
        this.IsDecendingSort = false;
        this.ItemsSourceCount = -1;
        this.IsReady = false;
        this.ValueChanged = new core_1.EventEmitter();
        this.SelectedItemChanged = new core_1.EventEmitter();
        this.showPopup = false;
        this.PartnerTypes = [];
        this.tabkeyDown = false;
        this.FocusOnMe = false;
        this.searchTextChanged = false;
        this.FocusOnSelect = true;
        this.isRTL = false;
        this.LovPartnerTypes = [];
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.ZeroItemsSourceCount = -1;
        this.ShowErrorPopup = false;
        this.DropDownWidth = 300;
        this.DropDownHeight = 257;
        this.isLoading = false;
        this.isLoadingZero = false;
        this.isAddDisabled = true;
        this.ShowAddLink = false;
        this.isEditDisabled = true;
        this.isDeleteDisabled = true;
        this.showOnlyDelete = false;
        this.isAddVisible = true;
        this.isEditVisible = true;
        this.HideMaintenanceIcon = false;
        this.HideAddLink = false;
        this.NoObjectField = false;
        this.NoValidation = false;
        this.ShowMaintenanceBtn = false;
        this.OnBlurEvent = new core_1.EventEmitter();
        this.LostFocus = new core_1.EventEmitter();
        this.HideEdit = false;
        this.HideAdd = false;
        this.IsCTRLDown = false;
        this.AfterViewInitialized = false;
        this.ManipulateData = false;
        this.LovToolTip = "";
        this.IsAddTypesVisible = false;
        this.IsPartnerMenuVisible = false;
        this.Retries = 0;
        this.onhover = false;
        this.ShowLanguageFilter = false;
        this.show = false;
        this.TenantPM = InfraSettings_1.InfraSettings.TenantPM;
        this.LayoutDirection = ObjectsLocator_1.ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    }
    Object.defineProperty(LogLovV2Component.prototype, "ForceFocus", {
        get: function () {
            return this.forceFocus;
        },
        set: function (newValue) {
            if (newValue) {
                var element = document.getElementById(this.ElementId);
                if (element) {
                    element.focus();
                }
            }
            this.forceFocus = false;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogLovV2Component.prototype, "IsVisible", {
        get: function () {
            if (!this.uiProperty) {
                this.uiProperty = this.DataContext.UIProperties.GetUIProperty(this.ObjectFieldName, this.ObjectTableName, this.DataContext);
            }
            return this.uiProperty.IsVisible;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogLovV2Component.prototype, "IsDisabled", {
        //public set IsVisibile(newValue: boolean) {
        //  this.isVisibile = newValue;
        //}
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
                        if (this.searchTextChanged) {
                            this.InputDivStyle = { 'opacity': 1, /*'pointer-events': 'all',*/ 'border': '1px solid #ff0000' };
                        }
                        else {
                            this.InputDivStyle = { 'opacity': 1 };
                        }
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogLovV2Component.prototype, "SelectedValue", {
        get: function () {
            return this.selectedValue;
        },
        set: function (newValue) {
            var _this = this;
            if (this.selectedValue != newValue) {
                this.selectedValue = newValue;
                if (!this.isSelectedFromList || this.IsFreeText) { //&& !this.isFirstTime) {
                    if (this.uiProperty != null) {
                        //this.uiProperty.UIPropertyChanged.emit("valuechanges"); // caused "a changed was made after it was checked in generatedComponent"
                    }
                    this.ValueChanged.emit(this.selectedValue);
                    var lookup = window.ObjectTables.filter(function (d) { return d.Name === _this.LookUpTableName; })[0];
                    if (!newValue) {
                        this.ForceShowValidation = true;
                        this.deleteSearchText = true;
                    }
                    else {
                        this.ForceShowValidation = false;
                    }
                    this.GetSingle(lookup);
                    this.isSelectedFromList = false;
                }
                this.isSelectedFromList = false;
            }
        },
        enumerable: true,
        configurable: true
    });
    LogLovV2Component.prototype.ngAfterViewInit = function () {
        this.RunComponent();
        //this.InitializeAfterViewInit();
    };
    LogLovV2Component.prototype.RunComponent = function () {
        var input = document.getElementById(this.ElementId);
        if (input) {
            this.InitializeAfterViewInit();
        }
        else {
            this.RunComponentTimer();
        }
    };
    LogLovV2Component.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerTokenComponent) {
            clearTimeout(this.timerTokenComponent);
        }
        if (this.Retries < 3) {
            this.timerTokenComponent = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    LogLovV2Component.prototype.InitializeAfterViewInit = function () {
        var _this = this;
        var input = document.getElementById(this.ElementId);
        if (input) {
            this.AfterViewInitialized = true;
        }
        else {
            this.AfterViewInitialized = false;
            this.RunComponentTimer();
        }
        if (this.AfterViewInitialized) {
            this._KeyDownSubscribe =
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
        }
    };
    LogLovV2Component.prototype.ngOnInit = function () {
        var _this = this;
        this.LookUpTable = window.ObjectTables.filter(function (d) { return d.Name === _this.LookUpTableName; })[0];
        this._entityResourceService.getEntityResourceByTableName(this.LookUpTableName, 0).subscribe(function (res) {
            if (_this.LookUpTable.CacheOnClient) {
                var filters;
                filters = new ApiQueryFilters_1.ApiQueryFilters();
                //filters.PageSize = 50;
                _this.entityListService.getAllFromCache(_this.LookUpTableName, filters).then(function (res) {
                    res.subscribe(function (resp) {
                    });
                });
            }
            else {
                var filters;
                filters = new ApiQueryFilters_1.ApiQueryFilters();
                //filters.PageSize = 50;
                var loadPr = _this.entityListService.getByFilters(_this.LookUpTableName, filters);
                loadPr.then(function (res) {
                    res.subscribe(function (resp) {
                        console.log(resp);
                    });
                });
            }
        });
        this.InitializeControl();
    };
    LogLovV2Component.prototype.ngOnDestroy = function () {
        if (this._KeyDownSubscribe) {
            this._KeyDownSubscribe.unsubscribe();
        }
        if (this.PropertyChangedSubscribtion != null && this.PropertyChangedSubscribtion != undefined) {
            this.PropertyChangedSubscribtion.unsubscribe();
        }
        if (this.CopyValueSubs) {
            this.CopyValueSubs.unsubscribe();
            this.CopyValueSubs = null;
        }
        //if (this.uiProperty != null && this.uiProperty != undefined) {
        //    if (this.uiProperty.UIPropertyChanged != null && this.uiProperty.UIPropertyChanged != undefined) {
        //        this.uiProperty.UIPropertyChanged.unsubscribe();
        //        this.uiProperty = null;
        //    }
        //}
    };
    LogLovV2Component.prototype.CheckIfExists = function (IdCom) {
        var element = document.getElementById(IdCom);
        if (element != null && element != undefined) {
            return true;
        }
        return false;
    };
    LogLovV2Component.prototype.SetControlIds = function (baseIdCombination) {
        this.DivLogLovId = 'LogLov_' + baseIdCombination;
        this.ElementId = baseIdCombination;
        this.DropdownId = 'LogLovDropDown-' + baseIdCombination;
        this.ErrorPopUpId = 'loglovererrorpop_' + baseIdCombination;
        this.MyDataListId = 'mydatalist_' + baseIdCombination;
        this.AllDataListId = 'alldatalist_' + baseIdCombination;
        this.SearchIconId = 'searchicon_' + baseIdCombination;
        this.ToolTipId = 'tooltip_' + baseIdCombination;
    };
    LogLovV2Component.prototype.InitializeControl = function () {
        var _this = this;
        this.Widths = [];
        this.MinWidths = [];
        this.ItemsNgStyles = [];
        this.LogLOVControlClass = "LogLOVControl";
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
        if (this.FocusOnMe) { // it means it is inside a grid.
            this.CopyValueSubs = this.CurrentSession.CopyCellIntoMemory.subscribe(function (id) {
                if (id == _this.ElementId) {
                    //this.CurrentSession.CopiedCell = this.DataContext[this.ObjectFieldName];
                    _this.DataContext[_this.ObjectFieldName] = _this.CurrentSession.CopiedCell;
                    _this.GetSingle(_this.LookUpTable);
                    _this.CurrentSession.CopiedCell = null;
                }
            });
            //if (this.CurrentSession.CopiedCell) {
            //    this.DataContext[this.ObjectFieldName] = this.CurrentSession.CopiedCell;
            //    this.CurrentSession.CopiedCell = null;
            //}
        }
        var objectFieldAvailable = true;
        this.LookUpTable = window.ObjectTables.filter(function (d) { return d.Name === _this.LookUpTableName; })[0];
        this.ObjectTable = window.ObjectTables.filter(function (d) { return d.Name === _this.ObjectTableName; })[0];
        //(currentTable.AutoCompleteSearchWindow || this.AutoCompleteSearchWindow) && !this.RunToggleMode
        if ((this.LookUpTable.AutoCompleteSearchWindow || this.AutoCompleteSearchWindow) && !this.RunToggleMode) {
            this.ShowSearchButton = true;
            this.showToggleButton = false;
        }
        else {
            this.showToggleButton = true;
            this.ShowSearchButton = false;
        }
        this.LookUp1 = this.LookUpTable.LookUp1;
        this.LookUp2 = this.LookUpTable.LookUp2;
        this.headerColumns = [];
        this.dataColumns = [];
        if (this.LookUpTableName == 'Card' || this.LookUpTableName == 'User') {
            this.DropDownWidth = 400;
        }
        if (this.LookUpTableName == 'Carrier') {
            this.DropDownWidth = 350;
        }
        if (this.LookUpTableName == 'GLAccount') {
            this.DropDownWidth = 400;
        }
        if (this.LookUpTableName == 'Port' || this.LookUpTableName == 'Carrier' || this.LookUpTableName == 'Card') {
            this.UseCompactSearch = true;
        }
        if (this.LookUpTableName == 'Port' || this.LookUpTableName == 'Carrier') {
            this.IsAllDataVisible = true;
            this.DropDownHeight = 276;
            this.MyDropDownHeight = { 'height': '105px' };
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
        if (!this.SelectedValuePath) {
            this.SelectedValuePath = this.LookUpTable.KeyPropertyPath;
        }
        this.KeyPropertyPath = this.LookUpTable.KeyPropertyPath;
        this.uiProperty = this.DataContext.UIProperties.GetUIProperty(this.ObjectFieldName, this.ObjectTableName, this.DataContext);
        //console.log(this.uiProperty);
        if (this.AlwaysEnabled == true) {
            this.IsDisabled = false;
        }
        else {
            this.IsDisabled = !this.uiProperty.IsEnabled;
            //console.log("Out " + this.LookUpTableName+ " " + this.uiProperty.IsEnabled)
        }
        if (this.SetIsDisabledTimer) {
            clearTimeout(this.SetIsDisabledTimer);
        }
        this.SetIsDisabledTimer = setInterval(function () { return _this.SetIsDisabled(); }, 1);
        this._entityResourceService.getEntityResourceByTableName(this.LookUpTableName, 0).subscribe(function (res) {
            _this._entityResourceService.getEntityResourceByTableName("PartnerType", 0).subscribe(function (res3) {
                var apiQueryFilter;
                if (!_this.QueryFilterItems) {
                    apiQueryFilter = new ApiQueryFilters_1.ApiQueryFilters();
                }
                else {
                    apiQueryFilter = new ApiQueryFilters_1.ApiQueryFilters();
                    for (var i = 0; i < _this.QueryFilterItems.AdditionalFilters.length; i++) {
                        var filter = _this.QueryFilterItems.AdditionalFilters[i];
                        apiQueryFilter.addAdditionalFilter(filter.FieldName, filter.FieldValue, filter.FieldValue2, filter.FieldValue3, filter.Operator, filter.IsCustom, filter.DisplayInList, filter.IsCustomField, filter.FieldDataType, filter.IgnoreFilter, _this.LookUpTable.CacheOnClient);
                    }
                }
                // apiQueryFilter.GetAll = true; by mohammad.
                _this.entityListService.getAllFromCache("PartnerType", apiQueryFilter).then(function (res3) {
                    res3.subscribe(function (res4) {
                        _this.PartnerTypes = res4.Result;
                        var parentName = _this.GetObjectTableName(_this.LookUpTableName);
                        _this._entityResourceService.getEntityResourceByTableName(parentName, 0).subscribe(function (res5) {
                            if (_this.LookUpTableName == "Card" || _this.LookUpTableName == "Carrier") {
                                if (_this.DependencyFilter1Value != null && _this.DependencyFilter1Value != undefined) {
                                    //this._entityResourceService.getEntityResourceByTableName(parentName, 0).subscribe((otherResp: any) => { });
                                    if (_this.DependencyFilter1Value.toString().split(',').length > 1) {
                                        var types = _this.DependencyFilter1Value.toString().split(',');
                                        _this.LovPartnerTypes = _this.PartnerTypes.filter(function (d) { return types.indexOf(d.Id) > -1; });
                                    }
                                }
                            }
                            // drow columns
                            var lang = 'E';
                            if (SessionInfo_1.SessionInfo.LoggedUserPM.ShowLocalNameInLOV) {
                                lang = 'L';
                                _this.ShowLanguageFilter = true;
                            }
                            _this.LanguageFilterValue = lang;
                            _this.DrawColumns();
                            if (!_this.DisplayMemberPath) {
                                _this.SetDisplayMemberPath();
                            }
                            else {
                                _this.DisplayMemberPathManuallySet = true;
                            }
                            if (_this.ObjectTable) {
                                _this.ObjectField = window.ObjectFields.filter(function (d) { return d.ObjectTableId === _this.ObjectTable.Id && d.FieldName === _this.ObjectFieldName; })[0];
                                if (!_this.ObjectField) {
                                    objectFieldAvailable = false;
                                }
                                else {
                                    objectFieldAvailable = true;
                                    if (_this.ObjectField.HelpTextCodeId != null) {
                                        _this.ObjectFieldHelp = TextCodeTranslator_1.TextCodeTranslator.Translate(_this.ObjectField.HelpTextTextCodeCode);
                                        if (!Tools_1.AppTool.IsNullOrEmpty(_this.ObjectFieldHelp)) {
                                            if (_this.ObjectFieldHelp.length > 1) {
                                                if (!_this.IsFreeText)
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
                                    else if (_this.ObjectField.ControlField1) {
                                        _this.DependencyFilter1Value = _this.DataContext[_this.ObjectField.ControlField1];
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
                                    else if (_this.ObjectField.ControlField2) {
                                        _this.DependencyFilter2Value = _this.DataContext[_this.ObjectField.ControlField2];
                                    }
                                    if (_this.ObjectField.DependencyFilter3Value) {
                                        if (_this.ObjectField.DependencyFilter3Type == "Constant") {
                                            _this.DependencyFilter3Value = _this.ObjectField.DependencyFilter3Value;
                                            _this.DependencyFilter3IsList = _this.ObjectField.DependencyFilter3IsList;
                                        }
                                        else {
                                            _this.DependencyFilter3Value = _this.DataContext[_this.ObjectField.DependencyFilter3Value];
                                        }
                                    }
                                    else if (_this.ObjectField.ControlField3) {
                                        _this.DependencyFilter3Value = _this.DataContext[_this.ObjectField.ControlField3];
                                    }
                                    if (_this.ObjectField.ControlField1 || _this.ObjectField.ControlField2 || _this.ObjectField.ControlField3) {
                                        if (_this.DataContext.PropertyChanged != null && _this.DataContext.PropertyChanged != undefined) {
                                            _this.PropertyChangedSubscribtion = _this.DataContext.PropertyChanged.subscribe(function (args) {
                                                if (args.PropertyName == _this.ObjectField.ControlField1) {
                                                    _this.DependencyFilter1Value = _this.DataContext[_this.ObjectField.ControlField1];
                                                    _this.OnDeleteValue();
                                                }
                                                if (args.PropertyName == _this.ObjectField.ControlField2) {
                                                    _this.DependencyFilter2Value = _this.DataContext[_this.ObjectField.ControlField2];
                                                    _this.OnDeleteValue();
                                                }
                                                if (args.PropertyName == _this.ObjectField.ControlField3) {
                                                    _this.DependencyFilter3Value = _this.DataContext[_this.ObjectField.ControlField3];
                                                    _this.OnDeleteValue();
                                                }
                                            });
                                        }
                                    }
                                }
                            }
                            var value = _this.DataContext[_this.ObjectFieldName];
                            if (_this.ObjectField && _this.ObjectField.IsCustom && _this.IgnoreCustomFieldCheck == false) {
                                var customFieldClass = _this.DataContext[_this.ObjectFieldName];
                                if (customFieldClass != null && customFieldClass != undefined) {
                                    value = customFieldClass.Value;
                                }
                                else {
                                    console.warn("Custom Fields are not implemented in: " + _this.ObjectTableName);
                                }
                            }
                            if (value) {
                                _this.GetSingle(_this.LookUpTable);
                            }
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
                        });
                    });
                });
            });
        });
        if (this.LookUpTable.IsClosed)
            this.showOnlyDelete = true;
        if (this.LookUpTable.EnableAddFromLOV) {
            this.isAddDisabled = false;
            if (SessionLocator_1.SessionLocator.Tenant == 65 && !SessionLocator_1.SessionLocator.LoggedUserPM.IsCustomerCare && this.LookUpTableName == "User") {
                this.ShowAddLink = false;
            }
            else {
                this.ShowAddLink = true;
            }
        }
        if (this.LookUpTable.EnableEditFromLOV)
            this.isEditDisabled = false;
        if (this.isAddDisabled && this.isEditDisabled)
            this.showOnlyDelete = true;
        if (this.LookUpTableName == "Card" || this.LookUpTableName == "Carrier") {
            if (this.DependencyFilter1Value != null && this.DependencyFilter1Value != undefined) {
                if (this.DependencyFilter1Value.toString().split(',').length > 1) {
                    this.IsAddTypesVisible = true;
                }
            }
        }
        if (this.HideAddLink) {
            this.isAddDisabled = true;
            this.ShowAddLink = false;
        }
        if (!this.LookUpTable.EnableAddFromLOV) {
            if (this.ForceShowAddLink) {
                this.isAddDisabled = false;
                this.ShowAddLink = true;
            }
        }
        //*ngIf="ShowAddLink || ShowSearchButton"
        if (!this.ShowAddLink && !this.ShowSearchButton) {
            this.MyDropDownHeight = { 'height': '255px' };
        }
        if (!objectFieldAvailable && !this.NoObjectField) {
            console.warn(this.ObjectFieldName + " LOV has no object field!");
        }
    };
    LogLovV2Component.prototype.DrawColumns = function () {
        var _this = this;
        var lookupFields;
        this.headerColumns = [];
        this.dataColumns = [];
        if (this.DisplayFieldsFromList != null && this.DisplayFieldsFromList != undefined) {
            var fields = this.DisplayFieldsFromList.split(',');
            lookupFields = window.ObjectFields.filter(function (d) { return d.ObjectTableId == _this.LookUpTable.Id && fields.lastIndexOf(d.FieldName) > -1; });
        }
        else {
            //if(!SessionInfo.LoggedUserPM.ShowLocalNameInLOV){
            if (this.LanguageFilterValue == 'E') {
                lookupFields = window.ObjectFields.filter(function (d) { return d.DisplayOnLookUp && d.ObjectTableId == _this.LookUpTable.Id; });
            }
            else {
                lookupFields = window.ObjectFields.filter(function (d) { return d.DisplayOnLookUpLocal && d.ObjectTableId == _this.LookUpTable.Id; });
                if (lookupFields.length == 0) {
                    console.warn("There is no Fields defined as display in lookup local");
                    this.ShowLanguageFilter = false;
                    lookupFields = window.ObjectFields.filter(function (d) { return d.DisplayOnLookUp && d.ObjectTableId == _this.LookUpTable.Id; });
                }
            }
        }
        if (!this.DisplayFieldsFromList) {
            lookupFields = lookupFields.sort(function (a, b) { return a.DisplayInLookUpIndex - b.DisplayInLookUpIndex; });
        }
        for (var i = 0; i < lookupFields.length; i++) {
            //this.Widths.push(this.DropDownWidth / lookupFields.length);
            this.MinWidths.push(TextCodeTranslator_1.TextCodeTranslator.Translate(lookupFields[i].ListTextCodeCode).length * 8 + 6);
        }
        //var dropdownWidth = lookupFields.length * 120 + 20;
        //this.DropPopUpStyle = { width: dropdownWidth.toString() + 'px' };
        var additionalCols = lookupFields.filter(function (d) { return d.DisplayInLookUpIndex > 1; });
        if (lookupFields.length == 1) {
            this.DisplayHeader = false;
        }
        else if (!this.IsAllDataVisible) {
            this.DisplayHeader = true;
        }
        if (!this.ShowAddLink && !this.ShowSearchButton) {
            if (this.DisplayHeader) {
                this.DropDownHeight = 278;
            }
            else {
                this.DropDownHeight = 260;
            }
        }
        this.IsReady = true;
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
                        width = 85;
                    }
                    if (additionalCols.length > 1) {
                        width = (400 - 170) / additionalCols.length;
                    }
                }
            }
            this.headerColumns.push({
                Display: TextCodeTranslator_1.TextCodeTranslator.Translate(lookupFields[i].ListTextCodeCode),
                Width: width,
                Field: lookupFields[i].FieldName,
                Index: lookupFields[i].DisplayInLookUpIndex,
            });
            this.dataColumns.push({ Field: lookupFields[i].FieldName });
        }
    };
    LogLovV2Component.prototype.SetDisplayMemberPath = function () {
        if (!this.DisplayMemberPathManuallySet) {
            /// this case we have to show the local display member path if available
            if (this.LanguageFilterValue != 'E') {
                if (this.LookUpTable.LovDisplayMemberPathLocal) {
                    this.DisplayMemberPath = this.LookUpTable.LovDisplayMemberPathLocal;
                }
                else if (this.LookUpTable.LovDisplayMemberPath) {
                    this.DisplayMemberPath = this.LookUpTable.LovDisplayMemberPath;
                }
                else if (this.LookUpTable.LookUp2) {
                    this.DisplayMemberPath = this.LookUpTable.LookUp2;
                }
                else {
                    this.DisplayMemberPath = this.LookUpTable.LookUp1;
                }
            }
            /// this case we have to show the display member path if available
            else {
                if (this.LookUpTable.LovDisplayMemberPath) {
                    this.DisplayMemberPath = this.LookUpTable.LovDisplayMemberPath;
                }
                else if (this.LookUpTable.LookUp2) {
                    this.DisplayMemberPath = this.LookUpTable.LookUp2;
                }
                else {
                    this.DisplayMemberPath = this.LookUpTable.LookUp1;
                }
            }
        }
    };
    LogLovV2Component.prototype.SetIsDisabled = function () {
        this.uiProperty = this.DataContext.UIProperties.GetUIProperty(this.ObjectFieldName, this.ObjectTableName, this.DataContext);
        //console.log(this.uiProperty);
        if (this.AlwaysEnabled == true) {
            this.IsDisabled = false;
        }
        else {
            this.IsDisabled = !this.uiProperty.IsEnabled;
            //console.log("Out " + this.LookUpTableName+ " " + this.uiProperty.IsEnabled)
        }
        if (this.SetIsDisabledTimer) {
            clearTimeout(this.SetIsDisabledTimer);
        }
    };
    LogLovV2Component.prototype.GetSingle = function (lookup) {
        var _this = this;
        var dataContextValue = this.DataContext[this.ObjectFieldName];
        if (this.ObjectField && this.ObjectField.IsCustom && this.IgnoreCustomFieldCheck == false) {
            var customFieldClass = this.DataContext[this.ObjectFieldName];
            if (customFieldClass != null && customFieldClass != undefined) {
                dataContextValue = customFieldClass.Value;
            }
            else {
                console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
            }
        }
        if (dataContextValue || this.IsFreeText) {
            var value = dataContextValue; //this.DataContext[this.ObjectFieldName];
            if (this.IsFreeText)
                value = this.SelectedValue;
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
                                _this.SetToolTipInfo();
                                _this.SelectedItemObject = _this.SelectedItem;
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
                                _this.SetToolTipInfo();
                                _this.SelectedItemObject = _this.SelectedItem;
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
            this.SetToolTipInfo();
            this.SelectedItemObject = null;
            this.ValidateField(true);
            this.SelectedItemChanged.emit(this.SelectedItem);
        }
    };
    LogLovV2Component.prototype.OnWindowClick = function ($event) {
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
    LogLovV2Component.prototype.OnLogLovFocus = function () {
    };
    LogLovV2Component.prototype.OnLogLovKeyDown = function ($event) {
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
    LogLovV2Component.prototype.OnSelected = function (item) {
        this.isSelectedFromList = true;
        this.SelectedItem = item;
        var value = this.DataContext[this.ObjectFieldName];
        if (this.ObjectField && this.ObjectField.IsCustom && this.IgnoreCustomFieldCheck == false) {
            var customFieldClass = this.DataContext[this.ObjectFieldName];
            if (customFieldClass != null && customFieldClass != undefined) {
                customFieldClass.Value = this.SelectedItem[this.SelectedValuePath];
                ;
                this.DataContext[this.ObjectFieldName] = customFieldClass;
                value = customFieldClass.Value;
            }
            else {
                console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
            }
        }
        else {
            this.DataContext[this.ObjectFieldName] = this.SelectedItem[this.SelectedValuePath];
            value = this.DataContext[this.ObjectFieldName];
        }
        this.SelectedItemObject = this.SelectedItem;
        this.SelectedItemChanged.emit(this.SelectedItem);
        this.DisplayValue = this.SelectedItem[this.DisplayMemberPath];
        this.SearchTextNgModel = this.SelectedItem[this.DisplayMemberPath];
        this.SetToolTipInfo();
        this.OldSearchInput = this.SearchTextNgModel;
        this.ValueChanged.emit(value);
        this.ValidateField();
        if (this.IsOpen) {
            this.ToggleOpenDropDown();
        }
        this.show = false;
        this.ShowErrorPopup = false;
        if (this.uiProperty.ValidValue) {
            this.InputDivStyle = null;
        }
        if (this.FocusOnSelect) {
            var elem = document.getElementById(this.ElementId);
            elem.focus();
        }
        this.FocusOnSelect = true;
        this.MouseInArea = false;
        //this.OnBlurEvent.emit({ Id : this.ElementId});
    };
    LogLovV2Component.prototype.OnLogLovClicked = function () {
        this.ToggleOpenDropDown();
        if (this.IsOpen) {
            this.Populate(null);
        }
    };
    LogLovV2Component.prototype.ToggleOpenDropDown = function () {
        this.IsDropDownVisible = !this.IsDropDownVisible;
        this.IsOpen = !this.IsOpen;
        if (this.IsOpen) {
            var input = document.getElementById(this.ElementId);
            input.focus();
            this.showPopup = false;
        }
    };
    LogLovV2Component.prototype.NavigateListItems = function (isDown) {
        if (isDown) {
            var isSelected = false;
            var active = document.getElementsByClassName("highlighted");
            if (!active[0]) {
                if (this.ItemsSource && this.ItemsSource.length > 0) {
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
                if (this.ItemsSource && this.ItemsSource.length > 0) {
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
    LogLovV2Component.prototype.OnSearchIputKeyDown = function ($event) {
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
                            this.FocusOnSelect = false;
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
                                this.FocusOnSelect = false;
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
        //if ((48 <= key && key <= 57) || (65 <= key && key <= 90) || key == 8 || key == 46 || key == 32) {
        //    this.OnDeleteValue(false);
        //}
    };
    LogLovV2Component.prototype.OnLiMouseOver = function ($event) {
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
    LogLovV2Component.prototype.OnLiMouseLeave = function ($event) {
        $event.target.classList.remove("highlighted");
    };
    LogLovV2Component.prototype.HighlightSelectedValue = function (items) {
        var _this = this;
        this.SelectedItemKey = null;
        if (this.SearchTextNgModel != null && this.SearchTextNgModel != undefined && this.SearchTextNgModel != "") {
            var oldItems = items;
            var item = items.filter(function (d) { return d[_this.LookUp1] != null && d[_this.LookUp1].toLowerCase() === _this.SearchTextNgModel.toLowerCase(); })[0];
            if (!item && this.LookUp2) {
                var item = items.filter(function (d) { return d[_this.LookUp2] != null && d[_this.LookUp2].toLowerCase() === _this.SearchTextNgModel.toLowerCase(); })[0];
            }
            if (!item) {
                var item = items.filter(function (d) { return d[_this.DisplayMemberPath] != null && d[_this.DisplayMemberPath].toLowerCase() === _this.SearchTextNgModel.toLowerCase(); })[0];
            }
            if (item) {
                this.ItemsSource = [];
                this.ItemsSource.push(item);
                var index = oldItems.indexOf(item);
                oldItems.splice(index, 1);
                oldItems.forEach(function (itm) {
                    _this.ItemsSource.push(itm);
                });
                //var index = items.indexOf(item);
                //var temp = items[0];
                //items[0] = item;
                //items[index] = temp;
                this.SelectedItemKey = item[this.KeyPropertyPath];
            }
            else if (items.length > 0) {
                this.SelectedItemKey = items[0][this.KeyPropertyPath];
            }
        }
    };
    LogLovV2Component.prototype.Populate = function (searchText, setFirstAsSelected) {
        var _this = this;
        if (setFirstAsSelected === void 0) { setFirstAsSelected = false; }
        this.LovMessage = null;
        if (!FeatureLocator_1.FeatureLocator.HasFeaturePermession(this.LookUpTableName, "Module") && this.LookUpTable.EnableSecurity) {
            this.LovMessage = "Your package doesn't include this module..";
            return;
        }
        else if (!FeatureLocator_1.FeatureLocator.HasFeaturePermession(this.LookUpTableName, "READ") && this.LookUpTable.EnableSecurity) {
            this.LovMessage = "You have no permission to view entities of this type.";
            return;
        }
        //reset counters
        this.bufferData = [];
        this.callCount = 0;
        this.ZeroItemsSourceCount = -1;
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
        if (this.IsDecendingSort == true) {
            filters.SortDirection = "Decending";
        }
        else {
            filters.SortDirection = "Ascending";
        }
        if (this.IsTenantZeroSearch) {
            filters.Tenant = 0;
        }
        this.SetDependencyProperties(filters);
        filters.PageIndex = 0;
        if (this.LookUpTable.SortingByObjectField) {
            filters.SortBy = this.LookUpTable.SortingByObjectField;
        }
        var parentName = this.GetObjectTableName(this.LookUpTableName);
        var parenttable = window.ObjectTables.filter(function (d) { return d.Name === parentName; })[0];
        var originalTable = this.LookUpTable;
        if (originalTable.Name == "Carrier") {
            originalTable = window.ObjectTables.filter(function (d) { return d.Name === "Card"; })[0];
        }
        var inactiveField = window.ObjectFields.filter(function (d) { return d.FieldName.toLowerCase() === "inactive" && (d.ObjectTableId === parenttable.Id || d.ObjectTableId === originalTable.Id); })[0];
        if (inactiveField && !this.ShowInActive) {
            filters.addAdditionalFilter(inactiveField.FieldName, false, null, null, "Equals", false, false, false, null, false, this.LookUpTable.CacheOnClient);
        }
        if (searchText) {
            if (searchText.indexOf('"') > -1) {
                searchText = searchText.replace(/"/g, "");
            }
        }
        if (!this.LookUpTable.CacheOnClient || this.IsTenantZeroSearch) { //calling data from server;
            this.CallDataFromServer(searchText, filters);
        }
        else { //calling data from cache;
            this.CallDataFromCache(searchText, filters, setFirstAsSelected);
        }
        if (this.IsAllDataVisible && !setFirstAsSelected) {
            var tenantZeroFilters;
            if (!this.QueryFilterItems) {
                tenantZeroFilters = new ApiQueryFilters_1.ApiQueryFilters();
            }
            else {
                tenantZeroFilters = new ApiQueryFilters_1.ApiQueryFilters();
                for (var i = 0; i < this.QueryFilterItems.AdditionalFilters.length; i++) {
                    var filter = this.QueryFilterItems.AdditionalFilters[i];
                    tenantZeroFilters.addAdditionalFilter(filter.FieldName, filter.FieldValue, filter.FieldValue2, filter.FieldValue3, filter.Operator, filter.IsCustom, filter.DisplayInList, filter.IsCustomField, filter.FieldDataType, filter.IgnoreFilter, false);
                }
            }
            if (this.IsDecendingSort == true) {
                tenantZeroFilters.SortDirection = "Decending";
            }
            else {
                tenantZeroFilters.SortDirection = "Ascending";
            }
            if (this.LookUpTable.SortingByObjectField) {
                tenantZeroFilters.SortBy = this.LookUpTable.SortingByObjectField;
            }
            tenantZeroFilters.Tenant = 0;
            if (searchText && !this.UseCompactSearch) {
                tenantZeroFilters.addAdditionalFilter("SearchFields", searchText, null, null, "Contains", false, false, false, null, false, false);
            }
            if (inactiveField && !this.ShowInActive) {
                tenantZeroFilters.addAdditionalFilter(inactiveField.FieldName, false, null, null, "Equals", false, false, false, null, false, false);
            }
            this.SetDependencyProperties(tenantZeroFilters);
            tenantZeroFilters.PageIndex = 0;
            if (this.IsAllDataVisible) {
                tenantZeroFilters.PageSize = 5;
            }
            else {
                tenantZeroFilters.PageSize = 10;
            }
            //turn loading flag on
            this.isLoadingZero = true;
            var loadPromise = this.entityListService.getByFilters(this.LookUpTableName, tenantZeroFilters);
            if (this.UseCompactSearch) {
                tenantZeroFilters.addAdditionalFilter("CompactSearchField", searchText, null, null, "Contains", false, false, false, null, false, false);
                loadPromise = this.entityListService.getByCompactFilters(this.LookUpTableName, tenantZeroFilters);
            }
            loadPromise.then(function (res) {
                res.subscribe(function (resp) {
                    //turn loading flag off
                    _this.isLoadingZero = false;
                    if (resp.Result) {
                        _this.CalculateWidths(resp.Result);
                        _this.ZeroItemsSourceCount = resp.Result.length;
                        _this.ZeroItemsSource = resp.Result;
                    }
                    else {
                        _this.CalculateWidths(resp);
                        _this.ZeroItemsSourceCount = resp.length;
                        _this.ZeroItemsSource = resp;
                    }
                });
            });
        }
    };
    LogLovV2Component.prototype.OnSearchInputBlur = function () {
        var _this = this;
        if (!this.MouseInArea) {
            if (this.IsOpen) {
                this.ToggleOpenDropDown();
            }
            clearTimeout(this.focusTimerToken);
            this.show = false;
            this.ShowErrorPopup = false;
            this.ShowMaintenanceBtn = false;
            this.IsPartnerMenuVisible = false;
            if (this.uiProperty.ValidValue) {
                this.timerToken = setTimeout(function () {
                    _this.InputDivStyle = null;
                }, 300);
            }
            this.OnBlurEvent.emit({ Id: this.ElementId });
            this.LostFocus.emit(this.SelectedValue);
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
        // this.OnBlurEvent.emit({ Id: this.ElementId, Close: true });
    };
    LogLovV2Component.prototype.OnMouseOver = function () {
        //this.ShowToolTip = true;
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
        else {
        }
    };
    LogLovV2Component.prototype.OnMouseOut = function () {
        //this.ShowToolTip = false;
        this.MouseInArea = false;
    };
    LogLovV2Component.prototype.SetDependencyProperties = function (apiQueryFilters) {
        var _this = this;
        //var table = window.ObjectTables.filter(d => d.Name === this.LookUpTableName)[0];
        if (this.LookUpTable.DependencyFilter1 != null && this.LookUpTable.DependencyFilter1 != undefined) {
            if (this.DependencyFilter1Value != null && this.DependencyFilter1Value != undefined) {
                var dependencyField = window.ObjectFields.filter(function (d) { return d.ObjectTableId === _this.LookUpTable.Id && d.FieldName === _this.LookUpTable.DependencyFilter1; })[0];
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
                    apiQueryFilters.addAdditionalFilter(dependencyField.FieldName, this.GetFieldValue(dependencyField.DataTypeCode, this.DependencyFilter1Value), null, null, fieldOperator, dependencyField.IsCustomFilter, dependencyField.DisplayInList, dependencyField.IsCustom, dependencyField.DataTypeCode, false, this.LookUpTable.CacheOnClient);
                    //apiQueryFilters.Filter2Name = dependencyField.FieldName;
                    //apiQueryFilters.Filter2Operator = fieldOperator;
                    //apiQueryFilters.Filter2Value = this.GetFieldValue(dependencyField.DataTypeCode, this.DependencyFilter1Value);
                }
            }
        }
        if (this.LookUpTable.DependencyFilter2 != null && this.LookUpTable.DependencyFilter2 != undefined) {
            if (this.DependencyFilter2Value != null && this.DependencyFilter2Value != undefined) {
                var dependencyField2 = window.ObjectFields.filter(function (d) { return d.ObjectTableId === _this.LookUpTable.Id && d.FieldName === _this.LookUpTable.DependencyFilter2; })[0];
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
                    apiQueryFilters.addAdditionalFilter(dependencyField2.FieldName, this.GetFieldValue(dependencyField2.DataTypeCode, this.DependencyFilter2Value), null, null, fieldOperator, dependencyField2.IsCustomFilter, dependencyField2.DisplayInList, dependencyField2.IsCustom, dependencyField2.DataTypeCode, false, this.LookUpTable.CacheOnClient);
                    //apiQueryFilters.Filter3Name = dependencyField2.FieldName;
                    //apiQueryFilters.Filter3Operator = fieldOperator;
                    //apiQueryFilters.Filter3Value = this.GetFieldValue(dependencyField2.DataTypeCode, this.DependencyFilter2Value);
                }
            }
        }
        if (this.LookUpTable.DependencyFilter3 != null && this.LookUpTable.DependencyFilter3 != undefined) {
            if (this.DependencyFilter3Value != null && this.DependencyFilter3Value != undefined) {
                var dependencyField = window.ObjectFields.filter(function (d) { return d.ObjectTableId === _this.LookUpTable.Id && d.FieldName === _this.LookUpTable.DependencyFilter3; })[0];
                if (dependencyField != null) {
                    var fieldOperator = dependencyField.IsCustomFilter ? "Contains" : "Equals";
                    if (this.DependencyFilter3IsList) {
                        fieldOperator = "InList";
                    }
                    if (this.DependencyFilter3IsListExact) {
                        fieldOperator = "InListExact";
                    }
                    apiQueryFilters.addAdditionalFilter(dependencyField.FieldName, this.GetFieldValue(dependencyField.DataTypeCode, this.DependencyFilter3Value), null, null, fieldOperator, dependencyField.IsCustomFilter, dependencyField.DisplayInList, dependencyField.IsCustom, dependencyField.DataTypeCode, false, this.LookUpTable.CacheOnClient);
                }
            }
        }
    };
    LogLovV2Component.prototype.GetFieldValue = function (dataTypeCode, value) {
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
                    date = new Date(value);
                    return date;
                }
            case "boolean":
                {
                    var bb;
                    if (typeof (value) == "string") {
                        if (value.toLowerCase() == 'false') {
                            bb = false;
                        }
                        else if (value.toLowerCase() == 'true') {
                            bb = true;
                        }
                    }
                    else {
                        bb = value;
                    }
                    return bb;
                }
            case "integer":
            case "double":
            case "decimal":
                {
                    var nn;
                    nn = Number(value);
                    return nn;
                }
            default:
                {
                    return value;
                }
        }
    };
    LogLovV2Component.prototype.OnAllDataSelect = function (item) {
        this.CopySelectedItem(item.Id);
    };
    LogLovV2Component.prototype.OnDropDownMouseOver = function () {
        this.MouseInArea = true;
        //this.CanClose = false;
        var input = document.getElementById(this.ElementId);
        input.focus();
    };
    LogLovV2Component.prototype.OnDropDownMouseOut = function () {
        //this.CanClose = true;
        this.MouseInArea = false;
    };
    LogLovV2Component.prototype.OnDeleteValue = function (deleteSearch) {
        if (deleteSearch === void 0) { deleteSearch = true; }
        this.showPopup = false;
        this.ShowMaintenanceBtn = false;
        this.IsPartnerMenuVisible = false;
        this.deleteSearchText = deleteSearch;
        this.selectedValue = null;
        this.SelectedItem = null;
        this.SetToolTipInfo();
        var value = this.DataContext[this.ObjectFieldName];
        if (this.ObjectField && this.ObjectField.IsCustom && this.IgnoreCustomFieldCheck == false) {
            var customFieldClass = this.DataContext[this.ObjectFieldName];
            if (customFieldClass != null && customFieldClass != undefined) {
                customFieldClass.Value = null;
                this.DataContext[this.ObjectFieldName] = customFieldClass;
                value = customFieldClass.Value;
            }
            else {
                console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
            }
        }
        else {
            this.DataContext[this.ObjectFieldName] = null;
            value = this.DataContext[this.ObjectFieldName];
        }
        this.SelectedItemObject = null;
        this.SelectedItemChanged.emit(this.SelectedItem);
        this.DisplayValue = null;
        this.ValueChanged.emit(value);
        if (deleteSearch) {
            this.SearchTextNgModel = null;
            this.OldSearchInput = this.SearchTextNgModel;
            this.ValidateField();
        }
    };
    LogLovV2Component.prototype.SetValidity = function (validValue, errorMessage) {
        this.uiProperty.ValidValue = validValue;
        this.uiProperty.ValidationError = errorMessage;
        if (!validValue) {
            if (this.IsDisabled) {
                this.ShowErrorPopup = false;
                this.show = false;
                this.InputDivStyle = { 'border': '1px solid #AAAAAA', 'opacity': .5, };
            }
            else {
                if (this.searchTextChanged || this.ForceShowValidation) {
                    this.InputDivStyle = { 'border': '1px solid #ff0000', };
                    if (this.show) {
                        this.ShowErrorPopup = true;
                    }
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
    LogLovV2Component.prototype.SearchButtonClicked = function () {
        var _this = this;
        if (!FeatureLocator_1.FeatureLocator.HasFeaturePermession(this.LookUpTableName, "Module") && this.LookUpTable.EnableSecurity) {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show("Your package doesn't include this module..");
            return;
        }
        else if (!FeatureLocator_1.FeatureLocator.HasFeaturePermession(this.LookUpTableName, "READ") && this.LookUpTable.EnableSecurity) {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show("You have no permission to view entities of this type.");
            return;
        }
        if (this.IsOpen) {
            this.ToggleOpenDropDown();
        }
        this.showPopup = false;
        this.ShowMaintenanceBtn = false;
        this.IsPartnerMenuVisible = false;
        this.ShowErrorPopup = false;
        //var ObjectTable = window.ObjectTables.filter(x => x.Name === this.LookUpTableName)[0];
        //var ObjectTableId = window.ObjectTables.filter(x => x.Name === this.LookUpTableName)[0].Id;
        var args = new LogSearchWindowComponent_1.CustomEntityArgs();
        args.ObjectTableId = this.LookUpTable.Id;
        args.ObjectTableName = this.LookUpTableName;
        args.IsTenantZeroSearch = this.IsTenantZeroSearch;
        args.IsAllDataVisible = this.IsAllDataVisible;
        args.ShowInActive = this.ShowInActive;
        args.PartnerTypes = this.PartnerTypes;
        args.DependencyFilter1Value = this.DependencyFilter1Value;
        args.DependencyFilter1IsList = this.DependencyFilter1IsList;
        args.DependencyFilter1IsListExact = this.DependencyFilter1IsListExact;
        args.DependencyFilter2Value = this.DependencyFilter2Value;
        args.DependencyFilter2IsList = this.DependencyFilter2IsList;
        args.DependencyFilter2IsListExact = this.DependencyFilter2IsListExact;
        args.DependencyFilter3Value = this.DependencyFilter3Value;
        args.DependencyFilter3IsList = this.DependencyFilter3IsList;
        args.DependencyFilter3IsListExact = this.DependencyFilter3IsListExact;
        args.UseCompactSearch = this.UseCompactSearch;
        args.QueryFilterItems = this.QueryFilterItems;
        args.HideAdd = this.HideAdd;
        args.HideEdit = this.HideEdit;
        args.IsAddDisabled = this.isAddDisabled;
        args.IsEditDisabled = this.isEditDisabled;
        args.DisplayFieldsFromList = this.DisplayFieldsFromList;
        var tablename = TextCodeTranslator_1.TextCodeTranslator.TranslateTablePlural(this.GetObjectTableName(this.LookUpTableName));
        if (tablename == "Cards") {
            tablename = "Partners";
        }
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 800;
        logitudeWindow.Height = 600;
        if (Tools_1.AppTool.IsMobileDetected()) {
            logitudeWindow.IsFullScreen = true;
        }
        logitudeWindow.WindowArgs = args;
        logitudeWindow.Title = tablename + " Search";
        logitudeWindow.Show('./Infrastructure/Components/LogitudeComponents/LogSearchWindowComponent');
        logitudeWindow.WindowClosed.subscribe(function ($event) { return _this.OnSearchWindowClosed($event); });
    };
    LogLovV2Component.prototype.OnSearchWindowClosed = function (args) {
        var _this = this;
        if (args && args != 'event') { // No value returned
            // Get Selected value
            var id = null;
            var tenant = null;
            if (args.indexOf(',')) {
                var argsarr = args.split(',');
                id = argsarr[0];
                tenant = argsarr[1];
            }
            else {
                id = args;
            }
            if (tenant == 0 && !this.IsTenantZeroSearch) {
                this.CopySelectedItem(id);
            }
            else {
                this.entityListService.getSingle(id, this.LookUpTableName).then(function (res) {
                    res.subscribe(function (myResponse) {
                        if (myResponse != null) {
                            var list = myResponse;
                            if (myResponse instanceof ServiceResponse_1.ServiceResponse) {
                                list = myResponse.Result;
                            }
                            _this.isSelectedFromList = true;
                            _this.SearchTextNgModel = list[_this.DisplayMemberPath];
                            _this.OldSearchInput = _this.SearchTextNgModel;
                            _this.DisplayValue = list[_this.DisplayMemberPath];
                            _this.SelectedItem = list;
                            _this.SetToolTipInfo();
                            var value = _this.DataContext[_this.ObjectFieldName];
                            if (_this.ObjectField && _this.ObjectField.IsCustom && _this.IgnoreCustomFieldCheck == false) {
                                var customFieldClass = _this.DataContext[_this.ObjectFieldName];
                                if (customFieldClass != null && customFieldClass != undefined) {
                                    customFieldClass.Value = _this.SelectedItem[_this.SelectedValuePath];
                                    ;
                                    _this.DataContext[_this.ObjectFieldName] = customFieldClass;
                                    value = customFieldClass.Value;
                                }
                                else {
                                    console.warn("Custom Fields are not implemented in: " + _this.ObjectTableName);
                                }
                            }
                            else {
                                _this.DataContext[_this.ObjectFieldName] = _this.SelectedItem[_this.SelectedValuePath];
                                value = _this.DataContext[_this.ObjectFieldName];
                            }
                            _this.SelectedItemObject = _this.SelectedItem;
                            _this.SelectedItemChanged.emit(_this.SelectedItem);
                            _this.DisplayValue = _this.SelectedItem[_this.DisplayMemberPath];
                            _this.ValueChanged.emit(value);
                            _this.ValidateField();
                            _this.IsDropDownVisible = false;
                            _this.IsOpen = false;
                        }
                    });
                });
            }
            //var item = this.GetSingle(args);
            ////var item = args.SelectedItem;
            //this.isSelectedFromList = true;
            //this.SelectedItem = item;
            //this.DataContext[this.ObjectFieldName] = this.SelectedItem[this.SelectedValuePath];
            //this.SelectedItemObject = this.SelectedItem;
            //this.SelectedItemChanged.emit(this.SelectedItem);
            //this.DisplayValue = this.SelectedItem[this.DisplayMemberPath];
            //this.SearchTextNgModel = this.SelectedItem[this.DisplayMemberPath];
            //this.ValidateField();
            ////this.ToggleOpenDropDown();
            //this.IsDropDownVisible = false;
            //this.IsOpen = false;
        }
    };
    LogLovV2Component.prototype.ValidateField = function (emitPropertyChanged) {
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
    LogLovV2Component.prototype.OnFocus = function () {
        var _this = this;
        this.show = true;
        this.showPopup = false;
        this.focusTimerToken = setTimeout(function () {
            if (_this.uiProperty.ValidValue) {
                _this.InputDivStyle = { 'border': '1px solid #3BB3E2' };
                _this.ShowErrorPopup = false;
            }
            else if (_this.searchTextChanged || _this.ForceShowValidation) {
                _this.InputDivStyle = { 'border': '1px solid #ff0000' };
                _this.ShowErrorPopup = true;
            }
            if (!_this.IsOpen) {
                var input = document.getElementById(_this.ElementId);
                Selection(input);
                //input.select();
            }
        }, 1);
    };
    LogLovV2Component.prototype.OnBlur = function () {
        this.show = false;
        this.showPopup = false;
        this.ShowErrorPopup = false;
        if (this.uiProperty.ValidValue) {
            this.InputDivStyle = null;
        }
    };
    LogLovV2Component.prototype.OnToggleClicked = function () {
        this.ToggleOpenDropDown();
        if (this.IsOpen) {
            this.Populate(null);
        }
    };
    LogLovV2Component.prototype.TogglePopup = function () {
        this.showPopup = !this.showPopup;
    };
    // Tool Button On Click
    LogLovV2Component.prototype.OnMaintenanceClick = function () {
        this.CheckEditAddEnable();
        var input = document.getElementById(this.ElementId);
        input.focus();
        this.TogglePopup();
        this.IsOpen = false;
        this.IsDropDownVisible = false;
    };
    LogLovV2Component.prototype.ColWidth = function (index) {
        return this.Widths[index];
    };
    LogLovV2Component.prototype.CalculateWidths = function (items) {
        var _this = this;
        if (items.length > 0) {
            this.Widths = [];
            //var lookup = window.ObjectTables.filter(d => d.Name === this.LookUpTableName)[0];
            var lookupFields;
            if (this.DisplayFieldsFromList != null && this.DisplayFieldsFromList != undefined) {
                var fields = this.DisplayFieldsFromList.split(',');
                lookupFields = window.ObjectFields.filter(function (d) { return d.ObjectTableId == _this.LookUpTable.Id && fields.lastIndexOf(d.FieldName) > -1; });
            }
            else {
                //if(!SessionInfo.LoggedUserPM.ShowLocalNameInLOV){
                if (this.LanguageFilterValue == 'E') {
                    lookupFields = window.ObjectFields.filter(function (d) { return d.DisplayOnLookUp && d.ObjectTableId == _this.LookUpTable.Id; });
                }
                else {
                    lookupFields = window.ObjectFields.filter(function (d) { return d.DisplayOnLookUpLocal && d.ObjectTableId == _this.LookUpTable.Id; });
                    if (lookupFields.length == 0) {
                        console.warn("There is no Fields defined as display in lookup local");
                        this.ShowLanguageFilter = false;
                        lookupFields = window.ObjectFields.filter(function (d) { return d.DisplayOnLookUp && d.ObjectTableId == _this.LookUpTable.Id; });
                    }
                }
            }
            if (!this.DisplayFieldsFromList) {
                lookupFields = lookupFields.sort(function (a, b) { return a.DisplayInLookUpIndex - b.DisplayInLookUpIndex; });
            }
            for (var i = 0; i < lookupFields.length; i++) {
                var words = [];
                for (var j = 0; j < items.length; j++) {
                    words.push(items[j][lookupFields[i].FieldName]);
                }
                var value = this.GetTheLongestWord(words);
                var maxLength = value.length;
                if (maxLength > 22) {
                    maxLength = 22;
                }
                var width = (maxLength * 7) + 8;
                if (i > 1 && width > 85) {
                    width = 85;
                }
                this.Widths.push(width);
            }
            var sum = 0;
            for (var i = 0; i < this.Widths.length; i++) {
                if (i != 1) {
                    sum += (this.Widths[i] > this.MinWidths[i] ? this.Widths[i] : this.MinWidths[i]);
                }
            }
            this.Widths[1] = this.DropDownWidth - sum;
        }
    };
    LogLovV2Component.prototype.GetTheLongestWord = function (words) {
        var maxWord = '';
        for (var i = 0; i < words.length; i++) {
            if (words[i]) {
                if (words[i].length > maxWord.length) {
                    maxWord = words[i];
                }
            }
        }
        return maxWord;
    };
    // Edit Button Commands
    LogLovV2Component.prototype.OnEditValue = function () {
        var _this = this;
        if (!FeatureLocator_1.FeatureLocator.HasFeaturePermession(this.LookUpTableName, "UPDATE") && this.LookUpTable.EnableSecurity) {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show("You have no permission to edit an entity of this type.");
            return;
        }
        if (!FeatureLocator_1.FeatureLocator.HasFeaturePermession(this.LookUpTableName, "Module") && this.LookUpTable.EnableSecurity) {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show("Your package doesn't include this module..");
            return;
        }
        if (this.isEditDisabled)
            return;
        if (!this.IsPickList) {
            var currentEntity = this.SelectedItem;
            if (currentEntity != null && currentEntity != null) {
                //disableValidationPopups = true;
                ////validationPopup.IsOpen = false;
                ////warningPopup.IsOpen = false;
                //SetWarningPopupVisibility(false);
                //SetValidationPopupVisibility(false);
                var objectTableName = this.LookUpTableName;
                if (objectTableName == "Card" || objectTableName == "Carrier") {
                    objectTableName = this.GetObjectTableNameForDependency(this.SelectedItem["PartnerTypeId"], objectTableName);
                }
                //if (objectTableName == "Card" || objectTableName == "Carrier") {
                //    objectTableName = this.GetObjectTableName(objectTableName);
                //}
                if (!Tools_1.AppTool.IsNullOrEmpty(currentEntity.Id)) {
                    var logWindow = new LogitudeWindow_1.LogitudeWindow();
                    logWindow.Title = TextCodeTranslator_1.TextCodeTranslator.TranslateTable("General.B.Edit") + " " + TextCodeTranslator_1.TextCodeTranslator.TranslateTable(objectTableName);
                    logWindow.ShowEditComponent(currentEntity.Id, objectTableName);
                    logWindow.WindowClosed.subscribe(function ($event) {
                        //if ($event)
                        _this.OnEditCompleted();
                    });
                }
                //SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                //    .then(cmpRef => {
                //        cmpRef.instance.ComponentRef = cmpRef;
                //        cmpRef.instance.Run({ EntityId: currentEntity.Id, ObjectTableName: objectTableName });
                //        cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                //            if ($event)
                //                this.OnEditCompleted();
                //        });
                //    });
            }
        }
        this.ShowMaintenanceBtn = false;
        this.IsPartnerMenuVisible = false;
        this.showPopup = false;
        if (this.IsOpen) {
            this.ToggleOpenDropDown();
        }
    };
    LogLovV2Component.prototype.OnEditCompleted = function () {
        var _this = this;
        var value = this.DataContext[this.ObjectFieldName];
        if (this.ObjectField && this.ObjectField.IsCustom) {
            var customFieldClass = this.DataContext[this.ObjectFieldName];
            if (customFieldClass != null && customFieldClass != undefined) {
                customFieldClass.Value = this.SelectedItem[this.SelectedValuePath];
                ;
                value = customFieldClass.Value;
            }
            else {
                console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
            }
        }
        this.entityListService.getSingle(value, this.LookUpTableName).then(function (res) {
            res.subscribe(function (myResponse) {
                if (myResponse != null) {
                    var list = myResponse;
                    if (myResponse instanceof ServiceResponse_1.ServiceResponse) {
                        list = myResponse.Result;
                    }
                    _this.SearchTextNgModel = list[_this.DisplayMemberPath];
                    _this.OldSearchInput = _this.SearchTextNgModel;
                    _this.DisplayValue = list[_this.DisplayMemberPath];
                    _this.SelectedItem = list;
                    _this.SetToolTipInfo();
                    _this.SelectedItemObject = _this.SelectedItem;
                    _this.SelectedItemChanged.emit(_this.SelectedItem);
                    var dateContextValue = _this.DataContext[_this.ObjectFieldName];
                    if (_this.ObjectField && _this.ObjectField.IsCustom && _this.IgnoreCustomFieldCheck == false) {
                        var customFieldClass = _this.DataContext[_this.ObjectFieldName];
                        if (customFieldClass != null && customFieldClass != undefined) {
                            customFieldClass.Value = _this.SelectedItem[_this.SelectedValuePath];
                            ;
                            _this.DataContext[_this.ObjectFieldName] = customFieldClass;
                            dateContextValue = customFieldClass.Value;
                        }
                        else {
                            console.warn("Custom Fields are not implemented in: " + _this.ObjectTableName);
                        }
                    }
                    else {
                        //this.DataContext[this.ObjectFieldName] = null; -------------bug 33534
                        _this.DataContext[_this.ObjectFieldName] = _this.SelectedItem[_this.SelectedValuePath];
                        dateContextValue = _this.DataContext[_this.ObjectFieldName];
                    }
                    _this.ValueChanged.emit(dateContextValue);
                    _this.ValidateField();
                }
            });
        });
        this.showPopup = false;
    };
    // Add Button Commands
    LogLovV2Component.prototype.OnAddValue = function () {
        if (this.isAddDisabled || this.IsAddTypesVisible)
            return;
        this.ShowMaintenanceBtn = false;
        this.IsPartnerMenuVisible = false;
        this.showPopup = false;
        this.ShowErrorPopup = false;
        if (this.IsOpen) {
            this.ToggleOpenDropDown();
        }
        if (!this.IsPickList) {
            this.NewEntityMethod(this.LookUpTableName);
        }
    };
    LogLovV2Component.prototype.NewEntityMethod = function (objectTableName) {
        // var lookup: ObjectTablePM = window.ObjectTables.filter(d => d.Name === objectTableName)[0];
        var _this = this;
        //disableValidationPopups = true;
        //SetWarningPopupVisibility(false);
        //SetValidationPopupVisibility(false);
        objectTableName = this.GetObjectTableName(objectTableName);
        var originalTable = window.ObjectTables.filter(function (d) { return d.Name.toLowerCase() === objectTableName.toLocaleLowerCase(); })[0];
        if (!FeatureLocator_1.FeatureLocator.HasFeaturePermession(this.LookUpTableName, "NEW") && this.LookUpTable.EnableSecurity) {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show("You have no permission to add a new entity of this type.");
            return;
        }
        if (!FeatureLocator_1.FeatureLocator.HasFeaturePermession(this.LookUpTableName, "Module") && this.LookUpTable.EnableSecurity) {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Show("Your package doesn't include this module..");
            return;
        }
        if (this.TenantPM.Id != 0) {
            if (objectTableName == "Port" || objectTableName == "Airline" || objectTableName == "Carrier" || objectTableName == "ShippingLine") {
                var windowTitle = "Add " + objectTableName;
                var logWindow = new LogitudeWindow_1.LogitudeWindow();
                var args = new TenantImportComponent_1.ImportEntityArgs();
                args.ObjectTableId = this.LookUpTable.Id;
                args.ObjectTableName = objectTableName;
                logWindow.WindowArgs = args;
                logWindow.Width = 1000;
                logWindow.Height = 600;
                logWindow.Title = windowTitle;
                logWindow.Show('./Common/Components/Maintenance/TenantImportComponent');
                logWindow.WindowClosed.subscribe(function ($event) {
                    var tablename = _this.GetObjectTableName(_this.LookUpTableName);
                    CachedDataManager_1.CachedDataManager.RefreshTableData(tablename, true);
                    _this.OnNewEntityWindowClosed($event);
                });
                return;
            }
        }
        if (originalTable.IsNewWizard) {
            this.RunNewEntityWizard(originalTable.NewWizardComponentPath, originalTable.Name);
        }
        else {
            this.RunNewGenaricEntity();
        }
    };
    LogLovV2Component.prototype.RunNewEntityWizard = function (newWizardComponentPath, originalTableName) {
        //var ObjectTable = window.ObjectTables.filter(x => x.Name === this.LookUpTableName)[0];
        var _this = this;
        var componentPath = newWizardComponentPath; //this.LookUpTable.NewWizardComponentPath;
        if (componentPath != null) {
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            switch (this.LookUpTableName) {
                case "Customs.Client": {
                    logWindow.Width = 800;
                    logWindow.Height = 600;
                    logWindow.IsShowCloseButton = true;
                    break;
                }
                case "Customs.Declaration":
                case "Customs.PaymentOrder":
                    {
                        logWindow.Width = 400;
                        logWindow.Height = 300;
                        logWindow.IsShowCloseButton = true;
                        break;
                    }
                case "Customs.CustomsVendor": {
                    logWindow.IsShowCloseButton = true;
                    break;
                }
                case "User": {
                    logWindow.Width = 965;
                    logWindow.Height = 600;
                    break;
                }
            }
            var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator_1.TextCodeTranslator.Translate(originalTableName));
            logWindow.Title = windowTitle;
            logWindow.WindowClosed.subscribe(function ($event) { return _this.OnNewEntityWindowClosed($event); });
            logWindow.Show(componentPath);
        }
        else {
            var messageWindow = new MessageWindow_1.MessageWindow();
            messageWindow.Width = 450;
            messageWindow.Height = 190;
            messageWindow.Show("Fill NewWizard Component Path and Name in ObjectTable !!");
        }
    };
    LogLovV2Component.prototype.RunNewGenaricEntity = function () {
        var _this = this;
        var componentPath = "./Infrastructure/GenericComponents/NewEntityComponent";
        this.entityPMService.getNewEntity(this.LookUpTableName).then(function (response) {
            var args = new AddEntityArgs();
            args.EntityPM = response;
            args.ObjectTableName = _this.LookUpTableName;
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.Width = 960;
            logWindow.Height = 570;
            var windowTitle = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NewEntity").replace("%Entity", TextCodeTranslator_1.TextCodeTranslator.Translate(_this.LookUpTableName));
            logWindow.WindowArgs = args;
            logWindow.Title = windowTitle;
            logWindow.WindowClosed.subscribe(function ($event) { return _this.OnNewEntityWindowClosed($event); });
            logWindow.Show(componentPath);
        });
    };
    LogLovV2Component.prototype.OnNewEntityWindowClosed = function ($event) {
        var _this = this;
        //console.log($event);
        if ($event && $event != "event") {
            this.entityListService.getSingle($event, this.LookUpTableName).then(function (res) {
                res.subscribe(function (myResponse) {
                    if (myResponse != null) {
                        if (_this.LookUpTable.CacheOnClient) {
                            CachedDataManager_1.CachedDataManager.RefreshTableData(_this.LookUpTableName, true);
                        }
                        var list = myResponse;
                        if (myResponse instanceof ServiceResponse_1.ServiceResponse) {
                            list = myResponse.Result;
                        }
                        _this.SearchTextNgModel = list[_this.DisplayMemberPath];
                        _this.OldSearchInput = _this.SearchTextNgModel;
                        _this.DisplayValue = list[_this.DisplayMemberPath];
                        _this.SelectedItem = list;
                        _this.SetToolTipInfo();
                        _this.SelectedItemObject = _this.SelectedItem;
                        _this.SelectedItemChanged.emit(_this.SelectedItem);
                        _this.selectedValue = _this.SelectedItem[_this.SelectedValuePath];
                        var value = _this.DataContext[_this.ObjectFieldName];
                        if (_this.ObjectField && _this.ObjectField.IsCustom && _this.IgnoreCustomFieldCheck == false) {
                            var customFieldClass = _this.DataContext[_this.ObjectFieldName];
                            if (customFieldClass != null && customFieldClass != undefined) {
                                customFieldClass.Value = _this.SelectedItem[_this.SelectedValuePath];
                                ;
                                _this.DataContext[_this.ObjectFieldName] = customFieldClass;
                                value = customFieldClass.Value;
                            }
                            else {
                                console.warn("Custom Fields are not implemented in: " + _this.ObjectTableName);
                            }
                        }
                        else {
                            _this.DataContext[_this.ObjectFieldName] = _this.SelectedItem[_this.SelectedValuePath];
                            value = _this.DataContext[_this.ObjectFieldName];
                        }
                        _this.ValidateField();
                    }
                });
            });
            this.showPopup = false;
        }
    };
    //****
    LogLovV2Component.prototype.CheckEditAddEnable = function () {
        //var lookup = window.ObjectTables.filter(d => d.Name === this.LookUpTableName)[0];
        if (this.SelectedItem == null) {
            this.isEditDisabled = true;
            this.isDeleteDisabled = true;
        }
        else {
            this.isEditDisabled = false;
            this.isDeleteDisabled = false;
            if (this.TenantPM.Id == 65 && !SessionLocator_1.SessionLocator.LoggedUserPM.IsCustomerCare) {
                if (this.LookUpTableName == "ChargesType" || this.LookUpTableName == "User") {
                    this.isEditDisabled = true;
                }
            }
        }
        if (!this.IsPickList) {
            if ((this.LookUpTable.EnableAddFromLOV) && !this.HideAdd) {
                this.isAddDisabled = false;
                if (SessionLocator_1.SessionLocator.Tenant == 65 && !SessionLocator_1.SessionLocator.LoggedUserPM.IsCustomerCare && this.LookUpTableName == "User") {
                    this.ShowAddLink = false;
                }
                else {
                    this.ShowAddLink = true;
                }
                if (this.TenantPM.Id == 65 && !SessionLocator_1.SessionLocator.LoggedUserPM.IsCustomerCare) {
                    if (this.LookUpTableName == "ChargesType" || this.LookUpTableName == "User") {
                        this.isAddDisabled = true;
                        this.ShowAddLink = false;
                    }
                }
            }
            else {
                this.isAddVisible = false;
            }
            if (!this.LookUpTable.EnableEditFromLOV || this.HideEdit) {
                this.isEditVisible = false;
            }
        }
        if (this.LookUpTableName == "Card" || this.LookUpTableName == "Carrier") {
            if (this.DependencyFilter1Value != null && this.DependencyFilter1Value != undefined) {
                if (this.DependencyFilter1Value.toString().split(',').length > 1) {
                    this.IsAddTypesVisible = true;
                }
            }
        }
    };
    LogLovV2Component.prototype.GetObjectTableName = function (parentObjectName) {
        var dep = this.DependencyFilter1Value != null ? this.DependencyFilter1Value.toString() : "";
        return this.GetObjectTableNameForDependency(dep, parentObjectName);
    };
    LogLovV2Component.prototype.GetObjectTableNameForDependency = function (dependency, parentObjectName) {
        var partnerType = this.PartnerTypes.filter(function (p) { return p.Id.toLowerCase() == dependency.toLowerCase(); })[0];
        if (partnerType != null && partnerType != undefined) {
            var name = partnerType.Name.replace(" ", "");
            if (name == "PotentialCustomer") {
                name = "Customer";
            }
            var table = window.ObjectTables.filter(function (d) { return d.Name.toLowerCase() === name.toLocaleLowerCase(); })[0];
            if (table != null) {
                return table.Name;
            }
            else {
                return parentObjectName;
            }
        }
        else {
            return parentObjectName;
        }
    };
    LogLovV2Component.prototype.OnMouseHover = function () {
        var _this = this;
        this.onhover = true;
        this.timerToken = setTimeout(function () {
            if (_this.onhover) {
                //this.show = true;
                _this.showPopup = false;
                if (SessionLocator_1.SessionLocator.Tenant == 65 && !SessionLocator_1.SessionLocator.LoggedUserPM.IsCustomerCare && _this.LookUpTableName == "User") {
                    _this.ShowMaintenanceBtn = false;
                }
                else {
                    _this.ShowMaintenanceBtn = true;
                }
            }
        }, 700);
    };
    LogLovV2Component.prototype.OnMouseLeave = function () {
        var _this = this;
        this.onhover = false;
        this.timerToken = setTimeout(function () {
            if (!_this.showPopup && !_this.MouseInArea) {
                _this.ShowMaintenanceBtn = false;
                _this.IsPartnerMenuVisible = false;
                _this.showPopup = false;
            }
        }, 700);
    };
    LogLovV2Component.prototype.CopySelectedItem = function (id) {
        var _this = this;
        this.entityListService.getEntityCopyToCurrentTenant(id, this.LookUpTableName).then(function (res) {
            res.subscribe(function (myResponse) {
                if (myResponse != null) {
                    var list = myResponse;
                    if (myResponse instanceof ServiceResponse_1.ServiceResponse) {
                        list = myResponse.Result;
                    }
                    _this.isSelectedFromList = true;
                    _this.SelectedItem = list;
                    _this.SetToolTipInfo();
                    var value = _this.DataContext[_this.ObjectFieldName];
                    if (_this.ObjectField && _this.ObjectField.IsCustom && _this.IgnoreCustomFieldCheck == false) {
                        var customFieldClass = _this.DataContext[_this.ObjectFieldName];
                        if (customFieldClass != null && customFieldClass != undefined) {
                            customFieldClass.Value = _this.SelectedItem[_this.SelectedValuePath];
                            ;
                            _this.DataContext[_this.ObjectFieldName] = customFieldClass;
                            value = customFieldClass.Value;
                        }
                        else {
                            console.warn("Custom Fields are not implemented in: " + _this.ObjectTableName);
                        }
                    }
                    else {
                        _this.DataContext[_this.ObjectFieldName] = _this.SelectedItem[_this.SelectedValuePath];
                        value = _this.DataContext[_this.ObjectFieldName];
                    }
                    _this.SelectedItemObject = _this.SelectedItem;
                    _this.SelectedItemChanged.emit(_this.SelectedItem);
                    _this.DisplayValue = _this.SelectedItem[_this.DisplayMemberPath];
                    _this.SearchTextNgModel = _this.SelectedItem[_this.DisplayMemberPath];
                    _this.OldSearchInput = _this.SearchTextNgModel;
                    if (_this.IsOpen) {
                        _this.ToggleOpenDropDown();
                    }
                    _this.ValidateField();
                    CachedDataManager_1.CachedDataManager.RefreshTableData(_this.LookUpTableName, true);
                    // this.OnBlurEvent.emit("");
                }
            });
        });
    };
    LogLovV2Component.prototype.CallDataFromCache = function (searchText, filters, setFirstAsSelected) {
        var _this = this;
        if (setFirstAsSelected === void 0) { setFirstAsSelected = false; }
        if (!this.LookUpTable.AutoCompleteSearchWindow) {
            filters.PageSize = 1000;
        }
        else {
            if (!this.IsAllDataVisible) {
                filters.PageSize = 10;
            }
            else {
                filters.PageSize = 5;
            }
        }
        if (this.UseCompactSearch) {
            filters.removeAdditionalFilter("CompactSearchField");
        }
        if (searchText) {
            //if (this.QueryFilterItems && this.QueryFilterItems.AdditionalFilters.length > 0 && this.callCount == 0) {
            //    this.callCount = 1;
            //}
            if (this.currentFilter == null || this.currentFilter == undefined) {
                this.callCount = 1;
                var forceEnableAdd = false;
                if (this.QueryFilterItems && this.QueryFilterItems.AdditionalFilters.length > 0) {
                    for (var i = 0; i < this.QueryFilterItems.AdditionalFilters.length; i++) {
                        var filter = this.QueryFilterItems.AdditionalFilters[i];
                        if (this.LookUp1 == filter.FieldName) {
                            forceEnableAdd = true;
                        }
                    }
                }
                filters.addAdditionalFilter(this.LookUp1, searchText, null, null, "StartsWith", false, false, false, null, false, this.LookUpTable.CacheOnClient, forceEnableAdd);
                this.currentFilter = this.LookUp1;
            }
            else if (this.currentFilter == this.LookUp1 && this.LookUp2 != null && this.LookUp2 != undefined && this.LookUp1 != this.LookUp2) {
                this.callCount = 2;
                var forceEnableAdd = false;
                if (this.QueryFilterItems && this.QueryFilterItems.AdditionalFilters.length > 0) {
                    for (var i = 0; i < this.QueryFilterItems.AdditionalFilters.length; i++) {
                        var filter = this.QueryFilterItems.AdditionalFilters[i];
                        if (this.LookUp2 == filter.FieldName) {
                            forceEnableAdd = true;
                        }
                    }
                }
                if (this.LookUpTable.DependencyFilter1 != this.LookUp1 && this.LookUpTable.DependencyFilter2 != this.LookUp1 && this.LookUpTable.DependencyFilter3 != this.LookUp1) {
                    filters.removeAdditionalFilter(this.LookUp1);
                }
                filters.addAdditionalFilter(this.LookUp2, searchText, null, null, "StartsWith", false, false, false, null, false, this.LookUpTable.CacheOnClient, forceEnableAdd);
                this.currentFilter = this.LookUp2;
            }
            else {
                this.callCount = 3;
                var forceEnableAdd = false;
                if (this.QueryFilterItems && this.QueryFilterItems.AdditionalFilters.length > 0) {
                    for (var i = 0; i < this.QueryFilterItems.AdditionalFilters.length; i++) {
                        var filter = this.QueryFilterItems.AdditionalFilters[i];
                        if ("SearchFields" == filter.FieldName) {
                            forceEnableAdd = true;
                        }
                    }
                }
                if (this.LookUpTable.DependencyFilter1 != this.LookUp1 && this.LookUpTable.DependencyFilter2 != this.LookUp1 && this.LookUpTable.DependencyFilter3 != this.LookUp1) {
                    filters.removeAdditionalFilter(this.LookUp1);
                }
                if (this.LookUpTable.DependencyFilter1 != this.LookUp2 && this.LookUpTable.DependencyFilter2 != this.LookUp2 && this.LookUpTable.DependencyFilter3 != this.LookUp2) {
                    filters.removeAdditionalFilter(this.LookUp2);
                }
                filters.addAdditionalFilter("SearchFields", searchText, null, null, "Contains", false, false, false, null, false, this.LookUpTable.CacheOnClient, forceEnableAdd);
                this.currentFilter = null;
            }
        }
        //turn loading flag on
        this.isLoading = true;
        this.entityListService.getAllFromCache(this.LookUpTableName, filters).then(function (res) {
            res.subscribe(function (resp) {
                if (resp.Result) {
                    for (var i = 0; i < resp.Result.length; i++) {
                        var item = _this.bufferData.filter(function (d) { return d[_this.LookUpTable.KeyPropertyPath] == resp.Result[i][_this.LookUpTable.KeyPropertyPath]; })[0];
                        if (!item) {
                            _this.bufferData.push(resp.Result[i]);
                        }
                    }
                    if (setFirstAsSelected) {
                        if (_this.bufferData.length > 0) {
                            _this.FocusOnSelect = false;
                            var item = _this.bufferData.filter(function (d) { return d[_this.LookUp1] != null && d[_this.LookUp1].toLowerCase() === _this.SearchTextNgModel.toLowerCase(); })[0];
                            if (!item && _this.LookUp2) {
                                var item = _this.bufferData.filter(function (d) { return d[_this.LookUp2] != null && d[_this.LookUp2].toLowerCase() === _this.SearchTextNgModel.toLowerCase(); })[0];
                            }
                            if (!item) {
                                var item = _this.bufferData.filter(function (d) { return d[_this.DisplayMemberPath].toLowerCase() === _this.SearchTextNgModel.toLowerCase(); })[0];
                            }
                            if (item) {
                                _this.OnSelected(item);
                            }
                            else {
                                _this.SearchTextNgModel = null;
                                _this.OldSearchInput = _this.SearchTextNgModel;
                                _this.ValidateField();
                            }
                            _this.callCount = 0;
                            return;
                        }
                        else if (_this.callCount == 3) {
                            _this.SearchTextNgModel = null;
                            _this.OldSearchInput = _this.SearchTextNgModel;
                            _this.ValidateField();
                            return;
                        }
                    }
                    if (_this.bufferData.length < filters.PageSize && _this.callCount < 3 && _this.callCount > 0) {
                        filters.PageSize = filters.PageSize - _this.bufferData.length;
                        _this.CallDataFromCache(searchText, filters, setFirstAsSelected);
                        return;
                    }
                    _this.callCount = 0;
                    _this.CalculateWidths(_this.bufferData);
                    if (_this.ManipulateData) {
                        _this.ApplyManipulateData(_this.bufferData);
                    }
                    else {
                        _this.ItemsSource = _this.bufferData; //resp.Result;
                        _this.ItemsSourceCount = _this.ItemsSource.length; //resp.Result.length;
                    }
                }
                else {
                    for (var i = 0; i < resp.length; i++) {
                        var item = _this.bufferData.filter(function (d) { return d[_this.LookUp1] == resp[i][_this.LookUp1]; })[0];
                        if (!item) {
                            _this.bufferData.push(resp[i]);
                        }
                    }
                    if (setFirstAsSelected) {
                        if (_this.bufferData.length > 0) {
                            _this.FocusOnSelect = false;
                            _this.OnSelected(_this.bufferData[0]);
                            _this.callCount = 0;
                            return;
                        }
                        else if (_this.callCount == 3) {
                            _this.SearchTextNgModel = null;
                            _this.OldSearchInput = _this.SearchTextNgModel;
                            _this.ValidateField();
                            return;
                        }
                    }
                    if (_this.bufferData.length < filters.PageSize && _this.callCount < 3 && _this.callCount > 0) {
                        filters.PageSize = filters.PageSize - _this.bufferData.length;
                        _this.CallDataFromCache(searchText, filters, setFirstAsSelected);
                        return;
                    }
                    _this.callCount = 0;
                    _this.CalculateWidths(_this.bufferData);
                    if (_this.ManipulateData) {
                        _this.ApplyManipulateData(_this.bufferData);
                    }
                    else {
                        _this.ItemsSource = _this.bufferData; //resp.Result;
                        _this.ItemsSourceCount = _this.ItemsSource.length; //resp.Result.length;
                    }
                }
                if (!_this.ManipulateData) {
                    if (_this.ItemsSourceCount == 0) {
                        //this.LovMessage = "No more results founds";
                        _this.LovMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NoMoreResult");
                    }
                    else {
                        _this.LovMessage = null;
                    }
                    _this.ItemsSourceStatic = _this.ItemsSource;
                    _this.HighlightSelectedValue(_this.ItemsSource);
                    _this.isLoading = false;
                }
            });
        });
    };
    LogLovV2Component.prototype.CallDataFromServer = function (searchText, filters) {
        var _this = this;
        //turn loading flag on
        //if (searchText && !this.UseCompactSearch) {
        //    filters.addAdditionalFilter("SearchFields", searchText, null, null, "Contains", false, false, false, null);
        //}
        if (!this.LookUpTable.AutoCompleteSearchWindow) {
            filters.PageSize = 50;
        }
        else {
            if (!this.IsAllDataVisible) {
                filters.PageSize = 10;
            }
            else {
                filters.PageSize = 5;
            }
        }
        this.isLoading = true;
        var loadPromise = this.entityListService.getByFilters(this.LookUpTableName, filters);
        if (this.UseCompactSearch) {
            filters.addAdditionalFilter("CompactSearchField", searchText, null, null, "Contains", false, false, false, null);
            loadPromise = this.entityListService.getByCompactFilters(this.LookUpTableName, filters);
        }
        else if (searchText) {
            searchText = searchText.replace(/\\/g, "\\\\");
            if (this.currentFilter == null || this.currentFilter == undefined) {
                this.callCount = 1;
                filters.addAdditionalFilter(this.LookUp1, searchText, null, null, "StartsWith", false, false, false, null);
                this.currentFilter = this.LookUp1;
            }
            else if (this.currentFilter == this.LookUp1 && this.LookUp2 != null && this.LookUp2 != undefined && this.LookUp1 != this.LookUp2) {
                this.callCount = 2;
                if (this.LookUpTable.DependencyFilter1 != this.LookUp1 && this.LookUpTable.DependencyFilter2 != this.LookUp1 && this.LookUpTable.DependencyFilter3 != this.LookUp1) {
                    filters.removeAdditionalFilter(this.LookUp1);
                }
                filters.addAdditionalFilter(this.LookUp2, searchText, null, null, "StartsWith", false, false, false, null);
                this.currentFilter = this.LookUp2;
            }
            else {
                this.callCount = 3;
                if (this.LookUpTable.DependencyFilter1 != this.LookUp1 && this.LookUpTable.DependencyFilter2 != this.LookUp1 && this.LookUpTable.DependencyFilter3 != this.LookUp1) {
                    filters.removeAdditionalFilter(this.LookUp1);
                }
                if (this.LookUpTable.DependencyFilter1 != this.LookUp2 && this.LookUpTable.DependencyFilter2 != this.LookUp2 && this.LookUpTable.DependencyFilter3 != this.LookUp2) {
                    filters.removeAdditionalFilter(this.LookUp2);
                }
                filters.addAdditionalFilter("SearchFields", searchText, null, null, "Contains", false, false, false, null);
                this.currentFilter = null;
            }
        }
        loadPromise.then(function (res) {
            res.subscribe(function (resp) {
                if (resp.Result) {
                    for (var i = 0; i < resp.Result.length; i++) {
                        var item = _this.bufferData.filter(function (d) { return d[_this.LookUpTable.KeyPropertyPath] == resp.Result[i][_this.LookUpTable.KeyPropertyPath]; })[0];
                        if (!item) {
                            _this.bufferData.push(resp.Result[i]);
                        }
                    }
                    if (_this.bufferData.length < filters.PageSize && _this.callCount < 3 && !_this.UseCompactSearch && _this.callCount > 0) {
                        _this.CallDataFromServer(searchText, filters);
                        return;
                    }
                    _this.callCount = 0;
                    _this.CalculateWidths(_this.bufferData);
                    if (_this.ManipulateData) {
                        _this.ApplyManipulateData(_this.bufferData);
                    }
                    else {
                        _this.ItemsSource = _this.bufferData; //resp.Result;
                        _this.ItemsSourceCount = _this.ItemsSource.length; //resp.Result.length;
                    }
                }
                else {
                    for (var i = 0; i < resp.length; i++) {
                        _this.bufferData.push(resp[i]);
                    }
                    if (_this.bufferData.length < filters.PageSize) {
                        _this.CallDataFromServer(searchText, filters);
                        return;
                    }
                    _this.CalculateWidths(resp);
                    if (_this.ManipulateData) {
                        _this.ApplyManipulateData(_this.bufferData);
                    }
                    else {
                        _this.ItemsSource = _this.bufferData; //resp.Result;
                        _this.ItemsSourceCount = _this.ItemsSource.length; //resp.Result.length;
                    }
                }
                if (!_this.ManipulateData) {
                    if (_this.ItemsSourceCount == 0) {
                        //this.LovMessage = "No more results founds";
                        _this.LovMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NoMoreResult");
                    }
                    else {
                        _this.LovMessage = null;
                    }
                    _this.ItemsSourceStatic = _this.ItemsSource;
                    _this.HighlightSelectedValue(_this.ItemsSource);
                    //turn loading flag off
                    _this.isLoading = false;
                }
            });
        });
    };
    LogLovV2Component.prototype.OnSearchInputKeyUP = function ($event) {
        var CTRL = 17;
        if ($event.keyCode == CTRL) {
            this.IsCTRLDown = false;
        }
    };
    LogLovV2Component.prototype.ApplyManipulateData = function (bufferData) {
        var _this = this;
        var objectTableName = this.LookUpTableName;
        if (this.LookUpTableName.indexOf('Customs.') > -1) {
            objectTableName = objectTableName.split('.')[1];
        }
        var moduleName = this.LookUpTable.ClientModuleName;
        var classname = objectTableName + "DataChangeService";
        var servicelink = './' + moduleName + '/Services/DataChange/' + classname;
        return new Promise(function (resolve, reject) {
            SessionLocator_1.SessionLocator.DynamicLoader.GetInstance(servicelink).then(function (service) {
                _this.ItemsSource = service.ApplyDataChange(bufferData);
                _this.ItemsSourceCount = _this.ItemsSource.length;
                if (_this.ItemsSourceCount == 0) {
                    //this.LovMessage = "No more results founds";
                    _this.LovMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NoMoreResult");
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
    LogLovV2Component.prototype.SetToolTipInfo = function () {
        if (this.SelectedItem != null && !Tools_1.AppTool.IsNullOrEmpty(this.SearchTextNgModel)) {
            var hasCodeField = this.headerColumns.filter(function (f) { return f.Field == "Code"; })[0];
            if (this.LookUp2 != null) {
                var nameText = this.SelectedItem[this.LookUp2];
                var codeText = this.SelectedItem[this.LookUp1];
                this.LovToolTip = codeText + "," + nameText;
            }
            else {
                if (this.LookUp1 != "Code" && hasCodeField) {
                    var actualCodeText = this.SelectedItem["Code"];
                    var codeText = this.SelectedItem[this.LookUp1];
                    this.LovToolTip = actualCodeText + "," + codeText;
                }
            }
        }
        else {
            this.LovToolTip = '';
        }
    };
    LogLovV2Component.prototype.OnMouseOverAdd = function () {
        if (this.IsAddTypesVisible) {
            this.IsPartnerMenuVisible = true;
            var mtcPopup = document.getElementById('MTCPopup');
            var itemRect = mtcPopup.getBoundingClientRect();
            var top = itemRect.top;
            var left = itemRect.left;
            this.PartnersPopupTop = top + 'px';
            this.PartnersPopupLeft = left + 87 + 'px';
        }
    };
    LogLovV2Component.prototype.OnMouseOverEdit = function () {
        if (this.IsAddTypesVisible) {
            this.IsPartnerMenuVisible = false;
        }
    };
    LogLovV2Component.prototype.OnMouseOverDelete = function () {
        if (this.IsAddTypesVisible) {
            this.IsPartnerMenuVisible = false;
        }
    };
    LogLovV2Component.prototype.OnMouseOutAdd = function () {
        //if (this.IsAddTypesVisible) {
        //    this.IsPartnerMenuVisible = false;
        //}
    };
    LogLovV2Component.prototype.AddPartnerOfType = function (partnerType) {
        this.IsPartnerMenuVisible = false;
        this.ShowMaintenanceBtn = false;
        this.showPopup = false;
        this.ShowErrorPopup = false;
        if (this.IsOpen) {
            this.ToggleOpenDropDown();
        }
        var objectTableName = this.GetObjectTableNameForDependency(partnerType.Id, this.LookUpTableName);
        var objectTable = window.ObjectTables.filter(function (d) { return d.Name == objectTableName; })[0];
        if (objectTable.IsNewWizard) {
            this.RunNewEntityWizard(objectTable.NewWizardComponentPath, objectTable.Name);
        }
        else {
            this.RunNewGenaricEntity();
        }
    };
    LogLovV2Component.prototype.SwitchBetweenLocalAndEng = function (lang) {
        this.LanguageFilterValue = lang;
        this.DrawColumns();
        this.SetDisplayMemberPath();
    };
    __decorate([
        core_1.Input(),
        __metadata("design:type", Object),
        __metadata("design:paramtypes", [Object])
    ], LogLovV2Component.prototype, "ForceFocus", null);
    __decorate([
        core_1.Input(),
        __metadata("design:type", forms_1.FormGroup)
    ], LogLovV2Component.prototype, "LogitudeForm", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Object)
    ], LogLovV2Component.prototype, "SelectedItemObject", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], LogLovV2Component.prototype, "ValueChanged", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], LogLovV2Component.prototype, "SelectedItemChanged", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Object),
        __metadata("design:paramtypes", [Object])
    ], LogLovV2Component.prototype, "SelectedValue", null);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogLovV2Component.prototype, "RunToggleMode", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogLovV2Component.prototype, "AutoCompleteSearchWindow", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogLovV2Component.prototype, "ForceShowAddLink", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], LogLovV2Component.prototype, "DisplayFieldsFromList", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogLovV2Component.prototype, "IsPickList", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogLovV2Component.prototype, "HideMaintenanceIcon", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogLovV2Component.prototype, "HideAddLink", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogLovV2Component.prototype, "NoObjectField", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogLovV2Component.prototype, "NoValidation", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], LogLovV2Component.prototype, "OnBlurEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], LogLovV2Component.prototype, "LostFocus", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogLovV2Component.prototype, "HideEdit", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogLovV2Component.prototype, "HideAdd", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", ApiQueryFilters_1.ApiQueryFilters)
    ], LogLovV2Component.prototype, "QueryFilterItems", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogLovV2Component.prototype, "ManipulateData", void 0);
    LogLovV2Component = __decorate([
        core_1.Component({
            selector: 'LogLov',
            moduleId: module.id,
            templateUrl: './LogLovV2Component.html',
            providers: [EntityListService_1.EntityListService, ServiceArgs_1.ServiceArgs, EntityResourceService_1.EntityResourceService],
            inputs: ['ObjectFieldName', 'ObjectTableName', 'DataContext', 'LookUpTableName', 'DisplayMemberPath', 'SelectedValuePath',
                'PlaceHolder', 'DependencyFilter1Value', 'DependencyFilter2Value', 'DependencyFilter3Value', "HideColumns", "HideLastColumn", "DependencyFilter1IsList",
                "DependencyFilter2IsList", "DependencyFilter3IsList", "DependencyFilter1IsListExact", "DependencyFilter2IsListExact", "DependencyFilter3IsListExact", "AutoFocus", "IsTenantZeroSearch", "ShowInActive", "FocusOnMe", "IsFreeText", "AlwaysEnabled", "IgnoreCustomFieldCheck", "IsDecendingSort"],
        }),
        __metadata("design:paramtypes", [EntityListService_1.EntityListService, EntityPMService_1.EntityPMService,
            EntityResourceService_1.EntityResourceService])
    ], LogLovV2Component);
    return LogLovV2Component;
}());
exports.LogLovV2Component = LogLovV2Component;
var EntityArgs = /** @class */ (function () {
    function EntityArgs() {
        this.ObjectTableName = null;
        this.ObjectTableId = null;
    }
    return EntityArgs;
}());
exports.EntityArgs = EntityArgs;
var AddEntityArgs = /** @class */ (function () {
    function AddEntityArgs() {
    }
    return AddEntityArgs;
}());
exports.AddEntityArgs = AddEntityArgs;
//# sourceMappingURL=LogLovV2Component.js.map