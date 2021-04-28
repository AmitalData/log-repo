import { Component } from '@angular/core';
import { LoginService } from '../LoginService';
import { SessionInfo } from '../SessionInfo';
export var ResetPasswordComponent = (function () {
    function ResetPasswordComponent(_LoginService) {
        this._LoginService = _LoginService;
        this.HasErrors = false;
        this.ErrorMessage = null;
        this.Succeeded = false;
        this.ShowbusyIndicator = false;
        this.captchaCode = "";
        this.HasCaptchaErrors = false;
    }
    ResetPasswordComponent.prototype.HideAreaCaptcha = function () {
        this.IsShowAreaCaptcha = false;
        this.CaptchaCode = null;
        this.CaptchaKey = null;
    };
    Object.defineProperty(ResetPasswordComponent.prototype, "CaptchaCode", {
        get: function () { return this.captchaCode; },
        set: function (value) {
            if (this.captchaCode != value) {
                this.captchaCode = value;
            }
            if (this.captchaCode) {
                if (this.HasCaptchaErrors) {
                    this.HasCaptchaErrors = false;
                    this.HasErrors = false;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
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
        else if (this.IsShowAreaCaptcha && !this.CaptchaCode) {
            this.ShowbusyIndicator = false;
            this.HasErrors = true;
            this.ErrorMessage = "Please re-enter the characters you see in the image above";
        }
        else {
            this.Succeeded = false;
            this.HasErrors = false;
            this.ErrorMessage = "";
            this.HasCaptchaErrors = false;
            var params = {
                Email: this.Email,
                IsChampLogin: false,
                CaptchaCode: this.CaptchaCode,
                CaptchaKey: this.CaptchaKey,
            };
            this._LoginService.PostRequestResetUserPassword(params).subscribe(function (userdata) {
                _this.ShowbusyIndicator = false;
                if (!userdata.HasError) {
                    _this.Succeeded = true;
                    _this.HasErrors = false;
                    _this.HideAreaCaptcha();
                }
                else {
                    _this.CaptchaKey = userdata ? userdata.CaptchaKey : "";
                    if (userdata.InValidCaptcha) {
                        _this.CaptchaCode = "";
                        _this.IsShowAreaCaptcha = true;
                        _this.CaptchaImageUrl = userdata.CaptchaImage;
                        _this.HasCaptchaErrors = true;
                    }
                    var errorMessage = "";
                    if (userdata.ExceptionMessage)
                        alert(userdata.ExceptionMessage);
                    else {
                        if (userdata.InValidCaptcha)
                            errorMessage = "Please re-enter the characters you see in the image above";
                        if (userdata.IpRestricted)
                            errorMessage = "Unauthorized IP Address. Your IP is not authorized to access this account!";
                        if (userdata.InActive)
                            errorMessage = "Your account has been deactivated!" + "please contact your administrator.";
                        if (errorMessage) {
                            _this.HasErrors = true;
                            _this.ErrorMessage = errorMessage;
                        }
                        else {
                            _this.Succeeded = true;
                            _this.HasErrors = false;
                            _this.HideAreaCaptcha();
                        }
                    }
                }
            });
        }
    };
    ResetPasswordComponent.prototype.ValidateEmail = function (email) {
        var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(email);
    };
    ResetPasswordComponent.prototype.BackToLoginClicked = function () {
        document.location.href = SessionInfo.GetLogitudeURL() + "Login.aspx";
    };
    ResetPasswordComponent.decorators = [
        { type: Component, args: [{
                    selector: 'ResetPasswordComponent',
                    moduleId: './Login/Components/',
                    templateUrl: 'ResetPasswordComponent.html',
                    styleUrls: ['ChangePasswordComponent.css']
                },] },
    ];
    /** @nocollapse */
    ResetPasswordComponent.ctorParameters = [
        { type: LoginService, },
    ];
    return ResetPasswordComponent;
}());
//# sourceMappingURL=ResetPasswordComponent.js.map