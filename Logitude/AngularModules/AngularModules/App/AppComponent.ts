import { Component, ViewChild, AfterViewInit } from '@angular/core';
import { Injector, Compiler, Inject, NgModuleFactory, Type } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LAZY_WIDGETS } from './DynamicLoader/LazyWidgetsTokens';
import { DynamicLoader } from './DynamicLoader/DynamicLoader';
import { ChildDirective } from './Directives/ChildDirective';
//import { LogitudeMonitoringService } from './Services/logging.service';

@Component({
    selector: 'AppComponent',
    
    template:
    `
    <div class="MediaFillRelative">
        <img *ngIf="!IsLoginScreenLoaded" class="CenterCenter" src="./_Resources/Images/Gif/Bluespin.gif" />
        <div ChildDirective></div>
    </div>
    `,
})

export class AppComponent implements AfterViewInit {
  public IsLoginScreenLoaded: boolean = false;
  
  @ViewChild(ChildDirective) Child: ChildDirective;
  //,private logitudeMonitoringService : LogitudeMonitoringService
  constructor(private http: HttpClient, private injector: Injector, private compiler: Compiler, @Inject(LAZY_WIDGETS) private lazyWidgets: { [key: string]: () => Promise<NgModuleFactory<any> | Type<any>> }) {
    DynamicLoader.Injector = injector;
    DynamicLoader.Compiler = compiler;
    DynamicLoader.LazyWidgets = lazyWidgets;
  }
  
  ngAfterViewInit() {
    DynamicLoader.Load("./Infrastructure/RootComponent", this.Child.Location)
      .then(cmpRef => {
        this.IsLoginScreenLoaded = true;
        cmpRef.instance.Boot({ Http: this.http });
      });
  }
}
