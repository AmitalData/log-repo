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
var InnerSpanComponent = /** @class */ (function () {
    function InnerSpanComponent() {
        this.format = null;
        this.spanclickevent = new core_1.EventEmitter();
    }
    InnerSpanComponent.prototype.ngOnInit = function () {
        var x = this.format;
        //if (this.format != undefined) {
        //    if (this.format[0] == "n") {
        //        var numafterdot = this.format[1];
        //        var tempo = this.rowData[this.fieldName];
        //        var number = parseFloat(tempo).toFixed(parseInt(numafterdot));
        //        this.Data = number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        //    }
        //}
        //else {
        this.Data = this.rowData[this.fieldName];
        //}
        //this.element = document.getElementById("inner-input-control");
        //this.element.value(); 
    };
    InnerSpanComponent.prototype.onclick = function () {
        this.spanclickevent.emit("");
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], InnerSpanComponent.prototype, "spanclickevent", void 0);
    InnerSpanComponent = __decorate([
        core_1.Component({
            selector: 'inner-span-component',
            template: " \n               <span *ngIf=\"format\" id=\"inner-span-id\" style=\"text-overflow: ellipsis;padding-left: 3px;padding-right: 5px;\" (click)=\"onclick()\" >{{Data | NumbersPipe:format}}</span>\n               <span *ngIf=\"!format\" id=\"inner-span-id\" style=\"text-overflow: ellipsis;padding-left: 3px;padding-right: 5px;\" (click)=\"onclick()\" >{{Data}}</span>\n              ",
            inputs: ['Data', 'fieldName', 'rowData', 'type', 'format']
        })
    ], InnerSpanComponent);
    return InnerSpanComponent;
}());
exports.InnerSpanComponent = InnerSpanComponent;
//# sourceMappingURL=InnerSpanComponent.js.map