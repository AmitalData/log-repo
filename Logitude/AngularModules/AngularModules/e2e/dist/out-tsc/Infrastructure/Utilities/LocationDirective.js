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
var LocationDirective = /** @class */ (function () {
    function LocationDirective(viewContainerRef) {
        this.viewContainerRef = viewContainerRef;
        this.DirectiveLoaded = new core_1.EventEmitter();
    }
    LocationDirective.prototype.ngAfterViewInit = function () {
        //this.DirectiveLoaded.emit('Directive Loaded');
    };
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], LocationDirective.prototype, "Code", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", Number)
    ], LocationDirective.prototype, "Index", void 0);
    __decorate([
        core_1.Input(),
        __metadata("design:type", String)
    ], LocationDirective.prototype, "ItemCode", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], LocationDirective.prototype, "DirectiveLoaded", void 0);
    LocationDirective = __decorate([
        core_1.Directive({
            selector: '[LocationDirective]'
        }),
        __metadata("design:paramtypes", [core_1.ViewContainerRef])
    ], LocationDirective);
    return LocationDirective;
}());
exports.LocationDirective = LocationDirective;
//# sourceMappingURL=LocationDirective.js.map