import { Component } from '@angular/core';  
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
     
    public BackgroundImage: string = "";
    public ForgetPasswordImage: string = ""; 
    public MainLogo: string = "";   
    public SecondaryColor: string = null;
    private privateUrl;

    constructor(public ss: PasswordChangeService, public ll: LoginService, private privateLabelsBrandingDataService: PrivateLabelsBrandingDataService) {
        super(ss, ll); 

    }

    ngOnInit() { 
        this.GetPrivateLabelsData();
        this.privateUrl = SessionInfo.GetLogitudeURL();
        this.GetPrivateLabelsColor(this.privateUrl);

    }


    GetPrivateLabelsData() {     
        this.BackgroundImage = BrandingDataService.GetImage("BackgroundImage");
        this.MainLogo = BrandingDataService.GetImage("MainLogo");
        this.ForgetPasswordImage = BrandingDataService.GetImage("ForgetPasswordImage");  
        this.SecondaryColor = BrandingDataService.SecondaryColor;  
    }

    GetPrivateLabelsColor(privateUrl: string) {
        this.privateLabelsBrandingDataService.GetUserDashboardBrandingData(BrandingDataService.GetPrivateLabelsDataRequest(privateUrl)).subscribe((response: ServiceResponse) => {
            if (response.Result) {
                this.SecondaryColor = response.Result.SecondaryColor;
                BrandingDataService.SecondaryColor = this.SecondaryColor;
                //BrandingDataService.SetPrivateLabelsDataRequest(response.Result, privateUrl);
            }
        }) 
    } 

}