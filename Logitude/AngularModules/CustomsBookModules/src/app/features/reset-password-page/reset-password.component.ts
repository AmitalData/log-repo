import { Component, Inject, InjectionToken, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../core/Services/auth.service';
import { LoginExtendedService } from '../../core/Services/login-extended.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

function getBaseUrl(): string {
    return document.getElementsByTagName('base')[0].href;
}

export const BASE_URL = new InjectionToken<string>('BaseURL', {
	providedIn: 'root',
	factory: () => getBaseUrl()
});

@Component({
    selector: 'app-reset-password',
	standalone: true,
    templateUrl: './reset-password.component.html',
    styleUrls: ['./reset-password.component.css'],
    imports: [FormsModule, CommonModule],
})
export class ResetPasswordComponent implements OnInit {
    public Email: string = "";
    public LogoImgSrc: string = "./assets/images/UnifreightLogo.jpg";
    public IsShowAreaCaptcha: boolean = false;
    public CaptchaKey: string = "";
    public CaptchaImageUrl: string = "";
    public ErrorMessage: string = "";
    public Tenant: number;
    public Succeeded: boolean = false;
    public ShowbusyIndicator: boolean = false;
    public MainColor: string = "rgb(25, 105, 180)"; // "#000000";
    public SecondaryColor: string = "rgb(184, 189, 229)"; // "#002664";
    public CustomerURL: string = "";
    public ContactEmail: string = "mailto:support@amital.co.il";
    public BackGroundImg: string = "url('assets/images/map-bg.svg')";

    constructor(private router: Router,
        private route: ActivatedRoute,
        private loginExtendedService: LoginExtendedService,
        private authService: AuthService,
        @Inject(BASE_URL) baseUrl: string) {
            this.CustomerURL = baseUrl;    
    }

    SetContactEmail(contactEmail: string) {
        if(!contactEmail || contactEmail.length == 0) return;
        this.ContactEmail = 'mailto:' + contactEmail;
    }

    ngOnInit() {
        this.initComponent();
    }

    private initComponent() {
        // document.body.style.background = "#fff";
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
                Domain: this.CustomerURL + this.authService.DefaultPageCustomsBook,
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

            this.router.navigate(["customs-book/login"]);//,{ queryParams: {tenant: this.Tenant}}
        else
            this.router.navigate(["customs-book/login"]);
    }
}
