import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TenantPM} from '../../../../Common/EntityPMs/TenantPM';
import {RatesTablePM} from '../../../../Infrastructure/EntityPMs/RatesTablePM';
import {CurrencyRatesService, LastRate} from '../../../../Common/Services/CurrencyRatesService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import {RatesTablePMService} from '../../../../Infrastructure/Services/StandardPMs/RatesTablePMService';

@Component({
    selector: 'CurrencyRatesComponent',
    moduleId: module.id,
    templateUrl: './CurrencyRatesComponent.html',
})

export class CurrencyRatesComponent extends BaseComponent{

    public DataContext: CurrencyRatesComponent = this;
    public ObjectTableName: string = "Tenant";
    public TenantPM: TenantPM;
    public ItemsSource: CurrencyRatesModelData[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        this.TenantPM = SessionLocator.TenantPM;
        //this.BuildRatesList();
    }

    SetWindowArgs(args: any) {
        this.CurrencyFieldIsEnabled = args["CurrencyFieldIsEnabled"];
        this.currencyId = args["CurrencyId"];
        this.UIProperties.SetEnabled("CurrencyId", "Tenant", this.CurrencyFieldIsEnabled);
    }

    public CurrencyFieldIsEnabled: boolean = false;

    BuildRatesList() {
        var list: LastRate[] = new Array<LastRate>();
        var myService: CurrencyRatesService = new CurrencyRatesService();
        this.ItemsSource = [];
        var date: Date = DateTool.GetCurrentDateAsUtc();
        myService.GetRatesByValueDate(this.TenantPM.CurrencyId, date).subscribe(resp => {
            var result: ServiceResponse = resp;
            if (!result.HasError) {
                result.Result.forEach(item => {
                    var itemData: CurrencyRatesModelData = new CurrencyRatesModelData(item, this.CurrencyFieldIsEnabled);
                    this.ItemsSource.push(itemData);
                });
            }
            else {
                var errors = result.ErrorsArray;
            }
        });
    }

    // Props
    private currencyId: string = null;
    get CurrencyId() {
        return this.currencyId;
    }
    set CurrencyId(value: string) {
        if (this.currencyId != value) {
            this.currencyId = value;
            this.RatesListVisibility = true;
            this.BuildRatesList();
        }
    }

    private ratesListVisibility: boolean = false;
    get RatesListVisibility() {
        return this.ratesListVisibility;
    }
    set RatesListVisibility(value: boolean) {
        if (this.ratesListVisibility != value) {
            this.ratesListVisibility = value;
        }
    }

    //Commands
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    public ValidationErrorsList: string[];
    public list: RatesTablePM[] = [];

    OkButtonClicked() {
        var errors: string[] = [];

        this.ItemsSource.forEach(item => {
            Validator.TryValidateObject(item,"RatesTable", errors);
            if (item.Rate == null) {
                errors.push("Rate field for currency " + item.Code + " is required");
            }

            if (errors.length == 0) {
                var rate = new RatesTablePM();
                rate.Tenant = this.TenantPM.Id;
                rate.BaseCurrencyId = this.TenantPM.CurrencyId;
                rate.ForeignCurrencyId = item.ForeignCurrencyId;
                rate.LogDateTime = DateTool.GetCurrentDateTimeAsUtc();
                rate.ValueDate = DateTool.GetCurrentDateTimeAsUtc();
                rate.Rate = item.Rate;
                this.list.push(rate);
            }

        });

        this.ValidationErrorsList = errors;
        if (this.ValidationErrorsList.length == 0) {
            this.SubmitChanges();
        }
    }

    SubmitChanges() {
        this.CurrentSession.StartBusyIndicator("Saving...");
        var myService: CurrencyRatesService = new CurrencyRatesService();

        var ratesTables: LastRate[] = [];
        this.list.forEach(item => {
            var rate = new LastRate();
            rate.Tenant = item.Tenant;
            rate.BaseCurrencyId = item.BaseCurrencyId;
            rate.ForeignCurrencyId = item.ForeignCurrencyId;
            rate.LogDateTime = item.LogDateTime;
            rate.ValueDate = item.ValueDate;
            rate.Rate = item.Rate;
            ratesTables.push(rate);
        });

        myService.InsertListOfRatesTable(ratesTables).subscribe((myResponse: ServiceResponse) => {
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

        //this.list.forEach(item => {
        //    myService.insert(item).subscribe((myResponse: ServiceResponse) => {
        //        if (myResponse != null) {
        //            if (!myResponse.HasError) {
        //                this.CurrentSession.CloseCurrentWindowEmit("ok");
        //            }

        //            else {
        //                this.ValidationErrorsList = myResponse.ErrorsArray;
        //                this.CurrentSession.StopBusyIndicator();
        //            }
        //        }
        //    });
        //});
    }
}

export class CurrencyRatesModelData extends BaseComponent {

    public RatesTablePM: RatesTablePM = new RatesTablePM();


    constructor(entityPM: RatesTablePM, RateIsEnabled: boolean) {
        super();
        this.RatesTablePM = entityPM;
        this.UIProperties.SetEnabled("Rate", "RatesTable", RateIsEnabled);
    }

    // Props
    get ForeignCurrencyId() {
        return this.RatesTablePM.ForeignCurrencyId;
    }

    get Code() {
        return this.RatesTablePM.ForeignCurrencyCode;
    }
    get ValueDate() {
        return this.RatesTablePM.ValueDate;
    }

    get Rate() {
        return this.RatesTablePM.Rate;
    }
    set Rate(value: number) {
        this.RatesTablePM.Rate = value;
    }
}
