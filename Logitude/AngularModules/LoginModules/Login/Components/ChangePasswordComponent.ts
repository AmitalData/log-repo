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

    ResetPWD: string;
    email: string;
    requestNumber: string;
    IsResetPasswordViaEmail: boolean = false;

    PasswordLenghtImg: string;
    PasswordContainsCharactersImg: string;
    PasswordContainsNumberImg: string;
    constructor(public _passwordChangeService: PasswordChangeService, public _loginService: LoginService) {




        this.PasswordLenghtImg = "./Images/ChangePassword/verified.png"
        this.PasswordContainsCharactersImg = "./Images/ChangePassword/verified.png"
        this.PasswordContainsNumberImg = "./Images/ChangePassword/verified.png"

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


        var seriesMessage: string = this.IsPasswordContainsSeries(this.NewPassword);
        if (seriesMessage) {
            messageError = seriesMessage;
            return messageError;
        }

        return "";

    }

    IsPasswordContainsSeries(password: string) {
        var messageError: string = "";
        var result = false;
        if (password) password = password.toUpperCase();

        var passwordNumnberList: any = [];
        for (var i = 0; i < password.length; i++) {
            var char = password.charAt(i);
            var x = 0;
            if ('0123456789'.indexOf(char) !== -1) x = Number(char);
            else x = char.charCodeAt(0);

            passwordNumnberList.push(x);
        }

        result = this.IsSeries(passwordNumnberList, "+");
        if (!result) result = this.IsSeries(passwordNumnberList, "-");
        if (result) messageError = "Password should not contain more than 3 following characters";
        if (!result) {
            result = this.IsSeries(passwordNumnberList, "Same");
            if (result) messageError = "Password should not contain more than 3 consecutive repeating characters";
        }

        return messageError;



        //return result;
    }
    IsSeries(passwordNumnberList: any, operatorCode: string) {

        var result = false;
        var seriesNumnberCount: number = 0;
        var seriesNumnberList: any = [];
        passwordNumnberList.forEach((item) => {
            var IsNotSeriesNumnber = false;
            if (item <= 9 || ((item >= 65 && item <= 90))) {
                if (seriesNumnberList.length == 0) seriesNumnberList.push(item);

                else {

                    if (operatorCode == "+") {
                        if (seriesNumnberList[seriesNumnberList.length - 1] + 1 == item) {
                            seriesNumnberList.push(item);
                            seriesNumnberCount += 1;
                        } else IsNotSeriesNumnber = true;
                    }

                    else if (operatorCode == "-") {
                        if (seriesNumnberList[seriesNumnberList.length - 1] - 1 == item) {
                            seriesNumnberList.push(item);
                            seriesNumnberCount += 1;
                        } else IsNotSeriesNumnber = true;
                    }

                    else if (operatorCode == "Same") {
                        if (seriesNumnberList[seriesNumnberList.length - 1] == item) {
                            seriesNumnberList.push(item);
                            seriesNumnberCount += 1;
                        } else IsNotSeriesNumnber = true;
                    }
                }

            } else IsNotSeriesNumnber = true;


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


    IsContainsLowerUpperCase(str) {
        return str.match(/[a-z]/) && str.match(/[A-Z]/);
    }
    
    IsContainsNumber(str) {
        var regex = /\d/g;
        return regex.test(str);
    }

      

    Passwordkeyup(passtring) {
    
        this.HasErrors = false;

        document.getElementById("PasswordLenghtDiv").style.color = "gray";
        document.getElementById("PasswordContainsCharactersDiv").style.color = "gray";
        document.getElementById("PasswordContainsNumberDiv").style.color = "gray";

        this.PasswordLenghtImg = "./Images/ChangePassword/verified.png"
        this.PasswordContainsCharactersImg = "./Images/ChangePassword/verified.png"
        this.PasswordContainsNumberImg = "./Images/ChangePassword/verified.png"

        if (passtring) {

            if (passtring.length >= 8) {
                document.getElementById("PasswordLenghtDiv").style.color = "green";
                this.PasswordLenghtImg = "./Images/ChangePassword/verifiedGreen.png"
       
            }

            if (this.IsContainsLowerUpperCase(passtring)) {
                document.getElementById("PasswordContainsCharactersDiv").style.color = "green";
                this.PasswordContainsCharactersImg = "./Images/ChangePassword/verifiedGreen.png"
            }

            if (this.IsContainsNumber(passtring)) {
                document.getElementById("PasswordContainsNumberDiv").style.color = "green";
                this.PasswordContainsNumberImg = "./Images/ChangePassword/verifiedGreen.png"
            }


            var errorMessage = this.PasswordValidation();

            this.ErrorMessage = errorMessage;
            if (errorMessage) {
                this.HasErrors = true;
       
            }
            else this.HasErrors = false;


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
