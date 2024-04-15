import { Directive, ElementRef } from '@angular/core';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';

@Directive({
    selector: '[text-start]'
})

export class TextAlignStart {

    public isRTL: boolean = false;

    constructor(el: ElementRef) {

        this.isRTL = ObjectsLocator.GlobalSetting ? (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl") : false;



        // el.nativeElement.style.textAlign = (this.isRTL ? 'left' : 'right');
        el.nativeElement.style.textAlign = (this.isRTL ? 'right' : 'left');

    }



}
