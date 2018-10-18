import {Component, OnInit, EventEmitter, Output} from '@angular/core';

@Component({
    selector: 'inner-span-component',
    template: ` 
               <span *ngIf="format" id="inner-span-id" style="text-overflow: ellipsis;padding-left: 3px;padding-right: 5px;" (click)="onclick()" >{{Data | NumbersPipe:format}}</span>
               <span *ngIf="!format" id="inner-span-id" style="text-overflow: ellipsis;padding-left: 3px;padding-right: 5px;" (click)="onclick()" >{{Data}}</span>
              `, 
    
    inputs: ['Data', 'fieldName', 'rowData', 'type', 'format']
})

export class InnerSpanComponent implements OnInit {

  
    public Data: any;
    public rowData: any; 
    public fieldName: any;
    public type: string;
    public format: string = null;
    @Output() spanclickevent = new EventEmitter(); 
    public element: any; 

    ngOnInit() {
        var x = this.format;
        //if (this.format != undefined) {
        //    if (this.format[0] == "n") {
        //        var numafterdot = this.format[1];
        //        var tempo = this.rowData[this.fieldName];
        //        var number = parseFloat(tempo).toFixed(parseInt(numafterdot));
        //        this.Data = number.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        //    }
        //}
        //else {
            this.Data = this.rowData[this.fieldName];
        //}
        //this.element = document.getElementById("inner-input-control");
        //this.element.value(); 
    }
    onclick() {
        this.spanclickevent.emit("");
    }
     
     
    
}