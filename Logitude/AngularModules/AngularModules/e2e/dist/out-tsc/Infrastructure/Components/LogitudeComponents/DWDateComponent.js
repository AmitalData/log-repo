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
var BaseComponent_1 = require("./BaseComponent");
var Tools_1 = require("../../Tools");
var DateAgeHelper_1 = require("../../../Infrastructure/Utilities/DateAgeHelper");
var DWDateComponent = /** @class */ (function (_super) {
    __extends(DWDateComponent, _super);
    function DWDateComponent() {
        var _this = _super.call(this) || this;
        _this.DateAgeHelper = new DateAgeHelper_1.DateAgeHelper(null);
        _this.IsLoad = false;
        _this.ValueChanged = new core_1.EventEmitter();
        _this.SelectedRange = "Day";
        _this.IntervalValue = 1;
        _this.ShowRange = false;
        _this.ShowInterval = false;
        _this.ShowLogDatePicker = false;
        _this.IsFirstTime = true;
        return _this;
    }
    DWDateComponent.prototype.ngOnInit = function () {
        this.FillListRange();
        this.ShowControl();
        this.InitializeComponent();
    };
    Object.defineProperty(DWDateComponent.prototype, "Operation", {
        get: function () {
            return this.operation;
        },
        set: function (newValue) {
            if (this.operation != newValue) {
                this.operation = newValue;
                if (!this.IsFirstTime) {
                    this.DataContext.TextValue = "";
                    this.SetDefultValue(this.operation, newValue);
                    this.ShowControl();
                    this.SetValue();
                }
                this.IsFirstTime = false;
            }
        },
        enumerable: true,
        configurable: true
    });
    DWDateComponent.prototype.ShowControl = function () {
        this.ShowLogDatePicker = false;
        this.ShowRange = false;
        this.ShowInterval = false;
        if (this.Operation == "Before" || this.Operation == "After") {
            this.ShowLogDatePicker = true;
        }
        else if (this.Operation == "Previous" || this.Operation == "Next" || this.Operation == "Current") {
            this.ShowRange = true;
            if (this.Operation != "Current") {
                this.ShowInterval = true;
            }
        }
    };
    DWDateComponent.prototype.FillListRange = function () {
        this.RangeLists = [];
        this.RangeLists.push("Day");
        this.RangeLists.push("Week");
        this.RangeLists.push("Month");
        this.RangeLists.push("Quarter");
        this.RangeLists.push("Year");
    };
    DWDateComponent.prototype.SelectedRangeChanged = function (value) {
        this.SelectedRange = value;
        this.SetValue();
    };
    DWDateComponent.prototype.DatePickerValueChange = function (value) {
        if (this.IsDateValueChange(value, this.DateValue)) {
            this.DateValue = value;
            this.SetValue();
        }
    };
    DWDateComponent.prototype.DatePickerBetweenValue1Change = function (value) {
        if (this.IsDateValueChange(value, this.BetweenDateValue1)) {
            this.BetweenDateValue1 = value;
            this.SetValue();
        }
    };
    DWDateComponent.prototype.DatePickerBetweenValue2Change = function (value) {
        if (this.IsDateValueChange(value, this.BetweenDateValue2)) {
            this.BetweenDateValue2 = value;
            this.SetValue();
        }
    };
    DWDateComponent.prototype.IsDateValueChange = function (value1, value2) {
        var isChange = false;
        var valueA = value1 != null ? value1.toString() : "";
        var valueB = value2 != null ? value2.toString() : "";
        if (valueA != valueB) {
            isChange = true;
        }
        return isChange;
    };
    DWDateComponent.prototype.SelectedIntervalChanged = function (value) {
        if (value != this.IntervalValue) {
            this.IntervalValue = value;
            this.SetValue();
        }
    };
    DWDateComponent.prototype.SetDefultValue = function (operation, newoperation) {
        if ((operation == "Previous" && newoperation != "Next") || (operation == "Next" && newoperation != "Previous") || (operation == "Current" && newoperation != "Current")) {
            this.IntervalValue = 1;
            this.SelectedRange = "Day";
        }
    };
    DWDateComponent.prototype.SetValue = function () {
        var selectedValue = "";
        if (this.Operation == "Before" || this.Operation == "After") {
            if (this.DateValue) {
                var myFormats = Tools_1.DateTool.GetDateFormats(this.DateValue);
                if (myFormats) {
                    selectedValue = this.GetDateFormats(myFormats);
                }
            }
        }
        else if (this.Operation == "Previous" || this.Operation == "Next") {
            selectedValue = this.Operation;
            selectedValue += "^";
            selectedValue += (!Tools_1.AppTool.IsNullOrEmpty(this.IntervalValue) ? this.IntervalValue : 0);
            selectedValue += "^";
            selectedValue += (!Tools_1.AppTool.IsNullOrEmpty(this.SelectedRange) ? this.SelectedRange : "");
        }
        else if (this.Operation == "Current") {
            selectedValue = this.Operation;
            selectedValue += "^";
            selectedValue += (!Tools_1.AppTool.IsNullOrEmpty(this.SelectedRange) ? this.SelectedRange : "");
        }
        else if (this.Operation == "Between") {
            if (this.BetweenDateValue1) {
                var myFormats = Tools_1.DateTool.GetDateFormats(this.BetweenDateValue1);
                if (myFormats) {
                    selectedValue = this.GetDateFormats(myFormats);
                }
            }
            if (this.BetweenDateValue2) {
                var myFormats = Tools_1.DateTool.GetDateFormats(this.BetweenDateValue2);
                if (myFormats) {
                    selectedValue += ("^" + this.GetDateFormats(myFormats));
                }
            }
        }
        else if (this.Operation == "Between") {
            if (this.BetweenDateValue1) {
                var myFormats = Tools_1.DateTool.GetDateFormats(this.BetweenDateValue1);
                if (myFormats) {
                    selectedValue = this.GetDateFormats(myFormats);
                }
            }
        }
        if (this.DataContext.TextValue != selectedValue) {
            this.ValueChanged.emit(selectedValue);
        }
    };
    DWDateComponent.prototype.InitializeComponent = function () {
        if (this.SelectedValue) {
            if (this.Operation == "Before" || this.Operation == "After") {
                this.DateValue = this.ConvertDateToString(this.SelectedValue);
            }
            else if (this.Operation == "Previous" || this.Operation == "Next") {
                var values = this.SelectedValue.toString().split('^');
                if (values.length > 1)
                    this.IntervalValue = Number(values[1]);
                if (values.length > 2)
                    this.SelectedRange = values[2];
            }
            else if (this.Operation == "Current") {
                var values = this.SelectedValue.toString().split('^');
                if (values.length > 1)
                    this.SelectedRange = values[1];
            }
            else if (this.Operation == "Between") {
                var dateBetweenValues = this.SelectedValue.toString().split('^');
                if (dateBetweenValues[0])
                    this.BetweenDateValue1 = this.ConvertDateToString(dateBetweenValues[0]);
                if (dateBetweenValues[1])
                    this.BetweenDateValue2 = this.ConvertDateToString(dateBetweenValues[1]);
            }
        }
        this.IsLoad = true;
    };
    DWDateComponent.prototype.ConvertDateToString = function (value) {
        var date = new Date(value);
        var year = date.getUTCFullYear();
        var month = date.getUTCMonth() + 1;
        var day = date.getUTCDate() + 1;
        var dateString = month + "/" + day + "/" + year;
        return new Date(dateString);
    };
    DWDateComponent.prototype.GetDateFormats = function (myFormats) {
        var result = "";
        if (myFormats) {
            var myDateParts = myFormats.DateParts;
            var stringOfYear = Tools_1.AppTool.PadLeft("" + myDateParts.Year, 4, '0');
            var stringOfMonth = Tools_1.AppTool.PadLeft("" + myDateParts.Month, 2, '0');
            var stringOfDay = Tools_1.AppTool.PadLeft("" + myDateParts.Day, 2, '0');
            result = stringOfYear + "-" + stringOfMonth + "-" + stringOfDay;
        }
        return result;
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], DWDateComponent.prototype, "ValueChanged", void 0);
    DWDateComponent = __decorate([
        core_1.Component({
            selector: 'DWDate',
            moduleId: module.id,
            templateUrl: './DWDateComponent.html',
            inputs: ['DataContext', 'Operation', 'ObjectFieldName', 'SelectedValue'],
        }),
        __metadata("design:paramtypes", [])
    ], DWDateComponent);
    return DWDateComponent;
}(BaseComponent_1.BaseComponent));
exports.DWDateComponent = DWDateComponent;
//# sourceMappingURL=DWDateComponent.js.map