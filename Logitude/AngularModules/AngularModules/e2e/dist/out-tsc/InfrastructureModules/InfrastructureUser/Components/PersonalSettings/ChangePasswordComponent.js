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
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var Tools_1 = require("../../../../Infrastructure/Tools");
var PasswordChangeService_1 = require("../../../../Common/Services/Others/PasswordChangeService");
var SessionInfo_1 = require("../../../../Infrastructure/Utilities/SessionInfo");
var ChangePasswordParameter_1 = require("../../../../Infrastructure/DataContracts/ChangePasswordParameter");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var ChangePasswordComponent = /** @class */ (function () {
    function ChangePasswordComponent(_passwordChangeService) {
        this._passwordChangeService = _passwordChangeService;
        this.strongPassword = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.PasswordLenghtImg = "./_Resources/Images/Icons/ChangePassword/verified.png";
        this.PasswordContainsCharactersImg = "./_Resources/Images/Icons/ChangePassword/verified.png";
        this.PasswordContainsNumberImg = "./_Resources/Images/Icons/ChangePassword/verified.png";
    }
    ChangePasswordComponent.prototype.ngOnInit = function () {
        this.NewPassword = "";
        this.RetypePassword = "";
    };
    ChangePasswordComponent.prototype.SetDataContext = function (dataContext) {
    };
    ChangePasswordComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    ChangePasswordComponent.prototype.Validation = function () {
        this.ValidationErrorsList = [];
        if (this.NewPassword == this.RetypePassword) {
            if (!this.NewPassword) {
                this.ValidationErrorsList.push("Confirm your password");
                return;
            }
            this.PasswordValidation();
            if (this.ValidationErrorsList.length > 0) {
                return;
            }
            if (!this.IsContainsLowerUpperCase(this.NewPassword)) {
                this.ValidationErrorsList.push("Your password must include an uppercase and lowercase letter.");
                return;
            }
            if (!this.IsContainsNumber(this.NewPassword)) {
                this.ValidationErrorsList.push("Your password must include a number.");
                return;
            }
            if (this.NewPassword.length < 8) {
                this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("User.M.PasswordsMinimumLengthIs8Characters"));
                return;
            }
        }
        else {
            this.ValidationErrorsList.push("The passwords you entered do not match.");
            return;
        }
        if (this.ValidationErrorsList.length == 0) {
            if (!Tools_1.AppTool.IsNullOrEmpty(this.CurrentPassword)) {
                if (this.NewPassword == this.CurrentPassword) {
                    this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("User.M.NewPasswordCantBeSameAsCurrentOne"));
                    return;
                }
            }
            else {
                this.ValidationErrorsList.push("Current Password can't be empty!");
                return;
            }
        }
    };
    ChangePasswordComponent.prototype.SaveButtonClicked = function () {
        var _this = this;
        this.Validation();
        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.CurrentWindow.StartBusyIndicator("Saving...");
            var changePasswordParameter = new ChangePasswordParameter_1.ChangePasswordParameter();
            changePasswordParameter.ContactId = SessionInfo_1.SessionInfo.LoggedUserPM.Id;
            changePasswordParameter.CurrentPassword = this.CurrentPassword;
            changePasswordParameter.Email = SessionInfo_1.SessionInfo.LoggedUserPM.Email;
            this._passwordChangeService.CheckUserPassword(changePasswordParameter).subscribe(function (res) {
                var pmResponse = res;
                if (!pmResponse.HasError) {
                    var myResult = pmResponse.Result;
                    if (myResult) {
                        if (_this.CurrentPassword == _this.NewPassword) {
                            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                            _this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("User.M.NewPasswordCantBeSameAsCurrentOne"));
                        }
                        else
                            _this.ChangePassword();
                    }
                    else {
                        _this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("User.M.CurrentPasswordDoesntMatchYourInput"));
                        _this.CurrentSession.CurrentWindow.StopBusyIndicator();
                    }
                }
            });
        }
    };
    ChangePasswordComponent.prototype.PasswordValidation = function () {
        var _this = this;
        var isvalidPass = true;
        var userName = SessionInfo_1.SessionInfo.LoggedUserPM.EnglishName.split(' ');
        var email = SessionInfo_1.SessionInfo.LoggedUserPM.Email;
        //Password Contains User Name
        var ContainsUserName = false;
        if (userName) {
            userName.forEach(function (item) {
                if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
                    if (_this.NewPassword.toLowerCase().indexOf(item.toLowerCase()) > -1) {
                        ContainsUserName = true;
                    }
                }
            });
        }
        if (ContainsUserName) {
            this.ValidationErrorsList.push(TextCodeTranslator_1.TextCodeTranslator.Translate("User.M.PasswordMustntContainUserName"));
        }
        var ContainsEmail = false;
        if (this.NewPassword.toLowerCase().indexOf(email.toLowerCase()) > -1) {
            ContainsEmail = true;
        }
        else {
            var emalData = [];
            var userEmail = SessionInfo_1.SessionInfo.LoggedUserPM.Email.split('@');
            emalData.push(userEmail[0]);
            emalData.push(userEmail[1].split('.')[0]);
            emalData.push(userEmail[1].split('.')[1]);
            if (emalData) {
                emalData.forEach(function (item) {
                    if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
                        if (_this.NewPassword.toLowerCase().indexOf(item.toLowerCase()) > -1) {
                            ContainsEmail = true;
                        }
                    }
                });
            }
        }
        if (ContainsEmail) {
            this.ValidationErrorsList.push("Password mustn't contain user email!");
        }
        // contain series(5 letters / numbers)
        var errorList = this.IsPasswordContainsSeries(this.NewPassword);
        if (errorList.length > 0) {
            errorList.forEach(function (item) {
                _this.ValidationErrorsList.push(item);
            });
        }
    };
    ChangePasswordComponent.prototype.ChangePassword = function () {
        var _this = this;
        var changePasswordParameter = new ChangePasswordParameter_1.ChangePasswordParameter();
        changePasswordParameter.Email = SessionInfo_1.SessionInfo.LoggedUserPM.Email;
        changePasswordParameter.NewPassword = this.NewPassword;
        changePasswordParameter.CurrentPassword = this.CurrentPassword;
        this._passwordChangeService.ChangeUserPassword(changePasswordParameter).subscribe(function (res) {
            _this.CurrentSession.CurrentWindow.StopBusyIndicator();
            var pmResponse = res;
            if (!pmResponse.HasError) {
                var myResult = pmResponse.Result;
                if (myResult) {
                    _this.CancelButtonClicked();
                }
                else {
                    _this.ValidationErrorsList.push("Changing password failed!");
                }
            }
            else {
                if (pmResponse.ErrorsArray && pmResponse.ErrorsArray.length > 0) {
                    var messageWindow = new MessageWindow_1.MessageWindow();
                    messageWindow.Title = "Logitude Message";
                    messageWindow.Show(pmResponse.ErrorsArray[0]);
                }
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
    ChangePasswordComponent.prototype.IsPasswordContainsSeries = function (password) {
        var errorList = [];
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
            errorList.push("Password should not contain more than 3 following characters");
        result = this.IsSeries(passwordNumnberList, "Same");
        if (result)
            errorList.push("Password should not contain more than 3 consecutive repeating characters");
        return errorList;
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
    ChangePasswordComponent.prototype.IsContainsLowerUpperCase = function (str) {
        return str.match(/[a-z]/) && str.match(/[A-Z]/);
    };
    ChangePasswordComponent.prototype.IsContainsNumber = function (str) {
        var regex = /\d/g;
        return regex.test(str);
    };
    ChangePasswordComponent.prototype.Passwordkeyup = function (passtring) {
        this.ValidationErrorsList = [];
        document.getElementById("PasswordLenghtDiv").style.color = "gray";
        document.getElementById("PasswordContainsCharactersDiv").style.color = "gray";
        document.getElementById("PasswordContainsNumberDiv").style.color = "gray";
        this.PasswordLenghtImg = "./_Resources/Images/Icons/ChangePassword/verified.png";
        this.PasswordContainsCharactersImg = "./_Resources/Images/Icons/ChangePassword/verified.png";
        this.PasswordContainsNumberImg = "./_Resources/Images/Icons/ChangePassword/verified.png";
        if (passtring) {
            if (passtring.length >= 8) {
                document.getElementById("PasswordLenghtDiv").style.color = "green";
                this.PasswordLenghtImg = "./_Resources/Images/Icons/ChangePassword/verifiedGreen.png";
            }
            if (this.IsContainsLowerUpperCase(passtring)) {
                document.getElementById("PasswordContainsCharactersDiv").style.color = "green";
                this.PasswordContainsCharactersImg = "./_Resources/Images/Icons/ChangePassword/verifiedGreen.png";
            }
            if (this.IsContainsNumber(passtring)) {
                document.getElementById("PasswordContainsNumberDiv").style.color = "green";
                this.PasswordContainsNumberImg = "./_Resources/Images/Icons/ChangePassword/verifiedGreen.png";
            }
            this.PasswordValidation();
        }
    };
    ChangePasswordComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'ChangePassword',
            templateUrl: './ChangePasswordComponent.html',
            providers: [PasswordChangeService_1.PasswordChangeService]
        }),
        __metadata("design:paramtypes", [PasswordChangeService_1.PasswordChangeService])
    ], ChangePasswordComponent);
    return ChangePasswordComponent;
}());
exports.ChangePasswordComponent = ChangePasswordComponent;
//# sourceMappingURL=ChangePasswordComponent.js.map