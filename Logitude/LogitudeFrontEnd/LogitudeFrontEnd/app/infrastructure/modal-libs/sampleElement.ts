import {Component, ElementRef, Inject, forwardRef} from 'angular2/core';
import {ShipmentsListComponent} from '../../shipment/shipments-list/shipments-list.component';

@Component({
    selector: 'sample-element',
    template:
    `<!--
    <div>
        <h1>I Am an Element!</h1>
        <p>I can be modaled!</p>
    </div>
    -->
    <div #myModal></div>
     `
})
export class SampleElement {
    constructor( @Inject(forwardRef(() => ShipmentsListComponent)) listPage: ShipmentsListComponent, elementRef: ElementRef) {
        //TODO: Replace with querying instead of letting the DI decide?
        listPage.mySampleElement = elementRef;
    }
}