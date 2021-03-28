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
export var PrivateChangePasswordComponent = (function (_super) {
    __extends(PrivateChangePasswordComponent, _super);
    function PrivateChangePasswordComponent(ss, ll) {
        _super.call(this, ss, ll);
        this.ss = ss;
        this.ll = ll;
        this.BackgroundImage = "";
        this.ForgetPasswordImage = "";
        this.MainLogo = "";
        this.ContactUsEmail = "";
    }
    PrivateChangePasswordComponent.prototype.ngOnInit = function () {
        this.privateUrl = SessionInfo.GetLogitudeURL();
        this.GetPrivateLabelsData();
    };
    PrivateChangePasswordComponent.prototype.GetPrivateLabelsData = function () {
        this.BackgroundImage = BrandingDataService.GetBackgroundImage();
        this.ForgetPasswordImage = BrandingDataService.GetForgetPasswordImage();
        this.MainLogo = BrandingDataService.GetMainLogo();
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
    ];
    return PrivateChangePasswordComponent;
}(ChangePasswordComponent));
//# sourceMappingURL=PrivateChangePasswordComponent.js.map