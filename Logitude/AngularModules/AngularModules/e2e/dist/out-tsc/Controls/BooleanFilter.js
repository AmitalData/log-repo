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
var BooleanFilter = /** @class */ (function () {
    function BooleanFilter() {
        this.SelectedValue = "True";
        this.SelectedTypeValueChanged = new core_1.EventEmitter();
    }
    BooleanFilter.prototype.itemClicked = function (itemValue) {
        if (this.SelectedValue != itemValue) {
            this.SelectedValue = itemValue;
            this.SelectedTypeValueChanged.emit(itemValue);
        }
    };
    BooleanFilter.prototype.itemMouseOver = function (itemValue) {
    };
    BooleanFilter.prototype.itemMouseLeave = function (itemValue) {
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], BooleanFilter.prototype, "SelectedTypeValueChanged", void 0);
    BooleanFilter = __decorate([
        core_1.Component({
            selector: 'BooleanFilter',
            inputs: ['SelectedValue'],
            template: "\n    <ul class=\"FiltersMenu\"> \n        <li (click)=\"itemClicked('True')\" (mouseover)=\"itemMouseOver('True')\" style=\"width:35px;\" (mouseleave)=\"itemMouseLeave('True')\" [class.SelectedFilter]=\"SelectedValue === 'True'\">\n           Yes\n        </li>\n        <li (click)=\"itemClicked('False')\" (mouseover)=\"itemMouseOver('False')\" style=\"width:35px;\" (mouseleave)=\"itemMouseLeave('False')\" [class.SelectedFilter]=\"SelectedValue === 'False'\">\n           No\n        </li> \n    </ul>\n    "
        })
    ], BooleanFilter);
    return BooleanFilter;
}());
exports.BooleanFilter = BooleanFilter;
//# sourceMappingURL=BooleanFilter.js.map