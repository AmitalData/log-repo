import { Component, Input, OnInit, ChangeDetectorRef, ContentChildren, TemplateRef, ContentChild} from '@angular/core';
import {LogColumnComponent} from './LogColumnComponent'
import {LogFooterTemplateComponent} from './LogFooterTemplateComponent'
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';

@Component({
    selector: 'log-footer',
    moduleId: module.id,
    templateUrl: './LogFooterComponent.html',
})

export class LogFooterComponent  {
   
    constructor() {
        
    }
    @ContentChild(TemplateRef) myChild: any;
    ngOnInit() { 
   
      
    }
    ngAfterContentInit() {
        // get all active tabs
        //if (this.Children.length > 0) {
        //    //alert("bbbbbbbbb");
        //}
        //this.hastemplate = this.innerContentTpl.length == 0 ? false : true;
    }
}
