import { NgModule, SystemJsNgModuleLoader } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { LAZY_WIDGETS } from './DynamicLoader/LazyWidgetsTokens';
import { LazyArrayToObjects } from './DynamicLoader/LazyWidgets';
import { ChildDirective } from './Directives/ChildDirective';
import { AppComponent } from './AppComponent';

@NgModule({
  imports: [BrowserModule, HttpClientModule],
  declarations: [ChildDirective, AppComponent],

  providers: [
    SystemJsNgModuleLoader,

    { provide: LAZY_WIDGETS, useFactory: LazyArrayToObjects }

  ],

  bootstrap: [AppComponent]
})

export class AppModule { }
