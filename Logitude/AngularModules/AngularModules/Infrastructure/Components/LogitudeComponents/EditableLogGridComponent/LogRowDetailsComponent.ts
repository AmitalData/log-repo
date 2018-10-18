import { Component, Input, OnInit, QueryList, ViewChildren, ContentChildren, TemplateRef, ContentChild} from '@angular/core'; 
import {LogCellTemplateComponent} from './LogCellTemplateComponent';

@Component({
    selector: 'log-row-details',
    template: '', 
})
export class LogRowDetailsComponent {
    hastemplate: boolean;
    index: number;
    @ContentChildren(TemplateRef) innerContentTpl: any;
    ngAfterContentInit() {
        this.hastemplate = this.innerContentTpl.length == 0 ? false : true;  
    }
    
    ngOnInit() { 
    }
}
