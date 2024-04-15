import { ViewContainerRef, Directive, Input, Output, EventEmitter, AfterViewInit, ElementRef} from '@angular/core';

@Directive({
    selector: '[LocationDirective]'
})

export class LocationDirective implements AfterViewInit {
    @Input() Code: string;
    @Input() Index: number;
    @Input() ItemCode: string;
    @Output() DirectiveLoaded = new EventEmitter();
    
    private _IsSelected: boolean = false;
    @Input()
    public get IsSelected(): boolean {
        return this._IsSelected;
    }
    public set IsSelected(value: boolean) {
        this._IsSelected = value;
        //this.elementRef.nativeElement.style.backgroundColor = '';
        if (value) {
            this.elementRef.nativeElement.scrollIntoView(true);
            //this.elementRef.nativeElement.style.backgroundColor = 'red';
        }
    }
    constructor(public viewContainerRef: ViewContainerRef, private elementRef: ElementRef) {

    }

    ngAfterViewInit() {
        //this.DirectiveLoaded.emit('Directive Loaded');
    }
}
