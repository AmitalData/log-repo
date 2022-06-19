import {Component, OnDestroy} from '@angular/core';
import {AppTool} from '../../../Infrastructure/Tools';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ChargesTypePM} from '../../EntityPMs/ChargesTypePM';
import {ChargeTypeAccountingPM} from '../../EntityPMs/ChargeTypeAccountingPM';
import {VatTypeList} from '../../EntityLists/VatTypeList';
import {VatTypeListService} from '../../Services/StandardLists/VatTypeListService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {ApiQueryFilters} from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ObjectsLocator } from '../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    
    templateUrl: './AccountingTab_ChargesType.html',
})

export class AccountingTab_ChargesType extends BaseComponent implements OnDestroy {
    public EntityPM: ChargesTypePM = null;
    public ObjectTableName: string;
    public DataContext = this;
    public ItemsSource: AccountingTabVATCharge[] = [];
    private AllVatTypes: VatTypeList[] = [];
    public IsAccountingActivated = false;
    public ReceivableCreditGLAccountFilterItems: ApiQueryFilters;
    public PayableDebitGLAcountFilterItems: ApiQueryFilters;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.IsAccountingActivated = SessionLocator.TenantPM.AccountingActivated;
        this.ObjectTableName = entityArgs.ObjectTableName;
        this.SetUIProperties();
        this.InitializeComponent();

        if (SessionLocator.AccountingSystemPM) {
            if (SessionLocator.AccountingSystemPM.Code == "GI" || SessionLocator.AccountingSystemPM.Code == "AI") {
                this.IsExternalByProductsVisible = true;
            }
        }
        this.ReceivableCreditGLAccountFilterItems = new ApiQueryFilters();
        this.PayableDebitGLAcountFilterItems = new ApiQueryFilters();
        this.ReceivableCreditGLAccountFilterItems.addAdditionalFilter("ReceivableCreditFilter", "1", null, null, "Equals", true, false, false, "string", false, true);
        this.PayableDebitGLAcountFilterItems.addAdditionalFilter("PayableDebitFilter", "2", null, null, "Equals", true, false, false, "string", false, true);

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

    SetUIProperties() {

        var isVATSplitEnabled: boolean = true;
        var isPayableFieldEnabled: boolean = false;
        var isReceivableFieldEnabled: boolean = false;

        if (ObjectsLocator.IsDemoTenant(SessionLocator.Tenant.toString())) {
            if (!SessionLocator.LoggedUserPM.IsCustomerCare) {
                isVATSplitEnabled = false;
            }
        }

        if (isVATSplitEnabled) {
            isPayableFieldEnabled = this.EntityPM.IsPayable && !this.AccountingVATSplit ? true : false;
            isReceivableFieldEnabled = this.EntityPM.IsReceivable && !this.AccountingVATSplit ? true : false;
        }

        if (this.IsAccountingActivated) {
            this.UIProperties.SetEnabled("PayableDebitGLAcountId", this.ObjectTableName, isPayableFieldEnabled);
            this.UIProperties.SetEnabled("ReceivableCreditGLAccountId", this.ObjectTableName, isReceivableFieldEnabled);
        }
        else {
            this.UIProperties.SetEnabled("AccountingVATSplit", this.ObjectTableName, isVATSplitEnabled);
            this.UIProperties.SetEnabled("PayableDebitAccount", this.ObjectTableName, isPayableFieldEnabled);
            this.UIProperties.SetEnabled("ReceivableCreditAccount", this.ObjectTableName, isReceivableFieldEnabled);
        }
        

        this.ItemsSource.forEach(item => {
            item.SetUIProperties();
        });
    }
    InitializeComponent() {
        var myService = new VatTypeListService();

        myService.getAllFromCache().subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {
                this.AllVatTypes = myResponse.Result;
            }

            this.BuildItemsSource();
        });
    }

    BuildItemsSource() {
        this.ItemsSource = [];

        this.EntityPM.ChargeTypeAccountings.forEach(item => {
            this.ItemsSource.push(new AccountingTabVATCharge(item, this));
        });

        this.AllVatTypes.forEach(list => {
            var existingItem: AccountingTabVATCharge = this.ItemsSource.filter(f => f.VatTypeId == list.Id)[0];
            if (existingItem == null) {
                var newItemPM = new ChargeTypeAccountingPM(null);
                newItemPM.Tenant = SessionLocator.Tenant;
                newItemPM.VatTypeId = list.Id;                    
                newItemPM.VatTypeName = list.EnglishName;
                newItemPM.ChargeTypeId = this.EntityPM.Id;
                newItemPM.ChargeTypeName = this.EntityPM.EnglishName;

                this.ItemsSource.push(new AccountingTabVATCharge(newItemPM, this));
            }
        });
    }

    get AccountingVATSplit() { return this.EntityPM.AccountingVATSplit; }
    set AccountingVATSplit(value: boolean) {
        if (this.EntityPM.AccountingVATSplit != value) {
            this.EntityPM.AccountingVATSplit = value;
            this.SetUIProperties();
        }
    }

    get PayableDebitAccount() { return this.EntityPM.PayableDebitAccount; }
    set PayableDebitAccount(value: string) {
        if (this.EntityPM.PayableDebitAccount != value) {
            this.EntityPM.PayableDebitAccount = value;
        }
    }

    get PayableDebitGLAcountId() { return this.EntityPM.PayableDebitGLAcountId; }
    set PayableDebitGLAcountId(value: string) {
        if (this.EntityPM.PayableDebitGLAcountId != value) {
            this.EntityPM.PayableDebitGLAcountId = value;
            if (this.EntityPM.PayableDebitGLAcountId == null) this.EntityPM.PayDebitGLAcountLocalName = null;
        }
    }
    
    get ReceivableCreditAccount() { return this.EntityPM.ReceivableCreditAccount; }
    set ReceivableCreditAccount(value: string) {
        if (this.EntityPM.ReceivableCreditAccount != value) {
            this.EntityPM.ReceivableCreditAccount = value;
        }
    }

    get ReceivableCreditGLAccountId() { return this.EntityPM.ReceivableCreditGLAccountId; }
    set ReceivableCreditGLAccountId(value: string) {
        if (this.EntityPM.ReceivableCreditGLAccountId != value) {
            this.EntityPM.ReceivableCreditGLAccountId = value;
            if (this.EntityPM.ReceivableCreditGLAccountId == null) this.EntityPM.RecCreditGLAcountLocalName = null;
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
        logWindow.WindowArgs = { EntityPM: this.EntityPM };
        logWindow.Show('./Common/Components/AccountingTab/Advanced/ChargesExternalByProductsComponent');
    }
}
export class AccountingTabVATCharge extends BaseComponent {
    public EntityPM: ChargeTypeAccountingPM;
    public ObjectTableName: string = "ChargeTypeAccounting";
    public DataContext = this;
    public IsAccountingActivated = false; 

    constructor(entityPM: ChargeTypeAccountingPM, private father: AccountingTab_ChargesType) {
        super();
        this.EntityPM = entityPM;
        this.IsAccountingActivated = SessionLocator.TenantPM.AccountingActivated;
        this.SetUIProperties();
    }

    get VatTypeId() { return this.EntityPM.VatTypeId; }
    get VatTypeName() { return this.EntityPM.VatTypeName; }
    get ChargeTypeId() { return this.EntityPM.ChargeTypeId; }
    get ChargeTypeName() { return this.EntityPM.ChargeTypeName; }

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

    get PayableDebitGLAcountId() { return this.EntityPM.PayableDebitGLAcountId; }
    set PayableDebitGLAcountId(value: string) {
        if (this.EntityPM.PayableDebitGLAcountId != value) {
            this.EntityPM.PayableDebitGLAcountId = value;
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

    get ReceivableCreditGLAccountId() { return this.EntityPM.ReceivableCreditGLAccountId; }
    set ReceivableCreditGLAccountId(value: string) {
        if (this.EntityPM.ReceivableCreditGLAccountId != value) {
            this.EntityPM.ReceivableCreditGLAccountId = value;
            this.OnDataInput();
        }
    }

    OnDataInput() {
        if (this.IsAccountingActivated) {

            if (AppTool.IsNullOrEmpty(this.PayableDebitGLAcountId) && AppTool.IsNullOrEmpty(this.ReceivableCreditGLAccountId)) {
                this.father.EntityPM.RemoveChargeTypeAccountingPM(this.EntityPM);
            }
            else {
                this.father.EntityPM.AddChargeTypeAccountingPM(this.EntityPM);
            }
        }
        else {
            if (AppTool.IsNullOrEmpty(this.PayableDebitAccount) && AppTool.IsNullOrEmpty(this.ReceivableCreditAccount)) {
                this.father.EntityPM.RemoveChargeTypeAccountingPM(this.EntityPM);
            }
            else {
                this.father.EntityPM.AddChargeTypeAccountingPM(this.EntityPM);
            }
        }
    }
}
