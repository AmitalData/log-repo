declare var window: any;
import { Component, OnInit } from '@angular/core';
import { Router, RouteReuseStrategy } from '@angular/router';
import { LoginExtendedService } from '../../core/Services/login-extended.service';
import { SessionInfo } from '../../core/Infrastructure/Utilities/SessionInfo';
import { AuthService } from '../../core/Services/auth.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { FeatureLocator } from '../../core/Infrastructure/Utilities/FeatureLocator';
import { LoginService } from '../../core/Infrastructure/Services/LoginService';
import { InfrastructureDomainService } from '../../core/Infrastructure/Services/InfrastructureDomainService';
import { HostScreenService } from '../../core/Services/host-screen.service';

@Component({
    selector: 'app-login',
    standalone: true,
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css'],
    imports: [FormsModule, CommonModule],
})

export class LoginComponent implements OnInit {
    public LogoImgSrc: string = "./assets/images/UnifreightLogo.jpg";
    public Email: string = "";
    public Password: string = "";
    public CloseEyePass: boolean = true;
    public PasswordType: string = "password";
    public PassEyeIcon: string = "./assets/images/password_eye_closed.png";
    public PassEyeIconTitle: string = "Show Password";
    public IsShowAreaCaptcha: boolean = false;
    public CaptchaKey: string = "";
    public CaptchaImageUrl: string = "";
    public CaptchaTextValue: string = "";
    public errorMessage: string = "";
    public Tenant: number;
    public ShowbusyIndicator: boolean = false;
    public MainColor: string = "rgb(25, 105, 180)"; // "#000000";;
    public SecondaryColor: string = "rgb(184, 189, 229)"; // "#00266";
    public BackGroundImg: string = "url('assets/images/map-bg.svg')";
    public isLoadingHost: boolean = false;

    constructor(private router: Router,
        public routeReuseStrategy: RouteReuseStrategy,
        private loginExtendedService: LoginExtendedService,
        private authService: AuthService,
        private hostScreenService: HostScreenService,
        private loginService: LoginService,
        private myInfrastructureDomainService: InfrastructureDomainService) {
        this.authService.closeSession();
    }

    ngOnInit() {
        this.InitRouteHostScreen();
    }

    RedirectAppToHttps() {
        const isLocally = window.location.origin.indexOf('localhost') > -1;

        if (!isLocally && location.protocol === 'http:') {
            window.location.href = location.href.replace('http', 'https');
        }
    }

    public passEyeClicked() {
        this.CloseEyePass = !this.CloseEyePass;
        this.PassEyeIcon = this.CloseEyePass ? "./assets/images/password_eye_closed.png" : "./assets/images/password_eye_opened.png";
        this.PassEyeIconTitle = this.CloseEyePass ? "Show Password" : "Hide Password";
        this.PasswordType = this.CloseEyePass ? "password" : "text";
    }

    public LogInClicked() {
        this.ShowbusyIndicator = true;
        this.errorMessage = "";
        const isCustomsBookSite = true;

        let LoginParams = {
            Email: this.Email,
            Password: this.Password,
            ByToken: false,
            CardId: "",
            CardType: "",
            IsMobileLogin: false,
            IsUser: !isCustomsBookSite,
            GetToken: true,
            IsAngularLogin: true,
            MobileVersion: "",
            ClientType: "Web",
            CaptchaKey: this.CaptchaKey,
            CaptchaCode: this.CaptchaTextValue,
            IsCargoTracking: false,
            IsCustomsBook: isCustomsBookSite,
        };

        this.loginExtendedService.PostUserValidation(LoginParams).subscribe((userData: any) => {
            if ((userData && (userData.HasError == true || userData.ExceptionMessage)) || !userData) {
                this.LoginFailed(userData);
                this.ShowbusyIndicator = false;
            }
            else this.Login(LoginParams, userData);
        });
    }

    private LoginFailed(userData: any) {
        this.CaptchaKey = userData ? userData.CaptchaKey : "";
        if (userData && userData.ExceptionMessage) {
            alert(userData.ExceptionMessage);
        }
        if (userData.MustChangePassword) {
            this.router.navigate(["changepassword"], { queryParams: { email: this.Email } });
        } else if (userData.PasswordExpirationDateMessage) {
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

    private Login(LoginParams: any, userData: any) {
        this.Tenant = userData.Tenant;
        this.errorMessage = "";
        let tenantList = userData.CompanyLogins;
        let LogInToTenant = tenantList.filter(tenan => tenan.Tenant == this.Tenant)[0];
        SessionInfo.DisplayCookies = true;
        sessionStorage.setItem("DisplayCookies", JSON.stringify(true));

        if (tenantList?.length > 1 && LoginParams.IsCustomsBook && !LogInToTenant) {
            this.errorMessage = "Login failed! Feature installed on more than one company.";
            this.ShowbusyIndicator = false;
        }
        else if (!LogInToTenant) {
            this.errorMessage = "Login failed! unauthorized user.";
            this.ShowbusyIndicator = false;
        }
        else {
            SessionInfo.LoggedUserCompanyLogins = userData.CompanyLogins;
            sessionStorage.setItem("LoggedUserCompanyLogins", JSON.stringify(userData.CompanyLogins));
            this.loginExtendedService.PostLoginData(LoginParams, LogInToTenant.Tenant).subscribe((userData: any) => {
                this.ShowbusyIndicator = false;
                SessionInfo.IsAdmin = userData.IsAdmin;
                if (userData) {
                    SessionInfo.LoggedUserTenant = userData.Tenant;
                    if (SessionInfo.LoggedUserTenant != 0) {
                        SessionInfo.Token = userData.Token;
                        this.loginService
                            .GetObjectTables()
                            .subscribe((myResult: any) => {
                                window.ObjectTables = myResult;
                                this.myInfrastructureDomainService
                                    .GetAllowedFeaturesForLoggedUser()
                                    .subscribe((myResponse: any) => {
                                        if (!FeatureLocator.HasFeaturePermession("Customs.CB_CustomsItemComputedData", "CustomsBookFeature")) {
                                            this.errorMessage = "You have no permission to access this feature";
                                            return;
                                        }
                                        else {
                                            this.FillSessionInfoData(userData);
                                            this.RouteToMainPage();
                                        }
                                    });
                            });
                    }
                    else {
                        this.FillSessionInfoData(userData);
                        this.RouteToMainPage();
                    }
                }
            });
            this.GetLoggedUserPM(LoginParams.Email, LogInToTenant.Tenant);
        }
    }

    private GetLoggedUserPM(email: any, tenant: any) {
        this.loginExtendedService.GetLoggedUser(email, tenant).subscribe((loggedUserPM: any) => {
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

    private RouteToMainPage() {
        if (this.hostScreenService.redirectUrl) {
            this.router.navigate([this.hostScreenService.redirectUrl]);
            this.hostScreenService.redirectUrl = null;
        }
        else if (this.authService.redirectUrl) {
            this.router.navigate([this.authService.redirectUrl]);
            this.authService.redirectUrl = null;
        }
        else if (sessionStorage.getItem("Token")) {
            this.router.navigate([this.authService.DefaultPageCustomsBook])
        }
    }

    private InitRouteHostScreen() {
        let userData: any = sessionStorage.getItem("userdata");
        if (userData) {
            userData = JSON.parse(userData);
            userData.Tenant = userData.CurrentTenant;
        }
        if (this.hostScreenService.redirectUrl) {
            this.isLoadingHost = true;
            this.ShowbusyIndicator = false;
            if (userData) {
                SessionInfo.LoggedUserTenant = userData.Tenant;
                if (SessionInfo.LoggedUserTenant != 0) {
                    SessionInfo.Token = userData.Token;
                    this.loginService
                        .GetObjectTables()
                        .subscribe((myResult: any) => {
                            window.ObjectTables = myResult;
                            this.myInfrastructureDomainService
                                .GetAllowedFeaturesForLoggedUser()
                                .subscribe({
                                    next: (myResponse: any) => {
                                        this.isLoadingHost = false;
                                        if (!FeatureLocator.HasFeaturePermession("Customs.CB_CustomsItemComputedData", "CustomsBookFeature")) {
                                            this.errorMessage = "You have no permission to access this feature";
                                            return;
                                        } 
                                        else {
                                            this.FillSessionInfoData(userData);
                                            this.RouteToMainPage();
                                        }
                                    },
                                    error: () => {
                                        this.isLoadingHost = false;
                                        this.errorMessage = "An error occurred while loading features.";
                                    }
                                });
                        });
                }
                else {
                    this.isLoadingHost = false;
                    this.FillSessionInfoData(userData);
                    this.RouteToMainPage();
                }
            }
            else this.isLoadingHost = false;        
        }
        else if (this.authService.redirectUrl) {
            this.router.navigate([this.authService.redirectUrl]);
            this.authService.redirectUrl = null;
        }
        else if (sessionStorage.getItem("Token")) {
            this.router.navigate([this.authService.DefaultPageCustomsBook])
        }

    }

    public ForgotPasswordClicked() {
        if (this.Tenant) this.router.navigate(["resetpassword"]);//,{ queryParams: {tenant: this.Tenant}}
        else this.router.navigate(["resetpassword"]);
    }
}
