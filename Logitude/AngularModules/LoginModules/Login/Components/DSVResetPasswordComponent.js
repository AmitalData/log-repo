var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
import { Component } from '@angular/core';
import { LoginService } from '../LoginService';
import { SessionInfo } from '../SessionInfo';
import { ResetPasswordComponent } from './ResetPasswordComponent';
import { BrandingDataService } from '../PrivateLabels/Services/BrandingDataService';
export var DSVResetPasswordComponent = (function (_super) {
    __extends(DSVResetPasswordComponent, _super);
    function DSVResetPasswordComponent(ss) {
        _super.call(this, ss);
        this.ss = ss;
        this.MainColor = null;
        this.BackgroundImage = "";
        this.ForgetPasswordImage = "";
        this.MainLogo = "";
        this.ContactUsEmail = sessionStorage.getItem('ContactEmail');
    }
    DSVResetPasswordComponent.prototype.ngOnInit = function () {
        this.privateUrl = SessionInfo.GetLogitudeURL();
        this.GetPrivateLabelsData();
    };
    DSVResetPasswordComponent.prototype.GetPrivateLabelsData = function () {
        this.BackgroundImage = BrandingDataService.GetBackgroundImage();
        this.ForgetPasswordImage = BrandingDataService.GetForgetPasswordImage();
        this.MainLogo = BrandingDataService.GetMainLogo();
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
    ];
    return DSVResetPasswordComponent;
}(ResetPasswordComponent));
//# sourceMappingURL=DSVResetPasswordComponent.js.map