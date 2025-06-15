import { Component, Output, EventEmitter, OnInit, AfterViewInit, ChangeDetectorRef, ViewChild } from '@angular/core';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TaxDeductionReportPM } from '../../../EntityPMs/TaxDeductionReportPM';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TaxDeductionReportExtendedPMService } from '../../../Services/ExtendedPMs/TaxDeductionReportExtendedPMService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { BatchTaskExecutionListService } from '../../../../Infrastructure/Services/StandardLists/BatchTaskExecutionListService';
import { TaxDeductionReportPMService } from '../../../Services/StandardPMs/TaxDeductionReportPMService';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { ChildDirective } from '../../../../Infrastructure/Directives/ChildDirective';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';


@Component({
    
    templateUrl: './TaxDeductionReportGeneralTabComponent.html'
})

export class TaxDeductionReportGeneralTabComponent implements AfterViewInit {

    public DataContext: any = this;
    public ObjectTableName: string = "TaxDeductionReport";
    public EntityPM: TaxDeductionReportPM;

    @ViewChild(ChildDirective) Child: ChildDirective;

    isRTL: boolean = false;


  constructor(private entityArgs: EntityArgs, private entityResourceService: EntityResourceService) {
    if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    this.EntityPM = entityArgs.EntityPM;
  }


  ngAfterViewInit() {
    let COMPLETED = "3";
    if (this.EntityPM.StatusTypeCode === COMPLETED) {
        SessionLocator.DynamicLoader.Load("./Accounting/Components/EditTabs/TaxDeductionReport/TaxDeductionReportGeneralTabCompleted", this.Child.Location)
            .then(cmpRef => {
            });
    }
    else {
        SessionLocator.DynamicLoader.Load("./Accounting/Components/EditTabs/TaxDeductionReport/TaxDeductionReportGeneralTabNotCompleted", this.Child.Location)
            .then(cmpRef => {
            });
    }
  }
}



