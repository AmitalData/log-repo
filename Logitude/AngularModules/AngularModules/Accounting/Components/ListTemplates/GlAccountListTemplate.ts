import { SessionLocator } from './../../../Infrastructure/Utilities/SessionLocator';
import {Component,ChangeDetectorRef} from '@angular/core';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';

@Component({

    templateUrl: "./GlAccountListTemplate.html"
})
export class GlAccountListTemplate {
    public rowData: any;
    public fieldName: any;
    public AdditionalData: any;
    public isRTL: boolean = false;
    public showLocal: boolean = !SessionLocator.LoggedUserPM.DontShowLocal;
    private CurrentSession = SessionLocator.SelectedSession;

    constructor(private CD: ChangeDetectorRef) {
        if (ObjectsLocator.GlobalSetting)
            this.isRTL = ObjectsLocator.GlobalSetting.LayoutDirection == "rtl";
    }

    setVariables(rowData: any, fieldName: string, MyAdditionalData: any) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.AdditionalData = MyAdditionalData;

        var isDestroyed: boolean = this.CD["destroyed"];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    }

}
