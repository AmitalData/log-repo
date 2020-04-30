import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
//import { AppModule } from './Module_APP';
import { environment } from '../environments/environment';
import { AppModule } from './Module_APP_CUST';

if (environment.production) {
    enableProdMode();
}

//platformBrowserDynamic().bootstrapModule(AppModule).catch(err => console.log(err));

//import('./Module_APP_CUST').then(m => {
//  platformBrowserDynamic().bootstrapModule(m.AppModule).catch(err => console.log(err));
//});

platformBrowserDynamic().bootstrapModule(AppModule).catch(err => console.log(err));

