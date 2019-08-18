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
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
Object.defineProperty(exports, "__esModule", { value: true });
var LogitudeWindow_1 = require("./../../../Controls/Windows/LogitudeWindow");
var core_1 = require("@angular/core");
var UIProperties_1 = require("./UIProperties");
var SessionLocator_1 = require("../../Utilities/SessionLocator");
var Tools_1 = require("../../Tools");
var TextCodeTranslator_1 = require("../../Utilities/TextCodeTranslator");
var ControlsIdCounter_1 = require("../../Utilities/ControlsIdCounter");
var FieldValidator_1 = require("../../Validators/FieldValidator");
var Observable_1 = require("rxjs/Observable");
require("rxjs/add/operator/debounceTime");
require("rxjs/add/operator/throttleTime");
require("rxjs/add/observable/fromEvent");
var forms_1 = require("@angular/forms");
var ObjectsLocator_1 = require("../../Locators/ObjectsLocator");
var timer_1 = require("rxjs/observable/timer");
//import { timer } from 'rxjs';
var operators_1 = require("rxjs/operators");
function BeforeOnDestroy(target, key, descriptor) {
    return {
        value: function () {
            var args = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                args[_i] = arguments[_i];
            }
            return __awaiter(this, void 0, void 0, function () {
                return __generator(this, function (_a) {
                    switch (_a.label) {
                        case 0: return [4 /*yield*/, target.ngxBeforeOnDestroy()];
                        case 1:
                            _a.sent();
                            return [2 /*return*/, descriptor.value.apply(target, args)];
                    }
                });
            });
        }
    };
}
exports.BeforeOnDestroy = BeforeOnDestroy;
var LogTextBoxComponent = /** @class */ (function () {
    function LogTextBoxComponent(ngzone, cd, appref) {
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
        this.IsRatioBox = false;
        this.textboxHeight = '100%';
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
        this.AddCommasToNumbers = true;
        this.firstDigit = ",";
        this.secondDigit = ".";
        this.isFirstTime = true;
        this.IsFreeText = false;
        this.FocusOnMe = false;
        this.InputDivStyle = {};
        this.ShowErrorPopup = false;
        this.IsPasted = false;
        this.LayoutDirection = 'ltr';
        this.showLocal = false;
        this.OriginalText = new core_1.EventEmitter();
        this.isRTL = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.isCtrlKeyDown = false;
        this.Retries = 0;
        this.isShiftKeyDown = false;
        //DetectChanges() {
        //    return;
        //    this.cd.reattach();
        //    this.cd.detectChanges();
        //    if (this.Detach) {
        //        setTimeout(() => this.cd.detach(), 1000);
        //    }
        //}
        this.isMouseOver = false;
        this.isExpanded = false;
        this.show = false;
        this.IdentityKey = Tools_1.AppTool.GetNewGuid();
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.LayoutDirection = ObjectsLocator_1.ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection;
        this.showLocal = !SessionLocator_1.SessionLocator.LoggedUserPM.DontShowLocal;
        this.setDigits();
        //this.CurrentSession.isShiftClicked = false;
        //this.CurrentSession.isTabWithShiftClicked = false;
    }
    Object.defineProperty(LogTextBoxComponent.prototype, "Text", {
        get: function () {
            return this.text;
        },
        set: function (newValue) {
            if (this.text != newValue) {
                this.text = newValue;
                if (!this.keydown) {
                    if (!this.uiProperty) {
                        this.uiProperty = this.DataContext.UIProperties.GetUIProperty(this.ObjectFieldName, this.ObjectTableName, this.DataContext);
                    }
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
    Object.defineProperty(LogTextBoxComponent.prototype, "TextValue", {
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
                switch (this.InputType && this.InputType.toLowerCase()) {
                    case 'text':
                    case 'ntext':
                        {
                            break;
                        }
                    default: {
                        if (newValue && newValue.indexOf(',') > -1) {
                            newValue = newValue.replace(',', '');
                        }
                    }
                }
                this.textValue = newValue;
                this.TextValueChanges(newValue);
            }
        },
        enumerable: true,
        configurable: true
    });
    LogTextBoxComponent.prototype.CheckIfExists = function (IdCom) {
        var element = document.getElementById(IdCom);
        if (element != null && element != undefined) {
            return true;
        }
        return false;
    };
    LogTextBoxComponent.prototype.SetControlIds = function (baseIdCombination) {
        this.ErrorPopUpId = 'textboxererrorpop_' + baseIdCombination;
        this.InputId = baseIdCombination;
        this.InputDivId = "textboxdiv_" + baseIdCombination;
        this.InputIdGenerated.emit(this.InputId);
    };
    LogTextBoxComponent.prototype.OnPaste = function ($event) {
        this.IsPasted = true;
        //console.log("paste paste: " + $event.clipboardData.getData('Text'));
        //this.TextValue = $event.clipboardData.getData('Text');
        //this.TextValueChanges(this.TextValue);
    };
    LogTextBoxComponent.prototype.ngAfterViewInit = function () {
        this.RunComponent();
    };
    LogTextBoxComponent.prototype.AfterViewInited = function () {
        var _this = this;
        if (!this.DebounceTime) {
            this.DebounceTime = 100;
        }
        this.ngzone.runOutsideAngular(function () {
            var input = document.getElementById(_this.InputId);
            _this._debounceTimeSub =
                Observable_1.Observable.fromEvent(input, 'keydown')
                    .debounceTime(_this.DebounceTime)
                    .subscribe(function (keyboardEvent) {
                    var which = keyBoardWhich(keyboardEvent);
                    var key = keyBoardKey(keyboardEvent);
                    if (which == 53) {
                        console.log("");
                    }
                    var result = _this.CheckKey(which, key);
                    if (result != null || which == 13 || key == '-') {
                        var applyTextValue = true;
                        _this.ngzone.run(function () {
                            if ((which == 9 || which == 13) && _this.AllowPercentage) {
                                applyTextValue = false;
                            }
                            if (applyTextValue) {
                                _this.TextValueChanges(_this.TextValue);
                            }
                            if (which == 13) {
                                //if (this.FocusOnMe) {
                                //    var element = document.getElementById(this.InputId);
                                //    if (element) {
                                //        element.focus();
                                //    }
                                //    this.CurrentSession.SessionEvent.emit({ IsCell: true, Id: element.id, IsEnterCLicked: true });
                                //}
                            }
                        });
                        //this.TextValueChanges(this.TextValue);
                        var isDestroyed = _this.cd["destroyed"];
                        if (_this.cd && isDestroyed == false) {
                            _this.cd.detectChanges();
                        }
                    }
                    else {
                        return;
                    }
                });
        });
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
                if (typeof (SelectingElement) === "undefined") {
                }
                else {
                    SelectingElement(element);
                }
            }, 1);
        }
        //check rowscount
        // if (this.IsMultiline) { // commented due single line expand window
        setTimeout(function () {
            if (_this.RowsCount) {
                //calculate height: (rowcount * 18 row height) + 8 padding
                _this.textboxHeight = ((_this.RowsCount * 18) + 8) + 'px';
            }
            else {
                var _element = document.getElementById(_this.InputId);
                var elHeight = _element.clientHeight;
                var calculatedRowsCount = (elHeight / 18);
                var ___roundedCalculatedRowsCountHaha = Math.trunc(calculatedRowsCount);
                _this.RowsCount = ___roundedCalculatedRowsCountHaha;
                _this.textboxHeight = ((_this.RowsCount * 18) + 8) + 'px';
            }
        }, 100);
        // }
    };
    LogTextBoxComponent.prototype.RunComponent = function () {
        var input = document.getElementById(this.InputId);
        if (input) {
            this.AfterViewInited();
        }
        else {
            this.RunComponentTimer();
        }
    };
    LogTextBoxComponent.prototype.RunComponentTimer = function () {
        var _this = this;
        this.Retries++;
        if (this.timerTokenComponent) {
            clearTimeout(this.timerTokenComponent);
        }
        if (this.Retries < 3) {
            this.timerTokenComponent = setTimeout(function () { return _this.RunComponent(); }, 1);
        }
    };
    LogTextBoxComponent.prototype.setDigits = function () {
        switch (SessionLocator_1.SessionLocator.TenantPM.NumberFormatCode) {
            case "CD": {
                this.firstDigit = ",";
                this.secondDigit = ".";
                break;
            }
            case "DC": {
                this.firstDigit = ".";
                this.secondDigit = ",";
                break;
            }
            case "AD": {
                this.firstDigit = "'";
                this.secondDigit = ".";
                break;
            }
            default:
                {
                    this.firstDigit = ",";
                    this.secondDigit = ".";
                    break;
                }
        }
    };
    LogTextBoxComponent.prototype.ngOnInit = function () {
        var _this = this;
        if (this.IsRatioBox == true) {
            this.DigitsAfterPoint = 1;
            this.InputDivStyle = {};
        }
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
    LogTextBoxComponent.prototype.ngxBeforeOnDestroy = function () {
        var _this = this;
        //console.log('1. BEFORE ONDESTROY INVOKE METHOD (await 2 sec)');
        return new Promise(function (resolve) {
            setTimeout(function () { return _this.WaitFunction(resolve); }, 2000);
        });
    };
    LogTextBoxComponent.prototype.ngOnDestroy = function () {
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
    LogTextBoxComponent.prototype.WaitFunction = function (resolve) {
        //console.log('2. EXECUTE HEAVY FUNCTION (3 sec)');
        var sourcef = timer_1.timer(3000)
            .pipe(operators_1.take(1))
            .subscribe(function () {
            resolve();
        });
    };
    LogTextBoxComponent.prototype.onFocus = function () {
        var _this = this;
        this.Detach = false;
        //this.DetectChanges();
        this.show = true;
        if (this.uiProperty.ValidValue) {
            this.timerToken = setTimeout(function () {
                if (_this.IsRatioBox) {
                    _this.InputDivStyle = null;
                }
                else {
                    _this.InputDivStyle = { 'border': '1px solid #3BB3E2' };
                }
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
            if (typeof (SelectingElement) === "undefined") {
            }
            else {
                SelectingElement(input);
            }
        }
    };
    LogTextBoxComponent.prototype.onBlur = function () {
        var _this = this;
        this.timerToken = setTimeout(function () {
            _this.ShowErrorPopup = false;
            if (_this.uiProperty.ValidValue) {
                _this.InputDivStyle = null;
            }
        }, 300);
        this.Detach = true;
        //this.DetectChanges();
        this.show = false;
        // this.TextValue = this.DataContext[this.ObjectFieldName];
        this.keydown = false;
        this.HandleMinusOnlyValue();
        this.GetValueFormatted(this.TextValue);
        this.LostFocus.emit(this.TextValue);
    };
    LogTextBoxComponent.prototype.HandleMinusOnlyValue = function () {
        if (this.TextValue + "" == '-' && this.InputType.toLowerCase() == 'sigdouble') {
            this.TextValue = "0";
        }
    };
    LogTextBoxComponent.prototype.OnKeyUp = function (event) {
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
    LogTextBoxComponent.prototype.OnKeyDown = function (event) {
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
    LogTextBoxComponent.prototype.CheckKey = function (key, keyChar) {
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
                case 'unsdecimal':
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
                                    var selection = window.getSelection().toString();
                                    if (selection == this.TextValue) {
                                        return key;
                                    }
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
                                    var selection = window.getSelection().toString();
                                    if (selection == this.TextValue) {
                                        return key;
                                    }
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
                        if (!Tools_1.AppTool.IsNullOrEmpty(this.TextValue)) {
                            var selection = window.getSelection().toString();
                            if (selection == this.TextValue) {
                                return key;
                            }
                        }
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
                                    var selection = window.getSelection().toString();
                                    if (selection == this.TextValue) {
                                        return key;
                                    }
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
    LogTextBoxComponent.prototype.onchange = function (event) {
        this.Change.emit(this.TextValue);
    };
    LogTextBoxComponent.prototype.GetValueFormatted = function (valueFromField) {
        if (this.TextValue) {
            this.OriginalText.emit(this.TextValue);
            if (this.InputType) {
                switch (this.InputType.toLowerCase()) {
                    case 'double':
                    case 'sigdouble':
                    case 'decimal':
                    case 'unsdecimal':
                        {
                            var isSignOk = true;
                            if (this.InputType == 'unsdecimal' || this.InputType == 'double') {
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
                                if ((this.TextValue + "").indexOf(',') == -1) {
                                    val = Number(this.TextValue);
                                }
                                if (this.AddCommasToNumbers) {
                                    if ((this.TextValue + "").indexOf(',') > -1) {
                                        var txtval = this.TextValue.replace(/,/g, "");
                                        val = Number(txtval);
                                    }
                                }
                            }
                            if (isNaN(val) || !isSignOk) {
                                this.SetValidity(false, TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.InvalidInput")); //"Invalid Input");
                            }
                            else {
                                if (!this.DisableZeroPadding) {
                                    if (Tools_1.AppTool.IsNullOrEmpty(this.DigitsAfterPoint)) {
                                        this.DigitsAfterPoint = 3;
                                    }
                                    this.TextValue = val.toFixed(this.DigitsAfterPoint);
                                }
                            }
                            if (this.AddCommasToNumbers) {
                                var txtNum;
                                if ((this.TextValue + "").indexOf(',') > -1) {
                                    var txtval = this.TextValue.replace(/,/g, "");
                                    txtNum = Number(txtval);
                                }
                                else {
                                    txtNum = Number(this.TextValue);
                                }
                                if (this.DataContext[this.ObjectFieldName] != txtNum) {
                                    this.TextValueChanges(this.TextValue);
                                }
                                if (this.firstDigit == ",") {
                                    var textWithCommas = numberWithCommas(this.TextValue);
                                    if (textWithCommas.indexOf('.') > -1) {
                                        var textWithCommasArr = textWithCommas.split('.');
                                        var beforeDot = textWithCommasArr[0];
                                        var afterDot = textWithCommasArr[1];
                                        if (afterDot.indexOf(',') > -1) {
                                            afterDot = afterDot.replace(',', "");
                                        }
                                        textWithCommas = beforeDot + '.' + afterDot;
                                    }
                                    this.TextValue = textWithCommas;
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
                            //if (this.AddCommasToNumbers) {
                            //    if ((this.TextValue + "").indexOf(',') > -1) {
                            //        var txtval = this.TextValue.replace(',', "");
                            //        val = Number(txtval);
                            //    }
                            //}
                            if (isNaN(val) || !isSignOk) {
                                this.SetValidity(false, TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.InvalidInput")); //"Invalid Input");
                            }
                            else {
                                this.TextValue = val.toFixed(0);
                            }
                            //if (this.AddCommasToNumbers) { //for now it is just for decimal
                            //    if (this.DataContext[this.ObjectFieldName] + "" != this.TextValue) {
                            //        this.TextValueChanges(this.TextValue);
                            //    }
                            //    this.TextValue = numberWithCommas(this.TextValue);
                            //}
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
            }
        }
    };
    LogTextBoxComponent.prototype.TextValueChanges = function (res) {
        if (this.DataContext[this.ObjectFieldName] + "" != this.TextValue) {
            if (this.TextValue) {
                switch (this.InputType) {
                    case 'double':
                    case 'sigdouble':
                    case 'decimal':
                    case 'unsdecimal':
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
                            if (this.TextValue + "" == '-' && this.InputType.toLowerCase() == 'sigdouble')
                                this.DataContext[this.ObjectFieldName] = 0;
                            if (!isNaN(value)) {
                                var isok = true;
                                if (this.InputType == 'unsdecimal' || this.InputType == 'unsinteger' || this.InputType == 'double') {
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
    LogTextBoxComponent.prototype.DataContextValueChanges = function (res) {
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
    LogTextBoxComponent.prototype.SetValidity = function (validValue, errorMessage) {
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
                if (this.IsRatioBox) {
                    this.InputDivStyle = null;
                }
                else {
                    this.InputDivStyle = { 'border': '1px solid #3BB3E2' };
                }
            }
            else {
                this.InputDivStyle = null;
            }
        }
    };
    LogTextBoxComponent.prototype.SetDisabled = function () {
        var inputDiv = document.getElementById(this.InputDivId); //("DatePickerInputDiv");
        if (inputDiv && !this.IsMultiline) {
            inputDiv.classList.add("InputDivDisabled");
        }
        if (this.IsMultiline) {
            var input = document.getElementById(this.InputId);
            if (input) {
                input.classList.add("TextAreaDisabled");
                inputDiv.classList.add("InputDivDisabled");
            }
        }
    };
    LogTextBoxComponent.prototype.SetEnabled = function () {
        var inputDiv = document.getElementById(this.InputDivId); //("DatePickerInputDiv");
        if (inputDiv && !this.IsMultiline) {
            inputDiv.classList.remove("InputDivDisabled");
        }
        if (this.IsMultiline) {
            var input = document.getElementById(this.InputId);
            if (input) {
                input.classList.remove("TextAreaDisabled");
                inputDiv.classList.remove("InputDivDisabled");
            }
        }
    };
    LogTextBoxComponent.prototype.ValidateField = function (emitPropertyChanged) {
        var _this = this;
        if (emitPropertyChanged === void 0) { emitPropertyChanged = true; }
        var suppressValidation = false;
        if (this.InputType) {
            switch (this.InputType.toLowerCase()) {
                case 'text':
                case 'ntext':
                    {
                        break;
                    }
                default: {
                    var val;
                    if ((this.TextValue + "").indexOf(',') == -1) {
                        val = Number(this.TextValue);
                    }
                    if (this.AddCommasToNumbers) {
                        if ((this.TextValue + "").indexOf(',') > -1) {
                            var txtval = this.TextValue.replace(/,/g, "");
                            val = Number(txtval);
                        }
                    }
                    if (this.InputType.toLowerCase() == 'sigdouble' && ((this.TextValue + "") == '-')) {
                        // skip for minus only
                    }
                    else if (isNaN(Number(val))) {
                        this.SetValidity(false, TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.InvalidInput"));
                        suppressValidation = true;
                    }
                    else {
                        this.SetValidity(true, null);
                        suppressValidation = false;
                    }
                }
            }
        }
        if (!this.NoValidation && this.uiProperty != null && !suppressValidation) {
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
    LogTextBoxComponent.prototype.OnMouseOver = function () {
        this.isMouseOver = true;
        if (this.IsDisabled) {
            if (this.IsRatioBox) {
                this.InputDivStyle = null;
            }
            else {
                this.InputDivStyle = { 'border': '1px solid #AAAAAA' };
            }
        }
    };
    LogTextBoxComponent.prototype.OnMouseLeave = function () {
        this.isMouseOver = false;
    };
    Object.defineProperty(LogTextBoxComponent.prototype, "multlineTextBoxLines", {
        get: function () {
            var length = 0;
            if (this.textValue)
                length = this.textValue.split(/\r*\n/).length;
            return length;
        },
        enumerable: true,
        configurable: true
    });
    LogTextBoxComponent.prototype.ExpandButtonClicked = function () {
        console.log("[ExpandButtonClicked]");
        //this.isExpanded = !this.isExpanded;
        this.showMultiLineWindow();
        // if(this.CurrentSession.CurrentWindow){
        //     var wd = document.getElementsByClassName("LogitudeWindow")[0];
        // }
        // var rect = wd.getBoundingClientRect();
        // var left = rect.left;
        // var right = rect.right;
        // var top = rect.top;
        // var bottom = rect.bottom;
        // this.InputDivStyle =
        // {
        //     'z-index': '9999',
        //     'position': 'fixed',
        //     'left': left+'px',
        //     'right': right+'px',
        //     'top': top+'px',
        //     'bottom': bottom+'px',
        //     'width': wd.clientWidth+'px',
        //     'height': wd.clientHeight+'px',
        // };
    };
    LogTextBoxComponent.prototype.showMultiLineWindow = function () {
        var _this = this;
        // show window
        var windowArgs = {};
        // windowArgs.ObjectTableName = this.ObjectTableName;
        // windowArgs.ObjectFieldName = this.ObjectFieldName;
        windowArgs.TextValue = this.TextValue;
        windowArgs.RowsCount = this.RowsCount;
        var wind = new LogitudeWindow_1.LogitudeWindow();
        // wind.IsFullScreen = true;
        wind.Width = 960;
        wind.Height = 570;
        wind.WindowArgs = windowArgs;
        if (this.ObjectField)
            wind.Title = this.showLocal ? this.ObjectField.FullNameTextCodeLocalDefaultText : this.ObjectField.FullNameTextCodeDefaultText;
        else
            wind.Title = "";
        wind.Show("./Infrastructure/Component/LogitudeComponents/MultilineTextBoxWindow");
        wind.WindowClosed.subscribe(function (res) {
            console.log("Rsukt--", res);
            if (res != "<!#cancelled>") {
                _this.TextValue = res;
                _this.TextValueChanges(_this.TextValue);
            }
        });
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], LogTextBoxComponent.prototype, "KeyUp", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], LogTextBoxComponent.prototype, "ValueChanged", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], LogTextBoxComponent.prototype, "LostFocus", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], LogTextBoxComponent.prototype, "InputIdGenerated", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], LogTextBoxComponent.prototype, "Change", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], LogTextBoxComponent.prototype, "HasValue", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogTextBoxComponent.prototype, "DontUseTimer", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogTextBoxComponent.prototype, "DisableZeroPadding", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogTextBoxComponent.prototype, "AllowSpellCheck", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogTextBoxComponent.prototype, "AllowLocalLanguage", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogTextBoxComponent.prototype, "IsNewEntityCall", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogTextBoxComponent.prototype, "NoObjectField", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogTextBoxComponent.prototype, "NoValidation", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], LogTextBoxComponent.prototype, "Placeholder", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogTextBoxComponent.prototype, "AlignTextToRight", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], LogTextBoxComponent.prototype, "AddCommasToNumbers", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Number)
    ], LogTextBoxComponent.prototype, "Max", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Number)
    ], LogTextBoxComponent.prototype, "Min", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Number)
    ], LogTextBoxComponent.prototype, "RowsCount", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Object),
        __metadata("design:paramtypes", [Object])
    ], LogTextBoxComponent.prototype, "Text", null);
    __decorate([
        core_1.Input(),
        __metadata("design:type", forms_1.FormGroup)
    ], LogTextBoxComponent.prototype, "LogitudeForm", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], LogTextBoxComponent.prototype, "OriginalText", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Number)
    ], LogTextBoxComponent.prototype, "DebounceTime", void 0);
    __decorate([
        BeforeOnDestroy,
        __metadata("design:type", Function),
        __metadata("design:paramtypes", []),
        __metadata("design:returntype", void 0)
    ], LogTextBoxComponent.prototype, "ngOnDestroy", null);
    LogTextBoxComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'LogTextBox',
            templateUrl: "./LogTextBoxComponent.html",
            //directives: [CORE_DIRECTIVES, FORM_DIRECTIVES, HelpIcon, FixedPositionDirective],
            inputs: ['ObjectFieldName', 'ObjectTableName', 'DataContext', "IsMultiline", "InputType", "HideColumns", "HideLastColumn", "DigitsAfterPoint", "FocusOnMe", "IsFreeText", "IsAccumulative", "AllowPercentage", "UseArialFont", "DontAllowAutoSelect", 'IsRatioBox'],
        }),
        __metadata("design:paramtypes", [core_1.NgZone, core_1.ChangeDetectorRef,
            core_1.ApplicationRef])
    ], LogTextBoxComponent);
    return LogTextBoxComponent;
}());
exports.LogTextBoxComponent = LogTextBoxComponent;
//# sourceMappingURL=LogTextBoxComponent.js.map