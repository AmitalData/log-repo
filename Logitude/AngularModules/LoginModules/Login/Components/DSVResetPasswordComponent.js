var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
import { Component } from '@angular/core';
import { LoginService } from '../LoginService';
import { SessionInfo } from '../SessionInfo';
import { ResetPasswordComponent } from './ResetPasswordComponent';
import { Router } from '@angular/router';
import { HybridLabelsBrandingDataService } from '../HybridLabels/Services/HybridLabelsBrandingDataService';
import { BrandingDataService } from '../HybridLabels/Services/BrandingDataService';
export var DSVResetPasswordComponent = (function (_super) {
    __extends(DSVResetPasswordComponent, _super);
    function DSVResetPasswordComponent(router, ss, hybridLabelsBrandingDataService) {
        _super.call(this, ss);
        this.router = router;
        this.ss = ss;
        this.hybridLabelsBrandingDataService = hybridLabelsBrandingDataService;
        this.MainColor = null;
        this.BackgroundImage = "";
        this.ForgetPasswordImage = "";
        this.Id = "";
        this.MainLogo = "";
        this.ContactUsEmail = "";
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
                _this.MainLogo = response.Result.MainLogo;
                _this.MainLogo = BrandingDataService.GetMainLogo();
            }
            else {
                _this.GoToError401();
            }
        });
    };
    DSVResetPasswordComponent.prototype.GoToError401 = function () {
        this.router.navigate(['Error401']);
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
        { type: Router, },
        { type: LoginService, },
        { type: HybridLabelsBrandingDataService, },
    ];
    return DSVResetPasswordComponent;
}(ResetPasswordComponent));
//# sourceMappingURL=DSVResetPasswordComponent.js.map