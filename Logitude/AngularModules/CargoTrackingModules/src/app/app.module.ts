import { CargoTrackingSearchService } from '../CargoTracking/Services/Others/CargoTrackingSearchService';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
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
import { CommonModule, DatePipe } from '@angular/common';
import { CargoTrackingPortService } from '../CargoTracking/Services/Others/CargoTrackingPortService';
import { CargoTrackingShipmentService } from '../CargoTracking/Services/Others/CargoTrackingShipmentService';
import { DocumentDownloadService } from '../CargoTracking/Services/Others/DocumentDownloadService';
import { MatDialogModule, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MessageWindowComponent } from '../Infrastructure/Components/MessageWindow/MessageWindowComponent';
import { ToolTipComponent } from 'src/Infrastructure/Components/ToolTip/ToolTipComponent';
import { OverlayModule } from '@angular/cdk/overlay';
import { MultipleSelectionComponent } from '../Infrastructure/Components/MultipleSelection/MultipleSelectionComponent';
import { MatNativeDateModule } from '@angular/material/core';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';
import { CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { CargoTrackingShipmentOrderService } from '../CargoTracking/Services/Others/CargoTrackingShipmentOrderService';
import { IconButtonComponent } from '../Infrastructure/Components/IconButton/IconButtonComponent';
import { DateTimeFormatPipe } from '../Infrastructure/Pipes/DateTimeFormatPipe';
import { CargoTrackingShipmentExtendedService } from 'src/CargoTracking/Services/Others/CargoTrackingShipmentExtendedService';
import { SharedService } from 'src/CargoTracking/Services/Others/SharedService';
import { CustomLabelComponent, LabelUtilContentDirective, LabelUtilElementDirective} from 'src/Infrastructure/Components/CustomLabel/CustomLabel.component';
import {MatTooltipModule} from '@angular/material/tooltip';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { LogboxShipmentExportExcelService } from 'src/CargoTracking/Services/Others/LogboxShipmentExportExcelService';
import { LogitudeGridExportToExcelService } from 'src/CargoTracking/Services/Others/LogitudeGridExportToExcelComponent';
import { SafePipe } from 'src/Infrastructure/Pipes/SafePipe';
import { StopLoopPipe } from 'src/Infrastructure/Pipes/StopLoopPipe';
import { TenantManagementService } from 'src/CargoTracking/Services/Others/TenantManagementService';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CookieconsentComponent } from 'src/CargoTracking/Components/cookieconsent/cookieconsent.component';

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
        MessageWindowComponent,
        ToolTipComponent,
        MultipleSelectionComponent,
        IconButtonComponent,
        DateTimeFormatPipe,
        SafePipe,
        StopLoopPipe,
        CustomLabelComponent,
        LabelUtilContentDirective,
        LabelUtilElementDirective,
        CookieconsentComponent,
        
        //Erros
        Error401Component


    ],
    imports: [
        BrowserModule,
        CommonModule,
        HttpClientModule,
        AppRoutingModule,
        MatDialogModule,
        MatNativeDateModule,
        ReactiveFormsModule,
        ScrollingModule,
        OverlayModule,
        MatInputModule,
        MatSelectModule,
        MatFormFieldModule,
        MatIconModule,
        FormsModule, HttpClientModule, NoopAnimationsModule,
        MatTooltipModule,
        MatSlideToggleModule,
        BrowserAnimationsModule
    ],
    providers: [
        CargoTrackingSearchService,
        CargoTrackingBrandingDataExtendedService,
        LoginExtendedService,
        CargoTrackingPortService,
        CargoTrackingShipmentService,
        DocumentDownloadService,
        CommonDataExtendedService,
        CargoTrackingMilestoneService,
        AuthGuardService,
        AuthService,
        LoginServiceHelper,
        CargoTrackingShipmentOrderService,
        CargoTrackingShipmentExtendedService,
        DatePipe,
        DateTimeFormatPipe,
        SafePipe,
        StopLoopPipe,
        SharedService,
        LogboxShipmentExportExcelService,
        LogitudeGridExportToExcelService,
        TenantManagementService,
        { provide: 'BASE_URL', useFactory: getBaseUrl, deps: [] },
        { provide: MAT_DIALOG_DATA, useValue: {} },
        { provide: MatDialogRef, useValue: {} }
    ],
    schemas: [CUSTOM_ELEMENTS_SCHEMA],
    bootstrap: [AppComponent]
})
export class AppModule { }
