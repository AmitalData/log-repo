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
var LocationsFilter = /** @class */ (function () {
    function LocationsFilter() {
        this.SelectedValueChanged = new core_1.EventEmitter();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.selectedValue = "O";
        if (this.CurrentSession == null) {
            this.FilterId_A = "LocationFilter_A_-1_-1";
            this.FilterId_O = "LocationFilter_O_-1_-1";
            this.FilterId_H = "LocationFilter_H_-1_-1";
            this.FilterId_I = "LocationFilter_C_-1_-1";
            this.FilterId_D = "LocationFilter_D_-1_-1";
        }
        else {
            var idIndex = this.CurrentSession.GetNewId("LocationsFilter");
            this.FilterId_A = "LocationFilter_A_" + idIndex;
            this.FilterId_O = "LocationFilter_O_" + idIndex;
            this.FilterId_H = "LocationFilter_H_" + idIndex;
            this.FilterId_I = "LocationFilter_C_" + idIndex;
            this.FilterId_D = "LocationFilter_D_" + idIndex;
        }
    }
    Object.defineProperty(LocationsFilter.prototype, "SelectedValue", {
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
    LocationsFilter.prototype.itemClicked = function (itemValue) {
        if (this.SelectedValue != itemValue) {
            this.SelectedValue = itemValue;
            this.SelectedValueChanged.emit(itemValue);
        }
    };
    LocationsFilter.prototype.itemMouseOver = function (itemValue) {
    };
    LocationsFilter.prototype.itemMouseLeave = function (itemValue) {
    };
    LocationsFilter.prototype.ApplySelectedStyle = function () {
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], LocationsFilter.prototype, "SelectedValueChanged", void 0);
    LocationsFilter = __decorate([
        core_1.Component({
            selector: 'LocationsFilter',
            inputs: ['SelectedValue'],
            changeDetection: core_1.ChangeDetectionStrategy.OnPush,
            template: "\n    <ul class=\"FiltersMenu\">\n        <li style=\" width: 40px;\" (click)=\"itemClicked('A')\" (mouseover)=\"itemMouseOver('A')\" (mouseleave)=\"itemMouseLeave('A')\" [class.SelectedFilter]=\"SelectedValue === 'A'\" title=\"All\">\n            All\n        </li>\n        <li style=\" width: 40px;\" (click)=\"itemClicked('O')\" (mouseover)=\"itemMouseOver('O')\" (mouseleave)=\"itemMouseLeave('O')\" [class.SelectedFilter]=\"SelectedValue === 'O'\" title=\"Office\">\n            Office\n        </li>\n        <li style=\" width: 40px;\" (click)=\"itemClicked('H')\" (mouseover)=\"itemMouseOver('H')\" (mouseleave)=\"itemMouseLeave('H')\" [class.SelectedFilter]=\"SelectedValue === 'H'\" title=\"House\">\n            Home\n        </li>\n        <li style=\" width: 40px;\" (click)=\"itemClicked('C')\" (mouseover)=\"itemMouseOver('C')\" (mouseleave)=\"itemMouseLeave('C')\" [class.SelectedFilter]=\"SelectedValue === 'C'\" title=\"Client\">\n          Client\n        </li>\n         <li style=\" width: 50px;\" (click)=\"itemClicked('D')\" (mouseover)=\"itemMouseOver('D')\" (mouseleave)=\"itemMouseLeave('D')\" [class.SelectedFilter]=\"SelectedValue === 'D'\" title=\"Day Off\">\n          Day Off\n        </li>\n\n    </ul>\n    "
        }),
        __metadata("design:paramtypes", [])
    ], LocationsFilter);
    return LocationsFilter;
}());
exports.LocationsFilter = LocationsFilter;
//# sourceMappingURL=LocationsFilter.js.map