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
    Domain:string;
    public baseUrl:string;
  

    constructor(private cargoTrackingDataExtendedService: CargoTrackingBrandingDataExtendedService, @Inject('BASE_URL') baseUrl: string, private router: Router, 
        private location: Location)
    {
        this.baseUrl =baseUrl;
        this.getcargoTrackingData();
        
    }


    private getcargoTrackingData()
    {   
        this.cargoTrackingDataExtendedService.get(ServiceHelper.GetCurrentDomain(this.baseUrl)).subscribe((response: ServiceResponse) =>
        { if(response.Result){
            ServiceHelper.SetCargoTrackingDate(response.Result,this.baseUrl);
            this.IsBrandingDataLoaded = true;
            this.listenToRouterEvents();
        }
        else{
            this.GoToError401();
        }
         
        });
    }

    get ComapnyLogo(){
        return CargoTrackingBrandingData.ComapnylogoURL;
    } 
    get BrowserIcon(){
        return CargoTrackingBrandingData.BrowserIconURL;
    } 
    get BackGroundImg(){
        return CargoTrackingBrandingData.BackgroundURL;
    } 
  
    public GoToPrivateSite(){
        this.router.navigate(['dashboard']);
    }
    public GoToError401(){
        this.router.navigate(['Error401']);
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
