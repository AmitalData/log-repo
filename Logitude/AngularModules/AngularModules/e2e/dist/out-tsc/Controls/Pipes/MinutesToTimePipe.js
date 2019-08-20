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
var MinutesToTimePipe = /** @class */ (function () {
    function MinutesToTimePipe() {
    }
    MinutesToTimePipe.prototype.transform = function (value) {
        var myResult = "";
        if (!Tools_1.AppTool.IsNullOrZero(value)) {
            if (Tools_1.FormatTool.IsNumeric(value + "")) {
                var num = value;
                var hours = (num / 60);
                var rhours = Math.floor(hours);
                var minutes = (hours - rhours) * 60;
                var rminutes = Math.round(minutes);
                myResult = Tools_1.AppTool.PadLeft(rhours + "", 2, "0") + ":" + Tools_1.AppTool.PadLeft(rminutes + "", 2, "0");
            }
        }
        return myResult;
    };
    MinutesToTimePipe = __decorate([
        core_1.Pipe({ name: 'MinutesToTimePipe' })
    ], MinutesToTimePipe);
    return MinutesToTimePipe;
}());
exports.MinutesToTimePipe = MinutesToTimePipe;
//# sourceMappingURL=MinutesToTimePipe.js.map