import { Component, Output, EventEmitter  } from "@angular/core";

@Component({
    selector: 'log-close-save-buttons',
    template: `
         <div class='button-wrapper'>
            <button class="Button" (click)="close.emit(false)">{{'General.B.Close' | TextCodeTranslationPipe}}</button>
            <button class="Button RedButton" (click)="close.emit(true)">{{'General.B.Save' | TextCodeTranslationPipe}}</button>
        </div>   
    `,
    styles: [`
        .button-wrapper {
            direction: ltr;
        }

        .Button {
            display: inline-block;
            width: 50px;
            margin: 0 5px;
        }

    `],
})
export class CloseSaveButtonsComponent {
    @Output() close: EventEmitter<boolean> = new EventEmitter();    
}