import { Component, Inject, Input } from '@angular/core';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
    selector: 'message-window',
    templateUrl: './MessageWindowComponent.html',
    styleUrls: ['./MessageWindowComponent.css']
})
export class MessageWindowComponent {

    message = "";
    date: Date = null;
    description = "";
    title = "Message";
    constructor(@Inject(MAT_DIALOG_DATA) public data: any) {
        this.date = data?.date || '';
        this.description = data?.description;
        this.title = data?.title; 
    }

}
