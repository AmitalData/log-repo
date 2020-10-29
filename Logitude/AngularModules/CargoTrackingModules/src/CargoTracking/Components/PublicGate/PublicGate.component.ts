import { CargoTrackingShipmentList } from '../../EntityLists/CargoTrackingShipmentList';
import { CargoTrackingSearchService } from '../../Services/Others/CargoTrackingSearchService';
import { Component, ViewChild, ElementRef, AfterViewInit } from '@angular/core';
import { Router, ActivatedRoute, Event, RoutesRecognized, NavigationStart, NavigationEnd, NavigationError } from '@angular/router';
import { fromEvent } from 'rxjs';
import { filter, debounceTime, distinctUntilChanged, tap, map } from 'rxjs/operators';
import { FormBuilder } from '@angular/forms';
import { db } from '../../../app/mem.data';
import { CargoTrackingBrandingData } from '../../DataContracts/CargoTrackingBrandingData';
import { CargoTrackingBrandingDataExtendedService } from 'src/CargoTracking/Services/Others/CargoTrackingBrandingDataExtendedService';
import { ServiceResponse } from 'src/CargoTracking/DataContracts/ServiceResponse';
import { Location } from '@angular/common';
import { ShipmentComponent } from '../Shipment/shipment.component';
import { AppHelper } from 'src/CargoTracking/Utilities/AppHelper';


@Component({
    selector: 'PublicGate',
    templateUrl: './PublicGate.component.html',
    styleUrls: ['./PublicGate.component.css']
})
export class PublicGateComponent
{

    IsBrandingDataLoaded: boolean = false;
    displayMenu: boolean = false;
    showBackButton: boolean = false;
    currentDate: Date = new Date();
    companyLabel: string = "DSV";
    companyName: string = "Unifreight Cloud Services";
    tenant: number;
    BackGroundImg:string;
    constructor(private cargoTrackingDataExtendedService: CargoTrackingBrandingDataExtendedService, private activerouter: ActivatedRoute, private router: Router, 
        private location: Location)
    {

        this.GetDataFromURL();
        
    }


    private getcargoTrackingData()
    {
        this.cargoTrackingDataExtendedService.get(this.tenant).subscribe((response: ServiceResponse) =>
        { 
 
            CargoTrackingBrandingData.MainColor = response.Result.MainColor;
            if(CargoTrackingBrandingData.MainColor) {

                CargoTrackingBrandingData.MainColor = this.ConvertHexaToRGBA(CargoTrackingBrandingData.MainColor);
    
                document.documentElement.style.setProperty('--BGColor', CargoTrackingBrandingData.MainColor);
                document.documentElement.style.setProperty('--MainColor', CargoTrackingBrandingData.MainColor);
                document.documentElement.style.setProperty('--CircleImageColor', CargoTrackingBrandingData.MainColor);
                document.documentElement.style.setProperty('--TitleColor', CargoTrackingBrandingData.MainColor);
                document.documentElement.style.setProperty('--busyIndicatorColor', CargoTrackingBrandingData.MainColor);

            }
            this.IsBrandingDataLoaded = true;
            this.BackGroundImg=response.Result.BackgroundImg!=null? "url("+ response.Result.BackgroundImg+")":"url('../assets/images/misc/map-bg.svg')";
            this.listenToRouterEvents();
        });
    }
    private GetDataFromURL()
    {

        this.router.events.subscribe((event: any) =>
        {
            if (this.tenant == null || Number.isNaN(this.tenant)) {
                // var params:any[] = event.snapshot.params;
                // var tenant = params['Tenant'];
                // this._Tenant = tenant;
                var url: string =this.router.url;
                var URLParts = url.split('/');

                for (let i = 0; i < URLParts.length; i++) {
                    if (URLParts && URLParts.length > 0 && URLParts[i]) {
                        this.tenant = Number(URLParts[i]);
                        if (!Number.isNaN(this.tenant)) {
                            break;
                        }
                    }
                }

                if (Number.isNaN(this.tenant) || !this.tenant || this.tenant==null){
                    this.tenant=1;
                    this.back();  
                }
                // else{
                //     this._Tenant=1; 
                // }
                this.getcargoTrackingData();
            }

        });

    }
    private ConvertHexaToRGBA(color: string)
    {
        var alpha = parseInt(color.slice(1, 3), 16) / 255;
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
        AppHelper.AppBack(this.router,this.location,this.tenant);

    }
 
    GetBackEnabled()
    {
        return AppHelper.GetBackEnabled(this.router);
    }
}
