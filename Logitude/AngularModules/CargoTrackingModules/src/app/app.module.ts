import { SearchComponent } from './../CargoTracking/Components/Search/search.component';
import { ShipmentComponent } from './../CargoTracking/Components/Shipment/shipment.component';
import { HttpClientModule } from '@angular/common/http';

import { BrowserModule } from '@angular/platform-browser';
import { NgModule  } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CargoTrackingBrandingDataExtendedService } from '../CargoTracking/Services/Others/CargoTrackingBrandingDataExtendedService';// '../../../../../Services/Others/CargoTrackingDataExtendedService';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

@NgModule({
    declarations: [
        AppComponent,
        ShipmentComponent,
        SearchComponent,
        
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        ReactiveFormsModule,
        FormsModule, HttpClientModule
    ],
    providers: [CargoTrackingBrandingDataExtendedService],
    bootstrap: [AppComponent]
})
export class AppModule { }
