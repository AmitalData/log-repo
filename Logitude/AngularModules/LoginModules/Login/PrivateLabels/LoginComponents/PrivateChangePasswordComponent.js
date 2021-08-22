var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
import { Component } from '@angular/core';
import { ChangePasswordComponent } from '../../Components/ChangePasswordComponent';
import { LoginService } from '../../LoginService';
import { PasswordChangeService } from '../../PasswordChangeService';
import { SessionInfo } from '../../SessionInfo';
import { BrandingDataService } from '../Services/BrandingDataService';
import { PrivateLabelsBrandingDataService } from '../Services/PrivateLabelsBrandingDataService';
export var PrivateChangePasswordComponent = (function (_super) {
    __extends(PrivateChangePasswordComponent, _super);
    function PrivateChangePasswordComponent(ss, ll, privateLabelsBrandingDataService) {
        _super.call(this, ss, ll);
        this.ss = ss;
        this.ll = ll;
        this.privateLabelsBrandingDataService = privateLabelsBrandingDataService;
        this.BackgroundImage = "";
        this.ForgetPasswordImage = "";
        this.MainLogo = "";
        this.SecondaryColor = null;
    }
    PrivateChangePasswordComponent.prototype.ngOnInit = function () {
        this.privateUrl = SessionInfo.GetLogitudeURL();
        this.GetImagesFromCash();
        this.GetPrivateLabelsImages(this.privateUrl);
    };
    PrivateChangePasswordComponent.prototype.GetImagesFromCash = function () {
        this.BackgroundImage = BrandingDataService.GetImage("BackgroundImage");
        this.MainLogo = BrandingDataService.GetImage("MainLogo");
        this.ForgetPasswordImage = BrandingDataService.GetImage("ForgetPasswordImage");
        this.SecondaryColor = BrandingDataService.GetColor("SecondaryColor");
    };
    PrivateChangePasswordComponent.prototype.GetPrivateLabelsImages = function (privateUrl) {
        var _this = this;
        this.privateLabelsBrandingDataService.GetUserDashboardBrandingData(BrandingDataService.GetPrivateLabelsDataRequest(privateUrl)).subscribe(function (response) {
            if (response.Result) {
                BrandingDataService.SetPrivateLabelsDataRequest(response.Result, privateUrl);
                _this.GetImagesFromCash();
            }
        });
    };
    PrivateChangePasswordComponent.decorators = [
        { type: Component, args: [{
                    selector: 'PrivateChangePasswordComponent',
                    moduleId: './Login/Components/',
                    templateUrl: 'PrivateChangePasswordComponent.html',
                    styleUrls: ['PrivateChangePasswordComponent.css']
                },] },
    ];
    /** @nocollapse */
    PrivateChangePasswordComponent.ctorParameters = [
        { type: PasswordChangeService, },
        { type: LoginService, },
        { type: PrivateLabelsBrandingDataService, },
    ];
    return PrivateChangePasswordComponent;
}(ChangePasswordComponent));
//# sourceMappingURL=PrivateChangePasswordComponent.js.map