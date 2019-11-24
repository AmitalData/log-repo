import {Component} from '@angular/core';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {RatesTablePM} from '../../../../Infrastructure/EntityPMs/RatesTablePM';
import {RatesTablePMService} from '../../../../Infrastructure/Services/StandardPMs/RatesTablePMService';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {CurrencyRatesService, LastRate} from '../../../../Common/Services/CurrencyRatesService';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';

@Component({
    moduleId: module.id,
    templateUrl: './UpdateCurrencyRateComponent.html',
})

export class UpdateCurrencyRateComponent extends BaseComponent {
    public EntityPM: RatesTablePM;   
    public WarningErrorsList: string[] = [];
    public ValidationErrorsList: string[] = [];
    public ObjectTableName = "RatesTable";
    public DataContext = this;
    public RatesList: LastRate[] = [];
    private myService: RatesTablePMService;
    public IsResourcesReady: boolean = false;
    public isRTL: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityResourceService: EntityResourceService) {
        super();
        if (ObjectsLocator.GlobalSetting) {
            this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        }
        this.EntityPM = new RatesTablePM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.BaseCurrencyId = SessionLocator.TenantPM.CurrencyId;
        this.EntityPM.LogDateTime = DateTool.GetCurrentDateTimeAsUtc();
        this.EntityPM.ValueDate = DateTool.GetCurrentDateAsUtc();
        this.myService = new RatesTablePMService();
    }

    public CurrencyId: string = null;
    public CurrencyCode: string = null;
    private OldRate: number = null;
    private LoadDate: Date = null;
    SetWindowArgs(args: any) {
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res: any) => {
            this.IsResourcesReady = true;

            this.OldRate = args['Rate'];
            this.LoadDate = args['Date'];
            this.CurrencyId = args['CurrencyId'];
            this.CurrencyCode = args['CurrencyCode'];

            this.EntityPM.ForeignCurrencyId = this.CurrencyId;
            this.EntityPM.ForeignCurrencyCode = this.CurrencyCode
            this.EntityPM.Rate = this.OldRate;

            this.SetIsEditingEnabled();
        });
    }

    public IsEditingEnabled: boolean = true;
    SetIsEditingEnabled() {
        var isEditingEnabled = true;

        if (SessionLocator.Tenant == 65) {
            isEditingEnabled = false;

            if (SessionLocator.LoggedUserPM.IsCustomerCare) {
                isEditingEnabled = true;
            }
        }

        this.IsEditingEnabled = isEditingEnabled;
    }

    get RateDate() { return this.EntityPM.ValueDate; }
    get LogDateTime() { return this.EntityPM.LogDateTime; }

    get Rate() { return this.EntityPM.Rate; }
    set Rate(newValue: number) {
        if (this.EntityPM.Rate != newValue) {
            this.EntityPM.Rate = AppTool.Round(newValue, 5);
            this.ValidateRateWarningMethod();
        }
    }

    ValidateRateWarningMethod() {
        var warnings: string[] = [];

        if (this.Rate != 0 && this.Rate != null && this.OldRate != null) {
            var acceptRatio = 0.05;

            var rr = Math.abs(this.OldRate - this.Rate) / this.OldRate;
            if (rr > acceptRatio) {
                warnings.push(TextCodeTranslator.Translate("RatesTable.M.DefirenceIsMoreThan"));
            }
        }

        this.WarningErrorsList = warnings;
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        if (AppTool.IsNullOrZero(this.Rate)) {
            var msg = TextCodeTranslator.Translate("General.M.FieldIsRequired");
            errors.push(msg.replace("%FieldName", TextCodeTranslator.Translate("RatesTable.F.Rate")));
        }

        this.ValidationErrorsList = errors;

        if (errors.length == 0) {
            var confirmWindow = new ConfirmWindow();
            confirmWindow.Show(TextCodeTranslator.Translate("RatesTable.M.ChangingTheExchangeRate") + this.Rate);
            confirmWindow.WindowClosed.subscribe((event: any) => {
                if (confirmWindow.Yes) {
                    this.SubmitChanges();
                }
            }); 
        }
    }

    SubmitChanges() {
        this.CurrentSession.StartBusyIndicatorSaving();

        this.myService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
            if (myResponse.HasError) {
                this.ValidationErrorsList = myResponse.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }

            else {
                this.LoadRates();
            }
        });
    }

    private myCurrencyRatesService: CurrencyRatesService;
    LoadRates() {
        if (this.myCurrencyRatesService == null) {
            this.myCurrencyRatesService = new CurrencyRatesService();
        }

        var loadingDate = this.LoadDate;
        if (loadingDate == null) {
            loadingDate = DateTool.GetCurrentDateAsUtc();
        }

        this.myCurrencyRatesService.GetCurrenciesExchangeRateByValueDate(SessionLocator.AccountingCurrencyId, loadingDate).subscribe((myResponse: ServiceResponse) => {
            if (myResponse.HasError) {
                this.ValidationErrorsList = myResponse.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }

            else {
                this.RatesList = myResponse.Result;        
                this.CurrentSession.CloseCurrentWindowEmit("OK");
            }
        });
    }
}
