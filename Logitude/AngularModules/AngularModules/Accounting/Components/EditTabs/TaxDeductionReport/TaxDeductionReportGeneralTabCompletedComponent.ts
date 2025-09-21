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
import { interval, Subscription } from 'rxjs';
import { switchMap } from 'rxjs/operators';


@Component({
    
    templateUrl: './TaxDeductionReportGeneralTabCompletedComponent.html'
})

export class TaxDeductionReportGeneralTabCompletedComponent extends BaseComponent {

    public DataContext: any = this;
    ObjectTableName: string = "TaxDeductionReport";
    DataObjectTableName: string = "TaxDeductionReportData";
    public entityPM: TaxDeductionReportPM;
    private CurrentSession = SessionLocator.SelectedSession;

    isRTL: boolean = false;
    showLocals: boolean = false;
    taxDeductionReportExtendedPMService: TaxDeductionReportExtendedPMService = new TaxDeductionReportExtendedPMService();
    _BatchTaskExecutionListService: BatchTaskExecutionListService = new BatchTaskExecutionListService();
    failed: boolean = false;
    taxDeductionReportPMService: TaxDeductionReportPMService = new TaxDeductionReportPMService();
    firstTotalPayments?: number;
    firstTotalDeductions?: number;
    TaxDeductionStatus = TaxDeductionStatus; 
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
        this.startWatching();
    }
    ngOnDestroy() {
        this.watcher?.unsubscribe();
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

    RunService() {
        this.entityPM.StatusTypeCode = this.TaxDeductionStatus.InProgress;
        this.Building=true;
        this.CurrentSession.StartBusyIndicator(TextCodeTranslator.Translate("Accounting.General.O.Saving"));
        this.taxDeductionReportPMService.update(this.entityPM).subscribe({
            next: (response: ServiceResponse) => {
            if (!response?.HasError) {
                this.CurrentSession.StopBusyIndicator();
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                
                this.startWatching();
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

    private watcher?: Subscription;

    startWatching() {
        
        if(this.entityPM.StatusTypeCode === TaxDeductionStatus.InProgress){
            this.watcher = interval(1000) 
          .pipe(
            switchMap(() => this.taxDeductionReportPMService.get(this.entityPM.Id))
          )
          .subscribe({
            next: res => {
                if (!res?.HasError) {
                    if (res.Result.StatusTypeCode !== TaxDeductionStatus.InProgress){
                        this.watcher?.unsubscribe();
                        this.entityPM = res.Result;
                        this.BuildTaxDeductionReportData();
                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    }                   
                   
                }                           
               
            },
            error: err => {
              console.error('Error checking report status', err);
            }
          });
        }
        
      }
    

}

export enum TaxDeductionStatus {
    Completed = '3'
    , Failed = '4'
    , InProgress = '2'
  }