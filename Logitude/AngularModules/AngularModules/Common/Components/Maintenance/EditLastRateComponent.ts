import {Component} from '@angular/core';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {RatesItem} from './RatesMainTabComponent';
import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import {RatesTablePMService} from '../../../Infrastructure/Services/StandardPMs/RatesTablePMService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {RatesTablePM} from '../../../Infrastructure/EntityPMs/RatesTablePM';
import {TenantPM} from '../../EntityPMs/TenantPM';
import {LastRate} from '../../../Common/Services/CurrencyRatesService';
import {ConfirmWindow} from '../../../Controls/Windows/ConfirmWindow';

@Component({
    moduleId: module.id,
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
    constructor() {
        super();
        this.TenantPM = SessionLocator.TenantPM;
    }

    CreateRatesTablePM() {
        this.RatesTable = new RatesTablePM();
        this.RatesTable.Tenant = this.TenantPM.Id;
        this.RatesTable.BaseCurrencyId = this.TenantPM.CurrencyId;
        this.RatesTable.ForeignCurrencyId = this.EntityPM.ForeignCurrencyId;
        this.RatesTable.LogDateTime = DateTool.GetCurrentDateAsUtc();      
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

    get Rate() { return this.RatesTable.Rate; }
    set Rate(value: number) {
        if (this.RatesTable.Rate != value) {
            this.RatesTable.Rate = AppTool.Round(value, 5);
            this.ValidateRateWarningMethod();
        }
    }

    ValidateRateWarningMethod() {
        var warnings: string[] = [];

        if (this.Rate != null && this.OldRate != null) {
            var acceptRatio = 0.05;

            var rr = Math.abs(this.OldRate - this.Rate) / this.OldRate;
            if (rr > acceptRatio) {
                warnings.push("Difference between new and old value is more than 0.05");
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

        var myService: RatesTablePMService = new RatesTablePMService();
        myService.insert(this.RatesTable).subscribe((myResponse: ServiceResponse) => {
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
