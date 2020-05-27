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
var ObjectsLocator_1 = require("../Locators/ObjectsLocator");
var FixedPositionDirective = /** @class */ (function () {
    function FixedPositionDirective(viewContainerRef) {
        this.viewContainerRef = viewContainerRef;
        this.PaintTop = false;
        this.PaintRight = false;
        this.LayoutDirection = ObjectsLocator_1.ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection;
    }
    FixedPositionDirective.prototype.ngOnInit = function () {
    };
    FixedPositionDirective.prototype.ngAfterViewInit = function () {
        document.getElementById(this.ElementId).style.position = "fixed";
        this.CalculateFixedPosition();
        this.RunPositionTimer();
    };
    FixedPositionDirective.prototype.ngOnDestroy = function () {
        this.StopPositionTimer();
    };
    FixedPositionDirective.prototype.CalculateFixedPosition = function () {
        if (this.LayoutDirection == 'rtl' && this.ComponentType != 'DocumentPanel') { // DocumentPanel will be LTR always
            this.DrawRightToLeft();
        }
        else {
            this.DrawLeftToRight();
        }
    };
    FixedPositionDirective.prototype.StopPositionTimer = function () {
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
    };
    FixedPositionDirective.prototype.RunPositionTimer = function () {
        var _this = this;
        this.StopPositionTimer();
        this.timerToken = setInterval(function () { return _this.CalculateFixedPosition(); }, 100);
    };
    FixedPositionDirective.prototype.getScreenHeight = function () {
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
    FixedPositionDirective.prototype.getScreenWidth = function () {
        if (self.innerWidth) {
            return self.innerWidth;
        }
        if (document.documentElement && document.documentElement.clientWidth) {
            return document.documentElement.clientWidth;
        }
        if (document.body) {
            return document.body.clientWidth;
        }
    };
    FixedPositionDirective.prototype.DrawLeftToRight = function () {
        if (this.RelativeElementId) {
            var item = document.getElementById(this.RelativeElementId);
            if (item) {
                var itemRect = item.getBoundingClientRect();
                //// Abdullah
                var h = this.getScreenHeight();
                if (itemRect.bottom + this.ComponentHight > h) {
                    this.PaintTop = true;
                }
                if (itemRect.left + this.PopupWidth > this.getScreenWidth()) {
                    this.PaintRight = true;
                }
                document.getElementById(this.ElementId).style.minWidth = (itemRect.width > this.PopupWidth ? this.PopupWidth : itemRect.width) + 'px';
                var left = 0;
                var top = 0;
                if (this.ComponentType == "popup") {
                    document.getElementById(this.ElementId).style.position = "fixed";
                    top = (itemRect.top + this.TopDisplacement);
                    left = itemRect.left + itemRect.width;
                }
                else if (this.ComponentType == "datepicker") {
                    document.getElementById(this.ElementId).style.minWidth = '';
                    top = (this.PaintTop ? (itemRect.top - this.ComponentHight) : (itemRect.top + this.VertDisplacement));
                    left = itemRect.left + itemRect.width + 20 - this.PopupWidth;
                }
                else if (this.ComponentType == "MTCPopoup") {
                    top = (this.PaintTop ? (itemRect.top - this.ComponentHight) : (itemRect.top + this.VertDisplacement));
                    left = (this.PaintRight ? (this.getScreenWidth() - this.PopupWidth) : (itemRect.left - this.PopupWidth + itemRect.width));
                }
                else {
                    top = (this.PaintTop ? (itemRect.top - this.ComponentHight) : (itemRect.top + this.VertDisplacement));
                    left = (this.PaintRight ? (this.getScreenWidth() - this.PopupWidth) : (itemRect.left));
                }
                left = left + (this.HorizDisplacement ? this.HorizDisplacement : 0);
                left = (this.ComponentType == "DocumentPanel" ? left - 25 : left);
                document.getElementById(this.ElementId).style.top = top + 'px';
                document.getElementById(this.ElementId).style.left = left + 'px';
            }
        }
    };
    FixedPositionDirective.prototype.DrawRightToLeft = function () {
        if (this.RelativeElementId) {
            var item = document.getElementById(this.RelativeElementId);
            if (item) {
                var itemRect = item.getBoundingClientRect();
                //// Abdullah
                var h = this.getScreenHeight();
                if (itemRect.bottom + this.ComponentHight > h) {
                    this.PaintTop = true;
                }
                //if (itemRect.left + this.PopupWidth > this.getScreenWidth()) {
                //    this.PaintRight = true;
                //}
                document.getElementById(this.ElementId).style.minWidth = (itemRect.width > this.PopupWidth ? this.PopupWidth : itemRect.width) + 'px';
                var left = 0;
                var top = 0;
                if (this.ComponentType == "popup") {
                    document.getElementById(this.ElementId).style.position = "fixed";
                    top = (itemRect.top + this.TopDisplacement);
                    left = itemRect.left + itemRect.width - this.PopupWidth;
                }
                else if (this.ComponentType == "datepicker") {
                    document.getElementById(this.ElementId).style.minWidth = '';
                    top = (this.PaintTop ? (itemRect.top - this.ComponentHight) : (itemRect.top + this.VertDisplacement));
                    left = itemRect.left + itemRect.width - this.PopupWidth;
                }
                else {
                    top = (this.PaintTop ? (itemRect.top - this.ComponentHight) : (itemRect.top + this.VertDisplacement));
                    left = (this.PaintRight ? (this.getScreenWidth() - this.PopupWidth) : (itemRect.left - this.PopupWidth + itemRect.width));
                }
                left = left + (this.HorizDisplacement ? this.HorizDisplacement : 0);
                document.getElementById(this.ElementId).style.top = top + 'px';
                document.getElementById(this.ElementId).style.left = (left > 0 ? left : 0) + 'px';
            }
        }
    };
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], FixedPositionDirective.prototype, "ElementId", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], FixedPositionDirective.prototype, "RelativeElementId", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Number)
    ], FixedPositionDirective.prototype, "TopDisplacement", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Number)
    ], FixedPositionDirective.prototype, "LeftDisplacement", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], FixedPositionDirective.prototype, "ComponentType", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Number)
    ], FixedPositionDirective.prototype, "ComponentHight", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Number)
    ], FixedPositionDirective.prototype, "PopupWidth", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Number)
    ], FixedPositionDirective.prototype, "VertDisplacement", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Number)
    ], FixedPositionDirective.prototype, "HorizDisplacement", void 0);
    FixedPositionDirective = __decorate([
        core_1.Directive({
            selector: '[FixedPosition]'
        }),
        __metadata("design:paramtypes", [core_1.ViewContainerRef])
    ], FixedPositionDirective);
    return FixedPositionDirective;
}());
exports.FixedPositionDirective = FixedPositionDirective;
//# sourceMappingURL=FixedPositionDirective.js.map