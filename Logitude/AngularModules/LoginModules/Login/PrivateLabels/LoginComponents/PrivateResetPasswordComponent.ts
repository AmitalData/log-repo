import { Component } from '@angular/core'; 
import { ResetPasswordComponent } from '../../Components/ResetPasswordComponent';
import { LoginService } from '../../LoginService';  
import { SessionInfo } from '../../SessionInfo';
import { ServiceResponse } from '../DataContracts/ServiceResponse';
import { BrandingDataService } from '../Services/BrandingDataService';
import { PrivateLabelsBrandingDataService } from '../Services/PrivateLabelsBrandingDataService';

@Component({
    selector: 'PrivateResetPasswordComponent',
    moduleId: './Login/Components/',
    templateUrl: 'PrivateResetPasswordComponent.html',
    styleUrls: ['PrivateResetPasswordComponent.css']
})
export class PrivateResetPasswordComponent extends ResetPasswordComponent {
     

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