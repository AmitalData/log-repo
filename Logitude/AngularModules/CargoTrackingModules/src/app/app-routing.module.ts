
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SearchComponent } from 'src/CargoTracking/Components/Search/search.component';
import { ShipmentComponent } from 'src/CargoTracking/Components/Shipment/shipment.component';

const routes: Routes = [
    
    {path: ':Tenant', redirectTo: '/:Tenant/search/', pathMatch: 'full'},
    {path: ':Tenant/search', redirectTo: '/:Tenant/search/', pathMatch: 'full'},
    {path: ':Tenant/search/:searchKey', component: SearchComponent },
    // { path: 'pageb', component: PageBComponent },
    // { path: 'login', component: LoginComponent },
    {path: ':Tenant/shipment/:shipmentId', component: ShipmentComponent},    
    {path: '', component: SearchComponent , pathMatch: 'full' },
    {path: '**',component: SearchComponent, pathMatch: 'full' },
];

@NgModule({
    imports: [RouterModule.forRoot(routes)], //,  { useHash: true}
    exports: [RouterModule]
})
export class AppRoutingModule { }
