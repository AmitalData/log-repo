import { Component, Inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/auth.service';
import { CargoTrackingBrandingData } from 'src/CargoTracking/DataContracts/CargoTrackingBrandingData';
import { ServiceResponse } from 'src/CargoTracking/DataContracts/ServiceResponse';
import { CargoTrackingBrandingDataExtendedService } from 'src/CargoTracking/Services/Others/CargoTrackingBrandingDataExtendedService';
import { ServiceHelper } from 'src/CargoTracking/Utilities/ServiceHelper';
import { CommonDataExtendedService } from 'src/Infrastructure/Services/Extended/CommonDataExtendedService';
import { LoginExtendedService } from 'src/Infrastructure/Services/Extended/LoginExtendedService';
import { LoginServiceHelper } from 'src/Infrastructure/Utilities/LoginServiceHelper';
import { SessionInfo } from 'src/Infrastructure/Utilities/SessionInfo';

@Component({
    selector: 'login',
    templateUrl: './Login.Component.html',
    styleUrls: ['./Login.Component.css']
})

export class LoginComponent implements OnInit {
    public LogoImgSrc: string = "";
    public Email: string = "";
    public Password: string = "";
    public CloseEyePass: boolean = true;
    public PasswordType: string = "password";
    public PassEyeIcon: string = "./assets/images/icons/password_eye_closed.png";
    public PassEyeIconTitle: string = "Show Password";
    public IsShowAreaCaptcha: boolean = false;
    public CaptchaKey: string = "";
    public CaptchaImageUrl: string = "";
    public CaptchaTextValue: string = "";
    public errorMessage: string = "";
    public Tenant: number;
    public ShowbusyIndicator: boolean = false;
    public MainColor: string = null;
    public SecondaryColor: string = null;
    constructor(private router: Router,
        private route: ActivatedRoute,
        private loginExtendedService: LoginExtendedService,
        private commonDataExtendedService: CommonDataExtendedService,
        private authService: AuthService,
        private cargoTrackingBrandingDataExtendedService: CargoTrackingBrandingDataExtendedService,
        private loginServiceHelper: LoginServiceHelper,
        @Inject('BASE_URL') baseUrl: string) {
        this.RouteToMainPage();
        this.GetcargoTrackingData(baseUrl);
    }

    private GetcargoTrackingData(baseUrl:string) {
        this.LogoImgSrc = "./assets/images/logo/White.jpg";
        this.cargoTrackingBrandingDataExtendedService.GetUserDashboardBrandingData(ServiceHelper.GetcargoTrackingDataRequest(baseUrl)).subscribe((response: ServiceResponse) => {
            if(response.Result){
                this.Tenant = response.Result.Tenant;
                ServiceHelper.SetCargoTrackingDate(response.Result,baseUrl);
                this.LogoImgSrc = this.loginServiceHelper.GetLoginLogoImg();
                this.MainColor = response.Result.MainColor;
                this.SecondaryColor = response.Result.SecondaryColor;
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
        // document.body.style.background = "#fff";
    }

    public passEyeClicked() {
        this.CloseEyePass = !this.CloseEyePass;
        this.PassEyeIcon = this.CloseEyePass ? "./assets/images/icons/password_eye_closed.png" : "./assets/images/icons/password_eye_opened.png";
        this.PassEyeIconTitle = this.CloseEyePass ? "Show Password" : "Hide Password";
        this.PasswordType = this.CloseEyePass ? "password" : "text";
    }

    public LogInClicked() {
        this.ShowbusyIndicator = true;
        this.errorMessage = "";

        let LoginParams = {
            Email: this.Email,
            Password: this.Password,
            ByToken: false,
            CardId: "",
            CardType: "",
            IsMobileLogin: false,
            IsUser: true,
            GetToken: true,
            IsAngularLogin: true,
            MobileVersion: "",
            ClientType: "Web",
            CaptchaKey: this.CaptchaKey,
            CaptchaCode: this.CaptchaTextValue
        };

        this.loginExtendedService.PostUserValidation(LoginParams).subscribe((userData: any) => {
            if ((userData && (userData.HasError == true || userData.ExceptionMessage)) || !userData) {
                this.LoginFailed(userData);
                this.ShowbusyIndicator = false;
            }
            else this.LoginSucceeded(LoginParams, userData);
        });
    }

    private LoginFailed(userData: any) {
        this.CaptchaKey = userData ? userData.CaptchaKey : "";

        if (userData && userData.ExceptionMessage) {
            alert(userData.ExceptionMessage);
        }

        if (userData.MustChangePassword) {
            //Must Change Password
            this.errorMessage = "Must Change Password";
        } else if (userData.PasswordExpirationDateMessage) {
            //Password Expired
            this.errorMessage = "Password Expired";
        }
        else {
            if (userData.InValidCaptcha) this.SetCaptchaImage(userData.CaptchaImage);

            this.SetErrorMessage(userData);
        }
    }

    private SetCaptchaImage(captchaImage) {
        if (this.IsShowAreaCaptcha) this.CaptchaTextValue = "";

        this.IsShowAreaCaptcha = true;
        this.CaptchaImageUrl = captchaImage;
    }

    private SetErrorMessage(userData: any) {
        this.errorMessage = "";

        if (userData.IpRestricted) this.errorMessage = "Trying to log in from unauthorised station!" + " (The IP address you are trying to " + " log in from is restricted for this user)";
        else if (userData.InActive) this.errorMessage = "Your account has been deactivated!" + "<br/>" + "please contact your administrator.";
        else if (userData.Unlicensed) this.errorMessage = "Your account is unlicensed!" + " please contact your administrator.";
        else if (userData.InValidMailOrPassword) this.errorMessage = "Login failed! invalid user name or password.";
        else if (userData.InValidCaptcha && userData.CaptchaImage) this.errorMessage = "Please re-enter the characters you see in the image above";
        else this.errorMessage = "Login failed! invalid user name or password.";
    }

     private GoToError401(){
        this.router.navigate(['Error401']);
    }

    private LoginSucceeded(LoginParams: any, userData: any) {
        this.errorMessage = "";
        let tenantList = userData.CompanyLogins;
        let LogInToTenant  = tenantList.filter(tenan => tenan.Tenant == this.Tenant)[0];
        if(!LogInToTenant) {
            this.errorMessage = "Login failed! unauthorized user.";
            this.ShowbusyIndicator = false;
        }
        else {
            SessionInfo.LoggedUserCompanyLogins = userData.CompanyLogins;
            sessionStorage.setItem("LoggedUserCompanyLogins", JSON.stringify(userData.CompanyLogins));

            this.loginExtendedService.PostLoginData(LoginParams, LogInToTenant.Tenant).subscribe((userData: any) => {
                this.ShowbusyIndicator = false;
                if (userData) {
                    this.FillSessionInfoData(userData);
                    this.RouteToMainPage();
                }
            });

            this.GetLoggedUserPM(LoginParams.Email, LogInToTenant.Tenant);
        }
    }

    private GetLoggedUserPM(email: any, tenant: any)
    {
        this.loginExtendedService.GetLoggedUser(email, tenant).subscribe((loggedUserPM: any) =>
        {
            if (loggedUserPM) {
                SessionInfo.LoggedUserPM = loggedUserPM;
            }
        });
    }

    private FillSessionInfoData(userData: any) {
        sessionStorage.setItem("Token", userData.Token);
        sessionStorage.setItem("LoggedUserTenant", userData.CurrentTenant);
        sessionStorage.setItem("LoggedUserEmail", userData.UserName);
        sessionStorage.setItem("LoggedUserId", userData.Id);
        sessionStorage.setItem("DocumentDownloadToken", userData.DocumentDownloadToken);

        SessionInfo.LoggedUserEmail = userData.UserName;
        SessionInfo.LoggedUserId = userData.Id;
        SessionInfo.LoggedUserTenant = userData.CurrentTenant;
        SessionInfo.Token = userData.Token;
        SessionInfo.DocumentDownloadToken = userData.DocumentDownloadToken;
    }

    private RouteToMainPage(){
        if (this.authService.redirectUrl) {
            this.router.navigate([this.authService.redirectUrl]);
            this.authService.redirectUrl = null;
          }
        else if (sessionStorage.getItem("Token")) {
            this.router.navigate([this.authService.DefaultPageCargoTracking])
        }
    }

    public ForgotPasswordClicked() {
        //this.Tenant = this.route.snapshot.queryParams?.tenant;
        if(this.Tenant)

            this.router.navigate(["Cargo-Tracking/resetpassword"]);//,{ queryParams: {tenant: this.Tenant}}
        else
            this.router.navigate(["Cargo-Tracking/resetpassword"]);
    }

}
