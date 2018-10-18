
import { Component, Output, EventEmitter, OnInit, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TaxDeductionReportPM } from '../../../EntityPMs/TaxDeductionReportPM';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';


@Component({
    moduleId: module.id,
    templateUrl: './TaxDeductionReportLogTabComponent.html'
})

export class TaxDeductionReportLogTabComponent extends BaseComponent {

   public DataContext: any = this;
    ObjectTableName: string = "TaxDeductionReport";
    entityPM: TaxDeductionReportPM;
    isRTL: boolean = false;
    showLocals: boolean = false;
    constructor(private entityArgs: EntityArgs) {
        super();
        this.entityPM = entityArgs.EntityPM;
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.showLocals = !SessionLocator.LoggedUserPM.DontShowLocal;
        this.UIProperties.SetEnabled("ErrorMessage","TaxDeductionReport",false)
    }

    get ErrorMessage() { return this.entityPM.ErrorMessage; }
    

}
