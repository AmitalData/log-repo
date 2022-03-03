import { Component, Inject, Input } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

const requiredFieldValidationMessage = "זהו שדה נדרש שאינו יכול להיות ריק";
@Component({
    selector: 'message-window',
    templateUrl: './MessageWindowComponent.html',
    styleUrls: ['./MessageWindowComponent.css']
})
export class MessageWindowComponent {

    message = "";
    date: Date = null;
    description = "";
    link = "";
    title = "Message";
    showOkButton: boolean = false;
    showCancelButton: boolean = false;
    showTextBox: boolean = false;
    showMultilineTextBox: boolean = false;
    inputRequired: boolean = false;
    isLoading: boolean = false;
    OkButtonText: string = 'Ok';
    CancelButtonText: string = 'Cancel';
    error: string;

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
        this.OkButtonText = data?.okButtonText || 'Ok';
        this.CancelButtonText = data?.cancelButtonText || 'Cancel';
        this.showTextBox = data?.showTextBox;
        this.inputRequired = data?.inputRequired;
        this.showMultilineTextBox = data?.showMultilineTextBox;
        this.title = data?.title != null ? data?.title : this.title;
        this.link = data?.link;
        this.isLoading = data?.isLoading;
    }


    OkButtonClicked(){
        if(this.inputRequired && !this.TextBoxValue){
            this.error = requiredFieldValidationMessage;
        }else{
            this.dialogRef.close({button: 'ok', textValue: this.TextBoxValue});
        }
    }

    CancelButtonClicked(){
        this.dialogRef.close({button: 'cancel', textValue: this.TextBoxValue});
    }

}
