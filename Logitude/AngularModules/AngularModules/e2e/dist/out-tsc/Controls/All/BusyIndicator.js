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
/// <reference path="../../infrastructure/utilities/amitalgatewayutil.ts" />
var core_1 = require("@angular/core");
var AmitalGatewayUtil_1 = require("../../Infrastructure/Utilities/AmitalGatewayUtil");
var BusyIndicator = /** @class */ (function () {
    function BusyIndicator() {
        this.Width = 200;
        this.Height = 120;
        this.ImageWidth = 50;
        this.ImageHeight = 50;
        this.IsAmitalVer = false;
        this.isBusy = false;
        this._TimeSpan = 500;
        this._Delta = 20;
        this.progressValue = 0;
        this.IsAmitalVer = AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.AmitalBrowserInUse;
        //this.IsAmitalVer = false;
    }
    Object.defineProperty(BusyIndicator.prototype, "IsBusy", {
        get: function () { return this.isBusy; },
        set: function (value) {
            var _this = this;
            if (this.isBusy != value) {
                this.isBusy = value;
            }
            if (!this.IsAmitalVer) {
                return;
            }
            if (this.isBusy) {
                this._TimerToken =
                    setTimeout(function () {
                        _this.AnimateIt();
                    }, this._TimeSpan);
            }
            else {
                clearTimeout(this._TimerToken);
            }
        },
        enumerable: true,
        configurable: true
    });
    BusyIndicator.prototype.AnimateIt = function () {
        var _this = this;
        //if (this.progressValue > 100) {
        //    this._Delta = -10;
        //} else if (this.progressValue < 1) {
        //    this._Delta = 10;
        //}
        if (this.progressValue > 100) {
            this.progressValue = 0;
        }
        this.progressValue = this.progressValue + this._Delta;
        if (this.isBusy) {
            this._TimerToken =
                setTimeout(function () {
                    _this.AnimateIt();
                }, this._TimeSpan);
        }
    };
    BusyIndicator = __decorate([
        core_1.Component({
            selector: 'BusyIndicator',
            inputs: ['Text', 'IsBusy', 'Width', 'Height', 'ImageWidth', 'ImageHeight'],
            ///changeDetection: ChangeDetectionStrategy.OnPush,
            //border: 0;height: 10px;border-radius: 5px;
            template: "\n    <div [hidden]=\"!IsBusy\" class=\"BusyIndicatorControlLayout\" tabindex=\"-1\" contenteditable=\"false\"></div>\n    <div [hidden]=\"!IsBusy\" class=\"BusyIndicatorControl\" Id=\"BusyIndecator\">\n        <div class=\"BusyIndicatorControlOuter\" [style.width.px]=\"Width\" [style.height.px]=\"Height\">\n            <div class=\"BusyIndicatorControlInner\" [style.width.px]=\"Width\" [style.height.px]=\"Height\">\n                <div style=\"margin: auto; margin-top: 20px;\" [style.width.px]=\"ImageWidth\" [style.height.px]=\"ImageHeight\">\n                    <img *ngIf=\"!IsAmitalVer\" src=\"./Images/BusyIndicator.gif\" alt=\"Loading...\" [style.width.px]=\"ImageWidth\" [style.height.px]=\"ImageHeight\"/> \n                    <progress  *ngIf=\"IsAmitalVer\" value=\"{{progressValue}}\" max=\"100\" style=\"margin-right: -35px;\" ></progress>\n                      \n\n                </div>\n\n                <div style=\"margin-top: 15px; height: 20px; width: 100%; text-align: center;\">\n                    <label>{{Text}}</label>\n                </div>\n            </div>\n        </div>\n    </div>\n    ",
            styles: ["\n    .BusyIndicatorControlLayout {\n        position: absolute;\n        top: 0;\n        bottom: 0;\n        left: 0;\n        right: 0;\n        margin: auto;\n        opacity: 0.5;\n        background: white;\n        z-index: 99;\n    }\n\n    .BusyIndicatorControl {\n        position: absolute;\n        top: 0;\n        bottom: 0;\n        left: 0;\n        right: 0;\n        margin: auto;\n        z-index: 100;\n    }\n\n    .BusyIndicatorControlOuter {\n        border: 1px solid #DADADA;\n        border-radius: 2px;\n        -moz-border-radius: 2px;\n        -webkit-border-radius: 2px;\n        background: white;\n        position: absolute;\n        top: 0;\n        bottom: 0;\n        left: 0;\n        right: 0;\n        margin: auto;\n    }\n\n    .BusyIndicatorControlInner {\n        border-radius: 2px;\n        -moz-border-radius: 2px;\n        -webkit-border-radius: 2px;\n        background: linear-gradient(rgba(240, 240, 240, 0.5), rgba(198, 198, 198, 0.5));\n        position: absolute;\n        top: 0;\n        bottom: 0;\n        left: 0;\n        right: 0;\n        margin: auto;\n    }\n    "],
        }),
        __metadata("design:paramtypes", [])
    ], BusyIndicator);
    return BusyIndicator;
}());
exports.BusyIndicator = BusyIndicator;
//# sourceMappingURL=BusyIndicator.js.map