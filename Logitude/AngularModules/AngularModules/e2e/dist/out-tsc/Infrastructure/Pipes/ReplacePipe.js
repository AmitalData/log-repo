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
var ReplacePipe = /** @class */ (function () {
    function ReplacePipe() {
    }
    ReplacePipe.prototype.transform = function (text, replaceFrom, replaceTo) {
        var myResult = "";
        if (!Tools_1.AppTool.IsNullOrEmpty(text)) {
            if (!Tools_1.AppTool.IsNullOrEmpty(replaceFrom)) {
                if (!Tools_1.AppTool.IsNullOrEmpty(replaceTo)) {
                    //myResult = text.replace(/replaceFrom/gi, replaceTo);
                    myResult = text.replace(new RegExp(replaceFrom, 'g'), replaceTo);
                }
            }
        }
        return myResult;
    };
    ReplacePipe = __decorate([
        core_1.Pipe({ name: 'ReplacePipe' })
    ], ReplacePipe);
    return ReplacePipe;
}());
exports.ReplacePipe = ReplacePipe;
//# sourceMappingURL=ReplacePipe.js.map