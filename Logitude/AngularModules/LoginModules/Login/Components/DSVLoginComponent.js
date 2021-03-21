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
import { PrivateLabelsBrandingDataService } from '../PrivateLabels/Services/PrivateLabelsBrandingDataService';
import { BrandingDataService } from '../PrivateLabels/Services/BrandingDataService';
export var DSVLoginComponent = (function (_super) {
    __extends(DSVLoginComponent, _super);
    function DSVLoginComponent(ss, privateLabelsBrandingDataService) {
        _super.call(this, ss);
        this.ss = ss;
        this.privateLabelsBrandingDataService = privateLabelsBrandingDataService;
        this.MainColor = null;
        this.BackgroundImage = "";
        this.MainImage = "";
        this.MainLogo = "";
        this.LoginProcessImage = "";
        this.showSpinner = true;
    }
    DSVLoginComponent.prototype.ngOnInit = function () {
        this.get_cookie_data();
        this.privateUrl = SessionInfo.GetLogitudeURL();
        this.GetPrivateLabelsData(this.privateUrl);
        this.showSpinner = false;
    };
    DSVLoginComponent.prototype.GetPrivateLabelsData = function (privateUrl) {
        var _this = this;
        this.privateLabelsBrandingDataService.GetUserDashboardBrandingData(BrandingDataService.GetPrivateLabelsDataRequest(privateUrl)).subscribe(function (response) {
            if (response.Result) {
                _this.Tenant = response.Result.Tenant;
                BrandingDataService.SetPrivateLabelsDataRequest(response.Result, privateUrl);
                _this.MainColor = response.Result.MainColor;
                _this.BackgroundImage = BrandingDataService.GetBackgroundImage();
                _this.MainImage = BrandingDataService.GetMainImage();
                _this.MainLogo = BrandingDataService.GetMainLogo();
                _this.LoginProcessImage = BrandingDataService.GetLoginProgressImage();
            }
        }, function (error) {
            _this.BackgroundImage = BrandingDataService.DefaultBackground;
            _this.MainImage = BrandingDataService.DefaultMainImage;
            _this.MainLogo = BrandingDataService.DefaultMainLogo;
        });
        this.showSpinner = false;
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
        { type: PrivateLabelsBrandingDataService, },
    ];
    return DSVLoginComponent;
}(LoginComponent));
//# sourceMappingURL=DSVLoginComponent.js.map