"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var core_1 = require("@angular/core");
var EntityArgs_1 = require("../../../../Infrastructure/DataContracts/EntityArgs");
var APInvoicePaymentPM_1 = require("../../../../Invoice/EntityPMs/APInvoicePaymentPM");
var Tools_1 = require("../../../../Infrastructure/Tools");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var APPaymentListService_1 = require("../../../../Invoice/Services/StandardLists/APPaymentListService");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var APInvoicePaymentsTabComponent = /** @class */ (function () {
    function APInvoicePaymentsTabComponent(entityArgs, entityResourceService) {
        var _this = this;
        this.entityArgs = entityArgs;
        this.entityResourceService = entityResourceService;
        this.EntityPM = null;
        this.ObjectTableName = "APInvoice";
        this.DataContext = this;
        this.ItemsSource1 = [];
        this.ItemsSource2 = [];
        this.ItemsSource1Hidden = false;
        this.ItemsSource2Hidden = false;
        this.IsResourcesReady = false;
        this.isRTL = false;
        this.IsEnabledDisconnect = false;
        this.IsEnabledConnect = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.SaveCompletedEvent = null;
        this.LoadCompletedEvent = null;
        this.IsEditingEnabled = false;
        this.AddPaymentButtonIsEnabled = false;
        this.noPermissionVisibility = false;
        this.IsNoPermissionVisible = false;
        this.NoColumnWidth = 50;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting)
            this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.EntityPM = entityArgs.EntityPM;
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("APPayment", "APPaymentDissconectInvoices")) {
            this.IsEnabledDisconnect = true;
            this.DisConnectFeatureTitle = "";
        }
        else {
            this.IsEnabledDisconnect = false;
            this.DisConnectFeatureTitle = "You have no permission to disconnect invoices";
        }
        if (FeatureLocator_1.FeatureLocator.HasFeaturePermession("APPayment", "APPaymentConnectInvoices")) {
            this.IsEnabledConnect = true;
            this.ConnectFeatureTitle = "";
        }
        else {
            this.IsEnabledConnect = false;
            this.ConnectFeatureTitle = "You have no permission to connect invoices";
        }
        entityResourceService.getEntityResourceByTableName("APPayment", 0).subscribe(function (response) {
            _this.IsResourcesReady = true;
            _this.Listen();
            _this.LoadInvoicePayments();
        });
    }
    APInvoicePaymentsTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent != null) {
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.LoadInvoicePayments();
                }
            });
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.LoadInvoicePayments();
                }
            });
        }
    };
    APInvoicePaymentsTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    Object.defineProperty(APInvoicePaymentsTabComponent.prototype, "CantConnectMessageVisibility", {
        get: function () {
            var result = false;
            if (this.EntityPM.AmountInInvoiceCurrency < 0) {
                result = true;
            }
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoicePaymentsTabComponent.prototype, "NoPermissionVisibility", {
        get: function () { return this.noPermissionVisibility; },
        set: function (value) { this.noPermissionVisibility = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoicePaymentsTabComponent.prototype, "InvoiceCurrencyCode", {
        get: function () { return this.EntityPM.InvoiceCurrencyCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoicePaymentsTabComponent.prototype, "InvoiceAmount", {
        get: function () { return this.EntityPM.AmountInInvoiceCurrency; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoicePaymentsTabComponent.prototype, "AmountPaid", {
        get: function () { return this.EntityPM.AmountInInvoiceCurrency - this.EntityPM.AmountDue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoicePaymentsTabComponent.prototype, "AmountDue", {
        get: function () { return this.EntityPM.AmountDue; },
        enumerable: true,
        configurable: true
    });
    APInvoicePaymentsTabComponent.prototype.LoadInvoicePayments = function () {
        var _this = this;
        var isLoading = true;
        this.ItemsSource1 = [];
        this.ItemsSource2 = [];
        if (!FeatureLocator_1.FeatureLocator.HasFeaturePermession("APPayment", "READ")) {
            isLoading = false;
            this.NoPermissionVisibility = true;
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id) || Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "WA" || this.EntityPM.StatusCode == "VD") {
            isLoading = false;
        }
        else {
            if (isLoading) {
                this.CurrentSession.StartBusyIndicatorLoading();
                if (this.myService == null) {
                    this.myService = new APPaymentListService_1.APPaymentListService();
                }
                var filters = new ApiQueryFilters_1.ApiQueryFilters();
                filters.PageIndex = 0;
                filters.PageSize = 200;
                filters.Filter1Name = "VendorId";
                filters.Filter1Value = this.EntityPM.VendorId;
                filters.Filter1Operator = "Equals";
                filters.Filter2Name = "StatusCode";
                filters.Filter2Value = "DR,AD,CL,PR";
                filters.Filter2Operator = "InList";
                this.myService.getByFilters(filters).subscribe(function (myResponse) {
                    if (!myResponse.HasError) {
                        var list = myResponse.Result;
                        var connectedList = [];
                        var unConnectedMatchedList = [];
                        var unConnectedListNotMatched = [];
                        list = list.sort(function (a, b) { return a.PaymentNo.toLowerCase() == b.PaymentNo.toLowerCase() ? 0 : a.PaymentNo.toLowerCase() < b.PaymentNo.toLowerCase() ? -1 : 1; });
                        list.forEach(function (item) {
                            if (_this.EntityPM.StatusCode == "PD") {
                                if (_this.EntityPM.InvoicePayments.filter(function (f) { return f.APPaymentId == item.Id; }).length > 0) {
                                    connectedList.push(item);
                                }
                            }
                            else {
                                if (_this.EntityPM.InvoicePayments.filter(function (f) { return f.APPaymentId == item.Id; }).length > 0) {
                                    connectedList.push(item);
                                }
                                else {
                                    if (!item.IsClosed) {
                                        if (item.PaymentCurrencyId != _this.EntityPM.InvoiceCurrencyId || item.OpenAmount <= 0) {
                                            unConnectedListNotMatched.push(item);
                                        }
                                        else {
                                            unConnectedMatchedList.push(item);
                                        }
                                    }
                                }
                            }
                        });
                        connectedList.forEach(function (item) {
                            _this.ItemsSource1.push(new APInvoicePaymentItem(item, _this));
                        });
                        if (!_this.EntityPM.IsClosed) {
                            unConnectedMatchedList.forEach(function (item) {
                                _this.ItemsSource2.push(new APInvoicePaymentItem(item, _this));
                            });
                            unConnectedListNotMatched.forEach(function (item) {
                                _this.ItemsSource2.push(new APInvoicePaymentItem(item, _this));
                            });
                        }
                        _this.SetGridColumnsWidth();
                    }
                    _this.CurrentSession.StopBusyIndicator();
                });
            }
        }
    };
    APInvoicePaymentsTabComponent.prototype.RunReachedBoundsMessage = function () {
        var messageText = TextCodeTranslator_1.TextCodeTranslator.Translate("APInvoice.M.AmountPaidBiggerThanInvoiceAmount");
        var window = new MessageWindow_1.MessageWindow();
        window.Width = 400;
        window.Show(messageText);
    };
    APInvoicePaymentsTabComponent.prototype.SetGridColumnsWidth = function () {
        var noColumnWidth = 50;
        this.ItemsSource1.forEach(function (item) {
            if (!Tools_1.AppTool.IsNullOrEmpty(item.PaymentNo)) {
                var widthOfLabel = Tools_1.AppTool.GetTextWidth(item.PaymentNo) + 10;
                if (widthOfLabel > noColumnWidth) {
                    noColumnWidth = widthOfLabel;
                }
            }
        });
        if (noColumnWidth > 100) {
            noColumnWidth = 100;
        }
        this.NoColumnWidth = noColumnWidth;
    };
    APInvoicePaymentsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './APInvoicePaymentsTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], APInvoicePaymentsTabComponent);
    return APInvoicePaymentsTabComponent;
}());
exports.APInvoicePaymentsTabComponent = APInvoicePaymentsTabComponent;
var APInvoicePaymentItem = /** @class */ (function () {
    function APInvoicePaymentItem(item, fatherComponent) {
        this.item = item;
        this.fatherComponent = fatherComponent;
        this.IsConnected = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsNotMatchedVisibile = false;
        this.IsConnectButtonVisibile = false;
        this.DisconnectButtonIsEnabled = false;
        this.ForeignCurrencyCodeBackground = "transparent";
        this.ForeignCurrencyCodeForeground = "#282E30";
        this.OpenAmountBackground = "transparent";
        this.OpenAmountForeground = "#282E30";
        this.SetUIProperties();
    }
    APInvoicePaymentItem.prototype.SetUIProperties = function () {
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
    };
    Object.defineProperty(APInvoicePaymentItem.prototype, "ConnectButtonIsEnabled", {
        //public ConnectButtonIsEnabled: boolean = false;
        get: function () {
            var result = true;
            if (this.fatherComponent.EntityPM.AmountInInvoiceCurrency < 0) {
                result = false;
            }
            this.fatherComponent.IsEnabledDisconnect ? result = true : result = false;
            return result;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoicePaymentItem.prototype, "PaymentNo", {
        get: function () { return this.item.PaymentNo; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoicePaymentItem.prototype, "ForeignCurrencyCode", {
        get: function () { return this.item.PaymentCurrencyCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoicePaymentItem.prototype, "OpenAmount", {
        get: function () { return this.item.OpenAmount; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoicePaymentItem.prototype, "Status", {
        get: function () { return this.item.StatusName; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(APInvoicePaymentItem.prototype, "ForeignAmount", {
        get: function () { return this.item.AmountInPaymentCurrency; },
        enumerable: true,
        configurable: true
    });
    APInvoicePaymentItem.prototype.ViewEntityClicked = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: _this.item.Id, ObjectTableName: 'APPayment', BackButtonLabel: "A/P Invoice: " + _this.fatherComponent.EntityPM.InvoiceNumber });
            var isEditComponentSaved = false;
            cmpRef.instance.BackCompleted.subscribe(function (bk) {
                if (isEditComponentSaved) {
                    _this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                }
            });
            cmpRef.instance.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    isEditComponentSaved = true;
                }
            });
            cmpRef.instance.SaveAndCloseCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    isEditComponentSaved = true;
                }
            });
        });
    };
    APInvoicePaymentItem.prototype.ConnectClicked = function () {
        if (this.fatherComponent.EntityPM.StatusCode == "PD" || this.fatherComponent.EntityPM.IsClosed) {
            this.fatherComponent.RunReachedBoundsMessage();
        }
        else {
            var invoiceAmount = this.fatherComponent.EntityPM.AmountDue;
            var paymentAmount = this.item.OpenAmount;
            var smallestAmount = invoiceAmount <= paymentAmount ? invoiceAmount : paymentAmount;
            var smallestAmountLocal = smallestAmount * this.item.PaymentCurrencyExchangeRate;
            var entityPM = new APInvoicePaymentPM_1.APInvoicePaymentPM(this.fatherComponent.EntityPM);
            entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
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
    };
    APInvoicePaymentItem.prototype.DisconnectClicked = function () {
        var _this = this;
        var entityPM = this.fatherComponent.EntityPM.InvoicePayments.filter(function (d) { return d.APPaymentId == _this.item.Id; })[0];
        if (entityPM != null) {
            this.fatherComponent.EntityPM.RemoveAPInvoicePaymentPM(entityPM);
            this.SaveEntity();
        }
    };
    APInvoicePaymentItem.prototype.SaveEntity = function () {
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveChanges();
        }
    };
    return APInvoicePaymentItem;
}());
exports.APInvoicePaymentItem = APInvoicePaymentItem;
//# sourceMappingURL=APInvoicePaymentsTabComponent.js.map