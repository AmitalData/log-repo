var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
import { Component } from '@angular/core';
import { ResetPasswordComponent } from '../../Components/ResetPasswordComponent';
import { LoginService } from '../../LoginService';
import { BrandingDataService } from '../Services/BrandingDataService';
export var PrivateResetPasswordComponent = (function (_super) {
    __extends(PrivateResetPasswordComponent, _super);
    function PrivateResetPasswordComponent(ss) {
        _super.call(this, ss);
        this.ss = ss;
        this.BackgroundImage = "";
        this.ForgetPasswordImage = "";
        this.Id = "";
        this.MainLogo = "";
        this.ContactUsEmail = "mailto:" + sessionStorage.getItem('ContactEmail');
        this.MainColor = null;
    }
    PrivateResetPasswordComponent.prototype.ngOnInit = function () {
        this.GetPrivateLabelsData();
    };
    PrivateResetPasswordComponent.prototype.GetPrivateLabelsData = function () {
        this.BackgroundImage = BrandingDataService.GetImage("BackgroundImage");
        this.MainLogo = BrandingDataService.GetImage("MainLogo");
        this.ForgetPasswordImage = BrandingDataService.GetImage("ForgetPasswordImage");
        this.MainColor = BrandingDataService.GetColor("MainColor");
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
    ];
    return PrivateResetPasswordComponent;
}(ResetPasswordComponent));
//# sourceMappingURL=PrivateResetPasswordComponent.js.map