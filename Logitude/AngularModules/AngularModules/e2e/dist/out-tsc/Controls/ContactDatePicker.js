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
var ContactDatePicker = /** @class */ (function () {
    function ContactDatePicker() {
        this.DateChanged = new core_1.EventEmitter();
        this.date = null;
        this.day = null;
        this.month = null;
        this.year = null;
        this.Days = [];
        this.Months = [];
        this.Years = [];
        for (var i = 1; i <= 12; i++) {
            this.Months.push(i);
        }
        for (var i = new Date().getFullYear(); i >= 1950; i--) {
            this.Years.push(i);
        }
    }
    ContactDatePicker.prototype.ngOnInit = function () {
        if (this.Date != null) {
            var myDate = new Date(this.Date.toString());
            this.year = myDate.getFullYear();
            this.month = myDate.getMonth() + 1;
            this.FillDays();
            this.day = myDate.getDate();
        }
        else {
            this.FillDays();
        }
    };
    Object.defineProperty(ContactDatePicker.prototype, "Date", {
        get: function () { return this.date; },
        set: function (newValue) {
            if (this.date != newValue) {
                this.date = newValue;
                this.DateChanged.emit(this.date);
                if (!this.date) {
                    this.year = null;
                    this.month = null;
                    this.day = null;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactDatePicker.prototype, "Day", {
        get: function () { return this.day; },
        set: function (newValue) {
            if (this.day != newValue) {
                this.day = newValue;
                this.BuildDateObject();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactDatePicker.prototype, "Month", {
        get: function () { return this.month; },
        set: function (newValue) {
            if (this.month != newValue) {
                this.month = newValue;
                this.FillDays();
                this.BuildDateObject();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ContactDatePicker.prototype, "Year", {
        get: function () { return this.year; },
        set: function (newValue) {
            if (this.year != newValue) {
                this.year = newValue;
                this.FillDays();
                this.BuildDateObject();
            }
        },
        enumerable: true,
        configurable: true
    });
    ContactDatePicker.prototype.FillDays = function () {
        this.Days = [];
        var myMonth = this.Month == null ? new Date().getMonth() + 1 : this.Month;
        var myYear = this.Year == null ? new Date().getFullYear() : this.Year;
        var days = new Date(myYear, myMonth, 0).getDate();
        for (var i = 1; i <= days; i++) {
            this.Days.push(i);
        }
    };
    ContactDatePicker.prototype.BuildDateObject = function () {
        if (this.Day == null || this.Month == null || this.Year == null) {
            this.Date = null;
        }
        else {
            this.Date = new Date();
            this.Date.setUTCFullYear(this.Year);
            this.Date.setUTCMonth(this.Month - 1);
            this.Date.setUTCDate(this.Day);
            this.Date.setUTCHours(0);
            this.Date.setUTCMinutes(0);
            this.Date.setUTCSeconds(0);
            this.Date.setUTCMilliseconds(0);
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", core_1.EventEmitter)
    ], ContactDatePicker.prototype, "DateChanged", void 0);
    ContactDatePicker = __decorate([
        core_1.Component({
            selector: 'ContactDatePicker',
            moduleId: module.id,
            templateUrl: './ContactDatePicker.html',
            inputs: ['Date'],
        }),
        __metadata("design:paramtypes", [])
    ], ContactDatePicker);
    return ContactDatePicker;
}());
exports.ContactDatePicker = ContactDatePicker;
//# sourceMappingURL=ContactDatePicker.js.map