import { Component, OnInit, ViewChild, ViewContainerRef } from '@angular/core'; 
import { Headers } from '@angular/http'; 
import { DomSanitizer } from '@angular/platform-browser';
import { Router } from '@angular/router';import { LoginComponent } from '../../Components/LoginComponent';
import { LoginService } from '../../LoginService';
import { SessionInfo } from '../../SessionInfo';
import { Tools } from '../../Utilities/Tools';
import { ServiceResponse } from '../DataContracts/ServiceResponse';
import { BrandingDataService } from '../Services/BrandingDataService';
import { HybridLabelsBrandingDataService } from '../Services/HybridLabelsBrandingDataService';
 ;

@Component({
    selector: 'HybridLoginComponent',
    moduleId: './Login/Components/',
    templateUrl: 'HybridLoginComponent.html',
    styleUrls: ['HybridLoginComponent.css']
})
export class HybridLoginComponent extends LoginComponent implements OnInit {

    // need spiner
    public authHeader;
    private privateUrl;
    public MainColor: string = null;
    public BackgroundImage: string = "";
    public MainImage: string = "";
    public Id = "";
    public MainLogo: string = "";

    public show = true;
    constructor( 
        private router: Router,
        private ss: LoginService,
        private hybridLabelsBrandingDataService: HybridLabelsBrandingDataService) {
        super(ss);
        this.privateUrl = "http://localhost:9996/" ;
    }
    ngOnInit() {
        console.log("ngOnInit");
        this.get_cookie_data();

        this.privateUrl = SessionInfo.GetLogitudeURL();
        this.GetHybridLabelsData(this.privateUrl);
        console.log("on init " + this.BackgroundImage); 
    }
     

    GetHybridLabelsData(privateUrl: string) {
        this.hybridLabelsBrandingDataService.GetUserDashboardBrandingData(BrandingDataService.GetHybridLabelsDataRequest(privateUrl)).subscribe((response: ServiceResponse) => {
            if (response.Result) {
                this.Tenant = response.Result.Tenant;
                BrandingDataService.SetHybridLabelsDataRequest(response.Result, privateUrl);
                this.MainColor = response.Result.MainColor;

                console.log("MainColor " + this.MainColor);
                this.BackgroundImage = BrandingDataService.GetBackgroundImage();
                this.MainImage = BrandingDataService.GetMainImage();
                this.Id = response.Result.Id;
                this.MainLogo = response.Result.MainLogo;
                console.log("before main logo " + this.MainLogo);
                this.MainLogo = BrandingDataService.GetMainLogo();
                console.log("after main logo " + this.MainLogo);
            } 
        });

    }

    //  private GoToError401() {
    //    this.router.navigate(['Error401']);
    // }

    private ClearLocation() {
        if (SessionInfo.MainLocation) {
            SessionInfo.MainLocation.clear();
        }
    }
    ForgotPasswordClicked() {
        this.ClearLocation();
        Tools.DynamicLoader.Load("./Login/Components/DSVResetPasswordComponent", SessionInfo.MainLocation)
            .then(cmpRef => {
            });
    }
}
