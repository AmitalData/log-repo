import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CargoTrackingBrandingData } from 'src/CargoTracking/DataContracts/CargoTrackingBrandingData';
import { ServiceResponse } from 'src/CargoTracking/DataContracts/ServiceResponse';
import { CargoTrackingBrandingDataExtendedService } from 'src/CargoTracking/Services/Others/CargoTrackingBrandingDataExtendedService';
import { ServiceHelper } from 'src/CargoTracking/Utilities/ServiceHelper';
import { CommonDataExtendedService } from 'src/Infrastructure/Services/Extended/CommonDataExtendedService';
import { ChangePasswordParameter, LoginExtendedService } from 'src/Infrastructure/Services/Extended/LoginExtendedService';
import { LoginServiceHelper } from 'src/Infrastructure/Utilities/LoginServiceHelper';

@Component({
    selector: 'changePassword',
    templateUrl: './ChangePassword.Component.html',
    styleUrls: ['./ChangePassword.Component.css']
})

export class ChangePasswordComponent implements OnInit {
    public Email: string = "";
    public LogoImgSrc: string = "";
    public ErrorMessage: string = "";
    public Tenant: number;
    public CurrentPassword: string;
    public NewPassword: string;
    public RetypePassword: string;
    public requestNumber: string;
    public IsResetPasswordViaEmail: boolean = false;
    public PasswordLenghtImg: string;
    public PasswordContainsCharactersImg: string;
    public PasswordContainsNumberImg: string;
    public MainColor: string = null;
    public SecondaryColor: string = null;

    constructor(private router: Router,
        private route: ActivatedRoute,
        private loginExtendedService: LoginExtendedService,
        private commonDataExtendedService: CommonDataExtendedService,
        private cargoTrackingBrandingDataExtendedService: CargoTrackingBrandingDataExtendedService,
        private loginServiceHelper: LoginServiceHelper,
        @Inject('BASE_URL') baseUrl: string) {
        this.GetcargoTrackingData(baseUrl);
    }

    private GoToError401(){
        this.router.navigate(['Error401']);
    }

    private GetcargoTrackingData(baseUrl:string) {
        this.LogoImgSrc = "./assets/images/logo/White.jpg";
        this.cargoTrackingBrandingDataExtendedService.GetUserDashboardBrandingData(ServiceHelper.GetcargoTrackingDataRequest(baseUrl)).subscribe((response: ServiceResponse) => {
            if(response.Result){
                this.Tenant = response.Result.Tenant;
                ServiceHelper.SetCargoTrackingDate(response.Result,baseUrl);
                this.LogoImgSrc = this.loginServiceHelper.GetLoginLogoImg();
                this.MainColor = response.Result.MainColor != null ? ServiceHelper.ConvertHexaToRGBA(response.Result.MainColor) : null;
                this.SecondaryColor = response.Result.SecondaryColor != null ? ServiceHelper.ConvertHexaToRGBA(response.Result.SecondaryColor) : null;
            }
            else{
                this.GoToError401();
            }
        });
    }

    ngOnInit() {
        this.initComponent();
    }

    private initComponent() {
        document.body.style.background = "#fff";
        this.Email =  this.route.snapshot.queryParams.email;
        this.requestNumber = this.route.snapshot.queryParams.reset_request_number;
        if(this.requestNumber) this.IsResetPasswordViaEmail = true;

        this.SetPasswordImagesAndColors();
    }

    private SetPasswordImagesAndColors(){
        this.PasswordLenghtImg = "./assets/images/changePassword/verified.png";
        this.PasswordContainsCharactersImg = "./assets/images/changePassword/verified.png";
        this.PasswordContainsNumberImg = "./assets/images/changePassword/verified.png";

        document.documentElement.style.setProperty('--PasswordLenghtColor', "gray");
        document.documentElement.style.setProperty('--PasswordContainsCharacters', "gray");
        document.documentElement.style.setProperty('--PasswordContainsNumber', "gray");
    }

    public SubmitClicked() {
        if (!this.Email) {
            this.ErrorMessage = "Your email is empty.";
            return;
        }

        if (this.NewPassword == this.RetypePassword) {
            if (!this.NewPassword) {
                this.ErrorMessage = "Confirm your password";
                return;
            }

            this.ErrorMessage = this.PasswordValidation();
            if (this.ErrorMessage) {
                return;
            }

            if (!this.IsContainsLowerUpperCase(this.NewPassword)) {
                this.ErrorMessage = "Your password must include an uppercase and lowercase letter.";
                return;
            }

            if (!this.IsContainsNumber(this.NewPassword)) {
                this.ErrorMessage = "Your password must include a number.";
                return;
            }
            if (this.NewPassword.length < 8) {
                this.ErrorMessage = "Your password must be at least 8 characters.";
                return;
            }
        }
        else {
            this.ErrorMessage = "The passwords you entered do not match.";
            return;
        }

        if (!this.IsResetPasswordViaEmail) {
            if (!this.CurrentPassword) {
                this.ErrorMessage = "Current Password can't be empty!";
                return;
            }
            else if (this.CurrentPassword == this.NewPassword) {
                this.ErrorMessage = "New password can't be the same as the current password";
            }
        }

        if (this.CurrentPassword) {
            var changePasswordParameter: ChangePasswordParameter = new ChangePasswordParameter();
            changePasswordParameter.CurrentPassword = this.CurrentPassword;
            changePasswordParameter.Email = this.Email;
            this.loginExtendedService.CheckUserPassword(changePasswordParameter).subscribe((res:any) => {

                if (!res) {
                    this.ErrorMessage = "The current password is wrong!";
                } else {
                    if (this.CurrentPassword == this.NewPassword) {
                        this.ErrorMessage = "New password can't be the same as the current password";
                    } else this.ChangePassword();

                }
            });
        }
        else this.ChangePassword();

    }

    private PasswordValidation() {
        var messageError = "";
        if (this.Email) {

            var ContainsEmail = false;
            if (this.NewPassword.toLowerCase().indexOf(this.Email.toLowerCase()) > -1) {
                ContainsEmail = true;
            }
            else {
                var emalData = [];
                var userEmail = this.Email.split('@');
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
                return messageError;
            }
        }

        var seriesMessage: string = this.IsPasswordContainsSeries(this.NewPassword);
        if (seriesMessage) {
            messageError = seriesMessage;
            return messageError;
        }
        return "";
    }

    private IsPasswordContainsSeries(password: string) {
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
    }

    private IsSeries(passwordNumnberList: any, operatorCode: string) {
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

    private ChangePassword() {
        let params = {
            RequestNumber: this.IsResetPasswordViaEmail ? this.requestNumber : null,
            NewPassword: this.NewPassword,
            OldPassword: this.CurrentPassword ? this.CurrentPassword : null,
            IsResetRequest: this.IsResetPasswordViaEmail ? true : false,
        };

        this.loginExtendedService.PostChangePassword(this.Email, params).subscribe((res:any) => {
            if (res) {
                //this.Tenant = this.route.snapshot.queryParams?.tenant;
                if(this.Tenant)

                    this.router.navigate(["Cargo-Tracking/login"]);//,{ queryParams: {tenant: this.Tenant}}
                else
                    this.router.navigate(["Cargo-Tracking/login"]);
            }
            else {
                this.ErrorMessage = "Changing password failed!"
            }
        });
    }

    private IsContainsLowerUpperCase(str: any) {
        return str.match(/[a-z]/) && str.match(/[A-Z]/);
    }

    private IsContainsNumber(str: any) {
        var regex = /\d/g;
        return regex.test(str);
    }

    public Passwordkeyup(passtring: any) {
        this.SetPasswordImagesAndColors();

        if (passtring) {
            if (passtring.length >= 8) {
                document.documentElement.style.setProperty('--PasswordLenghtColor', "green");
                this.PasswordLenghtImg = "./assets/images/changePassword/verifiedGreen.png";
            }

            if (this.IsContainsLowerUpperCase(passtring)) {
                document.documentElement.style.setProperty('--PasswordContainsCharacters', "green");
                this.PasswordContainsCharactersImg = "./assets/images/changePassword/verifiedGreen.png";
            }

            if (this.IsContainsNumber(passtring)) {
                document.documentElement.style.setProperty('--PasswordContainsNumber', "green");
                this.PasswordContainsNumberImg = "./assets/images/changePassword/verifiedGreen.png";
            }

            this.ErrorMessage = this.PasswordValidation();
        }
    }

    public capLock(e: any) {
        var kc = e.keyCode ? e.keyCode : e.which;
        if (kc == 20) { }
        else {
            var sk = e.shiftKey ? e.shiftKey : ((kc == 16) ? true : false);
            if (((kc >= 65 && kc <= 90) && !sk) || ((kc >= 97 && kc <= 122) && sk)) { }
            else { }
        }
    }
}
