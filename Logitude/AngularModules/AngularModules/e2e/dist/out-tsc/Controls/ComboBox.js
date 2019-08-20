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
var SessionLocator_1 = require("../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../Infrastructure/Tools");
var ObjectsLocator_1 = require("../Infrastructure/Locators/ObjectsLocator");
var ComboBox = /** @class */ (function () {
    function ComboBox(cd) {
        this.cd = cd;
        this.Text = null;
        this.WaterMark = null;
        this.Binding = null;
        this.ControlId = null;
        this.DropdownId = null;
        this.ListControlId = null;
        this.MinHeight = 30;
        this.MaxHeight = 250;
        this.IsBlueBox = false;
        this.IsDisabled = false;
        this.IsMouseOverControl = false;
        this.FocusOnMe = false;
        this.LayoutDirection = 'ltr';
        this.SelectedItemChanged = new core_1.EventEmitter();
        this.ComboBoxDropDownClicked = new core_1.EventEmitter();
        this.LostFocus = new core_1.EventEmitter();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.MouseDownEvent = null;
        //private itemsSource: any[];
        //get ItemsSource() { return this.itemsSource; }
        //set ItemsSource(value: any[]) {
        //    if (this.itemsSource != value) {
        //        this.itemsSource = value;
        //        if (this.ItemsButtonClicked && !this.IsOpened) {
        //            this.CloseDropDown();
        //        }
        //        this.ItemsButtonClicked = false;
        //    }
        //}
        this.isGreenButton = false;
        this.selectedItem = null;
        this.selectedValue = null;
        this.selectedValuePath = null;
        this.isOpened = false;
        this.LayoutDirection = ObjectsLocator_1.ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection;
        this.ItemsSource = [];
        if (this.CurrentSession == null) {
            this.ControlId = "ComboBox_-1_-1";
            this.DropdownId = "Dropdown_-1_-1";
            this.ListControlId = "List_-1_-1";
        }
        else {
            var idIndex = this.CurrentSession.GetNewId("ComboBox");
            this.ControlId = "ComboBox_" + idIndex;
            this.DropdownId = "Dropdown_" + idIndex;
            this.ListControlId = "List_" + idIndex;
        }
    }
    Object.defineProperty(ComboBox.prototype, "ItemsSource", {
        get: function () { return this.itemsSource; },
        set: function (value) {
            var _this = this;
            if (this.itemsSource != value) {
                this.itemsSource = value;
                if (this.ItemsButtonClicked) {
                    //this.OnMouseDown(false);
                    if (this.dropdownTimertoken) {
                        clearTimeout(this.dropdownTimertoken);
                    }
                    this.dropdownTimertoken = setTimeout(function () {
                        //this.OnMouseDown(false);
                        if (_this.selectedIndex)
                            _this.SelectedItem = _this.ItemsSource[_this.selectedIndex];
                    }, 1);
                    //this.cd.detectChanges();
                }
                this.ItemsButtonClicked = false;
            }
        },
        enumerable: true,
        configurable: true
    });
    ComboBox.prototype.ngOnInit = function () {
        var _this = this;
        if (this.SelectedItem != null) {
            this.SetDisplayText();
        }
        if (this.SelectedValue != null) {
            this.SetSelectedItemFromValue();
        }
        if (this.CurrentSession) {
            if (this.CurrentSession.MouseDownEvent) {
                this.MouseDownEvent = this.CurrentSession.MouseDownEvent.subscribe(function (s) {
                    if (s) {
                        if (_this.IsOpened) {
                            if (_this.IsMouseOverControl == false) {
                                _this.OnLostFocus();
                            }
                        }
                    }
                });
            }
        }
    };
    ComboBox.prototype.ngAfterViewInit = function () {
        if (this.FocusOnMe) {
            var element = document.getElementById(this.ControlId);
            element.focus();
            this.CurrentSession.SessionEvent.emit({ IsCell: true, Id: element.id });
            //this.timerToken = setTimeout(() => {
            //    SelectingElement(element);
            //}, 1);
        }
    };
    ComboBox.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.MouseDownEvent);
    };
    Object.defineProperty(ComboBox.prototype, "IsGreenButton", {
        get: function () { return this.isGreenButton; },
        set: function (value) {
            if (this.isGreenButton != value) {
                this.isGreenButton = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ComboBox.prototype, "SelectedItem", {
        get: function () { return this.selectedItem; },
        set: function (value) {
            if (this.selectedItem != value) {
                this.selectedItem = value;
                this.SetDisplayText();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ComboBox.prototype, "SelectedValue", {
        get: function () { return this.selectedValue; },
        set: function (value) {
            if (this.selectedValue != value) {
                this.selectedValue = value;
                this.SetSelectedItemFromValue();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ComboBox.prototype, "SelectedValuePath", {
        get: function () { return this.selectedValuePath; },
        set: function (value) {
            if (this.selectedValuePath != value) {
                this.selectedValuePath = value;
                this.SetSelectedItemFromValue();
            }
        },
        enumerable: true,
        configurable: true
    });
    ComboBox.prototype.SetSelectedItemFromValue = function () {
        var _this = this;
        if (this.SelectedValue && this.SelectedValuePath) {
            var selectedItem = this.ItemsSource.filter(function (f) { return f[_this.SelectedValuePath] === _this.SelectedValue; })[0];
            this.SelectedItem = selectedItem;
        }
        else {
            this.SelectedItem = null;
        }
    };
    Object.defineProperty(ComboBox.prototype, "IsOpened", {
        get: function () { return this.isOpened; },
        set: function (value) {
            if (value != undefined) {
                if (this.isOpened != value) {
                    this.isOpened = value;
                    if (value) {
                        this.SetPopupSize();
                        this.RunPositionTimer();
                    }
                    else {
                        this.StopPositionTimer();
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    ComboBox.prototype.StopPositionTimer = function () {
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
    };
    ComboBox.prototype.RunPositionTimer = function () {
        var _this = this;
        this.StopPositionTimer();
        this.timerToken = setInterval(function () { return _this.CalculateFixedPosition(); }, 0);
    };
    ComboBox.prototype.CalculateFixedPosition = function () {
        var item = document.getElementById(this.ControlId);
        if (item) {
            var itemRect = item.getBoundingClientRect();
            document.getElementById(this.DropdownId).style.top = (itemRect.top + 23) + 'px';
            document.getElementById(this.DropdownId).style.left = (itemRect.left) + 'px';
        }
    };
    ComboBox.prototype.SetPopupSize = function () {
        var item = document.getElementById(this.ControlId);
        if (item) {
            var itemRect = item.getBoundingClientRect();
            document.getElementById(this.DropdownId).style.width = itemRect.width + "px";
            if (this.ItemsSource.length == 0) {
                document.getElementById(this.DropdownId).style.height = this.MinHeight + "px";
            }
            else {
                var itemsHeight = ((this.ItemsSource.length * 23) + 3);
                if (itemsHeight > this.MaxHeight) {
                    document.getElementById(this.DropdownId).style.height = this.MaxHeight + "px";
                    document.getElementById(this.ListControlId).style.height = itemsHeight + "px";
                }
                else {
                    document.getElementById(this.DropdownId).style.height = itemsHeight + "px";
                    document.getElementById(this.ListControlId).style.height = "100%";
                }
            }
        }
    };
    ComboBox.prototype.mousedown = function () {
        if (this.IsOpened) {
            this.StopPositionTimer();
            this.IsOpened = false;
        }
        else {
            this.CalculateFixedPosition();
            this.IsOpened = true;
            //this.ItemsButtonClicked = true;
            this.ComboBoxDropDownClicked.emit();
        }
    };
    ComboBox.prototype.OnLostFocus = function () {
        this.CloseDropDown();
        this.LostFocus.emit(true);
    };
    ComboBox.prototype.CloseDropDown = function () {
        this.IsOpened = false;
        this.StopPositionTimer();
        this.IsOpened = false;
    };
    ComboBox.prototype.ItemClicked = function (clickedItem) {
        if (clickedItem != null) {
            if (this.SelectedItem != clickedItem) {
                this.SelectedItem = clickedItem;
                this.SetDisplayText();
                this.SelectedItemChanged.emit(this.SelectedItem);
                this.CloseDropDown();
                //}
            }
        }
    };
    ComboBox.prototype.SetDisplayText = function () {
        var myDisplayText = null;
        if (this.SelectedItem != null) {
            if (this.Binding == null) {
                myDisplayText = this.SelectedItem;
            }
            else {
                myDisplayText = this.SelectedItem[this.Binding];
            }
        }
        this.Text = myDisplayText;
    };
    ComboBox.prototype.OnKeyDown = function ($event) {
        if ($event) {
            switch ($event.keyCode) {
                case 8:
                case 46: // delete, backspace
                    {
                        this.SelectedItem = null;
                        this.SelectedItemChanged.emit(null);
                        break;
                    }
                case 40: // ↓
                    {
                        if (!this.IsOpened) {
                            this.OnMouseDown(true);
                            this.InitiateSelectedItem();
                        }
                        else {
                            // navigate to items
                            this.selectedIndex = ((this.selectedIndex == this.ItemsSource.length - 1) ? 0 : this.selectedIndex + 1); // increment index
                            this.SetSelectedItem(this.selectedIndex);
                        }
                        break;
                    }
                case 38: // ↑
                    {
                        if (this.IsOpened) {
                            // navigate to items
                            this.selectedIndex = ((this.selectedIndex == 0) ? this.ItemsSource.length - 1 : this.selectedIndex - 1); // decrement index
                            this.SetSelectedItem(this.selectedIndex);
                        }
                        break;
                    }
                case 13: // Enter
                    {
                        if (!this.IsOpened) {
                            this.OnMouseDown(true);
                            this.IsOpened = true;
                        }
                        else {
                            this.SetDisplayText();
                            this.SelectedItemChanged.emit(this.SelectedItem);
                            this.CloseDropDown();
                        }
                        break;
                    }
                case 27: // Esc
                    {
                        if (this.IsOpened) {
                            this.CloseDropDown();
                        }
                        break;
                    }
                case 9: // Tab
                    {
                        if (this.IsOpened) {
                            this.SetDisplayText();
                            this.SelectedItemChanged.emit(this.SelectedItem);
                            this.CloseDropDown();
                        }
                        break;
                    }
            }
        }
    };
    ComboBox.prototype.SetSelectedItem = function (index) {
        if (this.ItemsSource && !Tools_1.AppTool.IsNullOrEmpty(index)) {
            //Select the item
            this.SelectedItem = this.ItemsSource[index];
            this.cd.detectChanges();
            //Set scroll
            var selectedElements = document.getElementsByClassName("SelectedComboboxItem");
            if (selectedElements) {
                var selectedElement = selectedElements[0];
                if (selectedElement)
                    selectedElement.scrollIntoView(false);
            }
        }
    };
    ComboBox.prototype.mousedownGreenButton = function () {
        var clickedItem = this.SelectedItem;
        if (clickedItem != null) {
            var myComboBox = document.getElementById(this.ControlId);
            if (myComboBox != null) {
                myComboBox.blur();
            }
            this.OnLostFocus();
            this.SetDisplayText();
            this.SelectedItemChanged.emit(this.SelectedItem);
        }
    };
    ComboBox.prototype.InitiateSelectedItem = function () {
        // set selected item for first time
        if (this.ItemsSource && this.ItemsSource.length > 0) {
            if (!this.SelectedItem) {
                this.selectedIndex = 0;
                this.SelectedItem = this.ItemsSource[0];
            }
            else {
                var index = this.ItemsSource.indexOf(this.SelectedItem);
                this.selectedIndex = index;
                this.SetSelectedItem(this.selectedIndex);
            }
        }
    };
    ComboBox.prototype.OnMouseDown = function (emitClicked) {
        var myRootControl = document.getElementById(this.ControlId);
        if (myRootControl) {
            var myDropDownControl = document.getElementById(this.DropdownId);
            if (myDropDownControl) {
                if (myDropDownControl.style.visibility == "visible") {
                    this.CloseDropDown();
                }
                else {
                    this.IsOpened = true;
                    // set selected item for first time
                    this.InitiateSelectedItem();
                    if (this.ItemsSource == null) {
                        this.ItemsSource = [];
                    }
                    myDropDownControl.style.width = myRootControl.offsetWidth + "px";
                    if (this.ItemsSource.length == 0) {
                        myDropDownControl.style.height = this.MinHeight + "px";
                    }
                    else {
                        var itemsHeight = ((this.ItemsSource.length * 23) + 3);
                        if (itemsHeight > this.MaxHeight) {
                            myDropDownControl.style.height = this.MaxHeight + "px";
                            document.getElementById(this.ListControlId).style.height = itemsHeight + "px";
                        }
                        else {
                            myDropDownControl.style.height = itemsHeight + "px";
                            document.getElementById(this.ListControlId).style.height = "100%";
                        }
                    }
                    if (emitClicked) {
                        this.ItemsButtonClicked = true;
                        this.ComboBoxDropDownClicked.emit();
                    }
                    myDropDownControl.style.visibility = "visible";
                    this.RunPositionTimer();
                }
            }
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], ComboBox.prototype, "SelectedItemChanged", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], ComboBox.prototype, "ComboBoxDropDownClicked", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], ComboBox.prototype, "LostFocus", void 0);
    ComboBox = __decorate([
        core_1.Component({
            selector: 'ComboBox',
            moduleId: module.id,
            templateUrl: './ComboBox.html',
            inputs: ['ItemsSource', 'SelectedItem', 'Binding', 'IsDisabled', 'WaterMark', 'IsBlueBox', 'IsGreenButton', 'FocusOnMe', 'SelectedValue', 'SelectedValuePath', 'MaxHeight'],
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], ComboBox);
    return ComboBox;
}());
exports.ComboBox = ComboBox;
//# sourceMappingURL=ComboBox.js.map