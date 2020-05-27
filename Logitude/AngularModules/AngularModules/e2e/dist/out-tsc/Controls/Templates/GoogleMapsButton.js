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
var ServiceLocator_1 = require("../../Infrastructure/Locators/ServiceLocator");
var GoogleMapsButton = /** @class */ (function () {
    function GoogleMapsButton() {
        this.IsVisible = false;
        this.addressList = null;
        this.addressString = null;
    }
    Object.defineProperty(GoogleMapsButton.prototype, "AddressList", {
        get: function () { return this.addressList; },
        set: function (value) {
            if (this.addressList != value) {
                this.addressList = value;
                this.SetIsVisible();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GoogleMapsButton.prototype, "AddressString", {
        get: function () { return this.addressString; },
        set: function (value) {
            if (this.addressString != value) {
                this.addressString = value;
                this.SetIsVisible();
            }
        },
        enumerable: true,
        configurable: true
    });
    GoogleMapsButton.prototype.SetIsVisible = function () {
        var isVisible = false;
        if (this.AddressList != null) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.AddressList.CountryCode)) {
                isVisible = true;
            }
        }
        else if (!Tools_1.AppTool.IsNullOrEmpty(this.AddressString)) {
            isVisible = true;
        }
        this.IsVisible = isVisible;
    };
    GoogleMapsButton.prototype.OnClick = function () {
        if (this.AddressList != null || !Tools_1.AppTool.IsNullOrEmpty(this.AddressString)) {
            ServiceLocator_1.ServiceLocator.SendTotangoUserActivity("GoogleMaps", "Google maps search click");
            var myString = this.AddressString;
            if (this.AddressList != null) {
                myString = this.AddressList.Address1 + " " + this.AddressList.City + " " + (this.AddressList.StateName != null ? (this.AddressList.StateName + " ") : "") + this.AddressList.CountryName;
            }
            var link = Tools_1.AppTool.GetLogitudeURL() + "/WebPages/GoogleMap.aspx?address=" + myString;
            var win = window.open(link, '_blank');
            win.focus();
        }
    };
    GoogleMapsButton = __decorate([
        core_1.Component({
            selector: "GoogleMapsButton",
            inputs: ['AddressList', 'AddressString'],
            template: "\n    <div *ngIf=\"IsVisible\" style=\"width: 20px; height: 15px; position: relative; cursor:pointer;\" title=\"Google Maps\" (click)=\"OnClick()\">\n        <img src=\"./Images/GooglePin.png\" style=\"width: 20px; height: 15px;\"/>\n    </div> \n    ",
        }),
        __metadata("design:paramtypes", [])
    ], GoogleMapsButton);
    return GoogleMapsButton;
}());
exports.GoogleMapsButton = GoogleMapsButton;
//# sourceMappingURL=GoogleMapsButton.js.map