var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
import { Component } from '@angular/core';
import { ResetPasswordComponent } from '../../Components/ResetPasswordComponent';
import { LoginService } from '../../LoginService';
import { SessionInfo } from '../../SessionInfo';
import { BrandingDataService } from '../Services/BrandingDataService';
import { HybridLabelsBrandingDataService } from '../Services/HybridLabelsBrandingDataService';
export var HybridResetPasswordComponent = (function (_super) {
    __extends(HybridResetPasswordComponent, _super);
    function HybridResetPasswordComponent(ss, hybridLabelsBrandingDataService) {
        _super.call(this, ss);
        this.ss = ss;
        this.hybridLabelsBrandingDataService = hybridLabelsBrandingDataService;
        this.MainColor = null;
        this.BackgroundImage = "";
        this.ForgetPasswordImage = "";
        this.Id = "";
        this.MainLogo = "";
        this.ContactUsEmail = "";
        this.show = true;
        //this.privateUrl = "http://localhost:9996/";
    }
    HybridResetPasswordComponent.prototype.ngOnInit = function () {
        console.log("ngOnInit");
        this.privateUrl = SessionInfo.GetLogitudeURL();
        console.log("on init " + this.privateUrl);
        this.GetHybridLabelsData(this.privateUrl);
    };
    HybridResetPasswordComponent.prototype.GetHybridLabelsData = function (privateUrl) {
        var _this = this;
        this.hybridLabelsBrandingDataService.GetUserDashboardBrandingData(BrandingDataService.GetHybridLabelsDataRequest(privateUrl)).subscribe(function (response) {
            if (response.Result) {
                BrandingDataService.SetHybridLabelsDataRequest(response.Result, privateUrl);
                _this.ContactUsEmail = response.Result.ContactUsEmail;
                _this.MainColor = response.Result.MainColor;
                _this.BackgroundImage = BrandingDataService.GetBackgroundImage();
                _this.ForgetPasswordImage = BrandingDataService.GetForgetPasswordImage();
                _this.Id = response.Result.Id;
                _this.MainLogo = response.Result.MainLogo;
                _this.MainLogo = BrandingDataService.GetMainLogo();
                console.log("BackgroundImage " + _this.BackgroundImage);
                console.log("ForgetPasswordImage " + _this.ForgetPasswordImage);
                console.log("MainLogo " + _this.MainLogo);
            }
        });
    };
    //  private GoToError401() {
    //    this.router.navigate(['Error401']);
    // }
    HybridResetPasswordComponent.decorators = [
        { type: Component, args: [{
                    selector: 'HybridResetPasswordComponent',
                    moduleId: './Login/Components/',
                    templateUrl: 'HybridResetPasswordComponent.html',
                    styleUrls: ['HybridResetPasswordComponent.css']
                },] },
    ];
    /** @nocollapse */
    HybridResetPasswordComponent.ctorParameters = [
        { type: LoginService, },
        { type: HybridLabelsBrandingDataService, },
    ];
    return HybridResetPasswordComponent;
}(ResetPasswordComponent));
//# sourceMappingURL=HybridResetPasswordComponent.js.map