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
var FeatureLocator_1 = require("../Infrastructure/Utilities/FeatureLocator");
var UserFilter = /** @class */ (function () {
    function UserFilter() {
        this.FilterId_M_Feature = false;
        this.FilterId_A_Feature = false;
        this.SelectedValueChanged = new core_1.EventEmitter();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.selectedValue = "M";
        this.SetVisibilityOfFilters();
        if (this.CurrentSession == null) {
            this.FilterId_M = "LocationFilter_M_-1_-1";
            this.FilterId_A = "LocationFilter_A_-1_-1";
        }
        else {
            var idIndex = this.CurrentSession.GetNewId("UserFilter");
            this.FilterId_M = "LocationFilter_M_" + idIndex;
            this.FilterId_A = "LocationFilter_A_" + idIndex;
        }
    }
    Object.defineProperty(UserFilter.prototype, "SelectedValue", {
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
    UserFilter.prototype.itemClicked = function (itemValue) {
        if (this.SelectedValue != itemValue) {
            this.SelectedValue = itemValue;
            this.SelectedValueChanged.emit(itemValue);
        }
    };
    UserFilter.prototype.itemMouseOver = function (itemValue) {
    };
    UserFilter.prototype.itemMouseLeave = function (itemValue) {
    };
    UserFilter.prototype.ApplySelectedStyle = function () {
    };
    UserFilter.prototype.SetVisibilityOfFilters = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("FilingInbox", "FilingInboxMyFilter")) {
            this.FilterId_M_Feature = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("FilingInbox", "FilingInboxAllFilter")) {
            this.FilterId_A_Feature = true;
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], UserFilter.prototype, "SelectedValueChanged", void 0);
    UserFilter = __decorate([
        core_1.Component({
            selector: 'UserFilter',
            inputs: ['SelectedValue'],
            changeDetection: core_1.ChangeDetectionStrategy.OnPush,
            template: "\n    <ul class=\"FiltersMenu\">\n        <li *ngIf=\"FilterId_M_Feature\" style=\" width: 55px;\" (click)=\"itemClicked('M')\" (mouseover)=\"itemMouseOver('M')\" (mouseleave)=\"itemMouseLeave('M')\" [class.SelectedFilter]=\"SelectedValue === 'M'\" title=\"My\">\n            My\n        </li>\n        <li *ngIf=\"FilterId_A_Feature\" style=\" width: 55px;\" (click)=\"itemClicked('A')\" (mouseover)=\"itemMouseOver('A')\" (mouseleave)=\"itemMouseLeave('A')\" [class.SelectedFilter]=\"SelectedValue === 'A'\" title=\"All\">\n            All\n        </li>\n    </ul>\n    "
        }),
        __metadata("design:paramtypes", [])
    ], UserFilter);
    return UserFilter;
}());
exports.UserFilter = UserFilter;
//# sourceMappingURL=UserFilter.js.map