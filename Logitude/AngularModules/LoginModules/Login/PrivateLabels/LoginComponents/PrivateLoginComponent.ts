import { Component, OnInit, ViewChild, ViewContainerRef } from '@angular/core'; 
 
import { LoginComponent } from '../../Components/LoginComponent';
import { LoginService } from '../../LoginService';
import { SessionInfo } from '../../SessionInfo';
import { Tools } from '../../Utilities/Tools';
import { ServiceResponse } from '../DataContracts/ServiceResponse';
import { BrandingDataService } from '../Services/BrandingDataService';
import { PrivateLabelsBrandingDataService } from '../Services/PrivateLabelsBrandingDataService'; 
 
@Component({
    selector: 'PrivateLoginComponent',
    moduleId: './Login/Components/',
    templateUrl: 'PrivateLoginComponent.html',
    styleUrls: ['PrivateLoginComponent.css']
})
export class PrivateLoginComponent extends LoginComponent implements OnInit { 
    public authHeader;
    private privateUrl;
    public MainColor: string = null;
    public BackgroundImage: string = "";
    public MainImage: string = ""; 
    public MainLogo: string = "";
    public SmallLogo: string = ""; 
    public showSpinner = true;
    constructor(
        private ss: LoginService,
        private privateLabelsBrandingDataService: PrivateLabelsBrandingDataService) {
        super(ss);
    }
    ngOnInit() {
        this.get_cookie_data();
        this.privateUrl = SessionInfo.GetLogitudeURL();
        this.GetPrivateLabelsData(this.privateUrl);
    }
     

    GetPrivateLabelsData(privateUrl: string) {
        this.privateLabelsBrandingDataService.GetUserDashboardBrandingData(BrandingDataService.GetPrivateLabelsDataRequest(privateUrl)).subscribe((response: ServiceResponse) => {
            if (response.Result) {
                this.Tenant = response.Result.Tenant;
                BrandingDataService.SetPrivateLabelsDataRequest(response.Result, privateUrl);
                this.MainColor = response.Result.MainColor;
                this.BackgroundImage = BrandingDataService.GetBackgroundImage();
                this.MainImage = BrandingDataService.GetMainImage(); 
                this.MainLogo = BrandingDataService.GetMainLogo();
                this.SmallLogo = BrandingDataService.GetSmallLogo(); 
            }
        },
            (error) => {
                this.BackgroundImage = BrandingDataService.DefaultBackground;
                this.MainImage = BrandingDataService.DefaultMainImage;
                this.MainLogo = BrandingDataService.DefaultMainLogo;
            },
        )
        this.showSpinner = false;
    } 

    private ClearLocation() {
        if (SessionInfo.MainLocation) {
            SessionInfo.MainLocation.clear();
        }
    }

    ForgotPasswordClicked() {
        this.ClearLocation();
        Tools.DynamicLoader.Load("/Login/PrivateLabels/LoginComponents/PrivateResetPasswordComponent", SessionInfo.MainLocation)
            .then(cmpRef => {
            });
    }
}
