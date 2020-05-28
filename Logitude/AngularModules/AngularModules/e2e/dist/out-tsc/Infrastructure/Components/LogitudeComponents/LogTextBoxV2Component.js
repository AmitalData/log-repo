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
var ControlsIdCounter_1 = require("../../Utilities/ControlsIdCounter");
var FieldValidator_1 = require("../../Validators/FieldValidator");
require("rxjs/add/operator/debounceTime");
require("rxjs/add/operator/throttleTime");
require("rxjs/add/observable/fromEvent");
var forms_1 = require("@angular/forms");
var ObjectsLocator_1 = require("../../Locators/ObjectsLocator");
var LogTextBoxV2Component = /** @class */ (function () {
    function LogTextBoxV2Component(ngzone, cd, appref) {
        this.ngzone = ngzone;
        this.cd = cd;
        this.appref = appref;
        this.ShowHelp = false;
        this.UseArialFont = false;
        this.ObjectFieldName = null;
        this.ObjectFieldHelp = null;
        this.ObjectTableName = null;
        this.HideColumns = false;
        this.HideLastColumn = false;
        this.KeyUp = new core_1.EventEmitter();
        this.ValueChanged = new core_1.EventEmitter();
        this.LostFocus = new core_1.EventEmitter();
        this.InputIdGenerated = new core_1.EventEmitter();
        this.Change = new core_1.EventEmitter();
        this.HasValue = new core_1.EventEmitter();
        this.NoObjectField = false;
        this.NoValidation = false;
        this.Placeholder = "";
        this.AlignTextToRight = false;
        this.DontAllowAutoSelect = false;
        this.AddCommasToNumbers = false;
        this.isFirstTime = true;
        this.IsFreeText = false;
        this.FocusOnMe = false;
        this.InputDivStyle = {};
        this.ShowErrorPopup = false;
        this.IsPasted = false;
        this.LayoutDirection = 'ltr';
        this.OriginalText = new core_1.EventEmitter();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.isCtrlKeyDown = false;
        this.Retries = 0;
        this.isShiftKeyDown = false;
        this.show = false;
        this.IdentityKey = Tools_1.AppTool.GetNewGuid();
        this.LayoutDirection = ObjectsLocator_1.ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection;
        //this.CurrentSession.isShiftClicked = false;
        //this.CurrentSession.isTabWithShiftClicked = false;
    }
    Object.defineProperty(LogTextBoxV2Component.prototype, "Text", {
        get: function () {
            return this.text;
        },
        set: function (newValue) {
            if (this.text != newValue) {
                this.text = newValue;
                if (!this.keydown) {
                    this.DataContextValueChanges(newValue);
                    this.ValidateField();
                    //this.DetectChanges();
                    if (this.uiProperty != null) {
                        this.uiProperty.UIPropertyChanged.emit("valuechanges");
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogTextBoxV2Component.prototype, "TextValue", {
        get: function () {
            return this.textValue;
        },
        set: function (newValue) {
            if (this.TextValue == newValue) {
                return;
            }
            if ((this.textValue == "" || this.textValue == null || this.textValue == undefined) && newValue) {
                this.HasValue.emit(true);
            }
            else if (this.textValue && (newValue == "" || newValue == null || newValue == undefined)) {
                this.HasValue.emit(false);
            }
            this.textValue = newValue;
            if (this.IsPasted) {
                this.TextValueChanges(newValue);
            }
        },
        enumerable: true,
        configurable: true
    });
    LogTextBoxV2Component.prototype.CheckIfExists = function (IdCom) {
        var element = document.getElementById(IdCom);
        if (element != null && element != undefined) {
            return true;
        }
        return false;
    };
    LogTextBoxV2Component.prototype.SetControlIds = function (baseIdCombination) {
        this.ErrorPopUpId = 'textboxererrorpop_' + baseIdCombination;
        this.InputId = baseIdCombination;
        this.InputDivId = "textboxdiv_" + baseIdCombination;
        this.InputIdGenerated.emit(this.InputId);
    };
    LogTextBoxV2Component.prototype.OnPaste = function ($event) {
        this.IsPasted = true;
        //console.log("paste paste: " + $event.clipboardData.getData('Text'));
        //this.TextValue = $event.clipboardData.getData('Text');
        //this.TextValueChanges(this.TextValue);
    };
    LogTextBoxV2Component.prototype.ngAfterViewInit = function () {
        this.RunComponent();
    };
    LogTextBoxV2Component.prototype.AfterViewInited = function () {
        if (!this.DebounceTime) {
            this.DebounceTime = 100;
        }
        //this.ngzone.runOutsideAngular(() => {
        //    var input = document.getElementById(this.InputId);
        //    this._debounceTimeSub =
        //        Observable.fromEvent(input, 'keydown')
        //            .debounceTime(this.DebounceTime)
        //            .subscribe(keyboardEvent => {
        //                var which = keyBoardWhich(keyboardEvent);
        //                var key = keyBoardKey(keyboardEvent);
        //                if (which == 53) {
        //                    console.log("");
        //                }
        //                var result = this.CheckKey(which, key);
        //                if (result != null || which == 13) {
        //                    var applyTextValue = true;
        //                    this.ngzone.run(() => {
        //                        if ((which == 9 || which == 13) && this.AllowPercentage) {
        //                            applyTextValue = false;
        //                        }
        //                        if (applyTextValue) {
        //                            this.TextValueChanges(this.TextValue);
        //                        }
        //                        if (which == 13) {
        //                            //if (this.FocusOnMe) {
        //                            //    var element = document.getElementById(this.InputId);
        //                            //    if (element) {
        //                            //        element.focus();
        //                            //    }
        //                            //    this.CurrentSession.SessionEvent.emit({ IsCell: true, Id: element.id, IsEnterCLicked: true });
        //                            //}
        //                        }
        //                    });
        //                    //this.TextValueChanges(this.TextValue);
        //                    if (this.cd) {
        //                        this.cd.detectChanges();
        //                    }
        //                }
        //                else {
        //                    return;
        //                }
        //            });
        //});
        if (this.IsDisabled) {
            this.SetDisabled();
        }
        else {
            this.SetEnabled();
        }
        if (this.FocusOnMe) {
            var element = document.getElementById(this.InputId);
            element.focus();
            this.CurrentSession.SessionEvent.emit({ IsCell: true, Id: element.id, IdentityKey: this.IdentityKey });
            this.timerToken = setTimeout(function () {
                SelectingElement(element);
            }, 1);
        }
    };
    LogTextBoxV2Component.prototype.RunComponent = function () {
        var input = document.getElementById(this.InputId);
        if (input) {
            this.AfterViewInited();
        }
        else {
            this.RunComponentTimer();
        }
    };
    LogTextBoxV2Component.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerTokenComponent) {
            clearTimeout(this.timerTokenComponent);
        }
        if (this.Retries < 3) {
            this.timerTokenComponent = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    LogTextBoxV2Component.prototype.ngOnInit = function () {
        var _this = this;
        var objectFieldAvailable = true;
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
                if (id == _this.InputId) {
                    //this.CurrentSession.CopiedCell = this.DataContext[this.ObjectFieldName];
                    _this.DataContext[_this.ObjectFieldName] = _this.CurrentSession.CopiedCell;
                    _this.CurrentSession.CopiedCell = null;
                }
            });
            if (this.CurrentSession.CopiedCell) {
                //this.DataContext[this.ObjectFieldName] = this.CurrentSession.CopiedCell;
                //this.CurrentSession.CopiedCell = null;
            }
        }
        var table = window.ObjectTables.filter(function (d) { return d.Name === _this.ObjectTableName; })[0];
        if (table) {
            this.ObjectField = window.ObjectFields.filter(function (d) { return d.ObjectTableId === table.Id && d.FieldName === _this.ObjectFieldName; })[0];
            if (!this.ObjectField) {
                objectFieldAvailable = false;
            }
            else if (this.ObjectField.HelpTextCodeId != null) {
                this.ObjectFieldHelp = TextCodeTranslator_1.TextCodeTranslator.Translate(this.ObjectField.HelpTextTextCodeCode);
                if (!Tools_1.AppTool.IsNullOrEmpty(this.ObjectFieldHelp)) {
                    if (this.ObjectFieldHelp.length > 1) {
                        if (!this.IsFreeText)
                            this.ShowHelp = true;
                    }
                }
            }
        }
        else {
            objectFieldAvailable = false;
        }
        this.uiProperty = this.DataContext.UIProperties.GetUIProperty(this.ObjectFieldName, this.ObjectTableName, this.DataContext);
        this.IsDisabled = !this.uiProperty.IsEnabled;
        if (this.IsDisabled) {
            this.SetDisabled();
        }
        else {
            this.SetEnabled();
        }
        this.uiProperty.UIPropertyChanged.subscribe(function (value) {
            if (value instanceof UIProperties_1.UIPropertyArgs) {
                var uiPropertyArgs = value;
                var uiProperty = uiPropertyArgs.uiProperty;
                if (uiProperty.FieldName == _this.ObjectFieldName && uiProperty.ObjectTableName == _this.ObjectTableName) {
                    if (uiPropertyArgs.property == "IsEnabled") {
                        var isEnabled = uiPropertyArgs.newValue;
                        _this.IsDisabled = !isEnabled;
                        _this.uiProperty.IsEnabled = isEnabled;
                        if (_this.IsDisabled) {
                            _this.SetDisabled();
                        }
                        else {
                            _this.SetEnabled();
                        }
                    }
                    else if (uiPropertyArgs.property == "IsRequired" || uiPropertyArgs.property == "IsValid") {
                        if (!_this.isFirstTime) {
                            _this.ValidateField(false);
                        }
                        else {
                            _this.isFirstTime = false;
                        }
                    }
                }
            }
            //this.DetectChanges();
        });
        //if there is an objectfield in the metadata:
        if (objectFieldAvailable) {
            if (!this.DigitsAfterPoint) {
                if (this.ObjectField.DigitsAfterPoint == 0) {
                    this.DigitsAfterPoint = 3;
                }
                else {
                    this.DigitsAfterPoint = this.ObjectField.DigitsAfterPoint;
                }
            }
            if (!this.InputType) {
                this.InputType = this.ObjectField.DataTypeCode.toLowerCase();
            }
            if (this.IsMultiline == undefined) {
                this.IsMultiline = this.ObjectField.MultiLine;
            }
            if (this.ObjectField.UniqueField && !this.IsNewEntityCall) {
                this.IsDisabled = true;
            }
            // this.ValidateField();
        }
        //using it without an objectfield.
        else {
            if (this.DigitsAfterPoint == undefined) {
                this.DigitsAfterPoint = 3;
            }
            if (!this.InputType) {
                this.InputType = 'ntext';
            }
        }
        if (!objectFieldAvailable && !this.NoObjectField) {
            console.warn(this.ObjectFieldName + " TEXTBOX has no object field!");
        }
        if (!this.IsFreeText) {
            if (this.ObjectField && this.ObjectField.IsCustom) {
                var customFieldClass = this.DataContext[this.ObjectFieldName];
                if (customFieldClass != null && customFieldClass != undefined) {
                    var customRes = customFieldClass.GetFieldDataTypeValue(this.ObjectField, customFieldClass.Value); //customFieldClass.Value;
                    this.TextValue = (customRes != null && customRes != undefined) ? customRes + '' : customRes;
                }
                else {
                    console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
                }
            }
            else {
                this.TextValue = this.DataContext[this.ObjectFieldName] != undefined && this.DataContext[this.ObjectFieldName] != null ? this.DataContext[this.ObjectFieldName] + '' : this.DataContext[this.ObjectFieldName];
            }
            this.GetValueFormatted(this.TextValue);
        }
    };
    LogTextBoxV2Component.prototype.ngOnDestroy = function () {
        console.log("LogTextBox:ngOnDestroy");
        this.cd = null;
        if (this._debounceTimeSub) {
            this._debounceTimeSub.unsubscribe();
        }
        if (this.CopyValueSubs) {
            this.CopyValueSubs.unsubscribe();
            this.CopyValueSubs = null;
        }
    };
    LogTextBoxV2Component.prototype.onFocus = function () {
        var _this = this;
        this.Detach = false;
        //this.DetectChanges();
        this.show = true;
        if (this.uiProperty.ValidValue) {
            this.timerToken = setTimeout(function () {
                _this.InputDivStyle = { 'border': '1px solid #3BB3E2' };
                _this.ShowErrorPopup = false;
            }, 300);
        }
        else {
            this.timerToken = setTimeout(function () {
                _this.InputDivStyle = { 'border': '1px solid #ff0000' };
                _this.ShowErrorPopup = true;
            }, 300);
        }
        if (!this.DontAllowAutoSelect) {
            var input = document.getElementById(this.InputId);
            SelectingElement(input);
        }
    };
    LogTextBoxV2Component.prototype.onBlur = function () {
        var _this = this;
        this.timerToken = setTimeout(function () {
            _this.ShowErrorPopup = false;
            if (_this.uiProperty.ValidValue) {
                _this.InputDivStyle = null;
            }
            _this.TextValueChanges(_this.TextValue);
        }, 300);
        this.timerToken = setTimeout(function () {
            _this.TextValueChanges(_this.TextValue);
        }, 30);
        this.Detach = true;
        //this.DetectChanges();
        this.show = false;
        //if(!this.FocusOnMe){
        //}
        // this.TextValue = this.DataContext[this.ObjectFieldName];
        this.keydown = false;
        this.GetValueFormatted(this.TextValue);
        this.LostFocus.emit(this.TextValue);
    };
    LogTextBoxV2Component.prototype.OnKeyUp = function (event) {
        var SHIFT = 16;
        var CTRL = 17;
        var key = event.keyCode;
        //if (key == SHIFT) {
        //    this.CurrentSession.isShiftClicked = false;
        //    this.CurrentSession.isTabWithShiftClicked = false;
        //    console.log("isTabWithShiftClicked = false;")
        //}
        if (key == SHIFT) {
            this.isShiftKeyDown = false;
        }
        if (key == CTRL) {
            this.isCtrlKeyDown = false;
        }
        this.KeyUp.emit(event);
    };
    LogTextBoxV2Component.prototype.OnKeyDown = function (event) {
        var SHIFT = 16;
        var CTRL = 17;
        var TAB = 9;
        this.keydown = true;
        var key = event.keyCode;
        var keyChar = event.key;
        if (key == 13) {
            if (this.FocusOnMe) {
                //var element = document.getElementById(this.InputId);
                //element.focus();
                //this.CurrentSession.SessionEvent.emit({ IsCell: true, Id: element.id, IsEnterCLicked: true, IdentityKey: this.IdentityKey });
            }
        }
        //if (key == SHIFT) {
        //    this.CurrentSession.isShiftClicked = true;
        //}
        if (key == TAB) {
            //if (this.CurrentSession.isShiftClicked == true) {
            //    this.CurrentSession.isTabWithShiftClicked = true;
            //    //console.log("isTabWithShiftClicked = true;");
            //}
            //else {
            //    this.CurrentSession.AllowShiftTab = true;
            //}
        }
        var result = this.CheckKey(key, keyChar);
        // console.log("watashi wa keydown des" + result);
        if (result) {
            this.TextValueChanges(this.TextValue);
        }
        if (key == CTRL) {
            this.isCtrlKeyDown = true;
        }
        if (result != null) {
            return key;
        }
        else {
            return false;
        }
    };
    LogTextBoxV2Component.prototype.CheckKey = function (key, keyChar) {
        var BACKSPACE = 8;
        var SHIFT = 16;
        var DASH = 189;
        var SUBTRACT = 109;
        var DELETE = 46;
        var HOME = 36;
        var END = 35;
        var PAGEUP = 33;
        var PAGEDOWN = 34;
        var LEFT = 37;
        var UP = 38;
        var RIGHT = 39;
        var DOWN = 40;
        var DECIMALPT = 110;
        var PERIOD = 190;
        var ADD = 107;
        var TAB = 9;
        var SPACEBAR = 32;
        var EQUALSIGN = 187;
        var GRAVEACCENT = 192;
        var BACKSLASH = 220;
        var CLOSEBRACKET = 221;
        var OPENBRACKET = 219;
        var SINGLEQOUTE = 222;
        var ENTER = 13;
        var FORWARDSLASH = 191;
        var COMMA = 188;
        var ESC = 27;
        var SEMICOLON = 186;
        var CTRL = 17;
        var EQUAL = 187;
        //(key >= 48 && key <= 57) ARE THE NUMBERS ON TOP || (key >= 96 && key <= 105) ARE THE NUMBERS ON NUMPAD
        if (key == TAB) {
            this.keydown = false;
        }
        if (key == SHIFT) {
            this.keydown = false;
            this.isShiftKeyDown = true; // this is used to check some keys 
        }
        if (key == CTRL) {
            this.isCtrlKeyDown = true;
        }
        if (key == 67 || key == 65 || key == 86 || key == 88) { // ctrl+a,v,a,x
            if (this.isCtrlKeyDown) {
                return key;
            }
        }
        if (this.InputType) {
            var numChars = ['1', '2', '3', '4', '5', '6', '7', '8', '9', '0'];
            switch (this.InputType.toLowerCase()) {
                case 'double':
                case 'unsDecimal':
                    {
                        if ((key >= 48 && key <= 57) || (key >= 96 && key <= 105) || key == BACKSPACE || key == PERIOD || key == DECIMALPT || key == TAB || key == DELETE
                            || key == END || key == HOME || key == SHIFT || key == PAGEUP || key == PAGEDOWN || key == LEFT || key == UP || key == RIGHT || key == DOWN || key == ADD || key == EQUAL) {
                            if (key == 53) {
                                if (keyChar == "%") {
                                    if (this.AllowPercentage && !Tools_1.AppTool.IsNullOrEmpty(this.TextValue)) {
                                        return key;
                                    }
                                    else {
                                        return null;
                                    }
                                }
                            }
                            if (key >= 48 && key <= 57) {
                                if (numChars.indexOf(keyChar) == -1) {
                                    return null;
                                }
                            }
                            if (key == ADD) {
                                if (this.IsAccumulative) {
                                    return key;
                                }
                                else {
                                    return null;
                                }
                            }
                            if (key == EQUAL) {
                                if (this.IsAccumulative && keyChar == "+") {
                                    return key;
                                }
                                else {
                                    return null;
                                }
                            }
                            return key;
                        }
                        return null;
                    }
                case 'unsinteger':
                    {
                        if ((key >= 48 && key <= 57) || (key >= 96 && key <= 105) || key == BACKSPACE || key == TAB || key == DELETE || key == END
                            || key == HOME || key == SHIFT || key == PAGEUP || key == PAGEDOWN || key == LEFT || key == UP || key == RIGHT || key == DOWN || key == ADD || key == EQUAL) {
                            if (key == 53) {
                                if (keyChar == "%") {
                                    if (this.AllowPercentage && !Tools_1.AppTool.IsNullOrEmpty(this.TextValue) && this.TextValue.indexOf("%") < 0) {
                                        return key;
                                    }
                                    else {
                                        return null;
                                    }
                                }
                            }
                            if (key >= 48 && key <= 57) {
                                if (numChars.indexOf(keyChar) == -1) {
                                    return null;
                                }
                            }
                            if (key == ADD) {
                                if (this.IsAccumulative) {
                                    return key;
                                }
                                else {
                                    return null;
                                }
                            }
                            if (key == EQUAL) {
                                if (this.IsAccumulative && keyChar == "+") {
                                    return key;
                                }
                                else {
                                    return null;
                                }
                            }
                            return key;
                        }
                        return null;
                    }
                case 'decimal':
                case 'sigdouble':
                    {
                        if ((key >= 48 && key <= 57) || (key >= 96 && key <= 105) || key == BACKSPACE || key == PERIOD || key == DECIMALPT || key == TAB || key == DELETE
                            || key == END || key == HOME || key == SHIFT || key == PAGEUP || key == PAGEDOWN || key == LEFT || key == UP || key == RIGHT || key == DOWN || key == SUBTRACT || key == DASH || key == ADD || key == EQUAL || key == 173) {
                            if (key == SUBTRACT || key == DASH || key == 173) {
                                if (!Tools_1.AppTool.IsNullOrEmpty(this.TextValue) && this.TextValue.toString().indexOf('-') > -1) {
                                    return null;
                                }
                                else {
                                    var input = document.getElementById(this.InputId);
                                    if (input != null) {
                                        if (selectionStart(input) == 0 && !this.isShiftKeyDown) {
                                            return key;
                                        }
                                        else {
                                            return null;
                                        }
                                    }
                                }
                            }
                            if (key == ADD) {
                                if (this.IsAccumulative) {
                                    return key;
                                }
                                else {
                                    return null;
                                }
                            }
                            if (key == EQUAL) {
                                if (this.IsAccumulative && keyChar == "+") {
                                    return key;
                                }
                                else {
                                    return null;
                                }
                            }
                            if (key == 53) {
                                if (keyChar == "%") {
                                    if (this.AllowPercentage && !Tools_1.AppTool.IsNullOrEmpty(this.TextValue)) {
                                        return key;
                                    }
                                    else {
                                        return null;
                                    }
                                }
                            }
                            if (key >= 48 && key <= 57) {
                                if (numChars.indexOf(keyChar) == -1) {
                                    return null;
                                }
                            }
                            return key;
                        }
                        return null;
                    }
                case 'integer':
                    {
                        if ((key >= 48 && key <= 57) || (key >= 96 && key <= 105) || key == BACKSPACE || key == TAB || key == DELETE || key == END
                            || key == HOME || key == SHIFT || key == PAGEUP || key == PAGEDOWN || key == LEFT || key == UP || key == RIGHT || key == DOWN || key == SUBTRACT || key == DASH || key == ADD || key == EQUAL || key == 173) {
                            if (key == SUBTRACT || key == DASH || key == 173) {
                                if (!Tools_1.AppTool.IsNullOrEmpty(this.TextValue) && this.TextValue.toString().indexOf('-') > -1) {
                                    return null;
                                }
                                else {
                                    var input = document.getElementById(this.InputId);
                                    if (input != null) {
                                        if (selectionStart(input) == 0 && !this.isShiftKeyDown) {
                                            return key;
                                        }
                                        else {
                                            return null;
                                        }
                                    }
                                }
                            }
                            if (key == 53) {
                                if (keyChar == "%") {
                                    if (this.AllowPercentage && !Tools_1.AppTool.IsNullOrEmpty(this.TextValue) && this.TextValue.indexOf("%") < 0) {
                                        return key;
                                    }
                                    else {
                                        return null;
                                    }
                                }
                            }
                            if (key >= 48 && key <= 57) {
                                if (numChars.indexOf(keyChar) == -1) {
                                    return null;
                                }
                            }
                            if (key == ADD) {
                                if (this.IsAccumulative) {
                                    return key;
                                }
                                else {
                                    return null;
                                }
                            }
                            if (key == EQUAL) {
                                if (this.IsAccumulative && keyChar == "+") {
                                    return key;
                                }
                                else {
                                    return null;
                                }
                            }
                            return key;
                        }
                        return null;
                    }
                case 'text': {
                    if (!this.AllowLocalLanguage) {
                        //key == 173 || key == 61 || key == 59 : - AND = AND ; IN FIREFOX.
                        if (keyChar == "@") {
                            return key;
                        }
                        var engChars = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
                            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];
                        if (key == SPACEBAR || key == BACKSPACE || key == TAB || key == DELETE || key == END || key == HOME || key == SHIFT || key == PAGEUP
                            || key == PAGEDOWN || key == PERIOD || key == DECIMALPT || key == EQUALSIGN || key == DASH || key == GRAVEACCENT || key == BACKSLASH
                            || key == CLOSEBRACKET || key == OPENBRACKET || key == FORWARDSLASH || key == COMMA || key == ESC || key == ENTER || key == SEMICOLON || key == SINGLEQOUTE
                            || key == LEFT || key == UP || key == RIGHT || key == DOWN || key == ADD || key == 173 || key == 61 || key == 59)
                            return key;
                        if (48 <= key && key <= 57)
                            return key;
                        if (65 <= key && key <= 90) {
                            if (engChars.indexOf(keyChar) > -1) {
                                return key;
                            }
                            return null;
                        }
                        if (96 <= key && key <= 122)
                            return key;
                        return null;
                    }
                    return key;
                }
                case 'ntext': {
                    return key;
                }
                case 'salepriceandmarkup': {
                    var isOk = false;
                    var input = document.getElementById(this.InputId);
                    if (key == BACKSPACE || key == DELETE || key == END || key == HOME || key == SHIFT || key == PAGEUP || key == PAGEDOWN || key == LEFT || key == UP || key == RIGHT || key == DOWN || key == TAB) {
                        isOk = true;
                    }
                    else {
                        if (keyChar == "+") {
                            if (selectionStart(input) == 0) {
                                if (!Tools_1.AppTool.IsNullOrEmpty(this.TextValue)) {
                                    if (this.TextValue.indexOf("+") == -1) {
                                        isOk = true;
                                    }
                                }
                                else {
                                    isOk = true;
                                }
                            }
                        }
                        if (keyChar == "-") {
                            if (selectionStart(input) == 0) {
                                if (!Tools_1.AppTool.IsNullOrEmpty(this.TextValue)) {
                                    if (this.TextValue.indexOf("-") == -1) {
                                        isOk = true;
                                    }
                                }
                                else {
                                    isOk = true;
                                }
                            }
                        }
                        else {
                            if ((key >= 48 && key <= 57) || (key >= 96 && key <= 105) || key == PERIOD || key == DECIMALPT || key == ADD || key == SUBTRACT || key == DASH || key == 173 || (this.isShiftKeyDown && (key == 187 || key == 53))) {
                                if (numChars.indexOf(keyChar) > -1) {
                                    if (!Tools_1.AppTool.IsNullOrEmpty(this.TextValue)) {
                                        if (this.TextValue.indexOf("%") > -1) {
                                            if (selectionStart(input) != this.TextValue.length) {
                                                isOk = true;
                                            }
                                        }
                                        else if ((this.TextValue.indexOf("+") > -1) || (this.TextValue.indexOf("-") > -1)) {
                                            if (selectionStart(input) != 0) {
                                                isOk = true;
                                            }
                                        }
                                        else {
                                            isOk = true;
                                        }
                                    }
                                    else {
                                        isOk = true;
                                    }
                                }
                                //////////////////////////////////////
                                if (keyChar == ".") {
                                    if (!Tools_1.AppTool.IsNullOrEmpty(this.TextValue) && this.TextValue.indexOf(".") == -1) {
                                        if (this.TextValue.indexOf("%") > -1) {
                                            if (selectionStart(input) != this.TextValue.length) {
                                                isOk = true;
                                            }
                                        }
                                        else {
                                            isOk = true;
                                        }
                                    }
                                    else {
                                        isOk = true;
                                    }
                                }
                                //////////////////////////////////////
                                if (keyChar == "%") {
                                    if (!Tools_1.AppTool.IsNullOrEmpty(this.TextValue) && selectionStart(input) != 0 && selectionStart(input) == this.TextValue.length && this.TextValue.indexOf("%") == -1) {
                                        if ((this.TextValue.indexOf("+") > -1) || (this.TextValue.indexOf("-") > -1)) {
                                            isOk = true;
                                        }
                                    }
                                }
                                //////////////////////////////////////
                                if (keyChar == "-") {
                                    if (selectionStart(input) == 0) {
                                        if (!Tools_1.AppTool.IsNullOrEmpty(this.TextValue)) {
                                            if (this.TextValue.indexOf("-") == -1 && this.TextValue.indexOf("+") == -1) {
                                                isOk = true;
                                            }
                                        }
                                    }
                                    else {
                                        isOk = true;
                                    }
                                }
                            }
                        }
                    }
                    if (isOk) {
                        return key;
                    }
                    else {
                        return null;
                    }
                }
                case 'integertext':
                    {
                        if ((key >= 48 && key <= 57) || (key >= 96 && key <= 105) || key == BACKSPACE || key == TAB || key == DELETE || key == END
                            || key == HOME || key == SHIFT || key == PAGEUP || key == PAGEDOWN || key == LEFT || key == UP || key == RIGHT || key == DOWN || key == SUBTRACT || key == DASH) {
                            if (key == SUBTRACT || key == DASH) {
                                if (!Tools_1.AppTool.IsNullOrEmpty(this.TextValue) && this.TextValue.toString().indexOf('-') > -1) {
                                    return null;
                                }
                                else {
                                    var input = document.getElementById(this.InputId);
                                    if (input != null) {
                                        if (selectionStart(input) == 0) {
                                            return key;
                                        }
                                        else {
                                            return null;
                                        }
                                    }
                                }
                            }
                            if (key >= 48 && key <= 57) {
                                if (numChars.indexOf(keyChar) == -1) {
                                    return null;
                                }
                            }
                            return key;
                        }
                        return null;
                    }
                default:
                    return key;
            }
        }
        else {
            return key;
        }
    };
    LogTextBoxV2Component.prototype.onchange = function (event) {
        this.Change.emit(this.TextValue);
    };
    LogTextBoxV2Component.prototype.GetValueFormatted = function (valueFromField) {
        if (this.TextValue) {
            this.OriginalText.emit(this.TextValue);
            if (this.InputType) {
                switch (this.InputType.toLowerCase()) {
                    case 'double':
                    case 'sigdouble':
                    case 'decimal':
                    case 'unsDecimal':
                        {
                            var isSignOk = true;
                            if (this.InputType == 'unsDecimal' || this.InputType == 'double') {
                                var text = this.TextValue + "";
                                if (text.indexOf('-') > -1) {
                                    isSignOk = false;
                                }
                            }
                            var val;
                            if (this.IsAccumulative && (this.TextValue + "").indexOf('+') > -1) {
                                var accString = this.TextValue.split('+');
                                var accumulativeAmount = 0;
                                accString.forEach(function (accitem) {
                                    var v = Number(accitem);
                                    accumulativeAmount = accumulativeAmount + v;
                                });
                                val = accumulativeAmount;
                            }
                            else if (this.AllowPercentage && (this.TextValue + "").indexOf('%') > -1) {
                                var txt = this.TextValue.replace('%', '');
                                val = Number(txt) / 100;
                                //val = val / 100;
                            }
                            else {
                                val = Number(this.TextValue);
                                if (this.AddCommasToNumbers) {
                                    if ((this.TextValue + "").indexOf(',') > -1) {
                                        var txtval = this.TextValue.replace(',', "");
                                        val = Number(txtval);
                                    }
                                }
                            }
                            if (isNaN(val) || !isSignOk) {
                                this.SetValidity(false, TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.InvalidInput")); //"Invalid Input");
                            }
                            else {
                                if (!this.DisableZeroPadding) {
                                    this.TextValue = val.toFixed(this.DigitsAfterPoint);
                                }
                            }
                            break;
                        }
                    case 'unsinteger':
                    case 'integer':
                        {
                            var isSignOk = true;
                            if (this.InputType == 'unsinteger') {
                                var text = this.TextValue + "";
                                if (text.indexOf('-') > -1) {
                                    isSignOk = false;
                                }
                            }
                            var val;
                            val = Number(this.TextValue);
                            if (this.AddCommasToNumbers) {
                                if ((this.TextValue + "").indexOf(',') > -1) {
                                    var txtval = this.TextValue.replace(',', "");
                                    val = Number(txtval);
                                }
                            }
                            if (isNaN(val) || !isSignOk) {
                                this.SetValidity(false, TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.InvalidInput")); //"Invalid Input");
                            }
                            else {
                                this.TextValue = val.toFixed(0);
                            }
                            break;
                        }
                    case "integertext": {
                        if (isNaN(Number(this.TextValue))) {
                            this.SetValidity(false, TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.InvalidInput"));
                        }
                        break;
                    }
                    default:
                        {
                            break;
                        }
                }
                if (this.AddCommasToNumbers) {
                    if (this.DataContext[this.ObjectFieldName] + "" != this.TextValue) {
                        this.TextValueChanges(this.TextValue);
                    }
                    this.TextValue = numberWithCommas(this.TextValue);
                }
            }
        }
    };
    LogTextBoxV2Component.prototype.TextValueChanges = function (res) {
        if (this.DataContext[this.ObjectFieldName] + "" != this.TextValue) {
            if (this.TextValue) {
                switch (this.InputType) {
                    case 'double':
                    case 'sigdouble':
                    case 'decimal':
                    case 'unsDecimal':
                    case 'unsinteger':
                    case 'integer':
                        {
                            var value;
                            if (this.IsAccumulative && (this.TextValue + "").indexOf('+') > -1) {
                                var accString = this.TextValue.split('+');
                                var accumulativeAmount = 0;
                                accString.forEach(function (accitem) {
                                    var v = Number(accitem);
                                    accumulativeAmount = accumulativeAmount + v;
                                });
                                value = accumulativeAmount;
                            }
                            else if (this.AllowPercentage && (this.TextValue + "").indexOf('%') > -1) {
                                var txt = this.TextValue.replace('%', '');
                                value = Number(txt) / 100;
                            }
                            else {
                                value = Number(this.TextValue);
                            }
                            if (!isNaN(value)) {
                                var isok = true;
                                if (this.InputType == 'unsDecimal' || this.InputType == 'unsinteger' || this.InputType == 'double') {
                                    if (this.TextValue.indexOf('-') > -1) {
                                        isok = false;
                                    }
                                }
                                if (isok) {
                                    if (this.ObjectField && this.ObjectField.IsCustom) {
                                        var customFieldClass = this.DataContext[this.ObjectFieldName];
                                        if (customFieldClass != null && customFieldClass != undefined) {
                                            customFieldClass.Value = customFieldClass.SetFieldDataType(this.ObjectField, this.TextValue); // this.TextValue;
                                        }
                                        else {
                                            console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
                                        }
                                        this.DataContext[this.ObjectFieldName] = customFieldClass;
                                    }
                                    else {
                                        this.DataContext[this.ObjectFieldName] = value;
                                    }
                                }
                            }
                            break;
                        }
                    case "integertext": {
                        if (!isNaN(Number(this.TextValue))) {
                            if (this.ObjectField && this.ObjectField.IsCustom) {
                                var customFieldClass = this.DataContext[this.ObjectFieldName];
                                if (customFieldClass != null && customFieldClass != undefined) {
                                    customFieldClass.Value = this.TextValue;
                                }
                                else {
                                    console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
                                }
                                this.DataContext[this.ObjectFieldName] = customFieldClass;
                            }
                            else {
                                this.DataContext[this.ObjectFieldName] = (this.TextValue);
                            }
                        }
                        break;
                    }
                    default:
                        {
                            if (this.ObjectField && this.ObjectField.IsCustom) {
                                var customFieldClass = this.DataContext[this.ObjectFieldName];
                                if (customFieldClass != null && customFieldClass != undefined) {
                                    customFieldClass.Value = customFieldClass.SetFieldDataType(this.ObjectField, this.TextValue); // this.TextValue;
                                }
                                else {
                                    console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
                                }
                                this.DataContext[this.ObjectFieldName] = customFieldClass;
                            }
                            else {
                                this.DataContext[this.ObjectFieldName] = (this.TextValue);
                            }
                            break;
                        }
                }
                // validate min max value
                if (this.Max) {
                    if (Number(this.TextValue) > this.Max)
                        this.TextValue = this.Max + "";
                }
                if (this.Min) {
                    if (Number(this.TextValue) < this.Min)
                        this.TextValue = this.Min + "";
                }
            }
            else {
                if (this.TextValue == "") {
                    this.TextValue = null;
                }
                if (this.ObjectField && this.ObjectField.IsCustom) {
                    var customFieldClass = this.DataContext[this.ObjectFieldName];
                    if (customFieldClass != null && customFieldClass != undefined) {
                        customFieldClass.Value = customFieldClass.SetFieldDataType(this.ObjectField, this.TextValue); // this.TextValue;
                    }
                    else {
                        console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
                    }
                    this.DataContext[this.ObjectFieldName] = customFieldClass;
                }
                else {
                    this.DataContext[this.ObjectFieldName] = (this.TextValue);
                }
            }
            this.ValidateField();
            this.ValueChanged.emit(this.TextValue);
        }
    };
    LogTextBoxV2Component.prototype.DataContextValueChanges = function (res) {
        var _this = this;
        var dataContextValue = this.DataContext[this.ObjectFieldName];
        if (!this.ObjectField) {
            var table = window.ObjectTables.filter(function (d) { return d.Name === _this.ObjectTableName; })[0];
            if (table) {
                this.ObjectField = window.ObjectFields.filter(function (d) { return d.ObjectTableId === table.Id && d.FieldName === _this.ObjectFieldName; })[0];
            }
        }
        if (this.ObjectField && this.ObjectField.IsCustom) {
            var customFieldClass = this.DataContext[this.ObjectFieldName];
            if (customFieldClass != null && customFieldClass != undefined) {
                dataContextValue = customFieldClass.Value;
            }
            else {
                console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
            }
        }
        if (dataContextValue != this.TextValue || this.IsFreeText) {
            if (this.IsFreeText)
                this.TextValue = res;
            else {
                if (this.ObjectField && this.ObjectField.IsCustom) {
                    var customFieldClass = this.DataContext[this.ObjectFieldName];
                    if (customFieldClass != null && customFieldClass != undefined) {
                        var customRes = customFieldClass.GetFieldDataTypeValue(this.ObjectField, customFieldClass.Value); //customFieldClass.Value;
                        this.TextValue = (customRes != null && customRes != undefined) ? customRes + '' : customRes;
                    }
                    else {
                        console.warn("Custom Fields are not implemented in: " + this.ObjectTableName);
                    }
                }
                else {
                    this.TextValue = this.DataContext[this.ObjectFieldName] != undefined && this.DataContext[this.ObjectFieldName] != null ? this.DataContext[this.ObjectFieldName] + '' : this.DataContext[this.ObjectFieldName];
                }
            }
            this.GetValueFormatted(this.TextValue);
        }
    };
    LogTextBoxV2Component.prototype.SetValidity = function (validValue, errorMessage) {
        if (this.uiProperty != null) {
            this.uiProperty.ValidValue = validValue;
            this.uiProperty.ValidationError = errorMessage;
        }
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
    LogTextBoxV2Component.prototype.SetDisabled = function () {
        var inputDiv = document.getElementById(this.InputDivId); //("DatePickerInputDiv");
        if (inputDiv) {
            inputDiv.classList.add("InputDivDisabled");
        }
    };
    LogTextBoxV2Component.prototype.SetEnabled = function () {
        var inputDiv = document.getElementById(this.InputDivId); //("DatePickerInputDiv");
        if (inputDiv) {
            inputDiv.classList.remove("InputDivDisabled");
        }
    };
    LogTextBoxV2Component.prototype.ValidateField = function (emitPropertyChanged) {
        var _this = this;
        if (emitPropertyChanged === void 0) { emitPropertyChanged = true; }
        if (!this.NoValidation && this.uiProperty != null) {
            var errors = null;
            var table = window.ObjectTables.filter(function (d) { return d.Name === _this.uiProperty.ObjectTableName; })[0];
            if (table) {
                var field = window.ObjectFields.filter(function (d) { return d.ObjectTableId === table.Id && d.FieldName === _this.uiProperty.FieldName; })[0];
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
                //else if (this.uiProperty.IsValidManually === false) {
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
    //DetectChanges() {
    //    return;
    //    this.cd.reattach();
    //    this.cd.detectChanges();
    //    if (this.Detach) {
    //        setTimeout(() => this.cd.detach(), 1000);
    //    }
    //}
    LogTextBoxV2Component.prototype.OnMouseOver = function () {
        if (this.IsDisabled) {
            this.InputDivStyle = { 'border': '1px solid #AAAAAA' };
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], LogTextBoxV2Component.prototype, "KeyUp", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], LogTextBoxV2Component.prototype, "ValueChanged", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], LogTextBoxV2Component.prototype, "LostFocus", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], LogTextBoxV2Component.prototype, "InputIdGenerated", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], LogTextBoxV2Component.prototype, "Change", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], LogTextBoxV2Component.prototype, "HasValue", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogTextBoxV2Component.prototype, "DontUseTimer", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogTextBoxV2Component.prototype, "DisableZeroPadding", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogTextBoxV2Component.prototype, "AllowSpellCheck", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogTextBoxV2Component.prototype, "AllowLocalLanguage", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogTextBoxV2Component.prototype, "IsNewEntityCall", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogTextBoxV2Component.prototype, "NoObjectField", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogTextBoxV2Component.prototype, "NoValidation", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], LogTextBoxV2Component.prototype, "Placeholder", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogTextBoxV2Component.prototype, "AlignTextToRight", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogTextBoxV2Component.prototype, "AddCommasToNumbers", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Number)
    ], LogTextBoxV2Component.prototype, "Max", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Number)
    ], LogTextBoxV2Component.prototype, "Min", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Object),
        __metadata("design:paramtypes", [Object])
    ], LogTextBoxV2Component.prototype, "Text", null);
    __decorate([
        core_1.Input(),
        __metadata("design:type", forms_1.FormGroup)
    ], LogTextBoxV2Component.prototype, "LogitudeForm", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], LogTextBoxV2Component.prototype, "OriginalText", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Number)
    ], LogTextBoxV2Component.prototype, "DebounceTime", void 0);
    LogTextBoxV2Component = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'LogTextBoxV2',
            templateUrl: "./LogTextBoxV2Component.html",
            //directives: [CORE_DIRECTIVES, FORM_DIRECTIVES, HelpIcon, FixedPositionDirective],
            inputs: ['ObjectFieldName', 'ObjectTableName', 'DataContext', "IsMultiline", "InputType", "HideColumns", "HideLastColumn", "DigitsAfterPoint", "FocusOnMe", "IsFreeText", "IsAccumulative", "AllowPercentage", "UseArialFont", "DontAllowAutoSelect"],
        }),
        __metadata("design:paramtypes", [core_1.NgZone, core_1.ChangeDetectorRef,
            core_1.ApplicationRef])
    ], LogTextBoxV2Component);
    return LogTextBoxV2Component;
}());
exports.LogTextBoxV2Component = LogTextBoxV2Component;
//# sourceMappingURL=LogTextBoxV2Component.js.map