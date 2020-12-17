import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { CargoTrackingBrandingData } from 'src/CargoTracking/DataContracts/CargoTrackingBrandingData';

@Injectable()
export class LoginServiceHelper {

    constructor(private router:Router) {

    }

    public GetLoginLogoImg() {
        if(CargoTrackingBrandingData.Tenant && CargoTrackingBrandingData.ComapnylogoURL != null)
            return CargoTrackingBrandingData.ComapnylogoURL;
        else
            return "./assets/images/logo/UnifreightLogo.jpg";
    }

}