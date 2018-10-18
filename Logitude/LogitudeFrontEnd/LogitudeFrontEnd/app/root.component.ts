import {Component} from 'angular2/core';
import {RouteConfig, RouterOutlet, AsyncRoute} from 'angular2/router';
//import {AppComponent} from './app.component';
//import {LoginComponent} from './infrastructure/login/login.component';

@Component({
    selector: 'my-app',
    templateUrl: 'app/root.component.html',
  //  template: `
  //  <router-outlet></router-outlet>
  //`,
    directives: [RouterOutlet]
})

@RouteConfig([

    //{
    //    path: '/root/...',
    //    name: 'Root',
    //    component: AppComponent,
    //    //useAsDefault: true
    //},
    new AsyncRoute({
        path: '/root/...',
        loader: () => System.import('./app/app.component').then(m => m.AppComponent),
        name: 'Root'
    }),
    //{
    //    path: '/login',
    //    name: 'Login',
    //    component: LoginComponent,
    //    useAsDefault: true
    //},
    new AsyncRoute({
        path: '/login',
        loader: () => System.import('./app/infrastructure/login/login.component').then(m => m.LoginComponent),
        name: 'Login',
        useAsDefault: true
    }),

])

export class RootComponent { }
