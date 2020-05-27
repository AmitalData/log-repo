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
var CurrencyFilter = /** @class */ (function () {
    function CurrencyFilter() {
        this.SelectedValue = "NIS,local";
        this.SelectedValueChanged = new core_1.EventEmitter();
    }
    CurrencyFilter.prototype.itemClicked = function (itemValue) {
        if (this.SelectedValue != itemValue) {
            this.SelectedValue = itemValue;
            this.SelectedValueChanged.emit(itemValue);
        }
    };
    CurrencyFilter.prototype.itemMouseOver = function (itemValue) {
        if (this.SelectedValue != itemValue) {
            var att = document.getElementById(itemValue);
            att.style.backgroundColor = 'white';
        }
    };
    CurrencyFilter.prototype.itemMouseLeave = function (itemValue) {
        if (this.SelectedValue != itemValue) {
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], CurrencyFilter.prototype, "SelectedValueChanged", void 0);
    CurrencyFilter = __decorate([
        core_1.Component({
            selector: 'CurrencyFilter',
            inputs: ['CurrencyFilter'],
            template: "\n    <ul class=\"FiltersMenu\">\n       \n       \n        <li id=\"USD,profit\" (click)=\"itemClicked('USD,profit')\" (mouseover)=\"itemMouseOver('USD,profit')\" (mouseleave)=\"itemMouseLeave('USD,profit')\" [class.SelectedFilter]=\"SelectedValue === 'USD,profit'\" title=\"USD\">\n            USD\n        </li>\n\n <li id=\"NIS,local\" (click)=\"itemClicked('NIS,local')\" (mouseover)=\"itemMouseOver('NIS,local')\" (mouseleave)=\"itemMouseLeave('NIS,local')\" [class.SelectedFilter]=\"SelectedValue === 'NIS,local'\" title=\"NIS\">\n          NIS\n        </li>\n        \n    </ul>\n    "
        })
    ], CurrencyFilter);
    return CurrencyFilter;
}());
exports.CurrencyFilter = CurrencyFilter;
//# sourceMappingURL=CurrencyFilter.js.map