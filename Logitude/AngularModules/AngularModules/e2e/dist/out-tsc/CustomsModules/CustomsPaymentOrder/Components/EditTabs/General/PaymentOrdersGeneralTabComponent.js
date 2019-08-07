"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
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
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var Tools_1 = require("../../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../../Infrastructure/Utilities/SessionLocator");
var TextCodeTranslator_1 = require("../../../../../Infrastructure/Utilities/TextCodeTranslator");
var PaymentOrderPM_1 = require("../../../../../Customs/EntityPMs/PaymentOrderPM");
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ObservableCollection_1 = require("../../../../../Infrastructure/Utilities/ObservableCollection");
var PaymentOrderConnectionTableExtendedPMService_1 = require("../../../../../Customs/Services/ExtendedPMs/PaymentOrderConnectionTableExtendedPMService");
var CustomsSettingListService_1 = require("../../../../../Customs/Services/StandardLists/CustomsSettingListService");
var PaymentOrderMethodPM_1 = require("../../../../../Customs/EntityPMs/PaymentOrderMethodPM");
var PaymentOrderProtestReasonPM_1 = require("../../../../../Customs/EntityPMs/PaymentOrderProtestReasonPM");
var MessageWindow_1 = require("../../../../../Controls/Windows/MessageWindow");
var LogitudeWindow_1 = require("../../../../../Controls/Windows/LogitudeWindow");
var DeclarationWebService_1 = require("../../../../../Customs/Services/WebServices/DeclarationWebService");
var DeclarationExtendedListService_1 = require("../../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService");
var CustomBankListService_1 = require("../../../../../Customs/Services/StandardLists/CustomBankListService");
var CustomBankCardExtendedPMService_1 = require("../../../../../Customs/Services/ExtendedPMs/CustomBankCardExtendedPMService");
//import { EditPaymentOrderComponent } from '../../../Declaration/EditTabs/PaymentOrder/EditPaymentOrderComponent';
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var PaymentOrdersGeneralTabComponent = /** @class */ (function (_super) {
    __extends(PaymentOrdersGeneralTabComponent, _super);
    function PaymentOrdersGeneralTabComponent(entityArgs, EntityResourceService) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.EntityResourceService = EntityResourceService;
        _this.DataContext = _this;
        _this.EntityPM = new PaymentOrderPM_1.PaymentOrderPM();
        _this.ObjectTableName = "Customs.PaymentOrder";
        _this.AccountingSelectionFilterList = [];
        _this.banksList = [];
        _this.isControlEnabled = true;
        _this.isErrorMessageVisibility = false;
        _this.isByAccountingCustomFile = true;
        _this.isByPaymentOrderAccCard = false;
        _this.showMethodsColumnFooters = false;
        _this.showProtestsColumnFooters = false;
        _this.isFromBuildAccounting = false;
        _this.isTranslationLoaded = false;
        //private paymentOrderAccCard: string;
        _this.customFilesList = [];
        _this.AmountInDisputeTotal = 0;
        _this.declarationWebService = new DeclarationWebService_1.DeclarationWebService;
        _this.declarationExtendedListService = new DeclarationExtendedListService_1.DeclarationExtendedListService();
        _this.paymentOrderConnectionTableExtendedPMService = new PaymentOrderConnectionTableExtendedPMService_1.PaymentOrderConnectionTableExtendedPMService;
        _this.customsSettingListService = new CustomsSettingListService_1.CustomsSettingListService();
        _this.imgNgStyle = "";
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        //#endregion
        //#region PaymentOrderMethods
        _this.FooterMethods = 0;
        _this.methodItemsAmountTotal = 0;
        //#endregion
        //#region PaymentOrderProtestReason
        _this.FooterProtests = 0;
        _this.LinesList = new ObservableCollection_1.ObservableCollection([]);
        _this.MethodsList = new ObservableCollection_1.ObservableCollection([]);
        _this.ProtestsList = new ObservableCollection_1.ObservableCollection([]);
        _this.banksList = [];
        _this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        _this.CurrentSession.StartBusyIndicator("");
        _this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrder").subscribe(function (response) {
            _this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrderLine").subscribe(function (response) {
                _this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrderMethod").subscribe(function (response) {
                    _this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrderProtestReason").subscribe(function (response) {
                        _this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsSetting").subscribe(function (response) {
                            _this.EntityResourceService.getEntityResourceByTableName("Customs.DeclarationPaymentMethod").subscribe(function (response) {
                                if (_this.entityArgs.EntityPM != null) {
                                    _this.EntityPM = _this.entityArgs.EntityPM;
                                    _this.ObjectTableName = _this.entityArgs.ObjectTableName;
                                    _this.InitPaymentOrderScreen();
                                }
                                _this.Listen();
                                _this.isTranslationLoaded = true;
                                _this.CurrentSession.StopBusyIndicator();
                            });
                        });
                    });
                });
            });
        });
        return _this;
    }
    PaymentOrdersGeneralTabComponent.prototype.Listen = function () {
        var _this = this;
        if (this.CurrentSession.CurrentEditComponent != null) {
            this.currentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe(function (isSaveSuccess) {
                if (isSaveSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.EntityPM.CustomerChanged = false;
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe(function (isLoadSuccess) {
                if (isLoadSuccess) {
                    _this.EntityPM = _this.CurrentSession.CurrentEditComponent.EntityPM;
                    _this.DisplayOnlyCheck();
                    _this.BuildPaymentOrderMethods();
                }
            }));
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(this.CurrentSession.CurrentEditComponent.TabSelected.subscribe(function (tabCode) {
                if (_this.currentEditComponentId == _this.CurrentSession.CurrentEditComponent.ComponentId) {
                    if (tabCode == "POGN") {
                    }
                }
            }));
        }
    };
    PaymentOrdersGeneralTabComponent.prototype.InitPaymentOrderScreen = function () {
        var _this = this;
        this.UIProperties.SetEnabled("PaymentOrderTypeCode", "Customs.PaymentOrder", false);
        this.UIProperties.SetEnabled("CustomerActivityTypeCode", "Customs.PaymentOrder", false);
        this.UIProperties.SetEnabled("ImporterId", "Customs.PaymentOrder", false);
        this.UIProperties.SetEnabled("TotalSumToPay", "Customs.PaymentOrder", false);
        this.UIProperties.SetEnabled("PaymentProcessCode", "Customs.PaymentOrder", false);
        this.UIProperties.SetEnabled("PaymentOrderLeftAmount", "Customs.PaymentOrder", false);
        if (this.EntityPM.PaymentOrderLines) {
            this.EntityPM.PaymentOrderLines.forEach(function (itemLine) {
                _this.LinesList.Insert(itemLine);
            });
        }
        this.BuildPaymentOrderMethods();
        if (this.EntityPM.PaymentOrderProtestReasons) {
            this.EntityPM.PaymentOrderProtestReasons.forEach(function (itemLine) {
                if (itemLine.AmountInDispute != null) {
                    var amount = itemLine.AmountInDispute;
                    _this.AmountInDisputeTotal = _this.AmountInDisputeTotal + Number(amount);
                }
                _this.ProtestsList.Insert(itemLine);
            });
            if ((this.ProtestsList.Length * 27) + 27 < 123) {
                this.FooterProtests = (this.ProtestsList.Length * 27) + 27;
            }
            else {
                this.FooterProtests = 123;
            }
        }
        this.BuildAccountingSelectionGroupFilterList();
        this.BuildAccountingCustomFilesList();
        this.DisplayOnlyCheck();
    };
    PaymentOrdersGeneralTabComponent.prototype.InitTab = function (entityPM, /*parent: EditPaymentOrderComponent,*/ isDisplayOnly) {
        this.EntityPM = entityPM;
        this.InitPaymentOrderScreen();
        //this.Parent = parent;
        //this.IsDisplayOnly = isDisplayOnly;
    };
    PaymentOrdersGeneralTabComponent.prototype.BuildPaymentOrderMethods = function () {
        this.MethodsList = new ObservableCollection_1.ObservableCollection([]);
        this.MethodItemsAmountTotal = 0;
        if (this.EntityPM.PaymentOrderMethods) {
            for (var _i = 0, _a = this.EntityPM.PaymentOrderMethods; _i < _a.length; _i++) {
                var item = _a[_i];
                if (item.Amount != null) {
                    var amount = item.Amount;
                    this.MethodItemsAmountTotal = this.MethodItemsAmountTotal + Number(amount);
                }
                this.MethodsList.Insert(new PaymentMethodModel(item, this));
                if ((this.MethodsList.Length * 27) + 27 < 123) {
                    this.FooterMethods = (this.MethodsList.Length * 27) + 27;
                }
                else {
                    this.FooterMethods = 123;
                }
            }
        }
    };
    PaymentOrdersGeneralTabComponent.prototype.DisplayOnlyCheck = function () {
        if (this.EntityPM.IsClosed) {
            this.IsControlEnabled = false;
            this.IsErrorMessageVisibility = true;
            this.SetScreenFieldsEditability();
        }
        else {
            this.IsControlEnabled = true;
            this.IsErrorMessageVisibility = false;
        }
    };
    PaymentOrdersGeneralTabComponent.prototype.SetScreenFieldsEditability = function () {
        this.UIProperties.SetEnabled("AccountingCustomFile", null, false);
        this.UIProperties.SetEnabled("ImporterId", "Customs.PaymentOrder", false);
        this.UIProperties.SetEnabled("PaymentOrderTypeCode", "Customs.PaymentOrder", false);
        this.UIProperties.SetEnabled("CustomerActivityTypeCode", "Customs.PaymentOrder", false);
        this.UIProperties.SetEnabled("TotalSumToPay", "Customs.PaymentOrder", false);
        this.UIProperties.SetEnabled("PaymentProcessCode", "Customs.PaymentOrder", false);
        this.UIProperties.SetEnabled("PaymentOrderLeftAmount", "Customs.PaymentOrder", false);
        this.UIProperties.SetEnabled("CustomerId", "Customs.PaymentOrder", false);
    };
    Object.defineProperty(PaymentOrdersGeneralTabComponent.prototype, "SelectedTab", {
        get: function () { return this.selectedTab; },
        set: function (tab) {
            this.selectedTab = tab;
        },
        enumerable: true,
        configurable: true
    });
    PaymentOrdersGeneralTabComponent.prototype.SetTabArgs = function (args, valdationErrorList) {
        if (valdationErrorList === void 0) { valdationErrorList = null; }
        this.EntityPM = args.EntityPM;
        console.log("EntityPM", this.EntityPM);
    };
    Object.defineProperty(PaymentOrdersGeneralTabComponent.prototype, "PaymentOrderTypeCode", {
        get: function () { return this.EntityPM.PaymentOrderTypeCode; },
        set: function (newValue) { this.EntityPM.PaymentOrderTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrdersGeneralTabComponent.prototype, "CustomerActivityTypeCode", {
        get: function () { return this.EntityPM.CustomerActivityTypeCode; },
        set: function (newValue) { this.EntityPM.CustomerActivityTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrdersGeneralTabComponent.prototype, "ImporterId", {
        get: function () { return this.EntityPM.ImporterId; },
        set: function (newValue) { this.EntityPM.ImporterId = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrdersGeneralTabComponent.prototype, "TotalSumToPay", {
        get: function () { return this.EntityPM.TotalSumToPay; },
        set: function (newValue) { this.EntityPM.TotalSumToPay = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrdersGeneralTabComponent.prototype, "PaymentProcessCode", {
        get: function () { return this.EntityPM.PaymentProcessCode; },
        set: function (newValue) { this.EntityPM.PaymentProcessCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrdersGeneralTabComponent.prototype, "PaymentOrderSelectedLabel", {
        get: function () { return this.EntityPM.PaymentOrderSelectedLabel; },
        set: function (newValue) { this.EntityPM.PaymentOrderSelectedLabel = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrdersGeneralTabComponent.prototype, "PaymentOrderLeftAmount", {
        get: function () { return this.EntityPM.PaymentOrderLeftAmount; },
        set: function (newValue) { this.EntityPM.PaymentOrderLeftAmount = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrdersGeneralTabComponent.prototype, "AccountingCustomFile", {
        get: function () { return this.EntityPM.AccountingCustomFile; },
        set: function (newValue) { this.EntityPM.AccountingCustomFile = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrdersGeneralTabComponent.prototype, "Reason", {
        get: function () { return this.EntityPM.Reason; },
        set: function (newValue) { this.EntityPM.Reason = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrdersGeneralTabComponent.prototype, "InternalNotes", {
        get: function () { return this.EntityPM.InternalNotes; },
        set: function (newValue) { this.EntityPM.InternalNotes = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrdersGeneralTabComponent.prototype, "CustomerId", {
        get: function () { return this.EntityPM.CustomerId; },
        set: function (newValue) {
            this.EntityPM.CustomerId = newValue;
            this.EntityPM.CustomerChanged = true;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrdersGeneralTabComponent.prototype, "IsControlEnabled", {
        get: function () { return this.isControlEnabled; },
        set: function (newValue) { this.isControlEnabled = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrdersGeneralTabComponent.prototype, "IsErrorMessageVisibility", {
        get: function () { return this.isErrorMessageVisibility; },
        set: function (newValue) { this.isErrorMessageVisibility = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrdersGeneralTabComponent.prototype, "IsByAccountingCustomFile", {
        get: function () { return this.isByAccountingCustomFile; },
        set: function (newValue) { this.isByAccountingCustomFile = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrdersGeneralTabComponent.prototype, "IsByPaymentOrderAccCard", {
        get: function () { return this.isByPaymentOrderAccCard; },
        set: function (newValue) { this.isByPaymentOrderAccCard = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrdersGeneralTabComponent.prototype, "ShowMethodsColumnFooters", {
        //public get PaymentOrderAccCard() { return this.paymentOrderAccCard; }
        //public set PaymentOrderAccCard(newValue: string) { this.paymentOrderAccCard = newValue; }
        get: function () { return this.showMethodsColumnFooters; },
        set: function (newValue) { this.showMethodsColumnFooters = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentOrdersGeneralTabComponent.prototype, "ShowProtestsColumnFooters", {
        get: function () { return this.showProtestsColumnFooters; },
        set: function (newValue) { this.showProtestsColumnFooters = newValue; },
        enumerable: true,
        configurable: true
    });
    PaymentOrdersGeneralTabComponent.prototype.CheckAndCalcDeclrationByAccountingCustomFile = function () {
        if (this.PaymentOrderSelectedLabel == "PaymentOrderAccCard") {
            return (true);
        }
        if (Tools_1.AppTool.IsNullOrEmpty(this.AccountingCustomFile)) {
            this.MessageAccountingCardWindow(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.PaymentOrder.O.AccountingCustomFileMissing"));
            return (false);
        }
        if (this.customFilesList.length > 0) {
            if (this.customFilesList.indexOf(this.AccountingCustomFile) < 0) {
                this.MessageAccountingCardWindow("יש לבחור תיק עמילות מהרשימה!");
                return (false);
            }
        }
    };
    //#region AccountingCustomFile
    PaymentOrdersGeneralTabComponent.prototype.BuildAccountingSelectionGroupFilterList = function () {
        this.AccountingSelectionFilterList = [];
        this.isFromBuildAccounting = true;
        var myAccountingCustomFileItem = new CodeNameClass();
        myAccountingCustomFileItem.Code = "0"; // "AccountingCustomFile"
        myAccountingCustomFileItem.Name = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.PaymentOrder.F.AccountingCustomFile");
        this.AccountingSelectionFilterList.push(myAccountingCustomFileItem);
        var myPaymentOrderAccCardItem = new CodeNameClass();
        myPaymentOrderAccCardItem.Code = "1"; // "PaymentOrderAccCard"
        myPaymentOrderAccCardItem.Name = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomsSetting.F.PaymentOrderAccCard");
        this.AccountingSelectionFilterList.push(myPaymentOrderAccCardItem);
        if (this.PaymentOrderSelectedLabel == "PaymentOrderAccCard") {
            this.SelectedAccountingFilter = myPaymentOrderAccCardItem;
        }
        else { // AccountingCustomFile
            this.SelectedAccountingFilter = myAccountingCustomFileItem;
        }
        this.isFromBuildAccounting = false;
    };
    Object.defineProperty(PaymentOrdersGeneralTabComponent.prototype, "SelectedAccountingFilter", {
        get: function () {
            return this.selectedAccountingFilter;
        },
        set: function (newValue) {
            var _this = this;
            if (this.selectedAccountingFilter != newValue) {
                this.selectedAccountingFilter = newValue;
            }
            this.UIProperties.SetEnabled("AccountingCustomFile", this.ObjectTableName, true);
            if (newValue.Code == "1") {
                this.PaymentOrderSelectedLabel = "PaymentOrderAccCard";
                this.customsSettingListService.getSingleFromCache(SessionLocator_1.SessionLocator.Tenant.toString())
                    .subscribe(function (customsSettingList) {
                    if (customsSettingList != null) {
                        if (!Tools_1.AppTool.IsNullOrEmpty(customsSettingList.Result)) {
                            _this.AccountingCustomFile = customsSettingList.Result.PaymentOrderAccCard;
                            _this.UIProperties.SetEnabled("AccountingCustomFile", _this.ObjectTableName, false);
                        }
                        else {
                            _this.MessageAccountingCardWindow(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.PaymentOrder.O.NoAccountingCard"));
                        }
                    }
                });
            }
            else if (this.isFromBuildAccounting == false) {
                this.PaymentOrderSelectedLabel = "AccountingCustomFile";
                this.AccountingCustomFile = "";
                if (this.customFilesList != null && this.customFilesList.length == 1) {
                    this.AccountingCustomFile = this.customFilesList[0];
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    PaymentOrdersGeneralTabComponent.prototype.AccountingCustomFileLostFocusMethod = function (item) {
        var _this = this;
        if (this.PaymentOrderSelectedLabel == "PaymentOrderAccCard") {
            return (true);
        }
        var errorMessage = "";
        this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
        if (Tools_1.AppTool.IsNullOrEmpty(this.AccountingCustomFile)) {
            return;
        }
        if (this.customFilesList != null && this.customFilesList.length > 0) {
            if (!(this.customFilesList.indexOf(this.AccountingCustomFile) > -1)) {
                errorMessage = "יש לבחור תיק עמילות מהרשימה!";
                this.MessageAccountingCardWindow(errorMessage);
                this.CurrentSession.CurrentEditComponent.ValidationErrorsList.push(errorMessage);
                return;
            }
        }
        else {
            this.declarationExtendedListService.GetDeclarationByCustomFileNoAndCCU(this.AccountingCustomFile)
                .subscribe(function (myResponse) {
                if (myResponse.Result == null || (myResponse.Result != null && Tools_1.AppTool.IsNullOrEmpty(myResponse.Result.Id))) {
                    errorMessage = TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.Declaration.O.Didntfindcustomfile");
                    _this.MessageAccountingCardWindow(errorMessage);
                    _this.CurrentSession.CurrentEditComponent.ValidationErrorsList.push(errorMessage);
                    _this.AccountingCustomFile = "";
                    return;
                }
                else {
                    _this.PaymentOrderSelectedLabel = "AccountingCustomFile";
                }
            });
        }
    };
    PaymentOrdersGeneralTabComponent.prototype.MessageAccountingCardWindow = function (message) {
        var messageWindow = new MessageWindow_1.MessageWindow();
        messageWindow.Width = 300;
        messageWindow.Height = 150;
        messageWindow.Show(message);
    };
    PaymentOrdersGeneralTabComponent.prototype.OpenCustomFilesScreen = function () {
        var _this = this;
        if (!this.IsControlEnabled) {
            return;
        }
        var logitudeWindow = new LogitudeWindow_1.LogitudeWindow();
        logitudeWindow.Width = 300;
        logitudeWindow.Height = 400;
        logitudeWindow.IsShowCloseButton = true;
        logitudeWindow.Title = "תיק עמילות לחיוב";
        logitudeWindow.WindowArgs = this.customFilesList;
        logitudeWindow.WindowClosed.subscribe(function ($event) { return _this.OnCustomFilesScreenWindowClosed($event); });
        logitudeWindow.Show('./CustomsModules/CustomsPaymentOrder/Components/EditTabs/General/AccountingCustomFilesComponent');
    };
    PaymentOrdersGeneralTabComponent.prototype.OnCustomFilesScreenWindowClosed = function (arg) {
        if (!Tools_1.AppTool.IsNullOrEmpty(arg)) {
            this.AccountingCustomFile = arg;
            this.IsByAccountingCustomFile = true;
        }
    };
    PaymentOrdersGeneralTabComponent.prototype.BuildAccountingCustomFilesList = function () {
        var _this = this;
        this.paymentOrderConnectionTableExtendedPMService.GetAccountingCustomFileNumbers(this.EntityPM.Id, SessionLocator_1.SessionLocator.Tenant)
            .subscribe(function (myResponse) {
            _this.CurrentSession.StopBusyIndicator();
            _this.BuildAccountingCustomFilesListOp_Completed(myResponse, false);
        });
    };
    PaymentOrdersGeneralTabComponent.prototype.BuildAccountingCustomFilesListOp_Completed = function (myResponse, sourceIsCostomFile) {
        var _this = this;
        this.customFilesList = [];
        if (myResponse.Result != null) {
            myResponse.Result.forEach(function (item) {
                _this.customFilesList.push(item);
            });
        }
        if (this.EntityPM != null) {
            if (Tools_1.AppTool.IsNullOrEmpty(this.AccountingCustomFile) && this.customFilesList.length == 1) {
                //LoadOperation getFileOp = declarationContext.Load(declarationContext.GetSingleDeclarationByCustomFileNoQuery(entityPM.AccountingCustomFile, TenantContext.Current.Id), LoadBehavior.RefreshCurrent, true);
                //getFileOp.Completed += AccountingCustomFileLostFocus_Completed;
                this.AccountingCustomFile = this.customFilesList[0];
            }
        }
    };
    Object.defineProperty(PaymentOrdersGeneralTabComponent.prototype, "MethodItemsAmountTotal", {
        get: function () { return this.methodItemsAmountTotal; },
        set: function (newValue) {
            if (this.methodItemsAmountTotal != newValue) {
                this.methodItemsAmountTotal = newValue;
            }
        },
        enumerable: true,
        configurable: true
    });
    PaymentOrdersGeneralTabComponent.prototype.CalcMethodItemsAmountTotal = function () {
        var _this = this;
        this.MethodItemsAmountTotal = 0;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.MethodsList)) {
            this.MethodsList.Collection.forEach(function (method) {
                if (method.Amount != null) {
                    var amount = method.Amount;
                    _this.MethodItemsAmountTotal = _this.MethodItemsAmountTotal + Number(amount);
                }
            });
        }
    };
    PaymentOrdersGeneralTabComponent.prototype.DeleteMethodsList = function (item) {
        if (!this.IsControlEnabled)
            return;
        if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
            this.MethodsList.Remove(item);
            if ((this.MethodsList.Length * 27) + 27 < 123) {
                this.FooterMethods = (this.MethodsList.Length * 27) + 27;
            }
            else {
                this.FooterMethods = 123;
            }
            if (item.Amount != null) {
                var amount = item.Amount;
                this.MethodItemsAmountTotal = this.MethodItemsAmountTotal - Number(amount);
            }
            this.EntityPM.RemovePaymentOrderMethod(item.methodPM);
        }
    };
    PaymentOrdersGeneralTabComponent.prototype.AddPaymentOrderMethod = function () {
        if (!this.IsControlEnabled)
            return;
        var newPaymentOrderMethodPM = new PaymentOrderMethodPM_1.PaymentOrderMethodPM(this.EntityPM);
        newPaymentOrderMethodPM.PaymentOrderId = this.EntityPM.Id;
        newPaymentOrderMethodPM.Tenant = this.EntityPM.Tenant;
        newPaymentOrderMethodPM.Line = (Tools_1.ArrayTool.Max(this.EntityPM.PaymentOrderMethods, "Line") + 1);
        this.EntityPM.AddPaymentOrderMethod(newPaymentOrderMethodPM);
        var itemModel = new PaymentMethodModel(newPaymentOrderMethodPM, this);
        this.MethodsList.Insert(itemModel);
        if ((this.MethodsList.Length * 27) + 27 < 123) {
            this.FooterMethods = (this.MethodsList.Length * 27) + 27;
        }
        else {
            this.FooterMethods = 123;
        }
        this.ShowMethodsColumnFooters = true;
    };
    PaymentOrdersGeneralTabComponent.prototype.CalcProtestItemsTotal = function (event) {
        var _this = this;
        var total = 0;
        this.AmountInDisputeTotal = 0;
        if (!Tools_1.AppTool.IsNullOrEmpty(this.ProtestsList)) {
            this.ProtestsList.Collection.forEach(function (item) {
                if (item.AmountInDispute != null) {
                    total = item.AmountInDispute;
                    _this.AmountInDisputeTotal = _this.AmountInDisputeTotal + Number(total);
                }
            });
        }
    };
    PaymentOrdersGeneralTabComponent.prototype.DeleteProtestsList = function (item) {
        if (!this.IsControlEnabled)
            return;
        if (!Tools_1.AppTool.IsNullOrEmpty(item)) {
            this.ProtestsList.Remove(item);
            this.EntityPM.RemovePaymentOrderProtestReason(item);
            if (item.AmountInDispute != null) {
                var amount = item.AmountInDispute;
                this.AmountInDisputeTotal = this.AmountInDisputeTotal - Number(amount);
            }
            if ((this.ProtestsList.Length * 27) + 27 < 123) {
                this.FooterProtests = (this.ProtestsList.Length * 27) + 27;
            }
            else {
                this.FooterProtests = 123;
            }
        }
    };
    PaymentOrdersGeneralTabComponent.prototype.AddPaymentOrderProtestReason = function () {
        if (!this.IsControlEnabled)
            return;
        var newPaymentOrderProtestReasonPM = new PaymentOrderProtestReasonPM_1.PaymentOrderProtestReasonPM(this.EntityPM);
        newPaymentOrderProtestReasonPM.PaymentOrderId = this.EntityPM.Id;
        newPaymentOrderProtestReasonPM.Tenant = this.EntityPM.Tenant;
        newPaymentOrderProtestReasonPM.Line = (Tools_1.ArrayTool.Max(this.EntityPM.PaymentOrderProtestReasons, "Line") + 1);
        this.ProtestsList.Insert(newPaymentOrderProtestReasonPM);
        this.EntityPM.AddPaymentOrderProtestReason(newPaymentOrderProtestReasonPM);
        if ((this.ProtestsList.Length * 27) + 27 < 123) {
            this.FooterProtests = (this.ProtestsList.Length * 27) + 27;
        }
        else {
            this.FooterProtests = 123;
        }
        this.ShowProtestsColumnFooters = true;
    };
    PaymentOrdersGeneralTabComponent.prototype.SetProtestTypeLocalName = function (item, lookupEntity) {
        if (!Tools_1.AppTool.IsNullOrEmpty(lookupEntity)) {
            item.ProtestTypeName = lookupEntity.LocalName;
        }
    };
    PaymentOrdersGeneralTabComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './PaymentOrdersGeneralTabComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs, EntityResourceService_1.EntityResourceService])
    ], PaymentOrdersGeneralTabComponent);
    return PaymentOrdersGeneralTabComponent;
}(BaseComponent_1.BaseComponent));
exports.PaymentOrdersGeneralTabComponent = PaymentOrdersGeneralTabComponent;
var CodeNameClass = /** @class */ (function () {
    function CodeNameClass() {
    }
    return CodeNameClass;
}());
var PaymentMethodModel = /** @class */ (function (_super) {
    __extends(PaymentMethodModel, _super);
    function PaymentMethodModel(methodPM, parent) {
        var _this = _super.call(this) || this;
        _this.methodPM = methodPM;
        _this.parent = parent;
        _this.ObjectTableName = "Customs.PaymentOrderMethod";
        _this.DataContext = _this;
        _this.BanksList = [];
        _this.AgentBanks = [];
        _this.customBankListService = new CustomBankListService_1.CustomBankListService();
        _this.customBankCardExtendedPMService = new CustomBankCardExtendedPMService_1.CustomBankCardExtendedPMService();
        if (methodPM.TypeCode == "1") {
            _this.BanksList = [];
            _this.AgentBanks = [];
            _this.LoadBanks();
        }
        _this.UIProperties.SetEnabled("PaymentMethodStatusCode", _this.ObjectTableName, false);
        _this.UIProperties.SetEnabled("PaymentMethodStatusName", _this.ObjectTableName, false);
        return _this;
    }
    PaymentMethodModel.prototype.CalcAmountDifference = function (logCellTemplate) {
        var amount = 0;
        if (this.parent.MethodsList != null) {
            this.parent.MethodsList.Collection.forEach(function (method) {
                if (method.Amount != null) {
                    amount = amount + Number(method.Amount);
                }
            });
        }
        if (this.parent.TotalSumToPay > amount) {
            this.Amount = this.parent.TotalSumToPay - amount;
        }
    };
    PaymentMethodModel.prototype.LoadBanks = function () {
        var _this = this;
        this.parent.declarationWebService.GetCustomBanksForCard(this.parent.EntityPM.CustomerId).subscribe(function (response) {
            var result = response.Result;
            console.log("[Response] GetCustomBanksForCard: ", result);
            if (!Tools_1.AppTool.IsNullOrEmpty(result)) {
                _this.BanksList = result;
                _this.parent.customsSettingListService.getAll().subscribe(function (response) {
                    var list = response.Result;
                    if (!Tools_1.AppTool.IsNullOrEmpty(list)) {
                        var customsSetting = list[0];
                        //if (this.BanksList.length == 0) {
                        //    this.customBankListService.getAllFromCache().subscribe((response: ServiceResponse) => {
                        //        if (response) {
                        //            if (!response.HasError) {
                        //                this.agentBanks = response.Result.filter(d => d.PayerTypeCode == "3" && !d.InActive);
                        //                if (this.agentBanks.length == 1) {
                        //                    this.InternalBankId = this.agentBanks[0].Id;
                        //                    this.SelectedBank = this.agentBanks[0];
                        //                    this.BanksList = this.agentBanks;
                        //                }
                        //            }
                        //        }
                        //    });
                        //}
                        //else {
                        if (customsSetting != null) {
                            if (customsSetting.BlockAgentBankForMasab) {
                                if (_this.BanksList.length > 0) {
                                    if (_this.BanksList.length == 1) {
                                        if (_this.InternalBankId == null) {
                                            _this.InternalBankId = _this.BanksList[0].Id;
                                        }
                                        var bank = _this.BanksList.filter(function (d) { return d.Id == _this.InternalBankId; })[0];
                                        _this.SelectedBank = bank;
                                    }
                                    else {
                                        if (_this.InternalBankId != null) {
                                            var bank = _this.BanksList.filter(function (d) { return d.Id == _this.InternalBankId; })[0];
                                            _this.SelectedBank = bank;
                                        }
                                    }
                                }
                            }
                            else {
                                if (_this.BanksList.length > 0) {
                                    if (_this.BanksList.length == 1) {
                                        if (_this.InternalBankId == null) {
                                            _this.InternalBankId = _this.BanksList[0].Id;
                                        }
                                        _this.customBankListService.getAllFromCache().subscribe(function (response) {
                                            if (response) {
                                                if (!response.HasError) {
                                                    _this.BanksList = response.Result.filter(function (d) { return !d.InActive; });
                                                    if (_this.InternalBankId != null) {
                                                        var bank = _this.BanksList.filter(function (d) { return d.Id == _this.InternalBankId; })[0];
                                                        _this.SelectedBank = bank;
                                                    }
                                                }
                                            }
                                        });
                                    }
                                    else {
                                        _this.customBankListService.getAllFromCache().subscribe(function (response) {
                                            if (response) {
                                                if (!response.HasError) {
                                                    _this.BanksList = response.Result.filter(function (d) { return !d.InActive; });
                                                    if (_this.InternalBankId != null) {
                                                        var bank = _this.BanksList.filter(function (d) { return d.Id == _this.InternalBankId; })[0];
                                                        _this.SelectedBank = bank;
                                                    }
                                                }
                                            }
                                        });
                                    }
                                }
                                else if (_this.BanksList.length == 0) {
                                    _this.customBankListService.getAllFromCache().subscribe(function (response) {
                                        if (response) {
                                            if (!response.HasError) {
                                                _this.AgentBanks = response.Result.filter(function (d) { return d.PayerTypeCode == "3" && !d.InActive; });
                                                if (_this.AgentBanks.length > 0) {
                                                    _this.BanksList = _this.AgentBanks;
                                                    if (_this.AgentBanks.length == 1) {
                                                        if (_this.InternalBankId == null) {
                                                            _this.InternalBankId = _this.BanksList[0].Id;
                                                        }
                                                    }
                                                    else {
                                                        if (_this.InternalBankId != null) {
                                                            var bank = _this.BanksList.filter(function (d) { return d.Id == _this.InternalBankId; })[0];
                                                            _this.SelectedBank = bank;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    });
                                }
                            }
                        }
                        //}
                    }
                });
            }
        });
    };
    Object.defineProperty(PaymentMethodModel.prototype, "SelectedBank", {
        get: function () { return this.selectedBank; },
        set: function (value) {
            if (this.selectedBank != value) {
                this.selectedBank = value;
                if (!Tools_1.AppTool.IsNullOrEmpty(value)) {
                    this.InternalBankId = value.Id;
                    this.InternalBankName = value.LocalName;
                    if (Tools_1.AppTool.IsNullOrEmpty(value.LocalName)) {
                        this.InternalBankName = value.EnglishName;
                    }
                }
                else {
                    this.InternalBankId = null;
                    this.InternalBankName = null;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentMethodModel.prototype, "TypeCode", {
        get: function () {
            return this.methodPM.TypeCode;
        },
        set: function (value) {
            if (this.methodPM.TypeCode != value) {
                this.methodPM.TypeCode = value;
                if (value == "1") {
                    this.BanksList = [];
                    this.AgentBanks = [];
                    this.LoadBanks();
                }
                else {
                    this.InternalBankId = null;
                    this.CustomerActivityTypeCode = null;
                    this.CustomerActivityTypeName = null;
                    this.BanksList = [];
                    this.AgentBanks = [];
                    this.SelectedBank = null;
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentMethodModel.prototype, "TypeName", {
        get: function () { return this.methodPM.TypeName; },
        set: function (value) {
            if (this.methodPM.TypeName != value) {
                this.methodPM.TypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentMethodModel.prototype, "Amount", {
        get: function () { return this.methodPM.Amount; },
        set: function (value) {
            if (this.methodPM.Amount != value) {
                this.methodPM.Amount = value;
                this.parent.CalcMethodItemsAmountTotal();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentMethodModel.prototype, "BankCode", {
        get: function () { return this.methodPM.BankCode; },
        set: function (value) {
            if (this.methodPM.BankCode != value) {
                this.methodPM.BankCode = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentMethodModel.prototype, "InternalBankId", {
        get: function () { return this.methodPM.InternalBankId; },
        set: function (value) {
            var _this = this;
            if (this.methodPM.InternalBankId != value) {
                this.methodPM.InternalBankId = value;
                this.customBankListService.getAllFromCache().subscribe(function (response) {
                    if (response) {
                        if (!response.HasError) {
                            var customBank = response.Result.filter(function (d) { return d.Id == value; })[0];
                            if (customBank == null && _this.BanksList != null) {
                                customBank = _this.BanksList.filter(function (d) { return d.Id == value; })[0];
                            }
                            if (customBank != null) {
                                _this.InternalBankName = customBank.LocalName != null ? customBank.LocalName : customBank.BankName;
                                if (customBank.PayerTypeCode == "0") {
                                    _this.customBankCardExtendedPMService.GetSingleCustomBanksCard(value, _this.parent.EntityPM.CustomerId).subscribe(function (response) {
                                        if (response) {
                                            if (!response.HasError) {
                                                var bankCard = response.Result;
                                                if (bankCard != null) {
                                                    _this.methodPM.BankCode = customBank.BankCode;
                                                    _this.methodPM.BranchCode = customBank.BranchCode;
                                                    _this.methodPM.CustomsBranchId = customBank.CustomsBranchId;
                                                    _this.methodPM.AccountNumber = customBank.AccountNumber;
                                                    _this.CustomerActivityTypeCode = customBank.PayerTypeCode;
                                                    _this.CustomerActivityTypeName = customBank.PayerTypeName;
                                                }
                                                else {
                                                    var messageWindow = new MessageWindow_1.MessageWindow();
                                                    messageWindow.Show(TextCodeTranslator_1.TextCodeTranslator.Translate("Customs.CustomBank.O.BankNotConnectedToCustomer"));
                                                    _this.InternalBankId = null;
                                                    _this.methodPM.InternalBankId = null;
                                                    _this.InternalBankName = null;
                                                    _this.SelectedBank = null;
                                                }
                                            }
                                        }
                                    });
                                }
                                else {
                                    _this.methodPM.BankCode = customBank.BankCode;
                                    _this.methodPM.BranchCode = customBank.BranchCode;
                                    _this.methodPM.CustomsBranchId = customBank.CustomsBranchId;
                                    _this.methodPM.AccountNumber = customBank.AccountNumber;
                                    _this.CustomerActivityTypeCode = customBank.PayerTypeCode;
                                    _this.CustomerActivityTypeName = customBank.PayerTypeName;
                                }
                            }
                            if (customBank == null) {
                                _this.methodPM.BankCode = null;
                                _this.methodPM.BranchCode = null;
                                _this.methodPM.CustomsBranchId = null;
                                _this.methodPM.AccountNumber = null;
                                _this.CustomerActivityTypeCode = null;
                            }
                        }
                    }
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentMethodModel.prototype, "InternalBankName", {
        get: function () { return this.internalBankName; },
        set: function (value) {
            if (this.internalBankName != value) {
                this.internalBankName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentMethodModel.prototype, "CustomerActivityTypeCode", {
        get: function () { return this.methodPM.CustomerActivityTypeCode; },
        set: function (newValue) { this.methodPM.CustomerActivityTypeCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentMethodModel.prototype, "CustomerActivityTypeName", {
        get: function () { return this.methodPM.CustomerActivityTypeName; },
        set: function (value) {
            if (this.methodPM.CustomerActivityTypeName != value) {
                this.methodPM.CustomerActivityTypeName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentMethodModel.prototype, "PaymentMethodStatusCode", {
        get: function () { return this.methodPM.PaymentMethodStatusCode; },
        set: function (newValue) { this.methodPM.PaymentMethodStatusCode = newValue; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(PaymentMethodModel.prototype, "PaymentMethodStatusName", {
        get: function () { return this.methodPM.PaymentMethodStatusName; },
        set: function (value) {
            if (this.methodPM.PaymentMethodStatusName != value) {
                this.methodPM.PaymentMethodStatusName = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    //#endregion
    PaymentMethodModel.prototype.SetLocalName = function (entity, fieldName) {
        if (!Tools_1.AppTool.IsNullOrEmpty(entity)) {
            this[fieldName] = entity.LocalName;
        }
        else {
            this[fieldName] = null;
        }
    };
    return PaymentMethodModel;
}(BaseComponent_1.BaseComponent));
exports.PaymentMethodModel = PaymentMethodModel;
//# sourceMappingURL=PaymentOrdersGeneralTabComponent.js.map