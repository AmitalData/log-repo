import {Component, OnInit} from 'angular2/core';
import {RouteConfig, ROUTER_DIRECTIVES, AsyncRoute} from 'angular2/router';

//import {EditControlComponent} from './infrastructure/edit-control/edit-control.component';
//import {ShipmentsListComponent} from './shipment/shipments-list/shipments-list.component';
//import {MainMenuComponent} from './infrastructure/main-menu/main-menu.component';


@Component({
    //selector: 'my-app',
    templateUrl: './app/app.component.html',
    directives: [ROUTER_DIRECTIVES]
})
@RouteConfig([

    //{
    //    path: '/main-menu/...',
    //    name: 'MainMenu',
    //    component: MainMenuComponent,
    //    useAsDefault: true
    //},
    new AsyncRoute({
        path: '/main-menu/...',
        loader: () => System.import('./app/infrastructure/main-menu/main-menu.component').then(m => m["MainMenuComponent"]),
        name: 'MainMenu',
        useAsDefault: true
    }),
    //{ 
    //    path: '/edit-control/:id/...',
    //    name: 'EditControl',
    //    component: EditControlComponent,
    //},
    new AsyncRoute({
        path: '/edit-control/:id',
        loader: () => System.import('./app/infrastructure/edit-control/edit-control.component').then(m => m.EditControlComponent),
        name: 'EditControl'
    })

])
export class AppComponent implements OnInit {

    constructor() { }

    ngOnInit() {
        
    }

}

var dynaRoutes = [
    new AsyncRoute({
        path: '/main-menu/...',
        loader: () => System.import('./app/infrastructure/main-menu/main-menu.component').then(m => m["MainMenuComponent"]),
        name: 'MainMenu',
        useAsDefault: true
    }),
    new AsyncRoute({
        path: '/edit-control/:id/...',
        loader: () => System.import('./app/infrastructure/edit-control/edit-control.component').then(m => m.EditControlComponent),
        name: 'EditControl'
    })
];