import { Component, OnInit } from '@angular/core';
import { LoginComponent } from '../../Components/LoginComponent';
import { ResetPasswordComponent } from '../../Components/ResetPasswordComponent';
import { LoginService } from '../../LoginService';
import { SessionInfo } from '../../SessionInfo';
import { ServiceResponse } from '../DataContracts/ServiceResponse';
import { BrandingDataService } from '../Services/BrandingDataService';
import { HybridLabelsBrandingDataService } from '../Services/HybridLabelsBrandingDataService';

@Component({
    selector: 'HybridLoginProcessComponent',
    moduleId: './Login/Components/',
    templateUrl: 'HybridLoginProcessComponent.html',
    styleUrls: ['HybridLoginProcessComponent.css']
})
export class HybridLoginProcessComponent extends LoginComponent implements OnInit {

    public authHeader;
    private privateUrl;
    public MainColor: string = null;
    public BackgroundImage: string = "";
    public ForgetPasswordImage: string = "";
    public LoginProgressImage: string = "";  
    public Id = "";
    public MainLogo: string = "";
    public ContactUsEmail: string = "";
    public showSpinner = true;

    public show = true;
    constructor(
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
                BrandingDataService.SetHybridLabelsDataRequest(response.Result, privateUrl);
                this.ContactUsEmail = response.Result.ContactUsEmail;
                this.MainColor = response.Result.MainColor;
                this.BackgroundImage = BrandingDataService.GetBackgroundImage();
                this.MainLogo = BrandingDataService.GetMainLogo();
                this.LoginProgressImage = BrandingDataService.GetLoginProgressImage();
                console.log(this.LoginProgressImage);
                this.showSpinner = false;
            }
        });
    }
}
