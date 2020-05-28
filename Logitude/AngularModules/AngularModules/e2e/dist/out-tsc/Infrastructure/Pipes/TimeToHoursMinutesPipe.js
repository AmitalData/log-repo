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
var TimeToHoursMinutesPipe = /** @class */ (function () {
    function TimeToHoursMinutesPipe() {
    }
    TimeToHoursMinutesPipe.prototype.transform = function (value) {
        var myResult = "";
        var result = "";
        var val = value;
        var h = val / 60 | 0, m = val % 60 | 0;
        var result = h + ":" + Tools_1.AppTool.PadLeft("" + m, 2, '0');
        if (result == "0:00") {
            myResult = "";
        }
        else {
            myResult = result;
        }
        return myResult;
    };
    TimeToHoursMinutesPipe = __decorate([
        core_1.Pipe({ name: 'TimeToHoursMinutesPipe' })
    ], TimeToHoursMinutesPipe);
    return TimeToHoursMinutesPipe;
}());
exports.TimeToHoursMinutesPipe = TimeToHoursMinutesPipe;
//# sourceMappingURL=TimeToHoursMinutesPipe.js.map