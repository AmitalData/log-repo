import { Component, EventEmitter, Output, Input, OnInit, ElementRef, ChangeDetectorRef, ContentChild, AfterContentChecked, SimpleChanges } from '@angular/core';
import { AppTool } from '../../Tools';
import { LogLovV2Component } from './LogLovV2Component';
import { UserList } from '../../../Common/EntityLists/UserList';

@Component({
    selector: 'MultiSelectLOV',
    host: { '(document:click)': 'handleClick($event)', },
    templateUrl: 'MultiSelectLOVComponent.html',
})

export class MultiSelectLOVComponent implements OnInit, AfterContentChecked {

    @Input()
    public IsDisabled: boolean
    //@Output()
    //public DropdownMenuButtonClicked: EventEmitter<any> = new EventEmitter<any>();

    //private _CustomSendOptionsArgs: CustomSendOptionsArgs;
    public _DropdownDisplay: string = 'none';
    private _ElementRef: any;
    static MyId: number = 1;
    static LastDropdownMenuFilterId: number = 0;
    public _MultiSelectLOVId: string;
    public _MultiSelectLOVMenuId: string;
    MyDropdownMenuFilterId: number;

    @Input()
    public DataContext: any;
    @Input()
    public LOVListComponentPropName: string = null;
    @Input()
    public PlaceHolder: string = "Multi Select ....";
    @Input()
    LayoutDirection: string = 'rtl'//'ltr';

    @Output()
    ChosenListItemsChanged = new EventEmitter();

    IsDisplayOnly: boolean = false;

    //@ViewChild(LogLovV2Component)
    InitKeyDownEvent: boolean = false;
    @ContentChild(LogLovV2Component)
    private _MyLogLovV2Component: LogLovV2Component = null;
    public get MyLogLovV2Component(): LogLovV2Component {
        if (!this.InitKeyDownEvent && this._MyLogLovV2Component != null) {
            this.InitKeyDownEvent = true;
            this._MyLogLovV2Component.KeyDownEvent
                .subscribe(char => {
                    var TABKEY = 9;
                    var ENTERKEY = 13;
                    var DOWNKEY = 40;
                    var UPKEY = 38;
                    var ESC = 27;
                    var CTRL = 17;
                    var SHIFT = 16;
                    if (char == ENTERKEY && this._MyLogLovV2Component.SelectedItem != null) {
                        this.AddToList();
                    }
                });
        }
        return this._MyLogLovV2Component;
    }
    public set MyLogLovV2Component(value: LogLovV2Component) {
        this._MyLogLovV2Component = value;

    }


    @Input()
    ChosenListHeader: string = 'Chosen List';

    @Input()
    ButtonAddLabel: string = null;

    constructor(private _CD: ChangeDetectorRef, myElement: ElementRef) {
        this._ElementRef = myElement;
        ///this.DataContext = this; 
        //this._CustomSendOptionsArgs = new CustomSendOptionsArgs();
        //this._CustomSendOptionsArgs.ForcePersonalSign = false;
        var curId = MultiSelectLOVComponent.MyId++;
        this.MyDropdownMenuFilterId = curId;
        this._MultiSelectLOVId = "MultiSelectLOV_" + curId;
        this._MultiSelectLOVMenuId = "MultiSelectLOVMenuId_" + curId;

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
            if (clickedComponent.className === this._MultiSelectLOVMenuId) {
                inside = true;
                break;
            }

            if (this.MyLogLovV2Component.ElementId == clickedComponent.id || this.MyLogLovV2Component.DropdownId == clickedComponent.id) {
                inside = true;
                break;
            }
            if (clickedComponent.class === "class-MultiSelectLOV-content") {
                inside = true;
                break;
            }
            if (clickedComponent.class === "myLOV-td") {
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

    ngAfterContentChecked() {
      //  this.FormatList();
        this._CD.detectChanges();
    }

    public Invalidate(): any {

        this.FormatList();
        this._CD.detectChanges();
    }

    addOnBlur() {
        var list: any[] = this.DataContext[this.LOVListComponentPropName];
        if (this.MyLogLovV2Component.SelectedItem == null && list.length == 0) {
            this.DropdownDisplayClose();

        }
    }

    DropdownDisplayClose() {
        this._DropdownDisplay = 'none';
        this._CD.detectChanges();
    }


    Width = -60;
    Height = -40;

    public static EnsureLastDropdownMenuIsClosed() {
        //var lastDropdownMenuFilter = document.getElementById("MultiSelectLOVMenuId_" + MultiSelectLOV.LastDropdownMenuFilterId);
        //if (!AppTool.IsNullOrEmpty(lastDropdownMenuFilter)) {
        //    lastDropdownMenuFilter.style.display = 'none';
        //}
    }

    ChosenFormatedListFocus() {
        this.DropdowndisplayToggle(null, true);
    }
    DropdownMenuButtonClick(event, fucusMe: boolean) {
        //this.CloseOtherLastmenu()
        if (MultiSelectLOVComponent.LastDropdownMenuFilterId != 0 && MultiSelectLOVComponent.LastDropdownMenuFilterId != this.MyDropdownMenuFilterId) {
            var lastSplitButtonComponentMenu = document.getElementById("MultiSelectLOVMenuId_" + MultiSelectLOVComponent.LastDropdownMenuFilterId);
            if (!AppTool.IsNullOrEmpty(lastSplitButtonComponentMenu)) {
                lastSplitButtonComponentMenu.style.display = 'none';
            }
        }
        MultiSelectLOVComponent.LastDropdownMenuFilterId = this.MyDropdownMenuFilterId;

        this.DropdowndisplayToggle(event, fucusMe);
    }

    DropdowndisplayToggle(event, fucusMe: boolean) {


        //MouseEvent

        //MultiSelectLOVComponent.LastDropdownMenuFilterId = this.MyDropdownMenuFilterId;

        if (this._DropdownDisplay == 'none') {
            var item = document.getElementById(this._MultiSelectLOVId);
            var itemRect = item.getBoundingClientRect();
            let myTop = itemRect.top;
            let myleft = itemRect.left;
            let useMouseXY = false;
            if (useMouseXY && !AppTool.IsNullOrEmpty(event)) {
                myleft = event.clientX;//: 19
                myTop = event.clientY;//: 19
                // event.stopPropagation();
            }
            myTop = myTop + 25;
            document.getElementById(this._MultiSelectLOVMenuId).style.top =
                (myTop/*itemRect.top*/ /*+ 27*/ /*-5*/) + 'px';

            let DDLHeight = 65 + 20;//    height: 22px; * 3 +30 
            let Extra = 22 + 1 + 1; //    height: 22px; +1 UP +1 DOWN

            if (itemRect.bottom + DDLHeight + Extra > this.getScreenHeight()) {//this.PaintTop = true
                //to shoe the div Upper the Input due -At the end of screen
                document.getElementById(this._MultiSelectLOVMenuId).style.top =
                    (itemRect.top - DDLHeight - Extra + 25) + 'px';
            }
            let usengStyle = true;
            if (!usengStyle) {
                document.getElementById(this._MultiSelectLOVMenuId).style.left =
                    (myleft/*- 100*/) + 'px';//min-width: 80px
            }
            this._DropdownDisplay = 'block';
            if (fucusMe) {
                this.MyLogLovV2Component.OnToggleClicked();
                this.MyLogLovV2Component.ForceFocus = true;

                setInterval(() => {
                    var input = document.getElementById(this.MyLogLovV2Component.ElementId);
                    if (input) {
                        input.focus();
                    }
                }, 200);


            }

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

    _ChosenFormatedList: String;
    ClearList() {
        console.log("ClearList");
        var list: any[] = this.DataContext[this.LOVListComponentPropName];
        //list = [];//list.forEach(r => { list.pop() });
        while (list.length > 0) {
            list.pop();
        }
        this.FormatList();
    }
    DeleteFromList(item2Del) {
        console.log("DeleteFromList");
        var list: any[] = this.DataContext[this.LOVListComponentPropName];
        var index = list.findIndex(d => d == item2Del);
        if (index > -1) {
            list.splice(index, 1);
        }
        this.FormatList();


    }
    AddToList() {
        //console.log("AddToList");
        if (this.MyLogLovV2Component.SelectedItem == null) {
            return;
        }
        if (AppTool.IsNullOrEmpty(this.DataContext[this.LOVListComponentPropName])) {
            this.DataContext[this.LOVListComponentPropName] = [];
        }
        var list: any[] = this.DataContext[this.LOVListComponentPropName];
        if (list.filter(r => r[this.MyLogLovV2Component.SelectedValuePath] == this.MyLogLovV2Component.SelectedItem[this.MyLogLovV2Component.SelectedValuePath]).length > 0) {
            return;
        }
        list.push(this.MyLogLovV2Component.SelectedItem);
        this.FormatList();
        this.MyLogLovV2Component.OnDeleteValue();
        //this.MyLogLovV2Component.SelectedItem = null;
    }

    FormatList(): any {
        this._ChosenFormatedList = "";
        var list: any[] = this.DataContext[this.LOVListComponentPropName];
        if (list.length > 2) {

            this._ChosenFormatedList = list[0][this.MyLogLovV2Component.DisplayMemberPath] + ','
                + list[1][this.MyLogLovV2Component.DisplayMemberPath] + "+" + (list.length - 2);
        } else {
            list.forEach(item => {
                if (!AppTool.IsNullOrEmpty(this._ChosenFormatedList)) {
                    this._ChosenFormatedList += ','
                }
                this._ChosenFormatedList += item[this.MyLogLovV2Component.DisplayMemberPath];
            });

        }
        this.ChosenListItemsChanged.emit(list);
    }
}
