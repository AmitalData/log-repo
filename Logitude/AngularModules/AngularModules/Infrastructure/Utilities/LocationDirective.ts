import {ViewContainerRef, Directive, Input, Output, EventEmitter, AfterViewInit} from '@angular/core';

@Directive({
    selector: '[LocationDirective]'
})

export class LocationDirective implements AfterViewInit {
    @Input() Code: string;
    @Input() Index: number;
    @Input() ItemCode: string;
    @Output() DirectiveLoaded = new EventEmitter();
    constructor(public viewContainerRef: ViewContainerRef) {

    }

    ngAfterViewInit() {
        //this.DirectiveLoaded.emit('Directive Loaded');
    }
}
