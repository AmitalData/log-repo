import {Component, OnDestroy} from '@angular/core';
import {AppTool} from '../../../Infrastructure/Tools';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import { CardExternalCodeByCurrencyPM } from '../../EntityPMs/CardExternalCodeByCurrencyPM';
import { CurrencyList } from '../../EntityLists/CurrencyList';
import { CurrencyListService } from '../../Services/StandardLists/CurrencyListService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';

@Component({
    
    templateUrl: './AccountingTab_Partners.html',
})

export class AccountingTab_Partners extends BaseComponent implements OnDestroy {
    public EntityPM: any = null;
    public ObjectTableName: string;
    public DataContext = this;
    private CurrentSession = SessionLocator.SelectedSession;
    public ItemsSource: CardCurrenciesAccountingTab[] = [];
    public IsPayablesAccountingCardVisible = true;
    private AllCurrencies: CurrencyList[] = [];

    constructor(private entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.ObjectTableName = entityArgs.ObjectTableName;
        this.InitializeComponent();
        if (SessionLocator.AccountingSystemPM) {
            if (SessionLocator.AccountingSystemPM.Code == "GI" || SessionLocator.AccountingSystemPM.Code == "AI") {
                this.IsExternalByProductsVisible = true;
            }
        }

        if (this.ObjectTableName == "Customer") {
            this.UIProperties.SetVisibility("PayablesAccountingCard", this.ObjectTableName, false);
            this.IsPayablesAccountingCardVisible = false;
        }

        this.Listen();
       
    }

    InitializeComponent() {
        this.SetUIProperties();
        var myService = new CurrencyListService();
        myService.getAll().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.AllCurrencies = myResponse.Result.filter(a => !a.InActive);
                this.BuildItemsSource();
            }
           
        });
    }

    BuildItemsSource() {
        this.ItemsSource = [];

        this.EntityPM.CardExternalCodeByCurrencies.forEach(item => {
            this.ItemsSource.push(new CardCurrenciesAccountingTab(item, this));
        });

        this.AllCurrencies.forEach(list => {
            var existingItem: CardCurrenciesAccountingTab = this.ItemsSource.filter(f => f.CurrencyId == list.Id)[0];
            if (existingItem == null) {
                var newItemPM = new CardExternalCodeByCurrencyPM(null);
                newItemPM.Tenant = SessionLocator.Tenant;
                newItemPM.CurrencyId = list.Id;
                newItemPM.CurrencyName = list.EnglishName;
                newItemPM.CurrencyCode = list.Code;
                newItemPM.CardId = this.EntityPM.Id;
                newItemPM.CardName = this.EntityPM.EnglishName;

                this.ItemsSource.push(new CardCurrenciesAccountingTab(newItemPM, this));
            }
        });
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.BuildItemsSource();
                        if (this.isExternalByProductsRequestd) {
                            this.ApplyExternalByProducts();
                        }
                    }

                    this.isExternalByProductsRequestd = false;
                });
            }

            if (this.LoadCompletedEvent == null) {
                this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        this.BuildItemsSource();
                    }
                });
            }
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    get BillToId() { return this.EntityPM.BillToId; }
    set BillToId(value: boolean) {
        if (this.EntityPM.BillToId != value) {
            this.EntityPM.BillToId = value;
        }
    }

    get AccountingVATSplit() { return this.EntityPM.AccountingVATSplit; }
    set AccountingVATSplit(value: boolean) {
        if (this.EntityPM.AccountingVATSplit != value) {
            this.EntityPM.AccountingVATSplit = value;
            this.SetUIProperties();
        }
    }

    SetUIProperties() {
        var isVATSplitEnabled: boolean = true;
        var isPayableFieldEnabled: boolean = false;
        var isReceivableFieldEnabled: boolean = false;

        if (SessionLocator.Tenant == 65) {
            if (!SessionLocator.LoggedUserPM.IsCustomerCare) {
                isVATSplitEnabled = false;
            }
        }

        if (isVATSplitEnabled) {
            isPayableFieldEnabled = !this.AccountingVATSplit ? true : false;
            isReceivableFieldEnabled = !this.AccountingVATSplit ? true : false;
        }

        this.UIProperties.SetEnabled("AccountingVATSplit", this.ObjectTableName, isVATSplitEnabled);
        this.UIProperties.SetEnabled("ReceivablesAccountingCard", this.ObjectTableName, isPayableFieldEnabled);
        this.UIProperties.SetEnabled("PayablesAccountingCard", this.ObjectTableName, isReceivableFieldEnabled);

        this.ItemsSource.forEach(item => {
            item.SetUIProperties();
        });
    }

    get ReceivablesAccountingCard() { return this.EntityPM.ReceivablesAccountingCard; }
    set ReceivablesAccountingCard(value: string) {
        if (this.EntityPM.ReceivablesAccountingCard != value) {
            this.EntityPM.ReceivablesAccountingCard = value;
        }
    }

    get PayablesAccountingCard() { return this.EntityPM.PayablesAccountingCard; }
    set PayablesAccountingCard(value: string) {
        if (this.EntityPM.PayablesAccountingCard != value) {
            this.EntityPM.PayablesAccountingCard = value;
        }
    }

    public IsExternalByProductsVisible: boolean = false;
    private isExternalByProductsRequestd: boolean = false;
    ExternalByProductsClicked() {
        if (!this.isExternalByProductsRequestd) {
            this.isExternalByProductsRequestd = true;
            this.CurrentSession.CurrentEditComponent.SaveChanges();
        }
    }
    ApplyExternalByProducts() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = "Accounting advanced";
        logWindow.WindowArgs = { EntityPM: this.EntityPM, ObjectTableName: this.ObjectTableName };
        logWindow.Show('./Common/Components/Partners/AddEdit/ExternalAccountsByProductsComponent');
    }
}

export class CardCurrenciesAccountingTab extends BaseComponent {
    public EntityPM: CardExternalCodeByCurrencyPM;
    public ObjectTableName: string = "CardCurrenciesAccounting";
    public DataContext = this;

    constructor(entityPM: CardExternalCodeByCurrencyPM, private father: AccountingTab_Partners) {
        super();
        this.EntityPM = entityPM;
        this.SetUIProperties();
    }

    get CardId() { return this.EntityPM.CardId; }
    get CurrencyId() { return this.EntityPM.CurrencyId; }
    get CardName() { return this.EntityPM.CardName; }
    get CurrencyName() { return this.EntityPM.CurrencyName; }
    get CurrencyCode() { return this.EntityPM.CurrencyCode; }

    SetUIProperties() {
        var isPayableFieldEnabled: boolean = false;
        var isReceivableFieldEnabled: boolean = false;

        if (this.father.AccountingVATSplit) {
            if (SessionLocator.Tenant == 65) {
                if (SessionLocator.LoggedUserPM.IsCustomerCare) {
                    isPayableFieldEnabled = isReceivableFieldEnabled = true;
                }
            }

            else {
                isPayableFieldEnabled = isReceivableFieldEnabled = true;
            }
        }

        this.UIProperties.SetEnabled("PayableDebitAccount", this.ObjectTableName, isPayableFieldEnabled);
        this.UIProperties.SetEnabled("ReceivableCreditAccount", this.ObjectTableName, isReceivableFieldEnabled);
    }

    get PayableDebitAccount() { return this.EntityPM.ExternalPayableTableId; }
    set PayableDebitAccount(value: string) {
        if (this.EntityPM.ExternalPayableTableId != value) {
            this.EntityPM.ExternalPayableTableId = value;
            this.OnDataInput();
        }
    }

    get ReceivableCreditAccount() { return this.EntityPM.ExternalRecievableTableId; }
    set ReceivableCreditAccount(value: string) {
        if (this.EntityPM.ExternalRecievableTableId != value) {
            this.EntityPM.ExternalRecievableTableId = value;
            this.OnDataInput();
        }
    }

    OnDataInput() {
        if (AppTool.IsNullOrEmpty(this.PayableDebitAccount) && AppTool.IsNullOrEmpty(this.ReceivableCreditAccount)) {
            this.father.EntityPM.RemoveCardExternalCodeByCurrencyPM(this.EntityPM);
        }
        else {
            this.father.EntityPM.AddCardExternalCodeByCurrencyPM(this.EntityPM);
        }
    }
}
