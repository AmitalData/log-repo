import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CargoTrackingBrandingData } from 'src/CargoTracking/DataContracts/CargoTrackingBrandingData';
import { ServiceResponse } from 'src/CargoTracking/DataContracts/ServiceResponse';
import { CargoTrackingBrandingDataExtendedService } from 'src/CargoTracking/Services/Others/CargoTrackingBrandingDataExtendedService';
import { Location } from '@angular/common';


@Component({
    selector: 'Error401Component',
    templateUrl: './Error401Component.html',
    styleUrls: ['./Error401Component.css']
})
export class Error401Component 
{

    public SiteUrl:string = location.hostname;
    isTenantLoaded:boolean = false;
    constructor(private cargoTrackingDataExtendedService: CargoTrackingBrandingDataExtendedService,private router: Router,   private Minlocation: Location)
    {
             this.GetTenantByDomain();
    }

    private GetTenantByDomain()
    {   console.log(location.hostname);
        this.cargoTrackingDataExtendedService.GetTenantByDomain(location.hostname).subscribe((response: ServiceResponse) =>
        {   this.isTenantLoaded =true;
            if(response.Result!=null){
            CargoTrackingBrandingData.Tenant = response.Result;
            this.GoBack();
          }
        });
    }

    public GoBack(){
        this.Minlocation.back();
   }
}
