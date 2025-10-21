import { Component, EventEmitter, Output, Input, OnInit, ElementRef } from '@angular/core';
import { AppTool } from '../../../Infrastructure/Tools';
import { CommunicationLogStepListService } from '../../../Common/Services/ExtendedLists/CommunicationLogStepListService';
import { TextCodeTranslator } from              '../../../Infrastructure/Utilities/TextCodeTranslator';


@Component({
    selector: 'dropdown-button',
    
    host: {
        '(document:click)': 'handleClick($event)',
    },
    templateUrl:'DropdownButtonComponent.html',
})

export class DropdownButtonComponent implements OnInit {
    
    
    @Input()
    public IsDisabled: boolean
    @Input()
    public Dropdownbutton_Text: string = "Show Dropdown Content";
    @Input()
    public dataCy: string; // Optional stable selector for e2e tests
    
    _Dropdownbutton_TextCode: string;
    @Input()
    public get Dropdownbutton_TextCode() { return this._Dropdownbutton_TextCode; }
    public set Dropdownbutton_TextCode(val: string) {
        if (!AppTool.IsNullOrEmpty(val) && val != this._Dropdownbutton_TextCode) {
            this._Dropdownbutton_TextCode = val;
            this.Dropdownbutton_Text = TextCodeTranslator.Translate(val);
        }
    }
    
    public _DropdownDisplay: string = 'none';
    private _ElementRef: any;

    static MyId: number = 0;
    public _DropdownButtonComponentId: string;
    public _DropdownButtonComponentMenuId: string;
    constructor(myElement: ElementRef) {
        this._ElementRef = myElement;
        var curId = DropdownButtonComponent.MyId++;
        this._DropdownButtonComponentId = "DropdownButtonComponent_" + curId;
        this._DropdownButtonComponentMenuId = "DropdownButtonComponentMenuId_" + curId;
    }
    
    
  
    
  
    handleClick(event) {
        var clickedComponent = event.target;
        var inside = false;
        let conter = 0;
        do {
            if (clickedComponent === this._ElementRef.nativeElement) {
                inside = true;
                break;
            }
            if (clickedComponent.class === "class-dropdown-content") {
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
            
            if (this._DropdownDisplay == 'block') {
                this.dropdowndisplayToggle();
            }

        }
    }
    ngOnInit() {
    }

    DropdownDisplayClose() {
        this._DropdownDisplay = 'none';
    }
    
    Width = -60;
    Height = -40;
    dropdowndisplayToggle() {
        if (this._DropdownDisplay == 'none') {
            var item = document.getElementById(this._DropdownButtonComponentId);
            var itemRect = item.getBoundingClientRect();
            document.getElementById(this._DropdownButtonComponentMenuId).style.top =
                (itemRect.top + 27) + 'px';
            document.getElementById(this._DropdownButtonComponentMenuId).style.left =
                (itemRect.left - 50) + 'px';//min-width: 80px
            this._DropdownDisplay = 'block';
        } else {
            this._DropdownDisplay = 'none';
        }

    }


}