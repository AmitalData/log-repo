var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
import { Component } from '@angular/core';
import { ResetPasswordComponent } from '../../Components/ResetPasswordComponent';
import { LoginService } from '../../LoginService';
import { SessionInfo } from '../../SessionInfo';
import { BrandingDataService } from '../Services/BrandingDataService';
import { PrivateLabelsBrandingDataService } from '../Services/PrivateLabelsBrandingDataService';
export var PrivateResetPasswordComponent = (function (_super) {
    __extends(PrivateResetPasswordComponent, _super);
    function PrivateResetPasswordComponent(ss, privateLabelsBrandingDataService) {
        _super.call(this, ss);
        this.ss = ss;
        this.privateLabelsBrandingDataService = privateLabelsBrandingDataService;
        this.MainColor = null;
        this.BackgroundImage = "";
        this.ForgetPasswordImage = "";
        this.Id = "";
        this.MainLogo = "";
        this.ContactUsEmail = "";
        this.showSpinner = true;
        this.show = true;
    }
    PrivateResetPasswordComponent.prototype.ngOnInit = function () {
        this.privateUrl = SessionInfo.GetLogitudeURL();
        this.GetPrivateLabelsData(this.privateUrl);
    };
    PrivateResetPasswordComponent.prototype.GetPrivateLabelsData = function (privateUrl) {
        var _this = this;
        this.privateLabelsBrandingDataService.GetUserDashboardBrandingData(BrandingDataService.GetPrivateLabelsDataRequest(privateUrl)).subscribe(function (response) {
            if (response.Result) {
                //BrandingDataService.SetPrivateLabelsDataRequest(response.Result, privateUrl);
                _this.ContactUsEmail = response.Result.ContactUsEmail;
                _this.MainColor = response.Result.MainColor;
                _this.BackgroundImage = BrandingDataService.GetBackgroundImage();
                _this.ForgetPasswordImage = BrandingDataService.GetForgetPasswordImage();
                _this.MainLogo = BrandingDataService.GetMainLogo();
            }
        }, function (error) {
            _this.BackgroundImage = BrandingDataService.DefaultBackground;
            _this.ForgetPasswordImage = BrandingDataService.DefaultForgetPassword;
            _this.MainLogo = BrandingDataService.DefaultMainLogo;
        });
        this.showSpinner = false;
    };
    PrivateResetPasswordComponent.decorators = [
        { type: Component, args: [{
                    selector: 'PrivateResetPasswordComponent',
                    moduleId: './Login/Components/',
                    templateUrl: 'PrivateResetPasswordComponent.html',
                    styleUrls: ['PrivateResetPasswordComponent.css']
                },] },
    ];
    /** @nocollapse */
    PrivateResetPasswordComponent.ctorParameters = [
        { type: LoginService, },
        { type: PrivateLabelsBrandingDataService, },
    ];
    return PrivateResetPasswordComponent;
}(ResetPasswordComponent));
//# sourceMappingURL=PrivateResetPasswordComponent.js.map