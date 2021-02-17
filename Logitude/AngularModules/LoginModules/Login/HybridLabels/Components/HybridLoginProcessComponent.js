var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
import { Component } from '@angular/core';
import { LoginComponent } from '../../Components/LoginComponent';
import { LoginService } from '../../LoginService';
import { SessionInfo } from '../../SessionInfo';
import { BrandingDataService } from '../Services/BrandingDataService';
import { HybridLabelsBrandingDataService } from '../Services/HybridLabelsBrandingDataService';
export var HybridLoginProcessComponent = (function (_super) {
    __extends(HybridLoginProcessComponent, _super);
    function HybridLoginProcessComponent(ss, hybridLabelsBrandingDataService) {
        _super.call(this, ss);
        this.ss = ss;
        this.hybridLabelsBrandingDataService = hybridLabelsBrandingDataService;
        this.MainColor = null;
        this.BackgroundImage = "";
        this.ForgetPasswordImage = "";
        this.LoginProgressImage = "";
        this.Id = "";
        this.MainLogo = "";
        this.ContactUsEmail = "";
        this.showSpinner = true;
        this.show = true;
    }
    HybridLoginProcessComponent.prototype.ngOnInit = function () {
        this.privateUrl = SessionInfo.GetLogitudeURL();
        this.GetHybridLabelsData(this.privateUrl);
    };
    HybridLoginProcessComponent.prototype.GetHybridLabelsData = function (privateUrl) {
        var _this = this;
        this.hybridLabelsBrandingDataService.GetUserDashboardBrandingData(BrandingDataService.GetHybridLabelsDataRequest(privateUrl)).subscribe(function (response) {
            if (response.Result) {
                BrandingDataService.SetHybridLabelsDataRequest(response.Result, privateUrl);
                _this.ContactUsEmail = response.Result.ContactUsEmail;
                _this.MainColor = response.Result.MainColor;
                _this.BackgroundImage = BrandingDataService.GetBackgroundImage();
                _this.MainLogo = BrandingDataService.GetMainLogo();
                _this.LoginProgressImage = BrandingDataService.GetLoginProgressImage();
                console.log(_this.LoginProgressImage);
                _this.showSpinner = false;
            }
        });
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
        { type: HybridLabelsBrandingDataService, },
    ];
    return HybridLoginProcessComponent;
}(LoginComponent));
//# sourceMappingURL=HybridLoginProcessComponent.js.map