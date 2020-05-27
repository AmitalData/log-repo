"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var WarningSummary = /** @class */ (function () {
    function WarningSummary() {
        this.SingleLine = false;
        this.HideHeader = false;
        this.ItemWidth = "50%";
        this.itemsSource = [];
    }
    WarningSummary.prototype.ngOnInit = function () {
        if (this.SingleLine) {
            this.ItemWidth = "100%";
        }
    };
    Object.defineProperty(WarningSummary.prototype, "ItemsSource", {
        get: function () { return this.itemsSource; },
        set: function (newValue) {
            var _this = this;
            if (this.itemsSource != newValue) {
                this.itemsSource = [];
                if (newValue != null) {
                    newValue.forEach(function (item) {
                        if (item != null) {
                            if (item.startsWith("!!")) {
                                item = item.substr(1);
                            }
                            if (_this.itemsSource.indexOf(item) == -1) {
                                _this.itemsSource.push(item);
                            }
                        }
                        //if (item != null) {
                        //    var fixedItem = item;
                        //    while (fixedItem.charAt(0) === '!') {
                        //        fixedItem = fixedItem.substr(1);
                        //    }
                        //    if (this.itemsSource.indexOf(fixedItem) == -1) {
                        //        this.itemsSource.push(fixedItem);
                        //    }
                        //}
                    });
                }
                this.ErrorsCount = this.itemsSource.length;
                this.UpdateItemWidth();
            }
        },
        enumerable: true,
        configurable: true
    });
    WarningSummary.prototype.UpdateItemWidth = function () {
        if (this.ErrorsCount <= 1) {
            this.ItemWidth = "100%";
        }
        else {
            if (this.SingleLine) {
                this.ItemWidth = "100%";
            }
            else {
                this.ItemWidth = "50%";
            }
        }
    };
    WarningSummary = __decorate([
        core_1.Component({
            selector: 'WarningSummary',
            inputs: ['ItemsSource', 'SingleLine', 'HideHeader'],
            changeDetection: core_1.ChangeDetectionStrategy.OnPush,
            template: "\n    <table style=\"min-height: 25px;\">\n        <tr>\n            <td>\n                <div class=\"WarningSummary\">\n                    <table>\n                        <tr>\n                            <td *ngIf=\"!HideHeader\" style=\"width: 120px; vertical-align: top; text-align: left; padding-left: 8px; padding-top: 4px; color: #D8770E; font-size: 12px;\">\n                                {{ErrorsCount}} Warnings Found:\n                            </td>\n\n                            <td style=\"position: relative; vertical-align:top; padding-top: 2px;\">\n                                <ul>\n                                    <li class=\"ValidationItem\" [ngStyle]=\"{width: ItemWidth}\" *ngFor=\"let item of ItemsSource\">\n                                        <img class=\"LeftCenter\" src=\"./Images/WarningIcon.png\" />\n                                        <span class=\"LeftCenter\" style=\"margin-left: 20px;\">{{item}}</span>\n                                    </li>\n                                </ul>\n                            </td>\n                        </tr>\n                    </table>\n                </div>\n            </td>\n        </tr>\n\n        <tr style=\"height: 5px;\">\n            <td>\n                <div></div>\n            </td>\n        </tr>\n    </table>\n    ",
            styles: ["\n    .WarningSummary span {\n        text-align: left;\n        width: calc(100% - 25px);\n        overflow: hidden;\n        white-space: nowrap;\n        text-overflow: ellipsis;\n    }\n\n    .WarningSummary .ValidationItem {\n        display: block;\n        float: left;\n        width: 50%;\n        height: 18px;\n        line-height: 20px;\n        position: relative;\n    }\n\n    .WarningSummary ul {\n        max-height: 80px;\n        box-sizing: border-box;\n        -moz-box-sizing: border-box;\n        -webkit-box-sizing: border-box;\n        overflow-x: hidden;\n        overflow-y: auto;\n    }\n\n    .WarningSummary {\n        height: 100%;\n        width: 100%;\n        border: 1px solid Orange;\n        border-radius: 5px;\n        background: #FFFBDA;\n        box-sizing: border-box;\n        -moz-box-sizing: border-box;\n        -webkit-box-sizing: border-box;\n        overflow: hidden;\n    }\n    "],
        })
    ], WarningSummary);
    return WarningSummary;
}());
exports.WarningSummary = WarningSummary;
//# sourceMappingURL=WarningSummary.js.map