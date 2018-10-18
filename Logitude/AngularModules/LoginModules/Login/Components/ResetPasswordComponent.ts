import { Component } from '@angular/core';
import {LoginService, LoginParameters} from '../LoginService';
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
    constructor(public _LoginService: LoginService) {

    }


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
        else {
            this.HasErrors = false;
            this._LoginService.GetRequestResetUserPassword(this.Email, false).subscribe(userdata => {
                this.ShowbusyIndicator = false;

                if (!userdata.HasError) {

                    this.Succeeded = true;
                    this.HasErrors = false;
                }
                else {


                    //disableForm(false);

                    var errorMessage = "Submit failed! invalid email." + "<br/>";


                    if (userdata.IpRestricted) {

                        errorMessage = "Trying to submit in from unauthorised station!" + "<br/>" + "(The IP address you are trying to " + "<br/>" + "submit from is restricted for this user)";//

                    }

                    if (userdata.IsLocked) {

                        errorMessage = "Your account has been locked out!" + "<br/>" + "please contact your administrator.";
                    }


                    if (userdata.InActive) {
                        errorMessage = "Your account has been deactivated!" + "<br/>" + "please contact your administrator.";
                    }

                    //document.getElementById("errorsList").innerHTML = errorMessage;
                    // $("#errorsList").text(errorMessage);
                    //$("#errorsList").show();
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
