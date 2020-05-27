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
var FollowUpDatePipe = /** @class */ (function () {
    function FollowUpDatePipe() {
    }
    FollowUpDatePipe.prototype.transform = function (value, parameter) {
        var myResult = "";
        if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
            var todayDate = Tools_1.DateTool.GetCurrentDateAsUtc();
            if (parameter != null) {
                if (parameter == "F") {
                    myResult = "black";
                    if (value != null) {
                        if (value.valueOf() < todayDate.valueOf()) {
                            myResult = "red";
                        }
                    }
                }
            }
            else {
                myResult = "white";
                var myDate = Tools_1.DateTool.GetDateFromDate(value, true);
                if (value != null) {
                    if (myDate.valueOf() < todayDate.valueOf()) {
                        myResult = "#F7E3E3";
                    }
                }
            }
        }
        return myResult;
    };
    FollowUpDatePipe = __decorate([
        core_1.Pipe({ name: 'FollowUpDatePipe' })
    ], FollowUpDatePipe);
    return FollowUpDatePipe;
}());
exports.FollowUpDatePipe = FollowUpDatePipe;
//# sourceMappingURL=FollowUpDatePipe.js.map