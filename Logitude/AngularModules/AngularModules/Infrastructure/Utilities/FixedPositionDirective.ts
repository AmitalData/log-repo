import {ViewContainerRef, Directive, Input, Output, EventEmitter, AfterViewInit , OnInit, OnDestroy} from '@angular/core';
import {ObjectsLocator} from '../Locators/ObjectsLocator';

@Directive({
    selector: '[FixedPosition]'
})

export class FixedPositionDirective implements AfterViewInit, OnDestroy {
    @Input() ElementId: string;
    @Input() RelativeElementId: string;
    @Input() TopDisplacement: number;
    @Input() LeftDisplacement: number;
    @Input() ComponentType: string;
    @Input() ComponentHight: number; 
    @Input() PopupWidth: number; 
    @Input() VertDisplacement: number;
    @Input() HorizDisplacement: number;
    @Input() UseDefaultPosition: boolean = false;
    PaintTop: boolean = false;
    PaintRight: boolean = false;
    LayoutDirection: string ;
    constructor(public viewContainerRef: ViewContainerRef) {
        this.LayoutDirection = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
    }

    ngOnInit() {

    }

    ngAfterViewInit() {
        document.getElementById(this.ElementId).style.position = "fixed";
        this.CalculateFixedPosition()
        this.RunPositionTimer();
    }

    ngOnDestroy() {
        this.StopPositionTimer();
    }

    CalculateFixedPosition() {
        if (this.LayoutDirection == 'rtl' && this.ComponentType != 'DocumentPanel') { // DocumentPanel will be LTR always
            this.DrawRightToLeft();
        }
        else {
            this.DrawLeftToRight();
        }
     
    }

    private timerToken: any;
    private StopPositionTimer() {
        if (this.timerToken) {
            clearTimeout(this.timerToken);
        }
    }
    private RunPositionTimer() {
        this.StopPositionTimer();
        this.timerToken = setInterval(() => this.CalculateFixedPosition(), 100);
    }

    getScreenHeight() {
        if (self.innerHeight) {
            return self.innerHeight;
        }

        if (document.documentElement && document.documentElement.clientHeight) {
            return document.documentElement.clientHeight;
        }

        if (document.body) {
            return document.body.clientHeight;
        }
    }

    getScreenWidth() {
        if (self.innerWidth) {
            return self.innerWidth;
        }

        if (document.documentElement && document.documentElement.clientWidth) {
            return document.documentElement.clientWidth;
        }

        if (document.body) {
            return document.body.clientWidth;
        }
    }

    DrawLeftToRight() {

        if (this.UseDefaultPosition) {
            return;
        }
        if (this.RelativeElementId) {
            var item = document.getElementById(this.RelativeElementId);
            if (item) {
                var itemRect = item.getBoundingClientRect();

                //// Abdullah
                var h = this.getScreenHeight()
                let componentScreenHight=this.GetComponentScreenHight(itemRect);
                if (componentScreenHight > h) {
                    this.PaintTop = true;
                }
                if (itemRect.left + this.PopupWidth > this.getScreenWidth()) {
                    this.PaintRight = true;
                }

                document.getElementById(this.ElementId).style.minWidth = (itemRect.width > this.PopupWidth ? this.PopupWidth : itemRect.width) + 'px';

                var left = 0;
                var top = 0;

                if (this.ComponentType == "popup") {
                    document.getElementById(this.ElementId).style.position = "fixed";
                    top = (itemRect.top + this.TopDisplacement);
                    left = itemRect.left + itemRect.width;
                }
                else if (this.ComponentType == "datepicker") {
                    document.getElementById(this.ElementId).style.minWidth = '';
                    top = (this.PaintTop ? (itemRect.top - this.ComponentHight) : (itemRect.top + this.VertDisplacement));
                    left = itemRect.left + itemRect.width + 20 - this.PopupWidth;
                }
                else if (this.ComponentType == "MTCPopoup") {
                    top = (this.PaintTop ? (itemRect.top - this.ComponentHight) : (itemRect.top + this.VertDisplacement));
                    left = (this.PaintRight ? (this.getScreenWidth() - this.PopupWidth) : (itemRect.left - this.PopupWidth + itemRect.width));
                }
                else {
                    top = (this.PaintTop ? (itemRect.top - this.ComponentHight) : (itemRect.top + this.VertDisplacement));
                    left = (this.PaintRight ? (this.getScreenWidth() - this.PopupWidth) : (itemRect.left));
                }
                left = left + (this.HorizDisplacement ? this.HorizDisplacement : 0);

                left = (this.ComponentType == "DocumentPanel" ? left - 25 : left);

                document.getElementById(this.ElementId).style.top = top + 'px';
                document.getElementById(this.ElementId).style.left = left + 'px';

            }
        }
    }

    DrawRightToLeft() {
        if (this.UseDefaultPosition) {
            return;
        }
        if (this.RelativeElementId) {
            var item = document.getElementById(this.RelativeElementId);
            if (item) {
                var itemRect = item.getBoundingClientRect();

                //// Abdullah
                var h = this.getScreenHeight()
                let componentScreenHight=this.GetComponentScreenHight(itemRect);
                if (componentScreenHight > h) {
                    this.PaintTop = true;
                }
                //if (itemRect.left + this.PopupWidth > this.getScreenWidth()) {
                //    this.PaintRight = true;
                //}

                document.getElementById(this.ElementId).style.minWidth = (itemRect.width > this.PopupWidth ? this.PopupWidth : itemRect.width)+ 'px';

                var left = 0;
                var top = 0;

                if (this.ComponentType == "popup") {
                    document.getElementById(this.ElementId).style.position = "fixed";

                    top = (itemRect.top + this.TopDisplacement);
                    left = itemRect.left + itemRect.width - this.PopupWidth;
                }
                else if (this.ComponentType == "datepicker") {
                    document.getElementById(this.ElementId).style.minWidth = '';

                    top = (this.PaintTop ? (itemRect.top - this.ComponentHight) : (itemRect.top + this.VertDisplacement));
                    left = itemRect.left + itemRect.width - this.PopupWidth;
                }
                else {
                    top = (this.PaintTop ? (itemRect.top - this.ComponentHight) : (itemRect.top + this.VertDisplacement));
                    left = (this.PaintRight ? (this.getScreenWidth() - this.PopupWidth) : (itemRect.left - this.PopupWidth + itemRect.width));
                }

                left = left + (this.HorizDisplacement ? this.HorizDisplacement : 0);

                document.getElementById(this.ElementId).style.top = top + 'px';
                document.getElementById(this.ElementId).style.left = (left > 0 ? left : 0) + 'px';

            }
        }
    }

    GetComponentScreenHight(itemRect:ClientRect){
        return itemRect.bottom + this.ComponentHight+10;
    }
}
