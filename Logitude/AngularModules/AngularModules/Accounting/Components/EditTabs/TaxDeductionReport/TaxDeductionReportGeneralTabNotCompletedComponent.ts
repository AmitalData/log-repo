import { Component } from '@angular/core';
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
    
    templateUrl: './TaxDeductionReportGeneralTabNotCompletedComponent.html'
})

export class TaxDeductionReportGeneralTabNotCompletedComponent extends BaseComponent {

    private readonly STATUS_TYPE_CODE_CREATED = "1";
    private readonly STATUS_TYPE_CODE_IN_PROGRESS = "2";
    private readonly STATUS_TYPE_CODE_COMPLETED = "3";
    private readonly STATUS_TYPE_CODE_FAILED = "4";


    public DataContext = this;
    ObjectTableName: string = "TaxDeductionReport";
    public entityPM: TaxDeductionReportPM;
    isRTL: boolean = false;
    showLocals: boolean = false;
    taxDeductionReportExtendedPMService: TaxDeductionReportExtendedPMService = new TaxDeductionReportExtendedPMService();
    batchTaskExecutionListService: BatchTaskExecutionListService = new BatchTaskExecutionListService();
    Failed: boolean = false;
    taxDeductionReportPMService: TaxDeductionReportPMService = new TaxDeductionReportPMService();
   
    private CurrentSession = SessionLocator.SelectedSession;



    
    constructor(private entityArgs: EntityArgs) {
        super();
        this.entityPM = entityArgs.EntityPM;
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.showLocals = !SessionLocator.LoggedUserPM.DontShowLocal;
        this.UIProperties.SetEnabled("IsAdditionalReportExist", "TaxDeductionReport", false);
        this.UIProperties.SetEnabled("Email", "TaxDeductionReport", false);
      
        if (this.entityPM.StatusTypeCode === this.STATUS_TYPE_CODE_FAILED) {
            this.Failed = true;
        }
    }

    Building: boolean= false;
    get IsAdditionalReportExist() { return this.entityPM.IsAdditionalReportExist; }

    get Email(): string {
        return this.entityPM?.Email ?? '';
    }
    RunService() {
        this.entityPM.StatusTypeCode = this.STATUS_TYPE_CODE_IN_PROGRESS;
        this.Building=true;
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("Accounting.General.O.Saving"));
        this.taxDeductionReportPMService.update(this.entityPM).subscribe({
            next: (response: ServiceResponse) => {
            if (!response?.HasError) {
                this.CurrentSession.StopBusyIndicator();
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                this.download856File();
            } else {
                this.CurrentSession.StopBusyIndicator();
            }
            },
            error: (err) => {
                console.error('Update failed:', err);
                this.CurrentSession.StopBusyIndicator();
            }
            });


    }
    download856File() {
        this.taxDeductionReportExtendedPMService.DownloadTaxDeduction856FileInBatch(this.entityPM).subscribe((myResult: ServiceResponse) => {
            if (myResult != null) {
                if (!myResult.HasError) {
                    this.CurrentSession.StopBusyIndicator();
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                } else {
                    this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    }

  
}

