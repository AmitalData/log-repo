import {Component, OnInit, AfterViewInit, Output, EventEmitter, ChangeDetectionStrategy} from '@angular/core';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';

@Component({
    selector: 'CellTooltip',

    templateUrl: './CellTooltip.html',
    inputs: ['IconWidth', 'IconHeight', 'IconPath', 'Width', 'Height', 'Head', 'Body', 'MaxHeight', 'IsOnClick', 'IsOpened','IsToRight'],
    changeDetection: ChangeDetectionStrategy.OnPush,
})

export class CellTooltip implements OnInit, AfterViewInit {
    public TooltipId: string = null;
    public TooltipButtonId: string = null;
    public TooltipContentId: string = null;
    public IconWidth: number = 18;
    public IconHeight: number = 18;
    public IconPath: string = "./Images/Help.png";
    public IconBackground: string = null;
    public Width: number = 296;
    public MinHeight: number = 130;
    public MaxHeight: number = 130;
    public IsOnClick: boolean = false;
    public Head: string = null;
    public Body: string = null;
    public IsMouseOver: boolean = false;
    public IsPopupMouseOver: boolean = false;
    @Output() Opened: EventEmitter<boolean> = new EventEmitter<boolean>();
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        var idIndex = this.CurrentSession.GetNewId("Tooltip");
        this.TooltipId = "Tooltip_" + idIndex;
        this.TooltipButtonId = "TooltipButton_" + idIndex;
        this.TooltipContentId = "TooltipContent_" + idIndex;
    }

    ngOnInit() {
        this.IconBackground = "url(" + this.IconPath + ") no-repeat";
    }

    ngAfterViewInit() {
        if (this.IsOnClick) {
            document.getElementById(this.TooltipButtonId).style.cursor = "pointer";
        }

        else {
            document.getElementById(this.TooltipButtonId).style.cursor = "default";
        }
    }

    private height: number = 130;
    get Height() { return this.height; }
    set Height(value: number) {
        if (this.height != value) {
            if (value > this.MinHeight) {
                this.height = value;

                this.SetTooltipSize();
            }
        }
    }

    private isOpened: boolean = false;
    get IsOpened() { return this.isOpened; }
    set IsOpened(value: boolean) {
        if (value != undefined) {
            if (this.isOpened != value) {
                this.isOpened = value;

                if (!value) {
                    document.getElementById(this.TooltipContentId).style.visibility = "hidden";
                }
            }
        }
    }

    private isToRight: boolean = false;
    get IsToRight() { return this.isToRight; }
    set IsToRight(value: boolean) {
        if (this.isToRight != value) {
            this.isToRight = value;
        }
    }

    mouseover() {
        this.IsMouseOver = true;
        if (!this.IsOnClick) {
            var item = document.getElementById(this.TooltipId);
            var itemRect = item.getBoundingClientRect();
            if (this.IsToRight) {
                document.getElementById(this.TooltipContentId).style.position = "fixed";
                document.getElementById(this.TooltipContentId).style.top = (itemRect.top - this.Height + 5) + 'px';
                document.getElementById(this.TooltipContentId).style.backgroundImage = "url('./_Resources/Images/Icons/Tooltips/Tootip.png')";
                document.getElementById(this.TooltipContentId).style.left = (itemRect.left + 5) + 'px';
            }
            else {
                document.getElementById(this.TooltipContentId).style.top = (itemRect.top - (this.Height / 2) + 7) + 'px';
                document.getElementById(this.TooltipContentId).style.left = (itemRect.left - this.Width + 5) + 'px';
            }
            document.getElementById(this.TooltipContentId).style.visibility = "visible";
        }
    }

    mouseleave() {
        this.IsMouseOver = false;
        if (!this.IsOnClick) {
                document.getElementById(this.TooltipContentId).style.visibility = "hidden";
        }
    }

    OnButtonMouseLeave(){
        setTimeout(() => {
            if (!this.IsOnClick && !this.IsPopupMouseOver) {
                document.getElementById(this.TooltipContentId).style.visibility = "hidden";
            }
        },200)
    }
    click() {
        if (this.IsOnClick) {
            if (document.getElementById(this.TooltipContentId).style.visibility == "visible") {
                document.getElementById(this.TooltipContentId).style.visibility = "hidden";
                this.isOpened = false;
                this.Opened.emit(this.isOpened);
            }

            else {
                var item = document.getElementById(this.TooltipId);
                var itemRect = item.getBoundingClientRect();

                document.getElementById(this.TooltipContentId).style.top = (itemRect.top - (this.Height / 2) + 7) + 'px';
                document.getElementById(this.TooltipContentId).style.left = (itemRect.left - this.Width + 5) + 'px';
                document.getElementById(this.TooltipContentId).style.visibility = "visible";
                this.isOpened = true;
                this.Opened.emit(this.isOpened);
            }
        }
    }

    blur() {
        if (this.IsOnClick) {
            if (this.IsMouseOver) {
                document.getElementById(this.TooltipButtonId).focus();
            }

            else {
                document.getElementById(this.TooltipContentId).style.visibility = "hidden";
                this.isOpened = false;
                this.Opened.emit(this.isOpened);
            }
        }
    }



    
    SetTooltipSize() {
        if (this.TooltipId) {
            var item = document.getElementById(this.TooltipId);

            if (item) {
                var itemRect = item.getBoundingClientRect();

                var fixedHeight = this.Height;
                if (fixedHeight > this.MaxHeight) {
                    fixedHeight = this.MaxHeight;
                }

                document.getElementById(this.TooltipContentId).style.top = (itemRect.top - (fixedHeight / 2) + 7) + 'px';
            }
        }
    }
}
