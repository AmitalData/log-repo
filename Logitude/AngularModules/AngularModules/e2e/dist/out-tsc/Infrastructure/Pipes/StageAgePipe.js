"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var Tools_1 = require("../Tools");
var StageAgePipe = /** @class */ (function () {
    function StageAgePipe() {
    }
    StageAgePipe.prototype.transform = function (value, sizeCode) {
        if (sizeCode === void 0) { sizeCode = null; }
        var myResult = "";
        if (value != null) {
            var myDate = new Date(value.toString());
            var helper = new DateAgeHelper(myDate);
            if (helper.Years > 0) {
                if (sizeCode == "1") {
                    myResult = helper.Years == 1 ? helper.Years + "y" : helper.Years + "y";
                }
                else if (sizeCode == "2") {
                    myResult = helper.Years == 1 ? helper.Years + " y" : helper.Years + " y";
                }
                else {
                    myResult = helper.Years == 1 ? helper.Years + " year" : helper.Years + " years";
                }
            }
            else if (helper.Months > 0) {
                if (sizeCode == "1") {
                    myResult = helper.Months == 1 ? helper.Months + "m" : helper.Months + "m";
                }
                else if (sizeCode == "2") {
                    myResult = helper.Months == 1 ? helper.Months + " m" : helper.Months + " m";
                }
                else {
                    myResult = helper.Months == 1 ? helper.Months + " month" : helper.Months + " months";
                }
            }
            else if (helper.Days > 0) {
                if (sizeCode == "1") {
                    myResult = helper.Days == 1 ? helper.Days + "d" : helper.Days + "d";
                    if (myResult == "0d")
                        myResult = "Today";
                }
                else if (sizeCode == "2") {
                    myResult = helper.Days == 1 ? helper.Days + " d" : helper.Days + " d";
                }
                else {
                    myResult = helper.Days == 1 ? helper.Days + " day" : helper.Days + " days";
                }
            }
            else {
                myResult = "Today";
            }
        }
        return myResult;
    };
    StageAgePipe = __decorate([
        core_1.Pipe({ name: 'StageAgePipe' })
    ], StageAgePipe);
    return StageAgePipe;
}());
exports.StageAgePipe = StageAgePipe;
var DateAgeHelper = /** @class */ (function () {
    function DateAgeHelper(myDate) {
        var todayDate = Tools_1.DateTool.GetCurrentDateTimeAsUtc();
        if (myDate != null && todayDate != null) {
            if (myDate != undefined && todayDate != undefined) {
                var d1 = new Date(myDate.toString());
                var d2 = new Date(todayDate.toString());
                var timeDiff = Math.abs(d2.getTime() - d1.getTime());
                var Secndiff = Math.ceil(timeDiff / (1000 * 3600 * 24));
                var Mintdiff = Math.ceil(timeDiff / (1000 * 3600 * 24));
                var Hourdiff = Math.ceil(timeDiff / (1000 * 3600 * 24));
                var Daysdiff = Math.ceil(timeDiff / (1000 * 3600 * 24)) - 1;
                this.Seconds = +Secndiff.toString().split('.')[0];
                this.Minutes = +Mintdiff.toString().split('.')[0];
                this.Hours = +Hourdiff.toString().split('.')[0];
                this.Days = +Daysdiff.toString().split('.')[0];
                this.Months = +(Daysdiff / 31).toString().split('.')[0];
                this.Years = +(Daysdiff / 365).toString().split('.')[0];
            }
        }
        //this.Seconds = myTimeSpan.Seconds;
        //this.Minutes = myTimeSpan.Minutes;
        //this.Hours = myTimeSpan.Hours;
        //this.Days = myTimeSpan.Days;
        //this.Months = myTimeSpan.Days / 31;
        //this.Years = myTimeSpan.Days / 365; 
    }
    return DateAgeHelper;
}());
exports.DateAgeHelper = DateAgeHelper;
//# sourceMappingURL=StageAgePipe.js.map