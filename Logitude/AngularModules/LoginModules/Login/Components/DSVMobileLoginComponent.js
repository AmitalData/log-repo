var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
import { Component } from '@angular/core';
import { LoginService } from '../LoginService';
import { SessionInfo } from '../SessionInfo';
import { LoginComponent } from './LoginComponent';
import { Tools } from '../Utilities/Tools';
export var DSVMobileLoginComponent = (function (_super) {
    __extends(DSVMobileLoginComponent, _super);
    function DSVMobileLoginComponent(ss) {
        _super.call(this, ss);
        this.ss = ss;
    }
    DSVMobileLoginComponent.prototype.ngOnInit = function () {
        console.log("ngOnInit");
        this.get_cookie_data();
    };
    DSVMobileLoginComponent.prototype.ClearLocation = function () {
        if (SessionInfo.MainLocation) {
            SessionInfo.MainLocation.clear();
        }
    };
    DSVMobileLoginComponent.prototype.ForgotPasswordClicked = function () {
        this.ClearLocation();
        Tools.DynamicLoader.Load("./Login/Components/DSVResetPasswordComponent", SessionInfo.MainLocation)
            .then(function (cmpRef) {
        });
    };
    DSVMobileLoginComponent.decorators = [
        { type: Component, args: [{
                    selector: 'DSVMobileLoginComponent',
                    moduleId: './Login/Components/',
                    templateUrl: 'DSVMobileLoginComponent.html',
                    styleUrls: ['DSVMobileLoginComponent.css']
                },] },
    ];
    /** @nocollapse */
    DSVMobileLoginComponent.ctorParameters = [
        { type: LoginService, },
    ];
    return DSVMobileLoginComponent;
}(LoginComponent));
//# sourceMappingURL=DSVMobileLoginComponent.js.map