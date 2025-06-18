import { Component} from '@angular/core';
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
import { TaxDeductionReportData  } from '../../../DataContracts/TaxDeductionReportData';


@Component({
    
    templateUrl: './TaxDeductionReportGeneralTabCompletedComponent.html'
})

export class TaxDeductionReportGeneralTabCompletedComponent extends BaseComponent {

    public DataContext: any = this;
    ObjectTableName: string = "TaxDeductionReport";
    DataObjectTableName: string = "TaxDeductionReportData";
    public entityPM: TaxDeductionReportPM;
    isRTL: boolean = false;
    showLocals: boolean = false;
    taxDeductionReportExtendedPMService: TaxDeductionReportExtendedPMService = new TaxDeductionReportExtendedPMService();
    _BatchTaskExecutionListService: BatchTaskExecutionListService = new BatchTaskExecutionListService();
    Faild: boolean = false;
    taxDeductionReportPMService: TaxDeductionReportPMService = new TaxDeductionReportPMService();
   
    private CurrentSession = SessionLocator.SelectedSession;
    _TaxDeductionReportData: TaxDeductionReportData;
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
        this. _TaxDeductionReportData = new TaxDeductionReportData();
        //this.BuildTaxDeductionReportData();
    }

    Building: boolean= false;
    get IsAdditionalReportExist() { return this.entityPM.IsAdditionalReportExist; }



    get ReportType() { 
            return this.entityPM.ByMonth === false
                        ? TextCodeTranslator.Translate("TaxDeductionReport.O.ByYear")
                        : this.entityPM.FromMonth === this.entityPM.Month
                            ? TextCodeTranslator.Translate("TaxDeductionReport.O.ByMonth")
                            :  TextCodeTranslator.Translate("TaxDeductionReport.O.Periodic") ;
    }


    private FormatDateToMonthYear(dateInput: Date | string): string {
        const date = new Date(dateInput);
        if (isNaN(date.getTime())) {
            throw new Error("Invalid date input");
        }
        const month = (date.getMonth() + 1).toString().padStart(2, '0');
        const year = date.getFullYear();
        return `${month}/${year}`;
    }


    get ReportPeriod() {
        return this.entityPM.ByMonth === false
            ? this.entityPM.TaxYear.toString()
            : this.entityPM.FromMonth === this.entityPM.Month
                ? `${TextCodeTranslator.Translate("TaxDeductionReport.F.Month")} ${this.FormatDateToMonthYear(this.entityPM.Month)}`
                : `${TextCodeTranslator.Translate("TaxDeductionReport.F.FromMonth")} ` +
                  `${this.FormatDateToMonthYear(this.entityPM.FromMonth)} ` +
                  `${TextCodeTranslator.Translate("Accounting.General.O.To")} ` +
                  `${TextCodeTranslator.Translate("TaxDeductionReport.O.ToMonth")} ` +
                  `${this.FormatDateToMonthYear(this.entityPM.Month)}`;
    }



    get Email() { return this.entityPM?.Email ?? ''; }

    BuildTaxDeductionReportData() {

        this.taxDeductionReportExtendedPMService.GetTaxDeductionReportData(this.entityPM.Id).subscribe((myResponse: ServiceResponse) => {

            if (myResponse) {
                if (!myResponse.HasError) {
                        this._TaxDeductionReportData = myResponse.Result;

                }
            }

        });

    }
  
}

