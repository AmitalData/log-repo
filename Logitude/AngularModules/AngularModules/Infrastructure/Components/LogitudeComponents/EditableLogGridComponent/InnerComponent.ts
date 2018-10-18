import {Component, OnInit, EventEmitter, Output} from '@angular/core';

@Component({
    selector: 'inner-component',
    template: ` 
               <input tabindex="0" type="text" [ngStyle]="customewidth" id="inner-input-control"  (blur)="onblur()" [(ngModel)] = "rowData[fieldName]">
              `, 
    
    inputs: ['width', 'fieldName', 'rowData', 'type', 'src','Id']
})

export class InnerComponent implements OnInit {

  
    public width: any;
    public customewidth: {};
    public rowData: any; 
    public fieldName: any;
    public type: string;
    public src: string;
    public Id: string;
    @Output() blurevent = new EventEmitter();
    @Output() changeevent = new EventEmitter();
    public element: any; 

    ngOnInit() {
        this.customewidth = { "width": this.width}
        this.element = document.getElementById("inner-input-control");
        this.element.focus(); 
    }
    onblur() {
        this.blurevent.emit("");
    }
    onchange() {
        this.changeevent.emit(this.element.value);
    }
    //(change)="onchange()"
    
}