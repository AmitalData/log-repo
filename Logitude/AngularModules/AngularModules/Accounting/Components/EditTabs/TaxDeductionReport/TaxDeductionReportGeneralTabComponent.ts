import { Component, Output, EventEmitter, OnInit, AfterViewInit, ChangeDetectorRef } from '@angular/core';
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


@Component({
    moduleId: module.id,
    templateUrl: './TaxDeductionReportGeneralTabComponent.html'
})

export class TaxDeductionReportGeneralTabComponent extends BaseComponent {

    public DataContext: any = this;
    ObjectTableName: string = "TaxDeductionReport";
    entityPM: TaxDeductionReportPM;
    isRTL: boolean = false;
    showLocals: boolean = false;
    taxDeductionReportExtendedPMService: TaxDeductionReportExtendedPMService = new TaxDeductionReportExtendedPMService();
    _BatchTaskExecutionListService: BatchTaskExecutionListService = new BatchTaskExecutionListService();
    Faild: boolean = false;
    taxDeductionReportPMService: TaxDeductionReportPMService = new TaxDeductionReportPMService();
    
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs) {
        super();
        this.entityPM = entityArgs.EntityPM;
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.showLocals = !SessionLocator.LoggedUserPM.DontShowLocal;
        this.UIProperties.SetEnabled("IsAdditionalReportExist", "TaxDeductionReport", false);
        this.UIProperties.SetEnabled("Email", "TaxDeductionReport", false);

        if (this.entityPM.StatusTypeCode == "4") {
            this.Faild = true;
        }
    }

    Building: boolean= false;
    get IsAdditionalReportExist() { return this.entityPM.IsAdditionalReportExist; }

    get Email() { return this.entityPM.Email; }
    RunService() {
        this.entityPM.StatusTypeCode = "2";
        this.Building=true;
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("Accounting.General.O.Saving"));
        this.taxDeductionReportPMService.update(this.entityPM).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                 
                    this.CurrentSession.StopBusyIndicator();
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    this.taxDeductionReportExtendedPMService.DownloadTaxDeduction856FileInBatch(this.entityPM).subscribe(myResult => {
                        var mm: ServiceResponse = myResult;
                        var entity = mm.Result;



                    });
                }

                else {
                  
                    this.CurrentSession.StopBusyIndicator();
                }
            }
        });
        


    }

  
}

