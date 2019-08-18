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
var SessionLocator_1 = require("../Infrastructure/Utilities/SessionLocator");
var DatesFilter = /** @class */ (function () {
    function DatesFilter() {
        this.SelectedValueChanged = new core_1.EventEmitter();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.selectedValue = "T";
        if (this.CurrentSession == null) {
            this.FilterId_A = "DateFilter_T_-1_-1";
            this.FilterId_T = "DateFilter_Y_-1_-1";
            this.FilterId_I = "DateFilter_P_-1_-1";
        }
        else {
            var idIndex = this.CurrentSession.GetNewId("DatesFilter");
            this.FilterId_A = "DateFilter_T_" + idIndex;
            this.FilterId_T = "DateFilter_Y_" + idIndex;
            this.FilterId_I = "DateFilter_P_" + idIndex;
        }
    }
    Object.defineProperty(DatesFilter.prototype, "SelectedValue", {
        get: function () { return this.selectedValue; },
        set: function (value) {
            if (this.selectedValue != value) {
                this.selectedValue = value;
                this.ApplySelectedStyle();
            }
        },
        enumerable: true,
        configurable: true
    });
    DatesFilter.prototype.itemClicked = function (itemValue) {
        if (this.SelectedValue != itemValue) {
            this.SelectedValue = itemValue;
            this.SelectedValueChanged.emit(itemValue);
        }
    };
    DatesFilter.prototype.itemMouseOver = function (itemValue) {
    };
    DatesFilter.prototype.itemMouseLeave = function (itemValue) {
    };
    DatesFilter.prototype.ApplySelectedStyle = function () {
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], DatesFilter.prototype, "SelectedValueChanged", void 0);
    DatesFilter = __decorate([
        core_1.Component({
            selector: 'DatesFilter',
            inputs: ['SelectedValue'],
            changeDetection: core_1.ChangeDetectionStrategy.OnPush,
            template: "\n    <ul class=\"FiltersMenu\">\n        <li style=\" width: 65px;\" (click)=\"itemClicked('T')\" (mouseover)=\"itemMouseOver('T')\" (mouseleave)=\"itemMouseLeave('T')\" [class.SelectedFilter]=\"SelectedValue === 'T'\">\n            Today\n        </li>\n        <li style=\" width: 65px;\" (click)=\"itemClicked('Y')\" (mouseover)=\"itemMouseOver('Y')\" (mouseleave)=\"itemMouseLeave('Y')\" [class.SelectedFilter]=\"SelectedValue === 'Y'\">\n            Yesterday\n        </li>\n        <li style=\" width: 65px;\" (click)=\"itemClicked('P')\" (mouseover)=\"itemMouseOver('P')\" (mouseleave)=\"itemMouseLeave('P')\" [class.SelectedFilter]=\"SelectedValue === 'P'\">\n          Period\n        </li>\n    </ul>\n    "
        }),
        __metadata("design:paramtypes", [])
    ], DatesFilter);
    return DatesFilter;
}());
exports.DatesFilter = DatesFilter;
//# sourceMappingURL=DatesFilter.js.map