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
var LogFooterComponent = /** @class */ (function () {
    function LogFooterComponent() {
    }
    LogFooterComponent.prototype.ngOnInit = function () {
    };
    LogFooterComponent.prototype.ngAfterContentInit = function () {
        // get all active tabs
        //if (this.Children.length > 0) {
        //    //alert("bbbbbbbbb");
        //}
        //this.hastemplate = this.innerContentTpl.length == 0 ? false : true;
    };
    __decorate([
        core_1.ContentChild(core_1.TemplateRef),
        __metadata("design:type", Object)
    ], LogFooterComponent.prototype, "myChild", void 0);
    LogFooterComponent = __decorate([
        core_1.Component({
            selector: 'log-footer',
            moduleId: module.id,
            templateUrl: './LogFooterComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], LogFooterComponent);
    return LogFooterComponent;
}());
exports.LogFooterComponent = LogFooterComponent;
//# sourceMappingURL=LogFooterComponent.js.map