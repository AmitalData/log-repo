import { Component, EventEmitter, Output, Input, OnInit, ElementRef, ChangeDetectorRef } from '@angular/core';
import { AppTool } from '../../Tools';

@Component({
    selector: 'log-dropdown-menu',
    host: { '(document:click)': 'handleClick($event)', },
    templateUrl: 'DropdownMenuComponent.html',
})

export class DropdownMenuComponent implements OnInit {
    @Input()
    public IsDisabled: boolean
    @Output()
    public DropdownMenuButtonClicked: EventEmitter<any> = new EventEmitter<any>();

    //private _CustomSendOptionsArgs: CustomSendOptionsArgs;
    public _DropdownDisplay: string = 'none';
    private _ElementRef: any;

    static MyId: number = 0;
    static LastDropdownMenuFilterId: number = 0;
    public _DropdownMenuComponentId: string;
    public _DropdownMenuComponentMenuId: string;
    MyDropdownMenuFilterId: number;

    constructor(private _CD: ChangeDetectorRef, myElement: ElementRef) {
        this._ElementRef = myElement;
        ///this.DataContext = this; 
        //this._CustomSendOptionsArgs = new CustomSendOptionsArgs();
        //this._CustomSendOptionsArgs.ForcePersonalSign = false;
        var curId = DropdownMenuComponent.MyId++;
        this.MyDropdownMenuFilterId = curId;
        this._DropdownMenuComponentId = "DropdownMenuComponent_" + curId;
        this._DropdownMenuComponentMenuId = "DropdownButtonComponentMenuId_" + curId;
    }

    handleClick(event) {
        if (this._DropdownDisplay == 'none') {
            return;
        }
        var clickedComponent = event.target;
        var inside = false;
        let conter = 0;
        do {
            if (clickedComponent === this._ElementRef.nativeElement) {
                inside = true;
                break;
            }
            if (clickedComponent.class === "class-dropdownfilter-content") {
                inside = true;
                break;
            }
            if (conter > 50) {
                break;
            }
            conter++;
            clickedComponent = clickedComponent.parentNode;
        } while (clickedComponent);
        if (inside) {

        } else {

            //if (this._DropdownDisplay == 'block') {
            //    this.DropdowndisplayToggle(null);
            //}
            this.DropdownDisplayClose();
        }
    }

    ngOnInit() {
    }

    DropdownDisplayClose() {
        this._DropdownDisplay = 'none';
        this._CD.detectChanges();
    }


    Width = -60;
    Height = -40;

    public static EnsureLastDropdownMenuIsClosed() {
        //var lastDropdownMenuFilter = document.getElementById("DropdownButtonComponentMenuId_" + DropdownMenuComponent.LastDropdownMenuFilterId);
        //if (!AppTool.IsNullOrEmpty(lastDropdownMenuFilter)) {
        //    lastDropdownMenuFilter.style.display = 'none';
        //}
    }
    DropdownMenuButtonClick(event) {
        this.DropdowndisplayToggle(event);
        this.DropdownMenuButtonClicked.emit(event);

    }
    DropdowndisplayToggle(event) {


        //MouseEvent

        DropdownMenuComponent.LastDropdownMenuFilterId = this.MyDropdownMenuFilterId;
        if (this._DropdownDisplay == 'none') {
            var item = document.getElementById(this._DropdownMenuComponentId);
            var itemRect = item.getBoundingClientRect();
            let myTop = itemRect.top;
            let myleft = itemRect.left;
            if (!AppTool.IsNullOrEmpty(event)) {
                myleft = event.clientX;//: 19
                myTop = event.clientY;//: 19
                // event.stopPropagation();
            }

            document.getElementById(this._DropdownMenuComponentMenuId).style.top =
                (myTop/*itemRect.top*/ /*+ 27*/ /*-5*/) + 'px';

            let DDLHeight = 65 + 20;//    height: 22px; * 3 +30 
            let Extra = 22 + 1 + 1; //    height: 22px; +1 UP +1 DOWN 
            if (itemRect.bottom + DDLHeight + Extra > this.getScreenHeight()) {//this.PaintTop = true                
                document.getElementById(this._DropdownMenuComponentMenuId).style.top =
                    (itemRect.top - DDLHeight - Extra) + 'px';
            }
            document.getElementById(this._DropdownMenuComponentMenuId).style.left =
                (myleft/*itemRect.left*/ /*- 50*/ - 100 /*+5*/) + 'px';//min-width: 80px
            this._DropdownDisplay = 'block';
        } else {
            this._DropdownDisplay = 'none';
        }
        this._CD.detectChanges();
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
}
