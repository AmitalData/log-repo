import { Component, Inject, Input } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

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
    showOkButton: boolean = false;
    showCancelButton: boolean = false;
    showTextBox: boolean = false;


    private _TextBoxValue : string;
    public get TextBoxValue() : string {
        return this._TextBoxValue;
    }
    public set TextBoxValue(v : string) {
        this._TextBoxValue = v;
    }


    constructor(@Inject(MAT_DIALOG_DATA) public data: any,
        public dialogRef: MatDialogRef<MessageWindowComponent>)
    {
        this.date = data?.date || '';
        this.description = data?.description;
        this.showOkButton = data?.showOkButton;
        this.showCancelButton = data?.showCancelButton;
        this.showTextBox = data?.showTextBox;
        this.title = data?.title != null ? data?.title : this.title;
    }


    OkButtonClicked(){
        this.dialogRef.close({button: 'ok', textValue: this.TextBoxValue});
    }

    CancelButtonClicked(){
        this.dialogRef.close({button: 'cancel', textValue: this.TextBoxValue});
    }

}
