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

    constructor(public ss: PasswordChangeService, public ll: LoginService) {
        super(ss, ll);

    }

    ngOnInit() { 
        this.GetPrivateLabelsData();
    }


    GetPrivateLabelsData() {
        this.BackgroundImage = BrandingDataService.GetBackgroundImage();
        this.ForgetPasswordImage = BrandingDataService.GetForgetPasswordImage();
        this.MainLogo = BrandingDataService.GetMainLogo();
    }
}