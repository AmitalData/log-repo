
import {Component,ChangeDetectorRef} from '@angular/core';

import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    templateUrl: './InvoiceSequenceListTemplate.html',
})

export class InvoiceSequenceListTemplate {

    public rowData: any;
    public fieldName: any;
    public AdditionalData: any;
    public fieldValue: any;

    public isRTL: boolean = false;


    constructor(private CD: ChangeDetectorRef) {
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    }

    setVariables(rowData: any, fieldName: string, MyAdditionalData: any) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.AdditionalData = MyAdditionalData;

        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    }

   


}
