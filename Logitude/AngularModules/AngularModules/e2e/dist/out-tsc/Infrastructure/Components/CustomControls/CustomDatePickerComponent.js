"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
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
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var Tools_1 = require("../../../Infrastructure/Tools");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var forms_1 = require("@angular/forms");
var LogitudeWindow_1 = require("../../../Controls/Windows/LogitudeWindow");
var CustomDatePickerComponent = /** @class */ (function (_super) {
    __extends(CustomDatePickerComponent, _super);
    function CustomDatePickerComponent(fb) {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.FromDate = null;
        _this.ToDate = null;
        _this.Text = null;
        _this.WaterMark = "dd/mm/yy-dd/mm/yy";
        _this.ControlId = null;
        _this.DropdownId = null;
        _this.ListControlId = null;
        // public DateId: string = null;
        _this.MinHeight = 30;
        _this.MaxHeight = 250;
        _this.PublicDate = new Date();
        _this.CloseMenu = true;
        _this.IsMenuOpened = false;
        _this.NoDateVisibile = true;
        _this.mouseOver = false;
        _this.IsDisabled = false;
        _this.SelectedItemChanged = new core_1.EventEmitter();
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsOpened = false;
        _this.myForm = fb.group({});
        if (_this.CurrentSession == null) {
            _this.ControlId = "ComboBox_-1_-1";
            _this.DropdownId = "Dropdown_-1_-1";
            _this.ListControlId = "List_-1_-1";
        }
        else {
            var idIndex = _this.CurrentSession.GetNewId("ComboBox");
            _this.ControlId = "ComboBox_" + idIndex;
            _this.DropdownId = "Dropdown_" + idIndex;
            _this.ListControlId = "List_" + idIndex;
        }
        document.onmouseup = function (e) {
            if (_this.mouseOver == false) {
                _this.OnLostFocus();
            }
        };
        return _this;
    }
    CustomDatePickerComponent.prototype.ngOnInit = function () {
        var _this = this;
        this.TommorowDate = Tools_1.DateTool.AddDays((new Date()), 1);
        this.TommorowDate.setHours(0, 0, 0, 0);
        this.TodayDate = new Date();
        this.TodayDate.setHours(0, 0, 0, 0);
        this.Today = (this.TodayDate.getDate() < 10 ? "0" : "") + this.TodayDate.getDate() + '-' + (this.TodayDate.getMonth() + 1 < 10 ? "0" : "") + (this.TodayDate.getMonth() + 1) + '-' + this.TodayDate.getFullYear();
        this.YesterdayDate = Tools_1.DateTool.AddDays((new Date()), -1);
        this.YesterdayDate.setHours(0, 0, 0, 0);
        this.Yesterday = (this.YesterdayDate.getDate() < 10 ? "0" : "") + (this.YesterdayDate.getDate()) + '-' + (this.YesterdayDate.getMonth() + 1 < 10 ? "0" : "") + (this.YesterdayDate.getMonth() + 1) + '-' + this.YesterdayDate.getFullYear();
        this.LastSevenDaysDate = Tools_1.DateTool.AddDays((new Date()), -7);
        this.LastSevenDaysDate.setHours(0, 0, 0, 0);
        this.LastSevenDays = (this.LastSevenDaysDate.getDate() < 10 ? "0" : "") + (this.LastSevenDaysDate.getDate()) + '-' + (this.LastSevenDaysDate.getMonth() + 1 < 10 ? "0" : "") + (this.LastSevenDaysDate.getMonth() + 1) + '-' + this.LastSevenDaysDate.getFullYear() + " - " + this.Today;
        this.LastThirtyDaysDate = Tools_1.DateTool.AddDays((new Date()), -30);
        this.LastThirtyDaysDate.setHours(0, 0, 0, 0);
        this.LastThirtyDays = (this.LastThirtyDaysDate.getDate() < 10 ? "0" : "") + this.LastThirtyDaysDate.getDate() + '-' + (this.LastThirtyDaysDate.getMonth() + 1 < 10 ? "0" : "") + (this.LastThirtyDaysDate.getMonth() + 1) + '-' + this.LastThirtyDaysDate.getFullYear() + " - " + this.Today;
        this.CurrentYearFromDate = new Date(new Date().getFullYear(), 0, 1);
        this.CurrentYearFromDate.setHours(0, 0, 0, 0);
        this.CurrentYearToDate = Tools_1.DateTool.AddDays((new Date()), 1);
        this.CurrentYearToDate.setHours(0, 0, 0, 0);
        this.CurrentYear = '01-01-' + ((new Date()).getFullYear()) + ' - ' + 'Today'; //(this.CurrentYearToDate.getDate() < 10 ? "0" : "") + (this.CurrentYearToDate.getDate()) + '-' + (this.CurrentYearToDate.getMonth() + 1 < 10 ? "0" : "") + (this.CurrentYearToDate.getMonth() + 1) + '-' + (this.CurrentYearToDate.getFullYear());
        //this.LastYearFromDate = DateTool.AddDays((new Date()), -365);
        //this.LastYearFromDate.setHours(0, 0, 0, 0);
        //this.LastYearToDate = DateTool.AddDays((new Date()), 1);
        //this.LastYearToDate.setHours(0, 0, 0, 0);
        //this.LastYear = (this.LastYearFromDate.getDate() < 10 ? "0" : "") + (this.LastYearFromDate.getDate() - 1) + '-' + (this.LastYearFromDate.getMonth() + 1 < 10 ? "0" : "") + (this.LastYearFromDate.getMonth() + 1) + '-' + this.LastYearFromDate.getFullYear() + ' - ' + (this.LastYearFromDate.getDate() < 10 ? "0" : "") + (this.LastYearToDate.getDate() - 1) + '-' + (this.LastYearFromDate.getMonth() + 1 < 10 ? "0" : "") + (this.LastYearToDate.getMonth() + 1) + '-' + (this.LastYearToDate.getFullYear());
        this.LastYearFromDate = Tools_1.DateTool.AddDays(Tools_1.DateTool.GetCurrentDateAsUtc(), -365);
        this.LastYearToDate = Tools_1.DateTool.AddDays(Tools_1.DateTool.GetCurrentDateAsUtc(), 1);
        var lastYearDateParts1 = Tools_1.DateTool.GetDateParts(this.LastYearFromDate);
        var lastYearDateParts2 = Tools_1.DateTool.GetDateParts(this.LastYearToDate);
        this.LastYear = "";
        this.LastYear += Tools_1.AppTool.PadLeft(lastYearDateParts1.Day + "", 2, "0") + '-' + Tools_1.AppTool.PadLeft(lastYearDateParts1.Month + "", 2, "0") + '-' + lastYearDateParts1.Year;
        this.LastYear += " - ";
        this.LastYear += Tools_1.AppTool.PadLeft(lastYearDateParts2.Day + "", 2, "0") + '-' + Tools_1.AppTool.PadLeft(lastYearDateParts2.Month + "", 2, "0") + '-' + lastYearDateParts2.Year;
        var predefinedFilter = window.PreDefinedFilters.filter(function (d) { return d.ObjectFieldId == _this.ObjectField.Id && d.QueryId == _this.QueryId; })[0];
        if (predefinedFilter != null && (this.ObjectField.DataTypeCode == "DateTime" || this.ObjectField.DataTypeCode == "Date")) {
            this.SelectedItem = predefinedFilter.PredefinedValue;
            if (this.SelectedItem == "NoDate") {
                this.Text = "No Date";
            }
            if (predefinedFilter.Operator == "LargerThan") {
                var myDate = Tools_1.DateTool.GetDateParts(predefinedFilter.PredefinedValue);
                var stringDate = (myDate.Day < 10 ? "0" : "") + myDate.Day + '-' + (myDate.Month < 10 ? "0" : "") + myDate.Month + '-' + myDate.Year;
                this.SelectedItem = "Greater Than";
                this.Text = "Greater Than " + stringDate;
                this.GreaterTextValue;
            }
            if (predefinedFilter.Operator == "LessThan") {
                var myDate = Tools_1.DateTool.GetDateParts(predefinedFilter.PredefinedValue);
                var stringDate = (myDate.Day < 10 ? "0" : "") + myDate.Day + '-' + (myDate.Month < 10 ? "0" : "") + myDate.Month + '-' + myDate.Year;
                this.SelectedItem = "Less Than";
                this.Text = "Less Than " + stringDate;
            }
            if (predefinedFilter.Operator == "Between") {
                var myDate = Tools_1.DateTool.GetDateParts(predefinedFilter.PredefinedValue);
                var myDate1 = Tools_1.DateTool.GetDateParts(predefinedFilter.PredefinedValue2);
                var stringDate = (myDate.Day < 10 ? "0" : "") + myDate.Day + '-' + (myDate.Month < 10 ? "0" : "") + myDate.Month + '-' + myDate.Year;
                var endingDate = (myDate1.Day < 10 ? "0" : "") + myDate1.Day + '-' + (myDate1.Month < 10 ? "0" : "") + myDate1.Month + '-' + myDate1.Year;
                this.SelectedItem = stringDate + " - " + endingDate;
                this.Text = stringDate + " - " + endingDate;
            }
            //Between
        }
        if (this.SelectedItem != null) {
            this.SetDisplayText();
        }
        if (this.ObjectField.IsRequiered == true) {
            this.NoDateVisibile = false;
        }
    };
    CustomDatePickerComponent.prototype.mousedown = function () {
        console.log("mousedown" + this.CloseMenu);
        if (this.mouseOver == false) {
            this.CloseMenu = true;
            this.IsMenuOpened = false;
            this.OnLostFocus();
        }
        else {
            this.IsMenuOpened = true;
            var item = document.getElementById(this.ControlId);
            if (item != null) {
                this.SetControlPosition();
                document.getElementById(this.DropdownId).style.width = item.offsetWidth + 50 + "px";
                var itemsHeight = ((10 * 23) + 3);
                if (this.ObjectField.IsRequiered == true) {
                    itemsHeight = ((9 * 23) + 3);
                }
                if (itemsHeight > this.MaxHeight) {
                    document.getElementById(this.DropdownId).style.height = this.MaxHeight + "px";
                    document.getElementById(this.ListControlId).style.height = itemsHeight + "px";
                }
                else {
                    document.getElementById(this.DropdownId).style.height = itemsHeight + "px";
                    //document.getElementById(this.DropdownId).style.minHeight = "350px";
                    document.getElementById(this.ListControlId).style.height = "100%";
                }
                document.getElementById(this.DropdownId).style.visibility = "visible";
            }
        }
    };
    CustomDatePickerComponent.prototype.StopPositionTimer = function () {
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
    };
    CustomDatePickerComponent.prototype.RunPositionTimer = function () {
        var _this = this;
        this.StopPositionTimer();
        this.timerToken = setInterval(function () { return _this.SetControlPosition(); }, 1);
    };
    CustomDatePickerComponent.prototype.SetControlPosition = function () {
        var item = document.getElementById(this.ControlId);
        if (item != null) {
            var itemRect = item.getBoundingClientRect();
            document.getElementById(this.DropdownId).style.position = "fixed";
            document.getElementById(this.DropdownId).style.top = (itemRect.top + 22) + 'px';
            document.getElementById(this.DropdownId).style.left = itemRect.left + 'px';
        }
    };
    CustomDatePickerComponent.prototype.OnFocus = function () {
        this.RunPositionTimer();
    };
    CustomDatePickerComponent.prototype.OnLostFocus = function () {
        if (this.mouseOver == false) {
            this.StopPositionTimer();
            if (document.getElementById(this.DropdownId)) {
                document.getElementById(this.DropdownId).style.height = "0px";
                document.getElementById(this.DropdownId).style.visibility = "hidden";
            }
        }
    };
    CustomDatePickerComponent.prototype.ComboBoxClicked = function () {
        var item = document.getElementById(this.ControlId);
        if (item != null) {
            this.IsOpened = !this.IsOpened;
            if (!this.IsOpened) {
                item.blur();
            }
        }
    };
    CustomDatePickerComponent.prototype.ItemClicked = function (clickedItem) {
        //if (clickedItem == "Less Than" || clickedItem == "Greater Than") {
        //    this.CloseMenu = false;
        //}
        if (clickedItem != null) {
            if (this.SelectedItem != clickedItem) {
                this.SelectedItem = clickedItem;
                this.mouseOver = false;
                this.GreaterTextValue = null;
                this.LesstextValue = null;
                this.IsMenuOpened = false;
                this.StopPositionTimer();
                this.OnLostFocus();
                //var myComboBox = document.getElementById(this.ControlId);
                //if (myComboBox != null) {
                //    myComboBox.blur();
                //}
                //this.SetDisplayText();
                //this.SelectedItemChanged.emit(this.SelectedItem);
            }
        }
    };
    CustomDatePickerComponent.prototype.SetDisplayText = function () {
        var myDisplayText = null;
        if (this.SelectedItem != null && this.SelectedItem != "Choose") {
            myDisplayText = this.Text;
        }
        this.Text = myDisplayText;
    };
    Object.defineProperty(CustomDatePickerComponent.prototype, "SelectedItem", {
        get: function () { return this.selectedItem; },
        set: function (newValue) {
            this.selectedItem = newValue;
            if (newValue != null) {
                switch (newValue) {
                    case "Today":
                        {
                            this.Text = "Today " + this.Today;
                            //this.SetDisplayText();
                            this.SelectedItemChanged.emit({ FromDate: this.TodayDate, ToDate: this.TommorowDate, Operation: "Equals", MyName: "Today" });
                            break;
                        }
                    case "Yesterday":
                        {
                            this.Text = "Yesterday " + this.Yesterday;
                            //this.SetDisplayText();
                            this.SelectedItemChanged.emit({ FromDate: this.YesterdayDate, ToDate: this.TodayDate, Operation: "Equals", MyName: "Yesterday" });
                            break;
                        }
                    case "Last 7 Days":
                        {
                            this.Text = "Last 7 Days " + this.LastSevenDays;
                            //this.SetDisplayText();
                            this.SelectedItemChanged.emit({ FromDate: this.LastSevenDaysDate, ToDate: this.TommorowDate, Operation: "Equals", MyName: "Last 7 Days" });
                            break;
                        }
                    case "Last 30 Days":
                        {
                            this.Text = "Last 30 Days " + this.LastThirtyDays;
                            //this.SetDisplayText();
                            this.SelectedItemChanged.emit({ FromDate: this.LastThirtyDaysDate, ToDate: this.TommorowDate, Operation: "Equals", MyName: "Last 30 Days" });
                            break;
                        }
                    case "Current Year":
                        {
                            this.Text = "Current Year " + this.CurrentYear;
                            //this.SetDisplayText();
                            this.SelectedItemChanged.emit({ FromDate: this.CurrentYearFromDate, ToDate: this.CurrentYearToDate, Operation: "Equals", MyName: "Current Year" });
                            break;
                        }
                    case "Last Year":
                        {
                            this.Text = "Last Year " + this.LastYear;
                            //this.SetDisplayText();
                            this.SelectedItemChanged.emit({ FromDate: this.LastYearFromDate, ToDate: this.LastYearToDate, Operation: "Equals", MyName: "Last Year" });
                            break;
                        }
                    case "No Date":
                        {
                            this.Text = "No Date";
                            //this.SetDisplayText();
                            this.SelectedItemChanged.emit("NoDate");
                            break;
                        }
                    case "Greater Than":
                        {
                            //this.SelectedItemChanged.emit(this.LastSevenDays);
                            break;
                        }
                    case "Less Than":
                        {
                            //this.SelectedItemChanged.emit(this.LastSevenDays);
                            break;
                        }
                    default: {
                        break;
                    }
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomDatePickerComponent.prototype, "GreaterTextValue", {
        get: function () { return this.greatertextValue; },
        set: function (newValue) {
            this.greatertextValue = newValue;
            if (newValue != null) {
                this.lesstextValue = null;
                this.Text = "Greater Than" + " " + newValue.getDate() + '/' + (newValue.getMonth() + 1) + '/' + newValue.getFullYear();
                this.SelectedItem = "Greater Than";
                this.SelectedItemChanged.emit({ Date: newValue, Operation: "LargerThan" });
                this.CloseMenu = true;
                this.mouseOver = false;
                this.OnLostFocus();
            }
            else {
                if (this.LesstextValue == null && this.SelectedItem == "Greater Than") {
                    this.Text = null;
                    this.SelectedItem = "Greater Than";
                    this.SelectedItemChanged.emit({ Date: "", Operation: "LargerThan" });
                    this.CloseMenu = true;
                    this.mouseOver = false;
                    this.OnLostFocus();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CustomDatePickerComponent.prototype, "LesstextValue", {
        get: function () { return this.lesstextValue; },
        set: function (newValue) {
            this.lesstextValue = newValue;
            if (newValue != null) {
                this.greatertextValue = null;
                this.Text = "Less Than" + " " + newValue.getDate() + '/' + (newValue.getMonth() + 1) + '/' + newValue.getFullYear();
                this.SelectedItem = "Less Than";
                this.SelectedItemChanged.emit({ Date: newValue, Operation: "LessThan" });
                this.CloseMenu = true;
                this.mouseOver = false;
                this.OnLostFocus();
            }
            else {
                if (this.GreaterTextValue == null && this.SelectedItem == "Less Than") {
                    this.Text = null;
                    this.SelectedItem = "Less Than";
                    this.SelectedItemChanged.emit({ Date: "", Operation: "LessThan" });
                    this.CloseMenu = true;
                    this.mouseOver = false;
                    this.OnLostFocus();
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    CustomDatePickerComponent.prototype.ChooseDatesClicked = function () {
        var _this = this;
        this.GreaterTextValue = null;
        this.LesstextValue = null;
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 408;
        logitudeWindow.Height = 330;
        logitudeWindow.Title = "Choose Dates";
        logitudeWindow.Show('./Infrastructure/Components/CustomControls/ChooseDatesComponent');
        this.mouseOver = false;
        logitudeWindow.ComponentLoaded.subscribe(function (cmp) {
            cmp.DateSelected.subscribe(function (res) {
                _this.FromDate = res.From;
                _this.FromDate.setHours(0, 0, 0, 0);
                _this.ToDate = res.To;
                _this.ToDate.setHours(23, 59, 59, 999);
                _this.SelectedItemChanged.emit({ FromDate: _this.FromDate, ToDate: _this.ToDate, Operation: "Between" });
                _this.SelectedItem = _this.FromDate.getDate() + '/' + (_this.FromDate.getMonth() + 1) + '/' + _this.FromDate.getFullYear() + "-" + _this.ToDate.getDate() + '/' + (_this.ToDate.getMonth() + 1) + '/' + _this.ToDate.getFullYear();
                _this.Text = _this.FromDate.getDate() + '/' + (_this.FromDate.getMonth() + 1) + '/' + _this.FromDate.getFullYear() + "-" + _this.ToDate.getDate() + '/' + (_this.ToDate.getMonth() + 1) + '/' + _this.ToDate.getFullYear();
                _this.SetDisplayText();
                //alert(this.FromDate + " - " + this.ToDate);
            });
        });
        this.CloseMenu = true;
        this.OnLostFocus();
    };
    CustomDatePickerComponent.prototype.OnCalendarClick = function () {
        //console.log("OnCalendarClick");
        //this.CloseMenu = false; 
        ////alert("Hi");
        //this.IsMenuOpened = false;
        ////this.mousedown();
        //var DropdownId = this.DropdownId;
        //var ss = document.activeElement;
        //ss.addEventListener("blur", function (event) {
        //    document.getElementById(DropdownId).style.height = "0px";
        //    document.getElementById(DropdownId).style.visibility = "hidden"; 
        //});
    };
    CustomDatePickerComponent.prototype.onMouseOver = function () {
        this.mouseOver = true;
    };
    CustomDatePickerComponent.prototype.onMouseOut = function () {
        this.mouseOver = false;
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], CustomDatePickerComponent.prototype, "SelectedItemChanged", void 0);
    CustomDatePickerComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'CustomDatePicker',
            templateUrl: './CustomDatePickerComponent.html',
            inputs: ['ObjectField', 'QueryId', 'IsDisabled']
        }),
        __metadata("design:paramtypes", [forms_1.FormBuilder])
    ], CustomDatePickerComponent);
    return CustomDatePickerComponent;
}(BaseComponent_1.BaseComponent));
exports.CustomDatePickerComponent = CustomDatePickerComponent;
//# sourceMappingURL=CustomDatePickerComponent.js.map