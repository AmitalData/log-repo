declare var System: any; 
import {Component, ElementRef, OnInit, EventEmitter, Output, Injector} from '@angular/core';

@Component({
    moduleId: module.id,

    selector: 'edit-list-template',
    //template: `<div style="{{customestyle}}" tabindex={{temp}} (click)="onclick()" (keyup)="keyupHandler($event)"> 
    //           <span><span style="text-overflow: ellipsis" (click)="onclick()" *ngIf="noComponent && showtext">{{rowData[fieldName]}}</span></span>
    //           <img style="vertical-align: middle;" tabindex=1 *ngIf="showimg" src="{{src}}" />
               
    //           <inner-component  style="width:85%" [rowData]="rowData" [fieldName]="fieldName" (blurevent)="handleonblur()" *ngIf="!noComponent && showtext"></inner-component>
    //           </div>`,
    templateUrl:'./EditableListTemplateComponent.html',
    //directives: [InnerComponent, InnerSpanComponent],
    inputs: ['fieldName', 'rowData', 'editable', 'type', 'src', 'event', 'parentId', 'Alignment', 'required', 'ComponentName', 'ComponentUrl', 'ObjectTableName', 'format', 'allowtomove', 'width', 'Id','TabIndex'],
    //host: {'(blur)': 'onBlur($event)'}
})
    
export class EditableListTemplateComponent implements OnInit {

    public rowData: any;
    public Id: string;
    public editable: boolean;
    public fieldName: string;
    public ObjectTableName: any;
    public customestyle: {};
    public type: string;
    public src: string;
    public showimg: boolean;
    public showtext: boolean;
    public showbtn: boolean;
    public showlookup: boolean;
    public event: EventEmitter<any>;
    public parentId: string;
    public Alignment: string;
    public required: boolean;
    public temp: any;
    public ComponentUrl: string;
    public ComponentName: string;
    public Data: any;
    public format: string;
    public allowtomove: boolean;
    public width: number;
    public TabIndex: number;
    public Pipe: string;
    @Output() clickevent = new EventEmitter();  
    @Output() onblurEvent = new EventEmitter();
    constructor(public _injector: Injector, public _elementRef: ElementRef) {
        this.noComponent = true; 
    }
    onclick() {
        if (this.editable == true) {
            this.noComponent = false; 
        } 
    }

    onblurevt() {
        this.onblurEvent.emit("");
    }

    handleonblur() {
        this.noComponent = true;
        this.onblurEvent.emit(this.parentId);
    }
    
    public noComponent: boolean;
   

    ngOnInit() {
        //if (this.format != undefined) {
        //    var numafterdot = this.format[1];
        //    var number = this.rowData[this.fieldName].toFixed(numafterdot);
        //    this.Data = number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        //}
        //else {
        //    this.Data = this.rowData[this.fieldName];
        //}
        //var temp = this.fieldName.split(':');
        //if (temp.length == 2) {
        //    this.fieldName = temp[0];
        //    this.Pipe = temp[1];
        //}

        var BackGroundColor = "transparent"
        
        if (this.editable == false) {
            this.temp = "-1";
            BackGroundColor = "rgba(230, 231, 232, 0.5)";
            this.customestyle = "overflow: hidden; text-overflow: ellipsis; background-color: rgba(230, 231, 232, 0.5);"
        }
        else {
            this.temp = "0";
            this.customestyle = "overflow: hidden; text-overflow: ellipsis; background: transparent none repeat scroll 0 0;";
        }
        if (this.Alignment != undefined) {
            this.customestyle = {
                "overflow": "hidden", "text-overflow": "ellipsis", "height": "25px", "text-align": this.Alignment, "background-color": BackGroundColor
            }; 
        }
        else {
            this.customestyle = { "overflow": "hidden", "text-overflow": "ellipsis", "height": "25px", "text-align": "right", "background-color": BackGroundColor}; 
        }
        this.event.subscribe((res) => {
            if (res == this.parentId)
            this.onclick();
        });
        if (this.TabIndex == -1) {
            this.temp = "-1";
        }
       
        
        if (this.type == "Image") {
            this.showimg = true;
            this.showtext = false;
            this.showbtn = false;
        } else if (this.type == "lookup") {
            this.showlookup = true;
            this.showimg = false;
            this.showtext = false;
            this.showbtn = false;
        }
        else if (this.type == "IconButton") {
            this.showimg = false;
            this.showtext = false;
            this.showbtn = true;
        }
        else {
            this.showimg = false;
            this.showtext = true;
            this.showbtn = false;
        } 
    }
    
}