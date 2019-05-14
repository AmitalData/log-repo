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
    public DataContext = this;
    public TenantPM: TenantPM;
    public ObjectTableName: string = "Tenant";
    public ItemsSource: CurrencyRatesModelData[] = [];
    public CurrencyFieldIsEnabled: boolean = true;
    public RatesListVisibility: boolean = false;
    public ValidationErrorsList: string[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();

    }

    SetWindowArgs(args: any) {

        this.TenantPM = args["EntityPM"];

        var entitiesCount: number = args["ShipmentsQuotesCount"];
        if (entitiesCount > 0) {
            this.CurrencyFieldIsEnabled = false;
            this.UIProperties.SetEnabled("CurrencyId", "Tenant", this.CurrencyFieldIsEnabled);
        }
    }

    get CurrencyId() { return this.TenantPM.CurrencyId; }
    set CurrencyId(value: string) {
        if (this.TenantPM.CurrencyId != value) {
            this.TenantPM.CurrencyId = value;
            this.RatesListVisibility = true;
            this.BuildRatesList();
        }
    }

    BuildRatesList() {

        var myService: CurrencyRatesService = new CurrencyRatesService();

        myService.GetRatesByValueDate(this.TenantPM.CurrencyId, DateTool.GetCurrentDateAsUtc()).subscribe((myResponse: ServiceResponse) => {

            this.ItemsSource = [];

            if (myResponse.HasError) {
                this.ValidationErrorsList = myResponse.ErrorsArray;
            }

            else {
                myResponse.Result.forEach(item => {
                    this.ItemsSource.push(new CurrencyRatesModelData(item, this.CurrencyFieldIsEnabled));
                });
            }            
        });
    }





    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

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
    public RatesTablePM: RatesTablePM;
    constructor(entityPM: RatesTablePM, RateIsEnabled: boolean) {
        super();
        this.RatesTablePM = entityPM;
        this.UIProperties.SetEnabled("Rate", "RatesTable", RateIsEnabled);
    }

    get ForeignCurrencyId() { return this.RatesTablePM.ForeignCurrencyId; }
    get Code() { return this.RatesTablePM.ForeignCurrencyCode; }
    get ValueDate() { return this.RatesTablePM.ValueDate; }

    get Rate() { return this.RatesTablePM.Rate; }
    set Rate(value: number) {
        if (this.RatesTablePM.Rate != value) {
            this.RatesTablePM.Rate = value;
        }
    }
}
