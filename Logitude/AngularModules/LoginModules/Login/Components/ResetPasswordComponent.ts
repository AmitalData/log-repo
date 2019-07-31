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

    CaptchaKey: string;


    constructor(public _LoginService: LoginService) {

    }

    HideAreaCaptcha() {
        this.IsShowAreaCaptcha = false;
        this.CaptchaCode = null;
        this.CaptchaKey = null;
    }



    private captchaCode: string = "";
    get CaptchaCode() { return this.captchaCode; }
    set CaptchaCode(value) {
        if (this.captchaCode != value) {
            this.captchaCode = value;
        }

        if (this.captchaCode) {
            if (this.HasCaptchaErrors) {
                this.HasCaptchaErrors = false;
                this.HasErrors = false;
            }
        }
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

            this.Succeeded = false;
            this.HasErrors = false;
            this.ErrorMessage = "";
            this.HasCaptchaErrors = false;

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

                    if (userdata.InValidCaptcha) {
                        this.CaptchaCode = "";
                        this.IsShowAreaCaptcha = true;
                        this.CaptchaImageUrl = userdata.CaptchaImage;
                        this.HasCaptchaErrors = true;

                    }

                    var errorMessage = "";

                    if (userdata.ExceptionMessage) alert(userdata.ExceptionMessage);
                    else {
                        if (userdata.InValidCaptcha) errorMessage = "Please re-enter the characters you see in the image above";
                        if (userdata.IpRestricted) errorMessage = "Unauthorized IP Address. Your IP is not authorized to access this account!";
                        if (userdata.InActive) errorMessage = "Your account has been deactivated!" + "please contact your administrator.";
                        if (errorMessage) {
                            this.HasErrors = true;
                            this.ErrorMessage = errorMessage;
                        }
                        else {
                            this.Succeeded = true;
                            this.HasErrors = false;
                            this.HideAreaCaptcha();
                        }
                    }

      

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
