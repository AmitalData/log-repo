
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from 'src/CargoTracking/Components/Dashboard/dashboard.component';
import { FavoritesComponent } from 'src/CargoTracking/Components/Dashboard/favorites/favorites.component';
import { DashboardShipmentsComponent } from 'src/CargoTracking/Components/Dashboard/shipments/dashboard-shipments.component';
import { ShipmentDetailsComponent } from 'src/CargoTracking/Components/Dashboard/shipments/ShipmentDetailsComponent';
import { PublicGateComponent } from 'src/CargoTracking/Components/PublicGate/PublicGate.component';
import { SearchComponent } from 'src/CargoTracking/Components/Search/search.component';
import { ShipmentComponent } from 'src/CargoTracking/Components/Shipment/shipment.component';
import { LoginComponent } from 'src/Infrastructure/Components/LoginComponent/Login.Component';

const routes: Routes = [
    

      
    { 
        path: ':Tenant/dashboard', 
        component: DashboardComponent,
        children: [
            { path: "", redirectTo: "shipments", pathMatch: "full" }, 
            { path: "shipments", component: DashboardShipmentsComponent }, 
            { path: "shipment/:SecurityKey", component: ShipmentDetailsComponent }, 
            { path: "favorites", component: FavoritesComponent }, 
           
        ]
    },
    {
        path: ':Tenant/search',
        component: PublicGateComponent,
        children: [
            // { path: "", redirectTo: "/:Tenant/search/", pathMatch: "full" },
            { path: "", component: SearchComponent }, 
            { path: "shipment", component: ShipmentComponent },
            { path: "shipment/:SecurityKey", component: ShipmentComponent },
            { path: ":searchKey", component: SearchComponent },
            {path: '**', redirectTo: '/1/search/', pathMatch: 'full' }, 
        ]
    },
    
 
    {path: ':Tenant/login', component: LoginComponent },
    {path: '', component: PublicGateComponent, pathMatch: 'full' },
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
