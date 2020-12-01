import {Component, OnDestroy}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {ARInvoicePM} from '../../../../Invoice/EntityPMs/ARInvoicePM';
import {ARInvoicePaymentPM} from '../../../../Invoice/EntityPMs/ARInvoicePaymentPM';
import {DateTool, AppTool} from '../../../../Infrastructure/Tools';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {ARPaymentList} from '../../../../Invoice/EntityLists/ARPaymentList';
import {ARPaymentListService} from '../../../../Invoice/Services/StandardLists/ARPaymentListService';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    
    templateUrl: './ARInvoicePaymentsTabComponent.html',
})

export class ARInvoicePaymentsTabComponent implements OnDestroy {
    public EntityPM: ARInvoicePM = null;
    public ObjectTableName = "ARInvoice";
    public DataContext = this;
    public ItemsSource1: ARInvoicePaymentItem[] = [];
    public ItemsSource2: ARInvoicePaymentItem[] = [];
    public ItemsSource1Hidden: boolean = false;
    public ItemsSource2Hidden: boolean = false;
    public IsResourcesReady: boolean = false;
    public isRTL: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, private entityResourceService: EntityResourceService) {
        if (ObjectsLocator.GlobalSetting) {
            this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        }

        this.EntityPM = entityArgs.EntityPM;

        entityResourceService.getEntityResourceByTableName("ARPayment", 0).subscribe((response:any) => {
            this.IsResourcesReady = true;
            this.SetUIProperties();
            this.Listen();
            this.LoadInvoicePayments();
        });
    }

    private SessionEvent: any = null;
    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;  
    private Listen() {
        if (this.entityArgs.EditComponent != null) {

            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(s => {
                if (s == "NewARPaymentInvoiceTabCreated") {
                    this.entityArgs.EditComponent.ReloadEntityPM();
                }
            });

            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.SetUIProperties();
                    this.LoadInvoicePayments();
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.SetUIProperties();
                    this.LoadInvoicePayments();
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SessionEvent);
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }

    public IsEditingEnabled: boolean = false;
    public AddPaymentButtonIsEnabled: boolean = false;
    SetUIProperties() {
        var isEditingEnabled = true;
        var isAddButtonEnabled = true;

        if (this.EntityPM.StatusCode == "PD" || this.EntityPM.StatusCode == "VD" || this.EntityPM.StatusCode == "LL") {
            isEditingEnabled = false;
        }

        if (!isEditingEnabled) {
            isAddButtonEnabled = false;
        }

        else if (this.EntityPM.IsConstituentInvoice) {
            isAddButtonEnabled = false;
        }

        //else if (this.EntityPM.ARInvoiceTypeCode == "CD") {
        //    isAddButtonEnabled = false;
        //}

        else if (AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "DR") {
            isAddButtonEnabled = false;
        }

        this.IsEditingEnabled = isEditingEnabled;
        this.AddPaymentButtonIsEnabled = isAddButtonEnabled;
    }

    get IsConstituentInvoice() { return this.EntityPM.IsConstituentInvoice; }
    set IsConstituentInvoice(newValue: boolean) {
        if (this.EntityPM.IsConstituentInvoice != newValue) {
            this.EntityPM.IsConstituentInvoice = newValue;
        }
    }

    get ARInvoiceTypeCode() { return this.EntityPM.ARInvoiceTypeCode; }
    set ARInvoiceTypeCode(newValue: string) {
        if (this.EntityPM.ARInvoiceTypeCode != newValue) {
            this.EntityPM.ARInvoiceTypeCode = newValue;
        }
    }
    
    get InvoiceCurrencyCode() { return this.EntityPM.InvoiceCurrencyCode; }
    get InvoiceAmount() { return this.EntityPM.AmountInInvoiceCurrency; }
    get AmountPaid() { return this.EntityPM.AmountInInvoiceCurrency - this.EntityPM.AmountDue; }
    get AmountDue() { return this.EntityPM.AmountDue; }

    private myService: ARPaymentListService;
    public IsNoPermissionVisible: boolean = false;
    LoadInvoicePayments() {

        var isLoading = true;
        this.ItemsSource1 = [];
        this.ItemsSource2 = [];

        this.LoadConnectedPayments();

        if (AppTool.IsNullOrEmpty(this.EntityPM.Id) || AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "DR" || this.EntityPM.StatusCode == "VD") {
            isLoading = false;
        }

        if (!FeatureLocator.HasFeaturePermession("ARPayment", "READ")) {
            isLoading = false;
            this.IsNoPermissionVisible = true;
        }

        if (this.EntityPM.IsClosed) {
            isLoading = false;
        }

        if (isLoading) {

            this.CurrentSession.StartBusyIndicatorLoading();

            if (this.myService == null) {
                this.myService = new ARPaymentListService();
            }

            var filters = new ApiQueryFilters();
            filters.PageIndex = 0;
            filters.PageSize = 500;

            filters.Filter1Name = "BillToId";
            filters.Filter1Value = this.EntityPM.BillToId;
            filters.Filter1Operator = "Equals";

            filters.Filter2Name = "StatusCode";
            filters.Filter2Value = "DR,AD,PP,PD,CL";
            filters.Filter2Operator = "InList";

            filters.Filter3Name = "IsClosed";
            filters.Filter3Value = false;
            filters.Filter3Operator = "Equals";

            this.myService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {

                    var list: ARPaymentList[] = myResponse.Result;
                    var unConnectedMatchedList: ARPaymentList[] = [];
                    var unConnectedListNotMatched: ARPaymentList[] = [];

                    list = list.sort(function (a, b) { return a.PaymentNo.toLowerCase() == b.PaymentNo.toLowerCase() ? 0 : a.PaymentNo.toLowerCase() < b.PaymentNo.toLowerCase() ? -1 : 1; });

                    list.forEach(item => {
                        if (this.EntityPM.InvoicePayments.filter(f => f.ARPaymentId == item.Id).length == 0) {
                            if (item.PaymentCurrencyId != this.EntityPM.InvoiceCurrencyId || item.OpenAmount <= 0) {
                                unConnectedListNotMatched.push(item);
                            }

                            else {
                                unConnectedMatchedList.push(item);
                            }
                        } 
                    });

                    unConnectedMatchedList.forEach(item => {
                        this.ItemsSource2.push(new ARInvoicePaymentItem(item, this));
                    });

                    unConnectedListNotMatched.forEach(item => {
                        this.ItemsSource2.push(new ARInvoicePaymentItem(item, this));
                    });
                }

                this.CurrentSession.StopBusyIndicator();
            });
        }
    }
    LoadConnectedPayments() {

        var ids: string = null;

        this.EntityPM.InvoicePayments.forEach(item => {
            if (ids) {
                ids += "," + item.ARPaymentId;
            }

            else {
                ids = item.ARPaymentId;
            }
        });

        if (ids) {

            var filters = new ApiQueryFilters();
            filters.PageIndex = 0;
            filters.PageSize = 500;

            filters.Filter1Name = "Id";
            filters.Filter1Value = ids;
            filters.Filter1Operator = "InList";

            var service = new ARPaymentListService();

            service.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
                if (!myResponse.HasError) {

                    var list: ARPaymentList[] = myResponse.Result;

                    list = list.sort(function (a, b) { return a.PaymentNo.toLowerCase() == b.PaymentNo.toLowerCase() ? 0 : a.PaymentNo.toLowerCase() < b.PaymentNo.toLowerCase() ? -1 : 1; });

                    list.forEach(item => {
                        this.ItemsSource1.push(new ARInvoicePaymentItem(item, this));
                    });
                }
            });
        }
    }

    RunReachedBoundsMessage() {
        var messageText = TextCodeTranslator.Translate("ARInvoice.M.AmountPaidBiggerThanInvoiceAmount");
        var window = new MessageWindow();
        window.Width = 400;
        window.Show(messageText);
    }

    // Add Payment Button 
    AddPaymentClicked() {
        var message = "";
        if (!FeatureLocator.HasEntityPermessions("ARPayment", "NEW", true)) {
            return;
        }

        var totalAmount = 0;
        this.EntityPM.InvoicePayments.forEach(item => {
            totalAmount += item.LocalAmount;
        });

        if (this.EntityPM.StatusCode == "DR") {
            var messageText = TextCodeTranslator.Translate("ARInvoice.M.CantAddPaymentForDraftInvoice");
            var window = new MessageWindow();
            window.Width = 300;
            window.Show(messageText);
        }

        else if (this.EntityPM.AmountDue <= 0) {
            this.RunReachedBoundsMessage();
        }

        else {
            var str = TextCodeTranslator.Translate("General.O.NewEntity");
            str = str.replace("%Entity", TextCodeTranslator.TranslateTable("ARPayment"));

            var logWindow = new LogitudeWindow();
            logWindow.WindowArgs = { ARInvoice: this.EntityPM};
            logWindow.Title = str;
            logWindow.Show("./InvoiceModules/ARPayment/Components/NewEntity/NewARPaymentComponent");
        }
    }
}
export class ARInvoicePaymentItem {
    public IsConnected: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private item: ARPaymentList, private fatherComponent: ARInvoicePaymentsTabComponent) {

        if (this.fatherComponent.EntityPM.InvoicePayments.filter(f => f.ARPaymentId == this.item.Id).length > 0) {
            this.IsConnected = true;
        }

        this.SetUIProperties();
    }

    public IsNotMatchedVisibile: boolean = false;
    public IsConnectButtonVisibile: boolean = false;
    public ConnectButtonIsEnabled: boolean = false;
    public DisconnectButtonIsEnabled: boolean = false;
    public ForeignCurrencyCodeBackground: string = "transparent";
    public ForeignCurrencyCodeForeground: string = "#282E30";
    public OpenAmountBackground: string = "transparent";
    public OpenAmountForeground: string = "#282E30";
    SetUIProperties() {

        this.DisconnectButtonIsEnabled = this.fatherComponent.EntityPM.StatusCode == "LL" ? false : true;

        var isConnectEnabled = true;
        if (this.fatherComponent.EntityPM.ARInvoiceTypeCode == "CD" || this.fatherComponent.EntityPM.IsConstituentInvoice || this.fatherComponent.EntityPM.StatusCode == "LL") {
            isConnectEnabled = false;
        }
        this.ConnectButtonIsEnabled = isConnectEnabled;

        if (this.IsConnected) {
            this.ForeignCurrencyCodeBackground = "transparent";
            this.ForeignCurrencyCodeForeground = "#282E30";
            this.OpenAmountBackground = "transparent";
            this.OpenAmountForeground = "#282E30";
        }

        else {
            if (this.item.PaymentCurrencyId != this.fatherComponent.EntityPM.InvoiceCurrencyId) {
                this.ForeignCurrencyCodeBackground = "rgba(255, 171, 3, 0.6)";
                this.ForeignCurrencyCodeForeground = "rgba(255, 94, 0, 1)";
            }

            if (this.item.OpenAmount <= 0) {
                this.OpenAmountBackground = "rgba(255, 171, 3, 0.6)";
                this.OpenAmountForeground = "rgba(255, 94, 0, 1)";
            }

            if (!this.fatherComponent.EntityPM.IsConstituentInvoice) {
                if (this.item.PaymentCurrencyId != this.fatherComponent.EntityPM.InvoiceCurrencyId || this.OpenAmount <= 0) {
                    this.IsNotMatchedVisibile = true;
                }

                if (this.item.PaymentCurrencyId == this.fatherComponent.EntityPM.InvoiceCurrencyId && this.OpenAmount > 0) {
                    this.IsConnectButtonVisibile = true;
                }
            }
        }


    }

    get PaymentNo() { return this.item.PaymentNo; }
    get ForeignAmount() { return this.item.AmountInPaymentCurrency; }
    get ForeignCurrencyCode() { return this.item.PaymentCurrencyCode; }
    get OpenAmount() { return this.item.OpenAmount; }
    get Status() { return this.item.StatusName; }

    ViewEntityClicked() {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: this.item.Id, ObjectTableName: 'ARPayment', BackButtonLabel: "A/R Invoice: " + this.fatherComponent.EntityPM.InvoiceNumber });

                let isEditComponentSaved = false;

                cmpRef.instance.BackCompleted.subscribe(bk => {
                    if (isEditComponentSaved) {
                        this.fatherComponent.entityArgs.EditComponent.ReloadEntityPM();
                    }
                });

                cmpRef.instance.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        isEditComponentSaved = true;
                    }
                });

                cmpRef.instance.SaveAndCloseCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        isEditComponentSaved = true;
                    }
                });
            });
    }

    ConnectClicked() {
        if (this.fatherComponent.EntityPM.StatusCode == "PD" || this.fatherComponent.EntityPM.IsClosed) {
            this.fatherComponent.RunReachedBoundsMessage();
        }

        else if (this.fatherComponent.EntityPM.AmountDue <= 0) {
            var messageText = "Amount paid equals or bigger than invoice amount";

            var window: MessageWindow = new MessageWindow();
            window.Width = 400;
            window.Height = 150;
            window.Show(messageText);
        }

        else {
            var invoiceAmount: number = this.fatherComponent.EntityPM.AmountDue;
            var paymentAmount: number = this.item.OpenAmount;

            if (this.fatherComponent.EntityPM.ARInvoiceTypeCode == "CD") {
                if (invoiceAmount > 0) {
                    invoiceAmount = invoiceAmount * -1;
                }
            }

            var smallestAmount: number = invoiceAmount <= paymentAmount ? invoiceAmount : paymentAmount;
            var smallestAmountLocal: number = smallestAmount * this.item.PaymentCurrencyExchangeRate;

            var entityPM = new ARInvoicePaymentPM(this.fatherComponent.EntityPM);
            entityPM.Tenant = SessionLocator.Tenant;
            entityPM.ARInvoiceId = this.fatherComponent.EntityPM.Id;
            entityPM.ARPaymentId = this.item.Id;
            entityPM.ForeignAmount = smallestAmount;
            entityPM.PaymentAmount = smallestAmount;
            entityPM.LocalAmount = smallestAmountLocal;
            entityPM.ForeignCurrencyId = this.item.PaymentCurrencyId;
            entityPM.ExchangeRate = this.item.PaymentCurrencyExchangeRate;

            this.fatherComponent.EntityPM.AddARInvoicePaymentPM(entityPM);
            this.SaveEntity();
        }
    }
    DisconnectClicked() {
        var entityPM: ARInvoicePaymentPM = this.fatherComponent.EntityPM.InvoicePayments.filter(d => d.ARPaymentId == this.item.Id)[0];
        if (entityPM != null) {
            this.fatherComponent.EntityPM.RemoveARInvoicePaymentPM(entityPM);
            this.SaveEntity();
        }
    }
    SaveEntity() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveChanges();
        }
    }
}
