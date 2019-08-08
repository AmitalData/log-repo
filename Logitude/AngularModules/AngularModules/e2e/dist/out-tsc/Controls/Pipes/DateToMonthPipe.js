"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var Tools_1 = require("../../Infrastructure/Tools");
var DateToMonthPipe = /** @class */ (function () {
    function DateToMonthPipe() {
    }
    DateToMonthPipe.prototype.transform = function (value) {
        var myResult = "";
        var month = null;
        var year = null;
        if (value) {
            if (typeof (value) == "string") {
                var chars = value.split("-");
                month = chars[1];
                year = chars[0];
            }
            else {
                month = value.getUTCMonth();
                year = value.getFullYear();
            }
        }
        myResult = month + "." + year;
        return myResult;
    };
    DateToMonthPipe.Pipe = function (value) {
        var myResult = null;
        if (value) {
            myResult = Tools_1.DateTool.GetDateFromDate(value);
        }
        return myResult;
    };
    DateToMonthPipe = __decorate([
        core_1.Pipe({ name: 'DateToMonthPipe' })
    ], DateToMonthPipe);
    return DateToMonthPipe;
}());
exports.DateToMonthPipe = DateToMonthPipe;
//# sourceMappingURL=DateToMonthPipe.js.map