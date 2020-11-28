
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { Error401Component } from 'src/CargoTracking/Components/Errors/Error401Component';
import { HomeComponent } from 'src/CargoTracking/Components/PublicSite/HomeComponent/HomeComponent';
import { PublicShipmentDetailsComponent } from 'src/CargoTracking/Components/PublicSite/PublicShipmentDetailsComponent/PublicShipmentDetailsComponent';
import { SearchComponent } from 'src/CargoTracking/Components/PublicSite/SearchComponent/SearchComponent';
import { FavoritesPageComponent } from 'src/CargoTracking/Components/UserDashboard/FavoritesPage/FavoritesPageComponent';
import { ShipmentDetailsComponent } from 'src/CargoTracking/Components/UserDashboard/ShipmentsPage/ShipmentDetails/ShipmentDetailsComponent';
import { ShipmentsListComponent } from 'src/CargoTracking/Components/UserDashboard/ShipmentsPage/ShipmentsList/ShipmentsListComponent';
import { UserDashboardComponent } from 'src/CargoTracking/Components/UserDashboard/UserDashboardComponent';

const routes: Routes = [
    

      
    { 
        path: 'dashboard', 
        component: UserDashboardComponent,
        children: [
            { path: "", redirectTo: "shipments", pathMatch: "full" }, 
            { path: "shipments", component: ShipmentsListComponent }, 
            { path: "shipment/:SecurityKey", component: ShipmentDetailsComponent }, 
            { path: "favorites", component: FavoritesPageComponent }, 
           
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
            {path: '**', redirectTo: 'public-tracking/search', pathMatch: 'full' }, 
        ]
    },
    
    {path: 'Error401', component: Error401Component },
    {path: '', redirectTo: 'public-tracking/search', pathMatch: 'full' },
    {path: '**', redirectTo: 'public-tracking/search', pathMatch: 'full' },
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
