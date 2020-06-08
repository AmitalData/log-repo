import {Component, OnDestroy} from '@angular/core';
import {AppTool} from '../../../Infrastructure/Tools';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import { CardCurrenciesAccountingPM } from '../../EntityPMs/CardCurrenciesAccountingPM';

@Component({
    
    templateUrl: './AccountingTab_Partners.html',
})

export class AccountingTab_Partners extends BaseComponent implements OnDestroy {
    public EntityPM: any = null;
    public ObjectTableName: string;
    public DataContext = this;
    private CurrentSession = SessionLocator.SelectedSession;
    public ItemsSource: CardCurrenciesAccountingTab[] = [];

    constructor(private entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.ObjectTableName = entityArgs.ObjectTableName;

        if (SessionLocator.AccountingSystemPM) {
            if (SessionLocator.AccountingSystemPM.Code == "GI" || SessionLocator.AccountingSystemPM.Code == "AI") {
                this.IsExternalByProductsVisible = true;
            }
        }

        if (this.ObjectTableName == "Customer") {
            this.UIProperties.SetVisibility("PayablesAccountingCard", this.ObjectTableName, false);
        }

        this.Listen();
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;

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
                    }
                });
            }
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
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
    public EntityPM: CardCurrenciesAccountingPM;
    public ObjectTableName: string = "CardCurrenciesAccounting";
    public DataContext = this;
    public IsAccountingActivated = false;

    constructor(entityPM: CardCurrenciesAccountingPM, private father: AccountingTab_Partners) {
        super();
        this.EntityPM = entityPM;
        this.IsAccountingActivated = SessionLocator.TenantPM.AccountingActivated;
        this.SetUIProperties();
    }

    get CardId() { return this.EntityPM.CardId; }
    //get CurrencyName() { return this.EntityPM.CurrencyName; }
    get CurrencyId() { return this.EntityPM.CurrencyId; }


    SetUIProperties() {
        var isPayableFieldEnabled: boolean = false;
        var isReceivableFieldEnabled: boolean = false;

        if (this.father.AccountingVATSplit) {
            if (SessionLocator.Tenant == 65) {
                if (SessionLocator.LoggedUserPM.IsCustomerCare) {
                    isPayableFieldEnabled = this.father.EntityPM.IsPayable ? true : false;
                    isReceivableFieldEnabled = this.father.EntityPM.IsReceivable ? true : false;
                }
            }

            else {
                isPayableFieldEnabled = this.father.EntityPM.IsPayable ? true : false;
                isReceivableFieldEnabled = this.father.EntityPM.IsReceivable ? true : false;
            }
        }

        if (this.IsAccountingActivated) {
            this.UIProperties.SetEnabled("PayableDebitGLAcountId", this.ObjectTableName, isPayableFieldEnabled);
            this.UIProperties.SetEnabled("ReceivableCreditGLAccountId", this.ObjectTableName, isReceivableFieldEnabled);
        }
        else {
            this.UIProperties.SetEnabled("PayableDebitAccount", this.ObjectTableName, isPayableFieldEnabled);
            this.UIProperties.SetEnabled("ReceivableCreditAccount", this.ObjectTableName, isReceivableFieldEnabled);
        }
    }

    get PayableDebitAccount() { return this.EntityPM.PayableDebitAccount; }
    set PayableDebitAccount(value: string) {
        if (this.EntityPM.PayableDebitAccount != value) {
            this.EntityPM.PayableDebitAccount = value;
            this.OnDataInput();
        }
    }

    get ReceivableCreditAccount() { return this.EntityPM.ReceivableCreditAccount; }
    set ReceivableCreditAccount(value: string) {
        if (this.EntityPM.ReceivableCreditAccount != value) {
            this.EntityPM.ReceivableCreditAccount = value;
            this.OnDataInput();
        }
    }

    OnDataInput() {
        if (this.IsAccountingActivated) {

            this.father.EntityPM.AddCardCurrenciesAccountingPM(this.EntityPM);
        }
        else {
            if (AppTool.IsNullOrEmpty(this.PayableDebitAccount) && AppTool.IsNullOrEmpty(this.ReceivableCreditAccount)) {
                this.father.EntityPM.RemoveCardCurrenciesAccountingPM(this.EntityPM);
            }
            else {
                this.father.EntityPM.AddCardCurrenciesAccountingPM(this.EntityPM);
            }
        }
    }
}
