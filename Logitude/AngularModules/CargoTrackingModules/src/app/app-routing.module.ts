
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SearchComponent } from 'src/CargoTracking/Components/Search/search.component';
import { ShipmentComponent } from 'src/CargoTracking/Components/Shipment/shipment.component';

const routes: Routes = [

    {path: 'search', redirectTo: 'search/', pathMatch: 'full'},
    { path: 'search/:searchKey', component: SearchComponent },
    // { path: 'pageb', component: PageBComponent },
    // { path: 'login', component: LoginComponent },
    {
        path: 'shipment/:shipmentId', component: ShipmentComponent
    },
    { path: '', redirectTo: '/search/', pathMatch: 'full' },
    { path: '**', redirectTo: '/search/', pathMatch: 'full' },
];

@NgModule({
    imports: [RouterModule.forRoot(routes)], //,  { useHash: true}
    exports: [RouterModule]
})
export class AppRoutingModule { }
