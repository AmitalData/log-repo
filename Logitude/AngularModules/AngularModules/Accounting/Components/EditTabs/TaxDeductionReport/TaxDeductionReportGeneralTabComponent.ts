import { Component, AfterViewInit, ViewChild } from '@angular/core';
import { TaxDeductionReportPM } from '../../../EntityPMs/TaxDeductionReportPM';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
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
    const enum TaxDeductionStatus {
      Completed = '3'
    }

    if (this.EntityPM.StatusTypeCode === TaxDeductionStatus.Completed){
        SessionLocator.DynamicLoader.Load("./Accounting/Components/EditTabs/TaxDeductionReport/TaxDeductionReportGeneralTabCompletedComponent", this.Child.Location);
    }
    else {
        SessionLocator.DynamicLoader.Load("./Accounting/Components/EditTabs/TaxDeductionReport/TaxDeductionReportGeneralTabNotCompletedComponent", this.Child.Location);
    }
  }
}



