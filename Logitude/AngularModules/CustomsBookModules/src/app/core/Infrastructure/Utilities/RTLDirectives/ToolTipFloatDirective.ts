import { Directive, ElementRef, Input } from '@angular/core';

@Directive({
    selector: '[ToolTipFloatDirective]'
})

export class ToolTipFloatDirective {
 
    @Input('ToolTipFloatDirective') toolTipFloatDirectiveFloat: string;

    constructor(el: ElementRef) {
       if(this.toolTipFloatDirectiveFloat){
        el.nativeElement.style.float = this.toolTipFloatDirectiveFloat;
       }

    }
}
