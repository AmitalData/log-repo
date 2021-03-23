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
    public Id = "";
    public MainLogo: string = "";
    public ContactUsEmail: string = "";  
    public showSpinner = true; 

    public show = true;
    constructor( 
        private ss: LoginService,
        private privateLabelsBrandingDataService: PrivateLabelsBrandingDataService) {
        super(ss); 
    }

    ngOnInit() { 
        this.privateUrl = SessionInfo.GetLogitudeURL();  
        this.GetPrivateLabelsData(this.privateUrl); 
    }


    GetPrivateLabelsData(privateUrl: string) {
        this.privateLabelsBrandingDataService.GetUserDashboardBrandingData(BrandingDataService.GetPrivateLabelsDataRequest(privateUrl)).subscribe((response: ServiceResponse) => {
            if (response.Result) {
                //BrandingDataService.SetPrivateLabelsDataRequest(response.Result, privateUrl);
                this.ContactUsEmail = response.Result.ContactUsEmail;
                this.MainColor = response.Result.MainColor;
                this.BackgroundImage = BrandingDataService.GetBackgroundImage(); 
                this.ForgetPasswordImage = BrandingDataService.GetForgetPasswordImage();
                this.Id = response.Result.Id; 
                this.MainLogo = BrandingDataService.GetMainLogo();   
            }
        },
            (error) => {
                this.BackgroundImage = BrandingDataService.DefaultBackground;
                this.ForgetPasswordImage = BrandingDataService.DefaultForgetPassword;
                this.MainLogo = BrandingDataService.DefaultMainLogo;
            },
        )
        this.showSpinner = false;
    } 
}
