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
export var DSVLoginComponent = (function (_super) {
    __extends(DSVLoginComponent, _super);
    function DSVLoginComponent(ss) {
        _super.call(this, ss);
        this.ss = ss;
    }
    DSVLoginComponent.prototype.ngOnInit = function () {
        console.log("ngOnInit");
        this.get_cookie_data();
    };
    DSVLoginComponent.prototype.ClearLocation = function () {
        if (SessionInfo.MainLocation) {
            SessionInfo.MainLocation.clear();
        }
    };
    DSVLoginComponent.prototype.ForgotPasswordClicked = function () {
        this.ClearLocation();
        Tools.DynamicLoader.Load("./Login/Components/DSVResetPasswordComponent", SessionInfo.MainLocation)
            .then(function (cmpRef) {
        });
    };
    DSVLoginComponent.decorators = [
        { type: Component, args: [{
                    selector: 'DSVLoginComponent',
                    moduleId: './Login/Components/',
                    templateUrl: 'DSVLoginComponent.html',
                    styleUrls: ['DSVLoginComponent.css']
                },] },
    ];
    /** @nocollapse */
    DSVLoginComponent.ctorParameters = [
        { type: LoginService, },
    ];
    return DSVLoginComponent;
}(LoginComponent));
//# sourceMappingURL=DSVLoginComponent.js.map