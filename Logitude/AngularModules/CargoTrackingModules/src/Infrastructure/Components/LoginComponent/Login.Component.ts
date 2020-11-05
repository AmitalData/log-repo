import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoginService } from 'src/Infrastructure/Services/Extended/LoginService';


@Component({
    selector: 'login',
    templateUrl: './Login.Component.html',
    styleUrls: ['./Login.Component.css']
})

export class LoginComponent implements OnInit
{
    public LogoImg: string = "";
    public Email: string = "";
    public Password: string = "";
    public CloseEyePass: boolean = true;
    public PasswordType: string = "password";
    public PassEyeIcon: string = "./assets/images/icons/password_eye_closed.png";
    public PassEyeIconTitle: string = "Show Password";

    constructor(private router: Router, private loginService: LoginService) {
        
    }

    ngOnInit()
    {
        this.initForm();
    }


    private initForm()
    {
        document.body.style.background = '#fff';
        this.LogoImg = "./assets/images/logo/UnifreightLogo.jpg";
    }

    public passEyeClicked(){
        this.CloseEyePass = !this.CloseEyePass;
        this.PassEyeIcon = this.CloseEyePass ? "./assets/images/icons/password_eye_closed.png" : "./assets/images/icons/password_eye_opened.png";
        this.PassEyeIconTitle = this.CloseEyePass ? "Show Password" : "Hide Password";
        this.PasswordType = this.CloseEyePass ? "password" : "text";
    }

    public LogInClicked()
    {   
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
            CaptchaKey: "",//"this.CaptchaKey",
            CaptchaCode: "",//this.CaptchaTextValue,

        };
        this.loginService.PostUserValidation(LoginParams).subscribe((userData: any) => {
            if(userData){
                console.log(userData);
                let tenantList = userData.CompanyLogins;
                let tenant = tenantList[0].Tenant;
                this.PostLoginData(LoginParams, tenant);
            }


        });
        //this.router.navigate([0,'dashboard'])
    }

    PostLoginData(LoginParams: any, tenant: number){
        this.loginService.PostLoginData(LoginParams, tenant).subscribe((userData: any) => {
            if(userData){
                console.log(userData);
            }


        });
    }
}