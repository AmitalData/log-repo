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
var LogColumnComponent_1 = require("./LogColumnComponent");
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var LogCellTemplateComponent = /** @class */ (function () {
    function LogCellTemplateComponent(CC, CD) {
        var _this = this;
        this.CD = CD;
        this.IgnoreMods = false;
        this.RIndex = -1;
        this.CellClicked = new core_1.EventEmitter();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.RowIndex = 0;
        this.TempRowIndex = 0;
        this.IsFilled = false;
        this.isEnabled = true;
        this.cellColor = "transparent";
        this.fontColor = "#282E30";
        this.disableColors = false;
        this.tabIndex = 0;
        this.alignment = "left";
        this.isEditMode = false;
        this.isDisplayMode = true;
        this.leftIndent = 3;
        this.rightIndent = 3;
        this.IsClickedOnce = false;
        this.ColumnComponent = CC;
        //alert(this.ColumnComponent.LogGridId)
        this.ObsNewElementInsertedSub = this.CurrentSession.ObsNewElementInsertedEvent.subscribe(function (res) {
            if (res.Id == _this.ColumnComponent.LogGridId) {
                //var ctrl = document.getElementById(this.ColumnComponent.LogGridId + this.CurrentSession.SessionIndex + "_" + 0 + "_" + (res.length));
                //if (ctrl) {
                //    ctrl.focus();
                //}
                var rowIndex = 0;
                var focusSpecificRow = false;
                if (res.RowIndex) {
                    rowIndex = res.RowIndex;
                    focusSpecificRow = true;
                }
                else {
                    if (_this.RIndex == -1 || _this.RIndex == null) {
                        rowIndex = res.length - 1;
                    }
                    else {
                        rowIndex = res.length;
                    }
                }
                _this.focusFirstEditableElement(0, rowIndex, focusSpecificRow);
                //if (this.RIndex == -1 || this.RIndex == null) {
                //    this.focusFirstEditableElement(0, res.length - 1);
                //}
                //else {
                //    this.focusFirstEditableElement(0, res.length);
                //}
            }
        });
    }
    LogCellTemplateComponent.prototype.focusFirstEditableElement = function (i, length, focusSpecificRow) {
        if (focusSpecificRow === void 0) { focusSpecificRow = false; }
        var ctrl = document.getElementById(this.ColumnComponent.LogGridId + this.CurrentSession.SessionIndex + "_" + i + "_" + length); //(length - 1 < 0 ? 0 : length - 1)
        if (ctrl == null) {
            ctrl = document.getElementById(this.ColumnComponent.LogGridId + this.CurrentSession.SessionIndex + "_" + i + "_" + (length - 1));
        }
        if (focusSpecificRow) {
            ctrl = document.getElementById(this.ColumnComponent.LogGridId + this.CurrentSession.SessionIndex + "_" + i + "_" + length);
        }
        if (ctrl) {
            if (ctrl.tabIndex == 0) {
                ctrl.focus();
                return;
            }
            else {
                this.focusFirstEditableElement(i + 1, length, focusSpecificRow);
            }
        }
    };
    LogCellTemplateComponent.prototype.SetCellColors = function () {
        var myTabIndex = 0;
        var myFontColor = "#282E30";
        var myCellColor = "transparent";
        if (this.IsEnabled == false) {
            myTabIndex = -1;
            if (!this.DisableColors) {
                myFontColor = "#A8AAAD";
                myCellColor = "rgba(230, 231, 232, 0.5)";
            }
        }
        this.TabIndex = myTabIndex;
        this.FontColor = myFontColor;
        this.CellColor = myCellColor;
    };
    Object.defineProperty(LogCellTemplateComponent.prototype, "IsEnabled", {
        get: function () { return this.isEnabled; },
        set: function (value) {
            if (this.isEnabled != value) {
                this.isEnabled = value;
                this.SetCellColors();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogCellTemplateComponent.prototype, "CellColor", {
        get: function () { return this.cellColor; },
        set: function (value) {
            if (this.cellColor != value) {
                this.cellColor = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogCellTemplateComponent.prototype, "FontColor", {
        get: function () { return this.fontColor; },
        set: function (value) {
            if (this.fontColor != value) {
                this.fontColor = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogCellTemplateComponent.prototype, "DisableColors", {
        get: function () { return this.disableColors; },
        set: function (value) {
            if (this.disableColors != value) {
                this.disableColors = value;
                this.SetCellColors();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogCellTemplateComponent.prototype, "TabIndex", {
        get: function () { return this.tabIndex; },
        set: function (value) {
            if (this.tabIndex != value) {
                this.tabIndex = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogCellTemplateComponent.prototype, "Alignment", {
        get: function () { return this.alignment; },
        set: function (value) {
            if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                value = value.toLowerCase();
            }
            if (this.alignment != value) {
                this.alignment = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogCellTemplateComponent.prototype, "IsEditMode", {
        get: function () { return this.isEditMode; },
        set: function (value) {
            if (this.isEditMode != value) {
                this.isEditMode = value;
                if (value) {
                    this.LeftIndent = this.RightIndent = 0;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogCellTemplateComponent.prototype, "IsDisplayMode", {
        get: function () { return this.isDisplayMode; },
        set: function (value) {
            if (this.isDisplayMode != value) {
                this.isDisplayMode = value;
                if (value) {
                    this.LeftIndent = this.RightIndent = 3;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogCellTemplateComponent.prototype, "LeftIndent", {
        get: function () { return this.leftIndent; },
        set: function (value) {
            if (!this.IsFilled) {
                if (this.leftIndent != value) {
                    this.leftIndent = value;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LogCellTemplateComponent.prototype, "RightIndent", {
        get: function () { return this.rightIndent; },
        set: function (value) {
            if (!this.IsFilled) {
                if (this.rightIndent != value) {
                    this.rightIndent = value;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    LogCellTemplateComponent.prototype.ngOnInit = function () {
        if (this.IsFilled) {
            this.leftIndent = this.rightIndent = 0;
        }
        this.HStyle = this.ColumnComponent.HeaderStyle;
        this.headertext = this.ColumnComponent.header;
        this.bindingfield = this.ColumnComponent.binding;
        this.cellwidth = this.ColumnComponent.width;
        this.cellvisibility = this.ColumnComponent.visibility;
        this.isEditable = this.ColumnComponent.Editable;
        this.isrequired = this.ColumnComponent.required;
        //this.Editindex = this.ColumnComponent.Editindex;
        ////this.CurrentSession.ResetRowIndex();
        if (this.RIndex >= 0) {
            this.RowIndex = this.RIndex;
            //console.log("RIndex = " + this.RowIndex + " LogId : " + this.ColumnComponent.LogGridId);
        }
        else {
            this.RowIndex = this.CurrentSession.LogitudeGridHelper.GetNextRowIndex(this.ColumnComponent.LogGridId);
            //this.RowIndex = this.CurrentSession.LogitudeGridHelper.GetRowIndex(this.ColumnComponent.LogGridId);
            //console.log("RowIndex = " + this.RowIndex + " LogId : " + this.ColumnComponent.LogGridId);
        }
        this.TempRowIndex = this.RowIndex;
        this.OuterDivId = this.ColumnComponent.LogGridId + this.CurrentSession.SessionIndex + "_" + this.ColumnComponent.index + "_" + this.RowIndex;
        //if (this.RowIndex == 0) {
        //    this.RowIndex = 1;
        //}
        var ind = this.CurrentSession.LogitudeGridHelper.GetEditCellIndex();
        if ((this.ColumnComponent.index + 1) == this.CurrentSession.LogitudeGridHelper.getColumnsCount(this.ColumnComponent.LogGridId)) {
            //console.log("IN " + this.RowIndex);
            this.CurrentSession.LogitudeGridHelper.ResetEditCellIndex();
            ind = this.CurrentSession.LogitudeGridHelper.GetEditCellIndex();
            if (this.RIndex < 0) {
                this.RowIndex = this.CurrentSession.LogitudeGridHelper.GetNextRowIndex(this.ColumnComponent.LogGridId);
                this.CurrentSession.LogitudeGridHelper.SetNextRowIndex(this.ColumnComponent.LogGridId);
                //this.RowIndex = this.CurrentSession.LogitudeGridHelper.SetRowIndex(this.ColumnComponent.LogGridId);
            }
            console.log("RowIndex = " + this.RowIndex + " LogId : " + this.ColumnComponent.LogGridId);
            this.TempRowIndex = this.RowIndex;
        }
        this.Editindex = ind;
    };
    //this.Editindex
    LogCellTemplateComponent.prototype.KeyUpEvent = function ($event) {
        //if ((this.ColumnComponent.index + 1) == this.CurrentSession.LogitudeGridHelper.getColumnsCount(this.ColumnComponent.LogGridId)) {
        //    //console.log("End Of Current Row : " + this.Editindex);
        //    this.CurrentSession.EndOfRowReachedEvent.emit(this.RowIndex + 1);
        //}
        if ($event.keyCode == 9) {
            if ($event.shiftKey) {
                //this.CurrentSession.isShiftClicked = false;
                this.CurrentSession.isTabWithShiftClicked = false;
            }
        }
    };
    LogCellTemplateComponent.prototype.KeyDownEvent = function ($event) {
        if ($event.keyCode == 9) {
            if ($event.shiftKey) {
                this.CurrentSession.isTabWithShiftClicked = true;
            }
        }
        var colCount = this.CurrentSession.LogitudeGridHelper.getColumnsCount(this.ColumnComponent.LogGridId);
        if ((this.ColumnComponent.index + 1) == colCount && $event.keyCode == 9) {
            //console.log("End Of Current Row : " + this.Editindex);
            this.CurrentSession.CurrentLogGrid = this.ColumnComponent.LogGridId;
            this.CurrentSession.EndOfRowReachedEvent.emit(this.RowIndex + 1);
        }
        if ($event.keyCode == 13) {
            var element = document.getElementById(this.ColumnComponent.LogGridId + this.CurrentSession.SessionIndex + "_" + this.ColumnComponent.index + "_" + (this.TempRowIndex + 1));
            if (element) {
                element.focus();
            }
        }
        if ($event.keyCode == 121) {
            if ($event.altKey) {
                var originalElement = document.getElementById(this.ColumnComponent.LogGridId + this.CurrentSession.SessionIndex + "_" + this.ColumnComponent.index + "_" + (this.TempRowIndex));
                //var nextElement = document.getElementById(this.ColumnComponent.LogGridId + this.CurrentSession.SessionIndex + "_" + this.ColumnComponent.index + "_" + (this.TempRowIndex + 1));
                var originalData = this.ColumnComponent.EditableLogGridComponent.ItemSource.Collection[this.RowIndex - 1][this.ColumnComponent.binding];
                var nextData = this.ColumnComponent.EditableLogGridComponent.ItemSource.Collection[this.RowIndex][this.ColumnComponent.binding];
                var elementinputs = originalElement.getElementsByTagName("input");
                if (elementinputs.length > 0) {
                    this.CurrentSession.CopiedCell = originalData;
                    this.CurrentSession.CopyCellIntoMemory.emit(elementinputs[0].id);
                }
                //this.ColumnComponent.EditableLogGridComponent.ItemSource.Collection[this.RowIndex][this.ColumnComponent.binding] = originalData;
                if ((this.ColumnComponent.index + 1) == colCount) {
                    this.CurrentSession.CurrentLogGrid = this.ColumnComponent.LogGridId;
                    this.CurrentSession.EndOfRowReachedEvent.emit(this.RowIndex + 1);
                    if (this.ColumnComponent.EditableLogGridComponent.ItemSource.Length > (this.RowIndex + 1)) {
                        var columnIndex = this.ColumnComponent.index + 1;
                        var elementId = this.ColumnComponent.LogGridId + this.CurrentSession.SessionIndex + "_" + 1 + "_" + (this.TempRowIndex + 1);
                        var nextElement = this.getNextIndexedElement(this.CurrentSession.SessionIndex, (this.ColumnComponent.index + 1), (this.TempRowIndex)); //document.getElementById(elementId);
                        nextElement.focus();
                    }
                }
                else {
                    var columnIndex = this.ColumnComponent.index + 1;
                    var elementId = this.ColumnComponent.LogGridId + this.CurrentSession.SessionIndex + "_" + columnIndex + "_" + (this.TempRowIndex);
                    var nextElement = document.getElementById(elementId);
                    nextElement.focus();
                }
                //var elements = originalElement.getElementsByTagName("logtextbox");
                //var elementinputs = originalElement.getElementsByTagName("input");
                //if (elementinputs != null && elementinputs.length>0 && nextElement != null) {
                //    this.CurrentSession.CopyCellIntoMemory.emit(elementinputs[0].id);
                //    nextElement.focus();
                //}
                //var fieldName = elements[0].getAttributeNode("ng-reflect--object-field-name");
                //var dataContext = elements[0].getAttributeNode("ng-reflect--data-context");
                //var nextElements = nextElement.getElementsByTagName("logtextbox");
                //var nFieldName = nextElements[0].getAttribute("ng-reflect--object-field-name");
                //var nDataContext = nextElements[0].getAttribute("ng-reflect--data-context");
                // nDataContext[nFieldName] = dataContext[fieldName];
            }
        }
    };
    LogCellTemplateComponent.prototype.blurevt = function (_thisComponent) {
        var _this = this;
        if (!SessionLocator_1.SessionLocator.SustainFocusOnCell) {
            //this.focusTimerToken = setTimeout(() => {
            //  ElementProperities(_thisComponent);
            //}, 1);
            ElementProperities(_thisComponent);
            if (_thisComponent.EventSub) {
                _thisComponent.EventSub.unsubscribe();
                _thisComponent.IsClickedOnce = false;
            }
            //console.log("Cell Id : " + _thisComponent.MyElement.id);
            _thisComponent.MyElement.removeEventListener("blur", function () { return _this.blurevt(_thisComponent); });
            // SessionLocator.SustainLostFocusOnCell = true;
        }
        //else {
        //    SessionLocator.SustainFocusOnCell = false;
        //    // SessionLocator.SustainLostFocusOnCell = false;
        //}
    };
    LogCellTemplateComponent.prototype.OnBlurEventImplementation = function (_thisComponent) {
        if (!SessionLocator_1.SessionLocator.SustainFocusOnCell) {
            this.focusTimerToken = setTimeout(function () {
                ElementProperities(_thisComponent);
            }, 1);
            if (_thisComponent.EventSub) {
                _thisComponent.EventSub.unsubscribe();
                _thisComponent.IsClickedOnce = false;
            }
            this.CurrentSession.LostFocusEvent.emit("");
        }
        else {
            SessionLocator_1.SessionLocator.SustainFocusOnCell = false;
        }
    };
    LogCellTemplateComponent.prototype.OnClick = function () {
        if (this.CurrentSession.isTabWithShiftClicked == false) {
            if (this.IsEnabled && !this.ColumnComponent.IsReadOnlyGrid) {
                if (this.isEditable && this.IgnoreMods == false) {
                    if (this.TabIndex == 0) {
                        this.IsEditMode = true;
                        this.IsDisplayMode = false;
                        this.CellClicked.emit(this.RowIndex);
                    }
                }
            }
            if (!this.IsClickedOnce) {
                this.SubscribeCellFocus();
            }
            this.IsClickedOnce = true;
        }
        else {
            this.CurrentSession.isTabWithShiftClicked = false;
            //this.CurrentSession.isShiftClicked = false;
            //console.log("isTabWithShiftClicked = false;")
            //this.CurrentSession.isShiftClicked = false
            var element = this.getPrevIndexedElement(this.CurrentSession.SessionIndex, (this.ColumnComponent.index - 1), (this.TempRowIndex));
            if (element) {
                element.focus();
            }
            //else {
            //    this.CurrentSession.AllowShiftTab = false;
            //    //this.CurrentSession.isTabWithShiftClicked = false;
            //    //console.log("isTabWithShiftClicked = false;")
            //}
        }
    };
    LogCellTemplateComponent.prototype.SubscribeCellFocus = function () {
        var _this = this;
        this.EventSub = this.CurrentSession.SessionEvent.subscribe(function (res) {
            if (SessionLocator_1.SessionLocator.SustainFocusOnCell) {
                if (_this.OuterDivId == res.OuterDivId) {
                    _this.IsEditMode = true;
                    _this.IsDisplayMode = false;
                    var element = document.getElementById(res.LogTextBoxId);
                    element.focus();
                }
            }
            if (res.IsCell && !res.IsEnterCLicked) {
                var _thisComponent = _this;
                if (res.OnBlurEvent) {
                    res.OnBlurEvent.subscribe(function (res1) { return _this.OnBlurEventImplementation(_thisComponent); });
                }
                else {
                    _this.MyElement = document.getElementById(res.Id);
                    _this.MyElement.addEventListener("blur", function () { return _this.blurevt(_thisComponent); });
                }
            }
            if (res.IsCell && res.IsEnterCLicked == true) {
                var ctrl = document.getElementById(res.Id);
                var mine = _this.checkIt(ctrl);
                if (mine) {
                    _this.IsEditMode = false;
                    _this.IsDisplayMode = true;
                    var element = document.getElementById(_this.ColumnComponent.LogGridId + _this.CurrentSession.SessionIndex + "_" + _this.ColumnComponent.index + "_" + (_this.TempRowIndex + 1));
                    if (element) {
                        element.focus();
                    }
                }
            }
        });
    };
    //SetFirstLostFocusFalse() {
    //    this.IsLostFocusOnce = false;
    //}
    LogCellTemplateComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.EventSub);
        Tools_1.AppTool.KillEventEmitter(this.ObsNewElementInsertedSub);
    };
    LogCellTemplateComponent.prototype.ObBlure = function () {
        //console.log("ObBlure");
        //setTimeout(function () {
        //    var focus = document.activeElement;
        //    var MainElement = document.getElementById("OuterDiv");
        //    for (var i = 0; i < MainElement.children.length; i++) {
        //        var tableChild = MainElement.children[i];
        //        if (tableChild.isEqualNode(focus)) {
        //            console.log("still focused");
        //        } else {
        //            this.IsDisplayMode = true;
        //            this.IsEditMode = false;
        //            console.log("No");
        //            this.CD.detectChanges();
        //        }
        //    }
        //}, 0);
    };
    LogCellTemplateComponent.prototype.addListner = function (MainElement) {
        for (var i = 0; i < MainElement.children.length; i++) {
            var Child = MainElement.children[i];
            this.addListner(Child);
        }
        MainElement.addEventListener("blur", function () {
            MainMenuProperties(MainElement);
            //this.IsDisplayMode = true;
            //this.IsEditMode = false;
            //this.CD.detectChanges();
            //console.log("OnCustomBlure");
        });
    };
    //@Input('') active = false;
    LogCellTemplateComponent.prototype.checkIt = function (ctrl) {
        //var ctrl = document.getElementById(ctrlId);
        if (ctrl.parentElement) {
            if (this.OuterDivId == ctrl.parentElement.id) {
                return true;
            }
            return this.checkIt(ctrl.parentElement);
        }
        else {
            return false;
        }
    };
    LogCellTemplateComponent.prototype.getPrevIndexedElement = function (SessionIndex, Columnindex, Rowindex) {
        //this.CurrentSession.getColumnsCount()
        if (Columnindex >= 0 || Rowindex > 0) {
            var ctrl = document.getElementById(this.ColumnComponent.LogGridId + SessionIndex + "_" + Columnindex + "_" + Rowindex);
            if (Columnindex < 0) {
                var ctrl = document.getElementById(this.ColumnComponent.LogGridId + SessionIndex + "_" + (this.CurrentSession.LogitudeGridHelper.getColumnsCount(this.ColumnComponent.LogGridId) - 1) + "_" + (Rowindex - 1));
                if (ctrl == null || ctrl.tabIndex != 0) {
                    return this.getPrevIndexedElement(SessionIndex, (this.CurrentSession.LogitudeGridHelper.getColumnsCount(this.ColumnComponent.LogGridId) - 1), Rowindex - 1);
                }
                else {
                    return ctrl;
                }
            }
            if (ctrl == null || ctrl.tabIndex != 0) {
                return this.getPrevIndexedElement(SessionIndex, Columnindex - 1, Rowindex);
            }
            else {
                return ctrl;
            }
        }
        else {
            return null;
        }
    };
    LogCellTemplateComponent.prototype.getNextIndexedElement = function (SessionIndex, Columnindex, Rowindex) {
        //this.CurrentSession.getColumnsCount()
        var colCount = this.CurrentSession.LogitudeGridHelper.getColumnsCount(this.ColumnComponent.LogGridId);
        if (Columnindex <= colCount || Rowindex > 0) {
            var ctrl = document.getElementById(this.ColumnComponent.LogGridId + SessionIndex + "_" + Columnindex + "_" + Rowindex);
            if (Columnindex > colCount) {
                var ctrl = document.getElementById(this.ColumnComponent.LogGridId + SessionIndex + "_" + 0 + "_" + (Rowindex + 1));
                if (ctrl == null || ctrl.tabIndex != 0) {
                    return this.getNextIndexedElement(SessionIndex, (Columnindex > 0 ? (0 + 1) : (Columnindex + 1)), Rowindex + 1);
                }
                else {
                    return ctrl;
                }
            }
            if (ctrl == null || ctrl.tabIndex != 0) {
                return this.getNextIndexedElement(SessionIndex, Columnindex + 1, Rowindex);
            }
            else {
                return ctrl;
            }
        }
        else {
            return null;
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], LogCellTemplateComponent.prototype, "CellClicked", void 0);
    LogCellTemplateComponent = __decorate([
        core_1.Component({
            selector: 'log-cell-template',
            moduleId: module.id,
            templateUrl: './LogCellTemplateComponent.html',
            inputs: ['TabIndex', 'IsEnabled', 'CellColor', 'Alignment', 'IsFilled', 'DisableColors', 'IgnoreMods', 'RIndex', 'IsEditMode']
        }),
        __metadata("design:paramtypes", [LogColumnComponent_1.LogColumnComponent, core_1.ChangeDetectorRef])
    ], LogCellTemplateComponent);
    return LogCellTemplateComponent;
}());
exports.LogCellTemplateComponent = LogCellTemplateComponent;
//# sourceMappingURL=LogCellTemplateComponent.js.map