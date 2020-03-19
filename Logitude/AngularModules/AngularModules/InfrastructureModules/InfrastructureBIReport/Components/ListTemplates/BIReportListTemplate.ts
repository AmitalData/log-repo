
import { Component, ChangeDetectorRef } from '@angular/core';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    moduleId: module.id,
    templateUrl: './BIReportListTemplate.html',
})

export class BIReportListTemplate {

    public rowData: any;
    public fieldName: any;
    public AdditionalData: any;
    public IsTenantZero: any;

    public isRTL: boolean = false;
    public showLocal: boolean = false;

 
    constructor(private CD: ChangeDetectorRef) {
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
            this.showLocal = !SessionLocator.LoggedUserPM.DontShowLocal;
        if (SessionLocator.Tenant == 0)
            this.IsTenantZero = true;
    }

    setVariables(rowData: any, fieldName: string, MyAdditionalData: any) {
        this.rowData = rowData;
        this.AdditionalData = MyAdditionalData;
        this.fieldName = fieldName;

        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }



    }
}
