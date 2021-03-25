import { Component } from '@angular/core'; 
import { Headers } from '@angular/http'; 
import { ChangePasswordComponent } from '../../Components/ChangePasswordComponent';
import { LoginService } from '../../LoginService';
import { PasswordChangeService } from '../../PasswordChangeService';
import { SessionInfo } from '../../SessionInfo';
import { ServiceResponse } from '../DataContracts/ServiceResponse';
import { BrandingDataService } from '../Services/BrandingDataService';
import { PrivateLabelsBrandingDataService } from '../Services/PrivateLabelsBrandingDataService';

@Component({
    selector: 'PrivateChangePasswordComponent',
    moduleId: './Login/Components/',
    templateUrl: 'PrivateChangePasswordComponent.html',
    styleUrls: ['PrivateChangePasswordComponent.css']
})
export class PrivateChangePasswordComponent extends ChangePasswordComponent {

    public MainColor: string = null;
    public BackgroundImage: string = "";
    public ForgetPasswordImage: string = "";
    public Id = "";
    public MainLogo: string = "";
    public ContactUsEmail: string = "";
    public showSpinner = true; 
    private privateUrl; 

    constructor(public ss: PasswordChangeService, public ll: LoginService, private privateLabelsBrandingDataService: PrivateLabelsBrandingDataService) {
        super(ss, ll);
        console.log("CHANGE PRIVATE");

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