import { Component, OnInit, ViewChild, ViewContainerRef } from '@angular/core'; 
 
import { LoginComponent } from '../../Components/LoginComponent';
import { LoginService } from '../../LoginService';
import { SessionInfo } from '../../SessionInfo';
import { Tools } from '../../Utilities/Tools';
import { ServiceResponse } from '../DataContracts/ServiceResponse';
import { BrandingDataService } from '../Services/BrandingDataService';
import { HybridLabelsBrandingDataService } from '../Services/HybridLabelsBrandingDataService'; 
 
@Component({
    selector: 'HybridLoginComponent',
    moduleId: './Login/Components/',
    templateUrl: 'HybridLoginComponent.html',
    styleUrls: ['HybridLoginComponent.css']
})
export class HybridLoginComponent extends LoginComponent implements OnInit { 
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
        private hybridLabelsBrandingDataService: HybridLabelsBrandingDataService) {
        super(ss);
    }
    ngOnInit() {
        this.get_cookie_data();
        this.privateUrl = SessionInfo.GetLogitudeURL();
        this.GetHybridLabelsData(this.privateUrl);
    }
     

    GetHybridLabelsData(privateUrl: string) {
        this.hybridLabelsBrandingDataService.GetUserDashboardBrandingData(BrandingDataService.GetHybridLabelsDataRequest(privateUrl)).subscribe((response: ServiceResponse) => {
            if (response.Result) {
                this.Tenant = response.Result.Tenant;
                BrandingDataService.SetHybridLabelsDataRequest(response.Result, privateUrl);
                this.MainColor = response.Result.MainColor;
                this.BackgroundImage = BrandingDataService.GetBackgroundImage();
                this.MainImage = BrandingDataService.GetMainImage(); 
                this.MainLogo = BrandingDataService.GetMainLogo();
                this.SmallLogo = BrandingDataService.GetSmallLogo();
                this.showSpinner = false;
            } 
        });

    }

    private ClearLocation() {
        if (SessionInfo.MainLocation) {
            SessionInfo.MainLocation.clear();
        }
    }

    ForgotPasswordClicked() {
        this.ClearLocation();
        Tools.DynamicLoader.Load("/Login/HybridLabels/Components/HybridResetPasswordComponent", SessionInfo.MainLocation)
            .then(cmpRef => {
            });
    }
}
