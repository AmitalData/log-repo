import {Component} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {TenantPM} from '../../../../Common/EntityPMs/TenantPM';
import {RatesTablePM} from '../../../../Infrastructure/EntityPMs/RatesTablePM';
import { CurrencyRatesService, LastRate, AccountingCurrencyHelper} from '../../../../Common/Services/CurrencyRatesService';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {Validator} from '../../../../Infrastructure/Validators/Validator';
import { InfraSettings } from '../../../../Infrastructure/Utilities/InfraSettings';
import { Cloner } from '../../../../Infrastructure/Utilities/Cloner';

@Component({
    selector: 'CurrencyRatesComponent',
    moduleId: module.id,
    templateUrl: './CurrencyRatesComponent.html',
})

export class CurrencyRatesComponent extends BaseComponent{
    public DataContext = this;
    public EntityPM: TenantPM;
    public ObjectTableName: string = "Tenant";
    public ItemsSource: CurrencyRatesModelData[] = [];
    public CurrencyFieldIsEnabled: boolean = true;
    public RatesListVisibility: boolean = false;
    public ValidationErrorsList: string[];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }

    get CurrencyId() { return this.EntityPM.CurrencyId; }
    set CurrencyId(value: string) {
        if (this.EntityPM.CurrencyId != value) {
            this.EntityPM.CurrencyId = value;
            this.RatesListVisibility = true;
            this.BuildRatesList();
        }
    }

    SetWindowArgs(args: any) {

        this.EntityPM = args["EntityPM"];

        var entitiesCount: number = args["ShipmentsQuotesCount"];
        if (entitiesCount > 0) {
            this.CurrencyFieldIsEnabled = false;
            this.UIProperties.SetEnabled("CurrencyId", "Tenant", this.CurrencyFieldIsEnabled);
        }

        this.Clone();
    }
    BuildRatesList() {

        var myService: CurrencyRatesService = new CurrencyRatesService();

        myService.GetRatesByValueDate(this.CurrencyId, DateTool.GetCurrentDateAsUtc()).subscribe((myResponse: ServiceResponse) => {

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
        this.RejectChanges();
        this.CurrentSession.CloseCurrentWindow();
    }
    OkButtonClicked() {
        if (!this.EntityPM.IsDirty) {
            this.CurrentSession.CloseCurrentWindow();
        }

        else {
            var errors: string[] = [];
            
            this.ItemsSource.forEach(item => {
                if (item.Rate == null) {
                    errors.push("Rate field for currency " + item.Code + " is required");
                }
            });

            this.ValidationErrorsList = errors;

            if (this.ValidationErrorsList.length == 0) {

                this.CurrentSession.StartBusyIndicator("Saving...");

                var lastRates: LastRate[] = [];

                this.ItemsSource.forEach(item => {
                    var lastRateItem = new LastRate();
                    lastRateItem.Tenant = this.EntityPM.Id;
                    lastRateItem.BaseCurrencyId = this.EntityPM.CurrencyId;
                    lastRateItem.ForeignCurrencyId = item.ForeignCurrencyId;
                    lastRateItem.LogDateTime = DateTool.GetCurrentDateTimeAsUtc();
                    lastRateItem.ValueDate = DateTool.GetCurrentDateTimeAsUtc();
                    lastRateItem.Rate = item.Rate;
                    lastRates.push(lastRateItem);
                });


                var myService: CurrencyRatesService = new CurrencyRatesService();
                var myHelper = new AccountingCurrencyHelper();
                myHelper.TenantPM = this.EntityPM;
                myHelper.LastRates = lastRates;

                myService.UpdateAccountingCurrency(myHelper).subscribe((myResponse: ServiceResponse) => {
                    this.CurrentSession.StopBusyIndicator();

                    if (!myResponse.HasError) {
                        this.EntityPM = myResponse.Result;
                        InfraSettings.TenantPM = myResponse.Result;
                        this.CurrentSession.CloseCurrentWindowEmit("ok");
                    }

                    else {
                        this.ValidationErrorsList = myResponse.ErrorsArray;
                    }
                });
            }
        }
    }

    private myCloner: Cloner;
    private Clone() {
        this.myCloner = new Cloner(this.EntityPM);
        this.myCloner.AddField('CurrencyId');
        this.myCloner.AddEntity(this.EntityPM);
    }
    private RejectChanges() {
        this.myCloner.RejectChanges();
    }
}

export class CurrencyRatesModelData extends BaseComponent {
    public DataContext = this;
    public EntityPM: RatesTablePM;
    public ObjectTableName: string = "RatesTable";
    constructor(entityPM: RatesTablePM, RateIsEnabled: boolean) {
        super();
        this.EntityPM = entityPM;
        this.UIProperties.SetEnabled("Rate", this.ObjectTableName, RateIsEnabled);
    }

    get ForeignCurrencyId() { return this.EntityPM.ForeignCurrencyId; }
    get Code() { return this.EntityPM.ForeignCurrencyCode; }
    get ValueDate() { return this.EntityPM.ValueDate; }

    get Rate() { return this.EntityPM.Rate; }
    set Rate(value: number) {
        if (this.EntityPM.Rate != value) {
            this.EntityPM.Rate = value;
        }
    }
}
