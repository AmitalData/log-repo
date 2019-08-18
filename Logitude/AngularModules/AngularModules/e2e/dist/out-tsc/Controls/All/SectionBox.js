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
var SectionBox = /** @class */ (function () {
    function SectionBox() {
        this.Top = null;
        this.Bottom = null;
        this.Scrolling = false;
        this.headerHeight = 27;
        this.border = null;
    }
    Object.defineProperty(SectionBox.prototype, "HeaderHeight", {
        get: function () { return this.headerHeight; },
        set: function (value) {
            this.headerHeight = value;
            if (this.SectionHead) {
                this.SectionHead.Height = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SectionBox.prototype, "Border", {
        get: function () { return this.border; },
        set: function (value) {
            this.border = value;
            if (this.SectionHead) {
                this.SectionHead.Border = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    SectionBox = __decorate([
        core_1.Component({
            selector: 'SectionBox',
            inputs: ['Top', 'Bottom', 'Scrolling', 'HeaderHeight', 'Border'],
            changeDetection: core_1.ChangeDetectionStrategy.OnPush,
            template: "\n    <div class=\"MediaFill\">\n        <table>\n            <tr [style.height.px]=\"Top\" *ngIf=\"Top\">\n                <td>\n                    <div></div>\n                </td>\n            </tr>\n\n            <tr [style.height.px]=\"HeaderHeight\">\n                <td [style.height.px]=\"HeaderHeight\">\n                    <ng-content select=\"SectionHead\"></ng-content>\n                </td>\n            </tr>\n\n            <tr style=\"height: 3px;\" *ngIf=\"!Border\">\n                <td>\n                    <div></div>\n                </td>\n            </tr>\n\n            <tr>\n                <td>\n                    <div class=\"MediaFill\">\n                        <div class=\"MediaFixed\">\n                            <div class=\"LogitudeSectionBody LogitudeSmallScrollViewer\" [ngStyle]=\"{'overflow-y': Scrolling ? 'auto' : 'hidden', 'border': Border, 'border-top': '0px solid transparent'}\">                                 \n                                <ng-content select=\"SectionBody\"></ng-content>\n                            </div>\n                        </div>\n                    </div>\n                </td>\n            </tr>\n\n            <tr [style.height.px]=\"Bottom\" *ngIf=\"Bottom\">\n                <td>\n                    <div></div>\n                </td>\n            </tr>\n        </table>\n    </div>\n    ",
            styles: ["\n    .LogitudeSectionBody {\n        height:100%;        \n        width:100%;\n        min-width: 100%;\n        min-height: 100%;\n        max-height: 100%;\n        max-width: 100%;\n        overflow-x: hidden;\n        position: relative;\n        border-radius: 0px 0px 4px 4px;\n        -moz-border-radius: 0px 0px 4px 4px;\n        -webkit-border-radius: 0px 0px 4px 4px;\n        padding-right: 1px;\n    }\n    "],
        }),
        __metadata("design:paramtypes", [])
    ], SectionBox);
    return SectionBox;
}());
exports.SectionBox = SectionBox;
var SectionHead = /** @class */ (function () {
    function SectionHead(Parent) {
        this.Left = 10;
        this.Right = 5;
        this.IsLight = false;
        this.IsBlue = false;
        this.height = 27;
        this.border = null;
        if (Parent) {
            Parent.SectionHead = this;
        }
    }
    Object.defineProperty(SectionHead.prototype, "Height", {
        get: function () { return this.height; },
        set: function (value) {
            this.height = value;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(SectionHead.prototype, "Border", {
        get: function () { return this.border; },
        set: function (value) {
            this.border = value;
        },
        enumerable: true,
        configurable: true
    });
    SectionHead = __decorate([
        core_1.Component({
            selector: 'SectionHead',
            inputs: ['Left', 'Right', 'IsLight', 'IsBlue'],
            template: "\n    <div\n        [class.LogitudeSectionHead]=\"!IsLight && !IsBlue\"\n        [class.LogitudeSectionHead2]=\"IsLight\"\n        [class.LogitudeSectionBlueHeader]=\"IsBlue\"\n        [style.height.px]=\"Height\"\n        [style.line-height.px]=\"Height\"\n        [style.padding-left.px]=\"Left\"\n        [style.padding-right.px]=\"Right\"\n        [ngStyle]=\"{border: Border, 'border-bottom': '0px solid transparent'}\"\n    > \n        <ng-content></ng-content>\n    </div>\n    ",
            styles: ["\n    .LogitudeSectionHead {\n        color: #45494A;\n        font-size: 14px;\n        text-align: left;\n        border-radius: 4px 4px 0px 0px;\n        -moz-border-radius: 4px 4px 0px 0px;\n        -webkit-border-radius: 4px 4px 0px 0px;\n        background: -moz-linear-gradient(50% 0% -90deg,rgba(235, 235, 235, 1) 0%,rgba(233, 233, 233, 1) 48.14%,rgba(226, 226, 226, 1) 67.05%,rgba(214, 214, 214, 1) 80.81%,rgba(197, 197, 197, 1) 92.01%,rgba(179, 179, 179, 1) 100%);\n        background: -webkit-linear-gradient(-90deg, rgba(235, 235, 235, 1) 0%, rgba(233, 233, 233, 1) 48.14%, rgba(226, 226, 226, 1) 67.05%, rgba(214, 214, 214, 1) 80.81%, rgba(197, 197, 197, 1) 92.01%, rgba(179, 179, 179, 1) 100%);\n        background: -webkit-gradient(linear,50% 0%,50% 100%,color-stop(0,rgba(235, 235, 235, 1) ),color-stop(0.4814,rgba(233, 233, 233, 1) ),color-stop(0.6705,rgba(226, 226, 226, 1) ),color-stop(0.8081,rgba(214, 214, 214, 1) ),color-stop(0.9201,rgba(197, 197, 197, 1) ),color-stop(1,rgba(179, 179, 179, 1) ));\n        background: -o-linear-gradient(-90deg, rgba(235, 235, 235, 1) 0%, rgba(233, 233, 233, 1) 48.14%, rgba(226, 226, 226, 1) 67.05%, rgba(214, 214, 214, 1) 80.81%, rgba(197, 197, 197, 1) 92.01%, rgba(179, 179, 179, 1) 100%);\n        background: linear-gradient(180deg, rgba(235, 235, 235, 1) 0%, rgba(233, 233, 233, 1) 48.14%, rgba(226, 226, 226, 1) 67.05%, rgba(214, 214, 214, 1) 80.81%, rgba(197, 197, 197, 1) 92.01%, rgba(179, 179, 179, 1) 100%);\n    }\n\n    .LogitudeSectionHead2 {\n        color: #45494A;\n        font-size: 14px;\n        text-align: left;\n        border-radius: 4px 4px 0px 0px;\n        -moz-border-radius: 4px 4px 0px 0px;\n        -webkit-border-radius: 4px 4px 0px 0px;\n        background: -moz-linear-gradient(50% 0% -90deg,rgba(245, 245, 245, 1) 0%,rgba(243, 243, 243, 1) 50.24%,rgba(236, 236, 236, 1) 69.98%,rgba(224, 224, 224, 1) 84.35%,rgba(207, 207, 207, 1) 96.04%,rgba(199, 199, 199, 1) 100%);\n        background: -webkit-linear-gradient(-90deg, rgba(245, 245, 245, 1) 0%, rgba(243, 243, 243, 1) 50.24%, rgba(236, 236, 236, 1) 69.98%, rgba(224, 224, 224, 1) 84.35%, rgba(207, 207, 207, 1) 96.04%, rgba(199, 199, 199, 1) 100%);\n        background: -webkit-gradient(linear,50% 0%,50% 100%,color-stop(0,rgba(245, 245, 245, 1) ),color-stop(0.5024,rgba(243, 243, 243, 1) ),color-stop(0.6998,rgba(236, 236, 236, 1) ),color-stop(0.8435,rgba(224, 224, 224, 1) ),color-stop(0.9604,rgba(207, 207, 207, 1) ),color-stop(1,rgba(199, 199, 199, 1) ));\n        background: -o-linear-gradient(-90deg, rgba(245, 245, 245, 1) 0%, rgba(243, 243, 243, 1) 50.24%, rgba(236, 236, 236, 1) 69.98%, rgba(224, 224, 224, 1) 84.35%, rgba(207, 207, 207, 1) 96.04%, rgba(199, 199, 199, 1) 100%);\n        background: linear-gradient(180deg, rgba(245, 245, 245, 1) 0%, rgba(243, 243, 243, 1) 50.24%, rgba(236, 236, 236, 1) 69.98%, rgba(224, 224, 224, 1) 84.35%, rgba(207, 207, 207, 1) 96.04%, rgba(199, 199, 199, 1) 100%);\n    }\n\n    .LogitudeSectionBlueHeader {\n        color: #282E30;\n        font-size: 12px;\n        text-align: left;\n        border-radius: 4px 4px 0px 0px;\n        -moz-border-radius: 4px 4px 0px 0px;\n        -webkit-border-radius: 4px 4px 0px 0px;\n        background: -moz-linear-gradient(50% 0% -90deg,rgba(235, 243, 255, 1) 0%,rgba(198, 223, 255, 1) 42%,rgba(171, 201, 238, 1) 43%,rgba(208, 232, 255, 1) 100%);\n        background: -webkit-linear-gradient(-90deg, rgba(235, 243, 255, 1) 0%, rgba(198, 223, 255, 1) 42%, rgba(171, 201, 238, 1) 43%, rgba(208, 232, 255, 1) 100%);\n        background: -o-linear-gradient(-90deg, rgba(235, 243, 255, 1) 0%, rgba(198, 223, 255, 1) 42%, rgba(171, 201, 238, 1) 43%, rgba(208, 232, 255, 1) 100%);\n        background: linear-gradient(180deg, rgba(235, 243, 255, 1) 0%, rgba(198, 223, 255, 1) 42%, rgba(171, 201, 238, 1) 43%, rgba(208, 232, 255, 1) 100%);\n    }\n    "],
        }),
        __metadata("design:paramtypes", [SectionBox])
    ], SectionHead);
    return SectionHead;
}());
exports.SectionHead = SectionHead;
var SectionBody = /** @class */ (function () {
    function SectionBody() {
    }
    SectionBody = __decorate([
        core_1.Component({
            selector: 'SectionBody',
            template: '<ng-content></ng-content>',
        })
    ], SectionBody);
    return SectionBody;
}());
exports.SectionBody = SectionBody;
//# sourceMappingURL=SectionBox.js.map