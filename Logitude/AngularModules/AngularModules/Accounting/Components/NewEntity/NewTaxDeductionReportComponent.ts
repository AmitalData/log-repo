import { Component } from '@angular/core';
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
import { MessageWindow } from 'Controls/Windows/MessageWindow';

@Component({
    selector: 'NewTaxDeductionReportComponent',
    templateUrl: './NewTaxDeductionReportComponent.html',
})
export class NewTaxDeductionReportComponent extends BaseComponent {
    ObjectTableName: string = 'TaxDeductionReport';
    DataContext: any = this;
    enterdEmail: string;
    entityPM: TaxDeductionReportPM = new TaxDeductionReportPM();
    TaxDeductionReportPMService: TaxDeductionReportPMService =
        new TaxDeductionReportPMService();
    public TenantPM: TenantPM;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

        var date = new Date();
        this.entityPM.TaxYear = date.getFullYear();
        this.entityPM.Tenant = SessionLocator.Tenant;
        this.entityPM.Email = this.enterdEmail =
            SessionLocator.LoggedUserPM.Email;
        this.BuildMonthList();
    }
    public readonly TimePeriod = {
        Year: "Year",
        Periodic: "Periodic",
        Month: "Month",
    } as const;

    public MonthsList: CodeNameClass[];
    BuildMonthList() {
        this.MonthsList = [];
        this.MonthsList.push(new CodeNameClass('1', 'January'));
        this.MonthsList.push(new CodeNameClass('2', 'February'));

        this.MonthsList.push(new CodeNameClass('3', 'March'));
        this.MonthsList.push(new CodeNameClass('4', 'April'));
        this.MonthsList.push(new CodeNameClass('5', 'May'));
        this.MonthsList.push(new CodeNameClass('6', 'June'));
        this.MonthsList.push(new CodeNameClass('7', 'July'));
        this.MonthsList.push(new CodeNameClass('8', 'August'));
        this.MonthsList.push(new CodeNameClass('9', 'September'));
        this.MonthsList.push(new CodeNameClass('10', 'October'));
        this.MonthsList.push(new CodeNameClass('11', 'November'));
        this.MonthsList.push(new CodeNameClass('12', 'December'));
        this.SelectedFromMonth = this.MonthsList[0];
        this.SelectedMonth = this.MonthsList[0];
    }
    private selectedMonth: CodeNameClass;
    get SelectedMonth() {
        return this.selectedMonth;
    }
    set SelectedMonth(value: CodeNameClass) {
        if (this.selectedMonth != value) {
            this.selectedMonth = value;
            if (value != null) {
                this.UIProperties.SetRequired(
                    'Month',
                    this.ObjectTableName,
                    false
                );
                this.entityPM.Month = new Date();
                this.entityPM.Month.setDate(1);
                this.entityPM.Month.setMonth(+value.Code - 1);
            } else {
                this.UIProperties.SetRequired(
                    'Month',
                    this.ObjectTableName,
                    true
                );
            }
        }
    }

    private selectedFromMonth: CodeNameClass;
    get SelectedFromMonth() {
        return this.selectedFromMonth;
    }
    set SelectedFromMonth(value: CodeNameClass) {
        if (this.selectedFromMonth != value) {
            this.selectedFromMonth = value;
            if (value != null) {
                this.entityPM.FromMonth = new Date();
                this.entityPM.FromMonth.setDate(1);
                this.entityPM.FromMonth.setMonth(+value.Code - 1);
            }
            this.UIProperties.SetRequired('FromMonth', this.ObjectTableName, AppTool.IsNullOrEmpty(value));
        }
    }

    public FilterSelectedValue: string = this.TimePeriod.Year;
    FilterItemClicked(itemValue: string) {
        if (this.FilterSelectedValue != itemValue) {
            this.FilterSelectedValue = itemValue;

            this.ByMonth = itemValue == this.TimePeriod.Month || itemValue == this.TimePeriod.Periodic;
            this.SetEmailValue();
        }
    }


    private SetEmailValue(): void {
        if (!AppTool.IsNullOrEmpty(this.entityPM.Email)) {
            this.enterdEmail = this.entityPM.Email;
        }
        this.entityPM.Email =
                this.FilterSelectedValue === this.TimePeriod.Month ? ' ' : this.enterdEmail;
    }

    get Email() {
        return this.entityPM.Email;
    }
    set Email(value: string) {
        if (this.entityPM.Email != value) {
            this.entityPM.Email = value;
        }
    }

    get Month() {
        return this.entityPM.Month;
    }
    set Month(value: Date) {
        if (this.entityPM.Month != value) {
            this.entityPM.Month = value;
        }
    }

    get ByMonth() {
        return this.entityPM.ByMonth;
    }
    set ByMonth(value: boolean) {
        if (this.entityPM.ByMonth != value) {
            this.entityPM.ByMonth = value;
        }
    }

    get TaxYear() {
        return this.entityPM.TaxYear;
    }
    set TaxYear(value: number) {
        if (this.entityPM.TaxYear != value) {
            this.entityPM.TaxYear = value;
        }
    }

    get IsAdditionalReportExist() {
        return this.entityPM.IsAdditionalReportExist;
    }
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
        this.FIELD_IS_REQUIERD = TextCodeTranslator.Translate(
            'General.M.FieldIsRequired'
        );
        Validator.TryValidateObject(
            this.entityPM,
            this.ObjectTableName,
            errors
        );

        if (AppTool.IsNullOrEmpty(this.entityPM.TaxYear)) {
            var s: string = this.FIELD_IS_REQUIERD.replace(
                '%FieldName',
                TextCodeTranslator.Translate('TaxDeductionReport.F.TaxYear')
            );

            errors.push(s);
        }
        if (
            this.entityPM.ByMonth &&
            AppTool.IsNullOrEmpty(this.entityPM.Month)
        ) {
            var s: string = this.FIELD_IS_REQUIERD.replace(
                '%FieldName',
                TextCodeTranslator.Translate('TaxDeductionReport.F.Month')
            );

            errors.push(s);
        }


        if (this.FilterSelectedValue == this.TimePeriod.Periodic &&
            this.entityPM.Month.getMonth() <= this.entityPM.FromMonth.getMonth()) {
            errors.push(TextCodeTranslator.Translate('TaxDeductionReport.O.InvalidMonthPeriod'));
        }
        this.entityPM.FromMonth.setFullYear(this.entityPM.TaxYear);
        this.entityPM.Month.setFullYear(this.entityPM.TaxYear);
        this.ValidationErrorsList = errors;

        if (this.ValidationErrorsList.length == 0) {
            if (this.FilterSelectedValue !== this.TimePeriod.Periodic) {
                this.entityPM.FromMonth = null;
            }

            this.CurrentSession.StartBusyIndicator('');
            this.TaxDeductionReportPMService.insert(this.entityPM).subscribe(
                (myResult: any) => {
                    var mm: ServiceResponse = myResult;
                    if (!mm.HasError) {
                        var entity = mm.Result;

                        this.CurrentSession.CloseCurrentWindowEmit('ok');
                        this.CurrentSession.StopBusyIndicator();
                        var messageWindow= new MessageWindow()
                        messageWindow.Show(TextCodeTranslator.Translate("TaxDeductionReport.O.ReportGenerationMessage"));

                        setTimeout(() => {
                          messageWindow.Close();  
                        }, 2000);

                    } else {
                        this.ValidationErrorsList = mm.ErrorsArray;
                        this.CurrentSession.StopBusyIndicator();
                    }
                }
            );
        }
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
}
