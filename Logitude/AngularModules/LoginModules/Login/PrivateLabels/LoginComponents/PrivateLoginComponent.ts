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
    public LoginImage: string = ""; 
    public MainLogo: string = "";
    public SmallLogo: string = ""; 
    public showSpinner = true;
    public IsDSV = false;
    public SecondaryColor: string = null;

    constructor(
        private ss: LoginService,
        private privateLabelsBrandingDataService: PrivateLabelsBrandingDataService) {
        super(ss);
    }
    ngOnInit() {
        this.get_cookie_data();
        this.privateUrl = SessionInfo.GetLogitudeURL();
        // Get Images from storage, then request from server to change
        this.GetLoginPageData();
        this.GetPrivateLabelsData(this.privateUrl);
        this.IsDSV = window.sessionStorage.getItem("IsDSV") == "true";
    }
     
    private GetLoginPageData() {
    this.BackgroundImage = BrandingDataService.GetImage("BackgroundImage"); 
    this.MainLogo = BrandingDataService.GetImage("MainLogo"); 
    this.LoginImage = BrandingDataService.GetImage("LoginImage");  
    this.SecondaryColor = BrandingDataService.GetColor("SecondaryColor");  
         
    if (this.checkImagesValues()) {
        this.showSpinner = false;
     } 
    }

    private checkImagesValues() {
        return this.BackgroundImage != null && this.MainLogo != null && this.LoginImage != null;
    }

    GetPrivateLabelsData(privateUrl: string) {
        this.privateLabelsBrandingDataService.GetUserDashboardBrandingData(BrandingDataService.GetPrivateLabelsDataRequest(privateUrl)).subscribe((response: ServiceResponse) => {
            if (response.Result) {    
                BrandingDataService.SetPrivateLabelsDataRequest(response.Result, privateUrl); 
                this.GetLoginPageData();  
            } })
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
