import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from 'src/Infrastructure/Components/LoginComponent/Login.Component';
import { Error401Component } from 'src/CargoTracking/Components/Errors/Error401Component';
import { HomeComponent } from 'src/CargoTracking/Components/PublicSite/HomeComponent/HomeComponent';
import { PublicShipmentDetailsComponent } from 'src/CargoTracking/Components/PublicSite/PublicShipmentDetailsComponent/PublicShipmentDetailsComponent';
import { SearchComponent } from 'src/CargoTracking/Components/PublicSite/SearchComponent/SearchComponent';
import { FavoritesPageComponent } from 'src/CargoTracking/Components/UserDashboard/FavoritesPage/FavoritesPageComponent';
import { ShipmentDetailsComponent } from 'src/CargoTracking/Components/UserDashboard/ShipmentsPage/ShipmentDetails/ShipmentDetailsComponent';
import { ShipmentsListComponent } from 'src/CargoTracking/Components/UserDashboard/ShipmentsPage/ShipmentsList/ShipmentsListComponent';
import { UserDashboardComponent } from 'src/CargoTracking/Components/UserDashboard/UserDashboardComponent';
import { ResetPasswordComponent } from 'src/Infrastructure/Components/LoginComponent/ResetPassword.Component';
import { ChangePasswordComponent } from 'src/Infrastructure/Components/LoginComponent/ChangePassword.Component';
import { AuthGuardService as AuthGuard } from 'src/Infrastructure/Services/auth-guard.service';
const routes: Routes = [

    { path: 'Cargo-Tracking', redirectTo: "cargo-tracking/login", pathMatch: "full" },
    { path: 'Cargo-Tracking/login', redirectTo: "cargo-tracking/login", pathMatch: "full" },
    { path: 'cargo-tracking/login', component: LoginComponent },
    { path: 'cargo-tracking/resetpassword', component: ResetPasswordComponent },
    { path: 'cargo-tracking/changepassword', component: ChangePasswordComponent },
    {
        path: 'cargo-tracking',
        component: UserDashboardComponent,
        canActivate: [AuthGuard],
        children: [
            { path: "", redirectTo: "shipments", pathMatch: "full" },
            { path: "shipments", component: ShipmentsListComponent },
            { path: "shipment/:SecurityKey", component: ShipmentDetailsComponent },
            { path: "favorites", component: FavoritesPageComponent },
            { path: "favorites", component: FavoritesPageComponent },
            { path: '**', redirectTo: 'cargo-tracking', pathMatch: 'full' },
        ]
    },
    {
        path: 'public-tracking/search',
        component: HomeComponent,
        children: [
            { path: "", component: SearchComponent },  
            { path: "shipment/:SecurityKey", component: PublicShipmentDetailsComponent },
            { path: "shipment", redirectTo: 'public-tracking/search' },
            { path: ":searchKey", component: SearchComponent },
            { path: '**', redirectTo: 'public-tracking/search', pathMatch: 'full' },
        ]
    },

    
    { path: 'Error401', component: Error401Component },
    { path: '', redirectTo: 'public-tracking/search', pathMatch: 'full' },
    { path: '**', redirectTo: 'public-tracking/search', pathMatch: 'full' },
];
@NgModule({
    imports: [RouterModule.forRoot(routes)], //, { useHash: true}
    exports: [RouterModule]
})
export class AppRoutingModule { }