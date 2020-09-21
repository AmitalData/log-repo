import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute, Event, NavigationStart, NavigationEnd, NavigationError, RoutesRecognized } from '@angular/router';
import { CargoTrackingBrandingDataExtendedService } from '../CargoTracking/Services/Others/CargoTrackingBrandingDataExtendedService';// '../../../../../Services/Others/CargoTrackingDataExtendedService';
import { ServiceResponse } from '../CargoTracking/DataContracts/ServiceResponse';
import { CargoTrackingBrandingData } from '../CargoTracking/DataContracts/CargoTrackingBrandingData';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { RootContext } from 'src/CargoTracking/Utilities/RootContext';
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
    _Tenant:number ;
    constructor(private cargoTrackingDataExtendedService: CargoTrackingBrandingDataExtendedService,private _location: Location, private activerouter: ActivatedRoute, private router: Router)
    {

        RootContext.AppComponent = this;
        
        this.GetDataFromURL();
    }

    
 private getcargoTrackingData (){
    this.cargoTrackingDataExtendedService.get(this._Tenant).subscribe((response: ServiceResponse) => {
           
        
        CargoTrackingBrandingData.MainColor = response.Result.MainColor;
        this.IsBrandingDataLoaded = true;
        CargoTrackingBrandingData.MainColor=  this.ConvertHexaToRGBA(CargoTrackingBrandingData.MainColor);

        document.documentElement.style.setProperty('--MainColor', CargoTrackingBrandingData.MainColor);
        document.documentElement.style.setProperty('--CircleImageColor', CargoTrackingBrandingData.MainColor);
        document.documentElement.style.setProperty('--TitleColor', CargoTrackingBrandingData.MainColor);

        this.listenToRouterEvents();
    });
 }
    private GetDataFromURL(){
 
        this.router.events.subscribe((event: any) => {
          if(this._Tenant==null || Number.isNaN(this._Tenant)){
            var url:string = event.url;
            var URLParts=url.split('/');

            for(let i=0 ; i < URLParts.length ; i++){
                if (URLParts && URLParts.length > 0 && URLParts[i]) {
                this._Tenant = Number(URLParts[i]);
                if(!Number.isNaN(this._Tenant)){
                   break;
                }
              }
            }

                if (Number.isNaN(this._Tenant) || !this._Tenant || this._Tenant==null){
                    this._Tenant=1;
                    this.back();  
                }
                // else{
                //     this._Tenant=1; 
                // }
                this.getcargoTrackingData();
            }
                
          });
 
    }
    private ConvertHexaToRGBA(color: string) {
        var alpha = parseInt(color.slice(1,3), 16)/255;
        return 'rgba(' + parseInt(color.slice(-6, -4), 16) + ',' + parseInt(color.slice(-4, -2), 16) + ',' + parseInt(color.slice(-2), 16) + ',' + alpha + ')';

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
        this.router.navigate([this._Tenant,'search']);
    }

    public BusyIndicatorText: string = null;

    private showBusyIndicator: boolean = false;
    get ShowBusyIndicator() { return this.showBusyIndicator; }
    set ShowBusyIndicator(newValue: boolean) {
      if (this.showBusyIndicator != newValue) {
        this.showBusyIndicator = newValue;
      }
    }


}
