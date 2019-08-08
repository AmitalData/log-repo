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
/// <reference path="../infrastructure/components/logitudecomponents/basecomponent.ts" />
var core_1 = require("@angular/core");
var SessionLocator_1 = require("../Infrastructure/Utilities/SessionLocator");
var ComboBoxWithInCheckBox = /** @class */ (function () {
    function ComboBoxWithInCheckBox() {
        this.Text = null;
        this.WaterMark = null;
        this.WithinImage = false;
        this.Binding = null;
        this.ControlId = null;
        this.DropdownId = null;
        this.ListControlId = null;
        this.MinHeight = 30;
        this.MaxHeight = 250;
        this.IsBlueBox = false;
        this.IsDisabled = false;
        this.IsOpened = false;
        this.ShowSelected = false;
        this.DataContext = this;
        this.IsAll = true;
        this.IsSelected = false;
        this.RadioFocus = false;
        this.IsMouseOverInput = false;
        this.SelectionType = " Products";
        this.ProductsSelectionType = " Selected " + this.SelectionType;
        this.TotalPickedItems = " All";
        this.CheckBoxOnly = false;
        this.SelectedItemChanged = new core_1.EventEmitter();
        this.EditedItemSource = new core_1.EventEmitter();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsAreasMenu = false;
        this.SearchAreasId = "SearchAreasId";
        this.IsMouseOver = false;
        this.selectedItem = null;
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
        this.SearchAreasId += this.CurrentSession.GetNewId("SearchAreasId_1");
    }
    ComboBoxWithInCheckBox.prototype.ngAfterViewInit = function () {
    };
    ComboBoxWithInCheckBox.prototype.SetControlPosition = function () {
        var item = document.getElementById(this.ControlId);
        if (item != null) {
            var itemRect = item.getBoundingClientRect();
            document.getElementById(this.DropdownId).style.position = "fixed";
            document.getElementById(this.DropdownId).style.top = (itemRect.top) + 'px';
            document.getElementById(this.DropdownId).style.left = itemRect.left + 'px';
        }
    };
    ComboBoxWithInCheckBox.prototype.mousedown = function () {
        var item = document.getElementById(this.ControlId);
        if (item != null) {
            this.SetControlPosition();
            document.getElementById(this.DropdownId).style.width = item.offsetWidth + "px";
            if (this.ItemsSource.length == 0 || this.ItemsSource == null) {
                document.getElementById(this.DropdownId).style.height = this.MinHeight + "px";
            }
            else {
                var itemsHeight = ((this.ItemsSource.length * 23) + 3);
                if (itemsHeight > this.MaxHeight) {
                    document.getElementById(this.DropdownId).style.height = this.MaxHeight + "px";
                    //   document.getElementById(this.ListControlId).style.height = itemsHeight + "px";
                }
                else {
                    document.getElementById(this.DropdownId).style.height = itemsHeight + "px";
                    // document.getElementById(this.ListControlId).style.height = "100%";
                }
            }
            //  document.getElementById(this.DropdownId).style.visibility = "visible";
        }
    };
    ComboBoxWithInCheckBox.prototype.setToggleButtonMenuTemp = function () {
        var ToggleBTN = document.getElementById(this.ControlId);
        ToggleBTN.className = "ToggleButtonMenuTemp";
    };
    ComboBoxWithInCheckBox.prototype.setToggleButtonMenu = function () {
        var ToggleBTN = document.getElementById(this.ControlId);
        ToggleBTN.className = "ToggleButtonMenu";
    };
    ComboBoxWithInCheckBox.prototype.ngOnInit = function () {
        if (this.SelectedItem != null) {
            this.SetDisplayText();
        }
    };
    Object.defineProperty(ComboBoxWithInCheckBox.prototype, "SelectedItem", {
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
    ComboBoxWithInCheckBox.prototype.clickItem = function (item, index) {
        if (this.CheckSource == null) {
            if (this.ItemsSource != null) {
                this.CheckSource = new Array(this.ItemsSource.length);
                for (var i = 0; i < this.CheckSource.length; i++)
                    this.CheckSource[i] = false;
            }
        }
        if (this.CheckSource[index] == false)
            this.CheckSource[index] = true;
        else
            this.CheckSource[index] = false;
        item.Checked = this.CheckSource[index];
        this.TotalPickedItems += "," + item.Name;
        this.ItemsSource[index] = item;
        this.TotalPickedItems = "";
        if (!this.WithinImage) {
            for (var i = 0; i < this.ItemsSource.length; i++) {
                if (this.ItemsSource[i].Checked)
                    this.TotalPickedItems += this.ItemsSource[i].Name + ",";
            }
        }
        else {
            for (var i = 0; i < this.ItemsSource.length; i++) {
                if (this.ItemsSource[i].Checked)
                    this.TotalPickedItems += this.ItemsSource[i].Code + ",";
            }
        }
        this.EditedItemSource.emit(this.ItemsSource);
    };
    ComboBoxWithInCheckBox.prototype.IsChecked = function () {
        return false;
    };
    ComboBoxWithInCheckBox.prototype.ComboBoxClicked = function () {
        var item = document.getElementById(this.ControlId);
        if (item != null) {
            this.IsOpened = !this.IsOpened;
            if (!this.IsOpened) {
                item.blur();
            }
        }
    };
    ComboBoxWithInCheckBox.prototype.isSelectedClicked = function () {
        this.ShowSelected = true;
        this.SelectedItem = "NotAll";
        this.TotalPickedItems = "";
        this.SelectedItemChanged.emit(this.SelectedItem);
        this.IsAll = false;
    };
    ComboBoxWithInCheckBox.prototype.isAllClicked = function () {
        this.ShowSelected = false;
        this.SelectedItem = "All";
        this.TotalPickedItems = "All";
        this.IsAll = true;
        if (this.ItemsSource != null) {
            this.ItemsSource.forEach(function (item) {
                item.Checked = false;
            });
        }
        this.CheckSource = null;
        this.SelectedItemChanged.emit(this.SelectedItem);
    };
    ComboBoxWithInCheckBox.prototype.ItemClicked = function (clickedItem) {
        if (this.CheckSource == null) {
            if (this.ItemsSource != null) {
                this.CheckSource = new Array(this.ItemsSource.length);
                for (var i = 0; i < this.CheckSource.length; i++)
                    this.CheckSource[i] = false;
            }
        }
    };
    ComboBoxWithInCheckBox.prototype.SetDisplayText = function () {
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
    ComboBoxWithInCheckBox.prototype.OnKeyDown = function ($event) {
        if ($event) {
            switch ($event.keyCode) {
                case 8:
                case 46:
                    {
                        this.SelectedItem = null;
                        this.SelectedItemChanged.emit(null);
                        break;
                    }
            }
        }
    };
    ComboBoxWithInCheckBox.prototype.ClearPlaceHolder = function () {
        var temp = document.getElementById(this.SearchAreasId);
        temp.placeholder = "";
        temp.style.background = "rgba(0, 0, 0, 0)";
        //var ToggleBTN = document.getElementById(this.SearchProductDropButtonCustomerId) as HTMLDivElement;
        //ToggleBTN.className = "ToggleButtonMenuTemp";
    };
    ComboBoxWithInCheckBox.prototype.FillPlaceHolder = function () {
        if (!this.SearchText) {
            var temp = document.getElementById(this.SearchAreasId);
            temp.placeholder = "Search";
            temp.style.background = "url(Images/Search.png) no-repeat scroll";
            temp.style.backgroundPosition = "right center";
            temp.style.paddingRight = "30px";
        }
        //var ToggleBTN = document.getElementById(this.SearchProductDropButtonCustomerId) as HTMLDivElement;
        //ToggleBTN.className = "ToggleButtonMenu";
    };
    ComboBoxWithInCheckBox.prototype.OnDeleteValue = function () {
        var temp = document.getElementById(this.SearchAreasId);
        temp.value = null;
        this.SearchText = null;
        temp.focus();
    };
    Object.defineProperty(ComboBoxWithInCheckBox.prototype, "SearchText", {
        get: function () { return this.searchText; },
        set: function (value) {
            if (this.searchText != value) {
                this.searchText = value;
                //this.FillList();
                //this.CD.detectChanges();
            }
        },
        enumerable: true,
        configurable: true
    });
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], ComboBoxWithInCheckBox.prototype, "SelectedItemChanged", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], ComboBoxWithInCheckBox.prototype, "EditedItemSource", void 0);
    ComboBoxWithInCheckBox = __decorate([
        core_1.Component({
            selector: 'ComboBoxWithInCheckBox',
            moduleId: module.id,
            templateUrl: './ComboBoxWithInCheckBox.html',
            inputs: ['ItemsSource', 'SelectedItem', 'Binding', 'IsDisabled', 'WaterMark', 'IsBlueBox', 'WithinImage', 'SelectionType', 'CheckBoxOnly', 'IsAreasMenu'],
        }),
        __metadata("design:paramtypes", [])
    ], ComboBoxWithInCheckBox);
    return ComboBoxWithInCheckBox;
}());
exports.ComboBoxWithInCheckBox = ComboBoxWithInCheckBox;
//# sourceMappingURL=ComboBoxWithInCheckBox.js.map