import { Component } from '@angular/core';
 
import { Headers } from '@angular/http'; 
import { Router } from '@angular/router';
import { ResetPasswordComponent } from '../../Components/ResetPasswordComponent';
import { LoginService } from '../../LoginService';  
import { SessionInfo } from '../../SessionInfo';
import { ServiceResponse } from '../DataContracts/ServiceResponse';
import { BrandingDataService } from '../Services/BrandingDataService';
import { HybridLabelsBrandingDataService } from '../Services/HybridLabelsBrandingDataService';

@Component({
    selector: 'HybridResetPasswordComponent',
    moduleId: './Login/Components/',
    templateUrl: 'HybridResetPasswordComponent.html',
    styleUrls: ['HybridResetPasswordComponent.css']
})
export class HybridResetPasswordComponent extends ResetPasswordComponent {

    // need spiner
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
        console.log("ngOnInit"); 

        this.privateUrl = SessionInfo.GetLogitudeURL();
        console.log("on init " + this.privateUrl);
        this.GetHybridLabelsData(this.privateUrl); 
    }


    GetHybridLabelsData(privateUrl: string) {
        this.hybridLabelsBrandingDataService.GetUserDashboardBrandingData(BrandingDataService.GetHybridLabelsDataRequest(privateUrl)).subscribe((response: ServiceResponse) => {
            if (response.Result) { 
                BrandingDataService.SetHybridLabelsDataRequest(response.Result, privateUrl);
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
