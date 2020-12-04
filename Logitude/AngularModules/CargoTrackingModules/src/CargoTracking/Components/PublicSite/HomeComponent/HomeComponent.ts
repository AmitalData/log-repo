import { Component, Inject } from '@angular/core';
import { Router, Event, RoutesRecognized, NavigationStart, NavigationEnd, NavigationError } from '@angular/router';
import { CargoTrackingBrandingDataExtendedService } from '../../../Services/Others/CargoTrackingBrandingDataExtendedService';
import { ServiceResponse } from '../../../DataContracts/ServiceResponse';
import { Location } from '@angular/common';
import { AppHelper } from '../../../Utilities/AppHelper';
import { CargoTrackingBrandingData } from '../../../DataContracts/CargoTrackingBrandingData';
import { ServiceHelper } from 'src/CargoTracking/Utilities/ServiceHelper';


@Component({
    selector: 'HomeComponent',
    templateUrl: './HomeComponent.html',
    styleUrls: ['./HomeComponent.css']
})
export class HomeComponent
{

    IsBrandingDataLoaded: boolean = false;
    displayMenu: boolean = false;
    showBackButton: boolean = false;
    currentDate: Date = new Date();
    companyLabel: string = "DSV";
    companyName: string = "Unifreight Cloud Services";
    BackGroundImg:string;
    Domain:string;
    MapImgSRC:string ="";
    public baseUrl:string;
    constructor(private cargoTrackingDataExtendedService: CargoTrackingBrandingDataExtendedService, @Inject('BASE_URL') baseUrl: string, private router: Router, 
        private location: Location)
    {

        this.GetDataFromURL(baseUrl);
        
    }


    private getcargoTrackingData()
    {   
        this.cargoTrackingDataExtendedService.get(ServiceHelper.GetCurrentDomain(this.baseUrl)).subscribe((response: ServiceResponse) =>
        { if(response.Result){
            CargoTrackingBrandingData.Tenant = response.Result.Tenant;
            CargoTrackingBrandingData.MainColor = response.Result.MainColor != null ? this.ConvertHexaToRGBA(response.Result.MainColor) :"#000000";
            CargoTrackingBrandingData.SecondaryColor = response.Result.SecondaryColor ? this.ConvertHexaToRGBA(response.Result.SecondaryColor) : "#002664";
            document.documentElement.style.setProperty('--BGColor', CargoTrackingBrandingData.MainColor);
            document.documentElement.style.setProperty('--MainColor', CargoTrackingBrandingData.MainColor);
            document.documentElement.style.setProperty('--busyIndicatorColor', CargoTrackingBrandingData.MainColor);
            document.documentElement.style.setProperty('--secondaryColor', CargoTrackingBrandingData.SecondaryColor);
            this.Logo = response.Result.Logo != null ? response.Result.Logo:null;
            this.BackGroundImg=response.Result.BackgroundURL!=null? "url("+ ServiceHelper.GetAppURL(this.baseUrl)+response.Result.BackgroundURL+")":this.MapImgSRC;
            this.IsBrandingDataLoaded = true;
            this.listenToRouterEvents();
        }
        else{
            this.GoToError401();
        }
         
        });
    }
    public Logo: string; 
    private GetDataFromURL(baseUrl: string)
    {
        this.baseUrl =baseUrl;
        this.MapImgSRC  = "url('"+this.baseUrl+"assets/images/misc/map-bg.svg')"
        this.getcargoTrackingData();
          
    }

    public GoToPrivateSite(){
        this.router.navigate(['dashboard']);
    }
    public GoToError401(){
        this.router.navigate(['Error401']);
    }
    private ConvertHexaToRGBA(color: string)
    {
        if (color) {
            var alpha = parseInt(color.slice(1, 3), 16) / 255;
            return 'rgba(' + parseInt(color.slice(-6, -4), 16) + ',' + parseInt(color.slice(-4, -2), 16) + ',' + parseInt(color.slice(-2), 16) + ',' + alpha + ')';
        }
    }
    private listenToRouterEvents()
    {
        this.router.events.subscribe((event: Event) =>
        {
            if (event instanceof RoutesRecognized) {
                // Show loading indicator
                // var url = window.location.pathname;
                var url = event.urlAfterRedirects;
                if (url == "public-tracking/search/")
                    this.showBackButton = false;
                else // if (url.includes('/shipment/'))
                    this.showBackButton = true;

            }

            if (event instanceof NavigationStart) {
                var url = window.location.pathname;
            }

            if (event instanceof NavigationEnd) {
                // Hide loading indicator
            }

            if (event instanceof NavigationError) {
                // Hide loading indicator
                // Present error to user
                console.log(event.error);
            }
        });
    }

    back()
    {
        AppHelper.AppBack(this.router,this.location,CargoTrackingBrandingData.Tenant);

    }
 
    GetBackEnabled()
    {
        return AppHelper.GetBackEnabled(this.router);
    }
}
