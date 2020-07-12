import { Location } from '@angular/common';
import { Component } from '@angular/core';
import { Router, ActivatedRoute, Event, NavigationStart, NavigationEnd, NavigationError, RoutesRecognized } from '@angular/router';
import { CargoTrackingBrandingDataExtendedService } from '../CargoTracking/Services/Others/CargoTrackingBrandingDataExtendedService';// '../../../../../Services/Others/CargoTrackingDataExtendedService';
import { ServiceResponse } from '../CargoTracking/DataContracts/ServiceResponse';
import { CargoTrackingBrandingData } from '../CargoTracking/DataContracts/CargoTrackingBrandingData';
@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css'],
 
})
export class AppComponent
{
    IsBrandingDataLoaded: boolean = false;
    displayMenu: boolean = false;
    showBackButton: boolean = false;
    currentDate: Date = new Date();
    companyLabel: string = "DSV";
    companyName: string = "Unifreight Cloud Services";
    constructor(private cargoTrackingDataExtendedService: CargoTrackingBrandingDataExtendedService,private _location: Location, private activerouter: ActivatedRoute, private router: Router)
    {
        this.cargoTrackingDataExtendedService.get().subscribe((response: ServiceResponse) => {
           
        
            CargoTrackingBrandingData.MainColor = response.Result.MainColor;
            this.IsBrandingDataLoaded = true;
            document.documentElement.style.setProperty('--MainColor', CargoTrackingBrandingData.MainColor);
            document.documentElement.style.setProperty('--CircleImageColor', CargoTrackingBrandingData.MainColor);
            document.documentElement.style.setProperty('--TitleColor', CargoTrackingBrandingData.MainColor);

            this.listenToRouterEvents();
        });

       
    }

    private listenToRouterEvents()
    {
        this.router.events.subscribe((event: Event) =>
        {
            if (event instanceof RoutesRecognized) {
                // Show loading indicator
                // var url = window.location.pathname;
                var url = event.urlAfterRedirects;
                if (url == "/search/")
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
        // this._location.back();
        this.router.navigate(['search']);
    }

}
