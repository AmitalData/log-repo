import { Component, EventEmitter, Input, Output } from "@angular/core";

@Component({
    selector: "FooterButtons",
    templateUrl: "./FooterButtonsComponent.html"
})

export class FooterButtonsComponent {

    @Input() SaveButtonLabel: string = "Save";
    @Input() CancelButtonLabel: string = "Cancel";
    @Input() SaveButtonDataCy: string = "save-button";
    @Input() CancelButtonDataCy: string = "cancel-button";
    @Input() IsSaveButtonDisabled: boolean = false;
    @Input() IsCancelButtonDisabled: boolean = false;
    @Input() ShowSaveOnly: boolean = false;

    @Output() SaveButtonClicked = new EventEmitter();
    @Output() CancelButtonClicked = new EventEmitter();

    saveButtonClicked() {
        this.SaveButtonClicked.emit();
    }

    cancelButtonClicked() {
        this.CancelButtonClicked.emit();
    }
}