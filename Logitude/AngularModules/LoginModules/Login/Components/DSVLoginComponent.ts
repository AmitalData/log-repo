import { Component, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { LoginService, LoginParameters } from '../LoginService';
import { Headers } from '@angular/http';
import { SessionInfo } from '../SessionInfo';
import { LoginComponent } from './LoginComponent';
import { DynamicLoaderTSC } from '../Utilities/DynamicLoaderTSC';
import { Tools } from '../Utilities/Tools';
import { Router } from '@angular/router';
import { PrivateLabelsBrandingDataService } from '../PrivateLabels/Services/PrivateLabelsBrandingDataService';
import { ServiceResponse } from '../PrivateLabels/DataContracts/ServiceResponse';
import { BrandingDataService } from '../PrivateLabels/Services/BrandingDataService';
import { error } from 'core-js/fn/log';

@Component({
    selector: 'DSVLoginComponent',
    moduleId: './Login/Components/',
    templateUrl: 'DSVLoginComponent.html',
    styleUrls: ['DSVLoginComponent.css']
})
export class DSVLoginComponent extends LoginComponent implements OnInit {

    public authHeader;
    private privateUrl;
    public MainColor: string = null;
    public BackgroundImage: string = "";
    public MainImage: string = "";
    public MainLogo: string = "";
    public LoginProcessImage: string = ""; 
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
        this.privateLabelsBrandingDataService.GetUserDashboardBrandingData(BrandingDataService.GetPrivateLabelsDataRequest(privateUrl)).subscribe(
            (response: ServiceResponse) => {
                if (response.Result) {
                    this.Tenant = response.Result.Tenant;
                    BrandingDataService.SetPrivateLabelsDataRequest(response.Result, privateUrl);
                    this.MainColor = response.Result.MainColor;
                    this.BackgroundImage = BrandingDataService.GetBackgroundImage();
                    this.MainImage = BrandingDataService.GetMainImage();
                    this.MainLogo = BrandingDataService.GetMainLogo();
                    this.LoginProcessImage = BrandingDataService.GetLoginProgressImage();
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
        Tools.DynamicLoader.Load("./Login/Components/DSVResetPasswordComponent", SessionInfo.MainLocation)
            .then(cmpRef => {
            });
    }
}
