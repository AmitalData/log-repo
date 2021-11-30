declare var window: any;
import {Component,ChangeDetectorRef} from '@angular/core'; 
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';

@Component({  
    templateUrl: './MultiUpdateCheckTemplate.html',
})

export class MultiUpdateCheckTemplate {
    private CurrentSession = SessionLocator.SelectedSession;
    public UpdateSuccess: any;
    public fieldName: any;
    public AdditionalData: any;

    public isRTL: boolean = false;


    constructor(private CD: ChangeDetectorRef) {
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    }

    setVariables(rowData: any, fieldName: string, MyAdditionalData: any) {        
        this.fieldName = fieldName;
        this.AdditionalData = MyAdditionalData;

        var record = window.AllRecords?.filter(item=> item.Id == rowData.Id)[0];
        this.UpdateSuccess = record?.UpdateSuccess;
        
        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    }

   

}