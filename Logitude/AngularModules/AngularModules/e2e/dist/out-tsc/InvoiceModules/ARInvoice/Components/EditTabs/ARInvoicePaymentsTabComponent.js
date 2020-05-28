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
var ARInvoicePaymentPM_1 = require("../../../../Invoice/EntityPMs/ARInvoicePaymentPM");
var Tools_1 = require("../../../../Infrastructure/Tools");
var FeatureLocator_1 = require("../../../../Infrastructure/Utilities/FeatureLocator");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var ARPaymentListService_1 = require("../../../../Invoice/Services/StandardLists/ARPaymentListService");
var ApiQueryFilters_1 = require("../../../../Infrastructure/DataContracts/ApiQueryFilters");
var LogitudeWindow_1 = require("../../../../Controls/Windows/LogitudeWindow");
var TextCodeTranslator_1 = require("../../../../Infrastructure/Utilities/TextCodeTranslator");
var MessageWindow_1 = require("../../../../Controls/Windows/MessageWindow");
var EntityResourceService_1 = require("../../../../Infrastructure/Services/EntityResourceService");
var ObjectsLocator_1 = require("../../../../Infrastructure/Locators/ObjectsLocator");
var ARInvoicePaymentsTabComponent = /** @class */ (function () {
    function ARInvoicePaymentsTabComponent(entityArgs, entityResourceService) {
        var _this = this;
        this.entityArgs = entityArgs;
        this.entityResourceService = entityResourceService;
        this.EntityPM = null;
        this.ObjectTableName = "ARInvoice";
        this.DataContext = this;
        this.ItemsSource1 = [];
        this.ItemsSource2 = [];
        this.ItemsSource1Hidden = false;
        this.ItemsSource2Hidden = false;
        this.IsResourcesReady = false;
        this.isRTL = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.SessionEvent = null;
        this.SaveCompletedEvent = null;
        this.LoadCompletedEvent = null;
        this.IsEditingEnabled = false;
        this.AddPaymentButtonIsEnabled = false;
        this.IsNoPermissionVisible = false;
        if (ObjectsLocator_1.ObjectsLocator.GlobalSetting) {
            this.isRTL = (ObjectsLocator_1.ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        }
        this.EntityPM = entityArgs.EntityPM;
        entityResourceService.getEntityResourceByTableName("ARPayment", 0).subscribe(function (response) {
            _this.IsResourcesReady = true;
            _this.SetUIProperties();
            _this.Listen();
            _this.LoadInvoicePayments();
        });
    }
    ARInvoicePaymentsTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.entityArgs.EditComponent != null) {
            this.SessionEvent = this.CurrentSession.SessionEvent.subscribe(function (s) {
                if (s == "NewARPaymentInvoiceTabCreated") {
                    _this.entityArgs.EditComponent.ReloadEntityPM();
                }
            });
            this.SaveCompletedEvent = this.entityArgs.EditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.SetUIProperties();
                    _this.LoadInvoicePayments();
                }
            });
            this.LoadCompletedEvent = this.entityArgs.EditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.entityArgs.EditComponent.EntityPM;
                    _this.SetUIProperties();
                    _this.LoadInvoicePayments();
                }
            });
        }
    };
    ARInvoicePaymentsTabComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.SessionEvent);
        Tools_1.AppTool.KillEventEmitter(this.SaveCompletedEvent);
        Tools_1.AppTool.KillEventEmitter(this.LoadCompletedEvent);
    };
    ARInvoicePaymentsTabComponent.prototype.SetUIProperties = function () {
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
        else if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "DR") {
            isAddButtonEnabled = false;
        }
        this.IsEditingEnabled = isEditingEnabled;
        this.AddPaymentButtonIsEnabled = isAddButtonEnabled;
    };
    Object.defineProperty(ARInvoicePaymentsTabComponent.prototype, "IsConstituentInvoice", {
        get: function () { return this.EntityPM.IsConstituentInvoice; },
        set: function (newValue) {
            if (this.EntityPM.IsConstituentInvoice != newValue) {
                this.EntityPM.IsConstituentInvoice = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoicePaymentsTabComponent.prototype, "ARInvoiceTypeCode", {
        get: function () { return this.EntityPM.ARInvoiceTypeCode; },
        set: function (newValue) {
            if (this.EntityPM.ARInvoiceTypeCode != newValue) {
                this.EntityPM.ARInvoiceTypeCode = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoicePaymentsTabComponent.prototype, "InvoiceCurrencyCode", {
        get: function () { return this.EntityPM.InvoiceCurrencyCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoicePaymentsTabComponent.prototype, "InvoiceAmount", {
        get: function () { return this.EntityPM.AmountInInvoiceCurrency; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoicePaymentsTabComponent.prototype, "AmountPaid", {
        get: function () { return this.EntityPM.AmountInInvoiceCurrency - this.EntityPM.AmountDue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoicePaymentsTabComponent.prototype, "AmountDue", {
        get: function () { return this.EntityPM.AmountDue; },
        enumerable: true,
        configurable: true
    });
    ARInvoicePaymentsTabComponent.prototype.LoadInvoicePayments = function () {
        var _this = this;
        var isLoading = true;
        this.ItemsSource1 = [];
        this.ItemsSource2 = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.Id) || Tools_1.AppTool.IsNullOrEmpty(this.EntityPM.StatusCode) || this.EntityPM.StatusCode == "DR" || this.EntityPM.StatusCode == "VD") {
            isLoading = false;
        }
        if (!FeatureLocator_1.FeatureLocator.HasFeaturePermession("ARPayment", "READ")) {
            isLoading = false;
            this.IsNoPermissionVisible = true;
        }
        if (isLoading) {
            this.CurrentSession.StartBusyIndicatorLoading();
            if (this.myService == null) {
                this.myService = new ARPaymentListService_1.ARPaymentListService();
            }
            var filters = new ApiQueryFilters_1.ApiQueryFilters();
            filters.PageIndex = 0;
            filters.PageSize = 200;
            filters.Filter1Name = "BillToId";
            filters.Filter1Value = this.EntityPM.BillToId;
            filters.Filter1Operator = "Equals";
            filters.Filter2Name = "StatusCode";
            filters.Filter2Value = "DR,AD,PP,PD,CL";
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
                            if (_this.EntityPM.InvoicePayments.filter(function (f) { return f.ARPaymentId == item.Id; }).length > 0) {
                                connectedList.push(item);
                            }
                        }
                        else {
                            if (_this.EntityPM.InvoicePayments.filter(function (f) { return f.ARPaymentId == item.Id; }).length > 0) {
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
                        _this.ItemsSource1.push(new ARInvoicePaymentItem(item, _this));
                    });
                    if (!_this.EntityPM.IsClosed) {
                        unConnectedMatchedList.forEach(function (item) {
                            _this.ItemsSource2.push(new ARInvoicePaymentItem(item, _this));
                        });
                        unConnectedListNotMatched.forEach(function (item) {
                            _this.ItemsSource2.push(new ARInvoicePaymentItem(item, _this));
                        });
                    }
                }
                _this.CurrentSession.StopBusyIndicator();
            });
        }
    };
    ARInvoicePaymentsTabComponent.prototype.RunReachedBoundsMessage = function () {
        var messageText = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.M.AmountPaidBiggerThanInvoiceAmount");
        var window = new MessageWindow_1.MessageWindow();
        window.Width = 400;
        window.Show(messageText);
    };
    // Add Payment Button 
    ARInvoicePaymentsTabComponent.prototype.AddPaymentClicked = function () {
        var message = "";
        if (!FeatureLocator_1.FeatureLocator.HasEntityPermessions("ARPayment", "NEW", true)) {
            return;
        }
        var totalAmount = 0;
        this.EntityPM.InvoicePayments.forEach(function (item) {
            totalAmount += item.LocalAmount;
        });
        if (this.EntityPM.StatusCode == "DR") {
            var messageText = TextCodeTranslator_1.TextCodeTranslator.Translate("ARInvoice.M.CantAddPaymentForDraftInvoice");
            var window = new MessageWindow_1.MessageWindow();
            window.Width = 300;
            window.Show(messageText);
        }
        else if (this.EntityPM.AmountDue <= 0) {
            this.RunReachedBoundsMessage();
        }
        else {
            var str = TextCodeTranslator_1.TextCodeTranslator.Translate("General.O.NewEntity");
            str = str.replace("%Entity", TextCodeTranslator_1.TextCodeTranslator.TranslateTable("ARPayment"));
            var logWindow = new LogitudeWindow_1.LogitudeWindow();
            logWindow.WindowArgs = { ARInvoice: this.EntityPM };
            logWindow.Title = str;
            logWindow.Show("./InvoiceModules/ARPayment/Components/NewEntity/NewARPaymentComponent");
        }
    };
    ARInvoicePaymentsTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './ARInvoicePaymentsTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], ARInvoicePaymentsTabComponent);
    return ARInvoicePaymentsTabComponent;
}());
exports.ARInvoicePaymentsTabComponent = ARInvoicePaymentsTabComponent;
var ARInvoicePaymentItem = /** @class */ (function () {
    function ARInvoicePaymentItem(item, fatherComponent) {
        var _this = this;
        this.item = item;
        this.fatherComponent = fatherComponent;
        this.IsConnected = false;
        this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        this.IsNotMatchedVisibile = false;
        this.IsConnectButtonVisibile = false;
        this.ConnectButtonIsEnabled = false;
        this.DisconnectButtonIsEnabled = false;
        this.ForeignCurrencyCodeBackground = "transparent";
        this.ForeignCurrencyCodeForeground = "#282E30";
        this.OpenAmountBackground = "transparent";
        this.OpenAmountForeground = "#282E30";
        if (this.fatherComponent.EntityPM.InvoicePayments.filter(function (f) { return f.ARPaymentId == _this.item.Id; }).length > 0) {
            this.IsConnected = true;
        }
        this.SetUIProperties();
    }
    ARInvoicePaymentItem.prototype.SetUIProperties = function () {
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
    };
    Object.defineProperty(ARInvoicePaymentItem.prototype, "PaymentNo", {
        get: function () { return this.item.PaymentNo; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoicePaymentItem.prototype, "ForeignAmount", {
        get: function () { return this.item.AmountInPaymentCurrency; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoicePaymentItem.prototype, "ForeignCurrencyCode", {
        get: function () { return this.item.PaymentCurrencyCode; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoicePaymentItem.prototype, "OpenAmount", {
        get: function () { return this.item.OpenAmount; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ARInvoicePaymentItem.prototype, "Status", {
        get: function () { return this.item.StatusName; },
        enumerable: true,
        configurable: true
    });
    ARInvoicePaymentItem.prototype.ViewEntityClicked = function () {
        var _this = this;
        SessionLocator_1.SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(function (cmpRef) {
            cmpRef.instance.ComponentRef = cmpRef;
            cmpRef.instance.Run({ EntityId: _this.item.Id, ObjectTableName: 'ARPayment', BackButtonLabel: "A/R Invoice: " + _this.fatherComponent.EntityPM.InvoiceNumber });
            var isEditComponentSaved = false;
            cmpRef.instance.BackCompleted.subscribe(function (bk) {
                if (isEditComponentSaved) {
                    _this.fatherComponent.entityArgs.EditComponent.ReloadEntityPM();
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
    ARInvoicePaymentItem.prototype.ConnectClicked = function () {
        if (this.fatherComponent.EntityPM.StatusCode == "PD" || this.fatherComponent.EntityPM.IsClosed) {
            this.fatherComponent.RunReachedBoundsMessage();
        }
        else if (this.fatherComponent.EntityPM.AmountDue <= 0) {
            var messageText = "Amount paid equals or bigger than invoice amount";
            var window = new MessageWindow_1.MessageWindow();
            window.Width = 400;
            window.Height = 150;
            window.Show(messageText);
        }
        else {
            var invoiceAmount = this.fatherComponent.EntityPM.AmountDue;
            var paymentAmount = this.item.OpenAmount;
            if (this.fatherComponent.EntityPM.ARInvoiceTypeCode == "CD") {
                if (invoiceAmount > 0) {
                    invoiceAmount = invoiceAmount * -1;
                }
            }
            var smallestAmount = invoiceAmount <= paymentAmount ? invoiceAmount : paymentAmount;
            var smallestAmountLocal = smallestAmount * this.item.PaymentCurrencyExchangeRate;
            var entityPM = new ARInvoicePaymentPM_1.ARInvoicePaymentPM(this.fatherComponent.EntityPM);
            entityPM.Tenant = SessionLocator_1.SessionLocator.Tenant;
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
    };
    ARInvoicePaymentItem.prototype.DisconnectClicked = function () {
        var _this = this;
        var entityPM = this.fatherComponent.EntityPM.InvoicePayments.filter(function (d) { return d.ARPaymentId == _this.item.Id; })[0];
        if (entityPM != null) {
            this.fatherComponent.EntityPM.RemoveARInvoicePaymentPM(entityPM);
            this.SaveEntity();
        }
    };
    ARInvoicePaymentItem.prototype.SaveEntity = function () {
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.CurrentSession.CurrentEditComponent.SaveChanges();
        }
    };
    return ARInvoicePaymentItem;
}());
exports.ARInvoicePaymentItem = ARInvoicePaymentItem;
//# sourceMappingURL=ARInvoicePaymentsTabComponent.js.map