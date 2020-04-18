
import { Component, OnInit, ViewChild, ViewContainerRef } from '@angular/core';
import { Injector, Compiler, Inject, NgModuleFactory, Type } from '@angular/core';
import { LAZY_WIDGETS } from './DynamicLoader/LazyWidgetsTokens';
import { DynamicLoader } from './DynamicLoader/DynamicLoader';
import { HttpClient } from '@angular/common/http';
import { environment } from '../environments/environment';

@Component({
    selector: 'AppComponent',
    
    template:
    `
    <div class="MediaFillRelative">
        <img *ngIf="!_FinishLogin" class="CenterCenter" src="./_Resources/Images/Gif/Bluespin.gif" />
        <div #Child></div>
    </div>
    `,
})

export class AppComponent implements OnInit {
  _FinishLogin: boolean = false;

  @ViewChild("Child", { read: ViewContainerRef, static: true }) location: ViewContainerRef;

  constructor(private http: HttpClient, private injector: Injector, private compiler: Compiler, @Inject(LAZY_WIDGETS) private lazyWidgets: { [key: string]: () => Promise<NgModuleFactory<any> | Type<any>> }) {
    DynamicLoader.Injector = injector;
    DynamicLoader.Compiler = compiler;
    DynamicLoader.LazyWidgets = lazyWidgets;
  }
  
  ngOnInit() {

    setTimeout(() => { this._FinishLogin = true; }, 30000);

    DynamicLoader.Load("./Infrastructure/RootComponent", this.location)
      .then(cmpRef => {
        cmpRef.instance.Boot({ Http: this.http });
      });
  }

}
