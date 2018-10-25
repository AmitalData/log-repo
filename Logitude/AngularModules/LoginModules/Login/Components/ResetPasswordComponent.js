"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
var core_1 = require('@angular/core');
var LoginService_1 = require('../LoginService');
var SessionInfo_1 = require('../SessionInfo');
var ResetPasswordComponent = (function () {
    function ResetPasswordComponent(_LoginService) {
        this._LoginService = _LoginService;
        this.HasErrors = false;
        this.ErrorMessage = null;
        this.Succeeded = false;
        this.ShowbusyIndicator = false;
    }
    ResetPasswordComponent.prototype.SubmitBtnClicked = function () {
        var _this = this;
        this.ShowbusyIndicator = true;
        if (!this.Email) {
            this.ShowbusyIndicator = false;
            this.HasErrors = true;
            this.ErrorMessage = "Email can't be empty !";
        }
        else if (!this.ValidateEmail(this.Email)) {
            this.ShowbusyIndicator = false;
            this.HasErrors = true;
            this.ErrorMessage = "Your email address is invalid !";
        }
        else {
            this.HasErrors = false;
            this._LoginService.GetRequestResetUserPassword(this.Email, false).subscribe(function (userdata) {
                _this.ShowbusyIndicator = false;
                if (!userdata.HasError) {
                    _this.Succeeded = true;
                    _this.HasErrors = false;
                }
                else {
                    _this.CaptchaKey = userdata ? userdata.CaptchaKey : "";
                    //disableForm(false);
                    var errorMessage = "Submit failed! invalid email." + "<br/>";
                    if (userdata.InValidCaptcha) {
                        if (_this.IsShowAreaCaptcha) {
                            _this.CaptchaTextValue = "";
                            errorMessage = "Please re-enter the characters you see in the image above";
                        }
                        _this.IsShowAreaCaptcha = true;
                        _this.CaptchaImageUrl = userdata.CaptchaImage;
                    }
                    else if (userdata.IpRestricted) {
                        errorMessage = "Trying to submit in from unauthorised station!" + "<br/>" + "(The IP address you are trying to " + "<br/>" + "submit from is restricted for this user)"; //
                    }
                    if (userdata.IsLocked) {
                        errorMessage = "Your account has been locked out!" + "<br/>" + "please contact your administrator.";
                    }
                    if (userdata.InActive) {
                        errorMessage = "Your account has been deactivated!" + "<br/>" + "please contact your administrator.";
                    }
                    if (errorMessage == "Submit failed! invalid email." + "<br/>") {
                        if (_this.IsShowAreaCaptcha) {
                            _this.CaptchaImageUrl = userdata.CaptchaImage;
                            _this.CaptchaTextValue = "";
                        }
                    }
                    //document.getElementById("errorsList").innerHTML = errorMessage;
                    // $("#errorsList").text(errorMessage);
                    //$("#errorsList").show();
                    _this.HasErrors = true;
                    _this.ErrorMessage = errorMessage;
                }
            });
        }
    };
    ResetPasswordComponent.prototype.ValidateEmail = function (email) {
        var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(email);
    };
    ResetPasswordComponent.prototype.BackToLoginClicked = function () {
        document.location.href = SessionInfo_1.SessionInfo.GetLogitudeURL() + "Login.aspx";
    };
    ResetPasswordComponent = __decorate([
        core_1.Component({
            selector: 'ResetPasswordComponent',
            moduleId: './Login/Components/',
            templateUrl: 'ResetPasswordComponent.html',
            styleUrls: ['ChangePasswordComponent.css']
        }), 
        __metadata('design:paramtypes', [LoginService_1.LoginService])
    ], ResetPasswordComponent);
    return ResetPasswordComponent;
}());
exports.ResetPasswordComponent = ResetPasswordComponent;
//# sourceMappingURL=ResetPasswordComponent.js.map