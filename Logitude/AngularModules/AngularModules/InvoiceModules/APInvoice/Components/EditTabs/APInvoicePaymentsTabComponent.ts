import {Component, OnDestroy}  from '@angular/core';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {APInvoicePM} from '../../../../Invoice/EntityPMs/APInvoicePM';
import {APInvoicePaymentPM} from '../../../../Invoice/EntityPMs/APInvoicePaymentPM';
import {DateTool, AppTool} from '../../../../Infrastructure/Tools';
import {FeatureLocator} from '../../../../Infrastructure/Utilities/FeatureLocator';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {APPaymentList} from '../../../../Invoice/EntityLists/APPaymentList';
import {APPaymentListService} from '../../../../Invoice/Services/StandardLists/APPaymentListService';
import {ApiQueryFilters} from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {ConfirmWindow} from '../../../../Controls/Windows/ConfirmWindow';
import {MessageWindow} from '../../../../Controls/Windows/MessageWindow';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';

@Component({
    moduleId: module.id,
    templateUrl: './APInvoicePaymentsTabComponent.html',
})

export class APInvoicePaymentsTabComponent implements OnDestroy {
    public EntityPM: APInvoicePM = null;
    public ObjectTableName = "APInvoice";
    public DataContext = this;
    public ItemsSource1: APInvoicePaymentItem[] = [];
    public ItemsSource2: APInvoicePaymentItem[] = [];
    public ItemsSource1Hidden: boolean = false;
    public ItemsSource2Hidden: boolean = false;
    public IsResourcesReady: boolean = false;
    public isRTL: boolean = false;
    public IsEnabledDisconnect: boolean = false;
    public IsEnabledConnect: boolean = false;
    public ConnectFeatureTitle: string;
    public DisConnectFeatureTitle: string;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private entityArgs: EntityArgs, private entityResourceService: EntityResourceService) {
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");       

        this.EntityPM = entityArgs.EntityPM;

        if (FeatureLocator.HasFeaturePermession("APPayment", "APPaymentDissconectInvoices")) {
            this.IsEnabledDisconnect = true;
            this.DisConnectFeatureTitle = "";
        }
        else {
            this.IsEnabledDisconnect = false;
            this.DisConnectFeatureTitle = "You have no permission to disconnect invoices";
        }

        if (FeatureLocator.HasFeaturePermession("APPayment", "APPaymentConnectInvoices")) {
            this.IsEnabledConnect = true;
            this.ConnectFeatureTitle = "";
        }
        else {
            this.IsEnabledConnect = false;
            this.ConnectFeatureTitle = "You have no permission to connect invoices";
        }

        entityResourceService.getEntityResourceByTableName("APPayment", 0).subscribe(response => {
            this.IsResourcesReady = true;
            this.Listen();
            this.LoadInvoicePayments();
        });
    }

    private SaveCompletedEvent: any = null;
    private LoadCompletedEvent: any = null;
    private Listen() {
        if (this.entityArgs.EditComponent != null) {

            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                if (isSaveSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.LoadInvoicePayments();
                }
            });

            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                if (isLoadSuccess) {
                    this.EntityPM = this.entityArgs.EditComponent.EntityPM;
                    this.LoadInvoicePayments();
                }
            });
        }
    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.SaveCompletedEvent);
        AppTool.KillEventEmitter(this.LoadCompletedEvent);
    }


    public IsEditingEnabled: boolean = false;
    public AddPaymentButtonIsEnabled: boolean = false;
    
    get CantConnectMessageVisibility() {
        var result = false;
        if (this.EntityPM.AmountInInvoiceCurrency < 0) {
            result = true;
        }
        return result;
    }

    private noPermissionVisibility = false;
    get NoPermissionVisibility() { return this.noPermissionVisibility; }
    set NoPermissionVisibility(value: boolean) { this.noPermissionVisibility = value;}

    get InvoiceCurrencyCode() { return this.EntityPM.InvoiceCurrencyCode; }
    get InvoiceAmount() { return this.EntityPM.AmountInInvoiceCurrency; }
    get AmountPaid() { return this.EntityPM.AmountInInvoiceCurrency - this.EntityPM.AmountDue; }
    get AmountDue() { return this.EntityPM.AmountDue; }

    private myService: APPaymentListService;
    public IsNoPermissionVisible: boolean = false;
    LoadInvoicePayments() {
        var isLoading = true;
        this.ItemsSource1 = [];
        this.ItemsSource2 = [];

        if (!FeatureLocator.HasFeaturePermession("APPayment", "READ")) {
            isLoading = false;
            this.NoPermissionVisibility = true;
        }

        if (AppTool.IsNullOrEmpty(this.EntityPM.Id) || AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "WA" || this.EntityPM.StatusCode == "VD") {
            isLoading = false;
        }

        else {
            if (isLoading) {

                this.CurrentSession.StartBusyIndicatorLoading();
                if (this.myService == null) {
                    this.myService = new APPaymentListService();
                }

                var filters = new ApiQueryFilters();
                filters.PageIndex = 0;
                filters.PageSize = 200;

                filters.Filter1Name = "VendorId";
                filters.Filter1Value = this.EntityPM.VendorId;
                filters.Filter1Operator = "Equals";

                filters.Filter2Name = "StatusCode";
                filters.Filter2Value = "DR,AD,CL,PR";
                filters.Filter2Operator = "InList";

                this.myService.getByFilters(filters).subscribe((myResponse: ServiceResponse) => {
                    if (!myResponse.HasError) {

                        var list: APPaymentList[] = myResponse.Result;
                        var connectedList: APPaymentList[] = [];
                        var unConnectedMatchedList: APPaymentList[] = [];
                        var unConnectedListNotMatched: APPaymentList[] = [];

                        list = list.sort(function (a, b) { return a.PaymentNo.toLowerCase() == b.PaymentNo.toLowerCase() ? 0 : a.PaymentNo.toLowerCase() < b.PaymentNo.toLowerCase() ? -1 : 1; });

                        list.forEach(item => {

                            if (this.EntityPM.StatusCode == "PD") {
                                if (this.EntityPM.InvoicePayments.filter(f => f.APPaymentId == item.Id).length > 0) {
                                    connectedList.push(item);
                                }
                            }

                            else {
                                if (this.EntityPM.InvoicePayments.filter(f => f.APPaymentId == item.Id).length > 0) {
                                    connectedList.push(item);
                                }

                                else {
                                    if (!item.IsClosed) {
                                        if (item.PaymentCurrencyId != this.EntityPM.InvoiceCurrencyId || item.OpenAmount <= 0) {
                                            unConnectedListNotMatched.push(item);
                                        }

                                        else {
                                            unConnectedMatchedList.push(item);
                                        }
                                    }
                                }
                            }
                        });

                        connectedList.forEach(item => {
                            this.ItemsSource1.push(new APInvoicePaymentItem(item, this));
                        });

                        if (!this.EntityPM.IsClosed) {
                            unConnectedMatchedList.forEach(item => {
                                this.ItemsSource2.push(new APInvoicePaymentItem(item, this));
                            });

                            unConnectedListNotMatched.forEach(item => {
                                this.ItemsSource2.push(new APInvoicePaymentItem(item, this));
                            });
                        }

                        this.SetGridColumnsWidth();
                    }

                    this.CurrentSession.StopBusyIndicator();
                });
            }
        }
    }

    RunReachedBoundsMessage() {
        var messageText = TextCodeTranslator.Translate("APInvoice.M.AmountPaidBiggerThanInvoiceAmount");
        var window = new MessageWindow();
        window.Width = 400;
        window.Show(messageText);
    }

    public NoColumnWidth: number = 50;
    SetGridColumnsWidth() {
        var noColumnWidth = 50;
        this.ItemsSource1.forEach(item => {
            if (!AppTool.IsNullOrEmpty(item.PaymentNo)) {
                var widthOfLabel = AppTool.GetTextWidth(item.PaymentNo) + 10;

                if (widthOfLabel > noColumnWidth) {
                    noColumnWidth = widthOfLabel;
                }
            }
        });

        if (noColumnWidth > 100) {
            noColumnWidth = 100;
        }
        this.NoColumnWidth = noColumnWidth;
    }
}

export class APInvoicePaymentItem {
    public IsConnected: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private item: APPaymentList, private fatherComponent: APInvoicePaymentsTabComponent) {
        this.SetUIProperties();
    }

    SetUIProperties() {
        //if (this.fatherComponent.EntityPM.AmountInInvoiceCurrency < 0) {
        //    this.ConnectButtonIsEnabled = false;
        //}
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

            if (!this.IsConnected) {
                if (this.item.PaymentCurrencyId != this.fatherComponent.EntityPM.InvoiceCurrencyId || this.OpenAmount <= 0) {
                    this.IsNotMatchedVisibile = true;
                }

                if (this.item.PaymentCurrencyId == this.fatherComponent.EntityPM.InvoiceCurrencyId && this.OpenAmount > 0) {
                    this.IsConnectButtonVisibile = true;
                }
            }
        }
    }

    public IsNotMatchedVisibile: boolean = false;
    public IsConnectButtonVisibile: boolean = false;
    //public ConnectButtonIsEnabled: boolean = false;

    get ConnectButtonIsEnabled() {
        var result = true;
        if (this.fatherComponent.EntityPM.AmountInInvoiceCurrency  < 0) {
            result = false;
        }
        this.fatherComponent.IsEnabledDisconnect ? result = true : result = false;
        return result;
    }

    public DisconnectButtonIsEnabled: boolean = false;
    public ForeignCurrencyCodeBackground: string = "transparent";
    public ForeignCurrencyCodeForeground: string = "#282E30";
    public OpenAmountBackground: string = "transparent";
    public OpenAmountForeground: string = "#282E30";

    get PaymentNo() { return this.item.PaymentNo; }
    get ForeignCurrencyCode() { return this.item.PaymentCurrencyCode; }
    get OpenAmount() { return this.item.OpenAmount; }
    get Status() { return this.item.StatusName; }
    get ForeignAmount() { return this.item.AmountInPaymentCurrency; }

    ViewEntityClicked() {
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({ EntityId: this.item.Id, ObjectTableName: 'APPayment', BackButtonLabel: "A/P Invoice: " + this.fatherComponent.EntityPM.InvoiceNumber });

                let isEditComponentSaved = false;
                cmpRef.instance.BackCompleted.subscribe(bk => {
                    if (isEditComponentSaved) {
                        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
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

        else {
            var invoiceAmount = this.fatherComponent.EntityPM.AmountDue;
            var paymentAmount = this.item.OpenAmount;

            var smallestAmount: number = invoiceAmount <= paymentAmount ? invoiceAmount : paymentAmount;
            var smallestAmountLocal: number = smallestAmount * this.item.PaymentCurrencyExchangeRate;

            var entityPM: APInvoicePaymentPM = new APInvoicePaymentPM(this.fatherComponent.EntityPM);

            entityPM.Tenant = SessionLocator.Tenant;
            entityPM.APInvoiceId = this.fatherComponent.EntityPM.Id;
            entityPM.APPaymentId = this.item.Id;
            entityPM.ForeignAmount = smallestAmount;
            entityPM.PaymentAmount = smallestAmount;
            entityPM.LocalAmount = smallestAmountLocal;
            entityPM.ForeignCurrencyId = this.item.PaymentCurrencyId;
            entityPM.ExchangeRate = this.item.PaymentCurrencyExchangeRate;

            this.fatherComponent.EntityPM.AddAPInvoicePaymentPM(entityPM);
            this.SaveEntity();
        }
    }

    DisconnectClicked() {
        var entityPM: APInvoicePaymentPM = this.fatherComponent.EntityPM.InvoicePayments.filter(d => d.APPaymentId == this.item.Id)[0];
        if (entityPM != null) {
            this.fatherComponent.EntityPM.RemoveAPInvoicePaymentPM(entityPM);
            this.SaveEntity();
        }
    }

    SaveEntity() {
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveChanges();
        }
    }
}
