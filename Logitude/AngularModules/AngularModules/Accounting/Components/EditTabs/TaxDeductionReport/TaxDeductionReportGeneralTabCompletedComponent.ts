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
    failed: boolean = false;
    taxDeductionReportPMService: TaxDeductionReportPMService = new TaxDeductionReportPMService();
    firstTotalPayments?: number;
    firstTotalDeductions?: number;

    _TaxDeductionReportData: TaxDeductionReportData;
    constructor(private entityArgs: EntityArgs) {
        super();
        this.entityPM = entityArgs.EntityPM;
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.showLocals = !SessionLocator.LoggedUserPM.DontShowLocal;
        this.UIProperties.SetEnabled("IsAdditionalReportExist", "TaxDeductionReport", false);
        this.UIProperties.SetEnabled("Email", "TaxDeductionReport", false);
      
        if (this.entityPM.StatusTypeCode == "4") {
            this.failed = true;
        }
        this._TaxDeductionReportData = new TaxDeductionReportData();
        this.BuildTaxDeductionReportData();

    }

    Building: boolean= false;
    get IsAdditionalReportExist() { return this.entityPM.IsAdditionalReportExist; }



    get ReportType(): string {
        if (!this.entityPM) return '';
        if (!this.entityPM.ByMonth) return TextCodeTranslator.Translate("TaxDeductionReport.O.ByYear");
        if (this.entityPM.FromMonth === this.entityPM.Month) return TextCodeTranslator.Translate("TaxDeductionReport.O.ByMonth");
        return TextCodeTranslator.Translate("TaxDeductionReport.O.Periodic");
    }


    private FormatDateToMonthYear(dateInput: Date | string): string {
        const date = new Date(dateInput);
        if (isNaN(date.getTime())) {
            console.error("Invalid date input in FormatDateToMonthYear:", dateInput);
            return '';
        }
        return `${(date.getMonth() + 1).toString().padStart(2, '0')}/${date.getFullYear()}`;
    }


    get ReportPeriod(): string {
        if (!this.entityPM) return '';


        if (!this.entityPM.ByMonth) {
            return this.entityPM.TaxYear.toString();
        }

        if (this.entityPM.FromMonth === this.entityPM.Month || !this.entityPM.FromMonth) {
            return `${TextCodeTranslator.Translate("TaxDeductionReport.F.Month")} ${this.FormatDateToMonthYear(this.entityPM.Month)}`;
        }

        return `${TextCodeTranslator.Translate("TaxDeductionReport.F.FromMonth")} ` +
            `${this.FormatDateToMonthYear(this.entityPM.FromMonth)} ` +
            `${TextCodeTranslator.Translate("Accounting.General.O.To")} ` +
            `${TextCodeTranslator.Translate("TaxDeductionReport.O.ToMonth")} ` +
            `${this.FormatDateToMonthYear(this.entityPM.Month)}`;
    }

    get Email() { return this.entityPM?.Email ?? ''; }

    BuildTaxDeductionReportData() {
        this.taxDeductionReportExtendedPMService
            .GetTaxDeductionReportData(this.entityPM.Id)
            .subscribe({
            next: (response: ServiceResponse) => {
                if (!response?.HasError) {
                    this._TaxDeductionReportData = response.Result;


                    this.firstTotalPayments = this._TaxDeductionReportData?.TotalForCompany &&
                        this._TaxDeductionReportData.TotalForCompany[0]
                        ? this._TaxDeductionReportData.TotalForCompany[0].TotalPayments
                        : undefined;

                    this.firstTotalDeductions = this._TaxDeductionReportData?.TotalForCompany &&
                        this._TaxDeductionReportData.TotalForCompany[0]
                        ? this._TaxDeductionReportData.TotalForCompany[0].TotalDeductions
                        : undefined;
                }
            },
            error: err => {
            console.error("Failed to load TaxDeductionReportData", err);
            }
        });

    }
  
}

