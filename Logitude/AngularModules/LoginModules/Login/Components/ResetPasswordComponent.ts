import { Component } from '@angular/core';
import { LoginService} from '../LoginService';
import {Headers} from '@angular/http';
import {SessionInfo} from '../SessionInfo';
import {PasswordChangeService} from '../PasswordChangeService';
import {Tools} from '../Utilities/Tools';
declare var passtring, PassWordValueTriming, isctype, ClientSideBestPassword, gSimilarityMap, gDictionary, DispPwdStrength, ClientSideStrongPassword, DispPwdStrength, ClientSideMediumPassword, DispPwdStrength, ClientSideWeakPassword
    : any;

@Component({
    selector: 'ResetPasswordComponent',
    moduleId: './Login/Components/',
    templateUrl: 'ResetPasswordComponent.html',
    styleUrls: ['ChangePasswordComponent.css']
})
export class ResetPasswordComponent {

    HasErrors: boolean = false;
    ErrorMessage: string = null;
    Succeeded: boolean = false;
    Email: string;
    ShowbusyIndicator: boolean = false;

    IsShowAreaCaptcha: boolean;
    CaptchaImageUrl: string;
    CaptchaCode: string;
    CaptchaKey: string;


    constructor(public _LoginService: LoginService) {

    }

    HideAreaCaptcha() {
        this.IsShowAreaCaptcha = false;
        this.CaptchaCode = null;
        this.CaptchaKey = null;
    }


    HasCaptchaErrors: boolean = false;
    SubmitBtnClicked() {
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
            this.HasErrors = false;


            var params = {
                Email: this.Email,
                IsChampLogin: false,
                CaptchaCode: this.CaptchaCode,
                CaptchaKey: this.CaptchaKey,
            }

            this._LoginService.PostRequestResetUserPassword(params).subscribe(userdata => {
                this.ShowbusyIndicator = false;

                if (!userdata.HasError) {

                    this.Succeeded = true;
                    this.HasErrors = false;
                    this.HideAreaCaptcha();
                }
                else {

                    this.CaptchaKey = userdata ? userdata.CaptchaKey : "";
                    //disableForm(false);
                    var errorMessage = null;

                    if (userdata.InValidCaptcha) {
                        this.CaptchaCode = "";
                        this.IsShowAreaCaptcha = true;
                        this.CaptchaImageUrl = userdata.CaptchaImage;
                        this.HasCaptchaErrors = true;

                    }


                    if (userdata.IpRestricted) errorMessage = "Trying to log in from unauthorised station!" + " (The IP address you are trying to " + " log in from is restricted for this user)";//
                    if (userdata.InActive) errorMessage = "Your account has been deactivated!" + "<br/>" + "please contact your administrator.";
                    if (userdata.Unlicensed) errorMessage = "Your account is unlicensed!" + " please contact your administrator.";
                    if (userdata.InValidCaptcha) errorMessage = "Please re-enter the characters you see in the image above";
                    if (userdata.InValidMailOrPassword) {

                        errorMessage = "Login failed! invalid user name or password.";
                        this.HasCaptchaErrors = false;
                    }

                    this.HasErrors = true;
                    this.ErrorMessage = errorMessage;

                }
            });
        }
        
    }

    ValidateEmail(email) {
        var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(email);
    }

    BackToLoginClicked() {
        document.location.href = SessionInfo.GetLogitudeURL() + "Login.aspx";
    }
}
