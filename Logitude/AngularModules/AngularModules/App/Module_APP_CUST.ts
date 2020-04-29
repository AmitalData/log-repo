import { NgModule, SystemJsNgModuleLoader } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { LAZY_WIDGETS } from './DynamicLoader/LazyWidgetsTokens';
import { LazyArrayToObjects } from './DynamicLoader/LazyWidgetsCustoms';
import { ChildDirective } from './Directives/ChildDirective';
import { AppComponent_Cust } from './AppComponent_Cust';

@NgModule({
  imports: [BrowserModule, HttpClientModule],
  declarations: [ChildDirective, AppComponent_Cust],

  providers: [
    SystemJsNgModuleLoader,

    { provide: LAZY_WIDGETS, useFactory: LazyArrayToObjects }

  ],

  bootstrap: [AppComponent_Cust]
})

export class AppModule { }
