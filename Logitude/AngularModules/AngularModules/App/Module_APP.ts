import { ErrorHandler, NgModule, SystemJsNgModuleLoader } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { LAZY_WIDGETS } from './DynamicLoader/LazyWidgetsTokens';
import { ChildDirective } from './Directives/ChildDirective';



import { AppComponent } from './AppComponent';
import { LazyArrayToObjects } from './DynamicLoader/LazyWidgetsLogitude';
import { CookieconsentComponent } from '../Common/Components/cookieconsent/cookieconsent.component';
// import { AppComponent } from './AppComponent_Cust';

// import { LazyArrayToObjects } from './DynamicLoader/LazyWidgetsCustoms';

 
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'; 

import { LogitudeMonitoringService } from './Services/logging.service';
import { ErrorHandlerService } from './Services/ErrorHandler.Service';

 


@NgModule({
  imports: [BrowserModule, HttpClientModule, BrowserAnimationsModule],
  declarations: [ChildDirective, AppComponent, CookieconsentComponent],

  providers: [
    SystemJsNgModuleLoader,
    //LogitudeMonitoringService,
    // { provide: ErrorHandler, useClass: ErrorHandlerService },
    { provide: LAZY_WIDGETS, useFactory: LazyArrayToObjects }  

     

  ],

  bootstrap: [AppComponent]
})

export class AppModule { }
