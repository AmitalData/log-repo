var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
import { Component } from '@angular/core';
import { LoginService } from '../LoginService';
import { PasswordChangeService } from '../PasswordChangeService';
import { ChangePasswordComponent } from './ChangePasswordComponent';
import { BrandingDataService } from '../PrivateLabels/Services/BrandingDataService';
import { SessionInfo } from '../SessionInfo';
import { PrivateLabelsBrandingDataService } from '../PrivateLabels/Services/PrivateLabelsBrandingDataService';
export var DSVChangePasswordComponent = (function (_super) {
    __extends(DSVChangePasswordComponent, _super);
    function DSVChangePasswordComponent(ss, ll, privateLabelsBrandingDataService) {
        _super.call(this, ss, ll);
        this.ss = ss;
        this.ll = ll;
        this.privateLabelsBrandingDataService = privateLabelsBrandingDataService;
        this.BackgroundImage = "";
        this.ForgetPasswordImage = "";
        this.MainLogo = "";
        this.MainColor = null;
    }
    DSVChangePasswordComponent.prototype.ngOnInit = function () {
        this.privateUrl = SessionInfo.GetLogitudeURL();
        this.GetImagesFromCash();
        this.GetPrivateLabelsImages(this.privateUrl);
    };
    DSVChangePasswordComponent.prototype.GetImagesFromCash = function () {
        this.BackgroundImage = BrandingDataService.GetImage("BackgroundImage");
        this.MainLogo = BrandingDataService.GetImage("MainLogo");
        this.ForgetPasswordImage = BrandingDataService.GetImage("ForgetPasswordImage");
        this.MainColor = BrandingDataService.GetColor("MainColor");
    };
    DSVChangePasswordComponent.prototype.GetPrivateLabelsImages = function (privateUrl) {
        var _this = this;
        this.privateLabelsBrandingDataService.GetUserDashboardBrandingData(BrandingDataService.GetPrivateLabelsDataRequest(privateUrl)).subscribe(function (response) {
            if (response.Result) {
                BrandingDataService.SetPrivateLabelsDataRequest(response.Result, privateUrl);
                _this.GetImagesFromCash();
            }
        });
    };
    DSVChangePasswordComponent.decorators = [
        { type: Component, args: [{
                    selector: 'DSVChangePasswordComponent',
                    moduleId: './Login/Components/',
                    templateUrl: 'DSVChangePasswordComponent.html',
                    styleUrls: ['ChangePasswordComponent.css']
                },] },
    ];
    /** @nocollapse */
    DSVChangePasswordComponent.ctorParameters = [
        { type: PasswordChangeService, },
        { type: LoginService, },
        { type: PrivateLabelsBrandingDataService, },
    ];
    return DSVChangePasswordComponent;
}(ChangePasswordComponent));
//# sourceMappingURL=DSVChangePasswordComponent.js.map