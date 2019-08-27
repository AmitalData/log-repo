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
var CountryFlagPipe = /** @class */ (function () {
    function CountryFlagPipe() {
    }
    CountryFlagPipe.prototype.transform = function (value) {
        var myResult = "";
        if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
            myResult = "./Images/Flags/" + value + ".png";
        }
        return myResult;
    };
    CountryFlagPipe = __decorate([
        core_1.Pipe({ name: 'CountryFlagPipe' })
    ], CountryFlagPipe);
    return CountryFlagPipe;
}());
exports.CountryFlagPipe = CountryFlagPipe;
//# sourceMappingURL=CountryFlagPipe.js.map