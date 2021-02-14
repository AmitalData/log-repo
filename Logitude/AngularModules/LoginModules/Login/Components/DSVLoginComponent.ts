import { Component, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { LoginService, LoginParameters } from '../LoginService';
import { ActivatedRoute, Router } from '@angular/router';
import {Headers} from '@angular/http';
import {SessionInfo} from '../SessionInfo';
import {LoginComponent} from './LoginComponent';
import {DynamicLoaderTSC} from '../Utilities/DynamicLoaderTSC';
import {Tools} from '../Utilities/Tools';
import { BrandingDataService } from '../HybridLabels/Services/BrandingDataService';
import { HybridLabelsBrandingDataService } from '../HybridLabels/Services/HybridLabelsBrandingDataService';
import { ServiceResponse } from '../HybridLabels/DataContracts/ServiceResponse';

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

    constructor(
        private router: Router,
        private ss: LoginService,
        private hybridLabelsBrandingDataService: HybridLabelsBrandingDataService) {
        super(ss);
        this.privateUrl = SessionInfo.GetLogitudeURL();
        this.GetHybridLabelsData(this.privateUrl);
    }
    ngOnInit() {
        console.log("ngOnInit");
        this.get_cookie_data();
    }

    GetHybridLabelsData(privateUrl: string) {
        this.hybridLabelsBrandingDataService.GetBrandingData(BrandingDataService.GetHybridLabelsDataRequest(privateUrl)).subscribe((response: ServiceResponse) => {
            if (response.Result) {
                this.Tenant = response.Result.Tenant;
                BrandingDataService.SetHybridLabelsDataRequest(response.Result, privateUrl); 
                this.MainColor = response.Result.MainColor != null ? BrandingDataService.ConvertHexaToRGBA(response.Result.MainColor) : null; 
                this.BackgroundImage = BrandingDataService.GetBackgroundImage();
                this.MainImage = BrandingDataService.GetMainImage();            }
            else {
                this.GoToError401();
            }
        });

    }

    private GoToError401() {
        this.router.navigate(['Error401']);
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
