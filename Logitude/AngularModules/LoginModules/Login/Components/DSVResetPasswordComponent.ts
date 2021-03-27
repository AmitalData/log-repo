import { Component } from '@angular/core';
import {LoginService, LoginParameters} from '../LoginService';
import {Headers} from '@angular/http';
import {SessionInfo} from '../SessionInfo';
import {PasswordChangeService} from '../PasswordChangeService';
import {Tools} from '../Utilities/Tools';
import {ResetPasswordComponent} from './ResetPasswordComponent'; 
import { Router } from '@angular/router';
import { PrivateLabelsBrandingDataService } from '../PrivateLabels/Services/PrivateLabelsBrandingDataService';
import { BrandingDataService } from '../PrivateLabels/Services/BrandingDataService';
import { ServiceResponse } from '../PrivateLabels/DataContracts/ServiceResponse';

@Component({
    selector: 'DSVResetPasswordComponent',
    moduleId: './Login/Components/',
    templateUrl: 'DSVResetPasswordComponent.html',
    styleUrls: ['ChangePasswordComponent.css']
})
export class DSVResetPasswordComponent extends ResetPasswordComponent { 
     
    public authHeader;
    private privateUrl;
    public MainColor: string = null;
    public BackgroundImage: string = "";
    public ForgetPasswordImage: string = ""; 
    public MainLogo: string = "";
    public ContactUsEmail: string = sessionStorage.getItem('ContactEmail');  

    constructor( 
        private ss: LoginService) {
        super(ss); 
    }

    ngOnInit() { 
        this.privateUrl = SessionInfo.GetLogitudeURL();  
        this.GetPrivateLabelsData(); 
    }


    GetPrivateLabelsData() {  
         this.BackgroundImage = BrandingDataService.GetBackgroundImage(); 
         this.ForgetPasswordImage = BrandingDataService.GetForgetPasswordImage(); 
         this.MainLogo = BrandingDataService.GetMainLogo();   
     }
          
}
