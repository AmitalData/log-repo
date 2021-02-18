var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
import { Component } from '@angular/core';
import { LoginService } from '../LoginService';
import { ResetPasswordComponent } from './ResetPasswordComponent';
export var DSVResetPasswordComponent = (function (_super) {
    __extends(DSVResetPasswordComponent, _super);
    function DSVResetPasswordComponent(ss) {
        _super.call(this, ss);
        this.ss = ss;
    }
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