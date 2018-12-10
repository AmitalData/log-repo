import {Component, ChangeDetectorRef, OnDestroy} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { OpenFormatReportPM } from '../../../EntityPMs/OpenFormatReportPM';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';




@Component({
    selector: 'OpenFormatReportLogTabComponent',
    moduleId: module.id,
    templateUrl: './OpenFormatReportLogTabComponent.html',
})


export class OpenFormatReportLogTabComponent extends BaseComponent{
  public DataContext: any = this;
    ObjectTableName: string = "OpenFormatReport";
    entityPM: OpenFormatReportPM;
    isRTL: boolean = false;
    showLocals: boolean = false;
    constructor(private entityArgs: EntityArgs) {
        super();
        this.entityPM = entityArgs.EntityPM;
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.showLocals = !SessionLocator.LoggedUserPM.DontShowLocal;
        this.UIProperties.SetEnabled("ErrorMessage","OpenFormatReport",false)
    }

    get ErrorMessage() { return this.entityPM.ErrorMessage; }
    
}
