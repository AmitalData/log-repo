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
    _Tenant: number;
    constructor(private cargoTrackingDataExtendedService: CargoTrackingBrandingDataExtendedService, private _location: Location, private activerouter: ActivatedRoute, private router: Router)
    {

        RootContext.AppComponent = this;


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
