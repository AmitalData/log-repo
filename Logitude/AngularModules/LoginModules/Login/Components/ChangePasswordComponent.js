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
var ChangePasswordComponent = (function () {
    function ChangePasswordComponent(_passwordChangeService, _loginService) {
        this._passwordChangeService = _passwordChangeService;
        this._loginService = _loginService;
        this.HasErrors = false;
        this.ErrorMessage = null;
        this.IsResetPasswordViaEmail = false;
        this.PasswordLenghtImg = "./Images/ChangePassword/verified.png";
        this.PasswordContainsCharactersImg = "./Images/ChangePassword/verified.png";
        this.PasswordContainsNumberImg = "./Images/ChangePassword/verified.png";
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
        this.email = "hamodi@fnarsoft.com";
        if (!this.email) {
            this.HasErrors = true;
            this.ErrorMessage = "Your email is empty.";
            return;
        }
        if (this.NewPassword == this.RetypePassword) {
            if (!this.NewPassword) {
                this.ErrorMessage = "Confirm your password";
                this.HasErrors = true;
                return;
            }
            this.ErrorMessage = this.PasswordValidation();
            if (this.ErrorMessage) {
                this.HasErrors = true;
                return;
            }
            if (!this.IsContainsLowerUpperCase(this.NewPassword)) {
                this.ErrorMessage = "Your password must include an uppercase and lowercase letter.";
                this.HasErrors = true;
                return;
            }
            if (!this.IsContainsNumber(this.NewPassword)) {
                this.ErrorMessage = "Your password must include a number.";
                this.HasErrors = true;
                return;
            }
            if (this.NewPassword.length < 8) {
                this.ErrorMessage = "Your password must be at least 8 characters.";
                this.HasErrors = true;
                return;
            }
        }
        else {
            this.ErrorMessage = "The passwords you entered do not match.";
            this.HasErrors = true;
            return;
        }
        if (!this.IsResetPasswordViaEmail) {
            if (!this.CurrentPassword) {
                this.HasErrors = true;
                this.ErrorMessage = "Current Password can't be empty!";
                return;
            }
            else if (this.CurrentPassword == this.NewPassword) {
                this.ErrorMessage = "New password can't be the same as the current password";
                this.HasErrors = true;
            }
        }
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
        var seriesMessage = this.IsPasswordContainsSeries(this.NewPassword);
        if (seriesMessage) {
            messageError = seriesMessage;
            return messageError;
        }
        return "";
    };
    ChangePasswordComponent.prototype.IsPasswordContainsSeries = function (password) {
        var messageError = "";
        var result = false;
        if (password)
            password = password.toUpperCase();
        var passwordNumnberList = [];
        for (var i = 0; i < password.length; i++) {
            var char = password.charAt(i);
            var x = 0;
            if ('0123456789'.indexOf(char) !== -1)
                x = Number(char);
            else
                x = char.charCodeAt(0);
            passwordNumnberList.push(x);
        }
        result = this.IsSeries(passwordNumnberList, "+");
        if (!result)
            result = this.IsSeries(passwordNumnberList, "-");
        if (result)
            messageError = "Password should not contain more than 3 following characters";
        if (!result) {
            result = this.IsSeries(passwordNumnberList, "Same");
            if (result)
                messageError = "Password should not contain more than 3 consecutive repeating characters";
        }
        return messageError;
        //return result;
    };
    ChangePasswordComponent.prototype.IsSeries = function (passwordNumnberList, operatorCode) {
        var result = false;
        var seriesNumnberCount = 0;
        var seriesNumnberList = [];
        passwordNumnberList.forEach(function (item) {
            var IsNotSeriesNumnber = false;
            if (item <= 9 || ((item >= 65 && item <= 90))) {
                if (seriesNumnberList.length == 0)
                    seriesNumnberList.push(item);
                else {
                    if (operatorCode == "+") {
                        if (seriesNumnberList[seriesNumnberList.length - 1] + 1 == item) {
                            seriesNumnberList.push(item);
                            seriesNumnberCount += 1;
                        }
                        else
                            IsNotSeriesNumnber = true;
                    }
                    else if (operatorCode == "-") {
                        if (seriesNumnberList[seriesNumnberList.length - 1] - 1 == item) {
                            seriesNumnberList.push(item);
                            seriesNumnberCount += 1;
                        }
                        else
                            IsNotSeriesNumnber = true;
                    }
                    else if (operatorCode == "Same") {
                        if (seriesNumnberList[seriesNumnberList.length - 1] == item) {
                            seriesNumnberList.push(item);
                            seriesNumnberCount += 1;
                        }
                        else
                            IsNotSeriesNumnber = true;
                    }
                }
            }
            else
                IsNotSeriesNumnber = true;
            if (seriesNumnberCount == 3) {
                result = true;
                return;
            }
            if (IsNotSeriesNumnber) {
                seriesNumnberCount = 0;
                seriesNumnberList = [];
                seriesNumnberList.push(item);
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
    ChangePasswordComponent.prototype.IsContainsLowerUpperCase = function (str) {
        return str.match(/[a-z]/) && str.match(/[A-Z]/);
    };
    ChangePasswordComponent.prototype.IsContainsNumber = function (str) {
        var regex = /\d/g;
        return regex.test(str);
    };
    ChangePasswordComponent.prototype.Passwordkeyup = function (passtring) {
        this.HasErrors = false;
        document.getElementById("PasswordLenghtDiv").style.color = "gray";
        document.getElementById("PasswordContainsCharactersDiv").style.color = "gray";
        document.getElementById("PasswordContainsNumberDiv").style.color = "gray";
        this.PasswordLenghtImg = "./Images/ChangePassword/verified.png";
        this.PasswordContainsCharactersImg = "./Images/ChangePassword/verified.png";
        this.PasswordContainsNumberImg = "./Images/ChangePassword/verified.png";
        if (passtring) {
            if (passtring.length >= 8) {
                document.getElementById("PasswordLenghtDiv").style.color = "green";
                this.PasswordLenghtImg = "./Images/ChangePassword/verifiedGreen.png";
            }
            if (this.IsContainsLowerUpperCase(passtring)) {
                document.getElementById("PasswordContainsCharactersDiv").style.color = "green";
                this.PasswordContainsCharactersImg = "./Images/ChangePassword/verifiedGreen.png";
            }
            if (this.IsContainsNumber(passtring)) {
                document.getElementById("PasswordContainsNumberDiv").style.color = "green";
                this.PasswordContainsNumberImg = "./Images/ChangePassword/verifiedGreen.png";
            }
            var errorMessage = this.PasswordValidation();
            this.ErrorMessage = errorMessage;
            if (errorMessage) {
                this.HasErrors = true;
            }
            else
                this.HasErrors = false;
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
        __metadata("design:paramtypes", [PasswordChangeService_1.PasswordChangeService, LoginService_1.LoginService])
    ], ChangePasswordComponent);
    return ChangePasswordComponent;
}());
exports.ChangePasswordComponent = ChangePasswordComponent;
//# sourceMappingURL=ChangePasswordComponent.js.map