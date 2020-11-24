import { CargoTrackingSearchService } from '../CargoTracking/Services/Others/CargoTrackingSearchService';

import { BrowserModule } from '@angular/platform-browser';
import { NgModule  } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CargoTrackingBrandingDataExtendedService } from '../CargoTracking/Services/Others/CargoTrackingBrandingDataExtendedService';// '../../../../../Services/Others/CargoTrackingDataExtendedService';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { BusyIndicator } from '../CargoTracking/Materials/BusyIndicator/BusyIndicator';
import { FavoritesPageComponent } from '../CargoTracking/Components/UserDashboard/FavoritesPage/FavoritesPageComponent';
import { PanelComponent } from "../Infrastructure/Components/PanelComponent/PanelComponent";
import { CheckBoxComponent } from '../Infrastructure/Components/CheckBox/CheckBoxComponent';
import { DetailsMenuComponent } from '../Infrastructure/Components/DetailsMenu/DetailsMenuComponent';
import { UserDashboardComponent } from '../CargoTracking/Components/UserDashboard/UserDashboardComponent';
import { ShipmentDetailsComponent } from '../CargoTracking/Components/UserDashboard/ShipmentsPage/ShipmentDetails/ShipmentDetailsComponent';
import { ShipmentsListComponent } from '../CargoTracking/Components/UserDashboard/ShipmentsPage/ShipmentsList/ShipmentsListComponent';
import { PublicShipmentDetailsComponent } from 'src/CargoTracking/Components/PublicSite/PublicShipmentDetailsComponent/PublicShipmentDetailsComponent';
import { SearchComponent } from 'src/CargoTracking/Components/PublicSite/SearchComponent/SearchComponent';
import { HomeComponent } from 'src/CargoTracking/Components/PublicSite/HomeComponent/HomeComponent';
  
export function getBaseUrl() {
    return document.getElementsByTagName('base')[0].href;
}

@NgModule({
    declarations: [
        AppComponent,
        PublicShipmentDetailsComponent,
        SearchComponent,
        BusyIndicator,
        HomeComponent,
        
        // Dashboard
        UserDashboardComponent,
        ShipmentsListComponent,
        FavoritesPageComponent, 
        ShipmentDetailsComponent,

        // Infra
        PanelComponent,
        CheckBoxComponent,
        DetailsMenuComponent,
        
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
        { provide: 'BASE_URL', useFactory: getBaseUrl, deps: [] }
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
