"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var core_2 = require("@angular/core");
var EntityArgs = /** @class */ (function () {
    function EntityArgs() {
        this.IsNewEntity = false;
        this.SkipCtor = false;
        this.PreSelectedTabCode = null;
        this.EntityArgEventEmitter = new core_1.EventEmitter();
    }
    EntityArgs.prototype.SendMessage = function (token) {
        this.EntityArgEventEmitter.emit(token);
    };
    EntityArgs = __decorate([
        core_2.Injectable()
    ], EntityArgs);
    return EntityArgs;
}());
exports.EntityArgs = EntityArgs;
//# sourceMappingURL=EntityArgs.js.map