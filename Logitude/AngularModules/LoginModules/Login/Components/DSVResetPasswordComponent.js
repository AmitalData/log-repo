var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
import { Component } from '@angular/core';
import { LoginService } from '../LoginService';
import { SessionInfo } from '../SessionInfo';
import { ResetPasswordComponent } from './ResetPasswordComponent';
import { HybridLabelsBrandingDataService } from '../HybridLabels/Services/HybridLabelsBrandingDataService';
import { BrandingDataService } from '../HybridLabels/Services/BrandingDataService';
export var DSVResetPasswordComponent = (function (_super) {
    __extends(DSVResetPasswordComponent, _super);
    function DSVResetPasswordComponent(ss, hybridLabelsBrandingDataService) {
        _super.call(this, ss);
        this.ss = ss;
        this.hybridLabelsBrandingDataService = hybridLabelsBrandingDataService;
        this.MainColor = null;
        this.BackgroundImage = "";
        this.ForgetPasswordImage = "";
        this.Id = "";
        this.MainLogo = "";
        this.ContactUsEmail = "";
        this.showSpinner = true;
        this.show = true;
    }
    DSVResetPasswordComponent.prototype.ngOnInit = function () {
        this.privateUrl = SessionInfo.GetLogitudeURL();
        this.GetHybridLabelsData(this.privateUrl);
    };
    DSVResetPasswordComponent.prototype.GetHybridLabelsData = function (privateUrl) {
        var _this = this;
        this.hybridLabelsBrandingDataService.GetUserDashboardBrandingData(BrandingDataService.GetHybridLabelsDataRequest(privateUrl)).subscribe(function (response) {
            if (response.Result) {
                //BrandingDataService.SetHybridLabelsDataRequest(response.Result, privateUrl);
                _this.ContactUsEmail = response.Result.ContactUsEmail;
                _this.MainColor = response.Result.MainColor;
                _this.BackgroundImage = BrandingDataService.GetBackgroundImage();
                _this.ForgetPasswordImage = BrandingDataService.GetForgetPasswordImage();
                _this.Id = response.Result.Id;
                _this.MainLogo = BrandingDataService.GetMainLogo();
            }
        }, function (error) {
            _this.BackgroundImage = BrandingDataService.DefaultBackground;
            _this.ForgetPasswordImage = BrandingDataService.DefaultForgetPassword;
            _this.MainLogo = BrandingDataService.DefaultMainLogo;
        });
        this.showSpinner = false;
    };
    DSVResetPasswordComponent.decorators = [
        { type: Component, args: [{
                    selector: 'DSVResetPasswordComponent',
                    moduleId: './Login/Components/',
                    templateUrl: 'DSVResetPasswordComponent.html',
                    styleUrls: ['ChangePasswordComponent.css']
                },] },
    ];
    /** @nocollapse */
    DSVResetPasswordComponent.ctorParameters = [
        { type: LoginService, },
        { type: HybridLabelsBrandingDataService, },
    ];
    return DSVResetPasswordComponent;
}(ResetPasswordComponent));
//# sourceMappingURL=DSVResetPasswordComponent.js.map