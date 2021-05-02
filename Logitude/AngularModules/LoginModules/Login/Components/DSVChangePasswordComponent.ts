import { Component } from '@angular/core';
import {LoginService} from '../LoginService'; 
import {PasswordChangeService} from '../PasswordChangeService'; 
import {ChangePasswordComponent} from './ChangePasswordComponent'; 
import { BrandingDataService } from '../PrivateLabels/Services/BrandingDataService'; 

@Component({
    selector: 'DSVChangePasswordComponent',
    moduleId: './Login/Components/',
    templateUrl: 'DSVChangePasswordComponent.html',
    styleUrls: ['ChangePasswordComponent.css']
})
export class DSVChangePasswordComponent extends ChangePasswordComponent {

    public BackgroundImage: string = "";
    public ForgetPasswordImage: string = "";
    public MainLogo: string = ""; 
    public MainColor: string = null;

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
        this.MainColor = BrandingDataService.GetColor("MainColor");  
    }
}