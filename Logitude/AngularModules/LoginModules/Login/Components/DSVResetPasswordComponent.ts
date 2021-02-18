import { Component } from '@angular/core';
import {LoginService, LoginParameters} from '../LoginService';
import {Headers} from '@angular/http';
import {SessionInfo} from '../SessionInfo';
import {PasswordChangeService} from '../PasswordChangeService';
import {Tools} from '../Utilities/Tools';
import {ResetPasswordComponent} from './ResetPasswordComponent'; 
import { Router } from '@angular/router';
import { HybridLabelsBrandingDataService } from '../HybridLabels/Services/HybridLabelsBrandingDataService';
import { BrandingDataService } from '../HybridLabels/Services/BrandingDataService';
import { ServiceResponse } from '../HybridLabels/DataContracts/ServiceResponse';

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

    public show = true;
    constructor(
        private router: Router,
        private ss: LoginService,
        private hybridLabelsBrandingDataService: HybridLabelsBrandingDataService) {
        super(ss);
    }

    ngOnInit() { 
        this.privateUrl = SessionInfo.GetLogitudeURL(); 
        this.GetHybridLabelsData(this.privateUrl);
    }


    GetHybridLabelsData(privateUrl: string) {
        this.hybridLabelsBrandingDataService.GetUserDashboardBrandingData(BrandingDataService.GetHybridLabelsDataRequest(privateUrl)).subscribe((response: ServiceResponse) => {
            if (response.Result) {
                //BrandingDataService.SetHybridLabelsDataRequest(response.Result, privateUrl);
                this.ContactUsEmail = response.Result.ContactUsEmail;
                this.MainColor = response.Result.MainColor;
                this.BackgroundImage = BrandingDataService.GetBackgroundImage(); 
                this.ForgetPasswordImage = BrandingDataService.GetForgetPasswordImage();
                this.Id = response.Result.Id;
                this.MainLogo = response.Result.MainLogo;
                this.MainLogo = BrandingDataService.GetMainLogo(); 
            }
            else {
                this.GoToError401();
            }
        });

    }

    private GoToError401() {
        this.router.navigate(['Error401']);
    }

}
