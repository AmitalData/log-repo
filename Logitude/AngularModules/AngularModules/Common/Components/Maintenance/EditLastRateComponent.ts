import { Component } from '@angular/core';
import { Validator } from '../../../Infrastructure/Validators/Validator';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { RatesItem } from './RatesMainTabComponent';
import { AppTool, DateTool } from '../../../Infrastructure/Tools';
import { RatesTablePMService } from '../../../Infrastructure/Services/StandardPMs/RatesTablePMService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { RatesTablePM } from '../../../Infrastructure/EntityPMs/RatesTablePM';
import { TenantPM } from '../../EntityPMs/TenantPM';
import { CurrencyRatesService, LastRate } from '../../../Common/Services/CurrencyRatesService';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { RatesTableExtendedService } from 'Infrastructure/Services/ExtendedPMs/RatesTableExtendedService';
import { AdditionalCurrencyRateList } from 'Infrastructure/EntityLists/AdditionalCurrencyRateList';
import { CurrencyRatePM } from 'Infrastructure/EntityPMs/CurrencyRatePM';

@Component({

    templateUrl: './EditLastRateComponent.html',
})

export class EditLastRateComponent extends BaseComponent {
    public ObjectTableName: string = "RatesTable";
    public DataContext = this;
    public ValidationErrorsList: string[] = [];
    public WarningsList: string[] = [];
    public IsEditingEnabled: boolean = false;
    public TodayDate: Date = DateTool.GetCurrentDateAsUtc();
    public RatesTable: RatesTablePM = new RatesTablePM();
    public TenantPM: TenantPM;
    public EntityPM: LastRate;
    public CurrentRate: number;
    public CurrentValueDate: Date;
    private CurrentSession = SessionLocator.SelectedSession;
    IsAccountingActivated: boolean = false;
    public AdditionalCurrencyRateTypes: AdditionalCurrencyRateList[] = [];
    public CurrencyRates = {};
    public InvalidRates: { [key: string]: boolean } = {}; 
    public RateFieldIsInValid: boolean = false;

    constructor() {
        super();
        this.TenantPM = SessionLocator.TenantPM;
        this.IsAccountingActivated = SessionLocator.TenantPM.AccountingActivated;
    }

    CreateRatesTablePM() {
        this.RatesTable = new RatesTablePM();
        this.RatesTable.Tenant = this.TenantPM.Id;
        this.RatesTable.BaseCurrencyId = this.TenantPM.CurrencyId;
        this.RatesTable.ForeignCurrencyId = this.EntityPM.ForeignCurrencyId;
        if (this.IsAccountingActivated) {
            this.RatesTable.Unit = this.EntityPM.Unit;
        }
        this.RatesTable.LogDateTime = DateTool.GetCurrentDateAsUtc();

        const currentCurrencyRates = this.EntityPM.CurrencyRates || [];
        this.CurrencyRates = {};
        this.AdditionalCurrencyRateTypes.forEach(additionalCurrencyRateType => {

            const currentCurrencyRate = currentCurrencyRates.filter(currencyRate => currencyRate.AdditionalCurrencyRateId == additionalCurrencyRateType.Id);

            this.CurrencyRates[additionalCurrencyRateType.Id] = {
                Rate: null,
                OldRate: currentCurrencyRate?.length > 0 ? currentCurrencyRate[0].Rate : null,
                RateCoefficient: additionalCurrencyRateType.RateCoefficient,
                RatePercent: this.convertToPercentage(additionalCurrencyRateType.RateCoefficient)
            }
        });
    }

    convertToPercentage(decimal) {
        if (!decimal) {
            return decimal;
        }
        let percentage = (decimal >= 1) ? (decimal - 1) * 100 : (1 - decimal) * 100;
        let sign = (decimal >= 1) ? "+" : "-";
        return sign + percentage.toFixed(0) + "%";
    }

    private OldRate: number = null;
    SetDataContext(dataContext: RatesItem) {
        this.IsEditingEnabled = dataContext.IsEditingEnabled;
        this.EntityPM = dataContext.LastRate;
        this.OldRate = this.EntityPM.Rate;
        this.CurrentRate = dataContext.CurrentRate;
        this.CurrentValueDate = dataContext.CurrentValueDate;
        this.CreateRatesTablePM();
    }

    SetWindowArgs(args: any) {
        if (args != null) {
            this.AdditionalCurrencyRateTypes = args.CurrencyRateTypes;
        }
    }

    get Unit() {

        if (this.RatesTable.Unit == null || this.RatesTable.Unit <= 0) {
            return 1;
        }
        return this.RatesTable.Unit;

    }

    get Rate() { return this.RatesTable.Rate; }
    set Rate(value: number) {
        if (this.RatesTable.Rate != value) {

            for (var id in this.CurrencyRates) {
                if (this.CurrencyRates[id].RateCoefficient) {
                    this.CurrencyRates[id].Rate = AppTool.Round(value * this.CurrencyRates[id].RateCoefficient, 5);
                }
            }
        }
    }
    OnRateChange(value) {
        this.Rate = value;

    }
    SetAdditionalRate(id, value) {
        if (this.CurrencyRates[id].Rate! = Number(value.target.value)) {
            this.CurrencyRates[id].Rate = Number(value.target.value);

            this.ValidateRateWarningMethod(this.CurrencyRates[id].Rate, this.CurrencyRates[id].OldRate, id);
        }

    }
    OnRateBlur(value) {
        if (this.RatesTable.Rate != value.target.value) {
            this.RatesTable.Rate = AppTool.Round(value.target.value, 5);
            this.ValidateRateWarningMethod(this.Rate, this.OldRate, null);

            for (var id in this.CurrencyRates) {

                if (this.CurrencyRates[id].RateCoefficient) {
                    this.CurrencyRates[id].Rate = AppTool.Round(this.RatesTable.Rate * this.CurrencyRates[id].RateCoefficient, 5);
                }
            }
        }
    }
    ValidateRateWarningMethod(rate, oldRate, id) {
        var warnings: string[] = [];
        if (id === null) {
            this.InvalidRates = {};
            this.RateFieldIsInValid = false;

        }
        else {
            this.InvalidRates[id] = false;
        }
        if (rate != 0 && rate != null && oldRate != null) {
            var acceptRatio = 0.05;

            var rr = Math.abs(oldRate - rate) / oldRate;
            if (rr > acceptRatio) {
                warnings.push("Difference between new and old value is more than 0.05");
                if (id === null) {
                    for (var i in this.CurrencyRates) {
                        this.InvalidRates[i] = true;
                    }
                    this.RateFieldIsInValid = true;
                }
                else {
                    this.InvalidRates[id] = true;

                }

            }

        }

        this.WarningsList = warnings;
    }

    get ValueDate() { return this.RatesTable.ValueDate; }
    set ValueDate(value: Date) {
        if (this.RatesTable.ValueDate != value) {
            this.RatesTable.ValueDate = value;
        }
    }

    SetAsToday() {
        this.ValueDate = DateTool.GetCurrentDateAsUtc();
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.RatesTable, this.ObjectTableName, errors);

        var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
        if (AppTool.IsNullOrZero(this.Rate)) {
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("RatesTable.F.Rate")));
        }

        for (var id in this.CurrencyRates) {
            if (!this.CurrencyRates[id].Rate) {
                const additionalCurrencyRateType = this.AdditionalCurrencyRateTypes.filter(additionalCurrencyRateType => additionalCurrencyRateType.Id == id);
                errors.push(msg.replace("%FieldName", additionalCurrencyRateType.length ? additionalCurrencyRateType[0].Name : "שער"));
            }
        }

        if (errors.length == 0) {
            if (this.ValueDate == null) {
                errors.push("Date is required");
            }

            else {
                var valueDate = new Date(this.ValueDate.valueOf()).valueOf();
                var today = DateTool.GetCurrentDateAsUtc().valueOf();
                if (valueDate > today) {
                    errors.push("Cant add future date rate");
                }
            }
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Show("Please confirm changing the exchange rate to " + this.Rate);
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.SubmitChanges();
                }
            });
        }
    }

    SubmitChanges() {

        this.CurrentSession.StartBusyIndicatorSaving();

        let CurrencyRates: CurrencyRatePM[] = [];
        for (var id in this.CurrencyRates) {
            let currencyRate = new CurrencyRatePM();
            currencyRate.AdditionalCurrencyRateId = id;
            currencyRate.Rate = this.CurrencyRates[id].Rate;
            currencyRate.Tenant = this.EntityPM.Tenant;
            CurrencyRates.push(currencyRate);
        }

        var myService: RatesTableExtendedService = new RatesTableExtendedService();
        myService.UpdateRate(this.RatesTable, CurrencyRates).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    this.CurrentSession.CloseCurrentWindowEmit("ok");
                }

                else {
                    this.ValidationErrorsList = myResponse.ErrorsArray;
                    this.CurrentSession.StopBusyIndicator();
                }
            }
        });
    }
}
