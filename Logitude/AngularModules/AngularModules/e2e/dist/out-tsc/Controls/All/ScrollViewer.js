"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var ScrollViewer = /** @class */ (function () {
    function ScrollViewer() {
        this.Small = false;
        this.IsTowSides = false;
    }
    ScrollViewer = __decorate([
        core_1.Component({
            selector: 'ScrollViewer',
            inputs: ['Small', 'IsTowSides'],
            changeDetection: core_1.ChangeDetectionStrategy.OnPush,
            template: "\n    <div class=\"MediaFill\">\n        <div class=\"LogitudeScrollViewer\" [class.LogitudeSmallScrollViewer]=\"Small\" [ngStyle]=\"{'overflow-x': IsTowSides ? 'auto' : 'hidden'}\">\n            <ng-content></ng-content>\n        </div>\n    </div>\n    ",
            styles: ["\n    .LogitudeScrollViewer {\n        height: 100%;\n        width: 100%;\n        max-height: 100%;\n        max-width: 100%;\n        overflow-y: auto;        \n        position: relative;\n    }\n    "],
        })
    ], ScrollViewer);
    return ScrollViewer;
}());
exports.ScrollViewer = ScrollViewer;
//# sourceMappingURL=ScrollViewer.js.map