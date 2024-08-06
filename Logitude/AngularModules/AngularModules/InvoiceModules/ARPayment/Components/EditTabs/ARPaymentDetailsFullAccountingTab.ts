import { Component, OnDestroy, OnInit } from '@angular/core';
import { ARPaymentBankTranferPM } from 'Invoice/EntityPMs/ARPaymentBankTranferPM';
import { BankAccountPM } from '../../../../Accounting/EntityPMs/BankAccountPM';
import { CashBookPM } from '../../../../Accounting/EntityPMs/CashBookPM';
import { GLAccountPM } from '../../../../Accounting/EntityPMs/GLAccountPM';
import { BankAccountPMService } from '../../../../Accounting/Services/StandardPMs/BankAccountPMService';
import { GLAccountPMService } from '../../../../Accounting/Services/StandardPMs/GLAccountPMService';
import { AddressList } from '../../../../Common/EntityLists/AddressList';
import { CardList } from '../../../../Common/EntityLists/CardList';
import { CurrencyList } from '../../../../Common/EntityLists/CurrencyList';
import { CurrencyRatesService, LastRate } from '../../../../Common/Services/CurrencyRatesService';
import { PartnersDomainService } from '../../../../Common/Services/PartnersDomainService';
import { AddressListService } from '../../../../Common/Services/StandardLists/AddressListService';
import { CardListService } from '../../../../Common/Services/StandardLists/CardListService';
import { CurrencyListService } from '../../../../Common/Services/StandardLists/CurrencyListService';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ApiQueryFilters } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { AppTool, ArrayTool, DateTool } from '../../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { AccountingPaymentMethodList } from '../../../../Invoice/EntityLists/AccountingPaymentMethodList';
import { ARInvoiceList } from '../../../../Invoice/EntityLists/ARInvoiceList';
import { ARPaymentChequeReplicaPM } from '../../../../Invoice/EntityPMs/ARPaymentChequeReplicaPM';
import { ARPaymentPM } from '../../../../Invoice/EntityPMs/ARPaymentPM';
import { InvoiceDomainService } from '../../../../Invoice/Services/InvoiceDomainService';
import { AccountingPaymentMethodListService } from '../../../../Invoice/Services/StandardLists/AccountingPaymentMethodListService';
import { ARInvoiceListService } from '../../../../Invoice/Services/StandardLists/ARInvoiceListService';
import { ARPaymentValidator } from '../../../../Invoice/Validators/ARPaymentValidator';
import { JournalPM } from './../../../../Accounting/EntityPMs/JournalPM';
import { LedgerTransactionPM } from './../../../../Accounting/EntityPMs/LedgerTransactionPM';
import { LedgerTransactionExtendedListService } from './../../../../Accounting/Services/ExtendedLists/LedgerTransactionExtendedListService';
import { JournalExtendedPMService } from './../../../../Accounting/Services/ExtendedPMs/JournalExtendedPMService';
import { ReconciliationExtendedPMService } from './../../../../Accounting/Services/ExtendedPMs/ReconciliationExtendedPMService';
import { GLAccountListService } from './../../../../Accounting/Services/StandardLists/GLAccountListService';
import { AccountingEntityHelper } from './../../../../Accounting/Utilities/AccountingEntityHelper';
import { PartnerTypeList } from 'Common/EntityLists/PartnerTypeList';
import { PartnerTypeListService } from 'Common/Services/StandardLists/PartnerTypeListService';
import { FullAccountingSettingPM } from 'Accounting/EntityPMs/FullAccountingSettingPM';
import { EntityListService } from 'Infrastructure/Services/EntityListService';
import { BankAccountListService } from 'Accounting/Services/StandardLists/BankAccountListService';
declare var window: any;

@Component({

    templateUrl: './ARPaymentDetailsFullAccountingTab.html',
    styleUrls: ['./ARPaymentDetailsFullAccountingTab.css']
})

export class ARPaymentDetailsFullAccountingTab extends BaseComponent implements OnInit, OnDestroy
{


    public TransactionsList: ObservableCollection;


    public EntityPM: ARPaymentPM;
    public glAccount: GLAccountPM;
    public ObjectTableName = "ARPayment";
    public DataContext = this;
    public ItemsSource: ObservableCollection;
    public isFullAccounting: boolean = false;
    public DisplaySATSettings: boolean = false;
    public IsMultiCurrency: boolean = false;
    public TransferStatusVisibilityColumn: boolean = false;
    public EnableNegativeOffsetARPayments: boolean = false;
    public IsEditExchangeRateVisible: boolean = false;
    _PartnerTypeListService: PartnerTypeListService = new PartnerTypeListService();
    DisabledPartnerTypes: boolean = false;
    get IsNegativeAmountEnabled() { return this.EnableNegativeOffsetARPayments == true && this.AccountingPaymentMethodCode == "FS" ? true : false; }
    public isRTL: boolean = false;
    public showLocal: boolean = false;
    public ARPaymentChequeStatus = "";
    public ARPaymentChequeStatusColor = "black";
    BankFieldsVisibile: boolean;
    isMultipleCheques: boolean = false;
    isMultipleBankTransfers: boolean = false;
    public OpenAmountCurrency: string;
    ARPaymentValidator: ARPaymentValidator;
    public chequeAmount: number;
    public bankTransferAmount: number;
    public InvoiceAmountCurrency: string = TextCodeTranslator.Translate("Accounting.O.ARP.InvoiceAmount") + " (" + SessionLocator.TenantPM.CurrencyCode + ")";
    public PaymenyAmount: number;
    public BankAccountsFilterItems: ApiQueryFilters;
    _AccountingPaymentMethodListService = new AccountingPaymentMethodListService();
    public PartnerTypes: PartnerTypeList[] = [];
    private FullAccountingSetting: FullAccountingSettingPM = new FullAccountingSettingPM();
    public BillToFilter:ApiQueryFilters;
   get TextStore()
    {
        return TextStore;
    }

    _LedgerTransactionExtendedListService: LedgerTransactionExtendedListService = new LedgerTransactionExtendedListService();
    _ReconciliationExtendedPMService: ReconciliationExtendedPMService = new ReconciliationExtendedPMService();
    _JournalExtendedPMService: JournalExtendedPMService = new JournalExtendedPMService();
    private _glaService: GLAccountListService = new GLAccountListService();
    private CurrentSession = SessionLocator.SelectedSession;
    public PaymentCurrencySign: string;
    DisplayFieldsFromList:string;
    DisplayLocalFieldsFromList:string;
    BillToLovSizeForFullAccounting:number;
    public GLAccountsFilterItems: ApiQueryFilters;

    constructor(private entityArgs: EntityArgs, private _entityResourceService: EntityResourceService, public entityListService: EntityListService)
    {
        super();
        console.log("[FULL ACCOUNING ARPayment]");

        if (ObjectsLocator.GlobalSetting) {
            this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        }

        this.showLocal = !SessionLocator.LoggedUserPM.DontShowLocal;
        
        this.EntityPM = entityArgs.EntityPM;
        if( this.EntityPM.StatusCode==null)
            this.CreateARPayment();
            this.InitLOVFilters();
        this.SetAmountCurrencyCode();
        this.ComputeLocalAmount();
        this.SetPaymentAmount();
        this.SetChequeAmount();
        this.SetBankTransfersAmount();
        if (this.EntityPM.ARPaymentChequeReplicas.length > 1) {
            this.isMultipleCheques = true;
        }
        if (this.EntityPM.ARPaymentBankTranfers.length > 1) {
            this.isMultipleBankTransfers = true;
        }

        this.isFullAccounting = SessionLocator.TenantPM.AccountingActivated;
        this.originalPaymentOpenAmount = this.EntityPM.OpenAmount;
        this.paymentAmountTotal = this.EntityPM.AmountInPaymentCurrency;
        this.PaymentCurrencySign = this.EntityPM.PaymentCurrencySign;
        this.TransactionsList = new ObservableCollection([]);
        this.ARPaymentValidator = new ARPaymentValidator();
        //#region old
        this.ItemsSource = new ObservableCollection([]);
        this.EnableNegativeOffsetARPayments = ObjectsLocator.AccountingSettingPM.EnableNegativeOffsetARPayments;
        this.BankAccountsFilterItems = new ApiQueryFilters();
        if(this.EntityPM.PaymentCurrencyId && this.EntityPM.AccountingPaymentMethodCode == "BT") {
            this.BankAccountsFilterItems.addAdditionalFilter("CurrencyId", this.EntityPM.PaymentCurrencyId, null, null, "Equals", false, false, false, "string");
            this.loadBankAccounts(this.BankAccountsFilterItems);
        }
        if (FeatureLocator.HasFeaturePermession(this.ObjectTableName, "EnableMultiCurrency")) {
            if (ObjectsLocator.AccountingSettingPM.EnableMultiCurrencyARPayments) {
                this.IsMultiCurrency = true;
            }
        }

        if (SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBO" || SessionLocator.AccountingSettingPM.AccountingSystemCode == "QBOG") {
            this.TransferStatusVisibilityColumn = true;
        }

        if (SessionLocator.SATInterfaceSettings.SATInterfaceCode != "NONE") {
            this.DisplaySATSettings = true;
        }

        if (FeatureLocator.HasFeaturePermession("General", "General.Features.SystemCurrencies")) {
            this.IsEditExchangeRateVisible = true;
        }
        this.GetFullAccountingSettings();
        this.SetUIProperties();
        this.ComputeRelativeRateDate();
        this.Listen();
        this.CheckARPaymentCashBook();

        if (AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "DR") {
            this.LoadCurrencyRates();
        }

        else {
            this.LoadData();
        }

        //#endregion
        if (!this.PrintNotes) this.PrintNotes = TextCodeTranslator.Translate("ARPayment.S.ShortTitle");

        this.GetData();

        // this.UIProperties.SetEnabled("AmountToReconcile","LedgerTransaction",!this.IsGridReadOnly);
        this.InitializeBillToLov();
        this.getPartnerTypes();
    }

    InitLOVBillToFilters() {
        this.BillToFilter = new ApiQueryFilters();
        this.BillToFilter.addAdditionalFilter("ActiveGLAccount", true, null, null, "Equals", true, false, false, "Boolean");
    }
    InitLOVFilters() {
        this.GLAccountsFilterItems = new ApiQueryFilters();
        this.GLAccountsFilterItems.addAdditionalFilter("GLAccountId", "null", null, null, "NotEqual", false, false, false, "string");
    }
  GetFullAccountingSettings() {
        this.CurrentSession.StartBusyIndicatorLoading();
        this.entityListService.getSingle(SessionLocator.TenantPM.Id.toString(), "FullAccountingSetting").then((res: any) => {
        this.CurrentSession.StopBusyIndicator();
            res.subscribe(myResponse => {
                if (myResponse != null) {

                
                    this.FullAccountingSetting =  myResponse.Result;
                    this.GetRateIsEnabled();
                }
            })
        });

    }
    get IsRateDisabled (){
        return (this.isFullAccounting && !this.FullAccountingSetting.AllowEditingExchangeRate);
    }
    
    CreateARPayment() {
        //this.EntityPM = new ARPaymentPM();
        this.EntityPM.Tenant =  SessionLocator.TenantPM.Id;
        this.EntityPM.StatusCode = "DR";
        this.EntityPM.StatusName = "Draft";
        this.EntityPM.SATTransferStatusCode = "NT";
        this.EntityPM.SATTransferStatusName = "Not Transfered";
        this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.CreateDate = DateTool.GetCurrentDateAsUtc();
        this.EntityPM.UpdateDate = DateTool.GetCurrentDateAsUtc();
        this.EntityPM.LocalCurrencyId = SessionLocator.TenantPM.CurrencyId;
        this.EntityPM.RegisterDate = DateTool.GetCurrentDateAsUtc();
        this.EntityPM.PaymentCurrencyId = SessionLocator.TenantPM.CurrencyId;
        this.EntityPM.PaymentCurrencyExchangeRate = 1;
        this.loadPartnerTypesFilter();
        this.SetDefalutPaymentMethod();


    }
    private loadPartnerTypesFilter()
    {
        this.checkPartnerTypesFilterFeature();
        this.getPartnerTypes();
    }
    checkPartnerTypesFilterFeature(){
        var arpaymentOT = window.ObjectTables.filter(d => d.Name === "ARPayment")[0];
        this.isPartnerTypesFilterEnabled =  FeatureLocator.Features.filter(f => (f.Code == "NewScreenPartnerTypes") && f.ObjectTableId == arpaymentOT.Id)[0]? true : false;
    }
    SetDefalutPaymentMethod() {
        var DefaultSelectedPaymentMethodCode = "BT";
        var filter = new ApiQueryFilters(true);
        filter.addAdditionalFilter("Code", DefaultSelectedPaymentMethodCode, null, null, "Equal", false, false, false, "string");
        this._AccountingPaymentMethodListService.getByFilters(filter).subscribe(e=>{
            if(e && !e.HasError && e.Result.length > 0){
                this.AccountingPaymentMethodId = e.Result[0].Id;
                this.AccountingPaymentMethodCode = DefaultSelectedPaymentMethodCode;
                this.UIProperties.SetVisibility("BankAccountId", this.ObjectTableName, true);
                this.BankAccountIdVisibility = true;
                this.UIProperties.SetRequired("BankAccountId", this.ObjectTableName, true);
            }
        });

    }


    private SetChequeAmount() {
        if (this.EntityPM.ARPaymentChequeReplicas.length == 0) {
            this.chequeAmount = this.EntityPM.AmountInPaymentCurrency;
        }
        else {
            var firstCheque = this.EntityPM.ARPaymentChequeReplicas.filter(d => d.LineNumber == 1)[0];
            this.chequeAmount = firstCheque.ForeignAmount;
        }
    }

    private SetBankTransfersAmount() {
        if (this.EntityPM.ARPaymentBankTranfers.length == 0) {
            this.bankTransferAmount = this.EntityPM.AmountInPaymentCurrency;
        }
        else {
            var firstBankTransfer = this.EntityPM.ARPaymentBankTranfers.filter(d => d.LineNumber == 1)[0];
            this.bankTransferAmount = firstBankTransfer.ForeignAmount;
        }
    }

    private InitializeBillToLov() {
        this.InitLOVBillToFilters();
        this.DisplayFieldsFromList = "Code,CalculatedEnglishName,CalculatedLocalName,CalculatedLocalName,GLAccountDisplayNumber,CountryCode,PartnerTypeName";
        this.DisplayLocalFieldsFromList = "Code,CalculatedLocalName,GLAccountDisplayNumber,CountryCode,PartnerTypeName";
        this.BillToLovSizeForFullAccounting = 550;

    }

    SetAmountCurrencyCode()
    {
        if (this.EntityPM)
            if (this.EntityPM.GLAccountRecoMethodCode == "0") {
                this.OpenAmountCurrency = TextCodeTranslator.Translate("LedgerTransaction.F.OpenAmount") + " (" + SessionLocator.TenantPM.CurrencyCode + ")";
                this.ReconcileAmountCurrency = " (" + SessionLocator.TenantPM.CurrencyCode + ")";

            }
            else {
                this.OpenAmountCurrency = TextCodeTranslator.Translate("LedgerTransaction.F.OpenAmount") + " (" + this.EntityPM.GLAccountCurrencyCode + ")";
                this.ReconcileAmountCurrency = " (" + this.EntityPM.GLAccountCurrencyCode + ")";

            }
    }
    ReconcileAmountCurrency: string;

    ngOnInit() {
        this.LoadPaymentMethods();

        this.checkLedgerCreated();
    }

    ngOnDestroy()
    {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    _IsDisplayOnly = false;
    public get IsDisplayOnly(): boolean
    {
        return this._IsDisplayOnly;
    }
    public set IsDisplayOnly(v: boolean)
    {
        this._IsDisplayOnly = v;
    }

    checkLedgerCreated(firstCall: boolean = false)
    {

        if (this.EntityPM.Id && this.isFullAccounting && this.EntityPM.StatusCode == 'AD') {

            this.StartBusyIndicator('checkLedgerCreated');
            this._JournalExtendedPMService.GetByAccountingEntityId(this.EntityPM.Id, '3').subscribe((myResult: ServiceResponse) => // 3- ARPayment
            {
                console.log("_JournalExtendedPMService.GetByAccountingEntityId", myResult);
                this.StopBusyIndicator('checkLedgerCreated');

                var res: ServiceResponse = myResult;
                var createdJournal: JournalPM = res.Result;

                if (createdJournal) {
                    if (this.IsDisplayOnly == true && createdJournal.IsLedgerCreated) {
                        this.entityArgs.EditComponent.ReloadEntityPM();
                        // this.GetData();
                    }
                    this.IsDisplayOnly = !createdJournal.IsLedgerCreated;

                }
                else {
                    console.log("[Check Ledger] no journal created");
                }

            });

        }

    }

    //#region abdullah code
    originalPaymentOpenAmount: number;
    paymentAmountTotal: number = 0;
    amount2reconcileTotal: number = 0;
    paymentReconciledAmountTotal: number = 0;


    StartBusyIndicator(msg: string = '')
    {
        console.log("[Busy Indicator] start: ", msg);
        this.CurrentSession.StartBusyIndicatorLoading();
    }
    StopBusyIndicator(msg: string = '')
    {
        console.log("[Busy Indicator] stop: ", msg);
        this.CurrentSession.StopBusyIndicator();
    }

    // IsEntityValid: boolean = true;

    public get IsGridReadOnly(): boolean
    {

        // return this.EntityPM.StatusCode == 'CL' || this.EntityPM.StatusCode == 'VD' || this.EntityPM.OpenAmount == 0;
        return this.EntityPM.StatusCode == 'CL' || this.EntityPM.StatusCode == 'VD';
    }


    public get IsEntityValid(): boolean
    {

        var _valid = true;

        _valid = this.TransactionsList.Collection.every(d => d.isLineValid == true);

        return _valid;
    }
    SetEntityValidity()
    {
        this.CurrentSession.CurrentEditComponent.IsEditValid = this.IsEntityValid;
    }


    _loading: boolean = false;
    SetPaymentAmount()
    {
        if (this.EntityPM)
            if (this.EntityPM.GLAccountRecoMethodCode == "0") {
                this.PaymenyAmount = this.EntityPM.AmountInLocalCurrency;
            }
            else {
                this.PaymenyAmount = this.EntityPM.AmountInPaymentCurrency;
            }
    }
    glaccount: any;
    ReloadGLAccount()
    {
        //1- get glaccount
        this.fetchBillToCard().then(res =>
        {
            var card = res;
            this.glaccount=null;
            this.fetchGLAccount().then(response =>
            {
                 this.glaccount= response;

                if (this.glaccount) {
                    this.EntityPM.GLAccountId = this.glaccount.Id;
                    this.EntityPM.GLAccountRecoMethodCode = this.glaccount.ReconcileMethodCode;
                    this.EntityPM.GLAccountCurrencyCode = this.glaccount.CurrencyCode;
                    this.SetAmountCurrencyCode();
                    this.ComputeLocalAmount();
                    this.SetPaymentAmount();
                    console.log("GLAccount reloaded: " + this.EntityPM.GLAccountId);
                    this.GetData();
                }

            });

        });
    }

    GetData()
    {
        if (this.EntityPM.GLAccountId) {
            console.log(">>> Getting transactions for Account: ", this.EntityPM.GLAccountId);
            this.TransactionsList.Clear();
            this.EntityPM.InvoicesLedgerTransactions = [];

            this._loading = true;

            // setTimeout(() => {








            this._LedgerTransactionExtendedListService.getTransactionsForARPayment(this.EntityPM.Id, this.EntityPM.GLAccountId, this.EntityPM.PaymentCurrencyId).subscribe((myResult: ServiceResponse) => {

                this._loading = false;

                var mm: ServiceResponse = myResult;
                if (!mm.HasError) {

                    var transactions = mm.Result;
                    var tempItemSource: any[] = [];
                    if (transactions != null) {
                        for (var i = 0; i < transactions.length; i++) {
                            var line = new TransactionLineModel(transactions[i], this);
                            // var line = transactions;
                            tempItemSource.push(line);
                        }

                        var sortedTransactions = this.sortTransactionsByStatus(tempItemSource);
                        // tempItemSource = tempItemSource.sort((a: TransactionLineModel, b: TransactionLineModel) =>
                        // {
                        //     if(a.Status == TextStore.Closed)
                        //         return -1;
                        //     else if(a.Status == TextStore.open)
                        //         return 0;
                        //     else
                        //         return 1;

                        //     // return (a.ReconciledAmount === b.ReconciledAmount) ? 0 : (a.ReconciledAmount > b.ReconciledAmount) ? -1 : 1;
                        // });
                        if(this.glaccount?.IsMultiCurrency && this.glaccount?.ReconcileMethodCode==1)
                                sortedTransactions=sortedTransactions.filter(a=>a.CurrencyId==this.PaymentCurrencyId)
                        this.TransactionsList.InsertCollection(sortedTransactions);
                    }
                }
                else {
                }

                this.CalculateTotals();
            });
            // }, 6000);

        } else {
            console.error("No GLAccount for this payment ", this.EntityPM);

        }
    }

    fetchGLAccount()
    {
        return new Promise((resolve, reject) =>
        {

            var _glaId = this.billtoCard.GLAccountId;
            this.StartBusyIndicator('fetchGLAccount');
            this._glaService.getSingle(_glaId)
                .subscribe((response: ServiceResponse) =>
                {

                    var res: ServiceResponse = response;
                    if (!res.HasError) {
                        var glaccount = res.Result;

                        resolve(glaccount);
                        this.StopBusyIndicator('fetchGLAccount');
                    }
                    else {
                        reject();

                        console.error(res.ErrorsArray);
                        this.StopBusyIndicator('fetchGLAccount');
                    }
                });

        });
    }

    billtoCard;
    fetchBillToCard()
    {
        return new Promise((resolve, reject) =>
        {

            var myService: CardListService = new CardListService();
            myService.getSingle(this.BillToId).subscribe((resp: ServiceResponse) =>
            {
                if (resp != null) {
                    if (!resp.HasError) {
                        var cardList = resp.Result;
                        if (cardList != null) {
                            this.billtoCard = cardList;
                            resolve(cardList);
                        }
                    }
                }
            });

        });
    }

    AdjustedAmount: number = 0;
    CalculateTotals()
    {

        // Reconciliation amount
        var _linesAmount2reco = 0;
        var _linespaymentReconciledAmount = 0;
        this.TransactionsList.Collection.forEach((line: TransactionLineModel) =>
        {
            if (line) { // && line.AmountToReconcile >= 0) {
                _linesAmount2reco += line.AmountToReconcile;
                _linespaymentReconciledAmount += line.PaymentReconciledAmount;
            }
        });

        this.amount2reconcileTotal = _linesAmount2reco;
        this.paymentReconciledAmountTotal = _linespaymentReconciledAmount;
        //if (this.EntityPM.GLAccountRecoMethodCode == "1") {
        //    this.amount2reconcileTotal = _linesAmount2reco * this.EntityPM.PaymentCurrencyExchangeRate;
        //    this.paymentReconciledAmountTotal = _linespaymentReconciledAmount * this.EntityPM.PaymentCurrencyExchangeRate;
        //}
        this.AdjustedAmount = this.paymentReconciledAmountTotal == 0 ? this.amount2reconcileTotal : this.paymentReconciledAmountTotal + this.amount2reconcileTotal;
        if (this.EntityPM.InvoicesLedgerTransactions.length == 0) {
            // this.EntityPM.OpenAmount = this.originalPaymentOpenAmount;
            // this.EntityPM.IsDirty = false;
        } else {
            // Open Amount
            var _openAmount = this.paymentAmountTotal - _linesAmount2reco;
            if (this.EntityPM.OpenAmount != _openAmount) {
                // this.EntityPM.OpenAmount = _openAmount < 0 ? 0 : _openAmount;
            }
        }

    }
    OpenSource(id: string, sourceTypeCode: string)
    {

        var tableName = AccountingEntityHelper.getEntityObjectTableName(sourceTypeCode);;
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef =>
            {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({
                    EntityId: id,
                    ObjectTableName: tableName
                });
            });

    }
    OpenJournal(id)
    {
        if (!AppTool.IsNullOrEmpty(id)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef =>
                {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: id, ObjectTableName: 'Journal', BackButtonLabel: 'Back' });
                    cmpRef.instance.BackCompleted.subscribe(bk =>
                    {
                    });
                });
        }
    }
    OpenReco(recoNumber)
    {
        if (!AppTool.IsNullOrEmpty(recoNumber)) {

            this.StartBusyIndicator('OpenReco');

            this._ReconciliationExtendedPMService.getByNumber(recoNumber)
                .subscribe((myResult: ServiceResponse) =>
                {
                    this.StopBusyIndicator('OpenReco');

                    var mm: ServiceResponse = myResult;
                    if (!mm.HasError) {

                        var reco: any = mm.Result;
                        var recoId = reco.Id;

                        // this.showAlert = false;
                        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                            .then(cmpRef =>
                            {
                                cmpRef.instance.ComponentRef = cmpRef;
                                cmpRef.instance.Run({ EntityId: recoId, ObjectTableName: 'Reconciliation' });
                                cmpRef.instance.BackCompleted.subscribe(bk =>
                                {
                                    // this.CurrentSession.CloseCurrentWindow();
                                    this.GetData();
                                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                                });
                            });
                    }
                    else {

                    }
                });




        }
    }
    PushTransaction(trans: LedgerTransactionPM)
    {
        if (trans != null) {
            var index = this.EntityPM.InvoicesLedgerTransactions.indexOf(trans);
            if (index == -1) {
                this.EntityPM.IsDirty = true;
                this.EntityPM.InvoicesLedgerTransactions.push(trans);
            }
        }
    }
    PopTransaction(trans: LedgerTransactionPM)
    {
        if (trans != null) {
            var index = this.EntityPM.InvoicesLedgerTransactions.indexOf(trans);
            if (index > -1) {
                // this.EntityPM.IsDirty = true;
                this.EntityPM.InvoicesLedgerTransactions.splice(index, 1);
            }

        }
    }
    //#endregion

    //#region old code
    public AllMethods: AccountingPaymentMethodList[] = [];
    LoadPaymentMethods()
    {
        var myService: AccountingPaymentMethodListService = new AccountingPaymentMethodListService();
        myService.getAll().subscribe((response: ServiceResponse) =>
        {
            if (response != null) {
                this.AllMethods = response.Result;
            }
        });
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen()
    {
        if (this.entityArgs.EditComponent != null) {

            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) =>
            {
                console.log('[EVENT] SaveCompleted', isSaveSuccess);

                if (isSaveSuccess) {
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.SetUIProperties();
                    // this.GetData();

                    this.checkLedgerCreated();
                }



                // if (this.RequestedCommandCode) {
                //     this.ApplyRequestedCommand();
                // }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) =>
            {
                console.log('[EVENT] LoadCompleted', isLoadSuccess);
                if (isLoadSuccess) {


                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.SetUIProperties();
                    this.GetData();
                }
            });
        }
    }

    // UIProperties
    public RateIsEnabled: boolean = true;
    get IsScreenEnabled()
    {
        var result = true;

        if (this.EntityPM != null) {
            if (AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "DR") {
                result = true;
            }

            else {
                result = false;
            }
        }

        return result;
    }
    SetUIProperties()
    {
        this.SetUIProperties_Cheque();
        this.SetUIProperties_Invoices();
        this.SetUIProperties_CreditCard();
        this.SetUIProperties_BankTransfer();
        this.SetUIProperties_ValueDate();
        this.GetRateIsEnabled();


        if (!this.IsScreenEnabled) {
            this.DisabledPartnerTypes = true;
            this.UIProperties.SetEnabled("AccountingPaymentMethodId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("AmountInPaymentCurrency", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("RegisterDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ValueDate", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ChequeOrPaymentRef", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("Bank", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("BankBranch", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("Account", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("CreditCardTypeId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("BranchId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("ChequeAmount", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("BankTransferAmount", this.ObjectTableName, false);
            if (this.isFullAccounting) {
                this.UIProperties.SetEnabled("BankAccountId", this.ObjectTableName, false);
            }
        }

        else {
            this.DisabledPartnerTypes = false;
            this.UIProperties.SetEnabled("AccountingPaymentMethodId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("AmountInPaymentCurrency", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("RegisterDate", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("ValueDate", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("ChequeOrPaymentRef", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("Bank", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("BankBranch", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("Account", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("PrintNotes", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("CreditCardTypeId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("BranchId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("ChequeAmount", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("BankTransferAmount", this.ObjectTableName, true);

            if (AppTool.IsNullOrEmpty(this.EntityPM.BillToId) && !SessionLocator.TenantPM.AccountingActivated) {
                this.UIProperties.SetEnabled("BillToAddressId", this.ObjectTableName, false);
            }

            if (this.EntityPM.StatusCode == "VD") {
                this.UIProperties.SetEnabled("PrintNotes", this.ObjectTableName, false);
            }
            if (this.isFullAccounting) {
                this.UIProperties.SetEnabled("BankAccountId", this.ObjectTableName, true);
            }
            if(this.EntityPM.ForceUsingBankTransferMethod)
                this.UIProperties.SetEnabled("AccountingPaymentMethodId", this.ObjectTableName, false);

            if(this.EntityPM.BankTransferPaymentArguments){
                if(this.EntityPM.BankTransferPaymentArguments.ValueDate)
                    this.UIProperties.SetEnabled("ValueDate", this.ObjectTableName, false);

                if(this.EntityPM.BankAccountId)
                    this.UIProperties.SetEnabled("BankAccountId", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("BankTransferAmount", this.ObjectTableName, false);
            }

        }
        this.SetBankRequired();
    }

    SetBankRequired()
    {

        if (this.EntityPM.AccountingPaymentMethodCode == "CH" && this.Bank == null) {

            this.UIProperties.SetRequired("Bank", this.ObjectTableName, true);
        }

    }
    SetUIProperties_Invoices()
    {
        if (!this.IsScreenEnabled) {
            this.UIProperties.SetEnabled("BillToId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("BillToAddressId", this.ObjectTableName, false);
            this.UIProperties.SetEnabled("PaymentCurrencyId", this.ObjectTableName, false);
        }

        else {
            this.UIProperties.SetEnabled("BillToId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("PaymentCurrencyId", this.ObjectTableName, true);
            this.UIProperties.SetEnabled("BillToAddressId", this.ObjectTableName, true);

            if (this.EntityPM.PaymentInvoices.length > 0) {
                this.UIProperties.SetEnabled("BillToId", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("BillToAddressId", this.ObjectTableName, false);
                this.UIProperties.SetEnabled("PaymentCurrencyId", this.ObjectTableName, false);
            }
        }

        this.SetUIProperties_ExchangeRate();
    }
    SetUIProperties_ExchangeRate()
    {
        var isEnabled: boolean = false;

        if (this.IsScreenEnabled) {


            if (!this.isFullAccounting) {
                if (FeatureLocator.HasFeaturePermession(this.ObjectTableName, "ARPaymentEditExchangeRate")) {
                    if (this.EntityPM.PaymentInvoices.length > 0) {
                        isEnabled = false;
                    }
                    else {
                        isEnabled = true;
                    }
                }
            }
            if (this.PaymentCurrencyId == SessionLocator.TenantPM.CurrencyId) {
                isEnabled = false;
            }
            else {
                isEnabled = true;
            }
        }


        this.UIProperties.SetEnabled("PaymentCurrencyExchangeRate", this.ObjectTableName, isEnabled && !this.IsRateDisabled);
        this.UIProperties.SetEnabled("ExchangeRateDate", this.ObjectTableName, isEnabled );
    }
    SetUIProperties_Cheque()
    {
        if (this.isFullAccounting && this.AccountingPaymentMethodCode == "CH") {
            this.UpdatePaymentChequeFields();
            this.UIProperties.SetRequired("BankBranch", this.ObjectTableName, AppTool.IsNullOrEmpty(this.BankBranch));
            this.UIProperties.SetRequired("Account", this.ObjectTableName, AppTool.IsNullOrEmpty(this.Account));
            var service: InvoiceDomainService = new InvoiceDomainService();
            service.GetStatusOfARPaymentCheques(this.EntityPM.Id).subscribe((myResponse: ServiceResponse) =>
            {
                if (myResponse != null && !myResponse.HasError) {
                    this.ARPaymentChequeStatus = myResponse.Result;
                    if (this.ARPaymentChequeStatus == "בקופה" || this.ARPaymentChequeStatus == "משמרת" || this.ARPaymentChequeStatus == "הופקד- טרם נפרע") {
                        this.ARPaymentChequeStatusColor = "orange";
                    }
                    else if (this.ARPaymentChequeStatus == "הוחזר ללקוח") {
                        this.ARPaymentChequeStatusColor = "red";
                    }
                    else if (this.ARPaymentChequeStatus == "נפרע") {
                        this.ARPaymentChequeStatusColor = "green";
                    }
                }
            });
        }
        else {
            this.UIProperties.SetRequired("BankBranch", this.ObjectTableName, false);
            this.UIProperties.SetRequired("Account", this.ObjectTableName, false);
        }

        this.UIProperties.SetRequired("ChequeOrPaymentRef", this.ObjectTableName, false);

        if (!AppTool.IsNullOrEmpty(this.AccountingPaymentMethodCode)) {
            if (this.AccountingPaymentMethodCode == "CH") {
                if (AppTool.IsNullOrEmpty(this.ChequeOrPaymentRef)) {
                    this.UIProperties.SetRequired("ChequeOrPaymentRef", this.ObjectTableName, true);
                }
                if (AppTool.IsNullOrEmpty(this.Bank)) {
                    this.UIProperties.SetRequired("Bank", this.ObjectTableName, true);
                }
                //this.UIProperties.SetVisibility("Bank", this.ObjectTableName, true);
                //this.UIProperties.SetVisibility("BankBranch", this.ObjectTableName, true);
                //this.UIProperties.SetVisibility("Account", this.ObjectTableName, true);
                this.BankFieldsVisibile = true;
            }
        }
    }
    SetUIProperties_CreditCard()
    {
        this.UIProperties.SetVisibility("CreditCardTypeId", this.ObjectTableName, false);
        this.CreditCardTypeIdVisibility = false;
        this.UIProperties.SetRequired("CreditCardTypeId", this.ObjectTableName, false);
        if (this.AccountingPaymentMethodCode == "CC") {

            if (AppTool.IsNullOrEmpty(this.CreditCardTypeId)) {
                this.UIProperties.SetRequired("CreditCardTypeId", this.ObjectTableName, true);
            }
            this.UIProperties.SetVisibility("CreditCardTypeId", this.ObjectTableName, true);
            //this.UIProperties.SetVisibility("Bank", this.ObjectTableName, true);
            //this.UIProperties.SetVisibility("BankBranch", this.ObjectTableName, true);
            //this.UIProperties.SetVisibility("Account", this.ObjectTableName, true);
            this.BankFieldsVisibile = true;
            this.CreditCardTypeIdVisibility = true;
        }
    }
    SetUIProperties_BankTransfer()
    {
        this.UIProperties.SetRequired("BankAccountId", this.ObjectTableName, false);
        if (this.isFullAccounting == true && this.AccountingPaymentMethodCode == "BT") {
            if (AppTool.IsNullOrEmpty(this.BankAccountId)) {
                this.UIProperties.SetRequired("BankAccountId", this.ObjectTableName, true);
            }

            this.UIProperties.SetVisibility("BankAccountId", this.ObjectTableName, true);
            if (!this.isFullAccounting) {
                //this.UIProperties.SetVisibility("Bank", this.ObjectTableName, true);
                //this.UIProperties.SetVisibility("BankBranch", this.ObjectTableName, true);
                //this.UIProperties.SetVisibility("Account", this.ObjectTableName, true);
                this.BankFieldsVisibile = true;
            }
            else {
                //this.UIProperties.SetVisibility("Bank", this.ObjectTableName, false);
                //this.UIProperties.SetVisibility("BankBranch", this.ObjectTableName, false);
                //this.UIProperties.SetVisibility("Account", this.ObjectTableName, false);
                this.BankFieldsVisibile = false;
            }
            this.BankAccountIdVisibility = true;

            this.UIProperties.SetValidity("BranchId", this.ObjectTableName, true, "");
            this.UIProperties.SetRequired("BranchId", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetVisibility("BankAccountId", this.ObjectTableName, false);
            this.BankAccountIdVisibility = false;
        }
    }
    GetRateIsEnabled()
    {
        var result = true;
        if (this.EntityPM != null) {
            if (this.EntityPM.PaymentCurrencyId == SessionLocator.TenantPM.CurrencyId || this.EntityPM.PaymentCurrencyId == null || SessionLocator.TenantPM.CurrencyId == null
                || this.IsRateDisabled) {
                result = false;
            }
        }

        this.SetUIProperties_ExchangeRate();

        this.RateIsEnabled = result;
    }

    // Load Data
    public IsDataLoaded: boolean = false;
    private searchText: string = "";
    SearchTextKeyUp(args: any)
    {
        this.searchText = args;
        this.LoadData();
    }

    private ConnectedList: ARInvoiceList[] = [];
    private IsMatchedList: ARInvoiceList[] = [];
    LoadData()
    {

        this.ItemsSource.Clear();

        if (!AppTool.IsNullOrEmpty(this.BillToId) && this.EntityPM.StatusCode != "VD") {

            if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {

                if (this.EntityPM.PaymentInvoices.length == 1) {
                    this.LoadPaymentInvoices_Created();
                    //var iConnectedItem = new ARInvoiceList();
                    //iConnectedItem.Id = this.EntityPM.PaymentInvoices[0].ARInvoiceId;
                    //iConnectedItem.InvoiceCurrencyId = this.EntityPM.PaymentInvoices[0].ForeignCurrencyId;
                    //iConnectedItem.InvoiceCurrencyExchangeRate = this.EntityPM.PaymentInvoices[0].ExchangeRate;
                    //iConnectedItem.AmountInInvoiceCurrency = this.EntityPM.PaymentInvoices[0].ForeignAmount;
                    //iConnectedItem.AmountInLocalCurrency = this.EntityPM.PaymentInvoices[0].LocalAmount;
                    //iConnectedItem.Id = this.EntityPM.PaymentInvoices[0].ARInvoiceId;
                    //iConnectedItem.MetodoPagoCode = this.EntityPM.PaymentInvoices[0].ARInvoiceMetodoPagoCode;
                    //this.ConnectedList.push(iConnectedItem);
                }

                else {
                    this.LoadPaymentInvoices_IsMatched();
                }
            }

            else {
                this.LoadPaymentInvoices_Connected();
            }
        }

        else {
            this.UpdateSummary();
            this.IsDataLoaded = true;
        }
    }
    LoadPaymentInvoices_Created()
    {
        var invoiceId = this.EntityPM.PaymentInvoices[0].ARInvoiceId;

        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 1;
        filters.SortBy = "InvoiceDate";
        filters.SortDirection = "Descending";

        filters.addAdditionalFilter("Id", invoiceId, null, null, "Equals", false, false, false, "string");

        var myService = new ARInvoiceListService();
        myService.getByFilters(filters).subscribe((myResponse: ServiceResponse) =>
        {
            if (!myResponse.HasError) {

                this.ConnectedList = myResponse.Result;

                this.LoadPaymentInvoices_IsMatched();
            }
        });
    }
    LoadPaymentInvoices_Connected()
    {
        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 1000;
        filters.SortBy = "InvoiceDate";
        filters.SortDirection = "Descending";

        var searchValue = null;
        if (!AppTool.IsNullOrEmpty(this.searchText)) {
            searchValue = AppTool.IsNullOrEmpty(this.searchText.trim()) ? null : this.searchText;
        }

        filters.addAdditionalFilter("ARPaymentInvoicesSearch", searchValue, null, null, "Contains", true, false, false, "string");
        filters.addAdditionalFilter("ARPaymentInvoicesConnected", this.EntityPM.Id, null, null, "Contains", true, false, false, "string");

        var myService = new ARInvoiceListService();
        myService.getByFilters(filters).subscribe((myResponse: ServiceResponse) =>
        {
            if (!myResponse.HasError) {

                this.ConnectedList = myResponse.Result;

                if (this.EntityPM.IsClosed) {
                    this.FillBaselist();
                }

                else {
                    this.LoadPaymentInvoices_IsMatched();
                }
            }
        });
    }
    LoadPaymentInvoices_IsMatched()
    {
        var filters = new ApiQueryFilters();
        filters.PageIndex = 0;
        filters.PageSize = 1000;
        filters.SortBy = "InvoiceDate";
        filters.SortDirection = "Descending";

        filters.addAdditionalFilter("BillToId", this.EntityPM.BillToId, null, null, "Equals", false, false, false, "string");
        filters.addAdditionalFilter("StatusCode", "DR,AD,PP,PD", null, null, "InList", false, true, false, "string");
        filters.addAdditionalFilter("IsClosed", false, null, null, "Equals", false, false, false, "Boolean");
        filters.addAdditionalFilter("IsConstituentInvoice", false, null, null, "Equals", false, false, false, "Boolean");

        var searchValue = null;
        if (!AppTool.IsNullOrEmpty(this.searchText)) {
            searchValue = AppTool.IsNullOrEmpty(this.searchText.trim()) ? null : this.searchText;
        }

        filters.addAdditionalFilter("ARPaymentInvoicesSearch", searchValue, null, null, "Contains", true, false, false, "string");

        var myService = new ARInvoiceListService();
        myService.getByFilters(filters).subscribe((myResponse: ServiceResponse) =>
        {
            if (!myResponse.HasError) {
                this.IsMatchedList = myResponse.Result;
            }

            this.FillBaselist();
        });
    }
    FillBaselist()
    {
        // this.ItemsSource.Clear();

        // var connectedList: ARPaymentInvoiceArgs[] = [];
        // var unConnectedMatchedList: ARPaymentInvoiceArgs[] = [];
        // var unConnectedListNotMatched: ARPaymentInvoiceArgs[] = [];
        // var itemsCollection: ARPaymentInvoiceArgs[] = [];

        // if (this.ConnectedList.length > 0) {
        //     this.ConnectedList.forEach(item => {
        //         if (this.EntityPM.PaymentInvoices.filter(d => d.ARInvoiceId == item.Id)[0]) {

        //             if (AppTool.IsNullOrEmpty(this.EntityPM.Id)) {
        //                 if (this.EntityPM.AmountInPaymentCurrency > 0) {
        //                     var value = this.EntityPM.AmountInPaymentCurrency;

        //                     if (item.AmountDue > value) {
        //                         item.AmountDue = item.AmountDue - value;
        //                     }

        //                     else {
        //                         item.AmountDue = 0;
        //                     }
        //                 }
        //             }

        //             connectedList.push(new ARPaymentInvoiceArgs(item, this));
        //         }
        //     });

        //     connectedList.sort((a, b) => { return (a.SortingValue === b.SortingValue) ? 0 : (a.SortingValue < b.SortingValue) ? -1 : 1 }).forEach(item => {
        //         itemsCollection.push(item);
        //     });
        // }

        // if (!this.EntityPM.IsClosed) {
        //     this.IsMatchedList.forEach(item => {
        //         if (connectedList.filter(f => f.Id == item.Id).length == 0) {
        //             if (!item.IsClosed) {
        //                 var isCurrencyMatched: boolean = false;

        //                 if (this.PaymentCurrencyId == item.InvoiceCurrencyId) {
        //                     isCurrencyMatched = true;
        //                 }

        //                 else if (this.IsMultiCurrency) {
        //                     if (this.PaymentCurrencyId == SessionLocator.LocalCurrencyId) {
        //                         isCurrencyMatched = true;
        //                     }

        //                     else if (item.InvoiceCurrencyId == SessionLocator.LocalCurrencyId) {
        //                         isCurrencyMatched = true;
        //                     }
        //                 }

        //                 if (isCurrencyMatched == false || item.StatusCode == "DR") {
        //                     unConnectedListNotMatched.push(new ARPaymentInvoiceArgs(item, this));
        //                 }

        //                 else {
        //                     unConnectedMatchedList.push(new ARPaymentInvoiceArgs(item, this));
        //                 }
        //             }
        //         }
        //     });

        //     unConnectedMatchedList.filter(f => f.CurrencyId == this.PaymentCurrencyId).sort((a, b) => { return (a.SortingValue === b.SortingValue) ? 0 : (a.SortingValue < b.SortingValue) ? -1 : 1 }).forEach(item => {
        //         itemsCollection.push(item);
        //     });

        //     unConnectedMatchedList.filter(f => f.CurrencyId != this.PaymentCurrencyId).sort((a, b) => { return (a.SortingValue === b.SortingValue) ? 0 : (a.SortingValue < b.SortingValue) ? -1 : 1 }).forEach(item => {
        //         itemsCollection.push(item);
        //     });

        //     unConnectedListNotMatched.sort((a, b) => { return (a.SortingValue === b.SortingValue) ? 0 : (a.SortingValue < b.SortingValue) ? -1 : 1 }).forEach(item => {
        //         itemsCollection.push(item);
        //     });
        // }

        // this.ItemsSource.InsertCollection(itemsCollection);
        // this.UpdateSummary();
        // this.IsDataLoaded = true;
    }
    AllowedPartnerTypesCodes: string[] = ['CS','AG','AC','AL','CG','SG','SL','TR','VD','WH'];
    filterByPartnerTypeCode: string;
    isPartnerTypesFilterEnabled: boolean = false;
    getPartnerTypes(){
        this._PartnerTypeListService.getAll().subscribe((res:ServiceResponse)=>
        {
            var partnerTypes: PartnerTypeList[] = res.Result || [];
            this.PartnerTypes = partnerTypes.filter(d => this.AllowedPartnerTypesCodes.indexOf(d.Id) > -1); // filter
            if( this.EntityPM.StatusCode=="DR")
                this.SelectedPartnerType = partnerTypes.filter(d => d.Id == 'CS')[0]; // default
            this.getSelectedPartnerTypes(partnerTypes);
        });
    }

    getSelectedPartnerTypes(partnerTypes){
        var myCardService = new CardListService;
        myCardService.getSingle(this.EntityPM.BillToId).subscribe((myResponse: ServiceResponse) => {
            if (myResponse != null) {
                if (!myResponse.HasError) {
                    var list: CardList = myResponse.Result;
                    this.SelectedPartnerType = partnerTypes.filter(d => d.Id == list.PartnerTypeId)[0];
                }
            }
        });

    }
    private _SelectedPartnerType : PartnerTypeList;
    public get SelectedPartnerType() : PartnerTypeList {
        return this._SelectedPartnerType;
    }
    public set SelectedPartnerType(type : PartnerTypeList) {
        this._SelectedPartnerType = type;
        this.filterByPartnerTypeCode = type.Id;

    }

    // BillTo
    get BillToId()
    {
        if (this.EntityPM == null) {
            return null;
        }
        return this.EntityPM.BillToId;
    }
    set BillToId(value: string)
    {
        if (this.EntityPM != null) {
            if (this.EntityPM.BillToId != value) {
                this.EntityPM.BillToId = value;

        if (value) {
            console.log('[!] BillTo changed, reload GLAccount.  ', value);
            this.ReloadGLAccount();
        }

        if(AppTool.IsNullOrEmpty(this.EntityPM.Id))
            this.GetCardProperties();

        else
        {

            if (AppTool.IsNullOrEmpty(this.EntityPM.BillToId)) {
                this.UIProperties.SetEnabled("BillToAddressId", this.ObjectTableName, false);
            }

            else {
                this.UIProperties.SetEnabled("BillToAddressId", this.ObjectTableName, true);
            }

            this.LoadData();

            if (AppTool.IsNullOrEmpty(this.EntityPM.BillToId)) {
                this.BillToAddressId = null;
                this.EntityPM.BillToName = null;
                this.EntityPM.BillToPartnerTypeId = null;
                this.PaymentCurrencyId = SessionLocator.TenantPM.CurrencyId;
            }

            else {
                var myService: CardListService = new CardListService();
                myService.getSingle(this.EntityPM.BillToId).subscribe((myResponse: ServiceResponse) =>
                {
                    if (!myResponse.HasError) {
                        var list: CardList = myResponse.Result;
                        if (list) {
                            this.EntityPM.BillToName = list.EnglishName;
                            this.EntityPM.BillToPartnerTypeId = list.PartnerTypeId;
                            this.EntityPM.BillToAddressId = list.Id;


                            if (!AppTool.IsNullOrEmpty(list.InvoiceCurrencyId)) {
                                this.PaymentCurrencyId = list.InvoiceCurrencyId;
                            }

                            this.LoadAddress();
                            if (this.isFullAccounting) {
                                if (!AppTool.IsNullOrEmpty(list.GLAccountId)) {
                                    var myGLAccountPMService = new GLAccountPMService();
                                    myGLAccountPMService.get(list.GLAccountId).subscribe((myResponse: ServiceResponse) =>
                                    {
                                        if (!myResponse.HasError) {
                                            var glaccount: GLAccountPM = myResponse.Result;
                                            this.EntityPM.GLAccountRecoMethodCode = glaccount.ReconcileMethodCode;
                                            this.EntityPM.GLAccountCurrencyCode = glaccount.CurrencyCode;
                                            this.SetAmountCurrencyCode();
                                            this.ComputeLocalAmount();
                                            this.SetPaymentAmount();

                                            if (glaccount != null && !glaccount.IsMultiCurrency) {
                                                this.PaymentCurrencyId = glaccount.CurrencyId;
                                            }
                                        }
                                    });
                                }
                            }
                        }
                    }
                });
            }
        }


            }
        }
    }

    get BillToAddressId() { return this.EntityPM.BillToAddressId; }
    set BillToAddressId(value: string)
    {
        if (this.EntityPM != null) {
            if (this.EntityPM.BillToAddressId != value) {
                this.EntityPM.BillToAddressId = value;
            }
        }
    }

    LoadAddress()
    {
        var myService: PartnersDomainService = new PartnersDomainService();
        myService.GetBillingAddressListByCardId(this.EntityPM.BillToId).subscribe((resp: any) =>
        {
            if (resp != null) {
                var billingAddress = resp;
                if (billingAddress != null) {
                    this.BillToAddressId = billingAddress.Id;
                }
                else {
                    var myService: AddressListService = new AddressListService();
                    myService.getSingle(this.EntityPM.BillToId).subscribe((myResult: any) =>
                    {
                        var myResponse: ServiceResponse = myResult;
                        if (!myResponse.HasError) {
                            var billingAddress: AddressList = myResponse.Result;
                            if (billingAddress != null) {
                                this.BillToAddressId = billingAddress.Id;
                            }
                            else {
                                var myService: PartnersDomainService = new PartnersDomainService();
                                myService.GetMainAddressListByCardId(this.EntityPM.BillToId).subscribe((resp: any) =>
                                {
                                    if (resp != null) {
                                        var mainAddress = resp;
                                        if (mainAddress != null) {
                                            this.BillToAddressId = mainAddress.Id;
                                        }

                                        else {
                                            this.BillToAddressId = null;
                                        }
                                    }
                                });

                            }
                        }
                    });
                }

            }
        });
    }

/******************************************* new ARPayment - BillToAddressId ******************************************************************/
    GetCardProperties() {
        if (AppTool.IsNullOrEmpty(this.EntityPM.BillToId)) {
            this.FillDataFromCardList(new CardList());
        }

        else {
            var myService: CardListService = new CardListService();
            myService.getSingle(this.EntityPM.BillToId).subscribe((resp: ServiceResponse) => {
                if (resp != null) {
                    if (!resp.HasError) {
                        var cardList = resp.Result;
                        if (cardList != null) {
                            this.billtoCard = cardList;
                            this.FillDataFromCardList(cardList);
                        }
                    }
                }
            });
        }
    }

    FillDataFromCardList(list: CardList) {
        if (list == null) {
            this.BillToAddressId = null;
            this.EntityPM.BillToName = null;
            this.AccountingPaymentMethodId = null;
            this.AccountingPaymentMethodCode = null;
            this.SATPaymentMethodCode = null;
            if(!this.EntityPM.BankTransferPaymentArguments || (this.EntityPM.BankTransferPaymentArguments && !this.EntityPM.BankTransferPaymentArguments.CurrencyId))
                this.PaymentCurrencyId = SessionLocator.TenantPM.CurrencyId;
            this.EntityPM.BillToPartnerTypeId = null;
        }

        else {
            if(!AppTool.IsNullOrEmpty(list.InvoiceCurrencyId) && (
                !this.EntityPM.BankTransferPaymentArguments || (this.EntityPM.BankTransferPaymentArguments && !this.EntityPM.BankTransferPaymentArguments.CurrencyId)
                )){
                this.PaymentCurrencyId = list.InvoiceCurrencyId;
            }
            if (!AppTool.IsNullOrEmpty(list.SATPaymentMethodCode)) {
               // this.ARPaymentMethodCode = list.SATPaymentMethodCode;
            }

            this.EntityPM.BillToPartnerTypeId = list.PartnerTypeId;
            this.EntityPM.BillToName = list.EnglishName;
            this.LoadAddressForNewARPayment();

            if (SessionLocator.TenantPM.AccountingActivated == true) {
                if (!AppTool.IsNullOrEmpty(list.GLAccountId)) {
                    var myGLAccountPMService = new GLAccountPMService();
                    myGLAccountPMService.get(list.GLAccountId).subscribe((myResponse: ServiceResponse) => {
                        if (!myResponse.HasError) {
                            var glaccount: GLAccountPM = myResponse.Result;
                            if (glaccount != null && !glaccount.IsMultiCurrency && (this.EntityPM.BankTransferPaymentArguments && !this.EntityPM.BankTransferPaymentArguments.CurrencyId)) {
                                this.PaymentCurrencyId = glaccount.CurrencyId;
                            }
                        }
                    });
                }
            }
            //if (!AppTool.IsNullOrEmpty(list.SATPaymentMethodCode)) {
            //    this.SATPaymentMethodCode = list.SATPaymentMethodCode;
            //}
        }
    }

    LoadAddressForNewARPayment() {
        var myService: PartnersDomainService = new PartnersDomainService();
        myService.GetAddressByCardAndType(this.EntityPM.BillToId, "B").subscribe((resp: any) => {
            var billingAddress = resp;
            if (billingAddress != null) {
                var item = billingAddress;
                this.BillToAddressId = item.Id;
            }

            else {
                this.GetBillingAddress();
            }

        });
    }

    GetBillingAddress() {
        var myService: PartnersDomainService = new PartnersDomainService();
        myService.GetBillingAddressListByCardId(this.EntityPM.BillToId).subscribe((resp: any) => {
            var billingAddress = resp;
            if (billingAddress != null) {
                this.BillToAddressId = billingAddress.Id;
            }

            else {
                var myService: PartnersDomainService = new PartnersDomainService();
                myService.GetAddressByCardAndType(this.EntityPM.BillToId, "M").subscribe((resp: any) => {
                    if (resp != null) {
                        var mainAddress = resp;
                        if (mainAddress != null) {
                            var item = mainAddress;
                            this.BillToAddressId = item.Id;
                        }
                        else {

                            this.GetMainAddressListByCardId();
                        }
                    }
                });
            }
        });
    }

    GetMainAddressListByCardId() {
        var myService: PartnersDomainService = new PartnersDomainService();
        myService.GetMainAddressListByCardId(this.EntityPM.BillToId).subscribe((resp: any) => {
            if (resp != null) {
                var mainAddress = resp;
                if (mainAddress != null) {
                    this.BillToAddressId = mainAddress.Id;
                }

                else {
                    this.BillToAddressId = null;
                }
            }
        });
    }

/************************************************************************************************************/


    // Currency
    get PaymentCurrencyId() { 
        return this.EntityPM.PaymentCurrencyId;
     }
    set PaymentCurrencyId(value: string)
    {
        if (this.EntityPM.PaymentCurrencyId != value) {
            this.EntityPM.PaymentCurrencyId = value;
            this.CheckARPaymentCashBook();
            this.PaymentCurrencyExchangeRate = this.GetCurrencyRate(value);
            this.ExchangeRateDate = this.GetCurrencyRateDate(value);

            if (AppTool.IsNullOrEmpty(value)) {
                this.PaymentCurrencyCode = null;
            }

            else {
                var myService = new CurrencyListService();
                myService.getSingleFromCache(value).subscribe((myResponse: ServiceResponse) =>
                {
                    if (!myResponse.HasError) {
                        var list: CurrencyList = myResponse.Result;
                        if (list) {
                            this.PaymentCurrencyCode = list.Code;
                            this.PaymentCurrencySign = list.Sign;
                            this.EntityPM.PaymentCurrencySign = list.Sign;
                        }
                    }
                });
            }

            this.SetUIProperties_ExchangeRate();
            this.GetData();
            this.ItemsSource.Collection.forEach(item =>
            {
                item.SetUIProperties();
                item.InitExchangeRate();
            });

            if (this.isFullAccounting == true && this.AccountingPaymentMethodCode == "BT") {
                this.CheckGLAccountCurrencyId();
            }
            this.filterBankAccountsUsingPaymentCurrencyId();
        }
    }
    filterBankAccountsUsingPaymentCurrencyId(){
        this.BankAccountsFilterItems = new ApiQueryFilters();
        if (this.isFullAccounting == true && this.AccountingPaymentMethodCode == "BT" && this.EntityPM.PaymentCurrencyId) {
            this.BankAccountsFilterItems.addAdditionalFilter("CurrencyId", this.EntityPM.PaymentCurrencyId, null, null, "Equals", false, false, false, "string");
            this.loadBankAccounts(this.BankAccountsFilterItems);
        }
    }
    get PaymentCurrencyCode() { return this.EntityPM.PaymentCurrencyCode; }
    set PaymentCurrencyCode(value: string)
    {
        if (this.EntityPM.PaymentCurrencyCode != value) {
            this.EntityPM.PaymentCurrencyCode = value;
        }
    }

    get PaymentCurrencyExchangeRate() { return this.EntityPM.PaymentCurrencyExchangeRate; }
    set PaymentCurrencyExchangeRate(value: number)
    {
        if (this.EntityPM != null) {
            if (this.EntityPM.PaymentCurrencyExchangeRate != value) {
                this.EntityPM.PaymentCurrencyExchangeRate = AppTool.Round(value, 5);
                this.GetRateIsEnabled();
                this.ComputeLocalAmount();
                this.SetPaymentAmount();
                this.ComputeOpenAmountInLocalCurrency();
                this.ItemsSource.Collection.forEach(item =>
                {
                    item.InitExchangeRate();
                });
            }
        }
    }

    get ExchangeRateDate() { return this.EntityPM.ExchangeRateDate; }
    set ExchangeRateDate(value: Date)
    {
        if (this.EntityPM != null) {
            if (this.EntityPM.ExchangeRateDate != value) {
                this.EntityPM.ExchangeRateDate = value;
                this.ComputeRelativeRateDate();
            }
        }
    }

    private myRelativeRateDate: string = null;
    get RelativeRateDate() { return this.myRelativeRateDate; }
    set RelativeRateDate(value: string)
    {
        if (this.myRelativeRateDate != value) {
            this.myRelativeRateDate = value;
        }
    }
    ComputeRelativeRateDate()
    {
        this.RelativeRateDate = DateTool.GetRelativeRateDate(this.RegisterDate, this.ExchangeRateDate, "old");
    }

    public LastRatesList: LastRate[] = [];
    LoadCurrencyRates()
    {
        var loadingDate = this.EntityPM.RegisterDate;
        if (loadingDate == null) {
            loadingDate = DateTool.GetCurrentDateAsUtc();
        }

        var myService: CurrencyRatesService = new CurrencyRatesService();
        myService.GetCurrenciesExchangeRateByValueDate(SessionLocator.TenantPM.CurrencyId, loadingDate).subscribe((myResponse: ServiceResponse) =>
        {
            if (!myResponse.HasError) {
                this.LastRatesList = myResponse.Result;
            }

            this.LoadData();
        });
    }
    UpdateCurrencyRates()
    {
        var loadingDate = this.RegisterDate;
        if (loadingDate == null) {
            loadingDate = DateTool.GetCurrentDateAsUtc();
        }

        var myService: CurrencyRatesService = new CurrencyRatesService();
        myService.GetCurrenciesExchangeRateByValueDate(SessionLocator.TenantPM.CurrencyId, loadingDate).subscribe((myResponse: ServiceResponse) =>
        {
            if (!myResponse.HasError) {
                this.LastRatesList = myResponse.Result;

                this.SetCurrencyRateData();
            }
        });
    }
    SetCurrencyRateData()
    {
        var myRate: number = null;
        var myRateDate: Date = null;

        if (!AppTool.IsNullOrEmpty(this.PaymentCurrencyId)) {
            if (this.PaymentCurrencyId == SessionLocator.TenantPM.CurrencyId) {
                myRate = 1;
            }

            else {
                var lastRate: LastRate = this.LastRatesList.filter(d => d.ForeignCurrencyId == this.PaymentCurrencyId)[0];
                if (lastRate != null) {
                    myRate = lastRate.Rate;
                    myRateDate = lastRate.ValueDate;
                }
            }
        }

        this.PaymentCurrencyExchangeRate = myRate;
        this.ExchangeRateDate = myRateDate;
    }
    GetCurrencyRate(currencyId: string): number
    {
        var result = null;

        if (AppTool.IsNullOrEmpty(currencyId)) {
            result = null;
        }

        else {
            if (currencyId == SessionLocator.TenantPM.CurrencyId) {
                result = 1;
            }

            else {
                if (this.LastRatesList) {
                    var lastRate: LastRate = this.LastRatesList.filter(d => d.ForeignCurrencyId == currencyId)[0];
                    if (lastRate != null) {
                        result = lastRate.Rate;
                    }
                }
            }
        }

        return result;
    }
    GetCurrencyRateDate(currencyId: string): Date
    {

        var result = null;

        if (!AppTool.IsNullOrEmpty(currencyId)) {
            if (currencyId == SessionLocator.TenantPM.CurrencyId) {
                result = null;
            }

            else {
                if (this.LastRatesList) {
                    var lastRate: LastRate = this.LastRatesList.filter(d => d.ForeignCurrencyId == currencyId)[0];
                    if (lastRate != null) {
                        result = lastRate.ValueDate;
                    }
                }
            }
        }

        return result;
    }

    get RegisterDate()
    {
        if (this.EntityPM == null) {
            return null;
        }
        return this.EntityPM.RegisterDate;
    }
    set RegisterDate(value: Date)
    {
        if (this.EntityPM != null) {
            if (this.EntityPM.RegisterDate != value) {
                this.EntityPM.RegisterDate = value;
                this.UpdateCurrencyRates();
            }
        }
    }

    get AccountingPaymentMethodId()
    {
        if (this.EntityPM == null) {
            return null;
        }
        return this.EntityPM.AccountingPaymentMethodId;
    }
    set AccountingPaymentMethodId(value: string)
    {
        if (this.EntityPM != null) {
            if (this.EntityPM.AccountingPaymentMethodId != value) {
                this.EntityPM.AccountingPaymentMethodId = value;

                this.RefreshPaymentMethodFields();
                //this.CheckARPaymentCashBook();
            }
        }
    }

    get AccountingPaymentMethodCode()
    {
        if (this.EntityPM == null) {
            return null;
        }
        return this.EntityPM.AccountingPaymentMethodCode;
    }
    set AccountingPaymentMethodCode(value: string)
    {
        if (this.EntityPM != null) {
            if (this.EntityPM.AccountingPaymentMethodCode != value) {
                this.EntityPM.AccountingPaymentMethodCode = value;

                this.CheckARPaymentCashBook();
                this.filterBankAccountsUsingPaymentCurrencyId();
            }
        }
    }

    get SATPaymentMethodCode()
    {
        if (this.EntityPM == null) {
            return null;
        }
        return this.EntityPM.SATPaymentMethodCode;
    }
    set SATPaymentMethodCode(value: string)
    {
        if (this.EntityPM != null) {
            if (this.EntityPM.SATPaymentMethodCode != value) {
                this.EntityPM.SATPaymentMethodCode = value;

            }
        }
    }

    get BranchId()
    {
        if (this.EntityPM == null) {
            return null;
        }
        return this.EntityPM.BranchId;
    }
    set BranchId(value: string)
    {
        if (this.EntityPM != null) {
            if (this.EntityPM.BranchId != value) {
                this.EntityPM.BranchId = value;
                this.CheckARPaymentCashBook();
            }
        }
    }

    public CashBookName: string;
    public BranchGLAccountNumber: string;
    public IsCashBookValid: boolean = false;
    public BranchGLAccountId: string;
    CheckARPaymentCashBook()
    {
        this.IsCashBookValid = false;

        if (this.isFullAccounting && (this.AccountingPaymentMethodCode == "CA" || this.AccountingPaymentMethodCode == "CH")) {
            var service: InvoiceDomainService = new InvoiceDomainService();

            service.CheckARPaymentCashBook(this.AccountingPaymentMethodCode, this.PaymentCurrencyId, this.BranchId).subscribe((myResponse: ServiceResponse) =>
            {
                if (myResponse != null && !myResponse.HasError) {
                    var cashbooks: CashBookPM[] = myResponse.Result;
                    if (cashbooks != null && cashbooks.length > 0) {
                        if (cashbooks.length == 1) {

                            var _cashbook: CashBookPM = cashbooks[0];

                            this.setCashbookFields(_cashbook);

                            if (AppTool.IsNullOrEmpty(this.EntityPM.CashbookId)) {
                                this.EntityPM.CashbookId = _cashbook.Id;
                            }
                            this.UIProperties.SetValidity("BranchId", this.ObjectTableName, true, "");
                        }
                        else {
                            this.setBranchesText(cashbooks);
                        }
                    }
                    else {
                        var msg = "There is no cashbook that compatible to this ARPayment, create one please";
                        this.UIProperties.SetValidity("BranchId", this.ObjectTableName, false, msg);
                    }
                }
            });
        }
    }

    private setBranchesText(cashbooks: CashBookPM[])
    {
        var branchesText = "";
        cashbooks.forEach((item: CashBookPM) =>
        {
            if (!AppTool.IsNullOrEmpty(item.EnglishName)) {
                branchesText += item.EnglishName + ",";
            }
        });
        branchesText = branchesText.replace(/,\s*$/, "");
        var msg2 = "There is no cashbook for this branch, Cashbooks for branches: " + branchesText + "  was found, change the branch please";
        this.UIProperties.SetValidity("BranchId", this.ObjectTableName, false, msg2);
    }

    private setCashbookFields(_cashbook: CashBookPM)
    {
        this.CashBookName = this.showLocal ? _cashbook.LocalName : _cashbook.EnglishName;
        this.BranchGLAccountNumber = _cashbook.AccountNumber;
        this.BranchGLAccountId = _cashbook.AccountId;
        this.IsCashBookValid = true;
    }

    //Payment Line Properties
    private RefreshPaymentMethodFields()
    {
        this.Bank = null;
        this.BankBranch = null;
        this.Account = null;
        // this.ChequeOrPaymentRef = null;
        //   this.ValueDate = null;
        this.CreditCardTypeId = null;

        var lists: AccountingPaymentMethodList[] = this.AllMethods.filter(d => d.Id == this.AccountingPaymentMethodId);
        if (lists) {
            var list = lists[0];
            if (list) {
                this.AccountingPaymentMethodCode = list.Code;
            }
        }

        if (this.AccountingPaymentMethodCode == "CA" || this.AccountingPaymentMethodCode == "FS") {
            this.ValueDate = DateTool.GetCurrentDateAsUtc();
        }

        this.SetUIProperties_Cheque();
        this.SetUIProperties_CreditCard();
        this.SetUIProperties_BankTransfer();
    }
    UpdatePaymentChequeFields() {
        if (this.EntityPM.ARPaymentChequeReplicas.length > 0) {
            this.EntityPM.ARPaymentChequeReplicas.filter(d => d.LineNumber==1).forEach((cheque: ARPaymentChequeReplicaPM) => {
                if (cheque) {
                    cheque.BankId = this.Bank;
                    cheque.BankBranch = this.BankBranch;
                    cheque.BankAccount = this.Account;
                    cheque.ValueDate = this.ValueDate;
                    cheque.ForeignAmount = this.ChequeAmount;
                    cheque.ChequeNumber = this.ChequeOrPaymentRef
                }
            });
        }
    }

    UpdatePaymentBankTranferFields() {
        if (this.EntityPM.ARPaymentBankTranfers.length > 0) {
            this.EntityPM.ARPaymentBankTranfers.filter(d => d.LineNumber == 1).forEach((bankTransfer: ARPaymentBankTranferPM) => {
                if (bankTransfer) {
                    bankTransfer.BankAccountId = this.BankAccountId;
                    bankTransfer.ValueDate = this.ValueDate;
                    bankTransfer.PaymentRef = this.ChequeOrPaymentRef;
                    bankTransfer.ForeignAmount = this.BankTransferAmount;
                }
            });
        }
    }

    UpdatePaymentBankTranferBankAccountField() {
        if (this.EntityPM.ARPaymentBankTranfers.length > 0) {
            this.EntityPM.ARPaymentBankTranfers.filter(d => d.LineNumber == 1).forEach((bankTransfer: ARPaymentBankTranferPM) => {
                if (bankTransfer) {
                    bankTransfer.BankAccountId = this.BankAccountId;
                }
            });
        }
    }

    UpdateBankFieldForPaymentCheque() {
        if (this.EntityPM.ARPaymentChequeReplicas.length > 0) {
            this.EntityPM.ARPaymentChequeReplicas.filter(d => d.LineNumber == 1).forEach((cheque: ARPaymentChequeReplicaPM) => {
                if (cheque) {
                    cheque.BankId = this.Bank;
                }
            });
        }
    }

    UpdateBankBranchFieldForPaymentCheque() {
        if (this.EntityPM.ARPaymentChequeReplicas.length > 0) {
            this.EntityPM.ARPaymentChequeReplicas.filter(d => d.LineNumber == 1).forEach((cheque: ARPaymentChequeReplicaPM) => {
                if (cheque) {
                    cheque.BankBranch = this.BankBranch;
                }
            });
        }
    }

    UpdateAccountFieldForPaymentCheque() {
        if (this.EntityPM.ARPaymentChequeReplicas.length > 0) {
            this.EntityPM.ARPaymentChequeReplicas.filter(d => d.LineNumber == 1).forEach((cheque: ARPaymentChequeReplicaPM) => {
                if (cheque) {
                    cheque.BankAccount = this.Account;
                }
            });
        }
    }
    UpdatePaymentAmountFieldForPaymentCheque() {
        if (this.EntityPM.ARPaymentChequeReplicas.length > 0) {
            this.EntityPM.ARPaymentChequeReplicas.filter(d => d.LineNumber == 1).forEach((cheque: ARPaymentChequeReplicaPM) => {
                if (cheque) {
                    cheque.ForeignAmount = this.ChequeAmount;
                }
            });
        }

    }
    UpdateValueDateFieldForPaymentCheque() {
        if (this.EntityPM.ARPaymentChequeReplicas.length > 0) {
            this.EntityPM.ARPaymentChequeReplicas.filter(d => d.LineNumber == 1).forEach((cheque: ARPaymentChequeReplicaPM) => {
                if (cheque) {
                    cheque.ValueDate = this.ValueDate;
                }
            });
        }
    }

    UpdateValueDateFieldForPaymentBankTransfer() {
        if (this.EntityPM.ARPaymentBankTranfers.length > 0) {
            this.EntityPM.ARPaymentBankTranfers.filter(d => d.LineNumber == 1).forEach((aRPaymentBankTranfer: ARPaymentBankTranferPM) => {
                if (aRPaymentBankTranfer) {
                    aRPaymentBankTranfer.ValueDate = this.ValueDate;
                }
            });
        }
    }

    UpdatePaymentRefFieldForPaymentBankTransfer() {
        if (this.EntityPM.ARPaymentBankTranfers.length > 0) {
            this.EntityPM.ARPaymentBankTranfers.filter(d => d.LineNumber == 1).forEach((aRPaymentBankTranfer: ARPaymentBankTranferPM) => {
                if (aRPaymentBankTranfer) {
                    aRPaymentBankTranfer.PaymentRef = this.ChequeOrPaymentRef;
                }
            });
        }
    }


    UpdateChequeAmountFieldForPaymentCheque() {
        if (this.EntityPM.ARPaymentChequeReplicas.length > 0) {
            this.EntityPM.ARPaymentChequeReplicas.filter(d => d.LineNumber == 1).forEach((cheque: ARPaymentChequeReplicaPM) => {
                if (cheque) {
                    cheque.ForeignAmount = this.ChequeAmount;
                }
            });
        }
    }


    UpdatePaymentChequeBankBranchField() {
        if (this.EntityPM.ARPaymentChequeReplicas.length > 0) {
            this.EntityPM.ARPaymentChequeReplicas.forEach((cheque: ARPaymentChequeReplicaPM) => {
                if (cheque) {
                    cheque.BankBranch = this.BankBranch;

                }
            });
        }
    }
    get PaymentMethodDetailsLabel()
    {
        var result = "";
        if (this.AccountingPaymentMethodCode == "CH") {
            if (!this.isMultipleCheques) {
                result = TextCodeTranslator.Translate("ARPayment.S.Details.Cheque");
            }
            else {
                result = TextCodeTranslator.Translate("Accounting.General.O.Cheques") + " (" + this.EntityPM.ARPaymentChequeReplicas.length + ")";
            }
        }

        else if (this.AccountingPaymentMethodCode == "FS") {
            result = "Offsetting";
        }

        else if (this.AccountingPaymentMethodCode == "BT") {
            result = TextCodeTranslator.Translate("ARPayment.S.Details.BankTransfer");
        }
        else if (this.AccountingPaymentMethodCode == "CC") {
            result = TextCodeTranslator.Translate("ARPayment.S.Details.CreditCard");
        }

        else {
            var method: AccountingPaymentMethodList = this.AllMethods.filter(d => d.Code == this.AccountingPaymentMethodCode)[0];
            if (method != null) {
                result = method.Name;
            }
        }

        return result;
    }
    get ChequePaymentRefLabel()
    {
        var result = TextCodeTranslator.Translate("ARPayment.S.Details.PaymentRef");
        if (this.AccountingPaymentMethodCode == "CH") {
            result = TextCodeTranslator.Translate("ARPayment.S.Details.ChequeRef");
        }

        return result;
    }
    get VisibleIfCash()
    {
        var result = false;
        if (!AppTool.IsNullOrEmpty(this.AccountingPaymentMethodCode)) {
            if (this.AccountingPaymentMethodCode == "CA" || this.AccountingPaymentMethodCode == "FS") {
                result = true;
            }
        }
        return result;
    }
    get CollapsedIfCash()
    {
        var result = false;
        if (!AppTool.IsNullOrEmpty(this.AccountingPaymentMethodCode)) {
            if (this.AccountingPaymentMethodCode == "CA" || this.AccountingPaymentMethodCode == "FS") {
                result = false;
            }

            else {
                result = true;
            }
        }
        return result;
    }


    get Bank()
    {
        if (this.EntityPM == null) {
            return null;
        }
        return this.EntityPM.Bank;
    }
    set Bank(value: string)
    {
        if (this.EntityPM != null) {
            if (this.EntityPM.Bank != value) {
                this.EntityPM.Bank = value;
                if (this.EntityPM.AccountingPaymentMethodCode == "CH") {
                    this.UpdateBankFieldForPaymentCheque();
                }
                if (!AppTool.IsNullOrEmpty(value)) {
                    this.UIProperties.SetRequired("Bank", this.ObjectTableName, false);

                }
                else {

                    this.UIProperties.SetRequired("Bank", this.ObjectTableName, true);
                }
            }
        }
    }

    get BankBranch()
    {
        if (this.EntityPM == null) {
            return null;
        }
        return this.EntityPM.BankBranch;
    }
    set BankBranch(value: string)
    {
        if (this.EntityPM != null) {
            if (this.EntityPM.BankBranch != value) {
                this.EntityPM.BankBranch = value;

                if (this.isFullAccounting && this.AccountingPaymentMethodCode == "CH") {
                    if (!AppTool.IsNullOrEmpty(value)) {
                        this.UIProperties.SetRequired("BankBranch", this.ObjectTableName, false);
                    }
                    else {
                        this.UIProperties.SetRequired("BankBranch", this.ObjectTableName, true);
                    }
                    this.UpdateBankBranchFieldForPaymentCheque();
                }
            }
        }
    }

    get Account()
    {
        if (this.EntityPM == null) {
            return null;
        }
        return this.EntityPM.Account;
    }
    set Account(value: string)
    {
        if (this.EntityPM != null) {
            if (this.EntityPM.Account != value) {
                this.EntityPM.Account = value;
                if (this.isFullAccounting && this.AccountingPaymentMethodCode == "CH") {
                    if (!AppTool.IsNullOrEmpty(value)) {
                        this.UIProperties.SetRequired("Account", this.ObjectTableName, false);
                    }
                    else {
                        this.UIProperties.SetRequired("Account", this.ObjectTableName, true);
                    }
                    this.UpdateAccountFieldForPaymentCheque();
                }
            }
        }
    }

    get ValueDate()
    {
        if (this.EntityPM == null) {
            return null;
        }
        return this.EntityPM.ValueDate;
    }
    set ValueDate(value: Date)
    {
        if (this.EntityPM != null) {
            if (this.EntityPM.ValueDate != value) {
                this.EntityPM.ValueDate = value;

                this.SetUIProperties_ValueDate();
                if (this.EntityPM.AccountingPaymentMethodCode == "CH") {
                    this.UpdateValueDateFieldForPaymentCheque();
                }

                if (this.EntityPM.AccountingPaymentMethodCode == "BT") {
                    this.UpdateValueDateFieldForPaymentBankTransfer();
                }
            }
        }
    }

    SetUIProperties_ValueDate() {
        this.UIProperties.SetRequired("ValueDate", this.ObjectTableName, this.ValueDate != null ? false : true);

    }

    get ChequeOrPaymentRef()
    {
        if (this.EntityPM == null) {
            return null;
        }
        return this.EntityPM.ChequeOrPaymentRef;
    }
    set ChequeOrPaymentRef(value: string)
    {
        if (this.EntityPM != null) {
            if (this.EntityPM.ChequeOrPaymentRef != value) {
                this.EntityPM.ChequeOrPaymentRef = value;
                if (this.EntityPM.AccountingPaymentMethodCode == "CH") {
                    this.SetUIProperties_Cheque();
                }
                if (this.EntityPM.AccountingPaymentMethodCode == "BT") {
                    this.UpdatePaymentRefFieldForPaymentBankTransfer();
                    this.SetUIProperties_BankTransfer();
                }
            }
        }
    }

    get CreditCardTypeId()
    {
        if (this.EntityPM == null) {
            return null;
        }
        return this.EntityPM.CreditCardTypeId;
    }
    set CreditCardTypeId(value: string)
    {
        if (this.EntityPM != null) {
            if (this.EntityPM.CreditCardTypeId != value) {
                this.EntityPM.CreditCardTypeId = value;
            }

            if (!AppTool.IsNullOrEmpty(value)) {
                this.UIProperties.SetRequired("CreditCardTypeId", this.ObjectTableName, false);
            }
            else {
                this.UIProperties.SetRequired("CreditCardTypeId", this.ObjectTableName, true);
            }
        }
    }

    public CreditCardTypeIdVisibility: boolean = false;
    public BankAccountIdVisibility: boolean = false;

    get BankAccountId()
    {
        if (this.EntityPM == null) {
            return null;
        }
        return this.EntityPM.BankAccountId;
    }
    set BankAccountId(value: string)
    {
        if (this.EntityPM != null) {
            if (this.EntityPM.BankAccountId != value) {
                this.EntityPM.BankAccountId = value;
                this.CheckGLAccountCurrencyId();

                if (!AppTool.IsNullOrEmpty(value)) {
                    this.UIProperties.SetRequired("BankAccountId", this.ObjectTableName, false);
                }
                else {
                    this.UIProperties.SetRequired("BankAccountId", this.ObjectTableName, true);
                }

                if (this.EntityPM.AccountingPaymentMethodCode == "BT") {
                    this.UpdatePaymentBankTranferBankAccountField();
                }
            }
        }
    }
    loadBankAccounts(filters: ApiQueryFilters) {
        var myService: BankAccountListService = new BankAccountListService();
        filters.addAdditionalFilter("Inactive", false, null, null, "Equals", false, false, false, "Boolean");
        filters.PageSize = 50;
        myService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
            if (!myResponse.HasError) {

                var bankAccounts = myResponse.Result;
                if(bankAccounts && bankAccounts.length == 1) {
                    this.BankAccountId = bankAccounts[0].Id;
                } else if(bankAccounts && bankAccounts.length > 1 && this.BankAccountId && bankAccounts.filter(x=>x.Id == this.BankAccountId).length == 0 ) {
                    this.BankAccountId = null;
                } else if(bankAccounts.length == 0) {
                    this.BankAccountId = null;
                }
            }
        });
    }
    bankAccount: BankAccountPM;
    public GLAccountNumber: string = "";
    public IsGLAccountCurrencyDifferent = false;
    private CheckGLAccountCurrencyId()
    {
        var service: BankAccountPMService = new BankAccountPMService();
        if (!AppTool.IsNullOrEmpty(this.BankAccountId)) {
            service.get(this.BankAccountId).subscribe((myResponse: ServiceResponse) =>
            {
                console.log('myResponse', myResponse);
                if (myResponse != null && !myResponse.HasError) {
                    this.bankAccount = myResponse.Result;
                    this.EntityPM.BankAccount = this.bankAccount;
                    if (this.bankAccount != null && this.bankAccount.GLAccountCurrencyId != null && this.bankAccount.GLAccountCurrencyId != "multi") {
                        if (this.bankAccount.GLAccountCurrencyId != this.PaymentCurrencyId) {
                            this.GLAccountNumber = this.bankAccount.GLAccountNumber;
                            this.IsGLAccountCurrencyDifferent = true;
                            var msg = "The currency of the bank account GLAccount (" + this.GLAccountNumber + ") is different from ARPayment curreny";
                            this.UIProperties.SetValidity("BankAccountId", this.ObjectTableName, false, msg);
                        }
                        else {
                            this.IsGLAccountCurrencyDifferent = false;
                            this.UIProperties.SetValidity("BankAccountId", this.ObjectTableName, true, "");
                        }
                    }
                    else {
                        this.IsGLAccountCurrencyDifferent = false;
                        this.UIProperties.SetValidity("BankAccountId", this.ObjectTableName, true, "");
                    }
                }
            });
        }
        else {
            this.IsGLAccountCurrencyDifferent = false;
            this.UIProperties.SetValidity("BankAccountId", this.ObjectTableName, true, "");
        }
    }
    EditGLAccount(arg: string)
    {
        if (arg == "BA") {
            if (!AppTool.IsNullOrEmpty(this.bankAccount.GLAccountId)) {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(cmpRef =>
                    {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ EntityId: this.bankAccount.GLAccountId, ObjectTableName: 'GLAccount' });
                    });
            }
        }
        else {
            if (!AppTool.IsNullOrEmpty(this.BranchGLAccountId)) {
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(cmpRef =>
                    {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ EntityId: this.BranchGLAccountId, ObjectTableName: 'GLAccount' });
                    });
            }
        }
    }

    // Amounts
    get AmountInPaymentCurrency() { return this.EntityPM.AmountInPaymentCurrency; }
    set AmountInPaymentCurrency(value: number)
    {
        if (this.EntityPM.AmountInPaymentCurrency != value) {
            this.EntityPM.AmountInPaymentCurrency = AppTool.Round(value, 2);
            this.ComputeLocalAmount();
            this.SetPaymentAmount();
            this.ComputeOpenAmount();
            this.ComputeOpenAmountInLocalCurrency();
            this.UpdateSummary();

            this.originalPaymentOpenAmount = this.EntityPM.AmountInPaymentCurrency;
            this.paymentAmountTotal = this.EntityPM.AmountInPaymentCurrency;

            this.CalculateTotals();
            if (this.EntityPM.AccountingPaymentMethodCode == "CH") {
                this.UpdateChequeAmountFieldForPaymentCheque();
            }

            if (this.EntityPM.AccountingPaymentMethodCode == "BT") {
                this.UpdateBankTransferAmountFieldForBankTransferPayment();
            }

            this.ItemsSource.Collection.forEach(item =>
            {
                item.SetUIProperties();
            });
        }
    }

    get AmountInLocalCurrency() { return this.EntityPM.AmountInLocalCurrency; }
    set AmountInLocalCurrency(value: number)
    {
        if (this.EntityPM.AmountInLocalCurrency != value) {
            this.EntityPM.AmountInLocalCurrency = AppTool.Round(value, 2);
        }
    }

    get OpenAmount() { return this.EntityPM.OpenAmount == null ? 0 : this.EntityPM.OpenAmount; }
    set OpenAmount(value: number)
    {
        if (this.EntityPM.OpenAmount != value) {
            this.EntityPM.OpenAmount = AppTool.Round(value, 2);
            this.ComputeOpenAmountInLocalCurrency();
        }
    }

    get OpenAmountInLocalCurrency() { return this.EntityPM.OpenAmountInLocalCurrency == null ? 0 : this.EntityPM.OpenAmountInLocalCurrency; }
    set OpenAmountInLocalCurrency(value: number)
    {
        if (this.EntityPM.OpenAmountInLocalCurrency != value) {
            this.EntityPM.OpenAmountInLocalCurrency = AppTool.Round(value, 2);
        }
    }

    get PrintNotes() { return this.EntityPM.PrintNotes; }
    set PrintNotes(value: string)
    {
        if (this.EntityPM.PrintNotes != value) {
            this.EntityPM.PrintNotes = value;
        }
    }
    get ChequeAmount() { return this.chequeAmount }
    set ChequeAmount(value: number) {
        if (this.chequeAmount != value) {
            if (this.EntityPM.ARPaymentChequeReplicas.length == 1 || this.EntityPM.ARPaymentChequeReplicas.length == 0) {
                this.chequeAmount = value;
                this.AmountInPaymentCurrency = value;
            }
            this.chequeAmount = value;

            if(value==null){
                this.UIProperties.SetRequired("AmountInPaymentCurrency", this.ObjectTableName, true);
            }
            else
            {
                this.UIProperties.SetRequired("AmountInPaymentCurrency", this.ObjectTableName, false);
            }

            if (this.EntityPM.AccountingPaymentMethodCode == "CH") {
                this.UpdateChequeAmountFieldForPaymentCheque();
            }
            this.CalculatePaymentTotalAmount();
        }
    }

    get BankTransferAmount() { return this.bankTransferAmount }
    set BankTransferAmount(value: number) {
        if (this.bankTransferAmount != value) {
            if (this.EntityPM.ARPaymentBankTranfers.length == 1 || this.EntityPM.ARPaymentBankTranfers.length == 0) {
                this.bankTransferAmount = value;
                this.AmountInPaymentCurrency = value;

            }

            this.bankTransferAmount = value;
            if(value==null){
                this.UIProperties.SetRequired("AmountInPaymentCurrency", this.ObjectTableName, true);
            }
            else
            {
                this.UIProperties.SetRequired("AmountInPaymentCurrency", this.ObjectTableName, false);
            }

            if (this.EntityPM.AccountingPaymentMethodCode == "BT") {
                this.UpdateBankTransferAmountFieldForBankTransferPayment();
            }
            this.CalculatePaymentTotalAmountForBankTransfers();

        }
    }

    ComputeOpenAmount()
    {
        this.OpenAmount = this.AmountInPaymentCurrency - this.Summary_AmountPaid;
    }

    ComputeLocalAmount()
    {
        this.AmountInLocalCurrency = this.AmountInPaymentCurrency * this.PaymentCurrencyExchangeRate;
    }

    ComputeOpenAmountInLocalCurrency()
    {
        this.OpenAmountInLocalCurrency = this.OpenAmount * this.PaymentCurrencyExchangeRate;
    }


    // Summary
    public Summary_Amount: number = 0;
    public Summary_AmountPaid: number = 0;
    public Summary_AmountPaidColor: string = "#282E30";
    UpdateSummary()
    {
        var Amount: number = 0;
        var AmountPaid: number = 0;
        var AmountPaidColor: string = "#282E30";

        if (this.AmountInPaymentCurrency) {
            Amount = AppTool.Round(this.AmountInPaymentCurrency, 2);
        }

        if (this.EntityPM.PaymentInvoices.length > 0) {
            AmountPaid = ArrayTool.Sum(this.EntityPM.PaymentInvoices, "PaymentAmount");
            AmountPaid = AppTool.Round(AmountPaid, 2);
        }

        if (this.IsNegativeAmountEnabled == false) {
            if (AmountPaid < 0) {
                AmountPaidColor = "#E53030";
            }
        }

        if (AmountPaid > Amount) {
            AmountPaidColor = "#E53030";
        }

        this.Summary_Amount = Amount;
        this.Summary_AmountPaid = AmountPaid;
        this.Summary_AmountPaidColor = AmountPaidColor;
    }

    ViewEntity(args: any)
    {
        if (args) {
            this.RequestedCommandCode = "ViewInvoice";
            this.RequestedCommandParam = args.Id;

            this.entityArgs.EditComponent.SaveChanges();
        }
    }
    UpdateCurrencyRateMethod()
    {
        this._entityResourceService.getEntityResourceByTableName("RatesTable", 0).subscribe((response: any) =>
        {
            var logWindow = new LogitudeWindow();
            logWindow.Width = 600;
            logWindow.Height = 350;
            logWindow.Title = "Update Currency Rate";
            logWindow.WindowArgs = { CurrencyId: this.EntityPM.PaymentCurrencyId, CurrencyCode: this.EntityPM.PaymentCurrencyCode, Rate: this.EntityPM.PaymentCurrencyExchangeRate, Date: this.RegisterDate };
            logWindow.ComponentLoaded.subscribe(comp =>
            {
                logWindow.WindowClosed.subscribe(s =>
                {
                    if (s) {
                        this.PaymentCurrencyExchangeRate = comp.Rate;
                        this.ExchangeRateDate = comp.RateDate;
                    }
                });
            });
            logWindow.Show('./CommonModules/CommonOthers/Components/UpdateCurrencyRate/UpdateCurrencyRateComponent');
        });
    }
    UpdatePaymentInvoicesErrors()
    {
        var haserrors = false;

        if (this.ItemsSource.Collection.filter(d => d.InputHasError)[0]) {
            haserrors = true;
        }

        this.EntityPM.HasInvoicesErrors = haserrors;
    }

    private RequestedCommandCode: string = null;
    private RequestedCommandParam: string = null;
    ApplyRequestedCommand()
    {
        if (this.RequestedCommandCode == "ViewInvoice") {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
                .then(cmpRef =>
                {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: this.RequestedCommandParam, ObjectTableName: 'ARInvoice' });

                    this.RequestedCommandParam = null;

                    let isEditComponentSaved = false;

                    cmpRef.instance.BackCompleted.subscribe(bk =>
                    {
                        if (isEditComponentSaved) {
                            this.entityArgs.EditComponent.ReloadEntityPM();
                        }
                    });

                    cmpRef.instance.SaveCompleted.subscribe((isSaveSuccess: boolean) =>
                    {
                        if (isSaveSuccess) {
                            isEditComponentSaved = true;
                        }
                    });

                    cmpRef.instance.SaveAndCloseCompleted.subscribe((isSaveSuccess: boolean) =>
                    {
                        if (isSaveSuccess) {
                            isEditComponentSaved = true;
                        }
                    });
                });
        }

        this.RequestedCommandCode = null;
    }

    //#endregion

    sortTransactionsByStatus(transactions: TransactionLineModel[])
    {
        var closedTransactions = transactions.filter(d => d.Status == TextStore.Closed);
        var partiallyOpenedTransactions = transactions.filter(d => d.Status == TextStore.partiallyOpened);
        var openedTransactions = transactions.filter(d => d.Status == TextStore.open);

        var sortedTransactions: TransactionLineModel[] = [];
        sortedTransactions = closedTransactions.concat(partiallyOpenedTransactions).concat(openedTransactions);
        return sortedTransactions;
    }


    public get PaymentOpenAmount() : number {
        return this.PaymenyAmount - this.paymentReconciledAmountTotal - this.amount2reconcileTotal;
    }

    ValidateChequeFields() {
        var errors: string[];
        errors = this.ARPaymentValidator.Validate(this.EntityPM);
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
        return errors;
    }
    AddChequesButtonClicked() {
        var errors: string[] = this.ValidateChequeFields();
        if (errors.length > 0) {
            return;
        }
        this.ShowMultiChequeScreen();
    }

    ShowMultiChequeScreen() {
        var windowArgs: any = {};
        windowArgs.EntityPM = this.EntityPM;
        var logWindow = new LogitudeWindow();
        logWindow.Width = 1000;
        logWindow.Height = 600;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => this.UpdateChequesSection($event));
        logWindow.Show('./InvoiceModules/ARPayment/Components/Other/ARPaymentMultiChequesComponent');
    }
    UpdateChequesSection(event: any) {
        if (event == 'ok') {
            this.isMultipleCheques = false;
            if (this.EntityPM.ARPaymentChequeReplicas.length > 1) {
                this.isMultipleCheques = true;
                //this.GetData();
            }
            if (this.EntityPM.ARPaymentChequeReplicas.length >= 0) {
                this.SetDefaultChequeFields();
            }
        }
        if(this.EntityPM.AccountingPaymentMethodCode == "CH" && event.hasOwnProperty('chequesReplicas')) {
            let chequesReplicas: ARPaymentChequeReplicaPM[] = event.chequesReplicas;
            this.EntityPM.ARPaymentChequeReplicas.map((item, i) => {
                if (chequesReplicas.filter(x => x.ChequeNumber == item.ChequeNumber).length > 0){
                    this.EntityPM.ARPaymentChequeReplicas[i].StatusCode = chequesReplicas.filter(x => x.ChequeNumber == item.ChequeNumber)[0].StatusCode;
                    this.EntityPM.ARPaymentChequeReplicas[i].StatusName = chequesReplicas.filter(x => x.ChequeNumber == item.ChequeNumber)[0].StatusName;
                }
            });


        }
    }
    SetDefaultChequeFields() {
        if (this.EntityPM.ARPaymentChequeReplicas.length > 0) {
            var firstCheque: ARPaymentChequeReplicaPM = this.EntityPM.ARPaymentChequeReplicas.filter(d => d.LineNumber == 1)[0];
            this.MapChequeFields(firstCheque);
            this.CalculatePaymentTotalAmount();

        }
        else if (this.EntityPM.ARPaymentChequeReplicas.length == 0) {
            this.MapChequeFields(null);
        }

    }
    CalculatePaymentTotalAmount() {

        var total = 0;
        for (let cheque of this.EntityPM.ARPaymentChequeReplicas) {
            if (!AppTool.IsNullOrEmpty(cheque.ForeignAmount)) {
                total += cheque.ForeignAmount;

            }
        }
        if (this.EntityPM.ARPaymentChequeReplicas.length >0)
            this.AmountInPaymentCurrency = total;
        this.ComputeLocalAmount();
        this.SetPaymentAmount();
    }
    MapChequeFields(cheque: ARPaymentChequeReplicaPM) {
        this.Bank = cheque != null ? cheque.BankId : null;
        this.Account = cheque != null ? cheque.BankAccount : null;
        this.ValueDate = cheque != null ? cheque.ValueDate : null;
        this.BankBranch = cheque != null ? cheque.BankBranch : null;
        this.ChequeAmount = cheque != null ? cheque.ForeignAmount : null;
        this.ChequeOrPaymentRef = cheque != null ? cheque.ChequeNumber : null;
    }
    DisplayChequesButtonClicked() {
        this.ShowMultiChequeScreen();
    }
    /* Bank Transferes */
    AddBankTransfersButtonClicked() {
        var errors: string[] = this.ValidateBankTransferFields();
        if (errors.length > 0) {
            return;
        }
        this.ShowMultiBankTransfersScreen();
    }

    ShowMultiBankTransfersScreen() {
        var windowArgs: any = {};
        windowArgs.EntityPM = this.EntityPM;
        var logWindow = new LogitudeWindow();
        logWindow.Width = 630;
        logWindow.Height = 600;
        logWindow.ShowCloseButton = false;
        logWindow.WindowArgs = windowArgs;
        logWindow.WindowClosed.subscribe(($event: any) => this.UpdateBankTransfersSection($event));
        logWindow.Show('./InvoiceModules/ARPayment/Components/Other/ARPaymentMultiBankTransfersComponent');
    }

    UpdateBankTransfersSection(event: any) {
        if (event == 'ok') {
            this.isMultipleBankTransfers = false;
            if (this.EntityPM.ARPaymentBankTranfers.length > 1) {
                this.isMultipleBankTransfers = true;
                this.GetData();
            }
            if (this.EntityPM.ARPaymentBankTranfers.length >= 0) {
                this.SetDefaultBankTransfersFields();
            }
        }
    }

    UpdateBankTransferAmountFieldForBankTransferPayment() {
        if (this.EntityPM.ARPaymentBankTranfers.length > 0) {
            this.EntityPM.ARPaymentBankTranfers.filter(d => d.LineNumber == 1).forEach((bankTransfer: ARPaymentBankTranferPM) => {
                if (bankTransfer) {
                    bankTransfer.ForeignAmount = this.BankTransferAmount;
                }
            });
        }
    }

    SetDefaultBankTransfersFields() {
        if (this.EntityPM.ARPaymentBankTranfers.length > 0) {
            var firstBankTransfer: ARPaymentBankTranferPM = this.EntityPM.ARPaymentBankTranfers.filter(d => d.LineNumber == 1)[0];
            this.MapBankTransferFields(firstBankTransfer);
            this.CalculatePaymentTotalAmountForBankTransfers();

        }
        else if (this.EntityPM.ARPaymentBankTranfers.length == 0) {
            this.MapBankTransferFields(null);
        }

    }

    CalculatePaymentTotalAmountForBankTransfers() {

        var total = 0;
        for (let bankTransfer of this.EntityPM.ARPaymentBankTranfers) {
            if (!AppTool.IsNullOrEmpty(bankTransfer.ForeignAmount)) {
                total += bankTransfer.ForeignAmount;
            }
        }
        if (this.EntityPM.ARPaymentBankTranfers.length >0)
            this.AmountInPaymentCurrency = total;
        this.ComputeLocalAmount();
        this.SetPaymentAmount();
    }

    MapBankTransferFields(bankTransfer: ARPaymentBankTranferPM) {
        this.BankAccountId = bankTransfer != null ? bankTransfer.BankAccountId : null;
        this.ValueDate = bankTransfer != null ? bankTransfer.ValueDate : null;
        this.ChequeOrPaymentRef = bankTransfer != null ? bankTransfer.PaymentRef : null;
        this.BankTransferAmount = bankTransfer != null ? bankTransfer.ForeignAmount : null;
    }

    ValidateBankTransferFields() {
        var errors: string[];
        errors = this.ARPaymentValidator.Validate(this.EntityPM);
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
        return errors;
    }
}


export class TextStore
{
    static open: string = TextCodeTranslator.Translate('Accounting.O.ARP.Open');
    static Closed: string = TextCodeTranslator.Translate('Accounting.O.ARP.Closed');
    static partiallyOpened: string = TextCodeTranslator.Translate('Accounting.O.ARP.partiallyOpened');

    static ErrorsInSelectedLines: string = SessionLocator.TenantPM.AccountingActivated ? TextCodeTranslator.Translate('Reconciliations.O.ErrorsInSelectedLines') : "";
    static invoiceAmount2reconcileMSG: string = TextCodeTranslator.Translate('Accounting.O.ARP.invoiceAmount2reconcileMSG');


}

export class TransactionLineModel extends BaseComponent
{
    public DataContext = this;
    public LedgerTransactionPM: LedgerTransactionPM = null;
    public ObjectTableName = "LedgerTransaction";
    public isRTL: boolean = false;
    isLineValid: boolean = true;
    public InvoiceCurrency: string;
    IsAccountingActivated: boolean = false;
    constructor(
        private ledgerTransaction: LedgerTransactionPM,
        private parent: ARPaymentDetailsFullAccountingTab
    )
    {
        super();
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.EntityPM = this.parent.EntityPM;
        this.LedgerTransactionPM = ledgerTransaction;
        this.InvoiceCurrency = this.LedgerTransactionPM.CurrencyCode;
        this.IsAccountingActivated = SessionLocator.TenantPM.AccountingActivated;
        this.CalculateFields();

        this.UIProperties.SetEnabled("AmountToReconcile", "LedgerTransaction", this.Status != TextStore.Closed && !this.parent.IsGridReadOnly );
    }

    CalculateFields()
    {
        this.IconCode = AccountingEntityHelper.getEntityIcon(this.LedgerTransactionPM.SourceTypeCode);

        this.OriginalAmount = this.CalculateOriginalAmount();
        this.OriginalInvoiceAmount = this.SetOriginalInvoiceAmount();
        this.OriginalAmountCurrency = this.CalculatOriginalCurruncy();

        this.CalculatedOpenAmount = this.CalculateOpenAmount();
        this.originalOpenAmount = this.CalculatedOpenAmount;
        this.Status = this.GetStatus();
        this.RecociliationNumbers = this.getRecoLinkList();
    }
    //#region Properties

    public IconCode: string;
    public ReconciliationNumber: string;
    calculatedOpenAmount: number;
    public get CalculatedOpenAmount(): number { return this.calculatedOpenAmount; }
    public set CalculatedOpenAmount(value: number)
    {
        this.calculatedOpenAmount = value;
    }
    private _OriginalAmount: number;
    public get OriginalAmount(): number
    {
        return this._OriginalAmount;
    }
    public set OriginalAmount(v: number)
    {
        this._OriginalAmount = v;
    }
    private originalInvoiceAmount: number;
    public get OriginalInvoiceAmount(): number
    {
        return this.originalInvoiceAmount;
    }
    public set OriginalInvoiceAmount(value: number)
    {
        this.originalInvoiceAmount = value;
    }
    private _OriginalAmountCurrency: string;
    public get OriginalAmountCurrency(): string
    {
        return this._OriginalAmountCurrency;
    }
    public set OriginalAmountCurrency(v: string)
    {
        this._OriginalAmountCurrency = v;
    }

    private _isChecked: boolean;
    public get IsChecked(): boolean
    {
        if (this.IsReconciled) {
            return true;
        } else {
            return this._isChecked;
        }
    }
    public set IsChecked(v: boolean)
    {

        this._isChecked = v;

        if (v) {


            this.SetAmountToReconcile();

            this.parent.PushTransaction(this.ledgerTransaction);
        } else {
            this.AmountToReconcile = 0;

            this.parent.PopTransaction(this.ledgerTransaction);
            if (this.EntityPM.InvoicesLedgerTransactions.length == 0) {
                this.EntityPM.IsDirty = false;
            }

        }

    }




    private SetAmountToReconcile()
    {
        if (this.AmountToReconcile == null || this.AmountToReconcile == 0)
        {
            // if(this.CurrencyId == this.parent.EntityPM.PaymentCurrencyId)
            // {

            if (this.OpenAmount <= this.parent.PaymentOpenAmount)
                this.AmountToReconcile = this.OpenAmount;

            else if (this.OpenAmount > this.parent.PaymentOpenAmount)
                this.AmountToReconcile = this.parent.PaymentOpenAmount;

            // }else{

            // }

        }
    }

    get IsReconciled() { return this.LedgerTransactionPM.IsReconciled; }
    set IsReconciled(value: boolean)
    {
        if (this.LedgerTransactionPM.IsReconciled != value) {
            this.LedgerTransactionPM.IsReconciled = value;
        }
    }

    originalOpenAmount: number;

    get AmountToReconcile() { return this.LedgerTransactionPM.AmountToReconcile; }
    set AmountToReconcile(value: number)
    {
        if (this.LedgerTransactionPM.AmountToReconcile != value) {
            this.LedgerTransactionPM.AmountToReconcile = value;

            if (value == 0 || !value) {
                this.IsChecked = false;
                this.UIProperties.SetValidity("AmountToReconcile", this.ObjectTableName, true, "valid");
                this.isLineValid = true;
                this.parent.SetEntityValidity();
            }
            else
                this.IsChecked = true;

            this.setAmounts();

            if (this.IsChecked)
                this.validateLine();

            this.parent.CalculateTotals();
        }

    }

    OnAmountToReconcileLostFocus(logCellTemplate: any, classificationTextBox: any)
    {

        if (this.IsChecked)
            this.validateLine();

    }

    AutomaticallyFillAmountToReconciledblclick(logCellTemplate: any, classificationTextBox: any){


        const inputAmountToReconciled = document.getElementById(classificationTextBox.InputId);
        inputAmountToReconciled.blur();

        if(this.IsAccountingActivated && (classificationTextBox.IsDisabled == false)) {
            var totalOpenAmount = this.parent.PaymenyAmount - this.parent.paymentReconciledAmountTotal -this.parent.amount2reconcileTotal;

            if(this.OpenAmount > 0){
                if(this.AmountToReconcile > 0) {
                    totalOpenAmount = totalOpenAmount + this.AmountToReconcile;
                    this.AmountToReconcile = 0;
                }
                if(totalOpenAmount >= this.OpenAmount) {
                    this.AmountToReconcile = this.OpenAmount;
                } else if(totalOpenAmount <= this.OpenAmount && totalOpenAmount > 0){
                    this.AmountToReconcile = totalOpenAmount;
                } else {
                    this.AmountToReconcile = 0;
                }
            } else if(this.OpenAmount < 0){
                this.AmountToReconcile = 0;
            }

        }


    }

    get OpenAmount() { return this.LedgerTransactionPM.OpenAmount; }
    set OpenAmount(value: number)
    {
        if (this.LedgerTransactionPM.OpenAmount != value) {
            this.LedgerTransactionPM.OpenAmount = value;
        }
    }

    private _Status: string;
    public get Status(): string
    {
        return this._Status;
    }
    public set Status(v: string)
    {
        this._Status = v;
    }


    public get ReconciledAmount(): number
    {
        return this.OriginalAmount - this.originalOpenAmount;
    }

    public get NewReconciliationAmount(): number
    {
        return this.AmountToReconcile + this.ReconciledAmount;
    }

    public get PaymentReconciledAmount(): number
    {
        return this.LedgerTransactionPM.PaymentReconciledAmount;
    }


    //#endregion

    validateLine()
    {

        if (this.originalOpenAmount < 0) {
            if (this.AmountToReconcile <= 0 && this.AmountToReconcile >= this.originalOpenAmount) {
                this.UIProperties.SetValidity("AmountToReconcile", this.ObjectTableName, true, "valid");
                this.isLineValid = true;
                this.parent.SetEntityValidity();
            } else {
                this.SetLineAmountValidity();
            }

        }
        else {
            if (this.AmountToReconcile >= 0 && this.AmountToReconcile <= Math.abs(this.originalOpenAmount)) {
                this.UIProperties.SetValidity("AmountToReconcile", this.ObjectTableName, true, "valid");
                this.isLineValid = true;
                this.parent.SetEntityValidity();

                // SessionLocator.SustainFocusOnCell = false;

            } else {
                this.SetLineAmountValidity();
            }
        }


    }

    private SetLineAmountValidity()
    {
        this.UIProperties.SetValidity("AmountToReconcile", this.ObjectTableName, false, TextStore.invoiceAmount2reconcileMSG);
        this.isLineValid = false;
        this.parent.SetEntityValidity();

        // SessionLocator.SustainFocusOnCell = true;
        // SessionLocator.SelectedSession.SessionEvent.emit({ FocusNow: true, OuterDivId: logCellTemplate.OuterDivId, LogTextBoxId: classificationTextBox.InputId });
    }

    setAmounts()
    {
        //set amount
        if (this.AmountToReconcile >= 0 && this.AmountToReconcile <= this.originalOpenAmount) {
            this.OpenAmount = this.originalOpenAmount - this.AmountToReconcile;
            this.CalculatedOpenAmount = this.originalOpenAmount - this.AmountToReconcile;

        } else {
            this.CalculatedOpenAmount = this.originalOpenAmount;

            this.OpenAmount = this.originalOpenAmount;
        }
    }

    //#region Other Properties
    get Id() { return this.LedgerTransactionPM.Id; }
    get Tenant() { return this.LedgerTransactionPM.Tenant; }
    get AccountingDate() { return this.LedgerTransactionPM.AccountingDate; }
    get DocumentDate() { return this.LedgerTransactionPM.DocumentDate; }
    get JournalNumber() { return this.LedgerTransactionPM.JournalNumber; }
    get Source() { return this.LedgerTransactionPM.Source; }
    get SourceType() { return this.LedgerTransactionPM.SourceType; }
    get SourceId() { return this.LedgerTransactionPM.SourceId; }
    get DueDate() { return this.LedgerTransactionPM.DueDate; }
    get LocalAmountCredit() { return this.LedgerTransactionPM.LocalAmountCredit; }
    get LocalAmountDebit() { return this.LedgerTransactionPM.LocalAmountDebit; }
    get ForeignAmountCredit() { return this.LedgerTransactionPM.ForeignAmountCredit; }
    get ForeignAmountDebit() { return this.LedgerTransactionPM.ForeignAmountDebit; }
    get OpenAmountCurrencyCode() { return this.LedgerTransactionPM.OpenAmountCurrencyCode; }
    get OpenAmountCurrencySign() { return this.LedgerTransactionPM.OpenAmountCurrencySign; }
    get CurrencyId() { return this.LedgerTransactionPM.CurrencyId; }
    get Reference1() { return this.LedgerTransactionPM.Reference1; }
    get Reference2() { return this.LedgerTransactionPM.Reference2; }
    get Reference3() { return this.LedgerTransactionPM.Reference3; }
    get Notes() { return this.LedgerTransactionPM.Notes; }
    get IsPartial() { return this.OpenAmount != this.AmountToReconcile; }
    get OpenAmountCurrencyId() { return this.LedgerTransactionPM.OpenAmountCurrencyId; }
    get SourceTypeCode() { return this.LedgerTransactionPM.SourceTypeCode; }
    get SourceNumber() { return this.LedgerTransactionPM.SourceNumber; }
    get GroupNumber() { return this.LedgerTransactionPM.GroupHash; }
    get RecoNumber() { return this.LedgerTransactionPM.RecoNumber; }
    get ReconciliationId() { return this.LedgerTransactionPM.ReconciliationId; }

    //#endregion

    GetStatus()
    {
        var __s = "";
        var invoiceAmount: number;
        if (this.parent.EntityPM.GLAccountRecoMethodCode == "0") {

            invoiceAmount = this.OriginalAmount;
        }
        else { invoiceAmount = this.OriginalInvoiceAmount; }

        if (AppTool.Round(invoiceAmount, 2) == AppTool.Round(this.originalOpenAmount, 2))
            __s = TextStore.open;
        else if (0 == this.originalOpenAmount)
            __s = TextStore.Closed;
        else
            __s = TextStore.partiallyOpened;

        return __s;
    }
    GetStatusColor()
    {
        var _color = 'black';
        var invoiceAmount: number;
        if (this.parent.EntityPM.GLAccountRecoMethodCode == "0") {

            invoiceAmount = this.OriginalAmount;
        }
        else { invoiceAmount = this.OriginalInvoiceAmount; }

        if (invoiceAmount == this.originalOpenAmount)
            _color = 'green';
        else if (0 == this.originalOpenAmount)
            _color = 'black';
        else
            _color = 'orange';
        return _color;
    }

    CalculateOriginalAmount()
    {
        var transaction = this.LedgerTransactionPM;
        if (transaction['LocalAmountCredit'] == 0) {
            return transaction['LocalAmountDebit'];
        } else {
            return -1 * transaction['LocalAmountCredit'];
        }
        //if (!AppTool.IsNullOrEmpty(this.parent.EntityPM.GLAccountRecoMethodCode)) {

        //    if (this.parent.EntityPM.GLAccountRecoMethodCode == "0") { // 0-local currency

        //        if (transaction['LocalAmountCredit'] == 0) {
        //            return transaction['LocalAmountDebit'];
        //        } else {
        //            return -1 * transaction['LocalAmountCredit'];
        //        }

        //    } else if (this.parent.EntityPM.GLAccountRecoMethodCode == "1") { // 1-foreign currency

        //        if (transaction['ForeignAmountCredit'] == 0) {
        //            return transaction['ForeignAmountDebit'];
        //        } else {
        //            return -1 * transaction['ForeignAmountCredit'];
        //        }

        //    }

        //}
    }
    SetOriginalInvoiceAmount()
    {
        var transaction = this.LedgerTransactionPM;
        if (transaction['ForeignAmountCredit'] == 0) {
            return transaction['ForeignAmountDebit'];
        } else {
            return -1 * transaction['ForeignAmountCredit'];
        }


    }
    CalculateOpenAmount()
    {
        if (this.parent.EntityPM.GLAccountRecoMethodCode == "0" && this.parent.EntityPM.GLAccountCurrencyCode == "Multi") {

            return this.OpenAmount //* this.parent.PaymentCurrencyExchangeRate;

        } else { return this.OpenAmount; }
    }


    CalculatOriginalCurruncy() {
        //
        // [i] copied from list template
        //

        if (!AppTool.IsNullOrEmpty(this.parent.EntityPM.GLAccountRecoMethodCode)) {
            // this code was copied to reconcile window, if it need change, please chenge it in reconcile window too
            if (this.parent.EntityPM.GLAccountRecoMethodCode == "0") { // 0-local currency

                // local
                return SessionLocator.TenantPM.CurrencySign;

            } else if (this.parent.EntityPM.GLAccountRecoMethodCode == "1") { // 1-foreign currency

                // foreign
                return this.ledgerTransaction.CurrencySign;

            }

        }
    }

    RecociliationNumbers: string[] = [];
    getRecoLinkList()
    {
        var res: string[] = [];

        if (this.RecoNumber) {
            res = this.RecoNumber.split(',');
        }
        return res;

    }
}
