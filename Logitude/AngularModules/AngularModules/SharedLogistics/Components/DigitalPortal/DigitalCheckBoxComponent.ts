import { Component, ChangeDetectorRef } from '@angular/core';

@Component({
    selector: 'DigitalCheckBoxComponent',
    templateUrl: './DigitalCheckBoxComponent.html',
})

export class DigitalCheckBoxComponent {

    public rowData: any;
    public fieldName: any;
    public AdditionalData: any;
    public fieldValue: any;

    constructor(private CD: ChangeDetectorRef) {
    }

    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
        this.fieldName = fieldName;

        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    }

}
