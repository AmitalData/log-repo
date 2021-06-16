import { NgModule, SystemJsNgModuleLoader } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { LAZY_WIDGETS } from './DynamicLoader/LazyWidgetsTokens';
import { ChildDirective } from './Directives/ChildDirective';

// import { AppComponent } from './AppComponent';
// import { LazyArrayToObjects } from './DynamicLoader/LazyWidgetsLogitude';

import { AppComponent } from './AppComponent_Cust';
import { LazyArrayToObjects } from './DynamicLoader/LazyWidgetsCustoms';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
    imports: [BrowserModule, HttpClientModule,
        BrowserAnimationsModule, // primeng>> npm i @angular/animations@latest --save
    ],
  declarations: [ChildDirective, AppComponent],

  providers: [
    SystemJsNgModuleLoader,

    { provide: LAZY_WIDGETS, useFactory: LazyArrayToObjects } 

  ],

  bootstrap: [AppComponent]
})

export class AppModule { }
