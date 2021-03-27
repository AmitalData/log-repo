var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
import { Component } from '@angular/core';
import { LoginComponent } from '../../Components/LoginComponent';
import { LoginService } from '../../LoginService';
import { SessionInfo } from '../../SessionInfo';
import { Tools } from '../../Utilities/Tools';
import { BrandingDataService } from '../Services/BrandingDataService';
import { PrivateLabelsBrandingDataService } from '../Services/PrivateLabelsBrandingDataService';
export var PrivateLoginComponent = (function (_super) {
    __extends(PrivateLoginComponent, _super);
    function PrivateLoginComponent(ss, privateLabelsBrandingDataService) {
        _super.call(this, ss);
        this.ss = ss;
        this.privateLabelsBrandingDataService = privateLabelsBrandingDataService;
        this.MainColor = null;
        this.BackgroundImage = "";
        this.MainImage = "";
        this.MainLogo = "";
        this.SmallLogo = "";
        this.showSpinner = true;
    }
    PrivateLoginComponent.prototype.ngOnInit = function () {
        this.get_cookie_data();
        this.privateUrl = SessionInfo.GetLogitudeURL();
        this.GetPrivateLabelsData(this.privateUrl);
    };
    PrivateLoginComponent.prototype.GetPrivateLabelsData = function (privateUrl) {
        var _this = this;
        this.privateLabelsBrandingDataService.GetUserDashboardBrandingData(BrandingDataService.GetPrivateLabelsDataRequest(privateUrl)).subscribe(function (response) {
            if (response.Result) {
                _this.Tenant = response.Result.Tenant;
                BrandingDataService.SetPrivateLabelsDataRequest(response.Result, privateUrl);
                _this.MainColor = response.Result.MainColor;
                _this.BackgroundImage = BrandingDataService.GetBackgroundImage();
                _this.MainImage = BrandingDataService.GetMainImage();
                _this.MainLogo = BrandingDataService.GetMainLogo();
                _this.SmallLogo = BrandingDataService.GetSmallLogo();
            }
        }, function (error) {
            _this.BackgroundImage = BrandingDataService.DefaultBackground;
            _this.MainImage = BrandingDataService.DefaultMainImage;
            _this.MainLogo = BrandingDataService.DefaultMainLogo;
        });
        this.showSpinner = false;
    };
    PrivateLoginComponent.prototype.ClearLocation = function () {
        if (SessionInfo.MainLocation) {
            SessionInfo.MainLocation.clear();
        }
    };
    PrivateLoginComponent.prototype.ForgotPasswordClicked = function () {
        this.ClearLocation();
        Tools.DynamicLoader.Load("/Login/PrivateLabels/LoginComponents/PrivateChangePasswordComponent", SessionInfo.MainLocation)
            .then(function (cmpRef) {
        });
    };
    PrivateLoginComponent.decorators = [
        { type: Component, args: [{
                    selector: 'PrivateLoginComponent',
                    moduleId: './Login/Components/',
                    templateUrl: 'PrivateLoginComponent.html',
                    styleUrls: ['PrivateLoginComponent.css']
                },] },
    ];
    /** @nocollapse */
    PrivateLoginComponent.ctorParameters = [
        { type: LoginService, },
        { type: PrivateLabelsBrandingDataService, },
    ];
    return PrivateLoginComponent;
}(LoginComponent));
//# sourceMappingURL=PrivateLoginComponent.js.map