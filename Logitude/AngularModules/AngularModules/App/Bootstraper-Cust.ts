import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AppModule } from './Module_APP_CUST';
import { environment } from '../environments/environment';

if (environment.production) {
  enableProdMode();

  import('./Module_APP_CUST').then(m => {
    platformBrowserDynamic().bootstrapModule(m.AppModule).catch(err => console.log(err));
  }); 
}

else {
  import('./Module_APP_CUST').then(m => {
    platformBrowserDynamic().bootstrapModule(m.AppModule).catch(err => console.log(err));
  }); 
}
