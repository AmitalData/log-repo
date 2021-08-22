import { Component } from '@angular/core';
import {LoginService} from '../LoginService'; 
import {PasswordChangeService} from '../PasswordChangeService'; 
import {ChangePasswordComponent} from './ChangePasswordComponent'; 
import { BrandingDataService } from '../PrivateLabels/Services/BrandingDataService'; 
import { SessionInfo } from '../SessionInfo';
import { PrivateLabelsBrandingDataService } from '../PrivateLabels/Services/PrivateLabelsBrandingDataService';
import { ServiceResponse } from '../PrivateLabels/DataContracts/ServiceResponse';

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
    public SecondaryColor: string = null;
    private privateUrl;

    constructor(public ss: PasswordChangeService, public ll: LoginService, private privateLabelsBrandingDataService: PrivateLabelsBrandingDataService) {
        super(ss, ll);

    }

    ngOnInit() {
        this.privateUrl = SessionInfo.GetLogitudeURL();
        this.GetImagesFromCash();
        this.GetPrivateLabelsImages(this.privateUrl);
         
    }

    GetImagesFromCash() {
        this.BackgroundImage = BrandingDataService.GetImage("BackgroundImage");
        this.MainLogo = BrandingDataService.GetImage("MainLogo");
        this.ForgetPasswordImage = BrandingDataService.GetImage("ForgetPasswordImage");
        this.SecondaryColor = BrandingDataService.GetColor("SecondaryColor");
    }

    GetPrivateLabelsImages(privateUrl: string) {
        this.privateLabelsBrandingDataService.GetUserDashboardBrandingData(BrandingDataService.GetPrivateLabelsDataRequest(privateUrl)).subscribe((response: ServiceResponse) => {
            if (response.Result) {
                BrandingDataService.SetPrivateLabelsDataRequest(response.Result, privateUrl);
                this.GetImagesFromCash();
            }
        })
    }
}