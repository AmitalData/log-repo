import { CargoTrackingSearchService } from '../CargoTracking/Services/Others/CargoTrackingSearchService';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule  } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CargoTrackingBrandingDataExtendedService } from '../CargoTracking/Services/Others/CargoTrackingBrandingDataExtendedService';// '../../../../../Services/Others/CargoTrackingDataExtendedService';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { BusyIndicator } from 'src/CargoTracking/Materials/BusyIndicator/BusyIndicator';
import { PanelComponent } from "src/Infrastructure/Components/PanelComponent/PanelComponent";
import { CheckBoxComponent } from 'src/Infrastructure/Components/CheckBox/CheckBoxComponent';
import { DetailsMenuComponent } from 'src/Infrastructure/Components/DetailsMenu/DetailsMenuComponent';
import { LoginComponent } from 'src/Infrastructure/Components/LoginComponent/Login.Component';
import { ResetPasswordComponent } from 'src/Infrastructure/Components/LoginComponent/ResetPassword.Component';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { ScrollingModule } from '@angular/cdk/scrolling';
import { FavoritesPageComponent } from '../CargoTracking/Components/UserDashboard/FavoritesPage/FavoritesPageComponent';
import { UserDashboardComponent } from '../CargoTracking/Components/UserDashboard/UserDashboardComponent';
import { ShipmentsListComponent } from '../CargoTracking/Components/UserDashboard/ShipmentsPage/ShipmentsList/ShipmentsListComponent';
import { PublicShipmentDetailsComponent } from '../CargoTracking/Components/PublicSite/PublicShipmentDetailsComponent/PublicShipmentDetailsComponent';
import { SearchComponent } from '../CargoTracking/Components/PublicSite/SearchComponent/SearchComponent';
import { HomeComponent } from '../CargoTracking/Components/PublicSite/HomeComponent/HomeComponent';
import { LoginExtendedService } from 'src/Infrastructure/Services/Extended/LoginExtendedService';
import { CommonDataExtendedService } from 'src/Infrastructure/Services/Extended/CommonDataExtendedService';
import { ShipmentDetailsComponent } from 'src/CargoTracking/Components/UserDashboard/ShipmentsPage/ShipmentDetails/ShipmentDetailsComponent';
import { CargoTrackingMilestoneService } from 'src/CargoTracking/Services/Others/CargoTrackingMilestoneService';
import { ChangePasswordComponent } from 'src/Infrastructure/Components/LoginComponent/ChangePassword.Component';
import { AuthService } from './auth.service';
import { AuthGuardService } from 'src/Infrastructure/Services/auth-guard.service';
import { Error401Component } from 'src/CargoTracking/Components/Errors/Error401Component';
import { LoginServiceHelper } from 'src/Infrastructure/Utilities/LoginServiceHelper';



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
        LoginComponent,
        ResetPasswordComponent,
        ChangePasswordComponent,


        //Erros
        Error401Component

        
    ],
    imports: [
        BrowserModule,
        HttpClientModule,
        AppRoutingModule,
        ReactiveFormsModule,
        ScrollingModule,
        FormsModule, HttpClientModule, NoopAnimationsModule,
    ],
    providers: [
        CargoTrackingSearchService,
        CargoTrackingBrandingDataExtendedService,
        LoginExtendedService,
        CommonDataExtendedService,
        CargoTrackingMilestoneService,
        AuthGuardService,
        AuthService,
        LoginServiceHelper,
        { provide: 'BASE_URL', useFactory: getBaseUrl, deps: [] }
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
