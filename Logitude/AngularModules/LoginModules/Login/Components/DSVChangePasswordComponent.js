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
export var DSVChangePasswordComponent = (function (_super) {
    __extends(DSVChangePasswordComponent, _super);
    function DSVChangePasswordComponent(ss, ll) {
        _super.call(this, ss, ll);
        this.ss = ss;
        this.ll = ll;
        this.BackgroundImage = "";
        this.ForgetPasswordImage = "";
        this.MainLogo = "";
    }
    DSVChangePasswordComponent.prototype.ngOnInit = function () {
        this.GetPrivateLabelsData();
    };
    DSVChangePasswordComponent.prototype.GetPrivateLabelsData = function () {
        this.BackgroundImage = BrandingDataService.GetImage("BackgroundImage");
        this.MainLogo = BrandingDataService.GetImage("MainLogo");
        this.ForgetPasswordImage = BrandingDataService.GetImage("ForgetPasswordImage");
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
    ];
    return DSVChangePasswordComponent;
}(ChangePasswordComponent));
//# sourceMappingURL=DSVChangePasswordComponent.js.map