import { Component, Input, OnInit, ChangeDetectorRef, ContentChildren, TemplateRef} from '@angular/core';
import {LogColumnComponent} from './LogColumnComponent'
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';

@Component({
    selector: 'log-footer-template',
    moduleId: module.id,
    templateUrl: './LogFooterTemplateComponent.html',
})

export class LogFooterTemplateComponent  {
   
    constructor() {
        
    }
    @ContentChildren(TemplateRef) Children: any;
    ngOnInit() { 
   
      
    }
    ngAfterContentInit() {
        // get all active tabs
        if (this.Children.length > 0) {
            //alert("bbbbbbbbb");
        }
        //this.hastemplate = this.innerContentTpl.length == 0 ? false : true;
    }
}
