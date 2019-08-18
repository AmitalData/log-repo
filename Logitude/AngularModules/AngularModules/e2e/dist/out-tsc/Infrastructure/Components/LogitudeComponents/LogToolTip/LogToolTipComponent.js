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
var TextCodeTranslator_1 = require("./../../../Utilities/TextCodeTranslator");
var ObjectsLocator_1 = require("./../../../Locators/ObjectsLocator");
var core_1 = require("@angular/core");
var Tools_1 = require("./../../../Tools");
var LogToolTipComponent = /** @class */ (function () {
    function LogToolTipComponent() {
        this.isRTL = false;
        this.name = 'no1';
        this.direction = 'bottomright';
        this.mode = 'info';
        this.isMouseIn = false;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    }
    LogToolTipComponent.prototype.ngAfterViewInit = function () {
        switch (this.mode) {
            case 'Warning':
                this.title = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.Warning"); // "Warning";
                break;
            default:
                break;
        }
    };
    LogToolTipComponent.prototype.OnMouseOver = function () {
        var _this = this;
        this.isMouseIn = true;
        setTimeout(function () {
            if (_this.isMouseIn) {
                _this.draw();
            }
        }, 100);
    };
    LogToolTipComponent.prototype.OnMouseLeave = function () {
        var _this = this;
        this.isMouseIn = false;
        //if (this.currencyRate) {
        setTimeout(function () {
            document.getElementById("tooltip-body" + _this.name).style.visibility = "hidden";
        }, 400);
        //}
    };
    LogToolTipComponent.prototype.draw = function () {
        // var i = document.getElementById("tooltip-body" + this.name);
        // if (AppTool.IsNullOrEmpty(i))
        //     return;
        var item = document.getElementById("tooltip" + this.name);
        if (Tools_1.AppTool.IsNullOrEmpty(item))
            return;
        var itemRect = item.getBoundingClientRect();
        var element = document.getElementById("tooltip-body" + this.name);
        element.style.position = "fixed";
        element.style.visibility = "visible";
        switch (this.direction) {
            case 'topright':
                {
                    element.style.bottom = (this.getScreenHeight() - itemRect.top - 10) + 'px';
                    element.style.left = (itemRect.left + 15) + 'px';
                    break;
                }
            case 'bottomright':
                {
                    element.style.top = (itemRect.top - 3) + 'px';
                    element.style.left = (itemRect.left + 32) + 'px';
                    break;
                }
            case 'bottomleft':
                {
                    // get popup width
                    var bodyItem = document.getElementById("tooltip-body" + this.name);
                    var bodyItemRect = bodyItem.getBoundingClientRect();
                    element.style.top = (itemRect.top - 3) + 'px';
                    element.style.left = (itemRect.right - bodyItemRect.width - 30) + 'px';
                    break;
                }
            default:
                break;
        }
    };
    LogToolTipComponent.prototype.getScreenHeight = function () {
        if (self.innerHeight) {
            return self.innerHeight;
        }
        if (document.documentElement && document.documentElement.clientHeight) {
            return document.documentElement.clientHeight;
        }
        if (document.body) {
            return document.body.clientHeight;
        }
    };
    LogToolTipComponent.prototype.GetArrowClassName = function () {
        var className = "small-tooltip-arrow";
        switch (this.direction) {
            case 'topright':
                {
                    break;
                }
            case 'bottomright':
                {
                    className += " arrow-left";
                    break;
                }
            case 'bottomleft':
                {
                    className += " arrow-right";
                    break;
                }
            default:
                break;
        }
        return className;
    };
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], LogToolTipComponent.prototype, "title", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], LogToolTipComponent.prototype, "name", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], LogToolTipComponent.prototype, "direction", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], LogToolTipComponent.prototype, "mode", void 0);
    LogToolTipComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'LogToolTip',
            templateUrl: './LogToolTipComponent.html',
            styleUrls: ['./LogToolTipComponent.css']
        }),
        __metadata("design:paramtypes", [])
    ], LogToolTipComponent);
    return LogToolTipComponent;
}());
exports.LogToolTipComponent = LogToolTipComponent;
//# sourceMappingURL=LogToolTipComponent.js.map