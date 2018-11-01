import { Component } from '@angular/core';
import {LoginService, LoginParameters, ChangePasswordParameter} from '../LoginService';
import {Headers} from '@angular/http';
import {SessionInfo} from '../SessionInfo';
import {PasswordChangeService} from '../PasswordChangeService';
import {Tools} from '../Utilities/Tools';





declare var passtring, PassWordValueTriming, isctype, ClientSideBestPassword, gSimilarityMap, gDictionary, DispPwdStrength, ClientSideStrongPassword, DispPwdStrength, ClientSideMediumPassword, DispPwdStrength, ClientSideWeakPassword
    : any;

@Component({
    selector: 'ChangePasswordComponent',
    moduleId: './Login/Components/',
    templateUrl: 'ChangePasswordComponent.html',
    styleUrls: ['ChangePasswordComponent.css']
})
export class ChangePasswordComponent {

    HasErrors: boolean = false;
    ErrorMessage: string = null;

    CurrentPassword: string;
    NewPassword: string;
    RetypePassword: string;
    strongPassword: boolean = false;
    Pdwcheckmsg0DivId: string;
    Pdwcheckmsg1DivId: string;
    Pdwcheckmsg2DivId: string;
    Pdwcheckmsg3DivId: string;
    Pdwcheckmsg4DivId: string;

    idSM1HtmlId: string;
    idSM2HtmlId: string;
    idSM3HtmlId: string;
    idSM4HtmlId: string;

    PdwcheckmsgKey: string;
    SMKey: string;
    ResetPWD: string;
    email: string;
    requestNumber: string;
    IsResetPasswordViaEmail: boolean = false;

    constructor(public _passwordChangeService: PasswordChangeService, public _loginService: LoginService) {
        this.PdwcheckmsgKey = Tools.newGuid();
        this.SMKey = Tools.newGuid();
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
        } else {
            this.email = SessionInfo.LoggedUserEmail;

        }

       
    }

    
    ngOnInit() {

    } 

    SubmitBtnClicked() {

        if (!this.email) {
            this.HasErrors = true;
            this.ErrorMessage = "Your email is empty.";
            //this.ValidationErrorsList.push(TextCodeTranslator.Translate("User.M.PasswordsMaximumLlengthIs16Characters"));
            return;
        }


        this.ErrorMessage =  this.PasswordValidation();

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
                        var changePasswordParameter: ChangePasswordParameter = new ChangePasswordParameter();
                        changePasswordParameter.CurrentPassword = this.CurrentPassword;
                        changePasswordParameter.Email = this.email;
                        this._loginService.CheckUserPassword(changePasswordParameter).subscribe(res => {

                            if (!res) {
                                this.ErrorMessage = "The current password is wrong!";
                                this.HasErrors = true;
                            } else {
                                if (this.CurrentPassword == this.NewPassword) {
                                    this.ErrorMessage = "New password can't be the same as the current password";
                                    this.HasErrors = true;
                                } else this.ChangePassword();
                               
                            }
                        });
                    }

                    else this.ChangePassword();

                }

            }

            else {
                this.HasErrors = true;
                this.ErrorMessage = "Passwords are not mached!";
                //this.IsShowVerifypassError = true;
                //SessionLocator.CurrentSession.CurrentWindow.StopBusyIndicator();
            }

        }
        else {
            this.HasErrors = true;
            this.ErrorMessage = "Password Cant Be Empty!";
            //this.ValidationErrorsList.push(TextCodeTranslator.Translate("User.M.PasswordCantBeEmpty")); 
            //SessionLocator.CurrentSession.CurrentWindow.StopBusyIndicator(); 
        }
    }


    PasswordValidation() {

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
                    emalData.forEach((item) => {
                        if (item) {
                            if (this.NewPassword.toLowerCase().indexOf(item.toLowerCase()) > -1) {
                                ContainsEmail = true;
                            }
                        }

                    });
                }

            }
            if (ContainsEmail) {
                messageError = "Password mustn't contain user email!";
                return messageError

            }
        }


        // contain series(5 letters / numbers)
        if (this.IsPasswordContainsSeries(this.NewPassword)) {
            messageError = "Password can't contain series (5 letters/numbers)";
            return messageError;
        }

        return "";

    }

    IsPasswordContainsSeries(password: string) {

        var result = false;
        if (password) password = password.toUpperCase();

        var passwordNumnberList: any = [];
        for (var i = 0; i < password.length; i++) {
            var char = password.charAt(i);
            var x = 0;
            if ('0123456789'.indexOf(char) !== -1) {
                x = Number(char);

            } else {
                x = char.charCodeAt(0);
            }

            passwordNumnberList.push(x);
        }

        //Series
        var seriesNumnberCount: number = 0;
        var seriesNumnberList: any = [];
        passwordNumnberList.forEach((item) => {
            var IsNotSeriesNumnber = false;
            if (item <= 9 || ((item >= 65 && item <= 90))) {
                if (seriesNumnberList.length == 0) {
                    seriesNumnberList.push(item);
                }
                else {
                    if (seriesNumnberList[seriesNumnberList.length - 1] + 1 == item) {
                        seriesNumnberList.push(item);
                        seriesNumnberCount += 1;
                    } else {
                        IsNotSeriesNumnber = true;
                    }
                }

            } else IsNotSeriesNumnber = true;


            if (seriesNumnberCount == 2) {
                result = true;
                return;
            }

            if (IsNotSeriesNumnber) {
                seriesNumnberCount = 0;
                seriesNumnberList = [];
            }

        });



        //Same
        if (!result) {
            seriesNumnberCount = 0;
            seriesNumnberList = [];
            passwordNumnberList.forEach((item) => {
                var IsNotSeriesNumnber = false;
                if (item <= 9 || ((item >= 65 && item <= 90))) {
                    if (seriesNumnberList.length == 0) {
                        seriesNumnberList.push(item);
                    }
                    else {
                        if (seriesNumnberList[seriesNumnberList.length - 1] == item) {
                            seriesNumnberList.push(item);
                            seriesNumnberCount += 1;
                        } else {
                            IsNotSeriesNumnber = true;
                        }
                    }

                } else IsNotSeriesNumnber = true;


                if (seriesNumnberCount == 2) {
                    result = true;
                    return;
                }

                if (IsNotSeriesNumnber) {
                    seriesNumnberCount = 0;
                    seriesNumnberList = [];
                }

            });
        }


        //reverse
        if (!result) {
            seriesNumnberCount = 0;
            seriesNumnberList = [];

            passwordNumnberList.forEach((item) => {
                var IsNotSeriesNumnber = false;
                if (item <= 9 || ((item >= 65 && item <= 90))) {
                    if (seriesNumnberList.length == 0) {
                        seriesNumnberList.push(item);
                    }
                    else {
                        if (seriesNumnberList[seriesNumnberList.length - 1] - 1 == item) {
                            seriesNumnberList.push(item);
                            seriesNumnberCount += 1;
                        } else {
                            IsNotSeriesNumnber = true;
                        }
                    }

                } else IsNotSeriesNumnber = true;


                if (seriesNumnberCount == 2) {
                    result = true;
                    return;
                }

                if (IsNotSeriesNumnber) {
                    seriesNumnberCount = 0;
                    seriesNumnberList = [];
                }

            });



        }



        return result;
    }

    

    ChangePassword() {
        var params = {
            RequestNumber: this.IsResetPasswordViaEmail ? this.requestNumber : null,
            NewPassword: this.NewPassword,
            OldPassword: this.CurrentPassword ? this.CurrentPassword : null,
            IsResetRequest: this.IsResetPasswordViaEmail ? true : false,
        }

        this._loginService.PostChangePassword(this.email, params).subscribe(res => {
            if (res) {
                document.location.href = SessionInfo.GetLogitudeURL() + "Login.aspx";
            }
            else {
                this.ErrorMessage = "Changing password failed!"
                this.HasErrors = true;
            }

        });

    }

    onPasswordChanged() {

        var passtringstring = passtring();
        if (passtringstring) {

            var cursorPosition = this.NewPassword.length;
            PassWordValueTriming(passtringstring);

        }

    }








    EvalPwdStrength(sP: string, pdwcheckmsgKey: string, sMKey: string) {

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
    }

    capLock(e: any) {

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

    }
   

}
