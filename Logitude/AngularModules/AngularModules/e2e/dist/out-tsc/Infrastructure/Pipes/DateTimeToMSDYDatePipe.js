"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var DateAgeHelper_1 = require("../Utilities/DateAgeHelper");
var Tools_1 = require("../Tools");
var DateTimeToMSDYDatePipe = /** @class */ (function () {
    function DateTimeToMSDYDatePipe() {
    }
    DateTimeToMSDYDatePipe.prototype.transform = function (value) {
        var myResult = "";
        if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
            var helper = new DateAgeHelper_1.DateAgeHelper(value);
            if (helper) {
                if (helper.Years > 0) {
                    myResult = helper.Years + " y";
                }
                else if (helper.Months > 0) {
                    myResult = Math.floor(Number(helper.Days / 7)) + "w";
                    // myResult = helper.Months + " m ago";
                }
                else if (helper.Week > 0) {
                    myResult = helper.Week + " w";
                }
                else if (helper.Days > 0) {
                    myResult = helper.Days + " d";
                }
                else if (helper.Hours > 0) {
                    myResult = helper.Hours + " h";
                }
                else if (helper.Minutes > 0) {
                    myResult = helper.Minutes + " m";
                }
                else if (helper.Seconds > 0) {
                    myResult = helper.Seconds + " s";
                }
                else {
                    myResult = "1 s ";
                }
            }
        }
        return myResult;
    };
    DateTimeToMSDYDatePipe = __decorate([
        core_1.Pipe({ name: 'DateTimeToMSDYDatePipe' })
    ], DateTimeToMSDYDatePipe);
    return DateTimeToMSDYDatePipe;
}());
exports.DateTimeToMSDYDatePipe = DateTimeToMSDYDatePipe;
//# sourceMappingURL=DateTimeToMSDYDatePipe.js.map