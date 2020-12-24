
import { Component, DoCheck } from '@angular/core';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TaxDeductionReportPM } from '../../EntityPMs/TaxDeductionReportPM';
import { TaxDeductionReportPMService } from '../../Services/StandardPMs/TaxDeductionReportPMService';
import { TenantPM } from '../../../Common/EntityPMs/TenantPM';
import { CodeNameClass } from '../../../Infrastructure/DataContracts/CodeNameClass';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { AppTool } from '../../../Infrastructure/Tools';

@Component({
    selector: 'NewTaxDeductionReportComponent',
    templateUrl: './NewTaxDeductionReportComponent.html',
})

export class NewTaxDeductionReportComponent extends BaseComponent implements DoCheck{


    ObjectTableName: string = "TaxDeductionReport";
    DataContext: any = this;
    FilterSelectedOldValue: string;
    entityPM: TaxDeductionReportPM = new TaxDeductionReportPM();
    TaxDeductionReportPMService: TaxDeductionReportPMService = new TaxDeductionReportPMService();
    public TenantPM: TenantPM;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

        var date = new Date();
        this.entityPM.TaxYear = date.getFullYear();
        this.entityPM.Tenant = SessionLocator.Tenant;
        this.entityPM.Email = SessionLocator.LoggedUserPM.Email;
        this.BuildMonthList();
        this.FilterSelectedOldValue = this.FilterSelectedValue;

    }

    ngDoCheck(): void {
        if (this.FilterSelectedOldValue != this.FilterSelectedValue) {
            this.entityPM.Email = this.FilterSelectedValue == "Month" ? " " : SessionLocator.LoggedUserPM.Email;
            this.FilterSelectedOldValue = this.FilterSelectedValue;
            this.SetUIProperties();
        }
    }

    SetUIProperties() {
        this.UIProperties.SetEnabled("Email", this.ObjectTableName, this.ByMonth ? false : true);
    }
   
 public MonthsList: CodeNameClass[];
    BuildMonthList() {

        this.MonthsList = [];
        this.MonthsList.push(new CodeNameClass("1", "January"));
        this.MonthsList.push(new CodeNameClass("2", "February"));

        this.MonthsList.push(new CodeNameClass("3", "March"));
        this.MonthsList.push(new CodeNameClass("4", "April"));
        this.MonthsList.push(new CodeNameClass("5", "May"));
        this.MonthsList.push(new CodeNameClass("6", "June"));
        this.MonthsList.push(new CodeNameClass("7", "July"));
        this.MonthsList.push(new CodeNameClass("8", "August"));
        this.MonthsList.push(new CodeNameClass("9", "September"));
        this.MonthsList.push(new CodeNameClass("10", "October"));
        this.MonthsList.push(new CodeNameClass("11", "November"));
        this.MonthsList.push(new CodeNameClass("12", "December"));
   this.SelectedMonth=   this.MonthsList[0];

    }
private selectedMonth: CodeNameClass;
    get SelectedMonth() { return this.selectedMonth; }
    set SelectedMonth(value: CodeNameClass) {
        if (this.selectedMonth != value) {
            this.selectedMonth = value;
            if (value != null) {
                this.UIProperties.SetRequired("Month", this.ObjectTableName, false);
                this.entityPM.Month = new Date();
                this.entityPM.Month.setMonth(+value.Code-1);
                
            }
            else {
                this.UIProperties.SetRequired("Month", this.ObjectTableName, true);
            }

        }
    }

    
 public FilterSelectedValue: string = 'Year';
  FilterItemClicked(itemValue: string) {
    if (this.FilterSelectedValue != itemValue) {
      this.FilterSelectedValue = itemValue;
        if (itemValue == "Month") {
            this.ByMonth = true;
        }
        else {
            this.ByMonth = false;
        }
    }
    }

    get Email() { return this.entityPM.Email; }
    set Email(value: string) {
        if (this.entityPM.Email != value) {
            this.entityPM.Email = value;
        }
    }

  get Month() { return this.entityPM.Month; }
    set Month(value: Date) {
        if (this.entityPM.Month != value) {
            this.entityPM.Month = value;
        }
    }


 get ByMonth() { return this.entityPM.ByMonth; }
    set ByMonth(value: boolean) {
        if (this.entityPM.ByMonth != value) {
            this.entityPM.ByMonth = value;
        }
    }

    get TaxYear() { return this.entityPM.TaxYear; }
    set TaxYear(value: number) {
        if (this.entityPM.TaxYear != value) {
            this.entityPM.TaxYear = value;
        }
    }

    get IsAdditionalReportExist() { return this.entityPM.IsAdditionalReportExist; }
    set IsAdditionalReportExist(value: boolean) {
        if (this.entityPM.IsAdditionalReportExist != value) {
            this.entityPM.IsAdditionalReportExist = value;
        }
    }

    FIELD_IS_REQUIERD: string = null;
    ValidationErrorsList: string[] = [];
    OkButtonClicked() {
        this.entityPM.CreateDate = new Date();
        this.entityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.entityPM.UpdateDate = new Date();
        this.entityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        var errors: string[] = [];
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        Validator.TryValidateObject(this.entityPM, this.ObjectTableName, errors);

        if ( AppTool.IsNullOrEmpty(this.entityPM.TaxYear)) {
            var s: string = this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("TaxDeductionReport.F.TaxYear"));

            errors.push(s);
        }
      if (this.entityPM.ByMonth && AppTool.IsNullOrEmpty(this.entityPM.Month)) {
            var s: string = this.FIELD_IS_REQUIERD.replace("%FieldName", TextCodeTranslator.Translate("TaxDeductionReport.F.Month"));

            errors.push(s);
        }
       

        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            this.CurrentSession.StartBusyIndicator("");
            this.TaxDeductionReportPMService.insert(this.entityPM).subscribe((myResult:any) => {

                var mm: ServiceResponse = myResult;
                if (!mm.HasError) {
                    var entity = mm.Result;

                    this.CurrentSession.CloseCurrentWindowEmit("ok");

                    SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent',
                        this.CurrentSession.SessionLocation.viewContainerRef)
                        .then(cmpRef => {
                            cmpRef.instance.ComponentRef = cmpRef;
                            cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: this.ObjectTableName });
                            cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                                this.CancelButtonClicked();
                            });
                        });
                    this.CurrentSession.StopBusyIndicator();
                }

                else {
                    this.ValidationErrorsList = mm.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }
            });
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();

    }

}
