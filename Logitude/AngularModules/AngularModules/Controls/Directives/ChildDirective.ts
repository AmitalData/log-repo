import { Directive, ViewContainerRef } from '@angular/core';

@Directive({
    selector: '[ChildDirective]'
})

export class ChildDirective {
    public Code: string;
    constructor(public Location: ViewContainerRef) {

    }
}
