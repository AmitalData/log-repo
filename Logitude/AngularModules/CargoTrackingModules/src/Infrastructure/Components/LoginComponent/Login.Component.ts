import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';


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

    constructor(private router: Router) {
        
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
        this.router.navigate([1,'search'])
    }
}