import {Component, OnInit} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {AppTool, DateTool} from '../../../Infrastructure/Tools';
import {CurrencyRatesService, LastRate} from '../../../Common/Services/CurrencyRatesService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {MessageWindow} from '../../../Controls/Windows/MessageWindow';
import {RatesTablePM} from '../../../Infrastructure/EntityPMs/RatesTablePM';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';


@Component({
    selector: 'RatesMainTabComponent',
    
    templateUrl: './RatesMainTabComponent.html',
})

export class RatesMainTabComponent extends BaseComponent {
  public imgNgStyle: any = null;
    //Props 
    public TodayDate: Date = DateTool.GetCurrentDateAsUtc();
    public ItemsSource: RatesItem[] = [];
    public TenantPM: TenantPM;
    public DataContext: RatesMainTabComponent = this;
    private CurrentSession = SessionLocator.SelectedSession;
    IsAccountingActivated: boolean = false;
    constructor() {
        super();
        this.TenantPM = SessionLocator.TenantPM;
        this.IsAccountingActivated = SessionLocator.TenantPM.AccountingActivated;
        this.BuildData();
    }

    //Commands
    public EditRate(item: RatesItem) {
        var text: string = TextCodeTranslator.Translate("RatesTable.O.EditCurrencyRate");
        this.RunRateWindow(item, text);
    }

    public ViewHistory(item: RatesItem) {
        var windowTitle = TextCodeTranslator.Translate("RatesTable.O.CurrencyHistory") + ": " + item.Code;;
        var logWindow = new LogitudeWindow();
        logWindow.Width = 600;
        logWindow.Height = 400;
        logWindow.Title = windowTitle;
        logWindow.WindowArgs = item.LastRate;
        logWindow.Show('./Common/Components/Maintenance/RatesHistoryComponent');
    }

    private RunRateWindow(itemComponent: RatesItem, windowTitle: string) {
        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 600;
        logitudeWindow.Height = 400;
        logitudeWindow.Title = windowTitle;

        var entityPM: RatesTablePM = new RatesTablePM();
        entityPM.Tenant = this.TenantPM.Id;
        entityPM.BaseCurrencyId = this.TenantPM.CurrencyId;
        entityPM.ForeignCurrencyId = itemComponent.LastRate.ForeignCurrencyId;
        entityPM.LogDateTime = DateTool.GetCurrentDateTimeAsUtc();
        itemComponent.EntityPM = entityPM;
        logitudeWindow.DataContext = itemComponent;
        logitudeWindow.Show('./Common/Components/Maintenance/EditLastRateComponent');
        logitudeWindow.WindowClosed.subscribe(($event: any) => this.OnEditWindowClosed($event));
    }

    OnEditWindowClosed(arg: any) {
        if (arg == 'ok') {
            this.BuildData();
        }
    }

    CloseButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    //public ViewHistoryIsEnabled: boolean = false;

    // BuildData
    public BuildData() {
        this.ItemsSource = [];
        if (AppTool.IsNullOrEmpty(this.TenantPM.CurrencyId)) {
            var messageWindow: MessageWindow = new MessageWindow();
            messageWindow.Show("The System Base Currency is unknown!");
            return;
        }

        else {
            var list: LastRate[] = new Array<LastRate>();
            var myService: CurrencyRatesService = new CurrencyRatesService();
            var loadingDate: Date = this.TodayDate;
            if (loadingDate == null) {
                loadingDate = DateTool.GetCurrentDateAsUtc();
            }

            if (loadingDate != null) {
                //loadingDate = Date.SpecifyKind(loadingDate, Date.UTC);
            }

            myService.GetCurrenciesExchangeRateByValueDate(this.TenantPM.CurrencyId, loadingDate,true).subscribe((resp:any) => {
                var result: ServiceResponse = resp;
                if (!result.HasError) {
                    result.Result.forEach(item => {
                        var itemData: RatesItem = new RatesItem(item);
                        this.ItemsSource.push(itemData);
                    });
                }
                //else {
                //    var errors = result.ErrorsArray;
                //}
            });
        }
    }    
}

export class RatesItem extends BaseComponent implements OnInit {

    public LastRate: LastRate = new LastRate();
    public TenantPM: TenantPM;
    public EntityPM: RatesTablePM;
    public ObjectTableName = "RatesTable";
    public DataContext: RatesItem = this;
    IsAccountingActivated: boolean = false;
    constructor(entityPM: LastRate) {
        super();
        this.LastRate = entityPM;
        this.TenantPM = SessionLocator.TenantPM;
        this.IsAccountingActivated = SessionLocator.TenantPM.AccountingActivated;
        this.CreateRatesTablePM();
    }

    CreateRatesTablePM() {
        this.EntityPM = new RatesTablePM();
        this.EntityPM.Tenant = this.TenantPM.Id;
        this.EntityPM.BaseCurrencyId = this.TenantPM.CurrencyId;
        this.EntityPM.ForeignCurrencyId = this.LastRate.ForeignCurrencyId;
        this.EntityPM.LogDateTime = DateTool.GetCurrentDateAsUtc();
        this.EntityPM.ValueDate = DateTool.GetCurrentDateAsUtc();
    }

    ngOnInit() {
        this.SetUIProperties();
    }

    SetUIProperties() {
        var isEnabled: boolean = this.IsEditingEnabled;
        this.UIProperties.SetEnabled("Rate", "RatesTable", isEnabled);
        this.UIProperties.SetEnabled("ValueDate", "RatesTable", isEnabled);
    }

    // Props 
    get ValueDate() {
        return this.EntityPM.ValueDate;
    }
    set ValueDate(value: Date) {
        if (this.EntityPM.ValueDate != value) {
            this.EntityPM.ValueDate = null;

            if (value != null) {
                //this.RatesTablePM.ValueDate = value.Value.Date;
                this.EntityPM.ValueDate = value;
            }
        }
    }

    

    get Rate() {
        return this.EntityPM.Rate;
    }
    set Rate(value: number) {
        var varValue = AppTool.Round(value, 5);

        if (this.EntityPM.Rate != varValue) {
            this.EntityPM.Rate = varValue;
        }
    }

    get IsEditingEnabled() {
        var myResult: boolean = true;

        if (ObjectsLocator.IsDemoTenant(this.TenantPM.Id.toString())) {
            myResult = false;

            if (SessionLocator.LoggedUserPM.IsCustomerCare) {
                myResult = true;
            }
        }

        return myResult;
    }

    get LocalCode() {
        return this.LastRate.BaseCurrencyCode;
    }

    get Code() {
        return this.LastRate.ForeignCurrencyCode;
    }

    get Name() {
        return this.LastRate.ForeignCurrencyName;
    }

    get CurrentValueDate() {
        return this.LastRate.ValueDate;
    }
    set CurrentValueDate(value: Date) {
        this.LastRate.ValueDate = value;
    }

    get CurrentRate() {
        return this.LastRate.Rate;
    }

    set CurrentRate(value: number) {
        this.LastRate.Rate = value;
    }

    get Unit() {
        
        if(this.IsAccountingActivated){
            if(this.LastRate.Unit == null || this.LastRate.Unit <= 0){
                return 1;
            }
            return this.LastRate.Unit;
        } else {
            return 1;
        }
       
    }

    get ViewHistoryIsEnabled() {
        if (this.LastRate.HistoryCount > 0)
            return true;

        return false;
    }

    get ViewHistoryOpacity() {
        if (this.LastRate.HistoryCount > 0)
            return 1;
        return 0.5;
    }
}
