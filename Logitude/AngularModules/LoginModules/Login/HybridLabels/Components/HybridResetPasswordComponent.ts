import { Component } from '@angular/core'; 
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
                this.MainLogo = BrandingDataService.GetMainLogo();
                this.showSpinner = false; 

            }
        });

    }

}
