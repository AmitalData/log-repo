
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from 'src/Infrastructure/Components/LoginComponent/Login.Component';
import { HomeComponent } from 'src/CargoTracking/Components/PublicSite/HomeComponent/HomeComponent';
import { PublicShipmentDetailsComponent } from 'src/CargoTracking/Components/PublicSite/PublicShipmentDetailsComponent/PublicShipmentDetailsComponent';
import { SearchComponent } from 'src/CargoTracking/Components/PublicSite/SearchComponent/SearchComponent';
import { FavoritesPageComponent } from 'src/CargoTracking/Components/UserDashboard/FavoritesPage/FavoritesPageComponent';
import { ShipmentDetailsComponent } from 'src/CargoTracking/Components/UserDashboard/ShipmentsPage/ShipmentDetails/ShipmentDetailsComponent';
import { ShipmentsListComponent } from 'src/CargoTracking/Components/UserDashboard/ShipmentsPage/ShipmentsList/ShipmentsListComponent';
import { UserDashboardComponent } from 'src/CargoTracking/Components/UserDashboard/UserDashboardComponent';
import { ResetPasswordComponent } from 'src/Infrastructure/Components/LoginComponent/ResetPassword.Component';
import { ChangePasswordComponent } from 'src/Infrastructure/Components/LoginComponent/ChangePassword.Component';
import { AuthGuardService as AuthGuard  } from 'src/Infrastructure/Services/auth-guard.service';

const routes: Routes = [
    

      
    { 
        path: ':Tenant/dashboard', 
        component: UserDashboardComponent,
        canActivate: [AuthGuard],
        children: [
            { path: "", redirectTo: "shipments", pathMatch: "full" }, 
            { path: "shipments", component: ShipmentsListComponent }, 
            { path: "shipment/:SecurityKey", component: ShipmentDetailsComponent }, 
            { path: "favorites", component: FavoritesPageComponent  }, 
           
        ]
    },
    {
        path: ':Tenant/search',
        component: HomeComponent,
        children: [ 
            // { path: "", redirectTo: "/:Tenant/search/", pathMatch: "full" },
            { path: "", component: SearchComponent }, 
            { path: "shipment/:SecurityKey", component: PublicShipmentDetailsComponent },
            { path: "shipment", redirectTo: ':Tenant/search' },
            { path: ":searchKey", component: SearchComponent },
            {path: '**', redirectTo: '/1/search/', pathMatch: 'full' }, 
        ]
    },
    
 
    { path: 'login', component: LoginComponent },
    { path: 'resetpassword', component: ResetPasswordComponent },
    { path: 'changepassword', component: ChangePasswordComponent },
    // {path: '', component: PublicGateComponent, pathMatch: 'full' },
    {path: '', component: HomeComponent, pathMatch: 'full' },
    {path: ':Tenant', redirectTo: '/:Tenant/search/', pathMatch: 'full'},
    {path: '**', redirectTo: '/1/search/', pathMatch: 'full' },
    // {path: '**',redirectTo: '1/search', pathMatch: 'full'  },

 

    // {path: ':Tenant', redirectTo: '/:Tenant/search/', pathMatch: 'full'},
    // {path: ':Tenant/search', redirectTo: '/:Tenant/search/', pathMatch: 'full'},
    // {path: ':Tenant/search/:searchKey', component: SearchComponent },
    // { path: 'pageb', component: PageBComponent },
    // { path: 'login', component: LoginComponent },
    // {path: ':Tenant/shipment/:SecurityKey', component: ShipmentComponent},    
    // {path: '', component: SearchComponent , pathMatch: 'full' },
    // {path: '**',component: SearchComponent, pathMatch: 'full' },
]; 

@NgModule({
    imports: [RouterModule.forRoot(routes)], //,  { useHash: true}
    exports: [RouterModule]
})
export class AppRoutingModule { }
