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
var ConnectToFilter = /** @class */ (function () {
    function ConnectToFilter() {
        this.FilterId_M_Feature = false;
        this.FilterId_S_Feature = false;
        this.FilterId_Q_Feature = false;
        this.SelectedValueChanged = new core_1.EventEmitter();
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.selectedValue = "M";
        this.SetVisibilityOfFilters();
        if (this.CurrentSession == null) {
            this.FilterId_M = "LocationFilter_M_-1_-1";
            this.FilterId_S = "LocationFilter_S_-1_-1";
            this.FilterId_Q = "LocationFilter_Q_-1_-1";
        }
        else {
            var idIndex = this.CurrentSession.GetNewId("ConnectToFilter");
            this.FilterId_M = "LocationFilter_M_" + idIndex;
            this.FilterId_S = "LocationFilter_S_" + idIndex;
            this.FilterId_Q = "LocationFilter_Q_" + idIndex;
        }
    }
    Object.defineProperty(ConnectToFilter.prototype, "SelectedValue", {
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
    ConnectToFilter.prototype.itemClicked = function (itemValue) {
        if (this.SelectedValue != itemValue) {
            this.SelectedValue = itemValue;
            this.SelectedValueChanged.emit(itemValue);
        }
    };
    ConnectToFilter.prototype.itemMouseOver = function (itemValue) {
    };
    ConnectToFilter.prototype.itemMouseLeave = function (itemValue) {
    };
    ConnectToFilter.prototype.ApplySelectedStyle = function () {
    };
    ConnectToFilter.prototype.SetVisibilityOfFilters = function () {
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("FilingInbox", "ShipmentsFilter")) {
            this.FilterId_S_Feature = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("FilingInbox", "MastersFilter")) {
            this.FilterId_M_Feature = true;
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("FilingInbox", "QuotesFilter")) {
            this.FilterId_Q_Feature = true;
        }
    };
    __decorate([
        core_1.Output(),
        __metadata("design:type", Object)
    ], ConnectToFilter.prototype, "SelectedValueChanged", void 0);
    ConnectToFilter = __decorate([
        core_1.Component({
            selector: 'ConnectToFilter',
            inputs: ['SelectedValue'],
            changeDetection: core_1.ChangeDetectionStrategy.OnPush,
            template: "\n    <ul class=\"FiltersMenu\">\n        <li *ngIf=\"FilterId_M_Feature\" style=\" width: 60px;\" (click)=\"itemClicked('M')\" (mouseover)=\"itemMouseOver('M')\" (mouseleave)=\"itemMouseLeave('M')\" [class.SelectedFilter]=\"SelectedValue === 'M'\" title=\"Master\">\n            Master\n        </li>\n        <li *ngIf=\"FilterId_S_Feature\" style=\" width: 60px;\" (click)=\"itemClicked('S')\" (mouseover)=\"itemMouseOver('S')\" (mouseleave)=\"itemMouseLeave('S')\" [class.SelectedFilter]=\"SelectedValue === 'S'\" title=\"Shipment\">\n            Shipment\n        </li>\n        <li *ngIf=\"FilterId_Q_Feature\" style=\" width: 60px;\" (click)=\"itemClicked('Q')\" (mouseover)=\"itemMouseOver('Q')\" (mouseleave)=\"itemMouseLeave('Q')\" [class.SelectedFilter]=\"SelectedValue === 'Q'\" title=\"Quote\">\n          Quote\n        </li>\n    </ul>\n    "
        }),
        __metadata("design:paramtypes", [])
    ], ConnectToFilter);
    return ConnectToFilter;
}());
exports.ConnectToFilter = ConnectToFilter;
//# sourceMappingURL=ConnectToFilter.js.map