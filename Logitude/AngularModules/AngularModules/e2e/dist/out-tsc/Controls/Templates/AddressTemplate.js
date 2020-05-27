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
var AddressTemplate = /** @class */ (function () {
    function AddressTemplate() {
        this.Height = 70;
        this.Right = null;
        this.Border = "1px solid #AAAAAA";
        this.address = null;
        this.CityLineText = null;
    }
    Object.defineProperty(AddressTemplate.prototype, "Address", {
        get: function () { return this.address; },
        set: function (value) {
            if (this.address != value) {
                this.address = value;
                this.GetCityLineText();
            }
        },
        enumerable: true,
        configurable: true
    });
    AddressTemplate.prototype.GetCityLineText = function () {
        var myResult = null;
        if (this.Address != null) {
            myResult = this.Address.City;
            if (!Tools_1.AppTool.IsNullOrEmpty(this.Address.StateName)) {
                myResult += ", " + this.Address.StateName;
            }
            if (!Tools_1.AppTool.IsNullOrEmpty(this.Address.ZipCode)) {
                myResult += ", " + this.Address.ZipCode;
            }
        }
        this.CityLineText = myResult;
    };
    AddressTemplate = __decorate([
        core_1.Component({
            selector: 'AddressTemplate',
            inputs: ['Address', 'Height', 'Right', 'Border'],
            template: "\n    <table [style.height.px]=\"Height\">\n        <tr>\n            <td>\n                <div class=\"MediaFill\">\n                    <div class=\"LogitudeAddressTemplate\" [ngStyle]= \"{border:Border}\">\n                        <div *ngIf=\"Address\" class=\"MediaFillAbsolute\">\n                            <div class=\"TextTrimming\" *ngIf=\"Address.Address1\">{{Address.Address1}}</div>\n                            <div class=\"TextTrimming\" *ngIf=\"Address.Address2\" style=\"padding-right: 30px;\">{{Address.Address2}}</div>\n                            <div class=\"TextTrimming\" *ngIf=\"CityLineText\">{{CityLineText}}</div>\n                            <div class=\"TextTrimming\" *ngIf=\"Address.CountryName\">{{Address.CountryName}}</div>\n\n                            <img style=\"position: absolute; right:2px; bottom:-2px; width: 32px; height: 32px;\" [attr.src]=\"Address.CountryCode | CountryFlagPipe\" />\n                            <div style=\"width: 20px; height: 15px; position:absolute; right: 10px; bottom: 30px;\">\n                                <GoogleMapsButton [AddressList]=\"Address\"></GoogleMapsButton>\n                            </div>\n                        </div>\n                    </div>\n                </div>\n            </td>\n\n            <td [style.width.px]=\"Right\" *ngIf=\"Right\">\n                <div></div>\n            </td>\n        </tr>\n    </table>  \n    ",
            styles: ["\n    .LogitudeAddressTemplate {\n        position: relative\n        width: 100%;\n        height: 100%;\n        background: #EBEBEB;\n        border: 1px solid #AAAAAA;\n        border-radius: 3px;\n        -moz-border-radius: 3px;\n        -webkit-border-radius: 3px;\n        overflow: hidden;        \n    }\n\n    .LogitudeAddressTemplate div {\n        font-size: 10px;\n        color: #282E30;\n        text-indent: 5px;\n        line-height: 17px;\n    }\n    "],
        }),
        __metadata("design:paramtypes", [])
    ], AddressTemplate);
    return AddressTemplate;
}());
exports.AddressTemplate = AddressTemplate;
//# sourceMappingURL=AddressTemplate.js.map