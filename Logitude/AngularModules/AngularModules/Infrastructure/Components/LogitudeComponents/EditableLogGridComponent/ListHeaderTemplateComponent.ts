declare var System: any;

import {Component, ElementRef, OnInit} from '@angular/core';

@Component({
    selector: 'list-header-template',
    template: `<!--<div>-->
                
               <span><span style="text-overflow: ellipsis" *ngIf="noComponent">{{col.Display}}</span></span>
            
               <!--</div>-->`,
    inputs: ['colDef']
})

export class ListHeaderTemplateComponent implements OnInit {

    public colDef: any;

    constructor(private _elementRef: ElementRef) {
        this.noComponent = false;
    }

    public noComponent: boolean;

    ngOnInit() {


    }

}