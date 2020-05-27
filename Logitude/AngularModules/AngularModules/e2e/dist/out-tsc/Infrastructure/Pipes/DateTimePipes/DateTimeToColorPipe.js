"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var Tools_1 = require("../../Tools");
var DateTimeToColorPipe = /** @class */ (function () {
    function DateTimeToColorPipe() {
    }
    DateTimeToColorPipe.prototype.transform = function (value, defaultColor) {
        if (defaultColor === void 0) { defaultColor = null; }
        var myResult = Tools_1.FontTool.Black;
        switch (defaultColor) {
            case "O": {
                myResult = Tools_1.FontTool.Orange;
                break;
            }
            case "A": { // ActivityWork
                myResult = "rgb(110,113,114)";
                break;
            }
            default: {
                myResult = Tools_1.FontTool.Black;
                break;
            }
        }
        if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
            var myDateTicks = Tools_1.DateTool.GetDateParts(value).DateTicks;
            var todayDateTicks = Tools_1.DateTool.GetDateParts(Tools_1.DateTool.GetCurrentDateAsUtc()).DateTicks;
            if (myDateTicks < todayDateTicks) {
                myResult = Tools_1.FontTool.Red;
            }
        }
        return myResult;
    };
    DateTimeToColorPipe = __decorate([
        core_1.Pipe({ name: 'DateTimeToColorPipe' })
    ], DateTimeToColorPipe);
    return DateTimeToColorPipe;
}());
exports.DateTimeToColorPipe = DateTimeToColorPipe;
//# sourceMappingURL=DateTimeToColorPipe.js.map