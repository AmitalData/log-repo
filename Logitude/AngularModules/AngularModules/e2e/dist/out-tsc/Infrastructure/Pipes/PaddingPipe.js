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
var PaddingPipe = /** @class */ (function () {
    function PaddingPipe() {
    }
    PaddingPipe.prototype.transform = function (number, Direction, Count, Character) {
        var myResult = "";
        if (!Tools_1.AppTool.IsNullOrEmpty(number)) {
            myResult = number.toString();
            if (!Tools_1.AppTool.IsNullOrEmpty(myResult)) {
                if (Count > 0) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(Character)) {
                        if (Direction == "L") {
                            while (myResult.length < Count) {
                                myResult = Character + myResult;
                            }
                        }
                        else if (Direction == "R") {
                            while (myResult.length < Count) {
                                myResult = myResult + Character;
                            }
                        }
                    }
                }
            }
        }
        return myResult;
    };
    PaddingPipe = __decorate([
        core_1.Pipe({ name: 'PaddingPipe' })
    ], PaddingPipe);
    return PaddingPipe;
}());
exports.PaddingPipe = PaddingPipe;
//# sourceMappingURL=PaddingPipe.js.map