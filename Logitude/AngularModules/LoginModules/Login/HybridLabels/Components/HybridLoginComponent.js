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
import { HybridLabelsBrandingDataService } from '../Services/HybridLabelsBrandingDataService';
export var HybridLoginComponent = (function (_super) {
    __extends(HybridLoginComponent, _super);
    function HybridLoginComponent(ss, hybridLabelsBrandingDataService) {
        _super.call(this, ss);
        this.ss = ss;
        this.hybridLabelsBrandingDataService = hybridLabelsBrandingDataService;
        this.MainColor = null;
        this.BackgroundImage = "";
        this.MainImage = "";
        this.MainLogo = "";
        this.SmallLogo = "";
        this.showSpinner = true;
    }
    HybridLoginComponent.prototype.ngOnInit = function () {
        this.get_cookie_data();
        this.privateUrl = SessionInfo.GetLogitudeURL();
        this.GetHybridLabelsData(this.privateUrl);
    };
    HybridLoginComponent.prototype.GetHybridLabelsData = function (privateUrl) {
        var _this = this;
        this.hybridLabelsBrandingDataService.GetUserDashboardBrandingData(BrandingDataService.GetHybridLabelsDataRequest(privateUrl)).subscribe(function (response) {
            if (response.Result) {
                _this.Tenant = response.Result.Tenant;
                BrandingDataService.SetHybridLabelsDataRequest(response.Result, privateUrl);
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
    HybridLoginComponent.prototype.ClearLocation = function () {
        if (SessionInfo.MainLocation) {
            SessionInfo.MainLocation.clear();
        }
    };
    HybridLoginComponent.prototype.ForgotPasswordClicked = function () {
        this.ClearLocation();
        Tools.DynamicLoader.Load("/Login/HybridLabels/Components/HybridResetPasswordComponent", SessionInfo.MainLocation)
            .then(function (cmpRef) {
        });
    };
    HybridLoginComponent.decorators = [
        { type: Component, args: [{
                    selector: 'HybridLoginComponent',
                    moduleId: './Login/Components/',
                    templateUrl: 'HybridLoginComponent.html',
                    styleUrls: ['HybridLoginComponent.css']
                },] },
    ];
    /** @nocollapse */
    HybridLoginComponent.ctorParameters = [
        { type: LoginService, },
        { type: HybridLabelsBrandingDataService, },
    ];
    return HybridLoginComponent;
}(LoginComponent));
//# sourceMappingURL=HybridLoginComponent.js.map