import {ViewContainerRef, Directive, Input, Output, EventEmitter, AfterViewInit} from '@angular/core';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';

@Directive({
    selector: '[FocusMe]'
})

export class FocusMeDirective implements AfterViewInit {
    @Input() ElementId: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public viewContainerRef: ViewContainerRef) {
        
    }

    ngAfterViewInit() {

       // console.log("i will focus the hell out of you." + this.ElementId);
        var element = document.getElementById(this.ElementId);
        element.focus();
        this.CurrentSession.SessionEvent.emit({ IsCell : true,Id : element.id });
        //element.addEventListener("blur", function () {
        //    this.CurrentSession.SessionEvent.emit("LostFocusMe");
        //});
    }

    BlurMe() {
    }
}
