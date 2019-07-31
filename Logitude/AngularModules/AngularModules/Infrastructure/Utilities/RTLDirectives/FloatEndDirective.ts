import { Directive, ElementRef } from '@angular/core';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';

@Directive({
    selector: '[float-end]'
})

export class FloatEndDirective {

    public isRTL: boolean = false;

    constructor(el: ElementRef) {

        this.isRTL = ObjectsLocator.GlobalSetting ? (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl") : false;



        el.nativeElement.style.float = (this.isRTL ? 'left' : 'right');

    }



}
