var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
import { Component } from '@angular/core';
import { LoginComponent } from '../../Components/LoginComponent';
import { LoginService } from '../../LoginService';
import { BrandingDataService } from '../Services/BrandingDataService';
export var HybridLoginProcessComponent = (function (_super) {
    __extends(HybridLoginProcessComponent, _super);
    function HybridLoginProcessComponent(ss) {
        _super.call(this, ss);
        this.ss = ss;
        this.MainColor = null;
        this.BackgroundImage = "";
        this.ForgetPasswordImage = "";
        this.LoginProgressImage = "";
        this.Id = "";
        this.MainLogo = "";
        this.ContactUsEmail = "";
        this.show = true;
    }
    HybridLoginProcessComponent.prototype.ngOnInit = function () {
        this.GetHybridLabelsData();
    };
    HybridLoginProcessComponent.prototype.GetHybridLabelsData = function () {
        this.BackgroundImage = BrandingDataService.GetBackgroundImageFromStorage();
        this.MainLogo = BrandingDataService.GetMainLogoFromStorage();
        this.LoginProgressImage = BrandingDataService.GetLoginProgressFromStorage();
    };
    HybridLoginProcessComponent.decorators = [
        { type: Component, args: [{
                    selector: 'HybridLoginProcessComponent',
                    moduleId: './Login/Components/',
                    templateUrl: 'HybridLoginProcessComponent.html',
                    styleUrls: ['HybridLoginProcessComponent.css']
                },] },
    ];
    /** @nocollapse */
    HybridLoginProcessComponent.ctorParameters = [
        { type: LoginService, },
    ];
    return HybridLoginProcessComponent;
}(LoginComponent));
//# sourceMappingURL=HybridLoginProcessComponent.js.map