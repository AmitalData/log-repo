import { CargoTrackingShipmentList } from '../../EntityLists/CargoTrackingShipmentList';
import { CargoTrackingSearchService } from '../../Services/Others/CargoTrackingSearchService';
import { Component, ViewChild, ElementRef, AfterViewInit, Inject } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder } from '@angular/forms';
import { CargoTrackingBrandingDataExtendedService } from 'src/CargoTracking/Services/Others/CargoTrackingBrandingDataExtendedService';
import { CargoTrackingBrandingData } from 'src/CargoTracking/DataContracts/CargoTrackingBrandingData';
import { ServiceResponse } from 'src/CargoTracking/DataContracts/ServiceResponse';
import { ServiceHelper } from 'src/CargoTracking/Utilities/ServiceHelper';


@Component({
    selector: 'UserDashboard',
    templateUrl: './UserDashboardComponent.html',
    styleUrls: ['./UserDashboardComponent.css']
})
export class UserDashboardComponent implements AfterViewInit
{

    @ViewChild('input') input: ElementRef;
    isLoading: boolean = false;
    noResult: boolean = false;
    currentDate = new Date();
    FilteredItems: any[] = [];
    Shipments: CargoTrackingShipmentList[] = [];
    isTenantLoaded:boolean = false;
    constructor(private cargoTrackingDataExtendedService: CargoTrackingBrandingDataExtendedService,private router: Router,@Inject('BASE_URL') baseUrl: string )
    {
         
         document.documentElement.style.setProperty('--BGColor', 'RGB(250,251,252)');
         this.GetTenantByDomain(baseUrl);

    }

    isNavOpened = false;
    openNav(){
        this.isNavOpened = !this.isNavOpened;
    }
    public GoToError401(){
        this.router.navigate(['Error401']);
    }

    private GetTenantByDomain(baseUrl:string)
    {
        this.cargoTrackingDataExtendedService.GetTenantByDomain(ServiceHelper.GetCurrentDomain(baseUrl)).subscribe((response: ServiceResponse) =>
        { if(response.Result){
            CargoTrackingBrandingData.Tenant = response.Result;
            this.isTenantLoaded =true;
          }
          else{
              this.GoToError401();
          }
        });
    }

    ngAfterViewInit()
    { 

    }

    
}
