var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
import { Component } from '@angular/core';
import { ChangePasswordComponent } from '../../Components/ChangePasswordComponent';
import { LoginService } from '../../LoginService';
import { PasswordChangeService } from '../../PasswordChangeService';
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
        this.MainColor = null;
    }
    PrivateChangePasswordComponent.prototype.ngOnInit = function () {
        this.GetPrivateLabelsData();
    };
    PrivateChangePasswordComponent.prototype.GetPrivateLabelsData = function () {
        this.BackgroundImage = BrandingDataService.GetImage("BackgroundImage");
        this.MainLogo = BrandingDataService.GetImage("MainLogo");
        this.ForgetPasswordImage = BrandingDataService.GetImage("ForgetPasswordImage");
        this.MainColor = BrandingDataService.GetColor("MainColor");
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