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
var TabSummary = /** @class */ (function () {
    function TabSummary() {
        this.height = 100;
    }
    Object.defineProperty(TabSummary.prototype, "Height", {
        get: function () { return this.height; },
        set: function (value) {
            if (this.height != value) {
                this.height = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    TabSummary = __decorate([
        core_1.Component({
            selector: 'TabSummary',
            inputs: ['Height'],
            changeDetection: core_1.ChangeDetectionStrategy.OnPush,
            template: "\n    <table [style.height.px]=\"Height\" [style.min-height.px]=\"Height\" [style.max-height.px]=\"Height\">\n        <tr>\n            <td>\n                <div class=\"MediaFill\">\n                    <div class=\"LogitudeTabSummary\">\n                        <div class=\"MarginAbsolute5\">\n                            <ng-content></ng-content>\n                        </div>\n                    </div>\n                </div>\n            </td>\n        </tr>\n    </table>    \n    ",
            styles: ["\n    .LogitudeTabSummary {\n        width: 100%;\n        height: 100%;\n        position: relative;\n        overflow: hidden;\n        \n        border: 0px;\n        border-top-width: 1px;\n        box-shadow: 0px -1px 7px #AAAAAA;\n        -moz-box-shadow: 0px -1px 7px #AAAAAA;\n        -webkit-box-shadow: 0px -1px 7px #AAAAAA;\n        border-radius: 10px 10px 8px 0px;\n        -moz-border-radius: 10px 10px 8px 0px;\n        -webkit-border-radius: 10px 10px 8px 0px;\n    }\n    "],
        }),
        __metadata("design:paramtypes", [])
    ], TabSummary);
    return TabSummary;
}());
exports.TabSummary = TabSummary;
//# sourceMappingURL=TabSummary.js.map