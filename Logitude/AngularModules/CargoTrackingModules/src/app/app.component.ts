
import { Component, OnInit } from '@angular/core';
import { RootContext } from 'src/CargoTracking/Utilities/RootContext';
import { SessionInfo } from '../Infrastructure/Utilities/SessionInfo';
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
    constructor()
    {
        RootContext.AppComponent = this;
        this.SetSeSessionInfo();
    }

    private SetSeSessionInfo() {
        SessionInfo.LoggedUserEmail = sessionStorage.getItem("LoggedUserEmail");
        SessionInfo.LoggedUserId = sessionStorage.getItem("LoggedUserId");
        SessionInfo.LoggedUserTenant = Number(sessionStorage.getItem("LoggedUserTenant"));
        SessionInfo.Token = sessionStorage.getItem("Token");
        SessionInfo.DocumentDownloadToken = sessionStorage.getItem("DocumentDownloadToken");
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
