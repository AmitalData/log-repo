import { Directive, ViewContainerRef } from '@angular/core';

@Directive({
  selector: '[ChildDirective]'
})

export class ChildDirective {
  constructor(public Location: ViewContainerRef) {

  }
}
