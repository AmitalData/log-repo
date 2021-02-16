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
import { HybridLabelsBrandingDataService } from '../HybridLabels/Services/HybridLabelsBrandingDataService';
import { BrandingDataService } from '../HybridLabels/Services/BrandingDataService';
export var DSVLoginComponent = (function (_super) {
    __extends(DSVLoginComponent, _super);
    function DSVLoginComponent(ss, hybridLabelsBrandingDataService) {
        _super.call(this, ss);
        this.ss = ss;
        this.hybridLabelsBrandingDataService = hybridLabelsBrandingDataService;
        this.MainColor = null;
        this.BackgroundImage = "";
        this.MainImage = "";
        this.show = true;
        this.privateUrl = "http://localhost:9996/";
        //this.GetHybridLabelsData(this.privateUrl);
        //console.log("const " + this.BackgroundImage);
    }
    DSVLoginComponent.prototype.ngOnInit = function () {
        console.log("ngOnInit");
        this.get_cookie_data();
        this.privateUrl = SessionInfo.GetLogitudeURL();
        this.GetHybridLabelsData(this.privateUrl);
        console.log("on init " + this.BackgroundImage);
    };
    DSVLoginComponent.prototype.GetHybridLabelsData = function (privateUrl) {
        var _this = this;
        this.hybridLabelsBrandingDataService.GetUserDashboardBrandingData(BrandingDataService.GetHybridLabelsDataRequest(privateUrl)).subscribe(function (response) {
            if (response.Result) {
                _this.Tenant = response.Result.Tenant;
                BrandingDataService.SetHybridLabelsDataRequest(response.Result, privateUrl);
                _this.MainColor = response.Result.MainColor != null ? BrandingDataService.ConvertHexaToRGBA(response.Result.MainColor) : null;
                _this.BackgroundImage = BrandingDataService.GetBackgroundImage();
                _this.MainImage = BrandingDataService.GetMainImage();
                console.log("Inside " + _this.BackgroundImage);
                //  this.BackgroundImage = `background: url(${this.BackgroundImage})`; 
                _this.MainImage = BrandingDataService.GetMainImage();
                console.log("Inside " + _this.MainImage);
            }
            _this.show = false;
            //else {
            //     this.GoToError401();
            // }
        });
    };
    //  private GoToError401() {
    //    this.router.navigate(['Error401']);
    // }
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
        { type: HybridLabelsBrandingDataService, },
    ];
    return DSVLoginComponent;
}(LoginComponent));
//# sourceMappingURL=DSVLoginComponent.js.map