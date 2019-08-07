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
var Tools_1 = require("../../Infrastructure/Tools");
var Hyperlink = /** @class */ (function () {
    function Hyperlink(elementRef) {
        this.elementRef = elementRef;
        this.FontSize = 10;
        this.Title = null;
        this.myText = null;
        this.myColor = "#1E4AC4";
        this.isEnabled = true;
    }
    Hyperlink.prototype.ngOnInit = function () {
        this.SetComponent();
    };
    Hyperlink.prototype.SetComponent = function () {
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
    Object.defineProperty(Hyperlink.prototype, "Text", {
        get: function () { return this.myText; },
        set: function (value) {
            if (this.myText != value) {
                this.myText = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Hyperlink.prototype, "Color", {
        get: function () { return this.myColor; },
        set: function (value) {
            if (this.myColor != value) {
                if (Tools_1.AppTool.IsNullOrEmpty(value)) {
                    value = "#1E4AC4";
                }
                this.myColor = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Hyperlink.prototype, "IsEnabled", {
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
    Hyperlink = __decorate([
        core_1.Component({
            selector: 'Hyperlink',
            inputs: ['Text', 'FontSize', 'Color', 'IsEnabled', 'Title'],
            changeDetection: core_1.ChangeDetectionStrategy.OnPush,
            template: "    \n        <button class=\"HyperlinkButtonControl\" [disabled]=\"!IsEnabled\" [style.font-size.px]=\"FontSize\" [style.color]=\"Color\" tabindex=\"-1\">\n            {{Text}}\n            \n            <span style=\"pointer-events: none;\" [style.font-size.px]=\"FontSize\" [style.color]=\"Color\">            \n                <ng-content></ng-content>\n            </span>\n        </button>    \n    ",
            styles: ["\n    .HyperlinkButtonControl:disabled {\n        opacity: 0.5;\n        cursor: default;        \n    }\n\n    .HyperlinkButtonControl:hover:not(:disabled), .HyperlinkButtonControl:focus:not(:disabled) {\n        color: #1E8DC4;\n    }\n\n    .HyperlinkButtonControl {\n        height: auto !important;\n        width: auto !important;\n        line-height: unset !important;\n        background: none !important;\n        border: none !important;\n        padding: 0 !important;\n        color: #1E4AC4;\n        cursor: pointer;\n        text-decoration: underline;\n        display: inline-block !important;        \n        white-space: nowrap;        \n    }\n    "],
        }),
        __metadata("design:paramtypes", [core_1.ElementRef])
    ], Hyperlink);
    return Hyperlink;
}());
exports.Hyperlink = Hyperlink;
//# sourceMappingURL=Hyperlink.js.map