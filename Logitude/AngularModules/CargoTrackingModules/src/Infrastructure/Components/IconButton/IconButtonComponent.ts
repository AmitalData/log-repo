import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
    selector: 'IconButton',
    templateUrl: './IconButtonComponent.html',
    styleUrls: ['./IconButtonComponent.css']
})
export class IconButtonComponent implements OnInit
{

    @Input() IconName: string;
    @Output() public IconClick: EventEmitter<any> = new EventEmitter<any>();


    constructor() { }

    ngOnInit(): void
    {
    }

    IconClicked(){
        this.IconClick.emit();
    }

}
