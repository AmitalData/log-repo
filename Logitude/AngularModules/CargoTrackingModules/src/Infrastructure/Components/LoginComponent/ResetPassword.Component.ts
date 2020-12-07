import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CargoTrackingBrandingData } from 'src/CargoTracking/DataContracts/CargoTrackingBrandingData';
import { ServiceResponse } from 'src/CargoTracking/DataContracts/ServiceResponse';
import { CargoTrackingBrandingDataExtendedService } from 'src/CargoTracking/Services/Others/CargoTrackingBrandingDataExtendedService';
import { ServiceHelper } from 'src/CargoTracking/Utilities/ServiceHelper';
import { CommonDataExtendedService } from 'src/Infrastructure/Services/Extended/CommonDataExtendedService';
import { LoginExtendedService } from 'src/Infrastructure/Services/Extended/LoginExtendedService';
import { LoginServiceHelper } from 'src/Infrastructure/Utilities/LoginServiceHelper';

@Component({
    selector: 'resetPassword',
    templateUrl: './ResetPassword.Component.html',
    styleUrls: ['./ResetPassword.Component.css']
})

export class ResetPasswordComponent implements OnInit {
    public Email: string = "";
    public LogoImgSrc: string = "";
    public IsShowAreaCaptcha: boolean = false;
    public CaptchaKey: string = "";
    public CaptchaImageUrl: string = "";
    public ErrorMessage: string = "";
    public Tenant: number;
    public Succeeded: boolean = false;
    public ShowbusyIndicator: boolean = false;
    public MainColor: string = null;
    public SecondaryColor: string = null;
    public CustomerURL: string = "";
    constructor(private router: Router,
        private route: ActivatedRoute,
        private loginExtendedService: LoginExtendedService,
        private commonDataExtendedService: CommonDataExtendedService,
        private cargoTrackingBrandingDataExtendedService: CargoTrackingBrandingDataExtendedService,
        private loginServiceHelper: LoginServiceHelper,
        @Inject('BASE_URL') baseUrl: string) {
        this.GetcargoTrackingData(baseUrl);
    }

    private GetcargoTrackingData(baseUrl:string) {
        this.LogoImgSrc = "./assets/images/logo/White.jpg";
        this.cargoTrackingBrandingDataExtendedService.GetCargoTrackingBrandingDataForPrivateSite(ServiceHelper.GetCurrentDomain(baseUrl)).subscribe((response: ServiceResponse) => { 
            if(response.Result){
                this.Tenant = response.Result.Tenant;
                this.CustomerURL = response.Result.CustomerURL;
                ServiceHelper.SetCargoTrackingDate(response.Result,baseUrl);
                this.LogoImgSrc = this.loginServiceHelper.GetLoginLogoImg();
                this.MainColor = response.Result.MainColor != null ? ServiceHelper.ConvertHexaToRGBA(response.Result.MainColor) : null;
                this.SecondaryColor = response.Result.SecondaryColor != null ? ServiceHelper.ConvertHexaToRGBA(response.Result.SecondaryColor) : null;
            }
            else{
                this.loginServiceHelper.GoToError401();
            }
        });
    }


    ngOnInit() {
        this.initComponent();
    }

    private initComponent() {
        document.body.style.background = "#fff";
    }

    private captchaCode: string = "";
    get CaptchaCode() { return this.captchaCode; }
    set CaptchaCode(value) {
        if (this.captchaCode != value) {
            this.captchaCode = value;
        }
    }

    private HideAreaCaptcha() {
        this.IsShowAreaCaptcha = false;
        this.CaptchaCode = null;
        this.CaptchaKey = null;
    }

    public SubmitClicked() {
        this.ShowbusyIndicator = true;

        if (!this.Email) {
            this.ErrorMessage = "Email can't be empty!";
            this.ShowbusyIndicator = false;
        }
        else if (!this.ValidateEmail(this.Email)) {
            this.ErrorMessage = "Your email address is invalid!";
            this.ShowbusyIndicator = false;
        }
        else if (this.IsShowAreaCaptcha && !this.CaptchaCode) {
            this.ErrorMessage = "Please re-enter the characters you see in the image above";
            this.ShowbusyIndicator = false;
        }
        else {
            this.ErrorMessage = "";
            this.Succeeded = false;

            let params = {
                Email: this.Email,
                IsChampLogin: false,
                CaptchaCode: this.CaptchaCode,
                CaptchaKey: this.CaptchaKey,
                PageName: "changepassword",
                Domain: this.CustomerURL + "/Cargo-Tracking",
                BrandingTenant: this.Tenant ? this.Tenant.toString() : "",
            }

            this.loginExtendedService.PostRequestResetUserPassword(params).subscribe((userData: any) => {
                this.ShowbusyIndicator = false;
                if (!userData.HasError) {
                    this.Succeeded = true;
                    this.HideAreaCaptcha();
                }
                else {
                    this.Succeeded = false;
                    this.CaptchaKey = userData ? userData.CaptchaKey : "";

                    if (userData.InValidCaptcha) {
                        this.CaptchaCode = "";
                        this.IsShowAreaCaptcha = true;
                        this.CaptchaImageUrl = userData.CaptchaImage;
                    }
                    if (userData.ExceptionMessage) alert(userData.ExceptionMessage);
                    else this.SetErrorMessage(userData);
                }
            });
        }

    }

    private ValidateEmail(email) {
        var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(email);
    }

    private SetErrorMessage(userData: any) {
        this.ErrorMessage = "";
        if (userData.InValidCaptcha) this.ErrorMessage = "Please re-enter the characters you see in the image above";
        else if (userData.IpRestricted) this.ErrorMessage = "Unauthorized IP Address. Your IP is not authorized to access this account!";
        else if (userData.InActive) this.ErrorMessage = "Your account has been deactivated!" + "please contact your administrator.";
        else this.ErrorMessage = "Error.";
    }

    public BackToLoginClicked() {
        //this.Tenant = this.route.snapshot.queryParams?.tenant;
        if(this.Tenant)
            this.router.navigate(["Cargo-Tracking/login"]);//,{ queryParams: {tenant: this.Tenant}}
        else
            this.router.navigate(["Cargo-Tracking/login"]);
    }
}