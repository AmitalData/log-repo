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
var InnerComponent = /** @class */ (function () {
    function InnerComponent() {
        this.blurevent = new core_1.EventEmitter();
        this.changeevent = new core_1.EventEmitter();
        //(change)="onchange()"
    }
    InnerComponent.prototype.ngOnInit = function () {
        this.customewidth = { "width": this.width };
        this.element = document.getElementById("inner-input-control");
        this.element.focus();
    };
    InnerComponent.prototype.onblur = function () {
        this.blurevent.emit("");
    };
    InnerComponent.prototype.onchange = function () {
        this.changeevent.emit(this.element.value);
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], InnerComponent.prototype, "blurevent", void 0);
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], InnerComponent.prototype, "changeevent", void 0);
    InnerComponent = __decorate([
        core_1.Component({
            selector: 'inner-component',
            template: " \n               <input tabindex=\"0\" type=\"text\" [ngStyle]=\"customewidth\" id=\"inner-input-control\"  (blur)=\"onblur()\" [(ngModel)] = \"rowData[fieldName]\">\n              ",
            inputs: ['width', 'fieldName', 'rowData', 'type', 'src', 'Id']
        })
    ], InnerComponent);
    return InnerComponent;
}());
exports.InnerComponent = InnerComponent;
//# sourceMappingURL=InnerComponent.js.map