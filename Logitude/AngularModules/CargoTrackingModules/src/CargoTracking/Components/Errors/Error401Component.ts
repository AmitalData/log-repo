import { Component, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { CargoTrackingBrandingData } from 'src/CargoTracking/DataContracts/CargoTrackingBrandingData';
import { ServiceResponse } from 'src/CargoTracking/DataContracts/ServiceResponse';
import { CargoTrackingBrandingDataExtendedService } from 'src/CargoTracking/Services/Others/CargoTrackingBrandingDataExtendedService';
import { Location } from '@angular/common';
import { ServiceHelper } from 'src/CargoTracking/Utilities/ServiceHelper';


@Component({
    selector: 'Error401Component',
    templateUrl: './Error401Component.html',
    styleUrls: ['./Error401Component.css']
})
export class Error401Component 
{

    public SiteUrl:string = location.hostname;
    isTenantLoaded:boolean = false;
    constructor(private cargoTrackingDataExtendedService: CargoTrackingBrandingDataExtendedService,@Inject('BASE_URL') baseUrl: string,   private Minlocation: Location)
    {

             this.GetDomainName(baseUrl);
             this.GetTenantByDomain(baseUrl);
              
    }
    private GetDomainName(baseUrl:string)
    {     this.SiteUrl=baseUrl;
          this.SiteUrl = this.SiteUrl.toLocaleLowerCase().replace("http://", '').replace("https://", '').replace("WWW.", '');  
          if(this.SiteUrl && this.SiteUrl.endsWith("/")){
            this.SiteUrl = this.SiteUrl.substring(0,this.SiteUrl.length-1);
          }
          
    }
    private GetTenantByDomain(baseUrl:string)
    {   
        this.cargoTrackingDataExtendedService.GetTenantByDomain(ServiceHelper.GetCurrentDomain(baseUrl)).subscribe((response: ServiceResponse) =>
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
