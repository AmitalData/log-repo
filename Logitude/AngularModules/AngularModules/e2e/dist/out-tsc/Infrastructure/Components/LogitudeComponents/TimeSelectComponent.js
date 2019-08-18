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
var TimeSelectComponent = /** @class */ (function () {
    function TimeSelectComponent() {
        this.TimeArray24 = [
            "00:00",
            "01:00",
            "02:00",
            "03:00",
            "04:00",
            "05:00",
            "06:00",
            "07:00",
            "08:00",
            "09:00",
            "10:00",
            "11:00",
            "12:00",
            "13:00",
            "14:00",
            "15:00",
            "16:00",
            "17:00",
            "18:00",
            "19:00",
            "20:00",
            "21:00",
            "22:00",
            "23:00",
        ];
        this.TimeArray12 = [
            "12:00 AM",
            "01:00 AM",
            "02:00 AM",
            "03:00 AM",
            "04:00 AM",
            "05:00 AM",
            "06:00 AM",
            "07:00 AM",
            "08:00 AM",
            "09:00 AM",
            "10:00 AM",
            "11:00 AM",
            "12:00 PM",
            "01:00 PM",
            "02:00 PM",
            "03:00 PM",
            "04:00 PM",
            "05:00 PM",
            "06:00 PM",
            "07:00 PM",
            "08:00 PM",
            "09:00 PM",
            "10:00 PM",
            "11:00 PM",
        ];
        this.OnSelectedTimeChanged = new core_1.EventEmitter();
    }
    TimeSelectComponent.prototype.ngOnInit = function () {
        //console.log("oninit1", this.SelectedTime.toUTCString());
        // if (this.TimeMode == '24') {
        this.TimeItemsSource = this.TimeArray24;
        //}
        // else {
        // this.TimeItemsSource = this.TimeArray12;
        //}
    };
    TimeSelectComponent.prototype.OnTimeClick = function (time) {
        var strArr = time.split(':');
        var hours = Number(strArr[0]);
        var minutes;
        var suffix = null;
        ;
        if (strArr[1].indexOf('A') > -1 || strArr[1].indexOf('P') > -1) {
            var arr = strArr[1].split(' ');
            minutes = Number(arr[0]);
            suffix = arr[1];
        }
        else {
            minutes = Number(strArr[1]);
        }
        var year = 0;
        var month = 0;
        var day = 0;
        if (!this.SelectedTime) {
            var today = new Date();
            year = today.getFullYear();
            month = today.getMonth();
            day = today.getDate();
        }
        else {
            year = this.SelectedTime.getUTCFullYear();
            month = this.SelectedTime.getUTCMonth();
            day = this.SelectedTime.getUTCDate();
        }
        this.SelectedTime = this.GetDate(year, month, day, hours, minutes, 0);
        this.OnSelectedTimeChanged.emit({ SelectedTime: this.SelectedTime, Suffix: suffix });
    };
    TimeSelectComponent.prototype.GetDate = function (year, month, day, hour, minute, second) {
        var date = new Date();
        date.setUTCDate(1);
        date.setUTCFullYear(year);
        date.setUTCMonth(month);
        date.setUTCDate(day);
        date.setUTCHours(hour);
        date.setUTCMinutes(minute);
        date.setUTCSeconds(second);
        date.setUTCMilliseconds(0);
        return date;
    };
    TimeSelectComponent.prototype.ToDate = function (time) {
        var strArr = time.split(':');
        var hours = Number(strArr[0]);
        var minutes;
        var suffix = null;
        ;
        if (strArr[1].indexOf('A') > -1 || strArr[1].indexOf('P') > -1) {
            var arr = strArr[1].split(' ');
            minutes = Number(arr[0]);
            suffix = arr[1];
        }
        else {
            minutes = Number(strArr[1]);
        }
        var year = this.SelectedTime.getFullYear();
        var month = this.SelectedTime.getMonth();
        var day = this.SelectedTime.getDate();
        var date = this.GetDate(year, month, day, hours, minutes, 0).toUTCString();
        return new Date(date);
        //var mydate = new Date(time);
        //console.log(mydate.toDateString());
        //return mydate;
    };
    TimeSelectComponent.prototype.isSelected = function (checktime) {
        //var currentDate:Date = this.ToDate(checktime);
        //var mydate: Date = new Date(this.SelectedTime.toUTCString());
        //if (currentDate.getTime() == mydate.getTime())
        //    return true;
        return false;
    };
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], TimeSelectComponent.prototype, "TimeMode", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Boolean)
    ], TimeSelectComponent.prototype, "ToggleOpened", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], TimeSelectComponent.prototype, "OnSelectedTimeChanged", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Date)
    ], TimeSelectComponent.prototype, "SelectedTime", void 0);
    TimeSelectComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'LogTimePicker',
            templateUrl: './TimeSelectComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], TimeSelectComponent);
    return TimeSelectComponent;
}());
exports.TimeSelectComponent = TimeSelectComponent;
//# sourceMappingURL=TimeSelectComponent.js.map