import { Component } from '@angular/core';  
import { ChangePasswordComponent } from '../../Components/ChangePasswordComponent';
import { LoginService } from '../../LoginService';
import { PasswordChangeService } from '../../PasswordChangeService';  
import { BrandingDataService } from '../Services/BrandingDataService';  

@Component({
    selector: 'PrivateChangePasswordComponent',
    moduleId: './Login/Components/',
    templateUrl: 'PrivateChangePasswordComponent.html',
    styleUrls: ['PrivateChangePasswordComponent.css']
})
export class PrivateChangePasswordComponent extends ChangePasswordComponent {
     
    public BackgroundImage: string = "";
    public ForgetPasswordImage: string = ""; 
    public MainLogo: string = "";   
    public SecondaryColor: string = null;

    constructor(public ss: PasswordChangeService, public ll: LoginService) {
        super(ss, ll); 

    }

    ngOnInit() { 
        this.GetPrivateLabelsData();
    }


    GetPrivateLabelsData() {     
        this.BackgroundImage = BrandingDataService.GetImage("BackgroundImage");
        this.MainLogo = BrandingDataService.GetImage("MainLogo");
        this.ForgetPasswordImage = BrandingDataService.GetImage("ForgetPasswordImage");   
        this.SecondaryColor = BrandingDataService.GetColor("SecondaryColor");  
    } 
}