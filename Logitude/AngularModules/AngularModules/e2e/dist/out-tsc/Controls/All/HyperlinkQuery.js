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
var HyperlinkQuery = /** @class */ (function () {
    function HyperlinkQuery(elementRef) {
        this.elementRef = elementRef;
        this.myText = null;
        this.isEnabled = true;
    }
    HyperlinkQuery.prototype.ngOnInit = function () {
        this.SetComponent();
    };
    HyperlinkQuery.prototype.SetComponent = function () {
        if (this.elementRef) {
            var nativeElement = this.elementRef.nativeElement;
            if (nativeElement) {
                if (this.isEnabled) {
                    nativeElement.style.pointerEvents = "auto";
                }
                else {
                    nativeElement.style.pointerEvents = "none";
                }
            }
        }
    };
    Object.defineProperty(HyperlinkQuery.prototype, "Text", {
        get: function () { return this.myText; },
        set: function (value) {
            if (this.myText != value) {
                this.myText = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(HyperlinkQuery.prototype, "IsEnabled", {
        get: function () { return this.isEnabled; },
        set: function (value) {
            if (this.isEnabled != value) {
                this.isEnabled = value;
                this.SetComponent();
            }
        },
        enumerable: true,
        configurable: true
    });
    HyperlinkQuery = __decorate([
        core_1.Component({
            selector: 'HyperlinkQuery',
            inputs: ['Text', 'IsEnabled'],
            changeDetection: core_1.ChangeDetectionStrategy.OnPush,
            template: "    \n        <button class=\"HyperlinkQueryButtonControl\" [disabled]=\"!IsEnabled\" tabindex=\"-1\">            \n            {{Text}}\n\n            <span style=\"pointer-events: none;\">            \n                <ng-content></ng-content>\n            </span>\n        </button>\n    ",
            styles: ["\n    .HyperlinkQueryButtonControl:disabled {\n        opacity: 0.5;\n        cursor: default;\n\n    }\n\n    .HyperlinkQueryButtonControl:hover:not(:disabled), .HyperlinkQueryButtonControl:focus:not(:disabled) {\n        color: #1E8DC4;\n    }\n\n    .HyperlinkQueryButtonControl {\n        height: 21px !important;\n        width: auto !important;\n        line-height: 21px !important;\n        background: none !important;\n        border: none !important;\n        padding: 0 !important;\n        padding-left: 10px !important;\n        color: #282E30;\n        font-size: 12px !important;\n        cursor: pointer;\n        display: block !important;        \n        white-space: nowrap;        \n    }\n    "],
        }),
        __metadata("design:paramtypes", [core_1.ElementRef])
    ], HyperlinkQuery);
    return HyperlinkQuery;
}());
exports.HyperlinkQuery = HyperlinkQuery;
//# sourceMappingURL=HyperlinkQuery.js.map