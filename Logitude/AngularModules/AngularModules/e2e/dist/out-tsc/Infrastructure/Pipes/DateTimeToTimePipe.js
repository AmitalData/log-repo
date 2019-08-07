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
var DateTimeToTimePipe = /** @class */ (function () {
    function DateTimeToTimePipe() {
    }
    DateTimeToTimePipe.prototype.transform = function (value, myFormat) {
        if (myFormat === void 0) { myFormat = null; }
        var myResult = "";
        if (value) {
            switch (myFormat) {
                case "LT": {
                    myResult = Tools_1.DateTool.GetDateFormats(value).LongTimeString;
                    break;
                }
                case "L12": {
                    myResult = Tools_1.DateTool.GetDateFormats(value).LongTimeString12;
                    break;
                }
                case "S12": {
                    myResult = Tools_1.DateTool.GetDateFormats(value).ShortTimeString12;
                    break;
                }
                default: {
                    myResult = Tools_1.DateTool.GetDateFormats(value).ShortTimeString;
                    break;
                }
            }
        }
        return myResult;
    };
    DateTimeToTimePipe.Pipe = function (value, myFormat) {
        if (myFormat === void 0) { myFormat = null; }
        var myResult = "";
        if (value) {
            switch (myFormat) {
                case "LT": {
                    myResult = Tools_1.DateTool.GetDateFormats(value).LongTimeString;
                    break;
                }
                case "L12": {
                    myResult = Tools_1.DateTool.GetDateFormats(value).LongTimeString12;
                    break;
                }
                case "S12": {
                    myResult = Tools_1.DateTool.GetDateFormats(value).ShortTimeString12;
                    break;
                }
                default: {
                    myResult = Tools_1.DateTool.GetDateFormats(value).ShortTimeString;
                    break;
                }
            }
        }
        return myResult;
    };
    DateTimeToTimePipe = __decorate([
        core_1.Pipe({ name: 'DateTimeToTimePipe' })
    ], DateTimeToTimePipe);
    return DateTimeToTimePipe;
}());
exports.DateTimeToTimePipe = DateTimeToTimePipe;
//# sourceMappingURL=DateTimeToTimePipe.js.map