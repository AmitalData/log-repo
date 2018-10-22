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
var PasswordChangeService_1 = require('../PasswordChangeService');
var Tools_1 = require('../Utilities/Tools');
var ChangePasswordComponent = (function () {
    function ChangePasswordComponent(_passwordChangeService, _loginService) {
        this._passwordChangeService = _passwordChangeService;
        this._loginService = _loginService;
        this.HasErrors = false;
        this.ErrorMessage = null;
        this.strongPassword = false;
        this.IsResetPasswordViaEmail = false;
        this.PdwcheckmsgKey = Tools_1.Tools.newGuid();
        this.SMKey = Tools_1.Tools.newGuid();
        this.Pdwcheckmsg0DivId = this.PdwcheckmsgKey + "0";
        this.Pdwcheckmsg1DivId = this.PdwcheckmsgKey + "1";
        this.Pdwcheckmsg2DivId = this.PdwcheckmsgKey + "2";
        this.Pdwcheckmsg3DivId = this.PdwcheckmsgKey + "3";
        this.Pdwcheckmsg4DivId = this.PdwcheckmsgKey + "4";
        this.idSM1HtmlId = this.SMKey + "1";
        this.idSM2HtmlId = this.SMKey + "2";
        this.idSM3HtmlId = this.SMKey + "3";
        this.idSM4HtmlId = this.SMKey + "4";
        this.email = window.sessionStorage.getItem("email");
        this.requestNumber = window.sessionStorage.getItem("requestNumber");
        this.ResetPWD = window.sessionStorage.getItem("ResetPWD");
        if (this.ResetPWD == "true") {
            this.IsResetPasswordViaEmail = true;
        }
        else {
            this.email = SessionInfo_1.SessionInfo.LoggedUserEmail;
        }
    }
    ChangePasswordComponent.prototype.ngOnInit = function () {
    };
    ChangePasswordComponent.prototype.SubmitBtnClicked = function () {
        var _this = this;
        if (!this.email) {
            this.HasErrors = true;
            this.ErrorMessage = "Your email is empty.";
            //this.ValidationErrorsList.push(TextCodeTranslator.Translate("User.M.PasswordsMaximumLlengthIs16Characters"));
            return;
        }
        this.ErrorMessage = this.PasswordValidation();
        if (this.ErrorMessage) {
            this.HasErrors = true;
            return;
        }
        if (!this.NewPassword || this.NewPassword.length < 8) {
            this.HasErrors = true;
            this.ErrorMessage = "Your password length must be more than 8 characters.";
            //this.ValidationErrorsList.push(TextCodeTranslator.Translate("User.M.PasswordsMinimumLengthIs8Characters"));
            return;
        }
        if (this.NewPassword.length > 16) {
            this.HasErrors = true;
            this.ErrorMessage = "Your password length must be less than 16 characters.";
            //this.ValidationErrorsList.push(TextCodeTranslator.Translate("User.M.PasswordsMaximumLlengthIs16Characters"));
            return;
        }
        if (this.NewPassword) {
            if (!this.IsResetPasswordViaEmail) {
                if (!this.CurrentPassword) {
                    this.HasErrors = true;
                    this.ErrorMessage = "Current Password can't be empty!";
                    return;
                }
            }
            if (this.NewPassword == this.RetypePassword) {
                if (!this.strongPassword) {
                    this.HasErrors = true;
                    this.ErrorMessage = "Your password is invalid.";
                }
                else {
                    if (this.CurrentPassword) {
                        var changePasswordParameter = new LoginService_1.ChangePasswordParameter();
                        changePasswordParameter.CurrentPassword = this.CurrentPassword;
                        changePasswordParameter.Email = this.email;
                        this._loginService.CheckUserPassword(changePasswordParameter).subscribe(function (res) {
                            if (!res) {
                                _this.ErrorMessage = "The current password is wrong!";
                                _this.HasErrors = true;
                            }
                            else {
                                if (_this.CurrentPassword == _this.NewPassword) {
                                    _this.ErrorMessage = "New password can't be the same as the current password";
                                    _this.HasErrors = true;
                                }
                                else
                                    _this.ChangePassword();
                            }
                        });
                    }
                    else
                        this.ChangePassword();
                }
            }
            else {
                this.HasErrors = true;
                this.ErrorMessage = "Passwords are not mached!";
            }
        }
        else {
            this.HasErrors = true;
            this.ErrorMessage = "Password Cant Be Empty!";
        }
    };
    ChangePasswordComponent.prototype.PasswordValidation = function () {
        var _this = this;
        var messageError = "";
        var email = this.email;
        //Password Contains User Email
        if (email) {
            var ContainsEmail = false;
            if (this.NewPassword.toLowerCase().indexOf(email.toLowerCase()) > -1) {
                ContainsEmail = true;
            }
            else {
                var emalData = [];
                var userEmail = email.split('@');
                emalData.push(userEmail[0]);
                emalData.push(userEmail[1].split('.')[0]);
                emalData.push(userEmail[1].split('.')[1]);
                if (emalData) {
                    emalData.forEach(function (item) {
                        if (item) {
                            if (_this.NewPassword.toLowerCase().indexOf(item.toLowerCase()) > -1) {
                                ContainsEmail = true;
                            }
                        }
                    });
                }
            }
            if (ContainsEmail) {
                messageError = "Password mustn't contain user email!";
                return messageError;
            }
        }
        // contain series(5 letters / numbers)
        if (this.IsPasswordContainsSeries(this.NewPassword)) {
            messageError = "Password can't contain series (5 letters/numbers)";
            return messageError;
        }
        return "";
    };
    ChangePasswordComponent.prototype.IsPasswordContainsSeries = function (password) {
        var result = false;
        if (password)
            password = password.toUpperCase();
        var passwordNumnberList = [];
        for (var i = 0; i < password.length; i++) {
            var char = password.charAt(i);
            var x = 0;
            if ('0123456789'.indexOf(char) !== -1) {
                x = Number(char);
            }
            else {
                x = char.charCodeAt(0);
            }
            passwordNumnberList.push(x);
        }
        var seriesNumnberCount = 0;
        var seriesNumnberList = [];
        passwordNumnberList.forEach(function (item) {
            var IsNotSeriesNumnber = false;
            if (item <= 9 || ((item >= 65 && item <= 90))) {
                if (seriesNumnberList.length == 0) {
                    seriesNumnberList.push(item);
                }
                else {
                    if (seriesNumnberList[seriesNumnberList.length - 1] + 1 == item) {
                        seriesNumnberList.push(item);
                        seriesNumnberCount += 1;
                    }
                    else {
                        IsNotSeriesNumnber = true;
                    }
                }
            }
            else
                IsNotSeriesNumnber = true;
            if (seriesNumnberCount == 4) {
                result = true;
                return;
            }
            if (IsNotSeriesNumnber) {
                seriesNumnberCount = 0;
                seriesNumnberList = [];
            }
        });
        return result;
    };
    ChangePasswordComponent.prototype.ChangePassword = function () {
        var _this = this;
        var params = {
            RequestNumber: this.IsResetPasswordViaEmail ? this.requestNumber : null,
            NewPassword: this.NewPassword,
            OldPassword: this.CurrentPassword ? this.CurrentPassword : null,
            IsResetRequest: this.IsResetPasswordViaEmail ? true : false,
        };
        this._loginService.PostChangePassword(this.email, params).subscribe(function (res) {
            if (res) {
                document.location.href = SessionInfo_1.SessionInfo.GetLogitudeURL() + "Login.aspx";
            }
            else {
                _this.ErrorMessage = "Changing password failed!";
                _this.HasErrors = true;
            }
        });
    };
    ChangePasswordComponent.prototype.onPasswordChanged = function () {
        var passtringstring = passtring();
        if (passtringstring) {
            var cursorPosition = this.NewPassword.length;
            PassWordValueTriming(passtringstring);
        }
    };
    ChangePasswordComponent.prototype.EvalPwdStrength = function (sP, pdwcheckmsgKey, sMKey) {
        this.strongPassword = false;
        if (ClientSideBestPassword(sP, gSimilarityMap, gDictionary)) {
            DispPwdStrength(4, 'pwdCheckCase4', pdwcheckmsgKey, sMKey);
            this.strongPassword = true;
        }
        else if (ClientSideStrongPassword(sP, gSimilarityMap, gDictionary)) {
            DispPwdStrength(3, 'pwdCheckCase3', pdwcheckmsgKey, sMKey);
            this.strongPassword = true;
        }
        else if (ClientSideMediumPassword(sP, gSimilarityMap, gDictionary)) {
            DispPwdStrength(2, 'pwdCheckCase2', pdwcheckmsgKey, sMKey);
        }
        else if (ClientSideWeakPassword(sP, gSimilarityMap, gDictionary)) {
            DispPwdStrength(1, 'pwdCheckCase1', pdwcheckmsgKey, sMKey);
        }
        else {
            DispPwdStrength(0, 'pwdCheckCase0', pdwcheckmsgKey, sMKey);
        }
    };
    ChangePasswordComponent.prototype.capLock = function (e) {
        var kc = e.keyCode ? e.keyCode : e.which;
        if (kc == 20) {
        }
        else {
            var sk = e.shiftKey ? e.shiftKey : ((kc == 16) ? true : false);
            if (((kc >= 65 && kc <= 90) && !sk) || ((kc >= 97 && kc <= 122) && sk)) {
            }
            else {
            }
        }
    };
    ChangePasswordComponent = __decorate([
        core_1.Component({
            selector: 'ChangePasswordComponent',
            moduleId: './Login/Components/',
            templateUrl: 'ChangePasswordComponent.html',
            styleUrls: ['ChangePasswordComponent.css']
        }), 
        __metadata('design:paramtypes', [PasswordChangeService_1.PasswordChangeService, LoginService_1.LoginService])
    ], ChangePasswordComponent);
    return ChangePasswordComponent;
}());
exports.ChangePasswordComponent = ChangePasswordComponent;
//# sourceMappingURL=ChangePasswordComponent.js.map