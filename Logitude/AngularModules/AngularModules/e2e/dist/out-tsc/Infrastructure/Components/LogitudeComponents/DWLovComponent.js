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
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var DWLogSearchWindowComponent_1 = require("./DWLogSearchWindowComponent");
var Observable_1 = require("rxjs/Observable");
require("rxjs/add/operator/debounceTime");
require("rxjs/add/operator/throttleTime");
require("rxjs/add/observable/fromEvent");
var InfraSettings_1 = require("../../Utilities/InfraSettings");
var EntityPMService_1 = require("../../Services/EntityPMService");
var ObjectsLocator_1 = require("../../Locators/ObjectsLocator");
var DWQueryBuilderService_1 = require("../../Services/ExtendedPMs/DWQueryBuilderService");
var DWLovComponent = /** @class */ (function () {
    function DWLovComponent(entityListService, entityPMService, _entityResourceService) {
        this.entityListService = entityListService;
        this.entityPMService = entityPMService;
        this._entityResourceService = _entityResourceService;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.ForceShowValidation = false;
        this.ShowHelp = false;
        this.ObjectFieldName = null;
        this.LOVAdditionalColumns = null;
        this.ObjectTableName = null;
        this.IsFreeText = false;
        this.AlwaysEnabled = false;
        this.isFirstTime = true;
        this.LayoutDirection = 'ltr';
        this.ItemsSourceCount = -1;
        this.IsReady = false;
        this.ValueChanged = new core_1.EventEmitter();
        this.SelectedItemChanged = new core_1.EventEmitter();
        this.searchTextNgModel = "";
        this.showPopup = false;
        this.PartnerTypes = [];
        this.tabkeyDown = false;
        this.FocusOnMe = false;
        this.searchTextChanged = false;
        this.FocusOnSelect = true;
        this.LovPartnerTypes = [];
        this.ZeroItemsSourceCount = -1;
        this.ShowErrorPopup = false;
        this.DropDownWidth = 300;
        this.DropDownHeight = 204;
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
        this._DWQueryBuilderService = new DWQueryBuilderService_1.DWQueryBuilderService();
        this.TenantPM = InfraSettings_1.InfraSettings.TenantPM;
        this.LayoutDirection = ObjectsLocator_1.ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection;
    }
    Object.defineProperty(DWLovComponent.prototype, "ForceFocus", {
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
    Object.defineProperty(DWLovComponent.prototype, "SelectedValue", {
        get: function () {
            return this.selectedValue;
        },
        set: function (newValue) {
            if (this.selectedValue != newValue) {
                this.selectedValue = newValue;
                this.SearchTextNgModel = newValue;
                //this.ValueChanged.emit(this.SelectedValue);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DWLovComponent.prototype, "SearchTextNgModel", {
        get: function () {
            return this.searchTextNgModel;
        },
        set: function (newValue) {
            var _this = this;
            if (this.searchTextNgModel != newValue) {
                this.searchTextNgModel = newValue;
                this.DisplayTextNgModel = "";
                if (this.searchTextNgModel) {
                    this.searchTextNgModel.split(";;").forEach(function (item) {
                        _this.DisplayTextNgModel += (item + "; ");
                    });
                    this.DisplayTextNgModel += "@@";
                    this.DisplayTextNgModel = this.DisplayTextNgModel.replace("; @@", "").replace("@@", "");
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    DWLovComponent.prototype.ngAfterViewInit = function () {
        this.RunComponent();
        //this.InitializeAfterViewInit();
    };
    DWLovComponent.prototype.RunComponent = function () {
        var input = document.getElementById(this.ElementId);
        if (input) {
            this.InitializeAfterViewInit();
        }
        else {
            this.RunComponentTimer();
        }
    };
    DWLovComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerTokenComponent) {
            clearTimeout(this.timerTokenComponent);
        }
        if (this.Retries < 3) {
            this.timerTokenComponent = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    DWLovComponent.prototype.InitializeAfterViewInit = function () {
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
                        /*this.OldSearchInput = this.SearchTextNgModel;
                        this.IsDropDownVisible = true;
                        this.IsOpen = true;
                        this._DWQueryBuilderService.GetDWDataForDimTabel(this.ObjectTableName, this.ObjectFieldName, this.SearchTextNgModel).subscribe(myResult => {
                            if (!myResult.HasError) {
                                this.ItemsSource = myResult.Result;
                            }

                        });
                        this.searchTextChanged = true;
                      */
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
    DWLovComponent.prototype.ngOnInit = function () {
        //this.entityListService.getAllFromCache(this.LookUpTableName, filters).then((res: any) => {
        //    res.subscribe(resp => {
        //    });
        //});
        this.InitializeControl();
    };
    DWLovComponent.prototype.ngOnDestroy = function () {
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
    DWLovComponent.prototype.CheckIfExists = function (IdCom) {
        var element = document.getElementById(IdCom);
        if (element != null && element != undefined) {
            return true;
        }
        return false;
    };
    DWLovComponent.prototype.SetControlIds = function (baseIdCombination) {
        this.DivLogLovId = 'LogLov_' + baseIdCombination;
        this.ElementId = baseIdCombination;
        this.DropdownId = 'LogLovDropDown-' + baseIdCombination;
        this.ErrorPopUpId = 'loglovererrorpop_' + baseIdCombination;
        this.MyDataListId = 'mydatalist_' + baseIdCombination;
        this.AllDataListId = 'alldatalist_' + baseIdCombination;
        this.SearchIconId = 'searchicon_' + baseIdCombination;
        this.ToolTipId = 'tooltip_' + baseIdCombination;
    };
    DWLovComponent.prototype.InitializeControl = function () {
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
        var objectFieldAvailable = true;
        this.ObjectTable = window.ObjectTables.filter(function (d) { return d.Name === _this.ObjectTableName; })[0];
        //(currentTable.AutoCompleteSearchWindow || this.AutoCompleteSearchWindow) && !this.RunToggleMode
        //*ngIf="ShowAddLink || ShowSearchButton"
        if (!this.ShowAddLink && !this.ShowSearchButton) {
            this.MyDropDownHeight = { 'height': '204px' };
        }
        if (!objectFieldAvailable && !this.NoObjectField) {
            console.warn(this.ObjectFieldName + " LOV has no object field!");
        }
    };
    DWLovComponent.prototype.OnWindowClick = function ($event) {
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
    DWLovComponent.prototype.OnLogLovFocus = function () {
    };
    DWLovComponent.prototype.OnLogLovKeyDown = function ($event) {
        var TABKEY = 9;
        var SHIFTKEY = 16;
        var DELETEKEY = 46;
        if ($event.keyCode == TABKEY) {
            if (this.IsOpen) {
                this.ToggleOpenDropDown();
            }
        }
        else if ($event.keyCode == DELETEKEY) {
            this.ToggleOpenDropDown();
        }
        else if ($event.keyCode != SHIFTKEY) {
            this.ToggleOpenDropDown();
        }
    };
    DWLovComponent.prototype.OnSelected = function (item) {
        this.isSelectedFromList = true;
        this.SelectedItem = item;
        this.SearchTextNgModel = item[this.ObjectFieldName];
        this.IsDropDownVisible = false;
        this.IsOpen = false;
        if (this.FocusOnSelect) {
            var elem = document.getElementById(this.ElementId);
            elem.focus();
        }
        this.FocusOnSelect = true;
        this.MouseInArea = false;
        this.ValueChanged.emit(item[this.ObjectFieldName]);
    };
    DWLovComponent.prototype.OnLogLovClicked = function () {
        this.ToggleOpenDropDown();
    };
    DWLovComponent.prototype.ToggleOpenDropDown = function () {
        this.IsDropDownVisible = !this.IsDropDownVisible;
        this.IsOpen = !this.IsOpen;
        if (this.IsOpen) {
            var input = document.getElementById(this.ElementId);
            input.focus();
            this.showPopup = false;
        }
    };
    DWLovComponent.prototype.NavigateListItems = function (isDown) {
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
    DWLovComponent.prototype.OnSearchIputKeyDown = function ($event) {
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
        }
        //if ((48 <= key && key <= 57) || (65 <= key && key <= 90) || key == 8 || key == 46 || key == 32) {
        //    this.OnDeleteValue(false);
        //}
    };
    DWLovComponent.prototype.OnLiMouseOver = function ($event) {
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
    DWLovComponent.prototype.OnLiMouseLeave = function ($event) {
        $event.target.classList.remove("highlighted");
    };
    DWLovComponent.prototype.HighlightSelectedValue = function (items) {
        var _this = this;
        this.SelectedItemKey = null;
        if (this.SearchTextNgModel != null && this.SearchTextNgModel != undefined && this.SearchTextNgModel != "") {
            var oldItems = items;
            var item = items.filter(function (d) { return d[_this.DisplayMemberPath] != null && d[_this.DisplayMemberPath].toLowerCase() === _this.SearchTextNgModel.toLowerCase(); })[0];
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
    DWLovComponent.prototype.OnSearchInputBlur = function () {
        if (!this.MouseInArea) {
            if (this.IsOpen) {
                this.ToggleOpenDropDown();
            }
            clearTimeout(this.focusTimerToken);
            this.ShowErrorPopup = false;
            this.ShowMaintenanceBtn = false;
            this.IsPartnerMenuVisible = false;
            this.OnBlurEvent.emit({ Id: this.ElementId });
            this.LostFocus.emit(this.SelectedValue);
        }
        if (!this.SelectedItem && this.SearchTextNgModel != null && this.SearchTextNgModel != undefined) {
            this.SearchTextNgModel = null;
            this.OldSearchInput = this.SearchTextNgModel;
            //this.ValidateField();
        }
    };
    DWLovComponent.prototype.OnMouseOver = function () {
        //this.ShowToolTip = true;
        this.MouseInArea = true;
        if (!this.SelectedItem) {
            this.DeleteButtonNgStyle = { 'visibility': 'hidden' };
        }
        else {
            this.DeleteButtonNgStyle = null;
        }
    };
    DWLovComponent.prototype.OnMouseOut = function () {
        //this.ShowToolTip = false;
        this.MouseInArea = false;
    };
    DWLovComponent.prototype.OnDropDownMouseOver = function () {
        this.MouseInArea = true;
        //this.CanClose = false;
        var input = document.getElementById(this.ElementId);
        input.focus();
    };
    DWLovComponent.prototype.OnDropDownMouseOut = function () {
        //this.CanClose = true;
        this.MouseInArea = false;
    };
    DWLovComponent.prototype.SearchButtonClicked = function () {
        var _this = this;
        //if (this.IsOpen)
        //    this.ToggleOpenDropDown();
        //this.showPopup = false;
        //this.ShowMaintenanceBtn = false;
        //this.IsPartnerMenuVisible = false;
        //this.ShowErrorPopup = false;
        ////var ObjectTable = window.ObjectTables.filter(x => x.Name === this.LookUpTableName)[0];
        ////var ObjectTableId = window.ObjectTables.filter(x => x.Name === this.LookUpTableName)[0].Id;
        var args = new DWLogSearchWindowComponent_1.CustomEntityArgs();
        args.ObjectTableName = this.ObjectTableName;
        args.DisplayFieldsFromList = this.ObjectFieldName;
        args.LOVAdditionalColumns = this.LOVAdditionalColumns;
        args.DataContext = this.DataContext;
        //args.IsAllDataVisible = this.IsAllDataVisible;
        //args.ShowInActive = this.ShowInActive;
        //args.PartnerTypes = this.PartnerTypes;
        //args.QueryFilterItems = this.QueryFilterItems;
        //args.HideAdd = this.HideAdd;
        //args.HideEdit = this.HideEdit;
        //args.IsAddDisabled = this.isAddDisabled;
        //args.IsEditDisabled = this.isEditDisabled;
        //args.DisplayFieldsFromList = this.DisplayFieldsFromList;
        //var tablename = TextCodeTranslator.TranslateTablePlural(this.GetObjectTableName(this.LookUpTableName));
        //if (tablename == "Cards")
        //    tablename = "Partners";
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 900;
        logitudeWindow.Height = 600;
        logitudeWindow.WindowArgs = args;
        logitudeWindow.Title = this.ObjectTableName + " Search";
        logitudeWindow.Show('./Infrastructure/Components/LogitudeComponents/DWLogSearchWindowComponent');
        logitudeWindow.WindowClosed.subscribe(function ($event) {
            if ($event != "Cancel") {
                _this.OnSearchWindowClosed($event);
            }
        });
    };
    DWLovComponent.prototype.OnSearchWindowClosed = function (args) {
        this.SearchTextNgModel = args;
        this.SelectedValue = args;
        this.SelectedItem = args;
        this.ValueChanged.emit(this.SelectedValue);
    };
    DWLovComponent.prototype.OnFocus = function () {
        var _this = this;
        this.showPopup = false;
        this.focusTimerToken = setTimeout(function () {
            //if (this.searchTextChanged || this.ForceShowValidation) {
            //    this.InputDivStyle = { 'border': '1px solid #ff0000' };
            //    this.ShowErrorPopup = true;
            //}
            if (!_this.IsOpen) {
                var input = document.getElementById(_this.ElementId);
                Selection(input);
                //input.select();
            }
        }, 1);
    };
    DWLovComponent.prototype.OnBlur = function () {
        //this.show = false;
        this.showPopup = false;
        this.ShowErrorPopup = false;
        //if (this.uiProperty.ValidValue) {
        //    this.InputDivStyle = null;
        //}
    };
    DWLovComponent.prototype.OnToggleClicked = function () {
        /*this._DWQueryBuilderService.GetDWDataForDimTabel(this.ObjectTableName, this.ObjectFieldName, this.SearchTextNgModel ? this.SearchTextNgModel : "").subscribe(myResult => {
            if (!myResult.HasError) {
                this.ItemsSource = myResult.Result;
                this.ToggleOpenDropDown();
            }

        });
        */
    };
    DWLovComponent.prototype.TogglePopup = function () {
        this.showPopup = !this.showPopup;
    };
    // Tool Button On Click
    DWLovComponent.prototype.OnMaintenanceClick = function () {
        var input = document.getElementById(this.ElementId);
        input.focus();
        this.TogglePopup();
        this.IsOpen = false;
        this.IsDropDownVisible = false;
    };
    DWLovComponent.prototype.ColWidth = function (index) {
        return this.Widths[index];
    };
    DWLovComponent.prototype.GetTheLongestWord = function (words) {
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
    //****
    DWLovComponent.prototype.GetObjectTableNameForDependency = function (dependency, parentObjectName) {
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
    DWLovComponent.prototype.OnMouseHover = function () {
        var _this = this;
        this.onhover = true;
        this.timerToken = setTimeout(function () {
            if (_this.onhover) {
                //this.show = true;            
                _this.showPopup = false;
                _this.ShowMaintenanceBtn = true;
            }
        }, 700);
    };
    DWLovComponent.prototype.OnMouseLeave = function () {
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
    DWLovComponent.prototype.OnSearchInputKeyUP = function ($event) {
        var CTRL = 17;
        if ($event.keyCode == CTRL) {
            this.IsCTRLDown = false;
        }
    };
    DWLovComponent.prototype.OnMouseOverAdd = function () {
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
    DWLovComponent.prototype.OnMouseOverEdit = function () {
        if (this.IsAddTypesVisible) {
            this.IsPartnerMenuVisible = false;
        }
    };
    DWLovComponent.prototype.OnMouseOverDelete = function () {
        if (this.IsAddTypesVisible) {
            this.IsPartnerMenuVisible = false;
        }
    };
    DWLovComponent.prototype.OnMouseOutAdd = function () {
        //if (this.IsAddTypesVisible) {
        //    this.IsPartnerMenuVisible = false;
        //}
    };
    __decorate([
        core_1.Input(),
        __metadata("design:type", Object),
        __metadata("design:paramtypes", [Object])
    ], DWLovComponent.prototype, "ForceFocus", null);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Object)
    ], DWLovComponent.prototype, "SelectedItemObject", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], DWLovComponent.prototype, "ValueChanged", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], DWLovComponent.prototype, "SelectedItemChanged", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Object),
        __metadata("design:paramtypes", [Object])
    ], DWLovComponent.prototype, "SelectedValue", null);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], DWLovComponent.prototype, "RunToggleMode", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], DWLovComponent.prototype, "AutoCompleteSearchWindow", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], DWLovComponent.prototype, "ForceShowAddLink", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], DWLovComponent.prototype, "DisplayFieldsFromList", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], DWLovComponent.prototype, "IsPickList", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], DWLovComponent.prototype, "HideMaintenanceIcon", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], DWLovComponent.prototype, "HideAddLink", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], DWLovComponent.prototype, "NoObjectField", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], DWLovComponent.prototype, "NoValidation", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], DWLovComponent.prototype, "OnBlurEvent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], DWLovComponent.prototype, "LostFocus", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], DWLovComponent.prototype, "HideEdit", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], DWLovComponent.prototype, "HideAdd", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", ApiQueryFilters_1.ApiQueryFilters)
    ], DWLovComponent.prototype, "QueryFilterItems", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], DWLovComponent.prototype, "ManipulateData", void 0);
    DWLovComponent = __decorate([
        core_1.Component({
            selector: 'DWLov',
            moduleId: module.id,
            templateUrl: './DWLovComponent.html',
            providers: [EntityListService_1.EntityListService, ServiceArgs_1.ServiceArgs, EntityResourceService_1.EntityResourceService],
            inputs: ['ObjectFieldName', 'ObjectTableName', 'DataContext', 'DisplayMemberPath', 'SelectedValuePath',
                "AutoFocus", "IsFreeText", "AlwaysEnabled", "Operation", "LOVAdditionalColumns"],
        }),
        __metadata("design:paramtypes", [EntityListService_1.EntityListService, EntityPMService_1.EntityPMService,
            EntityResourceService_1.EntityResourceService])
    ], DWLovComponent);
    return DWLovComponent;
}());
exports.DWLovComponent = DWLovComponent;
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
//# sourceMappingURL=DWLovComponent.js.map