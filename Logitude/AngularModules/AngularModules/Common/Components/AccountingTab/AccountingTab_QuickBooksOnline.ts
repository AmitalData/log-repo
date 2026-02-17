import {Component, OnDestroy} from '@angular/core';
import {AppTool} from '../../../Infrastructure/Tools';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {CurrencyList} from '../../EntityLists/CurrencyList';
import {CurrencyListService} from '../../Services/StandardLists/CurrencyListService';
import {CardExternalCodeByCurrencyPM} from '../../EntityPMs/CardExternalCodeByCurrencyPM';
import {GlobalDomainService}  from '../../Services/GlobalDomainService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {GetAccountingSystemWindowArgs} from '../../Args';

@Component({
    moduleId: module.id,
    templateUrl: './AccountingTab_QuickBooksOnline.html',
})

export class AccountingTab_QuickBooksOnline extends BaseComponent implements OnDestroy {
    public EntityPM: any = null;
    public ObjectTableName: string;
    public DataContext = this;
    public Loading: boolean = false;
    public LoadingSecond: boolean = false;
    public PayablesFailed: boolean = false;
    public ReceivablesFailed: boolean = false;
    public ExternalIDPayableOrReceivable: string = "";
    public CurrenciesListFilterd: CustomerCurrencies[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs) {
        super();
        this.EntityPM = entityArgs.EntityPM;
        this.ObjectTableName = entityArgs.ObjectTableName;
        this.SetCustomersParams();
        this.InitializeServices();
        this.CheckTextAccountingCurrencySimilarity();
        this.GetAccountingSystemType();
        this.Listen();
    }

    public PayableAndRecievableCard: boolean = false;
    public RecievableCardOnly: boolean = false;
    public PayableCardOnly: boolean = false;
    public OtherEntities: boolean = false;

    SetCustomersParams() {
        this.PayableAndRecievableCard = false;
        this.RecievableCardOnly = false;
        this.PayableCardOnly = false;
        this.OtherEntities = false;
        switch (this.ObjectTableName) {
            case "CustomAgent":
            case "Agent":
            case "ShippingAgent":
            case "ShippingLine":
            case "Trucker":
            case "Vendor":
            case "Airline": {
                this.PayableAndRecievableCard = true;
                break;
            }
            case "Customer": {
                this.RecievableCardOnly = true;
                this.ExternalIDPayableOrReceivable = "Account Receivables External Id";
                break;
            }
            case "Warehouse": {
                this.PayableCardOnly = true;
                this.ExternalIDPayableOrReceivable = "Account Payables External Id";
                break;
            }

            default: {
                this.OtherEntities = true;
                break;
            }
        }
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            if (this.SaveCompletedEvent == null) {
                this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
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

    private CurrenciesList: CurrencyList[] = [];
    private ItemName: string = null;
    private CustomItem: string = null;
    private BlockTranslation: boolean = false;
    private ExistRecordOtherExternalFromAPi: boolean = false;
    private ExistPayablesRecordOtherExternalFromAPi: boolean = false;

    
    
    private cardExternalCodeByCurrencyPM: CardExternalCodeByCurrencyPM;
    private GlobalDomainService: GlobalDomainService;
    private currencyService: CurrencyListService;
    InitializeServices() {
        this.currencyService = new CurrencyListService();
        this.GlobalDomainService = new GlobalDomainService();
    }

    FailedLoading(index=null, type=null) {
        if (type == "R")
            return !this.CurrenciesListFilterd[index].ReceivableLoaded && this.CurrenciesListFilterd[index].ReceivableFirstLoaded;
        else if (type == "P")
            return !this.CurrenciesListFilterd[index].PayableLoaded && this.CurrenciesListFilterd[index].PayableFirstLoaded;
        else
            return this.Loading;
    }

    FailedLoadingSecond() {
        return this.LoadingSecond;
    }
    imgVisibile(index,type) {
        if (type=="R") {
            if (!AppTool.IsNullOrEmpty(this.CurrenciesListFilterd[index].ReceivableCardAPIID) && AppTool.IsNullOrEmpty(this.CurrenciesListFilterd[index].RecievableCustomerExternalName))
                return true;
            return false;
        }
        else if (type=="P") {
            if (!AppTool.IsNullOrEmpty(this.CurrenciesListFilterd[index].PayableCardAPIID) && AppTool.IsNullOrEmpty(this.CurrenciesListFilterd[index].PayableCustomerExternalName))
                return true;
            return false;
        }

    }

    imgVisibileOthers() {
        if (AppTool.IsNullOrEmpty(this.TextBoxText) && this.ExistRecordOtherExternalFromAPi)
            return true;
        return false;

    }
    imgVisibilePayables() {
        if (AppTool.IsNullOrEmpty(this.ChargeTypeTextBoxText) && this.ExistPayablesRecordOtherExternalFromAPi)
            return true;
        return false;
    }

    RefreshLoadingData() {
        this.Loading = false;
        this.ReceivablesFailed = false;
        this.GetAccountingSystemType();
    }


    RefreshLoadingDataSecond() {
        this.LoadingSecond = false;
        this.PayablesFailed = false;
        this.GetAccountingSystemType();
    }

    RefreshLoadingDataCustomer(item, index, type) {
        if (type == "R") {
            this.CurrenciesListFilterd[index].ReceivableLoaded = true;
            this.GlobalDomainService.GetQuickBooksOnlineCustomerById(item.ReceivableCardAPIID).subscribe(result => {
                if (result.Result != null && result.Result.length > 0) {
                    this.CurrenciesListFilterd[index].RecievableCustomerExternalName = result.Result[0].displayNameField;
                }
                else {
                    this.CurrenciesListFilterd[index].ReceivableLoaded = false;
                }

            });
        }
        else if (type == "P") {
            this.CurrenciesListFilterd[index].PayableLoaded = true;
            this.GlobalDomainService.GetQuickBooksOnlineCustomerById(item.ReceivableCardAPIID).subscribe(result => {
                if (result.Result != null && result.Result.length > 0) {
                    this.CurrenciesListFilterd[index].PayableCustomerExternalName = result.Result[0].displayNameField;
                }
                else {
                    this.CurrenciesListFilterd[index].PayableLoaded = false;
                }

            });

        }

    }

    CheckTextAccountingCurrencySimilarity() {
        if (this.ObjectTableName == "Currency") {
            if (SessionLocator.AccountingCurrencyId == this.EntityPM.Id) {
                this.TextBoxText = "No need to translate your accounting currency";
                this.BlockTranslation = true;
                this.Loading = false;
            }

            else {
                this.BlockTranslation = false;
            }
        }
        else {
            this.BlockTranslation = false;
        }
    }
    GetAccountingSystemType() {
        if (this.RecievableCardOnly || this.PayableAndRecievableCard || this.PayableCardOnly) {
            if (!this.BlockTranslation) {
                setTimeout(() => this.FillDataIfCustomerExternalFromApi(), 500);
            }
        }
        else {
            if (!this.BlockTranslation) {
                this.FillDataIfOtherExternalFromApi();
            }
        }
    }
    FillDataIfCustomerExternalFromApi() {
        this.CurrenciesListFilterd = [];
        this.currencyService.getAllFromCache().subscribe(result => {
            this.CurrenciesList = result.Result;
            this.CurrenciesList.forEach(currency => {
                var item: CustomerCurrencies = new CustomerCurrencies();
                item.CurrencyCode = currency.Code;
                item.CurrencyId = currency.Id;
                this.CurrenciesListFilterd.push(item);
            });
            var index = 0;
            this.CurrenciesListFilterd.forEach(p => {
                this.EntityPM.CardExternalCodeByCurrencies.forEach(p2 => {
                    if (p2.CurrencyId == p.CurrencyId) {
                        p.ReceivableCardAPIID = p2.ExternalRecievableTableId;
                        p.PayableCardAPIID = p2.ExternalPayableTableId;
                        return;
                    }
                });
                // document.getElementById("img"+index).style.display = 'none';


                if ((p.ReceivableCardAPIID != null && p.ReceivableCardAPIID != "") || (p.PayableCardAPIID != null && p.PayableCardAPIID != "")) {
                    var item2: CustomerCurrencies = new CustomerCurrencies();
                    item2.ReceivableCardAPIID = p.ReceivableCardAPIID;
                    item2.PayableCardAPIID = p.PayableCardAPIID;
                    item2.CurrencyCode = p.CurrencyCode;
                    item2.CurrencyId = p.CurrencyId;
                    item2.index = index;
                    //     document.getElementById("img" + item2.index).style.display = 'visibile';

                    if (this.PayableAndRecievableCard) {
                        if (p.ReceivableCardAPIID != null && p.ReceivableCardAPIID != ""){
                            this.GlobalDomainService.GetQuickBooksOnlineCustomerById(p.ReceivableCardAPIID).subscribe(result => {
                                if (result.Result != null && result.Result.length > 0) {
                                    item2.ReceivableLoaded = true;
                                    item2.RecievableCustomerExternalName = result.Result[0].displayNameField;
                                }
                                this.CurrenciesListFilterd[item2.index] = item2;
                                item2.ReceivableFirstLoaded = true;
                                //   document.getElementById("img" + item2.index).style.display = 'none';

                            });
                        }
                        if (p.PayableCardAPIID != null && p.PayableCardAPIID != "") {

                            this.GlobalDomainService.GetQuickBooksOnlineVendorById(p.PayableCardAPIID).subscribe(result => {
                                if (result.Result != null && result.Result.length > 0) {
                                    item2.PayableLoaded = true;
                                    item2.PayableCustomerExternalName = result.Result[0].displayNameField;
                                }
                                this.CurrenciesListFilterd[item2.index] = item2;
                                item2.PayableFirstLoaded = true;
                                //   document.getElementById("img" + item2.index).style.display = 'none';

                            });
                        }

                    }
                    else if (this.PayableCardOnly) {
                        this.GlobalDomainService.GetQuickBooksOnlineVendorById(p.PayableCardAPIID).subscribe(result => {
                            if (result.Result != null && result.Result.length > 0) {
                                item2.PayableLoaded = true;
                                item2.PayableCustomerExternalName = result.Result[0].displayNameField;
                            }
                            this.CurrenciesListFilterd[item2.index] = item2;
                            item2.PayableFirstLoaded = true;

                        });

                    }

                    else if (this.RecievableCardOnly) {
                        this.GlobalDomainService.GetQuickBooksOnlineCustomerById(p.ReceivableCardAPIID).subscribe(result => {
                            if (result.Result != null && result.Result.length > 0) {
                                item2.ReceivableLoaded = true;
                                item2.RecievableCustomerExternalName = result.Result[0].displayNameField;
                            }
                            this.CurrenciesListFilterd[item2.index] = item2;
                            item2.ReceivableFirstLoaded = true;
                        });

                    }
                }
                index++;


            });
        });

    }
    FillDataIfOtherExternalFromApi() {

        if (this.ObjectTableName == "VatType") {
            if (this.EntityPM.ExternalVATCard != null && this.EntityPM.ExternalVATCard != "") {
                this.ExistRecordOtherExternalFromAPi = true;
                this.GlobalDomainService.GetQuickBooksOnlineVatTypesById(this.EntityPM.ExternalVATCard).subscribe(result => {
                    if (result.Result != null && result.Result.length > 0) {
                        this.TextBoxText = result.Result[0].nameField;
                        this.Loading = true;
                        this.ReceivablesFailed = false;
                    }
                    else {
                        this.Loading = false;
                        this.ReceivablesFailed = true;
                    }
                });

            }

        }

        else if (this.ObjectTableName == "PaymentTerm") {
            if (this.EntityPM != null)
                if (this.EntityPM.ExternalId != null && this.EntityPM.ExternalId != "") {
                    this.ExistRecordOtherExternalFromAPi = true;
                    this.GlobalDomainService.GetQuickBooksOnlinePaymentTermsById(this.EntityPM.ExternalId).subscribe(result => {
                        if (result.Result != null && result.Result.length > 0) {
                            this.TextBoxText = result.Result[0].nameField;
                            this.Loading = true;
                            this.ReceivablesFailed = false;
                        }
                        else {
                            this.Loading = false;
                            this.ReceivablesFailed = true;
                        }
                        
                    });
                }



        }


        else if (this.ObjectTableName == "Currency") {
            if (this.EntityPM != null)
                if (this.EntityPM.AccountingExternalCode != null && this.EntityPM.AccountingExternalCode != "") {
                    this.ExistRecordOtherExternalFromAPi = true;
                    this.GlobalDomainService.GetQuickBooksOnlineCurrenciesById(this.EntityPM.AccountingExternalCode).subscribe(result => {
                        if (result.Result != null && result.Result.length > 0) {
                            this.TextBoxText = result.Result[0].codeField;
                            this.Loading = true;
                            this.ReceivablesFailed = false;
                        }
                        else {
                            this.Loading = false;
                            this.ReceivablesFailed = true;
                        }
                    });



                }
        }

        else if (this.ObjectTableName == "ARPaymentMethod") {
            if (this.EntityPM != null)
                if (this.EntityPM.AccountingExternalId != null && this.EntityPM.AccountingExternalId != "") {
                    this.ExistRecordOtherExternalFromAPi = true;
                    this.GlobalDomainService.GetQuickBooksOnlinePaymentMethodsById(this.EntityPM.AccountingExternalId).subscribe(result => {
                        if (result.Result != null && result.Result.length > 0) {
                            this.TextBoxText = result.Result[0].nameField;
                            this.Loading = true;
                            this.ReceivablesFailed = false;
                        }
                        else {
                            this.Loading = false;
                            this.ReceivablesFailed = true;
                        }
                    });



                }
        }


        else if (this.ObjectTableName == "APPaymentMethod") {
            if (this.EntityPM != null)
                if (this.EntityPM.AccountingExternalId != null && this.EntityPM.AccountingExternalId != "") {
                    this.ExistRecordOtherExternalFromAPi = true;
                    this.GlobalDomainService.GetQuickBooksOnlinePayablesChargesTypesById(this.EntityPM.AccountingExternalId).subscribe(result => {
                        if (result.Result != null && result.Result.length > 0) {
                            this.TextBoxText = result.Result[0].nameField;
                            this.Loading = true;
                            this.ReceivablesFailed = false;
                        }
                        else {
                            this.Loading = false;
                            this.ReceivablesFailed = true;
                        }
                    });



                }
        }

        else if (this.ObjectTableName == "ChargesType") {
            if (this.EntityPM != null)
                if (this.EntityPM.ReceivablesChargesTypeExternalCode != null && this.EntityPM.ReceivablesChargesTypeExternalCode != "") {
                    this.ExistRecordOtherExternalFromAPi = true;
                   
                    this.GlobalDomainService.GetQuickBooksOnlineReceivableChargesTypesById(this.EntityPM.ReceivablesChargesTypeExternalCode).subscribe(result => {
                        if (result.Result != null && result.Result.length > 0) {
                            this.TextBoxText = result.Result[0].fullyQualifiedNameField;
                            this.Loading = true;
                            this.ReceivablesFailed = false;
                        }
                        else {
                            this.Loading = false;
                            this.ReceivablesFailed = true;
                        }

                    });

                    
                 
                }

            if (this.EntityPM.PayablesChargesTypeExternalCode != null && this.EntityPM.PayablesChargesTypeExternalCode != ""){
                this.ExistPayablesRecordOtherExternalFromAPi = true;
                this.GlobalDomainService.GetQuickBooksOnlinePayablesChargesTypesById(this.EntityPM.PayablesChargesTypeExternalCode).subscribe(result => {
                    if (result.Result != null && result.Result.length > 0) {
                        this.ChargeTypeTextBoxText = result.Result[0].nameField;
                        this.LoadingSecond = true;
                        this.PayablesFailed = false;
                    }
                    else {
                        this.LoadingSecond = false;
                        this.PayablesFailed = true;
                    }
                });


            }
        }



        else if (this.ObjectTableName == "AccountingPaymentMethod") {
            if (this.EntityPM != null)
                if (this.EntityPM.ARExternalId != null && this.EntityPM.ARExternalId != "") {
                    this.ExistRecordOtherExternalFromAPi = true;

                    this.GlobalDomainService.GetQuickBooksOnlinePaymentMethodsById(this.EntityPM.ARExternalId).subscribe(result => {
                        if (result.Result != null && result.Result.length > 0) {
                            this.TextBoxText = result.Result[0].nameField;
                            this.Loading = true;
                            this.ReceivablesFailed = false;
                        }
                        else {
                            this.Loading = false;
                            this.ReceivablesFailed = true;
                        }
                    });



                }

            if (this.EntityPM.APExternalId != null && this.EntityPM.APExternalId != "") {
                this.ExistPayablesRecordOtherExternalFromAPi = true;
                
                this.GlobalDomainService.GetQuickBooksOnlinePayablesChargesTypesById(this.EntityPM.APExternalId).subscribe(result => {
                    if (result.Result != null && result.Result.length > 0) {
                        this.ChargeTypeTextBoxText = result.Result[0].nameField;
                        this.LoadingSecond = true;
                        this.PayablesFailed = false;
                    }
                    else {
                        this.LoadingSecond = false;
                        this.PayablesFailed = true;
                    }
                });             

            }
        }

    }

    private Item: any;
    private isWindowOpened: boolean = false;
    private isPayables: boolean = false;

    private subTableName: string;
    OpenARPaymentMethods() {
        this.subTableName = "ARPaymentMethod";
        this.OpenQuickBooks();
    }
    OpenAPPaymentMethods() {
        this.subTableName = "APPaymentMethod";
        this.OpenQuickBooks();
    }

    OpenChargesTypesPayables() {
        this.isPayables = true;
        this.OpenQuickBooks();
    }



    OpenQuickBooks(currency = null,type:string=null) {
        if (!this.isWindowOpened && !this.BlockTranslation) {
            this.isWindowOpened = true;
            var windowTitle = "";
            var windowArgs = new GetAccountingSystemWindowArgs();
            windowArgs.PayableCard = false;

            windowArgs.SelectedEntity = null;

            if (this.isPayables) {
                windowArgs.CardName = "Account";
                windowArgs.SearchField = "Name";
                windowArgs.LogitudeCardName = "ChargesType";
                windowTitle = "Payables ChargesTypes In " + SessionLocator.AccountingSystemPM.Name;

            }
            
            else if (this.ObjectTableName == "Customer") {
                windowArgs.CardName = "Customer";
                windowArgs.SearchField = "displayName";
                windowArgs.LogitudeCardName = "Customer";
                windowTitle = "Search Customers In " + SessionLocator.AccountingSystemPM.Name;

            }

            else if (this.ObjectTableName == "Agent") {
                windowArgs.CardName = "Customer";
                windowArgs.SearchField = "displayName";
                windowArgs.LogitudeCardName = "Agent";
                windowTitle = "Search Agent In " + SessionLocator.AccountingSystemPM.Name;
            }
          

            else if (this.ObjectTableName == "VatType") {
                windowArgs.CardName = "TaxCode";
                windowArgs.SearchField = "Name";
                windowArgs.LogitudeCardName = "VatType";
                windowTitle = "VatTypes in " + SessionLocator.AccountingSystemPM.Name;;

            }


            else if (this.ObjectTableName == "ChargesType") {
                windowArgs.CardName = "Item";
                windowArgs.SearchField = "Name";
                windowArgs.LogitudeCardName = "ChargesType";
                windowTitle = "Receivables ChargesTypes in " + SessionLocator.AccountingSystemPM.Name;;

            }


            else if (this.ObjectTableName == "PaymentTerm") {
                windowArgs.CardName = "Term";
                windowArgs.SearchField = "Name";
                windowArgs.LogitudeCardName = "PaymentTerm";
                windowTitle = "PaymentTerms in " + SessionLocator.AccountingSystemPM.Name;;

            }

            else if (this.ObjectTableName == "Currency") {
                windowArgs.CardName = "CompanyCurrency";
                windowArgs.SearchField = "Code";
                windowArgs.LogitudeCardName = "Currency";
                windowTitle = "Currencies in " + SessionLocator.AccountingSystemPM.Name;;

            }

            else if (this.ObjectTableName == "Warehouse") {
                windowArgs.CardName = "Vendor";
                windowArgs.SearchField = "displayName";
                windowArgs.LogitudeCardName = "Warehouse";
                windowTitle = "Warehouses in " + SessionLocator.AccountingSystemPM.Name;

            }

            else if (this.ObjectTableName == "Airline") {
                windowArgs.LogitudeCardName = "Airline";
                windowTitle = "Airlines in " + SessionLocator.AccountingSystemPM.Name;

                }

            else if (this.ObjectTableName == "CustomAgent") {
                windowArgs.LogitudeCardName = "CustomAgent";
                windowTitle = "Customs Agents in " + SessionLocator.AccountingSystemPM.Name;

                }

            else if (this.ObjectTableName == "ShippingAgent") {
                windowArgs.LogitudeCardName = "ShippingAgent";
                windowTitle = "Shipping Agents in " + SessionLocator.AccountingSystemPM.Name;

                }

            else if (this.ObjectTableName == "ShippingLine") {
                windowArgs.LogitudeCardName = "ShippingLine";
                windowTitle = "Shipping Lines in " + SessionLocator.AccountingSystemPM.Name;

            }


            else if (this.ObjectTableName == "Trucker") {
                windowArgs.LogitudeCardName = "Trucker";
                windowTitle = "Truckers in " + SessionLocator.AccountingSystemPM.Name;

            }
                 else if (this.ObjectTableName == "Vendor") {
                     windowArgs.LogitudeCardName = "Vendor";
                     windowTitle = "Vendors in " + SessionLocator.AccountingSystemPM.Name;
            }
            else if (this.subTableName == "ARPaymentMethod") {
                windowArgs.CardName = "PaymentMethod";
                windowArgs.SearchField = "Name";
                windowArgs.LogitudeCardName = "ARPayment";
                windowTitle = "Payment Methods In " + SessionLocator.AccountingSystemPM.Name;
            }

            else if (this.subTableName == "APPaymentMethod") {
                windowArgs.CardName = "AccountBankCredit";
                windowArgs.SearchField = "Name";
                windowArgs.LogitudeCardName = "APPayment";
                windowTitle = "Payment Account Types In " + SessionLocator.AccountingSystemPM.Name;
             }

          

            if (type == "P")
                windowArgs.PayableCard = true;
            else if (type == "R")
                windowArgs.ReceivableCard = true;

            var logWindow = new LogitudeWindow();
            logWindow.Width = 960;
            logWindow.WindowArgs = windowArgs;
            logWindow.Height = 600;
            logWindow.Title = windowTitle;
            logWindow.Show('./CommonModules/CommonCustomer/Components/EditTabs/QuickBooksComponent');
            var item = null;

            logWindow.WindowClosed.subscribe(s => {
                this.isWindowOpened = false;
                if (windowArgs.SelectedEntity != null) {

                    this.Item = windowArgs.SelectedEntity;

                    if (type=="R") {
                        this.ItemName = this.Item.displayNameField;
                        var cardExternal: CardExternalCodeByCurrencyPM = new CardExternalCodeByCurrencyPM(null);
                        cardExternal.Tenant = SessionLocator.TenantPM.Id;
                        cardExternal.CardId = this.EntityPM.Id;
                        cardExternal.ExternalRecievableTableId = this.Item.idField;
                        cardExternal.CurrencyId = currency.CurrencyId;
                        currency.RecievableCustomerExternalName = this.Item.displayNameField;
                        var Exist: boolean = this.EntityPM.CardExternalCodeByCurrencies.filter(p => p.CurrencyId == currency.CurrencyId)[0] == null;
                        if ((currency.ReceivableCardAPIID == null || currency.ReceivableCardAPIID == "") && Exist) {
                            this.EntityPM.AddCardExternalCodeByCurrencyPM(cardExternal);
                            currency.ReceivableCardAPIID = this.Item.idField;
                        }
                        else {
                            this.EntityPM.CardExternalCodeByCurrencies.filter(p => p.CurrencyId == currency.CurrencyId)[0].ExternalRecievableTableId = cardExternal.ExternalRecievableTableId;
                        }

                    }

                    else if (type=="P") {
                        this.ItemName = this.Item.displayNameField;
                        var cardExternal: CardExternalCodeByCurrencyPM = new CardExternalCodeByCurrencyPM(null);
                        cardExternal.Tenant = SessionLocator.TenantPM.Id;
                        cardExternal.CardId = this.EntityPM.Id;
                        cardExternal.ExternalPayableTableId = this.Item.idField;
                        cardExternal.CurrencyId = currency.CurrencyId;
                        currency.PayableCustomerExternalName = this.Item.displayNameField;
                        var Exist: boolean = this.EntityPM.CardExternalCodeByCurrencies.filter(p => p.CurrencyId == currency.CurrencyId)[0] == null;
                        if ((currency.PayableCardAPIID == null || currency.PayableCardAPIID == "") && Exist) {
                            this.EntityPM.AddCardExternalCodeByCurrencyPM(cardExternal);
                            currency.PayableCardAPIID = this.Item.idField;
                        }
                        else {
                            this.EntityPM.CardExternalCodeByCurrencies.filter(p => p.CurrencyId == currency.CurrencyId)[0].ExternalPayableTableId = cardExternal.ExternalPayableTableId;
                        }
                    }

                    else if (this.OtherEntities) {
                        switch (this.ObjectTableName) {
                            case "VatType": {
                                this.EntityPM.ExternalVATCard = this.Item.idField;
                                this.TextBoxText = this.Item.nameField;
                                break;
                            }

                            case "PaymentTerm": {
                                this.EntityPM.ExternalId = this.Item.idField;
                                this.TextBoxText = this.Item.nameField;
                                break;
                            }

                            case "Currency": {
                                this.EntityPM.AccountingExternalCode = this.Item.codeField;
                                this.TextBoxText = this.Item.nameField;
                                break;
                            }

                           

                            case "ChargesType": {
                                if (!this.isPayables) {
                                    this.EntityPM.ReceivablesChargesTypeExternalCode = this.Item.idField;
                                    this.TextBoxText = this.Item.fullyQualifiedNameField;
                                }
                                else {
                                    this.EntityPM.PayablesChargesTypeExternalCode = this.Item.idField;
                                    this.ChargeTypeTextBoxText = this.Item.nameField;
                                }
                                break;
                            }

                            case "AccountingPaymentMethod": {

                                if (this.subTableName== "ARPaymentMethod") {
                                this.EntityPM.ARExternalId = this.Item.idField;
                                this.TextBoxText = this.Item.nameField;
                            }

                                if (this.subTableName== "APPaymentMethod") {
                                this.EntityPM.APExternalId = this.Item.idField;
                                this.ChargeTypeTextBoxText = this.Item.nameField;
                                break;
                            }


                                break;
                            }


                        }
                    }
                }
                this.isPayables = false;

            });
        }
    }

    private textBoxText: string = null;
    get TextBoxText() { return this.textBoxText; }
    set TextBoxText(value: string) {
        if (this.textBoxText != value) {
            this.textBoxText = value;
        }
    }


    private chargeTypeTextBoxText: string = null;
    get ChargeTypeTextBoxText() { return this.chargeTypeTextBoxText; }
    set ChargeTypeTextBoxText(value: string) {
        if (this.chargeTypeTextBoxText != value) {
            this.chargeTypeTextBoxText = value;
        }
    }

}
export class CustomerCurrencies {
    constructor() {
        this.CurrencyCode = "";
        this.RecievableCustomerExternalName = "";
        this.PayableCustomerExternalName = "";
        this.CurrencyId = "";
        this.ReceivableCardAPIID = "";
        this.PayableCardAPIID = "";
        this.index = 0;
        this.ReceivableLoaded = false;
        this.PayableLoaded = false;
    }
    public CurrencyCode: string;
    public RecievableCustomerExternalName: string;
    public PayableCustomerExternalName: string;
    public CurrencyId: string;
    public ReceivableCardAPIID: string;
    public PayableCardAPIID: string;
    public index: number;
    public ReceivableLoaded: boolean;
    public PayableLoaded: boolean;
    public ReceivableFirstLoaded: boolean;
    public PayableFirstLoaded: boolean;

}
