import { CargoTrackingSearchService } from './../CargoTracking/Services/Others/CargoTrackingSearchService';
import { SearchComponent } from './../CargoTracking/Components/Search/search.component';
import { ShipmentComponent } from './../CargoTracking/Components/Shipment/shipment.component';

import { BrowserModule } from '@angular/platform-browser';
import { NgModule  } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CargoTrackingBrandingDataExtendedService } from '../CargoTracking/Services/Others/CargoTrackingBrandingDataExtendedService';// '../../../../../Services/Others/CargoTrackingDataExtendedService';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { BusyIndicator } from 'src/CargoTracking/Materials/BusyIndicator/BusyIndicator';
import { DashboardComponent } from 'src/CargoTracking/Components/Dashboard/dashboard.component';
import { PublicGateComponent } from 'src/CargoTracking/Components/PublicGate/PublicGate.component';
import { DashboardShipmentsComponent } from 'src/CargoTracking/Components/Dashboard/shipments/dashboard-shipments.component';
import { FavoritesComponent } from 'src/CargoTracking/Components/Dashboard/favorites/favorites.component';
import { ShipmentDetailsComponent } from 'src/CargoTracking/Components/Dashboard/shipments/ShipmentDetailsComponent';
import { PanelComponent } from "src/Infrastructure/Components/PanelComponent/PanelComponent";
import { CheckBoxComponent } from 'src/Infrastructure/Components/CheckBox/CheckBoxComponent';
import { DetailsMenuComponent } from 'src/Infrastructure/Components/DetailsMenu/DetailsMenuComponent';
import { LoginComponent } from 'src/Infrastructure/Components/LoginComponent/Login.Component';
import { LoginExtendedService } from 'src/Infrastructure/Services/Extended/LoginExtendedService';
import { CommonDataExtendedService } from 'src/Infrastructure/Services/Extended/CommonDataExtendedService';

export function getBaseUrl() {
    return document.getElementsByTagName('base')[0].href;
}
  
@NgModule({
    declarations: [
        AppComponent,
        ShipmentComponent,
        SearchComponent,
        BusyIndicator,
        PublicGateComponent,
        
        // Dashboard
        DashboardComponent,
        DashboardShipmentsComponent,
        FavoritesComponent, 
        ShipmentDetailsComponent,

        // Infra
        PanelComponent,
        CheckBoxComponent,
        DetailsMenuComponent,
        LoginComponent,
        
    ],
    imports: [
        BrowserModule,
        HttpClientModule,
        AppRoutingModule,
        ReactiveFormsModule,
        FormsModule, HttpClientModule
    ],
    providers: [
        CargoTrackingSearchService,
        CargoTrackingBrandingDataExtendedService,
        LoginExtendedService,
        CommonDataExtendedService,
        { provide: 'BASE_URL', useFactory: getBaseUrl, deps: [] }
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
