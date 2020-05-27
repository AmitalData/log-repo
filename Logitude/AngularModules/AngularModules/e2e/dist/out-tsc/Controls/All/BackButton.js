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
var ObjectsLocator_1 = require("../../Infrastructure/Locators/ObjectsLocator");
var BackButton = /** @class */ (function () {
    function BackButton() {
        this.Text = "Back";
        this.IsHover = false;
        this.LayoutDirection = 'ltr';
        this.isEnabled = true;
        this.LayoutDirection = ObjectsLocator_1.ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection;
    }
    Object.defineProperty(BackButton.prototype, "IsEnabled", {
        get: function () { return this.isEnabled; },
        set: function (value) {
            if (this.isEnabled != value) {
                this.isEnabled = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    BackButton.prototype.ngOnInit = function () {
        if (!this.Text) {
            this.Text = "Back";
        }
    };
    BackButton = __decorate([
        core_1.Component({
            selector: 'BackButton',
            inputs: ['Text', 'IsEnabled'],
            changeDetection: core_1.ChangeDetectionStrategy.OnPush,
            template: "\n    <div id=\"{{ 'EditBackbutton' | IdGeneratorPipe}}\" class=\"BackBottun\" (mouseover)=\"IsHover = true\" (mouseleave)=\"IsHover = false\"\n        [ngStyle]=\"LayoutDirection == 'rtl' ? {'padding-right': '10px'} : {'padding-left': '10px'}\">\n        <div class=\"BackBottonBody\" [ngStyle]=\"LayoutDirection == 'rtl' ? {'border-right': 'none'} : {'border-left': 'none'}\">{{Text}}</div>\n\n        <div class=\"BackBottonHead\" [className]=\"LayoutDirection == 'rtl' ? 'BackBottonHead FlipImgHoriz' : 'BackBottonHead'\" [ngStyle]=\"LayoutDirection == 'rtl' ? {'right': '0'} : {'left': '0'}\">\n            <img src=\"./Images/Buttons/BackBottonHead.png\" style=\"width: 15px; height: 24px;\" [hidden]=\"IsHover\" />\n            <img src=\"./Images/Buttons/BackBottonHeadHover.png\" style=\"width: 15px; height: 24px;\" [hidden]=\"!IsHover\" />\n        </div>\n    </div>\n    ",
            styles: ["\n    .disabled{\n        pointer-events: none;\n        opacity: 0.7;\n        cursor: default;\n    }\n    .BackBottun:hover .BackBottonBody {\n        border-color: #EDC093;\n        background: -moz-linear-gradient(50% 0% -90deg,rgba(255, 255, 255, 1) 0%,rgba(237, 192, 147, 1) 100%);\n        background: -webkit-linear-gradient(-90deg, rgba(255, 255, 255, 1) 0%, rgba(237, 192, 147, 1) 100%);\n        background: -webkit-gradient(linear,50% 0%,50% 100%,color-stop(0,rgba(255, 255, 255, 1) ),color-stop(1,rgba(237, 192, 147, 1) ));\n        background: -o-linear-gradient(-90deg, rgba(255, 255, 255, 1) 0%, rgba(237, 192, 147, 1) 100%);\n        background: linear-gradient(180deg, rgba(255, 255, 255, 1) 0%, rgba(237, 192, 147, 1) 100%);\n        -webkit-box-shadow: 0px 0px 6px 0px #EDC093;\n        -moz-box-shadow: 0px 0px 6px 0px #EDC093;\n        box-shadow: 0px 0px 6px 0px #EDC093;\n        text-shadow: 1px 1px white;\n        color: #EE8F55;\n    }\n\n    .BackBottonBody {\n        height: 24px;\n        width: 100%;\n        min-width: 40px;\n        border: 1px solid #6B8399;\n        border-radius: 5px;     \n        font-size: 12px;\n        font-family: Arial;           \n        background: -moz-linear-gradient(50% 0% -90deg,rgba(255, 255, 255, 1) 0%,rgba(236, 192, 147, 1) 100%);\n        background: -webkit-linear-gradient(-90deg, rgba(255, 255, 255, 1) 0%, rgba(236, 192, 147, 1) 100%);\n        background: -webkit-gradient(linear,50% 0%,50% 100%,color-stop(0,rgba(255, 255, 255, 1) ),color-stop(1,rgba(236, 192, 147, 1) ));\n        background: -o-linear-gradient(-90deg, rgba(255, 255, 255, 1) 0%, rgba(236, 192, 147, 1) 100%);\n        background: linear-gradient(180deg, rgba(255, 255, 255, 1) 0%, rgba(236, 192, 147, 1) 100%);\n        -moz-box-shadow: inset 0 0 3px #AAAAAA;\n        -webkit-box-shadow: inset 0 0 3px #AAAAAA;\n        box-shadow: inset 0 0 3px #AAAAAA;\n        padding-left: 5px;\n        padding-right: 5px;\n        padding-top: 4px;\n    }\n\n    @media all and (-ms-high-contrast: none), (-ms-high-contrast: active) {\n        .BackBottonBody {\n            padding-top: 4.5px;\n        }\n    }\n    .BackBottonHead {\n        height: 24px;\n        width: 15px;\n        min-width: 15px;\n        position: absolute;\n        top: 0;\n    }\n\n    .BackBottun {\n        height: 24px;        \n        width: 100%;\n        cursor: pointer;\n        position: relative;\n    }\n    .FlipImgHoriz{\n        -moz-transform: scaleX(-1);\n        -o-transform: scaleX(-1);\n        -webkit-transform: scaleX(-1);\n        transform: scaleX(-1);\n        filter: FlipH;\n        -ms-filter: \"FlipH\";\n    }\n    "],
        }),
        __metadata("design:paramtypes", [])
    ], BackButton);
    return BackButton;
}());
exports.BackButton = BackButton;
//# sourceMappingURL=BackButton.js.map