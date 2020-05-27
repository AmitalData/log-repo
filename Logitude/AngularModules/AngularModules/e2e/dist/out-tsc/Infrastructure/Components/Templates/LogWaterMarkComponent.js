"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var Tools_1 = require("../../Tools");
var LogWaterMarkComponent = /** @class */ (function () {
    function LogWaterMarkComponent() {
        this._SetFucos = false;
        this.LogWaterMarkValue = "";
        this.Watermark = "";
    }
    LogWaterMarkComponent.prototype.SetFucos = function (val) {
        this._SetFucos = val;
    };
    Object.defineProperty(LogWaterMarkComponent.prototype, "ShowWaterMark", {
        get: function () {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.LogWaterMarkValue)) {
                return false;
            }
            if (this._SetFucos) {
                return false;
            }
            return true;
        },
        enumerable: true,
        configurable: true
    });
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], LogWaterMarkComponent.prototype, "LogWaterMarkValue", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], LogWaterMarkComponent.prototype, "Watermark", void 0);
    LogWaterMarkComponent = __decorate([
        core_1.Component({
            selector: 'LogWaterMark',
            template: 
            //width:166px;height: 22px;
            "<div \n    style=\"position: relative;\" \n    (mouseleave)=\"SetFucos(false)\"  \n    >\n    <ng-content></ng-content>\n    <input \n        style=\"position: absolute;top: 0px;left: 0px;opacity:0.7;\"\n        (click)=\"SetFucos(true)\"  \n        *ngIf=\"ShowWaterMark\"\n        placeholder=\"{{Watermark}}\" />\n</div>",
        }),
        __metadata("design:paramtypes", [])
    ], LogWaterMarkComponent);
    return LogWaterMarkComponent;
}());
exports.LogWaterMarkComponent = LogWaterMarkComponent;
//# sourceMappingURL=LogWaterMarkComponent.js.map